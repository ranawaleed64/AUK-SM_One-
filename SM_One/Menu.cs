using SAPbouiCOM.Framework;
using SM_One.SAP;
using System;
using System.Collections.Generic;
using System.Text;

namespace SM_One
{
    class Menu
    {
        public void SBO_Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
       
            try
            {
                if (pVal.BeforeAction && pVal.MenuUID == "BL_SM4")
                {
                    ImportRegistrations activeForm = new ImportRegistrations();
                    activeForm.UIAPIRawForm.State = SAPbouiCOM.BoFormStateEnum.fs_Maximized;
                    activeForm.Show();
                }
                else if (pVal.BeforeAction && pVal.MenuUID == "BL_SM5")
                {
                    SAPbouiCOM.MenuItem oMenu = Global.myApi.Menus.Item("51200");
                    for (int i = 0; i < oMenu.SubMenus.Count; i++)
                    {
                        string MenuName = oMenu.SubMenus.Item(i).String;
                        string MenuID = oMenu.SubMenus.Item(i).UID;
                        if (MenuName == "OSEM - Semester Setup")
                        {
                            Global.myApi.ActivateMenuItem(MenuID);
                            break;
                        }
                    }
                    
                }
                else if (pVal.BeforeAction && pVal.MenuUID == "BL_SM6")
                {
                    Registration activeForm = new Registration();
                    activeForm.UIAPIRawForm.State = SAPbouiCOM.BoFormStateEnum.fs_Maximized;
                    activeForm.Show();
                }
                else if (pVal.BeforeAction && pVal.MenuUID == "BL_SM7")
                {
                    GenerateInvoices activeForm = new GenerateInvoices("R");
                    activeForm.UIAPIRawForm.State = SAPbouiCOM.BoFormStateEnum.fs_Maximized;
                    activeForm.UIAPIRawForm.Title += " (Invoices)";
                    activeForm.Show();
                    activeForm.cmbDocType.Select("R", SAPbouiCOM.BoSearchKey.psk_ByValue);
                 
                }
                else if (pVal.BeforeAction && pVal.MenuUID == "BL_SM8")
                {
                    //Global.myApi.ActivateMenuItem("51201");
                   SAPbouiCOM.MenuItem  oMenu = Global.myApi.Menus.Item("51200");
                    for (int i = 0; i < oMenu.SubMenus.Count; i++)
                    {
                        string MenuName = oMenu.SubMenus.Item(i).String;
                        string MenuID = oMenu.SubMenus.Item(i).UID;
                        if (MenuName == "ORMP - Revenue Mapping")
                        {
                            Global.myApi.ActivateMenuItem(MenuID);
                            break;
                        }
                    }
                    //Global.myApi.OpenForm(SAPbouiCOM.BoFormObjectEnum.fo_UserDefinedObject, "-3@ORMP", "");
                    //Global.myApi.Forms.ActiveForm
                }
                else if (pVal.BeforeAction && pVal.MenuUID == "BL_SM3")
                {
                    //Global.myApi.ActivateMenuItem("51201");
                    SAPbouiCOM.MenuItem oMenu = Global.myApi.Menus.Item("51200");
                    for (int i = 0; i < oMenu.SubMenus.Count; i++)
                    {
                        string MenuName = oMenu.SubMenus.Item(i).String;
                        string MenuID = oMenu.SubMenus.Item(i).UID;
                        if (MenuName == "OSHL - Scholarship Setup")
                        {
                            Global.myApi.ActivateMenuItem(MenuID);
                            break;
                        }
                    }
                    //Global.myApi.OpenForm(SAPbouiCOM.BoFormObjectEnum.fo_UserDefinedObject, "-3@ORMP", "");
                    //Global.myApi.Forms.ActiveForm
                }
                else if (pVal.BeforeAction && pVal.MenuUID == "BL_SM2")
                {
                    SMConfig sMConfig = new SMConfig();
                    sMConfig.UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE;
                    sMConfig.txtCode.String = "CONF";
                    sMConfig.btnSave.Item.Click(); sMConfig.Show();
                }
                else if (pVal.BeforeAction && pVal.MenuUID == "BL_SM10")
                {
                    GenerateInvoices activeForm = new GenerateInvoices("C");
               
                    activeForm.UIAPIRawForm.Title += " (Refunds)";
                    activeForm.cmbDocType.Select("C", SAPbouiCOM.BoSearchKey.psk_ByValue);
                    activeForm.Show();
                    activeForm.UIAPIRawForm.State = SAPbouiCOM.BoFormStateEnum.fs_Maximized;
                    activeForm.cmbCancelType.Item.Enabled = true;
                }
                else if (pVal.BeforeAction && pVal.MenuUID == "BL_SM11")
                {
                    ERPIntegration activeForm = new ERPIntegration();
                    activeForm.UIAPIRawForm.State = SAPbouiCOM.BoFormStateEnum.fs_Maximized;
                    activeForm.Show();
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.ToString(), 1, "Ok", "", "");
            }
        }

    }
}
