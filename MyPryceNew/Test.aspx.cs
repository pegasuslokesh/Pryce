using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using System.Data;
using System.IO;
using System.Drawing;
using PryceDevicesLib;
using System.Collections;

public partial class Test : System.Web.UI.Page
{
    DataAccessClass objDa = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            objDa = new DataAccessClass(Session["DBConnection"].ToString());
            IAttDeviceOperation objDa1 = PryceDevicesLib.DeviceOperation.getDeviceLibObj(TechType.PULL,"zkTecho");
            //List<clsUserLog> lstObj=objDa.getDeviceLog("", System.DateTime.Now, System.DateTime.Now);
            //clsUserLog cls = new clsUserLog();
            
            //IAttDeviceOperation attDeviceOperation = Att_getDeviceClass.getDeviceClass("ZkTecho");
            //attDeviceOperation.connectDevice("");
        }

       

    }


    public System.Drawing.Image ByteArrayToImage(byte[] data)
    {
        MemoryStream bipimag = new MemoryStream(data);
        System.Drawing.Image imag = new Bitmap(bipimag);
        return imag;
    }







    protected void btnupdate_Click(object sender, EventArgs e)
    {

        //txtscaleamountresult.Text = SystemParameter.GetScaleAmount(txtscaleamount.Text, txtdeninimation.Text);



        string strsql = string.Empty;

        strsql = "select emp_id,User_Id from set_usermaster";
        DataTable dt = objDa.return_DataTable(strsql);

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            objDa.execute_Command("update set_usermaster set password ='" + Common.Encrypt(dt.Rows[i]["User_Id"].ToString()) + "' where emp_id=" + dt.Rows[i]["Emp_Id"].ToString() + "");
        }

        objDa.execute_Command("update Set_ApplicationParameter set param_value = 'fc0iUkg331qk3V8HY6MWvQ==' where param_name = 'Support_Email_Password'");

        // update Set_ApplicationParameter set param_value = 'fc0iUkg331qk3V8HY6MWvQ==' where param_name = 'Support_Email_Password'


        //for chnage com

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


    protected void btnbytearaytoimage_Click(object sender, EventArgs e)
    {


        DataTable dt = objDa.return_DataTable("select inv_productmaster.Company_Id,inv_productmaster.productcode,inv_product_image.pImage,inv_product_image.ProductId from inv_product_image inner join inv_productmaster on inv_product_image.productid = inv_productmaster.ProductId where inv_product_image.Field1 is null or inv_product_image.Field1=''");

        for (int i = 0; i < dt.Rows.Count; i++)
        {


            CreateDirectoryIfNotExist(Server.MapPath("/CompanyResource/" + dt.Rows[i]["Company_Id"] + "/Product/"));

            try
            {
                byte[] bytes = (Byte[])dt.Rows[i]["pImage"];
                

                System.Drawing.Image img = ByteArrayToImage(bytes);
                string strFilename = dt.Rows[i]["productcode"].ToString().Replace("/", "") + ".png";
                img.Save(Server.MapPath("/CompanyResource/" + dt.Rows[i]["Company_Id"] + "/Product/" + strFilename));


                objDa.execute_Command("update inv_product_image set Field1= '" + strFilename + "' where  ProductId = " + dt.Rows[i]["ProductId"] + " ");
            }
            catch (Exception ex)
            {
                continue;
            }
        }
    }

    protected void btnDummy_Click(object sender, EventArgs e)
    {
        int ab = 0;
        ab = 25;
    }

    protected void ab_Click(object sender, EventArgs e)
    {
        string val = abc.Text;
        foreach(var item in ab.Controls)
        {
           
        }
    }
}