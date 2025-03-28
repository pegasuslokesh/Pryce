<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="Leave_Opening_Balance.aspx.cs" Inherits="MasterSetUp_Leave_Opening_Balance" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="far fa-calendar-alt"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="Leave Opening Balance"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="Leave Opening Balance"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Page" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="col-md-12">
                    <div class="box box-info">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <asp:Label ID="Label17" runat="server" Text="<%$ Resources:Attendance,Search%>"></asp:Label></h3>
                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                    <i class="fa fa-minus"></i>
                                </button>
                            </div>
                        </div>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-md-12">
                                    <div class="col-md-6">
                                        <asp:HyperLink ID="HyperLink2" Visible="true" runat="server" NavigateUrl="~/CompanyResource/Leave_OpeningBalance.xls" Text="Download Excel For Upload Leave Balance"></asp:HyperLink>
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <div class="input-group" style="width: 100%;">
                                            <cc1:AsyncFileUpload ID="fileLoad"
                                                OnClientUploadStarted="FUAll_UploadStarted"
                                                OnClientUploadError="FUAll_UploadError"
                                                OnClientUploadComplete="FUAll_UploadComplete"
                                                OnUploadedComplete="FUAll_FileUploadComplete"
                                                runat="server" CssClass="form-control"
                                                CompleteBackColor="White"
                                                UploaderStyle="Traditional"
                                                UploadingBackColor="#CCFFFF"
                                                ThrobberID="FUAll_ImgLoader" Width="100%" />
                                            <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                <asp:LinkButton ID="FUAll_Img_Right" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-check" style="font-size:30px;color:#22cb33"></i></asp:LinkButton>
                                                <asp:LinkButton ID="FUAll_Img_Wrong" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-remove" style="font-size:30px"></i></asp:LinkButton>
                                                <asp:Image ID="FUAll_ImgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                            </div>
                                        </div>
                                        <br />
                                        <asp:Button ID="Btn_upload" runat="server" ValidationGroup="Save" Text="Upload"
                                            class="btn btn-primary" OnClick="Btn_upload_Click" />

                                        <asp:Button ID="btn_Save" runat="server" ValidationGroup="Save" Text="Save"
                                            class="btn btn-success" OnClick="btn_Save_Click" />

                                        <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="Are you sure to save the record ?"
                                            TargetControlID="btn_Save">
                                        </cc1:ConfirmButtonExtender>

                                        <br />
                                    </div>
                                </div>

                                <div class="col-md-6" runat="server">
                                    <asp:RadioButton ID="rbtnall" runat="server" Text="All" Checked="true" GroupName="a" AutoPostBack="true" OnCheckedChanged="rbtnall_CheckedChanged" />
                                    <asp:RadioButton ID="rbtnValid" runat="server" GroupName="a" Text="Valid" AutoPostBack="true" OnCheckedChanged="rbtnall_CheckedChanged" />
                                    <asp:RadioButton ID="rbtninValid" runat="server" GroupName="a" Text="InValid" AutoPostBack="true" OnCheckedChanged="rbtnall_CheckedChanged" />
                                </div>

                                <div class="col-md-6" runat="server">
                                    <asp:Label ID="lblvalidRecord" Font-Bold="true" Font-Size="16px" runat="server"
                                        Text="Valid Record : 0"></asp:Label>

                                </div>
                                <div class="col-md-12" style="display: block;">
                                    <div style="overflow: auto; max-height: 300px;">

                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvValidEmployee" runat="server" Width="100%" AutoGenerateColumns="true">
                                            <Columns>
                                            </Columns>


                                            <PagerStyle CssClass="pagination-ys" />

                                        </asp:GridView>
                                    </div>
                                </div>

                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Page">
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

    </script>
    <script type="text/javascript">
        function FUAll_UploadComplete(sender, args) {
            document.getElementById('<%= FUAll_Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= FUAll_Img_Right.ClientID %>').style.display = "";
        }
        function FUAll_UploadError(sender, args) {
            document.getElementById('<%= FUAll_Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= FUAll_Img_Wrong.ClientID %>').style.display = "";
        }
        function FUAll_UploadStarted(sender, args) {

        }
    </script>
</asp:Content>

