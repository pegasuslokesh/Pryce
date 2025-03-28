<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster_Old.master" AutoEventWireup="true"
    CodeFile="EditableOvertimeReport.aspx.cs" Inherits="Attendance_Report_EditableOvertimeReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table id="Table1" runat="server" width="100%">
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
                                                <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Attendance Register  %>"
                                                    CssClass="LableHeaderTitle"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor="#ccddee" colspan="4" width="100%" height="500px" valign="top">
                                    <asp:Panel ID="pnlEmp" runat="server">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label3" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Employee Name %>"></asp:Label>
                                                </td>
                                                <td width="1px">
                                                    :
                                                </td>
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' colspan="6">
                                                    <asp:TextBox ID="txtEmpName" Width="287px" runat="server" CssClass="textComman" BackColor="#eeeeee"
                                                        AutoPostBack="true" OnTextChanged="txtEmpName_textChanged" />
                                                    <a style="color: Red">*</a>
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtEmpName" UseContextKey="True"
                                                        CompletionListCssClass="completionList" CompletionListItemCssClass="listItem"
                                                        CompletionListHighlightedItemCssClass="itemHighlighted">
                                                    </cc1:AutoCompleteExtender>
                                                    <asp:HiddenField ID="hdnEmpId" runat="server" />
                                                </td>
                                            </tr>
                                            <tr id="Tr1" runat="server">
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>" style="padding-left: 10px;">
                                                    <asp:Label ID="Label27" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,From Date %>"></asp:Label>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    :
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="labelComman" Width="130px"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="txtFrom_CalendarExtender" runat="server" Enabled="True"
                                                        TargetControlID="txtFromDate">
                                                    </cc1:CalendarExtender>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>" style="padding-left: 10px;">
                                                    <asp:Label ID="Label1" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,To Date %>"></asp:Label>
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    :
                                                </td>
                                                <td align="<%= PageControlCommon.ChangeTDForDefaultLeft()%>">
                                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="labelComman" Width="130px"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtToDate">
                                                    </cc1:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" align="center">
                                                    <asp:Button ID="btnReport" runat="server" Width="50px" Text="<%$ Resources:Attendance,Get Report %>"
                                                        CssClass="buttonCommman" OnClick="btnReport_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlReport" runat="server">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvrecord" Style="margin-top: 10px;" runat="server" AutoGenerateColumns="False"
                                                        Width="100%" >
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Date %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDate" runat="server" Text='<%#Eval("Att_Date") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Shift Name %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblShiftName" runat="server" Text='<%#Eval("Shift_Name") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Time Table Name %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTimeTableName" runat="server" Text='<%#Eval("TimeTable_Name") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,In Time %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblInTime" runat="server" Text='<%#Eval("In_Time") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Out Time %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOutTime" runat="server" Text='<%#Eval("Out_Time") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Over Time Hour %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOTHour" runat="server" Text='<%#Eval("OverTime_Min_hhmm") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
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
</asp:Content>
