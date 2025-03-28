<%@ Page Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="ProductPriceList.aspx.cs" Inherits="Inventory_ProductPriceList" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/StockAnalysis.ascx" TagName="StockAnalysis" TagPrefix="SA" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-industry"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="Sales Price List"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="Sales Price List"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">
                <ul class="nav nav-tabs pull-right bg-blue-gradient">
                    <li id="Li_Upload"><a href="#Upload" data-toggle="tab">
                        <asp:UpdatePanel ID="Upload_li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;<asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Upload %>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_New"><a href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List" class="active"><a href="#List" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane active" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server">   
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvProductList" />
                                 <asp:PostBackTrigger ControlID="btnexportPrice" />
                                <asp:PostBackTrigger ControlID="btnExportPDF" />

                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div1" runat="server" class="box box-info">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				 <asp:Label ID="lblTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-minus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblBrandsearch" runat="server" Text="<%$ Resources:Attendance,Manufacturing Brand %>"></asp:Label>  <br />
                                                        <div class="col-md-5" style="width: 350px; height: 160px; overflow: auto;border:2px solid gray">
                                                            <asp:CheckBoxList ID="Chkbrandsearch" runat="server"></asp:CheckBoxList>
                                                        </div>
                                                        <br />                                
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lbllocationsearch" runat="server" Text="<%$ Resources:Attendance,Category %>" />
                                                        <br />
                                                         <div class="col-md-5" style="width: 350px; height: 160px; overflow: auto;border:2px solid gray">
                                                        <asp:CheckBoxList ID="Chkcategorysearch" runat="server" ></asp:CheckBoxList>  
                                                        </div>
                                                        <br />
                                                    </div>        
                                                    <div class="col-md-12">
                                                        <br />
                                                      <asp:CheckBox ID="chkStockZero" runat="server" Text="Stock Zero" />
                                                    </div>
                                                    <div class="col-lg-3">                                                     
                                                        <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlFieldName_OnSelectedIndexChanged">
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Product Id %>" Value="ProductId"  Selected="True" ></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Product Name %>" Value="ProductName" ></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Model No. %>" Value="Model_No"></asp:ListItem>
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
                                                          <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbindrpt_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;
                                                         <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefreshReport_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;                                                  
                                                           <asp:LinkButton ID="btnExportPDF" runat="server" CausesValidation="False" OnClick="btnExportPDF_Click" ToolTip="Export To Pdf"><span class="fas fa-file-pdf pdf-icon"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;
       


                                                     <asp:LinkButton ID="btnexportPrice" runat="server" CausesValidation="False" OnClick="btnExportPriceList_Click" ToolTip="Price List"><span class="fas fa-file-csv"  style="font-size:25px;"></span></asp:LinkButton>
                                                    </div>
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
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvProductList" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="gvProductList_PageIndexChanging"
                                                        AllowSorting="True" OnSorting="gvProductList_Sorting">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="ProductId" SortExpression="ProductId">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("ProductId") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ProductName" SortExpression="ProductName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductName" runat="server" Text='<%# Eval("ProductName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Model No" SortExpression="Model_No">
                                                                <ItemTemplate>
                                                                  <asp:LinkButton ID="linkBtnModel_No"  runat="server" OnClientClick='<%# "GetModelNo(\"" + Eval("Model_No") + "\"); return false;" %>' Text='<%# Eval("Model_No") %>'></asp:LinkButton>

                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Unit" SortExpression="Unit_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUnit_Name" runat="server" Text='<%# Eval("Unit_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Cost Price" SortExpression="CostPrice">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCostPricegv" runat="server" Text='<%# Eval("CostPrice") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Stock" SortExpression="Stock">
                                                                <ItemTemplate>
                                                                   <%-- <asp:Label ID="lblStock" runat="server" Text='<%# Eval("Stock") %>'></asp:Label>--%>
                                                                     <asp:LinkButton ID="lnkStockInfo" runat="server" Text='<%# Eval("Stock") %>' Font-Underline="true" ForeColor="Blue"  ToolTip="View Detail" OnCommand="lnkStockInfo_Command" OnClientClick="$('#Product_StockAnalysis').modal('show');" CommandArgument='<%# GetProductId(Eval("ProductId").ToString()) %>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="UpComing Quantity" SortExpression="UPCommingQty">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblupcomingQty" runat="server" Text='<%# Eval("UPCommingQty") %>' ></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                          <asp:TemplateField SortExpression="SalesPrice1">
                                                              <HeaderTemplate>             
                                                                  <asp:Label ID="lblHeaderSalesPrice1" runat="server" Text='<%# GetCurrencyName("SalesPrice1").ToString() %>' style="color:#3c8dbc;"></asp:Label>
                                                                                                                                 
                                                              </HeaderTemplate>

                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSalesPrice1" runat="server" Text='<%#GetCurrencySymbol(Eval("SalesPrice1").ToString(),"0") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="SalesPrice2" SortExpression="SalesPrice1">
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="lblHeaderSalesPrice2" runat="server" Text='<%# GetCurrencyName("SalesPrice2").ToString() %>' style="color:#3c8dbc;"></asp:Label>
                                                                  
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSalesPrice2" runat="server" Text='<%#GetCurrencySymbol(Eval("SalesPrice2").ToString(),"0") %>'></asp:Label>

                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="SalesPrice3" SortExpression="SalesPrice3">
                                                                <HeaderTemplate>
                                                                   <asp:Label ID="lblHeaderSalesPrice3" runat="server" Text='<%# GetCurrencyName("SalesPrice3").ToString() %>' style="color:#3c8dbc;"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSalesPrice3" runat="server" Text='<%# GetCurrencySymbol(Eval("SalesPrice3").ToString(),"0") %>'></asp:Label>
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
                    <div class="tab-pane" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblProductcode" runat="server" Text="Product Id"></asp:Label>
                                                        <asp:TextBox ID="txtProductcode" runat="server" AutoPostBack="true" OnTextChanged="txtProductCode_TextChanged" CssClass="form-control" TabIndex="1" BackColor="#eeeeee"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" CompletionInterval="100"
                                                            DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductCode"
                                                            ServicePath="" TargetControlID="txtProductcode" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblProductName" runat="server" Text="Product Name"></asp:Label>
                                                        <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtProductName_TextChanged" TabIndex="2" BackColor="#eeeeee"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListProductName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtProductName"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblSalesPrice1" runat="server" Text="Sales Price 1"></asp:Label>
                                                        <asp:TextBox ID="txtSalesPrice1" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblSalesPrice2" runat="server" Text="Sales Price 2"></asp:Label>
                                                        <asp:TextBox ID="txtSalesPrice2" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblSalesPrice3" runat="server" Text="Sales Price 3"></asp:Label>
                                                        <asp:TextBox ID="txtSalesPrice3" runat="server" CssClass="form-control" TabIndex="5"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <br />
                                                        <asp:Button ID="btnAdd" Text="Add" CssClass="btn btn-primary" runat="server" OnClick="btnAdd_Click" />
                                                        &nbsp;
                                                        <asp:Button ID="btnReset" Text="Reset" CssClass="btn btn-danger" runat="server" OnClick="btnReset_Click" />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <br />
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvProduct" runat="server" Width="100%" ShowFooter="true"
                                                            AutoGenerateColumns="False">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Delete">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgBtnDetailDelete" runat="server" CommandArgument='<%# Eval("Serial_No") %>'
                                                                            Height="16px" ImageUrl="~/Images/Erase.png" Width="16px" OnCommand="imgBtnDetailDelete_Command" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Serial No.">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSNo" runat="server" Text='<%# Eval("Serial_No") %>'></asp:Label>

                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="ProductId">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="gvProductCode" runat="server" Text='<%#ProductCode(Eval("ProductId").ToString()) %>'></asp:Label>
                                                                        <asp:HiddenField ID="hdngvProductId" runat="server" Value='<%# Eval("ProductId") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Product Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblProductName" runat="server" Text='<%# GetProductName(Eval("ProductId").ToString()) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="SalesPrice 1">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="gvSalesPrice1" runat="server" Text='<%# Eval("SalesPrice1") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="SalesPrice 2">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="gvSalesPrice2" runat="server" Text='<%# Eval("SalesPrice2") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="SalesPrice3">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="gvSalesPrice3" runat="server" Text='<%# Eval("SalesPrice3") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                    <div class="col-md-12" id="divButtons" runat="server" style="text-align: center">
                                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-success" OnClick="btnSave_Click" />
                                                        &nbsp;
                                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger" OnClick="btnCancel_Click" />
                                                    </div>
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
                                <asp:PostBackTrigger ControlID="btnExport" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label runat="server" Text="Browse Excel File" ID="Label169"></asp:Label>
                                                        <div class="input-group" style="width: 100%;">
                                                            <cc1:AsyncFileUpload ID="fileLoad"
                                                                OnClientUploadStarted="FUExcel_UploadStarted"
                                                                OnClientUploadError="FUExcel_UploadError"
                                                                OnClientUploadComplete="FUExcel_UploadComplete"
                                                                OnUploadedComplete="FUExcel_FileUploadComplete"
                                                                runat="server" CssClass="form-control"
                                                                CompleteBackColor="White"
                                                                UploaderStyle="Traditional"
                                                                UploadingBackColor="#CCFFFF"
                                                                ThrobberID="FUExcel_ImgLoader" Width="100%" />
                                                            <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                                <asp:LinkButton ID="FUExcel_Img_Right" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-check" style="font-size:30px;color:#22cb33"></i></asp:LinkButton>
                                                                <asp:LinkButton ID="FUExcel_Img_Wrong" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-remove" style="font-size:30px"></i></asp:LinkButton>
                                                                <asp:Image ID="FUExcel_ImgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <br />
                                                        <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUploadExcel_Click" CssClass="btn btn-primary" />
                                                        &nbsp; 
                                                          <asp:Button ID="btnExport" runat="server" Text="Export"  OnClick="btnExportExcel_Click" CssClass="btn btn-primary" />
                                                    </div>
                                                    <div class="row" id="uploadOb" runat="server" visible="false">
                                                        <br />
                                                        <div class="col-md-6" style="text-align: left">
                                                            <asp:RadioButton ID="rbtnupdall" Style="margin-left: 20px; margin-right: 20px;" runat="server" GroupName="upd" AutoPostBack="true" Checked="true" OnCheckedChanged="rbtnupd_CheckedChanged" Text="All" />
                                                            <asp:RadioButton ID="rbtnupdValid" Style="margin-left: 20px; margin-right: 20px;" runat="server" GroupName="upd" AutoPostBack="true" Text="Valid" OnCheckedChanged="rbtnupd_CheckedChanged" />
                                                            <asp:RadioButton ID="rbtnupdInValid" Style="margin-left: 20px; margin-right: 20px;" runat="server" GroupName="upd" AutoPostBack="true" Text="Invalid" OnCheckedChanged="rbtnupd_CheckedChanged" />
                                                        </div>

                                                        <div class="col-md-6" style="text-align: right;">
                                                            <asp:Label ID="lbltotaluploadRecord" runat="server"></asp:Label>
                                                        </div>
                                                        <div class="col-md-12">
                                                            <div style="overflow: auto; max-height: 300px;">
                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" AutoGenerateColumns="False" ID="gvImport" runat="server" Width="100%">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="ProductId" SortExpression="ProductCode">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvImportProductCode" runat="server" Text='<%# Eval("ProductId") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                         <asp:TemplateField HeaderText="ProductName" SortExpression="ProductCode">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvImportProductName" runat="server" Text='<%# Eval("ProductName") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                         <asp:TemplateField HeaderText="SalesPrice1" SortExpression="SalesPrice1">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvImportSalesPrice1" runat="server" Text='<%# Eval("SalesPrice1") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                         <asp:TemplateField HeaderText="SalesPrice2" SortExpression="SalesPrice2">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvImportSalesPrice2" runat="server" Text='<%# Eval("SalesPrice2") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                         <asp:TemplateField HeaderText="SalesPrice3" SortExpression="SalesPrice3">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvImportSalesPrice3" runat="server" Text='<%# Eval("SalesPrice3") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                         <asp:TemplateField HeaderText="Is Valid" SortExpression="IsValid">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvImportIsValid" runat="server" Text='<%# Eval("IsValid") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                    </Columns>

                                                                    <PagerStyle CssClass="pagination-ys" />
                                                                </asp:GridView>
                                                            </div>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12" style="text-align:left">
                                                            <asp:Label ID="lblCheckLocation" runat="server" Text="Location In Same Currency :" ></asp:Label> &nbsp;
                                                            <br />
                                                            <asp:CheckBoxList ID="ChkSubLocation" CellPadding="5" Font-Names="Trebuchet MS" Font-Size="Small" ForeColor="Gray"   runat="server"></asp:CheckBoxList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-12" style="text-align:center" >
                                                            <asp:Button ID="btnSaveUpload" runat="server" Text="Save" CssClass="btn btn-success" OnClick="btnUploadSave_Click" />
                                                            &nbsp;
                                                            <asp:Button ID="btnResetUpload" runat="server" Text="Reset" CssClass="btn btn-danger" OnClick="btnUploadReset_Click" />
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
                    <asp:UpdateProgress ID="UpdateProgress8" runat="server" AssociatedUpdatePanelID="Update_New">
                        <ProgressTemplate>
                            <div class="modal_Progress">
                                <div class="center_Progress">
                                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                </div>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                      <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Upload">
                        <ProgressTemplate>
                            <div class="modal_Progress">
                                <div class="center_Progress">
                                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                </div>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                      <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_List">
                        <ProgressTemplate>
                            <div class="modal_Progress">
                                <div class="center_Progress">
                                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                </div>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
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
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
    <script src="../Script/ReportSystem.js"></script>
    <script src="../Script/master.js"></script>
    <script type="text/javascript">
        function resetPosition(object, args) {
            $(object._completionListElement.children).each(function () {
                var data = $(this)[0];
                if (data != null) {
                    data.style.paddingLeft = "10px";
                    data.style.cursor = "pointer";
                    data.style.borderBottom = "solid 1px #e7e7e7";
                }
            });
            object._completionListElement.className = "scrollbar scrollbar-primary force-overflow";


            var tb = object._element;
            var tbposition = findPositionWithScrolling(tb);
            var xposition = tbposition[0] + 2;
            var yposition = tbposition[1] + 25;
            var ex = object._completionListElement;
            if (ex)
                $common.setLocation(ex, new Sys.UI.Point(xposition, yposition));
        }
        function FUExcel_UploadComplete(sender, args) {
            document.getElementById('<%= FUExcel_Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= FUExcel_Img_Right.ClientID %>').style.display = "";
        }
        function FUExcel_UploadError(sender, args) {
            document.getElementById('<%= FUExcel_Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= FUExcel_Img_Wrong.ClientID %>').style.display = "";
            alert('Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file');
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
        function GetModelNo(ModelNo) {
            window.open("ModelProductList.aspx?ModelNo=" + ModelNo +"");
        }
        function lnkStockInfo_Command(productId) {
            window.open('../Inventory/Magic_Stock_Analysis.aspx?ProductId=' + productId + '&&Type=&&Contact=');
        }
    </script>
</asp:Content>
