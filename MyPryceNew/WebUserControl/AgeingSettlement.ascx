<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AgeingSettlement.ascx.cs" Inherits="WebUserControl_AgeingSettlement" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="../Bootstrap_Files/dist/css/AdminLTE.min.css" rel="stylesheet" />
  <div class="row" style="text-align:center">
      <div class="col-md-12">
          <asp:RadioButton ID="rbtnVoucher" AutoPostBack="true" Checked="true" OnCheckedChanged="rbtnCheck_Changed" Text="Settle Against Invoice"  runat="server"  GroupName="Settle" />
          &nbsp;
          <asp:RadioButton ID="rbtnDirect" AutoPostBack="true" OnCheckedChanged="rbtnCheck_Changed" Text="Settle Amount"  runat="server" GroupName="Settle" />
      </div>
  </div>
<div class="row" id="VoucherPannel" runat="server" >
    <div class="col-md-12">
        <asp:Label ID="lblSettleSupplier" runat="server" Text="<%$ Resources:Attendance,Supplier %>" />
        <a style="color: Red">*</a>
      <%--  <asp:RequiredFieldValidator EnableClientScript="true" style="float:right;" runat="server" id="RequiredFieldValidator3" 
        Display="Dynamic" SetFocusOnError="true" controltovalidate="txtSettleSupplier" errormessage="<%$ Resources:Attendance,Enter Supplier Name %>"></asp:RequiredFieldValidator>--%>

        <div class="input-group">
            <asp:TextBox ID="txtSettleSupplier" runat="server" CssClass="form-control" BackColor="#eeeeee"
                AutoPostBack="True" OnTextChanged="txtSettleSupplier_TextChanged" />
            <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" CompletionInterval="100"
                DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionList"
                ServicePath="" TargetControlID="txtSettleSupplier" UseContextKey="True"  CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
            </cc1:AutoCompleteExtender>
            <div class="input-group-btn">
                <asp:Button ID="btnSettleSupplierAdd" runat="server" CssClass="btn btn-primary" OnClick="btnSettleSupplierAdd_OnClick"
                    Text="<%$ Resources:Attendance,Execute %>" CausesValidation="False" />
            </div>
        </div>
        <br />
    </div>
    <div class="col-md-12" style="text-align:center;">
        <br />
        <asp:Button ID="btnUpdateAgeing" runat="server" Text="<%$ Resources:Attendance,Update  %>"
            CssClass="btn btn-primary" OnClick="btnUpdateAgeing_Click" />

        <asp:Button ID="btnAgeingReset" runat="server" Text="<%$ Resources:Attendance,Reset  %>"
            CssClass="btn btn-primary" OnClick="btnAgeingReset_Click" />
        <br />
    </div>
    <div class="col-md-6">
        <br />
        <asp:Label ID="SettleCR" Visible="false" runat="server" Text="Voucher Detail" />
        <br />
    </div>
    <div class="col-md-12" style="overflow: auto; max-height: 500px;">
        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GVSettleMentCredit" runat="server" AutoGenerateColumns="False"
            Width="100%"  ShowFooter="true">
            <Columns>
                <asp:TemplateField HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSettleCeditId" runat="server" OnCheckedChanged="chkSettleCeditId_CheckedChanged"
                            AutoPostBack="true" />

                        <asp:HiddenField ID="hdnVoucherId" runat="server" Value='<%#Eval("Voucher_Id") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemStyle  />
                    <FooterStyle BorderStyle="None" />
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lblSNo" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemStyle  />
                    <FooterStyle BorderStyle="None" />
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Voucher No">
                    <ItemTemplate>
                        <asp:Label ID="lblVoucherNo" runat="server" Text='<%# Eval("Voucher_No") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemStyle  />
                    <FooterStyle BorderStyle="None" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Voucher Date">
                    <ItemTemplate>
                        <asp:Label ID="lblVoucherDate" runat="server" Text='<%# GetDateFormat(Eval("Voucher_Date").ToString()) %>'></asp:Label>
                    </ItemTemplate>
                    <FooterStyle BorderStyle="None" />
                    <ItemStyle  />
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Currency">
                    <ItemTemplate>
                        <asp:Label ID="lblCurrency" runat="server" Text='<%# Eval("currency_name") %>'></asp:Label>
                        <asp:HiddenField ID="hdnCurrencyId" runat="server" Value='<%#Eval("currency_id") %>' />
                        <asp:HiddenField ID="hdnVoucherExchangeRate" runat="server" Value='<%#Eval("Exchange_rate") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemStyle  />
                    <FooterStyle BorderStyle="None" />
                </asp:TemplateField>


                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Voucher Amount">
                    <ItemTemplate>
                        <asp:Label ID="lblVoucherAmount" runat="server" Text='<%# Eval("actual_voucher_Amount").ToString() %>'></asp:Label>
                    </ItemTemplate>
                    <FooterStyle BorderStyle="None" />
                    <ItemStyle  />
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Balance Amount">
                    <ItemTemplate>
                        <asp:Label ID="lblBalanceAmount" runat="server" Text='<%# Eval("actual_balance_amount").ToString() %>'></asp:Label>
                    </ItemTemplate>
                    <FooterStyle BorderStyle="None" />
                    <ItemStyle  />
                </asp:TemplateField>
            </Columns>
            
            
            
            <PagerStyle CssClass="pagination-ys" />
            
        </asp:GridView>
        <br />
    </div>
    <div class="col-md-12">
        <br />
        <asp:Label ID="SettleDR" Visible="false" runat="server" Text="Pending Invoice" />
        <br />
    </div>
    <div class="col-md-12" style="overflow: auto; max-height: 500px;">
        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GVSettleMentDebit" runat="server" AutoGenerateColumns="False" Width="100%"
             ShowFooter="true">
            <Columns>
                <asp:TemplateField HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkTrandId" runat="server" Visible="false"
                            AutoPostBack="true" />
                    </ItemTemplate>
                    <ItemStyle Width="20px" HorizontalAlign="Center" />
                    <ItemStyle  />
                    <FooterStyle BorderStyle="None" />
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lblSNo" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="20px"  HorizontalAlign="Center" />
                    <ItemStyle  />
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
                        <asp:HiddenField ID="hdnLocationId" runat="server" Value='<%# Eval("location_id") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemStyle  />
                    <FooterStyle BorderStyle="None" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Invoice Date">
                    <ItemTemplate>
                        <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# GetDateFormat(Eval("Invoice_Date").ToString()) %>'></asp:Label>
                    </ItemTemplate>
                    <FooterStyle BorderStyle="None" />
                    <ItemStyle  />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Currency">
                    <ItemTemplate>
                        <asp:Label ID="lblCurrency" runat="server" Text='<%# Eval("Currency_Name") %>'></asp:Label>
                        <asp:HiddenField ID="hdnCurrencyId" runat="server" Value='<%# Eval("Currency_Id") %>' />
                        <asp:HiddenField ID="hdnNewExchangeRate" runat="server" Value='<%# Eval("Exchange_Rate") %>' />
                    </ItemTemplate>
                    <FooterStyle BorderStyle="None" />
                    <ItemStyle  />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Exchange Rate">
                    <ItemTemplate>
                        <asp:Label ID="lblExchangeRate" runat="server" Text='<%# Eval("Exchange_Rate") %>'></asp:Label>

                    </ItemTemplate>
                    <FooterStyle BorderStyle="None" />
                    <ItemStyle  />
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Invoice Amount">
                    <ItemTemplate>
                        <asp:Label ID="lblinvamount" runat="server" Text='<%# Eval("actual_Invoice_amt")%>'></asp:Label>
                        <%-- <asp:HiddenField ID="hdnExchangeRate" runat="server" />--%>
                        <%--Value='<%#Eval("Exchange_Rate") %>'--%>
                    </ItemTemplate>
                    <FooterStyle BorderStyle="None" />
                    <ItemStyle  />
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Balance Amount">
                    <ItemTemplate>
                        <asp:Label ID="lblBalanceAmount" runat="server" Text='<%# Eval("actual_balance_amt").ToString() %>'></asp:Label>
                    </ItemTemplate>
                    <FooterStyle BorderStyle="None" />
                    <ItemStyle  />
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Settle Amount">
                    <ItemTemplate>
                        <asp:TextBox ID="txtSettleAmount" runat="server" OnTextChanged="txtSettleAmount_OnTextChanged"
                            AutoPostBack="true" Width="100px"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                            TargetControlID="txtSettleAmount" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                        </cc1:FilteredTextBoxExtender>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblgvSettleTotal" runat="server" />
                    </FooterTemplate>
                    <FooterStyle BorderStyle="None" />
                    <ItemStyle  />
                </asp:TemplateField>

            </Columns>
            
            
            
            <PagerStyle CssClass="pagination-ys" />
            
        </asp:GridView>
        <br />
    </div>
</div>
<div class="row" id="DirectPannel" visible="false" runat="server">
    <div class="col-md-12">
        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Supplier %>" />
        <a style="color: Red">*</a>
        <%--  <asp:RequiredFieldValidator EnableClientScript="true" style="float:right;" runat="server" id="RequiredFieldValidator3" 
  Display="Dynamic" SetFocusOnError="true" controltovalidate="txtSettleSupplier" errormessage="<%$ Resources:Attendance,Enter Supplier Name %>"></asp:RequiredFieldValidator>--%>

        <div class="input-group">
            <asp:TextBox ID="txtDirectSupplier" OnTextChanged="txtSupplierName_TextChanged" runat="server" CssClass="form-control" BackColor="#eeeeee"
                AutoPostBack="True"  />
            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="100"
                DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionList"
                ServicePath="" TargetControlID="txtDirectSupplier" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
            </cc1:AutoCompleteExtender>
            <div class="input-group-btn">
                <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary" OnClick="btnDirectSettleSupplierAdd_OnClick"
                    Text="<%$ Resources:Attendance,Execute %>" CausesValidation="False" />
            </div>
        </div>
        <div class="row">
            <br />
            <div class="col-md-6">
               <asp:Label ID="lblCurrency" runat="server" Text="<%$ Resources:Attendance,Currency %>" />
                <asp:DropDownList ID="ddlForeginCurrency" runat="server" CssClass="form-control"
    OnSelectedIndexChanged="ddlForeginCurrency_SelectedIndexChanged" AutoPostBack="true" />
            </div>
            <div class="col-md-6">
                 <asp:Label ID="lblExchangeRate" runat="server" Text="<%$ Resources:Attendance,Exchange Rate %>" />
 <asp:TextBox ID="txtExchangeRate" runat="server" CssClass="form-control" AutoPostBack="True"
      />
            </div>
        </div>



    </div>
        <div class="col-md-12" style="overflow: auto; max-height: 500px;">
            <br />
            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvDirectDebit" runat="server" AutoGenerateColumns="False" Width="100%"
         ShowFooter="true">
        <Columns>
            <asp:TemplateField HeaderStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:CheckBox ID="chkDirectTrandId" runat="server" Visible="false"
                        AutoPostBack="true" />
                </ItemTemplate>
                <ItemStyle Width="20px" HorizontalAlign="Center" />
                <ItemStyle  />
                <FooterStyle BorderStyle="None" />
            </asp:TemplateField>

            <asp:TemplateField HeaderStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Label ID="lblDirectSNo" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="20px"  HorizontalAlign="Center" />
                <ItemStyle  />
                <FooterStyle BorderStyle="None" />
            </asp:TemplateField>
            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Invoice No.">
                <ItemTemplate>
                    <asp:Label ID="lblDirectPONo" runat="server" Text='<%# Eval("Invoice_No") %>'></asp:Label>
                    <asp:HiddenField ID="hdnDirectRefId" runat="server" Value='<%# Eval("Ref_Id") %>' />
                    <asp:HiddenField ID="hdnDirectRefType" runat="server" Value='<%# Eval("Ref_Type") %>' />
                    <asp:HiddenField ID="hdnDirectAgeingType" runat="server" Value='<%# Eval("AgeingType") %>' />
                    <asp:HiddenField ID="hdnDirectAccountNo" runat="server" Value='<%# Eval("Account_No") %>' />
                    <asp:HiddenField ID="hdnDirectOtherAccountNo" runat="server" Value='<%# Eval("Other_Account_No") %>' />
                    <asp:HiddenField ID="hdnDirectLocationId" runat="server" Value='<%# Eval("location_id") %>' />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
                <ItemStyle  />
                <FooterStyle BorderStyle="None" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Invoice Date">
                <ItemTemplate>
                    <asp:Label ID="lblDirectInvoiceDate" runat="server" Text='<%# GetDateFormat(Eval("Invoice_Date").ToString()) %>'></asp:Label>
                </ItemTemplate>
                <FooterStyle BorderStyle="None" />
                <ItemStyle  />
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Currency">
                <ItemTemplate>
                    <asp:Label ID="lblDirectCurrency" runat="server" Text='<%# Eval("Currency_Name") %>'></asp:Label>
                    <asp:HiddenField ID="hdnDirectCurrencyId" runat="server" Value='<%# Eval("Currency_Id") %>' />
                    <asp:HiddenField ID="hdnDirectNewExchangeRate" runat="server" Value='<%# Eval("Exchange_Rate") %>' />
                </ItemTemplate>
                <FooterStyle BorderStyle="None" />
                <ItemStyle  />
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Exchange Rate">
                <ItemTemplate>
                    <asp:Label ID="lblDirectExchangeRate" runat="server" Text='<%# Eval("Exchange_Rate") %>'></asp:Label>

                </ItemTemplate>
                <FooterStyle BorderStyle="None" />
                <ItemStyle  />
            </asp:TemplateField>

            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Invoice Amount">
                <ItemTemplate>
                    <asp:Label ID="lblDirectinvamount" runat="server" Text='<%# Eval("actual_Invoice_amt")%>'></asp:Label>
                    <%-- <asp:HiddenField ID="hdnExchangeRate" runat="server" />--%>
                    <%--Value='<%#Eval("Exchange_Rate") %>'--%>
                </ItemTemplate>
                <FooterStyle BorderStyle="None" />
                <ItemStyle  />
            </asp:TemplateField>
            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Balance Amount">
                <ItemTemplate>
                    <asp:Label ID="lblDirectBalanceAmount" runat="server" Text='<%# Eval("actual_balance_amt").ToString() %>'></asp:Label>
                </ItemTemplate>
                <FooterStyle BorderStyle="None" />
                <ItemStyle  />
            </asp:TemplateField>

            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Settle Amount">
                <ItemTemplate>
                    <asp:TextBox ID="txtDirectSettleAmount" runat="server" OnTextChanged="txtDirectSettleAmount_OnTextChanged"
                        AutoPostBack="true" Width="100px"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                        TargetControlID="txtDirectSettleAmount" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                    </cc1:FilteredTextBoxExtender>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:Label ID="lblDirectgvSettleTotal" runat="server" />
                </FooterTemplate>
                <FooterStyle BorderStyle="None" />
                <ItemStyle  />
            </asp:TemplateField>

        </Columns>
        
        
        
        <PagerStyle CssClass="pagination-ys" />
        
    </asp:GridView>
        <br />
            <div class="row" style="text-align:center" >
                <div class="col-md-12">
                    <asp:Button ID="btnDirectSaveAging" CssClass="btn btn-success" runat="server" OnClick="btnDirectSave_Click" Text="Update" />
                    <asp:Button ID="btnDirectReset" CssClass="btn btn-danger" runat="server"  OnClick="btnDirectReset_Click" Text="Reset" />
                </div>
            </div>
    </div>
   
</div>
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