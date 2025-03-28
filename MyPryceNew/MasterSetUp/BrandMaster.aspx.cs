using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;
using PegasusDataAccess;
public partial class MasterSetUp_BrandMaster : BasePage
{
    BrandMaster objBrand = null;
    Common cmn = null;
    SystemParameter objSys = null;
    Set_AddressMaster AM = null;
    Set_AddressCategory ObjAddressCat = null;
    Set_AddressChild objAddChild = null;
    EmployeeMaster objEmp = null;
    IT_ObjectEntry objObjectEntry = null;
    CountryMaster ObjCountry = null;
    LocationMaster objLocation = null;
    Country_Currency objCountryCurrency = null;
    ContactNoMaster objContactnoMaster = null;
    DataAccessClass objda = null;
    UserDataPermission objUserDataPerm = null;
    Set_ApplicationParameter objAppParam = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        hdntxtaddressid.Value = txtAddressName.ID;
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objBrand = new BrandMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        AM = new Set_AddressMaster(Session["DBConnection"].ToString());
        ObjAddressCat = new Set_AddressCategory(Session["DBConnection"].ToString());
        objAddChild = new Set_AddressChild(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        ObjCountry = new CountryMaster(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objCountryCurrency = new Country_Currency(Session["DBConnection"].ToString());
        objContactnoMaster = new ContactNoMaster(Session["DBConnection"].ToString());
        objda = new DataAccessClass(Session["DBConnection"].ToString());
        objUserDataPerm = new UserDataPermission(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
    
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../MasterSetUp/BrandMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            Session["empimgpath"] = null;
            Session["CHECKED_ITEMS"] = null;

            Session["AddCtrl_Country_Id"] = "";
            Session["AddCtrl_State_Id"] = "";

            txtValue.Focus();
            FillGridBin();
            FillGrid();
            RootFolder();
            string ParmValue = objAppParam.GetApplicationParameterValueByParamName("ImageFileUploadSize", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            try
            {
                ASPxFileManager1.SettingsUpload.ValidationSettings.MaxFileSize = int.Parse(ParmValue) * 1000;
            }
            catch(Exception ex)
            {

            }
        }
        //AllPageCode();
    }

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSave.Visible = clsPagePermission.bAdd;
        imgAddAddressName.Visible = clsPagePermission.bAdd;
        btnAddNewAddress.Visible = clsPagePermission.bAdd;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        imgBtnRestore.Visible = clsPagePermission.bRestore;       
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        DataTable dt = Common.GetEmployee(prefixText, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["DBConnection"].ToString());

        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = "" + dt.Rows[i][1].ToString() + "/(" + dt.Rows[i][2].ToString() + ")/" + dt.Rows[i][0].ToString() + "";
        }
        return str;
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        txtbinValue.Focus();
        FillGridBin();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        Session["CHECKED_ITEMS"] = null;
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtValue.Text = "";
    }

    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlOption.SelectedIndex != 0)
        {
            string condition = string.Empty;


            if (ddlOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + txtValue.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '%" + txtValue.Text.Trim() + "%'";
            }
            DataTable dtCust = (DataTable)Session["Brand"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_Brand__Master"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvBrandMaster, view.ToTable(), "", "");
            //AllPageCode();
        }
        txtValue.Focus();
    }
    protected void btnbinbind_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlbinOption.SelectedIndex != 0)
        {
            string condition = string.Empty;


            if (ddlbinOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String)='" + txtbinValue.Text.Trim() + "'";
            }
            else if (ddlbinOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) like '%" + txtbinValue.Text + "%'";
            }
            else
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) Like '" + txtbinValue.Text + "%'";
            }
            DataTable dtCust = (DataTable)Session["dtbinBrand"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtbinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvBrandMasterBin, view.ToTable(), "", "");

            if (view.ToTable().Rows.Count == 0)
            {
                // imgBtnRestore.Visible = false;
                // ImgbtnSelectAll.Visible = false;
            }
            else
            {
                //AllPageCode();
            }
        }
        txtbinValue.Focus();
    }

    protected void btnbinRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillGridBin();
        Session["CHECKED_ITEMS"] = null;
        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 0;
        txtbinValue.Text = "";
        lblSelectedRecord.Text = "";
    }
    private void SaveCheckedValuesemplog()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in gvBrandMasterBin.Rows)
        {
            index = (int)gvBrandMasterBin.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked;


            // Check in the Session
            if (Session["CHECKED_ITEMS"] != null)
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
            if (result)
            {
                if (!userdetails.Contains(index))
                    userdetails.Add(index);
            }
            else
                userdetails.Remove(index);

        }
        if (userdetails != null && userdetails.Count > 0)
            Session["CHECKED_ITEMS"] = userdetails;
    }
    private void PopulateCheckedValuesemplog()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in gvBrandMasterBin.Rows)
            {
                int index = (int)gvBrandMasterBin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }

    protected void gvBrandMasterBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValuesemplog();
        gvBrandMasterBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtbinFilter"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvBrandMasterBin, dt, "", "");
        //AllPageCode();
        PopulateCheckedValuesemplog();
    }
    protected void gvBrandMasterBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objBrand.GetBrandMasterInactive(Session["CompId"].ToString());
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtbinFilter"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvBrandMasterBin, dt, "", "");
        //AllPageCode();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        int b = 0;

        if (txtBrandCode.Text == "")
        {
            DisplayMessage("Enter Brand Code");
            txtBrandCode.Focus();
            return;
        }

        if (txtBrandName.Text == "")
        {
            DisplayMessage("Enter Brand Name");
            txtBrandName.Focus();
            return;
        }


        if (Session["empimgpath"] == null)
        {
            Session["empimgpath"] = "";

        }

        string empid = string.Empty;
        if (txtManagerName.Text != "")
        {
            empid = txtManagerName.Text.Split('/')[txtManagerName.Text.Split('/').Length - 1];

            DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());

            dtEmp = new DataView(dtEmp, "Emp_Code='" + empid + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtEmp.Rows.Count > 0)
            {
                empid = dtEmp.Rows[0]["Emp_Id"].ToString();
            }
            else
            {
                DisplayMessage("Employee Not Exists");
                txtManagerName.Text = "";
                txtManagerName.Focus();
                return;
            }

        }
        else
        {
            empid = "0";
        }
        if (editid.Value == "")
        {
            DataTable dt = objBrand.GetBrandMaster(Session["CompId"].ToString());

            dt = new DataView(dt, "Brand_Code='" + txtBrandCode.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count > 0)
            {
                DisplayMessage("Brand Code Already Exists");
                txtBrandCode.Focus();
                return;

            }
            DataTable dt1 = objBrand.GetBrandMaster(Session["CompId"].ToString());

            dt1 = new DataView(dt1, "Brand_Name='" + txtBrandName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                DisplayMessage("Brand Name Already Exists");
                txtBrandName.Focus();
                return;
            }
            b = objBrand.InsertBrandMaster(Session["CompId"].ToString(), txtBrandName.Text, txtBrandNameL.Text, txtBrandCode.Text, empid, Session["empimgpath"].ToString(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            if (b != 0)
            {
                string strMaxId = string.Empty;
                strMaxId = b.ToString();

                //here we are giving default permission to admin user 

                if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "cloud" && Session["UserId"].ToString().Trim().ToLower() == "admin")
                {
                    objUserDataPerm.InsertUserDataPermission(Session["UserId"].ToString().Trim(), Session["CompId"].ToString(), "B", strMaxId, "0", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }

                bool Isdefault = false;
                foreach (GridViewRow gvr in GvAddressName.Rows)
                {
                    if (((CheckBox)gvr.FindControl("chkdefault")).Checked == true)
                    {
                        Isdefault = true;
                        break;
                    }

                }

                foreach (GridViewRow gvr in GvAddressName.Rows)
                {
                    Label lblGvAddressName = (Label)gvr.FindControl("lblgvAddressName");
                    CheckBox chk = (CheckBox)gvr.FindControl("chkdefault");
                    if (GvAddressName.Rows.Count == 1)
                    {
                        chk.Checked = true;
                    }
                    else
                    {
                        if (Isdefault == false)
                        {
                            if (gvr.RowIndex == 0)
                            {
                                chk.Checked = true;
                            }

                        }
                    }

                    if (lblGvAddressName.Text != "")
                    {
                        DataTable dtAddId = AM.GetAddressDataByAddressName(lblGvAddressName.Text);
                        if (dtAddId.Rows.Count > 0)
                        {
                            string strAddressId = dtAddId.Rows[0]["Trans_Id"].ToString();
                            objAddChild.InsertAddressChild(strAddressId, "Brand", strMaxId, chk.Checked.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        }
                    }
                }


                DisplayMessage("Record Saved","green");
                FillGrid();
                Reset();
            }
            else
            {
                DisplayMessage("Record Not Saved");
            }
        }
        else
        {
            DataTable dt = objBrand.GetBrandMaster(Session["CompId"].ToString());


            string BrandCode = string.Empty;
            string BrandName = string.Empty;

            try
            {
                BrandCode = (new DataView(dt, "Brand_Id='" + editid.Value + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Brand_Code"].ToString();
            }
            catch
            {
                BrandCode = "";
            }

            dt = new DataView(dt, "Brand_Code='" + txtBrandCode.Text + "' and Brand_Code<>'" + BrandCode + "' ", "", DataViewRowState.CurrentRows).ToTable();

            if (dt.Rows.Count > 0)
            {
                DisplayMessage("Brand Code Already Exists");
                txtBrandCode.Focus();
                return;

            }



            DataTable dt1 = objBrand.GetBrandMaster(Session["CompId"].ToString());
            try
            {
                BrandName = (new DataView(dt1, "Brand_Id='" + editid.Value + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Brand_Name"].ToString();
            }
            catch
            {
                BrandName = "";
            }
            dt1 = new DataView(dt1, "Brand_Name='" + txtBrandName.Text + "' and Brand_Name<>'" + BrandName + "' ", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                DisplayMessage("Brand Name Already Exists");
                txtBrandName.Focus();
                return;

            }


            b = objBrand.UpdateBrandMaster(Session["CompId"].ToString(), editid.Value, txtBrandName.Text, txtBrandNameL.Text, txtBrandCode.Text, empid, Session["empimgpath"].ToString(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            if (b != 0)
            {


                objAddChild.DeleteAddressChild("Brand", editid.Value);
                bool Isdefault = false;
                foreach (GridViewRow gvr in GvAddressName.Rows)
                {
                    if (((CheckBox)gvr.FindControl("chkdefault")).Checked == true)
                    {
                        Isdefault = true;
                        break;
                    }

                }



                foreach (GridViewRow gvr in GvAddressName.Rows)
                {
                    Label lblGvAddressName = (Label)gvr.FindControl("lblgvAddressName");
                    CheckBox chk = (CheckBox)gvr.FindControl("chkdefault");
                    if (GvAddressName.Rows.Count == 1)
                    {
                        chk.Checked = true;
                    }
                    else
                    {
                        if (Isdefault == false)
                        {
                            if (gvr.RowIndex == 0)
                            {
                                chk.Checked = true;
                            }

                        }
                    }

                    if (lblGvAddressName.Text != "")
                    {
                        DataTable dtAddId = AM.GetAddressDataByAddressName(lblGvAddressName.Text);
                        if (dtAddId.Rows.Count > 0)
                        {
                            string strAddressId = dtAddId.Rows[0]["Trans_Id"].ToString();
                            objAddChild.InsertAddressChild(strAddressId, "Brand", editid.Value, chk.Checked.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        }
                    }
                }

                DisplayMessage("Record Updated", "green");
                Reset();
                FillGrid();
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);

            }
            else
            {
                DisplayMessage("Record Not Updated");
            }

        }
    }

    #region Filetobytearray
    public byte[] FileToByteArray(string fileName)
    {
        byte[] buff = null;
        FileStream fs = new FileStream(fileName,
                                       FileMode.Open,
                                       FileAccess.Read);
        BinaryReader br = new BinaryReader(fs);
        long numBytes = new FileInfo(fileName).Length;
        buff = br.ReadBytes((int)numBytes);
        return buff;
    }
    #endregion
    #region Upload
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string ASPxFileManager1_SelectedFileOpened(string fullname, string name)    
    {
        try
        {

            DataAccessClass objda = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
            string Date = DateTime.Now.ToString("yyyyMMddHHss");
            string fileExtension = Path.GetExtension(name);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(name) + Date;
            name = fileNameWithoutExtension + fileExtension;
            CompanyMaster objComp = new CompanyMaster(HttpContext.Current.Session["DBConnection"].ToString());
            string compid = (int.Parse(objComp.GetMaxCompanyId())).ToString();          
            try
            {
                try
                {
                    string RegistrationCode = Common.Decrypt(objda.get_SingleValue("Select registration_code from Application_Lic_Main"));

                    fullname = fullname.Replace("Product_", "Product//Product_");

                    FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(fullname), FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    long numBytes = new FileInfo(HttpContext.Current.Server.MapPath(fullname)).Length;
                    Byte[] buffer = br.ReadBytes((int)numBytes);
                    string fullPath = "~/CompanyResource/" + RegistrationCode + "/" + compid + "";
                    string MapfullPath = HttpContext.Current.Server.MapPath(fullPath);
                    if (Directory.Exists(MapfullPath))
                    {
                        if (RegistrationCode != "" && RegistrationCode != null)
                        {
                            File.WriteAllBytes(HttpContext.Current.Server.MapPath(fullPath + "/" + name), buffer);

                        }
                    }
                    else
                    {
                        try
                        {
                            Directory.CreateDirectory(MapfullPath);
                            File.WriteAllBytes(HttpContext.Current.Server.MapPath(fullPath + "/" + name), buffer);
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                }
                catch(Exception ex)
                {
                    FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(fullname), FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    long numBytes = new FileInfo(HttpContext.Current.Server.MapPath(fullname)).Length;
                    Byte[] buffer = br.ReadBytes((int)numBytes);
                    string fullPath = "~/CompanyResource/" + HttpContext.Current.Session["CompId"].ToString() +"";
                    string MapfullPath = HttpContext.Current.Server.MapPath(fullPath);
                    if (Directory.Exists(MapfullPath))
                    {                        
                       File.WriteAllBytes(HttpContext.Current.Server.MapPath(fullPath + "/" + name), buffer);                        
                    }
                    else
                    {
                        try
                        {
                            Directory.CreateDirectory(MapfullPath);
                            File.WriteAllBytes(HttpContext.Current.Server.MapPath(fullPath + "/" + name), buffer);
                        }
                        catch (Exception exp)
                        {

                        }
                    }
                }
            }
            catch
            {
            }
            HttpContext.Current.Session["empimgpath"] = name;
            return "true";
        }
        catch (Exception err)
        {
            return "false";
        }
    }
    #region Filemanager
    public void RootFolder()
    {
        string User = HttpContext.Current.Session["UserId"].ToString();

        if (User == "superadmin")
        {
            ASPxFileManager1.Settings.RootFolder = "~\\Product";
            ASPxFileManager1.Settings.InitialFolder = "~\\Product";
            ASPxFileManager1.Settings.ThumbnailFolder = "~\\Product\\Thumbnail\\";
        }
        else
        {
            string folderPath = "";
            try
            {
                string RegistrationCode = Common.Decrypt(objda.get_SingleValue("Select registration_code from Application_Lic_Main"));
                folderPath = "~\\Product\\Product_" + RegistrationCode + "";
                string fullPath = Server.MapPath(folderPath);
                if (Directory.Exists(fullPath))
                {
                    //Console.WriteLine("The folder already exists.");
                }
                else
                {
                    try
                    {
                        Directory.CreateDirectory(fullPath);
                        //Console.WriteLine("The folder has been created.");
                    }
                    catch (Exception ex)
                    {

                    }
                }
                if (Directory.Exists(folderPath + "\\Thumbnail"))
                {
                    //Console.WriteLine("The folder already exists.");
                }
                else
                {
                    try
                    {
                        Directory.CreateDirectory(folderPath + "\\Thumbnail");
                        //Console.WriteLine("The folder has been created.");
                    }
                    catch (Exception ex)
                    {

                    }
                }
                ASPxFileManager1.Settings.RootFolder = folderPath;
                ASPxFileManager1.Settings.InitialFolder = folderPath;
                ASPxFileManager1.Settings.ThumbnailFolder = folderPath + "\\Thumbnail";
            }
            catch(Exception ex)
            {
                ASPxFileManager1.Settings.RootFolder = "~\\Product";
                ASPxFileManager1.Settings.InitialFolder = "~\\Product";
                ASPxFileManager1.Settings.ThumbnailFolder = "~\\Product\\Thumbnail\\";
            }
            
        }

    }
    #endregion
    #endregion

    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();


        DataTable dt = objBrand.GetBrandMasterById(Session["CompId"].ToString(), editid.Value);
        if (dt.Rows.Count > 0)
        {
            txtBrandCode.Text = dt.Rows[0]["Brand_Code"].ToString();
            txtBrandName.Text = dt.Rows[0]["Brand_Name"].ToString();
            txtBrandNameL.Text = dt.Rows[0]["Brand_Name_L"].ToString();


            if (dt.Rows[0]["Emp_Id"].ToString().Trim() != "0" || dt.Rows[0]["Emp_Id"].ToString().Trim() != "")
            {
                txtManagerName.Text = cmn.GetEmpName(dt.Rows[0]["Emp_Id"].ToString(), HttpContext.Current.Session["CompId"].ToString());
            }
            else
            {
                txtManagerName.Text = "";

            }

            try
            {
                string RegistrationCode = Common.Decrypt(objda.get_SingleValue("Select registration_code from Application_Lic_Main"));


                imgLogo.ImageUrl = "~/CompanyResource/" + RegistrationCode + "/" + "/" + Session["CompId"].ToString() + "/" + dt.Rows[0]["Logo_Path"].ToString();

                Session["empimgpath"] = dt.Rows[0]["Logo_Path"].ToString();
            }
            catch(Exception ex)
            {
                imgLogo.ImageUrl = "~/CompanyResource"+ "/" + Session["CompId"].ToString() + "/" + dt.Rows[0]["Logo_Path"].ToString();

                Session["empimgpath"] = dt.Rows[0]["Logo_Path"].ToString();
            }

            DataTable dtChild = objAddChild.GetAddressChildDataByAddTypeAndAddRefId("Brand", editid.Value);
            if (dtChild.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 12-05-2015
                objPageCmn.FillData((object)GvAddressName, dtChild, "", "");

                int Sr_No = 1;
                foreach (GridViewRow gvr in GvAddressName.Rows)
                {
                    Label lblAddressName = (Label)gvr.FindControl("lblgvAddressName");
                    Label lblAddress = (Label)gvr.FindControl("lblgvAddress");
                    lblAddress.Text = GetAddressByAddressName(lblAddressName.Text);
                    Label lblSNo = (Label)gvr.FindControl("lblSNo");
                    lblSNo.Text = Sr_No.ToString();
                    Sr_No++;
                }
            }

            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_New_Active()", true);
        }
        //AllPageCode();
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        int b = 0;
        string BrandId = Session["BrandId"].ToString();
        if (BrandId == "0")
        {
            b = objBrand.DeleteBrandMaster(Session["CompId"].ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        }
        else
        {
            DataTable dtEmp = objEmp.GetEmployeeMasterBy_Id(e.CommandArgument.ToString(), "11");
            if (dtEmp.Rows.Count > 0)
            {
                b = -11;
            }
            else
            {
                b = objBrand.DeleteBrandMaster(Session["CompId"].ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
        }
        if (b != 0)
        {
            if (b == -11)
            {
                DisplayMessage("Brand is Used for Transaction so you can not Delete this Brand");
            }
            else
            {
                DisplayMessage("Record Deleted");
                FillGridBin();
                FillGrid();
                Reset();
            }
        }
        else
        {
            DisplayMessage("Record Not Deleted");
            FillGrid();
        }
    }
    protected void gvBrandMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvBrandMaster.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilter_Brand__Master"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvBrandMaster, dt, "", "");
        //AllPageCode();
    }
    protected void gvBrandMaster_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Brand__Master"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Brand__Master"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvBrandMaster, dt, "", "");
        //AllPageCode();
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListBrandCode(string prefixText, int count, string contextKey)
    {
        BrandMaster objBrandMaster = new BrandMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objBrandMaster.GetBrandMaster(HttpContext.Current.Session["CompId"].ToString()), "Brand_Code like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Brand_Code"].ToString();
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListBrandName(string prefixText, int count, string contextKey)
    {
        BrandMaster objBrandMaster = new BrandMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objBrandMaster.GetBrandMaster(HttpContext.Current.Session["CompId"].ToString()), "Brand_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Brand_Name"].ToString();
        }
        return txt;
    }


    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        FillGrid();
        FillGridBin();
        Reset();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }

    public void FillGrid()
    {
        DataTable dt = objBrand.GetBrandMaster(Session["CompId"].ToString());

        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvBrandMaster, dt, "", "");
        //AllPageCode();
        Session["dtFilter_Brand__Master"] = dt;
        Session["Brand"] = dt;

        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";

    }
    public void FillGridBin()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = objBrand.GetBrandMasterInactive(Session["CompId"].ToString());
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvBrandMasterBin, dt, "", "");

            Session["dtbinFilter"] = dt;
            Session["dtbinBrand"] = dt;
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
            if (dt.Rows.Count == 0)
            {
                // imgBtnRestore.Visible = false;
                // ImgbtnSelectAll.Visible = false;
            }
            else
            {
                //AllPageCode();
            }
        }
        catch
        {
        }

    }


    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvBrandMasterBin.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in gvBrandMasterBin.Rows)
        {

            if (chkSelAll.Checked == true)
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = false;
            }
        }
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtUnit = (DataTable)Session["dtbinFilter"];
        ArrayList userdetails = new ArrayList();
        Session["CHECKED_ITEMS"] = null;

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (Session["CHECKED_ITEMS"] != null)
                {
                    userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                }

                if (!userdetails.Contains(dr["Brand_Id"]))
                {
                    userdetails.Add(dr["Brand_Id"]);
                }
            }
            foreach (GridViewRow GR in gvBrandMasterBin.Rows)
            {
                ((CheckBox)GR.FindControl("chkgvSelect")).Checked = true;
            }
            if (userdetails.Count > 0 && userdetails != null)
            {
                Session["CHECKED_ITEMS"] = userdetails;
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtBinFilter"];
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvBrandMasterBin, dtUnit1, "", "");
            ViewState["Select"] = null;
        }
    }

    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int b = 0;
        ArrayList userdetail = new ArrayList();
        if (gvBrandMasterBin.Rows.Count > 0)
        {
            SaveCheckedValuesemplog();
            if (Session["CHECKED_ITEMS"] != null)
            {
                userdetail = (ArrayList)Session["CHECKED_ITEMS"];
                if (userdetail.Count > 0)
                {
                    for (int j = 0; j < userdetail.Count; j++)
                    {
                        if (userdetail[j].ToString() != "")
                        {
                            b = objBrand.DeleteBrandMaster(Session["CompId"].ToString(), userdetail[j].ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString());

                        }
                    }
                }

                if (b != 0)
                {

                    FillGrid();
                    FillGridBin();
                    lblSelectedRecord.Text = "";
                    ViewState["Select"] = null;
                    DisplayMessage("Record Activated");
                    Session["CHECKED_ITEMS"] = null;

                }
                else
                {
                    int fleg = 0;
                    foreach (GridViewRow Gvr in gvBrandMasterBin.Rows)
                    {
                        CheckBox chk = (CheckBox)Gvr.FindControl("chkgvSelect");
                        if (chk.Checked)
                        {
                            fleg = 1;
                        }
                        else
                        {
                            fleg = 0;
                        }
                    }
                    if (fleg == 0)
                    {
                        DisplayMessage("Please Select Record");
                    }
                    else
                    {
                        DisplayMessage("Record Not Activated");
                    }
                }
            }
            else
            {
                DisplayMessage("Please Select Record");
                gvBrandMasterBin.Focus();
                return;
            }
        }
    }

    public void Reset()
    {
        txtBrandCode.Text = "";
        txtBrandName.Text = "";
        txtBrandNameL.Text = "";
        Session["CHECKED_ITEMS"] = null;
        txtManagerName.Text = "";
        imgLogo.ImageUrl = "";
        txtAddressName.Text = "";

        Lbl_Tab_New.Text = Resources.Attendance.New;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        editid.Value = "";
        txtValue.Text = "";
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        GvAddressName.DataSource = null;
        GvAddressName.DataBind();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
        //AllPageCode();

    }


    protected void txtBrand_OnTextChanged(object sender, EventArgs e)
    {

        if (editid.Value == "")
        {
            DataTable dt = objBrand.GetBrandMasterByBrandName(Session["CompId"].ToString().ToString(), txtBrandName.Text.Trim());
            if (dt.Rows.Count > 0)
            {
                txtBrandName.Text = "";
                DisplayMessage("Brand Name Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtBrandName);
                return;
            }
            DataTable dt1 = objBrand.GetBrandMasterInactive(Session["CompId"].ToString().ToString());
            dt1 = new DataView(dt1, "Brand_Name='" + txtBrandName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                txtBrandName.Text = "";
                DisplayMessage("Brand Name Already Exists - Go to Bin Tab");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtBrandName);
                return;
            }
            txtBrandNameL.Focus();
        }
        else
        {
            DataTable dtTemp = objBrand.GetBrandMasterById(Session["CompId"].ToString().ToString(), editid.Value);
            if (dtTemp.Rows.Count > 0)
            {
                if (dtTemp.Rows[0]["Brand_Name"].ToString() != txtBrandName.Text)
                {
                    DataTable dt = objBrand.GetBrandMaster(Session["CompId"].ToString().ToString());
                    dt = new DataView(dt, "Brand_Name='" + txtBrandName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        txtBrandName.Text = "";
                        DisplayMessage("Brand Name Already Exists");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtBrandName);
                        return;
                    }
                    DataTable dt1 = objBrand.GetBrandMasterInactive(Session["CompId"].ToString().ToString());
                    dt1 = new DataView(dt1, "Brand_Name='" + txtBrandName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt1.Rows.Count > 0)
                    {
                        txtBrandName.Text = "";
                        DisplayMessage("Brand Name Already Exists - Go to Bin Tab");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtBrandName);
                        return;
                    }
                }
            }
            txtBrandNameL.Focus();
        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        imgLogo.ImageUrl = "~/CompanyResource/" + "/" + Session["CompId"] + "/" + FULogoPath.FileName;
    }

    #region Add AddressName Concept

    public void ResetAddressName()
    {
        txtAddressName.Text = "";
        hdnAddressId.Value = "";
        hdnAddressName.Value = "";
    }
    public DataTable CreateAddressDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Trans_Id");
        dt.Columns.Add("Address_Name");
        dt.Columns.Add("FullAddress");
        dt.Columns.Add("Is_Default", typeof(bool));
        return dt;
    }
    public DataTable FillAddressDataTabel()
    {
        string strNewSNo = string.Empty;
        DataTable dt = CreateAddressDataTable();
        if (GvAddressName.Rows.Count > 0)
        {
            for (int i = 0; i < GvAddressName.Rows.Count + 1; i++)
            {
                if (dt.Rows.Count != GvAddressName.Rows.Count)
                {
                    dt.Rows.Add(i);
                    Label lblSNo = (Label)GvAddressName.Rows[i].FindControl("lblSNo");
                    Label lblAddressName = (Label)GvAddressName.Rows[i].FindControl("lblgvAddressName");

                    dt.Rows[i]["Trans_Id"] = lblSNo.Text;
                    strNewSNo = lblSNo.Text;
                    dt.Rows[i]["Address_Name"] = lblAddressName.Text;
                    try
                    {
                        dt.Rows[i]["FullAddress"] = GetAddressByAddressName(lblAddressName.Text);
                    }
                    catch
                    {

                    }
                    dt.Rows[i]["Is_Default"] = ((CheckBox)GvAddressName.Rows[i].FindControl("chkdefault")).Checked;


                }
                else
                {
                    dt.Rows.Add(i);
                    dt.Rows[i]["Trans_Id"] = (float.Parse(strNewSNo) + 1).ToString();
                    dt.Rows[i]["Address_Name"] = txtAddressName.Text;
                    try
                    {
                        dt.Rows[i]["FullAddress"] = GetAddressByAddressName(txtAddressName.Text);
                    }
                    catch
                    {

                    }
                    dt.Rows[i]["Is_Default"] = false.ToString();
                }
            }
        }
        else
        {
            dt.Rows.Add(0);
            dt.Rows[0]["Trans_Id"] = "1";
            dt.Rows[0]["Address_Name"] = txtAddressName.Text;
            try
            {
                dt.Rows[0]["FullAddress"] = GetAddressByAddressName(txtAddressName.Text);
            }
            catch
            {

            }
            dt.Rows[0]["Is_Default"] = false.ToString();

        }
        if (dt.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvAddressName, dt, "", "");
        }
        return dt;
    }

    public string GetAddressByAddressName(string AddressName)
    {
        string Address = string.Empty;
        DataTable dt = AM.GetAddressDataByAddressName(AddressName);

        if (dt.Rows.Count > 0)
        {
            Address = dt.Rows[0]["FullAddress"].ToString();
        }
        return Address;
    }
    protected void chkgvSelect_CheckedChangedDefault(object sender, EventArgs e)
    {
        foreach (GridViewRow gvr in GvAddressName.Rows)
        {
            CheckBox chk = (CheckBox)gvr.FindControl("chkdefault");
            chk.Checked = false;
        }

        CheckBox chk1 = (CheckBox)sender;

        chk1.Checked = true;


    }
    public string GetContactEmailId(string AddressName)
    {
        string ContactEmailId = string.Empty;
        DataTable DtAddress = new DataTable();
        DtAddress = AM.GetAddressDataByAddressName(AddressName);
        if (DtAddress.Rows.Count > 0)
        {
            if (DtAddress.Rows[0]["EmailId1"].ToString() != "")
            {
                ContactEmailId = DtAddress.Rows[0]["EmailId1"].ToString();
            }
        }
        return ContactEmailId;
    }
    public string GetContactFaxNo(string AddressName)
    {
        string ContactFaxNo = string.Empty;
        DataTable DtAddress = new DataTable();
        DtAddress = AM.GetAddressDataByAddressName(AddressName);
        if (DtAddress.Rows.Count > 0)
        {
            DataTable dt_ContactNoData = objContactnoMaster.getDataByPKID(DtAddress.Rows[0]["Trans_Id"].ToString(), "Set_AddressMaster");
            if (dt_ContactNoData.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_ContactNoData.Rows)
                {
                    if (dr["Type"].ToString() == "Work Fax")
                    {
                        ContactFaxNo = ContactFaxNo + "Work: " + dr["Phone_no"].ToString() + " <br/> ";
                    }

                    if (dr["Type"].ToString() == "Home Fax")
                    {
                        ContactFaxNo = ContactFaxNo + "Home: " + dr["Phone_no"].ToString() + " <br/> ";
                    }
                }

            }
            //if (DtAddress.Rows[0]["FaxNo"].ToString() != "")
            //{
            //    ContactFaxNo = DtAddress.Rows[0]["FaxNo"].ToString();
            //}
        }
        return ContactFaxNo;
    }
    public string GetContactPhoneNo(string AddressName)
    {
        string ContactPhoneNo = string.Empty;
        DataTable DtAddress = new DataTable();
        DtAddress = AM.GetAddressDataByAddressName(AddressName);
        if (DtAddress.Rows.Count > 0)
        {

            DataTable dt_ContactNoData = objContactnoMaster.getDataByPKID(DtAddress.Rows[0]["Trans_Id"].ToString(), "Set_AddressMaster");

            if (dt_ContactNoData.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_ContactNoData.Rows)
                {
                    if (dr["Type"].ToString() == "Work")
                    {
                        ContactPhoneNo = ContactPhoneNo + "Work: " + dr["Phone_no"].ToString() + " <br/> ";
                    }

                    if (dr["Type"].ToString() == "LandLine")
                    {
                        ContactPhoneNo = ContactPhoneNo + "LandLine:" + dr["Phone_no"].ToString() + " <br/> ";
                    }

                    if (dr["Type"].ToString() == "Home")
                    {
                        ContactPhoneNo = ContactPhoneNo + "Home:" + dr["Phone_no"].ToString() + " <br/> ";
                    }
                }
            }
            else
            {
                ContactPhoneNo = "";
            }
            //if (DtAddress.Rows[0]["PhoneNo1"].ToString() != "")
            //{
            //    ContactPhoneNo = DtAddress.Rows[0]["PhoneNo1"].ToString();
            //}
            //if (DtAddress.Rows[0]["PhoneNo2"].ToString() != "")
            //{
            //    if (ContactPhoneNo != "")
            //    {
            //        ContactPhoneNo = ContactPhoneNo + "," + DtAddress.Rows[0]["PhoneNo2"].ToString();
            //    }
            //    else
            //    {
            //        ContactPhoneNo = DtAddress.Rows[0]["PhoneNo2"].ToString();
            //    }
            //}


        }
        return ContactPhoneNo;
    }
    public string GetContactMobileNo(string AddressName)
    {
        string ContactPhoneNo = string.Empty;
        DataTable DtAddress = new DataTable();
        DtAddress = AM.GetAddressDataByAddressName(AddressName);
        if (DtAddress.Rows.Count > 0)
        {
            DataTable dt_ContactNoData = objContactnoMaster.getDataByPKID(DtAddress.Rows[0]["Trans_Id"].ToString(), "Set_AddressMaster");
            if (dt_ContactNoData.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_ContactNoData.Rows)
                {
                    if (dr["Type"].ToString() == "Mobile" && dr["Is_default"].ToString() == "True")
                    {
                        ContactPhoneNo = ContactPhoneNo + dr["Phone_no"].ToString() + " ";
                    }
                }

            }

            //if (DtAddress.Rows[0]["MobileNo1"].ToString() != "")
            //{
            //    ContactPhoneNo = DtAddress.Rows[0]["MobileNo1"].ToString();
            //}
            //if (DtAddress.Rows[0]["MobileNo2"].ToString() != "")
            //{
            //    if (ContactPhoneNo != "")
            //    {
            //        ContactPhoneNo = ContactPhoneNo + "," + DtAddress.Rows[0]["MobileNo2"].ToString();
            //    }
            //    else
            //    {
            //        ContactPhoneNo = DtAddress.Rows[0]["MobileNo2"].ToString();
            //    }
            //}
        }
        return ContactPhoneNo;
    }


    #endregion

    #region Add New Address Concept

    protected void btnClosePanel_Click(object sender, EventArgs e)
    {

    }
    #endregion


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListNewAddress(string prefixText, int count, string contextKey)
    {
        Set_AddressMaster Address = new Set_AddressMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = Address.GetDistinctAddressName(prefixText);

        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Address_Name"].ToString();
        }
        return str;
    }
    protected void FuLogo_FileUploadComplete(object sender, EventArgs e)
    {
        if (FULogoPath.HasFile)
        {
            string ext = FULogoPath.FileName.Substring(FULogoPath.FileName.Split('.')[0].Length);
            if ((ext != ".png") && (ext != ".jpg") && (ext != ".jpge"))
            {
                DisplayMessage("Invalid File Type, Select Only .png, .jpg, .jpge extension file");
                return;
            }
            else
            {
                if (!Directory.Exists(Server.MapPath("~/CompanyResource/") + Session["CompId"]))
                {
                    Directory.CreateDirectory(Server.MapPath("~/CompanyResource/") + Session["CompId"]);
                }

                string path = Server.MapPath("~/CompanyResource/" + "/" + Session["CompId"] + "/") + FULogoPath.FileName;
                FULogoPath.SaveAs(path);
                Session["empimgpath"] = FULogoPath.FileName;
            }
        }
    }




    protected void txtAddressName_TextChanged(object sender, EventArgs e)
    {
        if (txtAddressName.Text != "")
        {
            DataTable dtAM = AM.GetAddressDataByAddressName(txtAddressName.Text);
            if (dtAM.Rows.Count > 0)
            {
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(imgAddAddressName);
            }
            else
            {
                txtAddressName.Text = "";
                DisplayMessage("Choose In Suggestions Only");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
                return;
            }
        }
        else
        {
            DisplayMessage("Enter Address Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAddressName(string prefixText, int count, string contextKey)
    {
        Set_AddressMaster AddressN = new Set_AddressMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = AddressN.getAddressNamePreText(prefixText);
        string[] str = new string[0];
        if (dt != null)
        {
            str = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["Address_Name"].ToString();
            }
        }
        else
        {
            str = null;
        }
        return str;
    }
    protected void imgAddAddressName_Click(object sender, EventArgs e)
    {
        if (txtAddressName.Text != "")
        {
            string strA = "0";
            foreach (GridViewRow gve in GvAddressName.Rows)
            {
                Label lblCAddressName = (Label)gve.FindControl("lblgvAddressName");
                if (txtAddressName.Text == lblCAddressName.Text)
                {
                    strA = "1";
                }
            }
            if (hdnAddressId.Value == "")
            {
                if (strA == "0")
                {
                    FillAddressChidGird("Save");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
                }
                else
                {
                    txtAddressName.Text = "";
                    DisplayMessage("Address Name Already Exists");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
                }
            }
            else
            {
                if (txtAddressName.Text == hdnAddressName.Value)
                {
                    FillAddressChidGird("Edit");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
                }
                else
                {
                    if (strA == "0")
                    {
                        FillAddressChidGird("Edit");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
                    }
                    else
                    {
                        txtAddressName.Text = "";
                        DisplayMessage("Address Name Already Exists");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
                    }
                }
            }
        }
        else
        {
            DisplayMessage("Enter Address Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
        }
        txtAddressName.Focus();
    }
    protected void btnAddNewAddress_Click(object sender, EventArgs e)
    {
        addaddress.Reset();
        Session["Add_Address_Popup"] = "";
        txtAddressName.Text = "";
        Hdn_Address_ID.Value = "";
        hdnAddressId.Value = "";
        hdnAddressName.Value = "";
        hdntxtaddressid.Value = txtAddressName.Text;
        HttpContext.Current.Session["AddCtrl_State_Id"] = "0";

        if (ViewState["Country_Id"] != null)
        {
            addaddress.BtnNew_click(ViewState["Country_Id"].ToString());
        }
        addaddress.fillHeader(txtBrandName.Text);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Address_Open()", true);
    }
    protected void btnAddressEdit_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        Label lblgvAddressName = gvRow.FindControl("lblgvAddressName") as Label;
        Hdn_Address_ID.Value = lblgvAddressName.Text;
        hdntxtaddressid.Value = lblgvAddressName.Text;
        txtAddressName.Text = lblgvAddressName.Text;
        Session["Add_Address_Popup"] = lblgvAddressName.Text;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Address_Open()", true);
        hdnAddressId.Value = e.CommandArgument.ToString();

        //user to fill address data on edit button click 
        addaddress.GetAddressInformationByAddressname(lblgvAddressName.Text);

        FillAddressDataTabelEdit();
        addaddress.fillHeader(txtBrandName.Text);
        addaddress.FillLocationNCode();
    }
    public DataTable FillAddressDataTabelEdit()
    {
        DataTable dt = CreateAddressDataTable();
        for (int i = 0; i < GvAddressName.Rows.Count; i++)
        {
            dt.Rows.Add(i);
            Label lblSNo = (Label)GvAddressName.Rows[i].FindControl("lblSNo");
            Label lblgvAddressName = (Label)GvAddressName.Rows[i].FindControl("lblgvAddressName");
            dt.Rows[i]["Trans_Id"] = lblSNo.Text;
            dt.Rows[i]["Address_Name"] = lblgvAddressName.Text;
            dt.Rows[i]["FullAddress"] = (GvAddressName.Rows[i].FindControl("lblgvAddress") as Label).Text;
            //dt.Rows[i]["Address"] = GetAddressByAddressName(lblgvAddressName.Text);
            dt.Rows[i]["Is_Default"] = ((CheckBox)GvAddressName.Rows[i].FindControl("chkdefault")).Checked;
        }
        DataView dv = new DataView(dt);
        dv.RowFilter = "Trans_Id='" + hdnAddressId.Value + "'";
        dt = (DataTable)dv.ToTable();
        if (dt.Rows.Count != 0)
        {
            txtAddressName.Text = dt.Rows[0]["Address_Name"].ToString();
            hdnAddressName.Value = dt.Rows[0]["Address_Name"].ToString();
        }
        return dt;
    }
    protected void btnAddressDelete_Command(object sender, CommandEventArgs e)
    {
        hdnAddressId.Value = e.CommandArgument.ToString();
        FillAddressChidGird("Del");
    }
    public void FillAddressChidGird(string CommandName)
    {
        DataTable dt = new DataTable();
        if (CommandName.ToString() == "Del")
        {
            dt = FillAddressDataTabelDelete();
        }
        else if (CommandName.ToString() == "Edit")
        {
            dt = FillAddressDataTableUpdate();
        }
        else
        {
            dt = FillAddressDataTabel();
        }
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvAddressName, dt, "", "");
        ResetAddressName();
    }
    public DataTable FillAddressDataTabelDelete()
    {
        DataTable dt = CreateAddressDataTable();
        for (int i = 0; i < GvAddressName.Rows.Count; i++)
        {
            dt.Rows.Add(i);
            Label lblSNo = (Label)GvAddressName.Rows[i].FindControl("lblSNo");
            Label lblgvAddressName = (Label)GvAddressName.Rows[i].FindControl("lblgvAddressName");
            dt.Rows[i]["Trans_Id"] = lblSNo.Text;
            dt.Rows[i]["Address_Name"] = lblgvAddressName.Text;
            dt.Rows[i]["FullAddress"] = GetAddressByAddressName(lblgvAddressName.Text);
            dt.Rows[i]["Is_Default"] = ((CheckBox)GvAddressName.Rows[i].FindControl("chkdefault")).Checked;
        }
        DataView dv = new DataView(dt);
        dv.RowFilter = "Trans_Id<>'" + hdnAddressId.Value + "'";
        dt = (DataTable)dv.ToTable();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            dt.Rows[i]["Trans_Id"] = i + 1;
        }
        return dt;
    }
    public DataTable FillAddressDataTableUpdate()
    {
        DataTable dt = CreateAddressDataTable();
        for (int i = 0; i < GvAddressName.Rows.Count; i++)
        {
            dt.Rows.Add(i);
            Label lblSNo = (Label)GvAddressName.Rows[i].FindControl("lblSNo");
            Label lblgvAddressName = (Label)GvAddressName.Rows[i].FindControl("lblgvAddressName");
            dt.Rows[i]["Trans_Id"] = lblSNo.Text;
            dt.Rows[i]["Address_Name"] = lblgvAddressName.Text;
            dt.Rows[i]["FullAddress"] = GetAddressByAddressName(lblgvAddressName.Text);
            dt.Rows[i]["Is_Default"] = ((CheckBox)GvAddressName.Rows[i].FindControl("chkdefault")).Checked;
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (hdnAddressId.Value == dt.Rows[i]["Trans_Id"].ToString())
            {
                dt.Rows[i]["Address_Name"] = txtAddressName.Text;
                dt.Rows[i]["FullAddress"] = GetAddressByAddressName(txtAddressName.Text);
            }
        }
        return dt;
    }


    protected void BtnMoreNumber_Command(object sender, CommandEventArgs e)
    {

        DataTable DtAddress = new DataTable();
        DtAddress = AM.GetAddressDataByAddressName(e.CommandArgument.ToString());
        string data = "";
        if (DtAddress.Rows.Count > 0)
        {
            DataTable dt_ContactNodata = objContactnoMaster.getDataByPKID(DtAddress.Rows[0]["Trans_Id"].ToString(), "Set_AddressMaster");

            if (dt_ContactNodata.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_ContactNodata.Rows)
                {
                    data = data + "<b>" + dr["Type"].ToString() + "</b>:" + dr["Phone_no"].ToString() + " <br/>";
                }
            }
        }

        if (data.Trim() != "")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "Modal_Number_Open('" + data + "');", true);
        }
    }

    public string IsVisible(string data)
    {
        DataTable DtAddress = AM.GetAddressDataByAddressName(data);
        if (DtAddress.Rows.Count > 0)
        {
            DataTable dt_ContactNoData = objContactnoMaster.getDataByPKID(DtAddress.Rows[0]["Trans_Id"].ToString(), "Set_AddressMaster");
            if (dt_ContactNoData.Rows.Count > 1)
            {
                return "More";
            }
            else
            {
                return "";
            }
        }
        else
        {
            return "";
        }

    }


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListStateName(string prefixText, int count, string contextKey)
    {
        if (HttpContext.Current.Session["AddCtrl_Country_Id"].ToString() == "")
        {
            return null;
        }
        StateMaster objStateMaster = new StateMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objStateMaster.GetAllStateByPrefixText(prefixText, HttpContext.Current.Session["AddCtrl_Country_Id"].ToString());
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["State_Name"].ToString();
        }
        return txt;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCityName(string prefixText, int count, string contextKey)
    {
        try
        {
            if (HttpContext.Current.Session["AddCtrl_State_Id"].ToString() == "" || HttpContext.Current.Session["AddCtrl_State_Id"].ToString() == "0")
            {
                return null;
            }
            CityMaster objCityMaster = new CityMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dt = objCityMaster.GetAllCityByPrefixText(prefixText, HttpContext.Current.Session["AddCtrl_State_Id"].ToString());
            string[] txt = new string[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["City_Name"].ToString();
            }
            return txt;
        }
        catch
        {
            return null;
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListContactNumber(string prefixText, int count, string contextKey)
    {
        ContactNoMaster objContactNumMaster = new ContactNoMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objContactNumMaster.getNumberList_PreText(prefixText);
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Phone_no"].ToString();
        }
        return txt;
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string ddlCountry_IndexChanged(string CountryId)
    {
        CountryMaster ObjSysCountryMaster = new CountryMaster(HttpContext.Current.Session["DBConnection"].ToString());
        HttpContext.Current.Session["AddCtrl_Country_Id"] = CountryId;
        HttpContext.Current.Session["AddCtrl_State_Id"] = "0";
        return "+" + ObjSysCountryMaster.GetCountryMasterById(CountryId).Rows[0]["Country_Code"].ToString();
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static void resetAddress()
    {
        HttpContext.Current.Session["AddCtrl_State_Id"] = "0";
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string txtCity_TextChanged(string stateId, string cityName)
    {
        CityMaster ObjCityMaster = new CityMaster(HttpContext.Current.Session["DBConnection"].ToString());

        string City_id = ObjCityMaster.GetCityIdFromStateIdNCityName(stateId, cityName);

        if (City_id != "")
        {
            return City_id;
        }
        else
        {
            return "0";
        }
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string txtState_TextChanged(string CountryId, string StateName)
    {
        StateMaster ObjStatemaster = new StateMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string stateId = ObjStatemaster.GetStateIdFromCountryIdNStateName(CountryId, StateName);
        if (stateId != "")
        {
            HttpContext.Current.Session["AddCtrl_State_Id"] = stateId;
            return stateId;
        }
        else
        {
            HttpContext.Current.Session["AddCtrl_State_Id"] = "0";
            return "0";
        }
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static int txtAddressNameNew_TextChanged(string AddressName, string addressId)
    {
        // return  1 when 'Address Name Already Exists' and 0 when not present
        Set_AddressMaster AM = new Set_AddressMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string data = AM.GetAddressDataExistOrNot(AddressName, addressId);
        if (data == "0")
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmailMaster(string prefixText, int count, string contextKey)
    {
        ES_EmailMaster_Header Email = new ES_EmailMaster_Header(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = Email.GetDistinctEmailId(prefixText);

        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Email_Id"].ToString();
        }
        return str;
    }

}
