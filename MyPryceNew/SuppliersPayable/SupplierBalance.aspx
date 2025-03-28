<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" CodeFile="SupplierBalance.aspx.cs" Inherits="SuppliersPayable_SupplierBalance" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Src="~/WebUserControl/ContactInfo.ascx" TagName="ViewContact" TagPrefix="AT1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/account_voucher.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Supplier Balance%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Finance Setup%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Supplier Balance%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Supplier Balance%>"></asp:Label></li>
    </ol>
        <script>
        function resetPosition1()
        {

        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_myModal" Style="display: none;" runat="server" data-toggle="modal" data-target="#myModal" Text="View Modal" />
            <asp:Button ID="Btn_Invoice_Statement_Detail" Style="display: none;" runat="server" data-toggle="modal" data-target="#Ageing_Detail" Text="View Modal" />
            <asp:Button ID="Btn_ContactInfo_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#modelContactDetail" Text="Add Contact" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgress11" runat="server" AssociatedUpdatePanelID="Update_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="Update_New" runat="server">
        <ContentTemplate>
            <div id="PnlSupplierBalance" runat="server" class="row">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-md-6">
                                    <asp:Label ID="lblFromDate" runat="server" Text="<%$ Resources:Attendance,From Date %>" />
                                    <a style="color: Red">*</a>
                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtFromDate" ErrorMessage="<%$ Resources:Attendance,Enter From Date%>"></asp:RequiredFieldValidator>

                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" />
                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender_VoucherDate" runat="server" TargetControlID="txtFromDate"
                                        Format="dd/MMM/yyyy" />
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblToDate" runat="server" Text="<%$ Resources:Attendance,To Date %>" />
                                    <a style="color: Red">*</a>
                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtToDate" ErrorMessage="<%$ Resources:Attendance,Enter To Date%>"></asp:RequiredFieldValidator>

                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" />
                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" TargetControlID="txtToDate"
                                        Format="dd/MMM/yyyy" />
                                    <br />
                                </div>
                                <div class="col-md-12" style="text-align: center">
                                    <asp:CheckBox ID="chkShowZero" runat="server" Text="<%$ Resources:Attendance,Show Zero Balance Account%>" />
                                    <br />
                                    <br />
                                </div>
                                <div class="col-md-2">
                                    <br />
                                </div>
                                <div class="col-md-3">
                                    <asp:ListBox ID="lstLocation" runat="server" Style="width: 100%;" Height="200px"
                                        SelectionMode="Multiple" CssClass="form-control" Font-Names="Trebuchet MS" Font-Size="Small"
                                        ForeColor="Gray"></asp:ListBox>
                                    <br />
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
                                    <br />
                                </div>
                                <div class="col-md-2">
                                    <br />
                                </div>
                                <div class="col-md-12" style="text-align: center">
                                    <asp:Button ID="btnGetReport" runat="server" ValidationGroup="Save" Text="<%$ Resources:Attendance,Execute %>"
                                        CssClass="btn btn-primary" OnClick="btnGetReport_Click" />

                                    <asp:Button ID="btnShowReport" runat="server" Text="<%$ Resources:Attendance,Print %>"
                                        CssClass="btn btn-primary" OnClick="btnShowReport_Click" />

                                    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                        CssClass="btn btn-primary" OnClick="btnCancel_Click" />
                                    <br />
                                </div>
                                <div class="col-md-12" runat="server" id="scrollArea" onscroll="SetDivPosition()" style="overflow: auto; max-height: 500px;">
                                    <br />
                                    <asp:HiddenField ID="hfScrollPosition" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdfCurrentRow" runat="server" />
                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GVAllSupplier" Width="100%" runat="server" ShowFooter="true" AutoGenerateColumns="false"
                                         AllowSorting="true" OnSorting="GVAllSupplier_Sorting">
                                        
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Supplier %>" SortExpression="Name">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkgvSupplier" runat="server" Text='<%# Eval("Name") %>' OnClick="lnkgvSupplier_Click"
                                                        Font-Bold="true" CommandArgument='<%#Eval("Supplier_id")%>' OnClientClick="SetSelectedRow(this)" />

                                                    <asp:HiddenField ID="hdnSupplierId" runat="server" Value='<%#Eval("Supplier_Id") %>' />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblgvTotal" runat="server" Text="<%$ Resources:Attendance,TOTAL: %>"
                                                        CssClass="labelComman" />
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Left"  />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Opening Balance %>" SortExpression="Opening_Final">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvOpeningBalance" runat="server" Text='<%#Eval("Opening_Final") %>' />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblgvOpeningBalanceTotal" runat="server" CssClass="labelComman" />
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right"  />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Debit%>" SortExpression="Debit_Final">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvDebitAmount" runat="server" Text='<%#Eval("Debit_Final") %>' />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblgvDebitAmountTotal" runat="server" CssClass="labelComman" />
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right"  />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Credit %>" SortExpression="Credit_Final">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvCreditAmount" runat="server" Text='<%#Eval("Credit_Final") %>' />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblgvCreditAmountTotal" runat="server" CssClass="labelComman" />
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right"  />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Balance%>" SortExpression="Closing_Final">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkAccountStatement" runat="server" Text='<%# Eval("Closing_Final") %>' OnClick="lnkAccountStatement_Click"
                                                        Font-Bold="true" CommandArgument='<%#Eval("OtherAccountId")%>' ToolTip="Account Statement" OnClientClick="SetSelectedRow(this)" />

                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblgvClosingBalanceTotal" runat="server" CssClass="labelComman" />
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right"  />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Foreign %>" SortExpression="ForeignClosing_Balance">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvForeignClosingBalance" runat="server" Text='<%#Eval("ForeignClosing_Balance") %>' />
                                                    <asp:HiddenField ID="hdn_currency_id" runat="server" Value='<%#Eval("currency_id") %>' />
                                                    <asp:HiddenField ID="hdn_currency_code" runat="server" Value='<%#Eval("currency_code") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right"  />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Ageing Balance%>" SortExpression="AgeingBalance_Final">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkInvoiceStatement" runat="server" Text='<%#Eval("AgeingBalance_final") %>' OnClick="lnkInvoiceStatement_Click"
                                                        Font-Bold="true" CommandArgument='<%#Eval("OtherAccountId").ToString() + "," + Eval("Currency_id").ToString()  %>' ToolTip="Supplier Invoice Statement" OnClientClick="SetSelectedRow(this)" />

                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblgvAgeingBalanceTotal" runat="server" CssClass="labelComman" />
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right"  />
                                            </asp:TemplateField>
                                        </Columns>
                                        
                                        
                                        
                                    </asp:GridView>
                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                    <asp:HiddenField ID="HDFCurrencyID" runat="server" />
                                    <asp:HiddenField ID="HDFCurrentSupplierID" runat="server" />
                                    <br />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
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
    <div class="modal fade" id="Ageing_Detail" tabindex="-1" role="dialog" aria-labelledby="Voucher_DetailLabel" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Voucher_DetailLabel">
                        <asp:Label ID="Label3" runat="server" Text="Supplier Invoice Statement" Font-Bold="true"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Voucher_Detail" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-12">
                                                    <dx:ASPxWebDocumentViewer ID="ReportViewer1" runat="server" Width="100%"></dx:ASPxWebDocumentViewer>
                                                    <%--<dx:ReportViewer ID="ReportViewer1" runat="server" Width="100%">
                                                    </dx:ReportViewer>--%>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">

                    <button type="button" class="btn btn-danger" data-dismiss="modal">
                        Close</button>


                </div>
            </div>
        </div>
    </div>

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
        function LI_Edit_Active() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }
        function LI_List_Active() {
            $("#Li_List").addClass("active");
            $("#List").addClass("active");

            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");
        }

        function myModal_Popup() {
            document.getElementById('<%= Btn_myModal.ClientID %>').click();
        }
        function myModal_Popup() {
            document.getElementById('<%= Btn_myModal.ClientID %>').click();
        }
        $('#Ageing_Detail').on('hidden.bs.modal', function () {
            $('#<%=HDFCurrentSupplierID.ClientID %>').val("0");
         });
         function Invoice_Statement_Popup() {
             document.getElementById('<%= Btn_Invoice_Statement_Detail.ClientID %>').click();
         }
         function Modal_ContactInfo_Open() {
             document.getElementById('<%= Btn_ContactInfo_Modal.ClientID %>').click();
         }
        function setScrollAndRow() {
            try {
                debugger;
                var rowIndex = $('#<%= hdfCurrentRow.ClientID %>').val();
                var parent = document.getElementById('<%= GVAllSupplier.ClientID %>');
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
        function displayMessage(strMessage)
        {
            debugger;
            alert(strMessage);
        }
    </script>
</asp:Content>
