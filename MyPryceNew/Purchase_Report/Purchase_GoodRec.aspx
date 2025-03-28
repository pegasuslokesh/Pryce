<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="Purchase_GoodRec.aspx.cs" Inherits="Purchase_Report_Purchase_GoodRec" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/purchase_report.png" alt="" style="width: 25px;" />
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Purchase Goods Receive Report%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Purchase Report%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Purchase Goods Receive Report%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Filter" runat="server">
        <ContentTemplate>
            <div class="box box-primary">
                <div class="box-header with-border">
                    <h3 class="box-title">Filter</h3>
                    <div class="box-tools pull-right">
                        <button type="button" class="btn btn-box-tool" data-widget="collapse" data-toggle="tooltip">
                            <i class="fa fa-minus"></i>
                        </button>
                    </div>
                </div>
                <div class="box-body">
                    <div class="row"> 
                            <div class="col-md-6">
                                <asp:Label ID="Label27" runat="server" Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control"></asp:TextBox>
                                <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtFrom_CalendarExtender" runat="server" Enabled="True"
                                    TargetControlID="txtFromDate">
                                </cc1:CalendarExtender>
                                <br />
                            </div>
                            <div class="col-md-6">
                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,To Date %>"></asp:Label>
                                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control"></asp:TextBox>
                                <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtToDate">
                                </cc1:CalendarExtender>
                                <br />
                            </div>
                        <div class="col-md-6">
                            <asp:Label ID="Label7" runat="server"  Text="<%$ Resources:Attendance,Invoice Type%>"></asp:Label>
                            <asp:DropDownList ID="ddlInvoiceType" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="--Select--"></asp:ListItem>
                                                        <asp:ListItem Text="Direct" Value="WOutPO"></asp:ListItem>
                                                        <asp:ListItem Text="InDirect" Value="PO"></asp:ListItem>
                                                    </asp:DropDownList>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="Label8" runat="server"  Text="<%$ Resources:Attendance,Invoice No %>"></asp:Label>
                            <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                        AutoPostBack="True" OnTextChanged="txtInvoiceNo_TextChanged" />
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" CompletionInterval="100"
                                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionList_InvoiceNo"
                                                        ServicePath="" TargetControlID="txtInvoiceNo" UseContextKey="True"  CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                    </cc1:AutoCompleteExtender>
                                                    <asp:HiddenField ID="hdnInvoiceNo" runat="server" Value="0" />
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="Label2" runat="server"  Text="<%$ Resources:Attendance,Supplier Invoice No %>"></asp:Label>
                            <asp:TextBox ID="txtSupInvoiceNo" runat="server" CssClass="form-control" ></asp:TextBox>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="Label1" runat="server"  Text="<%$ Resources:Attendance,Supplier Name %>"></asp:Label>
                            <asp:TextBox ID="txtSupplierName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                        AutoPostBack="True" OnTextChanged="txtSupplierName_TextChanged" />
                                                    <cc1:AutoCompleteExtender ID="txtSupplierName_AutoCompleteExtender" runat="server"
                                                        CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                                        ServiceMethod="GetCompletionList" ServicePath="" TargetControlID="txtSupplierName"
                                                        UseContextKey="True"  CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                    </cc1:AutoCompleteExtender>
                                                    <asp:HiddenField ID="hdnSupplierId" runat="server" Value="0" />
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="Label9" runat="server"  Text="<%$ Resources:Attendance,Order No. %>"></asp:Label>
                            <asp:TextBox ID="txtOrderNo" BackColor="#eeeeee" runat="server" CssClass="form-control"
                                                        AutoPostBack="True" OnTextChanged="txtOrderNo_TextChanged"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetCompletionListOrderNo" ServicePath="" CompletionInterval="100"
                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtOrderNo" UseContextKey="True"
                                                         CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                    </cc1:AutoCompleteExtender>
                                                    <asp:HiddenField ID="hdnOrderNo" runat="server" Value="0" />
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="Label11" runat="server"  Text="<%$ Resources:Attendance,Status %>"></asp:Label>
                            <asp:DropDownList ID="ddlPostStatus" runat="server" CssClass="form-control" >
                                                        <asp:ListItem Text="--Select--"></asp:ListItem>
                                                        <asp:ListItem Text="Post" Value="True"></asp:ListItem>
                                                        <asp:ListItem Text="UnPost" Value="False"></asp:ListItem>
                                                    </asp:DropDownList>
                            <br />
                        </div>
                        <div id="trProduct">
                            <div class="col-md-12" id="tdProductName">
                                <asp:Label ID="lblProductName" runat="server"
                                    Text="<%$ Resources:Attendance,Product Name %>"></asp:Label>
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
                            <div class="col-md-12">
                                <asp:Label ID="Label4" runat="server" Style="padding: 15px 15px 0px 0px" Text="<%$ Resources:Attendance,Group By%>"></asp:Label>:                                
                                <asp:RadioButton ID="rbtnInvoiceNo" runat="server" GroupName="a" 
                                                        Text="<%$ Resources:Attendance,Invoice No %>" />
                                                    <asp:RadioButton ID="rbtnOrderNo" runat="server" GroupName="a" 
                                                        Text="<%$ Resources:Attendance,Purchase Order No %>" />
                                <br />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="box-footer">
                    <div class="col-md-12" style="text-align: center">
                        <asp:Button ID="btngo" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Go %>"
                            CssClass="btn btn-primary" OnClick="btngo_Click" />

                        <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                            CssClass="btn btn-primary" OnClick="btnReset_Click" CausesValidation="False" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="Update_Report" runat="server">
        <ContentTemplate>
            <div id="Div_Report" runat="server" visible="false" class="box box-primary">
                <div class="box-header with-border">
                    <h3 class="box-title">Report</h3>

                    <div class="box-tools pull-right">
                        <button type="button" class="btn btn-box-tool" data-widget="collapse" data-toggle="tooltip">
                            <i class="fa fa-minus"></i>
                        </button>
                    </div>
                </div>
                <div class="box-body">
                    <div class="row">
                        <div class="col-md-=12" style="text-align: center">
                            <%--<asp:Button ID="btnSend" runat="server" Text="Send" CssClass="btn btn-primary" OnClick="btnSend_Click" />--%>
                        </div>
                        <div class="col-md-12">
                            <br />
                        </div>
                        <div class="col-md-12">
                            <div style="overflow: auto">
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
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div style="overflow: auto">
                                <dx:ReportViewer ID="rptViewer" Width="100%" runat="server">
                                </dx:ReportViewer>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Filter">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_Report">
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
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server"><script src="../Script/common.js"></script>
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
