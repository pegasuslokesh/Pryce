<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="ChartOfAccount.aspx.cs" Inherits="GeneralLedger_ChartOfAccount" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <script src="../Script/customer.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-poll"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Chart Of Finance%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Finance Setup%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,General Ledger%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Chart Of Finance%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="btnList_Click" Text="List" />
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" OnClick="btnNew_Click" Text="New" />
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:Button ID="Btn_Chart_Of_Account" Style="display: none;" runat="server" data-toggle="modal" data-target="#Chart_Of_Account" Text="View Modal" />
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
                    <li id="Li_Import"><a href="#import" onclick="Li_Tab_Import()" data-toggle="tab">
                        <i class="fa fa-upload"></i>&nbsp;&nbsp;<asp:Label ID="Label19" runat="server" Text="<%$ Resources:Attendance,Import %>"></asp:Label></a></li>
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
					<asp:Label ID="lblTotalRecords" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>" />

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Account No %>" Value="Account_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Account Name %>" Value="AccountName"
                                                            Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Opening Balance %>" Value="Opening_Balance"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Created By %>" Value="CreatedByName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Modified By%>" Value="ModifiedByName"></asp:ListItem>

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
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbind">
                                                        <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" placeholder="Search from Content" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False"
                                                        OnClick="btnbindrpt_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False"
                                                        OnClick="btnRefreshReport_Click"
                                                        ToolTip="<%$ Resources:Attendance,Refresh %>"> <span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="btnGridView" runat="server" CausesValidation="False"
                                                        Visible="true" OnClick="btnGridView_Click"
                                                        ToolTip="<%$ Resources:Attendance, Tree View %>"><span class="fa fa-sitemap"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:ImageButton ID="btnTreeView" Visible="false" runat="server" CausesValidation="False" Style="width: 33px;"
                                                        ImageUrl="~/Images/NewTree.png" OnClick="btnTreeView_Click"></asp:ImageButton>


                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid" <%= GvCOA.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvCOA" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server"
                                                        AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvCOA_PageIndexChanging"
                                                        AllowSorting="True" OnSorting="GvCOA_Sorting">

                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                    ToolTip="View" OnCommand="lnkViewDetail_Command"
                                                                                    CausesValidation="False"><i class="fa fa-eye"></i>View</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                    OnCommand="btnEdit_Command" CausesValidation="False"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                    OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>
                                                                        </ul>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Id %>" SortExpression="Trans_Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvTransId" runat="server" Text='<%#Eval("Trans_Id") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Account No. %>" SortExpression="Account_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvAccountNo" runat="server" Text='<%#Eval("Account_No") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Account Name %>" SortExpression="AccountName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvAcountName" runat="server" Text='<%#Eval("AccountName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Currency %>" SortExpression="Currency_Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCurrency" runat="server" Text='<%# GetCurrency(Eval("Currency_Id").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="false" HeaderText="<%$ Resources:Attendance, Opening Credit Balance%>"
                                                                SortExpression="O_Cr_Bal">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvOpeningCrBalance" runat="server" Text='<%#Eval("O_Cr_Bal") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="false" HeaderText="<%$ Resources:Attendance, Opening Debit Balance%>"
                                                                SortExpression="O_Dr_Bal">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvOpeningDrBalance" runat="server" Text='<%#Eval("O_Dr_Bal") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Created By%>"
                                                                SortExpression="CreatedByName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCreatedByName" runat="server" Text='<%#Eval("CreatedByName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Modified By%>"
                                                                SortExpression="ModifiedByName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvModifiedByName" runat="server" Text='<%#Eval("ModifiedByName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <%--   <asp:TemplateField HeaderText="<%$ Resources:Attendance, Account Group %>" SortExpression="Acc_Group_Id">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvAccountGroup" runat="server" Text='<%#GetAccountGroupName(Eval("Acc_Group_Id").ToString()) %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center"  />
                                                    </asp:TemplateField>--%>
                                                            <%-- <asp:TemplateField HeaderText="<%$ Resources:Attendance, Type Of Account %>" SortExpression="Type_Account">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvTypeOfAccount" runat="server" Text='<%#Eval("Type_Account") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center"  />
                                                    </asp:TemplateField>--%>
                                                        </Columns>

                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                    <asp:TreeView ID="TreeViewCOA" runat="server" Visible="false" OnSelectedNodeChanged="TreeViewCOA_SelectedNodeChanged"
                                                        ExpandDepth="2">
                                                    </asp:TreeView>
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
                                <asp:Panel ID="PnlNewContant" runat="server" class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div id="tr1" runat="server" visible="false" class="col-md-6">
                                                        <asp:Label ID="lblOpeningDebitBalance" runat="server" Text="<%$ Resources:Attendance,Opening Debit Balance %>" />
                                                        <asp:TextBox ID="txtOpeningDebitBalance" Text="0.00" ReadOnly="true" runat="server"
                                                            CssClass="form-control" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                            TargetControlID="txtOpeningDebitBalance" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div id="tr2" runat="server" visible="false" class="col-md-6">
                                                        <asp:Label ID="lblOpeningCreditBalance" runat="server" Text="<%$ Resources:Attendance,Opening Credit Balance %>" />
                                                        <asp:TextBox ID="txtOpeningCreditBalance" Text="0.00" ReadOnly="true" runat="server"
                                                            CssClass="form-control" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                            TargetControlID="txtOpeningCreditBalance" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" id="TRA" runat="server" visible="false">
                                                        <asp:Label ID="lblAccountType" runat="server" Text="<%$ Resources:Attendance,Account Type %>" />
                                                        <asp:TextBox ID="txtAccountType" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div id="tr3" runat="server" visible="false" class="col-md-6">
                                                        <asp:Label ID="lblDebitAmount" runat="server" Text="<%$ Resources:Attendance,Debit Amount %>" />
                                                        <asp:TextBox ID="txtDebitAmount" runat="server" Text="0.00" CssClass="form-control"
                                                            ReadOnly="true" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                            TargetControlID="txtDebitAmount" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" id="tr4" runat="server" visible="false">
                                                        <asp:Label ID="lblCreditAmount" runat="server" Text="<%$ Resources:Attendance,Credit Amount %>" />
                                                        <asp:TextBox ID="txtCreditAmount" runat="server" Text="0.00" CssClass="form-control"
                                                            ReadOnly="true" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                            TargetControlID="txtCreditAmount" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div id="tr5" runat="server" visible="false" class="col-md-6">
                                                        <asp:Label ID="lblLastDebit" runat="server" Text="<%$ Resources:Attendance,Last Debit %>" />
                                                        <asp:TextBox ID="txtLastDebit" runat="server" Text="0.00" CssClass="form-control" ReadOnly="true" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                            TargetControlID="txtLastDebit" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div id="tr6" runat="server" visible="false" class="col-md-6">
                                                        <asp:Label ID="lblLastCredit" runat="server" Text="<%$ Resources:Attendance,Last Credit %>" />
                                                        <asp:TextBox ID="txtLastCredit" runat="server" Text="0.00" CssClass="form-control"
                                                            ReadOnly="true" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                                            TargetControlID="txtLastCredit" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblAccountGroup" runat="server" Text="<%$ Resources:Attendance,Account Group %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtAccountGroup" ErrorMessage="<%$ Resources:Attendance,Enter Account Group%>"></asp:RequiredFieldValidator>

                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtAccountGroup" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                OnTextChanged="txtAccountGroup_TextChanged" AutoPostBack="true" />
                                                            <cc1:AutoCompleteExtender ID="txtAccountGroup_AutoCompleteExtender" runat="server"
                                                                DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                                MinimumPrefixLength="1" ServiceMethod="GetCompletionListAccGroupName" ServicePath=""
                                                                TargetControlID="txtAccountGroup" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <div class="input-group-btn">
                                                                <asp:Button ID="btnAddAccountGroup" runat="server" Text="Open Group" CssClass="btn btn-primary"
                                                                    OnClick="btnAddAccountGroup_OnClick" />
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblAccountNo" runat="server" Text="<%$ Resources:Attendance,Account No. %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtAccountNo" ErrorMessage="<%$ Resources:Attendance,Enter Account No%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtAccountNo" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblAccountName" runat="server" Text="<%$ Resources:Attendance,Account Name %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtAccountName" ErrorMessage="<%$ Resources:Attendance,Enter Account Name%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblAccountNameL" runat="server" Text="<%$ Resources:Attendance,Account Name(Local) %>" />
                                                        <asp:TextBox ID="txtAccountNameL" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblCurrency" runat="server" Text="<%$ Resources:Attendance,Currency %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Save" Display="Dynamic"
                                                            SetFocusOnError="true" ControlToValidate="ddlCurrency" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Currency %>" />

                                                        <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" visible="true">
                                                        <asp:Label ID="Label3" runat="server" Text="GST No" />
                                                        <asp:TextBox ID="txtGstNo" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label4" runat="server" Text="Contact" />
                                                        <asp:TextBox ID="txtContactList" runat="server" Class="form-control" placeholder="Contact Name (*)" BackColor="#eeeeee" onchange="validateContactPerson(this)" />
                                                        <asp:RequiredFieldValidator ID="requiredfieldvalidator5" runat="server" ControlToValidate="txtContactList" ErrorMessage="Please Enter Contact Name !" Display="Dynamic" ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                                        <cc1:AutoCompleteExtender ID="txtContactList_AutoCompleteExtender" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="0" ServiceMethod="GetContactListCustomer" ServicePath=""
                                                            TargetControlID="txtContactList" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>

                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" id="trLock" runat="server" visible="false">
                                                        <asp:Label ID="lblLockAccount" runat="server" Text="<%$ Resources:Attendance,Lock Account %>" />
                                                        <asp:CheckBox ID="chkLockAccount" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div id="Div_Tree" runat="server" visible="false" class="col-md-12">
                                                        <hr />
                                                        <asp:Label ID="lblslectgroup" runat="server" Text="Select Account group" Font-Bold="true"
                                                            Visible="false"></asp:Label>
                                                        <asp:Panel ID="pnlTreeView" runat="server" Height="200px" ScrollBars="Auto" HorizontalAlign="Left">
                                                            <asp:TreeView ID="navTreeAccontGroup" runat="server" Visible="false" OnSelectedNodeChanged="navTreeAccontGroup_SelectedNodeChanged"
                                                                ExpandDepth="2">
                                                            </asp:TreeView>
                                                        </asp:Panel>
                                                        <hr />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Width="100%"
                                                            CssClass="ajax__tab_yuitabview-theme" OnClientActiveTabChanged="tabChanged">
                                                            <cc1:TabPanel ID="Tab_Opening_Balance" runat="server" HeaderText="<%$ Resources:Attendance,Opening Balance%>"
                                                                BorderColor="#C4C4C4">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_Tab_Opening_Balance" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12" style="overflow: auto; max-height: 500px;">
                                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvLocation" runat="server" AutoGenerateColumns="False" Width="100%">
                                                                                        <Columns>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Location %>">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblgvLocationName" Width="300px" runat="server" Text='<%#GetLocationName(Eval("Location_Id").ToString()) %>' />
                                                                                                    <asp:HiddenField ID="hdngvLocationId" runat="server" Value='<%#Eval("Location_Id") %>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Currency %>">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblgvCurrencyName" runat="server" Text='<%#GetCurrencyName(Eval("Field1").ToString()) %>' />
                                                                                                    <asp:HiddenField ID="hdngvCurrencyId" runat="server" Value='<%#Eval("Field1") %>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Debit%>">
                                                                                                <ItemTemplate>
                                                                                                    <asp:TextBox ID="txtgvDebit" Width="80px" runat="server" />
                                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredtxtgvDebit" runat="server" Enabled="True"
                                                                                                        TargetControlID="txtgvDebit" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Credit%>">
                                                                                                <ItemTemplate>
                                                                                                    <asp:TextBox ID="txtgvCredit" Width="80px" runat="server" />
                                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredtxtgvCredit" runat="server" Enabled="True"
                                                                                                        TargetControlID="txtgvCredit" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Foreign Debit%>">
                                                                                                <ItemTemplate>
                                                                                                    <asp:TextBox ID="txtgvForeignDebit" Width="80px" runat="server" />
                                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredtxtgvForeignDebit" runat="server" Enabled="True"
                                                                                                        TargetControlID="txtgvForeignDebit" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Foreign Credit%>">
                                                                                                <ItemTemplate>
                                                                                                    <asp:TextBox ID="txtgvForeignCredit" Width="80px" runat="server" />
                                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredtxtgvForeignCredit" runat="server" Enabled="True"
                                                                                                        TargetControlID="txtgvForeignCredit" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                        </Columns>


                                                                                        <PagerStyle CssClass="pagination-ys" />

                                                                                    </asp:GridView>
                                                                                    <br />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Tab_Opening_Balance">
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

                                                        <br />
                                                    </div>
                                                    <div id="PnlOperationButton" runat="server" class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnCOASave" ValidationGroup="Save" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                            CssClass="btn btn-success" OnClick="btnCOASave_Click" UseSubmitBehavior="false" OnClientClick="this.disabled='true'; this.value='Please wait...';" />

                                                        <asp:Button ID="BtnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                            CssClass="btn btn-primary" CausesValidation="False" OnClick="BtnReset_Click" />

                                                        <asp:Button ID="btnCOACancel" runat="server" CssClass="btn btn-danger" Text="<%$ Resources:Attendance,Cancel %>"
                                                            CausesValidation="False" OnClick="btnCOACancel_Click" />

                                                        <asp:HiddenField ID="hdnCOAId" runat="server" Value="0" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>


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
                                                    <asp:Label ID="Label2" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecordsBin" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldNameBin" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Account No %>" Value="Account_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Account Name %>" Value="AccountName"
                                                            Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Opening Balance %>" Value="Opening_Balance"></asp:ListItem>
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
                                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbindBin">
                                                        <asp:TextBox ID="txtValueBin" runat="server" CssClass="form-control" placeholder="Search from Content"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="btnbindBin" runat="server" CausesValidation="False"
                                                        OnClick="btnbindBin_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="btnRefreshBin" runat="server" CausesValidation="False"
                                                        OnClick="btnRefreshBin_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="imgBtnRestore" CausesValidation="False"
                                                        runat="server" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                </div>
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
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvCOABin" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server"
                                                        AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvCOABin_PageIndexChanging"
                                                        OnSorting="GvCOABin_OnSorting" AllowSorting="true">
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
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Id %>" SortExpression="Trans_Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvTransId" runat="server" Text='<%#Eval("Trans_Id") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Account No. %>" SortExpression="Account_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvAccountNo" runat="server" Text='<%#Eval("Account_No") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Account Name %>" SortExpression="AccountName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvAcountName" runat="server" Text='<%#Eval("AccountName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Account Name Local %>" SortExpression="AccountNameL">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvAccountNameL" runat="server" Text='<%#Eval("AccountNameL") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Opening Balance%>" SortExpression="Opening_Balance">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvOpeningBalance" runat="server" Text='<%#Eval("Opening_Balance") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Account Group %>" SortExpression="Acc_Group_Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvAccountGroup" runat="server" Text='<%#Eval("Acc_Group_Id") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Type Of Account %>" SortExpression="Type_Account">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvTypeOfAccount" runat="server" Text='<%#Eval("Type_Account") %>' />
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
                     <div class="tab-pane" id="import">
                        <asp:UpdatePanel ID="update_import" runat="server">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="lnkDownloadData" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div4" runat="server" class="box box-info">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label20" runat="server" Text="Import Using Excel"></asp:Label></h3>
                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="lnkTotalExcelImportRecords" runat="server" OnClick="lnkTotalExcelImportRecords_Click" CssClass="btn btn-default" Text="Total Records:0"></asp:LinkButton>
                                                <asp:HiddenField ID="hdnTotalExcelRecords" runat="server" Value="0" />
                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="lnkInvalidRecords" runat="server" OnClick="lnkInvalidRecords_Click" CssClass="btn btn-danger" Text="InValid:0"></asp:LinkButton>
                                                <asp:HiddenField ID="hdnInvalidExcelRecords" runat="server" Value="0" />
                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="lnkValidRecords" runat="server" OnClick="lnkValidRecords_Click" CssClass="btn btn-success" Text="Valid:0"></asp:LinkButton>
                                                <asp:HiddenField ID="hdnValidExcelRecords" runat="server" Value="0" />
                                            </div>
                                            <div class="box-body">
                                                <div class="row">
                                                    <div class="col-md-4">
                                                        <asp:Button ID="lnkDownloadData" runat="server"  Text="Download sample/data" OnClick="lnkDownloadData_Click"  Font-Bold="true" Font-Size="15px"></asp:Button>
                                                         <%--<asp:LinkButton ID="lnkDownloadData" runat="server"  Text="Download sample/data" OnClick="lnkDownloadData_Click"  Font-Bold="true" Font-Size="15px"></asp:LinkButton>-%>
                                                        <%--<asp:HyperLink ID="lnkDownloadSameExcel" runat="server" Font-Bold="true" Font-Size="15px"
                                                            NavigateUrl="~/CompanyResource/cof_ob.xls" Text="Download sample/data" Font-Underline="true"></asp:HyperLink>--%>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label runat="server" Text="Browse Excel File" ID="Label169"></asp:Label>
                                                        <div class="input-group" style="width: 100%;">
                                                            <cc1:AsyncFileUpload ID="fileLoad"
                                                               
                                                                OnClientUploadStarted="FUExcel_UploadStarted"
                                                                OnClientUploadError="FUExcel_UploadError"
                                                                OnClientUploadComplete="FUExcel_UploadComplete"
                                                                OnUploadedComplete="FUExcel_FileUploadComplete"
                                                                runat="server" CssClass="form-control"
                                                                CompleteBackColor="White"
                                                                UploaderStyle="Traditional"
                                                                UploadingBackColor="#CCFFFF"
                                                                ThrobberID="FUExcel_ImgLoader" Width="100%" />
                                                            <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                                <asp:LinkButton ID="FUExcel_Img_Right" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-check" style="font-size:30px;color:#22cb33"></i></asp:LinkButton>
                                                                <asp:LinkButton ID="FUExcel_Img_Wrong" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-remove" style="font-size:30px"></i></asp:LinkButton>
                                                                <asp:Image ID="FUExcel_ImgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Button ID="btnUploadExcel" runat="server" CssClass="btn btn-primary" OnClick="btnUploadExcel_Click" Text="Import" />
                                                        <asp:Button ID="btnSaveExcelData" runat="server" CssClass="btn btn-primary" OnClick="btnSaveExcelData_Click" Text="Update" Enabled="false" />
                                                    </div>
                                                </div>
                                                <br />
                                                <div style="overflow: auto; max-height: 300px;">
                                                    <asp:GridView  CssClass="table-striped table-bordered table table-hover" ID="gvImport" runat="server" Width="100%"></asp:GridView>
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

    <div class="modal fade" id="Chart_Of_Account" tabindex="-1" role="dialog" aria-labelledby="Chart_Of_AccountLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Chart_Of_AccountLabel">
                        <asp:Label ID="Label7" runat="server" Font-Size="14px" Font-Bold="true"
                            Text="<%$ Resources:Attendance,Chart Of Account %>"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Chart_Of_Account" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblVAccountGroup" runat="server" Text="<%$ Resources:Attendance,Account Group %>" />
                                                    &nbsp:&nbsp<asp:Label ID="txtVAccountGroup" Font-Bold="true" runat="server"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblVAccountNo" runat="server" Text="<%$ Resources:Attendance,Account No. %>" />
                                                    &nbsp:&nbsp<asp:Label ID="txtVAccountNo" Font-Bold="true" runat="server" />
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblVAccountName" runat="server" Text="<%$ Resources:Attendance,Account Name %>" />
                                                    &nbsp:&nbsp<asp:Label ID="txtVAccountName" Font-Bold="true" runat="server"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblVAccountNameL" runat="server" Text="<%$ Resources:Attendance,Account Name(Local) %>" />
                                                    &nbsp:&nbsp<asp:Label ID="txtVAccountNameL" Font-Bold="true" runat="server"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblVCurrency" runat="server" Text="<%$ Resources:Attendance,Currency %>" />
                                                    &nbsp:&nbsp<asp:Label ID="txtVCurrency" Font-Bold="true" runat="server"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblVLockAccount" runat="server" Text="<%$ Resources:Attendance,Lock Account %>" />
                                                    &nbsp:&nbsp<asp:CheckBox ID="chkVLockAccount" Font-Bold="true" Enabled="false" runat="server" />
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <br />
                                                </div>
                                                <div class="col-md-12" style="overflow: auto; max-height: 500px;">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GVLocationView" runat="server" AutoGenerateColumns="False" Width="100%">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Location %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvLocationName" Width="300px" runat="server" Text='<%#GetLocationName(Eval("Location_Id").ToString()) %>' />
                                                                    <asp:HiddenField ID="hdngvLocationId" runat="server" Value='<%#Eval("Location_Id") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Currency %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCurrencyName" runat="server" Text='<%#GetCurrencyName(Eval("Field1").ToString()) %>' />
                                                                    <asp:HiddenField ID="hdngvCurrencyId" runat="server" Value='<%#Eval("Field1") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Debit%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="txtgvDebit" Width="80px" runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Credit%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="txtgvCredit" Width="80px" runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Foreign Debit%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="txtgvForeignDebit" Width="80px" runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Foreign Credit%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="txtgvForeignCredit" Width="80px" runat="server" />
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
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="Update_Chart_Of_Account_Button" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                            <asp:Button ID="BtnCancelView" runat="server" CssClass="btn btn-primary" OnClick="BtnCancelView_Click" Visible="false"
                                Text="<%$ Resources:Attendance,Close %>" CausesValidation="False" />
                            <asp:HiddenField ID="hdnCOAIdView" runat="server" Value="0" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_Chart_Of_Account">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress10" runat="server" AssociatedUpdatePanelID="update_import">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_Chart_Of_Account_Button">
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

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Bin">
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
    <asp:Panel ID="pnlMenuNew" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlMenuList" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlMenuBin" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PnlList" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PnlNewEdit" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PnlBin" runat="server" Visible="false"></asp:Panel>
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

        function Chart_Of_Account_Popup() {
            document.getElementById('<%= Btn_Chart_Of_Account.ClientID %>').click();
        }
    </script>
    <script type="text/javascript">
        var lasttab = 0;
        function tabChanged(sender, args) {
            lasttab = sender.get_activeTabIndex();
        }

        function FUExcel_UploadComplete(sender, args) {
            document.getElementById('<%= FUExcel_Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= FUExcel_Img_Right.ClientID %>').style.display = "";
        }
        function FUExcel_UploadError(sender, args) {
            document.getElementById('<%= FUExcel_Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= FUExcel_Img_Wrong.ClientID %>').style.display = "";
            alert('Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file');
        }
        function FUExcel_UploadStarted(sender, args) {
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
    </script>
</asp:Content>
