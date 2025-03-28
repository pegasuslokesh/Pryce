<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddProduct.aspx.cs" Inherits="Purchase_AddProduct" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Add Product</title>

    <script language="javascript" type="text/javascript">

        function setprint() {
            window.print();
        }
        function refreshParent() {
           
            window.opener.location.href = window.opener.location.href;
            window.opener.location.reload();
            window.close();
        }
    </script>

    <link href="../CSS/InvStyle.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-color: #4e4a4a">
    <form id="form1" runat="server">
    <div>
        <center>
            <table cellpadding="0" cellspacing="0">
                <tr style="background-color: #90BDE9">
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <img src="../Images/compare.png" width="31" height="30" alt="D" />
                                </td>
                                <td>
                                    <img src="../Images/seperater.png" width="2" height="43" alt="SS" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="padding-left: 5px" align="left">
                        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Add Product %>"
                            CssClass="LableHeaderTitle"></asp:Label>
                    </td>
                    <td>
                      
                    </td>
                </tr>
                <tr style="background-color: #e7e7e7">
                    <td align="right" colspan="3">
                       
                        <hr />
                        <br />
                    </td>
                </tr>
                <tr style="background-color: #e7e7e7">
              <td></td>
                <td align="right"> 
                <table>
                <tr>
                <td> 
                 <asp:Panel ID="pnlAddProductDetail" runat="server" DefaultButton="btnSave">
                            <table width="100%" style="padding-left: 43px">
                                <tr>
                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                        <asp:Label ID="Label38" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Product Id%>" />
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' colspan="4">
                                        <asp:TextBox ID="txtProductcode" runat="server" CssClass="textComman" AutoPostBack="True"
                                            OnTextChanged="txtProductCode_TextChanged" BackColor="#eeeeee" Width="630px" />
                                    
                                    </td>
                                </tr>
                                <tr>
                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                        <asp:Label ID="lblProductName" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Product Name %>" />
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' colspan="4">
                                        <asp:TextBox ID="txtProductName" runat="server" CssClass="textComman" BackColor="#eeeeee"
                                            AutoPostBack="True" OnTextChanged="txtProductName_TextChanged" Width="630PX" />
                                      
                                    </td>
                                </tr>
                                <tr>
                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                        <asp:Label ID="lblUnit" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Unit %>" />
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td width="280px" align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                        <asp:DropDownList ID="ddlUnit" runat="server" CssClass="textComman" Width="260PX" />
                                    </td>
                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                       
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                       
                                    </td>
                                </tr>
                               
                                <tr>
                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>'>
                                        <asp:Label ID="lblPDescription" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Product Description %>" />
                                    </td>
                                    <td align="center">
                                        :
                                    </td>
                                    <td align='<%= PageControlCommon.ChangeTDForDefaultLeft()%>' colspan="4">
                                     
                                        <asp:Panel ID="pnlPDescription" runat="server" Width="630px" Height="100px" CssClass="textComman"
                                            BorderColor="#8ca7c1" BackColor="#ffffff" ScrollBars="Vertical">
                                            <asp:Literal ID="txtPDescription" runat="server"></asp:Literal>
                                        </asp:Panel>
                </td>
                </tr>
                <tr>
                <td colspan="3" align="center">
                <asp:Button ID="btnSave" runat="server" CssClass="buttonCommman" Text="<%$ Resources:Attendance,Save %>"
                                                                    OnClick="btnSave_Click" />
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
        </center>
    </div>
    </form>
</body>
</html>
