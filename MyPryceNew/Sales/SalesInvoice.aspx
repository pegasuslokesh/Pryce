<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="SalesInvoice.aspx.cs" Inherits="Sales_SalesInvoice" %>

<%@ Register Src="~/WebUserControl/Expenses_Tax.ascx" TagName="Expenses_Tax" TagPrefix="ET" %>
<%@ Register Src="~/WebUserControl/AddContact.ascx" TagName="Contactmaster" TagPrefix="UC" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/AccountMaster.ascx" TagPrefix="uc1" TagName="AccountMaster" %>

<%@ Register Src="~/WebUserControl/AddressControl.ascx" TagName="AddAddress" TagPrefix="AA1" %>
<%@ Register Src="~/WebUserControl/ReportSystem.ascx" TagName="ReportSystem" TagPrefix="RS" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/ContactInfo.ascx" TagName="ViewContact" TagPrefix="AT1" %>
<%@ Register Src="~/WebUserControl/ucControlsSetting.ascx" TagName="ucCtlSetting" TagPrefix="UC" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <style type="text/css">
        .pager {
            LI_Edit_Active background-color: #3AC0F2;
            color: white;
            height: 30px;
            min-width: 30px;
            line-height: 30px;
            display: block;
            text-align: center;
            text-decoration: none;
            border: 1px solid #2E99C1;
        }

        .active_pager {
            background-color: #2E99C1;
            color: white;
            height: 30px;
            min-width: 30px;
            line-height: 30px;
            display: block;
            text-align: center;
            text-decoration: none;
            border: 1px solid #2E99C1;
        }

        .grid td, .grid th {
            text-align: center;
        }

        tooltip-inner {
            max-width: none;
            white-space: nowrap;
        }
    </style>
    <script>


        function LI_Edit_Active() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }

        function Li_Import_Active() {
            $("#Li_Import").addClass("active");
            $("#import_invoice").addClass("active");
            document.getElementById('<%= Btn_Li_Import.ClientID %>').click();
        }


        function resetPosition1() {

        }

        <%--    window.onbeforeunload = closingCode;
        function closingCode() {
            debugger;
          <% Session.Abandon(); %>
            return null;
        }--%>

    </script>
    <link href="../CSS/controlsCss.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-file-invoice-dollar"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Sales Invoice %>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Sales%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Sales Invoice%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="btn_New" runat="server" Text="New" Visible="false" />
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnUnPost_Click" Text="Bin" />
            <asp:Button ID="Btn_Li_Import" Style="display: none;" runat="server" OnClick="Btn_Li_Import_Click" Text="Import" />
            <asp:Button ID="Btn_Pending_Order" Style="display: none;" runat="server" OnClick="btnPendingOrder_Click" Text="Pending Order" />
            <asp:Button ID="Btn_Pending_Job_Card" Style="display: none;" runat="server" OnClick="btnPendingjobCard_Click" Text="Job Card" />
            <asp:Button ID="Btn_View_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Return_Modal" Text="View Modal" />
            <asp:Button ID="Btn_GST_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Modal_GST" Text="GST" />
            <asp:Button ID="Btn_Show_Expenses_Tax" Style="display: none;" runat="server" data-toggle="modal" data-target="#Expenses_Tax_Web_Control" Text="Expenses Tax" />
            <asp:Button ID="Btn_Modal_Expenses_Tax" Style="display: none;" runat="server" data-toggle="modal" data-target="#Modal_Expenses_Tax" Text="Expenses Tax" />
            <asp:Button ID="Btn_NewAddress" Style="display: none;" runat="server" data-toggle="modal" data-target="#NewAddress" Text="New Address" />
           <asp:Button ID="Btn_ModalTransport" Style="display:none" runat="server" data-toggle="modal" data-target="#Modal_TransPort" Text="Transport" />
             <asp:HiddenField runat="server" ID="hdnCanPrint" />
            <asp:HiddenField runat="server" ID="hdnCanView" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
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
                    <li id="Li_Import"><a href="#import_invoice" onclick="Li_Tab_Import()" data-toggle="tab">
                        <i class="fa fa-upload"></i>&nbsp;&nbsp;<asp:Label ID="Label19" runat="server" Text="<%$ Resources:Attendance,Import %>"></asp:Label></a></li>
                    <li style="display: none" id="Li_Job_Card"><a href="#Job_Card" onclick="Li_Tab_Job_Card()" data-toggle="tab">
                        <i class="fa fa-file"></i>&nbsp;&nbsp;<asp:Label ID="Label29" runat="server" Text="<%$ Resources:Attendance,Job Card%>"></asp:Label></a></li>
                    <li id="Li_Pending_Order"><a href="#Pending_Order" onclick="Li_Tab_Pending_Order()" data-toggle="tab">
                        <i class="fas fa-user-check"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Pending Order %>"></asp:Label></a></li>
                    <li id="Li_Bin"><a href="#Bin" onclick="Li_Tab_Bin()" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label28" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
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
                        <asp:UpdatePanel ID="Update_List" runat="server">
                           
                            <ContentTemplate>


                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div1" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <asp:HiddenField ID="hdnTransType" runat="server" />
                                                <asp:HiddenField ID="hdnTransTypeValue" runat="server" />

                                                <div class="col-lg-12">
                                                    <asp:DropDownList ID="ddlLocation" runat="server" Class="form-control" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged"
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
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control" OnSelectedIndexChanged="SetCustomerTextBox" AutoPostBack="true">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Invoice Id %>" Value="Trans_Id"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Invoice No %>" Value="Invoice_No" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Invoice Date %>" Value="Invoice_Date"></asp:ListItem>
                                                        <asp:ListItem Text="Ref. Type" Value="Transtype"></asp:ListItem>
                                                        <asp:ListItem Text="Ref. No." Value="OrderList"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Sales Person %>" Value="Emp_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Customer Name %>" Value="Name"></asp:ListItem>
                                                        <asp:ListItem Text="Customer Order No" Value="ref_order_number"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Serial No %>" Value="SerialNo"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="InvoiceCreatedBy"></asp:ListItem>
                                                        <asp:ListItem Text="Net Amount" Value="GrandTotal"></asp:ListItem>
                                                        <asp:ListItem Text="Merchant Name" Value="Merchant_Name"></asp:ListItem>
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
                                                        <asp:TextBox ID="txtCustValue" runat="server" CssClass="form-control" Visible="false" placeholder="Search from Content"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                            Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="0"
                                                            ServiceMethod="GetCompletionListCustomer" ServicePath="" TargetControlID="txtCustValue"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <asp:TextBox ID="txtValueDate" runat="server" CssClass="form-control" Visible="false" placeholder="Search from Date"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueDate" runat="server" TargetControlID="txtValueDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="true" OnClick="btnbindrpt_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefreshReport_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnGvListSetting" ToolTip="List Settings" runat="server" OnClick="btnGvListSetting_Click" Visible="false"><span class="fas fa-wrench"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid" <%= GvSalesInvoice.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                Page Size :
                                                <asp:DropDownList ID="ddlPageSize" runat="server" OnSelectedIndexChanged="ddlPageSize_OnSelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                <br />
                                                <br />

                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover"  ID="GvSalesInvoice" runat="server" AutoGenerateColumns="False" Width="100%"
                                                        AllowPaging="false"
                                                        AllowSorting="true" OnSorting="GvSalesInvoice_Sorting" OnPageIndexChanging="GvSalesInvoice_PageIndexChanging">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnPrint" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="IbtnPrint_Command"><i class="fa fa-print"></i>Invoice</asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDeliveryPrint" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="IbtnDeliveryPrint_Command" Visible='<%# Eval("IsDeliveryPrint") %>'><i class="fa fa-print"></i>Print</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <a style="cursor: pointer" onclick="getReportData('<%# Eval("Trans_Id") %>')"><i class="fa fa-print"></i>Report System</a>
                                                                            </li>

                                                                            <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="lnkViewDetail" runat="server" CommandName='<%# Eval("Location_Id") %>' CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="lnkViewDetail_Command" CausesValidation="False"><i class="fa fa-eye"></i>View</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' CommandName='<%# Eval("Location_Id") %>' OnCommand="btnEdit_Command" CausesValidation="False"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>                                                                  
                                                                        </ul>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Invoice No. %>" SortExpression="Invoice_No">
                                                                <ItemTemplate>
                                                                    <asp:UpdatePanel ID="UpdateTransGrid" runat="server" >
                                                                        
                                                                        <ContentTemplate>
                                                                                                                                            <asp:LinkButton ID="IbtnTransport" runat="server" Text='<%# Eval("Invoice_No") %>'
                                                                                OnClientClick='<%# "SetSalesTransport(\"" + Eval("Invoice_No") + "\", \"" + Eval("Trans_Id") + "\"); return false;" %>'
                                                                                ToolTip="Transport">
                                                                            </asp:LinkButton>
                                                                        </ContentTemplate>

                                                                    </asp:UpdatePanel>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Invoice Date %>" SortExpression="Invoice_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvSInvDate" runat="server" Text='<%#GetDate(Eval("Invoice_Date").ToString())%>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Ref. Type" SortExpression="Transtype">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvRefType" runat="server" Text='<%#Eval("Transtype") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Ref. No." SortExpression="OrderList">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvOrderList" runat="server" Text='<%#Eval("OrderList") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Sales Person %>" SortExpression="EmployeeName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvSalesPerson" runat="server" Text='<%#Eval("EmployeeName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Name%>" SortExpression="CustomerName">
                                                                <ItemTemplate>
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td width="80%">
                                                                                <asp:Label ID="lblgvCustomerName" runat="server" Text='<%#Eval("CustomerName") %>' />
                                                                            </td>
                                                                            <td align="right" width="20%">
                                                                                <asp:LinkButton ID="lnkcustomerdetail" runat="server" Text="More.." ForeColor="Blue"
                                                                                    CommandArgument='<%# Eval("Customer_Id") %>' OnCommand="lblgvCustomerName_Command"
                                                                                    ToolTip="Customer History" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>
                                                                <FooterStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Customer Order No" SortExpression="Field4">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCustomerOrderNo" runat="server" Text='<%# Eval("ref_order_number").ToString() %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                           <asp:TemplateField HeaderText="Merchant_Name" SortExpression="Merchant_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblMerchant_Name" runat="server" Text='<%#Eval("Merchant_Name") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status%>" SortExpression="Field4">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblst" runat="server" Text='<%# Eval("Field4").ToString() %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By%>" SortExpression="InvoiceCreatedBy">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCreatedBy" runat="server" Text='<%#Eval("InvoiceCreatedBy")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Payment Mode" SortExpression="PaymentType">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvPaymentType" runat="server" Text='<%#Eval("PaymentType")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Amount%>" SortExpression="GrandTotal">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvGrandTotal1" runat="server" Text='<%# GetCurrencySymbol(Eval("GrandTotal").ToString(),Eval("Currency_Id").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <FooterStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Local Amount" SortExpression="LocalGrandTotal">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvGrandTotal" runat="server" Visible="false" Text='<%#Eval("LocalGrandTotal")%>'></asp:Label>
                                                                    <asp:Label ID="lblgvGrandTotalSI" runat="server" Text='<%# GetCurrencySymbol(Eval("LocalGrandTotal").ToString(),Eval("LocationCurrency").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <FooterStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                        </Columns>


                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                </div>
                                                <br />
                                                <div class="col-md-12" style="margin-left: -30px">
                                                    <div class="col-md-9">
                                                        <asp:DataList CellPadding="10" RepeatDirection="Horizontal" runat="server" ID="dlPager" OnItemCommand="dlPager_ItemCommand">
                                                            <ItemTemplate>

                                                                <ul class="pagination">
                                                                    <li class='<%# Convert.ToBoolean(Eval("Enabled")) ? "page-item" : "page-item active" %>'>

                                                                        <asp:LinkButton Enabled='<%#Eval("Enabled") %>' runat="server" ID="lnkPageNo" Text='<%#Eval("Text") %>' CommandArgument='<%#Eval("Value") %>' CommandName="PageNo" CssClass="page-link"></asp:LinkButton>
                                                                    </li>
                                                                </ul>


                                                            </ItemTemplate>
                                                        </asp:DataList>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:Label ID="lblTotalAmount_List" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total %>"></asp:Label>
                                                    </div>
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
                        <asp:UpdatePanel ID="Update_New" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12">
                                                        <asp:LinkButton ID="btnControlsSetting" ToolTip="Controls Setting" runat="server" OnClick="btnControlsSetting_Click" Visible="true"><i class="fas fa-wrench"></i></asp:LinkButton>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:RadioButton ID="rbtEdit" runat="server" GroupName="SaveOrEdit" Text="Edit" Visible="false" Checked="true" />
                                                        <asp:RadioButton ID="rbtNew" runat="server" GroupName="SaveOrEdit" Text="New" Visible="false" />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:HiddenField ID="Hdn_Tax_By" runat="server" />
                                                        <asp:Label ID="lblSInvDate" runat="server" Text="<%$ Resources:Attendance,Invoice Date %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSInvDate" ErrorMessage="<%$ Resources:Attendance,Enter Invoice Date %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtSInvDate" runat="server" CssClass="form-control" Enabled="false" />
														
														<%--  <% 
                                                            if (Session["LocId"].ToString() == "8" || Session["LocId"].ToString() == "11" || Session["LocId"].ToString() == "14" || Session["LocId"].ToString() == "15")
                                                            {
                                                        %>--%>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender" runat="server" TargetControlID="txtSInvDate" />
                                                       <%-- <%
                                                            }
                                                        %>--%>

                                                        
                                                        <br />
                                                    </div>
                                                    <asp:HiddenField runat="server" ID="hdnOrderId" Value="" />
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblSInvNo" runat="server" Text="<%$ Resources:Attendance,Invoice No. %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSInvNo" ErrorMessage="<%$ Resources:Attendance,Enter Invoice No %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtSInvNo" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div style="display: none" class="col-md-6">
                                                        <asp:Label ID="lblOrderType" runat="server" Visible="false"
                                                            Text="<%$ Resources:Attendance,Order Type %>" />
                                                        <asp:DropDownList ID="ddlOrderType" Visible="false" runat="server"
                                                            CssClass="form-control" OnSelectedIndexChanged="ddlOrderType_SelectedIndexChanged"
                                                            AutoPostBack="true">
                                                            <asp:ListItem Text="<%$ Resources:Attendance, --Select--%>" Value="--Select--"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Direct%>" Value="D"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,By Sales Order%>" Value="Q"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:CheckBox ID="chk1" Visible="false" runat="server" Text="<%$ Resources:Attendance,Post %>" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblCurrency" runat="server" Text="<%$ Resources:Attendance,Currency %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save" Display="Dynamic"
                                                            SetFocusOnError="true" ControlToValidate="ddlCurrency" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Currency %>" />

                                                        <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="form-control"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlCurrency_OnSelectedIndexChanged" />

                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblexchangerate" runat="server" Text="<%$ Resources:Attendance,Exchange Rate%>" />
                                                        <asp:TextBox ID="txtExchangeRate" Text="1" AutoPostBack="true" OnTextChanged="txtExchangeRate_TextChanged" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" Enabled="True"
                                                            TargetControlID="txtExchangeRate" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" id="ctlInvoiceRefNo" runat="server">
                                                        <asp:Label ID="lblrefference" runat="server" Text="Reference No." />
                                                        <asp:TextBox ID="txtInvoiecRefno" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-6" id="ctlOrderId" runat="server">
                                                        <asp:Label ID="lblOrderId" runat="server" Text="Customer Order Id" />
                                                        <asp:TextBox ID="txtOrderId" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" id="ctlMerchant" runat="server">
                                                        <asp:Label ID="Label16" runat="server" Text="Invoice Merchant" />
                                                        <asp:DropDownList ID="ddlMerchant" runat="server" CssClass="form-control">
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" style="display: none">

                                                        <asp:Label ID="Label3145" runat="server" Text="Invoice Type" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Save" Display="Dynamic"
                                                            SetFocusOnError="true" ControlToValidate="ddlPaymentMode" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Invoice Type %>" />

                                                        <asp:DropDownList ID="ddlPaymentMode" runat="server" CssClass="form-control" Enabled="false"
                                                            AutoPostBack="True" OnSelectedIndexChanged="ddlPaymentTypeMode_SelectedIndexChanged" />

                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" id="ctlPosNo" runat="server">
                                                        <asp:Label ID="lblPOSNo" runat="server" Text="<%$ Resources:Attendance,POS No.%>" />
                                                        <a style="color: Red; display: none">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator6" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtPOSNo" ErrorMessage="<%$ Resources:Attendance,Enter POS No%>" Enabled="false"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtPOSNo" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblCustomerName" runat="server" Text="<%$ Resources:Attendance,Customer Name %>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator7" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCustomer" ErrorMessage="<%$ Resources:Attendance,Enter Customer Name %>"></asp:RequiredFieldValidator>

                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtCustomer" runat="server" CssClass="form-control" OnTextChanged="txtCustomer_TextChanged"
                                                                BackColor="#eeeeee" AutoPostBack="true" />
                                                            <cc1:AutoCompleteExtender ID="txtCustomerName_AutoCompleteExtender" runat="server"
                                                                DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                                MinimumPrefixLength="1" ServiceMethod="GetCompletionListCustomer" ServicePath=""
                                                                TargetControlID="txtCustomer" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <div class="input-group-btn">
                                                                <asp:Button ID="btnAddCustomer" runat="server" CssClass="btn btn-primary" OnClick="btnAddCustomer_OnClick"
                                                                    Text="<%$ Resources:Attendance,Add %>" CausesValidation="False" />
                                                                <asp:Button ID="lnkcustomerHistory" runat="server" Text="History" CssClass="btn btn-primary" CausesValidation="False"
                                                                    OnClick="lnkcustomerHistory_OnClick"></asp:Button>
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>




                                                    <div class="col-md-6" id="ctlContactPerson" runat="server">
                                                        <asp:HiddenField runat="server" ID="hdnContactId" />
                                                        <asp:Label ID="Label4" runat="server" Text="Contact" />
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtContactPerson" runat="server" CssClass="form-control" BackColor="#eeeeee" onchange="txtContactPerson_TextChanged(this)" />
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server"
                                                                DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                                MinimumPrefixLength="1" ServiceMethod="GetCompletionContactList" ServicePath=""
                                                                TargetControlID="txtContactPerson" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                            <div class="input-group-btn">
                                                                <asp:LinkButton ID="lnkAddNewContact" runat="server" ToolTip="Add New Contact" OnClick="lnkAddNewContact_Click" AlternateText="<%$ Resources:Attendance,Add %>" CausesValidation="False"><i class="fa fa-plus-square" style="font-size:35px;padding:5px;margin-top: -5px;"></i></asp:LinkButton>
                                                            </div>
                                                        </div>

                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblSalesPerson" runat="server" Text="<%$ Resources:Attendance,Sales Person%>" />
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSalesPerson" ErrorMessage="<%$ Resources:Attendance,Enter Sales Person %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtSalesPerson" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            OnTextChanged="txtSalesPerson_TextChanged" AutoPostBack="true" />
                                                        <cc1:AutoCompleteExtender ID="txtSalesPerson_AutoCompleteExtender" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="1" ServiceMethod="GetCompletionListEmployeeName" ServicePath=""
                                                            TargetControlID="txtSalesPerson" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>

                                                    <div style="display: none" class="col-md-6">
                                                        <asp:Label ID="lblAccountNo" runat="server" Text="<%$ Resources:Attendance,Account No%>" />
                                                        <asp:TextBox ID="txtAccountNo" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div style="display: none" class="col-md-6">
                                                        <asp:Label ID="lblTender" runat="server" Text="<%$ Resources:Attendance,Tender%>" />
                                                        <asp:TextBox ID="txtTender" runat="server" CssClass="form-control" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                            TargetControlID="txtTender" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div style="display: none" class="col-md-6">
                                                        <asp:Label ID="lblShift" runat="server" Text="<%$ Resources:Attendance,Shift%>" />
                                                        <asp:TextBox ID="txtShift" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div style="display: none" class="col-md-6">
                                                        <asp:Label ID="lblInvoiceCosting" runat="server" Text="<%$ Resources:Attendance,Invoice Costing%>" />
                                                        <asp:TextBox ID="txtInvoiceCosting" runat="server" CssClass="form-control" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                                            TargetControlID="txtInvoiceCosting" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>



                                                    <div class="col-md-6" id="Trans_Div" runat="server">
                                                        <asp:Label ID="lblTransType" runat="server" Text="Transaction Type"></asp:Label>
                                                        <asp:DropDownList ID="ddlTransType" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlTransType_SelectedIndexChanged"></asp:DropDownList>
                                                        <br />
                                                    </div>

                                                    <div id="trCreditInfo" runat="server" visible="false" class="col-md-12">
                                                        <div class="box box-info">
                                                            <div class="box-header with-border">
                                                                <h3 class="box-title">
                                                                    <asp:Label ID="Label46" Font-Names="Times New roman" Font-Size="18px" Font-Bold="true"
                                                                        runat="server" Text="Credit Terms & Condition"></asp:Label></h3>
                                                                <div class="box-tools pull-right">
                                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                        <i class="fa fa-minus"></i>
                                                                    </button>
                                                                </div>
                                                            </div>
                                                            <div class="box-body">
                                                                <div class="form-group">
                                                                    <div class="col-md-3">
                                                                        <asp:Label ID="lblCreditLimit" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Credit Limit %>" />
                                                                        &nbsp:&nbsp<asp:Label ID="lblCreditLimitValue" runat="server"></asp:Label>
                                                                        <asp:Label ID="lblCurrencyCreditLimit" runat="server"></asp:Label>
                                                                        <br />
                                                                    </div>
                                                                    <div class="col-md-3">
                                                                        <asp:Label ID="lblCreditDays" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Credit Days %>" />
                                                                        &nbsp:&nbsp<asp:Label ID="lblCreditDaysValue" runat="server"></asp:Label>
                                                                        <br />
                                                                    </div>
                                                                    <div class="col-md-3">
                                                                        <asp:Label ID="Label21" runat="server" Font-Bold="true" Text="Currenct Balance" />
                                                                        &nbsp:&nbsp<asp:Label ID="lblCurrentBalance" runat="server"></asp:Label>
                                                                        <br />
                                                                    </div>
                                                                    <div class="col-md-3">
                                                                        <asp:Label ID="Label47" runat="server" Font-Bold="true" Text="Credit Parameter" />
                                                                        &nbsp:&nbsp<asp:Label ID="lblCreditParameterValue" runat="server"></asp:Label>
                                                                        <%--  <asp:RadioButton ID="rbtnAdvanceCheque" runat="server" Text="Advance Cheque Basis" Enabled="false"
                                                                                GroupName="Parameter"  />
                                                                            <asp:RadioButton ID="rbtnInvoicetoInvoice"  Enabled="false" runat="server" GroupName="Parameter" Text="Invoice to Invoice Payment"
                                                                                 />
                                                                            <asp:RadioButton ID="rbtnAdvanceHalfpayment"  Enabled="false" GroupName="Parameter" runat="server"
                                                                                Text="50% Advance and 50% on Delivery"  />--%>
                                                                        <br />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-6" id="ctlInvoiceTo" runat="server">
                                                        <asp:Label ID="lblInvoiceTo" runat="server" Text="<%$ Resources:Attendance,Invoice Address %>" />
                                                        <asp:TextBox ID="txtInvoiceTo" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            AutoPostBack="true" OnTextChanged="txtInvoiceTo_TextChanged" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender_invoice" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListAddressName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtInvoiceTo"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" id="ctlShipTo" runat="server">
                                                        <asp:Label ID="lblShipTo" runat="server" Text="<%$ Resources:Attendance,Ship To %>" />
                                                        <asp:TextBox ID="txtShipCustomerName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            AutoPostBack="true" OnTextChanged="txtShipTo_TextChanged" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender_shipCustname" runat="server" DelimiterCharacters=""
                                                            Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1"
                                                            ServiceMethod="GetCompletionListContact" ServicePath="" TargetControlID="txtShipCustomerName"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
													        <div class="col-md-6">
                                                        <table style="border: 1px solid;">
                                                            <tr>
                                                                <th>
                                                                    <asp:Label ID="InvoiceAddress" runat="server" Text=""></asp:Label>
                                                                </th>
                                                            </tr>

                                                        </table>

                                                    </div>
                                                    <div class="col-md-6">
                                                        <table style="border: 1px solid;">
                                                            <tr>
                                                                <th>
                                                                    <asp:Label ID="ShipAddress" runat="server" Text=""></asp:Label>
                                                                </th>
                                                            </tr>
                                                        </table>
                                                    </div>									
													
													
													
                                                    <div class="col-md-6" id="ctlShipingAddress" runat="server">
                                                        <asp:Label ID="lblShipingAddress" runat="server" Text="<%$ Resources:Attendance,Shipping Address %>" />
                                                        <asp:TextBox ID="txtShipingAddress" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            AutoPostBack="true" OnTextChanged="txtShipingAddress_TextChanged" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender_shipaddress" runat="server" DelimiterCharacters=""
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

                                                    <div class="col-md-4">
                                                        <br />
                                                        <asp:RadioButton ID="RdoSo" runat="server" Text="<%$ Resources:Attendance,Sales Order %>"
                                                            OnCheckedChanged="RdoSo_CheckedChanged" AutoPostBack="true" GroupName="So" />
                                                        <asp:RadioButton ID="RdoWithOutSo" runat="server" Style="margin-left: 15px;" GroupName="So"
                                                            Text="<%$ Resources:Attendance,Without Sales Order %>" OnCheckedChanged="RdoSo_CheckedChanged"
                                                            AutoPostBack="true" />
                                                        <br />
                                                    </div>
													    <div class="col-md-6">
                                                        <br />
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
                                                    </div>													
                                                    <div style="display: none" class="col-md-6">
                                                        <asp:CheckBox ID="chkPost" runat="server" Visible="false" Text="<%$ Resources:Attendance,Post %>" />
                                                        <br />
                                                    </div>

                                                    <div class="col-md-6" id="ctlShipmentId" runat="server">
                                                        <asp:Label ID="lblShipmentRef" runat="server" Text="Shipment No" />
                                                        <asp:TextBox ID="txtShipmentRef" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:RadioButton ID="rbtnFormView" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Form View%>"
                                                            Visible="false" AutoPostBack="true" GroupName="Product"
                                                            OnCheckedChanged="rbtnFormView_OnCheckedChanged" />&nbsp;&nbsp;&nbsp;
                                                        <asp:RadioButton ID="rbtnAdvancesearchView" Font-Bold="true" runat="server" Visible="false"
                                                            Text="<%$ Resources:Attendance,Advance Search View%>"
                                                            AutoPostBack="true" GroupName="Product" OnCheckedChanged="rbtnFormView_OnCheckedChanged" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Button ID="btnAddNewProduct" runat="server" Text="<%$ Resources:Attendance,Add Product %>"
                                                            Visible="false" CssClass="btn btn-info"
                                                            OnClick="btnAddNewProduct_Click" />

                                                        <asp:Button ID="btnAddProductScreen" Visible="false" runat="server"
                                                            Text="<%$ Resources:Attendance,Add Product List %>" CssClass="btn btn-info" OnClick="btnAddProductScreen_Click" />

                                                        <asp:Button ID="btnAddtoList" runat="server" Text="<%$ Resources:Attendance,Fill Your Product %>"
                                                            CssClass="btn btn-info" Visible="false" OnClick="btnAddtoList_Click" />
                                                        <br />
                                                    </div>

                                                    <div id="PnlProductSearching" runat="server" class="col-md-12">
                                                        <br />

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
                                                                        <%-- <asp:ListItem Text="<%$ Resources:Attendance,Sales Order No %>" Value="SalesOrderNo"></asp:ListItem>--%>
                                                                    </asp:DropDownList>
                                                                </div>
                                                                <div class="col-lg-3">
                                                                    <asp:TextBox ID="txtProductSerachValue" runat="server" CssClass="form-control" AutoPostBack="false" OnTextChanged="txtProductSerachValue_OnTextChanged" />
                                                                    <asp:TextBox ID="txtProductId" runat="server" CssClass="form-control" AutoPostBack="True"
                                                                        OnTextChanged="txtProductCode_TextChanged" BackColor="#eeeeee" Visible="false" />
                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtenderProductcode" runat="server" CompletionInterval="100"
                                                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductCode"
                                                                        ServicePath="" TargetControlID="txtProductId" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                    </cc1:AutoCompleteExtender>
                                                                    <asp:TextBox ID="txtSearchProductName" runat="server" BackColor="#eeeeee" CssClass="form-control"
                                                                        AutoPostBack="True" OnTextChanged="txtProductName_TextChanged" />
                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtenderProductName" runat="server" CompletionInterval="100"
                                                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductName"
                                                                        ServicePath="" TargetControlID="txtSearchProductName" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                    </cc1:AutoCompleteExtender>
                                                                </div>
                                                                <div class="col-lg-2" style="text-align: center">
                                                                    <asp:LinkButton runat="server" CausesValidation="False" ToolTip="<%$ Resources:Attendance,Add %>" ID="ImgbtnProductSave" OnClick="btnProductSave_Click" Visible="false"><i class="fa fa-plus-square" style="font-size:35px;padding:5px;margin-top: -5px;"></i></asp:LinkButton>
                                                                    <asp:LinkButton ID="imgbtnsearch" runat="server" CausesValidation="False" OnClick="imgbtnsearch_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                                        <asp:LinkButton ID="ImgbtnRefresh" runat="server" CausesValidation="False" OnClick="ImgbtnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                                                </div>
                                                                <div class="col-md-2"></div>
                                                            </div>
                                                        </div>
                                                        <div class="box box-warning box-solid">
                                                            <div class="box-body">
                                                                <div class="row">
                                                                    <div class="col-md-12">
                                                                        <div style="overflow: auto; max-height: 500px;">
                                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvSerachGrid" runat="server" AutoGenerateColumns="False" Width="100%" OnRowDataBound="gvSerachGrid_OnRowDataBound"
                                                                                ShowFooter="true">
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center">
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="chkTrandId" runat="server" AutoPostBack="true" OnCheckedChanged="chkTrandId_CheckedChanged" />
                                                                                            <asp:Label ID="TransId" runat="server" Text='<%# Eval("Trans_Id") %>' Visible="false"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                        <ItemStyle />
                                                                                        <FooterStyle BorderStyle="None" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Order No. %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblPONo" runat="server" Text='<%# Eval("SalesOrderNo") %>'></asp:Label>
                                                                                            <asp:Label ID="lblsoid" runat="server" Visible="false" Text='<%# Eval("SoID") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                        <ItemStyle />
                                                                                        <FooterStyle BorderStyle="None" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order Date %>" SortExpression="SalesOrderDate">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblgvOrdervDate" runat="server" Text='<%#GetDate(Eval("SalesOrderDate").ToString())%>' />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id%>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblproductcode" runat="server" Text='<%# ProductCode(Eval("Product_Id").ToString()) %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterStyle BorderStyle="None" />
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Product Name %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblProductId" runat="server" Text='<%#Eval("ProductName") %>'></asp:Label>
                                                                                            <asp:Label ID="lblgvProductId" Visible="false" runat="server" Text='<%# Eval("Product_Id") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterStyle BorderStyle="None" />
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance, Unit %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblUnitID" runat="server" Text='<%# Eval("UnitId").ToString() %>'
                                                                                                Visible="false"></asp:Label>
                                                                                            <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("Unit_Name") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterStyle BorderStyle="None" />
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblgvOrderqty" runat="server" Text='<%#SetDecimal(Eval("OrderQty").ToString()) %>' />
                                                                                        </ItemTemplate>
                                                                                        <FooterStyle BorderStyle="None" />
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                </Columns>



                                                                                <PagerStyle CssClass="pagination-ys" />

                                                                            </asp:GridView>
                                                                        </div>
                                                                        <asp:HiddenField ID="hdnProductId" runat="server" />
                                                                        <asp:HiddenField ID="hdnProductName" runat="server" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="box box-warning box-solid">
                                                            <div class="box-body">
                                                                <div class="row">
                                                                    <div class="col-md-12">
                                                                        <div class="col-md-12" runat="server" id="scrollArea" onscroll="SetDivPosition()" style="overflow: auto; max-height: 500px;">

                                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvProduct" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                                OnRowCreated="GvSalesOrderDetail_RowCreated" OnRowDataBound="GvProductDetail_OnRowDataBound">

                                                                                <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton ID="IbtnPDDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' Visible='<%# hdnCanDelete.Value=="true"?true:false%>' OnCommand="IbtnPDDelete_Command" ToolTip="<%$ Resources:Attendance,Delete %>"><i class="fa fa-trash"></i></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                        <ItemStyle />
                                                                                        <FooterStyle BorderStyle="None" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblSONo" runat="server" Text='<%# Eval("SalesOrderNo") %>'></asp:Label>
                                                                                            <asp:Label ID="lblSOId" runat="server" Visible="false" Text='<%# Eval("SoID") %>'></asp:Label>
                                                                                            <asp:Label ID="lblTransId" runat="server" Visible="false"
                                                                                                Text='<%# Eval("Trans_Id") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                        <ItemStyle />
                                                                                        <FooterTemplate>
                                                                                            <asp:Label ID="lblTotalQuantity" runat="server" Text="<%$ Resources:Attendance,Total %>"></asp:Label>
                                                                                        </FooterTemplate>
                                                                                        <FooterStyle BorderStyle="None" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblgvSerialNo" Width="30px" runat="server" Text='<%#Eval("Serial_No") %>'
                                                                                                Visible="false" />
                                                                                            <asp:Label ID="lblgvsNo" Width="30px" runat="server" Text='<%#Container.DataItemIndex+1 %>' />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Id%>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblgvProductCode" runat="server" Text='<%#ProductCode(Eval("Product_Id").ToString()) %>' />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" Width="70px" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Name %>">
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lblGrdHdProductName" Text="<%$ Resources:Attendance,Name %>" runat="server"></asp:Label>
                                                                                            <asp:CheckBox ID="chkShortProductName1" Text="" runat="server" ToolTip="Dispay detail Name" OnCheckedChanged="chkShortProductName_CheckedChanged" AutoPostBack="true"></asp:CheckBox>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <table width="100%">
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:HiddenField ID="hdngvProductId" runat="server" Value='<%#Eval("Product_Id") %>' />
                                                                                                        <asp:Label ID="lblgvProductName" Font-Size="10" data-html="true" data-toggle="tooltip" title='<%#Eval("ProductDescription") %>' Width="80px" runat="server" Text='<%#Eval("ProductName") %>' Visible="false" />
                                                                                                        <asp:Label ID="lblShortProductName1" Font-Size="10" runat="server" Text='<%# Eval("ProductName").ToString().Length>16?Eval("ProductName").ToString().Substring(0,15) + "...":Eval("ProductName").ToString() %>' Visible="true"></asp:Label>
                                                                                                    </td>
                                                                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultRight()%>'>

                                                                                                        <asp:ImageButton ID="lnkDes" runat="server" ImageUrl="~/Images/detail.png" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                            <br />
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
                                                                                                                <tr style="background-color: whitesmoke;">
                                                                                                                    <td colspan="2" align="left" valign="top">
                                                                                                                        <asp:Panel ID="pnl" runat="server" Width="100%" Height="300px" ScrollBars="Vertical">
                                                                                                                            <asp:Label ID="lblgvProductDescription" runat="server" Text='<%#Eval("ProductDescription") %>' />
                                                                                                                        </asp:Panel>
                                                                                                                        <br />
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </asp:Panel>
                                                                                            <cc1:HoverMenuExtender ID="hme37" runat="Server" TargetControlID="lnkDes" PopupControlID="PopupMenu1"
                                                                                                HoverCssClass="popupHover" PopupPosition="Left" OffsetX="0" OffsetY="0" PopDelay="50" />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Name %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:DropDownList ID="ddlUnitName" runat="server" Width="100px"
                                                                                                AppendDataBoundItems="true">
                                                                                            </asp:DropDownList>
                                                                                            <asp:Label ID="lblgvUnit" runat="server" Text='<%#Eval("Unit_Name") %>' Visible="false" />
                                                                                            <asp:HiddenField ID="hdngvUnitId" runat="server" Value='<%#Eval("UnitId") %>' />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Price %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="lblgvUnitPrice" onchange="SetSelectedRow(this)"
                                                                                                runat="server" Width="80px" Text='<%#Eval("UnitPrice") %>'
                                                                                                OnTextChanged="txtgvUnitPrice_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                            <cc1:FilteredTextBoxExtender ID="FiltergvlblgvUnitPrice" runat="server" Enabled="True"
                                                                                                TargetControlID="lblgvUnitPrice" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                            <%--<asp:Label ID="lblgvUnitPrice" runat="server" Text='<%#Eval("UnitPrice") %>' />--%>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Free %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="lblgvFreeQuantity" onchange="SetSelectedRow(this)" runat="server" Width="30px" Text='<%#Eval("FreeQty") %>' />
                                                                                            <cc1:FilteredTextBoxExtender ID="FiltergvlblgvFreeQuantity" runat="server" Enabled="True"
                                                                                                TargetControlID="lblgvFreeQuantity" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblgvOrderqty" runat="server" Text='<%#Eval("OrderQty") %>' />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Sold %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblgvSoldQuantity" runat="server" Text='<%#Eval("SoldQty") %>' />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Sys. %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblgvSystemQuantity" runat="server" Text='<%#Eval("SysQty") %>' />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Remain %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblgvRemaningQuantity" runat="server" Text='<%#Eval("RemainQty") %>' />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Sales %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtgvSalesQuantity" onchange="SetSelectedRow(this)" Width="30px" runat="server" OnTextChanged="txtgvSalesQuantity_TextChanged"
                                                                                                AutoPostBack="true" Text='<%#Eval("Quantity") %>' />
                                                                                            <asp:LinkButton ID="lnkAddSO" runat="server" Text="<%$ Resources:Attendance,Add %>"
                                                                                                OnClick="lnkAddSO_Click" ForeColor="Blue" Font-Underline="true"></asp:LinkButton>
                                                                                            <cc1:FilteredTextBoxExtender ID="FiltergvSalesQuantity" runat="server" Enabled="True"
                                                                                                TargetControlID="txtgvSalesQuantity" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Price %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblgvQuantityPrice" runat="server" />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,% %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtgvDiscountP" onchange="SetSelectedRow(this)" Width="30px" Text='<%#Eval("DiscountP") %>' runat="server"
                                                                                                OnTextChanged="txtgvDiscountP_TextChanged" AutoPostBack="true" />
                                                                                            <cc1:FilteredTextBoxExtender ID="FilterDiscountP" runat="server" Enabled="True" TargetControlID="txtgvDiscountP"
                                                                                                ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Value %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtgvDiscountV" onchange="SetSelectedRow(this)" Enabled="false" Width="45px" Text='<%#Eval("DiscountV") %>' runat="server"
                                                                                                OnTextChanged="txtgvDiscountV_TextChanged" AutoPostBack="true" />
                                                                                            <cc1:FilteredTextBoxExtender ID="FilterDiscountV" runat="server" Enabled="True" TargetControlID="txtgvDiscountV"
                                                                                                ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtgvTaxP" onchange="SetSelectedRow(this)" Width="30px" Text='<%#Eval("TaxP") %>' runat="server"
                                                                                                Enabled="false" OnTextChanged="txtgvTaxP_TextChanged" AutoPostBack="true" Visible="true" />
                                                                                            <cc1:FilteredTextBoxExtender ID="FilterTaxP" runat="server" Enabled="True" TargetControlID="txtgvTaxP"
                                                                                                ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                            <asp:ImageButton ID="imgBtnaddtax" runat="server" CommandArgument='<%# Eval("Serial_No") %>'
                                                                                                ImageUrl="~/Images/plus.png" Width="30px" Height="30px" OnCommand="imgaddTax_Command"
                                                                                                ToolTip="Add Tax" />
                                                                                            <asp:ImageButton ID="BtnAddTax" runat="server" CommandName="gvProduct" CommandArgument='<%# Eval("Product_Id") %>' OnCommand="BtnAddTax_Command" ImageUrl="~/Images/plus.png" Width="30px" Height="30px" ToolTip="Add Tax" />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Value %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtgvTaxV" onchange="SetSelectedRow(this)" Width="45px" runat="server" OnTextChanged="txtgvTaxV_TextChanged"
                                                                                                AutoPostBack="true" Text='<%#Eval("TaxV") %>'
                                                                                                Enabled="false" />
                                                                                            <cc1:FilteredTextBoxExtender ID="FilterTaxV" runat="server" Enabled="True" TargetControlID="txtgvTaxV"
                                                                                                ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtgvTotal" onchange="SetSelectedRow(this)" Width="50px" ReadOnly="true" runat="server" />
                                                                                            <cc1:FilteredTextBoxExtender ID="FilterTotal" runat="server" Enabled="True" TargetControlID="txtgvTotal"
                                                                                                ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton ID="lnkStockInfo" runat="server" Text='<%#GetProductStock(Eval("Product_Id").ToString()) %>'
                                                                                                Font-Underline="true" ToolTip="View Detail" OnCommand="lnkStockInfo_Command"
                                                                                                ForeColor="Blue" CommandArgument='<%# Eval("Product_Id") %>'></asp:LinkButton>
                                                                                            <asp:Literal runat="server" ID="lit1" Text="<tr id='trGrid'><td colspan='18' align='right'>" />
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
                                                                                                            <asp:TextBox ID="txttaxPerchild" runat="server" AutoPostBack="true"
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
                                                                                                            <asp:TextBox ID="txttaxValuechild" runat="server" AutoPostBack="true"
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
                                                    <div class="col-md-12">
                                                        <hr />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblAmount" runat="server" Text="<%$ Resources:Attendance,Gross Amount %>" />
                                                        <asp:TextBox ID="txtAmount" runat="server" ReadOnly="true" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblTotalQuantity" runat="server" Text="<%$ Resources:Attendance, Total Quantity %>" />
                                                        <asp:TextBox ID="txtTotalQuantity" runat="server" CssClass="form-control" ReadOnly="true" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True"
                                                            TargetControlID="txtTotalQuantity" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblDiscountP" runat="server" Text="<%$ Resources:Attendance,Discount(%) %>" />
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtDiscountP" runat="server" CssClass="form-control"
                                                                OnTextChanged="txtDiscountP_TextChanged" AutoPostBack="true" />
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" Enabled="True"
                                                                TargetControlID="txtDiscountP" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <div style="width: 50%" class="input-group-btn">
                                                                <asp:Label ID="Label3" Width="100px" runat="server" CssClass="form-control" Text="<%$ Resources:Attendance,Value %>" />
                                                                <asp:TextBox ID="txtDiscountV" Width="110px" runat="server" CssClass="form-control"
                                                                    OnTextChanged="txtDiscountV_TextChanged" AutoPostBack="true" />
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" Enabled="True"
                                                                    TargetControlID="txtDiscountV" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblPriceafterdiscountheader" runat="server"
                                                            Text="<%$ Resources:Attendance,Price After Discount %>" Visible="false" />
                                                        <asp:TextBox ID="txtPriceafterdiscountheader" runat="server" CssClass="form-control"
                                                            Visible="false" ReadOnly="True" />
                                                        <br />
                                                    </div>
                                                    <div style="display: none;" id="Div_Tax_Grid" runat="server" visible="false" class="col-md-12">
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
                                                                                OnTextChanged="txtTaxName_TextChanged" BackColor="#eeeeee"
                                                                                Text='<%#Eval("TaxName") %>' CausesValidation="false"></asp:TextBox>

                                                                            <cc1:AutoCompleteExtender ID="autoComplete122566" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionListTax" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtTaxName" UseContextKey="True"
                                                                                CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                        </EditItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:TextBox ID="txtTaxFooter" runat="server" Font-Names="Verdana" AutoPostBack="false"
                                                                                OnTextChanged="txtTaxFooter_TextChanged" BackColor="#eeeeee"
                                                                                CausesValidation="true" Width="400px"></asp:TextBox>
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
                                                                            <asp:Label ID="lblTaxper" runat="server" Text='<%#SetDecimal(Eval("Tax_Per").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtTaxper" runat="server" Font-Names="Verdana"
                                                                                Text='<%#Eval("Tax_Per") %>' CausesValidation="true" AutoPostBack="true"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FiltergvSalesQuantity11488taxper" runat="server"
                                                                                Enabled="True" TargetControlID="txtTaxper" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </EditItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:TextBox ID="txtTaxperFooter" runat="server" Font-Names="Verdana"
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
                                                                            <asp:Label ID="lblTaxValue" runat="server" Text='<%#SetDecimal(Eval("Tax_Value").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtTaxValue" runat="server" Font-Names="Verdana"
                                                                                Text='<%#Eval("Tax_Value") %>' CausesValidation="true" AutoPostBack="false"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FiltergvSalesQuantity11488" runat="server" Enabled="True"
                                                                                TargetControlID="txtTaxValue" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </EditItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:TextBox ID="txtTaxValueFooter" runat="server" Font-Names="Verdana"
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
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblTaxP" runat="server" Text="<%$ Resources:Attendance,Tax(%) %>" />
                                                            <asp:TextBox ID="txtTaxP" runat="server" CssClass="form-control" OnTextChanged="txtTaxP_TextChanged"
                                                                AutoPostBack="true" Enabled="false" />
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" Enabled="True"
                                                                TargetControlID="txtTaxP" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Value %>" />
                                                            <asp:TextBox ID="txtTaxV" runat="server" CssClass="form-control" OnTextChanged="txtTaxV_TextChanged"
                                                                AutoPostBack="true" Enabled="false" />
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" Enabled="True"
                                                                TargetControlID="txtTaxV" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblGrandTotal" runat="server" Text="<%$ Resources:Attendance,Net Amount %>" />
                                                        <asp:TextBox ID="txtNetAmount" runat="server" CssClass="form-control" Visible="false"
                                                            ReadOnly="True" />
                                                        <asp:TextBox ID="txtGrandTotal" runat="server" CssClass="form-control" ReadOnly="True" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Total Expenses %>" />
                                                        <asp:TextBox ID="txtTotalExpensesAmount" runat="server" CssClass="form-control" ReadOnly="True" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,Net Amount %>" />
                                                        <asp:TextBox ID="txtNetAmountwithexpenses" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div style="overflow: auto">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvadvancepayment" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                EmptyDataText="Record Not Found" ShowFooter="true" BorderStyle="Solid" Width="100%"
                                                                PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'>

                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order No. %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblorderNo" runat="server" Text='<%# Eval("OrderNo").ToString() %>' />

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
                                                        <cc1:TabContainer ID="TabContainer1" runat="server" CssClass="ajax__tab_yuitabview-theme" ActiveTabIndex="0">
                                                            <cc1:TabPanel ID="TabProductPaymentMode" runat="server" Width="100%" HeaderText="<%$ Resources:Attendance,Payment Mode %>">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_TabPayment" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Payment Mode %>"></asp:Label>
                                                                                        <a style="color: Red">*</a>
                                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator8" ValidationGroup="Add_Container" Display="Dynamic"
                                                                                            SetFocusOnError="true" ControlToValidate="ddlTabPaymentMode" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Payment Mode %>" />

                                                                                        <asp:DropDownList ID="ddlTabPaymentMode" runat="server" CssClass="form-control"
                                                                                            AutoPostBack="True" OnSelectedIndexChanged="ddlPaymentMode_SelectedIndexChanged">
                                                                                        </asp:DropDownList>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblCreditNote" runat="server" Text="<%$ Resources:Attendance,Customer Credit Note %>"></asp:Label>
                                                                                        <asp:TextBox ID="txtCreditNote" runat="server" CssClass="form-control" AutoPostBack="true" Enabled="false" OnTextChanged="txtCreditNote_TextChanged"></asp:TextBox>
                                                                                        <cc1:AutoCompleteExtender ID="autoCompleteCreditNote" runat="server" DelimiterCharacters=""
                                                                                            Enabled="True" ServiceMethod="GetCompletionListCreditNote" ServicePath="" CompletionInterval="100"
                                                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtCreditNote"
                                                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                        </cc1:AutoCompleteExtender>
                                                                                        <br />

                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblPayAmmount" runat="server" Text="<%$ Resources:Attendance,Balance Amount%>"></asp:Label>
                                                                                        <asp:TextBox ID="txtPayAmount" runat="server" CssClass="form-control" OnTextChanged="txtPayAmount_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                                            TargetControlID="txtPayAmount" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                </div>
                                                                                <div id="pnlpaybank" runat="server" class="col-md-12">
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblPayAccountNo" runat="server" Text="<%$ Resources:Attendance,Account No. %>"></asp:Label>
                                                                                        <a style="color: Red">*</a>
                                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator9" ValidationGroup="Add_Container"
                                                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtPayAccountNo" ErrorMessage="<%$ Resources:Attendance,Enter Account No %>"></asp:RequiredFieldValidator>

                                                                                        <asp:TextBox ID="txtPayAccountNo" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                                            OnTextChanged="txtPayAccountNo_TextChanged" BackColor="#eeeeee" />
                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                                                            Enabled="True" ServiceMethod="GetCompletionListAccountNo" ServicePath="" CompletionInterval="100"
                                                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtPayAccountNo"
                                                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                        </cc1:AutoCompleteExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
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
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Exchange Rate %>"></asp:Label>
                                                                                        <asp:TextBox ID="txtPaymentExchangerate" runat="server" CssClass="form-control" Text="1" Enabled="false"></asp:TextBox>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="Label7" runat="server" Text="Local Amount"></asp:Label>
                                                                                        <div class="input-group">
                                                                                            <asp:TextBox ID="txtLocalAmount" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                                                            <div class="input-group-btn">
                                                                                                <asp:LinkButton runat="server" ValidationGroup="Add_Container" ToolTip="<%$ Resources:Attendance,Add %>" ID="btnPaymentSave" OnClick="btnPaymentSave_Click"><i class="fa fa-plus-square" style="font-size:35px;padding:5px;margin-top: -5px;"></i></asp:LinkButton>
                                                                                            </div>
                                                                                        </div>
                                                                                        <br />
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <div style="overflow: auto">
                                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvPayment" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                                            ShowFooter="true" BorderStyle="Solid" Width="100%" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'>

                                                                                            <Columns>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:LinkButton ID="IbtnDeletePay" runat="server" CausesValidation="False" CommandArgument='<%# Eval("TransId") %>' ToolTip="<%$ Resources:Attendance,Delete %>" OnCommand="btnDeletePay_Command"><i class="fa fa-trash"></i></asp:LinkButton>
                                                                                                    </ItemTemplate>
                                                                                                    <FooterStyle BorderStyle="None" />
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Payment Mode %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblCreditNoteId" runat="server" Text='<%# Eval("field3").ToString() %>' Visible="false" />
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
                                                            <cc1:TabPanel ID="TabProductSupplier" runat="server" Width="100%" HeaderText="<%$ Resources:Attendance,Expenses %>">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_ProductSupplier" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">

                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Attendance,Currency %>"></asp:Label>
                                                                                    <a style="color: Red">*</a>
                                                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator11" ValidationGroup="Add_Supplier" Display="Dynamic"
                                                                                        SetFocusOnError="true" ControlToValidate="ddlExpCurrency" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Currency %>" />

                                                                                    <asp:DropDownList ID="ddlExpCurrency" runat="server" Enabled="false" CssClass="form-control">
                                                                                    </asp:DropDownList>
                                                                                    <br />
                                                                                </div>

                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Attendance,Exchange Rate %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtExpExchangeRate" Enabled="false" ReadOnly="true" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtFCExpAmount_OnTextChanged">0</asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender41" runat="server" Enabled="True"
                                                                                        TargetControlID="txtExpExchangeRate" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <br />
                                                                                </div>





                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblSelectExp" runat="server" Text="<%$ Resources:Attendance,Select Expenses %>" />
                                                                                    <a style="color: Red">*</a>
                                                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator10" ValidationGroup="Add_Supplier" Display="Dynamic"
                                                                                        SetFocusOnError="true" ControlToValidate="ddlExpense" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Expenses %>" />

                                                                                    <asp:DropDownList ID="ddlExpense" runat="server" AutoPostBack="True"
                                                                                        OnSelectedIndexChanged="ddlExpense_SelectedIndexChanged" CssClass="form-control">
                                                                                    </asp:DropDownList>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblExpAccount" runat="server" Text="<%$ Resources:Attendance,Expenses Account %>"></asp:Label>
                                                                                    <a style="color: Red">*</a>
                                                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator12" ValidationGroup="Add_Supplier"
                                                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtExpensesAccount" ErrorMessage="<%$ Resources:Attendance,Enter Expenses Account %>"></asp:RequiredFieldValidator>

                                                                                    <asp:TextBox ID="txtExpensesAccount" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                                        OnTextChanged="txtExpensesAccount_TextChanged" BackColor="#eeeeee" />
                                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                                                        Enabled="True" ServiceMethod="GetCompletionListAccountNo" ServicePath="" CompletionInterval="100"
                                                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtExpensesAccount"
                                                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                    </cc1:AutoCompleteExtender>
                                                                                    <br />
                                                                                </div>

                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblFCExpAmount" runat="server" Text="<%$ Resources:Attendance,FCExp Amount %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtFCExpAmount" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtFCExpAmount_OnTextChanged">0</asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender40" runat="server" Enabled="True"
                                                                                        TargetControlID="txtFCExpAmount" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <br />
                                                                                </div>

                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblExpCharges" runat="server" Text="<%$ Resources:Attendance,Expenses Charges %>" />
                                                                                    <a style="color: Red">*</a>
                                                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator13" ValidationGroup="Add_Supplier"
                                                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtExpCharges" ErrorMessage="<%$ Resources:Attendance,Enter Expenses Charges %>"></asp:RequiredFieldValidator>

                                                                                    <asp:TextBox ID="txtExpCharges" runat="server" ReadOnly="true" Enabled="false" CssClass="form-control">0</asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender42" runat="server" Enabled="True"
                                                                                        TargetControlID="txtExpCharges" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <asp:HiddenField ID="hdnProductExpenses" runat="server" Value="0" />
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
                                                                                            <asp:Button ID="Btn_Add_Expenses_Tax" runat="server" ValidationGroup="Add_Supplier" Text="Add Tax" OnClick="Btn_Add_Expenses_Tax_Click" CssClass="btn btn-info" />
                                                                                        </div>
                                                                                    </div>
                                                                                    <br />
                                                                                </div>

                                                                                <div class="col-md-6" style="text-align: center">
                                                                                    <br />
                                                                                    <asp:Button ID="Btn_Add_Expenses" Visible="false" ValidationGroup="Add_Supplier" runat="server" Text="Add Expenses" OnClick="Btn_Add_Expenses_Click" CssClass="btn btn-info" />

                                                                                    <asp:Button ID="Btn_Exp_Reset" Visible="false" runat="server" Text="Reset" OnClick="Btn_Exp_Reset_Click" CssClass="btn btn-warning" />
                                                                                </div>

                                                                                <div class="col-md-12">
                                                                                    <br />
                                                                                    <div style="overflow: auto">
                                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GridExpenses" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                                            ShowFooter="True" BorderStyle="Solid" Width="100%" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'>

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
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Expenses %>" SortExpression="Expense_Id">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:HiddenField ID="Hdn_Expense_Id_GV" runat="server" Value='<%# Eval("Expense_Id") %>' />
                                                                                                        <asp:Label ID="lblgvExpense_Id" runat="server" Text='<%# GetExpName(Eval("Expense_Id").ToString()) %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterStyle BorderStyle="None" />
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Currency %>" SortExpression="ProductSupplierCode">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvExpCurrencyID" runat="server" Text='<%# CurrencyName(Eval("ExpCurrencyID").ToString()) %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterStyle BorderStyle="None" />
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, FC Exchange Amount%>" SortExpression="ProductSupplierCode">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvFCExchangeAmount" runat="server" Text='<%# getCurrencyConversion(Eval("ExpCurrencyID").ToString(),Eval("FCExpAmount").ToString()) %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterStyle BorderStyle="None" />
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Exchange Rate %>" SortExpression="ProductSupplierCode">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvExpExchangeRate" runat="server" Text='<%# SetDecimal(Eval("ExpExchangeRate").ToString()) %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lbltotExp" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance, Total Expenses%> " /><b>:</b>
                                                                                                    </FooterTemplate>
                                                                                                    <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Expenses Charges %>" SortExpression="Exp_Charges">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvExp_Charges" runat="server" Text='<%#SetDecimal(Eval("Exp_Charges").ToString()) %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="txttotExp" runat="server" Font-Bold="true" Text="0" />
                                                                                                    </FooterTemplate>
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
                                                                                        <asp:HiddenField ID="hdnfPSC" runat="server" />
                                                                                    </div>
                                                                                    <br />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_ProductSupplier">
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
                                                            <cc1:TabPanel ID="TabPanel1" runat="server" Width="100%" HeaderText="<%$ Resources:Attendance,Remark %>">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_Remark" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12" id="ctlRemark" runat="server">
                                                                                    <asp:TextBox ID="txtRemark" runat="server" CssClass="form-control" TextMode="MultiLine" Height="160px"></asp:TextBox>
                                                                                    <br />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress8" runat="server" AssociatedUpdatePanelID="Update_Remark">
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
                                                            <cc1:TabPanel ID="TabPanel2" runat="server" Width="100%" HeaderText="<%$ Resources:Attendance,Terms & Conditions %>">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_Terms_Conditions" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <cc1:Editor ID="txtCondition1" runat="server" TextMode="MultiLine" Height="160px" />
                                                                                    <br />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress9" runat="server" AssociatedUpdatePanelID="Update_Terms_Conditions">
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

                                                            <cc1:TabPanel ID="TabPanel3" runat="server" Width="100%" HeaderText="<%$ Resources:Attendance,Transport %>">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Transport_Panel" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <div class="col-md-12" style="text-align:center">
                                                                                  <asp:RadioButton ID="chkCustomer" runat="server" Checked="true" OnCheckedChanged="ChkTrans_Changed" AutoPostBack="true" GroupName="Trans" Text="Courier" />&nbsp;

                                                                                    <asp:RadioButton ID="chkEmployee" runat="server" OnCheckedChanged="ChkTrans_Changed" AutoPostBack="true" GroupName="Trans" Text="Employee" />
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row" id="pnlCustomer" runat="server">
                                                                                <div class="col-md-12">
                                                                                    <div class="col-md-12">
                                                                                        <asp:Label ID="Label23" runat="server" Text="Courier Services"></asp:Label>
                                                                                        <a style="color: Red">*</a>
                                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator18" ValidationGroup="Save"
                                                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtcustomername" ErrorMessage="<%$ Resources:Attendance,Enter Customer Name%>"></asp:RequiredFieldValidator>

                                                                                        <asp:TextBox ID="txtcustomername" runat="server" BackColor="#eeeeee"
                                                                                            AutoPostBack="true" OnTextChanged="txtcustomername_TextChanged"  CssClass="form-control"></asp:TextBox>
                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server"
                                                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                                                            MinimumPrefixLength="1" ServiceMethod="GetCompletionALLContactList" ServicePath=""
                                                                                            TargetControlID="txtcustomername" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                        </cc1:AutoCompleteExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                   <asp:Label ID="lblPersonName" runat="server" Text="Person Name"></asp:Label>
                                                                                        <asp:TextBox ID="txtPersonName" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                           <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblPersonMobileNo" runat="server" Text="Person Mobile No."></asp:Label>
                                                                                        <asp:TextBox ID="txtPersonMobileNo" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                           <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblPermanentMobileNo" runat="server" Text="<%$ Resources:Attendance,Permanent MobileNo.%>" />
                                                                                        <asp:TextBox ID="txtPermanentMobileNo" runat="server" CssClass="form-control" />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                                            TargetControlID="txtPermanentMobileNo" ValidChars="+,0,1,2,3,4,5,6,7,8,9">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="Label25" runat="server" Text="<%$ Resources:Attendance,Area Name  %>"></asp:Label>
                                                                                        <asp:TextBox ID="txtAreaName" TabIndex="104" BackColor="#eeeeee" runat="server"
                                                                                            CssClass="form-control" />
                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                                                            Enabled="True" ServiceMethod="GetCompletionListAreaName" ServicePath="" CompletionInterval="100"
                                                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtAreaName"
                                                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                        </cc1:AutoCompleteExtender>
                                                                                        <asp:HiddenField ID="hdnParentid" runat="server" Value="0" />
                                                                                        <br />
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row" id="pnlEmployee" visible="false" runat="server">
                                                                                <div class="col-md-12">                                                                                   
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="Label26" runat="server" Text="<%$ Resources:Attendance,Visit Date %>"></asp:Label>
                                                                                        <a style="color: Red">*</a>
                                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator14" ValidationGroup="Save"
                                                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtVisitDate" ErrorMessage="<%$ Resources:Attendance,Enter Visit Date%>"></asp:RequiredFieldValidator>

                                                                                        <asp:TextBox ID="txtVisitDate" runat="server" CssClass="form-control" />
                                                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtVisitDate" runat="server" TargetControlID="txtVisitDate">
                                                                                        </cc1:CalendarExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="Label27" runat="server" Text="<%$ Resources:Attendance,Visit Time %>"></asp:Label>
                                                                                        <a style="color: Red">*</a>
                                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator15" ValidationGroup="Save"
                                                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtVisitTime" ErrorMessage="<%$ Resources:Attendance,Enter Visit Time%>"></asp:RequiredFieldValidator>

                                                                                        <asp:TextBox ID="txtVisitTime" runat="server" CssClass="form-control" />
                                                                                        <cc1:MaskedEditExtender ID="MaskedEditExtender4" runat="server" CultureAMPMPlaceholder=""
                                                                                            CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                                            CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                            Enabled="True" ErrorTooltipEnabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtVisitTime"
                                                                                            UserTimeFormat="TwentyFourHour">
                                                                                        </cc1:MaskedEditExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="Label33" runat="server" Text="<%$ Resources:Attendance, Employee Name %>"></asp:Label>
                                                                                        <a style="color: Red">*</a>
                                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator17" ValidationGroup="Save"
                                                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtdrivername" ErrorMessage="<%$ Resources:Attendance,Enter Driver Name%>"></asp:RequiredFieldValidator>

                                                                                        <asp:TextBox ID="txtdrivername" runat="server" Font-Names="Verdana" AutoPostBack="true"
                                                                                            OnTextChanged="txtdrivername_TextChanged" CssClass="form-control" BackColor="#eeeeee"
                                                                                            TabIndex="5"></asp:TextBox>
                                                                                        <cc1:AutoCompleteExtender ID="txtEmpName_AutoCompleteExtender" runat="server" DelimiterCharacters=""
                                                                                            Enabled="True" ServiceMethod="GetCompletionListDriverName" ServicePath="" CompletionInterval="100"
                                                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtdrivername"
                                                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                        </cc1:AutoCompleteExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="Label32" runat="server" Text="<%$ Resources:Attendance,Vehicle Name %>" />
                                                                                        <a style="color: Red">*</a>
                                                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator16" ValidationGroup="Save"
                                                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtvehiclename" ErrorMessage="<%$ Resources:Attendance,Enter Vehicle Name%>"></asp:RequiredFieldValidator>

                                                                                        <asp:TextBox ID="txtvehiclename" runat="server" AutoPostBack="true"
                                                                                            OnTextChanged="txtvehiclename_TextChanged" BackColor="#eeeeee" CssClass="form-control" />
                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                                                                            Enabled="True" ServiceMethod="GetCompletionListVehicleName" ServicePath="" CompletionInterval="100"
                                                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtvehiclename"
                                                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                        </cc1:AutoCompleteExtender>
                                                                                        <br />
                                                                                    </div>                                                             
                                                                                </div>
                                                                            </div>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="Label24" runat="server" Text="<%$ Resources:Attendance,Chargeable Amount %>"></asp:Label>
                                                                                        <asp:TextBox ID="txtChargableAmount" runat="server" CssClass="form-control" />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                                                            TargetControlID="txtChargableAmount" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Label ID="lblTrackId" runat="server" Text="Tracking ID" ></asp:Label>
                                                                                        <asp:TextBox ID="txtTrakingId" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                    </div>
                                                                                    <div class="col-md-12">
                                                                                        <asp:Label ID="Label34" runat="server" Text="Description"></asp:Label>
                                                                                        <asp:TextBox ID="txtdescription" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                                                        <br />
                                                                                    </div>

                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>

                                                        </cc1:TabContainer>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: left">

                                                        <asp:Button ID="btnPost" runat="server" OnClick="btnPost_Click" Text="<%$ Resources:Attendance,Post & Update %>"
                                                            CssClass="btn btn-primaryOrder" Visible="false" />
                                                        <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="Are you sure to post the record ?"
                                                            TargetControlID="btnPost">
                                                        </cc1:ConfirmButtonExtender>

                                                        <asp:Button ID="btnSInvSave" runat="server" ValidationGroup="Save" CssClass="btn btn-success" OnClick="btnSInvSave_Click"
                                                            Visible="false" Text="<%$ Resources:Attendance,Save %>" OnClientClick="Confirm()" />

                                                        <asp:Button ID="BtnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                            CssClass="btn btn-primary" CausesValidation="False" OnClick="BtnReset_Click" />

                                                        <asp:Button ID="btnSInvCancel" runat="server" CssClass="btn btn-danger" Text="<%$ Resources:Attendance,Cancel %>"
                                                            CausesValidation="False" OnClick="btnSInvCancel_Click" />

                                                        <asp:HiddenField ID="editid" runat="server" />
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
                                                    <asp:Label ID="Label17" runat="server" Text="Advance Search"></asp:Label></h3>
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
                                                    <asp:DropDownList ID="ddlFieldNameBin" runat="server" CssClass="form-control" OnSelectedIndexChanged="SetCustomerTextBoxBin" AutoPostBack="true">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Invoice No %>" Value="Invoice_No" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Invoice Date %>" Value="Invoice_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Order No. %>" Value="OrderList"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Sales Person %>" Value="EmployeeName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Customer Name %>" Value="CustomerName"></asp:ListItem>
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
                                                <div class="col-lg-3">
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbindBin">
                                                        <asp:TextBox ID="txtValueBin" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <asp:TextBox ID="txtCustValueBin" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender_txtCustValueBin" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="0" ServiceMethod="GetCompletionListCustomer" ServicePath=""
                                                            TargetControlID="txtCustValueBin" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <asp:TextBox ID="txtValueBinDate" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueBinDate" runat="server" TargetControlID="txtValueBinDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbindBin" runat="server" CausesValidation="False" OnClick="btnbindBin_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefreshBin" runat="server" CausesValidation="False" OnClick="btnRefreshBin_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imgBtnRestore" CausesValidation="False" Visible="false" runat="server" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;


                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid" <%= GvSalesInvoiceBin.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvSalesInvoiceBin" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvSalesInvoiceBin_PageIndexChanging"
                                                        OnSorting="GvSalesInvoiceBin_OnSorting" AllowSorting="true">
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
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Invoice No. %>" SortExpression="Invoice_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvSInvNo" runat="server" Text='<%#Eval("Invoice_No") %>' />
                                                                    <asp:HiddenField ID="hdnTransId" runat="server" Value='<%#Eval("Trans_Id") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Invoice Date %>" SortExpression="Invoice_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvSInvDate" runat="server" Text='<%#GetDate(Eval("Invoice_Date").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order No. %>" SortExpression="OrderList">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvOrderList" runat="server" Text='<%#Eval("OrderList") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Sales Person %>" SortExpression="EmployeeName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvSalesPerson" runat="server" Text='<%#Eval("EmployeeName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer%>" SortExpression="CustomerName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvTransType" runat="server" Text='<%#Eval("CustomerName")%>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
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
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Amount%>" SortExpression="SIFromTransType">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvGrandtotal" runat="server" Text='<%# GetCurrencySymbol(Eval("GrandTotal").ToString(),Eval("Currency_Id").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                        </Columns>


                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                    <asp:HiddenField ID="HDFSortbin" runat="server" />
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                    <!-- /.box-body -->
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="Pending_Order">
                        <asp:UpdatePanel ID="Update_Pending_Order" runat="server">
                            <ContentTemplate>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div3" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label18" runat="server" Text="Advance Search"></asp:Label></h3>
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
                                                    <asp:DropDownList ID="ddlQSeleclField" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlQSeleclField_SelectedIndexChanged">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Order Id %>" Value="Trans_Id"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Order No. %>" Value="SalesOrderNo"
                                                            Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Order Date %>" Value="SalesOrderDate"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Transfer Type %>" Value="TransType"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Transfer No. %>" Value="QauotationNo"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Customer Name %>" Value="CustomerName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="CreatedUser"></asp:ListItem>
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
                                                <div class="col-lg-3">
                                                    <asp:Panel ID="Panel" runat="server" DefaultButton="ImgBtnQBind">
                                                        <asp:TextBox ID="txtQValue" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <asp:TextBox ID="txtQValueDate" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" Format="dd-MMM-yyyy" ID="CalendarExtendertxtQValue" runat="server" TargetControlID="txtQValueDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="ImgBtnQBind" runat="server" CausesValidation="False" OnClick="ImgBtnQBind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="ImgBtnQRefresh" runat="server" CausesValidation="False" OnClick="ImgBtnQRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid" <%= gvSalesOrder.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvSalesOrder" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' DataKeyNames="Trans_Id"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                        OnPageIndexChanging="gvPurchaseOrder_PageIndexChanging" OnSorting="gvPurchaseOrder_OnSorting">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order No. %>" SortExpression="SalesOrderNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbPONo" runat="server" Text='<%# Eval("SalesOrderNo") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Order Date %>" SortExpression="SalesOrderDate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPODate" runat="server" Text='<%# GetDateFromat(Eval("SalesOrderDate").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Transfer Type%>" SortExpression="TransType">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbSupId" runat="server" Text='<%# Eval("TransType").ToString() %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Transfer No. %>" SortExpression="QauotationNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOrderType" runat="server" Text='<%# Eval("QauotationNo")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Name %>" SortExpression="CustomerName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDeliveryDate" runat="server" Text='<%# Eval("CustomerName") %>'
                                                                        Width="150px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="150px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Product Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvtaxName" runat="server" Width="100px" Text='<%#Eval("ProductCode") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Unit Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvUnitName" runat="server" Width="80px" Text='<%#Eval("Unit_Name") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Sys Qty">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvsysqty" runat="server" Width="70px" Text='<%#SetDecimal(Eval("sysQty").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" Width="70px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Order Qty">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvorderqty" runat="server" Width="70px" Text='<%#SetDecimal(Eval("OrderQty").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" Width="70px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Remain Qty">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvRemainqty" runat="server" Width="70px" Text='<%#SetDecimal(Eval("RemainQty").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" Width="70px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="CreatedUser">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvUser" runat="server" Text='<%#Eval("CreatedUser") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Amount %>" SortExpression="NetAmount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPoAmount" runat="server" Text='<%# GetOrderCurrencySymbol(Eval("NetAmount").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
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
                    <div class="tab-pane" id="Job_Card">
                        <asp:UpdatePanel ID="Update_Job_Card" runat="server">
                            <ContentTemplate>
                                <div class="alert alert-info ">
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="ddlFieldNameQuote" runat="server" CssClass="form-control"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlFieldNameQuote_SelectedIndexChanged">
                                                    <asp:ListItem Selected="True" Text="Job No." Value="Job_No"></asp:ListItem>
                                                    <asp:ListItem Text="Job Date" Value="Job_date"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Customer Name %>" Value="CustomerName"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Handled Employee %>" Value="SalesPersonName"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Status %>" Value="Status"></asp:ListItem>
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
                                            <div class="col-lg-3">
                                                <asp:Panel ID="Panel3" runat="server" DefaultButton="btnbindQuote">
                                                    <asp:TextBox ID="txtValueQuote" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <asp:TextBox ID="txtValueQuoteDate" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueQuoteDate" runat="server" TargetControlID="txtValueQuoteDate" />
                                                </asp:Panel>
                                            </div>
                                            <div class="col-lg-2">
                                                <asp:ImageButton ID="btnbindQuote" runat="server" CausesValidation="False" Style="margin-top: -5px;"
                                                    ImageUrl="~/Images/search.png" OnClick="btnbindrptQuote_Click" ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>

                                                <asp:ImageButton ID="btnRefreshQuote" runat="server" CausesValidation="False" Style="width: 33px;"
                                                    ImageUrl="~/Images/refresh.png" OnClick="btnRefreshQuoteReport_Click"
                                                    ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                            </div>
                                            <div class="col-lg-2">
                                                <h5>
                                                    <asp:Label ID="lblTotalRecordsQuote" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title"></h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvCustomerInquiry" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvCustomerInquiry_PageIndexChanging"
                                                        AllowSorting="True" OnSorting="GvCustomerInquiry_Sorting">

                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btnEdit" runat="server" BorderStyle="None" BackColor="Transparent"
                                                                        CausesValidation="False" CssClass="btnPull" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        OnCommand="btnSIEdit_Command" Visible="false" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Job No" SortExpression="Job_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvworkNo" runat="server" Text='<%#Eval("Job_No") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Job Date" SortExpression="Job_date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblJobrDate" runat="server" Text='<%#GetDate(Eval("Job_date").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Expected End date" SortExpression="Expected_Job_End_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblExpJobrDate" runat="server" Text='<%#GetDate(Eval("Expected_Job_End_Date").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="End date" SortExpression="Job_End_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEndDate" runat="server" Text='<%#GetDate(Eval("Job_End_Date").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Customer Name %>" SortExpression="CustomerName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCustomerName" runat="server" Text='<%#Eval("CustomerName") %>' />
                                                                    <asp:Label ID="lblgvCustomerId" runat="server" Text='<%#Eval("Customer_Id") %>' Visible="false" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Handled Employee %>" SortExpression="SalesPersonName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvContactName" runat="server" Text='<%#Eval("SalesPersonName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Contact Name" SortExpression="ContactPersonName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvContactpersoneName" runat="server" Text='<%#Eval("ContactPersonName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status%>" SortExpression="Status">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCallType" runat="server" Text='<%#Eval("Status") %>' />
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
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="import_invoice">
                        <asp:UpdatePanel ID="update_import_invoice" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div4" runat="server" class="box box-info">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label20" runat="server" Text="Import Invoice Using Excel"></asp:Label></h3>
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
                                                        <asp:HyperLink ID="lnkDownloadSameExcel" runat="server" Font-Bold="true" Font-Size="15px"
                                                            NavigateUrl="~/CompanyResource/import_invoice_sample.xls" Text="Download sample excel format" Font-Underline="true"></asp:HyperLink>
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
                                                        <asp:Button ID="btnUploadExcel" runat="server" CssClass="btn btn-primary" OnClick="btnUploadExcel_Click" Text="Import Invoice" />
                                                        <asp:Button ID="btnSaveExcelInvoice" runat="server" CssClass="btn btn-primary" OnClick="btnSaveExcelInvoice_Click" Text="Update" Enabled="false" />
                                                    </div>
                                                </div>
                                                <br />
                                                <div style="overflow: auto; max-height: 300px;">
                                                    <asp:GridView ID="gvImportInvoiceList" DataKeyNames="validation_remark,merchant,customer_name,billing_address,billing_country,billing_state" runat="server"></asp:GridView>

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

    <div class="modal fade" id="Return_Modal" tabindex="-1" role="dialog" aria-labelledby="Return_ModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Return_ModalLabel">Serial No</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Modal" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="hdnSalesOrderId" runat="server" Value="0" />
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-12">
                                                    <asp:Label ID="lblproductId" runat="server" Text="Product Id"></asp:Label>
                                                    &nbsp:&nbsp<asp:Label ID="lblProductIdvalue" runat="server" Text="0"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <asp:Label ID="lblserialProductname" runat="server" Text="Product Name"></asp:Label>
                                                    &nbsp:&nbsp<asp:Label ID="lblProductNameValue" runat="server"
                                                        Text="0"></asp:Label>
                                                    <br />
                                                </div>
                                                <div id="pnlSerialNumber" runat="server" class="col-md-12">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label30" runat="server" Text="<%$ Resources:Attendance, File Upload%>"></asp:Label>
                                                        <div class="input-group" style="width: 100%;">
                                                            <cc1:AsyncFileUpload ID="FULogoPath"
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
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" style="text-align: center">
                                                        <asp:Button ID="Btnloadfile" runat="server" Text="Load" CssClass="btn btn-primary"
                                                            OnClick="Btnloadfile_Click" />

                                                        <asp:Button ID="btnexecute" runat="server" Text="Execute" CssClass="btn btn-primary"
                                                            OnClick="btnexecute_Click" />

                                                        <asp:Button ID="btnLoadTempSerial" runat="server" Text="Load Temp Serial" CssClass="btn btn-primary"
                                                            OnClick="btnLoadTempSerial_Click" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="alert alert-info ">
                                                            <div class="row">
                                                                <div class="form-group">
                                                                    <div class="col-lg-3">
                                                                        <asp:TextBox ID="txtserachserialnumber" runat="server" CssClass="form-control"></asp:TextBox>
                                                                        <cc1:TextBoxWatermarkExtender ID="txtwatermarkup" runat="server" TargetControlID="txtserachserialnumber"
                                                                            WatermarkText="Search Serial Number">
                                                                        </cc1:TextBoxWatermarkExtender>
                                                                    </div>
                                                                    <div class="col-lg-2">
                                                                        <asp:ImageButton ID="btnsearchserial" runat="server" CausesValidation="False"
                                                                            ImageUrl="~/Images/search.png" OnClick="btnsearchserial_Click" Style="margin-top: -5px;" ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>

                                                                        <asp:ImageButton ID="btnRefreshserial" runat="server" CausesValidation="False"
                                                                            ImageUrl="~/Images/refresh.png" OnClick="btnRefreshserial_Click" Style="width: 33px;"
                                                                            ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                                                    </div>
                                                                    <div class="col-lg-2">
                                                                        <h5>
                                                                            <asp:Label ID="Label31" runat="server" Text="Total :"></asp:Label>
                                                                            <asp:Label ID="txtselectedSerialNumber" runat="server" Text="0"></asp:Label></h5>
                                                                    </div>
                                                                    <div class="col-lg-2">
                                                                        <h5>
                                                                            <asp:Label ID="lblCount" runat="server"></asp:Label>
                                                                            <asp:Label ID="txtCount" runat="server" Text="0"></asp:Label></h5>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="box box-warning box-solid">
                                                            <div class="box-header with-border">
                                                                <h3 class="box-title"></h3>
                                                            </div>
                                                            <div class="box-body">
                                                                <div class="row">
                                                                    <div class="col-md-8">
                                                                        <div class="flow">
                                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvSerialNumber" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                                                                AllowSorting="true" BorderStyle="Solid" Width="100%"
                                                                                PageSize="5" OnSorting="gvSerialNumber_OnSorting">
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("SerialNo") %>'
                                                                                                ImageUrl="~/Images/Erase.png" OnCommand="IbtnDeleteserialNumber_Command" Width="16px" />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Serial Number" SortExpression="SerialNo">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblsrno" runat="server" Text='<%#Eval("SerialNo") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Manufacturing Date">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblmfg" runat="server" Text='<%#Eval("ManufacturerDate") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Batch No.">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblBatchNo" runat="server" Text='<%#Eval("BatchNo") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                </Columns>


                                                                                <PagerStyle CssClass="pagination-ys" />

                                                                            </asp:GridView>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <asp:TextBox ID="txtSerialNo" Height="350px" runat="server" CssClass="form-control"
                                                                            TextMode="MultiLine"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-md-8">
                                                                        <div class="flow">
                                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvSerialIssue" runat="server"
                                                                                BorderStyle="Solid" Width="100%">
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4">
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
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="Update_Modal_Button" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="BtnSerialSave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                CssClass="btn btn-success" OnClick="BtnSerialSave_Click" />
                            <asp:Button ID="btnResetSerial" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                CssClass="btn btn-primary" OnClick="btnResetSerial_Click" />

                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                            <asp:Button ID="btnDefault" runat="server" Style="visibility: hidden" />

                            <asp:Button ID="btnNotFound" runat="server" Text="Save Serial Issue"
                                Visible="false" CssClass="btn btn-success" OnClick="btnNotFound_Click" />

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

   

    <asp:UpdateProgress ID="UpdateProgress14" runat="server" AssociatedUpdatePanelID="UpdatePanel_Expenses">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_Modal">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress7" runat="server" AssociatedUpdatePanelID="Update_Modal_Button">
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

    <asp:UpdateProgress ID="UpdateProgress10" runat="server" AssociatedUpdatePanelID="update_import_invoice">
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
    <div class="modal fade" id="Modal_TransPort"  runat="server" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-body">
                    <%--<UCT:AddTransPort ID="AddTransModalPort" runat="server" />--%>
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


        function Li_Tab_Pending_Order() {
            document.getElementById('<%= Btn_Pending_Order.ClientID %>').click();
        }
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }
        function Li_Tab_Job_Card() {
            document.getElementById('<%= Btn_Pending_Job_Card.ClientID %>').click();
        }
        function Close_Modal_Popup() {
            document.getElementById('<%= Btn_View_Modal.ClientID %>').click();
        }
        function Show_Modal_Popup() {
            document.getElementById('<%= Btn_View_Modal.ClientID %>').click();
        }

        function Show_Modal_GST() {
            document.getElementById('<%= Btn_GST_Modal.ClientID %>').click();
        }

        function Hide_List() {
            var List_LI = document.getElementById("Li_List");
            List_LI.style.display = List_LI.style.display = 'none';
        }
        function Show_List() {
            var List_LI = document.getElementById("Li_List");
            List_LI.style.display = List_LI.style.display = '';
        }

        function Hide_Bin() {
            var Bin_LI = document.getElementById("Li_Bin");
            Bin_LI.style.display = Bin_LI.style.display = 'none';
        }
        function Show_Bin() {
            var Bin_LI = document.getElementById("Li_Bin");
            Bin_LI.style.display = Bin_LI.style.display = '';
        }

    </script>
    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do you want to print Invoice ?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }

        function DeliveryConfirm() {
            var confirm_value1 = document.createElement("INPUT");
            confirm_value1.type = "hidden";
            confirm_value1.name = "confirm_value1";
            if (confirm("Do you want to print delivery ?")) {
                confirm_value1.value = "Yes";
            } else {
                confirm_value1.value = "No";
            }
            document.forms[0].appendChild(confirm_value1);
        }

        function count(clientId) {
            var txtInput = document.getElementById(clientId);
            if (event.keyCode == 13) {
                document.getElementById('<%= txtCount.ClientID %>').innerHTML = lineBreakCount(txtInput.value);
            }
            if (event.keyCode == 8 || event.keyCode == 46) {
                document.getElementById('<%= txtCount.ClientID %>').innerHTML = lineDelBreakCount(txtInput.value); // The button id over here
            }
        }

        function lineBreakCount(str) {
            try {
                return ((str.match(/[^\n]*\n[^\n]*/gi).length) + 1);
            } catch (e) {
                return 1;
            }
        }

        function lineDelBreakCount(str) {
            try {
                return ((str.match(/[^\n]*\n[^\n]*/gi).length) - 1);
            }
            catch (e) {
                return 0;
            }
        }
        function Hide_Model_GST() {
            $('#btnClosePopup').click();
        }
    </script>
    <script type="text/javascript">
        function FUAll_UploadComplete(sender, args) {
            document.getElementById('<%= FUAll_Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= FUAll_Img_Right.ClientID %>').style.display = "";
        }
        function FUAll_UploadError(sender, args) {
            document.getElementById('<%= FUAll_Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= FUAll_Img_Wrong.ClientID %>').style.display = "";
        }
        function FUAll_UploadStarted(sender, args) {

        }
        function Show_Modal_Expenses_Tax() {
            document.getElementById('<%= Btn_Modal_Expenses_Tax.ClientID %>').click();
        }
        function Show_Expenses_Tax_Web_Control() {
            document.getElementById('<%= Btn_Show_Expenses_Tax.ClientID %>').click();
        }

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) { var oControl = args.get_postBackElement(); oControl.disabled = true; }

        function Modal_AcMaster_Open() {
            var _result = confirm("Account not exist in " + $('#<%= ddlCurrency.ClientID %> option:selected').text() + ". Do you want to create it");
            if (_result == true) {
                $('#ModelAcMaster').modal('show');
            }
        }
        //Initialize tooltip with jQuery
        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip({
                'selector': '',
                'placement': 'top',
                'container': 'body'
            });
        });

        function showUcControlsSettings() {
            $('#ControlSettingModal').modal('show');
        }

        function txtContactPerson_TextChanged(ctrl) {


            if (ctrl.value == "") {
                ctrl.value = "";
                document.getElementById('<%= hdnContactId.ClientID %>').value = "";
                return;
            }
            var id = ctrl.value.split('/')[3];
            var phone = ctrl.value.split('/')[2];
            var email = ctrl.value.split('/')[1];
            var name = ctrl.value.split('/')[0];
            if (!$.isNumeric(id)) {
                alert("Contact Person is not valid");
                ctrl.value = "";
                document.getElementById('<%= hdnContactId.ClientID %>').value = "";
                return;
            }
            var id1 = getContactIdFromNameNId(name, id);
            document.getElementById('<%= hdnContactId.ClientID %>').value = id1;
            if (id.trim() != id1.trim()) {
                alert("Contact Person is not valid");
                ctrl.value = "";
                document.getElementById('<%= hdnContactId.ClientID %>').value = "";
            }
        }

        function Modal_NewAddress_Open() {
            document.getElementById('<%= Btn_NewAddress.ClientID %>').click();
        }

        function Modal_TransPort_Open() {

            $('#<%= Modal_TransPort.ClientID %>').modal('show');
        }


        function resetPosition1() {

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


        function redirectToHome(msg) {
            if (confirm(msg)) {
                window.open('../MasterSetup/Home.aspx', 'window', 'width=1024,');
                return true;
            }
            else {
                return false;
            }
        }
        function getReportData(transId) {
            $("#prgBar").css("display", "block");

            document.getElementById('<%= reportSystem.FindControl("hdnTransId").ClientID %>').value = transId;
            document.getElementById('<%= reportSystem.FindControl("hdnPageRef").ClientID %>').value = "SI";
            debugger;
            setReportData();
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

        function SetSalesTransport(CommandName, CommandArgument) {

            var commandName = CommandName;
            var commandArgument = CommandArgument;
            var url = '../Sales/SalesTransPort.aspx?CommandName=' + encodeURIComponent(commandName) + '&CommandArgument=' + encodeURIComponent(commandArgument);
            window.open(url, 'window', 'width=1024');

        }

        function Modal_CustomerInfo_Open() {
            $('#modelContactDetail').modal('show');
        }
    </script>
    <script src="../Script/ReportSystem.js"></script>
    <script src="../Script/customer.js"></script>
</asp:Content>

