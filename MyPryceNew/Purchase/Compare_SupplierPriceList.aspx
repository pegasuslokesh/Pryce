<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Compare_SupplierPriceList.aspx.cs"
    Inherits="Purchase_PurchaseRequestPrint" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Print</title>

    <script language="javascript" type="text/javascript">

        function setprint() {
            window.print();
        }
    </script>

    <link href="../CSS/InvStyle.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-color: #4e4a4a" onload="setprint()">
    <form id="form1" runat="server">
    <div>
        <center>
            <table width="70%" cellpadding="0" cellspacing="0">
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
                        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Supplier Price List %>"
                            CssClass="LableHeaderTitle"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="imgPrint" ImageUrl="../Images/print.png" runat="server" Width="31px"
                            Height="30px" OnClick="imgPrint_Click" />
                    </td>
                </tr>
                <tr style="background-color: #fff">
                    <td>
                    </td>
                    <td>
                        <table width="100%">
                            <tr>
                                <td align="left" colspan="6">
                                    <table width="100%">
                                        <tr>
                                            <td align="left">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblCompanyName" runat="server" Font-Bold="true" CssClass="labelComman"
                                                                ForeColor="Black" Font-Size="16px"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblAddress" runat="server" Font-Bold="true" CssClass="labelComman"
                                                                ForeColor="Black"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblPhone" runat="server" Font-Bold="true" CssClass="labelComman" ForeColor="Black"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="right">
                                                <asp:Image ID="imgCompany" runat="server" Width="150px" Height="100px" />
                                            </td>
                                        </tr>
                                    </table>
                                    <hr />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td width="100%" align="left" colspan="6">
                                    <asp:DataList Width="100%" ID="datalistSupplier" runat="server" OnItemDataBound="datalistSupplier_ItemDataBound">
                                        <ItemTemplate>
                                            <br />
                                            <table>
                                                <tr>
                                                    <td width="100px" align="left">
                                                        <asp:Label ID="lblSName" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Supplier Name %>"></asp:Label>
                                                    </td>
                                                    <td width="10px" align="left">
                                                        :
                                                    </td>
                                                    <td >
                                                        <asp:Label ID="txtSupplierName" runat="server" Text='<%#Eval("Name") %>'
                                                            CssClass="labelComman" Font-Bold="true"></asp:Label>
                                                        <asp:HiddenField ID="hdnSupplierId" runat="server" Value='<%#Eval("Supplier_Id") %>' />
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvSupplier" runat="server"  Width="100%" AutoGenerateColumns="False">
                                                
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id%>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblproductcode" runat="server" Text='<%#Eval("ProductCode")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle  HorizontalAlign="Center" Width="80px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvProductId" runat="server" Text='<%#Eval("EProductName") %>' />
                                                            <asp:Label ID="lblProductId" runat="server" Text='<%#Eval("ProductId") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle  HorizontalAlign="Center" Width="80px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Price %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvSalesPrice" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle  HorizontalAlign="Center" Width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Last Purchase Price %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvLastPurchasePrice" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle  HorizontalAlign="Center" Width="100px" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                
                                                
                                                
                                            </asp:GridView>
                                        </ItemTemplate>
                                    </asp:DataList>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr style="background-color: #90BDE9">
                    <td>
                        <br />
                    </td>
                    <td style="padding-left: 5px" align="left">
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </center>
    </div>
    </form>
</body>
</html>
