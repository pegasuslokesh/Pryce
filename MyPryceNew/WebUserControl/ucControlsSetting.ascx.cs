using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebUserControl_ucControlsSetting : System.Web.UI.UserControl
{
    PageControlsSetting objPageCtlSetting = null;
    public delegate void parentPageHandler(Object obj, EventArgs e);
    public event parentPageHandler refreshControlsFromChild;
    protected void Page_Load(object sender, EventArgs e)
    {
        objPageCtlSetting = new PageControlsSetting(Session["DBConnection"].ToString());
        
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = new List<PageControlsSetting.clsContorlsSetting> { };
        if(dlGrdCols.Visible==true)
        {
            foreach(DataListItem dr in dlGrdCols.Items)
            {
                PageControlsSetting.clsContorlsSetting cls = new PageControlsSetting.clsContorlsSetting();
                cls.PageName = hdnPageName.Value;
                cls.ControlId = ((HiddenField)dr.FindControl("hdnColIndex")).Value;
                cls.Caption = ((CheckBox)dr.FindControl("chkGrdColVisibility")).Text;
                cls.IsVisible = ((CheckBox)dr.FindControl("chkGrdColVisibility")).Checked;
                cls.IsMandatory = false;
                cls.IsGridColumn = true;
                cls.ContainerId = hdnGrdName.Value;
                cls.IsActive = true;
                cls.ModifiedBy = Session["UserId"].ToString();
                cls.ModifiedDate = DateTime.Now;
                lstCls.Add(cls);
            }
        }
        else
        {
            foreach (DataListItem dr in dlPageControls.Items)
            {
                PageControlsSetting.clsContorlsSetting cls = new PageControlsSetting.clsContorlsSetting();
                cls.PageName = hdnPageName.Value;
                cls.ControlId = ((HiddenField)dr.FindControl("hdnCtlName")).Value;
                cls.Caption = ((Label)dr.FindControl("lblCtlCaption")).Text;
                cls.IsVisible = ((CheckBox)dr.FindControl("chkCtlVisibility")).Checked;
                cls.IsMandatory = ((CheckBox)dr.FindControl("chkCtlMandatory")).Checked;
                cls.IsGridColumn = false;
                cls.ContainerId = ((HiddenField)dr.FindControl("hdnCtlContainer")).Value;
                cls.MandatoryControlId= ((HiddenField)dr.FindControl("hdnMandatoryControlId")).Value;
                cls.IsActive = true;
                cls.ModifiedBy = Session["UserId"].ToString();
                cls.ModifiedDate = DateTime.Now;
                lstCls.Add(cls);
            }
        }
        if(lstCls.Count>0)
        {
            objPageCtlSetting.updateControlSetting(hdnPageName.Value, dlGrdCols.Visible, lstCls);
            if (dlGrdCols.Visible == false)
            {
                objPageCtlSetting.setControlsVisibility(lstCls, this.Page);
            }
            else
            {
                try
                {
                    GridView _gv = (GridView)this.Page.FindControl(lstCls[0].ContainerId);
                    objPageCtlSetting.setColumnsVisibility(_gv, lstCls);
                }
                catch
                {

                }
                try
                {
                    DevExpress.Web.ASPxGridView _gv = (DevExpress.Web.ASPxGridView)this.Page.FindControl(lstCls[0].ContainerId);
                    objPageCtlSetting.setColumnsVisibility(_gv, lstCls);
                }
                catch
                {

                }
            }
            if (this.refreshControlsFromChild!=null)
            {
                refreshControlsFromChild(this,new EventArgs());
            }
            string strMessag = dlGrdCols.Visible == true ? "Table Columns visibility setting has been changed successfully" : "Contros setting has been changed successfully";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "alert('" + strMessag + "'),hidePopup()", true);
        }        
    }

    public void getPageControlsSetting(string strPageName, List<PageControlsSetting.clsContorlsSetting> lstCls)
    {
        try
        {
            hdnPageName.Value = strPageName;
            hdnGrdName.Value = string.Empty;
            dlPageControls.Visible = true;
            dlGrdCols.Visible = false;
            lstCls = lstCls.Where(w => w.IsGridColumn == false).ToList();
            if (lstCls.Count > 0)
            {
                dlPageControls.DataSource = lstCls;
                dlPageControls.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void getGrdColumnsSettings(string strPageName,GridView grd, int defaultColCount)
    {
        try
        {
            hdnPageName.Value = strPageName;
            hdnGrdName.Value = grd.UniqueID;
            dlGrdCols.Visible = true;
            dlPageControls.Visible = false;
            List<PageControlsSetting.clsContorlsSetting> lstClsCtlSetting = new List<PageControlsSetting.clsContorlsSetting> { };
            for (int i = defaultColCount - 1; i < grd.Columns.Count; i++)
            {
                PageControlsSetting.clsContorlsSetting cls = new PageControlsSetting.clsContorlsSetting();
                cls.Caption = grd.Columns[i].HeaderText;
                cls.IsVisible = grd.Columns[i].Visible == true ? true : false;
                cls.ControlId = i.ToString();
                lstClsCtlSetting.Add(cls);
            }
            if (lstClsCtlSetting.Count > 0)
            {
                dlGrdCols.DataSource = lstClsCtlSetting;
                dlGrdCols.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void getGrdColumnsSettings(string strPageName, DevExpress.Web.ASPxGridView grd, int defaultColCount)
    {
        try
        {
            hdnPageName.Value = strPageName;
            hdnGrdName.Value = grd.UniqueID;
            dlGrdCols.Visible = true;
            dlPageControls.Visible = false;
            List<PageControlsSetting.clsContorlsSetting> lstClsCtlSetting = new List<PageControlsSetting.clsContorlsSetting> { };
            for (int i = defaultColCount - 1; i < grd.Columns.Count; i++)
            {
                PageControlsSetting.clsContorlsSetting cls = new PageControlsSetting.clsContorlsSetting();
                cls.Caption = grd.Columns[i].Caption;
                cls.IsVisible = grd.Columns[i].Visible == true ? true : false;
                cls.ControlId = i.ToString();
                lstClsCtlSetting.Add(cls);
            }
            if (lstClsCtlSetting.Count > 0)
            {
                dlGrdCols.DataSource = lstClsCtlSetting;
                dlGrdCols.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
    }

}