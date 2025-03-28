<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewFeedBack.aspx.cs" Inherits="ServiceManagement_ViewFeedBack" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <link href="../CSS/InvStyle.css" rel="stylesheet" type="text/css" />
    <form id="form1" runat="server">
    <asp:UpdatePanel ID="upallpage" runat="server">
        <triggers>
         
           <asp:PostBackTrigger ControlID="GvFeedback" />
        </triggers>
        <contenttemplate>
            <table width="100%">
                <tr>
                   <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
          
                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                        <table width="100%" cellpadding="0" cellspacing="0" bordercolor="#F0F0F0">
                            <tr bgcolor="#90BDE9">
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <img src="../Images/view_ticket_feedback.png" alt="D"  />
                                            </td>
                                            <td>
                                                <img src="../Images/seperater.png" width="2" height="43" alt="SS" />
                                            </td>
                                            <td style="padding-left: 5px">
                                                <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Ticket Feedback%>"
                                                    CssClass="LableHeaderTitle"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                  
                                </td>
                                <td align="right">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                             
                                            </td>
                                            <td>
                                                
                                            </td>
                                            <td>
                                              
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor="#ccddee" colspan="4" width="100%"  valign="top">
                                    <asp:Panel ID="PnlList" runat="server" >
                                      <asp:Panel ID="pnlTicket" runat="server">
                                  <fieldset style="width:1304px;">
                                                        <legend>
                                                            <asp:Label ID="lblDeviceParameter" Font-Names="Times New roman" Font-Size="18px"
                                                                Font-Bold="true" runat="server" Text="Ticket Detail" CssClass="labelComman"></asp:Label>
                                                        </legend>
                                 <table width="100%">
                                  <tr>
                                   <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' width="145px">
                                                    <asp:Label ID="lblPINo" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Ticket No.%>" Font-Bold="true"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' >
                                                    <asp:Label ID="lblticketno" runat="server" CssClass="labelComman" TabIndex="14" />
                                                </td>
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' width="130px">
                                                    <asp:Label ID="lblTiDate" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Ticket Date %>" Font-Bold="true"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' >
                                                <asp:Label ID="lblTickeDate" runat="server" CssClass="labelComman"></asp:Label>
                                                </td>
                                                   <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                    <asp:Label ID="lblCallType" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Task Type %>" Font-Bold="true"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td>
                                                <asp:Label ID="lblTaskType" runat="server" CssClass="labelComman">
                                                </asp:Label>
                                                </td>
                                                 
                                            </tr>
                                             <tr>
                                             
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'  >
                                                    <asp:Label ID="lblCustomerName" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Customer Name %>" Font-Bold="true"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td>
                                                <asp:Label ID="lblCustomerNameValue" runat="server" CssClass="labelComman"></asp:Label>
                                                </td>
                                                 <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                    <asp:Label ID="Label8" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Schedule Date %>" Font-Bold="true"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                     <asp:Label ID="lblScheduledate" runat="server" CssClass="labelComman"></asp:Label>
                                                </td>
                                                 <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                    <asp:Label ID="Label2" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Status%>" Font-Bold="true"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td>
                                                 <asp:Label ID="lblStatus" runat="server" CssClass="labelComman"></asp:Label>
                                                </td>
                                                </tr>
                                                 <tr>
                                               
                                                </tr>
                                                <tr>
                                                <td>
                                                 <asp:Label ID="lblDesription" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Description %>" Font-Bold="true"  />
                                                </td>
                                                <td align="center">
                                                :
                                                </td>
                                                <td colspan="7">
                                                <asp:Label ID="lblDescriptionvalue" runat="server" CssClass="labelComman"></asp:Label>
                                                <asp:Label ID="lblCustomerEmailId" runat="server" Visible="false" ></asp:Label>
                                                     <asp:Label ID="lblCustomerContactNo" runat="server" Visible="false" ></asp:Label>
                                                </td>
                                                </tr>


                                 </table>
                                 </fieldset>
                                 </asp:Panel> 
                                 <asp:Panel ID="pnlcomments" runat="server">
                                 <table width="100%" style="padding-left:19px;">
                                 <tr>
                                   <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' width="145px" >
                                                    <asp:Label ID="Label3" runat="server" CssClass="labelComman" Text="Comments" />
                                                </td>
                                                <td width="1px">
                                                    :
                                                </td>
                                                <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' Width="740px" >
                                                    <asp:TextBox ID="txtAction" runat="server" Width="713px" TextMode="MultiLine" Height="50px"
                                                        TabIndex="42" CssClass="textComman" Font-Names="Arial" /><a style="color: Red;">*</a>
                                                </td>
                                                  <td   >
                                                 <asp:Button ID="btnInquirySave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                                    CssClass="buttonCommman" OnClick="btnInquirySave_Click"  />
                                                </td>
                                 </tr>
                                 </table>
                                 </asp:Panel>
                                        <asp:Panel ID="pnlSearchRecords" runat="server" DefaultButton="btnbind" >
                                            <div width="100%" style="background-image: url(../Images/bg_repeat.jpg); background-repeat: repeat;">
                                                <table width="100%" style="padding-left: 20px; height: 38px">
                                                    <tr>
                                                        <td width="90px">
                                                            <asp:Label ID="lblSelectField" runat="server" Text="<%$ Resources:Attendance,Select Field %>"
                                                                CssClass="labelComman"></asp:Label>
                                                        </td>
                                                        <td width="180px">
                                                            <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="DropdownSearch" Height="25px"
                                                                Width="200px" OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged" AutoPostBack="true">
                                                                  <asp:ListItem Text="<%$ Resources:Attendance,Employee Name%>" Value="Emp_Name"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Feedback Date%>" Value="Date"></asp:ListItem>
                                                               
                                                                 
                                                              
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td width="135px">
                                                            <asp:DropDownList ID="ddlOption" runat="server" CssClass="DropdownSearch" Height="25px"
                                                                Width="120px">
                                                                <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td width="270px">
                                                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                                <asp:TextBox ID="txtValue" runat="server" CssClass="textCommanSearch" Height="14px"
                                                               Width="100%"></asp:TextBox>
                                                                    <asp:TextBox ID="txtValueDate" runat="server" CssClass="textCommanSearch" Height="14px" TabIndex="4"
                                                                Width="100%" Visible="false"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="CalendartxtValueDate" runat="server" TargetControlID="txtValueDate" />
                                                        </asp:Panel>                                                            
                                                        </td>
                                                        <td width="50px" align="center">
                                                            <asp:ImageButton ID="btnbind" runat="server" CausesValidation="False" Height="25px"
                                                                ImageUrl="~/Images/search.png" OnClick="btnbindrpt_Click" Width="25px" ToolTip="<%$ Resources:Attendance,Search %>">
                                                            </asp:ImageButton>
                                                        </td>
                                                        <td>
                                                            <asp:Panel ID="PnlRefresh" runat="server" DefaultButton="btnRefresh">
                                                                <asp:ImageButton ID="btnRefresh" runat="server" CausesValidation="False" Height="25px"
                                                                    ImageUrl="~/Images/refresh.png" OnClick="btnRefreshReport_Click" Width="25px"
                                                                    ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                                            </asp:Panel>
                                                        </td>
                                                        <td align="center">
                                                            <asp:Label ID="lblTotalRecords" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"
                                                                CssClass="labelComman"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvFeedback" 
                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="false" OnPageIndexChanging="GvFeedback_PageIndexChanging"
                                                AllowSorting="True" OnSorting="GvFeedback_Sorting" >
                                                
                                                <Columns>
                                                   
                                                     <asp:TemplateField HeaderText="<%$ Resources:Attendance,Download %>" ItemStyle-HorizontalAlign="Center"
                                                        >
                                                        <ItemTemplate>
                                                                   <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%#Eval("Trans_Id") %>' CommandName='<%#Eval("Field2") %>' Text= '<%#Eval("Field2") %>' OnCommand="OnDownloadCommand" ForeColor="Blue" ToolTip= "<%$ Resources:Attendance,Download %>"></asp:LinkButton>
                                                    
                                                           
                                                        </ItemTemplate>
                                                        <ItemStyle  Width="200px" />
                                                    </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code%>" SortExpression="Emp_Code" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvEmpCode" runat="server" Text='<%#Eval("Emp_Code")%>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center"  Width="150px"  />
                                                    </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name%>" SortExpression="Emp_Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvEmpName" runat="server" Text='<%#Eval("Emp_Name")%>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center"  Width="150px"  />
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="<%$ Resources:Attendance,Feedback Date%>" SortExpression="Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvdate" runat="server" Text='<%#GetDate(Eval("Date").ToString())%>' />
                                                               <asp:Label ID="lblgvcalltime" runat="server" Text='<%#Convert.ToDateTime(Eval("CreatedDate").ToString()).ToString("hh:mm") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center"  Width="140px"  />
                                                    </asp:TemplateField>
                                                       <asp:TemplateField HeaderText="<%$ Resources:Attendance,Action%>" SortExpression="Action">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvAction" runat="server" Text='<%#Eval("Action")%>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center"  />
                                                    </asp:TemplateField>
                                                       <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status%>" SortExpression="Action" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvfeedBackStatus" runat="server" Text='<%#Eval("Field1")%>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center"  Width="70px"  />
                                                    </asp:TemplateField>
                                                    
                                                </Columns>
                                                
                                                
                                                
                                            </asp:GridView>
                                            <asp:HiddenField ID="hdnValue" runat="server" />
                                        </asp:Panel>
                                    </asp:Panel>
                                  
                                </td>
                            </tr>
                            
                        </table>
                    </td>
                </tr>
            </table>
        </contenttemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <progresstemplate>
            <div id="Background">
            </div>
            <div id="Progress">
                <center>
                    <img src="../Images/ajax-loader2.gif" style="vertical-align: middle" alt="Pegasus Technologies" />
                </center>
            </div>
        </progresstemplate>
    </asp:UpdateProgress>
    </form>
</body>
</html>
