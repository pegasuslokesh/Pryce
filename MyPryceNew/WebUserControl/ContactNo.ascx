<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ContactNo.ascx.cs" Inherits="WebUserControl_ContactNo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<div class="col-md-2">
    <asp:Label ID="lblType" runat="server" Text="Contact Type"></asp:Label>
    <asp:DropDownList ID="ddlContactType" runat="server" CssClass="form-control" onchange="validation1();">
        <asp:ListItem Text="Mobile"></asp:ListItem>
        <asp:ListItem Text="Work"></asp:ListItem>
        <asp:ListItem Text="LandLine"></asp:ListItem>
        <asp:ListItem Text="Home"></asp:ListItem>
        <asp:ListItem Text="Work Fax"></asp:ListItem>
        <asp:ListItem Text="Home Fax"></asp:ListItem>
        <asp:ListItem Text="Other"></asp:ListItem>
    </asp:DropDownList>
    <br />
</div>


<div class="col-md-2">
    <asp:Label ID="lblCountryCode" runat="server" Text="Country Code"></asp:Label>
    <asp:TextBox ID="txtCountryCode" runat="server" CssClass="form-control" Enabled="false">
    </asp:TextBox>

    <br />
</div>

<div class="col-md-4">
    <asp:Label ID="Label1" runat="server" Text="Number"></asp:Label>
    <asp:TextBox ID="txtNumber" runat="server" CssClass="form-control"></asp:TextBox>
    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" TargetControlID="txtNumber" FilterType="Numbers" FilterMode="ValidChars"></cc1:FilteredTextBoxExtender>
    <cc1:AutoCompleteExtender ID="AutoCompleteExtender9" runat="server" DelimiterCharacters=""
        Enabled="True" ServiceMethod="GetCompletionListContactNumber" ServicePath="" CompletionInterval="100"
        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtNumber"
        UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition1">
    </cc1:AutoCompleteExtender>
    <br />
</div>

<div class="col-md-2">
    <asp:Label ID="lblExtensionNumber" runat="server" Text="Extension No."></asp:Label>
    <asp:TextBox ID="txtExtensionNumber" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" TargetControlID="txtExtensionNumber" FilterType="Numbers" FilterMode="ValidChars"></cc1:FilteredTextBoxExtender>

    <br />
</div>

<div class="col-md-2">
    <br />
    <asp:Button ID="btnAddNum" runat="server" Text="Add Number" CssClass="btn btn-primary" OnClick="btnAddNum_Click" />
    <br />
</div>

<div class="row">
    <div class="col-md-12">
        <div id="Div_Add_Num" runat="server" class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">Added Number List</h3>
                <div class="box-tools pull-right">
                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                        <i id="Btn_Div_Add_Num" runat="server" class="fa fa-minus"></i>
                    </button>
                </div>
            </div>
            <div class="box-body">
                <div class="form-group">

                    <div class="col-md-12">
                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvNumber" runat="server" AutoGenerateColumns="False" OnRowDeleting="GvNumber_RowDeleting">
                            <Columns>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="ibtnDeleteNum" runat="server" ToolTip="Delete" CommandName="Delete" CommandArgument='<%#Eval("Phone_no") %>'><i class="fa fa-remove"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Type" HeaderStyle-Width="200px">
                                    <ItemTemplate>
                                        <asp:Label ID="lbltype" runat="server" Text='<%#Eval("Type") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Code" HeaderStyle-Width="200px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCode" runat="server" Text='<%#Eval("Country_code") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Number" HeaderStyle-Width="200px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNum" runat="server" Text='<%#Eval("Phone_no") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Is Default" HeaderStyle-Width="200px">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ChkDefault" runat="server" Checked='<%#Eval("Is_default") %>' OnCheckedChanged="ChkDefault_CheckedChanged" AutoPostBack="true"></asp:CheckBox>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Extension" HeaderStyle-Width="200px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblExtension" runat="server" Text='<%#Eval("Extension_no") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            
                            <HeaderStyle HorizontalAlign="Center" />

                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>


   