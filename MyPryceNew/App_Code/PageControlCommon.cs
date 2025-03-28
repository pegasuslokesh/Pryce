using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Reflection;
using System.Collections.Generic;
using DevExpress.Web;
using System.Net.NetworkInformation;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

using System.Configuration;


using System.Text.RegularExpressions;
using System.Resources;
using System.Collections;
/// <summary>
/// Summary description for PageControlCommon
/// </summary>
public class PageControlCommon
{
    DataAccessClass daClass = null;
    CompanyMaster objComp = null;
    BrandMaster objbrand = null;
    LocationMaster objLocation = null;
    Set_AddressChild Objaddress = null;
    Set_Location_Department objLocDept = null;
    RoleDataPermission objRoleData = null;
    UserPermission objUserPermission = null;
    DataAccessClass objDa = null;
    private string _strConString = string.Empty;
    public PageControlCommon(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
        objDa = new DataAccessClass(strConString);
        objComp = new CompanyMaster(strConString);
        objbrand = new BrandMaster(strConString);
        objLocation = new LocationMaster(strConString);
        Objaddress = new Set_AddressChild(strConString);
        objLocDept = new Set_Location_Department(strConString);
        objRoleData = new RoleDataPermission(strConString);
        objUserPermission = new UserPermission(strConString);
        _strConString = strConString;
    }

    public bool FillData(object objControl, DataTable dtData, string strDisplayField, string strValueField)
    {
        bool result = false;
        if (objControl is GridView)
        {
            try
            {
                ((GridView)objControl).DataSource = dtData;
                ((GridView)objControl).DataBind();
                result = true;
            }
            catch (Exception ex)
            {

            }
        }

        if (objControl is DropDownList)
        {
            try
            {
                dtData = new DataView(dtData, "", strDisplayField, DataViewRowState.CurrentRows).ToTable();


                ((DropDownList)objControl).DataSource = dtData;
                ((DropDownList)objControl).DataTextField = strDisplayField;
                ((DropDownList)objControl).DataValueField = strValueField;
                ((DropDownList)objControl).DataBind();

                //if (IsaddZeroIndex)
                //{
                ((DropDownList)objControl).Items.Insert(0, "--Select--");
                ((DropDownList)objControl).SelectedIndex = 0;
                //}


                result = true;
            }
            catch
            {
            }


        }


        if (objControl is ASPxComboBox)
        {

            string sttrValue = string.Empty;

            try
            {
                sttrValue = ((ASPxComboBox)objControl).Value.ToString();
            }
            catch
            {

            }

            ListEditItem Li1 = new ListEditItem();
            Li1.Text = "---select---";


            try
            {
                dtData = new DataView(dtData, "", strDisplayField, DataViewRowState.CurrentRows).ToTable();

                ((ASPxComboBox)objControl).Items.Clear();
                ((ASPxComboBox)objControl).DataSource = dtData;

                ((ASPxComboBox)objControl).DataBind();

                //if (IsaddZeroIndex)
                //{
                ((ASPxComboBox)objControl).Items.Insert(0, Li1);
                ((ASPxComboBox)objControl).SelectedIndex = 0;
                //}


                if (((ASPxComboBox)objControl).Items.FindByValue(sttrValue) == null)
                {
                    ((ASPxComboBox)objControl).SelectedIndex = 0;
                }
                else
                {
                    ((ASPxComboBox)objControl).Value = sttrValue;

                }
                result = true;


                ((ASPxComboBox)objControl).ItemStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
            }
            catch
            {
            }


        }
        if (objControl is DataList)
        {
            try
            {
                ((DataList)objControl).DataSource = dtData;
                ((DataList)objControl).DataBind();
                result = true;
            }
            catch
            {
            }
        }
        if (objControl is CheckBoxList)
        {
            try
            {
                dtData = new DataView(dtData, "", strDisplayField, DataViewRowState.CurrentRows).ToTable();

                ((CheckBoxList)objControl).DataSource = dtData;
                ((CheckBoxList)objControl).DataTextField = strDisplayField;
                ((CheckBoxList)objControl).DataValueField = strValueField;
                ((CheckBoxList)objControl).DataBind();
                result = true;
            }
            catch
            {
            }
        }
        if (objControl is ListBox)
        {
            try
            {
                dtData = new DataView(dtData, "", strDisplayField, DataViewRowState.CurrentRows).ToTable();

                ((ListBox)objControl).DataSource = dtData;
                ((ListBox)objControl).DataTextField = strDisplayField;
                ((ListBox)objControl).DataValueField = strValueField;
                ((ListBox)objControl).DataBind();
                result = true;
            }
            catch
            {

            }
        }
        if (objControl is RadioButtonList)
        {
            try
            {
                dtData = new DataView(dtData, "", strDisplayField, DataViewRowState.CurrentRows).ToTable();

                ((RadioButtonList)objControl).DataSource = dtData;
                ((RadioButtonList)objControl).DataTextField = strDisplayField;
                ((RadioButtonList)objControl).DataValueField = strValueField;
                ((RadioButtonList)objControl).DataBind();
                result = true;
            }
            catch
            {
            }
        }

        dtData = null;
        return result;
    }

    public static DataTable GetEmployeeFilteredRecord(DropDownList ddlLocationFilter, DropDownList ddlDepartmentFilter, string strConString)
    {
        Common ObjComman = new Common(strConString);

        EmployeeMaster ObjEmp = new EmployeeMaster(strConString);
        string strDepId = string.Empty;
        string strLocationId = string.Empty;
        DataTable dtEmp = ObjEmp.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());
        if (ddlLocationFilter.SelectedIndex > 0)
        {
            dtEmp = new DataView(dtEmp, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "'  and Location_Id='" + ddlLocationFilter.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();

            strLocationId = ddlLocationFilter.SelectedValue;
        }
        else
        {
            dtEmp = new DataView(dtEmp, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "'  and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            strLocationId = HttpContext.Current.Session["LocId"].ToString();
        }

        if (HttpContext.Current.Session["EmpId"].ToString() != "0")
        {

            strDepId = ObjComman.GetRoleDataPermission(HttpContext.Current.Session["RoleId"].ToString(), "D", strLocationId, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

            if (strDepId == "")
            {
                strDepId = "0";
            }

            if(strDepId!= "0")
            {
                dtEmp = new DataView(dtEmp, "Department_Id in (" + strDepId.Substring(0, strDepId.ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
            
        }

        if (ddlDepartmentFilter.SelectedIndex > 0)
        {
            dtEmp = new DataView(dtEmp, "Department_Id = " + ddlDepartmentFilter.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
        }

        return dtEmp;
    }


    public static DataTable GetLocationDepartment(DropDownList ddlLocation, DropDownList ddlDepartment, string strConString)
    {
        Common ObjComman = new Common(strConString);
        DepartmentMaster objDep = new DepartmentMaster(strConString);

        ddlDepartment.Items.Clear();
        DataTable dtDepartment = new DataTable();
        string strDepId = string.Empty;
        string strLocId = HttpContext.Current.Session["LocId"].ToString();

        if (ddlLocation.SelectedIndex > 0)
        {
            strLocId = ddlLocation.SelectedValue;
        }
        dtDepartment = objDep.GetDepartmentMaster();

        strDepId = ObjComman.GetRoleDataPermission(HttpContext.Current.Session["RoleId"].ToString(), "D", strLocId, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["CompId"].ToString());

        if (strDepId == "")
        {
            strDepId = "0";
        }

        if (HttpContext.Current.Session["EmpId"].ToString() != "0")
        {
            if(strDepId != "0")
                {
                    dtDepartment = new DataView(dtDepartment, "Dep_Id in(" + strDepId.Substring(0, strDepId.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                }
            
        }

        new PageControlCommon(strConString).FillData((object)ddlDepartment, dtDepartment, "Dep_Name", "Dep_Id");


        return dtDepartment;
    }

     public void FillUser(string StrCompanyId, string UserId, DropDownList dl, string strModuleId, string ObjectId, string locationID)
    {
        bool isViewalluser = false;

        Common objCommon = new Common(_strConString);
        if (UserId.ToString().Trim().ToUpper() != "SUPERADMIN")
        {
            if (new DataView(objCommon.GetAllPagePermission(HttpContext.Current.Session["UserId"].ToString(), strModuleId, ObjectId, HttpContext.Current.Session["CompId"].ToString()), "Op_Id=14", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
            {
                isViewalluser = true;
            }
        }



        UserMaster objUserMaster = new UserMaster(_strConString);
        if (UserId.ToString().Trim().ToUpper() == "SUPERADMIN" || isViewalluser == true)
        {
            DataTable dt = new DataView(objUserMaster.GetUserMaster(HttpContext.Current.Session["CompId"].ToString()), "Company_Id='" + StrCompanyId + "'", "", DataViewRowState.CurrentRows).ToTable();

            dt = new DataView(dt, "Location_Id in (" + locationID + ")", "", DataViewRowState.CurrentRows).ToTable();

            //if (Convert.ToInt32(locationID) != 0)
            //{
            //    dt = new DataView(dt, "Location_Id=" + locationID + "", "", DataViewRowState.CurrentRows).ToTable();
            //}

            if (UserId.ToString().Trim().ToUpper() == "SUPERADMIN")
            {
                if (dt.Rows.Count != 0)
                {
                    dl.DataSource = dt;
                    dl.DataTextField = "Emp_Name";
                    dl.DataValueField = "User_Id";
                    dl.DataBind();
                    dl.Items.Insert(0, "--Select User--");
                    dl.SelectedIndex = 0;
                }
                else
                {
                    dl.Items.Clear();
                    dl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(objCommon.GetEmpName(HttpContext.Current.Session["EmpId"].ToString(),HttpContext.Current.Session["CompId"].ToString()).Split('/')[0].ToString(), UserId));
                    dl.SelectedIndex = 0;
                }

            }
            else
            {
                //used to find all employee's handled by employee id,code,name

                DataTable dt1 = new DataTable();
                dt1 = objUserMaster.GetEmpDtlsFromUserID(HttpContext.Current.Session["UserId"].ToString(),HttpContext.Current.Session["CompId"].ToString());
                DataTable dt_handledBy = new DataTable();
                if (dt1.Rows.Count > 0)
                {
                    dt_handledBy = objCommon.handledBy_Code(dt1.Rows[0]["Emp_id"].ToString(), HttpContext.Current.Session["CompId"].ToString());

                    if (dt_handledBy.Rows.Count != 0)
                    {
                        dl.DataSource = dt_handledBy;
                        dl.DataTextField = "Emp_Name";
                        dl.DataValueField = "Emp_code";
                        dl.DataBind();
                        dl.Items.Insert(0, "--Select User--");
                        dl.SelectedIndex = 0;
                    }
                    else
                    {
                        dl.Items.Clear();
                        dl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(objCommon.GetEmpName(HttpContext.Current.Session["EmpId"].ToString(),HttpContext.Current.Session["CompId"].ToString()).Split('/')[0].ToString(), UserId));
                        dl.SelectedIndex = 0;
                    }
                }
            }
        }
        else
        {

            dl.Items.Clear();

            dl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(objCommon.GetEmpName(HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["CompId"].ToString()).Split('/')[0].ToString(), UserId));
            dl.SelectedIndex = 0;

        }
    }

    public void FillUser(string StrCompanyId, string UserId, DropDownList dl, string strModuleId, string ObjectId, string locationID, string locationCondition)
    {
        bool isViewalluser = false;
        Common objCommon = new Common(_strConString);
        if (UserId.ToString().Trim().ToUpper() != "SUPERADMIN")
        {
            if (new DataView(objCommon.GetAllPagePermission(HttpContext.Current.Session["UserId"].ToString(), strModuleId, ObjectId, HttpContext.Current.Session["CompId"].ToString()), "Op_Id=14", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
            {
                isViewalluser = true;
            }
        }



        UserMaster objUserMaster = new UserMaster(_strConString);
        if (UserId.ToString().Trim().ToUpper() == "SUPERADMIN" || isViewalluser == true)
        {
            DataTable dt = new DataView(objUserMaster.GetUserMaster(HttpContext.Current.Session["CompId"].ToString()), "Company_Id='" + StrCompanyId + "'", "", DataViewRowState.CurrentRows).ToTable();

            dt = new DataView(dt, "Location_Id in (" + locationID + ")", "", DataViewRowState.CurrentRows).ToTable();

            //if (Convert.ToInt32(locationID) != 0)
            //{
            //    dt = new DataView(dt, "Location_Id in (" + locationID + ")", "", DataViewRowState.CurrentRows).ToTable();
            //}
            //else
            //{
            //    dt = new DataView(dt, locationCondition, "", DataViewRowState.CurrentRows).ToTable();
            //}

            if (UserId.ToString().Trim().ToUpper() == "SUPERADMIN")
            {
                if (dt.Rows.Count != 0)
                {
                    dl.DataSource = dt;
                    dl.DataTextField = "Emp_Name";
                    dl.DataValueField = "User_Id";
                    dl.DataBind();
                    dl.Items.Insert(0, "--Select User--");
                    dl.SelectedIndex = 0;
                }
                else
                {
                    dl.Items.Clear();
                    dl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(objCommon.GetEmpName(HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["CompId"].ToString()).Split('/')[0].ToString(), UserId));
                    dl.SelectedIndex = 0;
                }

            }
            else
            {
                //used to find all employee's handled by employee id,code,name

                DataTable dt1 = new DataTable();
                dt1 = objUserMaster.GetEmpDtlsFromUserID(HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                DataTable dt_handledBy = new DataTable();
                if (dt1.Rows.Count > 0)
                {
                    dt_handledBy = objCommon.handledBy_Code(dt1.Rows[0]["Emp_id"].ToString(), HttpContext.Current.Session["CompId"].ToString());

                    if (dt_handledBy.Rows.Count != 0)
                    {
                        dl.DataSource = dt_handledBy;
                        dl.DataTextField = "Emp_Name";
                        dl.DataValueField = "Emp_code";
                        dl.DataBind();
                        dl.Items.Insert(0, "--Select User--");
                        dl.SelectedIndex = 0;
                    }
                    else
                    {
                        dl.Items.Clear();
                        dl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(objCommon.GetEmpName(HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["CompId"].ToString()).Split('/')[0].ToString(), UserId));
                        dl.SelectedIndex = 0;
                    }
                }
            }
        }
        else
        {

            dl.Items.Clear();

            dl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(objCommon.GetEmpName(HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["CompId"].ToString()).Split('/')[0].ToString(), UserId));
            dl.SelectedIndex = 0;

        }
    }

    public bool ExportGridViewToExcel(GridView gv, string fileNameWithExtension)
    {
        try
        {

            if (gv.Rows.Count > 0)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                if (fileNameWithExtension == string.Empty)
                {
                    fileNameWithExtension = "pryce.xls";
                }
                HttpContext.Current.Response.AddHeader("content-disposition",
                "attachment;filename=" + fileNameWithExtension + "");
                HttpContext.Current.Response.Charset = "";
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                for (int i = 0; i < gv.Rows.Count; i++)
                {
                    GridViewRow row = gv.Rows[i];

                    //Change Color back to white
                    row.BackColor = System.Drawing.Color.White;

                    //Apply text style to each Row
                    row.Attributes.Add("class", "textmode");

                    //Apply style to Individual Cells of Alternating Row
                    if (i % 2 != 0)
                    {
                        // row.Cells[0].Style.Add("background-color", "#C2D69B");
                        // row.Cells[1].Style.Add("background-color", "#C2D69B");
                        // row.Cells[2].Style.Add("background-color", "#C2D69B");
                        //row.Cells[3].Style.Add("background-color", "#C2D69B");
                    }
                }
                gv.RenderControl(hw);

                //style to format numbers to string
                // string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                //HttpContext.Current.Response.Write(style);
                HttpContext.Current.Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
                return true;
            }
            return false;
        }
        catch (Exception er)
        {
            return false;
        }
    }

    public bool ExportGridViewToPDF(GridView gv, string fileName)
    {
        //btnExecute_Click(null, null);
        try
        {
            if (gv.Rows.Count > 0)
            {
                if (fileName == string.Empty)
                {
                    fileName = "pryce.pdf";
                }


                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".pdf");
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                gv.RenderControl(hw);
                StringReader sr = new StringReader(sw.ToString());
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
                pdfDoc.Open();
                htmlparser.Parse(sr);
                pdfDoc.Close();
                HttpContext.Current.Response.Write(pdfDoc);
                HttpContext.Current.Response.End();
                return true;
            }
            return false;
        }
        catch (Exception err)
        {
            return false;
        }
    }

    public static bool PopulatePager(Repeater rptPager, int recordCount, int currentPage)
    {
        List<System.Web.UI.WebControls.ListItem> pages = new List<System.Web.UI.WebControls.ListItem>();
        int startIndex, endIndex;
        int pagerSpan = 5;
        //Calculate the Start and End Index of pages to be displayed.
        
        double dblPageCount = (double)((decimal)recordCount / Convert.ToDecimal("10"));
        int pageCount = (int)Math.Ceiling(dblPageCount);
        startIndex = currentPage > 1 && (currentPage + pagerSpan - 1) < pagerSpan ? currentPage : 1;
        endIndex = pageCount > pagerSpan ? pagerSpan : pageCount;
        if (currentPage > pagerSpan % 2)
        {
            if (currentPage == 2)
            {
                endIndex = 5;
            }
            else
            {
                endIndex = currentPage + 2;
            }
        }
        else
        {
            endIndex = (pagerSpan - currentPage) + 1;
        }

        if (endIndex - (pagerSpan - 1) > startIndex)
        {
            startIndex = endIndex - (pagerSpan - 1);
        }

        if (endIndex > pageCount)
        {
            endIndex = pageCount;
            startIndex = ((endIndex - pagerSpan) + 1) > 0 ? (endIndex - pagerSpan) + 1 : 1;
        }

        //Add the First Page Button.
        if (currentPage > 1)
        {
            pages.Add(new System.Web.UI.WebControls.ListItem("First", "1"));
        }

        //Add the Previous Button.
        if (currentPage > 1)
        {
            pages.Add(new System.Web.UI.WebControls.ListItem("<<", (currentPage - 1).ToString()));
        }

        for (int i = startIndex; i <= endIndex; i++)
        {
            //pages.Add(new ListItem(i.ToString(), i.ToString(),true));
            pages.Add(new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString(), i != currentPage));
        }

        //Add the Next Button.
        if (currentPage < pageCount)
        {
            pages.Add(new System.Web.UI.WebControls.ListItem(">>", (currentPage + 1).ToString()));
        }

        //Add the Last Button.
        if (currentPage != pageCount)
        {
            pages.Add(new System.Web.UI.WebControls.ListItem("Last", pageCount.ToString()));
        }
        rptPager.DataSource = pages;
        rptPager.DataBind();
        return true;
    }

    public static DataTable GetEmployeeListbyLocationandDepartment(DropDownList ddlLocation, DropDownList ddlDepartment, bool IsPayroll, string strConString)
    {
        //if you want  to employee list in attendance section then is payroll should be false 
        // for payroll section is should be true
        string strLocationId = string.Empty;
        string strDepId = string.Empty;
        EmployeeMaster objEmp = new EmployeeMaster(strConString);
        DataTable dtEmp = new DataTable();
        Common objcmn = new Common(strConString);
        if (IsPayroll)
        {
            dtEmp = objEmp.GetEmployee_InPayroll(HttpContext.Current.Session["CompId"].ToString());
        }
        else
        {
            dtEmp = objEmp.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());
        }
        if (ddlLocation.SelectedIndex == 0)
        {
            strLocationId = HttpContext.Current.Session["LocId"].ToString();
            dtEmp = new DataView(dtEmp, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "'  and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else
        {
            strLocationId = ddlLocation.SelectedValue;
            dtEmp = new DataView(dtEmp, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "'  and Location_Id=" + ddlLocation.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
        }

        strDepId = objcmn.GetRoleDataPermission(HttpContext.Current.Session["RoleId"].ToString(), "D", strLocationId, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (strDepId == "")
        {
            strDepId = "0";
        }

        dtEmp = new DataView(dtEmp, "Department_Id in(" + strDepId + ")", "", DataViewRowState.CurrentRows).ToTable();

        if (ddlDepartment.SelectedIndex != 0)
        {
            dtEmp = new DataView(dtEmp, "Department_Id = " + ddlDepartment.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
        }

        return dtEmp;

    }

    public static DataTable GetEmployeeDepartmentByLocationId(DropDownList ddlLocation, DropDownList dpDepartment, string strConString)
    {
        DepartmentMaster objDep = new DepartmentMaster(strConString);
        Common ObjComman = new Common(strConString);
        dpDepartment.Items.Clear();
        DataTable dtDepartment = new DataTable();
        string strDepId = string.Empty;
        string strLocId = HttpContext.Current.Session["LocId"].ToString();
        string strDeptvalue = string.Empty;
        if (ddlLocation.SelectedIndex > 0)
        {
            strLocId = ddlLocation.SelectedValue;

            strDepId = ObjComman.GetRoleDataPermission(HttpContext.Current.Session["RoleId"].ToString(), "D", strLocId, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
        }
        else
        {
            strDepId = ObjComman.GetRoleDataPermission(HttpContext.Current.Session["RoleId"].ToString(), "D", HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
        }
        dtDepartment = objDep.GetDepartmentMaster();

        if (strDepId == "")
        {
            strDepId = "0";
        }
        dtDepartment = new DataView(dtDepartment, "Dep_Id in  (" + strDepId + ")", "", DataViewRowState.CurrentRows).ToTable(true, "Dep_Name", "Dep_Id");
        return dtDepartment;
    }


    public static DataTable GetEmployeeDepartmentByLocationValue(string strLocationId, DropDownList dpDepartment, string strConString)
    {
        DepartmentMaster objDep = new DepartmentMaster(strConString);
        Common ObjComman = new Common(strConString);
        dpDepartment.Items.Clear();
        DataTable dtDepartment = new DataTable();
        string strDepId = string.Empty;
        string strLocId = HttpContext.Current.Session["LocId"].ToString();
        string strDeptvalue = string.Empty;

        strDepId = ObjComman.GetRoleDataPermission(HttpContext.Current.Session["RoleId"].ToString(), "D", strLocationId, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        dtDepartment = objDep.GetDepartmentMaster();

        if (strDepId == "")
        {
            strDepId = "0";
        }
        dtDepartment = new DataView(dtDepartment, "Dep_Id in  (" + strDepId + ")", "", DataViewRowState.CurrentRows).ToTable(true, "Dep_Name", "Dep_Id");
        return dtDepartment;
    }

    public void fillLocationWithAllOption(DropDownList ddlLocation)
    {
        ddlLocation.Items.Clear();
        Common objCommon = new Common(_strConString);
        DataTable dtLoc = objLocation.GetLocationMaster(HttpContext.Current.Session["CompId"].ToString());

        try
        {
            dtLoc = new DataView(dtLoc, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        string LocIds = "";

        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            LocIds = objCommon.GetRoleDataPermission(HttpContext.Current.Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                if (dtLoc.Rows.Count > 1)
                {
                    ddlLocation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", LocIds.Substring(0, LocIds.Length - 1)));
                }
            }
            else
            {
                ddlLocation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", HttpContext.Current.Session["LocId"].ToString()));
            }
        }
        if (dtLoc.Rows.Count > 0)
        {
            ddlLocation.DataSource = dtLoc;
            ddlLocation.DataTextField = "Location_name";
            ddlLocation.DataValueField = "Location_Id";
            ddlLocation.DataBind();
            ddlLocation.SelectedValue = HttpContext.Current.Session["LocId"].ToString();
            if (!(dtLoc.Rows.Count > 1 && LocIds != ""))
            {
                foreach (DataRow dr in dtLoc.Rows)
                {

                    LocIds += dr["location_Id"].ToString() + ",";

                }
            }
            ddlLocation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", LocIds.Substring(0, LocIds.Length - 1)));
        }
        else
        {
            ddlLocation.Items.Clear();
        }
        dtLoc = null;
    }
    public void fillLocation(DropDownList ddlLocation)
    {
        Common objCommon = new Common(_strConString);
        ddlLocation.Items.Clear();
        DataTable dtLoc = objLocation.GetLocationMaster(HttpContext.Current.Session["CompId"].ToString());

        try
        {
            dtLoc = new DataView(dtLoc, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        string LocIds = "";
        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            LocIds = objCommon.GetRoleDataPermission(HttpContext.Current.Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        if (dtLoc.Rows.Count > 0)
        {
            ddlLocation.DataSource = dtLoc;
            ddlLocation.DataTextField = "Location_name";
            ddlLocation.DataValueField = "Location_Id";
            ddlLocation.DataBind();
            ddlLocation.SelectedValue = HttpContext.Current.Session["LocId"].ToString();
        }
        else
        {
            ddlLocation.Items.Clear();
        }
        dtLoc = null;
    }
    
    public void FillUser(string StrCompanyId, string UserId, DropDownList dl, string strModuleId, string ObjectId)
    {
        Common objCommon = new Common(_strConString);
        bool isViewalluser = false;

        if (UserId.ToString().Trim().ToUpper() != "SUPERADMIN")
        {
            if (new DataView(objCommon.GetAllPagePermission(HttpContext.Current.Session["UserId"].ToString(), strModuleId, ObjectId, HttpContext.Current.Session["CompId"].ToString()), "Op_Id=14", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
            {
                isViewalluser = true;
            }
        }



        UserMaster objUserMaster = new UserMaster(_strConString);
        if (UserId.ToString().Trim().ToUpper() == "SUPERADMIN" || isViewalluser == true)
        {
            DataTable dt = new DataView(objUserMaster.GetUserMaster(HttpContext.Current.Session["CompId"].ToString()), "Company_Id='" + StrCompanyId + "'", "", DataViewRowState.CurrentRows).ToTable();

            dt = new DataView(dt, "Location_Id=" + HttpContext.Current.Session["LocId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();


            if (UserId.ToString().Trim().ToUpper() == "SUPERADMIN")
            {
                if (dt.Rows.Count != 0)
                {
                    dl.DataSource = dt;
                    dl.DataTextField = "Emp_Name";
                    dl.DataValueField = "User_Id";
                    dl.DataBind();
                    dl.Items.Insert(0, "--Select User--");
                    dl.SelectedIndex = 0;
                }
                else
                {
                    dl.Items.Clear();
                    dl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(objCommon.GetEmpName(HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["CompId"].ToString()).Split('/')[0].ToString(), UserId));
                    dl.SelectedIndex = 0;
                }

            }
            else
            {
                //used to find all employee's handled by employee id,code,name

                DataTable dt1 = new DataTable();
                dt1 = objUserMaster.GetEmpDtlsFromUserID(HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                DataTable dt_handledBy = new DataTable();
                if (dt1.Rows.Count > 0)
                {
                    dt_handledBy = objCommon.handledBy_Code(dt1.Rows[0]["Emp_id"].ToString(), HttpContext.Current.Session["CompId"].ToString());

                    if (dt_handledBy.Rows.Count != 0)
                    {
                        dl.DataSource = dt_handledBy;
                        dl.DataTextField = "Emp_Name";
                        dl.DataValueField = "Emp_code";
                        dl.DataBind();
                        dl.Items.Insert(0, "--Select User--");
                        dl.SelectedIndex = 0;
                    }
                    else
                    {
                        dl.Items.Clear();
                        dl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(objCommon.GetEmpName(HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["CompId"].ToString()).Split('/')[0].ToString(), UserId));
                        dl.SelectedIndex = 0;
                    }
                }
            }
        }
        else
        {

            dl.Items.Clear();

            dl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(objCommon.GetEmpName(HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["CompId"].ToString()).Split('/')[0].ToString(), UserId));
            dl.SelectedIndex = 0;

        }
    }

    //this function is created by jitendra upadhyay on 05-05-2015

    //this function for bind gridview and dropdown for common function



    // Add by ghanshyam suthar on 09-10-2017
    public bool Fill_Country_Code(object objControl, DataTable dtData, string strDisplayField, string strValueField)
    {
        bool result = false;
        if (objControl is GridView)
        {
            try
            {
                ((GridView)objControl).DataSource = dtData;
                ((GridView)objControl).DataBind();
                result = true;
            }
            catch (Exception ex)
            {

            }
        }

        if (objControl is DropDownList)
        {
            try
            {
                dtData = new DataView(dtData, "", strDisplayField, DataViewRowState.CurrentRows).ToTable();


                ((DropDownList)objControl).DataSource = dtData;
                ((DropDownList)objControl).DataTextField = strDisplayField;
                ((DropDownList)objControl).DataValueField = strValueField;
                ((DropDownList)objControl).DataBind();

                //if (IsaddZeroIndex)
                //{
                ((DropDownList)objControl).Items.Insert(0, "--Country--");
                ((DropDownList)objControl).SelectedIndex = 0;
                //}


                result = true;
            }
            catch
            {
            }


        }


        if (objControl is ASPxComboBox)
        {

            string sttrValue = string.Empty;

            try
            {
                sttrValue = ((ASPxComboBox)objControl).Value.ToString();
            }
            catch
            {

            }

            ListEditItem Li1 = new ListEditItem();
            Li1.Text = "---Country---";


            try
            {
                dtData = new DataView(dtData, "", strDisplayField, DataViewRowState.CurrentRows).ToTable();

                ((ASPxComboBox)objControl).Items.Clear();
                ((ASPxComboBox)objControl).DataSource = dtData;

                ((ASPxComboBox)objControl).DataBind();

                //if (IsaddZeroIndex)
                //{
                ((ASPxComboBox)objControl).Items.Insert(0, Li1);
                ((ASPxComboBox)objControl).SelectedIndex = 0;
                //}


                if (((ASPxComboBox)objControl).Items.FindByValue(sttrValue) == null)
                {
                    ((ASPxComboBox)objControl).SelectedIndex = 0;
                }
                else
                {
                    ((ASPxComboBox)objControl).Value = sttrValue;

                }
                result = true;


                ((ASPxComboBox)objControl).ItemStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
            }
            catch
            {
            }


        }
        if (objControl is DataList)
        {
            try
            {
                ((DataList)objControl).DataSource = dtData;
                ((DataList)objControl).DataBind();
                result = true;
            }
            catch
            {
            }
        }
        if (objControl is CheckBoxList)
        {
            try
            {
                dtData = new DataView(dtData, "", strDisplayField, DataViewRowState.CurrentRows).ToTable();

                ((CheckBoxList)objControl).DataSource = dtData;
                ((CheckBoxList)objControl).DataTextField = strDisplayField;
                ((CheckBoxList)objControl).DataValueField = strValueField;
                ((CheckBoxList)objControl).DataBind();
                result = true;
            }
            catch
            {
            }
        }
        if (objControl is ListBox)
        {
            try
            {
                dtData = new DataView(dtData, "", strDisplayField, DataViewRowState.CurrentRows).ToTable();

                ((ListBox)objControl).DataSource = dtData;
                ((ListBox)objControl).DataTextField = strDisplayField;
                ((ListBox)objControl).DataValueField = strValueField;
                ((ListBox)objControl).DataBind();
                result = true;
            }
            catch
            {

            }
        }
        if (objControl is RadioButtonList)
        {
            try
            {
                dtData = new DataView(dtData, "", strDisplayField, DataViewRowState.CurrentRows).ToTable();

                ((RadioButtonList)objControl).DataSource = dtData;
                ((RadioButtonList)objControl).DataTextField = strDisplayField;
                ((RadioButtonList)objControl).DataValueField = strValueField;
                ((RadioButtonList)objControl).DataBind();
                result = true;
            }
            catch
            {
            }
        }

        return result;
    }
    //---------------------------------------

    public DataTable GetArabicMessage()
    {
        System.Collections.SortedList slist = new SortedList();
        DataTable dtForArebicMess = GetTable();
        string filename = HttpContext.Current.Request.PhysicalApplicationPath + "Arabic\\" + "ArabicMessage.resx";
        Stream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);

        ResXResourceReader RrX = new ResXResourceReader(stream);
        IDictionaryEnumerator RrEn = RrX.GetEnumerator();

        while (RrEn.MoveNext())
        {
            slist.Add(RrEn.Key, RrEn.Value);
            DataRow row = dtForArebicMess.NewRow();
            row[0] = RrEn.Key;
            row[1] = RrEn.Value;
            dtForArebicMess.Rows.Add(row);
        }
        RrX.Close();
        stream.Dispose();
        return dtForArebicMess;
    }

    public DataTable GetTable()
    {
        DataTable dtTemp = new DataTable();
        dtTemp.Columns.Add(new DataColumn("Key"));
        dtTemp.Columns.Add(new DataColumn("Value"));
        return dtTemp;
    }

    public static int GetPageSize()
    {
        int size = 10;
        try
        {
            size = Convert.ToInt32(HttpContext.Current.Session["GridSize"]);
        }
        catch (Exception)
        {

        }
        return size;
    }

    public static string ChangeTDForDefaultLeft()
    {
        string retval = string.Empty;
        try
        {
            string lang = HttpContext.Current.Session["lang"].ToString();
        }
        catch (Exception)
        {
            return "left";
        }
        if (HttpContext.Current.Session["lang"] != null && HttpContext.Current.Session["lang"].ToString() == "2")
        {
            retval = "right";
        }
        else
        {
            retval = "left";
        }
        return retval;
    }
    public static string ChangeTDForDefaultRight()
    {
        string retval = string.Empty;
        if (HttpContext.Current.Session["lang"] != null && HttpContext.Current.Session["lang"] == "2")
        {
            retval = "left";
        }
        else
        {
            retval = "right";
        }
        return retval;
    }

}