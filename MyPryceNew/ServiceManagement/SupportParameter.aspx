<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="SupportParameter.aspx.cs" Inherits="ServiceManagement_SupportParameter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/system_perameter1.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Parameter Setup%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Service Management%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Service Management%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Service Setup%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    
                        <asp:UpdatePanel ID="Update_New" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">												
												<div class="col-md-12">
                                                    <cc1:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="0" CssClass="ajax__tab_yuitabview-theme" Width="100%">
                                                        <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="Email Setup">
                                                            <ContentTemplate>
                                                                <asp:UpdatePanel ID="Update_Email_Setup" runat="server">
                                                                    <ContentTemplate>
                                                                        <div class="row">
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="Label29" runat="server"  Text="<%$ Resources:Attendance,Email %>"></asp:Label>
                                                                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="Label30" runat="server"  Text="<%$ Resources:Attendance,Password %>"></asp:Label>
                                                                                <asp:TextBox ID="txtPasswordEmail" TextMode="Password" runat="server"
                                                                            CssClass="form-control" />
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="Label31" runat="server"  Text="<%$ Resources:Attendance,SMTP %>"></asp:Label>
                                                                                <asp:TextBox ID="txtSMTP" runat="server" CssClass="form-control" />
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="Label32" runat="server"  Text="<%$ Resources:Attendance,Port %>"></asp:Label>
                                                                                <asp:TextBox ID="txtPort" runat="server" CssClass="form-control" />
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="lblpop3" runat="server"  Text="<%$ Resources:Attendance,POP3  %>"></asp:Label>
                                                                                <asp:TextBox ID="txtPop3" runat="server" CssClass="form-control" />
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="Label98" runat="server"  Text="<%$ Resources:Attendance,  Port %>"></asp:Label>
                                                                                <asp:TextBox ID="txtpopport" runat="server" CssClass="form-control" />
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="Label1" runat="server"  Text="<%$ Resources:Attendance,Display Text %>"></asp:Label>
                                                                                <asp:TextBox ID="txtDisplayText" runat="server" CssClass="form-control" />
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="Label33" runat="server"  Text="<%$ Resources:Attendance,EnableSSL %>"></asp:Label>
                                                                                <asp:CheckBox ID="chkEnableSSL"  runat="server" />
                                                                                <br />
                                                                            </div>
                                                                        </div>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                                <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_Email_Setup">
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
                                                        <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="Job Card Terms & Condition">
                                                            <ContentTemplate>
                                                                <asp:UpdatePanel ID="Update_Job_Card_Terms_Condition" runat="server">
                                                                    <ContentTemplate>
                                                                        <div class="row">
                                                                            <div class="col-md-12">
                                                                                <cc1:Editor ID="txtTerms" runat="server" Height="300px" />
                                                                            </div>
                                                                        </div>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Job_Card_Terms_Condition">
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
                                                        <cc1:TabPanel ID="TabPanel3" runat="server" HeaderText="Final Acceptance Terms & Condition">
                                                            <ContentTemplate>
                                                                <asp:UpdatePanel ID="Update_Final_Acceptance_Terms_Condition" runat="server">
                                                                    <ContentTemplate>
                                                                        <div class="row">
                                                                            <div class="col-md-12">
                                                                                <cc1:Editor ID="txtAcceptance" runat="server" Height="300px" />
                                                                            </div>
                                                                        </div>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                                <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Final_Acceptance_Terms_Condition">
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
                                                        <cc1:TabPanel ID="TabPanel4" runat="server" HeaderText="Job Card Tools Category">
                                                            <ContentTemplate>
                                                                <asp:UpdatePanel ID="Update_Job_Card_Tools_Category" runat="server">
                                                                    <ContentTemplate>
                                                                        <div class="row">
                                                                            <div class="col-md-12">
                                                                                <asp:Label ID="Label2" runat="server"  Text="<%$ Resources:Attendance,Category Name %>"></asp:Label>
                                                                                <asp:Panel ID="pnlProductCategory" runat="server" Height="300px" Width="100%" BorderStyle="Solid"
                                                                                BorderWidth="1px" BorderColor="#abadb3" BackColor="White" ScrollBars="Auto">
                                                                                <asp:CheckBoxList ID="ChkProductCategory" runat="server" RepeatColumns="1" Font-Names="Trebuchet MS"
                                                                                     Font-Size="Small" ForeColor="Gray"  />
                                                                            </asp:Panel>
                                                                            </div>
                                                                        </div>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                                <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Job_Card_Tools_Category">
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
                                                    <div class="col-md-12" style="text-align:center">
                                                        <asp:Button ID="btnSaveEmail" runat="server" CssClass="btn btn-success" OnClick="btnSaveSMSEmail_Click"
                                                    Text="<%$ Resources:Attendance,Save %>"/>
                                                <asp:Button ID="btnreset" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                                                    OnClick="btnCancelSMSEmail_Click" Text="<%$ Resources:Attendance,Reset %>"/>
                                                        <br />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgress25" runat="server" AssociatedUpdatePanelID="Update_New">
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
