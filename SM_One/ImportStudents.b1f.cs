//using System.Windows.Forms;
using SAPbobsCOM;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SM_One
{
    [FormAttribute("SM_One.ImportStudents", "ImportStudents.b1f")]
    class ImportStudents : UserFormBase
    {
        public ImportStudents()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("stFile").Specific));
            this.txtFilePath = ((SAPbouiCOM.EditText)(this.GetItem("etFile").Specific));
            this.txtFilePath.KeyDownBefore += new SAPbouiCOM._IEditTextEvents_KeyDownBeforeEventHandler(this.txtFilePath_KeyDownBefore);
            this.txtFilePath.ClickBefore += new SAPbouiCOM._IEditTextEvents_ClickBeforeEventHandler(this.txtFilePath_ClickBefore);
            this.btnBrowse = ((SAPbouiCOM.Button)(this.GetItem("btBrowse").Specific));
            this.btnBrowse.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btnBrowse_PressedAfter);
            this.btnLoad = ((SAPbouiCOM.Button)(this.GetItem("btLoad").Specific));
            this.btnLoad.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btnLoad_PressedAfter);
            this.btnLoad.PressedBefore += new SAPbouiCOM._IButtonEvents_PressedBeforeEventHandler(this.btnLoad_PressedBefore);
            this.Grid0 = ((SAPbouiCOM.Grid)(this.GetItem("Grid_0").Specific));
            this.Grid0.LinkPressedAfter += new SAPbouiCOM._IGridEvents_LinkPressedAfterEventHandler(this.Grid0_LinkPressedAfter);
            this.Grid0.ClickAfter += new SAPbouiCOM._IGridEvents_ClickAfterEventHandler(this.Grid0_ClickAfter);
            this.Grid0.DoubleClickAfter += new SAPbouiCOM._IGridEvents_DoubleClickAfterEventHandler(this.Grid0_DoubleClickAfter);
            this.Grid0.PressedAfter += new SAPbouiCOM._IGridEvents_PressedAfterEventHandler(this.Grid0_PressedAfter);
            this.btnSelectAll = ((SAPbouiCOM.Button)(this.GetItem("btSelect").Specific));
            this.btnSelectAll.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btnSelectAll_PressedAfter);
            this.btnSelectAll.PressedBefore += new SAPbouiCOM._IButtonEvents_PressedBeforeEventHandler(this.btnSelectAll_PressedBefore);
            this.btnDeselectAll = ((SAPbouiCOM.Button)(this.GetItem("btDeselect").Specific));
            this.btnDeselectAll.PressedBefore += new SAPbouiCOM._IButtonEvents_PressedBeforeEventHandler(this.btnDeselectAll_PressedBefore);
            this.btnProceed = ((SAPbouiCOM.Button)(this.GetItem("btProceed").Specific));
            this.btnProceed.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btnProceed_PressedAfter);
            this.btnProceed.PressedBefore += new SAPbouiCOM._IButtonEvents_PressedBeforeEventHandler(this.btnProceed_PressedBefore);
            this.DTCSV = this.UIAPIRawForm.DataSources.DataTables.Item("DT_0");
            this.btnClear = ((SAPbouiCOM.Button)(this.GetItem("btClear").Specific));
            this.btnClear.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btnClear_PressedAfter);
            this.btnSelectAll.Item.Visible = false;
            this.btnDeselectAll.Item.Visible = false;
            this.btnRecheck = ((SAPbouiCOM.Button)(this.GetItem("btRecheck").Specific));
            this.btnRecheck.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btnRecheck_PressedAfter);
            this.btnRecheck.PressedBefore += new SAPbouiCOM._IButtonEvents_PressedBeforeEventHandler(this.btnRecheck_PressedBefore);
            //        this.txtRegNo.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.txtRegNo_ChooseFromListAfter);
            //        this.txtRegNo.ChooseFromListBefore += new SAPbouiCOM._IEditTextEvents_ChooseFromListBeforeEventHandler(this.txtRegNo_ChooseFromListBefore);
            //         this.txtRegNo.Item.Width = 0;
            this.btnRegNo = ((SAPbouiCOM.Button)(this.GetItem("btRegNo").Specific));
            this.btnRegNo.ChooseFromListAfter += new SAPbouiCOM._IButtonEvents_ChooseFromListAfterEventHandler(this.btnRegNo_ChooseFromListAfter);
            this.btnRegNo.ChooseFromListBefore += new SAPbouiCOM._IButtonEvents_ChooseFromListBeforeEventHandler(this.btnRegNo_ChooseFromListBefore);
            //       btnRegNo.Item.Visible = false;
            this.Grid0.AutoResizeColumns();
            this.Grid1 = ((SAPbouiCOM.Grid)(this.GetItem("Grid1").Specific));
            this.Grid0.CommonSetting.EnableArrowKey = true;
            this.Grid0.CommonSetting.FixedColumnsCount = 6;
            this.Grid0.Item.Description = "Import Grid";
            this.Grid1.Item.Description = "Differences Grid";
            this.btnDifferences = ((SAPbouiCOM.Button)(this.GetItem("btnDiff").Specific));
            this.btnDifferences.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btnDifferences_PressedAfter);
            this.cbUpdatePriceList = ((SAPbouiCOM.CheckBox)(this.GetItem("chUpdate").Specific));
            Global.myApi.MenuEvent += MyApi_MenuEvent;
            if (!Config.UpdatePriceList)
            {
                cbUpdatePriceList.Checked = false;
                cbUpdatePriceList.Item.Enabled = false;
            }
            else
            {
                cbUpdatePriceList.Item.Enabled = true;
            }
            this.OnCustomInitialize();

        }
        
        private void MyApi_MenuEvent(ref MenuEvent pVal, out bool BubbleEvent)
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
            this.ResizeAfter += new ResizeAfterHandler(this.Form_ResizeAfter);
            this.CloseAfter += new CloseAfterHandler(this.Form_CloseAfter);
        }
        private void Form_CloseAfter(SBOItemEventArg pVal)
        {
            Global.myApi.MenuEvent -= this.MyApi_MenuEvent;
        }
        private StaticText StaticText0;

        private void OnCustomInitialize()
        {

        }

        private EditText txtFilePath;
        private Button btnBrowse;
        private Button btnLoad;
        private Grid Grid0;
        private Button btnSelectAll;
        private Button btnDeselectAll;
        private Button btnProceed;
        private DataTable DTCSV;


        private List<string[]> ParseCSVtoList(string FilePath)
        {
            var reader = new CsvReader();
            string csv = File.ReadAllText(FilePath);
            var values = reader.Read(csv).ToList();
            return values;
        }

        private int CheckAndFetchTrailOrRetake(string StudentID, out string RegNo)
        {
            Recordset oRec = Global.Comp_DI.GetBusinessObject(BoObjectTypes.BoRecordset) as SAPbobsCOM.Recordset;

            try
            {
                oRec.DoQuery("Select count(*),T0.\"DocEntry\" from \"@SRG1\" T0 inner join \"@OSRG\" T1 on T0.\"DocEntry\" = T1.\"DocEntry\" where T1.\"U_StudentID\" = '" + StudentID + "' and T0.\"U_ToTrail\" = 'Y' and coalesce(T0.\"U_IsReTrailed\",'N') = 'N' group by T0.\"DocEntry\"");
                RegNo = oRec.Fields.Item("DocEntry").Value.ToString();
                return oRec.RecordCount;
            }
            finally
            {
                oRec = null;
            }
        }
        List<string> LstTrailNotFound = new List<string>();
        int RedColor = Color.FromArgb(235, 69, 95).R - 10;
        int GreenColor = Color.Green.R | (Color.Green.G << 8) | (Color.Green.B << 16);
        int WhiteColor = Color.White.R | (Color.White.G << 8) | (Color.White.B << 16);
        int BlueColor = Color.FromArgb(37, 150, 190).ToArgb() * -1;

        List<string[]> LstFromCSV = new List<string[]>();
        private DataTable ListToDataTable(List<string[]> lstCsv, out bool Success)
        {
            LstFromCSV.Clear();
            LstFromCSV = lstCsv;
            DTCSV.Rows.Clear();
            ProgressBar oBar = Global.myApi.StatusBar.CreateProgressBar("Importing Students", lstCsv.Count, true);
            try
            {
                Success = true;
                for (int i = 0; i < lstCsv.Count; i++)
                {
                    if (i == 0)
                    {
                        DTCSV.Rows.Add(lstCsv.Count - 1);
                    }
                    else
                    {
                        oBar.Text = "Importing " + i.ToString() + " out of " + lstCsv.Count.ToString();
                        DTCSV.SetValue("Select", i - 1, "Y");
                        DTCSV.SetValue("Registered", i - 1, "N");
                        DTCSV.SetValue("BPStatus", i - 1, "Pending");
                        DTCSV.SetValue("Retake", i - 1, "N");
                        for (int j = 0; j < lstCsv[i].Length; j++)
                        {
                            DTCSV.SetValue(lstCsv[0][j], i - 1, lstCsv[i][j]);
                        }
                    }
                    if (i != 0)
                    {
                        if (DTCSV.GetValue("Status", i - 1).ToString().ToLower() == "retake")//New Code for Retake
                        {
                            DTCSV.SetValue("Retake", i - 1, "Y");//New Code for Retake
                        }//New Code for Retake
                        if (DTCSV.GetValue("Retake", i - 1).ToString().ToLower() == "y" && DTCSV.GetValue("Trail", i - 1).ToString().ToLower() == "y")//New Code for Retake
                        {

                            oBar.Stop();//New Code for Retake
                            Global.SetMessage(DTCSV.GetValue("ID", i - 1).ToString() + " cannot have Trail and Retake at the same time. Re-Import all after correction", BoStatusBarMessageType.smt_Error);//New Code for Retake
                            Success = false;//New Code for Retake
                            return DTCSV;//New Code for Retake
                        }//New Code for Retake
                        if (DTCSV.GetValue("Trail", i - 1).ToString().ToLower() == "y")
                        {
                            string RegNo = "";
                            if (CheckAndFetchTrailOrRetake(DTCSV.GetValue("ID", i - 1).ToString(), out RegNo) > 0)
                            {
                                DTCSV.SetValue("TrailNo", i - 1, RegNo);
                            }
                            else
                            {
                                //DTCSV.SetValue("Trail", i - 1, "N");
                                if (!Config.AllowWithoutTrail)
                                {
                                    Grid0.CommonSetting.SetCellBackColor(i, 18, RedColor);
                                    Grid0.CommonSetting.SetCellBackColor(i, 19, RedColor);
                                }
                                LstTrailNotFound.Add(DTCSV.GetValue("ID", i - 1).ToString());
                            }
                        }
                    }
                    oBar.Value = i;
                }
                oBar.Stop();
                return DTCSV;
            }
            catch (Exception ex)
            {
                Global.SetMessage(ex.Message, BoStatusBarMessageType.smt_Error);
                oBar.Stop();
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oBar);
                oBar = null;
            }
            Success = true;//New Code for Retake
            return null;
        }


        private Button btnClear;
        bool Processed = false;

        //private void ClearUnselectedStudents(DataTable dt)
        //{
        //    ProgressBar oBar = Global.myApi.StatusBar.CreateProgressBar("Processing Student(s)", Grid0.Rows.Count, true);
        //    for (int i = 0; i < DTCSV.Rows.Count; i++)
        //    {
        //        if (DTCSV.GetValue("Select", i).ToString() == "Y")
        //        {
        //            DTSelected.Rows.Add();
        //            for (int k = 0; k < DTCSV.Columns.Count; k++)
        //            {
        //                DTSelected.SetValue(DTCSV.Columns.Item(k).Name, DTSelected.Rows.Count - 1, DTCSV.GetValue(DTCSV.Columns.Item(k).Name, i));
        //            }
        //        }
        //        oBar.Value = i;
        //    }
        //    oBar.Stop();
        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(oBar);
        //    oBar = null;
        //    Grid0.DataTable = DTSelected;

        //    //Grid0.Columns.Item(0).Type = BoGridColumnType.gct_CheckBox;
        //}

        //private void CheckForBusinessParnter()
        //{

        //}
        private bool ValidateBeforeRegisration(int Row)
        {
            Recordset oRec = Global.Comp_DI.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
            try
            {
                string Query = "Select \"U_StudentID\" from \"@OSRG\" where \"U_StudentID\" = '" + DTCSV.GetValue("ID", Row).ToString() + "' and \"U_Semester\" = '" + DTCSV.GetValue("Semester", Row).ToString() + "' and \"U_School\" = '" + DTCSV.GetValue("School", Row).ToString() + "' and \"U_Status\" = '" + DTCSV.GetValue("Status", Row).ToString() + "' and \"U_FiscalYear\" = " + DTCSV.GetValue("Fiscal Year", Row).ToString() + " and \"U_ProgramCode\" = '" + DTCSV.GetValue("Program", Row).ToString() + "'";
                oRec.DoQuery(Query);

                if (oRec.RecordCount > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oRec);
                oRec = null;
            }
        }
        private void CheckAndUpdateBusinessParnter()
        {
            ProgressBar oBar = Global.myApi.StatusBar.CreateProgressBar("Checking Student(s)", Grid0.Rows.Count, true);
            try
            {
                int GridCount = Grid0.Rows.Count;
                List<int> Removeable = new List<int>();
                for (int i = 0; i < GridCount; i++)
                {
                    if (DTCSV.GetValue("Select", i).ToString() == "Y" && !ValidateBeforeRegisration(i))
                    {
                        DTCSV.SetValue("Message", i, "Duplicate registration not allowed for the same combination of Student, Program, Semester, Fiscal Year, Status and School");
                        continue;
                    }
                    
                    if (DTCSV.GetValue("Select", i).ToString() == "Y")
                    {
                        CreateOrUpdateBusinessPartners(i);
                    }
                    else
                    {
                        Removeable.Add(i);
                    }
                    oBar.Text = "Updating " + i.ToString() + " out of " + GridCount.ToString();
                    oBar.Value = i;
                }
                oBar.Stop();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oBar);
                oBar = null;
                oBar = Global.myApi.StatusBar.CreateProgressBar("Removing Unselected Student(s)", Removeable.Count, true);
                for (int i = Removeable.Count - 1; i >= 0; i--)
                {
                    int Index = Removeable[i];
                    Grid0.CommonSetting.SetRowEditable(Index + 1, false);
                    oBar.Value = i;
                }
                oBar.Stop();

            }
            catch (Exception ex)
            {
                Global.SetMessage(ex.Message, BoStatusBarMessageType.smt_Error);
                oBar.Stop();
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oBar);
                oBar = null;
                UIAPIRawForm.Freeze(false);
            }
        }
        private void CreateOrUpdateBusinessPartners(int Row)
        {
            if (DTCSV.GetValue("Select", Row).ToString() == "Y")
            {
                BusinessPartners oBP = Global.Comp_DI.GetBusinessObject(BoObjectTypes.oBusinessPartners) as BusinessPartners;
                bool BPExists = false;
                bool DuplicateEntry = false;
                //Global.SetMessage("BP Code "+ DTCSV.GetValue("ID", Row).ToString(), BoStatusBarMessageType.smt_Error);
                if (oBP.GetByKey(DTCSV.GetValue("ID", Row).ToString()))
                {
                    BPExists = true;
                    //if (oBP.CardCode == DTCSV.GetValue("ID", Row).ToString()
                    //    && oBP.UserFields.Fields.Item("U_Semester").Value.ToString() == DTCSV.GetValue("Semester", Row).ToString()
                    //    && oBP.UserFields.Fields.Item("U_Program").Value.ToString() == DTCSV.GetValue("Program", Row).ToString()
                    //    && oBP.UserFields.Fields.Item("U_School").Value.ToString() == DTCSV.GetValue("School", Row).ToString()
                    //    && oBP.UserFields.Fields.Item("U_Status").Value.ToString() == DTCSV.GetValue("Status", Row).ToString()
                    //    && oBP.UserFields.Fields.Item("U_Joining_Year").Value.ToString() == DTCSV.GetValue("Fiscal Year", Row).ToString()
                    //    )
                    //{


                    //    DTCSV.SetValue("Message", Row, "This student is already registered for provided ");
                    //    return;

                    //}

                }
                else
                {
                    Global.SetMessage("BP Not found", BoStatusBarMessageType.smt_Error);
                    oBP.CardCode = DTCSV.GetValue("ID", Row).ToString();
                    oBP.GroupCode = Config.GroupCode;
                    oBP.UserFields.Fields.Item("U_YearJoined").Value = DTCSV.GetValue("Fiscal Year", Row).ToString();
                    try
                    {
                        oBP.PriceListNum = Config.PriceListMapping[DTCSV.GetValue("Fiscal Year", Row).ToString()];
                    }
                    catch
                    {

                        DTCSV.SetValue("Message", Row, "Price List for Fiscal Year not found");
                        return;
                    }
                }
                oBP.CardName = DTCSV.GetValue("Student Name", Row).ToString();
                oBP.EmailAddress = DTCSV.GetValue("Email", Row).ToString();
                oBP.Cellular = DTCSV.GetValue("Phone Number", Row).ToString();
                oBP.UserFields.Fields.Item("U_School").Value = DTCSV.GetValue("School", Row).ToString();
                oBP.UserFields.Fields.Item("U_Level").Value = DTCSV.GetValue("Level", Row).ToString();
                oBP.UserFields.Fields.Item("U_SL").Value = DTCSV.GetValue("Sub-Level", Row).ToString();
                oBP.UserFields.Fields.Item("U_Program").Value = DTCSV.GetValue("Program", Row).ToString();
                oBP.UserFields.Fields.Item("U_Sponsor").Value = DTCSV.GetValue("Sponsor", Row).ToString();
                oBP.UserFields.Fields.Item("U_Status").Value = DTCSV.GetValue("Status", Row).ToString();
                //oBP.UserFields.Fields.Item("U_Joining_Year").Value = new DateTime(Convert.ToInt32(DTCSV.GetValue("Fiscal Year", Row).ToString()), 1, 1); Joining Year Changes
                oBP.UserFields.Fields.Item("U_Joining_Year").Value = DTCSV.GetValue("Fiscal Year", Row).ToString();
                oBP.UserFields.Fields.Item("U_AY").Value = DTCSV.GetValue("Academic Year", Row).ToString();
                oBP.UserFields.Fields.Item("U_Semester").Value = DTCSV.GetValue("Semester", Row).ToString();
                oBP.Properties[63] = BoYesNoEnum.tYES;
                oBP.Properties[64] = BoYesNoEnum.tNO;
                if (BPExists)
                {
                    if (Config.UpdatePriceList && cbUpdatePriceList.Checked)
                    {
                        try
                        {
                            oBP.PriceListNum = Config.PriceListMapping[DTCSV.GetValue("Fiscal Year", Row).ToString()];
                        }
                        catch
                        {

                            DTCSV.SetValue("Message", Row, "Price List for Fiscal Year not found");
                            return;
                        }
                    }
                    DTCSV.SetValue("BP Code", Row, oBP.CardCode);

                    if (oBP.Update() != 0)
                    {
                        Global.SetMessage(oBP.CardCode + "-" + oBP.CardName + " : " + Global.Comp_DI.GetLastErrorDescription(), BoStatusBarMessageType.smt_Error);
                    }
                    else
                    {
                        Grid0.CommonSetting.SetCellBackColor(Row + 1, 21, BlueColor);
                        Grid0.CommonSetting.SetCellFontColor(Row + 1, 21, WhiteColor);
                        DTCSV.SetValue("BPStatus", Row, "Updated");
                    }
                }
                else
                {
                    if (oBP.Add() != 0)
                    {
                        Global.SetMessage(oBP.CardCode + "-" + oBP.CardName + " : " + Global.Comp_DI.GetLastErrorDescription(), BoStatusBarMessageType.smt_Error);
                    }
                    else
                    {
                        DTCSV.SetValue("BPStatus", Row, "Created");
                        DTCSV.SetValue("BP Code", Row, oBP.CardCode);
                        Grid0.CommonSetting.SetCellBackColor(Row + 1, 21, GreenColor);
                        Grid0.CommonSetting.SetCellFontColor(Row + 1, 21, WhiteColor);
                    }
                }
            }
        }

        GeneralService oGeneralService;
        GeneralData oHeader;
        CompanyService oCmpSrv;
        GeneralData oRowItem;
        GeneralDataCollection oRows;
        List<string> LstFinalizedTrails = new List<string>();
        List<string> LstFinalizedRetakes = new List<string>();
        private void GenerateStudentRegistration()
        {
            ProgressBar oBar = Global.myApi.StatusBar.CreateProgressBar("Registering Student(s)", Grid0.Rows.Count, true);
            LstFinalizedTrails.Clear();
            LstFinalizedRetakes.Clear();
            try
            {

                for (int i = 0; i < DTCSV.Rows.Count; i++)
                {
                    try
                    {
                        UIAPIRawForm.Freeze(true);
                        if (DTCSV.GetValue("Select", i).ToString() == "Y" && DTCSV.GetValue("Registered", i).ToString() == "N" && !string.IsNullOrEmpty(DTCSV.GetValue("BP Code", i).ToString()))
                        {
                            bool Registered = RegisterStudent(DTCSV.GetValue("BP Code", i).ToString(), DTCSV.GetValue("Student Name", i).ToString(), i, DTCSV.GetValue("TrailNo", i).ToString(), DTCSV.GetValue("Trail", i).ToString(), DTCSV.GetValue("Retake", i).ToString(), DTCSV.GetValue("Semester", i).ToString());
                            oBar.Text = "Processing " + i.ToString() + " out of " + Grid0.Rows.Count.ToString() + ". Registering " + DTCSV.GetValue("BP Code", i).ToString();
                            oBar.Value = i;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(DTCSV.GetValue("BP Code", i).ToString()))
                            {
                                DTCSV.SetValue("Message", i, "Cannot register without saving/updating student data");

                            }
                            Grid0.CommonSetting.SetRowEditable(i + 1, true);
                        }
                        UIAPIRawForm.Freeze(false);
                    }
                    catch (Exception ex)
                    {
                        Global.SetMessage(ex.Message, BoStatusBarMessageType.smt_Error);
                    }
                }
                Grid0.AutoResizeColumns();
                Global.SetMessage("Student Import Completed", BoStatusBarMessageType.smt_Success);

            }
            catch (Exception ex)
            {
                Global.SetMessage(ex.Message, BoStatusBarMessageType.smt_Error);
            }
            finally
            {
                UIAPIRawForm.Freeze(false);
                oBar.Stop();
            }
        }


        private bool RegisterStudent(string CardCode, string CardName, int RowNum, string TrailFrom, string Trail, string Retake, string Semester)
        {
            Recordset oRec = Global.Comp_DI.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
            string YearType = "";
            if (Config.PricedOn == "J")
            {
                YearType = "U_YearJoined";
            }
            else
            {
                YearType = "U_Joining_Year";
            }
            try
            {
                int AddToStart = 0;
                int AddToEnd = 0;
                if (Semester == "1" && Config.FiscalNotCalendar)
                {
                    AddToStart = Config.Semester1Start;
                    AddToEnd = Config.Semester1End;
                }
                else if (Semester == "2" && Config.FiscalNotCalendar)
                {
                    AddToStart = Config.Semester2Start;
                    AddToEnd = Config.Semester2End;
                }
                string Query = "";
                if (Global.Comp_DI.DbServerType == BoDataServerTypes.dst_HANADB)
                {
                    //HANA Trail (T5.""Price""*(CAST(T6.""U_Duration"" as float)/12))/(Select CASE WHEN T11.""U_CB"" = 'Credit' THEN sum(T10.""U_CH"") ELSE  count(T10.""LineId"") END AS ""Summation"" from ""@SCL1"" T10 inner join ""@OSCL"" T11 on T10.""DocEntry"" = T11.""DocEntry"" where T10.""U_RC"" ='Y' and T11.""U_PC"" = T0.""U_Program"" and T11.""U_FY"" = T0.""U_YearJoined"" and T10.""U_Level"" = T1.""U_Level"" group by T11.""U_CB"") as ""Price"",
                    //HANA Retake (T5.""Price""*(CAST(T3.""U_Duration"" as float)/12))/(Select CASE WHEN T11.""U_CB"" = 'Credit' THEN sum(T10.""U_CH"") ELSE  count(T10.""LineId"") END AS ""Summation"" from ""@SCL1"" T10 inner join ""@OSCL"" T11 on T10.""DocEntry"" = T11.""DocEntry"" where T10.""U_RC"" ='Y' and T11.""U_PC"" = T0.""U_Program"" and T11.""U_FY"" = T0.""U_YearJoined"" and T10.""U_Level"" =  T1.""U_Level""  group by T11.""U_CB"") as ""Price"",
                    
                    Query = @"Select T0.""CardCode"" as ""StudentID"",T0.""CardName"" as ""StudentName"",T0.""QryGroup63"" as ""DoesRegister"",T0.""QryGroup64"" as ""IsRegistered"",
T0.""U_Status"" as ""Status"", T0.""U_Level"" as ""Level"", T0.""U_SL"" as ""SubLevel"", T0.""U_School"" as ""School"",T0.""U_Joining_Year"" as ""JoiningYear"",
T0.""U_AY"" as ""AcademicYear"", T0.""U_Sponsor"" as ""Sponsor"", T0.""U_Program"" as ""ProgramCode"",T2.""U_SC"" as ""ItemCode"",T2.""U_SN"" as ""ItemName"", 
T3.""U_Month"" as ""StartMonth"",T3.""U_Day"" as ""StartDate"",T0.""U_Joining_Year"" as ""StartYear"",T3.""U_Duration"" as ""Duration"", T0.""U_Semester"",
TO_VARCHAR(TO_DATE((T0.""U_Joining_Year""+" + AddToStart.ToString() + ") || '" + Global._dateSeparator + @"' || T3.""U_Month"" || '" + Global._dateSeparator + @"' || T3.""U_Day"", 'YYYY" + Global._dateSeparator + @"MM" + Global._dateSeparator + @"DD'), 'MM-DD-YYYY') as ""StartingDate"", 
TO_VARCHAR(ADD_DAYS(ADD_MONTHS(TO_DATE((T0.""U_Joining_Year""+" + AddToEnd.ToString() + ") || '" + Global._dateSeparator + @"' || T3.""U_Month"" || '" + Global._dateSeparator + @"' || T3.""U_Day"", 'YYYY" + Global._dateSeparator + @"MM" + Global._dateSeparator + @"DD'), T3.""U_Duration""),T3.""U_Day""*-1), 'MM-DD-YYYY') as ""EndingDate"",
T2.""U_CH"" as ""Credits"", T5.""Price"" , 'N' as ""ToTrail"",'N' as ""ToRetake"",0 as ""TrailFrom"",0 as ""TrailLine"",T1.""U_CB"" as ""CalculationBase"",'A' as ""LineType""
from OCRD T0 inner join ""@OSCL"" T1 on T0.""U_Program"" = T1.""U_PC"" and T1.""U_FY"" = T0."""+ YearType + @""" and T1.""U_School"" = T0.""U_School""
inner join ""@SCL1"" T2 on T2.""DocEntry"" = T1.""DocEntry"" and T0.""U_Semester"" = T2.""U_Semester"" and T0.""U_SL"" = T2.""U_Level""
inner join ""@OSEM"" T3 on T3.""U_SubLevel"" = T2.""U_Level"" and T0.""U_Semester"" = T3.""U_Semester"" left join OPLN T4 on T4.""U_FiscalYear"" = T0."""+ YearType + @""" left join ITM1 T5 on T4.""ListNum"" = T5.""PriceList"" and T5.""ItemCode"" = T2.""U_SC"" left join ""@ONF1"" T7 on T7.""U_Status"" = T0.""U_Status"" where 'N' = '" + Retake + @"' and T0.""CardCode"" = '" + CardCode + @"' and T7.""U_Program"" = 'Y'

union all

Select T0.""CardCode"" as ""StudentID"",T0.""CardName"" as ""StudentName"",T0.""QryGroup63"" as ""DoesRegister"",T0.""QryGroup64"" as ""IsRegistered"",
T0.""U_Status"" as ""Status"", T0.""U_Level"" as ""Level"", T0.""U_SL"" as ""SubLevel"", T0.""U_School"" as ""School"",T0.""U_Joining_Year"" as ""JoiningYear"",
T0.""U_AY"" as ""AcademicYear"", T0.""U_Sponsor"" as ""Sponsor"", T0.""U_Program"" as ""ProgramCode"",T2.""U_SubjectCode"" as ""ItemCode"",T2.""U_SubjectName"" as ""ItemName"", 
0 as ""StartMonth"",0 as ""StartDate"",0 as ""StartYear"", 0 as ""Duration"", T0.""U_Semester"",
TO_VARCHAR(CURRENT_DATE,'" + Global._dateFormat + @"') as ""StartingDate"", 
TO_VARCHAR(CURRENT_DATE,'" + Global._dateFormat + @"') as ""EndingDate"",
T2.""U_Credits"" as ""Credits"", 

((T5.""Price""/(Select T10.""U_ProgType"" from ""@OSCL"" T10 where T10.""U_PC"" = T0.""U_Program"" and T10.""U_FY"" = T1.""U_FiscalYear""))*(Select count(DISTINCT T10.""U_Semester"") as ""Multiplication"" from ""@SCL1"" T10 inner join ""@OSCL"" T11 on T10.""DocEntry"" = T11.""DocEntry"" where T10.""U_RC"" ='Y' and T11.""U_PC"" = T0.""U_Program"" and T11.""U_FY"" = T1.""U_FiscalYear"" and T10.""U_Level"" = T1.""U_Level"" group by T11.""U_CB""))/(Select CASE WHEN T11.""U_CB"" = 'Credit' THEN sum(T10.""U_CH"") ELSE  count(T10.""LineId"") END AS ""Summation"" from ""@SCL1"" T10 inner join ""@OSCL"" T11 on T10.""DocEntry"" = T11.""DocEntry"" where T10.""U_RC"" ='Y' and T11.""U_PC"" = T0.""U_Program"" and T11.""U_FY"" = T1.""U_FiscalYear"" and T10.""U_Level"" = T1.""U_Level"" group by T11.""U_CB"") as ""Price"",

T2.""U_ToTrail"" as ""ToTrail"",T2.""U_ToRetake"" as ""ToRetake"",T2.""DocEntry"" as ""TrailFrom"",T2.""LineId"" as ""TrailLine"", (Select T11.""U_CB"" from ""@OSCL"" T11 where T11.""U_PC"" = T0.""U_Program"" and T11.""U_FY"" = T1.""U_FiscalYear"") as ""CalculationBase"",'M' as ""LineType""
from OCRD T0 inner join ""@OSRG"" T1 on T0.""CardCode"" = T1.""U_StudentID""  
inner join ""@SRG1"" T2 on T2.""DocEntry"" = T1.""DocEntry""
left join OPLN T4 on T4.""ListNum"" = T0.""ListNum"" 
left join ITM1 T5 on T4.""ListNum"" = T5.""PriceList"" and T5.""ItemCode"" = T0.""U_Program""
inner join ""@OSEM"" T6 on T6.""U_Semester"" = T0.""U_Semester"" and T6.""U_Level"" = T0.""U_Level"" and T6.""U_SubLevel"" = T0.""U_SL""
left join ""@ONF1"" T7 on T7.""U_Status"" = T0.""U_Status"" 
where  T1.""U_StudentID"" = '" + CardCode + @"' and T2.""U_ToTrail"" = 'Y' and T2.""U_IsReTrailed"" = 'N' and 'N' ='" + Retake + @"' and T2.""DocEntry"" = " + TrailFrom + @"

union all

Select T0.""CardCode"" as ""StudentID"",T0.""CardName"" as ""StudentName"",T0.""QryGroup63"" as ""DoesRegister"",T0.""QryGroup64"" as ""IsRegistered"",
T0.""U_Status"" as ""Status"", T0.""U_Level"" as ""Level"", T0.""U_SL"" as ""SubLevel"", T0.""U_School"" as ""School"",T0.""U_Joining_Year"" as ""JoiningYear"",
T0.""U_AY"" as ""AcademicYear"", T0.""U_Sponsor"" as ""Sponsor"", T0.""U_Program"" as ""ProgramCode"",T2.""U_SubjectCode"" as ""ItemCode"",T2.""U_SubjectName"" as ""ItemName"", 
T3.""U_Month"" as ""StartMonth"",T3.""U_Day"" as ""StartDate"",T1.""U_FiscalYear"" as ""StartYear"",T3.""U_Duration"" as ""Duration"", T0.""U_Semester"",
TO_VARCHAR(TO_DATE((T0.""U_Joining_Year""+" + AddToStart.ToString() + ")  || '" + Global._dateSeparator + @"' || T3.""U_Month"" || '" + Global._dateSeparator + @"' || T3.""U_Day"", 'YYYY" + Global._dateSeparator + @"MM" + Global._dateSeparator + @"DD'), 'MM-DD-YYYY') as ""StartingDate"", 
TO_VARCHAR(ADD_DAYS(ADD_MONTHS(TO_DATE((T0.""U_Joining_Year""+" + AddToEnd.ToString() + ") || '" + Global._dateSeparator + @"' || T3.""U_Month"" || '" + Global._dateSeparator + @"' || T3.""U_Day"", 'YYYY" + Global._dateSeparator + @"MM" + Global._dateSeparator + @"DD'), T3.""U_Duration""),T3.""U_Day""*-1), 'MM-DD-YYYY') as ""EndingDate"",
T2.""U_Credits"" as ""Credits"", 

((T5.""Price""/(Select T10.""U_ProgType"" from ""@OSCL"" T10 where T10.""U_PC"" = T0.""U_Program"" and T10.""U_FY"" = T1.""U_FiscalYear""))*(Select count(DISTINCT T10.""U_Semester"") as ""Multiplication"" from ""@SCL1"" T10 inner join ""@OSCL"" T11 on T10.""DocEntry"" = T11.""DocEntry"" where T10.""U_RC"" ='Y' and T11.""U_PC"" = T0.""U_Program"" and T11.""U_FY"" = T1.""U_FiscalYear"" and T10.""U_Level"" = T1.""U_Level"" group by T11.""U_CB""))/(Select CASE WHEN T11.""U_CB"" = 'Credit' THEN sum(T10.""U_CH"") ELSE  count(T10.""LineId"") END AS ""Summation"" from ""@SCL1"" T10 inner join ""@OSCL"" T11 on T10.""DocEntry"" = T11.""DocEntry"" where T10.""U_RC"" ='Y' and T11.""U_PC"" = T0.""U_Program"" and T11.""U_FY"" = T1.""U_FiscalYear"" and T10.""U_Level"" = T1.""U_Level"" group by T11.""U_CB"") as ""Price"",

'N' as ""ToTrail"",T2.""U_ToRetake"" as ""ToRetake"",T2.""DocEntry"" as ""TrailFrom"",T2.""LineId"" as ""TrailLine"",(Select T11.""U_CB"" from ""@OSCL"" T11 where T11.""U_PC"" = T0.""U_Program"" and T11.""U_FY"" = T1.""U_FiscalYear"") as ""CalculationBase"",'M' as ""LineType""
from OCRD T0 
inner join ""@OSRG"" T1 on T0.""U_Program"" = T1.""U_ProgramCode"" and T1.""U_StudentID"" = T0.""CardCode""
inner join ""@SRG1"" T2 on T2.""DocEntry"" = T1.""DocEntry"" 
inner join ""@OSEM"" T3 on T3.""U_SubLevel"" = T1.""U_Level"" and T0.""U_Semester"" = T3.""U_Semester"" 
left join OPLN T4 on T4.""ListNum"" = T0.""ListNum"" 
left join ITM1 T5 on T4.""ListNum"" = T5.""PriceList"" and T5.""ItemCode"" = T0.""U_Program"" left join ""@ONF1"" T7 on T7.""U_Status"" = T0.""U_Status"" where  T1.""U_StudentID"" = '" + CardCode + @"' and T2.""U_ToRetake"" = 'Y' and T2.""U_IsReTrailed"" = 'N'";

                    //(T5.""Price""/(Select T10.""U_ProgType"" from ""@OSCL"" T10 where T10.""U_PC"" = T0.""U_Program"" and T10.""U_FY"" = T1.""U_FiscalYear""))/(Select CASE WHEN T11.""U_CB"" = 'Credit' THEN sum(T10.""U_CH"") ELSE  count(T10.""LineId"") END AS ""Summation"" from ""@SCL1"" T10 inner join ""@OSCL"" T11 on T10.""DocEntry"" = T11.""DocEntry"" where T10.""U_RC"" ='Y' and T11.""U_PC"" = T0.""U_Program"" and T11.""U_FY"" = T1.""U_FiscalYear"" and T10.""U_Level"" = T1.""U_Level"" and T10.""U_Semester"" = T1.""U_Semester"" group by T11.""U_CB"") as ""Price"",
                    //(T5.""Price""/(Select T10.""U_ProgType"" from ""@OSCL"" T10 where T10.""U_PC"" = T0.""U_Program"" and T10.""U_FY"" = T1.""U_FiscalYear""))/(Select CASE WHEN T11.""U_CB"" = 'Credit' THEN sum(T10.""U_CH"") ELSE  count(T10.""LineId"") END AS ""Summation"" from ""@SCL1"" T10 inner join ""@OSCL"" T11 on T10.""DocEntry"" = T11.""DocEntry"" where T10.""U_RC"" ='Y' and T11.""U_PC"" = T0.""U_Program"" and T11.""U_FY"" = T1.""U_FiscalYear"" and T10.""U_Level"" = T1.""U_Level"" and T10.""U_Semester"" = T1.""U_Semester"" group by T11.""U_CB"") as ""Price"",

                }
                else
                {

                    Query = @"Select T0.""CardCode"" as ""StudentID"",T0.""CardName"" as ""StudentName"",T0.""QryGroup63"" as ""DoesRegister"",T0.""QryGroup64"" as ""IsRegistered"",
T0.""U_Status"" as ""Status"", T0.""U_Level"" as ""Level"", T0.""U_SL"" as ""SubLevel"", T0.""U_School"" as ""School"",T0.""U_Joining_Year"" as ""JoiningYear"",
T0.""U_AY"" as ""AcademicYear"", T0.""U_Sponsor"" as ""Sponsor"", T0.""U_Program"" as ""ProgramCode"",T2.""U_SC"" as ""ItemCode"",T2.""U_SN"" as ""ItemName"", 
T3.""U_Month"" as ""StartMonth"",T3.""U_Day"" as ""StartDate"",T0.""U_Joining_Year"" as ""StartYear"",T3.""U_Duration"" as ""Duration"", T0.""U_Semester"",
FORMAT(DATEFROMPARTS((T0.""U_Joining_Year""+" + AddToStart.ToString() + @"), T3.""U_Month"", T3.""U_Day""), '" + Global._dateFormat + @"') as ""StartingDate"", 
FORMAT(dateadd(day,T3.""U_Day""*-1,dateadd(month, T3.""U_Duration"", DATEFROMPARTS((T0.""U_Joining_Year""+" + AddToEnd.ToString() + @"), T3.""U_Month"", T3.""U_Day""))), '" + Global._dateFormat + @"') as ""EndingDate"",
T2.""U_CH"" as ""Credits"", T5.""Price"" ,  'N' as ""ToTrail"",'N' as ""ToRetake"",'' as ""TrailFrom"",null as ""TrailLine"",T1.""U_CB"" as ""CalculationBase"",'A' as ""LineType""
from OCRD T0 inner join ""@OSCL"" T1 on T0.""U_Program"" = T1.""U_PC"" and T1.""U_FY"" = T0.""" + YearType + @""" and T1.""U_School"" = T0.""U_School"" inner join ""@SCL1"" T2 on T2.""DocEntry"" = T1.""DocEntry"" and T0.""U_Semester"" = T2.""U_Semester"" and T0.""U_SL"" = T2.""U_Level"" 
inner join ""@OSEM"" T3 on T3.""U_SubLevel"" = T2.""U_Level"" and T0.""U_Semester"" = T3.""U_Semester"" left join OPLN T4 on T4.""U_FiscalYear"" = T0.""" + YearType + @""" left join ITM1 T5 on T4.""ListNum"" = T5.""PriceList"" and T5.""ItemCode"" = T2.""U_SC"" left join ""@ONF1"" T7 on T7.""U_Status"" = T0.""U_Status"" where 'N' = '" + Retake + @"' and T0.""CardCode"" = '" + CardCode + @"'

union all

Select T0.""CardCode"" as ""StudentID"",T0.""CardName"" as ""StudentName"",T0.""QryGroup63"" as ""DoesRegister"",T0.""QryGroup64"" as ""IsRegistered"",
T0.""U_Status"" as ""Status"", T0.""U_Level"" as ""Level"", T0.""U_SL"" as ""SubLevel"", T0.""U_School"" as ""School"",T0.""U_Joining_Year"" as ""JoiningYear"",
T0.""U_AY"" as ""AcademicYear"", T0.""U_Sponsor"" as ""Sponsor"", T0.""U_Program"" as ""ProgramCode"",T2.""U_SubjectCode"" as ""ItemCode"",T2.""U_SubjectName"" as ""ItemName"", 
0 as ""StartMonth"",0 as ""StartDate"",0 as ""StartYear"", 0 as ""Duration"", T0.""U_Semester"",
'' ""StartingDate"", 
'' as ""EndingDate"",
T2.""U_Credits"" as ""Credits"", 

((T5.""Price""/(Select T10.""U_ProgType"" from ""@OSCL"" T10 where T10.""U_PC"" = T0.""U_Program"" and T10.""U_FY"" = T1.""U_FiscalYear""))*(Select count(DISTINCT T10.""U_Semester"") as ""Multiplication"" from ""@SCL1"" T10 inner join ""@OSCL"" T11 on T10.""DocEntry"" = T11.""DocEntry"" where T10.""U_RC"" ='Y' and T11.""U_PC"" = T0.""U_Program"" and T11.""U_FY"" = T1.""U_FiscalYear"" and T10.""U_Level"" = T1.""U_Level"" group by T11.""U_CB""))/(Select CASE WHEN T11.""U_CB"" = 'Credit' THEN sum(T10.""U_CH"") ELSE  count(T10.""LineId"") END AS ""Summation"" from ""@SCL1"" T10 inner join ""@OSCL"" T11 on T10.""DocEntry"" = T11.""DocEntry"" where T10.""U_RC"" ='Y' and T11.""U_PC"" = T0.""U_Program"" and T11.""U_FY"" = T1.""U_FiscalYear"" and T10.""U_Level"" = T1.""U_Level"" group by T11.""U_CB"") as ""Price"",

T2.""U_ToTrail"" as ""ToTrail"",'N' as ""ToRetake"",T2.""DocEntry"" as ""TrailFrom"",T2.""LineId"" as ""TrailLine"",(Select T11.""U_CB"" from ""@OSCL"" T11 where T11.""U_PC"" = T0.""U_Program"" and T11.""U_FY"" = T0.""U_YearJoined"") as ""CalculationBase"",'M' as ""LineType""
from OCRD T0 inner join ""@OSRG"" T1 on T0.""CardCode"" = T1.""U_StudentID""
inner join ""@SRG1"" T2 on T2.""DocEntry"" = T1.""DocEntry"" left join OPLN T4 on T4.""ListNum"" = T0.""ListNum"" 
inner join ""@OSEM"" T6 on T6.""U_Semester"" = T0.""U_Semester"" and T6.""U_Level"" = T0.""U_Level"" and T6.""U_SubLevel"" = T0.""U_SL""
left join ""@ONF1"" T7 on T7.""U_Status"" = T0.""U_Status""
left join ITM1 T5 on T4.""ListNum"" = T5.""PriceList"" and T5.""ItemCode"" = T0.""U_Program"" where  T1.""U_StudentID"" = '" + CardCode + @"' and T2.""U_ToTrail"" = 'Y' and T2.""U_IsReTrailed"" = 'N' and 'N' ='" + Retake + @"' and T2.""DocEntry"" = " + TrailFrom + @"

union all

Select T0.""CardCode"" as ""StudentID"",T0.""CardName"" as ""StudentName"",T0.""QryGroup63"" as ""DoesRegister"",T0.""QryGroup64"" as ""IsRegistered"",
T0.""U_Status"" as ""Status"", T0.""U_Level"" as ""Level"", T0.""U_SL"" as ""SubLevel"", T0.""U_School"" as ""School"",T0.""U_Joining_Year"" as ""JoiningYear"",
T0.""U_AY"" as ""AcademicYear"", T0.""U_Sponsor"" as ""Sponsor"", T0.""U_Program"" as ""ProgramCode"",T2.""U_SubjectCode"" as ""ItemCode"",T2.""U_SubjectName"" as ""ItemName"", 
T3.""U_Month"" as ""StartMonth"",T3.""U_Day"" as ""StartDate"",T1.""U_FiscalYear"" as ""StartYear"",T3.""U_Duration"" as ""Duration"", T0.""U_Semester"",
FORMAT(DATEFROMPARTS((T0.""U_Joining_Year""+" + AddToStart.ToString() + @"), T3.""U_Month"", T3.""U_Day""), 'dd/MM/yyyy') as ""StartingDate"", 
FORMAT(dateadd(day,T3.""U_Day""*-1,dateadd(month, T3.""U_Duration"", DATEFROMPARTS((T0.""U_Joining_Year""+" + AddToEnd.ToString() + @"), T3.""U_Month"", T3.""U_Day""))), 'dd/MM/yyyy') as ""EndingDate"",
T2.""U_Credits"" as ""Credits"",

((T5.""Price""/(Select T10.""U_ProgType"" from ""@OSCL"" T10 where T10.""U_PC"" = T0.""U_Program"" and T10.""U_FY"" = T1.""U_FiscalYear""))*(Select count(DISTINCT T10.""U_Semester"") as ""Multiplication"" from ""@SCL1"" T10 inner join ""@OSCL"" T11 on T10.""DocEntry"" = T11.""DocEntry"" where T10.""U_RC"" ='Y' and T11.""U_PC"" = T0.""U_Program"" and T11.""U_FY"" = T1.""U_FiscalYear"" and T10.""U_Level"" = T1.""U_Level"" group by T11.""U_CB""))/(Select CASE WHEN T11.""U_CB"" = 'Credit' THEN sum(T10.""U_CH"") ELSE  count(T10.""LineId"") END AS ""Summation"" from ""@SCL1"" T10 inner join ""@OSCL"" T11 on T10.""DocEntry"" = T11.""DocEntry"" where T10.""U_RC"" ='Y' and T11.""U_PC"" = T0.""U_Program"" and T11.""U_FY"" = T1.""U_FiscalYear"" and T10.""U_Level"" = T1.""U_Level"" group by T11.""U_CB"") as ""Price"",

'N' as ""ToTrail"",T2.""U_ToRetake"" as ""ToRetake"",T2.""DocEntry"" as ""TrailFrom"",T2.""LineId"" as ""TrailLine"",(Select T11.""U_CB"" from ""@OSCL"" T11 where T11.""U_PC"" = T0.""U_Program"" and T11.""U_FY"" = T0.""U_YearJoined"") as ""CalculationBase"",'M' as ""LineType""
from OCRD T0 
inner join ""@OSRG"" T1 on T0.""U_Program"" = T1.""U_ProgramCode""  and T1.""U_StudentID"" = T0.""CardCode""
inner join ""@SRG1"" T2 on T2.""DocEntry"" = T1.""DocEntry"" 
inner join ""@OSEM"" T3 on T3.""U_SubLevel"" = T1.""U_Level"" and T0.""U_Semester"" = T3.""U_Semester"" 
left join OPLN T4 on T4.""ListNum"" = T0.""ListNum"" 
left join ""@ONF1"" T7 on T7.""U_Status"" = T0.""U_Status""
left join ITM1 T5 on T4.""ListNum"" = T5.""PriceList"" and T5.""ItemCode"" = T0.""U_Program"" where T1.""U_StudentID"" = '" + CardCode + @"' and T2.""U_ToRetake"" = 'Y' and T2.""U_IsReTrailed"" = 'N'";
                    
                }
                oRec.DoQuery(Query);
                if (oRec.RecordCount > 0)
                {
                    oCmpSrv = Global.Comp_DI.GetCompanyService();
                    oGeneralService = oCmpSrv.GetGeneralService("SRG");
                    oHeader = (SAPbobsCOM.GeneralData)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData);
                    oHeader.SetProperty("U_StudentID", oRec.Fields.Item("StudentID").Value);
                    oHeader.SetProperty("U_StudentName", oRec.Fields.Item("StudentName").Value);
                    oHeader.SetProperty("U_Level", oRec.Fields.Item("SubLevel").Value);
                    oHeader.SetProperty("U_ProgramCode", oRec.Fields.Item("ProgramCode").Value);
                    oHeader.SetProperty("U_FiscalYear", oRec.Fields.Item("StartYear").Value);
                    oHeader.SetProperty("U_AcademicYear", oRec.Fields.Item("AcademicYear").Value);
                    oHeader.SetProperty("U_StartDate", oRec.Fields.Item("StartingDate").Value);
                    oHeader.SetProperty("U_EndDate", oRec.Fields.Item("EndingDate").Value);
                    oHeader.SetProperty("U_Semester", oRec.Fields.Item("U_Semester").Value);
                    oHeader.SetProperty("U_Status", oRec.Fields.Item("Status").Value);
                    oHeader.SetProperty("U_School", oRec.Fields.Item("School").Value);
                    //oHeader.SetProperty("U_Type", "13");
                    oHeader.SetProperty("Remark", DTCSV.GetValue("Notes", RowNum).ToString());
                    oRows = oHeader.Child("SRG1");
                    while (!oRec.EoF)
                    {
                        oRowItem = oRows.Add();
                        oRowItem.SetProperty("U_SubjectCode", oRec.Fields.Item("ItemCode").Value);
                        oRowItem.SetProperty("U_SubjectName", oRec.Fields.Item("ItemName").Value);
                        oRowItem.SetProperty("U_Credits", oRec.Fields.Item("Credits").Value);
                        oRowItem.SetProperty("U_LineType", oRec.Fields.Item("LineType").Value);
                        if (oRec.Fields.Item("CalculationBase").Value.ToString() == "Credit")
                        {
                            oRowItem.SetProperty("U_Price", string.IsNullOrEmpty(oRec.Fields.Item("Price").Value.ToString()) == true ? "0" : (Convert.ToDouble(oRec.Fields.Item("Price").Value) * Convert.ToDouble(oRec.Fields.Item("Credits").Value)).ToString());
                            oRowItem.SetProperty("U_Total", string.IsNullOrEmpty(oRec.Fields.Item("Price").Value.ToString()) == true ? "0" : (Convert.ToDouble(oRec.Fields.Item("Price").Value) * Convert.ToDouble(oRec.Fields.Item("Credits").Value)).ToString());
                        }
                        else
                        {
                            oRowItem.SetProperty("U_Price", string.IsNullOrEmpty(oRec.Fields.Item("Price").Value.ToString()) == true ? "0" : oRec.Fields.Item("Price").Value);
                            oRowItem.SetProperty("U_Total", string.IsNullOrEmpty(oRec.Fields.Item("Price").Value.ToString()) == true ? "0" : oRec.Fields.Item("Price").Value);
                        }
                        oRowItem.SetProperty("U_ToTrail", "N");
                        oRowItem.SetProperty("U_IsReTrailed", "N");
                        if (oRec.Fields.Item("ToTrail").Value.ToString() == "Y" || oRec.Fields.Item("ToRetake").Value.ToString() == "Y")
                        {
                            oRowItem.SetProperty("U_FromReTrail", "Y");
                            oRowItem.SetProperty("U_ReTrailedFrom", oRec.Fields.Item("TrailFrom").Value.ToString());
                            oRowItem.SetProperty("U_ReTrailLine", oRec.Fields.Item("TrailLine").Value.ToString());
                            //27-02-2023 :
                            if (oRec.Fields.Item("ToTrail").Value.ToString() == "Y")
                            {
                                LstFinalizedTrails.Add(oRec.Fields.Item("TrailFrom").Value.ToString());
                            }
                            else
                            {
                                LstFinalizedRetakes.Add(oRec.Fields.Item("TrailFrom").Value.ToString());
                            }
                        }
                        oRec.MoveNext();
                    }
                    string Doc = oGeneralService.Add(oHeader).GetProperty("DocEntry").ToString();
                    if (!string.IsNullOrEmpty(Doc))
                    {
                        DTCSV.SetValue("Register No", RowNum, Doc);
                        DTCSV.SetValue("Registered", RowNum, "Y");
                        return true;
                    }
                    else
                    {
                        Global.SetMessage("No curriculum found for Student : " + CardCode + "-" + CardName, BoStatusBarMessageType.smt_Error);
                        DTCSV.SetValue("Message", RowNum, "Registration failed : No curriculum found");
                        return false;
                    }
                }
                else
                {
                    DTCSV.SetValue("Message", RowNum, "Student Registeration Failed. Check for Curriculum,Semester or Price List Configuration. In case of Trail or Retake, make sure to mark Trail/Retake subjects on previous registeration(s)");
                    return false;
                }
            }
            catch (Exception ex)
            {
                DTCSV.SetValue("Message", RowNum, "Unexpected Error cause registeration to fail");
                Global.SetMessage(ex.Message, BoStatusBarMessageType.smt_Error);
                return false;
            }
            finally
            {
                oGeneralService = null;
                oHeader = null;
                oCmpSrv = null;
                oRowItem = null;
                oRows = null;
                oRec = null;
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
            }
        }

        private void RemoveOrAddToTrail(string StudentID, string AddOrRemove, int RowIndex)
        {
            if (AddOrRemove == "R")
            {
                LstTrailNotFound.Remove(StudentID);
                Grid0.CommonSetting.SetCellBackColor(RowIndex + 1, 18, Grid0.CommonSetting.GetCellBackColor(RowIndex + 1, 3));
                Grid0.CommonSetting.SetCellBackColor(RowIndex + 1, 19, Grid0.CommonSetting.GetCellBackColor(RowIndex + 1, 3));
            }
            else
            {
                LstTrailNotFound.Add(StudentID);
                if (!Config.AllowWithoutTrail)
                {
                    Grid0.CommonSetting.SetCellBackColor(RowIndex + 1, 18, RedColor);
                    Grid0.CommonSetting.SetCellBackColor(RowIndex + 1, 19, RedColor);
                }

            }
        }

        private void UpdateAllTrails()
        {
            Recordset oRec = Global.Comp_DI.GetBusinessObject(BoObjectTypes.BoRecordset) as SAPbobsCOM.Recordset;
            string DocEntries = "";

            try
            {
                if (LstFinalizedTrails.Count > 0)
                {
                    for (int i = 0; i < LstFinalizedTrails.Count; i++)
                    {
                        DocEntries += LstFinalizedTrails[i] + ",";
                    }
                    DocEntries = DocEntries.TrimEnd(',');
                    string Query = @"Update T0 set T0.""U_IsReTrailed"" = 'Y',T0.""U_ReTrailedIn"" = T1.""DocEntry"" from ""@SRG1"" T0 inner join ""@SRG1"" T1 on T0.""DocEntry"" = T1.""U_ReTrailedFrom"" and T0.""LineId"" = T1.""U_ReTrailLine"" where T0.""U_ToTrail"" = 'Y' and T0.""DocEntry"" in (" + DocEntries + @")";
                    oRec.DoQuery(Query);
                }
                DocEntries = "";
                if (LstFinalizedRetakes.Count > 0)
                {

                    for (int i = 0; i < LstFinalizedRetakes.Count; i++)
                    {
                        DocEntries += LstFinalizedRetakes[i] + ",";
                    }
                    DocEntries = DocEntries.TrimEnd(',');
                    string Query = @"Update T0 set T0.""U_IsReTrailed"" = 'Y',T0.""U_ReTrailedIn"" = T1.""DocEntry"" from ""@SRG1"" T0 inner join ""@SRG1"" T1 on T0.""DocEntry"" = T1.""U_ReTrailedFrom"" and T0.""LineId"" = T1.""U_ReTrailLine"" where T0.""U_ToRetake"" = 'Y' and T0.""DocEntry"" in (" + DocEntries + @")";
                    oRec.DoQuery(Query);
                }

            }
            catch (Exception ex)
            {
                Global.SetMessage(ex.Message, BoStatusBarMessageType.smt_Error);
            }
        }

        private Button btnRecheck;

        #region FormEvents
        private void btnLoad_PressedBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (string.IsNullOrEmpty(txtFilePath.Value))
            {
                Global.SetMessage("Please select a file", BoStatusBarMessageType.smt_Error);
                BubbleEvent = false;
            }
            if (!File.Exists(txtFilePath.Value))
            {
                Global.SetMessage("Cannot find the selected file", BoStatusBarMessageType.smt_Error);
                BubbleEvent = false;
            }
            else if (txtFilePath.Value.Substring(txtFilePath.Value.Length-3).ToLower() != "csv")
            {
                Global.SetMessage("Selected file is not a CSV", BoStatusBarMessageType.smt_Error);
                BubbleEvent = false;
            }
            else if (File.ReadAllLines(txtFilePath.Value).First() != "#,ID,Student Name,Status,School,Level,Sub-Level,Program,Sponsor,Academic Year,Semester,Phone Number,Email,Fiscal Year,Notes,Trail")
            {
                Global.SetMessage("Invalid file template", BoStatusBarMessageType.smt_Error);
                BubbleEvent = false;
            }
        }
        private void btnLoad_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                UIAPIRawForm.Freeze(true);
                DTCSV.Rows.Clear();
                Grid0.Item.Refresh();
                bool Success = false;
                Grid0.DataTable = ListToDataTable(ParseCSVtoList(txtFilePath.Value), out Success);
                Grid0.Columns.Item(0).Type = BoGridColumnType.gct_CheckBox;
                Grid0.Columns.Item(1).Type = BoGridColumnType.gct_CheckBox;
                Grid0.Columns.Item("Trail").Type = BoGridColumnType.gct_CheckBox;
                Grid0.Columns.Item("Retake").Type = BoGridColumnType.gct_CheckBox;
                SAPbouiCOM.EditTextColumn oCol = Grid0.Columns.Item("ID") as EditTextColumn;
                oCol.LinkedObjectType = "2";
                oCol = Grid0.Columns.Item("Register No") as EditTextColumn;
                oCol.LinkedObjectType = "SRG";
                File.Delete(txtFilePath.Value);
                txtFilePath.Value = "";
                Grid0.AutoResizeColumns();
                Grid0.Item.Enabled = false;
                UIAPIRawForm.Freeze(false);
            }
            catch (Exception ex)
            {
                Global.SetMessage(ex.Message, BoStatusBarMessageType.smt_Error);

            }
            finally
            {
                UIAPIRawForm.Freeze(false);
            }
        }
        private void btnProceed_PressedBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (DTCSV.IsEmpty)
            {
                Global.SetMessage("Please select a file to process", BoStatusBarMessageType.smt_Error);
                BubbleEvent = false;
            }
            else if (LstTrailNotFound.Count > 0 && !Config.AllowWithoutTrail)
            {
                Global.SetMessage("Update Prior and Recheck trail for Student(s) highlighted in (Red)", BoStatusBarMessageType.smt_Error);
                BubbleEvent = false;
            }
        }
        private void btnProceed_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (UIAPIRawForm.PaneLevel == 1)
                {
                    if (Global.myApi.MessageBox("Are you sure you want to process selected student(s)?\nNote: This action is irreversible", 1, "Yes", "No") == 1)
                    {
                        UIAPIRawForm.Freeze(true);
                        CheckAndUpdateBusinessParnter();
                        Global.SetMessage("Student Import Completed. You can now initiate Student Registration", BoStatusBarMessageType.smt_Success);
                        Grid0.Columns.Item(0).Type = BoGridColumnType.gct_CheckBox;
                        Grid0.Columns.Item(1).Type = BoGridColumnType.gct_CheckBox;
                        UIAPIRawForm.PaneLevel = 2;
                        btnProceed.Caption = "Register";
                    }
                }
                else if (UIAPIRawForm.PaneLevel == 2)
                {
                    if (Global.myApi.MessageBox("Are you sure you want to complete registration of selected student(s)?\nNote: This action is irreversible", 1, "Yes", "No") == 1)
                    {
                        GenerateStudentRegistration();
                        if (LstFinalizedTrails.Count > 0 || LstFinalizedRetakes.Count > 0)
                        {
                            UpdateAllTrails();
                        }
                        LstFinalizedTrails.Clear();
                        UIAPIRawForm.PaneLevel = 3;
                        btnProceed.Caption = "Complete";
                    }
                }
                else if (UIAPIRawForm.PaneLevel == 3)
                {
                    LstTrailNotFound.Clear();
                    LstFinalizedTrails.Clear();
                    Grid0.DataTable.Rows.Clear();
                    UIAPIRawForm.PaneLevel = 1;
                    txtFilePath.Value = "";
                    btnProceed.Caption = "Process";
                }
            }
            catch (Exception ex)
            {
                Global.SetMessage(ex.Message, BoStatusBarMessageType.smt_Error);

            }
            finally
            {
                UIAPIRawForm.Freeze(false);
            }

        }
        private void btnBrowse_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                //Global.CreateDialogAndFileValue(txtFilePath, "CSV (Comma Delimited)|*.csv");
                if (!string.IsNullOrEmpty(txtFilePath.Value))
                {
                    txtFilePath.Value = "";
                }
                txtFilePath.Item.Click(BoCellClickType.ct_Double);
            }
            catch (Exception ex)
            {
                Global.SetMessage(ex.Message, BoStatusBarMessageType.smt_Error);
            }
        }
        private void btnClear_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (!Grid0.DataTable.IsEmpty && !Processed)
            {
                DTCSV.Rows.Clear();
            }

        }
        private void btnSelectAll_PressedBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (DTCSV.IsEmpty)
            {
                Global.SetMessage("No data to select/deselect", BoStatusBarMessageType.smt_Error);
            }
        }
        private void Grid0_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                bool iseditable = false;
                if (pVal.Row > -1)
                {
                    iseditable = Grid0.CommonSetting.GetCellEditable(pVal.Row + 1, 1);
                }
                if ((pVal.ColUID == "Select" || pVal.ColUID == "Trail") && pVal.Row > -1 && iseditable)
                {
                    UIAPIRawForm.Freeze(true);
                    string YesOrNo = Grid0.DataTable.GetValue(pVal.ColUID, pVal.Row).ToString();
                    DTCSV.SetValue(pVal.ColUID, pVal.Row, YesOrNo == "N" ? "Y" : "N");
                    string SelectDeselect = Grid0.DataTable.GetValue("Select", pVal.Row).ToString();
                    string IsTrail = DTCSV.GetValue("Trail", pVal.Row).ToString();
                    string TrailNo = DTCSV.GetValue("TrailNo", pVal.Row).ToString();
                    string StudentID = DTCSV.GetValue("ID", pVal.Row).ToString();
                    if (!Config.AllowWithoutTrail)
                    {
                        if (SelectDeselect == "Y")
                        {
                            if (IsTrail == "Y" && TrailNo != "0")
                            {
                                RemoveOrAddToTrail(StudentID, "R", pVal.Row);

                            }
                            else if (IsTrail == "N")
                            {
                                RemoveOrAddToTrail(StudentID, "R", pVal.Row);

                            }
                            else
                            {

                                RemoveOrAddToTrail(StudentID, "A", pVal.Row);
                            }
                        }
                        else
                        {
                            RemoveOrAddToTrail(StudentID, "R", pVal.Row);
                        }
                    }
                    //else//Not Alllow Without Trail
                    //{

                    //}
                    //if ((IsTrail == "N" && SelectDeselect == "Y" && TrailNo == "0") && !Config.AllowWithoutTrail) 
                    //{
                    //    RemoveOrAddToTrail(StudentID, "R", pVal.Row);
                    //}
                    //else if ((IsTrail == "N" || SelectDeselect == "N" || TrailNo == "0") && !Config.AllowWithoutTrail)
                    //{
                    //    RemoveOrAddToTrail(StudentID, "R", pVal.Row);
                    //}
                    //else if ((IsTrail == "Y" && SelectDeselect == "Y" && TrailNo != "0") && !Config.AllowWithoutTrail)
                    //{
                    //    RemoveOrAddToTrail(StudentID, "R", pVal.Row);
                    //}

                    UIAPIRawForm.Freeze(false);
                }


            }
            catch (Exception ex)
            {

                Global.myApi.SetStatusBarMessage(ex.Message, BoMessageTime.bmt_Short, true);
                UIAPIRawForm.Freeze(false);
            }

        }
        private void btnDeselectAll_PressedBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {

            BubbleEvent = true;
            if (DTCSV.IsEmpty)
            {
                Global.SetMessage("No data to select/deselect", BoStatusBarMessageType.smt_Error);
            }

        }
        private void btnSelectAll_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {

        }
        private void Grid0_DoubleClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (pVal.Row > -1 && pVal.ColUID == "RowsHeader" && (UIAPIRawForm.PaneLevel == 1 || UIAPIRawForm.PaneLevel == 2))
            {
                //GlobalStudentID = DTCSV.GetValue("ID", pVal.Row).ToString();
                btnRegNo.Item.Click();
                //string RegisterationNo = DTCSV.GetValue("Register No", pVal.Row).ToString();
                //string Registered = DTCSV.GetValue("Registered", pVal.Row).ToString();
                //string Trail = DTCSV.GetValue("Trail", pVal.Row).ToString();
                //string TrailNo = DTCSV.GetValue("TrailNo", pVal.Row).ToString();
                //string StudentID = DTCSV.GetValue("ID", pVal.Row).ToString();
                //if (Registered == "Y")
                //{
                //    Registration active = new Registration();
                //    active.UIAPIRawForm.Mode = BoFormMode.fm_FIND_MODE;
                //    active.txtDocEntry.Value = RegisterationNo;
                //    active.btnSave.Item.Click();
                //    active.Show();
                //}
                //else if (Trail == "Y" && TrailNo != "0")
                //{
                //    Registration active = new Registration();
                //    active.UIAPIRawForm.Mode = BoFormMode.fm_FIND_MODE;
                //    active.txtDocEntry.Value = TrailNo;
                //    active.UIAPIRawForm.Title = "Student Registration (Trail From " + TrailNo + ")";
                //    active.btnSave.Item.Click();
                //    active.Show();
                //}
                //else
                //{
                //    Global.myApi.OpenForm(BoFormObjectEnum.fo_BusinessPartner, "", StudentID);
                //}
            }
            else if (UIAPIRawForm.PaneLevel == 3)
            {
                string RegisterationNo = DTCSV.GetValue("Register No", pVal.Row).ToString();
                string Registered = DTCSV.GetValue("Registered", pVal.Row).ToString();
                string Trail = DTCSV.GetValue("Trail", pVal.Row).ToString();
                string TrailNo = DTCSV.GetValue("TrailNo", pVal.Row).ToString();
                string StudentID = DTCSV.GetValue("ID", pVal.Row).ToString();
                if (Registered == "Y")
                {
                    Registration active = new Registration();
                    active.UIAPIRawForm.Mode = BoFormMode.fm_FIND_MODE;
                    active.txtDocEntry.Value = RegisterationNo;
                    active.btnSave.Item.Click();
                    active.Show();
                }
            }

        }
        private void btnRecheck_PressedBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (Grid0.Rows.SelectedRows.Count == 0)
            {
                Global.SetMessage("Select Student to Recheck Trail", BoStatusBarMessageType.smt_Warning);
                BubbleEvent = false;
            }
        }
        private void btnRecheck_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            int SelectedRowIndex = Grid0.Rows.SelectedRows.Item(0, BoOrderType.ot_SelectionOrder);
            if (DTCSV.GetValue("Trail", SelectedRowIndex).ToString().ToLower() == "y")
            {
                string RegNo = "";
                if (CheckAndFetchTrailOrRetake(DTCSV.GetValue("ID", SelectedRowIndex).ToString(), out RegNo) > 0)
                {
                    DTCSV.SetValue("TrailNo", SelectedRowIndex, RegNo);
                    Grid0.CommonSetting.SetCellBackColor(SelectedRowIndex + 1, 18, Grid0.CommonSetting.GetCellBackColor(SelectedRowIndex + 1, 3));
                    Grid0.CommonSetting.SetCellBackColor(SelectedRowIndex + 1, 19, Grid0.CommonSetting.GetCellBackColor(SelectedRowIndex + 1, 3));
                    LstTrailNotFound.Remove(DTCSV.GetValue("ID", SelectedRowIndex).ToString());
                }
                else
                {
                    Global.SetMessage("No Subjects to Trail", BoStatusBarMessageType.smt_Error);
                    DTCSV.SetValue("TrailNo", SelectedRowIndex, "0");
                    RemoveOrAddToTrail(DTCSV.GetValue("ID", SelectedRowIndex).ToString(), "A", SelectedRowIndex);
                }
            }
        }

        #endregion

        private void txtFilePath_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (!pVal.InnerEvent)
            {
                BubbleEvent = false;
            }

        }

        private void txtFilePath_KeyDownBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (!pVal.InnerEvent)
            {
                BubbleEvent = false;
            }

        }
        SAPbouiCOM.Conditions Cons;
        string GlobalStudentID = "";
        private void btnRegNo_ChooseFromListBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            if (Grid0.Rows.SelectedRows.Count > 0)
            {
                GlobalStudentID = DTCSV.GetValue("ID", Grid0.Rows.SelectedRows.Item(0, BoOrderType.ot_RowOrder)).ToString();
                BubbleEvent = true;
                string CFL = "CFL_0";
                Cons = null;
                UIAPIRawForm.ChooseFromLists.Item(CFL).SetConditions(Cons);
                Cons = UIAPIRawForm.ChooseFromLists.Item(CFL).GetConditions();
                SAPbouiCOM.Condition con = Cons.Add();
                con.Alias = "U_StudentID";
                con.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                con.CondVal = GlobalStudentID;
                //con.Relationship = BoConditionRelationship.cr_AND;
                //con = Cons.Add();
                //con.Alias = "U_Invoiced";
                //con.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                //con.CondVal = "Y";
                UIAPIRawForm.ChooseFromLists.Item(CFL).SetConditions(Cons);
            }
            else
            {
                BubbleEvent = false;
            }
        }

        private void btnRegNo_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            ISBOChooseFromListEventArg args = (ISBOChooseFromListEventArg)pVal;
            if (args.SelectedObjects != null)
            {
                btnRegNo.Item.Click();
                string DocEntry = args.SelectedObjects.GetValue("DocEntry", 0).ToString();
                Registration active = new Registration(true, (SAPbouiCOM.Form)UIAPIRawForm);
                active.UIAPIRawForm.Mode = BoFormMode.fm_FIND_MODE;
                active.txtDocEntry.Value = DocEntry;
                active.btnSave.Item.Click();
                active.Show();

            }

        }

        private Button btnRegNo;

        private void Form_ResizeAfter(SBOItemEventArg pVal)
        {
            btnBrowse.Item.Left = txtFilePath.Item.Left + txtFilePath.Item.Width + 5;
            btnRegNo.Item.Left = btnBrowse.Item.Left + btnBrowse.Item.Width + 5;
            Grid1.Item.Top = Grid0.Item.Top + Grid0.Item.Height + 5;
            if (Grid0.Item.FontSize > 14)
            {
                Grid1.Item.Height = Convert.ToInt32(Grid0.Item.Height * .23);
            }
            else
            {
                Grid1.Item.Height = Convert.ToInt32(Grid0.Item.Height * .17);
            }
            btnProceed.Item.Top = Grid1.Item.Top + Grid1.Item.Height + 5;
            btnRecheck.Item.Top = Grid1.Item.Top + Grid1.Item.Height + 5;
            btnDifferences.Item.Top = Grid1.Item.Top + Grid1.Item.Height + 5;
        }

        private void Grid0_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                UIAPIRawForm.Freeze(true);

                if (pVal.ColUID == "RowsHeader" && pVal.Row > -1)
                {
                    string Query = @"Select T0.""CardCode"" as ""StudentID"",T0.""CardName"" as ""Student Name"",T0.""U_Status"" as ""Status"",
T0.""U_School"" as ""School"", T0.""U_Level"" as ""Level"", T0.""U_SL"" as ""SubLevel"",
T0.""U_Program"" as ""ProgramCode"",T0.""U_Sponsor"" as ""Sponsor"",T0.""U_AY"" as ""AcademicYear"", 
T0.""Cellular"" as ""Phone Number"",T0.""E_Mail"" as ""Email"", T0.""U_Joining_Year"" as ""Fiscal Year"", 'Current' as ""Data"" 
                from OCRD T0 where ""CardCode"" = '" + DTCSV.GetValue("ID", pVal.Row).ToString() + @"'

union all 

Select '" + DTCSV.GetValue("ID", pVal.Row).ToString() + @"' as ""ID"",
'" + DTCSV.GetValue("Student Name", pVal.Row).ToString() + @"' as ""Student Name"",
'" + DTCSV.GetValue("Status", pVal.Row).ToString() + @"' as ""Status"",
'" + DTCSV.GetValue("School", pVal.Row).ToString() + @"' as ""School"",
'" + DTCSV.GetValue("Level", pVal.Row).ToString() + @"' as ""Level"",
'" + DTCSV.GetValue("Sub-Level", pVal.Row).ToString() + @"' as ""Sub-Level"",
'" + DTCSV.GetValue("Program", pVal.Row).ToString() + @"' as ""Program"",
'" + DTCSV.GetValue("Sponsor", pVal.Row).ToString() + @"' as ""Sponsor"",
'" + DTCSV.GetValue("Academic Year", pVal.Row).ToString() + @"' as ""Academic Year"",
'" + DTCSV.GetValue("Phone Number", pVal.Row).ToString() + @"' as ""Phone Number"",
'" + DTCSV.GetValue("Email", pVal.Row).ToString() + @"' as ""Email"",
'" + DTCSV.GetValue("Fiscal Year", pVal.Row).ToString() + @"' as ""Fiscal Year"",
'New' as ""Data""" + (Global.Comp_DI.DbServerType == BoDataServerTypes.dst_HANADB ? " from dummy" : "");
                    Grid1.DataTable.ExecuteQuery(Query);
                }
                CompareData();
                UIAPIRawForm.Freeze(false);
            }
            catch (Exception ex)
            {
                Global.SetMessage(ex.Message, BoStatusBarMessageType.smt_Error);
                UIAPIRawForm.Freeze(true);
            }
        }
        private void CompareData()
        {
            if (Grid1.Rows.Count > 1)
            {
                for (int i = 1; i < Grid1.Columns.Count; i++)
                {
                    if (Grid1.DataTable.GetValue(i, 0).ToString() != Grid1.DataTable.GetValue(i, 1).ToString())
                    {
                        Grid1.CommonSetting.SetCellBackColor(2, i + 1, BlueColor);
                        Grid1.CommonSetting.SetCellFontColor(2, i + 1, WhiteColor);
                    }
                    else
                    {
                        Grid1.CommonSetting.SetCellBackColor(2, i + 1, Grid0.CommonSetting.GetCellBackColor(1, 1));
                        Grid1.CommonSetting.SetCellFontColor(2, i + 1, Grid0.CommonSetting.GetCellFontColor(1, 1));
                    }
                }
            }
        }
        private Grid Grid1;

        private void Grid0_LinkPressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (pVal.ColUID == "Register No")
            {
                string RegisterationNo = DTCSV.GetValue("Register No", pVal.Row).ToString();
                string Registered = DTCSV.GetValue("Registered", pVal.Row).ToString();
                //string Trail = DTCSV.GetValue("Trail", pVal.Row).ToString();
                //string TrailNo = DTCSV.GetValue("TrailNo", pVal.Row).ToString();
                string StudentID = DTCSV.GetValue("ID", pVal.Row).ToString();
                if (Registered == "Y")
                {
                    Registration active = new Registration();
                    active.UIAPIRawForm.Mode = BoFormMode.fm_FIND_MODE;
                    active.txtDocEntry.Value = RegisterationNo;
                    active.btnSave.Item.Click();
                    active.Show();
                }
            }

        }

        private Button btnDifferences;

        private void btnDifferences_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (!Grid0.DataTable.IsEmpty)
                {
                    Global.SetMessage("Please wait while differences are being detected", BoStatusBarMessageType.smt_Warning);
                    ViewDifferences active = new ViewDifferences(LstFromCSV);
                    active.btnRefresh.Item.Click();
                    active.Show();
                }
            }
            catch (Exception ex)
            {
                Global.SetMessage("There was a problem detecting differences", BoStatusBarMessageType.smt_Error);
            }
        }

        private CheckBox cbUpdatePriceList;
    }
}