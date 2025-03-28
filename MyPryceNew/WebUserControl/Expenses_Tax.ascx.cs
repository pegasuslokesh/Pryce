using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



public delegate void SendMessageToThePageHandler(DataTable DT_Send_to_Page);
public partial class WebUserControl_Expenses_Tax : System.Web.UI.UserControl
{
    Common cmn = null;
    TaxMaster Obj_TaxMaster = null;
    public static string Expenses_Amount = "0.00";
    public static string Expenses_Tax = "0%";

    public static int Decimal_Count_For_Tax;

    protected void Page_Load(object sender, EventArgs e)
    {
        cmn = new Common(Session["DBConnection"].ToString());
        Obj_TaxMaster = new TaxMaster(Session["DBConnection"].ToString());

        if (((HiddenField)Parent.FindControl("Hdn_Expenses_Id_Web_Control"))!=null && ((HiddenField)Parent.FindControl("Hdn_Expenses_Id_Web_Control")).Value != "")
            Hdn_Expenses_Id_Web_Control.Value = ((HiddenField)Parent.FindControl("Hdn_Expenses_Id_Web_Control")).Value;
        if (((HiddenField)Parent.FindControl("Hdn_Expenses_Amount_Web_Control"))!=null && ((HiddenField)Parent.FindControl("Hdn_Expenses_Amount_Web_Control")).Value != "")
            Hdn_Expenses_Amount_Web_Control.Value = ((HiddenField)Parent.FindControl("Hdn_Expenses_Amount_Web_Control")).Value;
        if (((HiddenField)Parent.FindControl("Hdn_Expenses_Name_Web_Control"))!=null && ((HiddenField)Parent.FindControl("Hdn_Expenses_Name_Web_Control")).Value != "")
            Hdn_Expenses_Name_Web_Control.Value = ((HiddenField)Parent.FindControl("Hdn_Expenses_Name_Web_Control")).Value;
        if (((HiddenField)Parent.FindControl("Hdn_Page_Name_Web_Control"))!=null && ((HiddenField)Parent.FindControl("Hdn_Page_Name_Web_Control")).Value != "")
            Hdn_Page_Name_Web_Control.Value = ((HiddenField)Parent.FindControl("Hdn_Page_Name_Web_Control")).Value;
        if (((HiddenField)Parent.FindControl("Hdn_Tax_Entry_Type"))!=null && ((HiddenField)Parent.FindControl("Hdn_Tax_Entry_Type")).Value != "")
            Hdn_Tax_Entry_Type.Value = ((HiddenField)Parent.FindControl("Hdn_Tax_Entry_Type")).Value;

        
        if (((HiddenField)Parent.FindControl("Hdn_Saved_Expenses_Tax_Session"))!=null && ((HiddenField)Parent.FindControl("Hdn_Saved_Expenses_Tax_Session")).Value != "")
            Hdn_Saved_Expenses_Tax_Session.Value = ((HiddenField)Parent.FindControl("Hdn_Saved_Expenses_Tax_Session")).Value;

        if (((HiddenField)Parent.FindControl("Hdn_Local_Expenses_Tax_Session"))!=null && ((HiddenField)Parent.FindControl("Hdn_Local_Expenses_Tax_Session")).Value != "")
            Hdn_Local_Expenses_Tax_Session.Value = ((HiddenField)Parent.FindControl("Hdn_Local_Expenses_Tax_Session")).Value;

        //if (Session[Hdn_Local_Expenses_Tax_Session.Value] != null)
        //{
        //    Gv_Tax.DataSource = Session[Hdn_Local_Expenses_Tax_Session.Value] as DataTable;
        //    Gv_Tax.DataBind();
        //}
        if (Session[Hdn_Local_Expenses_Tax_Session.Value] == null)
        {
            Gv_Tax.DataSource = null;
            Gv_Tax.DataBind();
        }

        Txt_Tax_Type_Web_Control.Focus();
        Visible_Save_Button();

        if(!IsPostBack)
        {
            string Decimal_Count = string.Empty;
            Decimal_Count = cmn.Get_Decimal_Count_By_Location(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            Decimal_Count_For_Tax = Convert.ToInt32(Decimal_Count);
        }

    }

    public string Convert_Into_DF(string Amount)
    {
        try
        {
            if (Amount == "")
                Amount = "0";

            if (Decimal_Count_For_Tax.ToString() == "")
                Decimal_Count_For_Tax = 0;

            if (Decimal_Count_For_Tax == 0)
            {
                string Decimal_Count = string.Empty;
                Decimal_Count = cmn.Get_Decimal_Count_By_Location(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
                Decimal_Count_For_Tax = Convert.ToInt32(Decimal_Count);
            }

            decimal Amount_D = Convert.ToDecimal((Convert.ToDouble(Amount)).ToString("F7"));
            Amount = Amount_D.ToString();
            int index = Amount.ToString().LastIndexOf(".");
            if (index > 0)
                Amount = Amount.Substring(0, index + (Decimal_Count_For_Tax + 1));
            return Amount;
        }
        catch
        {
            return "0";
        }
    }

    public void Visible_Save_Button()
    {
        if (Gv_Tax.Rows.Count > 0)
            Btn_Save_Web.Visible = true;
        else
            Btn_Save_Web.Visible = false;
    }

    public void DisplayMessage(string str,string color="orange")
    {
        if (Session["lang"] == null)
        {
            Session["lang"] = "1";
        }
        if (Session["lang"].ToString() == "1")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
        }
        else if (Session["lang"].ToString() == "2")
        {
             ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + GetArebicMessage(str) + "','"+color+"','white');", true);
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

    protected void Img_Tax_Delete_GV_Command(object sender, CommandEventArgs e)
    {
        DataTable Dt_Grid_Tax_Web_Control = Session[Hdn_Local_Expenses_Tax_Session.Value] as DataTable;
        if (Dt_Grid_Tax_Web_Control != null)
        {
            if (Dt_Grid_Tax_Web_Control != null && Dt_Grid_Tax_Web_Control.Rows.Count > 0)
            {
                Dt_Grid_Tax_Web_Control = new DataView(Dt_Grid_Tax_Web_Control, "Tax_Type_Id<>" + e.CommandArgument.ToString() + " and Expenses_Id=" + e.CommandName.ToString(), "", DataViewRowState.CurrentRows).ToTable();                
                Gv_Tax.DataSource = Dt_Grid_Tax_Web_Control;
                Gv_Tax.DataBind();
            }
        }
        Session[Hdn_Local_Expenses_Tax_Session.Value] = Dt_Grid_Tax_Web_Control;
        Visible_Save_Button();
    }

    protected void Btn_Add_to_Grid_Click(object sender, EventArgs e)
    {   
        if (Txt_Tax_Type_Web_Control.Text == "")
        {
            DisplayMessage("Please enter Tax Type");
            return;
        }
        if (Txt_Tax_Percenage_Web_Control.Text == "")
        {
            DisplayMessage("Please enter Tax Percentage");
            return;
        }
        if (Txt_Tax_Percenage_Web_Control.Text != "")
        {
            double Tax_Percent = Convert.ToDouble(Txt_Tax_Percenage_Web_Control.Text);
            if (Tax_Percent > 100)
            {
                Txt_Tax_Percenage_Web_Control.Text = "";
                DisplayMessage("Please enter Tax Percentage between 0 to 100");
                return;
            }
        }
        foreach (GridViewRow GV_Row in Gv_Tax.Rows)
        {
            HiddenField Tax_Type_ID_GV = (HiddenField)GV_Row.FindControl("Hdn_Tax_Type_ID_GV");
            HiddenField Expenses_Id_GV = (HiddenField)GV_Row.FindControl("Hdn_Expenses_Id_GV");

            if (Tax_Type_ID_GV.Value == Txt_Tax_Type_Web_Control.Text.Split('/')[1].ToString() && Expenses_Id_GV.Value==Hdn_Expenses_Id_Web_Control.Value)
            {
                DisplayMessage("Tax Type " + Txt_Tax_Type_Web_Control.Text.Split('/')[0].ToString() + " already exists");
                return;
            }
        }
        DataTable Dt_Grid_Tax_Web_Control = Session[Hdn_Local_Expenses_Tax_Session.Value] as DataTable;
        if (Dt_Grid_Tax_Web_Control == null)
        {
            Dt_Grid_Tax_Web_Control = new DataTable();
            Dt_Grid_Tax_Web_Control.Columns.AddRange(new DataColumn[10] { new DataColumn("Tax_Type_Id", typeof(int)), new DataColumn("Tax_Type_Name"), new DataColumn("Tax_Percentage"), new DataColumn("Tax_Value"), new DataColumn("Expenses_Id"), new DataColumn("Expenses_Name"), new DataColumn("Expenses_Amount"), new DataColumn("Page_Name"), new DataColumn("Tax_Entry_Type"), new DataColumn("Tax_Account_Id") });
            //Session[Hdn_Saved_Expenses_Tax_Session.Value] = Dt_Grid_Tax_Web_Control;
        }

        double Tax_Per = Convert.ToDouble(Convert_Into_DF(Txt_Tax_Percenage_Web_Control.Text));
        double Amount = Convert.ToDouble(Convert_Into_DF(Hdn_Expenses_Amount_Web_Control.Value));
        double Tax_Value = (Amount * Tax_Per) / 100;
        try
        {
            Dt_Grid_Tax_Web_Control.Rows.Add(Txt_Tax_Type_Web_Control.Text.Split('/')[1].ToString(), Txt_Tax_Type_Web_Control.Text.Split('/')[0].ToString(), Convert_Into_DF(Txt_Tax_Percenage_Web_Control.Text), Convert.ToDouble(Convert_Into_DF(Tax_Value.ToString())), Hdn_Expenses_Id_Web_Control.Value, Hdn_Expenses_Name_Web_Control.Value, Hdn_Expenses_Amount_Web_Control.Value, Hdn_Page_Name_Web_Control.Value, Hdn_Tax_Entry_Type.Value, Hdn_Tax_Account_Id.Value);
        }
        catch
        {
            DisplayMessage("Please select Tax Type from suggestion only");
            Txt_Tax_Type_Web_Control.Focus();
        }
        Gv_Tax.DataSource = Dt_Grid_Tax_Web_Control;
        Gv_Tax.DataBind();

        Session[Hdn_Local_Expenses_Tax_Session.Value] = Dt_Grid_Tax_Web_Control;

        Reset();
        Visible_Save_Button();
    }

    private void Reset()
    {
        Txt_Tax_Type_Web_Control.Text = "";
        Txt_Tax_Percenage_Web_Control.Text = "";
    }

    protected void Txt_Tax_Type_Web_Control_TextChanged(object sender, EventArgs e)
    {
        if (Txt_Tax_Type_Web_Control.Text == "")
        {
            DisplayMessage("Please enter Tax Type");
            return;
        }

        DataTable Dt_Tax_Type = Obj_TaxMaster.GetTaxMaster_ByTaxName(Txt_Tax_Type_Web_Control.Text.Split('/')[0].ToString());
        if (Dt_Tax_Type == null || Dt_Tax_Type.Rows.Count == 0)
        {
            Hdn_Tax_Account_Id.Value = "";
            Txt_Tax_Type_Web_Control.Text = "";
            Txt_Tax_Type_Web_Control.Focus();
            DisplayMessage("Please select Tax Type from suggestion only");
            return;
        }
        else
        {
            Hdn_Tax_Account_Id.Value = Dt_Tax_Type.Rows[0]["Field3"].ToString();
            Txt_Tax_Percenage_Web_Control.Focus();
        }
    }

    protected void Btn_Save_Web_Click(object sender, EventArgs e)
    {
        int Check_Blank_Percentage = 0;
        int Check_Greater_Percentage = 0;
        foreach (GridViewRow GV_Row in Gv_Tax.Rows)
        {
            HiddenField Tax_Type_Id = (HiddenField)GV_Row.FindControl("Hdn_Tax_Type_ID_GV");
            Label Tax_Type_Name = (Label)GV_Row.FindControl("Lbl_Tax_Type_GV");
            TextBox Tax_Percentage = (TextBox)GV_Row.FindControl("Txt_Tax_Percentage_GV");
            if (Tax_Percentage.Text == "")
            {
                Check_Blank_Percentage++;
            }
            else if (Convert.ToDouble(Tax_Percentage.Text) > 100)
                Check_Greater_Percentage++;
        }
        if (Check_Blank_Percentage != 0)
        {
            DisplayMessage("Please enter Tax Percentage in grid");
            return;
        }
        if (Check_Greater_Percentage != 0)
        {
            DisplayMessage("Please enter Tax Percentage between 0 to 100 in grid");
            return;
        }
        DataTable Dt_Saved_Expenses_Tax_Web_Control = null;
        if (Dt_Saved_Expenses_Tax_Web_Control == null)
        {
            Dt_Saved_Expenses_Tax_Web_Control = new DataTable();
            Dt_Saved_Expenses_Tax_Web_Control.Columns.AddRange(new DataColumn[10] { new DataColumn("Tax_Type_Id", typeof(int)), new DataColumn("Tax_Type_Name"), new DataColumn("Tax_Percentage"), new DataColumn("Tax_Value"), new DataColumn("Expenses_Id"), new DataColumn("Expenses_Name"), new DataColumn("Expenses_Amount"), new DataColumn("Page_Name"), new DataColumn("Tax_Entry_Type"), new DataColumn("Tax_Account_Id") });
        }

        double Net_Tax = 0;
        foreach (GridViewRow GV_Row in Gv_Tax.Rows)
        {
            HiddenField Tax_Type_ID= (HiddenField)GV_Row.FindControl("Hdn_Tax_Type_ID_GV");
            HiddenField Expenses_Id = (HiddenField)GV_Row.FindControl("Hdn_Expenses_Id_GV");            
            Label Tax_Type_Name = (Label)GV_Row.FindControl("Lbl_Tax_Type_GV");
            Label Expenses_Name = (Label)GV_Row.FindControl("Lbl_Expenses_Name_GV");
            TextBox Tax_Percentage = (TextBox)GV_Row.FindControl("Txt_Tax_Percentage_GV");

            HiddenField Tax_Entry_Type = (HiddenField)GV_Row.FindControl("Hdn_Tax_Entry_Type_GV");
            HiddenField Tax_Account_Id = (HiddenField)GV_Row.FindControl("Hdn_Tax_Account_Id_GV");
            HiddenField Tax_Value = (HiddenField)GV_Row.FindControl("Hdn_Tax_Value_GV");
            HiddenField Expenses_Amount = (HiddenField)GV_Row.FindControl("Hdn_Expenses_Amount_GV");

            Tax_Value.Value = ((Convert.ToDouble(Expenses_Amount.Value) * Convert.ToDouble(Tax_Percentage.Text)) / 100).ToString();

            if (Expenses_Id.Value == Hdn_Expenses_Id_Web_Control.Value)
                Net_Tax = Net_Tax + (Convert.ToDouble(Tax_Percentage.Text));
            Dt_Saved_Expenses_Tax_Web_Control.Rows.Add(Tax_Type_ID.Value, Tax_Type_Name.Text, Convert_Into_DF(Tax_Percentage.Text), Tax_Value.Value, Expenses_Id.Value, Expenses_Name.Text, Expenses_Amount.Value, Hdn_Page_Name_Web_Control.Value, Tax_Entry_Type.Value, Tax_Account_Id.Value);
        }
        double Expenses_Tax_Amount = Convert.ToDouble(Hdn_Expenses_Amount_Web_Control.Value);
        if (Expenses_Tax_Amount > 0)
        {
            Expenses_Amount = ((Expenses_Tax_Amount * Net_Tax) / 100).ToString();
            Expenses_Tax = Net_Tax.ToString() + "%";
        }
        else
        {
            Expenses_Amount = "0.00";
            Expenses_Tax = "0%";
        }
        Session[Hdn_Saved_Expenses_Tax_Session.Value] = Dt_Saved_Expenses_Tax_Web_Control;
        
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Expenses_Tax_Web_Control()", true);

        Gv_Tax.DataSource = null;
        Gv_Tax.DataBind();
    }

    public string Expenses_Amount_Value()
    {
        string Expenses_Amount_Temp = "0.00";
        if (Expenses_Amount != "0.00")
            Expenses_Amount_Temp = Expenses_Amount;
        return Expenses_Amount_Temp;
    }

    public string Expenses_Tax_Value()
    {
        string Expenses_Tax_Temp = "0%";
        if (Expenses_Tax != "0%")
            Expenses_Tax_Temp = Expenses_Tax;
        return Expenses_Tax_Temp;
    }

    public void Expenses_Tax_And_Amount_Clear()
    {
        Expenses_Amount = "0.00";
        Expenses_Tax = "0%";
    }
    
}
