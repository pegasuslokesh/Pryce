<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="ReNew.aspx.cs" Inherits="GeneralLedger_ReNew" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/ucControlsSetting.ascx" TagName="ucCtlSetting" TagPrefix="UC" %>
<%@ Register Src="~/WebUserControl/ReportSystem.ascx" TagName="ReportSystem" TagPrefix="RS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <script>
        function FUAll_UploadComplete() {

        }
        function FUAll_UploadError() {

        }
        function FUAll_UploadStarted() {

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-dollar-sign"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Reconciliation Finance%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Finance Setup%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,General Ledger%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Reconciliation Finance%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="btnList_Click" Text="List" />
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" OnClick="btnNew_Click" Text="New" />
            <asp:Button ID="Btn_Report" Style="display: none;" runat="server" OnClick="btnReport_Click" Text="Report" />
            <asp:Button ID="Btn_Voucher_Detail" Style="display: none;" runat="server" data-toggle="modal" data-target="#Voucher_Detail" Text="View Modal" />
            <asp:Button ID="Btn_Vooucher_Details_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Vooucher_Details_Modal" Text="View Modal" />
            <asp:HiddenField runat="server" ID="hdnCanPrint" />
            <asp:HiddenField runat="server" ID="hdnCanView" />
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
                    <li id="Li_Report"><a href="#Report" onclick="Li_Tab_Report()" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Report %>"></asp:Label></a></li>
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
					<asp:Label ID="lblTotalRecordsList" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Reconciliation No %>" Value="ReconcilationNo"
                                                            Selected="True"></asp:ListItem>
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
                                                <div class="col-lg-3">
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
                                                    <asp:LinkButton ID="btnGvListSetting" ImageAlign="Right" ToolTip="List Settings" runat="server" OnClick="btnGvListSetting_Click" Visible="false"><span class="fa fa-wrench"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid" <%= GvReconcile.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvReconcile" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server"
                                                        AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvReconcile_PageIndexChanging"
                                                        AllowSorting="True" OnSorting="GvReconcile_Sorting">

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
                                                                                    Visible="true" Height="20px" ToolTip="<%$ Resources:Attendance,View %>"
                                                                                    OnCommand="lnkViewDetail_Command" CausesValidation="False"><i class="fa fa-eye"></i>View</asp:LinkButton>
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

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Reconciliation No %>" SortExpression="ReconcilationNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvReconcilationNo" runat="server" Text='<%#Eval("ReconcilationNo") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Reconciled By %>" SortExpression="Reconciled_By">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvReconciled_By" runat="server" Text='<%#GetEmployeeNameByEmpId(Eval("Reconciled_By").ToString())%>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Reconciliation Date %>"
                                                                SortExpression="ReconcileDate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvReconcileDate" Width="80px" runat="server" Text='<%#GetDate(Eval("ReconcileDate").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Account Name %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvAccountName" runat="server" Text='<%#GetAccountNameByTransId(Eval("Account_No").ToString())%>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Other Account %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvOtherAccount" runat="server" Text='<%#Ac_ParameterMaster.GetOtherAccountNameForDetail(Eval("Other_Account_No").ToString(),Eval("Account_No").ToString(),Session["CompId"].ToString(),Session["DBConnection"].ToString())%>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Reconciled %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvTotalReconciledRecord" runat="server" Text='<%#Eval("Total_Reconciled_Record") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,  Reconciled Amount %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvTotalReconciledAmount" runat="server" Text='<%#Eval("Total_Reconciled_Amount") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Conflicted%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvTotalNotReconciledRecord" runat="server" Text='<%#Eval("Total_Not_Reconciled_Record") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Conflicted Amount %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvTotalNotReconciledAmount" runat="server" Text='<%#Eval("Total_Not_Reconciled_Amount") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Remarks %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvRemarks" runat="server" Text='<%# Eval("Remarks") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Document %>">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkAddDocument" runat="server" Text="Document Upload" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        OnCommand="lnkAddDocument_Command" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                    <asp:HiddenField ID="hdnReconcileId" runat="server" Value="0" />
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
                                <asp:Panel ID="PnlNewEdit" runat="server">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="box box-primary">
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <div class="col-md-12">
                                                            <asp:ImageButton ID="btnControlsSetting" ImageAlign="Right" ToolTip="Controls Setting" runat="server" ImageUrl="~/Images/setting.png" OnClick="btnControlsSetting_Click" Style="width: 32px; height: 32px" Visible="false" />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblReconciled" runat="server" Text="<%$ Resources:Attendance,Status%>" />
                                                            <asp:DropDownList ID="ddlReconciled" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Not Reconciled %>" Value="NR" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Reconciled%>" Value="R"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,All %>" Value="A"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblReconcilationNo" runat="server" Text="<%$ Resources:Attendance,Reconciliation No %>" />
                                                            <asp:TextBox ID="txtReconcilationNo" runat="server" CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12">
                                                            <asp:Label ID="lblAccountName" runat="server" Text="<%$ Resources:Attendance,Account Name%>" />
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtAccountName" ErrorMessage="<%$ Resources:Attendance,Enter Account Name%>"></asp:RequiredFieldValidator>


                                                            <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                OnTextChanged="txtAccountName_TextChanged" BackColor="#eeeeee" />
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtAccountName"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>
                                                        <div id="trSupplier" runat="server" visible="false" class="col-md-6">
                                                            <asp:Label ID="lblSupplierName" runat="server" Text="<%$ Resources:Attendance,Supplier %>" />
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
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

                                                        <div class="col-md-6" id="trCustomer" runat="server" visible="false">
                                                            <asp:Label ID="lblCustomer" runat="server" Text="<%$ Resources:Attendance,Customer %>" />
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCustomerName" ErrorMessage="<%$ Resources:Attendance,Enter Customer%>"></asp:RequiredFieldValidator>

                                                            <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                AutoPostBack="True" OnTextChanged="txtCustomerName_TextChanged" />
                                                            <cc1:AutoCompleteExtender ID="txtCustomerName_AutoCompleteExtender" runat="server"
                                                                CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                                                ServiceMethod="GetCompletionListCustomer" ServicePath="" TargetControlID="txtCustomerName"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>

                                                        <div id="trEmployee" runat="server" visible="false" class="col-md-6">
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
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblFromDate" runat="server" Text="<%$ Resources:Attendance,From Date %>" />
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator8" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtFromDate" ErrorMessage="<%$ Resources:Attendance,Enter From Date%>"></asp:RequiredFieldValidator>

                                                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" />
                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender_VoucherDate" runat="server" TargetControlID="txtFromDate" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblToDate" runat="server" Text="<%$ Resources:Attendance,To Date %>" />
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator9" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtToDate" ErrorMessage="<%$ Resources:Attendance,Enter To Date%>"></asp:RequiredFieldValidator>

                                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" />
                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" TargetControlID="txtToDate" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblSVoucherType" runat="server" Text="<%$ Resources:Attendance,Voucher Type %>" />
                                                            <asp:DropDownList ID="ddlSVoucherType" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="--Select--" Value="--Select--"></asp:ListItem>
                                                                <asp:ListItem Text="Purchase Invoice" Value="PI"></asp:ListItem>
                                                                <asp:ListItem Text="Purchase Return" Value="PR"></asp:ListItem>
                                                                <asp:ListItem Text="Journal Vouchers" Value="JV"></asp:ListItem>
                                                                <asp:ListItem Text="Payment Vouchers" Value="PV"></asp:ListItem>
                                                                <asp:ListItem Text="Sales Invoice" Value="SI"></asp:ListItem>
                                                                <asp:ListItem Text="Receive Vouchers" Value="RV"></asp:ListItem>
                                                                <asp:ListItem Text="Sales Return" Value="SR"></asp:ListItem>
                                                                <asp:ListItem Text="Supplier Payment Vouchers" Value="SPV"></asp:ListItem>
                                                                <asp:ListItem Text="Customer Receive Vouchers" Value="CRV"></asp:ListItem>
                                                                <asp:ListItem Text="Customer Debit Note" Value="CDN"></asp:ListItem>
                                                                <asp:ListItem Text="Supplier Debit Note" Value="SDN"></asp:ListItem>
                                                                <asp:ListItem Text="Customer Credit Note" Value="CCN"></asp:ListItem>
                                                                <asp:ListItem Text="Supplier Credit Note" Value="SCN"></asp:ListItem>
                                                                <asp:ListItem Text="PDC Customer" Value="PDC"></asp:ListItem>
                                                                <asp:ListItem Text="PDC Supplier" Value="PDS"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:HiddenField ID="hdnNOAId" runat="server" Value="0" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <br />
                                                            <asp:Label ID="lblTotalRecords" CssClass="form-control" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                                                            <br />
                                                        </div>

                                                        <div class="col-md-12">
                                                            <div class="col-md-2">
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:ListBox ID="lstLocation" runat="server" Style="width: 100%;" Height="200px"
                                                                    SelectionMode="Multiple" CssClass="form-control" Font-Names="Trebuchet MS" Font-Size="Small"
                                                                    ForeColor="Gray"></asp:ListBox>
                                                            </div>
                                                            <div class="col-lg-2" style="text-align: center">
                                                                <div style="margin-top: 35px; margin-bottom: 35px;" class="btn-group-vertical">

                                                                    <asp:Button ID="btnPushDept" runat="server" CssClass="btn btn-info" Text=">" OnClick="btnPushDept_Click" />

                                                                    <asp:Button ID="btnPullDept" Text="<" runat="server" CssClass="btn btn-info" OnClick="btnPullDept_Click" />

                                                                    <asp:Button ID="btnPushAllDept" Text=">>" OnClick="btnPushAllDept_Click" runat="server" CssClass="btn btn-info" />

                                                                    <asp:Button ID="btnPullAllDept" Text="<<" OnClick="btnPullAllDept_Click" runat="server" CssClass="btn btn-info" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:ListBox ID="lstLocationSelect" runat="server" Style="width: 100%;" Height="200px"
                                                                    SelectionMode="Multiple" CssClass="form-control" Font-Names="Trebuchet MS" Font-Size="Small"
                                                                    ForeColor="Gray"></asp:ListBox>
                                                            </div>
                                                            <div class="col-md-2">
                                                            </div>
                                                            <br />
                                                        </div>

                                                        <div class="col-md-12" style="text-align: center">
                                                            <br />
                                                            <asp:Button ID="btnGetReport" ValidationGroup="Save" runat="server" Text="<%$ Resources:Attendance,Execute %>"
                                                                CssClass="btn btn-primary" OnClick="btnGetReport_Click" />

                                                            <asp:Button ID="btnReconciled" runat="server" Text="<%$ Resources:Attendance,Update %>"
                                                                CssClass="btn btn-success" OnClick="btnReconciled_Click" />
                                                            <cc1:ConfirmButtonExtender ID="CnfirmGeneratePayroll" runat="server" TargetControlID="btnReconciled"
                                                                ConfirmText="<%$ Resources:Attendance,Are you sure To update that record?%>">
                                                            </cc1:ConfirmButtonExtender>

                                                            <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                                CssClass="btn btn-danger" OnClick="btnCancel_Click" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12">
                                                            <asp:Label ID="lblRemarks" runat="server" Text="<%$ Resources:Attendance,Remarks %>" />
                                                            <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12" runat="server" id="scrollArea" onscroll="SetDivPosition()" style="overflow: auto; max-height: 500px;">
                                                            <asp:HiddenField ID="hfScrollPosition" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdfCurrentRow" runat="server" />
                                                            <asp:HiddenField ID="hdnNewVoucherId" runat="server" Value="0" />
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvVoucher" ClientIDMode="Static" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server"
                                                                AutoGenerateColumns="False" Width="100%" AllowPaging="False" OnPageIndexChanging="GvVoucher_PageIndexChanging"
                                                                AllowSorting="True" OnSorting="GvVoucher_Sorting">

                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Yes %>">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkReconciledYes" runat="server" AutoPostBack="true" OnCheckedChanged="chkReconciledYes_CheckedChanged" OnClick="SetSelectedRow(this);" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,No %>">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkReconciledNot" runat="server" AutoPostBack="true" OnCheckedChanged="chkReconciledNot_CheckedChanged" OnClick="SetSelectedRow(this);" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Reconciled By %>" SortExpression="ReconciledBy">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvReconciledBy" runat="server" Text='<%#GetEmployeeNameByEmpId(Eval("ReconciledBy").ToString())%>' />
                                                                            <asp:Label ID="lblgvTransId" runat="server" Visible="false" Text='<%#Eval("Trans_Id") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Reconciled Date %>" SortExpression="ReconciledDate">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvReconciledDate" runat="server" Text='<%#Eval("ReconciledDate") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Voucher No %>" SortExpression="Voucher_No">
                                                                        <ItemTemplate>
                                                                            <%-- <asp:Label ID="lblgvVoucherNo" runat="server" Text='<%#Eval("Voucher_No") %>' />--%>
                                                                            <asp:LinkButton ID="lnkgvVoucherNo" Font-Bold="true" ForeColor="Black" runat="server"
                                                                                Text='<%#Eval("Voucher_No") %>' CommandArgument='<%# Eval("HeaderTransId") %>'
                                                                                ToolTip="<%$ Resources:Attendance,View %>" OnCommand="lnkViewVoucherDetail_Command" OnClientClick="SetSelectedRow(this);" CausesValidation="False" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Voucher Date %>" SortExpression="Voucher_Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvVoucherDate" Width="80px" runat="server" Text='<%#GetDate(Eval("Voucher_Date").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Voucher Type %>" SortExpression="Voucher_Type">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvVoucherType" runat="server" Text='<%#Eval("Voucher_Type") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Debit Amount%>" SortExpression="Debit_Amount">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvDebitAmount" runat="server" Text='<%#Eval("Debit_Amount") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Credit Amount %>" SortExpression="Credit_Amount">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvCreditAmount" runat="server" Text='<%#Eval("Credit_Amount") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Foreign Amount%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvForeignAmount" runat="server" Text='<%#Eval("Foreign_Amount") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Narration %>" SortExpression="Narration">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvNarration" Width="140px" runat="server" Text='<%#Eval("Narration") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Remarks %>">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtgvRemarks" Width="140px" runat="server" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>

                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>

                                                            <br />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <div id="PnlView" runat="server" visible="false" class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblReconciledByView" runat="server" Text="<%$ Resources:Attendance,Reconciled By %>" />
                                                        <asp:TextBox ID="txtReconciledByView" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblReconcilationNoView" runat="server" Text="<%$ Resources:Attendance,Reconciliation No %>" />
                                                        <asp:TextBox ID="txtReconcilationNoView" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblReconciledDateView" runat="server" Text="<%$ Resources:Attendance,Reconciled Date%>" />
                                                        <asp:TextBox ID="txtReconciledDateView" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblAccountNameView" runat="server" Text="<%$ Resources:Attendance,Account Name%>" />
                                                        <asp:TextBox ID="txtAccountNameView" runat="server" CssClass="form-control" BackColor="#eeeeee" />
                                                        <br />
                                                    </div>
                                                    <div id="tr1ViewSupplier" runat="server" visible="false" class="col-md-12">
                                                        <asp:Label ID="lblSupplierNameView" runat="server" Text="<%$ Resources:Attendance,Supplier %>" />
                                                        <asp:TextBox ID="txtSupplierNameView" runat="server" CssClass="form-control" BackColor="#eeeeee" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" id="tr2ViewCustomer" runat="server" visible="false">
                                                        <asp:Label ID="lblCustomerNameView" runat="server" Text="<%$ Resources:Attendance,Customer %>" />
                                                        <asp:TextBox ID="txtCustomerNameView" runat="server" CssClass="form-control" BackColor="#eeeeee" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" id="tr3ViewEmployee" runat="server" visible="false">
                                                        <asp:Label ID="lblEmployeeNameView" runat="server" Text="Employee" />
                                                        <asp:TextBox ID="txtEmployeeNameView" runat="server" CssClass="form-control" BackColor="#eeeeee" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblTotalRRecordView" runat="server" Text="<%$ Resources:Attendance,Reconciled Record %>" />
                                                        <asp:TextBox ID="txtTotalRRecordView" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblTotalRAmountView" runat="server" Text="<%$ Resources:Attendance,Reconciled Amount%>" />
                                                        <asp:TextBox ID="txtTotalRAmountView" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblTotalNRRecordView" runat="server" Text="<%$ Resources:Attendance,Conflicted %>" />
                                                        <asp:TextBox ID="txtTotalNRRecordView" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblTotalNRAmountView" runat="server" Text="<%$ Resources:Attendance,Conflicted%>" />
                                                        <asp:TextBox ID="txtTotalNRAmountView" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblRemarksView" runat="server" Text="<%$ Resources:Attendance,Remarks %>" />
                                                        <asp:TextBox ID="txtRemarksView" runat="server" TextMode="MultiLine"
                                                            CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <br />
                                                        <asp:Button ID="Button7" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                            CssClass="btn btn-primary" OnClick="btnCancelView_Click" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <br />
                                                        <div style="overflow: auto; max-height: 500px;">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvView" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server"
                                                                AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvView_PageIndexChanging"
                                                                AllowSorting="True">

                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Reconciled Status %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvReconciledStatus" runat="server" Text='<%#GetStatus(Eval("Is_Reconciled").ToString())%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Voucher No %>" SortExpression="Voucher_No">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvVoucherNo" runat="server" Text='<%#Eval("Voucher_No") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Voucher Date %>" SortExpression="Voucher_Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvVoucherDate" Width="80px" runat="server" Text='<%#GetDate(Eval("Voucher_Date").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Voucher Type %>" SortExpression="Voucher_Type">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvVoucherType" runat="server" Text='<%#Eval("Voucher_Type") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Debit Amount%>" SortExpression="Debit_Amount">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvDebitAmount" runat="server" Text='<%#Eval("Debit_Amount") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Credit Amount %>" SortExpression="Credit_Amount">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvCreditAmount" runat="server" Text='<%#Eval("Credit_Amount") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Remarks %>" SortExpression="Remarks">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvNarration" Width="140px" runat="server" Text='<%#Eval("Remarks") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>

                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <br />
                                                        <div style="overflow: auto; max-height: 500px;">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvUploadedDocument" PageSize="5" runat="server" AutoGenerateColumns="False"
                                                                Width="100%">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Download %>" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" OnCommand="OnDownloadCommand"
                                                                                CommandArgument='<%#Eval("Trans_id") %>'></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                ImageUrl="~/Images/Erase.png" OnCommand="IbtnDeleteDocument_Command" ToolTip="<%$ Resources:Attendance,Delete %>" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Document Name %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDocumentName" runat="server" Text='<%# Eval("DocumentName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,File Name %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblFileName" runat="server" Text='<%# Eval("File_Name") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                </Columns>


                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>
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
                    <div class="tab-pane" id="Report">
                        <asp:UpdatePanel ID="Update_Report" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblReconciledNoReport" runat="server" Text="<%$ Resources:Attendance,Reconciliation No%>" />
                                                        <asp:TextBox ID="txtReconciledNoReport" runat="server" CssClass="form-control" BackColor="#eeeeee" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListReconcileNo" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtReconciledNoReport"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblReconciledByReport" runat="server" Text="<%$ Resources:Attendance,Reconciled By%>" />
                                                        <a style="color: Red">*</a>

                                                        <asp:TextBox ID="txtReconciledByReport" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            AutoPostBack="true" OnTextChanged="txtReconciledByReport_TextChanged" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender_Employee" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtReconciledByReport"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblAccountNameReport" runat="server" Text="<%$ Resources:Attendance,Account Name%>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="OnReport"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtAccountNameReport" ErrorMessage="<%$ Resources:Attendance,Enter Account Name%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtAccountNameReport" runat="server" CssClass="form-control" AutoPostBack="true"
                                                            OnTextChanged="txtAccountNameReport_TextChanged" BackColor="#eeeeee" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtAccountNameReport"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div id="trSupplierReport" runat="server" visible="false" class="col-md-6">
                                                        <asp:Label ID="lblSupplierNameReport" runat="server" Text="<%$ Resources:Attendance,Supplier %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator6" ValidationGroup="OnReport"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSupplierNameReport" ErrorMessage="<%$ Resources:Attendance,Enter Supplier%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtSupplierNameReport" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            AutoPostBack="True" OnTextChanged="txtSupplierNameReport_TextChanged" />
                                                        <cc1:AutoCompleteExtender ID="txtSupplierNameReport_AutoCompleteExtender" runat="server"
                                                            CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                                            ServiceMethod="GetCompletionList" ServicePath="" TargetControlID="txtSupplierNameReport"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" id="trCustomerReport" runat="server" visible="false">
                                                        <asp:Label ID="lblCustomerReport" runat="server" Text="<%$ Resources:Attendance,Customer %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator7" ValidationGroup="OnReport"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCustomerNameReport" ErrorMessage="<%$ Resources:Attendance,Enter Customer%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtCustomerNameReport" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            AutoPostBack="True" OnTextChanged="txtCustomerNameReport_TextChanged" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" CompletionInterval="100"
                                                            DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListCustomer"
                                                            ServicePath="" TargetControlID="txtCustomerNameReport" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblFromDateReport" runat="server" Text="<%$ Resources:Attendance,From Date %>" />
                                                        <asp:TextBox ID="txtFromDateReport" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender2" runat="server" TargetControlID="txtFromDateReport" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblToDateReport" runat="server" Text="<%$ Resources:Attendance,To Date %>" />
                                                        <asp:TextBox ID="txtToDateReport" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender3" runat="server" TargetControlID="txtToDateReport" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:RadioButton ID="rbHeaderReport" GroupName="Report" runat="server" Text="Header Report" />
                                                        <asp:RadioButton ID="rbDetailReport" Style="margin-left: 20px;" GroupName="Report" runat="server" Text="Detail Report" />
                                                        <asp:RadioButton ID="rbByVoucherReport" Style="margin-left: 20px;" GroupName="Report" runat="server" Text="By Voucher Report" />
                                                        <br />
                                                    </div>
                                                    <div style="text-align: center" class="col-md-12">
                                                        <asp:Button ID="btnReportAll" runat="server" Text="<%$ Resources:Attendance,Execute %>" ValidationGroup="OnReport"
                                                            CssClass="btn btn-primary" OnClick="btnReportAll_Click" />
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
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="Voucher_Detail" tabindex="-1" role="dialog" aria-labelledby="Voucher_DetailLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Voucher_DetailLabel">
                        <asp:Label ID="Label25" runat="server" Text="Voucher Detail" Font-Bold="true"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Voucher_Detail" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblVFinanceCode" runat="server" Text="<%$ Resources:Attendance,Finance Code %>" />
                                                    <asp:TextBox ID="txtVFinanceCode" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblVLocation" runat="server" Text="<%$ Resources:Attendance,Location To %>" />
                                                    <asp:TextBox ID="txtVToLocation" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblVDepartment" runat="server" Text="<%$ Resources:Attendance,Department %>" />
                                                    <asp:TextBox ID="txtVDepartment" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblVVoucherType" runat="server" Text="<%$ Resources:Attendance,Voucher Type %>" />
                                                    <asp:DropDownList ID="ddlVVoucherType" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="--Select--" Value="--Select--"></asp:ListItem>
                                                        <asp:ListItem Text="Purchase Invoice" Value="PI"></asp:ListItem>
                                                        <asp:ListItem Text="Purchase Return" Value="PR"></asp:ListItem>
                                                        <asp:ListItem Text="Journal Vouchers" Value="JV"></asp:ListItem>
                                                        <asp:ListItem Text="Payment Vouchers" Value="PV"></asp:ListItem>
                                                        <asp:ListItem Text="Sales Invoice" Value="SI"></asp:ListItem>
                                                        <asp:ListItem Text="Receive Vouchers" Value="RV"></asp:ListItem>
                                                        <asp:ListItem Text="Sales Return" Value="SR"></asp:ListItem>
                                                        <asp:ListItem Text="Supplier Payment Vouchers" Value="SPV"></asp:ListItem>
                                                        <asp:ListItem Text="Customer Receive Vouchers" Value="CRV"></asp:ListItem>
                                                        <asp:ListItem Text="Customer Debit Note" Value="CDN"></asp:ListItem>
                                                        <asp:ListItem Text="Supplier Debit Note" Value="SDN"></asp:ListItem>
                                                        <asp:ListItem Text="Customer Credit Note" Value="CCN"></asp:ListItem>
                                                        <asp:ListItem Text="Supplier Credit Note" Value="SCN"></asp:ListItem>
                                                        <asp:ListItem Text="PDC Customer" Value="PDC"></asp:ListItem>
                                                        <asp:ListItem Text="PDC Supplier" Value="PDS"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <%--<asp:TextBox ID="txtVoucherType" runat="server" CssClass="form-control" />--%>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblVVoucherNo" runat="server" Text="<%$ Resources:Attendance,Voucher No. %>" />
                                                    <a style="color: Red">*</a>
                                                    <%--  <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator10" ValidationGroup="Save"
                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtVVoucherNo" ErrorMessage="<%$ Resources:Attendance,Enter Voucher No%>"></asp:RequiredFieldValidator>--%>

                                                    <asp:TextBox ID="txtVVoucherNo" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblVVoucherDate" runat="server" Text="<%$ Resources:Attendance,Voucher Date %>" />
                                                    <asp:TextBox ID="txtVVoucherDate" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender13" runat="server" TargetControlID="txtVVoucherDate"
                                                        Format="dd/MMM/yyyy" />
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <asp:RadioButton ID="rbVCashPayment" runat="server" Text="Cash Payment"
                                                        GroupName="Pay" Enabled="false" OnCheckedChanged="rbVCashPayment_CheckedChanged"
                                                        AutoPostBack="true" />
                                                    <asp:RadioButton ID="rbVChequePayment" runat="server" Text="Cheque Payment"
                                                        GroupName="Pay" Enabled="false" OnCheckedChanged="rbVCashPayment_CheckedChanged"
                                                        AutoPostBack="true" />
                                                    <asp:CheckBox ID="chkVReconcile" Visible="false" runat="server" Text="<%$ Resources:Attendance, Reconcile%>" />
                                                    <br />
                                                </div>
                                                <div id="trVCheque1" runat="server" visible="false">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblVChequeIssueDate" runat="server" Text="Cheque Issue Date" />
                                                        <asp:TextBox ID="txtVChequeIssueDate" ReadOnly="true" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender14" runat="server" TargetControlID="txtVChequeIssueDate"
                                                            Format="dd/MMM/yyyy" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblVChequeClearDate" runat="server" Text="Cheque Clear Date" />
                                                        <asp:TextBox ID="txtVChequeClearDate" ReadOnly="true" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender15" runat="server" TargetControlID="txtVChequeClearDate"
                                                            Format="dd/MMM/yyyy" />
                                                        <br />
                                                    </div>
                                                </div>
                                                <div id="trVCheque2" runat="server" visible="false" class="col-md-6">
                                                    <asp:Label ID="lblVChequeNo" runat="server" Text="Cheque No." />
                                                    <asp:TextBox ID="txtVChequeNo" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblVReference" runat="server" Text="<%$ Resources:Attendance,Refrence%>" />
                                                    <asp:TextBox ID="txtVReference" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <div class="box box-warning box-solid">
                                                        <div class="box-header with-border">
                                                            <h3 class="box-title">
                                                                <asp:Label ID="Label26" runat="server" Text="<%$ Resources:Attendance,Detail View%>" />
                                                        </div>
                                                        <div class="box-body">
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <div style="overflow: auto; max-height: 500px;">
                                                                        <asp:HiddenField ID="hdnVAccountNo" runat="server" />
                                                                        <asp:HiddenField ID="hdnVAccountName" runat="server" />
                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvVDetail" ShowFooter="true" runat="server" Width="100%"
                                                                            AutoGenerateColumns="False">

                                                                            <Columns>
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
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Other Account No. %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvOtherAccountNo" runat="server" Text='<%#Ac_ParameterMaster.Get_Other_Account_Name(Eval("Other_Account_No").ToString(),Eval("Account_No").ToString(),Session["CompId"].ToString(),Session["DBConnection"].ToString())%>' />
                                                                                        <asp:HiddenField ID="hdnOtherAccountNo" runat="server" Value='<%#Eval("Other_Account_No") %>' />
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:Label ID="lblgvTotal" runat="server" Font-Bold="true"
                                                                                            Text="<%$ Resources:Attendance,Total%>" />
                                                                                    </FooterTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                    <FooterStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Narration %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvNarration" runat="server" Text='<%#Eval("Narration") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Debit Amount %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvDebitAmount" runat="server" Text='<%#Eval("Debit_Amount") %>' />
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:Label ID="lblgvDebitTotal" Font-Bold="true" runat="server" />
                                                                                    </FooterTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                    <FooterStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Credit Amount %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvCreditAmount" runat="server" Text='<%#Eval("Credit_Amount") %>' />
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:Label ID="lblgvCreditTotal" Font-Bold="true" runat="server" />
                                                                                    </FooterTemplate>
                                                                                    <FooterStyle HorizontalAlign="Center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Cost Center %>" Visible="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvCostCenter" runat="server" Text='<%#Eval("CostCenter_ID") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Employee %>" Visible="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="hdngvEmployeeId" runat="server" Value='<%#Eval("Emp_Id") %>' />
                                                                                        <asp:Label ID="lblgvEmployee" runat="server" Text='<%#GetEmployeeName(Eval("Emp_Id").ToString())%>' />
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
                                                                            </Columns>

                                                                            <PagerStyle CssClass="pagination-ys" />

                                                                        </asp:GridView>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
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
                <div class="modal-footer">
                    <asp:UpdatePanel ID="Update_Voucher_Detail_Button" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                            <asp:Button ID="Button2" runat="server" CausesValidation="False" CssClass="btn btn-primary" Visible="false"
                                OnClick="btnCancelPopLeave_Click" TabIndex="9" Text="<%$ Resources:Attendance,Cancel %>" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
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
    <div class="modal fade" id="Vooucher_Details_Modal" tabindex="-1" role="dialog" aria-labelledby="Vooucher_Details_ModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Vooucher_Details_ModalLabel">
                        <asp:Label ID="Label7" runat="server" Font-Size="14px" Font-Bold="true"
                            Text="<%$ Resources:Attendance,Voucher Detail %>"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Vooucher_Details_Modal" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblDocName" runat="server" Text="<%$ Resources:Attendance,Document Name %>"></asp:Label>
                                                    <asp:DropDownList ID="ddlDocumentName" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,File Upload %>" />
                                                    <div class="input-group" style="width: 100%;">
                                                        <cc1:AsyncFileUpload ID="UploadFile"
                                                            OnClientUploadStarted="FUAll_UploadStarted"
                                                            OnClientUploadError="FUAll_UploadError"
                                                            OnClientUploadComplete="FUAll_UploadComplete"
                                                            OnUploadedComplete="FUAll_FileUploadComplete"
                                                            runat="server" CssClass="form-control"
                                                            CompleteBackColor="White"
                                                            UploaderStyle="Traditional"
                                                            UploadingBackColor="#CCFFFF"
                                                            ThrobberID="FUAll_ImgLoader" Width="100%" />
                                                        <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                            <asp:Image ID="FUAll_Img_Right" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Allow.png" Style="display: none" />
                                                            <asp:Image ID="FUAll_Img_Wrong" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Delete1.png" Style="display: none" />
                                                            <asp:Image ID="FUAll_ImgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                        </div>
                                                        <div class="input-group-btn" style="width: 35px;">
                                                            <asp:ImageButton ID="BtnDocumentAdd" runat="server" CausesValidation="False" Height="29px"
                                                                ImageUrl="~/Images/add.png" OnClick="ImgButtonDocumentAdd_Click" Width="35px"
                                                                ToolTip="<%$ Resources:Attendance,Add %>" />
                                                        </div>
                                                    </div>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">

                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <div style="overflow: auto; max-height: 500px;">
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvFileMaster" PageSize="5" runat="server" AutoGenerateColumns="False"
                                                            Width="100%">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Download %>" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" OnCommand="OnDownloadCommand"
                                                                            CommandArgument='<%#Eval("Trans_id") %>'></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                            ImageUrl="~/Images/Erase.png" OnCommand="IbtnDeleteDocument_Command" ToolTip="<%$ Resources:Attendance,Delete %>" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Document Name %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDocumentName" runat="server" Text='<%# Eval("DocumentName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,File Name %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFileName" runat="server" Text='<%# Eval("File_Name") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                            </Columns>




                                                        </asp:GridView>
                                                        <asp:HiddenField ID="HiddenField4" runat="server" />
                                                    </div>
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
                <div class="modal-footer">
                    <asp:UpdatePanel ID="Update_Vooucher_Details_Modal_Button" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Vooucher_Details_Modal">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_Vooucher_Details_Modal_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress9" runat="server" AssociatedUpdatePanelID="Update_Voucher_Detail">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress10" runat="server" AssociatedUpdatePanelID="Update_Voucher_Detail_Button">
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

    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_New">
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
    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Report">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:Panel ID="PanelView1" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PanelView2" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlMenuList" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlMenuNew" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlMenuReport" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PnlList" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PnlReport" runat="server" Visible="false"></asp:Panel>
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
        function Li_Tab_Report() {
            document.getElementById('<%= Btn_Report.ClientID %>').click();
        }

        function Voucher_Detail_Popup() {
            document.getElementById('<%= Btn_Voucher_Detail.ClientID %>').click();
        }

        function Vooucher_Details_Modal_Popup() {
            document.getElementById('<%= Btn_Vooucher_Details_Modal.ClientID %>').click();
        }
        function setScrollAndRow() {
            try {
                debugger;
                var rowIndex = $('#<%= hdfCurrentRow.ClientID %>').val();
                var parent = document.getElementById('<%= GvVoucher.ClientID %>');
                var rowIndex = parseInt(rowIndex);
                parent.rows[rowIndex + 1].style.backgroundColor = "#A1DCF2";
                var h = document.getElementById("<%=hfScrollPosition.ClientID%>");
                document.getElementById("<%=scrollArea.ClientID%>").scrollTop = h.value;

            }
            catch (e) {

            }

        }

        function SetSelectedRow(lnk) {
            //Reference the GridView Row.
            debugger;
            try {
                var row = lnk.parentNode.parentNode;
                //var row = lnk;
                $('#<%= hdfCurrentRow.ClientID %>').val(row.rowIndex - 1);
                row.style.backgroundColor = "#A1DCF2";
            }
            catch (e) {

            }
        }

        function SetDivPosition() {
            try {
                var intY = document.getElementById("<%=scrollArea.ClientID%>").scrollTop;
                var h = document.getElementById("<%=hfScrollPosition.ClientID%>");
                h.value = intY;
            }
            catch (e) {

            }
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
