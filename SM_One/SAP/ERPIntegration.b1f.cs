using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SM_One.Services;
using SM_One.Repositories;
using SM_One.Models;
using SAPbobsCOM;
using System.Globalization;
using SAPbouiCOM;

namespace SM_One.SAP
{
    [FormAttribute("SM_One.SAP.ERPIntegration", "SAP/ERPIntegration.b1f")]
    class ERPIntegration : UserFormBase
    {
        public ERPIntegration()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.Grid0 = ((SAPbouiCOM.Grid)(this.GetItem("Grid0").Specific));
            this.cmbObject = ((SAPbouiCOM.ComboBox)(this.GetItem("cmbObject").Specific));
            this.btnFetch = ((SAPbouiCOM.Button)(this.GetItem("btnFetch").Specific));
            this.btnFetch.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btnFetch_PressedAfter);
            this.btnPost = ((SAPbouiCOM.Button)(this.GetItem("btnPost").Specific));
            this.btnPost.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btnPost_PressedAfter);
            cmbObject.ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly;
            this.OnCustomInitialize();

        }
        private CollegeService _collegeService;
        private SemesterService _semesterService;
        private StudentInfoService _studentService;
        private MajorService _majorService;
        private ScholarshipService _scholarService;
        private CourseService _courseService;

        DatabaseConfig dbConfig = new DatabaseConfig();
        List<Semesters> mySemesters = new List<Semesters>();
        List<Majors> myMajors = new List<Majors>();
        List<Scholarships> myScholarships = new List<Scholarships>();
        List<Colleges> myColleges = new List<Colleges>();
        List<Courses> myCourses = new List<Courses>();
        private void FetchSemesters()
        {
            try
            {
                UIAPIRawForm.Freeze(true);
                FormulateGrid();
                Global.SetMessage("Fetching Semesters. This can take few minutes", BoStatusBarMessageType.smt_Warning);
                var SemesterRepo = new SemesterRepository(dbConfig);
                _semesterService = new SemesterService(SemesterRepo);
                mySemesters = _semesterService.GetAllSemesters().ToList();
                foreach (Semesters semester in mySemesters)
                {
                    Grid0.DataTable.SetValue("ID", Grid0.Rows.Count - 1, semester.ID.ToString());
                    Grid0.DataTable.SetValue("Description", Grid0.Rows.Count - 1, semester.DescriptionEn);
                    Grid0.DataTable.SetValue("Sequence", Grid0.Rows.Count - 1, semester.Sequence.ToString());
                    Grid0.DataTable.SetValue("SemesterType", Grid0.Rows.Count - 1, semester.SemesterType);
                    Grid0.DataTable.SetValue("StartDate", Grid0.Rows.Count - 1, semester.SemesterStartDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture));
                    Grid0.DataTable.SetValue("EndDate", Grid0.Rows.Count - 1, semester.SemesterEndDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture));
                    Grid0.DataTable.SetValue("Integrated", Grid0.Rows.Count - 1, "N");
                    Grid0.DataTable.Rows.Add();
                }
                Grid0.DataTable.Rows.Remove(Grid0.Rows.Count - 1);
                Grid0.AutoResizeColumns();
                UIAPIRawForm.Freeze(false);
            }
            catch (Exception ex)
            {
                Global.SetMessage(ex.Message, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                UIAPIRawForm.Freeze(false);
            }
        }
        private void FetchMajors()
        {
            try
            {
                UIAPIRawForm.Freeze(true);
                FormulateGrid();
                Global.SetMessage("Fetching Majors. This can take few minutes", BoStatusBarMessageType.smt_Warning);

                var MajorRepo = new MajorRepository(dbConfig);
                _majorService = new MajorService(MajorRepo);
                myMajors = _majorService.GetAllMajors().ToList();
                foreach (Majors majors in myMajors)
                {
                    Grid0.DataTable.SetValue("ID", Grid0.Rows.Count - 1, majors.ID.ToString());
                    Grid0.DataTable.SetValue("Code", Grid0.Rows.Count - 1, majors.Code);
                    Grid0.DataTable.SetValue("Description", Grid0.Rows.Count - 1, majors.DescriptionEn);
                    Grid0.DataTable.SetValue("Integrated", Grid0.Rows.Count - 1, "N");
                    Grid0.DataTable.Rows.Add();
                }
                Grid0.DataTable.Rows.Remove(Grid0.Rows.Count - 1);
                Grid0.AutoResizeColumns();
                UIAPIRawForm.Freeze(false);
            }
            catch (Exception ex)
            {
                Global.SetMessage(ex.Message, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                UIAPIRawForm.Freeze(false);
            }
        }
        private void FetchScholarships()
        {
            try
            {
                UIAPIRawForm.Freeze(true);
                FormulateGrid();
                Global.SetMessage("Fetching Scholarships. This can take few minutes", BoStatusBarMessageType.smt_Warning);

                var ScholarRepo = new ScholarshipRepository(dbConfig);
                _scholarService = new ScholarshipService(ScholarRepo);
                myScholarships = _scholarService.GetAllScholarships().ToList();
                foreach (Scholarships scholars in myScholarships)
                {
                    Grid0.DataTable.SetValue("ID", Grid0.Rows.Count - 1, scholars.ID.ToString());
                    Grid0.DataTable.SetValue("Code", Grid0.Rows.Count - 1, scholars.Code);
                    Grid0.DataTable.SetValue("Description", Grid0.Rows.Count - 1, scholars.DescriptionEn);
                    Grid0.DataTable.SetValue("GPA", Grid0.Rows.Count - 1, scholars.MinCGPA);
                    Grid0.DataTable.SetValue("Integrated", Grid0.Rows.Count - 1, "N");
                    Grid0.DataTable.Rows.Add();
                }
                Grid0.DataTable.Rows.Remove(Grid0.Rows.Count - 1);
                Grid0.AutoResizeColumns();
                UIAPIRawForm.Freeze(false);
            }
            catch (Exception ex)
            {
                Global.SetMessage(ex.Message, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                UIAPIRawForm.Freeze(false);
            }
        }
        private void FetchColleges()
        {
            try
            {
                UIAPIRawForm.Freeze(true);
                FormulateGrid();
                Global.SetMessage("Fetching Colleges. This can take few minutes", BoStatusBarMessageType.smt_Warning);

                var CollegeRepo = new CollegeRepository(dbConfig);
                _collegeService = new CollegeService(CollegeRepo);
                myColleges = _collegeService.GetAllColleges().ToList();
                foreach (Colleges college in myColleges)
                {
                    Grid0.DataTable.SetValue("ID", Grid0.Rows.Count - 1, college.ID.ToString());
                    Grid0.DataTable.SetValue("Code", Grid0.Rows.Count - 1, college.Code);
                    Grid0.DataTable.SetValue("Description", Grid0.Rows.Count - 1, college.DescriptionEn);
                    Grid0.DataTable.SetValue("Integrated", Grid0.Rows.Count - 1, "N");
                    Grid0.DataTable.Rows.Add();
                }
                Grid0.DataTable.Rows.Remove(Grid0.Rows.Count - 1);
                Grid0.AutoResizeColumns();
                UIAPIRawForm.Freeze(false);
            }
            catch (Exception ex)
            {
                Global.SetMessage(ex.Message, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                UIAPIRawForm.Freeze(false);
            }
        }
        private void FetchCourses()
        {
            try
            {
                UIAPIRawForm.Freeze(true);
                FormulateGrid();
                Global.SetMessage("Fetching Semesters. This can take few minutes", BoStatusBarMessageType.smt_Warning);

                var CourseRepo = new CourseRepository(dbConfig);
                _courseService = new CourseService(CourseRepo);
                myCourses = _courseService.GetAllCourses().ToList();
                ProgressBar oBar = Global.myApi.StatusBar.CreateProgressBar("Fetching Courses", myCourses.Count, true);
                int i = 0;
                foreach (Courses course in myCourses)
                {
                    Grid0.DataTable.SetValue("ID", Grid0.Rows.Count - 1, course.ID.ToString());
                    Grid0.DataTable.SetValue("Code", Grid0.Rows.Count - 1, course.CourseCode);
                    Grid0.DataTable.SetValue("Description", Grid0.Rows.Count - 1, course.DescriptionEn);
                    Grid0.DataTable.SetValue("Credits", Grid0.Rows.Count - 1, course.CreditHours.ToString());
                    Grid0.DataTable.SetValue("PassingMarks", Grid0.Rows.Count - 1, course.PassMark.ToString());
                    Grid0.DataTable.SetValue("Scholarship", Grid0.Rows.Count - 1, course.Scholarship.ToString());
                    Grid0.DataTable.SetValue("Integrated", Grid0.Rows.Count - 1, "N");
                    Grid0.DataTable.Rows.Add();
                    i++;
                    oBar.Value = i;
                }
                oBar.Stop();
                Grid0.DataTable.Rows.Remove(Grid0.Rows.Count - 1);
                Grid0.AutoResizeColumns();
                UIAPIRawForm.Freeze(false);
            }
            catch (Exception ex)
            {
                Global.SetMessage(ex.Message, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                UIAPIRawForm.Freeze(false);
            }
        }
        private void PostSemesters()
        {
            Global.SetMessage("Posting Semesters. This can take few minutes", BoStatusBarMessageType.smt_Warning);
            int i = 0;
            UIAPIRawForm.Freeze(true);
            bool Error = false;

            foreach (Semesters semester in mySemesters)
            {
                bool AddError = false;
                AddOrUpdateSemester(semester, i, out AddError);
                if (AddError)
                {
                    Error = AddError;
                }
                i++;
            }
            Global.SetMessage("Add/Update of Semesters Completed" + (Error == true ? " With Some Errors" : ""), Error == true ? SAPbouiCOM.BoStatusBarMessageType.smt_Warning : SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            UIAPIRawForm.Freeze(false);
        }
        private void PostMajors()
        {
            Global.SetMessage("Posting Majors. This can take few minutes", BoStatusBarMessageType.smt_Warning);
            int i = 0;
            UIAPIRawForm.Freeze(true);
            bool Error = false;
            foreach (Majors major in myMajors)
            {
                bool AddError = false;
                AddOrUpdateMajor(major, i, out AddError);
                if (AddError)
                {
                    Error = AddError;
                }
                i++;
            }
            Global.SetMessage("Add/Update of Majors Completed" + (Error == true ? " With Some Errors" : ""), Error == true ? SAPbouiCOM.BoStatusBarMessageType.smt_Warning : SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            UIAPIRawForm.Freeze(false);
        }
        private void PostScholarships()
        {
            Global.SetMessage("Posting Scholarships. This can take few minutes", BoStatusBarMessageType.smt_Warning);
            int i = 0;
            UIAPIRawForm.Freeze(true);
            bool Error = false;
            foreach (Scholarships scholar in myScholarships)
            {
                bool AddError = false;
                AddOrUpdateScholarships(scholar, i, out AddError);
                if (AddError)
                {
                    Error = AddError;
                }
                i++;
            }
            Global.SetMessage("Add/Update of Scholarships Completed" + (Error == true ? " With Some Errors" : ""), Error == true ? SAPbouiCOM.BoStatusBarMessageType.smt_Warning : SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            UIAPIRawForm.Freeze(false);
        }
        private void PostColleges()
        {
            Global.SetMessage("Posting Colleges. This can take few minutes", BoStatusBarMessageType.smt_Warning);
            int i = 0;
            UIAPIRawForm.Freeze(true);
            bool Error = false;
            foreach (Colleges college in myColleges)
            {
                bool AddError = false;
                AddOrUpdateColleges(college, i, out AddError);
                if (AddError)
                {
                    Error = AddError;
                }
                i++;
            }
            Global.SetMessage("Add/Update of Colleges Completed" + (Error == true ? " With Some Errors" : ""), Error == true ? SAPbouiCOM.BoStatusBarMessageType.smt_Warning : SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            UIAPIRawForm.Freeze(false);
        }
        private void PostCourses()
        {
            int i = 0;
            UIAPIRawForm.Freeze(true);
            bool Error = false;
            Global.SetMessage("Please wait while Courses are being integrated. This can take a few minutes", BoStatusBarMessageType.smt_Warning);
            //ProgressBar oBar = Global.myApi.StatusBar.CreateProgressBar("Fetching Courses", myCourses.Count, true);
            foreach (Courses course in myCourses)
            {
                bool AddError = false;
                AddOrUpdateCourses(course, i, out AddError);
                if (AddError)
                {
                    Error = AddError;
                }
                //oBar.Value = i;
                i++;
            }
            //oBar.Stop();
            Global.SetMessage("Add/Update of Courses Completed" + (Error == true ? " With Some Errors" : ""), Error == true ? SAPbouiCOM.BoStatusBarMessageType.smt_Warning : SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            UIAPIRawForm.Freeze(false);
        }
        private void AddOrUpdateMajor(Majors major, int Row, out bool Error)
        {
            try
            {

                Recordset oRec = Global.Comp_DI.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
                oRec.DoQuery("Select 1 from OPRC where \"PrcCode\" = '" + major.Code + "'");
                if (oRec.RecordCount > 0)
                {
                    try
                    {
                        ProfitCentersService oPCService = Global.Comp_DI.GetCompanyService().GetBusinessService(ServiceTypes.ProfitCentersService) as ProfitCentersService;
                        ProfitCenterParams profitCenterParam = oPCService.GetDataInterface(ProfitCentersServiceDataInterfaces.pcsProfitCenterParams) as ProfitCenterParams;
                        profitCenterParam.CenterCode = major.Code;
                        ProfitCenter oPC = (ProfitCenter)oPCService.GetDataInterface(ProfitCentersServiceDataInterfaces.pcsProfitCenter);
                        oPC = oPCService.GetProfitCenter(profitCenterParam);
                        oPC.UserFields.Item("U_TrioName").Value = major.DescriptionEn;
                        oPC.UserFields.Item("U_TrioID").Value = major.ID.ToString();
                        oPCService.UpdateProfitCenter(oPC);
                        Grid0.DataTable.SetValue("Integrated", Row, "Y");
                        Grid0.DataTable.SetValue("Message", Row, "Successfully Updated");
                    }
                    catch (Exception ex)
                    {

                        Grid0.DataTable.SetValue("Message", Row, ex.Message);
                        Error = true;
                    }
                }
                else
                {
                    try
                    {
                        ProfitCentersService oPCService = Global.Comp_DI.GetCompanyService().GetBusinessService(ServiceTypes.ProfitCentersService) as ProfitCentersService;
                        ProfitCenter oPC = (ProfitCenter)oPCService.GetDataInterface(ProfitCentersServiceDataInterfaces.pcsProfitCenter);
                        oPC.CenterCode = major.Code;
                        oPC.CenterName = major.Code;
                        oPC.InWhichDimension = Convert.ToInt32(Config.MajorDim);
                        oPC.UserFields.Item("U_TrioName").Value = major.DescriptionEn;
                        oPC.UserFields.Item("U_TrioID").Value = major.ID.ToString();
                        oPC.Effectivefrom = new DateTime(2001, 1, 1);
                        ProfitCenterParams oPCParams = oPCService.AddProfitCenter(oPC);
                        Grid0.DataTable.SetValue("Integrated", Row, "Y");
                        Grid0.DataTable.SetValue("Message", Row, "Successfully Added");
                    }
                    catch (Exception ex)
                    {

                        Grid0.DataTable.SetValue("Message", Row, ex.Message);
                        Error = true;
                    }

                }
            }
            catch (Exception ex)
            {
                Grid0.DataTable.SetValue("Message", Row, ex.Message);
                Error = true;
            }

            Error = false;
        }
        private void AddOrUpdateScholarships(Scholarships scholar, int Row, out bool Error)
        {
            try
            {

                Recordset oRec = Global.Comp_DI.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
                oRec.DoQuery("Select 1 from OPRC where \"PrcCode\" = '" + scholar.Code + "'");
                if (oRec.RecordCount > 0)
                {
                    try
                    {
                        ProfitCentersService oPCService = Global.Comp_DI.GetCompanyService().GetBusinessService(ServiceTypes.ProfitCentersService) as ProfitCentersService;
                        ProfitCenterParams profitCenterParam = oPCService.GetDataInterface(ProfitCentersServiceDataInterfaces.pcsProfitCenterParams) as ProfitCenterParams;
                        profitCenterParam.CenterCode = scholar.Code;
                        ProfitCenter oPC = (ProfitCenter)oPCService.GetDataInterface(ProfitCentersServiceDataInterfaces.pcsProfitCenter);
                        oPC = oPCService.GetProfitCenter(profitCenterParam);
                        oPC.UserFields.Item("U_TrioName").Value = scholar.DescriptionEn;
                        oPC.UserFields.Item("U_GPA").Value = scholar.MinCGPA;
                        oPCService.UpdateProfitCenter(oPC);
                        Grid0.DataTable.SetValue("Integrated", Row, "Y");
                        Grid0.DataTable.SetValue("Message", Row, "Successfully Updated");
                    }
                    catch (Exception ex)
                    {
                        Grid0.DataTable.SetValue("Message", Row, ex.Message);
                        Error = true;
                    }
                }
                else
                {
                    try
                    {
                        ProfitCentersService oPCService = Global.Comp_DI.GetCompanyService().GetBusinessService(ServiceTypes.ProfitCentersService) as ProfitCentersService;
                        ProfitCenter oPC = (ProfitCenter)oPCService.GetDataInterface(ProfitCentersServiceDataInterfaces.pcsProfitCenter);
                        oPC.CenterCode = scholar.Code;
                        oPC.CenterName = scholar.Code;
                        oPC.InWhichDimension = Convert.ToInt32(Config.ScholarDim);
                        oPC.UserFields.Item("U_TrioName").Value = scholar.DescriptionEn;
                        oPC.UserFields.Item("U_TrioID").Value = scholar.ID.ToString();
                        oPC.UserFields.Item("U_GPA").Value = scholar.MinCGPA;
                        oPC.Effectivefrom = new DateTime(2001, 1, 1);
                        ProfitCenterParams oPCParams = oPCService.AddProfitCenter(oPC);
                        Grid0.DataTable.SetValue("Integrated", Row, "Y");
                        Grid0.DataTable.SetValue("Message", Row, "Successfully Added");
                    }
                    catch (Exception ex)
                    {
                        Grid0.DataTable.SetValue("Message", Row, ex.Message);
                        Error = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Grid0.DataTable.SetValue("Message", Row, ex.Message);
                Error = true;
            }
            try
            {
                UserTable tbSemester = Global.Comp_DI.UserTables.Item("OSHL");
                if (tbSemester.GetByKey(scholar.Code.ToString()))
                {
                    tbSemester.Name = scholar.DescriptionEn;
                    tbSemester.UserFields.Fields.Item("U_GPA").Value = scholar.MinCGPA;

                    if (tbSemester.Update() == 0)
                    {
                        Grid0.DataTable.SetValue("Integrated", Row, "Y");
                        Grid0.DataTable.SetValue("Message", Row, "Successfully Updated");
                        Error = false;
                    }
                    else
                    {
                        Grid0.DataTable.SetValue("Message", Row, Global.Comp_DI.GetLastErrorDescription());
                        Error = true;
                    }
                }
                else
                {
                    tbSemester.Code = scholar.Code.ToString();
                    tbSemester.Name = scholar.DescriptionEn;
                    tbSemester.UserFields.Fields.Item("U_SqlID").Value = scholar.ID.ToString();
                    tbSemester.UserFields.Fields.Item("U_GPA").Value = scholar.MinCGPA;

                    if (tbSemester.Add() == 0)
                    {
                        Grid0.DataTable.SetValue("Integrated", Row, "Y");
                        Grid0.DataTable.SetValue("Message", Row, "Successfully Added");
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
                Error = true;
            }
            Error = false;
        }
        private void AddOrUpdateSemester(Semesters semesters, int Row, out bool Error)
        {
            try
            {
                UserTable tbSemester = Global.Comp_DI.UserTables.Item("OSEM");
                if (tbSemester.GetByKey(semesters.ID.ToString()))
                {
                    tbSemester.UserFields.Fields.Item("U_Description").Value = semesters.DescriptionEn;
                    tbSemester.UserFields.Fields.Item("U_StartDate").Value = semesters.SemesterStartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    tbSemester.UserFields.Fields.Item("U_EndDate").Value = semesters.SemesterEndDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    tbSemester.UserFields.Fields.Item("U_Sequence").Value = semesters.Sequence.ToString();
                    tbSemester.UserFields.Fields.Item("U_SemesterType").Value = semesters.SemesterType;
                    if (tbSemester.Update() == 0)
                    {
                        Grid0.DataTable.SetValue("Integrated", Row, "Y");
                        Grid0.DataTable.SetValue("Message", Row, "Successfully Updated");
                        Error = false;
                    }
                    else
                    {
                        Grid0.DataTable.SetValue("Message", Row, Global.Comp_DI.GetLastErrorDescription());
                        Error = true;
                    }
                }
                else
                {
                    tbSemester.Code = semesters.ID.ToString();
                    tbSemester.Name = semesters.DescriptionEn;
                    tbSemester.UserFields.Fields.Item("U_Description").Value = semesters.DescriptionEn;
                    tbSemester.UserFields.Fields.Item("U_Sequence").Value = semesters.Sequence.ToString();
                    tbSemester.UserFields.Fields.Item("U_SemesterType").Value = semesters.SemesterType;
                    tbSemester.UserFields.Fields.Item("U_StartDate").Value = semesters.SemesterStartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    tbSemester.UserFields.Fields.Item("U_EndDate").Value = semesters.SemesterEndDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    if (tbSemester.Add() == 0)
                    {
                        Grid0.DataTable.SetValue("Integrated", Row, "Y");
                        Grid0.DataTable.SetValue("Message", Row, "Successfully Added");
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
                Error = true;
            }
        }
        private void AddOrUpdateColleges(Colleges college, int Row, out bool Error)
        {
            try
            {

                Recordset oRec = Global.Comp_DI.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
                oRec.DoQuery("Select 1 from OPRC where \"PrcCode\" = '" + college.Code + "'");
                if (oRec.RecordCount > 0)
                {
                    try
                    {
                        ProfitCentersService oPCService = Global.Comp_DI.GetCompanyService().GetBusinessService(ServiceTypes.ProfitCentersService) as ProfitCentersService;
                        ProfitCenterParams profitCenterParam = oPCService.GetDataInterface(ProfitCentersServiceDataInterfaces.pcsProfitCenterParams) as ProfitCenterParams;
                        profitCenterParam.CenterCode = college.Code;
                        ProfitCenter oPC = (ProfitCenter)oPCService.GetDataInterface(ProfitCentersServiceDataInterfaces.pcsProfitCenter);
                        oPC = oPCService.GetProfitCenter(profitCenterParam);
                        oPC.UserFields.Item("U_TrioName").Value = college.DescriptionEn;
                        oPCService.UpdateProfitCenter(oPC);
                        Grid0.DataTable.SetValue("Integrated", Row, "Y");
                        Grid0.DataTable.SetValue("Message", Row, "Successfully Updated");
                    }
                    catch (Exception ex)
                    {
                        Grid0.DataTable.SetValue("Message", Row, ex.Message);
                        Error = true;
                    }
                }
                else
                {
                    try
                    {
                        ProfitCentersService oPCService = Global.Comp_DI.GetCompanyService().GetBusinessService(ServiceTypes.ProfitCentersService) as ProfitCentersService;
                        ProfitCenter oPC = (ProfitCenter)oPCService.GetDataInterface(ProfitCentersServiceDataInterfaces.pcsProfitCenter);
                        oPC.CenterCode = college.Code;
                        oPC.CenterName = college.Code;
                        oPC.InWhichDimension = Convert.ToInt32(Config.CollegeDim);
                        oPC.UserFields.Item("U_TrioName").Value = college.DescriptionEn;
                        oPC.UserFields.Item("U_TrioID").Value = college.ID.ToString();
                        oPC.Effectivefrom = new DateTime(2001, 1, 1);
                        ProfitCenterParams oPCParams = oPCService.AddProfitCenter(oPC);
                        Grid0.DataTable.SetValue("Integrated", Row, "Y");
                        Grid0.DataTable.SetValue("Message", Row, "Successfully Added");
                    }
                    catch (Exception ex)
                    {
                        Grid0.DataTable.SetValue("Message", Row, ex.Message);
                        Error = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Grid0.DataTable.SetValue("Message", Row, ex.Message);
                Error = true;
            }
            try
            {
                UserTable tbCollege = Global.Comp_DI.UserTables.Item("OCOL");
                if (tbCollege.GetByKey(college.Code.ToString()))
                {
                    tbCollege.Name = college.DescriptionEn;
                    tbCollege.UserFields.Fields.Item("U_Description").Value = college.DescriptionEn;
                    if (tbCollege.Update() == 0)
                    {
                        Grid0.DataTable.SetValue("Integrated", Row, "Y");
                        Grid0.DataTable.SetValue("Message", Row, "Successfully Updated");
                        Error = false;
                    }
                    else
                    {
                        Grid0.DataTable.SetValue("Message", Row, Global.Comp_DI.GetLastErrorDescription());
                        Error = true;
                    }
                }
                else
                {
                    tbCollege.Code = college.Code.ToString();
                    tbCollege.Name = college.DescriptionEn;
                    tbCollege.UserFields.Fields.Item("U_ID").Value = college.ID.ToString();
                    tbCollege.UserFields.Fields.Item("U_Description").Value = college.DescriptionEn;
                    if (tbCollege.Add() == 0)
                    {
                        Grid0.DataTable.SetValue("Integrated", Row, "Y");
                        Grid0.DataTable.SetValue("Message", Row, "Successfully Added");
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
                Error = true;
            }
        }
        private void AddOrUpdateCourses(Courses course, int Row, out bool Error)
        {
            try
            {
                SAPbobsCOM.Items oItem = Global.Comp_DI.GetBusinessObject(BoObjectTypes.oItems) as SAPbobsCOM.Items;
                if (oItem.GetByKey(course.CourseCode.ToString()))
                {

                    oItem.ItemName = course.DescriptionEn;
                    oItem.UserFields.Fields.Item("U_Credits").Value = course.CreditHours.ToString();
                    oItem.UserFields.Fields.Item("U_PassMarks").Value = course.PassMark.ToString();
                    oItem.UserFields.Fields.Item("U_Scholarship").Value = course.Scholarship.ToString();
                    if (oItem.Update() == 0)
                    {
                        Grid0.DataTable.SetValue("Integrated", Row, "Y");
                        Grid0.DataTable.SetValue("Message", Row, "Successfully Updated");
                        Error = false;
                    }
                    else
                    {
                        Grid0.DataTable.SetValue("Message", Row, Global.Comp_DI.GetLastErrorDescription());
                        Error = true;
                    }
                }
                else
                {
                    oItem.Series = Config.ItemSeries;
                    oItem.ItemCode = course.CourseCode;
                    oItem.ItemName = course.DescriptionEn;
                    oItem.InventoryItem = BoYesNoEnum.tNO;
                    oItem.PurchaseItem = BoYesNoEnum.tNO;
                    oItem.UserFields.Fields.Item("U_ID").Value = course.ID.ToString();
                    oItem.UserFields.Fields.Item("U_Credits").Value = course.CreditHours.ToString();
                    oItem.UserFields.Fields.Item("U_PassMarks").Value = course.PassMark.ToString();
                    oItem.UserFields.Fields.Item("U_Scholarship").Value = course.Scholarship.ToString();
                    oItem.DefaultWarehouse = "ADM-001";
                    if (oItem.Add() == 0)
                    {
                        Grid0.DataTable.SetValue("Integrated", Row, "Y");
                        Grid0.DataTable.SetValue("Message", Row, "Successfully Added");
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
                Error = true;
            }
        }
        private void FormulateGrid()
        {
            Grid0.DataTable.Clear();
            switch (cmbObject.Value.ToString())
            {
                case "C":
                    Grid0.DataTable.Columns.Add("ID", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 11);
                    Grid0.DataTable.Columns.Add("Code", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
                    Grid0.DataTable.Columns.Add("Description", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 254);
                    Grid0.DataTable.Columns.Add("Credits", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 11);
                    Grid0.DataTable.Columns.Add("PassingMarks", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 11);
                    Grid0.DataTable.Columns.Add("Scholarship", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 1);
                    Grid0.Columns.Item("Scholarship").Type = SAPbouiCOM.BoGridColumnType.gct_CheckBox;
                    Grid0.DataTable.Columns.Add("Integrated", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 1);
                    Grid0.Columns.Item("Integrated").Type = SAPbouiCOM.BoGridColumnType.gct_CheckBox;
                    Grid0.DataTable.Columns.Add("Message", SAPbouiCOM.BoFieldsType.ft_Text);
                    break;
                case "S":
                    Grid0.DataTable.Columns.Add("ID", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 11);
                    Grid0.DataTable.Columns.Add("Description", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
                    Grid0.DataTable.Columns.Add("Sequence", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 11);
                    Grid0.DataTable.Columns.Add("SemesterType", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 50);
                    Grid0.DataTable.Columns.Add("StartDate", SAPbouiCOM.BoFieldsType.ft_Date, 10);
                    Grid0.DataTable.Columns.Add("EndDate", SAPbouiCOM.BoFieldsType.ft_Date, 10);
                    Grid0.DataTable.Columns.Add("Integrated", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 1);
                    Grid0.Columns.Item("Integrated").Type = SAPbouiCOM.BoGridColumnType.gct_CheckBox;
                    Grid0.DataTable.Columns.Add("Message", SAPbouiCOM.BoFieldsType.ft_Text);
                    break;
                case "M":
                case "H":
                case "L":
                    Grid0.DataTable.Columns.Add("ID", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 11);
                    Grid0.DataTable.Columns.Add("Code", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
                    Grid0.DataTable.Columns.Add("Description", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 254);
                    Grid0.DataTable.Columns.Add("GPA", SAPbouiCOM.BoFieldsType.ft_Float, 11);
                    Grid0.DataTable.Columns.Add("Integrated", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric, 1);
                    Grid0.Columns.Item("Integrated").Type = SAPbouiCOM.BoGridColumnType.gct_CheckBox;
                    Grid0.DataTable.Columns.Add("Message", SAPbouiCOM.BoFieldsType.ft_Text);
                    break;
              
            }
            Grid0.Item.Enabled = false;
            Grid0.DataTable.Rows.Add();
        }
        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }
        private SAPbouiCOM.Grid Grid0;
        private void OnCustomInitialize()
        {

        }

        private SAPbouiCOM.ComboBox cmbObject;
        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.Button btnFetch;
        private SAPbouiCOM.Button btnPost;
        private void FetchRecordsForIntegration()
        {
            switch (cmbObject.Selected.Value)
            {
                case "S": FetchSemesters(); break;
                case "M": FetchMajors(); break;
                case "H": FetchScholarships(); break;
                case "L": FetchColleges(); break;
                case "C": FetchCourses(); break;
            }
        }
        private void PostIntegrationData()
        {
            switch (cmbObject.Selected.Value)
            {
                case "S": PostSemesters(); break;
                case "M": PostMajors(); break;
                case "H": PostScholarships(); break;
                case "L": PostColleges(); break;
                case "C": PostCourses(); break;
            }
        }
        private void btnFetch_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            FetchRecordsForIntegration();
        }
        private void btnPost_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            PostIntegrationData();
        }
    }
}
