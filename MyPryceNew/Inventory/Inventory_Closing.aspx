<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="Inventory_Closing.aspx.cs" Inherits="Inventory_Inventory_Closing" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
         <i class="fas fa-box"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Inventory Closing%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Inventory Closing%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:UpdatePanel ID="Update_List" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-md-6">
                                    <asp:Label ID="lblFinanceCode" runat="server" Text="Finance Code"></asp:Label>
                                    <asp:TextBox ID="txtFinanceCode" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    <asp:HiddenField ID="hdnTransId" runat="server" />
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="Label1" runat="server" Text="From Date"></asp:Label>
                                    <asp:TextBox ID="txtFromdate" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="Label2" runat="server" Text="To Date"></asp:Label>
                                    <asp:TextBox ID="txTodate" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    <br />
                                </div>
                                <div class="col-md-6" style="text-align: center">
                                    <br />
                                    <asp:Button ID="btnGet" runat="server" Text="<%$ Resources:Attendance,Get %>" CssClass="btn btn-primary"
                                        OnClick="btnGet_Click" ValidationGroup="a" />
                                    <br />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="pnlclosing" runat="server" visible="false">
                <div class="box box-warning box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title"></h3>
                    </div>
                    <div class="box-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="flow">
                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvunPostedRecord" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                        Width="100%">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Page Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProductId" runat="server" Text='<%# Eval("PageName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unposted Record">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProductId" runat="server" Text='<%# Eval("UnpostRecord") %>'></asp:Label>
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
                <div class="row">
                    <div class="col-md-12">
                        <div class="box box-primary">
                            <div class="box-body">
                                <div class="form-group">
                                    <div class="col-md-6">
                                        <asp:Label ID="Label3" runat="server" Text="Closing Stock Quantity"></asp:Label>
                                        <asp:Label ID="lblClosingqty" runat="server"></asp:Label>
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="Label4" runat="server" Text="Closing Stock Value"></asp:Label>
                                        <asp:Label ID="lblClosingValue" runat="server"></asp:Label>
                                        <br />
                                    </div>
                                    <div class="col-md-12" style="text-align: center">
                                        <asp:Button ID="btnClosing" runat="server" Text="Closing" CssClass="btn btn-primary"
                                            OnClick="btnClosing_Click" ValidationGroup="a" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="Are you sure to closing for this location ?"
                                                                TargetControlID="btnClosing">
                                                            </cc1:ConfirmButtonExtender>
                                        <asp:Button ID="btnStockReport" runat="server" Text="Product Stock Report" CssClass="btn btn-primary"
                                            OnClick="btnStockReport_Click" ValidationGroup="a" />
                                        <br />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_List">
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
