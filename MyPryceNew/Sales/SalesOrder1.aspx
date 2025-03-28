<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="SalesOrder1.aspx.cs" Inherits="Sales_SalesOrder1" %>

<%@ Register Src="~/WebUserControl/ReportSystem.ascx" TagName="ReportSystem" TagPrefix="RS" %>
<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/ContactInfo.ascx" TagName="ViewContact" TagPrefix="AT1" %>
<%@ Register Src="~/WebUserControl/FileUpload.ascx" TagPrefix="AT1" TagName="FileUpload1" %>
<%@ Register Src="~/WebUserControl/AddressControl.ascx" TagName="AddAddress" TagPrefix="AA1" %>
<%@ Register Src="~/WebUserControl/AccountMaster.ascx" TagPrefix="uc1" TagName="AccountMaster" %>
<%@ Register Src="~/WebUserControl/ucControlsSetting.ascx" TagName="ucCtlSetting" TagPrefix="UC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
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
    </style>
    <script>
        function LI_Edit_Active() {

        }

        function resetPosition1() {

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">

    <h1>
        <i class="fa fa-file-text"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Sales Order%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Sales%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label532" runat="server" Text="<%$ Resources:Attendance,Sales Order%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hdnDecimalCount" runat="server" />
            <asp:HiddenField ID="hdnIsTaxEnabled" runat="server" Value="false" />
            <asp:HiddenField ID="hdnIsDiscountEnable" runat="server" Value="false" />
            <asp:HiddenField ID="hdnDenomination" runat="server" Value="" />
            <asp:HiddenField ID="hdnLocationId" runat="server" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
            <asp:HiddenField runat="server" ID="hdnCanPrint" />
            <asp:HiddenField runat="server" ID="hdnCanUpload" />
            <asp:HiddenField runat="server" ID="hdnCanViewAllRec" />
            <asp:HiddenField ID="hfScrollPosition" runat="server" Value="0" />
            <asp:HiddenField ID="hdfCurrentRow" runat="server" />
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:Button ID="Btn_Quotation" Style="display: none;" runat="server" OnClick="btnPRequest_Click" Text="Quotation" />
            <asp:Button ID="Btn_GST_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Modal_GST" Text="GST" />
            <asp:Button ID="Btn_CustomerInfo_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#modelContactDetail" />
            <asp:Button ID="Btn_Modal_FileUpload" Style="display: none;" runat="server" data-toggle="modal" data-target="#Fileupload123" Text="FileUpload" />
            <asp:Button ID="Btn_NewAddress" Style="display: none;" runat="server" data-toggle="modal" data-target="#NewAddress" Text="New Address" />
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
                        <i class="fas fa-file-invoice-dollar"></i>&nbsp;&nbsp;<asp:Label ID="Label223" runat="server" Text="<%$ Resources:Attendance,Quotation %>"></asp:Label></a></li>
                    <li id="Li_Bin"><a href="#Bin" onclick="Li_Tab_Bin()" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
                    <li id="Li_New"><a onclick="Li_Tab_New()" href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="GvSalesOrder" />
                                <asp:AsyncPostBackTrigger ControlID="btnSOrderSave" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="BtnReset" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="GvSalesQuotation" />
                            </Triggers>
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
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSOrderSave" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="imgBtnRestore" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="BtnReset" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>


                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div3" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label18" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
                                                <asp:Label ID="lblTotalRecords" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-md-5">
                                                    <asp:DropDownList ID="ddlLocation" runat="server" Class="form-control" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-5">
                                                    <asp:DropDownList ID="ddlUser" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlUser_Click">
                                                    </asp:DropDownList>
                                                    <br />
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:Panel ID="pnlGetOrder" runat="server" DefaultButton="btnGetOrder" Visible="false">
                                                        <asp:Button ID="btnGetOrder" runat="server" Text="Get Order" CssClass="btn btn-primary"
                                                            OnClick="btnGetOrder_Click" />
                                                    </asp:Panel>
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
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
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Order Id %>" Value="Trans_Id"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Order No. %>" Value="SalesOrderNo"
                                                            Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Product Id %>" Value="ProductId"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Order Date %>" Value="SalesOrderDate"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Reference Type %>" Value="TransType"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Ref No. %>" Value="QauotationNo"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Customer Name %>" Value="CustomerName"></asp:ListItem>
                                                        <asp:ListItem Text="Customer Order No" Value="Field3"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Payment Mode %>" Value="PaymentModeName"></asp:ListItem>
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
                                                        <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" placeholder="Search from Content"></asp:TextBox>
                                                        <asp:TextBox ID="txtValueDate" runat="server" CssClass="form-control" placeholder="Search from Date" Visible="false"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueDate" runat="server" TargetControlID="txtValueDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbindrpt_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefreshReport_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                                    <asp:LinkButton ID="btnGvListSetting" ImageAlign="Right" ToolTip="List Settings" runat="server" OnClick="btnGvListSetting_Click" Visible="false"><span class="fa fa-wrench"  style="font-size:25px;"></span></asp:LinkButton>
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
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvSalesOrder" PageSize="<%# PageControlCommon.GetPageSize() %>" runat="server"
                                                        AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvSalesOrder_PageIndexChanging"
                                                        AllowSorting="True" OnSorting="GvSalesOrder_Sorting" CurrentSortField="SalesOrderDate" CurrentSortDirection="DESC">
                                                        <Columns>

                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnPrint" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="IbtnPrint_Command" ToolTip="<%$ Resources:Attendance,Print %>"><i class="fa fa-print"></i>Print</asp:LinkButton>
                                                                            </li>
                                                                            <li runat="server" visible="true">
                                                                                <a onclick="getReportDataWithLoc('<%# Eval("Trans_Id") %>','<%# Eval("Location_Id") %>')"><i class="fa fa-print" style="cursor: pointer"></i>Report System</a>
                                                                            </li>

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' CommandName='<%# Eval("Location_Id") %>' OnCommand="lnkViewDetail_Command" CausesValidation="False"><i class="fa fa-eye"></i>View</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' CommandName='<%# Eval("Location_Id") %>' OnCommand="btnEdit_Command" CausesValidation="False"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>

                                                                            <li <%= hdnCanUpload.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnFileUpload" runat="server" CommandName='<%#Eval("SalesOrderNo") %>' CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="btnFileUpload_Command" CausesValidation="False"><i class="fa fa-upload"></i>File Upload</asp:LinkButton>
                                                                            </li>
                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>

                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order No. %>" SortExpression="SalesOrderNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvSONo" runat="server" Text='<%#Eval("SalesOrderNo") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order Date %>" SortExpression="SalesOrderDate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvSODate" runat="server" Text='<%#GetDate(Eval("SalesOrderDate").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Name %>" SortExpression="CustomerName">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lBtnCustomerInfo" ToolTip="Click To See Contact Information" runat="server" Text='<%# Eval("CustomerName").ToString() %>' CommandArgument='<%# Eval("CustomerId") %>' OnCommand="lBtnCustomerInfo_Command"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Customer Order No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lBtnCustomerRef" runat="server" Text='<%# Eval("Field3").ToString() %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Reference Type%>" SortExpression="TransType">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvTransType" runat="server" Text='<%#Eval("TransType") %>' />
                                                                    <asp:Label ID="lblInvoiceStatus" runat="server" Text='<%#Eval("InvoiceStatus") %>' Visible="false" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Ref No.%>" SortExpression="QauotationNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvTransNo" runat="server" Text='<%#Eval("QauotationNo") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status%>" SortExpression="Field4">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblst" runat="server" Text='<%# Eval("Field4").ToString() %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Payment Mode%>" SortExpression="PaymentModeName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPaymentname" runat="server" Text='<%# Eval("PaymentModeName").ToString() %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="CreatedUser">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvUser" runat="server" Text='<%#Eval("CreatedUser") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>



                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>" SortExpression="ModifiedUser">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModified_User" runat="server" Text='<%# Eval("ModifiedUser") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Amount%>" SortExpression="NetAmount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblnetAmount" runat="server" Text='<%# SystemParameter.GetAmountWithDecimal(Eval("NetAmount").ToString(),Eval("CurrencyDecimalCount").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerStyle CssClass="pagination-ys" />
                                                    </asp:GridView>
                                                    <asp:HiddenField ID="hdnGvSalesOrderCurrentPageIndex" Value="1" runat="server" />
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
                        </asp:UpdatePanel>

                    </div>
                    <div class="tab-pane" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="GvSalesOrder" />
                                <asp:AsyncPostBackTrigger ControlID="BtnReset" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="GvSalesQuotation" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <asp:ImageButton ID="btnControlsSetting" ImageAlign="Right" ToolTip="Controls Setting" runat="server" ImageUrl="~/Images/setting.png" OnClick="btnControlsSetting_Click" Style="width: 32px; height: 32px" Visible="false" />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:RadioButton ID="rbtEdit" runat="server" Visible="false" GroupName="SaveOrEdit" Checked="true" Text="Edit" />
                                                        <asp:RadioButton ID="rbtNew" runat="server" Visible="false" GroupName="SaveOrEdit" Text="New" />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblSODate" runat="server" Text="<%$ Resources:Attendance,Order Date %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSODate" ErrorMessage="<%$ Resources:Attendance,Enter Order Date%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtSODate" runat="server" CssClass="form-control" ReadOnly="true" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender" runat="server" TargetControlID="txtSODate" Enabled="false" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblSONo" runat="server" Text="<%$ Resources:Attendance,Order No. %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSONo" ErrorMessage="<%$ Resources:Attendance,Enter Order No%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtSONo" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblOrderType" runat="server" Text="<%$ Resources:Attendance,Order Type %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save" Display="Dynamic"
                                                            SetFocusOnError="true" ControlToValidate="ddlOrderType" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Order Type %>" />

                                                        <asp:DropDownList ID="ddlOrderType" runat="server" CssClass="form-control"
                                                            OnSelectedIndexChanged="ddlOrderType_SelectedIndexChanged" AutoPostBack="true">
                                                            <asp:ListItem Text="<%$ Resources:Attendance,--Select--%>" Value="--Select--"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance, Direct%>" Value="D"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,By Quotation%>" Value="Q"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" id="ctlCustomerOrderNO" runat="server">
                                                        <asp:Label ID="lblCustOrderNo" runat="server" Text="<%$ Resources:Attendance,Customer Order No. %>" />
                                                        <asp:TextBox ID="txtCustOrderNo" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblCurrency" runat="server" Text="<%$ Resources:Attendance,Currency %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Save" Display="Dynamic"
                                                            SetFocusOnError="true" ControlToValidate="ddlCurrency" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Currency%>" />

                                                        <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="form-control"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlCurrency_OnSelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblPaymentMode" runat="server" Text="<%$ Resources:Attendance,Payment Mode %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="Save" Display="Dynamic"
                                                            SetFocusOnError="true" ControlToValidate="ddlPaymentMode" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Payment Mode%>" />

                                                        <asp:DropDownList ID="ddlPaymentMode" runat="server" CssClass="form-control"
                                                            AutoPostBack="True" OnSelectedIndexChanged="ddlPaymentTypeMode_SelectedIndexChanged" />

                                                        <br />
                                                    </div>
                                                    <div class="col-md-4" id="ctlSalesPerson" runat="server">
                                                        <asp:HiddenField ID="hdnSalesPersonId" runat="server" />
                                                        <asp:Label ID="lblSalesPerson" runat="server" Text="Sales Person" />
                                                        <asp:TextBox ID="txtSalesPerson" runat="server" CssClass="form-control" OnTextChanged="txtSalesPerson_TextChanged" AutoPostBack="true" BackColor="#eeeeee" />
                                                        <cc1:AutoCompleteExtender ID="txtSalesPerson_AutoCompleteExtender" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="1" ServiceMethod="GetCompletionListEmployeeName" ServicePath=""
                                                            TargetControlID="txtSalesPerson" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div id="trTransfer" runat="server" visible="false">
                                                        <div class="col-md-4">
                                                            <asp:Label ID="lblTransFrom" runat="server" Text="<%$ Resources:Attendance,Transfer From %>" />
                                                            <asp:TextBox ID="txtTransFrom" runat="server" CssClass="form-control" ReadOnly="True" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:Label ID="lblQuotationNo" runat="server" Text="<%$ Resources:Attendance,Quotation No. %>" />
                                                            <a style="color: Red">*</a>
                                                            <asp:DropDownList ID="ddlQuotationNo" runat="server" CssClass="form-control"
                                                                Visible="false" OnSelectedIndexChanged="ddlQuotationNo_SelectedIndexChanged"
                                                                AutoPostBack="true" />
                                                            <asp:TextBox ID="txtQuotationNo" runat="server" ReadOnly="true" CssClass="form-control" Visible="false" />
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblCustomerName" runat="server" Text="<%$ Resources:Attendance,Customer Name %>" />
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtCustomer" runat="server" CssClass="form-control" AutoPostBack="false"
                                                                BackColor="#eeeeee" onchange="txtCustomer_TextChanged(this)" />
                                                            <cc1:AutoCompleteExtender ID="txtCustomerName_AutoCompleteExtender" runat="server"
                                                                DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                                MinimumPrefixLength="1" ServiceMethod="GetCompletionListCustomer" ServicePath=""
                                                                TargetControlID="txtCustomer" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <div class="input-group-btn">
                                                                <asp:Button ID="lnkcustomerHistory" runat="server" Text="History" CssClass="btn btn-primary" CausesValidation="False"
                                                                    OnClientClick="lnkcustomerHistory_OnClick()"></asp:Button>
                                                                <input type="button" class="btn btn-primary" value="Credit Detail" onclick="GetCreditInfo()" />
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" id="Trans_Div" runat="server">
                                                        <asp:Label ID="lblTransType" runat="server" Text="Transaction Type"></asp:Label>
                                                        <%--<a style="color: Red">*</a>--%>
                                                        <asp:DropDownList ID="ddlTransType" AutoPostBack="true" OnSelectedIndexChanged="ddlTransType_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        <%--<asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator14" ValidationGroup="Save" Display="Dynamic"
                                                                SetFocusOnError="true" ControlToValidate="ddlTransType" InitialValue="-1" ErrorMessage="<%$ Resources:Attendance,Select Transaction Type%>" />--%>
                                                        <br />
                                                    </div>


                                                    <div class="col-md-6">
                                                        <asp:HiddenField ID="hdnContactPersonId" runat="server" />
                                                        <asp:Label ID="lblContactPerson" runat="server" Text="Contact Person" />

                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtContactPerson" runat="server" CssClass="form-control" BackColor="#eeeeee" OnTextChanged="txtContactId_TextChanged" AutoPostBack="true" />
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" DelimiterCharacters=""
                                                                Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="0"
                                                                ServiceMethod="GetContactListCustomer" ServicePath="" TargetControlID="txtContactPerson"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <div class="input-group-btn">
                                                                <asp:Button ID="btnAddCustomer" runat="server" CssClass="btn btn-primary" OnClick="btnAddCustomer_Click"
                                                                    Text="<%$ Resources:Attendance,Add %>" CausesValidation="False" />
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label14" runat="server" Text="Agent Name" />
                                                        <asp:TextBox ID="txtAgentName" runat="server" CssClass="form-control"
                                                            Enabled="false" BackColor="#eeeeee" OnTextChanged="txtAgentName_TextChanged"
                                                            AutoPostBack="true" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtenderAgentName" runat="server" DelimiterCharacters=""
                                                            Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="0"
                                                            ServiceMethod="GetContactList" ServicePath="" TargetControlID="txtAgentName"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblInvoiceTo" runat="server" Text="<%$ Resources:Attendance,Invoice Address %>" />
                                                        <asp:TextBox ID="txtInvoiceTo" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            AutoPostBack="false" onchange="InvoiceAddressDetail();" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListAddressName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtInvoiceTo"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblEstimateDeliveryDate" runat="server" Text="<%$ Resources:Attendance,Estimate Delivery Date %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator6" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEstimateDeliveryDate" ErrorMessage="<%$ Resources:Attendance,Enter Estimate Delivery Date%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtEstimateDeliveryDate" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender" runat="server" TargetControlID="txtEstimateDeliveryDate" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <table style="border: 1px solid;">
                                                            <tr>
                                                                <th>
                                                                    <asp:Label ID="InvoiceAddress" runat="server" Text=""></asp:Label>
                                                                </th>
                                                            </tr>

                                                        </table>
                                                        <br />
                                                    </div>


                                                    <div class="col-md-5">
                                                        <asp:Label ID="Label332" runat="server" Text="<%$ Resources:Attendance,Ship To %>" />
                                                        <asp:TextBox ID="txtShipCustomerName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            onchange="txtCustomer_TextChanged(this)" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                            Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                            ServiceMethod="GetCompletionListContact" ServicePath="" TargetControlID="txtShipCustomerName"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-5" id="ctlShipingAddress" runat="server">
                                                        <asp:Label ID="lblShipingAddress" runat="server" Text="<%$ Resources:Attendance,Shipping Address %>" />
                                                        <asp:TextBox ID="txtShipingAddress" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            AutoPostBack="false" onchange="ShipingAddressDetail()" />
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
                                                        <br />
                                                        <table style="border: 1px solid;">
                                                            <tr>
                                                                <th>
                                                                    <asp:Label ID="ShipAddress" runat="server" Text=""></asp:Label>
                                                                </th>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <br />
                                                        <table style="border: 1px solid;">
                                                            <tr>
                                                                <th>
                                                                    <asp:Label ID="ShippingAddress" runat="server" Text=""></asp:Label>
                                                                </th>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:RadioButton ID="rbtnFormView" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Form View%>"
                                                            Visible="false" AutoPostBack="true" GroupName="Product"
                                                            OnCheckedChanged="rbtnFormView_OnCheckedChanged" />

                                                        <asp:RadioButton ID="rbtnAdvancesearchView" Style="margin-left: 20px;" Font-Bold="true" runat="server" Visible="false"
                                                            Text="<%$ Resources:Attendance,Advance Search View%>"
                                                            AutoPostBack="true" GroupName="Product" OnCheckedChanged="rbtnFormView_OnCheckedChanged" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Button ID="btnAddNewProduct" runat="server" Style="display: none" Text="<%$ Resources:Attendance,Add Product %>" CssClass="btn btn-info" Visible="false" OnClick="btnAddNewProduct_Click" />
                                                        <asp:Button ID="btnAddProductScreen" Visible="false" runat="server" Text="<%$ Resources:Attendance,Add Product List %>" CssClass="btn btn-info" OnClick="btnAddProductScreen_Click" />
                                                        <asp:Button ID="btnAddtoList" runat="server" Text="<%$ Resources:Attendance,Fill Your Product %>" CssClass="btn btn-info" Visible="false" OnClick="btnAddtoList_Click" />
                                                        <br />
                                                    </div>
                                                    <div id="pnlProduct1" runat="server" class="col-md-12">
                                                        <div class="row">
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
                                                                            <asp:UpdatePanel ID="updPnlPayment" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>

                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Product Id%>" />
                                                                                        <asp:TextBox ID="txtProductcode" runat="server" CssClass="form-control" AutoPostBack="True"
                                                                                            OnTextChanged="txtProductCode_TextChanged" BackColor="#eeeeee" />
                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" CompletionInterval="100"
                                                                                            DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductCode"
                                                                                            ServicePath="" TargetControlID="txtProductcode" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                        </cc1:AutoCompleteExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblProductName" runat="server" Text="<%$ Resources:Attendance,Product Name %>" />
                                                                                        <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                                            OnTextChanged="txtProductName_TextChanged" BackColor="#eeeeee" />
                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                                                            Enabled="True" ServiceMethod="GetCompletionListProductName" ServicePath="" CompletionInterval="100"
                                                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtProductName"
                                                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                        </cc1:AutoCompleteExtender>
                                                                                        <asp:HiddenField ID="hdnNewProductId" runat="server" Value="0" />
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblUnit" runat="server" Text="<%$ Resources:Attendance,Unit %>" />
                                                                                        <asp:DropDownList ID="ddlUnit" runat="server" CssClass="form-control" />
                                                                                        <asp:HiddenField ID="hdnUnitId" runat="server" Value="0" />
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblPQuantity" runat="server" Text="<%$ Resources:Attendance,Quantity %>" />
                                                                                        <asp:TextBox ID="txtPQuantity" runat="server" CssClass="form-control" OnTextChanged="txtPQuantity_TextChanged"
                                                                                            AutoPostBack="true" />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                                            TargetControlID="txtPQuantity" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblPFreeQuantity" runat="server" Text="<%$ Resources:Attendance,Free Quantity %>" />
                                                                                        <asp:TextBox ID="txtPFreeQuantity" runat="server" CssClass="form-control" />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                                            TargetControlID="txtPFreeQuantity" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblPUnitPrice" runat="server" Text="<%$ Resources:Attendance,Unit Price %>" />
                                                                                        <asp:TextBox ID="txtPUnitPrice" runat="server" CssClass="form-control" Text="0" OnTextChanged="txtPUnitPrice_TextChanged"
                                                                                            AutoPostBack="true" />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                                                            TargetControlID="txtPUnitPrice" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblQuantityPrice" runat="server" Text="<%$ Resources:Attendance,Gross Price %>" />
                                                                                        <asp:TextBox ID="txtPQuantityPrice" runat="server" CssClass="form-control" ReadOnly="true" />
                                                                                        <br />
                                                                                    </div>

                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblPDiscount" runat="server" Text="<%$ Resources:Attendance,Discount(%) %>" />
                                                                                        <div class="input-group">
                                                                                            <asp:TextBox ID="txtPDiscountPUnit" runat="server" CssClass="form-control"
                                                                                                OnTextChanged="txtPDiscountP_TextChanged" AutoPostBack="true" />
                                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                                                                                TargetControlID="txtPDiscountPUnit" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                            <div class="input-group-addon">
                                                                                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Value %>" />
                                                                                            </div>
                                                                                            <div class="input-group-btn">
                                                                                                <asp:TextBox ID="txtPDiscountVUnit" Width="120px" runat="server" CssClass="form-control"
                                                                                                    OnTextChanged="txtPDiscountV_TextChanged" AutoPostBack="true" />
                                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True"
                                                                                                    TargetControlID="txtPDiscountVUnit" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                                </cc1:FilteredTextBoxExtender>
                                                                                            </div>
                                                                                        </div>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div id="Div_Tax" runat="server" class="col-md-6">
                                                                                        <asp:Label ID="lblPTax" runat="server" Text="<%$ Resources:Attendance,Tax(%) %>"
                                                                                            Visible="false" />
                                                                                        <asp:TextBox ID="txtPTaxPUnit" runat="server" CssClass="form-control"
                                                                                            OnTextChanged="txtPTaxP_TextChanged" AutoPostBack="true" Visible="false" />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                                                            TargetControlID="txtPTaxPUnit" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <asp:Label ID="Label4" runat="server" Visible="false" Text="<%$ Resources:Attendance,Value %>" />
                                                                                        <asp:TextBox ID="txtPTaxVUnit" runat="server" CssClass="form-control"
                                                                                            OnTextChanged="txtPTaxV_TextChanged" AutoPostBack="true" Visible="false" />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                                                                            TargetControlID="txtPTaxVUnit" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblPTotalAmount" runat="server" Text="<%$ Resources:Attendance,Net Price%>" />
                                                                                        <asp:TextBox ID="txtPTotalAmount" runat="server" CssClass="form-control" ReadOnly="True" />
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-12">
                                                                                        <asp:Label ID="lblPDescription" runat="server" Text="<%$ Resources:Attendance,Product Description %>" />
                                                                                        <asp:Panel ID="pnlPDescription" Style="width: 100%; min-width: 100%; max-width: 100%; height: 70px; min-height: 70px; max-height: 200px; overflow: auto;" runat="server" CssClass="form-control"
                                                                                            BorderColor="#8ca7c1" BackColor="#ffffff" ScrollBars="Vertical">
                                                                                            <asp:Literal ID="txtPDescription" runat="server"></asp:Literal>
                                                                                        </asp:Panel>
                                                                                        <br />
                                                                                    </div>

                                                                                </ContentTemplate>
                                                                                <Triggers>
                                                                                    <asp:AsyncPostBackTrigger ControlID="btnProductSave" EventName="Click" />
                                                                                    <asp:AsyncPostBackTrigger ControlID="btnProductCancel" EventName="Click" />
                                                                                    <asp:AsyncPostBackTrigger ControlID="GvSalesOrder" />
                                                                                    <asp:AsyncPostBackTrigger ControlID="GvSalesQuotation" />
                                                                                    <asp:AsyncPostBackTrigger ControlID="GvQuotationDetail" />
                                                                                    <asp:AsyncPostBackTrigger ControlID="GvProductDetail" />
                                                                                </Triggers>
                                                                            </asp:UpdatePanel>
                                                                            <div class="col-md-12" style="text-align: center">
                                                                                <br />
                                                                                <asp:Button ID="btnProductSave" runat="server" Text="<%$ Resources:Attendance,Add Product %>"
                                                                                    CssClass="btn btn-primary" OnClick="btnProductSave_Click" />

                                                                                <asp:Button ID="btnProductCancel" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Cancel %>"
                                                                                    CausesValidation="False" OnClick="btnProductCancel_Click" />
                                                                                <asp:HiddenField ID="HiddenField3" runat="server" />
                                                                                <br />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <asp:UpdatePanel ID="updPnlDetail" runat="server" UpdateMode="Conditional">
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="GvSalesOrder" />
                                                            <asp:AsyncPostBackTrigger ControlID="GvSalesQuotation" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnProductSave" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnAddNewProduct" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnAddProductScreen" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnAddtoList" />
                                                        </Triggers>
                                                        <ContentTemplate>
                                                            <asp:Panel ID="pnlDetail" runat="server">
                                                                <div class="col-md-12">
                                                                    <div style="overflow: auto; max-height: 500px;">
                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvQuotationDetail" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                            OnRowCreated="GvQuotationDetail_RowCreated" OnRowDataBound="GvQuotationDetail_OnRowDataBound"
                                                                            DataKeyNames="Product_Id">

                                                                            <Columns>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkGvQuotationDetailSelect" runat="server" AutoPostBack="true"
                                                                                            OnCheckedChanged="chkGvQuotationDetailSelect_CheckedChanged" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvSerialNo" runat="server" Text='<%#Eval("Serial_No") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvProductCode" runat="server" Text='<%#ProductCode(Eval("Product_Id").ToString()) %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>">
                                                                                    <ItemTemplate>
                                                                                        <table width="100%">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:HiddenField ID="hdngvProductId" runat="server" Value='<%#Eval("Product_Id") %>' />
                                                                                                    <asp:Label ID="lblgvProductName" Width="200px" runat="server" Text='<%#GetProductName(Eval("Product_Id").ToString()) %>' />
                                                                                                </td>
                                                                                                <td align='<%= PageControlCommon.ChangeTDForDefaultRight()%>'>
                                                                                                    <asp:ImageButton ID="lnkDes" runat="server" ImageUrl="~/Images/detail.png" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                        <asp:Panel ID="PopupMenu1" Width="100%" runat="server">
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
                                                                                                                        <asp:Literal ID="lblgvProductDescription" runat="server" Text='<%# GetProductDescription(Eval("Product_Id").ToString()) %>'></asp:Literal>
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
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:DropDownList ID="ddlgvUnit" runat="server" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quantity %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="lblgvQuantity" ForeColor="#4d4c4c" Width="50px" runat="server" OnTextChanged="lblgvQuantity_TextChanged"
                                                                                            AutoPostBack="true" Text='<%#Eval("Quantity") %>' />
                                                                                        <cc1:FilteredTextBoxExtender ID="filtertextbox1" runat="server" Enabled="True" TargetControlID="lblgvQuantity"
                                                                                            ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Free %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtgvFreeQuantity" Width="50px" runat="server" />
                                                                                        <cc1:FilteredTextBoxExtender ID="filtertextbox22" runat="server" Enabled="True" TargetControlID="txtgvFreeQuantity"
                                                                                            ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Remain %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="txtgvRemainQuantity" Width="50px" runat="server" Text='<%#Convert.ToInt32(Eval("Quantity")).ToString() %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Price %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtgvUnitPrice" Width="50px" runat="server" OnTextChanged="txtgvUnitPrice_TextChanged"
                                                                                            AutoPostBack="true" />
                                                                                        <cc1:FilteredTextBoxExtender ID="filtertextbox2" runat="server" Enabled="True" TargetControlID="txtgvUnitPrice"
                                                                                            ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox Width="50px" ID="txtgvQuantityPrice" ReadOnly="true" runat="server" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,% %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtgvDiscountP" Width="50px" runat="server" OnTextChanged="txtgvDiscountP_TextChanged"
                                                                                            AutoPostBack="true" />
                                                                                        <cc1:FilteredTextBoxExtender ID="filtertextbox3" runat="server" Enabled="True" TargetControlID="txtgvDiscountP"
                                                                                            ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Value %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtgvDiscountV" Width="50px" runat="server" OnTextChanged="txtgvDiscountV_TextChanged"
                                                                                            AutoPostBack="true" />
                                                                                        <cc1:FilteredTextBoxExtender ID="filtertextbox4" runat="server" Enabled="True" TargetControlID="txtgvDiscountV"
                                                                                            ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,After Price %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtgvPriceAfterDiscount" Width="50px" ReadOnly="true" runat="server" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,% %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtgvTaxP" Visible="true" Width="50px" runat="server" Enabled="false" OnTextChanged="txtgvTaxP_TextChanged"
                                                                                            AutoPostBack="true" />
                                                                                        <cc1:FilteredTextBoxExtender ID="filtertextbox5" runat="server" Enabled="True" TargetControlID="txtgvTaxP"
                                                                                            ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <asp:ImageButton ID="BtnAddTax_Gv_Detail" runat="server" CommandName="GvQuotationDetail" CommandArgument='<%# Eval("Product_Id") %>' OnCommand="BtnAddTax_Command" ImageUrl="~/Images/plus.png" Width="30px" Height="30px" ToolTip="Add Tax" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Value %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtgvTaxV" Enabled="false" Width="50px" runat="server" OnTextChanged="txtgvTaxV_TextChanged"
                                                                                            AutoPostBack="true" />
                                                                                        <cc1:FilteredTextBoxExtender ID="filtertextbox6" runat="server" Enabled="True" TargetControlID="txtgvTaxV"
                                                                                            ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,After Price %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtgvPriceAfterTax" Width="50px" ReadOnly="true" runat="server" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtgvTotal" Width="50px" ReadOnly="true" runat="server" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <table width="100%">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:CheckBox Visible="false" ID="chkIsProduction" runat="server" Checked='<%#Eval("Field6") %>' />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:DropDownList Width="100px" ID="ddlLocation" runat="server">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton ID="lnkStockInfo" runat="server" Text='<%#GetProductStock(Eval("Product_Id").ToString()) %>'
                                                                                            Font-Underline="true" ToolTip="View Detail" OnCommand="lnkStockInfo_Command"
                                                                                            ForeColor="Blue" CommandArgument='<%# Eval("Product_Id") %>'></asp:LinkButton>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField Visible="false" HeaderText="Commission">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtgvAgentCommission" Width="50px" ForeColor="#4d4c4c" runat="server"
                                                                                            Enabled='<%#IsAddAgentCommission(Eval("AgentCommission").ToString()) %>' Text='<%#Eval("AgentCommission") %>' />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtgvAgentCommission" runat="server"
                                                                                            Enabled="True" TargetControlID="txtgvAgentCommission" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                            </Columns>

                                                                            <PagerStyle CssClass="pagination-ys" />

                                                                        </asp:GridView>
                                                                    </div>

                                                                    <div class="col-md-12" runat="server" id="scrollArea" onscroll="SetDivPosition()" style="overflow: auto; max-height: 500px;">
                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvProductDetail" runat="server" AutoGenerateColumns="False" Width="100%" OnRowCreated="GvProductDetail_RowCreated" DataKeyNames="Product_Id"
                                                                            ShowFooter="false" OnRowDataBound="GvProductDetail_OnRowDataBound">

                                                                            <Columns>

                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton ID="imgBtnaddtax" runat="server" CommandArgument='<%# Eval("Serial_No") %>'
                                                                                            ImageUrl="~/Images/plus.png" Height="30px" OnCommand="imgaddTax_Command"
                                                                                            ToolTip="Add Tax" />
                                                                                        <%-- <asp:ImageButton ID="imgBtnProductEdit" runat="server" CommandArgument='<%# Eval("Serial_No") %>'
                                                                            ImageUrl="~/Images/edit.png" Width="16px" OnCommand="imgBtnProductEdit_Command" />--%>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton ID="imgBtnProductDelete" runat="server" CommandArgument='<%# Eval("Serial_No") %>'
                                                                                            CssClass="fa fa-trash" OnCommand="imgBtnProductDelete_Command" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvSerialNo" runat="server" Visible="false" Text='<%#Eval("Serial_No") %>' />
                                                                                        <asp:Label ID="lblSerialNo" runat="server" Text='<%#Container.DataItemIndex+1 %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvProductCode" runat="server" Text='<%#ProductCode(Eval("Product_Id").ToString()) %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>" ItemStyle-Width="150px">
                                                                                    <HeaderTemplate>
                                                                                        <asp:Label ID="lblGrdHdProductName" Text="<%$ Resources:Attendance,Product Name %>" runat="server"></asp:Label>
                                                                                        <asp:CheckBox ID="chkShortProductName1" Text="" runat="server" ToolTip="Dispay detail Name" OnCheckedChanged="chkShortProductName_CheckedChanged" AutoPostBack="true"></asp:CheckBox>
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <table width="100%">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:HiddenField ID="hdngvProductId" runat="server" Value='<%#Eval("Product_Id") %>' />
                                                                                                    <asp:Label ID="lblgvProductName" Width="150px" runat="server" Text='<%#GetProductName(Eval("Product_Id").ToString()) %>' Visible="false" />
                                                                                                    <asp:Label ID="lblShortProductName1" Font-Size="10" runat="server" Text='<%# GetShortProductName(Eval("Product_Id").ToString()) %>' Visible="true"></asp:Label>
                                                                                                </td>
                                                                                                <td align='<%= PageControlCommon.ChangeTDForDefaultRight()%>'>
                                                                                                    <asp:ImageButton ID="lnkDes" runat="server" ImageUrl="~/Images/detail.png" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                        <asp:Panel ID="PopupMenu1" Width="100%" runat="server">
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
                                                                                                                        <asp:Literal ID="lblgvProductDescription" runat="server" Text='<%# GetProductDescription(Eval("Product_Id").ToString()) %>'></asp:Literal>
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
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="hdngvUnitId" runat="server" Value='<%#Eval("UnitId") %>' />
                                                                                        <asp:Label ID="lblgvUnit" runat="server" Text='<%#GetUnitName(Eval("UnitId").ToString()) %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quantity %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="lblgvQuantity" onchange="SetSelectedRow(this)" Width="50px" runat="server" Text='<%#Eval("Quantity") %>'
                                                                                            OnTextChanged="lblgvdQuantity_TextChanged" AutoPostBack="true" />
                                                                                        <cc1:FilteredTextBoxExtender ID="filtertextbox7" runat="server" Enabled="True" TargetControlID="lblgvQuantity"
                                                                                            ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Free %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="lblgvFreeQuantity" onchange="SetSelectedRow(this)" Width="50px" runat="server" Text='<%#Eval("FreeQty") %>' />
                                                                                        <cc1:FilteredTextBoxExtender ID="filtertextbox20" runat="server" Enabled="True" TargetControlID="lblgvFreeQuantity"
                                                                                            ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Remain %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvRemainQuantity" runat="server" Text='<%#Eval("RemainQty") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Price %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="lblgvUnitPrice" onchange="SetSelectedRow(this)" Width="50px" runat="server" Text='<%#Eval("UnitPrice") %>'
                                                                                            OnTextChanged="txtgvdUnitPrice_TextChanged" AutoPostBack="true" />
                                                                                        <cc1:FilteredTextBoxExtender ID="filtertextbox8" runat="server" Enabled="True" TargetControlID="lblgvUnitPrice"
                                                                                            ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvQuantityPrice" runat="server" Text='<%#Eval("GrossPrice") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,% %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="lblgvDiscountP" onchange="SetSelectedRow(this)" Width="50px" runat="server" Text='<%#Eval("DiscountP") %>'
                                                                                            OnTextChanged="txtgvdDiscountP_TextChanged" AutoPostBack="true" />
                                                                                        <cc1:FilteredTextBoxExtender ID="filtertextbox9" runat="server" Enabled="True" TargetControlID="lblgvDiscountP"
                                                                                            ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Value %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="lblgvDiscountV" onchange="SetSelectedRow(this)" Width="50px" runat="server" Text='<%#Eval("DiscountV") %>'
                                                                                            OnTextChanged="txtgvdDiscountV_TextChanged" AutoPostBack="true" />
                                                                                        <cc1:FilteredTextBoxExtender ID="filtertextbox10" runat="server" Enabled="True" TargetControlID="lblgvDiscountV"
                                                                                            ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,After Price %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvPriceAfterDiscount" runat="server" Visible="false" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,% %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="lblgvTaxP" onchange="SetSelectedRow(this)" Visible="true" Width="50px" runat="server" Text='<%#Eval("TaxP") %>'
                                                                                            OnTextChanged="txtgvdTaxP_TextChanged" AutoPostBack="true" Enabled="false" />
                                                                                        <cc1:FilteredTextBoxExtender ID="filtertextbox11" runat="server" Enabled="True" TargetControlID="lblgvTaxP"
                                                                                            ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <asp:ImageButton ID="BtnAddTax_Product" runat="server" CommandName="GvProductDetail" CommandArgument='<%# Eval("Product_Id") %>' OnCommand="BtnAddTax_Command" ImageUrl="~/Images/plus.png" Width="30px" Height="30px" ToolTip="Add Tax" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Value %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="lblgvTaxV" onchange="SetSelectedRow(this)" Width="50px" runat="server" Text='<%#Eval("TaxV") %>'
                                                                                            OnTextChanged="txtgvdTaxV_TextChanged" AutoPostBack="true" Enabled="false" />
                                                                                        <cc1:FilteredTextBoxExtender ID="filtertextbox12" runat="server" Enabled="True" TargetControlID="lblgvTaxV"
                                                                                            ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,After Price %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvPriceAfterTax" runat="server" Visible="false" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvTotal" runat="server" Text='<%#Eval("NetTotal") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <table width="100%">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:CheckBox Visible="false" ID="chkIsProduction" runat="server" Checked='<%#Eval("Field6") %>' />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:DropDownList ID="ddlLocation" Width="100px" runat="server">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField Visible="false" HeaderText="Commission">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtgvAgentCommission" onchange="SetSelectedRow(this)" ForeColor="#4d4c4c" runat="server"
                                                                                            Enabled='<%#IsAddAgentCommission(Eval("AgentCommission").ToString()) %>' Text='<%#Eval("AgentCommission") %>' />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtgvAgentCommission1" runat="server"
                                                                                            Enabled="True" TargetControlID="txtgvAgentCommission" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton ID="lnkStockInfo" runat="server" Text='<%#GetProductStock(Eval("Product_Id").ToString()) %>'
                                                                                            Font-Underline="true" ToolTip="View Detail" OnCommand="lnkStockInfo_Command"
                                                                                            ForeColor="Blue" CommandArgument='<%# Eval("Product_Id") %>'></asp:LinkButton>
                                                                                        <asp:Literal runat="server" ID="lit1" Text="<tr id='trGrid'><td colspan='20' align='right'>" />
                                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvchildGrid" runat="server" AutoGenerateColumns="false" DataKeyNames="Tax_Id" Visible="false">
                                                                                            <Columns>
                                                                                                <asp:TemplateField>
                                                                                                    <ItemTemplate>
                                                                                                        <asp:CheckBox ID="chkselecttax" runat="server" AutoPostBack="true" OnCheckedChanged="chkselecttax_OnCheckedChanged"
                                                                                                            Checked='<%#Eval("TaxSelected") %>' />
                                                                                                        <asp:HiddenField ID="hdntaxId" runat="server" Value='<%#Eval("Tax_Id") %>' />
                                                                                                        <asp:HiddenField ID="hdnCategoryId" runat="server" Value='<%#Eval("ProductCategoryId") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Category Name">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvcategoryName" runat="server" Visible="true" Text='<%#Eval("CategoryName") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Tax Name">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvtaxName" runat="server" Text='<%#Eval("TaxName") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Tax(%)">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="txttaxPerchild" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                                                            OnTextChanged="txttaxPerchild_OnTextChanged" Enabled='<%#Eval("TaxSelected") %>'
                                                                                                            Text='<%#Eval("Tax_Per") %>'></asp:TextBox>
                                                                                                        <cc1:FilteredTextBoxExtender ID="filtertextboxtaxperchild" runat="server" Enabled="True"
                                                                                                            TargetControlID="txttaxPerchild" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Tax Value">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="txttaxValuechild" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                                                            Enabled='<%#Eval("TaxSelected") %>' OnTextChanged="txttaxValuechild_OnTextChanged"
                                                                                                            Text='<%#Eval("Tax_value") %>'></asp:TextBox>
                                                                                                        <cc1:FilteredTextBoxExtender ID="filtertextbox12taxvaluechild" runat="server" Enabled="True"
                                                                                                            TargetControlID="txttaxValuechild" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                            </Columns>

                                                                                            <PagerStyle CssClass="pagination-ys" />
                                                                                        </asp:GridView>
                                                                                        <asp:Literal runat="server" ID="lit2" Text="</td></tr>" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                            </Columns>

                                                                            <PagerStyle CssClass="pagination-ys" />

                                                                        </asp:GridView>
                                                                        <asp:HiddenField ID="hdnProductId" runat="server" />
                                                                        <asp:HiddenField ID="hdnProductName" runat="server" />
                                                                    </div>
                                                                    <br />
                                                                </div>
                                                            </asp:Panel>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblAmount" runat="server" Text="<%$ Resources:Attendance,Gross Total %>" />
                                                                <asp:TextBox ID="txtAmount" runat="server" ReadOnly="true" CssClass="form-control"
                                                                    OnTextChanged="txtAmount_TextChanged" AutoPostBack="true" />
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblDiscountP" runat="server" Text="<%$ Resources:Attendance,Discount(%) %>" />
                                                                <div class="input-group">
                                                                    <asp:TextBox ID="txtDiscountP" runat="server" CssClass="form-control"
                                                                        OnTextChanged="txtDiscountP_TextChanged" AutoPostBack="true" />
                                                                    <cc1:FilteredTextBoxExtender ID="filtertextbox13" runat="server" Enabled="True" TargetControlID="txtDiscountP"
                                                                        ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <div class="input-group-addon">
                                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Value %>" />
                                                                    </div>
                                                                    <div class="input-group-btn">
                                                                        <asp:TextBox ID="txtDiscountV" runat="server" Width="120px" CssClass="form-control"
                                                                            OnTextChanged="txtDiscountV_TextChanged" AutoPostBack="true" />
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" Enabled="True"
                                                                            TargetControlID="txtDiscountV" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </div>
                                                                </div>
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblafterDiscountPrice" runat="server" Text="<%$ Resources:Attendance,Price After Discount %>"
                                                                    Visible="false" />
                                                                <asp:TextBox ID="txtPriceAfterDiscount" runat="server" CssClass="form-control" ReadOnly="True"
                                                                    Visible="false" />
                                                                <br />
                                                            </div>
                                                            <div class="col-md-12" style="display: none">
                                                                <div style="overflow: auto; max-height: 500px;">
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
                                                                                    <a style="color: Red">*</a>
                                                                                    <cc1:AutoCompleteExtender ID="autoComplete122566" runat="server" DelimiterCharacters=""
                                                                                        Enabled="True" ServiceMethod="GetCompletionListTax" ServicePath="" CompletionInterval="100"
                                                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtTaxName" UseContextKey="True"
                                                                                        CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                    </cc1:AutoCompleteExtender>
                                                                                </EditItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:TextBox ID="txtTaxFooter" runat="server" Font-Names="Verdana" AutoPostBack="true"
                                                                                        OnTextChanged="txtTaxFooter_TextChanged" CssClass="form-control" BackColor="#eeeeee"
                                                                                        CausesValidation="true"></asp:TextBox>
                                                                                    <cc1:AutoCompleteExtender ID="autoComplete12256660" runat="server" DelimiterCharacters=""
                                                                                        Enabled="True" ServiceMethod="GetCompletionListTax" ServicePath="" CompletionInterval="100"
                                                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtTaxFooter"
                                                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                    </cc1:AutoCompleteExtender>
                                                                                </FooterTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Tax(%)">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblTaxper" runat="server" Text='<%#Eval("Tax_Per") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:TextBox ID="txtTaxper" runat="server" Font-Names="Verdana" CssClass="form-control"
                                                                                        Text='<%#Eval("Tax_Per") %>' CausesValidation="true" AutoPostBack="false"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FiltergvSalesQuantity11488taxper" runat="server"
                                                                                        Enabled="True" TargetControlID="txtTaxper" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                </EditItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:TextBox ID="txtTaxperFooter" runat="server" Font-Names="Verdana" CssClass="form-control"
                                                                                        Text='<%#Eval("Tax_per") %>' CausesValidation="true" AutoPostBack="true" OnTextChanged="txtTaxperFooter_OnTextChanged"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FiltergvSalesQuantity2taxper" runat="server" Enabled="True"
                                                                                        TargetControlID="txtTaxperFooter" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                </FooterTemplate>
                                                                                <ItemStyle />
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
                                                                                        Text='<%#Eval("Tax_Value") %>' CausesValidation="true" AutoPostBack="true" OnTextChanged="txtTaxValueFooter_OnTextChanged"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FiltergvSalesQuantity2" runat="server" Enabled="True"
                                                                                        TargetControlID="txtTaxValueFooter" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                </FooterTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <EditItemTemplate>
                                                                                    <asp:Button ID="ButtonUpdate" runat="server" CommandName="Update" Text="Update" CausesValidation="true"
                                                                                        CommandArgument='<%#Eval("Tax_Id") %>' />
                                                                                    <asp:Button ID="ButtonCancel" runat="server" CommandName="Cancel" Text="Cancel" />
                                                                                </EditItemTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Button ID="ButtonEdit" runat="server" CommandName="Edit" Text="Edit" Visible="false" />
                                                                                    <asp:Button ID="ButtonDelete" runat="server" CommandName="Delete" Text="Delete" CommandArgument='<%#Eval("Tax_Id") %>' />
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:Button ID="ButtonAdd" runat="server" CommandName="AddNew" Text="Add New Row" />
                                                                                </FooterTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>

                                                                        <PagerStyle CssClass="pagination-ys" />

                                                                    </asp:GridView>
                                                                </div>
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblPriceAfterTax" runat="server" Text="<%$ Resources:Attendance,Price After Tax %>"
                                                                    Visible="false" />
                                                                <asp:TextBox ID="txtPriceAfterTax" runat="server" CssClass="form-control" ReadOnly="True"
                                                                    Visible="false" />
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblTaxP" runat="server" Text="<%$ Resources:Attendance,Tax(%) %>" />
                                                                <asp:TextBox ID="txtTaxP" runat="server" CssClass="form-control" OnTextChanged="txtTaxP_TextChanged"
                                                                    AutoPostBack="true" Enabled="false" />
                                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Value %>" />
                                                                <asp:TextBox ID="txtTaxV" runat="server" CssClass="form-control" OnTextChanged="txtTaxV_TextChanged"
                                                                    AutoPostBack="true" Enabled="false" />
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" Enabled="True"
                                                                    TargetControlID="txtTaxP" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                </cc1:FilteredTextBoxExtender>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" Enabled="True"
                                                                    TargetControlID="txtTaxV" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                </cc1:FilteredTextBoxExtender>

                                                                <br />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblTotalAmount" runat="server" Text="<%$ Resources:Attendance, Net Price %>" />
                                                                <asp:TextBox ID="txtTotalAmount" runat="server" CssClass="form-control" ReadOnly="True" />
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblShippingCharge" runat="server" Text="<%$ Resources:Attendance,Shipping Charge %>" />
                                                                <asp:TextBox ID="txtShippingCharge" runat="server" CssClass="form-control" OnTextChanged="txtShippingCharge_TextChanged"
                                                                    AutoPostBack="true" />
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" Enabled="True"
                                                                    TargetControlID="txtShippingCharge" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                </cc1:FilteredTextBoxExtender>
                                                                <br />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblNetAmount" runat="server" Text="<%$ Resources:Attendance,Net Amount %>" />
                                                                <asp:TextBox ID="txtNetAmount" runat="server" CssClass="form-control" ReadOnly="true" />
                                                                <br />
                                                            </div>

                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                    <div class="col-md-12" id="ctlRemark" runat="server">
                                                        <asp:Label ID="lblRemark" runat="server" Text="<%$ Resources:Attendance,Remark%>" />
                                                        <asp:TextBox ID="txtRemark" runat="server" CssClass="form-control" TextMode="MultiLine" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" CssClass="ajax__tab_yuitabview-theme">
                                                            <cc1:TabPanel ID="TabProductPaymentMode" runat="server" HeaderText="Advance Payment">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_TabProductPaymentMode" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,Payment Mode %>"></asp:Label>
                                                                                        <asp:DropDownList ID="ddlTabPaymentMode" runat="server" CssClass="form-control"
                                                                                            AutoPostBack="True" OnSelectedIndexChanged="ddlPaymentMode_SelectedIndexChanged">
                                                                                        </asp:DropDownList>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblPayAmmount" runat="server" Text="<%$ Resources:Attendance,Balance Amount%>"></asp:Label>
                                                                                        <asp:TextBox ID="txtPayAmount" runat="server" CssClass="form-control" OnTextChanged="txtPayAmount_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                                                            TargetControlID="txtPayAmount" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                </div>
                                                                                <div id="pnlpaybank" runat="server" class="col-md-12">
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblPayAccountNo" runat="server" Text="<%$ Resources:Attendance,Account No. %>"></asp:Label>
                                                                                        <asp:TextBox ID="txtPayAccountNo" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                                            OnTextChanged="txtPayAccountNo_TextChanged" BackColor="#eeeeee"></asp:TextBox>
                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                                                                            Enabled="True" ServiceMethod="GetCompletionListAccountNo" ServicePath="" CompletionInterval="100"
                                                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtPayAccountNo"
                                                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                        </cc1:AutoCompleteExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblPayBank" runat="server" Text="<%$ Resources:Attendance,Bank %>"
                                                                                            Visible="false"></asp:Label>
                                                                                        <asp:DropDownList ID="ddlPayBank" runat="server" CssClass="form-control" Visible="false">
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
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Attendance,Exchange Rate %>"></asp:Label>
                                                                                        <asp:TextBox ID="txtExchangerate" runat="server" CssClass="form-control" Text="1" Enabled="false"></asp:TextBox>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="Label16" runat="server" Text="Local Amount"></asp:Label>
                                                                                        <asp:TextBox ID="txtLocalAmount" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                                                        <br />
                                                                                    </div>

                                                                                    <div class="col-md-12" style="text-align: center">
                                                                                        <asp:Button ID="Btn_Payment_Save" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Add %>" OnClick="btnPaymentSave_Click" />
                                                                                        <br />
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <div style="overflow: auto; max-height: 500px;">
                                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvPayment" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                                            ShowFooter="true" BorderStyle="Solid" Width="100%" PageSize="<%# PageControlCommon.GetPageSize() %>">
                                                                                            <Columns>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:LinkButton ID="IbtnDeletePay" runat="server" CausesValidation="False" CommandArgument='<%# Eval("TransId") %>'
                                                                                                            ToolTip="<%$ Resources:Attendance,Delete %>" CssClass="fa fa-trash"
                                                                                                            OnCommand="btnDeletePay_Command" />
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
                                                                                                        <asp:Label ID="lbltotExp" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Amount%> " />
                                                                                                    </FooterTemplate>
                                                                                                    <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Net Price %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("FCPayAmount").ToString() %>' />
                                                                                                        <asp:Label ID="hidExpCur" runat="server" Visible="false" Text='<%# Eval("PayCurrencyID").ToString() %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="txttotAmount" runat="server" Font-Bold="true" Text="0" />
                                                                                                    </FooterTemplate>
                                                                                                    <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                                </asp:TemplateField>

                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Exchange Rate %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvExpExchangeRate" runat="server" Text='<%# Eval("PayExchangeRate") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Payment Amount (Local)" SortExpression="Pay_Charges">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvExp_Charges" runat="server" Text='<%# Eval("Pay_Charges") %>' />
                                                                                                    </ItemTemplate>

                                                                                                    <FooterStyle BorderStyle="None" />
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                                </asp:TemplateField>
                                                                                            </Columns>
                                                                                            <PagerStyle CssClass="pagination-ys" />
                                                                                        </asp:GridView>
                                                                                    </div>
                                                                                    <br />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="GvSalesOrder" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_TabProductPaymentMode">
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
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lbldeliveryVoucher" runat="server" Text="Create Delivery Voucher After Sales Invoice"
                                                            Visible="false" />
                                                        <asp:DropDownList ID="ddlDeliveryvoucher" runat="server" CssClass="form-control" Visible="false">
                                                            <asp:ListItem Text="False" Value="False"></asp:ListItem>
                                                            <asp:ListItem Text="True" Value="True"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <br />
                                                        <asp:CheckBox ID="chksendInproduction" runat="server" Text="Send In Production" Visible="false" />
                                                        <asp:CheckBox ID="chkSendInPO" runat="server" Style="margin-left: 20px;" Text="<%$Resources:Attendance, Send In Purchase Order%>" Visible="false" />
                                                        <asp:CheckBox ID="chkSendInProjectManagement" Style="margin-left: 20px;" runat="server" Text="Send In Project Management" />
                                                        <asp:CheckBox ID="chkPartialShipment" runat="server" Style="margin-left: 20px;" Text="<%$Resources:Attendance,Partial Shipment%>" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <br />
                                                    </div>
                                                    <asp:UpdatePanel ID="Update_Save" runat="server">
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnSOrderSave" />
                                                        </Triggers>
                                                        <ContentTemplate>
                                                            <div class="col-md-12" style="text-align: left">

                                                                <asp:HiddenField ID="Hdn_Edit_ID" runat="server" />
                                                                <asp:Button ID="btnSOrderSave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                                    CssClass="btn btn-success" OnClientClick="Confirm()" OnClick="btnSOrderSave_Click" Visible="false" />
                                                                <asp:Button ID="BtnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                                    CssClass="btn btn-primary" CausesValidation="False" OnClick="BtnReset_Click" />
                                                                <asp:Button ID="btnSOrderCancel" runat="server" CssClass="btn btn-danger" Text="<%$ Resources:Attendance,Cancel %>"
                                                                    CausesValidation="False" OnClick="btnSOrderCancel_Click" />
                                                                <asp:HiddenField ID="editid" runat="server" />

                                                            </div>

                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>




                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <asp:HiddenField ID="hdnSalesQuotationId" runat="server" Value="0" />
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                    <div class="tab-pane" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div1" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label7" runat="server" Text="Advance Search"></asp:Label></h3>
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
                                                    <asp:DropDownList ID="ddlFieldNameBin" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlFieldNameBin_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Order No. %>" Value="SalesOrderNo"
                                                            Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Order Date %>" Value="SalesOrderDate"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Customer Name%>" Value="CustomerName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Transfer Type %>" Value="TransType"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Transfer No. %>" Value="QauotationNo"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Payment Mode %>" Value="PaymentModeName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="CreatedUser"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Modified By %>" Value="ModifiedUser"></asp:ListItem>
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
                                                <div class="col-lg-4">
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbindBin">
                                                        <asp:TextBox ID="txtValueBin" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                                        <asp:TextBox ID="txtValueBinDate" runat="server" CssClass="form-control" placeholder="Search From Date" Visible="false"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueBinDate" runat="server" TargetControlID="txtValueBinDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="btnbindBin" runat="server" CausesValidation="False" OnClick="btnbindBin_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="btnRefreshBin" runat="server" CausesValidation="False" OnClick="btnRefreshBin_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="imgBtnRestore" CausesValidation="False" runat="server" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="ImgbtnSelectAll" Visible="false" runat="server" OnClick="ImgbtnSelectAll_Click" ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>

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
                                                    <asp:UpdatePanel ID="updPnlBin" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>

                                                            <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvSalesOrderBin" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvSalesOrderBin_PageIndexChanging"
                                                                OnSorting="GvSalesOrderBin_OnSorting" AllowSorting="true">
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
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order No. %>" SortExpression="SalesOrderNo">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvSONo" runat="server" Text='<%#Eval("SalesOrderNo") %>' />
                                                                            <asp:HiddenField ID="hdnTransId" runat="server" Value='<%#Eval("Trans_Id") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order Date %>" SortExpression="SalesOrderDate">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvSODate" runat="server" Text='<%#GetDate(Eval("SalesOrderDate").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Name %>" SortExpression="CustomerName">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvCustomer" runat="server" Text='<%#Eval("CustomerName") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Transfer Type%>" SortExpression="TransType">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvTransType" runat="server" Text='<%#Eval("TransType") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Transfer No.%>" SortExpression="QauotationNo">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvTransNo" runat="server" Text='<%#Eval("QauotationNo") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Payment Mode%>" SortExpression="PaymentModeName">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPaymentname" runat="server" Text='<%# Eval("PaymentModeName").ToString() %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="CreatedUser">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvUser" runat="server" Text='<%#Eval("CreatedUser") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>" SortExpression="ModifiedUser">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblModified_User" runat="server" Text='<%# Eval("ModifiedUser") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Amount%>" SortExpression="NetAmount">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblnetAmount" runat="server" Text='<%# GetCurrencySymbol(Eval("NetAmount").ToString(),Eval("Currency_Id").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                </Columns>


                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                            <asp:HiddenField ID="HDFSortbin" runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="Btn_Bin" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
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
                            <ContentTemplate>


                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div2" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label8" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
                                                   <asp:Label ID="lblTotalRecordsQuote" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I3" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">


                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldNameQuote" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFieldNameQuote_SelectedIndexChanged">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Quotation Id %>" Value="SQuotation_Id"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Quotation No. %>" Value="SQuotation_No"
                                                            Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Quotation Date %>" Value="Quotation_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Customer Name %>" Value="Customer_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="EmployeeName"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlOptionQuote" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-5">
                                                    <asp:Panel ID="Panel3" runat="server" DefaultButton="btnbindQuote">
                                                        <asp:TextBox ID="txtValueQuote" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                                        <asp:TextBox ID="txtValueQuoteDate" runat="server" CssClass="form-control" placeholder="Search From Date" Visible="false"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueQuoteDate" runat="server" TargetControlID="txtValueQuoteDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbindQuote" runat="server" CausesValidation="False" OnClick="btnbindrptQuote_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="btnRefreshQuote" runat="server" CausesValidation="False" OnClick="btnRefreshQuoteReport_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>


                                <div class="box box-warning box-solid">
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:UpdatePanel ID="UpdatePanel_sales" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <div class="flow">
                                                            <asp:HiddenField ID="Hdn_Tax_By" runat="server" />
                                                            <asp:HiddenField ID="HDN_Sales_Quotation_ID" runat="server" />
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvSalesQuotation" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvSalesQuotation_PageIndexChanging"
                                                                AllowSorting="True" OnSorting="GvSalesQuotation_Sorting" CurrentSortField="SQuotation_Id" CurrentSortDirection="DESC">

                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="btnEdit" runat="server" BackColor="Transparent" BorderStyle="None" TabIndex="76" CausesValidation="False" CommandName='<%#Eval("Location_Id") %>' CommandArgument='<%# Eval("SQuotation_Id") + "," + Eval("Customer_id")  %>' CssClass="btn fa fa-angle-left" OnCommand="btnSIEdit_Command"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quotation Id %>" SortExpression="SQuotation_Id">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvSQuotationId" runat="server" Text='<%#Eval("SQuotation_Id") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quotation No. %>" SortExpression="SQuotation_No">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvSINo" runat="server" Text='<%#Eval("SQuotation_No") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quotation Date %>" SortExpression="Quotation_Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvQuotationDate" runat="server" Text='<%#GetDate(Eval("Quotation_Date").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Employee %>" SortExpression="EmployeeName">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvCustomerName" runat="server" Text='<%#Eval("EmployeeName") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Customer Name %>" SortExpression="Customer_Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvCustomerName1" runat="server" Text='<%# Eval("Customer_Name") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Amount %>" SortExpression="TotalAmount">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvamtId" runat="server" Text='<%#SystemParameter.GetAmountWithDecimal(Eval("TotalAmount").ToString(),Eval("CurrencyDecimalCount").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                </Columns>

                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>

                                                            <asp:HiddenField ID="hdnGvSqCurrentPageIndex" Value="1" runat="server" />
                                                        </div>
                                                        <br />

                                                        <asp:Repeater ID="rptGvSqPager" runat="server">
                                                            <ItemTemplate>
                                                                <ul class="pagination">
                                                                    <li class='<%# Convert.ToBoolean(Eval("Enabled")) ? "page-item" : "page-item active" %>'>
                                                                        <asp:LinkButton ID="lnkGvSqPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                                                            CssClass="page-link"
                                                                            OnClick="GvSq_Page_Changed" OnClientClick='<%# !Convert.ToBoolean(Eval("Enabled")) ? "return false;" : "" %>'></asp:LinkButton>
                                                                    </li>
                                                                </ul>
                                                            </ItemTemplate>
                                                        </asp:Repeater>

                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="Btn_Quotation" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                                <asp:UpdateProgress ID="UpdateProgress13" runat="server" AssociatedUpdatePanelID="UpdatePanel_sales">
                                                    <ProgressTemplate>
                                                        <div class="modal_Progress">
                                                            <div class="center_Progress">
                                                                <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                            </div>
                                                        </div>
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
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


    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_Report">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


    <div class="modal fade" id="creditTermsNConditions" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h3 class="box-title">
                        <asp:Label ID="Label22" Font-Names="Times New roman" Font-Size="18px" Font-Bold="true" runat="server" Text="Credit Terms & Condition"></asp:Label>
                    </h3>
                </div>
                <div class="modal-body">
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
                        <div class="col-md-6">
                            <asp:Label ID="Label47" runat="server" Font-Bold="true" Text="Credit Parameter" />
                            &nbsp:&nbsp<asp:Label ID="lblCreditParameterValue" runat="server"></asp:Label>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="Label11" runat="server" Font-Bold="true" Text="Current Balance" />
                            &nbsp:&nbsp<asp:Label ID="lblCustomercurrentBalance" runat="server"></asp:Label>
                            <br />
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

    <asp:Panel ID="pnlorderreport" runat="server" Visible="false">
        <dx:ReportToolbar ID="rptToolBar" runat="server" ShowDefaultButtons="False" ReportViewer="<%# rptViewer %>"
            Width="100%" AccessibilityCompliant="True">
            <Items>
                <dx:ReportToolbarButton ItemKind="Search" />
                <dx:ReportToolbarSeparator />
                <dx:ReportToolbarButton ItemKind="PrintReport" />
                <dx:ReportToolbarButton ItemKind="PrintPage" />
                <dx:ReportToolbarSeparator />
                <dx:ReportToolbarButton Enabled="False" ItemKind="FirstPage" />
                <dx:ReportToolbarButton Enabled="False" ItemKind="PreviousPage" />
                <dx:ReportToolbarLabel ItemKind="PageLabel" />
                <dx:ReportToolbarComboBox ItemKind="PageNumber" Width="65px">
                </dx:ReportToolbarComboBox>
                <dx:ReportToolbarLabel ItemKind="OfLabel" />
                <dx:ReportToolbarTextBox IsReadOnly="True" ItemKind="PageCount" />
                <dx:ReportToolbarButton ItemKind="NextPage" />
                <dx:ReportToolbarButton ItemKind="LastPage" />
                <dx:ReportToolbarSeparator />
                <dx:ReportToolbarButton ItemKind="SaveToDisk" />
                <dx:ReportToolbarButton ItemKind="SaveToWindow" />
                <dx:ReportToolbarComboBox ItemKind="SaveFormat" Width="70px">
                    <Elements>
                        <dx:ListElement Value="pdf" />
                        <dx:ListElement Value="xls" />
                        <dx:ListElement Value="xlsx" />
                        <dx:ListElement Value="rtf" />
                        <dx:ListElement Value="mht" />
                        <dx:ListElement Value="html" />
                        <dx:ListElement Value="txt" />
                        <dx:ListElement Value="csv" />
                        <dx:ListElement Value="png" />
                    </Elements>
                </dx:ReportToolbarComboBox>
            </Items>
            <Styles>
                <LabelStyle>
                    <Margins MarginLeft="3px" MarginRight="3px" />
                </LabelStyle>
            </Styles>
        </dx:ReportToolbar>
        <dx:ReportViewer ID="rptViewer" runat="server">
        </dx:ReportViewer>
    </asp:Panel>

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
                    <asp:UpdatePanel ID="Update_Modal_GST" runat="server" UpdateMode="Conditional">
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
                    <asp:UpdatePanel ID="Update_Modal_Button_GST" runat="server" UpdateMode="Conditional">
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

    <asp:UpdateProgress ID="UpdateProgress14" runat="server" AssociatedUpdatePanelID="Update_Modal_Button_GST">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress15" runat="server" AssociatedUpdatePanelID="Update_Modal_GST">
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

    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Quotation">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress12" runat="server" AssociatedUpdatePanelID="Update_Save">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
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
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
    <script src="../Script/ReportSystem.js"></script>
    <script src="../Script/master.js"></script>
    <script type="text/javascript">


        function getReportDataWithLoc(transId, locId) {
            $("#prgBar").css("display", "block");
            document.getElementById('<%= reportSystem.FindControl("hdnTransId").ClientID %>').value = transId;
            document.getElementById('<%= reportSystem.FindControl("hdnLocId").ClientID %>').value = locId;
            setReportData();
        }

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
            Li_Tab_New();
        }
        function LI_List_Active() {
            $("#Li_List").addClass("active");
            $("#List").addClass("active");

            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");
        }

        function LI_List_Quotation_Active() {
            $("#Li_Quotation").removeClass("active");
            $("#Quotation").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
            Li_Tab_New();
        }

        function Li_List_Hide() {

        }
        function Li_Bin_Hide() {

        }
        function Li_Quotation_Hide() {

        }

        //function Li_List_Show() {

        //}
        //function Li_Bin_Show() {

        //}
        //function Li_Quotation_Show() {

        //}

        function Li_Tab_New() {
            var Lbl_Tab_New = document.getElementById('<%= Lbl_Tab_New.ClientID %>');
              var btnSOrderSave = document.getElementById('<%= btnSOrderSave.ClientID %>');
            if (Lbl_Tab_New.innerText == "View") {
                btnSOrderSave.disabled = true;
            }
            else {
                btnSOrderSave.disabled = false;
            }

        }
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }
        function Li_Tab_Quotation() {
            document.getElementById('<%= Btn_Quotation.ClientID %>').click();
        }
    </script>
    <script type="text/javascript">
        function Confirm() {
            debugger;
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do you want to send order acknowledgement email ?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
        function divexpandcollapse(divname) {
            var img = "img" + divname;
            if ($("#" + img).attr("src") == "../Images/plus.png") {
                $("#" + img)
                    .closest("tr")
                    .after("<tr><td></td><td colspan = '100%'>" + $("#" + divname)
                        .html() + "</td></tr>");
                $("#" + img).attr("src", "../Images/minus.png");
            } else {
                $("#" + img).closest("tr").next().remove();
                $("#" + img).attr("src", "../Images/plus.png");
            }
        }
    </script>
    <script type="text/javascript">

        function Show_Modal_GST() {
            document.getElementById('<%= Btn_GST_Modal.ClientID %>').click();
        }
        function Modal_CustomerInfo_Open() {
            document.getElementById('<%= Btn_CustomerInfo_Modal.ClientID %>').click();
        }
        function Modal_Open_FileUpload() {
            document.getElementById('<%= Btn_Modal_FileUpload.ClientID %>').click();
        }
        function Modal_NewAddress_Open() {
            document.getElementById('<%= Btn_NewAddress.ClientID %>').click();
        }
        function Modal_AcMaster_Open() {
            var _result = confirm("Account not exist in " + $('#<%= ddlCurrency.ClientID %> option:selected').text() + ". Do you want to create it");
            if (_result == true) {
                $('#ModelAcMaster').modal('show');
            }
        }

        function GetCreditInfo() {
            try {
                var id = getCustomerId();
                if (id == "") {
                    throw "Please enter customer name";
                }
                var currency = $('#<%= ddlCurrency.ClientID %>').val();
                if (currency == "") {
                    throw "Please select currency";
                }

                $.ajax({
                    url: '../WebServices/customer.asmx/getCreditInfo',
                    method: 'post',
                    contentType: "application/json; charset=utf-8",
                    data: "{'strCustomerId':'" + id + "','strCurrencyId':'" + currency + "'}",
                    async: true,
                    success: function (data) {
                        debugger;
                        var data = data.d;
                        if (data == null) {
                            showAlert('Not found any credit information', 'red', 'white');
                        }
                        else {

                            $('#<%= lblCreditLimitValue.ClientID %>').text(data.split(',')[0]);
                            $('#<%= lblCreditDaysValue.ClientID %>').text(data.split(',')[1]);
                            $('#<%= lblCreditParameterValue.ClientID %>').text(data.split(',')[2]);
                            $('#<%= lblCustomercurrentBalance.ClientID %>').text(data.split(',')[3]);
                            $('#creditTermsNConditions').modal('show')
                        }

                    },
                    error: function (ex) {
                        alert(ex);
                    }
                });
            }
            catch (ex) {
                alert(ex);
            }

        }
        function errorMessages(ex) {
            alert(ex);
        }
        function getCustomerId() {
            var ctl = $("#<%= txtCustomer.ClientID %>").val();
            if (ctl == "") {
                return "";
            }
            return ctl.split('/')[3];
        }
        function lnkcustomerHistory_OnClick() {
            var id = getCustomerId();
            if (id == "") {
                showAlert('Please enter customer name', 'orange', 'white');
                return;
            }
            window.open('../Purchase/CustomerHistory.aspx?ContactId=' + id + '&&Page=SINV', 'window', 'width=1024, ');
        }
        function btnAddCustomer_OnClick() {
            window.open('../Sales/AddContact.aspx?Page=SINV', 'window', 'width=1024');
        }
        function txtCustomer_TextChanged(ctl) {
            try {
                debugger;
                if (ctl.value == "") {
                    resetCustomerIdForContact();
                    return;
                }

                validateCustomer(ctl);
                document.getElementById('<%=txtContactPerson.ClientID %>').value = "";

                var data = getCustomerAddress(ctl);
                if (data[0] == "false") {
                    alert(data[1]);
                    ctl.value = "";
                    ctl.focus();
                }
                else {
                    if (ctl.id == "<%= txtCustomer.ClientID %>") {
                        $('#<%= txtShipingAddress.ClientID %>').val(data[1]);
                        $('#<%= txtInvoiceTo.ClientID %>').val(data[1]);
                        $('#<%= txtShipCustomerName.ClientID %>').val(ctl.value);
                    }
                    else {
                        $('#<%= txtShipingAddress.ClientID %>').val(data[1]);
                    }

                }
                InvoiceAddressDetail();
                ShipToAddressDetail();
                ShipingAddressDetail();
            }
            catch (ex) {
                showAlert(ex, 'orange', 'white');
            }
        }

        function AddressDetailFunctions() {
            InvoiceAddressDetail();
            ShipToAddressDetail();
            ShipingAddressDetail();
        }
        function InvoiceAddressDetail() {
            $.ajax({
                url: 'SalesOrder1.aspx/GetInvoiceAddressDetail',
                method: 'post',
                contentType: "application/json; charset=utf-8",
                data: "{'InvAddrees':'" + $('#<%= txtInvoiceTo.ClientID %>').val() + "'}",
                async: true,
                success: function (data) {
                    //console.log("rahul");
                    if (data.d != 0) {
                        $('#<%= InvoiceAddress.ClientID %>').text(data.d);
                    }

                    //var taxData = data.d;
                    //console.log(taxData);


                },
                error: function (ex) {

                }
            });
        }
        function ShipToAddressDetail() {
            $.ajax({
                url: 'SalesOrder1.aspx/GetShipToAddress',
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
        }
        function ShipingAddressDetail() {
            $.ajax({
                url: 'SalesOrder1.aspx/GetShippingAddress',
                method: 'post',
                contentType: "application/json; charset=utf-8",
                data: "{'ShippingAddress':'" + $('#<%= txtShipingAddress.ClientID %>').val() + "' }",
                async: true,
                success: function (data) {
                    if (data.d != 0) {
                        $('#<%= ShippingAddress.ClientID %>').text(data.d);
                    }

                },
                error: function (ex) {

                }
            });
        }


        function reCalculateGvProductLineTotal(ctl) {
            var row = $(ctl.parentNode.parentNode);
            var decimalCount = document.getElementById('<%= hdnDecimalCount.ClientID%>').value;
            if (decimalCount == undefined || decimalCount == "") {
                decimalCount = 2;
            }
            var discountPer = 0, discountValue = 0, taxPer = 0, taxValue = 0;

            //getting values of sales price and sales quantity
            var unitPrice = $(row).find("input[id*=lblgvUnitPrice]").val();
            var salesQty = $(row).find("input[id*=lblgvQuantity]").val();

            //setting the decimal (if user uses more values in decimal count)
            unitPrice = GetAmountWithDecimal(unitPrice, decimalCount);
            salesQty = GetAmountWithDecimal(salesQty, decimalCount);

            //getting the value again for calculation to avoide decimal count issue
            $(row).find("input[id*=lblgvUnitPrice]").val(unitPrice);
            $(row).find("input[id*=lblgvQuantity]").val(salesQty);

            $(row).find("span[id*=lblgvQuantityPrice]").html(GetAmountWithDecimal((unitPrice * salesQty), decimalCount));

            if ($('#<%= hdnIsDiscountEnable.ClientID %>').val() == "true") {
                var discountPer = $(row).find("input[id*=lblgvDiscountP]").val();
                var discountValue = (unitPrice * discountPer) / 100;
                discountValue = GetAmountWithDecimal(discountValue, decimalCount);
                $(row).find("input[id*=lblgvDiscountV]").val(discountValue);
            }
            <%--if ($('#<%= hdnIsTaxEnabled.ClientID %>').val() == "true") {
                var taxPer = $(row).find("td:eq(16) label[id='tblLblTaxPer']").text();
                var taxValue = ((unitPrice - discountValue) * taxPer) / 100;
                $(row).find('td:eq(17)').html(GetAmountWithDecimal(taxValue, 3));
            }--%>
            var lineTotal = (unitPrice - discountValue + taxValue) * salesQty;
            $(row).find("span[id*=lblgvTotal]").html(GetAmountWithDecimal(lineTotal, decimalCount));
            reCalculateHeader();
        }

        function reCalculateHeader() {
            var grossAmount = 0.000;
            var totalDiscount = 0.000;
            var totalTax = 0.000;
            var netAmount = 0.000;
            var taxPer = 0.000;
            var disPer = 0.000;
            var decimalCount = document.getElementById('<%= hdnDecimalCount.ClientID%>').value;
            if (decimalCount == undefined || decimalCount == "") {
                decimalCount = 2;
            }

            var gv = $("#<%= GvProductDetail.ClientID %>");
            $(gv).find('tr.Invgridrow').each(function () {
                var row = $(this);
                grossAmount = grossAmount + parseFloat($(row).find("span[id*=lblgvQuantityPrice]").html()); //Gross Amount


                if ($(row).find("input[id*=lblgvDiscountP]").val() != undefined) {
                    totalDiscount = totalDiscount + parseFloat($(row).find("input[id*=lblgvDiscountV]").val()); // Discount %
                }

                if ($(row).find("input[id*=lblgvTaxP]").val() != undefined) {
                    totalTax = totalTax + parseFloat($(row).find("input[id*=lblgvTaxV]").val()); // tax %
                }

                netAmount = netAmount + parseFloat($(row).find("span[id*=lblgvTotal]").html());

            });


            if (totalDiscount > 0 && grossAmount > 0) {
                disPer = (totalDiscount * 100) / grossAmount;
            }

            if (totalTax > 0) {
                taxPer = totalTax * 100 / (grossAmount - totalDiscount)
            }
            $('#<%= txtAmount.ClientID %>').val(GetAmountWithDecimal(grossAmount, decimalCount));
            $('#<%= txtDiscountP.ClientID %> ').val(disPer.toFixed(2));
            $('#<%= txtDiscountV.ClientID %> ').val(GetAmountWithDecimal(totalDiscount, decimalCount));
            $('#<%= txtPriceAfterDiscount.ClientID %> ').val(GetAmountWithDecimal((grossAmount - totalDiscount), decimalCount));
            $('#<%= txtTaxP.ClientID %> ').val(taxPer.toFixed(2));
            $('#<%= txtTaxV.ClientID %> ').val(GetAmountWithDecimal(totalTax, decimalCount));
            $('#<%= txtTotalAmount.ClientID %> ').val(getRoundOffAmount(netAmount));
            if ($('#<%= txtShippingCharge.ClientID %> ').val() == "") {
                $('#<%= txtShippingCharge.ClientID %> ').val("0");
            }
            var totalExpenses = getRoundOffAmount($('#<%= txtShippingCharge.ClientID %> ').val());

            var netAmtWithExpenses = parseFloat(totalExpenses) + parseFloat($('#<%= txtTotalAmount.ClientID %> ').val());

            $('#<%= txtNetAmount.ClientID %> ').val(GetAmountWithDecimal(netAmtWithExpenses, decimalCount));
            $('#<%= txtNetAmount.ClientID %> ').trackChanges();
            $('#<%= txtNetAmount.ClientID %> ').commitChanges();
        }


        function displaymessage(ctl) {
            alert(ctl.value);
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
        function setScrollAndRow() {
            try {
                debugger;
                var rowIndex = $('#<%= hdfCurrentRow.ClientID %>').val();
                var parent = document.getElementById('<%= GvProductDetail.ClientID %>');
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
        function showUcControlsSettings() {
            $('#ControlSettingModal').modal('show');
        }
    </script>
    <script src="../Script/customer.js"></script>
    <script src="../Script/employee.js"></script>
    <script src="../Script/address.js"></script>
    <script src="../Script/common.js"></script>
</asp:Content>
