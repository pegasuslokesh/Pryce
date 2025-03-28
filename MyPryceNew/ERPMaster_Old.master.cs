using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Threading;
using System.Net.NetworkInformation;

public partial class ERPMaster : System.Web.UI.MasterPage
{
    Common ObjCom = null;
    CompanyMaster ObjCompanyMaster = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    UserMaster objUserMaster = null;
    ModuleMaster ObjModuleMaster = null;
    SystemParameter ObjSysPeram = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        if (!IsPostBack)
        {
            //Page.Title = ObjSysPeram.GetSysTitle();
            ObjCom = new Common(Session["DBConnection"].ToString());
            ObjCompanyMaster = new CompanyMaster(Session["DBConnection"].ToString());
            ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
            ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
            objUserMaster = new UserMaster(Session["DBConnection"].ToString());
            ObjModuleMaster = new ModuleMaster(Session["DBConnection"].ToString());
            ObjSysPeram = new SystemParameter(Session["DBConnection"].ToString());
            // lblModuleIs.Text = "PegasusERP :: " + Session["HeaderText"].ToString();
            lnkUserName.Text = GetEmpName(Session["UserId"].ToString());
            setHeaderImage();

            label4.Text = "Version " + ObjSysPeram.GetSysParameterByParamName("Application_Version").Rows[0]["Param_Value"].ToString();
            if (Session["lang"] == null)
            {
                Session["lang"] = "1";
                Session["CompId"] = Session["CompId"].ToString();

                string strcompanyId = Session["CompId"].ToString();
                body1.Style[HtmlTextWriterStyle.Direction] = "ltr";
            }
            else if (Session["lang"].ToString() == "2")
            {
                body1.Style[HtmlTextWriterStyle.Direction] = "rtl";
            }
            else if (Session["lang"].ToString() == "1")
            {
                body1.Style[HtmlTextWriterStyle.Direction] = "ltr";
            }

            BasePage bs = new BasePage();

            try
            {
                lblCompany1.Text = Session["CompName"].ToString();
                lblLocation1.Text = Session["LocName"].ToString();
                lblBrand1.Text = Session["BrandName"].ToString();
                lblLanguage.Text = Session["Language"].ToString();
                txtCurrency.Text = Session["Currency"].ToString();
                lblFinancialyear.Text = Session["FinanceCode"].ToString();
            }
            catch
            {

            }
            txtDatetime.Text = DateTime.Now.ToString(ObjSysPeram.SetDateFormat()) + " " + DateTime.Now.ToShortTimeString();
            GetApplicationId();
        }
        Page.Title = ObjSysPeram.GetSysTitle();
        BindModuleList();
        PopulateAcrDynamically();

    }

    private void GetApplicationId()
    {
        DataTable DtApp_Id = ObjSysPeram.GetSysParameterByParamName("Application_Id");
    }
    public string GetEmpName(string UserId)
    {
        string EmpName = string.Empty;

        DataTable dtUser = new DataTable();
        if (Session["EmpId"].ToString() != "0")
        {
            dtUser = objUserMaster.GetUserMasterForUserName(Session["UserId"].ToString(), Session["LoginCompany"].ToString());
        }
        else
        {
            dtUser = objUserMaster.GetUserMasterByUserId(Session["UserId"].ToString(), Session["CompId"].ToString());
        }

        if (dtUser.Rows.Count > 0)
        {
            EmpName = dtUser.Rows[0]["EmpName"].ToString();
            if (EmpName == "")
            {
                EmpName = UserId;
            }
        }
        else
        {
            EmpName = UserId;
        }

        if (EmpName == "")
        {
            EmpName = UserId;
        }
        return EmpName;
    }
    private void setHeaderImage()
    {
        string imagepath = "";
        if (Session["AccordianId"] != null)
        {
            string Id = Session["AccordianId"].ToString();
            if (Id == "0")
            {
                //imagepath = "<img alt=Pegasus :: Project Management src=../Images/time_attendance_banner.png complete=complete width=100%/>";
                //LitImage.Text = imagepath;

                imagepath = "<img alt=Pegasus :: ERP src=../Images/Master_a.png complete=complete width=100%/>";
                LitImage.Text = imagepath;
            }
            else if (Id != "0")
            {
                DataTable dt1 = ObjModuleMaster.GetModuleMasterById(Id);
                try
                {
                    imagepath = dt1.Rows[0]["Module_Banner"].ToString();
                }
                catch
                {

                }
                LitImage.Text = imagepath;
            }
        }
    }
    public DataTable fillCompanybyUser()
    {
        DataTable dt = objUserMaster.GetUserMasterByUserId(Session["UserId"].ToString(), Session["CompId"].ToString());
        DataTable dtReturn = ObjCompanyMaster.GetCompanyMaster();
        if (dt.Rows.Count != 0)
        {
            if (dt.Rows[0]["Company_Id"].ToString() != "0" && dt.Rows[0]["Emp_Id"].ToString() != "0")
            {
                dtReturn = new DataView(dtReturn, "Company_Id='" + dt.Rows[0]["Company_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        return dtReturn;
    }
    public void PopulateAcrDynamically()
    {
        DataTable DtApp_Id = ObjSysPeram.GetSysParameterByParamName("Application_Id");
        string Application_Id = DtApp_Id.Rows[0]["Param_Value"].ToString();

        //if (Session["MyModule"] == null || Session["MyChild"] == null)
        //{
        DataTable dtAllModule1 = new DataTable();


        if (Session["ModuleDashBoardId"] != null)
        {
            tdpnlaccordian.Visible = true;

            dtAllModule1 = ObjCom.GetAccodion(Session["LoginCompany"].ToString(), Session["UserId"].ToString(), Application_Id).DefaultView.ToTable(true, "Module_Id", "Module_Name", "DashBoardIconUrl", "ParentId");

            try
            {
                dtAllModule1 = new DataView(dtAllModule1, "Module_Id=" + Session["ModuleDashBoardId"].ToString() + " or ParentId='" + Session["ModuleDashBoardId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {

            }
        }
        else
        {
            tdpnlaccordian.Visible = false;
        }
        Session["MyModule"] = dtAllModule1;


        DataTable dtAllChild1 = new DataTable();
        dtAllChild1 = ObjCom.GetAccodion(Session["LoginCompany"].ToString(), Session["UserId"].ToString(), Application_Id);

        for (int D = 0; D < dtAllChild1.Rows.Count; D++)
        {
            string strObjectIdforMail = dtAllChild1.Rows[D]["Object_Id"].ToString();
            if (strObjectIdforMail == "265")
            {
                lnkMailBox.Visible = true;
            }
        }

        Session["MyChild"] = dtAllChild1;


        Session["MyAllModule"] = ObjCom.GetModuleName();

        Session["MyAllChild"] = ObjCom.GetObjectName();
        //}


        HyperLink lbhyp;
        Literal litdd;
        AjaxControlToolkit.AccordionPane pn;
        int i = 0;
        int flag = 0;

        DataTable dtAllModule = (DataTable)Session["MyModule"];

        foreach (DataRow datarow in dtAllModule.Rows)
        {
            DataTable dtM = new DataView((DataTable)Session["MyAllModule"], "Module_Id='" + datarow["Module_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtM.Rows.Count == 0)
            {
                continue;
            }

            lbhyp = new HyperLink();
            litdd = new Literal();

            string strText = string.Empty;
            if (Session["lang"].ToString() == "1")
            {
                lbhyp.CssClass = "FrmTitle";
                string str = datarow["Module_Id"].ToString();
                lbhyp.Text = dtM.Rows[0]["Module_Name"].ToString();
                lbhyp.NavigateUrl = dtM.Rows[0]["DashBoard_Url"].ToString();
                lbhyp.Text = "<Table class='accordianh3'><tr><td valign='middle' align='left' width='30px'>" + dtM.Rows[0]["Module_Image"].ToString() + "</td><td valign='middle' align='left' width='220px'>" + lbhyp.Text + "</td></tr></table>";
            }
            else
            {
                //lbTitle.Text = dtM.Rows[0]["Module_Name_L"].ToString();
                //lbTitle.NavigateUrl = dtM.Rows[0]["DashBoard_Url"].ToString();
                //lbImg.ImageUrl = dtM.Rows[0]["Module_Image"].ToString();
                lbhyp.Text = dtM.Rows[0]["Module_Name_L"].ToString();
                lbhyp.NavigateUrl = dtM.Rows[0]["DashBoard_Url"].ToString();
                lbhyp.Text = "<Table class='accordianh3'><tr><td valign='middle' align='left' width='30px'>" + dtM.Rows[0]["Module_Image"].ToString() + "</td><td valign='middle' align='left' width='220px'>" + lbhyp.Text + "</td></tr></table>";
            }

            DataTable dtAllChild = new DataView((DataTable)Session["MyChild"], "Module_Id=" + datarow["Module_Id"].ToString() + "", "Order_Appear", DataViewRowState.CurrentRows).ToTable();

            strText = "<table>";
            foreach (DataRow childrow in dtAllChild.Rows)
            {
                DataTable dtObject = new DataView((DataTable)Session["MyAllChild"], "Object_Id='" + childrow["Object_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                if (Session["AccordianId"] != null)
                {
                    string strAccordianId = Session["AccordianId"].ToString();
                    if (strAccordianId != null && strAccordianId != "0")
                    {
                        if (datarow["Module_Id"].ToString().Trim() == strAccordianId)
                        {
                            flag = 1;
                        }
                    }
                }

                if (Session["lang"].ToString() == "1")
                {
                    if (datarow["Module_Id"].ToString() == "105" || datarow["Module_Id"].ToString() == "109" || datarow["Module_Id"].ToString() == "145")
                    {
                        strText = strText + "<tr><td align='left' class='accordianul'><a href=" + dtObject.Rows[0]["Page_Url"].ToString() + " target='_blank' class='acc'>" + dtObject.Rows[0]["Object_Name"].ToString() + "</a></tr></td>";
                    }
                    else
                    {
                        strText = strText + "<tr><td align='left' class='accordianul'><a href=" + dtObject.Rows[0]["Page_Url"].ToString() + " class='acc'>" + dtObject.Rows[0]["Object_Name"].ToString() + "</a></tr></td>";
                    }
                }
                else
                {
                    if (datarow["Module_Id"].ToString() == "105" || datarow["Module_Id"].ToString() == "109" || datarow["Module_Id"].ToString() == "145")
                    {
                        strText = strText + "<tr><td align='left' class='accordianul'><a href=" + dtObject.Rows[0]["Page_Url"].ToString() + " target='_blank' class='acc'>" + dtObject.Rows[0]["Object_Name_L"].ToString() + "</a></tr></td>";
                    }
                    else
                    {
                        strText = strText + "<tr><td align='left' class='accordianul'><a href=" + dtObject.Rows[0]["Page_Url"].ToString() + " target='_blank' class='acc'>" + dtObject.Rows[0]["Object_Name_L"].ToString() + "</a></tr></td>";
                    }
                }
            }

            strText = strText + "</table>";
            litdd.Text = strText;
            pn = new AjaxControlToolkit.AccordionPane();
            pn.ID = "Pane" + i;
            pn.HeaderContainer.Controls.Add(lbhyp);

            if (flag == 1)
            {
                flag = 0;
                acrDynamic.SelectedIndex = i;
            }

            pn.ContentContainer.Controls.Add(litdd);
            acrDynamic.Panes.Add(pn);
            ++i;
        }
        string name = string.Empty;

    }
    #region modulelist
    public void BindModuleList()
    {
        DataTable DtApp_Id = ObjSysPeram.GetSysParameterByParamName("Application_Id");
        string Application_Id = DtApp_Id.Rows[0]["Param_Value"].ToString();
        DataTable dtAllModule1 = ObjCom.GetAccodion(Session["LoginCompany"].ToString(), Session["UserId"].ToString(), Application_Id).DefaultView.ToTable(true, "ParentId", "ParentModuleName", "ParentDashBoardIconUrl", "ParentDashBoard_Url", "ParentModuleImage");
        dtModuellist.DataSource = dtAllModule1;
        dtModuellist.DataBind();
    }

    protected void lnkEditCommand(object sender, CommandEventArgs e)
    {
        //Session["AccordianId"] = e.CommandArgument.ToString();
        Session["ModuleDashBoardId"] = e.CommandArgument.ToString();

        if (e.CommandName == "")
        {
            Response.Redirect(Request.Url.AbsoluteUri);
        }
        else
        {
            Response.Redirect(e.CommandName);
        }
    }

    #endregion

    public void fillDropdown(DropDownList ddl, DataTable dt, string DataTextField, string DataValueField)
    {
        ddl.DataSource = dt;
        ddl.DataTextField = DataTextField;
        ddl.DataValueField = DataValueField;
        ddl.DataBind();
    }

    protected void Bld_onClick(object sender, EventArgs e)
    {
        Session["Page1"] = null;
        Session["Home"] = null;
        Response.Redirect("~/MasterSetup/Home.aspx");
    }


    public string[] GetIpAddress()
    {
        string[] IPAdd = new string[2];
        IPAdd[0] = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (string.IsNullOrEmpty(IPAdd[0]))
        IPAdd[0] = Request.ServerVariables["REMOTE_ADDR"];
        NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
        IPAdd[1] = nics[0].GetPhysicalAddress().ToString();
        return IPAdd;

    }
    protected void btnlogout_Click(object sender, ImageClickEventArgs e)
    {

        //user log out log entry 
        SystemLog.SaveSystemLog("User LogOut", DateTime.Now.ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(),"",GetIpAddress()[0].ToString(),GetIpAddress()[1].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["DBConnection"].ToString());
      
        Session.Abandon();
        Session.Clear();
        Response.Redirect("~/ERPLogin.aspx");
    }
    protected void ImgHome_Click(object sender, ImageClickEventArgs e)
    {
        Session["ModuleDashBoardId"] = null;

        if (Session["EmpId"].ToString() == "0")
        {
            Response.Redirect("~/Dashboard/ModuleDashboard.aspx");
        }
        else
        {

            //if (Convert.ToBoolean(new DataView(objUserMaster.GetUserMaster(), "Emp_Id=" + Session["EmpId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Field6"].ToString()))
            //{
            //    Response.Redirect("~/Dashboard/MasterDashboard.aspx");
            //}
            //else
            //{
            Response.Redirect("~/Dashboard/ModuleDashboard.aspx");
            //}
        }
    }
}
