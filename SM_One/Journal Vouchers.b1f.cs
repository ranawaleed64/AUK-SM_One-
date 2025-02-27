
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using SAPbouiCOM.Framework;

namespace SM_One
{

    [FormAttribute("229", "Journal Vouchers.b1f")]
    class Journal_Vouchers : SystemFormBase
    {
        public Journal_Vouchers()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.Matrix0 = ((SAPbouiCOM.Matrix)(this.GetItem("8").Specific));
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("btnSelect").Specific));
            this.Button0.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button0_PressedAfter);
            this.CheckBox0 = ((SAPbouiCOM.CheckBox)(this.GetItem("10").Specific));
            this.Folder0 = ((SAPbouiCOM.Folder)(this.GetItem("Item_1").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Matrix Matrix0;

        private void OnCustomInitialize()
        {

        }
        private void CheckforDueVouchers()
        {
            try
            {
                Recordset oRec = Global.Comp_DI.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
                string Query = "";
                if (Global.Comp_DI.DbServerType == BoDataServerTypes.dst_HANADB)
                {
                    Query = "Select T0.\"BatchNum\",T0.\"RefDate\",T0.\"DueDate\",T1.\"LocTotal\" from OBTF T0 inner join OBTD T1 on T0.\"BatchNum\" = T1.\"BatchNum\" where T0.\"BtfStatus\"='O' and DAYS_BETWEEN(T0.\"DueDate\",Current_Date) > -1";
                }
                else
                {
                    Query = "Select T0.\"BatchNum\",T0.\"RefDate\",T0.\"DueDate\",T1.\"LocTotal\" from OBTF T0 inner join OBTD T1 on T0.\"BatchNum\" = T1.\"BatchNum\" where T0.\"BtfStatus\"='O' and DATEDIFF(day,T0.\"DueDate\",getdate()) > -1";
                }

                oRec.DoQuery(Query);
                Matrix0.Columns.Item("1").TitleObject.Sort(SAPbouiCOM.BoGridSortType.gst_Ascending);
                //bool first = true;
                Matrix0.ClearSelections();
                int Count = 0;
                bool moved = false;
                UIAPIRawForm.Freeze(true);
                if (oRec.RecordCount > 0)
                {

                    while (!oRec.EoF)
                    {
                        moved = false;
                        for (int i = Count; i < Matrix0.RowCount; i++)
                        {
                            if (oRec.Fields.Item("BatchNum").Value.ToString() == ((SAPbouiCOM.EditText)Matrix0.GetCellSpecific("1", i + 1)).String)
                            {

                                //if (first)
                                //{
                                //    Matrix0.Columns.Item("1").Cells.Item(i + 1).Click(SAPbouiCOM.BoCellClickType.ct_Regular, 0);
                                //    first = false;
                                //}
                                //else
                                //{


                                Matrix0.Columns.Item("1").Cells.Item(i + 1).Click(SAPbouiCOM.BoCellClickType.ct_Regular, 4096);
                                Count = i;
                                oRec.MoveNext();
                                moved = true;
                                break;
                                //}

                            }
                        }
                        if (!moved)
                        {
                            oRec.MoveNext();
                        }

                    }

                }
                UIAPIRawForm.Freeze(false);
            }
            catch (Exception ex)
            {
                UIAPIRawForm.Freeze(false);
                throw;
            }
        }
        private void Button0_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                if (Global.myApi.MessageBox("Are you sure you want to fetch due vouchers?", 1, "Yes", "No") == 1)
                {
                    CheckBox0.Checked = true;
                    if (!Program.isOpen)
                    {
                        Postings myform = new Postings();
                        myform.Show();
                        Program.isOpen = true;
                    }
                    CheckforDueVouchers();
                }
            }
            catch
            {

            }

        }

        private SAPbouiCOM.CheckBox CheckBox0;
        private SAPbouiCOM.Folder Folder0;
    }
}
