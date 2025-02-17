using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SAPbobsCOM;
using SM_One.Models;
using SM_One.Services;
using SM_One.Repositories;
using System.Globalization;
using System.Windows.Forms;

namespace SM_One.SAP
{
    [FormAttribute("SM_One.SAP.ImportRegistrations", "SAP/ImportRegistrations.b1f")]
    class ImportRegistrations : UserFormBase
    {
        public ImportRegistrations()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.cmbSemester = ((SAPbouiCOM.ComboBox)(this.GetItem("cbSemester").Specific));
            this.cmbCollege = ((SAPbouiCOM.ComboBox)(this.GetItem("cbCollege").Specific));
            this.cmbMajors = ((SAPbouiCOM.ComboBox)(this.GetItem("cbMajor").Specific));
            this.Grid0 = ((SAPbouiCOM.Grid)(this.GetItem("Grid0").Specific));
            this.Grid0.LinkPressedAfter += new SAPbouiCOM._IGridEvents_LinkPressedAfterEventHandler(this.Grid0_LinkPressedAfter);
            this.btnFetch = ((SAPbouiCOM.Button)(this.GetItem("btnFetch").Specific));
            this.btnFetch.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btnFetch_PressedAfter);
            this.btnPost = ((SAPbouiCOM.Button)(this.GetItem("btnPost").Specific));
            this.btnPost.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btnPost_PressedAfter);
            this.btnPost.PressedBefore += new SAPbouiCOM._IButtonEvents_PressedBeforeEventHandler(this.btnPost_PressedBefore);
            this.FillCombos();
            this.btnSync = ((SAPbouiCOM.Button)(this.GetItem("btnSync").Specific));
            this.btnSync.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btnSync_PressedAfter);
            this.btnSync.PressedBefore += new SAPbouiCOM._IButtonEvents_PressedBeforeEventHandler(this.btnSync_PressedBefore);
            this.btnPost.Item.Enabled = false;
            this.btnSync.Item.Enabled = false;
            this.btnClear = ((SAPbouiCOM.Button)(this.GetItem("btnClear").Specific));
            this.btnClear.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btnClear_PressedAfter);
            this.btnSelect = ((SAPbouiCOM.Button)(this.GetItem("btSelect").Specific));
            this.btnSelect.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btnSelect_PressedAfter);
            this.btnDeselect = ((SAPbouiCOM.Button)(this.GetItem("btDeselect").Specific));
            this.btnDeselect.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btnDeselect_PressedAfter);
            this.OnCustomInitialize();

        }
        List<StudentInfo> myStudentInfo = new List<StudentInfo>();
        DatabaseConfig dbConfig = new DatabaseConfig();
        private StudentInfoService _studentService;
        SAPbouiCOM.ProgressBar oBar;
        private void FillCombos()
        {
            Global.FillCombo(cmbSemester, (SAPbouiCOM.Form)UIAPIRawForm, "Select \"Code\", \"Name\" from \"@OSEM\" order by CAST(\"U_Sequence\" as integer) asc", "", false);
            Global.FillCombo(cmbCollege, (SAPbouiCOM.Form)UIAPIRawForm, "Select \"U_ID\" as \"Code\", concat(\"Code\",concat('-',\"U_Description\")) as \"Name\" from \"@OCOL\" order by \"U_ID\" asc", "");
            Global.FillCombo(cmbMajors, (SAPbouiCOM.Form)UIAPIRawForm, "Select \"U_TrioID\" as \"Code\", \"U_TrioName\" as \"Name\" from \"OPRC\" where \"DimCode\" = 3 order by CAST(\"U_TrioID\" as integer) asc", "");

            cmbSemester.Select(0, BoSearchKey.psk_Index);
            cmbMajors.Select("", BoSearchKey.psk_ByValue);
            cmbCollege.Select("", BoSearchKey.psk_ByValue);
            //cmbSemester.ExpandType = BoExpandType.et_DescriptionOnly;
            cmbMajors.ExpandType = BoExpandType.et_DescriptionOnly;
            cmbCollege.ExpandType = BoExpandType.et_DescriptionOnly;
        }
        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        private SAPbouiCOM.ComboBox cmbSemester;

        private void OnCustomInitialize()
        {

        }

        private SAPbouiCOM.ComboBox cmbCollege;
        private SAPbouiCOM.ComboBox cmbMajors;
        private SAPbouiCOM.Grid Grid0;
        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.Button btnFetch;
        private SAPbouiCOM.Button btnPost;
        private void FormulateGrid()
        {

            Grid0.DataTable.Rows.Add();
            EditTextColumn oCol = (EditTextColumn)Grid0.Columns.Item("Student Code");
            oCol.LinkedObjectType = "2";
            oCol = (EditTextColumn)Grid0.Columns.Item("Registration No.");
            oCol.LinkedObjectType = "SRG";
            try
            {
                oCol = (EditTextColumn)Grid0.Columns.Item("Select");
                oCol.Type = BoGridColumnType.gct_CheckBox;
                oCol = (EditTextColumn)Grid0.Columns.Item("BP Sync");
                oCol.Type = BoGridColumnType.gct_CheckBox;
                oCol = (EditTextColumn)Grid0.Columns.Item("Registered");
                oCol.Type = BoGridColumnType.gct_CheckBox;
               
            }
            catch { }

            for (int i = 0; i < Grid0.Columns.Count; i++)
            {
                Grid0.Columns.Item(i).Editable = false;
            }
            Grid0.Columns.Item("Select").Editable = true;
        }
        private void FetchStudentRegistration()
        {
            Global.SetMessage("Fetching Student Registrations. This can take few minutes", BoStatusBarMessageType.smt_Warning);
             oBar = null;
            string Student = "";
            try
            {

                UIAPIRawForm.Freeze(true);
                FormulateGrid();
                var _studentRepo = new StudentRepository(dbConfig);
                _studentService = new StudentInfoService(_studentRepo);
                myStudentInfo = _studentService.GetStudentInfos(cmbSemester.Selected.Value.ToString(), cmbCollege.Selected.Value.ToString(), cmbMajors.Selected.Value.ToString()).ToList();
                oBar = Global.myApi.StatusBar.CreateProgressBar("Fetching Students", myStudentInfo.Count, false);
                int i = 0;
                foreach (StudentInfo student in myStudentInfo)
                {
                    oBar.Text = "Fetching Student : " + student.StudentCode + "-" + student.StudentNameEn;
                    Student = student.StudentCode;
                    Grid0.DataTable.SetValue("S.No", Grid0.Rows.Count - 1, student.RowNum.ToString());
                    Grid0.DataTable.SetValue("Select", Grid0.Rows.Count - 1, student.Select);
                    Grid0.DataTable.SetValue("Student Code", Grid0.Rows.Count - 1, student.StudentCode);
                    Grid0.DataTable.SetValue("Student Name", Grid0.Rows.Count - 1, student.StudentNameEn);
                    Grid0.DataTable.SetValue("Student Status", Grid0.Rows.Count - 1, student.StudentStatusID);
                    Grid0.DataTable.SetValue("Student Date", Grid0.Rows.Count - 1, student.StudentDate.ToString("yyyyMMdd"));
                    Grid0.DataTable.SetValue("Student Group", Grid0.Rows.Count - 1, student.StudentGroupID);
                    Grid0.DataTable.SetValue("Telephone1", Grid0.Rows.Count - 1, student.Telephone1);
                    Grid0.DataTable.SetValue("Telephone2", Grid0.Rows.Count - 1, student.Telephone2);
                    Grid0.DataTable.SetValue("Email", Grid0.Rows.Count - 1, student.Email);
                    Grid0.DataTable.SetValue("CollegeID", Grid0.Rows.Count - 1, student.CollegeID.ToString());
                    Grid0.DataTable.SetValue("College Code", Grid0.Rows.Count - 1, student.CollegeCode);
                    Grid0.DataTable.SetValue("College Name", Grid0.Rows.Count - 1, student.CollegeDescriptionEn);
                    Grid0.DataTable.SetValue("SemesterID", Grid0.Rows.Count - 1, student.SemesterID.ToString());
                    Grid0.DataTable.SetValue("Semester", Grid0.Rows.Count - 1, student.SemesterDescriptionEn);
                    Grid0.DataTable.SetValue("Academic Year", Grid0.Rows.Count - 1, student.AcademicYearDescriptionEn);
                    Grid0.DataTable.SetValue("Start Month", Grid0.Rows.Count - 1, student.SemesterStartMonth);
                    Grid0.DataTable.SetValue("End Month", Grid0.Rows.Count - 1, student.SemesterEndMonth);
                    Grid0.DataTable.SetValue("MajorID", Grid0.Rows.Count - 1, student.MajorID.ToString());
                    Grid0.DataTable.SetValue("Major Code", Grid0.Rows.Count - 1, student.MajorCode);
                    Grid0.DataTable.SetValue("Major Name", Grid0.Rows.Count - 1, student.MajorDescriptionEn);
                    Grid0.DataTable.SetValue("AScholarshipID", Grid0.Rows.Count - 1, student.AdmissionScholarshipID.ToString());
                    Grid0.DataTable.SetValue("AScholarshipCode", Grid0.Rows.Count - 1, student.AdmissionScholarshipCode);
                    Grid0.DataTable.SetValue("Adm. Scholarship", Grid0.Rows.Count - 1, student.AdmissionScholarshipDesc);
                    Grid0.DataTable.SetValue("CScholarshipID", Grid0.Rows.Count - 1, student.CurrentScholarshipID.ToString());
                    Grid0.DataTable.SetValue("CScholarshipCode", Grid0.Rows.Count - 1, student.StudentCode);
                    Grid0.DataTable.SetValue("Scholarship Hours", Grid0.Rows.Count - 1, student.TotalScholarshipHours);
                    Grid0.DataTable.SetValue("Attempted Credits", Grid0.Rows.Count - 1, student.AttemptedCredits);
                    Grid0.DataTable.SetValue("CGPA", Grid0.Rows.Count - 1, student.CGPA);
                    Grid0.DataTable.SetValue("BP Sync", Grid0.Rows.Count - 1, "N");
                    i++;
                    oBar.Value = i;
                    Grid0.DataTable.Rows.Add();
                }
                Grid0.DataTable.Rows.Remove(Grid0.Rows.Count - 1);
                if (Grid0.DataTable.Rows.Count > 0)
                {
                    oBar.Value = oBar.Maximum;
                    oBar.Stop();
                    btnFetch.Item.Enabled = false;
                    btnSync.Item.Enabled = true;
                }
                UIAPIRawForm.Freeze(false);
            }
            catch (Exception ex)
            {
                Global.SetMessage(ex.Message + Student, BoStatusBarMessageType.smt_Error);
                try
                {
                    //oBar.Stop();
                }
                catch
                {

                }
            }
            finally
            {
                
                UIAPIRawForm.Freeze(false);
            }
        }
        private void btnFetch_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (Grid0.DataTable.Rows.Count == 0 && !btnSync.Item.Enabled && !btnPost.Item.Enabled)
            {
                FetchStudentRegistration();
            }
        }

        GeneralService oGeneralService;
        GeneralData oHeader;
        CompanyService oCmpSrv;
        GeneralData oRowItem;
        GeneralDataCollection oRows;
        private string GetItemPrice(string ItemCode, string Code)
        {
            Recordset oRec = Global.Comp_DI.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
            string Query = "Select T2.\"Price\" from \"@OSHL\" T0 inner join OPLN T1 on T0.\"U_PriceList\" = T1.\"ListName\" inner join ITM1 T2 on T2.\"PriceList\" = T1.\"ListNum\" where T2.\"ItemCode\" = '" + ItemCode.ToString() + "' and T0.\"Code\" = '" + Code.ToString() + "'";
            oRec.DoQuery(Query);
            return oRec.Fields.Item("Price").Value.ToString();

        }
        private double GetDiscountAndTax(string CardCode,string College, string Scholarship, out double TaxRate)
        {
            Recordset oRec = Global.Comp_DI.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
            string Query = "Select T0.\"U_DiscountPC\",coalesce((Select T2.\"Rate\" from OCRD T1 inner join OVTG T2 on T1.\"ECVatGroup\" = T2.\"Code\" where T1.\"CardCode\" = '"+CardCode+"'),0) as \"TaxRate\" from \"@ORMP\" T0 where T0.\"U_College\" = '"+College+"' and \"U_Scholarship\" = '"+Scholarship+"' and T0.\"U_MapType\" = 'R'";
            oRec.DoQuery(Query);
            TaxRate = Convert.ToDouble(oRec.Fields.Item("TaxRate").Value.ToString());
            return Convert.ToDouble(oRec.Fields.Item("U_DiscountPC").Value.ToString());
        }
        private void RegisterStudent(StudentInfo student, int Row, out bool Error)
        {
            try
            {




                Recordset oRec = Global.Comp_DI.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
                string Query = $@"Select ""DocEntry""  from ""@OSRG"" WHERE ""U_StudentCode""='{student.StudentCode}' AND ""U_College""='{student.CollegeCode}' AND ""U_Major""='{student.MajorCode}' AND ""U_Semester""='{student.SemesterID.ToString()}'";
                oRec.DoQuery(Query);
                if (oRec.RecordCount > 0)
                {
                    Grid0.DataTable.SetValue("Message", Row, "Student Already Exists");
                    Error = false;

                }
                else
                {



                    oCmpSrv = Global.Comp_DI.GetCompanyService();
                    oGeneralService = oCmpSrv.GetGeneralService("SRG");
                    oHeader = (SAPbobsCOM.GeneralData)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData);
                    oHeader.SetProperty("U_StudentCode", student.StudentCode);
                    oHeader.SetProperty("U_StudentName", student.StudentNameEn);
                    oHeader.SetProperty("U_Major", student.MajorCode);
                    oHeader.SetProperty("U_College", student.CollegeCode);
                    oHeader.SetProperty("U_StartDate", student.SemesterStartDate.ToString(Global._dateFormat));
                    oHeader.SetProperty("U_EndDate", student.SemesterEndDate.ToString(Global._dateFormat));
                    oHeader.SetProperty("U_Scholarship", student.CurrentScholarshipCode);
                    oHeader.SetProperty("U_Semester", student.SemesterID.ToString());
                    oHeader.SetProperty("Remark", "");
                    oRows = oHeader.Child("SRG1");
                    var _studentRepo = new StudentRepository(dbConfig);
                    _studentService = new StudentInfoService(_studentRepo);
                    List<StudentCourses> myStudentCourses = _studentService.GetStudentCourses(student.SemesterID.ToString(), student.StudentCode, student.CollegeID.ToString(), student.MajorID.ToString()).ToList();
                    double TaxRate = 0;
                    double TaxAmount = 0;
                    double SchDiscountPC = GetDiscountAndTax(student.StudentCode, student.CollegeCode, student.CurrentScholarshipCode, out TaxRate);
                    double SchDiscount = 0;
                    double DocTotal = 0;
                    for (int i = 0; i < myStudentCourses.Count; i++)
                    {
                        oRowItem = oRows.Add();
                        string Price = GetItemPrice(myStudentCourses[i].CourseCode.ToString(), myStudentCourses[i].Repeat == "Y" ? Config.RepeatScholarship : student.CurrentScholarshipCode.ToString());
                        oRowItem.SetProperty("U_SubjectCode", myStudentCourses[i].CourseCode);
                        oRowItem.SetProperty("U_SubjectName", myStudentCourses[i].CourseDescriptionEn);
                        oRowItem.SetProperty("U_Credits", myStudentCourses[i].Hours.ToString());
                        oRowItem.SetProperty("U_Price", Price);
                        oRowItem.SetProperty("U_LineType", "A");
                        double Total = Convert.ToDouble(myStudentCourses[i].Hours) * Convert.ToDouble(Price);
                        DocTotal += Total;
                        oRowItem.SetProperty("U_Total", Total.ToString());
                        oRowItem.SetProperty("U_Repeat", myStudentCourses[i].Repeat);
                        if (myStudentCourses[i].Repeat == "Y")
                        {
                            oHeader.SetProperty("U_HasRepeat", "Y");
                        }
                        oRowItem.SetProperty("U_RepeatCourse", myStudentCourses[i].RepeatCourse.ToString());
                    }

                    oHeader.SetProperty("U_GrossDocTotal", DocTotal.ToString());
                    oHeader.SetProperty("U_SchDiscountPC", SchDiscountPC.ToString());

                    SchDiscount = DocTotal * (SchDiscountPC / 100);
                    oHeader.SetProperty("U_SchDiscount", SchDiscount.ToString());
                    oHeader.SetProperty("U_AfterSchDisc", (DocTotal - SchDiscount).ToString());

                    oHeader.SetProperty("U_Discount", "0");
                    oHeader.SetProperty("U_DiscountPC", "0");

                    oHeader.SetProperty("U_AfterDiscount", (DocTotal - SchDiscount).ToString());
                    oHeader.SetProperty("U_TaxPC", TaxRate.ToString());
                    TaxAmount = (DocTotal - SchDiscount) * (TaxRate / 100);
                    oHeader.SetProperty("U_TaxAmount", TaxAmount.ToString());
                    oHeader.SetProperty("U_DocTotal", (DocTotal - SchDiscount - TaxAmount));
                    string Doc = oGeneralService.Add(oHeader).GetProperty("DocEntry").ToString();
                    if (!string.IsNullOrEmpty(Doc))
                    {
                        Grid0.DataTable.SetValue("Message", Row, "Successfully Registered");
                        Grid0.DataTable.SetValue("Registration No.", Row, Doc);
                        Grid0.DataTable.SetValue("Registered", Row, "Y");
                        Error = false;
                    }
                    else
                    {
                        Grid0.DataTable.SetValue("Message", Row, Global.Comp_DI.GetLastErrorDescription());
                        Error = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Grid0.DataTable.SetValue("Message", Row, ex.Message);
                Global.myApi.MessageBox(ex.StackTrace);
                Error = true;
            }
        }
        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button btnSync;

        private void btnSync_PressedBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (btnPost.Item.Enabled)
            {
                Global.SetMessage("Please initiate BP Sync before Posting Registrations", BoStatusBarMessageType.smt_Error);
            }
        }
        private bool IsNotDuplicate(int Row)
        {
            try
            {
                Recordset oRec = Global.Comp_DI.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
                oRec.DoQuery("Select count(*) from \"@OSRG\" where \"U_StudentCode\" = '"+Grid0.DataTable.GetValue("Student Code",Row).ToString()+ "' and \"U_Semester\"='" + Grid0.DataTable.GetValue("SemesterID", Row).ToString() + "'");
                if (Convert.ToInt16(oRec.Fields.Item(0).Value.ToString()) > 0)
                {
                    Grid0.DataTable.SetValue("Message", Row, "Duplicate Registration for Student and Semester Combination");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Grid0.DataTable.SetValue("Message", Row, ex.Message);
                return false;
            }
        }
        private void AddOrUpdateStudent(StudentInfo student, int Row, out bool Error)
        {
            BusinessPartners oBP = Global.Comp_DI.GetBusinessObject(BoObjectTypes.oBusinessPartners) as BusinessPartners;
            bool Update = false;
            try
            {
                if (oBP.GetByKey(student.StudentCode))
                {
                    Update = true;
                    oBP.CardName = student.StudentNameEn;
                    oBP.Phone1 = student.Telephone1;
                    oBP.Phone2 = student.Telephone2;
                    oBP.EmailAddress = student.Email;
                    oBP.AdditionalID = student.StudentID.ToString();
                    oBP.UserFields.Fields.Item("U_Semester").Value = student.SemesterID.ToString();
                    oBP.UserFields.Fields.Item("U_CurScholar").Value = student.CurrentScholarshipCode;
                    oBP.UserFields.Fields.Item("U_Status").Value = student.StudentStatusID;
                    oBP.UserFields.Fields.Item("U_Major").Value = student.MajorCode;
                    oBP.UserFields.Fields.Item("U_College").Value = student.CollegeCode;
                    oBP.UserFields.Fields.Item("U_JoiningDate").Value = student.StudentDate.ToString(Global._dateFormat);
                    oBP.UserFields.Fields.Item("U_CGPA").Value = student.CGPA;
                    oBP.UserFields.Fields.Item("U_AttmptCHours").Value = student.AttemptedCredits;
                    oBP.UserFields.Fields.Item("U_SchrCHours").Value = student.TotalScholarshipHours;
                    oBP.VatGroup = Config.TaxGroup;
                }
                else
                {
                    oBP.AdditionalID = student.StudentID.ToString();
                    oBP.CardCode = student.StudentCode;
                    oBP.CardName = student.StudentNameEn;
                    oBP.Phone1 = student.Telephone1;
                    oBP.Phone2 = student.Telephone2;
                    oBP.EmailAddress = student.Email;
                    oBP.UserFields.Fields.Item("U_Semester").Value = student.SemesterID.ToString();
                    oBP.UserFields.Fields.Item("U_CurScholar").Value = student.CurrentScholarshipCode;
                    oBP.UserFields.Fields.Item("U_AdmScholar").Value = student.AdmissionScholarshipCode;
                    oBP.UserFields.Fields.Item("U_College").Value = student.CollegeCode;
                    oBP.UserFields.Fields.Item("U_Status").Value = student.StudentStatusID;
                    oBP.UserFields.Fields.Item("U_Major").Value = student.MajorCode;
                    oBP.UserFields.Fields.Item("U_JoiningDate").Value = student.StudentDate.ToString(Global._dateFormat);
                    oBP.UserFields.Fields.Item("U_CGPA").Value = student.CGPA;
                    oBP.UserFields.Fields.Item("U_AttmptCHours").Value = student.AttemptedCredits;
                    oBP.UserFields.Fields.Item("U_SchrCHours").Value = student.TotalScholarshipHours;
                    oBP.VatGroup = Config.TaxGroup;
                }

                if ((Update == true ? oBP.Update() : oBP.Add()) != 0)
                {
                    Grid0.DataTable.SetValue("Message", Row, Global.Comp_DI.GetLastErrorDescription());
                    Error = true;
                }
                else
                {
                    Grid0.DataTable.SetValue("Message", Row, "Successfully"+(Update == true ? " Updated":" Added"));
                    Grid0.DataTable.SetValue("BP Sync", Row, "Y");
                    Grid0.AutoResizeColumns();
                    Error = false;
                }
            }
            catch (Exception ex)
            {
                Grid0.DataTable.SetValue("Message", Row, ex.Message);
                Error = true;
            }
        }
        private void SyncBusinessParnters()
        {
            try
            {
                Global.SetMessage("Posting Business Partners. This can take few minutes", BoStatusBarMessageType.smt_Warning);
                int i = 0;
                bool Error = false;
                UIAPIRawForm.Freeze(true);
                oBar = null;
                 
                 oBar = Global.myApi.StatusBar.CreateProgressBar("Posting Student", myStudentInfo.Count, true);
                oBar.Maximum = myStudentInfo.Count;
                foreach (var student in myStudentInfo)
                {
                 
                    bool AddError = false;
                    if (Grid0.DataTable.GetValue("BP Sync", i).ToString() == "N" && Grid0.DataTable.GetValue("Select", i).ToString() == "Y" )//&& IsNotDuplicate(i))
                    {
                        oBar.Text = "Syncing Student : " +
                                    (student.StudentCode ?? string.Empty) + " - " +
                                    (student.StudentNameEn ?? string.Empty);
                        AddOrUpdateStudent(student, i, out AddError);
                    }
                    if (AddError)
                    {
                        Error = AddError;
                    }
                    i++;
                    oBar.Value = i;

                }
                oBar.Stop();
                if (!Error)
                {
                    btnSync.Item.Enabled = false;
                    btnPost.Item.Enabled = true;
                }
                Global.SetMessage("Sync Completed" + (Error == true ? " With Some Errors. Correct All Errors Before Creating Registrations" : ""), Error == true ? SAPbouiCOM.BoStatusBarMessageType.smt_Warning : SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                oBar.Stop();

                UIAPIRawForm.Freeze(false);
                Global.SetMessage(ex.Message, BoStatusBarMessageType.smt_Error);

            }
            finally
            {
                UIAPIRawForm.Freeze(false);
            }
        }
        private void btnSync_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            SyncBusinessParnters();
        }
        private void Clear()
        {
            Grid0.DataTable.Rows.Clear();
            myStudentInfo.Clear();
            btnFetch.Item.Enabled = true;
            btnPost.Item.Enabled = false;
            btnSync.Item.Enabled = false;
            cmbMajors.Select("", BoSearchKey.psk_ByValue);
            cmbCollege.Select("", BoSearchKey.psk_ByValue);
            Global.SetMessage("Records Cleared", BoStatusBarMessageType.smt_Warning);
        }

        private SAPbouiCOM.Button btnClear;

        private void btnClear_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            Clear();
          
        }

        private void btnPost_PressedBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (Grid0.Rows.Count == 0)
            {
                BubbleEvent = false;
                Global.SetMessage("No Student(s) to Register", BoStatusBarMessageType.smt_Error);
            }

        }

        private void btnPost_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                oBar = null;
                int i = 0;
                bool Error = false;
                UIAPIRawForm.Freeze(true);
                oBar = Global.myApi.StatusBar.CreateProgressBar("Posting Student", myStudentInfo.Count, true);
                oBar.Maximum = myStudentInfo.Count;
                foreach (var student in myStudentInfo)
                {
                    if (i == 172)
                    {
                        string a = "";
                    }
                    bool AddError = false;
                    if (Grid0.DataTable.GetValue("BP Sync", i).ToString() == "Y" && Grid0.DataTable.GetValue("Select", i).ToString() == "Y")
                    {
                        RegisterStudent(student, i, out AddError);
                    }
                    if (AddError)
                    {
                        Error = AddError;
                    }
                    i++;
                    oBar.Value = i;
                }
                oBar.Stop();
                Global.SetMessage("Sync Completed" + (Error == true ? " With Some Errors. Correct All Errors And Register Again" : ""), Error == true ? SAPbouiCOM.BoStatusBarMessageType.smt_Warning : SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                btnPost.Item.Enabled = false;
            }
            catch (Exception ex)
            {
                Global.SetMessage(ex.Message, BoStatusBarMessageType.smt_Error);
                oBar.Stop();

            }
            finally
            {
                UIAPIRawForm.Freeze(false);
            }

        }

        private void Grid0_LinkPressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            
            if (pVal.ColUID == "Registration No.")
            {
                string DocEntry = Grid0.DataTable.GetValue("Registration No.", pVal.Row).ToString();
                Registration active = new Registration();
                active.UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE;
                active.txtDocEntry.Value = DocEntry;
                active.btnSave.Item.Click(0);
                active.Show();
            }

        }

        private SAPbouiCOM.Button btnSelect;
        private SAPbouiCOM.Button btnDeselect;

        private void btnSelect_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            Global.SetMessage("Selecting All", BoStatusBarMessageType.smt_Warning);
            UIAPIRawForm.Freeze(true);
            for (int i = 0; i < Grid0.DataTable.Rows.Count; i++)
            {
                Grid0.DataTable.SetValue("Select", i, "Y");
            }
            UIAPIRawForm.Freeze(false);
        }

        private void btnDeselect_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            Global.SetMessage("Deselecting All", BoStatusBarMessageType.smt_Warning);
            UIAPIRawForm.Freeze(true);
            for (int i = 0; i < Grid0.DataTable.Rows.Count; i++)
            {
                Grid0.DataTable.SetValue("Select", i, "N");
            }
            UIAPIRawForm.Freeze(false);
        }
    }
}
