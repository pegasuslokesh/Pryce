<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="SalesQuotation.aspx.cs" Inherits="Sales_SalesQuotation" %>

<%@ Register Src="~/WebUserControl/ReportSystem.ascx" TagName="ReportSystem" TagPrefix="RS" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/Followup.ascx" TagName="AddFollowup" TagPrefix="AT1" %>
<%@ Register Src="~/WebUserControl/AddressControl.ascx" TagName="AddAddress" TagPrefix="AA1" %>
<%@ Register Src="~/WebUserControl/ContactInfo.ascx" TagName="ViewContact" TagPrefix="AT1" %>
<%@ Register Src="~/WebUserControl/FileUpload.ascx" TagPrefix="AT1" TagName="FileUpload1" %>
<%@ Register Src="~/WebUserControl/ucControlsSetting.ascx" TagName="ucCtlSetting" TagPrefix="UC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <script src="../Script/common.js"></script>
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
        .CenterClass{
            text-align:center;
            color:#3c8dbc;
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
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlPosted_SelectedIndexChanged">
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
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Quotation No. %>" Value="SQuotation_No" ></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Quotation Date %>" Value="Quotation_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Opportunity No. %>" Value="InquiryNo"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Order Close Date %>" Value="OrderCompletionDate"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Employee Name %>" Value="EmployeeName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Customer Name %>" Value="Customer_Name" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Reason %>" Value="Reason" ></asp:ListItem>
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
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbindrpt_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefreshReport_Click" ToolTip="<%$ Resources:Attendance,Refresh %>">                                                <span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <asp:HiddenField ID="hdnFollowupTableName" runat="server" Value="Inv_SalesQuotationHeader" />
                                <asp:HiddenField ID="hdnIsDiscountApplicable" runat="server" />
                                <asp:HiddenField ID="hdnIsTaxApplicable" runat="server" />

                                <div class="box box-warning box-solid" <%= GvSalesQuote.Rows.Count>0?"style='display:block'":"style='display:none'"%>>

                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:HiddenField ID="hdnGvSalesQuotationCurrentPageIndex" runat="server" Value="1" />
                                                    <asp:HiddenField ID="hdnGvSalesQuotationCurrentPageIndexBin" runat="server" Value="1" />
                                                    <asp:HiddenField ID="hdnSalesInquiryId" runat="server" Value="0" />
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvSalesQuote" PageSize="<%# PageControlCommon.GetPageSize() %>" runat="server" CurrentSortField="SQuotation_Id" CurrentSortDirection="DESC"
                                                        AutoGenerateColumns="False" Width="100%" AllowPaging="false"
                                                        AllowSorting="True" OnSorting="GvSalesQuote_Sorting">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnPrint" runat="server" CausesValidation="False" CommandArgument='<%# Eval("SQuotation_Id") %>' OnCommand="IbtnPrint_Command"><i class="fa fa-print"></i>Print</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <a onclick="getReportDataWithLoc('<%# Eval("SQuotation_Id") %>','<%# Eval("Location_Id") %>')"><i class="fa fa-print" style="cursor: pointer"></i>Report System</a>
                                                                            </li>

                                                                            <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("SQuotation_Id") %>' CommandName='<%# Eval("Location_Id") %>' OnCommand="lnkViewDetail_Command" CausesValidation="False"><i class="fa fa-eye"></i>View</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("SQuotation_Id") %>' CommandName='<%# Eval("Location_Id") %>' OnCommand="btnEdit_Command" CausesValidation="False"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("SQuotation_Id") %>' CommandName='<%# Eval("Location_Id") %>' OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>


                                                                            <li <%= hdnCanUpload.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnFileUpload" runat="server" CommandArgument='<%# Eval("SQuotation_Id") %>' CommandName='<%# Eval("SQuotation_No") %>' OnCommand="btnFileUpload_Command" CausesValidation="False"><i class="fa fa-upload"></i>File Upload</asp:LinkButton>
                                                                            </li>


                                                                            <li>
                                                                                <asp:LinkButton ID="IBtnFollowup" runat="server" CausesValidation="False" CommandArgument='<%# Eval("SQuotation_Id") %>' CommandName='<%# Eval("Location_Id") %>' OnCommand="IBtnFollowup_Command"><i class="fa fa-phone"></i>Followup</asp:LinkButton>
                                                                            </li>
                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quotation No. %>" SortExpression="SQuotation_No">
                                                                <ItemTemplate>
                                                                    <a onclick="lblgvSQNo_Command('<%# Eval("SQuotation_Id") %>','<%# Eval("Location_Id") %>')" style="cursor: pointer">
                                                                        <%#Eval("SQuotation_No") %>
                                                                    </a>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Name%>" SortExpression="Customer_Name">
                                                                <ItemTemplate>
                                                                    <%# Eval("Customer_Name") %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quotation Date %>" SortExpression="Quotation_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvSQDate" runat="server" Text='<%#GetDate(Eval("Quotation_Date").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Opportunity No. %>" SortExpression="InquiryNo">
                                                                <ItemTemplate>
                                                                    <%#Eval("InquiryNo") %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Reason %>" HeaderStyle-CssClass="CenterClass" SortExpression="InquiryNo">      
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="lblReason" Text="Reason" runat="server"></asp:Label>
                                                                    <asp:CheckBox ID="chkList" runat="server" OnCheckedChanged="chkShortReasonName_CheckedChanged" AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGvFullName"  runat="server" Text='<%#Eval("Reason").ToString() %>'  Visible="false" ></asp:Label>
                                                                   <asp:Label ID="lblGVShortName"  runat="server" Text='<%#getShortReasonName(Eval("Reason").ToString()) %>' ></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="false" HeaderText="<%$ Resources:Attendance,Order Close Date %>" SortExpression="OrderCompletionDate">
                                                                <ItemTemplate>
                                                                    <%#GetDate(Eval("OrderCompletionDate").ToString()) %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name%>" SortExpression="EmployeeName">
                                                                <ItemTemplate>
                                                                    <%# Eval("EmployeeName").ToString() %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>  
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Amount %>" SortExpression="TotalAmount">
                                                                <ItemTemplate>
                                                                    <%#GetAmountDecimal(Eval("TotalAmount").ToString()) %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                        </Columns>

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
                                <asp:AsyncPostBackTrigger ControlID="GvSalesQuote" />
                                <asp:AsyncPostBackTrigger ControlID="GvDetail" />
                                <asp:AsyncPostBackTrigger ControlID="btnAddTax" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="gvTaxCalculation" />
                                <asp:PostBackTrigger ControlID="btnDownload" />
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
                                                            <asp:TextBox ID="txtCustomer" runat="server" class="form-control"
                                                                placeholder="Customer Name (*)" BackColor="#eeeeee" ReadOnly="true"
                                                                onchange="txtCustomer_TextChanged(this)" />

                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCustomer" ErrorMessage="Enter Customer Name"></asp:RequiredFieldValidator>

                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server"
                                                                DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                                MinimumPrefixLength="2" ServiceMethod="GetCompletionListCustomer" ServicePath=""
                                                                TargetControlID="txtCustomer" UseContextKey="True" CompletionListCssClass="autoCompleteList"
                                                                CompletionListItemCssClass="autoCompleteListItem"
                                                                CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>


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
                                                            <asp:TextBox ID="txtContactName" runat="server" class="form-control" AutoPostBack="true" OnTextChanged="txtContactName_TextChanged" />
                                                            <cc1:AutoCompleteExtender ID="txtContactList_AutoCompleteExtender" runat="server"
                                                                DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                                MinimumPrefixLength="0" ServiceMethod="GetContactListCustomer" ServicePath=""
                                                                TargetControlID="txtContactName" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                            <div class="input-group-btn">
                                                                <asp:LinkButton ID="btnAddCustomer" runat="server" ToolTip="Add New Customer" OnClick="btnAddCustomer_Click" AlternateText="<%$ Resources:Attendance,Add %>" CausesValidation="False"><i class="fa fa-plus-square" style="font-size:35px;padding:5px;margin-top: -5px;"></i></asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Contact No%>" />
                                                        <asp:TextBox ID="txtContactNo" runat="server" class="form-control" ReadOnly="True" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-5">
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Ship To %>" />
                                                        <asp:TextBox ID="txtShipCustomerName" runat="server" class="form-control" BackColor="#eeeeee" onchange="txtShipCustomerName_TextChanged(this)" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                            Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                            ServiceMethod="GetCompletionListContact" ServicePath="" TargetControlID="txtShipCustomerName"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <td colspan="6"></td>
                                                    <div class="col-md-5">
                                                        <asp:Label ID="lblShipingAddress" runat="server" Text="<%$ Resources:Attendance,Shipping Address %>" />
                                                        <asp:TextBox ID="txtShipingAddress" runat="server" class="form-control" BackColor="#eeeeee"
                                                            AutoPostBack="true" OnTextChanged="txtShipingAddress_TextChanged" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListAddressName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtShipingAddress"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-2">
                                                        <br />
                                                        <asp:Button ID="btnNewaddress" runat="server" Text="New Address" class="btn btn btn-primary" OnClick="btnNewaddress_Click" />
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
                                                        <asp:TextBox ID="txtOrderCompletionDate" runat="server" class="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" TargetControlID="txtOrderCompletionDate" />
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator6" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtOrderCompletionDate" ErrorMessage="Enter Order Close Date"></asp:RequiredFieldValidator>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblCurrency" runat="server" Text="<%$ Resources:Attendance,Currency %>" />
                                                        <asp:DropDownList ID="ddlCurrency" runat="server" class="form-control" Enabled="true"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlCurrency_OnSelectedIndexChanged" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblEmployee" runat="server" Text="<%$ Resources:Attendance,Employee%>" />
                                                        <asp:TextBox ID="txtEmployee" runat="server" class="form-control" />
                                                        <cc1:AutoCompleteExtender ID="txtReceivedEmp_AutoCompleteExtender" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="1" ServiceMethod="GetCompletionListEmployeeName" ServicePath=""
                                                            TargetControlID="txtEmployee" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEmployee" ErrorMessage="Enter Employee Name"></asp:RequiredFieldValidator>
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
                                                        <asp:DropDownList ID="ddlTransType" AutoPostBack="true" OnSelectedIndexChanged="ddlTransType_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblReason" runat="server" Text="<%$ Resources:Attendance,Reason%>" />
                                                        <asp:TextBox ID="txtReason" runat="server" class="form-control" TextMode="MultiLine"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label14" runat="server" Text="Agent Name" />
                                                        <asp:TextBox ID="txtAgentName" runat="server" class="form-control" Enabled="false"
                                                            BackColor="#eeeeee" OnTextChanged="txtAgentName_TextChanged" AutoPostBack="true" />
                                                        <cc1:AutoCompleteExtender ID="txtCustomerName_AutoCompleteExtender" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="0" ServiceMethod="GetContactList" ServicePath="" TargetControlID="txtAgentName"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblTemplateName" runat="server" Text="<%$ Resources:Attendance,Template Name %>" />
                                                        <asp:TextBox ID="txtTemplateName" runat="server" class="form-control" BackColor="#eeeeee"
                                                            AutoPostBack="true" OnTextChanged="txtTemplateName_TextChanged" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                            Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                            ServiceMethod="GetCompletionListtemplateName" ServicePath="" TargetControlID="txtTemplateName"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:RadioButton ID="rbtnFormView" Font-Bold="true" Visible="true" runat="server"
                                                            Text="<%$ Resources:Attendance,Form View%>" AutoPostBack="true"
                                                            GroupName="Product" OnCheckedChanged="rbtnFormView_OnCheckedChanged" />
                                                        <asp:RadioButton ID="rbtnAdvancesearchView" Style="margin-left: 20px;" Font-Bold="true" Visible="false" runat="server"
                                                            Text="<%$ Resources:Attendance,Advance Search View%>"
                                                            AutoPostBack="true" GroupName="Product" OnCheckedChanged="rbtnFormView_OnCheckedChanged" />
                                                        <asp:RadioButton ID="rbtnUpload" Font-Bold="true" Visible="true" runat="server" Text="Upload Excel" AutoPostBack="true" GroupName="Product" OnCheckedChanged="rbtnFormView_OnCheckedChanged" />
                                                         <asp:RadioButton ID="rbtnUpdate" Font-Bold="true" Visible="true" runat="server" Text="Update Upload" AutoPostBack="true" GroupName="Product" OnCheckedChanged="rbtnFormView_OnCheckedChanged" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" id="Div_device_upload_operation" runat="server" visible="false">
                                                        <div class="col-md-12" style="text-align: center;">
                                                            <br />
                                                            <asp:HyperLink ID="uploadEmpInfo" runat="server" Font-Bold="true" Font-Size="15px"
                                                                NavigateUrl="~/CompanyResource/ProductUploadQta.xlsx" Text="Download sample format for update information" Font-Underline="true"></asp:HyperLink>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6" style="text-align: center;">
                                                            <br />
                                                            <asp:Label runat="server" Text="Browse Excel File" ID="Label169"></asp:Label>
                                                            <div class="input-group" style="width: 100%;">
                                                                <cc1:AsyncFileUpload ID="fileLoad" OnUploadedComplete="FileUploadComplete" OnClientUploadError="uploadError" OnClientUploadStarted="uploadStarted" OnClientUploadComplete="uploadComplete"
                                                                    runat="server" CssClass="form-control" CompleteBackColor="White" UploaderStyle="Traditional" UploadingBackColor="#CCFFFF" ThrobberID="imgLoaderUpload" Width="100%" />
                                                                <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                                    <asp:Image ID="Img_RightUpload1" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Allow.png" Style="display: none" />
                                                                    <asp:Image ID="Img_WrongUpload" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Delete1.png" Style="display: none" />
                                                                    <asp:Image ID="imgLoaderUpload" runat="server" ImageUrl="../Images/loader.gif" />
                                                                </div>
                                                            </div>
                                                            <br />
                                                            <asp:Button ID="btnGetSheet" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                                                                OnClick="btnGetSheet_Click" Visible="true" Text="<%$ Resources:Attendance,Connect To DataBase %>" />

                                                        </div>
                                                        <div class="col-md-6" style="text-align: center;">
                                                            <br />
                                                            <asp:Label runat="server" Text="Select Sheet" ID="Label170"></asp:Label>
                                                            <asp:DropDownList ID="ddlTables" CssClass="form-control" runat="server">
                                                            </asp:DropDownList>
                                                            <br />
                                                            <asp:Button ID="Button6" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                                                                OnClick="btnConnect_Click" Visible="true" Text="<%$ Resources:Attendance,Add Product %>" />
                                                            <br></br>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12" id="Update_Opration" runat="server" visible="false">
                                                        <div class="col-md-6" style="text-align: center;">
                                                            <br />
                                                            <asp:Label runat="server" Text="Browse Excel File" ID="Label12"></asp:Label>
                                                            <div class="input-group" style="width: 100%;">
                                                                <cc1:AsyncFileUpload ID="fileLoad1" OnUploadedComplete="FileUploadCompleteForUpdate" OnClientUploadError="uploadErrorUpdate" OnClientUploadStarted="uploadStartedUpdate" OnClientUploadComplete="uploadCompleteUpdate"
                                                                    runat="server" CssClass="form-control" CompleteBackColor="White" UploaderStyle="Traditional" UploadingBackColor="#CCFFFF" ThrobberID="imgLoaderUpload1" Width="100%" />
                                                                <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                                    <asp:Image ID="Img_RightUpload11" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Allow.png" Style="display: none" />
                                                                    <asp:Image ID="Img_WrongUpload1" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Delete1.png" Style="display: none" />
                                                                    <asp:Image ID="imgLoaderUpload1" runat="server" ImageUrl="../Images/loader.gif" />
                                                                </div>
                                                            </div>
                                                            <br />
                                                            <asp:Button ID="btnGetSheetForUpdate" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                                                                OnClick="btnGetSheetForUpdate_Click" Visible="true" Text="<%$ Resources:Attendance,Connect To DataBase %>" />

                                                        </div>
                                                        <div class="col-md-6" style="text-align: center;">
                                                            <br />
                                                            <asp:Label runat="server" Text="Select Sheet" ID="Label15"></asp:Label>
                                                            <asp:DropDownList ID="ddlTablesForUpdate" CssClass="form-control" runat="server">
                                                            </asp:DropDownList>
                                                            <br />
                                                            <asp:Button ID="btnUpdateConnect" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                                                                OnClick="btnConnectForUpdate_Click" Visible="true" Text="<%$ Resources:Attendance,Add Product %>" />
                                                            <br></br>
                                                            <br />
                                                        </div>
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
                                                        <div id="DivP1" runat="server" class="col-md-12">
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
                                                                            <asp:TextBox ID="txtProductcode" runat="server" class="form-control" onchange="txtProductCode_TextChanged(this)" BackColor="#eeeeee" />
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionInterval="100"
                                                                                DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductCode"
                                                                                ServicePath="" TargetControlID="txtProductcode" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-12" style="text-align: center">
                                                                            <asp:LinkButton ID="lnkProductBuilder" runat="server" Text="Product Builder" ForeColor="Blue"
                                                                                Style="margin-left: 15px" Font-Underline="true" OnClick="lnkProductBuilder_OnClick"></asp:LinkButton>

                                                                            <asp:LinkButton ID="lnkLabelBuilder" runat="server" Text="Label Builder" ForeColor="Blue"
                                                                                Style="margin-left: 15px" Font-Underline="true" OnClick="lnkLabelBuilder_OnClick"></asp:LinkButton>

                                                                            <asp:LinkButton ID="lbkGetProductList" runat="server" Text="Get Product List" ForeColor="Blue"
                                                                                Style="margin-left: 15px" Font-Underline="true" OnClick="lbkGetProductList_OnClick"></asp:LinkButton>

                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-12">
                                                                            <asp:Label ID="lblProductName" runat="server" Text="<%$ Resources:Attendance,Product Name %>" />
                                                                            <asp:TextBox ID="txtProductName" runat="server" class="form-control" onchange="txtProductCode_TextChanged(this)" BackColor="#eeeeee" />
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
                                                                            <asp:DropDownList ID="ddlUnit" runat="server" Class="form-control" />
                                                                            <asp:HiddenField ID="hdnUnitId" runat="server" Value="0" />
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblRequiredQty" runat="server" Text="<%$ Resources:Attendance,Required Quantity %>" />
                                                                            <asp:TextBox ID="txtRequiredQty" runat="server" class="form-control" /><a style="color: Red;">*</a>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                                TargetControlID="txtRequiredQty" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <br />
                                                                        </div>

                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblEstimatedUnitPrice" runat="server" Text="<%$ Resources:Attendance,Estimated Unit Price %>" />
                                                                            <asp:TextBox ID="txtEstimatedUnitPrice" runat="server" class="form-control" />
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
                                                                            <asp:TextBox ID="txtPDesc" runat="server" class="form-control" TextMode="MultiLine"></asp:TextBox>
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
                                                                                                        <asp:HiddenField ID="hdnUnitId" runat="server" Value='<%#Eval("UnitId") %>' />
                                                                                                        <asp:DropDownList ID="ddlunit" CssClass="form-control" runat="server">
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
                                                                                                        <asp:DropDownList ID="ddlCurrency" CssClass="form-control" runat="server">
                                                                                                        </asp:DropDownList>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="17%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Estimated Unit Price %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="txtEstimatedUnitPrice" runat="server" CssClass="form-control"></asp:TextBox>
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
                                                                                class="btn btn btn-primary" OnClick="btnProductSave_Click" />
                                                                            <asp:Button ID="btnProductCancel" runat="server" class="btn btn btn-primary" Text="<%$ Resources:Attendance,Cancel %>"
                                                                                CausesValidation="False" OnClick="btnProductCancel_Click" />
                                                                         </div>
                                                                        <div class="col-md-12" style="display:none">
                                                                            <asp:Button ID="btnClearGv" runat="server" OnClick="btnGvClear_Click"  />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12">
                                                            <div class="col-md-12" <%= GvDetail.Rows.Count>0?"style='display:block'":"style='display:none'"%> >
                                                                 <asp:Button ID="btnClearAddProduct" runat="server" OnClick="btnClearAddProduct_Click" CssClass="btn btn-danger" Text="Delete Product" />
                                                                    &nbsp;
                                                                    <asp:Button ID="btnDownload" runat="server" CssClass="btn btn-primary" Text="Download" OnClick="btnDownload_AddProduct" />                                                                     
                                                             <br />
                                                             </div>  
                                                             <br />
                                                            <asp:HiddenField ID="Hdn_Tax_By" runat="server" />

                                                            <div class="col-md-12" runat="server" id="scrollArea" onscroll="SetDivPosition()" style="overflow: auto; max-height: 500px;">
                                                                 <br />
                                                                <asp:UpdatePanel runat="server" ID="upProduct" UpdateMode="Conditional">
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="btnProductSave" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="GvSalesQuote" />
                                                                        <asp:AsyncPostBackTrigger ControlID="btnAddTax" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="gvTaxCalculation" />
                                                                    </Triggers>
                                                                    <ContentTemplate>
                                                                        <%if (Session["dtQuotationDetail"] == null)
                                                                          {
                                                                               
                                                                                GvDetail.Visible = false;
                                                                          }
                                                                          else
                                                                             {
                                                                             GvDetail.Visible = true;
                                                                          } %>
                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvDetail" runat="server" AutoGenerateColumns="False" Width="100%" OnRowCreated="GvDetail_RowCreated">
                                                                            <Columns>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton ID="IbtnDetailEdit" runat="server" CausesValidation="False" CommandArgument='<%#Eval("Serial_No") %>'
                                                                                            ImageUrl="~/Images/edit.png" OnCommand="IbtnDetailEdit_Command" Width="16px" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField Visible="false">
                                                                                    <HeaderTemplate>
                                                                                        <asp:Label ID="lblGvDetailselect" Text="Select All" runat="server"></asp:Label>
                                                                                        <asp:CheckBox ID="chkSelectAllDelete" OnCheckedChanged="chkGvDetailSelectAll_ChkChanged" AutoPostBack="true" runat="server" />
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                      <%--  <asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%#Eval("Serial_No") %>'
                                                                                            ImageUrl="~/Images/Erase.png" OnCommand="IbtnDetailDelete_Command" Width="16px" />--%>
                                                                                        <asp:HiddenField ID="hdnGvDetailSerialNo" runat="server" Value='<%#Eval("Serial_No") %>' />
                                                                                        <asp:CheckBox ID="ChkbtnDelete"  runat="server" CausesValidation="false" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvSerialNo" runat="server" Visible="false" Text='<%#Eval("Serial_No") %>' />
                                                                                        <asp:Label ID="lblSerialNo" runat="server" Text='<%#Container.DataItemIndex+1 %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Id %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvProductcode" runat="server" Text='<%#ProductCode(Eval("Product_Id").ToString()) %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Name %>">
                                                                                    <HeaderTemplate>
                                                                                        <asp:Label ID="lblGrdHdProductName" Text="<%$ Resources:Attendance,Name %>" runat="server"></asp:Label>
                                                                                        <asp:CheckBox ID="chkShortProductName1" Text="" runat="server" ToolTip="Dispay detail Name" Checked="true" OnCheckedChanged="chkShortProductName_CheckedChanged" AutoPostBack="true"></asp:CheckBox>
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <table>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:HiddenField ID="hdnProductId" runat="server" Value='<%#Eval("Product_Id") %>' />
                                                                                                    <cc1:Editor ID="hdnSuggestedProductdesc" Visible="false" runat="server" TopToolbarPreservePlace="false"
                                                                                                        Content='<%#Eval("ProductDescription") %>' />
                                                                                                    <asp:Label ID="lblgvProductName" runat="server" Text='<%#SuggestedProductName(Eval("Product_Id").ToString(),Eval("Serial_No").ToString()) %>' Visible="true" />
                                                                                                    <asp:Label ID="lblShortProductName1" runat="server" Text='<%# getShortSuggestedProductName(Eval("Product_Id").ToString(),Eval("Serial_No").ToString()) %>' Visible="false"></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Image ID="lnkDes" runat="server" Visible="false" ImageUrl="~/Images/detail.png" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                        <br />
                                                                                        <%--<asp:Panel ID="PopupMenu1" Width="100%" runat="server">
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
                                                                                                                        <cc1:Editor ID="lblgvProductDescription" runat="server" TopToolbarPreservePlace="false"
                                                                                                                            Content='<%#SuggestedProductName(Eval("Product_Id").ToString(),Eval("Serial_No").ToString()) %>' />
                                                                                                                    </asp:Panel>
                                                                                                                    <br />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </asp:Panel>--%>
                                                                                        <cc1:Editor ID="lblgvProductDescription" runat="server" Visible="false" TopToolbarPreservePlace="false"
                                                                                            Content='<%#SuggestedProductName(Eval("Product_Id").ToString(),Eval("Serial_No").ToString()) %>' />

                                                                                        <%--<cc1:HoverMenuExtender ID="hme3" runat="Server" TargetControlID="lnkDes" PopupControlID="PopupMenu1"
                                                                                            HoverCssClass="popupHover" PopupPosition="Right" OffsetX="0" OffsetY="0" PopDelay="50" />--%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="hdnUnitId" runat="server" Value='<%#Eval("UnitId") %>' />
                                                                                        <asp:Label ID="lblgvUnit" runat="server" Text='<%#GetUnitName(Eval("UnitId").ToString())     %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField Visible="false" HeaderText="<%$ Resources:Attendance,Description %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvProductDescription1" runat="server" Text='<%#Eval("ProductDescription") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Currency %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="hdnCurrencyId" runat="server" Value='<%#Eval("Currency_Id") %>' />
                                                                                        <asp:Label ID="lblgvCurrency" runat="server" Text='<%#GetCurrencyName(Eval("Currency_Id").ToString()) %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quantity %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="lblgvQuantity" Width="30px" ForeColor="#4d4c4c" runat="server" onchange="SetSelectedRow(this)" OnTextChanged="lblgvQuantity_TextChanged" AutoPostBack="true" Text='<%#Eval("Quantity") %>' />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderquantity" runat="server"
                                                                                            Enabled="True" TargetControlID="lblgvQuantity" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Estimated %>" Visible="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvEstimatedUnitPrice" runat="server" Text='<%#Eval("EstimatedUnitPrice") %>' />
                                                                                        <br />
                                                                                        <asp:LinkButton ID="lnkDeatil" runat="server" Text="<%$ Resources:Attendance,Suggested Sales Price%>"></asp:LinkButton><br />
                                                                                        <asp:Panel CssClass="popupMenu" ID="PopupMenu" Width="350px" Height="100px" runat="server">
                                                                                            <table border="1" cellpadding="0" cellspacing="0" bordercolor="#c6c6c6">
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <table width="314" height="110" cellspacing="0" bgcolor="#F9F9F9">
                                                                                                            <tr>
                                                                                                                <td height="21" colspan="2">
                                                                                                                    <div align="left" style="background: url(../Images/InvGridHdr.jpg) repeat">
                                                                                                                        Details
                                                                                                                    </div>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td colspan="2" align="left" valign="top">
                                                                                                                    <br />
                                                                                                                    <table style="height: 100px; width: 300px;">
                                                                                                                        <tr style="background-color: whitesmoke">
                                                                                                                            <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' valign="top">
                                                                                                                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Price%>" />
                                                                                                                            </td>
                                                                                                                            <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' valign="top">:
                                                                                                                            </td>
                                                                                                                            <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' valign="top">
                                                                                                                                <asp:Label ID="Label8" runat="server" Height="50px" Text='<%#Eval("PurchaseProductPrice") %>' />
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr style="background-color: whitesmoke">
                                                                                                                            <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' valign="top">
                                                                                                                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Description%>" />
                                                                                                                            </td>
                                                                                                                            <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' valign="top">:
                                                                                                                            </td>
                                                                                                                            <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' valign="top">
                                                                                                                                <asp:Literal ID="txtdesc" runat="server" Text='<%#Eval("PurchaseProductDescription") %>'></asp:Literal>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                    <%--<asp:Label ID="lblgvProductDescription" runat="server" Text='<%#Eval("ProductDescription") %>' />--%>
                                                                                                                    <br />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </asp:Panel>
                                                                                        <cc1:HoverMenuExtender ID="hme2" runat="Server" TargetControlID="lnkDeatil" PopupControlID="PopupMenu"
                                                                                            HoverCssClass="popupHover" PopupPosition="Right" OffsetX="0" OffsetY="0" PopDelay="50" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>

                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Price %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtgvUnitPrice" Width="50px" runat="server" ForeColor="#4d4c4c" onchange="SetSelectedRow(this)"
                                                                                            OnTextChanged="txtgvUnitPrice_TextChanged" Text='<%#Eval("SalesPrice") %>' AutoPostBack="true" />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                                            TargetControlID="txtgvUnitPrice" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtgvQuantityPrice" Width="70px" onchange="SetSelectedRow(this)" ReadOnly="true" runat="server"
                                                                                            ForeColor="#4d4c4c" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,% %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtgvDiscountP" onchange="SetSelectedRow(this)" Width="30px" ForeColor="#4d4c4c" runat="server"
                                                                                            OnTextChanged="txtgvDiscountP_TextChanged" AutoPostBack="true" Text='<%#Eval("DiscountPercent") %>' />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                                                            TargetControlID="txtgvDiscountP" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Value %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtgvDiscountV" onchange="SetSelectedRow(this)" Width="45px" ForeColor="#4d4c4c" runat="server"
                                                                                            OnTextChanged="txtgvDiscountV_TextChanged" AutoPostBack="true" Text='<%#Eval("DiscountValue") %>' />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                                                            TargetControlID="txtgvDiscountV" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,After Price %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtgvPriceAfterDiscount" onchange="SetSelectedRow(this)" ForeColor="#4d4c4c" Width="60px" ReadOnly="true"
                                                                                            runat="server" Text='<%#Eval("PriceAfterDiscount") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,% %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtgvTaxP" onchange="SetSelectedRow(this)" Enabled="false" Visible="true" Width="30px" ForeColor="#4d4c4c" runat="server" OnTextChanged="txtgvTaxP_TextChanged"
                                                                                            AutoPostBack="true" Text='<%#Eval("TaxPercent") %>' />
                                                                                        <asp:ImageButton ID="BtnAddTax_Gv_Detail" runat="server" CommandName="GvDetail" CommandArgument='<%# Eval("Product_Id") %>' OnCommand="BtnAddTax_Command" ImageUrl="~/Images/plus.png" Width="30px" Height="30px" ToolTip="Add Tax" />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                                            TargetControlID="txtgvTaxP" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                        <asp:Label runat="server" ID="lblTaxName" Text='<%# getTaxName(Eval("Serial_no").ToString()) %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Value %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtgvTaxV" onchange="SetSelectedRow(this)" Enabled="false" Width="45px" ForeColor="#4d4c4c" runat="server" OnTextChanged="txtgvTaxV_TextChanged"
                                                                                            AutoPostBack="true" Text='<%#Eval("TaxValue") %>' />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                                                            TargetControlID="txtgvTaxV" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,After Price %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtgvPriceAfterTax" onchange="SetSelectedRow(this)" Width="60px" ForeColor="#4d4c4c" ReadOnly="true"
                                                                                            runat="server" Text='<%#Eval("PriceAfterTax") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$Resources:Attendance,Net Amount %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtgvTotal" onchange="SetSelectedRow(this)" Width="60px" ForeColor="#4d4c4c" ReadOnly="true" runat="server"
                                                                                            Text='<%#Eval("PriceAfterTax") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Stock">
                                                                                    <ItemTemplate>
                                                                                        <a onclick="lnkStockInfo_Command('<%# Eval("Product_Id") %>')" style="cursor: pointer"><%#GetAmountDecimal(Eval("Sysqty").ToString()) %></a>
                                                                                        <%--<asp:LinkButton ID="lnkStockInfo" runat="server" Text='<%#GetAmountDecimal(Eval("Sysqty").ToString()) %>'
                                                                                    Font-Underline="true" ToolTip="View Detail" OnCommand="lnkStockInfo_Command"
                                                                                    ForeColor="Blue" CommandArgument='<%# Eval("Product_Id") %>'></asp:LinkButton>--%>

                                                                                        <a onclick="lnkStockInfo_Command('<%# Eval("Product_Id") %>')" style="cursor: pointer">(<%#GetUnpostedQty(Eval("Product_Id").ToString()) %>)</a>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField Visible="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtgvAgentCommission" OnClientClick="SetSelectedRow(this)" ForeColor="#4d4c4c" Width="60px" runat="server" Text='<%#Eval("AgentCommission") %>' Enabled='<%#IsAddAgentCommission(Eval("Sysqty").ToString()) %>' />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtgvAgentCommission" runat="server"
                                                                                            Enabled="True" TargetControlID="txtgvAgentCommission" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                                </asp:TemplateField>
                                                                            </Columns>

                                                                            <PagerStyle CssClass="pagination-ys" />

                                                                        </asp:GridView>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upProduct">
                                                                    <ProgressTemplate>
                                                                        <div class="modal_Progress">
                                                                            <div class="center_Progress">
                                                                                <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                            </div>
                                                                        </div>
                                                                    </ProgressTemplate>
                                                                </asp:UpdateProgress>
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
                                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Value %>" />
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
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                                            TargetControlID="txtTaxP" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label21" runat="server" Text="<%$ Resources:Attendance,Value %>" />
                                                        <asp:TextBox ID="txtTaxV" runat="server" class="form-control" Enabled="false" OnTextChanged="txtTaxV_TextChanged"
                                                            AutoPostBack="true" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
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
                                                                                    <cc1:Editor ID="txtFooter" runat="server" Class="form-control" Height="300px" />
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
                                                        <asp:Button ID="btnSQuoteSave" ValidationGroup="Save" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                            Visible="false" Class="btn btn-success" OnClick="btnSQuoteSave_Click" />&nbsp;&nbsp;
                                <asp:Button ID="btnSQuoteSaveandPrint" runat="server" Text="<%$ Resources:Attendance,Save & Print %>"
                                    Visible="false" Class="btn btn-primary" OnClick="btnSQuoteSaveandPrint_Click" />&nbsp;&nbsp;
                                <asp:Button ID="BtnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                    Class="btn btn-primary" CausesValidation="False" OnClick="BtnReset_Click" />&nbsp;&nbsp;
                                <asp:Button ID="btnSQuoteCancel" runat="server" Class="btn btn-danger" Text="<%$ Resources:Attendance,Cancel %>"
                                    CausesValidation="False" OnClick="btnSQuoteCancel_Click" />&nbsp;&nbsp;
                                <asp:Button ID="btnOrderDetail" runat="server" Class="btn btn-primary" Text="Order Detail"
                                    CausesValidation="False" OnClick="btnOrderDetail_Click" />
                                                        <asp:Button ID="Btn_Address_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Followup123" Text="Add Followup" />

                                                        <asp:HiddenField ID="editid" runat="server" />
                                                    </div>

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
                                                    <asp:Label ID="Label2" runat="server" Text="Advance Search"></asp:Label></h3>
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
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Quotation No. %>" Value="SQuotation_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Quotation Date %>" Value="Quotation_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Employee Name %>" Value="EmployeeName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Customer Name %>"  Value="Customer_Name" Selected="True"></asp:ListItem>
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
                                                                    <asp:Label ID="lblgvSQDate" runat="server" Text='<%#GetDate(Eval("Quotation_Date").ToString()) %>' />
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
                                                                    <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
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
                    <asp:UpdatePanel ID="Update_Modal_GST" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="hdnSerialNo" runat="server" />
                            <asp:HiddenField ID="hdnAmount" runat="server" />
                            <asp:HiddenField ID="hdnTaxAmount" runat="server" />

                            <div class="row">
                                <div class="col-md-12" runat="server" id="divTaxForNewProduct" visible="false">
                                    <div class="col-md-4">
                                        Select Tax:
                                        <asp:DropDownList runat="server" ID="ddltaxList" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-4">
                                        Tax Percentage:
                                        <asp:TextBox runat="server" ID="txtTaxValue" CssClass="form-control" onchange="validatePercentage(this)"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <br />
                                        <asp:Button runat="server" Text="Add Tax" ID="btnAddTax" OnClick="btnAddTax_Click" CssClass="btn btn-primary"></asp:Button>
                                    </div>
                                    <div class="col-md-12">
                                    </div>
                                </div>

                                <div class="col-md-12">
                                    <asp:UpdatePanel runat="server" ID="updatePanelTax" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnAddTax" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="gvTaxCalculation" />
                                            <asp:AsyncPostBackTrigger ControlID="GvDetail" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvTaxCalculation" runat="server" Width="100%" AutoGenerateColumns="false">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Delete" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:ImageButton runat="server" ID="gvtaxDelete" ImageUrl="~/Images/Erase.png" OnCommand="gvtaxDelete_Command" CommandName='<%# Eval("Tax_Id") %>' CommandArgument='<%# Eval("serial_no") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tax Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTaxName" runat="server" Text='<%# Eval("Tax_Name") %>' />
                                                            <asp:HiddenField ID="lblgvTaxId" runat="server" Value='<%# Eval("Tax_Id") %>' />
                                                            <asp:HiddenField ID="lblgvProductId" runat="server" Value='<%# Eval("Product_Id") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tax Value (%)">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtTaxValueInPer" CssClass="form-control" Style="text-align: center;" runat="server" Text='<%# Eval("Tax_Value") %>' AutoPostBack="true" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>



                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="Update_Modal_Button_GST" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="Hdn_Serial_No_Tax" runat="server" />
                            <asp:HiddenField ID="Hdn_Product_Id_Tax" runat="server" />
                            <asp:HiddenField ID="Hdn_unit_Price_Tax" runat="server" />
                            <asp:HiddenField ID="Hdn_Discount_Tax" runat="server" />
                            <asp:Button ID="Btn_Update_Tax" runat="server" CssClass="btn btn-primary"
                                Text="<%$ Resources:Attendance,Update %>" OnClick="Btn_Update_Tax_Click" />
                            <button id="btnClosePopup" type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

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




    <asp:UpdateProgress ID="UpdateProgress11" runat="server" AssociatedUpdatePanelID="Update_Modal_GST">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress16" runat="server" AssociatedUpdatePanelID="Update_Modal_Button_GST">
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
        function getReportDataWithLoc(transId, locId) {
            debugger;
            $("#prgBar").css("display", "block");
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
       
        function ClearGrid() {
           
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
        function uploadComplete(sender, args) {
            document.getElementById('<%= Img_WrongUpload.ClientID %>').style.display = "none";
            document.getElementById('<%= Img_RightUpload1.ClientID %>').style.display = "";
        }
        function uploadStarted(sender, args) {
            var filename = args.get_fileName();

            var filext = filename.substring(filename.lastIndexOf(".") + 1);
            filext = filext.toLowerCase();
            if (filext == "xls" || filext == "xlsx" || filext == "mdb" || filext == "accdb") {
                return true;
            }
            else {
                throw {
                    name: "Invalid File Type",
                    level: "Error",
                    message: "Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file",
                    htmlMessage: "Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file"
                }
                return false;
            }
        }
        function uploadError(sender, args) {
            document.getElementById('<%= Img_RightUpload1.ClientID %>').style.display = "none";
            document.getElementById('<%= Img_WrongUpload.ClientID %>').style.display = "";
            alert('Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file');
        }



        function uploadCompleteUpdate(sender, args) {
            document.getElementById('<%= Img_WrongUpload1.ClientID %>').style.display = "none";
            document.getElementById('<%= Img_RightUpload11.ClientID %>').style.display = "";
        }
        function uploadStartedUpdate(sender, args) {
            var filename = args.get_fileName();

            var filext = filename.substring(filename.lastIndexOf(".") + 1);
            filext = filext.toLowerCase();
            if (filext == "xls" || filext == "xlsx" || filext == "mdb" || filext == "accdb") {
                return true;
            }
            else {
                throw {
                    name: "Invalid File Type",
                    level: "Error",
                    message: "Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file",
                    htmlMessage: "Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file"
                }
                return false;
            }
        }
        function uploadErrorUpdate(sender, args) {
            document.getElementById('<%= Img_RightUpload11.ClientID %>').style.display = "none";
      document.getElementById('<%= Img_WrongUpload1.ClientID %>').style.display = "";
            alert('Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file');
        }






        function Show_Modal_GST() {
            document.getElementById('<%= Btn_GST_Modal.ClientID %>').click();
        }
        function Modal_Address_Open() {
            document.getElementById('<%= Btn_Address_Modal.ClientID %>').click();
        }
        function Modal_NewAddress_Open() {
            document.getElementById('<%= Btn_NewAddress.ClientID %>').click();
        }
      <%-- function Li_Tab_Upload() {
            document.getElementById('<%= Btn_Upload.ClientID %>').click();
        }--%>
        function LI_List_Active1() {
        }

        function lblgvSQNo_Command(quotationId, locationid) {
            PageMethods.lblgvSQNo_Command(quotationId, locationid, function (data) {
                debugger;
                if (data[0] == "false") {
                    if (data[1] == "Quotation") {
                        var newWindow = window.open('../Sales/SalesQuotation.aspx?QuotationID=' + quotationId + '&LocationID=' + locationid, 'window', 'width=1024');
                        newWindow.onload = function () {
                            newWindow.LI_Edit_Active();
                        };
                    }
                    else { 
                    alert(data[1]);
                        return;
                    }
                }
                else {
                    window.open('../Sales/SalesOrderView.aspx?OrderId=' + data[1] + '', 'window', 'width=1024, ');
                }
            }, errorMessage);

        }
        function errorMessage(err) {
            alert(err);
        }
        function txtShipCustomerName_TextChanged(ctrl) {
            var data = getCustomerAddressName(ctrl);
            var shippingAddress = document.getElementById('<%=txtShipingAddress.ClientID%>');
            if (data[0] == "true") {
                shippingAddress.value = data[1];
            }
            else {
                shippingAddress.value = "";
                ctrl.value = "";
                alert("select from suggestions only");
                return;
            }
        }
        function txtProductCode_TextChanged(ctrl) {

            $.ajax({
                url: 'SalesInquiry.aspx/txtProduct_TextChanged',
                method: 'post',
                async: false,
                contentType: "application/json; charset=utf-8",
                data: "{'product':'" + ctrl.value + "'}",
                success: function (data) {
                    debugger;
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

                function txtCustomer_TextChanged(ctl) {
                    try {
                        if (ctl.value == "") {
                            resetCustomerIdForContact();
                            return;
                        }
                        validateCustomer(ctl);
                        document.getElementById('<%= txtContactName.ClientID %>').value = "";
                }
                catch (ex) {
                    alert(ex);
                }
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
            var tblProduct = $('#<%=GvDetail.ClientID%> tbody tr').each(function () {

                if (count % 2 == 0) {
                    var row = $(this);
                    debugger;
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
                        }
                        else {
                            Line_Total = row[0].cells[13].childNodes[1];
                        }

                        if (Unit_Price.value == "")
                            Unit_Price.value = "0";

                        if (Quantity.value == "")
                            Quantity.value = "0";

                        if (Discount_Percentage.value == "")
                            Discount_Percentage.value = "0";

                        var F_Unit_Price = Unit_Price;
                        var F_Order_Quantity = Quantity;
                        var F_Discount_Percentage = Discount_Percentage.value;
                        var F_Tax_Percentage = 0;
                        var F_Tax_Amount = 0;

                        if (isDiscountApplicable == "true") {
                            var F_Discount_Amount = Get_Discount_Amount(F_Unit_Price, F_Discount_Percentage);
                        }
                        if (isTaxApplicable == "true") {
                            Add_Tax_To_Session((F_Unit_Price - F_Discount_Amount), Product_Id.Value, lblgvSerialNo.value);//ajax
                            var F_Tax_Percentage = getTaxPercentage(Product_Id.Value, lblgvSerialNo.value);//ajax
                            var F_Tax_Amount = GetAmountWithDecimal(getTaxAmount((F_Unit_Price - F_Discount_Amount).toString(), Product_Id.Value, lblgvSerialNo.value), decimalCount);//ajax
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
            }
            catch (err) {
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
            }
            catch (err) {
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
            }
            catch (er) {
                return 0;
            }
        }

        function setScrollAndRow() {
            try {
                debugger;
                var rowIndex = $('#<%= hdfCurrentRow.ClientID %>').val();
                var parent = document.getElementById('<%= GvDetail.ClientID %>');
                var rowIndex = parseInt(rowIndex);
                parent.rows[rowIndex + 1].style.backgroundColor = "#A1DCF2";
                var h = document.getElementById("<%=hfScrollPosition.ClientID%>");
                document.getElementById("<%=scrollArea.ClientID%>").scrollTop = h.value;

            }
            catch (e) {

            }
        }

        function SetDivPosition() {
            var intY = document.getElementById("<%=scrollArea.ClientID%>").scrollTop;
            var h = document.getElementById("<%=hfScrollPosition.ClientID%>");
            h.value = intY;
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
                }
                catch (er) {
                    CustomerId = "0";
                }
            }
            else {
                CustomerId = "0";
            }
            window.open('../Purchase/CustomerHistory.aspx?ContactId=' + CustomerId + '&&Page=SQ', 'window', 'width=1024, ');
        }
        function redirectToHome() {
            if (confirm('Tax is not enabled on this location do you want to continue ?')) {
                window.open('../MasterSetup/Home.aspx', 'window', 'width=1024,');
                return true;
            }
            else {
                return false;
            }

        }
         
        function redirectToHome(msg) {
            if (confirm(msg)) {
                window.open('../MasterSetup/Home.aspx', 'window', 'width=1024,');
                return true;
            }
            else {
                return false;
            }
        }
        function Modal_CustomerInfo_Open() {
            document.getElementById('<%= Btn_CustomerInfo_Modal.ClientID %>').click();
        }
    </script>
    <script src="../Script/customer.js"></script>
    <script src="../Script/employee.js"></script>


</asp:Content>
