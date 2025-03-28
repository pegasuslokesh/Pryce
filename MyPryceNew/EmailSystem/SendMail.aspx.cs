using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

public partial class EmailSystem_SendMail : System.Web.UI.Page
{
    #region Class Object
    ES_SendMailHeader Obj_SendMailHeader = null;
    UserMaster ObjUserMaster = null;
    SendMailSms ObjSendMailSms = null;
    ES_SendMailDetail OBj_SenddMailDetail = null;
    Set_AddressMaster Obj_AddressMaster = null;
    Set_AddressChild Obj_AddressChild = null;
    ES_Attachment Obj_Attachment = null;
    Ems_ContactMaster ObjContactMaster = null;
    ES_RefereneceMailArchaving objRefaArc = null;
    Ems_GroupMaster ObjGroupMaster = null;
    Inv_SalesQuotationDetail objsqdetail = null;
    Inv_ProductMaster ObjProductMaster = null;
    Ems_MailMarketing objMailMarket = null;
    Ems_TemplateMaster objTemplate = null;
    ES_MailInboxHeader ObjMailBoxHeader = null;
    UserDetail objUserDetail = null;
    ES_EmailMaster_Header objEMHeader = null;
    ES_EmailMasterDetail objEMDetail = null;
    Arc_Directory_Master objDir = null;
    Arc_FileTransaction ObjFile = null;
    #endregion
    #region  System Defined Function
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        btnSendMail.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSendMail, "").ToString());

        Obj_SendMailHeader = new ES_SendMailHeader(HttpContext.Current.Session["DBConnection_ES"].ToString());
        ObjUserMaster = new UserMaster(Session["DBConnection"].ToString());
        ObjSendMailSms = new SendMailSms(Session["DBConnection"].ToString());
        OBj_SenddMailDetail = new ES_SendMailDetail(HttpContext.Current.Session["DBConnection_ES"].ToString());
        Obj_AddressMaster = new Set_AddressMaster(Session["DBConnection"].ToString());
        Obj_AddressChild = new Set_AddressChild(Session["DBConnection"].ToString());
        Obj_Attachment = new ES_Attachment(HttpContext.Current.Session["DBConnection_ES"].ToString());
        ObjContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objRefaArc = new ES_RefereneceMailArchaving(HttpContext.Current.Session["DBConnection_ES"].ToString());
        ObjGroupMaster = new Ems_GroupMaster(Session["DBConnection"].ToString());
        objsqdetail = new Inv_SalesQuotationDetail(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objMailMarket = new Ems_MailMarketing(Session["DBConnection"].ToString());
        objTemplate = new Ems_TemplateMaster(Session["DBConnection"].ToString());
        ObjMailBoxHeader = new ES_MailInboxHeader(HttpContext.Current.Session["DBConnection_ES"].ToString());
        objUserDetail = new UserDetail(Session["DBConnection"].ToString());
        objEMHeader = new ES_EmailMaster_Header(Session["DBConnection"].ToString());
        objEMDetail = new ES_EmailMasterDetail(Session["DBConnection"].ToString());
        objDir = new Arc_Directory_Master(Session["DBConnection"].ToString());
        ObjFile = new Arc_FileTransaction(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            FillUserEmailAccount();
            if (ddlEmailUser.Items.Count != 0)
            {
                fillEmail();
            }
            else
            {
                DisplayMessage("Configure Your Mail First");
                return;
            }

            if (Request.QueryString["HD"]!=null)
            {
                txtMailTo.Text = Session["hdnEmailIDS"].ToString();
                string data = txtMessage.Content;
                txtMessage.Content = Session["hdnBody"].ToString() + data;
            }
            // coming from report system
            if (Request.QueryString["RS"] != null)
            {
                txtMailTo.Text = Session["hdnEmailIDS"].ToString();
                txtMessage.Content = txtMessage.Content;
                txtSubject.Text = Session["MailSubject"].ToString();
                DataTable dt = new DataTable();
                dt.Columns.Add("TransId");
                dt.Columns.Add("FilePath");
                dt.Columns.Add("File_Name");
                dt.Rows.Add();
                ViewState["dt"] = dt;
                dt.Rows[dt.Rows.Count - 1]["TransId"] = dt.Rows.Count;
                dt.Rows[dt.Rows.Count - 1]["File_Name"] = Session["MailSubject"].ToString()+".pdf";
                dt.Rows[dt.Rows.Count - 1]["FilePath"] = Session["mailAttachmentPath"].ToString();
                Gvmultiple.DataSource = dt;
                Gvmultiple.DataBind();
            }
        }
    }

    public void fillEmail()
    {
        DataTable DTuSER = new DataView(objUserDetail.GetbyUserId(Session["UserId"].ToString(), Session["CompId"].ToString()), "Email='" + ddlEmailUser.SelectedItem.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (DTuSER.Rows.Count != 0)
        {

            txtMessage.Content = "<br/><br/><br/><br/><br/><br/><br/>" + DTuSER.Rows[0]["Signature"].ToString();
            var imgSrcMatches = System.Text.RegularExpressions.Regex.Matches(DTuSER.Rows[0]["Signature"].ToString(), string.Format(@"<\s*img\s*src\s*=\s*{0}\s*([^{0}]+)\s*{0}", "\""), RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Multiline);
            string strEmailLinkPath = string.Empty;
            foreach (Match match in imgSrcMatches)
            {
                strEmailLinkPath = match.Groups[1].Value + "," + strEmailLinkPath;
            }

            ViewState["FilePathLink"] = strEmailLinkPath;
        }

        if (Request.QueryString["RT"] != null && Request.QueryString["Id"] != null)
        {
            string[] strmail = new string[4];

            DataTable dt = new DataView((DataTable)Session["DtInbox"], "Trans_Id='" + Request.QueryString["Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count != 0)
            {

                strmail = dt.Rows[0]["From"].ToString().Replace("<", "").Replace(">", "").Split(' ');
                txtMailTo.Text = strmail[strmail.Length - 1].Replace("&#x0D", "").ToString().Replace("&lt;", "").Replace("&gt;", "");

                txtSubject.Text = "RE:" + dt.Rows[0]["Subject"].ToString();
                txtMessage.ActiveMode = AjaxControlToolkit.HTMLEditor.ActiveModeType.Html;
                string str = string.Empty;
                if (dt.Rows[0]["To"].ToString() != "")
                {
                    str = "<br/><span style='font-weight: bold'>TO :</span>" + dt.Rows[0]["To"].ToString();

                }
                if (dt.Rows[0]["Cc"].ToString() != "")
                {
                    str += "<br/><span style='font-weight: bold'>CC :</span>" + dt.Rows[0]["Cc"].ToString();

                }
                if (dt.Rows[0]["Subject"].ToString() != "")
                {
                    str += "<br/><span style='font-weight: bold'>Subject :</span>" + dt.Rows[0]["Subject"].ToString();

                }
                if (dt.Rows[0]["Body"].ToString() != "")
                {
                    str += "<br/>" + dt.Rows[0]["Body"].ToString().Replace("&nb sp;", "");

                }

                if (DTuSER.Rows.Count != 0)
                {

                    txtMessage.Content = "<br/><br/><br/><br/><br/><br/><br/>" + DTuSER.Rows[0]["Signature"].ToString() + " <hr/><br/><span style='font-weight: bold'>From :</span>" + strmail[strmail.Length - 1].Replace("&#x0D", "").ToString() + "</br><span style='font-weight: bold'>Sent :</span>" + Convert.ToDateTime(dt.Rows[0]["Date"].ToString()).ToString() + str;
                }

                else
                {
                    txtMessage.Content = "<br/><br/><br/><br/><br/><br/><br/> <hr/><br/><span style='font-weight: bold'>From :</span>" + strmail[strmail.Length - 1].Replace("&#x0D", "").ToString() + "</br><span style='font-weight: bold'>Sent :</span>" + Convert.ToDateTime(dt.Rows[0]["Date"].ToString()).ToString() + str;
                }
                txtMessage.ActiveMode = AjaxControlToolkit.HTMLEditor.ActiveModeType.Design;
                txtMessage.Focus();

            }
            if (Request.QueryString["RT"].ToString() == "REA")
            {
                string[] StrUserName = GetUserNamePassEmpId();

                string Str = string.Empty;

                foreach (string strCc in dt.Rows[0]["Cc"].ToString().Split(';'))
                {
                    strmail = strCc.ToString().Split(' ');
                    if (!strmail[strmail.Length - 1].ToString().ToLower().Contains(StrUserName[1].ToString().ToLower().Trim()))
                    {
                        if (strmail[strmail.Length - 1].ToString().Trim().Replace("&#x0D", "").ToString() != "")
                        {
                            Str += strmail[strmail.Length - 1].ToString().Trim().Replace("&#x0D", "") + ";";
                        }
                    }
                }
                txtCC.Text = Str.ToString();
                Str = string.Empty;
                foreach (string strTo in dt.Rows[0]["To"].ToString().Split(';'))
                {
                    strmail = strTo.ToString().Split(' ');
                    if (!strmail[strmail.Length - 1].ToString().ToLower().Contains(StrUserName[1].ToString().ToLower().Trim()))
                    {
                        if (strmail[strmail.Length - 1].ToString().Trim().Replace("&#x0D", "").ToString() != "")
                        {
                            Str += strmail[strmail.Length - 1].ToString().Trim().Replace("&#x0D", "") + ";";
                        }
                    }
                }
                txtMailTo.Text += Str.ToString();
            }


        }

        if (Request.QueryString["FW"] != null && Request.QueryString["Id"] != null)
        {
            DataTable dt = new DataView((DataTable)Session["DtInbox"], "Trans_Id='" + Request.QueryString["Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count != 0)
            {
                string str = string.Empty;
                string[] strmail = dt.Rows[0]["Field1"].ToString().Split(' ');
                if (dt.Rows[0]["To"].ToString() != "")
                {
                    str = "<br/><span style='font-weight: bold'>TO :</span>" + dt.Rows[0]["To"].ToString();

                }
                if (dt.Rows[0]["Cc"].ToString() != "")
                {
                    str += "<br/><span style='font-weight: bold'>CC :</span>" + dt.Rows[0]["Cc"].ToString();

                }
                if (dt.Rows[0]["Subject"].ToString() != "")
                {
                    str += "<br/><span style='font-weight: bold'>Subject :</span>" + dt.Rows[0]["Subject"].ToString();

                }
                if (dt.Rows[0]["Body"].ToString() != "")
                {
                    str += "<br/>" + dt.Rows[0]["Body"].ToString().Replace("&nb sp;", "");

                }
                if (DTuSER.Rows.Count != 0)
                {


                    try
                    {

                        txtMessage.Content = "<br/><br/><br/><br/><br/><br/><br/>" + DTuSER.Rows[0]["Signature"].ToString() + "<hr/><br/><span style='font-weight: bold'>From :</span>" + strmail[strmail.Length - 1].Replace("&#x0D", "").ToString() + "</br><span style='font-weight: bold'>Sent :</span>" + Convert.ToDateTime(dt.Rows[0]["Date"].ToString()).ToString() + str;
                    }
                    catch
                    {
                        txtMessage.Content = "<br/><br/><br/><br/><br/><br/><br/>" + DTuSER.Rows[0]["Signature"].ToString() + "<hr/><br/><span style='font-weight: bold'>From :</span>" + strmail[strmail.Length - 1].Replace("&#x0D", "").ToString() + "</br><span style='font-weight: bold'>Sent :</span>" + Convert.ToDateTime(dt.Rows[0]["ModifiedDate"].ToString()).ToString() + str;

                    }
                }

                else
                {
                    try
                    {

                        txtMessage.Content = "<br/><br/><br/><br/><br/><br/><br/><hr/><br/><span style='font-weight: bold'>From :</span>" + strmail[strmail.Length - 1].Replace("&#x0D", "").ToString() + "</br><span style='font-weight: bold'>Sent :</span>" + Convert.ToDateTime(dt.Rows[0]["Date"].ToString()).ToString() + str;
                    }
                    catch
                    {
                        txtMessage.Content = "<br/><br/><br/><br/><br/><br/><br/><hr/><br/><span style='font-weight: bold'>From :</span>" + strmail[strmail.Length - 1].Replace("&#x0D", "").ToString() + "</br><span style='font-weight: bold'>Sent :</span>" + Convert.ToDateTime(dt.Rows[0]["ModifiedDate"].ToString()).ToString() + str;

                    }
                }

                txtMessage.ActiveMode = AjaxControlToolkit.HTMLEditor.ActiveModeType.Design;
                txtMessage.Focus();
                txtSubject.Text = "FW:" + dt.Rows[0]["Subject"].ToString().Replace("&n bsp;", "");
            }
            if (Request.QueryString["Page"] != null)
            {
                string Str = string.Empty;
                if (Request.QueryString["Page"].ToString() == "1")
                {
                    Str = "R";
                }
                else
                {
                    Str = "S";
                }
                FillDownloadList(Str, Request.QueryString["Id"].ToString());

            }

        }
        if (Request.QueryString["Page"] != null && Request.QueryString["Url"] != null)
        {

            if (DTuSER.Rows.Count != 0)
            {

                txtMessage.Content = "<br/><br/><br/><br/><br/><br/><br/>" + DTuSER.Rows[0]["Signature"].ToString();
            }

            try
            {
                if (Request.QueryString["Page"].ToString().Trim() == "SQ")
                {
                    try
                    {
                        
                      txtSubject.Text = ObjContactMaster.GetContactTrueById(Request.QueryString["ConId"].ToString().Trim()).Rows[0]["Name"].ToString() + " ( " + Session["MailSubject"].ToString() + " )";
                       
                    }
                    catch
                    {
                        txtSubject.Text = Session["MailSubject"].ToString();

                    }
                }
                else
                {

                    txtSubject.Text = Session["MailSubject"].ToString();
                }
            }
            catch
            {

            }
            DataTable dt = new DataTable();
            dt.Columns.Add("TransId");
            dt.Columns.Add("FilePath");
            dt.Columns.Add("File_Name");

            dt.Rows.Add();
            dt.Rows[dt.Rows.Count - 1]["TransId"] = dt.Rows.Count;
            dt.Rows[dt.Rows.Count - 1]["File_Name"] = Request.QueryString["Url"].ToString();
            dt.Rows[dt.Rows.Count - 1]["FilePath"] = Server.MapPath("~/Temp/" + Request.QueryString["Url"].ToString()).ToString();
            ViewState["dt"] = dt;

        }

        if (Request.QueryString["IsRefCont"] != null)
        {
            try
            {
                Inv_ReferenceMailContact ObjMailrefer = new Inv_ReferenceMailContact(HttpContext.Current.Session["DBConnection_ES"].ToString());
                DataTable dtEmail = ObjMailrefer.GetRecord_By_RefType_and_RefId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Request.QueryString["Page"].ToString(), Request.QueryString["Id"].ToString(), Session["DBConnection"].ToString());
                foreach (DataRow dr in dtEmail.Rows)
                {
                    if (dr["Field1"].ToString() != "")
                    {
                        txtMailTo.Text += dr["Field1"].ToString() + ";";
                    }

                }
            }
            catch
            {

            }
        }

        if (Request.QueryString["IsRefArch"] != null)
        {
            DataTable dt = (DataTable)ViewState["dt"];
            DataTable dtRefId = objRefaArc.GetAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Request.QueryString["Page"].ToString(), Request.QueryString["Id"].ToString(), Session["DBConnection"].ToString());
            for (int j = 0; j < dtRefId.Rows.Count; j++)
            {
                dt.Rows.Add();
                dt.Rows[dt.Rows.Count - 1]["TransId"] = dt.Rows.Count;
                dt.Rows[dt.Rows.Count - 1]["File_Name"] = dtRefId.Rows[j]["File_Name"].ToString();
                try
                {
                    dt.Rows[dt.Rows.Count - 1]["FilePath"] = Server.MapPath("~/ArcaWing/" + dtRefId.Rows[j]["Directory_Name"].ToString() + "/" + dtRefId.Rows[j]["File_Name"].ToString()).ToString();
                }
                catch
                {
                }
            }
            ViewState["dt"] = dt;
        }
        try
        {
            if (Session["Page&MailType&Id"] != null)
            {
                string[] split = Session["Page&MailType&Id"].ToString().Split(',');
                if (split.First() == "EMS")
                {
                    //  Bind Email Id For Selected Contact.............
                    DataTable dtContactMail = objMailMarket.GetRecordDetail(split.Last().ToString(), "4");
                    if (split[1].ToString() == "Failure")
                    {
                        dtContactMail = new DataView(dtContactMail, "Field1='False'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    else if (split[1].ToString() == "Success")
                    {
                        dtContactMail = new DataView(dtContactMail, "Field1='True'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    // Bind 
                    DataTable dt = objMailMarket.GetRecordHeader(split.Last().ToString(), "6");
                    if (dtContactMail.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtContactMail.Rows.Count; i++)
                        {
                            if (dtContactMail.Rows[i]["EMailId"].ToString() != "")
                            {
                                txtMailTo.Text += dtContactMail.Rows[i]["EMailId"].ToString() + ";";
                            }
                        }
                        // 
                        //if (dt.Rows[0]["Template_Type"].ToString() == "1")
                        // {
                        //dt = objTemplate.GetRecord("0", dt.Rows[0]["Template_Name"].ToString(), "5");
                        dt = objTemplate.GetRecord(dt.Rows[0]["TempId"].ToString(), "", "7");
                        txtMessage.Content = dt.Rows[0]["Template_Content"].ToString();
                        //}
                        // else
                        // {
                        //      txtMessage.Content = Encoding.ASCII.GetString((byte[])(Byte[])dt.Rows[0]["File_Data"]);
                        // }

                    }
                }
            }
        }
        catch (Exception Ex)
        {
        }
        Gvmultiple.DataSource = (DataTable)ViewState["dt"];
        Gvmultiple.DataBind();

        Value = "1";
        string str_html = "";
        if (Session["RV_AGE_Message"] != null && Request.QueryString["Page"].ToString()== "RV_AGE")
        {
            str_html = Session["RV_AGE_Message"].ToString();
            txtMessage.Content = str_html + txtMessage.Content;
        }
       

    }

    private bool IsValidEmailId(string InputEmail)
    {
        //Regex To validate Email Address
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Match match = regex.Match(InputEmail);
        if (match.Success)
            return true;
        else
            return false;
    }
 

    protected void btnSendMail_Click(object sender, EventArgs e)
     {
        

        //here we are checking that mentioned email id is valid or not 


        if (txtMailTo.Text.Trim() == "")
        {
            DisplayMessage("Enter To Email Id");
            txtMailTo.Focus();
            return;
        }
        else
        {
            foreach (string s in txtMailTo.Text.Split(';'))
            {
                if (s.Trim() != "")
                {
                    if (!IsValidEmailId(s.Trim()))
                    {
                        DisplayMessage("Enter Valid To Email-Id(" + s.Trim().Replace("'"," ")+")");
                        return;
                    }
                }
            }
        }




        if (txtCC.Text.Trim() != "")
        {
            foreach (string s in txtCC.Text.Split(';'))
            {
                if (s.Trim() != "")
                {
                    if (!IsValidEmailId(s.Trim()))
                    {
                        DisplayMessage("Enter Valid CC Email-Id(" + s.Trim().Replace("'", " ") + ")");
                        return;
                    }
                }
            }
        }


        if (txtBcc.Text.Trim() != "")
        {
            foreach (string s in txtBcc.Text.Split(';'))
            {
                if (s.Trim() != "")
                {
                    if (!IsValidEmailId(s.Trim()))
                    {
                        DisplayMessage("Enter Valid BCC Email-Id(" + s.Trim().Replace("'", " ") + ")");
                        return;
                    }
                }
            }
        }


        //new Code

        ASCIIEncoding encoding = new ASCIIEncoding();


        byte[] bytesin = encoding.GetBytes(txtMessage.Content);
        double lengthin = ((txtMessage.Content.Length));
        ViewState["TotalLength"] = Convert.ToDouble(lengthin.ToString());
        if (ViewState["dt"] != null)
        {

            foreach (DataRow dr in ((DataTable)ViewState["dt"]).Rows)
            {
                try
                {

                    string FilePath1 = dr["FilePath"].ToString();

                    FileInfo fileInfo1 = new FileInfo(FilePath1);

                    int ContantLenth1 = Convert.ToInt32(fileInfo1.Length);
                    ViewState["TotalLength"] = Convert.ToDouble((Convert.ToDouble(ViewState["TotalLength"].ToString()) + ContantLenth1));
                    //x = Convert.ToDouble(ViewState["TotalLength"].ToString());
                }

                catch
                {


                }
            }
        }
        //End Code
        if (ViewState["FilePathLink"] != null)
        {
            if (Session["FilePathLink"] != null)
            {
                Session["FilePathLink"] = Session["FilePathLink"].ToString() + ViewState["FilePathLink"].ToString();
            }
            else
            {
                Session["FilePathLink"] = ViewState["FilePathLink"].ToString();
            }
        }
        
        if (Value == null)
        {
            Value = "1";
        }
        if (Value.ToString() == "1")
        {
            Value = "2";
            try
            {
                if (txtMailTo.Text.Trim() == "")
                {
                    DisplayMessage("Enter To Email Id");
                    txtMailTo.Focus();
                    return;
                }
                else
                {
                    string StrCompId = string.Empty;
                    string StrBrandId = string.Empty;
                    string StrLocId = string.Empty;
                    try
                    {
                        StrCompId = Session["CompId"].ToString();
                    }
                    catch
                    {
                        StrCompId = "0";
                    }
                    try
                    {
                        StrBrandId = Session["BrandId"].ToString();
                    }
                    catch
                    {
                        StrBrandId = "0";
                    }
                    try
                    {
                        StrLocId = Session["LocId"].ToString();
                    }
                    catch
                    {
                        StrLocId = "0";
                    }

                    string StrFromId = string.Empty;
                    string StrUserName = string.Empty;
                    string StrPass = string.Empty;
                    try
                    {
                        string[] strUPEId = GetUserNamePassEmpId();
                        StrFromId = strUPEId[0].ToString();
                        StrUserName = strUPEId[1].ToString();
                        StrPass = strUPEId[2].ToString();

                    }
                    catch
                    {
                        StrFromId = "0";
                    }
                    string StrRef_Id = "0";
                    string StrRef_Type = "0";
                    if (Request.QueryString["PageRefId"] != null)
                    {
                        StrRef_Id = Request.QueryString["PageRefId"].ToString();
                    }
                    if (Request.QueryString["PageRefType"] != null)
                    {
                        StrRef_Type = Request.QueryString["PageRefType"].ToString();
                    }
                    int i = Obj_SendMailHeader.SendMail_Insert(StrCompId, StrBrandId, StrLocId, StrFromId, StrRef_Id, StrRef_Type, txtSubject.Text.ToString().Trim(), txtMessage.Content.Trim(), "O", StrUserName.ToString(), ViewState["TotalLength"].ToString(), "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    // i = 0;
                    if (i != 0)
                    {
                        try
                        {
                            string TransId = i.ToString();
                            FillChildTable(txtMailTo, StrCompId, StrBrandId, StrLocId, TransId.Trim(), "TO");
                            if (txtCC.Text.Trim() != "")
                            {
                                FillChildTable(txtCC, StrCompId, StrBrandId, StrLocId, TransId.Trim(), "CC");
                            }
                            if (txtBcc.Text.Trim() != "")
                            {
                                FillChildTable(txtBcc, StrCompId, StrBrandId, StrLocId, TransId.Trim(), "BCC");
                            }
                            string StrAttachement = string.Empty;
                            if (ViewState["dt"] != null)
                            {

                                foreach (DataRow dr in ((DataTable)ViewState["dt"]).Rows)
                                {
                                    try
                                    {
                                        //if (((DataTable)ViewState["dt"]).Columns.Count > 3)
                                        //{
                                        //    if (dr["Company_Id"].ToString() != "")
                                        //    {
                                        //        continue;
                                        //    }
                                        //}

                                        StrAttachement += dr["FilePath"].ToString() + ",";
                                        string fname = Path.GetFileName(dr["FilePath"].ToString());

                                        string ContantTranser = string.Empty;
                                        string AtachedFile = string.Empty;
                                        string FilePath = dr["FilePath"].ToString();

                                        FileInfo fileInfo = new FileInfo(FilePath);
                                        Byte[] bytes = new Byte[0];
                                        int ContantLenth = Convert.ToInt32(fileInfo.Length);
                                        // ViewState["TotalLength"] = Convert.ToDouble((Convert.ToDouble(ViewState["TotalLength"].ToString()) + ContantLenth));
                                        // x = x + ContantLenth;
                                        if (!IsBinaryFile(FilePath, ContantLenth))
                                        {
                                            var buffer = new char[ContantLenth];

                                            using (var sr = new StreamReader(FilePath))
                                            {
                                                int length = sr.Read(buffer, 0, ContantLenth);
                                                AtachedFile = new string(buffer, 0, length);
                                            }
                                            ContantTranser = "QUOTED-PRINTABLE";
                                        }
                                        else
                                        {
                                            FileStream fs = File.OpenRead(FilePath);
                                            BinaryReader br = new BinaryReader(fs);

                                            bytes = br.ReadBytes((Int32)fs.Length);

                                            AtachedFile = Convert.ToBase64String(bytes);
                                            ContantTranser = "BASE64";


                                        }
                                        Obj_Attachment.Insert_Attachment(StrCompId, "0", "0", "attachment;", ContantTranser, "", fname, AtachedFile, "O", TransId, "", "", "",dr["FilePath"].ToString(), "", true.ToString(), DateTime.Now.ToString(), true.ToString(), StrFromId, DateTime.Now.ToString(), StrFromId, DateTime.Now.ToString());

                                    }
                                    catch
                                    {

                                    }
                                }

                            }
                            bool b = false;
                            string EmpName = ObjUserMaster.GetUserMasterByUserId(Session["UserId"].ToString(), Session["LoginCompany"].ToString()).Rows[0]["EmpName"].ToString();

                            if (Request.QueryString["Page"] != null && Session["Page&MailType&Id"] != null)
                            {
                                string[] split = Session["Page&MailType&Id"].ToString().Split(',');
                                DataTable dtDetail = objMailMarket.GetRecordDetail(split.Last().ToString(), "4");
                                if (split[1].ToString() == "Failure")
                                {
                                    dtDetail = new DataView(dtDetail, "Field1='False'", "", DataViewRowState.CurrentRows).ToTable();
                                }
                                else if (split[1].ToString() == "Success")
                                {
                                    dtDetail = new DataView(dtDetail, "Field1='True'", "", DataViewRowState.CurrentRows).ToTable();
                                }
                                if (Request.QueryString["Page"].ToString() == "PI" || split.First().ToString().TrimEnd().Contains("EMS"))
                                {
                                    foreach (string ToEMailId in txtMailTo.Text.Split(';'))
                                    {
                                        if (ViewState["FilePathLink"] != null)
                                        {
                                            if (Session["FilePathLink"] != null)
                                            {
                                                Session["FilePathLink"] = Session["FilePathLink"].ToString() + ViewState["FilePathLink"].ToString();
                                            }
                                            else
                                            {
                                                Session["FilePathLink"] = ViewState["FilePathLink"].ToString();
                                            }
                                        }
                                        if (split.First().ToString().TrimEnd().Contains("EMS"))
                                        {
                                            try
                                            {
                                                EmpName = objMailMarket.GetRecordHeader(split.Last().ToString(), "5").Rows[0]["MailHeader"].ToString();
                                            }
                                            catch
                                            {
                                            }
                                        }
                                        if (ToEMailId != "")
                                        {
                                            try
                                            {
                                                b = ObjSendMailSms.SendMail(ToEMailId + ";", "", "", txtSubject.Text.Trim(), txtMessage.Content.Trim(), StrCompId, StrAttachement.ToString(), StrUserName, StrPass, EmpName, HttpContext.Current.Session["UserId"].ToString(), Session["FilePathLink"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                                                if (b == true)
                                                {
                                                    objMailMarket.CRUDDetailRecord("0", split.Last().ToString(), "0", "True", ToEMailId, "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "6");
                                                }
                                                else
                                                {
                                                    objMailMarket.CRUDDetailRecord("0", split.Last().ToString(), "0", "False", ToEMailId, "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "6");

                                                }
                                            }
                                            catch
                                            {

                                            }
                                        }
                                    }
                                }
                                else
                                {
                                   // StrAttachement = "http://37.34.226.169:81/pryce/images/Chrysanthemum.jpg";
                                    b = ObjSendMailSms.SendMail(txtMailTo.Text, txtCC.Text.Trim(), txtBcc.Text.Trim(), txtSubject.Text.Trim(), txtMessage.Content.Trim(), StrCompId, StrAttachement.ToString(), StrUserName, StrPass, EmpName, HttpContext.Current.Session["UserId"].ToString(), Session["FilePathLink"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                                }
                            }
                            else
                            {
                               // StrAttachement = "http://37.34.226.169:81/pryce/images/Chrysanthemum.jpg";
                                b = ObjSendMailSms.SendMail(txtMailTo.Text, txtCC.Text.Trim(), txtBcc.Text.Trim(), txtSubject.Text.Trim(), txtMessage.Content.Trim(), StrCompId, StrAttachement.ToString(), StrUserName, StrPass, EmpName, HttpContext.Current.Session["UserId"].ToString(), Session["FilePathLink"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                            }
                            if (b)
                            {
                                i = Obj_SendMailHeader.SendMail_UpdateStatus(TransId, StrCompId, StrBrandId, StrLocId, "S");


                                if (Request.QueryString["RT"] != null && Request.QueryString["Id"] != null)
                                {

                                    ObjMailBoxHeader.MailInbox_UpdateReplyStatus(Request.QueryString["Id"].ToString(), Session["CompId"].ToString(), Session["EmpId"].ToString(), "RE");


                                }
                                if (Request.QueryString["FW"] != null && Request.QueryString["Id"] != null)
                                {
                                    ObjMailBoxHeader.MailInbox_UpdateReplyStatus(Request.QueryString["Id"].ToString(), Session["CompId"].ToString(), Session["EmpId"].ToString(), "FW");

                                }

                                DisplayMessage("Mail Sent Successfully");
                                Reset();
                                foreach (string ToEMailId in StrAttachement.Split(','))
                                {
                                    try
                                    {
                                        System.IO.File.Delete(ToEMailId);
                                    }
                                    catch
                                    {

                                    }
                                }
                            }
                            else
                            {
                                DisplayMessage("Mail Sending Failed, Saved in outbox");
                                Reset();
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            }
            catch
            {


            }
        }

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "close", "window.close();", true);
       

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Reset();
    }

    protected void ImgBtnRemove_Command(object sender, CommandEventArgs e)
    {

        try
        {
            string fileName = new DataView((DataTable)ViewState["dt"], "Id='" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["FilePath"].ToString();
            if (File.Exists(Server.MapPath("~/Temp/") + fileName.ToString()))
            {
                File.Delete(Server.MapPath("~/Temp/") + fileName.ToString());
            }
        }
        catch
        {

        }
        DataTable dt = new DataView((DataTable)ViewState["dt"], "TransId<>'" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        Gvmultiple.DataSource = dt;
        Gvmultiple.DataBind();
        ViewState["dt"] = dt;
    }
    protected void ImgBtnAdd_Click(object sender, EventArgs e)
    {

        DataTable dt = new DataTable();
        if (ViewState["dt"] != null)
        {
            dt = (DataTable)ViewState["dt"];
        }
        else
        {
            dt.Columns.Add("TransId");
            dt.Columns.Add("FilePath");
            dt.Columns.Add("File_Name");
        }
        if (FileUpload1.HasFile)
        {

            FileUpload1.PostedFile.SaveAs(Server.MapPath("~/Temp/") + FileUpload1.FileName);
        }

        dt.Rows.Add();
        dt.Rows[dt.Rows.Count - 1]["TransId"] = dt.Rows.Count;
        dt.Rows[dt.Rows.Count - 1]["File_Name"] = FileUpload1.FileName.ToString();
        dt.Rows[dt.Rows.Count - 1]["FilePath"] = Server.MapPath("~/Temp/" + FileUpload1.FileName);


        Gvmultiple.DataSource = dt;
        Gvmultiple.DataBind();

        ViewState["dt"] = dt;


    }
    #endregion
    #region AutoCompleteMethod
    #region Contact
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetContactList(string prefixText, int count, string contextKey)
    {
        ES_EmailMaster_Header objEmailHeader = new ES_EmailMaster_Header(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable DtEmail = objEmailHeader.Get_EmailMasterHeaderAllTrue();

        try
        {
            DtEmail = new DataView(DtEmail, "Email_Id Like('%" + prefixText + "%')", "Email_Id Asc", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
            DtEmail = new DataTable();
        }

        string[] txt = new string[DtEmail.Rows.Count];

        if (DtEmail.Rows.Count > 0)
        {
            for (int i = 0; i < DtEmail.Rows.Count; i++)
            {
                txt[i] += DtEmail.Rows[i]["Email_Id"].ToString() + ";";
            }

        }

        return txt;
    }
    #endregion
    #endregion
    #region User Defined Function
    public void FillDownloadList(string RefType, string RefId)
    {
        DataTable dt = new DataView(Obj_Attachment.Get_Attachment(Session["CompId"].ToString(), "0", "0", RefType, RefId), "Field1=''", "", DataViewRowState.CurrentRows).ToTable();
        dt.Columns.Add("FilePath");
        foreach (DataRow Dr in dt.Rows)
        {
            Download(Dr["Content_Disposition"].ToString(), Dr["Content_Transfer"].ToString(), Dr["File_Data"].ToString(), Dr["File_Name"].ToString());
            Dr["FilePath"] = Server.MapPath("~/Atteched/" + Dr["File_Name"].ToString());
        }
        Gvmultiple.DataSource = dt;
        Gvmultiple.DataBind();
        ViewState["dt"] = dt;
    }
    public string[] GetUserNamePassEmpId()
    {
        string[] s = new string[3];
        try
        {
            DataTable dtUM = new DataView(objUserDetail.GetbyUserId(Session["UserId"].ToString(), Session["CompId"].ToString()), "Email='" + ddlEmailUser.SelectedItem.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtUM.Rows.Count != 0)
            {
                s[0] = Session["EmpId"].ToString();
                s[1] = dtUM.Rows[0]["Email"].ToString();
                s[2] = dtUM.Rows[0]["Password"].ToString();
            }
        }
        catch
        {
            s[0] = "0";
            s[1] = null;
            s[2] = null;

        }
        return s;

    }
    public void DisplayMessage(string str,string color="orange")
    {
        if (Session["lang"] == null)
        {
            Session["lang"] = "1";
        }
        if (Session["lang"].ToString() == "1")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('" + str + "');", true);
        }
        else if (Session["lang"].ToString() == "2")
        {
             ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('" + GetArebicMessage(str) + "');", true);
        }
    }
    public string GetArebicMessage(string EnglishMessage)
    {
        string ArebicMessage = string.Empty;
        DataTable dtres = (DataTable)Session["MessageDt"];
        if (dtres.Rows.Count != 0)
        {
            ArebicMessage = (new DataView(dtres, "Key='" + EnglishMessage + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Value"].ToString();
        }
        if (ArebicMessage == "")
        {
            ArebicMessage = EnglishMessage;
        }
        return ArebicMessage;
    }
    public void Reset()
    {
        txtMailTo.Text = "";
        txtBcc.Text = "";
        txtCC.Text = "";
        txtSubject.Text = "";
        txtMessage.Content = "";
        ViewState["dt"] = null;
        Gvmultiple.DataSource = null;
        Gvmultiple.DataBind();
        Session["Page&MailType&Id"] = null;

    }
    public void FillChildTable(TextBox text, string StrCompId, string StrBrandId, string StrLocId, string TransId, string RefType)
    {
        foreach (string s in text.Text.Split(';'))
        {
            if (s != "")
            {
                string StrAddressId = "0";
                DataTable DtContact = new DataTable();
                try
                {

                    DataAccessClass da = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());

                    string sql = "select ES_EmailMaster_Detail.Ref_Id from ES_EmailMaster_Header inner join ES_EmailMaster_Detail on ES_EmailMaster_Header.Trans_Id=ES_EmailMaster_Detail.Email_Ref_Id and ES_EmailMaster_Detail.Email_Ref_Type='Contact' where ES_EmailMaster_Header.Email_Id='" + s.ToString() + "'";

                    DtContact = da.return_DataTable(sql);
                    //StrAddressId = 
                    //new DataView(Obj_AddressMaster.GetAddressAllData(StrCompId), "EmailId1='" + s.ToString().Trim() + "' and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Trans_Id"].ToString();
                    //DtContact = new DataView(Obj_AddressChild.GetAddressChildDataByRef_Id(StrAddressId), "Add_Type='Contact'", "", DataViewRowState.CurrentRows).ToTable(true, "Add_Ref_Id");
                    if (DtContact.Rows.Count != 0)
                    {
                        for (int j = 0; j < DtContact.Rows.Count; j++)
                        {
                            OBj_SenddMailDetail.SendMailDetail_Insert(StrCompId, StrBrandId, StrLocId, TransId, RefType, DtContact.Rows[j]["Ref_Id"].ToString(), s.ToString(), "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                            //OBj_SenddMailDetail.SendMailDetail_Insert(StrCompId, StrBrandId, StrLocId, TransId, RefType, DtContact.Rows[j]["Add_Ref_Id"].ToString(), s.ToString(), "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        }
                    }
                    else
                    {
                        OBj_SenddMailDetail.SendMailDetail_Insert(StrCompId, StrBrandId, StrLocId, TransId, RefType, "0", s.ToString(), "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                    }
                }
                catch
                {
                    OBj_SenddMailDetail.SendMailDetail_Insert(StrCompId, StrBrandId, StrLocId, TransId, RefType, "0", s.ToString(), "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                }
            }


        }
    }
    //IsBinaryFile Funcation will check file is binary or not.
    private bool IsBinaryFile(string filePath, int sampleSize)
    {
        var buffer = new char[sampleSize];
        string sampleContent;

        using (var sr = new StreamReader(filePath))
        {
            int length = sr.Read(buffer, 0, sampleSize);
            sampleContent = new string(buffer, 0, length);
        }

        //Look for 4 consecutive binary zeroes
        if (sampleContent.Contains("\0\0\0\0"))
            return true;

        return false;
    }
    public void Download(string ContentDisposition, string ContentTransferEncoding, string m_data, string fileName)
    {
        // if BASE-64 data ...
        string m_contentDisposition = ContentDisposition;
        string m_contentTransferEncoding = ContentTransferEncoding;
        string FilePath = HttpContext.Current.Server.MapPath("~/Atteched");
        if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Atteched")))
        {
            Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Atteched"));
        }
        byte[] m_binaryData;
        if ((m_contentDisposition.Equals("attachment;")) &&
            (m_contentTransferEncoding.ToUpper()
            .Equals("BASE64")))
        {
            // convert attachment from BASE64 ...
            m_binaryData =
                  Convert.FromBase64String(m_data.Replace(@"\n", ""));
            if (File.Exists(HttpContext.Current.Server.MapPath("~/Atteched/" + fileName)))
            {
                File.Delete(HttpContext.Current.Server.MapPath("~/Atteched/" + fileName));
            }
            BinaryWriter bw;

            bw = new BinaryWriter(
                  new FileStream(FilePath + @"\" + fileName, FileMode.CreateNew));



            bw.Write(m_binaryData);
            bw.Flush();
            bw.Close();



        }
        else
        {

            if ((m_contentDisposition.Equals("attachment;")) &&
                (m_contentTransferEncoding.ToUpper()
                .Equals("QUOTED-PRINTABLE")))
            {
                using (StreamWriter sw = File.CreateText(FilePath + @"\" + fileName))
                {

                    sw.Write(FromQuotedPrintable(m_data));
                    sw.Flush();
                    sw.Close();
                }



            }
        }


    }
    private string FromQuotedPrintable(string inString)
    {
        string outputString = null;
        string Str = null;
        string inputString = inString.Replace("=\n", "");
        if (inputString.Length > 3)
        {
            outputString = "";

            for (int x = 0; x < inputString.Length; )
            {
                string s1 = inputString.Substring(x, 1);

                if ((s1.Equals("=")) && ((x + 2) < inputString.Length))
                {
                    string hexString = inputString.Substring(x + 1, 2);

                    // if hexadecimal ...
                    if (Regex.Match(hexString.ToUpper()
                        , @"^[A-F|0-9]+[A-F|0-9]+$").Success)
                    {
                        // convert to string representation ...
                        outputString +=
                            System.Text
                            .Encoding.ASCII.GetString(
                            new byte[] {
									System.Convert.ToByte(hexString,16)});
                        x += 3;
                    }
                    else
                    {
                        outputString += s1;
                        ++x;
                    }
                }
                else
                {
                    outputString += s1;
                    ++x;


                }

            }
        }
        else
        {
            outputString = inputString;
        }


        return outputString.Replace("\n", "\r\n");
    }
    public void fiilContactGrid()
    {
        DataTable dt = objEMHeader.Get_EmailMasterandContactAll();
        //ObjContactMaster.GetContactAllData();
        //dt.Columns["Name"].ColumnName = "ContactName";
        lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        gvContact.DataSource = dt;

        gvContact.DataBind();
        ViewState["DtAddress"] = dt;


    }
    public void fiilContactGridGroup(string Group_Name)
    {
        if (Group_Name != "")
        {
            string strGroupName = txtValueGroup.Text.Split('/')[0].ToString();
            DataTable dt = ObjContactMaster.GetContactNameandEmailIdAllDataByGroup(strGroupName);
            lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
            gvContact.DataSource = dt;
            //ViewState["DtAddress"] = dt;
            gvContact.DataBind();
        }


    }
    protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvContact.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < gvContact.Rows.Count; i++)
        {
            ((CheckBox)gvContact.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            string s = ((CheckBox)gvContact.Rows[i].FindControl("chkSelect")).Text.ToString();
            if (chkSelAll.Checked)
            {

                txtMailTo.Text += s.ToString() + ";";

            }
            else
            {
                string temp = string.Empty;
                string[] split = txtMailTo.Text.Split(';');
                foreach (string item in split)
                {
                    if (item != ((CheckBox)(gvContact.Rows[i].FindControl("chkSelect"))).Text.ToString())
                    {
                        if (item != "")
                        {
                            temp += item + ";";
                        }
                    }
                }
                txtMailTo.Text = temp;
            }
        }
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        fiilContactGrid();
        txtValue.Text = "";
        ddlFieldName.SelectedIndex = 0;
        txtValue.Visible = true;
        txtValueEmail.Visible = false;
        txtValueGroup.Visible = false;
        txtValueCompany.Visible = false;
    }
    protected void btnbind_Click(object sender, EventArgs e)
    {
        DataTable dtContact2 = new DataTable();

        if (ddlFieldName.SelectedIndex == 1)
        {
            if (txtValueCompany.Text != "")
            {
                dtContact2 = objEMHeader.Get_EmailMasterandContact(txtValueCompany.Text.Split('/')[1].ToString(), "Contact");

            }
            else
            {
                DisplayMessage("Select Company");
                txtValueCompany.Focus();
                return;
            }
        }

        if (ddlFieldName.SelectedIndex == 0)
        {

            if (txtValue.Text != "")
            {
                dtContact2 = objEMHeader.Get_EmailMasterandContact(txtValue.Text.Split('/')[1].ToString(), "Contact");
            }
            else
            {
                DisplayMessage("Select Name");
                txtValue.Focus();
                return;
            }

        }
        //if (ddlFieldName.SelectedIndex == 3)
        //{
        //    DataTable dtContact2 = new DataTable();
        //    dtContact2 = objEMHeader.Get_EmailMasterandContactbyemail(txtValueEmail.Text.Split('/')[1].ToString());
        //    gvContact.DataSource = dtContact2;
        //    gvContact.DataBind();
        //    return;
        //}
        if (ddlFieldName.SelectedIndex == 2)
        {

            if (txtValueEmail.Text != "")
            {
                DataTable dt = objEMHeader.Get_EmailMasterandContactAll();

                dtContact2 = new DataView(dt, "Field1 Like '%" + txtValueEmail.Text.Trim() + "%'", "Field1 ASC", DataViewRowState.CurrentRows).ToTable();
            }
            else
            {
                DisplayMessage("Select Email Id");
                txtValueEmail.Focus();
                return;
            }

        }
        lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtContact2.Rows.Count.ToString() + "";
        gvContact.DataSource = dtContact2;
        gvContact.DataBind();
        btnbind.Focus();

        //if (ddlFieldName.SelectedIndex != 2)
        //{
        //    string condition = string.Empty;

        //    condition = ddlFieldName.SelectedValue + " like '%" + txtValue.Text.Trim() + "%'";
        //    DataTable dtContact = new DataTable();
        //    if (ddlFieldName.SelectedValue != "CompanyName")
        //    {

        //        dtContact = ObjContactMaster.GetContactForMainGrid(condition);
        //    }
        //    else
        //    {
        //        condition = condition.Replace("CompanyName", "Name");
        //        dtContact = ObjContactMaster.GetContactForMainGridByComp(condition);
        //    }

        //    lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtContact.Rows.Count.ToString() + "";
        //    gvContact.DataSource = dtContact;
        //    gvContact.DataBind();
        //    btnbind.Focus();
        //}
        //else
        //{
        //    fiilContactGridGroup(txtValueGroup.Text);
        //}        
    }
    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFieldName.SelectedIndex == 2)//by email id
        {
            // txtValueGroup_AutoCompleteExtender.ContextKey = ((DropDownList)sender).SelectedValue;
            txtValue.Visible = false;
            txtValueGroup.Visible = false;
            txtValueGroup.Text = "";
            txtValueCompany.Visible = false;
            txtValueEmail.Visible = true;
            txtValueEmail.Text = "";
        }
        else if (ddlFieldName.SelectedIndex == 0)//By name 
        {
            txtValue.Visible = true;
            txtValueGroup.Visible = false;
            txtValue.Text = "";
            txtValueCompany.Visible = false;
            txtValueEmail.Visible = false;
        }
        else if (ddlFieldName.SelectedIndex == 1)//By Company
        {
            txtValueCompany.Visible = true;
            txtValueCompany.Text = "";
            txtValueGroup.Visible = false;
            txtValue.Visible = false;

            txtValueEmail.Visible = false;
            txtValue.Text = "";
        }
        //else if (ddlFieldName.SelectedIndex == 3)
        //{
        //    txtValueEmail.Visible = true;
        //    txtValue.Visible = false;
        //    txtValueCompany.Visible = false;
        //    txtValueGroup.Visible = false;
        //}

    }

    protected void ImgBccContant_Click(object sender, EventArgs e)
    {

        fiilContactGrid();
        pnl1.Visible = true;
        pnl2.Visible = true;
        ViewState["Id"] = "BCc";

        ddlFieldName.SelectedIndex = 0;
        txtValueGroup.Text = "";
        txtValue.Text = "";
        txtValue.Visible = true;
        txtValueGroup.Visible = false;
        txtValueEmail.Visible = false;
        txtValueCompany.Visible = false;
        txtValueGroup.Text = "";
        txtValueEmail.Text = "";
        txtValueCompany.Text = "";
    }
    protected void ImgCcContant_Click(object sender, EventArgs e)
    {

        fiilContactGrid();
        pnl1.Visible = true;
        pnl2.Visible = true;
        ViewState["Id"] = "Cc";


        ddlFieldName.SelectedIndex = 0;
        txtValueGroup.Text = "";
        txtValue.Text = "";
        txtValue.Visible = true;
        txtValueGroup.Visible = false;
        txtValueEmail.Visible = false;
        txtValueCompany.Visible = false;
        txtValueGroup.Text = "";
        txtValueEmail.Text = "";
        txtValueCompany.Text = "";
    }

    protected void ImgToContact_Click(object sender, EventArgs e)
    {
        fiilContactGrid();
        pnl1.Visible = true;
        pnl2.Visible = true;
        ViewState["Id"] = "To";
        ddlFieldName.SelectedIndex = 0;
        txtValueGroup.Text = "";
        txtValue.Text = "";
        txtValue.Visible = true;
        txtValueGroup.Visible = false;
        txtValueEmail.Visible = false;
        txtValueCompany.Visible = false;
        txtValueGroup.Text = "";
        txtValueEmail.Text = "";
        txtValueCompany.Text = "";

    }
    protected void chkselect_CheckedChanged(object sender, EventArgs e)
    {
        string s = ((CheckBox)sender).Text.ToString();



        if (ViewState["Id"].ToString() == "To")
        {
            if (((CheckBox)sender).Checked)
            {
                bool b = true;
                foreach (string str in txtMailTo.Text.Split(';'))
                {
                    if (str.Equals(s.ToString()))
                    {
                        b = false;
                    }

                }
                if (b)
                {
                    txtMailTo.Text += s.ToString() + ";";
                }
            }
            else
            {
                string StrTo = string.Empty;
                foreach (string str in txtMailTo.Text.Split(';'))
                {
                    if (str.ToString() != "")
                    {
                        if (!str.Equals(s.ToString()))
                        {
                            StrTo += str + ";";

                        }
                    }
                }

                txtMailTo.Text = StrTo;

            }

        }
        if (ViewState["Id"].ToString() == "Cc")
        {
            if (((CheckBox)sender).Checked)
            {
                bool b = true;
                foreach (string str in txtCC.Text.Split(';'))
                {
                    if (str.Equals(s.ToString()))
                    {
                        b = false;
                    }

                }
                if (b)
                {
                    txtCC.Text += s.ToString() + ";";
                }
            }
            else
            {
                string StrTo = string.Empty;
                foreach (string str in txtCC.Text.Split(';'))
                {
                    if (str.ToString() != "")
                    {
                        if (!str.Equals(s.ToString()))
                        {
                            StrTo += str + ";";

                        }
                    }
                }

                txtCC.Text = StrTo;

            }

        }
        if (ViewState["Id"].ToString() == "BCc")
        {
            if (((CheckBox)sender).Checked)
            {
                bool b = true;
                foreach (string str in txtBcc.Text.Split(';'))
                {
                    if (str.Equals(s.ToString()))
                    {
                        b = false;
                    }

                }
                if (b)
                {

                    txtBcc.Text += s.ToString() + ";";
                }
            }
            else
            {
                string StrTo = string.Empty;
                foreach (string str in txtBcc.Text.Split(';'))
                {
                    if (str.ToString() != "")
                    {
                        if (!str.Equals(s.ToString()))
                        {
                            StrTo += str + ";";

                        }
                    }
                }

                txtBcc.Text = StrTo;



            }
        }

    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        pnl1.Visible = false;
        pnl2.Visible = false;
    }
    protected void txtValueGroup_TextChanged(object sender, EventArgs e)
    {
        string strGroupId = string.Empty;
        if (txtValueGroup.Text != "")
        {
            try
            {
                strGroupId = txtValueGroup.Text.Split('/')[1].ToString();
            }
            catch
            {
                strGroupId = "0";

            }
            if (strGroupId == "0")
            {
                DisplayMessage("Select In Suggestions Only");
                txtValueGroup.Text = "";
                txtValueGroup.Focus();
            }


        }

    }
    protected void txtValueCompany_TextChanged(object sender, EventArgs e)
    {
        string strCompanyId = string.Empty;
        if (txtValueCompany.Text != "")
        {
            try
            {
                strCompanyId = txtValueCompany.Text.Split('/')[1].ToString();
            }
            catch
            {
                strCompanyId = "0";

            }
            if (strCompanyId == "0")
            {
                DisplayMessage("Select In Suggestions Only");
                txtValueCompany.Text = "";
                txtValueCompany.Focus();
            }


        }

    }
    protected void txtValue_TextChanged(object sender, EventArgs e)
    {
        string strId = string.Empty;
        if (txtValue.Text != "")
        {
            try
            {
                strId = txtValue.Text.Split('/')[1].ToString();
            }
            catch
            {
                strId = "0";

            }
            if (strId == "0")
            {
                DisplayMessage("Select In Suggestions Only");
                txtValue.Text = "";
                txtValue.Focus();
            }


        }

    }
    protected void txtValueEmail_TextChanged(object sender, EventArgs e)
    {
        string strEmailId = string.Empty;
        if (txtValueEmail.Text != "")
        {
            try
            {
                strEmailId = txtValueEmail.Text.Split('/')[1].ToString();
            }
            catch
            {
                strEmailId = "0";

            }
            if (strEmailId == "0")
            {
                DisplayMessage("Select In Suggestions Only");
                txtValueEmail.Text = "";
                txtValueEmail.Focus();
            }


        }

    }


    #endregion

    #region AutoComplete
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]

    public static string[] GetCompletionListGroup(string prefixText, int count, string contextKey)
    {
        Ems_GroupMaster ObjGroupMaster = new Ems_GroupMaster(HttpContext.Current.Session["DBConnection"].ToString());


        DataTable dtGroup = ObjGroupMaster.GetGroupMasterTrueAllData();


        DataTable dtMain = new DataTable();
        dtMain = dtGroup.Copy();


        string filtertext = "Group_Name like '%" + prefixText + "%'";
        DataTable dtCon = new DataView(dtMain, filtertext, "", DataViewRowState.CurrentRows).ToTable();
        if (dtCon.Rows.Count == 0)
        {
            dtCon = dtGroup;
        }
        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Group_Name"].ToString() + "/" + dtCon.Rows[i]["Group_Id"].ToString();
            }
        }
        return filterlist;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]

    public static string[] GetCompletionListCompany(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster ObjConMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());


        DataTable dtContact = ObjConMaster.GetAllCompanyList();


        DataTable dtMain = new DataTable();
        dtMain = dtContact.Copy();


        string filtertext = "ContactName like '%" + prefixText + "%'";
        DataTable dtCon = new DataView(dtMain, filtertext, "ContactName Asc", DataViewRowState.CurrentRows).ToTable();
        if (dtCon.Rows.Count == 0)
        {
            // dtCon = dtContact;
        }
        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["ContactName"].ToString() + "/" + dtCon.Rows[i]["Trans_Id"].ToString();
            }
        }
        return filterlist;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]

    public static string[] GetCompletionListName(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster ObjConMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());


        DataTable dtContact = ObjConMaster.GetContactTrueAllDataForGRid();


        DataTable dtMain = new DataTable();
        dtMain = dtContact.Copy();


        string filtertext = "ContactName like '%" + prefixText + "%'";
        DataTable dtCon = new DataView(dtMain, filtertext, "ContactName Asc", DataViewRowState.CurrentRows).ToTable();
        if (dtCon.Rows.Count == 0)
        {
            // dtCon = dtContact;
        }
        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["ContactName"].ToString() + "/" + dtCon.Rows[i]["Trans_Id"].ToString();
            }
        }
        return filterlist;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmail(string prefixText, int count, string contextKey)
    {
        ES_EmailMaster_Header objEmailHeader = new ES_EmailMaster_Header(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable DtEmail = new DataView(objEmailHeader.Get_EmailMasterHeaderAllTrue(), "Email_Id Like('%" + prefixText + "%')", "Email_Id Asc", DataViewRowState.CurrentRows).ToTable();

        string[] txt = new string[DtEmail.Rows.Count];

        if (DtEmail.Rows.Count > 0)
        {
            for (int i = 0; i < DtEmail.Rows.Count; i++)
            {
                //  txt[i] += DtEmail.Rows[i]["Email_Id"].ToString();
                txt[i] = DtEmail.Rows[i]["Email_Id"].ToString() + "/" + DtEmail.Rows[i]["Trans_Id"].ToString();

            }

        }

        return txt;
    }


    #endregion

    public static string Value
    {
        get;
        set;
    }

    public void FillUserEmailAccount()
    {
        DataTable dt = objUserDetail.GetbyUserId(Session["UserId"].ToString(), Session["CompId"].ToString());
        ddlEmailUser.DataSource = dt;
        ddlEmailUser.DataTextField = "Email";
        ddlEmailUser.DataValueField = "Trans_Id";
        ddlEmailUser.DataBind();
        if (Session["UserEmailSelect"] != null)
        {
            ddlEmailUser.SelectedValue = Session["UserEmailSelect"].ToString();
        }
    }
    public void ddlEmailUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillEmail();
    }
    protected void FUAll_FileUploadComplete(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
        {
            FileUpload1.PostedFile.SaveAs(Server.MapPath("~/Temp/") + FileUpload1.FileName);
        }
    }
}
