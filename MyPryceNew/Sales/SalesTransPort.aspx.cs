using PegasusDataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Sales_SalesTransPort : System.Web.UI.Page
{
    DataAccessClass objDa = null;
    Prj_VehicleMaster objVehicleMaster = null;
    Ems_ContactMaster objContact = null;
    EmployeeMaster ObjEmployeeMaster = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objVehicleMaster = new Prj_VehicleMaster(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        ObjEmployeeMaster = new EmployeeMaster(Session["DBConnection"].ToString());
        txtModalVisitDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

        DateTime CurrentTime = DateTime.Now;
        // Format the DateTime object to display only the hour and minute
        string formattedTime = CurrentTime.ToString("HH:mm");
        // Set the formatted time to the text box
        txtModalVisitTime.Text = formattedTime;
        if (!IsPostBack)
        {

            try
            {
                string CommandName = Request.QueryString["CommandName"].ToString();
                string CommandArgument = Request.QueryString["CommandArgument"].ToString();
                SetTransportId(CommandName, CommandArgument);
            }
            catch
            {

            }
        }
    }
    public void SetTransportId(string CommandName, string CommandArgument)
    {
        btnModalTransPortReset_Click(null, null);
        hdnSalesId.Value = CommandArgument;
        txtModalInvoiceNo.InnerText = CommandName;

        //transport Data Get
        DataTable dtTrnsPort = objDa.return_DataTable("SELECT * FROM Inv_InvoiceTransport where Ref_Id='" + hdnSalesId.Value + "' And Ref_Type='SI'");
        if (dtTrnsPort.Rows.Count > 0)
        {
            if (dtTrnsPort.Rows[0]["Customer_Id"].ToString() != "0")
            {
                DataTable dtCustomerName = objDa.return_DataTable("select * from Ems_ContactMaster where Trans_Id='" + dtTrnsPort.Rows[0]["Customer_Id"].ToString() + "'");
                if (dtCustomerName != null && dtCustomerName.Rows.Count > 0)
                {
                    string strCustomerEmail = dtCustomerName.Rows[0]["Field1"].ToString();
                    string strCustomerNumber = dtCustomerName.Rows[0]["Field2"].ToString();
                    txtModalcustomername.Text = objContact.GetContactNameByContactiD(dtTrnsPort.Rows[0]["Customer_Id"].ToString()) + "/" + strCustomerNumber + "/" + strCustomerEmail + "/" + dtTrnsPort.Rows[0]["Customer_Id"].ToString();
                    //txtCustomer.Text = dtInvEdit.Rows[0]["CustomerName"].ToString() + "/" + strCustomerId;
                }

                //txtModalcustomername.Text = GetCustomerNameByCustomerId(dtTrnsPort.Rows[0]["Customer_Id"].ToString()) + "/" + dtTrnsPort.Rows[0]["Customer_Id"].ToString();

            }
            if (dtTrnsPort.Rows[0]["Vehicle_Id"].ToString() != "0")
            {
                txtModalvehiclename.Text = GetvechileNameByVechileId(dtTrnsPort.Rows[0]["Vehicle_Id"].ToString()) + "/" + dtTrnsPort.Rows[0]["Vehicle_Id"].ToString();

            }
            if (dtTrnsPort.Rows[0]["Vehicle_Id"].ToString() != "0")
            {
                txtModalDriverName.Text = GetDriverNamebyDriverId(dtTrnsPort.Rows[0]["Vehicle_Id"].ToString()) + "/" + dtTrnsPort.Rows[0]["Driver_Id"].ToString();
            }
            txtModalChargableAmount.Text = dtTrnsPort.Rows[0]["Chargable_Amount"].ToString();
            txtModaldescription.Text = dtTrnsPort.Rows[0]["Description"].ToString();
            txtModalPermanentMobileNo.Text = dtTrnsPort.Rows[0]["Contact_No"].ToString();
            txtModalAreaName.Text = dtTrnsPort.Rows[0]["Field1"].ToString();
            txtModalPersonName.Text = dtTrnsPort.Rows[0]["Field2"].ToString();
            txtModalPersonMobileNo.Text = dtTrnsPort.Rows[0]["Field3"].ToString();
            txtModalTrakingId.Text = dtTrnsPort.Rows[0]["Field4"].ToString();
            txtModalVisitDate.Text = Convert.ToDateTime(dtTrnsPort.Rows[0]["Visit_Date"].ToString()).ToString("dd-MMM-yyyy");
            string strhour = dtTrnsPort.Rows[0]["Visit_Time"].ToString().Split(':')[0].ToString();
            string strminute = dtTrnsPort.Rows[0]["Visit_Time"].ToString().Split(':')[1].ToString();
            txtModalVisitTime.Text = strhour + ":" + strminute;
            if (txtModalDriverName.Text != "")
            {
                chkModalCustomer.Checked = false;
                chkModalEmployee.Checked = true;
                ChkModalTrans_Changed(null, null);
            }
        }

        ChkModalTrans_Changed(null, null);
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionContactList(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster ems = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string id = "0";

        DataTable dtCon = ems.GetAllContactAsPerFilterText(prefixText, id);
        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Filtertext"].ToString();
            }
        }
        return filterlist;
    }
    protected void ChkModalTrans_Changed(object sender, EventArgs e)
    {
        if (chkModalCustomer.Checked == true)
        {
            lblModalCourierService.Visible = true;
            txtModalcustomername.Visible = true;
            lblModalPermanentMobileNo.Visible = true;
            txtModalPermanentMobileNo.Visible = true;
            lblModalAreaName.Visible = true;
            txtModalAreaName.Visible = true;
            lblModalPersonName.Visible = true;
            txtModalPersonName.Visible = true;
            lblModalPersonMobileNo.Visible = true;
            txtModalPersonMobileNo.Visible = true;
            pnlModalCustomer.Visible = true;
            pnlModalEmployee.Visible = false;

        }
        else
        {
            txtModalcustomername.Visible = false;
            txtModalPersonName.Visible = false;
            txtModalPersonMobileNo.Visible = false;
            txtModalPermanentMobileNo.Visible = false;
            lblModalPersonName.Visible = false;
            lblModalPersonMobileNo.Visible = false;
            pnlModalEmployee.Visible = true;
            pnlModalCustomer.Visible = false;
        }
    }
    protected void TxtModalvehiclename_TextChanged(object sender, EventArgs e)
    {
        if (txtModalvehiclename.Text.Trim() != "")
        {

            DataTable dt = objVehicleMaster.GetAllTrueRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            try
            {
                dt = new DataView(dt, "Name='" + txtModalvehiclename.Text.Trim().Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Vehicle not found");
                txtModalvehiclename.Text = "";
                txtModalvehiclename.Focus();
                return;
            }
            else
            {
                if (dt.Rows[0]["Emp_id"].ToString() != "0")
                {
                    txtModalDriverName.Text = dt.Rows[0]["Emp_Name"].ToString() + "/" + dt.Rows[0]["Emp_Code"].ToString() + "/" + dt.Rows[0]["Emp_id"].ToString();
                }
            }
        }

    }
    protected void TxtModaldrivername_TextChanged(object sender, EventArgs e)
    {



        string empname = string.Empty;
        if (((TextBox)sender).Text != "")
        {
            try
            {
                empname = ((TextBox)sender).Text.Split('/')[1].ToString();
            }
            catch
            {
                DisplayMessage("Employee Not Exists");
                ((TextBox)sender).Text = "";
                ((TextBox)sender).Focus();
                return;
            }

            DataTable dtEmp = ObjEmployeeMaster.GetEmployeeMasterOnRole(Session["CompId"].ToString());

            dtEmp = new DataView(dtEmp, "Emp_Code='" + empname + "'", "", DataViewRowState.CurrentRows).ToTable();
            DataTable dt = objVehicleMaster.GetAllTrueRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            try
            {
                dt = new DataView(dt, "Emp_Code='" + empname + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Emp_id"].ToString() != "0")
                {
                    txtModalvehiclename.Text = dt.Rows[0]["Name"].ToString() + "/" + dt.Rows[0]["Vehicle_Id"].ToString();
                }
                else
                {
                    txtModalvehiclename.Text = "";
                }

            }
            else
            {
                txtModalvehiclename.Text = "";
            }
            if (dtEmp.Rows.Count == 0)
            {
                DisplayMessage("Employee Not Exists");
                ((TextBox)sender).Text = "";
                ((TextBox)sender).Focus();
                return;
            }
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAreaName(string prefixText, int count, string contextKey)
    {
        Sys_AreaMaster objAreaMaster = new Sys_AreaMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objAreaMaster.GetAreaMaster(), "Area_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Area_Name"].ToString();
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListVehicleName(string prefixText, int count, string contextKey)
    {
        Prj_VehicleMaster objVehicleMaster = new Prj_VehicleMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objVehicleMaster.GetAllRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), "Name like '%" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count == 0)
        {
            dt = objVehicleMaster.GetAllRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }
        dt = new DataView(dt, "Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count > 0)
        {
            string[] txt = new string[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["Name"].ToString() + "/" + dt.Rows[i]["Vehicle_Id"].ToString();
            }
            return txt;
        }
        return null;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDriverName(string prefixText, int count, string contextKey)
    {
        DataTable dt = Common.GetEmployee(prefixText, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["DBConnection"].ToString());
        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_Code"].ToString() + "/" + dt.Rows[i]["Emp_Id"].ToString();
        }
        return str;
    }
    protected void btnModalTransportSave_Click(object sender, EventArgs e)
    {
        if (chkModalCustomer.Checked)
        {
            if (txtModalcustomername.Text == "")
            {
                DisplayMessage("Please Fill Courior Service Name");
                return;
            }
            if (txtModalPersonName.Text == "")
            {
                DisplayMessage("Please Fill Person Name");
                return;
            }
            if (txtModalPersonMobileNo.Text == "")
            {
                DisplayMessage("Please Fill Person Mobile No.");
                return;
            }
            if (txtModalChargableAmount.Text == "")
            {
                DisplayMessage("Please Fill Chargable Amount");
                return;
            }
            if (txtModalTrakingId.Text == "")
            {
                DisplayMessage("Please Fill Tracking Id");
                return;
            }
        }
        else if (chkModalEmployee.Checked)
        {
            if (txtModalVisitDate.Text == "")
            {
                DisplayMessage("Please Fill Visit Date");
                return;
            }
            if (txtModalVisitTime.Text == "")
            {
                DisplayMessage("Please Fill Visit Time");
                return;
            }
            if (txtModalDriverName.Text == "")
            {
                DisplayMessage("Please Fill Employee Name");
                return;

            }
        }


        string VechileId = "0";
        string DriverId = "0";
        string strTransportEmpID = "0";
        string strAddressId = "0";

        try
        {
            string ShippingAddress = objDa.get_SingleValue("Select Field3 from Inv_SalesInvoiceHeader where Trans_Id='" + hdnSalesId.Value.ToString() + "'");
            if (ShippingAddress != null)
            {
                strAddressId = ShippingAddress;
            }
        }
        catch
        {

        }
        try
        {
            VechileId = txtModalvehiclename.Text.Split('/')[1].ToString();
        }
        catch
        {
            VechileId = "0";
        }

        try
        {
            DriverId = txtModalDriverName.Text.Split('/')[2].ToString();
        }
        catch
        {
            DriverId = "0";

        }

        strTransportEmpID = "0";

        string strTransportCustomertId = "0";

        if (txtModalcustomername.Text != "")
        {
            DataTable dtSupp = objContact.GetContactByContactName(txtModalcustomername.Text.Trim().Split('/')[0].ToString().Trim());
            if (dtSupp.Rows.Count > 0)
            {
                strTransportCustomertId = txtModalcustomername.Text.Split('/')[3].ToString();
                DataTable dtCompany = objContact.GetContactTrueById(strTransportCustomertId);
                if (dtCompany.Rows.Count > 0)
                {

                }
                else
                {
                    strTransportCustomertId = "0";
                }
            }
            else
            {
                strTransportCustomertId = "0";
            }
        }
        DateTime VisitDate;

        if (!string.IsNullOrEmpty(txtModalVisitDate.Text))
        {
            VisitDate = Convert.ToDateTime(txtModalVisitDate.Text);
        }
        else
        {
            VisitDate = DateTime.Now;
        }
        string InvoiceEditId = hdnSalesId.Value;
        try
        {
            objDa.execute_Command("Delete From Inv_InvoiceTransport  where Ref_Id='" + InvoiceEditId + "' And Ref_Type='SI'");
        }
        catch
        {


        }
        //objDa.execute_Command("Update Inv_InvoiceTransport Set Customer_Id='" + strTransportCustomertId + "',Emp_Id='" + strTransportEmpID + "',Vehicle_Id='" + VechileId + "',Driver_Id='" + DriverId + "',Description='" + txtdescription.Text + "',Chargable_Amount='" + txtChargableAmount.Text + "',Contact_Address='" + strAddressId + "',Contact_No='" + txtPermanentMobileNo.Text + "',Visit_Date='" + VisitDate.ToString("yyyy-MM-dd") + "',Visit_Time='" + txtVisitTime.Text + "',Field1='" + txtAreaName.Text + "',Is_Active='1',Modified_By='" + Session["UserId"].ToString() + "',Modified_Date='" + DateTime.Now.ToString("yyyy-MM-dd") + "' where Ref_Id='" + editid.Value + "'");
        try
        {
            objDa.execute_Command("INSERT INTO [dbo].[Inv_InvoiceTransport]([Ref_Type],[Ref_Id],[Customer_Id],[Emp_Id],[Vehicle_Id],[Driver_Id],[Description],[Chargable_Amount],[Contact_Address],[Contact_No],[Visit_Date],[Visit_Time],[Field1],[Field2],[Field3],[Field4],[Field5],[Field6],[Field7],[Is_Active],[Created_By],[Created_Date],[Modified_By],[Modified_Date]) VALUES ('SI','" + InvoiceEditId.ToString() + "','" + strTransportCustomertId + "','" + strTransportEmpID + "','" + VechileId + "','" + DriverId + "','" + txtModaldescription.Text + "','" + txtModalChargableAmount.Text + "','" + strAddressId + "','" + txtModalPermanentMobileNo.Text + "','" + VisitDate.ToString("yyyy-MM-dd") + "','" + txtModalVisitTime.Text + "','" + txtModalAreaName.Text + "','" + txtModalPersonName.Text + "','" + txtModalPersonMobileNo.Text + "','" + txtModalTrakingId.Text + "','','1','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','1','superadmin','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','superadmin','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')");
            //DisplayMessage("Transport Information Save Successfully");
            btnModalTransPortReset_Click(null, null);

        }
        catch (Exception exp)
        {
            string Message = "Data not Save";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseWindow", "closeWindow(" + Message + ");", true);
            // DisplayMessage("Data not Saved");
        }

        string message = "Data Saved";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseWindow", "closeWindow('" + message + "');", true);




    }
    protected void btnModalTransPortReset_Click(object sender, EventArgs e)
    {
        txtModalcustomername.Text = "";

        txtModalPersonName.Text = "";
        txtModalPersonMobileNo.Text = "";
        txtModalPermanentMobileNo.Text = "";
        txtModalAreaName.Text = "";
        txtModalChargableAmount.Text = "";
        txtModalTrakingId.Text = "";
        txtModaldescription.Text = "";
        txtModalVisitDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        DateTime CurrentTime = DateTime.Now;
        // Format the DateTime object to display only the hour and minute
        string formattedTime = CurrentTime.ToString("HH:mm");
        // Set the formatted time to the text box
        txtModalVisitTime.Text = formattedTime;
        txtModalvehiclename.Text = "";
        txtModalDriverName.Text = "";
    }
    public string GetCustomerNameByCustomerId(string CustomerId)
    {
        string CustomerName = "";
        string Name = objContact.GetContactNameByContactiD(CustomerId);
        //DataTable dt = ObjCustmer.GetCustomerAllDataByCustomerId(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), CustomerId);
        if (Name != null)
        {
            CustomerName = Name;
        }
        return CustomerName;
    }
    public string GetvechileNameByVechileId(string VechileId)
    {
        string VechileName = "";
        DataTable dt = objVehicleMaster.GetRecord_By_VehicleId(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), VechileId);
        if (dt.Rows.Count > 0)
        {
            VechileName = dt.Rows[0]["Name"].ToString();
        }
        return VechileName;
    }
    public string GetDriverNamebyDriverId(string DriverId)
    {
        string DriverName = "";
        DataTable dt = objVehicleMaster.GetRecord_By_VehicleId(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), DriverId);
        if (dt.Rows.Count > 0)
        {
            DriverName = dt.Rows[0]["Emp_Name"].ToString();
        }
        return DriverName;
    }
    public void DisplayMessage(string str, string color = "orange")
    {
        if (Session["lang"] == null)
        {
            Session["lang"] = "1";
        }
        if (Session["lang"].ToString() == "1")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
        }
        else if (Session["lang"].ToString() == "2")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + GetArebicMessage(str) + "','" + color + "','white');", true);
        }
    }
    public string GetArebicMessage(string EnglishMessage)
    {
        string ArebicMessage = string.Empty;
        DataTable dtres = (DataTable)ViewState["MessageDt"];
        if (dtres.Rows.Count != 0)
        {
            ArebicMessage = (new DataView(dtres, "Key='" + EnglishMessage + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Value"].ToString();
        }
        if (ArebicMessage == "")
        {
            ArebicMessage = EnglishMessage;
        }
        return ArebicMessage;
    }
}