<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="DocumentNumber.aspx.cs" Inherits="SystemSetUp_DocumentNumber" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="far fa-file-pdf"></i>
        <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Attendance,Document Number%>"></asp:Label>

        <%--<small>Control panel</small>--%>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label17" runat="server" Text="<%$ Resources:Attendance,System SetUp%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label18" runat="server" Text="<%$ Resources:Attendance,System SetUp%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label19" runat="server" Text="<%$ Resources:Attendance,Document Number%>"></asp:Label></li>
    </ol>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" OnClick="btnNew_Click" Text="New" />
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="btnList_Click" Text="List" />
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
                    <%--<li><a href="#Mail" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Mail Subject %>"></asp:Label></a></li>  --%>
                    <li class="active"><a href="#Document" data-toggle="tab">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label15" runat="server" Text="<%$ Resources:Attendance,Document Number %>"></asp:Label></a></li>
                </ul>
                <div class="tab-content">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="box box-primary">
                                <div class="box-body">
                                    <div class="form-group">
                                        <div class="col-lg-4">
                                            <asp:Label ID="lblModuleName" runat="server" Text="<%$ Resources:Attendance,Module Name %>"></asp:Label>
                                            <asp:DropDownList ID="ddlModuleName" AutoPostBack="true" OnSelectedIndexChanged="ddlModuleName_SelectedIndexChanged1" runat="server" CssClass="form-control"></asp:DropDownList>
                                            <br />
                                        </div>
                                        <div class="col-lg-4">
                                            <asp:Label ID="LblObjectName" runat="server" Text="<%$ Resources:Attendance,Object Name %>"></asp:Label>
                                            <asp:DropDownList ID="ddlObjectName" runat="server" CssClass="form-control"></asp:DropDownList>
                                            <br />
                                        </div>
                                        <div class="col-lg-2">
                                            <br />
                                            <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                            <asp:LinkButton ID="btnReferesh" runat="server" CausesValidation="False" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="tab-pane active" id="Document">
                        <asp:UpdatePanel ID="Update_Document" runat="server">
                                                       <ContentTemplate>
                                <div id="PnlNewEdit" runat="server">
                                    <div id="pnlDocNo" runat="server" visible="false">
                                        <div class="box box-warning box-solid">
                                            <div class="box-header with-border">
                                                <h3 class="box-title"></h3>
                                            </div>
                                            <div class="box-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <br />
                                                        <div class="flow">
                                                            <asp:HiddenField ID="editid" runat="server" />
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvDocMaster" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server"
                                                                AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                                OnPageIndexChanging="gvDocMaster_PageIndexChanging" OnSorting="gvDocMaster_OnSorting">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                ImageUrl="~/Images/edit.png" CausesValidation="False" OnCommand="btnEdit_Command"
                                                                                Visible="false" ToolTip="<%$ Resources:Attendance,Edit %>" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                ImageUrl="~/Images/Erase.png" OnCommand="IbtnDelete_Command" Visible="false"
                                                                                ToolTip="<%$ Resources:Attendance,Delete %>" />


                                                                            <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>"
                                                                                TargetControlID="IbtnDelete">
                                                                            </cc1:ConfirmButtonExtender>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDocumentId1" runat="server" Text='<%# Eval("Trans_Id") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Module Name %>" SortExpression="Module_Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDocName" runat="server" Text='<%# Eval("Module_Name") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Object Name %>" SortExpression="Object_Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDocNameLocal" runat="server" Text='<%# Eval("Object_Name") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Prefix Name %>" SortExpression="Prefix">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblprefix" runat="server" Text='<%# Eval("Prefix") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Suffix Name %>" SortExpression="Suffix">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblsuffix" runat="server" Text='<%# Eval("Suffix") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <%-- <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Company Id %>"
                                                                        SortExpression="CompId">
                                                                        <ItemTemplate>
                                                                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Allow.png" Visible='<%#Eval("CompId") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle  />
                                                                    </asp:TemplateField>--%>
                                                                    <%-- <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Brand Id %>"
                                                                        SortExpression="BrandId">
                                                                        <ItemTemplate>
                                                                            <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/Allow.png" Visible='<%#Eval("BrandId") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle  />
                                                                    </asp:TemplateField>--%>
                                                                    <%-- <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Location Id %>"
                                                                        SortExpression="LocationId">
                                                                        <ItemTemplate>
                                                                            <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/Allow.png" Visible='<%#Eval("LocationId") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Department Id %>"
                                                                        SortExpression="DeptId">
                                                                        <ItemTemplate>
                                                                            <asp:Image ID="Image4" runat="server" ImageUrl="~/Images/Allow.png" Visible='<%#Eval("DeptId") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Employee Id %>"
                                                                        SortExpression="EmpId">
                                                                        <ItemTemplate>
                                                                            <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/Allow.png" Visible='<%#Eval("EmpId")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Day %>"
                                                                        SortExpression="Day">
                                                                        <ItemTemplate>
                                                                            <asp:Image ID="Image6" runat="server" ImageUrl="~/Images/Allow.png" Visible='<%#Eval("Day")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Month %>"
                                                                        SortExpression="Month">
                                                                        <ItemTemplate>
                                                                            <asp:Image ID="Image7" runat="server" ImageUrl="~/Images/Allow.png" Visible='<%#Eval("Month")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle  />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Year %>"
                                                                        SortExpression="Year">
                                                                        <ItemTemplate>
                                                                            <asp:Image ID="Image8" runat="server" ImageUrl="~/Images/Allow.png" Visible='<%#Eval("Year")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle  />
                                                                    </asp:TemplateField>--%>
                                                                </Columns>


                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                            <asp:HiddenField ID="HDFSort" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="pnlDocNew" runat="server" visible="false">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="box box-primary">
                                                    <div class="box-body">
                                                        <div class="form-group">
                                                            <div class="col-md-12">
                                                                <div class="col-md-6">
                                                                    <asp:Label ID="Lbl_name" runat="server" Text="<%$ Resources:Attendance,Prefix Name %>"></asp:Label>
                                                                    <asp:TextBox ID="txtPrefixName" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    <br />
                                                                </div>

                                                                <div class="col-md-6">
                                                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Attendance,Suffix Name %>"></asp:Label>
                                                                    <asp:TextBox ID="txtSuffixName" runat="server" CssClass="form-control"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="col-md-12">
                                                                <div class="col-md-2">
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <asp:CheckBox ID="chkDay" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Day %>" />
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <asp:CheckBox ID="chkCompanyId" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Company Id %>" />
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <asp:CheckBox ID="chkEmpId" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Employee Id %>" />
                                                                </div>
                                                                <div class="col-md-3">
                                                                </div>
                                                            </div>



                                                            <div class="col-md-12">
                                                                <div class="col-md-2">
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <asp:CheckBox ID="chkMonth" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Month %>" />
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <asp:CheckBox ID="chkBrandId" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Brand Id %>" />
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <asp:CheckBox ID="ChkModelId" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Model Id %>" Visible="false" /><br />
                                                                </div>
                                                                <div class="col-md-3">
                                                                </div>
                                                            </div>

                                                            <div class="col-md-12">
                                                                <div class="col-md-2">
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <asp:CheckBox ID="chkYear" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Year %>" />
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <asp:CheckBox ID="chkLocationId" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Location Id %>" />
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <asp:CheckBox ID="chkDepartmentId" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Department Id %>" />
                                                                </div>
                                                                <div class="col-md-3">
                                                                </div>
                                                            </div>

                                                            <div class="col-md-12">
                                                                <div class="col-md-2">
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <asp:CheckBox ID="chkFinancialYear" runat="server" Visible="false"
                                                                        AutoPostBack="true" OnCheckedChanged="Check_Clicked"
                                                                        CssClass="labelComman" Text="According To Financial Year" />
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <asp:CheckBox ID="chkNumberAutoCalculate" Visible="false" runat="server" CssClass="labelComman"
                                                                        Text="Auto Generate Number" />
                                                                </div>
                                                                <div class="col-md-1">
                                                                </div>
                                                            </div>

                                                            <div class="col-md-12">
                                                                <div class="col-md-2">
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <asp:Label ID="lblFromMonth" runat="server" Text="From Month" Visible="false" />
                                                                    <asp:DropDownList ID="ddlFromMonth" runat="server" CssClass="form-control" Visible="false">
                                                                        <asp:ListItem Value="--Select--" Text="--Select--"></asp:ListItem>
                                                                        <asp:ListItem Value="1" Text="Jan"></asp:ListItem>
                                                                        <asp:ListItem Value="2" Text="Feb"></asp:ListItem>
                                                                        <asp:ListItem Value="3" Text="Mar"></asp:ListItem>
                                                                        <asp:ListItem Value="4" Text="Apr"></asp:ListItem>
                                                                        <asp:ListItem Value="5" Text="May"></asp:ListItem>
                                                                        <asp:ListItem Value="6" Text="Jun"></asp:ListItem>
                                                                        <asp:ListItem Value="7" Text="Jul"></asp:ListItem>
                                                                        <asp:ListItem Value="8" Text="Aug"></asp:ListItem>
                                                                        <asp:ListItem Value="9" Text="Sep"></asp:ListItem>
                                                                        <asp:ListItem Value="10" Text="Oct"></asp:ListItem>
                                                                        <asp:ListItem Value="11" Text="Nov"></asp:ListItem>
                                                                        <asp:ListItem Value="12" Text="Dec"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <%--  <asp:Label ID="lblFromDate" runat="server" Text="From Date" />
                                                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" />
                                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender" runat="server" TargetControlID="txtFromDate" />--%>
                                                                </div>
                                                                <div class="col-md-2">
                                                                </div>
                                                                <div class="col-md-3">
                                                                </div>
                                                            </div>

                                                            <div class="col-md-12">
                                                                <div class="col-md-2">
                                                                </div>
                                                                <div class="col-md-3">
                                                                </div>
                                                                <div class="col-md-2">
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <asp:CheckBox ID="chkSupplierId" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Supplier Id %>" Visible="false" />
                                                                </div>
                                                                <div class="col-md-3">
                                                                </div>
                                                            </div>

                                                            <div class="col-md-12">
                                                                <div class="col-md-2">
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <asp:CheckBox ID="chkManufacturingbrandId" runat="server" CssClass="labelComman"
                                                                        Text="Manufacturing Brand Id" Visible="false" />
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <asp:CheckBox ID="chkCategoryId" runat="server" CssClass="labelComman" Text="Category Id"
                                                                        Visible="false" />
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <asp:CheckBox ID="chkColour" runat="server" CssClass="labelComman" Text="Colour"
                                                                        Visible="false" />
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <asp:CheckBox ID="chkSize" runat="server" CssClass="labelComman" Text="Size"
                                                                        Visible="false" />
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <asp:ImageButton ID="btnAdddocProduct" runat="server" CausesValidation="False"
                                                                        ImageUrl="~/Images/add.png" Visible="false" OnClick="btnAddNewRecord_Click"
                                                                        ToolTip="<%$ Resources:Attendance,Add %>" />
                                                                </div>
                                                                <div class="col-md-3">
                                                                </div>
                                                            </div>

                                                            <div class="col-md-12">
                                                                <div style="overflow-y: auto;">
                                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvProductdocument" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="false" AllowSorting="True">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                        ImageUrl="~/Images/Erase.png" OnCommand="IbtnDeleteDocument_Command" ToolTip="<%$ Resources:Attendance,Delete %>" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Prefix Name %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblprefix" runat="server" Text='<%# Eval("Prefix") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Suffix Name %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblsuffix" runat="server" Text='<%# Eval("Suffix") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Company Id %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Allow.png" Visible='<%#Eval("CompId") %>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Brand Id %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/Allow.png" Visible='<%#Eval("BrandId") %>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Location Id %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/Allow.png" Visible='<%#Eval("LocationId") %>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Employee Id %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/Allow.png" Visible='<%#Eval("EmpId")%>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Model Id">
                                                                                <ItemTemplate>
                                                                                    <asp:Image ID="Image11" runat="server" ImageUrl="~/Images/Allow.png" Visible='<%#Eval("ModelId")%>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Manufacturing Brand Id">
                                                                                <ItemTemplate>
                                                                                    <asp:Image ID="Image12" runat="server" ImageUrl="~/Images/Allow.png" Visible='<%#Eval("ManufacturingBrandId")%>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Category Id">
                                                                                <ItemTemplate>
                                                                                    <asp:Image ID="Image13" runat="server" ImageUrl="~/Images/Allow.png" Visible='<%#Eval("CategoryId")%>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Supplier Id">
                                                                                <ItemTemplate>
                                                                                    <asp:Image ID="Image20" runat="server" ImageUrl="~/Images/Allow.png" Visible='<%#Eval("SupplierId")%>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Day %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Image ID="Image6" runat="server" ImageUrl="~/Images/Allow.png" Visible='<%#Eval("Day")%>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Month %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Image ID="Image7" runat="server" ImageUrl="~/Images/Allow.png" Visible='<%#Eval("Month")%>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Year %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Image ID="Image8" runat="server" ImageUrl="~/Images/Allow.png" Visible='<%#Eval("Year")%>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="FinancialYearValue">
                                                                                <ItemTemplate>
                                                                                    <asp:Image ID="FinancialYearValue" runat="server" ImageUrl="~/Images/Allow.png" Visible='<%#Eval("FinancialYearValue")%>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="AutoGenerateNumber">
                                                                                <ItemTemplate>
                                                                                    <asp:Image ID="AutoGenerateNumber" runat="server" ImageUrl="~/Images/Allow.png" Visible='<%#Eval("AutoGenerateNumber")%>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="AutoGenerateNumberMonth">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="AutoGenerateNumberMonth" runat="server" Text='<%# Eval("AutoGenerateNumberMonth") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Colour">
                                                                                <ItemTemplate>
                                                                                    <asp:Image ID="ImgColour" runat="server" ImageUrl="~/Images/Allow.png" Visible='<%#Eval("Colour")%>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Size">
                                                                                <ItemTemplate>
                                                                                    <asp:Image ID="ImgSize" runat="server" ImageUrl="~/Images/Allow.png" Visible='<%#Eval("Size")%>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <PagerStyle CssClass="pagination-ys" />
                                                                    </asp:GridView>
                                                                </div>
                                                            </div>

                                                            <div class="col-md-12" style="text-align: center">
                                                                <br />
                                                                <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Attendance,Save %>" OnClick="btnSave_Click" Visible="false" CssClass="btn btn-success" ValidationGroup="a" />
                                                                <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>" OnClick="btnReset_Click" CssClass="btn btn-primary" CausesValidation="False" />
                                                                <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>" OnClick="btnCancel_Click" CssClass="btn btn-danger" CausesValidation="False" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="tab-pane" id="Mail">
                        <asp:UpdatePanel ID="Update_Mail" runat="server">
                            <ContentTemplate>
                                <div id="PnlList" runat="server">
                                    <div id="pnlEmailSubject" runat="server" visible="false">

                                        <div class="box box-warning box-solid">
                                            <div class="box-header with-border">
                                                <h3 class="box-title"></h3>
                                            </div>
                                            <div class="box-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <br />
                                                        <div class="flow">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvMailSubject" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                                runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                                OnPageIndexChanging="gvMailSubject_PageIndexChanging" OnSorting="gvMailSubject_OnSorting">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Edit %>">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                ImageUrl="~/Images/edit.png" CausesValidation="False" OnCommand="btnMailEdit_Command"
                                                                                Visible="false" ToolTip="<%$ Resources:Attendance,Edit %>" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                ImageUrl="~/Images/Erase.png" OnCommand="IbtnMailDelete_Command" Visible="false"
                                                                                ToolTip="<%$ Resources:Attendance,Delete %>" />


                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDocumentId1" runat="server" Text='<%# Eval("Trans_Id") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Module Name %>" SortExpression="Module_Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDocName" runat="server" Text='<%# Eval("Module_Name") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Object Name %>" SortExpression="Object_Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDocNameLocal" runat="server" Text='<%# Eval("Object_Name") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Prefix Name %>" SortExpression="Prefix">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblprefix" runat="server" Text='<%# Eval("Prefix") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Suffix Name %>" SortExpression="Suffix">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblsuffix" runat="server" Text='<%# Eval("Suffix") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Contact Id %>"
                                                                        SortExpression="Contact_Id">
                                                                        <ItemTemplate>
                                                                            <asp:Image ID="Image4" runat="server" ImageUrl="~/Images/Allow.png" Visible='<%#Eval("Contact_Id") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Employee Id %>"
                                                                        SortExpression="EmpId">
                                                                        <ItemTemplate>
                                                                            <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/Allow.png" Visible='<%#Eval("EmpId")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Document Number %>"
                                                                        SortExpression="DocNo">
                                                                        <ItemTemplate>
                                                                            <asp:Image ID="Image9" runat="server" ImageUrl="~/Images/Allow.png" Visible='<%#Eval("DocNo") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Day %>"
                                                                        SortExpression="Day">
                                                                        <ItemTemplate>
                                                                            <asp:Image ID="Image6" runat="server" ImageUrl="~/Images/Allow.png" Visible='<%#Eval("Day")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Month %>"
                                                                        SortExpression="Month">
                                                                        <ItemTemplate>
                                                                            <asp:Image ID="Image7" runat="server" ImageUrl="~/Images/Allow.png" Visible='<%#Eval("Month")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Year %>"
                                                                        SortExpression="Year">
                                                                        <ItemTemplate>
                                                                            <asp:Image ID="Image8" runat="server" ImageUrl="~/Images/Allow.png" Visible='<%#Eval("Year")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                </Columns>


                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                            <asp:HiddenField ID="HDFEmailSort" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="pnlEmailNew" runat="server" visible="false">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="box box-primary">
                                                    <div class="box-body">
                                                        <div class="form-group">
                                                            <div class="col-md-12">
                                                                <div class="col-md-6">
                                                                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Prefix Name %>"></asp:Label>
                                                                    <asp:TextBox ID="txtMailPrefix" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    <br />
                                                                </div>

                                                                <div class="col-md-6">
                                                                    <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Suffix Name %>"></asp:Label>
                                                                    <asp:TextBox ID="txtMailSuffix" runat="server" CssClass="form-control"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="col-md-12">
                                                                <div class="col-md-3">
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <asp:CheckBox ID="chkMailContactId" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Contact Id %>" />
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <asp:CheckBox ID="chkMailEmpId" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Employee Id %>" /><br />
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <asp:CheckBox ID="chkDocNo" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Document Number %>" /><br />
                                                                </div>
                                                                <div class="col-md-3">
                                                                </div>
                                                            </div>

                                                            <div class="col-md-12">
                                                                <div class="col-md-3">
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <asp:CheckBox ID="chkMailDay" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Day %>" />
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <asp:CheckBox ID="chkMailMonth" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Month %>" />
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <asp:CheckBox ID="chkMailYear" runat="server" CssClass="labelComman" Text="<%$ Resources:Attendance,Year %>" />
                                                                </div>
                                                                <div class="col-md-3">
                                                                </div>
                                                            </div>

                                                            <div class="col-md-12" style="text-align: center">
                                                                <br />
                                                                <asp:Button ID="btnMailSubjectSave" runat="server" Text="<%$ Resources:Attendance,Save %>" OnClick="btnMailSubjectSave_Click" Visible="false" CssClass="btn btn-success" ValidationGroup="a" />
                                                                <asp:Button ID="btnMailSubjectReset" runat="server" Text="<%$ Resources:Attendance,Reset %>" OnClick="btnMailSubjectReset_Click" CssClass="btn btn-primary" CausesValidation="False" />
                                                                <asp:Button ID="btnMailSubjectCancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>" OnClick="btnMailSubjectCancel_Click" CssClass="btn btn-danger" CausesValidation="False" />
                                                                <asp:HiddenField ID="EditMailSubjectId" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>
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
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_Document">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_Mail">
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

        function Li_Tab_List() {
            document.getElementById('<%= Btn_List.ClientID %>').click();
        }

        function Li_Tab_New() {
            document.getElementById('<%= Btn_New.ClientID %>').click();
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
    </script>
</asp:Content>




