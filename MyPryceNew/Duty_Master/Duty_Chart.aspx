<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="Duty_Chart.aspx.cs" Inherits="Duty_Master_Duty_Chart" %>

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
        <i class="fas fa-tasks"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Duty Chart %>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Duty Chart%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Duty Chart%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Duty Chart%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
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
                        <asp:HiddenField runat="server" ID="hdnCanEdit" />
                        <asp:HiddenField runat="server" ID="hdnCanDelete" />
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
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance, Employee%>" Value="Emp_Name" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Department%>" Value="Dep_Name" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Email Id%>" Value="Email_Id" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Phone No%>" Value="Phone_No" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, TL/Supervisor%>" Value="Emp_TL" />
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


                                <div class="box box-warning box-solid" <%= Gv_Duty_Chart_List.Rows.Count>0?"style='display:block'":"style='display:none'"%>>

                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">

                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="Gv_Duty_Chart_List" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server" DataKeyNames="Emp_ID"
                                                        AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" Width="100%" OnPageIndexChanging="Gv_Duty_Chart_List_PageIndexChanging" OnSorting="Gv_Duty_Chart_List_Sorting">

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
                                                                         

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="Btn_Edit"  runat="server" OnCommand="Btn_Edit_Command" CommandArgument='<%# Eval("Emp_ID") %>'  CausesValidation="False" ToolTip="<%$ Resources:Attendance,Edit %>" ><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                               <asp:LinkButton ID="IBtn_Delete"  runat="server" OnCommand="IBtn_Delete_Command" CausesValidation="False" CommandArgument='<%# Eval("Emp_ID") %>'  ToolTip="<%$ Resources:Attendance,Delete %>" ><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IBtn_Delete"></cc1:ConfirmButtonExtender>
                                                                            </li>

                                                                            
                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                           
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name%>" ItemStyle-Width="30%" SortExpression="Emp_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Emp_Name_List" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Department%>" ItemStyle-Width="30%" SortExpression="Dep_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Department_List" runat="server" Text='<%# Eval("Dep_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Designation%>" ItemStyle-Width="30%" SortExpression="designation">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_designation" runat="server" Text='<%# Eval("designation") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Email Id%>" ItemStyle-Width="30%" SortExpression="Email_Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Email_Id_List" runat="server" Text='<%# Eval("Email_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Phone No%>" ItemStyle-Width="30%" SortExpression="Phone_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Phone_No_List" runat="server" Text='<%# Eval("Phone_No") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,TL/Supervisor%>" ItemStyle-Width="30%" SortExpression="Emp_TL">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Emp_TL_List" runat="server" Text='<%# Eval("Emp_TL") %>'></asp:Label>
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
                            <ContentTemplate>
                                <div>
                                    <asp:HiddenField ID="Edit_ID" runat="server" />
                                    <asp:HiddenField ID="Hdn_Emp_ID" runat="server" />
                                    <asp:HiddenField ID="Hdn_Report_To_ID" runat="server" />
                                    <asp:HiddenField ID="Hdn_Report_To_Code" runat="server" />
                                    <asp:HiddenField ID="Hdn_Report_To_Name" runat="server" />
                                    <asp:HiddenField ID="Hdn_Emp_DOJ" runat="server" />
                                    <asp:HiddenField ID="Hdn_Emp_Code" runat="server" />
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Lbl_Emp_Name" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Employee Name%>"></asp:Label>
                                                        <asp:TextBox ID="Txt_Emp_Name" MaxLength="50" runat="server" Style="margin-top: 7px;" CssClass="form-control" BackColor="#eeeeee" AutoPostBack="true" OnTextChanged="Txt_Emp_Name_TextChanged" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="Txt_Emp_Name" UseContextKey="True"
                                                            CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Lbl_Duty_Group" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Duty List By %>"></asp:Label>
                                                        <asp:RadioButton ID="Rbt_Group" runat="server" Font-Bold="true" AutoPostBack="true" OnCheckedChanged="Rbt_Group_CheckedChanged" Style="margin-left: 20px;" Checked="true" GroupName="Duty_Group" Text="Group" />
                                                        <asp:RadioButton ID="Rbt_Duty" runat="server" Font-Bold="true" AutoPostBack="true" OnCheckedChanged="Rbt_Group_CheckedChanged" Style="margin-left: 20px;" GroupName="Duty_Group" Text="Duty" />
                                                        <div class="input-group" style="width: 100%">
                                                            <asp:TextBox ID="Txt_Duty_Group" MaxLength="50" runat="server" BackColor="#eeeeee" CssClass="form-control"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="Get_Duty_Group" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="Txt_Duty_Group" UseContextKey="True"
                                                                CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>

                                                            <asp:TextBox ID="Txt_Duty_Duty" MaxLength="50" runat="server" Visible="false" BackColor="#eeeeee" CssClass="form-control"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="Get_Duty_Title" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="Txt_Duty_Duty" UseContextKey="True"
                                                                CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <div class="input-group-btn">
                                                                <asp:Button ID="Btn_Add_Group" CssClass="form-control" runat="server" Text="Add" OnClick="Btn_Add_Group_Click" />
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-4">
                                                        <asp:Label ID="Label8" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Designation%>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                     
                                                        <asp:DropDownList ID="Ddl_Designation" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-2"  style="margin-top: 25px;">
                                                        
                                                        <asp:Button ID="btnGetDuties" runat="server"  class="btn btn-success" Text="Get Duties" OnClick="btnGetDuties_Click"
                                                             />
                                                    </div>


                                                    <div class="col-md-4">
                                                        <asp:Label ID="Label3" runat="server" Font-Bold="true" Text="Duties Copy To New Employee"></asp:Label>
                                                        <asp:TextBox ID="txtReferenceEmployee" MaxLength="50" runat="server" Style="margin-top: 7px;" CssClass="form-control" BackColor="#eeeeee" AutoPostBack="true" OnTextChanged="Ref_Emp_Name_TextChanged" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtReferenceEmployee" UseContextKey="True"
                                                            CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-2" style="margin-top: 25px;">
                                                           <asp:Button ID="Btn_Copy" runat="server" OnClick="Btn_Copy_Click" Text="Duties Copy" class="btn btn-success" />
                                                    </div>

                                                    <div class="col-md-12">
                                                        <div style="overflow: auto; max-height: 500px;">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="Gv_Duty_Master_List" runat="server" DataKeyNames="Trans_ID"
                                                                AutoGenerateColumns="False" >

                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>" ItemStyle-Width="10px">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="IBtn_Delete_Duty" runat="server" CommandArgument='<%# Eval("Trans_ID") %>' ImageUrl="~/Images/Erase.png" Width="16px" ToolTip="<%$ Resources:Attendance,Delete %>" OnCommand="IBtn_Delete_Duty_Command" />
                                                                            <cc1:ConfirmButtonExtender ID="Delete_Confirrm" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>"
                                                                                TargetControlID="IBtn_Delete_Duty">
                                                                            </cc1:ConfirmButtonExtender>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Duty Title%>" ItemStyle-Width="150px">
                                                                        <ItemTemplate>
                                                                            <asp:HiddenField ID="Hdn_Trans_ID_Master" runat="server" Value='<%# Eval("Trans_ID") %>'></asp:HiddenField>
                                                                            <asp:Label ID="Txt_Duty_Title_Master" ToolTip='<%# Eval("Title") %>' Enabled="false" runat="server" Text='<%# Eval("Title") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Duty Description%>" ItemStyle-Width="150px">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="Txt_Duty_Description_Master" Enabled="false" ToolTip='<%# Eval("Description") %>' runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Duty Cycle%>" ItemStyle-Width="80px">
                                                                        <ItemTemplate>
                                                                            <%--<asp:TextBox ID="Txt_Duty_Cycle_Master" runat="server" Text='<%#GetCycle(Eval("Duty_Cycle")) %>'></asp:TextBox>--%>
                                                                            <asp:DropDownList ID="Ddl_Duty_Cycle_Master"  SelectedValue='<%# Eval("Duty_Cycle") %>' runat="server" class="form-control">
                                                                                <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,--Select-- %>" Value="0"></asp:ListItem>
                                                                                <asp:ListItem Text="<%$ Resources:Attendance,Daily%>" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Text="<%$ Resources:Attendance,Weekly%>" Value="2"></asp:ListItem>
                                                                                <asp:ListItem Text="<%$ Resources:Attendance,Biweekly%>" Value="3"></asp:ListItem>
                                                                                <asp:ListItem Text="<%$ Resources:Attendance,Monthly%>" Value="4"></asp:ListItem>
                                                                                <asp:ListItem Text="<%$ Resources:Attendance,Quarterly%>" Value="5"></asp:ListItem>
                                                                                <asp:ListItem Text="<%$ Resources:Attendance,Half Yearly%>" Value="6"></asp:ListItem>
                                                                                <asp:ListItem Text="<%$ Resources:Attendance,Yearly%>" Value="7"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,WEF Date%>" ItemStyle-Width="40px">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="Txt_WEF_Date_Master" Text='<%#Eval("WEF_Date")%>' Width="110px" OnTextChanged="Txt_WEF_Date_TextChanged" AutoPostBack="true" runat="server" CssClass="form-control"></asp:TextBox>
                                                                            <%--<asp:RegularExpressionValidator runat="server" ControlToValidate="Txt_WEF_Date_Master" ValidationExpression="^(([0-9])|([0-2][0-9])|([3][0-1]))\-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\-\d{4}$" ErrorMessage="Please Enter a valid date in the format (dd-MMM-yyyy)." ValidationGroup="Save" />--%>
                                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender2" Format="dd-MMM-yyyy" runat="server" Enabled="True" TargetControlID="Txt_WEF_Date_Master">
                                                                            </cc1:CalendarExtender>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Report To%>" ItemStyle-Width="150px">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="Txt_Report_To_Master" runat="server" TextMode="MultiLine" Style="resize: none;" CssClass="form-control" BackColor="#eeeeee" Text='<%#Eval("Report_To") %>'></asp:TextBox>
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters="," ShowOnlyCurrentWordInCompletionListItem="true"
                                                                                Enabled="True" ServiceMethod="Get_Employee" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="Txt_Report_To_Master" UseContextKey="True"
                                                                                CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Create By%>" ItemStyle-Width="50px">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="Lbl_Create_by" Text='<%#Eval("Created_By")%>' runat="server"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By%>" ItemStyle-Width="50px">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="Lbl_Modified_by" Text='<%#Eval("Modified_By")%>' runat="server"></asp:Label>
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
                                                        <table style="width:100%">
                                                            <tr>
                                                                <td style="float: right;">
                                                                     <label> Total Duties  </label>
                                                                </td>
                                                                <td>
                                                                    <label> :  </label>
                                                                </td>
                                                               <td style="float: right;">
                                                                   <asp:Label ID="lblEmpTotalDuties" runat="server" />
                                                               </td>

                                                            </tr>
                                                              <tr>
                                                                  <td style="float: right;">
                                                                     <label> Total Daily Duties  </label>
                                                                </td>
                                                                <td>
                                                                    <label> :  </label>
                                                                </td>
                                                               <td style="float: right;">
                                                                   <asp:Label ID="lblEmpTotalDailyDuties" runat="server" />
                                                               </td>

                                                            </tr>
                                                              <tr>
                                                                 <td style="float: right;">
                                                                     <label> Total Weekly Duties  </label>
                                                                </td>
                                                                <td>
                                                                    <label> :  </label>
                                                                </td>
                                                               <td style="float: right;">
                                                                   <asp:Label ID="lblEmpTotalWeeklyDuties" runat="server" />
                                                               </td>

                                                            </tr>
                                                              <tr>
                                                                <td style="float: right;">
                                                                     <label> Total Monthly Duties  </label>
                                                                </td>
                                                                <td>
                                                                    <label> :  </label>
                                                                </td>
                                                               <td style="float: right;">
                                                                   <asp:Label ID="lblEmpTotalMonthlyDuties" runat="server" />
                                                               </td>

                                                            </tr>

                                                        </table>
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
                           <Triggers>
                               <%--    <asp:PostBackTrigger ControlID="Rbt_Group" />
                                <asp:PostBackTrigger ControlID="Rbt_Duty" />--%>
      <asp:AsyncPostBackTrigger ControlID="Rbt_Group" EventName="CheckedChanged" />
      <asp:AsyncPostBackTrigger ControlID="Rbt_Duty" EventName="CheckedChanged" />


                           </Triggers>
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
                                                    <asp:ListItem Selected="True" Text="<%$ Resources:Attendance, Employee%>" Value="Emp_Name" />
                                                    <asp:ListItem Text="<%$ Resources:Attendance, Duty%>" Value="Duty_Title" />
                                                    <%--<asp:ListItem Text="<%$ Resources:Attendance, Duty Cycle%>" Value="Duty_Cycle" />--%>
                                                    <asp:ListItem Text="<%$ Resources:Attendance, Report To%>" Value="Report_To_Name" />
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
                                                <asp:LinkButton ID="btnbindBin"  runat="server" CausesValidation="False" 
                                                    ToolTip="<%$ Resources:Attendance,Search %>" OnClick="btnbindBin_Click"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="btnRefreshBin" runat="server"  CausesValidation="False"
                                                   ToolTip="<%$ Resources:Attendance,Refresh %>" OnClick="btnRefreshBin_Click"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="Img_Emp_Bin_Select_All"  runat="server" 
                                                    ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true"  OnClick="Img_Emp_Bin_Select_All_Click" ><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="Img_Emp_List_Active" OnClick="Img_Emp_List_Active_Click"  CausesValidation="true"
                                                    runat="server"  ToolTip="<%$ Resources:Attendance, Active All %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <cc1:ConfirmButtonExtender ID="Active_Confirm_Button" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to active the record?%>"
                                                    TargetControlID="Img_Emp_List_Active">
                                                </cc1:ConfirmButtonExtender>
                                            </div>
                                           

				</div>
			</div>
		</div>
	</div>


                                <div class="box box-warning box-solid"  <%= Gv_Duty_Chart_Bin.Rows.Count>0?"style='display:block'":"style='display:none'"%> >
                                   
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="Gv_Duty_Chart_Bin" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server" DataKeyNames="Trans_Id" OnPageIndexChanging="Gv_Duty_Chart_Bin_PageIndexChanging" OnSorting="Gv_Duty_Chart_Bin_Sorting"
                                                        AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" Width="100%">

                                                        <Columns>
                                                            <asp:TemplateField >
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
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name%>" ItemStyle-Width="30%" SortExpression="Emp_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Emp_Name_Bin" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Duty%>" SortExpression="Duty_Title">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Duty_Bin" runat="server" Text='<%# Eval("Duty_Title") %>'></asp:Label>
                                                                    <asp:Label ID="Lbl_Duty_ID_Bin" Visible="false" runat="server" Text='<%# Eval("Duty_ID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Duty Cycle%>" SortExpression="Duty_Cycle">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Duty_Cycle_Bin" runat="server" Text='<%#GetCycle(Eval("Duty_Cycle"))%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Report To%>" SortExpression="Report_To_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Report_To_Bin" runat="server" Text='<%# Eval("Report_To_Name")%>'></asp:Label>
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
                    <h4 class="modal-title" id="View_Duty_ModalLabel">Duty Chart View</h4>
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
        function LI_Active_Active() {
            $("#Li_Bin").removeClass("active");
            $("#Bin").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
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

