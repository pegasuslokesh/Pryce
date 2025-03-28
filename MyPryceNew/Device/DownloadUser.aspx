<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="DownloadUser.aspx.cs" Inherits="Device_DownloadUser" %>

<%@ Register Src="~/WebUserControl/TimeManLicense.ascx" TagPrefix="uc1" TagName="UpdateLicense" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-user"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Download User Setup%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Device%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Download User%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">
                <div class="tab-content">
                    <div class="tab-pane active" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <Triggers>

                                <asp:PostBackTrigger ControlID="btnDownload" />
                                <asp:PostBackTrigger ControlID="btnUpload" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">



                                                    <div id="Div_location" runat="server" visible="false" class="col-md-2">

                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Location %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged"></asp:DropDownList>

                                                    </div>
                                                    <div id="Div1" class="col-md-10">
                                                        <br />

                                                        <asp:CheckBox ID="chkDownloadFace" runat="server" Text="<%$ Resources:Attendance,Face%>" />
                                                        <asp:CheckBox ID="chkDownloadFinger" runat="server" Text="<%$ Resources:Attendance,Finger%>" />
                                                        <asp:Button ID="btnDownloadAllUser" Style="margin-left: 25px;" ValidationGroup="Save" runat="server" OnClick="btnDownload_Click" Text="<%$ Resources:Attendance,Browse All User%>"
                                                            CssClass="btn btn-primary" />


                                                        <asp:Button ID="btnDownloadNewUser" ValidationGroup="Save" runat="server" OnClick="btnDownload_Click" Text="<%$ Resources:Attendance,Browse New User%>"
                                                            CssClass="btn btn-primary" />



                                                        <asp:Button ID="btnsync" runat="server" OnClick="btnsync_Click" Text="<%$ Resources:Attendance,Device Syncronization%>"
                                                            CssClass="btn btn-primary" Visible="false" />


                                                        <asp:Button ID="btnconnectdevice" runat="server" OnClick="btnconnectdevice_Click" Text="<%$ Resources:Attendance,Connect Device%>" ValidationGroup="Save"
                                                            CssClass="btn btn-primary" />

                                                        <asp:Button ID="btnUserInfo" runat="server" OnClick="btnUserInfo_Click" Text="<%$ Resources:Attendance,User Information%>" ValidationGroup="Save"
                                                            CssClass="btn btn-primary" />

                                                        <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="<%$ Resources:Attendance,Cancel%>"
                                                            CssClass="btn btn-primary" />


                                                        <asp:HiddenField ID="HDFSort" runat="server" />
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div class="box box-primary box-solid" id="Div_Device_Download" runat="server">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Device List %>"></asp:Label>
                                        </h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i id="Btn_Div_General_Info" runat="server" class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">





                                        <asp:CustomValidator ID="CustomValidator1" ValidationGroup="Save" runat="server" ErrorMessage="Please select at least one record."
                                            ClientValidationFunction="Validate" ForeColor="Red"></asp:CustomValidator>
                                        <div class="row">
                                            <div class="col-md-12">

                                                <asp:RadioButton ID="rbtnAll" runat="server" Checked="True" Visible="false"
                                                    GroupName="userop" Text="<%$ Resources:Attendance,All User%>" />

                                                <asp:RadioButton ID="rbtnNew" Style="margin-left: 25px;" runat="server" GroupName="userop" Visible="false"
                                                    Text="<%$ Resources:Attendance,New User%>" />

                                                <div class="flow">

                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvDevice" DataKeyNames="Device_Id,Port,IP_Address,Device_Name,Communication_Type,field2"
                                                        PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server" AutoGenerateColumns="False"
                                                        Width="100%" AllowSorting="True" OnSorting="gvDevice_OnSorting">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelectDevice" runat="server" />
                                                                </ItemTemplate>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkSelAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelAll_CheckedChanged1" />
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>


                                                            <asp:TemplateField>
                                                                <ItemTemplate>

                                                                    <asp:LinkButton ID="LnkDeviceOp" runat="server" OnCommand="lnkViewDetail_Command" ToolTip="<%$ Resources:Attendance,Connect%>"><i class="fas fa-mobile" style="font-size:20px;"></i></asp:LinkButton>

                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>



                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Device Name %>" SortExpression="Device_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbDeviceName" runat="server" Text='<%# Eval("Device_Name") %>'></asp:Label>
                                                                    <asp:Label ID="lblDeviceId1" runat="server" Text='<%# Eval("Device_Id") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblCommunicationType" runat="server" Text='<%# Eval("Communication_Type") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblMake" runat="server" Text='<%# Eval("field2") %>' Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,IP Address %>" SortExpression="IP_Address">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblip" runat="server" Text='<%# Eval("IP_Address") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Port Number %>" SortExpression="Port">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblport" runat="server" Text='<%# Eval("Port") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Brand %>" SortExpression="Brand_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBrand2" runat="server" Text='<%# Eval("Brand_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Location %>" SortExpression="Location_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLoc2" runat="server" Text='<%# Eval("Location_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStatus" runat="server" Text="Disconnected"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,User Count%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUserCount" runat="server" Text="0"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Log Count%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLogCount" runat="server" Text="0"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Face Count%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFaceCount" runat="server" Text="0"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Finger Count%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFingerCount" runat="server" Text="0"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Password Count%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPasswordCount" runat="server" Text="0"></asp:Label>
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



                                <div class="box box-warning box-solid" id="Div_Sync" runat="server" visible="false">
                                    <div class="box-header with-border">
                                        <h3 class="box-title"></h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="row">

                                            <div class="col-md-6">
                                                <asp:CheckBox ID="chkuploadFace" runat="server" Text="Tranfer Face" />
                                                <asp:CheckBox ID="chkuploadFinger" runat="server" Text="Tranfer Finger" />
                                            </div>

                                            <div class="col-md-6">
                                                <asp:RadioButton ID="rbtnOverWrite" runat="server" Text="Overwrite Information" GroupName="Write" Checked="true" />
                                                <asp:RadioButton ID="rbtnAppend" runat="server" Text="Append Information" GroupName="Write" />
                                            </div>
                                            <div class="col-md-6">
                                                <div class="flow">

                                                    <asp:CustomValidator ID="CustomValidator2" ValidationGroup="Device_Sync" runat="server" ErrorMessage="Please select at least one record."
                                                        ClientValidationFunction="Validate_Source" ForeColor="Red"></asp:CustomValidator>

                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvSourceDevice" DataKeyNames="Device_Id,Port,IP_Address,Device_Name"
                                                        PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server" AutoGenerateColumns="False"
                                                        Width="100%" AllowSorting="false" OnSorting="gvDevice_OnSorting" AllowPaging="false">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelectDevice" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelectDevice_CheckedChanged" />
                                                                </ItemTemplate>

                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Device Name %>" SortExpression="Device_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbDeviceName" runat="server" Text='<%# Eval("Device_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,IP Address %>" SortExpression="IP_Address">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFromDate" runat="server" Text='<%# Eval("IP_Address") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                        </Columns>


                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                </div>
                                            </div>

                                            <div class="col-md-6">
                                                <div class="flow">
                                                    <asp:CustomValidator ID="CustomValidator3" ValidationGroup="Device_Sync" runat="server" ErrorMessage="Please select at least one record."
                                                        ClientValidationFunction="Validate_Destination" ForeColor="Red"></asp:CustomValidator>

                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvDestinationDevice" DataKeyNames="Device_Id,Port,IP_Address,Device_Name"
                                                        PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server" AutoGenerateColumns="False"
                                                        Width="100%" AllowSorting="false" OnSorting="gvDevice_OnSorting" AllowPaging="false">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelectDevice" runat="server" />
                                                                </ItemTemplate>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkSelAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelAllDestinationDevice_CheckedChanged1" />
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Device Name %>" SortExpression="Device_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbDeviceName" runat="server" Text='<%# Eval("Device_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,IP Address %>" SortExpression="IP_Address">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFromDate" runat="server" Text='<%# Eval("IP_Address") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                        </Columns>


                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                </div>
                                            </div>

                                            <div class="col-md-12" style="text-align: center;">
                                                <br />
                                                <asp:Button ID="btnTransfer" Style="margin-left: 25px;" runat="server" Text="Transfer"
                                                    CssClass="btn btn-primary" ValidationGroup="Device_Sync" />

                                            </div>



                                        </div>
                                    </div>
                                    <!-- /.box-body -->
                                </div>





                                <div id="pnlDeviceOp" runat="server" class="row">

                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Download With%>"></asp:Label>

                                                        <asp:CheckBox ID="chkFinger" runat="server" Text="<%$ Resources:Attendance,Finger%>" />
                                                        <asp:CheckBox ID="chkFace" runat="server" Text="<%$ Resources:Attendance,Face%>" />
                                                        <asp:HiddenField ID="EditId" runat="server" />
                                                    </div>
                                                    <div class="col-md-6" style="text-align: center;">
                                                        <asp:Button ID="btnSaveSelected" runat="server" CssClass="btn btn-primary" Visible="false"
                                                            OnClick="btnSaveSelected_Click" Text="<%$ Resources:Attendance,Download All User%>" />

                                                        <asp:Button ID="btnSaveSelected_1" runat="server" CssClass="btn btn-primary" Visible="false"
                                                            OnClick="btnSaveSelected_Click" Text="<%$ Resources:Attendance,Download User%>" />

                                                        <asp:Button ID="btnDownload" runat="server" Text="<%$ Resources:Attendance,Export%>" CssClass="btn btn-primary" OnClick="btnexport_Click" CausesValidation="False" />


                                                        <asp:Button ID="btnDownloadLog" runat="server" CssClass="btn btn-primary" Visible="false"
                                                            OnClick="btnDownloadLog_Click" Text="<%$ Resources:Attendance,Download Log%>" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <%--<div class="col-md-6">
                                                        <asp:LinkButton ID="lnkBackFromManage" runat="server" OnClick="lnkBackFromManage_Click"
                                                            Text="<%$ Resources:Attendance,Back%>"></asp:LinkButton>
                                                    </div>--%>

                                                    <%-- <div class="col-md-6">

                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,User Count%>"></asp:Label>
                                                        <b>:</b><asp:Label ID="lblUserCount" runat="server"></asp:Label>
                                                    </div>--%>


                                                    <div class="alert alert-info">
                                                        <div class="row">
                                                            <div class="form-group">

                                                                <div class="col-lg-3">
                                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Device Name %>" Value="DeviceName"></asp:ListItem>
                                                                        <asp:ListItem Text="<%$ Resources:Attendance,Employee Code %>" Value="enrollNumber"></asp:ListItem>
                                                                        <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="name"></asp:ListItem>
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
                                                                        <asp:TextBox ID="txtValue" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    </asp:Panel>

                                                                </div>
                                                                <div class="col-lg-2">
                                                                    <asp:ImageButton ID="btnbind" runat="server" Style="margin-top: -5px;" CausesValidation="False"
                                                                        ImageUrl="~/Images/search.png" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>
                                                                    <asp:ImageButton ID="btnRefresh" Style="width: 33px; margin-top: -5px;" runat="server" CausesValidation="False"
                                                                        ImageUrl="~/Images/refresh.png" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                                                </div>
                                                                <div class="col-lg-2">
                                                                    <h5>
                                                                        <asp:Label ID="lblTotalRecords" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>
                                                                    <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12" style="overflow: auto; max-height: 350px;" runat="server" id="divexport">
                                                        <br />
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvUser" runat="server" AutoGenerateColumns="False" DataKeyNames="enrollNumber,name,privilege,enabled,cardNumber,EmpId,password,IP,Port,deviceId,CommunicationType,deviceMake"
                                                            Width="100%" AllowPaging="false" AllowSorting="true" OnPageIndexChanging="gvUser_PageIndexChanging" OnSorting="gvUser_OnSorting">
                                                            <RowStyle />
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSel" runat="server" />
                                                                    </ItemTemplate>
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="chkSelAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelAll_CheckedChanged" />
                                                                    </HeaderTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <%-- <asp:TemplateField HeaderText="<%$ Resources:Attendance,Device Name %>">
                                                                    <ItemStyle Width="12%" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("DeviceName") %>'></asp:Label>
                                                                        
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>--%>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,IP Address %>">
                                                                    <ItemStyle Width="12%" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIp" runat="server" Text='<%# Eval("IP") %>'></asp:Label>
                                                                        <asp:Label ID="lblDevId" runat="server" Text='<%# Eval("deviceId") %>' Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <%--  <asp:TemplateField HeaderText="<%$ Resources:Attendance,Port No.%>">
                                                                    <ItemStyle Width="12%" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPort" runat="server" Text='<%# Eval("Port") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>--%>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code%>" SortExpression="sdwEnrollNumber">
                                                                    <ItemStyle Width="12%" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblsdwenrollNo" runat="server" Text='<%# Eval("enrollNumber") %>'></asp:Label>
                                                                        <asp:Label ID="lblEnrollNo" runat="server" Text='<%# Eval("empId") %>' Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name%>" SortExpression="sName">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEmpName" runat="server" Text='<%# Eval("name") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Card No.%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCardNo" runat="server" Text='<%# Eval("cardNumber") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Password %>" Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEmpId" runat="server" Text='<%# Eval("password") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Face%>">
                                                                    <ItemTemplate>

                                                                        <%--<asp:CheckBox ID="chkFace" runat="server" Checked='<%# Eval("Face") %>' />--%>
                                                                        <asp:Label ID="lblFace" runat="server" Text='<%# Eval("Face") %>'></asp:Label>

                                                                        <%--<asp:Image ID="img1" runat="server" ImageUrl='<%# "~/CompanyResource/Template/" +Eval("Field1") %>'
                                                                        Height="100px" Width="100px" />--%>
                                                                        <%--<asp:Image ID="img1" runat="server" ImageUrl='<%#GetFaceImage(Eval("Face").ToString())%>' AlternateText=""
                                                                            Height="30px" Width="30px" ToolTip="<%$ Resources:Attendance,Face%>" />--%>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Finger%>">
                                                                    <ItemTemplate>
                                                                        <%-- <asp:CheckBox ID="chkFinger" runat="server" Checked='<%# Eval("Finger") %>' />--%>
                                                                        <asp:Label ID="lblFinger" runat="server" Text='<%# Eval("Finger") %>'></asp:Label>
                                                                        <%--<asp:Image ID="img1" runat="server" ImageUrl='<%# "~/CompanyResource/Template/" +Eval("Field1") %>'
                                                                        Height="100px" Width="100px" />--%>
                                                                        <%--<asp:Image ID="img12" runat="server" ImageUrl='<%#GetFingerImage(Eval("Finger").ToString())%>' ToolTip="<%$ Resources:Attendance,Finger%>"
                                                                            Height="40px" Width="40px" />--%>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                            </Columns>


                                                            <PagerStyle CssClass="pagination-ys" />

                                                        </asp:GridView>

                                                    </div>

                                                    <div class="col-md-6" style="display: none;">
                                                        <br />
                                                        <asp:Label ID="lblGroupByDept" runat="server" Text="<%$ Resources:Attendance,Device %>"></asp:Label>
                                                        <asp:ListBox ID="listEmpDevice" SelectionMode="Multiple" runat="server" Style="width: 100%; height: 150px;"></asp:ListBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4" style="display: none;">

                                                        <br />
                                                        <br />
                                                        <asp:Button ID="btnUpload" ValidationGroup="Save" runat="server" CssClass="btn btn-primary" OnClick="btnUpload_Click"
                                                            Text="<%$ Resources:Attendance,Upload%>" />

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
    <div class="modal fade" id="ModelUpdateLicense" role="dialog">
        <div class="modal-dialog modal-lg">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <div class="modal-title" style="text-align: left;">
                        <img src="https://www.pegasustech.net/image/catalog/logo.png" class="img-responsive" width="120px" />
                    </div>
                </div>

                <div>
                    <uc1:UpdateLicense ID="UC_LicenseInfo" runat="server" />
                </div>
                <div class="modal-footer">
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
       <%-- function Li_Tab_List() {
            document.getElementById('<%= Btn_List.ClientID %>').click();
        }
        function Li_Tab_New() {
            document.getElementById('<%= Btn_New.ClientID %>').click();
        }
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }--%>
        function LI_Edit_Active() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }




        function Validate_Source(sender, args) {
            var gridView = document.getElementById("<%=gvSourceDevice.ClientID %>");
            var checkBoxes = gridView.getElementsByTagName("input");
            for (var i = 0; i < checkBoxes.length; i++) {
                if (checkBoxes[i].type == "checkbox" && checkBoxes[i].checked) {
                    args.IsValid = true;
                    return;
                }
            }
            args.IsValid = false;
        }



        function Validate_Destination(sender, args) {
            var gridView = document.getElementById("<%=gvDestinationDevice.ClientID %>");
            var checkBoxes = gridView.getElementsByTagName("input");
            for (var i = 0; i < checkBoxes.length; i++) {
                if (checkBoxes[i].type == "checkbox" && checkBoxes[i].checked) {
                    args.IsValid = true;
                    return;
                }
            }
            args.IsValid = false;
        }



        function Validate(sender, args) {
            var gridView = document.getElementById("<%=gvDevice.ClientID %>");
            var checkBoxes = gridView.getElementsByTagName("input");
            for (var i = 0; i < checkBoxes.length; i++) {
                if (checkBoxes[i].type == "checkbox" && checkBoxes[i].checked) {
                    args.IsValid = true;
                    return;
                }
            }
            args.IsValid = false;
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

        function Modal_UpdateLicense_Open() {
            $('#ModelUpdateLicense').modal('show')
        }

    </script>
</asp:Content>

