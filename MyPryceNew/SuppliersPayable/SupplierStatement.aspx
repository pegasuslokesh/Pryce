<%@ Page Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="SupplierStatement.aspx.cs" Inherits="SuppliersPayable_SupplierStatement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <%-- <script src="../Scripts/jquery-3.3.1.js"></script>
    <script src="//cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js">


    </script>--%>
<style>
    #tblStatement {
        border-collapse: separate; /* Set to separate for both horizontal and vertical borders */
    }   
    #tblStatement th, #tblStatement td {
        border: 1px solid #000; /* Set the desired border color and style */
    }   
</style>



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/account_voucher.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Supplier Statement%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Finance Setup%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,General Ledger%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Supplier Statement%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Voucher_Detail" Style="display: none;" runat="server" data-toggle="modal" data-target="#Voucher_Detail" Text="View Modal" />
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
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="form-group">
                                <%-- <div class="col-md-12" id="div_inAactive_account" runat="server" visible="false">
                                    <asp:CheckBox ID="chkShowInActiveAccount" runat="server" Text="InActive Account Only" OnClick="SetContextKey()" ClientIDMode="Static" />
                                    <br />
                                </div>--%>
                                <div class="col-md-12">
                                    <asp:Label ID="lblSupplier" runat="server" Text="<%$ Resources:Attendance,Supplier %>" />
                                    <a style="color: Red">*</a>
                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSupplierName" ErrorMessage="<%$ Resources:Attendance,Enter Supplier%>"></asp:RequiredFieldValidator>

                                    <asp:TextBox ID="txtSupplierName" runat="server" CssClass="form-control" BackColor="#eeeeee" />
                                    <cc1:AutoCompleteExtender ID="txtSupplierName_AutoCompleteExtender" runat="server"
                                        CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                        ServiceMethod="GetCompletionList" ServicePath="" TargetControlID="txtSupplierName"
                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                    </cc1:AutoCompleteExtender>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblFromDate" runat="server" Text="<%$ Resources:Attendance,From Date %>" />
                                    <a style="color: Red">*</a>
                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Save"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtFromDate" ErrorMessage="Enter Valid Date"></asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" />
                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender_VoucherDate" runat="server" TargetControlID="txtFromDate"
                                        Format="dd/MMM/yyyy" />
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblToDate" runat="server" Text="<%$ Resources:Attendance,To Date %>" />
                                    <a style="color: Red">*</a>
                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="Save"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtToDate" ErrorMessage="Enter Valid Date"></asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" />
                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" TargetControlID="txtToDate"
                                        Format="dd/MMM/yyyy" />
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblSVoucherType" runat="server" Text="<%$ Resources:Attendance,Voucher Type %>" />
                                    <select id="ddlVType" class="form-control">
                                        <option value="--Select--">--Select--</option>
                                        <option value="PI">Purchase Invoice</option>
                                        <option value="PR">Purchase Return</option>
                                        <option value="JV">Journal Vouchers</option>
                                        <option value="PV">Payment Vouchers</option>
                                        <option value="SPV">Supplier Payment Vouchers</option>
                                        <option value="SDN">Supplier Debit Note</option>
                                        <option value="SCN">Supplier Credit Note</option>
                                    </select>
                                    <asp:DropDownList ID="ddlSVoucherType" runat="server" CssClass="form-control" Visible="false">
                                        <asp:ListItem Text="--Select--" Value="--Select--"></asp:ListItem>
                                        <asp:ListItem Text="Purchase Invoice" Value="PI"></asp:ListItem>
                                        <asp:ListItem Text="Purchase Return" Value="PR"></asp:ListItem>
                                        <asp:ListItem Text="Journal Vouchers" Value="JV"></asp:ListItem>
                                        <asp:ListItem Text="Payment Vouchers" Value="PV"></asp:ListItem>
                                        <asp:ListItem Text="Supplier Payment Vouchers" Value="SPV"></asp:ListItem>
                                        <asp:ListItem Text="Supplier Debit Note" Value="SDN"></asp:ListItem>
                                        <asp:ListItem Text="Supplier Credit Note" Value="SCN"></asp:ListItem>
                                    </asp:DropDownList>
                                    <br />
                                </div>
                                <div class="col-md-12">

                                    <div class="col-md-3">
                                        <asp:ListBox ID="lstLocation" runat="server" Style="width: 100%;" Height="200px" ClientIDMode="Static"
                                            SelectionMode="Multiple" CssClass="form-control" Font-Names="Trebuchet MS" Font-Size="Small"
                                            ForeColor="Gray"></asp:ListBox>
                                    </div>
                                    <div class="col-lg-2" style="text-align: center">
                                        <div style="margin-top: 35px; margin-bottom: 35px;" class="btn-group-vertical">
                                            <input type="button" id="btnPushDept" class="btn btn-info" value=">" onclick="pullAndPushLocations(this)" />
                                            <input type="button" id="btnPullDept" class="btn btn-info" value="<" onclick="pullAndPushLocations(this)" />
                                            <input type="button" id="btnPushAllDept" class="btn btn-info" value=">>" onclick="pullAndPushLocations(this)" />
                                            <input type="button" id="btnPullAllDept" class="btn btn-info" value="<<" onclick="pullAndPushLocations(this)" />

                                            <%--<asp:Button ID="btnPushDept" CssClass="btn btn-info" Text=">" ClientIDMode="Static"  />

                                            <asp:Button ID="btnPullDept" Text="<" runat="server" CssClass="btn btn-info"  ClientIDMode="Static" />

                                            <asp:Button ID="btnPushAllDept" Text=">>"  runat="server" CssClass="btn btn-info" ClientIDMode="Static"/>

                                            <asp:Button ID="btnPullAllDept" Text="<<"  runat="server" CssClass="btn btn-info" ClientIDMode="Static"/>--%>
                                        </div>
                                    </div>

                                    <div class="col-md-3">
                                        <asp:ListBox ID="lstLocationSelect" runat="server" Style="width: 100%;" Height="200px" ClientIDMode="Static" ViewStateMode="Enabled"
                                            SelectionMode="Multiple" CssClass="form-control" Font-Names="Trebuchet MS" Font-Size="Small"
                                            ForeColor="Gray"></asp:ListBox>

                                    </div>
                                    <asp:HiddenField ID="hdnLocationIds" runat="server" />
                                    <div class="col-md-2">
                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Reconciled %>" />
                                        <asp:TextBox ID="txtReconciled" Enabled="false" ReadOnly="true" runat="server" CssClass="form-control" />
                                        <cc1:ColorPickerExtender ID="txtPresentColorCode_ColorPickerExtender" runat="server"
                                            Enabled="True" TargetControlID="txtReconciled" SampleControlID="txtReconciled">
                                        </cc1:ColorPickerExtender>
                                        <br />
                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Conflicted %>" />
                                        <asp:TextBox ID="txtConflicted" Enabled="false" ReadOnly="true" runat="server" CssClass="form-control" />
                                        <cc1:ColorPickerExtender ID="ColorPickerExtender1" runat="server" Enabled="True"
                                            TargetControlID="txtConflicted" SampleControlID="txtConflicted">
                                        </cc1:ColorPickerExtender>
                                        <br />
                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Not Reconciled %>" />
                                        <asp:TextBox ID="txtNotReconciled" Enabled="false" ReadOnly="true" runat="server" CssClass="form-control" />
                                        <cc1:ColorPickerExtender ID="ColorPickerExtender2" runat="server" Enabled="True"
                                            TargetControlID="txtNotReconciled" SampleControlID="txtNotReconciled">
                                        </cc1:ColorPickerExtender>
                                        <br />
                                    </div>

                                </div>
                                <div class="col-md-6">
                                    <asp:CheckBox ID="chkAgeingAnalysis" OnCheckedChanged="chkAgeingAnalysis_CheckedChanged"
                                        AutoPostBack="true" Visible="false" runat="server" Text="<%$ Resources:Attendance,Ageing Analysis %>" />
                                    <asp:HiddenField ID="hdnAgeing" runat="server" Value="0" />
                                    <br />
                                </div>
                                <div class="col-md-12" style="text-align: center">
                                    <asp:Button ID="btnGetReport" runat="server" Text="<%$ Resources:Attendance,Execute %>" Visible="false"
                                        CssClass="btn btn-primary" OnClientClick="Getreport()" />
                                    <input type="button" id="btnExecute" value="Execute"
                                        class="btn btn-primary" onclick="Getreport()" />
                                    <asp:Button ID="btnShowReport" runat="server" Text="<%$ Resources:Attendance,Print %>" Visible="false"
                                        CssClass="btn btn-primary" OnClick="btnShowReport_Click" />
                                    <input type="button" id="btnPrint" onclick="printAcStatement()" class="btn btn-primary" value="Print" />
                                    <asp:Button ID="btnShowProductReport" runat="server" Text="<%$ Resources:Attendance,With Product Detail %>" Visible="false"
                                        CssClass="btn btn-primary" OnClick="btnShowProductReport_Click" />
                                    <input type="button" id="btnPrintStatementWithProduct" value="Print with Product Details" onclick="printWithProductDetail()" class="btn btn-primary" />

                                    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Reset %>" Visible="false"
                                        CssClass="btn btn-primary" OnClientClick="resetControlsValue()" />
                                    <input type="button" id="Button1" value="Reset"
                                        class="btn btn-primary" onclick="resetControlsValue()" />
                                    <asp:Button ID="btnAllSupplier" runat="server" Text="All Supplier Print" Visible="false"
                                        CssClass="btn btn-primary" OnClick="btnAllSupplier_Click" />
                                    <br />
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblOpeningDebit" runat="server" Text="<%$ Resources:Attendance,Opening Balance %>" />
                                    <asp:TextBox ID="txtOpeningDebit" runat="server" CssClass="form-control"
                                        ReadOnly="true" />
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblForeignCreditOpening" runat="server" Text="<%$ Resources:Attendance,Foreign %>" />
                                    <asp:TextBox ID="txtForeignCreditOpening" runat="server" CssClass="form-control"
                                        ReadOnly="true" />
                                    <br />
                                </div>
                                <div class="col-md-12">                                     
                                    <div >                                       
                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GVSStatement" Width="100%" runat="server" ShowFooter="true" AutoGenerateColumns="false" ClientIDMode="Static">

                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,View %>">
                                                    <ItemTemplate>
                                                        <input type="button" id="lnkViewVoucher" onclick='<%# "showVoucherDetail(" + Eval("Header_Trans_Id") + ");" %>' />
                                                        <asp:ImageButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("Header_Trans_Id") %>'
                                                            ImageUrl="~/Images/Detail1.png" Height="20px" ToolTip="<%$ Resources:Attendance,View %>"
                                                            OnCommand="lnkViewDetail_Command" CausesValidation="False" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Voucher No %>" SortExpression="Voucher_No">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkgvVoucherNo" runat="server" Text='<%# Eval("Voucher_No") %>' BackColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("row_color").ToString())%>'
                                                            OnClick="lblgvVoucherNo_Click" Font-Bold="true" CommandArgument='<%#Eval("Ref_Id") + "," + Eval("Ref_Type")+ "," + Eval("Header_Trans_Id")+ "," + Eval("Location_Id")%>' />
                                                        <asp:HiddenField ID="hdnDetailId" runat="server" Value='<%#Eval("Detail_Trans_Id") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Date %>" SortExpression="Voucher_Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgvVoucherDate" runat="server" Text='<%# Eval("Voucher_Date") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Type %>" SortExpression="Voucher_Type">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgvVoucherType" runat="server" Text='<%#Eval("Voucher_Type") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Narration %>" SortExpression="Narration">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgvNarattion" runat="server" Text='<%#Eval("Narration") %>' />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblgvTotal" runat="server" Text="<%$ Resources:Attendance,TOTAL: %>" />
                                                    </FooterTemplate>
                                                    <FooterStyle BorderStyle="None" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Refrence %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgvRefrenceNumber" runat="server" Text='<%#Eval("RefrenceNumber") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Debit %>" SortExpression="Debit_Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgvDebitAmount" runat="server" Text='<%#Eval("Debit_Amount") %>' />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblgvDebitTotal" runat="server" Text='<%#Eval("Debit_Total") %>' />
                                                    </FooterTemplate>
                                                    <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Credit %>" SortExpression="Credit_Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgvCreditAmount" runat="server" Text='<%#Eval("Credit_Amount") %>' />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblgvCreditTotal" runat="server" Text='<%#Eval("Credit_Total") %>' />
                                                    </FooterTemplate>
                                                    <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Balance %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgvBalance" runat="server" Text='<%#Eval("L_Balance") %>' />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblgvBalanceTotal" runat="server" />
                                                    </FooterTemplate>
                                                    <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Foreign%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgvForeignAmount" runat="server" Text='<%#Eval("Foreign_Amount") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Balance %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgvFVBalance" runat="server" Text='<%#Eval("F_Balance") %>' />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblgvFreignBalanceTotal" runat="server" />
                                                    </FooterTemplate>
                                                    <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Created %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgvCreatedBy" runat="server" Text='<%# Eval("CreatedBy") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Modified %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgvModifiedBy" runat="server" Text='<%# Eval("ModifiedBy") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>



                                        </asp:GridView>
                                        <asp:HiddenField ID="hdnCurrencyType" runat="server" />
                                        <asp:HiddenField ID="hdnCurrencyId" runat="server" />

                                       
                                        <div class="col-md-12" style="overflow:auto" >
                                            <table id="tblStatementHeader" class="table table-striped table-border">                                               
                                                <tbody>
                                                  <tr>
                                                      <td>
                                                          <table id="tblStatement" >
                                                              <tbody>

                                                              </tbody>
                                                              <tfoot>
                                                                  <tr>
                                                                      <th colspan="6" style="text-align: right">Total:</th>
                                                                      <th id="debitTotal"></th>
                                                                      <th id="creditTotal"></th>
                                                                      <th>&nbsp;</th>
                                                                      <th>&nbsp;</th>
                                                                      <th>&nbsp;</th>
                                                                      <th>&nbsp;</th>
                                                                      <th></th>
 </tr>
                                                              </tfoot>

                                                          </table>
                                                      </td>
                                                  </tr>
                                                </tbody>   
                                            </table>
                                        </div>
                                    </div>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblClosingCredit" runat="server" Text="<%$ Resources:Attendance,Closing Balance %>" />
                                    <asp:TextBox ID="txtClosingCredit" runat="server" CssClass="form-control"
                                        ReadOnly="true" />
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblForeignCreditClosing" runat="server" Text="<%$ Resources:Attendance,Foreign Closing %>" />
                                    <asp:TextBox ID="txtForeignCreditClosing" runat="server" CssClass="form-control"
                                        ReadOnly="true" />
                                    <br />
                                </div>
                                <div id="tdAgeing" runat="server" visible="false">
                                    <div class="col-md-6">
                                        <asp:Label ID="lblAgeingAnalysis" runat="server" Text="<%$ Resources:Attendance,Ageing Analysis %>" />
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="lbl0to30" runat="server" Text="<%$ Resources:Attendance,0-30 Days %>" />
                                        <asp:TextBox ID="txt0to30" ReadOnly="true" Width="80px" runat="server" CssClass="form-control" />
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="lbl31to60" runat="server" Text="<%$ Resources:Attendance,31-60 Days %>" />
                                        <asp:TextBox ID="txt31to60" ReadOnly="true" Width="80px" runat="server" CssClass="form-control" />
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="lbl61to90" runat="server" Text="<%$ Resources:Attendance,61-90 Days %>" />
                                        <asp:TextBox ID="txt61to90" runat="server" ReadOnly="true" Width="80px" CssClass="form-control" />
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="lbl91to180" runat="server" Text="<%$ Resources:Attendance,91-180 Days %>" />
                                        <asp:TextBox ID="txt91to180" runat="server" ReadOnly="true" Width="80px" CssClass="form-control" />
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="lbl181to365" runat="server" Text="<%$ Resources:Attendance,181-365 Days %>" />
                                        <asp:TextBox ID="txt181to365" runat="server" ReadOnly="true" Width="80px" CssClass="form-control" />
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="lblAbove365" runat="server" Text="<%$ Resources:Attendance,Above 365 Days %>" />
                                        <asp:TextBox ID="txtAbove365" runat="server" ReadOnly="true" Width="80px" CssClass="form-control" />
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtCreditBalanceAmount" ReadOnly="true" Width="100px" runat="server"
                                            CssClass="form-control" Visible="false" />
                                        <br />
                                    </div>
                                    <div class="col-md-6">
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

    <div class="modal fade" id="Voucher_Detail" tabindex="-1" role="dialog" aria-labelledby="Voucher_DetailLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Voucher_DetailLabel">
                        <asp:Label ID="Label7" runat="server" Text="Voucher Detail" Font-Bold="true"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Voucher_Detail" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <%--<div class="col-md-6">
                                                    <asp:Label ID="lblFinanceCode" runat="server" Text="<%$ Resources:Attendance,Finance Code %>" />
                                                    <asp:TextBox ID="txtFinanceCode" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>--%>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblLocation" runat="server" Text="<%$ Resources:Attendance,Location %>" />
                                                    <asp:TextBox ID="txtToLocation" ReadOnly="true" runat="server" CssClass="form-control" BackColor="#eeeeee" />
                                                    <br />
                                                </div>
                                                <%--<div class="col-md-6">
                                                    <asp:Label ID="lblDepartment" runat="server" Text="<%$ Resources:Attendance,Department %>" />
                                                    <asp:TextBox ID="txtDepartment" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>--%>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblVoucherType" runat="server" Text="<%$ Resources:Attendance,Voucher Type %>" />
                                                    <asp:DropDownList ID="ddlVoucherType" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="--Select--" Value="--Select--"></asp:ListItem>
                                                        <asp:ListItem Text="Purchase Invoice" Value="PI"></asp:ListItem>
                                                        <asp:ListItem Text="Purchase Return" Value="PR"></asp:ListItem>
                                                        <asp:ListItem Text="Journal Vouchers" Value="JV"></asp:ListItem>
                                                        <asp:ListItem Text="Payment Vouchers" Value="PV"></asp:ListItem>
                                                        <asp:ListItem Text="Sales Invoice" Value="SI"></asp:ListItem>
                                                        <asp:ListItem Text="Receive Vouchers" Value="RV"></asp:ListItem>
                                                        <asp:ListItem Text="Sales Return" Value="SR"></asp:ListItem>
                                                        <asp:ListItem Text="Supplier Payment Vouchers" Value="SPV"></asp:ListItem>
                                                        <asp:ListItem Text="Customer Receive Vouchers" Value="CRV"></asp:ListItem>
                                                        <asp:ListItem Text="Customer Debit Note" Value="CDN"></asp:ListItem>
                                                        <asp:ListItem Text="Supplier Debit Note" Value="SDN"></asp:ListItem>
                                                        <asp:ListItem Text="Customer Credit Note" Value="CCN"></asp:ListItem>
                                                        <asp:ListItem Text="Supplier Credit Note" Value="SCN"></asp:ListItem>
                                                        <asp:ListItem Text="PDC Customer" Value="PDC"></asp:ListItem>
                                                        <asp:ListItem Text="PDC Supplier" Value="PDS"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblVoucherNo" runat="server" Text="<%$ Resources:Attendance,Voucher No. %>" />
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtVoucherNo" ErrorMessage="<%$ Resources:Attendance,Enter Voucher No%>"></asp:RequiredFieldValidator>

                                                    <asp:TextBox ID="txtVoucherNo" runat="server" CssClass="form-control" ClientIDMode="Static" />
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblVoucherDate" runat="server" Text="<%$ Resources:Attendance,Voucher Date %>" />
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtVoucherDate" ErrorMessage="<%$ Resources:Attendance,Enter Voucher Date%>"></asp:RequiredFieldValidator>

                                                    <asp:TextBox ID="txtVoucherDate" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender2" runat="server" TargetControlID="txtVoucherDate"
                                                        Format="dd/MMM/yyyy" />
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <asp:RadioButton ID="rbCashPayment" runat="server" Text="Cash Payment"
                                                        GroupName="Pay" Enabled="false" OnCheckedChanged="rbCashPayment_CheckedChanged"
                                                        AutoPostBack="true" />
                                                    <asp:RadioButton ID="rbChequePayment" runat="server" Text="Cheque Payment"
                                                        GroupName="Pay" Enabled="false" OnCheckedChanged="rbCashPayment_CheckedChanged"
                                                        AutoPostBack="true" />
                                                    <asp:CheckBox ID="chkReconcile" Visible="false" runat="server" Text="<%$ Resources:Attendance, Reconcile%>" />
                                                    <br />
                                                </div>
                                                <div id="trCheque1" runat="server" visible="true">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblChequeIssueDate" runat="server" Text="Cheque Issue Date" />
                                                        <asp:TextBox ID="txtChequeIssueDate" ReadOnly="true" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender_txtChequeIssueDate" runat="server" TargetControlID="txtChequeIssueDate"
                                                            Format="dd/MMM/yyyy" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblChequeClearDate" runat="server" Text="Cheque Clear Date" />
                                                        <asp:TextBox ID="txtChequeClearDate" ReadOnly="true" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender_ChequeClearDate" runat="server" TargetControlID="txtChequeClearDate"
                                                            Format="dd/MMM/yyyy" />
                                                        <br />
                                                    </div>
                                                </div>
                                                <div id="trCheque2" runat="server" visible="true" class="col-md-6">
                                                    <asp:Label ID="lblChequeNo" runat="server" Text="Cheque No." />
                                                    <asp:TextBox ID="txtChequeNo" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <asp:Label ID="lblReference" runat="server" Text="<%$ Resources:Attendance,Refrence%>" />
                                                    <asp:TextBox ID="txtReference" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-12" style="overflow: auto">
                                                    <table id="tblVoucherDetail" class="table table-striped table-bordered">
                                                        <thead>
                                                            <tr>
                                                                <th>Account Name</th>
                                                                <th>Other Account</th>
                                                                <th>Narration</th>
                                                                <th>Debit Amount</th>
                                                                <th>Credit Amount</th>
                                                                <th>Currency</th>
                                                                <th>Foreign Amount</th>
                                                                <th>Exchange Rate</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody></tbody>
                                                    </table>
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="t" ShowFooter="true" runat="server" Width="100%"
                                                        AutoGenerateColumns="False" ClientIDMode="Static">

                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,S No. %>">
                                                                <ItemTemplate>
                                                                    <%#Container.DataItemIndex+1 %>
                                                                    <asp:Label ID="lblSNo" Visible="false" runat="server" Text='<%#Eval("Trans_Id") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Account Name %>">
                                                                <ItemTemplate>
                                                                    <asp:HiddenField ID="hdngvAccountNo" runat="server" Value='<%#Eval("Account_No") %>' />
                                                                    <asp:Label ID="lblgvAccountName" runat="server" Text='<%#GetAccountNameByTransId(Eval("Account_No").ToString())%>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Other Account No. %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvOtherAccountNo" runat="server" Text='<%#Ac_ParameterMaster.GetOtherAccountNameForDetail(Eval("Other_Account_No").ToString(),Eval("Account_No").ToString(),Session["CompId"].ToString(),Session["DBConnection"].ToString())%>' />
                                                                    <asp:HiddenField ID="hdnOtherAccountNo" runat="server" Value='<%#Eval("Other_Account_No") %>' />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblgvTotal" runat="server" Font-Bold="true"
                                                                        Text="<%$ Resources:Attendance,Total%>" />
                                                                </FooterTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <FooterStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Narration %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvNarration" runat="server" Text='<%#Eval("Narration") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Debit Amount %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvDebitAmount" runat="server" Text='<%#Eval("Debit_Amount") %>' />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblgvDebitTotal" Font-Bold="true" runat="server" />
                                                                </FooterTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <FooterStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Credit Amount %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCreditAmount" runat="server" Text='<%#Eval("Credit_Amount") %>' />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblgvCreditTotal" Font-Bold="true" runat="server" />
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <%--<asp:TemplateField HeaderText="<%$ Resources:Attendance, Cost Center %>" Visible="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvCostCenter" runat="server" Text='<%#Eval("CostCenter_ID") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle  HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Employee %>" Visible="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="hdngvEmployeeId" runat="server" Value='<%#Eval("Emp_Id") %>' />
                                                                                        <asp:Label ID="lblgvEmployee" runat="server" Text='<%#GetEmployeeName(Eval("Emp_Id").ToString())%>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle  HorizontalAlign="Center" />
                                                                                </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Currency %>">
                                                                <ItemTemplate>
                                                                    <asp:HiddenField ID="hdngvCurrencyId" runat="server" Value='<%#Eval("Currency_Id") %>' />
                                                                    <asp:Label ID="lblgvCurrency" runat="server" Text='<%#GetCurrencyName(Eval("Currency_Id").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Foreign Amount %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvForeignAmount" runat="server" Text='<%#Eval("Foreign_Amount") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Exchange Rate %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvExchangeRate" runat="server" Text='<%#Eval("Exchange_Rate") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvDetail" ShowFooter="true" runat="server" Width="100%"
                                                        AutoGenerateColumns="False" ClientIDMode="Static">

                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,S No. %>">
                                                                <ItemTemplate>
                                                                    <%#Container.DataItemIndex+1 %>
                                                                    <asp:Label ID="lblSNo" Visible="false" runat="server" Text='<%#Eval("Trans_Id") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Account Name %>">
                                                                <ItemTemplate>
                                                                    <asp:HiddenField ID="hdngvAccountNo" runat="server" Value='<%#Eval("Account_No") %>' />
                                                                    <asp:Label ID="lblgvAccountName" runat="server" Text='<%#GetAccountNameByTransId(Eval("Account_No").ToString())%>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Other Account No. %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvOtherAccountNo" runat="server" Text='<%#Ac_ParameterMaster.GetOtherAccountNameForDetail(Eval("Other_Account_No").ToString(),Eval("Account_No").ToString(),Session["CompId"].ToString(),Session["DBConnection"].ToString())%>' />
                                                                    <asp:HiddenField ID="hdnOtherAccountNo" runat="server" Value='<%#Eval("Other_Account_No") %>' />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblgvTotal" runat="server" Font-Bold="true"
                                                                        Text="<%$ Resources:Attendance,Total%>" />
                                                                </FooterTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <FooterStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Narration %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvNarration" runat="server" Text='<%#Eval("Narration") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Debit Amount %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvDebitAmount" runat="server" Text='<%#Eval("Debit_Amount") %>' />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblgvDebitTotal" Font-Bold="true" runat="server" />
                                                                </FooterTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <FooterStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Credit Amount %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCreditAmount" runat="server" Text='<%#Eval("Credit_Amount") %>' />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblgvCreditTotal" Font-Bold="true" runat="server" />
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <%--<asp:TemplateField HeaderText="<%$ Resources:Attendance, Cost Center %>" Visible="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblgvCostCenter" runat="server" Text='<%#Eval("CostCenter_ID") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle  HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Employee %>" Visible="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="hdngvEmployeeId" runat="server" Value='<%#Eval("Emp_Id") %>' />
                                                                                        <asp:Label ID="lblgvEmployee" runat="server" Text='<%#GetEmployeeName(Eval("Emp_Id").ToString())%>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle  HorizontalAlign="Center" />
                                                                                </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Currency %>">
                                                                <ItemTemplate>
                                                                    <asp:HiddenField ID="hdngvCurrencyId" runat="server" Value='<%#Eval("Currency_Id") %>' />
                                                                    <asp:Label ID="lblgvCurrency" runat="server" Text='<%#GetCurrencyName(Eval("Currency_Id").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Foreign Amount %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvForeignAmount" runat="server" Text='<%#Eval("Foreign_Amount") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Exchange Rate %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvExchangeRate" runat="server" Text='<%#Eval("Exchange_Rate") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                    <asp:HiddenField ID="hdnAccountNo" runat="server" />
                                                    <asp:HiddenField ID="hdnAccountName" runat="server" />
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
                    <asp:UpdatePanel ID="Update_Voucher_Detail_Button" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                            <asp:Button ID="Button2" runat="server" CausesValidation="False" CssClass="btn btn-primary" Visible="false"
                                OnClick="btnCancelPopLeave_Click" TabIndex="9" Text="<%$ Resources:Attendance,Cancel %>" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Voucher_Detail">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_Voucher_Detail_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_New">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div id="prgBar" class="modal_Progress" style="display: none">
        <div class="center_Progress">
            <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
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

        function Voucher_Detail_Popup() {
            document.getElementById('<%= Btn_Voucher_Detail.ClientID %>').click();
        }
       
        //$(document).ready(function () {
        //    $('#tblStatement').DataTable({
        //        info: true,
        //        oLanguage: {
        //            sZeroRecords: "No records to display",
        //            sSearch: "Search from all Records"
        //        },
        //        paging: false,
        //        sort: true,
        //        searching: true
        //    });
        //});

        function Getreport() {
            debugger;

            try
            {
            //Validate supplier text box value
            var supplier = $('#<%= txtSupplierName.ClientID %>').val();  
            var otherAccountId = supplier.substring(supplier.lastIndexOf("/") + 1, supplier.length);
            var supplierName = supplier.substring(0, supplier.lastIndexOf("/"));
            if(!$.isNumeric(otherAccountId))
            {
                throw "Supplier Name is not valid";
            }          

            //check selected laocations
            var locationIds = "";
            $("#lstLocationSelect option").each(function () {
                if (locationIds=="")
                {
                    locationIds = this.value;
                }
                else
                {
                    locationIds = locationIds + ',' + this.value;
                }
                
            });
            if (locationIds == '') {
                locationIds = '<%= Session["LocId"].ToString() %>';
            }
            
            //check from date and to date
            var fromDate;
            var toDate;
            if(Date.parse($('#<%= txtFromDate.ClientID %>').val()))
            {
                fromDate = $('#<%= txtFromDate.ClientID %>').val();
            }
            else
            {
                throw "Form date is not valid";
            }

            if(Date.parse($('#<%= txtToDate.ClientID %>').val()))
            {
                toDate = $('#<%= txtToDate.ClientID %>').val();
            }
            else
            {
                throw "To date is not valid";
            }
                
                var voucherType = $('#ddlVType').val();
                var colorReconciled = '#' + $('#<%= txtReconciled.ClientID %>').val();
                var colorConflicted = '#' + $('#<%= txtConflicted.ClientID %>').val();
                var colorNotReconciled = '#' + $('#<%= txtNotReconciled.ClientID %>').val();

                debugger;
            $.ajax({
                url: 'SupplierStatement.aspx/GetJsonSupplierStatement',
                method: 'post',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{'strSupplier':'" + supplier + "','strFromDate':'" + fromDate + "','strToDate':'" + toDate + "','strLocationIds':'" + locationIds + "','strVoucherType':'" + voucherType + "'}",
                success: function (data) {
                    debugger;
                    var newData;
                    try
                    {
                        newData = JSON.parse(data.d);
                    }
                    catch(err)
                    {
                        $('#prgBar').hide();
                        alert(data.d);
                    }
                    var data = newData.clsSupplierStatementDetail;
                    $('#<%= txtOpeningDebit.ClientID %>').val(newData.lOb);
                    $('#<%= txtForeignCreditOpening.ClientID %>').val(newData.fOb);
                    $('#<%= txtClosingCredit.ClientID %>').val(newData.lCb);
                    $('#<%= txtForeignCreditClosing.ClientID %>').val(newData.fCb);
                    var tbl = $('#tblStatement').dataTable(
                        {
                            destroy: true,
                            paging: false,
                            sorting: false,
                            searching: true,                           
                            scrollY: '500px', // Set the height of the scrollable area
                            scrollCollapse: true, // Enable scrolling
                            data: data,
                            columns: [
                                { data: "headerTransId", render: function (data) { return "<a class='btn btn-default btn-sm' title='View' onclick='showVoucherDetail(" + data + ")'><img src='../Images/Detail1.png' alt='Pegasus' height='15px' width='15px'></a>" }, bSortable: false },
                       { data: "voucherNo", title: "Voucher No", render: function (data,row,start,end) { return "<a onclick='openVoucherSouce(" + start.refId + "," + start.headerTransId + "," + start.locationId + ");'>" + start.voucherNo + " </a>" } ,className: "text-center"},
                                { data: "voucherDate", title: "Voucher Date", className: "text-center" },
                                /* { data: "Inv_Number", title: "Supllier Invoice No", className: "text-center" },*/
                                { data: "referenceNumber", title: "Reference No/Supplier Invoice no", className: "text-left" },
                                { data: "Inv_Date", title: "Supplier Invoice Date", className: "text-center" },
                        { data: "voucherType", title: "Voucher Type",className:"text-center" },
                                { data: "narration", title: "Narration", className: "text-left", footer: "total" },
                        
                        { data: "debitAmount", title: "Debit Amount",className:"text-center",footer:true },
                        { data: "creditAmount", title: "Credit Amount",className:"text-center",footer:true },
                        { data: "lBalance", title: "Local balance",className:"text-center" },
                        { data: "foreignAmount", title: "Foreign Amount",className:"text-center" },
                        { data: "fBalance", title: "Foreign Balance",className:"text-center" },
                        { data: "createdBy", title: "Created by",className:"text-left" },
                        { data: "modifiedBy", title: "Modifid by",className:"text-left"},
                        { data: "reconciledStatus",title: "Status",className:"text-left",bVisible:false }

                            ],
  rowCallback: function (row, data, index) {
                        if (data.reconciledStatus == "Reconciled") {
                            $('td', row).css('background-color', colorReconciled);
                        }
                        else if (data.reconciledStatus == "Conflicted") {
                            $('td', row).css('background-color', colorConflicted);
                        }
                        else
                       {
                           $('td', row).css('background-color', colorNotReconciled);
                       }
                    },
footerCallback: function (row, data, start, end, display) {
debugger;
var api = this.api(),data;
var lastRow = data.pop();
var intVal = function ( i ) {
                return typeof i === 'string' ?
                    i.replace(/[\$,]/g, '')*1 :
                    typeof i === 'number' ?
                        i : 0;
            };

    try
    {
        $( api.column( 6 ).footer() ).html(lastRow.debitTotal);
        $( api.column( 7 ).footer() ).html(lastRow.creditTotal);
        $( api.column( 8 ).footer() ).html(lastRow.lBalance);
        $( api.column( 10 ).footer() ).html(lastRow.fBalance);
    }
    catch(err)
    {
         $( api.column( 6 ).footer() ).html('');
        $( api.column( 7 ).footer() ).html('');
        $( api.column( 8 ).footer() ).html('');
        $( api.column( 10 ).footer() ).html('');
    }

    
}                    
            

                        });
                }
            });
            }
            catch (err)
            {
                alert(err);
            }
        
        }

        
        function pullAndPushLocations(btn) {
            var listBox1 = $('#<%= lstLocation.ClientID %>');
            var listBox2 = $('#<%= lstLocationSelect.ClientID %>');
            if (btn.id == "btnPushDept") {
                listBox1.find('option:selected').appendTo(listBox2);
            }
            else if (btn.id == "btnPushAllDept") {
                listBox1.find('option').appendTo(listBox2);
            }
            else if (btn.id == "btnPullDept") {
                listBox2.find('option:selected').appendTo(listBox1);
            }
            else if (btn.id == "btnPullAllDept") {
                listBox2.find('option').appendTo(listBox1);
            }
        }
        function resetControlsValue() {
            $('#<%= txtSupplierName.ClientID %>').val("");
            $('#<%= txtOpeningDebit.ClientID %>').val("");
            $('#<%= txtForeignCreditOpening.ClientID %>').val("");
            $('#<%= txtClosingCredit.ClientID %>').val("");
            $('#<%= txtForeignCreditClosing.ClientID %>').val("");
            $('#<%= lstLocationSelect.ClientID %>').find('option').appendTo('#<%= lstLocation.ClientID %>');
            $('#<%= GVSStatement.ClientID %>').remove();
            $('#tblStatement tbody').html('');

        }

        function showVoucherDetail(voucherId) {
            $.ajax({
                url: 'SupplierStatement.aspx/GetVoucherData',
                method: 'post',
                contentType: "application/json; charset=utf-8",
                data: "{'strVoucherNo':'" + voucherId + "'}",
                success: function (data) {
                    var voucher_header = $.parseJSON(data.d);
                    var voucher_detail = voucher_header.voucherDetail;
                    $('#<%=txtToLocation.ClientID %>').val(voucher_header.locationName);
                    $('#txtVoucherNo').val(voucher_header.voucherNo);
                    $('#<%=txtVoucherDate.ClientID %>').val(voucher_header.voucherDate);
                    if (voucher_header.field2 == "Cheque") {
                        $('#<%=rbChequePayment.ClientID %>').prop('checked', true);
                        $('#<%=trCheque1.ClientID %>').attr('style', 'display:block');
                        $('#<%=trCheque2.ClientID %>').attr('style', 'display:block');
                         
                    }
                    else {
                        $('#<%= rbCashPayment.ClientID %>').prop('checked', true);
                        $('#<%=trCheque1.ClientID %>').attr('style', 'display:none');
                        $('#<%=trCheque2.ClientID %>').attr('style', 'display:none');
                        
                    }
                    $('#<%=ddlVoucherType.ClientID %>').val(voucher_header.voucherType);
                    $('#<%=txtChequeIssueDate.ClientID %>').val(voucher_header.chequeIssueDate);
                    $('#<%=txtChequeClearDate.ClientID %>').val(voucher_header.chequeClearDate);
                    $('#<%=txtChequeNo.ClientID %>').val(voucher_header.chequeNo);
                    $('#<%=txtReference.ClientID %>').val(voucher_header.referenceNo);

                    $('#tblVoucherDetail').dataTable({
                        destroy: true,
                        info: false,
                        paging: false,
                        sort: false,
                        searching: false,
                        data: voucher_detail,
                        columns: [
                        { data: "accountName", title: "Account Name", className: "text-left" },
                        { data: "otherAccountName", title: "Other Account", className: "text-left" },
                        { data: "narration", title: "Narration", className: "text-left" },
                        { data: "debitAmount", title: "Debit Amount", className: "text-right" },
                        { data: "creditAmount", title: "Credit Amount", className: "text-right" },
                        { data: "currencyCode", title: "Currency", className: "text-left" },
                        { data: "foreignAmount", title: "Foreign Amount", className: "text-right" },
                        { data: "exchangeRate", title: "Exchange Rate", className: "text-right" }
                        ]

                    });


                    $('#Voucher_Detail').modal('show')
                }
            });
        }
        function printAcStatement() {
            if ($('#<%= txtClosingCredit.ClientID %>').val() != "") {
                window.open('../Accounts_Report/SupplierStatement.aspx', 'window', 'width=1024, ');
            }
        }
        function printWithProductDetail() {
            if ($('#<%= txtClosingCredit.ClientID %>').val() != "") {
                window.open('../Accounts_Report/SupplierStatementWithProductDetail.aspx', 'window', 'width=1024, ');
            }
        }
        function openVoucherSouce(refId, headerTransId, locationId) {
            debugger;
            if (refId == 0) {
                window.open("../VoucherEntries/VoucherDetail.aspx?Id=" + headerTransId + "&LocId=" + locationId, 'window', 'width=1024, ');
            }
            else {
                window.open("../Purchase/PurchaseInvoice.aspx?Id=" + refId + "&LocId=" + locationId, 'window', 'width=1024,');
            }
        }

        $(document).ajaxStart(function () {
            // show loader on start
            $("#prgBar").css("display", "block");
        }).ajaxSuccess(function () {
            // hide loader on success
            $("#prgBar").css("display", "none");
        }).ajaxError(function () {
            // hide loader on erro
            $("#prgBar").css("display", "none");
        });
    </script>
</asp:Content>
