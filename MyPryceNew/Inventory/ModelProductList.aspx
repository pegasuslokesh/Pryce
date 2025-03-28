<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ModelProductList.aspx.cs" EnableEventValidation="false" Inherits="Inventory_ModelProductList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
    <link href="../Bootstrap_Files/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Bootstrap_Files/dist/css/AdminLTE.min.css" rel="stylesheet" />
    <link href="../CSS/InvStyle.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../Bootstrap_Files/dist/css/skins/_all-skins.min.css" />
    <title>Model Product List</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="Update_New" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="imgExport" />
            </Triggers>
            <ContentTemplate>
                <div class="wrapper" style="background-color: #ecf0f5;">
                    <section class="content">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="box box-primary box-solid">
                                    <div class="box-header with-border">
                                        <h2 class="box-title">
                                            <img src="../Images/product.png" width="31" height="30" alt="" />
                                            <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Product Setup%>"></asp:Label>
                                        </h2>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <div class="alert alert-info ">
                                                    <div class="row">
                                                        <div class="form-group">
                                                            <div class="col-lg-2">
                                                                <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFieldName_OnSelectedIndexChanged">
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Product Id %>" Value="ProductCode"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Product Name %>" Value="EProductName"
                                                                        Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Model No. %>" Value="ModelNo"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Item Type %>" Value="ItemType"></asp:ListItem>
                                                                    <asp:ListItem Text="AlternateId 1" Value="AlternateId1"></asp:ListItem>
                                                                    <asp:ListItem Text="AlternateId 2" Value="AlternateId2"></asp:ListItem>
                                                                    <asp:ListItem Text="AlternateId 3" Value="AlternateId3"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-lg-2">
                                                                <asp:DropDownList ID="ddlOption" runat="server" CssClass="form-control">
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-lg-3">
                                                                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                                <asp:TextBox ID="txtValue" runat="server" CssClass="form-control"/>
                                                                <asp:DropDownList ID="ddlitemtypeserach" runat="server" CssClass="textComman" Visible="false">
                                                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance, --Select--%>" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="Stockable" Value="S"></asp:ListItem>
                                                                    <asp:ListItem Text="NonStockable" Value="NS"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:Attendance, Kit %>" Value="K"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                    </asp:Panel>
                                                            </div>

                                                            <div class="col-lg-3">
                                                                <asp:ImageButton ID="btnbind" Style="margin-top: -5px;" runat="server" CausesValidation="False"
                                                                    ImageUrl="~/Images/search.png" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>

                                                                <asp:ImageButton ID="btnRefresh" runat="server" Style="width: 33px;" CausesValidation="False"
                                                                    ImageUrl="~/Images/refresh.png" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                                                <asp:HiddenField ID="HDFSort" runat="server" />

                                                                <asp:ImageButton ID="imbBtnGrid" Style="width: 33px;" CausesValidation="False" Visible="False"
                                                                    runat="server" ImageUrl="~/Images/a1.png" OnClick="imbBtnGrid_Click" ToolTip="<%$ Resources:Attendance, Grid View %>" />

                                                                <asp:ImageButton ID="imgBtnDatalist" Style="width: 33px;" CausesValidation="False"
                                                                    runat="server" ImageUrl="~/Images/NewTree.png" OnClick="imgBtnDatalist_Click"
                                                                    ToolTip="<%$ Resources:Attendance,List View %>" />

                                                                <asp:ImageButton ID="imgExport" Style="width: 33px;" CausesValidation="False"
                                                                    runat="server" ImageUrl="~/Images/xls-icon.png" OnClick="imgExport_Click" ToolTip="<%$ Resources:Attendance,List View %>" />
                                                            </div>

                                                            <div class="col-lg-2" style="margin-top:10px;">
                                                                <h4>
                                                                    <asp:Label ID="lblTotalRecord" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>" /></h4>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="box box-warning box-solid">
                                                    <div class="box-header with-border">
                                                        <h3 class="box-title"></h3>
                                                    </div>
                                                    <div class="box-body">
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div class="flow">
                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvProduct" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                                                        OnPageIndexChanging="gvProduct_PageIndexChanging"  Width="100%" OnRowDataBound="GvProductDetail_OnRowDataBound"
                                                                        DataKeyNames="ProductId" PageSize="<%# PageControlCommon.GetPageSize() %>" AllowSorting="true"  OnSorting="gvProduct_Sorting">
                                                                        <Columns>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <asp:Image ID="btnImgProduct" runat="server" Width="120px" Height="100px" ImageUrl='<%# string.IsNullOrEmpty(Eval("PImage").ToString()) ? "~/Login/Images/place-holder.png" : "~/CompanyResource/" + System.Web.HttpContext.Current.Session["CompId"].ToString() + "/Product/"+ Eval("PImage").ToString() %>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle  Width="120px" />
                                                                            </asp:TemplateField>

                                                                             <asp:TemplateField HeaderText="<%$ Resources:Attendance,Category Name %>" SortExpression="CategoryName">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblCategoryName" runat="server" Text='<%# Eval("CategoryName") %>'></asp:Label>
                                                                                   <%-- <asp:HiddenField ID="hdngvProductId" runat="server" Value='<%# Eval("ProductId") %>' />--%>
                                                                                </ItemTemplate>
                                                                                <ItemStyle  />
                                                                            </asp:TemplateField>


                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id %>" SortExpression="ProductCode">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblProductId" runat="server" Text='<%# Eval("ProductCode") %>'></asp:Label>
                                                                                    <asp:HiddenField ID="hdngvProductId" runat="server" Value='<%# Eval("ProductId") %>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle  />
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="EProductName" HeaderText="<%$ Resources:Attendance,Product Name %>"
                                                                                SortExpression="EProductName" ItemStyle-Width="40%" />
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Item Type %>" SortExpression="ItemTypeValue">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblItemType" runat="server" Text='<%# Eval("ItemTypeValue") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle  />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Product Unit %>" SortExpression="UnitName">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblPUnit" runat="server" Text='<%#  Eval("UnitName") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle  />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Stock %>">
                                                                                <ItemTemplate>
                                                                                    <%--  <asp:Label ID="lnkStockInfo" runat="server" Text='<%# Eval("StockQty") %>' ToolTip="View Detail"
                                                                CssClass="labelComman"></asp:Label>--%>
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td valign="top">

                                                                                                <asp:DataList ID="dtlistStock" runat="server" RepeatColumns="1" RepeatDirection="Horizontal">

                                                                                                    <ItemTemplate>


                                                                                                        <table>

                                                                                                            <tr>
                                                                                                                <td width="150px">
                                                                                                                    <asp:Label ID="lblLocation" CssClass="labelComman" runat="server" Text='<%# Eval("Short_Location_Name") %>'></asp:Label>
                                                                                                                </td>
                                                                                                                <td width="1px">:
                                                                                                                </td>
                                                                                                                <td width="50px" align="right">
                                                                                                                    <asp:Label ID="lblPurQty" runat="server" CssClass="labelComman" Text='<%# SetDecimal(Eval("Quantity").ToString()) %>'></asp:Label>
                                                                                                                </td>
                                                                                                            </tr>

                                                                                                        </table>

                                                                                                        </div>
                                                                                                    </ItemTemplate>
                                                                                                </asp:DataList>



                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </ItemTemplate>
                                                                                <ItemStyle  VerticalAlign="Top" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Sales Price%>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lbsalesPrice" runat="server" Text='<%# GetSalesPriceUsingID(Eval("ProductId").ToString()) %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle  />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblCreatedBy" runat="server" Text='<%# Eval("CreatedEmpName") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle  />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblModifiedBy" runat="server" Text='<%# Eval("ModifiedEmpName") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle  />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        
                                                                        
                                                                        
                                                                        
                                                                    </asp:GridView>
                                                                    <br />
                                                                    <asp:DataList ID="dtlistProduct" runat="server" RepeatColumns="4" RepeatDirection="Horizontal" Visible="false"
                                                                        Width="100%">
                                                                        <ItemTemplate>
                                                                            <div class="product_box">
                                                                                <table width="100%" style="height: 200px;">
                                                                                    <tr>
                                                                                        <td colspan="4">
                                                                                            <asp:Panel ID="pnlProduct" runat="server" Width="310px" Height="40px">
                                                                                                <asp:LinkButton ID="lbldlProductName" runat="server" Font-Bold="true" ForeColor="#1886b9"
                                                                                                    CssClass="labelComman" Style="text-decoration: none;" Width="310px" Text='<%# Eval("ShortProductName") %>'
                                                                                                    ToolTip='<%# Eval("EProductName") %>' Enabled="False"></asp:LinkButton>
                                                                                            </asp:Panel>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td valign="middle" rowspan="8" width="1px">
                                                                                            <%--                                                      <%-- <dx:ASPxImageZoom ID="zoom" runat="server" ImageUrl='<%#"~/Handler.ashx?ImID="+ Eval("ProductId") %>'
            LargeImageUrl='<%#"~/Handler.ashx?ImID="+ Eval("ProductId") %>' Height="120px" Width="120px" SettingsZoomMode-ZoomWindowWidth="500px" SettingsZoomMode-ZoomWindowHeight="500px"
            ExpandWindowText="Expand window text" ZoomWindowText="Zoom window text" >--%>
                                                                                            <%-- <dx:ASPxImageZoom ID="ASPxImageZoom1" runat="server" >
                                                                </dx:ASPxImageZoom>--%>
                                                                                            <asp:ImageButton ID="btnImgProduct" runat="server" CommandArgument='<%# Eval("ProductId") %>'
                                                                                                Width="120px" Height="120px" ImageUrl='<%# string.IsNullOrEmpty(Eval("PImage").ToString()) ? "~/Login/Images/place-holder.png" : "~/CompanyResource/" + System.Web.HttpContext.Current.Session["CompId"].ToString() + "/Product/"+ Eval("PImage").ToString() %>'
                                                                                                Enabled="False" />
                                                                                        </td>
                                                                                        <td width="60px">
                                                                                            <asp:Label ID="lblProductId" runat="server" Text="<%$ Resources:Attendance,Product Id %>"
                                                                                                CssClass="labelComman" Width="60px"></asp:Label>
                                                                                        </td>
                                                                                        <td width="3px">:
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lbldlProductId" runat="server" Width="135px" CssClass="labelComman"
                                                                                                Text='<%# Eval("ProductCode") %>'></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblModelNo" runat="server" Text="<%$ Resources:Attendance,Item Type %>"
                                                                                                CssClass="labelComman"></asp:Label>
                                                                                        </td>
                                                                                        <td>:
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblModelCode" runat="server" Text='<%# Eval("ItemTypeValue") %>' CssClass="labelComman"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblItypeType" runat="server" Text="<%$ Resources:Attendance,Stock%>"
                                                                                                CssClass="labelComman"></asp:Label>
                                                                                        </td>
                                                                                        <td>:
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lnkStockInfo" runat="server" Text='<%# Eval("StockQty") %>' ToolTip="View Detail"
                                                                                                CssClass="labelComman"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblCostPrice" Width="70px" runat="server" Text="<%$ Resources:Attendance,Sales Price %>"
                                                                                                CssClass="labelComman"></asp:Label>
                                                                                        </td>
                                                                                        <td>:
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lbldlCostPrice" runat="server" CssClass="labelComman" Text='<%#GetSalesPriceUsingID(Eval("ProductId").ToString()) %>'></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblProductUnit" runat="server" Text="<%$ Resources:Attendance,Unit %>"
                                                                                                CssClass="labelComman"></asp:Label>
                                                                                        </td>
                                                                                        <td>:
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblUnit" runat="server" CssClass="labelComman" Text='<%# Eval("UnitName") %>'></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Created By %>"
                                                                                                CssClass="labelComman"></asp:Label>
                                                                                        </td>
                                                                                        <td>:
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="Label9" runat="server" CssClass="labelComman" Text='<%# Eval("CreatedEmpName") %>'></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Modified By %>"
                                                                                                CssClass="labelComman"></asp:Label>
                                                                                        </td>
                                                                                        <td>:
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="Label11" runat="server" CssClass="labelComman" Text='<%# Eval("ModifiedEmpName") %>'></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <br />
                                                                        </ItemTemplate>
                                                                    </asp:DataList>
                                                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </section>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_New">
            <ProgressTemplate>
                <div class="modal_Progress">
                    <div class="center_Progress">
                        <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </form>
</body>
</html>

