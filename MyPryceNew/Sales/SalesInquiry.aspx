<%@ Page Title="" Language="C#" EnableEventValidation="false" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="SalesInquiry.aspx.cs" Inherits="Sales_SalesInquiry" %>

<%@ Register Src="~/WebUserControl/ReportSystem.ascx" TagName="ReportSystem" TagPrefix="RS" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/Followup.ascx" TagName="AddFollowup" TagPrefix="AT1" %>
<%@ Register Src="~/WebUserControl/FileUpload.ascx" TagPrefix="AT1" TagName="FileUpload1" %>
<%@ Register Src="~/WebUserControl/ContactInfo.ascx" TagName="ViewContact" TagPrefix="AT1" %>
<%@ Register Src="~/WebUserControl/ucControlsSetting.ascx" TagName="ucCtlSetting" TagPrefix="UC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">

    <style>
        select2-container {
            width: 90% !important;
        }

        .select2-container .select-all {
            position: absolute;
            top: 6px;
            right: 4px;
            width: 20px;
            height: 20px;
            margin: auto;
            display: block;
            background: url('data:image/svg+xml;utf8;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iaXNvLTg4NTktMSI/Pgo8IS0tIEdlbmVyYXRvcjogQWRvYmUgSWxsdXN0cmF0b3IgMTYuMC4wLCBTVkcgRXhwb3J0IFBsdWctSW4gLiBTVkcgVmVyc2lvbjogNi4wMCBCdWlsZCAwKSAgLS0+CjwhRE9DVFlQRSBzdmcgUFVCTElDICItLy9XM0MvL0RURCBTVkcgMS4xLy9FTiIgImh0dHA6Ly93d3cudzMub3JnL0dyYXBoaWNzL1NWRy8xLjEvRFREL3N2ZzExLmR0ZCI+CjxzdmcgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB4bWxuczp4bGluaz0iaHR0cDovL3d3dy53My5vcmcvMTk5OS94bGluayIgdmVyc2lvbj0iMS4xIiBpZD0iQ2FwYV8xIiB4PSIwcHgiIHk9IjBweCIgd2lkdGg9IjUxMnB4IiBoZWlnaHQ9IjUxMnB4IiB2aWV3Qm94PSIwIDAgNDc0LjggNDc0LjgwMSIgc3R5bGU9ImVuYWJsZS1iYWNrZ3JvdW5kOm5ldyAwIDAgNDc0LjggNDc0LjgwMTsiIHhtbDpzcGFjZT0icHJlc2VydmUiPgo8Zz4KCTxnPgoJCTxwYXRoIGQ9Ik0zOTYuMjgzLDI1Ny4wOTdjLTEuMTQtMC41NzUtMi4yODItMC44NjItMy40MzMtMC44NjJjLTIuNDc4LDAtNC42NjEsMC45NTEtNi41NjMsMi44NTdsLTE4LjI3NCwxOC4yNzEgICAgYy0xLjcwOCwxLjcxNS0yLjU2NiwzLjgwNi0yLjU2Niw2LjI4M3Y3Mi41MTNjMCwxMi41NjUtNC40NjMsMjMuMzE0LTEzLjQxNSwzMi4yNjRjLTguOTQ1LDguOTQ1LTE5LjcwMSwxMy40MTgtMzIuMjY0LDEzLjQxOCAgICBIODIuMjI2Yy0xMi41NjQsMC0yMy4zMTktNC40NzMtMzIuMjY0LTEzLjQxOGMtOC45NDctOC45NDktMTMuNDE4LTE5LjY5OC0xMy40MTgtMzIuMjY0VjExOC42MjIgICAgYzAtMTIuNTYyLDQuNDcxLTIzLjMxNiwxMy40MTgtMzIuMjY0YzguOTQ1LTguOTQ2LDE5LjctMTMuNDE4LDMyLjI2NC0xMy40MThIMzE5Ljc3YzQuMTg4LDAsOC40NywwLjU3MSwxMi44NDcsMS43MTQgICAgYzEuMTQzLDAuMzc4LDEuOTk5LDAuNTcxLDIuNTYzLDAuNTcxYzIuNDc4LDAsNC42NjgtMC45NDksNi41Ny0yLjg1MmwxMy45OS0xMy45OWMyLjI4Mi0yLjI4MSwzLjE0Mi01LjA0MywyLjU2Ni04LjI3NiAgICBjLTAuNTcxLTMuMDQ2LTIuMjg2LTUuMjM2LTUuMTQxLTYuNTY3Yy0xMC4yNzItNC43NTItMjEuNDEyLTcuMTM5LTMzLjQwMy03LjEzOUg4Mi4yMjZjLTIyLjY1LDAtNDIuMDE4LDguMDQyLTU4LjEwMiwyNC4xMjYgICAgQzguMDQyLDc2LjYxMywwLDk1Ljk3OCwwLDExOC42Mjl2MjM3LjU0M2MwLDIyLjY0Nyw4LjA0Miw0Mi4wMTQsMjQuMTI1LDU4LjA5OGMxNi4wODQsMTYuMDg4LDM1LjQ1MiwyNC4xMyw1OC4xMDIsMjQuMTNoMjM3LjU0MSAgICBjMjIuNjQ3LDAsNDIuMDE3LTguMDQyLDU4LjEwMS0yNC4xM2MxNi4wODUtMTYuMDg0LDI0LjEzNC0zNS40NSwyNC4xMzQtNTguMDk4di05MC43OTcgICAgQzQwMi4wMDEsMjYxLjM4MSw0MDAuMDg4LDI1OC42MjMsMzk2LjI4MywyNTcuMDk3eiIgZmlsbD0iIzAwMDAwMCIvPgoJCTxwYXRoIGQ9Ik00NjcuOTUsOTMuMjE2bC0zMS40MDktMzEuNDA5Yy00LjU2OC00LjU2Ny05Ljk5Ni02Ljg1MS0xNi4yNzktNi44NTFjLTYuMjc1LDAtMTEuNzA3LDIuMjg0LTE2LjI3MSw2Ljg1MSAgICBMMjE5LjI2NSwyNDYuNTMybC03NS4wODQtNzUuMDg5Yy00LjU2OS00LjU3LTkuOTk1LTYuODUxLTE2LjI3NC02Ljg1MWMtNi4yOCwwLTExLjcwNCwyLjI4MS0xNi4yNzQsNi44NTFsLTMxLjQwNSwzMS40MDUgICAgYy00LjU2OCw0LjU2OC02Ljg1NCw5Ljk5NC02Ljg1NCwxNi4yNzdjMCw2LjI4LDIuMjg2LDExLjcwNCw2Ljg1NCwxNi4yNzRsMTIyLjc2NywxMjIuNzY3YzQuNTY5LDQuNTcxLDkuOTk1LDYuODUxLDE2LjI3NCw2Ljg1MSAgICBjNi4yNzksMCwxMS43MDQtMi4yNzksMTYuMjc0LTYuODUxbDIzMi40MDQtMjMyLjQwM2M0LjU2NS00LjU2Nyw2Ljg1NC05Ljk5NCw2Ljg1NC0xNi4yNzRTNDcyLjUxOCw5Ny43ODMsNDY3Ljk1LDkzLjIxNnoiIGZpbGw9IiMwMDAwMDAiLz4KCTwvZz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8L3N2Zz4K') no-repeat center;
            background-size: contain;
            cursor: pointer;
            z-index: 999999;
        }
    </style>
    <script src="../Script/customer.js"></script>
    <script src="../Script/employee.js"></script>
    <script type="text/javascript">

        function LI_Edit_Active1() {
        }

    </script>
    <script>
        function resetPosition1() {

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <span class="fa fa-shopping-cart"></span>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Sales Opportunity%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Sales%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Sales Opportunity%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:Button ID="Btn_Customer_Call" Style="display: none;" runat="server" OnClick="btncustomerinquiry_Click" Text="Customer Call" />
            <asp:Button ID="Btn_Sales_Inquiry_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Sales_Inquiry_Modal" Text="Sales Inquiry Modal" />
            <asp:Button ID="Btn_CustomerInfo_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#modelContactDetail" />
            <asp:HiddenField ID="hdnSInquiryId" runat="server" Value="0" />
            <asp:HiddenField ID="hdnLocationId" runat="server" Value="0" />
            <asp:HiddenField ID="hdnSquotationExist" runat="server" Value="0" />
            <asp:Button ID="Btn_Address_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Followup123" Text="Add Followup" />
            <asp:Button ID="Btn_Modal_FileUpload" Style="display: none;" runat="server" data-toggle="modal" data-target="#Fileupload123" Text="FileUpload" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">
                <ul class="nav nav-tabs pull-right bg-blue-gradient">
                    <li id="Li_Customer_Call"><a href="#Customer_Call" onclick="Li_Tab_Customer_Call()" data-toggle="tab">
                        <i class="fa fa-phone"></i>&nbsp;&nbsp;<asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Customer Call %>"></asp:Label></a></li>
                    <li id="Li_Bin"><a href="#Bin" onclick="Li_Tab_Bin()" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
                    <li id="Li_New"><a href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List" class="active"><a href="#List" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane active" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSinquirysaveandquotation" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnSInquirySave" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="imgBtnRestore" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div1" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label7" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
                                                <asp:Label ID="lblTotalRecords" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="form-group">

                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="col-md-6">
                                                                <asp:DropDownList ID="ddlLocation" runat="server" Class="form-control" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged"
                                                                    AutoPostBack="true">
                                                                </asp:DropDownList>
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="input-group">
                                                                    <asp:DropDownList ID="ddlUser" runat="server" Class="form-control" Visible="true" AutoPostBack="true" OnSelectedIndexChanged="ddlUser_Click">
                                                                    </asp:DropDownList>
                                                                    <span class="input-group-btn">
                                                                        <asp:ImageButton ID="imgBtnSharedSalesPersonData" ImageAlign="Right" ToolTip="Config Sales Person Sharing" runat="server" ImageUrl="~/Images/setting.png" Style="width: 32px; height: 32px" Visible="true" data-toggle="modal" data-target="#SalesPersonModal" />
                                                                    </span>
                                                                </div>
                                                                <br />
                                                            </div>
                                                            <div class="col-lg-2">
                                                                <asp:DropDownList ID="ddlSalesStageList" runat="server" Class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlSalesStageList_SelectedIndexChanged">
                                                                    <asp:ListItem Text="Select"></asp:ListItem>
                                                                    <asp:ListItem Text="New"></asp:ListItem>
                                                                    <asp:ListItem Text="Assigned"></asp:ListItem>
                                                                    <asp:ListItem Text="In Process"></asp:ListItem>
                                                                    <asp:ListItem Text="Converted"></asp:ListItem>
                                                                    <asp:ListItem Text="Recycled"></asp:ListItem>
                                                                    <asp:ListItem Text="Dead"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                <br />
                                                            </div>
                                                            <div class="col-lg-2">
                                                                <asp:DropDownList ID="ddlFieldName" runat="server" Class="form-control"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged">
                                                                    <asp:ListItem Text="Opportunity Name" Value="Opportunity_name"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Opportunity No.%>" Value="SInquiryNo" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Text="Opportunity Date" Value="IDate"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance, Order Close Date %>" Value="OrderCompletionDate"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance, Received Employee %>" Value="ReceivedEmployee"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance, Handled Employee %>" Value="HandledEmployee"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance, Created By %>" Value="CreatedByName"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Customer Name %>" Value="Name"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Access Type%>" Value="Field4"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Opportunity Amt%>" Value="Inq_Amount"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                <br />
                                                            </div>
                                                            <div class="col-lg-2">
                                                                <asp:DropDownList ID="ddlOption" runat="server" Class="form-control">
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                <br />
                                                            </div>
                                                            <div class="col-lg-4">
                                                                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                                    <asp:TextBox ID="txtValue" runat="server" Class="form-control" placeholder="Search from Content"></asp:TextBox>
                                                                    <asp:TextBox ID="txtValueDate" runat="server" Class="form-control" placeholder="Search from Date"
                                                                        Visible="false"></asp:TextBox>
                                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendartxtValueDate" runat="server" TargetControlID="txtValueDate" />
                                                                </asp:Panel>
                                                                <asp:DropDownList ID="ddlPermission" runat="server" Visible="false" Class="form-control">
                                                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,All%>"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Public%>"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Private%>"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                <br />
                                                            </div>
                                                            <div class="col-lg-2">
                                                                <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbindrpt_Click">                                                    
                                                                    <span class="fa fa-search" style="font-size:25px;color:#3b484e"></span>
                                                                </asp:LinkButton>
                                                                &nbsp;&nbsp;
                                                                <asp:LinkButton ID="btnRefresh" runat="server" OnClick="btnRefreshReport_Click">
                                                                     <span class="fa fa-repeat" style="font-size:25px;color:#3b484e"></span>
                                                                </asp:LinkButton>&nbsp;&nbsp;
                                                                <asp:LinkButton ID="btnGvListSetting" ImageAlign="Right" ToolTip="List Settings" runat="server" OnClick="btnGvListSetting_Click" Visible="false"><span class="fa fa-wrench"  style="font-size:25px;"></span></asp:LinkButton>
                                                                <br />
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <asp:HiddenField ID="hdnCanPrint" runat="server" Value="false" />
                                <asp:HiddenField ID="hdnCanEdit" runat="server" Value="false" />
                                <asp:HiddenField ID="hdnCanView" runat="server" Value="false" />
                                <asp:HiddenField ID="hdnCanDelete" runat="server" Value="false" />
                                <asp:HiddenField ID="hdnCanUpload" runat="server" Value="false" />
                                <asp:HiddenField ID="hdnCanFolloup" runat="server" Value="false" />

                                <div class="box box-warning box-solid">
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:HiddenField ID="hdnGvSalesOpportunityCurrentPageIndex" runat="server" Value="1" />
                                                    <asp:HiddenField ID="hdnGvSalesOpportunityCurrentPageIndexBin" runat="server" Value="1" />
                                                    <asp:HiddenField ID="hdnFollowupTableName" runat="server" Value="Inv_SalesInquiryHeader" />



                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvSalesInquiry" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' CurrentSortField="SInquiryID" CurrentSortDirection="DESC"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="false" AllowSorting="True" OnSorting="GvSalesInquiry_Sorting">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" onclick="validateControls(this)" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu" id="">
                                                                            <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnPrint" runat="server" CausesValidation="False" CommandArgument='<%# Eval("SInquiryID") %>' OnCommand="IbtnPrint_Command"><i class="fa fa-print"></i>Print</asp:LinkButton></li>

                                                                            <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <a onclick="getReportDataWithLoc('<%# Eval("SInquiryID") %>','<%# Eval("Location_Id") %>')"><i class="fa fa-print" style="cursor: pointer"></i>Report System</a>
                                                                            </li>

                                                                            <li>
                                                                                <asp:LinkButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("SInquiryID") %>' OnCommand="lnkViewDetail_Command" CausesValidation="False"><i class="fa fa-eye"></i> View</asp:LinkButton></li>
                                                                            <li>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("SInquiryID") %>' OnCommand="btnEdit_Command" CausesValidation="False"><i class="fa fa-pencil"></i> Edit</asp:LinkButton></li>
                                                                            <li>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("SInquiryID") %>' OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>
                                                                            <li>
                                                                                <asp:LinkButton ID="btnFileUpload" runat="server" CommandArgument='<%# Eval("SInquiryID") %>' CommandName='<%#  Eval("SInquiryID")+"/"+Eval("location_id") %>' OnCommand="btnFileUpload_Command" CausesValidation="False"><i class="fa fa-upload"></i>File Upload </asp:LinkButton>
                                                                            </li>
                                                                            <li>
                                                                                <asp:LinkButton ID="btnFolloup" runat="server" CausesValidation="False" OnCommand="btnFolloup_Command" CommandArgument='<%# Eval("SInquiryID") %>'><i class="fa fa-phone"></i>Followup </asp:LinkButton>
                                                                            </li>
                                                                        </ul>
                                                                    </div>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="3%" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Opportunity" SortExpression="Opportunity_name">
                                                                <ItemTemplate>
                                                                    <asp:HiddenField ID="hdnLeadID" runat="server" Value='<%# Eval("SInquiryID") %>' />
                                                                    <asp:Label ID="lblgvOpportunityName" runat="server" Text='<%#Eval("Opportunity_name") %>' />
                                                                    <asp:Label ID="lblgvOpportunityName1" runat="server" Text='<%#Eval("Opportunity_name") %>' Visible="false" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Opportunity No.%>" SortExpression="SInquiryNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvInquiryNo" runat="server" Text='<%#Eval("SInquiryNo") %>' />
                                                                    <asp:Label ID="lblgvInquiryId" runat="server" Text='<%#Eval("SInquiryID") %>' Visible="false" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Date %>" SortExpression="IDate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvInquiryDate" runat="server" Text='<%#GetDate(Eval("IDate").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="8%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order Close Date %>" SortExpression="OrderCompletionDate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvOrdercloseDate" runat="server" Text='<%#GetDate(Eval("OrderCompletionDate").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="12%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Customer" SortExpression="Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCustomerName" runat="server" Text='<%#Eval("Name") %>' />
                                                                    <asp:Label ID="lblQuotationCrerated" runat="server" Text='<%#Eval("isquotationcreated") %>' Visible="false" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Received By" SortExpression="ReceivedEmployee">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvReceivedEmployee" runat="server" Text='<%#Eval("ReceivedEmployee") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Handled By" SortExpression="HandledEmployee">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvHandledEmployee" runat="server" Text='<%#Eval("HandledEmployee") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Status %>" SortExpression="PurchaseStatus">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvStstus" runat="server" Text='<%# Eval("PurchaseStatus") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="7%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Amount" SortExpression="Opportunity_amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvOppoAmount" runat="server" Text='<%# Eval("Opportunity_amount","{0:n}") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="7%" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle />
                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>

                                                    <asp:Repeater ID="rptPager" runat="server">
                                                        <ItemTemplate>
                                                            <ul class="pagination">
                                                                <li class='<%# Convert.ToBoolean(Eval("Enabled")) ? "page-item" : "page-item active" %>'>
                                                                    <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                                                        CssClass="page-link"
                                                                        OnClick="Page_Changed" OnClientClick='<%# !Convert.ToBoolean(Eval("Enabled")) ? "return false;" : "" %>'></asp:LinkButton>
                                                                </li>
                                                            </ul>
                                                        </ItemTemplate>
                                                    </asp:Repeater>


                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="tab-pane" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="GvSalesInquiry" />
                                <asp:AsyncPostBackTrigger ControlID="GvCallRequest" />
                                <asp:AsyncPostBackTrigger ControlID="GvProduct" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">

                                                        <asp:ImageButton ID="btnControlsSetting" ImageAlign="Right" ToolTip="Controls Setting" runat="server" ImageUrl="~/Images/setting.png" OnClick="btnControlsSetting_Click" Style="width: 32px; height: 32px" Visible="false" />
                                                    </div>
                                                    <div class="col-md-6">

                                                        <asp:TextBox ID="txtInquiryNo" TabIndex="1" runat="server" Class="form-control" placeholder="Opportunity No. (*)" />
                                                        <asp:RequiredFieldValidator ID="requiredfieldvalidator2" runat="server" ControlToValidate="txtInquiryNo" ErrorMessage="Please Enter Opportunity No. !" Display="Dynamic" ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">

                                                        <asp:TextBox ID="txtOppoName" TabIndex="2" runat="server" Class="form-control" placeholder="Opportunity Name (*)" />
                                                        <asp:RequiredFieldValidator ID="requiredfieldvalidatorOppoName" runat="server" ControlToValidate="txtOppoName" ErrorMessage="Please Enter Opportunity Name !" Display="Dynamic" ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">

                                                        <asp:TextBox ID="txtInquiryDate" runat="server" Class="form-control" placeholder="Date (*)" />
                                                        <asp:RequiredFieldValidator ID="requiredfieldvalidator3" runat="server" ControlToValidate="txtInquiryDate" ErrorMessage="Please Enter Opportunity Date !" Display="Dynamic" ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender" runat="server" TargetControlID="txtInquiryDate" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">

                                                        <asp:TextBox ID="txtOrderCompletionDate" TabIndex="3" runat="server" Class="form-control" placeholder="Order Close Date (*)" />
                                                        <asp:RequiredFieldValidator ID="requiredfieldvalidator1" runat="server" ControlToValidate="txtOrderCompletionDate" ErrorMessage="Please Enter Order Closing Date !" Display="Dynamic" ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender_txtOrderCompletionDate" runat="server"
                                                            TargetControlID="txtOrderCompletionDate" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:TextBox ID="txtCallNo" runat="server" Class="form-control" Enabled="false" placeholder="Call no." />
                                                        <asp:HiddenField ID="hdnCallId" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">

                                                        <asp:TextBox ID="txtCallDate" runat="server" Class="form-control" ReadOnly="true" placeholder="Call Date" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblInquiryType" runat="server" Text="<%$ Resources:Attendance,Opportunity Type%>" /><a style="color: Red">*</a>
                                                        <asp:DropDownList ID="ddlInquiryType" runat="server" TabIndex="4" Class="form-control" OnSelectedIndexChanged="ddlInquiryType_SelectedIndexChanged" AutoPostBack="true">
                                                            <asp:ListItem Value="--Select--" Text="Select"></asp:ListItem>
                                                            <asp:ListItem Value="Inquiry" Text="Inquiry"></asp:ListItem>
                                                            <asp:ListItem Value="Tender" Text="Tender"></asp:ListItem>
                                                            <asp:ListItem Value="Semi Tender" Text="Semi Tender"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtTenderNo" runat="server" Class="form-control" Visible="false" placeholder="Tender No" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtTenderDate" runat="server" Visible="false" Class="form-control" placeholder="Tender Date" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" TargetControlID="txtTenderDate" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-10">
                                                        <asp:TextBox ID="txtCustomerName" TabIndex="5" runat="server" placeholder="Customer Name (*)" Class="form-control" BackColor="#eeeeee" onchange="txtCustomer_TextChanged(this)" />
                                                        <cc1:AutoCompleteExtender ID="txtCustomerName_AutoCompleteExtender" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="2" ServiceMethod="GetCompletionListCustomer" ServicePath=""
                                                            TargetControlID="txtCustomerName" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-2">
                                                        <button type="button" id="btnnewcustomer" title="Add Customer" onclick="btnNewCustomer(); return false;" name="btnnewCustomer" class="btn btn-primary">Add</button>
                                                        &nbsp;
                                                        <a onclick="customerHistory(<%= txtCustomerName.ClientID %>)" title="Customer History">
                                                            <i class="fa fa-history" style="font-size: 25px; color: #333333"></i></a>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-11">
                                                        <asp:TextBox ID="txtContactList" TabIndex="6" runat="server" Class="form-control" placeholder="Contact Name (*)" BackColor="#eeeeee" onchange="validateContactPerson(this)" />
                                                        <asp:RequiredFieldValidator ID="requiredfieldvalidator4" runat="server" ControlToValidate="txtContactList" ErrorMessage="Please Enter Contact Name !" Display="Dynamic" ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                                        <cc1:AutoCompleteExtender ID="txtContactList_AutoCompleteExtender" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="0" ServiceMethod="GetContactListCustomer" ServicePath=""
                                                            TargetControlID="txtContactList" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>

                                                        <br />
                                                    </div>
                                                    <div class="col-md-1" style="margin-top: 2px">
                                                        <asp:LinkButton ID="btnAddCustomer" OnClick="btnAddCustomer_Click" runat="server" ToolTip="Add New Customer" CausesValidation="False" Visible="false">
                                                            <i class="fa fa-user-plus"  style="font-size: 25px; color:#333333"></i>
                                                        </asp:LinkButton>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblLeadSource" runat="server" Text="<%$ Resources:Attendance, Lead Source %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlLeadSource" runat="server" Class="form-control" AutoPostBack="true">
                                                            <asp:ListItem Text="Call"></asp:ListItem>
                                                            <asp:ListItem Text="Existing Customer"></asp:ListItem>
                                                            <asp:ListItem Text="Self Generated"></asp:ListItem>
                                                            <asp:ListItem Text="Employee"></asp:ListItem>
                                                            <asp:ListItem Text="Mail"></asp:ListItem>
                                                            <asp:ListItem Text="Conference"></asp:ListItem>
                                                            <asp:ListItem Text="Trade Show"></asp:ListItem>
                                                            <asp:ListItem Text="Website"></asp:ListItem>
                                                            <asp:ListItem Text="Compaign"></asp:ListItem>
                                                            <asp:ListItem Text="Others"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <br />
                                                        <asp:TextBox ID="txtCampaignID" runat="server" TabIndex="7" Class="form-control" placeholder="Campaign" BackColor="#eeeeee" onchange="txtCampaignID_TextChanged(this)" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="2" ServiceMethod="GetAllCampaignName" ServicePath=""
                                                            TargetControlID="txtCampaignID" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblLeadStatus" runat="server" Text="<%$ Resources:Attendance,Sales Stage%>"></asp:Label>
                                                        <asp:DropDownList ID="ddlSalesStage" runat="server" Class="form-control" AutoPostBack="true">
                                                            <asp:ListItem Text="New" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="Assigned"></asp:ListItem>
                                                            <asp:ListItem Text="In Process"></asp:ListItem>
                                                            <asp:ListItem Text="Converted"></asp:ListItem>
                                                            <asp:ListItem Text="Recycled"></asp:ListItem>
                                                            <asp:ListItem Text="Dead"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <br />
                                                        <asp:RadioButton ID="rbtnFormView" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Form View%>"
                                                            AutoPostBack="true" GroupName="Product" OnCheckedChanged="rbtnFormView_OnCheckedChanged" />&nbsp;&nbsp;&nbsp;
                                                        
                                                        <asp:RadioButton ID="rbtnAdvancesearchView" Style="margin-left: 15px;" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Advance Search View%>"
                                                            AutoPostBack="true" GroupName="Product" OnCheckedChanged="rbtnFormView_OnCheckedChanged" />

                                                        <asp:RadioButton ID="rbtnLabelBuilder" Style="margin-left: 15px;" Font-Bold="true" runat="server" Text="Label Builder" GroupName="Product" OnCheckedChanged="rbtnFormView_OnCheckedChanged" AutoPostBack="true" onchange="openLabelBuilder(this)" />

                                                        <asp:RadioButton ID="rbtnProductBuilder" Style="margin-left: 15px;" Font-Bold="true" runat="server" Text="Product Builder"
                                                            AutoPostBack="true" GroupName="Product" OnCheckedChanged="rbtnFormView_OnCheckedChanged" />

                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" id="ctlRemark" runat="server">

                                                        <asp:TextBox ID="txtRemark" TabIndex="8" placeholder="Opportunity Detailed Information" Style="width: 100%; min-width: 100%; max-width: 100%; height: 70px; min-height: 70px; max-height: 200px; overflow: auto;" runat="server" Class="form-control" TextMode="MultiLine"
                                                            Font-Names="Arial" />
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12" style="text-align: center">

                                                        <div class="col-md-12" visible="false" runat="server" id="divAdvanceSearch">
                                                            <asp:Button ID="btnAddProductScreen" runat="server" Text="<%$ Resources:Attendance,Add Product List %>" Class="btn btn-primary" OnClick="btnAddProductScreen_Click" />
                                                            <asp:Button ID="btnAddtoList" runat="server" Text="<%$ Resources:Attendance,Fill Your Product %>" Class="btn btn-primary" OnClick="btnAddtoList_Click" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12" runat="server" id="divProductBuilder" visible="false">
                                                            <asp:Button ID="btnAddProductFromBuilder" runat="server" Text="Create Product From Builder" Class="btn btn-primary" OnClientClick="openProductBuilder();return false;" />
                                                            <asp:Button ID="btnAddProductToGrid" runat="server" Text="<%$ Resources:Attendance,Fill Your Product %>" Class="btn btn-primary" OnClick="btnAddProductToGrid_Click" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12" runat="server" id="divLabelBuilder" visible="false">
                                                            <asp:Button ID="btnLBAddProduct" runat="server" Text="<%$ Resources:Attendance,Fill Your Product %>" Class="btn btn-primary" OnClick="btnAddProductToGrid_Click" />
                                                            <br />
                                                        </div>
                                                        <br />
                                                    </div>

                                                    <div id="pnlProduct1" runat="server" class="row">
                                                        <div style="display: none">
                                                            <asp:Button ID="btnFillRelatedProducts" runat="server" OnClick="btnFillRelatedProducts_Click" />
                                                        </div>
                                                        <div class="col-md-12">
                                                            <div id="Div_Add_Product_Info" runat="server" class="box box-info collapsed-box">
                                                                <div class="box-header with-border">
                                                                    <h3 class="box-title">
                                                                        <asp:Label ID="lbladdproduct" runat="server" Text="<%$ Resources:Attendance,Add Product %>"></asp:Label></h3>
                                                                    <div class="box-tools pull-right">
                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                            <i id="Btn_Add_Div" runat="server" class="fa fa-plus"></i>
                                                                        </button>
                                                                    </div>
                                                                </div>
                                                                <div class="box-body">
                                                                    <div class="form-group">
                                                                        <div class="col-md-6">

                                                                            <asp:TextBox ID="txtProductcode" TabIndex="9" runat="server" placeholder="Product Id" onchange="txtProductCode_TextChanged(this)" BackColor="#eeeeee" Class="form-control" />
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="100"
                                                                                DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductCode"
                                                                                ServicePath="" TargetControlID="txtProductcode" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:TextBox ID="txtProductName" TabIndex="10" runat="server" placeholder="Product Name" onchange="txtProductCode_TextChanged(this)" BackColor="#eeeeee" Class="form-control" />
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionListProductName" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtProductName"
                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <asp:HiddenField ID="hdnNewProductId" runat="server" Value="0" />
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblUnit" runat="server" Text="<%$ Resources:Attendance,Unit %>" /><a style="color: Red">*</a>
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator6" ValidationGroup="Add_Product" Display="Dynamic"
                                                                                SetFocusOnError="true" ControlToValidate="ddlUnit" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Unit %>" />
                                                                            <asp:DropDownList ID="ddlUnit" TabIndex="11" runat="server" Class="form-control" />
                                                                            <asp:HiddenField ID="hdnUnitId" runat="server" Value="0" />
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-4">
                                                                            <br />
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator7" ValidationGroup="Add_Product"
                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtRequiredQty" ErrorMessage="<%$ Resources:Attendance,Enter Required Quantity%>"></asp:RequiredFieldValidator>
                                                                            <asp:TextBox ID="txtRequiredQty" TabIndex="12" MaxLength="6" runat="server" Class="form-control" placeholder="Required Quantity (*)" />
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                                TargetControlID="txtRequiredQty" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-2">
                                                                            <br />
                                                                            <asp:LinkButton ID="txtStock" OnClientClick="lnkStockInfo_Command();return false;" MaxLength="6" runat="server" />
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-4">
                                                                            <br />
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator8" ValidationGroup="Add_Product"
                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEstimatedUnitPrice" ErrorMessage="<%$ Resources:Attendance,Enter Estimated Unit Price%>"></asp:RequiredFieldValidator>
                                                                            <asp:TextBox ID="txtEstimatedUnitPrice" onchange="txtUnitPrice_TextChanged();return false;" TabIndex="13" runat="server" Class="form-control" MaxLength="8" placeholder="Estimated Unit Price (*)" />
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                                TargetControlID="txtEstimatedUnitPrice" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-4">
                                                                            <asp:Label ID="lblPCurrency" runat="server" Text="<%$ Resources:Attendance,Currency %>" /><a style="color: Red">*</a>
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator9" ValidationGroup="Add_Product" Display="Dynamic"
                                                                                SetFocusOnError="true" ControlToValidate="ddlPCurrency" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Currency %>" />
                                                                            <asp:DropDownList ID="ddlPCurrency" runat="server" Class="form-control" />
                                                                            <asp:HiddenField ID="hdnCurrencyId" runat="server" Value="0" />
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-4">
                                                                            <br />
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator10" ValidationGroup="Add_Product"
                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtFrequency" ErrorMessage="<%$ Resources:Attendance,Enter Required Frequency (In Days)%>"></asp:RequiredFieldValidator>
                                                                            <asp:TextBox ID="txtFrequency" TabIndex="14" Class="form-control" MaxLength="3" runat="server" placeholder="Required Frequency (In Days) (*)"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                                                TargetControlID="txtFrequency" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-12">
                                                                            <asp:Panel ID="pnlPDescription" runat="server" Class="form-control" BorderColor="#8ca7c1" Style="width: 100%; min-width: 100%; max-width: 100%; height: 70px; min-height: 70px; max-height: 200px; overflow: auto;"
                                                                                BackColor="#ffffff" ScrollBars="Vertical" Visible="false">
                                                                                <asp:Literal ID="txtPDescription" runat="server"></asp:Literal>
                                                                            </asp:Panel>
                                                                            <asp:TextBox ID="txtPDesc" TabIndex="15" placeholder="Product Description" runat="server" Style="width: 100%; min-width: 100%; max-width: 100%; height: 70px; min-height: 70px; max-height: 200px; overflow: auto;" TextMode="MultiLine" Class="form-control"></asp:TextBox>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-12">
                                                                            <div style="overflow: auto; max-height: 500px;">
                                                                                <asp:UpdatePanel UpdateMode="Conditional" ID="updatePanelRelProduct" runat="server">
                                                                                    <Triggers>
                                                                                        <asp:AsyncPostBackTrigger ControlID="btnFillRelatedProducts" EventName="Click" />
                                                                                    </Triggers>
                                                                                    <ContentTemplate>
                                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvRelatedProduct" Visible="false" runat="server" Width="100%"
                                                                                            AutoGenerateColumns="False">
                                                                                            <Columns>
                                                                                                <asp:TemplateField>
                                                                                                    <ItemTemplate>
                                                                                                        <asp:CheckBox ID="chk" runat="server" />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="2%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id%>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvProduct" runat="server" Text='<%#Eval("ProductCode")%>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="8%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Product Name %>">
                                                                                                    <ItemTemplate>
                                                                                                        <table width="100%">
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="lblgvProductId" runat="server" Text='<%#Eval("SubProduct_Id") %>'
                                                                                                                        Visible="false" />
                                                                                                                    <asp:Label ID="lblgvProductName" runat="server" Text='<%#Eval("EProductName")%>' />
                                                                                                                </td>
                                                                                                                <td align='<%= PageControlCommon.ChangeTDForDefaultRight()%>'>
                                                                                                                    <asp:ImageButton ID="lnkDes" runat="server" ImageUrl="~/Images/detail.png" />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:Panel ID="PopupMenu1" Width="100%" runat="server">
                                                                                                            <table border="1" cellpadding="0" cellspacing="0" bordercolor="#c6c6c6">
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                        <table width="314" height="110" cellspacing="0" bgcolor="#F9F9F9">
                                                                                                                            <tr>
                                                                                                                                <td height="21" colspan="2">
                                                                                                                                    <div align="center" style="background: url(../Images/InvGridHdr.jpg) repeat">
                                                                                                                                        <asp:Label ID="lblDetail1" runat="server" Text="<%$ Resources:Attendance,Details %>"></asp:Label>
                                                                                                                                    </div>
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                            <tr style="background-color: whitesmoke">
                                                                                                                                <td colspan="2" align="left" valign="top">
                                                                                                                                    <asp:Panel ID="pnl" runat="server" Width="100%" Height="300px" ScrollBars="Vertical">
                                                                                                                                        <asp:Label ID="lblgvProductDescription" runat="server" Text='<%#Eval("Description") %>' />
                                                                                                                                    </asp:Panel>
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                        </table>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </asp:Panel>
                                                                                                        <cc1:HoverMenuExtender ID="hme3" runat="Server" TargetControlID="lnkDes" PopupControlID="PopupMenu1"
                                                                                                            HoverCssClass="popupHover" PopupPosition="Left" OffsetX="0" OffsetY="0" PopDelay="50" />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="27%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Unit %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:HiddenField ID="hdnUnitId" runat="server" Value='<%#Eval("UnitId") %>' />
                                                                                                        <asp:DropDownList ID="ddlunit" CssClass="textComman" runat="server" Width="80px">
                                                                                                        </asp:DropDownList>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="8%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Required Quantity %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="txtquantity" runat="server" CssClass="textComman" Width="100px"></asp:TextBox>
                                                                                                        <cc1:FilteredTextBoxExtender ID="filter1" runat="server" TargetControlID="txtquantity"
                                                                                                            FilterType="Numbers">
                                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="13%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Currency %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:DropDownList ID="ddlCurrency" CssClass="textComman" Enabled="false" runat="server"
                                                                                                            Width="150px">
                                                                                                        </asp:DropDownList>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="17%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Estimated Unit Price %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="txtEstimatedUnitPrice" runat="server" CssClass="textComman" Width="100px"></asp:TextBox>
                                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxtxtEstimatedUnitPrice" runat="server"
                                                                                                            Enabled="True" TargetControlID="txtEstimatedUnitPrice" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Frequency (In Days)">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="txtFrequency" Width="100px" runat="server" MaxLength="3"> </asp:TextBox>
                                                                                                        <asp:Label ID="lblFrequency" Visible="false" runat="server" />
                                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender57" runat="server" Enabled="True"
                                                                                                            TargetControlID="txtFrequency" FilterType="Numbers">
                                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="25%" />
                                                                                                </asp:TemplateField>
                                                                                            </Columns>
                                                                                        </asp:GridView>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>

                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-12">
                                                                            <asp:Button ID="btnProductSave" runat="server" Text="<%$ Resources:Attendance,Add Product %>"
                                                                                class="btn btn-primary" ValidationGroup="Add_Product" OnClick="btnProductSave_Click" />
                                                                            <a class="btn btn-primary" onclick="btnProductCancel_Click()">Reset</a>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12">
                                                        <br />
                                                        <div style="overflow: auto; max-height: 500px;">
                                                            <asp:UpdatePanel runat="server" ID="upProduct" UpdateMode="Conditional">
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btnProductSave" EventName="Click" />
                                                                    <asp:AsyncPostBackTrigger ControlID="GvSalesInquiry" />
                                                                    <asp:AsyncPostBackTrigger ControlID="btnAddtoList" EventName="Click" />
                                                                    <asp:AsyncPostBackTrigger ControlID="btnAddProductToGrid" EventName="Click" />
                                                                </Triggers>
                                                                <ContentTemplate>
                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvProduct" runat="server" Width="100%" AutoGenerateColumns="False">

                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Edit">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="imgBtnProductEdit" runat="server" CommandArgument='<%# Eval("Serial_No") %>' OnCommand="imgBtnProductEdit_Command" ToolTip="Edit">
                                                                                        <i class="fa fa-edit"></i>
                                                                                    </asp:LinkButton>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Delete">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="imgBtnProductDelete" runat="server" CommandArgument='<%# Eval("Serial_No") %>' OnCommand="imgBtnProductDelete_Command">
                                                                                        <i class="fa fa-trash"></i>
                                                                                    </asp:LinkButton>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,S No. %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblSNo" runat="server" Visible="false" Text='<%#Eval("Serial_No") %>' />
                                                                                    <asp:Label ID="lblSerialNo" runat="server" Text='<%#Container.DataItemIndex+1 %>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id%>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblgvProduct" runat="server" Text='<%#ProductCode(Eval("Product_Id").ToString())%>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Product Name %>">
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lblGrdHdProductName" Text="<%$ Resources:Attendance,Product Name %>" runat="server"></asp:Label>
                                                                                    <asp:CheckBox ID="chkShortProductName1" Text="" runat="server" ToolTip="Dispay detail Name" OnCheckedChanged="chkShortProductName_CheckedChanged" AutoPostBack="true"></asp:CheckBox>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="lblgvProductId" runat="server" Text='<%# Eval("Product_Id") %>' Visible="false" />
                                                                                                <asp:Label ID="lblgvProductName" runat="server" Text='<%# Eval("Product_Id").ToString()=="0"? Eval("SuggestedProductName").ToString(): SuggestedProductName(Eval("Product_Id").ToString()) %>' Visible="false" />
                                                                                                <asp:Label ID="lblShortProductName1" Font-Size="10" runat="server" Text='<%# Eval("Product_Id").ToString()=="0"? Eval("SuggestedProductName").ToString(): getShortProductName(Eval("Product_Id").ToString()) %>' Visible="true"></asp:Label>
                                                                                                <asp:HiddenField ID="hdnSuggestedProductName" runat="server" Value='<%#Eval("SuggestedProductName") %>' />
                                                                                            </td>
                                                                                            <td align='<%= PageControlCommon.ChangeTDForDefaultRight()%>'>
                                                                                                <asp:ImageButton ID="lnkDes" runat="server" ImageUrl="~/Images/detail.png" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <%--  <asp:LinkButton ID="lnkDes" runat="server" Text="<%$ Resources:Attendance,More %>"></asp:LinkButton>--%>
                                                                                    <asp:Panel ID="PopupMenu1" Width="100%" runat="server">
                                                                                        <table border="1" cellpadding="0" cellspacing="0" bordercolor="#c6c6c6">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table width="314" height="110" cellspacing="0" bgcolor="#F9F9F9">
                                                                                                        <tr>
                                                                                                            <td height="21" colspan="2">
                                                                                                                <div align="center" style="background: url(../Images/InvGridHdr.jpg) repeat">
                                                                                                                    <asp:Label ID="lblDetail1" runat="server" Text="<%$ Resources:Attendance,Details %>"></asp:Label>
                                                                                                                </div>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr style="background-color: whitesmoke">
                                                                                                            <td colspan="2" align="left" valign="top">
                                                                                                                <asp:Panel ID="pnl" runat="server" Width="100%" Height="300px" ScrollBars="Vertical">
                                                                                                                    <asp:Label ID="lblgvProductDescription" runat="server" Text='<%#Eval("ProductDescription") %>' />
                                                                                                                </asp:Panel>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </asp:Panel>
                                                                                    <cc1:HoverMenuExtender ID="hme3" runat="Server" TargetControlID="lnkDes" PopupControlID="PopupMenu1"
                                                                                        HoverCssClass="popupHover" PopupPosition="Left" OffsetX="0" OffsetY="0" PopDelay="50" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" Width="25%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Unit %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblgvUnitId" runat="server" Visible="false" Text='<%#Eval("UnitId") %>' />
                                                                                    <asp:Label ID="lblgvUnit" runat="server" Text='<%#GetUnitName(Eval("UnitId").ToString()) %>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Stock%>">
                                                                                <ItemTemplate>
                                                                                    <a onclick="lnkStockInfo_Command1('<%# Eval("Product_Id") %>')" style="cursor: pointer; color: blue">
                                                                                        <%#GetAmountDecimal(Eval("Sysqty").ToString()) %>
                                                                                    </a>
                                                                                    <%--<asp:LinkButton ID="lnkStockInfo" runat="server" Text='<%#GetAmountDecimal(Eval("Sysqty").ToString()) %>'
                                                                                        Font-Underline="true" ToolTip="View Detail" OnCommand="lnkStockInfo_Command" ForeColor="Blue" CommandArgument='<%# Eval("Product_Id") %>'></asp:LinkButton>--%>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Required Quantity %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblgvRequiredQty" runat="server" Text='<%#Eval("Quantity") %>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" Width="13%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Currency %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblgvCurrencyId" runat="server" Visible="false" Text='<%#Eval("Currency_Id") %>' />
                                                                                    <asp:Label ID="lblgvCurrency" runat="server" Text='<%#GetCurrencyName(Eval("Currency_Id").ToString()) %>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Estimated Unit Price %>">
                                                                                <ItemTemplate>
                                                                                    <%--<asp:TextBox ID="txtgvEstimatedUnitPrice" Width="100px" runat="server" Text='<%# GetAmountDecimal(Eval("EstimatedUnitPrice").ToString()) %>' OnTextChanged="txtEstimatedUnitPrice_TextChanged" AutoPostBack="true"> </asp:TextBox>--%>
                                                                                    <asp:TextBox ID="txtgvEstimatedUnitPrice" Width="100px" runat="server" Text='<%# GetAmountDecimal(Eval("EstimatedUnitPrice").ToString()) %>' onchange='<%# "CheckGvEstimatedUnitPrice(this, " + Eval("Product_Id") + "); return false;" %>'></asp:TextBox>


                                                                                    <asp:Label ID="lblgvEstimatedUnitPrice" Visible="false" runat="server" Text='<%# GetAmountDecimal(Eval("EstimatedUnitPrice").ToString()) %>' />
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                                                        TargetControlID="txtgvEstimatedUnitPrice" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" Width="25%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Frequency (In Days)">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtgvFrequency" Width="100px" runat="server" Text='<%# Eval("DaysFrequency").ToString() %>' MaxLength="3"> </asp:TextBox>
                                                                                    <asp:Label ID="lblgvFrequency" Visible="false" runat="server" Text='<%# Eval("DaysFrequency").ToString() %>' />
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender57" runat="server" Enabled="True"
                                                                                        TargetControlID="txtgvFrequency" FilterType="Numbers">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" Width="25%" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                            <asp:HiddenField ID="hdnProductId" runat="server" />
                                                            <asp:HiddenField ID="hdnProductName" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <br />
                                                        <asp:TextBox ID="txtProbability" TabIndex="16" runat="server" Class="form-control" MaxLength="3" Font-Names="Arial" onchange="validateProbability(this)" placeholder="Probability (*)" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True" TargetControlID="txtProbability" ValidChars="1,2,3,4,5,6,7,8,9,0"></cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblCurrency" runat="server" Text="<%$ Resources:Attendance,Currency %>" />
                                                        <asp:DropDownList ID="ddlCurrency" runat="server" Class="form-control" onchange="ddlCurrency_OnSelectedIndexChanged()" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <br />
                                                        <asp:TextBox ID="txtOpportunityAmt" TabIndex="17" runat="server" Class="form-control" Font-Names="Arial" placeholder="Opportunity Amt (*)" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True" TargetControlID="txtOpportunityAmt" ValidChars="1,2,3,4,5,6,7,8,9,0,."></cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="requiredfieldvalidator5" runat="server" ControlToValidate="txtOpportunityAmt" ErrorMessage="Please Enter Opportunity Amount !" Display="Dynamic" ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:TextBox ID="txtReceivedEmp" TabIndex="18" runat="server" Class="form-control" BackColor="#eeeeee" placeholder="Received Employee (*)" onchange="validateEmployee(this)" />
                                                        <cc1:AutoCompleteExtender ID="txtReceivedEmp_AutoCompleteExtender" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="1" ServiceMethod="GetCompletionListEmployeeName" ServicePath=""
                                                            TargetControlID="txtReceivedEmp" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:TextBox ID="txtHandledEmp" TabIndex="19" runat="server" Class="form-control" BackColor="#eeeeee" placeholder="Handled Employee (*)"
                                                            onchange="validateEmployee(this)" />
                                                        <cc1:AutoCompleteExtender ID="txtHandledEmp_AutoCompleteExtender" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="1" ServiceMethod="GetCompletionListEmployeeName" ServicePath=""
                                                            TargetControlID="txtHandledEmp" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblBuyingPriority" TabIndex="20" runat="server" Text="<%$ Resources:Attendance,Buying Priority %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlBuyingPriority" runat="server" Class="form-control">
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Urgent %>" Value="Urgent"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,7 Days %>" Value="7 Days" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,15 Days %>" Value="15 Days"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,30 Days%>" Value="30 Days"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <br />
                                                        <asp:CheckBox ID="ChkSendInPurchase" runat="server" Text="<%$ Resources:Attendance,Send In Purchase%>" CssClass="form-control" />
                                                        <asp:CheckBox ID="chkSendMail" runat="server" Visible="false" CssClass="form-control" Text="<%$ Resources:Attendance,Send Email %>" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Opportunity Access Type%>"></asp:Label>
                                                        <asp:DropDownList ID="ddlAccessType" TabIndex="21" runat="server" Class="form-control">
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Public%>" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Private%>"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblCondition1" runat="server" Text="<%$ Resources:Attendance,Terms & Conditions%>" />
                                                        <cc1:Editor ID="txtCondition1" TabIndex="22" runat="server" class="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Button ID="btnSinquirysaveandquotation" runat="server" Text="<%$ Resources:Attendance,Save & Quotation %>"
                                                            class="btn btn-primary" UseSubmitBehavior="false" OnClientClick="this.disabled='true'; this.value='please wait..';" OnClick="btnSinquirysaveandquotation_Click" Visible="false" CausesValidation="false" ValidationGroup="btnSave" />
                                                        <asp:Button ID="btnSInquirySave" runat="server" Text="<%$ Resources:Attendance,Save %>" UseSubmitBehavior="false" OnClientClick="this.disabled='true'; this.value='please wait..';" class="btn btn-success" OnClick="btnSinquirysaveandquotation_Click" Visible="false" ValidationGroup="btnSave" CausesValidation="false" />
                                                        <asp:Button ID="BtnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                            class="btn btn-primary" CausesValidation="False" OnClick="BtnReset_Click" />
                                                        <asp:Button ID="btnSInquiryCancel" runat="server" class="btn btn-danger" Text="<%$ Resources:Attendance,Cancel %>"
                                                            CausesValidation="False" OnClick="btnSInquiryCancel_Click" />
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="Btn_Bin" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="alert alert-info ">
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="ddlFieldNameBin" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFieldNameBin_SelectedIndexChanged">
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Opportunity No.%>" Value="SInquiryNo" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance, Date %>" Value="IDate"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance, Order Close Date %>" Value="OrderCompletionDate"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance, Received Employee %>" Value="ReceivedEmployee"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance, Handled Employee %>" Value="HandledEmployee"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Customer Name %>" Value="Name"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2">
                                                <asp:DropDownList ID="ddlOptionBin" runat="server" class="form-control">
                                                    <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2">
                                                <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbindBin">
                                                    <asp:TextBox ID="txtValueBin" runat="server" Class="form-control"></asp:TextBox>
                                                    <asp:TextBox ID="txtValueDateBin" runat="server" Class="form-control"
                                                        Visible="false"></asp:TextBox>
                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueDateBin" runat="server" TargetControlID="txtValueDateBin" />
                                                </asp:Panel>
                                            </div>
                                            <div class="col-lg-3" style="margin-top: 6px">
                                                <asp:LinkButton ID="btnbindBin" OnClick="btnbindBin_Click" runat="server" CausesValidation="False" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="btnRefreshBin" OnClick="btnRefreshBin_Click" runat="server" CausesValidation="False" ToolTip="<%$ Resources:Attendance,Refresh %>"> <span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="imgBtnRestore" OnClick="imgBtnRestore_Click" CausesValidation="False" runat="server" ToolTip="<%$ Resources:Attendance, Active %>"><i class="fa fa-lightbulb-o"  style="font-size:25px;"></i></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="ImgbtnSelectAll" Visible="false" runat="server" OnClick="ImgbtnSelectAll_Click" ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true"><span class="fa fa-th"  style="font-size:25px;"></span></asp:LinkButton>
                                            </div>
                                            <div class="col-lg-2">
                                                <h5>
                                                    <asp:Label ID="lblTotalRecordsBin" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div class="box box-warning box-solid">
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvSalesInquiryBin" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvSalesInquiryBin_PageIndexChanging"
                                                        OnSorting="GvSalesInquiryBin_OnSorting" AllowSorting="true">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkCurrent" runat="server" OnCheckedChanged="chkCurrent_CheckedChanged"
                                                                        AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelect_CheckedChanged" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="2%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Opp. No" SortExpression="SInquiryNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvInquiryNo" runat="server" Text='<%#Eval("SInquiryNo") %>' />
                                                                    <asp:Label ID="lblgvInquiryId" runat="server" Text='<%#Eval("SInquiryID") %>' Visible="false" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Date %>" SortExpression="IDate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvInquiryDate" runat="server" Text='<%#GetDate(Eval("IDate").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order Close Date %>" SortExpression="OrderCompletionDate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvOrdercloseDate" runat="server" Text='<%#GetDate(Eval("OrderCompletionDate").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Customer %>" SortExpression="Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCustomerName" runat="server" Text='<%#Eval("Name") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="30%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Received By" SortExpression="ReceivedEmployee">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvReceivedEmployee" runat="server" Text='<%#Eval("ReceivedEmployee") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Handled By" SortExpression="HandledEmployee">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvHandledEmployee" runat="server" Text='<%#Eval("HandledEmployee") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <PagerStyle CssClass="pagination-ys" />
                                                    </asp:GridView>
                                                    <asp:HiddenField ID="HDFSortbin" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="Customer_Call">
                        <asp:UpdatePanel ID="Update_Customer_Call" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="Btn_Customer_Call" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="alert alert-info ">
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="ddlFieldNameRequest" runat="server" CssClass="form-control" AutoPostBack="true">
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Call No. %>" Value="Call_No" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Customer Name %>" Value="Customer_Name"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Contact Name %>" Value="Contact_Person"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Contact No %>" Value="Contact_No"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Received By%>" Value="EmployeeName"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Priority%>" Value="Priority"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Call Type%>" Value="Call_Type"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2">
                                                <asp:DropDownList ID="ddlOptionRequest" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:Panel ID="Panel3" runat="server" DefaultButton="btnbindRequest">
                                                    <asp:TextBox ID="txtValueRequest" runat="server" CssClass="form-control"></asp:TextBox>
                                                </asp:Panel>
                                            </div>
                                            <div class="col-lg-2">
                                                <asp:LinkButton ID="btnbindRequest" runat="server" CausesValidation="False" OnClick="btnbindRequest_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;color:#3b484e"></span> </asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="btnRefreshRequest" runat="server" CausesValidation="False" OnClick="btnRefreshRequest_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;color:#3b484e"></span></asp:LinkButton>
                                            </div>
                                            <div class="col-lg-2">
                                                <h5>
                                                    <asp:Label ID="lblTotalRecordsRequest" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>
                                                <asp:Label ID="lblSelectedRecordRequest" runat="server" Visible="false"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid">
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvCallRequest" runat="server" AllowPaging="True" AllowSorting="True"
                                                        TabIndex="75" AutoGenerateColumns="False" OnPageIndexChanging="GvCallRequest_PageIndexChanging"
                                                        OnSorting="GvCallRequest_Sorting" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                        Width="100%">

                                                        <Columns>
                                                            <asp:TemplateField Visible="false">
                                                                <ItemTemplate>


                                                                    <asp:LinkButton ID="btnEdit" runat="server" BackColor="Transparent" BorderStyle="None" TabIndex="76" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' CssClass="btn fa fa-angle-left" OnCommand="btnPREdit_Command"></asp:LinkButton>


                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Call Id %>" SortExpression="Trans_Id"
                                                                Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvInquiryId" runat="server" Text='<%#Eval("Trans_Id") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Call No.%>" SortExpression="Call_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvInquiryNo" runat="server" Text='<%#Eval("Call_No") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Call Date %>" SortExpression="Call_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvInquiryDate" runat="server" Text='<%#GetDate(Eval("Call_Date").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Customer%>" SortExpression="CustomerName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCustomerName" runat="server" Text='<%#Eval("CustomerName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Contact Name %>" SortExpression="ContactName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvContactName" runat="server" Text='<%#Eval("ContactName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Contact No %>" SortExpression="Contact_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvContactNo" runat="server" Text='<%#Eval("Contact_No") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Call Type %>" SortExpression="Call_Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCallType" runat="server" Text='<%#Eval("Call_Type") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Received By %>" SortExpression="EmployeeName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvEmployeeName" runat="server" Text='<%#Eval("EmployeeName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Priority %>" SortExpression="Priority">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvPriority" runat="server" Text='<%#Eval("Priority") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerStyle CssClass="pagination-ys" />
                                                    </asp:GridView>
                                                    <asp:HiddenField ID="hdnCalllogsId" runat="server" Value="0" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>



    <asp:UpdatePanel ID="Update_Report" runat="server">
        <ContentTemplate>
            <div class="modal fade" id="ReportSystem" tabindex="-1" role="dialog" aria-labelledby="ReportSystem_Web_Control"
                aria-hidden="true">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">
                                <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                            <h4 class="modal-title" id="ReportSystem_Web_Control">Report System
                            </h4>
                        </div>
                        <div class="modal-body">
                            <RS:ReportSystem runat="server" ID="reportSystem" />
                        </div>
                        <div class="modal-footer">
                        </div>

                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_Report">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <div class="modal fade" id="ControlSettingModal" tabindex="-1" role="dialog" aria-labelledby="ControlSetting_ModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title">
                        <asp:Label ID="lblUcSettingsTitle" runat="server" Text="Set Columns Visibility" />
                    </h4>
                </div>
                <div class="modal-body">
                    <UC:ucCtlSetting ID="ucCtlSetting" runat="server" />
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="SalesPersonModal" tabindex="-1" role="dialog" aria-labelledby="ControlSetting_ModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title">
                        <asp:Label ID="Label3" runat="server" Text="Share Sales Person Records" />
                    </h4>
                </div>
                <div class="modal-body">

                    <div class="form-group">
                        <asp:Label ID="lblSalesPerson" runat="server" Text="Sales Person"></asp:Label>
                        <asp:DropDownList ID="ddlPerSPersonName" ClientIDMode="Static" CssClass="form-control" runat="server" onchange="onSalesPersonChange();"></asp:DropDownList>
                    </div>

                    <div class="form-group">
                        <asp:Label ID="Label8" runat="server" Text="Sales Person "></asp:Label>
                        <asp:DropDownList ID="ddlSharedSalesPerson" ClientIDMode="Static" Width="100%" runat="server" multiple="multiple" CssClass="form-control select2">
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="modal-footer">
                    <input type="button" value="Update" class="btn btn-default btn-success" onclick="btnUpdateSalesSharing()" />
                </div>

            </div>
        </div>
    </div>

    <div class="modal fade" id="Fileupload123" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-body">
                    <AT1:FileUpload1 runat="server" ID="FUpload1" />
                </div>
                <div class="modal-footer">
                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="Followup123" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-body">
                    <AT1:AddFollowup ID="FollowupUC" runat="server" />
                </div>
                <div class="modal-footer">
                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="modelContactDetail" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">

                <div class="modal-body">
                    <AT1:ViewContact ID="UcContactList" runat="server" />
                </div>


                <div class="modal-footer">
                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>

    <asp:UpdateProgress ID="UpdateProgress11" runat="server" AssociatedUpdatePanelID="Update_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_Li">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_List">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress7" runat="server" AssociatedUpdatePanelID="Update_New">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress8" runat="server" AssociatedUpdatePanelID="Update_Bin">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_Customer_Call">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
    <script src="../Script/ReportSystem.js"></script>
    <script src="../Script/master.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            $('.select2').select2({
                placeholder: 'Press CTRL+A for selecr or unselect all options'
            });

            $('.select2[multiple]').siblings('.select2-container').append('<span class="select-all"></span>');
        });

        $(document).on('click', '.select-all', function (e) {
            selectAllSelect2($(this).siblings('.selection').find('.select2-search__field'));
        });

        $(document).on("keyup", ".select2-search__field", function (e) {
            var eventObj = window.event ? event : e;
            if (eventObj.keyCode === 65 && eventObj.ctrlKey)
                selectAllSelect2($(this));
        });


        function selectAllSelect2(that) {

            var selectAll = true;
            var existUnselected = false;
            var id = that.parents("span[class*='select2-container']").siblings('select[multiple]').attr('id');
            var item = $("#" + id);

            item.find("option").each(function (k, v) {
                if (!$(v).prop('selected')) {
                    existUnselected = true;
                    return false;
                }
            });

            selectAll = existUnselected ? selectAll : !selectAll;

            item.find("option").prop('selected', selectAll).trigger('change');
        }

        function getReportDataWithLoc(transId, locId) {
            $("#prgBar").css("display", "block");
            document.getElementById('<%= reportSystem.FindControl("hdnTransId").ClientID %>').value = transId;
            document.getElementById('<%= reportSystem.FindControl("hdnLocId").ClientID %>').value = locId;
            setReportData();
        }
        function btnNewCustomer() {
            // window.open('../EMS/ContactMaster.aspx', 'window', 'width=1024, ');
            window.open('../Sales/AddContact.aspx?Page=SINV', 'window', 'width=1024');
        }

        function resetPosition(object, args) {
            $(object._completionListElement.children).each(function () {
                var data = $(this)[0];
                if (data != null) {
                    data.style.paddingLeft = "10px";
                    data.style.cursor = "pointer";
                    data.style.borderBottom = "solid 1px #e7e7e7";
                }
            });
            object._completionListElement.className = "scrollbar scrollbar-primary force-overflow";
            var tb = object._element;
            var tbposition = findPositionWithScrolling(tb);
            var xposition = tbposition[0] + 2;
            var yposition = tbposition[1] + 25;
            var ex = object._completionListElement;
            if (ex)
                $common.setLocation(ex, new Sys.UI.Point(xposition, yposition));
        }
        function findPositionWithScrolling(oElement) {
            if (typeof (oElement.offsetParent) != 'undefined') {
                var originalElement = oElement;
                for (var posX = 0, posY = 0; oElement; oElement = oElement.offsetParent) {
                    posX += oElement.offsetLeft;
                    posY += oElement.offsetTop;
                    if (oElement != originalElement && oElement != document.body && oElement != document.documentElement) {
                        posX -= oElement.scrollLeft;
                        posY -= oElement.scrollTop;
                    }
                }
                return [posX, posY];
            } else {
                return [oElement.x, oElement.y];
            }
        }
        function showCalendar(sender, args) {
            var ctlName = sender._textbox._element.name;
            ctlName = ctlName.replace('$', '_');
            ctlName = ctlName.replace('$', '_');
            var processingControl = $get(ctlName);
            //var targetCtlHeight = processingControl.clientHeight;
            sender._popupDiv.parentElement.style.top = processingControl.offsetTop + processingControl.clientHeight + 'px';
            sender._popupDiv.parentElement.style.left = processingControl.offsetLeft + 'px';
            var positionTop = processingControl.clientHeight + processingControl.offsetTop;
            var positionLeft = processingControl.offsetLeft;
            var processingParent;
            var continueLoop = false;
            do {
                // If the control has parents continue loop.
                if (processingControl.offsetParent != null) {
                    processingParent = processingControl.offsetParent;
                    positionTop += processingParent.offsetTop;
                    positionLeft += processingParent.offsetLeft;
                    processingControl = processingParent;
                    continueLoop = true;
                }
                else {
                    continueLoop = false;
                }
            } while (continueLoop);
            sender._popupDiv.parentElement.style.top = positionTop + 2 + 'px';
            sender._popupDiv.parentElement.style.left = positionLeft + 'px';
            sender._popupBehavior._element.style.zIndex = 10005;
        }
        function LI_Edit_Active() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");
            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }
        function LI_Customer_Active() {
            $("#Li_Customer_Call").removeClass("active");
            $("#Customer_Call").removeClass("active");
            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }
        function LI_List_Active() {
            $("#Li_List").addClass("active");
            $("#List").addClass("active");
            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");
        }
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }
        function Li_Tab_Customer_Call() {
            document.getElementById('<%= Btn_Customer_Call.ClientID %>').click();
        }
        function Sales_Inquiry_Modal_Popup() {
            document.getElementById('<%= Btn_Sales_Inquiry_Modal.ClientID %>').click();
        }
        function Sales_Inquiry_Modal_Popup() {
            document.getElementById('<%= Btn_Sales_Inquiry_Modal.ClientID %>').click();
        }
        function Modal_Address_Open() {
            document.getElementById('<%= Btn_Address_Modal.ClientID %>').click();
        }
        function LI_Edit_Active1() {
            $("#Li_FollowupList1").removeClass("active");
            $("#FollowupList1").removeClass("active");
            $("#Li_New1").addClass("active");
            $("#New1").addClass("active");
        }
        function LI_List_Active1() {
            $("#Li_FollowupList").addClass("active");
            $("#FollowupList1").addClass("active");
            $("#Li_New1").removeClass("active");
            $("#New1").removeClass("active");
        }
        function Modal_Open_FileUpload() {
            document.getElementById('<%= Btn_Modal_FileUpload.ClientID %>').click();
        }



        function btnVisible(id) {
            document.getElementById('btncall').style.display = 'none';
            document.getElementById('btnvisit').style.display = 'none';

            if (id.value == "Call") {
                document.getElementById('btncall').style.display = 'block';
            }
            if (id.value == "Visit") {
                document.getElementById('btnvisit').style.display = 'block';
            }
        }

        function quoteRedirect(id) {
            window.open("../Sales/SalesQuotation.aspx?ReminderID=" + id.innerHTML);
        }


        function txtCustomer_TextChanged(ctl) {
            debugger;
            try {
                if (ctl.value == "") {
                    resetCustomerIdForContact();
                    return;
                }
                validateCustomer(ctl);
                document.getElementById('<%= txtContactList.ClientID %>').value = "";
            }
            catch (ex) {
                alert(ex);
            }
        }
        function errorMessages(msg) {
            alert(errorMessages);
        }

        function txtCampaignID_TextChanged(ctl) {
            try {
                if (ctl.value == "") {
                    return;
                }
                var start_pos = ctl.value.lastIndexOf("/") + 1;
                var last_pos = ctl.value.length;
                var id = ctl.value.substring(start_pos, last_pos);
                var Last_pos_name = ctl.value.lastIndexOf("/");
                var name = ctl.value.substring(0, Last_pos_name - 0);

                if (!$.isNumeric(id)) {
                    throw "Not a valid Campaign";
                }
                PageMethods.txtCampaignID_TextChanged(name, id, function (data) {
                    if (data == false) {
                        alert("Not a valid Campaign");
                        ctl.value = "";
                        ctl.focus();
                    }
                }, errorMessages);
            }
            catch (er) {
                alert(er);
                ctl.value = "";
                ctl.focus();
            }
        }

        function txtUnitPrice_TextChanged() {
            $("#prgBar").css("display", "block");
            const unitPrice = $('#<%=txtEstimatedUnitPrice.ClientID%>').val();
            var Customer = $('#<%=txtCustomerName.ClientID%>').val();
            if (Customer == "") {
                Customer = "0";
            }
            var productId = $('#<%=hdnNewProductId.ClientID%>').val();
            $.ajax({
                url: 'SalesInquiry.aspx/GetProductPrice',
                method: 'post',
                contentType: "application/json;",
                data: "{ProductId:" + productId + ",CustomerID:\"" + Customer + "\"}",
                dataType: 'json',
                success: function (data) {
                    var response = data.d;
                    var salesPrice = parseFloat(response);

                    if (unitPrice < salesPrice) {
                        alert("Sales Price not less than " + salesPrice);
                        $("#prgBar").css("display", "none");
                        $('#<%=txtEstimatedUnitPrice.ClientID%>').val(salesPrice);
                        // Move the code that depends on salesPrice here                        
                        return;
                    }
                    $("#prgBar").css("display", "none");
                },
                error: function (request, status, error) {
                    alert(request.responseText);
                    $("#prgBar").css("display", "none");
                }
            });

        }
        function txtProductCode_TextChanged(ctrl) {
            debugger;
            $.ajax({
                url: 'SalesInquiry.aspx/txtProduct_TextChanged',
                method: 'post',
                async: false,
                contentType: "application/json; charset=utf-8",
                data: "{'product':'" + ctrl.value + "'}",
                success: function (data) {
                    if (data.d != null) {
                        var dd = JSON.parse(data.d);
                        document.getElementById('<%=txtProductName.ClientID%>').value = dd[0];
                        document.getElementById('<%=txtProductcode.ClientID%>').value = dd[1];
                        $('#<%=ddlUnit.ClientID%>').val(dd[2]);
                        var replacedData1 = dd[3].replaceAll('<', '');
                        var replacedData2 = replacedData1.replaceAll('>', '');
                        document.getElementById('<%=txtPDesc.ClientID%>').value = replacedData2;
                        document.getElementById('<%=hdnNewProductId.ClientID%>').value = dd[4];
                        document.getElementById('<%=txtStock.ClientID%>').text = dd[6];

                        if (parseInt(dd[5]) != 0) {
                            document.getElementById('<%=btnFillRelatedProducts.ClientID%>').click();
                            $('#<%=GvRelatedProduct.ClientID%>').show();
                        }
                        setTimeout(function () { jQuery('#<%=txtRequiredQty.ClientID%>').focus() }, 500);

                    }
                }
            });
            $('#<%=txtRequiredQty.ClientID%>').focus();
        }

        function CheckGvEstimatedUnitPrice(id, ProductId) {
            $("#prgBar").css("display", "block");
            const unitPrice = id.value;
            var Customer = $('#<%=txtCustomerName.ClientID%>').val();
            if (Customer == "") {
                Customer = "0";
            }
            var productId = ProductId;
            $.ajax({
                url: 'SalesInquiry.aspx/GetProductPrice',
                method: 'post',
                contentType: "application/json;",
                data: "{ProductId:" + productId + ",CustomerID:\"" + Customer + "\"}",
                dataType: 'json',
                success: function (data) {
                    var response = data.d;
                    var salesPrice = parseFloat(response);

                    if (unitPrice < salesPrice) {
                        alert("Sales Price not less than " + salesPrice);
                        $("#prgBar").css("display", "none");

                        id.value = salesPrice;
                        txtEstimatedUnitPrice_TextChanged();
                        // Move the code that depends on salesPrice here                        
                        return;
                    }
                    else {
                        txtEstimatedUnitPrice_TextChanged();
                    }
                    $("#prgBar").css("display", "none");
                },
                error: function (request, status, error) {
                    alert(request.responseText);
                    $("#prgBar").css("display", "none");
                }
            });


        }





        function txtEstimatedUnitPrice_TextChanged() {
            var txtOpportunityAmt_ = document.getElementById('<%= txtOpportunityAmt.ClientID %>');
            var count = 0;
            var total = 0.00;
            var tblProduct = $('#<%=GvProduct.ClientID%> tbody tr').each(function () {
                if (count % 2 != 0) {
                    var row = $(this);
                    debugger;
                    if (row[0].cells[9].childNodes[1] != undefined) {
                        total = total + (parseFloat(row[0].cells[7].innerText) * parseFloat(row[0].cells[9].childNodes[1].value));
                    }
                }
                count++;
            });
            txtOpportunityAmt_.value = total.toString();
        }

        function lnkStockInfo_Command1(productId) {
             //var tblProduct = $('#<%=GvProduct.ClientID%> tbody tr')[0].row[0].cells[9];
             var CustomerName_ = document.getElementById('<%=txtCustomerName.ClientID%>');
            window.open('../Inventory/Magic_Stock_Analysis.aspx?ProductId=' + productId + '&&Type=SALES&&Contact=' + CustomerName_.value.split('/')[0]);
        }

        function lnkStockInfo_Command() {
            var hdnNewProductId = document.getElementById('<%=hdnNewProductId.ClientID%>');
            var CustomerName_ = document.getElementById('<%=txtCustomerName.ClientID%>');
            window.open('../Inventory/Magic_Stock_Analysis.aspx?ProductId=' + hdnNewProductId.value + '&&Type=SALES&&Contact=' + CustomerName_.value.split('/')[0]);
        }

        function validateProbability(ctrl) {
            if (ctrl.value != "") {
                if (isNaN(ctrl.value)) {
                    alert("Please Enter a valid number");
                    ctrl.value = "";
                    ctrl.focus();
                    return;
                }
                if (parseFloat(ctrl.value) > 100) {
                    alert("Probability must be less then 100");
                    ctrl.value = "";
                    ctrl.focus();
                    return;
                }
            }
        }

        function btnProductCancel_Click() {
            var txtFrequency_ = document.getElementById('<%=txtFrequency.ClientID%>');
            var txtProductName_ = document.getElementById('<%=txtProductName.ClientID%>');
            var ddlUnit_ = document.getElementById('<%=ddlUnit.ClientID%>');
            var txtRequiredQty_ = document.getElementById('<%=txtRequiredQty.ClientID%>');
            var txtEstimatedUnitPrice_ = document.getElementById('<%=txtEstimatedUnitPrice.ClientID%>');
            var hdnProductId_ = document.getElementById('<%=hdnProductId.ClientID%>');
            var hdnProductName_ = document.getElementById('<%=hdnProductName.ClientID%>');
            var hdnNewProductId_ = document.getElementById('<%=hdnNewProductId.ClientID%>');
            var txtPDesc_ = document.getElementById('<%=txtPDesc.ClientID%>');
            var txtProductcode_ = document.getElementById('<%=txtProductcode.ClientID%>');

            txtFrequency_.value = "";
            txtProductName_.value = "";
            txtRequiredQty_.value = "1";
            ddlCurrency_OnSelectedIndexChanged()
            ddlUnit_.selectedIndex = 0;
            txtEstimatedUnitPrice_.value = "";
            hdnProductId_.value = "";
            hdnProductName_.value = "";
            hdnNewProductId_.value = "";
            txtPDesc_.defaultValue = "";
            txtProductcode_.value = "";
            txtProductcode_.focus();
            $('#<%=GvRelatedProduct.ClientID%>').hide();
        }

        function ddlCurrency_OnSelectedIndexChanged() {
            var ddlPCurrency_ = document.getElementById('<%=ddlPCurrency.ClientID%>');
            var ddlCurrency_ = document.getElementById('<%=ddlCurrency.ClientID%>');
            ddlPCurrency_.selectedIndex = ddlCurrency_.selectedIndex;
        }
        function openProductBuilder() {
            window.open('../Inventory/ProductBuilder.aspx');
        }
        function Modal_CustomerInfo_Open() {
            document.getElementById('<%= Btn_CustomerInfo_Modal.ClientID %>').click();
        }
        function openLabelBuilder(ctrl) {
            if (ctrl.firstChild.checked) {
                window.open('../Inventory/Labelbuilder.aspx?SOP=1');
            }
        }
        function validateControls(ctrl) {

            if (document.getElementById('<%= hdnCanPrint.ClientID %>').value != "true") {
                ctrl.children[1].children[0].hidden = true;
            }

            if (document.getElementById('<%= hdnCanView.ClientID %>').value != "true") {
                ctrl.children[1].children[1].hidden = true;
            }


            if (document.getElementById('<%= hdnCanEdit.ClientID %>').value != "true") {
                ctrl.children[1].children[2].hidden = true;
            }

            if (document.getElementById('<%= hdnCanDelete.ClientID %>').value != "true") {
                ctrl.children[1].children[3].hidden = true;
            }


            if (document.getElementById('<%= hdnCanUpload.ClientID %>').value != "true") {
                ctrl.children[1].children[4].hidden = true;
            }

            if (document.getElementById('<%= hdnCanFolloup.ClientID %>').value != "true") {
                ctrl.children[1].children[5].hidden = true;
            }

        }
        function showUcControlsSettings() {
            $('#ControlSettingModal').modal('show');
        }

        function onSalesPersonChange() {
            try {
                var empId = $('#ddlPerSPersonName').val();
                if (empId == '--Select--' || empId == "0") {
                    $('#ddlSharedSalesPerson').val('').change();
                    return;
                }
                $.ajax({
                    url: 'SalesInquiry.aspx/getSharedSalesPerson',
                    method: 'post',
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    data: "{'salesPersonId':'" + empId + "'}",
                    success: function (data) {
                        alert(data.d);
                        if (data.d != null && data.d != "@NOTFOUND@") {
                            var dd = data.d.split(',');
                            $('#ddlSharedSalesPerson').val(dd).change();

                        }
                        else {
                            alert("There is no shared sales person data");
                        }
                    }
                });
            }
            catch (err) {

            }
        }

        function btnUpdateSalesSharing() {
            try {
                debugger;
                var empId = $('#ddlPerSPersonName').val();
                var sharedEmpIds = $('#ddlSharedSalesPerson').val();
                if (empId == '--Select--' || empId == "0" || sharedEmpIds == '' || sharedEmpIds == undefined) {
                    $('#ddlSharedSalesPerson').val('').change();
                    return;
                }
                $.ajax({
                    url: 'SalesInquiry.aspx/setSharedSalesPerson',
                    method: 'post',
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    data: "{'salesPersonId':'" + empId + "','salesPersonList':'" + sharedEmpIds + "'}",
                    success: function (data) {
                        if (data.d != 'true') {
                            alert("Record successfully updated");
                        }
                        else {
                            alert("Record not updated");
                        }
                    }
                });
            }
            catch (err) {

            }
        }
        function SetFocus() {
            debugger;
            setTimeout(function () { jQuery('#<%=txtProductcode.ClientID%>').focus() }, 500);
            return;
        }
    </script>
    <script src="../Script/common.js"></script>
</asp:Content>
