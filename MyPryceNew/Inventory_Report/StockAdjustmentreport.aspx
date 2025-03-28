<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="StockAdjustmentreport.aspx.cs" Inherits="Inventory_Report_StockAdjustmentreport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/product_ledger.png" alt="" style="width: 25px;" />
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Stock Adjustment Report%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Inventory Report%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Stock Adjustment Report%>"></asp:Label></li>
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
                            <asp:Label ID="lblFromDate" runat="server"  Text="<%$ Resources:Attendance,From Date  %>"></asp:Label>
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control"></asp:TextBox>
                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtFromDate_CalendarExtender" runat="server" TargetControlID="txtFromDate">
                            </cc1:CalendarExtender>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblToDate" runat="server"  Text="<%$ Resources:Attendance,To Date  %>"></asp:Label>
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control"></asp:TextBox>
                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtToDate_CalendarExtender" runat="server" TargetControlID="txtToDate">
                            </cc1:CalendarExtender>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="Label9" runat="server"  Text="<%$ Resources:Attendance,Product Id%>" />
                            <asp:TextBox ID="txtProductcode" BackColor="#eeeeee" runat="server" CssClass="form-control" AutoPostBack="True"
                                OnTextChanged="txtProductCode_TextChanged"/>

                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="100"
                                DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductCode"
                                ServicePath="" TargetControlID="txtProductcode" UseContextKey="True"  CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                            </cc1:AutoCompleteExtender>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblProductName" runat="server"  Text="<%$ Resources:Attendance,Product Name %>" />
                            <asp:TextBox ID="txtProductName" BackColor="#eeeeee" runat="server" CssClass="form-control" AutoPostBack="true"
                                OnTextChanged="txtProductName_TextChanged" />
                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="GetCompletionListProductName" ServicePath="" CompletionInterval="100"
                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtProductName"
                                UseContextKey="True"  CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                            </cc1:AutoCompleteExtender>
                            <asp:HiddenField ID="hdnNewProductId" runat="server" Value="0" />
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="Label12" runat="server"  Text="<%$ Resources:Attendance,Location  %>"></asp:Label>
                            <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Post Status %>"></asp:Label>
                            <asp:DropDownList ID="ddlPosted" runat="server" CssClass="form-control">
                                                                 <asp:ListItem Text="<%$ Resources:Attendance,All %>" Value="0" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Posted %>" Value="True"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,UnPosted %>" Value="False" ></asp:ListItem>
                                                            </asp:DropDownList>
                            <br />
                        </div>
                    </div>
                </div>
                <div class="box-footer">
                    <div class="col-md-12" style="text-align: center">
                        <asp:Button ID="btngo" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Go %>"
                            CssClass="btn btn-primary" OnClick="btngo_Click" />

                        <asp:Button ID="btnReset" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Reset %>"
                            CssClass="btn btn-primary" OnClick="btnReset_Click" />
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
