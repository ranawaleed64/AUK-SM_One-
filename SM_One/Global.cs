using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;


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
      
        public static void ConnectUI()
        {

            myApi = SAPbouiCOM.Framework.Application.SBO_Application;
            Comp = myApi.Company;
        }
        private void GetFontSize()
        {
            
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
            //AddMenu(myApi, "ABS_PC", "Pre Costing", "43520", -1, BoMenuType.mt_POPUP, "");
            #region menus
            AddMenu(myApi, "BL_SM1", "Student Management", "43520", -1, BoMenuType.mt_POPUP, "student.bmp");
            AddMenu(myApi, "BL_SM9", "Setup", "BL_SM1", 1, BoMenuType.mt_POPUP, "");
            AddMenu(myApi, "BL_SM2", "Configuration", "BL_SM9", 1, BoMenuType.mt_STRING, "");
            AddMenu(myApi, "BL_SM5", "Semester Setup", "BL_SM9", 2, BoMenuType.mt_STRING, "");
            AddMenu(myApi, "BL_SM8", "Revenue Mapping", "BL_SM9", 3, BoMenuType.mt_STRING, "");
            AddMenu(myApi, "BL_SM3", "Student Curriculum", "BL_SM9", 4, BoMenuType.mt_STRING, "");
            AddMenu(myApi, "BL_SM4", "Student Import Wizard", "BL_SM1", 2, BoMenuType.mt_STRING, "");
            AddMenu(myApi, "BL_SM6", "Student Registration", "BL_SM1", 3, BoMenuType.mt_STRING, "");
            AddMenu(myApi, "BL_SM7", "Student Invoices", "BL_SM1", 4, BoMenuType.mt_STRING, "");
            AddMenu(myApi, "BL_SM10", "Student Refunds", "BL_SM1", 4, BoMenuType.mt_STRING, "");

            #endregion

            ////AddMenu(myApi, "AC_VC4", "Variant Selection", "AC_VC1", 5, BoMenuType.mt_STRING, "");
            ////AddMenu(myApi, "AC_VC5", "PO Generator", "4352", -1, BoMenuType.mt_STRING, "");
            ////AddMenu(myApi, "AC_VC9", "Configuration Master", "AC_VC1", 7, BoMenuType.mt_STRING, "");
            ////AddMenu(myApi, "AC_VC8", "Attribute Configuration Wizard", "AC_VC1", 6, BoMenuType.mt_STRING, "");
            ////AddMenu(myApi, "AC_VC10", "Option Categories Link", "AC_VC1", 8, BoMenuType.mt_STRING, "");
            ////AddMenu(myApi, "AC_VC11", "Labels", "AC_VC1", 9, BoMenuType.mt_STRING, "");
            ////AddMenu(myApi, "AC_VC12", "Special Remarks", "AC_VC1", 10, BoMenuType.mt_STRING, "");

            //Standard UDFs//
            #region StandardUDFS
            List<List<string>> validValsSts = new List<List<string>>();
            List<string> tables1 = new List<string>();
            List<string> udoFormCols1 = new List<string>();
            List<string> ColRech1 = new List<string>();
            List<string> enhancedudoFormCols1 = new List<string>();
            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("SSS"); validValsSts[0].Add("School of Social Sciences");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("SSE"); validValsSts[1].Add("School of Science & Engineering");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("SME"); validValsSts[2].Add("School of Management & Economics");
            validValsSts.Add(new List<string>()); validValsSts[3].Add("SOM"); validValsSts[3].Add("School of Medicine");
            validValsSts.Add(new List<string>()); validValsSts[4].Add("FND"); validValsSts[4].Add("Foundation");
            CheckAndCreateAlphaNumUserFieldWithValidValues("OCRD", "School", "School", BoFieldTypes.db_Alpha, 3, validValsSts, false, 0);
            CheckAndCreateDefaultUserField("OCRD", "Department", "Department", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            CheckAndCreateDefaultUserField("OCRD", "Program", "Program", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            CheckAndCreateDefaultUserField("OCRD", "Degree_Type", "Degree Type", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, "");
            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("UG"); validValsSts[0].Add("UG");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("PG"); validValsSts[1].Add("PG");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("PH"); validValsSts[2].Add("PH");
            validValsSts.Add(new List<string>()); validValsSts[3].Add("FN"); validValsSts[3].Add("FN");
            CheckAndCreateAlphaNumUserFieldWithValidValues("OCRD", "Level", "Level", BoFieldTypes.db_Alpha, 2, validValsSts, false, 0);

            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("New"); validValsSts[0].Add("New");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("Current"); validValsSts[1].Add("Current");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("Transfer"); validValsSts[2].Add("Transfer");
            validValsSts.Add(new List<string>()); validValsSts[3].Add("Retake"); validValsSts[3].Add("Retake");
            validValsSts.Add(new List<string>()); validValsSts[4].Add("Pending"); validValsSts[4].Add("Pending");
            validValsSts.Add(new List<string>()); validValsSts[5].Add("EXT"); validValsSts[5].Add("Extension/Reviva");
            validValsSts.Add(new List<string>()); validValsSts[6].Add("Readmit"); validValsSts[6].Add("Readmission");
            validValsSts.Add(new List<string>()); validValsSts[7].Add("PTDW"); validValsSts[7].Add("Postponed/Terminated");
            CheckAndCreateAlphaNumUserFieldWithValidValues("OCRD", "Status", "Status", BoFieldTypes.db_Alpha, 20, validValsSts, false, 0);
            CheckAndCreateDefaultUserField("OCRD", "Joining_Year", "Current Fiscal Year", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("OCRD", "YearJoined", "Joining Year", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 4, "");

            CheckAndCreateDefaultUserField("OCRD", "SL", "Sub-Level", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, "");
            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("RWG"); validValsSts[0].Add("Rwanga");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("MMA"); validValsSts[1].Add("Ministry of Martyrs & Anfal");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("CHA"); validValsSts[2].Add("Chancellor");
            CheckAndCreateAlphaNumUserFieldWithValidValues("OCRD", "Sponsor", "Sponsor", BoFieldTypes.db_Alpha, 3, validValsSts, false, 0);
            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("N"); validValsSts[0].Add("No");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("Y"); validValsSts[1].Add("Yes");
            CheckAndCreateAlphaNumUserFieldWithValidValues("OACT", "RefundAcct", "Refund Account", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);
            CheckAndCreateDefaultUserField("OCRD", "AY", "Academic Year", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, "");
            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("1"); validValsSts[0].Add("Semester 1");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("2"); validValsSts[1].Add("Semester 2");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("10"); validValsSts[2].Add("Annual");
            CheckAndCreateAlphaNumUserFieldWithValidValues("OCRD", "Semester", "Semester", BoFieldTypes.db_Alpha, 2, validValsSts, false, 0);


            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("N"); validValsSts[0].Add("No");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("Y"); validValsSts[1].Add("Yes");
            CheckAndCreateAlphaNumUserFieldWithValidValues("OINV", "SendInvoice", "Send Invoice", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);
            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("SSS"); validValsSts[0].Add("School of Social Sciences");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("SSE"); validValsSts[1].Add("School of Science & Engineering");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("SME"); validValsSts[2].Add("School of Management & Economics");
            validValsSts.Add(new List<string>()); validValsSts[3].Add("SOM"); validValsSts[3].Add("School of Medicine");
            validValsSts.Add(new List<string>()); validValsSts[4].Add("FND"); validValsSts[4].Add("Foundation");
            CheckAndCreateAlphaNumUserFieldWithValidValues("OINV", "School", "School", BoFieldTypes.db_Alpha, 3, validValsSts, false, 0);
            CheckAndCreateDefaultUserField("OINV", "Department", "Department", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            CheckAndCreateDefaultUserField("OINV", "Program", "Program", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            CheckAndCreateDefaultUserField("OINV", "Degree_Type", "Degree Type", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, "");
            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("UG"); validValsSts[0].Add("UG");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("PG"); validValsSts[1].Add("PG");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("PH"); validValsSts[2].Add("PH");
            validValsSts.Add(new List<string>()); validValsSts[3].Add("FN"); validValsSts[3].Add("FN");
            CheckAndCreateAlphaNumUserFieldWithValidValues("OINV", "Level", "Level", BoFieldTypes.db_Alpha, 2, validValsSts, false, 0);

            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("New"); validValsSts[0].Add("New");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("Current"); validValsSts[1].Add("Current");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("Transfer"); validValsSts[2].Add("Transfer");
            validValsSts.Add(new List<string>()); validValsSts[3].Add("Retake"); validValsSts[3].Add("Retake");
            validValsSts.Add(new List<string>()); validValsSts[4].Add("Pending"); validValsSts[4].Add("Pending");
            validValsSts.Add(new List<string>()); validValsSts[5].Add("EXT"); validValsSts[5].Add("Extension/Reviva");
            validValsSts.Add(new List<string>()); validValsSts[6].Add("Readmit"); validValsSts[6].Add("Readmission");
            validValsSts.Add(new List<string>()); validValsSts[7].Add("PTDW"); validValsSts[7].Add("Postponed/Terminated");
            CheckAndCreateAlphaNumUserFieldWithValidValues("OINV", "Status", "Status", BoFieldTypes.db_Alpha, 20, validValsSts, false, 0);
            CheckAndCreateDefaultUserField("OINV", "Joining_Year", "Invoicing Fiscal Year", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("OINV", "SL", "Sub-Level", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, "");
            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("RWG"); validValsSts[0].Add("Rwanga");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("MMA"); validValsSts[1].Add("Ministry of Martyrs & Anfal");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("CHA"); validValsSts[2].Add("Chancellor");
            CheckAndCreateAlphaNumUserFieldWithValidValues("OINV", "Sponsor", "Sponsor", BoFieldTypes.db_Alpha, 3, validValsSts, false, 0);
            CheckAndCreateDefaultUserField("OINV", "AY", "Academic Year", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, "");
            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("1"); validValsSts[0].Add("Semester 1");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("2"); validValsSts[1].Add("Semester 2");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("10"); validValsSts[2].Add("Annual");
            CheckAndCreateAlphaNumUserFieldWithValidValues("OINV", "Semester", "Semester", BoFieldTypes.db_Alpha, 2, validValsSts, false, 0);
            CheckAndCreateDefaultUserField("OINV", "RegNo", "Registration No.", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("OINV", "GenNo", "Generation No.", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("OINV", "GenLine", "Generation Line", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("OPLN", "FiscalYear", "Fiscal Year", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateAttachmentField("OADM", "FilePath", "File Path", BoFldSubTypes.st_Link);
            CheckAndCreateDefaultUserFieldWithLinkedObject("JDT1", "InvEntry", "Invoice Key", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, UDFLinkedSystemObjectTypesEnum.ulInvoices);
            CheckAndCreateDefaultUserFieldWithLinkedObject("JDT1", "MemoEntry", "Memo Key", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, UDFLinkedSystemObjectTypesEnum.ulCreditNotes);

            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("N"); validValsSts[0].Add("No");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("Y"); validValsSts[1].Add("Yes");
            CheckAndCreateAlphaNumUserFieldWithValidValues("OJDT", "StVoucher", "Student Voucher", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);
            CheckAndCreateAlphaNumUserFieldWithValidValues("OJDT", "Reversal", "Reversal Voucher", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);

            #endregion
            //Standard UDFs//
            #region UserDefinedTables
            CheckAndCreateTable("CONF", "Configuration", BoUTBTableType.bott_MasterData);
            CheckAndCreateTable("ONF1", "Configuration Lines", BoUTBTableType.bott_MasterDataLines);
            CheckAndCreateTable("ONF2", "Configuration Lines 2", BoUTBTableType.bott_MasterDataLines);
            CheckAndCreateTable("OSEM", "Semester Setup", BoUTBTableType.bott_MasterData);
            CheckAndCreateTable("ORMP", "Revenue Mapping", BoUTBTableType.bott_NoObject);
            CheckAndCreateTable("OSCL", "Student Curriculum", BoUTBTableType.bott_Document);
            CheckAndCreateTable("SCL1", "Student Curriculum Rows", BoUTBTableType.bott_DocumentLines);
            CheckAndCreateTable("OSRG", "Student Registration", BoUTBTableType.bott_Document);
            CheckAndCreateTable("SRG1", "Student Registration Rows", BoUTBTableType.bott_DocumentLines);
            CheckAndCreateTable("OING", "Invoice Gen. Header", BoUTBTableType.bott_Document);
            CheckAndCreateTable("ING1", "Invoice Gen. Rows", BoUTBTableType.bott_DocumentLines);
            CheckAndCreateTable("TING1", "Invoice Gen. Rows Temporary", BoUTBTableType.bott_DocumentLines);
            //CheckAndCreateTable("AC_OVCG", "Config Header", BoUTBTableType.bott_MasterData);
            //CheckAndCreateTable("AC_VCG1", "Config Lines", BoUTBTableType.bott_MasterDataLines);
            //CheckAndCreateTable("AC_OOPT", "Option Master", BoUTBTableType.bott_MasterData);
            //CheckAndCreateTable("AC_OCAT", "Categories", BoUTBTableType.bott_MasterData);
            //CheckAndCreateTable("AC_OIAC", "Item Config Header", BoUTBTableType.bott_Document);
            //CheckAndCreateTable("AC_IAC1", "Item Config Lines", BoUTBTableType.bott_DocumentLines);
            //CheckAndCreateTable("AC_ORSG", "Resource Group", BoUTBTableType.bott_NoObject);
            //CheckAndCreateTable("AC_OMCT", "Option Categories Link", BoUTBTableType.bott_MasterData);
            //CheckAndCreateTable("AC_OLBL", "Item Labels", BoUTBTableType.bott_MasterData);
            //CheckAndCreateTable("AC_OSPR", "Special Remarks", BoUTBTableType.bott_MasterData);
            #endregion

            //List<List<string>> validValsSts = new List<List<string>>();
            //Semester Setup//

            CheckAndCreateDefaultUserField("@CONF", "VSeries", "Voucher Series", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@CONF", "InvSeries", "Invoice Series", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, "");
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
            CheckAndCreateAlphaNumUserFieldWithValidValues("@CONF", "ProgramDim", "Program Dimension", BoFieldTypes.db_Alpha, 1, validValsSts, false, 0);
            CheckAndCreateDefaultUserField("@CONF", "GroupCode", "BP Group Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 20, "");
            CheckAndCreateDefaultUserField("@CONF", "GroupName", "BP Group Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");

            CheckAndCreateDefaultUserField("@CONF", "SQLServer", "ERP Server Address", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");
            CheckAndCreateDefaultUserField("@CONF", "SQLDatabase", "ERP Server Db", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");
            CheckAndCreateDefaultUserField("@CONF", "SQLUsername", "ERP Server Username", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");
            CheckAndCreateDefaultUserField("@CONF", "SQLPassword", "ERP Server Password", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");


            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("New"); validValsSts[0].Add("New");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("Current"); validValsSts[1].Add("Current");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("Transfer"); validValsSts[2].Add("Transfer");
            validValsSts.Add(new List<string>()); validValsSts[3].Add("Retake"); validValsSts[3].Add("Retake");
            validValsSts.Add(new List<string>()); validValsSts[4].Add("Pending"); validValsSts[4].Add("Pending");
            validValsSts.Add(new List<string>()); validValsSts[5].Add("EXT"); validValsSts[5].Add("Extension/Reviva");
            validValsSts.Add(new List<string>()); validValsSts[6].Add("Readmit"); validValsSts[6].Add("Readmission");
            validValsSts.Add(new List<string>()); validValsSts[7].Add("PTDW"); validValsSts[7].Add("Postponed/Terminated");

            CheckAndCreateAlphaNumUserFieldWithValidValues("@ONF1", "Status", "Status", BoFieldTypes.db_Alpha, 20, validValsSts, true, 0);
            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("N"); validValsSts[0].Add("No");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("Y"); validValsSts[1].Add("Yes");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@ONF1", "Program", "Inc. Program", BoFieldTypes.db_Alpha, 1, validValsSts, false, 0);
            CheckAndCreateAlphaNumUserFieldWithValidValues("@ONF1", "Courses", "Inc. Courses", BoFieldTypes.db_Alpha, 1, validValsSts, false, 0);
            CheckAndCreateDefaultUserField("@ONF2", "Freight", "FreightCode", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@ONF2", "Enabled", "Enabled", BoFieldTypes.db_Alpha, 1, validValsSts, false, 0);
            tables1.Add("ONF1");
            tables1.Add("ONF2");
            CheckAndCreateUDO("ONF", "SMConfig", BoUDOObjType.boud_MasterData, "CONF", BoYesNoEnum.tNO, BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tNO, BoYesNoEnum.tNO, ColRech1, udoFormCols1, tables1);
            InsertRecords("CONF", "CONF", "ONF", "@CONF");

            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("UG"); validValsSts[0].Add("UG");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("PG"); validValsSts[1].Add("PG");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("PH"); validValsSts[2].Add("PH");
            validValsSts.Add(new List<string>()); validValsSts[3].Add("FN"); validValsSts[3].Add("FN");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@OSEM", "Level", "Level", BoFieldTypes.db_Alpha, 2, validValsSts, false, 0);
            CheckAndCreateDefaultUserField("@OSEM", "SubLevel", "Sub Level", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None,10, "");
            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("1"); validValsSts[0].Add("JAN");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("2"); validValsSts[1].Add("FEB");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("3"); validValsSts[2].Add("MAR");
            validValsSts.Add(new List<string>()); validValsSts[3].Add("4"); validValsSts[3].Add("APR");
            validValsSts.Add(new List<string>()); validValsSts[4].Add("5"); validValsSts[4].Add("MAY");
            validValsSts.Add(new List<string>()); validValsSts[5].Add("6"); validValsSts[5].Add("JUN");
            validValsSts.Add(new List<string>()); validValsSts[6].Add("7"); validValsSts[6].Add("JUL");
            validValsSts.Add(new List<string>()); validValsSts[7].Add("8"); validValsSts[7].Add("AUG");
            validValsSts.Add(new List<string>()); validValsSts[8].Add("9"); validValsSts[8].Add("SEP");
            validValsSts.Add(new List<string>()); validValsSts[9].Add("10"); validValsSts[9].Add("OCT");
            validValsSts.Add(new List<string>()); validValsSts[10].Add("11"); validValsSts[10].Add("NOV");
            validValsSts.Add(new List<string>()); validValsSts[11].Add("12"); validValsSts[11].Add("DEC");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@OSEM", "Month", "Start Month", BoFieldTypes.db_Numeric, 2, validValsSts, false, 0);
            CheckAndCreateDefaultUserField("@OSEM", "Day", "Start Day", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 2, "");
            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("1"); validValsSts[0].Add("1 Month");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("2"); validValsSts[1].Add("2 Months");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("3"); validValsSts[2].Add("3 Months");
            validValsSts.Add(new List<string>()); validValsSts[3].Add("4"); validValsSts[3].Add("4 Months");
            validValsSts.Add(new List<string>()); validValsSts[4].Add("5"); validValsSts[4].Add("5 Months");
            validValsSts.Add(new List<string>()); validValsSts[5].Add("6"); validValsSts[5].Add("6 Months");
            validValsSts.Add(new List<string>()); validValsSts[6].Add("7"); validValsSts[6].Add("7 Months");
            validValsSts.Add(new List<string>()); validValsSts[7].Add("8"); validValsSts[7].Add("8 Months");
            validValsSts.Add(new List<string>()); validValsSts[8].Add("9"); validValsSts[8].Add("9 Months");
            validValsSts.Add(new List<string>()); validValsSts[9].Add("10"); validValsSts[9].Add("10 Months");
            validValsSts.Add(new List<string>()); validValsSts[10].Add("11"); validValsSts[10].Add("11 Months");
            validValsSts.Add(new List<string>()); validValsSts[11].Add("12"); validValsSts[11].Add("12 Months");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@OSEM", "Duration", "Duration", BoFieldTypes.db_Numeric, 2, validValsSts, false, 0);

            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("1"); validValsSts[0].Add("Semester 1");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("2"); validValsSts[1].Add("Semester 2");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("10"); validValsSts[2].Add("Annual");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@OSEM", "Semester", "Semester", BoFieldTypes.db_Alpha, 2, validValsSts, false, 0);

            udoFormCols1.Add("Code");
            udoFormCols1.Add("U_Level");
            udoFormCols1.Add("U_SubLevel");
            udoFormCols1.Add("U_Month");
            udoFormCols1.Add("U_Day");
            udoFormCols1.Add("U_Duration");
            udoFormCols1.Add("U_Semester");
            ColRech1.Add("Code");
            ColRech1.Add("U_Level");
            ColRech1.Add("U_SubLevel");
            ColRech1.Add("U_Month");
            ColRech1.Add("U_Day");
            ColRech1.Add("U_Duration");
            ColRech1.Add("U_Semester");

            CheckAndCreateUDO("SEM", "Semester", BoUDOObjType.boud_MasterData, "OSEM", BoYesNoEnum.tYES, BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tYES, BoYesNoEnum.tYES, ColRech1, udoFormCols1, BoYesNoEnum.tNO, BoYesNoEnum.tNO, "", "");
            udoFormCols1.Clear();
            ColRech1.Clear();

            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("UG"); validValsSts[0].Add("UG");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("PG"); validValsSts[1].Add("PG");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("PH"); validValsSts[2].Add("PH");
            validValsSts.Add(new List<string>()); validValsSts[3].Add("FN"); validValsSts[3].Add("FN");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@ORMP", "Level", "Level", BoFieldTypes.db_Alpha, 2, validValsSts, false, 0);
            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("SSS"); validValsSts[0].Add("School of Social Sciences");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("SSE"); validValsSts[1].Add("School of Science & Engineering");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("SME"); validValsSts[2].Add("School of Management & Economics");
            validValsSts.Add(new List<string>()); validValsSts[3].Add("SOM"); validValsSts[3].Add("School of Medicine");
            validValsSts.Add(new List<string>()); validValsSts[4].Add("FND"); validValsSts[4].Add("Foundation");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@ORMP", "School", "School", BoFieldTypes.db_Alpha, 3, validValsSts, false, 0);
            CheckAndCreateDefaultUserFieldWithLinkedObject("@ORMP", "Debit", "Debit Account", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 25, UDFLinkedSystemObjectTypesEnum.ulChartOfAccounts);
            CheckAndCreateDefaultUserFieldWithLinkedObject("@ORMP", "Credit", "Credit Account", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 25, UDFLinkedSystemObjectTypesEnum.ulChartOfAccounts);
            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("R"); validValsSts[0].Add("Invoice");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("C"); validValsSts[1].Add("Memo");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@ORMP", "MapType", "Mapping For", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);

            //udoFormCols1.Add("Code");
            //udoFormCols1.Add("U_Level");
            //udoFormCols1.Add("U_School");
            //udoFormCols1.Add("U_Revenue");
            //udoFormCols1.Add("U_Unearned");
            //ColRech1.Add("Code");
            //ColRech1.Add("U_Level");
            //ColRech1.Add("U_School");
            //ColRech1.Add("U_Revenue");
            //ColRech1.Add("U_Unearned");

            //CheckAndCreateUDO("RMP", "Revenue_Mapping", BoUDOObjType.boud_MasterData, "ORMP", BoYesNoEnum.tYES, BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tYES, BoYesNoEnum.tYES, ColRech1, udoFormCols1, BoYesNoEnum.tNO, BoYesNoEnum.tNO, "", "");
            //udoFormCols1.Clear();
            //ColRech1.Clear();
            //validValsSts.Clear();

            CheckAndCreateDefaultUserField("@OSCL", "PC", "Program Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@OSCL", "PN", "Program Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 70, "");
            CheckAndCreateDefaultUserField("@OSCL", "FD", "From Date", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@OSCL", "TD", "To Date", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@OSCL", "FY", "Fiscal Year", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, "");
            //CheckAndCreateDefaultUserField("@OSCL", "School", "School", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 35, "");
            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("Credit"); validValsSts[0].Add("Credit Hours Per Year");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("Subject"); validValsSts[1].Add("Subjects Per Year");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@OSCL", "CB", "Calculation Base", BoFieldTypes.db_Alpha, 10, validValsSts, false, 0);

            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("2"); validValsSts[0].Add("Bachelors");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("4"); validValsSts[1].Add("Masters");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@OSCL", "ProgType", "Program Type", BoFieldTypes.db_Alpha, 10, validValsSts, false, 0);
            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("SSS"); validValsSts[0].Add("School of Social Sciences");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("SSE"); validValsSts[1].Add("School of Science & Engineering");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("SME"); validValsSts[2].Add("School of Management & Economics");
            validValsSts.Add(new List<string>()); validValsSts[3].Add("SOM"); validValsSts[3].Add("School of Medicine");
            validValsSts.Add(new List<string>()); validValsSts[4].Add("FND"); validValsSts[4].Add("Foundation");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@OSCL", "School", "School", BoFieldTypes.db_Alpha, 3, validValsSts, false, 0);
            CheckAndCreateDefaultUserField("@SCL1", "Level", "Level", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, "");
            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("1"); validValsSts[0].Add("Semester 1");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("2"); validValsSts[1].Add("Semester 2");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("10"); validValsSts[2].Add("Annual");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@SCL1", "Semester", "Semester", BoFieldTypes.db_Numeric, 2, validValsSts, false, 0);
            CheckAndCreateDefaultUserFieldWithLinkedObject("@SCL1", "SC", "Subject Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, UDFLinkedSystemObjectTypesEnum.ulItems);
            CheckAndCreateDefaultUserField("@SCL1", "SN", "Subject Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 200, "");
            CheckAndCreateDefaultUserField("@SCL1", "CH", "Credit Hours", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 10, "");

            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("Y"); validValsSts[0].Add("Yes");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("N"); validValsSts[1].Add("No");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@SCL1", "RC", "Relevant for Calculation", BoFieldTypes.db_Alpha, 1, validValsSts, false, 0);


            tables1.Add("SCL1");
            ColRech1.Add("DocEntry");
            ColRech1.Add("U_PC");
            ColRech1.Add("U_PN");
            ColRech1.Add("U_FD");
            ColRech1.Add("U_TD");
            ColRech1.Add("U_FY");
            ColRech1.Add("U_School");
            ColRech1.Add("U_CB");
            udoFormCols1.Add("DocEntry");
            udoFormCols1.Add("U_PC");
            udoFormCols1.Add("U_PN");
            udoFormCols1.Add("U_FD");
            udoFormCols1.Add("U_TD");
            udoFormCols1.Add("U_FY");
            udoFormCols1.Add("U_School");
            udoFormCols1.Add("U_CB");

            CheckAndCreateUDO("OSCL", "OSCL", BoUDOObjType.boud_Document, "OSCL", BoYesNoEnum.tYES, BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tYES, BoYesNoEnum.tYES, ColRech1, udoFormCols1, tables1);

            CheckAndCreateDefaultUserField("@OSRG", "StudentID", "Student ID", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            CheckAndCreateDefaultUserField("@OSRG", "StudentName", "Student Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");
            CheckAndCreateDefaultUserField("@OSRG", "Level", "Level", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@OSRG", "ProgramCode", "Program Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@OSRG", "FiscalYear", "Registration Year", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@OSRG", "AcademicYear", "Academic Year", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@OSRG", "StartDate", "Start Date", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@OSRG", "EndDate", "End Date", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@OSRG", "TrgtEntry", "Invoice Entry", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@OSRG", "TrgtNum", "Target Number", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@OSRG", "Discount", "Discount Amount", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");

            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("SSS"); validValsSts[0].Add("School of Social Sciences");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("SSE"); validValsSts[1].Add("School of Science & Engineering");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("SME"); validValsSts[2].Add("School of Management & Economics");
            validValsSts.Add(new List<string>()); validValsSts[3].Add("SOM"); validValsSts[3].Add("School of Medicine");
            validValsSts.Add(new List<string>()); validValsSts[4].Add("FND"); validValsSts[4].Add("Foundation");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@OSRG", "School", "School", BoFieldTypes.db_Alpha, 3, validValsSts, false, 0);

            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("1"); validValsSts[0].Add("Semester 1");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("2"); validValsSts[1].Add("Semester 2");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("10"); validValsSts[2].Add("Annual");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@OSRG", "Semester", "Semester", BoFieldTypes.db_Numeric, 10, validValsSts, false, 0);
            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("New"); validValsSts[0].Add("New");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("Current"); validValsSts[1].Add("Current");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("Transfer"); validValsSts[2].Add("Transfer");
            validValsSts.Add(new List<string>()); validValsSts[3].Add("Retake"); validValsSts[3].Add("Retake");
            validValsSts.Add(new List<string>()); validValsSts[4].Add("Pending"); validValsSts[4].Add("Pending");
            validValsSts.Add(new List<string>()); validValsSts[5].Add("EXT"); validValsSts[5].Add("Extension/Reviva");
            validValsSts.Add(new List<string>()); validValsSts[6].Add("Readmit"); validValsSts[6].Add("Readmission");
            validValsSts.Add(new List<string>()); validValsSts[7].Add("PTDW"); validValsSts[7].Add("Postponed/Terminated");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@OSRG", "Status", "Status", BoFieldTypes.db_Alpha, 20, validValsSts, true, 0);
            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("N"); validValsSts[0].Add("N");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("Y"); validValsSts[1].Add("Y");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@OSRG", "Invoiced", "Invoiced", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);
            CheckAndCreateAlphaNumUserFieldWithValidValues("@OSRG", "FeeCreated", "FeeCreated", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);

            CheckAndCreateDefaultUserField("@SRG1", "SubjectCode", "Subject Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            CheckAndCreateDefaultUserField("@SRG1", "SubjectName", "Subject Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 200, "");
            CheckAndCreateDefaultUserField("@SRG1", "Credits", "Credit Hours", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 10, "");
            CheckAndCreateDefaultUserField("@SRG1", "Price", "Price", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@SRG1", "Discount", "Discount Amount", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@SRG1", "DiscountPC", "Discount Percent", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@SRG1", "Total", "Total", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");

            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("N"); validValsSts[0].Add("N");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("Y"); validValsSts[1].Add("Y");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@SRG1", "ToTrail", "Trail", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);
            CheckAndCreateAlphaNumUserFieldWithValidValues("@SRG1", "ToRetake", "Retake", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);

            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("A"); validValsSts[0].Add("Auto");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("M"); validValsSts[1].Add("Manual");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@SRG1", "LineType", "Line Type", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);

            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("N"); validValsSts[0].Add("N");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("Y"); validValsSts[1].Add("Y");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@SRG1", "IsReTrailed", "Trailed/Retaken", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);//Is.Trailed

            CheckAndCreateAlphaNumUserFieldWithValidValues("@SRG1", "FromReTrail", "From Trail/Retake", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);//From.Trail
            CheckAndCreateDefaultUserField("@SRG1", "ReTrailedIn", "Trailed/Retaken Into", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, "");//Trailed.In
            CheckAndCreateDefaultUserField("@SRG1", "ReTrailedFrom", "Trailed/Retaken From", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, "");//Trailed.From
            CheckAndCreateDefaultUserField("@SRG1", "ReTrailLine", "Base Line", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, "");//Trail.Line



            tables1.Clear();
            ColRech1.Clear();
            udoFormCols1.Clear();
            tables1.Add("SRG1");
            ColRech1.Add("DocEntry");
            ColRech1.Add("U_StudentID");
            ColRech1.Add("U_StudentName");
            ColRech1.Add("U_Level");
            ColRech1.Add("U_ProgramCode");
            ColRech1.Add("U_FiscalYear");
            ColRech1.Add("U_AcademicYear");
            ColRech1.Add("U_StartDate");
            ColRech1.Add("U_EndDate");
            ColRech1.Add("U_TrgtEntry");
            ColRech1.Add("U_TrgtNum");
            udoFormCols1.Add("DocEntry");
            udoFormCols1.Add("U_StudentID");
            udoFormCols1.Add("U_StudentName");
            udoFormCols1.Add("U_Level");
            udoFormCols1.Add("U_ProgramCode");
            udoFormCols1.Add("U_FiscalYear");
            udoFormCols1.Add("U_AcademicYear");
            udoFormCols1.Add("U_StartDate");
            udoFormCols1.Add("U_EndDate");
            udoFormCols1.Add("U_TrgtEntry");
            udoFormCols1.Add("U_TrgtNum");
            CheckAndCreateUDO("SRG", "Registration", BoUDOObjType.boud_Document, "OSRG", BoYesNoEnum.tYES, BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tNO, BoYesNoEnum.tNO, ColRech1, udoFormCols1, tables1);

            CheckAndCreateDefaultUserField("@OING", "FromDate", "FromDate", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, "", BoYesNoEnum.tYES);
            CheckAndCreateDefaultUserField("@OING", "ToDate", "ToDate", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, "", BoYesNoEnum.tYES);
            CheckAndCreateDefaultUserField("@OING", "DocDate", "Doc. Date", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, "", BoYesNoEnum.tYES);
            CheckAndCreateDefaultUserField("@OING", "DueDate", "Due Date", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, "", BoYesNoEnum.tYES);
            CheckAndCreateDefaultUserField("@OING", "ProgramCode", "Program Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "", BoYesNoEnum.tYES);
            CheckAndCreateDefaultUserField("@OING", "Student", "Student ID", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");


            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("-"); validValsSts[0].Add(" ");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("1"); validValsSts[1].Add("Semester 1");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("2"); validValsSts[2].Add("Semester 2");
            validValsSts.Add(new List<string>()); validValsSts[3].Add("10"); validValsSts[3].Add("Annual");

            CheckAndCreateAlphaNumUserFieldWithValidValues("@OING", "Semester", "Semester", BoFieldTypes.db_Alpha, 2, validValsSts, true, 0);


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
            validValsSts.Add(new List<string>()); validValsSts[0].Add("-"); validValsSts[0].Add(" ");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("New"); validValsSts[1].Add("New");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("Current"); validValsSts[2].Add("Current");
            validValsSts.Add(new List<string>()); validValsSts[3].Add("Transfer"); validValsSts[3].Add("Transfer");
            validValsSts.Add(new List<string>()); validValsSts[4].Add("Retake"); validValsSts[4].Add("Retake");
            validValsSts.Add(new List<string>()); validValsSts[5].Add("Pending"); validValsSts[5].Add("Pending");
            validValsSts.Add(new List<string>()); validValsSts[6].Add("EXT"); validValsSts[6].Add("Extension/Reviva");
            validValsSts.Add(new List<string>()); validValsSts[7].Add("Readmit"); validValsSts[7].Add("Readmission");
            validValsSts.Add(new List<string>()); validValsSts[8].Add("PTDW"); validValsSts[8].Add("Postponed/Terminated");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@OING", "Status", "Status", BoFieldTypes.db_Alpha, 20, validValsSts, true, 0);

            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("-"); validValsSts[0].Add(" ");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("SSS"); validValsSts[1].Add("School of Social Sciences");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("SSE"); validValsSts[2].Add("School of Science & Engineering");
            validValsSts.Add(new List<string>()); validValsSts[3].Add("SME"); validValsSts[3].Add("School of Management & Economics");
            validValsSts.Add(new List<string>()); validValsSts[4].Add("SOM"); validValsSts[4].Add("School of Medicine");
            validValsSts.Add(new List<string>()); validValsSts[5].Add("FND"); validValsSts[5].Add("Foundation");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@OING", "School", "School", BoFieldTypes.db_Alpha, 3, validValsSts, true, 0);

            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("Y"); validValsSts[0].Add("Y");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("N"); validValsSts[1].Add("N");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@ING1", "Select", "Select", BoFieldTypes.db_Alpha, 10, validValsSts, false, 0);
            CheckAndCreateAlphaNumUserFieldWithValidValues("@ING1", "Cancelled", "Cancelled", BoFieldTypes.db_Alpha, 1, validValsSts, true, 1);

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
            CheckAndCreateAlphaNumUserFieldWithValidValues("@ING1", "IsManual", "Is Manual", BoFieldTypes.db_Alpha, 10, validValsSts, false, 0);
            CheckAndCreateAlphaNumUserFieldWithValidValues("@ING1", "DocCancel", "Document Cancelled", BoFieldTypes.db_Alpha, 1, validValsSts, false, 0);
            CheckAndCreateAlphaNumUserFieldWithValidValues("@ING1", "IsVoucher", "Voucher Created", BoFieldTypes.db_Alpha, 10, validValsSts, false, 0);
            CheckAndCreateDefaultUserField("@ING1", "StudentID", "Student ID", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            CheckAndCreateDefaultUserField("@ING1", "StudentName", "Student Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");
            CheckAndCreateDefaultUserField("@ING1", "Level", "Level", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@ING1", "ProgramCode", "Program Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@ING1", "FiscalYear", "Fiscal Year", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@ING1", "AcademicYear", "Academic Year", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@ING1", "School", "School", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("New"); validValsSts[0].Add("New");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("Current"); validValsSts[1].Add("Current");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("Transfer"); validValsSts[2].Add("Transfer");
            validValsSts.Add(new List<string>()); validValsSts[3].Add("Retake"); validValsSts[3].Add("Retake");
            validValsSts.Add(new List<string>()); validValsSts[4].Add("Pending"); validValsSts[4].Add("Pending");
            validValsSts.Add(new List<string>()); validValsSts[5].Add("EXT"); validValsSts[5].Add("Extension/Reviva");
            validValsSts.Add(new List<string>()); validValsSts[6].Add("Readmit"); validValsSts[6].Add("Readmission");
            validValsSts.Add(new List<string>()); validValsSts[7].Add("PTDW"); validValsSts[7].Add("Postponed/Terminated");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@ING1", "Status", "Status", BoFieldTypes.db_Alpha, 20, validValsSts, false, 0);
            CheckAndCreateDefaultUserField("@ING1", "ItemCode", "Item No.", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            CheckAndCreateDefaultUserField("@ING1", "SalesUom", "Sales Uom", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@ING1", "Price", "Price", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "AppliedPrice", "Applied Price", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "HDiscount", "Reg. Discount", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "LDiscount", "Course Discount", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "TaxCode", "Tax Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@ING1", "TaxAmount", "Tax Amount", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "TaxRate", "Tax Rate", BoFieldTypes.db_Float, BoFldSubTypes.st_Percentage, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "RegNo", "Register No.", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "Remarks", "Notes", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 250, "");
            CheckAndCreateDefaultUserField("@ING1", "InvEntry", "Invoice Entry", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "InvNum", "Invoice No.", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "DocTotal", "Document Total", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "AfterDiscount", "After Discount", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "MemoEntry", "Memo Entry", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "MemoNum", "Memo No.", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "TrailCount", "Trail Count", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "TrailAmount", "Trail Amount", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "BaseLine", "Base Line", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "BaseDoc", "Base Doc", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "CancelEntry", "Cancel Entry", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "CancelNum", "Cancel No.", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@ING1", "AcctCode", "Account Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 20, "");
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
            ColRech1.Add("U_ProgramCode");

            udoFormCols1.Add("DocEntry");
            udoFormCols1.Add("CreateDate");
            udoFormCols1.Add("U_FromDate");
            udoFormCols1.Add("U_ToDate");
            udoFormCols1.Add("U_ProgramCode");
            CheckAndCreateUDO("ING", "Student_Invoices", BoUDOObjType.boud_Document, "OING", BoYesNoEnum.tYES, BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tNO, BoYesNoEnum.tNO, ColRech1, udoFormCols1, tables1);



            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("Y"); validValsSts[0].Add("Y");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("N"); validValsSts[1].Add("N");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@TING1", "Select", "Select", BoFieldTypes.db_Alpha, 10, validValsSts, false, 0);
            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("N"); validValsSts[0].Add("N");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("Y"); validValsSts[1].Add("Y");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@TING1", "Invoiced", "Invoiced", BoFieldTypes.db_Alpha, 10, validValsSts, false, 0);
            CheckAndCreateAlphaNumUserFieldWithValidValues("@TING1", "IsManual", "Is Manual", BoFieldTypes.db_Alpha, 10, validValsSts, false, 0);

            CheckAndCreateAlphaNumUserFieldWithValidValues("@TING1", "IsVoucher", "Voucher Created", BoFieldTypes.db_Alpha, 10, validValsSts, false, 0);
            CheckAndCreateAlphaNumUserFieldWithValidValues("@TING1", "Cancelled", "Cancelled", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);
            CheckAndCreateAlphaNumUserFieldWithValidValues("@TING1", "DocCancel", "Document Cancelled", BoFieldTypes.db_Alpha, 1, validValsSts, false, 0);

            CheckAndCreateDefaultUserField("@TING1", "StudentID", "Student ID", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            CheckAndCreateDefaultUserField("@TING1", "StudentName", "Student Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");
            CheckAndCreateDefaultUserField("@TING1", "Level", "Level", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@TING1", "ProgramCode", "Program Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@TING1", "FiscalYear", "Fiscal Year", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@TING1", "AcademicYear", "Academic Year", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@TING1", "School", "School", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("New"); validValsSts[0].Add("New");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("Current"); validValsSts[1].Add("Current");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("Transfer"); validValsSts[2].Add("Transfer");
            validValsSts.Add(new List<string>()); validValsSts[3].Add("Retake"); validValsSts[3].Add("Retake");
            validValsSts.Add(new List<string>()); validValsSts[4].Add("Pending"); validValsSts[4].Add("Pending");
            validValsSts.Add(new List<string>()); validValsSts[5].Add("EXT"); validValsSts[5].Add("Extension/Reviva");
            validValsSts.Add(new List<string>()); validValsSts[6].Add("Readmit"); validValsSts[6].Add("Readmission");
            validValsSts.Add(new List<string>()); validValsSts[7].Add("PTDW"); validValsSts[7].Add("Postponed/Terminated");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@TING1", "Status", "Status", BoFieldTypes.db_Alpha, 20, validValsSts, false, 0);
            CheckAndCreateDefaultUserField("@TING1", "ItemCode", "Item No.", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            CheckAndCreateDefaultUserField("@TING1", "SalesUom", "Sales Uom", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@TING1", "Price", "Price", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "HDiscount", "Reg. Discount", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "LDiscount", "Course Discount", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "AppliedPrice", "Applied Price", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "TaxCode", "Tax Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, "");
            CheckAndCreateDefaultUserField("@TING1", "TaxAmount", "Tax Amount", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "TaxRate", "Tax Rate", BoFieldTypes.db_Float, BoFldSubTypes.st_Percentage, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "RegNo", "Register No.", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "Remarks", "Notes", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 250, "");
            CheckAndCreateDefaultUserField("@TING1", "InvEntry", "Invoice Entry", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "InvNum", "Invoice No.", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "DocTotal", "Document Total", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "AfterDiscount", "After Discount", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "MemoEntry", "Memo Entry", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "MemoNum", "Memo No.", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "TrailCount", "Trail Count", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "TrailAmount", "Trail Amount", BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "BaseLine", "Base Line", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "BaseDoc", "Base Doc", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "CancelEntry", "Cancel Entry", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "CancelNum", "Cancel No.", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            CheckAndCreateDefaultUserField("@TING1", "AcctCode", "Account Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 20, "");
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

            validValsSts.Clear();
            validValsSts.Add(new List<string>()); validValsSts[0].Add("-"); validValsSts[0].Add(" ");
            validValsSts.Add(new List<string>()); validValsSts[1].Add("R"); validValsSts[1].Add("Refund");
            validValsSts.Add(new List<string>()); validValsSts[2].Add("N"); validValsSts[2].Add("Non-Refund");
            validValsSts.Add(new List<string>()); validValsSts[3].Add("C"); validValsSts[3].Add("Cancel");
            validValsSts.Add(new List<string>()); validValsSts[4].Add("M"); validValsSts[4].Add("Manual");
            CheckAndCreateAlphaNumUserFieldWithValidValues("@TING1", "CancelType", "Cancellation Type", BoFieldTypes.db_Alpha, 1, validValsSts, false, 0);
            Config.LoadConfig();
            //validValsSts.Clear();
            //validValsSts.Add(new List<string>()); validValsSts[0].Add("4"); validValsSts[0].Add("Item");
            //validValsSts.Add(new List<string>()); validValsSts[1].Add("290"); validValsSts[1].Add("Resource");
            //CheckAndCreateAlphaNumUserFieldWithValidValues("@AC_AMD1", "AC_ItemType", "Item Type", BoFieldTypes.db_Alpha, 3, validValsSts, true, 0);

            //validValsSts.Clear();
            //validValsSts.Add(new List<string>()); validValsSts[0].Add("N"); validValsSts[0].Add("N");
            //validValsSts.Add(new List<string>()); validValsSts[1].Add("Y"); validValsSts[1].Add("Y");
            //CheckAndCreateAlphaNumUserFieldWithValidValues("@AC_OAMD", "AC_Tray", "Tray", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);

            //CheckAndCreateDefaultUserField("@AC_AMD1", "AC_Quantity", "Quantity", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 11, "");
            //CheckAndCreateDefaultUserField("@AC_AMD1", "AC_AttCode", "Attribute Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_AMD1", "AC_AttName", "Attribute Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_AMD1", "AC_OptCode", "Option Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_AMD1", "AC_OptName", "Option Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");
            //CheckAndCreateDefaultUserField("@AC_AMD1", "AC_AttCode2", "Attribute Code 2", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_AMD1", "AC_AttName2", "Attribute Name 2", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_AMD1", "AC_OptCode2", "Option Code 2", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_AMD1", "AC_OptName2", "Option Name 2", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");
            //CheckAndCreateDefaultUserField("@AC_AMD1", "AC_AttCode3", "Attribute Code 3", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_AMD1", "AC_AttName3", "Attribute Name 3", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_AMD1", "AC_OptCode3", "Option Code 3", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_AMD1", "AC_OptName3", "Option Name 3", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");
            //CheckAndCreateDefaultUserField("@AC_AMD1", "AC_AttCode4", "Attribute Code4", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_AMD1", "AC_AttName4", "Attribute Name4", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_AMD1", "AC_OptCode4", "Option Code4", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_AMD1", "AC_OptName4", "Option Name4", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");
            //CheckAndCreateDefaultUserField("@AC_AMD1", "AC_Color", "Color", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_AMD1", "AC_ColName", "Color Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");
            //CheckAndCreateDefaultUserField("@AC_AMD1", "AC_Size", "Size", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_AMD1", "AC_SizeName", "Size Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");
            //CheckAndCreateDefaultUserField("@AC_AMD1", "AC_Category", "Category", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_AMD1", "AC_CatName", "Category Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //validValsSts.Clear();
            //validValsSts.Add(new List<string>()); validValsSts[0].Add("N"); validValsSts[0].Add("None");
            //validValsSts.Add(new List<string>()); validValsSts[1].Add("A"); validValsSts[1].Add("And");
            //validValsSts.Add(new List<string>()); validValsSts[2].Add("O"); validValsSts[2].Add("Or");
            //CheckAndCreateAlphaNumUserFieldWithValidValues("@AC_AMD1", "AC_DType", "Dependent Type", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);
            //validValsSts.Clear();
            //validValsSts.Add(new List<string>()); validValsSts[0].Add("Y"); validValsSts[0].Add("Y");
            //validValsSts.Add(new List<string>()); validValsSts[1].Add("N"); validValsSts[1].Add("N");
            //CheckAndCreateAlphaNumUserFieldWithValidValues("@AC_OAMD", "AC_Dependent", "Dependent", BoFieldTypes.db_Alpha, 1, validValsSts, true, 1);
            //CheckAndCreateAlphaNumUserFieldWithValidValues("@AC_OAMD", "AC_Variable", "Variable Qty", BoFieldTypes.db_Alpha, 1, validValsSts, true, 1);

            //validValsSts.Clear();
            //validValsSts.Add(new List<string>()); validValsSts[0].Add("Y"); validValsSts[0].Add("Y");
            //validValsSts.Add(new List<string>()); validValsSts[1].Add("N"); validValsSts[1].Add("N");
            //CheckAndCreateAlphaNumUserFieldWithValidValues("@AC_OAMD", "AC_Status", "Status", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);
            //CheckAndCreateAlphaNumUserFieldWithValidValues("@AC_OAMD", "AC_Mandatory", "Status", BoFieldTypes.db_Alpha, 1, validValsSts, true, 1);
            //CheckAndCreateDefaultUserField("@AC_OAMD", "AC_Group", "Group", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_OAMD", "AC_Category", "Category", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");

            ////validValsSts.Clear();
            ////validValsSts.Add(new List<string>()); validValsSts[0].Add("O"); validValsSts[0].Add("Optional");
            ////validValsSts.Add(new List<string>()); validValsSts[1].Add("M"); validValsSts[1].Add("Mandatory");
            ////CheckAndCreateAlphaNumUserFieldWithValidValues("@AC_OAMD", "AC_Type", "Attribute Type", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);
            //validValsSts.Clear();
            //validValsSts.Add(new List<string>()); validValsSts[0].Add("Y"); validValsSts[0].Add("Active");
            //validValsSts.Add(new List<string>()); validValsSts[1].Add("N"); validValsSts[1].Add("Inactive");
            //CheckAndCreateAlphaNumUserFieldWithValidValues("@AC_AMD1", "AC_Active", "Active", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);
            //validValsSts.Clear();
            //validValsSts.Add(new List<string>()); validValsSts[0].Add("Y"); validValsSts[0].Add("Y");
            //validValsSts.Add(new List<string>()); validValsSts[1].Add("N"); validValsSts[1].Add("N");
            //CheckAndCreateAlphaNumUserFieldWithValidValues("@AC_AMD1", "AC_Dependent", "Dependent", BoFieldTypes.db_Alpha, 1, validValsSts, true, 1);
            //List<string> tables1 = new List<string>();
            //List<string> udoFormCols1 = new List<string>();
            //List<string> ColRech1 = new List<string>();



            //tables1.Add("AC_AMD1");
            //ColRech1.Add("DocEntry");
            //ColRech1.Add("U_AC_Name");
            //ColRech1.Add("U_AC_Mandatory");
            //ColRech1.Add("U_AC_Group");
            //CheckAndCreateUDO("AC_AMD", "Attributes Data", BoUDOObjType.boud_Document, "AC_OAMD", BoYesNoEnum.tYES, BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tNO, ColRech1, udoFormCols1, tables1);

            //CheckAndCreateDefaultUserField("@AC_OSPR", "AC_AttCode", "Attribute Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_OSPR", "AC_AttName", "Attribute Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_OSPR", "AC_Remark", "Remark", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");
            //CheckAndCreateDefaultUserField("@AC_OSPR", "AC_Label", "Label", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");


            //tables1.Clear();
            //ColRech1.Clear();
            //udoFormCols1.Clear();

            //udoFormCols1.Add("Code");
            //ColRech1.Add("Code");
            //udoFormCols1.Add("U_AC_AttCode");
            //ColRech1.Add("U_AC_AttCode");
            //udoFormCols1.Add("U_AC_AttName");
            //ColRech1.Add("U_AC_AttName");
            //udoFormCols1.Add("U_AC_Remark");
            //ColRech1.Add("U_AC_Remark");
            //CheckAndCreateUDO("AC_SPR", "Remarks", BoUDOObjType.boud_MasterData, "AC_OSPR", BoYesNoEnum.tYES, BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tYES, BoYesNoEnum.tYES, ColRech1, udoFormCols1, BoYesNoEnum.tNO, BoYesNoEnum.tYES, "Remarks", "AC_VC12");


            //CheckAndCreateDefaultUserField("@AC_OVAR", "AC_ItemCode", "ItemCode", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "", BoYesNoEnum.tYES);
            //CheckAndCreateDefaultUserField("@AC_OVAR", "AC_ItemName", "ItemName", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");
            //CheckAndCreateDefaultUserField("@AC_OVAR", "AC_BaseEntry", "BaseEntry", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            //CheckAndCreateDefaultUserField("@AC_OVAR", "AC_BaseNum", "BaseNum", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "", BoYesNoEnum.tYES);
            //CheckAndCreateDefaultUserField("@AC_OVAR", "AC_BaseLine", "BaseLine", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            //CheckAndCreateDefaultUserField("@AC_OVAR", "AC_DocDate", "Document Date", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, "");
            //CheckAndCreateDefaultUserField("@AC_OVAR", "AC_GroupNo", "Variant Group No.", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "", BoYesNoEnum.tYES);
            //CheckAndCreateDefaultUserField("@AC_OVAR", "AC_GroupName", "Variant Group Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");
            //CheckAndCreateDefaultUserField("@AC_OVAR", "AC_ProdNo", "Production Order No.", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_OVAR", "AC_Summary", "Summary", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 250, "");
            //CheckAndCreateDefaultUserField("@AC_OVAR", "AC_AttOpt", "Attributes and Options", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");
            ////   CheckAndCreateUserKey("IX_0", "@AC_OVAR", "AC_AttOpt", false);

            //CheckAndCreateDefaultUserField("@AC_OVAR", "AC_BasePrice", "Unit Price", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");

            //validValsSts.Clear();
            //validValsSts.Add(new List<string>()); validValsSts[0].Add("Y"); validValsSts[0].Add("Y");
            //validValsSts.Add(new List<string>()); validValsSts[1].Add("N"); validValsSts[1].Add("N");
            //CheckAndCreateAlphaNumUserFieldWithValidValues("@AC_OVAR", "AC_Finalized", "Finalized", BoFieldTypes.db_Alpha, 1, validValsSts, true, 1);


            //CheckAndCreateDefaultUserField("@AC_VAR1", "AC_AttCode", "Attribute Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_VAR1", "AC_AttName", "Attribute Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_VAR1", "AC_OptCode", "Option Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_VAR1", "AC_CCode", "Color Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_VAR1", "AC_Color", "Color", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_VAR1", "AC_BCode", "Bay Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            ////CheckAndCreateDefaultUserField("@AC_VAR1", "AC_Bay", "Bay", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_VAR1", "AC_SCode", "Size Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_VAR1", "AC_Size", "Size", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_VAR1", "AC_OptName", "Attribute Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_VAR1", "AC_BaseLine", "BaseLine", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_VAR1", "AC_Quantity", "Quantity", BoFieldTypes.db_Numeric, BoFldSubTypes.st_Quantity, 11, "");
            //CheckAndCreateDefaultUserField("@AC_VAR1", "AC_BQty", "Bay Quantity", BoFieldTypes.db_Numeric, BoFldSubTypes.st_Quantity, 11, "");
            //CheckAndCreateDefaultUserField("@AC_VAR1", "AC_GroupNo", "Group No", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_VAR1", "AC_Label", "Label", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");
            //CheckAndCreateDefaultUserField("@AC_VAR1", "AC_Category", "Category", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_VAR1", "AC_UnitPrice", "Unit Price", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 11, "");
            //CheckAndCreateDefaultUserField("@AC_VAR1", "AC_GroupNo", "Group No", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_VAR1", "AC_RemCode", "Remark Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");


            //validValsSts.Clear();
            //validValsSts.Add(new List<string>()); validValsSts[0].Add("Y"); validValsSts[0].Add("Y");
            //validValsSts.Add(new List<string>()); validValsSts[1].Add("N"); validValsSts[1].Add("N");
            //CheckAndCreateAlphaNumUserFieldWithValidValues("@AC_VAR1", "AC_Mandatory", "Mandatory", BoFieldTypes.db_Alpha, 1, validValsSts, true, 1);
            //CheckAndCreateAlphaNumUserFieldWithValidValues("@AC_VAR1", "AC_Dependent", "Dependent", BoFieldTypes.db_Alpha, 1, validValsSts, true, 1);
            //CheckAndCreateAlphaNumUserFieldWithValidValues("@AC_VAR1", "AC_Variable", "Variable", BoFieldTypes.db_Alpha, 1, validValsSts, true, 1);
            //CheckAndCreateAlphaNumUserFieldWithValidValues("@AC_VAR1", "AC_Special", "Special", BoFieldTypes.db_Alpha, 1, validValsSts, true, 1);
            //CheckAndCreateDefaultUserField("@AC_VAR1", "AC_SpRemarks", "Special Remarks", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");




            //tables1.Clear();
            //ColRech1.Clear();
            //tables1.Add("AC_VAR1");
            //ColRech1.Add("DocEntry");
            ////ColRech1.Add("U_AC_Code");
            ////ColRech1.Add("U_AC_Name");
            //ColRech1.Add("U_AC_BaseEntry");
            //ColRech1.Add("U_AC_BaseNum");
            //ColRech1.Add("U_AC_BaseLine");
            //CheckAndCreateUDO("AC_VAR", "Variants", BoUDOObjType.boud_Document, "AC_OVAR", BoYesNoEnum.tYES, BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tYES, ColRech1, udoFormCols1, tables1);



            //CheckAndCreateDefaultUserField("@AC_OATG", "AC_Name", "Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //validValsSts.Clear();
            //validValsSts.Add(new List<string>()); validValsSts[0].Add("Y"); validValsSts[0].Add("Y");
            //validValsSts.Add(new List<string>()); validValsSts[1].Add("N"); validValsSts[1].Add("N");
            //CheckAndCreateAlphaNumUserFieldWithValidValues("@AC_OATG", "AC_Status", "Status", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);
            //CheckAndCreateDefaultUserField("@AC_ATG1", "AC_AttCode", "Attribute Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_ATG1", "AC_AttName", "Attribute Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_ATG1", "AC_CatCode", "Category Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_ATG1", "AC_CatName", "Category Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateAlphaNumUserFieldWithValidValues("@AC_ATG1", "AC_Dependent", "Dependent", BoFieldTypes.db_Alpha, 1, validValsSts, false, 0);
            //validValsSts.Clear();
            //validValsSts.Add(new List<string>()); validValsSts[0].Add("O"); validValsSts[0].Add("Optional");
            //validValsSts.Add(new List<string>()); validValsSts[1].Add("M"); validValsSts[1].Add("Mandatory");
            //CheckAndCreateAlphaNumUserFieldWithValidValues("@AC_ATG1", "AC_Type", "Type", BoFieldTypes.db_Alpha, 1, validValsSts, true, 0);
            //CheckAndCreateDefaultUserField("@AC_ATG1", "AC_Add", "Additional Field", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //validValsSts.Clear();
            //validValsSts.Add(new List<string>()); validValsSts[0].Add("N"); validValsSts[0].Add("N");
            //validValsSts.Add(new List<string>()); validValsSts[1].Add("Y"); validValsSts[1].Add("Y");
            //CheckAndCreateAlphaNumUserFieldWithValidValues("@AC_ATG1", "AC_Carcass", "Carcass", BoFieldTypes.db_Alpha, 1, validValsSts, false, 0);
            //CheckAndCreateAlphaNumUserFieldWithValidValues("@AC_ATG1", "AC_Frame", "Frame", BoFieldTypes.db_Alpha, 1, validValsSts, false, 0);

            //tables1.Clear();
            //ColRech1.Clear();
            //tables1.Add("AC_ATG1");
            //ColRech1.Add("Code");
            //ColRech1.Add("U_AC_Name");
            //CheckAndCreateUDO("AC_ATG", "Attribute Groups", BoUDOObjType.boud_MasterData, "AC_OATG", BoYesNoEnum.tYES, BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tNO, ColRech1, udoFormCols1, tables1);


            //tables1.Clear();
            //ColRech1.Clear();
            //validValsSts.Clear();
            //CheckAndCreateDefaultUserField("@AC_VCG1", "AC_BaseCode", "Base Option Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_VCG1", "AC_BaseName", "Base Option Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_VCG1", "AC_AttCode", "Target Attribute Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_VCG1", "AC_AttName", "Target Attribute Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_VCG1", "AC_OptCode", "Target Option Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_VCG1", "AC_OptName", "Target Option Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");

            //tables1.Add("AC_VCG1");
            //ColRech1.Add("Code");
            //ColRech1.Add("Name");
            //CheckAndCreateUDO("AC_VCG", "Attribute Configuration", BoUDOObjType.boud_MasterData, "AC_OVCG", BoYesNoEnum.tYES, BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tNO, ColRech1, udoFormCols1, tables1);

            //tables1.Clear();
            //ColRech1.Clear();
            //udoFormCols1.Clear();

            //udoFormCols1.Add("Code");
            //udoFormCols1.Add("Name");
            //ColRech1.Add("Code");
            //ColRech1.Add("Name");
            //CheckAndCreateUDO("AC_CAT", "Category Master", BoUDOObjType.boud_MasterData, "AC_OCAT", BoYesNoEnum.tYES, BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tYES, BoYesNoEnum.tYES, ColRech1, udoFormCols1, BoYesNoEnum.tNO, BoYesNoEnum.tYES, "Categories", "AC_VC7");


            //tables1.Clear();
            //ColRech1.Clear();
            //udoFormCols1.Clear();

            //udoFormCols1.Add("Code");
            //udoFormCols1.Add("Name");
            //ColRech1.Add("Code");
            //ColRech1.Add("Name");
            //CheckAndCreateUDO("AC_LBL", "Labels", BoUDOObjType.boud_MasterData, "AC_OLBL", BoYesNoEnum.tYES, BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tYES, BoYesNoEnum.tYES, ColRech1, udoFormCols1, BoYesNoEnum.tNO, BoYesNoEnum.tYES, "Lables", "AC_VC11");


            //validValsSts.Clear();
            //validValsSts.Add(new List<string>()); validValsSts[0].Add("Y"); validValsSts[0].Add("Y");
            //validValsSts.Add(new List<string>()); validValsSts[1].Add("N"); validValsSts[1].Add("N");
            //CheckAndCreateAlphaNumUserFieldWithValidValues("@AC_OOPT", "AC_Color", "Color", BoFieldTypes.db_Alpha, 1, validValsSts, false, 1);
            //CheckAndCreateAlphaNumUserFieldWithValidValues("@AC_OOPT", "AC_Size", "Size", BoFieldTypes.db_Alpha, 1, validValsSts, false, 1);
            //CheckAndCreateAlphaNumUserFieldWithValidValues("@AC_OOPT", "AC_Bay", "Bay", BoFieldTypes.db_Alpha, 1, validValsSts, false, 1);

            //CheckAndCreateFieldWithLinkedUDO("@AC_OOPT", "AC_Category", "Category", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "AC_OCAT", BoYesNoEnum.tNO);

            //tables1.Clear();
            //ColRech1.Clear();
            //udoFormCols1.Clear();
            //udoFormCols1.Add("Code");
            //udoFormCols1.Add("Name");
            //udoFormCols1.Add("U_AC_Category");
            //udoFormCols1.Add("U_AC_Color");
            //udoFormCols1.Add("U_AC_Size");
            //ColRech1.Add("Code");
            //ColRech1.Add("Name");
            //ColRech1.Add("U_AC_Category");
            //ColRech1.Add("U_AC_Color");
            //ColRech1.Add("U_AC_Size");
            //CheckAndCreateUDO("AC_OPT", "Option Master", BoUDOObjType.boud_MasterData, "AC_OOPT", BoYesNoEnum.tYES, BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tYES, BoYesNoEnum.tYES, ColRech1, udoFormCols1, BoYesNoEnum.tNO, BoYesNoEnum.tYES, "Option Master", "AC_VC6");

            //string queryID = "";

            //CheckAndCreateDefaultUserField("RDR1", "AC_ExtRemarks", "External Remarks", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 100, "");
            //CheckAndCreateDefaultUserField("RDR1", "AC_Labels", "Labels", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 100, "");
            //CheckAndCreateDefaultUserField("RDR1", "AC_Variant", "Variant Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("RDR1", "INIP", "Initial Price", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 10, "");
            //validValsSts.Clear();
            //validValsSts.Add(new List<string>()); validValsSts[0].Add("Y"); validValsSts[0].Add("Y");
            //validValsSts.Add(new List<string>()); validValsSts[1].Add("N"); validValsSts[1].Add("N");
            //CheckAndCreateAlphaNumUserFieldWithValidValues("RDR1", "AC_Created", "PO Created", BoFieldTypes.db_Alpha, 1, validValsSts, false, 1);
            //validValsSts.Clear();
            //validValsSts.Add(new List<string>()); validValsSts[0].Add("Stock"); validValsSts[0].Add("Showroom Stock");
            //validValsSts.Add(new List<string>()); validValsSts[1].Add("Production"); validValsSts[1].Add("To Be Produced");
            //CheckAndCreateAlphaNumUserFieldWithValidValues("RDR1", "AC_OType", "Manufacturing Type", BoFieldTypes.db_Alpha, 10, validValsSts, false, 1);
            //CheckAndCreateAlphaNumUserFieldWithValidValues("ITT1", "AC_VarAp", "Variant Applicable", BoFieldTypes.db_Alpha, 1, validValsSts, false, 1);
            //CheckAndCreateAlphaNumUserFieldWithValidValues("ITT1", "AC_Template", "Template BoM", BoFieldTypes.db_Alpha, 1, validValsSts, false, 1);
            //CheckAndCreateDefaultUserField("OWTQ", "AC_SONum", "Sales Order", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateAlphaNumUserFieldWithValidValues("OWTQ", "AC_OQty", "Overall Quantity", BoFieldTypes.db_Alpha, 1, validValsSts, false, 1);
            //CheckAndCreateDefaultUserField("OWTQ", "AC_ItemCode", "FG Item Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");

            ////CheckAndCreateDefaultUserField("ITT1", "AC_VarGrp", "Variant Group", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 11, "");
            ////CheckAndCreateDefaultUserField("ITT1", "AC_AttCode", "Attribute Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            ////CheckAndCreateDefaultUserField("ITT1", "AC_AttName", "Attribute Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            ////CheckAndCreateDefaultUserField("ITT1", "AC_OptCode", "Option Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            ////CheckAndCreateDefaultUserField("ITT1", "AC_OptName", "Option Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //validValsSts.Clear();
            //validValsSts.Add(new List<string>()); validValsSts[0].Add("D"); validValsSts[0].Add("Direct");
            //validValsSts.Add(new List<string>()); validValsSts[1].Add("I"); validValsSts[1].Add("Indirect");
            //CheckAndCreateAlphaNumUserFieldWithValidValues("OITM", "AC_MType", "Material Type", BoFieldTypes.db_Alpha, 1, validValsSts, false, 0);
            //validValsSts.Clear();
            //validValsSts.Add(new List<string>()); validValsSts[0].Add("I"); validValsSts[0].Add("Item");
            //validValsSts.Add(new List<string>()); validValsSts[1].Add("C"); validValsSts[1].Add("Carcass");
            //validValsSts.Add(new List<string>()); validValsSts[2].Add("F"); validValsSts[2].Add("Frame");
            //CheckAndCreateAlphaNumUserFieldWithValidValues("OITM", "AC_Label", "Item Label", BoFieldTypes.db_Alpha, 1, validValsSts, false, 0);

            //CheckAndCreateDefaultUserField("OITM", "AC_Group", "Attribute Group", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("OWOR", "AC_Variant", "Selected Variant", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("OWOR", "AC_FGCode", "Finished Goods", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("OWOR", "AC_SFGCode", "Semi Finished Goods", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");

            //CheckAndCreateDefaultUserField("OWOR", "AC_SOLine", "SO Line", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("OWOR", "AC_SpRemarks", "Special Remarks", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 500, "");


            //CheckAndCreateDefaultUserField("WOR1", "AC_Variant", "Variant", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            //CheckAndCreateDefaultUserField("WOR1", "AC_Quantity", "Variant", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 11, "");

            //CheckAndCreateDefaultUserField("WOR1", "AC_Setup", "Setup Time", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 11, "");
            //CheckAndCreateDefaultUserField("WOR1", "AC_Machine", "Machine Time", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 11, "");
            //CheckAndCreateDefaultUserField("WOR1", "AC_Labour", "Labour Time", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 11, "");
            //CheckAndCreateDefaultUserField("ITT1", "AC_Setup", "Setup Time", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 11, "");

            //CheckAndCreateDefaultUserField("ITT1", "AC_Machine", "Machine Time", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 11, "");
            //CheckAndCreateDefaultUserField("ITT1", "AC_Labour", "Labour Time", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 11, "");
            //InsertRecords("1", "Color", "AC_CAT", "@AC_OCAT");
            //InsertRecords("2", "Size", "AC_CAT", "@AC_OCAT");


            //validValsSts.Clear();
            //validValsSts.Add(new List<string>()); validValsSts[0].Add("1"); validValsSts[0].Add("1");
            //validValsSts.Add(new List<string>()); validValsSts[1].Add("2"); validValsSts[1].Add("2");
            //validValsSts.Add(new List<string>()); validValsSts[2].Add("3"); validValsSts[2].Add("3");
            //validValsSts.Add(new List<string>()); validValsSts[3].Add("4"); validValsSts[3].Add("4");
            //validValsSts.Add(new List<string>()); validValsSts[4].Add("5"); validValsSts[4].Add("5");
            //validValsSts.Add(new List<string>()); validValsSts[5].Add("6"); validValsSts[5].Add("6");
            //validValsSts.Add(new List<string>()); validValsSts[6].Add("7"); validValsSts[6].Add("7");
            //validValsSts.Add(new List<string>()); validValsSts[7].Add("8"); validValsSts[7].Add("8");
            //validValsSts.Add(new List<string>()); validValsSts[8].Add("9"); validValsSts[8].Add("9");
            //validValsSts.Add(new List<string>()); validValsSts[9].Add("10"); validValsSts[9].Add("10");
            //CheckAndCreateAlphaNumUserFieldWithValidValues("ORSC", "AC_Priority", "Resource Priority", BoFieldTypes.db_Numeric, 2, validValsSts, false, 1);
            //CheckAndCreateDefaultUserField("ORSC", "AC_ResourceGrp", "Resource Group", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "AC_ORSG");

            if (Comp_DI.DbServerType == BoDataServerTypes.dst_HANADB)
            {
                CreateUserQuery("CreateRevCode", "Select CONCAT($[@ORMP.U_Level],$[@ORMP.U_School],$[@ORMP.U_MapType])  from dummy", -1);
            }
            else
            {
                CreateUserQuery("CreateRevCode", "Select CONCAT($[@ORMP.U_Level],$[@ORMP.U_School],$[@ORMP.U_MapType])", -1);
            }
            //CreateFormattedSearch("RMP", "3", "Code", BoFormattedSearchActionEnum.bofsaQuery, GetQueryID("CreateRevCode", -1), BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tNO, new string[] { "U_Level", "U_School" });

            CreateUserQuery("GetDistinctPrograms", "Select DISTINCT \"U_PC\"  from \"@OSCL\"", -1);
            CreateFormattedSearch("SM_One.GenerateInvoices", "etProg", "-1", BoFormattedSearchActionEnum.bofsaQuery, GetQueryID("GetDistinctPrograms", -1), BoYesNoEnum.tYES, BoYesNoEnum.tNO, BoYesNoEnum.tNO, new string[] { });

            ////CreateUserQuery("GetOptName", "Select \"Name\" from \"@AC_OOPT\" where \"Code\" = $[$Item_0.DOptCode.0]", -1);
            ////CreateFormattedSearch("VS_VC.Attributes", "Item_0", "DOptName", BoFormattedSearchActionEnum.bofsaQuery, GetQueryID("GetOptName", -1), BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tNO, "DOptCode");

            ////CreateUserQuery("GetOptionsForAttribute2", "Select \"U_AC_Option\" as \"Code\",\"U_AC_Name\" as \"Name\" from \"@AC_AMD1\" where \"DocEntry\" = $[$Item_0.AttCode.0]", -1);
            ////CreateFormattedSearch("VS_VC.Attributes", "Item_0", "DOptCode", BoFormattedSearchActionEnum.bofsaQuery, GetQueryID("GetOptionsForAttribute", -1), BoYesNoEnum.tNO, BoYesNoEnum.tNO, BoYesNoEnum.tNO, "");

            ////CreateUserQuery("GetOptName2", "Select \"Name\" from \"@AC_OOPT\" where \"Code\" = $[$Item_0.DOptCode.0]", -1);
            ////CreateFormattedSearch("VS_VC.Attributes", "Item_0", "DOptName", BoFormattedSearchActionEnum.bofsaQuery, GetQueryID("GetOptName2", -1), BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tNO, "DOptCode");

            //CreateUserQuery("GetBays", "Select \"Code\",\"Name\" from \"@AC_OOPT\" where \"U_AC_Bay\" = 'Y'", -1);
            //CreateFormattedSearch("VS_VC.Variants", "Item_0", "BayCode", BoFormattedSearchActionEnum.bofsaQuery, GetQueryID("GetBays", -1), BoYesNoEnum.tNO, BoYesNoEnum.tNO, BoYesNoEnum.tNO, "");

            ////CreateUserQuery("GetBayName", "Select \"Name\" from \"@AC_OOPT\" where \"Code\" = $[$Item_0.BayCode.0]", -1);
            ////CreateFormattedSearch("VS_VC.Variants", "Item_0", "Bay", BoFormattedSearchActionEnum.bofsaQuery, GetQueryID("GetBayName", -1), BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tNO, "BayCode");

            //CreateUserQuery("GetGroupForItem", "Select T1.\"DocEntry\",T1.\"U_AC_AttCode\",T1.\"U_AC_AttName\", T1.\"U_AC_OptCode\",T1.\"U_AC_OptName\",T1.\"U_AC_Quantity\"  from \"@AC_IAC1\" T1 inner join \"@AC_OIAC\" T0 on T1.\"DocEntry\" = T0.\"DocEntry\" where T0.\"U_AC_Code\" = $[$3.1.0] and T1.\"U_AC_Active\" = 'Y'", -1);
            //CreateFormattedSearch("672", "3", "U_AC_VarGrp", BoFormattedSearchActionEnum.bofsaQuery, GetQueryID("GetGroupForItem", -1), BoYesNoEnum.tNO, BoYesNoEnum.tNO, BoYesNoEnum.tNO, "");



            //CreateUserQuery("GetUnitforRDR", "Select top 1 $[RDR1.U_INIP] from RDR1", -1);
            //CreateUserQuery("GetUnitforQUT", "Select  top 1 $[QUT1.U_INIP] from QUT1", -1);
            //CreateFormattedSearch("139", "38", "14", BoFormattedSearchActionEnum.bofsaQuery, GetQueryID("GetUnitforRDR", -1), BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tNO, "U_INIP");
            //CreateFormattedSearch("149", "38", "14", BoFormattedSearchActionEnum.bofsaQuery, GetQueryID("GetUnitforRDR", -1), BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tNO, "U_INIP");




            //CreateUserQuery("GetSOforWTQ", "SELECT DISTINCT T1.\"DocNum\" AS \"Sales Order #\" FROM OWOR T0 INNER JOIN ORDR T1 ON T0.\"OriginAbs\" = T1.\"DocEntry\" Where T0.\"Status\" <> 'C' ORDER BY T1.\"DocNum\"", -1);
            //CreateFormattedSearch("1250000940", "U_AC_SONum", "-1", BoFormattedSearchActionEnum.bofsaQuery, GetQueryID("GetSOforWTQ", -1), BoYesNoEnum.tNO, BoYesNoEnum.tNO, BoYesNoEnum.tNO, "");



            //CreateUserQuery("GetItemforWTQ", " SELECT T1.\"ItemCode\" , T1.\"Dscription\", T1.\"U_AC_ExtRemarks\" FROM ORDR T0 INNER JOIN RDR1 T1 ON T0.\"DocEntry\" = T1.\"DocEntry\" WHERE $[OWTQ.\"U_SONum\"] = T0.\"DocNum\" and T0.\"CANCELED\" = 'N'", -1);
            //CreateFormattedSearch("1250000940", "U_AC_ItemCode", "-1", BoFormattedSearchActionEnum.bofsaQuery, GetQueryID("GetItemforWTQ", -1), BoYesNoEnum.tNO, BoYesNoEnum.tNO, BoYesNoEnum.tNO, "");

            ////CreateUserQuery("GetAttributesForGroup", "Select DISTINCT T1.\"U_AC_AttCode\",T1.\"U_AC_AttName\" from \"@AC_IAC1\" T1 inner join \"@AC_OIAC\" T0 on T1.\"DocEntry\" = T0.\"DocEntry\" where T0.\"U_AC_Code\" = $[$3.1.0] and T1.\"U_AC_Active\" = 'Y' and T0.\"DocEntry\" = $[$3.U_AC_VarGrp.0]", -1);
            ////CreateFormattedSearch("672", "3", "U_AC_AttCode", BoFormattedSearchActionEnum.bofsaQuery, GetQueryID("GetAttributesForGroup", -1), BoYesNoEnum.tNO, BoYesNoEnum.tNO, BoYesNoEnum.tNO, "");

            ////CreateUserQuery("GetAttNameForGroup", "Select T0.\"U_AC_Name\" from \"@AC_OAMD\" T0 where  T0.\"DocEntry\" = $[$3.U_AC_AttCode.0]", -1);
            ////CreateFormattedSearch("672", "3", "U_AC_AttName", BoFormattedSearchActionEnum.bofsaQuery, GetQueryID("GetAttNameForGroup", -1), BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tNO, "U_AC_AttCode");

            ////CreateUserQuery("GetOptCodeForGroup", "Select T1.\"U_AC_OptCode\",T1.\"U_AC_OptName\",T1.\"U_AC_Quantity\" from \"@AC_IAC1\" T1 inner join \"@AC_OIAC\" T0 on T1.\"DocEntry\" = T0.\"DocEntry\" where T0.\"U_AC_Code\" = $[$3.1.0] and T1.\"U_AC_Active\" = 'Y' and T0.\"DocEntry\" = $[$3.U_AC_VarGrp.0] and T1.\"U_AC_AttCode\" = $[$3.U_AC_AttCode.0]", -1);
            ////CreateFormattedSearch("672", "3", "U_AC_OptCode", BoFormattedSearchActionEnum.bofsaQuery, GetQueryID("GetOptCodeForGroup", -1), BoYesNoEnum.tNO, BoYesNoEnum.tNO, BoYesNoEnum.tNO, "");

            ////CreateUserQuery("GetOptNameForGroup", "Select T1.\"U_AC_OptName\" from \"@AC_IAC1\" T1 inner join \"@AC_OIAC\" T0 on T1.\"DocEntry\" = T0.\"DocEntry\" where T0.\"U_AC_Code\" = $[$3.1.0] and T1.\"U_AC_Active\" = 'Y' and T0.\"DocEntry\" = $[$3.U_AC_VarGrp.0] and T1.\"U_AC_AttCode\" = $[$3.U_AC_AttCode.0] and T1.\"U_AC_OptCode\" = $[$3.U_AC_OptCode.0]", -1);
            ////CreateFormattedSearch("672", "3", "U_AC_OptName", BoFormattedSearchActionEnum.bofsaQuery, GetQueryID("GetOptNameForGroup", -1), BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tNO, "U_AC_OptCode");

            //if (Global.Comp_DI.DbServerType == BoDataServerTypes.dst_HANADB)
            //{
            //    CreateUserQuery("GetVarAp", "Select CASE WHEN $[$3.U_AC_VarGrp.0] = '' THEN 'N' ELSE 'Y' END from dummy", -1);
            //}
            //else
            //{
            //    CreateUserQuery("GetVarAp", "IF $[$3.U_AC_VarGrp.0] = '' Select 'N' ELSE Select 'Y'", -1);
            //}
            //CreateFormattedSearch("672", "3", "U_AC_VarAp", BoFormattedSearchActionEnum.bofsaQuery, GetQueryID("GetVarAp", -1), BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tNO, "U_AC_VarGrp");

            ////CreateUserQuery("GetBays", "Select \"Code\",\"Name\" from \"@AC_OOPT\" where \"U_AC_Bay\" = 'Y'", -1);
            ////CreateFormattedSearch("VS_VC.Variants", "Item_0", "BayCode", BoFormattedSearchActionEnum.bofsaQuery, GetQueryID("GetBays", -1), BoYesNoEnum.tNO, BoYesNoEnum.tNO, BoYesNoEnum.tNO, "");




            //CheckAndCreateDefaultUserField("@AC_OIAC", "AC_Code", "Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_OIAC", "AC_Parent", "Parent", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");

            //CheckAndCreateDefaultUserField("@AC_OIAC", "AC_Name", "Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");
            //CheckAndCreateDefaultUserField("@AC_OIAC", "AC_Remarks", "Remarks", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100, "");
            //CheckAndCreateDefaultUserField("@AC_OIAC", "AC_Attributes", "Attributes", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 100, "");
            //CheckAndCreateDefaultUserField("@AC_OIAC", "AC_Date", "Date", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, "");
            //CheckAndCreateDefaultUserField("@AC_IAC1", "AC_AttCode", "Attribute Code", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11, "");
            //CheckAndCreateDefaultUserField("@AC_IAC1", "AC_AttName", "Attribute Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_IAC1", "AC_OptCode", "Option Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_IAC1", "AC_OptName", "Option Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_IAC1", "AC_Quantity", "Quantity", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 11, "");
            //CheckAndCreateDefaultUserField("@AC_IAC1", "AC_SCode", "Size Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_IAC1", "AC_Size", "Size", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_IAC1", "AC_CCode", "Color Code", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateDefaultUserField("@AC_IAC1", "AC_Color", "Color", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");

            //validValsSts.Clear();
            //validValsSts.Add(new List<string>()); validValsSts[0].Add("Y"); validValsSts[0].Add("Y");
            //validValsSts.Add(new List<string>()); validValsSts[1].Add("N"); validValsSts[1].Add("N");
            //CheckAndCreateAlphaNumUserFieldWithValidValues("@AC_IAC1", "AC_Active", "Active", BoFieldTypes.db_Alpha, 1, validValsSts, false, 0);
            //CheckAndCreateAlphaNumUserFieldWithValidValues("@AC_OIAC", "AC_Active", "Active", BoFieldTypes.db_Alpha, 1, validValsSts, false, 0);

            //validValsSts.Clear();
            //validValsSts.Add(new List<string>()); validValsSts[0].Add("4"); validValsSts[0].Add("Item");
            //validValsSts.Add(new List<string>()); validValsSts[1].Add("290"); validValsSts[1].Add("Resource");
            //CheckAndCreateAlphaNumUserFieldWithValidValues("@AC_OIAC", "AC_Type", "Type", BoFieldTypes.db_Alpha, 3, validValsSts, true, 0);

            //udoFormCols1.Clear();
            //tables1.Clear();
            //ColRech1.Clear();
            //tables1.Add("AC_IAC1");
            //ColRech1.Add("DocEntry");
            //ColRech1.Add("U_AC_Code");
            //ColRech1.Add("U_AC_Name");
            //ColRech1.Add("U_AC_Attributes");
            //ColRech1.Add("U_AC_Date");
            //CheckAndCreateUDO("AC_IAC", "Item Configurator", BoUDOObjType.boud_Document, "AC_OIAC", BoYesNoEnum.tYES, BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tYES, ColRech1, udoFormCols1, tables1);
            //CheckAndCreateFieldWithLinkedUDO("@AC_OMCT", "AC_OptCode", "Option", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "AC_OPT", BoYesNoEnum.tYES);
            //CheckAndCreateDefaultUserField("@AC_OMCT", "AC_OptName", "Option Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "");
            //CheckAndCreateFieldWithLinkedUDO("@AC_OMCT", "AC_Category", "Category", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "AC_CAT", BoYesNoEnum.tYES);
            //CheckAndCreateFieldWithLinkedUDO("@AC_OMCT", "AC_CatName", "Category Name", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, "", BoYesNoEnum.tNO);

            //tables1.Clear();
            //ColRech1.Clear();
            //udoFormCols1.Clear();
            //udoFormCols1.Add("Code");
            //udoFormCols1.Add("U_AC_OptCode");
            //udoFormCols1.Add("U_AC_Category");
            //udoFormCols1.Add("U_AC_CatName");
            //ColRech1.Add("Code");
            //ColRech1.Add("U_AC_OptCode");
            //ColRech1.Add("U_AC_Category");
            //ColRech1.Add("U_AC_CatName");

            //CheckAndCreateUDO("AC_MCT", "Option Category", BoUDOObjType.boud_MasterData, "AC_OMCT", BoYesNoEnum.tYES, BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tYES, BoYesNoEnum.tYES, ColRech1, udoFormCols1, BoYesNoEnum.tNO, BoYesNoEnum.tYES, "Option Category", "AC_VC9");

            //if (Global.Comp_DI.DbServerType == BoDataServerTypes.dst_HANADB)
            //{
            //    CreateUserQuery("OptCatCode", "Select $[@AC_OMCT.U_AC_OptCode] || $[@AC_OMCT.U_AC_Category]  from dummy", -1);

            //}
            //else
            //{
            //    CreateUserQuery("OptCatCode", "Select CONCAT($[@AC_OMCT.U_AC_OptCode],$[@AC_OMCT.U_AC_Category])", -1);
            //}
            //CreateFormattedSearch("AC_MCT", "3", "Code", BoFormattedSearchActionEnum.bofsaQuery, GetQueryID("OptCatCode", -1), BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tNO, "U_AC_Category");
            //CreateUserQuery("GetAllAttributes", "Select \"DocEntry\" as \"Code\",\"U_AC_Name\" as \"Attribute\" from \"@AC_AMD1\"", -1);
            //CreateFormattedSearch("AC_SPR", "3", "U_AC_AttCode", BoFormattedSearchActionEnum.bofsaQuery, GetQueryID("GetAllAttributes", -1), BoYesNoEnum.tNO, BoYesNoEnum.tNO, BoYesNoEnum.tNO, "");
            //CreateUserQuery("GetAttName1", "Select \"U_AC_Name\"  from \"@AC_OAMD\" where \"DocEntry\" = $[@AC_OSPR.U_AC_AttCode]", -1);
            //CreateFormattedSearch("AC_SPR", "3", "U_AC_AttName", BoFormattedSearchActionEnum.bofsaQuery, GetQueryID("GetAttName1", -1), BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tNO, "U_AC_AttCode");


            //CreateUserQuery("GetOptionName", "Select \"U_AC_Name\" as \"Option Name\" from \"@AC_AMD1\" where \"DocEntry\" = $[@AC_AMD1.U_AC_AttCode] and \"U_AC_Option\" =  $[@AC_AMD1.U_AC_OptCode]", -1);
            //CreateFormattedSearch("VS_VC.Attributes", "Item_0", "DOptName", BoFormattedSearchActionEnum.bofsaQuery, GetQueryID("GetOptionName", -1), BoYesNoEnum.tNO, BoYesNoEnum.tYES, BoYesNoEnum.tNO, "DOptCode");
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
                    oRowItem.SetProperty("U_Status", "New");
                    oRowItem = oRows.Add();
                    oRowItem.SetProperty("U_Status", "Current");
                    oRowItem = oRows.Add();
                    oRowItem.SetProperty("U_Status", "Transfer");
                    oRowItem = oRows.Add();
                    oRowItem.SetProperty("U_Status", "Retake");
                    oRowItem = oRows.Add();
                    oRowItem.SetProperty("U_Status", "Pending");
                    oRowItem = oRows.Add();
                    oRowItem.SetProperty("U_Status", "EXT");
                    oRowItem = oRows.Add();
                    oRowItem.SetProperty("U_Status", "Readmit");
                    oRowItem = oRows.Add();
                    oRowItem.SetProperty("U_Status", "PTDW");

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
        public static void CheckAndCreateUDO(string UdoCode, string UdoName, BoUDOObjType UdoType, string table, BoYesNoEnum canDel, BoYesNoEnum canLog, BoYesNoEnum canFind,
        BoYesNoEnum canCreateDefForm, BoYesNoEnum canCancel, List<string> findColumns, List<string> formsCols,
        BoYesNoEnum EnableEnhancedForm, BoYesNoEnum MenuItem, string MenuCaption, string MenuUID)
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
                    udo.EnableEnhancedForm = EnableEnhancedForm;
                    udo.MenuItem = MenuItem;
                    udo.MenuCaption = MenuCaption;
                    udo.MenuUID = MenuUID;


                    udo.CanCancel = canCancel;

                    foreach (string tempstr in formsCols)
                    {
                        udo.FormColumns.FormColumnAlias = tempstr;
                        udo.FormColumns.Editable = BoYesNoEnum.tYES;
                        udo.FormColumns.Add();
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
                    if (Fields.Length == 1)
                    {
                        fs.FieldID = Fields[0];
                    }
                    else
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
        private static void CheckAndCreateUserKey(string KeyName, string TableName, string fieldName, bool bFlagFirst)
        {
            int nbres = 0;
            Global.QueryFirstValueRec("SELECT T0.\"KeyName\" FROM OUKD T0 WHERE T0.\"KeyName\"='" + KeyName + "'", true, out nbres);
            if (nbres == 0)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                SAPbobsCOM.UserKeysMD oUserKeysMD;
                oUserKeysMD = Comp_DI.GetBusinessObject(BoObjectTypes.oUserKeys) as SAPbobsCOM.UserKeysMD;
                oUserKeysMD.TableName = TableName;
                oUserKeysMD.KeyName = KeyName;
                if (bFlagFirst == true)
                {
                    bFlagFirst = false;
                }
                else
                {
                    oUserKeysMD.Elements.Add();
                }
                oUserKeysMD.Elements.ColumnAlias = fieldName;
                oUserKeysMD.Unique = BoYesNoEnum.tYES;
                if (oUserKeysMD.Add() != 0)
                {
                    string Message = Comp_DI.GetLastErrorDescription();
                    SetMessage("UserKey Not Created. " + Message, BoStatusBarMessageType.smt_Warning);
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserKeysMD);
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }
        private static void CreateItem(string code, string description)
        {
            try
            {
                SAPbobsCOM.Items oItem = Comp_DI.GetBusinessObject(BoObjectTypes.oItems) as SAPbobsCOM.Items;
                oItem.ItemCode = code;
                oItem.ItemName = description;
                oItem.Add();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oItem);
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }
            catch (Exception)
            {

                throw;
            }
        }

        private static void RemoveUDFiFExists(string TableName, string fieldName)
        {
            int nbres = 0;
            Global.QueryFirstValueRec("SELECT T0.\"AliasID\" FROM CUFD T0 WHERE T0.\"TableID\"='" + TableName + "' AND T0.\"AliasID\"='" + fieldName + "'", true, out nbres);
            if (nbres != 0)
            {
                int fieldID = (int)Global.QueryFirstValueRec("SELECT T0.\"FieldID\" FROM CUFD T0 WHERE T0.\"TableID\"='" + TableName + "' AND T0.\"AliasID\"='" + fieldName + "'", true);
                SAPbobsCOM.UserFieldsMD myUserFieldsMD = (SAPbobsCOM.UserFieldsMD)Comp_DI.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                if (myUserFieldsMD.GetByKey(TableName, fieldID))
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    int res = myUserFieldsMD.Remove();
                    if (res != 0)
                        SetAlertMessage(Comp_DI.GetLastErrorDescription());
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(myUserFieldsMD);
                    GC.WaitForPendingFinalizers();
                    GC.Collect();


                }
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

        //cré une zone alpha/numérique dans une table utilisateur
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
        //  public static void CheckAndCreateFieldWithLinkedObject(string TableName, string fieldName, string desc, SAPbobsCOM.BoFieldTypes type, SAPbobsCOM.BoFldSubTypes subType, int size, BoObjectTypes LinkedObj)
        //{
        //    int nbres = 0;
        //    Global.QueryFirstValueRec("SELECT T0.\"AliasID\" FROM CUFD T0 WHERE T0.\"TableID\"='" + TableName + "' AND T0.\"AliasID\"='" + fieldName + "'", true, out nbres);
        //    if (nbres == 0)
        //    {
        //        GC.Collect();
        //        GC.WaitForPendingFinalizers();
        //        SAPbobsCOM.UserFieldsMD myUserFieldsMD = (SAPbobsCOM.UserFieldsMD)Comp_DI.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
        //        myUserFieldsMD.Name = fieldName;
        //        myUserFieldsMD.TableName = TableName;
        //        myUserFieldsMD.Description = desc;
        //        myUserFieldsMD.Type = type;
        //        if (type == SAPbobsCOM.BoFieldTypes.db_Alpha || type == SAPbobsCOM.BoFieldTypes.db_Numeric)
        //        {
        //            if (size > 0 && type == SAPbobsCOM.BoFieldTypes.db_Alpha)
        //                myUserFieldsMD.Size = size;
        //            myUserFieldsMD.EditSize = size;
        //        }
        //        myUserFieldsMD.SubType = subType;
        //        myUserFieldsMD.LinkedSystemObject = LinkedObj;



        //        int res = myUserFieldsMD.Add();
        //        if (res != 0)
        //            SetAlertMessage(Comp_DI.GetLastErrorDescription());
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(myUserFieldsMD);
        //        GC.WaitForPendingFinalizers();
        //        GC.Collect();

        //    }
        //}

        public static void CheckAndCreateFieldWithLinkedUDO(string TableName, string fieldName, string desc, SAPbobsCOM.BoFieldTypes type, SAPbobsCOM.BoFldSubTypes subType, int size, string LinkedObj, BoYesNoEnum mand)
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
                myUserFieldsMD.LinkedUDO = LinkedObj;
                myUserFieldsMD.Mandatory = mand;


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
        //cré une zone alpha dans une table utilisateur avec validvalues et valeur par défaut & obligatoire
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

        public static bool CreateUserTable(string name, string description, SAPbobsCOM.BoUTBTableType tabType)
        {
            GC.WaitForPendingFinalizers();
            GC.Collect();
            SAPbobsCOM.UserTablesMD userTables = Comp_DI.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserTables)
                            as SAPbobsCOM.UserTablesMD;
            int retVal = 0;
            try
            {
                if (!userTables.GetByKey(name))
                {
                    userTables.TableName = name;
                    userTables.TableDescription = description;
                    userTables.TableType = tabType;
                    retVal = userTables.Add();
                    if (retVal != 0)
                        SetAlertMessage(Comp_DI.GetLastErrorDescription());
                }
            }
            finally
            {
                userTables = null;
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            return retVal == 0;
        }
        public static void CreateUserQuery(string name, string query, out string ID)
        {
            int nbres = 0;
            ID = "";
            Global.QueryFirstValueRec("SELECT \"IntrnalKey\"  FROM OUQR Where \"QName\" = '" + name + "' ", true, out nbres);
            if (nbres == 0)
            {
                GC.WaitForPendingFinalizers();
                GC.Collect();
                oUserQuery = (SAPbobsCOM.UserQueries)Comp_DI.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserQueries);
                oUserQuery.QueryDescription = name;
                oUserQuery.Query = query;
                //  oUserQuery.Query  = " SELECT T0.\"ItemCode\",T0.\"itemName\", T0.\"AbsEntry\", T0.\"DistNumber\", T0.\"ExpDate\" FROM OBTN T0 INNER JOIN OITM T1 ON T0.\"ItemCode\" = T1.\"ItemCode\"  WHERE ( T0.\"Status\"='0' OR T0.\"Status\"='1') AND T0.\"ExpDate\"<= ADD_DAYS(CURRENT_DATE,IFNULL( T1.\"U_CMC_RP_DDP\", (SELECT IFNULL(\"U_CMC_RP_DDP\",60) FROM OADM))) ORDER BY T0.\"itemName\",\"ExpDate\" ";
                oUserQuery.QueryCategory = -1;

                if (oUserQuery.Add() != 0)
                {
                    myApi.SetStatusBarMessage(Comp_DI.GetLastErrorDescription());
                }
                else
                {
                    ID = Comp_DI.GetNewObjectKey();
                    Comp_DI.GetNewObjectCode(out ID);
                }
                oUserQuery = null;
                GC.WaitForPendingFinalizers();
                GC.Collect();


            }

        }
        public static AlertManagementService oAlertTemplatesService;
        public static SAPbobsCOM.CompanyService oCmpSrv;
        public static void CreateAlert(string name)
        {
            int nbres = 0;
            Global.QueryFirstValueRec("select \"Code\" from oalt where \"Name\" =  '" + name + "' ", true, out nbres);
            if (nbres == 0)
            {
                int QueryID = (int)Global.QueryFirstValueRec(" SELECT \"IntrnalKey\" FROM OUQR Where \"QName\" = 'Lots à proche péremption' ", true);
                oCmpSrv = Comp_DI.GetCompanyService();
                AlertManagement oAlertTemplate;
                AlertManagementParams oAlertTemplateParams;
                AlertManagementRecipients oAlertTemplateRecipients;
                AlertManagementRecipient oAlertRecipient;

                oAlertTemplatesService = (SAPbobsCOM.AlertManagementService)oCmpSrv.GetBusinessService(ServiceTypes.AlertManagementService);
                //get alert template
                oAlertTemplate = (SAPbobsCOM.AlertManagement)oAlertTemplatesService.GetDataInterface(AlertManagementServiceDataInterfaces.atsdiAlertManagement);

                //set alert name
                oAlertTemplate.Name = name;

                //set query
                oAlertTemplate.QueryID = QueryID;

                oAlertTemplate.Active = BoYesNoEnum.tNO;

                //set priority
                oAlertTemplate.Priority = AlertManagementPriorityEnum.atp_High;

                //set the FrequencyType (minutes,hours...)
                oAlertTemplate.FrequencyType = AlertManagementFrequencyType.atfi_Days;

                //set intervals
                oAlertTemplate.FrequencyInterval = 1;

                //get Recipients collection
                oAlertTemplateRecipients = oAlertTemplate.AlertManagementRecipients;

                //add recipient
                oAlertRecipient = oAlertTemplateRecipients.Add();

                //set recipient code(manager=1)
                oAlertRecipient.UserCode = 1;

                //set internal message
                oAlertRecipient.SendInternal = BoYesNoEnum.tYES;

                //add alert template
                oAlertTemplateParams = oAlertTemplatesService.AddAlertManagement(oAlertTemplate);
            }
        }
        public static void CreatePermission(string name)
        {
            long RetVal;
            //			long ErrCode;
            string ErrMsg = "";
            SAPbobsCOM.UserPermissionTree oPermission;

            oPermission = (SAPbobsCOM.UserPermissionTree)Comp_DI.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserPermissionTree);
            if (!oPermission.GetByKey("CMC_PharmOne"))
            {
                oPermission.Name = name;
                oPermission.PermissionID = "CMC_PharmOne";


                oPermission.Options = SAPbobsCOM.BoUPTOptions.bou_FullReadNone;
                RetVal = oPermission.Add();

                int temp_int = (int)(RetVal);
                string temp_string = ErrMsg;
                Comp_DI.GetLastError(out temp_int, out temp_string);
                if (RetVal != 0)
                {
                    myApi.SetStatusBarMessage(temp_string);
                }

                oPermission = (SAPbobsCOM.UserPermissionTree)Comp_DI.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserPermissionTree);

                oPermission.PermissionID = "CMC_Quota";
                oPermission.ParentID = "CMC_PharmOne";
                oPermission.Name = "Gestion des produits quota";
                oPermission.Options = BoUPTOptions.bou_FullReadNone;
                oPermission.UserPermissionForms.FormType = "SBOQuo.Form2";


                if (oPermission.Add() != 0)
                {
                    myApi.SetStatusBarMessage(Comp_DI.GetLastErrorDescription());
                }

                oPermission = (SAPbobsCOM.UserPermissionTree)Comp_DI.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserPermissionTree);

                oPermission.PermissionID = "CMC_COMR";
                oPermission.ParentID = "CMC_PharmOne";
                oPermission.Name = "Commande Pharmaceutique";
                oPermission.Options = BoUPTOptions.bou_FullReadNone;
                oPermission.UserPermissionForms.FormType = "CMC_COMR";  //CMC_COMR //Pharma.DCI


                if (oPermission.Add() != 0)
                {
                    myApi.SetStatusBarMessage(Comp_DI.GetLastErrorDescription());
                }

                oPermission = (SAPbobsCOM.UserPermissionTree)Comp_DI.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserPermissionTree);

                oPermission.PermissionID = "CMC_DCI";
                oPermission.ParentID = "CMC_PharmOne";
                oPermission.Name = "Gestion des DCI";
                oPermission.Options = BoUPTOptions.bou_FullReadNone;
                oPermission.UserPermissionForms.FormType = "Pharma.DCI";


                if (oPermission.Add() != 0)
                {
                    myApi.SetStatusBarMessage(Comp_DI.GetLastErrorDescription());
                }


                oPermission = (SAPbobsCOM.UserPermissionTree)Comp_DI.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserPermissionTree);

                oPermission.PermissionID = "CMC_AsTr";
                oPermission.ParentID = "CMC_PharmOne";
                oPermission.Name = "Transfert de stocks";
                oPermission.Options = BoUPTOptions.bou_FullReadNone;
                oPermission.UserPermissionForms.FormType = "CMC_AsTr";   //Pharma.AssistentTransfer


                if (oPermission.Add() != 0)
                {
                    myApi.SetStatusBarMessage(Comp_DI.GetLastErrorDescription());
                }

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

        
        public static void CreateDialogAndFileValue(EditText TextBox, string SelectFormats)
        {
            try
            {
         
                Global.SetMessage("Select file to import", BoStatusBarMessageType.smt_Warning);
                Thread t = new Thread(() =>
                {
                    System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
                    openFileDialog.Filter = SelectFormats;
                    var NewForm = new System.Windows.Forms.Form();
                    System.Windows.Forms.DialogResult dr = openFileDialog.ShowDialog(NewForm);

                    if (dr == System.Windows.Forms.DialogResult.OK)
                    {
                        string fileName = openFileDialog.FileName;
                        TextBox.Value = fileName;
                    }
                });
                t.IsBackground = false;
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
            }
            catch (Exception ex)
            {
                Global.SetMessage(ex.Message, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
        }
        public static void showOpenFileDialog(EditText TextBox, string SelectFormats)
        {
            Thread ShowFolderBrowserThread;
            try
            {
                ShowFolderBrowserThread = new Thread( new ThreadStart(()=>ShowFolderBrowser(TextBox, SelectFormats)));

                if (ShowFolderBrowserThread.ThreadState == System.Threading.ThreadState.Unstarted)
                {
                    ShowFolderBrowserThread.SetApartmentState(System.Threading.ApartmentState.STA);

                    ShowFolderBrowserThread.Start();
                }
                else if (ShowFolderBrowserThread.ThreadState == System.Threading.ThreadState.Stopped)
                {
                    ShowFolderBrowserThread.Start();
                    ShowFolderBrowserThread.Join();
                }

                while (ShowFolderBrowserThread.ThreadState == System.Threading.ThreadState.Running)
                {
                    System.Windows.Forms.Application.DoEvents();
                }


            }
            catch (Exception ex)
            {
            
            }
        }
        private static string ShowFolderBrowser(EditText TextBox, string SelectFormats)
        {
            System.Diagnostics.Process[] myProcs;
            System.Windows.Forms.OpenFileDialog openFile = new System.Windows.Forms.OpenFileDialog();
            string fileName = "";

            try
            {
                openFile.Multiselect = false;
                openFile.Filter = SelectFormats;
                int filterIndex = 0;
                try
                {
                    filterIndex = 0;
                }
                catch (Exception) { }
                openFile.FilterIndex = filterIndex;
                openFile.RestoreDirectory = true;

                myProcs = System.Diagnostics.Process.GetProcessesByName("SAP Business One");
                if (myProcs.Length == 1)
                {
                    for (int i = 0; i < myProcs.Length; i++)
                    {
                        WindowWrapper myWindow = new WindowWrapper(myProcs[i].MainWindowHandle);
                        System.Windows.Forms.DialogResult ret = openFile.ShowDialog(myWindow);
                        System.Windows.Forms.SendKeys.SendWait("%{TAB}");
                        if (ret == System.Windows.Forms.DialogResult.OK)
                        {
                            TextBox.Value = openFile.FileName;
                            openFile.Dispose();
                        }
                        else
                        {
                            System.Windows.Forms.Application.ExitThread();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                fileName = "";
            }
            finally
            {
                openFile.Dispose();
            }
            return "";
        }

         public static void OpenFileDialogForProcesses(EditText TextBox, string SelectFormats)
        {
            try
            {
                Thread FileThread = new Thread(new ThreadStart(() => GetTheFile(TextBox, SelectFormats)));

                if (FileThread.ThreadState == ThreadState.Unstarted)
                {
                    FileThread.SetApartmentState(ApartmentState.STA);
                    FileThread.Start();
                }
                else
                {
                    FileThread.Start();
                    FileThread.Join();
                }

                while (FileThread.ThreadState == System.Threading.ThreadState.Running)
                {
                    System.Windows.Forms.Application.DoEvents();
                }

                //return RetFileName;
            }
            catch (Exception)
            {
                //return "";
            }
        }

          private static  void GetTheFile(EditText TextBox, string SelectFormats)
        {
            try
            {
                System.Windows.Forms.OpenFileDialog FileDialog = new System.Windows.Forms.OpenFileDialog();
                FileDialog.Multiselect = false;
                FileDialog.Filter = SelectFormats;
                FileDialog.ShowDialog();
                TextBox.Value = FileDialog.FileName;
                System.Windows.Forms.Application.ExitThread();
            }
            catch (Exception)
            {
                //RetFileName = "";
            }
        }
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
        public static void ClearCombo(ComboBox cmb)
        {
            for (int iCpt = cmb.ValidValues.Count - 1; iCpt > -1; iCpt--)
            {
                try
                {
                    cmb.ValidValues.Remove(iCpt, BoSearchKey.psk_Index);
                }
                catch
                {
                }
            }
        }
        public static DateTime Format_StringToDate(string strdate)
        {
            //renvoi un DateTime a partir d'un string date
            int y = 1900, m = 1, d = 1;
            if (strdate.Contains(" "))
                strdate = strdate.Substring(0, strdate.IndexOf(' '));
            try
            {
                if (strdate.Length == 8) // type 20070204
                {
                    y = int.Parse(strdate.Substring(6, 2));
                    m = int.Parse(strdate.Substring(3, 2));
                    d = int.Parse(strdate.Substring(0, 2));
                }
                else if (strdate.Length == 10)
                {// type 10/02/2005
                    y = int.Parse(strdate.Substring(6, 2));
                    m = int.Parse(strdate.Substring(3, 2));
                    d = int.Parse(strdate.Substring(0, 2));
                }
            }
            catch
            {
                try
                {
                    return DateTime.Parse(strdate);
                }
                catch (Exception)
                {
                }
            }
            return new DateTime(y, m, d);
        }
        public static double FormattingNumberToDB(string number)
        {
            double total = 0;
            if (number != "")
                total = double.Parse(number.Replace(".", _Sep));
            return total;
        }

        //set a message into status bar SapB1
        public static void SetMessage(string Text, BoStatusBarMessageType type)
        {
            myApi.StatusBar.SetText(Text, BoMessageTime.bmt_Short, type);
        }
        //set an alert message to the B1 user
        public static void SetAlertMessage(string text)
        {
            try
            {
                Global.myApi.MessageBox(text, 1, "OK", "", "");
            }
            catch (Exception) { }
        }

        public static string GetXmlValue(string XmlTree, string XmlNodeCode)
        {
            try
            {
                if (XmlTree.Length > XmlNodeCode.Length || !XmlTree.Contains(XmlNodeCode))
                {
                    string retour = "";
                    int index = XmlTree.IndexOf("<" + XmlNodeCode);
                    index += XmlNodeCode.Length + 1;
                    if (XmlTree[index] == '/')
                        return "";
                    else
                        index += 1;
                    while (index < XmlTree.Length && XmlTree[index] != '<')
                    {
                        retour = String.Concat(retour, XmlTree[index]);
                        index++;
                    }
                    return retour;
                }
            }
            catch (Exception)
            {
            }
            return XmlTree;
        }

        //Vérifie l'existance d'un UDO le crée si existe pas
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

        //renvoi un DateTime a partir d'un string date
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
        //quitte l'add-on
        public static void Quit()
        {
            try
            {
                //myApi.Menus.RemoveEx("");

                if (Comp_DI.Connected)
                    Comp_DI.Disconnect();
            }
            catch (Exception)
            {
            }
            finally
            {
                //myGuiApi = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Environment.Exit(0);
            }
        }



        public static string GetNumberDecimalSeparator()
        {
            return (_decimalSeparator);
        }
        public static string FormattingNumberFromDB(double number)
        {
            string sep = GetNumberDecimalSeparator();

            string total = "0";
            if (number != 0)
            {
                if (sep == ",")
                    total = number.ToString().Replace(",", ".");
                else
                    total = number.ToString().Replace(NumberDecimalSeparator, sep);
            }
            if (!string.IsNullOrEmpty(_thousandSeparator))
                total = total.Replace(_thousandSeparator, "");

            return total;
        }

        #endregion

        #region evènements




        public static void myApi_AppEvent(BoAppEventTypes EventType)
        {
            if (EventType == BoAppEventTypes.aet_CompanyChanged || EventType == BoAppEventTypes.aet_ShutDown || EventType == BoAppEventTypes.aet_ServerTerminition)
                Quit();
        }

        #endregion

        #region filtres

        public static void MAJ_filter()
        {
            //filtres		menu principale	
            EventFilters mesFiltres = new SAPbouiCOM.EventFilters();
            EventFilter monFiltre = mesFiltres.Add(SAPbouiCOM.BoEventTypes.et_ALL_EVENTS);
            monFiltre.AddEx("SM_One.SMConfig");
            monFiltre.AddEx("SM_One.ImportStudents");
            monFiltre.AddEx("SM_One.Registration");
            monFiltre.AddEx("SM_One.GenerateInvoices");
            monFiltre.AddEx("SM_One.ViewDifferences");
            monFiltre.AddEx("SM_One.SearchCourses");
            monFiltre.AddEx("SM_One.Postings"); 
            monFiltre.AddEx("229");
            

            //monFiltre.AddEx("AC_OPT");
            //monFiltre.AddEx("AC_CAT");
            //monFiltre.AddEx("AC_MCT");
            //monFiltre.AddEx("VS_VC.Variants");
            //monFiltre.AddEx("VS_VC.AttributeGroup");
            //monFiltre.AddEx("VS_VC.Configurator");
            //monFiltre.AddEx("VS_VC.SearchForm");
            //monFiltre.AddEx("VS_VC.ProductionWizard");
            //monFiltre.AddEx("VS_VC.ItemConfiguration");
            //monFiltre.AddEx("VS_VC.ItemWizard");

            //monFiltre.AddEx("139");
            //monFiltre.AddEx("142");
            //monFiltre.AddEx("1250000940");
            //monFiltre.AddEx("149");
            myApi.SetFilter(mesFiltres);
        }

        #endregion

        #region menus

        //fonction générique qui créé un menu s'il n'existe pas
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
