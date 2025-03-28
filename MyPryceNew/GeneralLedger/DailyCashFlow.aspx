<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="DailyCashFlow.aspx.cs" Inherits="GeneralLedger_DailyCashFlow" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-money-check-alt"></i>
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Daily Cash Flow%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Finance Setup%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Cash Management%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Daily Cash Flow%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_List" Style="display: none;" runat="server" OnClick="btnList_Click" Text="List" />
            <asp:Button ID="Btn_New" Style="display: none;" runat="server" OnClick="btnNew_Click" Text="New" />
            <asp:Button ID="Btn_Bin" Style="display: none;" runat="server" OnClick="btnBin_Click" Text="Bin" />
            <asp:Button ID="Btn_myModal" Style="display: none;" runat="server" data-toggle="modal" data-target="#myModal" Text="View Modal" />
            <asp:Button ID="btn_UpdateAmtModal" Style="display: none;" runat="server" data-toggle="modal" data-target="#UpdateAmt" Text="Update Amt" />
            <asp:HiddenField runat="server" ID="hdnCanPrint" />
            <asp:HiddenField runat="server" ID="hdnCanView" />
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
                    <li id="Li_Bin"><a href="#Bin" onclick="Li_Tab_Bin()" data-toggle="tab">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Attendance,Bin %>"></asp:Label></a></li>
                    <li id="Li_New"><a onclick="Li_Tab_New()" href="#New" data-toggle="tab">
                        <asp:UpdatePanel ID="Update_Li" runat="server">
                            <ContentTemplate>
                                <i class="fa fa-file"></i>&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Tab_New" runat="server" Text="<%$ Resources:Attendance,New%>"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </a></li>
                    <li id="Li_List" class="active"><a href="#List" onclick="Li_Tab_List()" data-toggle="tab">
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
					 <asp:Label ID="lblTotalRecords" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I1" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                 <div class="col-md-4">
                                                     <asp:HiddenField ID="hdnLocId" Value="0" runat="server" />
                                                     <asp:HiddenField ID="hdnCurrencyId" Value="0" runat="server" />
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="ddlLocationList" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlLocationList_SelectedIndexChanged" />
                                                    </div>
                                                    <br />
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:DropDownList ID="ddlFieldName" OnSelectedIndexChanged="ddlFieldName_SelectedIndexChanged" AutoPostBack="true" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Date %>" Value="CF_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Opening Amount %>" Value="CF_OpeningAmount" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Closing Amount %>" Value="CF_ClosingAmount"></asp:ListItem>
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
                                                <div class="col-lg-2">
                                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnbind">
                                                        <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" placeholder="Search from Content"></asp:TextBox>
                                                        <asp:TextBox ID="Txt_Date_List" Visible="false" runat="server" CssClass="form-control" placeholder="Search from Date"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" Format="dd-MMM-yyyy" ID="CalendarExtender3" runat="server" Enabled="True" TargetControlID="Txt_Date_List">
                                                        </cc1:CalendarExtender>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbind" runat="server" CausesValidation="False"
                                                        OnClick="btnbindrpt_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="btnRefresh" runat="server" CausesValidation="False"
                                                        OnClick="btnRefreshReport_Click"
                                                        ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                </div>


                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box box-warning box-solid" <%= GvCashDetail.Rows.Count>0?"style='display:block'":"style='display:none'"%>>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="flow">
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvCashDetail" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>' runat="server"
                                                        AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvCashDetail_PageIndexChanging"
                                                        AllowSorting="True" OnSorting="GvCashDetail_Sorting">

                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="dropdown" style="position: absolute;">
                                                                        <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                                                            <i class="fa fa-ellipsis-h" aria-hidden="true"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu">
                                                                            <li <%= hdnCanPrint.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnPrint" runat="server" CausesValidation="False" CommandArgument='<%# Eval("CF_Id") %>'
                                                                                    OnCommand="IbtnPrint_Command" ToolTip="<%$ Resources:Attendance,Print %>"
                                                                                    Visible="true"><i class="fa fa-print"></i>Print</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanView.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("CF_Id") %>'
                                                                                    Visible="true" ToolTip="<%$ Resources:Attendance,View %>"
                                                                                    OnCommand="lnkViewDetail_Command" CausesValidation="False"><i class="fa fa-eye"></i>View</asp:LinkButton>
                                                                            </li>

                                                                            <li <%= hdnCanDelete.Value=="true"?"style='display:block'":"style='display:none'" %>>
                                                                                <asp:LinkButton ID="IbtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("CF_Id") %>'
                                                                                    OnCommand="IbtnDelete_Command" Visible="true"><i class="fa fa-trash"></i>Delete</asp:LinkButton>
                                                                                <cc1:ConfirmButtonExtender ID="confirm1" runat="server" ConfirmText="<%$ Resources:Attendance,Are you sure you want to delete the record?%>" TargetControlID="IbtnDelete"></cc1:ConfirmButtonExtender>
                                                                            </li>

                                                                        </ul>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Cash Flow Date %>" SortExpression="CF_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCFDate" runat="server" Text='<%#GetDate(Eval("CF_Date").ToString()) %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Opening Amount %>" SortExpression="CF_OpeningAmount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCFOpeningAmount" runat="server" Text='<%#Eval("CF_OpeningAmount") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Closing Amount %>" SortExpression="CF_ClosingAmount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCFClosingAmount" runat="server" Text='<%#Eval("CF_ClosingAmount") %>' />
                                                                    <asp:ImageButton ID="btnEditClosingAmt" runat="server" CommandArgument='<%#Eval("CF_ClosingAmount")+"/"+Eval("CF_Id") %>'
                                                                        ImageUrl="~/Images/edit.png" Visible='<%# CheckPermission(Eval("CF_Date").ToString()) %>' CommandName='<%#GetDate(Eval("CF_Date").ToString()) %>' OnCommand="btnEditClosingAmt_Command" CausesValidation="False"
                                                                        ToolTip="Edit Closing Balance" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Created By%>" SortExpression="CreatedBy">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCFCreatedBy" runat="server" Text='<%#GetEmployeeNameByEmpCode(Eval("CreatedBy").ToString()) %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Created Date %>" SortExpression="CreatedDate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCFCreatedDate" runat="server" Text='<%#GetDateWithTime(Eval("CreatedDate").ToString()) %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <PagerStyle CssClass="pagination-ys" />

                                                    </asp:GridView>
                                                    <asp:HiddenField ID="HDFSort" runat="server" />
                                                    <asp:HiddenField ID="hdnCFDate" runat="server" />
                                                    <asp:HiddenField ID="hdnCFID" runat="server" />

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>



                    <div class="modal fade" id="UpdateAmt" tabindex="-1" role="dialog" aria-labelledby="UpdateAmt_ModalLabel" aria-hidden="true">
                        <div class="modal-dialog modal-mg">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal">
                                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                                    <h4 class="modal-title" id="AllGeneratedContracts">Generated Contract List:</h4>
                                </div>
                                <div class="modal-body">
                                    <asp:UpdatePanel ID="upAmtUpdate" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtUPClosingAmount" runat="server" />
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="true" TargetControlID="txtUPClosingAmount" ValidChars="1,2,3,4,5,6,7,8,9,0,.,-" />
                                            <asp:Button ID="btnUpdateAmt" runat="server" Text="Update Amount" OnClick="btnUpdateAmt_Click" />

                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </div>
                                <div class="modal-footer">
                                    <%--<button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                                        Close</button>--%>
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
                                                        <asp:Label ID="lblDate" runat="server" Text="<%$ Resources:Attendance,Date %>" />
                                                        <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" AutoPostBack="true"
                                                            OnTextChanged="txtDate_TextChanged" />
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender_VoucherDate" runat="server" TargetControlID="txtDate" />
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12">
                                                    </div>

                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblOpeningBalance" runat="server" Text="<%$ Resources:Attendance,Opening Balance %>" />
                                                        <asp:TextBox ID="txtOpeningBalance" ReadOnly="true" runat="server"
                                                            CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblClosingBalance" runat="server" Text="<%$ Resources:Attendance,Closing Balance %>" />
                                                        <asp:TextBox ID="txtClosingBalance" ReadOnly="true" runat="server"
                                                            CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblSummarizedTotal" runat="server" Text="<%$ Resources:Attendance,Summarized Total %>" />
                                                        <asp:TextBox ID="txtSummarizedTotal" ReadOnly="true" runat="server"
                                                            CssClass="form-control" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" style="text-align: center">
                                                        <asp:Button ID="btnGetReport" runat="server" Text="<%$ Resources:Attendance,Execute %>"
                                                            CssClass="btn btn-primary" OnClick="btnGetReport_Click" />

                                                        <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                                            CssClass="btn btn-danger" OnClick="btnCancel_Click" />

                                                        <asp:Button ID="btnShowReport" runat="server" Text="<%$ Resources:Attendance,Print %>"
                                                            CssClass="btn btn-primary" OnClick="btnShowReport_Click" Visible="false" />

                                                        <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Attendance,Save %>" Visible="false"
                                                            CssClass="btn btn-success" OnClick="btnSave_Click" />
                                                        <asp:HiddenField ID="hdnCashFlowId" runat="server" Value="0" />

                                                        <asp:Button ID="btnPost" runat="server" Text="<%$ Resources:Attendance,Post %>" CssClass="btn btn-primary"
                                                            OnClick="btnPost_Click" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <br />
                                                        <div style="overflow: auto; max-height: 500px;">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GVCashFlow" Width="100%" runat="server" ShowFooter="true" AutoGenerateColumns="false">

                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Voucher No %>" SortExpression="Voucher_No">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvVoucherNo" runat="server" Text='<%#Eval("Voucher_No") %>' />
                                                                            <asp:HiddenField ID="hdnVoucherDetailId" runat="server" Value='<%#Eval("Detail_Trans_Id") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Account Name %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvAccountName" runat="server" Text='<%#GetAccountName(Eval("Account_No").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Narration %>" SortExpression="Narration">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvNarattion" runat="server" Text='<%#Eval("Narration") %>' />
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblgvTotal" runat="server" Text="<%$ Resources:Attendance,TOTAL: %>" />
                                                                        </FooterTemplate>
                                                                        <FooterStyle BorderStyle="None" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Debit Amount %>" SortExpression="Debit_Amount">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvDebitAmount" runat="server" Text='<%#Eval("Debit_Amount") %>' />
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblgvDebitTotal" runat="server" />
                                                                        </FooterTemplate>
                                                                        <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Credit Amount %>" SortExpression="Credit_Amount">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvCreditAmount" runat="server" Text='<%#Eval("Credit_Amount") %>' />
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblgvCreditTotal" runat="server" />
                                                                        </FooterTemplate>
                                                                        <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Balance %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvBalance" runat="server" />
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblgvBalanceTotal" runat="server" />
                                                                        </FooterTemplate>
                                                                        <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                </Columns>

                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblAccountsSummarized" runat="server" Visible="false" Font-Bold="true"
                                                            Font-Size="Large" Text="<%$ Resources:Attendance,Accounts Summarized %>" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div style="overflow: auto; max-height: 500px;">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvSummarized" Width="100%" runat="server" ShowFooter="true" AutoGenerateColumns="false">

                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,S No.%>">
                                                                        <ItemTemplate>
                                                                            <%#Container.DataItemIndex+1 %>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Account No. %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvAccountNo" runat="server" Text='<%#GetAccountNo(Eval("Param_Value").ToString()) %>' />
                                                                            <asp:HiddenField ID="hdnAccountId" runat="server" Value='<%#Eval("Param_Value") %>' />
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblgvTotal" runat="server" Text="<%$ Resources:Attendance,TOTAL: %>" />
                                                                        </FooterTemplate>
                                                                        <FooterStyle BorderStyle="None" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Account Name %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvAccountName" runat="server" Text='<%#GetAccountName(Eval("Param_Value").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, System Balance %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvSystemBalance" runat="server" />
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblgvSystemBalanceTotal" runat="server" />
                                                                        </FooterTemplate>
                                                                        <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Physical Balance %>">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtgvPhysicalBalance" runat="server" Width="100px" AutoPostBack="true"
                                                                                OnTextChanged="txtgvPhysicalBalance_TextChanged" />
                                                                            <cc1:FilteredTextBoxExtender ID="flPhysicalBalance" runat="server" Enabled="true"
                                                                                TargetControlID="txtgvPhysicalBalance" ValidChars="1,2,3,4,5,6,7,8,9,0,.,-" />
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblgvPhysicalBalanceTotal" runat="server" />
                                                                        </FooterTemplate>
                                                                        <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Remarks %>">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtRemarks" runat="server" Width="430px" />
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:CheckBox ID="chkAllTrue" Font-Bold="true" runat="server" Text="<%$ Resources:Attendance, If All Balances are Same. %>"
                                                                                OnCheckedChanged="chkAllTrue_CheckedChanged" AutoPostBack="true" />
                                                                        </FooterTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                </Columns>

                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>

                                                        <div style="overflow: auto; max-height: 500px;">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvSummarizedView" Width="100%" runat="server" ShowFooter="true"
                                                                AutoGenerateColumns="false">

                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,S No.%>">
                                                                        <ItemTemplate>
                                                                            <%#Container.DataItemIndex+1 %>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Account No. %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvAccountNo" runat="server" Text='<%#Eval("Account_No") %>' />
                                                                            <asp:HiddenField ID="hdnAccountId" runat="server" Value='<%#Eval("Account_Id") %>' />
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblgvTotal" runat="server" Text="<%$ Resources:Attendance,TOTAL: %>" />
                                                                        </FooterTemplate>
                                                                        <FooterStyle BorderStyle="None" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Account Name %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvAccountName" runat="server" Text='<%#Eval("AccountName") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, System Balance %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvSystemBalance" runat="server" Text='<%#Eval("SystemBalance") %>' />
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblgvSystemBalanceTotal" runat="server" />
                                                                        </FooterTemplate>
                                                                        <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Physical Balance %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvPhysicalBalance" runat="server" Text='<%#Eval("PhysicalBalance") %>' />
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblgvPhysicalBalanceTotal" runat="server" />
                                                                        </FooterTemplate>
                                                                        <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Remarks %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvRemarks" runat="server" Width="430px" Text='<%#Eval("Remarks") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                </Columns>

                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblPostdatedCheque" runat="server" Visible="false" Font-Bold="true"
                                                            Font-Size="Large" Text="<%$ Resources:Attendance, Cheque Details %>" />
                                                        <asp:Label ID="lblCustomersCheque" runat="server" Visible="false" Font-Bold="true"
                                                            Font-Size="Large" Text="<%$ Resources:Attendance, Customers Cheque %>" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div style="overflow: auto; max-height: 500px;">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvCustomerChequeDetail" Width="100%" runat="server" ShowFooter="true"
                                                                AutoGenerateColumns="false">

                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkgvCSelect" runat="server" OnCheckedChanged="chkgvCSelect_CheckedChanged"
                                                                                AutoPostBack="true" />
                                                                            <asp:HiddenField ID="hdnCVD_Id" runat="server" Value='<%#Eval("VoucherDetailId") %>' />
                                                                        </ItemTemplate>

                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,S No.%>">
                                                                        <ItemTemplate>
                                                                            <%#Container.DataItemIndex+1 %>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Voucher No. %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvVoucherNumber" runat="server" Text='<%#Eval("Voucher_No") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Account Name %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvAccountName" runat="server" Text='<%#GetAccountName(Eval("DetailAccountNo").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <%-- <asp:TemplateField HeaderText="<%$ Resources:Attendance, Customer Name %>">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblgvCustomerName" runat="server" Text='<%#GetContactName(Eval("Other_Account_No").ToString()) %>' />
                                                                                                <asp:HiddenField ID="hdnCustomerId" runat="server" Value='<%#Eval("Other_Account_No") %>' />
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center"  />
                                                                                        </asp:TemplateField>--%>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Cheque Number %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvChequeNumber" runat="server" Text='<%#Eval("Cheque_No") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Cheque Issue Date %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvChequeIssueDate" runat="server" Text='<%#strDate(Eval("Cheque_Issue_Date").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Cheque Clear Date %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvChequeClearDate" runat="server" Text='<%#strDate(Eval("Cheque_Clear_Date").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblgvTotal" runat="server" Text="<%$ Resources:Attendance,TOTAL: %>" />
                                                                        </FooterTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Amount %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvChequeAmount" runat="server" Text='<%#Eval("Debit_Amount") %>' />
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblgvChequeAmountTotal" runat="server" />
                                                                        </FooterTemplate>
                                                                        <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Narration %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvNarration" runat="server" Text='<%#Eval("Narration") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                </Columns>

                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblSupplierCheque" runat="server" Visible="false" Font-Bold="true"
                                                            Font-Size="Large" Text="<%$ Resources:Attendance, Suppliers Cheque %>" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div style="overflow: auto; max-height: 500px;">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvSupplierChequeDetail" Width="100%" runat="server" ShowFooter="true"
                                                                AutoGenerateColumns="false">

                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkgvSSelect" runat="server" OnCheckedChanged="chkgvSSelect_CheckedChanged"
                                                                                AutoPostBack="true" />
                                                                            <asp:HiddenField ID="hdnSVD_Id" runat="server" Value='<%#Eval("VoucherDetailId") %>' />
                                                                        </ItemTemplate>

                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,S No.%>">
                                                                        <ItemTemplate>
                                                                            <%#Container.DataItemIndex+1 %>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Voucher No. %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvVoucherNumber" runat="server" Text='<%#Eval("Voucher_No") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Account Name %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvAccountName" runat="server" Text='<%#GetAccountName(Eval("DetailAccountNo").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <%--   <asp:TemplateField HeaderText="<%$ Resources:Attendance, Supplier Name %>">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblgvSupplierName" runat="server" Text='<%#GetContactName(Eval("Other_Account_No").ToString()) %>' />
                                                                                                <asp:HiddenField ID="hdnSupplierId" runat="server" Value='<%#Eval("Other_Account_No") %>' />
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center"  />
                                                                                        </asp:TemplateField>--%>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Cheque Number %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvChequeNumber" runat="server" Text='<%#Eval("Cheque_No") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Cheque Issue Date %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvChequeIssueDate" runat="server" Text='<%#strDate(Eval("Cheque_Issue_Date").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Cheque Clear Date %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvChequeClearDate" runat="server" Text='<%#strDate(Eval("Cheque_Clear_Date").ToString()) %>' />
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblgvTotal" runat="server" Text="<%$ Resources:Attendance,TOTAL: %>" />
                                                                        </FooterTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Amount %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvChequeAmount" runat="server" Text='<%#Eval("Credit_Amount") %>' />
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblgvChequeAmountTotal" runat="server" />
                                                                        </FooterTemplate>
                                                                        <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Narration %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvNarration" runat="server" Text='<%#Eval("Narration") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                </Columns>

                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblOtherCheque" runat="server" Visible="false" Font-Bold="true" Font-Size="Large"
                                                            Text="<%$ Resources:Attendance, Other Cheques %>" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div style="overflow: auto; max-height: 500px;">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvOtherCheque" Width="100%" runat="server" ShowFooter="true" AutoGenerateColumns="false">

                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,S No.%>">
                                                                        <ItemTemplate>
                                                                            <%#Container.DataItemIndex+1 %>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Cheque Issue Date %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvChequeIssueDate" runat="server" Text='<%#Eval("") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Cheque Clear Date %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvChequeClearDate" runat="server" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Cheque Number %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvChequeNumber" runat="server" Text='<%#Eval("") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Amount %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvChequeAmount" runat="server" Text='<%#Eval("") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Narration %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvNarration" runat="server" Text='<%#Eval("") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                </Columns>

                                                                <PagerStyle CssClass="pagination-ys" />

                                                            </asp:GridView>
                                                        </div>
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
                                                    <asp:Label ID="Label2" runat="server" Text="Advance Search"></asp:Label></h3>
                                                &nbsp;&nbsp;|&nbsp;&nbsp;
					<asp:Label ID="lblTotalRecordsBin" runat="server" Font-Bold="true" Text="<%$ Resources:Attendance,Total Records: 0 %>"></asp:Label>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i id="I2" runat="server" class="fa fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body">
                                                <div class="col-lg-3">
                                                    <asp:DropDownList ID="ddlFieldNameBin" AutoPostBack="true" OnSelectedIndexChanged="ddlFieldNameBin_SelectedIndexChanged" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Date %>" Value="CF_Date"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Opening Amount %>" Value="CF_OpeningAmount" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Attendance, Closing Amount %>" Value="CF_ClosingAmount"></asp:ListItem>
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
                                                <div class="col-lg-3">
                                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnbindrptBin">
                                                        <asp:TextBox ID="txtbinValue" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <asp:TextBox ID="Txt_Date_Bin" Visible="false" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <cc1:CalendarExtender OnClientShown="showCalendar" Format="dd-MMM-yyyy" ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="Txt_Date_Bin">
                                                        </cc1:CalendarExtender>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-lg-2">
                                                    <asp:LinkButton ID="btnbindrptBin" runat="server" CausesValidation="False" 
                                                        OnClick="btnbindrptBin_Click" ToolTip="<%$ Resources:Attendance,Search %>"><span class="fa fa-search"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="btnRefreshBin" runat="server" CausesValidation="False" 
                                                        OnClick="btnRefreshBin_Click" ToolTip="<%$ Resources:Attendance,Refresh %>"><span class="fa fa-repeat"  style="font-size:25px;"></span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                </div>
                                            
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
                                                    <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GvCashDetailBin" PageSize='<%# int.Parse(Session["GridSize"].ToString()) %>'
                                                        runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" OnPageIndexChanging="GvCashDetailBin_PageIndexChanging"
                                                        AllowSorting="True" OnSorting="GvCashDetailBin_Sorting">

                                                        <Columns>
                                                            <asp:TemplateField Visible="false" HeaderText="Restore">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgBtnRestore" Height="25px" Width="25px" CausesValidation="False" CommandArgument='<%# Eval("CF_Id") %>'
                                                                        TabIndex="39" runat="server" ImageUrl="~/Images/active.png" OnCommand="imgBtnRestore_Click"
                                                                        ToolTip="<%$ Resources:Attendance, Active %>" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Cash Flow Date %>" SortExpression="CF_Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblReqId" runat="server" Text='<%#Eval("CF_Id") %>' Visible="false" />
                                                                    <asp:Label ID="lblgvCFDate" runat="server" Text='<%#GetDate(Eval("CF_Date").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Opening Amount %>" SortExpression="CF_OpeningAmount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCFOpeningAmount" runat="server" Text='<%#Eval("CF_OpeningAmount") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Closing Amount %>" SortExpression="CF_ClosingAmount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCFClosingAmount" runat="server" Text='<%#Eval("CF_ClosingAmount") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Created By%>" SortExpression="CreatedBy">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCFCreatedBy" runat="server" Text='<%#GetEmployeeNameByEmpCode(Eval("CreatedBy").ToString()) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Created Date %>" SortExpression="CreatedDate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCFCreatedDate" runat="server" Text='<%#GetDateWithTime(Eval("CreatedDate").ToString()) %>' />
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
    <asp:UpdatePanel ID="Update" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlMenuList" runat="server" Visible="false"></asp:Panel>
            <asp:Panel ID="pnlMenuNew" runat="server" Visible="false"></asp:Panel>
            <asp:Panel ID="pnlMenuBin" runat="server" Visible="false"></asp:Panel>
            <asp:Panel ID="PnlList" runat="server" Visible="false"></asp:Panel>
            <asp:Panel ID="PnlNewEdit" runat="server" Visible="false"></asp:Panel>
            <asp:Panel ID="PnlBin" runat="server" Visible="false"></asp:Panel>
            <asp:Panel ID="Panel6" runat="server" Visible="false"></asp:Panel>
            <asp:Panel ID="Panel7" runat="server" Visible="false"></asp:Panel>
            <asp:Panel ID="Panel8" runat="server" Visible="false"></asp:Panel>
            <asp:Panel ID="Panel9" runat="server" Visible="false"></asp:Panel>
            <asp:Panel ID="Panel10" runat="server" Visible="false"></asp:Panel>
            <asp:Panel ID="Panel11" runat="server" Visible="false"></asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

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

        function Li_Tab_List() {
            document.getElementById('<%= Btn_List.ClientID %>').click();
        }
        function Li_Tab_New() {
            document.getElementById('<%= Btn_New.ClientID %>').click();
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
        function UpdateAmtModal_Popup() {
            document.getElementById('<%= btn_UpdateAmtModal.ClientID %>').click();
        }
    </script>
</asp:Content>
