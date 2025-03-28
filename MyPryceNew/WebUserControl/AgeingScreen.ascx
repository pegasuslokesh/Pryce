<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AgeingScreen.ascx.cs" Inherits="WebUserControl_AgeingScreen" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="../Bootstrap_Files/dist/css/AdminLTE.min.css" rel="stylesheet" />

<div class="row">
    <div class="col-md-6">
        <asp:Label ID="Label2" runat="server" Text="Payment Date" ></asp:Label>
        <asp:TextBox ID="txtPaymentdate" runat="server" CssClass="form-control" />
        <cc1:CalendarExtender OnClientShown="showCalendar"  ID="txtPaymentdate_CalendarExtender" runat="server" TargetControlID="txtPaymentdate"
            Format="dd-MMM-yyyy" />
        <br />
    </div>
    <div class="col-md-6">
        <asp:Label ID="lblContactname" runat="server" Text="Supplier Name"
            ></asp:Label>
        <a style="color: Red">*</a>
       <%-- <asp:RequiredFieldValidator EnableClientScript="true" style="float:right;" runat="server" id="RequiredFieldValidator3" 
        Display="Dynamic" SetFocusOnError="true" controltovalidate="txtSupplierPendingPayment" errormessage="<%$ Resources:Attendance,Enter Supplier Name %>"></asp:RequiredFieldValidator>--%>

        <asp:TextBox ID="txtSupplierPendingPayment" runat="server" CssClass="form-control" BackColor="#eeeeee" />
        <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server"
            CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
            ServiceMethod="GetCompletionList" ServicePath="" TargetControlID="txtSupplierPendingPayment"
            UseContextKey="True"  CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
        </cc1:AutoCompleteExtender>
        <br />
    </div>
    <div class="col-md-12" style="text-align:center">
        <asp:Label ID="Label6" runat="server" Text="Location Name" ></asp:Label>
        <br />
    </div>
    <div class="col-md-12">
        <div class="col-md-2"></div>
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
        <div class="col-md-2"></div>
        <br />
    </div>
    <div class="col-md-12" style="text-align:center">
        <br />
        <asp:Button ID="btnGetPendingList" runat="server" style="text-align:center" CssClass="btn btn-primary" OnClick="btnGetPendingList_OnClick"
            Text="<%$ Resources:Attendance,Get %>" CausesValidation="False" />
        <br />
    </div>
    <div class="col-md-12" style="overflow: auto; max-height: 500px;">
        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvPendingPayment" runat="server" AutoGenerateColumns="False" Width="100%"
             ShowFooter="true">
            <Columns>

                <%--    <asp:TemplateField HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                      <asp:Button ID="btnEdit" runat="server" BorderStyle="None" BackColor="Transparent"
                                                                CausesValidation="False" CssClass="btnPull" CommandArgument='<%# Eval("Other_Account_No") %>' CommandName='<%# Eval("Name") %>'
                                                                OnCommand="btnSIEdit_Command" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <ItemStyle  />
                                                                <FooterStyle BorderStyle="None" />
                                                            </asp:TemplateField>--%>


                <asp:TemplateField HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lblSNo" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemStyle  />
                    <FooterStyle BorderStyle="None" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Name">
                    <ItemTemplate>
                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterStyle BorderStyle="None" />
                    <ItemStyle  />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Invoice No">
                    <ItemTemplate>
                        <asp:LinkButton ID="lblInvoiceNo" runat="server" Text='<%# Eval("Invoice_No") %>'
                            OnClick="lblgvInvoiceNo_Click" Font-Bold="true" CommandArgument='<%#Eval("Ref_Id") + "," + Eval("Ref_Type")+ "," + Eval("Location_Id")%>' />
                    </ItemTemplate>
                    <FooterStyle BorderStyle="None" />
                    <ItemStyle  />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ref. Order">
                    <ItemTemplate>
                        <asp:Label ID="lblPoNo" runat="server" Text='<%# Eval("Po_No") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterStyle BorderStyle="None" />
                    <ItemStyle  />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Invoice Date">
                    <ItemTemplate>
                        <asp:Label ID="lblInvoiceDate" Width="80px" runat="server" Text='<%# GetDateFormat(Eval("Invoice_Date").ToString()) %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle  />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Due Date">
                    <ItemTemplate>
                        <asp:Label ID="lblDueDate" runat="server" Text='<%# GetDateFormat(Eval("paymentDate").ToString()) %>'></asp:Label>
                    </ItemTemplate>
                    <FooterStyle BorderStyle="None" />
                    <ItemStyle  />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Payement Terms">
                    <ItemTemplate>
                        <asp:Label ID="lblPaymentTerms" runat="server" Text='<%# Eval("Payment_Terms") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterStyle BorderStyle="None" />
                    <ItemStyle  />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Days Overdue">
                    <ItemTemplate>
                        <asp:Label ID="lblDaysOverdue" runat="server" Text='<%# Eval("Due_Days") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblgvTotal" runat="server" Text="TOTAL:" />
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Right"  />
                    <ItemStyle  />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Currency">
                    <ItemTemplate>
                        <asp:Label ID="lblCurrency" runat="server" Text='<%# Eval("Currency_Name") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterStyle BorderStyle="None" />
                    <ItemStyle  />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Invoice Amount">
                    <ItemTemplate>
                        <asp:Label ID="lblInvoiceAmt" runat="server" Text='<%# SystemParameter.SetDecimal(Eval("actual_Invoice_amt").ToString(),Session["DBConnection"].ToString(),Session["CompId"].ToString(),Session["LocId"].ToString()) %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblgvInvoiceAmtTotal" runat="server" />
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Right"  />
                    <ItemStyle HorizontalAlign="Right"  />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Balance Remaining">
                    <ItemTemplate>
                        <asp:Label ID="lblDueAmt" runat="server" Text='<%# SystemParameter.SetDecimal(Eval("actual_balance_amt").ToString(),Session["DBConnection"].ToString(),Session["CompId"].ToString(),Session["LocId"].ToString()) %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblgvDueAmtTotal" runat="server" />
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Right"  />
                    <ItemStyle HorizontalAlign="Right"  />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Location">
                    <ItemTemplate>
                        <asp:Label ID="lblLocationName" runat="server" Text='<%# Eval("Location_Name") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterStyle BorderStyle="None" />
                    <ItemStyle  />
                </asp:TemplateField>
            </Columns>
            
            
            
            <PagerStyle CssClass="pagination-ys" />
            
        </asp:GridView>
        <br />
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