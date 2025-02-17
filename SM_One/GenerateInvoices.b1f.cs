using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using System.Threading.Tasks;
using System.Globalization;
using SAPbouiCOM;

namespace SM_One
{
    [FormAttribute("SM_One.GenerateInvoices", "SAP/GenerateInvoices.b1f")]
    class GenerateInvoices : UserFormBase
    {
        public GenerateInvoices()
        {

        }
        public GenerateInvoices(string docType)
        {
            DocType = docType;
        }
        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.txtMajor = ((SAPbouiCOM.EditText)(this.GetItem("etMajor").Specific));
            this.txtMajor.ChooseFromListBefore += new SAPbouiCOM._IEditTextEvents_ChooseFromListBeforeEventHandler(this.txtMajor_ChooseFromListBefore);
            this.txtMajor.KeyDownBefore += new SAPbouiCOM._IEditTextEvents_KeyDownBeforeEventHandler(this.txtMajor_KeyDownBefore);
            this.txtMajor.ClickBefore += new SAPbouiCOM._IEditTextEvents_ClickBeforeEventHandler(this.txtMajor_ClickBefore);
            this.CustomIntialize();
            this.OnCustomInitialize();

        }
        private void CustomIntialize()
        {
            this.btnLoad = ((SAPbouiCOM.Button)(this.GetItem("btLoad").Specific));
            this.btnLoad.PressedBefore += new SAPbouiCOM._IButtonEvents_PressedBeforeEventHandler(this.btnLoad_PressedBefore);
            this.btnLoad.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btnLoad_PressedAfter);
            this.btnSelectAll = ((SAPbouiCOM.Button)(this.GetItem("btSelect").Specific));
            this.btnSelectAll.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btnSelectAll_PressedAfter);
            this.btnDeSelect = ((SAPbouiCOM.Button)(this.GetItem("btDSelect").Specific));
            this.btnDeSelect.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btnDeSelect_PressedAfter);
            this.btnSave = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.btnClear = ((SAPbouiCOM.Button)(this.GetItem("btClear").Specific));
            this.btnClear.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btnClear_PressedAfter);
            this.txtFromDate = ((SAPbouiCOM.EditText)(this.GetItem("etFrom").Specific));
            this.txtFromDate.KeyDownBefore += new SAPbouiCOM._IEditTextEvents_KeyDownBeforeEventHandler(this.txtFromDate_KeyDownBefore);
            this.txtFromDate.ClickBefore += new SAPbouiCOM._IEditTextEvents_ClickBeforeEventHandler(this.txtFromDate_ClickBefore);
            this.txtToDate = ((SAPbouiCOM.EditText)(this.GetItem("etTo").Specific));
            this.txtToDate.KeyDownBefore += new SAPbouiCOM._IEditTextEvents_KeyDownBeforeEventHandler(this.txtToDate_KeyDownBefore);
            this.txtToDate.ClickBefore += new SAPbouiCOM._IEditTextEvents_ClickBeforeEventHandler(this.txtToDate_ClickBefore);
            this.Matrix0 = ((SAPbouiCOM.Matrix)(this.GetItem("Matrix0").Specific));
            this.Matrix0.ValidateAfter += new SAPbouiCOM._IMatrixEvents_ValidateAfterEventHandler(this.Matrix0_ValidateAfter);
            //this.Matrix0.PressedBefore += new SAPbouiCOM._IMatrixEvents_PressedBeforeEventHandler(this.Matrix0_PressedBefore);
            //this.Matrix0.PressedAfter += new SAPbouiCOM._IMatrixEvents_PressedAfterEventHandler(this.Matrix0_PressedAfter);
            this.Matrix0.ValidateBefore += new SAPbouiCOM._IMatrixEvents_ValidateBeforeEventHandler(this.Matrix0_ValidateBefore);
            this.Matrix0.ComboSelectAfter += new SAPbouiCOM._IMatrixEvents_ComboSelectAfterEventHandler(this.Matrix0_ComboSelectAfter);
            this.Matrix0.ChooseFromListAfter += new SAPbouiCOM._IMatrixEvents_ChooseFromListAfterEventHandler(this.Matrix0_ChooseFromListAfter);
            this.Matrix0.ChooseFromListBefore += new SAPbouiCOM._IMatrixEvents_ChooseFromListBeforeEventHandler(this.Matrix0_ChooseFromListBefore);
            this.Matrix0.KeyDownBefore += new SAPbouiCOM._IMatrixEvents_KeyDownBeforeEventHandler(this.Matrix0_KeyDownBefore);
            this.Matrix0.ClickBefore += new SAPbouiCOM._IMatrixEvents_ClickBeforeEventHandler(this.Matrix0_ClickBefore);
            this.Matrix0.LinkPressedBefore += new SAPbouiCOM._IMatrixEvents_LinkPressedBeforeEventHandler(this.Matrix0_LinkPressedBefore);
            this.Matrix0.LinkPressedAfter += new SAPbouiCOM._IMatrixEvents_LinkPressedAfterEventHandler(this.Matrix0_LinkPressedAfter);
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("stFrom").Specific));
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("stTo").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("stProg").Specific));
            this.txtDocEntry = ((SAPbouiCOM.EditText)(this.GetItem("etDocNum").Specific));
            this.txtDocEntry.KeyDownBefore += new SAPbouiCOM._IEditTextEvents_KeyDownBeforeEventHandler(this.txtDocEntry_KeyDownBefore);
            this.txtDocEntry.ClickBefore += new SAPbouiCOM._IEditTextEvents_ClickBeforeEventHandler(this.txtDocEntry_ClickBefore);
            this.StaticText3 = ((SAPbouiCOM.StaticText)(this.GetItem("stDocNum").Specific));
            this.txtDocDate = ((SAPbouiCOM.EditText)(this.GetItem("etDocDate").Specific));
            this.txtDocDate.ValidateBefore += new SAPbouiCOM._IEditTextEvents_ValidateBeforeEventHandler(this.txtDocDate_ValidateBefore);
            this.txtDocDate.KeyDownBefore += new SAPbouiCOM._IEditTextEvents_KeyDownBeforeEventHandler(this.txtDocDate_KeyDownBefore);
            this.txtDocDate.ClickBefore += new SAPbouiCOM._IEditTextEvents_ClickBeforeEventHandler(this.txtDocDate_ClickBefore);
            this.StaticText4 = ((SAPbouiCOM.StaticText)(this.GetItem("stDocDate").Specific));
            this.txtCreateDate = ((SAPbouiCOM.EditText)(this.GetItem("etGenDate").Specific));
            this.txtCreateDate.KeyDownBefore += new SAPbouiCOM._IEditTextEvents_KeyDownBeforeEventHandler(this.txtCreateDate_KeyDownBefore);
            this.txtCreateDate.ClickBefore += new SAPbouiCOM._IEditTextEvents_ClickBeforeEventHandler(this.txtCreateDate_ClickBefore);
            this.StaticText5 = ((SAPbouiCOM.StaticText)(this.GetItem("stGenDate").Specific));
            this.btnCancel = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.AING1 = this.UIAPIRawForm.DataSources.DBDataSources.Add("@TING1");
            this.ING1 = this.UIAPIRawForm.DataSources.DBDataSources.Item("@ING1");
            this.Matrix0.Columns.Item("InvEntry").Visible = false;
            this.Matrix0.Columns.Item("MemoEntry").Visible = false;
            this.Matrix0.Columns.Item("BaseLine").Visible = false;
            this.Matrix0.Columns.Item("CancelEnt").Visible = false;
            this.Matrix0.Columns.Item("TaxRate").Visible = false;
            this.Matrix0.Columns.Item("Freight1").Visible = false;
            this.Matrix0.Columns.Item("Freight2").Visible = false;
            this.Matrix0.Columns.Item("Freight3").Visible = false;
            this.Matrix0.Columns.Item("Freight4").Visible = false;
            this.Matrix0.Columns.Item("Freight5").Visible = false;
            this.Matrix0.Columns.Item("Freight6").Visible = false;
            this.Matrix0.Columns.Item("Freight7").Visible = false;
            this.Matrix0.Columns.Item("Freight8").Visible = false;
            this.Matrix0.Columns.Item("Freight9").Visible = false;
            this.Matrix0.Columns.Item("Freight10").Visible = false;
            //this.Matrix0.Columns.Item("RegAmount").Visible = false;

            for (int i = 0; i < Config.FreightCodes.Count; i++)
            {
                if (Config.FreightCodes[i].FreightEnabled == "Y" && Config.FreightCodes[i].ScholarshipDiscount !="Y")
                {
                    this.Matrix0.Columns.Item("Freight" + Config.FreightCodes[i].Line).Visible = true;
                    this.Matrix0.Columns.Item("Freight" + Config.FreightCodes[i].Line).TitleObject.Caption = Config.FreightCodes[i].FreightName;
                }
            }
            this.EditText0 = ((SAPbouiCOM.EditText)(this.GetItem("etRemark").Specific));
            this.StaticText6 = ((SAPbouiCOM.StaticText)(this.GetItem("stRemark").Specific));
            this.StaticText7 = ((SAPbouiCOM.StaticText)(this.GetItem("stSchool").Specific));
            this.StaticText9 = ((SAPbouiCOM.StaticText)(this.GetItem("stStatus").Specific));
            this.cmbSchool = ((SAPbouiCOM.ComboBox)(this.GetItem("cbSchool").Specific));
            this.cmbSchool.KeyDownBefore += new SAPbouiCOM._IComboBoxEvents_KeyDownBeforeEventHandler(this.cmbSchool_KeyDownBefore);
            this.cmbSchool.ClickBefore += new SAPbouiCOM._IComboBoxEvents_ClickBeforeEventHandler(this.cmbSchool_ClickBefore);
            this.cmbStatus = ((SAPbouiCOM.ComboBox)(this.GetItem("cbStatus").Specific));
            this.cmbStatus.KeyDownBefore += new SAPbouiCOM._IComboBoxEvents_KeyDownBeforeEventHandler(this.cmbStatus_KeyDownBefore);
            this.cmbStatus.ClickBefore += new SAPbouiCOM._IComboBoxEvents_ClickBeforeEventHandler(this.cmbStatus_ClickBefore);
            this.cmbSemester = ((SAPbouiCOM.ComboBox)(this.GetItem("cbSemester").Specific));
            this.cmbSemester.KeyDownBefore += new SAPbouiCOM._IComboBoxEvents_KeyDownBeforeEventHandler(this.cmbSemester_KeyDownBefore);
            this.cmbSemester.ClickBefore += new SAPbouiCOM._IComboBoxEvents_ClickBeforeEventHandler(this.cmbSemester_ClickBefore);
            this.StaticText8 = ((SAPbouiCOM.StaticText)(this.GetItem("stDocType").Specific));
            this.cmbDocType = ((SAPbouiCOM.ComboBox)(this.GetItem("cbDocType").Specific));
            this.StaticText10 = ((SAPbouiCOM.StaticText)(this.GetItem("stCancel").Specific));
            this.cmbCancelType = ((SAPbouiCOM.ComboBox)(this.GetItem("cbCancel").Specific));
            this.cmbCancelType.KeyDownBefore += new SAPbouiCOM._IComboBoxEvents_KeyDownBeforeEventHandler(this.cmbCancelType_KeyDownBefore);
            this.cmbCancelType.ClickBefore += new SAPbouiCOM._IComboBoxEvents_ClickBeforeEventHandler(this.cmbCancelType_ClickBefore);

            //                    this.ConfigureCombos();
            this.Matrix0.AutoResizeColumns();
            this.txtDueDate = ((SAPbouiCOM.EditText)(this.GetItem("etDueDate").Specific));
            this.txtDueDate.KeyDownBefore += new SAPbouiCOM._IEditTextEvents_KeyDownBeforeEventHandler(this.txtDueDate_KeyDownBefore);
            this.txtDueDate.ClickBefore += new SAPbouiCOM._IEditTextEvents_ClickBeforeEventHandler(this.txtDueDate_ClickBefore);
            this.stDueDate = ((SAPbouiCOM.StaticText)(this.GetItem("stDueDate").Specific));
            this.txtStudent = ((SAPbouiCOM.EditText)(this.GetItem("etStud").Specific));
            this.txtStudent.KeyDownBefore += new SAPbouiCOM._IEditTextEvents_KeyDownBeforeEventHandler(this.txtStudent_KeyDownBefore);
            this.txtStudent.ClickBefore += new SAPbouiCOM._IEditTextEvents_ClickBeforeEventHandler(this.txtStudent_ClickBefore);
            this.stStudent = ((SAPbouiCOM.StaticText)(this.GetItem("stStud").Specific));
            RowsDb = UIAPIRawForm.DataSources.DBDataSources.Item("@ING1");
            this.txtDocDate.ValidateBefore += new SAPbouiCOM._IEditTextEvents_ValidateBeforeEventHandler(this.txtDocDate_ValidateBefore);
            this.OnCustomInitialize();
            SM_One.Global.myApi.MenuEvent += this.MyApi_MenuEvent;
            //Matrix0.Columns.Item("Price").ColumnSetting.SumType = SAPbouiCOM.BoColumnSumType.bst_Auto;
            //Matrix0.Columns.Item("AppPrice").ColumnSetting.SumType = SAPbouiCOM.BoColumnSumType.bst_Auto;
            //Matrix0.Columns.Item("TaxAmount").ColumnSetting.SumType = SAPbouiCOM.BoColumnSumType.bst_Auto;
            //Matrix0.Columns.Item("MAmount").ColumnSetting.SumType = SAPbouiCOM.BoColumnSumType.bst_Auto;
            Global.FillCombo(cmbSchool, (Form)UIAPIRawForm, "Select \"Code\", \"U_Description\" as \"Name\" from \"@OCOL\"", "");
            Global.FillCombo(((SAPbouiCOM.ComboBox)Matrix0.GetCellSpecific("Scholar", 0)), (Form)UIAPIRawForm, "Select \"Code\", \"Name\" from \"@OSHL\"", "");
            Global.FillCombo(cmbSemester, (Form)UIAPIRawForm, "Select \"Code\", \"Name\" from \"@OSEM\"", "");
            Matrix0.Columns.Item("DocTotal").ColumnSetting.SumType = SAPbouiCOM.BoColumnSumType.bst_Auto;
            Matrix0.Columns.Item("TaxAmount").ColumnSetting.SumType = SAPbouiCOM.BoColumnSumType.bst_Auto;
            Matrix0.Columns.Item("TFreight").ColumnSetting.SumType = SAPbouiCOM.BoColumnSumType.bst_Auto;
            Matrix0.Columns.Item("AfterDisc").ColumnSetting.SumType = SAPbouiCOM.BoColumnSumType.bst_Auto;
            Matrix0.Columns.Item("AftSchDisc").ColumnSetting.SumType = SAPbouiCOM.BoColumnSumType.bst_Auto;
            Matrix0.Columns.Item("RegAmount").ColumnSetting.SumType = SAPbouiCOM.BoColumnSumType.bst_Auto;
            Matrix0.Columns.Item("SchDiscAmt").ColumnSetting.SumType = SAPbouiCOM.BoColumnSumType.bst_Auto;

        }
        List<MonthlyAmounts> mymonths = new List<MonthlyAmounts>();
        private List<List<string>> DocEntries = new List<List<string>>();
        private List<List<string>> ErrorList = new List<List<string>>();

        DateTime FromDate = DateTime.Now;
        DateTime ToDate = DateTime.Now;
        public string DocType;
        //GeneralService oGeneralService;
        //GeneralData oHeader;
        //CompanyService oCmpSrv;
        //GeneralData oRowItem;
        //GeneralDataCollection oRows;
        //GeneralDataParams oGeneralParams;
        bool SecondIteration = false;
        SAPbouiCOM.DBDataSource RowsDb;
        private void MyApi_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if ((pVal.MenuUID == "1281" || pVal.MenuUID == "1282") && !pVal.BeforeAction && Global.myApi.Forms.ActiveForm.UniqueID == UIAPIRawForm.UniqueID)
            {
                cmbDocType.Select(DocType, SAPbouiCOM.BoSearchKey.psk_ByValue);
                if (DocType == "C")
                {
                    Matrix0.Columns.Item("Cancel").Editable = true;
                    Matrix0.Columns.Item("MAccount").Editable = true;
                    Matrix0.Columns.Item("MAmount").Editable = true;
                }
                else
                {
                    Matrix0.Columns.Item("MAccount").Editable = false;
                    Matrix0.Columns.Item("MAmount").Editable = false;
                    Matrix0.Columns.Item("Cancel").Editable = false;
                }
            }
        }
        private void ConfigureCombos()
        {
            this.cmbSchool.ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly;
            this.cmbStatus.ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly;
            this.cmbSemester.ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly;
            this.cmbDocType.ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly;
            this.cmbCancelType.ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly;
            this.cmbSchool.Select(" ", BoSearchKey.psk_ByValue);
            this.cmbStatus.Select("Active", BoSearchKey.psk_ByValue);
            this.cmbSemester.Select(" ", BoSearchKey.psk_ByValue);
            this.cmbCancelType.Select("-", BoSearchKey.psk_ByValue);
            this.cmbDocType.Select(DocType, SAPbouiCOM.BoSearchKey.psk_ByValue);
        }
        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.DataAddAfter += new SAPbouiCOM.Framework.FormBase.DataAddAfterHandler(this.Form_DataAddAfter);
            this.DataAddBefore += new SAPbouiCOM.Framework.FormBase.DataAddBeforeHandler(this.Form_DataAddBefore);
            this.DataLoadAfter += new SAPbouiCOM.Framework.FormBase.DataLoadAfterHandler(this.Form_DataLoadAfter);
            this.DataUpdateAfter += new SAPbouiCOM.Framework.FormBase.DataUpdateAfterHandler(this.Form_DataUpdateAfter);
            this.DataUpdateBefore += new SAPbouiCOM.Framework.FormBase.DataUpdateBeforeHandler(this.Form_DataUpdateBefore);
            this.CloseAfter += new CloseAfterHandler(this.Form_CloseAfter);
        }

        private SAPbouiCOM.Button btnLoad;

        private void OnCustomInitialize()
        {

        }
        private SAPbouiCOM.Button btnSelectAll;
        private SAPbouiCOM.Button btnDeSelect;
        private SAPbouiCOM.Button btnSave;
        private SAPbouiCOM.Button btnClear;
        private SAPbouiCOM.EditText txtFromDate;
        private SAPbouiCOM.EditText txtToDate;
        private SAPbouiCOM.Matrix Matrix0;
        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.EditText txtMajor;
        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.EditText txtDocEntry;
        private SAPbouiCOM.StaticText StaticText3;
        private SAPbouiCOM.EditText txtDocDate;
        private SAPbouiCOM.StaticText StaticText4;
        private SAPbouiCOM.EditText txtCreateDate;
        private SAPbouiCOM.StaticText StaticText5;
        private SAPbouiCOM.Button btnCancel;
        private SAPbouiCOM.DBDataSource AING1;
        private SAPbouiCOM.DBDataSource ING1;
        private SAPbouiCOM.EditText EditText0;
        private SAPbouiCOM.StaticText StaticText6;
        private SAPbouiCOM.StaticText StaticText7;
        private SAPbouiCOM.ComboBox cmbSchool;
        private SAPbouiCOM.StaticText StaticText9;
        private SAPbouiCOM.ComboBox cmbStatus;
        private SAPbouiCOM.StaticText StaticText8;
        public SAPbouiCOM.ComboBox cmbDocType;
        private SAPbouiCOM.StaticText StaticText10;
        public SAPbouiCOM.ComboBox cmbCancelType;
        private SAPbouiCOM.ComboBox cmbSemester;

        private string FormulatedQueryForRegisteration(string Select, string ExtraFilters)
        {
            string Query = "";

            if (Global.Comp_DI.DbServerType == BoDataServerTypes.dst_HANADB)
            {
                Query = @"Insert into ""@TING1"" (""LineId"",""U_Select"",""DocEntry"",""U_StudentCode"",""U_StudentName"",""U_Status"",""U_Major"",""U_RegNo""
,""U_Remarks"",""U_FiscalYear"",""U_AcademicYear"",""U_College"",""U_TaxCode"",""U_StartDate"",""U_EndDate"",""U_HDiscount"",""U_LDiscount"",""U_TaxRate"",""U_TotalBefTax"",""U_HasRepeat"",""U_Scholarship"",""U_DiscountGL"",""U_DiscountPC"",""U_SchDiscountPC"",""U_SchDiscount"",""U_DocTotal"",""U_AfterDiscount"",""U_RegAmount"",""U_AfterSchDisc"",""U_TaxAmount"") 
Select ROW_NUMBER() OVER(Order by ""DocEntry"") as ""S.No"",*,((coalesce(""TotalBefTax"",0)-coalesce(""SchDiscount"",0))-coalesce(""HDiscount"",0))+(((coalesce(""TotalBefTax"", 0) - coalesce(""SchDiscount"", 0)) - coalesce(""HDiscount"", 0)) * (""Rate"" / 100)) as ""DocTotal"",((coalesce(""TotalBefTax"",0)-coalesce(""SchDiscount"",0))-coalesce(""HDiscount"",0)) as ""AfterDiscount"",coalesce(""TotalBefTax"",0) as ""RegAmount"",coalesce(""TotalBefTax"",0)-coalesce(""SchDiscount"",0) as ""AfterSchDisc"",(((coalesce(""TotalBefTax"", 0) - coalesce(""SchDiscount"", 0)) - coalesce(""HDiscount"", 0)) * (""Rate"" / 100)) as ""TaxAmount"" from 
(Select '" + Select + @"' as ""Select"", T0.""DocEntry"" as ""RegNo"",T0.""U_StudentCode"",""U_StudentName"",
T0.""U_RegStatus"",T0.""U_Major"",T0.""DocEntry"",T0.""Remark"",T0.""U_FiscalYear"",T0.""U_AcademicYear"",T0.""U_College"",T1.""ECVatGroup"",
T0.""U_StartDate"",T0.""U_EndDate"",
T0.""U_Discount"" as ""HDiscount"",
(Select sum(T10.""U_Discount"") from ""@SRG1"" T10 where T10.""DocEntry"" = T0.""DocEntry"" and T10.""U_Repeat"" = 'N') as ""LDiscount"",
T5.""Rate"",
(Select sum(T10.""U_Total"") from ""@SRG1"" T10 where T10.""DocEntry"" = T0.""DocEntry"" and T10.""U_Repeat"" = 'N') as ""TotalBefTax"",
'N' as ""IsRepeat"",T0.""U_Scholarship"",
T9.""U_DiscountGL"",
T0.""U_DiscountPC"",
T9.""U_DiscountPC"" as ""SchDiscountPC"",
(Select sum(T10.""U_Total"")*(T9.""U_DiscountPC""/100) from ""@SRG1"" T10 where T10.""DocEntry"" = T0.""DocEntry"" and T10.""U_Repeat"" = 'N') as ""SchDiscount""
from ""@OSRG"" T0
inner join OCRD T1 on T0.""U_StudentCode"" = T1.""CardCode""
inner join OPLN T2 on T2.""ListNum"" = T1.""ListNum""
inner join ""@OSEM"" T6 on T6.""Code"" = T0.""U_Semester""
left join OVTG T5 on T5.""Code"" = T1.""ECVatGroup""
left join ""@ONF1"" T8 on T8.""U_Status"" = T0.""U_RegStatus""
left join ""@ORMP"" T9 on T9.""U_College"" = T0.""U_College"" and T9.""U_Scholarship"" = T0.""U_Scholarship"" and T9.""U_MapType"" = 'R'
where T0.""U_Status"" = 'O' and T0.""U_Invoiced"" ='N' and T0.""U_FeeCreated""= 'N' and TO_DATE(T0.""U_StartDate"") between TO_DATE('" + FromDate.ToString(Global._dateFormat) + "','" + Global._dateFormat.ToUpper() + "') and TO_DATE('" + ToDate.ToString(Global._dateFormat) + "','" + Global._dateFormat.ToUpper() + @"') and TO_DATE(T0.""U_EndDate"") <= TO_DATE('" + ToDate.ToString(Global._dateFormat) + "','" + Global._dateFormat.ToUpper() + @"')" + ExtraFilters + @" 

UNION ALL 

Select '" + Select + @"' as ""Select"", T0.""DocEntry"" as ""RegNo"",T0.""U_StudentCode"",""U_StudentName"",
T0.""U_RegStatus"",T0.""U_Major"",T0.""DocEntry"",T0.""Remark"",T0.""U_FiscalYear"",T0.""U_AcademicYear"",T0.""U_College"",T1.""ECVatGroup"",
T0.""U_StartDate"",T0.""U_EndDate"",T0.""U_Discount"" as ""HDiscount"",(Select sum(T10.""U_Discount"") from ""@SRG1"" T10 where T10.""DocEntry"" = T0.""DocEntry""  and T10.""U_Repeat"" = 'Y') as ""LDiscount"",T5.""Rate"",(Select sum(T10.""U_Total"") from ""@SRG1"" T10 where T10.""DocEntry"" = T0.""DocEntry"" and T10.""U_Repeat"" = 'Y') as ""TotalBefTax"",'Y' as ""IsRepeat"",T0.""U_Scholarship"",'' as ""U_DiscountGL"",T0.""U_DiscountPC"",0 as ""SchDiscountPC"",0 as ""SchDiscount""
from ""@OSRG"" T0
inner join OCRD T1 on T0.""U_StudentCode"" = T1.""CardCode""
inner join OPLN T2 on T2.""ListNum"" = T1.""ListNum""
inner join ""@OSEM"" T6 on T6.""Code"" = T0.""U_Semester""
left join OVTG T5 on T5.""Code"" = T1.""ECVatGroup""
left join ""@ONF1"" T8 on T8.""U_Status"" = T0.""U_RegStatus""
left join ""@ORMP"" T9 on T9.""U_College"" = T0.""U_College"" and T9.""U_Scholarship"" = T0.""U_Scholarship"" and T9.""U_MapType"" = 'R'
where T0.""U_Status"" = 'O' and T0.""U_RInvoiced"" = 'N' and T0.""U_HasRepeat"" = 'Y' and TO_DATE(T0.""U_StartDate"") between TO_DATE('" + FromDate.ToString(Global._dateFormat) + "','" + Global._dateFormat.ToUpper() + "') and TO_DATE('" + ToDate.ToString(Global._dateFormat) + "','" + Global._dateFormat.ToUpper() + @"') and TO_DATE(T0.""U_EndDate"") <= TO_DATE('" + ToDate.ToString(Global._dateFormat) + "', '" + Global._dateFormat.ToUpper() + @"')" + ExtraFilters + " ) a ";
            }
            else
            {

                Query = @"Insert into ""@TING1"" (""LineId"",""U_Select"",""DocEntry"",""U_StudentCode"",""U_StudentName"",""U_Status"",""U_Major"",""U_RegNo""
,""U_Remarks"",""U_FiscalYear"",""U_AcademicYear"",""U_College"",""U_TaxCode"",""U_StartDate"",""U_EndDate"",""U_HDiscount"",""U_LDiscount"",""U_TaxRate"",""U_TotalBefTax"",""U_HasRepeat"",""U_Scholarship"",""U_DiscountGL"",""U_DiscountPC"",""U_SchDiscountPC"",""U_SchDiscount"",""U_DocTotal"",""U_AfterDiscount"",""U_RegAmount"",""U_AfterSchDisc"",""U_TaxAmount"") 
Select ROW_NUMBER() OVER(Order by ""DocEntry"") as ""S.No"",*,((coalesce(""TotalBefTax"",0)-coalesce(""SchDiscount"",0))-coalesce(""HDiscount"",0))+(((coalesce(""TotalBefTax"", 0) - coalesce(""SchDiscount"", 0)) - coalesce(""HDiscount"", 0)) * (""Rate"" / 100)) as ""DocTotal"",((coalesce(""TotalBefTax"",0)-coalesce(""SchDiscount"",0))-coalesce(""HDiscount"",0)) as ""AfterDiscount"",coalesce(""TotalBefTax"",0) as ""RegAmount"",coalesce(""TotalBefTax"",0)-coalesce(""SchDiscount"",0) as ""AfterSchDisc"",(((coalesce(""TotalBefTax"", 0) - coalesce(""SchDiscount"", 0)) - coalesce(""HDiscount"", 0)) * (""Rate"" / 100)) as ""TaxAmount"" from 
(Select '" + Select + @"' as ""Select"", T0.""DocEntry"" as ""RegNo"",T0.""U_StudentCode"",""U_StudentName"",
T0.""U_RegStatus"",T0.""U_Major"",T0.""DocEntry"",T0.""Remark"",T0.""U_FiscalYear"",T0.""U_AcademicYear"",T0.""U_College"",T1.""ECVatGroup"",
T0.""U_StartDate"",T0.""U_EndDate"",T0.""U_Discount"" as ""HDiscount"",(Select sum(T10.""U_Discount"") from ""@SRG1"" T10 where T10.""DocEntry"" = T0.""DocEntry"" and T10.""U_Repeat"" = 'N') as ""LDiscount"",T5.""Rate"",(Select sum(T10.""U_Total"") from ""@SRG1"" T10 where T10.""DocEntry"" = T0.""DocEntry"" and T10.""U_Repeat"" = 'N') as ""TotalBefTax"",'N' as ""IsRepeat"",T0.""U_Scholarship"",T9.""U_DiscountGL"",T0.""U_DiscountPC"",T9.""U_DiscountPC"" as ""SchDiscountPC"",(Select sum(T10.""U_Total"")*(T9.""U_DiscountPC""/100) from ""@SRG1"" T10 where T10.""DocEntry"" = T0.""DocEntry"" and T10.""U_Repeat"" = 'N') as ""SchDiscount""
from ""@OSRG"" T0
inner join OCRD T1 on T0.""U_StudentCode"" = T1.""CardCode""
inner join OPLN T2 on T2.""ListNum"" = T1.""ListNum""
inner join ""@OSEM"" T6 on T6.""Code"" = T0.""U_Semester""
left join OVTG T5 on T5.""Code"" = T1.""ECVatGroup""
left join ""@ONF1"" T8 on T8.""U_Status"" = T0.""U_RegStatus""
left join ""@ORMP"" T9 on T9.""U_College"" = T0.""U_College"" and T9.""U_Scholarship"" = T0.""U_Scholarship"" and T9.""U_MapType"" = 'R'
where T0.""U_Status"" = 'O' and T0.""U_Invoiced"" ='N' and T0.""U_FeeCreated""= 'N' and T0.""U_StartDate"" between CONVERT(datetime,'" + FromDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) + "',103) and CONVERT(datetime,'" + ToDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) + @"',103) and T0.""U_EndDate"" <= CONVERT(datetime,'" + ToDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) + @"',103)" + ExtraFilters + @"

UNION ALL

Select '" + Select + @"' as ""Select"", T0.""DocEntry"" as ""RegNo"",T0.""U_StudentCode"",""U_StudentName"",
T0.""U_RegStatus"",T0.""U_Major"",T0.""DocEntry"",T0.""Remark"",T0.""U_FiscalYear"",T0.""U_AcademicYear"",T0.""U_College"",T1.""ECVatGroup"",
T0.""U_StartDate"",T0.""U_EndDate"",T0.""U_Discount"" as ""HDiscount"",(Select sum(T10.""U_Discount"") from ""@SRG1"" T10 where T10.""DocEntry"" = T0.""DocEntry""  and T10.""U_Repeat"" = 'Y') as ""LDiscount"",T5.""Rate"",(Select sum(T10.""U_Total"") from ""@SRG1"" T10 where T10.""DocEntry"" = T0.""DocEntry"" and T10.""U_Repeat"" = 'Y') as ""TotalBefTax"",'Y' as ""IsRepeat"",T0.""U_Scholarship"",'' as ""U_DiscountGL"",T0.""U_DiscountPC"",0 as ""SchDiscountPC"",0 as ""SchDiscount""
from ""@OSRG"" T0
inner join OCRD T1 on T0.""U_StudentCode"" = T1.""CardCode""
inner join OPLN T2 on T2.""ListNum"" = T1.""ListNum""
inner join ""@OSEM"" T6 on T6.""Code"" = T0.""U_Semester""
left join OVTG T5 on T5.""Code"" = T1.""ECVatGroup""
left join ""@ONF1"" T8 on T8.""U_Status"" = T0.""U_RegStatus""
left join ""@ORMP"" T9 on T9.""U_College"" = T0.""U_College"" and T9.""U_Scholarship"" = T0.""U_Scholarship"" and T9.""U_MapType"" = 'R'
where T0.""U_Status"" = 'O' and T0.""U_RInvoiced"" = 'N'  and T0.""U_HasRepeat"" = 'Y' and T0.""U_StartDate"" between CONVERT(datetime,'" + FromDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) + "',103) and CONVERT(datetime,'" + ToDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) + @"',103) and T0.""U_EndDate"" <= CONVERT(datetime, '" + ToDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) + @"', 103)" + ExtraFilters + ") a";
            }
            return Query;
        }

        private string FormulatedQueryCancellation(string Select, string ExtraFilters)
        {
            string Query = "";
            if (Global.Comp_DI.DbServerType == BoDataServerTypes.dst_HANADB)
            {
                Query = @"Insert into ""@TING1"" (""U_Select"",""DocEntry"",""LineId"",""U_StudentCode"",""U_StudentName"",""U_Major"",""U_Scholarship"",""U_Status"",""U_RegNo"",""U_Remarks"",""U_FiscalYear"",""U_AcademicYear"",""U_College"",""U_TaxCode"",""U_TaxAmount"",""U_InvEntry"",""U_InvNum"",""U_CancelType"",""U_BaseLine"",""U_Invoiced"",""U_BaseDoc"",""U_StartDate"",""U_EndDate"",""U_HDiscount"",""U_LDiscount"",""U_TaxRate"",""U_DocTotal"",""U_ManualAmount"",""U_AfterDiscount"",""U_Freight1"",""U_Freight2"",""U_Freight3"",""U_Freight4"",""U_Freight5"",""U_Freight6"",""U_Freight7"",""U_Freight8"",""U_Freight9"",""U_Freight10"",""U_TotalFreight"",""U_HasRepeat"")
Select '" + Select + @"' as ""Select"",T1.""DocEntry"",ROW_NUMBER() OVER(Order by T1.""DocEntry""),T1.""U_StudentCode"",T1.""U_StudentName"",T1.""U_Major"",T1.""U_Scholarship"",T1.""U_Status"",T1.""U_RegNo"",T1.""U_Remarks"",T1.""U_FiscalYear"",T1.""U_AcademicYear"",T1.""U_College"",T1.""U_TaxCode"",T1.""U_TaxAmount"",T1.""U_InvEntry"",T1.""U_InvNum"",'" + cmbCancelType.Selected.Value.ToString() + @"',T1.""LineId"",T1.""U_Invoiced"",T1.""DocEntry"",T1.""U_StartDate"",T1.""U_EndDate"",T1.""U_HDiscount"",T1.""U_LDiscount"",T1.""U_TaxRate"",T1.""U_DocTotal"" ,T1.""U_ManualAmount"",T1.""U_AfterDiscount"",T1.""U_Freight1"",T1.""U_Freight2"",T1.""U_Freight3"",T1.""U_Freight4"",T1.""U_Freight5"",T1.""U_Freight6"",T1.""U_Freight7"",T1.""U_Freight8"",T1.""U_Freight9"",T1.""U_Freight10"",T1.""U_TotalFreight"",T1.""U_HasRepeat"" from ""@ING1"" T1  
inner join OINV T2 on T2.""DocEntry"" = T1.""U_InvEntry"" inner join ""@OING"" T3 on T1.""DocEntry"" = T3.""DocEntry""  where (T2.""CANCELED"" = 'N' and T2.""CEECFlag"" = 'N') and T3.""U_DocType"" = 'R' and T1.""U_Invoiced"" = 'Y' and coalesce(T1.""U_Cancelled"",'N') = 'N' and TO_DATE(T2.""DocDate"") between TO_DATE('" + FromDate.ToString(Global._dateFormat) + "','" + Global._dateFormat.ToUpper() + "') and TO_DATE('" + ToDate.ToString(Global._dateFormat) + "','" + Global._dateFormat.ToUpper() + "')" + ExtraFilters;
            }
            else
            {
                Query = @"Insert into ""@TING1"" (""U_Select"",""DocEntry"",""LineId"",""U_StudentCode"",""U_StudentName"",""U_Major"",""U_Scholarship"",""U_Status"",""U_RegNo"",""U_Remarks"",""U_FiscalYear"",""U_AcademicYear"",""U_College"",""U_TaxCode"",""U_TaxAmount"",""U_InvEntry"",""U_InvNum"",""U_CancelType"",""U_BaseLine"",""U_Invoiced"",""U_BaseDoc"",""U_StartDate"",""U_EndDate"",""U_HDiscount"",""U_LDiscount"",""U_TaxRate"",""U_DocTotal"",""U_ManualAmount"",""U_AfterDiscount"",""U_Freight1"",""U_Freight2"",""U_Freight3"",""U_Freight4"",""U_Freight5"",""U_Freight6"",""U_Freight7"",""U_Freight8"",""U_Freight9"",""U_Freight10"",""U_TotalFreight"",""U_HasRepeat"")
Select '" + Select + @"' as ""Select"",T1.""DocEntry"",ROW_NUMBER() OVER(Order by T1.""DocEntry""),T1.""U_StudentCode"",T1.""U_StudentName"",T1.""U_Major"",T1.""U_Scholarship"",T1.""U_Status"",T1.""U_RegNo"",T1.""U_Remarks"",T1.""U_FiscalYear"",T1.""U_AcademicYear"",T1.""U_College"",T1.""U_TaxCode"",T1.""U_TaxAmount"",T1.""U_InvEntry"",T1.""U_InvNum"",'" + cmbCancelType.Selected.Value.ToString() + @"',T1.""LineId"",T1.""U_Invoiced"",T1.""DocEntry"",T1.""U_StartDate"",T1.""U_EndDate"",T1.""U_HDiscount"",T1.""U_LDiscount"",T1.""U_TaxRate"",T1.""U_DocTotal"" ,T1.""U_ManualAmount"",T1.""U_AfterDiscount"",T1.""U_Freight1"",T1.""U_Freight2"",T1.""U_Freight3"",T1.""U_Freight4"",T1.""U_Freight5"",T1.""U_Freight6"",T1.""U_Freight7"",T1.""U_Freight8"",T1.""U_Freight9"",T1.""U_Freight10"",T1.""U_TotalFreight"",T1.""U_HasRepeat"" from ""@ING1"" T1  
inner join OINV T2 on T2.""DocEntry"" = T1.""U_InvEntry""  inner join ""@OING"" T3 on T1.""DocEntry"" = T3.""DocEntry""  where (T2.""CANCELED"" = 'N'  and T2.""CEECFlag"" = 'N') and T3.""U_DocType"" = 'R' and T1.""U_Invoiced"" = 'Y' and coalesce(T1.""U_Cancelled"",'N') = 'N' and T2.""DocDate"" between CONVERT(datetime,'" + FromDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) + "',103) and CONVERT(datetime,'" + ToDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) + "',103)" + ExtraFilters;
            }
            return Query;
        }
        private void FillMatrixForRegisteration(string Select)
        {

            UIAPIRawForm.Freeze(true);
            Recordset oRec = Global.Comp_DI.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
            string ExtraFilters = "";
            oRec.DoQuery("Delete from \"@TING1\"");
            if (!string.IsNullOrWhiteSpace(cmbSchool.Selected.Description))
            {
                ExtraFilters += @" and T1.""U_College"" = '" + cmbSchool.Selected.Value + @"'";
            }
            if (!string.IsNullOrWhiteSpace(cmbStatus.Selected.Description))
            {
                ExtraFilters += @" and T0.""U_RegStatus"" = '" + cmbStatus.Selected.Value + @"'";
            }
            if (!string.IsNullOrWhiteSpace(cmbSemester.Selected.Description))
            {
                ExtraFilters += @" and T0.""U_Semester"" = '" + cmbSemester.Selected.Value + @"'";
            }
            if (!string.IsNullOrWhiteSpace(txtMajor.Value))
            {
                ExtraFilters += @" and T2.""U_Program"" = '" + txtMajor.Value + @"'";
            }
            if (!string.IsNullOrWhiteSpace(txtStudent.Value))
            {
                ExtraFilters += @" and T1.""CardCode"" = '" + txtStudent.Value + @"'";
            }
            oRec.DoQuery(FormulatedQueryForRegisteration(Select, ExtraFilters));
            string FormulateUpdateQuery = "Update \"@TING1\" set ";
            for (int i = 0; i < Config.FreightCodes.Count; i++)
            {         
                if (Config.FreightCodes[i].FreightEnabled == "Y" && Config.FreightCodes[i].ScholarshipDiscount != "Y")
                {
                    FormulateUpdateQuery += "\"U_Freight" + Config.FreightCodes[i].Line + "\" = " + Config.FreightCodes[i].DefaultFreightAmount+",";
                    FormulateUpdateQuery = FormulateUpdateQuery.TrimEnd(',');
                    oRec.DoQuery(FormulateUpdateQuery);
                }
            

            }
            oRec.DoQuery("Delete from \"@TING1\" where \"U_DocTotal\" = 0");
            AING1.Query();
            ING1.LoadFromXML(AING1.GetAsXML());
            oRec.DoQuery("Delete from \"@TING1\"");
            Matrix0.Columns.Item("Select").Editable = true;
            if (Matrix0.RowCount == 0)
            {
                Global.SetMessage("No Student(s) found to Invoice(s) with this selection criteria", SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
            }
           
            UIAPIRawForm.Freeze(false);
            Global.SetMessage("Calculating Totals. Please Wait", BoStatusBarMessageType.smt_Warning);
            UIAPIRawForm.Freeze(true);
            for (int i = 0; i < Matrix0.RowCount; i++)
            {
                CalculateAndSetTotals(i+1, "");
            }
            Global.SetMessage("Calculation Completed", BoStatusBarMessageType.smt_Success);
            UIAPIRawForm.Freeze(false);
        }
        private void FillMatrixForCancellation(string Select)
        {
            UIAPIRawForm.Freeze(true);
            Recordset oRec = Global.Comp_DI.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
            string ExtraFilters = "";

            if (!string.IsNullOrWhiteSpace(cmbSchool.Selected.Description))
            {
                ExtraFilters += @" and T1.""U_College"" = '" + cmbSchool.Selected.Value + @"'";
            }
            if (!string.IsNullOrWhiteSpace(cmbStatus.Selected.Description))
            {
                ExtraFilters += @" and T1.""U_Status"" = '" + cmbStatus.Selected.Value + @"'";
            }
            if (!string.IsNullOrWhiteSpace(cmbSemester.Selected.Description))
            {
                ExtraFilters += @" and T2.""U_Semester"" = '" + cmbSemester.Selected.Value + @"'";
            }
            if (!string.IsNullOrWhiteSpace(txtMajor.Value))
            {
                ExtraFilters += @" and T1.""U_Major"" = '" + txtMajor.Value + @"'";
            }
            if (!string.IsNullOrWhiteSpace(txtStudent.Value))
            {
                ExtraFilters += @" and T1.""U_StudentCode"" = '" + txtStudent.Value + @"'";
            }
            oRec.DoQuery(FormulatedQueryCancellation(Select, ExtraFilters));
            AING1.Query();
            ING1.LoadFromXML(AING1.GetAsXML());
            oRec.DoQuery("Delete from \"@TING1\"");
            Matrix0.Columns.Item("Select").Editable = true;
            if (Matrix0.RowCount == 0)
            {
                Global.SetMessage("No Student(s) found for Refund(s) with this selection criteria.", SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
            }
            Matrix0.AutoResizeColumns();
            UIAPIRawForm.Freeze(false);
        }
        private bool ValidateBeforeFind()
        {
            if (string.IsNullOrEmpty(txtFromDate.Value) || string.IsNullOrEmpty(txtToDate.Value))
            {
                Global.myApi.StatusBar.SetText("Enter a valid date range", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                return false;
            }
            int Year = Convert.ToInt16(txtFromDate.Value.Substring(0, 4));
            int Month = Convert.ToInt16(txtFromDate.Value.Substring(4, 2));
            int Day = Convert.ToInt16(txtFromDate.Value.Substring(6, 2));
            FromDate = new DateTime(Year, Month, Day);
            Year = Convert.ToInt16(txtToDate.Value.Substring(0, 4));
            Month = Convert.ToInt16(txtToDate.Value.Substring(4, 2));
            Day = Convert.ToInt16(txtToDate.Value.Substring(6, 2));
            ToDate = new DateTime(Year, Month, Day);
            if (FromDate > ToDate)
            {
                Global.myApi.StatusBar.SetText("Enter a valid date range", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                return false;
            }
            if (DocType == "C" && cmbCancelType.Selected.Description == "")
            {
                Global.myApi.StatusBar.SetText("Select Refund Type", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                return false;
            }
            //if (string.IsNullOrEmpty(txtProgramCode.Value))
            //{
            //    Global.myApi.StatusBar.SetText("Enter Program Code", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            //    return false;
            //}
            return true;
        }
        private bool ValidateFind(bool OnAdd)
        {
            if (OnAdd && UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
            {
                return false;
            }
            else if (!OnAdd && UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
            {
                return true;
            }
            else if (UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_FIND_MODE)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private async void InitiateRevenuePosting(string DocEntry)
        {
            await Task.Run(() => PostRevenue(DocEntry));
        }
        private async void InitiateDocumentsPosting(string DocEntry)
        {
            await Task.Run(() => PostDocuments(DocEntry));
            InitiateRevenuePosting(DocEntry);
        }
        string DiscountGL = "";
        private void PostDocuments(string GenNo)
        {
            try
            {
                ErrorList.Clear();
                DocEntries.Clear();
                Recordset oRec = Global.Comp_DI.GetBusinessObject(BoObjectTypes.BoRecordset) as SAPbobsCOM.Recordset;
                string Query = "";

                //if (Global.Comp_DI.DbServerType == BoDataServerTypes.dst_HANADB)
                //{
                //    Query = "Select TO_VARCHAR(T0.\"U_DocDate\",'" + Global._dateFormat.ToUpper() + "') as \"DocDate\",TO_VARCHAR(T0.\"U_DueDate\",'" + Global._dateFormat.ToUpper() + "') as \"DueDate\",T1.\"U_StudentCode\" as \"CardCode\",TO_VARCHAR(T2.\"U_StartDate\",'" + Global._dateFormat.ToUpper() + "') as \"StartDate\",TO_VARCHAR(T2.\"U_EndDate\",'" + Global._dateFormat.ToUpper() + "') as \"EndDate\",T1.\"U_RegNo\" as \"Register\",T1.\"LineId\",T1.\"U_TaxCode\" as \"TaxCode\",T1.\"U_Remarks\" as \"Remarks\",T3.\"U_College\",T1.\"U_Major\",T1.\"U_Scholarship\",T3.\"U_Status\",T0.\"U_DocType\",T1.\"U_CancelType\",T1.\"U_BaseLine\",T1.\"U_BaseDoc\",coalesce(T1.\"U_InvNum\",0) as \"U_InvNum\",coalesce(T1.\"U_InvEntry\",0) as \"U_InvEntry\",coalesce(T1.\"U_MemoNum\",0) as \"U_MemoNum\",coalesce(T1.\"U_MemoEntry\",0) as \"U_MemoEntry\" ,T3.\"U_Semester\",T1.\"U_HDiscount\",T1.\"U_LDiscount\",T1.\"U_Remarks\",T1.\"U_Freight1\" ,T1.\"U_Freight2\" ,T1.\"U_Freight3\" ,T1.\"U_Freight4\" ,T1.\"U_Freight5\" ,T1.\"U_Freight6\" ,T1.\"U_Freight7\",T1.\"U_Freight8\",T1.\"U_Freight9\" ,T1.\"U_Freight10\",T1.\"U_HasRepeat\"    from \"@OING\" T0 inner join \"@ING1\" T1 on T0.\"DocEntry\" = T1.\"DocEntry\" inner join \"@OSRG\" T2 on T1.\"U_RegNo\" = T2.\"DocEntry\" inner join OCRD T3 on T3.\"CardCode\" = T2.\"U_StudentCode\" where T1.\"U_Select\" = 'Y' and ((coalesce(T1.\"U_Invoiced\",'N') = 'N' and T0.\"U_DocType\" = 'R' and coalesce(T2.\"U_Invoiced\",'N') = 'N') or (coalesce(T1.\"U_Invoiced\",'N') = 'Y' and coalesce(T2.\"U_Invoiced\",'N') = 'Y' and T0.\"U_DocType\" = 'C' and coalesce(T1.\"U_DocCancel\",'N') = 'N')) and  T0.\"DocEntry\" = " + GenNo;
                //}
                //else
                //{
                //    Query = "Select FORMAT(T0.\"U_DocDate\",'" + Global._dateFormat + "') as \"DocDate\",FORMAT(T0.\"U_DueDate\",'" + Global._dateFormat + "') as \"DueDate\",T1.\"U_StudentCode\" as \"CardCode\",FORMAT(T2.\"U_StartDate\",'" + Global._dateFormat + "') as \"StartDate\",FORMAT(T2.\"U_EndDate\",'" + Global._dateFormat + "') as \"EndDate\",T1.\"U_RegNo\" as \"Register\",T1.\"LineId\",T1.\"U_TaxCode\" as \"TaxCode\",T1.\"U_Remarks\" as \"Remarks\",T3.\"U_College\",T1.\"U_Major\",T1.\"U_Scholarship\",T3.\"U_Status\",T0.\"U_DocType\",T1.\"U_CancelType\",T1.\"U_BaseLine\",T1.\"U_BaseDoc\",coalesce(T1.\"U_InvNum\",0) as \"U_InvNum\",coalesce(T1.\"U_InvEntry\",0) as \"U_InvEntry\",coalesce(T1.\"U_MemoNum\",0) as \"U_MemoNum\",coalesce(T1.\"U_MemoEntry\",0) as \"U_MemoEntry\" ,T3.\"U_Semester\",T1.\"U_HDiscount\",T1.\"U_LDiscount\",T1.\"U_Remarks\",T1.\"U_Freight1\" ,T1.\"U_Freight2\" ,T1.\"U_Freight3\" ,T1.\"U_Freight4\" ,T1.\"U_Freight5\" ,T1.\"U_Freight6\" ,T1.\"U_Freight7\",T1.\"U_Freight8\",T1.\"U_Freight9\" ,T1.\"U_Freight10\",T1.\"U_HasRepeat\"     from \"@OING\" T0 inner join \"@ING1\" T1 on T0.\"DocEntry\" = T1.\"DocEntry\" inner join \"@OSRG\" T2 on T1.\"U_RegNo\" = T2.\"DocEntry\" inner join OCRD T3 on T3.\"CardCode\" = T2.\"U_StudentCode\" where T1.\"U_Select\" = 'Y' and ((coalesce(T1.\"U_Invoiced\",'N') = 'N' and T0.\"U_DocType\" = 'R' and coalesce(T2.\"U_Invoiced\",'N') = 'N') or (coalesce(T1.\"U_Invoiced\",'N') = 'Y' and coalesce(T2.\"U_Invoiced\",'N') = 'Y' and T0.\"U_DocType\" = 'C' and coalesce(T1.\"U_DocCancel\",'N') = 'N')) and  T0.\"DocEntry\" = " + GenNo;
                //}
                if (Global.Comp_DI.DbServerType == BoDataServerTypes.dst_HANADB)
                {
                    Query = "Select TO_VARCHAR(T0.\"U_DocDate\",'" + Global._dateFormat.ToUpper() + "') as \"DocDate\",TO_VARCHAR(T0.\"U_DueDate\",'" + Global._dateFormat.ToUpper() + "') as \"DueDate\",T1.\"U_StudentCode\" as \"CardCode\",TO_VARCHAR(T2.\"U_StartDate\",'" + Global._dateFormat.ToUpper() + "') as \"StartDate\",TO_VARCHAR(T2.\"U_EndDate\",'" + Global._dateFormat.ToUpper() + "') as \"EndDate\",T1.\"U_RegNo\" as \"Register\",T1.\"LineId\",T1.\"U_TaxCode\" as \"TaxCode\",T1.\"U_Remarks\" as \"Remarks\",T1.\"U_College\",T1.\"U_Major\",T1.\"U_Scholarship\",T1.\"U_Status\",T0.\"U_DocType\",T1.\"U_CancelType\",T1.\"U_BaseLine\",T1.\"U_BaseDoc\",coalesce(T1.\"U_InvNum\",0) as \"U_InvNum\",coalesce(T1.\"U_InvEntry\",0) as \"U_InvEntry\",coalesce(T1.\"U_MemoNum\",0) as \"U_MemoNum\",coalesce(T1.\"U_MemoEntry\",0) as \"U_MemoEntry\" ,T3.\"U_Semester\",T1.\"U_HDiscount\",(T1.\"U_HDiscount\" / T1.\"U_DocTotal\") * 100 as \"HDiscountPC\",T1.\"U_LDiscount\",T1.\"U_Remarks\",T1.\"U_Freight1\" ,T1.\"U_Freight2\" ,T1.\"U_Freight3\" ,T1.\"U_Freight4\" ,T1.\"U_Freight5\" ,T1.\"U_Freight6\" ,T1.\"U_Freight7\",T1.\"U_Freight8\",T1.\"U_Freight9\" ,T1.\"U_Freight10\",T1.\"U_HasRepeat\",T1.\"U_SchDiscount\",T1.\"U_DiscountGL\"   from \"@OING\" T0 inner join \"@ING1\" T1 on T0.\"DocEntry\" = T1.\"DocEntry\" inner join \"@OSRG\" T2 on T1.\"U_RegNo\" = T2.\"DocEntry\" inner join OCRD T3 on T3.\"CardCode\" = T2.\"U_StudentCode\" where T1.\"U_Select\" = 'Y' and ((coalesce(T1.\"U_Invoiced\",'N') = 'N' and T0.\"U_DocType\" = 'R' and (coalesce(T2.\"U_Invoiced\",'N') = 'N' or (coalesce(T2.\"U_RInvoiced\",'N') = 'N' and coalesce(T2.\"U_HasRepeat\",'N') = 'Y'))) or (coalesce(T1.\"U_Invoiced\",'N') = 'Y' and (coalesce(T2.\"U_Invoiced\",'N') = 'Y' or (coalesce(T2.\"U_RInvoiced\",'N') = 'Y' and coalesce(T2.\"U_HasRepeat\",'N') = 'Y')) and T0.\"U_DocType\" = 'C' and coalesce(T1.\"U_DocCancel\",'N') = 'N')) and T1.\"U_DocTotal\" > 0 and T0.\"DocEntry\" = " + GenNo +" order by T1.\"U_DiscountGL\"";
                }
                else
                {
                    Query = "Select FORMAT(T0.\"U_DocDate\",'" + Global._dateFormat + "') as \"DocDate\",FORMAT(T0.\"U_DueDate\",'" + Global._dateFormat + "') as \"DueDate\",T1.\"U_StudentCode\" as \"CardCode\",FORMAT(T2.\"U_StartDate\",'" + Global._dateFormat + "') as \"StartDate\",FORMAT(T2.\"U_EndDate\",'" + Global._dateFormat + "') as \"EndDate\",T1.\"U_RegNo\" as \"Register\",T1.\"LineId\",T1.\"U_TaxCode\" as \"TaxCode\",T1.\"U_Remarks\" as \"Remarks\",T1.\"U_College\",T1.\"U_Major\",T1.\"U_Scholarship\",T1.\"U_Status\",T0.\"U_DocType\",T1.\"U_CancelType\",T1.\"U_BaseLine\",T1.\"U_BaseDoc\",coalesce(T1.\"U_InvNum\",0) as \"U_InvNum\",coalesce(T1.\"U_InvEntry\",0) as \"U_InvEntry\",coalesce(T1.\"U_MemoNum\",0) as \"U_MemoNum\",coalesce(T1.\"U_MemoEntry\",0) as \"U_MemoEntry\" ,T3.\"U_Semester\",T1.\"U_HDiscount\",(T1.\"U_HDiscount\" / T1.\"U_DocTotal\") * 100 as \"HDiscountPC\",T1.\"U_LDiscount\",T1.\"U_Remarks\",T1.\"U_Freight1\" ,T1.\"U_Freight2\" ,T1.\"U_Freight3\" ,T1.\"U_Freight4\" ,T1.\"U_Freight5\" ,T1.\"U_Freight6\" ,T1.\"U_Freight7\",T1.\"U_Freight8\",T1.\"U_Freight9\" ,T1.\"U_Freight10\",T1.\"U_HasRepeat\" ,T1.\"U_SchDiscount\",T1.\"U_DiscountGL\"   from \"@OING\" T0 inner join \"@ING1\" T1 on T0.\"DocEntry\" = T1.\"DocEntry\" inner join \"@OSRG\" T2 on T1.\"U_RegNo\" = T2.\"DocEntry\" inner join OCRD T3 on T3.\"CardCode\" = T2.\"U_StudentCode\" where T1.\"U_Select\" = 'Y' and ((coalesce(T1.\"U_Invoiced\",'N') = 'N' and T0.\"U_DocType\" = 'R' and (coalesce(T2.\"U_Invoiced\",'N') = 'N' or (coalesce(T2.\"U_RInvoiced\",'N') = 'N' and coalesce(T2.\"U_HasRepeat\",'N') = 'Y'))) or (coalesce(T1.\"U_Invoiced\",'N') = 'Y' and (coalesce(T2.\"U_Invoiced\",'N') = 'Y' or (coalesce(T2.\"U_RInvoiced\",'N') = 'Y' and coalesce(T2.\"U_HasRepeat\",'N') = 'Y')) and T0.\"U_DocType\" = 'C' and coalesce(T1.\"U_DocCancel\",'N') = 'N')) and T1.\"U_DocTotal\" > 0 and T0.\"DocEntry\" = " + GenNo + " order by T1.\"U_DiscountGL\"";
                }
                oRec.DoQuery(Query);
                string CardCode = "";
                string DocTypeInTransaction = oRec.Fields.Item("U_DocType").Value.ToString();
                DiscountGL = "";
                while (!oRec.EoF)
                {
                    try
                    {
                        CardCode = oRec.Fields.Item("CardCode").Value.ToString();
                        if (oRec.Fields.Item("U_DocType").Value.ToString() == "R")
                        {
                            if (oRec.Fields.Item("U_InvEntry").Value.ToString() == "0")
                            {
                                CreateInvoice(oRec, GenNo);
                            }
                            else
                            {
                                oRec.MoveNext();
                                continue;
                            }
                        }
                        else
                        {
                            if (oRec.Fields.Item("U_MemoEntry").Value.ToString() == "0")
                            {
                                switch (oRec.Fields.Item("U_CancelType").Value.ToString())
                                {

                                    case "-":
                                        ErrorList.Add(new List<string>());
                                        ErrorList[ErrorList.Count - 1].Add("2:" + CardCode);
                                        ErrorList[ErrorList.Count - 1].Add(CardCode + ": Update Refund Type"); break;
                                    case "R": case "N": CreateMemo(oRec, GenNo); break;
                                    case "C": CancelPaymentAndInvoice(oRec); break;
                                    case "M": CreateManualMemo(oRec, GenNo); break;
                                }
                            }
                            else
                            {
                                oRec.MoveNext();
                                continue;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
;                       Global.SetMessage(ex.Message, BoStatusBarMessageType.smt_Error);
                        ErrorList.Add(new List<string>());
                        ErrorList[ErrorList.Count - 1].Add("2:" + CardCode);
                        ErrorList[ErrorList.Count - 1].Add(CardCode + ":" + ex.Message);

                    }
                    oRec.MoveNext();
                }

                if (DocEntries.Count > 0)
                {
                    UpdateRegistration(GenNo, DocTypeInTransaction);
                    UpdateFetchingOfRegistration(GenNo);
                    Global.SendMessage("Document(s) Success Notification", "Following " + (DocType == "R" ? "Invoice(s)" : "Credit Memo(s)") + " Posted Successfully", new string[] { Global.Comp_DI.UserName }, DocEntries, DocType == "R" ? "13" : "14", DocType == "R" ? "Invoice(s)" : "Credit Memo(s)");
                }
                if (ErrorList.Count > 0)
                {
                    Global.SendMessage("Document(s) Failure Notification", "Following Student(s) Invoices Failed to Post", new string[] { Global.Comp_DI.UserName }, ErrorList, "2", "Student(s)");
                }
            }
            catch (Exception ex)
            {
                Global.SetMessage(ex.Message + " Method:" + "PostDocuments", SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
            finally
            {
                DocEntries.Clear();
                ErrorList.Clear();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
        private void UpdateFreight(int FreightCode, string Revenue,string CardCode)
        {
            try
            {
                AdditionalExpenses additionalExpenses = Global.Comp_DI.GetBusinessObject(BoObjectTypes.oAdditionalExpenses) as AdditionalExpenses;
                if (additionalExpenses.GetByKey(FreightCode))
                {
                    additionalExpenses.RevenuesAccount = Revenue;
                    additionalExpenses.Update();
                }
            }
            catch (Exception ex)
            {
                Global.SetMessage(ex.Message, BoStatusBarMessageType.smt_Error);
                ErrorList.Add(new List<string>());
                ErrorList[ErrorList.Count - 1].Add("2:" + CardCode);
                ErrorList[ErrorList.Count - 1].Add(CardCode + ":" + ex.Message);
            }
        }
        private bool CreateInvoice(Recordset oRec, string GenNo)
        {
            string CardCode = "";
            string[] formats = { Global._dateFormat };
            DateTime StartDate = new DateTime();
            DateTime EndDate = new DateTime();
            DateTime.TryParseExact(oRec.Fields.Item("DocDate").Value.ToString(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out StartDate);
            DateTime.TryParseExact(oRec.Fields.Item("DueDate").Value.ToString(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out EndDate);
            string Gen = GenNo;
            string Reg = oRec.Fields.Item("Register").Value.ToString();
            string Line = oRec.Fields.Item("LineId").Value.ToString();
            string VatCode = oRec.Fields.Item("TaxCode").Value.ToString();
            double HDiscountPC = 0;
            CardCode = oRec.Fields.Item("CardCode").Value.ToString();
            Documents oDoc = Global.Comp_DI.GetBusinessObject(BoObjectTypes.oInvoices) as Documents;
            oDoc.DocDate = StartDate;
            oDoc.DocDueDate = EndDate;
            double.TryParse(oRec.Fields.Item("HDiscountPC").Value.ToString(), out HDiscountPC);
            //Get Attempted Credit Hours
            string remainingHours = string.Format("Select [U_SchrCHours],[U_AttmptCHours] from OCRD where [CardCode] = '"+ oRec.Fields.Item("CardCode").Value.ToString() +"'").Replace("[","\"").Replace("]", "\"");
            oDoc.CardCode = oRec.Fields.Item("CardCode").Value.ToString();
            oDoc.DiscountPercent = HDiscountPC;
            oDoc.UserFields.Fields.Item("U_SendInvoice").Value = Config.SendInvoice;
            oDoc.UserFields.Fields.Item("U_RegNo").Value = Reg;
            oDoc.UserFields.Fields.Item("U_GenNo").Value = Gen;
            oDoc.UserFields.Fields.Item("U_GenLine").Value = Line;
            string COLLAGE = oRec.Fields.Item("U_College").Value.ToString();
            oDoc.UserFields.Fields.Item("U_College").Value = oRec.Fields.Item("U_College").Value.ToString();
            oDoc.UserFields.Fields.Item("U_Major").Value = oRec.Fields.Item("U_Major").Value.ToString();
            oDoc.UserFields.Fields.Item("U_Status").Value = oRec.Fields.Item("U_Status").Value.ToString();
            oDoc.UserFields.Fields.Item("U_Semester").Value = oRec.Fields.Item("U_Semester").Value.ToString();
            oDoc.UserFields.Fields.Item("U_RegType").Value = oRec.Fields.Item("U_HasRepeat").Value.ToString();
            oDoc.Comments = oRec.Fields.Item("Remarks").Value.ToString();
            oDoc.Series = Config.InvoiceSeries;
            var result = Config.FreightCodes.Where(x => x.ScholarshipDiscount == "Y").FirstOrDefault();
            string Freight = result?.FreightCode;

            var a = oRec.Fields.Item("U_DiscountGL").Value.ToString();
            if (oRec.Fields.Item("U_DiscountGL").Value.ToString() != DiscountGL && !string.IsNullOrEmpty(Freight))
            {
                DiscountGL = oRec.Fields.Item("U_DiscountGL").Value.ToString();
              
                UpdateFreight(Convert.ToInt32(Config.FreightCodes.Where(x => x.ScholarshipDiscount == "Y").FirstOrDefault().FreightCode), DiscountGL, CardCode);
            }
            if (oRec.Fields.Item("U_HasRepeat").Value.ToString() == "N" && !string.IsNullOrEmpty(Freight)) 
            {
                oDoc.Expenses.ExpenseCode = Convert.ToInt32(Config.FreightCodes.Where(x => x.ScholarshipDiscount == "Y").FirstOrDefault().FreightCode);
                oDoc.Expenses.LineTotal = Convert.ToDouble(oRec.Fields.Item("U_SchDiscount").Value.ToString()) * -1;
                oDoc.Expenses.VatGroup = VatCode;
                oDoc.Expenses.UserFields.Fields.Item("U_SchDiscount").Value = "Y";
                switch (Config.ScholarDim)
                {
                    case "1": oDoc.Expenses.DistributionRule = oRec.Fields.Item("U_Scholarship").Value.ToString(); break;
                    case "2": oDoc.Expenses.DistributionRule2 = oRec.Fields.Item("U_Scholarship").Value.ToString(); break;
                    case "3": oDoc.Expenses.DistributionRule3 = oRec.Fields.Item("U_Scholarship").Value.ToString(); break;
                    case "4": oDoc.Expenses.DistributionRule4 = oRec.Fields.Item("U_Scholarship").Value.ToString(); break;
                    case "5": oDoc.Expenses.DistributionRule5 = oRec.Fields.Item("U_Scholarship").Value.ToString(); break;
                }
                switch (Config.CollegeDim)
                {
                    case "1": oDoc.Expenses.DistributionRule = oRec.Fields.Item("U_College").Value.ToString(); break;
                    case "2": oDoc.Expenses.DistributionRule2 = oRec.Fields.Item("U_College").Value.ToString(); break;
                    case "3": oDoc.Expenses.DistributionRule3 = oRec.Fields.Item("U_College").Value.ToString(); break;
                    case "4": oDoc.Expenses.DistributionRule4 = oRec.Fields.Item("U_College").Value.ToString(); break;
                    case "5": oDoc.Expenses.DistributionRule5 = oRec.Fields.Item("U_College").Value.ToString(); break;
                }
                switch (Config.MajorDim)
                {
                    case "1": oDoc.Expenses.DistributionRule = oRec.Fields.Item("U_Major").Value.ToString(); break;
                    case "2": oDoc.Expenses.DistributionRule2 = oRec.Fields.Item("U_Major").Value.ToString(); break;
                    case "3": oDoc.Expenses.DistributionRule3 = oRec.Fields.Item("U_Major").Value.ToString(); break;
                    case "4": oDoc.Expenses.DistributionRule4 = oRec.Fields.Item("U_Major").Value.ToString(); break;
                    case "5": oDoc.Expenses.DistributionRule5 = oRec.Fields.Item("U_Major").Value.ToString(); break;
                }
            }
            for (int i = 0; i < 10; i++)
            {
                double FreightVal = 0;
                double.TryParse(oRec.Fields.Item("U_Freight" + (i + 1).ToString()).Value.ToString(), out FreightVal);
                if (FreightVal > 0)
                {
                    if (oDoc.Expenses.LineGross > 0)
                    {
                        oDoc.Expenses.Add();
                    }
                    if (Config.FreightCodes.Where(x => x.Line == (i + 1).ToString()).FirstOrDefault().ScholarshipDiscount == "N")
                    {
                        oDoc.Expenses.ExpenseCode = Convert.ToInt32(Config.FreightCodes.Where(x => x.Line == (i + 1).ToString()).FirstOrDefault().FreightCode);
                        oDoc.Expenses.LineGross = FreightVal;
                    }
                }
            }

            //if (Convert.ToInt32(oRec.Fields.Item("TrailCount").Value.ToString()) > 0 || oRec.Fields.Item("U_Status").Value.ToString().ToLower() == "retake" || oRec.Fields.Item("U_Courses").Value.ToString() == "Y")
            //{
            Recordset oRec1 = Global.Comp_DI.GetBusinessObject(BoObjectTypes.BoRecordset) as SAPbobsCOM.Recordset;

            oRec1.DoQuery("Select T0.\"U_SubjectCode\", T0.\"U_Credits\",coalesce(T0.\"U_Price\",0) as \"UnitPrice\",T0.\"U_Total\",T0.\"U_Discount\",coalesce(T0.\"U_DiscountPC\",0) as \"U_DiscountPC\" from \"@SRG1\" T0 inner join OITM T1 on T0.\"U_SubjectCode\" = T1.\"ItemCode\" where coalesce(T0.\"U_Repeat\",'N') = '" + oRec.Fields.Item("U_HasRepeat").Value.ToString() + "' and T0.\"DocEntry\" = " + Reg);
            while (!oRec1.EoF)
            {
                if (oDoc.Lines.ItemCode != "")
                {
                    oDoc.Lines.Add();
                }
                oDoc.Lines.ItemCode = oRec1.Fields.Item("U_SubjectCode").Value.ToString();
                oDoc.Lines.UnitPrice = Convert.ToDouble(oRec1.Fields.Item("UnitPrice").Value.ToString());
                oDoc.Lines.Quantity = Convert.ToDouble(oRec1.Fields.Item("U_Credits").Value.ToString());
                oDoc.Lines.VatGroup = VatCode;
                oDoc.Lines.DiscountPercent = Convert.ToDouble(oRec1.Fields.Item("U_DiscountPC").Value.ToString());
                var B = oRec.Fields.Item("U_College").Value.ToString();
                oDoc.Lines.UserFields.Fields.Item("U_Scholarship").Value = oRec.Fields.Item("U_Scholarship").Value.ToString();
            oDoc.Lines.UserFields.Fields.Item("U_College").Value = oRec.Fields.Item("U_College").Value.ToString();

            switch (Config.MajorDim)
                {
                    case "1": oDoc.Lines.CostingCode = oRec.Fields.Item("U_Major").Value.ToString(); break;
                    case "2": oDoc.Lines.CostingCode2 = oRec.Fields.Item("U_Major").Value.ToString(); break;
                    case "3": oDoc.Lines.CostingCode3 = oRec.Fields.Item("U_Major").Value.ToString(); break;
                    case "4": oDoc.Lines.CostingCode4 = oRec.Fields.Item("U_Major").Value.ToString(); break;
                    case "5": oDoc.Lines.CostingCode5 = oRec.Fields.Item("U_Major").Value.ToString(); break;
                }
                switch (Config.ScholarDim)
                {
                    case "1": oDoc.Lines.CostingCode = oRec.Fields.Item("U_Scholarship").Value.ToString(); break;
                    case "2": oDoc.Lines.CostingCode2 = oRec.Fields.Item("U_Scholarship").Value.ToString(); break;
                    case "3": oDoc.Lines.CostingCode3 = oRec.Fields.Item("U_Scholarship").Value.ToString(); break;
                    case "4": oDoc.Lines.CostingCode4 = oRec.Fields.Item("U_Scholarship").Value.ToString(); break;
                    case "5": oDoc.Lines.CostingCode5 = oRec.Fields.Item("U_Scholarship").Value.ToString(); break;
                }
                switch (Config.CollegeDim)
                {
                    case "1": oDoc.Lines.CostingCode = oRec.Fields.Item("U_College").Value.ToString(); break;
                    case "2": oDoc.Lines.CostingCode2 = oRec.Fields.Item("U_College").Value.ToString(); break;
                    case "3": oDoc.Lines.CostingCode3 = oRec.Fields.Item("U_College").Value.ToString(); break;
                    case "4": oDoc.Lines.CostingCode4 = oRec.Fields.Item("U_College").Value.ToString(); break;
                    case "5": oDoc.Lines.CostingCode5 = oRec.Fields.Item("U_College").Value.ToString(); break;
                }
                oRec1.MoveNext();

        }
            //}

            if (oDoc.Add() != 0)
            {
                string ErrorMsg = Global.Comp_DI.GetLastErrorDescription();
                ErrorList.Add(new List<string>());
                ErrorList[ErrorList.Count - 1].Add("2:" + oDoc.CardCode);
                ErrorList[ErrorList.Count - 1].Add(oDoc.CardCode + ":" + ErrorMsg);
                return false;
            }
            else
            {
                string NewEntry = Global.Comp_DI.GetNewObjectKey();
                DocEntries.Add(new List<string>());
                DocEntries[DocEntries.Count - 1].Add("13:" + NewEntry);
                DocEntries[DocEntries.Count - 1].Add(NewEntry);
                return true;
            }
        }
        private bool CreateManualMemo(Recordset oRec, string GenNo)
        {
            Documents oDoc;
            try
            {
                string[] formats = { Global._dateFormat };
                DateTime StartDate = new DateTime();
                DateTime EndDate = new DateTime();
                DateTime.TryParseExact(oRec.Fields.Item("DocDate").Value.ToString(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out StartDate);
                DateTime.TryParseExact(oRec.Fields.Item("DueDate").Value.ToString(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out EndDate);
                oDoc = Global.Comp_DI.GetBusinessObject(BoObjectTypes.oCreditNotes) as Documents;
                oDoc.DocDate = StartDate;
                oDoc.DocDueDate = EndDate;
                oDoc.CardCode = oRec.Fields.Item("CardCode").Value.ToString();
                oDoc.UserFields.Fields.Item("U_GenNo").Value = GenNo;
                oDoc.UserFields.Fields.Item("U_GenLine").Value = oRec.Fields.Item("LineId").Value.ToString();
                oDoc.DocType = BoDocumentTypes.dDocument_Service;
                oDoc.UserFields.Fields.Item("U_College").Value = oRec.Fields.Item("U_College").Value.ToString();
                oDoc.UserFields.Fields.Item("U_Major").Value = oRec.Fields.Item("U_Major").Value.ToString();
                oDoc.UserFields.Fields.Item("U_Status").Value = oRec.Fields.Item("U_Status").Value.ToString();
                oDoc.UserFields.Fields.Item("U_Semester").Value = oRec.Fields.Item("U_Semester").Value.ToString();
                oDoc.UserFields.Fields.Item("U_Scholarship").Value = oRec.Fields.Item("U_Scholarship").Value.ToString();
                oDoc.Lines.AccountCode = oRec.Fields.Item("ManualAccount").Value.ToString();
                oDoc.Lines.UnitPrice = Convert.ToDouble(oRec.Fields.Item("ManualAmount").Value.ToString());
                oDoc.Lines.VatGroup = oRec.Fields.Item("TaxCode").Value.ToString();
                switch (Config.MajorDim)
                {
                    case "1": oDoc.Lines.CostingCode = oRec.Fields.Item("U_Major").Value.ToString(); break;
                    case "2": oDoc.Lines.CostingCode2 = oRec.Fields.Item("U_Major").Value.ToString(); break;
                    case "3": oDoc.Lines.CostingCode3 = oRec.Fields.Item("U_Major").Value.ToString(); break;
                    case "4": oDoc.Lines.CostingCode4 = oRec.Fields.Item("U_Major").Value.ToString(); break;
                    case "5": oDoc.Lines.CostingCode5 = oRec.Fields.Item("U_Major").Value.ToString(); break;
                }
                switch (Config.ScholarDim)
                {
                    case "1": oDoc.Lines.CostingCode = oRec.Fields.Item("U_Scholarship").Value.ToString(); break;
                    case "2": oDoc.Lines.CostingCode2 = oRec.Fields.Item("U_Scholarship").Value.ToString(); break;
                    case "3": oDoc.Lines.CostingCode3 = oRec.Fields.Item("U_Scholarship").Value.ToString(); break;
                    case "4": oDoc.Lines.CostingCode4 = oRec.Fields.Item("U_Scholarship").Value.ToString(); break;
                    case "5": oDoc.Lines.CostingCode5 = oRec.Fields.Item("U_Scholarship").Value.ToString(); break;
                }
                switch (Config.CollegeDim)
                {
                    case "1": oDoc.Lines.CostingCode = oRec.Fields.Item("U_College").Value.ToString(); break;
                    case "2": oDoc.Lines.CostingCode2 = oRec.Fields.Item("U_College").Value.ToString(); break;
                    case "3": oDoc.Lines.CostingCode3 = oRec.Fields.Item("U_College").Value.ToString(); break;
                    case "4": oDoc.Lines.CostingCode4 = oRec.Fields.Item("U_College").Value.ToString(); break;
                    case "5": oDoc.Lines.CostingCode5 = oRec.Fields.Item("U_College").Value.ToString(); break;
                }
                if (oDoc.Add() != 0)
                {
                    string ErrorMsg = Global.Comp_DI.GetLastErrorDescription();
                    ErrorList.Add(new List<string>());
                    ErrorList[ErrorList.Count - 1].Add("2:" + oDoc.CardCode);
                    ErrorList[ErrorList.Count - 1].Add(oDoc.CardCode + ":" + ErrorMsg);
                    return false;
                }
                else
                {
                    string NewEntry = Global.Comp_DI.GetNewObjectKey();
                    DocEntries.Add(new List<string>());
                    DocEntries[DocEntries.Count - 1].Add("14:" + NewEntry);
                    DocEntries[DocEntries.Count - 1].Add(NewEntry);
                    return true;
                }
            }
            finally
            {
                oDoc = null;
            }
        }
        private void CreateMemo(Recordset oRec, string GenNo)
        {
            Recordset oRec1;
            Documents oDoc1;
            Documents oDoc;
            try
            {
                string[] formats = { Global._dateFormat };
                oRec1 = Global.Comp_DI.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
                string Query = @"Select T0.""DocEntry"",T0.""CardCode"",T1.""ItemCode"",T1.""LineNum"",T1.""Quantity"",T1.""PriceBefDi"" as ""UnitPrice"",T0.""DocTotal"",T0.""PaidToDate"" from ""INV1"" T1 inner join OINV T0 on T1.""DocEntry"" = T0.""DocEntry"" where T1.""Quantity"" >0 and T0.""DocEntry"" = " + oRec.Fields.Item("U_InvEntry").Value.ToString();
                oRec1.DoQuery(Query);
                if (oRec1.RecordCount > 0)
                {
                    DateTime StartDate = new DateTime();
                    DateTime EndDate = new DateTime();
                    DateTime.TryParseExact(oRec.Fields.Item("DocDate").Value.ToString(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out StartDate);
                    DateTime.TryParseExact(oRec.Fields.Item("DueDate").Value.ToString(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out EndDate);
                    oDoc = Global.Comp_DI.GetBusinessObject(BoObjectTypes.oCreditNotes) as Documents;
                    oDoc.DocDate = StartDate;
                    oDoc.DocDueDate = EndDate;
                    if (Convert.ToDouble(oRec1.Fields.Item("DocTotal").Value.ToString()) == Convert.ToDouble(oRec1.Fields.Item("PaidToDate").Value.ToString()))
                    {
                        oDoc1 = Global.Comp_DI.GetBusinessObject(BoObjectTypes.oInvoices) as Documents;
                        oDoc1.GetByKey(Convert.ToInt32(oRec1.Fields.Item("DocEntry").Value.ToString()));

                        if (oDoc1.Reopen() != 0)
                        {
                            string ErrorMsg = Global.Comp_DI.GetLastErrorDescription();
                            ErrorList.Add(new List<string>());
                            ErrorList[ErrorList.Count - 1].Add("2:" + oDoc1.CardCode);
                            ErrorList[ErrorList.Count - 1].Add(oDoc1.CardCode + " : Reopen Failed - " + ErrorMsg);
                        }
                    }
                    oDoc.CardCode = oRec1.Fields.Item("CardCode").Value.ToString();
                    oDoc.UserFields.Fields.Item("U_GenNo").Value = GenNo;
                    oDoc.UserFields.Fields.Item("U_GenLine").Value = oRec.Fields.Item("LineId").Value.ToString();
                    while (!oRec1.EoF)
                    {
                        if (oDoc.Lines.ItemCode != "")
                        {
                            oDoc.Lines.Add();
                        }
                        oDoc.Lines.ItemCode = oRec1.Fields.Item("ItemCode").Value.ToString();
                        oDoc.Lines.Quantity = Convert.ToDouble(oRec1.Fields.Item("Quantity").Value.ToString());
                        //oDoc.Lines.UnitPrice = Convert.ToDouble(oRec1.Fields.Item("UnitPrice").Value.ToString());
                        oDoc.Lines.BaseLine = Convert.ToInt32(oRec1.Fields.Item("LineNum").Value.ToString());
                        oDoc.Lines.BaseType = (int)BoObjectTypes.oInvoices;
                        oDoc.Lines.BaseEntry = Convert.ToInt32(oRec1.Fields.Item("DocEntry").Value.ToString());
                        oDoc.Lines.UserFields.Fields.Item("U_College").Value = oRec.Fields.Item("U_College").Value.ToString();
                        oDoc.Lines.UserFields.Fields.Item("U_Scholarship").Value = oRec.Fields.Item("U_Scholarship").Value.ToString();

                        oRec1.MoveNext();
                    }
                    Query = @"Select T0.""ExpnsCode"",T0.""GrsAmount"",T0.""DocEntry"",T0.""LineNum"" from ""INV3"" T0  where T0.""DocEntry"" = " + oRec.Fields.Item("U_InvEntry").Value.ToString();
                    oRec1.DoQuery(Query);
                    while (!oRec1.EoF)
                    {
                        if (oDoc.Expenses.BaseDocEntry > 0)
                        {
                            oDoc.Expenses.Add();
                        }
                        oDoc.Expenses.BaseDocEntry = Convert.ToInt32(oRec1.Fields.Item("DocEntry").Value.ToString());
                        oDoc.Expenses.BaseDocLine = Convert.ToInt32(oRec1.Fields.Item("LineNum").Value.ToString());
                        oDoc.Expenses.BaseDocType = 13;
                        oRec1.MoveNext();
                    }

                    if (oDoc.Add() != 0)
                    {
                        string ErrorMsg = Global.Comp_DI.GetLastErrorDescription();
                        ErrorList.Add(new List<string>());
                        ErrorList[ErrorList.Count - 1].Add("2:" + oDoc.CardCode);
                        ErrorList[ErrorList.Count - 1].Add(oDoc.CardCode + ":" + ErrorMsg);
                    }
                    else
                    {
                        string NewEntry = Global.Comp_DI.GetNewObjectKey();
                        DocEntries.Add(new List<string>());
                        DocEntries[DocEntries.Count - 1].Add("14:" + NewEntry);
                        DocEntries[DocEntries.Count - 1].Add(NewEntry);
                    }
                }
            }
            finally
            {
                oRec1 = null;
                oDoc = null;
                oDoc1 = null;
                GC.Collect();
            }

        }
        private void CancelPaymentAndInvoice(Recordset oRec)
        {
            Recordset oRec1;
            Payments oPay = null;
            Documents oDoc = null;
            try
            {
                oRec1 = Global.Comp_DI.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
                string Query = "Select T2.\"DocEntry\" as \"PayEntry\",T2.\"Canceled\" as \"PayStatus\",T1.\"DocEntry\" as \"InvEntry\",T1.\"DocNum\" as \"InvNum\",T1.\"CANCELED\" as \"InvStatus\" from OINV T1  left join VPM2 T0  on T0.\"DocEntry\" = T1.\"DocEntry\" left join OVPM T2 on T2.\"DocEntry\" = T0.\"DocNum\" where T1.\"DocEntry\" = " + oRec.Fields.Item("U_InvEntry").Value.ToString();
                oRec1.DoQuery(Query);
                bool HasPayment = false;
                if (oRec1.Fields.Item("PayStatus").Value.ToString() == "N")
                {

                    Global.Comp_DI.StartTransaction();
                    oPay = Global.Comp_DI.GetBusinessObject(BoObjectTypes.oIncomingPayments) as Payments;
                    oPay.GetByKey(Convert.ToInt32(oRec.Fields.Item("PayEntry").Value.ToString()));
                    HasPayment = true;
                    if (oPay.Cancel() != 0)
                    {
                        if (Global.Comp_DI.InTransaction)
                        {
                            Global.Comp_DI.EndTransaction(BoWfTransOpt.wf_RollBack);
                        }
                        string ErrorMsg = Global.Comp_DI.GetLastErrorDescription();
                        ErrorList.Add(new List<string>());
                        ErrorList[ErrorList.Count - 1].Add("24:" + oPay.DocEntry.ToString());
                        ErrorList[ErrorList.Count - 1].Add(oRec.Fields.Item("CardCode").Value.ToString() + ":" + ErrorMsg + "-Payment No." + oPay.DocNum.ToString());
                    }
                }
                if (oRec1.Fields.Item("InvStatus").Value.ToString() == "N" && ((HasPayment && Global.Comp_DI.InTransaction) || (!HasPayment && !Global.Comp_DI.InTransaction)))
                {
                    if (!Global.Comp_DI.InTransaction)
                    {
                        Global.Comp_DI.StartTransaction();
                    }
                    oDoc = Global.Comp_DI.GetBusinessObject(BoObjectTypes.oInvoices) as Documents;
                    oDoc.GetByKey(Convert.ToInt32(oRec1.Fields.Item("InvEntry").Value.ToString()));
                    oDoc.CancelDate = oDoc.DocDate;
                    if (oDoc.CreateCancellationDocument().Add() != 0)
                    {
                        if (Global.Comp_DI.InTransaction)
                        {
                            Global.Comp_DI.EndTransaction(BoWfTransOpt.wf_RollBack);
                        }
                        string ErrorMsg = Global.Comp_DI.GetLastErrorDescription();

                        ErrorList.Add(new List<string>());
                        ErrorList[ErrorList.Count - 1].Add("13:" + oDoc.DocEntry.ToString());
                        ErrorList[ErrorList.Count - 1].Add(oRec.Fields.Item("CardCode").Value.ToString() + ":" + ErrorMsg + "-Invoice No." + oDoc.DocNum.ToString());
                    }
                    else
                    {
                        if (Global.Comp_DI.InTransaction)
                        {
                            Global.Comp_DI.EndTransaction(BoWfTransOpt.wf_Commit);
                        }
                        string NewEntry = Global.Comp_DI.GetNewObjectKey();
                        DocEntries.Add(new List<string>());
                        DocEntries[DocEntries.Count - 1].Add("13:" + NewEntry);
                        DocEntries[DocEntries.Count - 1].Add(oRec.Fields.Item("CardCode").Value.ToString() + " : " + (HasPayment == true ? " Payment No." + oPay.DocNum.ToString() + " and " : "") + "Invoice No." + oDoc.DocNum.ToString() + " has been cancelled");
                    }
                }
            }
            catch (Exception ex)
            {
                Global.SetMessage(ex.Message, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

                if (Global.Comp_DI.InTransaction)
                {
                    Global.Comp_DI.EndTransaction(BoWfTransOpt.wf_RollBack);
                }
                string ErrorMsg = "Unable to cancel Payment and Invoice. Method : " + "CancelPaymentAndInvoice";
                ErrorList.Add(new List<string>());
                ErrorList[ErrorList.Count - 1].Add("2:" + oRec.Fields.Item("U_StudentCode").Value.ToString());
                ErrorList[ErrorList.Count - 1].Add(oRec.Fields.Item("U_StudentCode").Value.ToString() + ":" + ErrorMsg);
            }
        }
        private void ProrateRevenueAlgorithm(decimal AmountToProrate, DateTime startDate, DateTime endDate, string StudentID, string RegNo, string GenNo, string InvEntry, string InvNum, string DebitAccount, string CreditAccount, string ProgramCode, string Scholarship, string MemoEntry, string MemoNum, string DocumentType, string CancelType, string CancelEntry, string CancelNum)
        {

            int DaysBetween = (endDate - startDate).Days + 1;
            int Months = ((endDate.Year - startDate.Year) * 12) + endDate.Month - startDate.Month;

            //if (endDate.Month != startDate.Month && Config.ProratedOn == "M" && Months == 1)
            //{
            //    Months = 2;
            //}
            //RESOLVE
            int MonthsForMonths = ((endDate.Year - startDate.Year) * 12) + endDate.Month - startDate.Month + 1;
            decimal DailyAmount = AmountToProrate / DaysBetween;
            decimal MonthlyAmount = AmountToProrate / MonthsForMonths;

            if (Months < 0)
            {
                string ErrorMsg = "From Date (Document Date) cannot be greater than registration End Date";
                ErrorList.Add(new List<string>());
                ErrorList[ErrorList.Count - 1].Add("2:" + StudentID);
                ErrorList[ErrorList.Count - 1].Add(StudentID + " " + ErrorMsg);
            }
            //if (Config.AccountFrom == "C")
            //{
            //    DebitAccount = Config.DebitAccount;
            //    CreditAccount = Config.CreditAccount;
            //}

            if (Months == 0)
            {
                MonthlyAmounts amount = new MonthlyAmounts();
                amount.StudentID = StudentID;
                amount.RegNo = RegNo;
                amount.GenNo = GenNo;
                amount.DebitAccount = DebitAccount;
                amount.CreditAccount = CreditAccount;
                amount.InvEntry = InvEntry;
                amount.InvNum = InvNum;
                amount.Major = ProgramCode;
                amount.Scholarship = Scholarship;
                amount.MemoEntry = MemoEntry;
                amount.MemoNum = MemoNum;
                amount.DocType = DocumentType;
                amount.CancelType = CancelType;
                amount.CancelEntry = CancelEntry;
                amount.CancelNum = CancelNum;
                amount.First = true;
                amount.Days = (new DateTime(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month)) - startDate).Days + 1;
                decimal FirstMonthAmount = amount.Days * DailyAmount;
                amount.Amount = Config.ProratedOn == "M" ? MonthlyAmount : FirstMonthAmount;
                amount.Month = startDate.Month;
                amount.Year = startDate.Year;
                amount.Date = new DateTime(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month));
                amount.NameOfMonth = amount.Date.ToString("MMM");
                mymonths.Add(amount);
            }
            for (int i = 0; i < Months; i++)
            {
                MonthlyAmounts amount = new MonthlyAmounts();
                amount.StudentID = StudentID;
                amount.RegNo = RegNo;
                amount.GenNo = GenNo;
                amount.DebitAccount = DebitAccount;
                amount.CreditAccount = CreditAccount;
                amount.InvEntry = InvEntry;
                amount.InvNum = InvNum;
                amount.Major = ProgramCode;
                amount.Scholarship = Scholarship;
                amount.MemoEntry = MemoEntry;
                amount.MemoNum = MemoNum;
                amount.DocType = DocumentType;
                amount.CancelType = CancelType;
                amount.CancelEntry = CancelEntry;
                amount.CancelNum = CancelNum;
                if (i == 0 && Months != 1)
                {
                    amount.First = true;
                    amount.Days = (new DateTime(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month)) - startDate).Days + 1;
                    //amount.Days = DateTime.DaysInMonth(startDate.Year, startDate.Month) - startDate.Day + 1;
                    decimal FirstMonthAmount = amount.Days * DailyAmount;
                    amount.Amount = Config.ProratedOn == "M" ? MonthlyAmount : FirstMonthAmount;
                    amount.Month = startDate.Month;
                    amount.Year = startDate.Year;
                    amount.Date = new DateTime(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month));
                    amount.NameOfMonth = amount.Date.ToString("MMM");
                }
                else if (i > 0 && i < Months - 1)
                {
                    startDate = startDate.AddMonths(1);
                    amount.Days = DateTime.DaysInMonth(startDate.Year, startDate.Month);
                    amount.Amount = Config.ProratedOn == "M" ? MonthlyAmount : amount.Days * DailyAmount;
                    amount.Year = startDate.Year;
                    amount.Month = startDate.Month;
                    amount.Date = new DateTime(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month));
                    amount.NameOfMonth = amount.Date.ToString("MMM");
                }
                else if (i == Months - 1)
                {

                    DateTime SecondLastMonth = endDate.AddMonths(-1);
                    MonthlyAmounts SecondLast = new MonthlyAmounts();
                    SecondLast.StudentID = StudentID;
                    SecondLast.RegNo = RegNo;
                    SecondLast.GenNo = GenNo;
                    SecondLast.DebitAccount = DebitAccount;
                    SecondLast.CreditAccount = CreditAccount;
                    SecondLast.InvEntry = InvEntry;
                    SecondLast.InvNum = InvNum;
                    SecondLast.Days = DateTime.DaysInMonth(SecondLastMonth.Year, SecondLastMonth.Month);
                    SecondLast.Amount = Config.ProratedOn == "M" ? MonthlyAmount : SecondLast.Days * DailyAmount;
                    SecondLast.Year = SecondLastMonth.Year;
                    SecondLast.Month = SecondLastMonth.Month;
                    SecondLast.Date = new DateTime(SecondLastMonth.Year, SecondLastMonth.Month, DateTime.DaysInMonth(SecondLastMonth.Year, SecondLastMonth.Month));
                    SecondLast.NameOfMonth = SecondLast.Date.ToString("MMM");
                    SecondLast.Major = ProgramCode;
                    SecondLast.Scholarship = Scholarship;
                    SecondLast.MemoEntry = MemoEntry;
                    SecondLast.MemoNum = MemoNum;
                    SecondLast.DocType = DocumentType;
                    SecondLast.CancelType = CancelType;
                    SecondLast.CancelEntry = CancelEntry;
                    SecondLast.CancelNum = CancelNum;
                    mymonths.Add(SecondLast);

                    amount.Last = true;
                    amount.Days = endDate.Day;
                    amount.Amount = Config.ProratedOn == "M" ? MonthlyAmount : amount.Days * DailyAmount;
                    amount.Month = endDate.Month;
                    amount.Year = endDate.Year;
                    amount.Date = endDate;
                    amount.NameOfMonth = amount.Date.ToString("MMM");
                }


                //Global.SetMessage("Voucher Fetched for " + amount.Date.ToString("MMM") + "-" + amount.Year.ToString(), SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                mymonths.Add(amount);
            }
            //decimal RemainderAmount = mymonths.Sum(x => x.Amount) / AmountToProrate;
            //if (RemainderAmount > 0)
            //{
            //    for (int i = 0; i < mymonths.Count; i++)
            //    {
            //        mymonths[i].Amount = mymonths[i].Amount - (RemainderAmount/mymonths.Count);
            //    }
            //}
        }
        private void CreateNewVoucherPerDate(DateTime DateOfPosting, string GenNo)
        {

            var VouchersToPost = mymonths.Where(x => x.Date == DateOfPosting).ToList();
            JournalVouchers oJV = Global.Comp_DI.GetBusinessObject(BoObjectTypes.oJournalVouchers) as JournalVouchers;
            try
            {

                oJV.JournalEntries.ReferenceDate = DateOfPosting;
                oJV.JournalEntries.TaxDate = DateOfPosting;
                oJV.JournalEntries.DueDate = DateOfPosting;
                oJV.JournalEntries.Reference = GenNo;
                oJV.JournalEntries.Series = Config.VoucherSeries;
                oJV.JournalEntries.UserFields.Fields.Item("U_StVoucher").Value = "Y";

                if (VouchersToPost[0].DocType == "R")
                {
                    oJV.JournalEntries.Memo = "Student Revenue Posting for the Month of : " + VouchersToPost[0].NameOfMonth + "-" + VouchersToPost[0].Year.ToString();
                    oJV.JournalEntries.UserFields.Fields.Item("U_Reversal").Value = "N";
                }
                else
                {
                    oJV.JournalEntries.Memo = "Student Revenue Reversal Posting for the Month of : " + VouchersToPost[0].NameOfMonth + "-" + VouchersToPost[0].Year.ToString();
                    oJV.JournalEntries.UserFields.Fields.Item("U_Reversal").Value = "Y";
                }


                for (int i = 0; i < VouchersToPost.Count; i++)
                {
                    if (oJV.JournalEntries.Lines.AccountCode != "")
                    {
                        oJV.JournalEntries.Lines.Add();
                    }
                    oJV.JournalEntries.Lines.AccountCode = VouchersToPost[i].DebitAccount;
                    oJV.JournalEntries.Lines.Debit = Convert.ToDouble(VouchersToPost[i].Amount);
                    if (VouchersToPost[i].DocType == "R")
                    {

                        oJV.JournalEntries.Lines.AdditionalReference = VouchersToPost[i].InvNum;
                        oJV.JournalEntries.Lines.UserFields.Fields.Item("U_InvEntry").Value = VouchersToPost[i].InvEntry;
                    }
                    else
                    {
                        if (VouchersToPost[i].CancelType == "C")
                        {
                            oJV.JournalEntries.Lines.AdditionalReference = VouchersToPost[i].CancelNum;
                            oJV.JournalEntries.Lines.UserFields.Fields.Item("U_InvEntry").Value = VouchersToPost[i].CancelEntry;
                        }
                        else
                        {
                            oJV.JournalEntries.Lines.AdditionalReference = VouchersToPost[i].MemoNum;
                            oJV.JournalEntries.Lines.UserFields.Fields.Item("U_MemoEntry").Value = VouchersToPost[i].MemoEntry;
                        }
                    }

                    oJV.JournalEntries.Lines.Reference1 = VouchersToPost[i].StudentID;
                    oJV.JournalEntries.Lines.Reference2 = VouchersToPost[i].RegNo;

                    switch (Config.MajorDim)
                    {

                        case "1": oJV.JournalEntries.Lines.CostingCode = VouchersToPost[i].Major; break;
                        case "2": oJV.JournalEntries.Lines.CostingCode2 = VouchersToPost[i].Major; break;
                        case "3": oJV.JournalEntries.Lines.CostingCode3 = VouchersToPost[i].Major; break;
                        case "4": oJV.JournalEntries.Lines.CostingCode4 = VouchersToPost[i].Major; break;
                        case "5": oJV.JournalEntries.Lines.CostingCode5 = VouchersToPost[i].Major; break;
                    }
                    switch (Config.ScholarDim)
                    {

                        case "1": oJV.JournalEntries.Lines.CostingCode = VouchersToPost[i].Scholarship; break;
                        case "2": oJV.JournalEntries.Lines.CostingCode2 = VouchersToPost[i].Scholarship; break;
                        case "3": oJV.JournalEntries.Lines.CostingCode3 = VouchersToPost[i].Scholarship; break;
                        case "4": oJV.JournalEntries.Lines.CostingCode4 = VouchersToPost[i].Scholarship; break;
                        case "5": oJV.JournalEntries.Lines.CostingCode5 = VouchersToPost[i].Scholarship; break;
                    }
                    oJV.JournalEntries.Lines.Add();
                    oJV.JournalEntries.Lines.AccountCode = VouchersToPost[i].CreditAccount;
                    oJV.JournalEntries.Lines.Credit = Convert.ToDouble(VouchersToPost[i].Amount);
                    if (VouchersToPost[i].DocType == "R")
                    {
                        oJV.JournalEntries.Lines.AdditionalReference = VouchersToPost[i].InvNum;
                        oJV.JournalEntries.Lines.UserFields.Fields.Item("U_InvEntry").Value = VouchersToPost[i].InvEntry;
                    }
                    else
                    {
                        if (VouchersToPost[i].CancelType == "C")
                        {
                            oJV.JournalEntries.Lines.AdditionalReference = VouchersToPost[i].CancelNum;
                            oJV.JournalEntries.Lines.UserFields.Fields.Item("U_InvEntry").Value = VouchersToPost[i].CancelEntry;
                        }
                        else
                        {
                            oJV.JournalEntries.Lines.AdditionalReference = VouchersToPost[i].MemoNum;
                            oJV.JournalEntries.Lines.UserFields.Fields.Item("U_MemoEntry").Value = VouchersToPost[i].MemoEntry;
                        }
                    }
                    oJV.JournalEntries.Lines.Reference1 = VouchersToPost[i].StudentID;
                    oJV.JournalEntries.Lines.Reference2 = VouchersToPost[i].RegNo;

                    switch (Config.MajorDim)
                    {
                        case "1": oJV.JournalEntries.Lines.CostingCode = VouchersToPost[i].Major; break;
                        case "2": oJV.JournalEntries.Lines.CostingCode2 = VouchersToPost[i].Major; break;
                        case "3": oJV.JournalEntries.Lines.CostingCode3 = VouchersToPost[i].Major; break;
                        case "4": oJV.JournalEntries.Lines.CostingCode4 = VouchersToPost[i].Major; break;
                        case "5": oJV.JournalEntries.Lines.CostingCode5 = VouchersToPost[i].Major; break;
                    }
                    switch (Config.ScholarDim)
                    {

                        case "1": oJV.JournalEntries.Lines.CostingCode = VouchersToPost[i].Scholarship; break;
                        case "2": oJV.JournalEntries.Lines.CostingCode2 = VouchersToPost[i].Scholarship; break;
                        case "3": oJV.JournalEntries.Lines.CostingCode3 = VouchersToPost[i].Scholarship; break;
                        case "4": oJV.JournalEntries.Lines.CostingCode4 = VouchersToPost[i].Scholarship; break;
                        case "5": oJV.JournalEntries.Lines.CostingCode5 = VouchersToPost[i].Scholarship; break;
                    }
                }
                if (oJV.Add() != 0)
                {
                    string ErrorMsg = Global.Comp_DI.GetLastErrorDescription();
                    ErrorList.Add(new List<string>());
                    ErrorList[ErrorList.Count - 1].Add("2:" + VouchersToPost[0].StudentID);
                    ErrorList[ErrorList.Count - 1].Add(VouchersToPost[0].StudentID + ":" + ErrorMsg);
                    Global.SetMessage(ErrorMsg, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                    if (Global.Comp_DI.InTransaction)
                    {
                        Global.Comp_DI.EndTransaction(BoWfTransOpt.wf_RollBack);
                    }

                }
                else
                {
                    string NewEntry = Global.Comp_DI.GetNewObjectKey();
                    DocEntries.Add(new List<string>());
                    int Index = NewEntry.LastIndexOf("	");
                    DocEntries[DocEntries.Count - 1].Add("28:" + NewEntry.Substring(0, Index));
                    DocEntries[DocEntries.Count - 1].Add(NewEntry.Substring(0, Index));
                }
            }
            catch (Exception ex)
            {
                ErrorList.Add(new List<string>());
                ErrorList[ErrorList.Count - 1].Add("2:" + VouchersToPost[0].StudentID);
                ErrorList[ErrorList.Count - 1].Add(VouchersToPost[0].StudentID + ":" + ex.Message);

                Global.SetMessage(ex.Message, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                if (Global.Comp_DI.InTransaction)
                {
                    Global.Comp_DI.EndTransaction(BoWfTransOpt.wf_RollBack);
                }
            }
            finally
            {
                VouchersToPost.Clear();
                oJV = null;
            }
        }
        private void UpdateFetchingOfRegistration(string GenNo)
        {
            try
            {
                Recordset oRec = Global.Comp_DI.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
                oRec.DoQuery("Update T0 set T0.\"U_FeeCreated\" = 'Y' from \"@OSRG\" T0 inner join \"@ING1\" T1 on T0.\"DocEntry\" = T1.\"U_RegNo\" inner join \"@OING\" T2 on T2.\"DocEntry\" = T1.\"DocEntry\"  where T2.\"U_DocType\" = 'R' and T1.\"U_Invoiced\" ='Y' and T1.\"DocEntry\" = " + GenNo);
            }
            catch (Exception ex)
            {
                Global.SetMessage(ex.Message, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
        private void UpdateVouchers(string GenNo, string DocumentType)
        {
            try
            {
                Recordset oRec = Global.Comp_DI.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
                if (DocumentType == "R")
                {
                    oRec.DoQuery("Update T0 set T0.\"U_IsVoucher\" = 'Y' from \"@ING1\" T0 inner join BTF1 T1 on T0.\"U_StudentCode\" = T1.\"Ref1\" and T0.\"U_InvNum\" = T1.\"Ref3Line\" and T0.\"U_InvEntry\" = T1.\"U_InvEntry\" inner join OBTF T2 on T2.\"BatchNum\" = T1.\"BatchNum\" where T2.\"Ref1\" ='" + GenNo + "'");
                }
                else
                {
                    oRec.DoQuery("Update T0 set T0.\"U_IsVoucher\" = 'Y',T0.\"U_DocCancel\" = 'Y' from \"@ING1\" T0 inner join BTF1 T1 on T0.\"U_StudentCode\" = T1.\"Ref1\" and T0.\"U_MemoNum\" = T1.\"Ref3Line\" and T0.\"U_MemoEntry\" = T1.\"U_MemoEntry\" inner join OBTF T2 on T2.\"BatchNum\" = T1.\"BatchNum\" where T2.\"U_Reversal\" = 'Y' and T2.\"Ref1\" ='" + GenNo + "'");
                    oRec.DoQuery("Update T0 set T0.\"U_IsVoucher\" = 'Y',T0.\"U_DocCancel\" = 'Y' from \"@ING1\" T0 inner join BTF1 T1 on T0.\"U_StudentCode\" = T1.\"Ref1\" and T0.\"U_CancelNum\" = T1.\"Ref3Line\" and T0.\"U_CancelEntry\" = T1.\"U_InvEntry\" inner join OBTF T2 on T2.\"BatchNum\" = T1.\"BatchNum\" where T2.\"U_Reversal\" = 'Y' and T2.\"Ref1\" ='" + GenNo + "'");
                }
                //oRec.DoQuery("Update T0 set T0.\"U_TrgtEntry\" = T1.\"DocEntry\",T0.\"U_TrgtNum\" = T1.\"DocNum\", T0.\"U_Invoiced\" = 'Y' from \"@OSRG\" T0 inner join OINV T1 on T0.\"DocEntry\" = T1.\"U_RegNo\" where T1.\"U_GenNo\" = " + GenNo);
            }
            catch (Exception ex)
            {
                Global.SetMessage(ex.Message, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
        private void PostRevenue(string GenNo)
        {
            ErrorList.Clear();
            DocEntries.Clear();
            Global.SetMessage("Revenue Posting Initiated", SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
            Recordset oRec = Global.Comp_DI.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
            try
            {
                string format = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
                string Query = @"Select T0.""U_StudentCode"",T0.""U_StudentName"",T0.""U_RegNo"",T0.""LineId"",T0.""DocEntry"" as ""GenNo"",T0.""U_Scholarship"",
+coalesce(T2.""GrosProfit"",0)+coalesce(ABS(T2.""TotalExpns""),0) as ""DocTotal"",T2.""DocEntry"" as ""InvEntry""," + (Global.Comp_DI.DbServerType == BoDataServerTypes.dst_HANADB ? @"TO_VARCHAR(T2.""DocDate"",'" + Global._dateFormat.ToUpper() + @"')" : @"FORMAT(T2.""DocDate"",'" + Global._dateFormat + @"')") + @" as ""InvoiceDocDate"",T2.""DocNum"" as ""InvNum"",T1.""U_College"",T0.""U_CancelType"",T4.""U_DocType"",T2.""CANCELED"" as ""InvStatus"",coalesce((Select MAX(T10.""DocEntry"") from RIN1 T10 where T10.""BaseEntry"" = T0.""U_InvEntry""),0) as ""MemoEntry"",coalesce((Select MAX(T11.""DocNum"") from RIN1 T10 inner join ORIN T11 on T11.""DocEntry"" = T10.""DocEntry"" where T10.""BaseEntry"" = T0.""U_InvEntry""),0) as ""MemoNum"",T0.""U_CancelNum"",T0.""U_CancelEntry"",
T0.""U_InvEntry"",T0.""U_InvNum"",T0.""U_Major""," + (Global.Comp_DI.DbServerType == BoDataServerTypes.dst_HANADB ? @"TO_VARCHAR(T1.""U_StartDate"",'" + Global._dateFormat.ToUpper() + @"')" : @"FORMAT(T1.""U_StartDate"",'" + Global._dateFormat + @"')") + @" as ""FromDate""," + (Global.Comp_DI.DbServerType == BoDataServerTypes.dst_HANADB ? @"TO_VARCHAR(T1.""U_EndDate"",'" + Global._dateFormat.ToUpper() + "')" : @"FORMAT(T1.""U_EndDate"",'" + Global._dateFormat + @"')") + @" as ""ToDate"",
T5.""DocEntry"" as ""MemoEntry"",T5.""DocNum"" as ""MemoNum""," + (Global.Comp_DI.DbServerType == BoDataServerTypes.dst_HANADB ? @"TO_VARCHAR(T5.""DocDate"",'" + Global._dateFormat.ToUpper() + @"')" : @"FORMAT(T5.""DocDate"",'" + Global._dateFormat + @"')") + @" as ""MemoDocDate""," + (Global.Comp_DI.DbServerType == BoDataServerTypes.dst_HANADB ? @"TO_VARCHAR(T4.""U_DocDate"",'" + Global._dateFormat.ToUpper() + @"')" : @"FORMAT(T4.""U_DocDate"",'" + Global._dateFormat + @"')") + @" as ""CancelDate""
from ""@ING1"" T0 
inner join ""@OSRG"" T1 on T0.""U_StudentCode"" = T1.""U_StudentCode"" and T0.""U_RegNo"" = T1.""DocEntry""
inner join OINV T2 on T2.""DocEntry"" =     T0.""U_InvEntry"" and T2.""CardCode"" = T0.""U_StudentCode""
inner join OCRD T3 on T2.""CardCode"" = T3.""CardCode""
inner join ""@OING"" T4 on T4.""DocEntry"" = T0.""DocEntry""
left join ORIN T5 on T5.""DocEntry"" = T0.""U_MemoEntry"" and T5.""U_GenNo"" = T0.""DocEntry"" and T5.""U_GenLine"" = T0.""LineId""
where ((T4.""U_DocType"" = 'R' and coalesce(T0.""U_Invoiced"",'N') = 'Y') OR  (T4.""U_DocType"" = 'C' and ((coalesce(T0.""U_MemoEntry"",0) <> 0 and T0.""U_CancelType"" in ('R','M')) or (coalesce(T0.""U_MemoEntry"",0) = 0 and T0.""U_CancelType"" in ('C','N'))))) and coalesce(T0.""U_IsVoucher"",'N') = 'N' and T0.""DocEntry""= " + GenNo;

                //                string Query = @"Select T0.""U_StudentID"",T0.""U_StudentName"",T0.""U_RegNo"",T0.""LineId"",T0.""DocEntry"" as ""GenNo"",
                //T2.""GrosProfit"" as ""DocTotal"",T2.""DocEntry"" as ""InvEntry""," + (Global.Comp_DI.DbServerType == BoDataServerTypes.dst_HANADB ? @"TO_VARCHAR(T2.""DocDate"",'" + Global._dateFormat.ToUpper() + @"')" : @"FORMAT(T2.""DocDate"",'" + Global._dateFormat + @"')") + @" as ""InvoiceDocDate"",T2.""DocNum"" as ""InvNum"",T3.""U_School"",T3.""U_Level"",T0.""U_CancelType"",T4.""U_DocType"",T2.""CANCELED"" as ""InvStatus"",coalesce((Select MAX(T10.""DocEntry"") from RIN1 T10 where T10.""BaseEntry"" = T0.""U_InvEntry""),0) as ""MemoEntry"",coalesce((Select MAX(T11.""DocNum"") from RIN1 T10 inner join ORIN T11 on T11.""DocEntry"" = T10.""DocEntry"" where T10.""BaseEntry"" = T0.""U_InvEntry""),0) as ""MemoNum"",T0.""U_CancelNum"",T0.""U_CancelEntry"",
                //T0.""U_InvEntry"",T0.""U_InvNum"",T0.""U_ProgramCode""," + (Global.Comp_DI.DbServerType == BoDataServerTypes.dst_HANADB ? @"TO_VARCHAR(T1.""U_StartDate"",'" + Global._dateFormat.ToUpper() + @"')" : @"FORMAT(T1.""U_StartDate"",'" + Global._dateFormat + @"')") + @" as ""FromDate""," + (Global.Comp_DI.DbServerType == BoDataServerTypes.dst_HANADB ? @"TO_VARCHAR(T1.""U_EndDate"",'" + Global._dateFormat.ToUpper() + "')" : @"FORMAT(T1.""U_EndDate"",'" + Global._dateFormat + @"')") + @" as ""ToDate"",
                //T5.""DocEntry"" as ""MemoEntry"",T5.""DocNum"" as ""MemoNum""," + (Global.Comp_DI.DbServerType == BoDataServerTypes.dst_HANADB ? @"TO_VARCHAR(T5.""DocDate"",'" + Global._dateFormat.ToUpper() + @"')" : @"FORMAT(T5.""DocDate"",'" + Global._dateFormat + @"')") + @" as ""MemoDocDate""
                //from ""@ING1"" T0 
                //inner join ""@OSRG"" T1 on T0.""U_StudentID"" = T1.""U_StudentID"" and T0.""U_RegNo"" = T1.""DocEntry""
                //inner join OINV T2 on T2.""DocEntry"" = T0.""U_InvEntry"" and T2.""CardCode"" = T0.""U_StudentID""
                //inner join OCRD T3 on T2.""CardCode"" = T3.""CardCode""
                //inner join ""@OING"" T4 on T4.""DocEntry"" = T0.""DocEntry""
                //left join ORIN T5 on T5.""DocEntry"" = T0.""U_MemoEntry"" and T5.""U_GenNo"" = T0.""DocEntry"" and T5.""U_GenLine"" = T0.""LineId""
                //where ((T4.""U_DocType"" = 'R' and coalesce(T0.""U_Invoiced"",'N') = 'Y') OR  (T4.""U_DocType"" = 'C' and coalesce(T0.""U_MemoEntry"",0) <> 0)) and coalesce(T0.""U_IsVoucher"",'N') = 'N' and T0.""DocEntry""= " + GenNo;

                //                Query = @"Select T0.""U_StudentID"",T0.""U_StudentName"",T0.""U_RegNo"",T0.""LineId"",T0.""DocEntry"" as ""GenNo"",
                //T2.""GrosProfit"" as ""DocTotal"",T2.""DocEntry"" as ""InvEntry"",T2.""DocNum"" as ""InvNum"",
                //T0.""U_InvEntry"",T0.""U_InvNum"",T0.""U_ProgramCode""," + (Global.Comp_DI.DbServerType == BoDataServerTypes.dst_HANADB ? "TO_VARCHAR" : "FORMAT") + @"(T1.""U_StartDate"",'yyyy-MM-dd 00:00:00') as ""FromDate""," + (Global.Comp_DI.DbServerType == BoDataServerTypes.dst_HANADB ? "TO_VARCHAR" : "FORMAT") + @"(T1.""U_EndDate"",'yyyy-MM-dd 00:00:00') as ""ToDate"" 
                //from ""@ING1"" T0 
                //inner join ""@OSRG"" T1 on T0.""U_StudentID"" = T1.""U_StudentID"" and T0.""U_RegNo"" = T1.""DocEntry""
                //inner join OINV T2 on T2.""DocEntry"" = T0.""U_InvEntry"" and T2.""CardCode"" = T0.""U_StudentID"" and T2.""CANCELED"" = 'N' where coalesce(T0.""U_IsVoucher"",'N') = 'N' and T0.""DocEntry""= " + GenNo;
                oRec.DoQuery(Query);
                Global.SetMessage("Students Fetched for Vouchers.Total " + oRec.RecordCount.ToString(), SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                //string DebitAccount = oRec.Fields.Item("UnearnedAccount").Value.ToString();
                //string CreditAccount = Config.RevenueMapping[DebitAccount];
                string DocTypeInTransaction = "";

                mymonths.Clear();
                while (!oRec.EoF)
                {
                    try
                    {
                        DocTypeInTransaction = oRec.Fields.Item("U_DocType").Value.ToString();
                        if (oRec.Fields.Item("U_CancelType").Value.ToString() == "N")
                        {
                            oRec.MoveNext();
                            continue;
                        }

                        string Key = "";
                        string DebitAccount = "";
                        string CreditAccount = "";
                        try
                        {
                            Key = DocType + "-" + oRec.Fields.Item("U_College").Value.ToString() + "-" + oRec.Fields.Item("U_Scholarship").Value.ToString();
                            DebitAccount = Config.DebitMapping[Key];
                            CreditAccount = Config.CreditMapping[Key];
                        }
                        catch (Exception ex)
                        {
                            string ErrorMsg = "Revenue Mapping not found for " + Key;
                            ErrorList.Add(new List<string>());
                            ErrorList[ErrorList.Count - 1].Add("2:" + oRec.Fields.Item("U_StudentCode").Value.ToString());
                            ErrorList[ErrorList.Count - 1].Add(oRec.Fields.Item("U_StudentCode").Value.ToString() + ":" + ErrorMsg);
                            oRec.MoveNext();
                            continue;
                        }

                        decimal DocTotal = 0;
                        DateTime FromDate = new DateTime();
                        DateTime ToDate = new DateTime();
                        DateTime DocDate = new DateTime();
                        DateTime CancelDate = new DateTime();
                        decimal.TryParse(oRec.Fields.Item("DocTotal").Value.ToString(), out DocTotal);

                        if (oRec.Fields.Item("U_CancelType").Value.ToString() == "M")
                        {
                            decimal.TryParse(oRec.Fields.Item("U_ManualAmount").Value.ToString(), out DocTotal);
                        }
                        //int Year = Convert.ToInt16(oRec.Fields.Item("FromDate").Value.ToString().Substring(0, 4));
                        //int Month = Convert.ToInt16(oRec.Fields.Item("FromDate").Value.ToString().Substring(4, 2));
                        //int Day = Convert.ToInt16(oRec.Fields.Item("FromDate").Value.ToString().Substring(6, 2));
                        //FromDate = new DateTime(Year, Month, Day);
                        //Year = Convert.ToInt16(oRec.Fields.Item("ToDate").Value.ToString().Substring(0, 4));
                        //Month = Convert.ToInt16(oRec.Fields.Item("ToDate").Value.ToString().Substring(4, 2));
                        //Day = Convert.ToInt16(oRec.Fields.Item("ToDate").Value.ToString().Substring(6, 2));
                        //ToDate = new DateTime(Year, Day, Month);

                        string[] formats = { Global._dateFormat };
                        //DateTime.TryParseExact(input, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date)

                        DateTime.TryParseExact(oRec.Fields.Item("FromDate").Value.ToString(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out FromDate);
                        DateTime.TryParseExact(oRec.Fields.Item("ToDate").Value.ToString(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out ToDate);
                        DateTime.TryParseExact(oRec.Fields.Item("InvoiceDocDate").Value.ToString(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DocDate);
                        DateTime.TryParseExact(oRec.Fields.Item("CancelDate").Value.ToString(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out CancelDate);

                        if (oRec.Fields.Item("U_CancelType").Value.ToString() == "R" || oRec.Fields.Item("U_CancelType").Value.ToString() == "M")
                        {
                            DateTime.TryParseExact(oRec.Fields.Item("MemoDocDate").Value.ToString(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DocDate);
                        }

                        if (FromDate < DocDate)
                        {
                            FromDate = DocDate;
                        }
                        if (CancelDate > FromDate && oRec.Fields.Item("U_CancelType").Value.ToString() == "C")
                        {
                            FromDate = CancelDate;
                        }
                        ProrateRevenueAlgorithm(DocTotal, FromDate, ToDate, oRec.Fields.Item("U_StudentCode").Value.ToString(), oRec.Fields.Item("U_RegNo").Value.ToString(), oRec.Fields.Item("GenNo").Value.ToString(), oRec.Fields.Item("InvEntry").Value.ToString(), oRec.Fields.Item("InvNum").Value.ToString(), DebitAccount, CreditAccount, oRec.Fields.Item("U_Major").Value.ToString(), oRec.Fields.Item("U_Scholarship").Value.ToString(), oRec.Fields.Item("MemoEntry").Value.ToString(), oRec.Fields.Item("MemoNum").Value.ToString(), oRec.Fields.Item("U_DocType").Value.ToString(), oRec.Fields.Item("U_CancelType").Value.ToString(), oRec.Fields.Item("U_CancelEntry").Value.ToString(), oRec.Fields.Item("U_CancelNum").Value.ToString());
                        oRec.MoveNext();
                    }
                    catch (Exception ex)
                    {
                        Global.SetMessage(ex.Message + " Method: Post Revenue", SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                        oRec.MoveNext();
                    }
                }
                List<DateTime> UniqueDates = mymonths.Select(x => x.Date).Distinct().ToList();

                Global.Comp_DI.StartTransaction();
                bool HasFailed = false;
                for (int i = 0; i < UniqueDates.Count; i++)
                {
                    Global.SetMessage("Initiating Voucher Creation", BoStatusBarMessageType.smt_Warning);

                    CreateNewVoucherPerDate(UniqueDates[i], GenNo);
                    if (!Global.Comp_DI.InTransaction)
                    {
                        HasFailed = true;
                        break;
                    }
                }
                if (Global.Comp_DI.InTransaction && HasFailed)
                {
                    Global.Comp_DI.EndTransaction(BoWfTransOpt.wf_RollBack);
                }
                if (Global.Comp_DI.InTransaction && !HasFailed)
                {
                    Global.Comp_DI.EndTransaction(BoWfTransOpt.wf_Commit);
                }
                if (DocEntries.Count > 0 && !HasFailed)
                {
                    UpdateVouchers(GenNo, DocTypeInTransaction);
                    Global.SendMessage("Voucher(s) Success Notification", "Following Voucher(s) Posted Successfully", new string[] { Global.Comp_DI.UserName }, DocEntries, "28", "Voucher(s)");
                }
                if (ErrorList.Count > 0)
                {
                    Global.SendMessage("Voucher(s) Failure Notification", "Following Student(s) Vouchers Failed to Post", new string[] { Global.Comp_DI.UserName }, ErrorList, "2", "Student(s)");
                }
            }
            catch (Exception ex)
            {
                if (Global.Comp_DI.InTransaction)
                {
                    Global.Comp_DI.EndTransaction(BoWfTransOpt.wf_RollBack);
                }

                Global.SetMessage(ex.Message, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                if (ErrorList.Count > 0)
                {
                    Global.SendMessage("Voucher(s) Failure Notification", "Following Student(s) Vouchers Failed to Post", new string[] { Global.Comp_DI.UserName }, ErrorList, "2", "Student(s)");
                }
            }
            finally
            {
                DocEntries.Clear();
                ErrorList.Clear();
                mymonths.Clear();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
        private void UpdateRegistration(string GenNo, string DocumentType)
        {
            try
            {
                Recordset oRec = Global.Comp_DI.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
                if (DocumentType == "R")
                {
                    oRec.DoQuery("Update T0 set T0.\"U_TrgtEntry\" = T1.\"DocEntry\",T0.\"U_TrgtNum\" = T1.\"DocNum\", T0.\"U_Invoiced\" = 'Y' from \"@OSRG\" T0 inner join OINV T1 on T0.\"DocEntry\" = T1.\"U_RegNo\" and T1.\"U_RegType\" = 'N' where  T1.\"U_GenNo\" = " + GenNo);
                    oRec.DoQuery("Update T0 set T0.\"U_RTrgtEntry\" = T1.\"DocEntry\",T0.\"U_RTrgtNum\" = T1.\"DocNum\", T0.\"U_RInvoiced\" = 'Y' from \"@OSRG\" T0 inner join OINV T1 on T0.\"DocEntry\" = T1.\"U_RegNo\"  and T1.\"U_RegType\" = 'Y' where  T1.\"U_GenNo\" = " + GenNo);
                    oRec.DoQuery("Update T0 set T0.\"U_InvEntry\" = T1.\"DocEntry\",T0.\"U_InvNum\" = T1.\"DocNum\",T0.\"U_Invoiced\" = 'Y' from \"@ING1\" T0 inner join OINV T1 on T0.\"U_RegNo\" = T1.\"U_RegNo\" and T0.\"LineId\" = T1.\"U_GenLine\" inner join \"@OING\" T2 on T2.\"DocEntry\" = T0.\"DocEntry\" where T2.\"U_DocType\"= 'R' and T1.\"U_GenNo\" =" + GenNo);
                }
                else
                {
                    oRec.DoQuery("Update T0 set T0.\"U_Cancelled\" = 'Y' from \"@ING1\" T0  inner join \"@ING1\" T1 on T0.\"DocEntry\" = T1.\"U_BaseDoc\" and T0.\"LineId\" = T1.\"U_BaseLine\" where T1.\"DocEntry\" ='" + GenNo + "'");
                    oRec.DoQuery("Update T0 set T0.\"U_MemoEntry\" = T1.\"DocEntry\",T0.\"U_MemoNum\" = T1.\"DocNum\",T0.\"U_DocCancel\" = 'Y' from \"@ING1\" T0 inner join ORIN T1 on T0.\"DocEntry\" = T1.\"U_GenNo\" and T0.\"LineId\" = T1.\"U_GenLine\" inner join \"@OING\" T2 on T0.\"DocEntry\" = T2.\"DocEntry\" where T2.\"U_DocType\"= 'C' and T0.\"U_CancelType\" in ('R','N','M') and T1.\"U_GenNo\" =" + GenNo);
                    oRec.DoQuery("Update T0 set T0.\"U_CancelEntry\" = T1.\"DocEntry\",T0.\"U_CancelNum\" = T1.\"DocNum\" from \"@ING1\" T0 inner join OINV T1 on T0.\"U_RegNo\" = T1.\"U_RegNo\" and T1.\"U_GenNo\" = T0.\"U_BaseDoc\" and T0.\"U_BaseLine\" = T1.\"U_GenLine\" inner join \"@OING\" T2 on T2.\"DocEntry\" = T0.\"DocEntry\" where T2.\"U_DocType\"= 'C' and T0.\"U_CancelType\" = 'C' and T1.\"CANCELED\" = 'C' and T0.\"DocEntry\" =" + GenNo);
                }

            }
            catch (Exception ex)
            {
                Global.SetMessage(ex.Message, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
        private bool DeleteRows()
        {
            StoppedByManualRefund = false;
            bool ReturnVal = true;
            for (int i = Matrix0.RowCount - 1; i >= 0; i--)
            {
                if (ING1.GetValue("U_Select", i).ToString() == "N")
                {
                    ING1.RemoveRecord(i);
                    ReturnVal = true;
                    continue;
                }
                if (ING1.GetValue("U_CancelType", i).ToString() == "M" && (ING1.GetValue("U_ManualAmount", i).ToString() == "0" || ING1.GetValue("U_AcctCode", i).ToString() == ""))
                {

                    //Global.SetMessage("Please specify Account Code and Amount for Manual Memo Posting", SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                    StoppedByManualRefund = true;
                    ReturnVal = false;
                    Matrix0.SetCellFocus(i + 1, 23);
                    Global.myApi.StatusBarEvent += MyApi_StatusBarEvent;
                    break;
                }
                if (ING1.GetValue("U_CancelType", i).ToString() == "M" && (ING1.GetValue("U_ManualAmount", i).ToString() == "0" || ING1.GetValue("U_AcctCode", i).ToString() == ""))
                {

                    //Global.SetMessage("Please specify Account Code and Amount for Manual Memo Posting", SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                    StoppedByManualRefund = true;
                    ReturnVal = false;
                    Matrix0.SetCellFocus(i + 1, 23);
                    Global.myApi.StatusBarEvent += MyApi_StatusBarEvent;
                    break;
                }

            }
            return ReturnVal;
        }
        private void btnLoad_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {

            if (cmbDocType.Selected.Value == "R")
            {
                try
                {
                    if (UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
                    {
                        FillMatrixForRegisteration("Y");
                    }
                }
                catch (Exception ex)
                {

                    Global.SetMessage(ex.Message, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                    UIAPIRawForm.Freeze(false);
                }
            }
            else if (cmbDocType.Selected.Value == "C")
            {
                try
                {
                    if (UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
                    {
                        FillMatrixForCancellation("Y");
                    }
                }
                catch (Exception ex)
                {

                    Global.SetMessage(ex.Message, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                    UIAPIRawForm.Freeze(false);
                }
            }
            if (DocType == "C")
            {
                Matrix0.Columns.Item("Cancel").Editable = true;
                //Matrix0.Columns.Item("ManPrice").Editable = false;
                //Matrix0.Columns.Item("AppPrice").Editable = false;
                Matrix0.Columns.Item("HDiscount").Editable = false;
                Matrix0.Columns.Item("Freight1").Editable = false;
                Matrix0.Columns.Item("Freight2").Editable = false;
                Matrix0.Columns.Item("Freight3").Editable = false;
                Matrix0.Columns.Item("Freight4").Editable = false;
                Matrix0.Columns.Item("Freight5").Editable = false;
                Matrix0.Columns.Item("Freight6").Editable = false;
                Matrix0.Columns.Item("Freight7").Editable = false;
                Matrix0.Columns.Item("Freight8").Editable = false;
                Matrix0.Columns.Item("Freight9").Editable = false;
                Matrix0.Columns.Item("Freight10").Editable = false;
            }
            else
            {
                Matrix0.Columns.Item("Cancel").Editable = false;
                Matrix0.Columns.Item("MAccount").Editable = false;
                Matrix0.Columns.Item("MAmount").Editable = false;
                //Matrix0.Columns.Item("ManPrice").Editable = true;
                //Matrix0.Columns.Item("AppPrice").Editable = true;
                Matrix0.Columns.Item("Notes").Editable = true;
                Matrix0.Columns.Item("HDiscount").Editable = true;
                Matrix0.Columns.Item("Freight1").Editable = true;
                Matrix0.Columns.Item("Freight2").Editable = true;
                Matrix0.Columns.Item("Freight3").Editable = true;
                Matrix0.Columns.Item("Freight4").Editable = true;
                Matrix0.Columns.Item("Freight5").Editable = true;
                Matrix0.Columns.Item("Freight6").Editable = true;
                Matrix0.Columns.Item("Freight7").Editable = true;
                Matrix0.Columns.Item("Freight8").Editable = true;
                Matrix0.Columns.Item("Freight9").Editable = true;
                Matrix0.Columns.Item("Freight10").Editable = true;


            }
            Matrix0.Item.Refresh();
        }
        private void txtFromDate_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(false);
            if (Matrix0.RowCount > 0 && UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
            {
                Global.SetMessage("Please clear currently fetched registrations before changing start/end date", SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                BubbleEvent = false;
            }
        }
        private void txtFromDate_KeyDownBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(false);
            if (Matrix0.RowCount > 0 && UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
            {
                Global.SetMessage("Please clear currently fetched registrations before changing start/end date", SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                BubbleEvent = false;
            }
        }
        private void txtToDate_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(false);
            if (Matrix0.RowCount > 0 && UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
            {
                Global.SetMessage("Please clear currently fetched registrations before changing start/end date", SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                BubbleEvent = false;
            }

        }
        private void txtToDate_KeyDownBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(false);
            if (Matrix0.RowCount > 0 && UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
            {
                Global.SetMessage("Please clear currently fetched registrations before changing start/end date", SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                BubbleEvent = false;
            }

        }
        private void txtMajor_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(false);


        }
        private void txtMajor_KeyDownBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(false);


        }
        private void txtDocEntry_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(true);


        }
        private void txtDocEntry_KeyDownBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(true);
        }
        private void txtDocDate_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(false);
        }
        private void txtDocDate_KeyDownBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(false);
        }
        private void btnLoad_PressedBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateBeforeFind();
        }
        private void txtCreateDate_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(true);

        }
        private void txtCreateDate_KeyDownBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(true);
        }
        private void btnClear_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {

            if (Matrix0.RowCount > 0 && UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
            {
                UIAPIRawForm.Freeze(true);
                ING1.Clear();
                Matrix0.Clear();
                UIAPIRawForm.Freeze(false);
            }

        }
        private void Matrix0_LinkPressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            if (pVal.ColUID == "RegNo" && pVal.Row > -1)
            {
                string DocEntry = UIAPIRawForm.DataSources.DBDataSources.Item("@ING1").GetValue("U_RegNo", pVal.Row - 1);
                Registration active = new Registration();
                active.UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE;
                active.txtDocEntry.Value = DocEntry;
                active.btnSave.Item.Click(0);
                active.Show();
            }
            else if (pVal.ColUID == "BaseDoc" && pVal.Row > -1)
            {
                string DocEntry = UIAPIRawForm.DataSources.DBDataSources.Item("@ING1").GetValue("U_BaseDoc", pVal.Row - 1);
                GenerateInvoices active = new GenerateInvoices(cmbDocType.Selected.Value);
                active.UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE;
                active.txtDocEntry.Value = DocEntry;
                active.btnSave.Item.Click(0);
                active.Show();
                active.cmbDocType.Select("R", SAPbouiCOM.BoSearchKey.psk_ByValue);
            }
        }
        private void btnSelectAll_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            if (UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
            {
                if (DocType == "R")
                {
                    FillMatrixForRegisteration("Y");
                }
                else
                {
                    FillMatrixForCancellation("Y");
                }

            }

        }
        private void btnDeSelect_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            if (UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
            {
                if (DocType == "R")
                {
                    FillMatrixForRegisteration("N");
                }
                else
                {
                    FillMatrixForCancellation("N");
                }
            }
        }
        private void Form_DataAddAfter(ref SAPbouiCOM.BusinessObjectInfo pVal)
        {
            if (pVal.ActionSuccess && !pVal.BeforeAction)
            {
                Config.LoadConfig();
                string objKey = pVal.ObjectKey;
                string DocEntry = objKey.Substring(objKey.IndexOf("Entry>") + "Entry>".Length).Split("<\\Doc".ToCharArray()).First();
                Global.SendMessage("Document Posting", "Automated posting(s) initialized. Do not close SAP Business One client till the next notification", new string[] { Global.Comp_DI.UserName }, new List<List<string>>() { }, "", "");
                InitiateDocumentsPosting(DocEntry);
                ConfigureCombos();
            }
        }
        private void Form_DataAddBefore(ref SAPbouiCOM.BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            SecondIteration = false;
            if (DeleteRows())
            {
                BubbleEvent = true;
                int ret = 0;
                if (DocType == "R")
                {
                    ret = Global.myApi.MessageBox("Are you sure you want to save this document?\nNote: Saving this document will initiate Invoice(s) and Voucher(s) process in the background. Please keep SAP running until you receive messages to notify completion.", 1, "Yes", "No");

                }
                else
                {
                    ret = Global.myApi.MessageBox("Are you sure you want to save this document?\nNote: Saving this document will initiate Refund(s) and Cancellation(s) process in the background. Please keep SAP running until you receive messages to notify completion", 1, "Yes", "No");
                }
                if (ret != 1)
                {
                    BubbleEvent = false;
                    Global.myApi.StatusBarEvent += MyApi_StatusBarEvent;
                }

            }
            else
            {
                BubbleEvent = false;
            }
        }
        bool StoppedByManualRefund = false;
        bool StoppedByWrongForm = false;
        string MessageToShow = "";
        private void MyApi_StatusBarEvent(string Text, SAPbouiCOM.BoStatusBarMessageType messageType)
        {
            if (messageType == SAPbouiCOM.BoStatusBarMessageType.smt_Error && Text == "Action stopped by add-on (UI_API -7780)  [Message 66000-152]" && StoppedByManualRefund && SecondIteration)
            {
                Global.SetMessage("Please specify Account Code and Amount for Manual Memo Posting", SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                Global.myApi.StatusBarEvent -= MyApi_StatusBarEvent;
            }
            else if (messageType == SAPbouiCOM.BoStatusBarMessageType.smt_Error && Text == "Action stopped by add-on (UI_API -7780)  [Message 66000-152]" && SecondIteration && !StoppedByWrongForm)
            {
                Global.SetMessage("Posting of Invoice(s) & Voucher(s) NOT initiated", SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                Global.myApi.StatusBarEvent -= MyApi_StatusBarEvent;
            }
            else if (messageType == SAPbouiCOM.BoStatusBarMessageType.smt_Error && Text == "Action stopped by add-on (UI_API -7780)  [Message 66000-152]" && StoppedByWrongForm && SecondIteration)
            {
                Global.SetMessage(MessageToShow, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                Global.myApi.StatusBarEvent -= MyApi_StatusBarEvent;
            }
            SecondIteration = true;
        }
        private void Form_DataLoadAfter(ref SAPbouiCOM.BusinessObjectInfo pVal)
        {
            Matrix0.Columns.Item("Select").Editable = false;
            Matrix0.Columns.Item("Cancel").Editable = false;
            Matrix0.Columns.Item("MAccount").Editable = false;
            Matrix0.Columns.Item("MAmount").Editable = false;
            Matrix0.Columns.Item("DiscPC").Editable = false;
            //Matrix0.Columns.Item("ManPrice").Editable = false;
            Matrix0.Columns.Item("HDiscount").Editable = false;
            Matrix0.Columns.Item("Notes").Editable = false;

            Matrix0.Columns.Item("Freight1").Editable = false;
            Matrix0.Columns.Item("Freight2").Editable = false;
            Matrix0.Columns.Item("Freight3").Editable = false;
            Matrix0.Columns.Item("Freight4").Editable = false;
            Matrix0.Columns.Item("Freight5").Editable = false;
            Matrix0.Columns.Item("Freight6").Editable = false;
            Matrix0.Columns.Item("Freight7").Editable = false;
            Matrix0.Columns.Item("Freight8").Editable = false;
            Matrix0.Columns.Item("Freight9").Editable = false;
            Matrix0.Columns.Item("Freight10").Editable = false;
            Matrix0.Columns.Item("PaidSum").Editable = false;
            Matrix0.Columns.Item("OpenSum").Editable = false;


            Matrix0.AutoResizeColumns();
        }
        private void Matrix0_LinkPressedBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (pVal.ColUID == "InvNum")
            {
                BubbleEvent = false;
                string InvoiceEntry = ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("InvEntry", pVal.Row)).Value;
                Global.myApi.OpenForm(SAPbouiCOM.BoFormObjectEnum.fo_Invoice, "", InvoiceEntry);
            }
            else if (pVal.ColUID == "MemoNum")
            {
                BubbleEvent = false;
                string InvoiceEntry = ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("MemoEntry", pVal.Row)).Value;
                Global.myApi.OpenForm(SAPbouiCOM.BoFormObjectEnum.fo_InvoiceCreditMemo, "", InvoiceEntry);
            }
            else if (pVal.ColUID == "CancelNum")
            {
                BubbleEvent = false;
                string InvoiceEntry = ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("CancelEnt", pVal.Row)).Value;
                Global.myApi.OpenForm(SAPbouiCOM.BoFormObjectEnum.fo_Invoice, "", InvoiceEntry);
            }
        }
        private void Form_DataUpdateAfter(ref SAPbouiCOM.BusinessObjectInfo pVal)
        {
            if (pVal.ActionSuccess && !pVal.BeforeAction)
            {
                string objKey = pVal.ObjectKey;
                string DocEntry = objKey.Substring(objKey.IndexOf("Entry>") + "Entry>".Length).Split("<\\Doc".ToCharArray()).First();
                Recordset oRec = Global.Comp_DI.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
                //                string Query = @"Select coalesce(count(T1.""DocEntry""),0) from ""@ING1"" T1 inner join ""@OING"" T0 on T0.""DocEntry"" = T1.""DocEntry"" 
                //where T1.""U_Select"" = 'Y' and(((coalesce(T1.""U_Invoiced"", 'N') = 'N' or coalesce(T1.""U_IsVoucher"", 'N') = 'N')
                //and coalesce(T1.""U_DocCancel"",'N') = 'N' and T0.""U_DocType"" = 'R' and coalesce(T1.""U_CancelType"",'-') = '-') or
                //((((coalesce(T1.""U_Invoiced"", 'N') = 'Y' and coalesce(T1.""U_DocCancel"", 'N') = 'N') or(coalesce(T1.""U_IsVoucher"", 'N') = 'N')) and T1.""U_CancelType"" in ('R', 'C') and T0.""U_DocType"" = 'C') or(T0.""U_DocType"" = 'C' and T1.""U_CancelType"" = 'N' and coalesce(T1.""U_MemoEntry"", 0) = 0))) and T1.""DocEntry"" = " + DocEntry;

                string Query = @"Select coalesce(count(T1.""DocEntry""),0) AS ""DocCount"" from ""@ING1"" T1 inner join ""@OING"" T0 on T0.""DocEntry"" = T1.""DocEntry"" 
where T1.""U_Select"" = 'Y' and T1.""DocEntry"" = " + DocEntry + @" and (((coalesce(T1.""U_Invoiced"", 'N') = 'N' or coalesce(T1.""U_IsVoucher"", 'N') = 'N')
and coalesce(T1.""U_DocCancel"",'N') = 'N' and T0.""U_DocType"" = '" + DocType + @"' and coalesce(T1.""U_CancelType"",'-') = '-') or
(coalesce(T1.""U_Invoiced"", 'N') = 'Y' and (coalesce(T1.""U_DocCancel"", 'N') = 'N' or coalesce(T1.""U_DocCancel"", 'N') = 'Y') and (coalesce(T1.""U_IsVoucher"", 'N') = 'N' and T1.""U_CancelType"" in ('R', 'C','M') or (coalesce(T1.""U_IsVoucher"", 'N') = 'N' and T1.""U_CancelType"" in ('N')))))
";
                oRec.DoQuery(Query);
                if (Convert.ToInt32(oRec.Fields.Item("DocCount").Value) > 0)
                {
                    Global.SendMessage("Document Posting", "Automated posting(s) for pending documents initialized. Please do not close SAP Business One till the next notification", new string[] { Global.Comp_DI.UserName }, new List<List<string>>() { }, "", "");
                    InitiateDocumentsPosting(DocEntry);
                }

            }
        }
        private void Form_DataUpdateBefore(ref SAPbouiCOM.BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            StoppedByWrongForm = false;
            SecondIteration = false;

            BubbleEvent = true;
            int ret = 0;
            if (DocType == "R")
            {
                ret = Global.myApi.MessageBox("Are you sure you want to update this document?\nNote: Updating this document may initiate pending Invoice(s) and Voucher(s) process in the background. Please keep SAP running until you receive messages to notify completion.", 1, "Yes", "No");

            }
            else
            {
                ret = Global.myApi.MessageBox("Are you sure you want to update this document?\nNote: Updating this document may initiate pending Refund(s) and Cancellation(s) process in the background. Please keep SAP running until you receive messages to notify completion", 1, "Yes", "No");
            }
            if (cmbDocType.Selected.Value != DocType)
            {
                ret = 0;
                BubbleEvent = false;
                string CurrentDoc = (cmbDocType.Selected.Value == "R" ? " Invoice Generation " : " Refund Generation ");
                string CurrentForm = (DocType == "R" ? " Invoice Generation " : " Refund Generation ");
                MessageToShow = "Updating" + CurrentDoc + "from" + CurrentForm + "screen is not allowed";
                StoppedByWrongForm = true;
            }
            if (ret != 1)
            {
                BubbleEvent = false;
                Global.myApi.StatusBarEvent += MyApi_StatusBarEvent;
            }

        }

        private void cmbSchool_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(false);
        }

        private void cmbSchool_KeyDownBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(false);

        }

        private void cmbStatus_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(false);

        }
        private void cmbStatus_KeyDownBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(false);

        }
        private void cmbSemester_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(false);

        }
        private void cmbSemester_KeyDownBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(false);
        }

        private void cmbCancelType_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(false);

        }

        private void cmbCancelType_KeyDownBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(false);
        }

        private void Matrix0_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (UIAPIRawForm.Mode != SAPbouiCOM.BoFormMode.fm_ADD_MODE)
            {
                BubbleEvent = false;
                return;
            }
            if ((pVal.ColUID == "Cancel" || pVal.ColUID == "MAmount" || pVal.ColUID == "MAccount") && DocType == "R")
            {
                BubbleEvent = false;
            }
            if (DocType == "C" && (pVal.ColUID == "MAmount" || pVal.ColUID == "MAccount") && pVal.Row > -1)
            {
                string CancelType = ((SAPbouiCOM.ComboBox)Matrix0.GetCellSpecific("Cancel", pVal.Row)).Selected.Value;
                if (CancelType != "M")
                {
                    BubbleEvent = false;
                }
            }
            if (pVal.ColUID == "AppPrice")
            {
                bool IsManual = ((SAPbouiCOM.CheckBox)Matrix0.GetCellSpecific("ManPrice", pVal.Row)).Checked;
                if (!IsManual)
                {
                    BubbleEvent = false;
                }
                else
                {
                    BubbleEvent = true;
                }

            }
        }

        private void Matrix0_KeyDownBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (UIAPIRawForm.Mode != SAPbouiCOM.BoFormMode.fm_ADD_MODE)
            {
                BubbleEvent = false;
                return;
            }
            if ((pVal.ColUID == "Cancel" || pVal.ColUID == "MAmount" || pVal.ColUID == "MAccount") && DocType == "R")
            {
                BubbleEvent = false;
            }
            if (DocType == "C" && (pVal.ColUID == "MAmount" || pVal.ColUID == "MAccount") && pVal.Row > -1)
            {
                string CancelType = ((SAPbouiCOM.ComboBox)Matrix0.GetCellSpecific("Cancel", pVal.Row)).Selected.Value;
                if (CancelType != "M")
                {
                    BubbleEvent = false;
                }
            }
            //if (pVal.ColUID == "AppPrice")
            //{
            //    bool IsManual = ((SAPbouiCOM.CheckBox)Matrix0.GetCellSpecific("ManPrice", pVal.Row)).Checked;
            //    if (!IsManual)
            //    {
            //        BubbleEvent = false;
            //    }
            //    else
            //    {
            //        BubbleEvent = true;
            //    }

            //}
        }

        private SAPbouiCOM.EditText txtDueDate;
        private SAPbouiCOM.StaticText stDueDate;

        private void txtDueDate_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(false);

        }

        private void txtDueDate_KeyDownBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(false);
        }

        private void txtDocDate_ValidateBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (!string.IsNullOrEmpty(txtDocDate.Value) && !string.IsNullOrEmpty(txtToDate.Value))
            {
                int Year = Convert.ToInt16(txtToDate.Value.Substring(0, 4));
                int Month = Convert.ToInt16(txtToDate.Value.Substring(4, 2));
                int Day = Convert.ToInt16(txtToDate.Value.Substring(6, 2));
                DateTime newToDate = new DateTime(Year, Month, Day);
                Year = Convert.ToInt16(txtDocDate.Value.Substring(0, 4));
                Month = Convert.ToInt16(txtDocDate.Value.Substring(4, 2));
                Day = Convert.ToInt16(txtDocDate.Value.Substring(6, 2));
                DateTime newDocDate = new DateTime(Year, Month, Day);

                if (newDocDate > newToDate)
                {
                    Global.SetMessage("Document Date cannot be later than Ending Date", SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                    BubbleEvent = false;
                }
            }


        }

        private void Form_CloseAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {

            Global.myApi.MenuEvent -= this.MyApi_MenuEvent;
        }

        SAPbouiCOM.Conditions Cons;
        private void Matrix0_ChooseFromListBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (DocType == "C" && pVal.Row > -1)
            {
                string CancelType = ((SAPbouiCOM.ComboBox)Matrix0.GetCellSpecific("Cancel", pVal.Row)).Selected.Value;
                if (CancelType != "M")
                {
                    BubbleEvent = false;
                    return;
                }
            }
            if (pVal.ColUID == "MAccount")
            {
                Cons = null;
                UIAPIRawForm.ChooseFromLists.Item("CFL_1").SetConditions(Cons);
                Cons = UIAPIRawForm.ChooseFromLists.Item("CFL_1").GetConditions();
                SAPbouiCOM.Condition con = Cons.Add();
                con.Alias = "U_RefundAcct";
                con.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                con.CondVal = "Y";
                con.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND;
                con = Cons.Add();
                con.Alias = "Postable";
                con.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                con.CondVal = "Y";
                UIAPIRawForm.ChooseFromLists.Item("CFL_1").SetConditions(Cons);
            }
        }

        private void Matrix0_ChooseFromListAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            SAPbouiCOM.ISBOChooseFromListEventArg args = (SAPbouiCOM.ISBOChooseFromListEventArg)pVal;
            if (args.SelectedObjects != null)
            {
                Matrix0.SetCellWithoutValidation(pVal.Row, pVal.ColUID, args.SelectedObjects.GetValue("AcctCode", 0).ToString());
            }

        }

        private void Matrix0_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            if (pVal.ColUID == "Cancel" && DocType == "C")
            {
                string CancelType = ((SAPbouiCOM.ComboBox)Matrix0.GetCellSpecific("Cancel", pVal.Row)).Selected.Value;
                UIAPIRawForm.Freeze(true);
                if (CancelType != "M")
                {
                    ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("MAmount", pVal.Row)).Value = "0";
                    ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("MAccount", pVal.Row)).Value = "";
                }
                UIAPIRawForm.Freeze(false);
            }
        }

        private void Matrix0_ValidateBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (pVal.ColUID == "MAmount")
            {
                double PrincipleAmount = 0;
                double TaxAmount = 0;
                double TrailAmount = 0;
                double ManualAmount = 0;

                double.TryParse(((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("DocTotal", pVal.Row)).Value, out PrincipleAmount);
                double.TryParse(((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("TaxAmount", pVal.Row)).Value, out TaxAmount);
                double.TryParse(((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("MAmount", pVal.Row)).Value, out ManualAmount);
                if (ManualAmount > (PrincipleAmount + TaxAmount))
                {
                    Global.SetMessage("Refund amount cannot be greater than original amount", SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                    BubbleEvent = false;
                }
            }

        }

        private SAPbouiCOM.EditText txtStudent;
        private SAPbouiCOM.StaticText stStudent;

        private void txtStudent_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(false);
        }

        private void txtStudent_KeyDownBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = ValidateFind(false);
        }

        //private void Matrix0_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        //{
        //    UIAPIRawForm.Freeze(true);
        //    //if (pVal.ColUID == "ManPrice" && UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE && pVal.Row > 0 && Matrix0.RowCount != 0 && pVal.Row <= RowsDb.Size)
        //    //{
        //    //    bool IsManual = ((SAPbouiCOM.CheckBox)Matrix0.GetCellSpecific("ManPrice", pVal.Row)).Checked;
        //    //    if (IsManual)
        //    //    {
        //    //        ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("AppPrice", pVal.Row)).Value = ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("Price", pVal.Row)).Value;
        //    //    }
        //    //    else
        //    //    {
        //    //        ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("AppPrice", pVal.Row)).Value = "0";
        //    //    }
        //    //}
        //    UIAPIRawForm.Freeze(false);
        //}

        //private void Matrix0_PressedBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        //{

        //    BubbleEvent = true;
        //    //if (pVal.ColUID == "ManPrice" && pVal.Row > 0 && Matrix0.RowCount != 0 && pVal.Row <= RowsDb.Size)
        //    //{
        //    //    if (UIAPIRawForm.Mode != SAPbouiCOM.BoFormMode.fm_ADD_MODE)
        //    //    {
        //    //        BubbleEvent = false;
        //    //        return;
        //    //    }
        //    //    string Status = ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("Status", pVal.Row)).String;

        //    //    if (Status != "New")
        //    //    {
        //    //        BubbleEvent = false;
        //    //    }
        //    //    else
        //    //    {
        //    //        BubbleEvent = true;
        //    //    }

        //    //}

        //}
        private void CalculateAndSetTotals(int Row, string ColUID)
        {
            double Price = 0;
            double DiscountPC = 0;
            double HDiscount = 0;
            double TaxRate = 0;
            double TaxAmount = 0;
            double SchDiscount = 0;
            double DocTotal = 0;
            double AfterDiscount = 0;
            double HDiscountPC = 0;
            double RegAmount = 0;

            double Freight1 = 0;
            double Freight2 = 0;
            double Freight3 = 0;
            double Freight4 = 0;
            double Freight5 = 0;
            double Freight6 = 0;
            double Freight7 = 0;
            double Freight8 = 0;
            double Freight9 = 0;
            double Freight10 = 0;

            double TotalFreight = 0;

            string sDiscountPC = ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("DiscPC", Row)).Value;
            string sSchDiscount = ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("SchDiscPC", Row)).Value;
            string sRegAmount = ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("AftSchDisc", Row)).Value;
            string sHDiscount = ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("HDiscount", Row)).Value;
            string sDocTotal = ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("DocTotal", Row)).Value;
            string sTaxRate = ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("TaxRate", Row)).Value;
            string sTaxAmount = ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("TaxAmount", Row)).Value;
            //string sTrailAmount = ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("TAmount", Row)).Value;
            string sAfterDiscount = ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("AfterDisc", Row)).Value;
            //bool Manual = ((SAPbouiCOM.CheckBox)Matrix0.GetCellSpecific("ManPrice", Row)).Checked;
            string sFreight1 = ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("Freight1", Row)).Value;
            string sFreight2 = ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("Freight2", Row)).Value; ;
            string sFreight3 = ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("Freight3", Row)).Value; ;
            string sFreight4 = ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("Freight4", Row)).Value; ;
            string sFreight5 = ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("Freight5", Row)).Value; ;
            string sFreight6 = ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("Freight6", Row)).Value; ;
            string sFreight7 = ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("Freight7", Row)).Value; ;
            string sFreight8 = ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("Freight8", Row)).Value; ;
            string sFreight9 = ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("Freight9", Row)).Value; ;
            string sFreight10 = ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("Freight10", Row)).Value; ;

            //double.TryParse(sPrice, out Price);
            //double.TryParse(sAppliedPrice, out AppliedPrice);
            double.TryParse(sHDiscount, out HDiscount);
            double.TryParse(sTaxRate, out TaxRate);
            double.TryParse(sTaxAmount, out TaxAmount);
            double.TryParse(sSchDiscount, out SchDiscount);
            double.TryParse(sAfterDiscount, out AfterDiscount);
            double.TryParse(sDocTotal, out DocTotal);
            double.TryParse(sFreight1, out Freight1);
            double.TryParse(sFreight2, out Freight2);
            double.TryParse(sFreight3, out Freight3);
            double.TryParse(sFreight4, out Freight4);
            double.TryParse(sFreight5, out Freight5);
            double.TryParse(sFreight6, out Freight6);
            double.TryParse(sFreight7, out Freight7);
            double.TryParse(sFreight8, out Freight8);
            double.TryParse(sFreight9, out Freight9);
            double.TryParse(sFreight10, out Freight10);
            double.TryParse(sRegAmount, out RegAmount);
            double.TryParse(sDiscountPC, out DiscountPC);

            TotalFreight = Freight1 + Freight2 + Freight3 + Freight4 + Freight5 + Freight6 + Freight7 + Freight8 + Freight9 + Freight10;

            //if (!Manual)
            //{
            //    AppliedPrice = Price;
            //}
            if (ColUID == "DiscPC")
            {
                HDiscount = RegAmount * (DiscountPC / 100);
                ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("HDiscount", Row)).Value = HDiscount.ToString();
            }
            else if (ColUID == "HDiscount")
            {
                DiscountPC = (HDiscount / RegAmount) * 100;
                ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("DiscPC", Row)).Value = DiscountPC.ToString();
            }

            HDiscountPC = (HDiscount / RegAmount) * 100;
            AfterDiscount = (RegAmount - HDiscount);
            TaxAmount = ((RegAmount - HDiscount) + TotalFreight) * (TaxRate / 100);
            DocTotal = (RegAmount - HDiscount) + TaxAmount + TotalFreight;


            ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("TaxAmount", Row)).Value = TaxAmount.ToString();
            ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("DocTotal", Row)).Value = DocTotal.ToString();
            ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("AfterDisc", Row)).Value = AfterDiscount.ToString();
            ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("TFreight", Row)).Value = TotalFreight.ToString();
            Matrix0.FlushToDataSource();



        }
        private void Matrix0_ValidateAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            string FreightCol = "";
            try
            {
                FreightCol = pVal.ColUID.Substring(0, 7);
            }
            catch { }

            if ((pVal.ColUID == "AppPrice" || pVal.ColUID == "HDiscount" || FreightCol == "Freight" || pVal.ColUID == "DiscPC") && !pVal.InnerEvent)
            {
                UIAPIRawForm.Freeze(true);
                Matrix0.FlushToDataSource();
                Matrix0.LoadFromDataSource();
                CalculateAndSetTotals(pVal.Row, pVal.ColUID);
                Matrix0.LoadFromDataSource();
                UIAPIRawForm.Freeze(false);
            }

        }

        private void txtMajor_ChooseFromListBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            Cons = null;
            UIAPIRawForm.ChooseFromLists.Item("CFL_0").SetConditions(Cons);
            Cons = UIAPIRawForm.ChooseFromLists.Item("CFL_0").GetConditions();
            SAPbouiCOM.Condition con = Cons.Add();
            con.Alias = "DimCode";
            con.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            con.CondVal = Config.MajorDim;
            UIAPIRawForm.ChooseFromLists.Item("CFL_0").SetConditions(Cons);
        }




        //private void Matrix0_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        //{
        //    if (pVal.ColUID =="ManPrice")
        //    {

        //    }
        //}
    }
    public class MonthlyAmounts
    {
        public int Year, Days, Month;
        public DateTime Date;
        public decimal Amount;
        public string NameOfMonth, StudentID, RegNo, DebitAccount, CreditAccount, GenNo, InvEntry, InvNum, Major, Scholarship, MemoEntry, MemoNum, DocType, CancelType, CancelEntry, CancelNum;
        public bool First = false, Last = false;
    }
}
