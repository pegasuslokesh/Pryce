<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="UrlData.aspx.cs" Inherits="MasterSetUp_UrlData" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/AddressControl.ascx" TagName="Addressmaster" TagPrefix="UC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-university"></i>
        <asp:Label ID="lblHeader" runat="server" Text="Url Setup"></asp:Label>

        <%--<small>Control panel</small>--%>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,System SetUp%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,System SetUp%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="Url Setup"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="btnList_Click" Text="List" />
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" OnClick="btnNew_Click" Text="New" />
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
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
                <ul class="nav nav-tabs pull-right bg-blue-gradient">
                    <li><a href="#Bin" onclick="Li_Tab_Bin()" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
                    <li id="Li_New"><a href="#New" onclick="Li_Tab_New()" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List" class="active"><a onclick="Li_Tab_List()" href="#List" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
                </ul>
                <div class="tab-content">

                    <div class="tab-pane active" id="List">

                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div1" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                    <asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					   <asp:Label ID="lblTotalRecords" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>
                                                <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="LinkHeader" Value="LinkHeader"></asp:ListItem>                                                       
                                                        <asp:ListItem Text="LinkDescription" Value="LinkDescription"></asp:ListItem>  
                                                        <asp:ListItem Text="LinkNumber" Value="LinkNumber"></asp:ListItem>   
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
                                                <div class="col-lg-5">
                                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                        <asp:TextBox ID="txtValue" placeholder="Search from Content" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div class="box box-warning box-solid" <%= gvUrlMaster.Rows.Count>0?"style='display:block'":"style='display:none'"%>>

                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <br />
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvUrlMaster" PageSize="<%# PageControlCommon.GetPageSize() %>" runat="server"
                                                        AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                        OnPageIndexChanging="gvUrlMaster_PageIndexChanging" OnSorting="gvUrlMaster_OnSorting">
                                                        <Columns>

                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                      <ul class="dropdown-menu">
                                                                            <li>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("TransId") %>'
                                                                                    CausesValidation="False" OnCommand="btnEdit_Command"
                                                                                    ToolTip="<%$ Resources:Attendance,Edit %>"><i class="fa fa-pencil"></i><%# Resources.Attendance.Edit%> </asp:LinkButton>
                                                                            </li>
                                                                            <li>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("TransId") %>' OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i><%# Resources.Attendance.Delete%></asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm2" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>"
                                                                                    TargetControlID="IbtnDelete">
                                                                                </cc1:ConfirmButtonExtender>
                                                                            </li>
                                                                          <li>
                                                                                <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# Eval("TransId") %>'
                                                                                    CausesValidation="False" OnCommand="btnView_Command"
                                                                                    ToolTip="<%$ Resources:Attendance,Edit %>"><i class="fa fa-eye"></i><%# Resources.Attendance.View%> </asp:LinkButton>

                                                                          </li>
                                                                        </ul>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="LinkNumber" SortExpression="LinkNumber">
                                                                 <ItemTemplate>
                                                                     <asp:Label ID="ListLinkNumber" runat="server" Text='<%# Eval("LinkNumber") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />

                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Department" SortExpression="Dep_Name">
                                                                <ItemTemplate>
                                                                     <asp:Label ID="ListDepartment" runat="server" Text='<%# Eval("Dep_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />

                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="LinkHeader" SortExpression="LinkHeader">
                                                                <ItemTemplate>
                                                                    <a href="<%# Eval("LinkUrl") %>"  target="_blank"><%# Eval("LinkHeader") %></a>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="LinkDescription" SortExpression="LinkDescription">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LinkDescription" runat="server" Text='<%# Eval("LinkDescription") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>   
                                                        </Columns>
                                                        <PagerStyle CssClass="pagination-ys" />
                                                    </asp:GridView>

                                                </div>

                                            </div>

                                        </div>
                                    </div>
                                    <!-- /.box-body -->
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
                                                    <div class="col-md-4">
                                                    </div>
                                                    <div class="col-md-12">    
                                                        <div class="col-md-6">                                                       
                                                        <asp:TextBox runat="server" ID="Trans" BackColor="#eeeeee" CssClass="form-control" style="display:none"></asp:TextBox>    
                                                        <asp:TextBox runat="server" ID="Link" BackColor="#eeeeee" CssClass="form-control" style="display:none"></asp:TextBox>                                                      
                                                       <asp:Label ID="LblLinkNumber" runat="server" Text="Link Number" ></asp:Label>
                                                         <a style="color: Red; display: none">*</a>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="Save" ControlToValidate="LinkNumber" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true" Text="*" Enabled="true" runat="server"></asp:RequiredFieldValidator>
                                              
                                                        <asp:TextBox runat="server" ID="LinkNumber" BackColor="#eeeeee" CssClass="form-control"></asp:TextBox></br>
                                                             </div>
                                                     </div>
                                                    <div class="col-md-12">
                                                         <div class="col-md-6">
                                                             <asp:Label ID="lblLinkHeader" runat="server" Text="Link Header" ></asp:Label>
                                                        <a style="color: Red; display: none">*</a>                                                      
                                                           <asp:RequiredFieldValidator ID="RequiredFieldValidator_txtMiles" ValidationGroup="Save" ControlToValidate="LinkHeader" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true" Text="*" Enabled="true" runat="server"></asp:RequiredFieldValidator>
                                                        <asp:TextBox runat="server"  ValidationGroup="Save" ID="LinkHeader" BackColor="#eeeeee" CssClass="form-control"></asp:TextBox></br>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Lbldepartment" runat="server" Text="Department"></asp:Label>
                                                            <asp:DropDownList CssClass="form-control"  BackColor="#eeeeee" ID="Department" runat="server">

                                                            </asp:DropDownList>
                                                        </div>                                                       
                                                    </div>
                                                       <div class="col-md-12">
                                                           <div class="col-md-6">
                                                                 <asp:Label ID="lblLinkUrl" runat="server" Text="Link Url" ></asp:Label>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="Save" ControlToValidate="LinkUrl" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true" Text="*" Enabled="true" runat="server"></asp:RequiredFieldValidator>
                        
                                                                 <asp:TextBox runat="server" ID="LinkUrl" BackColor="#eeeeee" CssClass="form-control"></asp:TextBox></br>
                                                           </div>

                                                       </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                             <asp:Label ID="lblLinkDescription" runat="server" Text="Description" ></asp:Label>
                                                         <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="Save" ControlToValidate="LinkDescription" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true" Text="*" Enabled="true" runat="server"></asp:RequiredFieldValidator>                                                  
                                            
                                                        <asp:TextBox runat="server" ID="LinkDescription" BackColor="#eeeeee" CssClass="form-control"></asp:TextBox>   
                                                        </div>                                                              
                                                    </div>   
                                                    </div> 
                                                    
                                                    <asp:HiddenField ID="editid" runat="server" />
                                              
                                                    <div class="col-md-12" style="text-align: center">
                                                        <br />
                                                        <asp:Button ID="btnSave" runat="server" ValidationGroup="Save" Text="<%$ Resources:Attendance,Save %>" Visible="false" CssClass="btn btn-success" OnClick="btnSave_Click" />
                                                        <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>" CssClass="btn btn-primary" CausesValidation="False" OnClick="btnReset_Click" />
                                                        <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>" CssClass="btn btn-danger" CausesValidation="False" OnClick="btnCancel_Click" />
                                                    </div>
                                           
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <!-- /.tab-pane -->
                    <div class="tab-pane" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server">

                            <ContentTemplate>
                               
                                  
                                       
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div id="Div2" runat="server" class="box box-info collapsed-box">
                                                    <div class="box-header with-border">
                                                        <h3 class="box-title">
                                                            <asp:Label ID="Label1" runat="server" Text="Advance Search"></asp:Label></h3>
                                                        &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblbinTotalRecords" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                        <div class="box-tools pull-right">
                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                <i id="I2" runat="server" class="fa fa-plus"></i>
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="box-body">
                                                       <div class="col-lg-3">
                                                            <asp:DropDownList ID="ddlbinFieldName" runat="server" CssClass="form-control">
                                                                <asp:ListItem Selected="True" Text="LinkNumber" Value="LinkNumber"></asp:ListItem>
                                                                <asp:ListItem Text="LinkHeader" Value="LinkHeader"></asp:ListItem>
                                                                <asp:ListItem Text="UrlLink" Value="UrlLink"></asp:ListItem>
                                                                <asp:ListItem Text="LinkDescription" Value="LinkDescription"></asp:ListItem>                                                               
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-lg-2">
                                                            <asp:DropDownList ID="ddlbinOption" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-lg-4">
                                                            <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbinbind">
                                                                <asp:TextBox ID="txtbinValue" placeholder="Search from Content" runat="server" CssClass="form-control"></asp:TextBox>
                                                            </asp:Panel>
                                                        </div>
                                                        <div class="col-lg-3">
                                                            <asp:LinkButton ID="btnbinbind"  runat="server" CausesValidation="False"  OnClick="btnbinbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                            <asp:LinkButton ID="btnbinRefresh"  runat="server" CausesValidation="False" OnClick="btnbinRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                            <asp:LinkButton ID="imgBtnRestore"  CausesValidation="False"  runat="server"  OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                            <asp:ImageButton ID="ImgbtnSelectAll" Visible="false" Style="width: 33px;" runat="server" OnClick="ImgbtnSelectAll_Click" ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true" ImageUrl="~/Images/selectAll.png" />
                                                        </div>


                                                    </div>
                                                </div>
                                            </div>
                                        </div>     
                                <div class="box box-warning box-solid" <%= gvUrlMasterBin.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                  
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">

                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvUrlMasterBin" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" DataKeyNames="TransId" Width="100%"
                                                        AllowPaging="True" OnPageIndexChanging="gvUrlMasterBin_PageIndexChanging" OnSorting="gvUrlMasterBin_OnSorting"
                                                        AllowSorting="true">
                                                        <Columns>
                                                          <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkgvSelect" runat="server"/>
                                                                   
                                                                </ItemTemplate>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChanged" AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Center"/>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="LinkNumber" SortExpression="LinkNumber">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbllinkNo1" runat="server" Text='<%# Eval("LinkNumber") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Department" SortExpression="Dep_Name">
                                                                 <ItemTemplate>
                                                                    <asp:Label ID="LinkBinDepartment" runat="server" Text='<%# Eval("Dep_Name") %>'></asp:Label>                                                                    
                                                                </ItemTemplate>
                                                                <ItemStyle />

                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="LinkHeader" SortExpression="LinkHeader">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LinkHeader" runat="server" Text='<%# Eval("LinkHeader") %>'></asp:Label>                                                                    
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="LinkUrl" SortExpression="LinkUrl">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LinkUrl" runat="server" Text='<%# Eval("LinkUrl") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="LinkDescription" SortExpression="LinkDescription">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LinkDescription" runat="server" Text='<%# Eval("LinkDescription") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField> 
                                                        </Columns>


                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                    <asp:HiddenField ID="HDFSortbin" runat="server" />

                                                </div>

                                            </div>
                                        </div>
                                        <!-- /.box-body -->
                                    </div>
                                    <!-- /.box -->
                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                </div>
                <!-- /.tab-content -->
            </div>

        </div>
    </div>


    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-body">
                    <%--<asp:UpdatePanel ID="Update_Modal" runat="server">
                        <ContentTemplate>--%>
                    <UC:Addressmaster ID="addaddress" runat="server" />
                    <%--</ContentTemplate>
                    </asp:UpdatePanel>--%>
                </div>
                <div class="modal-footer">
                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_List">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_New">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Bin">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:Panel ID="pnlAddress1" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlAddress2" runat="server" Visible="false"></asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
    <link href="../CSS/InvStyle.css" rel="stylesheet" type="text/css" />
    <script>
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
    </script>
    <script type="text/javascript">
        function LI_New_Active() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }

    </script>
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

        function Li_Tab_List() {
            document.getElementById('<%= Btn_List.ClientID %>').click();
        }
        function Li_Tab_New() {
            document.getElementById('<%= Btn_New.ClientID %>').click();
        }
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }

        function LI_List_Active() {
            $("#Li_List").addClass("active");
            $("#List").addClass("active");

            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");
        }
        function Modal_Address_Close() {
            document.getElementById('<%= Btn_View_Modal.ClientID %>').click();
        }
    </script>
</asp:Content>




