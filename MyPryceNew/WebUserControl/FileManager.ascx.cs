using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using PegasusDataAccess;

public partial class WebUserControl_FileManager : System.Web.UI.UserControl
{
    DataAccessClass objDa = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
            DataTable dt = new DataTable();
        }

        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        RootFolder();
    }

    protected void ASPxFileManager1_SelectedFileOpened(object source, DevExpress.Web.FileManagerFileOpenedEventArgs e)
    {
        try
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "setImageUrlUsingAspxFileManager()", true);
            
            ((Image)Parent.FindControl("imgProduct")).ImageUrl = e.File.FullName.ToString();
            //((Image)((Page)Parent.Parent).FindControl("imgProduct")).ImageUrl = e.File.FullName.ToString();

        }
        catch (Exception ex)
        {

        }

        if (Parent.Page.ToString() == "ASP.inventory_itemmaster_aspx")
        {
            //Session["Image"] = FileToByteArray(Server.MapPath(e.File.FullName.ToString()));

            Byte[] buffer = FileToByteArray(Server.MapPath(e.File.FullName.ToString()));
            try
            {
                imgUrl.Value = "../CompanyResource/" + Session["CompId"].ToString() + "/Product/" + e.File.Name;
                File.WriteAllBytes(Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/Product/" + e.File.Name), buffer);
            }
            catch
            {
            }

            Session["Image"] = e.File.Name;
            
        }
        else if (Parent.Page.ToString() == "ASP.inventory_productcategorymaster_aspx")
        {
            Byte[] buffer = FileToByteArray(Server.MapPath(e.File.FullName.ToString()));
            try
            {
                imgUrl.Value = "~/CompanyResource/" + Session["CompId"].ToString() + "/ProductCategory/" + e.File.Name;
                File.WriteAllBytes(Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/ProductCategory/" + e.File.Name), buffer);
            }
            catch
            {
            }
            Session["Image"] = e.File.Name;
        }
        else if (Parent.Page.ToString() == "ASP.inventory_modelmaster_aspx")
        {
            Byte[] buffer = FileToByteArray(Server.MapPath(e.File.FullName.ToString()));
            try
            {
                imgUrl.Value = "~/CompanyResource/" + Session["CompId"].ToString() + "/Model/" + e.File.Name;
                File.WriteAllBytes(Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/Model/" + e.File.Name), buffer);
            }
            catch
            {
            }
            Session["Image"] = e.File.Name;

        }
        try
        {
            ((Panel)Parent.FindControl("pnlAddress1")).Visible = false;
            ((Panel)Parent.FindControl("pnlAddress2")).Visible = false;
        }
        catch
        { }
        Session["imgUrl"] = e.File.FullName.ToString();

    }

    public byte[] FileToByteArray(string fileName)
    {
        byte[] buff = null;
        FileStream fs = new FileStream(fileName,
                                       FileMode.Open,
                                       FileAccess.Read);
        BinaryReader br = new BinaryReader(fs);
        long numBytes = new FileInfo(fileName).Length;
        buff = br.ReadBytes((int)numBytes);
        return buff;
    }

    #region Filemanager
    public void RootFolder()
    {
        string User = HttpContext.Current.Session["UserId"].ToString();

        if (User == "superadmin")
        {
            ASPxFileManager1.Settings.RootFolder = "~\\Product";
            ASPxFileManager1.Settings.InitialFolder = "~\\Product";
            ASPxFileManager1.Settings.ThumbnailFolder = "~\\Product\\Thumbnail\\";
        }
        else
        {
            try
            {
                string RegistrationCode = Common.Decrypt(objDa.get_SingleValue("Select registration_code from Application_Lic_Main"));
                string folderPath = "~\\Product\\Product_" + RegistrationCode + "";
                string fullPath = Server.MapPath(folderPath);
                if (Directory.Exists(fullPath))
                {
                    //Console.WriteLine("The folder already exists.");
                }
                else
                {
                    try
                    {
                        Directory.CreateDirectory(fullPath);
                        //Console.WriteLine("The folder has been created.");
                    }
                    catch (Exception ex)
                    {

                    }
                }
                if (Directory.Exists(folderPath + "\\Thumbnail"))
                {
                    //Console.WriteLine("The folder already exists.");
                }
                else
                {
                    try
                    {
                        Directory.CreateDirectory(folderPath + "\\Thumbnail");
                        //Console.WriteLine("The folder has been created.");
                    }
                    catch (Exception ex)
                    {

                    }
                }
                ASPxFileManager1.Settings.RootFolder = folderPath;
                ASPxFileManager1.Settings.InitialFolder = folderPath;
                ASPxFileManager1.Settings.ThumbnailFolder = folderPath + "\\Thumbnail";
            }
            catch(Exception ex)
            {

                ASPxFileManager1.Settings.RootFolder = "~\\Product";
                ASPxFileManager1.Settings.InitialFolder = "~\\Product";
                ASPxFileManager1.Settings.ThumbnailFolder = "~\\Product\\Thumbnail\\";
            }
        }

    }
    #endregion
}