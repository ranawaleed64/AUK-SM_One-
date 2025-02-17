using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SM_One
{
    [FormAttribute("SM_One.Registration", "SAP/Registration.b1f")]
    class Registration : UserFormBase
    {
        public Registration()
        {
        }
        bool KeepOpen = false;
        SAPbouiCOM.Form oForm;
        public Registration(bool keepOpen, SAPbouiCOM.Form oform)
        {
            KeepOpen = keepOpen;
            oForm = oform;
        }
        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.txtStudentID = ((SAPbouiCOM.EditText)(this.GetItem("etID").Specific));
            this.txtStudentID.KeyDownBefore += new SAPbouiCOM._IEditTextEvents_KeyDownBeforeEventHandler(this.txtStudentID_KeyDownBefore);
            this.txtStudentID.ClickBefore += new SAPbouiCOM._IEditTextEvents_ClickBeforeEventHandler(this.txtStudentID_ClickBefore);
            this.txtInvEntry = ((SAPbouiCOM.EditText)(this.GetItem("etInvEnt").Specific));
            this.txtInvEntry.KeyDownBefore += new SAPbouiCOM._IEditTextEvents_KeyDownBeforeEventHandler(this.txtInvEntry_KeyDownBefore);
            this.txtInvEntry.ClickBefore += new SAPbouiCOM._IEditTextEvents_ClickBeforeEventHandler(this.txtInvEntry_ClickBefore);
            this.txtEndDate = ((SAPbouiCOM.EditText)(this.GetItem("etEDate").Specific));
            this.txtEndDate.ClickBefore += new SAPbouiCOM._IEditTextEvents_ClickBeforeEventHandler(this.txtEndDate_ClickBefore);
            this.txtEndDate.KeyDownBefore += new SAPbouiCOM._IEditTextEvents_KeyDownBeforeEventHandler(this.txtEndDate_KeyDownBefore);
            this.txtStartDate = ((SAPbouiCOM.EditText)(this.GetItem("etSDate").Specific));
            this.txtStartDate.KeyDownBefore += new SAPbouiCOM._IEditTextEvents_KeyDownBeforeEventHandler(this.txtStartDate_KeyDownBefore);
            this.txtStartDate.ClickBefore += new SAPbouiCOM._IEditTextEvents_ClickBeforeEventHandler(this.txtStartDate_ClickBefore);
            this.txtStudentName = ((SAPbouiCOM.EditText)(this.GetItem("etName").Specific));
            this.txtStudentName.KeyDownBefore += new SAPbouiCOM._IEditTextEvents_KeyDownBeforeEventHandler(this.txtStudentName_KeyDownBefore);
            this.txtStudentName.ClickBefore += new SAPbouiCOM._IEditTextEvents_ClickBeforeEventHandler(this.txtStudentName_ClickBefore);
            this.txtInvNum = ((SAPbouiCOM.EditText)(this.GetItem("etInvNum").Specific));
            this.txtInvNum.KeyDownBefore += new SAPbouiCOM._IEditTextEvents_KeyDownBeforeEventHandler(this.txtInvNum_KeyDownBefore);
            this.txtInvNum.ClickBefore += new SAPbouiCOM._IEditTextEvents_ClickBeforeEventHandler(this.txtInvNum_ClickBefore);
            this.Matrix0 = ((SAPbouiCOM.Matrix)(this.GetItem("Matrix0").Specific));
            this.Matrix0.KeyDownBefore += new SAPbouiCOM._IMatrixEvents_KeyDownBeforeEventHandler(this.Matrix0_KeyDownBefore);
            this.Matrix0.ValidateBefore += new SAPbouiCOM._IMatrixEvents_ValidateBeforeEventHandler(this.Matrix0_ValidateBefore);
            this.Matrix0.ValidateAfter += new SAPbouiCOM._IMatrixEvents_ValidateAfterEventHandler(this.Matrix0_ValidateAfter);
            this.Matrix0.ClickBefore += new SAPbouiCOM._IMatrixEvents_ClickBeforeEventHandler(this.Matrix0_ClickBefore);
            //                                              this.Matrix0.PressedBefore += new SAPbouiCOM._IMatrixEvents_PressedBeforeEventHandler(this.Matrix0_PressedBefore);
            this.Matrix0.LinkPressedAfter += new SAPbouiCOM._IMatrixEvents_LinkPressedAfterEventHandler(this.Matrix0_LinkPressedAfter);
            this.cmbSemester = ((SAPbouiCOM.ComboBox)(this.GetItem("cbSemester").Specific));
            this.cmbSemester.KeyDownBefore += new SAPbouiCOM._IComboBoxEvents_KeyDownBeforeEventHandler(this.cmbSemester_KeyDownBefore);
            this.cmbSemester.ClickBefore += new SAPbouiCOM._IComboBoxEvents_ClickBeforeEventHandler(this.cmbSemester_ClickBefore);
            this.txtDocEntry = ((SAPbouiCOM.EditText)(this.GetItem("etReg").Specific));
            this.txtDocEntry.KeyDownBefore += new SAPbouiCOM._IEditTextEvents_KeyDownBeforeEventHandler(this.txtDocEntry_KeyDownBefore);
            this.txtDocEntry.ClickBefore += new SAPbouiCOM._IEditTextEvents_ClickBeforeEventHandler(this.txtDocEntry_ClickBefore);
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("stReg").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("stID").Specific));
            this.StaticText3 = ((SAPbouiCOM.StaticText)(this.GetItem("stPC").Specific));
            this.StaticText4 = ((SAPbouiCOM.StaticText)(this.GetItem("stSDate").Specific));
            this.StaticText6 = ((SAPbouiCOM.StaticText)(this.GetItem("stInvEnt").Specific));
            this.StaticText7 = ((SAPbouiCOM.StaticText)(this.GetItem("stSemester").Specific));
            this.StaticText8 = ((SAPbouiCOM.StaticText)(this.GetItem("stName").Specific));
            this.StaticText10 = ((SAPbouiCOM.StaticText)(this.GetItem("stEDate").Specific));
            this.StaticText12 = ((SAPbouiCOM.StaticText)(this.GetItem("stInvNum").Specific));
            this.StaticText17 = ((SAPbouiCOM.StaticText)(this.GetItem("stRInvEnt").Specific));
            this.StaticText18 = ((SAPbouiCOM.StaticText)(this.GetItem("stRInvNum").Specific));
            this.btnSave = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.txtRemarks = ((SAPbouiCOM.EditText)(this.GetItem("eeRemarks").Specific));
            this.StaticText13 = ((SAPbouiCOM.StaticText)(this.GetItem("stRemarks").Specific));
            this.LinkedButton0 = ((SAPbouiCOM.LinkedButton)(this.GetItem("lbID").Specific));
            this.Matrix0.AutoResizeColumns();
            //                                                        Global.myApi.MenuEvent += this.MyApi_MenuEvent;
            this.cmbStatus = ((SAPbouiCOM.ComboBox)(this.GetItem("cbStatus").Specific));
            this.cmbStatus.KeyDownBefore += new SAPbouiCOM._IComboBoxEvents_KeyDownBeforeEventHandler(this.cmbStatus_KeyDownBefore);
            this.cmbStatus.ClickBefore += new SAPbouiCOM._IComboBoxEvents_ClickBeforeEventHandler(this.cmbStatus_ClickBefore);
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("stStatus").Specific));
            this.cbInvoices = ((SAPbouiCOM.CheckBox)(this.GetItem("chInvoice").Specific));
            //                                this.cbInvoices.PressedBefore += new SAPbouiCOM._ICheckBoxEvents_PressedBeforeEventHandler(this.cbInvoices_PressedBefore);
            //                                this.cbInvoices.KeyDownBefore += new SAPbouiCOM._ICheckBoxEvents_KeyDownBeforeEventHandler(this.cbInvoices_KeyDownBefore);
            this.cbInvoices.ClickBefore += new SAPbouiCOM._ICheckBoxEvents_ClickBeforeEventHandler(this.cbInvoices_ClickBefore);
            this.LinkedButton1 = ((SAPbouiCOM.LinkedButton)(this.GetItem("lbInvEnt").Specific));
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("btAddItem").Specific));
            this.Button0.PressedBefore += new SAPbouiCOM._IButtonEvents_PressedBeforeEventHandler(this.Button0_PressedBefore);
            this.Button0.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button0_PressedAfter);
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.RowsDb = this.UIAPIRawForm.DataSources.DBDataSources.Item("@SRG1");
            this.Matrix0.Columns.Item("LineType").Visible = false;
            this.Button2 = ((SAPbouiCOM.Button)(this.GetItem("btRemItem").Specific));
            this.Button2.PressedBefore += new SAPbouiCOM._IButtonEvents_PressedBeforeEventHandler(this.Button2_PressedBefore);
            this.Button2.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button2_PressedAfter);
            this.txtDiscount = ((SAPbouiCOM.EditText)(this.GetItem("etDiscount").Specific));
            this.txtDiscount.ValidateBefore += new SAPbouiCOM._IEditTextEvents_ValidateBeforeEventHandler(this.txtDiscount_ValidateBefore);
            this.txtDiscount.ValidateAfter += new SAPbouiCOM._IEditTextEvents_ValidateAfterEventHandler(this.txtDiscount_ValidateAfter);
            this.cmbMajor = ((SAPbouiCOM.ComboBox)(this.GetItem("cmbMajor").Specific));
            this.cmbMajor.KeyDownBefore += new SAPbouiCOM._IComboBoxEvents_KeyDownBeforeEventHandler(this.cmbMajor_KeyDownBefore);
            this.cmbMajor.ClickBefore += new SAPbouiCOM._IComboBoxEvents_ClickBeforeEventHandler(this.cmbMajor_ClickBefore);
            this.cmbCollege = ((SAPbouiCOM.ComboBox)(this.GetItem("cmbCollege").Specific));
            this.cmbCollege.KeyDownBefore += new SAPbouiCOM._IComboBoxEvents_KeyDownBeforeEventHandler(this.cmbCollege_KeyDownBefore);
            this.cmbCollege.ClickBefore += new SAPbouiCOM._IComboBoxEvents_ClickBeforeEventHandler(this.cmbCollege_ClickBefore);
            this.cmbScholarship = ((SAPbouiCOM.ComboBox)(this.GetItem("cmbScholar").Specific));
            this.cmbScholarship.KeyDownBefore += new SAPbouiCOM._IComboBoxEvents_KeyDownBeforeEventHandler(this.cmbScholarship_KeyDownBefore);
            this.cmbScholarship.ClickBefore += new SAPbouiCOM._IComboBoxEvents_ClickBeforeEventHandler(this.cmbScholarship_ClickBefore);
            this.cmbRStatus = ((SAPbouiCOM.ComboBox)(this.GetItem("cbRStatus").Specific));
            this.cbHasRepeat = ((SAPbouiCOM.CheckBox)(this.GetItem("cbRepeat").Specific));
            this.cbHasRepeat.ClickBefore += new SAPbouiCOM._ICheckBoxEvents_ClickBeforeEventHandler(this.cbHasRepeat_ClickBefore);
            this.txtRepInvEntry = ((SAPbouiCOM.EditText)(this.GetItem("etRInvEnt").Specific));
            this.txtRepInvEntry.KeyDownBefore += new SAPbouiCOM._IEditTextEvents_KeyDownBeforeEventHandler(this.txtRepInvEntry_KeyDownBefore);
            this.txtRepInvEntry.ClickBefore += new SAPbouiCOM._IEditTextEvents_ClickBeforeEventHandler(this.txtRepInvEntry_ClickBefore);
            this.txtRepInvNum = ((SAPbouiCOM.EditText)(this.GetItem("etRInvNum").Specific));
            this.txtRepInvNum.KeyDownBefore += new SAPbouiCOM._IEditTextEvents_KeyDownBeforeEventHandler(this.txtRepInvNum_KeyDownBefore);
            this.txtRepInvNum.ClickBefore += new SAPbouiCOM._IEditTextEvents_ClickBeforeEventHandler(this.txtRepInvNum_ClickBefore);
            this.LinkedButton3 = ((SAPbouiCOM.LinkedButton)(this.GetItem("lbIRnvEnt").Specific));
            this.cbRInvoice = ((SAPbouiCOM.CheckBox)(this.GetItem("Item_13").Specific));
            this.cbRInvoice.ClickBefore += new SAPbouiCOM._ICheckBoxEvents_ClickBeforeEventHandler(this.cbRInvoice_ClickBefore);
            this.StaticText16 = ((SAPbouiCOM.StaticText)(this.GetItem("stHDisc").Specific));
            this.txtDiscountPC = ((SAPbouiCOM.EditText)(this.GetItem("etDiscPC").Specific));
            this.txtDiscountPC.ValidateBefore += new SAPbouiCOM._IEditTextEvents_ValidateBeforeEventHandler(this.txtDiscountPC_ValidateBefore);
            this.txtDiscountPC.ValidateAfter += new SAPbouiCOM._IEditTextEvents_ValidateAfterEventHandler(this.txtDiscountPC_ValidateAfter);
            this.txtGrossTotal = ((SAPbouiCOM.EditText)(this.GetItem("etGrTotal").Specific));
            this.txtSchDiscountPC = ((SAPbouiCOM.EditText)(this.GetItem("etSDiscPC").Specific));
            this.txtSchDiscount = ((SAPbouiCOM.EditText)(this.GetItem("etSDisc").Specific));
            this.txtTaxAmount = ((SAPbouiCOM.EditText)(this.GetItem("etTax").Specific));
            this.txtTaxPC = ((SAPbouiCOM.EditText)(this.GetItem("etTaxPC").Specific));
            this.txtAfterSchDiscount = ((SAPbouiCOM.EditText)(this.GetItem("etASDisc").Specific));
            this.txtBeforeTax = ((SAPbouiCOM.EditText)(this.GetItem("etADisc").Specific));
            this.txtDocTotal = ((SAPbouiCOM.EditText)(this.GetItem("etDocTotal").Specific));
            this.FillCombos();
            this.EditText1 = ((SAPbouiCOM.EditText)(this.GetItem("edApp").Specific));
            this.StaticText27 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_1").Specific));
            this.EditText2 = ((SAPbouiCOM.EditText)(this.GetItem("edRem").Specific));
            this.StaticText28 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_4").Specific));
            this.EditText5 = ((SAPbouiCOM.EditText)(this.GetItem("edRApp").Specific));
            this.StaticText31 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_10").Specific));
            this.EditText6 = ((SAPbouiCOM.EditText)(this.GetItem("edRRem").Specific));
            this.StaticText32 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_12").Specific));
            this.StaticText29 = ((SAPbouiCOM.StaticText)(this.GetItem("stCGPA").Specific));
            this.EditText3 = ((SAPbouiCOM.EditText)(this.GetItem("cbCGPA").Specific));
            this.OnCustomInitialize();

        }
        private void FillCombos()
        {
            Global.FillCombo(cmbCollege, (SAPbouiCOM.Form)UIAPIRawForm, "Select \"Code\",\"Name\" from \"@OCOL\"", "");
            Global.FillCombo(cmbMajor, (SAPbouiCOM.Form)UIAPIRawForm, "Select \"PrcCode\" as \"Code\",\"PrcName\" as \"Name\" from \"OPRC\" where \"DimCode\" = 3", "");
            Global.FillCombo(cmbScholarship, (SAPbouiCOM.Form)UIAPIRawForm, "Select \"Code\",\"Name\" from \"@OSHL\"", "");
            Global.FillCombo(cmbSemester, (SAPbouiCOM.Form)UIAPIRawForm, "Select \"Code\",\"Name\" from \"@OSEM\"", "");

            this.cmbSemester.ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly;
            this.cmbCollege.ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly;
            this.cmbScholarship.ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly;
            this.cmbMajor.ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly;
            this.cmbStatus.ExpandType = BoExpandType.et_DescriptionOnly;
            Matrix0.Columns.Item("Total").ColumnSetting.SumType = SAPbouiCOM.BoColumnSumType.bst_Auto;

        }
        private void MyApi_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (Global.myApi.Forms.ActiveForm.UniqueID == UIAPIRawForm.UniqueID && pVal.MenuUID == "1282")
            {
                BubbleEvent = false;
            }
        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.CloseAfter += new SAPbouiCOM.Framework.FormBase.CloseAfterHandler(this.Form_CloseAfter);
            this.DataLoadAfter += new DataLoadAfterHandler(this.Form_DataLoadAfter);

        }

        private SAPbouiCOM.EditText txtStudentID;

        private void OnCustomInitialize()
        {

        }
        private SAPbouiCOM.DBDataSource RowsDb;
        private SAPbouiCOM.EditText txtInvEntry;
        private SAPbouiCOM.EditText txtEndDate;
        private SAPbouiCOM.EditText txtStartDate;
        private SAPbouiCOM.EditText txtStudentName;
        private SAPbouiCOM.EditText txtInvNum;
        private SAPbouiCOM.Matrix Matrix0;
        private SAPbouiCOM.ComboBox cmbSemester;
        public SAPbouiCOM.EditText txtDocEntry;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.StaticText StaticText3;
        private SAPbouiCOM.StaticText StaticText4;
        private SAPbouiCOM.StaticText StaticText6;
        private SAPbouiCOM.StaticText StaticText7;
        private SAPbouiCOM.StaticText StaticText8;
        private SAPbouiCOM.StaticText StaticText10;
        private SAPbouiCOM.StaticText StaticText12;
        public SAPbouiCOM.Button btnSave;
        private SAPbouiCOM.EditText txtRemarks;
        private SAPbouiCOM.StaticText StaticText13;
        private SAPbouiCOM.LinkedButton LinkedButton0;

        private bool ValidateFind(int KeyStroke)
        {
            if ((Keys)KeyStroke == Keys.Escape)
            {
                return true;
            }
            if (UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_FIND_MODE)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void txtDocEntry_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);
        }

        private void txtDocEntry_KeyDownBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);


        }

     
        private void txtStartDate_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);

        }

        private void txtStartDate_KeyDownBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);

        }

    

        private void txtInvEntry_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);
        }

        private void txtInvEntry_KeyDownBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);
        }

        private void cmbSemester_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);


        }

        private void cmbSemester_KeyDownBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);


        }

        private void txtStudentName_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);


        }

        private void txtStudentName_KeyDownBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);


        }

   

        private void txtEndDate_KeyDownBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);


        }

        private void txtEndDate_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);


        }

        

        private void txtInvNum_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);
        }

        private void txtInvNum_KeyDownBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);
        }

        private SAPbouiCOM.ComboBox cmbStatus;
        private SAPbouiCOM.StaticText StaticText0;

        private void cmbStatus_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            //BubbleEvent = ValidateFind(pVal.CharPressed);
            BubbleEvent = true;

        }

        private void cmbStatus_KeyDownBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            //BubbleEvent = ValidateFind(pVal.CharPressed);
            BubbleEvent = true;
        }

        private SAPbouiCOM.CheckBox cbInvoices;

        private void cbInvoices_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);
        }

        private void cbInvoices_KeyDownBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);

        }

        private void cbInvoices_PressedBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);

        }

        private void Matrix0_LinkPressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            if ((pVal.ColUID == "TrailNo" || pVal.ColUID == "TFrom") && pVal.Row > -1)
            {
                string DocEntry = ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific(pVal.ColUID, pVal.Row)).Value;
                Registration active = new Registration();
                active.UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE;
                active.txtDocEntry.Value = DocEntry;
                active.btnSave.Item.Click(0);
                active.Show();
            }

        }

        private SAPbouiCOM.LinkedButton LinkedButton1;

        private void Form_CloseAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            if (KeepOpen)
            {
                ((SAPbouiCOM.Button)oForm.Items.Item("btRegNo").Specific).Item.Click();
            }

        }
        private bool CheckAndAllowTrailOrRetake(string ColUID, int Row)
        {

            bool IsTrail = ((SAPbouiCOM.CheckBox)Matrix0.GetCellSpecific("ToTrail", Row)).Checked;
            bool IsRetake = ((SAPbouiCOM.CheckBox)Matrix0.GetCellSpecific("Retake", Row)).Checked;
            bool IsAlreadyTrailed = ((SAPbouiCOM.CheckBox)Matrix0.GetCellSpecific("IsTrailed", Row)).Checked;
            if (IsAlreadyTrailed)
            {
                Global.SetMessage("This subject is already carried forward and cannot be changed", SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                return false;
            }
            else if (ColUID == "ToTrail" && IsRetake)
            {
                Global.SetMessage("This subject is already marked as Trail cannot be marked as Retake", SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                return false;
            }
            else if (ColUID == "Retake" && IsTrail)
            {
                Global.SetMessage("This subject is already marked as Trail cannot be marked as Retake", SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                return false;
            }
            else
            {
                return true;
            }
        }
        //private void Matrix0_PressedBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        //{
        //    BubbleEvent = true;

        //}

        private void Matrix0_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if ((pVal.ColUID == "ToTrail" || pVal.ColUID == "Retake") && (UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_OK_MODE || UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_UPDATE_MODE))
            {
                BubbleEvent = CheckAndAllowTrailOrRetake(pVal.ColUID, pVal.Row);
            }
            if ((pVal.ColUID == "Price" || pVal.ColUID == "Discount" || pVal.ColUID == "DiscountPC") && ((SAPbouiCOM.ComboBox)Matrix0.GetCellSpecific("LineType", pVal.Row)).Selected.Value == "A")
            {
                Global.SetMessage("Cannot edit Price or Discount of curriculum courses", BoStatusBarMessageType.smt_Warning);
                BubbleEvent = false;
            }

        }

        private void txtStudentID_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);
        }

        private void txtStudentID_KeyDownBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);
        }

        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;

        private void Button0_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {

            if (Config.AllStatusConfig.FirstOrDefault(x => x.Status.ToLower() == cmbRStatus.Selected.Value.ToLower()).Courses == "Y")
            {
                string SubjectCodes = "";
                for (int i = 1; i < Matrix0.RowCount + 1; i++)
                {
                    SubjectCodes += "'" + ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("SubCode", i)).Value + "',";
                }
                SubjectCodes = SubjectCodes.TrimEnd(',');
                SearchCourses searchCourses = new SearchCourses(cmbScholarship.Selected.Value, SubjectCodes, Matrix0, RowsDb, (SAPbouiCOM.Form)UIAPIRawForm, txtStudentID.Value, this);
                searchCourses.ExecuteQuery();
                searchCourses.Show();
            }
            else
            {
                Global.SetMessage("This status does not allow adding additional courses", SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
            }
        }

        private void Button0_PressedBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = false;
            if ((UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_OK_MODE || UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) && !cbInvoices.Checked)
            {
                BubbleEvent = true;
            }

        }

        private SAPbouiCOM.Button Button2;

        private void Button2_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            int SelectedRowIndex = Matrix0.GetNextSelectedRow();
            if (SelectedRowIndex > 0)
            {
                string LineType = ((SAPbouiCOM.ComboBox)Matrix0.GetCellSpecific("LineType", SelectedRowIndex)).Selected.Value;
                if (LineType == "M")
                {
                    UIAPIRawForm.Freeze(true);
                    Matrix0.DeleteRow(SelectedRowIndex);
                    Matrix0.FlushToDataSource();
                    Matrix0.LoadFromDataSource();
                    UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;
                    UIAPIRawForm.Freeze(false);
                }
                else
                {
                    Global.SetMessage("Cannot remove original registration line", SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                }
            }
        }

        private void Button2_PressedBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = false;
            if ((UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_OK_MODE || UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) && !cbInvoices.Checked)
            {
                BubbleEvent = true;
            }
        }

        private SAPbouiCOM.EditText txtDiscount;
        private SAPbouiCOM.StaticText StaticText14;
        private bool ValidateBeforeCalculate(int Row, string Col, out string ErrMessage)
        {
            double Price = 0;
            double Discount = 0;
            double DiscountPC = 0;
            double Total = 0;
            string sPrice = ((EditText)Matrix0.GetCellSpecific("Price", Row)).Value;
            string sDiscount = ((EditText)Matrix0.GetCellSpecific("Discount", Row)).Value;
            string sDiscountPC = ((EditText)Matrix0.GetCellSpecific("DiscountPC", Row)).Value;
            string sTotal = ((EditText)Matrix0.GetCellSpecific("Total", Row)).Value;
            double.TryParse(sPrice, out Price);
            double.TryParse(sDiscount, out Discount);
            double.TryParse(sDiscountPC, out DiscountPC);
            double.TryParse(sTotal, out Total);

            if ((Col == "Price" || Col == "Discount") && Price < Discount)
            {
                ErrMessage = "Discount cannot be greater than Price";
                return false;
            }
            else if (Col == "DiscountPC" && DiscountPC > 100)
            {
                ErrMessage = "Discount % cannot be greater than 100";
                return false;
            }
            else 
            {
                ErrMessage = "";
                return true;
            }
        }
        private void CalculateAndSetValues(int Row, string Col)
        {
            try
            {
                UIAPIRawForm.Freeze(true);
                double Price = 0;
                double Discount = 0;
                double DiscountPC = 0;
                double Total = 0;
                double Credits = 0;
                string sPrice = ((EditText)Matrix0.GetCellSpecific("Price", Row)).Value;
                string sDiscount = ((EditText)Matrix0.GetCellSpecific("Discount", Row)).Value;
                string sDiscountPC = ((EditText)Matrix0.GetCellSpecific("DiscountPC", Row)).Value;
                string sTotal = ((EditText)Matrix0.GetCellSpecific("Total", Row)).Value;
                string sCredits = ((EditText)Matrix0.GetCellSpecific("Credits", Row)).Value;
                double.TryParse(sPrice, out Price);
                double.TryParse(sDiscount, out Discount);
                double.TryParse(sDiscountPC, out DiscountPC);
                double.TryParse(sTotal, out Total);
                double.TryParse(sCredits, out Credits);

                if (Col == "Price")
                {
                    DiscountPC = (Discount / Price) * 100;
                }
                else if (Col == "Discount")
                {
                    DiscountPC = (Discount / Price) * 100;
                }
                else if (Col == "DiscountPC")
                {
                    Discount = (Price * (DiscountPC / 100));
                }
                Total = (Price * Credits) - Discount;
                UIAPIRawForm.DataSources.DBDataSources.Item("@SRG1").SetValue("U_Price", Row - 1, Price.ToString());
                UIAPIRawForm.DataSources.DBDataSources.Item("@SRG1").SetValue("U_Discount", Row - 1, Discount.ToString());
                UIAPIRawForm.DataSources.DBDataSources.Item("@SRG1").SetValue("U_DiscountPC", Row - 1, DiscountPC.ToString());
                UIAPIRawForm.DataSources.DBDataSources.Item("@SRG1").SetValue("U_Total", Row - 1, Total.ToString());

             


                //((EditText)Matrix0.GetCellSpecific("Price", Row)).Value = Price.ToString();
                //((EditText)Matrix0.GetCellSpecific("Discount", Row)).Value = Discount.ToString();
                //((EditText)Matrix0.GetCellSpecific("DiscountPC", Row)).Value = DiscountPC.ToString();
                //((EditText)Matrix0.GetCellSpecific("Total", Row)).Value = Total.ToString();
                //Matrix0.FlushToDataSource();
                Matrix0.LoadFromDataSource();
     
                
                
                UIAPIRawForm.Freeze(false);
            }
            catch (Exception ex)
            {
                UIAPIRawForm.Freeze(false);
                Global.SetMessage(ex.Message, BoStatusBarMessageType.smt_Error);
            }
        }

        private void Matrix0_ValidateAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            if (!pVal.InnerEvent && (pVal.ColUID == "Price" || pVal.ColUID == "Discount" || pVal.ColUID == "DiscountPC") && IsValidated)
            {
                CalculateAndSetValues(pVal.Row, pVal.ColUID);
                CalculateDiscounts(pVal.ItemUID);
            }
        }
        bool IsValidated = false;
        private void Matrix0_ValidateBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            string ErrMessage = "";
            IsValidated = false; 
            if (ValidateBeforeCalculate(pVal.Row, pVal.ColUID, out ErrMessage))
            {
                IsValidated = true;
                BubbleEvent = true;
            }
            else
            {
                Global.SetMessage(ErrMessage, BoStatusBarMessageType.smt_Error);
                IsValidated = false;
                BubbleEvent = false;
            }

        }

        private void Matrix0_KeyDownBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if ((pVal.ColUID == "Price" || pVal.ColUID == "Discount" || pVal.ColUID == "DiscountPC") && ((SAPbouiCOM.ComboBox)Matrix0.GetCellSpecific("LineType", pVal.Row)).Selected.Value == "A")
            {
                Global.SetMessage("Cannot edit Price or Discount of curriculum courses", BoStatusBarMessageType.smt_Warning);
                BubbleEvent = false;
            }

        }

        private void Form_DataLoadAfter(ref BusinessObjectInfo pVal)
        {
            if (cbInvoices.Checked)
            {
                this.Matrix0.Columns.Item("Discount").Editable = false;
                this.Matrix0.Columns.Item("DiscountPC").Editable = false;
                this.Matrix0.Columns.Item("Price").Editable = false;
            }
            else
            {
                this.Matrix0.Columns.Item("Discount").Editable = true;
                this.Matrix0.Columns.Item("DiscountPC").Editable = true;
                this.Matrix0.Columns.Item("Price").Editable = true;
            }
            Matrix0.AutoResizeColumns();
        }

        private SAPbouiCOM.ComboBox cmbMajor;
        private StaticText StaticText9;
        private SAPbouiCOM.ComboBox cmbCollege;
        private StaticText StaticText11;
        private SAPbouiCOM.ComboBox cmbScholarship;
        private StaticText StaticText15;
        private SAPbouiCOM.ComboBox cmbRStatus;
        private StaticText StaticText5;
        private SAPbouiCOM.CheckBox cbHasRepeat;

        private void cbHasRepeat_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);
        }

        private EditText txtRepInvEntry;
        private EditText txtRepInvNum;
        private StaticText StaticText17;
        private StaticText StaticText18;
        private LinkedButton LinkedButton3;

        private void txtRepInvEntry_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);

        }

        private void txtRepInvEntry_KeyDownBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);
        }

        private void txtRepInvNum_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);
        }

        private void txtRepInvNum_KeyDownBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);
        }

        private SAPbouiCOM.CheckBox cbRInvoice;

        private void cbRInvoice_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);
        }

        private void cmbMajor_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);

        }

        private void cmbMajor_KeyDownBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);

        }

        private void cmbScholarship_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);

        }

        private void cmbScholarship_KeyDownBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);

        }

        private void cmbCollege_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);

        }

        private void cmbCollege_KeyDownBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);

        }

        private StaticText StaticText16;
        private EditText txtDiscountPC;

        private void txtDiscountPC_ValidateAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (!pVal.InnerEvent)
            {
                CalculateDiscounts(pVal.ItemUID);
            }
        }

        public void CalculateDiscounts(string ChangeItem)
        {
            string sGrossTotal = Matrix0.Columns.Item("Total").ColumnSetting.SumValue;
            txtGrossTotal.Value = sGrossTotal;
            double GrossTotal = 0;
            double.TryParse(sGrossTotal, out GrossTotal);
            string sSchDiscountPC = txtSchDiscountPC.Value;
            double SchDiscountPC = 0;
            double.TryParse(sSchDiscountPC, out SchDiscountPC);

            string sSchDiscount = (GrossTotal * (SchDiscountPC / 100)).ToString();
            double SchDiscount = 0;
            double.TryParse(sSchDiscount, out SchDiscount);
            txtSchDiscount.Value = sSchDiscount;
            string sAfterSchDiscount = (GrossTotal - SchDiscount).ToString();
            txtAfterSchDiscount.Value = sAfterSchDiscount;

            double AfterSchDiscount = 0;
            double.TryParse(sAfterSchDiscount, out AfterSchDiscount);
            if (GrossTotal > 0)
            {
                string sDiscountPC = txtDiscountPC.Value;
                string sDiscountAmount = txtDiscount.Value;
                double DiscountPC = 0;
                double DiscountAmount = 0;
                double TaxRate = 0;
                double.TryParse(sDiscountPC, out DiscountPC);
                double.TryParse(sDiscountAmount, out DiscountAmount);
                if (ChangeItem == "etDiscPC" && DiscountPC > -0.01 && DiscountPC <100)
                {
                  DiscountAmount =  AfterSchDiscount * (DiscountPC / 100);
                  txtDiscount.Value = DiscountAmount.ToString();
                }
                else if (ChangeItem == "etDiscount" && DiscountAmount > -0.01 && DiscountAmount < AfterSchDiscount)
                {
                  DiscountPC =  (DiscountAmount / AfterSchDiscount) * 100;
                  txtDiscountPC.Value = DiscountPC.ToString();
                }
                string sBeforeTax = (AfterSchDiscount - DiscountAmount).ToString();
                txtBeforeTax.Value = sBeforeTax;
                double BeforeTax = 0;
                double.TryParse(sBeforeTax, out BeforeTax);
                double.TryParse(txtTaxPC.Value, out TaxRate);
                string sTaxAmount = (BeforeTax * (TaxRate / 100)).ToString();
                double TaxAmount = 0;
                txtTaxAmount.Value = sTaxAmount;
                double.TryParse(sTaxAmount, out TaxAmount);
                string DocTotal = (BeforeTax + TaxAmount).ToString();
                txtDocTotal.Value = DocTotal;
            }
        }

        private void txtDiscount_ValidateAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (!pVal.InnerEvent)
            {
                CalculateDiscounts(pVal.ItemUID);
            }
        }
        private bool DiscountBefore(string ChangeItem)
        {
            string sDocTotal = Matrix0.Columns.Item("Total").ColumnSetting.SumValue;
            double DocTotal = 0;
            double.TryParse(sDocTotal, out DocTotal);
            if (DocTotal > 0)
            {
                string sDiscountPC = txtDiscountPC.Value;
                string sDiscountAmount = txtDiscount.Value;
                double DiscountPC = 0;
                double DiscountAmount = 0;

                double.TryParse(sDiscountPC, out DiscountPC);
                double.TryParse(sDiscountAmount, out DiscountAmount);
                if (ChangeItem == "etDiscPC" && (DiscountPC < 0 || DiscountPC > 100))
                {
                    Global.SetMessage("Percentage must be between 0 and 100", BoStatusBarMessageType.smt_Error);
                    return false;
                }
                else if (ChangeItem == "etDiscount" && (DiscountAmount < 0 || DiscountAmount > DocTotal))
                {
                    Global.SetMessage("Discount amount must be between 0 and "+DocTotal.ToString(), BoStatusBarMessageType.smt_Error);
                    return false;
                }
            }

            return true;
        }
        private void txtDiscountPC_ValidateBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = DiscountBefore(pVal.ItemUID);
        }

        private void txtDiscount_ValidateBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = DiscountBefore(pVal.ItemUID);
        }

        private StaticText StaticText19;
        private EditText EditText0;
        private StaticText StaticText20;
        private EditText txtGrossTotal;
        private EditText txtSchDiscountPC;
        private StaticText StaticText21;
        private EditText txtSchDiscount;
        private StaticText StaticText22;
        private EditText txtTaxAmount;
        private EditText txtTaxPC;
        private StaticText StaticText23;
        private EditText txtAfterSchDiscount;
        private StaticText StaticText24;
        private EditText txtBeforeTax;
        private StaticText StaticText25;
        private EditText txtDocTotal;
        private StaticText StaticText26;
        private EditText EditText1;
        private StaticText StaticText27;
        private EditText EditText2;
        private StaticText StaticText28;
        private EditText EditText5;
        private StaticText StaticText31;
        private EditText EditText6;
        private StaticText StaticText32;
        private StaticText StaticText29;
        private EditText EditText3;
    }
}
