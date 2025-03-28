<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompareQuatation.aspx.cs"
    Inherits="Purchase_CompareQuatation" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Compare Quotation</title>

    <script language="javascript" type="text/javascript">

        function setprint() {
            window.print();
        }
    </script>
    <link href="../CSS/InvStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Bootstrap_Files/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Bootstrap_Files/font-awesome-4.7.0/font-awesome-4.7.0/css/font-awesome.min.css" rel="stylesheet" />
    <link href="../Bootstrap_Files/ionicons-2.0.1/ionicons-2.0.1/css/ionicons.min.css" rel="stylesheet" />
    <link href="../Bootstrap_Files/dist/css/AdminLTE.min.css" rel="stylesheet" />
    <link href="../Bootstrap_Files/dist/css/skins/_all-skins.min.css" rel="stylesheet" />
    <link href="../Bootstrap_Files/Additional/Popup_Style.css" rel="stylesheet" />
</head>
<body style="background-color: #4e4a4a">
    <form id="form1" runat="server">
        <div class="row">
            <div class="col-md-12" style="background-color: #90BDE9">
                <div class="col-md-1">
                    <img src="../Images/compare.png" width="31" height="30" alt="D" />
                    <img src="../Images/seperater.png" width="2" height="30" alt="SS" />
                </div>
                <div class="col-md-9" style="float:left" align="left">
                    <asp:Label ID="lblHeader" style="font-size:20px;font-weight:500" runat="server" Text="<%$ Resources:Attendance,Quotation Comparison %>"
                       ></asp:Label>
                </div>
                <div class="col-md-2" style="float:right" align="right">
                    <asp:ImageButton ID="imgPrint" ImageUrl="../Images/print.png" runat="server" Width="31px"
                        Height="30px" OnClientClick="setprint();" />
                </div>
            </div>
            

            <div class="col-md-12" style="background-color: #e7e7e7">
                <div class="col-md-6">
                    <div class="col-md-12">
                        <asp:Label ID="lblCompanyName" runat="server" Font-Bold="true" 
                            ForeColor="Black" Font-Size="16px"></asp:Label>
                    </div>
                    <div class="col-md-12">
                        <asp:Label ID="lblAddress" runat="server" Font-Bold="true" 
                            ForeColor="Black"></asp:Label>
                    </div>
                    <div class="col-md-12">
                        <asp:Label ID="Label1" runat="server" Font-Bold="true"  ForeColor="Black"></asp:Label>
                    </div>
                    <div class="col-md-12">
                        <asp:Label ID="lblPhone" runat="server" Font-Bold="true"  ForeColor="Black"></asp:Label>
                    </div>
                </div>
                <div class="col-md-6" align="right">
                    <asp:Image CssClass="img-responsive" ID="imgCompany" runat="server" Width="150px" Height="100px" />
                </div>
            </div>
            <div class="col-md-12" style="background-color: #e7e7e7">
                <hr style="height:5px" />
            </div>
            <div class="col-md-12" style="background-color: #e7e7e7" align="right">
                <asp:Button ID="btnConfirm" Text="<%$ Resources:Attendance,Confirm %>" runat="server" OnClick="conOrder_Click" CssClass="buttonCommman" />
                <br />
            </div>
            <div class="col-md-12" style="overflow: auto">
                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvSupplier" runat="server" BackColor="White" CellPadding="0" CellSpacing="0"
                    ForeColor="Black" GridLines="Horizontal" AutoGenerateColumns="false" Width="100%"
                    OnRowDataBound="gvSupplier1_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button runat="server" Text="<%$ Resources:Attendance,Add to Product%>" ID="btnAddNewProduct" Visible='<%# Convert.ToBoolean(ProductCodeCheck(Eval("Product_Id").ToString())) %>' OnCommand="btnAddNewProduct_OnClick" CommandArgument='<%# Eval("Trans_Id") %>'></asp:Button>

                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Serial No %>" ItemStyle-BorderStyle="Solid"
                            ItemStyle-BorderWidth="1px">
                            <ItemTemplate>
                                <asp:Label ID="lblgvSerialNumber" Font-Size="13px" ForeColor="#474646" Font-Names="Trebuchet MS"
                                    runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                <asp:Label ID="lbltrans" Text='<%# Eval("Trans_Id").ToString() %>' Visible="false" runat="server"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle BorderStyle="Solid" BorderWidth="1px" Font-Size="13px" ForeColor="#474646"
                                Font-Names="Trebuchet MS" Font-Bold="true" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id%>" ItemStyle-BorderStyle="Solid"
                            ItemStyle-BorderWidth="1px">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdngvProductId" runat="server" Value='<%#Eval("Product_Id") %>' />
                                <asp:Label ID="lblproductcode" runat="server" Text='<%#Eval("ProductCode")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle BorderStyle="Solid" BorderWidth="1px" Font-Size="13px" Font-Bold="true"
                                ForeColor="#474646" Font-Names="Trebuchet MS" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>" ItemStyle-BorderStyle="Solid"
                            ItemStyle-BorderWidth="1px">
                            <ItemTemplate>
                                <asp:Label ID="txtProductName" runat="server" CssClass="labelComman" Font-Bold="true"
                                    Text='<%#Eval("ProductName") %>'></asp:Label>
                                <asp:Label ID="lblProductId" Visible="false" runat="server" CssClass="labelComman"
                                    Font-Bold="true" Text='<%# Eval("Product_Id") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle BorderStyle="Solid" BorderWidth="1px" Font-Size="13px" Font-Bold="true"
                                ForeColor="#474646" Font-Names="Trebuchet MS" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quantity %>" ItemStyle-Width="80px"
                            ItemStyle-BorderStyle="Solid" ItemStyle-BorderWidth="1px">
                            <ItemTemplate>
                                <asp:Label ID="lblReqQty" runat="server" Font-Size="13px" ForeColor="#474646" Font-Names="Trebuchet MS"
                                    Text='<%# GetAmountDecimal(Eval("ReqQty").ToString()) %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <HeaderStyle BorderStyle="Solid" BorderWidth="1px" Font-Size="13px" ForeColor="#474646"
                                Font-Names="Trebuchet MS" Font-Bold="true" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Supplier %>" ItemStyle-BorderStyle="Solid"
                            ItemStyle-BorderWidth="1px">
                            <ItemTemplate>
                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvSupplier" runat="server" BackColor="White" CellPadding="4" ForeColor="Black"
                                    GridLines="Horizontal" AutoGenerateColumns="false" Width="700px">
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Select %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltrans2" Text='<%# Eval("Trans_Id").ToString() %>' Visible="false" runat="server"></asp:Label>
                                                <asp:RadioButton ID="rbtnSelect" AutoPostBack="true" runat="server" OnCheckedChanged="rbtnSelect_CheckedChanged" Checked='<%# CheckSt(Eval("Trans_Id").ToString()) %>' />

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance, Name %>" ItemStyle-Width="120px"
                                            ItemStyle-BorderStyle="Solid" ItemStyle-BorderWidth="1px">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdngvSupplierId" runat="server" Value='<%#Eval("Supplier_Id") %>' />
                                                <asp:Label ID="lblgvSupplierName" runat="server" Font-Size="13px" ForeColor="#474646"
                                                    Font-Names="Trebuchet MS" Text='<%#Eval("SupplierName") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle BorderStyle="Solid" BorderWidth="1px" Font-Size="13px" Font-Bold="true"
                                                ForeColor="#474646" Font-Names="Trebuchet MS" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Terms & Conditions %>" ItemStyle-Width="400px"
                                            ItemStyle-BorderStyle="Solid" ItemStyle-BorderWidth="1px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvTermCondition" runat="server" Font-Size="13px" ForeColor="#474646"
                                                    Font-Names="Trebuchet MS" Text='<%# Eval("TermsCondition") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle BorderStyle="Solid" BorderWidth="1px" Font-Size="13px" Font-Bold="true"
                                                ForeColor="#474646" Font-Names="Trebuchet MS" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Amount %>" ItemStyle-Width="80px"
                                            ItemStyle-BorderStyle="Solid" ItemStyle-BorderWidth="1px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmmount" runat="server" Font-Size="13px" ForeColor="#474646" Font-Names="Trebuchet MS"
                                                    Text='<%# GetAmountDecimal(Eval("Field5").ToString()) %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                            <HeaderStyle BorderStyle="Solid" BorderWidth="1px" Font-Size="13px" ForeColor="#474646"
                                                Font-Names="Trebuchet MS" Font-Bold="true" />
                                        </asp:TemplateField>

                                    </Columns>
                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                    <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                                    <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle Font-Bold="True" ForeColor="Black" />
                                </asp:GridView>
                            </ItemTemplate>
                            <HeaderStyle BorderStyle="Solid" BorderWidth="1px" Font-Size="13px" Font-Bold="true"
                                ForeColor="#474646" Font-Names="Trebuchet MS" />
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                    <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                    <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle Font-Bold="True" ForeColor="Black" />
                </asp:GridView>
            </div>
            <div class="col-md-12" style="background-color: #90BDE9">
            </div>


        </div>
    </form>
</body>
</html>
