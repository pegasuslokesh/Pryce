<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddItem.aspx.cs" Inherits="Inventory_AddItem" %>

<%@ Register Src="~/WebUserControl/StockAnalysis.ascx" TagName="StockAnalysis" TagPrefix="SA" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>Add Item</title>
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
    <link href="../Bootstrap_Files/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Bootstrap_Files/font-awesome-4.7.0/font-awesome-4.7.0/css/font-awesome.min.css" rel="stylesheet" />
    <link href="../Bootstrap_Files/ionicons-2.0.1/ionicons-2.0.1/css/ionicons.min.css" rel="stylesheet" />
    <link href="../Bootstrap_Files/dist/css/AdminLTE.min.css" rel="stylesheet" />
    <link href="../Bootstrap_Files/dist/css/skins/_all-skins.min.css" rel="stylesheet" />
    <link href="../Bootstrap_Files/Additional/Popup_Style.css" rel="stylesheet" />
    <style>
        .FixedDiv {
            position: fixed;
            top: 64px;
        }
    </style>
</head>
<body id="Body_1" class="hold-transition skin-blue sidebar-mini fixed">
    <form id="Form" runat="server">
        <asp:ScriptManager ID="ScriptManager" runat="server" EnablePageMethods="true"></asp:ScriptManager>
        <div class="wrapper">
            <header class="main-header">
                <a class="logo" style="background-color: #3c8dbc;">
                    <span class="logo-mini">
                        <img src="../Images/compare.png" width="31" height="30" alt="D" /></span>
                    <span class="logo-lg">
                        <img src="../Images/compare.png" width="31" height="30" alt="D" />
                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Product Setup %>" CssClass="LableHeaderTitle"></asp:Label></span>
                </a>
                <nav class="navbar navbar-static-top">
                    <a id="Btn_Menu" href="#" class="sidebar-toggle" data-toggle="offcanvas" role="button">
                        <span class="sr-only">Toggle navigation</span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </a>
                    <div class="navbar-custom-menu">
                        <ul class="nav navbar-nav">
                        </ul>
                    </div>
                </nav>
            </header>

            <aside id="Aside_Menu" class="main-sidebar" style="background-color: #ecf0f5">
                <section class="sidebar">
                    <div id="Product_Div" class="FixedDiv" style="max-width: 245px; min-width: 245px;">
                        <div class="box box-warning box-solid">
                            <div class="box-header with-border" style="text-align: center">
                                <h3 class="box-title">Product List
                                </h3>
                            </div>
                            <div class="box-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div style="overflow: auto; max-height: 500px; overflow: auto;">
                                            <asp:UpdatePanel ID="Update_Product" runat="server">
                                                <ContentTemplate>
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvProduct" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                                        Width="100%" DataKeyNames="ProductId" PageSize="<%# PageControlCommon.GetPageSize() %>">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="S.No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSerialno" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="4%" />
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("ProductId") %>'
                                                                        ImageUrl="~/Images/Erase.png" OnCommand="IbtnDelete_Command" ToolTip="<%$ Resources:Attendance,Delete %>" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("ProductCode") %>'></asp:Label>
                                                                    <asp:HiddenField ID="hdnProductid" runat="server" Value='<%# Eval("ProductId") %>' />
                                                                    <asp:HiddenField ID="hdnMaintainStock" runat="server" Value='<%# Eval("MaintainStock") %>' />
                                                                    <asp:HiddenField ID="hdnunitId" runat="server" Value='<%# Eval("UnitId") %>' />
                                                                    <asp:Label ID="lblDesc" Visible="false" runat="server" Text='<%# Eval("ProductDescription") %>' />
                                                                    <asp:Label ID="lblProductName" Visible="false" runat="server" Text='<%# Eval("SuggestedProductName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="90%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quantity %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" />
                                                            </asp:TemplateField>
                                                        </Columns>




                                                    </asp:GridView>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div style="text-align: center">
                                            <br />
                                            <asp:Button ID="btnAddtoLIst" runat="server" CausesValidation="False" Text="Add To List" CssClass="btn btn-primary" OnClick="btnAddtoLIst_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
            </aside>

            <div class="content-wrapper">
                <section class="content">
                    <div class="box box-primary box-solid">
                        <div class="box-header with-border">
                            <h3 class="box-title"></h3>
                        </div>
                        <div class="box-body">
                            <asp:UpdatePanel ID="Update_List" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="box box-primary">
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <div class="col-md-4">
                                                            <asp:Label ID="lblBrandsearch" runat="server" Text="<%$ Resources:Attendance,Manufacturing Brand %>"></asp:Label>
                                                            <asp:DropDownList ID="ddlbrandsearch" runat="server" CssClass="form-control"></asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:Label ID="lbllocationsearch" runat="server" Text="<%$ Resources:Attendance,Category %>" />
                                                            <asp:DropDownList ID="ddlcategorysearch" runat="server" CssClass="form-control"></asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:Label ID="Label3" runat="server" Text="Location" />
                                                            <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                            <br />
                                                        </div>

                                                        <%-- <div class="col-md-4">
                                                            <br />
                                                            <asp:Panel ID="Pnl_Search" runat="server" DefaultButton="btngo">
                                                                <asp:TextBox ID="txtSearchPrduct" runat="server" CssClass="form-control"></asp:TextBox>
                                                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" WatermarkText="Search Product" TargetControlID="txtSearchPrduct"></cc1:TextBoxWatermarkExtender>
                                                                <asp:Button ID="btnsearchProduct" runat="server" OnClick="btngo_Click" Visible="false" />
                                                            </asp:Panel>
                                                            <br />
                                                        </div>--%>

                                                        <div class="col-md-4">
                                                            <asp:Label runat="server" ID="lblFrequency" Text="Frequency"></asp:Label>
                                                            <asp:DropDownList runat="server" ID="ddlFrequency" CssClass="form-control">
                                                                <asp:ListItem Text="Select" Value=""></asp:ListItem>
                                                                <asp:ListItem Text="Moving" Value="moving"></asp:ListItem>
                                                                <asp:ListItem Text="Non-Moving" Value="nonmoving"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>

                                                        <div class="col-md-4">
                                                            <asp:Label runat="server" ID="Label2" Text="From Date"></asp:Label>
                                                            <asp:DropDownList runat="server" ID="ddlTimePeriod" CssClass="form-control">
                                                                <asp:ListItem Text="1 Month" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="3 Month" Value="3"></asp:ListItem>
                                                                <asp:ListItem Text="6 Month" Value="6"></asp:ListItem>
                                                                <asp:ListItem Text="1 Year" Value="12"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>

                                                        <div class="col-md-4">
                                                            <br />
                                                            <asp:Button ID="btngo" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Go %>" CssClass="btn btn-primary" OnClick="btngo_Click" />
                                                            <asp:Button ID="btnResetSreach" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Reset %>" CssClass="btn btn-primary" OnClick="btnResetSreach_Click" />
                                                            <br />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="alert alert-info ">
                                        <div class="row">
                                            <div class="form-group">
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Product Id %>" Value="ProductCode" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Product Name %>" Value="EProductName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Alternate Id %>" Value="AlternateId"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Model Name %>" Value="ModelName"></asp:ListItem>
                                                        <%-- <asp:ListItem Text="<%$ Resources:Attendance,Model No. %>" Value="ModelNo"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Item Type %>" Value="ItemType"></asp:ListItem>--%>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlOption" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                        <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:ImageButton ID="btnbind" runat="server" CausesValidation="False" Style="margin-top: -5px;"
                                                        ImageUrl="~/Images/search.png" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>" />

                                                    <asp:ImageButton ID="btnRefresh" runat="server" CausesValidation="False" Style="width: 33px;"
                                                        ImageUrl="~/Images/refresh.png" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:DropDownList ID="ddlGridSize" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlGridSize_SelectedIndexChanged">
                                                        <asp:ListItem Text="Default" Value="0" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                        <asp:ListItem Text="100" Value="100"></asp:ListItem>
                                                        <asp:ListItem Text="150" Value="150"></asp:ListItem>
                                                        <asp:ListItem Text="200" Value="200"></asp:ListItem>
                                                        <asp:ListItem Text="250" Value="250"></asp:ListItem>
                                                        <asp:ListItem Text="300" Value="300"></asp:ListItem>
                                                        <asp:ListItem Text="350" Value="350"></asp:ListItem>
                                                        <asp:ListItem Text="400" Value="400"></asp:ListItem>
                                                        <asp:ListItem Text="450" Value="450"></asp:ListItem>
                                                        <asp:ListItem Text="500" Value="500"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <h5>
                                                        <asp:Label ID="lblTotalRecord" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>" /></h5>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-12">
                                        <div class="box box-warning box-solid">
                                            <div class="box-header with-border">
                                                <h3 class="box-title"></h3>
                                            </div>
                                            <div class="box-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div class="flow">
                                                            <asp:DataList ID="dtlistProduct" runat="server" RepeatColumns="1" RepeatDirection="Horizontal" Width="100%">
                                                                <ItemTemplate>
                                                                    <div class="col-md-12">
                                                                        <div class="box box-warning">
                                                                            <div class="box-header with-border">
                                                                                <div class="col-md-8">
                                                                                    <h5 class="box-title">
                                                                                        <asp:CheckBox ID="chkAddItem" runat="server" OnCheckedChanged="chkAddItem_CheckedChanged" AutoPostBack="true" />
                                                                                        <asp:LinkButton ID="lbldlProductName" runat="server" Font-Size="14px" Font-Bold="true" ForeColor="#1886b9"
                                                                                            Style="text-decoration: none;" CommandArgument='<%# Eval("ProductId") %>'
                                                                                            Text='<%# Eval("ShortProductName") %>' Enabled="False"></asp:LinkButton>
                                                                                        <asp:HiddenField ID="hdnProductId" runat="server" Value='<%# Eval("ProductId") %>' />
                                                                                        <asp:HiddenField ID="hdnMaintainStock" runat="server" Value='<%# Eval("MaintainStock") %>' />
                                                                                        <asp:Label ID="lblDesc" Visible="false" runat="server" Text='<%# Eval("Description") %>' />
                                                                                    </h5>
                                                                                </div>

                                                                                <div class="col-md-2">
                                                                                    <asp:TextBox ID="txtquantity" runat="server" onkeypress="Press_Enter_Key()" CssClass="form-control txtquantity" Text="1" MaxLength="7"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="txtFilter1" runat="server" TargetControlID="txtquantity" FilterType="Numbers"></cc1:FilteredTextBoxExtender>
                                                                                </div>
                                                                                <div class="col-md-2">
                                                                                    <asp:LinkButton ID="btnRelatedProduct" runat="server" ForeColor="Blue" CausesValidation="False" Text="More..." OnClick="btnRelatedProduct_Click" Font-Size="13px" Font-Underline="false" Height="30px" Visible="false" />
                                                                                </div>

                                                                            </div>
                                                                            <div class="box-body">
                                                                                <div class="col-md-12">
                                                                                    <div class="col-md-3">
                                                                                       <%-- <asp:ImageButton ID="btnImgProduct" runat="server" CommandArgument='<%# Eval("ProductId") %>' Width="70px" Height="70px" ImageUrl='<%# "~/Handler.ashx?ImID="+ Eval("ProductId") %>' Enabled="False" />--%>
                                                                                        <asp:ImageButton ID="btnImgProduct" runat="server" CommandArgument='<%# Eval("ProductId") %>' Width="70px" Height="70px" ImageUrl='<%#GetImageUrl(Eval("ProductId").ToString()) %>' Enabled="False" />
                                                                                    </div>
                                                                                    <div class="col-md-9">
                                                                                        <table>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblProductId" runat="server" Text="<%$ Resources:Attendance,Product Id %>"></asp:Label>
                                                                                                </td>
                                                                                                <td>:
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lbldlProductId" runat="server" Text='<%# Eval("ProductCode") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td style="width: 50px;">&nbsp
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblModelNo" runat="server" Text="<%$ Resources:Attendance,Unit%>"></asp:Label>
                                                                                                </td>
                                                                                                <td>:
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lbldltModelNo" runat="server" Text='<%# Eval("Unit_Name") %>'></asp:Label>
                                                                                                    <asp:HiddenField ID="hdnunitId" runat="server" Value='<%# Eval("UnitId") %>' />
                                                                                                </td>
                                                                                                <td style="width: 50px;">&nbsp
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblItypeType" runat="server" Text="<%$ Resources:Attendance,Sales Price %>"></asp:Label>
                                                                                                </td>
                                                                                                <td>:
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblsalesprice" runat="server" Text='<%#GetSalesPriceinLocal(Eval("SalesPrice").ToString())%>'></asp:Label>
                                                                                                </td>
                                                                                                <td style="width: 50px;">&nbsp
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblCostPrice" runat="server" Width="110px" Text="<%$ Resources:Attendance,Available Quantity%>"></asp:Label>
                                                                                                </td>
                                                                                                <td>:
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:LinkButton id="lnkStockInfo" runat="server" CommandArgument='<%# Eval("ProductId") %>' ToolTip="View Detail" OnCommand="lnkStockInfo_Command1"  OnClientClick="$('#Product_StockAnalysis').modal('show');" style="cursor: pointer">
                                                                                                        <%#GetAmountDecimal(Eval("StockQuantity").ToString()) %>
                                                                                                    </asp:LinkButton>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                </div>

                                                                                <div class="col-md-6">

                                                                                    <div class="col-md-6">
                                                                                        <asp:Button class="btn btn-box-tool" Text="Show Category Wise Related Product" runat="server" ID="btnRPC" OnCommand="btnRPC_Click" CommandArgument='<%# Eval("CategoryID") %>'></asp:Button>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Button class="btn btn-box-tool" Text="Hide Category Products" runat="server" ID="btnHideRPC" OnCommand="btnRPC_Click" CommandArgument='<%# Eval("CategoryID") %>'></asp:Button>
                                                                                        <br />
                                                                                    </div>

                                                                                    <div style="overflow: auto; max-height: 300px; min-height: 0px" class="col-md-12">
                                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" runat="server" ID="gvRelCat" AutoGenerateColumns="false">
                                                                                            <Columns>
                                                                                                <asp:TemplateField>
                                                                                                    <ItemTemplate>
                                                                                                        <asp:CheckBox runat="server" ID="chkRelCat" OnCheckedChanged="chkRelCat_CheckedChanged" AutoPostBack="true" />
                                                                                                        <asp:HiddenField runat="server" ID="hdnRelCatProductId" Value='<%# Eval("ProductID") %>' />
                                                                                                        <asp:HiddenField runat="server" ID="hdnRelCatMaintainStock" Value='<%# Eval("MaintainStock") %>' />
                                                                                                        <asp:HiddenField runat="server" ID="hdnRelCatUnitId" Value='<%# Eval("UnitId") %>' />
                                                                                                        <asp:HiddenField runat="server" ID="hdnRelCatQuantity" Value='<%# Eval("Quantity") %>' />
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>

                                                                                                <asp:TemplateField HeaderText="Category Name">
                                                                                                    <ItemTemplate>
                                                                                                        <%# Eval("Category_name") %>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Product Name">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label runat="server" ID="gvLblRelCatProductName" Text='<%# Eval("EProductName") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Product Code">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label runat="server" ID="gvLblRelCatProductcode" Text='<%# Eval("productcode") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Sales Price 1">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label runat="server" ID="gvLblRelCatPrice" Text='<%# Eval("salesprice1") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Available Qty">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox runat="server" Text='<%# Eval("Quantity") %>' Width="30px"></asp:TextBox>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Required Qty">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox runat="server" ID="gvTxtRelCatQuantity" Text="1" Width="30px"></asp:TextBox>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                            </Columns>
                                                                                        </asp:GridView>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <div class="col-md-6">
                                                                                        <asp:Button class="btn btn-box-tool" Text="Show Model Wise Related Product" runat="server" ID="Button1" OnCommand="btnRPM_Click" Visible='<%# Eval("Model_no").ToString()==""?false:true %>' CommandArgument='<%# Eval("Model_no") %>'></asp:Button>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-6">
                                                                                        <asp:Button class="btn btn-box-tool" Text="Hide Model Products" runat="server" ID="btnHideRPM" OnCommand="btnRPM_Click" Visible='<%# Eval("Model_no").ToString()==""?false:true %>' CommandArgument='<%# Eval("Model_no") %>'></asp:Button>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div style="overflow: auto; max-height: 300px; min-height: 0px" class="col-md-12">
                                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" runat="server" ID="gvRelMod" AutoGenerateColumns="false">
                                                                                            <Columns>
                                                                                                <asp:TemplateField>
                                                                                                    <ItemTemplate>
                                                                                                        <asp:CheckBox runat="server" ID="chkRelMod" OnCheckedChanged="chkRelMod_CheckedChanged" AutoPostBack="true" />
                                                                                                        <asp:HiddenField runat="server" ID="hdnRelModProductId" Value='<%# Eval("ProductID") %>' />
                                                                                                        <asp:HiddenField runat="server" ID="hdnRelModMaintainStock" Value='<%# Eval("MaintainStock") %>' />
                                                                                                        <asp:HiddenField runat="server" ID="hdnRelModUnitId" Value='<%# Eval("UnitId") %>' />
                                                                                                        <asp:HiddenField runat="server" ID="hdnRelModQuantity" Value='<%# Eval("Quantity") %>' />
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Model No">
                                                                                                    <ItemTemplate>
                                                                                                        <%# Eval("Model_no") %>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Model Name">
                                                                                                    <ItemTemplate>
                                                                                                        <%# Eval("Model_name") %>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Product Name">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label runat="server" ID="gvLblRelModProductName" Text='<%# Eval("EProductName") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Product Code">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label runat="server" ID="gvLblRelModProductcode" Text='<%# Eval("productcode") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Sales Price 1">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label runat="server" ID="gvLblRelModPrice" Text='<%# Eval("ProductSalesPrice") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Available Qty">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label runat="server" Text='<%# Eval("Quantity") %>' Width="30px"></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Required Qty">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox runat="server" ID="gvTxtRelModQuantity" Text="1" Width="30px"></asp:TextBox>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                            </Columns>
                                                                                        </asp:GridView>
                                                                                    </div>
                                                                                </div>

                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <br />
                                                                    <br />
                                                                </ItemTemplate>
                                                            </asp:DataList>
                                                            <asp:HiddenField ID="HDFSort" runat="server" />
                                                            <asp:HiddenField ID="hdnProductId" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <%--</div>--%>
                                    </div>
                                    <div class="col-md-12">
                                        <asp:Button ID="btnBack" runat="server" Text="Back To Main List" Visible="false" CssClass="btn btn-primary" OnClick="btnBack_Click" />
                                        <br />
                                        <br />
                                        <asp:Panel ID="pnlChkCategory" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                            BorderColor="#abadb3" BackColor="White" Visible="false" ScrollBars="Auto">
                                            <asp:CheckBoxList ID="chkCategory" runat="server" RepeatColumns="4" RepeatDirection="Horizontal" Width="100%"
                                                AutoPostBack="true" OnSelectedIndexChanged="chkCategory_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <br />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                                <div class="modal-dialog modal-lg">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <button type="button" class="close" data-dismiss="modal">
                                                <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                                            <h4 class="modal-title" id="myModalLabel">
                                                <asp:Label ID="lblProductHeader" runat="server" Font-Size="14px" Font-Bold="true" Text="<%$ Resources:Attendance,Location Stock %>"></asp:Label></h4>
                                        </div>
                                        <div class="modal-body">
                                            <asp:UpdatePanel ID="update_Modal" runat="server">
                                                <ContentTemplate>
                                                    <div class="box box-warning box-solid">
                                                        <div class="box-header with-border">
                                                            <h3 class="box-title"></h3>
                                                        </div>
                                                        <div class="box-body">
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <div class="flow">
                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvStockInfo" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                            AllowPaging="True" AllowSorting="True">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Location Name %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblLocation" runat="server" Text='<%# Eval("Location_Name") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Present  Quantity">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblPurQty" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle />
                                                                                </asp:TemplateField>
                                                                            </Columns>




                                                                        </asp:GridView>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="modal-footer">
                                            <asp:UpdatePanel ID="Update_Modal_Button" runat="server">
                                                <ContentTemplate>
                                                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="modal fade" id="Product_StockAnalysis" tabindex="-1" role="dialog" aria-labelledby="Product_ModalLabel" aria-hidden="true">
                                <div class="modal-dialog modal-lg">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <button type="button" class="close" data-dismiss="modal">
                                                <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                                            <h4 class="modal-title" id="Product_StockAnalysis1">
                                                <asp:Label ID="Label32" runat="server" Text="<%$ Resources:Attendance,Location Stock %>"></asp:Label></h4>
                                        </div>
                                        <div class="modal-body">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <SA:StockAnalysis runat="server" ID="modelSA" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </section>
            </div>
            <footer class="main-footer" style="margin-left: 0px;">
                <asp:Literal ID="Ltr_Footer_Content" runat="server"></asp:Literal>
            </footer>

            <div class="control-sidebar-bg"></div>
            <asp:Panel ID="pnlStock1" runat="server" Visible="false"></asp:Panel>
            <asp:Panel ID="pnlStock2" runat="server" Visible="false"></asp:Panel>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_List">
                <ProgressTemplate>
                    <div class="modal_Progress">
                        <div class="center_Progress">
                            <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </form>
    <script src="../Bootstrap_Files/plugins/jQuery/jquery-2.2.3.min.js"></script>
    <script src="../Bootstrap_Files/plugins/jQuery/jquery-2.2.3.min.js"></script>
    <script src="../Bootstrap_Files/bootstrap/js/bootstrap.min.js"></script>
    <script src="../Bootstrap_Files/dist/js/app.min.js"></script>
    <script src="../Bootstrap_Files/plugins/slimScroll/jquery.slimscroll.min.js"></script>
    <script>


        $(document).ready(function (On_Small) {
            if ($('#Body_1').attr('class') == 'skin-blue sidebar-mini fixed sidebar-collapse') {

                $('#Product_Div').hide();
            }
            if ($('#Body_1').attr('class') == 'skin-blue sidebar-mini fixed') {
                $('#Product_Div').show();

                $('a#Btn_Menu').click(function () {
                    $('#Product_Div').toggle(400);
                });

                //$('aside#Aside_Menu').mouseenter(function () {
                //    $('#Product_Div').toggle(400);
                //});

                //$('aside#Aside_Menu').mouseout(function () {
                //    $('#Product_Div').toggle(400);
                //});
            }
            if ($('#Body_1').attr('class') == 'skin-blue sidebar-mini fixed sidebar-expanded-on-hover') {
                $('#Product_Div').show();
            }
        });
        function Press_Enter_Key() {
            $(".txtquantity").keypress(function (e) {
                if (e.which == 13) {
                    e.preventDefault();
                    return false;
                }
            });
        }
        function lnkStockInfo_Command(productID) {
            window.open('../Inventory/Magic_Stock_Analysis.aspx?ProductId=' + productID + '&&Type=&&Contact=', '_blank', 'width=1024, ');
        }
        function loadRelatedProducts(row, productid) {
            PageMethods.relatedProductData(productid, function (data) {
                debugger;
                var rowData = JSON.parse(data);
                $(rowData).each(function () {
                    debugger;
                    var dd = $(this)[0];
                    var table = "<tr>";
                    table += "<td></td>";
                    table += "<td> " + dd.ProductCode + " </td>";
                    table += "<td> " + dd.EProductName + " </td>";
                    table += "<td> " + dd.unit_name + " </td>";
                    table += "<td> " + dd.salesprice1 + " </td>";
                    table += "<td> " + dd.Quantity + " </td>";
                    table += "<td> " + dd.Category_Name + " </td></tr>";
                    $('#tblRelated' + row + ' > tbody:last-child').append(table);
                });


            }, function (data) { alert(data); });
        }
        function showCalendar(sender, args) {

            var ctlName = sender._textbox._element.name;

            ctlName = ctlName.replace('$', '_');
            ctlName = ctlName.replace('$', '_');

            var processingControl = $get(ctlName);
            //var targetCtlHeight = processingControl.clientHeight;
            sender._popupDiv.parentElement.style.top = processingControl.offsetTop + processingControl.clientHeight + 'px';
            sender._popupDiv.parentElement.style.left = processingControl.offsetLeft + 'px';

            var positionTop = processingControl.clientHeight + processingControl.offsetTop;
            var positionLeft = processingControl.offsetLeft;
            var processingParent;
            var continueLoop = false;

            do {
                // If the control has parents continue loop.
                if (processingControl.offsetParent != null) {
                    processingParent = processingControl.offsetParent;
                    positionTop += processingParent.offsetTop;
                    positionLeft += processingParent.offsetLeft;
                    processingControl = processingParent;
                    continueLoop = true;
                }
                else {
                    continueLoop = false;
                }
            } while (continueLoop);

            sender._popupDiv.parentElement.style.top = positionTop + 2 + 'px';
            sender._popupDiv.parentElement.style.left = positionLeft + 'px';
            sender._popupBehavior._element.style.zIndex = 10005;
        }
    </script>
</body>
</html>
