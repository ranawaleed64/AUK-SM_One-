using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
namespace SM_One
{
    [FormAttribute("SM_One.Postings", "Postings.b1f")]
    class Postings : UserFormBase
    {
        public Postings()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.Grid0 = ((SAPbouiCOM.Grid)(this.GetItem("Item_0").Specific));
            this.Grid0.DoubleClickAfter += new SAPbouiCOM._IGridEvents_DoubleClickAfterEventHandler(this.Grid0_DoubleClickAfter);
            SM_One.Program.isOpen = true;
            this.FillMatrix();
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_1").Specific));
            this.OnCustomInitialize();

        }
        private void FillMatrix()
        {

            string Query = "";
            if (Global.Comp_DI.DbServerType == BoDataServerTypes.dst_HANADB)
            {
                Query = "Select T0.\"BatchNum\" as \"Voucher No.\",T0.\"RefDate\",T0.\"DueDate\",T1.\"LocTotal\" from OBTF T0 inner join OBTD T1 on T0.\"BatchNum\" = T1.\"BatchNum\" where T0.\"BtfStatus\"='O' and T0.\"U_StVoucher\" = 'Y' and DAYS_BETWEEN(T0.\"DueDate\",Current_Date) > -1";
            }
            else
            {
                Query = "Select T0.\"BatchNum\" as \"Voucher No.\",T0.\"RefDate\",T0.\"DueDate\",T1.\"LocTotal\" from OBTF T0 inner join OBTD T1 on T0.\"BatchNum\" = T1.\"BatchNum\" where T0.\"BtfStatus\"='O' and T0.\"U_StVoucher\" = 'Y' and DATEDIFF(day,T0.\"DueDate\",getdate()) > -1";
            }
            UIAPIRawForm.DataSources.DataTables.Item("DT_0").ExecuteQuery(Query);
        }
        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.ActivateAfter += new SAPbouiCOM.Framework.FormBase.ActivateAfterHandler(this.Form_ActivateAfter);
            this.CloseBefore += new CloseBeforeHandler(this.Form_CloseBefore);
        }

        private SAPbouiCOM.Grid Grid0;
        private void Grid0_DoubleClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {

            if (pVal.ColUID == "RowsHeader")
            {
                string GetValue = Grid0.DataTable.GetValue("Voucher No.", pVal.Row).ToString();
                Global.myApi.OpenForm(SAPbouiCOM.BoFormObjectEnum.fo_JournalVoucher, " ", GetValue);
            }

        }

        private void Form_ActivateAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            if (Grid0 != null && Grid0.Rows.Count > 0)
            {
                string Query = "";
                if (Global.Comp_DI.DbServerType == BoDataServerTypes.dst_HANADB)
                {
                    Query = "Select T0.\"BatchNum\" as \"Voucher No.\",T0.\"RefDate\",T0.\"DueDate\",T1.\"LocTotal\" from OBTF T0 inner join OBTD T1 on T0.\"BatchNum\" = T1.\"BatchNum\" where T0.\"BtfStatus\"='O' and T0.\"U_StVoucher\" = 'Y' and DAYS_BETWEEN(T0.\"DueDate\",Current_Date) > -1";
                }
                else
                {
                    Query = "Select T0.\"BatchNum\" as \"Voucher No.\",T0.\"RefDate\",T0.\"DueDate\",T1.\"LocTotal\" from OBTF T0 inner join OBTD T1 on T0.\"BatchNum\" = T1.\"BatchNum\" where T0.\"BtfStatus\"='O' and T0.\"U_StVoucher\" = 'Y' and DATEDIFF(day,T0.\"DueDate\",getdate()) > -1";
                }
                UIAPIRawForm.DataSources.DataTables.Item("DT_0").ExecuteQuery(Query);
            }
        }

        private void Form_CloseBefore(SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            Program.isOpen = false;
        }
        private void OnCustomInitialize()
        {

        }

        private SAPbouiCOM.StaticText StaticText0;
    }
}
