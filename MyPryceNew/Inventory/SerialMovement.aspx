<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="SerialMovement.aspx.cs" Inherits="Inventory_SerialMovement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-sliders-h"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Serial Movement%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Serial Movement%>"></asp:Label></li>
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
                                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Product Id%>" />
                                    <a style="color: Red">*</a>
                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtProductcode" ErrorMessage="<%$ Resources:Attendance,Enter Product Id%>"></asp:RequiredFieldValidator>

                                    <asp:TextBox ID="txtProductcode" runat="server" CssClass="form-control" AutoPostBack="True" BackColor="#e6e6e6"
                                        OnTextChanged="txtProductCode_TextChanged" />

                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="100"
                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductCode"
                                        ServicePath="" TargetControlID="txtProductcode" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                    </cc1:AutoCompleteExtender>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <br />
                                    <asp:Label ID="lblTotalRecords" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <asp:Label ID="lblProductName" runat="server" Text="<%$ Resources:Attendance,Product Name %>" />
                                    <a style="color: Red">*</a>
                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtProductName" ErrorMessage="<%$ Resources:Attendance,Enter Product Name%>"></asp:RequiredFieldValidator>

                                    <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" AutoPostBack="true" BackColor="#e6e6e6"
                                        OnTextChanged="txtProductName_TextChanged" />
                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetCompletionListProductName" ServicePath="" CompletionInterval="100"
                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtProductName"
                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                    </cc1:AutoCompleteExtender>
                                    <asp:HiddenField ID="hdnNewProductId" runat="server" Value="0" />
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblFromDate" runat="server" Text="<%$ Resources:Attendance,From Date  %>"></asp:Label>
                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control"></asp:TextBox>
                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate">
                                    </cc1:CalendarExtender>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblToDate" runat="server" Text="<%$ Resources:Attendance,To Date  %>"></asp:Label>
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control"></asp:TextBox>
                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtToDate_CalendarExtender" runat="server" TargetControlID="txtToDate">
                                    </cc1:CalendarExtender>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,Location  %>"></asp:Label>
                                    <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control"></asp:DropDownList>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Serial No %> "></asp:Label>
                                    <asp:TextBox ID="txtSerialNo" runat="server" CssClass="form-control" OnTextChanged="txtSerialNo_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    <br />
                                </div>
                                <div class="col-md-12" style="text-align: center">
                                    <asp:Button ID="btngo" runat="server" ValidationGroup="Save" Text="<%$ Resources:Attendance,Go %>"
                                        CssClass="btn btn-primary" OnClick="btngo_Click" />

                                    <asp:Button ID="btnReset" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Reset %>"
                                        CssClass="btn btn-primary" OnClick="btnReset_Click" />
                                    
                                    <asp:Button ID="btnExportExcel" runat="server" CausesValidation="False" Text="Export Data to Excel"
                                        CssClass="btn btn-primary" OnClick="btnExportExcel_Click" />
                                    <br />
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="box box-primary box-solid">
                <div class="box-header with-border">
                    <div style="text-align: center" class="col-md-4">
                        <asp:Label ID="lblAvailablestock" runat="server" Text="Available Stock Qty" Font-Bold="true"></asp:Label>
                        &nbsp:&nbsp<asp:Label ID="lblValueAvailablestock" runat="server" Font-Bold="true"></asp:Label>
                        <br />
                    </div>
                    <div style="text-align: center" class="col-md-4">
                        <asp:Label ID="Label2" runat="server" Text="Available Serial Qty" Font-Bold="true"></asp:Label>
                        &nbsp:&nbsp<asp:Label ID="lblValueAvailableserialstock" Font-Bold="true" runat="server"></asp:Label>
                        <br />
                    </div>
                    <div style="text-align: center" class="col-md-4">
                        <asp:Label ID="Label3" runat="server" Text="Exceptional Serial Qty" Font-Bold="true"></asp:Label>
                        &nbsp:&nbsp<asp:Label ID="lblValueExceptionalserialstock" Font-Bold="true" runat="server"></asp:Label>
                        <br />
                    </div>
                </div>
                <div class="box-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="flow">
                                <asp:HiddenField ID="HDFSort" runat="server" />
                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvserailMovement" PageSize="<%# PageControlCommon.GetPageSize() %>" ShowFooter="true"
                                    OnSorting="gvserailMovement_OnSorting" AllowSorting="true" runat="server" AutoGenerateColumns="false"
                                    Width="100%" AllowPaging="false">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblsrno" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Product Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProductode" runat="server" Text='<%# Eval("ProductCode").ToString() %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer/Supplier">
                                            <ItemTemplate>
                                                <asp:Label ID="lblContactName" runat="server" Text='<%# Eval("ContactName").ToString() %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="UnitName">
                                            <ItemTemplate>
                                                <asp:Label ID="bllUnitName" runat="server" Text='<%# Eval("UnitName").ToString() %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Voucher Type">
                                            <ItemTemplate>
                                                <asp:Label ID="bllRefferenceType" runat="server" Text='<%# Eval("RefferenceType").ToString() %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Voucher No">
                                            <ItemTemplate>
                                                <asp:Label ID="bllRefferenceId" runat="server" Text='<%# Eval("RefferenceId").ToString() %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="VoucherDate">
                                            <ItemTemplate>
                                                <asp:Label ID="bllVoucherDate" runat="server" Text='<%# Eval("VoucherDate").ToString() %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="InQty">
                                            <ItemTemplate>
                                                <asp:Label ID="bllInQty" runat="server" Text='<%# SetDecimal(Eval("InQty").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="txttotQuantityIn" runat="server" Text="0" />
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="OutQty">
                                            <ItemTemplate>
                                                <asp:Label ID="bllOutQty" runat="server" Text='<%# SetDecimal(Eval("OutQty").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="txttotQuantityOut" runat="server" Text="0" />
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BalanceQty">
                                            <ItemTemplate>
                                                <asp:Label ID="bllBalanceQty" runat="server" Text='<%# SetDecimal(Eval("BalanceQty").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="txttotQuantityBalance" runat="server" Text="0" />
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="SerialNo">
                                            <ItemTemplate>
                                                <asp:Label ID="bllSerialNo" runat="server" Text='<%# Eval("SerialNo").ToString() %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Location">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocation" runat="server" Text='<%# Eval("Location").ToString() %>'></asp:Label>
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
