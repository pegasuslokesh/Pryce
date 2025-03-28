<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="ModelMaster.aspx.cs" Inherits="Inventory_ModelMaster" %>

<%@ Register Src="~/WebUserControl/FileManager.ascx" TagName="FileManager" TagPrefix="UC" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<%@ Register Src="~/WebUserControl/FileUpload.ascx" TagPrefix="AT1" TagName="FileUpload1" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">

    <script>
        function LI_Edit_Active() {
            $("#Li_List").removeClass("active");
            $("#List").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }
    </script>
    <style type="text/css">
        .Initial {
            color: #666666;
            -moz-box-shadow: inset 0px 1px 0px 0px #dcecfb;
            -webkit-box-shadow: inset 0px 1px 0px 0px #dcecfb;
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0.05, #bddbfa), color-stop(1, #80b5ea));
            background: -moz-linear-gradient(top, #bddbfa 5%, #80b5ea 100%);
            background: -webkit-linear-gradient(top, #bddbfa 5%, #80b5ea 100%);
            background: -o-linear-gradient(top, #bddbfa 5%, #80b5ea 100%);
            background: -ms-linear-gradient(top, #bddbfa 5%, #80b5ea 100%);
            background: linear-gradient(to bottom, #bddbfa 5%, #80b5ea 100%);
            background-color: #bddbfa;
            -moz-border-radius: 6px;
            -webkit-border-radius: 6px;
            border: 1px solid #84bbf3;
            display: inline-block;
            color: #ffffff;
            font-family: arial;
            font-size: 13px;
            font-weight: bold;
            margin-left: -4px;
            text-decoration: none;
            height: 30px;
            cursor: pointer;
        }

        .Clicked {
            -moz-box-shadow: inset 0px 1px 0px 0px #ffffff;
            -webkit-box-shadow: inset 0px 1px 0px 0px #ffffff;
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0.05, #f9f9f9), color-stop(1, #e9e9e9));
            background: -moz-linear-gradient(top, #f9f9f9 5%, #e9e9e9 100%);
            background: -webkit-linear-gradient(top, #f9f9f9 5%, #e9e9e9 100%);
            background: -o-linear-gradient(top, #f9f9f9 5%, #e9e9e9 100%);
            background: -ms-linear-gradient(top, #f9f9f9 5%, #e9e9e9 100%);
            background: linear-gradient(to bottom, #f9f9f9 5%, #e9e9e9 100%);
            background-color: #f9f9f9;
            -moz-border-radius: 6px;
            -webkit-border-radius: 6px;
            border: 1px solid #dcdcdc;
            display: inline-block;
            color: #666666;
            font-family: arial;
            font-size: 13px;
            font-weight: bold;
            text-decoration: none;
            height: 30px;
            cursor: pointer;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-puzzle-piece"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Model Setup%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Model Master%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:Button ID="Btn_View_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Modal_Address" Text="View Modal" />
            <asp:Button ID="Btn_Modal_FileUpload" Style="display: none;" runat="server" data-toggle="modal" data-target="#Fileupload123" Text="FileUpload" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
            <asp:HiddenField runat="server" ID="hdnCanUpload" />
            <asp:HiddenField runat="server" ID="hdnCanView" />
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
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblModelCategory" runat="server" Text="<%$ Resources:Attendance,Model Category %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlcategorysearch" runat="server" CssClass="form-control">
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblSuppliersearch" runat="server" Text="<%$ Resources:Attendance,Model Supplier %>" />
                                                        <asp:TextBox ID="txtSupplierSearch" runat="server" BackColor="#eeeeee" OnTextChanged="txtSupplierSearch_OnTextChanged"
                                                            AutoPostBack="True" CssClass="form-control"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionInterval="100"
                                                            DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionList_Supplier"
                                                            ServicePath="" TargetControlID="txtSupplierSearch" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btngo" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Go %>"
                                                            CssClass="btn btn-primary" OnClick="btngo_Click" />

                                                        <asp:Button ID="btnResetSearch" runat="server" CausesValidation="False" Text="<%$ Resources:Attendance,Reset %>"
                                                            CssClass="btn btn-primary" OnClick="btnResetSearch_Click" />
                                                        <br />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div1" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
 <asp:Label ID="lblTotalRecords" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
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
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Model Name %>" Value="Model_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Model Name(Local) %>" Value="Model_Name_L"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Model No %>" Value="Model_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="Created_User"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Modified By %>" Value="Modified_User"></asp:ListItem>
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
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imbBtnGrid" CausesValidation="False" runat="server" OnClick="imbBtnGrid_Click" ToolTip="<%$ Resources:Attendance, Grid View %>" Visible="False"><span class="fa fa-sitemap"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imgBtnDatalist" CausesValidation="False" runat="server" OnClick="imgBtnDatalist_Click" ToolTip="<%$ Resources:Attendance,List View %>" ><span class="fa fa-list"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid">
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div style="overflow: auto; height: 500px;">
                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvModelMaster"  PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                        OnPageIndexChanging="gvModelMaster_PageIndexChanging" OnSorting="gvModelMaster_OnSorting">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">

                                                                            <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnView" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' CausesValidation="False" OnCommand="btnView_Command"><i class="fa fa-eye"></i>View</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' CausesValidation="False" OnCommand="btnEdit_Command"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm2" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>

                                                                            <li <%= hdnCanUpload.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnFileUpload" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' CommandName='<%# Eval("Model_No") %>' OnCommand="btnFileUpload_Command" CausesValidation="False"><i class="fa fa-upload"></i>File Upload</asp:LinkButton>
                                                                            </li>
                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Model No %>" SortExpression="Model_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModelNo" runat="server" Text='<%# Eval("Model_No") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Model Name %>" SortExpression="Model_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModelName" runat="server" Text='<%# Eval("Model_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Model Name(Local) %>" SortExpression="Model_Name_L">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModelNameL" runat="server" Text='<%# Eval("Model_Name_L") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Basic Price %>" SortExpression="Field1">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBasic_Price" runat="server" Text='<%# GetCurrencySymbol(Eval("Field1").ToString(),Eval("Field4").ToString()) %>'></asp:Label>
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
                                                        </Columns>


                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>

                                                    <asp:DataList ID="dtlistProduct" Visible="false" runat="server" RepeatColumns="2" RepeatDirection="Horizontal" Width="100%">
                                                        <ItemTemplate>

                                                            <div class="col-md-6" style="min-width: 500px;">
                                                                <div class="box box-primary ">
                                                                    <div class="box-header with-border">
                                                                        <div style="background-color: ghostwhite" class="col-md-12">
                                                                            <div class="col-md-3">
                                                                                <br />
                                                                                <asp:ImageButton ID="btnEdit" runat="server" OnCommand="btnEdit_Command" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                    Width="90px" Height="90px" Enabled='<%# hdnCanEdit.Value=="true"?true :false %>' ImageUrl='<%#  string.IsNullOrEmpty(Eval("Field2").ToString()) ? "~/Login/Images/place-holder.png" :"~/CompanyResource/"+Eval("Company_Id")+"/Model/" +Eval("Field2")%>' />
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-9">
                                                                                <br />
                                                                                <table>
                                                                                    <tr>
                                                                                        <td style="width: 90px;">
                                                                                            <asp:Label ID="lblProductId" runat="server" Text="<%$ Resources:Attendance,Model No %>"></asp:Label>
                                                                                        </td>
                                                                                        <td style="width: 5px; text-align: center">&nbsp:&nbsp
                                                                                        </td>
                                                                                        <td style="width: 250px;">
                                                                                            <asp:Label ID="lbldlProductId" runat="server" Text='<%# Eval("Model_No") %>'></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="Label19" runat="server" Text="<%$ Resources:Attendance,Model Name %>"></asp:Label>
                                                                                        </td>
                                                                                        <td>:
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:LinkButton ID="lbldlProductName" runat="server" Font-Bold="true" ForeColor="#1886b9"
                                                                                                Style="text-decoration: none;" OnCommand="btnEdit_Command"
                                                                                                CommandArgument='<%# Eval("Trans_Id") %>' Text='<%# Eval("Model_Name") %>'
                                                                                                Enabled='<%# hdnCanEdit.Value=="true"?true :false %>'></asp:LinkButton>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="Label20" runat="server" Text="<%$ Resources:Attendance,Basic Price %>"></asp:Label>
                                                                                        </td>
                                                                                        <td>:
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblBasicPrice" runat="server" Text='<%# GetCurrencySymbol(Eval("Field1").ToString(),Eval("Field4").ToString()) %>'></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Created By %>"></asp:Label>
                                                                                        </td>
                                                                                        <td>:
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="Label10" runat="server" Text='<%# Eval("Created_User") %>'></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="Label24" runat="server" Text="<%$ Resources:Attendance,Modified By %>"></asp:Label>
                                                                                        </td>
                                                                                        <td>:
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="Label12" runat="server" Text='<%# Eval("Modified_User") %>'></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                                <br />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <br />
                                                            </div>
                                                            <br />
                                                        </ItemTemplate>
                                                    </asp:DataList>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                    <!-- /.box-body -->
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

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
                    <div class="tab-pane" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblModelNo" runat="server" Text="<%$ Resources:Attendance,Model No %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtModelNo" ErrorMessage="<%$ Resources:Attendance,Enter Model No %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtModelNo" runat="server" CssClass="form-control" AutoPostBack="True"
                                                            OnTextChanged="txtModelNo_TextChanged" BackColor="#eeeeee" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionList1" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtModelNo" UseContextKey="True"
                                                            CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                            TargetControlID="txtModelNo" FilterMode="InvalidChars" InvalidChars="*%-_+#@<?/\|~!$&^{}:;.,=>()">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                        <asp:Label ID="lblEModelName" runat="server" Text="<%$ Resources:Attendance,Model Name %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEModelName" ErrorMessage="<%$ Resources:Attendance,Enter Model Name %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtEModelName" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            AutoPostBack="True" OnTextChanged="txtEModelName_TextChanged" />
                                                        <cc1:AutoCompleteExtender ID="txtEUnitName_AutoCompleteExtender" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionList" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtEModelName"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                        <asp:Label ID="lblLModelName" runat="server" Text="<%$ Resources:Attendance,Model Name(Local) %>"></asp:Label>
                                                        <asp:TextBox ID="txtlModelName" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblPImage" runat="server" Text="<%$ Resources:Attendance,Product Image %>"></asp:Label>
                                                        <div class="input-group" style="width: 100%; display:none;">
                                                            <cc1:AsyncFileUpload ID="fugProduct"
                                                                OnClientUploadStarted="FuLogo_UploadStarted"
                                                                OnClientUploadError="FuLogo_UploadError"
                                                                OnClientUploadComplete="FuLogo_UploadComplete"
                                                                OnUploadedComplete="FuLogo_FileUploadComplete"
                                                                runat="server" CssClass="form-control"
                                                                CompleteBackColor="White"
                                                                UploaderStyle="Traditional"
                                                                UploadingBackColor="#CCFFFF"
                                                                ThrobberID="FULogo_ImgLoader" Width="100%" />
                                                            <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                                <asp:LinkButton ID="FULogo_Img_Right" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-check" style="font-size:30px;color:#22cb33"></i></asp:LinkButton>
                                                                <asp:LinkButton ID="FULogo_Img_Wrong" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-remove" style="font-size:30px"></i></asp:LinkButton>
                                                                <asp:Image ID="FULogo_ImgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                            </div>
                                                        </div>
                                                        <br />
                                                        <div style="text-align: center">
                                                            <asp:Image ID="imgProduct" ClientIDMode="Static" ImageUrl="../Bootstrap_Files/dist/img/NoImage.jpg" runat="server" Height="120px" Width="120px" />
                                                            <br />
                                                            <asp:Button Style="margin-top: 10px" Visible="false" ID="btnloadimg" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance,Load %>" OnClick="btnloadimg_Click" />

                                                            <a onclick="popup.Show();" runat="server" id="File_Manager_Model" style="margin-top: 9px;" class="btn btn-primary">File Manager</a>

                                                            <%--   <dx:ASPxButton ID="dxBtnFileUpload" runat="server" Text="File Manager" AutoPostBack="false">
                                                                <ClientSideEvents Click="function(s, e) {
            popup.Show();
        }" />
                                                            </dx:ASPxButton>--%>

                                                            <asp:Button Style="margin-top: 10px" Visible="false" ID="btnRemove" runat="server" Text="Remove" OnClick="btnRemove_OnClick" CssClass="btn btn-primary" />
                                                        </div>
                                                        <br />
                                                    </div>

                                                     <div class="col-md-6">
                                                        <asp:Label ID="lblBasicPrice" runat="server" Text="<%$ Resources:Attendance,Basic Price %>"></asp:Label>
                                                        <asp:TextBox ID="txtBasicPrice" runat="server" CssClass="form-control" Columns="2" />
                                                        <cc1:FilteredTextBoxExtender ID="FiltxtExBasicPrice" runat="server" Enabled="True"
                                                            TargetControlID="txtBasicPrice" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                     <div class="col-md-6">
                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Attendance,Cost Price %>"></asp:Label>
                                                        <asp:TextBox ID="txtCostPrice" runat="server" CssClass="form-control" Columns="2" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredtxtCostPrice" runat="server" Enabled="True"
                                                            TargetControlID="txtCostPrice" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <br />
                                                    </div>
                                                   
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Currency %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="form-control">
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Description %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtModelDecription" ErrorMessage="<%$ Resources:Attendance,Enter Description %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtModelDecription" runat="server" TextMode="MultiLine"
                                                            CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <cc1:TabContainer ID="tab" runat="server" CssClass="ajax__tab_yuitabview-theme">
                                                            <cc1:TabPanel ID="tabpnlDesc" runat="server" HeaderText="Description">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_tabpnlDesc" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <cc1:Editor ID="txtDescription" runat="server" Height="300px" />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_tabpnlDesc">
                                                                        <ProgressTemplate>
                                                                            <div class="modal_Progress">
                                                                                <div class="center_Progress">
                                                                                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                                </div>
                                                                            </div>
                                                                        </ProgressTemplate>
                                                                    </asp:UpdateProgress>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                            <cc1:TabPanel ID="tabproductCategory" runat="server" HeaderText="Related Product">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_tabproductCategory" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-6">
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtproductcategorysearch" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                                            BackColor="#eeeeee" OnTextChanged="txtproductcategorysearch_OnTextChanged"></asp:TextBox>
                                                                                        <cc1:TextBoxWatermarkExtender ID="txtwatermarkup" runat="server" TargetControlID="txtproductcategorysearch"
                                                                                            WatermarkText="Search Product Category">
                                                                                        </cc1:TextBoxWatermarkExtender>
                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                                                            Enabled="True" ServiceMethod="GetCompletionListCategoryname" ServicePath="" CompletionInterval="100"
                                                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtproductcategorysearch"
                                                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                        </cc1:AutoCompleteExtender>
                                                                                        <div class="input-group-btn">
                                                                                            <asp:ImageButton ID="imgsearchproductcategory" runat="server" CausesValidation="False"
                                                                                                ImageUrl="~/Images/search.png" ToolTip="<%$ Resources:Attendance,Search %>"
                                                                                                OnClick="imgsearchproductcategory_OnClick" Visible="false"></asp:ImageButton>

                                                                                            <asp:LinkButton ID="ImgRefreshProductcategory" runat="server" CausesValidation="False" ToolTip="<%$ Resources:Attendance,Refresh %>" OnClick="ImgRefreshProductcategory_OnClick"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                                                                        </div>
                                                                                    </div>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtproductsearch" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                                            BackColor="#eeeeee" OnTextChanged="txtproductsearch_OnTextChanged"></asp:TextBox>
                                                                                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender5" runat="server" TargetControlID="txtproductsearch"
                                                                                            WatermarkText="Search Product Name">
                                                                                        </cc1:TextBoxWatermarkExtender>
                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                                                            Enabled="True" ServiceMethod="GetCompletionListProductName" ServicePath="" CompletionInterval="100"
                                                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtproductsearch"
                                                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                        </cc1:AutoCompleteExtender>
                                                                                        <div class="input-group-btn">
                                                                                            <asp:ImageButton ID="imgsearchproduct" runat="server" CausesValidation="False"
                                                                                                ImageUrl="~/Images/search.png" ToolTip="<%$ Resources:Attendance,Search %>"
                                                                                                OnClick="imgsearchproduct_OnClick" Visible="false"></asp:ImageButton>

                                                                                            <asp:LinkButton ID="ImgRefreshProduct" runat="server" CausesValidation="False" ToolTip="<%$ Resources:Attendance,Refresh %>" OnClick="ImgRefreshProduct_OnClick"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                                                                        </div>
                                                                                    </div>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <div style="height: 300px; border-style: solid; border-width: 1px; border-color: #eeeeee; overflow: auto">
                                                                                        <asp:CheckBoxList ID="ChkProductCategory" Style="margin-left: 10px;" runat="server" RepeatColumns="1" Font-Names="Trebuchet MS"
                                                                                            AutoPostBack="true" Font-Size="Small" ForeColor="Gray" OnSelectedIndexChanged="ChkProductCategory_SelectedIndexChanged" />
                                                                                    </div>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <div style="height: 300px; border-style: solid; border-width: 1px; border-color: #eeeeee; overflow: auto">
                                                                                        <asp:CheckBoxList ID="ChkProductChildCategory" Style="margin-left: 10px;" runat="server" RepeatColumns="1" Font-Names="Trebuchet MS" AutoPostBack="true" OnSelectedIndexChanged="ChkProductChildCategory_SelectedIndexChanged"
                                                                                            Font-Size="Small" ForeColor="Gray" AppendDataBoundItems="False" />
                                                                                    </div>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <div style="overflow: auto">
                                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvRelatedProduct" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                                                                            BorderStyle="Solid" Width="100%" PageSize="5" OnPageIndexChanging="gvRelatedProduct_PageIndexChanging">
                                                                                            <Columns>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:ImageButton ID="IbtnDeleteProduct" runat="server" CausesValidation="False" CommandArgument='<%# Eval("ProductId") %>'
                                                                                                            ImageUrl="~/Images/Erase.png" Width="16px" ToolTip="<%$ Resources:Attendance,Delete %>"
                                                                                                            OnCommand="IbtnDeleteProduct_Command" />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Category Name %>" SortExpression="CategoryName">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvCategoryName" runat="server" Text='<%#Eval("CategoryName") %>' />
                                                                                                        <asp:Label ID="lblgvCategoryid" runat="server" Text='<%#Eval("CategoryId") %>' Visible="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle Width="25%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id %>" SortExpression="ProductCode">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvProductCode" runat="server" Text='<%#Eval("ProductCode") %>' />
                                                                                                        <asp:Label ID="lblgvProductid" runat="server" Text='<%#Eval("ProductId") %>' Visible="false"></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle Width="20%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>" SortExpression="ProductName">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvProductName" runat="server" Text='<%#Eval("ProductName") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle Width="35%" />
                                                                                                </asp:TemplateField>
                                                                                            </Columns>

                                                                                            
                                                                                            <PagerStyle CssClass="pagination-ys" />

                                                                                        </asp:GridView>
                                                                                    </div>
                                                                                    <br />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_tabproductCategory">
                                                                        <ProgressTemplate>
                                                                            <div class="modal_Progress">
                                                                                <div class="center_Progress">
                                                                                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                                </div>
                                                                            </div>
                                                                        </ProgressTemplate>
                                                                    </asp:UpdateProgress>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                            <cc1:TabPanel ID="TabModelCategory" runat="server" HeaderText="<%$ Resources:Attendance,Model Category %>">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_TabModelCategory" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12" style="text-align: center">
                                                                                    <asp:Label ID="lblCategory" runat="server" Font-Bold="True" Font-Names="arial" Font-Size="13px"
                                                                                        ForeColor="#666666" Text="<%$ Resources:Attendance,Category %>"></asp:Label>
                                                                                </div>
                                                                                <div class="col-md-2"></div>
                                                                                <div class="col-md-3">
                                                                                    <asp:ListBox ID="lstProductCategory" runat="server"
                                                                                        SelectionMode="Multiple" CssClass="list" Width="100%" Height="300px" Font-Names="Trebuchet MS" Font-Size="Small"
                                                                                        ForeColor="Gray"></asp:ListBox>
                                                                                </div>
                                                                                <div class="col-lg-2" style="text-align: center">
                                                                                    <div style="margin-top: 75px; margin-bottom: 75px;" class="btn-group-vertical">
                                                                                        <asp:Button ID="btnPushCate" runat="server" CssClass="btn btn-info" Text=">" OnClick="btnPushCate_Click" />

                                                                                        <asp:Button ID="btnPullCate" runat="server" CssClass="btn btn-info" Text="<" OnClick="btnPullCate_Click" />

                                                                                        <asp:Button ID="btnPushAllCate" runat="server" CssClass="btn btn-info" Text=">>" OnClick="btnPushAllCate_Click" />

                                                                                        <asp:Button ID="btnPullAllCate" runat="server" CssClass="btn btn-info" Text="<<" OnClick="btnPullAllCate_Click" />
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-3">
                                                                                    <asp:ListBox ID="lstSelectProductCategory" runat="server"
                                                                                        SelectionMode="Multiple" Height="300px" Width="100%" CssClass="list" Font-Names="Trebuchet MS" Font-Size="Small"
                                                                                        ForeColor="Gray" Font-Bold="true"></asp:ListBox>
                                                                                </div>
                                                                                <div class="col-md-2"></div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_TabModelCategory">
                                                                        <ProgressTemplate>
                                                                            <div class="modal_Progress">
                                                                                <div class="center_Progress">
                                                                                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                                </div>
                                                                            </div>
                                                                        </ProgressTemplate>
                                                                    </asp:UpdateProgress>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                            <cc1:TabPanel ID="TabProductSupplier" runat="server" HeaderText="<%$ Resources:Attendance,Supplier %>">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_TabProductSupplier" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblSupplier" runat="server" Text="<%$ Resources:Attendance,Select Supplier %>" />
                                                                                    <asp:TextBox ID="txtSuppliers" runat="server" BackColor="#eeeeee" OnTextChanged="txtSuppliers_OnTextChanged"
                                                                                        AutoPostBack="True" CssClass="form-control"></asp:TextBox>
                                                                                    <cc1:AutoCompleteExtender ID="txtSuppliers_AutoCompleteExtender" runat="server" CompletionInterval="100"
                                                                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionList_Supplier"
                                                                                        ServicePath="" TargetControlID="txtSuppliers" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                    </cc1:AutoCompleteExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblProCode" runat="server" Text="<%$ Resources:Attendance,Supplier Code %>" />
                                                                                    <a style="color: Red">*</a>
                                                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator4" ValidationGroup="ProductSupplierCode"
                                                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtProductSupplierCode" ErrorMessage="<%$ Resources:Attendance,Enter Supplier Code %>"></asp:RequiredFieldValidator>

                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtProductSupplierCode" runat="server" CssClass="form-control"></asp:TextBox>

                                                                                        <div class="input-group-btn">
                                                                                            <asp:LinkButton ID="IbtnAddProductSupplierCode" ValidationGroup="ProductSupplierCode" runat="server" CausesValidation="False" OnClick="IbtnAddProductSupplierCode_Click" ToolTip="<%$ Resources:Attendance,Add %>"><i class="fa fa-plus-square" style="font-size:35px;padding:5px;margin-top: -5px;"></i></asp:LinkButton>
                                                                                            <asp:HiddenField ID="hdnProductSupplierCode" runat="server" Value="0" />
                                                                                        </div>
                                                                                    </div>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <div style="overflow: auto">
                                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvModelSupplierCode" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                                            BorderStyle="Solid" Width="100%" PageSize="5" OnPageIndexChanging="gvModelSupplierCode_PageIndexChanging">
                                                                                            <Columns>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:ImageButton ID="IbtnDeleteSupplier" runat="server" CausesValidation="False"
                                                                                                            CommandArgument='<%# Eval("Supplier_Id") %>' ImageUrl="~/Images/Erase.png" Width="16px"
                                                                                                            ToolTip="<%$ Resources:Attendance,Delete %>" OnCommand="IbtnDeleteSupplier_Command" />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Supplier Name %>" SortExpression="Name">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvSupplierName" runat="server" Text='<%#Eval("Name") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle Width="60%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Model Supplier Code %>"
                                                                                                    SortExpression="ProductSupplierCode">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvProductSupplierCode" runat="server" Text='<%#Eval("ModelSupplierCode") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                            </Columns>

                                                                                            
                                                                                            <PagerStyle CssClass="pagination-ys" />

                                                                                        </asp:GridView>
                                                                                        <asp:HiddenField ID="hdnfPSC" runat="server" />
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress7" runat="server" AssociatedUpdatePanelID="Update_TabProductSupplier">
                                                                        <ProgressTemplate>
                                                                            <div class="modal_Progress">
                                                                                <div class="center_Progress">
                                                                                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                                </div>
                                                                            </div>
                                                                        </ProgressTemplate>
                                                                    </asp:UpdateProgress>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                            <cc1:TabPanel ID="TabpnlHeader" runat="server" HeaderText="Header">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_TabpnlHeader" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <cc1:Editor ID="txtHeader" Height="300px" runat="server" CssClass="form-control" />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress8" runat="server" AssociatedUpdatePanelID="Update_TabpnlHeader">
                                                                        <ProgressTemplate>
                                                                            <div class="modal_Progress">
                                                                                <div class="center_Progress">
                                                                                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                                </div>
                                                                            </div>
                                                                        </ProgressTemplate>
                                                                    </asp:UpdateProgress>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                            <cc1:TabPanel ID="TabPnlFooter" runat="server" HeaderText="Footer">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_TabPnlFooter" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <cc1:Editor ID="txtFooter" Height="300px" runat="server" CssClass="form-control" />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress9" runat="server" AssociatedUpdatePanelID="Update_TabPnlFooter">
                                                                        <ProgressTemplate>
                                                                            <div class="modal_Progress">
                                                                                <div class="center_Progress">
                                                                                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                                </div>
                                                                            </div>
                                                                        </ProgressTemplate>
                                                                    </asp:UpdateProgress>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                            <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="<%$ Resources:Attendance,Label Size %>">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_TabPanel1" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-2">
                                                                                    <asp:TextBox ID="txtwidth" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                    <a style="color: Red">*</a>
                                                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator5" ValidationGroup="Btn_Add"
                                                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtwidth" ErrorMessage="<%$ Resources:Attendance,Enter Width %>"></asp:RequiredFieldValidator>

                                                                                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" WatermarkText="Width(MM)"
                                                                                        TargetControlID="txtwidth">
                                                                                    </cc1:TextBoxWatermarkExtender>
                                                                                    <cc1:FilteredTextBoxExtender ID="txtwidth1" runat="server" Enabled="True" TargetControlID="txtwidth"
                                                                                        ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-2">
                                                                                    <asp:TextBox ID="txtHeight" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                    <a style="color: Red">*</a>
                                                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator6" ValidationGroup="Btn_Add"
                                                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtHeight" ErrorMessage="<%$ Resources:Attendance,Enter Height %>"></asp:RequiredFieldValidator>

                                                                                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" WatermarkText="Height(MM)"
                                                                                        TargetControlID="txtHeight">
                                                                                    </cc1:TextBoxWatermarkExtender>
                                                                                    <cc1:FilteredTextBoxExtender ID="txtHeight1" runat="server" Enabled="True" TargetControlID="txtHeight"
                                                                                        ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-2">
                                                                                    <asp:TextBox ID="txtgap" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                    <a style="color: Red">*</a>
                                                                                    <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator7" ValidationGroup="Btn_Add"
                                                                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtgap" ErrorMessage="<%$ Resources:Attendance,Enter GAP %>"></asp:RequiredFieldValidator>

                                                                                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" WatermarkText="gap(MM)"
                                                                                        TargetControlID="txtgap">
                                                                                    </cc1:TextBoxWatermarkExtender>
                                                                                    <cc1:FilteredTextBoxExtender ID="txtgap1" runat="server" Enabled="True" TargetControlID="txtgap"
                                                                                        ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-2">
                                                                                    <asp:DropDownList ID="ddlPerforation" runat="server" CssClass="form-control">
                                                                                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                                        <asp:ListItem Text="Perforation" Value="Perforation"></asp:ListItem>
                                                                                        <asp:ListItem Text="Kiss Cut" Value="Kiss Cut"></asp:ListItem>
                                                                                        <asp:ListItem Text="Security Cut" Value="Security Cut"></asp:ListItem>
                                                                                        <asp:ListItem Text="3 Section" Value="3 Section"></asp:ListItem>
                                                                                        <asp:ListItem Text="2 Section" Value="2 Section"></asp:ListItem>
                                                                                        <asp:ListItem Text="PathFinder" Value="PathFinder"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-2">
                                                                                    <asp:TextBox ID="txtNarration" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" WatermarkText="Narration"
                                                                                        TargetControlID="txtNarration">
                                                                                    </cc1:TextBoxWatermarkExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-2">
                                                                                    <asp:LinkButton runat="server" ValidationGroup="Btn_Add" ToolTip="<%$ Resources:Attendance,Add %>" ID="btnAdd" OnClick="btnAdd_Click"><i class="fa fa-plus-square" style="font-size:35px;padding:5px;margin-top: -5px;"></i></asp:LinkButton>
                                                                                    <asp:LinkButton runat="server" CausesValidation="False" ToolTip="<%$ Resources:Attendance,Delete %>" ID="imgDelete" OnClick="imgDelete_Click"><i class="fa fa-remove" style="font-size:35px;padding:5px;margin-top: -5px;"></i></asp:LinkButton>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-8">
                                                                                    <asp:ListBox ID="chkSelectedItems" Style="width: 100%; height: 200px;" runat="server" RepeatColumns="1" RepeatDirection="Vertical"
                                                                                        Font-Names="Trebuchet MS" Font-Size="Large" ForeColor="Gray"></asp:ListBox>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-4">
                                                                                    <asp:Label ID="lblSalesprice1" runat="server" Text="<%$ Resources:Attendance,Sales price 1 %>"></asp:Label>
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtsalePrice1" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                        <div class="input-group-btn">
                                                                                            <asp:Label ID="Label_78" runat="server" Text="Times" CssClass="form-control"></asp:Label>
                                                                                        </div>
                                                                                    </div>
                                                                                    <br />
                                                                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Sales price 2 %>"></asp:Label>
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtSalesprice2" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                        <div class="input-group-btn">
                                                                                            <asp:Label ID="Label14" runat="server" Text="Times" CssClass="form-control"></asp:Label>
                                                                                        </div>
                                                                                    </div>
                                                                                    <br />
                                                                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Sales price 3 %>"></asp:Label>
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtsalesprice3" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                        <div class="input-group-btn">
                                                                                            <asp:Label ID="Label15" runat="server" Text="Times" CssClass="form-control"></asp:Label>
                                                                                        </div>
                                                                                    </div>
                                                                                    <br />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress10" runat="server" AssociatedUpdatePanelID="Update_TabPanel1">
                                                                        <ProgressTemplate>
                                                                            <div class="modal_Progress">
                                                                                <div class="center_Progress">
                                                                                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                                                                                </div>
                                                                            </div>
                                                                        </ProgressTemplate>
                                                                    </asp:UpdateProgress>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                            <cc1:TabPanel ID="Tab_Sno_Config" runat="server" HeaderText="Auto Serial Config">
                                                                <ContentTemplate>
                                                                    <div class="row">
                                                                        <div class="col-md-4">
                                                                            <asp:Label ID="Label61" runat="server" Text="Prefix for auto serial"></asp:Label>
                                                                            <asp:TextBox ID="txtPrefixName" runat="server" CssClass="form-control"></asp:TextBox>
                                                                            <br />
                                                                        </div>

                                                                        <div class="col-md-4">
                                                                            <asp:Label ID="Label62" runat="server" Text="Suffix for auto Serial"></asp:Label>
                                                                            <asp:TextBox ID="txtSuffixName" runat="server" CssClass="form-control"></asp:TextBox>
                                                                        </div>
                                                                        <div class="col-md-4">
                                                                            <asp:Label ID="Label63" runat="server" Text="Auto Serial Start From"></asp:Label>
                                                                            <asp:TextBox ID="txtSnoStartFrom" runat="server" CssClass="form-control"></asp:TextBox>
                                                                            <asp:RegularExpressionValidator ID="reqTxtSnoStartFrom" ControlToValidate="txtSnoStartFrom" runat="server" ErrorMessage="Only Number allowed" ForeColor="Red" ValidationExpression="^[0-9]*$" ValidationGroup="NumericaValue"></asp:RegularExpressionValidator>
                                                                        </div>
                                                                         <div class="col-md-12" id="divLastSerialNo" runat="server">
                                                                            <asp:Label ID="Label11" runat="server" Text="Last Generated Serial No : "></asp:Label>
                                                                            <asp:Label ID="lblLastSerial" runat="server" Text="-" style="font-size: x-large;color: red;"></asp:Label>
                                                                        </div>
                                                                    </div>
                                                                    
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                        </cc1:TabContainer>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:CheckBox ID="chkIsLabel" runat="server" Text="Is Label" CssClass="form-control" Font-Bold="true" Font-Size="16px" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Attendance,Save %>" ValidationGroup="Save" CssClass="btn btn-success"
                                                            OnClick="btnSave_Click" Visible="false" />
                                                        <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                            CssClass="btn btn-primary" OnClick="btnReset_Click" CausesValidation="False" />
                                                        <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                            CssClass="btn btn-danger" OnClick="btnCancel_Click" CausesValidation="False" />
                                                        <asp:HiddenField ID="editid" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                       


                        <dx:ASPxPopupControl ID="ASPxPopupControl1" ClientInstanceName="popup" runat="server" Height="500px" Width="1000px" Modal="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                            <ClientSideEvents Shown="function(s,e){
                    fileManager.AdjustControl();
                }" />

                            <ContentCollection>
                                <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
                                    <%--<UC:FileManager ID="addaddress" runat="server" />--%>
                                    <dx:ASPxFileManager ID="ASPxFileManager1" runat="server" ClientInstanceName="fileManager">
                                        <Settings AllowedFileExtensions=".jpg,.jpeg,.gif,.rtf,.txt,.avi,.png,.mp3,.xml,.doc,.pdf" EnableMultiSelect="false" />
                                        <SettingsEditing AllowCreate="true" AllowDelete="true" AllowMove="true" AllowRename="true" AllowCopy="true" AllowDownload="true" />
                                        <SettingsToolbar ShowDownloadButton="true" />
                                        <SettingsFileList View="Thumbnails">
                                            <ThumbnailsViewSettings ThumbnailSize="100" />
                                            <DetailsViewSettings AllowColumnResize="true" AllowColumnDragDrop="true" AllowColumnSort="true" ShowHeaderFilterButton="false">
                                                <Columns>
                                                    <dx:FileManagerDetailsColumn FileInfoType="Thumbnail" />
                                                    <dx:FileManagerDetailsColumn FileInfoType="FileName" />
                                                    <dx:FileManagerDetailsColumn FileInfoType="LastWriteTime" />
                                                    <dx:FileManagerDetailsColumn FileInfoType="Size" />
                                                </Columns>
                                            </DetailsViewSettings>
                                        </SettingsFileList>
                                        <SettingsUpload Enabled="true" />
                                          <ClientSideEvents SelectedFileOpened="function(s, e) {
    // Check if e.file and e.file.extension are defined
                                  debugger;
    if (e.file.name) {
       
        var fileExtension = e.file.name.substring(e.file.name.lastIndexOf('.') + 1);

        // Check if the file is an image (you can customize the list of allowed extensions)
        var isImage = ['jpg', 'jpeg', 'png', 'gif'].includes(fileExtension);


        if (!isImage) {
            alert('Invalid file type. Only images are allowed.');
            return false;
        }
    } else {
        // Handle the case where e.file or e.file.extension is undefined
        alert('Unable to determine file type.');
        return false;
    }

    var image = $('#imgProduct')[0];
    image.src = e.file.imageSrc;

    var subproduct = s.currentPath;
    if (subproduct != '') {
        subproduct = '/' + subproduct;
        subproduct = subproduct.replaceAll('\\', '/');
    }

    var dataa = '~/' + s.rootFolderName + subproduct + '/' + e.file.name;
    dataa = dataa.replaceAll('/', '//');

    // Call the server-side method only if it's an image
    PageMethods.ASPxFileManager1_SelectedFileOpened(dataa, e.file.name, function(data) {}, function(data) {});

    popup.Hide();
    return false;
}" />
                                    </dx:ASPxFileManager>

                                </dx:PopupControlContentControl>
                            </ContentCollection>
                        </dx:ASPxPopupControl>




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
				<asp:Label ID="lblbinTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlbinFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Model Name %>" Value="Model_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Model Name(Local) %>" Value="Model_Name_L"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Model No %>" Value="Model_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Created By %>" Value="Created_User"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Modified By %>" Value="Modified_User"></asp:ListItem>
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
                                                        <asp:TextBox ID="txtbinValue" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="btnbinbind" runat="server" CausesValidation="False" OnClick="btnbinbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnbinRefresh" runat="server" CausesValidation="False" OnClick="btnbinRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imgBtnRestore" CausesValidation="False" runat="server" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="ImgbtnSelectAll" Visible="false" runat="server" OnClick="ImgbtnSelectAll_Click" ToolTip="<%$ Resources:Attendance, Select All %>"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>
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
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvModelMasterBin" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="gvModelMasterBin_PageIndexChanging"
                                                        OnSorting="gvModelMasterBin_OnSorting" AllowSorting="true">
                                                        <Columns>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkgvSelect" runat="server" OnCheckedChanged="chkgvSelect_CheckedChanged"
                                                                        AutoPostBack="true" />
                                                                </ItemTemplate>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChanged"
                                                                        AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Model No %>" SortExpression="Model_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModelNo" runat="server" Text='<%# Eval("Model_No") %>'></asp:Label>
                                                                    <asp:Label ID="lblModelId" runat="server" Visible="false" Text='<%# Eval("Trans_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Model Name %>" SortExpression="Model_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModelName" runat="server" Text='<%# Eval("Model_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Model Name(Local) %>" SortExpression="Model_Name_L">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblModelNameL" runat="server" Text='<%# Eval("Model_Name_L") %>'></asp:Label>
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
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }

        function View_Modal_Popup() {
            document.getElementById('<%= Btn_View_Modal.ClientID %>').click();
        }
        function Close_Modal_Popup() {
            document.getElementById('<%= Btn_View_Modal.ClientID %>').click();
        }
    </script>
    <script language="javascript" type="text/javascript">
        function divexpandcollapse(divname) {
            var div = document.getElementById(divname);
            var img = document.getElementById('img' + divname);
            if (div.style.display == "none") {
                div.style.display = "inline";
                img.src = "../Images/minus.png";
            } else {
                div.style.display = "none";
                img.src = "../Images/plus.png";
            }
        }
    </script>
    <script type="text/javascript">
        function FuLogo_UploadComplete(sender, args) {
            document.getElementById('<%= FULogo_Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= FULogo_Img_Right.ClientID %>').style.display = "";
        }
        function FuLogo_UploadError(sender, args) {
            document.getElementById('<%= FULogo_Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= FULogo_Img_Wrong.ClientID %>').style.display = "";
            var img = document.getElementById('<%= imgProduct.ClientID %>');
            img.src = "../Bootstrap_Files/dist/img/NoImage.jpg";
            alert('Invalid File Type, Select Only .png, .jpg, .jpge extension file');
        }
        function FuLogo_UploadStarted(sender, args) {
            var filename = args.get_fileName();

            var filext = filename.substring(filename.lastIndexOf(".") + 1);
            filext = filext.toLowerCase();
            if (filext == "png" || filext == "jpg" || filext == "jpge") {
                return true;
            }
            else {
                throw {
                    name: "Invalid File Type",
                    level: "Error",
                    message: "Invalid File Type, Select Only .png, .jpg, .jpge extension file",
                    htmlMessage: "Invalid File Type, Select Only .png, .jpg, .jpge extension file"
                }
                return false;
            }
        }


        function Modal_Open_FileUpload() {
            document.getElementById('<%= Btn_Modal_FileUpload.ClientID %>').click();
        }


        function FUAll_UploadStarted(sender, args) {

        }
    </script>
</asp:Content>
