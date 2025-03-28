<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="ProductTaxMaster.aspx.cs" Inherits="SystemSetUp_ProductMasterTax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <style>
        .grid td, .grid th {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-percent"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="Product Tax Master"></asp:Label></h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,System SetUp%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,System SetUp%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="Product Tax Master"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_View_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#myModal" Text="View Modal" />
            <asp:Button ID="Btn_GST_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Modal_GST" Text="GST" />
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
                <div class="tab-content">
                    <div class="tab-pane active" id="New">
                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="col-md-6">
                                                                <asp:HiddenField ID="Hdn_Category_Trans_ID" runat="server" />
                                                                <asp:HiddenField ID="Hdn_Category_Name" runat="server" />
                                                                <asp:Label ID="lblProductCategory" runat="server" Text="Product Category" />
                                                                <asp:TextBox ID="txtProductCategory" runat="server" CssClass="form-control" BackColor="#eeeeee" AutoPostBack="true" OnTextChanged="txtProductCategory_TextChanged" />
                                                                <cc1:AutoCompleteExtender ID="ACE_ProductCategory" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="GetProductCategoryList" ServicePath="" CompletionInterval="100"
                                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtProductCategory" UseContextKey="True"
                                                                    CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                </cc1:AutoCompleteExtender>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="lblProductName" runat="server" Text="Product Name" />
                                                                <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" BackColor="#eeeeee" AutoPostBack="true"  OnTextChanged="txtProductName_TextChanged"/>
                                                                <cc1:AutoCompleteExtender ID="ACE_ProductName" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetProductNameList" ServicePath="" CompletionInterval="100"
                                                                MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtProductName" UseContextKey="True"
                                                                CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                            </cc1:AutoCompleteExtender>                                                                
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12">
                                                            <div class="col-md-6">
                                                                <br />
                                                                <asp:Label ID="lblHSNCode" runat="server" Text="HSN Code" />
                                                                <asp:TextBox ID="txtHSNCode" runat="server" CssClass="form-control" BackColor="#eeeeee" AutoPostBack="true" OnTextChanged="txtHSNCode_TextChanged" />
                                                                <cc1:AutoCompleteExtender ID="ACE_HSNCode" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="GetHSNCodeList" ServicePath="" CompletionInterval="100"
                                                                    MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtHSNCode" UseContextKey="True"
                                                                    CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                </cc1:AutoCompleteExtender>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <br />
                                                                <asp:Label ID="Lbl_Location" runat="server" Text="<%$ Resources:Attendance,Location%>"/>
                                                                <asp:DropDownList ID="DDL_Location" runat="server" CssClass="form-control"></asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12">
                                                            <div class="col-md-6">
                                                                <br />
                                                                <asp:Label ID="lblTaxType" runat="server" Text="Tax Type" />
                                                                <asp:DropDownList ID="ddlTaxType" runat="server" CssClass="form-control"></asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <br />
                                                                <asp:Label ID="lblTaxValue" runat="server" Text="Tax Value" />
                                                                <asp:TextBox ID="txtTaxValue" runat="server" CssClass="form-control" MaxLength="5" />
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" Enabled="True"
                                                                    TargetControlID="txtTaxValue" FilterType="Custom" ValidChars="1234567890.">
                                                                </cc1:FilteredTextBoxExtender>
                                                                <asp:RangeValidator ID="RVTaxValue1" runat="server"
                                                                    ControlToValidate="txtTaxValue" Display="Dynamic"
                                                                    ErrorMessage="Invalid Percentage" MaximumValue="99.99" MinimumValue="0.00"
                                                                    Type="Double"></asp:RangeValidator>
                                                            </div>
                                                             <div class="col-md-6">
                                                                <br />
                                                                <asp:CheckBox ID="chkApply" Text ="Apply on All Location in Same Country" runat="server"  />
                                                                </div>
                                                        </div>
                                                        <div class="col-md-12">
                                                            <br />
                                                            <div class="col-md-12" style="text-align: center">
                                                                <asp:Button ID="btnExecute" runat="server" Text="Execute" CssClass="btn btn-primary" OnClick="btnExecute_Click" />
                                                                
                                                                <asp:Button ID="btnSave" Visible="false" runat="server" Text="Save" CssClass="btn btn-success" OnClick="btnSave_Click" />
                                                                
                                                                <asp:Button ID="btnDelete" Visible="false" runat="server" Text="Delete" CssClass="btn btn-danger" OnClick="btnDelete_Click" />
                                                                
                                                               <asp:Button ID="btnCancel" runat="server" Text="Reset" CssClass="btn btn-primary" OnClick="btnCancel_Click" />
                                                                  <asp:Button ID="btnApplyDefault" runat="server" Text="Apply Default" CssClass="btn btn-primary" OnClick="btnApplyDefault_Click" />
                                                                <br />
                                                            </div>
                                                        </div>


                                                        <div class="col-md-12">
                                                            <div class="flow">
                                                                <br />
                                                                <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvTax" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server" DataKeyNames="Product_Id"
                                                                    AutoGenerateColumns="False" AllowPaging="false" AllowSorting="True" Width="100%"  OnPageIndexChanging="gvTax_PageIndexChanging" OnSorting="gvTax_Sorting">
                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkgvSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkgvSelect_CheckedChanged" />
                                                                            </ItemTemplate>
                                                                            <HeaderTemplate>
                                                                                <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChanged"
                                                                                    AutoPostBack="true" />
                                                                            </HeaderTemplate>
                                                                            <ItemStyle  HorizontalAlign="Center" Width="5%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField Visible="false" HeaderText="Edit">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Product_Id") %>' OnCommand="btnEdit_Command" CausesValidation="False" ToolTip="<%$ Resources:Attendance,Edit %>"><i class="fa fa-pencil"></i></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <ItemStyle  HorizontalAlign="Center" Width="5%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField Visible="false" HeaderText="Detail">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="btnDetail" runat="server" CommandArgument='<%# Eval("Product_Id") %>' OnCommand="btnDetail_Command" CausesValidation="False" ToolTip="Show Detail" ><i class="fa fa-desktop"></i></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <ItemStyle  HorizontalAlign="Center" Width="5%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Product Name" SortExpression="ProductName">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblPName" runat="server" Text='<%# Eval("ProductName") %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle  HorizontalAlign="Center" Width="30%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Product Id" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblPId" runat="server" Text='<%# Eval("Product_Id") %>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Product Category" SortExpression="Category_Name">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="Lbl_Category_Name" runat="server" Text='<%# Eval("Category_Name") %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle  HorizontalAlign="Center" Width="30%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="HSN Code" SortExpression="HScode">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="Lbl_HScode" runat="server" Text='<%# Eval("HScode") %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle  HorizontalAlign="Center" Width="30%" />
                                                                        </asp:TemplateField>
                                                                           <asp:TemplateField HeaderText="Tax Name" SortExpression="Tax_Name">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="Lbl_Tax_Name" runat="server" Text='<%# Eval("Tax_Name") %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle  HorizontalAlign="Center" Width="30%" />
                                                                        </asp:TemplateField>
                                                                           <asp:TemplateField HeaderText="Tax Percentage" SortExpression="Tax_Percentage">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="Lbl_Tax_Percentage" runat="server" Text='<%# Eval("Tax_Percentage") %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle  HorizontalAlign="Center" Width="30%" />
                                                                        </asp:TemplateField>

                                                                  <%--      <asp:TemplateField HeaderText="Tax Detail" SortExpression="tax_detail">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="Lbl_tax_detail" runat="server" Text='<%# Eval("tax_detail") %>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle  HorizontalAlign="Center" Width="30%" />
                                                                        </asp:TemplateField>--%>
                                                                    </Columns>
                                                                    
                                                                    <PagerStyle CssClass="pagination-ys" />
                                                                    
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                </div>
                                                <asp:HiddenField ID="editid" runat="server" />
                                                <asp:HiddenField ID="hdnProductId" runat="server" />
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

    <div class="modal fade" id="Modal_GST" tabindex="-1" role="dialog" aria-labelledby="Modal_GSTLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Modal_GSTLabel">Tax Details
                    </h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Modal_GST" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    
                                    <div class="col-md-12" style="text-align: center;">
                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvTaxCalculation" runat="server" AutoGenerateColumns="false"  Width="100%">
                                            <Columns>
                                                <asp:TemplateField Visible="false" HeaderText="<%$ Resources:Attendance,Delete %>">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                            ImageUrl="~/Images/Erase.png" OnCommand="btnDelete_Command" Width="16px" ToolTip="<%$ Resources:Attendance,Delete %>" />
                                                        <cc1:ConfirmButtonExtender ID="confirm2" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>"
                                                            TargetControlID="btnDelete">
                                                        </cc1:ConfirmButtonExtender>
                                                    </ItemTemplate>                                                    
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle  />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Tax Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxName" runat="server" Text='<%# Eval("Tax_Name") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle  HorizontalAlign="Center" Width="50%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Tax Value (%)">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtTaxValue" CssClass="form-control" style="text-align:center" Width="100%" runat="server" Text='<%# Eval("Tax_Value") %>' OnTextChanged="txtTaxValue_TextChanged" AutoPostBack="true" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" Enabled="True"
                                                            TargetControlID="txtTaxValue" FilterType="Custom" ValidChars="1234567890.">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle  HorizontalAlign="Center" Width="50%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Trans Id" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTrans_Id" runat="server" Text='<%# Eval("Trans_Id") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Product Id" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProduct_Id" runat="server" Text='<%# Eval("Product_Id") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Tax Id" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTax_Id" runat="server" Text='<%# Eval("Tax_Id") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            
                                            
                                            
                                        </asp:GridView>
                                    </div>
                                    
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="Update_Modal_Button_GST" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnSaveGST" runat="server" Visible="false" CausesValidation="False" CssClass="btn btn-success" OnClick="btnSaveGST_Click"
                                Text="<%$ Resources:Attendance,Save %>" />

                            <asp:HiddenField ID="hdnEmpIdGST" runat="server" />

                            <button type="button" class="btn btn-danger" data-dismiss="modal" id="btnClosePopup">Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Modal_GST">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_Modal_Button_GST">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_List">
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
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server"><script src="../Script/common.js"></script>
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
        function Show_Modal_GST() {
            document.getElementById('<%= Btn_GST_Modal.ClientID %>').click();
        }
        function Hide_Model_GST() {
            $('#btnClosePopup').click();
        }
        <%--function SetContextKey() {
        $find('<%=ACE_ProductName.ClientID%>').set_contextKey($get("<%=Hdn_Category_Trans_ID.ClientID %>").value);
        }--%>
    </script>
</asp:Content>

