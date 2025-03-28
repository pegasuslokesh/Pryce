<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true"
    CodeFile="Inv_ParameterMaster.aspx.cs" Inherits="Inventory_Inv_ParameterMaster"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-cogs"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Parameter Setup%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Parameter Setup%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="btnInventory_Click" Text="Inventory" />
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" OnClick="BtnPurchase_Click" Text="Purchase" />
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnSales_Click" Text="Sales" />
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

    <div id="myTab" class="nav-tabs-custom">
        <ul class="nav nav-tabs pull-right bg-blue-gradient">
            <li><a href="#PnlSalesDetail" data-toggle="tab">
                <asp:UpdatePanel ID="Update_label" runat="server">
                    <ContentTemplate>
                        <i class="fas fa-shopping-cart"></i>&nbsp;&nbsp;
                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Sales %>"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </a></li>
            <li><a href="#pnlPurchaseDetail" data-toggle="tab">
                <i class="fas fa-shopping-bag"></i>&nbsp;&nbsp;
                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,Purchase %>"></asp:Label></a></li>
            <li id="Li_Inventory" class="active"><a href="#pnlInventoryDetail" data-toggle="tab">
                <i class="fas fa-boxes"></i>
                <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Inventory %>"></asp:Label></a></li>
        </ul>
        <div class="tab-content">
            <div class="tab-pane" style="display: none;" id="User_Level">
                <asp:UpdatePanel ID="Userlevel_Update" runat="server">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-6">
                                <asp:Label ID="Lbl_name" runat="server" Text="<%$ Resources:Attendance,Employee %>"></asp:Label>
                                <asp:TextBox ID="txtEmp" OnTextChanged="ddlEmp_TextChanged" AutoPostBack="true" BackColor="#eeeeee"
                                    runat="server" CssClass="form-control" />
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="GetCompletionList" ServicePath="" CompletionInterval="100"
                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtEmp" UseContextKey="True"
                                    CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                    CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem">
                                </cc1:AutoCompleteExtender>
                                <br />
                            </div>
                            <div class="col-md-6">
                                <asp:CheckBox ID="chkIsShowSupplier" class="flat-red" Text="<%$ Resources:Attendance,Is Show Supplier%>"
                                    runat="server" />
                                <br />
                                <asp:CheckBox ID="chkinvCostPrice" Text="<%$ Resources:Attendance,Is Show CostPrice%>"
                                    runat="server" />
                                <br />
                            </div>
                            <div class="col-md-12" style="text-align: center; padding: 0px 15px 15px 15px;">
                                <asp:Button ID="btnCSave" OnClick="btnCSave_Click" Visible="false" CssClass="btn btn-success"
                                    runat="server" Text="<%$ Resources:Attendance,Save %>" />
                                <asp:Button ID="BtnReset" CausesValidation="False" OnClick="BtnReset_Click" CssClass="btn btn-primary"
                                    runat="server" Text="<%$ Resources:Attendance,Reset %>" />
                                <asp:HiddenField ID="editid" runat="server" />
                            </div>
                            <div class="col-md-12">
                                <div class="box box-warning box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Search Filter</h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <!-- /.box-header -->
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <div class="col-lg-2">
                                                        <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Employee %>" Value="Emp_Name" />
                                                            <asp:ListItem Text="Is Show Supplier" Value="IsShowSupplier"></asp:ListItem>
                                                            <asp:ListItem Text="Is Show CostPrice" Value="IsShowCostPrice"></asp:ListItem>
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
                                                            <asp:TextBox ID="txtValue" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </asp:Panel>
                                                    </div>
                                                    <div class="col-lg-2">
                                                        <asp:ImageButton ID="btnbind" runat="server" CausesValidation="False" ImageUrl="~/Images/search.png"
                                                            OnClick="btnbindrpt_Click" ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>
                                                        <asp:ImageButton ID="btnRefresh" runat="server" CausesValidation="False" ImageUrl="~/Images/refresh.png"
                                                            OnClick="btnRefreshReport_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                                    </div>
                                                    <div class="col-lg-3">
                                                        <h5 class="text-center">
                                                            <asp:Label ID="lblTotalRecords" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></asp:Label></h5>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvParameter" PageSize="<%# PageControlCommon.GetPageSize() %>" runat="server"
                                                        AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvParameter_PageIndexChanging"
                                                        AllowSorting="True" OnSorting="GvParameter_Sorting">

                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Emp_Id") %>'
                                                                        ImageUrl="~/Images/edit.png" OnCommand="btnEdit_Command" Visible="false" CausesValidation="False"
                                                                        ToolTip="<%$ Resources:Attendance,Edit %>" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name %>" SortExpression="ParameterName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblParameterName" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Is Show Supplier" SortExpression="IsShowSupplier">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblParameterValue" runat="server" Text='<%# Eval("IsShowSupplier") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Is Show CostPrice" SortExpression="IsShowCostPrice">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblParameterValue1" runat="server" Text='<%# Eval("IsShowCostPrice") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
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
                                <!-- /.box -->
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="tab-pane active" id="pnlInventoryDetail">
                <asp:UpdatePanel ID="Update_Inventory" runat="server">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="box box-info">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label35" runat="server" Text="<%$ Resources:Attendance,Scanning Solution%>"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <asp:CheckBox ID="chkTransferOutScanning" runat="server" Text="<%$ Resources:Attendance,Tranfer Out item through scanning only ?%>" />
                                            <br />
                                            <asp:CheckBox ID="chkTransferInScanning" runat="server" Text="<%$ Resources:Attendance,Tranfer In item through scanning only ?%>" />

                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="box box-info">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label36" runat="server" Text="<%$ Resources:Attendance,Serial Validation%>"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <asp:CheckBox ID="chkIsRequiredOnTransfer" runat="server" Text="<%$ Resources:Attendance,Is Serial required on transfer out voucher ?%>" />
                                            <br />
                                            <asp:CheckBox ID="chkisRequiredonSalesreturn" runat="server" Text="<%$ Resources:Attendance,Is Serial required on Purchase Goods Receive ?%>" />
                                            <br />
                                            <asp:CheckBox ID="chkisRequiredonPurchase" runat="server" Text="<%$ Resources:Attendance,Is Serial required on Sales Return Voucher ?%>" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="box box-info">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label37" runat="server" Text="<%$ Resources:Attendance,General%>"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <div class="col-md-12">

                                                <asp:Label ID="Lbls" runat="server" Text="<%$ Resources:Attendance,Commission Payment Allow(Before)%>"></asp:Label>
                                                <div style="width: 100%" class="input-group">
                                                    <asp:TextBox ID="txtCommisisonpaymentallow" runat="server" class="form-control"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredtxtCommisisonpaymentallow" runat="server"
                                                        Enabled="True" TargetControlID="txtCommisisonpaymentallow" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <span class="input-group-addon">
                                                        <asp:Label ID="txtpaymentMonth" runat="server" Text="<%$ Resources:Attendance,In Month%>"></asp:Label></span>
                                                </div>
                                                <br />
                                            </div>

                                            <div class="col-md-12">
                                                <asp:Label ID="Label57" runat="server" Text="Product Reorder Functionality"></asp:Label>
                                                <asp:DropDownList ID="ddlItemReorder" runat="server" class="form-control">
                                                    <asp:ListItem Text="Auto" Value="Auto"></asp:ListItem>
                                                    <asp:ListItem Text="Manual" Value="Manual"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <%--</div>--%>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6" style="text-align: center;">
                                <div class="form-group">
                                    <asp:Button ID="btnSaveInventory" runat="server" OnClick="btnSaveInventory_Click"
                                        Text="<%$ Resources:Attendance,Save%>" class="btn btn-primary" />
                                    <asp:Button ID="btnResetInventory" runat="server" OnClick="btnResetInventory_Click"
                                        Text="<%$ Resources:Attendance,Cancel%>" Style="margin-left: 15px;" class="btn btn-primary" />
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="tab-pane" id="pnlPurchaseDetail">
                <asp:UpdatePanel ID="Update_Purchase" runat="server">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="box box-info">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label38" runat="server" Text="<%$ Resources:Attendance,Tax & Discount%>"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Is Tax%>"></asp:Label>
                                            <asp:DropDownList ID="ddlPurchaseTax" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="True" Value="True"></asp:ListItem>
                                                <asp:ListItem Text="False" Value="False"></asp:ListItem>
                                            </asp:DropDownList>
                                            <br />
                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Is Discount%>"></asp:Label>
                                            <asp:DropDownList ID="ddlPurchaseDiscount" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="True" Value="True"></asp:ListItem>
                                                <asp:ListItem Text="False" Value="False"></asp:ListItem>
                                            </asp:DropDownList>
                                            <br />
                                            <asp:Label ID="Label58" runat="server" Text="<%$ Resources:Attendance,Allow TAX edit on individual transactions%>"></asp:Label>
                                            <asp:DropDownList ID="DDL_Edit_Individual_Purchase" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="True" Value="True"></asp:ListItem>
                                                <asp:ListItem Text="False" Value="False"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="box box-info">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label39" runat="server" Text="<%$ Resources:Attendance,Purchase Approval%>"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Purchase Request Approval%>"></asp:Label>
                                            <asp:DropDownList ID="ddlPurchaseRequestApoproval" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="True" Value="True"></asp:ListItem>
                                                <asp:ListItem Text="False" Value="False"></asp:ListItem>
                                            </asp:DropDownList>
                                            <br />
                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Purchase Order Approval%>"></asp:Label>
                                            <asp:DropDownList ID="ddlPurchaseOrderApproval" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="True" Value="True"></asp:ListItem>
                                                <asp:ListItem Text="False" Value="False"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="box box-info">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label40" runat="server" Text="<%$ Resources:Attendance,Purchase Account Parameter%>"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Purchase Account Number%>"></asp:Label>
                                            <asp:TextBox ID="txtPurchaseAccountNo" runat="server" CssClass="form-control" OnTextChanged="txtPurchaseAccountNo_TextChanged"
                                                BackColor="#eeeeee" AutoPostBack="true" />
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtPurchaseAccountNo"
                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                            </cc1:AutoCompleteExtender>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="box box-info">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label41" runat="server" Text="Expenses Account Parameter"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <asp:Label ID="lblExpensesAccount" runat="server" Text="Expenses Account for Reverse Charges" />
                                            <asp:TextBox ID="txtExpensesAccountNo" runat="server" CssClass="form-control" OnTextChanged="txtExpensesAccountNo_TextChanged"
                                                BackColor="#eeeeee" AutoPostBack="true" />
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtExpensesAccountNo"
                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                            </cc1:AutoCompleteExtender>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="box box-info">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label42" runat="server" Text="<%$ Resources:Attendance,Purchase Order%>"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Attendance,Is Duty%>"></asp:Label>
                                            <asp:DropDownList ID="ddlPurchaeOrderduty" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="True" Value="True"></asp:ListItem>
                                                <asp:ListItem Text="False" Value="False"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6" style="display: none">
                                <div class="box box-info">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label51" runat="server" Text="<%$ Resources:Attendance,Purchase Invoice%>"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <asp:CheckBox ID="chkIsPurchaseRunningBill" runat="server" CssClass="form-control" Text="Running Purchase Invoice allowed ?" />

                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <asp:CheckBox ID="chkIsunitCostshow" runat="server" Text="<%$ Resources:Attendance,Is Unit Cost show%>" />
                            </div>
                            <div class="col-md-12">
                                <div class="box box-info">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label473" runat="server" Text="<%$ Resources:Attendance,Report Header Level%>"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Attendance,Purchase Request%>"></asp:Label>
                                                <asp:DropDownList ID="ddlRptPR" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="Internal" Value="Internal"></asp:ListItem>
                                                    <asp:ListItem Text="External" Value="External"></asp:ListItem>
                                                </asp:DropDownList>
                                                <br />
                                                <asp:Label ID="Label17" runat="server" Text="<%$ Resources:Attendance,Purchase Order%>"></asp:Label>
                                                <asp:DropDownList ID="ddlRptPO" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="Internal" Value="Internal"></asp:ListItem>
                                                    <asp:ListItem Text="External" Value="External"></asp:ListItem>
                                                </asp:DropDownList>
                                                <br />
                                                <asp:Label ID="Label18" runat="server" Text="<%$ Resources:Attendance,Goods Receive%>"></asp:Label>
                                                <asp:DropDownList ID="ddlRptGR" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="Internal" Value="Internal"></asp:ListItem>
                                                    <asp:ListItem Text="External" Value="External"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <asp:Label ID="Label19" runat="server" Text="<%$ Resources:Attendance,Purchase Inquiry%>"></asp:Label>
                                                <asp:DropDownList ID="ddlRptPI" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="Internal" Value="Internal"></asp:ListItem>
                                                    <asp:ListItem Text="External" Value="External"></asp:ListItem>
                                                </asp:DropDownList>
                                                <br />
                                                <asp:Label ID="Label20" runat="server" Text="<%$ Resources:Attendance,Purchase Invoice%>"></asp:Label>
                                                <asp:DropDownList ID="ddlRptPIn" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="Internal" Value="Internal"></asp:ListItem>
                                                    <asp:ListItem Text="External" Value="External"></asp:ListItem>
                                                </asp:DropDownList>
                                                <br />
                                                <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Report Header%>"></asp:Label>
                                                <asp:DropDownList ID="ddlPurchaseReportheader" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="Company Level" Value="Company Level"></asp:ListItem>
                                                    <asp:ListItem Text="Brand Level" Value="Brand Level"></asp:ListItem>
                                                    <asp:ListItem Text="Location Level" Value="Location Level"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12" style="text-align: center;">
                                <div class="form-group">
                                    <asp:Button ID="Btn_Save_Purchase" runat="server" OnClick="btnSavePurchase_Click"
                                        Text="<%$ Resources:Attendance,Save%>" class="btn btn-primary" />
                                    <asp:Button ID="Btn_Cancel_Purchase" runat="server" OnClick="btnCancelPurchase_Click"
                                        Text="<%$ Resources:Attendance,Cancel%>" Style="margin-left: 15px;" class="btn btn-primary" />
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="tab-pane" id="PnlSalesDetail">
                <asp:UpdatePanel ID="Update_Salary" runat="server">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="box box-info">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label44" runat="server" Text="<%$ Resources:Attendance,Sales Price & Account%>"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Sales price%>"></asp:Label>
                                            <asp:DropDownList ID="ddlSalesPrice" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="Sales Price 1" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Sales Price 2" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Sales Price 3" Value="3"></asp:ListItem>
                                            </asp:DropDownList>
                                            <br />
                                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,Sales Account Number%>"></asp:Label>
                                            <asp:TextBox ID="txtSalesAccountNumber" runat="server" CssClass="form-control" OnTextChanged="txtSalesAccountNumber_TextChanged"
                                                BackColor="#eeeeee" AutoPostBack="true" />
                                            <cc1:AutoCompleteExtender ID="AutoComplete_AccountName" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtSalesAccountNumber"
                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                                CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem">
                                            </cc1:AutoCompleteExtender>
                                            <br />
                                            <asp:Label ID="Label43" runat="server" Text="<%$ Resources:Attendance,Price Should be%>"></asp:Label>
                                            <asp:DropDownList ID="ddlPriceshouldbe" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="Last Price" Value="Last Price"></asp:ListItem>
                                                <asp:ListItem Text="Sales Price" Value="Sales Price"></asp:ListItem>
                                                <asp:ListItem Text="Customer Price" Value="Customer Price"></asp:ListItem>
                                            </asp:DropDownList>
                                            <br />

                                            <asp:CheckBox ID="chkSalesbelowcostprice" runat="server" Text="Sales below cost price ?" CssClass="form-control" />
                                            <br />


                                            <asp:CheckBox ID="ChkSalesRunningBill" runat="server" Text="Running Sales Invoice allowed ?" CssClass="form-control" Visible="false" />

                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="box box-info">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label45" runat="server" Text="<%$ Resources:Attendance,Sales Approval%>"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Attendance,Sales Quotation Approval%>"></asp:Label>
                                            <asp:DropDownList ID="ddlSalesQuotationApproval" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="True" Value="True"></asp:ListItem>
                                                <asp:ListItem Text="False" Value="False"></asp:ListItem>
                                            </asp:DropDownList>
                                            <br />
                                            <asp:Label ID="Label21" runat="server" Text="<%$ Resources:Attendance,Sales Order Approval%>"></asp:Label>
                                            <asp:DropDownList ID="ddlSalesOrderApproval" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="True" Value="True"></asp:ListItem>
                                                <asp:ListItem Text="False" Value="False"></asp:ListItem>
                                            </asp:DropDownList>
                                            <br />
                                            <asp:Label ID="Label52" runat="server" Text="<%$ Resources:Attendance,Cash Order Approval%>"></asp:Label>
                                            <asp:DropDownList ID="ddlCashOrderApproval" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="True" Value="True"></asp:ListItem>
                                                <asp:ListItem Text="False" Value="False"></asp:ListItem>
                                            </asp:DropDownList>
                                            <br />
                                            <asp:Label ID="Label53" runat="server" Text="<%$ Resources:Attendance,Sales Invoice Approval%>"></asp:Label>
                                            <asp:DropDownList ID="ddlSalesInvoiceApproval" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="True" Value="True"></asp:ListItem>
                                                <asp:ListItem Text="False" Value="False"></asp:ListItem>
                                            </asp:DropDownList>
                                            <br />
                                            <asp:Label ID="Label54" runat="server" Text="<%$ Resources:Attendance,Credit Invoice Approval%>"></asp:Label>
                                            <asp:DropDownList ID="ddlCreditInvoiceApproval" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="True" Value="True"></asp:ListItem>
                                                <asp:ListItem Text="False" Value="False"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="box box-info">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label46" runat="server" Text="<%$ Resources:Attendance,Tax & Discount%>"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Is Tax%>"></asp:Label>
                                            <asp:DropDownList ID="ddlSalesTax" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="True" Value="True"></asp:ListItem>
                                                <asp:ListItem Text="False" Value="False"></asp:ListItem>
                                            </asp:DropDownList>
                                            <br />
                                            <asp:Label ID="Label55" runat="server" Text="<%$ Resources:Attendance,Is Discount%>"></asp:Label>
                                            <asp:DropDownList ID="ddlSalesDiscount" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="True" Value="True"></asp:ListItem>
                                                <asp:ListItem Text="False" Value="False"></asp:ListItem>
                                            </asp:DropDownList>
                                            <br />
                                            <asp:Label ID="Label59" runat="server" Text="<%$ Resources:Attendance,Allow TAX edit on individual transactions%>"></asp:Label>
                                            <asp:DropDownList ID="DDL_Edit_Individual_Sales" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="True" Value="True"></asp:ListItem>
                                                <asp:ListItem Text="False" Value="False"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="box box-info">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label47" runat="server" Text="<%$ Resources:Attendance,Report Parameter%>"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <asp:Label ID="Label23" runat="server" Text="<%$ Resources:Attendance,Report Format%>"></asp:Label>
                                            <asp:DropDownList ID="ddlReportFormat" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="System Format" Value="System Format"></asp:ListItem>
                                                <asp:ListItem Text="Customer Format 1" Value="Customer Format 1"></asp:ListItem>
                                                <asp:ListItem Text="iLabel  Format" Value="iLabel Format"></asp:ListItem>
                                                <asp:ListItem Text="India Location Report" Value="Taxable Report"></asp:ListItem>
                                                <asp:ListItem Text="kuwait(H.O.) Format" Value="Kuwait Format"></asp:ListItem>
                                            </asp:DropDownList>
                                            <br />
                                            <asp:Label ID="Label56" runat="server" Text="<%$ Resources:Attendance,Report Header%>"></asp:Label>
                                            <asp:DropDownList ID="ddlSalesReportHeader" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="Company Level" Value="Company Level"></asp:ListItem>
                                                <asp:ListItem Text="Brand Level" Value="Brand Level"></asp:ListItem>
                                                <asp:ListItem Text="Location Level" Value="Location Level"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="box box-info">
                                    <div class="box-header with-border">
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <asp:CheckBox ID="chkIsDelivery" runat="server" Text="<%$ Resources:Attendance,Is Delivery Voucher allow ?%>" />
                                            </div>
                                            <div class="col-md-4">
                                                <asp:CheckBox ID="chkscanningsolution" runat="server" Text="<%$ Resources:Attendance,Add sales invoice item through scanning only ?%>" />
                                            </div>
                                            <div class="col-md-4">
                                                <asp:CheckBox ID="chkDeliveryScanningSolution" runat="server" Text="<%$ Resources:Attendance,Add Delivery item through scanning only ?%>" />
                                            </div>
                                            <div class="col-md-4">
                                                <asp:CheckBox ID="chkCostingEntry" runat="server" Text="Is Costing Entry Allow" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="box box-info">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label48" runat="server" Text="<%$ Resources:Attendance,Sales Quotation%>"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <asp:CheckBox ID="SalesQtaPrm" runat="server" Text="Is Quotation Allow for New" />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label30" runat="server" Text="<%$ Resources:Attendance,Is Product Name Show%>"></asp:Label>
                                                <asp:DropDownList ID="ddlIsProductNameshow" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="True" Value="True"></asp:ListItem>
                                                    <asp:ListItem Text="False" Value="False"></asp:ListItem>
                                                </asp:DropDownList>
                                                <br />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="Label24" runat="server" Text="<%$ Resources:Attendance,Is barcode allow(report)%>"></asp:Label>
                                                <asp:DropDownList ID="ddlIsBarcodeallow" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="True" Value="True"></asp:ListItem>
                                                    <asp:ListItem Text="False" Value="False"></asp:ListItem>
                                                </asp:DropDownList>
                                                <br />
                                            </div>
                                            <div class="col-md-12">
                                                <div class="nav-tabs-custom">
                                                    <ul class="nav nav-tabs pull-right bg-blue-gradient">
                                                        <li><a href="#Footer" data-toggle="tab">
                                                            <asp:Label ID="Label31" runat="server" Text="<%$ Resources:Attendance,Footer %>"></asp:Label></a></li>
                                                        <li><a href="#Header" data-toggle="tab">
                                                            <asp:Label ID="Label32" runat="server" Text="<%$ Resources:Attendance,Header %>"></asp:Label></a></li>
                                                        <li class="active"><a href="#Condition" data-toggle="tab">
                                                            <asp:Label ID="Label33" runat="server" Text="<%$ Resources:Attendance,Terms & Condition%>"></asp:Label></a></li>
                                                    </ul>
                                                    <div class="tab-content">
                                                        <div class="tab-pane active" id="Condition">
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <cc1:Editor ID="txtSalesQuotationTermsandconditon" runat="server" Width="100%" Height="250px" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="tab-pane" id="Header">
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <cc1:Editor ID="txtSalesQuotationheader" runat="server" Width="100%" Height="250px" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="tab-pane" id="Footer">
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <cc1:Editor ID="txtSalesQuotationFooter" runat="server" Width="100%" Height="250px" />
                                                                </div>
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
                                <div class="box box-info">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label49" runat="server" Text="<%$ Resources:Attendance,Sales Invoice%>"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <asp:CheckBox ID="chkCashInvoiceAllow" runat="server" Text="Allow Retail Cash Invoice" />
                                            </div>
                                            <div class="col-md-12">
                                                <div class="nav-tabs-custom">
                                                    <ul class="nav nav-tabs pull-right bg-blue-gradient">
                                                        <li class="active"><a href="#Terms" data-toggle="tab">
                                                            <asp:Label ID="Label29" runat="server" Text="<%$ Resources:Attendance,Terms & Condition%>"></asp:Label></a></li>
                                                    </ul>
                                                    <div class="tab-content">
                                                        <div class="tab-pane active" id="Terms">
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <cc1:Editor ID="txtSalesInvoiceTermsandconditon" runat="server" Width="100%" Height="250px" />
                                                                </div>
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
                                <div class="box box-info">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label50" runat="server" Text="<%$ Resources:Attendance,Report Header Level%>"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <asp:Label ID="Label25" runat="server" Text="<%$ Resources:Attendance,Sales Inquiry%>"></asp:Label>
                                                <asp:DropDownList ID="ddlRptSI" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="Internal" Value="Internal"></asp:ListItem>
                                                    <asp:ListItem Text="External" Value="External"></asp:ListItem>
                                                </asp:DropDownList>
                                                <br />
                                                <asp:Label ID="Label26" runat="server" Text="<%$ Resources:Attendance,Sales Order%>"></asp:Label>
                                                <asp:DropDownList ID="ddlRptSO" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="Internal" Value="Internal"></asp:ListItem>
                                                    <asp:ListItem Text="External" Value="External"></asp:ListItem>
                                                </asp:DropDownList>
                                                <br />
                                                <asp:Label ID="Label27" runat="server" Text="<%$ Resources:Attendance,Delivery Voucher%>"></asp:Label>
                                                <asp:DropDownList ID="ddlRptDV" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="Internal" Value="Internal"></asp:ListItem>
                                                    <asp:ListItem Text="External" Value="External"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <asp:Label ID="Label28" runat="server" Text="<%$ Resources:Attendance,Sales Quotation%>"></asp:Label>
                                                <asp:DropDownList ID="ddlRptSQ" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="Internal" Value="Internal"></asp:ListItem>
                                                    <asp:ListItem Text="External" Value="External"></asp:ListItem>
                                                </asp:DropDownList>
                                                <br />
                                                <asp:Label ID="Label34" runat="server" Text="<%$ Resources:Attendance,Sales Invoice%>"></asp:Label>
                                                <asp:DropDownList ID="ddlRptSIn" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="Internal" Value="Internal"></asp:ListItem>
                                                    <asp:ListItem Text="External" Value="External"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12" style="text-align: center;">
                                <div class="form-group">
                                    <asp:Button ID="btnSaveSales" OnClick="btnSaveSales_Click" runat="server" Text="<%$ Resources:Attendance,Save%>"
                                        class="btn btn-primary" />
                                    <asp:Button ID="btnCancelSales" OnClick="btnCancelSales_Click" runat="server" Text="<%$ Resources:Attendance,Cancel%>"
                                        Style="margin-left: 15px;" class="btn btn-primary" />
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>


    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Inventory">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_Purchase">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Salary">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:Panel ID="PnlInventory" runat="server">
    </asp:Panel>
    <asp:Panel ID="PnlSales" runat="server">
    </asp:Panel>
    <asp:Panel ID="PnlPurchase" runat="server">
    </asp:Panel>
    <asp:Panel ID="pnlUserPermission" runat="server">
    </asp:Panel>
    <asp:Panel ID="Panel4" runat="server">
    </asp:Panel>
    <asp:Panel ID="Panel5" runat="server">
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>

    <script type="text/javascript">

        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10005;
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
    </script>

    <script type="text/javascript">
        function on_tab_position() {
            $("#Li_Extra").removeClass("active");
            $("#pnlExtra").removeClass("active");

            $("#Li_Inventory").addClass("active");
            $("#pnlInventoryDetail").addClass("active");

        }
    </script>

    <%--<script>
    $(function () {
        $('#datepicker').datepicker({
            autoclose: true
        });
    });
</script>--%>
</asp:Content>
