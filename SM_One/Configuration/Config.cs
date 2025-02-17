using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM_One
{
    public static class Config
    {
        public static int VoucherSeries, InvoiceSeries,ItemSeries,GroupCode,Semester1Start,Semester1End,Semester2Start,Semester2End;
        public static string TaxGroup,DebitAccount, CreditAccount, AccountFrom, MajorDim,ScholarDim,CollegeDim, ProratedOn, SendInvoice, PricedOn,Server,Database,Username,Password, GlobalConnection,RepeatScholarship;
        public static bool AllowWithoutTrail, AllowCancellation, AllowRefund, AllowNonRefund, UpdatePriceList, FiscalNotCalendar;
        public static Dictionary<string, string> CreditMapping = new Dictionary<string, string>();
        public static Dictionary<string, string> DebitMapping = new Dictionary<string, string>();
        public static Dictionary<string, int> PriceListMapping = new Dictionary<string, int>();
        public static List<StatusConfig> AllStatusConfig = new List<StatusConfig>();
        public static List<StatusConfig> FreightCodes = new List<StatusConfig>();
        private static void GenerateSQLConnection()
        {
            GlobalConnection = "Server="+Server+";Database="+Database+";User Id="+Username+";Password="+Password+";Trusted_Connection=False;MultipleActiveResultSets=True;";
        }
        public static void LoadConfig()
        {
            CreditMapping.Clear();
            DebitMapping.Clear();
            Recordset oRec = Global.Comp_DI.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
            try
            {
                oRec.DoQuery("Select * from \"@CONF\" where \"Code\" = 'CONF'");
                VoucherSeries = Convert.ToInt32(oRec.Fields.Item("U_VSeries").Value);
                InvoiceSeries = Convert.ToInt32(oRec.Fields.Item("U_InvSeries").Value);
                ItemSeries = Convert.ToInt32(oRec.Fields.Item("U_ItemSeries").Value);
                DebitAccount = oRec.Fields.Item("U_VDebit").Value.ToString();
                CreditAccount = oRec.Fields.Item("U_VCredit").Value.ToString();
                MajorDim = oRec.Fields.Item("U_ProgramDim").Value.ToString();
                ScholarDim = oRec.Fields.Item("U_ScholarDim").Value.ToString();
                CollegeDim = oRec.Fields.Item("U_CollegeDim").Value.ToString();
                AccountFrom = oRec.Fields.Item("U_AccountFrom").Value.ToString();
                ProratedOn = oRec.Fields.Item("U_ProratedOn").Value.ToString();
                SendInvoice = oRec.Fields.Item("U_SendInvoice").Value.ToString();
                TaxGroup = oRec.Fields.Item("U_TaxGroup").Value.ToString();
                Server = oRec.Fields.Item("U_SQLServer").Value.ToString();
                Database = oRec.Fields.Item("U_SQLDatabase").Value.ToString();
                Username = oRec.Fields.Item("U_SQLUsername").Value.ToString();
                Password = oRec.Fields.Item("U_SQLPassword").Value.ToString();
                RepeatScholarship = oRec.Fields.Item("U_RepScholar").Value.ToString();
                GenerateSQLConnection();
                AllowWithoutTrail = oRec.Fields.Item("U_AllowWOTrail").Value.ToString() == "Y"?true : false;
                UpdatePriceList = oRec.Fields.Item("U_UpdatePL").Value.ToString() == "Y" ? true : false;
                FiscalNotCalendar = oRec.Fields.Item("U_FiscalNotCal").Value.ToString() == "Y" ? true : false;
                AllowCancellation = oRec.Fields.Item("U_AllowCancel").Value.ToString() == "Y" ? true : false;
                AllowRefund = oRec.Fields.Item("U_AllowRefund").Value.ToString() == "Y" ? true : false;
                AllowNonRefund = oRec.Fields.Item("U_AllowNRefund").Value.ToString() == "Y" ? true : false;
                PricedOn = oRec.Fields.Item("U_PricedOn").Value.ToString();
                if (FiscalNotCalendar)
                {
                    Semester1Start = Convert.ToInt32(oRec.Fields.Item("U_Sem1Start").Value);
                    Semester1End = Convert.ToInt32(oRec.Fields.Item("U_Sem1End").Value);
                    Semester2Start = Convert.ToInt32(oRec.Fields.Item("U_Sem2Start").Value);
                    Semester2End = Convert.ToInt32(oRec.Fields.Item("U_Sem2Start").Value);
                }
                else
                {
                    Semester1Start = 0;
                    Semester1End = 0;
                    Semester2Start = 0;
                    Semester2End = 0;
                }
       
                int.TryParse(oRec.Fields.Item("U_GroupCode").Value.ToString(), out GroupCode);
                oRec.DoQuery("Select * from \"@ORMP\"");
                DebitMapping.Clear();
                while (!oRec.EoF)
                {
                    DebitMapping.Add(oRec.Fields.Item("Code").Value.ToString(),oRec.Fields.Item("U_Debit").Value.ToString());
                    CreditMapping.Add(oRec.Fields.Item("Code").Value.ToString(), oRec.Fields.Item("U_Credit").Value.ToString());
                    oRec.MoveNext();
                }
                //oRec.DoQuery("Select \"ListNum\", \"U_FiscalYear\" from \"OPLN\" where coalesce(\"U_FiscalYear\",0) <> 0");
                //PriceListMapping.Clear();
                //while (!oRec.EoF)
                //{
                //    PriceListMapping.Add(oRec.Fields.Item("U_FiscalYear").Value.ToString(), Convert.ToInt32(oRec.Fields.Item("ListNum").Value.ToString()));
                //    oRec.MoveNext();
                //}

                oRec.DoQuery("Select * from \"@ONF1\" where \"Code\" = 'CONF'");
                AllStatusConfig.Clear();
                while (!oRec.EoF)
                {
                    StatusConfig statusConfig = new StatusConfig();
                    statusConfig.Status = oRec.Fields.Item("U_Status").Value.ToString();
                    statusConfig.Program = oRec.Fields.Item("U_Program").Value.ToString();
                    statusConfig.Courses = oRec.Fields.Item("U_Courses").Value.ToString();
                    AllStatusConfig.Add(statusConfig);
                    oRec.MoveNext();
                }
                string a= ("Select T0.\"U_Freight\",T0.\"U_Enabled\",T0.\"LineId\",T1.\"ExpnsName\",T0.\"U_IsDiscount\",T0.\"U_DefaultValue\" from \"@ONF2\" T0 inner join OEXD T1 on T0.\"U_Freight\" = T1.\"ExpnsCode\"  where T0.\"Code\" = 'CONF' and T0.\"U_Freight\" != ''");
                oRec.DoQuery("Select T0.\"U_Freight\",T0.\"U_Enabled\",T0.\"LineId\",T1.\"ExpnsName\",T0.\"U_IsDiscount\",T0.\"U_DefaultValue\" from \"@ONF2\" T0 inner join OEXD T1 on T0.\"U_Freight\" = T1.\"ExpnsCode\"  where T0.\"Code\" = 'CONF' and T0.\"U_Freight\" != ''");
                FreightCodes.Clear();
                while (!oRec.EoF)
                {
                    StatusConfig statusConfig = new StatusConfig();
                    statusConfig.FreightCode = oRec.Fields.Item("U_Freight").Value.ToString();
                    statusConfig.FreightEnabled = oRec.Fields.Item("U_Enabled").Value.ToString();
                    statusConfig.Line = oRec.Fields.Item("LineId").Value.ToString();
                    statusConfig.FreightName = oRec.Fields.Item("ExpnsName").Value.ToString();
                    statusConfig.ScholarshipDiscount = oRec.Fields.Item("U_IsDiscount").Value.ToString();
                    statusConfig.DefaultFreightAmount = oRec.Fields.Item("U_DefaultValue").Value.ToString();
                    FreightCodes.Add(statusConfig);
                    oRec.MoveNext();
                }
            }
            catch (Exception ex)
            {
                Global.SetMessage(ex.Message, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oRec);
                oRec = null;
                GC.Collect();
            }
        }
    }
    public class StatusConfig
    {
        public string Status, Program, Courses, FreightCode, FreightEnabled, Line, FreightName, ScholarshipDiscount, DefaultFreightAmount;
    }
}
