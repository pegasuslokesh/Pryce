<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="JobPlan.aspx.cs" Inherits="ServiceManagement_JobPlan" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-briefcase"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Job Plans%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Service Management%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Service Management%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Job Plans%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_myModal" Style="display: none;" runat="server" data-toggle="modal" data-target="#myModal" Text="View Modal" />
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
                    <li id="Li_New"><a  href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List" class="active"><a href="#List" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane active" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <ContentTemplate>
                                <div class="alert alert-info " style="display: none;">
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Job Plan Id %>" Value="JobPlanId"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Job Plan Name %>" Value="JobPlanName"></asp:ListItem>
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
                                            <div class="col-lg-2" style="text-align: center">
                                                <asp:ImageButton ID="btnbind" runat="server" CausesValidation="False" Style="margin-top: -5px;"
                                                    ImageUrl="~/Images/search.png" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>

                                                <asp:ImageButton ID="btnRefresh" runat="server" CausesValidation="False" Style="width: 33px;"
                                                    ImageUrl="~/Images/refresh.png" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                            </div>
                                            <div class="col-lg-2">
                                                <h5>
                                                    <asp:Label ID="lblTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>
                                                <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid"  <%= gvJobPlan.VisibleRowCount>0?"style='display:block'":"style='display:none'"%> >
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:UpdatePanel ID="export" runat="server">
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="BtnExportPDF" />
                                                         <asp:PostBackTrigger ControlID="BtnExportExcel" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <asp:Button ID="BtnExportPDF" runat="server" Text="Export PDF" OnClick="BtnExportPDF_Click" />
                                                        <asp:Button ID="BtnExportExcel" runat="server" Text="Export Excel" OnClick="BtnExportExcel_Click" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                    <asp:HiddenField ID="editid" runat="server" />
                                                    <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server" GridViewID="gvJobPlan"></dx:ASPxGridViewExporter>                                                    
                                                    
                                                    <dx:ASPxGridView ID="gvJobPlan" EnableViewState="false" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False" Width="100%" KeyFieldName="Trans_Id">
                                                        <Columns>


                                                            <dx:GridViewDataColumn VisibleIndex="2" Visible="false">
                                                                <DataItemTemplate>
                                                                    <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        CausesValidation="False" OnCommand="btnEdit_Command"
                                                                        Visible="false" ToolTip="<%$ Resources:Attendance,Edit %>"> <i class="fa fa-pencil" style="font-size:15px"></i></asp:LinkButton>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>

                                                            <dx:GridViewDataColumn VisibleIndex="3" Visible="false">
                                                                <DataItemTemplate>
                                                                    <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                        OnCommand="IbtnDelete_Command" Visible="false"
                                                                        ToolTip="<%$ Resources:Attendance,Delete %>"><i class="fa fa-trash" style="font-size:15px"></i></asp:LinkButton>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>


                                                            <dx:GridViewDataTextColumn FieldName="JobPlanId" Settings-AutoFilterCondition="Contains" Caption="<%$ Resources:Attendance,Job Plan Id%>" VisibleIndex="6">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="JobPlanName" Settings-AutoFilterCondition="Contains" Caption="<%$ Resources:Attendance,Job Plan Name %>" VisibleIndex="12">
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
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblplanId" runat="server" Text="<%$ Resources:Attendance,Job Plan Id %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="OnSave"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtjobPlanId" ErrorMessage="<%$ Resources:Attendance,Enter Job Plan Id%>"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtjobPlanId" runat="server" MaxLength="195" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblPlanName" runat="server" Text="<%$ Resources:Attendance,Job Plan Name %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="OnSave"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtjobPlanName" ErrorMessage="<%$ Resources:Attendance,Enter Job Plan Name %>"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtjobPlanName" runat="server" CssClass="form-control" />

                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="overflow: auto; max-height: 500px;">
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvVisitTask" ShowHeader="true" runat="server" AutoGenerateColumns="false"
                                                            Width="100%" ShowFooter="true"  OnRowDeleting="gvVisitTask_RowDeleting"
                                                            OnRowEditing="gvVisitTask_RowEditing" OnRowCancelingEdit="gvVisitTask_OnRowCancelingEdit"
                                                            OnRowUpdating="gvVisitTask_OnRowUpdating" OnRowCommand="gvVisitTask_RowCommand">
                                                            
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblsno1" runat="server" Text='<%# Container.DataItemIndex+1 %>'
                                                                            Width="20px"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle  HorizontalAlign="Left" Width="20px" />
                                                                    <FooterStyle Width="20px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Description%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblItemTask" runat="server" Text='<%#Eval("Work") %>'
                                                                            Width="660px"></asp:Label>
                                                                        <asp:Label ID="lblTransId" runat="server" Text='<%#Eval("Trans_Id") %>' Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txteditTask" runat="server" CssClass="form-control" Width="680px"
                                                                            Text='<%#Eval("Work") %>'></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtFooterTask" runat="server" CssClass="form-control" Width="660px"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                    <ItemStyle  HorizontalAlign="Left" Width="660px" />
                                                                    <FooterStyle Width="660px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Minutes %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblsno" runat="server" Text='<%# Eval("Minute") %>'
                                                                            Width="20px"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txEdittMinutes" Width="95px" runat="server" CssClass="form-control"
                                                                            Text='<%#Eval("Minute") %>' />
                                                                        <cc1:MaskedEditExtender ID="MaskedEditExtender2" runat="server" CultureAMPMPlaceholder=""
                                                                            CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                            CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                            Enabled="True" ErrorTooltipEnabled="True" Mask="99:99" MaskType="Time" TargetControlID="txEdittMinutes"
                                                                            UserTimeFormat="TwentyFourHour" MessageValidatorTip="true" InputDirection="LeftToRight">
                                                                        </cc1:MaskedEditExtender>
                                                                    </EditItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtMinutes" Width="95px" runat="server" CssClass="form-control" />
                                                                        <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" CultureAMPMPlaceholder=""
                                                                            CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                            CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                            Enabled="True" ErrorTooltipEnabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtMinutes"
                                                                            UserTimeFormat="TwentyFourHour" MessageValidatorTip="true" InputDirection="LeftToRight">
                                                                        </cc1:MaskedEditExtender>
                                                                    </FooterTemplate>
                                                                    <ItemStyle  HorizontalAlign="Center" Width="60px" />
                                                                    <FooterStyle Width="60px" HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <EditItemTemplate>
                                                                        <asp:Panel ID="pnlupdatework" runat="server" DefaultButton="imgupdatework">
                                                                            <asp:ImageButton ID="imgupdatework" runat="server" ImageUrl="~/Images/Allow.png"
                                                                                CommandArgument='<%#Eval("Trans_Id") %>' CommandName="Update" ToolTip="<%$ Resources:Attendance,Update %>" />
                                                                            &nbsp;&nbsp;
                                                                                           <asp:ImageButton ID="IbtnCancel" runat="server" CausesValidation="False"
                                                                                               ImageUrl="~/Images/Erase.png" CommandName="Delete" Width="16px" ToolTip="<%$ Resources:Attendance,Cancel %>" />

                                                                        </asp:Panel>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/Images/edit.png" CommandName="Edit"
                                                                            ToolTip="<%$ Resources:Attendance,Edit %>" Style="width: 14px" />&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                        <%-- <asp:Button ID="ButtonEdit" runat="server" CommandName="Edit" Text="Edit" Visible="true" />--%>
                                                                        <asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                            ImageUrl="~/Images/Erase.png" CommandName="Delete" Width="16px" ToolTip="<%$ Resources:Attendance,Delete %>" />
                                                                        <%--<asp:Button ID="ButtonDelete" runat="server"  Text="Delete" CommandArgument='<%#Eval("Trans_Id") %>'  CommandName="Delete" />--%>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Panel ID="pnlGridviewfeedback" runat="server" DefaultButton="IbtnAddTask">
                                                                            <asp:LinkButton ID="IbtnAddTask" runat="server" CausesValidation="False" 
                                                                                CommandName="AddNew" ToolTip="<%$ Resources:Attendance,Add %>"><i class="fa fa-plus-square" style="font-size:35px;padding:5px;margin-top: -5px;"></i></asp:LinkButton>
                                                                        </asp:Panel>
                                                                    </FooterTemplate>
                                                                    <ItemStyle  HorizontalAlign="Center" Width="150px" />
                                                                    <FooterStyle Width="150px" HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            
                                                            <PagerStyle CssClass="pagination-ys" />
                                                            
                                                        </asp:GridView>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnSave" runat="server" TabIndex="107" Text="<%$ Resources:Attendance,Save %>"
                                                            Visible="false" CssClass="btn btn-success" ValidationGroup="OnSave" OnClick="btnSave_Click" />

                                                        <asp:Button ID="btnReset" runat="server" TabIndex="108" Text="<%$ Resources:Attendance,Reset %>"
                                                            CssClass="btn btn-primary" CausesValidation="False" OnClick="btnReset_Click" />

                                                        <asp:Button ID="btnCancel" runat="server" TabIndex="109" Text="<%$ Resources:Attendance,Cancel %>"
                                                            CssClass="btn btn-danger" CausesValidation="False" OnClick="btnCancel_Click" />
                                                        <asp:HiddenField ID="HiddenField1" runat="server" />
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
        function myModal_Popup() {
            document.getElementById('<%= Btn_myModal.ClientID %>').click();
        }
        function myModal_Popup() {
            document.getElementById('<%= Btn_myModal.ClientID %>').click();
		}
    </script>
</asp:Content>
