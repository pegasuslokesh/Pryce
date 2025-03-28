<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="OCStock.aspx.cs" Inherits="Inventory_OCStock" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-box-open"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Opening Stock%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Opening Stock%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="btnList_Click" Text="List" />
            <asp:Button ID="Btn_Upload" Style="display: none;" runat="server" Text="Upload" />
            <asp:Button ID="Btn_Serial_Number" Style="display: none;" runat="server" data-toggle="modal" data-target="#Serial_Number" Text="View Modal" />
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
                    <li id="Li_List" class="active"><a href="#List" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a>
                    </li>
                    <li id="Li_Upload"><a href="#Upload" onclick="Li_Tab_Upload()" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label33" runat="server" Text="<%$ Resources:Attendance,Upload %>"></asp:Label></a>
                    </li>
                </ul>




                <div class="tab-content">
                    <div class="tab-pane active" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="ImageButtonexport" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblBrandsearch" runat="server" Text="<%$ Resources:Attendance,Manufacturing Brand %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlbrandsearch" runat="server" CssClass="form-control">
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lbllocationsearch" runat="server" Text="<%$ Resources:Attendance,Category %>" />
                                                        <asp:DropDownList ID="ddlcategorysearch" runat="server" CssClass="form-control">
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btngo" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Go %>"
                                                            CssClass="btn btn-primary" OnClick="btngo_Click" />

                                                        <asp:Button ID="btnResetSreach" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Reset %>"
                                                            CssClass="btn btn-primary" OnClick="btnResetSreach_Click" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center;">
                                                        <br />
                                                        <br />
                                                        <asp:Button ID="btnPost" runat="server" Text="<%$ Resources:Attendance,Post %>" CssClass="btn btn-info" Style="margin-top: -27px;" OnClick="btnPost_Click" />

                                                        <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="Are you Sure to Post the Record ?"
                                                            TargetControlID="btnPost">
                                                        </cc1:ConfirmButtonExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" runat="server" visible="false">
                                                        <asp:Label runat="server" Text="Browse Excel File" ID="Label66"></asp:Label>
                                                        <div class="input-group" style="width: 100%;">
                                                            <cc1:AsyncFileUpload ID="fileLoadOLD"
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
                                                        <asp:Button ID="btnConnect" CssClass="btn btn-primary" runat="server" OnClick="btnConnect_Click" Visible="true" Text="<%$ Resources:Attendance,Update %>" />
                                                        <br />
                                                    </div>




                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div1" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				 <asp:Label ID="lblTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                                                <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Product Name %>" Value="EProductName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Product Id %>" Value="ProductCode"></asp:ListItem>
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
                                                <div class="col-lg-2">
                                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                        <asp:TextBox ID="txtValue" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:ImageButton ID="btnbind" runat="server" CausesValidation="False" Style="margin-top: -5px;"
                                                        ImageUrl="~/Images/search.png" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>

                                                    <asp:ImageButton ID="btnRefresh" runat="server" CausesValidation="False" Style="width: 33px;"
                                                        ImageUrl="~/Images/refresh.png" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>



                                                    <asp:ImageButton ID="ImageButtonexport" runat="server" CausesValidation="False" Style="width: 33px;"
                                                        ImageUrl="~/Images/xls-icon.png" OnClick="btnExport_Click" ToolTip="Export"></asp:ImageButton>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid">
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div style="overflow: auto; max-height: 500px;">
                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvProductStock" runat="server" AutoGenerateColumns="False" Width="100%"
                                                        ShowHeader="true">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Id %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductcodeSerial" runat="server" Text='<%# Eval("ProductId") %>'></asp:Label>
                                                                    <%-- <%# Container.DataItemIndex+1 %>--%>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="5%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductcode" runat="server" Text='<%# Eval("ProductCode") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="15%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductName" runat="server" Text='<%# Eval("EProductName") %>'></asp:Label>
                                                                    <asp:HiddenField ID="HdnProductId" runat="server" Value='<%# Eval("ProductId") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="50%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Name %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUnitName" runat="server" Text='<%# Eval("Unitname") %>'></asp:Label>
                                                                    <asp:HiddenField ID="HdnUnitId" runat="server" Value='<%# Eval("UnitId") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="5%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Cost %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUnitPrice" runat="server" Text='<%# SetDecimal(Eval("UnitPrice").ToString()) %>' Visible='<%# Eval("lblQtyVisibleStatus") %>'></asp:Label>
                                                                    <asp:TextBox ID="txtUnitPrice" runat="server" Text='<%# SetDecimal(Eval("UnitPrice").ToString()) %>' Visible='<%# Eval("txtQtyVisibleStatus") %>' CssClass="form-control"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FiltergvSalesQuantity" runat="server" Enabled="True"
                                                                        TargetControlID="txtUnitPrice" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="12%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quantity%>">
                                                                <ItemTemplate>
                                                                    <div class="input-group">
                                                                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control"
                                                                            Text='<%# Eval("Quantity") %>' Visible='<%# Eval("txtQtyVisibleStatus") %>' Enabled='<%# Eval("txtQtyEnableStatus") %>'></asp:TextBox>
                                                                        <asp:Label ID="lblQuantity" runat="server" Text='<%# SetDecimal(Eval("Quantity").ToString()) %>' Visible='<%# Eval("lblQtyVisibleStatus") %>'></asp:Label>
                                                                        <div class="input-group-btn">
                                                                            <asp:LinkButton ID="lnkAddSerial" runat="server" ToolTip="<%$ Resources:Attendance,Add %>" Visible='<%# Eval("lnkserialbtnVisibleStatus") %>' OnCommand="lnkAddSerial_Command" Font-Underline="true" CommandArgument='<%# Eval("ProductId") %>'><i class="fa fa-plus-square" style="font-size:35px;padding:5px;margin-top: -5px;"></i></asp:LinkButton>
                                                                        </div>
                                                                    </div>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="12%" />
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
                                                                    NavigateUrl="~/CompanyResource/OpeningStock.xlsx" Text="Download sample format for update information" Font-Underline="true"></asp:HyperLink>
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
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="Serial_Number" tabindex="-1" role="dialog" aria-labelledby="Serial_NumberLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Serial_NumberLabel">
                        <asp:Label ID="Label2" runat="server" Font-Size="14px" Font-Bold="true"
                            Text="<%$ Resources:Attendance, Serial No %>"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Serial_Number" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblproductId" runat="server" Text="Product Id"></asp:Label>
                                                    <asp:Label ID="lblProductIdvalue" runat="server" Text="0"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblserialProductname" runat="server" Text="Product Name"></asp:Label>
                                                    <asp:Label ID="lblProductNameValue" runat="server" Text="0"></asp:Label>
                                                    <br />
                                                </div>
                                                <div id="pnlSerialNumber" runat="server">
                                                    <div class="col-md-6" style="display: none">
                                                        <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Attendance, File Upload%>"></asp:Label>
                                                        <div class="input-group" style="width: 100%;">
                                                            <cc1:AsyncFileUpload ID="FULogoPath"
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
                                                    <div class="col-md-6" style="text-align: center; display: none">
                                                        <asp:Image ID="imgLogo" runat="server" Width="90px" Height="120px" /><br />
                                                        <br />
                                                        <asp:Button ID="Btnloadfile" runat="server" Text="Load" CssClass="btn btn-primary"
                                                            OnClick="Btnloadfile_Click" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="alert alert-info ">
                                                            <div class="row">
                                                                <div class="form-group">
                                                                    <div class="col-lg-3">
                                                                        <asp:TextBox ID="txtserachserialnumber" runat="server" CssClass="form-control"></asp:TextBox>
                                                                        <cc1:TextBoxWatermarkExtender ID="txtwatermarkup" runat="server" TargetControlID="txtserachserialnumber"
                                                                            WatermarkText="Search Serial Number">
                                                                        </cc1:TextBoxWatermarkExtender>
                                                                    </div>
                                                                    <div class="col-lg-3">
                                                                        <asp:ImageButton ID="btnsearchserial" runat="server" CausesValidation="False" Height="25px"
                                                                            ImageUrl="~/Images/search.png" OnClick="btnsearchserial_Click" Width="25px" ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>

                                                                        <asp:ImageButton ID="btnRefreshserial" runat="server" CausesValidation="False" Height="25px"
                                                                            ImageUrl="~/Images/refresh.png" OnClick="btnRefreshserial_Click" Width="25px"
                                                                            ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                                                    </div>
                                                                    <div class="col-lg-3">
                                                                        <asp:Label ID="Label17" runat="server" Text="Total :"></asp:Label>
                                                                        <asp:Label ID="txtselectedSerialNumber" runat="server" Text="0"></asp:Label>
                                                                    </div>
                                                                    <div class="col-lg-3">
                                                                        <asp:Label ID="lblCount" runat="server"></asp:Label>
                                                                        <asp:Label ID="txtCount" runat="server" Text="0"></asp:Label>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="box box-warning box-solid">
                                                            <div class="box-header with-border">
                                                                <h3 class="box-title"></h3>
                                                            </div>
                                                            <div class="box-body" style="height: 300px; overflow: auto">
                                                                <div class="row">
                                                                    <div class="col-md-8">
                                                                        <div style="overflow: auto">
                                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvSerialNumber" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                                                                AllowSorting="true" BorderStyle="Solid" Width="100%"
                                                                                PageSize="5" OnSorting="gvSerialNumber_OnSorting">
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("SerialNo") %>'
                                                                                                ImageUrl="~/Images/Erase.png" OnCommand="IbtnDeleteserialNumber_Command" Width="16px" />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Serial Number" SortExpression="SerialNo">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblsrno" runat="server" Text='<%#Eval("SerialNo") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Width">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblmfg" runat="server" Text='<%#Eval("Width") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Length">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblBatchNo" runat="server" Text='<%#Eval("Length") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Quantity">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblqty" runat="server" Text='<%#Eval("Quantity") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Pallet Id">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblpalletId" runat="server" Text='<%#Eval("Pallet_ID") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                </Columns>


                                                                                <PagerStyle CssClass="pagination-ys" />

                                                                            </asp:GridView>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <div class="flow">
                                                                            <asp:TextBox ID="txtSerialNo" Height="280px" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                </div>

                                                <div class="col-md-12" id="pnlexp_and_Manf" runat="server" visible="false" style="overflow: auto; max-height: 500px">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvStockwithManf_and_expiry" ShowHeader="true" runat="server" AutoGenerateColumns="false"
                                                        Width="100%" DataKeyNames="ProductId" ShowFooter="true" OnRowDeleting="gridView_RowDeleting"
                                                        OnRowCommand="gridView_RowCommand">

                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Quantity">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblQuantity" runat="server" Text='<%#setRoundValue(Eval("Quantity").ToString()) %>'></asp:Label>
                                                                    <asp:HiddenField ID="hdnProductId" runat="server" Value='<%#Eval("ProductId") %>' />
                                                                    <asp:HiddenField ID="hdnOrderId" runat="server" Value='<%#Eval("POID") %>' />
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                </EditItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtQuantity" runat="server" Font-Names="Verdana"
                                                                        CausesValidation="true" Width="250px"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="filtertextbox" runat="server" TargetControlID="txtQuantity"
                                                                        FilterType="Numbers">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </FooterTemplate>
                                                                <ItemStyle Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Expiry Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblExpiryDate" runat="server" Text='<%#setDateTime(Eval("ExpiryDate").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                </EditItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtExpiryFooter" runat="server" Font-Names="Verdana"
                                                                        Text='<%#Eval("ExpiryDate") %>' CausesValidation="true" Width="250px"></asp:TextBox>
                                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtExpiryFooter_CalenderExtender" runat="server" Enabled="True"
                                                                        TargetControlID="txtExpiryFooter" Format="dd-MMM-yyyy">
                                                                    </cc1:CalendarExtender>
                                                                </FooterTemplate>
                                                                <ItemStyle Width="8%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Manufacturing Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTaxValue" runat="server" Text='<%#setDateTime(Eval("ManufacturerDate").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                </EditItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtManfacturerFooter" runat="server" Font-Names="Verdana"
                                                                        Text='<%#Eval("ManufacturerDate") %>' CausesValidation="true" Width="250px"></asp:TextBox>
                                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtManfacturerFooter_CalenderExtender" runat="server" Enabled="True"
                                                                        TargetControlID="txtManfacturerFooter" Format="dd-MMM-yyyy">
                                                                    </cc1:CalendarExtender>
                                                                </FooterTemplate>
                                                                <ItemStyle Width="8%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <EditItemTemplate>
                                                                    <asp:Button ID="ButtonUpdate" runat="server" CommandName="Update" Text="Update" CausesValidation="true"
                                                                        CommandArgument='<%#Eval("Trans_Id") %>' />
                                                                    <asp:Button ID="ButtonCancel" runat="server" CommandName="Cancel" Text="Cancel" />
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="ButtonEdit" runat="server" CommandName="Edit" Text="Edit" Visible="false" />
                                                                    <asp:ImageButton ID="IbtnDeletePay" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        CommandName="Delete" ImageUrl="~/Images/Erase.png" ToolTip="<%$ Resources:Attendance,Delete %>"
                                                                        Width="16px" />
                                                                    <%--<asp:Button ID="ButtonDelete" runat="server" CommandName="Delete"  Text="Delete" CommandArgument='<%#Eval("Trans_Id") %>'  />--%>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:ImageButton ID="IbtnAddProductSupplierCode" runat="server" CausesValidation="False"
                                                                        CommandName="AddNew" Height="29px" ImageUrl="~/Images/add.png" Width="35px" ToolTip="<%$ Resources:Attendance,Add %>" />
                                                                    <%--<asp:Button ID="ButtonAdd" runat="server" CommandName="AddNew"  Text="Add New Row"  />--%>
                                                                </FooterTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
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
                <div class="modal-footer">
                    <asp:UpdatePanel ID="Update_Serial_Number_Button" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="BtnSerialSave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                CssClass="btn btn-success" OnClick="BtnSerialSave_Click" />

                            <asp:Button ID="btnResetSerial" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                CssClass="btn btn-primary" OnClick="btnResetSerial_Click" />

                            <asp:Button ID="btncloseserial" runat="server" Text="<%$ Resources:Attendance,Close %>" Visible="false"
                                CssClass="btn btn-danger" OnClick="btncloseserial_Click" />

                            <asp:Button ID="btnDefault" runat="server" Style="visibility: hidden" />

                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Serial_Number">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_Serial_Number_Button">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_List">
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
        function Serial_Number_Popup() {
            document.getElementById('<%= Btn_Serial_Number.ClientID %>').click();
        }
        function Serial_Number_Popup() {
            document.getElementById('<%= Btn_Serial_Number.ClientID %>').click();
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
        function Li_Tab_Upload() {
            document.getElementById('<%= Btn_Upload.ClientID %>').click();
        }
        function count(clientId) {
            var txtInput = document.getElementById(clientId);
            if (event.keyCode == 13) {
                document.getElementById('<%= txtCount.ClientID %>').innerHTML = lineBreakCount(txtInput.value);
            }
            if (event.keyCode == 8 || event.keyCode == 46) {
                document.getElementById('<%= txtCount.ClientID %>').innerHTML = lineDelBreakCount(txtInput.value);
              }
          }
          function lineBreakCount(str) {
              try {
                  return ((str.match(/[^\n]*\n[^\n]*/gi).length) + 1);
              } catch (e) {
                  return 1;
              }
          }
          function lineDelBreakCount(str) {
              try {
                  return ((str.match(/[^\n]*\n[^\n]*/gi).length - 1));
              } catch (e) {
                  return 0;
              }
          }
    </script>
    <script type="text/javascript">
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
                document.getElementById('<%= FULogo_Img_Wrong.ClientID %>').style.display = "none";
                document.getElementById('<%= FULogo_Img_Right.ClientID %>').style.display = "none";
                document.getElementById('<%= FUExcel_ImgLoader.ClientID %>').style.display = "block";

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

        function FuLogo_UploadComplete(sender, args) {
            document.getElementById('<%= FULogo_Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= FULogo_Img_Right.ClientID %>').style.display = "";

            var img = document.getElementById('<%= imgLogo.ClientID %>');
            img.src = "<%=ResolveUrl(FuLogo_UploadFolderPath) %>" + args.get_fileName();
        }
        function FuLogo_UploadError(sender, args) {
            document.getElementById('<%= FULogo_Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= FULogo_Img_Wrong.ClientID %>').style.display = "";
            var img = document.getElementById('<%= imgLogo.ClientID %>');
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

        function fnShowSerialPopup() {
            $('#Serial_Number').modal('show');
        }

    </script>

</asp:Content>
