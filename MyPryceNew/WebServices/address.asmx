<%@ WebService Language="C#" Class="address" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using PegasusDataAccess;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class address  : System.Web.Services.WebService {
    [WebMethod(enableSession:true)]
    public string[] validateAddressName(string addressName)
    {
        DataAccessClass objDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        string[] result = new string[2];
        string count = "0";
        count = objDa.get_SingleValue("SELECT count(trans_id) FROM dbo.Set_AddressMaster WHERE Set_AddressMaster.Address_Name = '" + addressName + "' ");
        count = count == "@NOTFOUND@" ? "0" : count;
        if (count == "0")
        {
            result[0] = "false";
            result[1] = "Address Not Valid";
        }
        else
        {
            result[0] = "true";
            result[1] = count;
        }
        return result;
    }

    [WebMethod(enableSession:true)]
    public void resetAddressStateId()
    {
        Session["AddCtrl_State_Id"] = "0";
    }
    [WebMethod(enableSession:true)]
    public string validateStateName(string CountryId, string StateName)
    {
        StateMaster ObjStatemaster = new StateMaster(Session["DBConnection"].ToString());
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
    [WebMethod(enableSession:true)]
    public string validateCity(string stateId, string cityName)
    {
        CityMaster ObjCityMaster = new CityMaster(Session["DBConnection"].ToString());
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
}