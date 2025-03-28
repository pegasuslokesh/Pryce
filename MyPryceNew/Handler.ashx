<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;


public class Handler : IHttpHandler
{


    public void ProcessRequest(HttpContext context)
    {
        try
        {
            //string str = "C:\\Users\\LENOVO\\Desktop\\Today_work\\Capture.png";
            //context.Response.TransmitFile(str);
            //context.Response.Flush();
            //context.Response.End();
            string imageid = context.Request.QueryString["ImID"];


            SqlConnection connection = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
            connection.Open();
            //SqlCommand command = new SqlCommand("select pImage, Field1 from Inv_Product_Image where ProductId='" + imageid + "'", connection);
            SqlCommand command = new SqlCommand("select Company_Id, Field1 from Inv_Product_Image where ProductId='" + imageid + "'", connection);
            SqlDataReader dr = command.ExecuteReader();
            dr.Read();
            try
            {
                context.Response.TransmitFile("~/CompanyResource/" + dr[0] + "/Product/" + dr[1]);
                //context.Response.BinaryWrite((byte[])dr[0]);
            }
            catch (Exception ex)
            {

            }
            connection.Close();
            context.Response.End();

        }
        catch
        {

        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}