<%@ Page Title="" Language="C#" EnableEventValidation="false" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="CityMaster.aspx.cs" Inherits="MasterSetUp_CityMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-flag-usa"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,City Master%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Master Setup%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,City Master%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="btn_fillGrid" Style="display: none;" runat="server" OnClick="btn_fillGrid_Click" />
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
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label28" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
                    <li id="Li_Upload"><a onclick="Li_Tab_New()" href="#Upload" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_Upload" runat="server" Text="<%$ Resources:Attendance,Upload%>"></asp:Label>
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
                                <div class="box box-warning box-solid">
                                    <div class="box-body">
                                        <div class="row">
                                            <asp:HiddenField ID="hdnTrans_id" runat="server" />

                                            <div class="col-md-4">
                                                <asp:Label ID="lblCountryName" runat="server" Text="<%$ Resources:Attendance,Country Name%>"></asp:Label>
                                                <asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-control" onchange="OnCountryChange(this);"></asp:DropDownList>
                                            </div>

                                            <div class="col-md-4">
                                                <asp:Label ID="lblStateName" runat="server" Text="<%$ Resources:Attendance,State Name%>"></asp:Label>
                                                <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control" onchange="OnStateChange(this);"></asp:DropDownList>
                                                <asp:HiddenField ID="hdnState_Id" runat="server" />
                                            </div>

                                            <div class="col-md-4">
                                                <asp:Label ID="lblCityName" runat="server" Text="<%$ Resources:Attendance,City Name%>"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtCityName" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:Label ID="lblCityNameLocal" runat="server" Text="<%$ Resources:Attendance,City Name Local%>"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtCityNameLocal" CssClass="form-control"></asp:TextBox>
                                            </div>

                                            <div class="col-md-12">
                                                <br />
                                            </div>
                                            <div class="col-md-12">
                                                <asp:Button runat="server" ID="btnSave" Text="<%$ Resources:Attendance,Save%>" OnClick="btnSave_Click" CssClass="btn btn-success" Visible="false" />
                                                <asp:Button runat="server" ID="btnReset" Text="<%$ Resources:Attendance,Reset%>" OnClick="btnReset_Click" CssClass="btn btn-primary" />
                                                <asp:Button runat="server" ID="btnCancel" Text="<%$ Resources:Attendance,Cancel%>" OnClientClick="btnCancelClick();" CssClass="btn btn-danger" />
                                            </div>
                                            <div class="col-md-12">
                                                <br />
                                            </div>
                                            <div class="col-md-12">

                                                <div class="flow">

                                                    <dx:ASPxGridView ID="GvCityMaster" Width="100%" EnableViewState="false" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False" KeyFieldName="Trans_Id">
                                                        <Columns>

                                                            <dx:GridViewDataColumn VisibleIndex="3" Visible="false">
                                                                <DataItemTemplate>
                                                                    <a title="Edit" onclick="btnEditClick('<%# Eval("Trans_Id") %>');" style="cursor: pointer"><i class="fa fa-edit" style="font-size: 15px"></i></a>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>

                                                            <dx:GridViewDataColumn VisibleIndex="4" Visible="false">
                                                                <DataItemTemplate>
                                                                    <a title="Delete" onclick="btnDeleteClick('<%# Eval("Trans_Id") %>');" style="cursor: pointer"><i class="fa fa-trash" style="font-size: 15px"></i></a>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>


                                                            <dx:GridViewDataTextColumn FieldName="Country_Name" Settings-AutoFilterCondition="Contains" Caption="<%$ Resources:Attendance,Country Name%>" VisibleIndex="6">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="State_Name" Settings-AutoFilterCondition="Contains" Caption="<%$ Resources:Attendance,State Name%>" VisibleIndex="7">
                                                            </dx:GridViewDataTextColumn>


                                                            <dx:GridViewDataTextColumn FieldName="City_Name" Settings-AutoFilterCondition="Contains" Caption="<%$ Resources:Attendance,City Name%>" VisibleIndex="13">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="City_Name_Local" Settings-AutoFilterCondition="Contains" Caption="<%$ Resources:Attendance,City Name Local%>" VisibleIndex="13">
                                                            </dx:GridViewDataTextColumn>

                                                        </Columns>

                                                        <Settings ShowGroupPanel="true" ShowFilterRow="true" />
                                                        <SettingsCommandButton>
                                                            <EditButton>
                                                                <Image ToolTip="Edit" Url="~/Images/edit.png" />
                                                            </EditButton>
                                                        </SettingsCommandButton>
                                                        <Styles>
                                                            <CommandColumn Spacing="2px" Wrap="False" />
                                                        </Styles>

                                                    </dx:ASPxGridView>


                                                </div>
                                                <br />

                                            </div>

                                        </div>
                                    </div>
                                    <!-- /.box-body -->
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="Upload">
                        <asp:UpdatePanel ID="Update_New" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <asp:UpdatePanel ID="Update_Upload" runat="server">
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="DownloadSampleExcel" />
                                                            <asp:PostBackTrigger ControlID="btndownloadInvalid" />

                                                        </Triggers>
                                                        <ContentTemplate>
                                                            <div class="row">
                                                                <div class="col-md-12">

                                                                    <div class="box-body">
                                                                        <div class="form-group">


                                                                            <div id="Div_device_Information" runat="server" class="col-md-12">



                                                                                <div class="row" id="Div_device_upload_operation" runat="server">
                                                                                    <div class="col-md-12" style="text-align: center;">

                                                                                        <br />
                                                                                        <asp:LinkButton ID="DownloadSampleExcel" runat="server" Font-Bold="true" Font-Size="15px" Text="<%$ Resources:Attendance,Download sample format for update information%>" Font-Underline="true" OnClick="DownloadSampleExcel_Click"></asp:LinkButton>
                                                                                        <br />

                                                                                    </div>
                                                                                    <div class="col-md-6" style="text-align: center;">
                                                                                        <br />
                                                                                        <asp:Label runat="server" Text="<%$ Resources:Attendance,Browse Excel File%>" ID="Label169"></asp:Label>
                                                                                        <div class="input-group" style="width: 100%;">
                                                                                            <cc1:AsyncFileUpload ID="fileLoad" OnUploadedComplete="FileUploadComplete" OnClientUploadError="uploadError" OnClientUploadStarted="uploadStarted" OnClientUploadComplete="uploadComplete"
                                                                                                runat="server" CssClass="form-control" CompleteBackColor="White" UploaderStyle="Traditional" UploadingBackColor="#CCFFFF" ThrobberID="imgLoader" Width="100%" />
                                                                                            <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                                                                <asp:Image ID="Img_Right" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Allow.png" Style="display: none" />
                                                                                                <asp:Image ID="Img_Wrong" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Delete1.png" Style="display: none" />
                                                                                                <asp:Image ID="imgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                                                            </div>

                                                                                        </div>
                                                                                        <br />

                                                                                        <asp:Button ID="btnGetSheet" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                                                                                            OnClick="btnGetSheet_Click" Visible="false" Text="<%$ Resources:Attendance,Connect To DataBase %>" />

                                                                                    </div>
                                                                                    <div class="col-md-6" style="text-align: center;">
                                                                                        <br />
                                                                                        <asp:Label runat="server" Text="<%$ Resources:Attendance,Select Sheet%>" ID="Label170"></asp:Label>
                                                                                        <asp:DropDownList ID="ddlTables" CssClass="form-control" runat="server">
                                                                                        </asp:DropDownList>
                                                                                        <br />
                                                                                        <asp:Button ID="btnConnect" CssClass="btn btn-primary" runat="server" CausesValidation="False"
                                                                                            OnClick="btnConnect_Click" Visible="false" Text="<%$ Resources:Attendance,Get Record%>" />
                                                                                        </br>
                                                             <br />
                                                                                    </div>
                                                                                </div>

                                                                                <div class="row" id="uploadEmpdetail" runat="server" visible="false">

                                                                                    <div class="col-md-6" style="text-align: left">
                                                                                        <asp:RadioButton ID="rbtnupdall" Style="margin-left: 20px; margin-right: 20px;" runat="server" GroupName="upd" AutoPostBack="true" Checked="true" OnCheckedChanged="rbtnupdall_OnCheckedChanged" Text="<%$ Resources:Attendance,All%>" />
                                                                                        <asp:RadioButton ID="rbtnupdValid" Style="margin-left: 20px; margin-right: 20px;" runat="server" GroupName="upd" AutoPostBack="true" Text="<%$ Resources:Attendance,Valid%>" OnCheckedChanged="rbtnupdall_OnCheckedChanged" />
                                                                                        <asp:RadioButton ID="rbtnupdInValid" Style="margin-left: 20px; margin-right: 20px;" runat="server" GroupName="upd" AutoPostBack="true" Text="<%$ Resources:Attendance,Invalid%>" OnCheckedChanged="rbtnupdall_OnCheckedChanged" />
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



                                                                                    <div class="col-md-6" style="text-align: center">
                                                                                        <br />



                                                                                        <asp:Button ID="btnUploadEmpInfo" runat="server" CssClass="btn btn-primary" OnClick="btnUploadEmpInfo_Click"
                                                                                            Text="<%$ Resources:Attendance,Upload Data %>" />

                                                                                        <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" TargetControlID="btnUploadEmpInfo"
                                                                                            ConfirmText="Are you sure to Save Records in Database.">
                                                                                        </cc1:ConfirmButtonExtender>
                                                                                        <asp:Button ID="btnResetEmpInfo" runat="server" CssClass="btn btn-primary" OnClick="btnResetEmpInfo_Click"
                                                                                            Text="<%$ Resources:Attendance,Reset %>" />


                                                                                        <asp:Button ID="btndownloadInvalid" CssClass="btn btn-primary" runat="server" Text="<%$ Resources:Attendance,Download Invalid Record%>" CausesValidation="False"
                                                                                            OnClick="btndownloadInvalid_Click" />
                                                                                    </div>
                                                                                </div>
                                                                            </div>




                                                                            <div id="pnlMap" runat="server" class="col-md-12" style="display: none;">
                                                                                <br />
                                                                                <div style="overflow: auto">
                                                                                </div>

                                                                            </div>

                                                                            <br />
                                                                            <div id="pnlshowdata" runat="server" class="col-md-12" style="display: none;">
                                                                                <br />
                                                                                <div class="col-md-6">
                                                                                    <asp:DropDownList ID="ddlFiltercol" CssClass="form-control" runat="server">
                                                                                    </asp:DropDownList>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:TextBox ID="txtfiltercol" CssClass="form-control" runat="server"></asp:TextBox>
                                                                                    <br />
                                                                                </div>

                                                                            </div>

                                                                            <div class="col-md-12" style="overflow: auto; display: none;">
                                                                                <br />
                                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvSelected1" Width="100%" runat="server">

                                                                                    
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
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server">
                            <ContentTemplate>



                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div1" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
                                                <asp:Label ID="lblTotalRecordsBin" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldNameBin" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Country Name%>" Value="Country_Name" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,State Name%>" Value="State_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,City Name%>" Value="City_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,City Name Local%>" Value="City_Name_Local"></asp:ListItem>
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
                                                <div class="col-lg-3">
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbindBin">
                                                        <asp:TextBox ID="txtValueBin" runat="server" CssClass="form-control" placeholder="Search from Content"   	></asp:TextBox>
                                                        <asp:TextBox ID="txtCustValueBin" runat="server" CssClass="form-control" Visible="false"  placeholder="Search from Content"   ></asp:TextBox>
                                                        <asp:TextBox ID="txtValueBinDate" runat="server" CssClass="form-control" Visible="false" placeholder="Search from Date"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueBinDate" runat="server" TargetControlID="txtValueBinDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbindBin" runat="server" CausesValidation="False" OnClick="btnbindBin_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefreshBin" runat="server" CausesValidation="False"  OnClick="btnRefreshBin_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imgBtnRestore" CausesValidation="False" Visible="false" runat="server"  OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>" >                                                   <span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div class="box box-warning box-solid" <%= gvCityMasterBin.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" runat="server" ID="gvCityMasterBin" AutoGenerateColumns="false" Width="100%" AllowPaging="True" OnPageIndexChanging="gvCityMasterBin_PageIndexChanging" AllowSorting="True" OnSorting="gvCityMasterBin_Sorting">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox runat="server" ID="chkgvSelectAll" OnCheckedChanged="chkSelectAll_CheckedChanged" AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox runat="server" ID="chkgvSelect" OnCheckedChanged="chkSelect_CheckedChanged" AutoPostBack="true" />
                                                                    <asp:HiddenField ID="hdnTrans_Id" runat="server" Value='<%# Eval("Trans_Id") %>' />

                                                                </ItemTemplate>
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Country Name%>" SortExpression="Country_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="gvlblCountryName" Text='<%# Eval("Country_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,State Name%>" SortExpression="State_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="gvlblStateName" Text='<%# Eval("State_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,City Name%>" SortExpression="City_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="gvlblCityName" Text='<%# Eval("City_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,City Name Local%>" SortExpression="State_Name_Local">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="gvlblCityNameLocal" Text='<%# Eval("City_Name_Local") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <asp:HiddenField ID="HDFSortbin" runat="server" />
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                    <!-- /.box-body -->
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

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
    <script type="text/javascript">

        function btnEditClick(TransId) {
            PageMethods.GetCityByTrans_Id(TransId, onSuccessCallBack, onFailCallBack);
        }

        function onSuccessCallBack(result) {

            document.getElementById('<%= ddlCountry.ClientID %>').value = result["Country_Id"];
            document.getElementById('<%= txtCityName.ClientID %>').value = result["City_Name"];
            document.getElementById('<%= txtCityNameLocal.ClientID %>').value = result["City_Name_Local"];
            document.getElementById('<%= hdnTrans_id.ClientID %>').value = result["TransId"];

            var ddlstate = document.getElementById('<%= ddlState.ClientID %>');
            ddlstate.innerHTML = "";

            PageMethods.FillState(result["Country_Id"], result["State_Id"], onSuccessCountryChanged);


            document.getElementById('<%= hdnState_Id.ClientID %>').value = result["State_Id"];
        }
        function OnStateChange(stateId) {
            document.getElementById('<%= hdnState_Id.ClientID %>').value = stateId.value;
        }

        function onFailCallBack(errors) {
            alert(errors);
        }

        function btnDeleteClick(TransId) {
            PageMethods.DeleteByTrans_Id(TransId, onSuccessDeleteCallBack, onFailCallBack);
        }

        function onSuccessDeleteCallBack(result) {
            if (result == "1") {
                alert("Record Deleted Successfully");
                document.getElementById('<%= btn_fillGrid.ClientID %>').click();
            }
            else {
                alert("Cant delete this record as it has been used");
                return;
            }
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

        function btnCancelClick() {
            document.getElementById('<%= ddlCountry.ClientID %>').value = "";
            document.getElementById('<%= ddlState.ClientID %>').value = "";
            document.getElementById('<%= txtCityName.ClientID %>').value = "";
            document.getElementById('<%= txtCityNameLocal.ClientID %>').value = "";
            document.getElementById('<%= hdnTrans_id.ClientID %>').value = "";
        }

        function OnCountryChange(id) {
            document.getElementById('<%= txtCityName.ClientID %>').value = "";
            document.getElementById('<%= txtCityNameLocal.ClientID %>').value = "";
            PageMethods.FillState(id.value, "0", onSuccessCountryChanged);
        }
        function onSuccessCountryChanged(response) {

            var stateid = document.getElementById('<%= hdnState_Id.ClientID %>');
            stateid.value = "";

            var ddlIPLtXML = document.getElementById('<%= ddlState.ClientID %>');
            ddlIPLtXML.innerHTML = "";
            var tableName = "table1";
            var count = 0;
            $(response).find(tableName).each(function () {
                if (count == 0) {
                    stateid.value = $(this).find('Trans_Id').text();
                }
                count++;
                var OptionValue = $(this).find('Trans_Id').text();
                var OptionText = $(this).find('State_Name').text();
                var option = document.createElement("option");
                option.value = OptionValue;
                option.innerHTML = OptionText;
                ddlIPLtXML.add(option);
                ddlIPLtXML.append(option);

            });

        }
    </script>

    <script type="text/javascript">
        function uploadComplete(sender, args) {
            document.getElementById('<%= Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= Img_Right.ClientID %>').style.display = "";
        }
        function uploadError(sender, args) {
            document.getElementById('<%= Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= Img_Wrong.ClientID %>').style.display = "";
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
</asp:Content>

