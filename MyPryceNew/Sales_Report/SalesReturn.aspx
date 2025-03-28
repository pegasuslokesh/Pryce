<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="SalesReturn.aspx.cs" Inherits="Sales_Report_SalesReturn" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/sales_report.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Sales Return Report %>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Sales Report%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Sales Report%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Sales Return Report%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_New" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-md-12" style="text-align: center">
                                    <asp:RadioButton ID="rbInvoiceDate" runat="server" GroupName="Date" OnCheckedChanged="rbInvoiceDate_CheckedChanged" AutoPostBack="true" Text="Data By Invoice Date" />
                                    <asp:RadioButton ID="rbReturnDate" runat="server" GroupName="Date" OnCheckedChanged="rbInvoiceDate_CheckedChanged" AutoPostBack="true" Text="Data By Return Date" />

                                </div>
                                <div class="col-md-6" id="InvoiceFromDate" runat="server" visible="false">
                                    <asp:Label ID="Label7" runat="server" Text="Invoice From Date"></asp:Label>
                                    <asp:TextBox ID="txtInvoiceFromDate" runat="server" CssClass="form-control"></asp:TextBox>
                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True"
                                        TargetControlID="txtInvoiceFromDate">
                                    </cc1:CalendarExtender>
                                    <br />
                                </div>
                                <div class="col-md-6" id="InvoiceToDate" runat="server" visible="false">
                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,To Date %>"></asp:Label>
                                    <asp:TextBox ID="txtInvoiceToDate" runat="server" CssClass="form-control"></asp:TextBox>
                                    <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" TargetControlID="txtInvoiceToDate">
                                    </cc1:CalendarExtender>
                                    <br />
                                </div>
                                <div id="Returndate" runat="server" visible="false">
                                    <div class="col-md-6">
                                        <asp:Label ID="Label27" runat="server" Text="Return From Date"></asp:Label>
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

                                <div class="col-md-6">

                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Invoice Type %>"></asp:Label>
                                    <asp:DropDownList ID="ddlInvoiceType" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="--Select--"></asp:ListItem>
                                        <asp:ListItem Text="Direct" Value="D"></asp:ListItem>
                                        <asp:ListItem Text="By SalesOrder" Value="S"></asp:ListItem>
                                    </asp:DropDownList>
                                    <br />
                                </div>
                                <div class="col-md-6">

                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Invoice No.%>"></asp:Label>
                                    <asp:TextBox ID="txtInvoiceNo" BackColor="#eeeeee" runat="server" CssClass="form-control"
                                        AutoPostBack="True" OnTextChanged="txtInvoiceNo_TextChanged"></asp:TextBox>
                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetCompletionListInvoiceNo" ServicePath="" CompletionInterval="100"
                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtInvoiceNo"
                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                    </cc1:AutoCompleteExtender>
                                    <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
                                    <br />
                                </div>
                                <div class="col-md-6">
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
                                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Customer Name %>"></asp:Label>
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
                                <div class="col-md-12" id="trProduct">
                                    <asp:Label ID="lblProductName" runat="server" Text="<%$ Resources:Attendance,Product Name %>"></asp:Label>
                                    <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" AutoPostBack="true"
                                        OnTextChanged="txtProductName_TextChanged" BackColor="#eeeeee" />
                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetCompletionListProductName" ServicePath="" CompletionInterval="100"
                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtProductName"
                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                    </cc1:AutoCompleteExtender>
                                    <asp:HiddenField ID="hdnProductId" runat="server" Value="0" />
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:CheckBox ID="chkGroupBy" runat="server" Text="Group Report" CssClass="form-control" />
                                    <br />
                                </div>


                                <div class="col-md-12" style="text-align: center">
                                    <asp:Button ID="btngo" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Go %>"
                                        CssClass="btn btn-primary" Visible="true" OnClick="btngo_Click" />

                                    <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                        CssClass="btn btn-primary" OnClick="btnReset_Click" CausesValidation="False" />


                                    <br />
                                </div>
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
