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
using PegasusDataAccess;
using PegasusUtility; 
/// <summary>
/// Summary description for Lic_App_Settings
/// </summary>
public class Lic_App_Settings
{
    DataAccessClass DaClass = null;
    PegasusUtility.EncryptionUtility objEU = new EncryptionUtility ();
	public Lic_App_Settings(string strConString)
	{
        //
        // TODO: Add constructor logic here
        //
        DaClass = new DataAccessClass(strConString);
	}
    public int App_Settings_CRUD(string Inquiry_Id, string Contact_Id, string Project_Id, string Installation_Type, string System_Code1, string System_Code2, string System_Code3, string System_Code4, string Field1, string Field2, string Field3, string Field4, string Field5, string Action_Date, string ExpiryDate, string OpType)
    {
        PassDataToSql[] paramList = new PassDataToSql[17];
        paramList[0] = new PassDataToSql("@Inquiry_Id",objEU.Encrypt (Inquiry_Id), PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Contact_Id",objEU.Encrypt(Contact_Id + "##" + Inquiry_Id ), PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Project_Id", objEU.Encrypt(Project_Id  + "##" + Inquiry_Id), PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Installation_Type", objEU.Encrypt(Installation_Type  + "##" + Inquiry_Id), PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@System_Code1", objEU.Encrypt(System_Code1  + "##" + Inquiry_Id), PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@System_Code2", objEU.Encrypt(System_Code2  + "##" + Inquiry_Id), PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@System_Code3", objEU.Encrypt(System_Code3  + "##" + Inquiry_Id), PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@System_Code4", objEU.Encrypt(System_Code4  + "##" + Inquiry_Id), PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Action_Date", objEU.Encrypt(Action_Date + "##" + Inquiry_Id), PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Expiry_Date", objEU.Encrypt(ExpiryDate  + "##" + Inquiry_Id), PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[16] = new PassDataToSql("@Op_Type", OpType, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        DaClass.execute_Sp("Lic_App_Settings_CRUD", paramList);
        return Convert.ToInt32(paramList[15].ParaValue);
    }
}
