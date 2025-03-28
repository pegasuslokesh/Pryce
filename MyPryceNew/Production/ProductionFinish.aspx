<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="ProductionFinish.aspx.cs" Inherits="Production_ProductionFinish" %>

<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-check"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Production Finish%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Production%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Production Finish%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hdnReportId" runat="server" />
            <asp:Button runat="server" ID="btnBarcodeReport" Style="display: none;" runat="server" data-toggle="modal" data-target="#Div_Barcode" />
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:Button ID="Btn_View_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Return_Modal" Text="View Modal" />
            <asp:HiddenField runat="server" ID="hdnCanPrint" />
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
                    <li style="display: none;" id="Li_Bin"><a href="#Bin" onclick="Li_Tab_Bin()" data-toggle="tab">
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
				 <asp:Label ID="lblTotalRecords" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

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
                                                    <asp:DropDownList ID="ddlFieldName" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Voucher No. %>" Value="Job_No" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="Job Creation Date" Value="Job_Creation_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Request No%>" Value="Request_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Request Date%>" Value="Request_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Order No.%>" Value="Order_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Customer Name %>" Value="Customername"></asp:ListItem>
                                                        <asp:ListItem Text="Is Cancel" Value="Is_Cancel"></asp:ListItem>
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
                                                        <asp:TextBox ID="txtValueDate" runat="server" CssClass="form-control" Visible="false" placeholder="Search From Date"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendartxtValueDate" runat="server" TargetControlID="txtValueDate" />
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


                                <div class="box box-warning box-solid" <%= GvProductionProcess.Rows.Count>0?"style='display:block'":"style='display:none'"%>>


                                    <div class="box-header with-border">
                                        <label>Barcode Size : </label>
                                        <asp:DropDownList ID="ddlbarcodesize" runat="server" Style="color: black !important;">
                                            <asp:ListItem Text="52*25" Value="52*25"></asp:ListItem>
                                            <asp:ListItem Text="102*105" Value="102*105"></asp:ListItem>
                                            <asp:ListItem Text="60*60" Value="60*60"></asp:ListItem>
                                        </asp:DropDownList>



                                        <h3 class="box-title"></h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvProductionProcess" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True"
                                                        OnPageIndexChanging="GvProductionProcess_PageIndexChanging" AllowSorting="True"
                                                        OnSorting="GvProductionProcess_Sorting">

                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Action">

                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnPrint" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Id") %>' OnCommand="IbtnPrint_Command"><i class="fa fa-print"></i>Print</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnPrintVoucher" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Id") %>' OnCommand="IbtnPrintVoucher_Command"><i class="fa fa-print"></i>Print Voucher</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnPrintbarcode" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Id") %>' OnCommand="IbtnPrintbarcode_Command"><i class="fa fa-print"></i>Print Barcode</asp:LinkButton>
                                                                            </li>


                                                                            <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("Id") %>' OnCommand="lnkViewDetail_Command" CausesValidation="False"><i class="fa fa-eye"></i>View</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Id") %>' CommandName='<%# Eval("Field2") %>' OnCommand="btnEdit_Command" CausesValidation="False"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Id") %>' CommandName='<%# Eval("Is_Post") %>' OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>
                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:BoundField DataField="Job_No" HeaderText="<%$ Resources:Attendance,Voucher No. %>"
                                                                SortExpression="Job_No" />
                                                            <asp:TemplateField SortExpression="Job_Creation_Date" HeaderText="Job Creation Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvDate" runat="server" Text='<%# GetDate(Eval("Job_Creation_Date").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="Request_No" HeaderText="Request No.">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblrequestNo" runat="server" Text='<%#Eval("Request_No") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="Request_Date" HeaderText="Request Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblrequestdate" runat="server" Text='<%#GetDate(Eval("Request_Date").ToString())%>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order No. %>" SortExpression="Order_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblorderno" runat="server" Text='<%# Eval("Order_No")%>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Name %>" SortExpression="Customername">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblcustomername" runat="server" Text='<%# Eval("Customername") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Is Finish" SortExpression="Is_Production_Finish">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblisProductionfinish" runat="server" Text='<%# Eval("Is_Production_Finish") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Location%>" SortExpression="From_Location_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFromLocation" runat="server" Text='<%# Eval("From_Location_Name").ToString() %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
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
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblPINo" runat="server" Text="<%$ Resources:Attendance,Voucher No. %>"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator3" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtPINo" ErrorMessage="<%$ Resources:Attendance,Enter Voucher No %>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtPINo" runat="server" CssClass="form-control"></asp:TextBox>

                                                        <asp:HiddenField ID="hdnrequestid" runat="server" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblReqLocation" runat="server" Text="<%$ Resources:Attendance,Request Location %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control" Enabled="false">
                                                        </asp:DropDownList>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Request No %>"></asp:Label>
                                                        <asp:TextBox ID="txtRequestNo" runat="server" CssClass="form-control"
                                                            ReadOnly="true" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Attendance,Request Date %>"></asp:Label>
                                                        <asp:TextBox ID="txtRequestDate" runat="server" CssClass="form-control"
                                                            ReadOnly="true" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblCustomerName" runat="server" Text="<%$ Resources:Attendance,Customer Name %>" />
                                                        <asp:TextBox ID="txtCustomer" runat="server" CssClass="form-control" ReadOnly="true" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblSONo" runat="server" Text="<%$ Resources:Attendance,Order No. %>"></asp:Label>
                                                        <asp:TextBox ID="txtSONo" runat="server" CssClass="form-control" ReadOnly="true" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblSODate" runat="server" Text="<%$ Resources:Attendance,Order Date %>"></asp:Label>
                                                        <asp:TextBox ID="txtSODate" runat="server" CssClass="form-control" ReadOnly="true" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label4" runat="server" Text="Job creation date"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator1" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtjobcreationdate" ErrorMessage="<%$ Resources:Attendance,Enter Job creation date%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtjobcreationdate" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" TargetControlID="txtjobcreationdate" />

                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label7" runat="server" Text="Exp. Job end date"></asp:Label>
                                                        <a style="color: Red">*</a>
                                                        <asp:RequiredFieldValidator EnableClientScript="true" Style="float: right;" runat="server" ID="RequiredFieldValidator2" ValidationGroup="Save"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtexpjobenddate" ErrorMessage="<%$ Resources:Attendance,Enter Exp. Job end date%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtexpjobenddate" runat="server" CssClass="form-control" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender2" runat="server" TargetControlID="txtexpjobenddate" />

                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label8" runat="server" Text="Job start date"></asp:Label>
                                                        <asp:TextBox ID="txtjobstartdate" runat="server" CssClass="form-control"
                                                            ReadOnly="true" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="Label9" runat="server" Text="Job end date"></asp:Label>
                                                        <asp:TextBox ID="txtjobenddate" runat="server" CssClass="form-control"
                                                            ReadOnly="true" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblDesription" runat="server" Text="<%$ Resources:Attendance,Remark %>" />
                                                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine"
                                                            CssClass="form-control" Font-Names="Arial" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:CheckBox ID="ChkisQualitycheck" runat="server" Text="Is Quality Check" />
                                                        <asp:CheckBox ID="chkCancel" Style="margin-left: 20px" runat="server" Text="Is Cancel" Visible="false" />
                                                        <asp:CheckBox ID="chkpost" Style="margin-left: 20px" runat="server" Visible="false" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <cc1:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="0" CssClass="ajax__tab_yuitabview-theme"
                                                            Width="100%">
                                                            <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="<%$ Resources:Attendance,Product Detail %>">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_TabPanel1" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <div style="overflow: auto">
                                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvProduct" runat="server" Width="100%" AutoGenerateColumns="False">

                                                                                            <Columns>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Serial No %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblSerialNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'
                                                                                                            Visible="false"></asp:Label>
                                                                                                        <asp:Label ID="lblSNo" runat="server" Text='<%#Container.DataItemIndex+1 %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblproductcode" runat="server" Text='<%# ProductCode(Eval("ProductId").ToString()) %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblPID" Visible="false" runat="server" Text='<%# Eval("ProductId") %>'></asp:Label>
                                                                                                        <asp:Label ID="lblProductId" runat="server" Text='<%# ProductName(Eval("ProductId").ToString())%>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Name %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblUID" Visible="false" runat="server" Text='<%# Eval("UnitId") %>'></asp:Label>
                                                                                                        <asp:Label ID="lblUnit" runat="server" Text='<%# UnitName(Eval("UnitId").ToString()) %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Quantity %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblReqQty" runat="server" Text='<%# SetDecimal(Eval("Production_Qty").ToString()) %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>


                                                                                                <asp:TemplateField HeaderText="Extra Qty">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="txtExtraQty" runat="server" Text='<%# SetDecimal(Eval("Extra_Qty").ToString()) %>' AutoPostBack="true" OnTextChanged="txtExtraQty_TextChanged"></asp:TextBox>

                                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender39" runat="server" Enabled="True"
                                                                                                            TargetControlID="txtExtraQty" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>



                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Price %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblunitPrice" runat="server" Text='<%# SetDecimal(Eval("UnitPrice").ToString()) %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>

                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Cost %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblunitCost" runat="server" Text='<%# SetDecimal(Eval("UnitPrice").ToString()) %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Line Total %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblLinetotal" runat="server" Text='<%#SetDecimal((Convert.ToDouble(Eval("UnitPrice").ToString())*(Convert.ToDouble(Eval("Production_Qty").ToString())+Convert.ToDouble(Eval("Extra_Qty").ToString()))  ).ToString())%>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                            </Columns>

                                                                                            <PagerStyle CssClass="pagination-ys" />

                                                                                        </asp:GridView>
                                                                                        <asp:HiddenField ID="hdnProductId" runat="server" />
                                                                                        <asp:HiddenField ID="hdnProductName" runat="server" />
                                                                                        <asp:HiddenField ID="hdnTransType" runat="server" />
                                                                                    </div>
                                                                                    <br />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Update_TabPanel1">
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
                                                            <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="<%$ Resources:Attendance,Bill Of Material%>">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_TabPanel2" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <div style="overflow: auto; max-height: 500px;">
                                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvBom" runat="server" AutoGenerateColumns="False" Width="100%">

                                                                                            <Columns>
                                                                                                <asp:TemplateField>
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblTransId" runat="server" Visible="false" Text='<%# Eval("Id") %>'></asp:Label>
                                                                                                        <asp:Label ID="lblgvsNo" runat="server" Text='<%#Container.DataItemIndex+1 %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id%>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvProductCode" runat="server" Text='<%#ProductCode(Eval("Item_Id").ToString()) %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center"  />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:HiddenField ID="hdngvProductId" runat="server" Value='<%#Eval("Item_Id") %>' />
                                                                                                        <asp:Label ID="lblgvProductName" runat="server" Text='<%#Eval("ShortDescription") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvUnit" runat="server" Text='<%#UnitName(Eval("Unit_Id").ToString()) %>' />
                                                                                                        <asp:HiddenField ID="hdngvUnitId" runat="server" Value='<%#Eval("Unit_Id") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="System quantity">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvSystemQuantity" runat="server" Text='<%#SetDecimal(Eval("Sys_qty").ToString()) %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center"  />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Request Quantity %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblRequestquantity" runat="server" Text='<%#SetDecimal(Eval("Req_Qty").ToString()) %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center"  />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Received Quantity">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="txtReceivedqty"  Width ="70px" runat="server" Text='<%#SetDecimal(Eval("Rec_Qty").ToString()) %>' OnTextChanged="txtReceivedqty_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center"  />
                                                                                                </asp:TemplateField>

                                                                                                <asp:TemplateField HeaderText="Waste Quantity">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="txtwasteqty"  runat="server" Width ="70px" Text='<%#SetDecimal(Eval("wst_Qty").ToString()) %>'  AutoPostBack="true"></asp:TextBox>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center"   />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Price %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvUnitPrice" runat="server" Text='<%#SetDecimal(Eval("Unit_Price").ToString()) %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center"  />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Line Total">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvtotal" runat="server" Text='<%#SetDecimal(Eval("Line_Total").ToString()) %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center"  />
                                                                                                </asp:TemplateField>
                                                                                            </Columns>



                                                                                        </asp:GridView>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_TabPanel2">
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

                                                            <cc1:TabPanel ID="TabPanel5" runat="server" HeaderText="<%$ Resources:Attendance,Issue Roll%>">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_TabPanel5" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblserialno" runat="server" Text="<%$ Resources:Attendance,Serial No %>"></asp:Label>
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtSNo" runat="server" CssClass="form-control" BackColor="#eeeeee"></asp:TextBox>

                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" CompletionInterval="100"
                                                                                            DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListSerialNumber"
                                                                                            ServicePath="" TargetControlID="txtSNo" UseContextKey="True" CompletionListCssClass="completionList"
                                                                                            CompletionListItemCssClass="listItem" CompletionListHighlightedItemCssClass="itemHighlighted">
                                                                                        </cc1:AutoCompleteExtender>
                                                                                        <div class="input-group-btn">
                                                                                            <asp:Button ID="btnadd" runat="server" OnClick="btnadd_OnClick" Text="Add" CssClass="btn btn-primary" />
                                                                                        </div>
                                                                                    </div>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <div style="overflow: auto; max-height: 500px;">
                                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvReturn" runat="server" AutoGenerateColumns="False" Width="100%" ShowFooter="true">

                                                                                            <Columns>
                                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:ImageButton ID="IbtnPDDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                                            ImageUrl="~/Images/Erase.png" OnCommand="IbtnPDDelete_Command" ToolTip="<%$ Resources:Attendance,Delete %>" />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                    <ItemStyle />
                                                                                                    <FooterStyle BorderStyle="None" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id%>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvProductCode" runat="server" Text='<%#ProductCode(Eval("ProductId").ToString()) %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>">
                                                                                                    <ItemTemplate>
                                                                                                       <asp:HiddenField ID="hdngvProductId" runat="server" Value='<%#Eval("ProductId") %>' />
                                                                                                        <asp:Label ID="lblgvProductName" runat="server" Text='<%#ProductName(Eval("ProductId").ToString())%>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvUnit" runat="server" Text='<%#UnitName(Eval("UnitId").ToString()) %>' />
                                                                                                        <asp:HiddenField ID="hdngvUnitId" runat="server" Value='<%#Eval("UnitId") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Serial No">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblserialNo" runat="server" Text='<%#Eval("SerialNo") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Width">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblwidth" runat="server" Text='<%#Eval("Width") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Length">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblLength" runat="server" Text='<%#Eval("Length").ToString() %>'></asp:Label>
                                                                                                        <asp:Label ID="lblPalletiD" runat="server" Text='<%#Eval("Pallet_ID").ToString() %>'
                                                                                                            Visible="false"></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Quantity">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblqty" runat="server" Text='<%#SetDecimal(Eval("TotalQuantity").ToString()) %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="txttotused" runat="server" Text="Total" Font-Bold="true" />
                                                                                                    </FooterTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Used">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="txtusedqty" runat="server" Text='<%#SetDecimal(Eval("UsedQuantity").ToString()) %>'
                                                                                                            Width="100px" OnTextChanged="txtusedqty_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                                        <cc1:FilteredTextBoxExtender ID="FiltergvtxtReturnquantity" runat="server" Enabled="True"
                                                                                                            TargetControlID="txtusedqty" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>

                                                                                                        <asp:Label ID="txttotUsedShow" runat="server" Font-Bold="true" Text="0" />
                                                                                                    </FooterTemplate>
                                                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                                                    <ItemStyle HorizontalAlign="Right" Width="100px" />
                                                                                                </asp:TemplateField>

                                                                                                <asp:TemplateField HeaderText="Waste">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="txtwasteqty" runat="server" Text='<%#SetDecimal(Eval("WasteQuantity").ToString()) %>'
                                                                                                            Width="100px" OnTextChanged="txtwasteqty_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                                        <cc1:FilteredTextBoxExtender ID="Filtertxtwasteqty" runat="server" Enabled="True"
                                                                                                            TargetControlID="txtwasteqty" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>

                                                                                                        <asp:Label ID="txttotWasteShow" runat="server" Font-Bold="true" Text="0" />
                                                                                                    </FooterTemplate>
                                                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                                                    <ItemStyle HorizontalAlign="Right" Width="100px" />
                                                                                                </asp:TemplateField>

                                                                                            </Columns>



                                                                                        </asp:GridView>
                                                                                    </div>
                                                                                    <br />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress10" runat="server" AssociatedUpdatePanelID="Update_TabPanel5">
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

                                                            <cc1:TabPanel ID="TabPanel3" runat="server" HeaderText="<%$ Resources:Attendance,Job Detail%>">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_TabPanel3" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <div style="overflow: auto">
                                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvVisitTask" runat="server" AutoGenerateColumns="False" Width="100%">

                                                                                            <Columns>
                                                                                                <asp:TemplateField>
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblsno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                                    <FooterStyle  />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblempname" runat="server" Text='<%#Eval("Emp_Name") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                                 <asp:TemplateField HeaderText="Job Date">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblJobdate" runat="server" Text='<%#GetDate(Eval("Job_Date").ToString()) %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                    <FooterStyle  HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>

                                                                                                <asp:TemplateField HeaderText="Start Time">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblstartdate" runat="server" Text='<%#Eval("Start_Time")%>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                    <FooterStyle  HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Stop Time">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblEnddate" runat="server" Text='<%#Eval("Stop_Time")%>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center"  />
                                                                                                    <FooterStyle  HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Duration %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblduration" runat="server" Text='<%#Eval("Duration") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center"  />
                                                                                                    <FooterStyle  HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Machine Name">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblMachineName" runat="server" Text='<%#Eval("Machine_Name") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center"  />
                                                                                                    <FooterStyle  HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                            </Columns>



                                                                                        </asp:GridView>
                                                                                    </div>
                                                                                    <br />
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress7" runat="server" AssociatedUpdatePanelID="Update_TabPanel3">
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
                                                            <cc1:TabPanel ID="TabPanel4" runat="server" HeaderText="<%$ Resources:Attendance,Expenses Information%>">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_TabPanel4" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Attendance,Currency %>"></asp:Label>
                                                                                    <asp:DropDownList ID="ddlExpCurrency" runat="server" CssClass="form-control"
                                                                                        AutoPostBack="True" OnSelectedIndexChanged="ddlExpCurrency_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Attendance,Exchange Rate %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtExpExchangeRate" runat="server" CssClass="form-control">0</asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender41" runat="server" Enabled="True"
                                                                                        TargetControlID="txtExpExchangeRate" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblSelectExp" runat="server" Text="<%$ Resources:Attendance,Select Expenses %>" />
                                                                                    <asp:DropDownList ID="ddlExpense" runat="server" AutoPostBack="True"
                                                                                        OnSelectedIndexChanged="ddlExpense_SelectedIndexChanged" CssClass="form-control">
                                                                                    </asp:DropDownList>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblExpAccount" runat="server" Text="<%$ Resources:Attendance, Expenses Account %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtExpensesAccount" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                                        OnTextChanged="txtExpensesAccount_TextChanged" BackColor="#eeeeee" />
                                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                                                        Enabled="True" ServiceMethod="GetCompletionListAccountNo" ServicePath="" CompletionInterval="100"
                                                                                        MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtExpensesAccount"
                                                                                        UseContextKey="True" CompletionListCssClass="completionList" CompletionListItemCssClass="listItem"
                                                                                        CompletionListHighlightedItemCssClass="itemHighlighted">
                                                                                    </cc1:AutoCompleteExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblFCExpAmount" runat="server" Text="Expenses Amount"></asp:Label>
                                                                                    <asp:TextBox ID="txtFCExpAmount" runat="server" CssClass="form-control"
                                                                                        AutoPostBack="true" OnTextChanged="txtFCExpAmount_TextChanged">0</asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender40" runat="server" Enabled="True"
                                                                                        TargetControlID="txtFCExpAmount" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <asp:Label ID="lblExpCharges" runat="server" Text="Expenses Amount (Local)" />
                                                                                    <asp:TextBox ID="txtExpCharges" runat="server" ReadOnly="true" CssClass="form-control">0</asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender42" runat="server" Enabled="True"
                                                                                        TargetControlID="txtExpCharges" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-12" style="text-align: center">
                                                                                    <%--<asp:ImageButton runat="server" CausesValidation="False" ImageUrl="~/Images/add.png"
                                                                                                Height="29px" ToolTip="<%$ Resources:Attendance,Add %>" Width="35px" ID="IbtnAddExpenses"
                                                                                                OnClick="IbtnAddExpenses_Click"></asp:ImageButton>--%>
                                                                                    <asp:Button ID="IbtnAddExpenses" runat="server" OnClick="IbtnAddExpenses_Click" CssClass="btn btn-info" Text="<%$ Resources:Attendance,Add %>" />
                                                                                    <asp:HiddenField ID="hdnProductExpenses" runat="server" Value="0" />
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <asp:HiddenField ID="hdnfPSC" runat="server" />
                                                                                    <div style="overflow: auto; max-height: 500px;">
                                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GridExpenses" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                                                                            BorderStyle="Solid" Width="100%">

                                                                                            <Columns>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>" Visible="false">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:ImageButton ID="IbtnDeleteExp" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Expense_Id") %>'
                                                                                                            ImageUrl="~/Images/Erase.png" Width="16px" ToolTip="<%$ Resources:Attendance,Delete %>"
                                                                                                            OnCommand="IbtnDeleteExp_Command" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterStyle BorderStyle="None" />
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Expenses %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvExpense_Id" runat="server" Text='<%# GetExpName(Eval("Expense_Id").ToString()) %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterStyle BorderStyle="None" />
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Currency %>" Visible="false">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvExpCurrencyID" runat="server" Text='<%# CurrencyName(Eval("ExpCurrencyID").ToString()) %>' />
                                                                                                        <asp:Label ID="hidExpCur" runat="server" Visible="false" Text='<%# Eval("ExpCurrencyID").ToString() %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterStyle BorderStyle="None" />
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Expenses Amount">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvFCExchangeAmount" runat="server" Text='<%# Eval("FCExpAmount") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterStyle BorderStyle="None" />
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Exchange Rate %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvExpExchangeRate" runat="server" Text='<%# Eval("ExpExchangeRate") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lbltotExp" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Expenses%>" /><b>:</b>
                                                                                                    </FooterTemplate>
                                                                                                    <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Expenses Amount (Local)" SortExpression="Exp_Charges">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvExp_Charges" runat="server" Text='<%# Eval("Exp_Charges") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="txttotExp" runat="server" Visible="false" Font-Bold="true" Text="0" />
                                                                                                        <asp:Label ID="txttotExpShow" runat="server" Font-Bold="true" Text="0" />
                                                                                                    </FooterTemplate>
                                                                                                    <FooterStyle BorderStyle="None" />
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                            </Columns>




                                                                                        </asp:GridView>
                                                                                    </div>
                                                                                    <br />
                                                                                </div>

                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:UpdateProgress ID="UpdateProgress8" runat="server" AssociatedUpdatePanelID="Update_TabPanel4">
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
                                                        </cc1:TabContainer>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnPost" runat="server" ValidationGroup="Save" Text="<%$ Resources:Attendance,Post %>" CssClass="btn btn-primary"
                                                            OnClick="btnPost_Click" Visible="false" />
                                                        <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="Are you sure to post the record ?"
                                                            TargetControlID="btnPost">
                                                        </cc1:ConfirmButtonExtender>

                                                        <asp:Button ID="btnPISave" ValidationGroup="Save" runat="server" Text="<%$ Resources:Attendance,Save %>" Visible="false"
                                                            CssClass="btn btn-success" OnClick="btnPISave_Click" />

                                                        <asp:Button ID="BtnReset" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                            CssClass="btn btn-primary" CausesValidation="False" OnClick="BtnReset_Click" />

                                                        <asp:Button ID="btnPICancel" runat="server" CssClass="btn btn-danger" Text="<%$ Resources:Attendance,Cancel %>"
                                                            CausesValidation="False" OnClick="btnPICancel_Click" />

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
                    <div style="display: none;" class="tab-pane" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server">
                            <ContentTemplate>
                                <div class="alert alert-info ">
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="ddlFieldNameBin" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlFieldNameBin_SelectedIndexChanged"
                                                    AutoPostBack="true">
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Voucher No. %>" Value="Job_No" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Job Creation Date" Value="Job_Creation_Date"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Request No%>" Value="Request_No"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Request Date%>" Value="Request_Date"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Order No.%>" Value="Order_No"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Attendance,Customer Name %>" Value="Customername"></asp:ListItem>
                                                    <asp:ListItem Text="Is Cancel" Value="Is_Cancel"></asp:ListItem>
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
                                            <div class="col-lg-2">
                                                <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbindBin">
                                                    <asp:TextBox ID="txtValueBin" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <asp:TextBox ID="txtValueBinDate" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueBinDate" runat="server" TargetControlID="txtValueBinDate" />
                                                </asp:Panel>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:ImageButton ID="btnbindBin" runat="server" CausesValidation="False" Style="margin-top: -5px;"
                                                    ImageUrl="~/Images/search.png" OnClick="btnbindBin_Click"
                                                    ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>

                                                <asp:ImageButton ID="btnRefreshBin" runat="server" CausesValidation="False" Style="width: 33px;"
                                                    ImageUrl="~/Images/refresh.png" OnClick="btnRefreshBin_Click"
                                                    ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>

                                                <asp:ImageButton ID="imgBtnRestore" Style="width: 33px;" CausesValidation="False"
                                                    runat="server" ImageUrl="~/Images/active.png" OnClick="imgBtnRestore_Click"
                                                    ToolTip="<%$ Resources:Attendance, Active %>" />

                                                <asp:ImageButton ID="ImgbtnSelectAll" Visible="false" runat="server" OnClick="ImgbtnSelectAll_Click"
                                                    Style="width: 33px;" ToolTip="<%$ Resources:Attendance, Select All %>" AutoPostBack="true"
                                                    ImageUrl="~/Images/selectAll.png" />
                                            </div>
                                            <div class="col-lg-2">
                                                <h5>
                                                    <asp:Label ID="lblTotalRecordsBin" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label></h5>
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
                                                    <asp:Label ID="lblSelectedRecord" runat="server" Visible="false"></asp:Label>
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvProductionProcessBin" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True"
                                                        OnPageIndexChanging="GvProductionProcessBin_PageIndexChanging" OnSorting="GvProductionProcessBin_OnSorting"
                                                        AllowSorting="true">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkCurrent" runat="server" OnCheckedChanged="chkCurrent_CheckedChanged"
                                                                        AutoPostBack="true" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:HiddenField ID="hdnTransId" runat="server" Value='<%#Eval("Id") %>' />
                                                                    <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelect_CheckedChanged" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Job_No" HeaderText="<%$ Resources:Attendance,Voucher No. %>"
                                                                SortExpression="Job_No" />
                                                            <asp:TemplateField SortExpression="Job_Creation_Date" HeaderText="Job Creation Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvDate" runat="server" Text='<%# GetDate(Eval("Job_Creation_Date").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="Request_No" HeaderText="Request No.">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblrequestNo" runat="server" Text='<%#Eval("Request_No") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="Request_Date" HeaderText="Request Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblrequestdate" runat="server" Text='<%#GetDate(Eval("Request_Date").ToString())%>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order No. %>" SortExpression="Order_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblorderno" runat="server" Text='<%# Eval("Order_No")%>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Name %>" SortExpression="Customername">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblcustomername" runat="server" Text='<%# Eval("Customername") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Is Cancel" SortExpression="Is_Cancel">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvcancel" runat="server" Text='<%# Eval("Is_Cancel").ToString()%>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
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

    <div class="modal fade" id="Return_Modal" tabindex="-1" role="dialog" aria-labelledby="Return_ModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Return_ModalLabel">Serial No</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Modal" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="hdnSalesOrderId" runat="server" Value="0" />
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-12">
                                                    <asp:Label ID="lblproductId" runat="server" Text="Product Id"></asp:Label>
                                                    &nbsp:&nbsp<asp:Label ID="lblProductIdvalue" runat="server" Text="0"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <asp:Label ID="lblserialProductname" runat="server" Text="Product Name"></asp:Label>
                                                    &nbsp:&nbsp<asp:Label ID="lblProductNameValue" runat="server"
                                                        Text="0"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <asp:Label ID="Label6222" runat="server" Text="Request Qty."></asp:Label>
                                                    &nbsp:&nbsp<asp:Label ID="lblpnlInvoiceQty" runat="server"
                                                        Text="0"></asp:Label>
                                                    <br />
                                                </div>
                                                <div class="col-md-12">
                                                    <hr />
                                                </div>
                                                <div id="pnlSerialNumber" runat="server" class="col-md-12">
                                                    <div class="col-md-6">
                                                        <br />
                                                        <asp:Label ID="Label30" runat="server" Text="<%$ Resources:Attendance, File Upload%>"></asp:Label>
                                                        <div class="input-group" style="width: 100%;">
                                                            <cc1:AsyncFileUpload ID="FULogoPath"
                                                                OnClientUploadStarted="FUAll_UploadStarted"
                                                                OnClientUploadError="FUAll_UploadError"
                                                                OnClientUploadComplete="FUAll_UploadComplete"
                                                                OnUploadedComplete="FUAll_FileUploadComplete"
                                                                runat="server" CssClass="form-control"
                                                                CompleteBackColor="White"
                                                                UploaderStyle="Traditional"
                                                                UploadingBackColor="#CCFFFF"
                                                                ThrobberID="FUAll_ImgLoader" Width="100%" />
                                                            <div class="input-group-btn" style="border: solid 1px #d2d6de; width: 35px;">
                                                                <asp:Image ID="FUAll_Img_Right" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Allow.png" Style="display: none" />
                                                                <asp:Image ID="FUAll_Img_Wrong" runat="server" Width="30px" Height="30px" ImageUrl="../Images/Delete1.png" Style="display: none" />
                                                                <asp:Image ID="FUAll_ImgLoader" runat="server" ImageUrl="../Images/loader.gif" />
                                                            </div>
                                                        </div>

                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" style="text-align: center">
                                                        <br />
                                                        <br />
                                                        <asp:Button ID="Btnloadfile" runat="server" Text="Load" CssClass="btn btn-primary"
                                                            OnClick="Btnloadfile_Click" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="alert alert-info ">
                                                            <div class="row">
                                                                <div class="form-group">
                                                                    <div class="col-lg-3">
                                                                        <asp:TextBox ID="txtserachserialnumber" runat="server" CssClass="form-control"></asp:TextBox>
                                                                        <cc1:TextBoxWatermarkExtender ID="txtwatermarkup" runat="server" TargetControlID="txtserachserialnumber"
                                                                            WatermarkText="Search Serial Number">
                                                                        </cc1:TextBoxWatermarkExtender>
                                                                    </div>
                                                                    <div class="col-lg-2">
                                                                        <asp:ImageButton ID="btnsearchserial" runat="server" CausesValidation="False"
                                                                            ImageUrl="~/Images/search.png" OnClick="btnsearchserial_Click" Style="margin-top: -5px;" ToolTip="<%$ Resources:Attendance,Search %>"></asp:ImageButton>

                                                                        <asp:ImageButton ID="btnRefreshserial" runat="server" CausesValidation="False"
                                                                            ImageUrl="~/Images/refresh.png" OnClick="btnRefreshserial_Click" Style="width: 33px;"
                                                                            ToolTip="<%$ Resources:Attendance,Refresh %>"></asp:ImageButton>
                                                                    </div>
                                                                    <div class="col-lg-2">
                                                                        <h5>
                                                                            <asp:Label ID="Label31" runat="server" Text="Total :"></asp:Label>
                                                                            <asp:Label ID="txtselectedSerialNumber" runat="server" Text="0"></asp:Label></h5>
                                                                    </div>
                                                                    <div class="col-lg-2">
                                                                        <h5>
                                                                            <asp:Label ID="lblCount" runat="server"></asp:Label>
                                                                            <asp:Label ID="txtCount" runat="server" Text="0"></asp:Label></h5>
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
                                                                    <div class="col-md-8">
                                                                        <div class="flow">
                                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvSerialNumber" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                                                                AllowSorting="true" BorderStyle="Solid" Width="100%"
                                                                                PageSize="5" OnSorting="gvSerialNumber_OnSorting">
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Delete %>">
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("SerialNo") %>'
                                                                                                ImageUrl="~/Images/Erase.png" OnCommand="IbtnDeleteserialNumber_Command" Width="16px" />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Serial Number" SortExpression="SerialNo">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblsrno" runat="server" Text='<%#Eval("SerialNo") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Manufacturing Date">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblmfg" runat="server" Text='<%#Eval("ManufacturerDate") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Batch No.">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblBatchNo" runat="server" Text='<%#Eval("BatchNo") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                </Columns>


                                                                                <PagerStyle CssClass="pagination-ys" />

                                                                            </asp:GridView>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <asp:TextBox ID="txtSerialNo" Height="350px" runat="server" CssClass="form-control"
                                                                            TextMode="MultiLine"></asp:TextBox>
                                                                    </div>
                                                                    <div id="pnlexp_and_Manf" runat="server" visible="false" class="col-md-12">
                                                                        <div style="overflow: auto; max-height: 500px;">
                                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvStockwithManf_and_expiry" ShowHeader="true" runat="server" AutoGenerateColumns="false"
                                                                                Width="100%" DataKeyNames="ProductId" ShowFooter="true" OnRowDeleting="gridView_RowDeleting"
                                                                                OnRowCommand="gridView_RowCommand">

                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="Quantity">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblQuantity" runat="server" Text='<%#setRoundValue(Eval("Quantity").ToString()) %>'></asp:Label>
                                                                                            <asp:HiddenField ID="hdnProductId" runat="server" Value='<%#Eval("ProductId") %>' />
                                                                                            <asp:HiddenField ID="hdnOrderId" runat="server" Value='<%#Eval("POID") %>' />
                                                                                        </ItemTemplate>
                                                                                        <EditItemTemplate>
                                                                                        </EditItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtQuantity" runat="server" Font-Names="Verdana"
                                                                                                CausesValidation="true" Width="250px"></asp:TextBox>
                                                                                            <cc1:FilteredTextBoxExtender ID="filtertextbox" runat="server" TargetControlID="txtQuantity"
                                                                                                FilterType="Numbers">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                        </FooterTemplate>
                                                                                        <ItemStyle Width="10%" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Expiry Date">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblExpiryDate" runat="server" Text='<%#setDateTime(Eval("ExpiryDate").ToString()) %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <EditItemTemplate>
                                                                                        </EditItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtExpiryFooter" runat="server" Font-Names="Verdana"
                                                                                                Text='<%#Eval("ExpiryDate") %>' CausesValidation="true" Width="250px"></asp:TextBox>
                                                                                            <cc1:CalendarExtender ID="txtExpiryFooter_CalenderExtender" runat="server" Enabled="True"
                                                                                                TargetControlID="txtExpiryFooter" Format="dd-MMM-yyyy">
                                                                                            </cc1:CalendarExtender>
                                                                                        </FooterTemplate>
                                                                                        <ItemStyle Width="8%" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Manufacturing Date">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblTaxValue" runat="server" Text='<%#setDateTime(Eval("ManufacturerDate").ToString()) %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <EditItemTemplate>
                                                                                        </EditItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtManfacturerFooter" runat="server" Font-Names="Verdana"
                                                                                                Text='<%#Eval("ManufacturerDate") %>' CausesValidation="true" Width="250px"></asp:TextBox>
                                                                                            <cc1:CalendarExtender ID="txtManfacturerFooter_CalenderExtender" runat="server" Enabled="True"
                                                                                                TargetControlID="txtManfacturerFooter" Format="dd-MMM-yyyy">
                                                                                            </cc1:CalendarExtender>
                                                                                        </FooterTemplate>
                                                                                        <ItemStyle Width="8%" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <EditItemTemplate>
                                                                                            <asp:Button ID="ButtonUpdate" runat="server" CommandName="Update" Text="Update" CausesValidation="true"
                                                                                                CommandArgument='<%#Eval("Trans_Id") %>' />
                                                                                            <asp:Button ID="ButtonCancel" runat="server" CommandName="Cancel" Text="Cancel" />
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Button ID="ButtonEdit" runat="server" CommandName="Edit" Text="Edit" Visible="false" />
                                                                                            <asp:ImageButton ID="IbtnDeletePay" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                                CommandName="Delete" ImageUrl="~/Images/Erase.png" ToolTip="<%$ Resources:Attendance,Delete %>"
                                                                                                Width="16px" />
                                                                                            <%--<asp:Button ID="ButtonDelete" runat="server" CommandName="Delete"  Text="Delete" CommandArgument='<%#Eval("Trans_Id") %>'  />--%>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:Panel ID="pnlIbtnAddProductSupplierCode" runat="server" DefaultButton="IbtnAddProductSupplierCode">
                                                                                                <asp:ImageButton ID="IbtnAddProductSupplierCode" runat="server" CausesValidation="False"
                                                                                                    CommandName="AddNew" Height="29px" ImageUrl="~/Images/add.png" Width="35px" ToolTip="<%$ Resources:Attendance,Add %>" />
                                                                                            </asp:Panel>
                                                                                            <%--<asp:Button ID="ButtonAdd" runat="server" CommandName="AddNew"  Text="Add New Row"  />--%>
                                                                                        </FooterTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                </Columns>



                                                                            </asp:GridView>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
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
                <div class="modal-footer">
                    <asp:UpdatePanel ID="Update_Modal_Button" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="BtnSerialSave" runat="server" Text="<%$ Resources:Attendance,Save %>"
                                CssClass="btn btn-success" OnClick="BtnSerialSave_Click" />
                            <asp:Button ID="btnResetSerial" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                CssClass="btn btn-primary" OnClick="btnResetSerial_Click" />
                            <asp:HiddenField ID="hdnusercontrolProductid" runat="server" />
                            <asp:HiddenField ID="hdnusercontrolrowindex" runat="server" />
                            <asp:HiddenField ID="hdnusercontrolsalesorderid" runat="server" />
                            <asp:HiddenField ID="hdnOrderId" runat="server" />
                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                Close</button>
                            <asp:Button ID="btnDefault" runat="server" Style="visibility: hidden" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="Div_Barcode" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" data-backdrop="static" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-body">
                    <asp:UpdatePanel ID="uptaskContrcat" runat="server">
                        <ContentTemplate>
                            <dx:ASPxWebDocumentViewer ID="ReportViewer1" runat="server" Width="100%"></dx:ASPxWebDocumentViewer>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal" onclick="btnCloseReport()">
                        Close</button>
                </div>
            </div>
        </div>
    </div>






    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Modal">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress9" runat="server" AssociatedUpdatePanelID="Update_Modal_Button">
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

        function LI_Edit_Request_Active() {
            $("#Li_Request").removeClass("active");
            $("#Request").removeClass("active");

            $("#Li_New").addClass("active");
            $("#New").addClass("active");
        }
        function Li_Tab_Bin() {
            document.getElementById('<%= Btn_Bin.ClientID %>').click();
        }

        function Close_Modal_Popup() {
            document.getElementById('<%= Btn_View_Modal.ClientID %>').click();
        }
        function Show_Modal_Popup() {
            document.getElementById('<%= Btn_View_Modal.ClientID %>').click();
        }

    </script>
    <script type="text/javascript">
        function count(clientId) {
            var txtInput = document.getElementById(clientId);
            if (event.keyCode == 13) {
                document.getElementById('<%= txtCount.ClientID %>').innerHTML = lineBreakCount(txtInput.value);
            }
            if (event.keyCode == 8 || event.keyCode == 46) {
                document.getElementById('<%= txtCount.ClientID %>').innerHTML = lineDelBreakCount(txtInput.value); // The button id over here
            }
        }
        function lineBreakCount(str) {
            try {
                return ((str.match(/[^\n]*\n[^\n]*/gi).length) + 1);
            } catch (e) {
                return 1;
            }
        }
        function lineDelBreakCount(str) {
            try {
                return ((str.match(/[^\n]*\n[^\n]*/gi).length - 1));
            } catch (e) {
                return 0;
            }
        }
        function checkDec(el) {
            var ex = /^[0-9]+\.?[0-9]*$/;
            if (ex.test(el.value) == false) {
                el.value = "";
                alert('Incorrect Number');
            }
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

        function openReport() {
            document.getElementById('<%= btnBarcodeReport.ClientID%>').click();
        }

        function btnCloseReport() {
            document.getElementById('<%= hdnReportId.ClientID%>').value = "";
        }




    </script>
</asp:Content>

