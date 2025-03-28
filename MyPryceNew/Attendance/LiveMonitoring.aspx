<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="LiveMonitoring.aspx.cs" Inherits="Attendance_LiveMonitoring" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-desktop"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Log Monitoring Setup%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Log Monitoring Setup%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Page" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="form-group">


                                <div class="col-md-4">
                                    <asp:Label ID="Label1" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Device Group %>"></asp:Label>
                                    <asp:DropDownList ID="ddlDeviceGroup" runat="server" Class="form-control" OnSelectedIndexChanged="ddlDeviceGroup_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                </div>

                                <div class="col-md-4">
                                    <asp:Label ID="Label2" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Select Device%>"></asp:Label>
                                    <asp:DropDownList ID="ddldeviceList" runat="server" Class="form-control" OnSelectedIndexChanged="ddldeviceList_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="lblSelectRecord" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Select Records : %>"></asp:Label>
                                    <asp:DropDownList ID="ddlSelectRecord" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlSelectRecord_SelectedIndexChanged">
                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                        <%--   <asp:ListItem Text="100" Value="100"></asp:ListItem>
                            <asp:ListItem Text="All" Value="all"></asp:ListItem>--%>
                                    </asp:DropDownList>
                                </div>


                                <div class="col-md-4">
                                    <asp:Label ID="lblLocation" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Location%>"></asp:Label>
                                    <asp:DropDownList ID="ddlLocation" runat="server" Class="form-control" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="lblDepartment" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Select Department%>"></asp:Label>
                                    <asp:DropDownList ID="dpDepartment" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dpDepartment_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>

                                <div class="col-md-12" style="overflow: auto;">
                                    <br />
                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvTheGrid" runat="server" Width="100%" AutoGenerateColumns="False"
                                        Visible="false">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Emp Code %> "
                                                HeaderStyle-Width="80" ItemStyle-Width="60">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmpId" runat="server" Text='<%# Eval("Emp_Code") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="80px" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Employee Name1%> "
                                                HeaderStyle-Width="80" ItemStyle-Width="60">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmpId" runat="server" Text='<%# Eval("Emp_Name") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="80px" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Date %> " HeaderStyle-Width="50"
                                                ItemStyle-Width="50">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDate" runat="server" Text='<%# GetDate(Eval("Event_Date")) %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="50px" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Time %> " HeaderStyle-Width="30"
                                                ItemStyle-Width="50">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTime" runat="server" Text='<%# Convert.ToDateTime(Eval("Event_Time")).ToString("HH:mm") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="30px" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,IP Address %> " HeaderStyle-Width="50"
                                                ItemStyle-Width="50">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDeviceNoG" runat="server" Text='<%# Eval("IP_Address") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="50px" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Device Name %> " HeaderStyle-Width="60">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDeviceG" runat="server" Text='<%# Eval("Device_Name") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="50px" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>


                                        <PagerStyle CssClass="pagination-ys" />

                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="Update_Timer" runat="server">
        <ContentTemplate>
            <asp:Timer ID="Timer1" runat="server" Interval="60000" OnTick="Timer1_Tick">
            </asp:Timer>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Page">
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

</asp:Content>

