<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="ProductStock.aspx.cs" Inherits="Inventory_ProductStock" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-box-open"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Product Stock%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Product Stock%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:UpdatePanel ID="Update_List" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportExcel" />
        </Triggers>
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-md-6">
                                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,Location  %>"></asp:Label>
                                    <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control"></asp:DropDownList>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lbllocationsearch" runat="server" Text="<%$ Resources:Attendance,Category %>" />
                                    <asp:DropDownList ID="ddlcategorysearch" runat="server" CssClass="form-control"></asp:DropDownList>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="Label1" runat="server" Text="Product Brand" />
                                    <asp:DropDownList ID="ddlProductBrand" runat="server" CssClass="form-control"></asp:DropDownList>
                                    <br />
                                </div>
                                <div class="col-md-12" style="text-align: center">
                                    <asp:Button ID="btngo" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Go %>"
                                        CssClass="btn btn-primary" OnClick="btngo_Click" />
                                    <asp:Button ID="btnReset" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Reset %>"
                                        CssClass="btn btn-primary" OnClick="btnReset_Click" />
                                    <asp:ImageButton ID="IbtnPrint" runat="server" CausesValidation="False" ImageUrl="~/Images/print.png"
                                        OnClick="IbtnPrint_Command" ToolTip="<%$ Resources:Attendance,Print %>" Visible="false" />
                                    <br />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-12">
                    <div id="Div1" runat="server" class="box box-info collapsed-box">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
                            &nbsp;&nbsp;|&nbsp;&nbsp;
				<asp:Label ID="lblTotalRecords" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>" CssClass="labelComman"></asp:Label>
                            <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>

                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                    <i id="I1" runat="server" class="fa fa-plus"></i>
                                </button>
                            </div>
                        </div>
                        <div class="box-body">
                            <div class="col-lg-3">
                                <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Product Name %>" Value="EProductName"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Attendance,Product Id %>" Value="ProductCode"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Attendance,Quantity %>" Value="Quantity"></asp:ListItem>
                                    <asp:ListItem Text="Model No" Value="Model_no"></asp:ListItem>
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
                                    <asp:TextBox ID="txtValue" runat="server" placeholder="Search From Content" CssClass="form-control"></asp:TextBox>
                                </asp:Panel>
                            </div>
                            <div class="col-lg-3">
                                <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="btnExportExcel" runat="server" CausesValidation="False" OnClick="btnExportExcel_Click" ToolTip="Export Data to Excel"><span class="fas fa-file-csv"  style="font-size:25px;"></span></asp:LinkButton>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
            <div class="box box-warning box-solid">
                <div class="box-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div style="overflow: auto; max-height: 500px;">
                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvProductStock" runat="server" ShowFooter="true" AutoGenerateColumns="False" Width="100%"
                                    ShowHeader="true">
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Serial No. %>">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                            <ItemStyle/>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProductcode" runat="server" Text='<%# Eval("ProductCode") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle/>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProductName" runat="server" Text='<%# Eval("EProductName") %>'></asp:Label>
                                                <asp:HiddenField ID="HdnProductId" runat="server" Value='<%# Eval("ProductId") %>' />
                                            </ItemTemplate>
                                            <ItemStyle/>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Model no">
                                            <ItemTemplate>
                                                <%# Eval("Model_No") %>
                                            </ItemTemplate>
                                            <ItemStyle/>
                                        </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Colour">
                                            <ItemTemplate>
                                                <%# Eval("ColourName") %>
                                            </ItemTemplate>
                                            <ItemStyle/>
                                        </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Size">
                                            <ItemTemplate>
                                                <%# Eval("SizeName") %>
                                            </ItemTemplate>
                                            <ItemStyle/>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Name %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblunit" runat="server" Text='<%# Eval("Unitname") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle/>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SalesPrice1">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSalesPrice1" runat="server" Text='<%# SetDecimal(string.IsNullOrEmpty(Eval("SalesPrice1").ToString())?"0":Eval("SalesPrice1").ToString(),Eval("CurrencyID").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblFooterSalesPrice1" runat="server" CssClass="labelComman"></asp:Label>
                                            </FooterTemplate>
                                            <ItemStyle/>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SalesPrice2">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSalesPrice2" runat="server" Text='<%# SetDecimal(string.IsNullOrEmpty(Eval("SalesPrice2").ToString())?"0":Eval("SalesPrice2").ToString(),Eval("CurrencyID").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblFooterSalesPrice2" runat="server" CssClass="labelComman"></asp:Label>
                                            </FooterTemplate>
                                            <ItemStyle/>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SalesPrice3">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSalesPrice3" runat="server" Text='<%# SetDecimal(string.IsNullOrEmpty(Eval("SalesPrice3").ToString())?"0":Eval("SalesPrice3").ToString(),Eval("CurrencyID").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblFooterSalesPrice3" runat="server" CssClass="labelComman"></asp:Label>
                                            </FooterTemplate>
                                            <ItemStyle/>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Average cost %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAvgCost" runat="server" Text='<%#  SetDecimal(string.IsNullOrEmpty(Eval("Field2").ToString())?"0":Eval("Field2").ToString(),Eval("CurrencyID").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblFooterAvgCost" runat="server" CssClass="labelComman"></asp:Label>
                                            </FooterTemplate>
                                            <ItemStyle/>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quantity %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuantity" runat="server" Text='<%# SetDecimal(Eval("Quantity").ToString(),Eval("CurrencyID").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle/>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Location %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblocation" runat="server" Text='<%# Eval("LocationName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle/>
                                        </asp:TemplateField>
                                    </Columns>




                                </asp:GridView>
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
