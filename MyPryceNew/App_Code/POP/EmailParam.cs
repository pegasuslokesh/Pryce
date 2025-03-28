using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Summary description for EmailParam
/// </summary>
public class EmailParam
{
	public EmailParam()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static string UserName
    {
        get;
        set;
    }
    public static string Password
    {
        get;
        set;
    }
    public static int LastMesscount
    {
        get;
        set;
    }

    public static string IncomingServer
    {
        get;
        set;
    }
    public static int IncomingPort
    {
        get;
        set;
    }
    public static string EnableSSL
    {
        get;
        set;
    }

    public static string StrCompid
    {
        get;
        set;
    }
    public static string StrBrandid
    {
        get;
        set;
    }
    public static string StrLocid
    {
        get;
        set;
    }
    public static string StrEmpId
    {
        get;
        set;
    }
    public static string StrUserId
    {
        get;
        set;
    }
}
