using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM_One
{
    [FormAttribute("SM_One.ViewDifferences", "ViewDifferences.b1f")]
    class ViewDifferences : UserFormBase
    {
        public ViewDifferences(List<string[]> lst)
        {
            LstOfItems = lst;
            this.ComboBox0.Select("150", SAPbouiCOM.BoSearchKey.psk_ByValue);

        }
        List<string[]> LstOfItems = new List<string[]>();

        int TotalRecords = 0;
        int Pages = 0;
        bool PagesCalculated = false;
        int CurrentPage = 1;
        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.ComboBox0 = ((SAPbouiCOM.ComboBox)(this.GetItem("cbFetch").Specific));
            this.ComboBox0.ComboSelectBefore += new SAPbouiCOM._IComboBoxEvents_ComboSelectBeforeEventHandler(this.ComboBox0_ComboSelectBefore);
            this.ComboBox0.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.ComboBox0_ComboSelectAfter);
            this.Grid0 = ((SAPbouiCOM.Grid)(this.GetItem("Grid0").Specific));
            this.btnNext = ((SAPbouiCOM.Button)(this.GetItem("btnNext").Specific));
            this.btnNext.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btnNext_PressedAfter);
            this.btnPrevious = ((SAPbouiCOM.Button)(this.GetItem("btnPrev").Specific));
            this.btnPrevious.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btnPrevious_PressedAfter);
            this.btnRefresh = ((SAPbouiCOM.Button)(this.GetItem("btRefresh").Specific));
            this.btnRefresh.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btnRefresh_PressedAfter);
            this.stShowing = ((SAPbouiCOM.StaticText)(this.GetItem("stShowing").Specific));
            this.stPages = ((SAPbouiCOM.StaticText)(this.GetItem("stPage").Specific));
            this.ComboBox0.ExpandType = BoExpandType.et_DescriptionOnly;
            PagesCalculated = false;
            this.OnCustomInitialize();

        }
        int BlueColor = System.Drawing.Color.FromArgb(37, 150, 190).ToArgb() * -1;
        int WhiteColor = System.Drawing.Color.White.R | (System.Drawing.Color.White.G << 8) | (System.Drawing.Color.White.B << 16);

        private void CalculatePages(bool withCurrentPage)
        {
            Pages = LstOfItems.Count / Convert.ToInt32(ComboBox0.Selected.Value);
            if (LstOfItems.Count % Convert.ToInt32(ComboBox0.Selected.Value) > 0)
            {
                Pages++;
            }
            PagesCalculated = true;
            if (Pages == 1)
            {
                btnNext.Item.Enabled = false;
            }
            btnPrevious.Item.Enabled = false;
            if (!withCurrentPage || CurrentPage > Pages)
            {
                CurrentPage = 1;
            }
        }
        private void FormulateQueryAndFillGrid()
        {
            try
            {

                UIAPIRawForm.Freeze(true);
                int StartingRecord = 0;
                if (!PagesCalculated)
                {
                    CalculatePages(false);
                }
                TotalRecords = LstOfItems.Count;
                int MaxRecordsToFetch = Convert.ToInt32(ComboBox0.Selected.Value);
                StartingRecord = CurrentPage;
                if ((StartingRecord * MaxRecordsToFetch) > TotalRecords)
                {
                    int Max = MaxRecordsToFetch;
                    int Start = StartingRecord;
                    StartingRecord = (Start - 1) * Max;
                    MaxRecordsToFetch = TotalRecords;
                }
                else if (StartingRecord > 1)
                {
                    int Max = MaxRecordsToFetch;
                    int Start = StartingRecord;
                    StartingRecord = (Start - 1) * Max;
                    MaxRecordsToFetch = MaxRecordsToFetch * (Start);
                }
                string Query = "";
                bool firstIteration = true;

                for (int i = StartingRecord; i < MaxRecordsToFetch; i++)
                {
                    if (i == 0)
                    {
                        continue;
                    }

                    string CardCode, CardName, Status, School, Level, SubLevel, Program, Sponsor, Semester, AcademicYear, FiscalYear, Phone, Email;
                    CardCode = LstOfItems[i][1];
                    CardName = LstOfItems[i][2];
                    Status = LstOfItems[i][3];
                    School = LstOfItems[i][4];
                    Level = LstOfItems[i][5];
                    SubLevel = LstOfItems[i][6];
                    Program = LstOfItems[i][7];
                    Sponsor = LstOfItems[i][8];
                    AcademicYear = LstOfItems[i][9];
                    Semester = LstOfItems[i][10];
                    Phone = LstOfItems[i][11];
                    Email = LstOfItems[i][12];
                    FiscalYear = LstOfItems[i][13];

                    if (!firstIteration)
                    {
                        Query += " Union All ";
                    }
                    firstIteration = false;
                    Query += @"Select T0.""CardCode"" as ""ID"",T0.""CardName"" as ""Current Name"",'" + CardName + @"' as ""Updating Name"",
T0.""U_Status"" as ""Current Status"",'" + Status + @"'  as ""Updating Status"",
T0.""U_School"" as ""Current School"",'" + School + @"' as ""Updating School"",
T0.""U_Level"" as ""Current Level"",'" + Level + @"'  as ""Updating Level"",
T0.""U_SL"" as ""Current SubLevel"",'" + SubLevel + @"'  as ""Updating SubLevel"",
T0.""U_Program"" as ""Current Program"",'" + Program + @"'  as ""Updating Program"",
T0.""U_Sponsor"" as ""Current Sponsor"",'" + Sponsor + @"'  as ""Updating Sponsor"",
T0.""U_Semester"" as ""Current Semester"",'" + Semester + @"'  as ""Updating Semester"",
T0.""U_AY"" as ""Current Academic"",'" + AcademicYear + @"'  as ""Updating Academic"",
T0.""U_Joining_Year"" as ""Current FiscalYear"",'" + FiscalYear + @"'  as ""Updating FiscalYear"",
T0.""Cellular"" as ""Current Phone Number"",'" + Phone + @"'  as ""Updating Phone Number"",
T0.""E_Mail"" as ""Current Email"",'" + Email + @"'  as ""Updating Email""
from OCRD T0 where T0.""CardCode"" = '" + CardCode + @"' and 
(T0.""CardName"" != '" + CardName + @"' or T0.""U_Status"" != '" + Status + @"' or T0.""U_Level"" !='" + Level + @"' or T0.""U_SL"" != '" + SubLevel + @"'
or T0.""U_Program"" != '" + Program + @"' or T0.""U_Sponsor"" != '" + Sponsor + @"' or T0.""U_Semester"" !='" + Semester + @"' or T0.""U_AY""!='" + AcademicYear + @"'
or T0.""U_Joining_Year"" != " + FiscalYear + @" or T0.""Cellular"" !='" + Phone + @"' or T0.""E_Mail"" != '" + Email + @"')";
                }
                Grid0.DataTable.ExecuteQuery(Query);
                stShowing.Caption = "Students from " + StartingRecord.ToString() + " to " + MaxRecordsToFetch + " of Total : " + TotalRecords.ToString() + "\n. Current Page Changes : " + Grid0.DataTable.Rows.Count.ToString();
                Grid0.AutoResizeColumns();
                Grid0.Item.Enabled = false;
                UIAPIRawForm.Freeze(false);
                UIAPIRawForm.Freeze(true);
                ColorGrid();
                stPages.Caption = "Page No." + CurrentPage.ToString() + " of " + Pages.ToString();
                UIAPIRawForm.Freeze(false);
            }
            catch (Exception ex)
            {

                UIAPIRawForm.Freeze(false);
                Global.SetMessage("There was a problem fetching differences", BoStatusBarMessageType.smt_Error);
            }
        }

        private void ColorGrid()
        {
            Global.myApi.StatusBar.SetText("Please continue to wait while changes are being highlighted", BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Warning);

            for (int r = 0; r < Grid0.DataTable.Rows.Count; r++)
            {
                for (int c = 1; c < Grid0.DataTable.Columns.Count; c += 2)
                {
                    if (Grid0.DataTable.GetValue(c, r).ToString() != Grid0.DataTable.GetValue(c + 1, r).ToString())
                    {
                        Grid0.CommonSetting.SetCellBackColor(r + 1, c + 2, BlueColor);
                        Grid0.CommonSetting.SetCellFontColor(r + 1, c + 2, WhiteColor);

                    }
                    else
                    {
                        Grid0.CommonSetting.SetCellBackColor(r + 1, c + 2, -1);
                        Grid0.CommonSetting.SetCellFontColor(r + 1, c + 2, -1);
                    }
                }
            }
            Global.SetMessage("Detected and Highlighted successfully", BoStatusBarMessageType.smt_Warning);
        }
        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        /// 
        private void ColorGridWithParallelForAndBackgroundWorker()
        {
            Parallel.For(0, Grid0.DataTable.Rows.Count, r =>
   {
       for (int c = 1; c < Grid0.DataTable.Columns.Count; c += 2)
       {
           if (Grid0.DataTable.GetValue(c, r).ToString() != Grid0.DataTable.GetValue(c + 1, r).ToString())
           {
               Grid0.CommonSetting.SetCellBackColor(r + 1, c + 2, BlueColor);
               Grid0.CommonSetting.SetCellFontColor(r + 1, c + 2, WhiteColor);
           }

       }
   });
        }
        private void ColorGridWithParallelFor()
        {
            Global.myApi.StatusBar.SetText("Please continue to wait while changes are being highlighted", BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Warning);

            Parallel.For(0, Grid0.DataTable.Rows.Count, r =>
            {
                for (int c = 1; c < Grid0.DataTable.Columns.Count; c += 2)
                {
                    if (Grid0.DataTable.GetValue(c, r).ToString() != Grid0.DataTable.GetValue(c + 1, r).ToString())
                    {
                        Grid0.CommonSetting.SetCellBackColor(r + 1, c + 2, BlueColor);
                        Grid0.CommonSetting.SetCellFontColor(r + 1, c + 2, WhiteColor);
                    }
                }
            });

            Global.SetMessage("Detected and Highlighted successfully", BoStatusBarMessageType.smt_Warning);
        }
        private BackgroundWorker bgWorker;

        private void ColorGridWihtBothParallelFor()
        {
            Global.myApi.StatusBar.SetText("Please continue to wait while changes are being highlighted", BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Warning);
            bgWorker = new BackgroundWorker();
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
            bgWorker.RunWorkerAsync();
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {

            Parallel.For(0, Grid0.DataTable.Rows.Count, new ParallelOptions() { MaxDegreeOfParallelism = 10 }, r =>
            {
                Parallel.For(1, Grid0.DataTable.Columns.Count,new ParallelOptions() { MaxDegreeOfParallelism = 10 }, c =>
                {
                    if (bgWorker.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                    if (c % 2 == 1)
                    {
                        if (Grid0.DataTable.GetValue(c, r).ToString() != Grid0.DataTable.GetValue(c + 1, r).ToString())
                        {
                            Grid0.CommonSetting.SetCellBackColor(r + 1, c + 2, BlueColor);
                            Grid0.CommonSetting.SetCellFontColor(r + 1, c + 2, WhiteColor);
                        }
                        else
                        {
                            Grid0.CommonSetting.SetCellBackColor(r + 1, c + 2, -1);
                            Grid0.CommonSetting.SetCellFontColor(r + 1, c + 2, -1);
                        }
                    }
                });
            });
            //for (int r = 0; r < Grid0.DataTable.Rows.Count; r++)
            //{
            //    for (int c = 1; c < Grid0.DataTable.Columns.Count; c += 2)
            //    {
            //        if (bgWorker.CancellationPending)
            //        {
            //            e.Cancel = true;
            //            return;
            //        }

            //        if (Grid0.DataTable.GetValue(c, r).ToString() != Grid0.DataTable.GetValue(c + 1, r).ToString())
            //        {
            //            Grid0.CommonSetting.SetCellBackColor(r + 1, c + 2, BlueColor);
            //            Grid0.CommonSetting.SetCellFontColor(r + 1, c + 2, WhiteColor);
            //        }
            //    }
            //}
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Global.SetMessage("Detected and Highlighted successfully", BoStatusBarMessageType.smt_Warning);
            bgWorker.Dispose();
            bgWorker = null;
        }
        public override void OnInitializeFormEvents()
        {
            this.CloseBefore += new CloseBeforeHandler(this.Form_CloseBefore);

        }

        private SAPbouiCOM.Grid Grid0;
        //private Button btnOkay;

        private void OnCustomInitialize()
        {

        }
        public SAPbouiCOM.Button btnRefresh;

        private void btnRefresh_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            //FormulateQueryAndFillGrid();
        }

        private ComboBox ComboBox0;
        //private StaticText StaticText0;
        private Button btnNext;
        private Button btnPrevious;

        private void btnNext_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            if ((CurrentPage + 1) <= Pages)
            {
                CurrentPage++;
                FormulateQueryAndFillGrid();
            }
            if ((CurrentPage) == Pages)
            {
                btnNext.Item.Enabled = false;
            }
            if (CurrentPage > 1)
            {
                btnPrevious.Item.Enabled = true;
            }
        }

        //private Button Button4;

        private void btnPrevious_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            if ((CurrentPage - 1) >= 1)
            {
                CurrentPage--;
                FormulateQueryAndFillGrid();
            }
            if ((CurrentPage) == 1)
            {
                btnPrevious.Item.Enabled = false;
            }
            if (CurrentPage < Pages)
            {
                btnNext.Item.Enabled = true;
            }

        }

        private StaticText stShowing;
        string PreviousValue = "";
        private void ComboBox0_ComboSelectAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (!pVal.InnerEvent && pVal.ActionSuccess)
            {
                if (Convert.ToInt32(ComboBox0.Selected.Value) == 500)
                {
                    int Ret = Global.myApi.MessageBox("Browsing 500 records per page will take slow down the screen. Are you sure want to continue?", 1, "Yes", "No");
                    if (Ret!= 1)
                    {
                        ComboBox0.Select(PreviousValue, BoSearchKey.psk_ByValue);
                        return;
                    }
                }
                CalculatePages(true);
                FormulateQueryAndFillGrid();
            }

        }

        private void Form_CloseBefore(SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (bgWorker != null)
            {
                if (bgWorker.IsBusy)
                {
                    bgWorker.CancelAsync();
                    bgWorker.Dispose();
                }
            }

        }

        private StaticText stPages;

        private void ComboBox0_ComboSelectBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (!pVal.InnerEvent && ComboBox0.Selected != null)
            {
                PreviousValue = ComboBox0.Selected.Value;
            }
        }
    }
}
