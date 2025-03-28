<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditAgeing.ascx.cs" Inherits="WebUserControl_EditAgeing" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<link href="../Bootstrap_Files/dist/css/AdminLTE.min.css" rel="stylesheet" />

<div class="row">
    <div class="col-md-12">
        <asp:Label ID="lblSettleSupplier" runat="server" Text="<%$ Resources:Attendance,Supplier %>" />
        <a style="color: Red">*</a>
        <%-- <asp:RequiredFieldValidator EnableClientScript="true" style="float:right;" runat="server" id="RequiredFieldValidator3" 
        Display="Dynamic" SetFocusOnError="true" controltovalidate="txtSettleSupplier" errormessage="<%$ Resources:Attendance,Enter Supplier Name %>"></asp:RequiredFieldValidator>--%>

        <asp:TextBox ID="txtSettleSupplier" runat="server" CssClass="form-control" BackColor="#eeeeee"
            AutoPostBack="True" OnTextChanged="txtSettleSupplier_TextChanged" />
        <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" CompletionInterval="100"
            DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionList"
            ServicePath="" TargetControlID="txtSettleSupplier" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
        </cc1:AutoCompleteExtender>
        <br />
    </div>
    <div class="col-md-6">
        <asp:Label ID="lblAgeingInvoiceNo" runat="server" Text="<%$ Resources:Attendance,Invoice No. %>" />
            <asp:TextBox ID="txtAgeingInvoiceNo" runat="server" CssClass="form-control" BackColor="#eeeeee"
                AutoPostBack="True" OnTextChanged="txtAgeingInvoiceNo_TextChanged"></asp:TextBox>
            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="100"
                DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListInvoiceNo"
                ServicePath="" TargetControlID="txtAgeingInvoiceNo" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
            </cc1:AutoCompleteExtender>
            <asp:HiddenField ID="hdnAgeingInvoiceId" runat="server" Value="0" />
            <asp:HiddenField ID="hdnAgeingInvoiceNo" runat="server" Value="0" />
        <br />
    </div>

    <div class="col-md-6">
        <asp:Label ID="lblAgeingVoucherNo" runat="server" Text="<%$ Resources:Attendance,Voucher No. %>" />
        <asp:TextBox ID="txtAgeingVoucherNo" runat="server" CssClass="form-control" BackColor="#eeeeee"
            AutoPostBack="True" OnTextChanged="txtAgeingVoucherNo_TextChanged"></asp:TextBox>
        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionInterval="100"
            DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListVoucherNo"
            ServicePath="" TargetControlID="txtAgeingVoucherNo" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
        </cc1:AutoCompleteExtender>
        <asp:HiddenField ID="hdnAgeingVoucherId" runat="server" Value="0" />
        <asp:HiddenField ID="hdnAgeingVoucherNo" runat="server" Value="0" />
        <br />
    </div>

    <div class="col-md-12">
        <asp:Button ID="btnSettleSupplierAdd" runat="server" CssClass="btn btn-primary" OnClick="btnSettleSupplierAdd_OnClick"
                    Text="<%$ Resources:Attendance,Execute %>" CausesValidation="False" />
        <br />
    </div>


<div class="col-md-12" style="overflow: auto; max-height: 500px">
    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GVSettleMentDebit" runat="server" AutoGenerateColumns="False" Width="100%"
        ShowFooter="true">
        <Columns>

            <asp:TemplateField HeaderStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Label ID="lblSNo" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
                <ItemStyle />
                <FooterStyle BorderStyle="None" />
            </asp:TemplateField>
            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Invoice No.">
                <ItemTemplate>
                    <asp:Label ID="lblPONo" runat="server" Text='<%# Eval("Invoice_No") %>'></asp:Label>
                    <asp:HiddenField ID="hdnRefId" runat="server" Value='<%# Eval("Ref_Id") %>' />
                    <asp:HiddenField ID="hdnRefType" runat="server" Value='<%# Eval("Ref_Type") %>' />
                    <asp:HiddenField ID="hdnAgeingType" runat="server" Value='<%# Eval("AgeingType") %>' />
                    <asp:HiddenField ID="hdnAccountNo" runat="server" Value='<%# Eval("Account_No") %>' />
                    <asp:HiddenField ID="hdnOtherAccountNo" runat="server" Value='<%# Eval("Other_Account_No") %>' />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
                <ItemStyle />
                <FooterStyle BorderStyle="None" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Invoice Date">
                <ItemTemplate>
                    <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# GetDateFormat(Eval("Invoice_Date").ToString()) %>'></asp:Label>
                </ItemTemplate>
                <FooterStyle BorderStyle="None" />
                <ItemStyle />
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Currency">
                <ItemTemplate>
                    <asp:Label ID="lblCurrency" runat="server" Text='<%# Eval("Currency_Name") %>'></asp:Label>
                    <asp:HiddenField ID="hdnCurrencyId" runat="server" Value='<%# Eval("Currency_Id") %>' />
                    <asp:HiddenField ID="hdnNewExchangeRate" runat="server" Value='<%# Eval("Exchange_Rate") %>' />
                </ItemTemplate>
                <FooterStyle BorderStyle="None" />
                <ItemStyle />
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Exchange Rate">
                <ItemTemplate>
                    <asp:Label ID="lblExchangeRate" runat="server" Text='<%# Eval("Exchange_Rate") %>'></asp:Label>

                </ItemTemplate>
                <FooterStyle BorderStyle="None" />
                <ItemStyle HorizontalAlign="Right" />
            </asp:TemplateField>

            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Invoice Amount">
                <ItemTemplate>
                    <asp:Label ID="lblinvamount" runat="server" Text='<%# Eval("actual_Invoice_amt")%>'></asp:Label>
                    <%-- <asp:HiddenField ID="hdnExchangeRate" runat="server" />--%>
                    <%--Value='<%#Eval("Exchange_Rate") %>'--%>
                </ItemTemplate>
                <FooterStyle BorderStyle="None" />
                <ItemStyle HorizontalAlign="Right" />
            </asp:TemplateField>
            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Balance Amount">
                <ItemTemplate>
                    <asp:Label ID="lblBalanceAmount" runat="server" Text='<%# Eval("actual_balance_amt").ToString() %>'></asp:Label>
                </ItemTemplate>
                <FooterStyle BorderStyle="None" />
                <ItemStyle HorizontalAlign="Right" />
            </asp:TemplateField>



        </Columns>



        <PagerStyle CssClass="pagination-ys" />

    </asp:GridView>
    <br />
</div>
<div class="col-md-12" style="overflow: auto; max-height: 500px;">
    <br />
    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GVSettleMentDetail" runat="server" AutoGenerateColumns="False"
        Width="100%">
        <Columns>
            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                <ItemTemplate>
                    <asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                        ImageUrl="~/Images/Erase.png" OnCommand="IbtnDelete_Command" Width="16px" />
                    <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>"
                        TargetControlID="IbtnDelete">
                    </cc1:ConfirmButtonExtender>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>

            <asp:TemplateField HeaderStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Label ID="lblSNo" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
                <ItemStyle />
                <FooterStyle BorderStyle="None" />
            </asp:TemplateField>
            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Invoice No">
                <ItemTemplate>
                    <asp:Label ID="lblinvoiceNo" runat="server" Text='<%# Eval("Invoice_No") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
                <ItemStyle />
                <FooterStyle BorderStyle="None" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Invoice Date">
                <ItemTemplate>
                    <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# GetDateFormat(Eval("Invoice_Date").ToString()) %>'></asp:Label>
                </ItemTemplate>
                <FooterStyle BorderStyle="None" />
                <ItemStyle />
            </asp:TemplateField>


            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Voucher No">
                <ItemTemplate>
                    <asp:Label ID="lblVoucherNo" runat="server" Text='<%# Eval("Voucher_No").ToString() %>'></asp:Label>
                </ItemTemplate>
                <FooterStyle BorderStyle="None" />
                <ItemStyle />
            </asp:TemplateField>
            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Voucher No">
                <ItemTemplate>
                    <asp:Label ID="lblVoucherdate" runat="server" Text='<%# GetDateFormat(Eval("Voucher_Date").ToString()) %>'></asp:Label>
                </ItemTemplate>
                <FooterStyle BorderStyle="None" />
                <ItemStyle />
            </asp:TemplateField>

            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Receive Amount">
                <ItemTemplate>
                    <asp:Label ID="lblBalanceAmount" runat="server" Text='<%# Eval("actual_Receive_amt").ToString() %>'></asp:Label>
                </ItemTemplate>
                <FooterStyle BorderStyle="None" />
                <ItemStyle HorizontalAlign="Right" />
            </asp:TemplateField>


        </Columns>



        <PagerStyle CssClass="pagination-ys" />

    </asp:GridView>
    <br />
</div>
</div>
<script type="text/javascript">
    function resetPosition(object, args) {
        var tbposition = findPositionWithScrolling(100004);
        var xposition = tbposition[0] + 2;
        var yposition = tbposition[1] + 25;
        var ex = object._completionListElement;
        if (ex)
            $common.setLocation(ex, new Sys.UI.Point(xposition, yposition));
    }
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
