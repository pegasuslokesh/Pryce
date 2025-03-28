using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class WebUserControl_productSerial : System.Web.UI.UserControl
{
    Common objCommon = null;
    Inv_StockBatchMaster ObjStockBatchMaster = null;
    PageControlCommon objPageCmn = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        objCommon = new Common(Session["DBConnection"].ToString());
        ObjStockBatchMaster = new Inv_StockBatchMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
    }



    protected void Btnloadfile_Click(object sender, EventArgs e)
    {
        int counter = 0;
        txtSerialNo.Text = "";
        try
        {
            string Path = Server.MapPath("~/Temp/" + FULogoPath.FileName);
            string[] csvRows = System.IO.File.ReadAllLines(Path);
            string[] fields = null;
            foreach (string csvRow in csvRows)
            {
                fields = csvRow.Split(',');

                if (fields[0].ToString() != "")
                {
                    if (txtSerialNo.Text == "")
                    {
                        txtSerialNo.Text = fields[0].ToString();
                    }
                    else
                    {
                        txtSerialNo.Text += Environment.NewLine + fields[0].ToString();
                    }
                    counter++;
                }
            }
            if (Directory.Exists(Path))
            {
                try
                {
                    Directory.Delete(Path);
                }
                catch
                {
                }
            }
            txtCount.Text = counter.ToString();
        }
        catch
        {
            txtSerialNo.Text = "";
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "", "DisplayMsg('" + "File Not Found ,Try Again" + "')", true);
        }
        txtCount.Text = counter.ToString();

        if (counter == 0)
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "", "DisplayMsg('Serial Number Not Found')", true);
        }


    }

    protected void btnLoadSerial_Click(object sender, EventArgs e)
    {
        DataTable dtserial = new DataTable();

        dtserial.Columns.Add("SerialNo");

        string TransId = string.Empty;
        if (hdnUcProductSnoRefId.Value == "")
        {
            TransId = "0";
        }
        else
        {
            TransId = hdnUcProductSnoRefId.Value;
        }
        int counter = 0;
        txtSerialNo.Text = "";
        DataTable dtSockCopy = new DataTable();

        DataTable dtstock = ObjStockBatchMaster.GetTempSerial(hdnUcProductSnoProductId.Value, hdnReferenceType.Value , hdnOrderId.Value, hdnMerchantId.Value);
        try
        {
            for (int i = 0; i < dtstock.Rows.Count; i++)
            {
                DataRow dr = dtserial.NewRow();

                dr[0] = dtstock.Rows[i]["SerialNo"].ToString();
                dtserial.Rows.Add(dr);
                if (txtSerialNo.Text == "")
                {
                    txtSerialNo.Text = dtstock.Rows[i]["SerialNo"].ToString();
                }
                else
                {
                    txtSerialNo.Text += Environment.NewLine + dtstock.Rows[i]["SerialNo"].ToString();
                }
                counter++;

            }
        }
        catch
        {
        }
        txtCount.Text = counter.ToString();
        if (counter == 0)
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "", "DisplayMsg('Serial Number Not Found')", true);
        }
    }

    protected void btnexecute_Click(object sender, EventArgs e)
    {
        DataTable dtserial = new DataTable();

        dtserial.Columns.Add("SerialNo");

        string TransId = string.Empty;
        if (hdnUcProductSnoRefId.Value == "")
        {
            TransId = "0";
        }
        else
        {
            TransId = hdnUcProductSnoRefId.Value;
        }
        int counter = 0;
        txtSerialNo.Text = "";
        DataTable dtSockCopy = new DataTable();

        DataTable dtstock = ObjStockBatchMaster.GetStockBatchMasterAll_By_ProductIdForSalesInvoice(hdnUcProductSnoProductId.Value);
        try
        {
            for (int i = 0; i < dtstock.Rows.Count; i++)
            {
                DataRow dr = dtserial.NewRow();

                dr[0] = dtstock.Rows[i]["SerialNo"].ToString();
                dtserial.Rows.Add(dr);
                if (txtSerialNo.Text == "")
                {
                    txtSerialNo.Text = dtstock.Rows[i]["SerialNo"].ToString();
                }
                else
                {
                    txtSerialNo.Text += Environment.NewLine + dtstock.Rows[i]["SerialNo"].ToString();
                }
                counter++;

            }
        }
        catch
        {
        }
        txtCount.Text = counter.ToString();
        if (counter == 0)
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "", "DisplayMsg('Serial Number Not Found')", true);
        }
    }

    protected void btnsearchserial_Click(object sender, EventArgs e)
    {
        if (txtserachserialnumber.Text != "")
        {
            DataTable dt = new DataTable();
            if (ViewState["dtSerial"] != null)
            {
                dt = (DataTable)ViewState["dtSerial"];

                if (dt.Rows.Count > 0)
                {
                    dt = new DataView(dt, "SerialNo='" + txtserachserialnumber.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
            }
            if (dt.Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "", "DisplayMsg('Serial Number Not Found')", true);
                txtserachserialnumber.Text = "";
                txtserachserialnumber.Focus();
                return;
            }
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvSerialNumber, dt, "", "");
            txtselectedSerialNumber.Text = gvSerialNumber.Rows.Count.ToString();
            btnRefreshserial.Focus();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "", "DisplayMsg('Enter Serial Number')", true);
            txtserachserialnumber.Text = "";
            txtserachserialnumber.Focus();
        }
    }

    protected void btnRefreshserial_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        if (ViewState["dtSerial"] != null)
        {
            dt = (DataTable)ViewState["dtSerial"];

        }
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvSerialNumber, dt, "", "");
        txtselectedSerialNumber.Text = gvSerialNumber.Rows.Count.ToString();
        txtserachserialnumber.Text = "";
        txtserachserialnumber.Focus();
    }

    protected void IbtnDeleteserialNumber_Command(object sender, CommandEventArgs e)
    {
        if (ViewState["dtSerial"] != null)
        {
            DataTable dt = (DataTable)ViewState["dtSerial"];

            if (dt.Rows.Count > 0)
            {
                dt = new DataView(dt, "SerialNo<>'" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            ViewState["dtSerial"] = dt;
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvSerialNumber, dt, "", "");

            txtselectedSerialNumber.Text = gvSerialNumber.Rows.Count.ToString();
            txtserachserialnumber.Text = "";
            txtserachserialnumber.Focus();
        }
    }
    protected void BtnSerialSave_Click(object sender, EventArgs e)
    {
        string TransId = string.Empty;
        if (hdnUcProductSnoRefId.Value == "")
        {
            TransId = "0";
        }
        else
        {
            TransId = hdnUcProductSnoRefId.Value;
        }
        string DuplicateserialNo = string.Empty;
        string serialNoExists = string.Empty;
        string alreadyout = string.Empty;
        DataTable dt = new DataTable();
        DataTable dtTemp = new DataTable();
        DataTable dtStockBatch = ObjStockBatchMaster.GetStockBatchMasterAll_By_ProductId(hdnUcProductSnoProductId.Value);

        if (txtSerialNo.Text.Trim() != "")
        {
            string[] txt = txtSerialNo.Text.Split('\n');
            int counter = 0;

            if (ViewState["dtSerial"] == null)
            {
                dt = new DataTable();
                dt.Columns.Add("Product_Id");
                dt.Columns.Add("SerialNo");
                dt.Columns.Add("SOrderNo");
                dt.Columns.Add("BarcodeNo");
                dt.Columns.Add("BatchNo");
                dt.Columns.Add("LotNo");
                dt.Columns.Add("ExpiryDate");
                dt.Columns.Add("ManufacturerDate");

                for (int i = 0; i < txt.Length; i++)
                {
                    if (txt[i].ToString().Trim() != "")
                    {
                        string[] result = isSerialNumberValid(dt, txt[i].ToString().Trim(), hdnUcProductSnoProductId.Value, TransId, gvSerialNumber);
                        if (result[0] == "VALID")
                        {

                            DataRow dr = dt.NewRow();
                            dt.Rows.Add(dr);
                            dr[0] = hdnUcProductSnoProductId.Value;
                            dr[1] = txt[i].ToString();
                            dr[2] = hdnUcProductSnoOrderId.Value;
                            //for batch number
                            dr[4] = result[4].ToString();

                            dr[5] = "0";
                            //for expiry date
                            dr[6] = result[3].ToString();
                            //for Manufacturer date
                            dr[7] = result[2].ToString();
                            counter++;

                        }
                        else if (result[0].ToString() == "ALREADY OUT")
                        {
                            DuplicateserialNo += txt[i].ToString() + ",";
                        }
                        else if (result[0].ToString() == "NOT EXISTS")
                        {
                            serialNoExists += txt[i].ToString() + ",";
                        }
                    }
                }

            }
            else
            {
                dt = (DataTable)ViewState["dtSerial"];
                dtTemp = dt.Copy();

                for (int i = 0; i < txt.Length; i++)
                {
                    if (txt[i].ToString().Trim() != "")
                    {
                        string[] result = isSerialNumberValid(dt, txt[i].ToString().Trim(), hdnUcProductSnoProductId.Value, TransId, gvSerialNumber);
                        if (result[0] == "VALID")
                        {
                            DataRow dr = dt.NewRow();
                            dt.Rows.Add(dr);
                            dr[0] = hdnUcProductSnoProductId.Value;
                            dr[1] = txt[i].ToString();
                            dr[2] = hdnUcProductSnoOrderId.Value;
                            //for batch number
                            dr[4] = result[4].ToString();
                            dr[5] = "0";
                            //for expiry date
                            dr[6] = result[3].ToString();
                            //for Manufacturer date
                            dr[7] = result[2].ToString();
                            counter++;
                        }
                        else if (result[0].ToString() == "ALREADY OUT")
                        {
                            DuplicateserialNo += txt[i].ToString() + ",";
                        }
                        else if (result[0].ToString() == "NOT EXISTS")
                        {
                            serialNoExists += txt[i].ToString() + ",";
                        }
                    }
                }
            }
        }
        else
        {
            dt = (DataTable)ViewState["dtSerial"];
        }
        string Message = string.Empty;
        if (DuplicateserialNo != "" || serialNoExists != "")
        {
            if (DuplicateserialNo != "")
            {
                Message += "Following serial Number is Already Out from the stock=" + DuplicateserialNo;
            }
            if (serialNoExists != "")
            {
                if (Message == "")
                {
                    Message += "Following serial number not exists in stock=" + serialNoExists;
                }
                else
                {
                    Message += Environment.NewLine + "Following serial number not exists in stock=" + serialNoExists;
                }
            }
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "", "DisplayMsg('" + Message + "')", true);
        }
        string[] constrainResult = checkQtyConstrain(dt.Rows.Count);
        if (constrainResult[0]=="false")  //check system qty and other constrain as per module
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "", "DisplayMsg('" + constrainResult[1] + "')", true);
            return;
        }

        //code end
        ViewState["dtSerial"] = dt;
        if (Session["dtFinal"] == null)
        {
            if (ViewState["dtSerial"] != null)
            {
                Session["dtFinal"] = (DataTable)ViewState["dtSerial"];
            }
        }
        else
        {
            dt = new DataTable();
            DataTable Dtfinal = (DataTable)Session["dtFinal"];
            if (hdnUcProductSnoOrderId.Value == "0")
            {
                Dtfinal = new DataView(Dtfinal, "Product_Id<>'" + hdnUcProductSnoProductId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            else
            {
                DataTable DtTemp = Dtfinal.Copy();
                try
                {
                    DtTemp = new DataView(DtTemp, "Product_Id='" + hdnUcProductSnoProductId.Value + "' and SOrderNo='" + hdnUcProductSnoOrderId.Value + "' ", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (DtTemp.Rows.Count > 0)
                {
                    string s = "SOrderNo Not In('" + hdnUcProductSnoOrderId.Value + "') or Product_Id Not In('" + hdnUcProductSnoProductId.Value + "')";
                    Dtfinal = new DataView(Dtfinal, s.ToString(), "", DataViewRowState.CurrentRows).ToTable();
                }
            }

            if (ViewState["dtSerial"] != null)
            {
                dt = (DataTable)ViewState["dtSerial"];
            }
            Dtfinal.Merge(dt);
            Session["dtFinal"] = Dtfinal;
            setQtyOnParentGrid(dt.Rows.Count.ToString());
        }
        if (ViewState["dtSerial"] != null)
        {
            objPageCmn.FillData((object)gvSerialNumber, (DataTable)ViewState["dtSerial"], "", "");
            txtselectedSerialNumber.Text = gvSerialNumber.Rows.Count.ToString();
            Session["ucSerialCount"] = txtselectedSerialNumber.Text;
        }
        else
        {
            Session["ucSerialCount"] = "0";
        }


        txtSerialNo.Text = "";
        txtCount.Text = "0";
        txtSerialNo.Focus();
    }
    protected string[] checkQtyConstrain(double qty)
    {
        string[] result= new string[2];
        result[0] = "true";
        GridViewRow gvr = ((GridView)this.Parent.FindControl(hdnParentGridId.Value)).Rows[Int32.Parse(hdnParentGridIndex.Value)];
        switch (hdnUcProductSnoRefType.Value)
        {
            case "DV":
                if (Convert.ToDouble(((Label)gvr.FindControl("lblgvSystemQuantity")).Text) < qty)
                {
                    result[0] = "false";
                    result[1] = "There is no sufficient stock to fulfill this transaction";
                    return result;
                }

                if (Convert.ToDouble(((Label)gvr.FindControl("lblgvRemaningQuantity")).Text) < qty)
                {
                    result[0] = "false";
                    result[1] = "You can not select serail no more then remaining qty";
                    return result;
                }
                break;
            case "RV":
                break;
        }
        return result;
    }
    protected void setQtyOnParentGrid(string strQty)
    {
        GridViewRow gvr = ((GridView)this.Parent.FindControl(hdnParentGridId.Value)).Rows[Int32.Parse(hdnParentGridIndex.Value)];
        switch (hdnUcProductSnoRefType.Value)
        {
            case "DV":
                ((TextBox)gvr.FindControl("txtgvSalesQuantity")).Text = strQty;
                //((UpdatePanel)this.Parent.FindControl("updPnlProductDtl")).Update();
                break;
            case "RV":
                break;
        }
    }
    protected void btnResetSerial_Click(object sender, EventArgs e)
    {
        ViewState["dtSerial"] = null;
        txtSerialNo.Text = "";
        gvSerialNumber.DataSource = null;
        gvSerialNumber.DataBind();
        txtCount.Text = "0";
        txtselectedSerialNumber.Text = "0";
        if (Session["dtFinal"] != null)
        {
            DataTable Dtfinal = (DataTable)Session["dtFinal"];
            if (hdnUcProductSnoOrderId.Value == "0")
            {
                Dtfinal = new DataView(Dtfinal, "Product_Id<>'" + hdnUcProductSnoProductId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            else
            {
                Dtfinal = new DataView(Dtfinal, "Product_Id<>'" + hdnUcProductSnoProductId.Value + "' and  SOrderNo<>'" + hdnUcProductSnoOrderId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                int SOId = Convert.ToInt32(hdnUcProductSnoOrderId.Value);
            }
            Session["dtFinal"] = Dtfinal;
        }
        setQtyOnParentGrid("0");
        txtSerialNo.Focus();
    }
    public static string[] isSerialNumberValid(DataTable dtserial, string serialNumber, string ProductId, string TransId, GridView gvSerialNumber)
    {
        Inv_StockBatchMaster ObjStockBatchMaster = new Inv_StockBatchMaster(HttpContext.Current.Session["DBConnection"].ToString());
        SystemParameter ObjSysParam = new SystemParameter(HttpContext.Current.Session["DBConnection"].ToString());
        string[] Result = new string[5];
        int counter = 0;
        if (dtserial != null)
        {
            if (new DataView(dtserial, "SerialNo='" + serialNumber.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
            {
                counter = 1;
            }
        }

        if (counter == 0)
        {
            dtserial = ObjStockBatchMaster.GetStockBatchMaster_By_ProductId_and_SerialNo(ProductId, serialNumber);


            if (dtserial.Rows.Count > 0)
            {

                if (dtserial.Rows[0]["InOut"].ToString() == "O")
                {
                    Result[0] = "ALREADY OUT";
                }
                else if (dtserial.Rows[0]["InOut"].ToString() == "I")
                {
                    Result[0] = "VALID";
                    Result[1] = dtserial.Rows[0]["SerialNo"].ToString();
                    Result[2] = Convert.ToDateTime(dtserial.Rows[0]["ManufacturerDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                    Result[3] = Convert.ToDateTime(dtserial.Rows[0]["ExpiryDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                    Result[4] = dtserial.Rows[0]["BatchNo"].ToString();
                }
            }
            else
            {
                Result[0] = "NOT EXISTS";
            }
        }
        else
        {
            Result[0] = "DUPLICATE";
        }

        return Result;
    }
    public void setValues(string strRefType, string parentGridId, int parentGridIndex)
    {
        try
        {
            string strProductCode = string.Empty;
            string strProductName = string.Empty;
            GridViewRow gvr = ((GridView)this.Parent.FindControl(parentGridId)).Rows[parentGridIndex];
            hdnParentGridId.Value = parentGridId;
            hdnParentGridIndex.Value = parentGridIndex.ToString();
            hdnUcProductSnoRefType.Value = strRefType;
            switch (strRefType)
            {
                case "DV": //deliver voucher
                    hdnUcProductSnoOrderId.Value = string.IsNullOrEmpty(((Label)gvr.FindControl("lblSOId")).Text) ? "0" : ((Label)gvr.FindControl("lblSOId")).Text;
                    hdnUcProductSnoRefId.Value = string.IsNullOrEmpty(((HiddenField)this.Parent.FindControl("editid")).Value) ? "0" : ((HiddenField)this.Parent.FindControl("editid")).Value;
                    hdnUcProductSnoProductId.Value = ((HiddenField)gvr.FindControl("hdngvProductId")).Value;
                    strProductCode = ((Label)gvr.FindControl("lblgvProductCode")).Text;
                    strProductName = ((Label)gvr.FindControl("lblgvProductName")).Text;
                    break;
                case "RV": //Receive Voucher
                    break;
            }


            lblCount.Text = Resources.Attendance.Serial_No + ":-";
            txtCount.Text = "0";
            lblProductIdvalue.Text = strProductCode;
            lblProductNameValue.Text = strProductName;
            gvSerialNumber.DataSource = null;
            gvSerialNumber.DataBind();
            txtSerialNo.Text = "";
            ViewState["PID"] = hdnUcProductSnoProductId.Value;
            if (Session["dtFinal"] != null)
            {
                using (DataTable dt = new DataView((DataTable)Session["dtFinal"], "Product_Id='" + hdnUcProductSnoProductId.Value + "' and SOrderNo='" + hdnUcProductSnoOrderId.Value + "'", "", DataViewRowState.CurrentRows).ToTable())
                {
                    objPageCmn.FillData((object)gvSerialNumber, dt, "", "");
                    ViewState["dtSerial"] = dt;
                    txtselectedSerialNumber.Text = gvSerialNumber.Rows.Count.ToString();
                }
            }
            txtSerialNo.Focus();
        }
        catch (Exception ex)
        {

        }
    }
    public void initialiseSerialTbl()
    {
        if (Session["dtFinal"] == null)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Product_Id");
            dt.Columns.Add("SerialNo");
            dt.Columns.Add("SOrderNo");
            dt.Columns.Add("BarcodeNo");
            dt.Columns.Add("BatchNo");
            dt.Columns.Add("LotNo");
            dt.Columns.Add("ExpiryDate");
            dt.Columns.Add("ManufacturerDate");
            Session["dtFinal"] = dt;
        }
    }
    protected void FUAll_FileUploadComplete(object sender, EventArgs e)
    {
        if (FULogoPath.HasFile)
        {
            FULogoPath.SaveAs(Server.MapPath("~/Temp/" + FULogoPath.FileName));
        }
    }
}