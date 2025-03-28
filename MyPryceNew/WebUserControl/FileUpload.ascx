<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FileUpload.ascx.cs" Inherits="WebUserControl_FileUpload" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>

<div class="row">
    <asp:UpdatePanel ID="Update_Header" runat="server">
        <ContentTemplate>
            <div class="col-md-3" style="max-width: 100%;">
                <h3>
                    <i class="fa fa-upload"></i>&nbsp;&nbsp;<asp:Label ID="lblHeaderL" runat="server" Text="File Upload"></asp:Label>
                </h3>
            </div>
            <div class="col-md-5" style="max-width: 100%; text-align: center;">
                <h3>
                    <asp:Label ID="lblRefname" runat="server" Font-Bold="true" ForeColor="Gray" Font-Size="15px" />
                </h3>
            </div>
            <div class="col-md-4" style="max-width: 100%;">
                <h3>
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" onkeyup="Search_Gridview(this, 'gvFileMaster')" Placeholder="Enter Search Keyword here"></asp:TextBox>
                </h3>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>


<asp:UpdateProgress ID="UpdateProgress11" runat="server" AssociatedUpdatePanelID="Update_Header">
    <ProgressTemplate>
        <div class="modal_Progress">
            <div class="center_Progress">
                <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>

<asp:UpdatePanel ID="Update_New" runat="server">
    <ContentTemplate>
        <div class="row">
            <div class="col-md-12">
                <div class="box box-primary">
                    <div class="box-body">
                        <div class="form-group">


                            <div class="col-md-6">
                                <asp:Label ID="lblDocName" runat="server" Text="<%$ Resources:Attendance,Document Type %>"></asp:Label>
                                <asp:DropDownList ID="ddlDocumentName" runat="server" CssClass="form-control" />
                                <br />
                            </div>
                            <div class="col-md-6">
                                <asp:Label ID="Label101" runat="server" Text="<%$ Resources:Attendance,File Upload %>" />
                                <div class="input-group" style="width: 100%;">
                                    <cc1:AsyncFileUpload ID="UploadFile"                                        
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
                            </div>


                            <div class="col-md-3">
                                <asp:Label ID="lblfileName1" runat="server" Text="<%$ Resources:Attendance,File Name %>"></asp:Label>
                                <asp:TextBox ID="txtDocumentName" runat="server" CssClass="form-control" />
                                <br />
                            </div>
                            <div class="col-md-3">
                                <asp:Label ID="lblSetReminder" runat="server" Text="Set Reminder"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddlSetReminder" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                </asp:DropDownList>
                                <br />
                            </div>

                            <div class="col-md-6">
                                <asp:Label ID="lblExpiryDate" runat="server" Text="<%$ Resources:Attendance,Expiry Date %>" />
                                <div class="input-group">
                                    <asp:TextBox ID="txtExpiryDate" runat="server" CssClass="form-control" />
                                    <cc1:CalendarExtender OnClientShown="calendarShown" ID="CalendarExtender3" runat="server" TargetControlID="txtExpiryDate"
                                        Format="dd-MMM-yyyy" />
                                    <div class="input-group-btn">
                                        <asp:LinkButton ID="ImgButtonDocumentAdd" runat="server" CausesValidation="False" OnClick="ImgButtonDocumentAdd_Click" ToolTip="<%$ Resources:Attendance,Add %>">&nbsp;&nbsp;<i class="fa fa-plus-square" style="font-size:35px;padding:5px;margin-top: -5px;"></i></asp:LinkButton>
                                    </div>
                                </div>
                                <br />
                            </div>

                            <div class="col-md-6"></div>
                            <div class="col-md-6">
                                <asp:Label ID="lblmessage" runat="server" ForeColor="Red" Visible="false" Text="Expiry Date Must Be Greater"></asp:Label>
                            </div>

                            <div class="col-md-12">
                            </div>
                            <div class="col-md-12" class="flow">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvFileMaster" runat="server" AutoGenerateColumns="False" Width="100%" ClientIDMode="Static"
                                            AllowPaging="false" AllowSorting="True" >
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSerialNo" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />

                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                            <Triggers>
                                                                <asp:PostBackTrigger ControlID="ImgDownload" />
                                                            </Triggers>
                                                            <ContentTemplate>
                                                                <asp:LinkButton ID="ImgDownload" runat="server" CommandArgument='<%#Eval("Trans_id") %>' OnCommand="OnDownloadDocumentCommand1" ToolTip="<%$ Resources:Attendance,Download %>"><i class="fa fa-download"></i></asp:LinkButton>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>


                                                        <asp:UpdatePanel ID="sdsd" runat="server">
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="IbtnDelete" EventName="Command" />
                                                            </Triggers>
                                                            <ContentTemplate>
                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="IbtnDeleteDocument_Command" ToolTip="<%$ Resources:Attendance,Delete %>" ><i class="fa fa-times"></i></asp:LinkButton>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>


                                                        
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Document Type %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDocumentName" runat="server" Text='<%# Eval("Document_Name") %>'></asp:Label>
                                                        <asp:Label ID="lblTrans_id" runat="server" Text='<%# Eval("Trans_id") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle  HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,File Name %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFileName" runat="server" Text='<%# Eval("File_Name").ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle  HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Upload Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGvUploadDate" runat="server" Text='<%# GetDate(Eval("File_Upload_Date").ToString()) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" Width="100px" />

                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Expiry Date %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGvExpiryDate" runat="server" Text='<%# GetDate(Eval("File_Expiry_Date").ToString()) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" Width="100px" />

                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,User Name %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGvUsername" runat="server" Text='<%# Eval("UserName").ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" Width="100px" />

                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle CssClass="pagination-ys" />
                                        </asp:GridView>
                                        <asp:HiddenField ID="HiddenField4" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </ContentTemplate>
</asp:UpdatePanel>

<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_New">
    <ProgressTemplate>
        <div class="modal_Progress">
            <div class="center_Progress">
                <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>


<asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
    <ProgressTemplate>
        <div class="modal_Progress">
            <div class="center_Progress">
                <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>



<script type="text/javascript">


    function calendarShown(sender, args) {
        sender._popupBehavior._element.style.zIndex = 100004;
    }
    
    function FUAll_UploadComplete(sender, args) {
        document.getElementById('<%= FUAll_Img_Wrong.ClientID %>').style.display = "none";
        document.getElementById('<%= FUAll_Img_Right.ClientID %>').style.display = "";
    }
    function FUAll_UploadError(sender, args) {
        document.getElementById('<%= FUAll_Img_Right.ClientID %>').style.display = "none";
        document.getElementById('<%= FUAll_Img_Wrong.ClientID %>').style.display = "";
    }

    function DispMsg(message) {
        alert(message);
        return;
    }


    function Search_Gridview(strKey, strGV) {
        var strData = strKey.value.toLowerCase().split(" ");
        var tblData = document.getElementById(strGV);
        var rowData;
        for (var i = 1; i < tblData.rows.length; i++) {
            rowData = tblData.rows[i].innerHTML;
            var styleDisplay = 'none';
            for (var j = 0; j < strData.length; j++) {
                if (rowData.toLowerCase().indexOf(strData[j]) >= 0)
                    styleDisplay = '';
                else {
                    styleDisplay = 'none';
                    break;
                }
            }
            tblData.rows[i].style.display = styleDisplay;
        }
    }

</script>

