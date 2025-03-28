<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" EnableEventValidation="true"
    AutoEventWireup="true" CodeFile="SalesInvoice.aspx.cs" Inherits="Sales_Report_SalesInvoice" %>

<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/sales_report.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Sales Invoice Report %>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Sales Report%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Sales Report%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Sales Invoice Report%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:UpdatePanel ID="Update_New" runat="server">
        <Triggers>

            <asp:PostBackTrigger ControlID="BtnExportPDF" />
            <asp:PostBackTrigger ControlID="BtnExportExcel" />
        </Triggers>
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

                            <div class="col-md-12" style="text-align: center">
                                <asp:RadioButton Style="margin-left: 20px; margin-right: 20px;" Checked="true" AutoPostBack="true" ID="rbtnheader"
                                    runat="server" GroupName="a" Text="<%$ Resources:Attendance,Header Report %>"
                                    OnCheckedChanged="rbtnheader_CheckedChanged" />
                                <asp:RadioButton Style="margin-left: 20px; margin-right: 20px;" GroupName="a" AutoPostBack="true" ID="RbtnDetail"
                                    runat="server" Text="<%$ Resources:Attendance,Detail Report %>" OnCheckedChanged="RbtnDetail_CheckedChanged" />
                                <asp:RadioButton Style="margin-left: 20px; margin-right: 20px;" GroupName="a" AutoPostBack="true" ID="RbtnVatHeader"
                                    runat="server" Text="Vat Report" OnCheckedChanged="rbtnheader_CheckedChanged" Visible="false" />
                                <asp:RadioButton Style="margin-left: 20px; margin-right: 20px;" GroupName="a" AutoPostBack="true" ID="rbtnheaderDailySales"
                                    runat="server" Text="Daily Sales Report" OnCheckedChanged="rbtnheader_CheckedChanged" />

                                <asp:RadioButton Style="margin-left: 20px; margin-right: 20px;" GroupName="a" AutoPostBack="true" ID="rbtnSummary"
                                    runat="server" Text="Summary Reports" OnCheckedChanged="rbtnheader_CheckedChanged" />

                                <br />
                            </div>

                            <div class="col-md-6">
                                <asp:Label ID="Label27" runat="server" Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control"></asp:TextBox>
                                <cc1:CalendarExtender ID="txtFrom_CalendarExtender" runat="server" Enabled="True"
                                    TargetControlID="txtFromDate">
                                </cc1:CalendarExtender>
                                <br />
                            </div>
                            <div class="col-md-6">
                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,To Date %>"></asp:Label>
                                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtToDate">
                                </cc1:CalendarExtender>
                                <br />
                            </div>


                            <div runat="server" id="divGrid" visible="false">
                                <div class="col-md-6" id="Location" runat="server" visible="false">
                                    <asp:DropDownList ID="ddlLocation" runat="server" Class="form-control">
                                    </asp:DropDownList>
                                    <br />
                                </div>

                                <div class="col-md-6" id="ReportType" visible="false" runat="server">
                                    <asp:DropDownList runat="server" ID="ddlReportType" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged">
                                        <asp:ListItem Text="Summary Product wise sales report" Value="SPWSR"></asp:ListItem>
                                        <asp:ListItem Text="Summary Customer Product wise sales report" Value="SPWSRC"></asp:ListItem>
                                        <asp:ListItem Text="Summary Category wise sales report" Value="SCWSR"></asp:ListItem>
                                        <asp:ListItem Text="Summary Daily sales report" Value="SDSR"></asp:ListItem>
                                        <asp:ListItem Text="Summary Sales Order and Delivery By Customer" Value="SSOADBC"></asp:ListItem>
                                        <asp:ListItem Text="Summary Sales report by employee" Value="SSRBE"></asp:ListItem>
                                     <%--   <asp:ListItem Text="Cusomter Evaluation Report" Value="CER"></asp:ListItem>
                                        <asp:ListItem Text="Product Statics Report" Value="PSR"></asp:ListItem>--%>
                                        <asp:ListItem Text="Sales Invoice Detail Report" Value="SIDR"></asp:ListItem>
                                     <%--   <asp:ListItem Text="Sales Invoice Cancel Report" Value="SICR"></asp:ListItem>--%>
                                        <asp:ListItem Text="Invoice & Return Product Report" Value="IRPR"></asp:ListItem>
                                        <asp:ListItem Text="Detail Sales By Product with Profit" Value="DSPP"></asp:ListItem>
                                        <%-- <asp:ListItem Text="Summary report by brand" Value="SRBB"></asp:ListItem>--%>
                                        <asp:ListItem Text="Product Expiry Report" Value="PER"></asp:ListItem>
                                    </asp:DropDownList>
                                    <br />
                                </div>

                                <div class="col-md-6" id="CustomerName" runat="server" visible="false">
                                    <asp:Label ID="lblCustomerName" runat="server" Text="<%$ Resources:Attendance,Customer Name %>" />
                                    <asp:TextBox ID="txtCustomer" runat="server" CssClass="form-control" OnTextChanged="txtCustomer_TextChanged"
                                        BackColor="#eeeeee" AutoPostBack="true" />
                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server"
                                        DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                        MinimumPrefixLength="1" ServiceMethod="GetCompletionListCustomer" ServicePath=""
                                        TargetControlID="txtCustomer" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                    </cc1:AutoCompleteExtender>
                                    <br />
                                </div>
                                <div class="col-md-6" id="SalesPerson" runat="server" visible="false">
                                    <asp:Label ID="lblSummarySalesPerson" runat="server" Text="<%$ Resources:Attendance,Sales Person%>" />
                                    <asp:TextBox ID="txtSummarySalesPerson" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                        OnTextChanged="txtSalesPerson_TextChanged" AutoPostBack="true" />
                                    <cc1:AutoCompleteExtender ID="txtSalesPerson_AutoCompleteExtender" runat="server"
                                        DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                        MinimumPrefixLength="1" ServiceMethod="GetCompletionListEmployeeName" ServicePath=""
                                        TargetControlID="txtSummarySalesPerson" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                    </cc1:AutoCompleteExtender>
                                    <br />
                                </div>

                                <div visible="false" class="col-md-6" id="DivCustomerName" runat="server">
                                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,Customer Name %>"></asp:Label>
                                    <asp:TextBox ID="txtCustomerNameSummery" runat="server" CssClass="form-control" BackColor="#eeeeee" AutoPostBack="True" OnTextChanged="txtCustomerName_TextChanged" />
                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server"
                                        CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                        ServiceMethod="GetCompletionListCustomer" ServicePath="" TargetControlID="txtCustomerNameSummery"
                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                    </cc1:AutoCompleteExtender>
                                    <br />
                                </div>
                            </div>

                            <div runat="server" id="divReports" visible="false">

                                <div class="col-md-6" id="InvoiceType" runat="server" visible="false">
                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Invoice Type %>"></asp:Label>
                                    <asp:DropDownList ID="ddlInvoiceType" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="--Select--"></asp:ListItem>
                                        <asp:ListItem Text="Direct" Value="Direct"></asp:ListItem>
                                        <asp:ListItem Text="By SalesOrder" Value="By SalesOrder"></asp:ListItem>
                                    </asp:DropDownList>
                                    <br />
                                </div>
                                <div class="col-md-6" id="InvoiceNo" runat="server" visible="false">
                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Invoice No.%>"></asp:Label>
                                    <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control"></asp:TextBox>
                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetCompletionListInvoiceNo" ServicePath="" CompletionInterval="100"
                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtInvoiceNo"
                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                    </cc1:AutoCompleteExtender>
                                    <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
                                    <br />
                                </div>
                                <div class="col-md-6" id="OrderNo" runat="server" visible="false">
                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Order No.%>"></asp:Label>
                                    <asp:TextBox ID="txtOrderNo" BackColor="#eeeeee" runat="server" CssClass="form-control"
                                        AutoPostBack="True" OnTextChanged="txtOrderNo_TextChanged"></asp:TextBox>
                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetCompletionListOrderNo" ServicePath="" CompletionInterval="100"
                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtOrderNo" UseContextKey="True"
                                        CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                    </cc1:AutoCompleteExtender>
                                    <asp:HiddenField ID="hdnOrderId" runat="server" Value="0" />
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Customer Name %>"></asp:Label>
                                    <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                        AutoPostBack="True" OnTextChanged="txtCustomerName_TextChanged" />
                                    <cc1:AutoCompleteExtender ID="txtCustomerName_AutoCompleteExtender" runat="server"
                                        CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                        ServiceMethod="GetCompletionListCustomer" ServicePath="" TargetControlID="txtCustomerName"
                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                    </cc1:AutoCompleteExtender>
                                    <asp:HiddenField ID="hdnCustomerId" runat="server" Value="0" />
                                    <br />
                                </div>
                               
                                <div class="col-md-6" id="GroupBy" runat="server" visible="false">
                                    <asp:Label ID="Label8" runat="server"
                                        Text="Group By"></asp:Label>
                                    <asp:DropDownList ID="ddlGroupBy" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="Invoice Date" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Customer Name" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Sales Person" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                    <br />
                                </div>
                                <div class="col-md-6" id="Merchent" visible="false" runat="server">
                                    <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Attendance,Merchant Name %>" />
                                    <asp:DropDownList ID="ddlMerchant" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                    <br />
                                </div>
                                <div class="col-md-6" id="OrderBy" runat="server" visible="false">
                                    <asp:Label ID="Label9" runat="server" Text="Order By" />
                                    <asp:DropDownList ID="ddlorderby" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="Ascending" Value="Asc"></asp:ListItem>
                                        <asp:ListItem Text="Descending" Value="Desc"></asp:ListItem>
                                    </asp:DropDownList>
                                    <br />
                                </div>
                                <div class="col-md-6" id="State" runat="server" visible="false">
                                    <asp:Label ID="Label11" runat="server" Text="State Name" />
                                    <asp:TextBox ID="txtStateName" runat="server" CssClass="form-control" />
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblSalesPerson" runat="server" Text="<%$ Resources:Attendance,Sales Person %>" />
                                    <asp:TextBox ID="txtSalesPerson" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                        OnTextChanged="txtSalesPerson_TextChanged" AutoPostBack="true" />
                                    <cc1:AutoCompleteExtender ID="txtHandledEmp_AutoCompleteExtender" runat="server"
                                        DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                        MinimumPrefixLength="1" ServiceMethod="GetCompletionListEmployeeName" ServicePath=""
                                        TargetControlID="txtSalesPerson" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                    </cc1:AutoCompleteExtender>
                                    <br />
                                </div>
                                <div class="col-md-6" id="Group" runat="server" visible="false">
                                    <br />
                                    <asp:CheckBox ID="chkGroupBy" CssClass="form-control" runat="server" Text="Group Report" />
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <br />
                                </div>
                            </div>

                             <div id="trProduct" runat="server" visible="false" class="col-md-12">
                                    <asp:Label ID="lblProductName" Visible="false" runat="server"
                                        Text="<%$ Resources:Attendance,Product Name %>"></asp:Label>
                                    <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" AutoPostBack="true"
                                        OnTextChanged="txtProductName_TextChanged" Visible="false" BackColor="#eeeeee" />
                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetCompletionListProductName" ServicePath="" CompletionInterval="100"
                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtProductName"
                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                    </cc1:AutoCompleteExtender>
                                    <asp:HiddenField ID="hdnProductId" runat="server" Value="0" />
                                    <br />
                                </div>

                            <div id="divSalesType">
                                <asp:RadioButton ID="rBtnTypeAll" runat="server" Text="All Sales" GroupName="SalesType" />
                                <asp:RadioButton ID="rBtnTypePosted" runat="server" Text="Posted Sales" GroupName="SalesType" />
                                <asp:RadioButton ID="rBtnTypeUnPosted" runat="server" Text="Unposted Sales" GroupName="SalesType" />

                            </div>

                            <div class="col-md-12" style="text-align: center">
                                <asp:Button ID="btngo" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Go %>" Visible="true" CssClass="btn btn-primary" OnClick="btngo_Click" />
                                <asp:Button ID="btnFillgrid" runat="server" CausesValidation="False" Text="Get Detail" Visible="true" CssClass="btn btn-primary" OnClick="btnFillgrid_Click" />
                                <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>" CssClass="btn btn-primary" OnClick="btnReset_Click" CausesValidation="False" />
                                <asp:Button ID="BtnExportPDF" CssClass="btn btn-primary" runat="server" Text="Export PDF" OnClick="BtnExportPDF_Click" />
                                <asp:Button ID="BtnExportExcel" CssClass="btn btn-primary" runat="server" Text="Export Excel" OnClick="BtnExportExcel_Click" />
                                <br />
                            </div>

                        </div>
                    </div>
                </div>
            </div>

            <div class="box box-warning box-solid" <%= gvExportData.VisibleRowCount>0?"style='display:block'":"style='display:none'"%>>
                <div class="box-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div style="overflow: auto; max-height: 500px;">
                                <asp:Label ID="lblTotalProfit" runat="server" CssClass="form-control" />

                                <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server" GridViewID="gvExportData"></dx:ASPxGridViewExporter>
                                <dx:ASPxGridView ID="gvExportData" Width="100%" ClientInstanceName="grid" runat="server">
                                    <Settings ShowGroupPanel="true" ShowFilterRow="true" />
                                    <Settings ShowFooter="true" />
                                </dx:ASPxGridView>

                            <%--    <asp:GridView ID="gvSalesData" runat="server" AutoGenerateColumns="true">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnSupplierId" runat="server" Value='<%#Eval("Supplier_Id") %>' />
                                                <asp:GridView ID="gvChildData" runat="server" AutoGenerateColumns="true">

                                                </asp:GridView>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>




        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_New">
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
    </script>
</asp:Content>
