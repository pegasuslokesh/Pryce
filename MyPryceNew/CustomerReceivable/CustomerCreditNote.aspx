﻿<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="CustomerCreditNote.aspx.cs" Inherits="CustomerReceivable_CustomerCreditNote" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/ReportSystem.ascx" TagName="ReportSystem" TagPrefix="RS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-money-check-alt"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Customer Credit Note%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Finance Setup%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Customer Receivable%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Customer Credit Note%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="btnList_Click" Text="List" />
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" OnClick="btnNew_Click" Text="New" />
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:Button ID="Btn_myModal" Style="display: none;" runat="server" data-toggle="modal" data-target="#myModal" Text="View Modal" />
            <asp:HiddenField runat="server" ID="hdnCanPrint" />
            <asp:HiddenField runat="server" ID="hdnCanView" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgress11" runat="server" AssociatedUpdatePanelID="Update_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">
                <ul class="nav nav-tabs pull-right bg-blue-gradient">
                    <li style="display: none;" id="Li_Bin"><a href="#Bin" onclick="Li_Tab_Bin()" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
                    <li id="Li_New"><a onclick="Li_Tab_New()" href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List" class="active"><a href="#List" onclick="Li_Tab_List()" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane active" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server">
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
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlPosted" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPosted_SelectedIndexChanged">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Posted %>" Value="Posted"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,UnPosted %>" Value="UnPosted" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,All %>" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Voucher No %>" Value="Voucher_No" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Customer Name %>" Value="Name"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlOption" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                        <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" placeholder="Search from Content"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False"
                                                        OnClick="btnbindrpt_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False"
                                                        OnClick="btnRefreshReport_Click"
                                                        ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                </div>
                                                <div class="col-md-12">
                                                    <br />
                                                </div>
                                                 <div class="col-md-4">
                                                     <asp:Label ID="lblLocationList" runat="server" Text="<%$ Resources:Attendance,Location %>"></asp:Label>
                                                     <asp:HiddenField ID="hdnLocId" Value="0" runat="server" />
                                                     <asp:HiddenField ID="hdnCurrencyId" Value="0" runat="server" />
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="ddlLocationList" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlLocationList_SelectedIndexChanged" />
                                                    </div>
                                                    <br />
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" />
                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate"
                                                        Format="dd-MMM-yyyy" />
                                                    <br />
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,To Date %>"></asp:Label>
                                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" />
                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"
                                                        Format="dd-MMM-yyyy" />
                                                    <br />
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:LinkButton ID="ImgDateSearch" runat="server" CausesValidation="False"
                                                        OnClick="ImgDateSearch_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="ImageButton2" runat="server" CausesValidation="False"
                                                        OnClick="btnRefreshReport_Click"
                                                        ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <br />
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid" <%= GvVoucher.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvVoucher" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server"
                                                        AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvVoucher_PageIndexChanging"
                                                        AllowSorting="True" OnSorting="GvVoucher_Sorting">

                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <a style="cursor: pointer" onclick="getReportData('<%# Eval("Trans_Id") %>')"><i class="fa fa-print"></i>Report System</a>
                                                                            </li>
                                                                            <%--<li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnPrint" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        OnCommand="IbtnPrint_Command" ToolTip="<%$ Resources:Attendance,Print %>"
                                                                        Visible="true"><i class="fa fa-print"></i>Print</asp:LinkButton>
                                                                            </li>--%>

                                                                            <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        Visible="true" ToolTip="<%$ Resources:Attendance,View %>"
                                                                        OnCommand="lnkViewDetail_Command" CausesValidation="False"><i class="fa fa-eye"></i>View</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        Visible="true" OnCommand="btnEdit_Command" CausesValidation="False"
                                                                        ToolTip="<%$ Resources:Attendance,Edit %>"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        OnCommand="IbtnDelete_Command" Visible="true"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>
                                                                        
                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            
                                                           
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Voucher No %>" SortExpression="Voucher_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvVoucherNo" runat="server" Text='<%#Eval("Voucher_No") %>' />
                                                                </ItemTemplate>
                                                                
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Voucher Date %>" SortExpression="Voucher_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvVoucherDate" runat="server" Text='<%#GetDate(Eval("Voucher_Date").ToString()) %>' />
                                                                </ItemTemplate>
                                                                
                                                            </asp:TemplateField>
                                                          
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCustomer" runat="server" Text='<%#Eval("Name") %>' />
                                                                </ItemTemplate>
                                                                
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Narration %>" SortExpression="Narration">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvNarration" runat="server" Text='<%#Eval("Narration") %>' />
                                                                </ItemTemplate>
                                                                
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Credit Amount%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvVoucherAmount" runat="server" Text='<%# SetDecimal(Eval("credit_amount").ToString()) %>' />
                                                                </ItemTemplate>
                                                                
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Adjustment Detail">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAdjustmentDetail" runat="server" Text='<%#Eval("Adjustment_detail") %>' />
                                                                </ItemTemplate>
                                                               
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Balance Amount%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBalanceAmount" runat="server" Text='<%# SetDecimal(Eval("balance").ToString()) %>' />
                                                                </ItemTemplate>
                                                               
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Created By %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCreatedBy" runat="server" Text='<%# GetEmployeeNameByEmpCode(Eval("CreatedBy").ToString()) %>' />
                                                                </ItemTemplate>
                                                                
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Modified By %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvModifiedBy" runat="server" Text='<%# GetEmployeeNameByEmpCode(Eval("ModifiedBy").ToString()) %>' />
                                                                </ItemTemplate>
                                                                
                                                            </asp:TemplateField>

                                                          
                                                        </Columns>

                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblVoucherType" runat="server" Text="<%$ Resources:Attendance,Voucher Type %>" />
                                                        <asp:DropDownList ID="ddlVoucherType" runat="server" CssClass="form-control">
                                                            <asp:ListItem Text="Credit Note" Value="CCN"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <%--<asp:TextBox ID="txtVoucherType" runat="server" CssClass="form-control" />--%>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblCurrency" runat="server" Text="<%$ Resources:Attendance,Local Currency %>" />
                                                        <asp:DropDownList ID="ddlLocalCurrency" runat="server" CssClass="form-control"
                                                            Enabled="false" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblVoucherNo" runat="server" Text="<%$ Resources:Attendance,Voucher No. %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtVoucherNo" ErrorMessage="<%$ Resources:Attendance,Enter Voucher No. %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtVoucherNo" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblVoucherDate" runat="server" Text="<%$ Resources:Attendance,Voucher Date %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtVoucherDate" ErrorMessage="<%$ Resources:Attendance,Enter Voucher Date %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtVoucherDate" runat="server" CssClass="form-control" Enabled="true" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender_txtVoucherDate" runat="server" TargetControlID="txtVoucherDate"
                                                            Format="dd/MMM/yyyy" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:RadioButton ID="rbCashPayment" runat="server" Text="Cash Payment"
                                                            GroupName="Pay" OnCheckedChanged="rbCashPayment_CheckedChanged" AutoPostBack="true" />
                                                        <asp:RadioButton ID="rbChequePayment" Style="margin-left: 20px;" runat="server" Text="Cheque Payment"
                                                            GroupName="Pay" OnCheckedChanged="rbCashPayment_CheckedChanged" AutoPostBack="true" />
                                                        <br />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                    </div>
                                                    <div id="trCheque1" runat="server" visible="false">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblChequeIssueDate" runat="server" Text="<%$ Resources:Attendance,Cheque Issue Date%>" />
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator7" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtChequeIssueDate" ErrorMessage="<%$ Resources:Attendance,Enter Cheque Issue Date %>"></asp:RequiredFieldValidator>

                                                            <asp:TextBox ID="txtChequeIssueDate" runat="server" CssClass="form-control" />
                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender_txtchequeissuedate" runat="server" TargetControlID="txtChequeIssueDate"
                                                                Format="dd/MMM/yyyy" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblChequeClearDate" runat="server" Text="<%$ Resources:Attendance,Cheque Clear Date%>" />
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator8" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtChequeClearDate" ErrorMessage="<%$ Resources:Attendance,Enter Cheque Clear Date %>"></asp:RequiredFieldValidator>

                                                            <asp:TextBox ID="txtChequeClearDate" runat="server" CssClass="form-control" />
                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender_chequeCleardate" runat="server" TargetControlID="txtChequeClearDate"
                                                                Format="dd/MMM/yyyy" />
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div id="trCheque2" runat="server" visible="false" class="col-md-6">
                                                        <asp:Label ID="lblChequeNo" runat="server" Text="<%$ Resources:Attendance,Cheque No.%>" />
                                                        <asp:TextBox ID="txtChequeNo" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Refrence%>" />
                                                        <asp:TextBox ID="txtReference" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblCustomer" runat="server" Text="<%$ Resources:Attendance,Customer %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Customer"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCustomerName" ErrorMessage="<%$ Resources:Attendance,Enter Customer%>"></asp:RequiredFieldValidator>

                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                AutoPostBack="True" OnTextChanged="txtCustomerName_TextChanged" />

                                                            <cc1:AutoCompleteExtender ID="txtCustomerName_AutoCompleteExtender" runat="server"
                                                                CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                                                ServiceMethod="GetCompletionListCustomer" ServicePath="" TargetControlID="txtCustomerName"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <div class="input-group-btn">
                                                                <asp:Button ID="btnAddCustomer" runat="server" CssClass="btn btn-primary" OnClick="btnAddCustomer_OnClick"
                                                                    Text="<%$ Resources:Attendance,Add %>" ValidationGroup="Customer" />
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="overflow: auto; max-height: 500px;">
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvPendingInvoice" runat="server" AutoGenerateColumns="False" Width="100%"
                                                            ShowFooter="true">
                                                            <Columns>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkTrandId" runat="server" OnCheckedChanged="chkTrandId_CheckedChanged"
                                                                            AutoPostBack="true" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <ItemStyle />
                                                                    <FooterStyle BorderStyle="None" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSNo" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <ItemStyle />
                                                                    <FooterStyle BorderStyle="None" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Invoice No.">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPONo" runat="server" Text='<%# Eval("Invoice_No") %>'></asp:Label>
                                                                        <asp:HiddenField ID="hdnRefId" runat="server" Value='<%# Eval("Ref_Id") %>' />
                                                                        <asp:HiddenField ID="hdnRefType" runat="server" Value='<%# Eval("Ref_Type") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <ItemStyle />
                                                                    <FooterStyle BorderStyle="None" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Invoice Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# GetDateFormat(Eval("Invoice_Date").ToString()) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterStyle BorderStyle="None" />
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Invoice Amount">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblinvamount" runat="server" Text='<%# Eval("Invoice_Amount")%>'></asp:Label>
                                                                        <asp:HiddenField ID="hdnExchangeRate" runat="server" />
                                                                    </ItemTemplate>
                                                                    <FooterStyle BorderStyle="None" />
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Received Amount">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblpaidamount" runat="server" Text='<%# Eval("Paid_Receive_Amount").ToString() %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterStyle BorderStyle="None" />
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Due Amount">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbldueamount" runat="server" Text='<%# Eval("Due_Amount").ToString() %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterStyle BorderStyle="None" />
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Receive Amount Local">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtgvReceiveAmountLocal" runat="server" OnTextChanged="txtgvReceiveAmountLocal_OnTextChanged"
                                                                            AutoPostBack="true" Width="100px" />
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                            TargetControlID="txtgvReceiveAmountLocal" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                    <FooterStyle BorderStyle="None" />
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Currency">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlgvCurrency" runat="server" Width="130px" OnSelectedIndexChanged="ddlgvCurrency_SelectedIndexChanged"
                                                                            AutoPostBack="true" />
                                                                    </ItemTemplate>
                                                                    <FooterStyle BorderStyle="None" />
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Exchange Rate">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtgvExcahangeRate" Width="70px" runat="server" OnTextChanged="txtgvExcahangeRate_OnTextChanged"
                                                                            AutoPostBack="true" />
                                                                    </ItemTemplate>
                                                                    <FooterStyle BorderStyle="None" />
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Receive Amount Foreign">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtgvReceiveAmountForeign" runat="server" OnTextChanged="txtpayforeign_OnTextChanged"
                                                                            AutoPostBack="true" Width="100px" />
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender24" runat="server" Enabled="True"
                                                                            TargetControlID="txtgvReceiveAmountForeign" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                    <FooterStyle BorderStyle="None" />
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <PagerStyle CssClass="pagination-ys" />

                                                        </asp:GridView>

                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <br />
                                                        <asp:CheckBox ID="chkCreditPay" CssClass="form-control" runat="server" Text="<%$ Resources:Attendance,Credit Pay %>" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblNetAmountLocal" runat="server" Text="<%$ Resources:Attendance,Net Amount Local %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtNetAmountLocal" ErrorMessage="<%$ Resources:Attendance,Enter Net Amount Local %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtNetAmountLocal" runat="server" ReadOnly="false" OnTextChanged="txtNetAmountLocal_OnTextChanged"
                                                            AutoPostBack="true" CssClass="form-control" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                            TargetControlID="txtNetAmountLocal" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblForeginCurrency" runat="server" Text="<%$ Resources:Attendance,Foreign Currency %>" />
                                                        <asp:DropDownList ID="ddlForeginCurrency" runat="server" CssClass="form-control"
                                                            OnSelectedIndexChanged="ddlForeginCurrency_SelectedIndexChanged" AutoPostBack="true" />
                                                        <asp:HiddenField ID="hdnFExchangeRate" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblExchangeRate" runat="server" Text="<%$ Resources:Attendance,Exchange Rate %>" />
                                                        <asp:TextBox ID="txtExchangeRate" runat="server" CssClass="form-control" AutoPostBack="True"
                                                            OnTextChanged="txtExchangeRate_TextChanged" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblNetAmountForeign" runat="server" Text="<%$ Resources:Attendance,Net Amount Foreign %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator6" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtNetAmountForeign" ErrorMessage="<%$ Resources:Attendance,Enter Net Amount Foreign %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtNetAmountForeign" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblDebitAccountName" runat="server" Text="<%$ Resources:Attendance,Debit A/C Name %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtDebitAccountName" ErrorMessage="<%$ Resources:Attendance,Enter Debit A/C Name %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtDebitAccountName" runat="server" CssClass="form-control"
                                                            BackColor="#eeeeee" AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtDebitAccountName"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblNarration" runat="server" Text="<%$ Resources:Attendance,Narration %>" />
                                                        <asp:TextBox ID="txtNarration" runat="server" CssClass="form-control" TextMode="MultiLine" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnVoucherSave" runat="server" ValidationGroup="Save" Text="<%$ Resources:Attendance,Save %>"
                                                            CssClass="btn btn-success" OnClick="btnVoucherSave_Click" UseSubmitBehavior="false" OnClientClick="this.disabled='true'; this.value='Please wait...';" />
                                                        <asp:HiddenField ID="hdnRef_Id" runat="server" Value="0" />
                                                        <asp:HiddenField ID="hdnRef_Type" runat="server" Value="0" />
                                                        <asp:HiddenField ID="hdnInvoiceNumber" runat="server" Value="0" />
                                                        <asp:HiddenField ID="hdnInvoiceDate" runat="server" Value="0" />

                                                        <asp:Button ID="BtnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                            CssClass="btn btn-primary" CausesValidation="False" OnClick="BtnReset_Click" />

                                                        <asp:Button ID="btnVoucherCancel" runat="server" CssClass="btn btn-danger" Text="<%$ Resources:Attendance,Cancel %>"
                                                            CausesValidation="False" OnClick="btnVoucherCancel_Click" />

                                                        <asp:HiddenField ID="hdnVoucherId" runat="server" Value="0" />
                                                        <br />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" style="display: none;" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server">
                            <ContentTemplate>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

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
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_List">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_New">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Bin">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:Panel ID="pnlMenuList" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlMenuNew" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlMenuBin" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PnlList" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PnlNewEdit" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PnlBin" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="Panel6" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="Panel7" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="Panel8" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="Panel9" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="Panel10" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="Panel11" runat="server" Visible="false"></asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
    <script type="text/javascript">

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
        function LI_List_Active() {
            $("#Li_List").addClass("active");
            $("#List").addClass("active");

            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");
        }

        function Li_Tab_List() {
            document.getElementById('<%= Btn_List.ClientID %>').click();
        }
        function Li_Tab_New() {
            document.getElementById('<%= Btn_New.ClientID %>').click();
        }
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }

        function myModal_Popup() {
            document.getElementById('<%= Btn_myModal.ClientID %>').click();
        }
        function myModal_Popup() {
            document.getElementById('<%= Btn_myModal.ClientID %>').click();
        }
        function getReportData(transId) {
            $("#prgBar").css("display", "block");
            document.getElementById('<%= reportSystem.FindControl("hdnTransId").ClientID %>').value = transId;
            setReportData();
        }
    </script>
    <script src="../Script/ReportSystem.js"></script>
</asp:Content>
