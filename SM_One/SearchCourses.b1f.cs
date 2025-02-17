using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SM_One
{
    [FormAttribute("SM_One.SearchCourses", "SearchCourses.b1f")]
    class SearchCourses : UserFormBase
    {
        public SearchCourses()
        {
        }
        string ProgramCode = "";
        string SubjectCodes = "";
        Matrix Matrix0;
        DBDataSource RowsDb;
        SAPbouiCOM.Form RegForm;
        string CardCode;
        public SearchCourses(string programCode, string subjectCodes, Matrix matrix0, DBDataSource rowsDb, SAPbouiCOM.Form form, string cardCode)
        {
            ProgramCode = programCode;
            SubjectCodes = subjectCodes;
            Matrix0 = matrix0;
            RowsDb = rowsDb;
            RegForm = form;
            CardCode = cardCode;
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.Grid0 = ((SAPbouiCOM.Grid)(this.GetItem("Grid0").Specific));
            this.btnCancel = ((SAPbouiCOM.Button)(this.GetItem("btCancel").Specific));
            this.btnChoose = ((SAPbouiCOM.Button)(this.GetItem("btChoose").Specific));
            this.btnChoose.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btnChoose_PressedAfter);
            this.DT = this.UIAPIRawForm.DataSources.DataTables.Item("DT_0");
            this.Grid0.Item.Enabled = false;
            this.OnCustomInitialize();
        }
        public void ExecuteQuery()
        {
            string Query = @"SELECT T1.""U_SC"" as ""Subject Code"", T1.""U_SN"" as ""Subject Name"", T1.""U_CH"" as ""Credit Hours"",T4.""Price"" FROM ""@OSCL""  T0 inner join ""@SCL1""  T1 on T0.""DocEntry"" = T1.""DocEntry"" inner join OCRD T2 on T2.""U_YearJoined"" = T0.""U_FY"" and T2.""CardCode"" = '"+CardCode+@"' inner join OPLN T3 on T3.""U_FiscalYear"" = T2.""U_YearJoined"" inner join ITM1 T4 on T4.""PriceList"" = T3.""ListNum"" and T4.""ItemCode"" = T1.""U_SC"" WHERE T0.""U_PC"" = '" + ProgramCode + @"' and T1.""U_SC"" not in (" + SubjectCodes + ")";
            DT.ExecuteQuery(Query);
        }
        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        private SAPbouiCOM.Grid Grid0;
        private Button btnCancel;

        private void OnCustomInitialize()
        {

        }

        private SAPbouiCOM.Button btnChoose;
        private SAPbouiCOM.DataTable DT;

        private void btnChoose_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {

                RegForm.Freeze(true);
                for (int i = 0; i < Grid0.Rows.SelectedRows.Count; i++)
                {
                    RowsDb.InsertRecord(RowsDb.Size);
                    Matrix0.LoadFromDataSource();
                    int RowIndex = Grid0.Rows.SelectedRows.Item(i, BoOrderType.ot_RowOrder);
                    Matrix0.SetCellWithoutValidation(Matrix0.RowCount, "SubCode", Grid0.DataTable.GetValue("Subject Code", RowIndex).ToString());
                    Matrix0.SetCellWithoutValidation(Matrix0.RowCount, "SubName", Grid0.DataTable.GetValue("Subject Name", RowIndex).ToString());
                    double Price = 0;
                    double CH = 0;
                    double.TryParse(Grid0.DataTable.GetValue("Credit Hours", RowIndex).ToString(), out CH);
                    double.TryParse(Grid0.DataTable.GetValue("Price", RowIndex).ToString(), out Price);
                    Matrix0.SetCellWithoutValidation(Matrix0.RowCount, "Credits", CH.ToString());
                    Matrix0.SetCellWithoutValidation(Matrix0.RowCount, "Price", (Price*CH).ToString());
                    //((ComboBox)Matrix0.GetCellSpecific("LineType", Matrix0.RowCount)).Select("M", BoSearchKey.psk_ByValue);
                    Matrix0.SetCellWithoutValidation(Matrix0.RowCount, "LineType", "M");
                    Matrix0.FlushToDataSource();

                }
                RegForm.Mode = BoFormMode.fm_UPDATE_MODE;
                RegForm.Freeze(false);
                UIAPIRawForm.Close();

            }
            catch (Exception ex)
            {
                Global.SetMessage(ex.Message, BoStatusBarMessageType.smt_Error);
            }
            finally
            {
                RegForm.Freeze(false);
            }
        }
    }
}
