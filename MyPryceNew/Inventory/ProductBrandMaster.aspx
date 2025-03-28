<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="ProductBrandMaster.aspx.cs" Inherits="Inventory_ProductBrandMaster" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-industry"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Manufacturing Brand%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Manufacturing Brand%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:Button ID="Btn_View_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#myModal" Text="View Modal" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
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
                                <asp:PostBackTrigger ControlID="btnExportExcel" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div1" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				 <asp:Label ID="lblTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                       
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Product Brand Name %>" Value="Brand_Name"
                                                            Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="Created_User"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Modified By %>" Value="Modified_User"></asp:ListItem>
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
                                                        <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbindrpt_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefreshReport_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnExportExcel" runat="server" CausesValidation="False" OnClick="btnExportExcel_Click" ToolTip="Export Excel"><span class="fas fa-file-csv"  style="font-size:25px;"></span></asp:LinkButton>
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
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvProductBrand" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="gvProductBrand_PageIndexChanging"
                                                        AllowSorting="True" OnSorting="gvProductBrand_Sorting">

                                                        <Columns>

                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle"  type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("PBrandId") %>' OnCommand="btnEdit_Command" CausesValidation="False"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("PBrandId") %>' OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>
                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:BoundField DataField="PBrandId" Visible ="false" HeaderText="<%$ Resources:Attendance,Product Brand Id %>"
                                                                SortExpression="PBrandId" />
                                                            <asp:BoundField DataField="Brand_Name" HeaderText="<%$ Resources:Attendance,Product Brand Name %>"
                                                                SortExpression="Brand_Name" ItemStyle-HorizontalAlign="Left" />
                                                            <asp:BoundField DataField="Brand_Name_L" HeaderText="<%$ Resources:Attendance,Product Brand Name(Local) %>"
                                                                SortExpression="Brand_Name_L" />
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="Created_User">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCreated_User" runat="server" Text='<%# Eval("Created_User") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>" SortExpression="Modified_User">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModified_User" runat="server" Text='<%# Eval("Modified_User") %>'></asp:Label>
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
                                    <!-- /.box-body -->
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
                                                        <asp:Label ID="lblBrandName" runat="server" Text="<%$ Resources:Attendance,Product Brand Name %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtBrandName" ErrorMessage="<%$ Resources:Attendance,Enter Product Brand Name%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtBrandName" runat="server" CssClass="form-control" AutoPostBack="true"
                                                            OnTextChanged="txtBrandName_TextChanged" BackColor="#eeeeee"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="txtBrandName_AutoCompleteExtender" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionList" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtBrandName"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                        <asp:Label ID="lblLBrandName" runat="server" Text="<%$ Resources:Attendance,Product Brand Name(Local) %>"></asp:Label>
                                                        <asp:TextBox ID="txtLBrandName" runat="server" CssClass="form-control" />
                                                        <br />
                                                        <asp:Label ID="lblLogoUpload" runat="server" Text="<%$ Resources:Attendance,Upload Brand Logo %>"></asp:Label>
                                                        <div class="input-group" style="width: 100%; display:none">
                                                            <cc1:AsyncFileUpload ID="FileUploadLogo"
                                                                OnClientUploadStarted="FuLogo_UploadStarted"
                                                                OnClientUploadError="FuLogo_UploadError"
                                                                OnClientUploadComplete="FuLogo_UploadComplete"
                                                                OnUploadedComplete="FuLogo_FileUploadComplete"
                                                                runat="server" CssClass="form-control"
                                                                CompleteBackColor="White"
                                                                UploaderStyle="Traditional"
                                                                UploadingBackColor="#CCFFFF"
                                                                ThrobberID="FULogo_ImgLoader" Width="100%" />
                                                            <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                                <asp:Image ID="FULogo_Img_Right" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Allow.png" Style="display: none" />
                                                                <asp:Image ID="FULogo_Img_Wrong" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Delete1.png" Style="display: none" />
                                                                <asp:Image ID="FULogo_ImgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div style="text-align: center" class="col-md-6">
                                                        <br />
                                                        <asp:Image ID="imgProduct" ClientIDMode="Static" runat="server" Height="120px" Width="120px" />
                                                        <br />
                                                         <br />&nbsp;
                                                            <a onclick="popup.Show();" runat="server" id="File_Manager_Product" class="btn btn-primary" style="cursor: pointer">File Manager</a>

                                                        <asp:Button ID="btnloadimg" Visible="false" runat="server" Style="margin-top: 5px;" CssClass="btn btn-info" Text="<%$ Resources:Attendance,Load %>" OnClick="btnloadimg_Click" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">

                                                        <asp:Label ID="lblDescription" runat="server" Text="<%$ Resources:Attendance,Description %>"></asp:Label>
                                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                                        <br />
                                                    </div>


                                                    <div class="col-md-12">
                                                        <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Width="100%" CssClass="ajax__tab_yuitabview-theme" OnClientActiveTabChanged="tabChanged">
                                                            <cc1:TabPanel ID="tabDescription" runat="server" HeaderText="<%$ Resources:Attendance,Description %>">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_Description" runat="server" UpdateMode="Conditional">
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="tabDescription" />
                                                                            <asp:AsyncPostBackTrigger ControlID="gvProductBrand" />
                                                                        </Triggers>
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <cc1:Editor ID="txtUnsuscribeUrl" runat="server" SuppressTabInDesignMode="false" />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Description">
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
                                                            <cc1:TabPanel ID="Tab_Header" runat="server" HeaderText="Template Header">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_Header" runat="server" UpdateMode="Conditional">
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="Tab_Header" />
                                                                            <asp:AsyncPostBackTrigger ControlID="gvProductBrand" />
                                                                        </Triggers>
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <cc1:Editor ID="EditorHeader" runat="server" SuppressTabInDesignMode="false" />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_Header">
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
                                                            <cc1:TabPanel ID="TabFooter" runat="server" HeaderText="Template Footer">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_Footer" runat="server" UpdateMode="Conditional">
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="TabFooter" />
                                                                            <asp:AsyncPostBackTrigger ControlID="gvProductBrand" />
                                                                        </Triggers>
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <cc1:Editor ID="EditorFooter" runat="server" SuppressTabInDesignMode="false" />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_Footer">
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
                                                        <asp:Button ID="btnCSave" runat="server" ValidationGroup="Save" Text="<%$ Resources:Attendance,Save %>"
                                                            CssClass="btn btn-success" OnClick="btnCSave_Click" />

                                                        <asp:Button ID="BtnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                            CssClass="btn btn-primary" CausesValidation="False" OnClick="BtnReset_Click" />

                                                        <asp:Button ID="BtnCCancel" runat="server" CssClass="btn btn-danger" Text="<%$ Resources:Attendance,Cancel %>"
                                                            CausesValidation="False" OnClick="BtnCCancel_Click" />
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
    // Check if e.file and e.file.extension are defined
                                  debugger;
    if (e.file.name) {
       
        var fileExtension = e.file.name.substring(e.file.name.lastIndexOf('.') + 1);

        // Check if the file is an image (you can customize the list of allowed extensions)
        var isImage = ['jpg', 'jpeg', 'png', 'gif'].includes(fileExtension);


        if (!isImage) {
            alert('Invalid file type. Only images are allowed.');
            return false;
        }
    } else {
        // Handle the case where e.file or e.file.extension is undefined
        alert('Unable to determine file type.');
        return false;
    }

    var image = $('#imgProduct')[0];
    image.src = e.file.imageSrc;

    var subproduct = s.currentPath;
    if (subproduct != '') {
        subproduct = '/' + subproduct;
        subproduct = subproduct.replaceAll('\\', '/');
    }

    var dataa = '~/' + s.rootFolderName + subproduct + '/' + e.file.name;
    dataa = dataa.replaceAll('/', '//');

    // Call the server-side method only if it's an image
    PageMethods.ASPxFileManager1_SelectedFileOpened(dataa, e.file.name, function(data) {}, function(data) {});

    popup.Hide();
    return false;
}" />

                                </dx:ASPxFileManager>

                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>
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
				 <asp:Label ID="lblTotalRecordsBin" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                <div class="col-lg-3">
                                                <asp:DropDownList ID="ddlFieldNameBin" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="<%$ Resources:Attendance, Product Brand Id %>" Value="PBrandId"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance, Product Brand Name %>" Value="Brand_Name"
                                                        Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="Created_User"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Modified By %>" Value="Modified_User"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2">
                                                <asp:DropDownList ID="ddlOptionBin" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-4">
                                                <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbindBin">
                                                    <asp:TextBox ID="txtValueBin" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                                </asp:Panel>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:LinkButton ID="btnbindBin" runat="server" CausesValidation="False"  OnClick="btnbindBin_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="btnRefreshBin" runat="server" CausesValidation="False" OnClick="btnRefreshBin_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="imgBtnRestore" CausesValidation="False"  runat="server" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>" ><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="ImgbtnSelectAll" Visible="false" runat="server" OnClick="ImgbtnSelectAll_Click"  ToolTip="<%$ Resources:Attendance, Select All %>" ><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>
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
                                                    <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvProductBrandMasterBin" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="gvProductBrandMasterBin_PageIndexChanging"
                                                        OnSorting="gvProductBrandMasterBin_OnSorting" AllowSorting="true">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkCurrent" runat="server" OnCheckedChanged="chkCurrent_CheckedChanged"
                                                                        AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelect_CheckedChanged" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Brand Id%>" SortExpression="PBrandId">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductBrandId" runat="server" Text='<%# Eval("PBrandId") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Brand Name %>" SortExpression="Brand_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductBrandName" runat="server" Text='<%# Eval("Brand_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="Created_User">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCreated_User" runat="server" Text='<%# Eval("Created_User") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>" SortExpression="Modified_User">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModified_User" runat="server" Text='<%# Eval("Modified_User") %>'></asp:Label>
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


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
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

        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }

        function View_Modal_Popup() {
            document.getElementById('<%= Btn_View_Modal.ClientID %>').click();
        }
        function Close_Modal_Popup() {
            document.getElementById('<%= Btn_View_Modal.ClientID %>').click();
        }
    </script>
    <script type="text/javascript">
        function FuLogo_UploadComplete(sender, args) {
            document.getElementById('<%= FULogo_Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= FULogo_Img_Right.ClientID %>').style.display = "";

            var img = document.getElementById('<%= imgProduct.ClientID %>');
            img.src = "<%=ResolveUrl(FuLogo_UploadFolderPath) %>" + args.get_fileName();
        }
        function FuLogo_UploadError(sender, args) {
            document.getElementById('<%= FULogo_Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= FULogo_Img_Wrong.ClientID %>').style.display = "";
            var img = document.getElementById('<%= imgProduct.ClientID %>');
            img.src = "../Bootstrap_Files/dist/img/NoImage.jpg";
            alert('Invalid File Type, Select Only .png, .jpg, .jpge extension file');
        }
        function FuLogo_UploadStarted(sender, args) {
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
        function tabChanged(sender, args) {
            // do what ever i want with lastTab value
            lasttab = sender.get_activeTabIndex();
        }
    </script>
</asp:Content>
