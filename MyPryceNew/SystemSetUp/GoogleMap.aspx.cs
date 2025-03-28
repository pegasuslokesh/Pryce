using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GoogleMap : System.Web.UI.Page
{
    SystemParameter objSys = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        objSys = new SystemParameter(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            if (Session["Add"] == null)
            {
                txtLong.Value = "0";
                txtLati.Value = "0";
                try
                {
                    txtLong.Value = Session["Long"].ToString();
                    txtLati.Value = Session["Lati"].ToString();
                }
                catch
                {
                    txtLong.Value = "0";
                    txtLati.Value = "0";
                }
            }
            else
            {
                try
                {
                    txtLong.Value = Session["Long"].ToString();
                    txtLati.Value = Session["Lati"].ToString();
                }
                catch
                {
                    txtLong.Value = "0";
                    txtLati.Value = "0";
                }
            }



        }
        Page.Title = objSys.GetSysTitle();

    }

    protected void btnSet_Click(object sender, EventArgs e)
    {
        Session["Long"] = txtLong.Value;
        Session["Lati"] = txtLati.Value;

        //ScriptManager.RegisterStartupScript(Page, typeof(Page), "close", "window.close();", true);


    }
}