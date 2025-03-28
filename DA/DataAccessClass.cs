using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Web;

namespace PegasusDataAccess
{
    public class DataAccessClass
    {



        private SqlConnection db;
        private SqlCommand cmd;
        //public string HttpContext.Current.Session["DBConnection"].ToString();
        public SqlConnection con;
        /********* Constructor will assign Connection string to variables *******/
        private string _strConString = string.Empty;
        public static int ab = 0;
        public DataAccessClass(string strConString)
        {
            {
                try
                {
                    ab = ab + 1;
                    _strConString = strConString;

                }
                catch (Exception ex)
                {

                }
            }
        }

        public string get_SingleValue(string sql, string conString = "")
        {
            try
            {
                string str_ReturnValue;
                using (db = new SqlConnection())
                {
                    // db.ConnectionString = Session["DBConnection"].ToString();
                    if (!string.IsNullOrEmpty(conString))
                    {
                        db.ConnectionString = conString;
                    }
                    else
                    {
                        //db.ConnectionString = HttpContext.Current.Session["DBConnection"].ToString();
                        db.ConnectionString = _strConString;
                    }

                    //db.ConnectionString = HttpContext.Current.Session["DBConnection"].ToString();
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
                    //db.ConnectionString = HttpContext.Current.Session["DBConnection"].ToString();
                    db.ConnectionString = _strConString;
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

                //db.ConnectionString = HttpContext.Current.Session["DBConnection"].ToString();
                //db.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = trns.Connection;
                    cmd.Transaction = trns;
                    cmd.CommandText = str_Sql;
                    cmd.CommandTimeout = 2000;
                    i = cmd.ExecuteNonQuery();
                }

                return i;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public int execute_CommandWithParams(string str_Sql, PassDataToSql[] ParaList, ref SqlTransaction trns, string conString = "")
        {
            try
            {
                int b;
                SqlParameter cmdpara;
                using (db = new SqlConnection())
                {
                    if (!string.IsNullOrEmpty(conString))
                    {
                        db.ConnectionString = conString;
                    }
                    else
                    {
                        //db.ConnectionString = HttpContext.Current.Session["DBConnection"].ToString();
                        db.ConnectionString = _strConString;
                    }

                    //db.ConnectionString = Session["DBConnection"].ToString();
                    db.Open();

                    using (cmd = new SqlCommand())
                    {
                        cmd.Connection = trns.Connection;
                        cmd.Transaction = trns;
                        cmd.CommandText = str_Sql;
                        //cmd.CommandType = CommandType.StoredProcedure;

                        for (int i = 0; i <= ParaList.GetUpperBound(0); i++)
                        {
                            cmdpara = new SqlParameter();

                            if (ParaList[i].ParaType.Equals(PassDataToSql.ParaTypeList.Nvarchar))
                                cmdpara.Size = 50000;
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
                        b = cmd.ExecuteNonQuery();
                    }
                    return b;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int execute_Command(string str_Sql, string conString = "")
        {
            try
            {
                int i;
                using (db = new SqlConnection())
                {
                    if (!string.IsNullOrEmpty(conString))
                    {
                        db.ConnectionString = conString;
                    }
                    else
                    {
                        //db.ConnectionString = HttpContext.Current.Session["DBConnection"].ToString();
                        db.ConnectionString = _strConString;
                    }
                    //db.ConnectionString = HttpContext.Current.Session["DBConnection"].ToString();
                    //db.ConnectionString = Session["DBConnection"].ToString();
                    db.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = db;
                        cmd.CommandText = str_Sql;
                        cmd.CommandTimeout = 2000;
                        i = cmd.ExecuteNonQuery();
                    }
                }
                return i;
            }
            catch
            {
                throw;
            }
        }

        public string execute_Sp(string str_Sql, PassDataToSql[] ParaList, string conString = "")
        {
            //try
            //{
            SqlParameter cmdpara;
            SqlParameter cmdpara_return;
            string str_Res = "";
            using (db = new SqlConnection())
            {
                // db.ConnectionString = Session["DBConnection"].ToString();
                if (!string.IsNullOrEmpty(conString))
                {
                    db.ConnectionString = conString;
                }
                else
                {
                    //db.ConnectionString = HttpContext.Current.Session["DBConnection"].ToString();
                    db.ConnectionString = _strConString;
                }
                //db.ConnectionString = HttpContext.Current.Session["DBConnection"].ToString();
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
                //db.ConnectionString = HttpContext.Current.Session["DBConnection"].ToString();
                db.ConnectionString = _strConString;

                //db.ConnectionString = Session["DBConnection"].ToString();
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
                    //db.ConnectionString = HttpContext.Current.Session["DBConnection"].ToString();
                    db.ConnectionString = _strConString;
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
                    //db.ConnectionString = HttpContext.Current.Session["DBConnection"].ToString();
                    db.ConnectionString = _strConString;

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

        public DataTable return_DataTable(string str_SqlCommand, string conString = "")
        {
            try
            {
                ;
                using (db = new SqlConnection())
                {
                    if (!string.IsNullOrEmpty(conString))
                    {
                        db.ConnectionString = conString;
                    }
                    else
                    {
                        //db.ConnectionString = HttpContext.Current.Session["DBConnection"].ToString();
                        db.ConnectionString = _strConString;
                    }

                    //db.ConnectionString = Session["DBConnection"].ToString();
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

        public DataTable GeTOracleRecord(string strQuery, string strConenction)
        {
            DataTable dt = new DataTable();
            OleDbConnection oCon = new OleDbConnection(strConenction);
            OleDbDataAdapter oAdp = new OleDbDataAdapter("", "");
            //apps.Tabs
            oAdp.SelectCommand.CommandText = strQuery;
            oAdp.SelectCommand.Connection = oCon;
            dt = new DataTable();
            oAdp.Fill(dt);
            return dt;

        }


        public DataTable return_DataTable_Using_Connectionstring(string str_SqlCommand, string strConnectionstring)
        {
            try
            {
                using (db = new SqlConnection())
                {
                    db.ConnectionString = strConnectionstring;
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
                    db.ConnectionString = HttpContext.Current.Session["DBConnection"].ToString();
                    //db.ConnectionString = _strConString;



                    //db.ConnectionString = Session["DBConnection"].ToString();
                    db.Open();
                    SqlDataReader reader_Temp;
                    using (cmd = new SqlCommand())
                    {
                        DataTable Table_Temp = new DataTable();
                        try
                        {
                            cmd.Connection = trns.Connection;
                            cmd.Transaction = trns;
                            cmd.CommandText = str_SqlCommand;
                            cmd.CommandType = CommandType.Text;
                            reader_Temp = cmd.ExecuteReader();
                            try
                            {
                                if (reader_Temp.HasRows == true)
                                {
                                    Table_Temp.Load(reader_Temp);
                                }
                            }
                            catch
                            {

                            }

                            reader_Temp.Close();
                            return Table_Temp;
                        }
                        catch
                        {
                            //reader_Temp.Close();
                            return Table_Temp;
                        }


                        //}
                        //else
                        //{
                        //    reader_Temp.Close();
                        //    return Table_Temp;
                        //}
                    }
                }
            }
            catch (Exception ex)
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
                    //db.ConnectionString = HttpContext.Current.Session["DBConnection"].ToString();
                    db.ConnectionString = _strConString;
                    //db.ConnectionString = Session["DBConnection"].ToString();
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
        public DataTable Reuturn_Datatable_Search(string storedProcedureName, PassDataToSql[] ParaList, string conString = "")
        {
            try
            {
                SqlParameter cmdpara;
                using (db = new SqlConnection())
                {
                    if (!string.IsNullOrEmpty(conString) && conString != "System.Data.SqlClient.SqlConnection")
                    {
                        db.ConnectionString = conString;
                    }
                    else
                    {
                        //db.ConnectionString = HttpContext.Current.Session["DBConnection"].ToString();
                        db.ConnectionString = _strConString;
                    }

                    //db.ConnectionString = Session["DBConnection"].ToString();
                    db.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        using (cmd = new SqlCommand())
                        {
                            cmd.Connection = db;
                            cmd.CommandText = storedProcedureName;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 1200;

                            for (int i = 0; i <= ParaList.GetUpperBound(0); i++)
                            {
                                cmdpara = new SqlParameter();

                                if (ParaList[i].ParaType.Equals(PassDataToSql.ParaTypeList.Nvarchar))
                                    cmdpara.Size = 50000;
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
                            //using (SqlDataReader sqlRd = cmd.ExecuteReader())
                            //{
                            //    using (DataTable dt = new DataTable())
                            //    {
                            //        dt.Load(sqlRd);
                            //        return dt;
                            //    }
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
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
                    //db.ConnectionString = HttpContext.Current.Session["DBConnection"].ToString();
                    db.ConnectionString = _strConString;
                    // db.ConnectionString = Session["DBConnection"].ToString();
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
                            using (SqlDataReader sqlRd = cmd.ExecuteReader())
                            {
                                using (DataTable dt = new DataTable())
                                {
                                    dt.Load(sqlRd);

                                    return dt;
                                }
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
                    //db.ConnectionString = HttpContext.Current.Session["DBConnection"].ToString();
                    db.ConnectionString = _strConString;
                    //db.ConnectionString = Session["DBConnection"].ToString();
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
                    //con.ConnectionString = HttpContext.Current.Session["DBConnection"].ToString();
                    con.ConnectionString = _strConString;
                    // con.ConnectionString = Session["DBConnection"].ToString();
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

        public DataTable GetDtFromQryWithParam(string sql, PassDataToSql[] ParaList, string conString = "")
        {
            try
            {
                SqlParameter cmdpara;
                using (db = new SqlConnection())
                {
                    if (!string.IsNullOrEmpty(conString))
                    {
                        db.ConnectionString = conString;
                    }
                    else
                    {
                        //db.ConnectionString = HttpContext.Current.Session["DBConnection"].ToString();
                        db.ConnectionString = _strConString;
                    }

                    //db.ConnectionString = Session["DBConnection"].ToString();
                    db.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        using (cmd = new SqlCommand())
                        {
                            cmd.Connection = db;
                            cmd.CommandText = sql;
                            //cmd.CommandType = CommandType.StoredProcedure;

                            for (int i = 0; i <= ParaList.GetUpperBound(0); i++)
                            {
                                cmdpara = new SqlParameter();

                                if (ParaList[i].ParaType.Equals(PassDataToSql.ParaTypeList.Nvarchar))
                                    cmdpara.Size = 50000;
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
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataTable GetDtFromQryWithParam(string sql, PassDataToSql[] ParaList, SqlTransaction trns, string conString = "")
        {
            try
            {
                SqlParameter cmdpara;
                using (db = new SqlConnection())
                {
                    if (!string.IsNullOrEmpty(conString))
                    {
                        db.ConnectionString = conString;
                    }
                    else
                    {
                        //db.ConnectionString = HttpContext.Current.Session["DBConnection"].ToString();
                        db.ConnectionString = _strConString;
                    }

                    //db.ConnectionString = Session["DBConnection"].ToString();
                    db.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        using (cmd = new SqlCommand())
                        {
                            cmd.Connection = trns.Connection;
                            cmd.Transaction = trns;
                            cmd.CommandText = sql;
                            //cmd.CommandType = CommandType.StoredProcedure;

                            for (int i = 0; i <= ParaList.GetUpperBound(0); i++)
                            {
                                cmdpara = new SqlParameter();

                                if (ParaList[i].ParaType.Equals(PassDataToSql.ParaTypeList.Nvarchar))
                                    cmdpara.Size = 50000;
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
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}