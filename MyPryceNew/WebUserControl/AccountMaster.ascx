<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountMaster.ascx.cs" Inherits="WebUserControl_AccountMaster" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<div class="row">
    <div class="col-lg-6" style="max-width: 100%;">
        <h3>
            <img src="../Images/follow-up.png" alt="Followup" /><asp:Label ID="lblHeaderL" Text="Account Master" runat="server"></asp:Label>
        </h3>
    </div>
    <div class="col-lg-6" style="text-align: right; margin-top: 12px; max-width: 100%;">
        <h4>
            <asp:UpdatePanel ID="Update_Header" runat="server">
                <ContentTemplate>
                    <asp:Label ID="lblHeaderR" runat="server"></asp:Label>
                </ContentTemplate>

            </asp:UpdatePanel>
        </h4>
    </div>
    <div class="col-lg-12">
        <div class="nav-tabs-custom">
            <ul class="nav nav-tabs pull-right bg-blue-gradient">
                <li id="Li_AccountMasterBin"><a href="#Tab_AcMasterBin" data-toggle="tab">
                    <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>

                <li id="Li_AccountMasterNew"><a href="#Tab_AcMasterNew" data-toggle="tab">
                    <asp:UpdatePanel ID="Update_Li" runat="server">
                        <ContentTemplate>
                            <i class="fa fa-file"></i>&nbsp;&nbsp;
                            <asp:Label ID="Lbl_Tab_AccountMasterNew" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </a></li>
                <li id="Li_AccountMasterList" class="active" onclick="Li_AccountMasterList('pnlListGrid')"><a href="#Tab_AcMasterList" data-toggle="tab">
                    <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,List%>"></asp:Label></a></li>

            </ul>

        </div>
    </div>
</div>
<div class="tab-content">
    <div class="tab-pane active" id="Tab_AcMasterList">
        <asp:UpdatePanel ID="upAcMasterList" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlCreditSetup" runat="server" class="panel panel-default" Style="display: none">
                    <div class="col-md-12">
                        <div id="Div_Credit_Approval" class="box box-primary">
                            <div class="box-header with-border">
                                <h3 class="box-title">
                                    <asp:Label ID="lblCreditSetupTitle" runat="server" CssClass="LableHeaderTitle" ForeColor="Gray"
                                        Font-Bold="true" Text="Credit Setup for "></asp:Label></h3>

                            </div>
                            <div class="box-body">
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblCreditLimit" runat="server" Text="<%$ Resources:Attendance,Credit Limit %>" />
                                            <div style="width: 100%" class="input-group">
                                                <asp:TextBox ID="txtCreditLimit" Style="width: 40%" runat="server" CssClass="form-control" />
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender15txtCreditLimit" runat="server"
                                                    Enabled="True" TargetControlID="txtCreditLimit" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                </cc1:FilteredTextBoxExtender>
                                            </div>
                                            <br />
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Label ID="lblCreditDays" runat="server" Text="<%$ Resources:Attendance,Credit Days %>" />
                                            <asp:TextBox ID="txtCreditDays" runat="server" CssClass="form-control" />
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" Enabled="True"
                                                TargetControlID="txtCreditDays" FilterType="Numbers">
                                            </cc1:FilteredTextBoxExtender>
                                            <br />
                                        </div>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="col-md-12">
                                            <asp:Label ID="Label33" runat="server" Text="Credit Parameter" />

                                            <asp:RadioButton ID="rbtnAdvanceCheque" runat="server" Text="Advance Cheque Basis"
                                                GroupName="Parameter" />
                                            <asp:RadioButton ID="rbtnInvoicetoInvoice" runat="server" GroupName="Parameter" Text="Invoice to Invoice(First Clear Old Invoice than Issue New invoice)" />
                                            <br />
                                            <asp:RadioButton ID="rbtnAdvanceHalfpayment" GroupName="Parameter" runat="server"
                                                Text="50% Advance and 50% on Delivery" />
                                            <asp:RadioButton ID="rbtnNone" GroupName="Parameter" runat="server" Checked="true"
                                                Text="None" />
                                            <br />
                                        </div>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="col-md-6">
                                            <br />
                                            <asp:Label ID="Label37" runat="server" Text="Financial Statement" />
                                            <div class="input-group" style="width: 100%;">
                                                <cc1:AsyncFileUpload ID="FileUploadFinancilaStatement"
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
                                                    <asp:Image ID="FUAll_Img_Right" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Allow.png" Style="display: none" />
                                                    <asp:Image ID="FUAll_Img_Wrong" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Delete1.png" Style="display: none" />
                                                    <asp:Image ID="FUAll_ImgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                </div>
                                            </div>
                                            <br />
                                        </div>
                                        <div class="col-md-6">
                                            <br />
                                            <asp:Button ID="btnuploadFiancialstatement" Style="margin-top: 17px;" runat="server" Text="<%$ Resources:Attendance,Load %>"
                                                CssClass="btn btn-primary" OnClick="btnuploadFiancialstatement_Click" />&nbsp;&nbsp;
                                                                <asp:LinkButton ID="lnkDownloadFiancialstatement" runat="server" ToolTip="Download"
                                                                    OnClick="btnDownloadFiancialstatement_Click"
                                                                    ForeColor="Blue" />
                                            <br />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="box-footer with-border">
                                <asp:Button ID="btnSaveCreditDetail" runat="server" CssClass="btn btn-primary" Text='<%$ Resources:Attendance,Save%>' OnCommand="btnSaveCreditDetail_Command" />
                                <asp:Button ID="btnCancelCreditDetail" runat="server" Text='<%$ Resources:Attendance,Cancel%>' CssClass="btn btn-primary" OnClientClick="Li_AccountMasterList('pnlListGrid')" />
                                <asp:HiddenField ID="hdn_ucAcMaster_OtherAccountId" runat="server" Value="0" />
                                <asp:HiddenField ID="hdn_ucAcMaster_CurrencyId" runat="server" Value="0" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlListGrid" runat="server" class="panel panel-default">
                    <div class="col-md-12">
                        <div class="flow">
                            <asp:HiddenField ID="hdnRefId" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="hdnRefType" runat="server"></asp:HiddenField>
                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvAcMasterList" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                runat="server" AutoGenerateColumns="False" Width="100%"
                                AllowSorting="true">
                                <Columns>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnDeleteAcMaster" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_id") %>' OnCommand="btnDeleteAcMaster_Command"
                                                ImageUrl="~/Images/Erase.png" Width="16px" ToolTip="Delete" />
                                            <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>"
                                                TargetControlID="btnDeleteAcMaster">
                                            </cc1:ConfirmButtonExtender>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField Visible="true">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnCreditSetup" OnCommand="btnCreditSetup_Command" CommandArgument='<%# Eval("Trans_id") + "," + Eval("Account_No") + "," + Eval("Currency_Name") + "," + Eval("Currency_Id")  %>' runat="server" ToolTip="Credit Setup"> <i class="fa fa-credit-card" aria-hidden="true"></i> </asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="FileUpload">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnFileUploadAcMaster" runat="server" CommandArgument='<%# Eval("Trans_id") %>' ToolTip="File-Upload"
                                            ImageUrl="~/Images/ModuleIcons/archiving.png" Height="30px" Width="30px" CausesValidation="False" OnCommand="btnFileUploadAcMaster_Command" />
                                    </ItemTemplate>
                                    <ItemStyle  HorizontalAlign="Center" Width="3%" />
                                </asp:TemplateField>--%>


                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Account No%>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAccountNoAcMaster" runat="server" Text='<%#Eval("Account_No") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Currency%>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCurrencyAcMaster" runat="server" Text='<%#Eval("Currency_Name") %>' />
                                            <asp:HiddenField ID="hdnCurrencyId" runat="server" Value='<%#Eval("Currency_id") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Current Balance">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkCurrentBalance" runat="server" CommandArgument='<%# Eval("Trans_id") %>' Text='<%# getCurrentBalance(Eval("Trans_id").ToString()) %>' OnCommand="lnkCurrentBalance_Command"> </asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Credit Limit%>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCreditLimit" runat="server" Text='<%# SystemParameter.GetAmountWithDecimal(Eval("Credit_Limit").ToString(),Session["LoginLocDecimalCount"].ToString()) %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Credit Days%>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCreditDays" runat="server" Text='<%#Eval("Credit_Days") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Credit Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCreditStatus" runat="server" Text='<%#Eval("credit_status") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ob">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtOb" runat="server" Text='<%#Eval("fy_ob") %>' Width="50px" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ob Type">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlObType" CssClass="input-group-text" runat="server">
                                                <asp:ListItem Text="DR" Value="DR" Selected="true"></asp:ListItem>
                                                <asp:ListItem Text="CR" Value="CR"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="hdnObType" runat="server" Value='<%# Eval("fy_ob_type") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ex. Rate">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtExchangeRate" runat="server" Text='<%#Eval("fy_ob_exchange_rate") %>' Width="70px" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnUpdateOb" runat="server" OnCommand="btnUpdateOb_Command" CommandArgument='<%#Eval("Trans_id") %>' ToolTip="Update Opening Balance">
                                           <i class="fa fa-upload" aria-hidden="true"></i></asp:LinkButton>
                                           
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>


                                <PagerStyle CssClass="pagination-ys" />

                            </asp:GridView>
                            <br />
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="uprgAcMasterList" runat="server" AssociatedUpdatePanelID="upAcMasterList">
            <ProgressTemplate>
                <div class="modal_Progress">
                    <div class="center_Progress">
                        <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <div class="tab-pane" id="Tab_AcMasterNew">
        <asp:UpdatePanel ID="upAcMasterNew" runat="server">
            <ContentTemplate>
                <div class="row">
                    <div class="col-md-12">
                        <div class="box box-primary">
                            <div class="box-body">
                                <div class="form-group">
                                    <div class="col-md-6">
                                        <asp:Label ID="lblAccountNo" Text='<%$ Resources:Attendance,Account No%>' runat="server" />
                                        <asp:TextBox ID="txtAccountNo" runat="server" CssClass="form-control" Enabled="false" />
                                        <%--<asp:RequiredFieldValidator ID="reqTxtAccountNo" runat="server" ControlToValidate="txtAccountNo" ForeColor="Red" ErrorMessage="Blank account no not allowed" />--%>
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="lblCurrency" Text='<%$ Resources:Attendance,Currency%>' runat="server" />
                                        <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="form-control" />
                                        <br />
                                    </div>

                                    <div class="col-md-6">
                                        <asp:Button ID="btnSaveAcMaster" runat="server" OnCommand="btnSaveAcMaster_Command" CausesValidation="true"  Text='<%$ Resources:Attendance,Save%>' CssClass="btn btn-primary" />
                                        <asp:Button ID="btnResetAcMaster" runat="server" OnCommand="btnResetAcMaster_Command" Text='<%$ Resources:Attendance,Reset%>' CssClass="btn btn-primary" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="uprgAcMasterNew" runat="server" AssociatedUpdatePanelID="upAcMasterNew">
            <ProgressTemplate>
                <div class="modal_Progress">
                    <div class="center_Progress">
                        <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <div class="tab-pane" id="Tab_AcMasterBin">
        <asp:UpdatePanel ID="upAcMasterBin" runat="server">
            <ContentTemplate>
                <div class="col-md-12">
                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvAcMasterBin" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                        runat="server" AutoGenerateColumns="False" Width="100%"
                        AllowSorting="true">
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Restore %>" Visible="false">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnRestore" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_id") %>' OnCommand="btnRestore_Command"
                                        ImageUrl="~/Images/restore.png" Width="16px" ToolTip="Restore" />
                                    <cc1:ConfirmButtonExtender ID="confirmRestore" runat="server" ConfirmText="Are you sure to restore record ?"
                                        TargetControlID="btnRestore">
                                    </cc1:ConfirmButtonExtender>
                                    <%--<asp:CheckBox ID="chkRestore" runat="server" OnCheckedChanged="chkRestore_CheckedChanged" />--%>
                                    <asp:Label ID="lblBinTransId" runat="server" Text='<%#Eval("Currency_Name") %>' Visible="false" />
                                    <%--<cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="Are you sure to restore this record?"
                                            TargetControlID="chkRestore">
                                        </cc1:ConfirmButtonExtender>--%>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="4%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Account No%>" SortExpression="Account_No">
                                <ItemTemplate>
                                    <asp:Label ID="lblAccountNoAcMasterBin" runat="server" Text='<%#Eval("Account_No") %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Currency%>" SortExpression="Currency_Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblCurrencyAcMasterBin" runat="server" Text='<%#Eval("Currency_Name") %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                            </asp:TemplateField>
                        </Columns>


                        <PagerStyle CssClass="pagination-ys" />

                    </asp:GridView>
                    <br />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="uprgAcMasterBin" runat="server" AssociatedUpdatePanelID="upAcMasterBin">
            <ProgressTemplate>
                <div class="modal_Progress">
                    <div class="center_Progress">
                        <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
</div>

<script>
    function DisplayMsg(str) {
        alert(str);
        return;
    }
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
    function Li_AccountMasterList(pnl) {
        if (pnl == 'pnlListGrid') {
            $('#<%= pnlListGrid.ClientID %>').show();
             $('#<%= pnlCreditSetup.ClientID %>').hide();
         }
         else {
             $('#<%= pnlListGrid.ClientID %>').hide();
             $('#<%= pnlCreditSetup.ClientID %>').show();
         }
     }
     function OpentStatementWindow(refType, customerId) {
         if (refType.trim() == "Customer") {
             window.open("../CustomerReceivable/CustomerStatement.aspx?Id=" + customerId);
         }
         else {
             window.open("../SuppliersPayable/SupplierStatement.aspx?Id=" + customerId);
         }
     }

</script>
