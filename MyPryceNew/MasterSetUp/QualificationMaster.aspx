﻿<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="QualificationMaster.aspx.cs" Inherits="MasterSetUp_QualificationMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/qualification.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Qualification Setup%>"></asp:Label>

        <%--<small>Control panel</small>--%>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,System SetUp%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,System SetUp%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Qualification Setup%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>

            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
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
                    <li><a href="#Bin" onclick="Li_Tab_Bin()" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
                    <li class="active" id="Li_New"><a href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                </ul>
                <div class="tab-content">


                    <div class="tab-pane active" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">


                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblQualificationName" runat="server" Text="<%$ Resources:Attendance,Qualification Name%>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtQualificationName" ErrorMessage="<%$ Resources:Attendance,Enter Qualification Name %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtQualificationName" runat="server" AutoPostBack="true" OnTextChanged="txtQualificationName_TextChanged" BackColor="#eeeeee" CssClass="form-control" />
                                                        <cc1:AutoCompleteExtender ID="autoComplete1" runat="server"
                                                            DelimiterCharacters="" Enabled="True" ServiceMethod="GetCompletionListQualification" ServicePath=""
                                                            CompletionInterval="100" MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtQualificationName"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblQualificationNameL" runat="server" Text="<%$ Resources:Attendance,Qualification Name(Local)%>"></asp:Label>
                                                        <asp:TextBox ID="txtQualificationNameL" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">

                                                        <asp:Button ID="btnCSave" ValidationGroup="Save" runat="server" Text="<%$ Resources:Attendance,Save %>" Visible="false" CssClass="btn btn-success" OnClick="btnCSave_Click" />
                                                        <asp:Button ID="BtnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>" CssClass="btn btn-primary" CausesValidation="False" OnClick="BtnReset_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <asp:HiddenField ID="editid" runat="server" />


                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div1" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				<asp:Label ID="lblTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Qualification Name %>" Value="Qualification" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Qualification Name(Local) %>" Value="Qualification_L" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Created By %>" Value="Created_User" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Modified By %>" Value="Modified_User" />
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
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbindrpt_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefreshReport_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div class="box box-warning box-solid">
                                    <div class="box-body">
                                        <div class="row">

                                            <div class="col-md-12">
                                                <br />
                                                <div class="flow">

                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvQualification" PageSize="<%# PageControlCommon.GetPageSize() %>" runat="server" AutoGenerateColumns="False"
                                                        Width="100%" AllowPaging="True" OnPageIndexChanging="GvQualification_PageIndexChanging"
                                                        AllowSorting="True" OnSorting="GvQualification_Sorting">
                                                        <Columns>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Action%>">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle"  type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Qualification_Id") %>' OnCommand="btnEdit_Command" CausesValidation="False" ToolTip="<%$ Resources:Attendance,Edit %>"><i class="fa fa-pencil"></i><%# Resources.Attendance.Edit%> </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Qualification_ID") %>' OnCommand="IbtnDelete_Command" Width="16px" ToolTip="<%$ Resources:Attendance,Delete %>"><i class="fa fa-trash"></i><%# Resources.Attendance.Delete%></asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm2" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>
                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Qualification Id %>" SortExpression="Qualification_Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDesigId1" runat="server" Text='<%# Eval("Qualification_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Qualification" HeaderText="<%$ Resources:Attendance,Qualification Name %>"
                                                                SortExpression="Qualification" />
                                                            <asp:BoundField DataField="Qualification_L" HeaderText="<%$ Resources:Attendance,Qualification Name(Local) %>"
                                                                SortExpression="Qualification_L" />
                                                            <asp:BoundField DataField="Created_User" HeaderText="<%$ Resources:Attendance,Created By %>"
                                                                SortExpression="Created_User" />
                                                            <asp:BoundField DataField="Modified_User" HeaderText="<%$ Resources:Attendance,Modified By %>"
                                                                SortExpression="Modified_User" />
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
                    <!-- /.tab-pane -->
                    <div class="tab-pane" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server">

                            <ContentTemplate>


                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div2" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Attendance,Advance Search%>"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
                                                <asp:Label ID="lblTotalRecordsBin" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                                                <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldNameBin" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Qualification Name%>" Value="Qualification" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Qualification Name(Local) %>" Value="Qualification_L" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Qualification Id %>" Value="Qualification_Id"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Created By %>" Value="Created_User" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Modified By %>" Value="Modified_User" />
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
                                                <div class="col-lg-4">
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbindBin">
                                                        <asp:TextBox placeholder="Search From Content" ID="txtValueBin" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="btnbindBin"  runat="server" CausesValidation="False"  OnClick="btnbindBin_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefreshBin"  runat="server" CausesValidation="False"  OnClick="btnRefreshBin_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imgBtnRestore" CausesValidation="False" Visible="false" runat="server" OnClick="btnRestoreSelected_Click" ToolTip="<%$ Resources:Attendance, Active %>" ><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
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
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvQualificationBin" PageSize="<%# PageControlCommon.GetPageSize() %>" runat="server" AutoGenerateColumns="False"
                                                        Width="100%" AllowPaging="True" OnPageIndexChanging="GvQualificationBin_PageIndexChanging"
                                                        OnSorting="GvQualificationBin_OnSorting" DataKeyNames="Qualification_Id" AllowSorting="true">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAll_CheckedChanged"
                                                                        AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Qualification Id %>" SortExpression="Qualification_Id">
                                                                <ItemTemplate>

                                                                    <asp:Label ID="lblDesigId1" runat="server" Text='<%# Eval("Qualification_Id") %>'></asp:Label>


                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Qualification Name %>" SortExpression="Qualification">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvQualificationName" runat="server" Text='<%# Eval("Qualification") %>'></asp:Label>
                                                                    <asp:Label ID="lblQualiId" Visible="false" runat="server" Text='<%# Eval("Qualification_Id") %>'></asp:Label>


                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Qualification Name(Local) %>" SortExpression="Qualification_L">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvQualificationNameL" runat="server" Text='<%# Eval("Qualification_L") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Created By %>" SortExpression="Created_User">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCreated_User" runat="server" Text='<%# Eval("Created_User") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Modified By %>" SortExpression="Modified_User">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCreated_User1" runat="server" Text='<%# Eval("Modified_User") %>'></asp:Label>
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
                                    <!-- /.box-body -->
                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                </div>
                <!-- /.tab-content -->
            </div>

        </div>
    </div>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_New">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_Bin">
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

        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }
    </script>
</asp:Content>






