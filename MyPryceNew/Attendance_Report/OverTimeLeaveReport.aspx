<%@ Page MasterPageFile="~/ERPMaster.master" Language="C#" AutoEventWireup="true" CodeFile="OverTimeLeaveReport.aspx.cs" Inherits="Attendance_Report_OverTimeLeaveReport" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <asp:Label ID="lblHeader" runat="server" Text="OverTime Report"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="Employee Leave Report"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_New" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnExportPDF" />
            <asp:PostBackTrigger ControlID="BtnExportExcel" />
        </Triggers>
        <ContentTemplate>
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
                            <div class="col-md-6">
                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Employee Name %>"></asp:Label>
                                <asp:TextBox ID="txtEmpName" runat="server" OnTextChanged="txtEmpName_textChanged" CssClass="form-control" BackColor="#eeeeee" AutoPostBack="true" />
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtEmpName" UseContextKey="True"
                                    CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                </cc1:AutoCompleteExtender>
                                <br />
                            </div>
                            <%--  <div class="col-md-6">
                                <asp:Label ID="Label27" runat="server" Text="Levae Type"></asp:Label>
                                <asp:DropDownList runat="server" id="ddlLeaveType" Class="form-control" DataTextField="Leave_Name" DataValueField="Leave_Name" >                                      
                                </asp:DropDownList>                              
                                                                            <br />
                                                   
                                <br />
                            </div> --%>
                            <div class="col-md-6">
                                <asp:Label ID="Label27" runat="server" Text="Location"></asp:Label>
                                <asp:DropDownList ID="ddlLocation" runat="server" Class="form-control">
                                </asp:DropDownList>
                                <asp:HiddenField ID="hdnEmpId" runat="server" />
                                <br />
                            </div>
                            <div class="col-md-12" style="text-align: center">
                                <asp:Button ID="btnFillgrid" runat="server" CausesValidation="False" Text="Get Detail" Visible="true" CssClass="btn btn-primary" OnClick="btnFillgrid_Click" />
                                <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>" CssClass="btn btn-primary" OnClick="btnReset_Click" CausesValidation="False" />
                                <asp:Button ID="BtnExportPDF" CssClass="btn btn-primary" runat="server" Text="Export PDF" OnClick="BtnExportPDF_Click" />
                                <asp:Button ID="BtnExportExcel" CssClass="btn btn-primary" runat="server" Text="Export Excel" OnClick="BtnExportExcel_Click" />
                                <br />
                            </div>

                        </div>
                    </div>
                </div>
            </div>

            <div class="box box-warning box-solid" <%= gvExportData.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                <%-- <div class="box-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div style="overflow: auto; max-height: 500px;">
                                <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server" GridViewID="gvExportData"></dx:ASPxGridViewExporter>
                                <dx:ASPxGridView ID="gvExportData" Width="100%" ClientInstanceName="grid" runat="server">
                                    <Settings ShowGroupPanel="true" ShowFilterRow="true" />
                                </dx:ASPxGridView>
                            </div>
                        </div>
                    </div>
                </div>--%>
                <div class="box-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div style="overflow: auto; max-height: 500px;">
                                <h3 style="position: center; text-align: center"><b>Balance of OverTime Leaves</b></h3>
                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvExportData" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                    Style="margin-top: 10px;" runat="server" AutoGenerateColumns="False" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Emp_Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpCode" runat="server" Text='<%#Eval("Emp_Code")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Employee_Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpCode" runat="server" Text='<%#Eval("Emp_Name")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Leave_Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpCode" runat="server" Text='<%#Eval("Leave_Name")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Assign Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpCode" runat="server" Text='<%# GetDate(Eval("Assign_Date"))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Days">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpCode" runat="server" Text='<%#Eval("TotalDays")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpCode" runat="server" Text='<%#Eval("Description")%>'></asp:Label>
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



            </div>


        </ContentTemplate>
    </asp:UpdatePanel>
    <script src="../Script/common.js"></script>
</asp:Content>
