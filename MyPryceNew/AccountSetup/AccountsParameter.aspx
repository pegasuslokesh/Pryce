<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="AccountsParameter.aspx.cs" Inherits="AccountSetup_AccountsParameter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-cog"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Finance Parameter%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Finance Setup%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Finance Setup%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Finance Parameter%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        
        <ContentTemplate>
            <asp:Button ID="Btn_Location" Style="display: none;" runat="server" OnClick="btnLocationLevel_Click" Text="Location" />
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
                    <li id="Li_Location"><a href="#Location" onclick="Li_Tab_Location()" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Location %>"></asp:Label></a></li>
                    <li id="Li_Company" class="active"><a href="#Company" data-toggle="tab">
                        <i class="fa fa-file"></i>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Company %>"></asp:Label></a></li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane active" id="Company">
                        <asp:UpdatePanel ID="Update_Company" runat="server">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSave" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <div class="box box-primary">
                                                                <div class="box-header with-border">
                                                                    <h3 class="box-title">
                                                                        <asp:Label ID="Label35" runat="server" Text="<%$ Resources:Attendance,Purchase Section%>"></asp:Label></h3>
                                                                    <div class="box-tools pull-right">
                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                            <i class="fa fa-minus"></i>
                                                                        </button>
                                                                    </div>
                                                                </div>
                                                                <div class="box-body">
                                                                    <div class="form-group">
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblAcPurchaseInvoice" runat="server" Text="<%$ Resources:Attendance,Purchase Invoice%>" />
                                                                            <asp:TextBox ID="txtAcPurchaseInvoice" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtAcPurchaseInvoice"
                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblPurchaseRetrun" runat="server" Text="<%$ Resources:Attendance,Purchase Return%>" />
                                                                          
                                                                            <asp:TextBox ID="txtPurchaseReturn" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender14" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtPurchaseReturn"
                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblPOAdvanceDebit" runat="server" Text="<%$ Resources:Attendance,PO Advance Debit A/C%>" />
                                                                            <a style="color: Red">*</a>
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator15" ValidationGroup="Cmp_Save"
                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtPOAdvanceDebit" ErrorMessage="<%$ Resources:Attendance,Enter PO Advance Debit A/C%>"></asp:RequiredFieldValidator>

                                                                            <asp:TextBox ID="txtPOAdvanceDebit" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender16" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtPOAdvanceDebit"
                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <br />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-6">
                                                            <div class="box box-primary">
                                                                <div class="box-header with-border">
                                                                    <h3 class="box-title">
                                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Sales Section%>"></asp:Label></h3>
                                                                    <div class="box-tools pull-right">
                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                            <i class="fa fa-minus"></i>
                                                                        </button>
                                                                    </div>
                                                                </div>
                                                                <div class="box-body">
                                                                    <div class="form-group">
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblAcsalesInvoice" runat="server" Text="<%$ Resources:Attendance,Sales Invoice%>" />
                                                                            <a style="color: Red">*</a>
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator24" ValidationGroup="Cmp_Save"
                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtAcSalesInvoice" ErrorMessage="<%$ Resources:Attendance,Enter Sales Invoice%>"></asp:RequiredFieldValidator>

                                                                            <asp:TextBox ID="txtAcSalesInvoice" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtAcSalesInvoice"
                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblAcSalesReturn" runat="server" Text="<%$ Resources:Attendance,Sales Return%>" />
                                                                            <a style="color: Red">*</a>
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator25" ValidationGroup="Cmp_Save"
                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSalesReturn" ErrorMessage="<%$ Resources:Attendance,Enter Sales Return%>"></asp:RequiredFieldValidator>

                                                                            <asp:TextBox ID="txtSalesReturn" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender13" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtSalesReturn"
                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblSOAdvanceCreditAC" runat="server" Text="<%$ Resources:Attendance,SO Advance Credit A/C%>" />
                                                                            <a style="color: Red">*</a>
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator12" ValidationGroup="Cmp_Save"
                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSOAdvanceCreditAC" ErrorMessage="<%$ Resources:Attendance,Enter SO Advance Credit A/C%>"></asp:RequiredFieldValidator>

                                                                            <asp:TextBox ID="txtSOAdvanceCreditAC" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender17" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtSOAdvanceCreditAC"
                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblCostOfSales" runat="server" Text="<%$ Resources:Attendance,Cost Of Sales%>" />
                                                                            <a style="color: Red">*</a>
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator16" ValidationGroup="Cmp_Save"
                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCostOfSales" ErrorMessage="<%$ Resources:Attendance,Enter Cost Of Sales%>"></asp:RequiredFieldValidator>

                                                                            <asp:TextBox ID="txtCostOfSales" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender19" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtCostOfSales"
                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <br />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-6">
                                                            <div class="box box-primary">
                                                                <div class="box-header with-border">
                                                                    <h3 class="box-title">
                                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Customer & Supplier%>"></asp:Label></h3>
                                                                    <div class="box-tools pull-right">
                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                            <i class="fa fa-minus"></i>
                                                                        </button>
                                                                    </div>
                                                                </div>
                                                                <div class="box-body">
                                                                    <div class="form-group">
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblAcPaymentVouchers" runat="server" Text="<%$ Resources:Attendance,Suppliers Account%>" />
                                                                            <a style="color: Red">*</a>
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator18" ValidationGroup="Cmp_Save"
                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtAcPaymentVouchers" ErrorMessage="<%$ Resources:Attendance,Enter Suppliers Account%>"></asp:RequiredFieldValidator>

                                                                            <asp:TextBox ID="txtAcPaymentVouchers" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtAcPaymentVouchers"
                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblAcReceiveVouchers" runat="server" Text="<%$ Resources:Attendance,Customers Account%>" />
                                                                            <a style="color: Red">*</a>
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Cmp_Save"
                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtAcReceiveVouchers" ErrorMessage="<%$ Resources:Attendance,Enter Customers Account%>"></asp:RequiredFieldValidator>

                                                                            <asp:TextBox ID="txtAcReceiveVouchers" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender12" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtAcReceiveVouchers"
                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblRoundOffAc" runat="server" Text="<%$ Resources:Attendance,Round Off Account%>" />
                                                                            <a style="color: Red">*</a>
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator17" ValidationGroup="Cmp_Save"
                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtRoundOffAc" ErrorMessage="<%$ Resources:Attendance,Enter Round Off Account%>"></asp:RequiredFieldValidator>

                                                                            <asp:TextBox ID="txtRoundOffAc" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender20" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtRoundOffAc"
                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <br />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-6">
                                                            <div class="box box-primary">
                                                                <div class="box-header with-border">
                                                                    <h3 class="box-title">
                                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Others%>"></asp:Label></h3>
                                                                    <div class="box-tools pull-right">
                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                            <i class="fa fa-minus"></i>
                                                                        </button>
                                                                    </div>
                                                                </div>
                                                                <div class="box-body">
                                                                    <div class="form-group">
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblAcHrSection" runat="server" Text="<%$ Resources:Attendance,HR Section%>" />
                                                                            <a style="color: Red">*</a>
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Cmp_Save"
                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtAcHrSection" ErrorMessage="<%$ Resources:Attendance,Enter HR Section%>"></asp:RequiredFieldValidator>

                                                                            <asp:TextBox ID="txtAcHrSection" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtAcHrSection"
                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblAcCashTransaction" runat="server" Text="<%$ Resources:Attendance,Cash Account%>" />
                                                                            <a style="color: Red">*</a>
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator11" ValidationGroup="Cmp_Save"
                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtAcCashTransaction" ErrorMessage="<%$ Resources:Attendance,Enter Cash Account%>"></asp:RequiredFieldValidator>

                                                                            <asp:TextBox ID="txtAcCashTransaction" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtAcCashTransaction"
                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="Label14" runat="server" Text="Employee Advance Payment" />
                                                                            <a style="color: Red">*</a>
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Cmp_Save"
                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEmpAdvancePayment" ErrorMessage="<%$ Resources:Attendance,Enter Employee Advance Payment%>"></asp:RequiredFieldValidator>

                                                                            <asp:TextBox ID="txtEmpAdvancePayment" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender23" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtEmpAdvancePayment"
                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblEmployeeAccount" runat="server" Text="Employee Account" />
                                                                            <a style="color: Red">*</a>
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator22" ValidationGroup="Cmp_Save"
                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEmployeeAccount" ErrorMessage="<%$ Resources:Attendance,Enter Employee Account%>"></asp:RequiredFieldValidator>

                                                                            <asp:TextBox ID="txtEmployeeAccount" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                AutoPostBack="true" OnTextChanged="txtEmployeeAccount_OnTextChanged"></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender ID="EmployeeAccount_AutoCompleteExtender" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtEmployeeAccount"
                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <br />
                                                                        </div>

                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblVehicleAccount" runat="server" Text="Vehicle Account" />
                                                                            <a style="color: Red">*</a>
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator26" ValidationGroup="Cmp_Save"
                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtVehicleAccount" ErrorMessage="<%$ Resources:Attendance,Enter Vehicle Account%>"></asp:RequiredFieldValidator>

                                                                            <asp:TextBox ID="txtVehicleAccount" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                AutoPostBack="true" OnTextChanged="txtVehicleAccount_OnTextChanged"></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender ID="VehicleAccount_AutoCompleteExtender" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtVehicleAccount"
                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="Label18" runat="server" Text="Employee Loan Account" />
                                                                            <a style="color: Red">*</a>
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator27" ValidationGroup="Cmp_Save"
                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEmpLoanAccount" ErrorMessage="<%$ Resources:Attendance,Enter Employee Loan Account%>"></asp:RequiredFieldValidator>

                                                                            <asp:TextBox ID="txtEmpLoanAccount" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                AutoPostBack="true" OnTextChanged="txtEmployeeAccount_OnTextChanged"></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender24" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtEmpLoanAccount"
                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="Label19" runat="server" Text="<%$ Resources:Attendance,Indirect Income%>" />
                                                                            <a style="color: Red">*</a>
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Cmp_Save"
                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_Indirect_Income" ErrorMessage="<%$ Resources:Attendance,Enter Indirect Income%>"></asp:RequiredFieldValidator>

                                                                            <asp:TextBox ID="Txt_Indirect_Income" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                AutoPostBack="true" OnTextChanged="txtEmployeeAccount_OnTextChanged"></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender25" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="Txt_Indirect_Income"
                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <br />
                                                                        </div>


                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblLeavesalary" runat="server" Text="Leave Salary Account" />
                                                                            <a style="color: Red">*</a>
                                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator10" ValidationGroup="Cmp_Save"
                                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_LeaveSalary_Account" ErrorMessage="Enter leave salary account"></asp:RequiredFieldValidator>

                                                                            <asp:TextBox ID="Txt_LeaveSalary_Account" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                AutoPostBack="true" OnTextChanged="txtEmployeeAccount_OnTextChanged"></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender26" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="Txt_LeaveSalary_Account"
                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <br />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>


                                                    </div>
                                                    <div class="col-md-12">
                                                        <hr />
                                                    </div>

                                                  <%--  <div class="col-md-6">
                                                        <asp:Label ID="lblProfitandLoss" runat="server" Text="<%$ Resources:Attendance,Profit & Loss%>" />
                                                        <asp:TextBox ID="txtProfitandLoss" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged" Text="0"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender21" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtProfitandLoss"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblCapitalAccount" runat="server" Text="<%$ Resources:Attendance,Capital Account%>" />
                                                        <asp:TextBox ID="txtCapitalAccount" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged" Text="0"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender22" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtCapitalAccount"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblAcLoss" runat="server" Text="<%$ Resources:Attendance,Loss%>" />
                                                        <asp:TextBox ID="txtAcLoss" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged" text="0"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtAcLoss" UseContextKey="True"
                                                            CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblAcProfit" runat="server" Text="<%$ Resources:Attendance,Profit%>" />
                                                        <asp:TextBox ID="txtAcProfit" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged" Text="0"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender8" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtAcProfit"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblAcdebitTransaction" runat="server" Text="<%$ Resources:Attendance,Debit Transaction%>" />
                                                        <asp:TextBox ID="txtAcdebitTransaction" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged" Text="0"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtAcdebitTransaction"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblAcGeneralaccount" runat="server" Text="<%$ Resources:Attendance,General Account%>" />
                                                        <asp:TextBox ID="txtAcGeneralaccount" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged" Text="0"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender9" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtAcGeneralaccount"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Trading Account%>" />
                                                        <asp:TextBox ID="txtAcTradingaccount" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged" Text="0"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender10" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtAcTradingaccount"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblAcCreditaccount" runat="server" Text="<%$ Resources:Attendance,Credit Account%>" />
                                                        <asp:TextBox ID="txtAcCreditaccount" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged" Text="0"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender11" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtAcCreditaccount"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>--%>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success" Visible="false"
                                                            OnClick="btnSave_Click" Text="<%$ Resources:Attendance,Save %>"
                                                            ValidationGroup="Cmp_Save" />

                                                        <asp:Button ID="btnReset" runat="server" CssClass="btn btn-primary" OnClick="btnReset_Click"
                                                            Text="<%$ Resources:Attendance,Reset %>" />

                                                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-danger" OnClick="btnCancel_Click"
                                                            Text="<%$ Resources:Attendance,Cancel %>" />
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
                    <div class="tab-pane" id="Location">
                        <asp:UpdatePanel ID="Update_Location" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="box box-primary">
                                                                <div class="box-header with-border">
                                                                    <h3 class="box-title">
                                                                        <asp:Label ID="Label8" runat="server" Text="Transfer In Finance"></asp:Label></h3>
                                                                    <div class="box-tools pull-right">
                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                            <i class="fa fa-minus"></i>
                                                                        </button>
                                                                    </div>
                                                                </div>
                                                                <div class="box-body">
                                                                    <div class="form-group">
                                                                        <div class="col-md-12">
                                                                            <br />
                                                                            <asp:Label ID="Label34" runat="server" Text="Allow All Data In Tranfer In Finance" />
                                                                        </div>
                                                                        <div class="col-md-4">
                                                                            <asp:CheckBox ID="chkTransferInFinance" runat="server" Text="Allow Tranfer In Finance" />
                                                                            <br />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="box box-primary">
                                                                <div class="box-header with-border">
                                                                    <h3 class="box-title">
                                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,For Cash Flow%>"></asp:Label></h3>
                                                                    <div class="box-tools pull-right">
                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                            <i class="fa fa-minus"></i>
                                                                        </button>
                                                                    </div>
                                                                </div>
                                                                <div class="box-body">
                                                                    <div class="form-group">
                                                                        <div class="col-md-6">
                                                                            <div class="col-md-12">
                                                                                <asp:Label ID="lblCFAcc" runat="server" Text="<%$ Resources:Attendance,Cash Flow Account%>" />
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-5">
                                                                                <asp:ListBox ID="lstCOA" runat="server" Style="width: 100%;" Height="200px"
                                                                                    SelectionMode="Multiple" CssClass="form-control" Font-Names="Trebuchet MS" Font-Size="Small"
                                                                                    ForeColor="Gray"></asp:ListBox>
                                                                            </div>
                                                                            <div class="col-lg-2" style="text-align: center">
                                                                                <div style="margin-top: 35px; margin-bottom: 35px;" class="btn-group-vertical">

                                                                                    <asp:Button ID="btnPushCOA" runat="server" CssClass="btn btn-info" Text=">" OnClick="btnPushCOA_Click" />

                                                                                    <asp:Button ID="btnPullCOA" Text="<" runat="server" CssClass="btn btn-info" OnClick="btnPullCOA_Click" />

                                                                                    <asp:Button ID="btnPushAllCOA" Text=">>" OnClick="btnPushAllCOA_Click" runat="server" CssClass="btn btn-info" />

                                                                                    <asp:Button ID="btnPullAllCOA" Text="<<" OnClick="btnPullAllCOA_Click" runat="server" CssClass="btn btn-info" />
                                                                                </div>
                                                                            </div>

                                                                            <div class="col-md-5">
                                                                                <asp:ListBox ID="lstCOASelected" runat="server" Style="width: 100%;" Height="200px"
                                                                                    SelectionMode="Multiple" CssClass="form-control" Font-Names="Trebuchet MS" Font-Size="Small"
                                                                                    ForeColor="Gray"></asp:ListBox>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-md-6">
                                                                            <div class="col-md-12">
                                                                                <asp:Label ID="lblCFLocation" runat="server" Text="<%$ Resources:Attendance,Cash Flow Location%>" />
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-5">
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
                                                                            <div class="col-md-5">
                                                                                <asp:ListBox ID="lstLocationSelect" runat="server" Style="width: 100%;" Height="200px"
                                                                                    SelectionMode="Multiple" CssClass="form-control" Font-Names="Trebuchet MS" Font-Size="Small"
                                                                                    ForeColor="Gray"></asp:ListBox>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col-md-12">
                                                                            <br />
                                                                            <asp:Label ID="lblColorCode" runat="server" Text="<%$ Resources:Attendance,Color Code%>" />
                                                                        </div>
                                                                        <div class="col-md-4">
                                                                            <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Attendance,Reconciled %>" />
                                                                            <asp:TextBox ID="txtReconciled" runat="server"
                                                                                CssClass="form-control" />
                                                                            <cc1:ColorPickerExtender ID="txtPresentColorCode_ColorPickerExtender" runat="server"
                                                                                Enabled="True" TargetControlID="txtReconciled" SampleControlID="txtReconciled">
                                                                            </cc1:ColorPickerExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-4">
                                                                            <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Attendance,Conflicted %>" />
                                                                            <asp:TextBox ID="txtConflicted" runat="server"
                                                                                CssClass="form-control" />
                                                                            <cc1:ColorPickerExtender ID="ColorPickerExtender1" runat="server" Enabled="True"
                                                                                TargetControlID="txtConflicted" SampleControlID="txtConflicted">
                                                                            </cc1:ColorPickerExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-4">
                                                                            <asp:Label ID="Label17" runat="server" Text="<%$ Resources:Attendance,Not Reconciled %>" />
                                                                            <asp:TextBox ID="txtNotReconciled" runat="server"
                                                                                CssClass="form-control" />
                                                                            <cc1:ColorPickerExtender ID="ColorPickerExtender2" runat="server" Enabled="True"
                                                                                TargetControlID="txtNotReconciled" SampleControlID="txtNotReconciled">
                                                                            </cc1:ColorPickerExtender>
                                                                            <br />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <div class="box box-primary">
                                                                <div class="box-header with-border">
                                                                    <h3 class="box-title">
                                                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,For Bank Account%>"></asp:Label></h3>
                                                                    <div class="box-tools pull-right">
                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                            <i class="fa fa-minus"></i>
                                                                        </button>
                                                                    </div>
                                                                </div>
                                                                <div class="box-body">
                                                                    <div class="form-group">
                                                                        <div class="col-md-12">
                                                                            <div class="col-md-5">
                                                                                <asp:ListBox ID="lstBankAccount" runat="server" Style="width: 100%;" Height="200px"
                                                                                    SelectionMode="Multiple" CssClass="form-control" Font-Names="Trebuchet MS" Font-Size="Small"
                                                                                    ForeColor="Gray"></asp:ListBox>
                                                                            </div>
                                                                            <div class="col-lg-2" style="text-align: center">
                                                                                <div style="margin-top: 35px; margin-bottom: 35px;" class="btn-group-vertical">

                                                                                    <asp:Button ID="btnPushBank" runat="server" CssClass="btn btn-info" Text=">" OnClick="btnPushBank_Click" />

                                                                                    <asp:Button ID="btnPullBank" Text="<" runat="server" CssClass="btn btn-info" OnClick="btnPullBank_Click" />

                                                                                    <asp:Button ID="btnPushAllBank" Text=">>" OnClick="btnPushAllBank_Click" runat="server" CssClass="btn btn-info" />

                                                                                    <asp:Button ID="btnPullAllBank" Text="<<" OnClick="btnPullAllBank_Click" runat="server" CssClass="btn btn-info" />
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-5">
                                                                                <asp:ListBox ID="lstBankAccountSelect" runat="server" Style="width: 100%;" Height="200px"
                                                                                    SelectionMode="Multiple" CssClass="form-control" Font-Names="Trebuchet MS" Font-Size="Small"
                                                                                    ForeColor="Gray"></asp:ListBox>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-6">
                                                            <div class="box box-primary">
                                                                <div class="box-header with-border">
                                                                    <h3 class="box-title">
                                                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,For Cash Flow Generation%>"></asp:Label></h3>
                                                                    <div class="box-tools pull-right">
                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                            <i class="fa fa-minus"></i>
                                                                        </button>
                                                                    </div>
                                                                </div>
                                                                <div class="box-body">
                                                                    <div class="form-group">
                                                                        <div class="col-md-6">
                                                                            <asp:CheckBox ID="chkWeekOff" runat="server" Text="<%$ Resources:Attendance,Exclude Week Off In Cash Flow%>" />
                                                                            <br />
                                                                            <br />
                                                                            <asp:CheckBox ID="chkHoliday" runat="server" Text="<%$ Resources:Attendance,Exclude Holiday In Cash Flow%>" />
                                                                            <br />
                                                                            <br />
                                                                            <asp:CheckBox ID="chkPaymentApproval" runat="server" Text="<%$ Resources:Attendance,Payment Need Approval%>" />
                                                                            <br />
                                                                            <br />
                                                                        </div>

                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblCashFlowStartDate" runat="server" Text="<%$ Resources:Attendance,Cash Flow Start Date%>" />
                                                                            <asp:TextBox ID="txtCashFlowStartDate" runat="server" CssClass="form-control" />
                                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender_VoucherDate" runat="server" TargetControlID="txtCashFlowStartDate"
                                                                                Format="dd/MMM/yyyy" />
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblCashFlowOpeningBalance" runat="server" Text="<%$ Resources:Attendance,Cash Flow Opening Balance%>" />
                                                                            <asp:TextBox ID="txtCashFlowOpeningBalance" runat="server" CssClass="form-control" />
                                                                            <br />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>





                                                    </div>

                                                    <div class="col-md-12">
                                                        <div id="Div_Box_Add1" runat="server" class="box box-info collapsed-box">
                                                            <div class="box-header with-border">
                                                                <h3 class="box-title">
                                                                    <asp:Label ID="lblAddContact" runat="server" Text="Email Setup Detail"></asp:Label></h3>
                                                                <div class="box-tools pull-right">
                                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                        <i id="Btn_Add_Div1" runat="server" class="fa fa-plus"></i>
                                                                    </button>
                                                                </div>
                                                            </div>
                                                            <div class="box-body">
                                                                <div class="form-group">

                                                                    <div class="row">
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="Label29" runat="server" Text="<%$ Resources:Attendance,Email %>"></asp:Label>
                                                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="Label30" runat="server" Text="<%$ Resources:Attendance,Password %>"></asp:Label>
                                                                            <asp:TextBox ID="txtPasswordEmail" TextMode="Password" runat="server"
                                                                                CssClass="form-control" />
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="Label31" runat="server" Text="<%$ Resources:Attendance,SMTP %>"></asp:Label>
                                                                            <asp:TextBox ID="txtSMTP" runat="server" CssClass="form-control" />
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="Label32" runat="server" Text="<%$ Resources:Attendance,Port %>"></asp:Label>
                                                                            <asp:TextBox ID="txtSmtpPort" runat="server" CssClass="form-control" />
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblpop3" runat="server" Text="<%$ Resources:Attendance,POP3  %>"></asp:Label>
                                                                            <asp:TextBox ID="txtPop3" runat="server" CssClass="form-control" />
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="Label98" runat="server" Text="<%$ Resources:Attendance,  Port %>"></asp:Label>
                                                                            <asp:TextBox ID="txtpopport" runat="server" CssClass="form-control" />
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Display Text %>"></asp:Label>
                                                                            <asp:TextBox ID="txtDisplayText" runat="server" CssClass="form-control" />
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="Label33" runat="server" Text="<%$ Resources:Attendance,EnableSSL %>"></asp:Label>
                                                                            <asp:CheckBox ID="chkEnableSSL" runat="server" />
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-12">
                                                                            <asp:Label ID="Label23" runat="server" Text="Estatement footer"></asp:Label>
                                                                            <cc1:Editor ID="txtEstatementFooter" runat="server" Height="100px" />
                                                                        </div>

                                                                        <div class="col-md-12">
                                                                            <asp:Label ID="lblDepartment" runat="server" Text="Finance Department"></asp:Label>
                                                                            <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-control" />
                                                                        </div>
                                                                    </div>




                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>




                                                    <div class="col-md-12">
                                                        <div class="box box-primary">
                                                            <div class="box-header with-border">
                                                                <h3 class="box-title">
                                                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Sales Commission%>"></asp:Label></h3>
                                                                <div class="box-tools pull-right">
                                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                        <i class="fa fa-minus"></i>
                                                                    </button>
                                                                </div>
                                                            </div>
                                                            <div class="box-body">
                                                                <div class="form-group">
                                                                    <div class="col-md-6">
                                                                        <asp:Label ID="lblSCDebitAccount" runat="server" Text="<%$ Resources:Attendance,For Debit Account%>" />
                                                                        <a style="color: Red">*</a>
                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator14"  ValidationGroup="loc_param"
                                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSCDebitAccountSales" ErrorMessage="<%$ Resources:Attendance,Enter For Debit Account%>"></asp:RequiredFieldValidator>

                                                                        <asp:TextBox ID="txtSCDebitAccountSales" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                            AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender15" runat="server" DelimiterCharacters=""
                                                                            Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtSCDebitAccountSales"
                                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                        </cc1:AutoCompleteExtender>
                                                                        <br />
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <asp:Label ID="lblSCCreditAccount" runat="server" Text="<%$ Resources:Attendance,For Credit Account%>" />
                                                                        <a style="color: Red">*</a>
                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator23" ValidationGroup="loc_param"
                                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSCCreditAccountSales" ErrorMessage="<%$ Resources:Attendance,Enter For Credit Account%>"></asp:RequiredFieldValidator>

                                                                        <asp:TextBox ID="txtSCCreditAccountSales" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                            AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender18" runat="server" DelimiterCharacters=""
                                                                            Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtSCCreditAccountSales"
                                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                        </cc1:AutoCompleteExtender>
                                                                        <br />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="box box-primary">
                                                            <div class="box-header with-border">
                                                                <h3 class="box-title">
                                                                    <asp:Label ID="Label24" runat="server" Text="Sales commission for agent"></asp:Label></h3>
                                                                <div class="box-tools pull-right">
                                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                        <i class="fa fa-minus"></i>
                                                                    </button>
                                                                </div>
                                                            </div>
                                                            <div class="box-body">
                                                                <div class="form-group">
                                                                    <div class="col-md-6">
                                                                        <asp:Label ID="Label25" runat="server" Text="<%$ Resources:Attendance,For Debit Account%>" />
                                                                        <a style="color: Red">*</a>
                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator29" ValidationGroup="loc_param"
                                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSCDebitAccountAgent" ErrorMessage="<%$ Resources:Attendance,Enter For Debit Account%>"></asp:RequiredFieldValidator>

                                                                        <asp:TextBox ID="txtSCDebitAccountAgent" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                            AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender29" runat="server" DelimiterCharacters=""
                                                                            Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtSCDebitAccountAgent"
                                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                        </cc1:AutoCompleteExtender>
                                                                        <br />
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <asp:Label ID="Label26" runat="server" Text="<%$ Resources:Attendance,For Credit Account%>" />
                                                                        <a style="color: Red">*</a>
                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator30" ValidationGroup="loc_param"
                                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSCCreditAccountAgent" ErrorMessage="<%$ Resources:Attendance,Enter For Credit Account%>"></asp:RequiredFieldValidator>

                                                                        <asp:TextBox ID="txtSCCreditAccountAgent" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                            AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender30" runat="server" DelimiterCharacters=""
                                                                            Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtSCCreditAccountAgent"
                                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                        </cc1:AutoCompleteExtender>
                                                                        <br />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-6">
                                                        <asp:CheckBox ID="chkLeaveSalary" runat="server" Text="Seperate voucher entry for payroll leave salary" />
                                                    </div>

                                                    <div class="col-md-6">
                                                        <asp:CheckBox ID="chkallowances" runat="server" Text="Seperate voucher entry for payroll allowances" />
                                                    </div>


                                                    <div class="col-md-6">
                                                        <asp:CheckBox ID="chkDeduction" runat="server" Text="Seperate voucher entry for payroll deduction" />
                                                    </div>

                                                    <div class="col-md-6">
                                                        <asp:CheckBox ID="chkautoageingsettlement" runat="server" Text="Auto ageing settle" />
                                                        <br />
                                                    </div>

                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label20" runat="server" Text="Supplier Payment Reminds To" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator13" ValidationGroup="loc_param"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSupplierPaymentReminderEmp" ErrorMessage="Enter Supplier Payment Remind To"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtSupplierPaymentReminderEmp" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            AutoPostBack="true" OnTextChanged="txtSupplierPaymentReminderEmp_textChanged" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender28" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListEmployeeName" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtSupplierPaymentReminderEmp"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label21" runat="server" Text="Customer Payment Reminds To" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator28"  ValidationGroup="loc_param"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCustomerPaymentReminderEmp" ErrorMessage="Enter Customer Payment Remind To"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtCustomerPaymentReminderEmp" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            AutoPostBack="true" OnTextChanged="txtCustomerPaymentReminderEmp_textChanged" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender27" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListEmployeeName" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtCustomerPaymentReminderEmp"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnLocationSave" runat="server" CssClass="btn btn-success" Visible="false" ValidationGroup="loc_param"
                                                            OnClick="btnLocationSave_Click" Text="<%$ Resources:Attendance,Save %>" />

                                                        <asp:Button ID="btnLocationReset" runat="server" CssClass="btn btn-primary" OnClick="btnLocationReset_Click"
                                                            Text="<%$ Resources:Attendance,Reset %>" />

                                                        <asp:Button ID="btnLocationCancel" runat="server" CssClass="btn btn-danger" OnClick="btnLocationCancel_Click"
                                                            Text="<%$ Resources:Attendance,Cancel %>" />
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

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Company">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Location">
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
    <script type="text/javascript">
        function Li_Tab_Location() {
            document.getElementById('<%= Btn_Location.ClientID %>').click();
        }
    </script>
</asp:Content>
