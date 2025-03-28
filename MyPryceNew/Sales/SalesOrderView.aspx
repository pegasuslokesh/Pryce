<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SalesOrderView.aspx.cs" Inherits="Sales_SalesOrderView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../CSS/InvStyle.css" rel="stylesheet" type="text/css" />

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

      <table width="100%">
                <tr>
                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                        <table width="100%" cellpadding="0" cellspacing="0" bordercolor="#F0F0F0">
                            <tr bgcolor="#90BDE9">
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <img src="../Images/sales_order.png" alt="D" />
                                            </td>
                                            <td>
                                                <img src="../Images/seperater.png" width="2" height="43" alt="SS" />
                                            </td>
                                            <td style="padding-left: 5px">
                                                <asp:Label ID="lblPHeader" runat="server" Text="<%$ Resources:Attendance,Sales Order %>"
                                                    CssClass="LableHeaderTitle" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                   
                                </td>
                                <td>
                              
                                 
                                </td>
                                <td align="right">
                                    <table cellpadding="0" cellspacing="0">
                                      
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor="#ccddee" colspan="5" width="100%" height="500px" valign="top">
                                        <asp:Panel ID="PnlNewContant" runat="server">
                                            <table width="100%" style="padding-left: 43px">
                                                <tr>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:Label ID="lblSODate" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Order Date %>"></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:TextBox ID="txtSODate" runat="server" CssClass="textComman" ReadOnly="true" /><a style="color: Red">*</a>
                                                        
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:Label ID="lblSONo" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Order No. %>"></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:TextBox ID="txtSONo" runat="server" CssClass="textComman" /><a style="color: Red">*</a>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:Label ID="lblOrderType" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Order Type %>" />
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:DropDownList ID="ddlOrderType" runat="server" Width="262px" CssClass="textComman"
                                                            >
                                                            <asp:ListItem Text="<%$ Resources:Attendance,--Select--%>" Value="--Select--"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance, Direct%>" Value="D"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,By Quotation%>" Value="Q"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <a style="color: Red">*</a>
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:Label ID="lblCustOrderNo" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Customer Order No. %>" />
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:TextBox ID="txtCustOrderNo" runat="server" CssClass="textComman" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:Label ID="lblCurrency" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Currency %>" />
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:DropDownList ID="ddlCurrency" runat="server" Width="260px" CssClass="textComman">
                                                        </asp:DropDownList>
                                                       </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:Label ID="lblPaymentMode" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Payment Mode %>" />
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:DropDownList ID="ddlPaymentMode" Width="262px" runat="server" CssClass="textComman" />
                                                        <a style="color: Red">*</a>
                                                    </td>
                                                </tr>
                                                <tr id="trTransfer" runat="server" visible="false">
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:Label ID="lblTransFrom" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Transfer From %>" />
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:TextBox ID="txtTransFrom" runat="server" CssClass="textComman" ReadOnly="True" />
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:Label ID="lblQuotationNo" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Quotation No. %>" />
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:DropDownList ID="ddlQuotationNo" Width="262px" runat="server" CssClass="textComman"
                                                            Visible="false"
                                                           />
                                                        <asp:TextBox ID="txtQuotationNo" runat="server" ReadOnly="true" CssClass="textComman"
                                                            Visible="false" /><a style="color: Red">*</a>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:Label ID="lblCustomerName" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Customer Name %>" />
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' colspan="4">
                                                        <asp:TextBox ID="txtCustomer" runat="server" CssClass="textComman" Width="928px"
                                                            BackColor="#eeeeee"  /><a
                                                                style="color: Red">*</a>
                                                       
                                                     
                                                       
                                                    </td>
                                                </tr>

                                               

                                            <tr>


                                                <tr>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:Label ID="Label14" runat="server" CssClass="labelComman" Text="Agent Name" />
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' colspan="4">
                                                        <asp:TextBox ID="txtAgentName" runat="server" CssClass="textComman" Width="928px" Enabled="false"
                                                            BackColor="#eeeeee"  />
                                                       
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:Label ID="lblInvoiceTo" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Invoice Address %>" />
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:TextBox ID="txtInvoiceTo" runat="server" CssClass="textComman" BackColor="#eeeeee"
                                                            />
                                                       
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:Label ID="lblEstimateDeliveryDate" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Estimate Delivery Date %>" />
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:TextBox ID="txtEstimateDeliveryDate" runat="server" CssClass="textComman" /><a
                                                            style="color: Red">*</a>
                                                      
                                                    </td>
                                                </tr>
                                               
                                                <tr>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:Label ID="Label6" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Ship To %>" />
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:TextBox ID="txtShipCustomerName" runat="server" CssClass="textComman" BackColor="#eeeeee"
                                                            />
                                                      
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:Label ID="lblShipingAddress" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Shipping Address %>" />
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' colspan="4">
                                                        <asp:TextBox ID="txtShipingAddress" runat="server" CssClass="textComman" BackColor="#eeeeee"
                                                            />
                                                      
                                                    </td>
                                                </tr>
                                               
                                              
                                                <tr>
                                                    <td colspan="6" width="100%">
                                                        <asp:Panel ID="pnlDetail" runat="server" Width="100%">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvQuotationDetail" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                 OnRowCreated="GvQuotationDetail_RowCreated">
                                                                
                                                                <Columns>
                                                                  <asp:TemplateField Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkGvQuotationDetailSelect" runat="server" AutoPostBack="true"
                                                                                 />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  Width="30px" />
                                                                    </asp:TemplateField>
                                                                   
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvSerialNo" runat="server" Text='<%#Eval("Serial_No") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  Width="30px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvProductCode" runat="server" Text='<%#ProductCode(Eval("Product_Id").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  Width="60px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>">
                                                                        <ItemTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:HiddenField ID="hdngvProductId" runat="server" Value='<%#Eval("Product_Id") %>' />
                                                                                        <asp:Label ID="lblgvProductName" runat="server" Text='<%#GetProductName(Eval("Product_Id").ToString()) %>' />
                                                                                    </td>
                                                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultRight()%>'>
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
                                                                                                            <asp:Literal ID="lblgvProductDescription" runat="server" Text='<%# GetProductDescription(Eval("Product_Id").ToString()) %>'></asp:Literal>
                                                                                                        </asp:Panel>
                                                                                                        <br />
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
                                                                        <ItemStyle HorizontalAlign="Center"  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit %>">
                                                                        <ItemTemplate>
                                                                            <asp:DropDownList ID="ddlgvUnit" Width="65px" runat="server" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quantity %>">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="lblgvQuantity" Width="30px" ForeColor="#4d4c4c" runat="server"
                                                                                 Text='<%#Eval("Quantity") %>' />
                                                                            
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Free %>">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtgvFreeQuantity" Width="30px" runat="server" />
                                                                         
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Remain %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="txtgvRemainQuantity" Width="30px" runat="server" Text='<%#Convert.ToInt32(Eval("Quantity")).ToString() %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Price %>">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtgvUnitPrice" Width="45px" runat="server" 
                                                                                 />
                                                                          
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtgvQuantityPrice" ReadOnly="true" runat="server" Width="70px" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,% %>" >
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtgvDiscountP" Width="30px" runat="server"
                                                                                />
                                                                           
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Value %>">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtgvDiscountV" Width="45px" runat="server" 
                                                                                />
                                                                          
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,After Price %>">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtgvPriceAfterDiscount" Width="60px" ReadOnly="true" runat="server" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,% %>">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtgvTaxP" Width="30px" runat="server" 
                                                                                />
                                                                            
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Value %>">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtgvTaxV" Width="45px" runat="server"
                                                                               />
                                                                           
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,After Price %>">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtgvPriceAfterTax" Width="60px" ReadOnly="true" runat="server" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtgvTotal" Width="60px" ReadOnly="true" runat="server" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkIsProduction" runat="server" Checked='<%#Eval("Field6") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField >
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkStockInfo" runat="server" Text='<%#GetProductStock(Eval("Product_Id").ToString()) %>'
                                                                                Font-Underline="true" ToolTip="View Detail" 
                                                                                CssClass="labelComman" ForeColor="Blue" CommandArgument='<%# Eval("Product_Id") %>'></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <ItemStyle  HorizontalAlign="Center" Width="5%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField Visible="false" HeaderText="Commission">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtgvAgentCommission" ForeColor="#4d4c4c" Width="60px" runat="server" Enabled="false"
                                                                                Text='<%#Eval("AgentCommission") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle  HorizontalAlign="Center" Width="5%" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                
                                                                
                                                                
                                                            </asp:GridView>
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvProductDetail" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                 OnRowCreated="GvProductDetail_RowCreated" DataKeyNames="Product_Id"
                                                                ShowFooter="false" OnRowDataBound="GvProductDetail_OnRowDataBound">
                                                                
                                                                <Columns>
                                                                    <asp:TemplateField Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="imgBtnaddtax" runat="server" CommandArgument='<%# Eval("Serial_No") %>'
                                                                                ImageUrl="~/Images/plus.png" Width="30px" Height="30px" 
                                                                                ToolTip="Add Tax" />
                                                                            <%-- <asp:ImageButton ID="imgBtnProductEdit" runat="server" CommandArgument='<%# Eval("Serial_No") %>'
                                                                            ImageUrl="~/Images/edit.png" Width="16px" OnCommand="imgBtnProductEdit_Command" />--%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle  HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="imgBtnProductDelete" runat="server" CommandArgument='<%# Eval("Serial_No") %>'
                                                                                Height="16px" ImageUrl="~/Images/Erase.png" Width="16px"  />
                                                                        </ItemTemplate>
                                                                        <ItemStyle  HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                  
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvSerialNo" runat="server" Visible="false" Text='<%#Eval("Serial_No") %>' />
                                                                            <asp:Label ID="lblSerialNo" runat="server" Text='<%#Container.DataItemIndex+1 %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  Width="30px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvProductCode" runat="server" Text='<%#ProductCode(Eval("Product_Id").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  Width="60px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>">
                                                                        <ItemTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:HiddenField ID="hdngvProductId" runat="server" Value='<%#Eval("Product_Id") %>' />
                                                                                        <asp:Label ID="lblgvProductName" runat="server" Text='<%#GetProductName(Eval("Product_Id").ToString()) %>' />
                                                                                    </td>
                                                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultRight()%>'>
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
                                                                                                            <asp:Literal ID="lblgvProductDescription" runat="server" Text='<%# GetProductDescription(Eval("Product_Id").ToString()) %>'></asp:Literal>
                                                                                                        </asp:Panel>
                                                                                                        <br />
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
                                                                        <ItemStyle HorizontalAlign="Center"  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit %>">
                                                                        <ItemTemplate>
                                                                            <asp:HiddenField ID="hdngvUnitId" runat="server" Value='<%#Eval("UnitId") %>' />
                                                                            <asp:Label ID="lblgvUnit" runat="server" Text='<%#GetUnitName(Eval("UnitId").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quantity %>">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="lblgvQuantity" Width="30px" runat="server" Text='<%#Eval("Quantity") %>'
                                                                                 />
                                                                           
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Free %>">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="lblgvFreeQuantity" Width="30px" runat="server" Text='<%#Eval("FreeQty") %>' />
                                                                           
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Remain %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvRemainQuantity" runat="server" Text='<%#Eval("RemainQty") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Price %>">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="lblgvUnitPrice" runat="server" Text='<%#Eval("UnitPrice") %>' Width="50px"
                                                                                 />
                                                                          
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvQuantityPrice" runat="server" Width="70px" Text='<%#Eval("GrossPrice") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,% %>">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="lblgvDiscountP" runat="server" Width="45px" Text='<%#Eval("DiscountP") %>'
                                                                                />
                                                                           
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Value %>">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="lblgvDiscountV" runat="server" Width="45px" Text='<%#Eval("DiscountV") %>'
                                                                                />
                                                                           
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,After Price %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvPriceAfterDiscount" runat="server" Visible="false" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,% %>">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="lblgvTaxP" runat="server" Width="40px" Text='<%#Eval("TaxP") %>'
                                                                                Enabled="false" />
                                                                         
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Value %>">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="lblgvTaxV" runat="server" Width="45px" Text='<%#Eval("TaxV") %>'
                                                                                Enabled="false" />
                                                                         
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,After Price %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvPriceAfterTax" runat="server" Visible="false" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvTotal" Width="60px" runat="server" Text='<%#Eval("NetTotal") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkIsProduction" runat="server" Checked='<%#Eval("Field6") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"  />
                                                                    </asp:TemplateField>
                                                                      <asp:TemplateField Visible="false" HeaderText="Commission">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtgvAgentCommission" ForeColor="#4d4c4c" Width="60px" runat="server" Enabled="false"
                                                                                Text='<%#Eval("AgentCommission") %>' />
                                                                           
                                                                        </ItemTemplate>
                                                                        <ItemStyle  HorizontalAlign="Center" Width="5%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField >
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkStockInfo" runat="server" Text='<%#GetProductStock(Eval("Product_Id").ToString()) %>'
                                                                                Font-Underline="true" ToolTip="View Detail" 
                                                                                CssClass="labelComman" ForeColor="Blue" CommandArgument='<%# Eval("Product_Id") %>'></asp:LinkButton>
                                                                            <asp:Literal runat="server" ID="lit1" Text="<tr id='trGrid'><td colspan='20' align='right'>" />
                                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvchildGrid" runat="server" AutoGenerateColumns="false" DataKeyNames="Tax_Id"
                                                                                 Visible="false">
                                                                                <Columns>
                                                                                    <asp:TemplateField>
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="chkselecttax" runat="server" Width="10px" 
                                                                                                Checked='<%#Eval("TaxSelected") %>' />
                                                                                            <asp:HiddenField ID="hdntaxId" runat="server" Value='<%#Eval("Tax_Id") %>' />
                                                                                            <asp:HiddenField ID="hdnCategoryId" runat="server" Value='<%#Eval("ProductCategoryId") %>' />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Left" Width="20px" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Category Name">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblgvcategoryName" runat="server" Width="200px" Visible="true" Text='<%#Eval("CategoryName") %>' />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Left" Width="200px" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Tax Name">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblgvtaxName" runat="server" Width="200px" Text='<%#Eval("TaxName") %>' />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Left" Width="200px" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Tax(%)">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txttaxPerchild" runat="server" CssClass="textComman" 
                                                                                                Width="100px" Enabled='<%#Eval("TaxSelected") %>'
                                                                                                Text='<%#Eval("Tax_Per") %>'></asp:TextBox>
                                                                                           
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Tax Value">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txttaxValuechild" runat="server" CssClass="textComman"
                                                                                                Width="100px" Enabled='<%#Eval("TaxSelected") %>' 
                                                                                                Text='<%#Eval("Tax_value") %>'></asp:TextBox>
                                                                                            
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                                
                                                                                
                                                                                
                                                                            </asp:GridView>
                                                                            <asp:Literal runat="server" ID="lit2" Text="</td></tr>" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle  HorizontalAlign="Center" Width="5%" />
                                                                    </asp:TemplateField>
                                                                  
                                                                </Columns>
                                                                
                                                                
                                                                
                                                            </asp:GridView>
                                                            <asp:HiddenField ID="hdnProductId" runat="server" />
                                                            <asp:HiddenField ID="hdnProductName" runat="server" />
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:Label ID="lblAmount" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Gross Total %>" />
                                                    </td>
                                                    <td>
                                                        :
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:TextBox ID="txtAmount" runat="server" ReadOnly="true" CssClass="textComman"
                                                           />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:Label ID="lblDiscountP" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Discount(%) %>" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblDiscountPvolon" runat="server" Text=":"></asp:Label>
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:TextBox ID="txtDiscountP" runat="server" Width="80px" CssClass="textComman"
                                                            />
                                                        <asp:Label ID="Label3" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Value %>" />
                                                        <asp:TextBox ID="txtDiscountV" runat="server" Width="81px" CssClass="textComman"
                                                             />
                                                        
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:Label ID="lblafterDiscountPrice" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Price After Discount %>"
                                                            Visible="false" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblPriceafterdiscountcolon" runat="server" Text=":" Visible="false"></asp:Label>
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:TextBox ID="txtPriceAfterDiscount" runat="server" CssClass="textComman" ReadOnly="True"
                                                            Visible="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td colspan="5">
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gridView" ShowHeader="true" runat="server" AutoGenerateColumns="false"
                                                            Width="94%" DataKeyNames="Tax_Id" ShowFooter="true"  OnRowCancelingEdit="gridView_RowCancelingEdit"
                                                            OnRowDeleting="gridView_RowDeleting" OnRowEditing="gridView_RowEditing" OnRowUpdating="gridView_RowUpdating"
                                                            OnRowCommand="gridView_RowCommand">
                                                            
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Tax Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTaxname" runat="server" CssClass="labelComman" Text='<%#Eval("TaxName") %>'></asp:Label>
                                                                        <asp:HiddenField ID="hdnTaxId" runat="server" Value='<%#Eval("Tax_Id") %>' />
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtTaxName" runat="server" Font-Names="Verdana" 
                                                                            CssClass="textComman" BackColor="#eeeeee"
                                                                            Text='<%#Eval("TaxName") %>' CausesValidation="false"></asp:TextBox>
                                                                        <a style="color: Red">*</a>
                                                                      
                                                                    </EditItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtTaxFooter" runat="server" Font-Names="Verdana"
                                                                            CssClass="textComman" BackColor="#eeeeee"
                                                                            CausesValidation="true" Width="400px"></asp:TextBox>
                                                                      
                                                                    </FooterTemplate>
                                                                    <ItemStyle  Width="10%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Tax(%)">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTaxper" runat="server" CssClass="labelComman" Text='<%#Eval("Tax_Per") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtTaxper" runat="server" Font-Names="Verdana" CssClass="textComman"
                                                                            Text='<%#Eval("Tax_Per") %>' CausesValidation="true" AutoPostBack="false"></asp:TextBox>
                                                                        
                                                                    </EditItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtTaxperFooter" runat="server" Font-Names="Verdana" CssClass="textComman"
                                                                            Text='<%#Eval("Tax_per") %>' CausesValidation="true"
                                                                            Width="100px"></asp:TextBox>
                                                                      
                                                                    </FooterTemplate>
                                                                    <ItemStyle  Width="8%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Tax Value">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTaxValue" runat="server" CssClass="labelComman" Text='<%#Eval("Tax_Value") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtTaxValue" runat="server" Font-Names="Verdana" CssClass="textComman"
                                                                            Text='<%#Eval("Tax_Value") %>'  AutoPostBack="false"></asp:TextBox>
                                                                        
                                                                    </EditItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtTaxValueFooter" runat="server" Font-Names="Verdana" CssClass="textComman"
                                                                            Text='<%#Eval("Tax_Value") %>' CausesValidation="true" 
                                                                            Width="100px"></asp:TextBox>
                                                                       
                                                                    </FooterTemplate>
                                                                    <ItemStyle  Width="8%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <EditItemTemplate>
                                                                        <asp:Button ID="ButtonUpdate" runat="server" CommandName="Update" Text="Update" CausesValidation="true"
                                                                            CommandArgument='<%#Eval("Tax_Id") %>' />
                                                                        <asp:Button ID="ButtonCancel" runat="server" CommandName="Cancel" Text="Cancel" />
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Button ID="ButtonEdit" runat="server" CommandName="Edit" Text="Edit" Visible="false" />
                                                                        <asp:Button ID="ButtonDelete" runat="server" CommandName="Delete" Text="Delete" CommandArgument='<%#Eval("Tax_Id") %>' />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Button ID="ButtonAdd" runat="server" CommandName="AddNew" Text="Add New Row" />
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            
                                                            
                                                            
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="6">
                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                            <asp:Label ID="lblPriceAfterTax" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Price After Tax %>"
                                                                Visible="false" />
                                                        </td>
                                                        <td align="center">
                                                            <asp:Label ID="lblPriceAfterTaxcolon" runat="server" Text=":" Visible="false"></asp:Label>
                                                        </td>
                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                            <asp:TextBox ID="txtPriceAfterTax" runat="server" CssClass="textComman" ReadOnly="True"
                                                                Visible="false" />
                                                        </td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:Label ID="lblTaxP" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Tax(%) %>" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblTaxPcolon" runat="server" Text=":"></asp:Label>
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:TextBox ID="txtTaxP" runat="server" Width="80px" CssClass="textComman" 
                                                             Enabled="false" />
                                                        <asp:Label ID="Label2" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Value %>" />
                                                        <asp:TextBox ID="txtTaxV" runat="server" Width="81px" CssClass="textComman"  Enabled="false" />
                                                      
                                                       
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:Label ID="lblTotalAmount" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance, Net Price %>" />
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:TextBox ID="txtTotalAmount" runat="server" CssClass="textComman" ReadOnly="True" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:Label ID="lblShippingCharge" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Shipping Charge %>" />
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:TextBox ID="txtShippingCharge" runat="server" CssClass="textComman"/>
                                                       
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:Label ID="lblNetAmount" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Net Amount %>" />
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:TextBox ID="txtNetAmount" runat="server" CssClass="textComman" ReadOnly="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:Label ID="lblRemark" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Remark%>" />
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' colspan="4">
                                                        <asp:TextBox ID="txtRemark" runat="server" CssClass="textComman" Width="928px" TextMode="MultiLine" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="6">
                                                        <table style="background-image: url(../Images/bgs.png); height: 100%" width="100%">
                                                            <tr>
                                                                <td valign="top">
                                                                    <asp:Panel ID="Panel10" runat="server">
                                                                        <fieldset>
                                                                            <legend>
                                                                                <asp:Label ID="lblDeviceParameter" Font-Names="Times New roman" Font-Size="18px"
                                                                                    Font-Bold="true" runat="server" Text="Advance Payment" CssClass="labelComman"></asp:Label>
                                                                            </legend>
                                                                            <table style="padding-left: 20px; padding-top: 5px; padding-bottom: 10px">
                                                                                <tr>
                                                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' width="140px" style="padding-left: 22px;">
                                                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Payment Mode %>"
                                                                                            CssClass="labelComman"></asp:Label>
                                                                                    </td>
                                                                                    <td width="1px">
                                                                                        :
                                                                                    </td>
                                                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' width="270px">
                                                                                        <asp:DropDownList ID="ddlTabPaymentMode" runat="server" CssClass="textComman" Width="260px"
                                                                                         >
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft() %>' width="150px">
                                                                                        <asp:Label ID="lblPayAmmount" runat="server" Text="<%$ Resources:Attendance,Balance Amount%>"
                                                                                            CssClass="labelComman"></asp:Label>
                                                                                    </td>
                                                                                    <td width="1px">
                                                                                        :
                                                                                    </td>
                                                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                                        <asp:TextBox ID="txtPayAmount" runat="server" CssClass="textComman"></asp:TextBox>
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                                                            TargetControlID="txtPayAmount" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <tr>
                                                                                        <td colspan="6" style="padding-left: 18px;">
                                                                                            <asp:Panel ID="pnlpaybank" runat="server">
                                                                                                <table>
                                                                                                    <tr>
                                                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' width="140px">
                                                                                                            <asp:Label ID="lblPayAccountNo" runat="server" Text="<%$ Resources:Attendance,Account No. %>"
                                                                                                                CssClass="labelComman"></asp:Label>
                                                                                                        </td>
                                                                                                        <td width="1px">
                                                                                                            :
                                                                                                        </td>
                                                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' width="270px">
                                                                                                            <asp:TextBox ID="txtPayAccountNo" runat="server" CssClass="textComman"
                                                                                                                BackColor="#eeeeee"></asp:TextBox>
                                                                                                            
                                                                                                        </td>
                                                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' width="150px">
                                                                                                            <asp:Label ID="lblPayBank" runat="server" Text="<%$ Resources:Attendance,Bank %>"
                                                                                                                CssClass="labelComman" Visible="false"></asp:Label>
                                                                                                        </td>
                                                                                                        <td width="1px">
                                                                                                            <asp:Label ID="lblpaybankcolon" runat="server" Text=":" CssClass="labelComman" Visible="false"></asp:Label>
                                                                                                        </td>
                                                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' width="270px">
                                                                                                            <asp:DropDownList ID="ddlPayBank" runat="server" CssClass="textComman" Width="261px"
                                                                                                                Visible="false">
                                                                                                            </asp:DropDownList>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr id="trcheque" runat="server" visible="false">
                                                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' width="130px">
                                                                                                            <asp:Label ID="lblPayChequeNo" runat="server" Text="<%$ Resources:Attendance,Cheque No %>"
                                                                                                                CssClass="labelComman"></asp:Label>
                                                                                                        </td>
                                                                                                        <td width="1px">
                                                                                                            :
                                                                                                        </td>
                                                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' width="270px">
                                                                                                            <asp:TextBox ID="txtPayChequeNo" runat="server" CssClass="textComman"></asp:TextBox>
                                                                                                        </td>
                                                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft() %>' width="150px">
                                                                                                            <asp:Label ID="lblPayChequeDate" runat="server" Text="<%$ Resources:Attendance,Cheque Date %>"
                                                                                                                CssClass="labelComman"></asp:Label>
                                                                                                        </td>
                                                                                                        <td width="1px">
                                                                                                            :
                                                                                                        </td>
                                                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                                                            <asp:TextBox ID="txtPayChequeDate" runat="server" CssClass="textComman"></asp:TextBox>
                                                                                                            <cc1:CalendarExtender ID="txtChequedate_CalenderExtender" runat="server" TargetControlID="txtPayChequeDate">
                                                                                                            </cc1:CalendarExtender>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr id="trcard" runat="server" visible="false">
                                                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                                                            <asp:Label ID="lblPayCardNo" runat="server" Text="<%$ Resources:Attendance,Card No %>"
                                                                                                                CssClass="labelComman"></asp:Label>
                                                                                                        </td>
                                                                                                        <td width="1px">
                                                                                                            :
                                                                                                        </td>
                                                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                                                            <asp:TextBox ID="txtPayCardNo" runat="server" CssClass="textComman"></asp:TextBox>
                                                                                                        </td>
                                                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft() %>'>
                                                                                                            <asp:Label ID="lblPayCardName" runat="server" Text="<%$ Resources:Attendance,Card Name %>"
                                                                                                                CssClass="labelComman"></asp:Label>
                                                                                                        </td>
                                                                                                        <td width="1px">
                                                                                                            :
                                                                                                        </td>
                                                                                                        <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                                                                            <asp:TextBox ID="txtPayCardName" runat="server" CssClass="textComman"></asp:TextBox>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </asp:Panel>
                                                                                        </td>
                                                                                    </tr>
                                                                                   
                                                                                    <tr>
                                                                                        <td colspan="6">
                                                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvPayment" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                                                ShowFooter="true" BorderStyle="Solid" Width="100%"  PageSize="<%# PageControlCommon.GetPageSize() %>">
                                                                                                
                                                                                                <Columns>
                                                                                                    
                                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Payment Mode %>">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblgvPaymentMode" runat="server" Text='<%# Eval("PaymentName").ToString() %>' />
                                                                                                        </ItemTemplate>
                                                                                                        <FooterStyle BorderStyle="None" />
                                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                                        <ItemStyle  />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Account Name%>">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblgvaccountno" runat="server" Text='<%# Eval("AccountName").ToString() %>' />
                                                                                                        </ItemTemplate>
                                                                                                        <FooterTemplate>
                                                                                                            <asp:Label ID="lbltotExp" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Amount%> " />
                                                                                                        </FooterTemplate>
                                                                                                        <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                                        <ItemStyle  />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Net Price %>">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("Pay_Charges").ToString() %>' />
                                                                                                        </ItemTemplate>
                                                                                                        <FooterTemplate>
                                                                                                            <asp:Label ID="txttotAmount" runat="server" Font-Bold="true" Text="0" />
                                                                                                        </FooterTemplate>
                                                                                                        <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                                        <ItemStyle  HorizontalAlign="Right" />
                                                                                                    </asp:TemplateField>
                                                                                                </Columns>
                                                                                                
                                                                                                
                                                                                                
                                                                                                
                                                                                            </asp:GridView>
                                                                                        </td>
                                                                                    </tr>
                                                                            </table>
                                                                        </fieldset>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                                        <asp:Label ID="lbldeliveryVoucher" runat="server" CssClass="labelComman" Text="Delivery Voucher"
                                                            Visible="false" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblcolondeliveryVoucher" runat="server" CssClass="labelComman" Text=":"
                                                            Visible="false" />
                                                    </td>
                                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' colspan="4">
                                                        <asp:DropDownList ID="ddlDeliveryvoucher" runat="server" CssClass="textComman" Visible="false">
                                                            <asp:ListItem Text="False" Value="False"></asp:ListItem>
                                                            <asp:ListItem Text="True" Value="True"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:CheckBox ID="chksendInproduction" runat="server" CssClass="labelComman" Text="Send In Production" />&nbsp;&nbsp;&nbsp;
                                                        <asp:CheckBox ID="chkSendInPO" runat="server" CssClass="labelComman" Text="<%$Resources:Attendance, Send In Purchase Order%>" />&nbsp;&nbsp;&nbsp;
                                                        <asp:CheckBox ID="chkPartialShipment" runat="server" CssClass="labelComman" Text="<%$Resources:Attendance,Partial Shipment%>" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:HiddenField ID="editid" runat="server" />
                                        <asp:HiddenField ID="hdnSalesQuotationId" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
    </div>
    </form>
</body>
</html>
