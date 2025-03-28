using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;


public partial class EmailSystem_ContactMailReference : System.Web.UI.Page
{
    Set_CustomerMaster ObjCustomer = null;
    Ems_ContactMaster ObjContactMaster = null;

    DepartmentMaster ObjDepMaster = null;
    SystemParameter objSysParam = null;
    Inv_CustomerInquiry ObjCustInquiry = null;
    Set_DocNumber ObjDoc = null;
    Inv_ReferenceMailContact objMailContact = null;
    Arc_FileTransaction ObjFile = null;
    Arc_Directory_Master objDir = null;
    ES_RefereneceMailArchaving objRefArch = null;
    Common cmn = null;
    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string strLocationId = string.Empty;
    string StrUserId = string.Empty;
    string StrCurrencyId = string.Empty;
    string Ref_Type = string.Empty;
    string Ref_Id = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {


        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();
        StrUserId = Session["UserId"].ToString();
        StrCurrencyId = Session["CurrencyId"].ToString();
        try
        {
            Ref_Type = Request.QueryString["Page"].ToString();
        }
        catch
        {


        }
        try
        {
            Ref_Id = Request.QueryString["Id"].ToString();
        }
        catch
        {
            Ref_Id = Request.QueryString["ConId"].ToString();
        }

       

        ObjCustomer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        ObjContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
        ObjDepMaster = new DepartmentMaster(Session["DBConnection"].ToString());
        objSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjCustInquiry = new Inv_CustomerInquiry(Session["DBConnection"].ToString());
        ObjDoc = new Set_DocNumber(Session["DBConnection"].ToString());
        objMailContact = new Inv_ReferenceMailContact(HttpContext.Current.Session["DBConnection_ES"].ToString());
        objRefArch = new ES_RefereneceMailArchaving(HttpContext.Current.Session["DBConnection_ES"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjFile = new Arc_FileTransaction(Session["DBConnection"].ToString());
        objDir = new Arc_Directory_Master(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            if (Request.QueryString["ProductId"] != null)
            {
                DataTable dt = fillArchFile(Request.QueryString["ProductId"].ToString(), true);
                gvFileMasterSearch.DataSource = dt;
                gvFileMasterSearch.DataBind();
                ViewState["dtSerchArcaWing"] = dt;

            }
            else
            {

            }
            if (Request.QueryString["ConId"] != null)
            {
                FillContactGrid(Request.QueryString["ConId"].ToString());

            }
            
            DataTable dtRefContact = objMailContact.GetRecord_By_RefType_and_RefId(StrCompId, StrBrandId, strLocationId, Ref_Type, Ref_Id, Session["DBConnection"].ToString());

            if (dtRefContact.Rows.Count != 0)
            {
                GvFinalContacts.DataSource = dtRefContact;
                GvFinalContacts.DataBind();
                ViewState["DtContact"] = dtRefContact;

            }
            DataTable dtrefArch = objRefArch.GetAllRecord(StrCompId, StrBrandId, strLocationId, Ref_Type, Ref_Id, Session["DBConnection"].ToString());
            if (dtrefArch.Rows.Count != 0)
            {
                dtrefArch.Columns.Add("Product_Id");
                foreach (DataRow dr in dtrefArch.Rows)
                {
                    dr["Product_Id"] = dr["Directory_Name"].ToString().Split('/')[3].ToString();
                }
                gvFileMaster.DataSource = dtrefArch;
                gvFileMaster.DataBind();
                ViewState["dtArcaWing"] = dtrefArch;

            }

            // Reset();
        }

    }

    public void FillContactGrid(string ContactId)
    {
        DataTable dtContact = ObjContactMaster.GetContactTrueById(ContactId);

        foreach (DataRow dr in dtContact.Rows)
        {
            if (dr["Status"].ToString() == "Company" && dr["Company_Id"].ToString()=="0")
            {
                chkCompany.Enabled = true;
                chkCompany.Checked = true;
            }
            if (dr["Status"].ToString() == "Individual" && dr["Company_Id"].ToString() == "0")
            {
                chkIndividual.Enabled = true;
                chkIndividual.Checked = true;
            }
        }

        chkCompany_OnCheckedChanged(null, null);

        //try
        //{
        //    dtContact = new DataView(dtContact, "Trans_id in (" + Request.QueryString["ConId"].ToString() + ")", "", DataViewRowState.CurrentRows).ToTable();
        //}
        //catch
        //{
        //}


        //dtContact = new DataView(dtContact, "Status='Company' and Company_Id=0", "", DataViewRowState.CurrentRows).ToTable();

        //if (dtContact.Rows.Count > 0)
        //{
        //    chkCompany.Checked = true;

        //    chkCompanyList.DataSource = dtContact;
        //    chkCompanyList.DataTextField = "Name";
        //    chkCompanyList.DataValueField = "Trans_Id";
        //    chkCompanyList.DataBind();
        //    pnlCompany.Visible = true;

        //}
        //if (dtContact.Rows.Count != 0)
        //{
        //    if (dtContact.Rows.Count == 1)
        //    {


        //        if (dtContact.Rows[0]["Status"].ToString() == "Company")
        //        {
        //            ddlCustType.SelectedValue = "Company";
        //            chkCompany.Checked = true;

        //            chkCompanyList.DataSource = dtContact;
        //            chkCompanyList.DataTextField = "Name";
        //            chkCompanyList.DataValueField = "Trans_Id";
        //            chkCompanyList.DataBind();
        //            GvCompany.DataSource = dtContact;
        //            GvCompany.DataBind();
        //            pnlCompany.Visible = true;

        //        }
        //        else
        //        {
        //            chkIndividual.Checked = true;
        //            ddlCustType.SelectedValue = "Individual";
        //            GvIndividual.DataSource = dtContact;
        //            GvIndividual.DataBind();
        //            pnlIndividual.Visible = true;
        //        }
        //    }
        //    else
        //    {
        //        chkCompany.Checked = true;
        //        ddlCustType.SelectedValue = "Company";
        //        chkCompanyList.DataSource = dtContact;
        //        chkCompanyList.DataTextField = "Name";
        //        chkCompanyList.DataValueField = "Trans_Id";
        //        chkCompanyList.DataBind();
        //        GvCompany.DataSource = dtContact;
        //        GvCompany.DataBind();
        //        pnlCompany.Visible = true;

        //    }

        //}

        //ddlCustType.Enabled = false;
    }
    public void Reset()
    {


        GvIndividual.DataSource = null;
        GvIndividual.DataBind();
        chkDepartmentList.DataSource = null;
        chkDepartmentList.DataBind();

        GvDepartment.DataSource = null;
        GvIndividual.DataBind();
        chkCompanyList.DataSource = null;
        chkCompanyList.DataBind();
        GvContact.DataSource = null;
        GvContact.DataBind();
        //int b = objMailContact.DeleteRecord_By_RefTypeandRefId(StrCompId, StrBrandId, strLocationId, Ref_Type, Ref_Id);


    }
    #region System Defined Function

    //protected void ddlCustType_SelectedIndexChanged(object sender, EventArgs e)
    //{


    //    //Session["CustType"] = ddlCustType.SelectedValue.ToString();
    //    DataTable dtContact = ObjContactMaster.GetContactTrueAllData();

    //    if (ddlCustType.SelectedValue.ToString() == "Individual")
    //    {


    //        dtContact = new DataView(dtContact, "Status='" + ddlCustType.SelectedValue.ToString() + "' and Company_Id=0 and Trans_id in (" + Request.QueryString["ConId"].ToString() + ")", "", DataViewRowState.CurrentRows).ToTable();
    //        GvIndividual.DataSource = dtContact;
    //        GvIndividual.DataBind();
    //        GvCompany.DataSource = null;
    //        GvCompany.DataBind();
    //        chkCompanyList.DataSource = null;
    //        chkCompanyList.DataBind();
    //        GvDepartment.DataSource = null;
    //        GvDepartment.DataBind();
    //        chkDepartmentList.DataSource = null;
    //        chkDepartmentList.DataBind();
    //        GvContact.DataSource = null;
    //        GvContact.DataBind();
    //        pnlCompany.Visible = false;
    //        pnlIndividual.Visible = true;

    //        pnlCompanyContact.Visible = false;
    //        //Reset();

    //    }
    //    else if (ddlCustType.SelectedValue.ToString() == "Company")
    //    {
    //        GvIndividual.DataSource = null;
    //        GvIndividual.DataBind();
    //        dtContact = new DataView(dtContact, "Status='" + ddlCustType.SelectedValue.ToString() + "' and Company_Id=0 and Trans_id in (" + Request.QueryString["ConId"].ToString() + ")", "Name asc", DataViewRowState.CurrentRows).ToTable();
    //        chkCompanyList.DataSource = dtContact;
    //        chkCompanyList.DataTextField = "Name";
    //        chkCompanyList.DataValueField = "Trans_Id";
    //        chkCompanyList.DataBind();
    //        GvCompany.DataSource = dtContact;
    //        GvCompany.DataBind();
    //        pnlCompany.Visible = true;
    //        pnlIndividual.Visible = false;

    //    }
    //    else
    //    {
    //        chkCompanyList.DataSource = null;
    //        chkCompanyList.DataBind();
    //        GvCompany.DataSource = null;
    //        GvCompany.DataBind();
    //        GvCompany.DataSource = null;
    //        GvCompany.DataBind();
    //        chkDepartmentList.DataSource = null;
    //        chkDepartmentList.DataBind();
    //        GvDepartment.DataSource = null;
    //        GvDepartment.DataBind();
    //        GvContact.DataSource = null;
    //        GvContact.DataBind();
    //        pnlCompany.Visible = false;
    //        pnlIndividual.Visible = false;

    //        pnlCompanyContact.Visible = false;
    //        return;
    //    }


    //}

    protected void chkCompany_OnCheckedChanged(object sender, EventArgs e)
    {
        DataTable dtContact = ObjContactMaster.GetContactTrueAllData();

        if (chkIndividual.Checked == true)
        {

            dtContact = new DataView(dtContact, "Status='Individual' and Company_Id=0 and Trans_id in (" + Request.QueryString["ConId"].ToString() + ")", "", DataViewRowState.CurrentRows).ToTable();
            GvIndividual.DataSource = dtContact;
            GvIndividual.DataBind();
          
            //pnlCompany.Visible = false;
            pnlIndividual.Visible = true;
            // pnlCompanyContact.Visible = false;
            //Reset();
            if (GvIndividual.Rows.Count == 0)
            {
                lblIndividual.Visible = false;
            }

        }
        if (chkCompany.Checked == true)
        {
            dtContact = ObjContactMaster.GetContactTrueAllData();
            //GvIndividual.DataSource = null;
            //GvIndividual.DataBind();
            dtContact = new DataView(dtContact, "Status='Company' and Company_Id=0 and Trans_id in (" + Request.QueryString["ConId"].ToString() + ")", "Name asc", DataViewRowState.CurrentRows).ToTable();
            chkCompanyList.DataSource = dtContact;
            chkCompanyList.DataTextField = "Name";
            chkCompanyList.DataValueField = "Trans_Id";
            chkCompanyList.DataBind();
            GvCompany.DataSource = dtContact;
            GvCompany.DataBind();
            pnlCompany.Visible = true;
            pnlCompanyContact.Visible = true;
            if (GvContact.Rows.Count == 0)
            {
                lblContacts.Visible = false;
            }
            //pnlIndividual.Visible = false;

        }

        if (chkCompany.Checked == false)
        {
            
        pnlCompanyContact.Visible = false;
        pnlCompany.Visible = false;
        GvContact.DataSource = null;
        GvContact.DataBind();
       
        }

         if (chkIndividual.Checked == false)
        {
            pnlIndividual.Visible = false;
            GvIndividual.DataSource = null;
            GvIndividual.DataBind();
           
        }
        //else if (chkCompany.Checked == false)
        //{
        //    pnlCompany.Visible = false;
        //    pnlCompanyContact.Visible = false;
        //}
        //else if (chkIndividual.Checked == false)
        //{
        //    GvIndividual.DataSource = null;
        //    GvIndividual.DataBind();
           
          
        //}


        //else
        //{
        //    //chkCompanyList.DataSource = null;
        //    //chkCompanyList.DataBind();
        //    //GvCompany.DataSource = null;
        //    //GvCompany.DataBind();
        //    //GvCompany.DataSource = null;
        //    //GvCompany.DataBind();
        //    //chkDepartmentList.DataSource = null;
        //    //chkDepartmentList.DataBind();
        //    //GvDepartment.DataSource = null;
        //    //GvDepartment.DataBind();
        //    //GvContact.DataSource = null;
        //    //GvContact.DataBind();
        //    //pnlCompany.Visible = false;
        //    //pnlIndividual.Visible = false;

        //    //pnlCompanyContact.Visible = false;
        //    //return;
        //}

    }

    protected void chkGvContactSelect_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dtContact = null;
        CheckBox chk = ((CheckBox)sender);
        GridViewRow Row = (GridViewRow)((CheckBox)sender).Parent.Parent;
        if (chk.Checked == true)
        {

            if (ViewState["DtContact"] != null)
            {
                if (new DataView(((DataTable)ViewState["DtContact"]), "Trans_Id='" + ((Label)Row.FindControl("lblGvContactContactID")).Text + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count == 0)
                {
                    dtContact = ObjContactMaster.GetContactTrueById(((Label)Row.FindControl("lblGvContactContactID")).Text);
                    dtContact.Merge((DataTable)ViewState["DtContact"]);

                }
                else
                {
                    dtContact = (DataTable)ViewState["DtContact"];
                }

            }
            else
            {
                dtContact = ObjContactMaster.GetContactTrueById(((Label)Row.FindControl("lblGvContactContactID")).Text);
            }



        }
        else
        {
            dtContact = (DataTable)ViewState["DtContact"];

            dtContact = new DataView(dtContact,"Trans_Id<>'" + ((Label)Row.FindControl("lblGvContactContactID")).Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            {
                ViewState["DtContact"] = dtContact;
            }
           


        }


        GvFinalContacts.DataSource = dtContact;
        GvFinalContacts.DataBind();


        ViewState["DtContact"] = dtContact;

    }
    protected void chkgvContactSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dtContact = new DataTable();
        if (((CheckBox)GvContact.HeaderRow.FindControl("chkgvContactSelectAll")).Checked == true)
        {
            
           
            foreach (GridViewRow Row in GvContact.Rows)
            {
                ((CheckBox)Row.FindControl("chkGvContactSelect")).Checked = true;
                if (ViewState["DtContact"] != null)
                {
                    if (new DataView(((DataTable)ViewState["DtContact"]), "Trans_Id='" + ((Label)Row.FindControl("lblGvContactContactID")).Text + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count == 0)
                    {
                        dtContact = ObjContactMaster.GetContactTrueById(((Label)Row.FindControl("lblGvContactContactID")).Text);
                        dtContact.Merge((DataTable)ViewState["DtContact"]);
                        ViewState["DtContact"] = dtContact;

                    }
                    else
                    {
                        dtContact = (DataTable)ViewState["DtContact"];
                    }

                }
                else
                {
                    dtContact = ObjContactMaster.GetContactTrueById(((Label)Row.FindControl("lblGvContactContactID")).Text);
                    ViewState["DtContact"] = dtContact;
                }


            }

        }
        else
        {
            string ContactList = string.Empty;
            foreach (GridViewRow Row in GvContact.Rows)
            {
                ((CheckBox)Row.FindControl("chkGvContactSelect")).Checked =false;

                if (ContactList == "")
                {
                    ContactList = ((Label)Row.FindControl("lblGvContactContactID")).Text;
                }
                else
                {
                    ContactList = ContactList + "," + ((Label)Row.FindControl("lblGvContactContactID")).Text;

                }

            }
            if (ViewState["DtContact"] != null)
            {
                dtContact = (DataTable)ViewState["DtContact"];

                dtContact = new DataView(dtContact, "Trans_Id not in (" + ContactList + ")", "", DataViewRowState.CurrentRows).ToTable();

            }
                
        }
        GvFinalContacts.DataSource = dtContact;
        GvFinalContacts.DataBind();


        ViewState["DtContact"] = dtContact;
    }
    protected void chkgvIndividualselect_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dtContact = ObjContactMaster.GetContactTrueAllData();
        CheckBox chk = ((CheckBox)sender);
        GridViewRow Row = (GridViewRow)((CheckBox)sender).Parent.Parent;
        if (chk.Checked == true)
        {


            if (ViewState["DtContact"] != null)
            {
                if (new DataView(((DataTable)ViewState["DtContact"]), "Trans_Id='" + ((Label)Row.FindControl("lblGvIndividualContactID")).Text + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count == 0)
                {
                    dtContact = ObjContactMaster.GetContactTrueById(((Label)Row.FindControl("lblGvIndividualContactID")).Text);
                    dtContact.Merge((DataTable)ViewState["DtContact"]);

                }
                else
                {
                    dtContact = (DataTable)ViewState["DtContact"];
                }

            }
            else
            {
                dtContact = ObjContactMaster.GetContactTrueById(((Label)Row.FindControl("lblGvIndividualContactID")).Text);
            }


            //string id = ((Label)Row.FindControl("lblGvIndividualContactID")).Text;
            //dtContact = new DataView(dtContact, "Trans_Id = '" + Convert.ToInt32(id.ToString()) + "'", "Company_Id Asc", DataViewRowState.CurrentRows).ToTable();


        }
        else
        {
            dtContact = (DataTable)ViewState["DtContact"];

            dtContact = new DataView(dtContact, "Trans_Id<>'" + ((Label)Row.FindControl("lblGvIndividualContactID")).Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            {
                ViewState["DtContact"] = dtContact;
            }
           
        }



        GvFinalContacts.DataSource = dtContact;
        GvFinalContacts.DataBind();


        ViewState["DtContact"] = dtContact;


    }
    protected void chkGvIndividualSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dtContact = new DataTable();
        if (((CheckBox)GvIndividual.HeaderRow.FindControl("chkGvIndividualSelectAll")).Checked == true)
        {


            foreach (GridViewRow Row in GvIndividual.Rows)
            {
                ((CheckBox)Row.FindControl("chkgvIndividualselect")).Checked = true;
                if (ViewState["DtContact"] != null)
                {
                    if (new DataView(((DataTable)ViewState["DtContact"]), "Trans_Id='" + ((Label)Row.FindControl("lblGvIndividualContactID")).Text + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count == 0)
                    {
                        dtContact = ObjContactMaster.GetContactTrueById(((Label)Row.FindControl("lblGvIndividualContactID")).Text);
                        dtContact.Merge((DataTable)ViewState["DtContact"]);
                        ViewState["DtContact"] = dtContact;

                    }
                    else
                    {
                        dtContact = (DataTable)ViewState["DtContact"];
                    }

                }
                else
                {
                    dtContact = ObjContactMaster.GetContactTrueById(((Label)Row.FindControl("lblGvIndividualContactID")).Text);
                    ViewState["DtContact"] = dtContact;
                }


            }

        }
        else
        {
            string ContactList = string.Empty;
            foreach (GridViewRow Row in GvIndividual.Rows)
            {
                ((CheckBox)Row.FindControl("chkgvIndividualselect")).Checked = false;

                if (ContactList == "")
                {
                    ContactList = ((Label)Row.FindControl("lblGvIndividualContactID")).Text;
                }
                else
                {
                    ContactList = ContactList + "," + ((Label)Row.FindControl("lblGvIndividualContactID")).Text;

                }

            }
            if (ViewState["DtContact"] != null)
            {
                dtContact = (DataTable)ViewState["DtContact"];

                dtContact = new DataView(dtContact, "Trans_Id not in (" + ContactList + ")", "", DataViewRowState.CurrentRows).ToTable();

            }

        }
        GvFinalContacts.DataSource = dtContact;
        GvFinalContacts.DataBind();


        ViewState["DtContact"] = dtContact;
    }
    protected void chkCompanyList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string IndList = string.Empty;
        bool b = false;




        for (int i = 0; i < chkCompanyList.Items.Count; i++)
        {


            if (chkCompanyList.Items[i].Selected == true)
            {
                if (IndList == "")
                {
                    IndList = chkCompanyList.Items[i].Value;
                }
                else
                {
                    IndList = IndList + "," + chkCompanyList.Items[i].Value;
                }
                b = true;
            }

        }
        if (b)
        {
            DataTable dtDept = ObjContactMaster.GetContactTrueAllData();
            DataTable dtDeptTemp;
            string[] splitList = IndList.Split(',');


            if (splitList.GetUpperBound(0) > 0)
            {


                if (dtDept != null)
                {
                    dtDept = new DataView(dtDept, "Company_Id in (" + IndList + ")", "Company_Id asc", DataViewRowState.CurrentRows).ToTable();
                }

            }
            else
            {
                if (dtDept != null)
                {
                    try
                    {
                        dtDept = new DataView(dtDept, "Company_Id = '" + Convert.ToInt32(IndList.ToString()) + "'", "Company_Id asc", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {

                    }
                }


            }


            pnlCompanyContact.Visible = true;
            if (dtDept.Rows.Count > 0)
            {
                lblContacts.Visible = true;
            }
           
            GvContact.DataSource = dtDept;
            GvContact.DataBind();
            try
            {
                dtDept = new DataView(dtDept, "Dep_Id<>''", "Dep_Name asc", DataViewRowState.CurrentRows).ToTable(true, "Company_Id", "Dep_Id", "Dep_Name");
            }
            catch
            {
                dtDept = new DataView(dtDept, "Dep_Id<>'0'", "Dep_Name asc", DataViewRowState.CurrentRows).ToTable(true, "Company_Id", "Dep_Id", "Dep_Name");

            }
            GvDepartment.DataSource = dtDept;
            GvDepartment.DataBind();

        }
        else
        {
            pnlCompanyContact.Visible = false;
            GvContact.DataSource = null;
            GvContact.DataBind();
            lblContacts.Visible = false;
            GvDepartment.DataSource = null;
            GvDepartment.DataBind();
        }
    }

    protected void chkGvCompanySelect_CheckedChanged(object sender, EventArgs e)
    {
        string IndList = string.Empty;
        bool b = false;




        for (int i = 0; i < GvCompany.Rows.Count; i++)
        {

            CheckBox chkGvCompanySelect = ((CheckBox)GvCompany.Rows[i].FindControl("chkGvCompanySelect"));
            Label lblGvCompanyID = ((Label)GvCompany.Rows[i].FindControl("lblGvCompanyID"));
            if (((CheckBox)GvCompany.Rows[i].FindControl("chkGvCompanySelect")).Checked == true)
            {
                if (IndList == "")
                {
                    IndList = lblGvCompanyID.Text;
                }
                else
                {
                    IndList = IndList + "," + lblGvCompanyID.Text;
                }
                b = true;
            }

        }
        if (b)
        {
            DataTable dtDept = ObjContactMaster.GetContactTrueAllData();
            DataTable dtDeptTemp;
            string[] splitList = IndList.Split(',');


            if (splitList.GetUpperBound(0) > 0)
            {


                if (dtDept != null)
                {
                    dtDept = new DataView(dtDept, "Company_Id in (" + IndList + ")", "Company_Id asc", DataViewRowState.CurrentRows).ToTable();
                }

            }
            else
            {
                if (dtDept != null)
                {
                    try
                    {
                        dtDept = new DataView(dtDept, "Company_Id = '" + Convert.ToInt32(IndList.ToString()) + "'", "Company_Id asc", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {

                    }
                }


            }


            pnlCompanyContact.Visible = true;
            GvContact.DataSource = dtDept;
            GvContact.DataBind();
            try
            {
                dtDept = new DataView(dtDept, "Dep_Id<>''", "Dep_Name asc", DataViewRowState.CurrentRows).ToTable(true, "Company_Id", "Dep_Id", "Dep_Name");
            }
            catch
            {
                dtDept = new DataView(dtDept, "Dep_Id<>'0'", "Dep_Name asc", DataViewRowState.CurrentRows).ToTable(true, "Company_Id", "Dep_Id", "Dep_Name");

            }
            GvDepartment.DataSource = dtDept;
            GvDepartment.DataBind();
        }
        else
        {
            pnlCompanyContact.Visible = false;
            GvContact.DataSource = null;
            GvContact.DataBind();
            GvDepartment.DataSource = null;
            GvDepartment.DataBind();
        }




    }
    protected void chkGvDepartmentSelect_CheckedChanged(object sender, EventArgs e)
    {

        DataTable dtDeptContact;
        string IndList = string.Empty;
        string IndListDept = string.Empty;
        bool b = false;
        for (int i = 0; i < GvDepartment.Rows.Count; i++)
        {

            CheckBox chkGvDepartmentSelect = ((CheckBox)GvDepartment.Rows[i].FindControl("chkGvDepartmentSelect"));
            Label lblGvDepartmentCompID = ((Label)GvDepartment.Rows[i].FindControl("lblGvDepartmentCompID"));
            Label lblGvDepartmentID = ((Label)GvDepartment.Rows[i].FindControl("lblGvDepartmentID"));

            if (IndList == "")
            {
                IndList = lblGvDepartmentCompID.Text;
            }
            else
            {
                IndList = IndList + "," + lblGvDepartmentCompID.Text;
            }
            if (chkGvDepartmentSelect.Checked == true)
            {

                if (IndListDept == "")
                {
                    IndListDept = lblGvDepartmentID.Text;
                }
                else
                {
                    IndListDept = IndListDept + "," + lblGvDepartmentID.Text;
                }
                b = true;
            }


        }
        string[] splitList = IndList.Split(',');
        dtDeptContact = ObjContactMaster.GetContactTrueAllData();
        if (b)
        {

            string[] splitListDept = IndListDept.Split(',');


            if (splitList.GetUpperBound(0) > 0)
            {
                try
                {


                    dtDeptContact = new DataView(dtDeptContact, "Company_Id in (" + IndList + ") and Dep_Id in (" + IndListDept + ")", "Company_Id asc", DataViewRowState.CurrentRows).ToTable();

                }
                catch
                {

                }
            }
            else
            {
                try
                {
                    dtDeptContact = new DataView(dtDeptContact, "Company_Id = '" + Convert.ToInt32(IndList.ToString()) + "' and Dep_Id = '" + Convert.ToInt32(IndListDept.ToString()) + "'", "Company_Id asc", DataViewRowState.CurrentRows).ToTable();

                }
                catch
                {

                }
            }


        }
        else
        {
            if (splitList.GetUpperBound(0) > 0)
            {


                dtDeptContact = new DataView(dtDeptContact, "Company_Id in (" + IndList + ") ", "Company_Id asc", DataViewRowState.CurrentRows).ToTable();

            }
            else
            {
                try
                {
                    dtDeptContact = new DataView(dtDeptContact, "Company_Id = '" + Convert.ToInt32(IndList.ToString()) + "'", "Company_Id asc", DataViewRowState.CurrentRows).ToTable();

                }
                catch
                {

                }
            }

        }
        GvContact.DataSource = dtDeptContact;
        GvContact.DataBind();
    }


    protected void btnSend_Click(object sender, EventArgs e)
    {
        objMailContact.DeleteRecord_By_RefTypeandRefId(StrCompId, StrBrandId, strLocationId, Ref_Type, Ref_Id);
        objRefArch.DeleteRecord_By_RefTypeandId(StrCompId, StrBrandId, strLocationId, Ref_Type, Ref_Id);
        for (int i = 0; i < GvFinalContacts.Rows.Count; i++)
        {

            Label lblGvFinalContactsContactID = ((Label)GvFinalContacts.Rows[i].FindControl("lblGvFinalContactsContactID"));
            objMailContact.InsertRecord(StrCompId, StrBrandId, strLocationId, Ref_Type, Ref_Id, lblGvFinalContactsContactID.Text, "TO", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        }

        for (int i = 0; i < gvFileMaster.Rows.Count; i++)
        {

            HiddenField hdnTransId = ((HiddenField)gvFileMaster.Rows[i].FindControl("hdnTransId"));
            objRefArch.InsertRecord(StrCompId, StrBrandId, strLocationId, Ref_Type, Ref_Id, "Product", "0", hdnTransId.Value, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        }


        if (Request.QueryString["PageRefType"] == null)
        {

            Response.Redirect("../EmailSystem/SendMail.aspx?Page=" + Ref_Type + "&&Id=" + Ref_Id + "&&Url=" + Request.QueryString["Url"].ToString() + "&&IsRefCont=y&&IsRefArch=y");

        }
        else if (Request.QueryString["PageRefType"].ToString() == "SQ" || Request.QueryString["PageRefType"].ToString() == "PO")
        {
            Response.Redirect("../EmailSystem/SendMail.aspx?Page=" + Ref_Type + "&&Id=" + Ref_Id + "&&Url=" + Request.QueryString["Url"].ToString() + "&&IsRefCont=y&&IsRefArch=y&&PageRefType=" + Request.QueryString["PageRefType"].ToString() + "&&PageRefId=0&&ConId=" + Request.QueryString["ConId"].ToString());
        }
        else if (Request.QueryString["PageRefType"].ToString() == "RV_AGE")
        {
            Response.Redirect("../EmailSystem/SendMail.aspx?Page=" + Ref_Type + "&&Id=" + Ref_Id + "&&IsRefCont=y&&IsRefArch=y&&PageRefType=" + Request.QueryString["PageRefType"].ToString() + "&&PageRefId=0&&ConId=" + Request.QueryString["ConId"].ToString());
        }
    }
    //protected void BtnReset_Click(object sender, EventArgs e)
    //{
    //    ddlCustType.SelectedValue = "0";
    //    ddlCustType_SelectedIndexChanged(null, null);

    //    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlCustType);
    //}
    protected void BtnSave_Click(object sender, EventArgs e)
    {




    }



    protected void IbtnDeleteContact_Command(object sender, CommandEventArgs e)
    {


        DataTable dt = (DataTable)ViewState["DtContact"];
        DataView dv = new DataView(dt);
        dv.RowFilter = "Trans_Id";

        dt = (DataTable)dv.ToTable();
        ViewState["DtContact"] = (DataTable)dt;
        GvFinalContacts.DataSource = ViewState["DtContact"];
        GvFinalContacts.DataBind();
    }
    protected void imgBtnContactDelete_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = (DataTable)ViewState["DtContact"];
        dt = new DataView(dt, "Trans_Id<>'" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();



        GvFinalContacts.DataSource = dt;

        GvFinalContacts.DataBind();

        ViewState["DtContact"] = dt;
    }

    #endregion
    #region Auto Complete
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]

    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {

        Ems_ContactMaster ObjContact = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCustomer = ObjContact.GetContactTrueAllData();


        DataTable dtMain = new DataTable();

        if (HttpContext.Current.Session["CustType"].ToString() == "Individual")
        {
            dtCustomer = new DataView(dtCustomer, "Status='" + HttpContext.Current.Session["CustType"].ToString() + "' and Company_Id=0", "", DataViewRowState.CurrentRows).ToTable();
        }
        if (HttpContext.Current.Session["CustType"].ToString() == "Company")
        {
            dtCustomer = new DataView(dtCustomer, "Status='" + HttpContext.Current.Session["CustType"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        }

        dtMain = dtCustomer.Copy();



        string filtertext = "Name like '%" + prefixText + "%'";
        DataTable dtCon = new DataView(dtMain, filtertext, "", DataViewRowState.CurrentRows).ToTable();
        if (dtCon.Rows.Count == 0)
        {
            dtCon = dtCustomer;
        }
        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Name"].ToString() + "/" + dtCon.Rows[i]["Trans_Id"].ToString();
            }
        }
        return filterlist;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]

    public static string[] GetCompletionListDepartment(string prefixText, int count, string contextKey)
    {


        DataTable dtMain = new DataTable();


        DataTable dtDept = (DataTable)HttpContext.Current.Session["DeptList"];
        dtMain = dtDept.Copy();

        string filtertext = "Dep_Name like '%" + prefixText + "%'";
        DataTable dtCon = new DataView(dtMain, filtertext, "", DataViewRowState.CurrentRows).ToTable();
        if (dtCon.Rows.Count == 0)
        {
            dtCon = dtDept;
        }
        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {

                filterlist[i] = dtCon.Rows[i]["Dep_Name"].ToString() + "/" + dtCon.Rows[i]["Dep_Id"].ToString();

            }
        }
        return filterlist;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]

    public static string[] GetCompletionListContact(string prefixText, int count, string contextKey)
    {


        Ems_ContactMaster ObjContact = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCustomer = ObjContact.GetContactTrueAllData();


        DataTable dtMain = new DataTable();
        if (HttpContext.Current.Session["DeptId"].ToString() != "")
        {
            dtCustomer = new DataView(dtCustomer, "Dep_Id=" + HttpContext.Current.Session["DeptId"].ToString() + " and Company_Id=" + HttpContext.Current.Session["CompDeptId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        }

        dtMain = dtCustomer.Copy();



        string filtertext = "Name like '%" + prefixText + "%'";
        DataTable dtCon = new DataView(dtMain, filtertext, "", DataViewRowState.CurrentRows).ToTable();
        if (dtCon.Rows.Count == 0)
        {
            dtCon = dtCustomer;
        }
        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Name"].ToString() + "/" + dtCon.Rows[i]["Trans_Id"].ToString();
            }
        }
        return filterlist;
    }
    #endregion
    #region User Defined Function
    private void FillGvContactGrid(string id)
    {
        DataTable dtContact = objMailContact.GetRecord_By_RefType_and_RefId(StrCompId, StrBrandId, strLocationId, "PI", id.ToString(), Session["DBConnection"].ToString());



        if (dtContact.Rows.Count > 0)
        {
            GvContact.DataSource = dtContact;
            GvContact.DataBind();
        }
        else
        {
            GvContact.DataSource = null;
            GvContact.DataBind();
        }

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
    public string GetDepartmentName(string DepartmentId)
    {
        string DepartmentName = string.Empty;

        DataTable dtDepartment = ObjDepMaster.GetDepartmentMasterById(DepartmentId);
        try
        {
            DepartmentName = dtDepartment.Rows[0]["Dep_Name"].ToString();
        }
        catch
        {
        }

        return DepartmentName;
    }
    public string GetCompanyName(string CompanyId)
    {
        string CompanyName = string.Empty;

        DataTable dtCompany = ObjContactMaster.GetContactTrueById(CompanyId);
        try
        {
            CompanyName = dtCompany.Rows[0]["Name"].ToString();
        }
        catch
        {
        }

        return CompanyName;

    }
    #endregion



    protected void lnkDownload_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = new DataTable();

        try
        {

            dt = ObjFile.Get_FileTransaction_By_TransactionId(StrCompId, e.CommandArgument.ToString());
        }
        catch
        {
        }
        download(dt);


    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = new DataView(((DataTable)ViewState["dtArcaWing"]), "Trans_Id<>'" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        gvFileMaster.DataSource = dt;
        gvFileMaster.DataBind();
        ViewState["dtArcaWing"] = dt;
    }

    protected void chkbox_CheckedChanged(object sender, EventArgs e)
    {
        GridViewRow row = (GridViewRow)((CheckBox)sender).Parent.Parent;
        string strId = ((LinkButton)row.FindControl("lnkDownload")).CommandArgument.ToString();
        string lblProductId = ((Label)row.FindControl("lblProductId")).Text.ToString();
        DataTable dt = new DataTable();
        if (ViewState["dtArcaWing"] != null)
        {
            dt = new DataView((DataTable)ViewState["dtArcaWing"], "Trans_Id='" + strId + "'and Product_Id='" + lblProductId.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        }
        if (ViewState["dtSerchArcaWing"] != null)
        {
            if (dt.Rows.Count == 0)
            {
                dt = new DataTable();
                dt = new DataView((DataTable)ViewState["dtSerchArcaWing"], "Trans_Id='" + strId + "'and Product_Id='" + lblProductId.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (ViewState["dtArcaWing"] != null)
                {
                    dt.Merge((DataTable)ViewState["dtArcaWing"]);
                }
            }
            else
            {
                dt = (DataTable)ViewState["dtArcaWing"];
            }
        }


        gvFileMaster.DataSource = dt;
        gvFileMaster.DataBind();
        ViewState["dtArcaWing"] = dt;

    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductName(string prefixText, int count, string contextKey)
    {
        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = PM.GetProductMasterTrueAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());



        dt = new DataView(dt, "EProductName like '%" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();

        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["EProductName"].ToString() + "/" + dt.Rows[i]["ProductCode"].ToString();
        }
        return txt;
    }
    protected void btnsearch_click(object sender, EventArgs e)
    {
        DataTable dt = null;
        if (txtEProductName.Text != "")
        {
            try
            {
                dt = fillArchFile(txtEProductName.Text.Split('/')[1].ToString(), false);
            }
            catch
            {
                dt = null;
            }


        }
        else
        {
            dt = null;
        }
        ViewState["dtSerchArcaWing"] = dt;
        gvFileMasterSearch.DataSource = dt;
        gvFileMasterSearch.DataBind();
    }

    public DataTable fillArchFile(string ProductId, bool IsIdMultiple)
    {

        string DirectoryName = string.Empty;
        DataTable dtDir = new DataTable();
        DataTable dtArcaWing = new DataTable();
        DataTable dtDocument = new DataTable();
        if (!IsIdMultiple)
        {
            DirectoryName = Session["CompId"].ToString() + "/Product/" + Session["BrandId"].ToString() + "/" + ProductId.ToString();


            dtDir = objDir.GetDirectoryMaster_By_DirectoryName(Session["CompId"].ToString(), DirectoryName);

        }
        else
        {
            foreach (string str in ProductId.ToString().Split(','))
            {
                if (str != "")
                {
                    DirectoryName = Session["CompId"].ToString() + "/Product/" + Session["BrandId"].ToString() + "/" + str.ToString();


                    dtDir.Merge(objDir.GetDirectoryMaster_By_DirectoryName(Session["CompId"].ToString(), DirectoryName));

                }

            }
        }
        if (dtDir.Rows.Count > 0)
        {

            if (!IsIdMultiple)
            {
                dtArcaWing = ObjFile.Get_FileTransaction_By_Documentid(Session["CompId"].ToString(), "0", dtDir.Rows[0]["Id"].ToString());

            }
            else
            {
                foreach (DataRow dr in dtDir.Rows)
                {
                    dtArcaWing.Merge(ObjFile.Get_FileTransaction_By_Documentid(Session["CompId"].ToString(), "0", dr["Id"].ToString()));
                }

            }

            dtDocument.Columns.Add("Trans_Id", typeof(int));
            dtDocument.Columns.Add("File_Name", typeof(string));

            dtDocument.Columns.Add("Document_Id", typeof(string));
            dtDocument.Columns.Add("Document_Name", typeof(string));
            dtDocument.Columns.Add("Product_Id", typeof(string));
            for (int i = 0; i < dtArcaWing.Rows.Count; i++)
            {

                dtDocument.Rows.Add(i);
                dtDocument.Rows[i]["Trans_Id"] = dtArcaWing.Rows[i]["Trans_Id"].ToString();
                dtDocument.Rows[i]["File_Name"] = dtArcaWing.Rows[i]["File_Name"].ToString();
                dtDocument.Rows[i]["Document_Id"] = dtArcaWing.Rows[i]["Document_Master_Id"].ToString();
                dtDocument.Rows[i]["Document_Name"] = dtArcaWing.Rows[i]["Document_Name"].ToString();
                try
                {
                    dtDocument.Rows[i]["Product_Id"] = dtArcaWing.Rows[i]["Directory_Name"].ToString().Split('/')[3].ToString();
                }
                catch
                {
                }
            }


        }

        return dtDocument;


    }
    public void btnArchReset_click(object sender, EventArgs e)
    {
        txtEProductName.Text = "";
        gvFileMasterSearch.DataSource = null;
        gvFileMasterSearch.DataBind();
        gvFileMasterSearch.DataSource = fillArchFile(Request.QueryString["ProductId"].ToString(), true);
        gvFileMasterSearch.DataBind();
        ViewState["dtSerchArcaWing"] = fillArchFile(Request.QueryString["ProductId"].ToString(), true);
    }

    private void download(DataTable dt)
    {
        Byte[] bytes = (Byte[])dt.Rows[0]["File_Data"];
        Response.Buffer = true;
        Response.Charset = "";
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //Response.ContentType = dt.Rows[0]["ContentType"].ToString();
        Response.AddHeader("content-disposition", "attachment;filename="
        + dt.Rows[0]["File_Name"].ToString());
        Response.BinaryWrite(bytes);
        Response.Flush();
        Response.End();
    }



}
