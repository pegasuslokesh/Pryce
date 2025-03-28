using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.XtraPrinting.Export.Pdf;
using System.IO;
using System.Data;
using PegasusDataAccess;


public partial class FileExplorer : System.Web.UI.Page
{
    Arc_Directory_Master objDir = null;
    Arc_FileTransaction objFile = null;
    DataAccessClass objDa = null;
    Document_Master ObjDocument = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        ObjDocument = new Document_Master(Session["DBConnection"].ToString());
        objDir = new Arc_Directory_Master(Session["DBConnection"].ToString());
        objFile = new Arc_FileTransaction(Session["DBConnection"].ToString());

        if (Session["EmpId"].ToString() != "0")
        {
            string strDirectoryList = string.Empty;
            DataTable dt = objDa.return_DataTable("SELECT STUFF((SELECT  DISTINCT ',' + RTRIM(    cast( Arc_Directory_Creation.Company_Id as nvarchar(50))+','+Arc_Directory_Creation.Directory_Name+','+Arc_Directory_Creation.createdby+','+ISNULL( Arc_Document_Master.Document_Name,'')) from Arc_Directory_Privileges left join Arc_Directory_Creation on Arc_Directory_Creation.Id=Arc_Directory_Privileges.directory_id left join Arc_File_Transaction on  Arc_File_Transaction.Directory_Id=Arc_Directory_Privileges.directory_id left join  Arc_Document_Master on Arc_File_Transaction.Document_Master_Id= Arc_Document_Master.Id   where Arc_Directory_Privileges.emp_id=" + Session["EmpId"].ToString() + " FOR xml PATH ('')), 1, 1, '')");
            if (dt != null && dt.Rows.Count > 0)
            {
                strDirectoryList = dt.Rows[0][0].ToString().Replace("/", ",");
            }

            ApplyRules(ASPxFileManager1.SelectedFolder, strDirectoryList);
        }




    }

    void ApplyRules(FileManagerFolder folder, string strDirectoryList)
    {
        FileManagerFolder[] folders = folder.GetFolders();




        for (int i = 0; i < folders.Length; i++)
        {

            if (folders[i].FullName == "~\\ArcaWing\\" + Session["CompId"].ToString())
            {
                ApplyRules(folders[i], strDirectoryList);
                continue;
            }





            if (strDirectoryList.Split(',').Contains(folders[i].Name))
            {
                ApplyRules(folders[i], strDirectoryList);
                continue;
            }
            else
            {


                FileManagerFolderAccessRule folderEditingRule = new FileManagerFolderAccessRule(folders[i].RelativeName);
                folderEditingRule.Edit = Rights.Deny;
                folderEditingRule.Browse = Rights.Deny;
                folderEditingRule.Upload = Rights.Deny;


                FileManagerFolderAccessRule folderContentEditingRule = new FileManagerFolderAccessRule(folders[i].RelativeName);
                folderContentEditingRule.EditContents = Rights.Deny;
                folderContentEditingRule.Edit = Rights.Deny;
                folderContentEditingRule.Upload = Rights.Deny;



                ASPxFileManager1.SettingsPermissions.AccessRules.Add(folderEditingRule);
                ASPxFileManager1.SettingsPermissions.AccessRules.Add(folderContentEditingRule);

                ApplyRules(folders[i], strDirectoryList);
            }


        }
    }





    protected void ASPxFileManager1_OnFolderCreating(object sender, FileManagerFolderCreateEventArgs e)
    {

        //string NewDirectory = Server.MapPath("ArcaWing/" + Session["CompId"].ToString() + "/" + Session["UserId"].ToString() + "/" + e.Name.ToString());
        //try
        //{
        //    int i = CreateDirectoryIfNotExist(NewDirectory);
        //}
        //catch
        //{

        //}
        //string Name = e.Name;

        //string parent = e.ParentFolder.Name.ToString();


        //objDir.InsertDirectorymaster(Session["CompId"].ToString(), e.Name.ToString(), "0", Session["BrandId"].ToString(), Session["UserId"].ToString(), "", Session["LocId"].ToString(), false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

        //Directory.Delete(Server.MapPath("ArcaWing/" + e.Name));

    }


    protected void ASPxFileManager1_OnFileUploading(object sender, FileManagerFileUploadEventArgs e)
    {

        //string strDirectoryId = string.Empty;

        //if (Directory.Exists(Server.MapPath("ArcaWingDocument/" + e.File.Folder.Name.ToString())))
        //{
        //    Directory.Delete(Server.MapPath("ArcaWingDocument/" + e.File.Folder.Name.ToString()));
        //}



        //strDirectoryId = GetDirectoryId(e.File.Folder.Name.ToString());

        //if (strDirectoryId == "0")
        //{
        //    DisplayMessage("Directory not exists");
        //    e.Cancel = true;
        //    return;
        //}


        //Byte[] bytes = new Byte[0];

        //Stream fs = e.InputStream;
        //BinaryReader br = new BinaryReader(fs);
        //bytes = br.ReadBytes((Int32)fs.Length);


        //string NewDirectory = Server.MapPath("ArcaWingDocument/" + Session["CompId"].ToString() + "/" + Session["UserId"].ToString() + "/" + e.File.Folder.Name.ToString() + "/" + txtDocumentName.Text);
        //try
        //{
        //    int i = CreateDirectoryIfNotExist(NewDirectory);
        //}
        //catch
        //{

        //}

        //string filepath = "~/" + "ArcaWingDocument" + "/" + Session["CompId"].ToString() + "/" + Session["UserId"].ToString() + "/" + e.File.Folder.Name.ToString() + "/" + txtDocumentName.Text + "/" + e.File.Name.ToString();
        //string filepath1 = "~/" + "ArcaWingDocument" + "/" + Session["CompId"].ToString() + "/" + Session["UserId"].ToString() + "/" + e.File.Folder.Name.ToString() + "/" + txtDocumentName.Text;


        //try
        //{

        //    System.IO.File.Move(Server.MapPath(e.File.FullName), Server.MapPath(filepath1) + "\\" + e.File.Name);

        //}

        //catch (Exception ex)
        //{

        //    //MessageBox.Show(ex.ToString());

        //}


        //objFile.Insert_In_FileTransaction(Session["CompId"].ToString(), strDirectoryId, "2", "0", e.File.Name.ToString(), DateTime.Now.ToString(), bytes, Server.MapPath(filepath), DateTime.Now.AddYears(20).ToString(), "", "0", Server.MapPath(filepath), "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


        ////if (!Directory.Exists(filepath))
        ////{

        ////    Byte[] buffer = FileToByteArray(e.File.Length.ToString());
        ////    //Byte[] buffer = FileToByteArray(Server.MapPath(e.File.FullName.ToString()));
        ////    try
        ////    {
        ////        File.WriteAllBytes(Server.MapPath(filepath), buffer);
        ////    }
        ////    catch
        ////    {
        ////    }
        ////}



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

    public int CreateDirectoryIfNotExist(string NewDirectory)
    {
        int checkDirectory = 0;
        try
        {
            // Checking the existance of directory
            if (!Directory.Exists(NewDirectory))
            {
                //If No any such directory then creates the new one
                Directory.CreateDirectory(NewDirectory);
            }
            else
            {
                checkDirectory = 1;
            }
        }
        catch (IOException _err)
        {
            Response.Write(_err.Message);
        }
        return checkDirectory;
    }
    protected void ASPxFileManager1_OnItemDeleting(object sender, FileManagerItemDeleteEventArgs e)
    {



        string strDirectoryName = string.Empty;



        if (GetDirectoryId(e.Item.Name.ToString()) != "0")
        {


            objDa.return_DataTable("delete from Arc_File_Transaction where Directory_Id='" + GetDirectoryId(strDirectoryName) + "'");
        }
        else
        {
            strDirectoryName = e.Item.RelativeName.Replace(e.Item.Name.ToString(), "");

            char r = Convert.ToChar(92);



            strDirectoryName = strDirectoryName.Replace(r, ' ');

            objDa.return_DataTable("delete from Arc_File_Transaction where Directory_Id='" + GetDirectoryId(strDirectoryName) + "'  and FILE_NAME='" + e.Item.Name.ToString() + "'");

        }



        //e.Cancel = true;




    }


    protected void ASPxFileManager1_OnItemRenaming(object sender, FileManagerItemRenameEventArgs e)
    {

        string strDirectoryName = e.Item.RelativeName.Replace(e.Item.Name.ToString(), "");

        char r = Convert.ToChar(92);


        strDirectoryName = strDirectoryName.Replace(r, ' ');

        objDa.return_DataTable("update Arc_File_Transaction set File_Name ='" + e.NewName.ToString() + "' where Directory_Id='" + GetDirectoryId(strDirectoryName) + "'  and FILE_NAME='" + e.Item.Name.ToString() + "'");


    }

    protected void ASPxFileManager1_OnItemMoving(object sender, FileManagerItemMoveEventArgs e)
    {

        string strDirectoryName = e.Item.RelativeName.Replace(e.Item.Name.ToString(), "");

        char r = Convert.ToChar(92);


        strDirectoryName = strDirectoryName.Replace(r, ' ');


        objDa.return_DataTable("update Arc_File_Transaction set Directory_Id ='" + GetDirectoryId(e.DestinationFolder.Name.ToString()) + "' where Directory_Id='" + GetDirectoryId(strDirectoryName) + "'  and FILE_NAME='" + e.Item.Name.ToString() + "'");

    }


    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
    }

    public string GetDirectoryId(string strDirectoryName)
    {

        string strDirectoryId = "0";
        DataTable dt = objDir.GetDirectoryMaster_By_DirectoryName(Session["CompId"].ToString(), strDirectoryName);

        if (dt.Rows.Count > 0)
        {

            strDirectoryId = dt.Rows[0]["Id"].ToString();
        }

        dt.Dispose();
        return strDirectoryId;

    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDocName(string prefixText, int count, string contextKey)
    {
        Document_Master objdoc = new Document_Master(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objdoc.getdocumentmaster(HttpContext.Current.Session["CompId"].ToString(), "0");
        DataTable dtFilter = new DataTable();
        dtFilter = dt.Copy();
        dt = new DataView(dt, "Document_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count == 0)
        {
            dt = dtFilter;
        }
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Document_Name"].ToString();
        }
        return txt;
    }


    //protected void txtDocumentName_OnTextChanged(object sender, EventArgs e)
    //{
    //    if (txtDocumentName.Text != "")
    //    {
    //        DataTable dt = ObjDocument.getdocumentmaster(HttpContext.Current.Session["CompId"].ToString(), "0");
    //        try
    //        {
    //            dt = new DataView(dt, "Document_Name ='" + txtDocumentName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
    //        }
    //        catch
    //        {
    //        }
    //        if (dt.Rows.Count == 0)
    //        {
    //            DisplayMessage("Select Document Name in Suggestion Only");
    //            txtDocumentName.Text = "";
    //            txtDocumentName.Focus();
    //            hdndocumentid.Value = "";
    //            return;
    //        }
    //        else
    //        {
    //            hdndocumentid.Value = dt.Rows[0]["Id"].ToString();

    //        }

    //    }
    //}
}