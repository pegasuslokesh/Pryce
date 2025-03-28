using PegasusDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web;
public partial class Inventory_GetStockValue : System.Web.UI.Page
{
    DataAccessClass objDa = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        FillGrid();
    }
    public void FillGrid()
    {

        //Product Master
        //Product ledger
        //Total value





        DataTable dt = new DataTable();

        string STQRL = @"Select IPM.ProductId, IPM.ProductCode, IPM.EProductName From Inv_ProductMaster IPM where IPM.IsActive='True' and IPM.ItemType='S'";


        dt = objDa.return_DataTable(STQRL);
        //dt = new DataView(dt, "QtyIn Is Not null and QtyOut Is Not Null and ActualStock Is Not Null", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            DataColumn newQtyIn = new DataColumn("QtyIn", typeof(string));
            DataColumn newQtyOut = new DataColumn("QtyOut", typeof(string));
            DataColumn newActualStock = new DataColumn("ActualStock", typeof(string));
            DataColumn newAvgCostColumn = new DataColumn("AvgCost", typeof(string));
            DataColumn newStockCoulmn = new DataColumn("StockValue", typeof(string));
            DataColumn CurrentStock = new DataColumn("CurrentStock", typeof(string));
            DataColumn IsValid = new DataColumn("IsValid", typeof(string));
            // Add the new column to the DataTable  
            dt.Columns.Add(newQtyIn);
            dt.Columns.Add(newQtyOut);
            dt.Columns.Add(newActualStock);
            dt.Columns.Add(newAvgCostColumn);
            dt.Columns.Add(newStockCoulmn);
            dt.Columns.Add(CurrentStock);
            dt.Columns.Add(IsValid);
            dt.AcceptChanges();

            float TotalStockValue = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string ProductId = dt.Rows[i]["ProductId"].ToString();
                
                string Stock = "";
                try
                {
                    Stock = objDa.get_SingleValue("Select Quantity from Inv_StockDetail where ProductId='" + ProductId + "' And IsActive='1' And Finance_Year_Id='15' And Location_Id='2'");
                    if (Stock != "" && Stock != "@NOTFOUND@")
                    {
                        dt.Rows[i]["CurrentStock"] = Stock;


                    }
                    else
                    {
                        dt.Rows[i]["CurrentStock"] = "0";
                    }

                }
                catch (Exception ex)
                {
                    dt.Rows[i]["CurrentStock"] = "0";
                }






                string strProductSql = "select * from Inv_ProductLedger where ProductId='" + ProductId + "'  and IsActive='True' and Location_Id in ('2')";
                DataTable dtAvgProduct = objDa.return_DataTable(strProductSql);
                if (dtAvgProduct.Rows.Count > 0)
                {
                   

                    //float fUnitTotal = 0;
                    //float fQtyTotal = 0;
                    //float fNetTotal = 0;

                    //float QtyIn = 0;
                    //float QtyOut = 0;

                    //for (int j = 0; j < dtAvgProduct.Rows.Count; j++)
                    //{

                    //    if (dtAvgProduct.Rows[j]["TransType"].ToString() == "PG" && dtAvgProduct.Rows[j]["InOut"].ToString() == "I")
                    //    {

                    //        string Quantity = dtAvgProduct.Rows[j]["QuantityIn"].ToString();
                    //        string UnitPrice = dtAvgProduct.Rows[j]["UnitPrice"].ToString();
                    //        float TotalPrice = float.Parse(Quantity) * float.Parse(UnitPrice);

                    //        fUnitTotal = fUnitTotal + float.Parse(UnitPrice);
                    //        fQtyTotal = fQtyTotal + float.Parse(Quantity);
                    //        fNetTotal = fNetTotal + TotalPrice;

                    //        //float AvgCost = (TotalPrice + (OldAvgCost * OldQty) / (float.Parse(Quantity) + OldQty));
                    //        QtyIn = QtyIn + float.Parse(Quantity);
                    //        //OldAvgCost = AvgCost;
                    //        //OldQty = OldQty + float.Parse(Quantity);
                    //    }
                    //    else if (dtAvgProduct.Rows[j]["TransType"].ToString() == "SR" || dtAvgProduct.Rows[j]["TransType"].ToString() == "FI" || dtAvgProduct.Rows[j]["TransType"].ToString() == "TI" && dtAvgProduct.Rows[j]["InOut"].ToString() == "I")
                    //    {
                    //        QtyIn = QtyIn + float.Parse(dtAvgProduct.Rows[j]["QuantityIn"].ToString());
                    //    }
                    //    else if (dtAvgProduct.Rows[j]["TransType"].ToString() == "Ph" && dtAvgProduct.Rows[j]["InOut"].ToString() == "I")
                    //    {
                    //        QtyIn = QtyIn + float.Parse(dtAvgProduct.Rows[j]["QuantityIn"].ToString());
                    //    }
                    //    else if (dtAvgProduct.Rows[j]["TransType"].ToString() == "Ph" && dtAvgProduct.Rows[j]["InOut"].ToString() == "O")
                    //    {
                    //        QtyOut = QtyOut + float.Parse(dtAvgProduct.Rows[j]["QuantityOut"].ToString());
                    //    }
                    //    else if (dtAvgProduct.Rows[j]["TransType"].ToString() == "SA" && dtAvgProduct.Rows[j]["InOut"].ToString() == "I")
                    //    {
                    //        QtyIn = QtyIn + float.Parse(dtAvgProduct.Rows[j]["QuantityIn"].ToString());
                    //    }
                    //    else if (dtAvgProduct.Rows[j]["TransType"].ToString() == "SA" && dtAvgProduct.Rows[j]["InOut"].ToString() == "O")
                    //    {
                    //        QtyOut = QtyOut + float.Parse(dtAvgProduct.Rows[j]["QuantityOut"].ToString());
                    //    }
                    //    else if (dtAvgProduct.Rows[j]["TransType"].ToString() == "SI" || dtAvgProduct.Rows[j]["TransType"].ToString() == "DV" || dtAvgProduct.Rows[j]["TransType"].ToString() == "PR" || dtAvgProduct.Rows[j]["TransType"].ToString() == "FO" || dtAvgProduct.Rows[j]["TransType"].ToString() == "TO" || dtAvgProduct.Rows[j]["TransType"].ToString() == "RM" && dtAvgProduct.Rows[j]["InOut"].ToString() == "O")
                    //    {
                    //        QtyOut = QtyOut + float.Parse(dtAvgProduct.Rows[j]["QuantityOut"].ToString());
                    //    }
                    //}


                    //dt.Rows[i]["ActualStock"] = (QtyIn - QtyOut).ToString();


                    //float fAvgCost = 0;
                    //if (fNetTotal == 0 && fQtyTotal == 0)
                    //{
                    //    fAvgCost = 0;
                    //}
                    //else
                    //{
                    //    fAvgCost = (fNetTotal / fQtyTotal);
                    //}


                    //float StockValue = (QtyIn - QtyOut) * fAvgCost;


                    //dt.Rows[i]["AvgCost"] = fAvgCost.ToString();
                    //dt.Rows[i]["QtyIn"] = QtyIn.ToString();
                    //dt.Rows[i]["QtyOut"] = QtyOut.ToString();


                    //dt.Rows[i]["StockValue"] = StockValue.ToString();

                    //TotalStockValue = TotalStockValue + StockValue;


                    //float ActualStock = float.Parse(dt.Rows[i]["ActualStock"].ToString());
                    //float CStock = float.Parse(dt.Rows[i]["CurrentStock"].ToString());


                    ////Update Product Stock in Stock Detail table and update the latest Average Cost
                    //int iStock = objDa.execute_Command("update Inv_StockDetail set Quantity='" + ActualStock.ToString() + "', Field2='" + fAvgCost.ToString() + "' where Company_Id='2' and Brand_Id='2' and Location_Id='2' and ProductId = '" + ProductId + "' and Finance_Year_Id = '15'");
                    ////int iPhysicalDetail = objDa.execute_Command("update Inv_PhysicalDetail set SystemQuantity='" + ActualStock.ToString() + "' where ProductId='" + ProductId + "' and Header_Id='46' and Location_Id='2'");


                    //if (ActualStock == CStock)
                    //{
                    //    dt.Rows[i]["IsValid"] = "True";
                    //}
                    //else
                    //{
                    //    dt.Rows[i]["IsValid"] = "False";
                    //}
                }
                else
                {
                    //Update Product Stock in Stock Detail table and update the latest Average Cost
                    int iStock = objDa.execute_Command("update Inv_StockDetail set Quantity='0' where Company_Id='2' and Brand_Id='2' and Location_Id='2' and ProductId = '" + ProductId + "' and Finance_Year_Id = '15'");
                    int iPhysicalDetail = objDa.execute_Command("update Inv_PhysicalDetail set SystemQuantity='0' where ProductId='" + ProductId + "' and Header_Id='46' and Location_Id='2'");
                }

            }
            NetValue.Text = TotalStockValue.ToString();

            dt = new DataView(dt, "QtyIn Is Not null and QtyOut Is Not Null and ActualStock Is Not Null and AvgCost Is Not Null and StockValue Is Not Null", "", DataViewRowState.CurrentRows).ToTable();

            GvStockDetail.DataSource = dt;
            GvStockDetail.DataBind();
        }
    }
}