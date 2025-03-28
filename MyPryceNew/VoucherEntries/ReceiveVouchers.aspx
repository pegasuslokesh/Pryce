<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="ReceiveVouchers.aspx.cs" Inherits="VoucherEntries_ReceiveVouchers" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/WebUserControl/ucControlsSetting.ascx" TagName="ucCtlSetting" TagPrefix="UC" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/ReportSystem.ascx" TagName="ReportSystem" TagPrefix="RS" %>
<%@ Register Src="~/WebUserControl/FileUpload.ascx" TagPrefix="AT1" TagName="FileUpload1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-money-check-alt"></i>

        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Receive Vouchers%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Finance Setup%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Voucher Entries%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Receive Vouchers%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="btnList_Click" Text="List" />
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" OnClick="btnNew_Click" Text="New" />
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:Button ID="Btn_myModal" Style="display: none;" runat="server" data-toggle="modal" data-target="#myModal" Text="View Modal" />
            <asp:Button ID="Btn_Modal_FileUpload" Style="display: none;" runat="server" data-toggle="modal" data-target="#Fileupload123" Text="FileUpload" />
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
                    <li id="Li_Bin"><a href="#Bin" onclick="Li_Tab_Bin()" data-toggle="tab">
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
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Voucher No %>" Value="Voucher_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Voucher Type %>" Value="Voucher_Type"
                                                            Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Finance Code %>" Value="Finance_Code"></asp:ListItem>
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
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="ddlLocationList" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlLocationList_SelectedIndexChanged" />
                                                    </div>
                                                    <br />
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" />
                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate"
                                                        Format="dd-MMM-yyyy" />
                                                    <br />
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,To Date %>"></asp:Label>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"
                                                            Format="dd-MMM-yyyy" />
                                                        &nbsp;&nbsp;
                                                       <div class="input-group-btn">
                                                           <asp:LinkButton ID="ImgDateSearch" runat="server" CausesValidation="False"
                                                               OnClick="ImgDateSearch_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                       </div>
                                                        <div class="input-group-btn">
                                                            <asp:LinkButton ID="ImageButton2" runat="server" CausesValidation="False"
                                                                OnClick="btnRefreshReport_Click"
                                                                ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                            <asp:LinkButton ID="btnGvListSetting" ImageAlign="Right" ToolTip="List Settings" runat="server" OnClick="btnGvListSetting_Click" Visible="false"><span class="fa fa-wrench"  style="font-size:25px;"></span></asp:LinkButton>
                                                        </div>
                                                    </div>
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
                                                                            <%--<li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnPrint" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                    OnCommand="IbtnPrint_Command" ToolTip="<%$ Resources:Attendance,Print %>"
                                                                                    Visible="true"><i class="fa fa-print"></i>Print</asp:LinkButton>
                                                                            </li>--%>
                                                                            <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <a style="cursor: pointer" onclick="getReportData('<%# Eval("Trans_Id") %>')"><i class="fa fa-print"></i>Report System</a>
                                                                            </li>

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
                                                                            <li>
                                                                                <asp:LinkButton ID="btnFileUpload" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' CommandName='<%# Eval("Voucher_No") %>' OnCommand="btnFileUpload_Command" CausesValidation="False"><i class="fa fa-upload"></i>File Upload</asp:LinkButton>
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
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Voucher Type %>" SortExpression="Voucher_Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvVoucherType" runat="server" Text='<%#Eval("Voucher_Type") %>' />
                                                                </ItemTemplate>

                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Narration %>" SortExpression="Narration">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvFinanceCode" runat="server" Text='<%#Eval("Narration") %>' />
                                                                </ItemTemplate>

                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Voucher Amount%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvExchangerate" runat="server" Text='<%#GetVoucherAmount(Eval("Trans_Id").ToString()) %>' />
                                                                </ItemTemplate>

                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Currency %>" SortExpression="Currency_Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCurrency" runat="server" Text='<%# GetCurrencyName(Eval("Currency_Id").ToString()) %>' />
                                                                </ItemTemplate>

                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Post %>" SortExpression="Post">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPost" runat="server" Text='<%# Eval("Post") %>' />
                                                                </ItemTemplate>

                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Created By %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCreatedBy" runat="server" Text='<%#GetEmployeeNameByEmpCode(Eval("CreatedBy").ToString()) %>' />
                                                                </ItemTemplate>

                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Modified By %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvModifiedBy" runat="server" Text='<%#GetEmployeeNameByEmpCode(Eval("ModifiedBy").ToString()) %>' />
                                                                </ItemTemplate>

                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                    <!-- /.box-body -->
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="New" ">
                        <asp:UpdatePanel ID="Update_New" runat="server" >

                            <ContentTemplate>
                                                       
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <asp:ImageButton ID="btnControlsSetting" ImageAlign="Right" ToolTip="Controls Setting" runat="server" ImageUrl="~/Images/setting.png" OnClick="btnControlsSetting_Click" Style="width: 32px; height: 32px" Visible="false" />
                                                        <asp:HiddenField runat="server" ID="hdnLocId" Value="0" />
                                                        <asp:HiddenField runat="server" ID="hdnCurrencyId" Value="0" />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblVoucherType" runat="server" Text="<%$ Resources:Attendance,Voucher Type %>" />
                                                        <asp:DropDownList ID="ddlVoucherType" runat="server" CssClass="form-control">
                                                            <asp:ListItem Text="Receive Vouchers" Value="RV"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblCurrency" runat="server" Text="<%$ Resources:Attendance,Local Currency%>" />
                                                        <asp:DropDownList ID="ddlLocalCurrency" runat="server" CssClass="form-control"
                                                            Enabled="false" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblVoucherNo" runat="server" Text="<%$ Resources:Attendance,Voucher No. %>" />
                                                        <asp:TextBox ID="txtVoucherNo" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblVoucherDate" runat="server" Text="<%$ Resources:Attendance,Voucher Date %>" />
                                                        <asp:TextBox ID="txtVoucherDate" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender_txtVoucherDate" runat="server" TargetControlID="txtVoucherDate"
                                                            Format="dd/MMM/yyyy" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:RadioButton ID="rbCashPayment" runat="server" Text="Cash Payment"
                                                            GroupName="Pay" OnCheckedChanged="rbCashPayment_CheckedChanged" AutoPostBack="true" />
                                                        <asp:RadioButton ID="rbChequePayment" Style="margin-left: 20px;" runat="server" Text="Cheque Payment"
                                                            GroupName="Pay" OnCheckedChanged="rbCashPayment_CheckedChanged" AutoPostBack="true" />
                                                        <asp:CheckBox ID="chkReconcile" Visible="false" Style="margin-left: 20px;" runat="server" Text="<%$ Resources:Attendance, Reconcile%>" />
                                                        <br />
                                                        <br />
                                                    </div>
                                                    <div id="trCheque1" runat="server" visible="false">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblChequeIssueDate" runat="server" Text="<%$ Resources:Attendance,Cheque Issue Date%>" />
                                                            <asp:TextBox ID="txtChequeIssueDate" runat="server" CssClass="form-control" />
                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender_txtchequeissuedate" runat="server" TargetControlID="txtChequeIssueDate"
                                                                Format="dd/MMM/yyyy" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblChequeClearDate" runat="server" Text="<%$ Resources:Attendance,Cheque Clear Date%>" />
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
                                                        <asp:Label ID="lblReference" runat="server" Text="Reference" />
                                                        <asp:TextBox ID="txtReference" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div  id="dvScroll" class="col-md-12" onscroll="setScrollPosition(this.scrollTop);" >
                                                        <div class="row">

                                                             <asp:HiddenField ID="hfScrollPosition" Value="0" runat="server" />
                                                            <div class="col-md-12">
                                                                <div class="box box-primary">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Detail View%>" /></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <div class="form-group">
                                                                            <div class="col-md-12">
                                                                                <asp:Label ID="lblPaymentTo" runat="server" Text="<%$ Resources:Attendance,From Receive A/C%>" />
                                                                                <a style="color: Red">*</a>
                                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Add"
                                                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtPaymentTo" ErrorMessage="<%$ Resources:Attendance,Enter From Receive A/C %>"></asp:RequiredFieldValidator>

                                                                                <asp:TextBox ID="txtPaymentTo" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                    AutoPostBack="true" OnTextChanged="txtPaymentTo_textChnaged" />
                                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                                                    Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtPaymentTo"
                                                                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                </cc1:AutoCompleteExtender>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-12">
                                                                                <div id="pnlCostCenter" runat="server"  visible="false" >
                                                                                       <asp:Label ID="lblCOC" runat="server" Text="Cost Center" ></asp:Label>
                                                                                <asp:TextBox ID="txtCostCenter" runat="server" AutoPostBack="true" OnTextChanged="txtCostCenter_TextChanged" CssClass="form-control" BackColor="#eeeeee"></asp:TextBox>
                                                                                     <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                                                    Enabled="True" ServiceMethod="GetCompletionListCostCenterName" ServicePath="" CompletionInterval="100"
                                                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtCostCenter"
                                                                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                </cc1:AutoCompleteExtender>
                                                                                </div>
                                                                               <br />
                                                                            </div>
                                                                            <div id="trSupplier" runat="server" visible="false" class="col-md-12">
                                                                                <asp:Label ID="lblSupplierName" runat="server" Text="<%$ Resources:Attendance,Supplier %>" />
                                                                                <a style="color: Red">*</a>
                                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator9" ValidationGroup="Save"
                                                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSupplierName" ErrorMessage="<%$ Resources:Attendance,Enter Supplier%>"></asp:RequiredFieldValidator>

                                                                                <asp:TextBox ID="txtSupplierName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                    AutoPostBack="True" OnTextChanged="txtSupplierName_TextChanged" />
                                                                                <cc1:AutoCompleteExtender ID="txtSupplierName_AutoCompleteExtender" runat="server"
                                                                                    CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                                                                    ServiceMethod="GetCompletionList" ServicePath="" TargetControlID="txtSupplierName"
                                                                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                </cc1:AutoCompleteExtender>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-12" id="trCustomer" runat="server" visible="false">
                                                                                <asp:Label ID="lblCustomer" runat="server" Text="<%$ Resources:Attendance,Customer %>" />
                                                                                <a style="color: Red">*</a>
                                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCustomerName" ErrorMessage="<%$ Resources:Attendance,Enter Customer Name %>"></asp:RequiredFieldValidator>

                                                                                <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                    AutoPostBack="True" OnTextChanged="txtCustomerName_TextChanged" />
                                                                                <cc1:AutoCompleteExtender ID="txtCustomerName_AutoCompleteExtender" runat="server"
                                                                                    CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                                                                    ServiceMethod="GetCompletionListCustomer" ServicePath="" TargetControlID="txtCustomerName"
                                                                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                </cc1:AutoCompleteExtender>
                                                                                <br />
                                                                            </div>

                                                                            <div id="trEmployee" runat="server" visible="false" class="col-md-12">
                                                                                <asp:Label ID="lblEmployeeName" runat="server" Text="Employee" />
                                                                                <a style="color: Red">*</a>
                                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RFVEmployee" ValidationGroup="Save"
                                                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEmployeeName" ErrorMessage="Enter Employee"></asp:RequiredFieldValidator>

                                                                                <asp:TextBox ID="txtEmployeeName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                    AutoPostBack="True" OnTextChanged="txtEmployeeName_TextChanged" />
                                                                                <cc1:AutoCompleteExtender ID="txtEmployeeName_AutoCompleteExtender" runat="server"
                                                                                    CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                                                                    ServiceMethod="GetCompletionListEmployee" ServicePath="" TargetControlID="txtEmployeeName"
                                                                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                </cc1:AutoCompleteExtender>
                                                                                <br />
                                                                            </div>

                                                                            <div id="trVehicle" runat="server" visible="false" class="col-md-12">
                                                                                <asp:Label ID="lblVehicleName" runat="server" Text="Vehicle" />
                                                                                <a style="color: Red">*</a>
                                                                                <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RFVVehicle" ValidationGroup="Save"
                                                                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtVehicleName" ErrorMessage="Enter Vehicle"></asp:RequiredFieldValidator>

                                                                                <asp:TextBox ID="txtVehicleName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                    AutoPostBack="True" OnTextChanged="txtVehicleName_TextChanged" />
                                                                                <cc1:AutoCompleteExtender ID="txtVehicleName_AutoCompleteExtender" runat="server"
                                                                                    CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                                                                    ServiceMethod="GetCompletionListVehicle" ServicePath="" TargetControlID="txtVehicleName"
                                                                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                </cc1:AutoCompleteExtender>
                                                                                <br />
                                                                            </div>

                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="lblReceiveAmount" runat="server" Text="<%$ Resources:Attendance,Receive Amount%>" />
                                                                                <asp:TextBox ID="txtReceiveAmount" runat="server" CssClass="form-control" AutoPostBack="True"
                                                                                    OnTextChanged="txtReceiveAmount_TextChanged" />
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                                    TargetControlID="txtReceiveAmount" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <br />
                                                                            </div>

                                                                            <div class="col-md-12">
                                                                                <asp:Label ID="lblDetailNarration" runat="server" Text="<%$ Resources:Attendance,Narration%>" />
                                                                                <asp:TextBox ID="txtDetailNarration" TextMode="MultiLine" runat="server"
                                                                                    CssClass="form-control" />
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
                                                                                <asp:Label ID="lblForeignamount" runat="server" Text="<%$ Resources:Attendance,Foreign Amount %>" />
                                                                                <asp:TextBox ID="txtForeignamount" runat="server" CssClass="form-control" AutoPostBack="True"
                                                                                    OnTextChanged="txtForeignamount_TextChanged"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                                    TargetControlID="txtForeignamount" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <br />
                                                                                <asp:LinkButton ID="btnDetailSave" runat="server" ValidationGroup="Add"
                                                                                    ToolTip="<%$ Resources:Attendance,Add %>" OnClick="btnDetailSave_Click"><i class="fa fa-plus-square" style="font-size:35px;padding:5px;margin-top: -5px;"></i></asp:LinkButton>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-12" style="overflow: auto; max-height: 500px;">
                                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvDetail" runat="server" Width="100%" AutoGenerateColumns="False">

                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="Delete">
                                                                                            <ItemTemplate>
                                                                                                <asp:ImageButton ID="imgBtnDetailDelete" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                                    Height="16px" ImageUrl="~/Images/Erase.png" Width="16px" OnCommand="imgBtnDetailDelete_Command" />
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,S No. %>">
                                                                                            <ItemTemplate>
                                                                                                <%#Container.DataItemIndex+1 %>
                                                                                                <asp:Label ID="lblSNo" Visible="false" runat="server" Text='<%#Eval("Trans_Id") %>' />
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Account Name %>">
                                                                                            <ItemTemplate>
                                                                                                <asp:HiddenField ID="hdngvAccountNo" runat="server" Value='<%#Eval("Account_No") %>' />
                                                                                                <asp:Label ID="lblgvAccountName" runat="server" Text='<%#GetAccountNameByTransId(Eval("Account_No").ToString())%>' />
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Other Account Name">
                                                                                            <ItemTemplate>
                                                                                                <asp:HiddenField ID="hdngvOtherAccountNo" runat="server" Value='<%#Eval("Other_Account_No") %>' />
                                                                                                <%--<asp:Label ID="lblgvOtherAccountName" runat="server" Text='<%#GetOtherAccountNameForDetail(Eval("Other_Account_No").ToString(),Eval("Type").ToString())%>' />--%>
                                                                                                <asp:Label ID="lblgvOtherAccountName" runat="server" Text='<%#Ac_ParameterMaster.GetOtherAccountNameForDetail(Eval("Other_Account_No").ToString(),Eval("Account_No").ToString(),Session["CompId"].ToString(),Session["DBConnection"].ToString())%>' />
                                                                                                <asp:HiddenField ID="hdngvType" runat="server" Value='<%#Eval("Type") %>' />
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Narration %>">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblgvNarration" runat="server" Text='<%#Eval("Narration") %>' />
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Paid Amount %>">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblgvDebitAmount" runat="server" Text='<%#Eval("Credit_Amount") %>' />
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Currency %>">
                                                                                            <ItemTemplate>
                                                                                                <asp:HiddenField ID="hdngvCurrencyId" runat="server" Value='<%#Eval("Currency_Id") %>' />
                                                                                                <asp:Label ID="lblgvCurrency" runat="server" Text='<%#GetCurrencyName(Eval("Currency_Id").ToString()) %>' />
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Foreign Amount %>">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblgvForeignAmount" runat="server" Text='<%#Eval("Foreign_Amount") %>' />
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Exchange Rate %>">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblgvExchangeRate" runat="server" Text='<%#Eval("Exchange_Rate") %>' />
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                        </asp:TemplateField>
                                                                                         <asp:TemplateField HeaderText="Cost Center">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblgvCostCenter" runat="server" Text='<%#Eval("CostCenter") %>' />
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                        </asp:TemplateField>
                                                                                    </Columns>

                                                                                    <PagerStyle CssClass="pagination-ys" />

                                                                                </asp:GridView>
                                                                                <asp:HiddenField ID="hdnAccountNo" runat="server" />
                                                                                <br />
                                                                            </div>

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblTotalReceivedAmount" runat="server" Text="<%$ Resources:Attendance, Total Received Amount%>" />
                                                        <asp:TextBox ID="txtTotalReceivedAmount" runat="server" CssClass="form-control" ReadOnly="true" />
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblCashBank" runat="server" Text="<%$ Resources:Attendance,Receive Cash/Bank%>" />
                                                        <asp:TextBox ID="txtCashBank" runat="server" BackColor="#eeeeee" CssClass="form-control"
                                                            OnTextChanged="txtPaymentTo_textChnaged" AutoPostBack="true" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListReceiveAccountName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtCashBank"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnVoucherSave" runat="server" Text="<%$ Resources:Attendance,Save %>"
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
                    <div class="tab-pane" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server">
                            <ContentTemplate>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div2" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label7" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecordsBin" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldNameBin" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Voucher No %>" Value="Voucher_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Voucher Type %>" Value="Voucher_Type"
                                                            Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Finance Code %>" Value="Finance_Code"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlOptionBin" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbindBin">
                                                        <asp:TextBox ID="txtValueBin" runat="server" CssClass="form-control" placeholder="Search from Content"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="btnbindBin" runat="server" CausesValidation="False"
                                                        OnClick="btnbindBin_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="btnRefreshBin" runat="server" CausesValidation="False"
                                                        OnClick="btnRefreshBin_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="imgBtnRestore" CausesValidation="False" Style="width: 33px;"
                                                        runat="server" ImageUrl="~/Images/active.png" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid" <%= GvVoucherBin.Rows.Count>0?"style='display:block'":"style='display:none'"%>>

                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvVoucherBin" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server"
                                                        AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvVoucherBin_PageIndexChanging"
                                                        OnSorting="GvVoucherBin_OnSorting" AllowSorting="true">
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
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Voucher No %>" SortExpression="Voucher_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvVoucherNo" runat="server" Text='<%#Eval("Voucher_No") %>' />
                                                                    <asp:Label ID="lblgvTransId" runat="server" Visible="false" Text='<%#Eval("Trans_Id") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Voucher Date %>" SortExpression="Voucher_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvVoucherDate" runat="server" Text='<%#GetDate(Eval("Voucher_Date").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Voucher Type %>" SortExpression="Voucher_Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvVoucherType" runat="server" Text='<%#Eval("Voucher_Type") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Voucher Amount%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvExchangerate" runat="server" Text='<%#GetVoucherAmount(Eval("Trans_Id").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Finance Code %>" SortExpression="Finance_Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvFinanceCode" runat="server" Text='<%#Eval("Finance_Code") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Currency %>" SortExpression="Currency_Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCurrency" runat="server" Text='<%#GetCurrencyName(Eval("Currency_Id").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Created By %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCreatedBy" runat="server" Text='<%#GetEmployeeNameByEmpCode(Eval("CreatedBy").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Modified By %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvModifiedBy" runat="server" Text='<%#GetEmployeeNameByEmpCode(Eval("ModifiedBy").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
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
    <asp:Panel ID="PnlNewEdit" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PnlList" runat="server" Visible="false"></asp:Panel>
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
        $(document).ready(function () {
            maintainScrollPosition();
        });
        function pageLoad() {
            maintainScrollPosition();
        }
        function maintainScrollPosition() {
            $("#dvScroll").scrollTop($('#<%=hfScrollPosition.ClientID%>').val());
        }
        function setScrollPosition(scrollValue) {

            $('#<%=hfScrollPosition.ClientID%>').val(scrollValue);
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
        function showUcControlsSettings() {
            $('#ControlSettingModal').modal('show');
        }
        function getReportData(transId) {
            $("#prgBar").css("display", "block");
            document.getElementById('<%= reportSystem.FindControl("hdnTransId").ClientID %>').value = transId;
            setReportData();
        }

    </script>

    <script src="../Script/ReportSystem.js"></script>
    <script src="../Script/master.js"></script>
</asp:Content>
