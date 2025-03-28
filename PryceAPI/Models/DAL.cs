using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PryceAPI.Models
{
    public class DAL
    {
        public DataTable GetRecords(string sql)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["PegaConnection"].ConnectionString);
                SqlDataAdapter adp = new SqlDataAdapter(sql,con);
                adp.Fill(dt);
            }
            catch
            {
            }
            return dt;
        }
        public int ExcuteDML(string sql)
        {
            int i = 0;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["PegaConnection"].ConnectionString);
            try
            {
                
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                con.Open();
                i = cmd.ExecuteNonQuery();
                con.Close();
            }
            catch
            {
                if(con.State  ==  ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return i;
        }
    }
}