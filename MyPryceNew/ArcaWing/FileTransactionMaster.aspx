<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="FileTransactionMaster.aspx.cs" Inherits="ArcaWing_FileTransactionMaster" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-exchange-alt"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,FileTransaction Setup%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Archiving%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Archiving%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,FileTransaction Setup%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
            <asp:HiddenField runat="server" ID="hdnCanDownload" />
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
                            <Triggers>
                                <asp:PostBackTrigger ControlID="gvFileMaster" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <asp:UpdatePanel ID="Update_Radio_Button" runat="server">
                                                        <ContentTemplate>

                                                            <div class="col-md-2" style="text-align: center">
                                                                <asp:RadioButton ID="rbtnListUsergenerated" Font-Size="14px" Font-Bold="true"
                                                                    runat="server" GroupName="a" AutoPostBack="true" Checked="true" Text="<%$ Resources:Attendance,User Generated%>"
                                                                    OnCheckedChanged="rbtnListUsergenerated_OnCheckedChanged" />
                                                                <br />
                                                            </div>
                                                            <div class="col-md-2" style="text-align: center">
                                                                <asp:RadioButton ID="rbtnListSystemgenerated" Font-Size="14px" Font-Bold="true"
                                                                    runat="server" GroupName="a" AutoPostBack="true" Text="<%$ Resources:Attendance,System Generated%>"
                                                                    OnCheckedChanged="rbtnListUsergenerated_OnCheckedChanged" />
                                                            </div>

                                                            <div class="col-md-4"></div>
                                                            <div class="col-md-4">

                                                                <asp:Button ID="btnTreeView" runat="server" Text="<%$ Resources:Attendance,Tree View %>" OnClick="btnTreeView_Click" Visible="false"
                                                                    CssClass="btn btn-primary" />
                                                            </div>
                                                            <div id="panelSystemGeneratedType" runat="server" visible="false" class="col-md-6">
                                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Type %>"></asp:Label>
                                                                <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                    OnSelectedIndexChanged="ddlType_OnSelectedIndexChanged">
                                                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="Employee" Value="Employee"></asp:ListItem>
                                                                    <asp:ListItem Text="Product" Value="Product"></asp:ListItem>
                                                                    <asp:ListItem Text="Contact" Value="Contact"></asp:ListItem>
                                                                    <asp:ListItem Text="Customer" Value="Customer"></asp:ListItem>
                                                                    <asp:ListItem Text="Supplier" Value="Supplier"></asp:ListItem>
                                                                    <asp:ListItem Text="Project" Value="Project"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                <br />
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="panelFileGrid" runat="server" class="box box-warning box-solid">
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                    <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                                    <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server" GridViewID="GvFollowupData"></dx:ASPxGridViewExporter>
                                                    <dx:ASPxGridView ID="gvFileMaster1" EnableViewState="false" ClientInstanceName="gvFileMaster1" runat="server" AutoGenerateColumns="False" Width="100%">
                                                        <Columns>
                                                            <dx:GridViewDataColumn Name="Download" VisibleIndex="0" Visible="false">
                                                                <DataItemTemplate>
                                                                    <asp:UpdatePanel ID="up_download" runat="server">
                                                                        <Triggers>
                                                                            <asp:PostBackTrigger ControlID="ibtn" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                    <asp:LinkButton ToolTip="Download" ID="ibtn" runat="server" OnCommand="Unnamed_Command" CommandArgument='<%# Eval("Trans_Id") %>'><i class="fa fa-download" style="font-size:15px"></i></asp:LinkButton>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>

                                                            <dx:GridViewDataColumn VisibleIndex="1" Visible="false">
                                                                <DataItemTemplate>
                                                                    <asp:LinkButton ToolTip="Delete" ID="ibtnDelete" runat="server" OnCommand="IbtnDelete_Command" CommandArgument='<%# Eval("Trans_Id") %>'><i class="fa fa-trash" style="font-size:15px"></i></asp:LinkButton>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataColumn>
                                                            <dx:GridViewDataTextColumn FieldName="Directory_Name" VisibleIndex="2">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="Document_Name" VisibleIndex="3">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="File_Name" VisibleIndex="4">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="File_Upload_Date" VisibleIndex="5">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="File_Expiry_Date" VisibleIndex="6">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="userName" Caption="User Name" VisibleIndex="7">
                                                            </dx:GridViewDataTextColumn>
                                                        </Columns>
                                                        <Settings ShowGroupPanel="true" ShowFilterRow="true" />

                                                    </dx:ASPxGridView>

                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" Visible="false" ID="gvFileMaster" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server"
                                                        AutoGenerateColumns="False" Width="100%" DataKeyNames="Trans_Id" AllowPaging="True"
                                                        AllowSorting="True" OnPageIndexChanging="gvFileMaster_PageIndexChanging"
                                                        OnSorting="gvFileMaster_OnSorting">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li <%= hdnCanDownload.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="ImgDownload" runat="server" CommandArgument='<%#Eval("Trans_id") %>' OnCommand="OnDownloadCommand"><i class="fa fa-download"></i>Download</asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' CausesValidation="False" OnCommand="btnEdit_Command"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>
                                                                        </ul>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,User Id%>" SortExpression="CreatedBy">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblfileId1" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Directory For%>" SortExpression="Directory_For">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDirectoryFor" runat="server" Text='<%# Eval("Directory_For") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Directory Name %>" SortExpression="Directory_name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblfileId2" runat="server" Text='<%# Eval("Directory_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Document Name %>" SortExpression="Document_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblfileId3" runat="server" Text='<%# Eval("Document_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,File Name %>" SortExpression="File_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFileName" runat="server" Text='<%# Eval("File_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Expiry Date %>" SortExpression="File_Expiry_Date">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtfileexpirydate" runat="server" Text='<%#GetDate(Eval("File_Expiry_Date").ToString()) %>'
                                                                        Width="90px"></asp:TextBox>
                                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtCalenderExtender_FileExpire" runat="server" TargetControlID="txtfileexpirydate"
                                                                        Format="dd-MMM-yyyy">
                                                                    </cc1:CalendarExtender>
                                                                    <asp:ImageButton ID="imgupdate" Visible="false" runat="server" ImageUrl="~/Images/Allow.png"
                                                                        CommandArgument='<%#Eval("Trans_id") %>' OnCommand="OnUpdateCommand" ToolTip="Update Expiry Date" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="140px" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>


                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="pnlEmployee" visible="false" runat="server">

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div id="Div2" runat="server" class="box box-info collapsed-box">
                                                <div class="box-header with-border">
                                                    <h3 class="box-title">
                                                        <asp:Label ID="Label7" runat="server" Text="Advance Search"></asp:Label></h3>
                                                    &nbsp;&nbsp;|&nbsp;&nbsp;
					    <asp:Label ID="lblTotalRecordsEmployee" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                                                    <asp:Label ID="lblSelectedRecordEmployee" runat="server" Visible="false"></asp:Label>

                                                    <div class="box-tools pull-right">
                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                            <i id="I2" runat="server" class="fa fa-plus"></i>
                                                        </button>
                                                    </div>
                                                </div>
                                                <div class="box-body">
                                                    <div class="col-lg-3">
                                                        <asp:DropDownList ID="ddlFieldNameEmployee" runat="server" CssClass="form-control">
                                                            <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Employee Code %>" Value="Emp_Code"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Employee Name %>" Value="Emp_Name"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-lg-2">
                                                        <asp:DropDownList ID="ddlOptionEmployee" runat="server" CssClass="form-control">
                                                            <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                            <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-lg-3">
                                                        <asp:Panel ID="Panel2" runat="server" DefaultButton="btnBindEmployee">
                                                            <asp:TextBox ID="txtvalueEmployee" runat="server" CssClass="form-control" />
                                                        </asp:Panel>

                                                    </div>
                                                    <div class="col-lg-2">
                                                        <asp:LinkButton ID="btnBindEmployee" runat="server" CausesValidation="False" OnClick="btnBindEmployee_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                        <asp:LinkButton ID="btnRefreshEmployee" runat="server" CausesValidation="False" OnClick="btnRefreshEmployee_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                        <asp:LinkButton ID="ImageButton1" runat="server" OnClick="ImgbtnSelectAllEmployee_Click" ToolTip="<%$ Resources:Attendance, Select All %>"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>
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
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvEmp" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                            OnPageIndexChanging="gvEmp_PageIndexChanging" AllowSorting="true" OnSorting="gvEmp_Sorting"
                                                            Width="100%" DataKeyNames="Emp_Id" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'>
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                                    </ItemTemplate>
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAllEmployee_CheckedChanged"
                                                                            AutoPostBack="true" />
                                                                    </HeaderTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Code %>" SortExpression="Emp_Code">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                                                        <asp:Label ID="lblEmpId" Visible="false" runat="server" Text='<%# Eval("Emp_Id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="Emp_Name" HeaderText="<%$ Resources:Attendance,Employee Name %>"
                                                                    SortExpression="Emp_Name" ItemStyle-Width="40%" />
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Email Id %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("Email_Id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Phone No. %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblItemType" runat="server" Text='<%# Eval("Phone_No") %>'></asp:Label>
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
                                <div id="panelProduct" visible="false" runat="server">

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div id="Div1" runat="server" class="box box-info collapsed-box">
                                                <div class="box-header with-border">
                                                    <h3 class="box-title">
                                                        <asp:Label ID="Label22" runat="server" Text="Advance Search"></asp:Label></h3>
                                                    &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecordProduct" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                    <div class="box-tools pull-right">
                                                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                            <i id="I1" runat="server" class="fa fa-plus"></i>
                                                        </button>
                                                    </div>
                                                </div>
                                                <div class="box-body">
                                                    <div class="col-lg-3">
                                                        <asp:DropDownList ID="ddlFieldNameProduct" runat="server" CssClass="form-control">
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Product Id %>" Value="ProductId"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Product Name %>" Value="EProductName" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Model No. %>" Value="ModelNo"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Item Type %>" Value="ItemType"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-lg-2">
                                                        <asp:DropDownList ID="ddlOptionproduct" runat="server" CssClass="form-control">
                                                            <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                            <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-lg-4">
                                                        <asp:Panel ID="Panel3" runat="server" DefaultButton="btnBindProduct">
                                                            <asp:TextBox placeholder="Search from Content" ID="txtValueProduct" runat="server" CssClass="form-control" />
                                                        </asp:Panel>

                                                    </div>
                                                    <div class="col-lg-3">
                                                        <asp:LinkButton ID="btnBindProduct" runat="server" CausesValidation="False" OnClick="btnBindProduct_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                        <asp:LinkButton ID="btnRefreshProduct" runat="server" CausesValidation="False" OnClick="btnRefreshProduct_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                        <asp:LinkButton ID="ImageButton5" runat="server" OnClick="ImgbtnSelectAllProduct_Click" ToolTip="<%$ Resources:Attendance, Select All %>"><span class="fas fa-th"  style="font-size:25px;"></span></asp:LinkButton>
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
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvProduct" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                            OnPageIndexChanging="gvProduct_PageIndexChanging" Width="100%"
                                                            AllowSorting="true" OnSorting="gvProduct_Sorting" DataKeyNames="ProductId" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'>
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                                    </ItemTemplate>
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkgvSelectAllProduct_CheckedChanged"
                                                                            AutoPostBack="true" />
                                                                    </HeaderTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id %>" SortExpression="ProductId">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblProductId" runat="server" Text='<%# Eval("ProductId") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="EProductName" HeaderText="<%$ Resources:Attendance,Product Name %>"
                                                                    SortExpression="EProductName" />
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Model No %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("ModelNo") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Item Type %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblItemType" runat="server" Text='<%# GetItemType(Eval("ItemType").ToString()) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance, Product Unit %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPUnit" runat="server" Text='<%#  GetUnitName(Eval("UnitId").ToString()) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <PagerStyle CssClass="pagination-ys" />
                                                        </asp:GridView>
                                                        <asp:Label ID="lblSelectedRecordProduct" runat="server" Visible="false"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="panelContact" visible="false" runat="server">
                                    <div class="alert alert-info ">
                                        <div class="row">
                                            <div class="form-group">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldNameContact" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Contact Id %>" Value="Codes"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Name %>" Value="Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Contact Type %>" Value="Status"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlOptionContact" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:Panel ID="Panel4" runat="server" DefaultButton="btnBindContact">
                                                        <asp:TextBox ID="txtvalueContact" runat="server" CssClass="form-control" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:ImageButton ID="btnBindContact" runat="server" CausesValidation="False" Style="margin-top: -5px;"
                                                        ImageUrl="~/Images/search.png" OnClick="btnBindContact_Click" ToolTip="<%$ Resources:Attendance,Search %>" />

                                                    <asp:ImageButton ID="btnRefreshContact" runat="server" CausesValidation="False" Style="width: 33px;"
                                                        ImageUrl="~/Images/refresh.png" OnClick="btnRefreshContact_Click"
                                                        ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>

                                                    <asp:ImageButton ID="ImageButton4" runat="server" OnClick="ImgbtnSelectAllContact_Click" Style="width: 33px;"
                                                        ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true" ImageUrl="~/Images/selectAll.png" />
                                                </div>
                                                <div class="col-lg-2">
                                                    <h5>
                                                        <asp:Label ID="lblTotalRecordContact" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>
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
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvContact" runat="server" DataKeyNames="Trans_Id" AllowPaging="True"
                                                            PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' AutoGenerateColumns="False" Width="100%"
                                                            AllowSorting="true" OnPageIndexChanging="GvContact_PageIndexChanging" OnSorting="GvContact_Sorting">

                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkActiveAll_CheckedChanged"
                                                                            AutoPostBack="true" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Contact Id %>" SortExpression="Code">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvContactCode" runat="server" Text='<%#Eval("Code") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Name %>" SortExpression="Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                                        <asp:Label ID="Label5" runat="server" Visible="false" Text='<%# Eval("Trans_Id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <%--<asp:TemplateField HeaderText="<%$ Resources:Attendance,Name (Local)%>" SortExpression="Name_L">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvCNameL" runat="server" Text='<%#Eval("Name_L") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle  />
                                                                </asp:TemplateField>--%>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Contact Type %>" SortExpression="Status">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                            </Columns>

                                                            <PagerStyle CssClass="pagination-ys" />

                                                        </asp:GridView>
                                                        <asp:Label ID="lblSelectedRecordContact" runat="server" Visible="false"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="panelCustomer" visible="false" runat="server">
                                    <div class="alert alert-info ">
                                        <div class="row">
                                            <div class="form-group">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldNameCustomer" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Customer Id %>" Value="Customer_Code"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Customer Name %>" Value="Name" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Customer Name(Local) %>" Value="Name_L"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlOptionCustomer" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:Panel ID="Panel5" runat="server" DefaultButton="btnBindCustomer">
                                                        <asp:TextBox ID="txtvalueCustomer" runat="server" CssClass="form-control" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:ImageButton ID="btnBindCustomer" runat="server" CausesValidation="False" Style="margin-top: -5px;"
                                                        ImageUrl="~/Images/search.png" OnClick="btnBindCustomer_Click" ToolTip="<%$ Resources:Attendance,Search %>" />

                                                    <asp:ImageButton ID="btnRefreshCustomer" runat="server" CausesValidation="False" Style="width: 33px;"
                                                        ImageUrl="~/Images/refresh.png" OnClick="btnRefreshCustomer_Click"
                                                        ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>

                                                    <asp:ImageButton ID="ImageButton6" runat="server" OnClick="ImgbtnSelectAllCustomer_Click" Style="width: 33px;"
                                                        ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true" ImageUrl="~/Images/selectAll.png" />
                                                </div>
                                                <div class="col-lg-2">
                                                    <h5>
                                                        <asp:Label ID="lblTotalRecordCustomer" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>
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
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvCustomer" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server"
                                                            AutoGenerateColumns="False" DataKeyNames="Customer_Id" Width="100%" AllowPaging="True"
                                                            OnPageIndexChanging="GvCustomer_PageIndexChanging" OnSorting="GvCustomer_OnSorting"
                                                            AllowSorting="true">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkCurrentCustomer_CheckedChanged"
                                                                            AutoPostBack="true" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Id%>" SortExpression="Code">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvCustomerCode" runat="server" Text='<%# Eval("Customer_Code") %>' />
                                                                        <asp:Label ID="lblgvCustomerId" runat="server" Visible="false" Text='<%# Eval("Customer_Id") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Name %>" SortExpression="Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvCustomerName" runat="server" Text='<%# Eval("Name") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Name(Local) %>"
                                                                    SortExpression="Name_L">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvLCustomerName" runat="server" Text='<%# Eval("Name_L") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Account No %>" SortExpression="Account_No">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvAccountNo" runat="server" Text='<%# Eval("Account_No") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Credit Limit %>" SortExpression="Credit_Limit">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvCreditLimit" runat="server" Text='<%# Eval("Credit_Limit") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                            </Columns>


                                                            <PagerStyle CssClass="pagination-ys" />

                                                        </asp:GridView>
                                                        <asp:Label ID="lblSelectedRecordCustomer" runat="server" Visible="false"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="panelSupplier" visible="false" runat="server">
                                    <div class="alert alert-info ">
                                        <div class="row">
                                            <div class="form-group">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldNameSupplier" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Supplier Id %>" Value="Supplier_Code"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Supplier Name %>" Value="Name" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Supplier Name(Local) %>" Value="Name_L"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlOptionSupplier" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:Panel ID="Panel6" runat="server" DefaultButton="btnBindSupplier">
                                                        <asp:TextBox ID="txtValueSupplier" runat="server" CssClass="form-control" />
                                                    </asp:Panel>

                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:ImageButton ID="btnBindSupplier" runat="server" CausesValidation="False" Style="margin-top: -5px;"
                                                        ImageUrl="~/Images/search.png" OnClick="btnBindSupplier_Click" ToolTip="<%$ Resources:Attendance,Search %>" />

                                                    <asp:ImageButton ID="btnRefreshSupplier" runat="server" CausesValidation="False" Style="width: 33px;"
                                                        ImageUrl="~/Images/refresh.png" OnClick="btnRefreshSupplier_Click"
                                                        ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>

                                                    <asp:ImageButton ID="ImageButton7" runat="server" OnClick="ImgbtnSelectAllSupplier_Click" Style="width: 33px;"
                                                        ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true" ImageUrl="~/Images/selectAll.png" />
                                                </div>
                                                <div class="col-lg-2">
                                                    <h5>
                                                        <asp:Label ID="lblTotalRecordSupplier" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>
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
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvSupplier" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server"
                                                            AutoGenerateColumns="False" DataKeyNames="Supplier_Id" Width="100%" AllowPaging="True"
                                                            OnPageIndexChanging="GvSupplier_PageIndexChanging" OnSorting="GvSupplier_OnSorting"
                                                            AllowSorting="true">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkCurrentSupplier_CheckedChanged"
                                                                            AutoPostBack="true" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Supplier Id%>" SortExpression="Supplier_Code">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvSupplierCode" runat="server" Text='<%# Eval("Supplier_Code") %>' />
                                                                        <asp:Label ID="lblgvSupplierId" Visible="false" runat="server" Text='<%# Eval("Supplier_Id") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Supplier Name %>" SortExpression="Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvSupplierName" runat="server" Text='<%# Eval("Name") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Supplier Name(Local) %>"
                                                                    SortExpression="Name_L">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvLCustomerName" runat="server" Text='<%# Eval("Name_L") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Account No %>" SortExpression="Account_No">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvAccountNo" runat="server" Text='<%# Eval("Account_No") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Purchase Limit %>" SortExpression="Purchase_Limit">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvPurchaseLimit" runat="server" Text='<%# Eval("Purchase_Limit") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                            </Columns>


                                                            <PagerStyle CssClass="pagination-ys" />

                                                        </asp:GridView>
                                                        <asp:Label ID="lblSelectedRecordSupplier" runat="server" Visible="false"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="pnlProject" visible="false" runat="server">
                                    <div class="alert alert-info ">
                                        <div class="row">
                                            <div class="form-group">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldNameProject" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="Project No." Value="Field7"
                                                            Selected="True" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Project Name %>" Value="Project_Name" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Project Type %>" Value="Project_Type" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Project Title %>" Value="Project_Title" />
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Customer Name %>" Value="Contact_Name" />
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlOptionProject" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:Panel ID="Panel7" runat="server" DefaultButton="btnBindProject">
                                                        <asp:TextBox ID="txtValueProject" runat="server" CssClass="form-control" />
                                                    </asp:Panel>

                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:ImageButton ID="btnBindProject" runat="server" CausesValidation="False" Style="margin-top: -5px;"
                                                        ImageUrl="~/Images/search.png" OnClick="btnBindProject_Click" ToolTip="<%$ Resources:Attendance,Search %>" />

                                                    <asp:ImageButton ID="btnRefreshProject" runat="server" CausesValidation="False" Style="width: 33px;"
                                                        ImageUrl="~/Images/refresh.png" OnClick="btnRefreshProject_Click"
                                                        ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>

                                                    <asp:ImageButton ID="ImgbtnSelectAllProject" runat="server" OnClick="ImgbtnSelectAllProject_Click" Style="width: 33px;"
                                                        ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true" ImageUrl="~/Images/selectAll.png" />
                                                </div>
                                                <div class="col-lg-2">
                                                    <h5>
                                                        <asp:Label ID="lblTotalRecordProject" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>
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
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvrProjectteam" runat="server" AutoGenerateColumns="False" Width="100%"
                                                            AllowPaging="True" OnPageIndexChanging="GvrProjectteam_PageIndexChanging" AllowSorting="True"
                                                            OnSorting="GvrProjectteam_Sorting" DataKeyNames="Project_Id">

                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="chkgvSelectAll" runat="server" OnCheckedChanged="chkCurrentProject_CheckedChanged"
                                                                            AutoPostBack="true" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkgvSelect" runat="server" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <%--  <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Project_Id") %>'
                                                            ImageUrl="~/Images/Erase.png" Visible="false" OnCommand="IbtnDelete_Command"
                                                            Width="16px" ToolTip="<%$ Resources:Attendance,Delete %>" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemStyle  />
                                                </asp:TemplateField>--%>

                                                                <asp:TemplateField HeaderText="Project No." SortExpression="Field7">
                                                                    <ItemTemplate>

                                                                        <asp:Label ID="lblprojectNo" runat="server" Text='<%# Eval("Field7") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Project Name %>" SortExpression="Project_Name">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="HiddeniD" runat="server" Value='<%# Eval("Project_Id") %>' />
                                                                        <asp:Label ID="lblprojectIdList" runat="server" Text='<%# Eval("Project_Name") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Project Name(Local)%>" SortExpression="Project_Name_L">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblprojectlname" runat="server" Text='<%# Eval("Project_Name_L") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Name %>" SortExpression="Contact_Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblcustname2" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Project Type%>" SortExpression="Project_Type">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblprojectype1" runat="server" Text='<%# Eval("Project_Type") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Start Date %>" SortExpression="Start_Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblcustname1" runat="server" Text='<%#Formatdate( Eval("Start_Date")) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Exp End Date %>" SortExpression="Exp_End_Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEmpnameList1" runat="server" Text='<%# Formatdate(Eval("Exp_End_Date")) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,End Date %>" SortExpression="End_date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEmpnameList2" runat="server" Text='<%#Formatdate( Eval("End_date")) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Project Title %>" SortExpression="Project_Title">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEmpnameList3" runat="server" Text='<%# Eval("Project_Title") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>

                                                            </Columns>

                                                            <PagerStyle CssClass="pagination-ys" />

                                                        </asp:GridView>
                                                        <asp:Label ID="lblSelectedRecordProject" runat="server" Visible="false"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="pnlGoButtton" visible="false" runat="server">
                                    <div class="row">
                                        <div class="col-md-12" style="text-align: center">

                                            <asp:Button ID="btnGo" runat="server" Style="margin-left: 15px; margin-right: 15px;" Text="Go" CssClass="btn btn-primary" ValidationGroup="a"
                                                OnClick="btnGo_OnClick" />

                                            <asp:Button ID="btnback" runat="server" Style="margin-left: 15px; margin-right: 15px;" Text="Back" CssClass="btn btn-primary" ValidationGroup="a"
                                                OnClick="btnback_OnClick" />

                                            <asp:CheckBox ID="chkGroupBy" Style="margin-left: 15px; margin-right: 15px;" runat="server" Text="Group By Document" />

                                            <asp:Button ID="btnReport" Style="margin-left: 15px; margin-right: 15px;" runat="server" CssClass="btn btn-primary" Text="<%$ Resources:Attendance, Generate Report %>"
                                                ValidationGroup="leavesave" OnClick="btnReport_Click" />

                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server">
                            <Triggers>
                                <%--    <asp:PostBackTrigger ControlID="btnSave" />--%>
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:RadioButton ID="rbtnUsergenerated" Font-Size="14px" Font-Bold="true"
                                                            runat="server" GroupName="b" AutoPostBack="true" Checked="true" Text="<%$ Resources:Attendance,User Generated%>"
                                                            OnCheckedChanged="rbtnUsergenerated_OnCheckedChanged" />
                                                        <asp:RadioButton ID="rbtnSystemgenerated" Font-Size="14px" Font-Bold="true"
                                                            runat="server" GroupName="b" AutoPostBack="true" Text="<%$ Resources:Attendance,System Generated%>"
                                                            OnCheckedChanged="rbtnUsergenerated_OnCheckedChanged" />


                                                        <hr />
                                                    </div>



                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblSystemDirectoryType" Visible="false" runat="server"
                                                            Text="<%$ Resources:Attendance,Type %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlSystemDirectoryType" Visible="false" runat="server"
                                                            CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlSystemDirectoryType_OnSelectedIndexChanged">
                                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Employee" Value="Employee"></asp:ListItem>
                                                            <asp:ListItem Text="Product" Value="Product"></asp:ListItem>
                                                            <asp:ListItem Text="Contact" Value="Contact"></asp:ListItem>
                                                            <asp:ListItem Text="Customer" Value="Customer"></asp:ListItem>
                                                            <asp:ListItem Text="Supplier" Value="Supplier"></asp:ListItem>
                                                            <asp:ListItem Text="Project" Value="Project"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:Label ID="lblLoginUser" Visible="false" runat="server"
                                                            Text="<%$ Resources:Attendance,Login User%>"></asp:Label>
                                                        <asp:DropDownList ID="ddlLoginUserName" Visible="false" runat="server"
                                                            CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlLoginUserName_OnSelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblDirectName" runat="server" Text="<%$ Resources:Attendance,Directory Name %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtDirectoryname" ErrorMessage="Enter Directory Name" />

                                                        <asp:TextBox ID="txtDirectoryname" BackColor="#eeeeee" AutoPostBack="true" OnTextChanged="txtDirectoryname_OnTextChanged" runat="server" CssClass="form-control" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListDirectoryName" ServicePath=""
                                                            CompletionInterval="100" MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtDirectoryname"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <asp:HiddenField ID="hdnDirectoryId" runat="server" Value="0" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblDocName" runat="server" Text="<%$ Resources:Attendance,Document Name %>"></asp:Label>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtDocumentName" ErrorMessage="Enter Document Name" />
                                                        <asp:TextBox ID="txtDocumentName" BackColor="#eeeeee" runat="server"
                                                            CssClass="form-control" AutoPostBack="true" OnTextChanged="txtDocumentName_OnTextChanged" />
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListDocName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtDocumentName"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <asp:HiddenField ID="hdndocumentid" runat="server" Value="0" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblfileName1" runat="server" Text="<%$ Resources:Attendance,File Name %>"></asp:Label>
                                                        <asp:TextBox ID="txtFileName" runat="server" CssClass="form-control" />

                                                        <br />
                                                    </div>
                                                    <div id="Div_File_Type" runat="server" visible="false" class="col-md-6">
                                                        <asp:Label ID="lblfiletype" runat="server" Text="<%$ Resources:Attendance,File Type %>" Visible="False"></asp:Label>
                                                        <asp:DropDownList ID="ddlFiletype" runat="server" CssClass="form-control" Visible="False" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblUploadfile" runat="server" Text="<%$ Resources:Attendance,File Upload %>"></asp:Label>
                                                        <div class="input-group" style="width: 100%;">
                                                            <cc1:AsyncFileUpload ID="UploadFile"
                                                                OnClientUploadStarted="FUAll_UploadStarted"
                                                                OnClientUploadError="FUAll_UploadError"
                                                                OnClientUploadComplete="FUAll_UploadComplete"
                                                                runat="server" CssClass="form-control btn btn-default"
                                                                CompleteBackColor="White"
                                                                UploaderStyle="Traditional"
                                                                UploadingBackColor="#CCFFFF"
                                                                ThrobberID="FUAll_ImgLoader" Width="100%" />
                                                            <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                                <asp:LinkButton ID="FUAll_Img_Right" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-check" style="font-size:30px;color:#22cb33"></i></asp:LinkButton>
                                                                <asp:LinkButton ID="FUAll_Img_Wrong" runat="server" Width="30px" Height="30px" Style="display: none"><i class="fa fa-remove" style="font-size:30px"></i></asp:LinkButton>
                                                                <asp:Image ID="FUAll_ImgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Description %>"></asp:Label>
                                                        <asp:TextBox ID="txtdesc" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                                        <br />
                                                    </div>


                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblSetReminder" runat="server" Text="Set Reminder"></asp:Label>
                                                        <br />
                                                        <asp:DropDownList ID="ddlSetReminder" runat="server" CssClass="form-control">
                                                            <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>



                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Expiry Date %>"></asp:Label>

                                                        <asp:TextBox ID="txtExpiryDate" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtCalenderExtender" runat="server" TargetControlID="txtExpiryDate"
                                                            Format="dd-MMM-yyyy">
                                                        </cc1:CalendarExtender>
                                                        <br />
                                                    </div>








                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Attendance,Save %>" OnClick="btnSave_Click"
                                                            Visible="false" CssClass="btn btn-success" ValidationGroup="Save" />

                                                        <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                            OnClick="btnReset_Click" CssClass="btn btn-primary" CausesValidation="False" />

                                                        <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                            OnClick="btnCancel_Click" CssClass="btn btn-danger" CausesValidation="False" />

                                                        <asp:HiddenField ID="editid" runat="server" />
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
                                        <div id="Div3" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label8" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblbinTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I3" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlbinFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,FileTransaction Id %>" Value="Trans_Id"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Directory Name %>" Value="Directory_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Document Name %>" Value="Document_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,File Name %>" Value="File_Name"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,User Id %>" Value="CreatedBy"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Directory For%>" Value="Directory_For"></asp:ListItem>
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
                                                    <asp:Panel ID="Panel8" runat="server" DefaultButton="btnbinbind">
                                                        <asp:TextBox placeholder="Search from Content"   ID="txtbinValue" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3" style="text-align: center">
                                                    <asp:LinkButton ID="btnbinbind" runat="server" CausesValidation="False" OnClick="btnbinbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnbinRefresh" runat="server" CausesValidation="False" OnClick="btnbinRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imgBtnRestore" CausesValidation="False" runat="server" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid" <%= gvFileMasterBin.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvFileMasterBin" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                        runat="server" AutoGenerateColumns="False" Width="100%" DataKeyNames="Trans_Id"
                                                        AllowPaging="True" OnPageIndexChanging="gvFileMasterBin_PageIndexChanging" OnSorting="gvFileMasterBin_OnSorting"
                                                        AllowSorting="True">
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
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,User Id%>" SortExpression="CreatedBy">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFileId1" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Directory For%>" SortExpression="Directory_For">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFileId2" runat="server" Text='<%# Eval("Directory_For") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Directory Name %>" SortExpression="Directory_name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblfileId3" runat="server" Text='<%# Eval("Directory_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Document Name %>" SortExpression="Document_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblfileId4" runat="server" Text='<%# Eval("Document_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,File Name %>" SortExpression="File_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFileName" runat="server" Text='<%# Eval("File_Name") %>'></asp:Label>
                                                                    <asp:Label ID="lblFileId" runat="server" Visible="false" Text='<%# Eval("Trans_Id") %>'></asp:Label>
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
    </script>
    <script type="text/javascript">
        function FUAll_UploadComplete(sender, args) {
            document.getElementById('<%= FUAll_Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= FUAll_Img_Right.ClientID %>').style.display = "";
        }
        function FUAll_UploadError(sender, args) {
            document.getElementById('<%= FUAll_Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= FUAll_Img_Wrong.ClientID %>').style.display = "";
        }
        function FUAll_UploadStarted(sender, args) {

        }
    </script>
</asp:Content>
