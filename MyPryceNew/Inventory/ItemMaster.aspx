<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" EnableEventValidation="true" AutoEventWireup="true" CodeFile="ItemMaster.aspx.cs" Inherits="Inventory_ItemMaster" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/FileUpload.ascx" TagPrefix="AT1" TagName="FileUpload1" %>
<%@ Register Src="~/WebUserControl/FileManager.ascx" TagName="FileManager" TagPrefix="UC" %>
<%@ Register Src="~/WebUserControl/ucControlsSetting.ascx" TagName="ucCtlSetting" TagPrefix="UC" %>
<%@ Register Src="~/WebUserControl/StockAnalysis.ascx" TagName="StockAnalysis" TagPrefix="SA" %>
<%@ Register Src="~/WebUserControl/ReportSystem.ascx" TagName="ReportSystem" TagPrefix="RS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <script src="http://localhost:8081/PryceTrial/Script/common.js"></script>
    <style type="text/css">
        .page_enabled, .page_disabled {
            text-align: center;
            font-size: larger;
            text-decoration: none;
            background: #01a9ff;
            border: 1px solid #90BDE9;
        }

        .page_enabled {
            background-color: #90BDE9;
            color: #000;
        }

        .page_disabled {
            background-color: #01A9FF;
            color: #fff !important;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-box"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Product Setup %>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Product%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:Button ID="Btn_Upload" Style="display: none;" runat="server" Text="Upload" />
            <asp:Button ID="Btn_Verify_Request" Style="display: none;" runat="server" OnClick="btnVerify_Click" Text="Verify Request" />
            <asp:Button ID="Btn_View_Address_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Address_Modal" Text="Address Modal" />
            <asp:Button ID="Btn_View_Product_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Product_Modal" Text="Product Modal" />
            <asp:Button ID="Btn_Modal_FileUpload" Style="display: none;" runat="server" data-toggle="modal" data-target="#Fileupload123" Text="FileUpload" />

            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
            <asp:HiddenField runat="server" ID="hdnCanPrint" />
            <asp:HiddenField runat="server" ID="hdnCanUpload" />
            <asp:HiddenField runat="server" ID="hdnCanView" />
            <asp:HiddenField runat="server" ID="hdnCanRestore" />

        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgress11" runat="server" AssociatedUpdatePanelID="Update_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">
                <ul class="nav nav-tabs pull-right bg-blue-gradient">
                    <li id="Li_Reorder"><a href="#Reorder" data-toggle="tab">
                        <i class="fas fa-file-invoice"></i>&nbsp;&nbsp;
                        <asp:Label ID="Label25" runat="server" Text="<%$ Resources:Attendance,Reorder%>"></asp:Label></a></li>
                    <li id="Li_Verify" runat="server" visible="false"><a href="#Verify" onclick="Li_Tab_Verify()" data-toggle="tab">
                        <i class="fas fa-user-check"></i>&nbsp;&nbsp;<asp:Label ID="Label24" runat="server" Text="<%$ Resources:Attendance,Verify Request %>"></asp:Label></a></li>
                    <li id="Li_Bin"><a href="#Bin" onclick="Li_Tab_Bin()" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>

                    <li id="Li_Upload"><a href="#Upload" onclick="Li_Tab_Upload()" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label33" runat="server" Text="<%$ Resources:Attendance,Upload %>"></asp:Label></a></li>

                    <li id="Li_New"><a onclick="Li_Tab_New()" href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List" class="active"><a href="#List" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a>
                    </li>
                    <%-- <li id="Li_Stock"><a href="#Stock" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label35" runat="server" Text="Stock"></asp:Label></a>
                    </li>--%>
                </ul>
                <div class="tab-content">

                    <%--<div class="tab-pane" id="Stock" style="display: none;">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div6" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label36" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				<asp:Label ID="lblTotalRecordsStock" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>" />
                                                <asp:HiddenField ID="HiddenField3" runat="server" Value="" />
                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I5" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>


                                            <div class="box-body">
                                                <div class="col-md-12">
                                                    <asp:Label ID="lblSearchLocation" runat="server" Text="<%$ Resources:Attendance,Location %>" />
                                                    <asp:DropDownList ID="ddlSearchLocation" runat="server" CssClass="form-control"></asp:DropDownList>
                                                    <br />
                                                </div>
                                                <div class="col-md-6" id="Div51" runat="server" defaultbutton="btnStockBind">
                                                    <asp:TextBox ID="txtSearchProduct" runat="server" CssClass="form-control" BackColor="#eeeeee"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender12" runat="server" CompletionInterval="100"
                                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductName"
                                                        ServicePath="" TargetControlID="txtSearchProduct" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                    </cc1:AutoCompleteExtender>

                                                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender6" runat="server" WatermarkText="Search Product"
                                                        TargetControlID="txtSearchProduct">
                                                    </cc1:TextBoxWatermarkExtender>
                                                    <br />
                                                </div>
                                                <div class="col-md-6" id="Div50" runat="server" defaultbutton="btnStockBind">
                                                    <asp:TextBox ID="txtSearchModelNo" runat="server" BackColor="#eeeeee" CssClass="form-control"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender13" runat="server" CompletionInterval="100"
                                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListtxtModelName"
                                                        ServicePath="" TargetControlID="txtSearchModelNo" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                    </cc1:AutoCompleteExtender>
                                                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender7" runat="server" WatermarkText="Search Model No"
                                                        TargetControlID="txtSearchModelNo">
                                                    </cc1:TextBoxWatermarkExtender>
                                                    <br />
                                                </div>
                                                <div class="col-md-6" id="Div16" runat="server" defaultbutton="btnStockBind">
                                                    <asp:TextBox ID="txtSearchColor" runat="server" BackColor="#eeeeee" CssClass="form-control"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender14" runat="server" CompletionInterval="100"
                                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListtxtColour"
                                                        ServicePath="" TargetControlID="txtSearchColor" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                    </cc1:AutoCompleteExtender>
                                                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender8" runat="server" WatermarkText="Search Color"
                                                        TargetControlID="txtSearchColor">
                                                    </cc1:TextBoxWatermarkExtender>
                                                    <br />
                                                </div>
                                                <div class="col-md-6" id="Div5" runat="server" defaultbutton="btnStockBind">
                                                    <asp:TextBox ID="txtSearchSize" runat="server" BackColor="#eeeeee" CssClass="form-control"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender15" runat="server" CompletionInterval="100"
                                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListtxtSize"
                                                        ServicePath="" TargetControlID="txtSearchSize" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                    </cc1:AutoCompleteExtender>
                                                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender5" runat="server" WatermarkText="Search Size"
                                                        TargetControlID="txtSearchSize">
                                                    </cc1:TextBoxWatermarkExtender>
                                                    <br />
                                                </div>

                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="btnStockBind" runat="server" OnClick="btnStockBind_Click" CausesValidation="False" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnStockReferesh" OnClick="btnStockReferesh_Click" runat="server" CausesValidation="False" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid">
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvStockProduct" runat="server" AllowPaging="true"
                                                        AutoGenerateColumns="False" AllowSorting="true"
                                                        OnPageIndexChanging="GvStcokProduct_PageIndexChanging" OnSorting="GvStockProduct_OnSorting" Width="100%"
                                                        DataKeyNames="ProductId" PageSize="<%# PageControlCommon.GetPageSize() %>" >
                                                        <Columns>
                                                             <asp:TemplateField HeaderText="Location" SortExpression="LocationName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvLocation" runat="server" Text='<%# Eval("LocationName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id %>" SortExpression="ProductCode">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductId" runat="server" Text='<%# Eval("ProductCode") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="EProductName" HeaderText="<%$ Resources:Attendance,Product Name %>"
                                                                SortExpression="EProductName" ItemStyle-Width="40%" />
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Model No %>" SortExpression="ModelNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("ModelNo") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblModelCode" runat="server" Text='<%# Eval("ModelNo") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="Color" SortExpression="ColorName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvColorName" runat="server" Text='<%# Eval("ColorName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Size" SortExpression="SizeName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvSizeName" runat="server" Text='<%# Eval("SizeName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Item Type %>" SortExpression="ItemTypeValue">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblItemType" runat="server" Text='<%# Eval("ItemTypeValue") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Stock %>" SortExpression="StockQty">
                                                                <ItemTemplate>
                                                                    
                                                                    <asp:LinkButton ID="lnkStockInfo" runat="server" Text='<%# Eval("StockQty") %>' Font-Underline="true" ForeColor="Blue" ToolTip="View Detail" OnCommand="lnkStockInfo_Command" OnClientClick="$('#Product_StockAnalysis').modal('show');" CommandArgument='<%# Eval("ProductId") %>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Product Unit %>" SortExpression="UnitName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPUnit" runat="server" Text='<%#  Eval("UnitName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Sales Price%>" SortExpression="ProductSalesPrice">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbsalesPrice" runat="server" Text='<%# GetSalesPriceinLocal(Eval("ProductSalesPrice").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="CreatedEmpName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCreatedBy" runat="server" Text='<%# Eval("CreatedEmpName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>" SortExpression="ModifiedEmpName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModifiedBy" runat="server" Text='<%# Eval("ModifiedEmpName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                    <br />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- /.box-body -->
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>--%>




                    <div class="tab-pane active" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnExport" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div1" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label28" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				<asp:Label ID="lblTotalRecord" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>" />
                                                <asp:HiddenField ID="hdnReportLocation" runat="server" Value="" />
                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblBrandsearch" runat="server" Text="<%$ Resources:Attendance,Manufacturing Brand %>"></asp:Label>
                                                    <asp:DropDownList ID="ddlbrandsearch" runat="server" CssClass="form-control"></asp:DropDownList>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lbllocationsearch" runat="server" Text="<%$ Resources:Attendance,Category %>" />
                                                    <asp:DropDownList ID="ddlcategorysearch" runat="server" CssClass="form-control"></asp:DropDownList>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:CheckBox ID="chkDiscontinue" CssClass="form-control" runat="server" Text="Discontinue Product" />
                                                    <br />
                                                </div>
                                                <div class="col-md-6" id="pnladvancseserach" runat="server" defaultbutton="btnsearchProduct">
                                                    <asp:TextBox ID="txtSearchPrduct" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" WatermarkText="Search Product" TargetControlID="txtSearchPrduct"></cc1:TextBoxWatermarkExtender>
                                                    <asp:Button ID="btnsearchProduct" runat="server" Visible="false" />
                                                    <br />
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlFieldName_OnSelectedIndexChanged">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Product Id %>" Value="ProductCode"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Product Name %>" Value="EProductName"
                                                            Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Model No. %>" Value="Inv_ModelMaster.Model_No"></asp:ListItem>
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
                                                <div class="col-lg-4">
                                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                        <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" />
                                                        <asp:DropDownList ID="ddlitemtypeserach" runat="server" CssClass="form-control"
                                                            Visible="false">
                                                            <asp:ListItem Selected="True" Text="<%$ Resources:Attendance, --Select--%>" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Stockable" Value="S"></asp:ListItem>
                                                            <asp:ListItem Text="NonStockable" Value="NS"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance, Kit %>" Value="K"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imbBtnGrid" CausesValidation="False" runat="server" OnClick="imbBtnGrid_Click" ToolTip="<%$ Resources:Attendance, Grid View %>"><span class="fa fa-table"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imgBtnDatalist" CausesValidation="False" runat="server" OnClick="imgBtnDatalist_Click" ToolTip="<%$ Resources:Attendance,List View %>" Visible="False"><span class="fa fa-list"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnGvProductSetting" ToolTip="Product List Setting" runat="server" OnClick="btnGvProductSetting_Click"><span class="fa fa-wrench"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnExport" runat="server" CausesValidation="False" ToolTip="<%$ Resources:Attendance,Export To Excel%>" OnClick="btnExport_Click" Visible="false"><span class="fas fa-file-export"  style="font-size:25px;"></span></asp:LinkButton>

                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:CheckBox ID="chkAvailableStock" runat="server" Text="Stock Available" />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Button ID="btnGetEcomData" runat="server" CssClass="btn btn-primary" OnClick="btnGetEcomData_Click" Text="Get All Items from ECommerece" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid">
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvProduct" runat="server" AllowPaging="false" AutoGenerateColumns="False" AllowSorting="true"
                                                        OnPageIndexChanging="gvProduct_PageIndexChanging" OnSorting="gvProduct_OnSorting" Width="100%"
                                                        DataKeyNames="ProductId" PageSize="<%# PageControlCommon.GetPageSize() %>" Visible="false">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">

                                                                            <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>

                                                                                <a onclick="getReportDataWithLoc('<%# Eval("ProductId") %>', '<%# hdnReportLocation.Value    %>' )"><i class="fa fa-print" style="cursor: pointer"></i>Report System</a>
                                                                            </li>

                                                                            <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnView" runat="server" CommandArgument='<%# Eval("ProductId") %>' CausesValidation="False" OnCommand="btnView_Command"><i class="fa fa-eye"></i>View</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="False" CommandArgument='<%# Eval("ProductId") %>' OnCommand="btnEdit_Command"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanUpload.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnFileUpload" runat="server" CommandArgument='<%# Eval("ProductId") %>' CommandName='<%# Eval("ProductCode") %>' OnCommand="btnFileUpload_Command" CausesValidation="False"><i class="fa fa-upload"></i>File Upload</asp:LinkButton>
                                                                            </li>
                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id %>" SortExpression="ProductCode">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductId" runat="server" Text='<%# Eval("ProductCode") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="EProductName" HeaderText="<%$ Resources:Attendance,Product Name %>"
                                                                SortExpression="EProductName" ItemStyle-Width="40%" />
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Model No %>" SortExpression="ModelNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("ModelNo") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblModelCode" runat="server" Text='<%# Eval("ModelNo") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Item Type %>" SortExpression="ItemTypeValue">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblItemType" runat="server" Text='<%# Eval("ItemTypeValue") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Stock %>" SortExpression="StockQty">
                                                                <ItemTemplate>
                                                                    <%-- <a onclick="lnkStockInfo_Command('<%# Eval("ProductId") %>')"><%# Eval("StockQty") %></a>--%>
                                                                    <asp:LinkButton ID="lnkStockInfo" runat="server" Text='<%# Eval("StockQty") %>' Font-Underline="true" ForeColor="Blue" ToolTip="View Detail" OnCommand="lnkStockInfo_Command" OnClientClick="$('#Product_StockAnalysis').modal('show');" CommandArgument='<%# Eval("ProductId") %>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Product Unit %>" SortExpression="UnitName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPUnit" runat="server" Text='<%#  Eval("UnitName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Sales Price%>" SortExpression="ProductSalesPrice">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbsalesPrice" runat="server" Text='<%# GetSalesPriceinLocal(Eval("ProductSalesPrice").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="CreatedEmpName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCreatedBy" runat="server" Text='<%# Eval("CreatedEmpName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>" SortExpression="ModifiedEmpName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModifiedBy" runat="server" Text='<%# Eval("ModifiedEmpName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                    <br />
                                                </div>
                                            </div>
                                            <asp:Label ID="lbldltModelNo" runat="server" Text='<%# Eval("ModelNo") %>' Visible="false"></asp:Label>
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:DataList ID="dtlistProduct" Style="width: 100%" runat="server" RepeatColumns="2" RepeatDirection="Horizontal">
                                                        <ItemTemplate>
                                                            <div class="col-md-6" style="min-width: 500px;">
                                                                <div class="box box-primary ">
                                                                    <div class="box-header with-border" style="height: 60px">
                                                                        <asp:LinkButton ID="lbldlProductName" runat="server" Font-Bold="true" ForeColor="#1886b9" OnCommand="btnEdit_Command"
                                                                            CommandArgument='<%# Eval("ProductId") %>' Text='<%# Eval("ShortProductName") %>'
                                                                            Enabled='<%# hdnCanEdit.Value=="true"?true:false %>'></asp:LinkButton>
                                                                    </div>
                                                                    <div class="box-footer no-padding" style="background-color: ghostwhite">
                                                                        <div class="col-md-12">
                                                                            <div class="col-md-4">
                                                                                <br />
                                                                                <asp:ImageButton ID="btnImgProduct" runat="server" OnCommand="btnEdit_Command" CommandArgument='<%# Eval("ProductId") %>'
                                                                                    Width="120px" Height="120px" ImageUrl='<%# string.IsNullOrEmpty(Eval("PImage").ToString()) ? "~/Login/Images/place-holder.png" : "~/CompanyResource/"+GetRagistrationCode()+"/" + System.Web.HttpContext.Current.Session["CompId"].ToString() + "/Product/"+ Eval("PImage").ToString() %>'
                                                                                    Enabled="False" />
                                                                            </div>
                                                                            <div class="col-md-8">
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblProductId" Width="90px" runat="server" Text="<%$ Resources:Attendance,Product Id %>"></asp:Label>
                                                                                        </td>
                                                                                        <td width="5px">:
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lbldlProductId" runat="server" Text='<%# Eval("ProductCode") %>'></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblModelNo" runat="server" Text="<%$ Resources:Attendance,Model No. %>"></asp:Label>
                                                                                        </td>
                                                                                        <td>:
                                                                                        </td>
                                                                                        <td>
                                                                                            <a onclick="lnkModel1Info_Command('<%# Eval("ModelNo") %>')" style="cursor: pointer; color: blue">
                                                                                                <%# Eval("ModelNo") %>
                                                                                            </a>
                                                                                            <%--<asp:LinkButton ID="lnkModel1Info" runat="server" ForeColor="Blue" Text='<%# Eval("ModelNo") %>'
                                                                                                OnCommand="lnkModel1Info_Command" CommandName='<%# Eval("ModelNo") %>'
                                                                                                ToolTip="Model History" />--%>
                                                                                            <asp:Label ID="lbldltModelNo" runat="server" Text='<%# Eval("ModelNo") %>' Visible="false"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblItypeType" runat="server" Text="<%$ Resources:Attendance,Stock%>"></asp:Label>
                                                                                        </td>
                                                                                        <td>:
                                                                                        </td>
                                                                                        <td>
                                                                                            <%--<a onclick="lnkStockInfo_Command('<%# Eval("ProductId") %>')" style="cursor: pointer; color: blue"><%# Eval("StockQty") %></a>--%>

                                                                                            <asp:LinkButton ID="lnkStockInfo" runat="server" Text='<%# Eval("StockQty") %>' Font-Underline="true" ToolTip="View Detail" OnCommand="lnkStockInfo_Command" OnClientClick="$('#Product_StockAnalysis').modal('show');" CommandArgument='<%# Eval("ProductId") %>'></asp:LinkButton>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblCostPrice" runat="server" Text="<%$ Resources:Attendance,Sales Price %>"></asp:Label>
                                                                                        </td>
                                                                                        <td>:
                                                                                        </td>
                                                                                        <td>
                                                                                            <%--<asp:Label ID="lbldlCostPrice" runat="server" Text='<%# GetSalesPriceinLocal(Eval("ProductSalesPrice").ToString()) %>'></asp:Label>--%>
                                                                                            <asp:Label ID="lbldlCostPrice" runat="server" Text='<%# GetSalesPriceUsingID(Eval("ProductId").ToString()) %>'></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblProductUnit" runat="server" Text="<%$ Resources:Attendance,Unit %>"></asp:Label>
                                                                                        </td>
                                                                                        <td>:
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("UnitName") %>'></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Created By %>"></asp:Label>
                                                                                        </td>
                                                                                        <td>:
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="Label9" runat="server" Text='<%# Eval("CreatedEmpName") %>'></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Modified By %>"></asp:Label>
                                                                                        </td>
                                                                                        <td>:
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="Label11" runat="server" Text='<%# Eval("ModifiedEmpName") %>'></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:DataList>
                                                    <asp:HiddenField ID="HDFSort" runat="server" />

                                                </div>
                                                <div style="text-align: center">
                                                    <asp:Repeater ID="rptPager" runat="server">
                                                        <ItemTemplate>
                                                            <ul class="pagination">
                                                                <li class='<%# Convert.ToBoolean(Eval("Enabled")) ? "page-item" : "page-item active" %>'>
                                                                    <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>' CssClass="page-link" OnClick="Page_Changed" OnClientClick='<%# !Convert.ToBoolean(Eval("Enabled")) ? "return false;" : "" %>'></asp:LinkButton>
                                                                </li>
                                                            </ul>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                    <asp:HiddenField ID="hdnProductId" runat="server" />
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                    <!-- /.box-body -->
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>



                    <div class="modal fade" id="Fileupload123" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
                        aria-hidden="true">
                        <div class="modal-dialog modal-lg">
                            <div class="modal-content">
                                <div class="modal-body">
                                    <AT1:FileUpload1 runat="server" ID="FUpload1" />
                                </div>
                                <div class="modal-footer">
                                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                                        Close</button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <dx:ASPxPopupControl ID="ASPxPopupControl1" ClientInstanceName="popup" runat="server" Height="500px" Width="1000px" Modal="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                        <ClientSideEvents Shown="function(s,e){
                    fileManager.AdjustControl();
                }" />

                        <ContentCollection>
                            <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">

                                <dx:ASPxFileManager ID="ASPxFileManager1" runat="server" ClientInstanceName="fileManager">
                                    <Settings AllowedFileExtensions=".jpg,.jpeg,.gif,.rtf,.txt,.avi,.png,.mp3,.xml,.doc,.pdf" EnableMultiSelect="false" />
                                    <SettingsEditing AllowCreate="true" AllowDelete="true" AllowMove="true" AllowRename="true" AllowCopy="true" AllowDownload="true" />
                                    <SettingsToolbar ShowDownloadButton="true" />
                                    <SettingsFileList View="Thumbnails">
                                        <ThumbnailsViewSettings ThumbnailSize="100" />
                                        <DetailsViewSettings AllowColumnResize="true" AllowColumnDragDrop="true" AllowColumnSort="true" ShowHeaderFilterButton="false">
                                            <Columns>
                                                <dx:FileManagerDetailsColumn FileInfoType="Thumbnail" />
                                                <dx:FileManagerDetailsColumn FileInfoType="FileName" />
                                                <dx:FileManagerDetailsColumn FileInfoType="LastWriteTime" />
                                                <dx:FileManagerDetailsColumn FileInfoType="Size" />
                                            </Columns>
                                        </DetailsViewSettings>
                                    </SettingsFileList>
                                    <SettingsUpload Enabled="true" />
                                    <ClientSideEvents SelectedFileOpened="function(s, e) {
                                            var image =  $('#imgProduct')[0];
                                            image.src =e.file.imageSrc;                                             
                                            var subproduct = s.currentPath;
                                            if(subproduct!='')
                                            {
                                                subproduct ='/'+subproduct; 
                                                subproduct = subproduct.replaceAll('\\','/');
                                            }                                            
                                            var dataa = '~/'+s.rootFolderName+subproduct +'/'+ e.file.name ;
                                            dataa = dataa.replaceAll('/', '//');
                                            PageMethods.ASPxFileManager1_SelectedFileOpened(dataa,e.file.name,function(data){ },function(data){}); 
                                            popup.Hide();return false; }" />
                                </dx:ASPxFileManager>

                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>



                    <div class="tab-pane" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12" align="right">
                                                        <asp:LinkButton ID="btnControlsSetting" ToolTip="Product List Setting" runat="server" OnClick="btnControlsSetting_Click" Visible="false"><i class="fa fa-wrench"></i></asp:LinkButton>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblProductId" runat="server" Text="<%$ Resources:Attendance,Product Id %>"></asp:Label>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtProductId" TabIndex="1" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                    OnTextChanged="txtProductId_TextChanged" BackColor="#eeeeee"></asp:TextBox>
                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" CompletionInterval="100"
                                                                    DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductId"
                                                                    ServicePath="" TargetControlID="txtProductId" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                </cc1:AutoCompleteExtender>

                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" Enabled="True"
                                                                    TargetControlID="txtProductId" FilterType="Custom" ValidChars="1234567890abcdefghijklmnopqrstuvwxyz_@#()- ABCDEFGHIJKLMNOPQRSTUVWXYZ,/&^*$!~.">
                                                                </cc1:FilteredTextBoxExtender>
                                                                <div class="input-group-btn" style="border: solid 1px #eeeeee">
                                                                    <asp:LinkButton ID="lnkAddExp" Style="margin-left: 5px;" runat="server"><i class="fa fa-search" style="font-size:25px"></i></asp:LinkButton>
                                                                </div>
                                                            </div>


                                                            <asp:HiddenField ID="hdnModelId" runat="server" />
                                                            <br />
                                                            <asp:Label ID="lblModelNo" runat="server" Text="<%$ Resources:Attendance,Model No. %>"></asp:Label>
                                                            <asp:TextBox ID="txtModelNo" TabIndex="2" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                OnTextChanged="txtModelNo_TextChanged"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" CompletionInterval="100"
                                                                DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListtxtModelNo"
                                                                ServicePath="" TargetControlID="txtModelNo" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                            <asp:LinkButton ID="lnkModelInfo" runat="server" Text="Model Product List" ForeColor="Blue" OnCommand="lnkModelInfo_Command" ToolTip="Model History" />
                                                            <br />
                                                            <br />
                                                        </div>

                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblPImage" runat="server" Text="<%$ Resources:Attendance,Product Image %>"></asp:Label>
                                                            <div style="text-align: center;">

                                                                <div class="input-group" style="width: 100%; display: none" style="text-align: center;">
                                                                    <cc1:AsyncFileUpload ID="fugProduct" OnUploadedComplete="FuLogo_FileUploadComplete" OnClientUploadError="FuLogoUploadError" OnClientUploadStarted="FuLogouploadStarted" OnClientUploadComplete="FuLogouploadComplete"
                                                                        runat="server" CssClass="form-control btn btn-default" CompleteBackColor="White" UploaderStyle="Traditional" UploadingBackColor="#CCFFFF" ThrobberID="imgLoader" Width="100%" />
                                                                    <asp:Label ID="lblImgMessageShow" runat="server" ForeColor="Red" Text=""></asp:Label>
                                                                    <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">

                                                                        <asp:LinkButton ID="Img_Right" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-check" style="font-size:30px;color:#22cb33"></i></asp:LinkButton>
                                                                        <asp:LinkButton ID="Img_Wrong" runat="server" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-remove" style="font-size:30px"></i></asp:LinkButton>
                                                                        <asp:Image ID="imgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                                    </div>
                                                                </div>
                                                                <br />
                                                                <asp:Image ID="imgProduct" ClientIDMode="Static" ImageUrl="../Bootstrap_Files/dist/img/NoImage.jpg" Width="120px" Height="120px" runat="server" />
                                                                <br />
                                                                <br />
                                                                <asp:Button ID="btnloadimg" Visible="false" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Load %>"
                                                                    OnClick="btnloadimg_Click" />

                                                                <a onclick="popup.Show();" runat="server" id="File_Manager_Product" class="btn btn-primary" style="cursor: pointer">File Manager</a>

                                                                <dx:ASPxButton ID="dxBtnFileUpload" runat="server" Text="File Manager" Visible="false" CssClass="btn btn-primary" AutoPostBack="false">
                                                                    <ClientSideEvents Click="function(s, e) { popup.Show(); }" />
                                                                </dx:ASPxButton>

                                                                <cc1:HoverMenuExtender ID="hme3" runat="Server" TargetControlID="lnkAddExp" PopupControlID="pnldescdetail"
                                                                    HoverCssClass="popupHover" PopupPosition="Right" OffsetX="0" OffsetY="0" PopDelay="50" />
                                                            </div>

                                                            <br />
                                                        </div>







                                                        <div class="col-md-12">
                                                            <asp:Label ID="lblModelName" runat="server" Text="<%$ Resources:Attendance,Model Name %>"></asp:Label>
                                                            <asp:TextBox ID="txtModelName" TabIndex="3" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                OnTextChanged="txtModelNo_TextChanged"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" CompletionInterval="100"
                                                                DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListtxtModelName"
                                                                ServicePath="" TargetControlID="txtModelName" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6" id="ctlColour" runat="server">
                                                            <asp:HiddenField ID="hdnColourId" runat="server" />
                                                            <asp:Label ID="lblColour" runat="server" Text="Colour"></asp:Label>
                                                            <asp:TextBox ID="txtColour" TabIndex="3" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                OnTextChanged="txtColour_TextChanged"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender10" runat="server" CompletionInterval="100"
                                                                DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListtxtColour"
                                                                ServicePath="" TargetControlID="txtColour" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6" id="ctlSize" runat="server">
                                                            <asp:HiddenField ID="hdnSizeId" runat="server" />
                                                            <asp:Label ID="lblSize" runat="server" Text="Size"></asp:Label>
                                                            <asp:TextBox ID="txtSize" TabIndex="3" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                OnTextChanged="txtSize_TextChanged"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender11" runat="server" CompletionInterval="100"
                                                                DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListtxtSize"
                                                                ServicePath="" TargetControlID="txtSize" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6" id="pnldescdetail" runat="server">
                                                            <asp:RadioButtonList ID="rdoProductid" runat="server" RepeatDirection="Vertical">
                                                            </asp:RadioButtonList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12">
                                                            <asp:Label ID="lblEProductName" runat="server" Text="<%$ Resources:Attendance,Product Name %>"></asp:Label>
                                                            <asp:TextBox ID="txtEProductName" TabIndex="4" runat="server" CssClass="form-control" BackColor="#eeeeee"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="100"
                                                                DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductName"
                                                                ServicePath="" TargetControlID="txtEProductName" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True"
                                                                TargetControlID="txtEProductName" FilterType="Custom" ValidChars=".1234567890abcdefghijklmnopqrstuvwxyz_@#()- ABCDEFGHIJKLMNOPQRSTUVWXYZ,/&^*$!~">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12" id="ctlLProductName" runat="server">
                                                            <asp:Label ID="lblLProductName" runat="server" Text="<%$ Resources:Attendance,Product Name(Ar) %>"></asp:Label>
                                                            <asp:TextBox ID="txtLProductName" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblRack1Name" runat="server" Text="<%$ Resources:Attendance,Rack Name %>" />
                                                            <asp:TextBox ID="txtRackName" TabIndex="5" runat="server" BackColor="#eeeeee" OnTextChanged="txtRackName_TextChanged"
                                                                CssClass="form-control" AutoPostBack="true" />
                                                            <cc1:AutoCompleteExtender ID="txtRackName_AutoCompleteExtender" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetCompletionListRackName" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtRackName"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>

                                                        <div class="col-md-6" runat="server" id="ctlChkIsResources">
                                                            <br />
                                                            <asp:CheckBox ID="chkIsResouces" runat="server" Text="Is Resources" CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="box box-primary">
                                                                <div class="box-body">
                                                                    <div class="form-group">
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblItypeType" runat="server" Text="<%$ Resources:Attendance, Item Type %>"></asp:Label>
                                                                            <asp:DropDownList ID="ddlItemType" TabIndex="6" runat="server" CssClass="form-control"
                                                                                AutoPostBack="True" OnSelectedIndexChanged="ddlItypeType_SelectedIndexChanged">
                                                                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance, --Select--%>" Value="0"></asp:ListItem>
                                                                                <asp:ListItem Text="Stockable" Value="S"></asp:ListItem>
                                                                                <asp:ListItem Text="NonStockable" Value="NS"></asp:ListItem>
                                                                                <asp:ListItem Text="Asset/Consumable" Value="A"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblReorderQty" runat="server" Text="<%$ Resources:Attendance,Reorder Month%>" />
                                                                            <asp:TextBox ID="txtReorderQty" TabIndex="7" runat="server" CssClass="form-control" />
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblMaintainStock" Visible="false" runat="server" Text="<%$ Resources:Attendance,Manage Inventory%>" />
                                                                            <asp:DropDownList ID="ddlMaintainStock" Visible="false" runat="server" CssClass="form-control"
                                                                                AutoPostBack="True" OnSelectedIndexChanged="ddlMaintainStock_SelectedIndexChanged"
                                                                                Enabled="false">
                                                                                <asp:ListItem Value="0" Text="<%$ Resources:Attendance,--Select--%>"></asp:ListItem>
                                                                                <asp:ListItem Value="FIFO" Text="FIFO(First In First Out)"></asp:ListItem>
                                                                                <asp:ListItem Value="LIFO" Text="LIFO(Last In First Out)"></asp:ListItem>
                                                                                <asp:ListItem Value="FEFO" Text="FEFO(First Expiry First Out)"></asp:ListItem>
                                                                                <asp:ListItem Value="BatchNo" Text="Expiry With Batch No."></asp:ListItem>
                                                                                <asp:ListItem Value="Expiry" Text="Expiry Without Batch No."></asp:ListItem>
                                                                                <asp:ListItem Value="SNO" Text="SERIAL NO."></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <asp:CheckBox ID="chkserialNo" Text="Is SerialNo" runat="server" />
                                                                            <br />
                                                                        </div>
                                                                        <div id="TdTypeOfBatchNo1" runat="server" visible="False" class="col-md-6">
                                                                            <asp:Label ID="lblTypeOfBatchNo" runat="server" Text="<%$ Resources:Attendance,Type %>" />
                                                                            <asp:DropDownList ID="ddlTypeOfBatchNo" runat="server" CssClass="form-control">
                                                                                <asp:ListItem Text="<%$ Resources:Attendance,Internally%>" Value="Internally"></asp:ListItem>
                                                                                <asp:ListItem Text="<%$ Resources:Attendance,Externally%>" Value="Externally"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <br />
                                                                        </div>

                                                                        <div class="col-md-12">
                                                                            <asp:Label ID="lbldimensional" runat="server" Text="<%$ Resources:Attendance,Dimensional %>"></asp:Label>
                                                                            <div class="input-group">
                                                                                <asp:TextBox ID="txtLenth" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                <cc1:TextBoxWatermarkExtender ID="txtLenth_TextBoxWatermarkExtender" runat="server"
                                                                                    WatermarkText="L" TargetControlID="txtLenth">
                                                                                </cc1:TextBoxWatermarkExtender>
                                                                                <div class="input-group-btn">
                                                                                    <asp:Label ID="lblWx" CssClass="form-control" runat="server" Text="X"></asp:Label>
                                                                                </div>

                                                                                <div class="input-group-btn" style="width: 31%">
                                                                                    <asp:TextBox ID="txtHeight" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                    <cc1:TextBoxWatermarkExtender ID="txtHeight_TextBoxWatermarkExtender" runat="server"
                                                                                        WatermarkText="H" TargetControlID="txtHeight">
                                                                                    </cc1:TextBoxWatermarkExtender>
                                                                                </div>
                                                                                <div class="input-group-btn">
                                                                                    <asp:Label ID="lblLx" runat="server" CssClass="form-control" Text="X"></asp:Label>
                                                                                </div>

                                                                                <asp:TextBox ID="txtDepth" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                <cc1:TextBoxWatermarkExtender ID="txtDepth_TextBoxWatermarkExtender" runat="server"
                                                                                    WatermarkText="D" TargetControlID="txtDepth">
                                                                                </cc1:TextBoxWatermarkExtender>
                                                                            </div>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-12">
                                                                            <asp:Label ID="lblalternateId" runat="server" Text="<%$ Resources:Attendance,Alternate Id %>"></asp:Label>
                                                                        </div>
                                                                        <div class="col-md-4">
                                                                            <asp:TextBox ID="txtAlterId1" runat="server" CssClass="form-control"
                                                                                AutoPostBack="false" OnTextChanged="txtAlterNateId_TextChanged"></asp:TextBox>
                                                                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" WatermarkText="AltId-1"
                                                                                TargetControlID="txtAlterId1">
                                                                            </cc1:TextBoxWatermarkExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-4">
                                                                            <asp:TextBox ID="txtAlterId2" runat="server" CssClass="form-control"
                                                                                AutoPostBack="false" OnTextChanged="txtAlterNateId_TextChanged"></asp:TextBox>
                                                                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" WatermarkText="AltId-2"
                                                                                TargetControlID="txtAlterId2">
                                                                            </cc1:TextBoxWatermarkExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-4">
                                                                            <asp:TextBox ID="txtAlterId3" runat="server" CssClass="form-control"
                                                                                AutoPostBack="false" OnTextChanged="txtAlterNateId_TextChanged"></asp:TextBox>
                                                                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" WatermarkText="AltId-3"
                                                                                TargetControlID="txtAlterId3">
                                                                            </cc1:TextBoxWatermarkExtender>
                                                                            <asp:Button ID="btnalternate" runat="server" Visible="false" />
                                                                            <br />
                                                                        </div>

                                                                        <div class="col-md-12">
                                                                            <asp:Label ID="Label34" runat="server" Text="Label Information"></asp:Label>
                                                                        </div>
                                                                        <div class="col-md-12">
                                                                            <div class="col-md-6">
                                                                                <span>Label Product Name</span>
                                                                            </div>

                                                                            <div class="col-md-6">
                                                                                <span>Label Product Description</span>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-6">

                                                                            <asp:TextBox ID="txtLabelProductName" TabIndex="8" runat="server" CssClass="form-control"
                                                                                AutoPostBack="false"></asp:TextBox>

                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:TextBox ID="txtLabelProductDescription" TabIndex="9" runat="server" CssClass="form-control"
                                                                                AutoPostBack="false"></asp:TextBox>

                                                                            <br />
                                                                        </div>

                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="box box-primary">
                                                                <div class="box-body">
                                                                    <div class="form-group">
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblProductUnit" runat="server" Text="<%$ Resources:Attendance,Product Unit %>" />
                                                                            <asp:TextBox ID="txtProductUnit" TabIndex="10" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                                BackColor="#eeeeee" OnTextChanged="txtProductUnit_TextChanged" />
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionInterval="100"
                                                                                DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionList3"
                                                                                ServicePath="" TargetControlID="txtProductUnit" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div style="display: none;" class="col-md-6">
                                                                            <asp:Label ID="lblPartNo" runat="server" Text="<%$ Resources:Attendance,Part No. %>"></asp:Label>
                                                                            <asp:TextBox ID="txtPartNo" runat="server" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblHSCode" runat="server" Text="<%$ Resources:Attendance,HS Code %>"></asp:Label>
                                                                            <asp:TextBox ID="txtHasCode" TabIndex="11" runat="server" CssClass="form-control" />
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="Label19" runat="server" Text="Warrranty(In Days)"></asp:Label>
                                                                            <asp:TextBox ID="txtProductyWarranty" TabIndex="12" runat="server" CssClass="form-control" MaxLength="3"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11_ProductWarrranty" runat="server" Enabled="True"
                                                                                TargetControlID="txtProductyWarranty" FilterType="Numbers">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblStatus" runat="server" Text="<%$ Resources:Attendance,status %>"></asp:Label>
                                                                            <asp:DropDownList ID="ddlstatus" TabIndex="13" runat="server" CssClass="form-control"
                                                                                Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddlstatus_OnSelectedIndexChanged">
                                                                                <asp:ListItem Value="True">Active</asp:ListItem>
                                                                                <asp:ListItem Value="False">InActive</asp:ListItem>
                                                                                <asp:ListItem>Discontinue</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <br />
                                                                        </div>





                                                                        <div style="display: none" class="col-md-6">
                                                                            <asp:Label ID="lbldoesProductHas" runat="server" Visible="false"
                                                                                Text="<%$ Resources:Attendance,Does Product Has %>"></asp:Label>
                                                                            <asp:Label ID="Label3" runat="server" Visible="false" Text="<%$ Resources:Attendance,Manufacturer Code%>"></asp:Label>
                                                                            <br />
                                                                        </div>
                                                                        <div style="display: none" class="col-md-6">
                                                                            <asp:TextBox ID="txtManufactururCode" runat="server" CssClass="form-control" />
                                                                            <asp:CheckBox ID="ChkHasBatchNo" runat="server" Text="<%$ Resources:Attendance,Batch No %>" />
                                                                            <br />
                                                                        </div>
                                                                        <div style="display: none" class="col-md-6">
                                                                            <asp:CheckBox ID="ChkHasSerialNo" runat="server" Text="<%$ Resources:Attendance,Serial No %>" />
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-12" id="trdiscontinuereason" runat="server" visible="false">
                                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Reason %>"></asp:Label>
                                                                            <asp:TextBox ID="txtDiscontinueReason" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                                                            <br />
                                                                        </div>

                                                                        <div class="col-md-12">
                                                                            <div class="col-md-12" style="background-color: antiquewhite">
                                                                                <asp:Label ID="lbl" runat="server" Text="<%$ Resources:Attendance,Brand %>"></asp:Label>
                                                                                <asp:CheckBoxList ID="ChkBrand" runat="server" RepeatColumns="4" CellPadding="5"
                                                                                    Font-Names="Trebuchet MS" Font-Size="Small" ForeColor="Gray" />
                                                                                <br />
                                                                            </div>
                                                                        </div>

                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" CssClass="ajax__tab_yuitabview-theme">
                                                            <cc1:TabPanel ID="Tab_Description" runat="server" HeaderText="<%$ Resources:Attendance,Description %>">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_Tab_Description" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <cc1:Editor ID="txtDesc" runat="server" Width="100%" />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Tab_Description">
                                                                        <ProgressTemplate>
                                                                            <div class="modal_Progress">
                                                                                <div class="center_Progress">
                                                                                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                                </div>
                                                                            </div>
                                                                        </ProgressTemplate>
                                                                    </asp:UpdateProgress>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                            <cc1:TabPanel ID="Tab_General" runat="server" HeaderText="<%$ Resources:Attendance,General %>">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_Tab_General" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <div class="col-md-4">
                                                                                        <asp:Label ID="lblProductCountry" runat="server" Text="<%$ Resources:Attendance,Made in %>" />
                                                                                        <asp:TextBox ID="txtProductCountry" runat="server" BackColor="#eeeeee"
                                                                                            CssClass="form-control" AutoPostBack="True" OnTextChanged="txtProductCountry_TextChanged" />

                                                                                        <cc1:AutoCompleteExtender ID="txtProductCountry_AutoCompleteExtender" runat="server"
                                                                                            CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                                                                            ServiceMethod="GetCompletionList_Contry" ServicePath="" TargetControlID="txtProductCountry"
                                                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                        </cc1:AutoCompleteExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-4">
                                                                                        <asp:Label ID="lblWholesalePrice" runat="server" Text="<%$ Resources:Attendance,Wholesale Price %>" />
                                                                                        <asp:TextBox ID="txtWholesalePrice" runat="server" CssClass="form-control" />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                                                            TargetControlID="txtWholesalePrice" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-4">
                                                                                        <asp:Label ID="lblSalesPrice1" runat="server" Text="<%$ Resources:Attendance,Sales Price 1 %>"></asp:Label>
                                                                                        <asp:TextBox ID="txtSalesPrice1" runat="server" CssClass="form-control"></asp:TextBox>

                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                                                            TargetControlID="txtSalesPrice1" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <div class="col-md-4">
                                                                                        <asp:Label ID="lblProductColor" runat="server" Text="<%$ Resources:Attendance,Color %>" />
                                                                                        <asp:TextBox ID="txtProductColor" runat="server" CssClass="form-control" />
                                                                                        <cc1:ColorPickerExtender ID="txtCardColor_ColorPickerExtender" runat="server" Enabled="True"
                                                                                            TargetControlID="txtProductColor" SampleControlID="txtProductColor">
                                                                                        </cc1:ColorPickerExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-4">
                                                                                        <asp:Label ID="lblDiscount" runat="server" Text="<%$ Resources:Attendance,Discount% %>" />
                                                                                        <asp:TextBox ID="txtDiscount" runat="server" CssClass="form-control" />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                                                            TargetControlID="txtDiscount" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-4">
                                                                                        <asp:Label ID="lblSalesPrice2" runat="server" Text="<%$ Resources:Attendance,Sales Price 2 %>"></asp:Label>
                                                                                        <asp:TextBox ID="txtSalesPrice2" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                                            TargetControlID="txtSalesPrice2" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <div class="col-md-4">
                                                                                        <asp:Label ID="lblCostPrice" runat="server" Text="<%$ Resources:Attendance,Cost Price %>" />
                                                                                        <asp:Label ID="lblcostcolon" runat="server" Text=":"></asp:Label>
                                                                                        <asp:TextBox ID="txtCostPrice" Enabled="false" runat="server" CssClass="form-control" />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                                                                            TargetControlID="txtCostPrice" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-4">
                                                                                        <asp:Label ID="lblProfit" runat="server" Text="<%$ Resources:Attendance,Profit %>" />
                                                                                        <asp:TextBox ID="txtprofit" runat="server" CssClass="form-control" />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                                                                            TargetControlID="txtprofit" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-4">
                                                                                        <asp:Label ID="lblSalesPrice3" runat="server" Text="<%$ Resources:Attendance,Sales Price 3 %>"></asp:Label>
                                                                                        <asp:TextBox ID="txtSalesPrice3" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                                            TargetControlID="txtSalesPrice3" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <div class="col-md-4">
                                                                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,Actual Weight %>" />
                                                                                        <asp:TextBox ID="txtActualWeight" runat="server" CssClass="form-control" />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True"
                                                                                            TargetControlID="txtCostPrice" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-4">
                                                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Weight Unit %>" />
                                                                                        <asp:TextBox ID="txtWeightUnit" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                                            BackColor="#eeeeee" OnTextChanged="txtProductUnit_TextChanged" />
                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtendertxtWeightUnit" runat="server" CompletionInterval="100"
                                                                                            DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionList3"
                                                                                            ServicePath="" TargetControlID="txtWeightUnit" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                        </cc1:AutoCompleteExtender>
                                                                                        <br />
                                                                                    </div>

                                                                                    <div class="col-md-4">
                                                                                        <asp:Label ID="lblminimum" runat="server" Text="<%$ Resources:Attendance,Minimum Quantity %>" />
                                                                                        <asp:TextBox ID="txtMiniQty" runat="server" CssClass="form-control" />
                                                                                        <cc1:FilteredTextBoxExtender ID="txtMiniQty_FilteredTextBoxExtender" runat="server"
                                                                                            Enabled="True" TargetControlID="txtMiniQty" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <div class="col-md-4">
                                                                                        <asp:Label ID="lblDamage" runat="server" Text="<%$ Resources:Attendance,Damage Quantity %>" />
                                                                                        <asp:TextBox ID="txtDamageQty" runat="server" ReadOnly="True" CssClass="form-control" />
                                                                                        <cc1:FilteredTextBoxExtender ID="txtDamageQty_FilteredTextBoxExtender" runat="server"
                                                                                            Enabled="True" TargetControlID="txtDamageQty" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-4">
                                                                                        <asp:Label ID="lblExpired" runat="server" Text="<%$ Resources:Attendance,Expired Quantity %>" />
                                                                                        <asp:TextBox ID="txtExpQty" runat="server" ReadOnly="True" CssClass="form-control" />
                                                                                        <cc1:FilteredTextBoxExtender ID="txtExpQty_FilteredTextBoxExtender" runat="server"
                                                                                            Enabled="True" TargetControlID="txtExpQty" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                    <div class="col-md-4">

                                                                                        <asp:Label ID="lblMaximum" runat="server" Text="<%$ Resources:Attendance,Maximum Quantity %>" />
                                                                                        <asp:TextBox ID="txtMaxQty" runat="server" CssClass="form-control" />
                                                                                        <cc1:FilteredTextBoxExtender ID="txtMaxQty_FilteredTextBoxExtender" runat="server"
                                                                                            Enabled="True" TargetControlID="txtMaxQty" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <br />
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_Tab_General">
                                                                        <ProgressTemplate>
                                                                            <div class="modal_Progress">
                                                                                <div class="center_Progress">
                                                                                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                                </div>
                                                                            </div>
                                                                        </ProgressTemplate>
                                                                    </asp:UpdateProgress>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                            <cc1:TabPanel ID="Tab_Product_Info" runat="server" HeaderText="<%$ Resources:Attendance,Product Info %>">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_Tab_Product_Info" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <%-- <div style="display: none" class="col-md-4">
                                                                                   
                                                                                </div>--%>
                                                                                <div style="display: none" class="col-md-4">
                                                                                    <asp:Button ID="btnpushBrand" runat="server" Visible="false" BorderStyle="None" BackColor="Transparent"
                                                                                        CssClass="btnPush" OnClick="btnpushBrand_Click" />
                                                                                    <br />
                                                                                    <asp:Button ID="btnpullBrand" runat="server" Visible="false" BorderStyle="None" BackColor="Transparent"
                                                                                        CssClass="btnPull" OnClick="btnpullBrand_Click" />
                                                                                    <br />
                                                                                    <asp:Button ID="btnpushBrandAll" runat="server" Visible="false" BorderStyle="None"
                                                                                        BackColor="Transparent" CssClass="btnPushAll" OnClick="btnpushBrandAll_Click" />
                                                                                    <br />
                                                                                    <asp:Button ID="btnpullBrandAll" runat="server" Visible="false" BorderStyle="None"
                                                                                        BackColor="Transparent" CssClass="btnPullAll" OnClick="btnpullBrandAll_Click" />
                                                                                    <br />
                                                                                </div>
                                                                                <div style="display: none" class="col-md-4">
                                                                                    <asp:Button ID="btnPushCate" runat="server" BorderStyle="None" Visible="false" BackColor="Transparent"
                                                                                        CssClass="btnPush" OnClick="btnPushCate_Click" />
                                                                                    <asp:Button ID="btnPullCate" runat="server" BorderStyle="None" Visible="false" BackColor="Transparent"
                                                                                        CssClass="btnPull" OnClick="btnPullCate_Click" />
                                                                                    <asp:Button ID="btnPushAllCate" runat="server" BorderStyle="None" Visible="false"
                                                                                        BackColor="Transparent" CssClass="btnPushAll" OnClick="btnPushAllCate_Click" />
                                                                                    <asp:Button ID="btnPullAllCate" runat="server" BorderStyle="None" Visible="false"
                                                                                        BackColor="Transparent" CssClass="btnPullAll" OnClick="btnPullAllCate_Click" />
                                                                                    <asp:ListBox ID="lstSelectProductCategory" runat="server" Visible="false"
                                                                                        SelectionMode="Multiple" CssClass="list" Font-Names="Trebuchet MS"
                                                                                        Font-Size="Small" ForeColor="Gray" Font-Bold="true"></asp:ListBox>
                                                                                    <asp:ListBox ID="lstSelectedProductBrand" runat="server" Visible="false" Height="156px"
                                                                                        Width="171px" SelectionMode="Multiple" CssClass="list" Font-Names="Trebuchet MS"
                                                                                        Font-Size="Small" ForeColor="Gray" Font-Bold="true"></asp:ListBox>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-4">
                                                                                    <asp:Label ID="lblProductBrand" runat="server" Font-Bold="True" Font-Names="arial"
                                                                                        Font-Size="13px" ForeColor="#666666" Text="<%$ Resources:Attendance,Manufacturing Brand %>"></asp:Label>
                                                                                    <br />
                                                                                    <asp:ListBox ID="lstProductBrand" Width="100%" Height="250px" runat="server" SelectionMode="Multiple"
                                                                                        CssClass="list" Font-Names="Trebuchet MS" Font-Size="Small" ForeColor="Gray"></asp:ListBox>
                                                                                </div>
                                                                                <div class="col-md-4">
                                                                                    <asp:Label ID="lblCategory" runat="server" Font-Bold="True" Font-Names="arial" Font-Size="13px"
                                                                                        ForeColor="#666666" Text="<%$ Resources:Attendance,Category %>"></asp:Label>
                                                                                    <div style="overflow: auto; width: 100%; height: 250px; border-style: solid; border-width: 1px; border-color: #ababd3;">
                                                                                        <asp:CheckBoxList ID="ChkProductCategory" runat="server" RepeatColumns="1" Font-Names="Trebuchet MS"
                                                                                            AutoPostBack="false" Font-Size="Small" ForeColor="Gray" OnSelectedIndexChanged="ChkProductCategory_SelectedIndexChanged" />
                                                                                    </div>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-4">
                                                                                    <asp:Label ID="Label7" runat="server" Text="Sales Commission" />
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtSalesCommission" runat="server" CssClass="form-control" />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtSalesCommission" runat="server"
                                                                                            Enabled="True" TargetControlID="txtSalesCommission" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <div class="input-group-btn">
                                                                                            <asp:Label ID="Label14" CssClass="form-control" runat="server" Text=" %"></asp:Label>
                                                                                        </div>
                                                                                    </div>
                                                                                    <br />

                                                                                    <asp:Label ID="Label15" runat="server" Text="Technical Commission"></asp:Label><br />
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtTechnicalCommission" runat="server" CssClass="form-control" />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtTechnicalCommission" runat="server"
                                                                                            Enabled="True" TargetControlID="txtTechnicalCommission" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <div class="input-group-btn">
                                                                                            <asp:Label ID="Label16" CssClass="form-control" runat="server" Text=" %"></asp:Label>
                                                                                        </div>
                                                                                    </div>
                                                                                    <br />

                                                                                    <asp:Label ID="Label17" runat="server" Text="Developer Commission"></asp:Label><br />
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtDeveloperCommission" runat="server" onchange="validatePercentage(this)" CssClass="form-control" />
                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server"
                                                                                            Enabled="True" TargetControlID="txtDeveloperCommission" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <div class="input-group-btn">
                                                                                            <asp:Label ID="Label26" CssClass="form-control" runat="server" Text=" %"></asp:Label>
                                                                                        </div>
                                                                                    </div>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-4" runat="server" id="ctlProjectName">
                                                                                    <asp:Label ID="Label27" runat="server" Text="Project Name"></asp:Label><br />
                                                                                    <asp:HiddenField runat="server" ID="hdnProjectId" Value="0" />
                                                                                    <asp:TextBox ID="txtProjectName" runat="server" CssClass="form-control" onchange="validateProject(this)" />
                                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtenderprojectName" runat="server"
                                                                                        DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                                                        MinimumPrefixLength="1" ServiceMethod="GetCompletionListProject" ServicePath=""
                                                                                        TargetControlID="txtProjectName" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                    </cc1:AutoCompleteExtender>
                                                                                    <br />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_Tab_Product_Info">
                                                                        <ProgressTemplate>
                                                                            <div class="modal_Progress">
                                                                                <div class="center_Progress">
                                                                                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                                </div>
                                                                            </div>
                                                                        </ProgressTemplate>
                                                                    </asp:UpdateProgress>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                            <cc1:TabPanel ID="Tab_Product_Supplier" runat="server" HeaderText="<%$ Resources:Attendance,Product Supplier %>">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_Tab_Product_Supplier" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblSupplier" runat="server" Text="<%$ Resources:Attendance,Select Supplier %>" />
                                                                                    <asp:TextBox ID="txtSuppliers" runat="server" BackColor="#eeeeee" OnTextChanged="txtSuppliers_OnTextChanged"
                                                                                        AutoPostBack="True" CssClass="form-control"></asp:TextBox>
                                                                                    <cc1:AutoCompleteExtender ID="txtSuppliers_AutoCompleteExtender" runat="server" CompletionInterval="100"
                                                                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionList_Supplier"
                                                                                        ServicePath="" TargetControlID="txtSuppliers" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                    </cc1:AutoCompleteExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblProCode" runat="server" Text="<%$ Resources:Attendance,Supplier Code %>" />
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtProductSupplierCode" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                        <div class="input-group-btn">
                                                                                            <asp:LinkButton ID="IbtnAddProductSupplierCode" runat="server" CausesValidation="False" OnClick="IbtnAddProductSupplierCode_Click" ToolTip="<%$ Resources:Attendance,Add %>"><i class="fa fa-plus-square" style="font-size:35px;padding:5px;margin-top: -5px;"></i></asp:LinkButton>
                                                                                            <asp:HiddenField ID="hdnProductSupplierCode" runat="server" Value="0" />
                                                                                        </div>
                                                                                    </div>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <div style="overflow: auto">
                                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GridProductSupplierCode" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                                            BorderStyle="Solid" Width="100%" PageSize="5" OnPageIndexChanging="GridProductSupplierCode_PageIndexChanging">
                                                                                            <Columns>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:ImageButton ID="IbtnDeleteSupplier" runat="server" CausesValidation="False"
                                                                                                            CommandArgument='<%# Eval("Supplier_Id") %>' ImageUrl="~/Images/Erase.png" Width="16px"
                                                                                                            ToolTip="<%$ Resources:Attendance,Delete %>" OnCommand="IbtnDeleteSupplier_Command" />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Supplier Name %>" SortExpression="Name">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvSupplierName" runat="server" Text='<%#Eval("Name") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle Width="60%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Product Supplier Code %>"
                                                                                                    SortExpression="ProductSupplierCode">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvProductSupplierCode" runat="server" Text='<%#Eval("ProductSupplierCode") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                            </Columns>


                                                                                            <PagerStyle CssClass="pagination-ys" />

                                                                                        </asp:GridView>
                                                                                        <asp:HiddenField ID="hdnfPSC" runat="server" />
                                                                                    </div>
                                                                                    <br />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress7" runat="server" AssociatedUpdatePanelID="Update_Tab_Product_Supplier">
                                                                        <ProgressTemplate>
                                                                            <div class="modal_Progress">
                                                                                <div class="center_Progress">
                                                                                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                                </div>
                                                                            </div>
                                                                        </ProgressTemplate>
                                                                    </asp:UpdateProgress>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                            <cc1:TabPanel ID="Tab_Related_Product" runat="server" HeaderText="<%$ Resources:Attendance,Related Product %>">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_Tab_Related_Product" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblProductCode" runat="server" Text="<%$ Resources:Attendance,Product Id %>" />
                                                                                    <asp:TextBox ID="txtProductCode" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                        OnTextChanged="txtProductCode_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtendertxtProductCode" runat="server"
                                                                                        CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                                                                        ServiceMethod="GetCompletionListRelatedProductCode" ServicePath="" TargetControlID="txtProductCode"
                                                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                    </cc1:AutoCompleteExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblRelatedProduct" runat="server" Text="<%$ Resources:Attendance,Product Name %>" />
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtERelatedProduct" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                                            OnTextChanged="txtERelatedProduct_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" CompletionInterval="100"
                                                                                            DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListRelatedProductName"
                                                                                            ServicePath="" TargetControlID="txtERelatedProduct" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                        </cc1:AutoCompleteExtender>
                                                                                        <div class="input-group-btn">
                                                                                            <asp:LinkButton ID="ImgButtonRelatedProductAdd" runat="server" CausesValidation="False" OnClick="ImgButtonRelatedProductAdd_Click" ToolTip="<%$ Resources:Attendance,Add %>"><i class="fa fa-plus-square" style="font-size:35px;padding:5px;margin-top: -5px;"></i></asp:LinkButton>
                                                                                            <asp:HiddenField ID="hdnRelatedProductId" runat="server" Value="0" />
                                                                                        </div>
                                                                                    </div>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <div style="overflow: auto">
                                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvRelatedProduct" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                                            BorderStyle="Solid" Width="100%" PageSize="5" OnPageIndexChanging="GvRelatedProduct_PageIndexChanging">
                                                                                            <Columns>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:ImageButton ID="IbtnDeleteSupplier" runat="server" CausesValidation="False"
                                                                                                            CommandArgument='<%# Eval("RelatedProductId") %>' ImageUrl="~/Images/Erase.png"
                                                                                                            ToolTip="<%$ Resources:Attendance,Delete %>" OnCommand="IbtnDeleteRelatedProduct_Command" />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id %>" SortExpression="Name">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvRelatedProductCode" runat="server" Text='<%#Eval("RelatedProductCode") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle Width="15%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>" SortExpression="Name">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvRelatedProductName" runat="server" Text='<%#Eval("RelatedProductName") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle Width="65%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="System Quantity">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblGvCurrenctStock" runat="server" Text='<%#GetProductStock(Eval("RelatedProductId").ToString()) %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                                                                </asp:TemplateField>
                                                                                            </Columns>


                                                                                            <PagerStyle CssClass="pagination-ys" />

                                                                                        </asp:GridView>
                                                                                        <asp:HiddenField ID="HiddenField2" runat="server" />
                                                                                    </div>
                                                                                    <br />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress8" runat="server" AssociatedUpdatePanelID="Update_Tab_Related_Product">
                                                                        <ProgressTemplate>
                                                                            <div class="modal_Progress">
                                                                                <div class="center_Progress">
                                                                                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                                </div>
                                                                            </div>
                                                                        </ProgressTemplate>
                                                                    </asp:UpdateProgress>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                            <cc1:TabPanel ID="Tab_Location_Stock" runat="server" HeaderText="<%$ Resources:Attendance,Location Stock %>">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_Tab_Location_Stock" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <div style="overflow: auto">
                                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvStockLocation" runat="server" AutoGenerateColumns="False" Width="100%"
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


                                                                                            <PagerStyle CssClass="pagination-ys" />

                                                                                        </asp:GridView>
                                                                                        <asp:HiddenField ID="HiddenField1" runat="server" />
                                                                                    </div>
                                                                                    <br />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress9" runat="server" AssociatedUpdatePanelID="Update_Tab_Location_Stock">
                                                                        <ProgressTemplate>
                                                                            <div class="modal_Progress">
                                                                                <div class="center_Progress">
                                                                                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                                </div>
                                                                            </div>
                                                                        </ProgressTemplate>
                                                                    </asp:UpdateProgress>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                            <cc1:TabPanel ID="Tab_Sno_Config" runat="server" HeaderText="Auto Serial Config">
                                                                <ContentTemplate>
                                                                    <div class="row">
                                                                        <div class="col-md-4">
                                                                            <asp:Label ID="Label61" runat="server" Text="Prefix for auto serial"></asp:Label>
                                                                            <asp:TextBox ID="txtPrefixName" runat="server" CssClass="form-control"></asp:TextBox>
                                                                            <br />
                                                                        </div>

                                                                        <div class="col-md-4">
                                                                            <asp:Label ID="Label62" runat="server" Text="Suffix for auto Serial"></asp:Label>
                                                                            <asp:TextBox ID="txtSuffixName" runat="server" CssClass="form-control"></asp:TextBox>
                                                                        </div>
                                                                        <div class="col-md-4">
                                                                            <asp:Label ID="Label63" runat="server" Text="Auto Serial Start From"></asp:Label>
                                                                            <asp:TextBox ID="txtSnoStartFrom" runat="server" CssClass="form-control"></asp:TextBox>
                                                                            <asp:RegularExpressionValidator ID="reqTxtSnoStartFrom" ControlToValidate="txtSnoStartFrom" runat="server" ErrorMessage="Only Number allowed" ForeColor="Red" ValidationExpression="^[0-9]*$" ValidationGroup="NumericaValue"></asp:RegularExpressionValidator>
                                                                        </div>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                        </cc1:TabContainer>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:CheckBox ID="chkVerify" TabIndex="14" runat="server" Visible="false" Text="<%$ Resources:Attendance,Is Verify%>" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnSave" TabIndex="15" runat="server" CssClass="btn btn-success" Text="<%$ Resources:Attendance,Save %>"
                                                            OnClick="btnSave_Click" Visible="false" />
                                                        <asp:Button ID="btnReset" TabIndex="16" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Reset %>"
                                                            OnClick="btnReset_Click" />
                                                        <asp:Button ID="btnCancel" TabIndex="17" runat="server" CssClass="btn btn-danger" Text="<%$ Resources:Attendance,Cancel %>"
                                                            OnClick="btnCancel_Click" />
                                                        <br />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div2" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label29" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				<asp:Label ID="lblbinTotalRecord" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblBinBrand" runat="server" Text="<%$ Resources:Attendance,Manufacturing Brand  %>"></asp:Label>
                                                    <asp:DropDownList ID="ddlBinBrandSearch" runat="server" CssClass="form-control">
                                                    </asp:DropDownList>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblbinCategory" runat="server" Text="<%$ Resources:Attendance,Category %>" />
                                                    <asp:DropDownList ID="ddlBinCategorySearch" runat="server" CssClass="form-control">
                                                    </asp:DropDownList>
                                                    <br />
                                                </div>

                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlrecordType" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlrecordType_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem Text="InActive" Value="InActive" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="Discontinue" Value="Discontinue"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>

                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlbinFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Product Id %>" Value="ProductCode"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Product Name %>" Value="EProductName"
                                                            Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Model No. %>" Value="ModelNo"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Item Type %>" Value="ItemType"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlbinOption" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbinbind">
                                                        <asp:TextBox ID="txtbinVal" runat="server" CssClass="form-control" placeholder="Search From Content" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">

                                                    <asp:LinkButton ID="btnbinbind" runat="server" CausesValidation="False" OnClick="btnbinbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnbinRefresh" runat="server" CausesValidation="False" OnClick="btnbinRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <%--<asp:LinkButton ID="imgBtnbinGrid" CausesValidation="False" runat="server" OnClick="imgBtnbinGrid_Click" ToolTip="<%$ Resources:Attendance, Grid View %>"><span class="fa fa-table"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imgBtnbinDatalist" CausesValidation="False" runat="server" OnClick="imgbtnbinDatalist_Click" ToolTip="<%$ Resources:Attendance,List View %>" Visible="False"><span class="fa fa-list"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;--%>
                                                    <asp:LinkButton ID="imgBtnRestore" CausesValidation="False" runat="server" Visible="false" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>

                                                    <asp:ImageButton ID="ImgbtnSelectAll" Visible="false" runat="server" Style="width: 33px;" OnClick="ImgbtnSelectAll_Click" ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true" ImageUrl="~/Images/selectAll.png" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid">
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div style="max-height: 500px; overflow: auto;">
                                                    <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvBinProduct" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                                        OnPageIndexChanging="gvBinProduct_PageIndexChanging" Width="100%"
                                                        DataKeyNames="ProductId" PageSize="<%# PageControlCommon.GetPageSize() %>">
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
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductId" runat="server" Text='<%# Eval("ProductId") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblProductCode1" runat="server" Text='<%# Eval("ProductCode") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="EProductName" HeaderText="<%$ Resources:Attendance,Product Name %>"
                                                                SortExpression="EProductName" />
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Model No %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("ModelNo") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Item Type %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblItemType" runat="server" Text='<%# Eval("ItemTypeValue") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Product Unit %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPUnit" runat="server" Text='<%# Eval("UnitName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCreatedBy" runat="server" Text='<%# Eval("CreatedEmpName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModifiedBy" runat="server" Text='<%# Eval("ModifiedEmpName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerStyle CssClass="pagination-ys" />
                                                    </asp:GridView>
                                                    <br />
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:DataList ID="dtlistbinProduct" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                                                        Width="100%">
                                                        <ItemTemplate>
                                                            <div class="col-md-6" style="min-width: 500px;">
                                                                <div class="box box-primary ">
                                                                    <div class="box-header with-border" style="height: 60px">
                                                                        <asp:LinkButton ID="LinkButton1" runat="server" Font-Bold="true" ForeColor="#1886b9" OnCommand="btnEdit_Command"
                                                                            CommandArgument='<%# Eval("ProductId") %>' Text='<%# Eval("ShortProductName") %>'
                                                                            Enabled="False"></asp:LinkButton>
                                                                    </div>
                                                                    <div class="box-footer no-padding" style="background-color: ghostwhite">

                                                                        <div class="col-md-12">
                                                                            <div class="col-md-4">
                                                                                <br />
                                                                                <asp:ImageButton ID="btnImgProduct" Width="120px" Height="120px" runat="server" ImageUrl='<%# "~/Handler.ashx?ImID="+ Eval("ProductId") %>' />
                                                                            </div>
                                                                            <div class="col-md-8">
                                                                                <ul class="nav nav-stacked">
                                                                                    <br />
                                                                                    <li>
                                                                                        <asp:Label ID="lblProductId" runat="server" Text="<%$ Resources:Attendance,Product Id %>"></asp:Label>
                                                                                        &nbsp:&nbsp<asp:Label ID="lbldlProductId" runat="server" Text='<%# Eval("ProductCode") %>'></asp:Label></li>
                                                                                    <li>
                                                                                        <asp:Label ID="lblModelNo" runat="server" Text="<%$ Resources:Attendance,Model No. %>"></asp:Label>
                                                                                        &nbsp:&nbsp<asp:Label ID="lbldltModelNo" runat="server" Text='<%# Eval("ModelNo") %>'></asp:Label></li>
                                                                                    <li>
                                                                                        <asp:Label ID="lblgvProductUnit" runat="server" Text="<%$ Resources:Attendance, Unit %>"></asp:Label>
                                                                                        &nbsp:&nbsp<asp:Label ID="lblgvPUnit" runat="server" Text='<%# Eval("UnitName") %>'></asp:Label></li>
                                                                                    <li>
                                                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Created By %>"></asp:Label>
                                                                                        &nbsp:&nbsp<asp:Label ID="Label9" runat="server" Text='<%# Eval("CreatedEmpName") %>'></asp:Label></li>
                                                                                    <li>
                                                                                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Modified By %>"></asp:Label>
                                                                                        &nbsp:&nbsp<asp:Label ID="Label11" runat="server" Text='<%# Eval("ModifiedEmpName") %>'></asp:Label></li>
                                                                                </ul>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-12">
                                                                            <asp:CheckBox ID="chkActive" runat="server" AutoPostBack="True" Text="Active"
                                                                                OnCheckedChanged="chkActive_CheckedChanged" Visible='<%# hdnCanRestore.Value=="true"?true:false %>' />
                                                                            <asp:HiddenField ID="hdnChkActive" runat="server" Value='<%# Eval("ProductId") %>' />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:DataList>
                                                </div>
                                                <div style="text-align: center">
                                                    <%-- <asp:Repeater ID="rptPagerBin" runat="server">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkPageBin" runat="server" Text='<%#Eval("Text") %>'
                                                                CommandArgument='<%# Eval("Value") %>' CssClass='<%# Convert.ToBoolean(Eval("Enabled")) ? "page_enabled" : "page_disabled" %>'
                                                                OnClick="PageBin_Changed" OnClientClick='<%# !Convert.ToBoolean(Eval("Enabled")) ? "return false;" : "" %>'></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:Repeater>--%>

                                                    <%--<asp:Repeater ID="rptPagerBin" runat="server">
                                                        <ItemTemplate>
                                                            <ul class="pagination">
                                                                <li class='<%# Convert.ToBoolean(Eval("Enabled")) ? "page-item" : "page-item active" %>'>
                                                                    <asp:LinkButton ID="lnkPageBin" runat="server" Text='<%#Eval("Text") %>'
                                                                        CommandArgument='<%# Eval("Value") %>' CssClass="page-link" OnClick="PageBin_Changed" OnClientClick='<%# !Convert.ToBoolean(Eval("Enabled")) ? "return false;" : "" %>'></asp:LinkButton>
                                                                </li>
                                                            </ul>
                                                        </ItemTemplate>
                                                    </asp:Repeater>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="Upload">
                        <asp:UpdatePanel ID="Update_Upload" runat="server">
                            <Triggers>

                                <asp:PostBackTrigger ControlID="btndownloadInvalid" />


                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">


                                                    <div id="Div_device_Information" runat="server" class="col-md-12">


                                                        <div class="row" id="Div_device_upload_operation" runat="server">
                                                            <div class="col-md-12" style="text-align: center;">

                                                                <br />
                                                                <asp:HyperLink ID="uploadEmpInfo" runat="server" Font-Bold="true" Font-Size="15px"
                                                                    NavigateUrl="~/CompanyResource/ProductMaster.xlsx" Text="Download sample format for update information" Font-Underline="true"></asp:HyperLink>
                                                                <br />

                                                            </div>
                                                            <div class="col-md-6" style="text-align: center;">
                                                                <br />
                                                                <asp:Label runat="server" Text="Browse Excel File" ID="Label169"></asp:Label>
                                                                <div class="input-group" style="width: 100%;">
                                                                    <cc1:AsyncFileUpload ID="fileLoad" OnUploadedComplete="FileUploadComplete" OnClientUploadError="uploadError" OnClientUploadStarted="uploadStarted" OnClientUploadComplete="uploadComplete"
                                                                        runat="server" CssClass="form-control" CompleteBackColor="White" UploaderStyle="Traditional" UploadingBackColor="#CCFFFF" ThrobberID="imgLoaderUpload" Width="100%" />
                                                                    <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">

                                                                        <asp:Image ID="Img_RightUpload1" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Allow.png" Style="display: none" />
                                                                        <asp:Image ID="Img_WrongUpload" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Delete1.png" Style="display: none" />
                                                                        <asp:Image ID="imgLoaderUpload" runat="server" ImageUrl="../Images/loader.gif" />
                                                                    </div>

                                                                </div>
                                                                <br />

                                                                <asp:Button ID="btnGetSheet" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                                                                    OnClick="btnGetSheet_Click" Visible="true" Text="<%$ Resources:Attendance,Connect To DataBase %>" />

                                                            </div>
                                                            <div class="col-md-6" style="text-align: center;">
                                                                <br />
                                                                <asp:Label runat="server" Text="Select Sheet" ID="Label170"></asp:Label>
                                                                <asp:DropDownList ID="ddlTables" CssClass="form-control" runat="server">
                                                                </asp:DropDownList>
                                                                <br />
                                                                <asp:Button ID="Button6" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                                                                    OnClick="btnConnect_Click" Visible="true" Text="GetRecord" />


                                                                </br>
                                                             <br />
                                                            </div>
                                                        </div>

                                                        <div class="row" id="uploadEmpdetail" runat="server" visible="false">

                                                            <div class="col-md-6" style="text-align: left">
                                                                <asp:RadioButton ID="rbtnupdall" Style="margin-left: 20px; margin-right: 20px;" runat="server" GroupName="upd" AutoPostBack="true" Checked="true" OnCheckedChanged="rbtnupdall_OnCheckedChanged" Text="All" />
                                                                <asp:RadioButton ID="rbtnupdValid" Style="margin-left: 20px; margin-right: 20px;" runat="server" GroupName="upd" AutoPostBack="true" Text="Valid" OnCheckedChanged="rbtnupdall_OnCheckedChanged" />
                                                                <asp:RadioButton ID="rbtnupdInValid" Style="margin-left: 20px; margin-right: 20px;" runat="server" GroupName="upd" AutoPostBack="true" Text="Invalid" OnCheckedChanged="rbtnupdall_OnCheckedChanged" />
                                                            </div>
                                                            <div class="col-md-6" style="text-align: right;">
                                                                <asp:Label ID="lbltotaluploadRecord" runat="server"></asp:Label>
                                                            </div>
                                                            <div class="col-md-12">
                                                                <div style="overflow: auto; max-height: 300px;">
                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvSelected" runat="server" Width="100%">


                                                                        <PagerStyle CssClass="pagination-ys" />

                                                                    </asp:GridView>
                                                                </div>
                                                                <br />
                                                            </div>



                                                            <div class="col-md-12" style="text-align: center">
                                                                <br />



                                                                <asp:Button ID="btnUploaditemInfo" runat="server" CssClass="btn btn-primary" OnClick="btnUploaditemInfo_Click"
                                                                    Text="<%$ Resources:Attendance,Upload Data %>" />

                                                                <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" TargetControlID="btnUploaditemInfo"
                                                                    ConfirmText="Are you sure to Save Records in Database.">
                                                                </cc1:ConfirmButtonExtender>
                                                                <asp:Button ID="btnResetitemInfo" runat="server" CssClass="btn btn-primary" OnClick="btnResetitemInfo_Click"
                                                                    Text="<%$ Resources:Attendance,Reset %>" />


                                                                <asp:Button ID="btndownloadInvalid" CssClass="btn btn-primary" runat="server" Text="Download Invalid Record" CausesValidation="False"
                                                                    OnClick="btndownloadInvalid_Click" />
                                                            </div>
                                                        </div>

                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="Verify">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>




                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div3" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label30" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				 <asp:Label ID="lblTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I3" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlVerifyProductFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Product Id %>" Value="ProductCode"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Product Name %>" Value="EProductName"
                                                            Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Model No. %>" Value="ModelNo"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Item Type %>" Value="ItemType"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="CreatedEmpName"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlVerifyProductOption" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-4">
                                                    <asp:Panel ID="Panel3" runat="server" DefaultButton="imgSearchVeryProduct">
                                                        <asp:TextBox ID="txtVerifyProduct" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="imgSearchVeryProduct" runat="server" CausesValidation="False" OnClick="imgSearchVeryProduct_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imgSearchVeryRefresh" runat="server" CausesValidation="False" OnClick="imgSearchVeryRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                     <asp:LinkButton ID="btnSelectRecord" runat="server" CausesValidation="False" OnClick="btnSelectRecord_Click" ToolTip="Select All"><span class="fa fa-th"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnVerifyProduct" runat="server" CausesValidation="False" OnClick="btnVerifyProduct_Click" ToolTip="Verify"><span class="fas fa-user-check"  style="font-size:25px;"></span></asp:LinkButton>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid">
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvVerifyProduct" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        DataKeyNames="ProductId" runat="server" AutoGenerateColumns="False" Width="100%"
                                                        AllowPaging="True" AllowSorting="True" OnPageIndexChanging="gvVerifyProduct_PageIndexChanging"
                                                        OnSorting="gvVerifyProduct_Sorting">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkgvSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkgvVerifySelectAll_CheckedChanged" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,View %>">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("ProductId") %>'
                                                                        ImageUrl="~/Images/Detail1.png" Height="20px" ToolTip="View" OnCommand="lnkViewDetail_Command"
                                                                        CausesValidation="False" />
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id %>" SortExpression="ProductCode">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="ProductCode" runat="server" Text='<%# Eval("ProductCode") %>'></asp:Label>
                                                                    <asp:Label ID="lblProductId" runat="server" Text='<%# Eval("ProductId") %>' Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>" SortExpression="EProductName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="EProductName" runat="server" Text='<%# Eval("EProductName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Model No %>" SortExpression="ModelNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="ModelNo" runat="server" Text='<%# Eval("ModelNo") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Unit %>" SortExpression="UnitName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="UnitName" runat="server" Text='<%# Eval("UnitName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="CreatedEmpName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="CreatedBy" runat="server" Text='<%# Eval("CreatedEmpName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                        </Columns>


                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="Reorder">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnReOrderExport" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Product Id%>" />
                                                        <asp:TextBox ID="txtReOrderProductCode" runat="server" CssClass="form-control" AutoPostBack="True" BackColor="#eeeeee"
                                                            TabIndex="27" OnTextChanged="txtReOrderProductCode_TextChanged" />

                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" CompletionInterval="100"
                                                            DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductCode"
                                                            ServicePath="" TargetControlID="txtReOrderProductCode" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblProductName" runat="server" Text="<%$ Resources:Attendance,Product Name %>" />
                                                        <asp:TextBox ID="txtReOrderProductName" runat="server" CssClass="form-control" AutoPostBack="true" BackColor="#eeeeee"
                                                            TabIndex="28" OnTextChanged="txtReOrderProductName_TextChanged" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender8" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListProductName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtReOrderProductName"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <asp:HiddenField ID="hdnNewProductId" runat="server" Value="0" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label18" runat="server" Text="<%$ Resources:Attendance,Supplier Name %>" />
                                                        <asp:TextBox ID="txtreOrderSuppliername" runat="server" BackColor="#eeeeee" OnTextChanged="txtreOrderSuppliername_OnTextChanged"
                                                            AutoPostBack="True" CssClass="form-control"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender9" runat="server" CompletionInterval="100"
                                                            DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionList_ReOrderSupplier"
                                                            ServicePath="" TargetControlID="txtreOrderSuppliername" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label20" runat="server" Text="Order For Month(In Days)" />
                                                        <asp:TextBox ID="txtOrderForMonth" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label21" runat="server" Text="<%$ Resources:Attendance,Manufacturing Brand %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlReOrderProductBrand" runat="server" CssClass="form-control">
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Category %>" />
                                                        <asp:DropDownList ID="ddlReOrderProductcategory" runat="server" CssClass="form-control">
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Label ID="Label23" runat="server" Text="<%$ Resources:Attendance,Location Name %>" />
                                                    </div>
                                                    <div class="col-md-12">

                                                        <div class="col-md-5">
                                                            <asp:ListBox ID="lstLocation" runat="server" SelectionMode="Multiple"
                                                                CssClass="form-control" Style="width: 100%;" Height="200px"></asp:ListBox>
                                                            <br />
                                                        </div>
                                                        <div class="col-lg-2" style="text-align: center">
                                                            <div style="margin-top: 33px; margin-bottom: 47px;" class="btn-group-vertical">
                                                                <asp:Button ID="btnPushDept" runat="server" CssClass="btn btn-info" Text=">" OnClick="btnPushDept_Click" />
                                                                <asp:Button ID="btnPullDept" runat="server" CssClass="btn btn-info" Text="<" OnClick="btnPullDept_Click" />
                                                                <asp:Button ID="btnPushAllDept" runat="server" CssClass="btn btn-info" Text=">>" OnClick="btnPushAllDept_Click" />
                                                                <asp:Button ID="btnPullAllDept" runat="server" CssClass="btn btn-info" Text="<<" OnClick="btnPullAllDept_Click" />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-5">
                                                            <asp:ListBox ID="lstLocationSelect" runat="server" SelectionMode="Multiple" CssClass="form-control" Style="width: 100%;" Height="200px"></asp:ListBox>
                                                        </div>

                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnreOrderreport" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Go %>"
                                                            CssClass="btn btn-primary" OnClick="btnreOrderreport_Click" />
                                                        <asp:Button ID="btnReOrderExport" runat="server" CausesValidation="False" Text="Export to Excel"
                                                            CssClass="btn btn-primary" OnClick="btnReOrderExport_Click" />
                                                        <asp:Button ID="btnreOrderreset" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Reset %>"
                                                            CssClass="btn btn-primary" OnClick="btnreOrderreset_Click" />

                                                        <br />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div4" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label31" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				 <asp:Label ID="lblQTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I4" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlQSeleclField" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Brand %>" Value="BrandName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Category%>" Value="CategoryName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Product Id %>" Value="ProductCode" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Product Name %>" Value="EProductName"></asp:ListItem>


                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlQOption" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-5">
                                                    <asp:TextBox ID="txtQValue" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="ImgBtnQBind" runat="server" CausesValidation="False" OnClick="ImgBtnQBind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="ImgBtnQRefresh" runat="server" CausesValidation="False" OnClick="ImgBtnQRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid">
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvReOrder" runat="server" AutoGenerateColumns="False" Width="100%"
                                                        OnSorting="gvReOrder_Sorting" AllowSorting="true">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="IbtnDeleteOrderItem" runat="server" CausesValidation="False" CommandArgument='<%# Eval("ProductId") %>'
                                                                        ImageUrl="~/Images/Erase.png" OnCommand="IbtnDeleteOrderItem_Command" ToolTip="<%$ Resources:Attendance,Delete %>" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Brand %>" SortExpression="BrandName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductBrand" runat="server" Text='<%# Eval("BrandName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Category %>" SortExpression="CategoryName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductCategory" runat="server" Text='<%# Eval("CategoryName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id %>" SortExpression="ProductCode">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("ProductCode") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>" SortExpression="EProductName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductName" runat="server" Text='<%# Eval("EProductName") %>'></asp:Label>
                                                                    <asp:Label ID="lblProductId" Visible="false" runat="server" Text='<%# Eval("ProductId") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="20%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Name %>" SortExpression="Unit_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUnitName" runat="server" Text='<%# Eval("Unit_Name") %>'></asp:Label>

                                                                </ItemTemplate>
                                                                <ItemStyle Width="5%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Currenct Stock" SortExpression="Currentstock">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCurrentStock" runat="server" Text='<%# Eval("Currentstock") %>'></asp:Label>

                                                                </ItemTemplate>
                                                                <ItemStyle Width="5%" HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Sales Month(Qty)" SortExpression="MonthlySales">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSalesMonth" runat="server" Text='<%# Eval("MonthlySales") %>'></asp:Label>

                                                                </ItemTemplate>
                                                                <ItemStyle Width="5%" HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Lead Days" SortExpression="LeadTimeDays">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLeaddays" runat="server" Text='<%# Eval("LeadTimeDays") %>'></asp:Label>

                                                                </ItemTemplate>
                                                                <ItemStyle Width="5%" HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Lead Time Qty" SortExpression="LeadTimeQty">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLeadqty" runat="server" Text='<%# Eval("LeadTimeQty") %>'></asp:Label>

                                                                </ItemTemplate>
                                                                <ItemStyle Width="5%" HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Order For(In Month)">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtOrderforMonth" runat="server" Text='<%# Eval("SuggestedOrderMonth") %>' AutoPostBack="true" OnTextChanged="txtOrderforMonth_OnTextChanged"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderlblOrderfor" runat="server" Enabled="True"
                                                                        TargetControlID="txtOrderforMonth" FilterType="Numbers">
                                                                    </cc1:FilteredTextBoxExtender>

                                                                </ItemTemplate>
                                                                <ItemStyle Width="5%" HorizontalAlign="Right" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Suggested Order Qty">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSuggestedOrderQty" runat="server" Text='<%# Eval("SuggestedOrderQty") %>'></asp:Label>

                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="Order Qty">
                                                                <ItemTemplate>

                                                                    <asp:TextBox ID="txtOrderqty" runat="server" Text='<%# Eval("OrderQty") %>'></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtOrderqty" runat="server" Enabled="True"
                                                                        TargetControlID="txtOrderqty" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                    </cc1:FilteredTextBoxExtender>

                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                            </asp:TemplateField>

                                                        </Columns>


                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="ControlSettingModal" tabindex="-1" role="dialog" aria-labelledby="ControlSetting_ModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title">
                        <asp:Label ID="lblUcSettingsTitle" runat="server" Text="Set Columns Visibility" />
                    </h4>

                </div>
                <div class="modal-body">
                    <UC:ucCtlSetting ID="ucCtlSetting" runat="server" />
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="Product_Modal" tabindex="-1" role="dialog" aria-labelledby="Product_ModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Product_ModalLabel">
                        <asp:Label ID="lblProductHeader" runat="server" Text="<%$ Resources:Attendance,Location Stock %>"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-12">
                                                    <div style="overflow: auto">
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
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
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


    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_List">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_New">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Bin">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


    <asp:UpdateProgress ID="UpdateProgress10" runat="server" AssociatedUpdatePanelID="Update_Upload">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <div class="modal fade" id="ReportSystem" tabindex="-1" role="dialog" aria-labelledby="ReportSystem_Web_Control"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="ReportSystem_Web_Control">Report System
                    </h4>
                </div>
                <div class="modal-body">
                    <RS:ReportSystem runat="server" ID="reportSystem" />
                </div>
                <div class="modal-footer">
                </div>

            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">

    <script type="text/javascript">

        function findPositionWithScrolling(oElement) {
            if (typeof (oElement.offsetParent) != 'undefined') {
                var originalElement = oElement;
                for (var posX = 0, posY = 0; oElement; oElement = oElement.offsetParent) {
                    posX += oElement.offsetLeft;
                    posY += oElement.offsetTop;
                    if (oElement != originalElement && oElement != document.body && oElement != document.documentElement) {
                        posX -= oElement.scrollLeft;
                        posY -= oElement.scrollTop;
                    }
                }
                return [posX, posY];
            } else {
                return [oElement.x, oElement.y];
            }
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
        function LI_Edit_Active() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }
        function LI_List_Active() {
            $("#Li_List").addClass("active");
            $("#List").addClass("active");

            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");
        }
        function Li_Tab_New() {

            setTimeout(function () { jQuery('#<%=txtProductId.ClientID%>').focus() }, 500);
        }
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }
        function Li_Tab_Upload() {
            document.getElementById('<%= Btn_Upload.ClientID %>').click();
        }
        function Li_Tab_Verify() {
            document.getElementById('<%= Btn_Verify_Request.ClientID %>').click();
        }
        function View_Address_Modal_Popup() {
            document.getElementById('<%= Btn_View_Address_Modal.ClientID %>').click();
        }
        function View_Product_Modal_Popup() {
            document.getElementById('<%= Btn_View_Product_Modal.ClientID %>').click();
        }
        function Close_Address_Modal_Popup() {
            document.getElementById('<%= Btn_View_Address_Modal.ClientID %>').click();
        }
        function Close_Product_Modal_Popup() {
            document.getElementById('<%= Btn_View_Product_Modal.ClientID %>').click();
        }



        function uploadComplete(sender, args) {
            document.getElementById('<%= Img_WrongUpload.ClientID %>').style.display = "none";
            document.getElementById('<%= Img_RightUpload1.ClientID %>').style.display = "";
        }

        function uploadError(sender, args) {
            document.getElementById('<%= Img_RightUpload1.ClientID %>').style.display = "none";
            document.getElementById('<%= Img_WrongUpload.ClientID %>').style.display = "";
            alert('Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file');
        }


        function uploadStarted(sender, args) {
            var filename = args.get_fileName();

            var filext = filename.substring(filename.lastIndexOf(".") + 1);
            filext = filext.toLowerCase();
            if (filext == "xls" || filext == "xlsx" || filext == "mdb" || filext == "accdb") {
                return true;
            }
            else {
                throw {
                    name: "Invalid File Type",
                    level: "Error",
                    message: "Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file",
                    htmlMessage: "Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file"
                }
                return false;
            }
        }


    </script>
    <script type="text/javascript">
        function Showerror() {
            alert("File size should be 50KB or less.")
        }
        function FuLogouploadStarted(sender, args) {
            debugger;
            var filename = args.get_fileName();

            var filext = filename.substring(filename.lastIndexOf(".") + 1);
            filext = filext.toLowerCase();
            if (filext == "png" || filext == "jpg" || filext == "jpge") {
                document.getElementById('<%= Img_Wrong.ClientID %>').style.display = "none";
                document.getElementById('<%= Img_Right.ClientID %>').style.display = "none";
                document.getElementById('<%= imgLoader.ClientID %>').style.display = "block";
                return true;
            }
            else {
                throw {
                    name: "Invalid File Type",
                    level: "Error",
                    message: "Invalid File Type, Select Only .png, .jpg, .jpge extension file",
                    htmlMessage: "Invalid File Type, Select Only .png, .jpg, .jpge extension file"
                }
                return false;
            }
        }
        function FuLogoUploadError(sender, args) {
            debugger;
            document.getElementById('<%= Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= Img_Wrong.ClientID %>').style.display = "";
            var img = document.getElementById('<%= imgProduct.ClientID %>');
            img.src = "../Bootstrap_Files/dist/img/NoImage.jpg";
            alert('Invalid File Type, Select Only .png, .jpg, .jpge extension file');
        }

        function FuLogouploadComplete(sender, args) {
            debugger;
            document.getElementById('<%= Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= Img_Right.ClientID %>').style.display = "";

            var img = document.getElementById('<%= imgProduct.ClientID %>');
            img.src = "<%=ResolveUrl(FuLogoUploadFolderPath) %>" + args.get_fileName();
        }


        function Modal_Open_FileUpload() {
            document.getElementById('<%= Btn_Modal_FileUpload.ClientID %>').click();
        }


        function UploadFile_UploadStarted(sender, args) {

        }


        function FUExcel_UploadStarted(sender, args) {
            var filename = args.get_fileName();

            var filext = filename.substring(filename.lastIndexOf(".") + 1);
            filext = filext.toLowerCase();
            if (filext == "xls" || filext == "xlsx" || filext == "mdb" || filext == "accdb") {
                return true;
            }
            else {
                throw {
                    name: "Invalid File Type",
                    level: "Error",
                    message: "Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file",
                    htmlMessage: "Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file"
                }
                return false;
            }
        }
        function lnkStockInfo_Command(productId) {
            window.open('../Inventory/Magic_Stock_Analysis.aspx?ProductId=' + productId + '&&Type=&&Contact=');
        }

        function lnkModel1Info_Command(modelNo) {
            if (modelNo == "" || modelNo == "0") {
                alert("Model Not Found");
                return;
            }
            /*window.open('../Inventory/ModelProductList.aspx?ModelNo=' + modelNo + '');*/
            window.open('../Inventory/ModelProductListNew.aspx?ModelNo=' + modelNo + '');
        }

        function validateProject(ctrl) {
            debugger;
            var hdnProjectId = document.getElementById('<%=hdnProjectId.ClientID%>');

            if (ctrl.value == "") {
                hdnProjectId.value = "0";
                return;
            }
            PageMethods.validateProjectName(ctrl.value, function (data) {
                hdnProjectId.value = data;
                if (data == "0") {
                    alert("Please Select From Suggestion Only");
                    ctrl.value = "";
                    ctrl.focus();
                }
            });
        }

        function showUcControlsSettings() {
            $('#ControlSettingModal').modal('show');
        }

        function fnOpenTaxConfigPage() {
            if (confirm("Do you want to configure tax")) {
                var win = window.open("../SystemSetUp/ProductTaxMaster.aspx", '_blank');
                win.focus();
            }
        }

        function getReportDataWithLoc(transId, locationid) {
            $("#prgBar").css("display", "block");
            document.getElementById('<%= reportSystem.FindControl("hdnTransId").ClientID %>').value = "";
            document.getElementById('<%= reportSystem.FindControl("hdnLocId").ClientID %>').value = locationid;
            document.getElementById('<%= reportSystem.FindControl("hdnPageRef").ClientID %>').value = "";

            document.getElementById('<%= reportSystem.FindControl("hdnProductCode").ClientID %>').value = transId;
            setReportData();
        }
    </script>

    <script src="../Script/common.js"></script>
    <script src="../Script/ReportSystem.js"></script>
    <!-- Include jQuery -->




</asp:Content>
