<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="ProductionProcess.aspx.cs" Inherits="Production_ProductionProcess" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-sync"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Production Process%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Inventory%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Production%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Production Process%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Modal_FileUpload" Style="display: none;" runat="server" data-toggle="modal" data-target="#PartialRequest" Text="Production Back Order" />
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:Button ID="Btn_Request" Style="display: none;" runat="server" OnClick="btnPRequest_Click" Text="Request" />
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
                    <li id="Li_Request"><a href="#Request" onclick="Li_Tab_Request()" data-toggle="tab">
                        <i class="fas fa-user-check"></i>&nbsp;&nbsp;<asp:Label ID="Label2" runat="server" Text="Back Order"></asp:Label></a></li>
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
                                                                                <asp:LinkButton ID="IbtnPrint" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Id") %>'
                                                                                    OnCommand="IbtnPrint_Command" ToolTip="Print Job Order"><i class="fa fa-print"></i>Print Job Order</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnPrintVoucher" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Id") %>'
                                                                                    OnCommand="IbtnPrintVoucher_Command" ToolTip="Print Voucher"><i class="fa fa-print"></i>Print Voucher</asp:LinkButton>
                                                                            </li>



                                                                            <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("Id") %>'
                                                                                    ToolTip="View" OnCommand="lnkViewDetail_Command"
                                                                                    CausesValidation="False"><i class="fa fa-eye"></i>View</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanEdit.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("Id") %>'
                                                                                    CommandName='<%# Eval("Is_Post") %>'
                                                                                    OnCommand="btnEdit_Command" CausesValidation="False"><i class="fa fa-pencil"></i>Edit </asp:LinkButton>
                                                                            </li>
                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Id") %>'
                                                                                    CommandName='<%# Eval("Is_Post") %>'
                                                                                    OnCommand="IbtnDelete_Command"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
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

                                                        <asp:TextBox ID="txtPINo" runat="server" CssClass="form-control" OnTextChanged="txtlRequestNo_TextChanged" AutoPostBack="true"></asp:TextBox>

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

                                                                                                <asp:TemplateField HeaderText="Request Qty">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblReqQty" runat="server" Text='<%# SetDecimal(Eval("Request_Qty").ToString()) %>'></asp:Label>

                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>


                                                                                                <asp:TemplateField HeaderText="Remain Qty">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblRemainQty" runat="server" Text='<%# SetDecimal(Eval("Remain_Qty").ToString()) %>'></asp:Label>

                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>


                                                                                                <asp:TemplateField HeaderText="Process Qty">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="txtReqQty" runat="server" Text='<%# SetDecimal(Eval("Production_Qty").ToString()) %>' AutoPostBack="true" OnTextChanged="txtReqQty_TextChanged"></asp:TextBox>

                                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender39" runat="server" Enabled="True"
                                                                                                            TargetControlID="txtReqQty" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>


                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Price %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblunitPrice" runat="server" Text='<%# Eval("UnitPrice") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Line Total %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblLinetotal" runat="server" Text='<%#SetDecimal((Convert.ToDouble(Eval("UnitPrice").ToString())*Convert.ToDouble(Eval("Production_Qty").ToString())).ToString())%>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                            </Columns>

                                                                                            <PagerStyle CssClass="pagination-ys" />

                                                                                        </asp:GridView>
                                                                                        <asp:HiddenField ID="hdnProductId" runat="server" />
                                                                                        <asp:HiddenField ID="hdnProductName" runat="server" />
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
                                                                                <div class="col-md-6">
                                                                                    <asp:DropDownList ID="ddlProductSerach" runat="server" CssClass="form-control"
                                                                                        OnSelectedIndexChanged="ddlProductSerach_SelectedIndexChanged"
                                                                                        AutoPostBack="true">
                                                                                        <asp:ListItem Text="<%$ Resources:Attendance,Product Id %>" Value="ProductCode"></asp:ListItem>
                                                                                        <asp:ListItem Text="<%$ Resources:Attendance,Product Name %>" Value="ProductName"
                                                                                            Selected="True"></asp:ListItem>
                                                                                        <%-- <asp:ListItem Text="<%$ Resources:Attendance,Sales Order No %>" Value="SalesOrderNo"></asp:ListItem>--%>
                                                                                    </asp:DropDownList>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-6">
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtProductId" Visible="false" runat="server" CssClass="form-control"
                                                                                            AutoPostBack="True" OnTextChanged="txtProductCode_TextChanged" BackColor="#eeeeee" />
                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" CompletionInterval="100"
                                                                                            DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductCode"
                                                                                            ServicePath="" TargetControlID="txtProductId" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                        </cc1:AutoCompleteExtender>
                                                                                        <asp:TextBox ID="txtSearchProductName" runat="server" BackColor="#eeeeee" CssClass="form-control"
                                                                                            AutoPostBack="True" OnTextChanged="txtProductName_TextChanged" />
                                                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" CompletionInterval="100"
                                                                                            DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListProductName"
                                                                                            ServicePath="" TargetControlID="txtSearchProductName" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                        </cc1:AutoCompleteExtender>
                                                                                        <div class="input-group-btn">
                                                                                            <asp:LinkButton runat="server" CausesValidation="False" ToolTip="<%$ Resources:Attendance,Add %>" ID="ImgbtnProductSave" OnClick="btnProductSave_Click"><i class="fa fa-plus-square" style="font-size:35px;padding:5px;margin-top: -5px;"></i></asp:LinkButton>
                                                                                        </div>
                                                                                    </div>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-md-12">
                                                                                    <div style="overflow: auto">
                                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvBom" runat="server" AutoGenerateColumns="False" Width="100%">

                                                                                            <Columns>
                                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:LinkButton ID="IbtnPDDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Id") %>' OnCommand="IbtnPDDelete_Command" ToolTip="<%$ Resources:Attendance,Delete %>"><i class="fa fa-trash"></i></asp:LinkButton>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                    <ItemStyle />
                                                                                                    <FooterStyle BorderStyle="None" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField>
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblTransId" runat="server" Visible="false" Text='<%# Eval("Id") %>'></asp:Label>
                                                                                                        <asp:Label ID="lblgvsNo" runat="server" Text='<%#Container.DataItemIndex+1 %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id%>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvProductCode" runat="server" Text='<%#ProductCode(Eval("Item_Id").ToString()) %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="70px" />
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
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Request Quantity %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="txtRequestquantity" runat="server" Width="100px" Text='<%#SetDecimal(Eval("Req_Qty").ToString()) %>'
                                                                                                            OnTextChanged="txtgvUnitPrice_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                                        <cc1:FilteredTextBoxExtender ID="FiltergvtxtRequestquantity" runat="server" Enabled="True"
                                                                                                            TargetControlID="txtRequestquantity" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Price %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="txtgvUnitPrice" runat="server" Width="100px" Text='<%#SetDecimal(Eval("Unit_Price").ToString()) %>'
                                                                                                            OnTextChanged="txtgvUnitPrice_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                                        <cc1:FilteredTextBoxExtender ID="FiltergvlblgvUnitPrice" runat="server" Enabled="True"
                                                                                                            TargetControlID="txtgvUnitPrice" ValidChars="1,2,3,4,5,6,7,8,9,0,.">
                                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Line Total">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblgvtotal" runat="server" Text='<%#SetDecimal(Eval("Line_Total").ToString()) %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
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
                                                            <cc1:TabPanel ID="TabPanel3" runat="server" HeaderText="<%$ Resources:Attendance,Job Detail%>">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_TabPanel3" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <div style="overflow: auto">
                                                                                        <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvVisitTask" ShowHeader="true" runat="server" AutoGenerateColumns="false"
                                                                                            Width="100%" ShowFooter="true" OnRowDeleting="gvVisitTask_RowDeleting"
                                                                                            OnRowEditing="gvVisitTask_RowEditing" OnRowCancelingEdit="gvVisitTask_OnRowCancelingEdit"
                                                                                            OnRowUpdating="gvVisitTask_OnRowUpdating" OnRowCommand="gvVisitTask_RowCommand">

                                                                                            <Columns>
                                                                                                <asp:TemplateField>
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblsno" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Left" Width="20px" />
                                                                                                    <FooterStyle Width="20px" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ Resources:Attendance,Employee Name %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblempname" runat="server" Text='<%#Eval("Emp_Name") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                   <%-- <EditItemTemplate>
                                                                                                        <asp:TextBox ID="txtempName" runat="server" Font-Names="Verdana" Text='<%#Eval("Emp_Name") %>'
                                                                                                            CssClass="form-control" BackColor="#eeeeee" CausesValidation="false"></asp:TextBox>
                                                                                                        <cc1:AutoCompleteExtender ID="autoComplete12256660" runat="server" DelimiterCharacters=""
                                                                                                            Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                                                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtempName" UseContextKey="True"
                                                                                                            CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                                        </cc1:AutoCompleteExtender>
                                                                                                    </EditItemTemplate>--%>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:TextBox ID="txtEmpFooter" runat="server" Font-Names="Verdana" CssClass="form-control"
                                                                                                            BackColor="#eeeeee" CausesValidation="true"></asp:TextBox>
                                                                                                        <cc1:AutoCompleteExtender ID="autoComplete1225666022222" runat="server" DelimiterCharacters=""
                                                                                                            Enabled="True" ServiceMethod="GetCompletionListEmployeeName" ServicePath="" CompletionInterval="100"
                                                                                                            MinimumPrefixLength="1" CompletionSetCount="1" TargetControlID="txtEmpFooter"
                                                                                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                                                                        </cc1:AutoCompleteExtender>
                                                                                                    </FooterTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                                                                    <FooterStyle Width="150px" HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Job Date">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblstartdate" runat="server" Text='<%#GetDate(Eval("Job_Date").ToString()) %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <EditItemTemplate>
                                                                                                        <asp:TextBox ID="txteditstartdate" CssClass="form-control" runat="server" Text='<%#GetDate(Eval("Job_Date").ToString()) %>'></asp:TextBox>
                                                                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender" runat="server" TargetControlID="txteditstartdate"
                                                                                                            Format="dd-MMM-yyyy" />
                                                                                                    </EditItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:TextBox ID="txtstartdate" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="txtstartdateCalender" runat="server" TargetControlID="txtstartdate" Format="dd-MMM-yyyy" />
                                                                                                    </FooterTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                                    <FooterStyle Width="100px" HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>

                                                                                                <asp:TemplateField HeaderText="Start Time">
                                                                                                    <ItemTemplate>

                                                                                                        <asp:Label ID="lblstartTime" runat="server" Text='<%#Eval("Start_Time") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <EditItemTemplate>
                                                                                                        <asp:TextBox ID="txteditstartime" runat="server" Text='<%#Eval("Start_Time") %>' AutoPostBack="true" OnTextChanged="txteditstartime_TextChanged" CssClass="form-control"></asp:TextBox>
                                                                                                        <cc1:MaskedEditExtender ID="MaskedEditExtender_txteditstartime" runat="server" CultureAMPMPlaceholder=""
                                                                                                            CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                                                            CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                            Enabled="True" ErrorTooltipEnabled="True" Mask="99:99" MaskType="Time" TargetControlID="txteditstartime"
                                                                                                            UserTimeFormat="TwentyFourHour" MessageValidatorTip="true" InputDirection="LeftToRight">
                                                                                                        </cc1:MaskedEditExtender>
                                                                                                    </EditItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:TextBox ID="txtStarttime" runat="server" CssClass="form-control" OnTextChanged="txteditstartime_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                                        <cc1:MaskedEditExtender ID="MaskedEditExtender_txtStarttime" runat="server" CultureAMPMPlaceholder=""
                                                                                                            CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                                                            CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                            Enabled="True" ErrorTooltipEnabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtStarttime"
                                                                                                            UserTimeFormat="TwentyFourHour" MessageValidatorTip="true" InputDirection="LeftToRight">
                                                                                                        </cc1:MaskedEditExtender>
                                                                                                    </FooterTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                                                    <FooterStyle Width="80px" HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>


                                                                                                <asp:TemplateField HeaderText="Stop Time">
                                                                                                    <ItemTemplate>

                                                                                                        <asp:Label ID="lblstopTime" runat="server" Text='<%#Eval("Stop_Time") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <EditItemTemplate>
                                                                                                        <asp:TextBox ID="txteditstoptime" runat="server" Text='<%#Eval("Stop_Time") %>' CssClass="form-control" ></asp:TextBox>
                                                                                                        <cc1:MaskedEditExtender ID="MaskedEditExtender_txteditstoptime" runat="server" CultureAMPMPlaceholder=""
                                                                                                            CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                                                            CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                            Enabled="True" ErrorTooltipEnabled="True" Mask="99:99" MaskType="Time" TargetControlID="txteditstoptime"
                                                                                                            UserTimeFormat="TwentyFourHour" MessageValidatorTip="true" InputDirection="LeftToRight">
                                                                                                        </cc1:MaskedEditExtender>
                                                                                                    </EditItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:TextBox ID="txtStoptime" runat="server" CssClass="form-control" OnTextChanged="txteditstartime_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                                        <cc1:MaskedEditExtender ID="MaskedEditExtender_txtStoptime" runat="server" CultureAMPMPlaceholder=""
                                                                                                            CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                                                            CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                            Enabled="True" ErrorTooltipEnabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtStoptime"
                                                                                                            UserTimeFormat="TwentyFourHour" MessageValidatorTip="true" InputDirection="LeftToRight">
                                                                                                        </cc1:MaskedEditExtender>
                                                                                                    </FooterTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                                                    <FooterStyle Width="80px" HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>

                                                                                                 <asp:TemplateField HeaderText="Duration">
                                                                                                    <ItemTemplate>

                                                                                                        <asp:Label ID="lblstopTime1" runat="server" Text='<%#Eval("Duration") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <EditItemTemplate>
                                                                                                        <asp:TextBox ID="txteditDuration" runat="server" Text='<%#Eval("Duration") %>' CssClass="form-control"></asp:TextBox>
                                                                                                        <cc1:MaskedEditExtender ID="MaskedEditExtender_txteditDuration" runat="server" CultureAMPMPlaceholder=""
                                                                                                            CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                                                            CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                            Enabled="True" ErrorTooltipEnabled="True" Mask="99:99" MaskType="Time" TargetControlID="txteditDuration"
                                                                                                            UserTimeFormat="TwentyFourHour" MessageValidatorTip="true" InputDirection="LeftToRight">
                                                                                                        </cc1:MaskedEditExtender>
                                                                                                    </EditItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:TextBox ID="txtDuration" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                                        <cc1:MaskedEditExtender ID="MaskedEditExtender_txtDuration" runat="server" CultureAMPMPlaceholder=""
                                                                                                            CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                                                            CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                            Enabled="True" ErrorTooltipEnabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtDuration"
                                                                                                            UserTimeFormat="TwentyFourHour" MessageValidatorTip="true" InputDirection="LeftToRight">
                                                                                                        </cc1:MaskedEditExtender>
                                                                                                    </FooterTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                                                    <FooterStyle Width="80px" HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>




                                                                                                <asp:TemplateField HeaderText="Machine">
                                                                                                    <ItemTemplate>

                                                                                                        <asp:Label ID="lblMachine" runat="server" Text='<%#Eval("Machine_Name") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <EditItemTemplate>
                                                                                                       <asp:DropDownList ID="ddlEditMachineName" runat="server" CssClass="form-control">
                                                                                                            <asp:ListItem Text="Sliiting machine-1" Value="Sliiting machine-1"></asp:ListItem>
                                                                                                            <asp:ListItem Text="Sliiting machine-2" Value="Sliiting machine-2"></asp:ListItem>
                                                                                                            <asp:ListItem Text="Printing Machine" Value="Printing Machine"></asp:ListItem>
                                                                                                            <asp:ListItem Text="Diecut Machine" Value="Diecut Machine"></asp:ListItem>
                                                                                                        </asp:DropDownList>
                                                                                                    </EditItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:DropDownList ID="ddlMachineName" runat="server" CssClass="form-control">
                                                                                                            <asp:ListItem Text="Sliiting machine-1" Value="Sliiting machine-1"></asp:ListItem>
                                                                                                            <asp:ListItem Text="Sliiting machine-2" Value="Sliiting machine-2"></asp:ListItem>
                                                                                                            <asp:ListItem Text="Printing Machine" Value="Printing Machine"></asp:ListItem>
                                                                                                            <asp:ListItem Text="Diecut Machine" Value="Diecut Machine"></asp:ListItem>
                                                                                                        </asp:DropDownList>
                                                                                                    </FooterTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                                                                    <FooterStyle Width="150px" HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>


                                                                                                <asp:TemplateField>
                                                                                                    <EditItemTemplate>
                                                                                                        <asp:Button ID="ButtonUpdate" runat="server" CommandName="Update" CssClass="btn btn-primary"
                                                                                                            Text="Update" CausesValidation="true" CommandArgument='<%#Eval("Id") %>' />&nbsp;&nbsp;
                                                                                                <asp:Button ID="ButtonCancel" runat="server" CommandName="Cancel" Text="Cancel" CssClass="btn btn-primary" />
                                                                                                    </EditItemTemplate>
                                                                                                    <ItemTemplate>
                                                                                                        <asp:ImageButton ID="btnEdit" runat="server" Visible="false" ImageUrl="~/Images/edit.png" CommandName="Edit"
                                                                                                            ToolTip="<%$ Resources:Attendance,Edit %>" Style="width: 14px" />&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                                <%-- <asp:Button ID="ButtonEdit" runat="server" CommandName="Edit" Text="Edit" Visible="true" />--%>
                                                                                                        <asp:ImageButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Id") %>'
                                                                                                            ImageUrl="~/Images/Erase.png" CommandName="Delete" Width="16px" ToolTip="<%$ Resources:Attendance,Delete %>" />
                                                                                                        <%--<asp:Button ID="ButtonDelete" runat="server"  Text="Delete" CommandArgument='<%#Eval("Trans_Id") %>'  CommandName="Delete" />--%>
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Panel ID="pnlGridviewfeedback" runat="server" DefaultButton="IbtnAddTask">
                                                                                                            <asp:ImageButton ID="IbtnAddTask" Style="margin-top: 5px;" runat="server" CausesValidation="False" Height="29px"
                                                                                                                ImageUrl="~/Images/add.png" CommandName="AddNew" Width="35px" ToolTip="<%$ Resources:Attendance,Add %>" />
                                                                                                        </asp:Panel>
                                                                                                    </FooterTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                                                    <FooterStyle Width="80px" HorizontalAlign="Center" />
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
                                                            <cc1:TabPanel ID="TabPanel4" Visible="false" runat="server" HeaderText="<%$ Resources:Attendance,Machine Information%>">
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="Update_TabPanel4" runat="server">
                                                                        <ContentTemplate>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <asp:CheckBoxList ID="chkMachineInformation" runat="server">
                                                                                        <asp:ListItem Text="Sliiting machine-1" Value="Sliiting machine-1"></asp:ListItem>
                                                                                        <asp:ListItem Text="Sliiting machine-2" Value="Sliiting machine-2"></asp:ListItem>
                                                                                        <asp:ListItem Text="Printing Machine" Value="Printing Machine"></asp:ListItem>
                                                                                        <asp:ListItem Text="Diecut Machine" Value="Diecut Machine"></asp:ListItem>
                                                                                    </asp:CheckBoxList>
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
                    <div class="tab-pane" id="Bin">
                        <asp:UpdatePanel ID="Update_Bin" runat="server">
                            <ContentTemplate>


                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div2" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label11" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				  <asp:Label ID="lblTotalRecordsBin" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
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
                                                <div class="col-lg-4">
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbindBin">
                                                        <asp:TextBox ID="txtValueBin" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                                        <asp:TextBox ID="txtValueBinDate" runat="server" CssClass="form-control" Visible="false" placeholder="Search From Date"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueBinDate" runat="server" TargetControlID="txtValueBinDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="btnbindBin" runat="server" CausesValidation="False" OnClick="btnbindBin_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefreshBin" runat="server" CausesValidation="False" OnClick="btnRefreshBin_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="imgBtnRestore" CausesValidation="False" runat="server" OnClick="imgBtnRestore_Click" ToolTip="<%$ Resources:Attendance, Active %>"><span class="far fa-lightbulb"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-warning box-solid" <%= GvProductionProcessBin.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
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
                    <div class="tab-pane" id="Request">
                        <asp:UpdatePanel ID="Update_Request" runat="server">
                            <ContentTemplate>


                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="Div3" runat="server" class="box box-info collapsed-box">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <asp:Label ID="Label12" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
				 <asp:Label ID="lblTotalRecordsRequest" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>
                                                <asp:Label ID="lblSelectedRecordRequest" runat="server" Visible="false"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I3" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">

                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldNameRequest" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlFieldNameRequest_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Request No %>" Value="Request_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Request Date %>" Value="Request_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Order No. %>" Value="Order_No"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Order Date %>" Value="Order_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Customer Name %>" Value="Customername"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Sales Person %>" Value="SalesPersonName"></asp:ListItem>
                                                        <asp:ListItem Text="From Location" Value="From_Location_Name"></asp:ListItem>
                                                        <asp:ListItem Text="Product Id" Value="ProductCode"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlOptionRequest" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance,--Select-- %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Equal %>"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance,Like %>"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Attendance,Contains %>"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-5">
                                                    <asp:Panel ID="Panel3" runat="server" DefaultButton="btnbindRequest">
                                                        <asp:TextBox ID="txtValueRequest" runat="server" CssClass="form-control" placeholder="Search From Content"></asp:TextBox>
                                                        <asp:TextBox ID="txtValueRequestDate" runat="server" CssClass="form-control" Visible="false" placeholder="Search From Date"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtendertxtValueRequestDate" runat="server" TargetControlID="txtValueRequestDate" />
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbindRequest" runat="server" CausesValidation="False" OnClick="btnbindRequest_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="btnRefreshRequest" runat="server" CausesValidation="False" OnClick="btnRefreshRequest_Click" ToolTip="<%$ Resources:Attendance,Refresh %>">                                                <span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box box-primary box-solid" <%= GvPurchaseRequest.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-header with-border">
                                        <asp:Button ID="btnAdjustbackOrder" runat="server" Text="Adjust Quantity" CssClass="btn" BackColor="Green"
                                            OnClick="btnAdjustbackOrder_Click" ValidationGroup="a" />

                                        <h3 class="box-title"></h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                    <asp:HiddenField ID="hdnOrderId" runat="server" />
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvPurchaseRequest" runat="server" AllowPaging="false" AllowSorting="True"
                                                        AutoGenerateColumns="False" OnPageIndexChanging="GvPurchaseRequest_PageIndexChanging" ClientIDMode="Static"
                                                        OnSorting="GvPurchaseRequest_Sorting" PageSize="<%# PageControlCommon.GetPageSize() %>"
                                                        Width="100%">

                                                        <Columns>

                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkgvselect" runat="server" />
                                                                </ItemTemplate>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkgvHeader" runat="server" onclick="checkItem_All1_Header(this)" />
                                                                </HeaderTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btnEdit" runat="server" BackColor="Transparent" BorderStyle="None"
                                                                        CausesValidation="False" CommandArgument='<%# Eval("Trans_Id") %>' CommandName='<%# Eval("Field2") %>'
                                                                        CssClass="btnPull" OnCommand="btnPREdit_Command" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <%--   <asp:TemplateField>
                                                                <ItemTemplate>

                                                                     <asp:LinkButton ID="btnBackOrder" runat="server" CommandArgument='<%# Eval("Trans_Id") %>'
                                                                                    ToolTip="Back Order" OnCommand="btnBackOrder_Command"
                                                                                    CausesValidation="False"><i class="far fa-file-alt" style="font-size:20px;"></i></asp:LinkButton>


                                                                    
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>--%>


                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Request No %>" SortExpression="Request_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRequestNo" runat="server" Text='<%# Eval("Request_No") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Request Date %>" SortExpression="Request_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRequestDate" runat="server" Text='<%# GetDate(Eval("Request_Date").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order No. %>" SortExpression="Order_No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblorderno" runat="server" Text='<%# Eval("Order_No") %>'></asp:Label>
                                                                    <asp:Label ID="lblRequestId" runat="server" Visible="false" Text='<%# Eval("Trans_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Order Date %>" SortExpression="Order_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblExpDelDate" runat="server" Text='<%# GetDate(Eval("Order_Date").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Customer Name %>" SortExpression="Customername">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCustomerName" runat="server" Text='<%#Eval("Customername")%>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Sales Person %>" SortExpression="SalesPersonName">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblsalesperson" runat="server" Text='<%# Eval("SalesPersonName") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="From Location" SortExpression="From_Location_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFromLocation" runat="server" Text='<%# Eval("From_Location_Name").ToString() %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Id %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPID" Visible="false" runat="server" Text='<%# Eval("ProductId") %>'></asp:Label>
                                                                    <asp:Label ID="lblproductcode" runat="server" Text='<%# Eval("ProductCode").ToString() %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                            <%--  <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPID" Visible="false" runat="server" Text='<%# Eval("ProductId") %>'></asp:Label>
                                                                            <asp:Label ID="lblProductId" runat="server" Text='<%# ProductName(Eval("ProductId").ToString())%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Name %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUID" Visible="false" runat="server" Text='<%# Eval("UnitId") %>'></asp:Label>
                                                                    <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("unit_name").ToString() %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Request Qty">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblReqQty" runat="server" Text='<%# SetDecimal(Eval("Request_Qty").ToString()) %>'></asp:Label>

                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="Processed Qty">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lblprocessedQty" Font-Underline="true" runat="server" CommandArgument='<%# Eval("ProductId") %>' Text='<%# SetDecimal(Eval("Processed_Qty").ToString()) %>' OnCommand="lblprocessedQty_Command"></asp:LinkButton>

                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="Remain Qty">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRemainQty" runat="server" Text='<%# SetDecimal(Eval("Remain_Qty").ToString()) %>'></asp:Label>

                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="Adjust Qty">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtReqQty" runat="server" Text='<%# SetDecimal(Eval("Remain_Qty").ToString()) %>' Enabled="false"></asp:TextBox>


                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>






                                                        </Columns>

                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                    <asp:HiddenField ID="hdnTransNo" runat="server" Value="0" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal fade" id="PartialRequest" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
                        aria-hidden="true">
                        <asp:UpdatePanel ID="Update_PLRequest" runat="server">
                            <ContentTemplate>
                                <div class="modal-dialog modal-lg">
                                    <div class="modal-content">
                                        <div class="modal-body">

                                            <div class="col-md-6" style="max-width: 100%;">
                                                <h3>
                                                    <i class="far fa-file-alt"></i>&nbsp;&nbsp;<asp:Label ID="lblHeaderL" runat="server" Text="Production Detail"></asp:Label>
                                                </h3>
                                            </div>

                                            <div class="col-md-6" style="max-width: 100%;">
                                                <h3>&nbsp;&nbsp;
                                                </h3>
                                            </div>




                                            <div class="col-md-12">
                                                <div class="box box-primary">
                                                    <div class="box-body">
                                                        <div class="form-group">
                                                            <br />

                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvBackorder" runat="server" Width="100%" AutoGenerateColumns="False">

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
                                                                            <asp:Label ID="lblPID" Visible="false" runat="server" Text='<%# Eval("ProductId") %>'></asp:Label>
                                                                            <asp:Label ID="lblproductcode" runat="server" Text='<%# ProductCode(Eval("ProductId").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <%--  <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPID" Visible="false" runat="server" Text='<%# Eval("ProductId") %>'></asp:Label>
                                                                            <asp:Label ID="lblProductId" runat="server" Text='<%# ProductName(Eval("ProductId").ToString())%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>--%>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Unit Name %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUID" Visible="false" runat="server" Text='<%# Eval("UnitId") %>'></asp:Label>
                                                                            <asp:Label ID="lblUnit" runat="server" Text='<%# UnitName(Eval("UnitId").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Request Qty">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblReqQty" runat="server" Text='<%# SetDecimal(Eval("Request_Qty").ToString()) %>'></asp:Label>

                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>


                                                                    <asp:TemplateField HeaderText="Processed Qty">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lblprocessedQty" Font-Underline="true" runat="server" CommandArgument='<%# Eval("ProductId") %>' Text='<%# SetDecimal(Eval("Field2").ToString()) %>' OnCommand="lblprocessedQty_Command"></asp:LinkButton>

                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>


                                                                    <asp:TemplateField HeaderText="Remain Qty">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRemainQty" runat="server" Text='<%# SetDecimal(Eval("Remain_Qty").ToString()) %>'></asp:Label>

                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>


                                                                    <asp:TemplateField HeaderText="Adjust Qty">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtReqQty" runat="server" Text='<%# SetDecimal(Eval("Production_Qty").ToString()) %>' Enabled="false"></asp:TextBox>


                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>



                                                                </Columns>

                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>





                                                            <div class="col-md-12" style="text-align: center;">
                                                            </div>

                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvProcessedhistory" runat="server" Width="100%" AutoGenerateColumns="False">

                                                                <Columns>

                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Serial No %>">
                                                                        <ItemTemplate>

                                                                            <asp:Label ID="lblSNo" runat="server" Text='<%#Container.DataItemIndex+1 %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Job No">
                                                                        <ItemTemplate>

                                                                            <asp:Label ID="lblSNo" runat="server" Text='<%#Eval("job_no") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Job Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbljobDate" runat="server" Text='<%#GetDate(Eval("Job_Creation_Date").ToString()) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Product Code">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPID" runat="server" Text='<%# Eval("ProductCode") %>'></asp:Label>

                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>

                                                                    <%--  <asp:TemplateField HeaderText="<%$ Resources:Attendance,Product Name %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPName" runat="server" Text='<%# Eval("eproductname") %>'></asp:Label>

                                                                        </ItemTemplate>
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>--%>


                                                                    <asp:TemplateField HeaderText="Processed Qty">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblprocessedQty" runat="server" Text='<%# SetDecimal(Eval("Production_Qty").ToString()) %>'></asp:Label>

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




                                            <div class="modal-footer">

                                                <%--  <asp:Button ID="btnSettlementSave" runat="server" CausesValidation="False"
                                                    CssClass="btn btn-primary" OnClick="btnSettlementSave_Click" TabIndex="10" Text="<%$ Resources:Attendance,Save %>" />--%>
                                                &nbsp;&nbsp;
                                   

                                                <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                                                    Close</button>
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


    <asp:UpdateProgress ID="UpdateProgress9" runat="server" AssociatedUpdatePanelID="Update_List">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_PLRequest">
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

    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="Update_Request">
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

        function Li_Tab_Request() {
            document.getElementById('<%= Btn_Request.ClientID %>').click();
        }

        function Modal_Open_FileUpload() {
            document.getElementById('<%= Btn_Modal_FileUpload.ClientID %>').click();
        }


    </script>
    <script>

        function checkItem_All1_Header(objRef) {
            debugger;
            $('#GvPurchaseRequest tbody tr td input[type="checkbox"]').each(function () {
                debugger;
                if (objRef.checked) {
                    $(this).parents('tr').find(':checkbox').prop('checked', true);
                }
                else {
                    // $(this).closest('tr').prop('checked', false);
                    $(this).parents('tr').find(':checkbox').prop('checked', false);
                }
            });
        }



        function checkDec(el) {
            var ex = /^[0-9]+\.?[0-9]*$/;
            if (ex.test(el.value) == false) {
                el.value = "";
                alert('Incorrect Number');
            }
        }
    </script>
</asp:Content>

