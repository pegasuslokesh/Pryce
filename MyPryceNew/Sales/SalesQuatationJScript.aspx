<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPMaster.master" EnableEventValidation="false" CodeFile="SalesQuatationJScript.aspx.cs" Inherits="Sales_SalesQuatationJScript" %>
<%@ Register Src="~/WebUserControl/ReportSystem.ascx" TagName="ReportSystem" TagPrefix="RS" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/Followup.ascx" TagName="AddFollowup" TagPrefix="AT1" %>
<%@ Register Src="~/WebUserControl/AddressControl.ascx" TagName="AddAddress" TagPrefix="AA1" %>
<%@ Register Src="~/WebUserControl/ContactInfo.ascx" TagName="ViewContact" TagPrefix="AT1" %>
<%@ Register Src="~/WebUserControl/FileUpload.ascx" TagPrefix="AT1" TagName="FileUpload1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <script src="../Script/common.js"></script>
   <!-- <link href="ht tps://cdn.jsdelivr.net/npm/summernote@0.8.18/dist/summernote.min.css" rel="stylesheet">-->
    <style type="text/css">
        .page_enabled, .page_disabled {
            display: inline-block;
            height: 25px;
            min-width: 25px;
            line-height: 25px;
            text-align: center;
            text-decoration: none;
            border: 1px solid #ccc;
        }

        .page_enabled {
            background-color: #eee;
            color: #000;
        }

        .page_disabled {
            background-color: #6C6C6C;
            color: #fff !important;
        }
         #tblProduct th {
        text-align: center;
    }
    </style>


    <script type="text/javascript">
        function alertMe() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }

        function LI_Edit_Active1() {
        }

        function resetPosition1() {

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <asp:HiddenField ID="hdnDecimalCount" runat="server" Value="0" />
    <asp:HiddenField ID="hfScrollPosition" runat="server" Value="0" />
    <asp:HiddenField ID="hdfCurrentRow" runat="server" />

    <h1>
        <i class="fas fa-money-check-alt"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Sales Quotation%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Sales%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Sales Quotation%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_GST_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Modal_GST" Text="GST" />
            <asp:Button ID="Btn_NewAddress" Style="display: none;" runat="server" data-toggle="modal" data-target="#NewAddress" Text="New Address" />
            <asp:Button ID="Btn_CustomerInfo_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#modelContactDetail" />
            <asp:Button ID="Btn_Modal_FileUpload" Style="display: none;" runat="server" data-toggle="modal" data-target="#Fileupload123" Text="FileUpload" />
            <asp:HiddenField runat="server" ID="hdnCanUpload" />

            <asp:HiddenField ID="Hdn_Get_Inquity" runat="server" />
            <asp:HiddenField ID="hdnLocationId" runat="server" />
            <asp:HiddenField runat="server" ID="hdnCanPrint" />
            <asp:HiddenField runat="server" ID="hdnCanView" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">
                <ul class="nav nav-tabs pull-right bg-blue-gradient">

                    <li id="Li_Bin"><a href="#Bin" onclick="li_tab_bin()" data-toggle="tab">
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
                            <ContentTemplate>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div1" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				<asp:Label ID="lblTotalRecords" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                <div class="col-md-6">
                                                    <asp:DropDownList ID="ddlLocation" runat="server" Class="form-control" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:DropDownList ID="ddlUser" runat="server" class="form-control" Visible="true" AutoPostBack="true" OnSelectedIndexChanged="ddlUser_Click">
                                                    </asp:DropDownList>
                                                    <br />
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlPosted" runat="server" class="form-control"
                                                        AutoPostBack="true" onchange="ddlPosted_SelectedIndexChanged();return false;">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, All%>" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Open%>" Value="Open" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Close%>" Value="Close"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Lost%>" Value="Lost"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" class="form-control"
                                                        OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Quotation Id %>" Value="SQuotation_Id"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Quotation No. %>" Value="SQuotation_No" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Quotation Date %>" Value="Quotation_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Opportunity No. %>" Value="InquiryNo"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Order Close Date %>" Value="OrderCompletionDate"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Employee Name %>" Value="EmployeeName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Customer Name %>" Value="Customer_Name"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlOption" runat="server" class="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-4">
                                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                        <asp:TextBox ID="txtValue" runat="server" class="form-control" placeholder="Search From Content"></asp:TextBox>
                                                        <asp:TextBox ID="txtValueDate" runat="server" class="form-control" Visible="false" placeholder="Search From Date"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueDate" runat="server" TargetControlID="txtValueDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClientClick="btnbindrpt_Click(); return false;" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClientClick="btnRefreshReport_Click(); return false;" ToolTip="<%$ Resources:Attendance,Refresh %>">                                                <span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <asp:HiddenField ID="hdnFollowupTableName" runat="server" Value="Inv_SalesQuotationHeader" />
                                <asp:HiddenField ID="hdnIsDiscountApplicable" runat="server" />
                                <asp:HiddenField ID="hdnIsTaxApplicable" runat="server" />
                                <div class="box box-warning box-solid">
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:HiddenField ID="hdnGvSalesQuotationCurrentPageIndex" runat="server" Value="1" />
                                                    <asp:HiddenField ID="hdnGvSalesQuotationCurrentPageIndexBin" runat="server" Value="1" />
                                                    <asp:HiddenField ID="hdnSalesInquiryId" runat="server" Value="0" />

                                                    <table id="tblQutList" class="table-striped table-bordered table table-hover">
                                                        <thead>
                                                            <th>Action</th>
                                                            <th>Quotation No.</th>
                                                            <th>Quotation Date.</th>
                                                            <th>Opportunity No.</th>
                                                            <th>Order Close Date</th>
                                                            <th>Employee Name</th>
                                                            <th>Customer Name</th>
                                                            <th>Amount</th>
                                                        </thead>
                                                        <tbody>
                                                        </tbody>
                                                    </table>
                                                    <asp:Repeater ID="rptPager" runat="server">
                                                        <ItemTemplate>
                                                            <ul class="pagination">
                                                                <li class='<%# Convert.ToBoolean(Eval("Enabled")) ? "page-item" : "page-item active" %>'>
                                                                    <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                                                        CssClass="page-link"></asp:LinkButton>
                                                                </li>
                                                            </ul>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </div>


                                            </div>
                                            <div class="col-md-12" style="text-align: right">
                                                <asp:Label ID="lblTotal" Visible="false" runat="server" Text="<%$ Resources:Attendance,Total %>"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- /.box-body -->
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSQuoteSave" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnSQuoteSaveandPrint" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="BtnReset" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnSQuoteCancel" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="imgBtnRestore" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <%--  <asp:AsyncPostBackTrigger ControlID="GvSalesQuote" />--%>

                               <%-- <asp:AsyncPostBackTrigger ControlID="btnAddTax" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="gvTaxCalculation" />--%>
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblSQDate" runat="server" Text="<%$ Resources:Attendance,Quotation Date %>"></asp:Label>
                                                        <asp:TextBox ID="txtSQDate" runat="server" class="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender" runat="server" TargetControlID="txtSQDate" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblSQNo" runat="server" Text="<%$ Resources:Attendance,Quotation No. %>"></asp:Label>
                                                        <asp:TextBox ID="txtSQNo" runat="server" class="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:HiddenField ID="hdnCustomerId" runat="server" />
                                                        <asp:Label ID="lblCustomerName" runat="server" Text="<%$ Resources:Attendance,Customer Name %>" />
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtCustomer" TabIndex="1" runat="server" class="form-control" ReadOnly="True" />
                                                            <div class="input-group-btn">
                                                                <asp:Button ID="lnkcustomerHistory" runat="server" class="btn btn-primary" OnClientClick="lnkcustomerHistory_OnClick();return false;"
                                                                    Text="Customer History" CausesValidation="False" />
                                                            </div>

                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblContactName" runat="server" Text="<%$ Resources:Attendance,Contact Name %>" />
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtContactName"  onchange="txtContactName_textChanged(); return false;" TabIndex="2" runat="server" class="form-control" />
                                                            <cc1:AutoCompleteExtender ID="txtContactList_AutoCompleteExtender" runat="server"
                                                                DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                                MinimumPrefixLength="0" ServiceMethod="GetContactListCustomer" ServicePath=""
                                                                TargetControlID="txtContactName" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                            <div class="input-group-btn">
                                                           <asp:LinkButton ID="LinkButton1" runat="server" ToolTip="Add New Contact"  OnClientClick="lnkAddNewContact_Click(); return false;" AlternateText="<%$ Resources:Attendance,Add %>" CausesValidation="False"><i class="fa fa-plus-square" style="font-size:35px;padding:5px;margin-top: -5px;"></i></asp:LinkButton></div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Contact No%>" />
                                                        <asp:TextBox ID="txtContactNo" runat="server" class="form-control" ReadOnly="True" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-5">
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Ship To %>" />
                                                        <asp:TextBox ID="txtShipCustomerName" TabIndex="3" runat="server" class="form-control" BackColor="#eeeeee" onchange="ShipToAddressDetail(); return false;" />
                                                       <small><asp:Label ID="ShipAddress" runat="server" Text=""></asp:Label></small>
                                                         <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                            Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                            ServiceMethod="GetContactList" ServicePath="" TargetControlID="txtShipCustomerName"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <td colspan="6"></td>
                                                    <div class="col-md-5">
                                                        <asp:Label ID="lblShipingAddress" runat="server" Text="<%$ Resources:Attendance,Shipping Address %>" />
                                                        <asp:TextBox ID="txtShipingAddress" TabIndex="4" runat="server" class="form-control" BackColor="#eeeeee" onchange="ShipingAddressDetail(); return false;" />
                                                        <small><asp:Label ID="ShippingAddress" runat="server" Text=""></asp:Label></small>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListAddressName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtShipingAddress"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-2">
                                                        <br />
                                                        <asp:Button ID="btnNewaddress" runat="server" Text="New Address" class="btn btn btn-primary"  OnClientClick="btnNewaddress_Click(); return false;" />
                                                         &nbsp;                                                         
                                                        <button id="btnGetLocation" type="button"  class="btn btn-primary" onclick="GetGeoLocation(); return false;"><i class="fa fa-map-marker" style="color:green"></i></button>
                                                        <br />
                                                    </div>                                                   
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblInquiryNo" runat="server" Text="<%$ Resources:Attendance,Opportunity No. %>" />
                                                        <asp:TextBox ID="txtInquiryNo" runat="server" class="form-control" ReadOnly="True" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblInquiryDate" runat="server" Text="<%$ Resources:Attendance,Opportunity Date%>" />
                                                        <asp:TextBox ID="txtInquiryDate" runat="server" class="form-control" ReadOnly="True" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblOrderCloseDate1" runat="server" Text="<%$ Resources:Attendance,Order Close Date %>" />
                                                        <asp:TextBox ID="txtOrderCompletionDate" TabIndex="5" runat="server" class="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" TargetControlID="txtOrderCompletionDate" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblCurrency" runat="server" Text="<%$ Resources:Attendance,Currency %>" />
                                                        <asp:DropDownList ID="ddlCurrency" runat="server" class="form-control" Enabled="false"
                                                            AutoPostBack="true" onchange="GetCurrencyName(); return false;" OnSelectedIndexChanged="ddlCurrency_OnSelectedIndexChanged" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblEmployee" runat="server" Text="<%$ Resources:Attendance,Employee%>" />
                                                        <asp:TextBox ID="txtEmployee" runat="server" class="form-control" ReadOnly="True" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Status %>" />
                                                        <asp:DropDownList ID="ddlStatus" runat="server" class="form-control">
                                                            <asp:ListItem Text="<%$ Resources:Attendance, Open%>" Value="Open" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance, Close%>" Value="Close"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance, Lost%>" Value="Lost"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-4" id="Trans_Div" runat="server">
                                                        <asp:Label ID="lblTransType" runat="server" Text="Transaction Type"></asp:Label>
                                                        <asp:DropDownList ID="ddlTransType"  onchange="ddlTransTypeChange();return false;" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblReason" runat="server" Text="<%$ Resources:Attendance,Reason%>" />
                                                        <asp:TextBox ID="txtReason" TabIndex="6" runat="server" class="form-control" TextMode="MultiLine"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label14" runat="server" Text="Agent Name" />
                                                        <asp:TextBox ID="txtAgentName" onchange="toggleCommissionColumn();return false;" TabIndex="7" runat="server" class="form-control" Enabled="false"
                                                            BackColor="#eeeeee" />
                                                        <cc1:AutoCompleteExtender ID="txtCustomerName_AutoCompleteExtender" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="0" ServiceMethod="GetContactList" ServicePath="" TargetControlID="txtAgentName"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblTemplateName" runat="server" Text="<%$ Resources:Attendance,Template Name %>" />
                                                        <asp:TextBox ID="txtTemplateName" TabIndex="8" runat="server" class="form-control" BackColor="#eeeeee"/>
                                                           
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                            Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                            ServiceMethod="GetCompletionListtemplateName" ServicePath="" TargetControlID="txtTemplateName"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:RadioButton ID="rbtnFormView" Font-Bold="true" Visible="false" runat="server"
                                                            Text="<%$ Resources:Attendance,Form View%>" AutoPostBack="true"
                                                            GroupName="Product" OnCheckedChanged="rbtnFormView_OnCheckedChanged" />
                                                        <asp:RadioButton ID="rbtnAdvancesearchView" Style="margin-left: 20px;" Font-Bold="true" Visible="false" runat="server"
                                                            Text="<%$ Resources:Attendance,Advance Search View%>"
                                                            AutoPostBack="true" GroupName="Product" OnCheckedChanged="rbtnFormView_OnCheckedChanged" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Button ID="btnAddNewProduct" Style="display: none;" runat="server" Text="<%$ Resources:Attendance,Add Product %>"
                                                            CssClass="btn btn-info" Visible="false" OnClick="btnAddNewProduct_Click" />

                                                        <asp:Button ID="btnAddProductScreen" Visible="false" runat="server" Text="<%$ Resources:Attendance,Add Product List %>"
                                                            CssClass="btn btn-info" OnClick="btnAddProductScreen_Click" />

                                                        <asp:Button ID="btnAddtoList" runat="server" Text="<%$ Resources:Attendance,Fill Your Product %>"
                                                            CssClass="btn btn-info" Visible="false" OnClick="btnAddtoList_Click" />
                                                        <br />
                                                    </div>


                                                    <div id="pnlProduct1" runat="server" class="row">
                                                        <br />
                                                        <div class="col-md-12">
                                                            <div class="box box-primary">

                                                                <div class="box-header with-border">
                                                                    <h3 class="box-title">
                                                                        <asp:Label ID="lbladdproduct" runat="server" Text="<%$ Resources:Attendance,Add Product %>"></asp:Label></h3>
                                                                    <div class="box-tools pull-right">
                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                            <i class="fa fa-minus"></i>
                                                                        </button>
                                                                    </div>
                                                                </div>
                                                                <div class="box-body">
                                                                    <div class="form-group">
                                                                        <div class="col-md-12">
                                                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Product Id%>" />
                                                                            <asp:TextBox ID="txtProductcode" runat="server" TabIndex="9" class="form-control" onchange="txtProductCode_TextChanged(this);return false;" BackColor="#eeeeee" />
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionInterval="100"
                                                                                DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductCode"
                                                                                ServicePath="" TargetControlID="txtProductcode" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <br />
                                                                        </div>
                                                                  <%--  <div class="col-md-12" style="text-align: center">
                                                                            <asp:LinkButton ID="lnkProductBuilder" runat="server" Text="Product Builder" ForeColor="Blue"
                                                                                Style="margin-left: 15px" Font-Underline="true" OnClick="lnkProductBuilder_OnClick"></asp:LinkButton>

                                                                            <asp:LinkButton ID="lnkLabelBuilder" runat="server" Text="Label Builder" ForeColor="Blue"
                                                                                Style="margin-left: 15px" Font-Underline="true" OnClick="lnkLabelBuilder_OnClick"></asp:LinkButton>

                                                                            <asp:LinkButton ID="lbkGetProductList" runat="server" Text="Get Product List" ForeColor="Blue"
                                                                                Style="margin-left: 15px" Font-Underline="true" OnClick="lbkGetProductList_OnClick"></asp:LinkButton>

                                                                            <br />
                                                                        </div>--%>
                                                                        <div class="col-md-12">
                                                                            <asp:Label ID="lblProductName" runat="server" Text="<%$ Resources:Attendance,Product Name %>" />
                                                                            <asp:TextBox ID="txtProductName" TabIndex="10" runat="server" class="form-control" onchange="txtProductCode_TextChanged(this)" BackColor="#eeeeee" />
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionListProductName" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtProductName"
                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <asp:HiddenField ID="hdnNewProductId" runat="server" Value="0" />
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblUnit" runat="server" Text="<%$ Resources:Attendance,Unit %>" />
                                                                            <asp:DropDownList TabIndex="11" ID="ddlUnit" runat="server" Class="form-control" />
                                                                            <asp:HiddenField ID="hdnUnitId" runat="server" Value="0" />
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblRequiredQty"  runat="server" Text="<%$ Resources:Attendance,Required Quantity %>" />
                                                                            <asp:TextBox ID="txtRequiredQty" TabIndex="12" runat="server" class="form-control" /><a style="color: Red;">*</a>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                                TargetControlID="txtRequiredQty" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <br />
                                                                        </div>

                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblEstimatedUnitPrice"  runat="server" Text="<%$ Resources:Attendance,Estimated Unit Price %>" />
                                                                            <asp:TextBox ID="txtEstimatedUnitPrice"  onchange="txtEstimatedUnitPrice_TextChanged();return false;" TabIndex="13"  runat="server" class="form-control" />
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                                TargetControlID="txtEstimatedUnitPrice" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblPCurrency" runat="server" Text="<%$ Resources:Attendance,Currency %>" />
                                                                            <asp:DropDownList ID="ddlPCurrency" runat="server" class="form-control" Enabled="false" />
                                                                            <asp:HiddenField ID="hdnCurrencyId" runat="server" Value="0" />
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-12">
                                                                            <asp:Label ID="lblPDescription" runat="server" Text="<%$ Resources:Attendance,Product Description %>" />
                                                                            <asp:Panel ID="pnlPDescription" runat="server" class="form-control" BorderColor="#8ca7c1"
                                                                                BackColor="#ffffff" ScrollBars="Vertical" Visible="false">
                                                                                <asp:Literal ID="txtPDescription" runat="server"></asp:Literal>
                                                                            </asp:Panel>
                                                                            <asp:TextBox ID="txtPDesc" TabIndex="14" runat="server" class="form-control" TextMode="MultiLine"></asp:TextBox>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-12">
                                                                            <div style="display: none">
                                                                                <asp:Button ID="btnFillRelatedProducts" runat="server" OnClick="btnFillRelatedProducts_Click" />
                                                                            </div>
                                                                            <div style="overflow: auto; max-height: 500px;">
                                                                                <asp:UpdatePanel runat="server" ID="upRelatedProduct" UpdateMode="Conditional">
                                                                                    <Triggers>
                                                                                        <asp:AsyncPostBackTrigger ControlID="btnFillRelatedProducts" EventName="Click" />
                                                                                    </Triggers>
                                                                                    <ContentTemplate>
                                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvRelatedProduct" runat="server" Width="100%" AutoGenerateColumns="False">

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
                                                                                                                <td width="90%">
                                                                                                                    <asp:Label ID="lblgvProductId" runat="server" Text='<%#Eval("SubProduct_Id") %>'
                                                                                                                        Visible="false" />
                                                                                                                    <asp:Label ID="lblgvProductName" runat="server" Text='<%#Eval("EProductName")%>' />
                                                                                                                </td>
                                                                                                                <td width="10%" align='<%= PageControlCommon.ChangeTDForDefaultRight()%>'>
                                                                                                                    <asp:ImageButton ID="lnkDes" runat="server" ImageUrl="~/Images/detail.png" Enabled="false" />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <%--  <asp:LinkButton ID="lnkDes" runat="server" Text="<%$ Resources:Attendance,More %>"></asp:LinkButton>--%>
                                                                                                        <asp:Panel ID="PopupMenu1" Width="100%" runat="server">
                                                                                                            <table border="1" cellpadding="0" cellspacing="0" bordercolor="#c6c6c6">
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                        <table height="110" cellspacing="0" bgcolor="#F9F9F9">
                                                                                                                            <tr style="background-color: whitesmoke">
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
                                                                                                                                    <br />
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
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="20%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Unit %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("UnitId") %>' />
                                                                                                        <asp:DropDownList ID="DropDownList1" CssClass="form-control" runat="server">
                                                                                                        </asp:DropDownList>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="8%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Required Quantity %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="txtquantity" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                                        <cc1:FilteredTextBoxExtender ID="filter1" runat="server" TargetControlID="txtquantity"
                                                                                                            FilterType="Numbers">
                                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="13%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Currency %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:DropDownList ID="DropDownList2" CssClass="form-control" runat="server">
                                                                                                        </asp:DropDownList>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="17%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Estimated Unit Price %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtEstimatedUnitPrice" runat="server"
                                                                                                            Enabled="True" TargetControlID="txtEstimatedUnitPrice" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                                                                </asp:TemplateField>
                                                                                            </Columns>

                                                                                            <PagerStyle CssClass="pagination-ys" />

                                                                                        </asp:GridView>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>

                                                                                <asp:HiddenField ID="hdnProductId" runat="server" />
                                                                                <asp:HiddenField ID="hdnProductName" runat="server" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-12" style="text-align: center">
                                                                            <br />
                                                                            <asp:Button ID="btnProductSave" runat="server" Text="<%$ Resources:Attendance,Add Product %>"
                                                                                class="btn btn btn-primary" OnClientClick="AddProductDetail(); return false;" />
                                                                            <asp:Button ID="btnProductCancel" runat="server" class="btn btn btn-primary" Text="<%$ Resources:Attendance,Cancel %>"
                                                                                CausesValidation="False" />
                                                                            <br />
                                                                            <br />
                                                                            <br />
                                                                            <br />
                                                                        </div>
                                                                        <br />
                                                                        <br />
                                                                        <br />
                                                                        <div>
                                                                            <div class="box-body">
                                                                                <div class="row">
                                                                                    <div class="col-md-12">
                                                                                        <div style="overflow: auto; max-height: 500px;" class="grid">                                                                                           
                                                                                            <table id="tblProduct" class="table-striped table-bordered table table-hover">
                                                                                                <thead>
                                                                                                    <tr >
                                                                                                       <!-- <th colspan="1">Edit</th>-->
                                                                                                        <th colspan="1">Delete</th>
                                                                                                        <th colspan="1">S No.</th>
                                                                                                        <th colspan="4">Product Detail</th>
                                                                                                        <th colspan="1">Quantity</th>
                                                                                                        <th colspan="2">Estimated Price</th>
                                                                                                        <th colspan="1">Gross Price</th>
                                                                                                        <th colspan="2">Discount</th>
                                                                                                        <th colspan="2">Tax</th>
                                                                                                        <th colspan="1">LineTotal</th>
                                                                                                        <th colspan="1">Stock</th>
                                                                                                        <th colspan="1">Commission</th>
                                                                                                        <tr style="background: #f0f0f0;">                                                                                                           
                                                                                                            <th scope="col">&nbsp;</th>
                                                                                                            <th scope="col">&nbsp;</th>
                                                                                                            <th scope="col">Product Id</th>
                                                                                                            <th scope="col">
                                                                                                                <span id="tblProductName">Product Name</span> &nbsp;<asp:CheckBox ID="ViewFullName" onchange="ViewFullProduct()" runat="server" />
                                                                                                            </th>
                                                                                                            <th scope="col">Unit</th>
                                                                                                            <th scope="col">Currency</th>
                                                                                                            <th scope="col">Quantity</th>
                                                                                                            <th scope="col"><label id="lblCurrencyName" name="lblCurrencyName"></label>Price</th>
                                                                                                            <th scope="col"><label id="lblCurrencyName1" name="lblCurrencyName1"></label> </th>
                                                                                                            <th scope="col">&nbsp;</th>
                                                                                                            <th scope="col"><label id="lblCurrencyName2" name="lblCurrencyName2"></label>%</th>
                                                                                                            <th scope="col">Value</th>
                                                                                                            <th scope="col">Tax%</th>
                                                                                                            <th scope="col">Tax Value</th>
                                                                                                            <th scope="col">Amount</th>                                                                                                          
                                                                                                            <th scope="col">Stock</th>
                                                                                                            <th scope="col">&nbsp;</th>                                                                                                           
                                                                                                        </tr>
                                                                                                </thead>
                                                                                                <tbody>
                                                                                                </tbody>
                                                                                            </table>
                                                                                            <br />
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12">
                                                            <br />
                                                            <asp:HiddenField ID="Hdn_Tax_By" runat="server" />

                                                            <%--                                                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upProduct">
                                                                    <ProgressTemplate>
                                                                        <div class="modal_Progress">
                                                                            <div class="center_Progress">
                                                                                <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                            </div>
                                                                        </div>
                                                                    </ProgressTemplate>
                                                                </asp:UpdateProgress>--%>
                                                            <asp:HiddenField ID="hdnEditProductId" runat="server" />
                                                        </div>
                                                        <br />
                                                    </div>
                                                </div>

                                                <div class="col-md-6">
                                                    <asp:Label ID="lblAmount" runat="server" Text="<%$ Resources:Attendance,Gross Amount%>" />
                                                    <asp:TextBox ID="txtAmount" runat="server" ReadOnly="true" Class="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-12"></div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblDiscountP" runat="server" Text="<%$ Resources:Attendance,Discount(%) %>" />
                                                    <asp:TextBox ID="txtDiscountP" runat="server" Enabled="false" class="form-control" OnTextChanged="txtDiscountP_TextChanged"
                                                        AutoPostBack="true" />
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True"
                                                        TargetControlID="txtDiscountP" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Attendance,Value %>" />
                                                    <asp:TextBox ID="txtDiscountV" runat="server" Enabled="false" class="form-control" OnTextChanged="txtDiscountV_TextChanged"
                                                        AutoPostBack="true" />
                                                    <asp:TextBox ID="txtPriceAfterTax" runat="server" CssClass="form-control" ReadOnly="True"
                                                        Visible="false" />
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True"
                                                        TargetControlID="txtDiscountV" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <br />
                                                </div>

                                                <div class="col-md-6">
                                                    <asp:Label ID="lblTaxP" runat="server" Text="<%$ Resources:Attendance,Tax(%) %>" />
                                                    <asp:TextBox ID="txtTaxP" runat="server" class="form-control" Enabled="false" OnTextChanged="txtTaxP_TextChanged"
                                                        AutoPostBack="true" />
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" Enabled="True"
                                                        TargetControlID="txtTaxP" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label21" runat="server" Text="<%$ Resources:Attendance,Value %>" />
                                                    <asp:TextBox ID="txtTaxV" runat="server" class="form-control" Enabled="false" OnTextChanged="txtTaxV_TextChanged"
                                                        AutoPostBack="true" />
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" Enabled="True"
                                                        TargetControlID="txtTaxV" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <br />
                                                </div>

                                                <div class="col-md-12">
                                                    <asp:Label ID="lblTotalAmount" runat="server" Text="<%$ Resources:Attendance, Net Amount %>" />
                                                    <asp:TextBox ID="txtTotalAmount" runat="server" class="form-control" ReadOnly="True" />
                                                    <br />
                                                </div>

                                                <div class="col-md-12">
                                                    <cc1:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="0" CssClass="ajax__tab_yuitabview-theme">
                                                        <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="<%$ Resources:Attendance,Header %>">
                                                            <ContentTemplate>
                                                                <asp:UpdatePanel ID="Update_TabPanel1" runat="server">
                                                                    <ContentTemplate>
                                                                        <div class="row">
                                                                            <div class="col-md-12">
                                                                            <cc1:Editor ID="txtHeader" runat="server" Class="form-control" Height="300px" />
                                                                                  <%--    <textarea name="txtHeader" id="txtHeader" runat="server" class="summernote"></textarea--%>
                                                                                <br />
                                                                            </div>
                                                                        </div>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                                <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_TabPanel1">
                                                                    <ProgressTemplate>
                                                                        <div class="modal_Progress">
                                                                            <div class="center_Progress">
                                                                                <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                            </div>
                                                                        </div>
                                                                    </ProgressTemplate>
                                                                </asp:UpdateProgress>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                        <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="<%$ Resources:Attendance,Footer %>">
                                                            <ContentTemplate>
                                                                <asp:UpdatePanel ID="Update_TabPanel2" runat="server">
                                                                    <ContentTemplate>
                                                                        <div class="row">
                                                                            <div class="col-md-12">
                                                                                <cc1:Editor  ID="txtFooter" runat="server" Class="form-control" Height="300px" />
                                                                                 <%-- <textarea name="txtFooter" id="txtFooter" runat="server" class="summernote"></textarea>--%>
                                                                                <br />
                                                                            </div>
                                                                        </div>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                                <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_TabPanel2">
                                                                    <ProgressTemplate>
                                                                        <div class="modal_Progress">
                                                                            <div class="center_Progress">
                                                                                <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                            </div>
                                                                        </div>
                                                                    </ProgressTemplate>
                                                                </asp:UpdateProgress>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                        <cc1:TabPanel ID="TabPanel3" runat="server" HeaderText="<%$ Resources:Attendance,Terms & Conditions %>">
                                                            <ContentTemplate>
                                                                <asp:UpdatePanel ID="Update_TabPanel3" runat="server">
                                                                    <ContentTemplate>
                                                                        <div class="row">
                                                                            <div class="col-md-12">
                                                                                  <cc1:Editor ID="txtCondition1" runat="server" Class="form-control" Height="300px" />
                                                                                 <%--  <textarea name="txtCondition1" id="txtCondition1" runat="server" class="summernote"></textarea>--%>
                                                                                <br />
                                                                            </div>
                                                                        </div>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                                <asp:UpdateProgress ID="UpdateProgress7" runat="server" AssociatedUpdatePanelID="Update_TabPanel3">
                                                                    <ProgressTemplate>
                                                                        <div class="modal_Progress">
                                                                            <div class="center_Progress">
                                                                                <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                            </div>
                                                                        </div>
                                                                    </ProgressTemplate>
                                                                </asp:UpdateProgress>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                    </cc1:TabContainer>
                                                </div>
                                                <div class="col-md-12">
                                                    <br />
                                                </div>
                                                <div class="col-md-12" style="text-align: left">

                                                    <asp:HiddenField ID="Hdn_Edit_ID" runat="server" />
                                                    <asp:Button ID="btnSQuoteSave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                        Visible="true" Class="btn btn-success" OnClientClick="btnSQuoteSave_Click(); return false;" />&nbsp;&nbsp;
                                <asp:Button ID="btnSQuoteSaveandPrint" runat="server" Text="<%$ Resources:Attendance,Save & Print %>"
                                    Visible="false" Class="btn btn-primary" OnClientClick="btnSQuoteSaveandPrint_Click(); return false;" />&nbsp;&nbsp;
                                <asp:Button ID="BtnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                    Class="btn btn-primary" CausesValidation="False" OnClientClick="BtnReset_Click(); return false;" />&nbsp;&nbsp;
                                <asp:Button ID="btnSQuoteCancel" runat="server" Class="btn btn-danger" Text="<%$ Resources:Attendance,Cancel %>"
                                    CausesValidation="False" OnClientClick="BtnReset_Click(); return false;" />&nbsp;&nbsp;
                                <!--<asp:Button ID="btnOrderDetail" runat="server" Class="btn btn-primary" Text="Order Detail"
                                    CausesValidation="False" OnClick="btnOrderDetail_Click" />-->
                                                    <asp:Button ID="Btn_Address_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Followup123" Text="Add Followup" />

                                                    <asp:HiddenField ID="editid" runat="server" />
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
                                <asp:AsyncPostBackTrigger ControlID="btn_bin" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:Button ID="btn_bin" runat="server" OnClick="btn_bin_Click" Style="display: none" />

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div2" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label16" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				 <asp:Label ID="lblTotalRecordsBin" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldNameBin" runat="server" class="form-control"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlFieldNameBin_SelectedIndexChanged">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Quotation Id %>" Value="SQuotation_Id"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Quotation No. %>" Value="SQuotation_No" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Quotation Date %>" Value="Quotation_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Employee Name %>" Value="EmployeeName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Customer Name %>" Value="Customer_Name"></asp:ListItem>
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
                                                <div class="col-lg-4">
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbindBin">
                                                        <asp:TextBox ID="txtValueBin" runat="server" Class="form-control" placeholder="Search From Content"></asp:TextBox>
                                                        <asp:TextBox ID="txtValueBinDate" runat="server" Class="form-control" placeholder="Search From Date" Visible="false"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueBinDate" runat="server" TargetControlID="txtValueBinDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="btnbindBin" runat="server" CausesValidation="False" OnClick="btnbindBin_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search" style="font-size: 25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefreshBin" runat="server" CausesValidation="False" OnClick="btnRefreshBin_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat" style="font-size: 25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imgBtnRestore" CausesValidation="False" runat="server" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>"><span class="far fa-lightbulb" style="font-size: 25px;"></span> </asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid" <%= GvSalesQuoteBin.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvSalesQuoteBin" PageSize="<%# PageControlCommon.GetPageSize() %>" CurrentSortField="Quotation_Date" CurrentSortDirection="DESC"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="false" OnPageIndexChanging="GvSalesQuoteBin_PageIndexChanging"
                                                        OnSorting="GvSalesQuoteBin_OnSorting" AllowSorting="true">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkCurrent" runat="server" OnCheckedChanged="chkCurrent_CheckedChanged"
                                                                        AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelect_CheckedChanged" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quotation No. %>" SortExpression="SQuotation_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvSQNo" runat="server" Text='<%#Eval("SQuotation_No") %>' />
                                                                    <asp:HiddenField ID="hdnTransId" runat="server" Value='<%#Eval("SQuotation_Id") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quotation Date %>" SortExpression="Quotation_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label17" runat="server" Text='<%#GetDate(Eval("Quotation_Date").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Opportunity No. %>" SortExpression="SInquiry_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvSINo" runat="server" Text='<%#Eval("InquiryNo") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name %>" SortExpression="EmployeeName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvEmpId" runat="server" Text='<%# Eval("EmployeeName").ToString() %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Name%>" SortExpression="Customer_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCustomerName" runat="server" Text='<%#Eval("Customer_Name") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvstbin" runat="server" Text='<%# Eval("Field4").ToString() %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Amount %>" SortExpression="TotalAmount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvamtId" runat="server" Text='<%#GetCurrencySymbol(GetAmountDecimal(Eval("TotalAmount").ToString()),Eval("Currency_Id").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                        </Columns>


                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                    <asp:HiddenField ID="HDFSortbin" runat="server" />

                                                    <asp:Repeater ID="rptPager_BIN" runat="server">
                                                        <ItemTemplate>
                                                            <ul class="pagination">
                                                                <li class='<%# Convert.ToBoolean(Eval("Enabled")) ? "page-item" : "page-item active" %>'>
                                                                    <asp:LinkButton ID="LinkButton1" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                                                        CssClass="page-link"
                                                                        OnClick="PageBin_Changed" OnClientClick='<%# !Convert.ToBoolean(Eval("Enabled")) ? "return false;" : "" %>'></asp:LinkButton>
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

    <div class="modal fade" id="NewAddress" tabindex="-1" role="dialog" aria-labelledby="NewAddress_ModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">

                <div class="modal-body">
                    <AA1:AddAddress ID="addaddress" runat="server" />
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
    <div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Product Description</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <label id="pDescription" class="control-label"></label>
                </div>
            </div>
        </div>
    </div>


    <div class="modal fade" id="Modal_GST" tabindex="-1" role="dialog" aria-labelledby="Modal_GSTLabel"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Modal_GSTLabel">TAX Calculation
                    </h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <table id="TblTaxCalculation" width="100%" class="table-bordered">
                                        <thead>
                                            <tr>
                                                <th>Tax Name</th>
                                                <th>Tax Value</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </ContentTemplate>

                    </asp:UpdatePanel>

                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <%--<asp:Button ID="btnSaveGST" Visible="false" runat="server" CssClass="btn btn-primary" ValidationGroup="S_update"
                                Text="<%$ Resources:Attendance,Save %>" OnClick="btnSaveGST_Click" />

                            <asp:Button ID="btnCancelGST" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                Text="<%$ Resources:Attendance,Reset %>" Visible="false" />--%>
                            <asp:HiddenField ID="HiddenField2" runat="server" />
                            <asp:HiddenField ID="HiddenField3" runat="server" />
                            <asp:HiddenField ID="HiddenField4" runat="server" />
                            <asp:HiddenField ID="HiddenField6" runat="server" />
                   <%--         <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary"
                                Text="<%$ Resources:Attendance,Update %>" />--%>
                            <button id="btnClosePopup" type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <asp:UpdateProgress ID="UpdateProgress14" runat="server" AssociatedUpdatePanelID="Update_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress15" runat="server" AssociatedUpdatePanelID="Update_Li">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress12" runat="server" AssociatedUpdatePanelID="Update_List">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress13" runat="server" AssociatedUpdatePanelID="Update_New">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress9" runat="server" AssociatedUpdatePanelID="Update_Bin">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>




 <%--   <asp:UpdateProgress ID="UpdateProgress11" runat="server" AssociatedUpdatePanelID="Update_Modal_GST">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>--%>

    <%--<asp:UpdateProgress ID="UpdateProgress16" runat="server" AssociatedUpdatePanelID="Update_Modal_Button_GST">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>--%>
    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
    <div id="prgBar" class="modal_Progress" style="display: none">
        <div class="center_Progress">
            <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
   
       <script src="../Script/common.js"></script>
    <script src="../Script/ReportSystem.js"></script>
    <script src="../Script/master.js"></script>
 <!--<script src="h ttps://cdn.jsdelivr.net/npm/summernote@0.8.18/dist/summernote.min.js"></script>  -->
   
    <script type="text/javascript">

        //here we chek estimeted unit price not less then according salesprice
        function txtEstimatedUnitPrice_TextChanged()
        {
            var Customer = $('#<%=txtCustomer.ClientID%>').val();
            if (Customer != "" && Customer != null) {
                var Customer = $('#<%=txtCustomer.ClientID%>').val();
                if (Customer != "") {
                    try {
                        Customer = getContactIDFromName(txtCustomer.value);
                    } catch (er) {
                        Customer = "0";
                    }
                } else {
                    Customer = "0";
                }
                var productId = $('#<%=hdnNewProductId.ClientID%>').val();
                if (productId != "" && productId != null)
                {
                    var unitPrice = $('#<%=txtEstimatedUnitPrice.ClientID%>').val();
                    $.ajax({
                        url: 'SalesQuatationJScript.aspx/GetProductPrice',
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
            }
            else
            {
                alert("Please Select Customer First");
            }
        }

        function txtContactName_textChanged()
        {
            debugger;
            var contact=  $('#<%=txtCustomer.ClientID%>').val();
                $.ajax
                ({
                url: 'SalesQuatationJScript.aspx/GetCustomerAdderss',
                method: 'post',
                contentType: "application/json;",
                data: "{'ContactName':'" + contact + "'}",
                dataType: 'json',
                async: true,
                success: function (data)
                {
                    var response = data.d;
                    var myArr = JSON.parse(response);                  
                }
            });
        }



$(document).ready(function () {

    
            
           // $('.summernote').summernote();
    GetSalesQuatationList();
    GetCurrencyName();
           
            //const urlParams = new URLSearchParams(window.location.href);
            //// Get the value of "InquiryId" from the query string
            //const inquiryId = urlParams.get("InquiryId");
            //if(inquiryId!=null && inquiryId!=""){
            //    GetInquiryDetail(inquiryId);
            //}
            const url = window.location.href;
            const inquiryId = url.match(/InquiryId=(\d+)/);

            if (inquiryId && inquiryId[1]) {
                const inquiryIdValue = inquiryId[1];
                GetInquiryDetail(inquiryIdValue);
            }
        });
        function btnSQuoteSaveandPrint_Click() {
            btnSQuoteSave_Click();
        }
        function ddlTransTypeChange() {
            Product_Tblbind();
        }
        function ddlPosted_SelectedIndexChanged()
        {
            btnbindrpt_Click();
        }
        function BtnReset_Click() {
            LI_List_Active();
            $('#<%= Lbl_Tab_New.ClientID %>').text("New");
        var inputString = $('#<%= txtSQNo.ClientID %>').val();
        var parts = inputString.split("-");
        $('#<%= txtSQNo.ClientID %>').val(parts[0] + "-");
        $('#<%=txtCustomer.ClientID%>').val('');
        $('#<%=txtContactName.ClientID%>').val('');
        $('#<%=txtContactNo.ClientID%>').val('');
        $('#<%=txtShipCustomerName.ClientID%>').val('');
        $('#<%=txtShipingAddress.ClientID%>').val('');
        $('#<%=txtInquiryNo.ClientID%>').val('');
        $('#<%=txtInquiryDate.ClientID%>').val('');
        $('#<%=txtOrderCompletionDate.ClientID%>').val('');
        $('#<%=ddlCurrency.ClientID%>').find('option:selected').text('');
        $('#<%=ddlCurrency.ClientID%>').val('');
        GetCurrencyName();
        $('#<%=txtEmployee.ClientID%>').val('');
        $('#<%=ddlPCurrency.ClientID%>').find('option:selected').text('');
        $('#<%=txtReason.ClientID%>').val('');
        $('#<%=txtAgentName.ClientID%>').val('');
        $('#<%=txtTemplateName.ClientID%>').val('');
        $('#<%=txtAmount.ClientID%>').val('');
        $('#<%=txtDiscountP.ClientID%>').val('');
        $('#<%=txtDiscountV.ClientID%>').val('');
        $('#<%=txtTotalAmount.ClientID%>').val('');

        ProductDetail = null;
        $('#<%=editid.ClientID%>').val('');
        $('#<%=hdnSalesInquiryId.ClientID%>').val('');
        $('#<%=ddlTransType.ClientID%>').find('option:selected').val('');
        $('#<%=txtTaxV.ClientID%>').val('');
        $('#<%=txtTaxP.ClientID%>').val('');
        $('#tblProduct > tbody > tr').remove();
        btnRefreshReport_Click();
        }
      
        function lnkAddNewContact_Click() {
            window.open('../EMS/ContactMaster.aspx', 'window', 'width=1024, ');
        }
        function btnNewaddress_Click() {
            window.open('../SystemSetup/AddressMaster.aspx', 'window', 'width=1024, ');
        }
    function getReportDataWithLoc(transId, locId) {
        

        document.getElementById('<%= reportSystem.FindControl("hdnTransId").ClientID %>').value = transId;
        document.getElementById('<%= reportSystem.FindControl("hdnLocId").ClientID %>').value = locId;
        setReportData();
    }
    function Modal_Open_FileUpload() {
        document.getElementById('<%= Btn_Modal_FileUpload.ClientID %>').click();
           }

           function GetInquiryDetail(inquiryId) {
               var Customer_Id = '<%= ViewState["Customer_Id"] %>';
        var ExchangeRate = '<%= ViewState["ExchangeRate"] %>';

        $.ajax({
            url: 'SalesQuatationJScript.aspx/GetInquiryDetail',
            method: 'post',
            contentType: "application/json;",
            data: "{'Id':'" + inquiryId + "','Customer_Id':'" + Customer_Id + "','ExchangeRate':'" + ExchangeRate + "'}",
            dataType: 'json',
            async:false,
            success: function (data) {
                var response = data.d;
                var myArr = JSON.parse(response);
                //console.log(myArr);
                ProductDetail = myArr;
                Product_Tblbind();

            }
        });
    }

        function GetCurrencyName() {
            var CurrencyId = $('#<%=ddlCurrency.ClientID%>').val()
            $.ajax({
                url: 'SalesQuatationJScript.aspx/GetCurrencyNames',
                method: 'post',
                contentType: "application/json;",
                data: "{'CurrencyId':'" + CurrencyId + "'}",
                dataType: 'json',
                async: false,
                success: function (data) {
                    debugger;
                    var response = data.d;                    
                    $('#lblCurrencyName').text(response);
                    $('#lblCurrencyName1').text(response);
                    $('#lblCurrencyName2').text(response);
                }
            });
        }

    function btnbindrpt_Click() {

        
        var ddlLocation = $('#<%=ddlLocation.ClientID%>').val();
          var ddlUser = $('#<%=ddlUser.ClientID%>').val();
          var ddlPosted = $('#<%=ddlPosted.ClientID%>').val();
          var ddlFieldName = $('#<%=ddlFieldName.ClientID%>').val();
          var ddlOption = $('#<%=ddlOption.ClientID%>').val();
          var txtValue = $('#<%=txtValue.ClientID%>').val();

          $.ajax({
              url: 'SalesQuatationJScript.aspx/btnbindrpt_Click',
              method: 'post',
              contentType: "application/json;",
              data: "{'ddlLocation':'" + ddlLocation + "','ddlUser':'" + ddlUser + "','ddlPosted':'" + ddlPosted + "','ddlFieldName':'" + ddlFieldName + "','ddlOption':'" + ddlOption + "','txtValue':'" + txtValue + "'}",
              dataType: 'json',
              success: function (data) {
                  
                  var response = data.d[0];
                  var myArr = JSON.parse(response);
                  var myArr = JSON.parse(response);
                  console.log(myArr);
                  var table = $('#tblQutList').DataTable();
                  table.clear().destroy();
                  var table = $('#tblQutList').DataTable({
                      // DataTable configuration options
                      // For example, searching, paging, etc.
                  });
                  for (var i = 0; i < myArr.length; i++) {

                      var htmlRow = "<tr>";
                      htmlRow = htmlRow + "<td><div class='dropdown' style='position: auto;'><button class='btn btn-default dropdown-toggle' style='align:right' type='button' data-toggle='dropdown'><i class='fa fa-ellipsis-h' aria-hidden='true'></i></button>";
                      htmlRow = htmlRow + "<ul class='dropdown-menu'>";
                      htmlRow = htmlRow + "<li><a href=''  onclick='EditQuatation(" + myArr[i].SQuotation_Id + ");return false;' ><i class='fa fa-pencil'></i>Edit</a></li>";
                      htmlRow = htmlRow + "<li><a href=''  onclick='ViewQuatation(" + myArr[i].SQuotation_Id + ");return false;' ><i class='fa fa-eye'></i>View</a></li>";
                      htmlRow = htmlRow + "<li><a href=''  onclick='PrintQuotation(" + myArr[i].SQuotation_Id + ");return false;' ><i class='fa fa-print'></i>Print</a></li>";
                      htmlRow = htmlRow + "<li><a href=''  onclick='DeleteQuotation(" + myArr[i].SQuotation_Id + "," + myArr[i].Location_Id + ");return false;' ><i class='fa fa-trash'></i>Delete</a></li>";
                      htmlRow = htmlRow + "<li><a href='' onclick='getReportDataWithLoc(" + myArr[i].SQuotation_Id + "," + myArr[i].Location_Id + ");return false;'><i class='fa fa-file'></i>Report System</a></li>";
                      htmlRow = htmlRow + "<li><a href='' onclick='btnFileUpload(" + myArr[i].SQuotation_Id + ");return false;'><i class='fa fa-upload'></i>File Upload</a></li>";
                      htmlRow = htmlRow + "</ul></div></td>";
                      htmlRow = htmlRow + "<td>" + myArr[i].SQuotation_No + "</td>";
                      htmlRow = htmlRow + "<td>" + formatDate(myArr[i].Quotation_Date) + "</td>";
                      htmlRow = htmlRow + "<td>" + myArr[i].SInquiry_No + "</td>";
                      htmlRow = htmlRow + "<td>" + formatDate(myArr[i].OrderCompletionDate) + "</td>";
                      htmlRow = htmlRow + "<td>" + myArr[i].EmployeeName + "</td>";
                      htmlRow = htmlRow + "<td>" + myArr[i].Customer_Name + "</td>";
                      htmlRow = htmlRow + "<td>" +getAmountWithDecimal(myArr[i].TotalAmount) + "</td>";
                      htmlRow = htmlRow + "</tr>";
                      // $('#tblQutList > tbody:last-child').append(htmlRow);
                      table.row.add($(htmlRow)).draw();
                  }


              }
          });

      }
      function GetSalesQuatationList() {
          $("#prgBar").css("display", "block");
          $.ajax({
              url: 'SalesQuatationJScript.aspx/FillSalesQuatationList',
              method: 'post',
              contentType: "application/json;",
              dataType: 'json',
              beforeSend: function () {                  $("#prgBar").css("display", "block");              },              complete: function () {                  $("#prgBar").css("display", "none");              },
              success: function (data) {
                  debugger;
                  if (data.d == "") {
                      $('#tblQutList').DataTable();
                      $("#prgBar").css("display", "none");
                      return;
                  }
                  else {
                      var response = data.d;
                      var myArr = JSON.parse(response);
                      console.log(myArr);
                      var table = $('#tblQutList').DataTable();
                      table.clear().destroy();
                      var table = $('#tblQutList').DataTable({
                          // DataTable configuration options
                          // For example, searching, paging, etc.
                      });
                      for (var i = 0; i < myArr.length; i++) {

                          var htmlRow = "<tr>";
                          htmlRow = htmlRow + "<td><div class='dropdown' style='position: auto;'><button class='btn btn-default dropdown-toggle' style='align:right' type='button' data-toggle='dropdown'><i class='fa fa-ellipsis-h' aria-hidden='true'></i></button>";
                          htmlRow = htmlRow + "<ul class='dropdown-menu'>";
                          htmlRow = htmlRow + "<li><a href=''  onclick='EditQuatation(" + myArr[i].SQuotation_Id + ");return false;' ><i class='fa fa-pencil'></i>Edit</a></li>";
                          htmlRow = htmlRow + "<li><a href=''  onclick='ViewQuatation(" + myArr[i].SQuotation_Id + ");return false;' ><i class='fa fa-eye'></i>View</a></li>";
                          htmlRow = htmlRow + "<li><a href=''  onclick='PrintQuotation(" + myArr[i].SQuotation_Id + ");return false;' ><i class='fa fa-print'></i>Print</a></li>";
                          htmlRow = htmlRow + "<li><a href=''  onclick='DeleteQuotation(" + myArr[i].SQuotation_Id + "," + myArr[i].Location_Id + ");return false;' ><i class='fa fa-trash'></i>Delete</a></li>";
                          htmlRow = htmlRow + "<li><a href='' onclick='getReportDataWithLoc(" + myArr[i].SQuotation_Id + "," + myArr[i].Location_Id + ");return false;'><i class='fa fa-file'></i>Report System</a></li>";
                          htmlRow = htmlRow + "<li><a href='' onclick='btnFileUpload(" + myArr[i].SQuotation_Id + ");return false;'><i class='fa fa-upload'></i>File Upload</a></li>";
                          htmlRow = htmlRow + "</ul></div></td>";
                          htmlRow = htmlRow + "<td>" + myArr[i].SQuotation_No + "</td>";
                          htmlRow = htmlRow + "<td>" + formatDate(myArr[i].Quotation_Date) + "</td>";
                          htmlRow = htmlRow + "<td>" + myArr[i].SInquiry_No + "</td>";
                          htmlRow = htmlRow + "<td>" + formatDate(myArr[i].OrderCompletionDate) + "</td>";
                          htmlRow = htmlRow + "<td>" + myArr[i].EmployeeName + "</td>";
                          htmlRow = htmlRow + "<td>" + myArr[i].Customer_Name + "</td>";
                          htmlRow = htmlRow + "<td>" + getAmountWithDecimal(myArr[i].TotalAmount) + "</td>";
                          htmlRow = htmlRow + "</tr>";
                          //$('#tblQutList > tbody:last-child').append(htmlRow);
                          table.row.add($(htmlRow)).draw();
                      }
                      $('#tblQutList').DataTable();
                      $("#prgBar").css("display", "none");
                  }
              },
              error: function (request, status, error) {
                  alert(request.responseText);
                  $("#prgBar").css("display", "none");
              }
          });

      }

      var ProductDetail = {};

      var EditId = "";

      function PrintQuotation(Trans_Id) {
          window.open('../Sales/SalesQuotationPrint.aspx?Id=' + Trans_Id + '');
      }
      function btnFileUpload(SQuotation_ID, SQuotation_No) {

          document.getElementById('<%= Btn_Modal_FileUpload.ClientID %>').click();


    }
    function DeleteQuotation(SQuotation_Id, Location_Id) {

        $.ajax({
            url: 'SalesQuatationJScript.aspx/DeleteQuotation',
            method: 'post',
            contentType: "application/json;",
            data: "{'QuatationId':'" + SQuotation_Id + "','Location_Id':'" + Location_Id + "'}",
            dataType: 'json',
            success: function (data) {
                
                alert(data.d[0]);
                GetSalesQuatationList();

            }
        });

    }
    function btnRefreshReport_Click() {
        GetSalesQuatationList();
    }
        //this function created by Rahul sharma for view quotation
    function ViewQuatation(SQuotation_Id) {
        $('#<%=btnSQuoteSave.ClientID%>').hide();
          $('#<%=btnSQuoteSaveandPrint.ClientID%>').hide();


          $('#<%=Lbl_Tab_New.ClientID%>').text("Edit");
          $.ajax({
              url: 'SalesQuatationJScript.aspx/editQuatation',
              method: 'post',
              contentType: "application/json;",
              data: "{'Id':'" + SQuotation_Id + "'}",
              dataType: 'json',
              success: function (data) {
                  
                  var response = data.d;
                  var myArr = JSON.parse(response[1]);
                  console.log(myArr);
                  $('#<%=txtSQDate.ClientID%>').val(myArr.Quotation_Date);
               $('#<%=txtSQNo.ClientID%>').val(myArr.Quotation_No);
                $('#<%=txtCustomer.ClientID%>').val(myArr.txtCustomer);
                $('#<%=txtContactName.ClientID%>').val(myArr.txtContactName);
                $('#<%=txtContactNo.ClientID%>').val(myArr.txtContactNo);
                $('#<%=txtShipCustomerName.ClientID%>').val(myArr.txtShipCustomerName);
                $('#<%=txtShipingAddress.ClientID%>').val(myArr.txtShipingAddress);
                $('#<%=txtInquiryNo.ClientID%>').val(myArr.txtInquiryNo);
                $('#<%=txtInquiryDate.ClientID%>').val(myArr.txtInquiryDate);
                $('#<%=txtOrderCompletionDate.ClientID%>').val(myArr.txtOrderCompletionDate);
                //$('#<%=ddlCurrency.ClientID%>').find('option:selected').text();
                  $('#<%=ddlCurrency.ClientID%>').val(myArr.Currency);
                  GetCurrencyName();
                $('#<%=txtEmployee.ClientID%>').val(myArr.txtEmployee);
                ///$('#<%=ddlPCurrency.ClientID%>').find('option:selected').text();
                $('#<%=txtReason.ClientID%>').val(myArr.txtReason);
                $('#<%=txtAgentName.ClientID%>').val(myArr.txtAgentName);
                $('#<%=txtTemplateName.ClientID%>').val(myArr.txtTemplateName);
                $('#<%=txtAmount.ClientID%>').val(myArr.txtTotalAmount);
                $('#<%=txtDiscountP.ClientID%>').val(myArr.txtDiscountP);
                $('#<%=txtDiscountV.ClientID%>').val(myArr.txtDiscountV);
                $('#<%=txtTotalAmount.ClientID%>').val(myArr.txtTotalAmount);
              $find('<%= txtCondition1.ClientID %>').set_content(myArr.txtCondition1);
                $find('<%= txtFooter.ClientID %>').set_content(myArr.txtFooter);
                $find('<%= txtHeader.ClientID %>').set_content(myArr.txtHeader); 
             <%--   '<%= ViewState["DocNo"] %>' = "";
        '<%= ViewState["ApprovalStatus"] %>'=
        '<%= ViewState["TimeStamp"] %>'=--%>
                $('#<%=editid.ClientID%>').val(myArr.editId);
                $('#<%=hdnSalesInquiryId.ClientID%>').val(myArr.hdnSalesInquiryId);
                // $('#<%=ddlTransType.ClientID%>').find('option:selected').val();
                $('#<%=txtTaxV.ClientID%>').val(myArr.txtTaxV);
                $('#<%=txtTaxP.ClientID%>').val(myArr.txtTaxP);
                //console.log(myArr);
                ProductDetail = myArr.Products;
                Product_Tblbind();
                LI_Edit_Active();
            }
        });

    }
        //This function created by rahul sharma for edit quotation 
        function EditQuatation(SQuotation_Id) {

         $('#<%=btnSQuoteSave.ClientID%>').show();
        $("#prgBar").css("display", "block");
        $('#<%=Lbl_Tab_New.ClientID%>').text("Edit");
        $.ajax({
            url: 'SalesQuatationJScript.aspx/editQuatation',
            method: 'post',
            contentType: "application/json;",
            data: "{'Id':'" + SQuotation_Id + "'}",
            dataType: 'json',
            success: function (data) {
                
                var response = data.d;
                var myArr = JSON.parse(response[1]);                
                if (myArr == null) {
                    $('#<%=Lbl_Tab_New.ClientID%>').text("New");
                    alert('Data not found');
                    $("#prgBar").css("display", "none");
                   return;
               }
                $('#<%=txtSQDate.ClientID%>').val(myArr.Quotation_Date);
                $('#<%=txtSQNo.ClientID%>').val(myArr.Quotation_No);
                $('#<%=txtCustomer.ClientID%>').val(myArr.txtCustomer);
                $('#<%=txtContactName.ClientID%>').val(myArr.txtContactName);
                $('#<%=txtContactNo.ClientID%>').val(myArr.txtContactNo);
                $('#<%=txtShipCustomerName.ClientID%>').val(myArr.txtShipCustomerName);
                $('#<%=txtShipingAddress.ClientID%>').val(myArr.txtShipingAddress);
                $('#<%=txtInquiryNo.ClientID%>').val(myArr.txtInquiryNo);
                $('#<%=txtInquiryDate.ClientID%>').val(myArr.txtInquiryDate);
                $('#<%=txtOrderCompletionDate.ClientID%>').val(myArr.txtOrderCompletionDate);
                //$('#<%=ddlCurrency.ClientID%>').find('option:selected').text();ddlPCurrency
                $('#<%=ddlCurrency.ClientID%>').val(myArr.Currency);
                $('#<%=ddlPCurrency.ClientID%>').val(myArr.Currency);
                GetCurrencyName();
                $('#<%=txtEmployee.ClientID%>').val(myArr.txtEmployee);
                ///$('#<%=ddlPCurrency.ClientID%>').find('option:selected').text();
                $('#<%=txtReason.ClientID%>').val(myArr.txtReason);
                $('#<%=txtAgentName.ClientID%>').val(myArr.txtAgentName);
                $('#<%=txtTemplateName.ClientID%>').val(myArr.txtTemplateName);
                $('#<%=txtAmount.ClientID%>').val(myArr.txtTotalAmount);
                $('#<%=txtDiscountP.ClientID%>').val(myArr.txtDiscountP);
                $('#<%=txtDiscountV.ClientID%>').val(myArr.txtDiscountV);
                $('#<%=txtTotalAmount.ClientID%>').val(myArr.txtTotalAmount);        
               $find('<%= txtCondition1.ClientID %>').set_content(myArr.txtCondition1);
                $find('<%= txtFooter.ClientID %>').set_content(myArr.txtFooter);
                $find('<%= txtHeader.ClientID %>').set_content(myArr.txtHeader);


                $('#<%=editid.ClientID%>').val(myArr.editId);
                $('#<%=hdnSalesInquiryId.ClientID%>').val(myArr.hdnSalesInquiryId);
                $('#<%=ddlTransType.ClientID%>').find('option:selected').val(myArr.ddlTransType);
                $('#<%=txtTaxV.ClientID%>').val(myArr.txtTaxV);
                $('#<%=txtTaxP.ClientID%>').val(myArr.txtTaxP);
                //console.log(myArr);
                ShipingAddressDetail();
                ProductDetail = myArr.Products;
                Product_Tblbind();
                LI_Edit_Active();
                $("#prgBar").css("display", "none");
            }
        });

        }
       
    //this function created by Rahul sharma for Save Quotation 
    function btnSQuoteSave_Click() {
        $("#prgBar").css("display", "block");
        var objSalesQta = new Object();        
        objSalesQta.Quotation_Date = $('#<%=txtSQDate.ClientID%>').val();
        objSalesQta.Quotation_No = $('#<%=txtSQNo.ClientID%>').val();
        objSalesQta.txtCustomer = $('#<%=txtCustomer.ClientID%>').val();
        objSalesQta.txtContactName = $('#<%=txtContactName.ClientID%>').val();
        objSalesQta.txtContactNo = $('#<%=txtContactNo.ClientID%>').val();
        objSalesQta.txtShipCustomerName = $('#<%=txtShipCustomerName.ClientID%>').val();
        objSalesQta.txtShipingAddress = $('#<%=txtShipingAddress.ClientID%>').val();
        objSalesQta.txtInquiryNo = $('#<%=txtInquiryNo.ClientID%>').val();
        objSalesQta.txtInquiryDate = $('#<%=txtInquiryDate.ClientID%>').val();
        objSalesQta.txtOrderCompletionDate = $('#<%=txtOrderCompletionDate.ClientID%>').val();
        objSalesQta.CurrencyText = $('#<%=ddlCurrency.ClientID%>').find('option:selected').text();
        objSalesQta.Currency = $('#<%=ddlCurrency.ClientID%>').val();
        objSalesQta.txtEmployee = $('#<%=txtEmployee.ClientID%>').val();
        objSalesQta.ddlStatus = $('#<%=ddlStatus.ClientID%>').find('option:selected').text();
        objSalesQta.txtReason = $('#<%=txtReason.ClientID%>').val();
        objSalesQta.txtAgentName = $('#<%=txtAgentName.ClientID%>').val();
        objSalesQta.txtTemplateName = $('#<%=txtTemplateName.ClientID%>').val();
        objSalesQta.GrossAmount = $('#<%=txtAmount.ClientID%>').val();
        objSalesQta.txtDiscountP = $('#<%=txtDiscountP.ClientID%>').val();
        objSalesQta.txtDiscountV = $('#<%=txtDiscountV.ClientID%>').val();
        objSalesQta.txtTotalAmount = $('#<%=txtTotalAmount.ClientID%>').val();
          objSalesQta.txtHeader = $find('<%= txtHeader.ClientID%>').get_content();
        objSalesQta.txtFooter = $find('<%= txtFooter.ClientID%>').get_content();
        objSalesQta.txtCondition1 = $find('<%= txtCondition1.ClientID%>').get_content();
        objSalesQta.Products = ProductDetail;
        debugger;
        if (objSalesQta.Products == "" || objSalesQta.Products == null) {
            alert("Please Add Product");
            $("#prgBar").css("display", "none");
            return;
        }
        else if (Object.keys(ProductDetail).length == 0) {
            alert("Please Add Product");
            $("#prgBar").css("display", "none");
            return;
        }
        objSalesQta.DocNo = '<%= ViewState["DocNo"] %>';
        objSalesQta.ApprovalStatus = '<%= ViewState["ApprovalStatus"] %>';
        objSalesQta.TimeStamp = '<%= ViewState["TimeStamp"] %>';
        objSalesQta.editId = $('#<%=editid.ClientID%>').val();
        objSalesQta.hdnSalesInquiryId = $('#<%=hdnSalesInquiryId.ClientID%>').val();
        objSalesQta.ddlTransType = $('#<%=ddlTransType.ClientID%>').find('option:selected').val();
        objSalesQta.txtTaxV = $('#<%=txtTaxV.ClientID%>').val();
        objSalesQta.txtTaxP = $('#<%=txtTaxP.ClientID%>').val();
        //console.log(JSON.stringify(objSalesQta));
        var SaveData = JSON.stringify(objSalesQta);

        //ajax call to set data
        $.ajax({
            url: 'SalesQuatationJScript.aspx/SaveQuotation',
            method: 'post',
            contentType: "application/json;",
            data: "{objquo:" + SaveData + "}",
            dataType: 'json',
            success: function (data) {
                var response = data.d;
                
                alert(response[0]);

                if (response[0] != "fail") {
                    $("#prgBar").css("display", "none");
                    reset();
                    if (confirm("Do you want to print the Quotation")) {
                        PrintQuotation(response[1]);
                    }
                    reloadPageWithNewURL();
                    //fillTblInvoice();
                }

            },
            error: function (request, status, error) {
                alert(request.responseText);
                $("#prgBar").css("display", "none");
            }
        });

    }
      
function toggleCommissionColumn() {
  var table = document.getElementById("tblProduct");

    // Find the index of the "Commission" column in the second row of the header
    var Agent=$('#<%=txtAgentName.ClientID%>').val();

  var headerRow = table.tHead.rows[0]; // Second row in the header 
  //alert(Agent);
  
  if(Agent == "") {
    // Hide the header cell
    headerRow.cells['10'].style.display = "none";

    // Loop through all the rows in the tbody and hide the corresponding cell
    var tbody = table.tBodies[0];
    var rows = tbody.rows;
    for (var i = 0; i < rows.length; i++) {
      rows[i].cells['16'].style.display = "none";
    }
  }
  else
  {
      // Show the header cell
      headerRow.cells['10'].style.display = "block";

      // Loop through all the rows in the tbody and hide the corresponding cell
      var tbody = table.tBodies[0];
      var rows = tbody.rows;
      for (var i = 0; i < rows.length; i++) {
          rows[i].cells['16'].style.display = "block";
      }
  }
  setTimeout(function () { jQuery('#<%=txtProductcode.ClientID%>').focus() }, 500);
}



    function reloadPageWithNewURL() {
        const currentUrl = window.location.href;
        var newUrl = currentUrl.split("?")
        //alert(newUrl[0]);

        //  var newURL = 'http://localhost:55203/Sales/SalesQuatationJScript.aspx';
        window.location = newUrl[0];
        // window.location.href = newURL;
        // window.location.reload();
    }
    function formatDate(dateString) {
        var months = [
          "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
        ];

        var date = new Date(dateString);
        var day = date.getDate();
        var monthIndex = date.getMonth();
        var year = date.getFullYear();

        return day + "-" + months[monthIndex] + "-" + year;
    }
    function GetTaxAllowOrnot() {
        $.ajax({
            url: 'SalesQuatationJScript.aspx/GetTaxPerameter',
            method: 'post',
            contentType: "application/json;",
            dataType: 'json',
            success: function (data) {
                var text = data.d;
                console.log(text);
                var table = document.getElementById("tblProduct");

                var headerRow = table.tHead.rows[0];
                var headerRow2 = table.tHead.rows[1];
                if (text === "False") {

                    // Hide the header cell
                    headerRow.cells['7'].style.display = "none";
                    headerRow2.cells['12'].style.display = "none";
                    headerRow2.cells['13'].style.display = "none";
                    // Loop through all the rows in the tbody and hide the corresponding cell
                    var tbody = table.tBodies[0];
                    var rows = tbody.rows;
                    for (var i = 0; i < rows.length; i++) {
                        rows[i].cells['12'].style.display = "none";
                        rows[i].cells['13'].style.display = "none";
                    }
                }
                else {
                    //Show the header cell
                    headerRow.cells['7'].removeAttribute('style');
                    headerRow2.cells['12'].removeAttribute('style');
                    headerRow2.cells['13'].removeAttribute('style');
                    // Loop through all the rows in the tbody and hide the corresponding cell
                    var tbody = table.tBodies[0];
                    var rows = tbody.rows;
                    for (var i = 0; i < rows.length; i++) {
                        rows[i].cells['12'].removeAttribute('style');
                        rows[i].cells['13'].removeAttribute('style');
                    }
                }
            }
        });
    }
    function ResetProductDetail() {
        $('#<%=txtProductcode.ClientID%>').val('');
        $('#<%=hdnNewProductId.ClientID%>').val('');
        $('#<%=txtProductName.ClientID%>').val('');
        $('#<%=ddlUnit.ClientID%>').val('');
        $('#<%=ddlUnit.ClientID%>').find('option:selected').text('');  
        $('#<%=txtRequiredQty.ClientID%>').val('');
        $('#<%=txtEstimatedUnitPrice.ClientID%>').val('');
        $('#<%=txtPDesc.ClientID%>').val('');
    }
        //Here we get Tax Percentage
        function GetTaxPercentage(ProductId) {
            if ($('#<%=ddlTransType.ClientID%>').find('option:selected').val() == null || $('#<%=ddlTransType.ClientID%>').find('option:selected').val() == "") {
                return 0;
            }
            var taxPercentage = "0";
            $.ajax({
                url: 'SalesQuatationJScript.aspx/GetAddTaxPercentage',
                method: 'post',
                contentType: "application/json;",
                data: "{ProductId:" + ProductId + ",transType:" +  $('#<%=ddlTransType.ClientID%>').find('option:selected').val() + "}",
                dataType: 'json',
                async:false,
                success: function (data) {
                    debugger;
                    var response = data.d;
                    taxPercentage = response;   
                },
                error: function (request, status, error) {
                    alert(request.responseText);

                }
            });
           return taxPercentage;
        }
        
    function AddProductDetail() {

        if ($('#<%=txtCustomer.ClientID%>').val() == "") {
            alert("Please Add Customer");
             $('#<%=txtCustomer.ClientID%>').focus();
             return;
         }
        if ($('#<%=txtProductcode.ClientID%>').val() == "") {
            alert("Please Add Product Detail");
            $('#<%=txtProductcode.ClientID%>').focus();
            return;
        }
        if ($('#<%=txtProductName.ClientID%>').val() == "") {
            alert("Please Add Product Detail");
              $('#<%=txtProductName.ClientID%>').focus();
            return;
        }
        if ($('#<%=txtRequiredQty.ClientID%>').val() == "") {
            alert("Please Add Product Quantity");
             $('#<%=txtRequiredQty.ClientID%>').focus();
            return;
        }
        if ($('#<%=txtEstimatedUnitPrice.ClientID%>').val() == "") {
            alert("Please Add Product Price");
              $('#<%=txtEstimatedUnitPrice.ClientID%>').focus();
            return;
        }
        if (EditId == "") {
            var ProductId = $('#<%=txtProductcode.ClientID%>').val();
          var hdnNewProductId = $('#<%=hdnNewProductId.ClientID%>').val();
          var ProductName = $('#<%=txtProductName.ClientID%>').val();
          var UnitId = $('#<%=ddlUnit.ClientID%>').val();
          var UnitName = $('#<%=ddlUnit.ClientID%>').find('option:selected').text();
          var CurrencyName = $('#<%=ddlPCurrency.ClientID%>').find('option:selected').text();
          var CurrencyId = $('#<%=ddlPCurrency.ClientID%>').val();
          var Quantity = getAmountWithDecimal($('#<%=txtRequiredQty.ClientID%>').val());
          var UnitPrice = getAmountWithDecimal($('#<%=txtEstimatedUnitPrice.ClientID%>').val());
          if (UnitPrice == "" || UnitPrice == null) {
              alert("Please enter Estimated Price");
              return;
          }
          var Description = $('#<%=txtPDesc.ClientID%>').val();
        var GrossPrice = UnitPrice;
        var Discount = "0";
        var DiscountValue = "0";

        var TaxP =getAmountWithDecimal(GetTaxPercentage(hdnNewProductId));
        debugger;
        var TaxV = "0";
        var TotalAmount = UnitPrice;
        var txtgvAgentCommission = "0.000";
        var new_arr = [];

        ProductDetail.length;
        if (Object.keys(ProductDetail).length > 0) {
            new_arr = {
                'ProductId': ProductId,
                'hdnNewProductId': hdnNewProductId,
                'ChkQuatationDetail': '',
                'ProductName': ProductName,
                'UnitId': UnitId,
                'UnitName': UnitName,
                'CurrencyId': CurrencyId,
                'CurrencyName': CurrencyName,
                'RequiredQuantity': Quantity,
                'UnitPrice': UnitPrice,
                'GrossPrice': GrossPrice,
                'Discount': Discount,
                'DiscountValue': DiscountValue,
                'TaxP': TaxP,
                'TaxV':TaxV,
                'Description': Description,
                'TotalAmount': TotalAmount,
                'txtgvAgentCommission': txtgvAgentCommission
            };
            ProductDetail[parseInt(Object.keys(ProductDetail).length)] = new_arr;
        } else {

            new_arr[Object.keys(ProductDetail).length] = {
                'ProductId': ProductId,
                'hdnNewProductId': hdnNewProductId,
                'ProductName': ProductName,
                'UnitId': UnitId,
                'UnitName': UnitName,
                'CurrencyId': CurrencyId,
                'CurrencyName': CurrencyName,
                'RequiredQuantity': Quantity,
                'UnitPrice': UnitPrice,
                'GrossPrice': GrossPrice,
                'Discount': Discount,
                'DiscountValue': DiscountValue,
                'TaxP': TaxP,
                'TaxV': TaxV,
                'Description': Description,
                'TotalAmount': TotalAmount,
                'txtgvAgentCommission': txtgvAgentCommission
            };
            ProductDetail = new_arr;
        }
        Product_Tblbind();
        ResetProductDetail();
       $('#<%=txtProductcode.ClientID%>').focus();

    } else {
        var ProductId = $('#<%=txtProductcode.ClientID%>').val();
          var ProductName = $('#<%=txtProductName.ClientID%>').val();
          var UnitId = $('#<%=ddlUnit.ClientID%>').val();
          var UnitName = $('#<%=ddlUnit.ClientID%>').find('option:selected').text();
          var CurrencyName = $('#<%=ddlPCurrency.ClientID%>').find('option:selected').text();
          var CurrencyId = $('#<%=ddlPCurrency.ClientID%>').val();
          var Quantity = $('#<%=txtRequiredQty.ClientID%>').val();
          var UnitPrice = $('#<%=txtEstimatedUnitPrice.ClientID%>').val();
          var Description = $('#<%=txtPDesc.ClientID%>').val();
          row = EditId;
          ProductDetail[row]["ProductId"] = ProductId;
          ProductDetail[row]["ProductName"] = ProductName;
          ProductDetail[row]["UnitName"] = UnitName;
          ProductDetail[row]["UnitId"] = UnitId;
          ProductDetail[row]["RequiredQuantity"] = Quantity;
          ProductDetail[row]["UnitPrice"] = UnitPrice;
          ProductDetail[row]["CurrencyId"] = CurrencyId;
          ProductDetail[row]["CurrencyName"] = CurrencyName;
          ProductDetail[row]["Description"] = Description;
          EditId = "";
          Product_Tblbind();
          ResetProductDetail();
             $('#<%=txtProductcode.ClientID%>').focus();
        }
      
  }
  function GetStockQty(ProductId) {
      
      var stock = [];
      $.ajax({
          type: "POST",
          url: 'SalesOrderJScript.aspx/GetStockList',
          async: false,
          contentType: "application/json; charset=utf-8",
          data: "{'ProductId':'" + ProductId + "'}",
          success: function (data) {
              stock = data;
          },
          error: function (response) {
              alert(response.responseText);
          },
          failure: function (response) {
              alert(response.responseText);
          }
      });
      return stock;
  }
  function DeleteTableRow(row) {
      var confirmed = confirm("Are you sure you want to delete this record?");
      if (confirmed) {
          

          // Assuming ProductDetail is an array of objects
          if (row >= 1 && row <= ProductDetail.length) {
              ProductDetail.splice(row - 1, 1); // Remove the row at the specified index
          }

          Product_Tblbind();
      }
     
  }

  function editTableRow(row) {
      
      var ProductId = ProductDetail[row]["ProductId"];
      var ProductName = ProductDetail[row]["ProductName"];
      var UnitName = ProductDetail[row]["UnitName"];
      var UnitId = ProductDetail[row]["UnitId"];
      var Quantity = ProductDetail[row]["RequiredQuantity"];
      var Price = ProductDetail[row]["UnitPrice"];
      var Currency = ProductDetail[row]["CurrencyId"];
      var CurrencyName = ProductDetail[row]["CurrencyName"];
      var Description = ProductDetail[row]["Description"];

      $('#<%=txtProductcode.ClientID%>').val(ProductId);

      $('#<%=txtProductName.ClientID%>').val(ProductName);
        $('#<%=ddlUnit.ClientID%>').val(UnitId);
        $('#<%=ddlUnit.ClientID%>').find('option:selected').text(UnitName);
        $('#<%=ddlPCurrency.ClientID%>').find('option:selected').text(CurrencyName);
        $('#<%=ddlPCurrency.ClientID%>').val(Currency);
        $('#<%=txtRequiredQty.ClientID%>').val(getAmountWithDecimal(Quantity));
        $('#<%=txtEstimatedUnitPrice.ClientID%>').val(getAmountWithDecimal(Price));
        $('#<%=txtPDesc.ClientID%>').val(getAmountWithDecimal(Description));
        EditId = "" + row + "";

  }

          function Show_Modal_GST(ProductID) {
              debugger;
            $.ajax({
                url: 'SalesQuatationJScript.aspx/GetProductTax',
                method: 'post',
                contentType: "application/json;",
                data: "{ProductId:" + ProductID + "}",
                dataType: 'json',
                async: false,
                success: function (data) {
                    var text = JSON.parse(data.d);
                    debugger;
                    var TaxArray = Object.keys(text).length;
                    $('#TblTaxCalculation > tbody > tr').remove();
                    for (var i = 0; i < TaxArray; i++)
                    {
                        var htmlRow = "<tr>";
                        htmlRow = htmlRow + "<td>" + text[i].Tax_Name + "</td>";
                        htmlRow = htmlRow + "<td>" + text[i].Tax_Value + "</td>";
                        htmlRow = htmlRow + "</tr>";
                        $('#TblTaxCalculation > tbody:last-child').append(htmlRow);
                    }
                    document.getElementById('<%= Btn_GST_Modal.ClientID %>').click();
                }
            });
           
        }
    //this function change Create By Rahul For Product Table Bind
    function Product_Tblbind() {
        debugger;
        
        var ProductArray = Object.keys(ProductDetail).length;
        var GrossAmount = "0";
        var NetAmount = "0";
        var UnitPrice = "0";
        var DiscountValue = "0";
        console.log(ProductDetail);
        $('#tblProduct > tbody > tr').remove();
        for (var i = 0; i < ProductArray; i++) {
            GrossAmount = parseFloat(GrossAmount) + parseFloat(ProductDetail[i].GrossPrice);
            NetAmount = parseFloat(NetAmount) + parseFloat(ProductDetail[i].TotalAmount);
            UnitPrice = parseFloat(UnitPrice) + parseFloat(ProductDetail[i].UnitPrice);
            DiscountValue = parseFloat(DiscountValue) + parseFloat(ProductDetail[i].DiscountValue);

            $('#<%=txtAmount.ClientID%>').val(getAmountWithDecimal(GrossAmount));
        $('#<%=txtTotalAmount.ClientID%>').val(getAmountWithDecimal(NetAmount));
          var StockQTy = GetStockQty(ProductDetail[i].hdnNewProductId);
          var Taxpercent = GetTaxPercentage(ProductDetail[i].hdnNewProductId);
          row = $('#tblProduct tbody tr').length;
          row = row + 1;
          var htmlRow = "<tr>";
         // htmlRow += "<td><i class='fa fa-pencil' id='btnedit" + i + "' onClick='editTableRow(" + i + "); return false;' style='font-size:15px'></i></td>";
          htmlRow += "<td><i class='fa fa-trash' id='btndelete" + i + "' onClick='DeleteTableRow(" + row + "); return false;' style='font-size:15px'></i></td>";
          htmlRow += "<td>" + row + "</td>";
          htmlRow += "<td><label id='ProductId" + i + "'>" + ProductDetail[i].ProductId + " </label></td>";
          htmlRow += "<td><label id='shortProductName'>" + ProductDetail[i].ProductName.substring(0, 8) + '...' + "</label><input id='txtDescription" + row + "' value='" + ProductDetail[i].Description + "' type='hidden'></input><button type='button' id='ModelHover' onClick='ProModelopen(" + row + ");' class='btn btn-primary'><i class='fa fa-search'></i></button></td>";

          htmlRow += "<td>" + ProductDetail[i].UnitName + "</td>";
          htmlRow += "<td>" + ProductDetail[i].CurrencyName + "</td>";
          htmlRow += "<td><input type='text' id='tblQuantity" + i + "' value=" + getAmountWithDecimal(ProductDetail[i].RequiredQuantity) + " onchange='calculatePrice(" + i + ");return false;' style='color:#4D4C4C;width:50px;' ></input></td>";
            htmlRow += "<td><input type='text' id='tblUnitPrice" + i + "' onchange='GetUnitPrice(" + i + "," + ProductDetail[i].hdnNewProductId + ");return false;' value=" + getAmountWithDecimal(ProductDetail[i].UnitPrice) + " style='color:#4D4C4C;width:50px;' ></input></td>";
          htmlRow += "<td><label id='tblKwdPrice" + i + "'>" + ProductDetail[i].GrossPrice + "</label></td>";
          htmlRow += "<td><label id='tblGross" + i + "'>" + ProductDetail[i].GrossPrice + "</label></td>";
          htmlRow += "<td><input type='text' id='tblDiscount" + i + "' onchange='calculatePrice(" + i + ",0);return false;' Value=" + getAmountWithDecimal(ProductDetail[i].Discount) + " style='color:#4D4C4C;width:50px;' ></input></td>";
          htmlRow += "<td><input type='text' id='tblDiscountValue" + i + "' onchange='calculatePrice(" + i + ",1);return false;' Value=" + getAmountWithDecimal(ProductDetail[i].DiscountValue) + " style='color:#4D4C4C;width:50px;' ></input></td>";
          //htmlRow += "<td><input type='text' id=tblTax" + i + " readonly/><i class='fa fa-plus' onclick='Show_Modal_GST(" + ProductDetail[i].hdnNewProductId + "); return false;' style='font-size:24px;color:blue'></i></td>";
          //htmlRow += "<td><input type='text' id=tblTaxValue" + i + " readonly/></td>";
            //
          htmlRow = htmlRow + "<td><input type='text' id='tblTaxPercentage" + i + "' onchange='calculateDetail(" + i + ",0);return false;' Value=" + ProductDetail[i].TaxP + " style='color:#4D4C4C;width:50px;' readonly>&nbsp;</input><i class='fa fa-plus' onclick='Show_Modal_GST(" + ProductDetail[i].hdnNewProductId + "); return false;' style='font-size:24px;color:blue'></i></td>";
          htmlRow = htmlRow + "<td><input type='text' id='tblTaxValue" + i + "' onchange='calculateDetail(" + i + ",0);return false;' Value=" + ProductDetail[i].TaxV + " style='color:#4D4C4C;width:50px;' readonly></input></td>";
          htmlRow += "<td><label id='tblTotalAmount" + i + "'>" + ProductDetail[i].TotalAmount + "</label></td>";
          if (StockQTy.d.length === 0)
          {
              htmlRow += "<td><a href='javascript:void(0)' onClick='lnkStockInfo_Command(\"" + ProductDetail[i].hdnNewProductId + "\")' >0.000</a></td>"
          } else {
              htmlRow += "<td><a href='javascript:void(0)' onClick='lnkStockInfo_Command(\"" + ProductDetail[i].hdnNewProductId + "\")' >" + StockQTy.d[0] + "</a></td>"
          }
          htmlRow += "<td><input type='text' id=tblAgentCommission" + i + " onchange='CommisionChange(" + i + "); return false;' value=" + getAmountWithDecimal(ProductDetail[i].txtgvAgentCommission) + "><td/>"
          htmlRow += "</tr>";
          $('#tblProduct > tbody:last-child').append(htmlRow);

          calculatePrice(i,0);
      }

      var DiscountP = parseFloat(DiscountValue) * 100 / parseFloat(GrossAmount);
      $('#<%=txtDiscountV.ClientID%>').val(getAmountWithDecimal(DiscountValue));
      $('#<%=txtDiscountP.ClientID%>').val(getAmountWithDecimal(DiscountP));
          //GetHeaderTotal();
        GrossCalculate();
        GetTaxAllowOrnot();
        toggleCommissionColumn();
    }


        //This Function Created for SalesPrice Get According Parameter
        function GetUnitPrice(row, ProductId) {
            debugger;
            $("#prgBar").css("display", "block");
            const unitPrice = parseFloat(document.getElementById('tblUnitPrice' + row + '').value) || 0;
            var Customer = $('#<%=txtCustomer.ClientID%>').val();
            if (Customer != "") {
                try {
                    Customer = getContactIDFromName(txtCustomer.value);
                } catch (er) {
                    Customer = "0";
                }
            } else {
                Customer = "0";
            }
            $.ajax({
                url: 'SalesQuatationJScript.aspx/GetProductPrice',
                method: 'post',
                contentType: "application/json;",
                data: "{ProductId:" + ProductId + ",CustomerID:\"" + Customer + "\"}",
                dataType: 'json',
                success: function (data) {
                    var response = data.d;
                    var salesPrice = parseFloat(response);

                    if (unitPrice < salesPrice) {
                        alert("Sales Price not less than " + salesPrice);
                        $("#prgBar").css("display", "none");
                        document.getElementById('tblUnitPrice' + row + '').value = salesPrice;
                        // Move the code that depends on salesPrice here
                        calculatePrice(row, 0);
                        return;
                    } else {
                        calculatePrice(row, 0);
                    }
                    $("#prgBar").css("display", "none");
                },
                error: function (request, status, error) {
                    alert(request.responseText);
                    $("#prgBar").css("display", "none");
                }
            });
        }

        function CommisionChange(row)
        {
            
            var Commision=$('#tblAgentCommission'+row+'').val();
            ProductDetail[row].txtgvAgentCommission = Commision;
        }
        //this function created by Rahul sharma if Prodcut Name Checkbox is true then its working and its created for Show full product name
      function ViewFullProduct() {
          $("#prgBar").css("display", "block");
          
          var ViewFullName = document.getElementById('<%=ViewFullName.ClientID%>');
      var ProductArray = Object.keys(ProductDetail).length;

      var GrossAmount = "0";
      var NetAmount = "0";
      var UnitPrice = "0";
      var DiscountValue = "0";
      if (ViewFullName.checked) {
          var ProductArray = ProductDetail.length;
          //var LocationOut = LoctionList();
          // console.log(ProductDetail);
          $('#tblProduct > tbody > tr').remove();
          for (var i = 0; i < ProductArray; i++) {
              GrossAmount = parseFloat(GrossAmount) + parseFloat(ProductDetail[i].GrossPrice);
              NetAmount = parseFloat(NetAmount) + parseFloat(ProductDetail[i].TotalAmount);
              UnitPrice = parseFloat(UnitPrice) + parseFloat(ProductDetail[i].UnitPrice);
              DiscountValue = parseFloat(DiscountValue) + parseFloat(ProductDetail[i].DiscountValue);

              $('#<%=txtAmount.ClientID%>').val(getAmountWithDecimal(GrossAmount));
          $('#<%=txtTotalAmount.ClientID%>').val(getAmountWithDecimal(NetAmount));
            var StockQTy = GetStockQty(ProductDetail[i].hdnNewProductId);
            var Taxpercent = GetTaxPercentage(ProductDetail[i].hdnNewProductId);
            row = $('#tblProduct tbody tr').length;
            row = row + 1;
            var htmlRow = "<tr>";
            //htmlRow += "<td><i class='fa fa-pencil' id='btnedit" + i + "' onClick='editTableRow(" + i + "); return false;' style='font-size:15px'></i></td>";
            htmlRow += "<td><i class='fa fa-trash' id='btndelete" + i + "' onClick='DeleteTableRow(" + row + "); return false;' style='font-size:15px'></i></td>";
            htmlRow += "<td>" + row + "</td>";
            htmlRow += "<td><label id='ProductId" + i + "'>" + ProductDetail[i].ProductId + " </label></td>";
            htmlRow += "<td><label id='shortProductName'>" + ProductDetail[i].ProductName + '...' + "</label><input id='txtDescription" + row + "' value='" + ProductDetail[i].Description + "' type='hidden'></input><button type='button' id='ModelHover' onClick='ProModelopen(" + row + ");' class='btn btn-primary'><i class='fa fa-search'></i></button></td>";

            htmlRow += "<td>" + ProductDetail[i].UnitName + "</td>";
            htmlRow += "<td>" + ProductDetail[i].CurrencyName + "</td>";
            htmlRow += "<td><input type='text' id='tblQuantity" + i + "' value=" +getAmountWithDecimal( ProductDetail[i].RequiredQuantity) + " onchange='calculatePrice(" + i + ",0);return false;' style='color:#4D4C4C;width:50px;' ></input></td>";

            htmlRow += "<td><input type='text' id='tblUnitPrice" + i + "' onchange='calculatePrice(" + i + ",0);return false;' value=" + getAmountWithDecimal(ProductDetail[i].UnitPrice) + " style='color:#4D4C4C;width:50px;' ></input></td>";
            htmlRow += "<td><label id='tblKwdPrice" + i + "'>" + getAmountWithDecimal(ProductDetail[i].GrossPrice) + "</label></td>";
            htmlRow += "<td><label id='tblGross" + i + "'>" + getAmountWithDecimal(ProductDetail[i].GrossPrice) + "</label></td>";
            htmlRow += "<td><input type='text' id='tblDiscount" + i + "' onchange='calculatePrice(" + i + ",0);return false;' Value=" + getAmountWithDecimal(ProductDetail[i].Discount) + " style='color:#4D4C4C;width:50px;' ></input></td>";
            htmlRow += "<td><input type='text' id='tblDiscountValue" + i + "' onchange='calculatePrice(" + i + ",1);return false;' Value=" + getAmountWithDecimal(ProductDetail[i].DiscountValue) + " style='color:#4D4C4C;width:50px;' ></input></td>";
            htmlRow = htmlRow + "<td><input type='text' id='tblTaxPercentage" + i + "' onchange='calculateDetail(" + i + ",0);return false;' Value=" + ProductDetail[i].TaxP + " style='color:#4D4C4C;width:50px;' readonly>&nbsp;</input><i class='fa fa-plus' onclick='Show_Modal_GST(" + ProductDetail[i].hdnNewProductId + "); return false;' style='font-size:24px;color:blue'></i></td>";
            htmlRow = htmlRow + "<td><input type='text' id='tblTaxValue" + i + "' onchange='calculateDetail(" + i + ",0);return false;' Value=" + ProductDetail[i].TaxV + " style='color:#4D4C4C;width:50px;' readonly></input></td>";
            htmlRow += "<td><label id='tblTotalAmount" + i + "'>" + ProductDetail[i].TotalAmount + "</label></td>";

            if (StockQTy.d.length === 0) {
                htmlRow += "<td><a href='javascript:void(0)' onClick='lnkStockInfo_Command(\"" + ProductDetail[i].hdnNewProductId + "\")' >0.000</a></td>"
            } else {
                htmlRow += "<td><a href='javascript:void(0)' onClick='lnkStockInfo_Command(\"" + ProductDetail[i].hdnNewProductId + "\")' >" + StockQTy.d[0] + "</a></td>"
            }
            htmlRow += "<td><input type='text' id=tblAgentCommission" + i + " onchange='CommisionChange(" + i + "); return false;' value=" + getAmountWithDecimal(ProductDetail[i].txtgvAgentCommission) + "><td/>"
            htmlRow += "</tr>";
            $('#tblProduct > tbody:last-child').append(htmlRow);
            calculatePrice(i, 0);
          }
          GetTaxAllowOrnot();
        $("#prgBar").css("display", "none");
        var DiscountP = parseFloat(DiscountValue) * 100 / parseFloat(GrossAmount);
        $('#<%=txtDiscountV.ClientID%>').val(getAmountWithDecimal(DiscountValue));
          $('#<%=txtDiscountP.ClientID%>').val(DiscountP);
          GrossCalculate();
      } else {
          Product_Tblbind();
          $("#prgBar").css("display", "none");
      }

  }


        //This Comman function created by Rahul Sharma for calculate Product Detail and also update the calculated values in Global variable
        function calculatePrice(row,type) {
            
             const quantity = parseFloat(document.getElementById('tblQuantity' + row + '').value) || 0;
             const unitPrice = parseFloat(document.getElementById('tblUnitPrice' + row + '').value) || 0;
             const discountPercentage = parseFloat(document.getElementById('tblDiscount' + row + '').value) || 0;
             const discountValue = parseFloat(document.getElementById('tblDiscountValue' + row + '').value) || 0;
             const TaxPer = parseFloat(document.getElementById('tblTaxPercentage' + row + '').value) || 0;
             const TaxValue = parseFloat(document.getElementById('tblTaxValue' + row + '').value) || 0;
            const grossPrice = quantity * unitPrice;
            let calculatedDiscountPercentage = 0;
             var strDecimalCount = $('#<%=hdnDecimalCount.ClientID%>').val();      
          decimalCount = parseInt(strDecimalCount);
            //Note:If type is equals to 1 then it comes from Discount value otherwise its come from others
            if (type == 1) {
                calculatedDiscountValue = discountValue;
                // Calculate discount percentage based on the discount value
                calculatedDiscountPercentage = (discountValue / grossPrice) * 100;
                document.getElementById('tblDiscount' + row + '').value = calculatedDiscountPercentage.toFixed(decimalCount);
                ProductDetail[row].Discount = getAmountWithDecimal(calculatedDiscountPercentage);
                const netPrice = grossPrice - calculatedDiscountValue;
                document.getElementById('tblGross' + row + '').innerText = grossPrice.toFixed(decimalCount);
                //document.getElementById('tblKwdPrice1' + row + '').innerText = grossPrice.toFixed(2);
                document.getElementById('tblKwdPrice' + row).innerText = grossPrice.toFixed(decimalCount);
                document.getElementById('tblDiscountValue' + row + '').value = calculatedDiscountValue.toFixed(decimalCount);
                document.getElementById('tblTotalAmount' + row).innerText = netPrice.toFixed(decimalCount);
                ProductDetail[row].TotalAmount = getAmountWithDecimal(netPrice);
                ProductDetail[row].DiscountValue = getAmountWithDecimal(calculatedDiscountValue);
                ProductDetail[row].UnitPrice = getAmountWithDecimal(unitPrice);
                ProductDetail[row].GrossPrice = getAmountWithDecimal(grossPrice);
                ProductDetail[row].RequiredQuantity = getAmountWithDecimal(quantity);
                ProductDetail[row].TotalAmount = getAmountWithDecimal(netPrice);
                ProductDetail[row].Discount = getAmountWithDecimal(calculatedDiscountPercentage);

            }
            else
            {
                const calculatedDiscountValue = (grossPrice * discountPercentage) / 100;
                //const netPrice = grossPrice - calculatedDiscountValue;
                const calculatedTaxValue = ((grossPrice - calculatedDiscountValue) * TaxPer) / 100;
                document.getElementById('tblTaxValue' + row + '').value = calculatedTaxValue.toFixed(decimalCount);
                const TotalNetPrice=grossPrice - calculatedDiscountValue
                const netPrice = TotalNetPrice + calculatedTaxValue;
                document.getElementById('tblGross' + row + '').innerText = grossPrice.toFixed(decimalCount);
                //document.getElementById('tblKwdPrice1' + row + '').innerText = grossPrice.toFixed(2);
                document.getElementById('tblKwdPrice' + row).innerText = grossPrice.toFixed(decimalCount);
                document.getElementById('tblDiscountValue' + row + '').value = calculatedDiscountValue.toFixed(decimalCount);
                document.getElementById('tblTotalAmount' + row).innerText = netPrice.toFixed(decimalCount);
                ProductDetail[row].TaxVal = calculatedTaxValue.toFixed(decimalCount);
                ProductDetail[row].TotalAmount = getAmountWithDecimal(netPrice);
                ProductDetail[row].DiscountValue = getAmountWithDecimal(calculatedDiscountValue);
                ProductDetail[row].UnitPrice = getAmountWithDecimal(unitPrice);
                ProductDetail[row].GrossPrice = getAmountWithDecimal(grossPrice);
                ProductDetail[row].RequiredQuantity = getAmountWithDecimal(quantity);
                ProductDetail[row].TotalAmount = getAmountWithDecimal(netPrice);
                ProductDetail[row].Discount = getAmountWithDecimal(discountPercentage);
            }
            GrossCalculate();
           
        }
        //this comman function is created by Rahul sharma its calculate all total value and discount value and percentage 
        function GrossCalculate()        
        {
            var grossAmount = 0;
            var NetAmount = 0;
            var DiscountValue = 0;
            var DiscountPercentage = 0;
            var TaxPer = $('#<%= txtTaxP.ClientID %>').val();
            var TaxValue = 0;
            $('#tblProduct tbody tr').each(function () {
                var row = $(this);
                grossAmount += parseFloat($('#tblGross' + row.index()).text()) || 0;
                NetAmount += parseFloat($('#tblTotalAmount' + row.index()).text()) || 0;
                DiscountValue += parseFloat($('#tblDiscountValue' + row.index()).val()) || 0;
                TaxValue += parseFloat($('#tblTaxValue' + row.index()).val()) || 0;
            });
            $('#<%= txtAmount.ClientID %>').val(getAmountWithDecimal(grossAmount));
            $('#<%= txtTotalAmount.ClientID %>').val(getAmountWithDecimal(NetAmount));
            if (TaxPer == "" || TaxPer == null) {
                TaxPer = (TaxValue / grossAmount) * 100;
            }
            //DiscountValue = grossAmount - NetAmount;
            DiscountPercentage = ((DiscountValue) / grossAmount) * 100;
            $('#<%=txtTaxP.ClientID%>').val(getAmountWithDecimal(TaxPer));
            $('#<%=txtTaxV.ClientID%>').val(getAmountWithDecimal(TaxValue));
            $('#<%= txtDiscountP.ClientID %>').val(getAmountWithDecimal(DiscountPercentage));
            $('#<%= txtDiscountV.ClientID %>').val(getAmountWithDecimal(DiscountValue));
        }


    function ProModelopen(row) {
        
        var description = $('#txtDescription' + row + '').val();
        //console.log(description);
        $('#pDescription').text(description);
        $('#exampleModal').modal({
            show: 'true'
        });
    }
    function getReportDataWithLoc(transId, locId) {
        

        document.getElementById('<%= reportSystem.FindControl("hdnTransId").ClientID %>').value = transId;
      document.getElementById('<%= reportSystem.FindControl("hdnLocId").ClientID %>').value = locId;
        setReportData();
    }
    function Modal_Open_FileUpload() {
        document.getElementById('<%= Btn_Modal_FileUpload.ClientID %>').click();
    }

    function findPositionWithScrolling(oElement) {
        if (typeof (oElement.offsetParent) != 'undefined') {
            var originalElement = oElement;
            for (var posX = 0,
            posY = 0; oElement; oElement = oElement.offsetParent) {
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
            } else {
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

    function LI_Inquiry_New_Active() {
        $("#Li_Inquiry").removeClass("active");
        $("#Inquiry").removeClass("active");

        $("#Li_New").addClass("active");
        $("#New").addClass("active");
    }

    function LI_List_Active() {
        $("#Li_List").addClass("active");
        $("#List").addClass("active");

        $("#Li_New").removeClass("active");
        $("#New").removeClass("active");
    }    
    function Modal_Address_Open() {
        document.getElementById('<%= Btn_Address_Modal.ClientID %>').click();
    }
    function Modal_NewAddress_Open() {
        document.getElementById('<%= Btn_NewAddress.ClientID %>').click();
    }

    function LI_List_Active1() { }

    function lblgvSQNo_Command(quotationId, locationid) {
        PageMethods.lblgvSQNo_Command(quotationId, locationid,
        function (data) {
            
            if (data[0] == "false") {
                alert(data[1]);
                return;
            } else {
                window.open('../Sales/SalesOrderView.aspx?OrderId=' + data[1] + '', 'window', 'width=1024, ');
            }
        },
        errorMessage);

    }
    function errorMessage(err) {
        alert(err);
    }
    function txtShipCustomerName_TextChanged(ctrl) {
        var ct = ctrl.value;
        var data = getCustomerAddressName(ctrl);

        
        var shippingAddress = document.getElementById('<%=txtShipingAddress.ClientID%>');
      if (data[0] == "true") {
          if (data[1] != "") {
              shippingAddress.value = data[1];
              GetShipToAddress(ct);
          }

      } else {
          shippingAddress.value = "";
          ctrl.value = "";
          alert("Select from suggestions only");
          return;
      }
        // 
  } <%--< %--
    function GetShipToAddress(ctrl) {
      
      var ShipToAddress = ctrl;
      var ShipTo = ShipToAddress.split('/', 1);
      //var splitArr = $("< %txtShipCustomerName.ClientID%>").value();
      /* console.log("hi");*/
      //console.log(ShipTo[0]);
      $.ajax({
        url: 'SalesQuotation.aspx/ShipTo_TextChanged',
        method: 'post',
        async: false,
        contentType: "application/json; charset=utf-8",
        data: "{'ShipTo':'" + ShipTo[0] + "'}",
        success: function(data) {
          
          console.log(data.d[0]);
          //$('#ShipAddress').text(data[0]);
          $('#<%= ShipAddress.ClientID %>').text(data.d[0]);

        }
      });
    }--%>
        function ShipToAddressDetail() {

            if ($('#<%= txtShipCustomerName.ClientID %>').val() == "") {
            $('#<%= ShipAddress.ClientID %>').text('');
      } else {
          $.ajax({
              url: 'SalesQuatationJScript.aspx/GetShipToAddress',
              method: 'post',
              contentType: "application/json; charset=utf-8",
              data: "{'ShipToAddress':'" + $('#<%= txtShipCustomerName.ClientID %>').val() + "'}",
          async: true,
          success: function (data) {
              if (data.d != 0) {
                  $('#<%= ShipAddress.ClientID %>').text(data.d);
            }

          },
            error: function (ex) {

            }
          });
             setTimeout(function () { jQuery('#<%=txtShipingAddress.ClientID%>').focus() }, 500);
}
}
function ShipingAddressDetail() {

    if ($('#<%= txtShipingAddress.ClientID %>').val() == "") {
            $('#<%= ShippingAddress.ClientID %>').text('');
      } else {
          $.ajax({
              url: 'SalesQuatationJScript.aspx/GetShippingAddress',
              method: 'post',
              contentType: "application/json; charset=utf-8",
              data: "{'ShippingAddress':'" + $('#<%= txtShipingAddress.ClientID %>').val() + "' }",
          async: true,
          success: function (data) {
              if (data.d != 0) {
                  $('#<%= ShippingAddress.ClientID %>').text(data.d);
                     setTimeout(function () { jQuery('#<%=txtReason.ClientID%>').focus() }, 500);
            }

          },
            error: function (ex) {

            }
        });
}
}

function txtProductCode_TextChanged(ctrl) {
    $.ajax({
        url: 'SalesQuatationJScript.aspx/txtProduct_TextChanged',
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
              //document.getElementById('<%=txtPDesc.ClientID%>').value = dd[3];
              var replacedData1 = dd[3].replaceAll('<', '');
              var replacedData2 = replacedData1.replaceAll('>', '');
              document.getElementById('<%=txtPDesc.ClientID%>').value = replacedData2;
          
            document.getElementById('<%=hdnNewProductId.ClientID%>').value = dd[4];
              if (parseInt(dd[5]) != 0) {
                  document.getElementById('<%=btnFillRelatedProducts.ClientID%>').click();
                  $('#<%=GvRelatedProduct.ClientID%>').show(); 
              }

               
               
            }

        },
        complete: function (data) {
            //alert('hi');
            setTimeout(function () { jQuery('#<%=txtRequiredQty.ClientID%>').focus() }, 500);
           // document.getElementById('< %=txtRequiredQty.ClientID%>').focus();
        }
    });
     
}

$(document).ready(function () {
    isDiscountApplicable();
    isTaxApplicable();
});

function isDiscountApplicable() {
    $.ajax({
        url: 'SalesQuotation.aspx/isDiscountApplicable',
        method: 'post',
        async: false,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            document.getElementById('<%=hdnIsDiscountApplicable.ClientID%>').value = data.d;
        }
      });
    }
    function isTaxApplicable() {
        $.ajax({
            url: 'SalesQuotation.aspx/isTaxApplicable',
            method: 'post',
            async: false,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                document.getElementById('<%=hdnIsTaxApplicable.ClientID%>').value = data.d;
        }
      });
    }

    function HeadearCalculateGrid() {
        var F_Gross_Total = 0;
        var F_Discount_Per = 0;
        var F_Discount_Value = 0;
        var F_Tax_Per = 0;
        var F_Tax_Value = 0;
        var F_Net_Total = 0;
        var Gross_Unit_Price = 0;
        var Gross_Discount_Amount = 0;
        var Gross_Tax_Amount = 0;
        var Gross_Line_Total = 0;
        var decimalCount = document.getElementById('<%=hdnDecimalCount.ClientID%>').value;
      var isDiscountApplicable = document.getElementById('<%=hdnIsDiscountApplicable.ClientID%>').value;
        var isTaxApplicable = document.getElementById('<%=hdnIsTaxApplicable.ClientID%>').value;

        var txtAmount = document.getElementById('<%=txtAmount.ClientID%>');
        var txtDiscountV = document.getElementById('<%=txtDiscountV.ClientID%>');
        var txtTaxV = document.getElementById('<%=txtTaxV.ClientID%>');
        var txtPriceAfterTax = document.getElementById('<%=txtPriceAfterTax.ClientID%>');
        var txtTotalAmount = document.getElementById('<%=txtTotalAmount.ClientID%>');
        var txtDiscountP = document.getElementById('<%=txtDiscountP.ClientID%>');
        var txtTaxP = document.getElementById('<%=txtTaxP.ClientID%>');

        var count = 0;
        var tblProduct = $('#tblDetail tbody tr').each(function () {

            if (count % 2 == 0) {
                var row = $(this);
                
                if (row[0].className != "Invgridheader") {
                    var lblgvSerialNo = row[0].cells[2].innerText;
                    var Product_Id = row[0].cells[4].children[0].childNodes[1].childNodes[0].cells[0].childNodes[1].value;
                    var Quantity = row[0].cells[7].childNodes[1].value;
                    var Unit_Price = row[0].cells[9].childNodes[1].value;
                    var Total_Quantity_Price = row[0].cells[10].childNodes[1];
                    var Discount_Percentage = row[0].cells[11].childNodes[1];
                    var Discount_Amount = row[0].cells[12].childNodes[1];
                    var Line_Total = 0;

                    if (isTaxApplicable == "true") {
                        var Tax_Percentage = row[0].cells[13].childNodes[1];
                        var Tax_Amount = row[0].cells[14].childNodes[1];
                        Line_Total = row[0].cells[15].childNodes[1];
                    } else {
                        Line_Total = row[0].cells[13].childNodes[1];
                    }

                    if (Unit_Price.value == "") Unit_Price.value = "0";

                    if (Quantity.value == "") Quantity.value = "0";

                    if (Discount_Percentage.value == "") Discount_Percentage.value = "0";

                    var F_Unit_Price = Unit_Price;
                    var F_Order_Quantity = Quantity;
                    var F_Discount_Percentage = Discount_Percentage.value;
                    var F_Tax_Percentage = 0;
                    var F_Tax_Amount = 0;

                    if (isDiscountApplicable == "true") {
                        var F_Discount_Amount = Get_Discount_Amount(F_Unit_Price, F_Discount_Percentage);
                    }
                    if (isTaxApplicable == "true") {
                        Add_Tax_To_Session((F_Unit_Price - F_Discount_Amount), Product_Id.Value, lblgvSerialNo.value); //ajax
                        var F_Tax_Percentage = getTaxPercentage(Product_Id.Value, lblgvSerialNo.value); //ajax
                        var F_Tax_Amount = GetAmountWithDecimal(getTaxAmount((F_Unit_Price - F_Discount_Amount).toString(), Product_Id.Value, lblgvSerialNo.value), decimalCount); //ajax
                        Tax_Percentage.value = GetAmountWithDecimal(F_Tax_Percentage, decimalCount);
                        Tax_Amount.value = GetAmountWithDecimal(F_Tax_Amount, decimalCount);
                    }
                    var F_Total_Amount = (F_Unit_Price - F_Discount_Amount) + F_Tax_Amount;
                    var F_Row_Total_Amount = F_Total_Amount * F_Order_Quantity;
                    Discount_Percentage.value = GetAmountWithDecimal(F_Discount_Percentage, decimalCount);
                    Discount_Amount.value = GetAmountWithDecimal(F_Discount_Amount, decimalCount);
                    Line_Total.value = GetAmountWithDecimal(F_Row_Total_Amount, decimalCount);
                    Total_Quantity_Price.value = (F_Unit_Price * F_Order_Quantity);
                    Gross_Unit_Price = Gross_Unit_Price + (F_Unit_Price * F_Order_Quantity);
                    Gross_Discount_Amount = Gross_Discount_Amount + (F_Discount_Amount * F_Order_Quantity);
                    Gross_Tax_Amount = Gross_Tax_Amount + (F_Tax_Amount * F_Order_Quantity);
                    Gross_Line_Total = Gross_Line_Total + F_Row_Total_Amount;
                }

            }
            count++;
        });
        txtAmount.value = GetAmountWithDecimal(Gross_Unit_Price, decimalCount);
        txtDiscountV.value = GetAmountWithDecimal(Gross_Discount_Amount, decimalCount);
        txtTotalAmount.value = GetAmountWithDecimal(Gross_Line_Total, decimalCount);
        txtDiscountP.value = GetAmountWithDecimal(Get_Discount_Percentage(Gross_Unit_Price, Gross_Discount_Amount), decimalCount);
        if (txtTaxP != null) {
            txtPriceAfterTax.value = GetAmountWithDecimal(Gross_Line_Total, decimalCount);
            txtTaxP.value = GetAmountWithDecimal(Get_Total_Tax_Percentage((Gross_Unit_Price - Gross_Discount_Amount), Gross_Tax_Amount), decimalCount);
            txtTaxV.value = GetAmountWithDecimal(Gross_Tax_Amount, decimalCount);
        }
    }

    function Get_Discount_Amount(Unit_Price, Discount_Percent) {
        try {
            var Discount_Amount = 0;
            var D_Unit_Price = parseFloat(Unit_Price);
            var D_Discount_Percent = parseFloat(Discount_Percent);
            Discount_Amount = (D_Unit_Price * D_Discount_Percent) / 100;
            return Discount_Amount;
        } catch (err) {
            return 0;
        }
    }
    function Get_Total_Tax_Percentage(Unit_Price, Tax_Amount) {
        try {
            var Tax_Percent = 0;
            var D_Unit_Price = parseFloat(Unit_Price);
            var D_Tax_Amount = parseFloat(Tax_Amount);
            Tax_Percent = (D_Tax_Amount / D_Unit_Price) * 100;
            return Tax_Percent;
        } catch (err) {
            return 0;
        }
    }
    function Get_Discount_Percentage(Unit_Price, Discount_Amount) {
        try {
            var Discount_Percent = 0;
            var D_Unit_Price = Convert.ToDouble(Unit_Price);
            var D_Discount_Amount = Convert.ToDouble(Discount_Amount);
            Discount_Percent = (D_Discount_Amount / D_Unit_Price) * 100;
            return Discount_Percent;
        } catch (er) {
            return 0;
        }
    }


<%--    function setScrollAndRow() {
      try {
        
        var rowIndex = $('#<%= hdfCurrentRow.ClientID %>').val();
        var parent = document.getElementById(tblDetail);
        var rowIndex = parseInt(rowIndex);
        parent.rows[rowIndex + 1].style.backgroundColor = "#A1DCF2";
        var h = document.getElementById("<%=hfScrollPosition.ClientID%>");
        //document.getElementById("< %= s crollArea.ClientID%>").scrollTop = h.value;

      } catch(e) {

}
    }--%>

        function SetDivPosition() {
            // var intY = document.getElementById("< %=s crollArea.ClientID%>").scrollTop;
            var h = document.getElementById("<%=hfScrollPosition.ClientID%>");
        // h.value = intY;
    }

    function SetSelectedRow(lnk) {
        //Reference the GridView Row.
        var row = lnk.parentNode.parentNode;
        $('#<%= hdfCurrentRow.ClientID %>').val(row.rowIndex - 1);
      row.style.backgroundColor = "#A1DCF2";
  }

  function li_tab_bin() {
      document.getElementById('<%=btn_bin.ClientID%>').click();
    }
    function lnkStockInfo_Command(productId) {
        var CustomerName = "";
        CustomerName = document.getElementById('<%=txtCustomer.ClientID%>').value;
      window.open('../Inventory/Magic_Stock_Analysis.aspx?ProductId=' + productId + '&&Type=SALES&&Contact=' + CustomerName + '');
  }
  function lnkcustomerHistory_OnClick() {
      var CustomerId = "";
      var txtCustomer = document.getElementById('<%=txtCustomer.ClientID%>');
      if (txtCustomer.value != "") {
          try {
              CustomerId = getContactIDFromName(txtCustomer.value);
          } catch (er) {
              CustomerId = "0";
          }
      } else {
          CustomerId = "0";
      }
      window.open('../Purchase/CustomerHistory.aspx?ContactId=' + CustomerId + '&&Page=SQ', 'window', 'width=1024, ');
  }
  function redirectToHome() {
      if (confirm('Tax is not enabled on this location do you want to continue ?')) {
          window.open('../MasterSetup/Home.aspx', 'window', 'width=1024,');
          return true;
      } else {
          return false;
      }

  }
  function redirectToHome(msg) {
      if (confirm(msg)) {
          window.open('../MasterSetup/Home.aspx', 'window', 'width=1024,');
          return true;
      } else {
          return false;
      }
  }
  function Modal_CustomerInfo_Open() {
      document.getElementById('<%= Btn_CustomerInfo_Modal.ClientID %>').click();
    }

   function getAmountWithDecimal(amount) {
          
        let Amt = 0;
        let decimalCount = 0;
        let convertedAmt = 0;
        let DotValue = "";
        var strDecimalCount = $('#<%=hdnDecimalCount.ClientID%>').val();
          //Amt = parseFloat(amount);
          decimalCount = parseInt(strDecimalCount);
        
          return parseFloat(amount).toFixed(decimalCount);


    }


function GetGeoLocation() {
    if ($('#<%= txtShipingAddress.ClientID %>').val() == "") {
        alert("Please Add Shipping Address");
    } else {
        $.ajax({
            url: 'SalesQuatationJScript.aspx/GetGeoLocation',
            method: 'post',
            contentType: 'application/json',
            data: JSON.stringify({ ShippingAddress: $('#<%= txtShipingAddress.ClientID %>').val() }),
            success: function (data) {
                
                if (data.d) {
                    // Process the data received from the server here
                    var linkToOpen = "https://www.google.com/maps/search/?api=1&query=" + data.d[1] + "," + data.d[0];
                    // Open the link in a new window or tab
                    window.open(linkToOpen, '_blank');
                } else {
                    // Handle the case when data.d is 0 or false
                }
            },
            error: function (ex) {
                console.error("AJAX error: " + ex.responseText);
            }
        });
    }
}


    </script>
    <script src="../Script/customer.js">
    </script>
    <script src="../Script/employee.js">
    </script>
</asp:Content>

