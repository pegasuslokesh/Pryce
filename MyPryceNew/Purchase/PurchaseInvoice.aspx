<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="PurchaseInvoice.aspx.cs" Inherits="Purchase_PurchaseInvoice" %>

<%@ Register Src="~/WebUserControl/Expenses_Tax.ascx" TagName="Expenses_Tax" TagPrefix="ET" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/AccountMaster.ascx" TagPrefix="uc1" TagName="AccountMaster" %>
<%@ Register Src="~/WebUserControl/FileUpload.ascx" TagPrefix="AT1" TagName="FileUpload1" %>
<%@ Register Src="~/WebUserControl/ucControlsSetting.ascx" TagName="ucCtlSetting" TagPrefix="UC" %>
<%@ Register Src="~/WebUserControl/StockAnalysis.ascx" TagName="StockAnalysis" TagPrefix="SA" %>
<%@ Register Src="~/WebUserControl/ReportSystem.ascx" TagName="ReportSystem" TagPrefix="RS" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <style>
        .grid td, .grid th {
            text-align: center;
        }
    </style>
   
    <script>

        
        function LI_Edit_Active() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
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

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-file-invoice-dollar"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Purchase Invoice %>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Purchase%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Purchase Invoice%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Pending_Order" Style="display: none;" runat="server" OnClick="btnPendingOrder_Click" Text="Pending Order" />
            <asp:Button ID="Btn_GST_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Modal_GST" Text="GST" />
            <asp:Button ID="Btn_Show_Expenses_Tax" Style="display: none;" runat="server" data-toggle="modal" data-target="#Expenses_Tax_Web_Control" Text="Expenses Tax" />
            <asp:Button ID="Btn_Modal_Expenses_Tax" Style="display: none;" runat="server" data-toggle="modal" data-target="#Modal_Expenses_Tax" Text="Expenses Tax" />
            <asp:Button ID="Btn_Modal_FileUpload" Style="display: none;" runat="server" data-toggle="modal" data-target="#Fileupload123" />
            <asp:HiddenField runat="server" ID="hdnCanView" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
            <asp:HiddenField runat="server" ID="hdnCanPrint" />
            <asp:HiddenField runat="server" ID="hdnCanUpload" />
            <asp:HiddenField ID="hfScrollPosition" runat="server" Value="0" />
            <asp:HiddenField ID="hdfCurrentRow" runat="server" />

        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:HiddenField ID="hdnDecimalCount" runat="server" Value="0" />
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
                    <li id="Li_Pending_Order"><a href="#Pending_Order" onclick="Li_Tab_Pending_Order()" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Pending Order %>"></asp:Label></a></li>
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
                                                    <asp:Label ID="Label29" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				 <asp:Label ID="lblTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records : 0 %>"></asp:Label>
                                                <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-12">
                                                    <asp:DropDownList ID="ddlLocList" runat="server" Class="form-control" OnSelectedIndexChanged="ddlLocList_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <br />
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlPosted" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPosted_SelectedIndexChanged">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Posted %>" Value="Posted"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,UnPosted %>" Value="UnPosted" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,All %>" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Invoice No %>" Value="InvoiceNo"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Invoice Date %>" Value="InvoiceDate"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Purchase Order No %>" Value="OrderList"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Supplier Name %>" Value="Supplier_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="CreatedUser"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Modified By %>" Value="ModifiedUser"></asp:ListItem>
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
                                                <div class="col-lg-4">
                                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                        <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" placeholder="Search From Content" />
                                                        <asp:TextBox ID="txtValueDate" runat="server" CssClass="form-control" Visible="false" placeholder="Search From Date"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueDate" runat="server" TargetControlID="txtValueDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnGvListSetting" ImageAlign="Right" ToolTip="List Settings" runat="server" OnClick="btnGvListSetting_Click" Visible="false"><span class="fas fa-wrench"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div class="box box-warning box-solid" <%= GvPurchaseInvoice.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:HiddenField ID="hdnConfirmValue1" runat="server" />
                                                    <asp:HiddenField ID="hdnConfirmValue2" runat="server" />
                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                    <asp:HiddenField ID="HdnEdit" runat="server" />
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvPurchaseInvoice" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowSorting="True" CurrentSortField="InvoiceDate" CurrentSortDirection="DESC"
                                                        OnSorting="GvPurchaseInvoice_OnSorting">
                                                        <Columns>

                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnPrint" runat="server" CausesValidation="False" CommandArgument='<%# Eval("TransId") %>' OnCommand="IbtnPrint_Command"><i class="fa fa-print"></i>Print</asp:LinkButton>
                                                                            </li>
                                                                           <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <a onclick="getReportDataWithLoc('<%# Eval("TransId") %>','<%# Eval("Location_Id") %>')"><i class="fa fa-print" style="cursor: pointer"></i>Report System</a>
                                                                            </li>

                                                                            <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("TransId") %>' CommandName='<%# Eval("Location_id") %>' OnCommand="lnkViewDetail_Command" CausesValidation="False"><i class="fa fa-eye"></i>View</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("TransId") %>' CommandName='<%# Eval("Location_id") %>' CausesValidation="False" OnCommand="btnEdit_Command"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("TransId") %>' OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>

                                                                            <li <%= hdnCanUpload.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnFileUpload" runat="server" CausesValidation="False" CommandArgument='<%# Eval("TransId") %>' CommandName='<%# Eval("InvoiceNo") %>' OnCommand="IbtnFileUpload_Command"><i class="fa fa-upload"></i>File Upload</asp:LinkButton>
                                                                            </li>
                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Invoice No %>" SortExpression="InvoiceNo">
                                                                <ItemTemplate>
                                                                    <%# Eval("InvoiceNo") %>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Invoice Date %>" SortExpression="InvoiceDate">
                                                                <ItemTemplate>
                                                                    <%# Convert.ToDateTime(Eval("InvoiceDate").ToString()).ToString("dd-MMM-yyyy") %>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Purchase Order No %>" SortExpression="OrderList">
                                                                <ItemTemplate>
                                                                    <%# Eval("OrderList") %>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Supplier Name %>" SortExpression="Supplier_Name">
                                                                <ItemTemplate>
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td width="80%">
                                                                                <asp:LinkButton ID="lnkSupplierAccountMaster" Text='<%# Eval("Supplier_Name") %>' runat="server" CommandArgument='<%# Eval("SupplierId") %>'
                                                                                    OnCommand="lnkSupplierAccountMaster_Command" ToolTip="Account Master" />
                                                                                <%--<asp:Label ID="lblSupInvoiceNo" runat="server" Text='<%# Eval("Supplier_Name").ToString() %>'></asp:Label>--%>
                                                                            </td>
                                                                            <td align="right" width="20%">
                                                                                <a onclick="lblgvSupplierName_Command('<%# Eval("SupplierId") %>')" style="cursor: pointer; color: blue">More..
                                                                                </a>
                                                                                <%--<asp:LinkButton ID="lblgvSupplierName" runat="server" Text="More.." ForeColor="Blue"
                                                                                    CommandArgument='<%# Eval("SupplierId") %>' OnCommand="lblgvSupplierName_Command"
                                                                                    ToolTip="Supplier History" />--%>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="CreatedUser">
                                                                <ItemTemplate>
                                                                    <%#Eval("CreatedUser") %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>" SortExpression="ModifiedUser">
                                                                <ItemTemplate>
                                                                    <%# Eval("ModifiedUser") %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Amount%>" SortExpression="GrandTotal">
                                                                <ItemTemplate>
                                                                    <%# Eval("Currency_Code").ToString()+" "+SystemParameter.GetAmountWithDecimal(Eval("GrandTotal").ToString(),Eval("CurrencyDecimalCount").ToString()) %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Costing Rate%>" SortExpression="CostingRate">
                                                                <ItemTemplate>
                                                                    <%#  SystemParameter.GetAmountWithDecimal(Eval("CostingRate").ToString(),Eval("CurrencyDecimalCount").ToString()) %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                        </Columns>


                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                </div>
                                                <asp:HiddenField ID="hdnGvPurchaseInvoiceCurrentPageIndex" runat="server" Value="1" />
                                                <div class="col-md-12">
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
                                    <!-- /.box-body -->
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnPost" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server" UpdateMode="Always">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="GvPurchaseInvoice" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12" align="right">
                                                        <asp:LinkButton ID="btnControlsSetting" ToolTip="Product List Setting" runat="server" OnClick="btnControlsSetting_Click" Visible="false"><i class="fas fa-wrench" style="font-size:20px"></i></asp:LinkButton>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:RadioButton ID="rbtEdit" runat="server" GroupName="SaveOrEdit" Text="Edit" Visible="false" Checked="true" />
                                                        <asp:RadioButton ID="rbtNew" runat="server" GroupName="SaveOrEdit" Text="New" Visible="false" />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblInvoicedate" runat="server" Text="<%$ Resources:Attendance,Invoice Date %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtInvoicedate" ErrorMessage="<%$ Resources:Attendance,Enter Invoice Date %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtInvoicedate" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtCalenderExtender" runat="server" TargetControlID="txtInvoicedate">
                                                        </cc1:CalendarExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblInvoiceNo" runat="server" Text="<%$ Resources:Attendance,Invoice No %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtInvoiceNo" ErrorMessage="<%$ Resources:Attendance,Enter Invoice No %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control" AutoPostBack="True"
                                                            OnTextChanged="txtInvoiceNo_TextChanged" />
                                                        <br />
                                                    </div>
                                                    <div style="display: none;" class="col-md-6">
                                                        <asp:Label ID="lblOrderType" runat="server" Text="<%$ Resources:Attendance,Invoice Type %>"
                                                            Visible="false"></asp:Label>
                                                        <asp:DropDownList ID="ddlInvoiceType" runat="server" Visible="false" CssClass="form-control">
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Cash %>" Value="Cash"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Credit %>" Value="Credit"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblSupInvoiceDate" runat="server" Text="<%$ Resources:Attendance,Supplier Invoice Date %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSupInvoiceDate" ErrorMessage="<%$ Resources:Attendance,Enter Supplier Invoice Date %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtSupInvoiceDate" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender4" runat="server" TargetControlID="txtSupInvoiceDate">
                                                        </cc1:CalendarExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" id="ctlSupplierInvoiceNo" runat="server">
                                                        <asp:Label ID="lblSupInvoiceNo" runat="server" Text="<%$ Resources:Attendance,Supplier Invoice No %>"></asp:Label>
                                                        <%-- <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSupplierInvoiceNo" ErrorMessage="<%$ Resources:Attendance,Enter Supplier Invoice No %>"></asp:RequiredFieldValidator>--%>

                                                        <asp:TextBox ID="txtSupplierInvoiceNo" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Currency %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator6" ValidationGroup="Save" Display="Dynamic"
                                                            SetFocusOnError="true" ControlToValidate="ddlCurrency" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Currency %>" />

                                                        <div class="input-group">
                                                            <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="form-control"
                                                                AutoPostBack="True" OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <div class="input-group-btn">
                                                                <asp:TextBox ID="TxtCurrencyValue" runat="server" Width="60px" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Exchange Rate %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtExchangeRate" ErrorMessage="<%$ Resources:Attendance,Enter Exchange Rate %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtExchangeRate" runat="server" CssClass="form-control" Text="0" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender39" runat="server" Enabled="True"
                                                            TargetControlID="txtExchangeRate" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblBillAmount" runat="server" Text="Bill Amount"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator7" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtBillAmount" ErrorMessage="<%$ Resources:Attendance,Enter Bill Amount %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtBillAmount" runat="server" CssClass="form-control" Text="0" AutoPostBack="true"
                                                            OnTextChanged="txtBillAmount_TextChanged" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender59" runat="server" Enabled="True"
                                                            TargetControlID="txtBillAmount" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" id="ctlCostingRate" runat="server">
                                                        <asp:Label ID="lblCostingRate" runat="server" Text="<%$ Resources:Attendance,Costing Rate%>"></asp:Label>
                                                        <asp:TextBox ID="txtCostingRate" runat="server" CssClass="form-control" ReadOnly="true"
                                                            Text="0"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender43" runat="server" Enabled="True"
                                                            TargetControlID="txtCostingRate" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label16" runat="server" Text="Receive Location"></asp:Label>
                                                        <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control">
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblBillType" runat="server" Text="Bill Type" Visible="false"></asp:Label>
                                                        <asp:DropDownList ID="ddlBillType" runat="server" CssClass="form-control" Visible="false">
                                                            <asp:ListItem Text="Normal" Value="Normal" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="Running" Value="Running"></asp:ListItem>
                                                            <asp:ListItem Text="Final" Value="Final"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div style="overflow: auto">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvOrderExpenses" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                                                BorderStyle="Solid" Width="100%">

                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Expenses %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvExpense_Id" runat="server" Text='<%# GetExpName(Eval("Expense_Id").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <FooterStyle BorderStyle="None" />
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Currency %>" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvExpCurrencyID" runat="server" Text='<%# CurrencyName(Eval("ExpCurrencyID").ToString()) %>' />
                                                                            <asp:Label ID="hidExpCur" runat="server" Visible="false" Text='<%# Eval("ExpCurrencyID").ToString() %>' />
                                                                        </ItemTemplate>
                                                                        <FooterStyle BorderStyle="None" />
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Expenses Amount">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvFCExchangeAmount" runat="server" Text='<%# Eval("FCExpAmount") %>' />
                                                                        </ItemTemplate>
                                                                        <FooterStyle BorderStyle="None" />
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Exchange Rate %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvExpExchangeRate" runat="server" Text='<%# Eval("ExpExchangeRate") %>' />
                                                                        </ItemTemplate>
                                                                        <%-- <FooterTemplate>
                                                                        <asp:Label ID="lbltotExp" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Expenses%>" /><b>:</b>
                                                                    </FooterTemplate>--%>
                                                                        <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Expenses Amount (Local)" SortExpression="Exp_Charges">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvExp_Charges" runat="server" Text='<%# Eval("Exp_Charges") %>' />
                                                                        </ItemTemplate>
                                                                        <%--  <FooterTemplate>
                                                                        <asp:Label ID="txttotExp" runat="server" Visible="false" Font-Bold="true" Text="0" />
                                                                        <asp:Label ID="txttotExpShow" runat="server" Font-Bold="true" Text="0" />
                                                                    </FooterTemplate>--%>
                                                                        <FooterStyle BorderStyle="None" />
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                </Columns>


                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Supplier Name %>"></asp:Label>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtSupplierName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                AutoPostBack="True" OnTextChanged="txtSupplierName_TextChanged" />
                                                            <cc1:AutoCompleteExtender ID="txtSupplierName_AutoCompleteExtender" runat="server"
                                                                CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                                                ServiceMethod="GetCompletionList" ServicePath="" TargetControlID="txtSupplierName"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <div class="input-group-btn">

                                                                <asp:Button ID="btnAddSupplier" runat="server" CssClass="btn btn-info" OnClientClick="btnAddSupplier_OnClick();return false;"
                                                                    Text="<%$ Resources:Attendance,Add %>" CausesValidation="False" />
                                                                <asp:Button ID="lnkSupplierHistory" runat="server" Text="History" CssClass="btn btn-info"
                                                                    OnClientClick="lnkSupplierHistory_OnClick();return false;" CausesValidation="False"></asp:Button>
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" id="Trans_Div" runat="server">
                                                        <asp:Label ID="lblTransType" runat="server" Text="Transaction Type"></asp:Label>
                                                        <%--<a style="color: Red">*</a>--%>
                                                        <asp:DropDownList ID="ddlTransType" OnSelectedIndexChanged="ddlTransType_SelectedIndexChanged" AutoPostBack="true" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        <%--<asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator14" ValidationGroup="Save" Display="Dynamic"
                                                                SetFocusOnError="true" ControlToValidate="ddlTransType" InitialValue="-1" ErrorMessage="<%$ Resources:Attendance,Select Transaction Type%>" />--%>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:RadioButton ID="RdoPo" runat="server" Text="<%$ Resources:Attendance,Purchase Order %>"
                                                            OnCheckedChanged="RdoPo_CheckedChanged" AutoPostBack="true" GroupName="Po" />
                                                        <asp:RadioButton ID="RdoWithOutPo" Style="margin-left: 15px;" runat="server" GroupName="Po"
                                                            Text="<%$ Resources:Attendance,Without Purchase Order %>" OnCheckedChanged="RdoPo_CheckedChanged"
                                                            AutoPostBack="true" />
                                                        <asp:RadioButton ID="RdoUpload" AutoPostBack="true" runat="server" GroupName="Po" Text="Upload" OnCheckedChanged="RdoPo_CheckedChanged" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Button ID="btnAddProductScreen" Visible="false" runat="server" Text="<%$ Resources:Attendance,Add Product List %>"
                                                            CssClass="btn btn-info" OnClick="btnAddProductScreen_Click" />
                                                        <asp:Button ID="btnAddtoList" runat="server" Text="<%$ Resources:Attendance,Fill Your Product %>"
                                                            CssClass="btn btn-info" Visible="false" OnClick="btnAddtoList_Click" />
                                                        <br />
                                                    </div>
                                                    <div id="pnlProductUpload" visible="false" runat="server" class="col-md-12">
                                                        <div class="col-md-5">
                                                            <asp:Label runat="server" Text="Browse Excel File" ID="Label169"></asp:Label>
                                                            <div class="input-group" style="width: 100%;">
                                                                <cc1:AsyncFileUpload ID="fileLoad" OnUploadedComplete="FileUploadComplete" OnClientUploadError="uploadError" OnClientUploadStarted="uploadStarted" OnClientUploadComplete="uploadComplete"
                                                                    runat="server" CssClass="form-control" CompleteBackColor="White" UploaderStyle="Traditional" UploadingBackColor="#CCFFFF" ThrobberID="imgLoader" Width="100%" />
                                                                <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                                    <asp:Image ID="Img_Right" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Allow.png" Style="display: none" />
                                                                    <asp:Image ID="Img_Wrong" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Delete1.png" Style="display: none" />
                                                                    <asp:Image ID="imgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                                </div>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <br />
                                                            <asp:Button CssClass="btn btn-primary" ID="btnGetExcelRecord" runat="server" OnClick="btnGetExcelRecord_Click" Text="Get Record" />
                                                            <asp:HyperLink ID="uploadPoItem" runat="server" Font-Bold="true" Font-Size="15px"
                                                                NavigateUrl="~/CompanyResource/UploadPIItem.xlsx" Text="Download sample excel" Font-Underline="true"></asp:HyperLink>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <br />
                                                            <asp:Button CssClass="btn btn-primary" runat="server" ID="btnDownloadProduct" OnClick="btnDownloadProduct_Click" Text="Download" />
                                                            &nbsp;
    <asp:Button ID="btnGvProductRefresh" CssClass="btn btn-warning" runat="server" Text="Refresh" ToolTip="Reset Product Table" OnClick="btnGvProductRefresh_Click" />

                                                        </div>
                                                    </div>
                                                    <div id="PnlProductSearching" runat="server" class="col-md-12">
                                                        <br />
                                                        <div class="alert alert-info ">
                                                            <div class="row">
                                                                <div class="form-group">
                                                                    <div class="col-md-2"></div>
                                                                    <div class="col-lg-3">
                                                                        <asp:DropDownList ID="ddlProductSerach" runat="server" CssClass="form-control"
                                                                            OnSelectedIndexChanged="ddlProductSerach_SelectedIndexChanged"
                                                                            AutoPostBack="true">
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,Product Id %>" Value="ProductCode"></asp:ListItem>
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,Product Name %>" Value="ProductName"
                                                                                Selected="True"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div class="col-lg-3">
                                                                        <asp:TextBox ID="txtProductSerachValue" runat="server" CssClass="form-control" />

                                                                        <asp:TextBox ID="txtProductId" runat="server" CssClass="form-control" AutoPostBack="True"
                                                                            OnTextChanged="txtProductCode_TextChanged" BackColor="#eeeeee" Visible="false" />
                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="100"
                                                                            DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductCode"
                                                                            ServicePath="" TargetControlID="txtProductId" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                        </cc1:AutoCompleteExtender>

                                                                        <asp:TextBox ID="txtSearchProductName" runat="server" BackColor="#eeeeee" CssClass="form-control"
                                                                            AutoPostBack="True" OnTextChanged="txtProductName_TextChanged" />
                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionInterval="100"
                                                                            DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductName"
                                                                            ServicePath="" TargetControlID="txtSearchProductName" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                        </cc1:AutoCompleteExtender>
                                                                    </div>
                                                                    <div class="col-lg-2" style="text-align: center">
                                                                        <asp:HiddenField ID="Hdn_Tax_By" runat="server" />
                                                                        <asp:ImageButton runat="server" CausesValidation="False" ImageUrl="~/Images/add.png"
                                                                            Style="width: 33px; margin-top: 3px;" ToolTip="<%$ Resources:Attendance,Add %>" ID="ImgbtnProductSave"
                                                                            OnClick="btnProductSave_Click" Visible="false"></asp:ImageButton>

                                                                        <asp:ImageButton ID="imgbtnsearch" runat="server" CausesValidation="False" Style="margin-top: -5px;"
                                                                            ImageUrl="~/Images/search.png" OnClick="imgbtnsearch_Click" ToolTip="<%$ Resources:Attendance,Search %>" />

                                                                        <asp:ImageButton ID="ImgbtnRefresh" runat="server" CausesValidation="False"
                                                                            ImageUrl="~/Images/refresh.png" OnClick="ImgbtnRefresh_Click" Style="width: 33px;" ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                                                    </div>
                                                                    <div class="col-md-2"></div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="box box-warning box-solid">
                                                            <div class="box-body">
                                                                <div class="row">
                                                                    <div class="col-md-12">
                                                                        <div style="overflow: auto; max-height: 200px;">
                                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvSerachGrid" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                                ShowFooter="true">
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center">
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="chkTrandId" runat="server" AutoPostBack="true" OnCheckedChanged="chkTrandId_CheckedChanged" />
                                                                                            <asp:Label ID="TransId" runat="server" Text='<%# Eval("Trans_Id") %>' Visible="false"></asp:Label>
                                                                                            <asp:Label ID="Lbl_Order_Currency" runat="server" Text='<%# Eval("Trans_Id") %>' Visible="false"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                        <ItemStyle />
                                                                                        <FooterStyle BorderStyle="None" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Purchase Order No %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblPONo" runat="server" Text='<%# Eval("PONo") %>'></asp:Label>
                                                                                            <asp:Label ID="lblOrderId" runat="server" Text='<%#Eval("POID") %>' Visible="false"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" Width="130px" />
                                                                                        <ItemStyle />
                                                                                        <FooterStyle BorderStyle="None" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Purchase Order Date %>" SortExpression="PODate">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblPODate" runat="server" Text='<%# Convert.ToDateTime(Eval("PODate").ToString()).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle Width="130px" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id%>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblproductcode" runat="server" Text='<%#new Inv_ProductMaster(Session["DBConnection"].ToString()).GetProductCodebyProductId(Eval("ProductId").ToString()) %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterStyle BorderStyle="None" />
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Product Name %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblProductId" runat="server" Text='<%# new Inv_ProductMaster(Session["DBConnection"].ToString()).GetProductNamebyProductId(Eval("ProductId").ToString()) %>'></asp:Label>
                                                                                            <asp:Label ID="lblgvProductId" Visible="false" runat="server" Text='<%# Eval("ProductId") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterStyle BorderStyle="None" />
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance, Unit %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblUnitID" runat="server" Text='<%# Eval("UnitId").ToString() %>'
                                                                                                Visible="false"></asp:Label>
                                                                                            <asp:Label ID="lblUnit" runat="server" Text='<%# Inv_UnitMaster.GetUnitCode((Eval("UnitId").ToString()),Session["DBConnection"].ToString(),HttpContext.Current.Session["CompId"].ToString()) %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterStyle BorderStyle="None" />
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Order Quantity %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblorderqty" runat="server" Text='<%#GetAmountDecimal(Eval("Orderqty").ToString())%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterStyle BorderStyle="None" />
                                                                                        <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Free Quantity %>" HeaderStyle-HorizontalAlign="Center">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblFreeQty" runat="server" Text='<%# GetAmountDecimal(Eval("FreeQty").ToString()) %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterStyle BorderStyle="None" />
                                                                                        <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                                                    </asp:TemplateField>
                                                                                </Columns>



                                                                                <PagerStyle CssClass="pagination-ys" />

                                                                            </asp:GridView>
                                                                        </div>
                                                                        <br />
                                                                        <div class="col-md-12" runat="server" id="scrollArea" onscroll="SetDivPosition()" style="overflow: auto; max-height: 500px;">
                                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvProduct" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                                ShowFooter="true" OnRowCreated="gvProduct_RowCreated" OnRowDataBound="GvProductDetail_OnRowDataBound">
                                                                                <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton ID="IbtnPDDelete" runat="server" CausesValidation="False" CommandName='<%# Eval("SerialNo") %>' CommandArgument='<%# Eval("TransId") %>'
                                                                                                ImageUrl="~/Images/Erase.png" OnCommand="IbtnPDDelete_Command" ToolTip="<%$ Resources:Attendance,Delete %>" />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                        <ItemStyle />
                                                                                        <FooterStyle BorderStyle="None" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="PO No">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblPONo" runat="server" Text='<%# Eval("PONo") %>'></asp:Label>
                                                                                            <br />
                                                                                            <asp:Label ID="lblAdvancePayment" runat="server" Visible="false" Text='<%# Eval("PONo") %>'></asp:Label>
                                                                                            <asp:Label ID="lblPOId" runat="server" Visible="false" Text='<%# Eval("POID") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                        <ItemStyle />
                                                                                        <FooterTemplate>
                                                                                            <asp:Label ID="lblTotalQuantity" runat="server" Text="<%$ Resources:Attendance,Total %>"></asp:Label>
                                                                                        </FooterTemplate>
                                                                                        <FooterStyle BorderStyle="None" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,S No. %>">
                                                                                        <ItemTemplate>
                                                                                            <%# Container.DataItemIndex+1 %>
                                                                                            <asp:Label ID="lblSerialNO" runat="server" Visible="false" Text='<%# Eval("SerialNo") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle />
                                                                                        <FooterStyle BorderStyle="None" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id%>">
                                                                                        <ItemTemplate>
                                                                                            <table width="100%">
                                                                                                <tr>
                                                                                                    <td colspan="2">
                                                                                                        <asp:Label ID="lblproductcode" runat="server" Width="120px" Text='<%# new Inv_ProductMaster(Session["DBConnection"].ToString()).GetProductCodebyProductId(Eval("ProductId").ToString()) %>'></asp:Label>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lblstockinfo" runat="server" Text="Stock Qty :" Font-Bold="false"></asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:LinkButton ID="lnkStockInfo" runat="server" Text='<%#GetProductStock(Eval("ProductId").ToString()) %>'
                                                                                                            Font-Underline="true" ToolTip="View Detail" OnCommand="lnkStockInfo_Command"
                                                                                                            ForeColor="Blue" CommandArgument='<%# Eval("ProductId") %>'></asp:LinkButton>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </ItemTemplate>
                                                                                        <FooterStyle BorderStyle="None" />
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Product Name %>">
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lblGrdHdProductName" Text="<%$ Resources:Attendance,Product Name %>" runat="server"></asp:Label>
                                                                                            <asp:CheckBox ID="chkShortProductName1" Text="" runat="server" ToolTip="Dispay detail Name" OnCheckedChanged="chkShortProductName_CheckedChanged" AutoPostBack="true"></asp:CheckBox>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblProductId" Font-Size="10" runat="server" Text='<%#  new Inv_ProductMaster(Session["DBConnection"].ToString()).GetProductNamebyProductId(Eval("ProductId").ToString()) %>' Visible="false"></asp:Label>
                                                                                            <asp:Label ID="lblShortProductName1" Font-Size="10" runat="server" Text='<%#   new Inv_ProductMaster(Session["DBConnection"].ToString()).GetProductNamebyProductId(Eval("ProductId").ToString()).Length>15?new Inv_ProductMaster(Session["DBConnection"].ToString()).GetProductNamebyProductId(Eval("ProductId").ToString()).Substring(0,15) + "...":new Inv_ProductMaster(Session["DBConnection"].ToString()).GetProductNamebyProductId(Eval("ProductId").ToString()) %>' Visible="true"></asp:Label>
                                                                                            <asp:Label ID="lblgvProductId" Visible="false" runat="server" Text='<%# Eval("ProductId") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterStyle BorderStyle="None" />
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance, Name %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:DropDownList ID="ddlUnitName" runat="server" Width="70px" CssClass="form-control"
                                                                                                AppendDataBoundItems="true">
                                                                                            </asp:DropDownList>
                                                                                            <asp:Label ID="lblUnitID" runat="server" Text='<%# Eval("UnitId").ToString() %>'
                                                                                                Visible="false"></asp:Label>
                                                                                            <asp:Label ID="lblUnit" Visible="false" runat="server" Text='<%# Inv_UnitMaster.GetUnitCode((Eval("UnitId").ToString()),Session["DBConnection"].ToString(),HttpContext.Current.Session["CompId"].ToString()) %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterStyle BorderStyle="None" />
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance, Price %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="lblUnitRate" OnChange="SetSelectedRow(this)" runat="server" Text='<%# GetAmountDecimal(Eval("UnitCost").ToString()) %>'
                                                                                                CssClass="form-control" Width="70px" Height="10px" AutoPostBack="true" OnTextChanged="lblGvUnitRate_TextChanged"></asp:TextBox>
                                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderQtylblUnitRate" runat="server"
                                                                                                Enabled="True" TargetControlID="lblUnitRate" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                        </ItemTemplate>
                                                                                        <FooterStyle BorderStyle="None" />
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance, Cost %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblUnitRateLocal" runat="server" Text='<%# GetLocalPrice(Eval("UnitCost").ToString()) %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterStyle BorderStyle="None" />
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Order %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="OrderQty" runat="server" Text='<%# GetAmountDecimal(Eval("OrderQty").ToString()) %>'></asp:Label>





                                                                                        </ItemTemplate>
                                                                                        <FooterStyle BorderStyle="None" />
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Free %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="lblFreeQty" OnChange="SetSelectedRow(this)" runat="server" Text='<%# GetAmountDecimal(Eval("FreeQty").ToString()) %>'
                                                                                                CssClass="form-control" Width="50px" Height="10px"></asp:TextBox>
                                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderFreeQty" runat="server" Enabled="True"
                                                                                                TargetControlID="lblFreeQty" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                        </ItemTemplate>
                                                                                        <FooterStyle BorderStyle="None" />
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Quantity %>">
                                                                                        <ItemTemplate>
                                                                                            <table cellpadding="0" cellspacing="0" style="border: solid 1 #c2c2c2;">
                                                                                                <tr>
                                                                                                    <td colspan="2">
                                                                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Remain %>" Font-Bold="true"></asp:Label>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lblInvoice" runat="server" Text="<%$ Resources:Attendance,Invoice %>"></asp:Label>
                                                                                                    </td>
                                                                                                    <td>:
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lblInvRemainQty" runat="server" Text='<%# Eval("InvRemainQty") %>'></asp:Label>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Physical %>"></asp:Label>
                                                                                                    </td>
                                                                                                    <td>:
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lblRemainQty" runat="server" Text='<%# Eval("RemainQty") %>'></asp:Label>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td colspan="2">
                                                                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Received %>"
                                                                                                            Font-Bold="true"></asp:Label>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Physical %>"></asp:Label>
                                                                                                    </td>
                                                                                                    <td>:
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <%--<asp:TextBox ID="txt1RecQty" runat="server" Text='<%# Eval("RecQty") %>' Width="50px" Enabled="false" Visible="true"></asp:TextBox>--%>
                                                                                                        <asp:Label ID="lblgoodsreceive" runat="server" Text='<%# Eval("RecQty") %>'></asp:Label>

                                                                                                        <asp:Label ID="lblSupplierCurrency" runat="server" Text='<%# GetSupplierCurrency(Eval("OrderQty").ToString()) %>' Font-Size="1px"></asp:Label>

                                                                                                        <%-- <asp:Label ID="lblInvoiceCurrency" runat="server" Text='<%# GetInvoiceCurrency(Eval("OrderQty").ToString()) %>' ></asp:Label>--%>
                                                                               

                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle />
                                                                                        <FooterStyle BorderStyle="None" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Invoice %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="QtyGoodReceived" OnChange="SetSelectedRow(this)" runat="server" Text='<%# GetAmountDecimal(Eval("InvoiceQty").ToString()) %>'
                                                                                                CssClass="form-control" Width="50px" Height="10px" AutoPostBack="true" OnTextChanged="lblOrderQty_TextChanged"></asp:TextBox>
                                                                                            <asp:Label ID="lblRecQty" runat="server" Text='<%# Eval("RecQty") %>' Visible="false"></asp:Label>
                                                                                             <asp:Label ID="lblgvExpiryDate" runat="server" Text='<%# Eval("Field1") %>' Visible="false"></asp:Label>
                                                                                            <asp:Label ID="lblgvBatchNo" runat="server" Text='<%# Eval("Field2") %>' Visible="false"></asp:Label>
                                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderQtyGoodReceived" runat="server"
                                                                                                Enabled="True" TargetControlID="QtyGoodReceived" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:Label ID="txtTotalQuantity" runat="server" Text="0"></asp:Label>
                                                                                        </FooterTemplate>
                                                                                        <FooterStyle BorderStyle="None" />
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance, Price%>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblQtyAmmount" runat="server" Text='<%# GetAmountDecimal((Convert.ToDouble(Eval("InvoiceQty").ToString())*Convert.ToDouble(Eval("UnitCost").ToString())).ToString()) %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:Label ID="txtTotalGrossAmount" runat="server" Text="0"></asp:Label>
                                                                                        </FooterTemplate>
                                                                                        <FooterStyle BorderStyle="None" />
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance, Cost%>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblLocalQtyAmmount" runat="server" Text='<%# GetLocalCurrencyConversion(((Convert.ToDouble(Eval("InvoiceQty").ToString())) * (Convert.ToDouble(Eval("UnitCost").ToString())+ Convert.ToDouble(Eval("TaxV").ToString())- Convert.ToDouble(Eval("DiscountV").ToString()))).ToString()) %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:Label ID="txtLocalTotalGrossAmount" runat="server" Text="0"></asp:Label>
                                                                                        </FooterTemplate>
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,% %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblTaxAfterPrice" Visible="false" runat="server" Text="0"></asp:Label>
                                                                                            <asp:TextBox ID="lblDiscount" OnChange="SetSelectedRow(this)" runat="server" Text='<%# GetAmountDecimal(Eval("DiscountP").ToString()) %>'
                                                                                                CssClass="form-control" Width="50px" Height="10px" AutoPostBack="true" OnTextChanged="lblDiscount_TextChanged"></asp:TextBox>
                                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderDiscount" runat="server"
                                                                                                Enabled="True" TargetControlID="lblDiscount" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                        </ItemTemplate>
                                                                                        <FooterStyle BorderStyle="None" />
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Value %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="lblDiscountValue" OnChange="SetSelectedRow(this)" runat="server" Text='<%# GetAmountDecimal(Eval("DiscountV").ToString()) %>'
                                                                                                CssClass="form-control" Width="70px" Height="10px" AutoPostBack="true" OnTextChanged="lblDiscountValue_TextChanged"></asp:TextBox>
                                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderDiscountValue" runat="server"
                                                                                                Enabled="True" TargetControlID="lblDiscountValue" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:Label ID="txtTotalDiscountPrice" runat="server" Text="0"></asp:Label>
                                                                                        </FooterTemplate>
                                                                                        <FooterStyle BorderStyle="None" />
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="lblTax" OnChange="SetSelectedRow(this)" runat="server" Text='<%# GetAmountDecimal(Eval("TaxP").ToString()) %>'
                                                                                                Enabled="false" CssClass="form-control" Width="50px" Height="10px" AutoPostBack="true"
                                                                                                OnTextChanged="lblTax_TextChanged"></asp:TextBox>
                                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderTax" runat="server" Enabled="True"
                                                                                                TargetControlID="lblTax" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                            <asp:ImageButton ID="imgBtnaddtax" runat="server" CommandArgument='<%# Eval("SerialNo") %>'
                                                                                                ImageUrl="~/Images/plus.png" Width="30px" Height="30px" OnCommand="imgaddTax_Command"
                                                                                                ToolTip="Add Tax" />
                                                                                            <asp:ImageButton ID="BtnAddTax" runat="server" CommandName='<%# Eval("SerialNo") %>' CommandArgument='<%# Eval("ProductId") %>' OnCommand="BtnAddTax_Command" ImageUrl="~/Images/plus.png" Width="30px" Height="30px" ToolTip="Add Tax" />
                                                                                        </ItemTemplate>
                                                                                        <FooterStyle BorderStyle="None" />
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Value %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="lblTaxValue" OnChange="SetSelectedRow(this)" runat="server" Text='<%# GetAmountDecimal(Eval("TaxV").ToString()) %>'
                                                                                                Enabled="false" CssClass="form-control" Width="70px" Height="10px" AutoPostBack="true"
                                                                                                OnTextChanged="lblTaxValue_TextChanged"></asp:TextBox>
                                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderTaxValue" runat="server"
                                                                                                Enabled="True" TargetControlID="lblTaxValue" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:Label ID="txtTotalTaxPrice" runat="server" Text="0"></asp:Label>
                                                                                        </FooterTemplate>
                                                                                        <FooterStyle BorderStyle="None" />
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Net Price%>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblAmount" runat="server" Text="0"></asp:Label>
                                                                                            <asp:Literal runat="server" ID="lit1" Text="<tr id='trGrid'><td colspan='17' align='right'>" />
                                                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvchildGrid" runat="server" AutoGenerateColumns="false" DataKeyNames="Tax_Id"
                                                                                                Visible="false">
                                                                                                <Columns>
                                                                                                    <asp:TemplateField>
                                                                                                        <ItemTemplate>
                                                                                                            <asp:CheckBox ID="chkselecttax" runat="server" Width="10px" AutoPostBack="true" OnCheckedChanged="chkselecttax_OnCheckedChanged"
                                                                                                                Checked='<%#Eval("TaxSelected") %>' />
                                                                                                            <asp:HiddenField ID="hdntaxId" runat="server" Value='<%#Eval("Tax_Id") %>' />
                                                                                                            <asp:HiddenField ID="hdnCategoryId" runat="server" Value='<%#Eval("ProductCategoryId") %>' />
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle HorizontalAlign="Left" Width="20px" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="Category Name">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblgvcategoryName" runat="server" Width="200px" Visible="true" Text='<%#Eval("CategoryName") %>' />
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle HorizontalAlign="Left" Width="200px" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="Tax Name">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblgvtaxName" runat="server" Width="200px" Text='<%#Eval("TaxName") %>' />
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle HorizontalAlign="Left" Width="200px" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="Tax(%)">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:TextBox ID="txttaxPerchild" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                                                                OnTextChanged="txttaxPerchild_OnTextChanged" Width="100px" Enabled='<%#Eval("TaxSelected") %>'
                                                                                                                Text='<%#Eval("Tax_Per") %>'></asp:TextBox>
                                                                                                            <cc1:FilteredTextBoxExtender ID="filtertextboxtaxperchild" runat="server" Enabled="True"
                                                                                                                TargetControlID="txttaxPerchild" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="Tax Value">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:TextBox ID="txttaxValuechild" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                                                                Width="100px" Enabled='<%#Eval("TaxSelected") %>' OnTextChanged="txttaxValuechild_OnTextChanged"
                                                                                                                Text='<%#Eval("Tax_value") %>'></asp:TextBox>
                                                                                                            <cc1:FilteredTextBoxExtender ID="filtertextbox12taxvaluechild" runat="server" Enabled="True"
                                                                                                                TargetControlID="txttaxValuechild" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                                    </asp:TemplateField>
                                                                                                </Columns>

                                                                                                <PagerStyle CssClass="pagination-ys" />

                                                                                            </asp:GridView>
                                                                                            <asp:Literal runat="server" ID="lit2" Text="</td></tr>" />
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:Label ID="txtTotalNetPrice" runat="server" Font-Bold="true" Text="0"></asp:Label>
                                                                                        </FooterTemplate>
                                                                                        <FooterStyle BorderStyle="None" />
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>
                                                                                </Columns>



                                                                                <PagerStyle CssClass="pagination-ys" />

                                                                            </asp:GridView>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <hr />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblNetDisPer" runat="server" Text="<%$ Resources:Attendance,Net Discount(%)%>"></asp:Label>
                                                        <asp:TextBox ID="txtNetDisPer" runat="server" CssClass="form-control"
                                                            Text="0" AutoPostBack="True" OnTextChanged="txtNetDisPer_TextChanged" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender34" runat="server" Enabled="True"
                                                            TargetControlID="txtNetDisPer" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblNetDisVal" runat="server" Text="<%$ Resources:Attendance,Value %>"></asp:Label>
                                                        <asp:TextBox ID="txtNetDisVal" runat="server" CssClass="form-control"
                                                            Text="0" AutoPostBack="True" OnTextChanged="txtNetDisVal_TextChanged" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender35" runat="server" Enabled="True"
                                                            TargetControlID="txtNetDisVal" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div runat="server" visible="false" class="col-md-6">
                                                        <asp:Label ID="lblNetTaxPar" runat="server" Text="<%$ Resources:Attendance,Net Tax(%)%>"></asp:Label>
                                                        <asp:TextBox ID="txtNetTaxPar" runat="server" CssClass="form-control"
                                                            Text="0" AutoPostBack="True" OnTextChanged="txtNetTaxPar_TextChanged" Enabled="false" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender36" runat="server" Enabled="True"
                                                            TargetControlID="txtNetTaxPar" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblNetTaxVal" runat="server" Text="Total Tax"></asp:Label>
                                                        <asp:TextBox ID="txtNetTaxVal" runat="server" CssClass="form-control"
                                                            Text="0" AutoPostBack="True" OnTextChanged="txtNetTaxVal_TextChanged" Enabled="false" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender37" runat="server" Enabled="True"
                                                            TargetControlID="txtNetTaxVal" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>


                                                    <div id="Div_Tax_Grid" runat="server" visible="false" style="display: none;" class="col-md-12">
                                                        <div style="overflow: auto">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gridView" ShowHeader="true" runat="server" AutoGenerateColumns="false"
                                                                Width="100%" DataKeyNames="Tax_Id" ShowFooter="true" OnRowCancelingEdit="gridView_RowCancelingEdit"
                                                                OnRowDeleting="gridView_RowDeleting" OnRowEditing="gridView_RowEditing" OnRowUpdating="gridView_RowUpdating"
                                                                OnRowCommand="gridView_RowCommand">

                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Tax Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTaxname" runat="server" Text='<%#Eval("TaxName") %>'></asp:Label>
                                                                            <asp:HiddenField ID="hdnTaxId" runat="server" Value='<%#Eval("Tax_Id") %>' />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtTaxName" runat="server" Font-Names="Verdana" AutoPostBack="true"
                                                                                OnTextChanged="txtTaxName_TextChanged" CssClass="form-control" BackColor="#eeeeee"
                                                                                Text='<%#Eval("TaxName") %>' CausesValidation="false"></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender ID="autoComplete122566" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionListTax" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtTaxName" UseContextKey="True"
                                                                                CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                        </EditItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:TextBox ID="txtTaxFooter" runat="server" Font-Names="Verdana" AutoPostBack="false"
                                                                                OnTextChanged="txtTaxFooter_TextChanged" CssClass="form-control" BackColor="#eeeeee"
                                                                                CausesValidation="true" Width="370px"></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender ID="autoComplete12256660" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionListTax" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtTaxFooter"
                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                        </FooterTemplate>
                                                                        <ItemStyle Width="10%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Tax(%)">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTaxper" runat="server" Text='<%#Eval("Tax_Per") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtTaxper" runat="server" Font-Names="Verdana" CssClass="form-control"
                                                                                Text='<%#Eval("Tax_Per") %>' CausesValidation="true" AutoPostBack="true"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FiltergvSalesQuantity11488taxper" runat="server"
                                                                                Enabled="True" TargetControlID="txtTaxper" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </EditItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:TextBox ID="txtTaxperFooter" runat="server" Font-Names="Verdana" CssClass="form-control"
                                                                                Text='<%#Eval("Tax_per") %>' CausesValidation="true" AutoPostBack="true" OnTextChanged="txtTaxperFooter_OnTextChanged"
                                                                                Width="100px"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FiltergvSalesQuantity2taxper" runat="server" Enabled="True"
                                                                                TargetControlID="txtTaxperFooter" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </FooterTemplate>
                                                                        <ItemStyle Width="8%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Tax Value">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTaxValue" runat="server" Text='<%#Eval("Tax_Value") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtTaxValue" runat="server" Font-Names="Verdana" CssClass="form-control"
                                                                                Text='<%#Eval("Tax_Value") %>' CausesValidation="true" AutoPostBack="false"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FiltergvSalesQuantity11488" runat="server" Enabled="True"
                                                                                TargetControlID="txtTaxValue" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </EditItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:TextBox ID="txtTaxValueFooter" runat="server" Font-Names="Verdana" CssClass="form-control"
                                                                                Text='<%#Eval("Tax_Value") %>' CausesValidation="true" AutoPostBack="true" OnTextChanged="txtTaxValueFooter_OnTextChanged"
                                                                                Width="100px"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FiltergvSalesQuantity2" runat="server" Enabled="True"
                                                                                TargetControlID="txtTaxValueFooter" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </FooterTemplate>
                                                                        <ItemStyle Width="8%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <EditItemTemplate>
                                                                            <asp:Button ID="ButtonUpdate" runat="server" CommandName="Update" Text="Update" CausesValidation="true"
                                                                                CommandArgument='<%#Eval("Tax_Id") %>' />
                                                                            <asp:Button ID="ButtonCancel" runat="server" CommandName="Cancel" Text="Cancel" />
                                                                        </EditItemTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Button ID="ButtonEdit" runat="server" CssClass="btn btn-warning" CommandName="Edit" Text="Edit" Visible="false" />
                                                                            <asp:Button ID="ButtonDelete" runat="server" CssClass="btn btn-warning" CommandName="Delete" Text="Delete" CommandArgument='<%#Eval("Tax_Id") %>' />
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Button ID="ButtonAdd" runat="server" CssClass="btn btn-warning" CommandName="AddNew" Text="Add New Row" />
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>

                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblNetAmount" runat="server" Text="<%$ Resources:Attendance,Net Amount %>" />
                                                        <asp:TextBox ID="txtNetAmount" runat="server" Visible="false" CssClass="form-control" Text="0" />
                                                        <asp:TextBox ID="txtGrandTotal" runat="server" CssClass="form-control" ReadOnly="true">0</asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lbllocalGrandtotal" runat="server" Text="<%$ Resources:Attendance,Net Amount %>" />
                                                        <asp:TextBox ID="txtlocalGrandtotal" runat="server" CssClass="form-control" ReadOnly="true">0</asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" id="ctlRemark" runat="server">
                                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Remark %>"></asp:Label>
                                                        <asp:TextBox ID="txRemark" runat="server" CssClass="form-control" TextMode="MultiLine" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div style="overflow: auto">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvadvancepayment" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                ShowFooter="true" BorderStyle="Solid" Width="100%" PageSize="<%# PageControlCommon.GetPageSize() %>">

                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order No. %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblorderNo" runat="server" Text='<%# Eval("OrderNo").ToString() %>' />
                                                                            <%-- <asp:ImageButton ID="IbtnDeletePay" runat="server" CausesValidation="False" CommandArgument='<%# Eval("TransId") %>'
                                                                                                        ImageUrl="~/Images/Erase.png" Width="16px" ToolTip="<%$ Resources:Attendance,Delete %>"
                                                                                                        OnCommand="IbtnDeletePay_Command" Visible="false" />--%>
                                                                        </ItemTemplate>
                                                                        <FooterStyle BorderStyle="None" />
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Payment Mode %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvPaymentMode" runat="server" Text='<%# Eval("PaymentName").ToString() %>' />
                                                                        </ItemTemplate>
                                                                        <FooterStyle BorderStyle="None" />
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Account Name%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvaccountno" runat="server" Text='<%# Eval("AccountName").ToString() %>' />
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lbltotExp" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Advance Payment%> " />
                                                                        </FooterTemplate>
                                                                        <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Paid Amount %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("Pay_Charges").ToString() %>' />
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="txttotAmount" runat="server" Font-Bold="true" Text="0" />
                                                                        </FooterTemplate>
                                                                        <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                </Columns>


                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <cc1:TabContainer ID="tabContainer" runat="server" CssClass="ajax__tab_yuitabview-theme" ActiveTabIndex="0">
                                                            <cc1:TabPanel ID="TabPayment" runat="server" Width="100%" HeaderText="Payment Mode">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_TabPayment" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">

                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label28" runat="server" Text="Purchase Account"></asp:Label>
                                                                                    <asp:TextBox ID="txtPurchaseAccount" runat="server" CssClass="form-control" onchange="txtPayAccountNo_TextChanged(this)" BackColor="#eeeeee"></asp:TextBox>
                                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                                                        Enabled="True" ServiceMethod="GetCompletionListAccountNo" ServicePath="" CompletionInterval="100"
                                                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtPurchaseAccount"
                                                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                    </cc1:AutoCompleteExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Lbl_Nanation" runat="server" Text="<%$ Resources:Attendance,Narration %>"></asp:Label>
                                                                                    <asp:TextBox ID="Txt_Narration" runat="server" MaxLength="250" CssClass="form-control"></asp:TextBox>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label17" runat="server" Text="<%$ Resources:Attendance,Payment Mode %>"></asp:Label>
                                                                                    <a style="color: Red">*</a>
                                                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator11" ValidationGroup="Add_Container" Display="Dynamic"
                                                                                        SetFocusOnError="true" ControlToValidate="ddlPaymentMode" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Payment Mode %>" />

                                                                                    <asp:DropDownList ID="ddlPaymentMode" runat="server" CssClass="form-control"
                                                                                        AutoPostBack="True" OnSelectedIndexChanged="ddlPaymentMode_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblPayAccountNo" runat="server" Text="<%$ Resources:Attendance,Account No. %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtPayAccountNo" runat="server" CssClass="form-control" onchange="txtPayAccountNo_TextChanged(this)" BackColor="#eeeeee"></asp:TextBox>
                                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                                                                        Enabled="True" ServiceMethod="GetCompletionListAccountNo" ServicePath="" CompletionInterval="100"
                                                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtPayAccountNo"
                                                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                    </cc1:AutoCompleteExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label20" runat="server" Text="<%$ Resources:Attendance,Currency %>"></asp:Label>
                                                                                    <a style="color: Red">*</a>
                                                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator12" ValidationGroup="Add_Container" Display="Dynamic"
                                                                                        SetFocusOnError="true" ControlToValidate="ddlPayCurrency" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Currency %>" />

                                                                                    <asp:DropDownList ID="ddlPayCurrency" runat="server" CssClass="form-control" onchange="ddlPayCurrency_SelectedIndexChanged(this)">
                                                                                    </asp:DropDownList>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label21" runat="server" Text="<%$ Resources:Attendance,Exchange Rate %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtPayExchangeRate" runat="server" CssClass="form-control" ReadOnly="true"
                                                                                        AutoPostBack="true" OnTextChanged="txtFCPayCharges_TextChanged">0</asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                                        TargetControlID="txtPayExchangeRate" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblPayAmmount" runat="server" Text="<%$ Resources:Attendance,Balance Amount%>"></asp:Label>
                                                                                    <a style="color: Red">*</a>
                                                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator13" ValidationGroup="Add_Container"
                                                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtFCPayCharges" ErrorMessage="<%$ Resources:Attendance,Enter Balance Amount %>"></asp:RequiredFieldValidator>

                                                                                    <asp:TextBox ID="txtFCPayCharges" runat="server" CssClass="form-control"
                                                                                        AutoPostBack="true" OnTextChanged="txtFCPayCharges_TextChanged"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                                                                        TargetControlID="txtFCPayCharges" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label12" runat="server" Text="Payment Amount (Local)" />
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtLCPayCharges" runat="server" ReadOnly="true" CssClass="form-control">0</asp:TextBox>
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                                            TargetControlID="txtLCPayCharges" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <div class="input-group-btn">
                                                                                            <asp:LinkButton runat="server" ValidationGroup="Add_Container" ToolTip="<%$ Resources:Attendance,Add %>" ID="btnPaymentSave" OnClick="btnPaymentSave_Click"><i class="fa fa-plus-square" style="font-size:35px;padding:5px;margin-top: -5px;"></i></asp:LinkButton>
                                                                                        </div>
                                                                                    </div>
                                                                                    <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6" id="trBank" runat="server" visible="false">
                                                                                    <asp:Label ID="lblPayBank" runat="server" Text="<%$ Resources:Attendance,Bank %>"
                                                                                        Visible="false"></asp:Label>
                                                                                    <asp:DropDownList ID="ddlPayBank" runat="server" CssClass="form-control" Visible="false">
                                                                                    </asp:DropDownList>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-12" id="trcheque" runat="server" visible="false">
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblPayChequeNo" runat="server" Text="<%$ Resources:Attendance,Cheque No %>"></asp:Label>
                                                                                        <asp:TextBox ID="txtPayChequeNo" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblPayChequeDate" runat="server" Text="<%$ Resources:Attendance,Cheque Date %>"></asp:Label>
                                                                                        <asp:TextBox ID="txtPayChequeDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtChequedate_CalenderExtender" runat="server" TargetControlID="txtPayChequeDate">
                                                                                        </cc1:CalendarExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                </div>

                                                                                <div class="col-md-12" id="trcard" runat="server" visible="false">
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblPayCardNo" runat="server" Text="<%$ Resources:Attendance,Card No %>"></asp:Label>
                                                                                        <asp:TextBox ID="txtPayCardNo" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblPayCardName" runat="server" Text="<%$ Resources:Attendance,Card Name %>"></asp:Label>
                                                                                        <asp:TextBox ID="txtPayCardName" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                        <br />
                                                                                    </div>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <div style="overflow: auto">
                                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvPayment" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                                                                            BorderStyle="Solid" Width="100%">

                                                                                            <Columns>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:ImageButton ID="IbtnDeletePay" runat="server" CausesValidation="False" CommandArgument='<%# Eval("TransId") %>'
                                                                                                            ImageUrl="~/Images/Erase.png" ToolTip="<%$ Resources:Attendance,Delete %>" Width="16px"
                                                                                                            OnCommand="btnDeletePay_Command" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterStyle BorderStyle="None" />
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Payment Mode %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvExpense_Id" runat="server" Text='<%# Eval("PaymentName") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lbltotExp" runat="server" Font-Bold="true" Text="Total Payment" /><b>:</b>
                                                                                                    </FooterTemplate>
                                                                                                    <FooterStyle BorderStyle="None" />
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Currency %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvExpCurrencyID" runat="server" Text='<%# CurrencyName(Eval("PayCurrencyID").ToString()) %>' />
                                                                                                        <asp:Label ID="hidExpCur" runat="server" Visible="false" Text='<%# Eval("PayCurrencyID").ToString() %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterStyle BorderStyle="None" />
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Payment Amount">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvFCExchangeAmount" runat="server" Text='<%# Eval("FCPayAmount") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="txttotFCExpShow" runat="server" Font-Bold="true" Text="0" />
                                                                                                    </FooterTemplate>
                                                                                                    <FooterStyle BorderStyle="None" />
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Account Name%>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvaccountno" runat="server" Text='<%# Eval("AccountName").ToString() %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterStyle BorderStyle="None" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Exchange Rate %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvExpExchangeRate" runat="server" Text='<%# Eval("PayExchangeRate") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Payment Amount (Local)" SortExpression="Pay_Charges">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvExp_Charges" runat="server" Text='<%# Eval("Pay_Charges") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="txttotExp" runat="server" Visible="false" Font-Bold="true" Text="0" />
                                                                                                        <asp:Label ID="txttotExpShow" runat="server" Font-Bold="true" Text="0" />
                                                                                                    </FooterTemplate>
                                                                                                    <FooterStyle BorderStyle="None" />
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                            </Columns>


                                                                                            <PagerStyle CssClass="pagination-ys" />

                                                                                        </asp:GridView>
                                                                                    </div>
                                                                                    <br />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_TabPayment">
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
                                                            <cc1:TabPanel ID="TabExpenses" runat="server" Width="100%" HeaderText="Expenses">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_TabExpenses" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">



                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Currency %>"></asp:Label>
                                                                                    <a style="color: Red">*</a>
                                                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator8" ValidationGroup="Add" Display="Dynamic"
                                                                                        SetFocusOnError="true" ControlToValidate="ddlExpCurrency" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Currency %>" />

                                                                                    <asp:DropDownList ID="ddlExpCurrency" runat="server" CssClass="form-control"
                                                                                        AutoPostBack="True" OnSelectedIndexChanged="ddlExpCurrency_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                    <br />
                                                                                </div>

                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Exchange Rate %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtExpExchangeRate" runat="server" CssClass="form-control">0</asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender41" runat="server" Enabled="True"
                                                                                        TargetControlID="txtExpExchangeRate" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <br />
                                                                                </div>

                                                                                <%--<div class="col-md-6">
                                                                                    <asp:Label ID="Label29" runat="server" Text="Debit Account"></asp:Label>
                                                                                    <asp:TextBox ID="Txt_Debit_Expenses_Account" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                                                    <br />
                                                                                </div>--%>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblSelectExp" runat="server" Text="<%$ Resources:Attendance,Select Expenses %>" />
                                                                                    <a style="color: Red">*</a>
                                                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator9" ValidationGroup="Add" Display="Dynamic"
                                                                                        SetFocusOnError="true" ControlToValidate="ddlExpense" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Expenses %>" />

                                                                                    <asp:DropDownList ID="ddlExpense" runat="server" AutoPostBack="True"
                                                                                        OnSelectedIndexChanged="ddlExpense_SelectedIndexChanged" CssClass="form-control">
                                                                                    </asp:DropDownList>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblExpAccount" runat="server" Text="<%$ Resources:Attendance,Expenses Account %>"></asp:Label>
                                                                                    <a style="color: Red">*</a>
                                                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator10" ValidationGroup="Add"
                                                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtExpensesAccount" ErrorMessage="<%$ Resources:Attendance,Enter Expenses Account %>"></asp:RequiredFieldValidator>

                                                                                    <asp:TextBox ID="txtExpensesAccount" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                                        OnTextChanged="txtExpensesAccount_TextChanged" BackColor="#eeeeee" />
                                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                                                        Enabled="True" ServiceMethod="GetCompletionListAccountNo" ServicePath="" CompletionInterval="100"
                                                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtExpensesAccount"
                                                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                    </cc1:AutoCompleteExtender>
                                                                                    <br />
                                                                                </div>

                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label31" runat="server" Text="Exp Invoice No"></asp:Label>
                                                                                    <asp:TextBox ID="txtExpInvoiceNo" runat="server" CssClass="form-control">0</asp:TextBox>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label33" runat="server" Text="Exp Invoice Date"></asp:Label>
                                                                                    <asp:TextBox ID="txtExpInvoiceDate" runat="server" CssClass="form-control" />
                                                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" TargetControlID="txtExpInvoiceDate">
                                                                                    </cc1:CalendarExtender>
                                                                                    <br />
                                                                                </div>


                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblFCExpAmount" runat="server" Text="Expenses Amount"></asp:Label>
                                                                                    <asp:TextBox ID="txtFCExpAmount" runat="server" CssClass="form-control"
                                                                                        AutoPostBack="true" OnTextChanged="txtFCExpAmount_TextChanged">0</asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender40" runat="server" Enabled="True"
                                                                                        TargetControlID="txtFCExpAmount" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <br />
                                                                                </div>

                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblExpCharges" runat="server" Text="Expenses Amount (Local)" />
                                                                                    <%--<div class="input-group">--%>
                                                                                    <asp:TextBox ID="txtExpCharges" runat="server" Enabled="false" ReadOnly="true" CssClass="form-control">0</asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender42" runat="server" Enabled="True"
                                                                                        TargetControlID="txtExpCharges" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <%--<div class="input-group-btn">
                                                                                            <asp:ImageButton runat="server" ValidationGroup="Add" ImageUrl="~/Images/add.png"
                                                                                                Height="29px" ToolTip="<%$ Resources:Attendance,Add %>" ID="IbtnAddExpenses"
                                                                                                OnClick="IbtnAddExpenses_Click"></asp:ImageButton>
                                                                                        </div>--%>
                                                                                    <%--</div>--%>
                                                                                    <asp:HiddenField ID="hdnProductExpenses" runat="server" Value="0" />
                                                                                    <asp:HiddenField ID="hdnfPSC" runat="server" />
                                                                                    <br />
                                                                                </div>
                                                                                <div id="Div_Add_Tax" runat="server" class="col-md-6">
                                                                                    <asp:Label ID="Label55" runat="server" Text="<%$ Resources:Attendance,Expenses Tax Amount%>"></asp:Label>
                                                                                    <div class="input-group" style="width: 100%;">
                                                                                        <asp:Label ID="Lbl_Expenses_Tax_Amount_ET" CssClass="form-control" runat="server"></asp:Label>
                                                                                        <div class="input-group-addon" style="width: 25%">
                                                                                            <asp:Label ID="Lbl_Expenses_Tax_ET" runat="server"></asp:Label>
                                                                                        </div>
                                                                                        <div class="input-group-btn">
                                                                                            <asp:Button ID="Btn_Add_Expenses_Tax" runat="server" ValidationGroup="Add" Text="Add Tax" OnClick="Btn_Add_Expenses_Tax_Click" CssClass="btn btn-info" />
                                                                                        </div>
                                                                                    </div>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6" style="text-align: center">
                                                                                    <%--<asp:Button ID="Btn_Add_Expenses_Tax" runat="server" ValidationGroup="Add" Text="Add Tax" OnClick="Btn_Add_Expenses_Tax_Click" CssClass="btn btn-info" />--%>
                                                                                    <br />
                                                                                    <asp:Button ID="Btn_Add_Expenses" Visible="false" ValidationGroup="Add" runat="server" Text="Add Expenses" OnClick="Btn_Add_Expenses_Click" CssClass="btn btn-info" />

                                                                                    <asp:Button ID="Btn_Exp_Reset" Visible="false" runat="server" Text="Reset" OnClick="Btn_Exp_Reset_Click" CssClass="btn btn-warning" />
                                                                                </div>

                                                                                <div class="col-md-12">
                                                                                    <br />
                                                                                    <div style="overflow: auto">
                                                                                        <asp:UpdatePanel ID="Update_Grid_Exp" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                                                                            <Triggers>
                                                                                                <asp:AsyncPostBackTrigger ControlID="GridExpenses" />
                                                                                            </Triggers>
                                                                                            <ContentTemplate>
                                                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GridExpenses" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                                                                                    BorderStyle="Solid" Width="100%">

                                                                                                    <Columns>
                                                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:ImageButton ID="IbtnDeleteExp" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Expense_Id") %>'
                                                                                                                    ImageUrl="~/Images/Erase.png" Width="16px" ToolTip="<%$ Resources:Attendance,Delete %>"
                                                                                                                    OnCommand="IbtnDeleteExp_Command" />
                                                                                                            </ItemTemplate>
                                                                                                            <FooterStyle BorderStyle="None" />
                                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Expenses %>">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:HiddenField ID="Hdn_Expense_Id_GV" runat="server" Value='<%# Eval("Expense_Id") %>' />
                                                                                                                <asp:Label ID="lblgvExpense_Id" runat="server" Text='<%# GetExpName(Eval("Expense_Id").ToString()) %>' />
                                                                                                            </ItemTemplate>
                                                                                                            <FooterStyle BorderStyle="None" />
                                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                                            <ItemStyle />
                                                                                                        </asp:TemplateField>
                                                                                                        <%--<asp:TemplateField HeaderText="Debit Account Name">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="Lbl_Debit_Account" runat="server" Text='<%#getAccountname(Eval("Debit_Account_No").ToString()) %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterStyle BorderStyle="None" />
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <ItemStyle  />
                                                                                                </asp:TemplateField>--%>
                                                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Currency %>" Visible="false">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblgvExpCurrencyID" runat="server" Text='<%# CurrencyName(Eval("ExpCurrencyID").ToString()) %>' />
                                                                                                                <asp:Label ID="hidExpCur" runat="server" Visible="false" Text='<%# Eval("ExpCurrencyID").ToString() %>' />
                                                                                                            </ItemTemplate>
                                                                                                            <FooterStyle BorderStyle="None" />
                                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                                            <ItemStyle />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Invoice No(Exp)" Visible="true">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblExpInvoiceNo" runat="server" Visible="true" Text='<%# Eval("field3").ToString() %>' />
                                                                                                            </ItemTemplate>
                                                                                                            <FooterStyle BorderStyle="None" />
                                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                                            <ItemStyle />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Invoice Date(Exp)" Visible="true">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblExpInvoiceDate" runat="server" Visible="true" Text='<%# Eval("field4").ToString() %>' />
                                                                                                            </ItemTemplate>
                                                                                                            <FooterStyle BorderStyle="None" />
                                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                                            <ItemStyle />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Expenses Amount">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblgvFCExchangeAmount" runat="server" Text='<%# Eval("FCExpAmount") %>' />
                                                                                                            </ItemTemplate>
                                                                                                            <FooterStyle BorderStyle="None" />
                                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                                            <ItemStyle />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Exchange Rate %>">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblgvExpExchangeRate" runat="server" Text='<%# Eval("ExpExchangeRate") %>' />
                                                                                                            </ItemTemplate>
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lbltotExp" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Expenses%>" /><b>:</b>
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                                            <ItemStyle />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Expenses Amount (Local)" SortExpression="Exp_Charges">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblgvExp_Charges" runat="server" Text='<%# Eval("Exp_Charges") %>' />
                                                                                                            </ItemTemplate>
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="txttotExp" runat="server" Visible="false" Font-Bold="true" Text="0" />
                                                                                                                <asp:Label ID="txttotExpShow" runat="server" Font-Bold="true" Text="0" />
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle BorderStyle="None" />
                                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                                            <ItemStyle />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Account Name %>">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblgvaccount_Id" runat="server" Text='<%#getAccountname(Eval("Account_No").ToString()) %>' />
                                                                                                            </ItemTemplate>
                                                                                                            <FooterStyle BorderStyle="None" />
                                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                                            <ItemStyle />
                                                                                                        </asp:TemplateField>




                                                                                                        <asp:TemplateField HeaderText="Tax (%)">
                                                                                                            <ItemTemplate>
                                                                                                                <div class="input-group">
                                                                                                                    <asp:Label ID="Lbl_Expenses_Tax_Percent_GV" CssClass="form-control" Text='<%#SetDecimal(Eval("F_Tax_Percent").ToString()) %>' runat="server"></asp:Label>
                                                                                                                    <div class="input-group-btn">
                                                                                                                        <asp:ImageButton ID="Imgbtn_Expesnses_Tax_GV" runat="server" ImageUrl="~/Images/plus.png" Width="30px" Height="30px" CommandArgument='<%# Eval("Expense_Id") %>' OnCommand="Imgbtn_Expesnses_Tax_GV_Command" ToolTip="View Tax on Expenses" />
                                                                                                                    </div>
                                                                                                                </div>
                                                                                                            </ItemTemplate>
                                                                                                            <FooterStyle BorderStyle="None" />
                                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                                            <ItemStyle />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Tax Value">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="Lbl_Expenses_Tax_Value_GV" Text='<%#SetDecimal(Eval("F_Tax_Value").ToString()) %>' runat="server"></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="Lbl_Total_Tax_Value_Footer" runat="server" Font-Bold="true" Text="0" />
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle BorderStyle="None" />
                                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                                            <ItemStyle />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Line Total %>">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="Lbl_Line_Total_GV" runat="server" Text='<%#SetDecimal(Eval("Line_Total").ToString()) %>' />
                                                                                                            </ItemTemplate>
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="Lbl_Line_Total_Footer" runat="server" Font-Bold="true" Text="0" />
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle BorderStyle="None" />
                                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                                            <ItemStyle />
                                                                                                        </asp:TemplateField>
                                                                                                    </Columns>


                                                                                                    <PagerStyle CssClass="pagination-ys" />

                                                                                                </asp:GridView>
                                                                                            </ContentTemplate>
                                                                                        </asp:UpdatePanel>

                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_TabExpenses">
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
                                                            <cc1:TabPanel ID="Tabshippinginformation" runat="server" Width="100%" HeaderText="Shipping Information"
                                                                Visible="false">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_Tabshippinginformation" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Shipping Line %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtShippingLine" runat="server" BackColor="#eeeeee" CssClass="form-control"
                                                                                        AutoPostBack="True" OnTextChanged="txtShippingLine_TextChanged" />
                                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" CompletionInterval="100"
                                                                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListShippingLine"
                                                                                        ServicePath="" TargetControlID="txtShippingLine" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                    </cc1:AutoCompleteExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Ship By %>"></asp:Label>
                                                                                    <asp:DropDownList ID="ddlShipBy" runat="server" CssClass="form-control">
                                                                                        <asp:ListItem Text="<%$ Resources:Attendance, By Track %>" Value="By Track"></asp:ListItem>
                                                                                        <asp:ListItem Text="<%$ Resources:Attendance, By Ship %>" Value="By Ship"></asp:ListItem>
                                                                                        <asp:ListItem Text="<%$ Resources:Attendance, By Train %>" Value="By Train"></asp:ListItem>
                                                                                        <asp:ListItem Text="<%$ Resources:Attendance, By Air Freight %>" Value="By Air Freight"></asp:ListItem>
                                                                                        <asp:ListItem Text="<%$ Resources:Attendance, By Courier %>" Value="By Courier"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label18" runat="server" Text="<%$ Resources:Attendance,Airway Bill No. %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtAirwaybillno" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label23" runat="server" Text="<%$ Resources:Attendance,Actual Weight %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtTotalWeight" runat="server" CssClass="form-control" />
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                                                        TargetControlID="txtTotalWeight" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Attendance,Ship Unit %>"></asp:Label>
                                                                                    <asp:DropDownList ID="ddlShipUnit" runat="server" CssClass="form-control">
                                                                                    </asp:DropDownList>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label19" runat="server" Text="<%$ Resources:Attendance,Volumetric weight %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtvolumetricweight" runat="server" CssClass="form-control" />
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True"
                                                                                        TargetControlID="txtvolumetricweight" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Attendance,Unit Rate %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtUnitRate" runat="server" CssClass="form-control" />
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                                                        TargetControlID="txtUnitRate" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label24" runat="server" Text="<%$ Resources:Attendance,Shipping Date %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtShippingDate" runat="server" CssClass="form-control" />
                                                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender2" runat="server" TargetControlID="txtShippingDate">
                                                                                    </cc1:CalendarExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label25" runat="server" Text="<%$ Resources:Attendance,Receiving Date %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtReceivingDate" runat="server" CssClass="form-control" />
                                                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender3" runat="server" TargetControlID="txtReceivingDate">
                                                                                    </cc1:CalendarExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label26" runat="server" Text="<%$ Resources:Attendance,Shipment Type %>"></asp:Label>
                                                                                    <asp:DropDownList ID="ddlShipmentType" runat="server" CssClass="form-control">
                                                                                        <asp:ListItem Text="<%$ Resources:Attendance, FOB (Freight On Board) %>" Value="FOB(Frieght On Board)"></asp:ListItem>
                                                                                        <asp:ListItem Text="<%$ Resources:Attendance, EX-Work %>" Value="EX-Work"></asp:ListItem>
                                                                                        <asp:ListItem Text="<%$ Resources:Attendance, C&F%>" Value="C&F"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label123" runat="server" Text="<%$ Resources:Attendance,Freight Status %>"></asp:Label>
                                                                                    <asp:DropDownList ID="ddlFreightStatus" runat="server" CssClass="form-control">
                                                                                        <asp:ListItem Text="<%$ Resources:Attendance, Paid %>" Value="Y"></asp:ListItem>
                                                                                        <asp:ListItem Text="<%$ Resources:Attendance, UnPaid %>" Value="N"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <div style="overflow: auto">
                                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvShippingInformation" ShowHeader="true" runat="server" AutoGenerateColumns="false"
                                                                                            Width="100%">

                                                                                            <Columns>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Serial No%>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblproductCode" runat="server" Width="100px"
                                                                                                            Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Lenght">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lbllength" runat="server" Text='<%#Eval("Length") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Height">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblheight" runat="server" Text='<%#Eval("Height") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Width">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblwidth" runat="server" Text='<%#Eval("Width") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Cartons">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblcartons" runat="server" Text='<%#Eval("Cartons") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Total">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblTotal" runat="server" Text='<%#Eval("Total") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                            </Columns>

                                                                                            <PagerStyle CssClass="pagination-ys" />

                                                                                        </asp:GridView>
                                                                                    </div>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblTotalVolume" runat="server" Text="Total Volume"></asp:Label>
                                                                                    <asp:TextBox ID="txttotalVolume" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label145" runat="server" Text="Divide by"></asp:Label>
                                                                                    <asp:TextBox ID="txtdivideby" runat="server" CssClass="form-control" Enabled="false"
                                                                                        AutoPostBack="true" OnTextChanged="txtdivideby_OnTextChanged"></asp:TextBox>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label27" runat="server" Text="Volumetric weight"></asp:Label>
                                                                                    <asp:TextBox ID="txttotalvolumetricweight" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                                                    <br />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_Tabshippinginformation">
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
                                                    <div class="col-md-6">
                                                        <asp:TextBox ID="txtOtherCharges" runat="server" CssClass="form-control" Visible="False">0</asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender44" runat="server" Enabled="True"
                                                            TargetControlID="txtOtherCharges" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Button ID="btnPost" runat="server" OnClick="btnPost_Click" Text="Post & Update"
                                                            Visible="false" CssClass="btn btn-primary" ValidationGroup="Save" OnClientClick="return validate1(); if (!Page_ClientValidate()){ return false; } this.disabled = true; this.value = 'Saving...';" />
                                                        <%-- <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="Are you sure to post the record ?"
                                                                    TargetControlID="btnPost">
                                                                </cc1:ConfirmButtonExtender>--%>

                                                        <asp:Button ID="btnSave" ValidationGroup="Save" runat="server" Text="<%$ Resources:Attendance,Save %>" CssClass="btn btn-success"
                                                            Visible="false" OnClick="btnSave_Click" />

                                                        <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                            CssClass="btn btn-primary" OnClick="btnReset_Click" />

                                                        <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                            CssClass="btn btn-danger" OnClick="btnCancel_Click" />
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
                    <div class="tab-pane" id="Pending_Order">
                        <asp:UpdatePanel ID="Update_Pending_Order" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="Btn_Pending_Order" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div2" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label30" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				<asp:Label ID="lblQTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records : 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlQSeleclField" runat="server" CssClass="form-control"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlQSeleclField_SelectedIndexChanged">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Purchase Order No %>"
                                                            Value="PONo"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Purchase Order Date %>" Value="PODate"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Supplier %>" Value="Supplier_Name"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlQOption" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-5">
                                                    <asp:Panel ID="Panel" runat="server" DefaultButton="ImgBtnQBind">
                                                        <asp:TextBox ID="txtQValue" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                                        <asp:TextBox ID="txtQValueDate" runat="server" CssClass="form-control" Visible="false" placeholder="Search From Date"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" Format="dd-MMM-yyyy" ID="CalendarExtendertxtQValue" runat="server" TargetControlID="txtQValueDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="ImgBtnQBind" runat="server" CausesValidation="False" OnClick="ImgBtnQBind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="ImgBtnQRefresh" runat="server" CausesValidation="False" OnClick="ImgBtnQRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid" <%= gvPurchaseOrder.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvPurchaseOrder" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        DataKeyNames="TransID" runat="server" AutoGenerateColumns="False" Width="100%" CurrentSortField="PODate" CurrentSortDirection="DESC"
                                                        AllowPaging="false" AllowSorting="True"
                                                        OnSorting="gvPurchaseOrder_OnSorting">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Order No" SortExpression="PONo">
                                                                <ItemTemplate>
                                                                    <%# Eval("PONo") %>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Order Date" SortExpression="PODate">
                                                                <ItemTemplate>
                                                                    <%# GetDateFromat(Eval("PODate").ToString()) %>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Supplier %>" SortExpression="Supplier_Name">
                                                                <ItemTemplate>
                                                                    <%# Eval("Supplier_Name").ToString() %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Product Code" SortExpression="ProductCode">
                                                                <ItemTemplate>
                                                                    <%#Eval("ProductCode") %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Unit Name" SortExpression="Unit_Name">
                                                                <ItemTemplate>
                                                                    <%#Eval("Unit_Name").ToString() %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Order Qty" SortExpression="OrderQty">
                                                                <ItemTemplate>
                                                                    <%# SetDecimal(Eval("OrderQty").ToString()) %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Remain Qty" SortExpression="RemainQty">
                                                                <ItemTemplate>
                                                                    <%#SetDecimal(Eval("RemainQty").ToString()) %>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                        </Columns>


                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                </div>
                                                <asp:HiddenField ID="hdnGvPurchaseOrderCurrentPageIndex" runat="server" Value="1" />
                                                <div class="col-md-12">

                                                    <asp:Repeater ID="rptPagerPO" runat="server">
                                                        <ItemTemplate>
                                                            <ul class="pagination">
                                                                <li class='<%# Convert.ToBoolean(Eval("Enabled")) ? "page-item" : "page-item active" %>'>
                                                                    <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                                                        CssClass="page-link"
                                                                        OnClick="PagePO_Changed" OnClientClick='<%# !Convert.ToBoolean(Eval("Enabled")) ? "return false;" : "" %>'></asp:LinkButton>
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
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvTaxCalculation" runat="server" Width="100%" AutoGenerateColumns="false">
                                        <Columns>
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
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="Update_Modal_Button_GST" runat="server">
                        <ContentTemplate>
                            <%--<asp:Button ID="btnSaveGST" Visible="false" runat="server" CssClass="btn btn-primary" ValidationGroup="S_update"
                                Text="<%$ Resources:Attendance,Save %>" OnClick="btnSaveGST_Click" />

                            <asp:Button ID="btnCancelGST" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                Text="<%$ Resources:Attendance,Reset %>" Visible="false" />--%>
                            <asp:HiddenField ID="Hdn_Serial_No_Tax" runat="server" />
                            <asp:HiddenField ID="Hdn_Product_Id_Tax" runat="server" />
                            <asp:Button ID="Btn_Update_Tax" runat="server" CssClass="btn btn-primary"
                                Text="<%$ Resources:Attendance,Update %>" OnClick="Btn_Update_Tax_Click" />
                            <button id="btnClosePopup" type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="Product_StockAnalysis" tabindex="-1" role="dialog" aria-labelledby="Product_ModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Product_StockAnalysis1">
                        <asp:Label ID="Label32" runat="server" Text="<%$ Resources:Attendance,Location Stock %>"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-12">
                            <SA:StockAnalysis runat="server" ID="modelSA" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="Modal_Expenses_Tax" tabindex="-1" role="dialog" aria-labelledby="Modal_Expenses_TaxLabel"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Modal_Expenses_TaxLabel">TAX Calculation
                    </h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvTaxExpenses_tax" runat="server" Width="100%" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Tax Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTaxName" runat="server" Text='<%# Eval("Tax_Type_Name") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax Value (%)">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTaxValueInPer" CssClass="form-control" Style="text-align: center;" runat="server" Text='<%# Eval("Tax_Percentage") %>' AutoPostBack="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>



                                    </asp:GridView>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <button id="btnClosePopup_Expenses_Tax" type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="Expenses_Tax_Web_Control" tabindex="-1" role="dialog" aria-labelledby="Expenses_Tax_Web_Control_Label" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Expenses_Tax_Web_Control_Label">Expenses TAX
                    </h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel_Expenses" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <ET:Expenses_Tax ID="Expenses_Tax_Modal" runat="server" />
                                    <asp:HiddenField ID="Hdn_Expenses_Id_Web_Control" runat="server" />
                                    <asp:HiddenField ID="Hdn_Expenses_Name_Web_Control" runat="server" />
                                    <asp:HiddenField ID="Hdn_Expenses_Amount_Web_Control" runat="server" />
                                    <%--<asp:HiddenField ID="Hdn_Session_Name_For_Expenses_Tax" runat="server" />
								<asp:HiddenField ID="Hdn_Save_Session_Name_For_Expenses_Tax" runat="server" />--%>
                                    <asp:HiddenField ID="Hdn_Page_Name_Web_Control" runat="server" />
                                    <asp:HiddenField ID="Hdn_Tax_Entry_Type" runat="server" />
                                    <asp:HiddenField ID="Hdn_Saved_Expenses_Tax_Session" runat="server" />
                                    <asp:HiddenField ID="Hdn_Local_Expenses_Tax_Session" runat="server" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
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


    <asp:UpdateProgress ID="UpdateProgress14" runat="server" AssociatedUpdatePanelID="UpdatePanel_Expenses">
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

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Pending_Order">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="up_UcAcMaster" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_ucAcMaster" Style="display: none;" runat="server" data-toggle="modal" data-target="#ModelAcMaster" Text="Add Accounts" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="modal fade" id="ModelAcMaster" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-body">
                    <uc1:AccountMaster runat="server" ID="UcAcMaster" />
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
    <asp:Panel ID="PnlNewEdit" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlMenuNew" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlMenuList" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlMenuOrder" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PnlList" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PnlBin" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlPendingOrder" runat="server" Visible="false"></asp:Panel>

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

        function LI_List_Active() {
            $("#Li_List").addClass("active");
            $("#List").addClass("active");

            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");
        }

        function Li_Tab_Pending_Order() {
            document.getElementById('<%= Btn_Pending_Order.ClientID %>').click();
        }

        function Hide_LI() {
            var List_LI = document.getElementById("Li_List");
            List_LI.style.display = List_LI.style.display = 'none';

            var Pending_Order_LI = document.getElementById("Li_Pending_Order");
            Pending_Order_LI.style.display = Pending_Order_LI.style.display = 'none';
        }

        function setScrollAndRow() {
            try {
                debugger;
                var rowIndex = $('#<%= hdfCurrentRow.ClientID %>').val();
                var parent = document.getElementById('<%= gvProduct.ClientID %>');
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
        function Show_LI() {
            var List_LI = document.getElementById("Li_List");
            List_LI.style.display = List_LI.style.display = '';

            var Pending_Order_LI = document.getElementById("Li_Pending_Order");
            Pending_Order_LI.style.display = Pending_Order_LI.style.display = '';
        }
    </script>
    <script type="text/javascript">
        function uploadComplete(sender, args) {
            document.getElementById('<%= Img_Wrong.ClientID %>').style.display = "none";
             document.getElementById('<%= Img_Right.ClientID %>').style.display = "";
 }
 function uploadError(sender, args) {
     document.getElementById('<%= Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= Img_Wrong.ClientID %>').style.display = "";
            alert('Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file');
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
        function validate1() {
            var k = confirm("Are you sure to post the record ?");
            if (k == true) {
                var GridView1 = document.getElementById('<%= gvProduct.ClientID %>');
                for (var i = 0; i < GridView1.rows.length - 1; i++) {
                    var $getRecQty = $("span[id*=lblgoodsreceive]")
                    var getInvQty = $("input[id*=QtyGoodReceived]")
                    if (parseFloat($getRecQty[i].innerHTML) > 0) {
                        if (parseFloat($getRecQty[i].innerHTML) != parseFloat(getInvQty[i].value)) {
                            var r = confirm("Invoice qty and receive qty is not same do u want  to continue ? ");
                            return r;
                        }
                    }
                }
                return true;
            }
            else {
                return false;
            }
        }

        function Currencyvalidation() {
            var getInvQty = $("input[id*=TxtCurrencyValue]")
            var GridView1 = document.getElementById('<%= gvProduct.ClientID %>');
            for (var i = 0; i < GridView1.rows.length - 1; i++) {
                var $getSupplierCurrency = $("span[id*=lblSupplierCurrency]")
                //var $getinvoiceCurrency = $("span[id*=lblInvoiceCurrency]")
                if ($getSupplierCurrency[i].innerHTML != getInvQty[i].value) {
                    var r = confirm("Supplier Currency and Invoice currency is not same do u want  to continue ? ");
                    return r;
                }
            }
        }




        function Show_Modal_GST() {
            document.getElementById('<%= Btn_GST_Modal.ClientID %>').click();
        }

        function Show_Modal_Expenses_Tax() {
            document.getElementById('<%= Btn_Modal_Expenses_Tax.ClientID %>').click();
        }



        function Hide_Model_GST() {
            $('#btnClosePopup').click();
        }
        function Show_Expenses_Tax_Web_Control() {
            document.getElementById('<%= Btn_Show_Expenses_Tax.ClientID %>').click();
        }
        function Modal_AcMaster_Open() {
            document.getElementById('<%= Btn_ucAcMaster.ClientID %>').click();
        }

        function lblgvSupplierName_Command(contactId) {
            window.open('../Purchase/CustomerHistory.aspx?ContactId=' + contactId + ' &&Page=PINV', 'window', 'width=1024, ');
        }

        function btnAddSupplier_OnClick() {
            window.open('../Sales/AddContact.aspx?Page=PINV', 'window', 'width=1024');
        }

        function lnkSupplierHistory_OnClick() {
            var strSupplierId = "";
            var txtSupplierName = document.getElementById('<%= txtSupplierName.ClientID%>');
            if (txtSupplierName.value != "") {
                try {
                    strSupplierId = txtSupplierName.value.split('/')[1];
                }
                catch (err) {
                    strSupplierId = "0";
                }
            }
            else {
                strSupplierId = "0";
            }
            window.open('../Purchase/CustomerHistory.aspx?ContactId=' + strSupplierId + '&&Page=PINV', 'window', 'width=1024, ');
        }
        function txtPayAccountNo_TextChanged(ctrl) {
            debugger;
            try {
                var name = ctrl.value.split('/')[0];
                var accId = ctrl.value.split('/')[1];
                if (accId == undefined) {
                    throw "not found";
                }
                var txtboxId = ctrl.id;

                PageMethods.txtPayAccountNo_TextChanged(name, accId, txtboxId, function (data) {
                    debugger;
                    if (data[0] == "false") {
                        alert(data[1]);
                        ctrl.value = "";
                        ctrl.focus();
                        return;
                    }
                });

            }
            catch (err) {
                alert("Account not found");
                ctrl.value = "";
                ctrl.focus();
            }
        }
        function ddlPayCurrency_SelectedIndexChanged(ctrl) {
            var txtPayExchangeRate_ = document.getElementById('<%=txtPayExchangeRate.ClientID%>');
            var txtFCPayCharges_ = document.getElementById('<%=txtFCPayCharges.ClientID%>');
            var txtLCPayCharges_ = document.getElementById('<%=txtLCPayCharges.ClientID%>');
            var txtlocalGrandtotal_ = document.getElementById('<%=txtlocalGrandtotal.ClientID%>');

            PageMethods.ddlPayCurrency_SelectedIndexChanged(ctrl.value, function (data) {
                txtPayExchangeRate_.value = data;
                if (txtPayExchangeRate_.value != "" && txtFCPayCharges_.value != "") {
                    txtLCPayCharges_.value = txtlocalGrandtotal_.value;
                }
            });
        }
        function Modal_Open_FileUpload() {
            document.getElementById('<%= Btn_Modal_FileUpload.ClientID %>').click();
        }

        function showUcControlsSettings() {
            $('#ControlSettingModal').modal('show');
        }
        

         function getReportData(transId) {
            $("#prgBar").css("display", "block");
            document.getElementById('<%= reportSystem.FindControl("hdnTransId").ClientID %>').value = transId;
             document.getElementById('<%= reportSystem.FindControl("hdnPageRef").ClientID %>').value = "PG";
            debugger;
            setReportData();
         }

        
function getReportDataWithLoc(transId, locId) {
    $("#prgBar").css("display", "block");
    document.getElementById('<%= reportSystem.FindControl("hdnTransId").ClientID %>').value = transId;
    document.getElementById('<%= reportSystem.FindControl("hdnLocId").ClientID %>').value = locId;
    document.getElementById('<%= reportSystem.FindControl("hdnPageRef").ClientID %>').value = "PG";
            setReportData();
}

    </script>
    <script src="../Script/ReportSystem.js"></script>
</asp:Content>
