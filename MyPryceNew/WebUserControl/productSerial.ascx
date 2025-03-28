<%@ Control Language="C#" AutoEventWireup="true" CodeFile="productSerial.ascx.cs" Inherits="WebUserControl_productSerial" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:UpdatePanel ID="Update_Modal" runat="server" >
    <ContentTemplate>
        <asp:HiddenField ID="hdnUcProductSnoRefId" runat="server" Value="0" />
        <asp:HiddenField ID="hdnUcProductSnoRefType" runat="server" Value="" />
        <asp:HiddenField ID="hdnUcProductSnoOrderId" runat="server" Value="0" />
        <asp:HiddenField ID="hdnUcProductSnoProductId" runat="server" Value="" />
        <asp:HiddenField ID="hdnParentGridId" runat="server" />
        <asp:HiddenField ID="hdnParentGridIndex" runat="server" />

        <asp:HiddenField ID="hdnReferenceType" runat="server" />
        <asp:HiddenField ID="hdnOrderId" runat="server" />
         <asp:HiddenField ID="hdnMerchantId" runat="server" />

        <div class="row">
            <div class="col-md-12">
               
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:Label ID="lblproductId" runat="server" Text="Product Id"></asp:Label>
                                &nbsp:&nbsp<asp:Label ID="lblProductIdvalue" runat="server" Text="0"></asp:Label>
                                <br />
                            </div>
                            <div class="col-md-12">
                                <asp:Label ID="lblserialProductname" runat="server" Text="Product Name"></asp:Label>
                                &nbsp:&nbsp<asp:Label ID="lblProductNameValue" runat="server"
                                    Text="0"></asp:Label>
                                <br />
                            </div>
                            <div id="pnlSerialNumber" runat="server" class="col-md-12">
                                <div class="col-md-6">
                                    <asp:Label ID="Label30" runat="server" Text="<%$ Resources:Attendance, File Upload%>"></asp:Label>
                                    <div class="input-group" style="width: 100%;">
                                        <cc1:asyncfileupload id="FULogoPath"
                                            onclientuploadstarted="FUAll_UploadStarted"
                                            onclientuploaderror="FUAll_UploadError"
                                            onclientuploadcomplete="FUAll_UploadComplete"
                                            onuploadedcomplete="FUAll_FileUploadComplete"
                                            runat="server" cssclass="form-control"
                                            completebackcolor="White"
                                            uploaderstyle="Traditional"
                                            uploadingbackcolor="#CCFFFF"
                                            throbberid="FUAll_ImgLoader" width="100%" />
                                        <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                            <asp:Image ID="FUAll_Img_Right" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Allow.png" Style="display: none" />
                                            <asp:Image ID="FUAll_Img_Wrong" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Delete1.png" Style="display: none" />
                                            <asp:Image ID="FUAll_ImgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                        </div>
                                    </div>
                                    <br />
                                </div>
                                <div class="col-md-6" style="text-align: center">
                                    <asp:Button ID="Btnloadfile" runat="server" Text="Load" CssClass="btn btn-primary"
                                        OnClick="Btnloadfile_Click" />

                                    <asp:Button ID="btnexecute" runat="server" Text="Execute" CssClass="btn btn-primary"
                                        OnClick="btnexecute_Click" />

                                      <asp:Button ID="btnLoadSerial" runat="server" Text="Temp Serial" CssClass="btn btn-primary"
                                        OnClick="btnLoadSerial_Click" />
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <div class="alert alert-info ">
                                        <div class="row">
                                            <div class="form-group">
                                                <div class="col-lg-3">
                                                    <asp:TextBox ID="txtserachserialnumber" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <cc1:textboxwatermarkextender id="txtwatermarkup" runat="server" targetcontrolid="txtserachserialnumber"
                                                        watermarktext="Search Serial Number">
                                                                        </cc1:textboxwatermarkextender>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:ImageButton ID="btnsearchserial" runat="server" CausesValidation="False"
                                                        ImageUrl="~/Images/search.png" OnClick="btnsearchserial_Click" Style="margin-top: -5px;" ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>

                                                    <asp:ImageButton ID="btnRefreshserial" runat="server" CausesValidation="False"
                                                        ImageUrl="~/Images/refresh.png" OnClick="btnRefreshserial_Click" Style="width: 33px;"
                                                        ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                                </div>
                                                <div class="col-lg-2">
                                                    <h5>
                                                        <asp:Label ID="Label31" runat="server" Text="Total :"></asp:Label>
                                                        <asp:Label ID="txtselectedSerialNumber" runat="server" Text="0"></asp:Label></h5>
                                                </div>
                                                <div class="col-lg-2">
                                                    <h5>
                                                        <asp:Label ID="lblCount" runat="server"></asp:Label>
                                                        <asp:Label ID="txtCount" runat="server" Text="0"></asp:Label></h5>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="box box-warning box-solid">
                                        <div class="box-body">
                                            <div class="row">
                                                <div class="col-md-8">
                                                    <div class="flow">
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvSerialNumber" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                                            AllowSorting="true" BorderStyle="Solid" Width="100%" 
                                                            PageSize="5">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("SerialNo") %>'
                                                                            ImageUrl="~/Images/Erase.png" OnCommand="IbtnDeleteserialNumber_Command" Width="16px" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle  HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Serial Number" SortExpression="SerialNo">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblsrno" runat="server" Text='<%#Eval("SerialNo") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center"  />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Manufacturing Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblmfg" runat="server" Text='<%#Eval("ManufacturerDate") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center"  />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Batch No.">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblBatchNo" runat="server" Text='<%#Eval("BatchNo") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center"  />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            
                                                            
                                                            <PagerStyle CssClass="pagination-ys" />
                                                            
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtSerialNo" Height="350px" runat="server" CssClass="form-control"
                                                        TextMode="MultiLine"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                
            </div>
        </div>

        <asp:Button ID="BtnSerialSave" runat="server" Text="<%$ Resources:Attendance,Save %>"
            CssClass="btn btn-success" OnClick="BtnSerialSave_Click" />
        <asp:Button ID="btnResetSerial" runat="server" Text="<%$ Resources:Attendance,Reset %>"
            CssClass="btn btn-primary" OnClick="btnResetSerial_Click" />
        <button type="button" class="btn btn-danger" data-dismiss="modal">
            Close</button>
        <asp:Button ID="btnDefault" runat="server" Style="visibility: hidden" />
    </ContentTemplate>
</asp:UpdatePanel>

<asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_Modal">
    <ProgressTemplate>
        <div class="modal_Progress">
            <div class="center_Progress">
                <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>

<script>
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
