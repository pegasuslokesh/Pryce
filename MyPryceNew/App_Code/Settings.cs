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
using PegasusUtility;
/// <summary>
/// Summary description for Settings
/// </summary>
public class Settings
{    
    public static string Project_Id = "3";
    public static string Code1 = "C1";
    public static string Code2 = "C2";
    public static string Code3 = "C3";
    public static string Code4 = "C4";
    public static bool IsDemo = false;
    public static bool IsValid = true;

	public Settings()
	{
		//
		// TODO: Add constructor logic here
		//

      

	}

    public static  string GetCode(int i)
    {

        PegasusUtility.PegasusResource objRe = new PegasusResource();
        string strResult = string.Empty;
        switch (i)
        {
            case 1:
                strResult = objRe.GetCode1();
                break;
            case 2:
                strResult = objRe.GetCode2();
                break;
            case 3:
                strResult = objRe.GetCode3("C");
                break;
            case 4:
                strResult = objRe.GetCode4();
                break;


        }

        return strResult;
    }
 
}
