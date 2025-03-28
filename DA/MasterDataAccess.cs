using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using System.Configuration;

namespace PegasusDataAccess
{
    public class MasterDataAccess
    {
        private SqlConnection db;
        private SqlCommand cmd;
        private string str_ConnetionString;

        public SqlConnection con;
        /********* Constructor will assign Connection string to variables *******/


        public MasterDataAccess(string strConString)
        {


            try
            {


                //str_ConnetionString = HttpContext.Current.Session["MyCon"].ToString();
                //str_ConnetionString = ConfigurationManager.ConnectionStrings["PegaConnection"].ConnectionString;
                str_ConnetionString = strConString;
            }
            catch (Exception ex)
            {
                str_ConnetionString = string.Empty;


            }


        }

        public string get_SingleValue(string sql)
        {
            try
            {
                string str_ReturnValue;
                using (db = new SqlConnection())
                {
                    db.ConnectionString = str_ConnetionString;
                    db.Open();
                    SqlDataReader temp_Reader;
                    {
                        using (cmd = new SqlCommand("", db))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = sql;
                            cmd.Connection = db;
                            temp_Reader = cmd.ExecuteReader();

                            if (temp_Reader.HasRows)
                            {
                                temp_Reader.Read();
                                str_ReturnValue = temp_Reader[0].ToString();
                            }
                            else
                                str_ReturnValue = "@NOTFOUND@";
                        }
                    }
                    temp_Reader.Close();
                }
                return str_ReturnValue;
            }
            catch
            {
                throw;
            }
        }

        public string get_SingleValue(string sql, ref SqlTransaction trns)
        {
            try
            {
                string str_ReturnValue;
                using (db = new SqlConnection())
                {
                    db.ConnectionString = str_ConnetionString;
                    db.Open();
                    SqlDataReader temp_Reader;
                    {
                        using (cmd = new SqlCommand("", db))
                        {

                            cmd.Transaction = trns;
                            cmd.Connection = trns.Connection;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = sql;
                            temp_Reader = cmd.ExecuteReader();

                            if (temp_Reader.HasRows)
                            {
                                temp_Reader.Read();
                                str_ReturnValue = temp_Reader[0].ToString();
                            }
                            else
                                str_ReturnValue = "@NOTFOUND@";
                        }
                    }
                    temp_Reader.Close();
                }
                return str_ReturnValue;
            }
            catch
            {
                throw;
            }
        }

        public int execute_Command(string str_Sql, ref SqlTransaction trns)
        {
            try
            {
                int i;

                //db.ConnectionString = str_ConnetionString;
                //db.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = trns.Connection;
                    cmd.Transaction = trns;
                    cmd.CommandText = str_Sql;

                    i = cmd.ExecuteNonQuery();
                }

                return i;
            }
            catch
            {
                throw;
            }
        }

        public int execute_Command(string str_Sql)
        {
            try
            {
                int i;
                using (db = new SqlConnection())
                {
                    db.ConnectionString = str_ConnetionString;
                    db.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = db;
                        cmd.CommandText = str_Sql;
                        cmd.CommandTimeout = 1000000000;
                        i = cmd.ExecuteNonQuery();
                    }
                }
                return i;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string execute_Sp(string str_Sql, PassDataToSql[] ParaList)
        {
            //try
            //{
            SqlParameter cmdpara;
            SqlParameter cmdpara_return;
            string str_Res = "";
            using (db = new SqlConnection())
            {
                db.ConnectionString = str_ConnetionString;
                db.Open();
                using (cmd = new SqlCommand())
                {
                    cmd.Connection = db;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = str_Sql;

                    cmdpara_return = cmd.Parameters.Add("@RowCount", SqlDbType.Int);
                    cmdpara_return.Direction = ParameterDirection.ReturnValue;

                    for (int i = 0; i <= ParaList.GetUpperBound(0); i++)
                    {
                        cmdpara = new SqlParameter();
                        if (ParaList[i].ParaType.Equals(PassDataToSql.ParaTypeList.Text))
                            cmdpara.Size = 1000000;
                        else if (ParaList[i].ParaType.Equals(PassDataToSql.ParaTypeList.Nvarchar))
                            cmdpara.Size = 4000;
                        else
                            if (ParaList[i].ParaType.Equals(PassDataToSql.ParaTypeList.Float))
                        {
                            cmdpara.Offset = 18;
                            cmdpara.Precision = 2;
                        }
                        cmdpara.ParameterName = ParaList[i].ParaName.ToString();

                        if (ParaList[i].ParaType.Equals(PassDataToSql.ParaTypeList.Image))
                        {
                            cmdpara.Value = ParaList[i].ParaValue1;
                        }
                        else
                        {
                            cmdpara.Value = ParaList[i].ParaValue;
                        }

                        cmdpara.SqlDbType = ParaList[i].GetParaType(ParaList[i].ParaType);
                        cmdpara.Direction = ParaList[i].GetParaDirection(ParaList[i].ParaDirection);

                        cmd.Parameters.Add(cmdpara);
                    }

                    cmd.ExecuteNonQuery();

                    for (int i = 0; i <= ParaList.GetUpperBound(0); i++)
                    {
                        if (ParaList[i].ParaDirection.Equals(PassDataToSql.ParaDirectonList.Output) || ParaList[i].ParaDirection.Equals(PassDataToSql.ParaDirectonList.Both))
                        {
                            ParaList[i].ParaValue = cmd.Parameters[ParaList[i].ParaName].Value.ToString();
                        }

                    }
                    if (!(String.IsNullOrEmpty(cmd.Parameters["@RowCount"].Value.ToString())))
                        str_Res = cmd.Parameters["@RowCount"].Value.ToString();

                }
            }
            return str_Res;
            //}
            //catch
            //{
            //    throw;
            //}
        }
        public string execute_Sp(string str_Sql, PassDataToSql[] ParaList, ref SqlTransaction trns)
        {
            //try
            //{
            SqlParameter cmdpara;
            SqlParameter cmdpara_return;
            string str_Res = "";
            using (db = new SqlConnection())
            {
                db.ConnectionString = str_ConnetionString;
                db.Open();
                using (cmd = new SqlCommand())
                {
                    cmd.Connection = trns.Connection;
                    cmd.Transaction = trns;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = str_Sql;

                    cmdpara_return = cmd.Parameters.Add("@RowCount", SqlDbType.Int);
                    cmdpara_return.Direction = ParameterDirection.ReturnValue;

                    for (int i = 0; i <= ParaList.GetUpperBound(0); i++)
                    {
                        cmdpara = new SqlParameter();
                        if (ParaList[i].ParaType.Equals(PassDataToSql.ParaTypeList.Text))
                            cmdpara.Size = 1000000;
                        else if (ParaList[i].ParaType.Equals(PassDataToSql.ParaTypeList.Nvarchar))
                            cmdpara.Size = 4000;
                        else
                            if (ParaList[i].ParaType.Equals(PassDataToSql.ParaTypeList.Float))
                        {
                            cmdpara.Offset = 18;
                            cmdpara.Precision = 2;
                        }
                        cmdpara.ParameterName = ParaList[i].ParaName.ToString();

                        if (ParaList[i].ParaType.Equals(PassDataToSql.ParaTypeList.Image))
                        {
                            cmdpara.Value = ParaList[i].ParaValue1;
                        }
                        else
                        {
                            cmdpara.Value = ParaList[i].ParaValue;
                        }

                        cmdpara.SqlDbType = ParaList[i].GetParaType(ParaList[i].ParaType);
                        cmdpara.Direction = ParaList[i].GetParaDirection(ParaList[i].ParaDirection);

                        cmd.Parameters.Add(cmdpara);
                    }

                    cmd.ExecuteNonQuery();

                    for (int i = 0; i <= ParaList.GetUpperBound(0); i++)
                    {
                        if (ParaList[i].ParaDirection.Equals(PassDataToSql.ParaDirectonList.Output) || ParaList[i].ParaDirection.Equals(PassDataToSql.ParaDirectonList.Both))
                        {
                            ParaList[i].ParaValue = cmd.Parameters[ParaList[i].ParaName].Value.ToString();
                        }

                    }
                    if (!(String.IsNullOrEmpty(cmd.Parameters["@RowCount"].Value.ToString())))
                        str_Res = cmd.Parameters["@RowCount"].Value.ToString();

                }
            }
            return str_Res;
            //}
            //catch
            //{
            //    throw;
            //}
        }

        public bool get_SingleRecord(string sql, out DataRow dr)
        {
            try
            {
                using (db = new SqlConnection())
                {
                    db.ConnectionString = str_ConnetionString;
                    db.Open();
                    DataRow drt;
                    DataTable dt = new DataTable();
                    cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = db;
                    cmd.CommandText = sql;
                    SqlDataReader datar = cmd.ExecuteReader();
                    if (datar.HasRows == true)
                    {
                        dt.Load(datar);
                        drt = dt.Rows[0];
                        dr = drt;
                        datar.Close();
                        return true;
                    }
                    else
                    {
                        DataColumn c = new DataColumn("a", typeof(string));
                        dt.Columns.Add(c);
                        drt = dt.NewRow();
                        drt[c] = "";
                        dt.Rows.Add(drt);
                        dr = drt;
                        datar.Close();
                        return false;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public DataSet return_DataSet(string str_SqlCommand)
        {
            try
            {
                using (db = new SqlConnection())
                {
                    db.ConnectionString = str_ConnetionString;
                    db.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        using (cmd = new SqlCommand())
                        {
                            cmd.Connection = db;
                            cmd.CommandText = str_SqlCommand;
                            cmd.CommandType = CommandType.Text;
                            da.SelectCommand = cmd;

                            using (DataSet ds = new DataSet())
                            {
                                da.Fill(ds, "table1");
                                return ds;
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public DataTable return_DataTable(string str_SqlCommand)
        {
            try
            {
                using (db = new SqlConnection())
                {
                    db.ConnectionString = str_ConnetionString;
                    db.Open();
                    SqlDataReader reader_Temp;
                    using (cmd = new SqlCommand())
                    {
                        DataTable Table_Temp = new DataTable();
                        cmd.Connection = db;
                        cmd.CommandText = str_SqlCommand;
                        cmd.CommandType = CommandType.Text;
                        reader_Temp = cmd.ExecuteReader();
                        if (reader_Temp.HasRows == true)
                        {
                            Table_Temp.Load(reader_Temp);
                            reader_Temp.Close();
                            return Table_Temp;
                        }
                        else
                        {
                            reader_Temp.Close();
                            return Table_Temp;
                        }
                    }
                }
            }
            catch
            {

                throw;
            }
        }
        public DataTable return_DataTable(string str_SqlCommand, ref SqlTransaction trns)
        {
            try
            {
                using (db = new SqlConnection())
                {
                    db.ConnectionString = str_ConnetionString;
                    db.Open();
                    SqlDataReader reader_Temp;
                    using (cmd = new SqlCommand())
                    {
                        DataTable Table_Temp = new DataTable();
                        cmd.Connection = trns.Connection;
                        cmd.Transaction = trns;
                        cmd.CommandText = str_SqlCommand;
                        cmd.CommandType = CommandType.Text;
                        reader_Temp = cmd.ExecuteReader();
                        if (reader_Temp.HasRows == true)
                        {
                            Table_Temp.Load(reader_Temp);
                            reader_Temp.Close();
                            return Table_Temp;
                        }
                        else
                        {
                            reader_Temp.Close();
                            return Table_Temp;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public DataTable return_DataTable_Modify(string str_SqlCommand)
        {
            try
            {
                using (db = new SqlConnection())
                {
                    db.ConnectionString = str_ConnetionString;
                    db.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        using (cmd = new SqlCommand())
                        {
                            cmd.Connection = db;
                            cmd.CommandText = str_SqlCommand;
                            cmd.CommandType = CommandType.Text;
                            da.SelectCommand = cmd;

                            using (DataSet ds = new DataSet())
                            {
                                da.Fill(ds, "table1");
                                return ds.Tables["table1"];
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// THIS FUNCTION SEARCH RECORD ACCORDING TO THE CONDITION AND RETUN DATATABLE:SHAKTI
        /// </summary>
        /// <param name="condition">PASS THE CONDITION HERE</param>
        /// <param name="ParaList">PASS THE PARAMETER LIST HERE</param>
        /// <returns>THIS WILL RETURN DATATABLE HERE</returns>
        public DataTable Reuturn_Datatable_Search(string storedProcedureName, PassDataToSql[] ParaList)
        {
            try
            {
                SqlParameter cmdpara;
                using (db = new SqlConnection())
                {
                    db.ConnectionString = str_ConnetionString;
                    db.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        using (cmd = new SqlCommand())
                        {
                            cmd.Connection = db;
                            cmd.CommandText = storedProcedureName;
                            cmd.CommandType = CommandType.StoredProcedure;
                            for (int i = 0; i <= ParaList.GetUpperBound(0); i++)
                            {
                                cmdpara = new SqlParameter();

                                if (ParaList[i].ParaType.Equals(PassDataToSql.ParaTypeList.Nvarchar))
                                    cmdpara.Size = 250;
                                else
                                    if (ParaList[i].ParaType.Equals(PassDataToSql.ParaTypeList.Float))
                                {
                                    cmdpara.Offset = 18;
                                    cmdpara.Precision = 2;
                                }
                                cmdpara.ParameterName = ParaList[i].ParaName.ToString();
                                cmdpara.Value = ParaList[i].ParaValue;
                                cmdpara.SqlDbType = ParaList[i].GetParaType(ParaList[i].ParaType);
                                cmdpara.Direction = ParaList[i].GetParaDirection(ParaList[i].ParaDirection);

                                cmd.Parameters.Add(cmdpara);
                            }
                            da.SelectCommand = cmd;
                            using (DataSet ds = new DataSet())
                            {
                                da.Fill(ds, "table1");
                                return ds.Tables["table1"];
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public DataTable Reuturn_Datatable_Search(string storedProcedureName, PassDataToSql[] ParaList, ref SqlTransaction trns)
        {
            try
            {
                SqlParameter cmdpara;
                using (db = new SqlConnection())
                {
                    db.ConnectionString = str_ConnetionString;
                    db.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        using (cmd = new SqlCommand())
                        {
                            cmd.Connection = trns.Connection;
                            cmd.Transaction = trns;
                            cmd.CommandText = storedProcedureName;
                            cmd.CommandType = CommandType.StoredProcedure;
                            for (int i = 0; i <= ParaList.GetUpperBound(0); i++)
                            {
                                cmdpara = new SqlParameter();

                                if (ParaList[i].ParaType.Equals(PassDataToSql.ParaTypeList.Nvarchar))
                                    cmdpara.Size = 250;
                                else
                                    if (ParaList[i].ParaType.Equals(PassDataToSql.ParaTypeList.Float))
                                {
                                    cmdpara.Offset = 18;
                                    cmdpara.Precision = 2;
                                }
                                cmdpara.ParameterName = ParaList[i].ParaName.ToString();
                                cmdpara.Value = ParaList[i].ParaValue;
                                cmdpara.SqlDbType = ParaList[i].GetParaType(ParaList[i].ParaType);
                                cmdpara.Direction = ParaList[i].GetParaDirection(ParaList[i].ParaDirection);

                                cmd.Parameters.Add(cmdpara);
                            }
                            da.SelectCommand = cmd;
                            using (DataSet ds = new DataSet())
                            {
                                da.Fill(ds, "table1");
                                return ds.Tables["table1"];
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public bool Is_Exists(string str_SqlCommand)
        {
            try
            {
                using (db = new SqlConnection())
                {
                    db.ConnectionString = str_ConnetionString;
                    db.Open();
                    using (cmd = new SqlCommand())
                    {
                        cmd.Connection = db;
                        cmd.CommandText = str_SqlCommand;
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows == true)
                        {
                            dr.Close();
                            return true;
                        }
                        else
                        {
                            dr.Close();
                            return false;
                        }
                    }
                }
            }

            catch
            {
                throw;
            }
        }

        internal DataTable return_DataTable(string p, PassDataToSql[] passDataToSql)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public DataTable return_NameNID(string args, string table, string where)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = str_ConnetionString;
                    DataSet ds = new DataSet();
                    con.Open();
                    DataTable dt = new DataTable();
                    string s = "select " + args + " from " + table + " where " + where + "";
                    using (SqlDataAdapter da = new SqlDataAdapter(s, con))
                    {
                        da.Fill(ds);
                        dt = ds.Tables[0];
                    }

                    con.Close();
                    return dt;
                }
            }
            catch (Exception error)
            {

            }
            return null;

        }

        public clsMasterCompany getMasterCompanyInfo(string registationCode, string strBaseApiAddress)
        {
            clsMasterCompany cls = new clsMasterCompany();
            try
            {
                ServicePointManager.Expect100Continue = true;
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 ;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;


                
                //ConfigurationManager.AppSettings["masterDbApiBaseAddress"].ToString();
                //var baseAddress = "http://192.168.1.72/pegasustech/index.php/api/company_detail/get_detail";
                //var baseAddress = ConfigurationManager.AppSettings["masterDbApiBaseAddress"].ToString();
                var baseAddress = strBaseApiAddress;
                var http = (System.Net.HttpWebRequest)WebRequest.Create(new Uri(baseAddress));
                http.Accept = "application/json; charset=utf-8";
                //http.ContentType = "application/json";
                http.ContentType = "application/x-www-form-urlencoded";
                http.Method = "POST";
                string parsedContent = "registration_code=" + registationCode + "&master_product_id=" + ConfigurationManager.AppSettings["master_product_id"].ToString();
                //string parsedContent = "registration_code=" + registationCode;
                ASCIIEncoding encoding = new ASCIIEncoding();
                Byte[] bytes = encoding.GetBytes(parsedContent);

                Stream newStream = http.GetRequestStream();
                newStream.Write(bytes, 0, bytes.Length);
                newStream.Close();

                var response = (HttpWebResponse)http.GetResponse();
                var stream = response.GetResponseStream();
                var sr = new StreamReader(stream);
                var content = sr.ReadToEnd();
                objPhpAPIResult objPhpApiResult = JsonConvert.DeserializeObject<objPhpAPIResult>(content);
                if (objPhpApiResult.status == true)
                {
                    cls = objPhpApiResult.result;
                }
                else
                {
                    cls = null;
                }
                return cls;

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public clsMasterCompany validateLicenseKey(string strLicenseKey, string strBaseApiAddress, string strProductCode)
        {
            clsMasterCompany cls = new clsMasterCompany();
            try
            {
                //get CPU ID
                string sProcessorID = "";
                string sQuery = "SELECT ProcessorId FROM Win32_Processor";
                //var baseAddress = ConfigurationManager.AppSettings["masterDbApiBaseAddress"].ToString() + "/company_detail/get_detail";
                var baseAddress = strBaseApiAddress;
                var http = (System.Net.HttpWebRequest)WebRequest.Create(new Uri(baseAddress));
                http.Accept = "application/json; charset=utf-8";
                //http.ContentType = "application/json";
                http.ContentType = "application/x-www-form-urlencoded";
                http.Method = "POST";
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                string parsedContent = "license_key=" + strLicenseKey + "&product_code=" + strProductCode + "&machine_id=" + sProcessorID;
                ASCIIEncoding encoding = new ASCIIEncoding();
                Byte[] bytes = encoding.GetBytes(parsedContent);

                Stream newStream = http.GetRequestStream();
                newStream.Write(bytes, 0, bytes.Length);
                newStream.Close();

                var response = (HttpWebResponse)http.GetResponse();
                var stream = response.GetResponseStream();
                var sr = new StreamReader(stream);
                var content = sr.ReadToEnd();
                objPhpAPIResult objPhpApiResult = JsonConvert.DeserializeObject<objPhpAPIResult>(content);
                if (objPhpApiResult.status == true)
                {
                    cls = objPhpApiResult.result;
                }
                else
                {
                    cls = null;
                }
                return cls;

            }
            catch (Exception ex)
            {
                return null;
            }

        }




        public class objPhpAPIResult
        {
            public bool status { get; set; }
            public clsMasterCompany result { get; set; }
            public string message { get; set; }
        }

        [Serializable]
        public class clsMasterCompany
        {
            //public string company_id { get; set; }
            public string registration_code { get; set; }
            public string company_name { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public string country_id { get; set; }
            public string country_name { get; set; }
            public string hostname { get; set; }
            public string username { get; set; }
            public string db_password { get; set; }
            public string database { get; set; }
            public string port { get; set; }
            public string password { get; set; }
            public string expiry_date { get; set; }
            public string att_device_count { get; set; }
            public string version_type { get; set; }
            public string license_key { get; set; }
            public string user { get; set; }
            public string no_of_employee { get; set; }
            public string product_code { get; set; }
            public string es_database { get; set; }

        }
    }
}