<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="PartnerManagement.aspx.cs" Inherits="CRM_PartnerManagement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/FileUpload.ascx" TagPrefix="AT1" TagName="FileUpload1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-handshake"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="Partner Management"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="CRM"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="Partner Management"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Modal_FileUpload" Style="display: none;" runat="server" data-toggle="modal" data-target="#Fileupload123" Text="FileUpload" />
            <asp:HiddenField runat="server" ID="hdnCanView" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
            <asp:HiddenField runat="server" ID="hdnCanUpload" />
             <asp:HiddenField runat="server" ID="hdnCanPrint" />
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
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label28" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
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
                                        <div id="Div3" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					 <asp:Label ID="lblTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-md-3">
                                                    <asp:Label ID="Label13" runat="server" Text="Group Name" ForeColor="Black" />
                                                    <asp:DropDownList ID="AS_ddlGroup" runat="server" Class="form-control"></asp:DropDownList>
                                                    <br />
                                                </div>

                                                <div class="col-md-3">
                                                    <asp:Label ID="Label3" runat="server" Text="Agreement No" ForeColor="Black" />
                                                    <asp:TextBox runat="server" ID="AS_txtAgreementNo" Class="form-control"></asp:TextBox>
                                                    <br />
                                                </div>


                                                <div class="col-md-3">
                                                    <asp:Label ID="Label4" runat="server" Text="Customer" ForeColor="Black" />
                                                    <asp:TextBox ID="AS_txtCustomer" runat="server" Class="form-control" />
                                                    <br />
                                                </div>

                                                <div class="col-md-3">
                                                    <asp:Label ID="Label7" runat="server" Text="Country" ForeColor="Black" />
                                                    <asp:TextBox runat="server" ID="AS_txtCountry" Class="form-control"></asp:TextBox>
                                                    <br />
                                                </div>


                                                <div class="col-md-3">
                                                    <asp:Label ID="Label8" runat="server" Text="State" ForeColor="Black" />
                                                    <asp:TextBox ID="AS_TxtState" runat="server" Class="form-control" />
                                                    <br />
                                                </div>


                                                <div class="col-md-3">
                                                    <asp:Label ID="Label9" runat="server" Text="City" ForeColor="Black" />
                                                    <asp:TextBox runat="server" ID="As_TxtCity" Class="form-control"></asp:TextBox>
                                                    <br />
                                                </div>


                                                <div class="col-md-3">
                                                    <asp:Label ID="Label10" runat="server" Text="Category" ForeColor="Black" />
                                                    <asp:TextBox ID="AS_TxtCategory" runat="server" Class="form-control" />
                                                    <br />
                                                </div>


                                                <div class="col-md-3">
                                                    <asp:Label ID="Label11" runat="server" Text="Product Code" ForeColor="Black" />
                                                    <asp:TextBox runat="server" ID="AS_TxtProductCode" Class="form-control"></asp:TextBox>
                                                    <br />
                                                </div>


                                                <div class="col-md-3">
                                                    <asp:Label ID="Label12" runat="server" Text="Product Name" ForeColor="Black" />
                                                    <asp:TextBox ID="AS_TxtProductName" runat="server" Class="form-control" />
                                                    <br />
                                                </div>

                                                <div class="col-md-2">
                                                    <br />
                                                    <asp:Button runat="server" ID="btn_advanceSearch" class="btn btn-primary" Text="Search" OnClick="btn_advanceSearch_Click" />
                                                    <br />
                                                </div>
                                                <div class="col-md-2">
                                                    <br />
                                                    <asp:Button runat="server" ID="btnResetAdvanceSearch" class="btn btn-primary" Text="Reset Advance Search" OnClick="btnResetAdvanceSearch_Click" />
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <br />
                                                </div>
                                                <asp:HiddenField ID="hdnTransType" runat="server" />
                                                <asp:HiddenField ID="hdnTransTypeValue" runat="server" />

                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control" OnTextChanged="ddlFieldName_TextChanged" AutoPostBack="true">
                                                        <asp:ListItem Text="Agreement No" Value="Agreement_No"></asp:ListItem>
                                                        <asp:ListItem Text="Agreement Date" Value="Agreement_Date"></asp:ListItem>
                                                        <asp:ListItem Text="Group" Value="Group_Name"></asp:ListItem>
                                                        <asp:ListItem Text="Customer" Value="customerName"></asp:ListItem>
                                                        <asp:ListItem Text="Contact Person" Value="contactName"></asp:ListItem>
                                                        <asp:ListItem Text="Handled By" Value="handledByName"></asp:ListItem>
                                                        <asp:ListItem Text="From Date" Value="From_Date"></asp:ListItem>
                                                        <asp:ListItem Text="To Date" Value="To_Date"></asp:ListItem>
                                                        <asp:ListItem Text="Country" Value="Country_Name"></asp:ListItem>
                                                        <asp:ListItem Text="State" Value="State_Name"></asp:ListItem>
                                                        <asp:ListItem Text="City" Value="City_Name"></asp:ListItem>
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
                                                <div class="col-lg-2">
                                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnListSearch">
                                                        <asp:TextBox ID="txtValue" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <asp:TextBox ID="txtValueDate" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueDate" runat="server" Format="dd-MMM-yyyy" TargetControlID="txtValueDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:ImageButton ID="btnListSearch" runat="server" CausesValidation="False"
                                                        ImageUrl="~/Images/search.png" OnClick="btnListSearch_Click" Style="margin-left: -5px;" ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>

                                                    <asp:ImageButton ID="btnRefresh" runat="server" CausesValidation="False" Style="width: 33px;"
                                                        ImageUrl="~/Images/refresh.png" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>


                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div class="box box-warning box-solid" <%= GvListData.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:HiddenField ID="hdntransID" runat="server"></asp:HiddenField>
                                                    <asp:UpdatePanel ID="asd" runat="server">
                                                        <ContentTemplate>


                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvListData" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvListData_PageIndexChanging" OnSorting="GvListData_Sorting"
                                                                AllowSorting="true">
                                                                <Columns>

                                                                    <asp:TemplateField HeaderText="Action">
                                                                        <ItemTemplate>
                                                                            <div class="dropdown" style="position: absolute;">
                                                                                <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                                    <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                                </button>
                                                                                <ul class="dropdown-menu">

                                                                                    <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                        <asp:LinkButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("Trans_id") %>' OnCommand="lnkViewDetail_Command" CausesValidation="False"><i class="fa fa-eye"></i>View</asp:LinkButton>
                                                                                    </li>

                                                                                    <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                        <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_id") %>' CommandName='<%# Eval("isExtendedOrNot") %>' CssClass='<%# Eval("ToolTip").ToString()=="Extend"?"fas fa-plus-square":"fa fa-pencil" %>' CausesValidation="False" OnCommand="lnkViewDetail_Command">&nbsp;&nbsp; <%# Eval("ToolTip").ToString() %> </asp:LinkButton>
                                                                                    </li>

                                                                                    <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                        <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_id") %>' OnCommand="IbtnDelete_Command" ToolTip="<%$ Resources:Attendance,Delete %>"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                        <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                                    </li>

                                                                                    <li <%= hdnCanUpload.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                        <asp:LinkButton ID="btnFileUpload" runat="server" CommandArgument='<%# Eval("Trans_id") %>' CommandName='<%# Eval("Agreement_No") %>' CausesValidation="False" OnCommand="btnFileUpload_Command"><i class="fa fa-upload"></i>File Upload</asp:LinkButton>
                                                                                    </li>

                                                                                      <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnPrint" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Agreement_No") %>' OnCommand="IbtnPrint_Command"><i class="fa fa-print"></i>Print</asp:LinkButton>
                                                                            </li>

                                                                                </ul>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>



                                                                    <asp:TemplateField HeaderText="Agreement No" SortExpression="Agreement_No">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%# Eval("Agreement_No") %>'></asp:Label>
                                                                        </ItemTemplate>

                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Agreement Date" SortExpression="Agreement_Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%# GetDate(Eval("Agreement_Date").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Group" SortExpression="Group_Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%# Eval("Group_Name") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>


                                                                    <asp:TemplateField HeaderText="Customer" SortExpression="customerName">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%# Eval("customerName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Contact Person" SortExpression="contactName">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%# Eval("contactName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Country" SortExpression="Country_Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%# Eval("Country_Name") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="State" SortExpression="State_Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%# Eval("State_Name") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="City" SortExpression="City_Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%# Eval("City_Name") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Handled By" SortExpression="handledByName">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%# Eval("handledByName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Product Code" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%# Eval("Product_Code") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Product Category" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%# Eval("Product_Category") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Product Name" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%# Eval("EProductName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                </Columns>


                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                    <asp:HiddenField ID="HiddenField1" runat="server" />
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
                                                        <asp:Label ID="lblAgreementNo" runat="server" Text="Agreement No" /><a style="color: Red">*</a>
                                                        <asp:TextBox ID="txtAgreementNo" runat="server" Class="form-control" />
                                                        <br />
                                                    </div>

                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblAgreementDate" runat="server" Text="Agreement Date" /><a style="color: Red">*</a>
                                                        <asp:TextBox ID="txtAgreementDate" runat="server" Class="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" TargetControlID="txtAgreementDate" Format="dd-MMM-yyyy" />
                                                        <br />
                                                    </div>

                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblGroup" runat="server" Text="Group" /><a style="color: Red">*</a>
                                                        <asp:DropDownList ID="ddlGroupSearch" runat="server" Class="form-control"></asp:DropDownList>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-8">
                                                        <asp:Label ID="lblCustomer" runat="server" Text="Customer Name" /><a style="color: Red">*</a>
                                                        <asp:TextBox ID="txtCustomer" runat="server" Class="form-control" onchange="CustomerName_textchange()" OnTextChanged="txtCustomer_TextChanged" AutoPostBack="true" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender8" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="0" ServiceMethod="GetCustomerList" ServicePath=""
                                                            TargetControlID="txtCustomer" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblContactName" runat="server" Text="Contact Name" /><a style="color: Red">*</a>
                                                        <asp:TextBox ID="txtContactName" runat="server" Class="form-control" onchange="ContactName_textchange()" />
                                                        <cc1:AutoCompleteExtender ID="txtContactList_AutoCompleteExtender" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="0" ServiceMethod="GetContactListCustomer" ServicePath=""
                                                            TargetControlID="txtContactName" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>


                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblEmailAddress" runat="server" Text="Email Address" /><a style="color: Red">*</a>
                                                        <asp:TextBox ID="txtEmailAddress" runat="server" Class="form-control" onchange="email_textChange()" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="0" ServiceMethod="GetCompletionListEmailMaster" ServicePath=""
                                                            TargetControlID="txtEmailAddress" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <asp:RegularExpressionValidator ID="validateEmail" runat="server" ErrorMessage="Invalid email."
                                                            ControlToValidate="txtEmailAddress" ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$" />
                                                        <br />
                                                    </div>


                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblMobileNo" runat="server" Text="Mobile Number" /><a style="color: Red">*</a>
                                                        <asp:TextBox ID="txtMobileNo" runat="server" Class="form-control" />

                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="0" ServiceMethod="GetCompletionListMobileNo" ServicePath=""
                                                            TargetControlID="txtMobileNo" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>

                                                        <br />
                                                    </div>

                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblAddress" runat="server" Text="Address" /><a style="color: Red">*</a>
                                                        <asp:DropDownList ID="ddlAddress" runat="server" Class="form-control" />
                                                        <br />
                                                    </div>

                                                    <asp:HiddenField ID="hdncust_Id" runat="server" />
                                                    <asp:HiddenField ID="hdnContact_Id" runat="server" />
                                                    <asp:HiddenField ID="hdnstateId" runat="server" />
                                                    <asp:HiddenField ID="hdncityId" runat="server" />


                                                    <div class="col-md-4">
                                                        <asp:Label ID="Label17" runat="server" Text="<%$ Resources:Attendance,Country %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlCountry" onchange="DDLCountry_Change()" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblState" runat="server" Text="<%$ Resources:Attendance,State %>"></asp:Label>
                                                        <asp:TextBox ID="txtState" runat="server" CssClass="form-control" onchange="txtState_TextChanged()"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListStateName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtState"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>

                                                        <br />
                                                    </div>
                                                    <div class="col-md-4">

                                                        <asp:Label ID="lblCity" runat="server" Text="<%$ Resources:Attendance,City %>"></asp:Label>
                                                        <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" onchange="txtCity_TextChanged()"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListCityName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtCity"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>


                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblfromDate" runat="server" Text="Agreement Start Date" /><a style="color: Red">*</a>
                                                        <asp:TextBox ID="txtfromDate" runat="server" Class="form-control" onchange="checkDateDiff_StartDate_ExpDate()" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender2" runat="server" TargetControlID="txtfromDate" Format="dd-MMM-yyyy" />
                                                        <br />
                                                    </div>

                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblToDate" runat="server" Text="Agreement Expiry Date" /><a style="color: Red">*</a>
                                                        <asp:TextBox ID="txtToDate" runat="server" Class="form-control" onchange="checkDateDiff_StartDate_ExpDate();checkDateDiff_AgreementDate_ExpDate()" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender3" runat="server" TargetControlID="txtToDate" Format="dd-MMM-yyyy" />
                                                        <br />
                                                    </div>


                                                    <div class="col-md-4">
                                                        <asp:Label ID="Label2" runat="server" Text="Handled By" /><a style="color: Red">*</a>
                                                        <asp:TextBox ID="txtHandledBy" runat="server" Class="form-control" BackColor="#eeeeee" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                            MinimumPrefixLength="0" ServiceMethod="GetCompletionListEmployeeName" ServicePath=""
                                                            TargetControlID="txtHandledBy" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblSecurityAmt" runat="server" Text="Security Amount" />
                                                        <asp:TextBox ID="txtSecurityAmt" runat="server" Class="form-control" onchange="validateAmt(this);" />
                                                        <br />
                                                    </div>


                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblTurnover" runat="server" Text="Company Turnover" />
                                                        <asp:TextBox ID="txtTurnover" runat="server" Class="form-control" onchange="validateAmt(this);" />
                                                        <br />
                                                    </div>


                                                    <div class="col-md-4">
                                                        <br />
                                                        <asp:CheckBox runat="server" ID="chkReminder" Text="Set Reminder" />
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblNotes" runat="server" Text="Notes" />
                                                        <asp:TextBox ID="txtNotes" Style="resize: vertical; height: 50px; min-height: 50px; max-height: 150px;" runat="server" Class="form-control" TextMode="MultiLine" />
                                                        <br />
                                                    </div>


                                                    <div class="col-md-12" id="product_div" runat="server" style="display: block">
                                                        <div class="box box-primary">
                                                            <div id="Div_Box_Add" runat="server" class="box box-info collapsed-box">
                                                                <div class="box-header with-border">
                                                                    <h4>
                                                                        <asp:Literal runat="server" Text="<%$ Resources:Attendance,Product Details%>" />:</h4>
                                                                    <div class="box-tools pull-right">
                                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                            <i id="Btn_Add_Div" class="fa fa-plus" runat="server"></i>
                                                                        </button>
                                                                    </div>
                                                                </div>
                                                                <div class="box-body">
                                                                    <div class="form-group">

                                                                        <div class="col-md-3">
                                                                            <asp:Label ID="lblCategory" runat="server" Text="Product Category" />
                                                                            <asp:DropDownList runat="server" ID="ddlProductCategory" Class="form-control" onchange="category_Change()"></asp:DropDownList>
                                                                            <br />
                                                                        </div>


                                                                        <div class="col-md-3">
                                                                            <asp:Label ID="lblProductCode" runat="server" Text="Product Code" />
                                                                            <asp:TextBox ID="txtProductCode" runat="server" Class="form-control" onchange="productcode_change()" />
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" CompletionInterval="100"
                                                                                DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductCode"
                                                                                ServicePath="" TargetControlID="txtProductCode" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <br />
                                                                        </div>

                                                                        <div class="col-md-6">
                                                                            <asp:Label ID="lblProductName" runat="server" Text="Product Name" />
                                                                            <asp:TextBox ID="txtProductName" runat="server" Class="form-control" />
                                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetCompletionListProductName" ServicePath="" CompletionInterval="100"
                                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtProductName"
                                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                            </cc1:AutoCompleteExtender>
                                                                            <br />
                                                                        </div>

                                                                        <div class="col-md-3">
                                                                            <asp:Label ID="lblTargetQuantity" runat="server" Text="Target Quantity" />
                                                                            <asp:TextBox ID="txtTargetQuantity" runat="server" Class="form-control" MaxLength="4" />
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                                TargetControlID="txtTargetQuantity" ValidChars="1,2,3,4,5,6,7,8,9,0">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-md-3">
                                                                            <asp:Label ID="lblTargetAmount" runat="server" Text="Target Amount" />
                                                                            <asp:TextBox ID="txtTargetAmount" onchange="validateTargetAmt()" runat="server" Class="form-control" />
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                                TargetControlID="txtTargetAmount" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <br />
                                                                        </div>

                                                                        <div class="col-md-2">
                                                                            <br />
                                                                            <asp:Button runat="server" ID="btn_AddProduct" class="btn btn-primary" Text="Add Product" OnClick="btn_AddProduct_Click" />
                                                                            <br />
                                                                        </div>


                                                                        <div class="col-md-12" class="flow">

                                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvProductData" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True"
                                                                                AllowSorting="True">
                                                                                <Columns>
                                                                                    <asp:TemplateField>
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton ID="gvimgBtnDelete" runat="server" CommandArgument='<%#Eval("Serial_No") %>' CommandName='<%#Eval("Trans_Id") %>' OnCommand="gvlblDelete_Command"><i class="fa fa-trash"></i></asp:LinkButton>
                                                                                            <asp:HiddenField ID="gvhdnSerial_No" runat="server" Value='<%# Eval("Serial_No") %>' />
                                                                                            <asp:HiddenField ID="gvhdnTrans_Id" runat="server" Value='<%# Eval("Trans_Id") %>' />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Left" Width="2%" />
                                                                                    </asp:TemplateField>

                                                                                    <asp:TemplateField HeaderText="Product Category">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="gvlblProductCategory" runat="server" Text='<%# Eval("Product_Category").ToString() != "Select" ? Eval("Product_Category").ToString() : "" %>' />
                                                                                            <asp:HiddenField ID="gvhdnCategoryId" runat="server" Value='<%# Eval("Product_Category_Id") %>' />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Left" Width="9%" />
                                                                                    </asp:TemplateField>


                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product code %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="gvlblProductCode" runat="server" Text='<%# Eval("Product_Code") %>' />
                                                                                            <asp:HiddenField ID="gvhdnProductId" runat="server" Value='<%# Eval("Product_Id") %>' />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Left" Width="9%" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product name %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="gvlblProductName" runat="server" Text='<%# Eval("EProductName") %>' />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Left" Width="9%" />
                                                                                    </asp:TemplateField>


                                                                                    <asp:TemplateField HeaderText="Target Quantity">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="gvlbltargetQuantity" runat="server" Text='<%#Eval("Target_Quantity") %>' />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Left" Width="9%" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Target Amount">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="gvlblQuantity" runat="server" Text='<%#Eval("Target_Amount") %>' />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Left" Width="9%" />
                                                                                    </asp:TemplateField>


                                                                                </Columns>
                                                                            </asp:GridView>


                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>


                                                        </div>
                                                    </div>

                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblTerms_Condition" runat="server" Text="Terms & Conditions" /><a style="color: Red">*</a>
                                                        <asp:TextBox ID="txtTerms_Conditions" Style="resize: vertical; height: 50px; min-height: 50px; max-height: 150px;" runat="server" Class="form-control" TextMode="MultiLine" />
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Panel ID="PnlOperationButton" runat="server">

                                                            <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                                                class="btn btn-success" OnClick="btnSave_Click" UseSubmitBehavior="false" OnClientClick="this.disabled='true'; this.value='please wait...'; " />
                                                            <asp:Button ID="BtnReset" runat="server" OnClick="BtnReset_Click" Text="<%$ Resources:Attendance,Reset %>"
                                                                class="btn btn-primary" />
                                                            <asp:HiddenField ID="hdnNo" runat="server" Value="0" />


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
                    <div class="tab-pane" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server">
                            <ContentTemplate>



                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div1" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label14" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecordsBin" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldNameBin" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlFieldNameBin_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Invoice No %>" Value="Invoice_No" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Invoice Date %>" Value="Invoice_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Order No. %>" Value="OrderList"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Sales Person %>" Value="EmployeeName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Customer Name %>" Value="CustomerName"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="CreatedUser"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Modified By %>" Value="ModifiedUser"></asp:ListItem>
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
                                                <div class="col-lg-5">
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnsearchBin">
                                                        <asp:TextBox ID="txtValueBin" runat="server" CssClass="form-control" placeholder="Search from Content"   	></asp:TextBox>
                                                        <asp:TextBox ID="txtCustValueBin" runat="server" CssClass="form-control" Visible="false" placeholder="Search from Content"></asp:TextBox>
                                                        <asp:TextBox ID="txtValueDateBin" runat="server" CssClass="form-control" Visible="false" placeholder="Search from Date"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueBinDate" runat="server" TargetControlID="txtValueDateBin" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnsearchBin" runat="server" CausesValidation="False" OnClick="btnsearchBin_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefreshBin" runat="server" CausesValidation="False" OnClick="btnRefreshBin_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imgBtnRestore" CausesValidation="False" OnClick="imgBtnRestore_Click" runat="server" ToolTip="<%$ Resources:Attendance, Active %>" ><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid" <%= GvBinData.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:Label ID="lblSelectedRecords" runat="server" Visible="false"></asp:Label>
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvBinData" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True"
                                                        AllowSorting="true" OnPageIndexChanging="GvBinData_PageIndexChanging" OnSorting="GvBinData_Sorting">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChanged"
                                                                        AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelect_CheckedChanged" />
                                                                    <asp:HiddenField ID="hdntransId" runat="server" Value='<%#Eval("Trans_id") %>' Visible="false" />
                                                                    <asp:Label ID="lbltransId" runat="server" Visible="false" Text='<%#Eval("Trans_id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="2%" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Agreement No" SortExpression="Agreement_No">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" Text='<%# Eval("Agreement_No") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="3%" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Agreement Date" SortExpression="Agreement_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" Text='<%# GetDate(Eval("Agreement_Date").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="3%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Contact Person" SortExpression="contactName">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" Text='<%# Eval("contactName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="3%" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Country" SortExpression="Country_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" Text='<%# Eval("Country_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="3%" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="State" SortExpression="State_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" Text='<%# Eval("State_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="3%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="City" SortExpression="City_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" Text='<%# Eval("City_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="3%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Handled By" SortExpression="handledByName">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" Text='<%# Eval("handledByName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="3%" />
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
            </div>
        </div>
    </div>


    <div class="modal fade" id="Fileupload123" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">

                <asp:UpdatePanel ID="up_fu" runat="server">
                    <ContentTemplate>
                        <div class="modal-body">
                            <AT1:FileUpload1 runat="server" ID="FUpload1" />
                        </div>
                        <div class="modal-footer">
                            <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
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

        function LI_Edit_Active_agreement() {

            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }


        function Li_Tab_New() {
        }
        function Li_Tab_Bin() {
        }
        function Li_Tab_List() {

        }
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 100004;
        }
        function Modal_Open_FileUpload() {
            document.getElementById('<%= Btn_Modal_FileUpload.ClientID %>').click();
        }


        function DisplayMsg(str) {
            alert(str);
            return;
        }
        function DDLCountry_Change() {
            var ddlcountry = document.getElementById('<%= ddlCountry.ClientID %>');
            if (ddlcountry.value != "--Select--") {
                PageMethods.ddlCountry_IndexChanged(ddlcountry.value, onSuccessChange, ErrorOnChange);
            }
        }

        function onSuccessChange(data) {
            document.getElementById('<%= txtState.ClientID %>').value = "";
            document.getElementById('<%= txtCity.ClientID %>').value = "";
        }
        function ErrorOnChange(error) {
            alert(error.getMessage());
        }

        function txtCity_TextChanged() {
            var stateId = document.getElementById('<%= hdnstateId.ClientID %>').value;
            var CityName = document.getElementById('<%= txtCity.ClientID %>').value;

            if (document.getElementById('<%= txtState.ClientID %>').value == "") {
                DispMsg("Please Select State");
                document.getElementById('<%= txtCity.ClientID %>').value = "";
                return;
            }
            if (stateId == "0" || stateId == "") {
                DispMsg("Please Select State");
                document.getElementById('<%= txtCity.ClientID %>').value = "";
                return;
            }
            PageMethods.txtCity_TextChanged(stateId, CityName, onSuccess_CityChange, ErrorOnChange);
        }
        function onSuccess_CityChange(CityId) {
            if (CityId == "" || CityId == "0") {
                DispMsg('Select From Suggestions Only');
                document.getElementById('<%= txtCity.ClientID %>').value = "";
                return;
            }
            document.getElementById('<%= hdncityId.ClientID %>').value = CityId;
        }

        function txtState_TextChanged() {
            var stateName = document.getElementById('<%= txtState.ClientID %>').value;
            var CountryId = document.getElementById('<%= ddlCountry.ClientID %>').value;
            if (CountryId != "--Select--") {
                PageMethods.txtState_TextChanged(CountryId, stateName, onSuccess_StateChange, ErrorOnChange);
            }
            else {
                DispMsg('Please Select Country');
                document.getElementById('<%= txtState.ClientID %>').value = "";
                return;
            }

        }

        function onSuccess_StateChange(StateId) {
            if (StateId == "" || StateId == "0") {
                DispMsg('Select From Suggestions Only');
                document.getElementById('<%= txtState.ClientID %>').value = "";
            }
            document.getElementById('<%= txtCity.ClientID %>').value = "";
            document.getElementById('<%= hdnstateId.ClientID %>').value = StateId;
        }


        function email_textChange() {
            var email = document.getElementById('<%= txtEmailAddress.ClientID %>').value;

        PageMethods.Check_email(email, onSuccessEmailCheck, ErrorOnChange)

    }
    function onSuccessEmailCheck(data) {
        if (data == 0) {
            if (confirm("Do you want to save this email address ?")) {

                var email = document.getElementById('<%= txtEmailAddress.ClientID %>').value;
                var country = document.getElementById('<%= ddlCountry.ClientID %>').value;
                var contactId = document.getElementById('<%= hdnContact_Id.ClientID %>').value;
                if (email == "") {
                    return;
                }
                if (country == "" || country == "0") {
                    DisplayMsg("Cant get country");
                    return;
                }
                if (contactId == "" || contactId == "0") {
                    DisplayMsg("Cant get contact information");
                    return;
                }

                var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;

                if (reg.test(email) == false) {
                    alert('Invalid Email Address');
                    return;
                }

                PageMethods.save_email(email, country, contactId, onSuccessEmailSave, ErrorOnChange);
            }
        }

    }

    function onSuccessEmailSave(data) {
        if (data == 1) {
            DisplayMsg("Email Saved Successfully");
        }
    }
    function CustomerName_textchange() {
        var CustomerName = document.getElementById('<%= txtCustomer.ClientID %>').value;

        if (CustomerName == "") {
            return;
        }
        var start_pos = CustomerName.lastIndexOf("/") + 1;
        var last_pos = CustomerName.length;

        var id = CustomerName.substring(start_pos, last_pos);

        var Last_pos_name = CustomerName.lastIndexOf("/");
        var name = CustomerName.substring(0, Last_pos_name - 0);

        if (id != "") {
            document.getElementById('<%= hdncust_Id.ClientID %>').value = id;
        }
        PageMethods.CustomerName_textchange(name, id, onSuccessCustomerName, ErrorOnChange);
    }
    function onSuccessCustomerName(data) {
        if (data == "0") {
            document.getElementById('<%= hdncust_Id.ClientID %>').value = data;
            DisplayMsg("Select from Suggestions only");
            document.getElementById('<%= txtCustomer.ClientID %>').value = "";
            return;

        }
    }

    function ContactName_textchange() {
        var contactName = document.getElementById('<%= txtContactName.ClientID %>').value;

        if (contactName == "") {
            return;
        }
        var start_pos = contactName.lastIndexOf("/") + 1;
        var last_pos = contactName.length;

        var id = contactName.substring(start_pos, last_pos);

        var Last_pos_name = contactName.lastIndexOf("/");
        var name = contactName.substring(0, Last_pos_name - 0);

        var customerId = document.getElementById('<%= hdncust_Id.ClientID %>').value;

        if (customerId == "") {
            DisplayMsg("Please Select Customer Name");
            document.getElementById('<%= txtContactName.ClientID %>').value = "";
            return;
        }

        if (id != "") {
            document.getElementById('<%= hdnContact_Id.ClientID %>').value = id;
        }

        PageMethods.ContactName_textchange(name, id, customerId, onSuccessContactName, ErrorOnChange);
    }

    function onSuccessContactName(data) {
        if (data == "0") {
            document.getElementById('<%= hdnContact_Id.ClientID %>').value = data;
                DisplayMsg("Select from Suggestions only");
                document.getElementById('<%= txtContactName.ClientID %>').value = "";
        }
    }


    function checkDateDiff_StartDate_ExpDate() {
        var fromdt = document.getElementById('<%= txtfromDate.ClientID%>');
        if (fromdt.value == "") {
            alert("Please Enter Agreement Start Date");
            return;
        }
        var todt = document.getElementById('<%= txtToDate.ClientID%>');
        if (fromdt.value != "" && todt.value != "") {
            if ((Date.parse(fromdt.value) >= Date.parse(todt.value))) {
                alert("Agreement Expiry Date Should Be Greater Than Agreement Start Date");
                todt.value = "";
            }
        }
    }
    function checkDateDiff_AgreementDate_ExpDate() {
        var fromdt = document.getElementById('<%= txtAgreementDate.ClientID%>');
        if (fromdt.value == "") {
            alert("Please Enter Agreement Date");
            document.getElementById('<%= txtToDate.ClientID%>').value = "";
            return;
        }
        var todt = document.getElementById('<%= txtToDate.ClientID%>');
        if (fromdt.value != "" && todt.value != "") {
            if ((Date.parse(fromdt.value) >= Date.parse(todt.value))) {
                alert("Agreement Expiry Date Should Be Greater Than Agreement Date");
                todt.value = "";
            }
        }
    }

    function validateTargetAmt() {
        var amt = document.getElementById('<%= txtTargetAmount.ClientID%>');


    }
    function category_Change() {
        var categoryId = document.getElementById('<%= ddlProductCategory.ClientID%>').value;

        if (categoryId == "") {
            categoryId = "0";
        }

        PageMethods.CategoryChange(categoryId);
    }

    function productcode_change() {
        var productcode = document.getElementById('<%= txtProductCode.ClientID%>').value;
        PageMethods.productcode_change(productcode, onsuccess_productCode, ErrorOnChange);
    }
    function onsuccess_productCode(data) {
        if (data == "@NOTFOUND@") {
            document.getElementById('<%= txtProductCode.ClientID%>').value = "";
             document.getElementById('<%= txtProductName.ClientID%>').value = "";
             alert("Select from Suggestions only");
         }
         else {
             document.getElementById('<%= txtProductName.ClientID%>').value = data;
         }

     }

     function validateAmt(id) {
         if (isNaN(id.value)) {
             alert("Please Enter A Valid Number");
             id.value = "";
             id.focus();
             test();
         }
     }



    </script>

</asp:Content>

