<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="VehicleMaster.aspx.cs" Inherits="ProjectManagement_VehicleMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">

    <h1>        
        <i class="fas fa-bus-alt"></i>

    <h1>        
        <img src="../Images/InvStyle.png" alt="" />

    <h1>
        <i class="fas fa-truck-moving"></i>

        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Vehicle Master %>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Transport%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Transport%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Vehicle Master%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
            <asp:Button ID="Btn_List_Li" Style="display: none;" runat="server" OnClick="Btn_List_Li_Click" Text="List" />
            <asp:Button ID="Btn_New_Li" Style="display: none;" runat="server" OnClick="Btn_New_Li_Click" Text="New" />
            <asp:Button ID="Btn_Bin_Li" Style="display: none;" runat="server" OnClick="Btn_Bin_Li_Click" Text="Bin" />
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
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" class="form-control" OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Vehicle Name%>" Value="Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Number Plate%>" Value="Plate_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Model No%>" Value="Model_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Registration No%>" Value="Reg_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Owner Name%>" Value="Owner"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Vehicle Type%>" Value="Field1"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Insurance Expiry Date%>" Value="Expiry_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Puc Expiry Date%>" Value="Pvc_Expiry_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Service Due Date%>" Value="ServiceDueDate"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="Created_User"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Modified By%>" Value="Modified_User"></asp:ListItem>
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
                                                    <asp:Panel ID="Pnl_List" runat="server" DefaultButton="btnbind">
                                                        <asp:TextBox ID="txtValue" placeholder="Search from Content" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <asp:TextBox ID="Txt_Search_Date" placeholder="Search from Date" Visible="false" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CE_Search_Date" runat="server" TargetControlID="Txt_Search_Date"></cc1:CalendarExtender>
                                                    </asp:Panel>
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
                                                        Visible="false" OnClick="Img_Emp_List_Delete_All_Click"><span class="fas fa-remove"  style="font-size:25px;"></span></asp:LinkButton>
                                                    <cc1:ConfirmButtonExtender ID="Delete_Confirm_Button" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>"
                                                        TargetControlID="Img_Emp_List_Delete_All">
                                                    </cc1:ConfirmButtonExtender>
                                                </div>


                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div class="box box-primary box-solid" <%= Gv_Vehicle_Master_List.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-header with-border">
                                        <div class="col-md-1" style="margin-top: 8px;">
                                            <h3 class="box-title">Filter</h3>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:DropDownList ID="ddlFilter" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlFilter_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Text="--Select--" Value="Select"></asp:ListItem>
                                                <asp:ListItem Text="Insurance Expiry Vehicle" Value="Insurance"></asp:ListItem>
                                                <asp:ListItem Text="PUC Expiry Vehicle" Value="PUC"></asp:ListItem>
                                                <asp:ListItem Text="Service Due Vehicle" Value="Service"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="Gv_Vehicle_Master_List" PageSize="<%# PageControlCommon.GetPageSize() %>" runat="server" DataKeyNames="Vehicle_Id"
                                                        AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" Width="100%" OnPageIndexChanging="Gv_Vehicle_Master_List_PageIndexChanging" OnSorting="Gv_Vehicle_Master_List_Sorting">

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

                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <div class="dropdown">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">


                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="Btn_Edit" runat="server" OnCommand="Btn_Edit_Command" CommandArgument='<%# Eval("Vehicle_Id") %>' CausesValidation="False"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IBtn_Delete" runat="server" OnCommand="IBtn_Delete_Command" CausesValidation="False" CommandArgument='<%# Eval("Vehicle_Id") %>' ToolTip="<%$ Resources:Attendance,Delete %>"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IBtn_Delete"></cc1:ConfirmButtonExtender>
                                                                            </li>


                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Vehicle Id %>" SortExpression="Vehicle_Id" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblvehicleId" runat="server" Text='<%# Eval("Vehicle_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Vehicle Name %>" SortExpression="Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Number Plate" SortExpression="Plate_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPlate_No" runat="server" Text='<%# Eval("Plate_No") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Model No %>" SortExpression="Model_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModel_No" runat="server" Text='<%# Eval("Model_No") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Registration No %>" SortExpression="Reg_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblReg_No" runat="server" Text='<%# Eval("Reg_No") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Driver Name%>" SortExpression="Emp_Name" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmp_Id" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Owner Name %>" SortExpression="Owner" Visible="true">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOwner_Name" runat="server" Text='<%# Eval("Owner") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Vehicle Type %>" SortExpression="Field1" Visible="true">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblVehicle_Type" runat="server" Text='<%# Eval("Field1") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Insurance Expiry Date %>" SortExpression="Expiry_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblExpiry_Date" runat="server" Text='<%# GetDate(Eval("Expiry_Date").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Puc Expiry Date" SortExpression="Pvc_Expiry_Date" Visible="true">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPvc_ExpiryDate" runat="server" Text='<%# GetDate(Eval("Pvc_Expiry_Date").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Service Due Date" SortExpression="ServiceDueDate" Visible="true">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPvc_ServiceDueDate" runat="server" Text='<%# GetDate(Eval("ServiceDueDate").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="Created_User">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCreated_User" runat="server" Text='<%# Eval("Created_User") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>" SortExpression="Modified_User">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModified_User" runat="server" Text='<%# Eval("Modified_User") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Remarks %>" SortExpression="Remarks" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
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
                                    <asp:HiddenField ID="Edit_Id" runat="server" />
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Lbl_name" runat="server" Text="Vehicle Name (with No.)"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtvehiclename" ErrorMessage="<%$ Resources:Attendance,Enter Vehicle Name %>"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtvehiclename" runat="server" AutoPostBack="true" OnTextChanged="txtvehiclename_TextChanged" BackColor="#eeeeee" CssClass="form-control" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListVehicleName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtvehiclename"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label7" runat="server" Text="Number Plate"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtplateno" ErrorMessage="Enter Plate No."></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtplateno" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label4" runat="server" Text="Vehicle Model No"></asp:Label>
                                                        <asp:TextBox ID="txtModelNo" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Attendance,Registration No%>"></asp:Label>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtregistrationNo" ErrorMessage="Enter Registration No."></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtregistrationNo" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Insurance Expiry Date%>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtexpiryDate" ErrorMessage="Enter Insurance Expiry Date"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtexpiryDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CE_Expiry_Date" runat="server" TargetControlID="txtexpiryDate" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label3" runat="server" Text="PUC Expiry Date"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtpvcexpirydate" ErrorMessage="Enter PVC Expiry Date"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtpvcexpirydate" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CE_PUC_Expiry_Date" runat="server" TargetControlID="txtpvcexpirydate" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,Vehicle Type%>"></asp:Label>
                                                        <asp:DropDownList ID="ddlVehicleType" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlVehicleType_OnSelectedIndexChanged">
                                                            <asp:ListItem Text="Own" Value="Own"></asp:ListItem>
                                                            <asp:ListItem Text="Rent" Value="Rent"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Owner Name%>"></asp:Label>
                                                        <a style="color: Red" id="A1" runat="server">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator6" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtownername" ErrorMessage="Enter Owner Name"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtownername" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListOwnerName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtownername"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Driver Name%>"></asp:Label>
                                                        <asp:TextBox ID="txtEmpName" runat="server" CssClass="form-control" BackColor="#eeeeee"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="txtEmpName_AutoCompleteExtender" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionList" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtEmpName" UseContextKey="True"
                                                            CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblProductName" runat="server" Text="<%$ Resources:Attendance,Product Code%>"></asp:Label>
                                                        <asp:TextBox ID="txtProductName" runat="server" AutoPostBack="true" OnTextChanged="txtProductName_TextChanged" BackColor="#eeeeee" CssClass="form-control"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="txtProductCode_AutoCompleteExtender" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListProductName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtProductName"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12"></div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblServiceDueDate" runat="server" Text="Service Due Date"></asp:Label>
                                                        <asp:TextBox ID="txtServiceDueDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CE_Service_Due_Date" runat="server" TargetControlID="txtServiceDueDate" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblServiceDueReading" runat="server" Text="Service Due Reading"></asp:Label>
                                                        <asp:TextBox ID="txtServiceDueReading" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                                            TargetControlID="txtServiceDueReading" ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Attendance,Remarks%>"></asp:Label>
                                                        <asp:TextBox ID="txtRemarks" runat="server" Style="resize: vertical; max-height: 200px; min-height: 50px;" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblOpeningDebit" runat="server" Text="Opening Debit Amount"></asp:Label>
                                                        <asp:TextBox ID="txtOpeningDebitAmt" runat="server" CssClass="form-control" OnBlur="SetOpeningBalance();"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                            TargetControlID="txtOpeningDebitAmt" ValidChars="1,2,3,4,5,6,7,8,9,0">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label15" runat="server" Text="Opening Credit Amount"></asp:Label>
                                                        <asp:TextBox ID="txtOpeningCreditAmt" runat="server" CssClass="form-control" OnBlur="SetOpeningBalance();"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                            TargetControlID="txtOpeningCreditAmt" ValidChars="1,2,3,4,5,6,7,8,9,0">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:UpdatePanel ID="Update_New_Button" runat="server">
                                                            <ContentTemplate>
                                                                <asp:Button ID="Btn_Save" ValidationGroup="Save" runat="server" Text="<%$ Resources:Attendance,Save %>" Visible="false" CssClass="btn btn-success" OnClick="Btn_Save_Click" />
                                                                <asp:Button ID="Btn_Reset" runat="server" Text="<%$ Resources:Attendance,Reset %>" CssClass="btn btn-primary" CausesValidation="False" OnClick="Btn_Reset_Click" />
                                                                <asp:Button ID="Btn_Cancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>" CssClass="btn btn-danger" CausesValidation="False" OnClick="Btn_Cancel_Click" />
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
                    </div>
                    <div class="tab-pane" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server">
                            <ContentTemplate>


                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div2" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label16" runat="server" Text="Advance Search"></asp:Label></h3>
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
                                                    <asp:DropDownList ID="ddlFieldNameBin" runat="server" class="form-control" OnSelectedIndexChanged="ddlFieldNameBin_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Vehicle Name%>" Value="Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Number Plate%>" Value="Plate_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Model No%>" Value="Model_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Registration No%>" Value="Reg_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Owner Name%>" Value="Owner"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Vehicle Type%>" Value="Field1"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Insurance Expiry Date%>" Value="Expiry_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Puc Expiry Date%>" Value="Pvc_Expiry_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Service Due Date%>" Value="ServiceDueDate"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="Created_User"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Modified By%>" Value="Modified_User"></asp:ListItem>
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
                                                    <asp:Panel ID="Pnl_Bin" runat="server" DefaultButton="btnbindBin">
                                                        <asp:TextBox ID="txtValueBin" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <asp:TextBox ID="Txt_Search_Date_Bin" Visible="false" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CE_Search_Date_Bin" runat="server" TargetControlID="Txt_Search_Date_Bin"></cc1:CalendarExtender>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3" style="text-align: center">
                                                    <asp:LinkButton ID="btnbindBin"  runat="server" CausesValidation="False" 
                                                        ToolTip="<%$ Resources:Attendance,Search %>" OnClick="btnbindBin_Click"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="btnRefreshBin" runat="server"  CausesValidation="False"
                                                         ToolTip="<%$ Resources:Attendance,Refresh %>" OnClick="btnRefreshBin_Click"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="Img_Emp_Bin_Select_All"  runat="server" 
                                                        ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true"  OnClick="Img_Emp_Bin_Select_All_Click"><span class="fa fa-th"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="Img_Emp_List_Active" OnClick="Img_Emp_List_Active_Click"  CausesValidation="true"
                                                        runat="server"  ToolTip="<%$ Resources:Attendance, Active All %>"><span class="fas fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>

                                                    <cc1:ConfirmButtonExtender ID="Active_Confirm_Button" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to active the record?%>"
                                                        TargetControlID="Img_Emp_List_Active">
                                                    </cc1:ConfirmButtonExtender>
                                                </div>

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
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="Gv_Vehicle_Master_Bin" PageSize="<%# PageControlCommon.GetPageSize() %>" runat="server" DataKeyNames="Vehicle_Id" OnPageIndexChanging="Gv_Vehicle_Master_Bin_PageIndexChanging" OnSorting="Gv_Vehicle_Master_Bin_Sorting"
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
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Active %>">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="IBtn_Active" runat="server" OnCommand="IBtn_Active_Command" CausesValidation="False" CommandArgument='<%# Eval("Vehicle_Id") %>' ImageUrl="~/Images/Active.png" Width="16px" ToolTip="<%$ Resources:Attendance,Active %>" />
                                                                    <cc1:ConfirmButtonExtender ID="Active_Confirrm" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to active the record?%>"
                                                                        TargetControlID="IBtn_Active">
                                                                    </cc1:ConfirmButtonExtender>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Vehicle Id %>" SortExpression="Vehicle_Id" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblvehicleId" runat="server" Text='<%# Eval("Vehicle_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Vehicle Name %>" SortExpression="Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Number Plate" SortExpression="Plate_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPlate_No" runat="server" Text='<%# Eval("Plate_No") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Model No %>" SortExpression="Model_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModel_No" runat="server" Text='<%# Eval("Model_No") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Registration No %>" SortExpression="Reg_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblReg_No" runat="server" Text='<%# Eval("Reg_No") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Driver Name%>" SortExpression="Emp_Name" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmp_Id" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Owner Name %>" SortExpression="Owner" Visible="true">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOwner_Name" runat="server" Text='<%# Eval("Owner") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Vehicle Type %>" SortExpression="Field1" Visible="true">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblVehicle_Type" runat="server" Text='<%# Eval("Field1") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Insurance Expiry Date %>" SortExpression="Expiry_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblExpiry_Date" runat="server" Text='<%# GetDate(Eval("Expiry_Date").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Puc Expiry Date" SortExpression="Pvc_Expiry_Date" Visible="true">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPvc_ExpiryDate" runat="server" Text='<%# GetDate(Eval("Pvc_Expiry_Date").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Service Due Date" SortExpression="ServiceDueDate" Visible="true">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPvc_ServiceDueDate" runat="server" Text='<%# GetDate(Eval("ServiceDueDate").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Created By %>" SortExpression="Created_User">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCreated_User" runat="server" Text='<%# Eval("Created_User") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Modified By %>" SortExpression="Modified_User" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModified_User" runat="server" Text='<%# Eval("Modified_User") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Remarks %>" SortExpression="Remarks" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
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
        function SetOpeningBalance() {
            var DrAmt = $("#<%=txtOpeningDebitAmt.ClientID%>").val();
            var CrAmt = $("#<%=txtOpeningCreditAmt.ClientID%>").val();
            if (DrAmt > 0) {
                $("#<%=txtOpeningCreditAmt.ClientID%>").val("0");
            }
            else if (CrAmt > 0) {
                $("#<%=txtOpeningDebitAmt.ClientID%>").val("0");
            }
    }
    </script>
</asp:Content>
