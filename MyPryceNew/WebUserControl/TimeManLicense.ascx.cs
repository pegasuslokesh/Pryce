using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebUserControl_TimeManLicense : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
      
    }

    public void GetLicenseInformation(string strRegistrationcode,string strLicenseKey,string strEmailId,string strExpiryDate,string strDeviceCount,string strUserCount,string strEmployeeCount,string strAppMode, string   strProductCode,string strMasterProductCode)
    {
        lblRegistrationcode.Text = strRegistrationcode;
        lblLicenseKey.Text = strLicenseKey;
        lblEmailId.Text = strEmailId;
        lblExpirydate.Text = strExpiryDate;
        lblDeviceCount.Text = strDeviceCount;
        if(Convert.ToInt32(strUserCount)>0)
        lblUserCount.Text = strUserCount;
        else
            lblUserCount.Text = "Unlimited";

        if (Convert.ToInt32(strEmployeeCount) > 0)
            lblEmployeeCount.Text = strEmployeeCount;
        else
            lblEmployeeCount.Text = "Unlimited";


        lblAppMode.Text = strAppMode;
        lblProductCode.Text = strProductCode;




        if (strMasterProductCode == "497")
        {
            lblProductCaption.Text = "Timeman Bio Attendance";
            lblProductText.Text = "TimeMan is a web based, Reliable, Fast & Easy to use attendance Solution. This Solution is our core Retail, Restaurant and Government operations offering. It is flexible to use and can easily be set up to support a broad variety of Attendance concepts and workflows. Our customers range is Restaurants, Schools, Offices Hospitals and so on where is need of Employee Attendance.. It is so easy to use that any common man can use it.";
        }
        else
        {
            lblProductCaption.Text = "Pryce ERP Solution";
            lblProductText.Text = "Pegasus engaged in providing management and software consultancy as well as implementing ERP Solutions. The Pryce ERP Software has been well received and rank high in meeting user expectations. We specialized and expert in Customized ERP Solutions.By having  PryceERP software system, rather than scattered different systems that can’t communicate with one another, companies can keep track of orders more easily, and coordinate manufacturing, inventory and shipping among many different locations at the same time.";

        }


    }
}