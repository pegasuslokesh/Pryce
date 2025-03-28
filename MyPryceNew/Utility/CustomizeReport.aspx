<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomizeReport.aspx.cs" MasterPageFile="~/ERPMaster.master" Inherits="dummy" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/InvStyle.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Report System%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">

        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Attendance Report%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Report System%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_View_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#myModal" Text="View Modal" />
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
                <%-- <ul class="nav nav-tabs pull-right bg-blue-gradient">                    
                    <li id="Li_List" class="active"><a href="#List" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
                </ul>--%>
                <div class="tab-content">
                    <div class="tab-pane active" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-4">
                                                        <asp:Label runat="server" ID="lblModules" Text="<%$ Resources:Attendance,Modules%>"></asp:Label>
                                                        <asp:DropDownList ID="ddlModule" runat="server" Class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlModule_slectedIndexChanged"></asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label runat="server" ID="Label1" Text="<%$ Resources:Attendance,Object%>"></asp:Label>
                                                        <asp:DropDownList ID="ddlObject" runat="server" Class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlObject_SelectedIndexChanged"></asp:DropDownList>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-4">
                                                        <asp:Label runat="server" ID="lblReport" Text="<%$ Resources:Attendance,Report Type%>"></asp:Label>
                                                        <asp:DropDownList ID="ddlReportType" runat="server" Class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged"></asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="flow">
                                                            <asp:ListBox ID="reportsList" runat="server" Width="100%" Height="150px" onchange="ReportSelectedChanged();"></asp:ListBox>
                                                            <br />
                                                            <br />
                                                            <asp:Button CssClass="btn btn-primary" ID="btnEditReport" runat="server" Text="<%$ Resources:Attendance,Edit%>" OnClick="btnEditReport_Click" Visible="false" />
                                                            <asp:Button CssClass="btn btn-primary" ID="btnDeleteReport" runat="server" Text="<%$ Resources:Attendance,Delete%>" OnClick="btnDeleteReport_Click" Visible="false" />

                                                            <cc1:ConfirmButtonExtender ID="confirm2" runat="server" ConfirmText="Are you sure you want to delete the report?"
                                                                TargetControlID="btnDeleteReport">
                                                            </cc1:ConfirmButtonExtender>

                                                            <asp:Button CssClass="btn btn-primary" ID="btnNewReport" runat="server" Text="<%$ Resources:Attendance,New Report%>" OnClick="btnNewReport_Click" Visible="false" />
                                                            <asp:Button CssClass="btn btn-primary" ID="btnPreviewReport" runat="server" Text="<%$ Resources:Attendance,Preview%>" OnClick="btnPreviewReport_Click" Visible="false" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>




                                <div class="box box-primary collapsed-box" runat="server" id="div_update" visible="false">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <asp:Label ID="lblDeviceParameter" EnableViewState="true" Font-Names="Times New roman" Font-Size="18px"
                                                Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Report To Edit:%>"></asp:Label></h3>
                                        <div class="box-tools pull-right">
                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-plus" id="I1" runat="server"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="form-group">

                                            <div class="col-lg-3">
                                                <asp:Label ID="lblReportName" runat="server" Text="<%$ Resources:Attendance,Report Name%>"> </asp:Label>
                                                <asp:TextBox ID="txtReportName" runat="server" Class="form-control"></asp:TextBox>
                                                <br />
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:Label ID="lblModule" runat="server" Text="<%$ Resources:Attendance,Module Name%>"> </asp:Label>
                                                <asp:DropDownList ID="ddlModuleList" runat="server" Class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlModuleList_SelectedIndexChanged"></asp:DropDownList>
                                                <br />
                                            </div>
                                            <div class="col-md-3">
                                                <asp:Label runat="server" ID="Label2" Text="<%$ Resources:Attendance,Object%>"></asp:Label>
                                                <asp:DropDownList ID="ddlObjectName" runat="server" Class="form-control"></asp:DropDownList>
                                                <br />
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:Label ID="lblReportType" runat="server" Text="<%$ Resources:Attendance,Report Type%>"> </asp:Label>
                                                <asp:DropDownList ID="ddlChangeReportType" runat="server" Class="form-control">
                                                    <asp:ListItem Text="Select" Selected="True" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Header Report" Value="Header Report"></asp:ListItem>
                                                    <asp:ListItem Text="Detail Report" Value="Detail Report"></asp:ListItem>
                                                    <asp:ListItem Text="Summary Report" Value="Summary Report"></asp:ListItem>
                                                    <asp:ListItem Text="Graph Report" Value="Graph Report"></asp:ListItem>
                                                    <asp:ListItem Text="Statistics Report" Value="statistics Report"></asp:ListItem>
                                                    <asp:ListItem Text="MIS Report" Value="MIS Report"></asp:ListItem>
                                                    <asp:ListItem Text="Drill Report" Value="Drill Report"></asp:ListItem>
                                                    <asp:ListItem Text="Sub Report" Value="Sub Report"></asp:ListItem>
                                                </asp:DropDownList>
                                                <br />
                                            </div>
                                            <div class="col-lg-3">
                                                <br />
                                                <asp:Button CssClass="btn btn-primary" ID="btnSaveChanges" Text="<%$ Resources:Attendance,Update Report%>" runat="server" OnClick="btnSaveChanges_Click" />
                                                <br />
                                                <br />
                                            </div>

                                            <div class="col-lg-12">
                                                <br />
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:UpdatePanel runat="server" ID="sdfsdf">
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="btnExportReport" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                                <asp:Button CssClass="btn btn-primary" ID="btnExportReport" runat="server" Text="<%$ Resources:Attendance,Export%>" OnClick="btnExportReport_Click" />
                                                <br />
                                            </div>
                                            <div class="col-lg-3">

                                                <%--  <asp:FileUpload ID="FileUploadControl" runat="server" CssClass="fa-border" />--%>
                                                <cc1:AsyncFileUpload ID="FileUploadControl"
                                                    runat="server" CssClass="form-control"
                                                    CompleteBackColor="White"
                                                    UploaderStyle="Traditional"
                                                    UploadingBackColor="#CCFFFF"
                                                    ThrobberID="FUAll_ImgLoader" Width="100%" />
                                                <br />
                                            </div>

                                            <div class="col-lg-3">
                                                <asp:Button runat="server" ID="BrowseButton" Text="<%$ Resources:Attendance,Import%>" OnClick="BrowseButton_Click" CssClass="btn btn-primary" />
                                                <br />
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
        function View_Modal_Popup() {
            document.getElementById('<%= Btn_View_Modal.ClientID %>').click();
        }
        function Close_Modal_Popup() {
            document.getElementById('<%= Btn_View_Modal.ClientID %>').click();
        }
        function ReportSelectedChanged() {
            var dpt = document.getElementById('<%= reportsList.ClientID %>');
          var ddl_module = document.getElementById('<%= ddlModule.ClientID %>');
          var ddl_object = document.getElementById('<%= ddlObject.ClientID %>');
          var ddl_reportType = document.getElementById('<%= ddlReportType.ClientID %>');

          var ReportName = document.getElementById('<%= lblDeviceParameter.ClientID %>');
          ReportName.innerHTML = "";
          ReportName.innerHTML = "Report To Edit: " + dpt.options[dpt.selectedIndex].text.split('/')[1];

          document.getElementById('<%= txtReportName.ClientID %>').value = dpt.options[dpt.selectedIndex].text.split('/')[1];

            document.getElementById('<%= ddlModuleList.ClientID %>').value = ddl_module.options[ddl_module.selectedIndex].value;
          document.getElementById('<%= ddlModuleList.ClientID %>').text = ddl_module.options[ddl_module.selectedIndex].text;

         <%-- document.getElementById('<%= ddlObjectName.ClientID %>').value = ddl_object.options[ddl_object.selectedIndex].value;
          document.getElementById('<%= ddlObjectName.ClientID %>').text = ddl_object.options[ddl_object.selectedIndex].text;--%>

          document.getElementById('<%= ddlChangeReportType.ClientID %>').value = ddl_reportType.options[ddl_reportType.selectedIndex].value;
          document.getElementById('<%= ddlChangeReportType.ClientID %>').text = ddl_reportType.options[ddl_reportType.selectedIndex].text;
      }
    </script>
</asp:Content>
