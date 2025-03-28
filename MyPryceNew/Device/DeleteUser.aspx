<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="DeleteUser.aspx.cs" Inherits="Device_DeleteUser" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-user-times"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Delete User Setup  %>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Device%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Device%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Delete User%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:UpdatePanel ID="Update_New" runat="server">
        <ContentTemplate>
            <div id="Div_Main" runat="server" class="row">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="form-group">
                                <asp:RadioButton Visible="false" ID="rbtnAll" runat="server" CssClass="labelComman" Checked="True"
                                    GroupName="userop" Text="<%$ Resources:Attendance,All User%>" />
                                <asp:RadioButton Visible="false" ID="rbtnNew" runat="server" CssClass="labelComman" GroupName="userop"
                                    Text="<%$ Resources:Attendance,New User%>" />
                                <asp:Button ID="btnDownload" runat="server" OnClick="btnDownload_Click" ValidationGroup="Device" Text="<%$ Resources:Attendance,Download%>"
                                    CssClass="btn btn-primary" />
                                <asp:HiddenField ID="HDFSort" runat="server" />

                                <br />

                                <div class="row" id="Div_location" runat="server" visible="false">
                                    <div class="col-md-6">

                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Location %>"></asp:Label>
                                        <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged"></asp:DropDownList>

                                    </div>
                                </div>
                                <asp:CustomValidator ID="CustomValidator1" ValidationGroup="Device" runat="server" ErrorMessage="Please select at least one record."
                                    ClientValidationFunction="Validate_Device" ForeColor="Red"></asp:CustomValidator>

                                <div style="max-height: 500px; overflow: auto">

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
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Device Id %>" SortExpression="Device_Id">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDeviceId1" runat="server" Text='<%# Eval("Device_Id") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Device Name %>" SortExpression="Device_Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbDeviceName" runat="server" Text='<%# Eval("Device_Name") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Device Name(Local) %>" SortExpression="Device_Name_L">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDeviceNameL" runat="server" Text='<%# Eval("Device_Name_L") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,IP Address %>" SortExpression="IP_Address">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFromDate" runat="server" Text='<%# Eval("IP_Address") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Port Number %>" SortExpression="Port">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbltoDate" runat="server" Text='<%# Eval("Port") %>'></asp:Label>
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
                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Communication Type %>" SortExpression="Communication_Type">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbltoDate1" runat="server" Text='<%# Eval("Communication_Type") %>'></asp:Label>
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
            </div>
            <div id="Div_Delete" visible="false" runat="server" class="row">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="form-group">
                                <div style="text-align: left" class="col-md-6">
                                    <asp:LinkButton ID="lnkBackFromManage" CssClass="btn btn-primary" runat="server" OnClick="lnkBackFromManage_Click"
                                        Text="<%$ Resources:Attendance,Back%>"></asp:LinkButton>
                                    <asp:Button ID="btnSaveSelected" Width="200px" runat="server" CssClass="btn btn-primary"
                                        OnClick="btnDeleteSelected_Click" ValidationGroup="User_Delete" Text="<%$ Resources:Attendance,Delete Selected Record%>" />

                                </div>
                                <div style="text-align: center" class="col-md-6">
                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Delete With%>"></asp:Label>

                                    <asp:CheckBox Style="margin-left: 20px;" ID="chkUser" runat="server" Text="<%$ Resources:Attendance,User%>" />
                                    <asp:CheckBox Style="margin-left: 20px;" ID="chkFinger" runat="server" Text="<%$ Resources:Attendance,Finger%>" />
                                    <asp:CheckBox Style="margin-left: 20px;" ID="chkFace" runat="server" Text="<%$ Resources:Attendance,Face%>" />
                                    <asp:HiddenField ID="EditId" runat="server" />
                                    <br />
                                </div>
                            

                                <div class="col-md-12">
                                    <asp:CustomValidator ID="CustomValidator2" ValidationGroup="User_Delete" runat="server" ErrorMessage="Please select at least one record."
                                        ClientValidationFunction="Validate_User_Grid" ForeColor="Red"></asp:CustomValidator>
                                    <div class="alert alert-info ">
                                        <div class="row">
                                            <div class="form-group">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Device Name %>" Value="DeviceName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,User Id %>" Value="enrollNumber"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,User Name %>" Value="name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,IP Address %>" Value="IP"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Designation %>" Value="Designation"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Department %>" Value="Department"></asp:ListItem>
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
                                                    <asp:Panel ID="Panel3" runat="server" DefaultButton="btnbind">
                                                        <asp:TextBox ID="txtValue" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:ImageButton ID="btnbind" Style="margin-top: -5px;" runat="server" CausesValidation="False"
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
                                </div>



                                <div class="col-md-12">

                                    <div style="overflow: auto">

                                        <asp:UpdatePanel ID="Update_Grid" runat="server">
                                            <ContentTemplate>

                                                <div style="overflow: auto; max-height: 350px">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvUser" runat="server" AllowPaging="false"
                                                        AutoGenerateColumns="False" TabIndex="1" OnPageIndexChanging="gvUser_PageIndexChanging" OnSorting="gvUser_OnSorting"
                                                        AllowSorting="true"
                                                        Width="100%" DataKeyNames="enrollNumber,name,privilege,enabled,cardNumber,empId,password,IP,Port,deviceId"
                                                        PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'>

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

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Device Name %>">
                                                                <ItemStyle />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDevId" runat="server" Text='<%# Eval("deviceId") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="LabelName" runat="server" Text='<%# Eval("DeviceName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,IP Address %>">
                                                                <ItemStyle />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIp" runat="server" Text='<%# Eval("IP") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,User Id%>" SortExpression="enrollNumber">
                                                                <ItemStyle />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("enrollNumber") %>'></asp:Label>
                                                                    <asp:Label ID="lblEnrollNo" Visible="false" runat="server" Text='<%# Eval("empId") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,User Name%>" SortExpression="name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpName" runat="server" Text='<%# Eval("name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Designation %>" SortExpression="Designation">
                                                                <ItemStyle />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="bllldesg" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Department%>" SortExpression="Department">
                                                                <ItemStyle />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbldept" runat="server" Text='<%# Eval("Department") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                        </Columns>


                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

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

        function Validate_Device(sender, args) {
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

        function Validate_User_Grid(sender, args) {
            var gridView = document.getElementById("<%=gvUser.ClientID %>");
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


    </script>
</asp:Content>
