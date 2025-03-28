<%@ Page Language="C#" MasterPageFile="~/ERPMaster_Old.master" AutoEventWireup="true"
    CodeFile="PurchaseRequestApproval.aspx.cs" Inherits="Purchase_PurchaseRequestApproval"
    Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="upallpage" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="gvPurchaseRequest" />
        </Triggers>
        <ContentTemplate>
            <table width="100%" cellpadding="0" cellspacing="0" bordercolor="#F0F0F0">
                <tr bgcolor="#90BDE9">
                    <td align="left">
                        <table>
                            <tr>
                                <td>
                                    <img src="../Images/product_icon.png" width="31" height="30" alt="D" />
                                </td>
                                <td>
                                    <img src="../Images/seperater.png" width="2" height="43" alt="SS" />
                                </td>
                                <td style="padding-left: 5px">
                                    <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Department Approval %>"
                                        CssClass="LableHeaderTitle"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#ccddee" colspan="4" width="100%" height="500px" valign="top">
                        <asp:Panel ID="PnlList" runat="server">
                            <asp:Panel ID="pnlSearchRecords" runat="server" DefaultButton="btnbind">
                                <div width="100%" style="background-image: url(../Images/bg_repeat.jpg); background-repeat: repeat;">
                                    <table width="100%" style="padding-left: 20px; height: 38px">
                                        <tr>
                                            <td width="90px">
                                                <asp:Label ID="lblSelectField" runat="server" Text="<%$ Resources:Attendance,Select Field %>"
                                                    CssClass="labelComman"></asp:Label>
                                            </td>
                                            <td width="180px">
                                                <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="DropdownSearch" Height="25px"
                                                    Width="170px">
                                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Request No %>" Value="RequestNo"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Request Date %>" Value="RequestDate"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td width="135px">
                                                <asp:DropDownList ID="ddlOption" runat="server" CssClass="DropdownSearch" Height="25px"
                                                    Width="120px">
                                                    <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td width="24%">
                                                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                    <asp:TextBox ID="txtValue" runat="server" CssClass="textCommanSearch" Height="14px"
                                                    Width="100%"></asp:TextBox>
                                                        </asp:Panel>                                                
                                            </td>
                                            <td width="50px" align="center">
                                                <asp:ImageButton ID="btnbind" runat="server" CausesValidation="False" Height="25px"
                                                    ImageUrl="~/Images/search.png" OnClick="btnbind_Click" Width="25px" ToolTip="<%$ Resources:Attendance,Search %>">
                                                </asp:ImageButton>
                                            </td>
                                            <td>
                                                <asp:Panel ID="PnlRefresh" runat="server" DefaultButton="btnRefresh">
                                                    <asp:ImageButton ID="btnRefresh" runat="server" CausesValidation="False" Height="25px"
                                                        ImageUrl="~/Images/refresh.png" OnClick="btnRefresh_Click" Width="25px" ToolTip="<%$ Resources:Attendance,Refresh %>">
                                                    </asp:ImageButton>
                                                </asp:Panel>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="lblTotalRecords" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"
                                                    CssClass="labelComman"></asp:Label>
                                                <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <asp:HiddenField ID="HDFSort" runat="server" />
                            </asp:Panel>
                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvPurchaseRequest" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                 OnPageIndexChanging="gvPurchaseRequest_PageIndexChanging" OnSorting="gvPurchaseRequest_Sorting">
                                <Columns>
                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Detail %>">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="IbtnDetail" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                Visible="false" ImageUrl="~/Images/Detail.png" Width="20px" Height="20px" OnCommand="IbtnDetail_Command"
                                                ToolTip="<%$ Resources:Attendance,Detail %>" />
                                            <asp:ImageButton ID="IbtnBack" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                ImageUrl="~/Images/PullBlue.png" Visible="false" OnCommand="IbtnBack_Command"
                                                ToolTip="<%$ Resources:Attendance,Back %>" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemStyle  />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Request No %>" SortExpression="RequestNo">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRequestNo" runat="server" Text='<%# Eval("RequestNo") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle  />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Request Date %>" SortExpression="RequestDate">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRequestDate" runat="server" Text='<%# GetDate(Eval("RequestDate").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle  />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Expected Delivery Date %>"
                                        SortExpression="ExpDelDate">
                                        <ItemTemplate>
                                            <asp:Label ID="lblExpDelDate" runat="server" Text='<%# GetDate(Eval("ExpDelDate").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle  />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Approve %>">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="IbtnApprove" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                Visible="false" ImageUrl="~/Images/approve.png" CausesValidation="False" OnCommand="btnApprove_Command"
                                                ToolTip="<%$ Resources:Attendance,Approve %>" />
                                            <cc1:ConfirmButtonExtender ID="IbtnApprove_ConfirmButtonExtender" runat="server"
                                                ConfirmText="Are you sure you want Approve" TargetControlID="IbtnApprove">
                                            </cc1:ConfirmButtonExtender>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemStyle  />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Reject %>">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="IbtnReject" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                ImageUrl="~/Images/disapprove.png" OnCommand="IbtnReject_Command" Visible="false"
                                                ToolTip="<%$ Resources:Attendance,Reject %>" />
                                            <cc1:ConfirmButtonExtender ID="IbtnReject_ConfirmButtonExtender" runat="server" ConfirmText="Are you sure you want Reject"
                                                TargetControlID="IbtnReject">
                                            </cc1:ConfirmButtonExtender>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemStyle  />
                                    </asp:TemplateField>
                                </Columns>
                                
                                
                                
                                
                            </asp:GridView>
                            <asp:HiddenField ID="EditId" runat="server" />
                            <table width="100%" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td style="padding-left: 80px;">
                                        <asp:Panel ID="pnlDetail" runat="server">
                                            <table width="90%" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td>
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvProductRequest" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                            runat="server" AutoGenerateColumns="False" Width="100%" >
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Serial No %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSerialNO" Width="50px" runat="server" Text='<%# Eval("Serial_No") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle  Width="80px" HorizontalAlign="Center"  />
                                                                </asp:TemplateField>
                                                                 <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblproductcode" runat="server" Text='<%# ProductCode(Eval("Product_Id").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle  />
                                                                    </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>">
                                                                    <ItemTemplate>
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblProductId" runat="server" Text='<%# ProductName(Eval("Product_Id").ToString(),Eval("Trans_Id").ToString()) %>'></asp:Label>
                                                                                </td>
                                                                                <td align='right'>
                                                                                    <asp:ImageButton ID="lnkDes" runat="server" ImageUrl="~/Images/detail.png" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:Panel ID="PopupMenu1" Width="100%" runat="server">
                                                                            <table border="1" cellpadding="0" cellspacing="0" bordercolor="#c6c6c6">
                                                                                <tr>
                                                                                    <td>
                                                                                        <table width="314" height="110" cellspacing="0" bgcolor="#F9F9F9">
                                                                                            <tr>
                                                                                                <td height="21" colspan="2">
                                                                                                    <div align="center" style="background: url(../Images/InvGridHdr.jpg) repeat">
                                                                                                        <asp:Label ID="lblDetail1" runat="server" Text="<%$ Resources:Attendance,Details %>"></asp:Label>
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="2" align="left" valign="top">
                                                                                                    <asp:Panel ID="pnl" runat="server" Width="100%" Height="300px" ScrollBars="Vertical">
                                                                                                        <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("ProductDescription") %>'></asp:Label>
                                                                                                    </asp:Panel>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                        <cc1:HoverMenuExtender ID="hme3" runat="Server" TargetControlID="lnkDes" PopupControlID="PopupMenu1"
                                                                            HoverCssClass="popupHover" PopupPosition="Left" OffsetX="0" OffsetY="0" PopDelay="50" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle  Width="200px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Name %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblUnit" Width="100px" runat="server" Text='<%# UnitName(Eval("UnitId").ToString()) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle  Width="100px" HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quantity %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblReqQty" runat="server" Text='<%# Eval("ReqQty") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle  />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            
                                                            
                                                            
                                                            
                                                        </asp:GridView>
                                                        <asp:Label ID="lblDescription" runat="server" Visible="false" CssClass="labelComman"
                                                            Font-Size="14px">
                                                        </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtDescription" runat="server" ReadOnly="true" TextMode="MultiLine"
                                                            Visible="false" Height="35px" Width="99%" CssClass="textComman"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
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
