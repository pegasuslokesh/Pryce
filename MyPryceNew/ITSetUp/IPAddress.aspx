<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="IPAddress.aspx.cs" Inherits="ITSetUp_NationalityMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/nationality_setup.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="IP Address"></asp:Label>

        <%--<small>Control panel</small>--%>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="ITSetUp"></asp:Label></a></li>

        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="IP Address"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" OnClick="btnNew_Click" Text="New" />
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" Text="Bin" />
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
                    <li><a href="#Bin" onclick="Li_Tab_Bin()" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
                    <li class="active" id="Li_New"><a onclick="Li_Tab_New()" href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                </ul>
                <div class="tab-content">


                    <div class="tab-pane active" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">


                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblIPAddress" runat="server" Text="IP Address"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtIPAddress" ErrorMessage="<%$ Resources:Attendance,Enter Nationality Name %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtIPAddress" runat="server" OnTextChanged="txtIPAddress_TextChanged" AutoPostBack="true" CssClass="form-control" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                            TargetControlID="txtIPAddress" ValidChars="a,b,c,d,e,f,1,2,3,4,5,6,7,8,9,0,.,:">
                                                        </cc1:FilteredTextBoxExtender>

                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <br />
                                                        <asp:CheckBox ID="ChkIsBlocked" Text="Is Blocked" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblDescription" runat="server" Text="Description"></asp:Label>
                                                        <asp:TextBox ID="txtDescription" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                    </div>

                                                    <asp:HiddenField ID="hdnTrans_id" runat="server" />
                                                    <div class="col-md-12" style="text-align: center">

                                                        <asp:Button ID="btnCSave" ValidationGroup="Save" runat="server" Text="<%$ Resources:Attendance,Save %>" Visible="false" CssClass="btn btn-success" OnClick="btnCSave_Click" />
                                                        <asp:Button ID="BtnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>" CssClass="btn btn-primary" CausesValidation="False" OnClick="BtnReset_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <asp:HiddenField ID="editid" runat="server" />


                                <div class="box box-warning box-solid">
                                    <div class="box-header with-border">
                                        <asp:Label ID="lblTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>

                                    </div>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">

                                                    <dx:ASPxGridView ID="GvIpAddress" Width="100%" EnableViewState="false" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False" KeyFieldName="Trans_Id">
                                                        <Columns>

                                                            <dx:GridViewDataColumn Visible="false" VisibleIndex="0" Caption="<%$ Resources:Attendance,Edit %>">
                                                                <DataItemTemplate>
                                                                    <asp:ImageButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("trans_id") %>'
                                                                        ImageUrl="~/Images/edit.png" OnCommand="btnEdit_Command" CausesValidation="False" />
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>

                                                            <dx:GridViewDataColumn Visible="false" VisibleIndex="1" Caption="<%$ Resources:Attendance,Delete %>">
                                                                <DataItemTemplate>
                                                                    <asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("trans_id") %>'
                                                                        ImageUrl="~/Images/Erase.png" OnCommand="IbtnDelete_Command" Width="16px" />
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>

                                                            <dx:GridViewDataTextColumn FieldName="ip_address" Settings-AutoFilterCondition="Contains" Caption="IP Address">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="description" Settings-AutoFilterCondition="Contains" Caption="Description">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn Caption="Is Blocked" FieldName="is_blocked" Settings-AutoFilterCondition="Contains"></dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataDateColumn Caption="Created Date" FieldName="CreatedDate"
                                                                ShowInCustomizationForm="True" PropertiesDateEdit-EditFormatString="dd-MMM-yyyy" PropertiesDateEdit-DisplayFormatString="dd-MMM-yyyy" ReadOnly="True">
                                                            </dx:GridViewDataDateColumn>


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
                                    <!-- /.box-body -->
                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <!-- /.tab-pane -->
                    <div class="tab-pane" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server">

                            <ContentTemplate>
                                <div class="alert alert-info ">
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="ddlFieldNameBin" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="IP Address" Value="ip_address" />
                                                    <asp:ListItem Text="Description" Value="description" />
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2">
                                                <asp:DropDownList ID="ddlOptionBin" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2">
                                                <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbindBin">
                                                    <asp:TextBox ID="txtValueBin" runat="server" CssClass="form-control"></asp:TextBox>
                                                </asp:Panel>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:ImageButton ID="btnbindBin" Style="margin-top: -5px;" runat="server" CausesValidation="False" ImageUrl="~/Images/search.png" OnClick="btnbindBin_Click" ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>

                                                <asp:ImageButton ID="btnRefreshBin" Style="width: 33px;" runat="server" CausesValidation="False" ImageUrl="~/Images/refresh.png" OnClick="btnRefreshBin_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>

                                                <asp:ImageButton ID="imgBtnRestore" Style="width: 33px;" CausesValidation="False" Visible="false" runat="server" ImageUrl="~/Images/active.png" OnClick="btnRestoreSelected_Click" ToolTip="<%$ Resources:Attendance, Active %>" />

                                                <asp:ImageButton ID="ImgbtnSelectAll" Visible="false" Style="width: 33px;" runat="server" OnClick="ImgbtnSelectAll_Click" ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true" ImageUrl="~/Images/selectAll.png" />
                                            </div>
                                            <div class="col-lg-2">
                                                <h5 class="text-center">
                                                    <asp:Label ID="lblTotalRecordsBin" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>
                                                <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
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
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvIpAddressBin" DataKeyNames="trans_id" PageSize="<%# PageControlCommon.GetPageSize() %>" runat="server" AutoGenerateColumns="False"
                                                        Width="100%" AllowPaging="True" OnPageIndexChanging="GvIpAddressBin_PageIndexChanging"
                                                        OnSorting="GvIpAddressBin_OnSorting" AllowSorting="true" >
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChanged"
                                                                        AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="IpAddress" SortExpression="ip_address">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="gvip_address" runat="server" Text='<%# Eval("ip_address") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Description" SortExpression="description">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="gvDescription" runat="server" Text='<%# Eval("description") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Is Blocked">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="gvIsBlocked" runat="server" Text='<%# Eval("is_blocked") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        
                                                        
                                                        <PagerStyle CssClass="pagination-ys" />
                                                        
                                                    </asp:GridView>
                                                    <asp:HiddenField ID="HDFSortbin" runat="server" />
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
                <!-- /.tab-content -->
            </div>

        </div>
    </div>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_New">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_Bin">
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

        function Li_Tab_New() {
            document.getElementById('<%= Btn_New.ClientID %>').click();
        }
        function Li_Tab_Bin() {

        }

    </script>
</asp:Content>






