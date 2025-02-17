using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Data.SqlClient;
using Dapper;

namespace SM_One
{
    public class Global
    {
        #region fields

        //Listes de références des formulaires

        protected static List<string> FormUids = new List<string>();
        public static string NumberDecimalSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        public static string _thousandSeparator = string.Empty;
        public static string _decimalSeparator = string.Empty;
        public static SAPbobsCOM.UserQueries oUserQuery;
        //Objets COM SAP
        protected static SboGuiApi myGuiApi;
        public static Application myApi;
        public static SAPbouiCOM.Company Comp;
        public static SAPbobsCOM.Company Comp_DI;
        private static Recordset RecCheck;
        public static Dictionary<string, string> userTables = new Dictionary<string, string>();
        public static Dictionary<string, string> OccReportForms = new Dictionary<string, string>();
        public static bool IsFormOpen;
        public static string HCMDb;
        public static string DBUser;
        public static string DBPass;
        public static string HCMServer;
        public static SAPbouiCOM.SelectedRows SelectedEmployees;
        public static string ItemCode = "";
        public int CompanyFontSize;
        public int UserFontSize;



        //Gestion des conditions       
        public static Conditions conds;
        public static Condition cond;
        public static SAPbouiCOM.Conditions NewConds
        {
            get { return (Conditions)myApi.CreateObject(BoCreatableObjectType.cot_Conditions); }

        }

        //Gestion des menus
        protected static MenuItem monMenuItem;
        protected static MenuCreationParams creationMenu;

        public static string CurrentDirectory = "";
        public static string _LastForm = "";
        public static string _LastType = "";
        public static string _Sep = "";
        public static string _dateFormat = "";
        public static string _dateSeparator = "";

        #endregion

        #region connexions and start

        public static bool TestSqlConnection(string connectionString)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var result = connection.ExecuteScalar("SELECT 1");
                    return result != null && Convert.ToInt32(result) == 1;
                }
            }
            catch (Exception ex)
            {
                Global.SetMessage($"Connection failed: {ex.Message}", BoStatusBarMessageType.smt_Error);
                return false;
            }
        }
        public static void ConnectUI()
        {

            myApi = SAPbouiCOM.Framework.Application.SBO_Application;
            Comp = myApi.Company;
        }
        public static void SendMessage(string Subject, string Text, string[] MsgRecipients, List<List<string>> ObjectToSend, string DocType, string DocColName)
        {
            CompanyService oCmpSrv = null;
            Message oMessage = null;
            MessagesService oMessageService = null;
            try
            {
                oCmpSrv = Comp_DI.GetCompanyService();
                oMessageService = (SAPbobsCOM.MessagesService)oCmpSrv.GetBusinessService(ServiceTypes.MessagesService);
                // get the data interface for the new message
                oMessage = (SAPbobsCOM.Message)oMessageService.GetDataInterface(MessagesServiceDataInterfaces.msdiMessage);
                //oMessages = Comp_DI.GetBusinessObject(BoObjectTypes.oMessages) as SAPbobsCOM.Messages;
                // fill subject
                oMessage.Subject = Subject;

                oMessage.Text = Text;

                // Add Recipient 
                for (int i = 0; i < MsgRecipients.Length; i++)
                {
                    oMessage.RecipientCollection.Add();
                    oMessage.RecipientCollection.Item(i).UserCode = MsgRecipients[i];
                    oMessage.RecipientCollection.Item(i).SendInternal = BoYesNoEnum.tYES;
                }
                if (ObjectToSend.Count > 0)
                {
                    oMessage.MessageDataColumns.Add();
                    oMessage.MessageDataColumns.Item(0).ColumnName = DocColName;

                }
                int k = 0;
                foreach (List<string> li in ObjectToSend)
                {
                    oMessage.MessageDataColumns.Item(0).Link = BoYesNoEnum.tYES;
                    oMessage.MessageDataColumns.Item(0).MessageDataLines.Add();
                    oMessage.MessageDataColumns.Item(0).MessageDataLines.Item(k).Value = li[1];
                    oMessage.MessageDataColumns.Item(0).MessageDataLines.Item(k).Object = li[0].Split(':')[0];
                    oMessage.MessageDataColumns.Item(0).MessageDataLines.Item(k).ObjectKey = li[0].Split(':')[1];
                    k++;
                }
                k = 0;
                oMessageService.SendMessage(oMessage);

            }
            catch (Exception ex)
            {
                Global.SetMessage(ex.Message, BoStatusBarMessageType.smt_Error);
            }
            finally
            {
                oCmpSrv = null;
                oMessage = null;
                oMessageService = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
        public static void Start()
        {

            CurrentDirectory = Environment.CurrentDirectory;
            ConnectUI();
            MAJ_filter();
            //  Global.myApi.StatusBar.SetText("Connecting to the data server...", BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Warning);
            Init_DI();
            if (!Comp_DI.Connected)
                throw new Exception("Cannot connect to the server, check network security settings\n" + Comp_DI.GetLastErrorDescription());
            initUserTables();
            _Sep = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            
            myApi.MetadataAutoRefresh = false;
            InitiateDataObjects();
            CompanyService oCmpService = Comp_DI.GetCompanyService();
            AdminInfo oAdminInfo = oCmpService.GetAdminInfo();
            _thousandSeparator = oAdminInfo.ThousandsSeparator.ToString();
            _decimalSeparator = oAdminInfo.DecimalSeparator;

            SetDateFormat();
            myApi.Forms.GetForm("169", 1).Freeze(true);
            myApi.Forms.GetForm("169", 1).Freeze(false);
            myApi.MetadataAutoRefresh = true;
            myApi.Forms.GetForm("169",1).Update();
            Global.SetMessage("SM_One connected", BoStatusBarMessageType.smt_Success);

            
       
        }

        public static string GetTableID(string TableName)
        {
            RecCheck = Comp_DI.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
            if (Global.Comp_DI.DbServerType == BoDataServerTypes.dst_HANADB)
                RecCheck.DoQuery("Select LPAD(\"TblNum\",2,'0') as \"TblNum\" from OUTB where \"TableName\" ='" + TableName + "'");
            else
                RecCheck.DoQuery("Select FORMAT(\"TblNum\",'00') as \"TblNum\" from OUTB where \"TableName\" ='" + TableName + "'");
            return "110" + RecCheck.Fields.Item("TblNum").Value.ToString();

        }

        private static void SetDateFormat()
        {
            string format = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            _dateFormat = format;
            _dateSeparator = CultureInfo.CurrentCulture.DateTimeFormat.DateSeparator;

            if (format == "M/d/yyyy")
            {
                _dateFormat = "MM/dd/yyyy";
            }
            if (format == "d/M/yyyy")
            {
                _dateFormat = "dd/MM/yyyy";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private static void InitiateDataObjects()
        {
            #region menus
            AddMenu(myApi, "BL_SM1", "Student Management", "43520", -1, BoMenuType.mt_POPUP, "student.bmp");
            AddMenu(myApi, "BL_SM9", "Setup", "BL_SM1", 1, BoMenuType.mt_POPUP, "");
            AddMenu(myApi, "BL_SM2", "Configuration", "BL_SM9", 1, BoMenuType.mt_STRING, "");
            AddMenu(myApi, "BL_SM5", "Semester Setup", "BL_SM9", 2, BoMenuType.mt_STRING, "");
            AddMenu(myApi, "BL_SM8", "Revenue Mapping", "BL_SM9", 3, BoMenuType.mt_STRING, "");
            AddMenu(myApi, "BL_SM3", "Scholarship Setup", "BL_SM9", 4, BoMenuType.mt_STRING, "");

            AddMenu(myApi, "BL_SM11", "ERP Integration", "BL_SM9", 5, BoMenuType.mt_STRING, "");
            AddMenu(myApi, "BL_SM4", "Student Import Wizard", "BL_SM1", 2, BoMenuType.mt_STRING, "");
            AddMenu(myApi, "BL_SM6", "Student Registration", "BL_SM1", 3, BoMenuType.mt_STRING, "");
            AddMenu(myApi, "BL_SM7", "Student Invoices", "BL_SM1", 4, BoMenuType.mt_STRING, "");
            AddMenu(myApi, "BL_SM10", "Student Refunds", "BL_SM1", 4, BoMenuType.mt_STRING, "");

            #endregion


           
            //return;
            #region UserDefinedTables
            CheckAndCreateTable("CONF", "Configuration", BoUTBTableType.bott_MasterData);
            CheckAndCreateTable("ONF1", "Configuration Lines", BoUTBTableType.bott_MasterDataLines);
            CheckAndCreateTable("ONF2", "Configuration Lines 2", BoUTBTableType.bott_MasterDataLines);
            CheckAndCreateTable("OSEM", "Semester Setup", BoUTBTableType.bott_NoObject);
            CheckAndCreateTable("OCOL", "College Setup", BoUTBTableType.bott_NoObject);
            CheckAndCreateTable("OSHL", "Scholarship Setup", BoUTBTableType.bott_NoObject);
            CheckAndCreateTable("ORMP", "Revenue Mapping", BoUTBTableType.bott_NoObject);
            CheckAndCreateTable("OSRG", "Student Registration", BoUTBTableType.bott_Document);
            CheckAndCreateTable("SRG1", "Student Registration Rows", BoUTBTableType.bott_DocumentLines);
            CheckAndCreateTable("OING", "Invoice Gen. Header", BoUTBTableType.bott_Document);
            CheckAndCreateTable("ING1", "Invoice Gen. Rows", BoUTBTableType.bott_DocumentLines);
            CheckAndCreateTable("TING1", "Invoice Gen. Rows Temporary", BoUTBTableType.bott_DocumentLines);

            #endregion




            //return;

            //Standard UDFs//
            #region StandardUDFS
            List<List<string>> validValsSts = new List<List<string>>();
            List<string> tables1 = new List<string>();
            List<string> udoFormCols1 = new List<string>();
            List<string> ColRech1 = new List<string>();
            List<string> enhancedudoFormCols1 = new List<string>();

            CheckAndCreateDefaultUserField("OCRD", "Semester", "Semester", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "OSEM");
            CheckAndCreateDefaultUserField("OCRD", "CurScholar", "Current Scholarship", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "OSHL");
            CheckAndCreateDefaultUserField("OCRD", "AdmScholar", "Admission Scholarship", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "OSHL");
            CheckAndCreateDefaultUserField("OCRD", "College", "College", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "OCOL");
            CheckAndCreateDefaultUserField("OCRD", "Major", "Major", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 8, "");
            CheckAndCreateDefaultUserField("OCRD", "JoiningDate", "Joining Date", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("OCRD", "CGPA", "CGPA", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 11, "");
            CheckAndCreateDefaultUserField("OCRD", "SchrCHours", "Scholarship Hours", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 11, "");
            CheckAndCreateDefaultUserField("OCRD", "AttmptCHours", "Attempted Hours", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 11, "");

            CheckAndCreateDefaultUserField("OITM", "ID", "Trio ID", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("OITM", "PassMarks", "Passing Marks", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("OITM", "Credits", "Credit Hours", BoFieldTypes.db_Float, BoFldSubTypes.st_Measurement, 11, "");
            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("Y"); validValsSts[0].Add("Yes");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("N"); validValsSts[1].Add("No");
            CheckAndCreateAlphaNumUserFieldWithValidValues("OITM", "Scholarship", "Scholarship", BoFieldTypes.db_Alpha, 1, validValsSts, false, 0);

            CheckAndCreateDefaultUserField("OPRC", "TrioID", "Trio ID", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");
            CheckAndCreateDefaultUserField("OPRC", "TrioName", "Trio Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");
            CheckAndCreateDefaultUserField("OPRC", "GPA", "Min GPA", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 11, "");

            CheckAndCreateAttachmentField("OADM", "FilePath", "File Path", BoFldSubTypes.st_Link);
            CheckAndCreateDefaultUserFieldWithLinkedObject("JDT1", "InvEntry", "Invoice Key", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, UDFLinkedSystemObjectTypesEnum.ulInvoices);
            CheckAndCreateDefaultUserFieldWithLinkedObject("JDT1", "MemoEntry", "Memo Key", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, UDFLinkedSystemObjectTypesEnum.ulCreditNotes);


            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("N"); validValsSts[0].Add("No");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("Y"); validValsSts[1].Add("Yes");
            CheckAndCreateAlphaNumUserFieldWithValidValues("OINV", "SendInvoice", "Send Invoice", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);
            CheckAndCreateAlphaNumUserFieldWithValidValues("INV3", "SchDiscount", "Scholarship Discount", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);
            CheckAndCreateDefaultUserField("OINV", "RegNo", "Registration No.", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("OINV", "GenNo", "Generation No.", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("OINV", "GenLine", "Generation Line", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("OINV", "Major", "Major", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            CheckAndCreateDefaultUserField("OINV", "Scholarship", "Scholarship", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "OSHL");
            CheckAndCreateDefaultUserField("OINV", "College", "College", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "OCOL");
            CheckAndCreateDefaultUserField("OINV", "Semester", "Semester", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "OSEM");
            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("N"); validValsSts[0].Add("Normal");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("Y"); validValsSts[1].Add("Repeat");
            CheckAndCreateAlphaNumUserFieldWithValidValues("OINV", "RegType", "Registration Type", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);
            CheckAndCreateAlphaNumUserFieldWithValidValues("OJDT", "RegType", "Registration Type", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);



            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("Active"); validValsSts[0].Add("Active");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("Approved"); validValsSts[1].Add("Approved");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("CompleteWithdrawal"); validValsSts[2].Add("Complete Withdrawal");
            validValsSts.Add(new List<string>()); validValsSts[3].Add("Expelled"); validValsSts[3].Add("Expelled");
            validValsSts.Add(new List<string>()); validValsSts[4].Add("Graduated"); validValsSts[4].Add("Graduated");
            validValsSts.Add(new List<string>()); validValsSts[5].Add("InActive"); validValsSts[5].Add("InActive");
            validValsSts.Add(new List<string>()); validValsSts[6].Add("Registered"); validValsSts[6].Add("Registered");
            validValsSts.Add(new List<string>()); validValsSts[7].Add("Rejected"); validValsSts[7].Add("Rejected");
            validValsSts.Add(new List<string>()); validValsSts[8].Add("Suspended"); validValsSts[8].Add("Suspended");

            CheckAndCreateAlphaNumUserFieldWithValidValues("OINV", "Status", "Status", BoFieldTypes.db_Alpha, 50, validValsSts, true, 0);
            CheckAndCreateAlphaNumUserFieldWithValidValues("OCRD", "Status", "Status", BoFieldTypes.db_Alpha, 50, validValsSts, true, 0);

            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("-"); validValsSts[0].Add("-");
            CheckAndCreateAlphaNumUserFieldWithValidValues("INV1", "College", "College", BoFieldTypes.db_Alpha, 50, validValsSts, true, 0);
            CheckAndCreateAlphaNumUserFieldWithValidValues("INV1", "Scholarship", "Scholarship", BoFieldTypes.db_Alpha, 50, validValsSts, true, 0);


            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("N"); validValsSts[0].Add("No");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("Y"); validValsSts[1].Add("Yes");
            CheckAndCreateAlphaNumUserFieldWithValidValues("OJDT", "StVoucher", "Student Voucher", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);
            CheckAndCreateAlphaNumUserFieldWithValidValues("OJDT", "Reversal", "Reversal Voucher", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);

            #endregion
            //Configuration Data


            CheckAndCreateDefaultUserField("@CONF", "VSeries", "Voucher Series", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@CONF", "InvSeries", "Invoice Series", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@CONF", "ItemSeries", "Item Series", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, "");

            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("N"); validValsSts[0].Add("N");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("Y"); validValsSts[1].Add("Y");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@CONF", "AllowWOTrail", "Allow Without Trail", BoFieldTypes.db_Alpha, 1, validValsSts, false, 0);
            CheckAndCreateAlphaNumUserFieldWithValidValues("@CONF", "FiscalNotCal", "Fiscal Year Runs Jan-Dec", BoFieldTypes.db_Alpha, 1, validValsSts, true, 1);
            CheckAndCreateAlphaNumUserFieldWithValidValues("@CONF", "UpdatePL", "UpdatePriceList", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);
            CheckAndCreateAlphaNumUserFieldWithValidValues("@CONF", "SendInvoice", "SendInvoice", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);



            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("D"); validValsSts[0].Add("Days");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("M"); validValsSts[1].Add("Months");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@CONF", "ProratedOn", "Prorated Calculation on", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);

            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("J"); validValsSts[0].Add("Joining Year");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("F"); validValsSts[1].Add("Fiscal Year");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@CONF", "PricedOn", "Priced On", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);

            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("0"); validValsSts[0].Add("0");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("1"); validValsSts[1].Add("1");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("-1"); validValsSts[2].Add("-1");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@CONF", "Sem1Start", "Semester 1 Start Add", BoFieldTypes.db_Numeric, 2, validValsSts, true, 0);
            CheckAndCreateAlphaNumUserFieldWithValidValues("@CONF", "Sem1End", "Semester 1 End Add", BoFieldTypes.db_Numeric, 2, validValsSts, true, 0);
            CheckAndCreateAlphaNumUserFieldWithValidValues("@CONF", "Sem2Start", "Semester 2 Start Add", BoFieldTypes.db_Numeric, 2, validValsSts, true, 0);
            CheckAndCreateAlphaNumUserFieldWithValidValues("@CONF", "Sem2End", "Semester 2 End Add", BoFieldTypes.db_Numeric,2, validValsSts, true, 0);
            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("C"); validValsSts[0].Add("Configuration");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("R"); validValsSts[1].Add("Revenue Mapping");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@CONF", "AccountFrom", "Account From", BoFieldTypes.db_Alpha, 1, validValsSts, false, 1);
            CheckAndCreateDefaultUserField("@CONF", "VDebit", "Debit Account", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 20, "");
            CheckAndCreateDefaultUserField("@CONF", "VCredit", "Credit Account", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 20, "");
            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("N"); validValsSts[0].Add("N");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("Y"); validValsSts[1].Add("Y");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@CONF", "AllowCancel", "Allow Cancellation", BoFieldTypes.db_Alpha, 1, validValsSts, false, 0);
            CheckAndCreateAlphaNumUserFieldWithValidValues("@CONF", "AllowRefund", "Allow Refund", BoFieldTypes.db_Alpha, 1, validValsSts, false, 0);
            CheckAndCreateAlphaNumUserFieldWithValidValues("@CONF", "AllowNRefund", "Allow Non Refund", BoFieldTypes.db_Alpha, 1, validValsSts, false, 0);
            CheckAndCreateDefaultUserField("@CONF", "ProgramDim", "Program Dimension", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1, "");
            CheckAndCreateDefaultUserField("@CONF", "ScholarDim", "Scholarship Dimension", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1, "");
            CheckAndCreateDefaultUserField("@CONF", "RepScholar", "Repeat Scholarship", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            CheckAndCreateDefaultUserField("@CONF", "CollegeDim", "College Dimension", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1, "");
            CheckAndCreateDefaultUserField("@CONF", "TaxGroup", "Tax Group", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 8, "");
            CheckAndCreateDefaultUserField("@CONF", "GroupCode", "BP Group Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 20, "");
            CheckAndCreateDefaultUserField("@CONF", "GroupName", "BP Group Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");
            CheckAndCreateDefaultUserField("@CONF", "SQLServer", "ERP Server Address", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");
            CheckAndCreateDefaultUserField("@CONF", "SQLDatabase", "ERP Server Db", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");
            CheckAndCreateDefaultUserField("@CONF", "SQLUsername", "ERP Server Username", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");
            CheckAndCreateDefaultUserField("@CONF", "SQLPassword", "ERP Server Password", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");


            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("Active"); validValsSts[0].Add("Active");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("Approved"); validValsSts[1].Add("Approved");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("CompleteWithdrawal"); validValsSts[2].Add("Complete Withdrawal");
            validValsSts.Add(new List<string>()); validValsSts[3].Add("Expelled"); validValsSts[3].Add("Expelled");
            validValsSts.Add(new List<string>()); validValsSts[4].Add("Graduated"); validValsSts[4].Add("Graduated");
            validValsSts.Add(new List<string>()); validValsSts[5].Add("InActive"); validValsSts[5].Add("InActive");
            validValsSts.Add(new List<string>()); validValsSts[6].Add("Registered"); validValsSts[6].Add("Registered");
            validValsSts.Add(new List<string>()); validValsSts[7].Add("Rejected"); validValsSts[7].Add("Rejected");
            validValsSts.Add(new List<string>()); validValsSts[8].Add("Suspended"); validValsSts[8].Add("Suspended");

            CheckAndCreateAlphaNumUserFieldWithValidValues("@ONF1", "Status", "Status", BoFieldTypes.db_Alpha, 50, validValsSts, true, 0);
            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("N"); validValsSts[0].Add("No");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("Y"); validValsSts[1].Add("Yes");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@ONF1", "Program", "Inc. Program", BoFieldTypes.db_Alpha, 1, validValsSts, false, 0);
            CheckAndCreateAlphaNumUserFieldWithValidValues("@ONF1", "Courses", "Inc. Courses", BoFieldTypes.db_Alpha, 1, validValsSts, false, 0);
            CheckAndCreateDefaultUserField("@ONF2", "Freight", "FreightCode", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@ONF2", "Enabled", "Enabled", BoFieldTypes.db_Alpha, 1, validValsSts, false, 0);
            CheckAndCreateAlphaNumUserFieldWithValidValues("@ONF2", "IsDiscount", "For Discount", BoFieldTypes.db_Alpha, 1, validValsSts, false, 0);
            CheckAndCreateDefaultUserField("@ONF2", "DefaultValue", "Default Value", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");


            tables1.Add("ONF1");
            tables1.Add("ONF2");
            CheckAndCreateUDO("ONF", "SMConfig", BoUDOObjType.boud_MasterData, "CONF", BoYesNoEnum.tNO, BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tNO, BoYesNoEnum.tNO, ColRech1, udoFormCols1, tables1);
            InsertRecords("CONF", "CONF", "ONF", "@CONF");




            //Semester Setup//
            CheckAndCreateDefaultUserField("@OSEM", "Description", "Description", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");
            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("Fall"); validValsSts[0].Add("Fall");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("Spring"); validValsSts[1].Add("Spring");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("Summer"); validValsSts[2].Add("Summer");
            validValsSts.Add(new List<string>()); validValsSts[3].Add("-"); validValsSts[3].Add("-");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@OSEM", "SemesterType", "Semester Type", BoFieldTypes.db_Alpha, 10, validValsSts, false, 3);
            CheckAndCreateDefaultUserField("@OSEM", "StartDate", "Semester Start", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@OSEM", "EndDate", "Semester End", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@OSEM", "Sequence", "Semester Sequence", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");

            CheckAndCreateDefaultUserField("@OCOL", "ID", "Trio ID", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@OCOL", "Description", "Description", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            CheckAndCreateDefaultUserField("@OSHL", "SqlID", "Trio ID", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@OSHL", "PriceList", "Price List", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 32, "");
            CheckAndCreateDefaultUserField("@OSHL", "GPA", "Minimum GPA", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 11, "");

            CheckAndCreateDefaultUserField("@ORMP", "College", "College", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "OCOL");
            CheckAndCreateDefaultUserField("@ORMP", "Scholarship", "Scholarship", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "OSHL");
            CheckAndCreateDefaultUserFieldWithLinkedObject("@ORMP", "Debit", "Debit Account", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 25, UDFLinkedSystemObjectTypesEnum.ulChartOfAccounts);
            CheckAndCreateDefaultUserFieldWithLinkedObject("@ORMP", "Credit", "Credit Account", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 25, UDFLinkedSystemObjectTypesEnum.ulChartOfAccounts);
            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("R"); validValsSts[0].Add("Invoice");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("C"); validValsSts[1].Add("Memo");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@ORMP", "MapType", "Mapping For", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);
            CheckAndCreateDefaultUserFieldWithLinkedObject("@ORMP", "DiscountGL", "Discount Account", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 25, UDFLinkedSystemObjectTypesEnum.ulChartOfAccounts);
            CheckAndCreateDefaultUserField("@ORMP", "DiscountPC", "Discount Percent", BoFieldTypes.db_Float, BoFldSubTypes.st_Percentage, 2, "");

            CheckAndCreateDefaultUserField("@OSRG", "StudentCode", "Student ID", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            CheckAndCreateDefaultUserField("@OSRG", "StudentName", "Student Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");
            CheckAndCreateDefaultUserField("@OSRG", "Major", "Major", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            CheckAndCreateDefaultUserField("@OSRG", "StartDate", "Start Date", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@OSRG", "EndDate", "End Date", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@OSRG", "TrgtEntry", "Invoice Entry", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@OSRG", "TrgtNum", "Invoice Number", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@OSRG", "RTrgtEntry", "Rep. Invoice Entry", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@OSRG", "RTrgtNum", "Rep. Invoice Number", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");

            CheckAndCreateDefaultUserField("@OSRG", "College", "College", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            CheckAndCreateDefaultUserField("@OSRG", "Semester", "Semester", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            CheckAndCreateDefaultUserField("@OSRG", "Scholarship", "Current Scholarship", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            CheckAndCreateDefaultUserField("@OSRG", "FiscalYear", "Registration Year", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@OSRG", "AcademicYear", "Academic Year", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, "");

            


            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("N"); validValsSts[0].Add("N");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("Y"); validValsSts[1].Add("Y");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@OSRG", "Invoiced", "Invoiced", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);
            CheckAndCreateAlphaNumUserFieldWithValidValues("@OSRG", "RInvoiced", "Repeat Invoiced", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);
            CheckAndCreateAlphaNumUserFieldWithValidValues("@OSRG", "FeeCreated", "FeeCreated", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);
            CheckAndCreateAlphaNumUserFieldWithValidValues("@OSRG", "HasRepeat", "Has Repeats", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);
            CheckAndCreateDefaultUserField("@OSRG", "GrossDocTotal", "Gross Document Total", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@OSRG", "SchDiscount", "Scholarship Discount Amount", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@OSRG", "SchDiscountPC", "Scholarship Discount Percent", BoFieldTypes.db_Float, BoFldSubTypes.st_Percentage, 11, "");
            CheckAndCreateDefaultUserField("@OSRG", "AfterSchDisc", "After Scholarship Discount", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@OSRG", "Discount", "Discount Amount", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@OSRG", "DiscountPC", "Discount Percent", BoFieldTypes.db_Float, BoFldSubTypes.st_Percentage, 11, "");
            CheckAndCreateDefaultUserField("@OSRG", "AfterDiscount", "After Header Discount", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@OSRG", "TaxPC", "Tax Percent", BoFieldTypes.db_Float, BoFldSubTypes.st_Percentage, 11, "");
            CheckAndCreateDefaultUserField("@OSRG", "TaxAmount", "Tax Amount", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@OSRG", "DocTotal", "After Scholarship Discount", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@OSRG", "Applied", "Applied Amount", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@OSRG", "Remaining", "Remaining Amount", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@OSRG", "RevApplied", "Rev.Applied Amount", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@OSRG", "RevRemaining", "Rev.Remaining Amount", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");







            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("O"); validValsSts[0].Add("Open");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("C"); validValsSts[1].Add("Closed");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@OSRG", "Status", "Status", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);

            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("Active"); validValsSts[0].Add("Active");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("Approved"); validValsSts[1].Add("Approved");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("CompleteWithdrawal"); validValsSts[2].Add("Complete Withdrawal");
            validValsSts.Add(new List<string>()); validValsSts[3].Add("Expelled"); validValsSts[3].Add("Expelled");
            validValsSts.Add(new List<string>()); validValsSts[4].Add("Graduated"); validValsSts[4].Add("Graduated");
            validValsSts.Add(new List<string>()); validValsSts[5].Add("InActive"); validValsSts[5].Add("InActive");
            validValsSts.Add(new List<string>()); validValsSts[6].Add("Registered"); validValsSts[6].Add("Registered");
            validValsSts.Add(new List<string>()); validValsSts[7].Add("Rejected"); validValsSts[7].Add("Rejected");
            validValsSts.Add(new List<string>()); validValsSts[8].Add("Suspended"); validValsSts[8].Add("Suspended");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@OSRG", "RegStatus", "Registration Status", BoFieldTypes.db_Alpha, 50, validValsSts, true, 0);

            CheckAndCreateDefaultUserField("@SRG1", "SubjectCode", "Subject Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            CheckAndCreateDefaultUserField("@SRG1", "SubjectName", "Subject Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 200, "");
            CheckAndCreateDefaultUserField("@SRG1", "Credits", "Credit Hours", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 10, "");
            CheckAndCreateDefaultUserField("@SRG1", "Price", "Price", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@SRG1", "Discount", "Discount Amount", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@SRG1", "DiscountPC", "Discount Percent", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
     
            CheckAndCreateDefaultUserField("@SRG1", "Total", "Total", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("A"); validValsSts[0].Add("Auto");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("M"); validValsSts[1].Add("Manual");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@SRG1", "LineType", "Line Type", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);

            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("N"); validValsSts[0].Add("N");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("Y"); validValsSts[1].Add("Y");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@SRG1", "Repeat", "Repeat", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);
            CheckAndCreateDefaultUserField("@SRG1", "RepeatCourse", "Repeat Course", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            
            tables1.Clear();
            ColRech1.Clear();
            udoFormCols1.Clear();
            tables1.Add("SRG1");
            ColRech1.Add("DocEntry");
            ColRech1.Add("U_StudentCode");
            ColRech1.Add("U_StudentName");
            ColRech1.Add("U_Major");
            ColRech1.Add("U_College");
            ColRech1.Add("U_Semester");
            ColRech1.Add("U_StartDate");
            ColRech1.Add("U_EndDate");
            ColRech1.Add("U_TrgtEntry");
            ColRech1.Add("U_TrgtNum");
            CheckAndCreateUDO("SRG", "Registration", BoUDOObjType.boud_Document, "OSRG", BoYesNoEnum.tYES, BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tNO, BoYesNoEnum.tNO, ColRech1, udoFormCols1, tables1);
            
            CheckAndCreateDefaultUserField("@OING", "FromDate", "FromDate", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, "", BoYesNoEnum.tYES);
            CheckAndCreateDefaultUserField("@OING", "ToDate", "ToDate", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, "", BoYesNoEnum.tYES);
            CheckAndCreateDefaultUserField("@OING", "DocDate", "Doc. Date", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, "", BoYesNoEnum.tYES);
            CheckAndCreateDefaultUserField("@OING", "DueDate", "Due Date", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, "", BoYesNoEnum.tYES);
            CheckAndCreateDefaultUserField("@OING", "Major", "Program Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "", BoYesNoEnum.tNO);
            CheckAndCreateDefaultUserField("@OING", "College", "College", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "", BoYesNoEnum.tNO);
            CheckAndCreateDefaultUserField("@OING", "Student", "Student ID", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            CheckAndCreateDefaultUserField("@OING", "Semester", "Semester", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "OSEM");
            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("R"); validValsSts[0].Add("Registration");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("C"); validValsSts[1].Add("Cancellation");

            CheckAndCreateAlphaNumUserFieldWithValidValues("@OING", "DocType", "Doc. Type", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);

            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("-"); validValsSts[0].Add(" ");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("R"); validValsSts[1].Add("Refund");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("N"); validValsSts[2].Add("Non-Refund");
            validValsSts.Add(new List<string>()); validValsSts[3].Add("C"); validValsSts[3].Add("Cancel");
            validValsSts.Add(new List<string>()); validValsSts[4].Add("M"); validValsSts[4].Add("Manual");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@OING", "CancelType", "Cancellation Type", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);

            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("Active"); validValsSts[0].Add("Active");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("Approved"); validValsSts[1].Add("Approved");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("CompleteWithdrawal"); validValsSts[2].Add("Complete Withdrawal");
            validValsSts.Add(new List<string>()); validValsSts[3].Add("Expelled"); validValsSts[3].Add("Expelled");
            validValsSts.Add(new List<string>()); validValsSts[4].Add("Graduated"); validValsSts[4].Add("Graduated");
            validValsSts.Add(new List<string>()); validValsSts[5].Add("InActive"); validValsSts[5].Add("InActive");
            validValsSts.Add(new List<string>()); validValsSts[6].Add("Registered"); validValsSts[6].Add("Registered");
            validValsSts.Add(new List<string>()); validValsSts[7].Add("Rejected"); validValsSts[7].Add("Rejected");
            validValsSts.Add(new List<string>()); validValsSts[8].Add("Suspended"); validValsSts[8].Add("Suspended");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@OING", "Status", "Registration Status", BoFieldTypes.db_Alpha, 50, validValsSts, true, 0);

            CheckAndCreateDefaultUserField("@OING", "College", "College", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "OCOL");
            CheckAndCreateDefaultUserField("@OING", "Scholarship", "Scholarship", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "OSHL");

            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("Y"); validValsSts[0].Add("Y");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("N"); validValsSts[1].Add("N");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@ING1", "Select", "Select", BoFieldTypes.db_Alpha, 10, validValsSts, false, 0);
            CheckAndCreateAlphaNumUserFieldWithValidValues("@ING1", "Cancelled", "Cancelled", BoFieldTypes.db_Alpha, 1, validValsSts, true, 1);
            CheckAndCreateAlphaNumUserFieldWithValidValues("@ING1", "HasRepeat", "Has Repeat", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);

            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("-"); validValsSts[0].Add(" ");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("R"); validValsSts[1].Add("Refund");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("N"); validValsSts[2].Add("Non-Refund");
            validValsSts.Add(new List<string>()); validValsSts[3].Add("C"); validValsSts[3].Add("Cancel");
            validValsSts.Add(new List<string>()); validValsSts[4].Add("M"); validValsSts[4].Add("Manual");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@ING1", "CancelType", "Cancellation Type", BoFieldTypes.db_Alpha, 1, validValsSts, false, 0);

            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("N"); validValsSts[0].Add("N");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("Y"); validValsSts[1].Add("Y");

            CheckAndCreateAlphaNumUserFieldWithValidValues("@ING1", "Invoiced", "Invoiced", BoFieldTypes.db_Alpha, 10, validValsSts, false, 0);
            CheckAndCreateAlphaNumUserFieldWithValidValues("@ING1", "DocCancel", "Document Cancelled", BoFieldTypes.db_Alpha, 1, validValsSts, false, 0);
            CheckAndCreateAlphaNumUserFieldWithValidValues("@ING1", "IsVoucher", "Voucher Created", BoFieldTypes.db_Alpha, 10, validValsSts, false, 0);

            CheckAndCreateDefaultUserField("@ING1", "StudentCode", "Student Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            CheckAndCreateDefaultUserField("@ING1", "StudentName", "Student Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");
            CheckAndCreateDefaultUserField("@ING1", "Major", "Major", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            CheckAndCreateDefaultUserField("@ING1", "Scholarship", "Scholarship", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");

            CheckAndCreateDefaultUserField("@ING1", "FiscalYear", "Fiscal Year", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@ING1", "AcademicYear", "Academic Year", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@ING1", "College", "College", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "OCOL");

            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("Active"); validValsSts[0].Add("Active");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("Approved"); validValsSts[1].Add("Approved");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("CompleteWithdrawal"); validValsSts[2].Add("Complete Withdrawal");
            validValsSts.Add(new List<string>()); validValsSts[3].Add("Expelled"); validValsSts[3].Add("Expelled");
            validValsSts.Add(new List<string>()); validValsSts[4].Add("Graduated"); validValsSts[4].Add("Graduated");
            validValsSts.Add(new List<string>()); validValsSts[5].Add("InActive"); validValsSts[5].Add("InActive");
            validValsSts.Add(new List<string>()); validValsSts[6].Add("Registered"); validValsSts[6].Add("Registered");
            validValsSts.Add(new List<string>()); validValsSts[7].Add("Rejected"); validValsSts[7].Add("Rejected");
            validValsSts.Add(new List<string>()); validValsSts[8].Add("Suspended"); validValsSts[8].Add("Suspended");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@ING1", "Status", "Registration Status", BoFieldTypes.db_Alpha, 50, validValsSts, true, 0);

         

            CheckAndCreateDefaultUserField("@ING1", "HDiscount", "Reg. Discount", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "DiscountPC", "Discount Percent", BoFieldTypes.db_Float, BoFldSubTypes.st_Percentage, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "SchDiscount", "Scholarship Discount Amount", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "SchDiscountPC", "Scholarship Discount Percent", BoFieldTypes.db_Float, BoFldSubTypes.st_Percentage, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "AfterSchDisc", "After Scholarship Discount", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "LDiscount", "Course Discount", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "TaxCode", "Tax Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@ING1", "TaxAmount", "Tax Amount", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "TaxRate", "Tax Rate", BoFieldTypes.db_Float, BoFldSubTypes.st_Percentage, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "RegNo", "Register No.", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "Remarks", "Notes", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 250, "");
            CheckAndCreateDefaultUserField("@ING1", "InvEntry", "Invoice Entry", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "InvNum", "Invoice No.", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "RInvEntry", "Repeat Invoice Entry", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "RInvNum", "Repeat Invoice No.", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "DocTotal", "Document Total", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "PaidSum", "Paid Amount", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "OpenSum", "Open Amount", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "RegAmount", "Registration Amount", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "TotalBefTax", "Document Total Bef. Tax", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "AfterDiscount", "After Discount", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "MemoEntry", "Memo Entry", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "MemoNum", "Memo No.", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");

            CheckAndCreateDefaultUserField("@ING1", "BaseLine", "Base Line", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "BaseDoc", "Base Doc", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "CancelEntry", "Cancel Entry", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "CancelNum", "Cancel No.", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "AcctCode", "Account Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 20, "");
            CheckAndCreateDefaultUserField("@ING1", "DiscountGL", "Scholarship Discount G/L", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 20, "");
            CheckAndCreateDefaultUserField("@ING1", "ManualAmount", "Manual Amount", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "StartDate", "Start Date", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@ING1", "EndDate", "To Date", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@ING1", "Remarks", "Remarks", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 250, "");

            CheckAndCreateDefaultUserField("@ING1", "Freight1", " Freight 1", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "Freight2", " Freight 2", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "Freight3", " Freight 3", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "Freight4", " Freight 4", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "Freight5", " Freight 5", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "Freight6", " Freight 6", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "Freight7", " Freight 7", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "Freight8", " Freight 8", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "Freight9", " Freight 9", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "Freight10", " Freight 10", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "TotalFreight", "Total Freight", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");

            tables1.Clear();
            ColRech1.Clear();
            udoFormCols1.Clear();
            tables1.Add("ING1");
            ColRech1.Add("DocEntry");
            ColRech1.Add("CreateDate");
            ColRech1.Add("U_FromDate");
            ColRech1.Add("U_ToDate");
            ColRech1.Add("U_Major");

            udoFormCols1.Add("DocEntry");
            udoFormCols1.Add("CreateDate");
            udoFormCols1.Add("U_FromDate");
            udoFormCols1.Add("U_ToDate");
            udoFormCols1.Add("U_Major");
            CheckAndCreateUDO("ING", "Student_Invoices", BoUDOObjType.boud_Document, "OING", BoYesNoEnum.tYES, BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tNO, BoYesNoEnum.tNO, ColRech1, udoFormCols1, tables1);

            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("Y"); validValsSts[0].Add("Y");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("N"); validValsSts[1].Add("N");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@TING1", "Select", "Select", BoFieldTypes.db_Alpha, 10, validValsSts, false, 0);
            CheckAndCreateAlphaNumUserFieldWithValidValues("@TING1", "Cancelled", "Cancelled", BoFieldTypes.db_Alpha, 1, validValsSts, true, 1);
            CheckAndCreateAlphaNumUserFieldWithValidValues("@TING1", "HasRepeat", "Has Repeat", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);

            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("-"); validValsSts[0].Add(" ");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("R"); validValsSts[1].Add("Refund");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("N"); validValsSts[2].Add("Non-Refund");
            validValsSts.Add(new List<string>()); validValsSts[3].Add("C"); validValsSts[3].Add("Cancel");
            validValsSts.Add(new List<string>()); validValsSts[4].Add("M"); validValsSts[4].Add("Manual");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@TING1", "CancelType", "Cancellation Type", BoFieldTypes.db_Alpha, 1, validValsSts, false, 0);

            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("N"); validValsSts[0].Add("N");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("Y"); validValsSts[1].Add("Y");

            CheckAndCreateAlphaNumUserFieldWithValidValues("@TING1", "Invoiced", "Invoiced", BoFieldTypes.db_Alpha, 10, validValsSts, false, 0);
            CheckAndCreateAlphaNumUserFieldWithValidValues("@TING1", "DocCancel", "Document Cancelled", BoFieldTypes.db_Alpha, 1, validValsSts, false, 0);
            CheckAndCreateAlphaNumUserFieldWithValidValues("@TING1", "IsVoucher", "Voucher Created", BoFieldTypes.db_Alpha, 10, validValsSts, false, 0);

            CheckAndCreateDefaultUserField("@TING1", "StudentCode", "Student Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            CheckAndCreateDefaultUserField("@TING1", "StudentName", "Student Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");
            CheckAndCreateDefaultUserField("@TING1", "Major", "Major", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            CheckAndCreateDefaultUserField("@TING1", "Scholarship", "Scholarship", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            CheckAndCreateDefaultUserField("@TING1", "FiscalYear", "Fiscal Year", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@TING1", "AcademicYear", "Academic Year", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@TING1", "College", "College", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "OCOL");

            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("Active"); validValsSts[0].Add("Active");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("Approved"); validValsSts[1].Add("Approved");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("CompleteWithdrawal"); validValsSts[2].Add("Complete Withdrawal");
            validValsSts.Add(new List<string>()); validValsSts[3].Add("Expelled"); validValsSts[3].Add("Expelled");
            validValsSts.Add(new List<string>()); validValsSts[4].Add("Graduated"); validValsSts[4].Add("Graduated");
            validValsSts.Add(new List<string>()); validValsSts[5].Add("InActive"); validValsSts[5].Add("InActive");
            validValsSts.Add(new List<string>()); validValsSts[6].Add("Registered"); validValsSts[6].Add("Registered");
            validValsSts.Add(new List<string>()); validValsSts[7].Add("Rejected"); validValsSts[7].Add("Rejected");
            validValsSts.Add(new List<string>()); validValsSts[8].Add("Suspended"); validValsSts[8].Add("Suspended");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@TING1", "Status", "Registration Status", BoFieldTypes.db_Alpha, 50, validValsSts, true, 0);
       

            CheckAndCreateDefaultUserField("@TING1", "HDiscount", "Reg. Discount", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "DiscountPC", "Discount Percent", BoFieldTypes.db_Float, BoFldSubTypes.st_Percentage, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "SchDiscount", "Scholarship Discount Amount", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "SchDiscountPC", "Scholarship Discount Percent", BoFieldTypes.db_Float, BoFldSubTypes.st_Percentage, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "AfterSchDisc", "After Scholarship Discount", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "LDiscount", "Course Discount", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "TaxCode", "Tax Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@TING1", "TaxAmount", "Tax Amount", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "TaxRate", "Tax Rate", BoFieldTypes.db_Float, BoFldSubTypes.st_Percentage, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "RegNo", "Register No.", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "Remarks", "Notes", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 250, "");
            CheckAndCreateDefaultUserField("@TING1", "InvEntry", "Invoice Entry", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "InvNum", "Invoice No.", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "RInvEntry", "Repeat Invoice Entry", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "RInvNum", "Repeat Invoice No.", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "DocTotal", "Document Total", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "PaidSum", "Paid Amount", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "OpenSum", "Open Amount", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "RegAmount", "Registration Amount", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "TotalBefTax", "Document Total Bef. Tax", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "AfterDiscount", "After Discount", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "MemoEntry", "Memo Entry", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "MemoNum", "Memo No.", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");

            CheckAndCreateDefaultUserField("@TING1", "BaseLine", "Base Line", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "BaseDoc", "Base Doc", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "CancelEntry", "Cancel Entry", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "CancelNum", "Cancel No.", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "AcctCode", "Account Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 20, "");
            CheckAndCreateDefaultUserField("@TING1", "DiscountGL", "Scholarship Discount G/L", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 20, "");
            CheckAndCreateDefaultUserField("@TING1", "ManualAmount", "Manual Amount", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "StartDate", "Start Date", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@TING1", "EndDate", "To Date", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@TING1", "Remarks", "Remarks", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 250, "");

            CheckAndCreateDefaultUserField("@TING1", "Freight1", " Freight 1", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "Freight2", " Freight 2", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "Freight3", " Freight 3", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "Freight4", " Freight 4", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "Freight5", " Freight 5", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "Freight6", " Freight 6", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "Freight7", " Freight 7", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "Freight8", " Freight 8", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "Freight9", " Freight 9", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "Freight10", " Freight 10", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "TotalFreight", "Total Freight", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");

            if (Comp_DI.DbServerType == BoDataServerTypes.dst_HANADB)
            {
                CreateUserQuery("CreateRevCode", "Select CONCAT($[@ORMP.U_MapType],'-',$[@ORMP.U_College],'-',$[@ORMP.U_Scholarship])  from dummy", -1);
            }
            else
            {
                CreateUserQuery("CreateRevCode", "Select CONCAT($[@ORMP.U_MapType],'-',$[@ORMP.U_College],'-',$[@ORMP.U_Scholarship])", -1);
            }
            CreateFormattedSearch(GetTableID("ORMP"), "3", "Code", BoFormattedSearchActionEnum.bofsaQuery, GetQueryID("CreateRevCode", -1), BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tNO, new string[] { "U_College", "U_Scholarship" ,"U_MapType"});
            CreateFormattedSearch(GetTableID("ORMP"), "3", "Name", BoFormattedSearchActionEnum.bofsaQuery, GetQueryID("CreateRevCode", -1), BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tNO, new string[] { "U_College", "U_Scholarship", "U_MapType" });

           
            CreateUserQuery("GetPriceList", "Select DISTINCT \"ListName\" from OPLN", -1);
            CreateFormattedSearch(GetTableID("OSHL"), "3", "U_PriceList", BoFormattedSearchActionEnum.bofsaQuery, GetQueryID("GetPriceList", -1), BoYesNoEnum.tNO, BoYesNoEnum.tNO, BoYesNoEnum.tNO, null);

            Config.LoadConfig();
           
        }

        public static void CreateUserQuery(string name, string query, int CatID)
        {
            int nbres = 0;

            Global.QueryFirstValueRec("SELECT \"IntrnalKey\"  FROM OUQR Where \"QName\" = '" + name + "' ", true, out nbres);
            if (nbres == 0)
            {
                GC.WaitForPendingFinalizers();
                GC.Collect();
                oUserQuery = (SAPbobsCOM.UserQueries)Comp_DI.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserQueries);
                oUserQuery.QueryDescription = name;
                oUserQuery.Query = query;
                //oUserQuery.QueryType = UserQueryTypeEnum.uqtWizard;
                //oUserQuery.
                //  oUserQuery.Query  = " SELECT T0.\"ItemCode\",T0.\"itemName\", T0.\"AbsEntry\", T0.\"DistNumber\", T0.\"ExpDate\" FROM OBTN T0 INNER JOIN OITM T1 ON T0.\"ItemCode\" = T1.\"ItemCode\"  WHERE ( T0.\"Status\"='0' OR T0.\"Status\"='1') AND T0.\"ExpDate\"<= ADD_DAYS(CURRENT_DATE,IFNULL( T1.\"U_CMC_RP_DDP\", (SELECT IFNULL(\"U_CMC_RP_DDP\",60) FROM OADM))) ORDER BY T0.\"itemName\",\"ExpDate\" ";
                oUserQuery.QueryCategory = CatID;

                if (oUserQuery.Add() != 0)
                {
                    myApi.SetStatusBarMessage(Comp_DI.GetLastErrorDescription());
                }
                else
                {
                    Comp_DI.GetNewObjectKey();
                }
                oUserQuery = null;
                GC.WaitForPendingFinalizers();
                GC.Collect();


            }

        }
        public static void InsertRecords(string Code, string Name, string UDO, string TableName)
        {
            CompanyService oCmpService;
            GeneralService oGeneralService;
            GeneralData oGeneralData;
            GeneralData oRowItem;
            GeneralDataCollection oRows;
            try
            {

                int nbres = 0;
                Global.QueryFirstValueRec("SELECT T0.\"Code\" FROM \"" + TableName + "\" T0 WHERE T0.\"Code\"='" + Code + "'", true, out nbres);
                if (nbres == 0)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    oCmpService = Global.Comp_DI.GetCompanyService();
                    oGeneralService = oCmpService.GetGeneralService(UDO);
                    oGeneralData = oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData) as GeneralData;
                    oGeneralData.SetProperty("Code", Code);
                    oGeneralData.SetProperty("Name", Name);
                    oRows = oGeneralData.Child("ONF1");
                    oRowItem = oRows.Add();
                    oRowItem.SetProperty("U_Status", "Active");
                    oRowItem = oRows.Add();
                    oRowItem.SetProperty("U_Status", "Approved");
                    oRowItem = oRows.Add();
                    oRowItem.SetProperty("U_Status", "CompleteWithdrawal");
                    oRowItem = oRows.Add();
                    oRowItem.SetProperty("U_Status", "Expelled");
                    oRowItem = oRows.Add();
                    oRowItem.SetProperty("U_Status", "Graduated");
                    oRowItem = oRows.Add();
                    oRowItem.SetProperty("U_Status", "InActive");
                    oRowItem = oRows.Add();
                    oRowItem.SetProperty("U_Status", "Registered");
                    oRowItem = oRows.Add();
                    oRowItem.SetProperty("U_Status", "Rejected");
                    oRowItem = oRows.Add();
                    oRowItem.SetProperty("U_Status", "Suspended");
                    oRows = oGeneralData.Child("ONF2");

                    for (int i = 0; i < 10; i++)
                    {
                        oRowItem = oRows.Add();
                    }

                    oGeneralService.Add(oGeneralData);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }

            }
            catch (Exception ex)
            {
                Global.SetMessage(ex.Message, BoStatusBarMessageType.smt_Error);

            }

        }
        public static int GetQueryID(string name, int Categorid)
        {
            try
            {
                //int nbres = 0;
                RecCheck = Comp_DI.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
                RecCheck.DoQuery("SELECT \"IntrnalKey\" FROM \"OUQR\" where \"QName\" = '" + name + "' and \"QCategory\"='" + Categorid.ToString() + "'");
                if (RecCheck.RecordCount != 0)
                    return Convert.ToInt32(RecCheck.Fields.Item("IntrnalKey").Value.ToString());
                else
                    return 0;

            }
            finally
            {
                RecCheck = null;
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }
        public static void CheckAndCreateAttachmentField(string TableName, string fieldName, string desc, SAPbobsCOM.BoFldSubTypes subType)
        {
            int nbres = 0;
            Global.QueryFirstValueRec("SELECT T0.\"AliasID\" FROM CUFD T0 WHERE T0.\"TableID\"='" + TableName + "' AND T0.\"AliasID\"='" + fieldName + "'", true, out nbres);
            if (nbres == 0)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                SAPbobsCOM.UserFieldsMD myUserFieldsMD = (SAPbobsCOM.UserFieldsMD)Comp_DI.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                myUserFieldsMD.Name = fieldName;
                myUserFieldsMD.TableName = TableName;
                myUserFieldsMD.Description = desc;
                myUserFieldsMD.Type = BoFieldTypes.db_Memo;
                //if (type == SAPbobsCOM.BoFieldTypes.db_Alpha || type == SAPbobsCOM.BoFieldTypes.db_Numeric)
                //{
                //    if (size > 0 && type == SAPbobsCOM.BoFieldTypes.db_Alpha)
                //        myUserFieldsMD.Size = size;
                //    myUserFieldsMD.EditSize = size;
                //}
                myUserFieldsMD.SubType = subType;

                //if (!string.IsNullOrEmpty(LinkedTable))
                //    myUserFieldsMD.LinkedTable = LinkedTable;

                int res = myUserFieldsMD.Add();
                if (res != 0)
                    SetAlertMessage(Comp_DI.GetLastErrorDescription() + " Table :" + TableName + "; Field" + fieldName);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(myUserFieldsMD);
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }
        }
       
        public static void CreateFormattedSearch(string formID, string itemID, string targetColumn, BoFormattedSearchActionEnum fmsAction, int queryID, BoYesNoEnum byField, BoYesNoEnum doesRefresh, BoYesNoEnum forceRefresh, string[] Fields)
        {
            try
            {
                int nbres = 0;
                Global.QueryFirstValueRec("SELECT \"FormID\" FROM CSHS Where \"FormID\" = '" + formID + "' and \"ItemID\" = '" + itemID + "' and \"ColID\"= '" + targetColumn + "'", true, out nbres);
                if (nbres == 0)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    FormattedSearches fs = Comp_DI.GetBusinessObject(BoObjectTypes.oFormattedSearches) as FormattedSearches;
                    fs.FormID = formID;
                    fs.ItemID = itemID;
                    fs.ColumnID = targetColumn;
                    fs.Action = fmsAction;
                    fs.QueryID = queryID;
                    
                    fs.ForceRefresh = forceRefresh;
                    fs.Refresh = doesRefresh;
                    fs.ByField = byField;
                    if (Fields != null)
                    {
                        for (int i = 0; i < Fields.Length; i++)
                        {

                            if (fs.FieldIDs.FieldID != "")
                            {
                                fs.FieldIDs.Add();
                            }
                            fs.FieldIDs.FieldID = Fields[i];
                        }
                    }
                    //if (byField == BoYesNoEnum.tYES)
                    //{
                    //    fs.FieldID = Fields[0];
                    //}
                    //else
                    //{

                    //}
                   
                    

                    if (fs.Add() != 0)
                    {
                        myApi.SetStatusBarMessage(Comp_DI.GetLastErrorDescription());
                    }
                    fs = null;
                }

            }
            finally
            {
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }


        }
      
     

        /// <summary>
        /// Connexion à la DI API
        /// </summary>
        protected static void Init_DI()
        {
            //get DI from UI
            Comp_DI = Comp.GetDICompany() as SAPbobsCOM.Company;
            if (!Comp_DI.Connected)
                Comp_DI.Disconnect();
        }

        #endregion

        #region creation des zones au démarrage


        public static void CheckAndCreateDefaultUserFieldWithLinkedObject(string TableName, string fieldName, string desc, SAPbobsCOM.BoFieldTypes type, SAPbobsCOM.BoFldSubTypes subType, int size, UDFLinkedSystemObjectTypesEnum LinkedObj)
        {
            int nbres = 0;
            Global.QueryFirstValueRec("SELECT T0.\"AliasID\" FROM CUFD T0 WHERE T0.\"TableID\"='" + TableName + "' AND T0.\"AliasID\"='" + fieldName + "'", true, out nbres);
            if (nbres == 0)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                SAPbobsCOM.UserFieldsMD myUserFieldsMD = (SAPbobsCOM.UserFieldsMD)Comp_DI.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                myUserFieldsMD.Name = fieldName;
                myUserFieldsMD.TableName = TableName;
                myUserFieldsMD.Description = desc;
                myUserFieldsMD.Type = type;

                if (type == SAPbobsCOM.BoFieldTypes.db_Alpha || type == SAPbobsCOM.BoFieldTypes.db_Numeric)
                {
                    if (size > 0 && type == SAPbobsCOM.BoFieldTypes.db_Alpha)
                        myUserFieldsMD.Size = size;
                    myUserFieldsMD.EditSize = size;
                }
                myUserFieldsMD.SubType = subType;

                myUserFieldsMD.LinkedSystemObject = LinkedObj;
                int res = myUserFieldsMD.Add();
                if (res != 0)
                    SetAlertMessage(Comp_DI.GetLastErrorDescription());
                System.Runtime.InteropServices.Marshal.ReleaseComObject(myUserFieldsMD);
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }
        }

        public static void CheckAndCreateDefaultUserField(string TableName, string fieldName, string desc, SAPbobsCOM.BoFieldTypes type, SAPbobsCOM.BoFldSubTypes subType, int size, string LinkedTable)
        {
            int nbres = 0;
            Global.QueryFirstValueRec("SELECT T0.\"AliasID\" FROM CUFD T0 WHERE T0.\"TableID\"='" + TableName + "' AND T0.\"AliasID\"='" + fieldName + "'", true, out nbres);
            if (nbres == 0)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                SAPbobsCOM.UserFieldsMD myUserFieldsMD = (SAPbobsCOM.UserFieldsMD)Comp_DI.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                myUserFieldsMD.Name = fieldName;
                myUserFieldsMD.TableName = TableName;
                myUserFieldsMD.Description = desc;
                myUserFieldsMD.Type = type;
                if (type == SAPbobsCOM.BoFieldTypes.db_Alpha || type == SAPbobsCOM.BoFieldTypes.db_Numeric)
                {
                    if (size > 0 && type == SAPbobsCOM.BoFieldTypes.db_Alpha)
                        myUserFieldsMD.Size = size;
                    myUserFieldsMD.EditSize = size;
                }
                myUserFieldsMD.SubType = subType;

                if (!string.IsNullOrEmpty(LinkedTable))
                {
                    myUserFieldsMD.LinkedTable = LinkedTable;
                }
                int res = myUserFieldsMD.Add();
                if (res != 0)
                    SetAlertMessage(Comp_DI.GetLastErrorDescription() + " Table :" + TableName + "; Field" + fieldName);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(myUserFieldsMD);
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }
        }
        public static void CheckAndCreateDefaultUserField(string TableName, string fieldName, string desc, SAPbobsCOM.BoFieldTypes type, SAPbobsCOM.BoFldSubTypes subType, int size, string LinkedTable, BoYesNoEnum mandatory)
        {
            int nbres = 0;
            Global.QueryFirstValueRec("SELECT T0.\"AliasID\" FROM CUFD T0 WHERE T0.\"TableID\"='" + TableName + "' AND T0.\"AliasID\"='" + fieldName + "'", true, out nbres);
            if (nbres == 0)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                SAPbobsCOM.UserFieldsMD myUserFieldsMD = (SAPbobsCOM.UserFieldsMD)Comp_DI.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                myUserFieldsMD.Name = fieldName;
                myUserFieldsMD.TableName = TableName;
                myUserFieldsMD.Description = desc;
                myUserFieldsMD.Type = type;
                myUserFieldsMD.Mandatory = mandatory;
                if (type == SAPbobsCOM.BoFieldTypes.db_Alpha || type == SAPbobsCOM.BoFieldTypes.db_Numeric)
                {
                    if (size > 0 && type == SAPbobsCOM.BoFieldTypes.db_Alpha)
                        myUserFieldsMD.Size = size;
                    myUserFieldsMD.EditSize = size;
                }
                myUserFieldsMD.SubType = subType;

                if (!string.IsNullOrEmpty(LinkedTable))
                {
                    myUserFieldsMD.LinkedTable = LinkedTable;
                }



                int res = myUserFieldsMD.Add();
                if (res != 0)
                    SetAlertMessage(Comp_DI.GetLastErrorDescription() + " Table :" + TableName + "; Field" + fieldName);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(myUserFieldsMD);
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }
        }
      
        public static void FillCombo(ComboBox cmb, Form oForm, string sSQL, string InitialValue)
        {
            Recordset oRs = (Recordset)Comp_DI.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRs.DoQuery(sSQL);
            oRs.MoveFirst();
            for (int i = cmb.ValidValues.Count - 1; i > -1; i--)
            {
                try
                {
                    cmb.ValidValues.Remove(i, BoSearchKey.psk_Index);
                }
                catch { }
            }

            while (!oRs.EoF)
            {
                if (oRs.Fields.Count > 1)
                {

                    try
                    {
                        cmb.ValidValues.Add(oRs.Fields.Item(0).Value.ToString().Trim(), oRs.Fields.Item(1).Value.ToString().Trim());
                    }
                    catch
                    {


                    }
                }

                else
                {
                    try
                    {
                        cmb.ValidValues.Add(oRs.Fields.Item(0).Value.ToString().Trim(), "");
                    }
                    catch { }
                }

                oRs.MoveNext();
            }
            SAPbouiCOM.ValidValues val = cmb.ValidValues;
            try
            {
                cmb.ValidValues.Add("", "");
            }
            catch
            {


            }

            if (InitialValue != "")
            {
                try
                {
                    DBDataSource oDB = (DBDataSource)oForm.DataSources.DBDataSources.Item(cmb.DataBind.TableName);
                    oDB.SetValue(cmb.DataBind.Alias, 0, InitialValue);
                }
                catch
                {
                    try
                    {
                        cmb.Select(InitialValue, BoSearchKey.psk_ByValue);
                    }
                    catch
                    {
                        try
                        {
                            cmb.Select(InitialValue, BoSearchKey.psk_ByValue);
                        }
                        catch { }
                    }
                }

            }
        }

        public static void FillCombo(ComboBox cmb, Form oForm, string sSQL, string InitialValue, bool AddEmptyValue)
        {
            Recordset oRs = (Recordset)Comp_DI.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRs.DoQuery(sSQL);
            oRs.MoveFirst();
            for (int i = cmb.ValidValues.Count - 1; i > -1; i--)
            {
                try
                {
                    cmb.ValidValues.Remove(i, BoSearchKey.psk_Index);
                }
                catch { }
            }

            while (!oRs.EoF)
            {
                if (oRs.Fields.Count > 1)
                {

                    try
                    {
                        cmb.ValidValues.Add(oRs.Fields.Item(0).Value.ToString().Trim(), oRs.Fields.Item(1).Value.ToString().Trim());
                    }
                    catch
                    {


                    }
                }

                else
                {
                    try
                    {
                    
                        
                            cmb.ValidValues.Add(oRs.Fields.Item(0).Value.ToString().Trim(), "");
                        
                    }
                    catch { }
                }

                oRs.MoveNext();
            }
            SAPbouiCOM.ValidValues val = cmb.ValidValues;
            try
            {
                if (AddEmptyValue)
                    cmb.ValidValues.Add("", "");
            }
            catch
            {


            }

            if (InitialValue != "")
            {
                try
                {
                    DBDataSource oDB = (DBDataSource)oForm.DataSources.DBDataSources.Item(cmb.DataBind.TableName);
                    oDB.SetValue(cmb.DataBind.Alias, 0, InitialValue);
                }
                catch
                {
                    try
                    {
                        cmb.Select(InitialValue, BoSearchKey.psk_ByValue);
                    }
                    catch
                    {
                        try
                        {
                            cmb.Select(InitialValue, BoSearchKey.psk_ByValue);
                        }
                        catch { }
                    }
                }

            }
        }

        public static void CheckAndCreateAlphaNumUserFieldWithValidValues(string TableName, string fieldName, string desc, SAPbobsCOM.BoFieldTypes type, int size, List<List<string>> ValidValues, bool IsMandatory, int rangDefVal)
        {
            int nbres = 0;
            Global.QueryFirstValueRec("SELECT T0.\"AliasID\" FROM CUFD T0 WHERE T0.\"TableID\"='" + TableName + "' AND T0.\"AliasID\"='" + fieldName + "'", true, out nbres);
            if (nbres == 0)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                SAPbobsCOM.UserFieldsMD myUserFieldsMD = (SAPbobsCOM.UserFieldsMD)Comp_DI.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                myUserFieldsMD.Name = fieldName;
                myUserFieldsMD.TableName = TableName;
                myUserFieldsMD.Description = desc;
                myUserFieldsMD.Type = type;
                myUserFieldsMD.EditSize = size;
                myUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_None;
                //validValues
                foreach (List<string> li in ValidValues)
                {
                    myUserFieldsMD.ValidValues.Value = li[0];
                    myUserFieldsMD.ValidValues.Description = li[1];
                    myUserFieldsMD.ValidValues.Add();
                }
                if (IsMandatory && rangDefVal > -1)
                {
                    myUserFieldsMD.DefaultValue = ValidValues[rangDefVal][0];
                    myUserFieldsMD.Mandatory = SAPbobsCOM.BoYesNoEnum.tYES;
                }
                int res = myUserFieldsMD.Add();
                if (res != 0)
                    SetAlertMessage(Comp_DI.GetLastErrorDescription() + " Table :" + TableName + "; Field" + fieldName);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(myUserFieldsMD);
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }
        }
        public static void initUserTables()
        {
            userTables.Clear();
            Menus userstablesMenus = myApi.Menus.Item("51200").SubMenus;
            string stemp;
            for (int i = 51201; i <= 51200 + userstablesMenus.Count; i++)
            {
                try
                {
                    stemp = userstablesMenus.Item(i.ToString()).String;
                    if (stemp.StartsWith("CMC_"))
                    {
                        userTables.Add(((string)(stemp.Split('-')).GetValue(0)).Trim(), i.ToString());
                    }
                }
                catch { }
            }
        }

        #endregion

        #region Others methodes

        /// <summary>
        /// Execute la requete dans un recordSet et le release si nécéssaire
        /// </summary>
        /// <param name="query">Requete</param>
        /// <param name="release">Release object</param>
        /// <returns>Valeur du premier champ et première ligne</returns>
        /// 

        public static object QueryFirstValueRec(string query, bool release, out int nbres, string nomchamps)
        {
            nbres = 0;
            if (RecCheck == null)
                RecCheck = (Recordset)Comp_DI.GetBusinessObject(BoObjectTypes.BoRecordset);
            try
            {
                RecCheck.DoQuery(query);
                nbres = RecCheck.RecordCount;
                if (RecCheck.RecordCount > 0)
                {
                    if (nomchamps.Trim().Length == 0)
                        return RecCheck.Fields.Item(0).Value;
                    else
                        return RecCheck.Fields.Item(nomchamps).Value;
                }
                else
                {
                    switch (RecCheck.Fields.Item(0).Type)
                    {
                        case BoFieldTypes.db_Alpha:
                        case BoFieldTypes.db_Memo: return "";
                        case BoFieldTypes.db_Float: return 0.0d;
                        case BoFieldTypes.db_Numeric: return 0;
                        case BoFieldTypes.db_Date: return "";
                    }
                    return null;
                }

            }
            catch (Exception)
            {
                if (!query.StartsWith("Delete", StringComparison.InvariantCultureIgnoreCase) && !query.StartsWith("Insert", StringComparison.InvariantCultureIgnoreCase) && !query.StartsWith("Update", StringComparison.InvariantCultureIgnoreCase))
                    Global.SetMessage("Erreur de requête sur : " + query, BoStatusBarMessageType.smt_Warning);
                return null;
            }
            finally
            {
                if (release)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(RecCheck);
                    RecCheck = null;
                    GC.WaitForPendingFinalizers();
                }
            }
        }
        public static object QueryFirstValueRec(string query, bool release, out int nbres)
        {
            return QueryFirstValueRec(query, release, out nbres, "");
        }
        public static object QueryFirstValueRec(string query, bool release)
        {
            int bidon = 0;
            return QueryFirstValueRec(query, release, out bidon, "");
        }
    
        public static void SetMessage(string Text, BoStatusBarMessageType type)
        {
            myApi.StatusBar.SetText(Text, BoMessageTime.bmt_Short, type);
        }
        public static void SetAlertMessage(string text)
        {
            try
            {
                Global.myApi.MessageBox(text, 1, "OK", "", "");
            }
            catch (Exception) { }
        }

        
        public static void CheckAndCreateUDO(string UdoCode, string UdoName, BoUDOObjType UdoType, string table, BoYesNoEnum canDel, BoYesNoEnum canLog, BoYesNoEnum canFind, BoYesNoEnum canCreateDefForm, BoYesNoEnum enhancedForm, List<string> findColumns, List<string> formsCols, List<string> childTables)
        {
            //Create UDOs
            GC.Collect();
            GC.WaitForPendingFinalizers();
            SAPbobsCOM.UserObjectsMD udo = (SAPbobsCOM.UserObjectsMD)Comp_DI.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserObjectsMD);
            if (!udo.GetByKey(UdoCode))
            {
                try
                {
                    udo.Code = UdoCode;
                    udo.Name = UdoName;
                    udo.ObjectType = UdoType;
                    udo.TableName = table;

                    udo.CanDelete = canDel;
                    udo.EnableEnhancedForm = enhancedForm;
                    udo.CanLog = canLog;
                    if (canLog == BoYesNoEnum.tYES)
                        udo.LogTableName = "A" + table;

                    udo.CanFind = canFind;
                    foreach (string tempstr in findColumns)
                    {
                        udo.FindColumns.ColumnAlias = tempstr;
                        udo.FindColumns.Add();
                    }

                    udo.CanCreateDefaultForm = canCreateDefForm;
                    foreach (string tempstr in formsCols)
                    {
                        udo.FormColumns.FormColumnAlias = tempstr;
                        udo.FormColumns.Add();
                    }
                    foreach (string tempstr in childTables)
                    {
                        udo.ChildTables.TableName = tempstr;
                        udo.ChildTables.Add();
                    }

                    if (udo.Add() != 0)
                        SetAlertMessage(Comp_DI.GetLastErrorDescription());
                    else
                        udo.Update();
                }
                catch (Exception e)
                {
                    SetAlertMessage(e.Message);
                }
                finally
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(udo);
                    udo = null;
                    GC.WaitForPendingFinalizers();
                    GC.Collect();

                }
            }

        }

        public static void CheckAndCreateTable(string TableName, string desc, SAPbobsCOM.BoUTBTableType type)
        {
            GC.WaitForPendingFinalizers();
            GC.Collect();
            SAPbobsCOM.UserTablesMD myUserTablesMD = (SAPbobsCOM.UserTablesMD)Comp_DI.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserTables);
            if (!myUserTablesMD.GetByKey(TableName))
            {
                myUserTablesMD.TableName = TableName;
                myUserTablesMD.TableDescription = desc;
                myUserTablesMD.TableType = type;
                int res = myUserTablesMD.Add();
                if (res != 0)
                    Global.SetAlertMessage(Comp_DI.GetLastErrorDescription());

            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(myUserTablesMD);
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
     
        public static void Quit()
        {
            try
            {
                if (Comp_DI.Connected)
                    Comp_DI.Disconnect();
            }
            catch (Exception)
            {
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Environment.Exit(0);
            }
        }



        #endregion

        #region evènements



        #endregion

        #region filtres

        public static void MAJ_filter()
        {
            //filtres		menu principale	
            EventFilters mesFiltres = new SAPbouiCOM.EventFilters();
            EventFilter monFiltre = mesFiltres.Add(SAPbouiCOM.BoEventTypes.et_ALL_EVENTS); 
            monFiltre.AddEx("SM_One.SMConfig");
            monFiltre.AddEx("SM_One.SAP.ImportRegistrations");
            monFiltre.AddEx("SM_One.Registration");
            monFiltre.AddEx("SM_One.GenerateInvoices");
            monFiltre.AddEx("SM_One.ViewDifferences");
            monFiltre.AddEx("SM_One.SearchCourses");
            monFiltre.AddEx("SM_One.Postings");
            monFiltre.AddEx("SM_One.SAP.ERPIntegration");
            monFiltre.AddEx("229");
         
            myApi.SetFilter(mesFiltres);
        }

        #endregion

        #region menus

        public static void AddMenu(SAPbouiCOM.Application oApp, string id, string name, string father, int position, BoMenuType type, string image)
        {
            MenuItem MenuParent;
            Menus collection;
            try
            {
                #region ajout du menu

                MenuParent = Global.myApi.Menus.Item(father);
                collection = MenuParent.SubMenus;
                if (!collection.Exists(id))
                {
                    if (position == -1)
                        position = collection.Count + 1;
                    try
                    {
                        creationMenu = oApp.CreateObject(BoCreatableObjectType.cot_MenuCreationParams) as MenuCreationParams;
                        creationMenu.UniqueID = id;
                        creationMenu.Type = type;
                        creationMenu.Position = position;
                        creationMenu.String = name;
                        creationMenu.Image = CurrentDirectory + "\\" + image;
                        monMenuItem = collection.AddEx(creationMenu);
                        monMenuItem.Enabled = true;
                    }
                    catch (Exception)
                    {
                        try
                        {
                            creationMenu = oApp.CreateObject(BoCreatableObjectType.cot_MenuCreationParams) as MenuCreationParams;
                            creationMenu.UniqueID = id;
                            creationMenu.Type = type;
                            creationMenu.Position = position;
                            creationMenu.String = name;
                            monMenuItem = collection.AddEx(creationMenu);
                            monMenuItem.Enabled = true;
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                #endregion
            }
            catch (Exception)
            {
            }
            finally
            {
                MenuParent = null;
                collection = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        #endregion
    }

    

    public class WindowWrapper : System.Windows.Forms.IWin32Window
    {
        public WindowWrapper(IntPtr handle)
        {
            _hwnd = handle;
        }

        public IntPtr Handle
        {
            get { return _hwnd; }
        }

        private IntPtr _hwnd;
    }
}
