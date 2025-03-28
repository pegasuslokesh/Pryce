<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="DownloadLog.aspx.cs" Inherits="Device_DownloadLog" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-download"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Download Log Setup%>"></asp:Label>


    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Device%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Download Log%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">


    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">              
                <div class="tab-content">
                    <div class="tab-pane active" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group" style="text-align: center">
                                                    <asp:Button ID="btnDownload" runat="server" Text="<%$ Resources:Attendance,Download%>" OnClick="btnDownload_OnClick" ValidationGroup="Save" CssClass="btn btn-primary" />
                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid"  <%= gvDevice.Rows.Count>0?"style='display:block'":"style='display:none'"%> >                                  
                                    <div class="box-body">
                                        <div class="row" id="Div_location" runat="server" visible="false">
                                            <div class="col-md-6">
                                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Location %>"></asp:Label>
                                                <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged"></asp:DropDownList>

                                            </div>
                                        </div>


                                        <asp:CustomValidator ID="CustomValidator1" ValidationGroup="Save" runat="server" ErrorMessage="Please select at least one record."
                                            ClientValidationFunction="Validate" ForeColor="Red"></asp:CustomValidator>
                                        <div class="row" >
                                            <div class="col-md-12">
                                                <div style="overflow: auto; max-height: 250px">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvDevice" DataKeyNames="Device_Id,Port,IP_Address,Device_Name,Communication_Type,field2"
                                                        PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server" AutoGenerateColumns="False" OnRowDataBound="gvDevice_OnRowDataBound"
                                                        Width="100%" AllowPaging="false" AllowSorting="True" OnPageIndexChanging="gvDevice_PageIndexChanging"
                                                        OnSorting="gvDevice_OnSorting">
                                                        <Columns>

                                                            <asp:TemplateField Visible="false">
                                                                <ItemStyle />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkdevice" runat="server" />
                                                                </ItemTemplate>
                                                                <HeaderTemplate>

                                                                    <asp:CheckBox ID="chkheaderdevice" runat="server" AutoPostBack="true" OnCheckedChanged="chkheaderdevice_OnCheckedChanged" />

                                                                </HeaderTemplate>

                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>


                                                            <%--   <asp:TemplateField>
                                                    <ItemStyle />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="LnkDeviceOp" runat="server" ImageUrl="~/Images/dwdlog.png" OnClick="LnkDeviceOp_Click"
                                                            ToolTip="<%$ Resources:Attendance,Download Log%>" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Device Name %>" SortExpression="Device_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbDeviceName" runat="server" Text='<%# Eval("Device_Name") %>'></asp:Label>
                                                                    <asp:Label ID="lblDeviceId1" runat="server" Text='<%# Eval("Device_Id") %>' Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <%--<asp:TemplateField HeaderText="<%$ Resources:Attendance,Device Name(Local) %>" SortExpression="Device_Name_L">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDeviceNameL" runat="server" Text='<%# Eval("Device_Name_L") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,IP Address %>" SortExpression="IP_Address">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFromDate" runat="server" Text='<%# Eval("IP_Address") %>'></asp:Label>
                                                                    <asp:Label ID="lbltoDate" runat="server" Text='<%# Eval("Port") %>' Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <%-- <asp:TemplateField HeaderText="<%$ Resources:Attendance,Port Number %>" SortExpression="Port">
                                                                <ItemTemplate>
                                                                    
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>--%>
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
                                                            <%-- <asp:TemplateField HeaderText="<%$ Resources:Attendance,Communication Type %>" SortExpression="Communication_Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbltoDate1" runat="server" Text='<%# Eval("Communication_Type") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>--%>
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

                                            <div class="col-md-12">

                                                <div style="overflow: auto; max-height: 250px">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLogStatus"
                                                        PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server" AutoGenerateColumns="False"
                                                        Width="100%" AllowPaging="false" AllowSorting="false">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDeviceStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />

                                                            </asp:TemplateField>

                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTime" runat="server" Text='<%# Eval("STime") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                        </Columns>


                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                    <%-- <asp:TextBox ID="txtlogStatus" runat="server" TextMode="MultiLine" Height="250px"></asp:TextBox>--%>
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
                                                    <div class="col-md-6">
                                                        <asp:LinkButton ID="lnkBackFromManage" runat="server" OnClick="lnkBackFromManage_Click"
                                                            Text="<%$ Resources:Attendance,Back%>"></asp:LinkButton>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Device Id %>"
                                                            Font-Bold="True"></asp:Label>
                                                        :
                                                                    <asp:Label ID="lblDeviceId" runat="server"></asp:Label>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Device Name %>"
                                                            Font-Bold="True"></asp:Label>
                                                        :
                                                                    <asp:Label ID="lblDeviceWithId" runat="server"></asp:Label>
                                                    </div>
                                                    <div class="col-md-12" style="overflow: auto; width: 100%;">
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvLog" runat="server" DataKeyNames="idwInOutMode,sTimeString,sdwEnrollNumber"
                                                            PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' Width="100%" AllowPaging="true"
                                                            OnPageIndexChanging="gvLog_OnPageIndexChanging">


                                                            <PagerStyle CssClass="pagination-ys" />

                                                        </asp:GridView>

                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" CssClass="btn btn-success"
                                                            Text="<%$ Resources:Attendance,Save %>" />
                                                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-danger" Text="<%$ Resources:Attendance,Cancel %>"
                                                            OnClick="btnCancel_Click" />
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <%--<div class="row">
                            <div class="col-md-12" style="overflow:auto;">
                                <div class="box box-primary">
                                    <div class="box-body">
                                        <div class="form-group" style="text-align: center">
                                            <asp:UpdatePanel ID="gv_update" runat="server">
                                                <ContentTemplate>

                                                
                                           
                                                    </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>--%>
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

    </script>
</asp:Content>

