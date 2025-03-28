<%@ Page Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="SalesLead.aspx.cs" Inherits="CRM_SalesLead" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/AddTask.ascx" TagName="AddTask" TagPrefix="AT1" %>
<%@ Register Src="~/WebUserControl/ContactInfo.ascx" TagName="ViewContact" TagPrefix="AT2" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-shopping-cart"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Sales Lead%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Sales%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Sales Lead%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:HiddenField ID="TableName" runat="server" Value="Inv_SalesLead" />
    <asp:HiddenField ID="Clicked" runat="server" Value="0" />
    <asp:HiddenField ID="SalesLeadId" runat="server" />
    <asp:HiddenField runat="server" ID="hdnCanEdit" />
    <asp:HiddenField runat="server" ID="hdnCanDelete" />
    <asp:HiddenField runat="server" ID="hdnCanView" />
    <asp:HiddenField runat="server" ID="hdnCanAdd" />


    <asp:Label ID="lblId" runat="server" Visible="false"></asp:Label>

    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">
                <ul class="nav nav-tabs pull-right bg-blue-gradient">
                    <li id="Li_Bin"><a href="#Bin" data-toggle="tab">
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
				<asp:Label ID="lblTotalRecords" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                <div class="col-md-6">
                                                    <asp:DropDownList ID="ddlLocation" runat="server" Class="form-control" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-6">
                                                    <asp:DropDownList ID="ddlUser" runat="server" Class="form-control"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlUser_Click">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-12">
                                                    <br />
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" Class="form-control"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged">

                                                        <asp:ListItem Text="Lead Date" Value="Lead_date"></asp:ListItem>
                                                        <asp:ListItem Text="Contact Name" Value="Emp_name_Contact"></asp:ListItem>
                                                        <asp:ListItem Text="Lead Status" Value="Lead_status"></asp:ListItem>
                                                        <asp:ListItem Text="Lead source" Value="Lead_source"></asp:ListItem>
                                                        <asp:ListItem Text="Generated By" Value="Emp_name_GeneratedBy"></asp:ListItem>
                                                        <asp:ListItem Text="Campaign Name" Value="Campaign_Name"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlOption" runat="server" Class="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>

                                                <div class="col-lg-6">
                                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                        <asp:TextBox ID="txtValue" Visible="false" runat="server" Class="form-control" placeholder="Search From Content"></asp:TextBox>
                                                        <asp:TextBox ID="txtValueDate" runat="server" Class="form-control" placeholder="Search From Date" Visible="true"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendartxtValueDate" runat="server" TargetControlID="txtValueDate" Format="dd-MMM-yyyy" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbindrpt_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefreshReport_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div class="box box-warning box-solid">
                                    <asp:HiddenField ID="hdnSalesLeadID" runat="server" />
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvSalesLead" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvSalesLead_PageIndexChanging"
                                                        AllowSorting="True" OnSorting="GvSalesLead_Sorting">
                                                        <Columns>

                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("Lead_Id") %>' ToolTip="View" OnCommand="lnkViewDetail_Command" CausesValidation="False"><i class="fa fa-eye"></i>View</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Lead_Id") %>' CausesValidation="False" OnCommand="lnkViewDetail_Command"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanAdd.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnAssignTask" runat="server" AlternateText="Assign" CausesValidation="False" OnCommand="btnAssignTask_Command" CommandArgument='<%# Eval("Lead_Id") %>'> <i class="fa fa-tasks"></i>Add Task</asp:LinkButton>
                                                                                <asp:HiddenField ID="hdnLeadID" runat="server" Value='<%# Eval("Lead_Id") %>' />
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Lead_Id") %>' OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>

                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Lead No. %>" SortExpression="Lead_no">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLeadNo" runat="server" Text='<%#Eval("Lead_no") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Lead Date%>" SortExpression="Lead_date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLeadDt" runat="server" Text='<%#GetDate(Eval("Lead_date").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Contact Name %>" SortExpression="Emp_name_Contact">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblContactName" runat="server" Text='<%#Eval("Emp_name_Contact") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Lead Status %>" SortExpression="Lead_status">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLeadStatus" runat="server" Text='<%#Eval("Lead_status") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Lead Source %>" SortExpression="Lead_source">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLeadSource" runat="server" Text='<%#Eval("Lead_source") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Opportunity Amt. %>" SortExpression="Opportunity_amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOpportunityAmt" runat="server" Text='<%#Eval("Opportunity_amount") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Generated By %>" SortExpression="Emp_name_GeneratedBy">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGeneratedBy" runat="server" Text='<%#Eval("Emp_name_GeneratedBy") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Campaign Name %>" SortExpression="Campaign_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCampaignName" runat="server" Text='<%#Eval("Campaign_Name") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
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
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblLeadNo" runat="server" Text="<%$ Resources:Attendance,Lead No. %>" />
                                                        <asp:TextBox ID="txtLeadNo" runat="server" Class="form-control" OnTextChanged="txtLeadNo_TextChanged" AutoPostBack="true" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblLeadDate" runat="server" Text="<%$ Resources:Attendance,Lead Date %>" />
                                                        <asp:TextBox ID="txtLeadDate" runat="server" Class="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender" runat="server" TargetControlID="txtLeadDate" Format="dd-MMM-yyyy" />
                                                        <br />
                                                    </div>

                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblTitle" runat="server" Text="Title" /><a style="color: Red">*</a>
                                                        <asp:TextBox ID="txtTitle" runat="server" Class="form-control" />
                                                        <asp:RequiredFieldValidator runat="server" ID="reqName" ControlToValidate="txtTitle" ErrorMessage="Please enter Title!" ValidationGroup="Save" Display="Dynamic" />
                                                        <br>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblCampaign1" runat="server" Text="<%$ Resources:Attendance,Campaign %>" />
                                                        <asp:TextBox ID="txtCampaign1" runat="server" Class="form-control" BackColor="#eeeeee"
                                                            OnTextChanged="txtCampaign1_TextChanged" AutoPostBack="true" />
                                                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidatorStreet" runat="server"
                                                            ErrorMessage="Please Enter Proper Campaign Name" ValidationExpression="^[a-zA-Z_()-.&,0-9\s]+/+[0-9]{0,5}$"
                                                            ControlToValidate="txtCampaign1"></asp:RegularExpressionValidator>--%>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="0" ServiceMethod="GetCompletionListCampaign" ServicePath=""
                                                            TargetControlID="txtCampaign1" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>


                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblCustomerName" runat="server" Text="<%$ Resources:Attendance,Customer Name %>" />
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtCustomerName" runat="server" Class="form-control" BackColor="#eeeeee"
                                                                OnTextChanged="txtCustomerName_TextChanged" AutoPostBack="true" />
                                                            <cc1:AutoCompleteExtender ID="txtCustomerName_AutoCompleteExtender" runat="server"
                                                                DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                                MinimumPrefixLength="0" ServiceMethod="GetCompletionListCustomer" ServicePath=""
                                                                TargetControlID="txtCustomerName" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <div class="input-group-btn">
                                                                <asp:Button ID="btnAddCustomer" runat="server" class="btn btn-primary" OnClick="btnAddCustomer_OnClick"
                                                                    Text="<%$ Resources:Attendance,Add %>" CausesValidation="False" Visible="false" />

                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Contact Name %>" /><a style="color: Red">*</a>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtContactList" runat="server" Class="form-control" BackColor="#eeeeee"
                                                                OnTextChanged="txtContactList_TextChanged" AutoPostBack="true" />
                                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtContactList" ErrorMessage="Please enter Contact Name!" ValidationGroup="Save" Display="Dynamic" />

                                                            <cc1:AutoCompleteExtender ID="txtContactList_AutoCompleteExtender" runat="server"
                                                                DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                                MinimumPrefixLength="0" ServiceMethod="GetContactListCustomer" ServicePath=""
                                                                TargetControlID="txtContactList" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>

                                                            <div class="input-group-btn">
                                                                <asp:LinkButton ID="lnkAddNewContact" runat="server" ToolTip="Add New Contact" OnClick="lnkAddNewContact_Click" AlternateText="<%$ Resources:Attendance,Add %>" CausesValidation="False"><i class="fa fa-plus-square" style="font-size:35px;padding:5px;margin-top: -5px;"></i></asp:LinkButton>
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblLeadStatus" runat="server" Text="<%$ Resources:Attendance,Lead Status %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlLeadStatus" runat="server" Class="form-control" OnSelectedIndexChanged="ddlLeadStatus_SelectedIndexChanged" AutoPostBack="true">
                                                            <asp:ListItem Text="Select"></asp:ListItem>
                                                            <asp:ListItem Text="New"></asp:ListItem>
                                                            <asp:ListItem Text="Assigned"></asp:ListItem>
                                                            <asp:ListItem Text="In Process"></asp:ListItem>
                                                            <asp:ListItem Text="Converted"></asp:ListItem>
                                                            <asp:ListItem Text="Recycled"></asp:ListItem>
                                                            <asp:ListItem Text="Dead"></asp:ListItem>

                                                        </asp:DropDownList>

                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblLeadSource" runat="server" Text="<%$ Resources:Attendance, Lead Source %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlLeadSource" runat="server" Class="form-control" Enabled="false" OnSelectedIndexChanged="ddlLeadSource_SelectedIndexChanged" AutoPostBack="true">
                                                            <asp:ListItem Text="Select"></asp:ListItem>
                                                            <asp:ListItem Text="Call"></asp:ListItem>
                                                            <asp:ListItem Text="Existing Customer"></asp:ListItem>
                                                            <asp:ListItem Text="Self Generated"></asp:ListItem>
                                                            <asp:ListItem Text="Employee"></asp:ListItem>
                                                            <asp:ListItem Text="Mail"></asp:ListItem>
                                                            <asp:ListItem Text="Conference"></asp:ListItem>
                                                            <asp:ListItem Text="Trade Show"></asp:ListItem>
                                                            <asp:ListItem Text="Website"></asp:ListItem>
                                                            <asp:ListItem Text="Compaign"></asp:ListItem>
                                                            <asp:ListItem Text="Others"></asp:ListItem>

                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblLeadStatusDesc" runat="server" Text="<%$ Resources:Attendance,Lead Status Description %>"></asp:Label>
                                                        <asp:TextBox ID="txtLeadStatusDesc" runat="server" Enabled="false" Style="resize: vertical; min-height: 55px; max-height: 150px; height: 50px;" Class="form-control" TextMode="MultiLine"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblLeadSourceDesc" runat="server" Text="<%$ Resources:Attendance,Lead Source Description %>"></asp:Label>
                                                        <asp:TextBox ID="txtLeadSourceDesc" runat="server" Enabled="false" Style="resize: vertical; min-height: 55px; max-height: 150px; height: 50px;" Class="form-control" TextMode="MultiLine"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12"></div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="Currency" runat="server" Text="<%$ Resources:Attendance,Currency %>" />
                                                        <asp:DropDownList ID="ddlCurrency" runat="server" Class="form-control" />

                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblOppAmt1" runat="server" Text="<%$ Resources:Attendance,Opportunity Amt. %>" /><a style="color: Red">*</a>
                                                        <asp:TextBox ID="txtOppAmt1" runat="server" Class="form-control" MaxLength="9" />
                                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtOppAmt1" ErrorMessage="Please enter Amount!" ValidationGroup="Save" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                            TargetControlID="txtOppAmt1" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblReferredBy1" runat="server" Text="<%$ Resources:Attendance,Referred By %>" />
                                                        <asp:TextBox ID="txtReferredBy1" runat="server" Class="form-control" />

                                                        <br />
                                                    </div>


                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblGeneratedBy" runat="server" Text="<%$ Resources:Attendance,Generated By %>" />
                                                        <asp:TextBox ID="txtGeneratedBy" runat="server" Class="form-control" BackColor="#eeeeee"
                                                            OnTextChanged="txtGeneratedBy_TextChanged" AutoPostBack="true" />
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                                            ErrorMessage="GeneratedBy Was Not In Proper Format" ValidationExpression="^[a-zA-Z_()-.&,0-9\s]+/+[0-9]{0,5}$"
                                                            ControlToValidate="txtGeneratedBy"></asp:RegularExpressionValidator>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="0" ServiceMethod="GetCompletionListGeneratedBy" ServicePath=""
                                                            TargetControlID="txtGeneratedBy" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblAssignedTo" runat="server" Text="<%$ Resources:Attendance,Assigned To %>" />
                                                        <asp:TextBox ID="txtAssignedTo" runat="server" Class="form-control" BackColor="#eeeeee"
                                                            OnTextChanged="txtAssignedTo_TextChanged" AutoPostBack="true" />

                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                                            ErrorMessage="AssignedTo Was Not In Proper Format" ValidationExpression="^[a-zA-Z_()-.&,0-9\s]+/+[0-9]{0,5}$"
                                                            ControlToValidate="txtAssignedTo"></asp:RegularExpressionValidator>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="0" ServiceMethod="GetCompletionListGeneratedBy" ServicePath=""
                                                            TargetControlID="txtAssignedTo" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblRemark" runat="server" Text="<%$ Resources:Attendance,Remark %>" />
                                                        <asp:TextBox ID="txtRemark" Style="width: 100%; min-width: 100%; max-width: 100%; height: 70px; min-height: 70px; max-height: 200px; overflow: auto;" runat="server" Class="form-control" TextMode="MultiLine"
                                                            Font-Names="Arial" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Panel ID="PnlOperationButton" runat="server">
                                                            <asp:Button ID="btnSaveNOpportunity" runat="server" Text="<%$ Resources:Attendance,Save & Create Opportunity %>" OnClick="btnSaveNOpportunity_Click"
                                                                class="btn btn-primary" Visible="false" />
                                                            <asp:Button ID="btnLeadSave" runat="server" Text="<%$ Resources:Attendance,Save %>" OnClick="btnLeadSave_Click" ValidationGroup="Save" CausesValidation="false"
                                                                class="btn btn-success" Visible="false" />
                                                            <asp:Button ID="BtnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                                class="btn btn-primary" CausesValidation="False" OnClick="BtnReset_Click" />
                                                            <asp:Button ID="btnSLeadCancel" runat="server" class="btn btn-danger" Text="<%$ Resources:Attendance,Cancel %>"
                                                                CausesValidation="False" OnClick="btnSLeadCancel_Click" />


                                                            <asp:Button ID="Btn_Address_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#task" Text="Assign Task" />

                                                        </asp:Panel>
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
                                                    <asp:Label ID="Label2" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				<asp:Label ID="lblTotalRecordsBin" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldNameBin" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFieldNameBin_SelectedIndexChanged">
                                                        <asp:ListItem Text="Contact Name" Value="Emp_name_Contact"></asp:ListItem>
                                                        <asp:ListItem Text="Lead source" Value="Lead_source"></asp:ListItem>
                                                        <asp:ListItem Text="Lead Date" Value="Lead_date"></asp:ListItem>
                                                        <asp:ListItem Text="Generated By" Value="Emp_name_GeneratedBy"></asp:ListItem>
                                                        <asp:ListItem Text="Campaign Name" Value="Campaign_Name"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlOptionBin" runat="server" class="form-control">
                                                        <asp:ListItem Text="--Select--"></asp:ListItem>
                                                        <asp:ListItem Text="Equal"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="Contains"></asp:ListItem>
                                                        <asp:ListItem Text="Like"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-4">
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbindBin">
                                                        <asp:TextBox ID="txtValueBin" runat="server" Class="form-control" placeholder="Search From Content"></asp:TextBox>
                                                        <asp:TextBox ID="txtValueDateBin" runat="server" Class="form-control" placeholder="Search From Date"
                                                            Visible="false"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" Format="dd-MMM-yyyy" ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtValueDateBin" OnClientDateSelectionChanged="Date">
                                                        </cc1:CalendarExtender>

                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="btnbindBin" runat="server" CausesValidation="False" OnClick="btnbindBin_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefreshBin" runat="server" CausesValidation="False" OnClick="btnRefreshBin_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imgBtnRestore" CausesValidation="False" OnClick="imgBtnRestore_Click" runat="server" ToolTip="<%$ Resources:Attendance, Active %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="ImgbtnSelectAll" Visible="false" runat="server" OnClick="ImgbtnSelectAll_Click" ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>
                                                    <asp:HiddenField ID="hdnCheck" runat="server" Value="true" />
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
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvSalesLeadBin" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvSalesLeadBin_PageIndexChanging"
                                                        OnSorting="GvSalesLeadBin_OnSorting" AllowSorting="true">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChanged"
                                                                        AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelect_CheckedChanged" />
                                                                    <asp:HiddenField ID="hdnLeadId" runat="server" Value='<%#Eval("Lead_Id") %>' Visible="false" />
                                                                    <asp:Label ID="lblLead_Id" runat="server" Visible="false" Text='<%#Eval("Lead_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="2%" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Lead No. %>" SortExpression="Lead_no">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLeadNo" runat="server" Text='<%#Eval("Lead_no") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Lead Date %>" SortExpression="Lead_date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLeadDt" runat="server" Text='<%#GetDate(Eval("Lead_date").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Contact Name%>" SortExpression="Emp_name_Contact">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblContactName" runat="server" Text='<%#Eval("Emp_name_Contact") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Lead Source%>" SortExpression="Lead_source">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLeadSource" runat="server" Text='<%#Eval("Lead_source") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Opportunity Amt.%>" SortExpression="Opportunity_amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOpportunityAmt" runat="server" Text='<%#Eval("Opportunity_amount") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Generated By%>" SortExpression="Emp_name_GeneratedBy">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGeneratedBy" runat="server" Text='<%#Eval("Emp_name_GeneratedBy") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Campaign Name%>" SortExpression="Campaign_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCampaignName" runat="server" Text='<%#Eval("Campaign_Name") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
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



    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_List">
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



    <asp:Panel ID="pnlMenuList" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlMenuNew" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlMenuBin" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="pnlMenuRequest" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PnlList" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PnlNewEdit" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PnlBin" runat="server" Visible="false"></asp:Panel>
    <asp:Panel ID="PnlNewContant" runat="server" Visible="false"></asp:Panel>


    <div class="modal fade" id="modelContactDetail" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">

                <div class="modal-body">
                    <AT2:ViewContact ID="UcContactList" runat="server" />
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
        function LI_Customer_Active() {
            $("#Li_Customer_Call").removeClass("active");
            $("#Customer_Call").removeClass("active");

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
        function Modal_CustomerInfo_Open() {
            $('#modelContactDetail').modal('show');
        }

    </script>
    <script src="../Script/ReportSystem.js"></script>
    <script src="../Script/customer.js"></script>
</asp:Content>
