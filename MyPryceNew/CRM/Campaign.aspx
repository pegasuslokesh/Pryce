<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="Campaign.aspx.cs" Inherits="CRM_Campaign" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Src="~/WebUserControl/AddTask.ascx" TagName="AddTask" TagPrefix="AT1" %>
<%@ Register Src="~/WebUserControl/FileUpload.ascx" TagPrefix="AT1" TagName="FileUpload1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
         <i class="fas fa-bullhorn"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Campaign %>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,CRM%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Campaign%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Campaign%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" Text="Bin" OnClick="Btn_Bin_Click" />
            <asp:Button ID="Btn_View_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#myModal" Text="View Modal" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
            <asp:HiddenField runat="server" ID="hdnCanAdd" />
            <asp:HiddenField runat="server" ID="hdnCanUpload" />
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
                    <li id="Li_New"><a href="#New" data-toggle="tab">
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
			<asp:Label ID="lblTotalRecords" Text="<%$ Resources:Attendance,Total Records: 0 %>" runat="server"></asp:Label><asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                                <asp:HiddenField ID="HDFSort" runat="server" />
                                                <asp:HiddenField ID="hdntxtaddressid" runat="server" />

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                <div class="col-md-6">
                                                    <asp:DropDownList ID="ddlLocation" runat="server" Class="form-control" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-12">
                                                    <br />
                                                </div>

                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged" CssClass="form-control">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Campaign Name %>" Value="Campaign_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Start Date %>" Value="Start_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,End Date%>" Value="End_Date"></asp:ListItem>
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
                                                        <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                                        <asp:TextBox ID="TxtValueDate" runat="server" Visible="false" class="form-control" placeholder="Search From Date"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" Format="dd-MMM-yyyy" ID="CalendarExtender3" runat="server" Enabled="True" TargetControlID="TxtValueDate">
                                                        </cc1:CalendarExtender>
                                                    </asp:Panel>
                                                </div>

                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="Refresh" runat="server" CausesValidation="False" OnClick="Refresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid">
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">

                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GV_Campaign_List" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                    runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                    OnPageIndexChanging="GV_Campaign_List_PageIndexChanging" OnSorting="GV_Campaign_List_Sorting">
                                                    <Columns>

                                                        <asp:TemplateField HeaderText="Action">
                                                            <ItemTemplate>
                                                                <div class="dropdown" style="position: absolute;">
                                                                    <button class="btn btn-default dropdown-toggle"  type="button" data-toggle="dropdown">
                                                                        <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                    </button>
                                                                    <ul class="dropdown-menu">

                                                                        <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                            <asp:LinkButton ID="Btn_Edit" runat="server" OnCommand="Btn_Edit_Command" CommandArgument='<%# Eval("Trans_ID") %>' CausesValidation="False" ToolTip="<%$ Resources:Attendance,Edit %>"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                        </li>
                                                                        <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                            <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_ID") %>' OnCommand="IbtnDelete_Command" ToolTip="<%$ Resources:Attendance,Delete %>"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                            <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="IbtnDelete"
                                                                                ConfirmText="Are you sure to Delete This Record?">
                                                                            </cc1:ConfirmButtonExtender>
                                                                        </li>

                                                                        <li <%= hdnCanUpload.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                            <asp:LinkButton ID="btnFileUpload" runat="server" AlternateText="File Upload" CausesValidation="False" OnCommand="btnFileUpload_Command" CommandArgument='<%# Eval("Trans_ID") %>' CommandName='<%# Eval("Campaign_Name") %>'><i class="fa fa-upload"></i>File Upload</asp:LinkButton>
                                                                        </li>
                                                                        <li <%= hdnCanAdd.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                            <asp:LinkButton ID="btnAssignTask" runat="server" AlternateText="Assign" CausesValidation="False" OnCommand="btnAssignTask_Command" CommandArgument='<%# Eval("Trans_ID") %>'><i class="fa fa-tasks"></i>Add Task</asp:LinkButton>
                                                                        </li>
                                                                    </ul>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>


                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Campaign Name %>" SortExpression="Campaign_Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCamp_Name_List" runat="server" Text='<%# Eval("Campaign_Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Start Date%>" SortExpression="Start_Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Lbl_Start_Date_List" runat="server" Text='<%#GetDate(Eval("Start_Date")) %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,End Date %>" SortExpression="End_Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Lbl_End_Date_List" runat="server" Text='<%#GetDate(Eval("End_Date")) %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Campaign Type %>" SortExpression="Campaign_type">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_Campaign_List" runat="server" Text='<%# Eval("Campaign_type") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Assign To%>" SortExpression="emp_name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_Assignto" runat="server" Text='<%# Eval("emp_name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Campaign Description %>" SortExpression="Description">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_Campaign_Description" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Attendance,Budget %>" SortExpression="Budget">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_Campaign_Buget" runat="server" Text='<%# Eval("Budget") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>
                                                    </Columns>


                                                    <PagerStyle CssClass="pagination-ys" />

                                                </asp:GridView>

                                            </div>
                                        </div>
                                    </div>
                                    <!-- /.box-body -->
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <asp:HiddenField ID="TableName" runat="server" Value="crm_campaign" />
                    <asp:Label ID="lblCampId" runat="server" Visible="false"></asp:Label>
                    <div class="tab-pane" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server">
                            <ContentTemplate>
                                <div>
                                    <asp:HiddenField ID="Edit_ID" runat="server" />
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">

                                                    <div class="col-md-12">
                                                        <asp:HiddenField ID="Trans_ID" runat="server" />
                                                        <asp:Label ID="Lbl_Campaign_Name" runat="server" Text="<%$ Resources:Attendance,Campaign Name %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:TextBox ID="Txt_Campaign_Name" runat="server" CssClass="form-control" OnTextChanged="Txt_Campaign_Name_TextChanged" AutoPostBack="true" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Start Date %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator runat="server" ID="reqName" ControlToValidate="Txt_Start_Date" ErrorMessage="Please enter Date!" ValidationGroup="Save" Display="Dynamic" />

                                                        <asp:TextBox ID="Txt_Start_Date" runat="server" class="form-control"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" Format="dd-MMM-yyyy" ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="Txt_Start_Date" OnClientDateSelectionChanged="Date">
                                                        </cc1:CalendarExtender>


                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,End Date%>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="Txt_End_Date" ErrorMessage="Please enter Date!" ValidationGroup="Save" Display="Dynamic" />

                                                        <asp:TextBox ID="Txt_End_Date" runat="server" class="form-control"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" Format="dd-MMM-yyyy" ID="CalendarExtender4" runat="server" OnClientDateSelectionChanged="function x(s, e) { s.set_MinimumAllowableDate(Txt_Start_Date); }" Enabled="True" TargetControlID="Txt_End_Date">
                                                        </cc1:CalendarExtender>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-4">
                                                        <asp:Label ID="Label12" runat="server" Text="Assigned To"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ControlToValidate="txtAssignTo" ErrorMessage="Please enter Date!" ValidationGroup="Save" Display="Dynamic" />
                                                        <asp:TextBox ID="txtAssignTo" runat="server" class="form-control" OnTextChanged="txtAssignTo_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtAssignTo"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>

                                                    <asp:HiddenField ID="hdnAssignedToEmpId" runat="server" />

                                                    <div class="col-md-4">
                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Campaign Type%>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:DropDownList ID="ddl_C_Type" runat="server" CssClass="form-control">
                                                            <asp:ListItem Text="Select" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="Advertisement"></asp:ListItem>
                                                            <asp:ListItem Text="Telemarketing"></asp:ListItem>
                                                            <asp:ListItem Text="Confrence"></asp:ListItem>
                                                            <asp:ListItem Text="Banner-Adds"></asp:ListItem>
                                                            <asp:ListItem Text="Referral Program"></asp:ListItem>
                                                            <asp:ListItem Text="Email"></asp:ListItem>
                                                            <asp:ListItem Text="Social Media"></asp:ListItem>
                                                            <asp:ListItem Text="Print"></asp:ListItem>
                                                            <asp:ListItem Text="Web"></asp:ListItem>
                                                            <asp:ListItem Text="Radio"></asp:ListItem>
                                                            <asp:ListItem Text="Television"></asp:ListItem>
                                                            <asp:ListItem Text="Trade Show"></asp:ListItem>
                                                            <asp:ListItem Text="Partners"></asp:ListItem>
                                                            <asp:ListItem Text="Other"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="Currency" runat="server" Text="<%$ Resources:Attendance,Currency %>" />
                                                        <asp:DropDownList ID="ddlCurrency" runat="server" Class="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Budget%>"></asp:Label>
                                                        <a style="color: Red">*</a><asp:TextBox ID="Txt_Budget" runat="server" class="form-control" MaxLength="9"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Description %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:TextBox ID="txt_Description" TextMode="MultiLine" MaxLength="1000" Style="resize: vertical; min-height: 60px; max-height: 150px; height: 60px;" runat="server" class="form-control"></asp:TextBox>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-6">
                                                        <div id="Div_Box_Add" runat="server" class="box box-info collapsed-box">
                                                            <div class="box-header with-border">
                                                                <h3 class="box-title">
                                                                    <asp:Label ID="lblAddProductDetail" runat="server" Text="Add Product Details"></asp:Label>
                                                                </h3>
                                                                <div class="box-tools pull-right">
                                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                        <i id="Btn_Add_Div" runat="server" class="fa fa-plus"></i>
                                                                    </button>
                                                                </div>
                                                            </div>
                                                            <div class="box-body">
                                                                <div class="form-group">
                                                                    <div class="col-md-12">

                                                                        <asp:Label ID="lblProductName" runat="server" Text="<%$ Resources:Attendance,Product Name %>" />
                                                                        <a style="color: Red">*</a>
                                                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtProductName" ErrorMessage="Enter Product" ValidationGroup="addproduct" Display="Dynamic" />
                                                                        <br />
                                                                        <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtProductName_TextChanged" BackColor="#eeeeee" />
                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                                            Enabled="True" ServiceMethod="GetCompletionListProductName" ServicePath="" CompletionInterval="100"
                                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtProductName"
                                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                        </cc1:AutoCompleteExtender>

                                                                        <asp:HiddenField ID="hdnNewProductId" runat="server" Value="0" />

                                                                    </div>
                                                                    <br />
                                                                    <br />
                                                                    <br />
                                                                    <div class="col-md-11">
                                                                        <asp:Label ID="Label11" runat="server" Text="Target Sales (Quantity)"></asp:Label><a style="color: Red">*</a>
                                                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="Txt_Target" ErrorMessage="Enter Product" ValidationGroup="addproduct" Display="Dynamic" />
                                                                        <asp:TextBox ID="Txt_Target" runat="server" class="form-control" MaxLength="4"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" TargetControlID="Txt_Target" FilterType="Numbers" FilterMode="ValidChars"></cc1:FilteredTextBoxExtender>
                                                                        <asp:CompareValidator ID="CompareValidator2" runat="server" ValueToCompare="0" ControlToValidate="Txt_Target" ErrorMessage="Must Be Greater than 0" Operator="NotEqual" Type="Integer"></asp:CompareValidator>
                                                                        <br />
                                                                    </div>
                                                                    <div class="col-md-1">
                                                                        <br />

                                                                        <asp:Button ID="ctl00_MainContent_Btnadd" runat="server" Text="<%$ Resources:Attendance,+ %>" CssClass="btn btn-primary" OnClick="btnProductAdd_Click" ValidationGroup="addproduct" CausesValidation="true" />
                                                                    </div>


                                                                    <div class="col-md-12" style="overflow: auto; max-height: 250px">
                                                                        <div class="progress-group">
                                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="Gv_Product_Add" runat="server" AutoGenerateColumns="False" Width="100%" OnRowDeleting="Gv_Product_Add_RowDeleting">

                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:HiddenField ID="hdnProductID" runat="server" Value='<%# Eval("ProductId") %>' />
                                                                                            <asp:LinkButton ID="IBtn_Delete_Duty" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ProductId") %>' ToolTip="<%$ Resources:Attendance,Delete %>"><i class="fa fa-trash"></i></asp:LinkButton>

                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name%>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="Lbl_ProductName" runat="server" Text='<%# Eval("EProductName") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Target Of Sale%>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="Lbl_TargetQty" runat="server" Text='<%# Eval("Target_sales_qty") %>'></asp:Label>
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
                                                    </div>
                                                    <br />

                                                    <div class="col-md-6">
                                                        <div id="Div_Box_Add1" runat="server" class="box box-info collapsed-box">
                                                            <div class="box-header with-border">
                                                                <h3 class="box-title">
                                                                    <asp:Label ID="lblAddContact" runat="server" Text="<%$ Resources:Attendance, Add Contacts%>"></asp:Label></h3>
                                                                <div class="box-tools pull-right">
                                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                        <i id="Btn_Add_Div1" runat="server" class="fa fa-plus"></i>
                                                                    </button>
                                                                </div>
                                                            </div>
                                                            <div class="box-body">
                                                                <div class="form-group">
                                                                    <div class="col-md-10">

                                                                        <asp:Label ID="lbl_contact_Name" runat="server" Text="<%$ Resources:Attendance,Contact Name %>" />
                                                                        <a style="color: Red">*</a><asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="txt_contactName" ErrorMessage="Enter Contact Name" ValidationGroup="addcontact" Display="Dynamic" />
                                                                        <br />
                                                                        <asp:TextBox ID="txt_contactName" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txt_contactName_TextChanged" BackColor="#eeeeee" />
                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1" ServiceMethod="GetCompletionListCustomer" TargetControlID="txt_contactName" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition"></cc1:AutoCompleteExtender>
                                                                        <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
                                                                        <br />
                                                                    </div>
                                                                    <div class="col-md-2">
                                                                        <br />
                                                                        <asp:Button ID="Btn_Add_New_Contact" runat="server" Text="<%$ Resources:Attendance,Add New %>"
                                                                            CssClass="btn btn-primary" OnClick="Btn_Add_New_Contact_Click" />
                                                                    </div>
                                                                    <div class="col-md-5">

                                                                        <asp:Label ID="lblEmployeeToVisit" runat="server" Text="Employee To Visit" />
                                                                        <asp:TextBox ID="txtEmployeeToVisit" runat="server" OnTextChanged="txtAssignTo_TextChanged" AutoPostBack="true" CssClass="form-control" BackColor="#eeeeee" />
                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1" MinimumPrefixLength="1" ServiceMethod="GetCompletionListEmployeeName" TargetControlID="txtEmployeeToVisit" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition"></cc1:AutoCompleteExtender>
                                                                        <br />
                                                                    </div>

                                                                    <div class="col-md-3">
                                                                        <asp:Label ID="lblVisitDate" runat="server" Text="Visit Date" />
                                                                        <asp:TextBox ID="txtVisitDate" runat="server" class="form-control"></asp:TextBox>
                                                                        <cc1:CalendarExtender OnClientShown="showCalendar" Format="dd-MMM-yyyy" ID="CalendarExtender5" runat="server" Enabled="True" TargetControlID="txtVisitDate">
                                                                        </cc1:CalendarExtender>
                                                                        <br />
                                                                    </div>

                                                                    <div class="col-md-3">
                                                                        <asp:Label ID="lblVisitClosedDate" runat="server" Text="Visit Closed Date" />
                                                                        <asp:TextBox ID="txtVisitClosedDate" runat="server" class="form-control"></asp:TextBox>
                                                                        <cc1:CalendarExtender OnClientShown="showCalendar" Format="dd-MMM-yyyy" ID="CalendarExtender6" runat="server" Enabled="True" TargetControlID="txtVisitClosedDate">
                                                                        </cc1:CalendarExtender>
                                                                        <br />
                                                                    </div>


                                                                    <div class="col-md-1">

                                                                        <br />
                                                                        <asp:Button ID="Btn_Contactadd" runat="server" Text="<%$ Resources:Attendance,+ %>"
                                                                            CssClass="btn btn-primary" OnClick="Btn_Contactadd_Click" ValidationGroup="addcontact" CausesValidation="true" />
                                                                    </div>



                                                                    <div class="col-md-12" style="overflow: auto; max-height: 250px">
                                                                        <div class="progress-group">
                                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="Gv_Contact_Add" runat="server" DataKeyNames="contact_id"
                                                                                AutoGenerateColumns="False" Width="100%" OnRowDeleting="Gv_Contact_Add_RowDeleting">

                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete%>">
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton ID="IBtn_Delete_Duty" runat="server" CommandName="Delete" CommandArgument='<%# Eval("contact_id") %>' ToolTip="<%$ Resources:Attendance,Delete %>"><i class="fa fa-trash"></i></asp:LinkButton>

                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Contact Name%>">
                                                                                        <ItemTemplate>
                                                                                            <asp:HiddenField ID="Hdn_Trans_ID_Master" runat="server" Value='<%# Eval("contact_id") %>'></asp:HiddenField>
                                                                                            <%--<asp:Label ID="Lbl_Duty_Title_Master_ID" Visible="false" runat="server" Text='<%# Eval("ProductId") %>'></asp:Label>--%>
                                                                                            <asp:Label ID="Lbl_Contact_Master" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>


                                                                                    <asp:TemplateField HeaderText="Employee">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="Lbl_EmpToVisit" runat="server" Text='<%# Eval("empToVisit") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>


                                                                                    <asp:TemplateField HeaderText="Date">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="Lbl_Visitdate" runat="server" Text='<%# Eval("Field2") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle />
                                                                                    </asp:TemplateField>


                                                                                    <asp:TemplateField HeaderText="Closed Date">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="Lbl_VisitClosedDate" runat="server" Text='<%# hasDateOrNot(Eval("Field3").ToString()) %>'></asp:Label>
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
                                                    </div>

                                                    <br />


                                                    <div class="col-md-12" style="text-align: center">

                                                        <asp:Button ID="Btn_Save" runat="server" CssClass="btn btn-primary" Visible="false" OnClick="Btn_Save_Click" Text="<%$ Resources:Attendance,Save%>" />
                                                        <asp:Button ID="Btn_Reset" runat="server" CssClass="btn btn-primary" OnClick="Btn_Reset_Click" Text="<%$ Resources:Attendance,Reset%>" />
                                                        <asp:Button ID="Btn_Cancel" runat="server" CssClass="btn btn-primary" OnClick="Btn_Cancel_Click" Text="<%$ Resources:Attendance,Cancel%>" />
                                                        <asp:HiddenField ID="editid" runat="server" />

                                                        <asp:Button ID="Btn_Address_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#task" Text="Assign Task" />
                                                        <asp:Button ID="Btn_Modal_FileUpload" Style="display: none;" runat="server" data-toggle="modal" data-target="#Fileupload123" Text="FileUpload" />


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

                    <div class="modal fade" id="task" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
                        aria-hidden="true">
                        <div class="modal-dialog modal-lg">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal">
                                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>

                                </div>
                                <div class="modal-body">
                                    <AT1:AddTask ID="AddTaskUC" runat="server" />
                                </div>
                                <div class="modal-footer">
                                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                                        Close</button>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="tab-pane" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server">
                            <ContentTemplate>


                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div2" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label14" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				  <asp:Label ID="lblbinTotalRecords" Text="<%$ Resources:Attendance,Total Records: 0 %>"
                      runat="server"></asp:Label>
                                                <asp:Label ID="Label7" runat="server" Visible="false"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">


                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlbinFieldName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlbinFieldName_SelectedIndexChanged" CssClass="form-control">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Campaign Name %>" Value="Campaign_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Start Date %>" Value="Start_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,End Date%>" Value="End_Date"></asp:ListItem>
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
                                                <div class="col-lg-5">
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbinbind">
                                                        <asp:TextBox ID="txtbinValue" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                                        <asp:TextBox ID="txtbinValueDate" runat="server" Visible="false" class="form-control" placeholder="Search From Date"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" Format="dd-MMM-yyyy" ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtbinValueDate">
                                                        </cc1:CalendarExtender>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2" style="text-align: center;">
                                                    <asp:LinkButton ID="btnbinbind" runat="server" CausesValidation="False" OnClientClick="tab_3_open()" OnClick="btnbinbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="btnbinRefresh" runat="server" CausesValidation="False" OnClick="btnbinRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="imgBtnRestore" CausesValidation="False" Visible="false" runat="server" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="ImgbtnSelectAll" Visible="false" runat="server" OnClick="ImgbtnSelectAll_Click" ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>
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

                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="Gv_Camapign_Bin" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' DataKeyNames="Trans_ID"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                        OnPageIndexChanging="Gv_Camapign_Bin_PageIndexChanging" OnSorting="Gv_Camapign_Bin_Sorting">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChanged" AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkgvSelect" runat="server" OnCheckedChanged="chkgvSelect_CheckedChanged" />
                                                                    <asp:HiddenField ID="hdnTrans_Id" runat="server" Value='<%#Eval("Trans_ID") %>' Visible="false" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Campaign Name %>" SortExpression="Campaign_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Trans_ID" runat="server" Visible="false" Text='<%# Eval("Trans_ID") %>'></asp:Label>
                                                                    <asp:Label ID="lbl_Camp_Name_Bin" runat="server" Text='<%# Eval("Campaign_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Start Date%>" SortExpression="Start_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_Start_Date_Bin" runat="server" Text='<%#GetDate(Eval("Start_Date"))%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,End Date %>" SortExpression="End_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Lbl_End_Date_Bin" runat="server" Text='<%#GetDate(Eval("End_Date")) %>'></asp:Label>
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

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Update_Bin">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <div class="modal fade" id="Fileupload123" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">

                <div class="modal-body">

                    <AT1:FileUpload1 runat="server" ID="FUpload1" />

                </div>
                <div class="modal-footer">
                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>


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

        function Modal_Address_Open() {
            document.getElementById('<%= Btn_Address_Modal.ClientID %>').click();
        }
        function Modal_Address_Close() {

            document.getElementById('<%= Btn_Address_Modal.ClientID %>').click();

        }
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }

        function View_Modal_Popup() {
            document.getElementById('<%= Btn_View_Modal.ClientID %>').click();
        }
        function Close_Modal_Popup() {
            document.getElementById('<%= Btn_View_Modal.ClientID %>').click();
        }
        function Div_AddProduct_Open() {
            $("#Btn_Add_Div").removeClass("fa fa-plus");
            $("#Div_Box_Add").removeClass("box box-primary collapsed-box");

            $("#Btn_Add_Div").addClass("fa fa-minus");
            $("#Div_Box_Add").addClass("box box-primary");

        }

        function Div_ContactAdd_Open() {
            $("#Btn_Add_Div1").removeClass("fa fa-plus");
            $("#Div_Box_Add1").removeClass("box box-primary collapsed-box");

            $("#Btn_Add_Div1").addClass("fa fa-minus");
            $("#Div_Box_Add1").addClass("box box-primary");

            $("#Btn_Add_Div").removeClass("fa fa-plus");
            $("#Div_Box_Add").removeClass("box box-primary collapsed-box");

            $("#Btn_Add_Div").addClass("fa fa-minus");
            $("#Div_Box_Add").addClass("box box-primary");
        }
        function isNumeric(keyCode) {
            return ((keyCode >= 48 && keyCode <= 57) || keyCode == 8)
        }

        function isAlpha(keyCode) {
            return ((keyCode >= 65 && keyCode <= 90) || keyCode == 8)
        }

        function isCampaign_Name(keyCode) {
            return ((keyCode >= 48 && keyCode <= 57) || (keyCode >= 65 && keyCode <= 90) || keyCode == 8)
        }
        function Modal_Open_FileUpload() {
            document.getElementById('<%= Btn_Modal_FileUpload.ClientID %>').click();
        }

        <%-- $(function Date() {
        var date = new Date();
        var currentMonth = date.getMonth(); // current month
        var currentDate = date.getDate(); // current date
        var currentYear = date.getFullYear(); //this year
        $("#<%= Txt_Start_Date.ClientID %>").datepicker({
            changeMonth: true, // this will allow users to chnage the month
            changeYear: true, // this will allow users to chnage the year
            minDate: new Date(currentYear, currentMonth, currentDate)
           
        });
    });--%>
    
    </script>
</asp:Content>

