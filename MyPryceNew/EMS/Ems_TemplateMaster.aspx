<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" EnableEventValidation="true" CodeFile="Ems_TemplateMaster.aspx.cs" Inherits="EMS_Ems_TemplateMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-file-alt"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Template Setup%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,EMS%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,EMS%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Template Setup%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="btnList_Click" Text="List" />
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" OnClick="btnNew_Click" Text="New" />
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
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
                    <li id="Li_Bin"><a href="#Bin" onclick="Li_Tab_Bin()" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
                    <li id="Li_New"><a onclick="Li_Tab_New()" href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List" class="active"><a href="#List" onclick="Li_Tab_List()" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane active" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <ContentTemplate>


                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-3">

                                                        <asp:DropDownList ID="ddlTemplatesearch" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTemplatesearch_SelectedIndexChanged">
                                                            <asp:ListItem Selected="True" Text="--Select Filter--" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Business Group" Value="CG"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Model No%>" Value="MO"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Product Brand Name%>" Value="PB"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Product Category%>" Value="PC"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Product Id%>" Value="PI"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Product Name%>" Value="PN"></asp:ListItem>

                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-9">
                                                        <asp:HiddenField ID="hdnAdvanceFilterValue" runat="server" />
                                                        <asp:TextBox ID="txtAdvanceFilterValue" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtAdvanceFilterValue_TextChanged" BackColor="#eeeeee"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" CompletionInterval="100"
                                                            DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListAdvanceFilter"
                                                            ServicePath="" TargetControlID="txtAdvanceFilterValue" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <%-- <div class="col-md-6">
                                                        <asp:CheckBox ID="chkDiscontinue" CssClass="form-control" runat="server" Text="Discontinue Product" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" id="pnladvancseserach" runat="server" defaultbutton="btnsearchProduct">
                                                        <asp:TextBox ID="txtSearchPrduct" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" WatermarkText="Search Product" TargetControlID="txtSearchPrduct"></cc1:TextBoxWatermarkExtender>
                                                        <asp:Button ID="btnsearchProduct" runat="server" OnClick="btngo_Click" Visible="false" />
                                                        <br />
                                                    </div>--%>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btngo" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Go %>" CssClass="btn btn-primary" OnClick="btngo_Click" />
                                                        <asp:Button ID="btnResetSreach" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Reset %>" CssClass="btn btn-primary" OnClick="btnResetSreach_Click" />

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <asp:HiddenField runat="server" ID="hdnCanEdit" />
                                <asp:HiddenField runat="server" ID="hdnCanDelete" />
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div1" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					  <asp:Label ID="lblTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                                                <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                                <asp:HiddenField ID="HDFSort" runat="server" />

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">

                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Template Name%>" Value="Template_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Model No%>" Value="Model_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Product Brand Name%>" Value="Brand_Name"></asp:ListItem>

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
                                                <div class="col-lg-5">
                                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                        <asp:TextBox ID="txtValue" placeholder="Search from Content" runat="server" CssClass="form-control"></asp:TextBox>

                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2" style="text-align: center">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False"
                                                        OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False"
                                                        OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                </div>


                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid" <%= gvTemplateMaster.Rows.Count>0?"style='display:block'":"style='display:none'"%>>

                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvTemplateMaster" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                        OnPageIndexChanging="gvTemplateMaster_PageIndexChanging" OnSorting="gvTemplateMaster_OnSorting">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Serial No. %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblserialNo" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">


                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                    CausesValidation="False" OnCommand="btnEdit_Command"
                                                                                    ToolTip="<%$ Resources:Attendance,Edit %>"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                    OnCommand="IbtnDelete_Command"
                                                                                    ToolTip="<%$ Resources:Attendance,Delete %>"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>


                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>



                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Template Name%>" SortExpression="Template_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblListTempName" runat="server" Text='<%# Eval("Template_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Model No %>" SortExpression="Model_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModelNo" runat="server" Text='<%# Eval("Model_No") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Brand Name %>" SortExpression="Brand_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBrandName" runat="server" Text='<%# Eval("Brand_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <%--<asp:Image ID="img1" runat="server" ImageUrl='<%# "~/CompanyResource/Template/" +Eval("Field1") %>'
                                                                        Height="100px" Width="100px" />--%>
                                                                    <asp:Image ID="img1" runat="server" ImageUrl='<%#GetImage(Eval("Field1"))%>'
                                                                        Height="100px" Width="100px" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="100px" />
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
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblTemName" runat="server" Text="<%$ Resources:Attendance,Template Name%>"></asp:Label>
                                                        <asp:TextBox ID="txtTemplateName" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblModelNo" runat="server" Text="<%$ Resources:Attendance,Model No %>"></asp:Label>

                                                        <asp:TextBox ID="txtModelNo" runat="server" BackColor="#eeeeee" CssClass="form-control"
                                                            AutoPostBack="true" OnTextChanged="txtModelNo_TextChanged"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionInterval="100"
                                                            DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListModelNo"
                                                            ServicePath="" TargetControlID="txtModelNo" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>

                                                        <asp:HiddenField ID="hdnModelId" runat="server" Value="0" />


                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblBrandName" runat="server" Text="<%$ Resources:Attendance,Product Brand Name %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlProductBrand" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlProductBrand_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                        <br />

                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblEmpLogo" runat="server" Text="<%$ Resources:Attendance,Image  %>"></asp:Label>
                                                        <div class="input-group" style="width: 100%;">
                                                            <cc1:AsyncFileUpload ID="FULogoPath" OnUploadedComplete="FuLogo_FileUploadComplete" OnClientUploadError="FuLogoUploadError" OnClientUploadStarted="FuLogouploadStarted" OnClientUploadComplete="FuLogouploadComplete"
                                                                runat="server" CssClass="form-control" CompleteBackColor="White" UploaderStyle="Traditional" UploadingBackColor="#CCFFFF" ThrobberID="imgLoader" Width="100%" />
                                                            <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                                <asp:Image ID="Img_Right" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Allow.png" Style="display: none" />
                                                                <asp:Image ID="Img_Wrong" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Delete1.png" Style="display: none" />
                                                                <asp:Image ID="imgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                            </div>
                                                        </div>

                                                        <asp:HiddenField ID="hdnimageurl" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" style="text-align: center">
                                                        <asp:Image ID="imgLogo" ImageUrl="../Bootstrap_Files/dist/img/NoImage.jpg" runat="server" Width="100px" Height="100px" />
                                                        <br />
                                                        <br />
                                                        <asp:Button ID="btnUpload" TabIndex="103" runat="server" Text="<%$ Resources:Attendance,Load %>"
                                                            CssClass="btn btn-primary" OnClick="btnUploadImage_Click" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Width="100%" CssClass="ajax__tab_yuitabview-theme" OnClientActiveTabChanged="tabChanged">
                                                            <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="Business Group">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_Tab_Business_Group" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12">

                                                                                    <asp:CheckBox ID="chkSelectAll" onClick="new_validation();" Text="Select all" runat="server" />
                                                                                    <br />

                                                                                    <%--<asp:Panel ID="pnlTreeView" runat="server" ScrollBars="Auto"
                                                                                        HorizontalAlign="Left" BackColor="White">--%>
                                                                                    <div style="overflow: auto; max-height: 500px;">
                                                                                        <asp:TreeView ID="navTree" runat="server" OnSelectedNodeChanged="navTree_SelectedNodeChanged1"
                                                                                            OnTreeNodeCheckChanged="navTree_TreeNodeCheckChanged" ShowCheckBoxes="All" Font-Names="Trebuchet MS"
                                                                                            Font-Size="Small" ForeColor="Gray">
                                                                                        </asp:TreeView>
                                                                                    </div>
                                                                                    <%--</asp:Panel>--%>
                                                                                    <br />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Tab_Business_Group">
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
                                                            <cc1:TabPanel ID="Tab_Product_category_and_Product" runat="server" HeaderText="Product category and Product">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_Tab_Product_category_and_Product" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-6">
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtproductcategorysearch" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                                            BackColor="#eeeeee" OnTextChanged="txtproductcategorysearch_OnTextChanged"></asp:TextBox>
                                                                                        <cc1:TextBoxWatermarkExtender ID="txtwatermarkup" runat="server" TargetControlID="txtproductcategorysearch"
                                                                                            WatermarkText="Search Product Category">
                                                                                        </cc1:TextBoxWatermarkExtender>
                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                                                            Enabled="True" ServiceMethod="GetCompletionListCategoryname" ServicePath="" CompletionInterval="100"
                                                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtproductcategorysearch"
                                                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                        </cc1:AutoCompleteExtender>
                                                                                        <div class="input-group-btn">
                                                                                            <asp:ImageButton ID="imgsearchproductcategory" runat="server" CausesValidation="False"
                                                                                                Height="25px" ImageUrl="~/Images/search.png" Width="25px" ToolTip="<%$ Resources:Attendance,Search %>"
                                                                                                OnClick="imgsearchproductcategory_OnClick" Visible="false"></asp:ImageButton>
                                                                                        </div>
                                                                                        <div class="input-group-btn">
                                                                                            <asp:ImageButton ID="ImgRefreshProductcategory" runat="server" CausesValidation="False"
                                                                                                Height="25px" ImageUrl="~/Images/refresh.png" Width="25px" ToolTip="<%$ Resources:Attendance,Refresh %>"
                                                                                                OnClick="ImgRefreshProductcategory_OnClick"></asp:ImageButton>
                                                                                        </div>
                                                                                    </div>
                                                                                    <br />
                                                                                    <%--<asp:Panel ID="pnlProductCategory" runat="server" BorderStyle="Solid"
                                                                                        BorderWidth="1px" BorderColor="#abadb3" BackColor="White" ScrollBars="Auto">--%>
                                                                                    <div style="overflow: auto; max-height: 300px; height: 300px; border-style: solid; border-width: 1px; border-color: #abadb3;">
                                                                                        <asp:CheckBoxList ID="ChkProductCategory" Style="margin-left: 10px;" runat="server" RepeatColumns="1" Font-Names="Trebuchet MS"
                                                                                            AutoPostBack="true" Font-Size="Small" ForeColor="Gray" OnSelectedIndexChanged="ChkProductCategory_SelectedIndexChanged" />
                                                                                    </div>
                                                                                    <%--</asp:Panel>--%>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtproductsearch" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                                            BackColor="#eeeeee" OnTextChanged="txtproductsearch_OnTextChanged"></asp:TextBox>
                                                                                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtproductsearch"
                                                                                            WatermarkText="Search Product Name">
                                                                                        </cc1:TextBoxWatermarkExtender>
                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                                                            Enabled="True" ServiceMethod="GetCompletionListProductName" ServicePath="" CompletionInterval="100"
                                                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtproductsearch"
                                                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                        </cc1:AutoCompleteExtender>
                                                                                        <div class="input-group-btn">
                                                                                            <asp:ImageButton ID="imgsearchproduct" runat="server" CausesValidation="False" Height="25px"
                                                                                                ImageUrl="~/Images/search.png" Width="25px" ToolTip="<%$ Resources:Attendance,Search %>"
                                                                                                OnClick="imgsearchproduct_OnClick" Visible="false"></asp:ImageButton>
                                                                                        </div>
                                                                                        <div class="input-group-btn">
                                                                                            <asp:ImageButton ID="ImgRefreshProduct" runat="server" CausesValidation="False" Height="25px"
                                                                                                ImageUrl="~/Images/refresh.png" Width="25px" ToolTip="<%$ Resources:Attendance,Refresh %>"
                                                                                                OnClick="ImgRefreshProduct_OnClick"></asp:ImageButton>
                                                                                        </div>
                                                                                    </div>
                                                                                    <br />
                                                                                    <%--<asp:Panel ID="pnlProductChildCategory" runat="server"
                                                                                        BorderStyle="Solid" BorderWidth="1px" BorderColor="#abadb3" BackColor="White"
                                                                                        ScrollBars="Auto">--%>
                                                                                    <div style="overflow: auto; max-height: 300px; height: 300px; border-style: solid; border-width: 1px; border-color: #abadb3;">
                                                                                        <asp:CheckBoxList ID="ChkProductChildCategory" Style="margin-left: 10px;" runat="server" RepeatColumns="1" Font-Names="Trebuchet MS"
                                                                                            Font-Size="Small" ForeColor="Gray" AppendDataBoundItems="False" />
                                                                                    </div>
                                                                                    <%--</asp:Panel>--%>
                                                                                    <br />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_Tab_Product_category_and_Product">
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
                                                            <cc1:TabPanel ID="Tab_Upload_Template" runat="server" HeaderText="Upload Template">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_Tab_Upload_Template" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-6">
                                                                                    <div class="input-group" style="width: 100%;">
                                                                                        <cc1:AsyncFileUpload ID="UploadTemplate" OnUploadedComplete="UploadTemplate_FileUploadComplete" OnClientUploadError="UploadTemplate_UploadError" OnClientUploadStarted="UploadTemplate_UploadStarted" OnClientUploadComplete="UploadTemplate_UploadComplete"
                                                                                            runat="server" CssClass="form-control" CompleteBackColor="White" UploaderStyle="Traditional" UploadingBackColor="#CCFFFF" ThrobberID="UploadTemplate_imgLoader" Width="100%" />
                                                                                        <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                                                            <asp:Image ID="UploadTemplate_Img_Right" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Allow.png" Style="display: none" />
                                                                                            <asp:Image ID="UploadTemplate_Img_Wrong" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Delete1.png" Style="display: none" />
                                                                                            <asp:Image ID="UploadTemplate_imgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                                                        </div>
                                                                                    </div>
                                                                                    <asp:Label ID="lblTemplatePath" runat="server"></asp:Label>
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Button ID="btnTemplate" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Load%>"
                                                                                        OnClick="btnTemplate_Click" />
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-12" style="height: 300px;">
                                                                                    <cc2:Editor ID="Editor1" runat="server" />
                                                                                    <br />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_Tab_Upload_Template">
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
                                                        </cc1:TabContainer>

                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Attendance,Save %>" OnClick="btnSave_Click"
                                                            Visible="false" CssClass="btn btn-success" ValidationGroup="a" />

                                                        <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                            OnClick="btnReset_Click" CssClass="btn btn-primary" CausesValidation="False" />

                                                        <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                            OnClick="btnCancel_Click" CssClass="btn btn-danger" CausesValidation="False" />

                                                        <asp:HiddenField ID="editid" runat="server" />
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
                                                    <asp:Label ID="Label2" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblbinTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlbinFieldName" runat="server" CssClass="form-control">
                                                        <%--  <asp:ListItem Text="<%$ Resources:Attendance,Vacancy Id %>" Value="Vacancy_Id"></asp:ListItem>--%>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Template Name %>" Value="Template_Name"></asp:ListItem>
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
                                                <div class="col-lg-4">
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbinbind">
                                                        <asp:TextBox ID="txtbinValue" placeholder="Search from Content" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3" >
                                                    <asp:LinkButton ID="btnbinbind" runat="server" CausesValidation="False" 
                                                        OnClick="btnbinbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="btnbinRefresh" runat="server" CausesValidation="False" 
                                                         OnClick="btnbinRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="imgBtnRestore" CausesValidation="False" 
                                                        runat="server"  OnClick="imgBtnRestore_Click"
                                                        ToolTip="<%$ Resources:Attendance, Active %>" ><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="ImgbtnSelectAll"  runat="server" OnClick="ImgbtnSelectAll_Click" 
                                                        ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true">
                                                         <span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>


                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid" <%= gvTemplateMasterBin.Rows.Count>0?"style='display:block'":"style='display:none'"%> >
                                   
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvTemplateMasterBin" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="gvTemplateMasterBin_PageIndexChanging"
                                                        OnSorting="gvTemplateMasterBin_OnSorting" DataKeyNames="Trans_Id" AllowSorting="True">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                                    <asp:Label ID="lblFileId" runat="server" Visible="false" Text='<%# Eval("Trans_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChanged"
                                                                        AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                               
                                                            </asp:TemplateField>
                                                            <%--   <asp:TemplateField HeaderText="<%$ Resources:Attendance,Trans Id%>" SortExpression="Trans_Id">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVacancyName" runat="server" Text='<%# Eval("Trans_Id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle  />
                                                </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Template Name%>" SortExpression="Template_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTempName" runat="server" Text='<%# Eval("Template_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                        </Columns>


                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                    <asp:HiddenField ID="HDFSortbin" runat="server" />
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

    <asp:Panel ID="pnlMenuList" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlMenuNew" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlMenuBin" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PnlNewEdit" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PnlList" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PnlBin" runat="server" Visible="false"></asp:Panel>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
    <script type="text/javascript">
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

        function Li_Tab_List() {
            document.getElementById('<%= Btn_List.ClientID %>').click();
        }
        function Li_Tab_New() {
            document.getElementById('<%= Btn_New.ClientID %>').click();
        }
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }
    </script>
    <script type="text/javascript">



        function new_validation() {
            var val = document.getElementById("<%= chkSelectAll.ClientID  %>").checked;
            if (val) {
                $('[id*=navTree] input[type=checkbox]').prop('checked', true);
            }
            else {
                $('[id*=navTree] input[type=checkbox]').prop('checked', false);
            }
        }



        function postBackByObject() {
            var o = window.event.srcElement;
            if (o.tagName == "INPUT" && o.type == "checkbox") {
                __doPostBack("<%= navTree.ClientID%>", "");
            }
        }
        var lasttab = 0;
        function tabChanged(sender, args) {
            // do what ever i want with lastTab value
            lasttab = sender.get_activeTabIndex();
        }
    </script>

    <script type="text/javascript">
        function FuLogouploadStarted(sender, args) {
            var filename = args.get_fileName();

            var filext = filename.substring(filename.lastIndexOf(".") + 1);
            filext = filext.toLowerCase();
            if (filext == "png" || filext == "jpg" || filext == "jpge") {
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
            document.getElementById('<%= Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= Img_Wrong.ClientID %>').style.display = "";
            var img = document.getElementById('<%= imgLogo.ClientID %>');
            img.src = "../Bootstrap_Files/dist/img/NoImage.jpg";
            alert('Invalid File Type, Select Only .png, .jpg, .jpge extension file');
        }

        function FuLogouploadComplete(sender, args) {
            document.getElementById('<%= Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= Img_Right.ClientID %>').style.display = ""; 

            <%--var img = document.getElementById('<%= imgLogo.ClientID %>');
            img.src = "<%=ResolveUrl(FuLogoUploadFolderPath) %>" + args.get_fileName();--%>
        }

        function UploadTemplate_UploadComplete(sender, args) {
            document.getElementById('<%= UploadTemplate_Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= UploadTemplate_Img_Right.ClientID %>').style.display = "";
        }
        function UploadTemplate_UploadError(sender, args) {
            document.getElementById('<%= UploadTemplate_Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= UploadTemplate_Img_Wrong.ClientID %>').style.display = "";
            alert('Invalid File Type, Select Only .html extension file');
        }
        function UploadTemplate_UploadStarted(sender, args) {
            var filename = args.get_fileName();
            debugger;
            var filext = filename.substring(filename.lastIndexOf(".") + 1);
            filext = filext.toLowerCase();
            //if (filext == "html") {
            //    return true;
            //}
            if (filext == "png" || filext == "jpg" || filext == "JPEG") {
                return true;
            }
            else {
                throw {
                    name: "Invalid File Type",
                    level: "Error",
                    message: "Invalid File Type, Select Only .html extension file",
                    htmlMessage: "Invalid File Type, Select Only .html extension file"
                }
                return false;
            }
        }
    </script>
</asp:Content>
