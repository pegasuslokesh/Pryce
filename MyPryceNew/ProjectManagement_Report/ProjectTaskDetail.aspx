<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster_Old.master" AutoEventWireup="true" CodeFile="ProjectTaskDetail.aspx.cs" Inherits="ProjectManagement_Report_ProjectTaskDetailaspx" %>
<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


 <link href="../CSS/InvStyle.css" rel="stylesheet" type="text/css" />
       
    <table width="100%">
        <tr>
            <td>
                <table width="100%" cellpadding="0" cellspacing="0" bordercolor="#F0F0F0">
                    <tr bgcolor="#90BDE9">
                        <td >
                            <table>
                                <tr>
                                    <td>
                                        <img src="../Images/product_icon.png" width="31" height="30" alt="D" />
                                    </td>
                                    <td>
                                        <img src="../Images/seperater.png" width="2" height="43" alt="SS" />
                                    </td>
                                    <td style="padding-left: 5px">
                                        <asp:Label ID="lblHeader" runat="server" Text="Project Task Detail Report"
                                            CssClass="LableHeaderTitle"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td bgcolor="#ccddee" colspan="4" width="100%" height="500px" valign="top">
                        
                        <asp:Panel ID="pnlProjectfilter" runat="server" >
                        
                        <table width="100%" style="padding-left:43px;">
                                                            <tr>
                                                                <td  width="150px">
                                                                    <asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Project Name %>"
                                                                        CssClass="labelComman"></asp:Label>
                                                                </td>
                                                                <td width="1px">
                                                                  :
                                                                </td >
                                                                <td  width="150px" style="height: 30px;"  >
                                                                    <asp:DropDownList ID="ddlprojectname" runat="server" 
                                                                         AutoPostBack="True" CssClass="textComman"
                                                                        onselectedindexchanged="ddlprojectname_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                </td>
                                                                </tr>
                                                                
                                                                <tr>
                                                                <td colspan="3">
                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvrProjecttask" runat="server" AutoGenerateColumns="False" Width="100%"
                                            AllowPaging="True" OnPageIndexChanging="GvrProjecttask_PageIndexChanging" AllowSorting="True"
                                             OnSorting="GvrProjecttask_Sorting">
                                            
                                            <Columns>
                                               
                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkgvSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkgvSelect_CheckedChanged" />
                                                                    </ItemTemplate>
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChanged"
                                                                            AutoPostBack="true" />
                                                                    </HeaderTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                               
                                                   <asp:TemplateField HeaderText="<%$ Resources:Attendance,Project Name %>" SortExpression="Emp_Name">
                                                    <ItemTemplate>
                                                        
                                                        <asp:Label ID="lblprojectName" runat="server" Text='<%# Eval("Project_Name") %>'></asp:Label>
                                                        
                                                        <asp:Label ID="lblProjectId" Visible="false" runat="server" Text='<%# Eval("Task_Id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Assign To %>" SortExpression="Emp_Name">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="HiddeniD" runat="server" />
                                                        <asp:Label ID="lblprojectIdList22" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>
                                                
                                                 <asp:TemplateField HeaderText="<%$ Resources:Attendance,Assign By %>" SortExpression="Emp_Name">
                                                    <ItemTemplate>
                                                      
                                                        <asp:Label ID="lblprojectIdList12" runat="server" Text='<%# GetAssignBy(Eval("CreatedBy").ToString()) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Subject %>" SortExpression="Subject">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmpnameList1" runat="server" Text='<%# Eval("Subject") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Assign Date %>" SortExpression="Assign_Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmpnameList2" runat="server"  Text='<%# Formatdate(Eval("Assign_Date")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Assign Time %>" SortExpression="Assign_Time">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmpnameList3" runat="server" Text='<%# FormatTime(Eval("Assign_Time")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>
                                                
                                                 <asp:TemplateField HeaderText="<%$ Resources:Attendance,Expected Close Date %>" SortExpression="Emp_Close_Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmpnameList121" runat="server"  Text='<%# Formatdate(Eval("Emp_Close_Date")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>
                                                
                                                
                                                 <asp:TemplateField HeaderText="<%$ Resources:Attendance,Expected Close Time %>" SortExpression="Emp_Close_Time">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmpnameList131" runat="server" Text='<%# FormatTime(Eval("Emp_Close_Time")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>
                                                
                                               
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>" SortExpression="Status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmpnameList4" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>
                                                
                                                 <asp:TemplateField HeaderText="<%$ Resources:Attendance,Mail Staus %>" SortExpression="Status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmpnameList42" runat="server" Text='<%# Eval("Field2") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>
                                                
                                                
                                            </Columns>
                                            
                                            
                                            
                                        </asp:GridView>
                                        <asp:Label ID="lblSelectRecord" runat="server" Visible="false"></asp:Label>
                                                                </td>
                                                                
                                                                </tr>
                                                                
                                                                
                                                                
                                                                <tr>
                                              
                                                <td style="padding:11px;" colspan="3" >
                                                    <asp:Button ID="btnsave" runat="server" CssClass="buttonCommman" OnClick="btnsave_Click" Visible="false"
                                                        Text="<%$ Resources:Attendance,Next%>" />&nbsp;&nbsp; &nbsp;&nbsp;
                                                     
                                                    <asp:Button ID="btnreset" runat="server" CssClass="buttonCommman" OnClick="btnreset_Click"
                                                        Text="<%$ Resources:Attendance,Reset%>" />
                                                  
                                                </td>
                                                
                                            </tr>
                                                                </table>
                        
                        
                        
                        </asp:Panel>
                        
                        <asp:Panel ID="pnlReport" runat="server" >
                                           
                                           
                                           <table width="100%">
                                <tr>
                                    <td style="width: 25%" >
                                        <asp:LinkButton ID="lnkback" runat="server" CssClass="acc" OnClick="lnkback_Click" Text="<%$ Resources:Attendance,Back %>"></asp:LinkButton>
                                    </td>
                                   
                                  
                                </tr>
                                    <tr>
                            <td>
                                <dx:ReportToolbar ID="rptToolBar" runat="server" ShowDefaultButtons="False" ReportViewer="<%# rptViewer %>"
                                    Width="100%" AccessibilityCompliant="True">
                                    <Items>
                                        <dx:ReportToolbarButton ItemKind="Search" />
                                        <dx:ReportToolbarSeparator />
                                        <dx:ReportToolbarButton ItemKind="PrintReport" />
                                        <dx:ReportToolbarButton ItemKind="PrintPage" />
                                        <dx:ReportToolbarSeparator />
                                        <dx:ReportToolbarButton Enabled="False" ItemKind="FirstPage" />
                                        <dx:ReportToolbarButton Enabled="False" ItemKind="PreviousPage" />
                                        <dx:ReportToolbarLabel ItemKind="PageLabel" />
                                        <dx:ReportToolbarComboBox ItemKind="PageNumber" Width="65px">
                                        </dx:ReportToolbarComboBox>
                                        <dx:ReportToolbarLabel ItemKind="OfLabel" />
                                        <dx:ReportToolbarTextBox IsReadOnly="True" ItemKind="PageCount" />
                                        <dx:ReportToolbarButton ItemKind="NextPage" />
                                        <dx:ReportToolbarButton ItemKind="LastPage" />
                                        <dx:ReportToolbarSeparator />
                                        <dx:ReportToolbarButton ItemKind="SaveToDisk" />
                                        <dx:ReportToolbarButton ItemKind="SaveToWindow" />
                                        <dx:ReportToolbarComboBox ItemKind="SaveFormat" Width="70px">
                                            <Elements>
                                                <dx:ListElement Value="pdf" />
                                                <dx:ListElement Value="xls" />
                                                <dx:ListElement Value="xlsx" />
                                                <dx:ListElement Value="rtf" />
                                                <dx:ListElement Value="mht" />
                                                <dx:ListElement Value="html" />
                                                <dx:ListElement Value="txt" />
                                                <dx:ListElement Value="csv" />
                                                <dx:ListElement Value="png" />
                                            </Elements>
                                        </dx:ReportToolbarComboBox>
                                    </Items>
                                    <Styles>
                                        <LabelStyle>
                                            <Margins MarginLeft="3px" MarginRight="3px" />
                                        </LabelStyle>
                                    </Styles>
                                </dx:ReportToolbar>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="pnlrptviewer" runat="server" Width="100%" Height="100%">
                                    <dx:ReportViewer ID="rptViewer" runat="server" AutoSize="False" Width="98%" Height="500px">
                                    </dx:ReportViewer>
                                </asp:Panel>
                            </td>
                        </tr>
                                
                              
                            </table>
                                            
                                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

</asp:Content>

