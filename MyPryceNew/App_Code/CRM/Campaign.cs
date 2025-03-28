using System;
using System.Data;
using PegasusDataAccess;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Campaign
/// </summary>
public class Campaign
{
    DataAccessClass daClass = null;
    int i = 0;
    public Campaign(string strConString)
    {
        daClass = new DataAccessClass(strConString);
    }

    public DataTable GetAllCampaignActiveData(string Company_ID, string Brand_ID, string Location_ID)
    {

        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Company_ID", Company_ID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_ID", Brand_ID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_ID", Location_ID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Is_Active", "true", PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("Sp_crm_campaign_All_Campaign_Data", paramList))
        {
            return dtInfo;
        }
    }
    public DataTable GetAllCampaignInActiveData(string Company_ID, string Brand_ID, string Location_ID)
    {
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Company_ID", Company_ID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_ID", Brand_ID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_ID", Location_ID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Is_Active", "false", PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("Sp_crm_campaign_All_Campaign_Data", paramList))
        {
            return dtInfo;
        }
    }


    public int InsertCampaign(string Company_ID, string Brand_ID, string Location_ID, string Campaign_Name, string C_Start_Date, string C_End_Date, string Created_By, string Modified_By, string Budget, string C_Description, string Campaign_type, string currencyID,string assignTo, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[17];
        paramList[0] = new PassDataToSql("@Trans_ID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Company_ID", Company_ID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Brand_ID", Brand_ID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Location_ID", Location_ID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Campaign_Name", Campaign_Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@C_Start_Date", C_Start_Date, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@C_End_Date", C_End_Date, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Is_Active", "true", PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Created_By", Created_By, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Modified_By", Modified_By, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Budget", Budget, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@C_Description", C_Description, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Campaign_type", Campaign_type, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Currency_id", currencyID, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@OpType", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[16] = new PassDataToSql("@AssignTo", assignTo, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("Sp_crm_campaign_Insert_Update", paramList, ref trns);
        return Convert.ToInt32(paramList[15].ParaValue);
    }

    public int UpdateCampaign(string Trans_ID, string Campaign_Name, string C_Start_Date, string C_End_Date, string Created_By, string Modified_By, string Budget, string C_Description, string Campaign_type, string currencyID,string assignto, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[17];
        paramList[0] = new PassDataToSql("@Trans_ID", Trans_ID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Company_ID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Brand_ID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Location_ID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Campaign_Name", Campaign_Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@C_Start_Date", C_Start_Date, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@C_End_Date", C_End_Date, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Is_Active", "true", PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Created_By", Created_By, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Modified_By", Modified_By, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Budget", Budget, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@C_Description", C_Description, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Campaign_type", Campaign_type, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Currency_id", currencyID, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@OpType", "2", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[16] = new PassDataToSql("@AssignTo", assignto, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("Sp_crm_campaign_Insert_Update", paramList, ref trns);
        return Convert.ToInt32(paramList[15].ParaValue);
    }


    public int ActivateCampaignRecordById(string trans_Id, string Modified_By)
    {
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Trans_ID", trans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Modified_By", Modified_By, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Is_Active", "true", PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("Sp_crm_campaign_Update_Campaign_By_Id", paramList);
        return 1;
    }

    public int InActivateCampaignRecordById(string trans_Id, string Modified_By)
    {
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Trans_ID", trans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Modified_By", Modified_By, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Is_Active", "false", PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("Sp_crm_campaign_Update_Campaign_By_Id", paramList);
        return 1;
    }

    public DataTable CheckDuplicacy(string Company_ID, string Campaign_Name, string OpType)
    {
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@OpType", OpType, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Company_ID", Company_ID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Campaign_Name", Campaign_Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("Sp_crm_campaign_Check_Duplicacy", paramList))
        {
            return dtInfo;
        }
    }
  

    public int InsertProduct( string Crm_campaign_id, string Product_id, string Created_By,  string Modified_By, string Target_sales_qty, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[9];
        paramList[0] = new PassDataToSql("@Trans_ID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Crm_campaign_id", Crm_campaign_id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Product_id", Product_id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Is_Active", "true", PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Created_By", Created_By, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Modified_By", Modified_By, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Target_sales_qty", Target_sales_qty, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@OpType", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("Sp_crm_campaign_Product_Insert", paramList, ref trns);
        return Convert.ToInt32(paramList[8].ParaValue);
    }

    
    public int DeleteProduct(string Crm_campaign_id, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[1];
        paramList[0] = new PassDataToSql("@Crm_campaign_id", Crm_campaign_id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("Sp_crm_campaign_Product_Delete", paramList, ref trns);
        return 1;
    }


    public DataTable ProductDataByCampaignId(string Trans_ID)
    {
        PassDataToSql[] paramList = new PassDataToSql[1];
        paramList[0] = new PassDataToSql("@Trans_ID", Trans_ID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("Sp_crm_campaign_Product_By_Campaign_Id", paramList))
        {
            return dtInfo;
        }
    }
    public DataTable AllContactData()
    {
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Ems_ContactMaster_SelectRow", paramList))
        {
            return dtInfo;
        }
    }

    public int InsertContact(string Crm_campaign_id, string contact_id,string emp_toVisit,string visitdate,string visitCloseDate, string Created_By, string Modified_By,   ref SqlTransaction trns)
    {
       
        PassDataToSql[] paramList = new PassDataToSql[12];
        paramList[0] = new PassDataToSql("@trans_id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Crm_campaign_id", Crm_campaign_id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@contact_id", contact_id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@IsActive", "true", PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);        
        paramList[4] = new PassDataToSql("@Field1", emp_toVisit, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Field2", visitdate, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Field3", visitCloseDate, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Field4", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Field5", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);        
        paramList[9] = new PassDataToSql("@Created_by", Created_By, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);       
        paramList[10] = new PassDataToSql("@Modified_by", Modified_By, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);       
        paramList[11] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("sp_crm_campaign_Contact_Insert", paramList, ref trns);
        return 1;
    }

    public int DeleteContact(string Crm_campaign_id,  ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[1];
        paramList[0] = new PassDataToSql("@Crm_campaign_id", Crm_campaign_id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
       daClass.execute_Sp("sp_crm_campaign_Contact_Delete", paramList, ref trns);
        return 1;
    }

 
    public DataTable ContactDataByCampaignId(string Trans_ID)
    {
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Trans_ID", Trans_ID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Is_Active", "true", PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("Sp_crm_campaign_Contact_By_Campaign_Id", paramList))
        {
            return dtInfo;
        }
    }

   public DataTable GetCampaignByID(string companyID,string brandID,string locationID, string Trans_ID)
    {
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Company_ID", companyID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_ID", brandID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_ID", locationID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans_ID", Trans_ID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Is_Active", "true", PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("Sp_crm_campaign_By_TransID", paramList))
        {
            return dtInfo;
        }

    }

    public string GetCampaignNameByID(string Trans_ID)
    {
        DataTable dtInfo = daClass.return_DataTable("select Campaign_name from crm_campaign where Trans_ID='"+Trans_ID+"'");
        if(dtInfo.Rows.Count>0)
        {
            return dtInfo.Rows[0][0].ToString()+"/"+Trans_ID;
        }

        return "";
    }
    public DataTable GetCampaignNameByPreText(string companyId,string brandid,string locationid, string preText)
    {
        using (DataTable dtInfo = daClass.return_DataTable("select Campaign_name+'/'+cast(Trans_ID as varchar(10)) as FilteredText from crm_campaign where Campaign_name + '/' + cast(Trans_ID as varchar(10)) like '%" + preText + "%' and company_id='" + companyId + "' and brand_id='" + brandid + "' and location_id='" + locationid + "'")) 
        {
            return dtInfo;
        }        
    }
}