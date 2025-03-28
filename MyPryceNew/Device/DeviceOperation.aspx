<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="DeviceOperation.aspx.cs" Inherits="Device_DeviceOperation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-cog"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Device Operation Setup%>"></asp:Label>

    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Device%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Device Operation%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

        <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Modal_Popup" Style="display: none;" data-toggle="modal" data-target="#myModal" runat="server" Text="Modal Popup" />
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
                <div class="tab-content">
                    <div class="tab-pane active" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <ContentTemplate>
                                <div class="box box-warning box-solid" <%= gvDevice.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">

                                        <div class="row" id="Div_location" runat="server" visible="false">
                                            <div class="col-md-6">
                                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Location %>"></asp:Label>
                                                <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged"></asp:DropDownList>
                                                <br />
                                            </div>
                                        </div>


                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvDevice" DataKeyNames="Device_Id,Port,IP_Address,Device_Name,Communication_Type,field2" OnRowDataBound="gvDevice_OnRowDataBound"
                                                        PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server" AutoGenerateColumns="False"
                                                        Width="100%" AllowPaging="True" AllowSorting="True" OnPageIndexChanging="gvDevice_PageIndexChanging"
                                                        OnSorting="gvDevice_OnSorting">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemStyle />
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="LnkDeviceOp" runat="server" OnClick="LnkDeviceOp_Click" ImageUrl="~/Images/deviceopration.png"
                                                                        ToolTip="<%$ Resources:Attendance,Device Operation%>" />
                                                                </ItemTemplate>
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
                                                            <%-- <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>--%>
                                                        </Columns>


                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                </div>
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
                                                    <asp:LinkButton ID="lnkBackFromManage" runat="server" OnClick="lnkBackFromManage_Click"
                                                        Text="<%$ Resources:Attendance,Back%>"></asp:LinkButton>

                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Device Id %>"
                                                        Font-Bold="True"></asp:Label>
                                                    :
                                                                    <asp:Label ID="lblDeviceId" runat="server"></asp:Label>


                                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Device Name %>"
                                                        Font-Bold="True"></asp:Label>
                                                    :
                                                                    <asp:Label ID="lblDeviceWithId" runat="server"></asp:Label>

                                                    <asp:Button ID="btnSynctime" runat="server" OnClick="btnSynctime_Click" Text="<%$ Resources:Attendance,Sync Time%>"
                                                        CssClass="btn btn-primary" />

                                                    <asp:Button ID="btnClearLog" runat="server" OnClick="btnClearLog_Click" Text="<%$ Resources:Attendance,Clear Log%>"
                                                        CssClass="btn btn-primary" />


                                                    <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server"
                                                        ConfirmText="Are you sure you want to clear log from device ?"
                                                        Enabled="True" TargetControlID="btnClearLog">
                                                    </cc1:ConfirmButtonExtender>

                                                    <asp:Button ID="btnRestrat" runat="server" OnClick="btnRestrat_Click" Text="<%$ Resources:Attendance,Restart Device%>"
                                                        CssClass="btn btn-primary" />

                                                    <asp:Button ID="btnPowerOff" runat="server" OnClick="btnPowerOff_Click" Text="<%$ Resources:Attendance,Power Off Device%>"
                                                        CssClass="btn btn-primary" />


                                                    <asp:Button ID="btnInitialize" runat="server"   OnClick="btnInitialize_Click" Text="<%$ Resources:Attendance,Initialize Device%>"
                                                        CssClass="btn btn-primary" />
                                                    <cc1:ConfirmButtonExtender ID="btnInitialize_ConfirmButtonExtender" runat="server"
                                                        ConfirmText="Are you sure you want to intialize the device ?"
                                                        Enabled="True" TargetControlID="btnInitialize">
                                                    </cc1:ConfirmButtonExtender>

                                                    <asp:Button ID="btnClearadmin" runat="server" OnClick="btnClearadmin_Click" Text="<%$ Resources:Attendance,Clear Admin Privilege%>"
                                                        CssClass="btn btn-primary" />



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
                                <div class="alert alert-info ">
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-lg-3">
                                            </div>
                                            <div class="col-lg-2">
                                            </div>
                                            <div class="col-lg-2">
                                            </div>
                                            <div class="col-lg-3">
                                            </div>
                                            <div class="col-lg-2">
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
                                                    Grid
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
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only"><asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Close%>"></asp:Label></span></button>
                    <h4 class="modal-title" id="myModalLabel">
                        <asp:Label ID="Label9" runat="server" Text="Login Credential"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="update_Modal" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">

                                                <div class="col-md-12" >

                                                   <asp:Label ID="lbluserName" runat="server" Text="User Name"></asp:Label>
                                                    
                                                    <asp:TextBox ID=txtUserName runat=server class="form-control"  placeholder="Enter User Name"></asp:TextBox>
                                                </div>
                                                 <div class="col-md-12" >

                                                   <asp:Label ID="lblPassword" runat="server" Text="Password"></asp:Label>
                                                    
                                                    <asp:TextBox ID=txtPassword runat=server class="form-control" TextMode="Password"  placeholder="Enter Password"></asp:TextBox>
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
                    <asp:UpdatePanel ID="Update_Modal_Button" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnSave" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                Text="<%$ Resources:Attendance,Save %>" OnClick="btnSave_Click" />

                            <button type="button" class="btn btn-default" data-dismiss="modal">
                                <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Close%>"></asp:Label></button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
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

    <asp:Panel ID="pnlList" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="Panel1" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="Panel2" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="Panel3" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="Panel4" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="Panel5" runat="server" Visible="false"></asp:Panel>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
    <script type="text/javascript">

         function show_modal() {
            document.getElementById('<%=Btn_Modal_Popup.ClientID %>').click();
        }
        function Modal_Close() {
            document.getElementById('<%= Btn_Modal_Popup.ClientID %>').click();
        }



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
