<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="DeviceDashboard.aspx.cs" Inherits="Device_DeviceMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/device.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Device Dashboard%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Device%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Device Dashboard%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="btnList_Click" Text="List" />

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

                    <li id="Li_List" class="active"><a href="#List" onclick="Li_Tab_List()" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane active" id="List">
                        <asp:Timer runat="server" ID="UpdateTimer" Interval="60000" OnTick="UpdateTimer_Tick" />
                        <asp:UpdatePanel ID="Update_List" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="UpdateTimer" EventName="Tick" />
                            </Triggers>
                            <ContentTemplate>


                                <div class="row" id="Div_location" runat="server" visible="false">


                                    <div class="col-md-6">
                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Location %>"></asp:Label>
                                        <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged"></asp:DropDownList>
                                        <br />
                                    </div>
                                </div>
                                <div class="alert alert-info " hidden="hidden">

                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control" Visible="false">
                                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Device Name %>" Value="Device_Name"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Device Name(Local) %>" Value="Device_Name_L"></asp:ListItem>


                                                    <asp:ListItem Text="<%$ Resources:Attendance,Device Id %>" Value="Device_Id"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2">
                                                <asp:DropDownList ID="ddlOption" runat="server" CssClass="form-control" Visible="false">
                                                    <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>

                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind" Visible="false">
                                                    <asp:TextBox ID="txtValue" runat="server" CssClass="form-control"></asp:TextBox>
                                                </asp:Panel>

                                            </div>
                                            <div class="col-lg-2">
                                                <asp:ImageButton ID="btnbind" runat="server" Style="margin-top: -5px;" CausesValidation="False"
                                                    ImageUrl="~/Images/search.png" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>" Visible="false"></asp:ImageButton>
                                                <asp:ImageButton ID="btnRefresh" Style="width: 33px; margin-top: -5px;" runat="server" CausesValidation="False"
                                                    ImageUrl="~/Images/refresh.png" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %> " Visible="false"></asp:ImageButton>
                                            </div>
                                            <div class="col-lg-2">
                                                <h5></h5>
                                                <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                                <asp:HiddenField ID="HDFSort" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="lblTotalRecords" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                                        </h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <%--<asp:DataList ID="dlDevice" runat="server" RepeatColumns="6" RepeatDirection="Horizontal" Width="100%">
                                                        <ItemTemplate>--%>
                                                    <asp:Repeater ID="dlDevice" runat="server">
                                                        <ItemTemplate>

                                                            <div class="col-md-2">
                                                                <div class="box box-primary">
                                                                    <div class="box-body">
                                                                        <ul class="products-list product-list-in-box">
                                                                            <li class="item" style="padding: 0px; margin-top: -10px;">
                                                                                <div class="product-img" style="margin-top:-5px; height:20px; margin-left: -10px;">
                                                                                    <img src='<%#Eval("Device_Status").ToString() == "Connected" ? "../Images/device_online.png" : "../Images/device_offline.png" %>' alt="" />
                                                                                </div>
                                                                                <div style="padding-left: 0px; margin-left: 42px;" class="product-info">
                                                                                    <div class="product-description">
                                                                                        <span style="display: inline;">
                                                                                            <asp:Label ID="lbDeviceName" runat="server" ToolTip='<%# Eval("Device_Name") %>' Text='<%# Eval("Device_Name") %>'></asp:Label></span>
                                                                                    </div>


                                                                                    <div class="product-description">
                                                                                        <span class="visible-lg visible-md" style="display: inline; float: left">RT:  </span>
                                                                                        <span style="display: inline;">
                                                                                            <asp:Label ID="lblResponseTime" runat="server" ToolTip="Response Time" Text='<%# Eval("Response_Time") %>'></asp:Label></span>
                                                                                    </div>
                                                                                </div>
                                                                                <div style="margin-top:-4px; padding-left: 0px; margin-left:-5px;" class="product-info">
                                                                                     <div class="product-description">
                                                                                        <span class="visible-lg visible-md" style="display: inline; float: left">IP:  </span>
                                                                                        <span style="display: inline;">
                                                                                            <asp:Label ID="lblIpAddress" runat="server" ToolTip='<%# Eval("IP_Address") %>' Text='<%# Eval("IP_Address") %>' Font-Size="14px"></asp:Label></span>
                                                                                    </div>

                                                                                     <div class="product-description">
                                                                                        <%--<span class="visible-lg visible-md" style="display: inline; float: left;">Time:  </span>--%>
                                                                                        <span style="display: inline;">
                                                                                            <asp:Label ID="Label7" runat="server" ToolTip="Maximum Log Time" Text='<%# Eval("Max_Log_Time") %>' Font-Size="14px"></asp:Label></span>
                                                                                    </div>
                                                                                </div>
                                                                            </li>
                                                                        </ul>
                                                                        <div class="col-md-4" runat="server" visible="false">
                                                                            <img src="../Images/Users_icon.png" style="width: 20px; height: 20px;" alt="" />
                                                                            <div class="input-group">
                                                                                <%--<asp:Label ID="lblUserCount" runat="server" Text="1234"></asp:Label>--%>
                                                                                <asp:Label ID="lblUserCount" runat="server" ToolTip="User Count" Text='<%# Eval("User_Count") %>'></asp:Label>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-4" runat="server" visible="false">
                                                                            <img src="../Images/Finger_icon.png" style="width: 20px; height: 20px;" alt="" />
                                                                            <asp:Label ID="lblFingerCount" runat="server" ToolTip="Finger Count" Text='<%# Eval("Finger_Count") %>'></asp:Label>
                                                                            <%--<asp:Label ID="Label2" runat="server" Text="1234"></asp:Label>--%>
                                                                        </div>
                                                                        <div class="col-md-4" runat="server" visible="false">
                                                                            <img src="../Images/Face_icon.png" style="width: 20px; height: 20px;" alt="" />
                                                                            <asp:Label ID="lblFaceCount" runat="server" ToolTip="Face Count" Text='<%# Eval("Face_Count") %>'></asp:Label>
                                                                            <%--<asp:Label ID="Label3" runat="server" Text="1234"></asp:Label>--%>
                                                                        </div>

                                                                    </div>

                                                                </div>
                                                            </div>



                                                            <%-- <table><tr>
                                                                <td>
                                                                    <img src='<%#Eval("Device_Status").ToString() == "Connected" ? "../Images/device_online.png" : "../Images/device_offline.png" %>'  alt="" />
                                                                </td>
                                                                <td> <tr><td><asp:Label ID="lbDeviceName" runat="server" Text='<%# Eval("Device_Name") %>' ></asp:Label></td></tr>
                                                                    <tr><td><asp:Label ID="lblIpAddress" runat="server" Text='<%# Eval("IP_Address") %>' ></asp:Label></td></tr>
                                                                    <tr><td><asp:Label ID="lblResponseTime" runat="server" Text='<%# Eval("Response_Time") %>' ></asp:Label></td></tr>

                                                                </td>
                                                               
                                                                </tr>
                                                                <tr>
                                                                    <td><img src="../Images/Finger_icon.png" alt="" />
                                                                        <asp:Label ID="lblUserCount" runat="server" Text='<%# Eval("User_Count") %>' ></asp:Label></td>
                                                                    <td><img src="../Images/Finger_icon.png" alt="" />
                                                                        <asp:Label ID="lblFingerCount" runat="server" Text='<%# Eval("Finger_Count") %>' ></asp:Label></td>
                                                                    <td><img src="../Images/Face_icon.png" alt="" />
                                                                        <asp:Label ID="lblFaceCount" runat="server" Text='<%# Eval("Face_Count") %>' ></asp:Label></td>
                                                                </tr>
                                                            </table>--%>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvDevice" DataKeyNames="Device_Id,Port,IP_Address" PageSize="40" runat="server" OnRowDataBound="gvDevice_OnRowDataBound"
                                                        AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                         OnPageIndexChanging="gvDevice_PageIndexChanging" OnSorting="gvDevice_OnSorting" Visible="false">
                                                        <Columns>


                                                            <%-- <asp:TemplateField HeaderText="<%$ Resources:Attendance,Test%>">
                                                                <HeaderStyle HorizontalAlign="Left" />

                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="lnkConnect" runat="server" ImageUrl="~/Images/connect.png" CommandArgument='<%# Eval("Device_Id") %>' Visible="false"
                                                                        OnClick="lnkConnect_Click" ToolTip="<%$ Resources:Attendance,Connect%>" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>--%>




                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Device Id %>" SortExpression="Device_Id">
                                                                <ItemTemplate>

                                                                    <asp:Label ID="lblDeviceId1" runat="server" Text='<%# Eval("Device_Id") %>'></asp:Label>


                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Device Name %>" SortExpression="Device_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbDeviceName" runat="server" Text='<%# Eval("Device_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>



                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,IP Address %>" SortExpression="IP_Address">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFromDate" runat="server" Text='<%# Eval("IP_Address") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Port Number %>" SortExpression="Port">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbltoDate" runat="server" Text='<%# Eval("Port") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>



                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Location %>" SortExpression="Location_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLoc2" runat="server" Text='<%# Eval("Location_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("device_status") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
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



    <asp:Panel ID="pnlMenuList" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlMenuNew" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlMenuBin" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PnlList" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PnlNewEdit" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PnlBin" runat="server" Visible="false"></asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server"><script src="../Script/common.js"></script>
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
        function Li_Tab_List() {
            document.getElementById('<%= Btn_List.ClientID %>').click();
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
