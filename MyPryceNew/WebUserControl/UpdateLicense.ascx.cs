using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
public partial class WebUserControl_UpdateLicense : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSyncLicInfo_Click(object sender, EventArgs e)
    {
        MasterDataAccess objMa = new MasterDataAccess(ConfigurationManager.ConnectionStrings["PegaConnection1"].ToString());
        
    }
    public void setDefaultValue(string strMessage)
    {
       lnkUpdateLicense.Text= "If your product is not registerd," + " <a href='" + ConfigurationManager.AppSettings["RegistrationUrl"].ToString()  + "'> Click here</a> to register";
    }
}