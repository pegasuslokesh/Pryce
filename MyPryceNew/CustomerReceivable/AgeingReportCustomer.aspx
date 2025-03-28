<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="AgeingReportCustomer.aspx.cs" Inherits="CustomerReceivable_AgeingReportCustomer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/ContactInfo.ascx" TagName="ViewContact" TagPrefix="AT1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/product_category.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Customer Ageing Report%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Finance Setup%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Customer Ageing Report%>"></asp:Label></li>
    </ol>
    <script>
        function resetPosition1() {

        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:UpdatePanel ID="Update_New" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-md-12">
                                    <asp:Label ID="lblCustomer" runat="server" Text="<%$ Resources:Attendance,Customer %>" />
                                    <%--<a style="color: Red">*</a>
        <asp:RequiredFieldValidator EnableClientScript="true" style="float:right;" runat="server" id="RequiredFieldValidator3" ValidationGroup="Save" 
        Display="Dynamic" SetFocusOnError="true" controltovalidate="txtCustomerName" errormessage="<%$ Resources:Attendance,Enter Customer%>"></asp:RequiredFieldValidator>--%>

                                    <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                        AutoPostBack="True" OnTextChanged="txtCustomerName_TextChanged" />
                                    <cc1:AutoCompleteExtender ID="txtCustomerName_AutoCompleteExtender" runat="server"
                                        CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                        ServiceMethod="GetCompletionListCustomer" ServicePath="" TargetControlID="txtCustomerName"
                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                    </cc1:AutoCompleteExtender>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblFromDate" runat="server" Text="<%$ Resources:Attendance,From Date %>" />
                                    <a style="color: Red">*</a>
                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtFromDate" ErrorMessage="<%$ Resources:Attendance,Enter From Date%>"></asp:RequiredFieldValidator>

                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" />
                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender_VoucherDate" runat="server" TargetControlID="txtFromDate"
                                        Format="dd/MMM/yyyy" />
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblToDate" runat="server" Text="<%$ Resources:Attendance,To Date %>" />
                                    <a style="color: Red">*</a>
                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtToDate" ErrorMessage="<%$ Resources:Attendance,Enter To Date%>"></asp:RequiredFieldValidator>

                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" />
                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" TargetControlID="txtToDate"
                                        Format="dd/MMM/yyyy" />
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:CheckBox ID="chkAllInvoices" AutoPostBack="False" Text="All Invoices"
                                        TextAlign="Right" Checked="False" runat="server" />
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-2">
                                    </div>
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
                                    <div class="col-md-2">
                                    </div>
                                    <br />
                                </div>
                                <div id="td1" runat="server" class="col-md-6">
                                    <br />
                                    <asp:Label ID="lblAgeingDays" runat="server" Text="Overdue Days (>=)" />
                                    <asp:TextBox ID="txtOverdueDays" CssClass="form-control" runat="server" />
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                        TargetControlID="txtOverdueDays" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                    </cc1:FilteredTextBoxExtender>
                                    <%--<asp:TextBox ID="txtVoucherType" runat="server" CssClass="form-control" />--%>
                                    <br />
                                </div>
                                <div class="col-md-12" style="text-align: center">
                                    <asp:Button ID="btnGetReport" runat="server" ValidationGroup="Save" Text="<%$ Resources:Attendance,Execute %>"
                                        CssClass="btn btn-primary" OnClick="btnGetReport_Click" />

                                    <asp:Button ID="btnShowReport" runat="server" ValidationGroup="Save" Text="<%$ Resources:Attendance,Print %>"
                                        CssClass="btn btn-primary" OnClick="btnShowReport_Click" />


                                    <asp:Button ID="btnsendMail" runat="server" ValidationGroup="Save" Text="Send Mail"
                                        CssClass="btn btn-primary" OnClick="btnsendMail_Click" />
                                    <cc1:ConfirmButtonExtender ID="confirm2" runat="server" ConfirmText="Are you sure you want to send an email of Account statement to the customer ?"
                                        TargetControlID="btnsendMail">
                                    </cc1:ConfirmButtonExtender>

                                    <br />
                                </div>
                                <div class="col-md-12" runat="server" id="scrollArea" onscroll="SetDivPosition()" style="overflow: auto; max-height: 500px;">
                                        <asp:HiddenField ID="hfScrollPosition" runat="server" Value="0" />
                                        <asp:HiddenField ID="hdfCurrentRow" runat="server" />
                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GVAgeingReport" runat="server" OnSorting="GVAgeingReport_Sorting" AllowSorting="true" AutoGenerateColumns="False" Width="100%"
                                            ShowFooter="true">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChanged"
                                                            AutoPostBack="true" />
                                                    </HeaderTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="SNo.">
                                                    <ItemTemplate>
                                                        <%--<asp:Label ID="lblSNo" runat="server" Text='<%#Eval("Trans_Id")%>'></asp:Label>--%>
                                                        <asp:Label ID="Label1" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle />
                                                    <FooterStyle BorderStyle="None" />
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Name" SortExpression="Name">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkContact" runat="server" Text='<%# Eval("Name") %>' OnClick="lnkContact_Click"
                                                            Font-Bold="true" CommandArgument='<%#Eval("other_account_no")%>' />
                                                    </ItemTemplate>
                                                    <FooterStyle BorderStyle="None" />
                                                    <ItemStyle />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Invoice No" SortExpression="Invoice_No">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lblInvoiceNo" runat="server" Text='<%# Eval("Invoice_No") %>'
                                                            OnClick="lblgvInvoiceNo_Click" OnClientClick="SetSelectedRow(this)" Font-Bold="true" CommandArgument='<%#Eval("Ref_Id") + "," + Eval("Ref_Type")+ "," + Eval("Location_Id")%>' />
                                                    </ItemTemplate>
                                                    <FooterStyle BorderStyle="None" />
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PO Number" SortExpression="Po_No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPoNo" runat="server" Text='<%# Eval("Po_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterStyle BorderStyle="None" />
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Invoice Date" SortExpression="Invoice_Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInvoiceDate" Width="80px" runat="server" Text='<%# GetDateFormat(Eval("Invoice_Date").ToString()) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Due Date" SortExpression="paymentDate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDueDate" runat="server" Text='<%# GetDateFormat(Eval("paymentDate").ToString()) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterStyle BorderStyle="None" />
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Payement Terms" SortExpression="Payment_Terms">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPaymentTerms" runat="server" Text='<%# Eval("Payment_Terms") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterStyle BorderStyle="None" />
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Days Overdue" SortExpression="Due_Days">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDaysOverdue" runat="server" Text='<%# Eval("Due_Days") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblgvTotal" runat="server" Text="TOTAL:" />
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <ItemStyle />
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Currency" SortExpression="Currency_Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCurrency" runat="server" Text='<%# Eval("Currency_Name") %>'></asp:Label>

                                                    </ItemTemplate>
                                                    <FooterStyle BorderStyle="None" />
                                                    <ItemStyle />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Invoice Amount" SortExpression="actual_Invoice_amt">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInvoiceAmt" runat="server" Text='<%# Common.GetAmountDecimal(Eval("actual_Invoice_amt").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblgvInvoiceAmtTotal" runat="server" />
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Balance Remaining" SortExpression="actual_balance_amt">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDueAmt" runat="server" Text='<%# Common.GetAmountDecimal(Eval("actual_balance_amt").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblgvDueAmtTotal" runat="server" />
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Location" SortExpression="Location_Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLocationName" runat="server" Text='<%# Eval("Location_Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterStyle BorderStyle="None" />
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Sales Person" SortExpression="SalesPerson">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSalesPerson" runat="server" Text='<%# Eval("SalesPerson") %>'></asp:Label>
                                                    </ItemTemplate>
                                                     
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                            </Columns>

                                            <PagerStyle CssClass="pagination-ys" />

                                        </asp:GridView>
                                    
                                    <br />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Address_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#modelContactDetail" Text="Add Followup" />

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
    <div class="modal fade" id="modelContactDetail" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">

                <div class="modal-body">
                    <AT1:ViewContact ID="UcContactList" runat="server" />
                </div>


                <div class="modal-footer">
                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
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
        function Modal_Address_Open() {
            document.getElementById('<%= Btn_Address_Modal.ClientID %>').click();
        }
        function setScrollAndRow() {
            try {
                debugger;
                var rowIndex = $('#<%= hdfCurrentRow.ClientID %>').val();
                var parent = document.getElementById('<%= GVAgeingReport.ClientID %>');
                    var rowIndex = parseInt(rowIndex);
                    parent.rows[rowIndex + 1].style.backgroundColor = "#A1DCF2";
                    var h = document.getElementById("<%=hfScrollPosition.ClientID%>");
                    document.getElementById("<%=scrollArea.ClientID%>").scrollTop = h.value;
           
            }
            catch (e) {

            }

        }

        function SetSelectedRow(lnk) {
            //Reference the GridView Row.
            try
            {
                var row = lnk.parentNode.parentNode;
                $('#<%= hdfCurrentRow.ClientID %>').val(row.rowIndex - 1);
                row.style.backgroundColor = "#A1DCF2";
            }
            catch(e)
            {

            }
        }

        function SetDivPosition() {
            try{
                var intY = document.getElementById("<%=scrollArea.ClientID%>").scrollTop;
                var h = document.getElementById("<%=hfScrollPosition.ClientID%>");
                h.value = intY;
            }
            catch(e)
            {

            }
        }
    </script>
</asp:Content>
