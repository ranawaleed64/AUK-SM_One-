using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SM_One
{
    [FormAttribute("SM_One.Registration", "Registration.b1f")]
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
            this.txtAcademic = ((SAPbouiCOM.EditText)(this.GetItem("etAYear").Specific));
            this.txtAcademic.KeyDownBefore += new SAPbouiCOM._IEditTextEvents_KeyDownBeforeEventHandler(this.txtAcademic_KeyDownBefore);
            this.txtAcademic.ClickBefore += new SAPbouiCOM._IEditTextEvents_ClickBeforeEventHandler(this.txtAcademic_ClickBefore);
            this.txtStudentName = ((SAPbouiCOM.EditText)(this.GetItem("etName").Specific));
            this.txtStudentName.KeyDownBefore += new SAPbouiCOM._IEditTextEvents_KeyDownBeforeEventHandler(this.txtStudentName_KeyDownBefore);
            this.txtStudentName.ClickBefore += new SAPbouiCOM._IEditTextEvents_ClickBeforeEventHandler(this.txtStudentName_ClickBefore);
            this.txtFiscal = ((SAPbouiCOM.EditText)(this.GetItem("etFYear").Specific));
            this.txtFiscal.KeyDownBefore += new SAPbouiCOM._IEditTextEvents_KeyDownBeforeEventHandler(this.txtFiscal_KeyDownBefore);
            this.txtFiscal.ClickBefore += new SAPbouiCOM._IEditTextEvents_ClickBeforeEventHandler(this.txtFiscal_ClickBefore);
            this.txtProgram = ((SAPbouiCOM.EditText)(this.GetItem("etPC").Specific));
            this.txtProgram.KeyDownBefore += new SAPbouiCOM._IEditTextEvents_KeyDownBeforeEventHandler(this.txtProgram_KeyDownBefore);
            this.txtProgram.ClickBefore += new SAPbouiCOM._IEditTextEvents_ClickBeforeEventHandler(this.txtProgram_ClickBefore);
            this.txtLevel = ((SAPbouiCOM.EditText)(this.GetItem("etLevel").Specific));
            this.txtLevel.KeyDownBefore += new SAPbouiCOM._IEditTextEvents_KeyDownBeforeEventHandler(this.txtLevel_KeyDownBefore);
            this.txtLevel.ClickBefore += new SAPbouiCOM._IEditTextEvents_ClickBeforeEventHandler(this.txtLevel_ClickBefore);
            this.txtInvNum = ((SAPbouiCOM.EditText)(this.GetItem("etInvNum").Specific));
            this.txtInvNum.KeyDownBefore += new SAPbouiCOM._IEditTextEvents_KeyDownBeforeEventHandler(this.txtInvNum_KeyDownBefore);
            this.txtInvNum.ClickBefore += new SAPbouiCOM._IEditTextEvents_ClickBeforeEventHandler(this.txtInvNum_ClickBefore);
            this.Matrix0 = ((SAPbouiCOM.Matrix)(this.GetItem("Matrix0").Specific));
            this.Matrix0.KeyDownBefore += new SAPbouiCOM._IMatrixEvents_KeyDownBeforeEventHandler(this.Matrix0_KeyDownBefore);
            this.Matrix0.ValidateBefore += new SAPbouiCOM._IMatrixEvents_ValidateBeforeEventHandler(this.Matrix0_ValidateBefore);
            this.Matrix0.ValidateAfter += new SAPbouiCOM._IMatrixEvents_ValidateAfterEventHandler(this.Matrix0_ValidateAfter);
            this.Matrix0.ClickBefore += new SAPbouiCOM._IMatrixEvents_ClickBeforeEventHandler(this.Matrix0_ClickBefore);
            //           this.Matrix0.PressedBefore += new SAPbouiCOM._IMatrixEvents_PressedBeforeEventHandler(this.Matrix0_PressedBefore);
            this.Matrix0.LinkPressedAfter += new SAPbouiCOM._IMatrixEvents_LinkPressedAfterEventHandler(this.Matrix0_LinkPressedAfter);
            this.cmbType = ((SAPbouiCOM.ComboBox)(this.GetItem("cbType").Specific));
            this.cmbType.KeyDownBefore += new SAPbouiCOM._IComboBoxEvents_KeyDownBeforeEventHandler(this.cmbType_KeyDownBefore);
            this.cmbType.ClickBefore += new SAPbouiCOM._IComboBoxEvents_ClickBeforeEventHandler(this.cmbType_ClickBefore);
            this.txtDocEntry = ((SAPbouiCOM.EditText)(this.GetItem("etReg").Specific));
            this.txtDocEntry.KeyDownBefore += new SAPbouiCOM._IEditTextEvents_KeyDownBeforeEventHandler(this.txtDocEntry_KeyDownBefore);
            this.txtDocEntry.ClickBefore += new SAPbouiCOM._IEditTextEvents_ClickBeforeEventHandler(this.txtDocEntry_ClickBefore);
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("stReg").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("stID").Specific));
            this.StaticText3 = ((SAPbouiCOM.StaticText)(this.GetItem("stPC").Specific));
            this.StaticText4 = ((SAPbouiCOM.StaticText)(this.GetItem("stSDate").Specific));
            this.StaticText5 = ((SAPbouiCOM.StaticText)(this.GetItem("stFYear").Specific));
            this.StaticText6 = ((SAPbouiCOM.StaticText)(this.GetItem("stInvEnt").Specific));
            this.StaticText7 = ((SAPbouiCOM.StaticText)(this.GetItem("stSemester").Specific));
            this.StaticText8 = ((SAPbouiCOM.StaticText)(this.GetItem("stName").Specific));
            this.StaticText9 = ((SAPbouiCOM.StaticText)(this.GetItem("stLevel").Specific));
            this.StaticText10 = ((SAPbouiCOM.StaticText)(this.GetItem("stEDate").Specific));
            this.StaticText11 = ((SAPbouiCOM.StaticText)(this.GetItem("stAYear").Specific));
            this.StaticText12 = ((SAPbouiCOM.StaticText)(this.GetItem("stInvNum").Specific));
            this.btnSave = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.txtRemarks = ((SAPbouiCOM.EditText)(this.GetItem("eeRemarks").Specific));
            this.StaticText13 = ((SAPbouiCOM.StaticText)(this.GetItem("stRemarks").Specific));
            this.LinkedButton0 = ((SAPbouiCOM.LinkedButton)(this.GetItem("lbID").Specific));
            this.Matrix0.AutoResizeColumns();
            //                     Global.myApi.MenuEvent += this.MyApi_MenuEvent;
            this.cmbStatus = ((SAPbouiCOM.ComboBox)(this.GetItem("cbStatus").Specific));
            this.cmbStatus.KeyDownBefore += new SAPbouiCOM._IComboBoxEvents_KeyDownBeforeEventHandler(this.cmbStatus_KeyDownBefore);
            this.cmbStatus.ClickBefore += new SAPbouiCOM._IComboBoxEvents_ClickBeforeEventHandler(this.cmbStatus_ClickBefore);
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("stStatus").Specific));
            this.cbInvoices = ((SAPbouiCOM.CheckBox)(this.GetItem("chInvoice").Specific));
            this.cbInvoices.PressedBefore += new SAPbouiCOM._ICheckBoxEvents_PressedBeforeEventHandler(this.cbInvoices_PressedBefore);
            this.cbInvoices.KeyDownBefore += new SAPbouiCOM._ICheckBoxEvents_KeyDownBeforeEventHandler(this.cbInvoices_KeyDownBefore);
            this.cbInvoices.ClickBefore += new SAPbouiCOM._ICheckBoxEvents_ClickBeforeEventHandler(this.cbInvoices_ClickBefore);
            this.LinkedButton1 = ((SAPbouiCOM.LinkedButton)(this.GetItem("lbInvEnt").Specific));
            this.cmbType.ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly;
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("btAddItem").Specific));
            this.Button0.PressedBefore += new SAPbouiCOM._IButtonEvents_PressedBeforeEventHandler(this.Button0_PressedBefore);
            this.Button0.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button0_PressedAfter);
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.RowsDb = this.UIAPIRawForm.DataSources.DBDataSources.Item("@SRG1");
            this.Matrix0.Columns.Item("LineType").Visible = false;
            this.Button2 = ((SAPbouiCOM.Button)(this.GetItem("btRemItem").Specific));
            this.Button2.PressedBefore += new SAPbouiCOM._IButtonEvents_PressedBeforeEventHandler(this.Button2_PressedBefore);
            this.Button2.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button2_PressedAfter);
            this.EditText0 = ((SAPbouiCOM.EditText)(this.GetItem("etDiscount").Specific));
            this.StaticText14 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_1").Specific));
            this.OnCustomInitialize();

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
        private SAPbouiCOM.EditText txtAcademic;
        private SAPbouiCOM.EditText txtStudentName;
        private SAPbouiCOM.EditText txtFiscal;
        private SAPbouiCOM.EditText txtProgram;
        private SAPbouiCOM.EditText txtLevel;
        private SAPbouiCOM.EditText txtInvNum;
        private SAPbouiCOM.Matrix Matrix0;
        private SAPbouiCOM.ComboBox cmbType;
        public SAPbouiCOM.EditText txtDocEntry;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.StaticText StaticText3;
        private SAPbouiCOM.StaticText StaticText4;
        private SAPbouiCOM.StaticText StaticText5;
        private SAPbouiCOM.StaticText StaticText6;
        private SAPbouiCOM.StaticText StaticText7;
        private SAPbouiCOM.StaticText StaticText8;
        private SAPbouiCOM.StaticText StaticText9;
        private SAPbouiCOM.StaticText StaticText10;
        private SAPbouiCOM.StaticText StaticText11;
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

        private void txtProgram_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);


        }

        private void txtProgram_KeyDownBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
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

        private void txtFiscal_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);
        }

        private void txtFiscal_KeyDownBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
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

        private void cmbType_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);


        }

        private void cmbType_KeyDownBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
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

        private void txtLevel_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);


        }

        private void txtLevel_KeyDownBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
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

        private void txtAcademic_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(pVal.CharPressed);


        }

        private void txtAcademic_KeyDownBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
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
             
            if (Config.AllStatusConfig.FirstOrDefault(x=>x.Status.ToLower() == cmbStatus.Selected.Value.ToLower()).Courses == "Y")
            {
                string SubjectCodes = "";
                for (int i = 1; i < Matrix0.RowCount + 1; i++)
                {
                    SubjectCodes += "'" + ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("SubCode", i)).Value + "',";
                }
                SubjectCodes = SubjectCodes.TrimEnd(',');
                SearchCourses searchCourses = new SearchCourses(txtProgram.Value, SubjectCodes, Matrix0, RowsDb, (SAPbouiCOM.Form)UIAPIRawForm, txtStudentID.Value);
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

        private SAPbouiCOM.EditText EditText0;
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
            else if (Col == "DiscountPC" && Discount > 100)
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
                string sPrice = ((EditText)Matrix0.GetCellSpecific("Price", Row)).Value;
                string sDiscount = ((EditText)Matrix0.GetCellSpecific("Discount", Row)).Value;
                string sDiscountPC = ((EditText)Matrix0.GetCellSpecific("DiscountPC", Row)).Value;
                string sTotal = ((EditText)Matrix0.GetCellSpecific("Total", Row)).Value;
                double.TryParse(sPrice, out Price);
                double.TryParse(sDiscount, out Discount);
                double.TryParse(sDiscountPC, out DiscountPC);
                double.TryParse(sTotal, out Total);

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
                Total = Price - Discount;
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
                this.Matrix0.Columns.Item("Discount").Visible = false;
                this.Matrix0.Columns.Item("DiscountPC").Visible = false;
                this.Matrix0.Columns.Item("Price").Visible = false;
            }
            else
            {
                this.Matrix0.Columns.Item("Discount").Visible = true;
                this.Matrix0.Columns.Item("DiscountPC").Visible = true;
                this.Matrix0.Columns.Item("Price").Visible = true;
            }
        }
    }
}
