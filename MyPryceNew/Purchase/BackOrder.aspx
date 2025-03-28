<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="BackOrder.aspx.cs" Inherits="Purchase_BackOrder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/StockAnalysis.ascx" TagName="StockAnalysis" TagPrefix="SA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-file-invoice"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Purchase Back Order%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Purchase%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Purchase Back Order%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:UpdatePanel ID="Update_List" runat="server">
        <ContentTemplate>
            <div id="PnlList" runat="server">
                <div class="row">
                    <div class="col-md-12">
                        <div class="box box-primary">
                            <div class="box-body">
                                <div class="form-group">
                                    <div class="col-md-6">
                                        <asp:Label ID="lblBrandsearch" runat="server" Text="<%$ Resources:Attendance,Supplier Name %>"></asp:Label>
                                        <asp:TextBox ID="txtCustomer" runat="server" CssClass="form-control"
                                            BackColor="#eeeeee" AutoPostBack="true" OnTextChanged="txtCustomer_TextChanged" />

                                        <cc1:AutoCompleteExtender ID="txtCustomerName_AutoCompleteExtender" runat="server"
                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                            MinimumPrefixLength="1" ServiceMethod="GetCompletionListCustomer" ServicePath=""
                                            TargetControlID="txtCustomer" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                        </cc1:AutoCompleteExtender>
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="Label8" runat="server" Text="Location" />
                                        <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        <br />
                                    </div>
                                    <asp:UpdatePanel runat="server" ID="upButtons">
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="btnExportExcel" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <div class="col-md-12">
                                                <asp:Button ID="btngo" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Get Order %>"
                                                    CssClass="btn btn-primary" OnClick="btngo_Click" />
                                                <asp:Button ID="btnReset" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Reset %>"
                                                    CssClass="btn btn-primary" OnClick="btnReset_Click" />
                                                <asp:Button ID="btnExportExcel" runat="server" CausesValidation="False" Text="Export to Excel"
                                                    CssClass="btn btn-primary" OnClick="btnExportExcel_Click" />
                                                <br />
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

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
				<asp:Label ID="lblTotalRecords" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                <div class="box-tools pull-right">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                    </button>
                                </div>
                            </div>
                            <div class="box-body">
                                <div class="col-lg-3">
                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Text="<%$ Resources:Attendance, Order No. %>" Value="PoNO" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:Attendance, Order Date %>" Value="PODate"></asp:ListItem>
                                        <asp:ListItem Text="Delivery Date" Value="DeliveryDate"></asp:ListItem>
                                        <asp:ListItem Text="SO No" Value="SONO"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:Attendance,Supplier Name %>" Value="SupplierName"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:Attendance,Product Id %>" Value="ProductCode"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:Attendance,Product Name %>" Value="EProductName"></asp:ListItem>
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
                                <div class="col-lg-5">
                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                        <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                        <asp:TextBox ID="txtValueDate" runat="server" CssClass="form-control" Visible="false" placeholder="Search From Date"></asp:TextBox>
                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueDate" runat="server" TargetControlID="txtValueDate" />
                                    </asp:Panel>
                                </div>
                                <div class="col-lg-2">
                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbindrpt_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefreshReport_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>


                <div class="box box-warning box-solid" <%= GvBackOrder.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                    <div class="box-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div style="overflow: auto; max-height: 350px">
                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvBackOrder" PageSize="<%# PageControlCommon.GetPageSize() %>" runat="server"
                                        AutoGenerateColumns="False" Width="100%" AllowPaging="false" OnPageIndexChanging="GvBackOrder_PageIndexChanging"
                                        AllowSorting="True" OnSorting="GvBackOrder_Sorting">

                                        <Columns>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:LinkButton ToolTip="<%$ Resources:Attendance,Edit %>" ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="btnEdit_Command" CausesValidation="False"> <i class="fa fa-pencil"></i> </asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Supplier Name %>" SortExpression="SupplierName">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvCustomer" runat="server" Text='<%#Eval("SupplierName") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PO No" SortExpression="PoNO">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvPOID" runat="server" Text='<%#Eval("PoID") %>' Visible="false" />
                                                    <asp:Label ID="lblgvSONo" runat="server" Text='<%#Eval("PoNO") %>' Visible="false" />
                                                    <a onclick="openPO('<%#Eval("PoID") %>')" style="cursor: pointer"><%#Eval("PoNO") %></a>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PO Date" SortExpression="PODate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvSODate" runat="server" Text='<%#GetDate(Eval("PODate").ToString()) %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delivery Date" SortExpression="DeliveryDate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvDeliveryDate" runat="server" Text='<%#GetDate(Eval("DeliveryDate").ToString()) %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="SO No" SortExpression="SONO">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvSOrderNO" runat="server" Text='<%#Eval("SONO")%>' Visible="false" />
                                                    <a style="cursor: pointer" onclick="openOrder('<%#Eval("SOID") %>')"><%#Eval("SONO") %></a>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id%>" SortExpression="ProductCode">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvproductid" runat="server" Text='<%#Eval("ProductCode") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name%>" SortExpression="EProductName">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvproductname" runat="server" Text='<%#Eval("EProductName") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="200px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Name%>" SortExpression="Unit_Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblunitname" runat="server" Text='<%# Eval("Unit_Name").ToString() %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="PO Quantity">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%# GetAmountDecimal(Eval("orderqty").ToString()) %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Received Qty">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%# GetAmountDecimal(Eval("deliveredQty").ToString()) %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="BackOrder Qty" SortExpression="BackOrderQty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblqty" runat="server" Text='<%# GetAmountDecimal(Eval("BackOrderQty").ToString()) %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="System Qty" SortExpression="quantity">
                                                <ItemTemplate>
                                                    <asp:LinkButton OnCommand="lnkStockInfo_Command" OnClientClick="$('#Product_StockAnalysis').modal('show');" Text='<%# GetAmountDecimal(Eval("quantity").ToString()) %>' runat="server" ID="lnkStockInfo"></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Remarks">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNotes" runat="server" Text='<%# Eval("field4") %>'></asp:Label>
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
                                    <asp:HiddenField ID="hdnTransId" runat="server" />
                                    <asp:HiddenField ID="hdnOrderId" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="PnlNewEdit" runat="server" visible="false">
                <div class="row">
                    <div class="col-md-12">
                        <div class="box box-primary">
                            <div class="box-body">
                                <div class="form-group">
                                    <div class="col-md-6">
                                        <asp:Label ID="lblSODate" runat="server" Text="<%$ Resources:Attendance,Order No. %>"></asp:Label>
                                        <asp:TextBox ID="txtsoNo" runat="server" CssClass="form-control" ReadOnly="true" />
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="lblSONo" runat="server" Text="<%$ Resources:Attendance,Order Date %>"></asp:Label>
                                        <asp:TextBox ID="txtSODate" runat="server" CssClass="form-control" ReadOnly="true" />
                                        <br />
                                    </div>

                                    <div class="col-md-6">
                                        <asp:Label ID="Label4" runat="server" Text="Delivery Date"></asp:Label>
                                        <asp:TextBox ID="txtDeliveryDate" runat="server" CssClass="form-control" />
                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" TargetControlID="txtDeliveryDate" Format="dd-MMM-yyyy" />
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="Label7" runat="server" Text="Sales Order No"></asp:Label>
                                        <asp:TextBox ID="txtSalesOrderNo" runat="server" CssClass="form-control" ReadOnly="true" />
                                        <br />
                                    </div>

                                    <div class="col-md-12">
                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Product name%>"></asp:Label>
                                        <asp:TextBox ID="txtprouctname" runat="server" CssClass="form-control" ReadOnly="true" />
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Unit name%>"></asp:Label>
                                        <asp:TextBox ID="txtUnitName" runat="server" CssClass="form-control" ReadOnly="true" />
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="Label3" runat="server" Text="Back Order Quantity"></asp:Label>
                                        <asp:TextBox ID="txtquantity" runat="server" CssClass="form-control" />
                                        <asp:HiddenField ID="hdnQuantity" runat="server" />
                                        <br />
                                    </div>
                                    <div class="col-md-12">
                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" placeholder="Remarks" />
                                        <br />
                                    </div>
                                    <div class="col-md-12" style="text-align: center">
                                        <asp:CheckBox ID="chkSelectall" runat="server" Text="Update for all items" />
                                        <asp:Button ID="btnSOrderSave" runat="server" Text="<%$ Resources:Attendance,Save %>" Visible="false"
                                            CssClass="btn btn-success" OnClick="btnSOrderSave_Click" />&nbsp;&nbsp;&nbsp;
                                                                      <asp:Button ID="btnSOrderCancel" runat="server" CssClass="btn btn-danger" Text="<%$ Resources:Attendance,Cancel %>"
                                                                          CausesValidation="False" OnClick="btnSOrderCancel_Click" />
                                        <asp:HiddenField ID="editid" runat="server" />
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

    <div class="modal fade" id="Product_StockAnalysis" tabindex="-1" role="dialog" aria-labelledby="Product_ModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Product_StockAnalysis1">
                        <asp:Label ID="Label32" runat="server" Text="<%$ Resources:Attendance,Location Stock %>"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-12">
                            <sa:stockanalysis runat="server" id="modelSA" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>



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
        function openPO(po) {
            window.open('../Purchase/PurchaseOrder.aspx?Id=' + po + '', '_blank', 'width=1024, ');
        }
        function openOrder(so) {
            window.open('../Sales/SalesOrder1.aspx?Id=' + so + '', '_blank', 'width=1024, ');
        }
        function lnkStockInfo_Command1(productId) {
            window.open('../Inventory/Magic_Stock_Analysis.aspx?ProductId=' + productId + '&&Type=SALES&&Contact=');
        }

    </script>
</asp:Content>
