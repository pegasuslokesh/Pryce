using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;

public partial class Production_Report_ProductionRequestReport : System.Web.UI.Page
{
    SystemParameter ObjSysParam = null;
    DataAccessClass ObjDa = null;
    LocationMaster objLocation = null;
    Common cmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjDa = new DataAccessClass(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "326", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            txtFrom_CalendarExtender.Format = Session["DateFormat"].ToString();
            txtTo_CalendarExtender2.Format = Session["DateFormat"].ToString();
            FillRequestLocation();
            Session["AccordianId"] = "168";
            Session["HeaderText"] = "Production Report";
        }
    }

    public void FillRequestLocation()
    {

        DataTable dtLocation = objLocation.GetLocationMaster(HttpContext.Current.Session["CompId"].ToString());
        dtLocation = new DataView(dtLocation, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "'", "Location_Name asc", DataViewRowState.CurrentRows).ToTable();

        new PageControlCommon(Session["DBConnection"].ToString()).FillData((object)ddlLocation, dtLocation, "Location_Name", "Location_Id");

    }
    public void DisplayMessage(string str,string color="orange")
    {
        if (Session["lang"] == null)
        {
            Session["lang"] = "1";
        }
        if (Session["lang"].ToString() == "1")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
        }
        else if (Session["lang"].ToString() == "2")
        {
             ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + GetArebicMessage(str) + "','"+color+"','white');", true);
        }
    }
    public string GetArebicMessage(string EnglishMessage)
    {
        string ArebicMessage = string.Empty;
        DataTable dtres = (DataTable)Session["MessageDt"];
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
    protected void btnShowReport_Click(object sender, EventArgs e)
    {

        Session["DtProduction"] = null;
        DataTable DtFilter = new DataTable();
        ProductionDataset ObjProductionDataset = new ProductionDataset();
        ObjProductionDataset.EnforceConstraints = false;

        ProductionDatasetTableAdapters.sp_Inv_ProductionRequestHeader_SelectRow_ReportTableAdapter objAdp = new ProductionDatasetTableAdapters.sp_Inv_ProductionRequestHeader_SelectRow_ReportTableAdapter();
        objAdp.Connection.ConnectionString = Session["DBConnection"].ToString();
        objAdp.Fill(ObjProductionDataset.sp_Inv_ProductionRequestHeader_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()));
        DtFilter = ObjProductionDataset.sp_Inv_ProductionRequestHeader_SelectRow_Report;

        if (DtFilter.Rows.Count == 0)
        {
            DisplayMessage("Record Not Found");
            return;
        }

        DtFilter = new DataView(DtFilter, "", "Request_Date asc", DataViewRowState.CurrentRows).ToTable();


        if (ddlStatus.SelectedIndex == 1)
        {
            DtFilter = new DataView(DtFilter, "Is_Production_Process='False' and Is_Production_Finish='False'", "", DataViewRowState.CurrentRows).ToTable();

            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);

                DtFilter = new DataView(DtFilter, "Request_Date>='" + txtFromDate.Text + "' and Request_Date<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            }


        }
        else if (ddlStatus.SelectedIndex == 2)
        {
            DtFilter = new DataView(DtFilter, "Is_Production_Process='True' and Is_Production_Finish='False'", "", DataViewRowState.CurrentRows).ToTable();

            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);

                DtFilter = new DataView(DtFilter, "Job_Creation_Date>='" + txtFromDate.Text + "' and Job_Creation_Date<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            }
        }
        else if (ddlStatus.SelectedIndex == 3)
        {
            DtFilter = new DataView(DtFilter, "Is_Production_Finish='True'", "", DataViewRowState.CurrentRows).ToTable();


            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);

                DtFilter = new DataView(DtFilter, "Job_Creation_Date>='" + txtFromDate.Text + "' and Job_Creation_Date<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            }
        }


        if (ddlLocation.SelectedIndex != 0)
        {
            try
            {

                DtFilter = new DataView(DtFilter, "Field2='" + ddlLocation.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {

            }

        }


        if (chkSalesOrder.Checked)
        {
            try
            {

                DtFilter = new DataView(DtFilter, "Order_No<>' '", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {

            }

        }



        if (DtFilter.Rows.Count == 0)
        {
            DisplayMessage("Record Not Found");
            return;
        }

        Session["DtProduction"] = DtFilter;

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Production_Report/ProductionReport.aspx?Type=" + ddlStatus.SelectedValue.Trim() + "','window','width=1024');", true);


    }

    protected void btnDailyProductionReport_Click(object sender, EventArgs e)
    {

        DataTable DtReport = new DataTable();






        DataColumn Status = new DataColumn();
        Status.ColumnName = "Status";
        Status.DataType = typeof(string);
        Status.AllowDBNull = true;
        Status.Unique = false;

        DataColumn ProductCode = new DataColumn();
        ProductCode.ColumnName = "ProductCode";
        ProductCode.DataType = typeof(string);
        ProductCode.AllowDBNull = true;
        ProductCode.Unique = false;

        DataColumn EProductName = new DataColumn();
        EProductName.ColumnName = "EProductName";
        EProductName.DataType = typeof(string);
        EProductName.AllowDBNull = true;
        EProductName.Unique = false;

        DataColumn OrderQty = new DataColumn();
        OrderQty.ColumnName = "OrderQty";
        OrderQty.DataType = typeof(string);
        OrderQty.AllowDBNull = true;
        OrderQty.Unique = false;

        DataColumn ProductionQty = new DataColumn();
        ProductionQty.ColumnName = "ProductionQty";
        ProductionQty.DataType = typeof(string);
        ProductionQty.AllowDBNull = true;
        ProductionQty.Unique = false;


        DataColumn MaterialName = new DataColumn();
        MaterialName.ColumnName = "MaterialName";
        MaterialName.DataType = typeof(string);
        MaterialName.AllowDBNull = true;
        MaterialName.Unique = false;



        DataColumn MachineName = new DataColumn();
        MachineName.ColumnName = "MachineName";
        MachineName.DataType = typeof(string);
        MachineName.AllowDBNull = true;

        MachineName.Unique = false;

        DataColumn Employee_Name = new DataColumn();
        Employee_Name.ColumnName = "Employee_Name";
        Employee_Name.DataType = typeof(string);
        Employee_Name.AllowDBNull = true;



        DataColumn Srno = new DataColumn();
        Srno.ColumnName = "Srno";
        Srno.DataType = typeof(float);
        Srno.AllowDBNull = true;
        Srno.Unique = false;

        DtReport.Columns.Add(Status);
        DtReport.Columns.Add(ProductCode);
        DtReport.Columns.Add(EProductName);
        DtReport.Columns.Add(OrderQty);
        DtReport.Columns.Add(ProductionQty);
        DtReport.Columns.Add(MaterialName);
        DtReport.Columns.Add(MachineName);
        DtReport.Columns.Add(Employee_Name);
        DtReport.Columns.Add(Srno);



        //first we get Pending order in datatable according selected criteria

        string strsql = string.Empty;



        strsql = "select Inv_ProductionRequestHeader.Request_Date,'Pending Orders' as Status,Inv_ProductMaster.ProductCode,Inv_ProductMaster.EProductName,Inv_ProductionRequestDetail.Quantity as OrderQty,0 as ProductionQty,' ' as MaterialName, ' ' as MachineName,' ' as Employee_Name  from Inv_ProductionRequestHeader  inner join Inv_ProductionRequestDetail on Inv_ProductionRequestHeader.Trans_Id=Inv_ProductionRequestDetail.Request_No  inner join Inv_ProductMaster on Inv_ProductionRequestDetail.ProductId=Inv_ProductMaster.ProductId where Inv_ProductionRequestHeader.Is_Production_Process='False' and Inv_ProductionRequestHeader.Is_Production_Finish='False' and Inv_ProductionRequestHeader.IsActive='True' and Inv_ProductionRequestHeader.Trans_Id not in (select Inv_Production_Process.Ref_Production_Req_No from Inv_Production_Process) and Inv_ProductionRequestHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_ProductionRequestHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_ProductionRequestHeader.Location_Id=" + Session["LocId"].ToString() + "";
        DataTable dtPendingOrder = ObjDa.return_DataTable(strsql);

        if (dtPendingOrder != null)
        {

            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                try
                {
                    dtPendingOrder = new DataView(dtPendingOrder, "Request_Date<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }

            }


            foreach (DataRow dr in dtPendingOrder.Rows)
            {
                DataRow drNew = DtReport.NewRow();

                drNew[0] = "PENDING ORDERS";
                drNew[1] = dr["ProductCode"].ToString();
                drNew[2] = dr["EProductName"].ToString();
                drNew[3] = dr["OrderQty"].ToString();
                drNew[4] = "";
                drNew[5] = "";
                drNew[6] = "";
                drNew[7] = "";
                drNew[8] = 1;


                DtReport.Rows.Add(drNew);
            }
        }


        //here we get Order In production



        strsql = "select Inv_Production_Process.Job_Creation_Date, 'Order In Production' as Status,PM.ProductCode,PM.EProductName,PD.Quantity as OrderQty,0 as ProductionQty,(SELECT STUFF((SELECT Distinct ',' + RTRIM(Inv_ProductMaster.EProductName)     FROM Inv_ProductMaster where Inv_ProductMaster.ProductId in (select distinct(Inv_Production_BOM.Item_Id) from Inv_Production_BOM where Inv_Production_BOM.Ref_Job_No=Inv_Production_Process.Id) FOR XML PATH('')),1,1,'') ) as MaterialName ,        Inv_Production_Process.Field1 as MachineName,        (SELECT STUFF((SELECT Distinct ',' + RTRIM(Inv_Production_Employee.Emp_Name)     FROM Inv_Production_Employee where Inv_Production_Employee.Ref_Job_No =Inv_Production_Process.Id FOR XML PATH('')),1,1,'') )  as Employee_Name         from Inv_ProductionRequestHeader as PH inner join Inv_ProductionRequestDetail as PD on PH.Trans_Id=Pd.Request_No  inner join Inv_ProductMaster as PM on PD.ProductId=PM.ProductId   inner join Inv_UnitMaster as UM on PD.UnitId=UM.Unit_Id   inner join Inv_Production_Process on Inv_Production_Process.Ref_Production_Req_No=PH.Trans_Id      Where PH.Company_Id=" + Session["CompId"].ToString() + " and PH.Brand_Id=" + Session["BrandId"].ToString() + " and PH.Location_Id=" + Session["LOcId"].ToString() + " and Ph.IsActive='True'      and  PH.Is_Production_Finish='False' and PH.Is_Production_Process='True' and Inv_Production_Process.IsActive='True'";

        DataTable dtOrderInProduction = ObjDa.return_DataTable(strsql);

        if (dtOrderInProduction != null)
        {

            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                try
                {

                    dtOrderInProduction = new DataView(dtOrderInProduction, "Job_Creation_Date<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
            }



            foreach (DataRow dr in dtOrderInProduction.Rows)
            {
                DataRow drNew = DtReport.NewRow();

                drNew[0] = "ORDER IN PRODUCTION";
                drNew[1] = dr["ProductCode"].ToString();
                drNew[2] = dr["EProductName"].ToString();
                drNew[3] = dr["OrderQty"].ToString();
                drNew[4] = "";
                drNew[5] = dr["MaterialName"].ToString();
                drNew[6] = dr["MachineName"].ToString();
                drNew[7] = dr["Employee_Name"].ToString();
                drNew[8] = 2;
                DtReport.Rows.Add(drNew);
            }
        }

        //here we get finish production with today date 

        strsql = "select Inv_Production_Process.Job_End,'Production Finish' as Status,PM.ProductCode,PM.EProductName,PD.Quantity as OrderQty,0  as ProductionQty,(SELECT STUFF((SELECT Distinct ',' + RTRIM(Inv_ProductMaster.EProductName)     FROM Inv_ProductMaster where Inv_ProductMaster.ProductId in (select distinct(Inv_Production_BOM.Item_Id) from Inv_Production_BOM where Inv_Production_BOM.Ref_Job_No=Inv_Production_Process.Id) FOR XML PATH('')),1,1,'') ) as MaterialName ,        Inv_Production_Process.Field1 as MachineName,        (SELECT STUFF((SELECT Distinct ',' + RTRIM(Inv_Production_Employee.Emp_Name)     FROM Inv_Production_Employee where Inv_Production_Employee.Ref_Job_No =Inv_Production_Process.Id FOR XML PATH('')),1,1,'') )  as Employee_Name         from Inv_ProductionRequestHeader as PH inner join Inv_ProductionRequestDetail as PD on PH.Trans_Id=Pd.Request_No  inner join Inv_ProductMaster as PM on PD.ProductId=PM.ProductId   inner join Inv_UnitMaster as UM on PD.UnitId=UM.Unit_Id   inner join Inv_Production_Process on Inv_Production_Process.Ref_Production_Req_No=PH.Trans_Id      Where PH.Company_Id=" + Session["CompId"].ToString() + " and PH.Brand_Id=" + Session["BrandId"].ToString() + " and PH.Location_Id=" + Session["LOcId"].ToString() + " and Ph.IsActive='True'      and  PH.Is_Production_Finish='True'  and Inv_Production_Process.IsActive='True'";

        DataTable dtProductionFinish = ObjDa.return_DataTable(strsql);


        if (dtProductionFinish != null)
        {
            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                try
                {
                    dtProductionFinish = new DataView(dtProductionFinish, "Job_End>='" + txtFromDate.Text + "' and Job_End<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }

            }
            else
            {
                DateTime FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 1);
                DateTime ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 1);
                try
                {
                    dtProductionFinish = new DataView(dtProductionFinish, "Job_End>='" + FromDate + "' and Job_End<='" + ToDate + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {

                }

            }



            foreach (DataRow dr in dtProductionFinish.Rows)
            {
                DataRow drNew = DtReport.NewRow();

                drNew[0] = "PRODUCTION FINISH";
                drNew[1] = dr["ProductCode"].ToString();
                drNew[2] = dr["EProductName"].ToString();
                drNew[3] = dr["OrderQty"].ToString();
                drNew[4] = dr["OrderQty"].ToString();
                drNew[5] = dr["MaterialName"].ToString();
                drNew[6] = dr["MachineName"].ToString();
                drNew[7] = dr["Employee_Name"].ToString();
                drNew[8] = 3;
                DtReport.Rows.Add(drNew);
            }
        }



        Session["DtDailyProduction"] = DtReport;


        if (txtFromDate.Text != "" && txtToDate.Text != "")
        {
            Session["DtFromDailyProduction"] = Convert.ToDateTime(txtFromDate.Text).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            Session["DtToDailyProduction"] = Convert.ToDateTime(txtToDate.Text).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        }
        else
        {
            Session["DtFromDailyProduction"] = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            Session["DtToDailyProduction"] = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        }

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Production_Report/ProductionReport.aspx?Type=DP','window','width=1024');", true);

        //DtReport.Merge(ObjDa.return_DataTable("select 'Production Finish' as Status,PM.ProductCode,PM.EProductName,PD.Quantity as OrderQty,0  as ProductionQty,(SELECT STUFF((SELECT Distinct ',' + RTRIM(Inv_ProductMaster.EProductName)     FROM Inv_ProductMaster where Inv_ProductMaster.ProductId in (select distinct(Inv_Production_BOM.Item_Id) from Inv_Production_BOM where Inv_Production_BOM.Ref_Job_No=Inv_Production_Process.Id) FOR XML PATH('')),1,1,'') ) as MaterialName ,        Inv_Production_Process.Field1 as MachineName,        (SELECT STUFF((SELECT Distinct ',' + RTRIM(Inv_Production_Employee.Emp_Name)     FROM Inv_Production_Employee where Inv_Production_Employee.Ref_Job_No =Inv_Production_Process.Id FOR XML PATH('')),1,1,'') )  as Employee_Name         from Inv_ProductionRequestHeader as PH inner join Inv_ProductionRequestDetail as PD on PH.Trans_Id=Pd.Request_No  inner join Inv_ProductMaster as PM on PD.ProductId=PM.ProductId   inner join Inv_UnitMaster as UM on PD.UnitId=UM.Unit_Id   inner join Inv_Production_Process on Inv_Production_Process.Ref_Production_Req_No=PH.Trans_Id      Where PH.Company_Id=" + Session["CompId"].ToString() + " and PH.Brand_Id=" + Session["BrandId"].ToString() + " and PH.Location_Id=" + Session["LOcId"].ToString() + " and Ph.IsActive='True'      and  PH.Is_Production_Finish='True'  and Inv_Production_Process.IsActive='True'"));


    }
}
