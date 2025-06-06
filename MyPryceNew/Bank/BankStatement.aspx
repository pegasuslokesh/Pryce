﻿<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true"
    CodeFile="BankStatement.aspx.cs" Inherits="Bank_BankStatement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/account_voucher.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="Bank Statement"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Finance%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Bank%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Statement Of Bank Accouont%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_View_Popup" Style="display: none;" runat="server" data-toggle="modal"
                data-target="#View_Popup" Text="View Modal" />
            <asp:Button ID="Btn_myModal" Style="display: none;" runat="server" data-toggle="modal"
                data-target="#myModal" Text="View Modal" />
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
                                <div class="col-md-12">
                                    <asp:Label ID="lblAccountName" runat="server" Text="<%$ Resources:Attendance,Account Name%>" />
                                    <a style="color: Red">*</a>
                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                        ID="RequiredFieldValidator3" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                        ControlToValidate="txtAccountName" ErrorMessage="<%$ Resources:Attendance,Enter Account Name%>"></asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control" BackColor="#eeeeee" />
                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtAccountName"
                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
                                        CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                    </cc1:AutoCompleteExtender>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblFromDate" runat="server" Text="<%$ Resources:Attendance,From Date %>" />
                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" />
                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender_VoucherDate" runat="server"
                                        TargetControlID="txtFromDate" />
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblToDate" runat="server" Text="<%$ Resources:Attendance,To Date %>" />
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" />
                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server"
                                        TargetControlID="txtToDate" />
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblSVoucherType" runat="server" Text="<%$ Resources:Attendance,Voucher Type %>" />
                                    <asp:DropDownList ID="ddlSVoucherType" runat="server" CssClass="form-control">
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
                                <div id="trclosing" runat="server" visible="false" class="col-md-6">
                                    <asp:Label ID="lblClosingCreditBalance" runat="server" Text="<%$ Resources:Attendance,Closing Balance %>" />
                                    <asp:TextBox ID="txtClosingCreditBalance" ReadOnly="true" runat="server" CssClass="form-control" />
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-5">
                                        <asp:ListBox ID="lstLocation" runat="server" Style="width: 100%;" Height="200px"
                                            SelectionMode="Multiple" CssClass="form-control" Font-Names="Trebuchet MS" Font-Size="Small"
                                            ForeColor="Gray"></asp:ListBox>
                                    </div>
                                    <div class="col-lg-2" style="text-align: center">
                                        <div style="margin-top: 55px; margin-bottom: 40px;" class="btn-group-vertical">
                                            <asp:Button ID="btnPushDept" runat="server" CssClass="btn btn-info" Text=">" OnClick="btnPushDept_Click" />
                                            <asp:Button ID="btnPullDept" Text="<" runat="server" CssClass="btn btn-info" OnClick="btnPullDept_Click" />
                                            <asp:Button ID="btnPushAllDept" Text=">>" OnClick="btnPushAllDept_Click" runat="server"
                                                CssClass="btn btn-info" />
                                            <asp:Button ID="btnPullAllDept" Text="<<" OnClick="btnPullAllDept_Click" runat="server"
                                                CssClass="btn btn-info" />
                                        </div>
                                    </div>
                                    <div class="col-md-5">
                                        <asp:ListBox ID="lstLocationSelect" runat="server" Style="width: 100%;" Height="200px"
                                            SelectionMode="Multiple" CssClass="form-control" Font-Names="Trebuchet MS" Font-Size="Small"
                                            ForeColor="Gray"></asp:ListBox>
                                    </div>
                                </div>
                                <div class="col-md-12" style="text-align: center">
                                    <div class="col-md-3">
                                    </div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtReconciled" Enabled="false" ReadOnly="true" runat="server" CssClass="form-control" />
                                        <cc1:ColorPickerExtender ID="txtPresentColorCode_ColorPickerExtender" runat="server"
                                            Enabled="True" TargetControlID="txtReconciled" SampleControlID="txtReconciled">
                                        </cc1:ColorPickerExtender>
                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Reconciled %>" />
                                    </div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtConflicted" Enabled="false" ReadOnly="true" runat="server" CssClass="form-control" />
                                        <cc1:ColorPickerExtender ID="ColorPickerExtender1" runat="server" Enabled="True"
                                            TargetControlID="txtConflicted" SampleControlID="txtConflicted">
                                        </cc1:ColorPickerExtender>
                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Conflicted %>" />
                                    </div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtNotReconciled" Enabled="false" ReadOnly="true" runat="server"
                                            CssClass="form-control" />
                                        <cc1:ColorPickerExtender ID="ColorPickerExtender2" runat="server" Enabled="True"
                                            TargetControlID="txtNotReconciled" SampleControlID="txtNotReconciled">
                                        </cc1:ColorPickerExtender>
                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Not Reconciled %>" />
                                    </div>
                                    <div class="col-md-3">
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <br />
                                </div>
                                <div class="col-md-12" style="text-align: center">
                                    <asp:Button ID="btnGetReport" ValidationGroup="Save" runat="server" Text="<%$ Resources:Attendance,Execute %>"
                                        CssClass="btn btn-primary" OnClick="btnGetReport_Click" />
                                    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                        CssClass="btn btn-primary" OnClick="btnCancel_Click" />
                                    <asp:Button ID="btnShowReport" runat="server" Text="<%$ Resources:Attendance,Print %>"
                                        CssClass="btn btn-primary" OnClick="btnShowReport_Click" />
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <br />
                                </div>
                                <div id="Div_Credit" runat="server" visible="false" class="col-md-6">
                                    <asp:Label ID="lblOpeningCreditBalance" Visible="false" runat="server" Text="<%$ Resources:Attendance,Opening Credit Balance %>" />
                                    <asp:Label ID="lblMid" runat="server" Text=":" Visible="false" />
                                    <asp:TextBox ID="txtOpenCreditBalance" Visible="false" ReadOnly="true" runat="server"
                                        CssClass="form-control" />
                                    <br />
                                </div>
                                <div id="Div_Debit" runat="server" visible="false" class="col-md-6">
                                    <asp:Label ID="lblOpeningDebitBalance" Visible="false" runat="server" Text="<%$ Resources:Attendance,Opening Debit Balance %>" />
                                    <asp:Label ID="Lbl_Mid_Debit" runat="server" Text=":" Visible="false" />
                                    <asp:TextBox ID="txtOpenDebitBalance" Visible="false" ReadOnly="true" runat="server"
                                        CssClass="form-control" />
                                    <asp:HiddenField ID="hdnNOAId" runat="server" Value="0" />
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Width="100%"
                                        CssClass="ajax__tab_yuitabview-theme" OnClientActiveTabChanged="tabChanged">
                                        <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="Reconciled">
                                            <ContentTemplate>
                                                <asp:UpdatePanel ID="Update_TabPanel1" runat="server">
                                                    <ContentTemplate>
                                                        <div class="row">
                                                            <div class="col-md-12" runat="server" id="scrollArea" onscroll="SetDivPosition()" style="overflow: auto; max-height: 500px;">
                                                                <asp:HiddenField ID="hfScrollPosition" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdfCurrentRow" runat="server" />
                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GVSStatement" Width="100%" runat="server" ShowFooter="true" AutoGenerateColumns="false">

                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,View %>">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("Header_Trans_Id") %>'
                                                                                    ImageUrl="~/Images/Detail1.png" Height="20px" ToolTip="<%$ Resources:Attendance,View %>"
                                                                                    OnCommand="lnkViewDetail_Command" OnClientClick="SetSelectedRow(this)" CausesValidation="False" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Voucher No %>" SortExpression="Voucher_No">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvVoucherNo" runat="server" Text='<%#Eval("Voucher_No") %>' />
                                                                                <asp:HiddenField ID="hdnDetailId" runat="server" Value='<%#Eval("Detail_Trans_Id") %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Voucher Date %>" SortExpression="Voucher_Date">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvVoucherDate" runat="server" Text='<%#GetDate(Eval("Voucher_Date").ToString()) %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Bank Reconcile Date %>" SortExpression="Voucher_Date">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvVoucherDate_r" runat="server" Text='<%#GetDate(Eval("bankReconcileDate").ToString()) %>' />
                                                                                <%--<asp:LinkButton ID="lnk_gvReconcileDate" runat="server" data-toggle="modal" data-target="#myModal" Text="Add"></asp:LinkButton>--%>
                                                                                <asp:ImageButton ID="btnUpdateReconcileDate_r" runat="server" CommandArgument='<%#Eval("Detail_Trans_Id") + ";" + GetDate(Eval("bankReconcileDate").ToString()) %>'
                                                                                    ImageUrl="~/Images/edit.png" Height="20px" ToolTip="<%$ Resources:Attendance,update %>"
                                                                                    OnCommand="btnUpdateReconcileDate_Command" OnClientClick="SetSelectedRow(this)" CausesValidation="False" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Voucher Type %>" SortExpression="Voucher_Type">
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
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Debit %>" SortExpression="Debit_Amount">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvDebitAmount" runat="server" Text='<%#Eval("Debit_Amount") %>' />
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:Label ID="lblgvDebitTotal" runat="server" />
                                                                            </FooterTemplate>
                                                                            <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Credit %>" SortExpression="Credit_Amount">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvCreditAmount" runat="server" Text='<%#Eval("Credit_Amount") %>' />
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:Label ID="lblgvCreditTotal" runat="server" />
                                                                            </FooterTemplate>
                                                                            <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Balance %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvBalance" runat="server" />
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:Label ID="lblgvBalanceTotal" runat="server" />
                                                                            </FooterTemplate>
                                                                            <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Created By %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvCreatedBy" runat="server" Text='<%#GetEmployeeNameByEmpCode(Eval("CreatedBy").ToString()) %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Modified By %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvModifiedBy" runat="server" Text='<%#GetEmployeeNameByEmpCode(Eval("ModifiedBy").ToString()) %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                    </Columns>

                                                                    <PagerStyle CssClass="pagination-ys" />

                                                                </asp:GridView>
                                                            </div>

                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_TabPanel1">
                                                    <ProgressTemplate>
                                                        <div class="modal_Progress">
                                                            <div class="center_Progress">
                                                                <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                            </div>
                                                        </div>
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                        <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="Unreconciled">
                                            <ContentTemplate>
                                                <asp:UpdatePanel ID="Update_Projected" runat="server">
                                                    <ContentTemplate>
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div style="overflow: auto; max-height: 500px;">
                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvStatementNoReconciled" Width="100%" runat="server" ShowFooter="true"
                                                                        AutoGenerateColumns="false">

                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,View %>">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("Header_Trans_Id") %>'
                                                                                        ImageUrl="~/Images/Detail1.png" Height="20px" ToolTip="<%$ Resources:Attendance,View %>"
                                                                                        OnCommand="lnkViewDetail_Command" CausesValidation="False" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Voucher No %>" SortExpression="Voucher_No">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblgvVoucherNo" runat="server" Text='<%#Eval("Voucher_No") %>' />
                                                                                    <asp:HiddenField ID="hdnDetailId" runat="server" Value='<%#Eval("Detail_Trans_Id") %>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Voucher Date %>" SortExpression="Voucher_Date">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblgvVoucherDate" runat="server" Text='<%#GetDate(Eval("Voucher_Date").ToString()) %>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Bank Reconcile Date %>" SortExpression="Voucher_Date">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblgvVoucherDate_u" runat="server" Text='<%#GetDate(Eval("bankReconcileDate").ToString()) %>' />
                                                                                    <%--<asp:LinkButton ID="lnk_gvReconcileDate" runat="server" data-toggle="modal" data-target="#myModal" Text="Add"></asp:LinkButton>--%>
                                                                                    <asp:ImageButton ID="btnUpdateReconcileDate_u" runat="server" CommandArgument='<%#Eval("Detail_Trans_Id") + ";" + GetDate(Eval("bankReconcileDate").ToString()) %>'
                                                                                        ImageUrl="~/Images/Detail1.png" Height="20px" ToolTip="<%$ Resources:Attendance,update %>"
                                                                                        OnCommand="btnUpdateReconcileDate_Command" CausesValidation="False" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Voucher Type %>" SortExpression="Voucher_Type">
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
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Debit %>" SortExpression="Debit_Amount">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblgvDebitAmount" runat="server" Text='<%#Eval("Debit_Amount") %>' />
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:Label ID="lblgvDebitTotal" runat="server" />
                                                                                </FooterTemplate>
                                                                                <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Credit %>" SortExpression="Credit_Amount">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblgvCreditAmount" runat="server" Text='<%#Eval("Credit_Amount") %>' />
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:Label ID="lblgvCreditTotal" runat="server" />
                                                                                </FooterTemplate>
                                                                                <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Balance %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblgvBalance" runat="server" />
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:Label ID="lblgvBalanceTotal" runat="server" />
                                                                                </FooterTemplate>
                                                                                <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Created By %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblgvCreatedBy" runat="server" Text='<%#GetEmployeeNameByEmpCode(Eval("CreatedBy").ToString()) %>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Modified By %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblgvModifiedBy" runat="server" Text='<%#GetEmployeeNameByEmpCode(Eval("ModifiedBy").ToString()) %>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                        </Columns>

                                                                        <PagerStyle CssClass="pagination-ys" />

                                                                    </asp:GridView>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Projected">
                                                    <ProgressTemplate>
                                                        <div class="modal_Progress">
                                                            <div class="center_Progress">
                                                                <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                            </div>
                                                        </div>
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                    </cc1:TabContainer>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblCreditBalanceAmount" runat="server" Text="Reconciled Balance" />
                                    <asp:TextBox ID="txtCreditBalanceAmount" ReadOnly="true" runat="server" CssClass="form-control" />
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="Label3" runat="server" Text="Unreconciled Balance" />
                                    <asp:TextBox ID="txtCreditBalanceAmountNoReconciled" ReadOnly="true" runat="server"
                                        CssClass="form-control" />
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="Label7" runat="server" Text="Closing(Reconciled+Unreconciled)" />
                                    <asp:TextBox ID="txtTotalBalance" ReadOnly="true" runat="server" CssClass="form-control" />
                                    <br />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="modal fade" id="View_Popup" tabindex="-1" role="dialog" aria-labelledby="View_PopupLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="View_PopupLabel">Voucher</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_View_Popup" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblFinanceCode" runat="server" Text="<%$ Resources:Attendance,Finance Code %>" />
                                                    <asp:TextBox ID="txtFinanceCode" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblLocation" runat="server" Text="<%$ Resources:Attendance,Location To %>" />
                                                    <asp:TextBox ID="txtToLocation" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblDepartment" runat="server" Text="<%$ Resources:Attendance,Department %>" />
                                                    <asp:TextBox ID="txtDepartment" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
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
                                                <div class="col-md-12">
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblVoucherNo" runat="server" Text="<%$ Resources:Attendance,Voucher No. %>" />
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                        ID="RequiredFieldValidator2" ValidationGroup="Save_No" Display="Dynamic" SetFocusOnError="true"
                                                        ControlToValidate="txtVoucherNo" ErrorMessage="<%$ Resources:Attendance,Enter Voucher No%>"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtVoucherNo" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblVoucherDate" runat="server" Text="<%$ Resources:Attendance,Voucher Date %>" />
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                        ID="RequiredFieldValidator1" ValidationGroup="Save_No" Display="Dynamic" SetFocusOnError="true"
                                                        ControlToValidate="txtVoucherDate" ErrorMessage="<%$ Resources:Attendance,Enter Voucher Date%>"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtVoucherDate" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender2" runat="server"
                                                        TargetControlID="txtVoucherDate" Format="dd/MMM/yyyy" />
                                                    <br />
                                                </div>
                                                <div class="col-md-12" style="text-align: center">
                                                    <asp:RadioButton ID="rbCashPayment" runat="server" Text="Cash Payment" GroupName="Pay"
                                                        Enabled="false" OnCheckedChanged="rbCashPayment_CheckedChanged" AutoPostBack="true" />
                                                    <asp:RadioButton ID="rbChequePayment" Style="margin-left: 25px" runat="server" Text="Cheque Payment"
                                                        GroupName="Pay" Enabled="false" OnCheckedChanged="rbCashPayment_CheckedChanged"
                                                        AutoPostBack="true" />
                                                    <asp:CheckBox ID="chkReconcile" Visible="false" runat="server" Style="margin-left: 25px"
                                                        Text="<%$ Resources:Attendance, Reconcile%>" />
                                                    <br />
                                                    <br />
                                                </div>
                                                <div id="trCheque1" runat="server" visible="false">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblChequeIssueDate" runat="server" Text="Cheque Issue Date" />
                                                        <asp:TextBox ID="txtChequeIssueDate" ReadOnly="true" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender_txtChequeIssueDate"
                                                            runat="server" TargetControlID="txtChequeIssueDate" Format="dd/MMM/yyyy" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblChequeClearDate" runat="server" Text="Cheque Clear Date" />
                                                        <asp:TextBox ID="txtChequeClearDate" ReadOnly="true" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender_ChequeClearDate"
                                                            runat="server" TargetControlID="txtChequeClearDate" Format="dd/MMM/yyyy" />
                                                        <br />
                                                    </div>
                                                </div>
                                                <div class="col-md-6" id="trCheque2" runat="server" visible="false">
                                                    <asp:Label ID="lblChequeNo" runat="server" Text="Cheque No." />
                                                    <asp:TextBox ID="txtChequeNo" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <asp:Label ID="lblReference" runat="server" Text="<%$ Resources:Attendance,Refrence%>" />
                                                    <asp:TextBox ID="txtReference" ReadOnly="true" runat="server" CssClass="form-control" />
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <div style="overflow: auto; max-height: 500px;">
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvDetail" ShowFooter="true" runat="server" Width="100%"
                                                            AutoGenerateColumns="False">

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
                                                                        <asp:Label ID="lblgvOtherAccountNo" runat="server" Text='<%#Ac_ParameterMaster.Get_Other_Account_Name(Eval("Other_Account_No").ToString(),Eval("Account_No").ToString(),Session["CompId"].ToString(),Session["DBConnection"].ToString())%>' />
                                                                        <asp:HiddenField ID="hdnOtherAccountNo" runat="server" Value='<%#Eval("Other_Account_No") %>' />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblgvTotal" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total%>" />
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
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Cost Center %>" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvCostCenter" runat="server" Text='<%#Eval("CostCenter_ID") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Employee %>" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdngvEmployeeId" runat="server" Value='<%#Eval("Emp_Id") %>' />
                                                                        <asp:Label ID="lblgvEmployee" runat="server" Text='<%#GetEmployeeName(Eval("Emp_Id").ToString())%>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
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
                                                    <br />
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
                    <asp:UpdatePanel ID="Update_View_Popup_Button" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_View_Popup">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_View_Popup_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="myModalLabel">Update Bank Reconcile Date</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_myModal" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblReconcile_Date" runat="server" Text="<%$ Resources:Attendance,Bank Reconcile Date%>" />
                                                    <asp:TextBox ID="txtReconcileDate" runat="server" CssClass="form-control" />
                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender13" runat="server"
                                                        TargetControlID="txtReconcileDate" Format="dd/MMM/yyyy" />
                                                    <asp:HiddenField ID="hdnVoucherDetailId" runat="server" />
                                                    <br />
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
                    <asp:UpdatePanel ID="Update_myModal_Button" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="Btn_Update_Reconcile" runat="server" OnClick="Btn_Update_Reconcile_Click_1"
                                Text="Update" CssClass="btn btn-primary" />
                            <asp:Button ID="button_delete_reconcile" runat="server" OnClick="Btn_delete_Reconcile_Click"
                                Text="Delete" CssClass="btn btn-primary" />
                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_myModal">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress7" runat="server" AssociatedUpdatePanelID="Update_myModal_Button">
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

        function View_Popup_Popup() {
            document.getElementById('<%= Btn_View_Popup.ClientID %>').click();
        }

        function myModal_Popup() {
            document.getElementById('<%= Btn_myModal.ClientID %>').click();
        }
    </script>
    <script type="text/javascript">
        var lasttab = 0;
        function tabChanged(sender, args) {
            // do what ever i want with lastTab value
            lasttab = sender.get_activeTabIndex();
        }
        function setScrollAndRow() {
            try {
                debugger;
                var rowIndex = $('#<%= hdfCurrentRow.ClientID %>').val();
                var parent = document.getElementById('<%= GVSStatement.ClientID %>');
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
