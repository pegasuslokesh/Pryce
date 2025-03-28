<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="VehicleExpenses.aspx.cs" Inherits="Transport_VehicleExpenses" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <script type="text/javascript">
        function ShowDatatables() {
            $("#ctl00_MainContent_gvExpensesMaster").dataTable({
                "aLengthMenu": [[10, 20, 30, 40, 50, 60, 70, 80, 90, 100], [10, 20, 30, 40, 50, 60, 70, 80, 90, 100]],
                "iDisplayLength": 10
            });
            $("#ctl00_MainContent_gvExpensesMaster_info").hide();
            $("#ctl00_MainContent_gvExpensesMaster_paginate").hide();
            $("#ctl00_MainContent_gvExpensesMaster_length").hide();
            //ShowBinDataTables();
            SetPageSizeInDt();
        }
        //function ShowBinDataTables() {
        //    $("#ctl00_MainContent_gvExpensesMasterBin").dataTable();
        //}
        function SetPageSizeInDt() {
            var myval = $("#ctl00_MainContent_ddlPageSize option:selected").val();
            $('#ctl00_MainContent_gvExpensesMaster_length').find("select").val(myval).change();
        }
        function SetSelectedCss(no) {
            $('#ctl00_MainContent_PagingPanel').find('input[type="submit"]').each(function () {
                if ($(this).val() != no) {
                    $(this).removeClass('pagingbtn-sel');
                }
                else {
                    $(this).addClass('pagingbtn-sel');
                }
            });
        }
        function SetSelectedCssMileage(no) {
            $('#ctl00_MainContent_PagingPanelMileage').find('input[type="submit"]').each(function () {
                if ($(this).val() != no) {
                    $(this).removeClass('pagingbtn-sel');
                }
                else {
                    $(this).addClass('pagingbtn-sel');
                }
            });
        }
    </script>
    <style type="text/css">
        .pagingbtn {
            background: #808080;
        }

        .pagingbtn-sel {
            background: #ADD8E6;
        }

        .grid td, .grid th {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-wallet"></i>
        <asp:Label ID="lblHeader" runat="server" Text="Vehicle Expenses"></asp:Label>
        <asp:HiddenField ID="hdnPageNo" runat="server" ClientIDMode="Static" />
        <%--<small>Control panel</small>--%>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,System SetUp%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,System SetUp%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="Vehicle Expenses"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" OnClick="btnNew_Click" Text="New" />
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="btnList_Click" Text="List" />
            <asp:Button ID="Btn_Mileage" Style="display: none;" runat="server" OnClick="Btn_Mileage_Click" Text="Mileage" />
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
                    <li id="Li_Mileage"><a href="#Mileage" onclick="Li_Tab_Mileage()" data-toggle="tab">
                        <img src="../Images/Mileage.png" style="width: 25px;" alt="" /><asp:Label ID="Label3" runat="server" Text="Mileage"></asp:Label></a></li>
                    <li><a href="#Bin" onclick="Li_Tab_Bin()" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
                    <li id="Li_New"><a onclick="Li_Tab_New()" href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List" class="active"><a href="#List" onclick="Li_Tab_List()" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label15" runat="server" Text="<%$ Resources:Attendance,List %>"></asp:Label></a></li>
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
						<asp:Label ID="lblTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                                                <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlPosted" runat="server" CssClass="form-control"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlPosted_SelectedIndexChanged">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Posted %>" Value="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,UnPosted %>" Value="False" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,All %>" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Selected="True" Text="Vehicle Name" Value="VehicleName"></asp:ListItem>
                                                        <asp:ListItem Text="Driver Name" Value="Driver_Name"></asp:ListItem>
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
                                                <div class="col-lg-4">
                                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                        <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" placeholder="Search from Content"></asp:TextBox>
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
                                <div class="box box-warning box-solid" <%= gvExpensesMaster.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                Page Size :
                                                <asp:DropDownList ID="ddlPageSize" runat="server" OnSelectedIndexChanged="ddlPageSize_OnSelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                <br />
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvExpensesMaster"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="false" AllowSorting="true"
                                                        OnPageIndexChanging="gvExpensesMaster_PageIndexChanging" OnSorting="gvExpensesMaster_OnSorting" OnPreRender="gvExpensesMaster_OnPreRender">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                    ToolTip="View"
                                                                                    OnCommand="lnkViewDetail_Command" CausesValidation="False"><i class="fa fa-eye"></i>View</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                    CausesValidation="False" OnCommand="btnEdit_Command"
                                                                                    ToolTip="<%$ Resources:Attendance,Edit %>"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                    OnCommand="IbtnDelete_Command"
                                                                                    ToolTip="<%$ Resources:Attendance,Delete %>"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>

                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Trans_Id" SortExpression="Trans_Id" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTransId" runat="server" Text='<%#  Eval("Trans_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Trans Date" SortExpression="Trans_date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTransdate" runat="server" Text='<%# GetDate(Eval("Trans_date").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Vehicle Name" SortExpression="VehicleName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblVehicleName" runat="server" Text='<%# Eval("VehicleName") %>'></asp:Label>
                                                                    <asp:Label ID="lblIsPost" runat="server" Text='<%# Eval("Field4") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Driver Name" SortExpression="Driver_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDriverName" runat="server" Text='<%# Eval("Driver_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Product" SortExpression="ProductCode">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("ProductCode") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Qty" SortExpression="qty">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblqty" runat="server" Text='<%# Common.GetAmountDecimal(Eval("qty").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Rate" SortExpression="Rate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRate" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Rate").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Amount" SortExpression="Amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAmount" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Amount").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                        </Columns>


                                                        <PagerStyle CssClass="pagination-ys" />

                                                        <PagerSettings Mode="NextPreviousFirstLast" FirstPageText="First" LastPageText="Last" NextPageText="Next" PreviousPageText="Previous" />
                                                    </asp:GridView>
                                                    &nbsp;&nbsp;&nbsp;
                                       <asp:Panel ID="PagingPanel" runat="server"></asp:Panel>
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
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblCategory" runat="server" Text="Category Type"></asp:Label>
                                                            <asp:DropDownList ID="ddlCategory" runat="server" OnSelectedIndexChanged="ddlCategory_OnSelectedIndexChanged" CssClass="form-control" AutoPostBack="true">
                                                                <asp:ListItem Text="Diesel Expenses" Value="Diesel"></asp:ListItem>
                                                                <asp:ListItem Text="Inventory Expenses" Value="Inventory"></asp:ListItem>
                                                                <asp:ListItem Text="Miscellaneous Expenses" Value="Miscellaneous"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label1" runat="server" Text="Trans Date"></asp:Label>
                                                            <asp:TextBox ID="txtTransdate" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender" runat="server" TargetControlID="txtTransdate" />
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Lbl_name" runat="server" Text="<%$ Resources:Attendance,Vehicle Name%>"></asp:Label>
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
                                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Driver Name%>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEmpName" ErrorMessage="Enter Driver Name"></asp:RequiredFieldValidator>
                                                            <asp:TextBox ID="txtEmpName" runat="server" AutoPostBack="true" CssClass="form-control" BackColor="#eeeeee"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="txtEmpName_AutoCompleteExtender" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetCompletionList" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtEmpName" UseContextKey="True"
                                                                CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-12">
                                                            <asp:Label ID="lblProductName" runat="server" Text="<%$ Resources:Attendance,Product Code%>"></asp:Label>
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator152" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtProductName" ErrorMessage="Enter Product Code"></asp:RequiredFieldValidator>
                                                            <asp:TextBox ID="txtProductName" runat="server" AutoPostBack="true" BackColor="#eeeeee" CssClass="form-control" OnTextChanged="txtProductName_OnTextChanged"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="txtProductCode_AutoCompleteExtender" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetCompletionListProductName" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtProductName"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <asp:HiddenField ID="hdnProductId" runat="server" Value="0" />
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblUnit" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Unit %>" />
                                                            <a style="color: Red" id="Unit" runat="server">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server"
                                                                ID="RFVUnit" ValidationGroup="Save" Display="Dynamic" SetFocusOnError="true"
                                                                ControlToValidate="ddlUnit" InitialValue="--Select--" ErrorMessage="<%$ Resources:Attendance,Select Unit Name%>" />
                                                            <asp:DropDownList ID="ddlUnit" runat="server" CssClass="form-control" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblQty" runat="server" Text="Qty"></asp:Label>
                                                            <a style="color: Red" id="Qty" runat="server">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RFVQty" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtQty" ErrorMessage="Enter Quantity"></asp:RequiredFieldValidator>
                                                            <asp:TextBox ID="txtQty" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtQty_OnTextChanged"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                                                TargetControlID="txtQty" ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label11" runat="server" Text="Rate"></asp:Label>
                                                            <a style="color: Red" id="Rate" runat="server">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RFVRate" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtRate" ErrorMessage="Enter Rate"></asp:RequiredFieldValidator>
                                                            <asp:TextBox ID="txtRate" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtQty_OnTextChanged"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                TargetControlID="txtRate" ValidChars="0,1,2,3,4,5,6,7,8,9,.">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label4" runat="server" Text="Amount"></asp:Label>
                                                            <a style="color: Red" id="Amount" runat="server" visible="false">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RFVAmount" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtAmount" ErrorMessage="Enter Amount" Enabled="false"></asp:RequiredFieldValidator>
                                                            <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblTankFull" runat="server" Text="Tank Full"></asp:Label>
                                                            <asp:DropDownList ID="ddlTankFull" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="No" Value="False"></asp:ListItem>
                                                                <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblPreReading" runat="server" Text="Pre-Reading"></asp:Label>
                                                            <a style="color: Red" id="PreReading" runat="server">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RFVPreReading" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtPreReading" ErrorMessage="Enter Pre-Reading"></asp:RequiredFieldValidator>
                                                            <asp:TextBox ID="txtPreReading" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                TargetControlID="txtPreReading" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblCurrentReading" runat="server" Text="Current Reading"></asp:Label>
                                                            <a style="color: Red" id="CurrentReading" runat="server">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RFVCurrentReading" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCurrentReading" ErrorMessage="Enter Current Reading"></asp:RequiredFieldValidator>
                                                            <asp:TextBox ID="txtCurrentReading" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                                TargetControlID="txtCurrentReading" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblPSReading" runat="server" Text="Pump Start Reading"></asp:Label>
                                                            <a style="color: Red" id="PSReading" runat="server">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator8" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtPumpStartReading" ErrorMessage="Enter pump start Reading"></asp:RequiredFieldValidator>
                                                            <asp:TextBox ID="txtPumpStartReading" runat="server" AutoPostBack="true" OnTextChanged="txtQty_OnTextChanged" CssClass="form-control"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                                TargetControlID="txtPumpStartReading" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblPEReading" runat="server" Text="Pump End Reading"></asp:Label>
                                                            <a style="color: Red" id="PEReading" runat="server">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator9" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtPumpEndReading" ErrorMessage="Enter pump end Reading"></asp:RequiredFieldValidator>
                                                            <asp:TextBox ID="txtPumpEndReading" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                                TargetControlID="txtPumpEndReading" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-12">
                                                            <asp:Label ID="Label17" runat="server" Text="<%$ Resources:Attendance,Remarks%>"></asp:Label>
                                                            <asp:TextBox ID="txtRemarks" runat="server" Style="resize: vertical; max-height: 200px; min-height: 50px;" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <br />
                                                            <asp:CheckBox ID="chkAccountEntry" runat="server" Text="Account Entry" CssClass="form-control" OnCheckedChanged="chkAccountEntry_CheckedChanged" AutoPostBack="true" Checked="true" />
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6" style="display: none">
                                                            <asp:Label ID="Label13" runat="server" Text="Other Account No"></asp:Label>
                                                            <asp:TextBox ID="txtOtherAccountNo" BackColor="#eeeeee" runat="server" CssClass="form-control" OnTextChanged="txtCustomer_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="txtSupplierName_AutoCompleteExtender" runat="server"
                                                                CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                                                ServiceMethod="GetCompletionListCustomer" ServicePath="" TargetControlID="txtOtherAccountNo"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6" style="display: none">
                                                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,For Debit Account %>"></asp:Label>
                                                            <asp:TextBox ID="txtpaymentdebitaccount" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtpaymentdebitaccount"
                                                                UseContextKey="True"
                                                                CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6" id="DivCreditAccount" runat="server">
                                                            <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Attendance,For Credit Account %>"></asp:Label>
                                                            <asp:TextBox ID="txtpaymentCreditaccount" runat="server" BackColor="#eeeeee"
                                                                AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged" CssClass="form-control"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtpaymentCreditaccount"
                                                                UseContextKey="True"
                                                                CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                        </div>
                                                        <div id="trSupplier" runat="server" visible="false" class="col-md-6">
                                                            <asp:Label ID="lblSupplierName" runat="server" Text="<%$ Resources:Attendance,Supplier %>" />
                                                            <a style="color: Red">*</a>
                                                            <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSupplierName" ErrorMessage="<%$ Resources:Attendance,Enter Supplier%>"></asp:RequiredFieldValidator>
                                                            <asp:TextBox ID="txtSupplierName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                                AutoPostBack="True" OnTextChanged="txtSupplierName_TextChanged" />
                                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server"
                                                                CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                                                ServiceMethod="GetSuppliersList" ServicePath="" TargetControlID="txtSupplierName"
                                                                UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <br />
                                                        <asp:UpdatePanel ID="Update_New_Button" runat="server">
                                                            <ContentTemplate>
                                                                <asp:Button ID="btnSave" ValidationGroup="Save" runat="server" Text="<%$ Resources:Attendance,Save %>" Visible="false" CssClass="btn btn-success" OnClick="btnSave_Click" />
                                                                <asp:Button ID="btn_Post" runat="server" ValidationGroup="Save" Text="<%$ Resources:Attendance,Post%>"
                                                                    class="btn btn-primary" OnClick="Btn_Post_Click" Visible="false" />
                                                                <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure to post record ?%>"
                                                                    TargetControlID="btn_Post">
                                                                </cc1:ConfirmButtonExtender>
                                                                <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>" CssClass="btn btn-primary" CausesValidation="False" OnClick="btnReset_Click" />
                                                                <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>" CssClass="btn btn-danger" CausesValidation="False" OnClick="btnCancel_Click" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <asp:HiddenField ID="editid" runat="server" />
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
                                                    <asp:Label ID="Label7" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
						<asp:Label ID="lblbinTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>

						<div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                <i id="I2" runat="server" class="fa fa-plus"></i>
                            </button>
                        </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlbinFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Selected="True" Text="Vehicle Name" Value="VehicleName"></asp:ListItem>
                                                        <asp:ListItem Text="Driver Name" Value="Driver_Name"></asp:ListItem>
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
                                                        <asp:TextBox ID="txtbinValue" runat="server" CssClass="form-control" placeholder="Search from Content"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbinbind" runat="server" CausesValidation="False" OnClick="btnbinbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnbinRefresh" runat="server" CausesValidation="False" OnClick="btnbinRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imgBtnRestore" CausesValidation="False" Visible="false" runat="server" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid" <%= gvExpensesMasterBin.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <br />
                                                <div class="flow">
                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvExpensesMasterBin" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                        runat="server" AutoGenerateColumns="False" DataKeyNames="Trans_Id" Width="100%"
                                                        AllowPaging="True" OnPageIndexChanging="gvExpensesMasterBin_PageIndexChanging"
                                                        OnSorting="gvExpensesMasterBin_OnSorting" AllowSorting="true">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                                </ItemTemplate>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChanged"
                                                                        AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Trans_Id" SortExpression="Trans_Id" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTransId" runat="server" Text='<%# Eval("Trans_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Trans Date" SortExpression="Trans_date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTransdate" runat="server" Text='<%# GetDate(Eval("Trans_date").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Vehicle Name" SortExpression="VehicleName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblVehicleName" runat="server" Text='<%# Eval("VehicleName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Driver Name" SortExpression="Driver_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDriverName" runat="server" Text='<%# Eval("Driver_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Product" SortExpression="ProductCode">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("ProductCode") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Qty" SortExpression="qty">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblqty" runat="server" Text='<%# Common.GetAmountDecimal(Eval("qty").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Rate" SortExpression="Rate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRate" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Rate").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Amount" SortExpression="Amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAmount" runat="server" Text='<%#Common.GetAmountDecimal(Eval("Amount").ToString(),Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) %>'></asp:Label>
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
                    <div class="tab-pane" id="Mileage">
                        <asp:UpdatePanel ID="Update_Mileage" runat="server">
                            <ContentTemplate>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div3" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label8" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
						<asp:Label ID="lblTotalRecordMileage" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I3" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:TextBox ID="txtVehicleSearch" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server"
                                                        CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                                        ServiceMethod="GetVehicleList" ServicePath="" TargetControlID="txtVehicleSearch"
                                                        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                    </cc1:AutoCompleteExtender>
                                                </div>
                                                <div class="col-lg-1">
                                                    <asp:TextBox ID="txtYear" runat="server" CssClass="form-control" MaxLength="4"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTBYear" runat="server" Enabled="True"
                                                        TargetControlID="txtYear" ValidChars="0,1,2,3,4,5,6,7,8,9">
                                                    </cc1:FilteredTextBoxExtender>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlMonth" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="January" Value="01"></asp:ListItem>
                                                        <asp:ListItem Text="Febuary" Value="02"></asp:ListItem>
                                                        <asp:ListItem Text="March" Value="03"></asp:ListItem>
                                                        <asp:ListItem Text="April" Value="04"></asp:ListItem>
                                                        <asp:ListItem Text="May" Value="05"></asp:ListItem>
                                                        <asp:ListItem Text="June" Value="06"></asp:ListItem>
                                                        <asp:ListItem Text="July" Value="07"></asp:ListItem>
                                                        <asp:ListItem Text="August" Value="08"></asp:ListItem>
                                                        <asp:ListItem Text="September" Value="09"></asp:ListItem>
                                                        <asp:ListItem Text="October" Value="10"></asp:ListItem>
                                                        <asp:ListItem Text="November" Value="11"></asp:ListItem>
                                                        <asp:ListItem Text="December" Value="12"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlLowMileage" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="All" Value="No"></asp:ListItem>
                                                        <asp:ListItem Text="Low Mileage" Value="Yes"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnBind_Mileage" runat="server" CausesValidation="False" OnClick="btnBind_Mileage_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh_Mileage" runat="server" CausesValidation="False" OnClick="btnRefresh_Mileage_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                </div>
                                                

                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid" <%= gvMileage.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvMileage" runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="false"
                                                        AllowSorting="true" OnPreRender="gvMileage_PreRender" OnSorting="gvMileage_Sorting">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="TransDate" SortExpression="Trans_date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTransDate" runat="server" Text='<%#  GetDate(Eval("Trans_date").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="VehicleName" SortExpression="Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTransDate" runat="server" Text='<%#  Eval("Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="PlateNo" SortExpression="Plate_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPlateNo" runat="server" Text='<%#  Eval("Plate_No") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ContractorName" SortExpression="ContractorName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblContractorName" runat="server" Text='<%#  Eval("ContractorName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="VehicleType" SortExpression="Field1">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblVehicleType" runat="server" Text='<%#  Eval("Field1") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Mileage" SortExpression="Mileage">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblMileage" runat="server" Text='<%#  Eval("Mileage") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="CreateBy" SortExpression="Created_User">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCreatedBy" runat="server" Text='<%#  Eval("Created_User") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="VehicleId" SortExpression="Vehicle_Id" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblVehicleId" runat="server" Text='<%#  Eval("Vehicle_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>



                                                    </asp:GridView>
                                                    &nbsp;&nbsp;&nbsp;
                                                    <asp:Panel ID="PagingPanelMileage" runat="server"></asp:Panel>
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
        function Li_Tab_List() {
            document.getElementById('<%= Btn_List.ClientID %>').click();
        }
        function Li_Tab_New() {
            document.getElementById('<%= Btn_New.ClientID %>').click();
        }
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }
        function Li_Tab_Mileage() {
            document.getElementById('<%= Btn_Mileage.ClientID %>').click();
        }
        function LI_Edit_Active() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");
            $("#Li_New").addClass("active");
            $("#New").addClass("active");
            $("#Li_Mileage").removeClass("active");
            $("#Mileage").removeClass("active");
        }
        function LI_List_Active() {
            $("#Li_List").addClass("active");
            $("#List").addClass("active");
            $("#Li_New").removeClass("active");
            $("#New").removeClass("active");
            $("#Li_Mileage").removeClass("active");
            $("#Mileage").removeClass("active");
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
    </script>
</asp:Content>
