using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using PegasusDataAccess;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Set_Approval_Employee
/// </summary>
public class Set_Approval_Employee
{
    DataAccessClass daClass = null;

    ObjectMaster objObject = null;

    SystemParameter objSys = null;
    Set_ApprovalMaster ObjApproval = null;

    public Set_Approval_Employee(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
        objObject = new ObjectMaster(strConString);
        ObjApproval = new Set_ApprovalMaster(strConString);
        objSys = new SystemParameter(strConString);

    }
    public DataTable GetApprovalTransation(string CompanyId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Department_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Op_Type", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_Approval_Transaction_SelectRow", paramList);
        return dtInfo;
    }

    public DataTable GetApprovalTransationNew(string CompanyId, string strRefId, string strApprovalName, string strApprovalId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];

        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strRefId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strApprovalName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Department_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@OpType", "4", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Approval_Id", strApprovalId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Status", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("Sp_set_approval_trans_selectrow_status_New", paramList);

        return dtInfo;
    }

    public DataTable GetApprovalTransationPayment(string CompanyId, string strRefId, string strApprovalId, ref SqlTransaction trns)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];

        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strRefId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Department_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@OpType", "9", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Approval_Id", strApprovalId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Status", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("Sp_set_approval_trans_selectrow_status_New", paramList, ref trns);

        return dtInfo;
    }

    public DataTable GetAttendanceLog(string CompanyId, string strEmpId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];

        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strEmpId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Department_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@OpType", "6", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Approval_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Status", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("Sp_set_approval_trans_selectrow_status_New", paramList);

        return dtInfo;
    }

    public DataTable GetApprovalTransationNewWithTransId(string CompanyId, string strTransId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];

        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strTransId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Department_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@OpType", "5", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Approval_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Status", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("Sp_set_approval_trans_selectrow_status_New", paramList);

        return dtInfo;
    }

    public DataTable GetApprovalTransationWithFilter(string CompanyId, string Brand_Id, string Location_Id, string Department_Id, string Optype, string FilterText)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[6];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", Brand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", Location_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Department_Id", Department_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Op_Type", Optype, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@FilterText", FilterText, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_Approval_Transaction_SelectRow_Filter", paramList);
        return dtInfo;
    }
    public DataTable GetApprovalTransation(string CompanyId, string EmpId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Department_Id", EmpId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Op_Type", "4", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_Approval_Transaction_SelectRow", paramList);
        return dtInfo;
    }

    public DataTable GetApprovalTransationbyTransId(string CompanyId, string strTransId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Company_id", CompanyId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Trans_Id", strTransId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Op_Type", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_Approval_Transaction_SelectRow_ByTransid", paramList);
        return dtInfo;
    }



    //created for rollback Trsnaction
    //15-02-2016
    //by jitendra upadhyay

    public DataTable GetApprovalTransation(string CompanyId, ref SqlTransaction trns)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Department_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Op_Type", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_Approval_Transaction_SelectRow", paramList, ref trns);
        return dtInfo;
    }


    public DataTable GetApprovalTransationByTransId(string CompanyId, string TransId, ref SqlTransaction trns)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[6];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Department_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Op_Type", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Ref_Id", TransId.ToString(), PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_Approval_Transaction_SelectRow_Single", paramList, ref trns);
        return dtInfo;
    }


    // Modified By Nitin On 26/09/2014.........
    public DataTable GetApprovalChild(string companyid, string strApprovalId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[6];

        paramList[0] = new PassDataToSql("@Company_Id", companyid, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Department_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@OpType", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Approval_Id", strApprovalId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        //dtInfo = daClass.Reuturn_Datatable_Search(chain"sp_Set_Approval_Child_SelectRow", paramList);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_Approval_Trans_SelectRow", paramList);

        return dtInfo;
    }

    public DataTable GetApprovalChildTermination1(string companyid, string strApprovalId, string strEmpId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];

        paramList[0] = new PassDataToSql("@Company_Id", companyid, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Department_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@OpType", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Approval_Id", strApprovalId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Emp_Id", strEmpId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_Approval_Trans_SelectRow_Termination", paramList);

        return dtInfo;
    }
    public DataTable GetApprovalChildTermination2(string companyid, string strApprovalId, string strEmpId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];

        paramList[0] = new PassDataToSql("@Company_Id", companyid, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Department_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@OpType", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Approval_Id", strApprovalId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Emp_Id", strEmpId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_Approval_Trans_SelectRow_Termination", paramList);

        return dtInfo;
    }

    public DataTable GetApprovalChildByStatus_New(string companyid, string brand_month, string loc_year, string strApprovalId, string strStatus)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[6];

        paramList[0] = new PassDataToSql("@Company_Id", companyid, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", brand_month, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", loc_year, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Department_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@OpType", strStatus, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Approval_Id", strApprovalId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        //dtInfo = daClass.Reuturn_Datatable_Search(chain"sp_Set_Approval_Child_SelectRow", paramList);
        dtInfo = daClass.Reuturn_Datatable_Search("Sp_set_approval_trans_selectrow_status", paramList);

        return dtInfo;
    }

    public DataTable GetApprovalChildByStatus(string companyid, string strApprovalId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[6];

        paramList[0] = new PassDataToSql("@Company_Id", companyid, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Department_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@OpType", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Approval_Id", strApprovalId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("Sp_set_approval_trans_selectrow_status", paramList);

        return dtInfo;
    }

    public DataTable GetApprovalChildByStatus1(string companyid, string strBrandId, string strApprovalId, string strStatus)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];

        paramList[0] = new PassDataToSql("@Company_Id", companyid, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrandId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Department_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@OpType", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Approval_Id", strApprovalId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Status", strStatus, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("Sp_set_approval_trans_selectrow_status_New", paramList);

        return dtInfo;
    }
    public DataTable GetApprovalChildByStatus2(string companyid, string strBrandId, string strLocation, string strApprovalId, string strStatus)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];

        paramList[0] = new PassDataToSql("@Company_Id", companyid, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrandId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Department_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@OpType", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Approval_Id", strApprovalId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Status", strStatus, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("Sp_set_approval_trans_selectrow_status_New", paramList);

        return dtInfo;
    }
    public DataTable GetApprovalChildByStatus3(string companyid, string strBrandId, string strLocation, string strDepartment, string strApprovalId, string strStatus)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];

        paramList[0] = new PassDataToSql("@Company_Id", companyid, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrandId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocation, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Department_Id", strDepartment, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@OpType", "3", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Approval_Id", strApprovalId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Status", strStatus, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("Sp_set_approval_trans_selectrow_status_New", paramList);

        return dtInfo;
    }

    public DataTable GetApprovalChildByStatus4(string companyid, string strApprovalId, string strStatus)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];

        paramList[0] = new PassDataToSql("@Company_Id", companyid, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Department_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@OpType", "7", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Approval_Id", strApprovalId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Status", strStatus, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("Sp_set_approval_trans_selectrow_status_New", paramList);

        return dtInfo;
    }
    public DataTable GetApprovalChildByStatus5(string companyid, string strApprovalId, string strStatus)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];

        paramList[0] = new PassDataToSql("@Company_Id", companyid, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Department_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@OpType", "8", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Approval_Id", strApprovalId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Status", strStatus, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("Sp_set_approval_trans_selectrow_status_New", paramList);

        return dtInfo;
    }
    public DataTable GetApprovalChildByStatusWithFilter(string companyid, string BrandId, string LocationId, string DepartmentId, string strApprovalId, string optype, string FilterText)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];

        paramList[0] = new PassDataToSql("@Company_Id", companyid, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Department_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Approval_Id", strApprovalId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@OpType", optype, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@FilterText", FilterText, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("Sp_set_approval_trans_selectrow_status_Filter", paramList);

        return dtInfo;
    }

    public DataTable GetApprovalByObjectId(string CompanyId, string BrandId, string LocationId, string DeparetmentId, string ApprovalId, string OpType)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[6];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", BrandId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", LocationId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Department_Id", DeparetmentId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Approval_Id", ApprovalId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Op_Type", OpType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_Approval_Employee_Select", paramList);
        return dtInfo;
    }

    //created for rollback Trsnaction
    //15-02-2016
    //by jitendra upadhyay

    public DataTable GetApprovalByObjectId(string CompanyId, string BrandId, string LocationId, string DeparetmentId, string ApprovalId, string OpType, ref SqlTransaction trns)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[6];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", BrandId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", LocationId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Department_Id", DeparetmentId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Approval_Id", ApprovalId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Op_Type", OpType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_Approval_Employee_Select", paramList, ref trns);
        return dtInfo;
    }
    public DataTable GetApprovalToEmployee(string ApprovalId, string CompanyId, string BrandId, string LocationId, string Depid, string Optype)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[6];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", LocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Department_Id", Depid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Approval_Id", ApprovalId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Op_Type", Optype, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_Approval_Employee_Select", paramList);
        return dtInfo;
    }
    // Modified By Nitin Jain On 08/10/2014//
    public DataTable GetApprovalType()
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[0];
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Get_Approval_Master_Select", paramList);
        return dtInfo;
    }
    //---------------------------------------
    public int UpdateApprovalMaster(string Approval_Id, string CompanyId, string BrandId, string LocationId, string DepartmentId, string Emp_Id, string Priority, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[20];
        paramList[0] = new PassDataToSql("@Approval_Id", Approval_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Brand_Id", BrandId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Location_Id", LocationId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Department_Id", DepartmentId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Priority", Priority, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Is_Active", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Created_By", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Created_Date", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Modify_By", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@Modify_Date", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Set_Approval_Employee_Update", paramList);
        return Convert.ToInt32(paramList[19].ParaValue);
    }
    public int DeleteApprovalTransaciton(string Approval_Type, string Ref_Id, string Object_Id, string ApprovalId)
    {
        PassDataToSql[] paramList = new PassDataToSql[5];

        paramList[0] = new PassDataToSql("@Approval_Type", Approval_Type, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Ref_Id", Ref_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Object_Id", Object_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[4] = new PassDataToSql("@Approval_Id", ApprovalId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Set_Approval_Transaction_Delete", paramList);
        return Convert.ToInt32(paramList[3].ParaValue);
    }
    public int UpdateApprovalTransaciton(string Approval_Type, string Ref_Id, string Object_Id, string EmpId, string Status, string Description, string ApprovalId, string strTimeZoneId)
    {
        PassDataToSql[] paramList = new PassDataToSql[7];

        // paramList[0] = new PassDataToSql("@Approval_Type", Approval_Type, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[0] = new PassDataToSql("@Ref_Id", Ref_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", EmpId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Request_Date", Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Status", Status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Description", Description, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Approval_Id", ApprovalId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Set_Approval_Transaction_Update", paramList);
        return Convert.ToInt32(paramList[6].ParaValue);
    }

    public int UpdateApprovalTransacitonByEmpId(string Approval_Type, string Ref_Id, string Object_Id, string EmpId, string Status, string Description, string ApprovalId, string strTimeZoneId)
    {
        PassDataToSql[] paramList = new PassDataToSql[7];

        paramList[0] = new PassDataToSql("@Ref_Id", Ref_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", EmpId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Request_Date", Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Status", Status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Description", Description, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Approval_Id", ApprovalId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Set_Approval_Transaction_UpdateByEmpId", paramList);
        return Convert.ToInt32(paramList[6].ParaValue);
    }

    public int UpdateApprovalTransacitonByEmpId(string Approval_Type, string Ref_Id, string Object_Id, string EmpId, string Status, string Description, string ApprovalId, ref SqlTransaction trns, string strTimeZoneId)
    {
        PassDataToSql[] paramList = new PassDataToSql[7];

        paramList[0] = new PassDataToSql("@Ref_Id", Ref_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", EmpId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Request_Date", Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Status", Status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Description", Description, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Approval_Id", ApprovalId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Set_Approval_Transaction_UpdateByEmpId", paramList, ref trns);
        return Convert.ToInt32(paramList[6].ParaValue);
    }

    //created for rollback Trsnaction
    //15-02-2016
    //by jitendra upadhyay

    public int UpdateApprovalTransaciton(string Approval_Type, string Ref_Id, string Object_Id, string EmpId, string Status, string Description, string ApprovalId, string strActionBy, ref SqlTransaction trns, string strTimeZoneId)
    {
        PassDataToSql[] paramList = new PassDataToSql[8];

        // paramList[0] = new PassDataToSql("@Approval_Type", Approval_Type, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[0] = new PassDataToSql("@Ref_Id", Ref_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", EmpId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Request_Date", Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Status", Status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Description", Description, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Approval_Id", ApprovalId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[7] = new PassDataToSql("@Action_By", strActionBy, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_Set_Approval_Transaction_Update", paramList, ref trns);
        return Convert.ToInt32(paramList[6].ParaValue);
    }
    public int InsertApprovalTransaciton(string Approval_Id, string CompanyId, string BrandId, string LocationId, string DepId, string ReqEmpId, string RefId, string EmpId, string IsDefault, string RequestDate, string StstusUpdateDate, string Status, string Description, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifyBy, string ModifyDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[26];

        paramList[0] = new PassDataToSql("@Approval_Id ", Approval_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Company_Id ", CompanyId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Brand_Id ", BrandId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Location_Id ", LocationId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Department_Id ", DepId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@RequestEmp_Id ", ReqEmpId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Ref_Id ", RefId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Emp_Id ", EmpId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@IsDefault ", IsDefault, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@RequestDate ", RequestDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@StatusUpdate_Date", StstusUpdateDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Status", Status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Description", Description, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@Is_Active", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@ModifiedBy", ModifyBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[24] = new PassDataToSql("@ModifiedDate", ModifyDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[25] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);


        daClass.execute_Sp("sp_Set_Approval_Transaction_Insert", paramList);
        return Convert.ToInt32(paramList[25].ParaValue);
    }
    //created for rollback Trsnaction
    //15-02-2016
    //by jitendra upadhyay




    public int InsertApprovalTransaciton(string Approval_Id, string CompanyId, string BrandId, string LocationId, string DepId, string ReqEmpId, string RefId, string EmpId, string IsDefault, string RequestDate, string StstusUpdateDate, string Status, string Description, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifyBy, string ModifyDate, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[26];

        paramList[0] = new PassDataToSql("@Approval_Id ", Approval_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Company_Id ", CompanyId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Brand_Id ", BrandId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Location_Id ", LocationId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Department_Id ", DepId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@RequestEmp_Id ", ReqEmpId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Ref_Id ", RefId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Emp_Id ", EmpId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@IsDefault ", IsDefault, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@RequestDate ", RequestDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@StatusUpdate_Date", StstusUpdateDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Status", Status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Description", Description, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@Is_Active", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@ModifiedBy", ModifyBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[24] = new PassDataToSql("@ModifiedDate", ModifyDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[25] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);


        daClass.execute_Sp("sp_Set_Approval_Transaction_Insert", paramList, ref trns);
        return Convert.ToInt32(paramList[25].ParaValue);
    }

    public string GetApprovaId(string ObjectId, string ApprovalType, string CompanyId)
    {
        string ApprovalId = string.Empty;
        //DataTable dt = GetApprovalByObjectId(ObjectId);
        //dt = new DataView(dt, "Approval='" + ApprovalType + "' and Company_Id='" + CompanyId + "' ", "", DataViewRowState.CurrentRows).ToTable();
        //if (dt.Rows.Count > 0)
        //{
        //    ApprovalId = dt.Rows[0]["Approval_Id"].ToString();
        //}
        return ApprovalId;
    }

    //public int InsertApprovalChildMaster(string Approval_Type, string Ref_Id, string Object_Id, string EmployeeId, string ApplyDate)
    //{
    //    string Approval_Id = string.Empty;
    //    Approval_Id = GetApprovaId(Object_Id, Approval_Type, HttpContext.Current.Session["CompId"].ToString());
    //    PassDataToSql[] paramList = new PassDataToSql[8];
    //    paramList[0] = new PassDataToSql("@Approval_Type", Approval_Type, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
    //    paramList[1] = new PassDataToSql("@Ref_Id", Ref_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

    //    paramList[2] = new PassDataToSql("@Object_Id", Object_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
    //    paramList[3] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

    //    paramList[4] = new PassDataToSql("@Request_Date", Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString()).ToString(), PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
    //    paramList[5] = new PassDataToSql("@Approval_Id", Approval_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
    //    paramList[6] = new PassDataToSql("@Emp_Id", EmployeeId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
    //    paramList[7] = new PassDataToSql("@Apply_Date", ApplyDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);


    //    daClass.execute_Sp("sp_Set_Approval_Child_Insert", paramList);
    //    return Convert.ToInt32(paramList[3].ParaValue);
    //}

    public int InsertApprovalMaster(string Approval_Id, string CompanyId, string BrandId, string LocationId, string DepartmentId, string Emp_Id, string Priority, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[20];
        paramList[0] = new PassDataToSql("@Approval_Id", Approval_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Brand_Id", BrandId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Location_Id", LocationId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Department_Id", DepartmentId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Priority", Priority, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Is_Active", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Created_By", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Created_Date", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Modify_By", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@Modify_Date", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Set_Approval_Employee_Insert", paramList);
        return Convert.ToInt32(paramList[19].ParaValue);
    }
    public int DeleteEmployeeApproval(string ApprovalId, string CompId, string BrandId, string LocId, string DepId, string OpType)
    {
        PassDataToSql[] paramList = new PassDataToSql[6];

        paramList[0] = new PassDataToSql("@Approval_Id", ApprovalId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Company_Id", CompId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Brand_Id ", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Location_Id ", LocId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Department_Id ", DepId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@OpType  ", OpType, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_Set_Approval_Employee_Delete", paramList);
        return Convert.ToInt32(paramList[5].ParaValue);
    }
    public int DeleteEmpLoanApproval(string ApprovalId, string CompId, string BrandId, string LocationId, string DeptId, string RefId)
    {
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Approval_Id", ApprovalId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Company_Id", CompId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@BrandId", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@LocationId", LocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@DepID", DeptId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@RefId", RefId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@OutID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_Set_Approval_Transaction_Delete", paramList);
        return Convert.ToInt32(paramList[6].ParaValue);
    }
    public int DeleteDuplicate_Record(string Level_ID, string Approval_ID)
    {
        string Op_Type = string.Empty;
        if (Level_ID == "1")
        {
            Op_Type = "1";
        }
        else if (Level_ID == "2")
        {
            Op_Type = "2";
        }
        else if (Level_ID == "3")
        {
            Op_Type = "3";
        }
        else if (Level_ID == "4")
        {
            Op_Type = "4";
        }
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Op_Type", Op_Type, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Approval_ID", Approval_ID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Ref_ID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("sp_Set_Approval_Employee_Duplicate", paramList);
        return Convert.ToInt32(paramList[2].ParaValue);
    }
    public int DeleteEmpLoanApproval(string ApprovalId, string CompId, string BrandId, string LocationId, string DeptId, string RefId, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Approval_Id", ApprovalId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Company_Id", CompId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@BrandId", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@LocationId", LocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@DepID", DeptId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@RefId", RefId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@OutID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_Set_Approval_Transaction_Delete", paramList, ref trns);
        return Convert.ToInt32(paramList[6].ParaValue);
    }

    public string GetApprovalTransactionStatus(string Approval_Type, string Ref_Id, string CompanyId)
    {
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Approval_Type", Approval_Type, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Ref_Id", Ref_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        DataTable dtInfo = new DataTable();
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_Approval_Transaction_SelectStatus", paramList);
        if (dtInfo != null && dtInfo.Rows.Count > 0)
        {
            return (dtInfo.Rows[0]["Status"].ToString());
        }
        else
        {
            return "";
        }
    }
    public int Delete_Approval_Transaction(string Approval_Id, string Company_Id, string Brand_Id, string Location_Id, string Dep_Id, string Ref_Id)
    {
        PassDataToSql[] paramList = new PassDataToSql[7];

        paramList[0] = new PassDataToSql("@Approval_Id", Approval_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Brand_Id", Brand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Location_Id", Location_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Dep_Id", Dep_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Ref_Id", Ref_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("sp_Set_Approval_Transaction_DeleteByAppTypeNRefID", paramList);
        return Convert.ToInt32(paramList[6].ParaValue);
    }

    public int Delete_Approval_Transaction(string Approval_Id, string Company_Id, string Brand_Id, string Location_Id, string Dep_Id, string Ref_Id, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[7];

        paramList[0] = new PassDataToSql("@Approval_Id", Approval_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Brand_Id", Brand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Location_Id", Location_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Dep_Id", Dep_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Ref_Id", Ref_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("sp_Set_Approval_Transaction_DeleteByAppTypeNRefID", paramList, ref trns);
        return Convert.ToInt32(paramList[6].ParaValue);
    }

    //For pending Approval
    public DataTable getApprovalTransByRef_IDandApprovalId(string Ref_ID, string Approval_Id)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Ref_Id", Ref_ID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Approval_Id", Approval_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Op_Type", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_Approval_Transaction_SelectRowByRef_ID", paramList);
        return dtInfo;
    }



    public DataTable getApprovalTransByRef_IDandApprovalId(string Ref_ID, string Approval_Id, ref SqlTransaction trns)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Ref_Id", Ref_ID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Approval_Id", Approval_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Op_Type", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_Approval_Transaction_SelectRowByRef_ID", paramList, ref trns);
        return dtInfo;
    }

    public int UpdateApprovalTransacitonPriority(string Ref_Id, string ApprovalId, string Emp_Id, string Is_Default)
    {

        PassDataToSql[] paramList = new PassDataToSql[5];

        paramList[0] = new PassDataToSql("@Ref_Id", Ref_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Approval_Id", ApprovalId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[3] = new PassDataToSql("@Is_Default", Is_Default, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[4] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Set_Approval_Transaction_UpdatePriority", paramList);
        return Convert.ToInt32(paramList[4].ParaValue);
    }

    public DataTable getApprovalTypeByEmpId(string Emp_Id)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Op_Type", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Get_ApprovalType_ByEmployee", paramList);
        return dtInfo;
    }


    public DataTable getApprovalChainByObjectid(string strCompanyId, string strBrandId, string strLocationId, string strobjectId, string strEmployeeId)
    {
        string TeamLeader = string.Empty;
        string DepartmentManager = string.Empty;
        string ParentDepartmentManager = string.Empty;
        string ResponsibleDepartmentManager = string.Empty;


        DataTable dtEmp = new DataTable();

        DataTable dt1 = new DataTable();
        //first we are checking that any approval type is linked or not with object id 
        string strApprovalId = objObject.GetObjectMasterById(strobjectId).Rows[0]["Approval_Id"].ToString();

        if (strApprovalId != "0")
        {

            if (ObjApproval.GetApprovalMasterById(strApprovalId).Rows[0]["Approval_Type"] != null)
            {

                //for priority


                if (ObjApproval.GetApprovalMasterById(strApprovalId).Rows[0]["Approval_Type"].ToString().Trim() == "Priority")
                {

                    string EmpPermission = string.Empty;

                    EmpPermission = objSys.Get_Approval_Parameter_By_ID(strApprovalId).Rows[0]["Approval_Level"].ToString();
                    if (EmpPermission == "1")
                    {
                        dt1 = GetApprovalByObjectId(strCompanyId, "0", "0", "0", strApprovalId, "1");
                    }
                    else if (EmpPermission == "2")
                    {
                        dt1 = GetApprovalByObjectId(strCompanyId, strBrandId, "0", "0", strApprovalId, "2");
                    }
                    else if (EmpPermission == "3")
                    {
                        dt1 = GetApprovalByObjectId(strCompanyId, strBrandId, strLocationId, "0", strApprovalId, "3");
                    }
                    else
                    {
                        dt1 = GetApprovalByObjectId(strCompanyId, strBrandId, strLocationId, "0", strApprovalId, "4");
                    }
                }

                //if hierarchy
                else if (ObjApproval.GetApprovalMasterById(strApprovalId).Rows[0]["Approval_Type"].ToString().Trim() == "Hierarchy")
                {
                    bool IsEmail = false;

                    DataTable dtTemp = ObjApproval.GetApprovalMasterById(strApprovalId);
                    if (dtTemp.Rows.Count > 0)
                    {

                        int counter = 0;

                        DataTable dtAppprovalList = new DataTable();


                        dtAppprovalList.Columns.Add("Emp_Id");
                        dtAppprovalList.Columns.Add("Priority");
                        dtAppprovalList.Columns.Add("Field1");
                        dtAppprovalList.Columns.Add("Approval_Id");

                        //here we are checking teaam leader

                        if (Convert.ToBoolean(dtTemp.Rows[0]["Is_TeamLeader"].ToString()))
                        {
                            TeamLeader = GetTeamLeader(strEmployeeId);
                            if (TeamLeader != "0")
                            {
                                DataRow dr = dtAppprovalList.NewRow();
                                dr[0] = TeamLeader;
                                dr[1] = "False";
                                dr[2] = "True";
                                dr[3] = strApprovalId;
                                IsEmail = true;
                                dtAppprovalList.Rows.Add(dr);
                            }
                            else
                            {
                                counter = 1;
                            }
                        }
                        if (Convert.ToBoolean(dtTemp.Rows[0]["Is_DepartmentManager"].ToString()))
                        {
                            //DepartmentManager = GetDepartmentManager(strLocationId, strEmployeeId);
                            string a = GetDepartmentManager(strEmployeeId, strLocationId);
                            string[] authorsList = a.Split(',');
                            foreach (string author in authorsList)
                            {
                                DepartmentManager = author;
                                if (DepartmentManager != "")
                                {
                                    if (new DataView(dtAppprovalList, "Emp_Id=" + DepartmentManager + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                                    {
                                        dtAppprovalList = new DataView(dtAppprovalList, "Emp_Id<>" + DepartmentManager + "", "", DataViewRowState.CurrentRows).ToTable();

                                    }


                                    DataRow dr = dtAppprovalList.NewRow();
                                    dr[0] = DepartmentManager;
                                    dr[1] = "False";
                                    if (new DataView(dtAppprovalList, "Field1='True'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                                    {
                                        dr[2] = "False";

                                    }
                                    else
                                    {
                                        dr[2] = "True";
                                    }
                                    dr[3] = strApprovalId;

                                    dtAppprovalList.Rows.Add(dr);
                                }
                                else
                                {
                                    counter = 1;
                                }

                            }
                        }
                        if (Convert.ToBoolean(dtTemp.Rows[0]["Is_Parent_departmentManager"].ToString()))
                        {
                            ParentDepartmentManager = GetParentDepartmentManager(strEmployeeId, strLocationId);

                            if (ParentDepartmentManager != "0")
                            {


                                if (new DataView(dtAppprovalList, "Emp_Id=" + ParentDepartmentManager + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                                {
                                    dtAppprovalList = new DataView(dtAppprovalList, "Emp_Id<>" + ParentDepartmentManager + "", "", DataViewRowState.CurrentRows).ToTable();

                                }

                                DataRow dr = dtAppprovalList.NewRow();

                                dr[0] = ParentDepartmentManager;
                                dr[1] = "False";
                                if (new DataView(dtAppprovalList, "Field1='True'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                                {
                                    dr[2] = "False";

                                }
                                else
                                {
                                    dr[2] = "True";
                                }
                                dr[3] = strApprovalId;
                                dtAppprovalList.Rows.Add(dr);
                            }
                            else
                            {
                                counter = 1;
                            }


                        }

                        //for responsibe department
                        ResponsibleDepartmentManager = GetResponsibleDepartmentManager(dtTemp.Rows[0]["ResponsibleDepartmentManager"].ToString(), strLocationId);
                        if (ResponsibleDepartmentManager != "0")
                        {

                            if (new DataView(dtAppprovalList, "Emp_Id=" + ResponsibleDepartmentManager + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                            {
                                dtAppprovalList = new DataView(dtAppprovalList, "Emp_Id<>" + ResponsibleDepartmentManager + "", "", DataViewRowState.CurrentRows).ToTable();
                            }

                            DataRow dr = dtAppprovalList.NewRow();
                            dr[0] = ResponsibleDepartmentManager;
                            dr[1] = "True";
                            if (new DataView(dtAppprovalList, "Field1='True'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                            {
                                dr[2] = "False";
                            }
                            else
                            {
                                dr[2] = "True";
                            }
                            dr[3] = strApprovalId;
                            dtAppprovalList.Rows.Add(dr);
                        }
                        else
                        {
                            counter = 1;
                        }


                        if ((counter == 0) || dtAppprovalList.Rows.Count > 0)
                        {
                            dt1 = dtAppprovalList.Copy();
                        }
                    }
                }
            }

        }
        return dt1;
    }



    //public DataTable getApprovalChainByObjectid(string strobjectId, string strEmployeeId,string strCompanyId,string strBrandId,string strLocId)
    //{
    //    string TeamLeader = string.Empty;
    //    string DepartmentManager = string.Empty;
    //    string ParentDepartmentManager = string.Empty;
    //    string ResponsibleDepartmentManager = string.Empty;


    //    DataTable dtEmp = new DataTable();

    //    DataTable dt1 = new DataTable();
    //    //first we are checking that any approval type is linked or not with object id 
    //    string strApprovalId = objObject.GetObjectMasterById(strobjectId).Rows[0]["Approval_Id"].ToString();

    //    if (strApprovalId != "0")
    //    {

    //        if (ObjApproval.GetApprovalMasterById(strApprovalId).Rows[0]["Approval_Type"] != null)
    //        {

    //            //for priority


    //            if (ObjApproval.GetApprovalMasterById(strApprovalId).Rows[0]["Approval_Type"].ToString().Trim() == "Priority")
    //            {

    //                string EmpPermission = string.Empty;

    //                EmpPermission = objSys.Get_Approval_Parameter_By_ID(strApprovalId).Rows[0]["Approval_Level"].ToString();
    //                if (EmpPermission == "1")
    //                {
    //                    dt1 = GetApprovalByObjectId(strCompanyId, "0", "0", "0", strApprovalId, "1");
    //                }
    //                else if (EmpPermission == "2")
    //                {
    //                    dt1 = GetApprovalByObjectId(strCompanyId, strBrandId, "0", "0", strApprovalId, "2");
    //                }
    //                else if (EmpPermission == "3")
    //                {
    //                    dt1 = GetApprovalByObjectId(strCompanyId, strBrandId, strLocId, "0", strApprovalId, "3");
    //                }
    //                else
    //                {
    //                    dt1 = GetApprovalByObjectId(strCompanyId, strBrandId, strLocId, "0", strApprovalId, "4");
    //                }
    //            }

    //            //if hierarchy
    //            else if (ObjApproval.GetApprovalMasterById(strApprovalId).Rows[0]["Approval_Type"].ToString().Trim() == "Hierarchy")
    //            {
    //                bool IsEmail = false;

    //                DataTable dtTemp = ObjApproval.GetApprovalMasterById(strApprovalId);
    //                if (dtTemp.Rows.Count > 0)
    //                {

    //                    int counter = 0;

    //                    DataTable dtAppprovalList = new DataTable();


    //                    dtAppprovalList.Columns.Add("Emp_Id");
    //                    dtAppprovalList.Columns.Add("Priority");
    //                    dtAppprovalList.Columns.Add("Field1");
    //                    dtAppprovalList.Columns.Add("Approval_Id");

    //                    //here we are checking teaam leader

    //                    if (Convert.ToBoolean(dtTemp.Rows[0]["Is_TeamLeader"].ToString()))
    //                    {
    //                        TeamLeader = GetTeamLeader(strEmployeeId);
    //                        if (TeamLeader != "0")
    //                        {
    //                            DataRow dr = dtAppprovalList.NewRow();
    //                            dr[0] = TeamLeader;
    //                            dr[1] = "False";
    //                            dr[2] = "True";
    //                            dr[3] = strApprovalId;
    //                            IsEmail = true;
    //                            dtAppprovalList.Rows.Add(dr);
    //                        }
    //                        else
    //                        {
    //                            counter = 1;
    //                        }
    //                    }
    //                    if (Convert.ToBoolean(dtTemp.Rows[0]["Is_DepartmentManager"].ToString()))
    //                    {
    //                        DepartmentManager = GetDepartmentManager(strEmployeeId);

    //                        if (DepartmentManager != "0")
    //                        {




    //                            if (new DataView(dtAppprovalList, "Emp_Id=" + DepartmentManager + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
    //                            {
    //                                dtAppprovalList = new DataView(dtAppprovalList, "Emp_Id<>" + DepartmentManager + "", "", DataViewRowState.CurrentRows).ToTable();

    //                            }


    //                            DataRow dr = dtAppprovalList.NewRow();
    //                            dr[0] = DepartmentManager;
    //                            dr[1] = "False";
    //                            if (new DataView(dtAppprovalList, "Field1='True'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
    //                            {
    //                                dr[2] = "False";

    //                            }
    //                            else
    //                            {
    //                                dr[2] = "True";
    //                            }
    //                            dr[3] = strApprovalId;

    //                            dtAppprovalList.Rows.Add(dr);
    //                        }
    //                        else
    //                        {
    //                            counter = 1;
    //                        }


    //                    }
    //                    if (Convert.ToBoolean(dtTemp.Rows[0]["Is_Parent_departmentManager"].ToString()))
    //                    {
    //                        ParentDepartmentManager = GetParentDepartmentManager(strEmployeeId);

    //                        if (ParentDepartmentManager != "0")
    //                        {


    //                            if (new DataView(dtAppprovalList, "Emp_Id=" + ParentDepartmentManager + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
    //                            {
    //                                dtAppprovalList = new DataView(dtAppprovalList, "Emp_Id<>" + ParentDepartmentManager + "", "", DataViewRowState.CurrentRows).ToTable();

    //                            }

    //                            DataRow dr = dtAppprovalList.NewRow();

    //                            dr[0] = ParentDepartmentManager;
    //                            dr[1] = "False";
    //                            if (new DataView(dtAppprovalList, "Field1='True'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
    //                            {
    //                                dr[2] = "False";

    //                            }
    //                            else
    //                            {
    //                                dr[2] = "True";
    //                            }
    //                            dr[3] = strApprovalId;
    //                            dtAppprovalList.Rows.Add(dr);
    //                        }
    //                        else
    //                        {
    //                            counter = 1;
    //                        }


    //                    }

    //                    //for responsibe department
    //                    ResponsibleDepartmentManager = GetResponsibleDepartmentManager(dtTemp.Rows[0]["ResponsibleDepartmentManager"].ToString());
    //                    if (ResponsibleDepartmentManager != "0")
    //                    {

    //                        if (new DataView(dtAppprovalList, "Emp_Id=" + ResponsibleDepartmentManager + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
    //                        {
    //                            dtAppprovalList = new DataView(dtAppprovalList, "Emp_Id<>" + ResponsibleDepartmentManager + "", "", DataViewRowState.CurrentRows).ToTable();
    //                        }

    //                        DataRow dr = dtAppprovalList.NewRow();
    //                        dr[0] = ResponsibleDepartmentManager;
    //                        dr[1] = "True";
    //                        if (new DataView(dtAppprovalList, "Field1='True'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
    //                        {
    //                            dr[2] = "False";
    //                        }
    //                        else
    //                        {
    //                            dr[2] = "True";
    //                        }
    //                        dr[3] = strApprovalId;
    //                        dtAppprovalList.Rows.Add(dr);
    //                    }
    //                    else
    //                    {
    //                        counter = 1;
    //                    }


    //                    if (counter == 0)
    //                    {
    //                        dt1 = dtAppprovalList.Copy();
    //                    }
    //                }
    //            }
    //        }

    //    }
    //    return dt1;
    //}




    public DataTable getApprovalChainByObjectid(string strobjectId, string strEmployeeId, ref SqlTransaction trns, string strCompanyId, string strBrandId, string strLocId)
    {
        string TeamLeader = string.Empty;
        string DepartmentManager = string.Empty;
        string ParentDepartmentManager = string.Empty;
        string ResponsibleDepartmentManager = string.Empty;


        DataTable dtEmp = new DataTable();

        DataTable dt1 = new DataTable();
        //first we are checking that any approval type is linked or not with object id 
        string strApprovalId = objObject.GetObjectMasterById(strobjectId, ref trns).Rows[0]["Approval_Id"].ToString();

        if (strApprovalId != "0")
        {

            if (ObjApproval.GetApprovalMasterById(strApprovalId, ref trns).Rows[0]["Approval_Type"] != null)
            {

                //for priority


                if (ObjApproval.GetApprovalMasterById(strApprovalId, ref trns).Rows[0]["Approval_Type"].ToString().Trim() == "Priority")
                {

                    string EmpPermission = string.Empty;

                    EmpPermission = objSys.Get_Approval_Parameter_By_ID(strApprovalId).Rows[0]["Approval_Level"].ToString();
                    if (EmpPermission == "1")
                    {
                        dt1 = GetApprovalByObjectId(strCompanyId, "0", "0", "0", strApprovalId, "1", ref trns);
                    }
                    else if (EmpPermission == "2")
                    {
                        dt1 = GetApprovalByObjectId(strCompanyId, strBrandId, "0", "0", strApprovalId, "2", ref trns);
                    }
                    else if (EmpPermission == "3")
                    {
                        dt1 = GetApprovalByObjectId(strCompanyId, strBrandId, strLocId, "0", strApprovalId, "3", ref trns);
                    }
                    else
                    {
                        dt1 = GetApprovalByObjectId(strCompanyId, strBrandId, strLocId, "0", strApprovalId, "4", ref trns);
                    }
                }

                //if hierarchy
                else if (ObjApproval.GetApprovalMasterById(strApprovalId, ref trns).Rows[0]["Approval_Type"].ToString().Trim() == "Hierarchy")
                {
                    bool IsEmail = false;

                    DataTable dtTemp = ObjApproval.GetApprovalMasterById(strApprovalId, ref trns);
                    if (dtTemp.Rows.Count > 0)
                    {

                        int counter = 0;

                        DataTable dtAppprovalList = new DataTable();


                        dtAppprovalList.Columns.Add("Emp_Id");
                        dtAppprovalList.Columns.Add("Priority");
                        dtAppprovalList.Columns.Add("Field1");
                        dtAppprovalList.Columns.Add("Approval_Id");

                        //here we are checking teaam leader

                        if (Convert.ToBoolean(dtTemp.Rows[0]["Is_TeamLeader"].ToString()))
                        {
                            TeamLeader = GetTeamLeader(strEmployeeId, ref trns);
                            if (TeamLeader != "0")
                            {
                                DataRow dr = dtAppprovalList.NewRow();
                                dr[0] = TeamLeader;
                                dr[1] = "False";
                                dr[2] = "True";
                                dr[3] = strApprovalId;
                                IsEmail = true;
                                dtAppprovalList.Rows.Add(dr);
                            }
                            else
                            {
                                counter = 1;
                            }
                        }
                        if (Convert.ToBoolean(dtTemp.Rows[0]["Is_DepartmentManager"].ToString()))
                        {
                            DepartmentManager = GetDepartmentManager(strEmployeeId, ref trns, strLocId);

                            if (DepartmentManager != "0")
                            {




                                if (new DataView(dtAppprovalList, "Emp_Id=" + DepartmentManager + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                                {
                                    dtAppprovalList = new DataView(dtAppprovalList, "Emp_Id<>" + DepartmentManager + "", "", DataViewRowState.CurrentRows).ToTable();

                                }


                                DataRow dr = dtAppprovalList.NewRow();
                                dr[0] = DepartmentManager;
                                dr[1] = "False";
                                if (new DataView(dtAppprovalList, "Field1='True'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                                {
                                    dr[2] = "False";

                                }
                                else
                                {
                                    dr[2] = "True";
                                }
                                dr[3] = strApprovalId;

                                dtAppprovalList.Rows.Add(dr);
                            }
                            else
                            {
                                counter = 1;
                            }


                        }
                        if (Convert.ToBoolean(dtTemp.Rows[0]["Is_Parent_departmentManager"].ToString()))
                        {
                            ParentDepartmentManager = GetParentDepartmentManager(strEmployeeId, ref trns, strLocId);

                            if (ParentDepartmentManager != "0")
                            {


                                if (new DataView(dtAppprovalList, "Emp_Id=" + ParentDepartmentManager + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                                {
                                    dtAppprovalList = new DataView(dtAppprovalList, "Emp_Id<>" + ParentDepartmentManager + "", "", DataViewRowState.CurrentRows).ToTable();

                                }

                                DataRow dr = dtAppprovalList.NewRow();

                                dr[0] = ParentDepartmentManager;
                                dr[1] = "False";
                                if (new DataView(dtAppprovalList, "Field1='True'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                                {
                                    dr[2] = "False";

                                }
                                else
                                {
                                    dr[2] = "True";
                                }
                                dr[3] = strApprovalId;
                                dtAppprovalList.Rows.Add(dr);
                            }
                            else
                            {
                                counter = 1;
                            }


                        }

                        //for responsibe department
                        ResponsibleDepartmentManager = GetResponsibleDepartmentManager(dtTemp.Rows[0]["ResponsibleDepartmentManager"].ToString(), ref trns, strLocId);
                        if (ResponsibleDepartmentManager != "0")
                        {

                            if (new DataView(dtAppprovalList, "Emp_Id=" + ResponsibleDepartmentManager + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                            {
                                dtAppprovalList = new DataView(dtAppprovalList, "Emp_Id<>" + ResponsibleDepartmentManager + "", "", DataViewRowState.CurrentRows).ToTable();
                            }

                            DataRow dr = dtAppprovalList.NewRow();
                            dr[0] = ResponsibleDepartmentManager;
                            dr[1] = "True";
                            if (new DataView(dtAppprovalList, "Field1='True'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                            {
                                dr[2] = "False";
                            }
                            else
                            {
                                dr[2] = "True";
                            }
                            dr[3] = strApprovalId;
                            dtAppprovalList.Rows.Add(dr);
                        }
                        else
                        {
                            counter = 1;
                        }


                        if (counter == 0)
                        {
                            dt1 = dtAppprovalList.Copy();
                        }
                    }
                }
            }

        }
        return dt1;
    }



    public string GetTeamLeader(string strEmpId)
    {
        string strTeamLeader = string.Empty;

        try
        {
            strTeamLeader = daClass.return_DataTable("select field5 from set_employeemaster where emp_id=" + strEmpId + "").Rows[0]["field5"].ToString();
        }
        catch
        {
            strTeamLeader = "0";
        }

        if (strTeamLeader == "")
        {
            strTeamLeader = "0";
        }

        return strTeamLeader;
    }

    public string GetTeamLeader(string strEmpId, ref SqlTransaction trns)
    {
        string strTeamLeader = string.Empty;

        try
        {
            strTeamLeader = daClass.return_DataTable("select field5 from set_employeemaster where emp_id=" + strEmpId + "", ref trns).Rows[0]["field5"].ToString();
        }
        catch
        {
            strTeamLeader = "0";
        }

        if (strTeamLeader == "")
        {
            strTeamLeader = "0";
        }

        return strTeamLeader;
    }

    public string GetDepartmentManager(string strEmpId, string strLocId)
    {
        string strDepartmentManager = string.Empty;


        /// DataTable dt = daClass.return_DataTable("select emp.emp_id from Set_EmployeeMaster  left join Set_Location_Department on Set_Location_Department.Dep_Id = Set_EmployeeMaster.Department_Id and  Set_Location_Department.Location_Id = Set_EmployeeMaster.Location_Id left join Set_EmployeeMaster as Emp on Set_Location_Department.Emp_Id = Emp.Emp_Id where Set_EmployeeMaster.Emp_Id=" + strEmpId + " ");
        //DataTable dt = daClass.return_DataTable("select emp.emp_id from Set_EmployeeMaster  left join Set_Location_Department on Set_Location_Department.Dep_Id = Set_EmployeeMaster.Department_Id left join Set_EmployeeMaster as Emp on Set_Location_Department.Emp_Id = Emp.Emp_Id where Set_EmployeeMaster.Emp_Id=" + strEmpId + " and Set_Location_Department.Location_Id=" + strLocId + "");
        DataTable dt = daClass.return_DataTable("select emp.emp_id from Set_EmployeeMaster left join Set_Location_Department on Set_Location_Department.Dep_Id = Set_EmployeeMaster.Department_Id and  Set_Location_Department.Location_Id = Set_EmployeeMaster.Location_Id left join Set_EmployeeMaster as Emp on(Set_Location_Department.Emp_Id = Emp.Emp_Id or Set_Location_Department.Field1 = Emp.Emp_Id or Set_Location_Department.Field2 = Emp.Emp_Id or Set_Location_Department.Field3 = Emp.Emp_Id or Set_Location_Department.Field4 = Emp.Emp_Id or Set_Location_Department.Field5 = Emp.Emp_Id) where Set_EmployeeMaster.Emp_Id = '" + strEmpId + "'");
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["emp_id"] != null)
            {

                if (dt.Rows[0]["emp_id"].ToString() != "0" && dt.Rows[0]["emp_id"].ToString() != "")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        strDepartmentManager = strDepartmentManager + dt.Rows[i]["emp_id"].ToString() + ",";
                    }

                }
            }
        }
        return strDepartmentManager;
    }
    public string GetDepartmentManager(string strEmpId, ref SqlTransaction trns, string strLocId)
    {
        string strDepartmentManager = string.Empty;

        strDepartmentManager = "0";
        //DataTable dt = daClass.return_DataTable("select emp.emp_id from Set_EmployeeMaster  left join Set_Location_Department on Set_Location_Department.Dep_Id = Set_EmployeeMaster.Department_Id left join Set_EmployeeMaster as Emp on Set_Location_Department.Emp_Id = Emp.Emp_Id where Set_EmployeeMaster.Emp_Id=" + strEmpId + " and Set_Location_Department.Location_Id=" + strLocId + "", ref trns);
        DataTable dt = daClass.return_DataTable("select emp.emp_id from Set_EmployeeMaster  left join Set_Location_Department on Set_Location_Department.Dep_Id = Set_EmployeeMaster.Department_Id left join Set_EmployeeMaster as Emp on Set_Location_Department.Emp_Id = Emp.Emp_Id where Set_EmployeeMaster.Emp_Id=" + strEmpId + " ", ref trns);

        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["emp_id"] != null)
            {
                if (dt.Rows[0]["emp_id"].ToString() != "0" && dt.Rows[0]["emp_id"].ToString() != "")
                {
                    strDepartmentManager = dt.Rows[0]["emp_id"].ToString();
                }
            }
        }


        return strDepartmentManager;
    }
    public string GetParentDepartmentManager(string strEmpId, string strLocId)
    {
        string strDepartmentManager = string.Empty;

        strDepartmentManager = "0";
        //DataTable dt = daClass.return_DataTable("select emp.emp_id from Set_EmployeeMaster left join Set_DepartmentMaster on   Set_EmployeeMaster.Department_Id =Set_DepartmentMaster.Dep_Id    left join Set_Location_Department on Set_Location_Department.Dep_Id = Set_DepartmentMaster.Parent_Id left join Set_EmployeeMaster as Emp on Set_Location_Department.Emp_Id = Emp.Emp_Id where Set_EmployeeMaster.Emp_Id=" + strEmpId + " and Set_Location_Department.Location_Id=" + strLocId + "");
        DataTable dt = daClass.return_DataTable("select emp.emp_id from Set_EmployeeMaster left join Set_DepartmentMaster on   Set_EmployeeMaster.Department_Id =Set_DepartmentMaster.Dep_Id    left join Set_Location_Department on Set_Location_Department.Dep_Id = Set_DepartmentMaster.Parent_Id and  Set_Location_Department.Location_Id = Set_EmployeeMaster.Location_Id left join Set_EmployeeMaster as Emp on Set_Location_Department.Emp_Id = Emp.Emp_Id where Set_EmployeeMaster.Emp_Id=" + strEmpId + " ");

        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["emp_id"] != null)
            {
                if (dt.Rows[0]["emp_id"].ToString() != "0" && dt.Rows[0]["emp_id"].ToString() != "")
                {

                    strDepartmentManager = dt.Rows[0]["emp_id"].ToString();
                }
            }
        }


        return strDepartmentManager;
    }



    public string GetParentDepartmentManager(string strEmpId, ref SqlTransaction trns, string strLocId)
    {
        string strDepartmentManager = string.Empty;

        strDepartmentManager = "0";
        DataTable dt = daClass.return_DataTable("select emp.emp_id from Set_EmployeeMaster left join Set_DepartmentMaster on   Set_EmployeeMaster.Department_Id =Set_DepartmentMaster.Dep_Id    left join Set_Location_Department on Set_Location_Department.Dep_Id = Set_DepartmentMaster.Parent_Id left join Set_EmployeeMaster as Emp on Set_Location_Department.Emp_Id = Emp.Emp_Id where Set_EmployeeMaster.Emp_Id=" + strEmpId + " and Set_Location_Department.Location_Id=" + strLocId + "", ref trns);

        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["emp_id"] != null)
            {
                if (dt.Rows[0]["emp_id"].ToString() != "0" && dt.Rows[0]["emp_id"].ToString() != "")
                {

                    strDepartmentManager = dt.Rows[0]["emp_id"].ToString();
                }
            }
        }


        return strDepartmentManager;
    }
    public string GetResponsibleDepartmentManager(string strDepartmentId, string strLocId)
    {
        string strDepartmentManager = string.Empty;

        strDepartmentManager = "0";
        //DataTable dt = daClass.return_DataTable("select Set_Location_Department.Emp_Id from Set_Location_Department where Set_Location_Department.Dep_Id=" + strDepartmentId + " and Set_Location_Department.Location_Id=" + strLocId + " ");

        DataTable dt = daClass.return_DataTable("select Set_Location_Department.Emp_Id from Set_Location_Department where Set_Location_Department.Dep_Id=" + strDepartmentId + " and Set_Location_Department.Location_Id=" + strLocId + " ");

        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["emp_id"] != null)
            {
                if (dt.Rows[0]["emp_id"].ToString() != "0" && dt.Rows[0]["emp_id"].ToString() != "")
                {

                    strDepartmentManager = dt.Rows[0]["emp_id"].ToString();
                }
            }
        }
        return strDepartmentManager;
    }
    public DataTable GetLeaveTrans(string strLeaveHeaderId, string strEmpId, string strConString, string strCompId)
    {
        SystemParameter ObjSysParam = new SystemParameter(strConString);
        DataAccessClass ObjDa = new PegasusDataAccess.DataAccessClass(strConString);
        DataTable dtleaveRquestDetail = ObjDa.return_DataTable("select Att_Leave_Request.Trans_id,Att_Leave_Request.Leave_Type_Id,Leave_Name,Emp_Description,From_date,To_Date,Att_Employee_Leave_Trans.Remaining_Days,(select  COUNT(*) from Att_Leave_Request_Child where Ref_Id = Att_Leave_Request.Trans_Id) as LeaveCount, Att_Leave_Request.Field4, Att_Leave_Request.Field5 from Att_Leave_Request inner join Att_LeaveMaster on Att_Leave_Request.Leave_Type_Id = Att_LeaveMaster.Leave_Id  inner join Att_Employee_Leave_Trans on Att_Employee_Leave_Trans.Leave_Type_Id = Att_Leave_Request.Leave_Type_Id where Att_Leave_Request.Field2 = '" + strLeaveHeaderId + "'  And Att_Employee_Leave_Trans.Emp_Id = '" + strEmpId + "' AND Att_Employee_Leave_Trans.IsActive = 'True' And Att_Employee_Leave_Trans.Field3 = 'Open' ");
        return dtleaveRquestDetail;
    }
    public string GetResponsibleDepartmentManager(string strDepartmentId, ref SqlTransaction trns, string strLocId)
    {
        string strDepartmentManager = string.Empty;

        strDepartmentManager = "0";
        DataTable dt = daClass.return_DataTable("select Set_Location_Department.Emp_Id from Set_Location_Department where Set_Location_Department.Dep_Id=" + strDepartmentId + " and Set_Location_Department.Location_Id=" + strLocId + " ", ref trns);

        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["emp_id"] != null)
            {
                if (dt.Rows[0]["emp_id"].ToString() != "0" && dt.Rows[0]["emp_id"].ToString() != "")
                {

                    strDepartmentManager = dt.Rows[0]["emp_id"].ToString();
                }
            }
        }


        return strDepartmentManager;
    }

    //End code

}
