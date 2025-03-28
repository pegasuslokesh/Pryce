<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="PurchaseOrder.aspx.cs" Inherits="Purchase_PurchaseOrder" %>

<%@ Register Src="~/WebUserControl/Expenses_Tax.ascx" TagName="Expenses_Tax" TagPrefix="ET" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/FileUpload.ascx" TagPrefix="AT1" TagName="FileUpload1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/ucControlsSetting.ascx" TagName="ucCtlSetting" TagPrefix="UC" %>
<%@ Register Src="~/WebUserControl/ReportSystem.ascx" TagName="ReportSystem" TagPrefix="RS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <script type="text/javascript">
        function List_Hide_Tab_Content() {
            //Don't remove this function
        }
        function LI_Edit_Active() {
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-file-invoice"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Purchase Order%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Purchase%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Purchase Order%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:Button ID="Btn_Quotation" Style="display: none;" runat="server" OnClick="BtnQuotation_Click" Text="Quotation" />
            <asp:Button ID="Btn_Purchase_Order_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Purchase_Order_Modal" Text="View Modal" />
            <asp:Button ID="Btn_GST_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Modal_GST" Text="GST" />
            <asp:Button ID="Btn_Show_Expenses_Tax" Style="display: none;" runat="server" data-toggle="modal" data-target="#Expenses_Tax_Web_Control" Text="Expenses Tax" />
            <asp:Button ID="Btn_Modal_FileUpload" Style="display: none;" runat="server" data-toggle="modal" data-target="#Fileupload123" Text="FileUpload" />
            <asp:HiddenField runat="server" ID="hdnLocationID" />
            <asp:HiddenField runat="server" ID="hdnCanView" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
            <asp:HiddenField runat="server" ID="hdnCanPrint" />
            <asp:HiddenField runat="server" ID="hdnCanUpload" />
            <asp:HiddenField ID="hfScrollPosition" runat="server" Value="0" />
            <asp:HiddenField ID="hdfCurrentRow" runat="server" />
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
                    <li id="Li_Quotation"><a href="#Quotation" onclick="Li_Tab_Quotation()" data-toggle="tab">
                        <i class="fas fa-user-check"></i>&nbsp;&nbsp;<asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Quotation %>"></asp:Label></a></li>
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
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div1" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label56" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				<asp:Label ID="lblTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                                                <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>

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
                                                    <asp:DropDownList ID="ddlUser" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlUser_Click">
                                                    </asp:DropDownList>
                                                    <br />
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlPosted" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPosted_SelectedIndexChanged">
                                                        <asp:ListItem Selected="True" Text="Invoice Pending" Value="Pending"></asp:ListItem>
                                                        <asp:ListItem Text="Invoice Created" Value="Created"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,All %>" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Purchase Order No %>"
                                                            Value="PONo"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Purchase Order Date %>" Value="PODate"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Delivery Date %>" Value="DeliveryDate"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Order Type %>" Value="Order_Type"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Supplier %>" Value="Supplier_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Status %>" Value="Field4"></asp:ListItem>
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
                                                        <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
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
                                <div class="box box-warning box-solid">
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                    <asp:HiddenField ID="HdnEdit" runat="server" />
                                                    <asp:HiddenField ID="Hdn_Edit_ID" runat="server" />
                                                    <asp:HiddenField ID="hdngvPurchaseOrderCurrentPageIndex" runat="server" Value="1" />

                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvPurchaseOrder" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True" CurrentSortField="PODate" CurrentSortDirection="DESC"
                                                        OnPageIndexChanging="gvPurchaseOrder_PageIndexChanging" OnSorting="gvPurchaseOrder_OnSorting">
                                                        <Columns>

                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">

                                                                            <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <a onclick="getReportDataWithLoc('<%# Eval("TransId") %>','<%# Eval("Location_Id") %>')"><i class="fa fa-print" style="cursor: pointer"></i>Report System</a>
                                                                            </li>
                                                                            <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <a onclick="IbtnQC_Command('<%# Eval("ReferenceId") %>','<%# Eval("ReferenceVoucherType") %>')" style="cursor: pointer"><i class="fa fa-print"></i>QC</a>
                                                                            </li>
                                                                            <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <a onclick="IbtnPrint_Command('<%# Eval("TransId") %>')" title="Print" style="cursor: pointer"><i class="fa fa-print"></i>Print </a>
                                                                            </li>

                                                                            <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("TransId") %>' CommandName='<%# Eval("Location_Id") %>' OnCommand="lnkViewDetail_Command" CausesValidation="False"><i class="fa fa-eye"></i>View</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("TransId") %>' CommandName='<%# Eval("Location_Id") %>' CausesValidation="False" OnCommand="btnEdit_Command"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("TransId") %>' OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>

                                                                            <li <%= hdnCanUpload.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnFileUpload" runat="server" CommandArgument='<%# Eval("TransId") %>' CommandName='<%# Eval("PONo") %>' OnCommand="btnFileUpload_Command" CausesValidation="False"><i class="fa fa-upload"></i>File Upload</asp:LinkButton>
                                                                            </li>
                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Purchase Order No %>" SortExpression="PONo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbPONo" runat="server" Text='<%# Eval("PONo") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Purchase Order Date %>" SortExpression="PODate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPODate" runat="server" Text='<%# Convert.ToDateTime(Eval("PODate").ToString()).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delivery Date %>" SortExpression="DeliveryDate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDeliveryDate" runat="server" Text='<%# Convert.ToDateTime(Eval("DeliveryDate").ToString()).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Supplier %>" SortExpression="Supplier_Name">
                                                                <ItemTemplate>
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td width="80%">
                                                                                <asp:Label ID="lbSupId" runat="server" Text='<%# Eval("Supplier_Name").ToString() %>'></asp:Label>
                                                                            </td>
                                                                            <td align="right" width="20%">
                                                                                <a title="Supplier History" onclick="lblgvSupplierName_Command('<%# Eval("SupplierId") %>')" style="color: blue; cursor: pointer">More..</a>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order Type %>" SortExpression="Order_Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOrderType" runat="server" Text='<%# Eval("Order_Type")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>" SortExpression="Field4">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOrderStatus" runat="server" Text='<%# Eval("Field4").ToString() %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="CreatedUser">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvUser" runat="server" Text='<%#Eval("CreatedUser") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>" SortExpression="ModifiedUser">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModified_User" runat="server" Text='<%# Eval("ModifiedUser") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Amount %>" SortExpression="PoAmount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPoAmount" runat="server" Text='<%# Eval("currency_symbol").ToString() +" "+ SystemParameter.GetAmountWithDecimal(Eval("PoAmount").ToString(),Eval("decimalCount").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Adv Paid" SortExpression="Spv_amount">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkSpv" runat="server" Text='<%# Eval("spv_amount")==null ? "0":SystemParameter.GetAmountWithDecimal(Eval("spv_amount").ToString(),Eval("decimalCount").ToString()) %>'
                                                                        OnClick="lnkSpv_Click" Font-Bold="true" CommandArgument='<%#Eval("voucher_id") + "," + Eval("Location_Id")%>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
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
                                                                        OnClick="lnkPage_Click" OnClientClick='<%# !Convert.ToBoolean(Eval("Enabled")) ? "return false;" : "" %>'></asp:LinkButton>
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

                            </Triggers>
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
                    <div class="modal fade" id="creditDetails" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
                        aria-hidden="true">
                        <div class="modal-dialog modal-lg">
                            <div class="modal-content">
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:UpdatePanel runat="server" ID="upCreditInfo" UpdateMode="Conditional">
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="gvPurchaseOrder" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlCurrency" />
                                                </Triggers>
                                                <ContentTemplate>
                                                    <h3 class="box-title">
                                                        <asp:Label ID="Label46" Font-Names="Times New roman" Font-Size="18px" Font-Bold="true" runat="server" Text="Credit Terms & Condition"></asp:Label></h3>
                                                    <br />
                                                    <div class="box-body">
                                                        <div class="form-group">
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblCreditLimit" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Credit Limit %>" />
                                                                &nbsp:&nbsp<asp:Label ID="lblCreditLimitValue" runat="server"></asp:Label>
                                                                <asp:Label ID="lblCurrencyCreditLimit" runat="server"></asp:Label>
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblCreditDays" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Credit Days %>" />
                                                                &nbsp:&nbsp<asp:Label ID="lblCreditDaysValue" runat="server"></asp:Label>
                                                                <br />
                                                            </div>
                                                            <div class="col-md-12">
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="Label47" runat="server" Font-Bold="true" Text="Credit Parameter" />
                                                                &nbsp:&nbsp<asp:Label ID="lblCreditParameterValue" runat="server"></asp:Label>
                                                                <br />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                                        Close</button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="tab-pane" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvPurchaseOrder" />
                                <asp:AsyncPostBackTrigger ControlID="GvPurchaseQuote" />
                                <asp:AsyncPostBackTrigger ControlID="txtSupplierName" />
                                <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="gvAddQuotationGrid" />
                                <asp:AsyncPostBackTrigger ControlID="gvAddSelesOrder" />
                                <asp:AsyncPostBackTrigger ControlID="gvProduct" />
                                <asp:AsyncPostBackTrigger ControlID="GvQuotationProductEdit" />
                                <asp:AsyncPostBackTrigger ControlID="GvSalesOrderDetail" />
                                <asp:AsyncPostBackTrigger ControlID="gvQuatationProduct" />
                                <asp:PostBackTrigger ControlID="btnDownloadProduct" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <asp:Panel ID="pnlOrderInfo" runat="server">
                                                        <div class="col-md-12" align="right">
                                                            <asp:LinkButton ID="btnControlsSetting" ToolTip="Product List Setting" runat="server" OnClick="btnControlsSetting_Click" Visible="false"><i class="fas fa-wrench" style="font-size:20px"></i></asp:LinkButton>
                                                        </div>
                                                        <div class="col-md-12">
                                                            <asp:RadioButton ID="rbtEdit" runat="server" GroupName="SaveOrEdit" Text="Edit" Visible="false" Checked="true" />
                                                            <asp:RadioButton ID="rbtNew" runat="server" GroupName="SaveOrEdit" Text="New" Visible="false" />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblRequestdate" runat="server" Text="<%$ Resources:Attendance,Purchase Order Date %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtPOdate" ErrorMessage="<%$ Resources:Attendance,Enter Purchase Order Date%>"></asp:RequiredFieldValidator>
                                                            <asp:TextBox ID="txtPOdate" runat="server" CssClass="form-control" />
                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtCalenderExtender" runat="server" TargetControlID="txtPOdate">
                                                            </cc1:CalendarExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Purchase Order No %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtPoNo" ErrorMessage="<%$ Resources:Attendance,Enter Purchase Order No%>"></asp:RequiredFieldValidator>
                                                            <asp:TextBox ID="txtPoNo" runat="server" CssClass="form-control" AutoPostBack="True"
                                                                OnTextChanged="txtPoNo_TextChanged" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Payment Mode %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save" Display="Dynamic"
                                                                SetFocusOnError="true" ControlToValidate="ddlPaymentMode" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Payment Mode %>" />
                                                            <asp:DropDownList ID="ddlPaymentMode" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Cash %>" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Credit %>" Value="2"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Card %>" Value="3"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label48" runat="server" Text="<%$ Resources:Attendance,Project Name %>"></asp:Label>
                                                            <dx:ASPxComboBox ID="ddlprojectname" runat="server" CssClass="form-control" DropDownWidth="550"
                                                                DropDownStyle="DropDownList" ValueField="Project_Id"
                                                                ValueType="System.String" TextFormatString="{0}" EnableCallbackMode="true" IncrementalFilteringMode="Contains"
                                                                CallbackPageSize="30">
                                                                <Columns>
                                                                    <dx:ListBoxColumn FieldName="Project_Name" />
                                                                </Columns>
                                                            </dx:ASPxComboBox>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12">
                                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Supplier Name %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator8" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSupplierName" ErrorMessage="<%$ Resources:Attendance,Enter Supplier Name%>"></asp:RequiredFieldValidator>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtSupplierName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                    AutoPostBack="True" OnTextChanged="txtSupplierName_TextChanged"></asp:TextBox>
                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionInterval="100"
                                                                    DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionList_Supplier"
                                                                    ServicePath="" TargetControlID="txtSupplierName" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                </cc1:AutoCompleteExtender>
                                                                <div class="input-group-btn">
                                                                    <asp:Button ID="btnAddSupplier" runat="server" CssClass="btn btn-primary" OnClick="btnAddSupplier_OnClick"
                                                                        Text="<%$ Resources:Attendance,Add %>" CausesValidation="False" />
                                                                </div>
                                                                <div class="input-group-btn">
                                                                    <br />
                                                                    <asp:LinkButton ID="btnCompareQuatation" runat="server" ToolTip="<%$ Resources:Attendance,Quotation Comparison %>" OnClick="btnCompareQuatation_Click"><i class="fas fa-exchange-alt" style="font-size:30px;margin-left:10px"></i></asp:LinkButton>
                                                                    <asp:LinkButton ID="lnkSupplierHistory" ToolTip="Supplier History" runat="server" Text="Supplier History" OnClientClick="lnkSupplierHistory_OnClick();return false;"><i class="fa fa-history" style="font-size:30px;margin-left:10px"></i></asp:LinkButton>
                                                                </div>
                                                            </div>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Currency %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Save" Display="Dynamic"
                                                                SetFocusOnError="true" ControlToValidate="ddlCurrency" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Currency%>" />
                                                            <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="form-control"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Currency Rate %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCurrencyRate" ErrorMessage="<%$ Resources:Attendance,Enter Currency Rate%>"></asp:RequiredFieldValidator>
                                                            <asp:TextBox ID="txtCurrencyRate" runat="server" CssClass="form-control" />
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                                                TargetControlID="txtCurrencyRate" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblOrderType" runat="server" Text="<%$ Resources:Attendance,Order Type %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator6" ValidationGroup="Save" Display="Dynamic"
                                                                SetFocusOnError="true" ControlToValidate="ddlOrderType" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Order Type %>" />
                                                            <asp:DropDownList ID="ddlOrderType" runat="server" CssClass="form-control"
                                                                AutoPostBack="True" OnSelectedIndexChanged="ddlOrderType_SelectedIndexChanged">
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Direct %>" Value="D"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,InDirect %>" Value="R"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Delivery Date %>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator7" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtDeliveryDate" ErrorMessage="<%$ Resources:Attendance,Enter Delivery Date%>"></asp:RequiredFieldValidator>
                                                            <asp:TextBox ID="txtDeliveryDate" runat="server" CssClass="form-control" />
                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" TargetControlID="txtDeliveryDate">
                                                            </cc1:CalendarExtender>
                                                            <br />
                                                        </div>
                                                        <div id="PnlReference" runat="server" visible="false">
                                                            <div class="col-md-6">
                                                                <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Reference Type %>"></asp:Label>
                                                                <asp:DropDownList ID="ddlReferenceVoucherType" runat="server" CssClass="form-control"
                                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlReferenceVoucherType_SelectedIndexChanged">
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Purchase Quotation %>" Value="PQ"></asp:ListItem>
                                                                    <%--<asp:ListItem Text="<%$ Resources:Attendance,Sales Order %>" Value="SO"></asp:ListItem>--%>
                                                                </asp:DropDownList>
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblReferenceNo" runat="server" Text="<%$ Resources:Attendance,Reference No %>"></asp:Label>
                                                                <asp:TextBox ID="txtReferenceNo" runat="server" BackColor="#eeeeee" CssClass="form-control"
                                                                    AutoPostBack="true" OnTextChanged="txtReferenceNo_TextChanged"></asp:TextBox>
                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" CompletionInterval="100"
                                                                    DelimiterCharacters="" Enabled="True" MinimumPrefixLength="0" ServiceMethod="GetCompletionList_PurchaseQuotation"
                                                                    ServicePath="" TargetControlID="txtReferenceNo" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                </cc1:AutoCompleteExtender>
                                                                <br />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label39" runat="server" Text="<%$ Resources:Attendance,Ship To %>" />
                                                            <asp:TextBox ID="txtShipSupplierName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                onchange="txtShipSupplierName_TextChanged(this)" />
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender9" runat="server" DelimiterCharacters=""
                                                                Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                                ServiceMethod="GetCompletionListContact" ServicePath="" TargetControlID="txtShipSupplierName"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblShipingAddress" runat="server" Text="<%$ Resources:Attendance,Shipping Address %>" />
                                                            <asp:TextBox ID="txtShipingAddress" runat="server" CssClass="form-control" BackColor="#eeeeee" onchange="validateAddressName(this)" />
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender10" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetCompletionListAddressName" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtShipingAddress"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12"></div>
                                                        <div id="trVQ" runat="server" visible="false" class="col-md-6">
                                                            <asp:Label ID="lblVendorQNo" runat="server" Text="<%$ Resources:Attendance,Vendor Quotation No. %>"></asp:Label>
                                                            <asp:TextBox ID="txtVendorQNo" runat="server" CssClass="form-control" ReadOnly="true" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <br />
                                                            <asp:Button ID="btnShowCreditDetails" runat="server" Text="Show Credit Info" OnClientClick="btnShowCreditInfo()" CssClass="btn btn-primary" />
                                                        </div>
                                                        <div class="col-md-12">
                                                            <asp:RadioButton ID="rbtnFormView" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Form View%>"
                                                                AutoPostBack="true" GroupName="Product" OnCheckedChanged="rbtnFormView_OnCheckedChanged" />&nbsp;&nbsp;&nbsp;
                                                            <asp:RadioButton ID="rbtnAdvancesearchView" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Advance Search View%>"
                                                                AutoPostBack="true" GroupName="Product" OnCheckedChanged="rbtnFormView_OnCheckedChanged" />&nbsp;&nbsp;&nbsp;
                                                            <asp:RadioButton ID="rbtnSalesOrder" Font-Bold="true" runat="server" Text="From Sales Order" AutoPostBack="true" GroupName="Product" OnCheckedChanged="rbtnFormView_OnCheckedChanged" />
                                                            <asp:RadioButton ID="rbtnUsingExcel" Font-Bold="true" runat="server" Text="Using Excel" AutoPostBack="true" GroupName="Product" OnCheckedChanged="rbtnFormView_OnCheckedChanged" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12"></div>
                                                        <div class="col-md-12" id="div_ExcelUpload" runat="server" visible="false">
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
                                                                    NavigateUrl="~/CompanyResource/UploadPoItem.xlsx" Text="Download sample excel" Font-Underline="true"></asp:HyperLink>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <br />
                                                                <asp:Button CssClass="btn btn-primary" Visible="false" runat="server" ID="btnDownloadProduct" OnClick="btnDownloadProduct_Click" Text="Download" />
                                                                &nbsp;
                                                               <asp:Button ID="btnGvProductRefresh" CssClass="btn btn-warning" runat="server" Text="Refresh" ToolTip="Reset Product Table" OnClick="btnGvProductRefresh_Click" />

                                                            </div>
                                                        </div>
                                                        <div class="col-md-6" id="Trans_Div" runat="server">
                                                            <asp:Label ID="lblTransType" runat="server" Text="Transaction Type"></asp:Label>
                                                            <asp:DropDownList ID="ddlTransType" runat="server" onchange="ddlTransType_SelectedIndexChanged(this)" CssClass="form-control"></asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12" style="text-align: center">
                                                            <asp:Button ID="btnAddProduct" runat="server" Text="<%$ Resources:Attendance,Add Product %>" CssClass="btn btn-info" OnClick="btnAddProduct_Click" />
                                                            <asp:Button ID="btnAddProductScreen" Visible="false" runat="server" Text="<%$ Resources:Attendance,Add Product List %>" CssClass="btn btn-info" OnClick="btnAddProductScreen_Click" />
                                                            <asp:Button ID="btnAddtoList" runat="server" Text="<%$ Resources:Attendance,Fill Your Product %>" CssClass="btn btn-info" Visible="false" OnClick="btnAddtoList_Click" />
                                                            <br />
                                                        </div>
                                                        <div runat="server" id="div_salesOrder" visible="false" class="col-md-12" style="min-height: 50px; max-height: 300px; overflow: auto">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" runat="server" ID="gvPendingSalesOrder" AutoGenerateColumns="false" Width="100%">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox runat="server" ID="chkAddSOToPO" OnCheckedChanged="chkAddSOToPO_CheckedChanged" AutoPostBack="true" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="SO No">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="lblOrderNo" Text='<%# Eval("SalesOrderNo") %>'></asp:Label>
                                                                            <asp:HiddenField runat="server" ID="gvOrderId" Value='<%# Eval("trans_id") %>'></asp:HiddenField>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="SO Date">
                                                                        <ItemTemplate>
                                                                            <%#  Eval("salesorderdate").ToString()==""?"":Convert.ToDateTime( Eval("salesorderdate").ToString()).ToString("dd-MMM-yyyy") %>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="80px" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Customer">
                                                                        <ItemTemplate>
                                                                            <%# Eval("Name") %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Product ID">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="lblProductCode" Text='<%# Eval("ProductCode") %>'></asp:Label>
                                                                            <asp:HiddenField runat="server" ID="gvhdnProductId" Value='<%# Eval("Product_Id") %>'></asp:HiddenField>
                                                                            <asp:HiddenField runat="server" ID="gvHdnUnitCost" Value='<%# Eval("SalesPrice1") %>'></asp:HiddenField>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Product Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="lblProductName" Text='<%# Eval("EproductName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Order Qty">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="lblQuantity" Text='<%# Eval("Quantity") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="System Qty">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="lblSysQuantity" Text='<%# Eval("sysqty") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Unit Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="lblUnit" Text='<%# Eval("Unit_name") %>'></asp:Label>
                                                                            <asp:HiddenField runat="server" ID="gvhdnUnitId" Value='<%# Eval("Unit_Id") %>'></asp:HiddenField>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                        <div class="col-md-12">
                                                            <br />
                                                        </div>
                                                        <div class="row" id="pnlProduct1" runat="server" visible="false">
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
                                                                            <div id="pnlAddProductDetail" runat="server">
                                                                                <div style="display: none">
                                                                                    <asp:Button runat="server" ID="btnFillUnit" OnClick="btnFillUnit_Click" />
                                                                                </div>
                                                                                <asp:HiddenField runat="server" ID="hdnProductID" />
                                                                                <asp:HiddenField runat="server" ID="hdnProductDesc" />

                                                                                <div class="col-md-12">
                                                                                    <asp:Label ID="Label38" runat="server" Text="<%$ Resources:Attendance,Product Id%>" />
                                                                                    <asp:TextBox ID="txtProductcode" runat="server" CssClass="form-control" OnTextChanged="txtProductCode_TextChanged" AutoPostBack="true" BackColor="#eeeeee" />
                                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" CompletionInterval="100"
                                                                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductCode"
                                                                                        ServicePath="" TargetControlID="txtProductcode" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                    </cc1:AutoCompleteExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <asp:Label ID="lblProductName" runat="server" Text="<%$ Resources:Attendance,Product Name %>" />
                                                                                    <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" BackColor="#eeeeee" AutoPostBack="True" OnTextChanged="txtProductName_TextChanged" />
                                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" CompletionInterval="100"
                                                                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductName"
                                                                                        ServicePath="" TargetControlID="txtProductName" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                    </cc1:AutoCompleteExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblUnit" runat="server" Text="<%$ Resources:Attendance,Unit %>" />
                                                                                    <asp:DropDownList ID="ddlUnit" runat="server" CssClass="form-control" />
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label24" runat="server" Text="<%$ Resources:Attendance,Order Quantity %>" />
                                                                                    <asp:TextBox ID="txtOrderQty" runat="server" CssClass="form-control" />
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                                        TargetControlID="txtOrderQty" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label25" runat="server" Text="<%$ Resources:Attendance,Free Quantity %>" />
                                                                                    <asp:TextBox ID="txtfreeQty" runat="server" CssClass="form-control" />
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                                                        TargetControlID="txtfreeQty" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblUnitCost" runat="server" Text="<%$ Resources:Attendance,Unit Cost %>" />
                                                                                    <asp:TextBox ID="txtUnitCost" runat="server" CssClass="form-control" />
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                                        TargetControlID="txtUnitCost" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <asp:Label ID="lblPDescription" runat="server" Text="<%$ Resources:Attendance,Product Description %>" />
                                                                                    <asp:Panel ID="pnlPDescription" runat="server" Height="100px" CssClass="form-control"
                                                                                        BorderColor="#8ca7c1" BackColor="#ffffff" ScrollBars="Vertical">
                                                                                        <asp:Literal ID="txtPDescription" runat="server"></asp:Literal>
                                                                                    </asp:Panel>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-12" style="text-align: center">
                                                                                    <asp:Button ID="btnProductSave" runat="server" Text="<%$ Resources:Attendance,Add Product %>"
                                                                                        CssClass="btn btn-primary" Visible="false" OnClick="btnProductSave_Click" />
                                                                                    <a onclick="resetDetail()" class="btn btn-primary">Reset
                                                                                    </a>
                                                                                    <asp:Button ID="btnProductCancel" runat="server" Visible="false" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Reset %>"
                                                                                        CausesValidation="False" OnClick="btnProductCancel_Click" />
                                                                                    <asp:Button ID="btnProductClose" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Close %>"
                                                                                        CausesValidation="False" OnClick="btnProductClose_Click" />
                                                                                    <asp:HiddenField ID="hidProduct" runat="server" />
                                                                                    <br />
                                                                                </div>
                                                                            </div>
                                                                            <div id="PnlQuatationGrid" runat="server" visible="false">
                                                                                <div class="col-md-12">
                                                                                    <div style="overflow: auto; max-height: 500px;">
                                                                                        <asp:HiddenField ID="Hdn_Quot_Id" runat="server" />
                                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvAddQuotationGrid" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                                                            runat="server" AutoGenerateColumns="False" Width="100%">
                                                                                            <Columns>
                                                                                                <asp:TemplateField>
                                                                                                    <ItemTemplate>
                                                                                                        <asp:CheckBox ID="chk" runat="server" />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,S No. %>">
                                                                                                    <ItemTemplate>
                                                                                                        <%#Container.DataItemIndex+1 %>
                                                                                                        <asp:Label ID="Lbl_Serial_No_Gv" runat="server" Text='<%#Container.DataItemIndex+1 %>' Visible="true" />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id%>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblproductcode" runat="server" Text='<%# ProductCode(Eval("Product_Id").ToString()) %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Product Name %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvProductId" runat="server" Text='<%#Eval("Product_Id") %>' Visible="false" />
                                                                                                        <asp:Label ID="lblgvProductName" runat="server" Text='<%# ProductName(Eval("Product_Id").ToString()) %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Unit %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvUnitId" runat="server" Visible="false" Text='<%#Eval("UnitId") %>' />
                                                                                                        <asp:Label ID="lblgvUnit" runat="server" Text='<%# UnitName(Eval("UnitId").ToString()) %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Order Quantity %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvRequiredQty" runat="server" Text='<%# SystemParameter.GetAmountWithDecimal(Eval("ReqQty").ToString(),Session["LoginLocDecimalCount"].ToString()) %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Unit Cost %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblUnitCost" runat="server" Text='<%# SystemParameter.GetAmountWithDecimal(Eval("UnitPrice").ToString(),Session["LoginLocDecimalCount"].ToString()) %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Gross Price %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblAmount" runat="server" Text='<%#SystemParameter.GetAmountWithDecimal((Convert.ToDouble(Eval("UnitPrice"))*Convert.ToDouble(Eval("ReqQty"))).ToString(),Session["LoginLocDecimalCount"].ToString()) %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Discount(%) %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblDiscountPercentage" runat="server" Text='<%#SystemParameter.GetAmountWithDecimal(Eval("DisPercentage").ToString(),Session["LoginLocDecimalCount"].ToString()) %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Discount(Value) %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblDiscountvalue" runat="server" Text='<%#SystemParameter.GetAmountWithDecimal(Eval("DiscountValue").ToString(),Session["LoginLocDecimalCount"].ToString())%>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Tax(%) %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lbltaxpercentage" runat="server" Text='<%#SystemParameter.GetAmountWithDecimal(Eval("TaxPercentage").ToString(),Session["LoginLocDecimalCount"].ToString()) %>'></asp:Label>
                                                                                                        <asp:ImageButton ID="BtnAddTax" runat="server" CommandName="gvAddQuotationGrid" CommandArgument='<%# Eval("Product_Id") %>' OnCommand="BtnAddTax_Command" ImageUrl="~/Images/plus.png" ToolTip="View Tax" />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Tax(Value) %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lbltaxvalue" runat="server" Text='<%#SystemParameter.GetAmountWithDecimal(Eval("TaxValue").ToString(),Session["LoginLocDecimalCount"].ToString())%>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Price After Tax %>" Visible="false">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblPriceaftertax" runat="server" Text='<%#SystemParameter.GetAmountWithDecimal(Eval("PriceAfterTax").ToString(),Session["LoginLocDecimalCount"].ToString())%>'
                                                                                                            Visible="false"></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Net Price %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblNetprice" runat="server" Text='<%#SystemParameter.GetAmountWithDecimal(Eval("Amount").ToString(),Session["LoginLocDecimalCount"].ToString()) %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Free Quantity %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="txtFreeQty" runat="server" Width="70px"
                                                                                                            Text="0" AutoPostBack="True" OnTextChanged="txtFreeQty_TextChanged"></asp:TextBox>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                            </Columns>


                                                                                            <PagerStyle CssClass="pagination-ys" />

                                                                                        </asp:GridView>
                                                                                    </div>
                                                                                    <div style="overflow: auto; max-height: 500px;">
                                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvAddSelesOrder" runat="server" AutoGenerateColumns="False" Width="100%">

                                                                                            <Columns>
                                                                                                <asp:TemplateField>
                                                                                                    <ItemTemplate>
                                                                                                        <asp:CheckBox ID="ChkSelect" runat="server" />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="10px" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField>
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvSerialNo" runat="server" Text='<%#Eval("Serial_No") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="20px" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id%>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblproductcode" runat="server" Text='<%# ProductCode(Eval("Product_Id").ToString()) %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:HiddenField ID="hdngvProductId" runat="server" Value='<%#Eval("Product_Id") %>' />
                                                                                                        <asp:Label ID="lblgvProductName" runat="server" Text='<%#ProductName(Eval("Product_Id").ToString()) %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:DropDownList ID="ddlgvUnit" Width="90px"
                                                                                                            runat="server" />
                                                                                                        <asp:HiddenField ID="hdnUnitId" runat="server" Value='<%#Eval("UnitId") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Price %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="txtgvUnitPrice" Width="70px"
                                                                                                            runat="server" Text='<%#SystemParameter.GetAmountWithDecimal(Eval("UnitPrice").ToString(),Session["LoginLocDecimalCount"].ToString()) %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quantity %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="txtgvQuantity" runat="server" Width="70px"
                                                                                                            Text='<%#SystemParameter.GetAmountWithDecimal(Eval("Quantity").ToString(),Session["LoginLocDecimalCount"].ToString()) %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Free %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="txtgvFreeQuantity" runat="server" Width="70px" Text='<%#Eval("FreeQty") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Tax (%)">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="lblTax" Visible="true" Enabled="false" runat="server" Text='<%# SystemParameter.GetAmountWithDecimal(Eval("TaxP").ToString(),Session["LoginLocDecimalCount"].ToString()) %>'
                                                                                                            CssClass="form-control" Width="50px"></asp:TextBox>
                                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderTax" runat="server" Enabled="True"
                                                                                                            TargetControlID="lblTax" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                                        <asp:ImageButton ID="BtnAddTax" runat="server" CommandName="gvAddSelesOrder" CommandArgument='<%# Eval("Product_Id") %>' OnCommand="BtnAddTax_Command" ImageUrl="~/Images/plus.png" Width="30px" Height="30px" ToolTip="Add Tax" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterStyle BorderStyle="None" />
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Tax Value %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="lblTaxValue" runat="server" Text='<%# SystemParameter.GetAmountWithDecimal(Eval("TaxV").ToString(),Session["LoginLocDecimalCount"].ToString()) %>'
                                                                                                            Enabled="false" CssClass="form-control"></asp:TextBox>
                                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderTaxValue" runat="server"
                                                                                                            Enabled="True" TargetControlID="lblTaxValue" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                                    </ItemTemplate>
                                                                                                    <FooterStyle BorderStyle="None" />
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                            </Columns>

                                                                                            <PagerStyle CssClass="pagination-ys" />

                                                                                        </asp:GridView>
                                                                                    </div>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-12" style="text-align: center">
                                                                                    <asp:Button ID="btnAddOrder" runat="server" Text="Add To Order" CssClass="btn btn-primary"
                                                                                        OnClick="btnAddOrder_Click" />
                                                                                    <br />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12">
                                                            <div class="col-md-12" runat="server" id="scrollArea" onscroll="SetDivPosition()" style="overflow: auto; max-height: 500px;">
                                                                <asp:HiddenField ID="Hdn_Tax_By" runat="server" />
                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvProduct" runat="server" AutoGenerateColumns="False" Width="100%">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>" HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="btnPDEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                    ImageUrl="~/Images/edit.png" CausesValidation="False" OnCommand="btnPDEdit_Command"
                                                                                    ToolTip="<%$ Resources:Attendance,Edit %>" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>" HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="IbtnPDDelete" runat="server" CommandName='<%# Eval("Serial_No") %>' CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                    ImageUrl="~/Images/Erase.png" OnCommand="IbtnPDDelete_Command"
                                                                                    ToolTip="<%$ Resources:Attendance,Delete %>" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Serial No %>" HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <%#Container.DataItemIndex+1 %>
                                                                                <asp:Label ID="lblSerialNO" runat="server" Visible="false" Text='<%# Eval("Serial_No") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="Order No" HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <asp:HiddenField ID="hdnSOId" runat="server" Value='<%# Eval("SOId").ToString()==""?"0":Eval("SOId").ToString() %>'></asp:HiddenField>
                                                                                <asp:Label ID="lblSONO" runat="server" Text='<%# Eval("SONO") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id%>" HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <table width="100%">
                                                                                    <tr>
                                                                                        <td colspan="2">
                                                                                            <asp:Label ID="lblproductcode" runat="server" Text='<%# ProductCode(Eval("Product_Id").ToString()) %>'></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblstockinfo" runat="server" Text="Stock Qty :" Font-Bold="false"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:LinkButton ID="lnkStockInfo" runat="server" Text='<%#GetProductStock(Eval("Product_Id").ToString()) %>'
                                                                                                Font-Underline="true" ToolTip="View Detail" OnCommand="lnkStockInfo_Command"
                                                                                                ForeColor="Blue" CommandArgument='<%# Eval("Product_Id") %>'></asp:LinkButton>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="120px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>" ItemStyle-Width="15%"
                                                                            HeaderStyle-VerticalAlign="Top">
                                                                            <HeaderTemplate>
                                                                                <asp:Label ID="lblGrdHdProductName" Text="<%$ Resources:Attendance,Product Name %>" runat="server"></asp:Label>
                                                                                <asp:CheckBox ID="chkShortProductName" Text="" runat="server" ToolTip="Dispay detail Name" OnCheckedChanged="chkShortProductName_CheckedChanged" AutoPostBack="true"></asp:CheckBox>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblProductId" runat="server" Text='<%# ProductName(Eval("Product_Id").ToString()) %>' Visible="false"></asp:Label>
                                                                                <asp:Label ID="lblShortProductName1" runat="server" Text='<%# GetShortProductName(Eval("Product_Id").ToString()) %>'></asp:Label>
                                                                                <asp:Label ID="lblGvProductId" Visible="false" runat="server" Text='<%# Eval("Product_Id") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Name %>" HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblUnit" runat="server" Text='<%# UnitName(Eval("UnitId").ToString()) %>'></asp:Label>
                                                                                <asp:Label ID="lblgvUnitID" runat="server" Text='<%# Eval("UnitId") %>' Visible="false"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order Quantity %>" HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblReqQty" runat="server" Text='<%#SystemParameter.GetAmountWithDecimal(Eval("OrderQty").ToString(),Session["LoginLocDecimalCount"].ToString()) %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Free Quantity %>" HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblFreeQty" runat="server" Text='<%#SystemParameter.GetAmountWithDecimal(Eval("FreeQty").ToString(),Session["LoginLocDecimalCount"].ToString()) %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Cost %>" HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="lblUnitRate" runat="server" Text='<%# SystemParameter.GetAmountWithDecimal(Eval("UnitCost").ToString(),Session["LoginLocDecimalCount"].ToString()) %>' AutoPostBack="true"
                                                                                    OnTextChanged="lblUnitRate_OnTextChanged" CssClass="form-control" Width="100px" onchange="SetSelectedRow(this)"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender39" runat="server" Enabled="True"
                                                                                    TargetControlID="lblUnitRate" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Discount (%) %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblTaxAfterPrice" Visible="false" runat="server" Text="0"></asp:Label>
                                                                                <asp:TextBox ID="lblDiscount" onchange="SetSelectedRow(this)" runat="server" Text='<%# SystemParameter.GetAmountWithDecimal(Eval("DiscountP").ToString(),Session["LoginLocDecimalCount"].ToString()) %>'
                                                                                    CssClass="form-control" AutoPostBack="true" OnTextChanged="lblDiscount_TextChanged"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderDiscount" runat="server"
                                                                                    Enabled="True" TargetControlID="lblDiscount" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                            </ItemTemplate>
                                                                            <FooterStyle BorderStyle="None" />
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Discount Value %>">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="lblDiscountValue" onchange="SetSelectedRow(this)" runat="server" Text='<%# SystemParameter.GetAmountWithDecimal(Eval("DiscountV").ToString(),Session["LoginLocDecimalCount"].ToString()) %>'
                                                                                    CssClass="form-control" AutoPostBack="true" OnTextChanged="lblDiscountValue_TextChanged"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderDiscountValue" runat="server"
                                                                                    Enabled="True" TargetControlID="lblDiscountValue" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                            </ItemTemplate>
                                                                            <FooterStyle BorderStyle="None" />
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Tax (%)">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="lblTax" onchange="SetSelectedRow(this)" Visible="true" Enabled="false" runat="server" Text='<%# SystemParameter.GetAmountWithDecimal(Eval("TaxP").ToString(),Session["LoginLocDecimalCount"].ToString()) %>'
                                                                                    CssClass="form-control" Width="50px"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderTax" runat="server" Enabled="True"
                                                                                    TargetControlID="lblTax" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <asp:ImageButton ID="BtnAddTax" runat="server" CommandName="gvProduct" CommandArgument='<%# Eval("Product_Id") %>' OnCommand="BtnAddTax_Command" ImageUrl="~/Images/plus.png" Width="30px" Height="30px" ToolTip="Add Tax" />
                                                                            </ItemTemplate>
                                                                            <FooterStyle BorderStyle="None" />
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Tax Value %>">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="lblTaxValue" onchange="SetSelectedRow(this)" runat="server" Text='<%# SystemParameter.GetAmountWithDecimal(Eval("TaxV").ToString(),Session["LoginLocDecimalCount"].ToString()) %>'
                                                                                    Enabled="false" CssClass="form-control"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderTaxValue" runat="server"
                                                                                    Enabled="True" TargetControlID="lblTaxValue" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                            </ItemTemplate>
                                                                            <FooterStyle BorderStyle="None" />
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Line Total %>" HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblLineTotal" runat="server" Text='<%# SystemParameter.GetAmountWithDecimal((Convert.ToDouble(Eval("UnitCost"))*Convert.ToDouble(Eval("OrderQty"))).ToString(),Session["LoginLocDecimalCount"].ToString()) %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                    </Columns>


                                                                    <PagerStyle CssClass="pagination-ys" />

                                                                </asp:GridView>
                                                            </div>
                                                            <br />
                                                            <div style="overflow: auto; max-height: 500px;">
                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvQuotationProductEdit" runat="server" AutoGenerateColumns="False"
                                                                    Width="100%" OnRowDataBound="GvQuotationProductEdit_RowDataBound">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>" HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="IbtnPDDelete" runat="server" CommandName='<%# Eval("Serial_No") %>' CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                    ImageUrl="~/Images/Erase.png" OnCommand="IbtnPQDelete_Command"
                                                                                    ToolTip="<%$ Resources:Attendance,Delete %>" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Serial No %>" HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <%#Container.DataItemIndex+1 %>
                                                                                <asp:Label ID="lblSerialNO" runat="server" Visible="false" Text='<%# Eval("Serial_No") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id%>" HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <table width="100%">
                                                                                    <tr>
                                                                                        <td colspan="2">
                                                                                            <asp:Label ID="lblproductcode" runat="server" Text='<%# ProductCode(Eval("Product_Id").ToString()) %>'></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblstockinfo" runat="server" Text="Stock Qty :" Font-Bold="false"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:LinkButton ID="lnkStockInfo" runat="server" Text='<%#GetProductStock(Eval("Product_Id").ToString()) %>'
                                                                                                Font-Underline="true" ToolTip="View Detail" OnCommand="lnkStockInfo_Command"
                                                                                                ForeColor="Blue" CommandArgument='<%# Eval("Product_Id") %>'></asp:LinkButton>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="120px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>" ItemStyle-Width="20%"
                                                                            HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblProductId" runat="server" Text='<%# ProductName(Eval("Product_Id").ToString()) %>'></asp:Label>
                                                                                <asp:Label ID="lblGvProductId" Visible="false" runat="server" Text='<%# Eval("Product_Id") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Name %>" HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblUnit" runat="server" Text='<%# UnitName(Eval("UnitId").ToString()) %>'></asp:Label>
                                                                                <asp:Label ID="lblGvUnitId" Visible="false" runat="server" Text='<%# Eval("UnitId") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order Quantity %>" HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtReqQty" runat="server" Text='<%# SystemParameter.GetAmountWithDecimal(Eval("OrderQty").ToString(),Session["LoginLocDecimalCount"].ToString()) %>'
                                                                                    Enabled="false" Width="50px" AutoPostBack="true" OnTextChanged="txtReqQty_OnTextChanged"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender_txtReqQty" runat="server"
                                                                                    Enabled="True" TargetControlID="txtReqQty" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Free Quantity %>" HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblFreeQty" runat="server" Text='<%#SystemParameter.GetAmountWithDecimal(Eval("FreeQty").ToString(),Session["LoginLocDecimalCount"].ToString()) %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Cost %>" HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtUnitRate" runat="server" Text='<%# SystemParameter.GetAmountWithDecimal(Eval("UnitCost").ToString(),Session["LoginLocDecimalCount"].ToString()) %>'
                                                                                    Width="50px" AutoPostBack="true" OnTextChanged="txtReqQty_OnTextChanged" Enabled="false"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender_txtUnitRate" runat="server"
                                                                                    Enabled="True" TargetControlID="txtUnitRate" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Gross Price %>" HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblAmount" runat="server" Text='<%#SystemParameter.GetAmountWithDecimal((Convert.ToDouble(Eval("UnitCost"))*Convert.ToDouble(Eval("OrderQty"))).ToString(),Session["LoginLocDecimalCount"].ToString()) %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Discount(%) %>" HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDiscountPercentage" runat="server" Text='<%#SystemParameter.GetAmountWithDecimal(Eval("DisPercentage").ToString(),Session["LoginLocDecimalCount"].ToString())%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Discount(Value) %>" HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDiscountvalue" runat="server" Text='<%#SystemParameter.GetAmountWithDecimal(Eval("DiscountValue").ToString(),Session["LoginLocDecimalCount"].ToString())%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Tax(%) %>" HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbltaxpercentage" runat="server" Text='<%#Eval("TaxPercentage")%>'></asp:Label>
                                                                                <asp:ImageButton ID="BtnAddTax" runat="server" CommandName="GvQuotationProductEdit" CommandArgument='<%# Eval("Product_Id") %>' OnCommand="BtnAddTax_Command" ImageUrl="~/Images/plus.png" ToolTip="View Tax" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Tax(Value) %>" HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbltaxvalue" runat="server" Text='<%#SystemParameter.GetAmountWithDecimal(Eval("TaxValue").ToString(),Session["LoginLocDecimalCount"].ToString())%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Price After Tax %>" ItemStyle-Width="10%"
                                                                            HeaderStyle-VerticalAlign="Top" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblPriceaftertax" runat="server" Text='<%#SystemParameter.GetAmountWithDecimal(Eval("PriceAfterTax").ToString(),Session["LoginLocDecimalCount"].ToString())%>'
                                                                                    Visible="false"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Net Price %>" HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblNetprice" runat="server" Text='<%#SystemParameter.GetAmountWithDecimal(Eval("Amount").ToString(),Session["LoginLocDecimalCount"].ToString()) %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                    </Columns>


                                                                    <PagerStyle CssClass="pagination-ys" />

                                                                </asp:GridView>
                                                            </div>
                                                            <br />
                                                            <div style="overflow: auto; max-height: 500px;">
                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvSalesOrderDetail" runat="server" AutoGenerateColumns="False"
                                                                    Width="100%">

                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="ChkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="ChkSelectSo_OnCheckedChanged" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" Width="10px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <%#Container.DataItemIndex+1 %>
                                                                                <asp:Label ID="lblgvSerialNo" Visible="false" runat="server" Text='<%#Eval("Serial_No") %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" Width="20px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id%>">
                                                                            <ItemTemplate>
                                                                                <table width="100%">
                                                                                    <tr>
                                                                                        <td colspan="2">
                                                                                            <asp:Label ID="lblproductcode" runat="server" Text='<%# ProductCode(Eval("Product_Id").ToString()) %>'></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblstockinfo" runat="server" Text="Stock Qty :" Font-Bold="false"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:LinkButton ID="lnkStockInfo" runat="server" Text='<%#GetProductStock(Eval("Product_Id").ToString()) %>'
                                                                                                Font-Underline="true" ToolTip="View Detail" OnCommand="lnkStockInfo_Command"
                                                                                                ForeColor="Blue" CommandArgument='<%# Eval("Product_Id") %>'></asp:LinkButton>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="120px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>">
                                                                            <ItemTemplate>
                                                                                <asp:HiddenField ID="hdngvProductId" runat="server" Value='<%#Eval("Product_Id") %>' />
                                                                                <asp:Label ID="lblgvProductName" runat="server" Text='<%#ProductName(Eval("Product_Id").ToString()) %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit %>">
                                                                            <ItemTemplate>
                                                                                <asp:DropDownList ID="ddlgvUnit" Width="90px" runat="server" />
                                                                                <asp:HiddenField ID="hdnUnitId" runat="server" Value='<%#Eval("UnitId") %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quantity %>">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtgvQuantity" runat="server" Width="70px"
                                                                                    Text='<%#SystemParameter.GetAmountWithDecimal(Eval("Quantity").ToString(),Session["LoginLocDecimalCount"].ToString()) %>' AutoPostBack="true" OnTextChanged="txtgvQuantity_OnTextChanged" />
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendersales" runat="server" Enabled="True"
                                                                                    TargetControlID="txtgvQuantity" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" Width="70px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Free %>">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtgvFreeQuantity" runat="server" Width="70px"
                                                                                    Text='<%#SystemParameter.GetAmountWithDecimal(Eval("FreeQty").ToString(),Session["LoginLocDecimalCount"].ToString()) %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" Width="70px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Price %>">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtgvUnitPrice" Width="70px"
                                                                                    runat="server" Text='<%#SystemParameter.GetAmountWithDecimal(Eval("UnitPrice").ToString(),Session["LoginLocDecimalCount"].ToString()) %>' OnTextChanged="txtgvUnitPriceSo_OnTextChanged"
                                                                                    AutoPostBack="true" />
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendersalesunit" runat="server"
                                                                                    Enabled="True" TargetControlID="txtgvUnitPrice" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" Width="70px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Discount (%) %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblTaxAfterPrice" Visible="false" runat="server" Text="0"></asp:Label>
                                                                                <asp:TextBox ID="Txt_Discount_Sales" runat="server" Text='<%# SystemParameter.GetAmountWithDecimal(Eval("DiscountP").ToString(),Session["LoginLocDecimalCount"].ToString()) %>'
                                                                                    CssClass="form-control" AutoPostBack="true" OnTextChanged="Txt_Discount_Sales_TextChanged"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderDiscount" runat="server"
                                                                                    Enabled="True" TargetControlID="Txt_Discount_Sales" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                            </ItemTemplate>
                                                                            <FooterStyle BorderStyle="None" />
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Discount Value %>">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="Txt_DiscountValue_Sales" runat="server" Text='<%# SystemParameter.GetAmountWithDecimal(Eval("DiscountV").ToString(),Session["LoginLocDecimalCount"].ToString()) %>'
                                                                                    CssClass="form-control" AutoPostBack="true" OnTextChanged="Txt_DiscountValue_Sales_TextChanged"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderDiscountValue" runat="server"
                                                                                    Enabled="True" TargetControlID="Txt_DiscountValue_Sales" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                            </ItemTemplate>
                                                                            <FooterStyle BorderStyle="None" />
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Tax (%)">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="Txt_Tax_Per_Sales" Visible="true" Enabled="false" runat="server" Text='<%# SystemParameter.GetAmountWithDecimal(Eval("TaxP").ToString(),Session["LoginLocDecimalCount"].ToString()) %>'
                                                                                    CssClass="form-control" Width="50px" Height="10px"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderTax" runat="server" Enabled="True"
                                                                                    TargetControlID="Txt_Tax_Per_Sales" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <asp:ImageButton ID="BtnAddTax_Sales" runat="server" CommandName="GvSalesOrderDetail" CommandArgument='<%# Eval("Product_Id") %>' OnCommand="BtnAddTax_Command" ImageUrl="~/Images/plus.png" Width="30px" Height="30px" ToolTip="Add Tax" />
                                                                            </ItemTemplate>
                                                                            <FooterStyle BorderStyle="None" />
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Tax Value %>">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="Txt_Tax_Value_Sales" runat="server" Text='<%# SystemParameter.GetAmountWithDecimal(Eval("TaxV").ToString(),Session["LoginLocDecimalCount"].ToString()) %>'
                                                                                    Enabled="false" CssClass="form-control"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderTaxValue" runat="server"
                                                                                    Enabled="True" TargetControlID="Txt_Tax_Value_Sales" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                            </ItemTemplate>
                                                                            <FooterStyle BorderStyle="None" />
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Line Total %>" HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblLineTotal" runat="server" Text='<%# SystemParameter.GetAmountWithDecimal((Convert.ToDouble(Eval("UnitPrice"))*Convert.ToDouble(Eval("Quantity"))).ToString(),Session["LoginLocDecimalCount"].ToString()) %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                    </Columns>

                                                                    <PagerStyle CssClass="pagination-ys" />

                                                                </asp:GridView>
                                                            </div>
                                                            <br />
                                                            <div style="overflow: auto; max-height: 500px;">
                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvQuatationProduct" runat="server" AutoGenerateColumns="False"
                                                                    Width="100%" OnRowCreated="gvQuatationProduct_RowCreated" OnRowDataBound="GvQuotationProduct_RowDataBound">
                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chk" runat="server" AutoPostBack="true" OnCheckedChanged="chk_OnCheckedChanged" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,S No. %>">
                                                                            <ItemTemplate>
                                                                                <%#Container.DataItemIndex+1 %>
                                                                                <asp:Label ID="lbltrans" runat="server" Text='<%# Eval("Trans_Id").ToString()%>' Visible="true"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id%>">
                                                                            <ItemTemplate>
                                                                                <table width="100%">
                                                                                    <tr>
                                                                                        <td colspan="2">
                                                                                            <asp:Label ID="lblproductcode" runat="server" Text='<%# ProductCode(Eval("Product_Id").ToString()) %>'></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblstockinfo" runat="server" Text="Stock Qty :" Font-Bold="false"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:LinkButton ID="lnkStockInfo" runat="server" Text='<%#GetProductStock(Eval("Product_Id").ToString()) %>'
                                                                                                Font-Underline="true" ToolTip="View Detail" OnCommand="lnkStockInfo_Command"
                                                                                                ForeColor="Blue" CommandArgument='<%# Eval("Product_Id") %>'></asp:LinkButton>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="120px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Product Name %>">
                                                                            <ItemTemplate>
                                                                                <asp:HiddenField ID="hdnSuggestedProductName" runat="server" Value='<%#Eval("SuggestedProductName") %>' />
                                                                                <asp:Label ID="lblgvProductId" runat="server" Text='<%#Eval("Product_Id") %>' Visible="false" />
                                                                                <asp:Label ID="lblgvProductName" runat="server" Text='<%#SuggestedProductName(Eval("Product_Id").ToString(),Eval("SuggestedProductName").ToString()) %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Name %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvUnitId" runat="server" Visible="false" Text='<%#Eval("UnitId") %>' />
                                                                                <asp:Label ID="lblgvUnit" runat="server" Text='<%# UnitName(Eval("UnitId").ToString()) %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Cost %>">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtUnitCost" CssClass="form-control" runat="server" Text='<%#GetAmountDecimal(Eval("UnitPrice").ToString()) %>' Width="50px"
                                                                                    Enabled="false" AutoPostBack="true" OnTextChanged="chk_OnCheckedChanged"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderCost" runat="server"
                                                                                    Enabled="True" TargetControlID="txtUnitCost" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Order %>">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtgvRequiredQty" runat="server" Text='<%#GetAmountDecimal(Eval("ReqQty").ToString()) %>' CssClass="form-control" Width="50px"
                                                                                    Enabled="false" AutoPostBack="true" OnTextChanged="chk_OnCheckedChanged"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender_txtgvRequiredQty" runat="server"
                                                                                    Enabled="True" TargetControlID="txtgvRequiredQty" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Price %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblQtyPrice" runat="server" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, % %>">
                                                                            <ItemTemplate>
                                                                                <%--<asp:Label ID="lblDiscount" runat="server" Text='<%#Eval("DisPercentage")%>' />--%>
                                                                                <asp:TextBox ID="Txt_Discount_Per_Quatation" Width="70px" CssClass="form-control" OnTextChanged="Txt_Discount_Quatation_TextChanged" AutoPostBack="true" runat="server" Text='<%#GetAmountDecimal(Eval("DisPercentage").ToString())%>' />
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderTaxValue" runat="server"
                                                                                    Enabled="True" TargetControlID="Txt_Discount_Per_Quatation" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Value %>">
                                                                            <ItemTemplate>
                                                                                <%--<asp:Label ID="lblDiscountValue" runat="server" Text='<%# Eval("DiscountValue")%>' />--%>
                                                                                <asp:TextBox ID="Txt_DiscountValue_Quatation" Width="70px" CssClass="form-control" OnTextChanged="Txt_DiscountValue_Quatation_TextChanged" AutoPostBack="true" runat="server" Text='<%#GetAmountDecimal(Eval("DiscountValue").ToString())%>' />
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderTaxValue_142" runat="server"
                                                                                    Enabled="True" TargetControlID="Txt_DiscountValue_Quatation" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, % %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblTax" runat="server" Text='<%#GetAmountDecimal(Eval("TaxPercentage").ToString())%>' />
                                                                                <asp:ImageButton ID="BtnAddTax" runat="server" CommandName="gvQuatationProduct" CommandArgument='<%# Eval("Product_Id") %>' OnCommand="BtnAddTax_Command" ImageUrl="~/Images/plus.png" ToolTip="View Tax" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Value %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblTaxValue" runat="server" Text='<%#GetAmountDecimal(Eval("TaxValue").ToString()) %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, After Price %>" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblTaxafterPrice" runat="server" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Net Price %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvAmmount" runat="server" Text='<%#GetAmountDecimal(Eval("Amount").ToString()) %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Free Quantity %>">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtFreeQty" CssClass="form-control" runat="server" Width="70px"
                                                                                    Text="0" AutoPostBack="True" OnTextChanged="txtFreeQty_TextChanged"></asp:TextBox>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                    </Columns>


                                                                    <PagerStyle CssClass="pagination-ys" />

                                                                </asp:GridView>
                                                            </div>
                                                            <br />
                                                        </div>
                                                        <div id="Div_GrossAmount" runat="server" class="col-md-6">
                                                            <asp:Label ID="lblAmount" runat="server" Visible="true" Text="<%$ Resources:Attendance,Gross Amount %>" />
                                                            <asp:TextBox ID="txtGrossAmount" runat="server" Visible="true" ReadOnly="true" CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                        <div id="Div_NetDutyPer" runat="server" class="col-md-6">
                                                            <asp:Label ID="lblNetdutyPer" Visible="false" runat="server" Text="Duty(%)"></asp:Label>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtNetDutyPer" runat="server" CssClass="form-control"
                                                                    Text="0" AutoPostBack="True" Visible="false" OnTextChanged="txtNetDutyPer_OnTextChanged"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender34" runat="server" Enabled="True"
                                                                    TargetControlID="txtNetDutyPer" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                </cc1:FilteredTextBoxExtender>
                                                                <div class="input-group-btn">
                                                                    <asp:Label ID="lblNetDutyVal" Visible="false" runat="server" Text="<%$ Resources:Attendance,Value %>"></asp:Label>
                                                                </div>
                                                                <div class="input-group-btn">
                                                                    <asp:TextBox ID="txtNetDutyVal" Width="120px" Visible="false" runat="server" CssClass="form-control"
                                                                        Text="0" AutoPostBack="True" OnTextChanged="txtNetDutyVal_OnTextChanged"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender35" runat="server" Enabled="True"
                                                                        TargetControlID="txtNetDutyVal" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </div>
                                                            </div>
                                                            <br />
                                                        </div>
                                                        <div id="Div_GrandTotal" runat="server" class="col-md-6">
                                                            <asp:Label ID="lblGrandTotal" Visible="false" runat="server"
                                                                Text="<%$ Resources:Attendance,Net Amount %>" />
                                                            <asp:TextBox ID="txtGrandTotal" Visible="false" runat="server" CssClass="form-control"
                                                                ReadOnly="True" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12" id="ctlRemark" runat="server">
                                                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,Remark %>"></asp:Label>
                                                            <asp:TextBox ID="txRemark" runat="server" CssClass="form-control" TextMode="MultiLine" />
                                                            <br />
                                                        </div>
                                                    </asp:Panel>
                                                    <div class="col-md-12">
                                                        <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Width="100%"
                                                            CssClass="ajax__tab_yuitabview-theme">
                                                            <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="<%$ Resources:Attendance,Shipping %>">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_TabPanel1" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Attendance,Freight Status %>"></asp:Label>
                                                                                    <asp:DropDownList ID="ddlFreightStatus" runat="server" CssClass="form-control">
                                                                                        <asp:ListItem Text="<%$ Resources:Attendance, Paid %>" Value="Y"></asp:ListItem>
                                                                                        <asp:ListItem Text="<%$ Resources:Attendance, UnPaid %>" Value="N"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Attendance,Shipping Line %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtShippingLine" runat="server" BackColor="#eeeeee" CssClass="form-control" onchange="txtShippingLine_TextChanged(this)" />
                                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="100"
                                                                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionList"
                                                                                        ServicePath="" TargetControlID="txtShippingLine" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                    </cc1:AutoCompleteExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <asp:Label ID="lblshipingLineMobileNo" runat="server"></asp:Label>
                                                                                    <br />
                                                                                    <asp:Label ID="lblShipingEmailId" runat="server"></asp:Label>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblSelectExp" runat="server" Text="<%$ Resources:Attendance,Select Expenses %>" />
                                                                                    <asp:DropDownList ID="ddlExpense" runat="server" AutoPostBack="True"
                                                                                        OnSelectedIndexChanged="ddlExpense_SelectedIndexChanged" CssClass="form-control">
                                                                                    </asp:DropDownList>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblFCExpAmount" runat="server" Text="<%$ Resources:Attendance,Paid Amount %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtpaidamount" runat="server" OnTextChanged="txtpaidamount_TextChanged" AutoPostBack="true" CssClass="form-control">0</asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender40" runat="server" Enabled="True"
                                                                                        TargetControlID="txtpaidamount" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblShippingAcc" runat="server" Text="<%$ Resources:Attendance,Shipping Account (Debit) %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtShippingAcc" runat="server" CssClass="form-control" onchange="txtShippingAcc_TextChanged(this)" BackColor="#eeeeee" />
                                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender8" runat="server" DelimiterCharacters=""
                                                                                        Enabled="True" ServiceMethod="GetCompletionListAccountNo" ServicePath="" CompletionInterval="100"
                                                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtShippingAcc"
                                                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                    </cc1:AutoCompleteExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblExpAccount" runat="server" Text="<%$ Resources:Attendance,Expenses Account (Credit) %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtExpensesAccount" runat="server" CssClass="form-control" onchange="txtShippingAcc_TextChanged(this)" BackColor="#eeeeee" />
                                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" DelimiterCharacters=""
                                                                                        Enabled="True" ServiceMethod="GetCompletionListAccountNo" ServicePath="" CompletionInterval="100"
                                                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtExpensesAccount"
                                                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                    </cc1:AutoCompleteExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Attendance,Ship By %>"></asp:Label>
                                                                                    <asp:DropDownList ID="ddlShipBy" runat="server" CssClass="form-control">
                                                                                        <asp:ListItem Text="By Truck" Value="By Track"></asp:ListItem>
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
                                                                                    <asp:Label ID="Label17" runat="server" Text="<%$ Resources:Attendance,Ship Unit %>"></asp:Label>
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
                                                                                    <asp:Label ID="Label20" runat="server" Text="<%$ Resources:Attendance,Unit Rate %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtUnitRate" runat="server" CssClass="form-control" onchange="txtCommon_Click(this);" />
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                                                        TargetControlID="txtUnitRate" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label21" runat="server" Text="<%$ Resources:Attendance,Shipping Date %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtShippingDate" runat="server" CssClass="form-control" />
                                                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender2" runat="server" TargetControlID="txtShippingDate">
                                                                                    </cc1:CalendarExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label26" runat="server" Text="<%$ Resources:Attendance,Receiving Date %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtReceivingDate" runat="server" CssClass="form-control" />
                                                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender3" runat="server" TargetControlID="txtReceivingDate">
                                                                                    </cc1:CalendarExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label27" runat="server" Text="<%$ Resources:Attendance,Shipment Type %>"></asp:Label>
                                                                                    <asp:DropDownList ID="ddlShipmentType" runat="server" CssClass="form-control">
                                                                                        <asp:ListItem Text="<%$ Resources:Attendance, FOB (Freight On Board) %>" Value="FOB(Frieght On Board)"></asp:ListItem>
                                                                                        <asp:ListItem Text="<%$ Resources:Attendance, EX-Work %>" Value="EX-Work"></asp:ListItem>
                                                                                        <asp:ListItem Text="<%$ Resources:Attendance, C&F%>" Value="C&F"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label28" runat="server" Text="<%$ Resources:Attendance, Partial Shipment %>"></asp:Label>
                                                                                    <asp:DropDownList ID="ddlPartialShipment" runat="server" CssClass="form-control">
                                                                                        <asp:ListItem Text="<%$ Resources:Attendance, Yes %>" Value="Y"></asp:ListItem>
                                                                                        <asp:ListItem Text="<%$ Resources:Attendance, No %>" Value="N"></asp:ListItem>
                                                                                    </asp:DropDownList>
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
                                                                                            <asp:Button ID="Btn_Add_Expenses_Tax" runat="server" Text="Add Tax" OnClick="Btn_Add_Expenses_Tax_Click" CssClass="btn btn-info" />
                                                                                        </div>
                                                                                    </div>
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
                                                            <cc1:TabPanel ID="TabProductSupplier" runat="server" HeaderText="<%$ Resources:Attendance,Term & Condition%>">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_TabProductSupplier" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <cc1:Editor ID="txtDesc" runat="server" Width="100%" />
                                                                                    <br />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_TabProductSupplier">
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
                                                            <cc1:TabPanel ID="TabAdvancePayment" runat="server" HeaderText="<%$ Resources:Attendance,Advance Payment %>">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_TabAdvancePayment" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label59" runat="server" Text="Advance Payment(%)"></asp:Label>
                                                                                    <asp:TextBox ID="txtAdvancePer" runat="server" CssClass="form-control"
                                                                                        AutoPostBack="false">0</asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True"
                                                                                        TargetControlID="txtAdvancePer" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <asp:RangeValidator ID="Range1"
                                                                                        ControlToValidate="txtAdvancePer"
                                                                                        MinimumValue="0"
                                                                                        MaximumValue="100"
                                                                                        Type="Double"
                                                                                        EnableClientScript="true"
                                                                                        Text="The value must be from 0 to 100!"
                                                                                        runat="server" />
                                                                                    </asp>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row" style="display: none">
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label29" runat="server" Text="<%$ Resources:Attendance,Payment Mode %>"></asp:Label>
                                                                                    <asp:DropDownList ID="ddlAdvancePayment" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPaymentMode_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblPayAccountNo" runat="server" Text="<%$ Resources:Attendance,Account No. %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtPayAccountNo" runat="server" CssClass="form-control" onchange="txtShippingAcc_TextChanged(this)" BackColor="#eeeeee"></asp:TextBox>
                                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                                                                        Enabled="True" ServiceMethod="GetCompletionListAccountNo" ServicePath="" CompletionInterval="100"
                                                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtPayAccountNo"
                                                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                    </cc1:AutoCompleteExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label30" runat="server" Text="<%$ Resources:Attendance,Currency %>"></asp:Label>
                                                                                    <asp:DropDownList ID="ddlPayCurrency" runat="server" CssClass="form-control"
                                                                                        Enabled="false" AutoPostBack="True" OnSelectedIndexChanged="ddlPayCurrency_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label31" runat="server" Text="<%$ Resources:Attendance,Exchange Rate %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtPayExchangeRate" runat="server" CssClass="form-control"
                                                                                        AutoPostBack="true" OnTextChanged="txtFCPayCharges_TextChanged">0</asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender41" runat="server" Enabled="True"
                                                                                        TargetControlID="txtPayExchangeRate" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblPayAmmount" runat="server" Text="<%$ Resources:Attendance,Balance Amount%>"></asp:Label>
                                                                                    <asp:TextBox ID="txtFCPayCharges" runat="server" CssClass="form-control"
                                                                                        AutoPostBack="true" OnTextChanged="txtFCPayCharges_TextChanged"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                                                                        TargetControlID="txtFCPayCharges" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblExpCharges" runat="server" Text="Payment Amount (Local)" />
                                                                                    <asp:TextBox ID="txtLCPayCharges" runat="server" ReadOnly="true" CssClass="form-control">0</asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender42" runat="server" Enabled="True"
                                                                                        TargetControlID="txtLCPayCharges" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-12" style="text-align: center">
                                                                                    <%--<asp:ImageButton runat="server" CausesValidation="False" ImageUrl="~/Images/add.png"
                                                                                        Height="29px" ToolTip="<%$ Resources:Attendance,Add %>" ID="btnPaymentSave"
                                                                                        OnClick="btnPaymentSave_Click"></asp:ImageButton>--%>
                                                                                    <asp:Button ID="btnPaymentSave" runat="server" CssClass="btn btn-primary" OnClick="btnPaymentSave_Click" Text="<%$ Resources:Attendance,Add %>" />
                                                                                    <asp:HiddenField ID="hdnProductExpenses" runat="server" Value="0" />
                                                                                    <br />
                                                                                </div>
                                                                                <div id="trBank" runat="server" visible="false" class="col-md-6">
                                                                                    <asp:Label ID="lblPayBank" runat="server" Text="<%$ Resources:Attendance,Bank %>"
                                                                                        Visible="false"></asp:Label>
                                                                                    <asp:DropDownList ID="ddlPayBank" runat="server" CssClass="form-control"
                                                                                        Visible="false">
                                                                                    </asp:DropDownList>
                                                                                    <br />
                                                                                </div>
                                                                                <div id="trcheque" runat="server" visible="false">
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
                                                                                <div id="trcard" runat="server" visible="false">
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
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <div style="overflow: auto; max-height: 500px;">
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
                                                                                                        <asp:Label ID="lblgvFCExchangeAmount" runat="server" Text='<%#SystemParameter.GetAmountWithDecimal(Eval("FCPayAmount").ToString(),Session["LoginLocDecimalCount"].ToString()) %>' />
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
                                                                                                        <asp:Label ID="lblgvExpExchangeRate" runat="server" Text='<%#SystemParameter.GetAmountWithDecimal(Eval("PayExchangeRate").ToString(),Session["LoginLocDecimalCount"].ToString()) %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Payment Amount (Local)" SortExpression="Pay_Charges">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvExp_Charges" runat="server" Text='<%#SystemParameter.GetAmountWithDecimal(Eval("Pay_Charges").ToString(),Session["LoginLocDecimalCount"].ToString()) %>' />
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
                                                                    <asp:UpdateProgress ID="UpdateProgress7" runat="server" AssociatedUpdatePanelID="Update_TabAdvancePayment">
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
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Attendance,Save %>" CssClass="btn btn-success"
                                                            OnClick="btnSave_Click" />
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
                    <div class="tab-pane" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="Btn_Bin" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div2" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label57" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				<asp:Label ID="lblbinTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlbinFieldName" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlbinFieldName_SelectedIndexChanged">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance, Purchase Order No %>"
                                                            Value="PONo"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Purchase Order Date %>" Value="PODate"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Delivery Date %>" Value="DeliveryDate"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Order Type %>" Value="Order_Type"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Supplier %>" Value="Supplier_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Status %>" Value="Field4"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="CreatedUser"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Modified By %>" Value="ModifiedUser"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlbinOption" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-4">
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbinbind">
                                                        <asp:TextBox ID="txtbinValue" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                                        <asp:TextBox ID="txtbinValueDate" runat="server" CssClass="form-control" Visible="false" placeholder="Search From Date"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtbinValueDate" runat="server" TargetControlID="txtbinValueDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="btnbinbind" runat="server" CausesValidation="False" OnClick="btnbinbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnbinRefresh" runat="server" CausesValidation="False" OnClick="btnbinRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imgBtnRestore" CausesValidation="False" runat="server" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid" <%= GvBinPurchaseOrder.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvBinPurchaseOrder" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                        OnPageIndexChanging="GvBinPurchaseOrder_PageIndexChanging" OnSorting="GvBinPurchaseOrder_Sorting">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkgvSelect" runat="server" OnCheckedChanged="chkgvSelect_CheckedChanged"
                                                                        AutoPostBack="true" />
                                                                </ItemTemplate>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChanged"
                                                                        AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Purchase Order No %>" SortExpression="PONo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbPONo" runat="server" Text='<%# Eval("PONo") %>'></asp:Label>
                                                                    <asp:Label ID="lblId" runat="server" Text='<%# Eval("TransId") %>' Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Purchase Order Date %>" SortExpression="PODate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPODate" runat="server" Text='<%# GetDateFromat(Eval("PODate").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delivery Date %>" SortExpression="DeliveryDate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDeliveryDate" runat="server" Text='<%# GetDateFromat(Eval("DeliveryDate").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order Type %>" SortExpression="Order_Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOrderType" runat="server" Text='<%# Eval("Order_Type") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Supplier %>" SortExpression="SupplierId">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBinSupId" runat="server" Text='<%# Eval("Supplier_Name").ToString() %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>" SortExpression="Field4">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblbinOrderStatus" runat="server" Text='<%# Eval("Field4").ToString() %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="CreatedUser">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvUser" runat="server" Text='<%#Eval("CreatedUser") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>" SortExpression="ModifiedUser">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModified_User" runat="server" Text='<%# Eval("ModifiedUser") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
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
                    <div class="tab-pane" id="Quotation">
                        <asp:UpdatePanel ID="Update_Quotation" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="Btn_Quotation" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>


                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div3" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label58" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				<asp:Label ID="lblQTotalRecords" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I3" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlQSeleclField" runat="server" CssClass="form-control"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlQSeleclField_SelectedIndexChanged">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Quotation No. %>" Value="RPQ_No" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Quotation Date %>" Value="RPQ_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Supplier Name %>" Value="Supplier_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="CreatedUser"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Modified By %>" Value="ModifiedUser"></asp:ListItem>
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
                                                <div class="col-lg-4">
                                                    <asp:Panel ID="Panel" runat="server" DefaultButton="ImgBtnQBind">
                                                        <asp:TextBox ID="txtQValue" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                                        <asp:TextBox ID="txtQValueDate" Visible="false" runat="server" CssClass="form-control" placeholder="Search From Date"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" Format="dd-MMM-yyyy" ID="CalendarExtendertxtQValue" runat="server" TargetControlID="txtQValueDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="ImgBtnQBind" runat="server" CausesValidation="False" OnClick="ImgBtnQBind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="ImgBtnQRefresh" runat="server" CausesValidation="False" OnClick="ImgBtnQRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid" <%= GvPurchaseQuote.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvPurchaseQuote" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvPurchaseQuote_PageIndexChanging"
                                                        AllowSorting="True" OnSorting="GvPurchaseQuote_Sorting">

                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btnpullBrand" runat="server" BorderStyle="None" BackColor="Transparent" CommandName='<%# Eval("Location_Id") %>'
                                                                        CssClass="btnPull" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="btnPurchaseQuote_Command" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quotation No. %>" SortExpression="RPQ_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvRPQNo" runat="server" Text='<%# Eval("RPQ_No") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quotation Date %>" SortExpression="RPQ_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvRPQDate" runat="server" Text='<%# GetDateFromat(Eval("RPQ_Date").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Purchase Inquiry No. %>"
                                                                SortExpression="Pinq_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvPINo" runat="server" Text='<%# Eval("Pinq_No") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Supplier %>" SortExpression="Supplier_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvSupplier" runat="server" Text='<%#Eval("Supplier_Name") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>" SortExpression="Status">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvStatus" runat="server" Text='<%# Eval("Status") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="CreatedUser">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvUser" runat="server" Text='<%#Eval("CreatedUser") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>" SortExpression="ModifiedUser">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModified_User" runat="server" Text='<%# Eval("ModifiedUser") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
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
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="Purchase_Order_Modal" tabindex="-1" role="dialog" aria-labelledby="Purchase_Order_ModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Purchase_Order_ModalLabel">
                        <asp:Label ID="Label45" runat="server" Font-Size="14px" Font-Bold="true"
                            Text="<%$ Resources:Attendance,Purchase Order %>"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Modal" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label32" runat="server" Text="<%$ Resources:Attendance,Purchase Order Date %>"></asp:Label>
                                                    &nbsp:&nbsp
                                                    <asp:Label ID="lblPOdateView" Font-Bold="true" runat="server"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label33" runat="server" Text="<%$ Resources:Attendance,Purchase Order No %>"></asp:Label>
                                                    &nbsp:&nbsp
                                                    <asp:Label ID="lblPONUmberView" Font-Bold="true" runat="server"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label34" runat="server" Text="<%$ Resources:Attendance,Order Type %>"></asp:Label>
                                                    &nbsp:&nbsp
                                                    <asp:Label ID="lblOrderTypeView" Font-Bold="true" runat="server"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label35" runat="server" Text="<%$ Resources:Attendance,Delivery Date %>"></asp:Label>
                                                    &nbsp:&nbsp
                                                    <asp:Label ID="lblDeliveryDateView" Font-Bold="true" runat="server"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label36" runat="server" Text="<%$ Resources:Attendance,Reference Type %>"></asp:Label>
                                                    &nbsp:&nbsp
                                                    <asp:Label ID="lblReferenceVoucherTypeView" Font-Bold="true" runat="server"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label37" runat="server" Text="<%$ Resources:Attendance,Reference No %>"></asp:Label>
                                                    &nbsp:&nbsp
                                                    <asp:Label ID="lblReferenceNoView" Font-Bold="true" runat="server"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label40" runat="server" Text="<%$ Resources:Attendance,Supplier Name %>"></asp:Label>
                                                    &nbsp:&nbsp
                                                    <asp:Label ID="lblSupplierNameView" Font-Bold="true" runat="server"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <div style="overflow: auto; max-height: 33px">
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GridViewDirectView" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                            runat="server" AutoGenerateColumns="False" Width="100%">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Serial No %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSerialNO" runat="server" Text='<%# Eval("Serial_No") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblproductcode" runat="server" Text='<%# ProductCode(Eval("Product_Id").ToString()) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>" ItemStyle-Width="30%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblProductId" runat="server" Text='<%# ProductName(Eval("Product_Id").ToString()) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Name %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblUnit" runat="server" Text='<%# UnitName(Eval("UnitId").ToString()) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order Quantity %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblReqQty" runat="server" Text='<%#SystemParameter.GetAmountWithDecimal(Eval("OrderQty").ToString(),Session["LoginLocDecimalCount"].ToString()) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Free Quantity %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFreeQty" runat="server" Text='<%#SystemParameter.GetAmountWithDecimal( Eval("FreeQty").ToString(),Session["LoginLocDecimalCount"].ToString()) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Cost %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblUnitRate" runat="server" Text='<%# SystemParameter.GetAmountWithDecimal(Eval("UnitCost").ToString(),Session["LoginLocDecimalCount"].ToString()) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                            </Columns>


                                                            <PagerStyle CssClass="pagination-ys" />

                                                        </asp:GridView>
                                                    </div>
                                                    <br />
                                                    <div style="overflow: auto; max-height: 33px">
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GridViewInDirectView" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                            runat="server" AutoGenerateColumns="False" Width="100%">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chk" runat="server" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,S No. %>">
                                                                    <ItemTemplate>
                                                                        <%#Container.DataItemIndex+1 %>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblproductcode" runat="server" Text='<%# ProductCode(Eval("Product_Id").ToString()) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Product Name %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvProductId" runat="server" Text='<%#Eval("Product_Id") %>' Visible="false" />
                                                                        <asp:Label ID="lblgvProductName" runat="server" Text='<%# ProductName(Eval("Product_Id").ToString()) %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Unit %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvUnitId" runat="server" Visible="false" Text='<%#Eval("UnitId") %>' />
                                                                        <asp:Label ID="lblgvUnit" runat="server" Text='<%# UnitName(Eval("UnitId").ToString()) %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Order Quantity %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvRequiredQty" runat="server" Text='<%#SystemParameter.GetAmountWithDecimal(Eval("ReqQty").ToString(),Session["LoginLocDecimalCount"].ToString()) %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Unit Cost %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblUnitCost" runat="server" Text='<%# SystemParameter.GetAmountWithDecimal(Eval("UnitPrice").ToString(),Session["LoginLocDecimalCount"].ToString()) %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Tax %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTax" runat="server" Text='<%# Eval("TaxPercentage")+"% " +SystemParameter.GetAmountWithDecimal(Eval("TaxValue").ToString(),Session["LoginLocDecimalCount"].ToString())+"(Value)"+ "="+ SystemParameter.GetAmountWithDecimal(Eval("PriceAfterTax").ToString(),Session["LoginLocDecimalCount"].ToString()) %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Discount %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDiscount" runat="server" Text='<%#Eval("DisPercentage")+"% " +SystemParameter.GetAmountWithDecimal(Eval("DiscountValue").ToString(),Session["LoginLocDecimalCount"].ToString())+"(Value)" %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Net Price%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvAmmount" runat="server" Text='<%#SystemParameter.GetAmountWithDecimal(Eval("Amount").ToString(),Session["LoginLocDecimalCount"].ToString()) %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Free Quantity %>">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtFreeQty" runat="server" Width="70px"
                                                                            Text="0" AutoPostBack="True" OnTextChanged="txtFreeQty_TextChanged"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                            </Columns>


                                                            <PagerStyle CssClass="pagination-ys" />

                                                        </asp:GridView>
                                                    </div>
                                                    <br />
                                                    <div style="overflow: auto; max-height: 33px">
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvQuotationProducteditView" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                            runat="server" AutoGenerateColumns="False" Width="100%" OnRowDataBound="gvQuotationProducteditView_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Serial No %>" HeaderStyle-VerticalAlign="Top">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSerialNO" runat="server" Text='<%# Eval("Serial_No") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id%>" HeaderStyle-VerticalAlign="Top">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblproductcode" runat="server" Text='<%# ProductCode(Eval("Product_Id").ToString()) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>" ItemStyle-Width="20%"
                                                                    HeaderStyle-VerticalAlign="Top">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblProductId" runat="server" Text='<%# ProductName(Eval("Product_Id").ToString()) %>'></asp:Label>
                                                                        <asp:Label ID="lblGvProductId" Visible="false" runat="server" Text='<%# Eval("Product_Id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Name %>" HeaderStyle-VerticalAlign="Top">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblUnit" runat="server" Text='<%# UnitName(Eval("UnitId").ToString()) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order Quantity %>" HeaderStyle-VerticalAlign="Top">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtReqQty" runat="server" Text='<%# SystemParameter.GetAmountWithDecimal(Eval("OrderQty").ToString(),Session["LoginLocDecimalCount"].ToString()) %>'
                                                                            Width="50px" AutoPostBack="true" OnTextChanged="txtReqQty_OnTextChanged"></asp:TextBox>
                                                                        <asp:Label ID="lblReqQty" runat="server" Text='<%# Eval("OrderQty") %>'></asp:Label>
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender_txtReqQty" runat="server"
                                                                            Enabled="True" TargetControlID="txtReqQty" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Free Quantity %>" HeaderStyle-VerticalAlign="Top">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFreeQty" runat="server" Text='<%# Eval("FreeQty") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Cost %>" HeaderStyle-VerticalAlign="Top">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtUnitRate" runat="server" Text='<%# SystemParameter.GetAmountWithDecimal(Eval("UnitCost").ToString(),Session["LoginLocDecimalCount"].ToString()) %>'
                                                                            Width="50px" AutoPostBack="true" OnTextChanged="txtReqQty_OnTextChanged"></asp:TextBox>
                                                                        <asp:Label ID="lblUnitRate" runat="server" Text='<%# SystemParameter.GetAmountWithDecimal(Eval("UnitCost").ToString(),Session["LoginLocDecimalCount"].ToString()) %>'></asp:Label>
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender_txtUnitRate" runat="server"
                                                                            Enabled="True" TargetControlID="txtUnitRate" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Amount %>" HeaderStyle-VerticalAlign="Top">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAmount" runat="server" Text='<%#SystemParameter.GetAmountWithDecimal((Convert.ToDouble(Eval("UnitCost"))*Convert.ToDouble(Eval("OrderQty"))).ToString(),Session["LoginLocDecimalCount"].ToString()) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Tax(%) %>" HeaderStyle-VerticalAlign="Top">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbltaxpercentage" runat="server" Text='<%#SystemParameter.GetAmountWithDecimal(Eval("TaxPercentage").ToString(),Session["LoginLocDecimalCount"].ToString())%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Tax(Value) %>" HeaderStyle-VerticalAlign="Top">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbltaxvalue" runat="server" Text='<%# SystemParameter.GetAmountWithDecimal(Eval("TaxValue").ToString(),Session["LoginLocDecimalCount"].ToString())%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Price After Tax %>" ItemStyle-Width="10%"
                                                                    HeaderStyle-VerticalAlign="Top" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPriceaftertax" runat="server" Text='<%# SystemParameter.GetAmountWithDecimal(Eval("PriceAfterTax").ToString(),Session["LoginLocDecimalCount"].ToString())%>'
                                                                            Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Discount(%) %>" HeaderStyle-VerticalAlign="Top">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDiscountPercentage" runat="server" Text='<%#SystemParameter.GetAmountWithDecimal(Eval("DisPercentage").ToString(),Session["LoginLocDecimalCount"].ToString())%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Discount(Value) %>" HeaderStyle-VerticalAlign="Top">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDiscountvalue" runat="server" Text='<%#SystemParameter.GetAmountWithDecimal(Eval("DiscountValue").ToString(),Session["LoginLocDecimalCount"].ToString())%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Net Price %>" HeaderStyle-VerticalAlign="Top">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblNetprice" runat="server" Text='<%#SystemParameter.GetAmountWithDecimal(Eval("Amount").ToString(),Session["LoginLocDecimalCount"].ToString()) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                            </Columns>


                                                            <PagerStyle CssClass="pagination-ys" />

                                                        </asp:GridView>
                                                    </div>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label41" runat="server" Text="<%$ Resources:Attendance,Payment Mode %>"></asp:Label>
                                                    &nbsp:&nbsp
                                                    <asp:Label ID="lblPaymentModeView" Font-Bold="true" runat="server"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label42" runat="server" Text="<%$ Resources:Attendance,Currency %>"></asp:Label>
                                                    &nbsp:&nbsp
                                                    <asp:Label ID="lblCurrencyView" Font-Bold="true" runat="server"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label43" runat="server" Text="<%$ Resources:Attendance,Currency Rate %>"></asp:Label>
                                                    &nbsp:&nbsp
                                                    <asp:Label ID="lblCurrencyRateView" Font-Bold="true" runat="server"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <asp:Label ID="Label44" runat="server" Text="<%$ Resources:Attendance,Remark %>"></asp:Label>
                                                    <asp:Panel ID="lblPnlRemarks" runat="server" BackColor="White" BorderColor="#8ca7c1"
                                                        BorderStyle="Solid" BorderWidth="1px">
                                                        <asp:Literal ID="txtRemarksView" runat="server"></asp:Literal>
                                                    </asp:Panel>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label49" runat="server" Text="<%$ Resources:Attendance,Shipping Line %>"></asp:Label>
                                                    &nbsp:&nbsp
                                                    <asp:Label ID="lblShippingLineView" Font-Bold="true" runat="server"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblMobileNoView" runat="server"></asp:Label>
                                                    &nbsp:&nbsp
                                                    <asp:Label ID="lblEmailIdView" runat="server"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label50" runat="server" Text="<%$ Resources:Attendance,Ship By %>"></asp:Label>
                                                    &nbsp:&nbsp
                                                    <asp:Label ID="lblShipByView" Font-Bold="true" runat="server"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label51" runat="server" Text="<%$ Resources:Attendance,Ship Unit %>"></asp:Label>
                                                    &nbsp:&nbsp
                                                    <asp:Label ID="lblShipUnitView" Font-Bold="true" runat="server"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label52" runat="server" Text="<%$ Resources:Attendance,Total Weight %>"></asp:Label>
                                                    &nbsp:&nbsp
                                                    <asp:Label ID="lblTotalWeightview" Font-Bold="true" runat="server"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label53" runat="server" Text="<%$ Resources:Attendance,Unit Rate %>"></asp:Label>
                                                    &nbsp:&nbsp
                                                    <asp:Label ID="lblUnitRateView" Font-Bold="true" runat="server"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label54" runat="server" Text="<%$ Resources:Attendance,Shipment Type %>"></asp:Label>
                                                    &nbsp:&nbsp
                                                    <asp:Label ID="lblShipmentTypeView" Font-Bold="true" runat="server"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label1View" runat="server" Text="<%$ Resources:Attendance,Freight Status %>"></asp:Label>
                                                    &nbsp:&nbsp
                                                    <asp:Label ID="lblFrieghtStatusView" Font-Bold="true" runat="server"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label14View" runat="server" Text="<%$ Resources:Attendance,Shipping Date %>"></asp:Label>
                                                    &nbsp:&nbsp
                                                    <asp:Label ID="lblShippingDateView" Font-Bold="true" runat="server"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label15View" runat="server" Text="<%$ Resources:Attendance,Receiving Date %>"></asp:Label>
                                                    &nbsp:&nbsp
                                                    <asp:Label ID="lblReceivingDateView" Font-Bold="true" runat="server"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label16View" runat="server" Text="<%$ Resources:Attendance,Partial Shipment %>"></asp:Label>
                                                    &nbsp:&nbsp
                                                    <asp:Label ID="lblPartialShipmentView" Font-Bold="true" runat="server"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <asp:Label ID="Label17View" runat="server" Text="<%$ Resources:Attendance,Condition 1 %>"></asp:Label>
                                                    <asp:Panel ID="pnlcondition1" runat="server" BorderColor="#8ca7c1" BorderStyle="Solid"
                                                        BorderWidth="1px" BackColor="White">
                                                        <asp:Literal ID="txtCondition1View" runat="server"></asp:Literal>
                                                    </asp:Panel>
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <asp:Label ID="Label18View" runat="server" Text="<%$ Resources:Attendance,Condition 2 %>"></asp:Label>
                                                    <asp:Panel ID="PanelCon2" runat="server" BorderColor="#8ca7c1" BorderStyle="Solid"
                                                        BorderWidth="1px" BackColor="White">
                                                        <asp:Literal ID="txtCondition2View" runat="server"></asp:Literal>
                                                    </asp:Panel>
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <asp:Label ID="Label19View" runat="server" Text="<%$ Resources:Attendance,Condition 3 %>"></asp:Label>
                                                    <asp:Panel ID="Pael4" runat="server" BorderColor="#8ca7c1" BorderStyle="Solid" BorderWidth="1px"
                                                        BackColor="White">
                                                        <asp:Literal ID="txtCondition3View" runat="server"></asp:Literal>
                                                    </asp:Panel>
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <asp:Label ID="Label20View" runat="server" Text="<%$ Resources:Attendance,Condition 4 %>"></asp:Label>
                                                    <asp:Panel ID="Panel10" runat="server" BorderColor="#8ca7c1" BorderStyle="Solid"
                                                        BorderWidth="1px" BackColor="White">
                                                        <asp:Literal ID="txtCondition4View" runat="server"></asp:Literal>
                                                    </asp:Panel>
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <asp:Label ID="Label21View" runat="server" Text="<%$ Resources:Attendance,Condition 5 %>"></asp:Label>
                                                    <asp:Panel ID="Panel11Con5" runat="server" BorderColor="#8ca7c1" BorderStyle="Solid"
                                                        BorderWidth="1px" BackColor="White">
                                                        <asp:Literal ID="txtCondition5View" runat="server"></asp:Literal>
                                                    </asp:Panel>
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
                    <asp:UpdatePanel ID="Update_Modal_Button" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                            <asp:Button ID="BtnCancelView" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Close %>" Visible="false"
                                CausesValidation="False" OnClick="BtnCancelView_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
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
    <div class="modal fade" id="Expenses_Tax_Web_Control" tabindex="-1" role="dialog" aria-labelledby="Expenses_Tax_Web_Control_Label"
        aria-hidden="true">
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
                                    <asp:HiddenField ID="Hdn_Saved_Expenses_Tax_Session" runat="server" />
                                    <asp:HiddenField ID="Hdn_Page_Name_Web_Control" runat="server" />
                                    <asp:HiddenField ID="Hdn_Tax_Entry_Type" runat="server" />
                                </div>
                            </div>
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

    <asp:UpdateProgress ID="UpdateProgress12" runat="server" AssociatedUpdatePanelID="Update_Li">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress13" runat="server" AssociatedUpdatePanelID="Update_List">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress9" runat="server" AssociatedUpdatePanelID="Update_New">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress10" runat="server" AssociatedUpdatePanelID="Update_Bin">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Quotation">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_Modal">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Modal_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Modal_GST">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress15" runat="server" AssociatedUpdatePanelID="Update_Modal_Button_GST">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress16" runat="server" AssociatedUpdatePanelID="UpdatePanel_Expenses">
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
        function LI_New_Quotation_Active() {
            $("#Li_Quotation").removeClass("active");
            $("#Quotation").removeClass("active");
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
        function Li_Tab_Quotation() {
            document.getElementById('<%= Btn_Quotation.ClientID %>').click();
        }
        function Purchase_Order_Modal_Popup() {
            document.getElementById('<%= Btn_Purchase_Order_Modal.ClientID %>').click();
        }
        function List_Hide_Tab_Content() {
            document.getElementById('Li_List').style.display = 'none';
            document.getElementById('List').style.display = 'none';
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");
            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }
    </script>
    <script type="text/javascript">
        function Show_Modal_GST() {
            document.getElementById('<%= Btn_GST_Modal.ClientID %>').click();
        }
        function Modal_Open_FileUpload() {
            document.getElementById('<%= Btn_Modal_FileUpload.ClientID %>').click();
        }
        function Show_Expenses_Tax_Web_Control() {
            document.getElementById('<%= Btn_Show_Expenses_Tax.ClientID %>').click();
        }
        function ddlTransType_SelectedIndexChanged(crtl) {
            PageMethods.ddlTransType_SelectedIndexChanged();
        }
        function txtProductCode_TextChanged(ctrl) {
            $.ajax({
                url: '../Sales/SalesInquiry.aspx/txtProduct_TextChanged',
                method: 'post',
                async: false,
                contentType: "application/json; charset=utf-8",
                data: "{'product':'" + ctrl.value + "'}",
                success: function (data) {
                    debugger;

                    if (data.d == null) {
                        alert("Select From Suggestions only");
                        document.getElementById('<%=txtProductName.ClientID%>').value = "";
                        document.getElementById('<%=txtProductcode.ClientID%>').value = "";
                        return;
                    }
                    else {
                        var dd = JSON.parse(data.d);
                        document.getElementById('<%=txtProductName.ClientID%>').value = dd[0];
                        document.getElementById('<%=txtProductcode.ClientID%>').value = dd[1];
                        //document.getElementById('<%=ddlUnit.ClientID%>').selectedIndex = parseInt(dd[2]);
                        if (dd[4] == null || dd[4] == undefined) {
                            document.getElementById('<%=hdnProductID.ClientID%>').value = "0";
                        }
                        else {
                            document.getElementById('<%=hdnProductID.ClientID%>').value = dd[4];
                        }
                        document.getElementById('<%=btnFillUnit.ClientID%>').click();
                        var replacedData1 = dd[3].replaceAll('<', '');
                        var replacedData2 = replacedData1.replaceAll('>', '');
                        document.getElementById('<%=hdnProductDesc.ClientID%>').value = replacedData2;
                    }
                }
            });
        }

        function lblgvSupplierName_Command(supplierId) {
            window.open('../Purchase/CustomerHistory.aspx?ContactId=' + supplierId + '&&Page=PO', 'window', 'width=1024, ');
        }
        function txtShipSupplierName_TextChanged(ctrl) {
            if (ctrl.value == "") {
                return;
            }
            var data = getSupplierAddressName(ctrl);
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
        function IbtnQC_Command(referenceId, referenceVoucherType) {
            if (referenceId != "0" && referenceVoucherType == "PQ") {
                PageMethods.IbtnQC_Command(referenceId, referenceVoucherType, function (data) {
                    if (data[0] == "true") {
                        window.open('../Purchase/CompareQuatation.aspx?RPQId=' + data[1] + '&&C=F');
                    }
                    else {
                        alert("Comparison Quataion Not Found");
                    }
                });
            }
            else {
                alert("Comparison Quataion Not Found");
            }
        }
        function IbtnPrint_Command(purchaseOrderId) {
            PageMethods.IbtnPrint_Command(purchaseOrderId, function (data) {
                if (data[0] == "false") {
                    alert(data[1]);
                    return;
                }
                else {
                    window.open('../Purchase/PurchaseOrder_Print.aspx?Id=' + data[1] + '', 'window', 'width=1024, ');
                }
            });
        }
        function resetDetail() {
            debugger;
            var dadat = $('#<%=pnlPDescription.ClientID%>');
            dadat[0].innerHTML = "";
            $('#<%=txtProductName.ClientID%>').val("");
            $('#<%=txtPDescription.ClientID%>').val("");
            $('#<%=txtfreeQty.ClientID%>').val("");
            $('#<%=hidProduct.ClientID%>').val("");
            $('#<%=hdnProductID.ClientID%>').val("");
            $('#<%=txtOrderQty.ClientID%>').val("");
            $('#<%=txtUnitCost.ClientID%>').val("");
            $('#<%=txtProductcode.ClientID%>').val("");
            $('#<%=txtProductcode.ClientID%>').focus();
            $('#<%=ddlUnit.ClientID%>').empty();
        }

        function txtShippingLine_TextChanged(ctrl) {
            var lblshipingLineMobileNo = document.getElementById('<%=lblshipingLineMobileNo.ClientID%>');
            var lblShipingEmailId = document.getElementById('<%=lblShipingEmailId.ClientID%>');
            debugger;
            var ContactId = "";
            try {
                ContactId = ctrl.value.split('/')[1];
            }
            catch (er) {
                ContactId = "0";
            }
            if (ContactId == "" || ContactId == "0" || ContactId == undefined) {
                //alert("Please Select from Suggestions Only");
                ctrl.value = "";
                lblshipingLineMobileNo.innerText = "";
                lblShipingEmailId.innerText = "";
                ctrl.focus();
                return;
            }
            else {
                lblshipingLineMobileNo.innerText = ' <%= Resources.Attendance.Mobile_No_1 %>: ' + ctrl.value.split('/')[3];
                lblShipingEmailId.innerText = ' <%= Resources.Attendance.Email_Id_1 %>: ' + ctrl.value.split('/')[2];
            }
        }
        function txtShippingAcc_TextChanged(ctrl) {
            debugger;
            var accountName = ctrl.value.split('/')[0];
            var TransId = ctrl.value.split('/')[1];
            PageMethods.txtShippingAcc_TextChanged(TransId, accountName, function (data) {
                debugger;
                if (data == "false") {
                    ctrl.value = "";
                    ctrl.focus();
                    alert("No Account Found");
                }
            });
        }
        function lnkSupplierHistory_OnClick() {
            var strSupplierId = "";
            var txtSupplierName = document.getElementById('<%=txtSupplierName.ClientID%>');

            if (txtSupplierName.value != "") {
                try {
                    strSupplierId = txtSupplierName.value.split('/')[1];
                }
                catch (er) {
                    strSupplierId = "0";
                }
            }
            else {
                strSupplierId = "0";
            }
            window.open('../Purchase/CustomerHistory.aspx?ContactId=' + strSupplierId + '&&Page=PO', 'window', 'width=1024, ');
        }

        function txtCommon_Click(ctrl) {
            if (ctrl.value == "") {
                ctrl.value = "0";
            }
        }
        function btnShowCreditInfo() {
            $('#creditDetails').modal('show');
        }
        function showUcControlsSettings() {
            $('#ControlSettingModal').modal('show');
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
        function openReportSystem(moduleID, objectId) {
            window.open('../utility/customizereport.aspx?Nav_ModuleId=' + moduleID + '&Nav_ObjectId=' + objectId + '', '_blank', '');
        }

        function getReportData(transId) {
            $("#prgBar").css("display", "block");
            document.getElementById('<%= reportSystem.FindControl("hdnTransId").ClientID %>').value = transId;
            setReportData();
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







        function getReportDataWithLoc(transId, locId) {
            $("#prgBar").css("display", "block");
            document.getElementById('<%= reportSystem.FindControl("hdnTransId").ClientID %>').value = transId;
            document.getElementById('<%= reportSystem.FindControl("hdnLocId").ClientID %>').value = locId;
            setReportData();
        }





    </script>
    <script src="../Script/customer.js"></script>
    <script src="../Script/ReportSystem.js"></script>
    <script src="../Script/master.js"></script>
    <script src="../Script/address.js"></script>
    <script src="../Script/common.js"></script>
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
    </script>
</asp:Content>
