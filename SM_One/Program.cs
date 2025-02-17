using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using SAPbobsCOM;
namespace SM_One
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Application oApp = null;
                if (args.Length < 1)
                {
                    oApp = new Application();
                }
                else
                {
                    //If you want to use an add-on identifier for the development license, you can specify an add-on identifier string as the second parameter.
                    //oApp = new Application(args[0], "XXXXX");
                    oApp = new Application(args[0]);
                }
                Menu MyMenu = new Menu();
                //MyMenu.AddMenuItems();
                Global.Start();
                oApp.RegisterMenuEventHandler(MyMenu.SBO_Application_MenuEvent);
                Application.SBO_Application.AppEvent += new SAPbouiCOM._IApplicationEvents_AppEventEventHandler(SBO_Application_AppEvent);
                Application.SBO_Application.MenuEvent += SBO_Application_MenuEvent;
                oApp.Run();
             
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        private static bool firstClick = true;
        public static bool isOpen = false;

        private static void SBO_Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {

                //if (!pVal.BeforeAction && pVal.MenuUID == "524")
                //{
                //    if (CheckforDueVouchers() && !isOpen)
                //    {
                //        Postings activeForm = new Postings();
                //        activeForm.Show();
                //    }
                //}
                //if (firstClick)
                //{
                //    if (CheckforDueVouchers() && !isOpen)
                //    {
                //        Postings activeForm = new Postings();
                //        activeForm.Show();
                //    }
                //    firstClick = false;
                //}
            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.ToString(), 1, "Ok", "", "");
            }
        }

        private static bool CheckforDueVouchers()
        {
            try
            {
                Recordset oRec = Global.Comp_DI.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
                string Query = "";
                if (Global.Comp_DI.DbServerType == BoDataServerTypes.dst_HANADB)
                {
                    Query = "Select T0.\"BatchNum\" as \"Voucher No.\",T0.\"RefDate\",T0.\"DueDate\",T1.\"LocTotal\" from OBTF T0 inner join OBTD T1 on T0.\"BatchNum\" = T1.\"BatchNum\" where T0.\"BtfStatus\"='O' and T0.\"U_StVoucher\" = 'Y' and DAYS_BETWEEN(T0.\"DueDate\",Current_Date) > -1";
                }
                else
                {
                    Query = "Select T0.\"BatchNum\" as \"Voucher No.\",T0.\"RefDate\",T0.\"DueDate\",T1.\"LocTotal\" from OBTF T0 inner join OBTD T1 on T0.\"BatchNum\" = T1.\"BatchNum\" where T0.\"BtfStatus\"='O' and T0.\"U_StVoucher\" = 'Y' and DATEDIFF(day,T0.\"DueDate\",getdate()) > -1";
                }

                oRec.DoQuery(Query);
                if (oRec.RecordCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

               Global.SetMessage(ex.Message, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
               return false;    
            }
        }
        static void SBO_Application_AppEvent(SAPbouiCOM.BoAppEventTypes EventType)
        {
            switch (EventType)
            {
                case SAPbouiCOM.BoAppEventTypes.aet_ShutDown:
                    //Exit Add-On
                    System.Windows.Forms.Application.Exit();
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_CompanyChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_FontChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_LanguageChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_ServerTerminition:
                    break;
                default:
                    break;
            }
        }
    }
}
