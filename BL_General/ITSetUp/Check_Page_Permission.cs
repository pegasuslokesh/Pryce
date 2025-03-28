using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;


/// <summary>
/// Summary description for Check_Page_Permission
/// </summary>
public class Check_Page_Permission
{
    IT_ObjectEntry objObjectEntry = null;
    Common cmn = null;
    

    public Check_Page_Permission(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        objObjectEntry = new IT_ObjectEntry(strConString);
        cmn = new Common(strConString);
    }

    public string CheckPagePermission(string UserID, string Object_Id,string strCompId,string strApplicationId)
    {
       // string URL = ".." + HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.Replace("~", "").Replace("#", "");

        string Condition = "True";
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        if (UserID == "superadmin")
        {
            Condition = "True";
        }
        else
        {
            
            DataTable dtModule = objObjectEntry.GetModuleIdAndName(Object_Id, objObjectEntry.GetModuleObjectByApplicationId(strApplicationId));
            if (dtModule.Rows.Count > 0)
            {
                strModuleId = dtModule.Rows[0]["Module_Id"].ToString();
                strModuleName = dtModule.Rows[0]["Module_Name"].ToString();
                Condition = "True";
            }
            else
            {
                Condition = "False";
            }
            if (strModuleId != "")
            {
                //DataTable dt_Page_Permission = cmn.GetAllPagePermission(UserID, strModuleId, Object_Id);
                //if (dt_Page_Permission.Rows.Count == 0)
                //{
                    //Condition = "False";
                    DataTable dt_user_Permission = cmn.Get_UserPermission(UserID, strModuleId, Object_Id,strCompId);
                    if (dt_user_Permission.Rows.Count == 0)
                    {
                        Condition = "False";
                    }
                    else
                    {
                        Condition = "True";
                    }
                //}
            }
        }
        return Condition;
    }
}