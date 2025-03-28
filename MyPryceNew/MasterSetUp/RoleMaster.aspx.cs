using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

public partial class MasterSetUp_RoleMaster : BasePage
{
    Set_Location_Department objLocDept = null;
    IT_Application_Customer objITCust = null;
    RoleMaster objRole = null;
    PageControlCommon objPageCmn = null;
    RolePermission objRolePermission = null;
    CompanyMaster ObjComp =null;
    RoleDataPermission objRoleDataPerm = null;

    ModuleMaster objModule = null;
    ObjectMaster ObjItObject = null;
    Common cmn = null;
    SystemParameter objSys = null;
    BrandMaster objBrand = null;
    LocationMaster objLocation = null;
    DepartmentMaster objDepartment = null;
    UserMaster objUser = null;
    IT_App_Op_Permission obj_OP_Permission = null;
    IT_ObjectEntry objObjectEntry = null;

    string CompanyId = string.Empty;
    string CompanyIds = string.Empty;
    string BrandChecked = string.Empty;
    string LocationChecked = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        objITCust = new IT_Application_Customer(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objRolePermission = new RolePermission(Session["DBConnection"].ToString());
        ObjComp = new CompanyMaster(Session["DBConnection"].ToString());
        objRoleDataPerm = new RoleDataPermission(Session["DBConnection"].ToString());

        objModule = new ModuleMaster(Session["DBConnection"].ToString());
        ObjItObject = new ObjectMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objBrand = new BrandMaster(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objDepartment = new DepartmentMaster(Session["DBConnection"].ToString());
        objUser = new UserMaster(Session["DBConnection"].ToString());
        obj_OP_Permission = new IT_App_Op_Permission(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        Page.Title = objSys.GetSysTitle();
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../mastersetup/rolemaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            txtValue.Focus();
            navTree.Attributes.Add("onclick", "OnTreeClick(event)");
            TreeViewDepartment.Attributes.Add("onclick", "OnTreeClick(event)");
            Div_Rol_Permission.Style.Add("display", "");

            //FillGridBin();
            FillGrid();
            FillCompany();
            Session["CHECKED_ITEMS"] = null;
            //btnList_Click(null, null);
            BindTree();
            ////AllPageCode();
        }


        //this is added by jitendra on 24-03-2015 for event on checkbox of treeview which was not working

        //navTree.Attributes.Add("onclick", "postBackByObject()");

        //else
        //{

        //    try
        //    {
        //        if (navTree.SelectedNode.Checked == true)
        //        {
        //            UnSelectChild(navTree.SelectedNode);
        //        }
        //        else
        //        {
        //            SelectChild(navTree.SelectedNode);
        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}
    }


    #region treeviewconcept
    protected void navTree_SelectedNodeChanged1(object sender, EventArgs e)
    {
        try
        {
            if (navTree.SelectedNode.Checked == true)
            {
                UnSelectChild(navTree.SelectedNode);
            }
            else
            {
                SelectChild(navTree.SelectedNode);
            }
        }
        catch (Exception)
        {
        }
    }
    protected void navTree_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
    {
        if (e != null && e.Node != null)
        {
            try
            {
                if (e.Node.Checked == true)
                {
                    CheckTreeNodeRecursive(e.Node, true);
                    try
                    {
                        SelectChild(e.Node);
                    }
                    catch (Exception)
                    {

                    }
                }
                else
                {
                    CheckTreeNodeRecursive(e.Node, false);
                    UnSelectChild(e.Node);
                }
            }
            catch (Exception)
            {

            }
        }
    }
    private void CheckTreeNodeRecursive(TreeNode parent, bool fCheck)
    {
        foreach (TreeNode child in parent.ChildNodes)
        {
            if (child.Checked != fCheck)
            {
                child.Checked = fCheck;
            }

            if (child.ChildNodes.Count > 0)
            {
                CheckTreeNodeRecursive(child, fCheck);
            }
        }
    }
    #endregion

    public string GetUserName(string UserId)
    {
        string UserName = objUser.GetNameByUserId(UserId, Session["CompId"].ToString());
        return UserName;
    }
    protected void txtRoleName_OnTextChanged(object sender, EventArgs e)
    {
        if (editid.Value == "")
        {
            DataTable dt = objRole.GetRoleMasterByRoleName(Session["CompId"].ToString().ToString(), txtRoleName.Text.Trim());
            if (dt.Rows.Count > 0)
            {
                txtRoleName.Text = "";
                DisplayMessage("Role Name Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtRoleName);
                return;
            }

            DataTable dt1 = objRole.GetRoleMasterInactive(Session["CompId"].ToString().ToString());
            dt1 = new DataView(dt1, "Role_Name='" + txtRoleName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                txtRoleName.Text = "";
                DisplayMessage("Role Name Already Exists - Go to Bin Tab");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtRoleName);
                return;
            }
            txtRoleNameL.Focus();
        }
        else
        {
            DataTable dtTemp = objRole.GetRoleMasterById(Session["CompId"].ToString().ToString(), editid.Value);
            if (dtTemp.Rows.Count > 0)
            {
                if (dtTemp.Rows[0]["Role_Name"].ToString() != txtRoleName.Text)
                {
                    DataTable dt = objRole.GetRoleMaster(Session["CompId"].ToString().ToString());
                    dt = new DataView(dt, "Role_Name='" + txtRoleName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        txtRoleName.Text = "";
                        DisplayMessage("Role Name Already Exists");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtRoleName);
                        return;
                    }
                    DataTable dt1 = objRole.GetRoleMaster(Session["CompId"].ToString().ToString());
                    dt1 = new DataView(dt1, "Role_Name='" + txtRoleName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt1.Rows.Count > 0)
                    {
                        txtRoleName.Text = "";
                        DisplayMessage("Role Name Already Exists - Go to Bin Tab");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtRoleName);
                        return;
                    }
                }
            }
            txtRoleNameL.Focus();
        }
    }



    private void FillCompany()
    {
        //string CompIds = string.Empty;
        //DataTable dtCompany = new DataTable();
        //dtCompany = ObjComp.GetCompanyMaster();
        //if (Session["CompId"].ToString() == "0" || Session["RoleId"].ToString() == "0")
        //{

        //}
        //else
        //{
        //    if (editid.Value.ToString() != "")
        //    {
        //        DataTable dtRoleData = objRoleDataPerm.GetRoleDataPermissionById(editid.Value);
        //        DataTable dtComp = new DataView(dtRoleData, "Record_Type='C'", "", DataViewRowState.CurrentRows).ToTable();
        //        for (int C = 0; C < dtComp.Rows.Count; C++)
        //        {
        //            if (dtComp.Rows.Count > 0)
        //            {
        //                CompIds += dtComp.Rows[C]["Record_Id"].ToString() + ",";
        //            }
        //        }
        //        dtCompany = new DataView(dtCompany, "Company_Id in(" + CompIds.Substring(0, CompIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        //    }
        //    else
        //    {
        //        DataTable dtRoleData = objRoleDataPerm.GetRoleDataPermissionById(Session["RoleId"].ToString());
        //        DataTable dtComp = new DataView(dtRoleData, "Record_Type='C'", "", DataViewRowState.CurrentRows).ToTable();
        //        for (int C = 0; C < dtComp.Rows.Count; C++)
        //        {
        //            if (dtComp.Rows.Count > 0)
        //            {
        //                CompIds += dtComp.Rows[C]["Record_Id"].ToString() + ",";
        //            }
        //        }
        //        dtCompany = new DataView(dtCompany, "Company_Id in(" + CompIds.Substring(0, CompIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        //    }

        //}
        //if (dtCompany.Rows.Count > 0)
        //{
        //    //Common Function add By Lokesh on 14-05-2015
        //    objPageCmn.FillData((object)chkCompany, dtCompany, "Company_Name", "Company_Id");
        //}
    }
    public void FillBrand()
    {
        //DataTable dtBrand = new DataTable();
        //string BndIds = string.Empty;
        //dtBrand = objBrand.GetAllBrandMaster();
        //if (Session["CompId"].ToString() == "0" || Session["RoleId"].ToString() == "0")
        //{

        //}
        //else
        //{
        //    // dtBrand = objBrand.GetBrandMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString());
        //    if (editid.Value.ToString() != "")
        //    {
        //        DataTable dtRoleData = objRoleDataPerm.GetRoleDataPermissionById(editid.Value);
        //        DataTable dtBnd = new DataView(dtRoleData, "Record_Type='B'", "", DataViewRowState.CurrentRows).ToTable();
        //        for (int C = 0; C < dtBnd.Rows.Count; C++)
        //        {
        //            if (dtBnd.Rows.Count > 0)
        //            {
        //                BndIds += dtBnd.Rows[C]["Record_Id"].ToString() + ",";
        //            }
        //        }
        //        dtBrand = new DataView(dtBrand, "Brand_Id in(" + BndIds.Substring(0, BndIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        //    }
        //    else
        //    {
        //        DataTable dtRoleData = objRoleDataPerm.GetRoleDataPermissionById(Session["RoleId"].ToString());
        //        DataTable dtBnd = new DataView(dtRoleData, "Record_Type='B'", "", DataViewRowState.CurrentRows).ToTable();
        //        for (int C = 0; C < dtBnd.Rows.Count; C++)
        //        {
        //            if (dtBnd.Rows.Count > 0)
        //            {
        //                BndIds += dtBnd.Rows[C]["Record_Id"].ToString() + ",";
        //            }
        //        }
        //        dtBrand = new DataView(dtBrand, "Brand_Id in(" + BndIds.Substring(0, BndIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        //    }
        //}
        //if (dtBrand.Rows.Count > 0)
        //{
        //    //Common Function add By Lokesh on 14-05-2015
        //    objPageCmn.FillData((object)chkBrand, dtBrand, "Brand_Name", "Brand_Id");
        //    foreach (ListItem lst1 in chkBrand.Items)
        //    {
        //        chkBrand.Items.FindByValue(lst1.Value).Selected = true;
        //    }
        //}
    }

    public void FillLocation()
    {
        //DataTable dtLocation = new DataTable();
        //string LocIds = string.Empty;
        //dtLocation = objLocation.GetAllLocationMaster();
        //if (Session["CompId"].ToString() == "0" || Session["RoleId"].ToString() == "0")
        //{

        //}
        //else
        //{
        //    if (editid.Value.ToString() != "")
        //    {
        //        DataTable dtRoleData = objRoleDataPerm.GetRoleDataPermissionById(editid.Value);
        //        DataTable dtLoc = new DataView(dtRoleData, "Record_Type='L'", "", DataViewRowState.CurrentRows).ToTable();
        //        for (int L = 0; L < dtLoc.Rows.Count; L++)
        //        {
        //            if (dtLoc.Rows.Count > 0)
        //            {
        //                LocIds += dtLoc.Rows[L]["Record_Id"].ToString() + ",";
        //            }
        //        }
        //        dtLocation = new DataView(dtLocation, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        //    }
        //    else
        //    {
        //        DataTable dtRoleData = objRoleDataPerm.GetRoleDataPermissionById(Session["RoleId"].ToString());
        //        DataTable dtLoc = new DataView(dtRoleData, "Record_Type='L'", "", DataViewRowState.CurrentRows).ToTable();
        //        for (int L = 0; L < dtLoc.Rows.Count; L++)
        //        {
        //            if (dtLoc.Rows.Count > 0)
        //            {
        //                LocIds += dtLoc.Rows[L]["Record_Id"].ToString() + ",";
        //            }
        //        }
        //        dtLocation = new DataView(dtLocation, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        //    }
        //}
        //if (dtLocation.Rows.Count > 0)
        //{
        //    //Common Function add By Lokesh on 14-05-2015
        //    objPageCmn.FillData((object)chkLocation, dtLocation, "Location_Name", "Location_Id");
        //}
    }
    protected void chkCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtBrand = new DataTable();
        string BndIds = string.Empty;
        string B1 = string.Empty;
        string B2 = string.Empty;
        string B3 = string.Empty;
        string TB = string.Empty;
        for (int i = 0; i < chkBrand.Items.Count; i++)
        {
            if (chkBrand.Items[i].Selected)
            {
                BrandChecked += chkBrand.Items[i].Value + ",";

            }
        }
        ViewState["BrandChecked"] = BrandChecked;

        CompanyIds = string.Empty;
        for (int i = 0; i < chkCompany.Items.Count; i++)
        {
            if (chkCompany.Items[i].Selected)
            {
                CompanyId = chkCompany.Items[i].Value;
                CompanyIds += CompanyId + ",";
            }

        }
        dtBrand = objBrand.GetBrandMaster(CompanyIds);
        if (Session["CompId"].ToString() == "0" || Session["RoleId"].ToString() == "0")
        {
        }
        else
        {
            if (editid.Value.ToString() != "")
            {
                DataTable dtRoleData = objRoleDataPerm.GetRoleDataPermissionById(editid.Value);
                DataTable dtBnd = new DataView(dtRoleData, "Record_Type='B'", "", DataViewRowState.CurrentRows).ToTable();
                for (int C = 0; C < dtBnd.Rows.Count; C++)
                {
                    if (dtBnd.Rows.Count > 0)
                    {
                        BndIds += dtBnd.Rows[C]["Record_Id"].ToString() + ",";
                    }
                }
                dtBrand = new DataView(dtBrand, "Brand_Id in(" + BndIds.Substring(0, BndIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
            else
            {
                DataTable dtRoleData = objRoleDataPerm.GetRoleDataPermissionById(Session["RoleId"].ToString());
                DataTable dtBnd = new DataView(dtRoleData, "Record_Type='B'", "", DataViewRowState.CurrentRows).ToTable();
                for (int C = 0; C < dtBnd.Rows.Count; C++)
                {
                    if (dtBnd.Rows.Count > 0)
                    {
                        BndIds += dtBnd.Rows[C]["Record_Id"].ToString() + ",";
                    }
                }
                dtBrand = new DataView(dtBrand, "Brand_Id in(" + BndIds.Substring(0, BndIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        for (int i = 0; i < dtBrand.Rows.Count; i++)
        {
            if (BrandChecked.Split(',').Contains(dtBrand.Rows[i]["Brand_Id"].ToString()))
            {
                B2 += dtBrand.Rows[i]["Brand_Id"].ToString() + ",";
            }
        }
        ViewState["BrandChecked"] = B2;

        DataTable dtFinal = dtBrand.Clone();
        string selectedBrand = string.Empty;
        for (int i = 0; i < chkCompany.Items.Count; i++)
        {
            if (chkCompany.Items[i].Selected)
            {
                selectedBrand += chkCompany.Items[i].Value + ",";
            }
        }
        foreach (ListItem lst in chkCompany.Items)
        {
            if (lst.Selected)
            {
                dtFinal.Merge(new DataView(dtBrand, "Company_Id='" + lst.Value + "'", "", DataViewRowState.CurrentRows).ToTable());
            }
        }
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)chkBrand, dtFinal, "Brand_Name", "Brand_Id");

        ViewState["CompIds"] = CompanyIds;
        chkBrand_SelectedIndexChanged(null, null);
    }
    protected void chkBrand_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtLocation = new DataTable();
        List<int> List = new List<int>();
        string BIds = string.Empty;
        string LocIds = string.Empty;
        int BId = 0;
        string L2 = string.Empty;
        BrandChecked = ViewState["BrandChecked"].ToString();

        for (int i = 0; i < chkBrand.Items.Count; i++)
        {
            if (chkBrand.Items[i].Selected)
            {
                BId = Convert.ToInt32(chkBrand.Items[i].Value);
                //   chkBrand.Items.FindByValue(chkBrand.Items[i]. = true
                // List.Add(BId);
                BIds += BId + ",";
            }
        }
        if (BIds == "")
        {
            BrandChecked = BIds + BrandChecked;
        }
        else
        {
            BrandChecked = BIds;
        }

        for (int j = 0; j < BrandChecked.Split(',').Length - 1; j++)
        {
            try
            {
                chkBrand.Items.FindByValue(BrandChecked.Split(',')[j]).Selected = true;
            }
            catch
            {
            }
        }

        for (int i = 0; i < chkLocation.Items.Count; i++)
        {
            if (chkLocation.Items[i].Selected)
            {
                LocationChecked += chkLocation.Items[i].Value + ",";

            }
        }

        CompanyIds = ViewState["CompIds"].ToString();
        dtLocation = objLocation.GetLocationMaster(CompanyIds);
        if (Session["CompId"].ToString() == "0" || Session["RoleId"].ToString() == "0")
        {
        }
        else
        {
            if (editid.Value.ToString() != "")
            {
                DataTable dtRoleData = objRoleDataPerm.GetRoleDataPermissionById(editid.Value);
                DataTable dtLoc = new DataView(dtRoleData, "Record_Type='L'", "", DataViewRowState.CurrentRows).ToTable();
                for (int L = 0; L < dtLoc.Rows.Count; L++)
                {
                    if (dtLoc.Rows.Count > 0)
                    {
                        LocIds += dtLoc.Rows[L]["Record_Id"].ToString() + ",";
                    }
                }
                dtLocation = new DataView(dtLocation, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
            else
            {
                DataTable dtRoleData = objRoleDataPerm.GetRoleDataPermissionById(Session["RoleId"].ToString());
                DataTable dtLoc = new DataView(dtRoleData, "Record_Type='L'", "", DataViewRowState.CurrentRows).ToTable();
                for (int L = 0; L < dtLoc.Rows.Count; L++)
                {
                    if (dtLoc.Rows.Count > 0)
                    {
                        LocIds += dtLoc.Rows[L]["Record_Id"].ToString() + ",";
                    }
                }
                dtLocation = new DataView(dtLocation, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        try
        {
            dtLocation = new DataView(dtLocation, "Brand_Id in(" + BIds.Substring(0, BIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
            // dtLocation = new DataView(dtLocation, "Brand_Id='" + BrandChecked.Split(',') + "'", "", DataViewRowState.CurrentRows).ToTable();
        }

        for (int i = 0; i < dtLocation.Rows.Count; i++)
        {
            if (LocationChecked.Split(',').Contains(dtLocation.Rows[i]["Location_Id"].ToString()))
            {
                L2 += dtLocation.Rows[i]["Location_Id"].ToString() + ",";
            }
        }

        DataTable dtFinal = dtLocation.Clone();
        string selectedLocation = string.Empty;
        for (int i = 0; i < chkBrand.Items.Count; i++)
        {
            if (chkBrand.Items[i].Selected)
            {
                selectedLocation += chkBrand.Items[i].Value + ",";
            }
        }
        foreach (ListItem lst in chkBrand.Items)
        {
            if (lst.Selected)
            {
                dtFinal.Merge(new DataView(dtLocation, "Brand_Id='" + lst.Value + "'", "", DataViewRowState.CurrentRows).ToTable());
            }
        }
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)chkLocation, dtFinal, "Location_Name", "Location_Id");

        ViewState["LocationChecked"] = L2;
        ViewState["BrandChecked"] = "";
        //ViewState["LocationChecked"] = "";
        chkLocation_SelectedIndexChanged(null, null);
    }
    protected void chkLocation_SelectedIndexChanged(object sender, EventArgs e)
    {



        bindTreeDepartment();
        //chkDepartment.Items.Clear();

        //int LId = 0;
        //string LIds = string.Empty;

        //LocationChecked = ViewState["LocationChecked"].ToString();

        //for (int i = 0; i < chkLocation.Items.Count; i++)
        //{
        //    if (chkLocation.Items[i].Selected)
        //    {
        //        LId = Convert.ToInt32(chkLocation.Items[i].Value);
        //        //   chkBrand.Items.FindByValue(chkBrand.Items[i]. = true
        //        // List.Add(BId);
        //        LIds += LId + ",";
        //    }
        //}
        //if (LIds == "")
        //{
        //    //LocationChecked = LIds + LocationChecked;
        //    foreach (ListItem lst1 in chkDepartment.Items)
        //    {
        //        chkDepartment.Items.FindByValue(lst1.Value).Selected = false;
        //    }
        //}
        //else
        //{
        //    LocationChecked = LIds;


        //    for (int j = 0; j < LocationChecked.Split(',').Length - 1; j++)
        //    {
        //        try
        //        {
        //            chkLocation.Items.FindByValue(LocationChecked.Split(',')[j]).Selected = true;
        //        }
        //        catch
        //        {
        //            chkLocation.ClearSelection();
        //        }
        //    }
        //    DataTable dtLDempart = objLocDept.GetDepartmentLocation();
        //    DataTable dtFinal = dtLDempart.Clone();
        //    string selectedDept = string.Empty;
        //    for (int i = 0; i < chkLocation.Items.Count; i++)
        //    {
        //        if (chkLocation.Items[i].Selected)
        //        {
        //            selectedDept += chkLocation.Items[i].Value + ",";
        //        }
        //    }
        //    foreach (ListItem lst in chkLocation.Items)
        //    {
        //        if (lst.Selected)
        //        {
        //            dtFinal.Merge(new DataView(dtLDempart, "Location_Id='" + lst.Value + "'", "", DataViewRowState.CurrentRows).ToTable());
        //        }
        //    }
        //    DataTable dtFin = new DataTable();

        //    if (dtFinal.Rows.Count > 0)
        //    {

        //        dtFin = dtFinal.Clone();
        //        dtFin.Columns.Add("Dep_Name1");

        //        for (int i = 0; i < dtFinal.Rows.Count; i++)
        //        {
        //            dtFin.ImportRow(dtFinal.Rows[i]);
        //            dtFin.Rows[i]["Dep_Name1"] = GetDept(dtFin.Rows[i]["Trans_Id"].ToString());
        //        }
        //    }

        //    //Common Function add By Lokesh on 14-05-2015
        //    objPageCmn.FillData((object)chkDepartment, dtFin, "Dep_Name1", "Trans_Id");
        //    foreach (ListItem lst1 in chkDepartment.Items)
        //    {
        //        chkDepartment.Items.FindByValue(lst1.Value).Selected = true;
        //    }

        //    ViewState["LocationChecked"] = "";
        //    if (editid.Value != "")
        //    {
        //        if (Session["RoleId"].ToString() == editid.Value)
        //        {
        //        }
        //        else
        //        {

        //        }
        //    }
        //}
    }

    public void bindTreeDepartment()
    {
        DataTable dtRoleData = new DataTable();

        if (editid.Value != "")
        {
            dtRoleData = objRoleDataPerm.GetRoleDataPermissionById(editid.Value);


            dtRoleData = new DataView(dtRoleData, "Record_Type='D'", "", DataViewRowState.CurrentRows).ToTable();

        }

        DataTable dtRolePermission = new DataTable();
        DataTable dtLDempart = new DataTable();
        TreeViewDepartment.DataSource = null;
        TreeViewDepartment.DataBind();
        TreeViewDepartment.Nodes.Clear();

        for (int i = 0; i < chkLocation.Items.Count; i++)
        {
            if (chkLocation.Items[i].Selected)
            {
                TreeNode tn = new TreeNode(chkLocation.Items[i].Text, chkLocation.Items[i].Value);

                dtLDempart = objLocDept.GetDepartmentByLocationId(chkLocation.Items[i].Value);

                dtLDempart = new DataView(dtLDempart, "", "Dep_Name", DataViewRowState.CurrentRows).ToTable();

                for (int j = 0; j < dtLDempart.Rows.Count; j++)
                {
                    TreeNode tnchild = new TreeNode();
                    tnchild = new TreeNode(dtLDempart.Rows[j]["Dep_Name"].ToString(), dtLDempart.Rows[j]["Dep_Id"].ToString());


                    if (editid.Value != "")
                    {
                        dtRolePermission = new DataView(dtRoleData, "Field1=" + chkLocation.Items[i].Value + " and Record_Id=" + dtLDempart.Rows[j]["Dep_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

                        if (dtRolePermission.Rows.Count > 0)
                        {
                            tnchild.Checked = true;
                            tn.Checked = true;
                        }
                    }

                    tn.ChildNodes.Add(tnchild);

                }
                TreeViewDepartment.Nodes.Add(tn);
            }


        }
        TreeViewDepartment.DataBind();
        TreeViewDepartment.ExpandAll();


    }

    public void BindTree()
    {
        DataTable dtLoginUserPermission = new DataTable();

        if (Session["EmpId"].ToString() == "0")
        {
            dtLoginUserPermission = cmn.GetAccodion(Session["LoginCompany"].ToString(), "superadmin", Session["Application_Id"].ToString(), "true");
        }
        else
        {
            dtLoginUserPermission = cmn.GetAccodion(Session["LoginCompany"].ToString(), Session["UserId"].ToString(), Session["Application_Id"].ToString(), "true");

        }



        string AppId = string.Empty;
        DataTable dtApp = objSys.GetSysParameterByParamName("Application_Id");
        if (dtApp.Rows.Count > 0)
        {
            AppId = dtApp.Rows[0]["Param_Value"].ToString().Trim();

        }
        else
        {
            return;
        }



        string RoleId = string.Empty;
        string moduleids = string.Empty;

        navTree.DataSource = null;
        navTree.DataBind();
        navTree.Nodes.Clear();

        DataTable dtRoleP = objRolePermission.GetRolePermissionById(editid.Value);
        //here we get module parentd Id
        DataTable DtModuleParentId = dtLoginUserPermission.DefaultView.ToTable(true, "ParentId", "ParentModuleName");
        DataTable DtOpType = ObjItObject.GetOpType();
        DataTable Dt_OpType = new DataTable();
        foreach (DataRow dataParentrow in DtModuleParentId.Rows)
        {

            TreeNode tnParent = new TreeNode(dataParentrow["ParentModuleName"].ToString(), dataParentrow["ParentId"].ToString());
            tnParent.SelectAction = TreeNodeSelectAction.Expand;

            tnParent.ShowCheckBox = false;
            DataTable DtModuleApp = dtLoginUserPermission.DefaultView.ToTable(true, "Module_Id", "Module_Name", "ParentId");


            try
            {
                DtModuleApp = new DataView(DtModuleApp, "Module_Id=" + dataParentrow["ParentId"].ToString() + " or ParentId='" + dataParentrow["ParentId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {

            }






            foreach (DataRow datarow in DtModuleApp.Rows)
            {
                TreeNode tn = new TreeNode();
                tn = new TreeNode(datarow["Module_Name"].ToString(), datarow["Module_Id"].ToString());
                tn.SelectAction = TreeNodeSelectAction.Expand;
                if (dtRoleP.Rows.Count > 0)
                {
                    DataTable dtModuleSaved = new DataView(dtRoleP, "Module_Id='" + datarow["Module_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                    if (dtModuleSaved.Rows.Count > 0)
                    {
                        tn.Checked = true;
                    }
                }

                tnParent.ChildNodes.Add(tn);

                DataTable dtAllChild = new DataView(dtLoginUserPermission, "Module_Id=" + datarow["Module_Id"].ToString() + "", "Order_Appear", DataViewRowState.CurrentRows).ToTable();




                foreach (DataRow childrow in dtAllChild.Rows)
                {
                    //DtOpType = ObjItObject.GetOpType();
                    string GetUrl = string.Empty;
                    GetUrl = childrow["Object_Id"].ToString();

                    TreeNode tnChild = new TreeNode(childrow["Object_Name"].ToString(), GetUrl);
                    tnChild.SelectAction = TreeNodeSelectAction.Expand;
                    if (dtRoleP.Rows.Count > 0)
                    {
                        DataTable dtObj = new DataView(dtRoleP, "Object_Id='" + childrow["Object_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dtObj.Rows.Count > 0)
                        {
                            tnChild.Checked = true;
                        }
                    }

                    tn.ChildNodes.Add(tnChild);

                    //this code is created by jitendra upadhyay on 05-09-2014
                    //this code for get the record fro it_app_op_permission for showing specific operation according the object id and module id 
                    //code start
                    DataTable DtopPermission = obj_OP_Permission.GetRecord(childrow["Object_Id"].ToString());
                    if (DtopPermission.Rows.Count > 0)
                    {
                        string Object_Opeartion = string.Empty;
                        foreach (DataRow dr_op in DtopPermission.Rows)
                        {
                            if (Object_Opeartion == "")
                            {
                                Object_Opeartion = dr_op["Op_Id"].ToString();
                            }
                            else
                            {
                                Object_Opeartion = Object_Opeartion + "," + dr_op["Op_Id"].ToString();
                            }
                        }

                        try
                        {
                            Dt_OpType = new DataView(DtOpType, "Op_id in (" + Object_Opeartion + ")", "", DataViewRowState.CurrentRows).ToTable();
                        }
                        catch
                        {

                        }
                        foreach (DataRow drOpType in Dt_OpType.Rows)
                        {
                            TreeNode tnOpType = new TreeNode(drOpType[1].ToString(), drOpType[0].ToString());

                            //this code is modify by the jitendra on 17-10-2014
                            //this code update according the database modification
                            DataTable dtOp = new DataView(dtRoleP, "Op_Id=" + drOpType["Op_Id"].ToString() + " and Object_Id='" + GetUrl + "'", "", DataViewRowState.CurrentRows).ToTable();

                            if (dtOp.Rows.Count > 0)
                            {
                                tnOpType.Checked = true;
                            }
                            tnOpType.SelectAction = TreeNodeSelectAction.Expand;
                            tnChild.ChildNodes.Add(tnOpType);


                        }
                    }
                    //code end
                }

            }
            navTree.Nodes.Add(tnParent);
        }


        navTree.DataBind();
        navTree.CollapseAll();

        return;
    }

    public void SaveCompBrandLocDept(string RoleId)
    {

        foreach (ListItem item in chkCompany.Items)
        {
            if (item.Selected)
            {
                objRoleDataPerm.InsertRoleDataPermission(RoleId, Session["CompId"].ToString(), "C", item.Value, "0", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
        }

        foreach (ListItem item in chkBrand.Items)
        {
            if (item.Selected)
            {
                objRoleDataPerm.InsertRoleDataPermission(RoleId, Session["CompId"].ToString(), "B", item.Value, "0", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
        }
        foreach (ListItem item in chkLocation.Items)
        {
            if (item.Selected)
            {
                objRoleDataPerm.InsertRoleDataPermission(RoleId, Session["CompId"].ToString(), "L", item.Value, "0", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
        }

        foreach (TreeNode tn in TreeViewDepartment.Nodes)
        {
            foreach (TreeNode tnchildnode in tn.ChildNodes)
            {
                if (tnchildnode.Checked)
                {
                    objRoleDataPerm.InsertRoleDataPermission(RoleId, Session["CompId"].ToString(), "D", tnchildnode.Value, tn.Value, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }
            }

        }


        //foreach (ListItem item in chkDepartment.Items)
        //{
        //    if (item.Selected)
        //    {
        //        objRoleDataPerm.InsertRoleDataPermission(RoleId, Session["CompId"].ToString(), "D", item.Value, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        //    }
        //}
    }
    public string GetBrandIdByLocationId(string LocationId)
    {
        string BrandId = string.Empty;

        DataTable dtLocation = objLocation.GetLocationMasterById(Session["CompId"].ToString(), LocationId);
        if (dtLocation.Rows.Count > 0)
        {
            BrandId = dtLocation.Rows[0]["BrandId"].ToString().Trim();
        }
        return BrandId;
    }
    public string GetLocationIdByDepartmentId(string DeptId)
    {
        string LocationId = string.Empty;
        DataTable dtDept = objDepartment.GetDepartmentMasterById(DeptId);
        if (dtDept.Rows.Count > 0)
        {
            LocationId = dtDept.Rows[0]["Location_Id"].ToString().Trim();
        }
        return LocationId;
    }
    public string GetDept(string TransId)
    {
        string retval = "";
        string LocationName = "";

        DataTable dtDept = objLocDept.GetDepartmentLocation();

        dtDept = new DataView(dtDept, "Trans_Id='" + TransId + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtDept.Rows.Count > 0)
        {
            LocationName = GetLocation(dtDept.Rows[0]["Location_Id"], dtDept.Rows[0]["Company_Id"]);
            retval = dtDept.Rows[0]["Dep_Name"].ToString() + "," + LocationName;
        }
        return retval;
    }
    protected string GetLocation(object obj, object CompId)
    {
        string retval = string.Empty;
        try
        {
            retval = (objLocation.GetLocationMasterById(CompId.ToString(), obj.ToString())).Rows[0]["Location_Name"].ToString();
        }
        catch (Exception)
        {
            return "";
        }
        return retval;
    }
    protected void ChkSelectAll_OnCheckedChanged(object sender, EventArgs e)
    {
        BindTree();
        if (chkSelectAll.Checked)
        {
            foreach (TreeNode Tn in navTree.Nodes)
            {
                SelectChild(Tn);
            }
        }
        else
        {
            foreach (TreeNode Tn in navTree.Nodes)
            {
                UnSelectChild(Tn);
            }
        }
    }
    public void FillDepartment()
    {
        //DataTable dtDepartment = new DataTable();
        //string DepIds = string.Empty;
        //dtDepartment = objLocDept.GetDepartmentLocation();
        //dtDepartment = new DataView(dtDepartment, "Company_Id='" + Session["CompId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //if (Session["CompId"].ToString() == "0" || Session["RoleId"].ToString() == "0")
        //{
        //}
        //else
        //{
        //    if (editid.Value.ToString() != "")
        //    {
        //        DataTable dtRoleData = objRoleDataPerm.GetRoleDataPermissionById(editid.Value);
        //        DataTable dtDep = new DataView(dtRoleData, "Record_Type='D'", "", DataViewRowState.CurrentRows).ToTable();
        //        for (int D = 0; D < dtDep.Rows.Count; D++)
        //        {
        //            if (dtDep.Rows.Count > 0)
        //            {
        //                DepIds += dtDep.Rows[D]["Record_Id"].ToString() + ",";
        //            }
        //        }
        //        try
        //        {
        //            dtDepartment = new DataView(dtDepartment, "Company_Id in(" + DepIds.Substring(0, DepIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        //        }
        //        catch
        //        {

        //        }
        //    }
        //}
        //if (dtDepartment.Rows.Count > 0)
        //{
        //    //Common Function add By Lokesh on 14-05-2015
        //    objPageCmn.FillData((object)chkDepartment, dtDepartment, "Dep_Name", "Trans_Id");
        //    foreach (ListItem lst1 in chkDepartment.Items)
        //    {
        //        chkDepartment.Items.FindByValue(lst1.Value).Text = GetDept(lst1.Value);
        //        chkDepartment.Items.FindByValue(lst1.Value).Selected = true;
        //    }
        //}
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        btnSave.Visible = clsPagePermission.bAdd;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        ImgbtnSelectAll.Visible = clsPagePermission.bRestore;
    }
    private void UnSelectChild(TreeNode treeNode)
    {
        int i = 0;
        treeNode.Checked = false;
        while (i < treeNode.ChildNodes.Count)
        {
            treeNode.ChildNodes[i].Checked = false;
            UnSelectChild(treeNode.ChildNodes[i]);
            i++;
        }
        navTree.DataBind();
    }
    private void SelectChild(TreeNode treeNode)
    {
        int i = 0;
        treeNode.Checked = true;
        while (i < treeNode.ChildNodes.Count)
        {
            treeNode.ChildNodes[i].Checked = true;
            SelectChild(treeNode.ChildNodes[i]);
            i++;
        }
        try
        {
            treeNode.Parent.Checked = true;
            treeNode.Parent.Parent.Checked = true;
        }
        catch { }

        navTree.DataBind();
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        txtbinValue.Focus();
        FillGridBin();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        FillGridBin();
        Session["CHECKED_ITEMS"] = null;
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtValue.Text = "";
    }
    protected void btnbind_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
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
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + txtValue.Text.Trim() + "%'";
            }
            DataTable dtCust = (DataTable)Session["Role"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_Role_Mstr"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)gvRoleMaster, view.ToTable(), "", "");
            //AllPageCode();
        }
        txtValue.Focus();
    }
    protected void btnbinbind_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

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
            DataTable dtCust = (DataTable)Session["dtbinRole"];

            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtbinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)gvRoleMasterBin, view.ToTable(), "", "");

            if (view.ToTable().Rows.Count == 0)
            {
                imgBtnRestore.Visible = false;
                ImgbtnSelectAll.Visible = false;
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
        //FillGrid();
        I2.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        FillGridBin();

        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 0;
        txtbinValue.Text = "";
        lblSelectedRecord.Text = "";
    }
    private void SaveCheckedValuesemplog()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in gvRoleMasterBin.Rows)
        {
            index = (int)gvRoleMasterBin.DataKeys[gvrow.RowIndex].Value;
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
            foreach (GridViewRow gvrow in gvRoleMasterBin.Rows)
            {
                int index = (int)gvRoleMasterBin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }

    protected void gvRoleMasterBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValuesemplog();
        gvRoleMasterBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtbinFilter"];
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvRoleMasterBin, dt, "", "");
        //AllPageCode();
        PopulateCheckedValuesemplog();
    }
    protected void gvRoleMasterBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objRole.GetRoleMasterInactive(Session["CompId"].ToString());
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtbinFilter"] = dt;
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvRoleMasterBin, dt, "", "");
        //AllPageCode();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        int b = 0;
        if (txtRoleName.Text == "")
        {
            DisplayMessage("Enter Role Name");
            txtRoleName.Focus();
            return;
        }
        if (editid.Value == "")
        {
            DataTable dt1 = objRole.GetRoleMaster(Session["CompId"].ToString());

            dt1 = new DataView(dt1, "Role_Name='" + txtRoleName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                DisplayMessage("Role Name Already Exists");
                txtRoleName.Focus();
                return;
            }

            b = objRole.InsertRoleMaster(Session["CompId"].ToString(), txtRoleName.Text, txtRoleNameL.Text, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            if (b != 0)
            {
                //DisplayMessage("Record Saved","green");
                FillGrid();
                btnSave.Text = "Save";
                editid.Value = b.ToString();
                btnSavePerm_Click(null, null);
                btnReset_Click(null, null);

            }
            else
            {
                DisplayMessage("Record Not Saved");
            }
        }
        else
        {
            DataTable dt1 = objRole.GetRoleMaster(Session["CompId"].ToString());
            string RoleName = string.Empty;

            try
            {
                RoleName = (new DataView(dt1, "Role_Id='" + editid.Value + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Role_Name"].ToString();
            }
            catch
            {
                RoleName = "";
            }



            dt1 = new DataView(dt1, "Role_Name='" + txtRoleName.Text + "' and Role_Name<>'" + RoleName + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                DisplayMessage("Role Name Already Exists");
                txtRoleName.Focus();
                return;

            }
            b = objRole.UpdateRoleMaster(Session["CompId"].ToString(), editid.Value, txtRoleName.Text, txtRoleNameL.Text, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            if (b != 0)
            {

                DisplayMessage("Record Saved","green");
                FillGrid();
                //FillGridBin();
                Div_Rol_Permission.Style.Add("display", "");
                btnSavePerm_Click(null, null);
                //objRoleDataPerm.DeleteRoleDataPermission(editid.Value);
                //SaveCompBrandLocDept(editid.Value);

                btnReset_Click(null, null);

                //btnList_Click(null, null);
                Lbl_New_tab.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            }
            else
            {
                DisplayMessage("Record Not Updated");
            }

        }
    }


    protected void btnSavePerm_Click(object sender, EventArgs e)
    {
        try
        {
            if (navTree.SelectedNode.Checked == true)
            {
                UnSelectChild(navTree.SelectedNode);
            }
            else
            {
                SelectChild(navTree.SelectedNode);
            }
        }
        catch (Exception)
        {

        }

        objRolePermission.DeleteRolePermission(editid.Value);

        foreach (TreeNode ParentNode in navTree.Nodes)
        {

            foreach (TreeNode ModuleNode in ParentNode.ChildNodes)
            {
                //here save one row for module
                if (ModuleNode.Checked)
                {


                    foreach (TreeNode ObjNode in ModuleNode.ChildNodes)
                    {
                        if (ObjNode.Checked)
                        {


                            int refid1 = 0;

                            refid1 = objRolePermission.InsertRolePermission(editid.Value, ModuleNode.Value, ObjNode.Value, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                            string refid = refid1.ToString();
                            //this code is modify by jitendra upadhyay on 25-09-2014
                            //this code for delete the all permission using ref id and insert single row in user_oppermission
                            //code start
                            objRolePermission.DeleteRoleOpPermission_By_RefId(refid.ToString());

                            //code end


                            foreach (TreeNode OpNode in ObjNode.ChildNodes)
                            {
                                if (OpNode.Checked == true)
                                {
                                    objRolePermission.InsertRoleOpPermission(refid, OpNode.Value);
                                }


                            }

                        }
                    }
                }
            }
        }
        DisplayMessage("Record Saved","green");
    }
    protected void btnCancelPerm_Click(object sender, EventArgs e)
    {
       // btnList_Click(null, null);
        Reset();
        FillGrid();
        FillGridBin();

        Div_Rol_Permission.Style.Add("display", "");
        //PnlRole.Visible = true;
    }
    protected void btnView_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        ViewState["View"] = "1";
        BindEditView();
        btnReset.Visible = false;
        btnSave.Visible = false;
        chkCompany.Enabled = false;
        chkBrand.Enabled = false;
        chkLocation.Enabled = false;
        //chkDepartment.Enabled = false;
        txtRoleName.Enabled = false;
        txtRoleNameL.Enabled = false;

        Lbl_New_tab.Text = Resources.Attendance.View.ToString();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "on_View_tab_position()", true);


    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        RoleFormFieldsEnableTRUE();
        editid.Value = e.CommandArgument.ToString();
        string EditType = e.CommandName.ToString();

        //if (Session["RoleId"].ToString() == editid.Value)
        //{
        //    DisplayMessage("You Can Not Edit This Role");
        //    return;
        //}
        //else
        //{
        BindEditView();
        //}
        Lbl_New_tab.Text = Resources.Attendance.Edit.ToString();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "on_View_tab_position()", true);
    }

    private void RoleFormFieldsEnableTRUE()
    {
        btnReset.Visible = true;
        btnSave.Visible = true;
        btnSave.Visible = true;
        chkCompany.Enabled = true;
        chkBrand.Enabled = true;
        chkLocation.Enabled = true;
        //chkDepartment.Enabled = true;
        txtRoleName.Enabled = true;
        txtRoleNameL.Enabled = true;
    }
    private void BindEditView()
    {
        string SelectedComp = string.Empty;
        string SelectedBrand = string.Empty;
        string SelectedLocation = string.Empty;
        string SelectedDepartment = string.Empty;
        DataTable dt = objRole.GetRoleMasterById(Session["CompId"].ToString(), editid.Value);
        if (dt.Rows.Count > 0)
        {
            txtRoleName.Text = dt.Rows[0]["Role_Name"].ToString();
            txtRoleNameL.Text = dt.Rows[0]["Role_Name_L"].ToString();
            btnSave.Text = "Save";
            //FillCompany();
            //FillBrand();
            //FillLocation();
            //FillDepartment();


            //DataTable dtRoleData = objRoleDataPerm.GetRoleDataPermissionById(editid.Value);

            //if (dtRoleData.Rows.Count > 0)
            //{
            //    // Company Master .............

            //    DataTable dtComp = new DataView(dtRoleData, "Record_Type='C'", "", DataViewRowState.CurrentRows).ToTable();

            //    if (dtComp.Rows.Count > 0)
            //    {


            //        foreach (DataRow dr in dtComp.Rows)
            //        {
            //            SelectedComp += dr["Record_Id"] + ",";
            //            ViewState["CompIds"] = SelectedComp;
            //        }

            //        for (int j = 0; j < SelectedComp.Split(',').Length; j++)
            //        {
            //            if (SelectedComp.Split(',')[j] != "")
            //            {
            //                try
            //                {
            //                    chkCompany.Items.FindByValue(SelectedComp.Split(',')[j]).Selected = true;
            //                }
            //                catch (Exception ex)
            //                {

            //                    continue;
            //                }
            //            }
            //        }
            //    }

            //    ///...........................


            //    chkCompany_SelectedIndexChanged(null, null);




            //    DataTable dtBrand = new DataView(dtRoleData, "Record_Type='B'", "", DataViewRowState.CurrentRows).ToTable();

            //    if (dtBrand.Rows.Count > 0)
            //    {


            //        foreach (DataRow dr in dtBrand.Rows)
            //        {
            //            SelectedBrand += dr["Record_Id"] + ",";
            //            ViewState["BrandChecked"] = SelectedBrand;
            //        }

            //        for (int j = 0; j < SelectedBrand.Split(',').Length; j++)
            //        {
            //            if (SelectedBrand.Split(',')[j] != "")
            //            {
            //                try
            //                {
            //                    chkBrand.Items.FindByValue(SelectedBrand.Split(',')[j]).Selected = true;
            //                }
            //                catch (Exception ex)
            //                {

            //                    continue;
            //                }
            //            }
            //        }
            //    }
            //    chkBrand_SelectedIndexChanged(null, null);

            //    foreach (ListItem lst1 in chkLocation.Items)
            //    {

            //        chkLocation.Items.FindByValue(lst1.Value).Selected = false;

            //    }

            //    DataTable dtLocation = new DataView(dtRoleData, "Record_Type='L'", "", DataViewRowState.CurrentRows).ToTable();

            //    if (dtLocation.Rows.Count > 0)
            //    {
            //        foreach (DataRow dr in dtLocation.Rows)
            //        {
            //            SelectedLocation += dr["Record_Id"] + ",";
            //            ViewState["LocationChecked"] = SelectedLocation;
            //        }

            //        for (int j = 0; j < SelectedLocation.Split(',').Length; j++)
            //        {
            //            if (SelectedLocation.Split(',')[j] != "")
            //            {
            //                try
            //                {
            //                    chkLocation.Items.FindByValue(SelectedLocation.Split(',')[j]).Selected = true;
            //                }
            //                catch (Exception ex)
            //                {

            //                    continue;
            //                }
            //            }
            //        }
            //    }
            //chkLocation_SelectedIndexChanged(null, null);
            //bindTreeDepartment();


            //foreach (ListItem lst1 in chkDepartment.Items)
            //{

            //    chkDepartment.Items.FindByValue(lst1.Value).Selected = false;

            //}
            //DataTable dtDepartment = new DataView(dtRoleData, "Record_Type='D'", "", DataViewRowState.CurrentRows).ToTable();
            //if (dtDepartment.Rows.Count > 0)
            //{
            //    foreach (DataRow dr in dtDepartment.Rows)
            //    {
            //        SelectedDepartment += dr["Record_Id"] + ",";
            //    }

            //    for (int j = 0; j < SelectedDepartment.Split(',').Length; j++)
            //    {
            //        if (SelectedDepartment.Split(',')[j] != "")
            //        {
            //            try
            //            {
            //                chkDepartment.Items.FindByValue(SelectedDepartment.Split(',')[j]).Selected = true;
            //            }
            //            catch (Exception ex)
            //            {

            //                continue;
            //            }
            //        }
            //    }
            //}
            //}
           // btnNew_Click(null, null);
            Lbl_New_tab.Text = Resources.Attendance.Edit;
        }
        Div_Rol_Permission.Style.Add("display", "");
        chkSelectAll.Checked = true;
        BindTree();
    }

    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        int b = 0;
        b = objRole.DeleteRoleMaster(Session["CompId"].ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b == -11)
        {
            DisplayMessage("Role already in use,so can not delete");
            FillGridBin();
            FillGrid();
            Reset();
        }
        else if (b > 0)
        {
            DisplayMessage("Record deleted");
            FillGridBin();
            FillGrid();
            Reset();
        }
        else
        {
            DisplayMessage("Record Not Deleted");
        }
    }
    protected void gvRoleMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvRoleMaster.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Role_Mstr"];
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvRoleMaster, dt, "", "");
        //AllPageCode();
    }
    protected void gvRoleMaster_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Role_Mstr"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Role_Mstr"] = dt;
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvRoleMaster, dt, "", "");
        //AllPageCode();
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListRoleCode(string prefixText, int count, string contextKey)
    {
        RoleMaster objRoleMaster = new RoleMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objRoleMaster.GetRoleMaster(HttpContext.Current.Session["CompId"].ToString()), "Role_Code like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Role_Code"].ToString();
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListRoleName(string prefixText, int count, string contextKey)
    {
        RoleMaster objRoleMaster = new RoleMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objRoleMaster.GetRoleMaster(HttpContext.Current.Session["CompId"].ToString()), "Role_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Role_Name"].ToString();
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
        chkCompany.Items.Clear();
        chkBrand.Items.Clear();
        chkLocation.Items.Clear();
        //chkDepartment.Items.Clear();
        FillCompany();
        chkCompany.Enabled = true;
        chkBrand.Enabled = true;
        chkLocation.Enabled = true;
        //chkDepartment.Enabled = true;
        btnReset.Visible = true;
        txtRoleName.Enabled = true;
        txtRoleNameL.Enabled = true;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        FillGrid();
        FillGridBin();
        Reset();
        //btnList_Click(null, null);
        chkCompany.Items.Clear();
        chkBrand.Items.Clear();
        chkLocation.Items.Clear();
        //chkDepartment.Items.Clear();
        FillCompany();
        chkCompany.Enabled = true;
        chkBrand.Enabled = true;
        chkLocation.Enabled = true;
        //chkDepartment.Enabled = true;
        btnReset.Visible = true;
        txtRoleName.Enabled = true;
        txtRoleNameL.Enabled = true;
        Lbl_New_tab.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }

    public void FillGrid()
    {
        DataTable dt = objRole.GetRoleMaster(Session["CompId"].ToString());
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvRoleMaster, dt, "", "");
        //AllPageCode();
        Session["dtFilter_Role_Mstr"] = dt;
        Session["Role"] = dt;
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
    }

    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objRole.GetRoleMasterInactive(Session["CompId"].ToString());
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvRoleMasterBin, dt, "", "");
        Session["dtbinFilter"] = dt;
        Session["dtbinRole"] = dt;
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        if (dt.Rows.Count == 0)
        {
            imgBtnRestore.Visible = false;
            ImgbtnSelectAll.Visible = false;
        }
        else
        {

            //AllPageCode();
        }

    }


    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvRoleMasterBin.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in gvRoleMasterBin.Rows)
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
    protected void ImgbtnSelectAll_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

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

                if (!userdetails.Contains(dr["Role_Id"]))
                {
                    userdetails.Add(dr["Role_Id"]);
                }
            }
            foreach (GridViewRow GR in gvRoleMasterBin.Rows)
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
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)gvRoleMasterBin, dtUnit1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        int b = 0;
        ArrayList userdetail = new ArrayList();
        SaveCheckedValuesemplog();
        userdetail = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetail.Count > 0)
        {
            for (int j = 0; j < userdetail.Count; j++)
            {
                if (userdetail[j].ToString() != "")
                {
                    b = objRole.DeleteRoleMaster(Session["CompId"].ToString(), userdetail[j].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
        }
        else
        {
            int fleg = 0;
            foreach (GridViewRow Gvr in gvRoleMasterBin.Rows)
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

    public void Reset()
    {
        Session["CHECKED_ITEMS"] = null;
        txtRoleName.Text = "";
        txtRoleNameL.Text = "";
        btnSave.Text = "Save";
        navTree.DataSource = null;
        navTree.DataBind();
        navTree.Nodes.Clear();
        Lbl_New_tab.Text = Resources.Attendance.New;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        editid.Value = "";
        txtValue.Text = "";
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        Div_Rol_Permission.Style.Add("display", "");
        FillBrand();
        FillLocation();
        FillDepartment();
        BindTree();
        chkSelectAll.Checked = false;
    }


}
