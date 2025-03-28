<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EmailConfiguration.ascx.cs" Inherits="WebUserControl_EmailConfiguration" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:UpdatePanel ID="Update_New" runat="server">
    <ContentTemplate>
        <div class="row">
            <div class="col-md-12">
                <%-- <div class="box box-primary">
                    <div class="box-body">--%>
                <div class="form-group">
                    <div class="col-md-6">
                        <asp:DropDownList ID="ddluserList" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddluserList_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                    <div class="col-md-6" style="text-align: right;">
                        <asp:HiddenField ID="hdnEmpId" runat="server" />
                        <asp:HiddenField ID="hdnUserId" runat="server" />
                        <asp:Label ID="lbltotalrecord" runat="server" Font-Bold="true"></asp:Label>
                        <br />
                    </div>


                    <div class="col-md-12" style="overflow: auto; max-height: 200px;">
                        <br />
                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmailPassuser" PageSize="5" runat="server" AutoGenerateColumns="False"
                            Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="IbtnEditEmail" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                            ImageUrl="~/Images/Edit.png" OnCommand="IbtnEditEmail_Command" ToolTip="<%$ Resources:Attendance,Delete %>" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemStyle />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="IbtnDeleteEmail" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                            ImageUrl="~/Images/Erase.png" OnCommand="IbtnDeleteEmail_Command" ToolTip="<%$ Resources:Attendance,Delete %>" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemStyle />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,User Name%>" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblUserName" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,EmailID%>" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblEmailId" Text='<%# Eval("Email") %>'></asp:Label>
                                        <asp:Label runat="server" ID="lblpassword" Visible="false" Text='<%# Eval("password") %>'></asp:Label>
                                        <asp:Label runat="server" ID="lblsmtp" Visible="false" Text='<%# Eval("Field1") %>'></asp:Label>
                                        <asp:Label runat="server" ID="lblsmtpPort" Visible="false" Text='<%# Eval("Field2") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>



                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Is Default%>">
                                    <ItemTemplate>
                                        <asp:Label ID="chkdefault" runat="server" Text='<%#Convert.ToBoolean(Eval("IsDefault")) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Last Synch Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lbllastsynchdate" runat="server" Text='<%#(Eval("LastSynchdate")==null || Eval("LastSynchdate").ToString()=="")?null:Convert.ToDateTime(Eval("LastSynchdate")).ToString("dd-MMM-yyyy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,EnableSSL %>" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblgvEnableSSL" Text='<%# Eval("Field5") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Test">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkTestEmail" runat="server" Font-Underline="true" Text='Test Email' OnCommand="lnkTestEmail_Command"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle CssClass="pagination-ys" />
                        </asp:GridView>
                        <br />
                    </div>
                    <div class="col-md-12">
                        <br />
                        <p style="color: red">Email:Your EmailId , Password:Your Email Password , smtp: smtp.hostinger.com , smtp Port:587,Pop3 :pop.hostinger.com ,pop port:995,Enable SSl:True, Is Default:true/false</p>
                    </div>
                    <div class="col-md-6">
                        <asp:Label ID="Label29" runat="server" Text="<%$ Resources:Attendance,Email %>"></asp:Label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
                        <br />
                    </div>
                    <div class="col-md-6">
                        <asp:Label ID="Label30" runat="server" Text="<%$ Resources:Attendance,Password %>"></asp:Label>
                        <asp:TextBox ID="txtPasswordEmail" TextMode="Password" runat="server"
                            CssClass="form-control" />
                        <br />
                    </div>
                    <div class="col-md-6">
                        <asp:Label ID="Label31" runat="server" Text="<%$ Resources:Attendance,SMTP %>"></asp:Label>
                        <asp:TextBox ID="txtSMTP" runat="server" CssClass="form-control" />
                        <br />
                    </div>
                    <div class="col-md-6">
                        <asp:Label ID="Label32" runat="server" Text="<%$ Resources:Attendance,Port %>"></asp:Label>
                        <asp:TextBox ID="txtPort" runat="server" CssClass="form-control" />
                        <br />
                    </div>
                    <div class="col-md-6">
                        <asp:Label ID="lblpop3" runat="server" Text="<%$ Resources:Attendance,POP3  %>"></asp:Label>
                        <asp:TextBox ID="txtPop3" runat="server" CssClass="form-control" />
                        <br />
                    </div>
                    <div class="col-md-6">
                        <asp:Label ID="Label98" runat="server" Text="<%$ Resources:Attendance,  Port %>"></asp:Label>
                        <asp:TextBox ID="txtpopport" runat="server" CssClass="form-control" />
                        <br />
                    </div>
                    <div class="col-md-6">
                        <asp:Label ID="Label33" runat="server" Text="<%$ Resources:Attendance,EnableSSL %>"></asp:Label>
                        <asp:CheckBox ID="chkEnableSSL" runat="server" />
                        <br />
                    </div>
                    <div class="col-md-6">
                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Is Default %>"></asp:Label>
                        <asp:CheckBox ID="chkDefault" runat="server" />
                        <br />
                    </div>
                    <div class="col-md-12">
                        <br />
                    </div>

                    <div class="col-md-6">
                        <div class="input-group" style="width: 100%;">
                            <cc1:AsyncFileUpload ID="FileUploadImage"
                                OnClientUploadStarted="FuLogo_UploadStarted"
                                OnClientUploadError="FuLogo_UploadError"
                                OnClientUploadComplete="FuLogo_UploadComplete"
                                OnUploadedComplete="FuLogo_FileUploadComplete"
                                runat="server" CssClass="form-control"
                                CompleteBackColor="White"
                                UploaderStyle="Traditional"
                                UploadingBackColor="#CCFFFF"
                                ThrobberID="FULogo_ImgLoader" Width="100%" />
                            <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                <asp:LinkButton ID="FULogo_Img_Right" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-check" style="font-size:25px;color:#22cb33"></i></asp:LinkButton>
                                <asp:LinkButton ID="FULogo_Img_Wrong" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-remove" style="font-size:25px"></i></asp:LinkButton>
                                <asp:Image ID="FULogo_ImgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                            </div>
                            <div class="input-group-btn" style="width: 35px;">
                                <asp:LinkButton ID="ImgFileUploadAdd" runat="server" CausesValidation="False" OnClick="ImgLogoAdd_Click" ToolTip="<%$ Resources:Attendance,Add %>"><i class="fa fa-plus-square" style="font-size:35px;padding:5px;margin-top: -5px;"></i></asp:LinkButton>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-6" style="text-align: center">
                        <asp:Button ID="btnSaveEmail" runat="server" CssClass="btn btn-success" OnClick="btnSaveSMSEmail_Click"
                            Text="<%$ Resources:Attendance,Save %>" />
                        <asp:Button ID="btnreset" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                            OnClick="btnCancelSMSEmail_Click" Text="<%$ Resources:Attendance,Reset %>" />
                        <br />
                    </div>
                    <div class="col-md-12">
                        <br />
                    </div>
                    <div class="col-md-12" style="overflow: auto; max-height: 500px;">
                        <cc2:Editor ID="txtEmailSignature" Height="300px" runat="server" />
                    </div>
                </div>
                <%--  </div>
                </div>--%>
            </div>
        </div>

    </ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript">
    function FuLogo_UploadComplete(sender, args) {
        document.getElementById('<%= FULogo_Img_Wrong.ClientID %>').style.display = "none";
        document.getElementById('<%= FULogo_Img_Right.ClientID %>').style.display = "";
    }
    function FuLogo_UploadError(sender, args) {
        document.getElementById('<%= FULogo_Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= FULogo_Img_Wrong.ClientID %>').style.display = "";
        alert('Invalid File Type, Select Only .png, .jpg, .jpge extension file');
    }
    function FuLogo_UploadStarted(sender, args) {
        var filename = args.get_fileName();

        var filext = filename.substring(filename.lastIndexOf(".") + 1);
        filext = filext.toLowerCase();
        if (filext == "png" || filext == "jpg" || filext == "jpge") {
            return true;
        }
        else {
            throw {
                name: "Invalid File Type",
                level: "Error",
                message: "Invalid File Type, Select Only .png, .jpg, .jpge extension file",
                htmlMessage: "Invalid File Type, Select Only .png, .jpg, .jpge extension file"
            }
            return false;
        }
    }
</script>

<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_New">
    <ProgressTemplate>
        <div class="modal_Progress">
            <div class="center_Progress">
                <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
