<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="Task.aspx.cs" Inherits="Duty_Master_Task" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <%--<script src="https://cdn.ckeditor.com/4.5.7/standard/ckeditor.js"></script>
    <link href="../Bootstrap_Files/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.min.css" rel="stylesheet" />--%>
    <style type="text/css">
        .ratingEmpty {
            background-image: url(../Images/ratingStarEmpty.gif);
            width: 18px;
            height: 18px;
        }

        .ratingFilled {
            background-image: url(../Images/ratingStarFilled.gif);
            width: 18px;
            height: 18px;
        }

        .ratingSaved {
            background-image: url(../Images/ratingStarSaved.gif);
            width: 18px;
            height: 18px;
        }

        .Selected {
            color: orange;
        }
    </style>

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
        <i class="fas fa-clipboard-list"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Task List %>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,HR%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Task List%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Task List%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="Hdn_Contact_Id" runat="server" />
            <asp:HiddenField ID="Emp_List_ID" runat="server" />
            <asp:HiddenField ID="Hdn_Select_Emp_ID" runat="server" />
            <asp:HiddenField ID="Edit_ID" runat="server" />
            <asp:HiddenField ID="Hdn_Conversation_ID" runat="server" />
            <asp:HiddenField ID="Hdn_Feedback_Trans" runat="server" />
            <asp:HiddenField ID="Hdn_Status" runat="server" />
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="Btn_Bin_Click" Text="Bin" />
            <asp:Button ID="Btn_Delete_Feedback" Style="display: none;" runat="server" OnClick="Btn_Delete_Feedback_Click" Text="Delete Feedback" />
            <asp:Button ID="Btn_View_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Modal_Conversation" Text="View Modal" />
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
                    <li runat="server" visible="false" id="Li_Bin"><a href="#Bin" onclick="Li_Tab_Bin()" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
                    <li runat="server" visible="false" id="Li_New"><a href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List" class="active"><a href="#List" data-toggle="tab">
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

                                                <div class="col-lg-3">
                                                    <asp:Label ID="Label2" Font-Bold="true" Text="<%$ Resources:Attendance,Start Date%>" runat="server"></asp:Label>
                                                    <asp:TextBox ID="Txt_Start_Date_List" runat="server" CssClass="form-control" />
                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="Txt_Start_Date_List">
                                                    </cc1:CalendarExtender>
                                                    <br />
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:Label ID="Label3" Font-Bold="true" Text="<%$ Resources:Attendance,Due Date%>" runat="server"></asp:Label>
                                                    <asp:TextBox ID="Txt_Due_Date_List" runat="server" CssClass="form-control" />
                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="Txt_Due_Date_List">
                                                    </cc1:CalendarExtender>
                                                    <br />
                                                </div>
                                                <div class="col-lg-6">
                                                    <asp:Label ID="Label4" Font-Bold="true" Text="<%$ Resources:Attendance,Employee%>" runat="server"></asp:Label>
                                                    <a style="color: Red">*</a>
                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Search" Display="Dynamic"
                                                        SetFocusOnError="true" ControlToValidate="Ddl_Employee_List" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Employee %>" />
                                                    <div class="input-group" style="width: 100%;">
                                                        <asp:DropDownList ID="Ddl_Employee_List" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        <div class="input-group-btn">
                                                            <asp:LinkButton ID="Img_Search" runat="server" ValidationGroup="Search" OnClick="Img_Search_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;margin-left:5px"></span></asp:LinkButton>
                                                        </div>
                                                    </div>
                                                    <br />
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged" AutoPostBack="true" class="form-control">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance, Task%>" Value="Title" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Start Date%>" Value="Start_Date" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Due Date%>" Value="Due_Date" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Status%>" Value="Status" />
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
                                                <div class="col-lg-6">
                                                    <asp:TextBox ID="txtValue" placeholder="Search from Content" onkeypress="return Accept_Enter_Key_List(this);" runat="server" class="form-control"></asp:TextBox>
                                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="BtnBindDate">
                                                        <asp:TextBox placeholder="Search from Date" ID="TxtValueDate" runat="server" Visible="false" class="form-control"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender3" runat="server" Enabled="True" TargetControlID="TxtValueDate">
                                                        </cc1:CalendarExtender>
                                                    </asp:Panel>

                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbind" OnClick="btnbind_Click" runat="server" CausesValidation="False" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="BtnBindDate" Visible="false" OnClick="BtnBindDate_Click" runat="server" CausesValidation="False" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="Img_Emp_List_Select_All" runat="server" ToolTip="<%$ Resources:Attendance, Select All %>" OnClick="Img_Emp_List_Select_All_Click" Visible="false"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="Img_Emp_List_Delete_All" runat="server" ToolTip="<%$ Resources:Attendance, Delete All %>" Visible="false" OnClick="Img_Emp_List_Delete_All_Click"><span class="fa fa-remove"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <cc1:ConfirmButtonExtender ID="Delete_Confirrm_Button" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="Img_Emp_List_Delete_All"></cc1:ConfirmButtonExtender>
                                                </div>

                                            </div>
                                        </div>
                                    </div>


                                </div>
                                <div class="box box-warning box-solid" <%= Gv_Feedback_List.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div style="overflow: auto; max-height: 500px;">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="Gv_Feedback_List" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server" DataKeyNames="Trans_ID"
                                                        AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" Width="100%" OnPageIndexChanging="Gv_Feedback_List_PageIndexChanging" OnSorting="Gv_Feedback_List_Sorting">

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
                                                                                <asp:LinkButton ID="Btn_Edit" runat="server" OnCommand="Btn_Edit_Command" CommandArgument='<%# Eval("Trans_ID") %>' CausesValidation="False" ><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IBtn_Delete" runat="server" OnCommand="IBtn_Delete_Command" CausesValidation="False" CommandArgument='<%# Eval("Trans_ID") %>'><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="Delete_Confirrm" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IBtn_Delete"> </cc1:ConfirmButtonExtender>
                                                                            </li>
                                                                            <li>
                                                                                <asp:LinkButton ID="Btn_Conversation" OnCommand="Btn_Conversation_Command" CommandArgument='<%# Eval("Trans_ID") %>' runat="server" CausesValidation="False"><i class="fa fa-comments"></i>Comments</asp:LinkButton>
                                                                            </li>
                                                                        </ul>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                           
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Task%>" SortExpression="Title">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_ID_List" runat="server" Visible="false" Text='<%# Eval("Trans_ID")%>'></asp:Label>
                                                                    <asp:Label ID="Lbl_Assign_To_ID_List" runat="server" Visible="false" Text='<%# Eval("Assign_To")%>'></asp:Label>
                                                                    <asp:Label ID="Lbl_Task_List" runat="server" ToolTip='<%# Eval("Description")%>' Text='<%# Eval("Title")%>'></asp:Label>
                                                                    <asp:Label ID="Lbl_Task_Description_List" runat="server" Visible="false" Text='<%# Eval("Description")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Start Date%>" SortExpression="Start_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Start_Date_List" runat="server" Text='<%# Convert.ToDateTime(Eval("Start_Date")).ToString("dd-MMM-yyyy")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Due Date%>" SortExpression="Due_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Due_Date_List" runat="server" Text='<%# Convert.ToDateTime(Eval("Due_Date")).ToString("dd-MMM-yyyy")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status%>" SortExpression="Status">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Status_List" runat="server" Text='<%# Eval("Status")%>'></asp:Label>
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
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label7" Font-Bold="true" Text="<%$ Resources:Attendance,Employee%>" runat="server"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save" Display="Dynamic"
                                                            SetFocusOnError="true" ControlToValidate="Ddl_Employee_New" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Employee %>" />
                                                        <asp:DropDownList ID="Ddl_Employee_New" runat="server" CssClass="form-control">
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div runat="server" visible="false" class="col-md-6">
                                                        <asp:Label ID="Label9" Font-Bold="true" Text="<%$ Resources:Attendance,Contact%>" runat="server"></asp:Label>
                                                        <asp:TextBox ID="Txt_Contact" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12"></div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label19" Font-Bold="true" Text="<%$ Resources:Attendance,Start Date%>" runat="server"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator7" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_Start_Date" ErrorMessage="<%$ Resources:Attendance,Enter Start Date%>"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="Txt_Start_Date" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender4" runat="server" Enabled="True" TargetControlID="Txt_Start_Date">
                                                        </cc1:CalendarExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label20" Font-Bold="true" Text="<%$ Resources:Attendance,Due Date%>" runat="server"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator8" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_Due_Date" ErrorMessage="<%$ Resources:Attendance,Enter Due Date%>"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="Txt_Due_Date" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender5" runat="server" Enabled="True" TargetControlID="Txt_Due_Date">
                                                        </cc1:CalendarExtender>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label8" Font-Bold="true" Text="<%$ Resources:Attendance,Title%>" runat="server"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_Title" ErrorMessage="<%$ Resources:Attendance,Enter Title%>"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="Txt_Title" runat="server" CssClass="form-control" MaxLength="200"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label15" Font-Bold="true" Text="<%$ Resources:Attendance,Description%>" runat="server"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <%--<asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="Editor_Description" ErrorMessage="<%$ Resources:Attendance,Enter Description%>"></asp:RequiredFieldValidator>--%>
                                                        <cc2:Editor ID="Editor_Description" runat="server" CssClass="form-control" />
                                                        <%--<textarea id="editor1" name="editor1" rows="10" cols="80"></textarea>--%>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12" style="text-align: center;">
                                                        <asp:Button ID="Btn_Save" runat="server" Visible="false" Text="<%$ Resources:Attendance,Save %>" ValidationGroup="Save" OnClick="Btn_Save_Click" class="btn btn-success" />
                                                        <asp:Button ID="Btn_Cancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>" OnClick="Btn_Cancel_Click" class="btn btn-danger" CausesValidation="False" />
                                                        <asp:Button ID="Btn_Reset" runat="server" Text="<%$ Resources:Attendance,Reset %>" OnClick="Btn_Reset_Click" class="btn btn-primary" CausesValidation="False" />
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
                    <div class="tab-pane" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div2" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label11" runat="server" Text="Advance Search"></asp:Label></h3>
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
                                                    <asp:DropDownList ID="ddlFieldNameBin" OnSelectedIndexChanged="ddlFieldNameBin_SelectedIndexChanged" AutoPostBack="true" runat="server" class="form-control">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance, Task%>" Value="Title" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Start Date%>" Value="Start_Date" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Due Date%>" Value="Due_Date" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Status%>" Value="Status" />
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
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="BtnBindDateBin">
                                                        <asp:TextBox ID="TxtValueDateBin" Visible="false" runat="server" placeholder="Search from Date" class="form-control"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender6" runat="server" Enabled="True" TargetControlID="TxtValueDateBin">
                                                        </cc1:CalendarExtender>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3" style="text-align: center">
                                                    <asp:LinkButton ID="btnbindBin" runat="server" CausesValidation="False" ToolTip="<%$ Resources:Attendance,Search %>" OnClick="btnbindBin_Click"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="BtnBindDateBin" Visible="false" runat="server" CausesValidation="False" ToolTip="<%$ Resources:Attendance,Search %>" OnClick="BtnBindDateBin_Click"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefreshBin" runat="server" CausesValidation="False" ToolTip="<%$ Resources:Attendance,Refresh %>" OnClick="btnRefreshBin_Click"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="Img_Emp_List_Active" OnClick="Img_Emp_List_Active_Click" Visible="false" CausesValidation="true" runat="server" ToolTip="<%$ Resources:Attendance, Active All %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <cc1:ConfirmButtonExtender ID="Active_Confirrm_Button" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to active the record?%>" TargetControlID="Img_Emp_List_Active"></cc1:ConfirmButtonExtender>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid" <%= Gv_Task_Bin.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div style="overflow: auto; max-height: 500px;">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="Gv_Task_Bin" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server" DataKeyNames="Trans_Id" OnPageIndexChanging="Gv_Task_Bin_PageIndexChanging" OnSorting="Gv_Task_Bin_Sorting"
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

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Task%>" SortExpression="Title">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_ID_Bin" runat="server" Visible="false" Text='<%# Eval("Trans_ID")%>'></asp:Label>
                                                                    <asp:Label ID="Lbl_Assign_To_ID_Bin" runat="server" Visible="false" Text='<%# Eval("Assign_To")%>'></asp:Label>
                                                                    <asp:Label ID="Lbl_Task_bin" runat="server" ToolTip='<%# Eval("Description")%>' Text='<%# Eval("Title")%>'></asp:Label>
                                                                    <asp:Label ID="Lbl_Task_Description_Bin" runat="server" Visible="false" Text='<%# Eval("Description")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Start Date%>" SortExpression="Start_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Start_Date_Bin" runat="server" Text='<%# Convert.ToDateTime(Eval("Start_Date")).ToString("dd-MMM-yyyy")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Due Date%>" SortExpression="Due_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Due_Date_Bin" runat="server" Text='<%# Convert.ToDateTime(Eval("Due_Date")).ToString("dd-MMM-yyyy")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Status%>" SortExpression="Status">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Status_Bin" runat="server" Text='<%# Eval("Status")%>'></asp:Label>
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

    <div class="modal fade" id="Modal_Conversation" tabindex="-1" role="dialog" aria-labelledby="Modal_ConversationLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Modal_ConversationLabel">Conversation</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="update_Modal" runat="server">
                        <ContentTemplate>
                            <!-- Chat box -->
                            <div class="box box-success">
                                <div class="box-header">
                                    <i class="fa fa-comments-o"></i>
                                    <h3 class="box-title">
                                        <asp:Label ID="Lbl_C_Task" Font-Bold="true" runat="server"></asp:Label></h3>
                                    <div class="box-tools pull-right" data-toggle="tooltip">
                                        <div class="btn-group" data-toggle="btn-toggle">
                                            <small class="text-muted pull-right"><i class="fa fa-clock-o"></i>&nbsp;
                                                    <asp:Label ID="Lbl_C_Date" runat="server"></asp:Label></small>&nbsp;
                                        </div>
                                    </div>
                                    <p>
                                        <asp:Literal ID="Ltr_C_Description" runat="server"></asp:Literal>
                                    </p>
                                    <hr />
                                </div>
                                <div class="box-body chat" id="chat-box">
                                    <div id="Div_Conversation" class='attachment' style="overflow: auto; max-height: 200px;">
                                        <asp:Literal ID="Ltr_Conversion" runat="server"></asp:Literal>
                                    </div>
                                </div>
                                <div class="box-footer">
                                    <cc1:Rating ID="ratingControl" OnChanged="ratingControl_Changed" Visible="false" AutoPostBack="true" runat="server" StarCssClass="ratingEmpty" WaitingStarCssClass="ratingSaved" EmptyStarCssClass="ratingEmpty" FilledStarCssClass="ratingFilled"></cc1:Rating>
                                    <br />
                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="Send" Display="Dynamic"
                                        SetFocusOnError="true" ControlToValidate="DDL_Status_Conversation" InitialValue="0" ErrorMessage="<%$ Resources:Attendance,Select Status %>" />
                                    <asp:DropDownList ID="DDL_Status_Conversation" Visible="false" runat="server" class="form-control">
                                        <asp:ListItem Selected="True" Text="Status" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Not Started" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Deferred" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="In-Progress" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Completed" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Waiting For Input" Value="5"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator6" ValidationGroup="Send"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="Txt_FeedBack" ErrorMessage="<%$ Resources:Attendance,Enter Feedback%>"></asp:RequiredFieldValidator>
                                    <asp:TextBox ID="Txt_FeedBack" runat="server" TextMode="MultiLine" MaxLength="1000" onKeyUp="textareaLengthCheck()" onkeypress="return Shift_Enter(this);" Style="resize: none; width: 100%; height: 70px;" Placeholder="Enter Feedback" CssClass="form-control"></asp:TextBox>
                                    <asp:Label ID="Feedback_Count" runat="server" class='text-muted pull-right' ForeColor="Red"></asp:Label>
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
                            <asp:Button ID="Btn_Send_Feedback" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="Btn_Send_Feedback_Click" ValidationGroup="Send" />
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
    <%--<script src="../Bootstrap_Files/plugins/bootstrap-wysihtml5/ckeditor.js"></script>
    <script src="../Bootstrap_Files/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.min.js"></script>--%>
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

            document.getElementById('<%=Li_New.ClientID%>').className = "active";
            $("#New").addClass("active");
        }
        function LI_List_Active() {
            $("#Li_List").addClass("active");
            $("#List").addClass("active");

            document.getElementById('<%=Li_New.ClientID%>').className = "";
            $("#New").removeClass("active");
        }
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }
        function View_Conversation() {
            document.getElementById('<%= Btn_View_Modal.ClientID %>').click();
        }
        //$(function () {
        //    CKEDITOR.replace('editor1');
        //    $(".textarea").wysihtml5();
        //});
        function Edit_Feedback(Trans_ID, User_ID, Emp_ID, RefTblPk, Feedback) {
            var regex = /<br\s*[\/]?>/gi;
            var feed = Feedback.replace(regex, "\n");
            document.getElementById('<%= Hdn_Status.ClientID %>').value = 'Edit';
            document.getElementById('<%= Txt_FeedBack.ClientID %>').value = feed;
            document.getElementById('<%= Txt_FeedBack.ClientID %>').focus();
        }

        function Delete_Feedback(Trans_ID, User_ID, Emp_ID, RefTblPk) {
            if (confirm("Are you sure you want to delete the record?") == true) {
                document.getElementById('<%= Hdn_Feedback_Trans.ClientID %>').value = Trans_ID;
                document.getElementById('<%= Hdn_Status.ClientID %>').value = 'Delete';
                document.getElementById('<%= Btn_Delete_Feedback.ClientID %>').click();
            }
            else
                return false;
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

        function textareaLengthCheck() {
            document.getElementById('<%=Txt_FeedBack.ClientID%>').maxLength = 1000;
            var length = document.getElementById('<%=Txt_FeedBack.ClientID%>').value.length;
            var charactersLeft = 1000 - length;
            var count = document.getElementById('<%=Feedback_Count.ClientID%>');
            count.innerHTML = "Characters left: " + charactersLeft;
        }

        $('#Modal_Conversation').on('shown.bs.modal', function () {
            $('#Div_Conversation').animate({
                scrollTop: $('#Div_Conversation')[0].scrollHeight
            }, 5);
            $("[data-modalfocus]", this).focus();
        });

        function On_Load_Modal() {
            $('#Div_Conversation').animate({
                scrollTop: $('#Div_Conversation')[0].scrollHeight
            }, 5);
        }

        function Shift_Enter(elementRef) {
            if (event.keyCode == 13 && event.shiftKey == true) {
                this.value = content.substring(0, caret - 1) + "\n" + content.substring(caret, content.length);
                event.stopPropagation();
                return false;
            }
            else if (event.keyCode == 13 && event.shiftKey == false) {
                document.getElementById('<%= Btn_Send_Feedback.ClientID %>').click();
                    return false;
                }
        }
    </script>
</asp:Content>

