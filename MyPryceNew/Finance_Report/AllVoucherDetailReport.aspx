<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true"
    CodeFile="AllVoucherDetailReport.aspx.cs" EnableEventValidation="true"
    Inherits="Finance_Report_AllVoucherDetailReport" %>

<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/sales_report.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="Voucher Detail Report"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="Voucher Report"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="Voucher Detail Report"></asp:Label></li>
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
                            <div class="col-md-4">
                                <asp:Label ID="Label27" runat="server" Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control"></asp:TextBox>
                                <cc1:CalendarExtender ID="txtFrom_CalendarExtender" runat="server" Enabled="True"
                                    TargetControlID="txtFromDate">
                                </cc1:CalendarExtender>
                                <br />
                            </div>
                            <div class="col-md-4">
                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,To Date %>"></asp:Label>
                                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtToDate">
                                </cc1:CalendarExtender>
                                <br />
                            </div>
                            <div class="col-md-4">
                                <asp:Label ID="Label1" runat="server" Text="Voucher Type"></asp:Label>
                                <asp:DropDownList runat="server" ID="ddlReportType" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged">
                                    <asp:ListItem Text="Journal Voucher" Value="JV"></asp:ListItem>
                                    <asp:ListItem Text="Receive Voucher" Value="RV"></asp:ListItem>
                                    <asp:ListItem Text="Payment Voucher" Value="PV"></asp:ListItem>
                                    <asp:ListItem Text="Customer Credit Note" Value="CCN"></asp:ListItem>
                                    <asp:ListItem Text="Customer Receive Voucher" Value="CRV"></asp:ListItem>
                                    <asp:ListItem Text="Supplier Payment Voucher" Value="SPV"></asp:ListItem>
                                </asp:DropDownList>
                                <br />
                            </div>

                            <div class="col-md-6" id="divCustomer" runat="server" visible="false">
                                <asp:Label ID="lblAccountName" runat="server" Text="<%$ Resources:Attendance,Customer%>" />
                                <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control" BackColor="#eeeeee" />
                                <cc1:AutoCompleteExtender ID="txtCustomerName_AutoCompleteExtender" runat="server" ClientIDMode="Static"
                                    CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                    ServiceMethod="GetCompletionListCustomer" ServicePath="" TargetControlID="txtCustomerName"
                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                </cc1:AutoCompleteExtender>
                                <br />
                            </div>
                            <div class="col-md-6" id="divSalesPerson" runat="server" visible="false">
                                <asp:Label ID="lblHandledEmployee" runat="server" Text="Sales Employee" />
                                <asp:TextBox ID="txtHandledEmployee" BackColor="#eeeeee" runat="server" CssClass="form-control"
                                    OnTextChanged="txtHandledEmployee_TextChanged" AutoPostBack="true" />
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender6" ServiceMethod="GetCompletionListEmployeeName"
                                    runat="server" DelimiterCharacters="" Enabled="True" TargetControlID="txtHandledEmployee"
                                    ServicePath="" CompletionInterval="100" MinimumPrefixLength="1" CompletionSetCount="1"
                                    CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                    CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition"
                                    UseContextKey="True">
                                </cc1:AutoCompleteExtender>
                                <br />
                            </div>
                            <div class="col-md-12" id="divSupplier" runat="server" visible="false">
                                <asp:Label ID="lblSupplier" runat="server" Text="<%$ Resources:Attendance,Supplier %>" />
                                <asp:TextBox ID="txtSupplierName" runat="server" CssClass="form-control" BackColor="#eeeeee" />
                                <cc1:AutoCompleteExtender ID="txtSupplierName_AutoCompleteExtender" runat="server"
                                    CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                    ServiceMethod="GetCompletionList" ServicePath="" TargetControlID="txtSupplierName"
                                    UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                </cc1:AutoCompleteExtender>
                                <br />
                            </div>
                            <div class="col-md-12">
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

                                <br />
                            </div>

                            <div class="col-md-12" style="text-align: center">

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


                                <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server" GridViewID="gvExportData"></dx:ASPxGridViewExporter>
                                <dx:ASPxGridView ID="gvExportData" Width="100%" ClientInstanceName="grid" runat="server">
                                    <Settings ShowGroupPanel="true" ShowFilterRow="true" />
                                    <Settings ShowFooter="true" />
                                </dx:ASPxGridView>
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
