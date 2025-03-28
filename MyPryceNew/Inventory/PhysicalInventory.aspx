<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="PhysicalInventory.aspx.cs" 
    Inherits="Inventory_PhysicalInventory" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server" ClientIDMode="Static">
    <h1>
        <i class="fas fa-people-carry"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Physical Inventory%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Physical Inventory%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:Button ID="Btn_myModal" Style="display: none;" runat="server" data-toggle="modal" data-target="#myModal" Text="View Modal" />
            <asp:HiddenField runat="server" ID="hdnCanEdit" />
            <asp:HiddenField runat="server" ID="hdnCanDelete" />
            <asp:HiddenField runat="server" ID="hdnCanPrint" />
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
                <ul class="nav nav-tabs pull-right bg-blue-gradient" id="tabTitleBar">
                    <li id="Li_Bin"><a href="#Bin" onclick="Li_Tab_Bin()" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
                    <li id="Li_Scan"><a href="#Scan" data-toggle="tab" onclick="fnScanTabClick()">
                        <i class="fa fa-list"></i>&nbsp;&nbsp;<asp:Label ID="Label16" runat="server" Text="Scan"></asp:Label></a></li>
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
                                                    <asp:DropDownList ID="ddlPosted" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPosted_SelectedIndexChanged">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Posted %>" Value="Posted"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,UnPosted %>" Value="UnPosted" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,All %>" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Voucher No %>" Value="VoucherNo"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Voucher Date %>" Value="VoucherDate"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Net Amount %>" Value="NetAmount"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Account No %>" Value="AccountNo"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Remark %>" Value="Remark"></asp:ListItem>
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
                                                        <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False" OnClick="btnbind_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False" OnClick="btnRefresh_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
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
                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvPhysical" PageSize="<%# PageControlCommon.GetPageSize() %>" runat="server"
                                                        AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                        OnPageIndexChanging="gvPhysical_PageIndexChanging" OnSorting="gvPhysical_Sorting">
                                                        <Columns>

                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnPrint" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' OnCommand="IbtnPrint_Command"><i class="fa fa-print"></i>Print</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnView" runat="server" CommandArgument='<%# Eval("Trans_Id") %>' CausesValidation="False" OnCommand="btnView_Command"><i class="fa fa-eye"></i>View</asp:LinkButton>
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
                                                                <ItemStyle Width="5%" />
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Voucher No %>" SortExpression="VoucherNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblVoucherNo" runat="server" Text='<%# Eval("VoucherNo") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Voucher Date %>" SortExpression="Vdate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblVdate" runat="server" Text='<%# SetDateFormat(Eval("Vdate").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Account No %>" SortExpression="AccountNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAccountNo" runat="server" Text='<%# Eval("AccountNo").ToString() %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Net Amount%>" SortExpression="NetAmount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblNetAmount" runat="server" Text='<%# SetDecimal(Eval("NetAmount").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Remark%>" SortExpression="Remark">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRemark" runat="server" Text='<%# Eval("Remark") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="40%" />
                                                            </asp:TemplateField>
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
                    <div class="tab-pane" id="New">
                        <asp:UpdatePanel ID="Update_New" runat="server">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnExport" />
                                <asp:PostBackTrigger ControlID="btnDownload" />
                                <asp:PostBackTrigger ControlID="btnConsolidatedDownload" />
                                 <asp:PostBackTrigger ControlID="gvProduct" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblVoucherdate" runat="server" Text="<%$ Resources:Attendance,Voucher Date %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtVoucherdate" ErrorMessage="<%$ Resources:Attendance,Enter Voucher Date%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtVoucherdate" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtCalenderExtender" runat="server" TargetControlID="txtVoucherdate"
                                                            Format="dd-MMM-yyyy">
                                                        </cc1:CalendarExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblVoucherNo" runat="server" Text="<%$ Resources:Attendance,Voucher No %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtVoucherNo" ErrorMessage="<%$ Resources:Attendance,Enter Voucher No%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtVoucherNo" runat="server" CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-5">
                                                        <asp:Label ID="lblType" runat="server" Text="<%$ Resources:Attendance,Type %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlPhysical" runat="server" CssClass="form-control"
                                                            Enabled="false">
                                                            <asp:ListItem Selected="True" Text="Product Wise" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Category Wise" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Manufacturing Brand Wise" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="Rack Wise" Value="3"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-7">
                                                        <br />
                                                        <asp:Button ID="btnGetRecord" CssClass="btn btn-primary" Style="margin-left: 10px;" runat="server" CausesValidation="False"
                                                            OnClick="btnGetRecord_Click" Text="Get System Inventory" />

                                                        <asp:Button ID="btnDownload" runat="server" Visible="false" CssClass="btn btn-primary"
                                                            Text="Download Excel" OnClick="btnDownload_Click" />

                                                        <asp:Button ID="btnConsolidatedDownload" runat="server" Visible="false" CssClass="btn btn-primary"
                                                            Text="Download Consolidated Excel" OnClick="btnConsolidatedDownload_Click" />
                                                        
                                                        <asp:CheckBox ID="chkDeleteLog" Text="Physical Record Update?"  runat="server" Visible="false" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div class="box box-primary">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="lblUpload" runat="server" Text="<%$ Resources:Attendance,Upload %>"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <div class="form-group">
                                                                            <div class="col-md-6">
                                                                                <div class="input-group" style="width: 100%;">
                                                                                    <cc1:AsyncFileUpload ID="fileLoad"
                                                                                        OnClientUploadStarted="FUExcel_UploadStarted"
                                                                                        OnClientUploadError="FUExcel_UploadError"
                                                                                        OnClientUploadComplete="FUExcel_UploadComplete"
                                                                                        OnUploadedComplete="FUExcel_FileUploadComplete"
                                                                                        runat="server" CssClass="form-control"
                                                                                        CompleteBackColor="White"
                                                                                        UploaderStyle="Traditional"
                                                                                        UploadingBackColor="#CCFFFF"
                                                                                        ThrobberID="FUExcel_ImgLoader" Width="100%" />
                                                                                    <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                                                        <asp:Image ID="FUExcel_Img_Right" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Allow.png" Style="display: none" />
                                                                                        <asp:Image ID="FUExcel_Img_Wrong" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Delete1.png" Style="display: none" />
                                                                                        <asp:Image ID="FUExcel_ImgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                                                    </div>
                                                                                </div>
                                                                                <br />
                                                                            </div>

                                                                            <div class="col-md-6">
                                                                                <asp:RadioButton ID="rbtnoverwrite" runat="server" Text="OverWrite"
                                                                                    GroupName="a" Checked="true" />
                                                                                <asp:RadioButton ID="rbtnAppend" Style="margin-left: 10px;" runat="server" Text="Append"
                                                                                    GroupName="a" />

                                                                                <asp:Button ID="btnUpload" CssClass="btn btn-primary" runat="server" Style="margin-left: 10px;" CausesValidation="False" OnClick="btnUpload_Click" Visible="false" Text="Upload Excel" />

                                                                                <br />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12" style="display: none">
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div class="box box-primary">
                                                                    <div class="box-header with-border">
                                                                        <h3 class="box-title">
                                                                            <asp:Label ID="lbladdproduct" runat="server" Text="Product Detail"></asp:Label></h3>
                                                                        <div class="box-tools pull-right">
                                                                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                                                <i class="fa fa-minus"></i>
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-body">
                                                                        <div class="form-group">

                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="Label3" runat="server" Text="Scan Product" Font-Bold="true"></asp:Label>
                                                                                <asp:TextBox ID="txtScanProductCode" runat="server" BackColor="#eeeeee" CssClass="form-control" AutoPostBack="True" OnTextChanged="txtScanProductCode_TextChanged" />

                                                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" CompletionInterval="100"
                                                                                    DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductCode"
                                                                                    ServicePath="" TargetControlID="txtScanProductCode" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                </cc1:AutoCompleteExtender>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="Label15" runat="server" Text="Scan Product(Scanning)" Font-Bold="true"></asp:Label>
                                                                                <asp:TextBox ID="txtScanProduct" runat="server" BackColor="#eeeeee" CssClass="form-control" AutoPostBack="False" onchange="alert('function fired');fnScanProduct();" ClientIDMode="Static" />
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-12">
                                                                                <div style="height: 33px; text-align: left;" class="input-group-addon">
                                                                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Product Code %>" Font-Bold="true"></asp:Label>
                                                                                    &nbsp:&nbsp<asp:Label ID="lblProductCode" Font-Bold="true" runat="server" ClientIDMode="Static"></asp:Label>
                                                                                </div>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-12">
                                                                                <div style="height: 33px; text-align: left;" class="input-group-addon">
                                                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Attendance,Product Name %>" Font-Bold="true"></asp:Label>
                                                                                    &nbsp:&nbsp<asp:Label ID="lblProuctName" Font-Bold="true" runat="server" ClientIDMode="Static"></asp:Label>
                                                                                </div>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-12">
                                                                                <div style="height: 33px; text-align: left;" class="input-group-addon">
                                                                                    <asp:Label ID="Label7" runat="server" Text="Physical Quantity" Font-Bold="true"></asp:Label>
                                                                                    &nbsp:&nbsp<asp:Label ID="lblPhysicalQuantity" Font-Bold="true" runat="server"></asp:Label>
                                                                                </div>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-12">
                                                                                <div style="height: 33px; text-align: left;" class="input-group-addon">
                                                                                    <asp:Label ID="Label8" runat="server" Text="System Qty" Font-Bold="true"></asp:Label>
                                                                                    &nbsp:&nbsp<asp:Label ID="lblSystemQty" runat="server" Font-Bold="true"></asp:Label>
                                                                                </div>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Label ID="Label9" runat="server" Text="New Quantity" Font-Bold="true"></asp:Label>
                                                                                <asp:TextBox ID="txtNewqty" ClientIDMode="Static" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <br />
                                                                                <asp:RadioButton ID="rbtnDetailOverwrite" runat="server" Text="OverWrite" GroupName="b" />
                                                                                <asp:RadioButton ID="rbtnDetailAppend" Style="margin-left: 10px;" runat="server" Text="Append" GroupName="b" Checked="true" />
                                                                                <br />
                                                                            </div>
                                                                            <div class="col-md-12" style="text-align: center">
                                                                                <asp:Button ID="btnadd" runat="server" CssClass="btn btn-primary" Text="Add" OnClick="btnadd_OnClick" />
                                                                                <br />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12" style="overflow: auto; max-height: 500px;">
                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvProduct" runat="server" AutoGenerateColumns="False" Width="100%"
                                                            AllowSorting="True">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Serial No %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSerialNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id%>" ItemStyle-Width="20%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("ProductCode") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>" ItemStyle-Width="50%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPID" Visible="false" runat="server" Text='<%# Eval("ProductId") %>'></asp:Label>
                                                                        <asp:Label ID="lblProductId" runat="server" Text='<%# Eval("EProductName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Rack Name %>">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtRackname" runat="server" Value='<%# Eval("RackName") %>' Width="100px" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Name %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("Unit_Name").ToString() %>'></asp:Label>
                                                                        <asp:Label ID="lblUnitId" runat="server" Visible="false" Text='<%# GetUnitIdByName(Eval("Unit_Name").ToString()) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit cost %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblUnitCost12" runat="server" Text='<%# SetDecimal(Eval("UnitCost").ToString()) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Average cost">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="lblUnitCost" Width="100px" runat="server"
                                                                            OnTextChanged="lblUnitCost_TextChanged" AutoPostBack="true" Text='<%# SetDecimal(Eval("AverageCost").ToString()) %>'></asp:TextBox>
                                                                        <asp:Label ID="lblUnitCost1" runat="server" Text='<%#  SetDecimal(Eval("AverageCost").ToString()) %>'
                                                                            Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,System Quantity %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSystemQuantity" runat="server" Text='<%# SetDecimal(Eval("SystemQuantity").ToString()) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Physical Quantity %>">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtPhysicalQuantity" Width="70px" runat="server"
                                                                            OnTextChanged="lblUnitCost_TextChanged" AutoPostBack="true" Text='<%# SetDecimal(Eval("PhysicalQuantity").ToString()) %>'></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Difference Qty">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="txtDifferenceQuantity" Width="70px" runat="server"
                                                                            Text='<%#SetDecimal((float.Parse(Eval("PhysicalQuantity").ToString()==""?"0":Eval("PhysicalQuantity").ToString() )-float.Parse(Eval("SystemQuantity").ToString()==""?"0":Eval("SystemQuantity").ToString())).ToString() ) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="100px" />
                                                                </asp:TemplateField>
                                                            </Columns>


                                                            <PagerStyle CssClass="pagination-ys" />

                                                        </asp:GridView>
                                                        <br />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label11" runat="server" Text="System Amount"></asp:Label>
                                                        <asp:TextBox ID="txtNetAmount" runat="server" CssClass="form-control" Enabled="false" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label12" runat="server" Text="Physical Amount"></asp:Label>
                                                        <asp:TextBox ID="txtPhysical" runat="server" CssClass="form-control" Enabled="false" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label24" runat="server" Text="<%$ Resources:Attendance,For Debit Account %>" />
                                                        <asp:TextBox ID="txtpaymentdebitaccount" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtpaymentdebitaccount"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label25" runat="server" Text="<%$ Resources:Attendance,For Credit Account %>" />
                                                        <asp:TextBox ID="txtpaymentCreditaccount" runat="server" CssClass="form-control" BackColor="#eeeeee"
                                                            AutoPostBack="true" OnTextChanged="txtcmnAccount_textChnaged"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetCompletionListAccountName" ServicePath="" CompletionInterval="100"
                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtpaymentCreditaccount"
                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                        </cc1:AutoCompleteExtender>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblRemarks" runat="server" Text="<%$ Resources:Attendance,Remark %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtRemarks" ErrorMessage="<%$ Resources:Attendance,Enter Remark%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnPost" runat="server" Text="<%$ Resources:Attendance,Post %>" ValidationGroup="Save" CssClass="btn btn-primary"
                                                            OnClick="btnPost_Click" />
                                                        <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="Are you sure to post the record ?"
                                                            TargetControlID="btnPost">
                                                        </cc1:ConfirmButtonExtender>

                                                        <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Attendance,Save %>" CssClass="btn btn-success"
                                                            OnClick="btnSave_Click" ValidationGroup="Save" Visible="false" />

                                                        <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                            CssClass="btn btn-primary" OnClick="btnReset_Click" CausesValidation="False" />

                                                        <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Cancel %>"
                                                            CssClass="btn btn-danger" OnClick="btnCancel_Click" CausesValidation="False" />

                                                        <asp:Button ID="btnExport" runat="server" Text="Export to Excel" CssClass="btn btn-primary"
                                                            OnClick="btnExport_Click" CausesValidation="False" />

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


                    <div class="tab-pane" id="Scan">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="box box-primary">
                                    <div class="box-body">
                                        <div class="form-group">
                                            <div id="divScan1">
                                                <div class="col-md-6">
                                                    <label title="Location">Location</label>
                                                    <asp:DropDownList ID="ddlScanLocation" runat="server" ClientIDMode="Static" CssClass="form-control" AutoPostBack="false" onchange="fnDdlScanLocationChange();return false">
                                                    </asp:DropDownList>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <label title="Voucher No">Voucher No</label>
                                                    <asp:TextBox ID="txtScanVoucherNo" runat="server" placeholder="Select Voucher No" class="form-control" ClientIDMode="Static" />
                                                    <asp:HiddenField ID="hdnScanVoucherId" runat="server" Value="0" ClientIDMode="Static" />
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <input type="button" id="btnScan1to2" value="Next" class="btn btn-default" onclick="fnMoveScanScreen(this)" />
                                                </div>
                                            </div>
                                            <div id="divScan2" style="display: none">
                                                <div class="col-md-6">
                                                    <label title="Product">Product</label>
                                                    <input id="txtScanProductId" class="form-control" type="text" value="" placeholder="Scan product Id or serial" onchange="fnScanProduct()" />
                                                    <asp:HiddenField ID="hdnScanProductId" ClientIDMode="Static" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdnScanSerialNo" ClientIDMode="Static" runat="server" Value="" />
                                                    <br />
                                                </div>


                                                <div class="col-md-6">
                                                    <label title="Rack No">Rack No</label>
                                                    <asp:DropDownList ID="ddlScanRackNo" runat="server" ClientIDMode="Static" CssClass="form-control">
                                                    </asp:DropDownList>
                                                    <br />
                                                </div>

                                                <div class="col-md-6">
                                                    <label title="Rack No">Product Code</label>
                                                    <input id="txtScanCode" placeholder="Product Code" class="form-control" type="text" disabled />
                                                    <br />
                                                </div>

                                                <div class="col-md-6">
                                                    <label title="Product Name">Product Name</label>
                                                    <input id="txtScanProductName" placeholder="Product Name" class="form-control" type="text" disabled />
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <label title="System Qty">System Qty</label>
                                                    <input id="txtScanSystemQty" placeholder="System Qty" class="form-control" type="text" disabled />
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <label title="Scan Qty">Scan Qty</label>
                                                    <div class="input-group">
                                                        <input id="txtScanScanQty" placeholder="Scan Qty" class="form-control" type="text" />
                                                        <div class="input-group-btn">
                                                            <input type="button" id="btnUpdateInventory" value="Update" class="btn btn-success" onclick="fnUpdateScanedProduct()" />
                                                        </div>
                                                    </div>
                                                    <br />
                                                </div>
                                                <div class="col-md-6">
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <div class="col-md-6" style="float: left">
                                                        <input type="button" id="btnScan2to1" value="Previous" class="btn btn-default" onclick="fnMoveScanScreen(this)" />
                                                    </div>
                                                    <div class="col-md-6" style="float: right">
                                                        <input type="button" id="btnScan2to3" value="Query" class="btn btn-default" onclick="fnMoveScanScreen(this)" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="divScan3" style="display: none">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlScanSearchFields" runat="server" CssClass="form-control" ClientIDMode="Static">
                                                        <asp:ListItem Selected="True" Text="Product Code" Value="productCode"></asp:ListItem>
                                                        <asp:ListItem Text="Product Name" Value="EProductName"></asp:ListItem>
                                                        <asp:ListItem Text="Serial No" Value="serial_no"></asp:ListItem>
                                                        <asp:ListItem Text="User" Value="emp_name"></asp:ListItem>
                                                        <asp:ListItem Text="Rack No" Value="rack_name"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <br />
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlScanOperator" runat="server" CssClass="form-control" ClientIDMode="Static">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>" Value="Equal"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>" Selected="True" Value="Like"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <br />
                                                </div>
                                                <div class="col-lg-6">
                                                    <div class="input-group">
                                                        <asp:Panel ID="Panel3" runat="server" DefaultButton="btnbind">
                                                            <asp:TextBox ID="txtScanProductTblSearch" runat="server" ClientIDMode="Static" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                                        </asp:Panel>
                                                        <div class="input-group-btn">
                                                            <asp:LinkButton ID="lnkSearchScanTbl" OnClientClick="fnSearchScannedProductLogs();return false;" runat="server" CausesValidation="False" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="lnkRefreshScanTbl" runat="server" OnClientClick="fnGetScannedProductLogs();return false;" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                                        </div>
                                                    </div>

                                                    <br />
                                                </div>

                                                <div class="col-md-12">
                                                    <input type="button" id="btnPostScanProducts" value="Submit Data" class="btn btn-success" onclick="fnPostScanLogs()" />
                                                    <br />
                                                </div>
                                                <div class="col-md-12" style="overflow: auto; height: 500px">
                                                    <br />
                                                    <table id="tblScanProducs" class="table-striped table-bordered table table-hover" style="width: 100%">
                                                        <thead>
                                                            <tr>
                                                                <td style="text-align: left; display: none">Product Id</td>
                                                                <td style="text-align: left">Product Code</td>
                                                                <td style="text-align: left">Name</td>
                                                                <td style="text-align: left">Serial</td>
                                                                <td style="text-align: right">Sys Qty</td>
                                                                <td style="text-align: right">Phy Qty</td>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                        </tbody>
                                                    </table>
                                                </div>
                                                <div class="col-md-12">
                                                    <input type="button" id="btnScan3to2" value="Previous" class="btn btn-default" onclick="fnMoveScanScreen(this)" />
                                                </div>
                                            </div>

                                        </div>
                                    </div>
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
                                                    <asp:Label ID="Label14" runat="server" Text="Advance Search"></asp:Label></h3>
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
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Voucher No %>" Value="VoucherNo"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Net Amount %>" Value="NetAmount"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Account No %>" Value="AccountNo"></asp:ListItem>
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
                                                    <asp:LinkButton ID="btnbinRefresh" runat="server" CausesValidation="False" OnClick="btnbinRefresh_Click" Width="25px" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imgBtnRestore" CausesValidation="False" runat="server" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>
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
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvBinPhysical" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" AllowSorting="True"
                                                        OnPageIndexChanging="gvBinPhysical_PageIndexChanging" OnSorting="gvBinPhysical_Sorting">
                                                        <Columns>
                                                            <asp:TemplateField>
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
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Voucher No %>" SortExpression="VoucherNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblVoucherNo" runat="server" Text='<%# Eval("VoucherNo") %>'></asp:Label>
                                                                    <asp:Label ID="lbltransId" runat="server" Text='<%# Eval("Trans_Id") %>' Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Voucher Date %>" SortExpression="Vdate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblVdate" runat="server" Text='<%# SetDateFormat(Eval("Vdate").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Net Amount%>" SortExpression="NetAmount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblNetAmount" runat="server" Text='<%# Eval("NetAmount") %>'></asp:Label>
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
        $(document).ready(function () {

        });
        var tblScanProducts;
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

        function myModal_Popup() {
            document.getElementById('<%= Btn_myModal.ClientID %>').click();
        }
        function myModal_Popup() {
            document.getElementById('<%= Btn_myModal.ClientID %>').click();
        }
    </script>

    <script type="text/javascript">

        function FUExcel_UploadComplete(sender, args) {
            document.getElementById('<%= FUExcel_Img_Wrong.ClientID %>').style.display = "none";
            document.getElementById('<%= FUExcel_Img_Right.ClientID %>').style.display = "";
        }
        function FUExcel_UploadError(sender, args) {
            document.getElementById('<%= FUExcel_Img_Right.ClientID %>').style.display = "none";
            document.getElementById('<%= FUExcel_Img_Wrong.ClientID %>').style.display = "";
            alert('Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file');
        }
        function FUExcel_UploadStarted(sender, args) {
            var filename = args.get_fileName();

            var filext = filename.substring(filename.lastIndexOf(".") + 1);
            filext = filext.toLowerCase();
            if (filext == "xls" || filext == "xlsx" || filext == "mdb" || filext == "accdb") {
                return true;
            }
            else {
                throw {
                    name: "Invalid File Type",
                    level: "Error",
                    message: "Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file",
                    htmlMessage: "Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file"
                }
                return false;
            }
        }

        function fnDdlScanLocationChange() {
            debugger;
            try {
                $('#hdnScanVoucherId').val('');
                $('#txtScanVoucherNo').val('');
                if ($('#ddlScanLocation').val() != undefined && $('#ddlScanLocation').val() != "0") {
                    var locationId = $('#ddlScanLocation').val();
                    $.ajax({
                        url: "PhysicalInventory.aspx/GetOpenVoucherList",
                        method: "post",
                        contentType: "application/json; charset=utf-8",
                        data: "{'strLocationId':'" + locationId + "'}",
                        success: function (data) {
                            debugger;
                            var result = data.d;
                            if (result != null) {
                                result = JSON.parse(result);
                                if (result.bResult == true) {
                                    var _detail = result.objResultLst[0];
                                    $('#hdnScanVoucherId').val(_detail.trans_id);
                                    $('#txtScanVoucherNo').val(_detail.voucher_no);
                                    return;
                                }
                            }
                        },
                        error: function (err) {
                            debugger;
                        }
                    });
                }
            }
            catch (ex) {
                $('#hdnScanVoucherId').val('');
                $('#txtScanVoucherNo').val('');
            }
        }

        function fnScanProduct() {
            debugger;
            try {
                if ($('#txtScanProductId').val() != undefined && $('#txtScanProductId').val() != "") {
                    var voucherId = $('#hdnScanVoucherId').val();
                    $.ajax({
                        url: "PhysicalInventory.aspx/GetScanProductDetail",
                        method: "post",
                        contentType: "application/json; charset=utf-8",
                        data: "{'strVoucherId':'" + voucherId + "','strScanText':'" + $('#txtScanProductId').val() + "'}",
                        success: function (data) {
                            debugger;
                            var result = data.d;
                            if (result != null) {
                                result = JSON.parse(result);
                                if (result.bResult == true) {
                                    var productDetail = result.objResultLst[0];
                                    $('#hdnScanProductId').val(productDetail.productId);
                                    $('#txtScanCode').val(productDetail.productCode);
                                    $('#txtScanProductName').val(productDetail.productName);
                                    $('#txtScanSystemQty').val(productDetail.sysQty);
                                    $('#hdnScanSerialNo').val(productDetail.serialNo);
                                    $('#txtScanScanQty').val('1');
                                    $('#txtScanScanQty').focus();
                                    $("#txtScanScanQty").select();
                                    return;
                                }
                            }
                            fnResetScanPanel();
                            $('#txtScanProductId').focus();
                            alert("Product not found");

                        },
                        error: function (err) {
                            debugger;
                        }
                    });
                }
            }
            catch (ex) {

            }
        }

        function fnPostScanLogs() {
            debugger;
            try {
                if ($('#hdnScanVoucherId').val() != undefined && $('#hdnScanVoucherId').val() != "" && $('#hdnScanVoucherId').val() != "0") {
                    var voucherId = $('#hdnScanVoucherId').val();
                    $.ajax({
                        url: "PhysicalInventory.aspx/PostScanLogs",
                        method: "post",
                        contentType: "application/json; charset=utf-8",
                        data: "{'strVoucherId':'" + voucherId + "'}",
                        success: function (data) {
                            debugger;
                            var result = data.d;
                            if (result != null) {
                                result = JSON.parse(result);
                                alert(result.msg);
                            }
                        },
                        error: function (err) {
                            debugger;
                        }
                    });
                }
            }
            catch (ex) {

            }
        }

        function fnUpdateScanedProduct() {
            debugger;
            try {
                if ($('#hdnScanVoucherId').val() == undefined || $('#hdnScanVoucherId').val() == "" || $('#hdnScanVoucherId').val() == "0") {
                    alert("Invalid Voucher");
                    return;
                }

                if ($('#txtScanScanQty').val() == undefined || $('#txtScanScanQty').val() == "" || $('#txtScanScanQty').val() == "0") {
                    alert("Invalid Qty");
                    return;
                }

                var phyQty = parseInt($('#txtScanScanQty').val());
                if (phyQty <= 0) {
                    alert("Invalid Qty");
                    return;
                }

                if ($('#hdnScanProductId').val() == "0" || $('#hdnScanProductId').val() == undefined || $('#hdnScanProductId').val() == "") {
                    alert("Invalid Product Id");
                    return;
                }

                var voucherId = $('#hdnScanVoucherId').val();
                var scanProducDetail = new Object();
                scanProducDetail.productId = $('#hdnScanProductId').val();
                scanProducDetail.productCode = '';
                scanProducDetail.productName = '';
                scanProducDetail.RackNo = $('#ddlScanRackNo').val();
                scanProducDetail.sysQty = '';
                scanProducDetail.phyQty = phyQty;
                scanProducDetail.serialNo = $('#hdnScanSerialNo').val();
                scanProducDetail.voucherId = $('#hdnScanVoucherId').val();

                $.ajax({
                    url: "PhysicalInventory.aspx/UpdateScanProductDetail",
                    method: "post",
                    contentType: "application/json; charset=utf-8",
                    data: "{_cls:" + JSON.stringify(scanProducDetail) + "}",
                    dataType: 'json',
                    success: function (data) {
                        debugger;
                        var result = data.d;
                        if (result != null) {
                            result = JSON.parse(result);
                            if (result.bResult == true) {
                                alert("Record updated");
                                fnResetScanPanel();
                                $('#txtScanProductId').focus();
                                return;
                            }
                        }
                        alert("error to update records");
                    },
                    error: function (err) {
                        debugger;
                    }
                });

            }
            catch (ex) {
                debugger;

            }
        }

        function fnResetScanPanel() {
            $('#hdnScanProductId').val('');
            $('#txtScanProductId').val('');
            $('#txtScanCode').val('');
            $('#txtScanProductName').val('');
            $('#txtScanSystemQty').val('');
            $('#txtScanScanQty').val('');
            $('#hdnScanSerialNo').val('');
            if ($("body").height() > $(window).height()) {
                window.scrollTo(0, 100);
            }
        }

        function fnGetScannedProductLogs() {
            debugger;
            try {
                var voucherId = $('#hdnScanVoucherId').val();
                $('#tblScanProducs tbody').empty();
                $('#tblScanProducs').DataTable().clear().destroy();
                if (voucherId != undefined && voucherId != "" && voucherId != "0") {
                    $.ajax({
                        url: "PhysicalInventory.aspx/GetScanProductList",
                        method: "post",
                        contentType: "application/json; charset=utf-8",
                        data: "{'strVoucherId':'" + voucherId + "',objSearch:null}",
                        success: function (data) {
                            debugger;
                            var result = data.d;
                            if (result != null) {
                                result = JSON.parse(result);
                                $(result.objResultLst).each(function () {
                                    debugger;
                                    var row = $(this);
                                    row = row[0];
                                    var htmlRow = "<tr>";
                                    htmlRow += "<td style='display:none'>" + row.productId + "</td>";
                                    htmlRow += "<td style='text-align: left'>" + row.productCode + "</td>";
                                    htmlRow += "<td style='text-align: left'>" + row.productName + "</td>";
                                    htmlRow += "<td style='text-align: left'>" + row.serialNo + "</td>";
                                    htmlRow += "<td style='text-align: right'>" + row.sysQty + "</td>";
                                    htmlRow += "<td style='text-align: right'>" + row.phyQty + "</td>";
                                    htmlRow = htmlRow + "</tr>";
                                    $('#tblScanProducs > tbody:last-child').append(htmlRow);
                                });
                                fnDrawTblScanProducs();
                            }
                            //alert("Product not found");

                        },
                        error: function (err) {
                            debugger;
                        }
                    });
                }
            }
            catch (ex) {

            }
        }

        function fnDrawTblScanProducs() {
            debugger;
            //try{
            //    $('#tblScanProducs').dataTable().fnClearTable();
            //    $('#tblScanProducs').dataTable().fnDestroy();
            //}
            //catch(ex)
            //{

            //}
            tblScanProducts = $('#tblScanProducs').DataTable({
                info: true,
                paging: true,
                sort: true,
                searching: true,
                destroy: true,
                draw: true,
                dom: 'Bfrtip',
                buttons: [
                    'copy', 'csv', 'excel', 'pdf', 'print'
                ],
            });
        }
        function fnSearchScannedProductLogs() {
            debugger;
            try {
                var searchOperator = $('#ddlScanOperator').val();
                var searchField = $('#ddlScanSearchFields').val();
                var searchText = $('#txtScanProductTblSearch').val();
                var voucherId = $('#hdnScanVoucherId').val();
                if (searchOperator == "0" || searchText == "" || searchText == undefined || voucherId == "") {
                    return;
                }
                var objSearch = new Object();
                objSearch.searchfieldName = searchField;
                objSearch.searchText = searchText;
                objSearch.searchOperator = searchOperator;



                $('#tblScanProducs tbody').empty();
                $('#tblScanProducs').DataTable().clear().destroy();

                if (voucherId != undefined && voucherId != "" && voucherId != "0") {
                    $.ajax({
                        url: "PhysicalInventory.aspx/GetScanProductList",
                        method: "post",
                        contentType: "application/json; charset=utf-8",
                        data: "{'strVoucherId':'" + voucherId + "', objSearch:" + JSON.stringify(objSearch) + "}",
                        success: function (data) {
                            debugger;
                            var result = data.d;
                            if (result != null) {
                                result = JSON.parse(result);
                                $(result.objResultLst).each(function () {
                                    var row = $(this);
                                    row = row[0];
                                    var htmlRow = "<tr>";
                                    htmlRow += "<td style='display:none'>" + row.productId + "</td>";
                                    htmlRow += "<td style='text-align: left'>" + row.productCode + "</td>";
                                    htmlRow += "<td style='text-align: left'>" + row.productName + "</td>";
                                    htmlRow += "<td style='text-align: left'>" + row.serialNo + "</td>";
                                    htmlRow += "<td style='text-align: right'>" + row.sysQty + "</td>";
                                    htmlRow += "<td style='text-align: right'>" + row.phyQty + "</td>";
                                    htmlRow = htmlRow + "</tr>";
                                    $('#tblScanProducs > tbody:last-child').append(htmlRow);
                                });
                                debugger;
                                fnDrawTblScanProducs();
                            }
                            //alert("Product not found");

                        },
                        error: function (err) {
                            debugger;
                        }
                    });
                }
            }
            catch (ex) {

            }
        }

        function fnMoveScanScreen(btn) {
            debugger;
            switch (btn.id) {
                case "btnScan1to2":
                    $('#divScan1').hide();
                    $('#divScan2').show();
                    if ($("body").height() > $(window).height()) {
                        window.scrollTo(0, 100);
                    }
                    break;
                case "btnScan2to3":
                    $('#divScan2').hide();
                    $('#divScan3').show();
                    break;
                case "btnScan2to1":
                    $('#divScan2').hide();
                    $('#divScan1').show();
                    break;
                case "btnScan3to2":
                    $('#divScan3').hide();
                    $('#divScan2').show();
                    if ($("body").height() > $(window).height()) {
                        window.scrollTo(0, 100);
                    }
                    break;
            }
        }
        $('#txtScanProductId').keyup(function (event) {
            if (event.keyCode == 13) {
                // Cancel the default action, if needed
                event.preventDefault();
                // Trigger the button element with a click
                fnScanProduct();
            }
        });

        $('#txtScanScanQty').keyup(function (event) {
            if (event.keyCode == 13) {
                // Cancel the default action, if needed
                event.preventDefault();
                // Trigger the button element with a click
                fnUpdateScanedProduct();
            }
        });

        function fnScanTabClick() {
            if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
                //$("#Content2").hide();

                $(".logo").hide();
                $(".content-header").hide();
                $('#txtScanProductId').focus();
            }
        }
    </script>
</asp:Content>
