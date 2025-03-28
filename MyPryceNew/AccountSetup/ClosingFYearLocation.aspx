<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="ClosingFYearLocation.aspx.cs" Inherits="AccountSetup_ClosingFYearLocation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-hand-holding-usd"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Financial Year Location Closing%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Finance Setup%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Finance Setup%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Finance Closing%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
   
    <asp:UpdatePanel ID="Update_New" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-md-6">
                                    <asp:Label ID="lblFinanceCode" runat="server" Text="<%$ Resources:Attendance,Finance Code %>" />
                                    <asp:TextBox ID="txtFinanceCode" runat="server" CssClass="form-control" ReadOnly="true" />
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblFromDate" runat="server" Text="<%$ Resources:Attendance,From Date%>" />
                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" ReadOnly="true" />
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblToDate" runat="server" Text="<%$ Resources:Attendance,To Date%>" />
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" ReadOnly="true" />
                                    <br />
                                </div>
                                <div class="col-md-12" style="text-align: center">
                                    <asp:Button ID="btnExceute" runat="server" Text="<%$ Resources:Attendance,Execute %>"
                                        CssClass="btn btn-primary" OnClick="btnExceute_Click" />

                                    <asp:Button ID="btnCloseYear" runat="server" Text="<%$ Resources:Attendance,Close Year %>"
                                        CssClass="btn btn-success" OnClick="btnCloseYear_Click" />
                                    <asp:Button ID="btnTestYear" runat="server" Text="Test Year"
                                        CssClass="btn btn-success" OnClick="btnTestYear_Click" />
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <br />
                                    </div>
                                <div class="col-md-12">
                                    <div style="overflow: auto; max-height: 500px">
                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GVComplete" GridLines="None" ShowHeader="false" ShowFooter="false"
                                            Width="100%" runat="server" AutoGenerateColumns="false" >
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvIncome" Width="100%" runat="server" ShowFooter="true" AutoGenerateColumns="false"
                                                                        >
                                                                        
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, LIABILITIES %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblgvGroupName" runat="server" Text='<%#Eval("name") %>' />
                                                                                    <asp:HiddenField ID="hdngvParentId" runat="server" Value='<%#Eval("parent_id") %>' />
                                                                                    <asp:HiddenField ID="hdngvAccountId" runat="server" Value='<%#Eval("account_no") %>' />
                                                                                    <asp:HiddenField ID="hdngvForeignCB" runat="server" Value='<%#Eval("foreign_cb") %>' />
                                                                                    <asp:HiddenField ID="hdngvCompanyCB" runat="server" Value='<%#Eval("company_cb") %>' />
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:Label ID="lblgvTotal" runat="server" Text="<%$ Resources:Attendance,TOTAL: %>"
                                                                                        />
                                                                                </FooterTemplate>
                                                                                <FooterStyle BorderStyle="None" Font-Bold="true" />
                                                                                <ItemStyle  />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblgvCb" runat="server" Text='<%#Eval("cb") %>' />
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:Label ID="lblgvCbTotal" runat="server" />
                                                                                </FooterTemplate>
                                                                                <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                                <ItemStyle  HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblgvCbType" runat="server" Text='<%#Eval("cb_type") %>' />
                                                                                </ItemTemplate>
                                                                                <FooterStyle BorderStyle="None" HorizontalAlign="left" />
                                                                                <FooterTemplate>
                                                                                    <asp:Label ID="lblgvCbTypeTotal" runat="server" />
                                                                                </FooterTemplate>
                                                                                <FooterStyle BorderStyle="None" HorizontalAlign="left" Font-Bold="true" />
                                                                                <ItemStyle  HorizontalAlign="left" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        
                                                                        <PagerStyle CssClass="pagination-ys" />
                                                                        
                                                                    </asp:GridView>
                                                                </td>
                                                                <td>
                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvExpenses" Width="100%" runat="server" ShowFooter="true" AutoGenerateColumns="false"
                                                                        >
                                                                        
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,ASSETS %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblgvGroupName_1" runat="server" Text='<%#Eval("name") %>' />
                                                                                    <asp:HiddenField ID="hdngvParentId_1" runat="server" Value='<%#Eval("parent_id") %>' />
                                                                                    <asp:HiddenField ID="hdngvAccountId_1" runat="server" Value='<%#Eval("account_no") %>' />
                                                                                    <asp:HiddenField ID="hdngvForeignCB_1" runat="server" Value='<%#Eval("foreign_cb") %>' />
                                                                                    <asp:HiddenField ID="hdngvCompanyCB_1" runat="server" Value='<%#Eval("company_cb") %>' />
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:Label ID="lblgvTotal_1" runat="server" Text="<%$ Resources:Attendance,TOTAL: %>"
                                                                                        />
                                                                                </FooterTemplate>
                                                                                <FooterStyle BorderStyle="None" Font-Bold="true" />
                                                                                <ItemStyle  />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblgvCb_1" runat="server" Text='<%#Eval("cb") %>' />
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:Label ID="lblgvCbTotal_1" runat="server" />
                                                                                </FooterTemplate>
                                                                                <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                                <ItemStyle  HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblgvCbType_1" runat="server" Text='<%#Eval("cb_type") %>' />
                                                                                </ItemTemplate>
                                                                                <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                                <FooterTemplate>
                                                                                    <asp:Label ID="lblgvCbTypeTotal_1" runat="server" />
                                                                                </FooterTemplate>
                                                                                <FooterStyle BorderStyle="None" HorizontalAlign="left" Font-Bold="true" />
                                                                                <ItemStyle  HorizontalAlign="left" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        
                                                                        <PagerStyle CssClass="pagination-ys" />
                                                                        
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
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

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server"><script src="../Script/common.js"></script>
</asp:Content>
