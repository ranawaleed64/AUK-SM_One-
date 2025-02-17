using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
namespace SM_One
{
    [FormAttribute("SM_One.SMConfig", "SAP/SMConfig.b1f")]
    class SMConfig : UserFormBase
    {
        public SMConfig()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.Folder0 = ((SAPbouiCOM.Folder)(this.GetItem("tab1").Specific));
            this.btnSave = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.btnCancel = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.txtCode = ((SAPbouiCOM.EditText)(this.GetItem("etCode").Specific));
            this.cmbVoucherSeries = ((SAPbouiCOM.ComboBox)(this.GetItem("cbVSeries").Specific));
            this.cmbInvoiceSeries = ((SAPbouiCOM.ComboBox)(this.GetItem("cbInvSer").Specific));
            this.stVoucherSeries = ((SAPbouiCOM.StaticText)(this.GetItem("stVSeries").Specific));
            this.stInvoiceSeries = ((SAPbouiCOM.StaticText)(this.GetItem("stInvSer").Specific));
            this.Folder0.Select();
            this.txtVoucherDebit = ((SAPbouiCOM.EditText)(this.GetItem("etVDr").Specific));
            this.txtVoucherCredit = ((SAPbouiCOM.EditText)(this.GetItem("etVCr").Specific));
            this.cmbFromAccounts = ((SAPbouiCOM.ComboBox)(this.GetItem("cbAccounts").Specific));
            this.stAccounts = ((SAPbouiCOM.StaticText)(this.GetItem("stAccounts").Specific));
            this.stVoucherDebit = ((SAPbouiCOM.StaticText)(this.GetItem("stVDr").Specific));
            this.stVoucherCredit = ((SAPbouiCOM.StaticText)(this.GetItem("stVCr").Specific));
            this.cbCancellation = ((SAPbouiCOM.CheckBox)(this.GetItem("cbCancel").Specific));
            this.cbRefund = ((SAPbouiCOM.CheckBox)(this.GetItem("cbRefund").Specific));
            this.cbNonRefund = ((SAPbouiCOM.CheckBox)(this.GetItem("cbNRefund").Specific));
            this.LinkedButton0 = ((SAPbouiCOM.LinkedButton)(this.GetItem("lbVDr").Specific));
            this.LinkedButton1 = ((SAPbouiCOM.LinkedButton)(this.GetItem("lbVCr").Specific));
            this.txtCode.Item.Width = 0;
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("stPDim").Specific));
            this.cmbProgramDim = ((SAPbouiCOM.ComboBox)(this.GetItem("cbPDim").Specific));
            this.txtGroupName = ((SAPbouiCOM.EditText)(this.GetItem("etBPGrpN").Specific));
            this.txtGroupName.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.txtGroupName_ChooseFromListAfter);
            this.txtGroupName.ChooseFromListBefore += new SAPbouiCOM._IEditTextEvents_ChooseFromListBeforeEventHandler(this.txtGroupName_ChooseFromListBefore);
            this.stBPGroup = ((SAPbouiCOM.StaticText)(this.GetItem("stBPGrp").Specific));
            this.txtGroupCode = ((SAPbouiCOM.EditText)(this.GetItem("etGrpC").Specific));
            this.txtGroupCode.Item.Width = 0;
            this.PictureBox0 = ((SAPbouiCOM.PictureBox)(this.GetItem("pbLogo").Specific));
            this.cbFiscalNotCalendar = ((SAPbouiCOM.CheckBox)(this.GetItem("chFiscal").Specific));
            this.cbFiscalNotCalendar.PressedAfter += new SAPbouiCOM._ICheckBoxEvents_PressedAfterEventHandler(this.cbFiscalNotCalendar_PressedAfter);
            this.cbUpdatePriceList = ((SAPbouiCOM.CheckBox)(this.GetItem("chPriceL").Specific));
            this.cmbProratedOn = ((SAPbouiCOM.ComboBox)(this.GetItem("cbProrate").Specific));
            this.stProratedOn = ((SAPbouiCOM.StaticText)(this.GetItem("stProrate").Specific));
            this.stAdd1 = ((SAPbouiCOM.StaticText)(this.GetItem("stadd1").Specific));
            this.stAdd2 = ((SAPbouiCOM.StaticText)(this.GetItem("stadd2").Specific));
            this.stAdd3 = ((SAPbouiCOM.StaticText)(this.GetItem("stadd3").Specific));
            this.stAdd4 = ((SAPbouiCOM.StaticText)(this.GetItem("stadd4").Specific));
            this.cmbSem1StartDate = ((SAPbouiCOM.ComboBox)(this.GetItem("cbSem1S").Specific));
            this.cmbSem1EndDate = ((SAPbouiCOM.ComboBox)(this.GetItem("cbSem1E").Specific));
            this.cmbSem2StartDate = ((SAPbouiCOM.ComboBox)(this.GetItem("cbSem2S").Specific));
            this.cmbSem2EndDate = ((SAPbouiCOM.ComboBox)(this.GetItem("cbSem2E").Specific));
            this.stSem1Start = ((SAPbouiCOM.StaticText)(this.GetItem("stSem1S").Specific));
            this.stSem1End = ((SAPbouiCOM.StaticText)(this.GetItem("stSem1E").Specific));
            this.stSem2Start = ((SAPbouiCOM.StaticText)(this.GetItem("stSem2S").Specific));
            this.stSem2End = ((SAPbouiCOM.StaticText)(this.GetItem("stSem2E").Specific));
            this.Folder1 = ((SAPbouiCOM.Folder)(this.GetItem("tab2").Specific));
            this.Folder1.PressedAfter += new SAPbouiCOM._IFolderEvents_PressedAfterEventHandler(this.Folder1_PressedAfter);
            this.Matrix0 = ((SAPbouiCOM.Matrix)(this.GetItem("Matrix0").Specific));
            this.Matrix0.AutoResizeColumns();
            this.btnAddRow = ((SAPbouiCOM.Button)(this.GetItem("btAddRow").Specific));
            this.btnAddRow.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btnAddRow_PressedAfter);
            this.btnDelRow = ((SAPbouiCOM.Button)(this.GetItem("btDelRow").Specific));
            this.btnDelRow.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btnDelRow_PressedAfter);
            this.RowsDb = this.UIAPIRawForm.DataSources.DBDataSources.Item("@ONF1");
            this.Matrix1 = ((SAPbouiCOM.Matrix)(this.GetItem("Matrix1").Specific));
            this.Matrix1.PressedAfter += new SAPbouiCOM._IMatrixEvents_PressedAfterEventHandler(this.Matrix1_PressedAfter);
            this.Matrix1.ValidateAfter += new SAPbouiCOM._IMatrixEvents_ValidateAfterEventHandler(this.Matrix1_ValidateAfter);
            this.RowsDb2 = this.UIAPIRawForm.DataSources.DBDataSources.Item("@ONF2");
            this.Matrix1.ComboSelectAfter += new SAPbouiCOM._IMatrixEvents_ComboSelectAfterEventHandler(this.Matrix1_ComboSelectAfter);
            this.Matrix1.ClickBefore += new SAPbouiCOM._IMatrixEvents_ClickBeforeEventHandler(this.Matrix1_ClickBefore);
            this.Matrix1.KeyDownBefore += new SAPbouiCOM._IMatrixEvents_KeyDownBeforeEventHandler(this.Matrix1_KeyDownBefore);
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_2").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_3").Specific));
            this.cbSendInvoice = ((SAPbouiCOM.CheckBox)(this.GetItem("cbSendInv").Specific));
            this.cmbPricedOn = ((SAPbouiCOM.ComboBox)(this.GetItem("cbPriceOn").Specific));
            this.StaticText3 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_4").Specific));
            this.Folder2 = ((SAPbouiCOM.Folder)(this.GetItem("tab4").Specific));
            this.txtSQLServer = ((SAPbouiCOM.EditText)(this.GetItem("etSQLS").Specific));
            this.txtSQLDatabase = ((SAPbouiCOM.EditText)(this.GetItem("etSQLD").Specific));
            this.txtSQLUsername = ((SAPbouiCOM.EditText)(this.GetItem("etSQLU").Specific));
            this.txtSQLPassword = ((SAPbouiCOM.EditText)(this.GetItem("etSQLP").Specific));
            this.StaticText8 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_5").Specific));
            this.StaticText9 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_6").Specific));
            this.StaticText10 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_7").Specific));
            this.StaticText11 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_8").Specific));
            this.btnTest = ((SAPbouiCOM.Button)(this.GetItem("btTest").Specific));
            this.btnTest.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btnTest_PressedAfter);
            this.StaticText12 = ((SAPbouiCOM.StaticText)(this.GetItem("stSDim").Specific));
            this.cmbScholarDim = ((SAPbouiCOM.ComboBox)(this.GetItem("cbSDim").Specific));
            this.cmbTaxGroup = ((SAPbouiCOM.ComboBox)(this.GetItem("cbTaxGrp").Specific));
            this.StaticText13 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_11").Specific));
            this.StaticText14 = ((SAPbouiCOM.StaticText)(this.GetItem("StCDim").Specific));
            this.cmbCollegeDim = ((SAPbouiCOM.ComboBox)(this.GetItem("cbCDim").Specific));
            this.cmbItemSeries = ((SAPbouiCOM.ComboBox)(this.GetItem("cbItmSer").Specific));
            this.StaticText15 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_9").Specific));
            this.cmbRepeatScholarship = ((SAPbouiCOM.ComboBox)(this.GetItem("cbRepSch").Specific));
            this.StaticText16 = ((SAPbouiCOM.StaticText)(this.GetItem("stRepSch").Specific));
            this.FillCombos();
            this.OnCustomInitialize();

        }
        SAPbouiCOM.DBDataSource RowsDb;
        SAPbouiCOM.DBDataSource RowsDb2;
        private void FillCombos()
        {
            this.PictureBox0.Picture = (SM_One.Global.CurrentDirectory + "\\UniPic.jpg");
            Global.myApi.MenuEvent += this.MyApi_MenuEvent;
            Global.FillCombo(this.cmbVoucherSeries, ((SAPbouiCOM.Form)(this.UIAPIRawForm)), "Select \"Series\" as \"Code\",\"SeriesName\" as \"Name\" from NNM1 where \"ObjectCode\" = '30' and \"Locked\" = \'N\'", "");
            Global.FillCombo(this.cmbInvoiceSeries, ((SAPbouiCOM.Form)(this.UIAPIRawForm)), "Select \"Series\" as \"Code\",\"SeriesName\" as \"Name\" from NNM1 where \"ObjectCode\" = '13' and \"Locked\" = \'N\'", "");
            Global.FillCombo(this.cmbItemSeries, ((SAPbouiCOM.Form)(this.UIAPIRawForm)), "Select \"Series\" as \"Code\",\"SeriesName\" as \"Name\" from NNM1 where \"ObjectCode\" = '4' and \"Locked\" = 'N'", "");
            Global.FillCombo(this.cmbProgramDim, ((SAPbouiCOM.Form)(this.UIAPIRawForm)), "Select \"DimCode\" as \"Code\",\"DimDesc\" as \"Name\" from ODIM where \"DimActive\" = 'Y'", "");
            Global.FillCombo(this.cmbScholarDim, ((SAPbouiCOM.Form)(this.UIAPIRawForm)), "Select \"DimCode\" as \"Code\",\"DimDesc\" as \"Name\" from ODIM where \"DimActive\" = 'Y'", "");
            Global.FillCombo(this.cmbRepeatScholarship, ((SAPbouiCOM.Form)(this.UIAPIRawForm)), "Select \"Code\", \"Name\" from \"@OSHL\"", "");
            Global.FillCombo(this.cmbCollegeDim, ((SAPbouiCOM.Form)(this.UIAPIRawForm)), "Select \"DimCode\" as \"Code\",\"DimDesc\" as \"Name\" from ODIM where \"DimActive\" = 'Y'", "");
            Global.FillCombo(this.cmbTaxGroup, ((SAPbouiCOM.Form)(this.UIAPIRawForm)), "Select  \"Code\",\"Name\" from OVTG where \"Category\" = 'O' and \"Inactive\" ='N'", "");
            Global.FillCombo(((ComboBox)Matrix1.GetCellSpecific("Freight", 1)), ((SAPbouiCOM.Form)(this.UIAPIRawForm)), "Select \"ExpnsCode\" as \"Code\",\"ExpnsName\" as \"Name\" from OEXD", "");

            cmbVoucherSeries.ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly;
            cmbInvoiceSeries.ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly;
            cmbFromAccounts.ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly;
            this.cmbSem1StartDate.ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly;
            this.cmbSem1EndDate.ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly;
            this.cmbSem2StartDate.ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly;
            this.cmbSem2EndDate.ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly;

        }
        private void MyApi_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (Global.myApi.Forms.ActiveForm.UniqueID == UIAPIRawForm.UniqueID && (pVal.MenuUID == "1282" || pVal.MenuUID == "1281"))
            {
                BubbleEvent = false;
            }
        }
        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.DataUpdateAfter += new SAPbouiCOM.Framework.FormBase.DataUpdateAfterHandler(this.Form_DataUpdateAfter);
            this.CloseAfter += new SAPbouiCOM.Framework.FormBase.CloseAfterHandler(this.Form_CloseAfter);
            this.DataLoadAfter += new DataLoadAfterHandler(this.Form_DataLoadAfter);
        }

        private SAPbouiCOM.Folder Folder0;

        private void OnCustomInitialize()
        {

        }

        public SAPbouiCOM.Button btnSave;
        private SAPbouiCOM.Button btnCancel;
        public SAPbouiCOM.EditText txtCode;
        private SAPbouiCOM.ComboBox cmbVoucherSeries;
        private SAPbouiCOM.ComboBox cmbInvoiceSeries;
        private SAPbouiCOM.StaticText stVoucherSeries;
        private SAPbouiCOM.StaticText stInvoiceSeries;
        private SAPbouiCOM.EditText txtVoucherDebit;
        private SAPbouiCOM.EditText txtVoucherCredit;
        private SAPbouiCOM.ComboBox cmbFromAccounts;
        private SAPbouiCOM.StaticText stAccounts;
        private SAPbouiCOM.StaticText stVoucherDebit;
        private SAPbouiCOM.StaticText stVoucherCredit;
        private SAPbouiCOM.CheckBox cbCancellation;
        private SAPbouiCOM.CheckBox cbRefund;
        private SAPbouiCOM.CheckBox cbNonRefund;
        private SAPbouiCOM.LinkedButton LinkedButton0;
        private SAPbouiCOM.LinkedButton LinkedButton1;

        private void Form_DataUpdateAfter(ref SAPbouiCOM.BusinessObjectInfo pVal)
        {
            Config.LoadConfig();
            DisableRows();
        }

        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.ComboBox cmbProgramDim;
        private SAPbouiCOM.EditText txtGroupName;
        private SAPbouiCOM.StaticText stBPGroup;
        private SAPbouiCOM.EditText txtGroupCode;
        SAPbouiCOM.Conditions Cons;
        private void txtGroupName_ChooseFromListBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            Cons = null;
            string CFL = "CFL_2";
            UIAPIRawForm.ChooseFromLists.Item(CFL).SetConditions(Cons);
            Cons = UIAPIRawForm.ChooseFromLists.Item(CFL).GetConditions();
            SAPbouiCOM.Condition con = Cons.Add();
            con.Alias = "GroupType";
            con.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            con.CondVal = "C";
            UIAPIRawForm.ChooseFromLists.Item(CFL).SetConditions(Cons);

        }

        private void txtGroupName_ChooseFromListAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            ISBOChooseFromListEventArg args = (ISBOChooseFromListEventArg)pVal;
            if (args.SelectedObjects != null)
            {
                txtGroupCode.Value = args.SelectedObjects.GetValue("GroupCode", 0).ToString();
                txtGroupName.Value = args.SelectedObjects.GetValue("GroupName", 0).ToString();
            }
        }

        private PictureBox PictureBox0;

        private void Form_CloseAfter(SBOItemEventArg pVal)
        {
            Global.myApi.MenuEvent -= this.MyApi_MenuEvent;
        }

        private CheckBox cbFiscalNotCalendar;
        private CheckBox cbUpdatePriceList;
        private ComboBox cmbProratedOn;
        private StaticText stProratedOn;
        private StaticText stAdd1;
        private StaticText stAdd2;
        private StaticText stAdd3;
        private StaticText stAdd4;
        private ComboBox cmbSem1StartDate;
        private ComboBox cmbSem1EndDate;
        private ComboBox cmbSem2StartDate;
        private ComboBox cmbSem2EndDate;
        private StaticText stSem1Start;
        private StaticText stSem1End;
        private StaticText stSem2Start;
        private StaticText stSem2End;

        private void cbFiscalNotCalendar_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            UIAPIRawForm.Freeze(true);
            if (!cbFiscalNotCalendar.Checked)
            {
                cmbSem1StartDate.Select(0, BoSearchKey.psk_Index);
                cmbSem1EndDate.Select(0, BoSearchKey.psk_Index);
                cmbSem2StartDate.Select(0, BoSearchKey.psk_Index);
                cmbSem2EndDate.Select(0, BoSearchKey.psk_Index);
                txtVoucherCredit.Item.Click();
                cmbSem1StartDate.Item.Enabled = false;
                cmbSem1EndDate.Item.Enabled = false;
                cmbSem2StartDate.Item.Enabled = false;
                cmbSem2EndDate.Item.Enabled = false;
            }
            else
            {
                cmbSem1StartDate.Item.Enabled = true;
                cmbSem1EndDate.Item.Enabled = true;
                cmbSem2StartDate.Item.Enabled = true;
                cmbSem2EndDate.Item.Enabled = true;
            }
            UIAPIRawForm.Freeze(false);
        }
        private void DisableRows()
        {
            for (int i = 0; i < Matrix1.RowCount; i++)
            {
                if (((CheckBox)Matrix1.GetCellSpecific("Enabled", i + 1)).Checked)
                {
                    Matrix1.CommonSetting.SetRowEditable(i + 1, false);
                }
                else
                {
                    Matrix1.CommonSetting.SetRowEditable(i + 1, true);
                }
            }
            Matrix1.Columns.Item("IsDiscount").Editable = true;
            Matrix1.Columns.Item("Default").Editable = true;
        }
        private void Form_DataLoadAfter(ref BusinessObjectInfo pVal)
        {
            if (!cbFiscalNotCalendar.Checked)
            {
                txtVoucherCredit.Item.Click();
                cmbSem1StartDate.Item.Enabled = false;
                cmbSem1EndDate.Item.Enabled = false;
                cmbSem2StartDate.Item.Enabled = false;
                cmbSem2EndDate.Item.Enabled = false;
            }
            else
            {
                cmbSem1StartDate.Item.Enabled = true;
                cmbSem1EndDate.Item.Enabled = true;
                cmbSem2StartDate.Item.Enabled = true;
                cmbSem2EndDate.Item.Enabled = true;
            }
            DisableRows();
        }

        private Folder Folder1;
        private Matrix Matrix0;
        private Button btnAddRow;
        private Button btnDelRow;

        private void btnAddRow_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (UIAPIRawForm.PaneLevel == 2)
            {
                UIAPIRawForm.Freeze(true);
                RowsDb.InsertRecord(RowsDb.Size);
                Matrix0.LoadFromDataSource();
                UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;
                UIAPIRawForm.Freeze(false);
            }
            if (UIAPIRawForm.PaneLevel == 3)
            {
                UIAPIRawForm.Freeze(true);
                RowsDb2.InsertRecord(RowsDb.Size);
                Matrix1.LoadFromDataSource();
                UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;
                UIAPIRawForm.Freeze(false);
            }
        }



        private void btnDelRow_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (UIAPIRawForm.PaneLevel == 2)
            {
                int SelectedRowIndex = Matrix0.GetNextSelectedRow();
                if (SelectedRowIndex > 0)
                {
                    UIAPIRawForm.Freeze(true);
                    Matrix0.DeleteRow(SelectedRowIndex);
                    Matrix0.FlushToDataSource();
                    Matrix0.LoadFromDataSource();
                    UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;
                    UIAPIRawForm.Freeze(false);
                }
            }
            if (UIAPIRawForm.PaneLevel == 3)
            {
                int SelectedRowIndex = Matrix1.GetNextSelectedRow();
                if (SelectedRowIndex > 0)
                {
                    UIAPIRawForm.Freeze(true);
                    Matrix1.DeleteRow(SelectedRowIndex);
                    Matrix1.FlushToDataSource();
                    Matrix1.LoadFromDataSource();
                    UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;
                    UIAPIRawForm.Freeze(false);
                }
            }
        }

        private void Folder1_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            UIAPIRawForm.Freeze(true);
            Matrix0.AutoResizeColumns();
            UIAPIRawForm.Freeze(false);
        }

        private Matrix Matrix1;
        private StaticText StaticText1;
        private StaticText StaticText2;



        private void Matrix1_KeyDownBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if ((pVal.ColUID == "Enabled" || pVal.ColUID == "IsDiscount") && pVal.Row > 0)
            {
                string Val = ((ComboBox)Matrix1.GetCellSpecific("Freight", pVal.Row)).Value;
                if (string.IsNullOrEmpty(Val))
                {
                    Global.SetMessage("Select Freight First", BoStatusBarMessageType.smt_Warning);
                    BubbleEvent = false;
                    return;
                }
                int ret = Global.myApi.MessageBox("Are you sure you want to enable?\nNote: This is an irreversible action", 1, "Yes", "No");
                if (ret != 1)
                {
                    BubbleEvent = false;
                }
            }
            if (pVal.ColUID == "IsDiscount")
            {
                for (int i = 0; i < Matrix1.RowCount; i++)
                {
                    if (((CheckBox)Matrix1.GetCellSpecific("IsDiscount", i)).Checked && i != pVal.Row)
                    {
                        Global.SetMessage("Only 1 Freight can be marked as Discount", BoStatusBarMessageType.smt_Error);
                        ((CheckBox)Matrix1.GetCellSpecific("IsDiscount", i)).Checked = false;
                    }
                }
            }

        }
        private void Matrix1_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if ((pVal.ColUID == "Enabled" || pVal.ColUID == "IsDiscount") && pVal.Row > 0)
            {
                string Val = ((ComboBox)Matrix1.GetCellSpecific("Freight", pVal.Row)).Value;
                if (string.IsNullOrEmpty(Val))
                {
                    Global.SetMessage("Select Freight First", BoStatusBarMessageType.smt_Warning);
                    BubbleEvent = false;
                    return;
                }
                int ret = Global.myApi.MessageBox("Are you sure you want to enable this Freight?\nNote: This is an irreversible action", 1, "Yes", "No");
                if (ret != 1)
                {
                    BubbleEvent = false;
                }
                else
                {
                    ((EditText)Matrix1.GetCellSpecific("Default", pVal.Row)).Value = "0";
                }
            }
            if (pVal.ColUID == "IsDiscount")
            {
                
                for (int i = 0; i < Matrix1.RowCount; i++)
                {
                    if (((CheckBox)Matrix1.GetCellSpecific("IsDiscount", i)).Checked && i != pVal.Row)
                    {
                        Global.SetMessage("Only 1 Freight can be marked as Discount", BoStatusBarMessageType.smt_Error);
                        ((CheckBox)Matrix1.GetCellSpecific("IsDiscount", i)).Checked = false;
                    }
                }
            }

        }

        private void Matrix1_ComboSelectAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (pVal.ColUID == "Freight" && pVal.ItemChanged)
            {
                string Val = ((ComboBox)Matrix1.GetCellSpecific("Freight", pVal.Row)).Value;
                if (Config.FreightCodes.Where(x => x.FreightCode == Val).ToList().Count > 0)
                {
                    Global.SetMessage("This freight is already setup on another line", BoStatusBarMessageType.smt_Error);
                    ((ComboBox)Matrix1.GetCellSpecific("Freight", pVal.Row)).Select("", BoSearchKey.psk_ByValue);
                }
            }
        }

        private CheckBox cbSendInvoice;
        private ComboBox cmbPricedOn;
        private StaticText StaticText3;
        private Folder Folder2;
        private EditText txtSQLServer;
        private StaticText StaticText4;
        private StaticText StaticText5;
        private StaticText StaticText6;
        private StaticText StaticText7;
        private EditText EditText0;
        private EditText txtSQLDatabase;
        private EditText txtSQLUsername;
        private EditText txtSQLPassword;
        private StaticText StaticText8;
        private StaticText StaticText9;
        private StaticText StaticText10;
        private StaticText StaticText11;
        private Button btnTest;

        private void btnTest_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (Global.TestSqlConnection(Config.GlobalConnection))
            {
                Global.SetMessage("Connection to SQL Server Successful", BoStatusBarMessageType.smt_Success);
            }
            else
            {
                Global.SetMessage("Could Not Connect to SQL Server", BoStatusBarMessageType.smt_Error);
            }

        }

        private StaticText StaticText12;
        private ComboBox cmbScholarDim;
        private ComboBox cmbTaxGroup;
        private StaticText StaticText13;
        private StaticText StaticText14;
        private ComboBox cmbCollegeDim;
        private ComboBox cmbItemSeries;
        private StaticText StaticText15;
        private ComboBox cmbRepeatScholarship;
        private StaticText StaticText16;

        private void Matrix1_ValidateAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (pVal.ColUID == "Default" && ((CheckBox)Matrix1.GetCellSpecific("IsDiscount", pVal.Row)).Checked && !pVal.InnerEvent)
            {
                ((EditText)Matrix1.GetCellSpecific("Default", pVal.Row)).Value = "0";
                Global.SetMessage("Default Value for Discount is defined on Revenue Mapping", BoStatusBarMessageType.smt_Warning);
            }
        }

        private void Matrix1_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (pVal.ColUID == "IsDiscount" && !pVal.InnerEvent)
            {
                ((EditText)Matrix1.GetCellSpecific("Default", pVal.Row)).Value = "0";
                Global.SetMessage("Default Value for Discount is defined on Revenue Mapping", BoStatusBarMessageType.smt_Warning);
            }

        }
    }
}
