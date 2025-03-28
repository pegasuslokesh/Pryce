<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPMaster.master" CodeFile="ContactMailReference.aspx.cs"
    Inherits="EmailSystem_ContactMailReference" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-cubes"></i>&nbsp;&nbsp;
                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Contact Mail Reference%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="Email System"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="Email System"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Contact Mail Reference%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">
                <ul class="nav nav-tabs pull-right bg-blue-gradient">
                    <li id="Li_List"><a href="#Archiving" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label4" runat="server" Text="Archving"></asp:Label></a></li>
                    <li id="Li_New" class="active">
                        <a href="#Contact" data-toggle="tab">
                            <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Contact%>"></asp:Label>
                        </a></li>

                </ul>

                <div class="tab-content">
                    <div class="tab-pane active" id="Contact">
                        <asp:UpdatePanel ID="upContact" runat="server">
                            <ContentTemplate>
                                <div class="col-md-12" style="float: right">
                                    <asp:Button ID="btnSend" runat="server" Text="Send" CssClass="buttonCommman" OnClick="btnSend_Click" />
                                </div>
                                <div class="col-md-12" style="overflow: auto">
                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvFinalContacts" runat="server" AllowPaging="False" AutoGenerateColumns="False"
                                        ShowFooter="False" BorderStyle="Solid" Width="95%">

                                        <Columns>
                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgBtnContactDelete" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                        Height="16px" ImageUrl="~/Images/Erase.png" Width="16px" OnCommand="imgBtnContactDelete_Command" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,S No. %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGvFinalContactsSerialNo" runat="server" Text='<%#Container.DataItemIndex+1 %>' />
                                                </ItemTemplate>
                                                <FooterStyle BorderStyle="None" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Contact %>" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGvFinalContactsContactID" runat="server" Text='<%# Eval("Trans_Id").ToString() %>'
                                                        Visible="false" />
                                                </ItemTemplate>
                                                <FooterStyle BorderStyle="None" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Company %>" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGvFinalContactsCompany_ID" runat="server" Text='<%# GetCompanyName(Eval("Company_Id").ToString()) %>'
                                                        Visible="true" />
                                                </ItemTemplate>
                                                <FooterStyle BorderStyle="None" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Department %>" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGvFinalContactsDepID" runat="server" Text='<%# GetDepartmentName(Eval("Dep_Id").ToString()) %>'
                                                        Visible="true" />
                                                </ItemTemplate>
                                                <FooterStyle BorderStyle="None" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Name%>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGvFinalContactsContactName" runat="server" Text='<%# Eval("Name") %>' />
                                                </ItemTemplate>
                                                <FooterStyle BorderStyle="None" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Email ID%>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGvFinalContactsEmailID" runat="server" Text='<%# Eval("Field1") %>' />
                                                </ItemTemplate>
                                                <FooterStyle BorderStyle="None" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Mobile No%>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGvFinalContactsMobileNo" runat="server" Text='<%# Eval("Field2") %>' />
                                                </ItemTemplate>
                                                <FooterStyle BorderStyle="None" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle />
                                            </asp:TemplateField>
                                        </Columns>




                                    </asp:GridView>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-4" style="overflow: auto">
                                        <div class="col-md-12">
                                            <asp:Panel ID="pnlContactType" runat="server">
                                                <table width="100%">
                                                    <tr>
                                                        <td width="100%" align="left" valign="top">
                                                            <asp:Label ID="Label3" runat="server" Text="Contact Type" Font-Bold="true" CssClass="labelComman"></asp:Label>
                                                            <br />
                                                            <table width="100%" style="background-color: White;">
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkCompany" runat="server" Enabled="false" Text="<%$ Resources:Attendance,Company%>"
                                                                            CssClass="labelComman" AutoPostBack="true" OnCheckedChanged="chkCompany_OnCheckedChanged" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkIndividual" runat="server" Enabled="false" Text="<%$ Resources:Attendance, Individual%>"
                                                                            CssClass="labelComman" AutoPostBack="true" OnCheckedChanged="chkCompany_OnCheckedChanged" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </div>
                                        <div class="col-md-12">
                                            <asp:Panel ID="pnlCompany" runat="server" Visible="false">
                                                <table width="100%">
                                                    <tr>
                                                        <td width="100%" align="left" valign="top">
                                                            <asp:Label ID="lblCompanyList" runat="server" Font-Bold="true" Text="Company List"
                                                                CssClass="labelComman"></asp:Label>
                                                            <br />
                                                            <asp:Panel ID="Panel2" runat="server" ScrollBars="Auto" Height="230px" Width="100%"
                                                                BackColor="White">
                                                                <asp:CheckBoxList ID="chkCompanyList" CssClass="labelComman" runat="server" RepeatDirection="Vertical"
                                                                    RepeatColumns="1" AutoPostBack="true" OnSelectedIndexChanged="chkCompanyList_OnSelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvCompany" runat="server" AllowPaging="False" AutoGenerateColumns="False"
                                                                    ShowFooter="False" BorderStyle="Solid" Width="100%" Visible="false">

                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkGvCompanySelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkGvCompanySelect_CheckedChanged" />
                                                                            </ItemTemplate>
                                                                            <FooterStyle BorderStyle="None" />
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,S No. %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblGvCompanySerialNo" runat="server" Text='<%#Container.DataItemIndex+1 %>' />
                                                                            </ItemTemplate>
                                                                            <FooterStyle BorderStyle="None" />
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Contact %>" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblGvCompanyID" runat="server" Text='<%# Eval("Trans_Id").ToString() %>'
                                                                                    Visible="false" />
                                                                            </ItemTemplate>
                                                                            <FooterStyle BorderStyle="None" />
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Name%>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblGvCompanyName" runat="server" Text='<%# Eval("Name") %>' />
                                                                            </ItemTemplate>
                                                                            <FooterStyle BorderStyle="None" />
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                    </Columns>




                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="100%" align="left" valign="top">
                                                            <asp:Label ID="lblDepartmentList" runat="server" Font-Bold="true" Text="Department List"
                                                                CssClass="labelComman"></asp:Label>
                                                            <br />
                                                            <asp:Panel ID="Panel3" runat="server" ScrollBars="Auto" Height="250px" Width="100%"
                                                                BackColor="White">
                                                                <asp:CheckBoxList ID="chkDepartmentList" CssClass="labelComman" runat="server" RepeatDirection="Vertical"
                                                                    RepeatColumns="1" AutoPostBack="true">
                                                                    <%-- OnSelectedIndexChanged="chkDepartmentList_OnSelectedIndexChanged" --%>
                                                                </asp:CheckBoxList>
                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvDepartment" runat="server" AllowPaging="False" AutoGenerateColumns="False"
                                                                    ShowFooter="False" BorderStyle="None" Width="100%" ShowHeader="false">
                                                                    <AlternatingRowStyle BorderStyle="None" />
                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkGvDepartmentSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkGvDepartmentSelect_CheckedChanged" />
                                                                            </ItemTemplate>
                                                                            <FooterStyle BorderStyle="None" />
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle BorderStyle="None" VerticalAlign="Top" />
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Company %>">
                                                                            <ItemTemplate>

                                                                                <%-- <asp:Label ID="lblGvDepartmentCompName" runat="server" CssClass="labelComman" Text='<%# GetCompanyName(Eval("Company_Id").ToString()) %>' />--%>
                                                                                <%--    /--%>

                                                                                <asp:Label ID="lblGvDepartmentName" runat="server" CssClass="labelComman" Text='<%# GetDepartmentName(Eval("Dep_Id").ToString()) %>' />
                                                                                <asp:Label ID="lblGvDepartmentID" runat="server" Text='<%# Eval("Dep_Id").ToString() %>'
                                                                                    Visible="false" />
                                                                                <asp:Label ID="lblGvDepartmentCompID" runat="server" Text='<%# Eval("Company_Id").ToString() %>'
                                                                                    Visible="false" />
                                                                            </ItemTemplate>
                                                                            <FooterStyle BorderStyle="None" />
                                                                            <HeaderStyle HorizontalAlign="Left" />

                                                                            <ItemStyle BorderStyle="None" HorizontalAlign="Left" />
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,S No. %>" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblGvCompanySerialNo" runat="server" Text='<%#Container.DataItemIndex+1 %>' />
                                                                            </ItemTemplate>
                                                                            <FooterStyle BorderStyle="None" />
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle BorderStyle="None" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <FooterStyle BorderStyle="None" />
                                                                    <HeaderStyle BorderStyle="None" />
                                                                    <PagerStyle BorderStyle="None" />
                                                                    <RowStyle BorderStyle="None" HorizontalAlign="Center" />
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </div>



                                    </div>
                                    <div class="col-md-8" style="overflow: auto">
                                        <div class="col-md-12">
                                            <asp:Panel ID="pnlIndividual" runat="server" Visible="false" Width="780px">
                                                <asp:Label ID="lblIndividual" runat="server" Text="Individual List" CssClass="labelComman"></asp:Label>
                                                <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Height="250px" Width="800px">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvIndividual" runat="server" AllowPaging="False" AutoGenerateColumns="False"
                                                        ShowFooter="False" BorderStyle="Solid" Width="95%">

                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkGvIndividualSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkGvIndividualSelectAll_CheckedChanged" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkgvIndividualselect" runat="server" AutoPostBack="true" OnCheckedChanged="chkgvIndividualselect_CheckedChanged" />
                                                                </ItemTemplate>
                                                                <FooterStyle BorderStyle="None" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,S No. %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGvIndividualSerialNo" runat="server" Text='<%#Container.DataItemIndex+1 %>' />
                                                                </ItemTemplate>
                                                                <FooterStyle BorderStyle="None" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Contact %>" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGvIndividualContactID" runat="server" Text='<%# Eval("Trans_Id").ToString() %>'
                                                                        Visible="false" />
                                                                </ItemTemplate>
                                                                <FooterStyle BorderStyle="None" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Name%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGvIndividualContactName" runat="server" Text='<%# Eval("Name") %>' />
                                                                </ItemTemplate>
                                                                <FooterStyle BorderStyle="None" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Email ID%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGvIndividualEmailID" runat="server" Text='<%# Eval("Field1") %>' />
                                                                </ItemTemplate>
                                                                <FooterStyle BorderStyle="None" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Mobile No%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGvIndividualMobileNo" runat="server" Text='<%# Eval("Field2") %>' />
                                                                </ItemTemplate>
                                                                <FooterStyle BorderStyle="None" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                        </Columns>




                                                    </asp:GridView>
                                                </asp:Panel>
                                            </asp:Panel>
                                        </div>
                                        <div class="col-md-12">
                                            <asp:Panel ID="pnlCompanyContact" runat="server" Visible="false" Width="780px">
                                                <asp:Label runat="server" ID="lblContacts" Text="Contact List" CssClass="labelComman"></asp:Label>
                                                <asp:Panel ID="Panel4" runat="server" ScrollBars="Auto" Height="100%" Width="800px">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvContact" runat="server" AllowPaging="False" AutoGenerateColumns="False"
                                                        ShowFooter="False" BorderStyle="Solid" Width="95%">

                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkgvContactSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkgvContactSelectAll_CheckedChanged" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkGvContactSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkGvContactSelect_CheckedChanged" />
                                                                </ItemTemplate>
                                                                <FooterStyle BorderStyle="None" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,S No. %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvContactSerialNo" runat="server" Text='<%#Container.DataItemIndex+1 %>' />
                                                                </ItemTemplate>
                                                                <FooterStyle BorderStyle="None" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Contact %>" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGvContactContactID" runat="server" Text='<%# Eval("Trans_Id").ToString() %>'
                                                                        Visible="false" />
                                                                </ItemTemplate>
                                                                <FooterStyle BorderStyle="None" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Company %>" Visible="true">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGvContactCompany_ID" runat="server" Text='<%# GetCompanyName(Eval("Company_Id").ToString()) %>'
                                                                        Visible="true" />
                                                                </ItemTemplate>
                                                                <FooterStyle BorderStyle="None" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Department %>" Visible="true">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGvContactCompanyID" runat="server" Text='<%# GetDepartmentName(Eval("Dep_Id").ToString()) %>'
                                                                        Visible="true" />
                                                                </ItemTemplate>
                                                                <FooterStyle BorderStyle="None" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Name%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGvContactContactName" runat="server" Text='<%# Eval("Name") %>' />
                                                                </ItemTemplate>
                                                                <FooterStyle BorderStyle="None" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Email ID%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGvContactEmailID" runat="server" Text='<%# Eval("Field1") %>' />
                                                                </ItemTemplate>
                                                                <FooterStyle BorderStyle="None" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Mobile No%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGvContactMobileNo" runat="server" Text='<%# Eval("Field2") %>' />
                                                                </ItemTemplate>
                                                                <FooterStyle BorderStyle="None" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                        </Columns>




                                                    </asp:GridView>
                                                </asp:Panel>
                                            </asp:Panel>
                                        </div>
                                    </div>

                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="Archiving">
                        <asp:UpdatePanel ID="upArchving" runat="server">
                            <ContentTemplate>
                                <div class="col-md-12">
                                    <br />
                                </div>
                                <div class="col-md-12" style="overflow: auto">
                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvFileMaster" runat="server" AutoGenerateColumns="False" Width="100%"
                                        AllowPaging="True" AllowSorting="True">
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                        ImageUrl="~/Images/Erase.png" OnCommand="IbtnDelete_Command" ToolTip="<%$ Resources:Attendance,Delete %>" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Download %>" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" OnCommand="lnkDownload_Command"
                                                        CommandArgument='<%#Eval("Trans_id") %>'></asp:LinkButton>
                                                    <asp:HiddenField ID="hdnTransId" runat="server" Value='<%#Eval("Trans_id") %>' />
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProductId" runat="server" Text='<%# Eval("Product_Id") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Document Name %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDocumentName" runat="server" Text='<%# Eval("Document_Name") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,File Name %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFileName" runat="server" Text='<%# Eval("File_Name") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>
                                        </Columns>




                                    </asp:GridView>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-4">
                                        <asp:Label ID="Label2" runat="server" Text="Archa Type"></asp:Label>
                                        <asp:DropDownList ID="ddlArchType" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="<%$ Resources:Attendance, Product Name%>" Value="ProductId"></asp:ListItem>
                                        </asp:DropDownList>
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="Label8" runat="server" Text="Product Name"></asp:Label>
                                        <asp:TextBox ID="txtEProductName" runat="server" CssClass="form-control"></asp:TextBox>
                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="100"
                                            DelimiterCharacters="" Enabled="True" MinimumPrefixLength="0" ServiceMethod="GetCompletionListProductName"
                                            ServicePath="" TargetControlID="txtEProductName" UseContextKey="True" CompletionListCssClass="completionList"
                                            CompletionListItemCssClass="listItem" CompletionListHighlightedItemCssClass="itemHighlighted">
                                        </cc1:AutoCompleteExtender>
                                        <br />
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Button ID="btnSerach" runat="server" Text="Search" CssClass="btn-primary"
                                            OnClick="btnsearch_click" />
                                        <asp:Button ID="btnArchReset" runat="server" Text="Reset" CssClass="btn-primary"
                                            OnClick="btnArchReset_click" />
                                        <br />
                                    </div>
                                </div>
                                <div class="col-md-12" style="overflow: auto">
                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvFileMasterSearch" runat="server" AutoGenerateColumns="False"
                                        Width="100%" AllowPaging="True" AllowSorting="True">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkbox" runat="server" AutoPostBack="true" OnCheckedChanged="chkbox_CheckedChanged" />
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Download %>" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" OnCommand="lnkDownload_Command"
                                                        CommandArgument='<%#Eval("Trans_id") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProductId" runat="server" Text='<%# Eval("Product_Id") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Document Name %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDocumentName" runat="server" Text='<%# Eval("Document_Name") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,File Name %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFileName" runat="server" Text='<%# Eval("File_Name") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>
                                        </Columns>




                                    </asp:GridView>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

            </div>
        </div>
    </div>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upArchving">
        <ProgressTemplate>
            <div id="Background">
            </div>
            <div id="Progress">
                <center>
                    <img src="../Images/ajax-loader2.gif" style="vertical-align: middle" alt="Pegasus Technologies" />
                </center>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="upContact">
        <ProgressTemplate>
            <div id="Background">
            </div>
            <div id="Progress">
                <center>
                    <img src="../Images/ajax-loader2.gif" style="vertical-align: middle" alt="Pegasus Technologies" />
                </center>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
</asp:Content>





