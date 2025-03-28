using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;

public partial class Duty_Master_Default3 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]

    public static List<string> GetAutoCompleteData(string username)
    {
        if (HttpContext.Current.Session["UserId"] == null)
        {
            HttpContext.Current.Response.Redirect("~/ERPLogin.aspx");
        }
        QualificationMaster QualificationMaster = new QualificationMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(QualificationMaster.GetQualification("0"), "Qualification like '" + username.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        List<string> result = new List<string>();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            result[i] = dt.Rows[i]["Qualification"].ToString();
        }
        return result;

        //List<string> result = new List<string>();
        //using (SqlConnection con = new SqlConnection("Data Source=SureshDasari;Integrated Security=true;Initial Catalog=MySampleDB"))
        //{
        //    using (SqlCommand cmd = new SqlCommand("select DISTINCT UserName from UserInformation where UserName LIKE '%'+@SearchText+'%'", con))
        //    {
        //        con.Open();
        //        cmd.Parameters.AddWithValue("@SearchText", username);
        //        SqlDataReader dr = cmd.ExecuteReader();
        //        while (dr.Read())
        //        {
        //            result.Add(dr["UserName"].ToString());
        //        }
        //        return result;
        //    }
        //}
    }
}