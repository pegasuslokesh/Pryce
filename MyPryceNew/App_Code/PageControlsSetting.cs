using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PegasusDataAccess;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;


/// <summary>
/// Summary description for PageControlsSetting
/// </summary>
public class PageControlsSetting
{
    DataAccessClass objDa = null;
    public PageControlsSetting(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        objDa = new DataAccessClass(strConString);
    }
    public class clsContorlsSetting
    {
        public string PageName { get; set; }
        public string ControlId { get; set; }
        public bool IsVisible { get; set; }
        public bool IsMandatory { get; set; }
        public string ContainerId { get; set; }
        public bool IsGridColumn { get; set; }
        public string Caption { get; set; }
        public string MandatoryControlId { get; set; }
        public bool IsActive { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public List<clsContorlsSetting> getControlsSetting(string strPageName)
    {
        clsContorlsSetting cls = new clsContorlsSetting();
        List<clsContorlsSetting> lstObject = new List<clsContorlsSetting> { };
        using (DataTable dtControlSettings = objDa.return_DataTable("select * from Set_PageControlsSetting where PageName='" + strPageName + "'"))
        {
            foreach (DataRow dr in dtControlSettings.Rows)
            {
                cls = new clsContorlsSetting();
                cls.ControlId = dr["ControlId"].ToString();
                cls.IsVisible = bool.Parse(dr["IsVisible"].ToString());
                cls.IsMandatory = bool.Parse(dr["IsMandatory"].ToString());
                cls.ContainerId = dr["ContainerId"].ToString();
                cls.Caption = dr["Caption"].ToString();
                cls.IsGridColumn = bool.Parse(dr["IsGridColumn"].ToString());
                cls.MandatoryControlId = dr["MandatoryControlId"].ToString();
                lstObject.Add(cls);
            }
        }
        return lstObject;
    }

    public List<clsContorlsSetting> getMandatoryControls(string strPageName)
    {
        clsContorlsSetting cls = new clsContorlsSetting();
        List<clsContorlsSetting> lstObject = new List<clsContorlsSetting> { };
        using (DataTable dtControlSettings = objDa.return_DataTable("select * from Set_PageControlsSetting where PageName='" + strPageName + "' and IsGridColumn='false' and IsVisible='true' and IsMandatory='true'"))
        {
            foreach (DataRow dr in dtControlSettings.Rows)
            {
                cls = new clsContorlsSetting();
                cls.IsMandatory = bool.Parse(dr["IsMandatory"].ToString());
                cls.MandatoryControlId = dr["MandatoryControlId"].ToString();
                cls.Caption = dr["Caption"].ToString();
                lstObject.Add(cls);
            }
        }
        return lstObject;
    }

    public void updateControlSetting(string strPageName, bool isGridColumn, List<clsContorlsSetting> lstCls)
    {
        string strSql = "delete from Set_PageControlsSetting where PageName='" + strPageName + "' and IsGridColumn='" + isGridColumn + "'";
        objDa.execute_Command(strSql);
        foreach (clsContorlsSetting cls in lstCls)
        {
            strSql = "insert into Set_PageControlsSetting(PageName,ControlId,Caption,IsVisible,IsMandatory,ContainerId,IsGridColumn,MandatoryControlId,IsActive,ModifiedBy,ModifiedDate)";
            strSql = strSql + " values('" + cls.PageName + "','" + cls.ControlId + "','" + cls.Caption + "','" + cls.IsVisible + "','" + cls.IsMandatory + "','" + cls.ContainerId + "','" + cls.IsGridColumn + "','" + cls.MandatoryControlId + "','" + cls.IsActive + "','" + cls.ModifiedBy +"','" + cls.ModifiedDate + "')";
            objDa.execute_Command(strSql);
        }
    }

    public string[] validateControlsSetting(string strPageName, Page page)
    {
        string[] _result =  {"",""};
        _result[0] = "true";
        try
        {
            List<clsContorlsSetting> lstCls = getMandatoryControls(strPageName);
            foreach (clsContorlsSetting cls in lstCls)
            {
                Control ctl = page.FindControl(cls.MandatoryControlId);
                if (ctl is TextBox)
                {
                    if (((TextBox)ctl).Text == string.Empty)
                    {
                        _result[0] = "false";
                        _result[1] = cls.Caption + " blank not allowed";
                        return _result;
                    }
                }
                else if (ctl is DropDownList)
                {
                    if (((DropDownList)ctl).SelectedValue == "0")
                    {
                        _result[0] = "false";
                        _result[1] = cls.Caption + " Invalid selection";
                        return _result;
                    }
                }
            }
            return _result;
        }
        catch
        {
            return _result;
        }
    }
    public void setControlsVisibility(List<clsContorlsSetting> lstObject, Page page)
    {
        try
        {
            lstObject = lstObject.Where(w => w.IsGridColumn == false).ToList();
            foreach (PageControlsSetting.clsContorlsSetting cls in lstObject)
            {
                Control ctl1 = page.FindControl(cls.ControlId);
                if (!cls.IsVisible)
                {
                    ctl1.Visible = false;
                }
                else
                {
                    ctl1.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void setColumnsVisibility(GridView grd, List<clsContorlsSetting> lstObject)
    {
        try
        {
            lstObject = lstObject.Where(w => ((w.IsGridColumn == true) && (w.ContainerId == grd.UniqueID))).ToList();
            foreach (clsContorlsSetting cls in lstObject)
            {
                if (!cls.IsVisible)
                {
                    grd.Columns[int.Parse(cls.ControlId)].Visible = false;
                }
                else
                {
                    grd.Columns[int.Parse(cls.ControlId)].Visible = true;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void setColumnsVisibility(DevExpress.Web.ASPxGridView grd, List<clsContorlsSetting> lstObject)
    {
        try
        {
            lstObject = lstObject.Where(w => ((w.IsGridColumn == true) && (w.ContainerId == grd.UniqueID))).ToList();
            foreach (clsContorlsSetting cls in lstObject)
            {
                if (!cls.IsVisible)
                {
                    grd.Columns[int.Parse(cls.ControlId)].Visible = false;
                }
                else
                {
                    grd.Columns[int.Parse(cls.ControlId)].Visible = true;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
}