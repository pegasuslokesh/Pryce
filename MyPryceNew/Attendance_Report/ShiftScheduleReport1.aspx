<%@ Page Language="C#"  AutoEventWireup="true" CodeFile="ShiftScheduleReport1.aspx.cs" Inherits="AttendanceReports_ShiftScheduleReport1" Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="exp1" Namespace="ControlFreak" Assembly="ExportPanel" %>
<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>



<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pegasus :: ERP</title>
</head>
<body>
    <form id="form1" runat="server">

<asp:scriptmanager runat="server" id="sm"></asp:scriptmanager>

    <link href="../CSS/InvStyle.css" rel="stylesheet" type="text/css" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="lnkBack" />
        </Triggers>
        <ContentTemplate>


            <table width="100%" runat="server" >
                <tr>
                    <td>
                        <table width="100%" cellpadding="0" cellspacing="0" bordercolor="#F0F0F0">
                            <tr bgcolor="#90BDE9">
                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                    <table>
                                        <tr>
                                            <td>
                                                <img src="../Images/product_icon.png" width="31" height="30" alt="D" />
                                            </td>
                                            <td>
                                                <img src="../Images/seperater.png" width="2" height="43" alt="SS" />
                                            </td>
                                            <td style="padding-left: 5px">
                                                <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Shift Schedule Report  %>"
                                                    CssClass="LableHeaderTitle"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>

                            </tr>
                            <tr>
                                <td bgcolor="#ccddee" colspan="4" width="100%" height="500px" valign="top">



                                    <asp:Panel ID="pnlEmpAtt" runat="server">

                                        <table width="100%">
                                            <tr>
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                    <asp:RadioButton ID="rbtnGroupSal" CssClass="labelComman" OnCheckedChanged="EmpGroupSal_CheckedChanged"
                                                        runat="server" Text="<%$ Resources:Attendance,Group %>" Font-Bold="true" GroupName="EmpGroupSal"
                                                        AutoPostBack="true" />
                                                    <asp:RadioButton ID="rbtnEmpSal" runat="server" CssClass="labelComman" AutoPostBack="true"
                                                        Text="<%$ Resources:Attendance,Employee %>" GroupName="EmpGroupSal" Font-Bold="true"
                                                        OnCheckedChanged="EmpGroupSal_CheckedChanged" />
                                                    <asp:Label ID="lblEmp" runat="server"></asp:Label>
                                                    &nbsp;&nbsp;&nbsp;
                                                         <asp:Label ID="Label1" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Location %>"></asp:Label>
                                                    &nbsp;
                                                    <asp:DropDownList ID="ddlLocation" runat="server" CssClass="textComman" AutoPostBack="true" OnSelectedIndexChanged="dpLocation_SelectedIndexChanged" Width="180px">
                                                    </asp:DropDownList>

                                                    &nbsp;&nbsp;&nbsp;
                                             
                                              
                                                                        <asp:Label ID="lblGroupByDept" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Department %>"></asp:Label>


                                                    <asp:DropDownList ID="dpDepartment" runat="server" CssClass="DropdownSearch" Height="25px" AutoPostBack="true" OnSelectedIndexChanged="dpDepartment_SelectedIndexChanged"
                                                        Width="200px">
                                                    </asp:DropDownList>

                                                    <asp:Panel ID="pnlserachdepartment" runat="server" DefaultButton="ImageButton1" Style="margin-left: 702px; margin-top: -34px;">
                                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False"
                                                            ImageUrl="~/Images/refresh.png" Width="25px" OnClick="btnAllRefresh_Click"
                                                            ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                                                                <asp:Button ID="btnLogProc" runat="server" CssClass="buttonCommman"
                                                                                    OnClick="btnLogProc_Click" Text="<%$ Resources:Attendance,Log Process %>"
                                                                                    Visible="false" />
                                                    </asp:Panel>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Panel ID="pnlGroupSal" runat="server">
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td valign="top" align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' width="170px">
                                                                                <asp:ListBox ID="lbxGroupSal" runat="server" Height="211px" Width="171px" SelectionMode="Multiple"
                                                                                    AutoPostBack="true" OnSelectedIndexChanged="lbxGroupSal_SelectedIndexChanged"
                                                                                    CssClass="list" Font-Names="Trebuchet MS" Font-Size="Small" ForeColor="Gray"></asp:ListBox>
                                                                            </td>
                                                                            <td valign="top" align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmployeeSal" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                                    OnPageIndexChanging="gvEmployeeSal_PageIndexChanging"  Width="100%"
                                                                                    PageSize="<%# PageControlCommon.GetPageSize() %>">
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                                                                <asp:Label ID="lblEmpId" Visible="false" runat="server" Text='<%# Eval("Emp_Id") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle  />
                                                                                        </asp:TemplateField>
                                                                                        <asp:BoundField DataField="Emp_Name" HeaderText="<%$ Resources:Attendance,Employee Name %>"
                                                                                            SortExpression="Emp_Name" ItemStyle-Width="40%" />
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Email Id %>">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("Email_Id") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle  />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Designation Name %>">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblItemType" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle  />
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                    
                                                                                    
                                                                                    
                                                                                    
                                                                                </asp:GridView>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <asp:Label ID="Label52" runat="server" Visible="false"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>


                                                    <asp:Panel ID="pnlEmp" runat="server" DefaultButton="ImageButton8">


                                                        <div width="100%" style="background-image: url(../Images/bg_repeat.jpg); background-repeat: repeat;">
                                                            <table width="100%" style="padding-left: 20px; height: 38px;">
                                                                <tr>
                                                                    <td width="90px">
                                                                        <asp:Label ID="Label24" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Select Option %>"></asp:Label>
                                                                    </td>
                                                                    <td width="170px">
                                                                        <asp:DropDownList ID="ddlField" runat="server" CssClass="DropdownSearch" Height="25px"
                                                                            Width="165px">
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,Employee Code %>" Value="Emp_Code"></asp:ListItem>
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name" Selected="True"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td width="125px">
                                                                        <asp:DropDownList ID="ddlOption" runat="server" CssClass="DropdownSearch" Height="25px"
                                                                            Width="120px">
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                                            <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                                            <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td width="24%">
                                                                        <asp:Panel ID="Panel2" runat="server" DefaultButton="ImageButton8">
                                                                            <asp:TextBox ID="txtValue" runat="server" CssClass="textCommanSearch" Height="14px"
                                                                            Width="100%" />
                                                        </asp:Panel>
                                                                        
                                                                    </td>
                                                                    <td width="50px" align="center">
                                                                        <asp:ImageButton ID="ImageButton8" runat="server" CausesValidation="False" Height="25px"
                                                                            ImageUrl="~/Images/search.png" Width="25px" OnClick="btnarybind_Click1" ToolTip="<%$ Resources:Attendance,Search %>" />
                                                                    </td>
                                                                    <td width="30px" align="center">
                                                                        <asp:Panel ID="Panel15" runat="server" DefaultButton="ImageButton9">
                                                                            <asp:ImageButton ID="ImageButton9" runat="server" CausesValidation="False" Height="25px"
                                                                                ImageUrl="~/Images/refresh.png" Width="25px" OnClick="btnaryRefresh_Click1"
                                                                                ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                                                        </asp:Panel>
                                                                    </td>
                                                                    <td width="30px" align="center"></td>
                                                                    <td width="30px" align="center"></td>
                                                                    <td>
                                                                        <asp:ImageButton ID="ImageButton10" runat="server" OnClick="ImgbtnSelectAll_Clickary"
                                                                            ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true" ImageUrl="~/Images/selectAll.png" />
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:Label ID="lblTotalRecord" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"
                                                                            CssClass="labelComman"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <asp:Label ID="lblSelectRecord" runat="server" Visible="false"></asp:Label>
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmployee" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                            OnPageIndexChanging="gvEmp_PageIndexChanging"  Width="100%"
                                                            DataKeyNames="Emp_Id" PageSize="<%# PageControlCommon.GetPageSize() %>" Visible="true">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                                    </ItemTemplate>
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChanged"
                                                                            AutoPostBack="true" />
                                                                    </HeaderTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <%--<asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Emp_Id") %>'
                                                                            ImageUrl="~/Images/edit.png" Visible="true" OnCommand="btnEditary_Command"
                                                                            Width="16px" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>--%>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                                        <asp:Label ID="lblEmpId" Visible="false" runat="server" Text='<%# Eval("Emp_Id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle  />
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="Emp_Name" HeaderText="<%$ Resources:Attendance,Employee Name %>"
                                                                    SortExpression="Emp_Name" ItemStyle-Width="40%" />
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Email Id %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("Email_Id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle  />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Phone No. %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblItemType" runat="server" Text='<%# Eval("Phone_No") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle  />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            
                                                            
                                                            
                                                            
                                                        </asp:GridView>

                                                    </asp:Panel>

                                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnLogProcess">


                                                        <table runat="server" width="100%">


                                                            <tr>
                                                                <td style="padding-left: 10px;" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                                    <asp:Label ID="Label27" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                                                </td>
                                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">:
                                                                </td>
                                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                                    <asp:TextBox ID="txtFromDate" runat="server" Width="130px" CssClass="labelComman"></asp:TextBox>

                                                                    <cc1:CalendarExtender
                                                                        ID="txtFrom_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtFromDate">
                                                                    </cc1:CalendarExtender>
                                                                </td>


                                                                <td style="padding-left: 10px;" align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                                    <asp:Label ID="Label3" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,To Date %>"></asp:Label>
                                                                </td>
                                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">:
                                                                </td>
                                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                                    <asp:TextBox ID="txtToDate" runat="server" Width="130px" CssClass="labelComman"></asp:TextBox>

                                                                    <cc1:CalendarExtender
                                                                        ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtToDate">
                                                                    </cc1:CalendarExtender>
                                                                </td>
                                                            </tr>



                                                            <tr>
                                                                <td colspan="6" align="center">
                                                                   
                                                                    &nbsp; &nbsp;
                                                                     <asp:Button ID="btnReset" runat="server" OnClick="btnReset_Click" Text="<%$ Resources:Attendance,Reset %>"
                                                                         CssClass="buttonCommman" Visible="true" />

                                                                    &nbsp; &nbsp;  
                                                                     <asp:Button ID="btnLogProcess" runat="server" OnClick="btnGenerate_Click" Text="<%$ Resources:Attendance,Next %>"
                                                                        CssClass="buttonCommman" Visible="true" />
                                                           
                                                                </td>

                                                            </tr>
                                                        </table>
                                                    </asp:Panel>


                                                </td>
                                            </tr>


                                        </table>


                                    </asp:Panel>

                                    <asp:Panel ID="pnlReport" runat="server">


                                        <table width="100%">
                                            <tr>
                                                <td style="width: 25%">
                                                    <asp:LinkButton ID="lnkback" runat="server" CssClass="acc" OnClick="lnkback_Click" Text="<%$ Resources:Attendance,Back %>" Visible="false"></asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="btnExportPdf" runat="server" CommandArgument="1" CommandName="OP"
                                                        Height="23px" ImageUrl="~/Images/pdfIcon.jpg" OnCommand="btnExportPdf_Command" /><asp:ImageButton
                                                            ID="btnExportToExcel" runat="server" CommandArgument="2" CommandName="OP" Height="21px"
                                                            ImageUrl="~/Images/excel-icon.gif" OnCommand="btnExportPdf_Command" />

                                                </td>

                                            </tr>

                                            <tr>

                                                <td colspan="2">
                                                    <exp1:ExportPanel ID="ExportPanel1" runat="server" ScrollBars="Both" Width="1000px" Height="390px"
                                                        ExportType="HTML" OpenInBrowser="True">
                                                        <asp:Label runat="server" ID="lblTitle" Font-Bold="true" CssClass="labelComman" Style="margin-left: 260px;"></asp:Label>
                                                        <table>
                                                            <tr style="width: 100%">
                                                                <td style="width: 40%">
                                                                    <asp:Label runat="server" ID="lblBrandName" Font-Bold="true" CssClass="labelComman" Text="<%$ Resources:Attendance,Brand%>"></asp:Label>
                                                                    : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                             <asp:Label runat="server" ID="lblBrand" Font-Bold="true" CssClass="labelComman"></asp:Label>
                                                                </td>
                                                                <td style="width: 40%">
                                                                    <asp:Label runat="server" ID="lblLocName" Font-Bold="true" CssClass="labelComman" Text="<%$ Resources:Attendance,Location%>"></asp:Label>
                                                                    :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                              <asp:Label runat="server" ID="lblLocation" Font-Bold="true" CssClass="labelComman"></asp:Label>
                                                                </td>
                                                                <td style="width: 33%">
                                                                    <asp:Label runat="server" ID="lblDeptName" Font-Bold="true" CssClass="labelComman" Text="<%$ Resources:Attendance,Department%>"></asp:Label>
                                                                    : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                               <asp:Label runat="server" ID="lblDept" Font-Bold="true" CssClass="labelComman"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <asp:Table ID="Table1" Font-Names="Verdana" BackColor="White" Font-Size="12px" runat="server" Font BorderStyle="Solid" CellPadding="1" CellSpacing="1"
                                                            GridLines="Both" Height="22px" Width="100%">
                                                        </asp:Table>
                                                    </exp1:ExportPanel>


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








        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div id="Background">
            </div>
            <div id="Progress">
                <center>
                    <img src="../Images/ajax-loader2.gif" style="vertical-align: middle" />
                </center>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


        </form>
</body>
</html>