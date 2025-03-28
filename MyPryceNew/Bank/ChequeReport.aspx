<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="ChequeReport.aspx.cs" Inherits="Bank_ChequeReport" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/sales_invoice.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="Cheque Report"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="Finance Setup"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="Bank"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="Cheque Report"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" Text="List" />
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" Text="New" />
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" Text="Bin" />
             <asp:Button ID="Btn_myModal" Style="display: none;" runat="server" data-toggle="modal"
                data-target="#myModal" Text="View Modal" />
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
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div_Box_Add" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="lblAddProductDetail" runat="server" Text="Advance Search"></asp:Label>
                                                </h3>
                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="Btn_Add_Div" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="form-group">

                                                      <div class="col-md-2">
                                                        <asp:Label ID="Label1" runat="server" Text="Option" />
                                                        <asp:DropDownList ID="ddlIsReconsiled" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlIsReconsiled_SelectedIndexChanged" AutoPostBack="true">
                                                            <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                            <asp:ListItem Text="Reconcile" Value="Reconcile"></asp:ListItem>
                                                            <asp:ListItem Text="Not Reconcile" Value="NotReconcile"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:Label ID="lblOption" runat="server" Text="Option" />
                                                        <asp:DropDownList ID="ddlOption" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlOption_SelectedIndexChanged" AutoPostBack="true">
                                                            <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                            <asp:ListItem Text="In-Ward" Value="Inward"></asp:ListItem>
                                                            <asp:ListItem Text="Out-Ward" Value="Outward"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblLocation" runat="server" Text="<%$ Resources:Attendance,Location %>" />
                                                        <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:Label ID="lblFromDate" runat="server" Text="<%$ Resources:Attendance,From Date %>" />
                                                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" OnTextChanged="txtFromDate_TextChanged" AutoPostBack="true" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender_VoucherDate" runat="server" TargetControlID="txtFromDate"
                                                            Format="dd-MMM-yyyy" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:Label ID="lblToDate" runat="server" Text="<%$ Resources:Attendance,To Date %>" />
                                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" OnTextChanged="txtFromDate_TextChanged" AutoPostBack="true" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" TargetControlID="txtToDate"
                                                            Format="dd-MMM-yyyy" />
                                                        <br />
                                                    </div>



                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div class="box box-warning box-solid">
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">

                                                <div class="flow">
                                                    <dx:ASPxGridView ID="GvChequeData" Width="100%" EnableViewState="false" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False" KeyFieldName="Trans_Id" >
                                                        <Columns>


                                                            <dx:GridViewDataTextColumn FieldName="AccountName" Settings-AutoFilterCondition="Contains" Caption="Account Name" VisibleIndex="5">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="Voucher_No" Settings-AutoFilterCondition="Contains" Caption="Voucher No" VisibleIndex="5">
                                                            </dx:GridViewDataTextColumn>


                                                            <dx:GridViewDataDateColumn Caption="Voucher Date" FieldName="Voucher_Date"
                                                                ShowInCustomizationForm="True" VisibleIndex="6" PropertiesDateEdit-EditFormatString="dd-MMM-yyyy" PropertiesDateEdit-DisplayFormatString="dd-MMM-yyyy" ReadOnly="True">
                                                            </dx:GridViewDataDateColumn>

                                                            <dx:GridViewDataTextColumn FieldName="Cheque_No" Settings-AutoFilterCondition="Contains" Caption="Cheque No" VisibleIndex="5">
                                                            </dx:GridViewDataTextColumn>

                                                              <dx:GridViewDataTextColumn FieldName="Narration" Settings-AutoFilterCondition="Contains" Caption="Narration" VisibleIndex="5">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataDateColumn Caption="Cheque Issue Date" FieldName="Cheque_Issue_Date"
                                                                ShowInCustomizationForm="True" VisibleIndex="6" PropertiesDateEdit-EditFormatString="dd-MMM-yyyy" PropertiesDateEdit-DisplayFormatString="dd-MMM-yyyy" ReadOnly="false">
                                                            </dx:GridViewDataDateColumn>

                                                            <dx:GridViewDataDateColumn Caption="Check Clear Date" FieldName="Cheque_Clear_Date"
                                                                ShowInCustomizationForm="True" VisibleIndex="6" PropertiesDateEdit-EditFormatString="dd-MMM-yyyy" PropertiesDateEdit-DisplayFormatString="dd-MMM-yyyy" ReadOnly="false">
                                                                <DataItemTemplate>
                                                                    <asp:Label runat="server" Text='<%# GetDate(Eval("Cheque_Clear_Date").ToString()) %>'></asp:Label>
                                                                    <asp:ImageButton ID="btnUpdateDate" runat="server" CommandArgument='<%#Eval("Detail_Trans_Id") + ";" + GetDate(Eval("Cheque_Clear_Date").ToString()) %>'
                                                                                        ImageUrl="~/Images/edit.png" Height="20px" ToolTip="<%$ Resources:Attendance,update %>"
                                                                                        OnCommand="btnUpdateReconcileDate_Command" CausesValidation="False" /> 
                                                               </DataItemTemplate>

                                                            </dx:GridViewDataDateColumn>

                                                            <dx:GridViewDataTextColumn FieldName="chequeAmt" Caption="Cheque Amount" VisibleIndex="9">
                                                                <DataItemTemplate>
                                                                    <asp:Label runat="server" Text='<%# Common.GetAmountDecimal(Eval("chequeAmt").ToString(),Session["DBConnection"].ToString(),HttpContext.Current.Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                                    <asp:Label runat="server" Text='<%# Session["LocCurrencyCode"].ToString() %>'> </asp:Label>
                                                                </DataItemTemplate>
                                                                
                                                            </dx:GridViewDataTextColumn>


                                                        </Columns>
                                                         <TotalSummary>
                                                            <dx:ASPxSummaryItem FieldName="chequeAmt" SummaryType="Sum" />
                                                        </TotalSummary>
                                                        
                                                        <GroupSummary>
                                                            <dx:ASPxSummaryItem SummaryType="Count" />
                                                        </GroupSummary>

                                                        <Settings ShowGroupPanel="true" ShowFilterRow="true" ShowFooter="true" />
                                                        <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
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
                </div>
            </div>
        </div>
    </div>

     <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="myModalLabel">
                        Update Cheque Issue Date</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_myModal" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-6">
                                                    <asp:Label ID="lblReconcile_Date" runat="server" Text="<%$ Resources:Attendance,Cheque Clear Date%>" />
                                                    <asp:TextBox ID="txtChequeIssueDate" runat="server" CssClass="form-control" />
                                                    <cc1:CalendarExtender OnClientShown="calendarShown" ID="CalendarExtender13" runat="server"
                                                        TargetControlID="txtChequeIssueDate" Format="dd/MMM/yyyy"  />
                                                    <asp:HiddenField ID="hdnVoucherDetailId" runat="server" />
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
                <div class="modal-footer">
                    <asp:UpdatePanel ID="Update_myModal_Button" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="Btn_Update_Reconcile" runat="server" OnClick="Btn_Update_Reconcile_Click"
                                Text="Update" CssClass="btn btn-primary" />
                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
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

    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_myModal_Button">
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
        
       
       
         function myModal_Popup() {
            document.getElementById('<%= Btn_myModal.ClientID %>').click();
         }
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 100004;
        }
    </script>
</asp:Content>

