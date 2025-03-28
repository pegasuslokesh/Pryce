<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="Duty_Group.aspx.cs" Inherits="Duty_Group_Duty_Group" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <style>
        .GridStyle {
            border: 1px solid rgb(217, 231, 255);
            background-color: White;
            font-family: arial;
            font-size: 12px;
            border-collapse: collapse;
            margin-bottom: 0px;
        }

            .GridStyle tr {
                border: 1px solid rgb(217, 231, 255);
                color: Black;
                height: 25px;
            }
            /* Your grid header column style */
            .GridStyle th {
                background-color: rgb(217, 231, 255);
                border: none;
                text-align: left;
                font-weight: bold;
                font-size: 15px;
                padding: 4px;
                color: Black;
                text-align: center;
            }
            /* Your grid header link style */
            .GridStyle tr th a, .GridStyle tr th a:visited {
                color: Black;
            }

            .GridStyle tr th, .GridStyle tr td table tr td {
                border: none;
            }

            .GridStyle td {
                border-bottom: 1px solid rgb(217, 231, 255);
                padding: 2px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-user-friends"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Duty Group %>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Duty Group%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Duty Group%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Duty Group%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnCanView" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
            <asp:Button ID="Btn_List_Li" Style="display: none;" runat="server" OnClick="Btn_List_Li_Click" Text="List" />
            <asp:Button ID="Btn_New_Li" Style="display: none;" runat="server" OnClick="Btn_New_Li_Click" Text="New" />
            <asp:Button ID="Btn_Bin_Li" Style="display: none;" runat="server" OnClick="Btn_Bin_Li_Click" Text="Bin" />
            <asp:Button ID="Btn_View_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#View_Duty_Modal" Text="View Modal" />
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
					<asp:Label ID="Lbl_TotalRecords" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>" runat="server"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" class="form-control">
                                                        <asp:ListItem Selected="True" Text="Duty Group Title" Value="Title" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Designation%>" Value="Designation" />
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlOption" runat="server" class="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-5">
                                                    <asp:TextBox ID="txtValue" placeholder="Search from Content" runat="server" onkeypress="return Accept_Enter_Key_List(this);" class="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False"
                                                        ToolTip="<%$ Resources:Attendance,Search %>" OnClick="btnbind_Click"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefresh_Click"
                                                    ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="Img_Emp_List_Select_All" runat="server"
                                                    ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true" OnClick="Img_Emp_List_Select_All_Click"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;


                                                <asp:LinkButton ID="Img_Emp_List_Delete_All" runat="server"
                                                    ToolTip="<%$ Resources:Attendance, Delete All %>" AutoPostBack="true"
                                                    OnClick="Img_Emp_List_Delete_All_Click"><span class="fa fa-remove"  style="font-size:30px;"></span></asp:LinkButton>
                                                    <cc1:ConfirmButtonExtender ID="Delete_Confirm_Button" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>"
                                                        TargetControlID="Img_Emp_List_Delete_All">
                                                    </cc1:ConfirmButtonExtender>
                                                </div>


                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid" <%= Gv_Duty_Group_List.Rows.Count>0?"style='display:block'":"style='display:none'"%>>

                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="Gv_Duty_Group_List" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server" DataKeyNames="Trans_ID"
                                                        AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" Width="100%" OnPageIndexChanging="Gv_Duty_Group_List_PageIndexChanging" OnSorting="Gv_Duty_Group_List_Sorting">

                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="Chk_Gv_Select_All" runat="server" AutoPostBack="true" OnCheckedChanged="Chk_Gv_Select_All_CheckedChanged" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="Chk_Gv_Select" runat="server" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">


                                                                            <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="Btn_View" runat="server" OnCommand="Btn_View_Command" CommandArgument='<%# Eval("Trans_ID") %>' CausesValidation="False" ToolTip="<%$ Resources:Attendance,View %>"><i class="fa fa-eye"></i>View</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="Btn_Edit" runat="server" OnCommand="Btn_Edit_Command" CommandArgument='<%# Eval("Trans_ID") %>' CausesValidation="False" ToolTip="<%$ Resources:Attendance,Edit %>"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IBtn_Delete" runat="server" OnCommand="IBtn_Delete_Command" CausesValidation="False" CommandArgument='<%# Eval("Trans_ID") %>' ToolTip="<%$ Resources:Attendance,Delete %>"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IBtn_Delete"></cc1:ConfirmButtonExtender>
                                                                            </li>


                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Duty Group Title" ItemStyle-Width="30%" SortExpression="Title">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Group_Title_List" runat="server" Text='<%# Eval("Title") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Group Description%>" SortExpression="Description">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Group_Description_List" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Designation%>" ItemStyle-Width="30%" SortExpression="Designation">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Designation_List" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
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
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server">
                            <Triggers>
                                <%--<asp:PostBackTrigger ControlID="btnGetDuties" />--%>
                            </Triggers>
                            <ContentTemplate>
                                <div>
                                    <asp:HiddenField ID="Edit_ID" runat="server" />
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-4">
                                                        <asp:HiddenField ID="HiddenField1" runat="server" />
                                                        <asp:Label ID="Lbl_Title" runat="server" Font-Bold="true" Text="Duty Group Title"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_Group_Title" ErrorMessage="<%$ Resources:Attendance,Enter Title%>"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="Txt_Group_Title" OnTextChanged="Txt_Group_Title_TextChanged" AutoPostBack="true" MaxLength="200" runat="server" BackColor="#eeeeee" CssClass="form-control"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="Duty_Title_Auto_Complete" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="Get_Duty_Group" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="Txt_Group_Title"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="Label2" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Designation%>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save" Display="Dynamic"
                                                            SetFocusOnError="true" ControlToValidate="Ddl_Designation" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Designation %>" />

                                                        <asp:DropDownList ID="Ddl_Designation" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-4" style="vertical-align:middle" style="margin-top: 25px;">
                                                        
                                                        <asp:Button ID="btnGetDuties" runat="server"  class="btn btn-success" Text="Get Duties" OnClick="btnGetDuties_Click" />
                                                    </div>

                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label3" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Duty%>"></asp:Label>
                                                        <div class="input-group" style="width: 100%">
                                                            <asp:TextBox ID="Txt_Duty_Title" MaxLength="200" runat="server" BackColor="#eeeeee" CssClass="form-control"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="Get_Duty_Title" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="Txt_Duty_Title"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <div class="input-group-btn">
                                                                <asp:Button ID="Btn_Add_Duty" Style="margin-top: -5px;" CssClass="form-control" runat="server" Text="Add" OnClick="Btn_Add_Duty_Click" />
                                                            </div>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div style="overflow: auto; max-height: 500px;">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="Gv_Duty_Master_List" runat="server" DataKeyNames="Trans_ID"
                                                                AutoGenerateColumns="False" Width="100%" OnRowDeleting="Gv_Duty_Master_List_RowDeleting">

                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="IBtn_Delete_Duty" runat="server" CommandName="Delete" CommandArgument='<%# Eval("Trans_ID") %>' ImageUrl="~/Images/Erase.png" Width="16px" ToolTip="<%$ Resources:Attendance,Delete %>" />
                                                                            <cc1:ConfirmButtonExtender ID="Delete_Confirrm" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>"
                                                                                TargetControlID="IBtn_Delete_Duty">
                                                                            </cc1:ConfirmButtonExtender>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Duty Title%>" ItemStyle-Width="30%">
                                                                        <ItemTemplate>
                                                                            <asp:HiddenField ID="Hdn_Trans_ID_Master" runat="server" Value='<%# Eval("Trans_ID") %>'></asp:HiddenField>
                                                                            <asp:Label ID="Lbl_Duty_Title_Master" runat="server" Text='<%# Eval("Title") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Duty Description%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="Lbl_Duty_Description_Master" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Duty Cycle%>" ItemStyle-Width="30%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="Lbl_Duty_Cycle_Master" runat="server" Text='<%#GetCycle(Eval("Duty_Cycle")) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                </Columns>

                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Lbl_Description" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Description%>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_Description" ErrorMessage="<%$ Resources:Attendance,Enter Description%>"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="Txt_Description" TextMode="MultiLine" Style="min-height: 100px; max-height: 500px; min-width: 100%; max-width: 100%;" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center;">
                                                        <asp:Button ID="Btn_Save" runat="server" ValidationGroup="Save" OnClick="Btn_Save_Click" Text="<%$ Resources:Attendance,Save %>" class="btn btn-success" />
                                                        <asp:Button ID="Btn_Reset" runat="server" Text="<%$ Resources:Attendance,Reset %>" class="btn btn-primary" OnClick="Btn_Reset_Click" CausesValidation="False" />
                                                        <asp:Button ID="Btn_Cancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>" class="btn btn-danger" OnClick="Btn_Cancel_Click" CausesValidation="False" />

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
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
                                                    <asp:Label ID="Label8" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="Lbl_TotalRecords_Bin" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>" runat="server"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlFieldNameBin" runat="server" class="form-control">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance, Title%>" Value="Title" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Designation%>" Value="Designation" />
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlOptionBin" runat="server" class="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-5">
                                                    <asp:TextBox ID="txtValueBin" placeholder="Search from Content" onkeypress="return Accept_Enter_Key_Bin(this);" runat="server" class="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-lg-3" style="text-align: center">
                                                    <asp:LinkButton ID="btnbindBin" runat="server" CausesValidation="False"
                                                        ToolTip="<%$ Resources:Attendance,Search %>" OnClick="btnbindBin_Click"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="btnRefreshBin" runat="server" CausesValidation="False"
                                                    ToolTip="<%$ Resources:Attendance,Refresh %>" OnClick="btnRefreshBin_Click"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="Img_Emp_Bin_Select_All" runat="server"
                                                    ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true" OnClick="Img_Emp_Bin_Select_All_Click"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="Img_Emp_List_Active" OnClick="Img_Emp_List_Active_Click" CausesValidation="true"
                                                    runat="server" ToolTip="<%$ Resources:Attendance, Active All %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <cc1:ConfirmButtonExtender ID="Active_Confirm_Button" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to active the record?%>"
                                                    TargetControlID="Img_Emp_List_Active">
                                                </cc1:ConfirmButtonExtender>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid" <%= Gv_Duty_Group_Bin.Rows.Count>0?"style='display:block'":"style='display:none'"%>>

                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="Gv_Duty_Group_Bin" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server" DataKeyNames="Trans_Id" OnPageIndexChanging="Gv_Duty_Group_Bin_PageIndexChanging" OnSorting="Gv_Duty_Group_Bin_Sorting"
                                                        AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" Width="100%">

                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="Chk_Gv_Select_All_Bin" runat="server" AutoPostBack="true" OnCheckedChanged="Chk_Gv_Select_All_Bin_CheckedChanged" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="Chk_Gv_Select_Bin" runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Active %>" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="IBtn_Active" Visible="false" runat="server" OnCommand="IBtn_Active_Command" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' ImageUrl="~/Images/Active.png" Width="16px" ToolTip="<%$ Resources:Attendance,Active %>" />
                                                                    <cc1:ConfirmButtonExtender ID="Active_Confirrm" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to active the record?%>"
                                                                        TargetControlID="IBtn_Active">
                                                                    </cc1:ConfirmButtonExtender>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Group Title%>" ItemStyle-Width="30%" SortExpression="Title">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Group_Title_Bin" runat="server" Text='<%# Eval("Title") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Group Description%>" SortExpression="Description">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Group_Description_Bin" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Designation%>" ItemStyle-Width="30%" SortExpression="Designation">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Designation_Bin" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
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
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="View_Duty_Modal" tabindex="-1" role="dialog" aria-labelledby="View_Duty_ModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="View_Duty_ModalLabel">Duty Group View</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="update_Modal" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label4" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Title%>"></asp:Label>
                                                    <asp:Label ID="Lbl_Title_Modal" runat="server" CssClass="form-control"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:Label ID="Label7" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Designation%>"></asp:Label>
                                                    <asp:Label ID="Lbl_Designation_Modal" runat="server" CssClass="form-control"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <div style="overflow: auto; max-height: 500px;">
                                                        <asp:Label ID="Lbl_Grid_Error" Visible="false" Text="<%$ Resources:Attendance,No Duty%>" runat="server"></asp:Label>
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="Gv_Duty_Modal" runat="server" DataKeyNames="Trans_ID" AutoGenerateColumns="False" Width="100%">

                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Duty Title%>" ItemStyle-Width="30%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Lbl_Duty_Title_Modal" runat="server" Text='<%# Eval("Title") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Duty Description%>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Lbl_Duty_Description_Modal" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Duty Cycle%>" ItemStyle-Width="30%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Lbl_Duty_Cycle_Modal" runat="server" Text='<%#GetCycle(Eval("Duty_Cycle")) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                            </Columns>

                                                            <PagerStyle CssClass="pagination-ys" />

                                                        </asp:GridView>
                                                    </div>
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <asp:Label ID="Label9" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Description%>"></asp:Label>
                                                    <asp:Label ID="Lbl_Description_Modal" TextMode="MultiLine" Style="resize: none; width: 100%; height: 100%" runat="server" CssClass="form-control"></asp:Label>
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
                    <asp:UpdatePanel ID="Update_Modal_Button" runat="server">
                        <ContentTemplate>
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

    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_New">
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

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
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

        function Li_Tab_List() {
            document.getElementById('<%= Btn_List_Li.ClientID %>').click();
        }
        function Li_Tab_New() {
            document.getElementById('<%= Btn_New_Li.ClientID %>').click();
        }
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin_Li.ClientID %>').click();
        }
        function Show_View_Duty_Modal() {
            document.getElementById('<%= Btn_View_Modal.ClientID %>').click();
        }


        function Accept_Enter_Key_List(elementRef) {
            var keyCodeEntered = (event.which) ? event.which : (window.event.keyCode) ? window.event.keyCode : -1;
            if ((keyCodeEntered == 13)) {
                document.getElementById('<%= btnbind.ClientID %>').click();
                return false;
            }
        }
        function Accept_Enter_Key_Bin(elementRef) {
            var keyCodeEntered = (event.which) ? event.which : (window.event.keyCode) ? window.event.keyCode : -1;
            if ((keyCodeEntered == 13)) {
                document.getElementById('<%= btnbindBin.ClientID %>').click();
                return false;
            }
        }
    </script>
</asp:Content>
