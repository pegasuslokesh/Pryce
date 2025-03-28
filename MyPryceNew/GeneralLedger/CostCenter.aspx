<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPMaster.master"  CodeFile="CostCenter.aspx.cs" Inherits="GeneralLedger_CostCenter" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <script src="../Script/customer.js"></script>
    <style>
        .HeaderCenter{
           text-align:center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-poll"></i>
        <asp:Label ID="lblHeader" runat="server" Text="Cost Center"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Finance Setup%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,General Ledger%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="Cost Center"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
     <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>            
            <asp:HiddenField runat="server" ID="hdnCanView" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
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
                    <li id="Li_Bin"><a href="#Bin" onclick="Li_Tab_Bin()" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
                    <li id="Li_New"><a onclick="Li_Tab_New()" href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List" class="active"><a href="#List" onclick="Li_Tab_List()" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
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
                                                    <asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecords" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>" />

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="Center Name" Value="CenterName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Account Name %>" Value="AccountName"
                                                            Selected="True"></asp:ListItem>                                                        

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
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbind">
                                                        <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" placeholder="Search from Content" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False"
                                                        OnClick="btnbindrpt_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False"
                                                        OnClick="btnRefreshReport_Click"
                                                        ToolTip="<%$ Resources:Attendance,Refresh %>"> <span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                         
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid" <%= GvCostOfCenter.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvCostOfCenter" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server"
                                                        AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvCOC_PageIndexChanging"
                                                        AllowSorting="True" OnSorting="GvCOC_Sorting">
                                                           <Columns>
                                                                   <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li>
                                                                                <asp:LinkButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                    ToolTip="View" OnCommand="lnkViewDetail_Command"
                                                                                    CausesValidation="False"><i class="fa fa-eye"></i>View</asp:LinkButton>
                                                                            </li>

                                                                            <li>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                    OnCommand="btnEdit_Command" CausesValidation="False"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                    OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>
                                                                        </ul>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                               <asp:TemplateField  HeaderStyle-CssClass="HeaderCenter" HeaderText="Account Name" SortExpression="Trans_Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvTransId" runat="server" Text='<%#Eval("AccountName") %>' />
                                                                </ItemTemplate>                                                                  
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-CssClass="HeaderCenter" HeaderText="Center Name" SortExpression="CenterName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCenterName" runat="server" Text='<%#Eval("CenterName") %>' />
                                                                </ItemTemplate>                                                            
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                                 <asp:TemplateField HeaderStyle-CssClass="HeaderCenter" HeaderText="Center Name L" SortExpression="CenterNameL">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCenterNameL" runat="server" Text='<%#Eval("CenterNameL") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />                                                                 
                                                            </asp:TemplateField>
                                                           </Columns>
                                                         <PagerStyle CssClass="pagination-ys" />
                                                    </asp:GridView>
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
                                <asp:Panel ID="PnlNewContant" runat="server" class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <br />
                                                        <asp:HiddenField ID="hdnEditId" Value="" runat="server" />
                                                        <asp:Label ID="lblAccount" Text="Cost Of Account" runat="server"></asp:Label>

                                                         <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCOA" ErrorMessage="<%$ Resources:Attendance,Enter Account Group%>"></asp:RequiredFieldValidator>



                                                       <asp:TextBox ID="txtCOA" runat="server" CssClass="form-control" BackColor="#eeeeee" ></asp:TextBox>
                                                          <cc1:AutoCompleteExtender ID="txtCOA_AutoCompleteExtender" runat="server"
                                                                DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                                MinimumPrefixLength="1" ServiceMethod="GetCompletionListCostOfAccountName" ServicePath=""
                                                                TargetControlID="txtCOA" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <br />
                                                        &nbsp  &nbsp
                                                    </div>
                                                    <div class="col-md-6">
                                                        <br />
                                                        &nbsp  &nbsp
                                                    </div>
                                                    <div class="col-md-6">                                                     

                                                        <asp:Label ID="lblCenterName" Text="Center Name" runat="server"> </asp:Label>
                                                         <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCenterName" ErrorMessage="Enter Center Name"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtCenterName" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblCenterNameL" Text="Center Name L" runat="server">
                                                        </asp:Label>
                                                        <asp:TextBox ID="txtCenterNameL" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <br />

                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Button ID="btnSave" ValidationGroup="Save" runat="server" Text="Save" CssClass="btn btn-success" OnClick="btnSave_Click" />
                                                        &nbsp;
                                                        <asp:Button ID="btnReset" runat="server" CssClass="btn btn-danger" Text="Reset" OnClick="btnReset_Click" />
                                                         &nbsp;
                                                             <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClick="btnCancel_Click" />
                                                      
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="tab-pane" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div2" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label2" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecordsBin" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldNameBin" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="Center Name" Value="CenterName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Account Name %>" Value="AccountName"
                                                            Selected="True"></asp:ListItem>                                                        
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
                                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbindBin">
                                                        <asp:TextBox ID="txtValueBin" runat="server" CssClass="form-control" placeholder="Search from Content"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="btnbindBin" runat="server" CausesValidation="False"
                                                        OnClick="btnbindBin_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="btnRefreshBin" runat="server" CausesValidation="False"
                                                        OnClick="btnRefreshBin_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="imgBtnRestore" CausesValidation="False"
                                                        runat="server" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
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
                                                     <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                                     <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GVCostCenterBin" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server"
                                                        AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvCOC_PageIndexChanging"
                                                        AllowSorting="True" OnSorting="GvCOC_Sorting">
                                                           <Columns>
                                                             <asp:TemplateField HeaderText="Select">
                                                                 <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkCurrent" runat="server" OnCheckedChanged="chkCurrent_CheckedChanged"
                                                                        AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelect_CheckedChanged" />
                                                                </ItemTemplate>                                                                  
                                                            </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Id %>" SortExpression="Trans_Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvTransId" runat="server" Text='<%#Eval("Trans_Id") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                               <asp:TemplateField  HeaderStyle-CssClass="HeaderCenter" HeaderText="AccountId" SortExpression="Trans_Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="AccountName" runat="server" Text='<%#Eval("AccountName") %>' />
                                                                </ItemTemplate>                                                                  
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-CssClass="HeaderCenter" HeaderText="Center Name" SortExpression="CenterName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCenterName" runat="server" Text='<%#Eval("CenterName") %>' />
                                                                </ItemTemplate>                                                            
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                                 <asp:TemplateField HeaderStyle-CssClass="HeaderCenter" HeaderText="Center Name L" SortExpression="CenterNameL">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCenterNameL" runat="server" Text='<%#Eval("CenterNameL") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />                                                                 
                                                            </asp:TemplateField>
                                                           </Columns>
                                                         <PagerStyle CssClass="pagination-ys" />
                                                    </asp:GridView>
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
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
    <script type="text/javascript">
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
    </script>
</asp:Content>