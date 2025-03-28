<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="CRM.aspx.cs" Inherits="CRM_CRM" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/WebUserControl/crm_agreement.ascx" TagPrefix="AT1" TagName="crm_agreement" %>
<%@ Register Src="~/WebUserControl/Followup.ascx" TagName="AddFollowup" TagPrefix="AT1" %>
<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fas fa-hands-helping"></i>&nbsp;&nbsp;
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,CRM Follow Up%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Master Setup%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Master Setup%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Customer Relation Management%>"></asp:Label></li>
    </ol>
    <script>
        function resetPosition1() {

        }
    </script>
    <asp:UpdatePanel ID="Update_Button" runat="server">
        <ContentTemplate>
            <asp:Button ID="Btn_Followup_Modal" Style="display: none;" runat="server" data-toggle="modal" data-target="#Followup" Text="Add Followup" />
            <asp:Button ID="Btn_Number" Style="display: none;" data-toggle="modal" data-target="#NumberData" runat="server" Text="Number Data" />
            <asp:Button ID="Btn_crm_agreement_popup" Style="display: none;" data-toggle="modal" data-target="#crm_agreement_popup" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_New" runat="server">
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="txtCustomerName" />--%>
        </Triggers>
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-md-6">
                                    <asp:Button ID="btnPrev" runat="server" Text="Previous" CssClass="btn btn-info" OnClick="btnPrev_Click" />
                                    <br />
                                </div>
                                <div class="col-md-6" align="right">
                                    <asp:Button ID="btnNext" runat="server" Text="Next" CssClass="btn btn-info" OnClick="btnNext_Click" />
                                    <br />
                                </div>

                                <div class="col-md-12">
                                    <br />
                                </div>

                                <div id="div_prev" runat="server">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblgroup" runat="server" Text="Group"></asp:Label>
                                        <asp:DropDownList ID="ddlGroupSearch" runat="server" Class="form-control">
                                        </asp:DropDownList>
                                        <br />
                                    </div>

                                    <div class="col-md-4">
                                        <asp:Label ID="Label1" runat="server" Text="Grade"></asp:Label>
                                        <asp:DropDownList ID="ddlGrade" runat="server" Class="form-control">
                                        </asp:DropDownList>
                                        <br />
                                    </div>


                                    <div class="col-md-4">
                                        <asp:Label ID="Label7" runat="server" Text="Sales Employee"></asp:Label>
                                        <asp:DropDownList ID="ddlEmployee" runat="server" Class="form-control">
                                        </asp:DropDownList>
                                        <br />
                                    </div>


                                    <div class="col-md-12" style="text-align: center;">

                                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-info" OnClick="btnSearch_Click" />
                                        <asp:Button ID="btnSearchRefresh" runat="server" Text="Refresh" CssClass="btn btn-info" OnClick="btnSearchRefresh_Click" />
                                    </div>


                                    <div class="col-md-12">

                                        <br />
                                        <dx:ASPxGridView ID="GvCustomerDetails" EnableViewState="false" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False" Width="100%" KeyFieldName="Trans_Id">
                                            <Columns>

                                                <dx:GridViewDataColumn VisibleIndex="1" Caption="<%$ Resources:Attendance,View %>">
                                                    <DataItemTemplate>
                                                        <asp:LinkButton ID="lnkViewDetail" runat="server" CommandArgument='<%# Eval("Customer_Id") %>' CommandName='<%# Eval("Name") %>' TabIndex="9" ToolTip="View" OnCommand="lnkViewDetail_Command" CausesValidation="False"><i class="fa fa-eye" style="font-size:20px;color:black"></i></asp:LinkButton>
                                                    </DataItemTemplate>
                                                </dx:GridViewDataColumn>


                                                <dx:GridViewDataColumn VisibleIndex="1" Caption="<%$ Resources:Attendance,Follow-Up %>">
                                                    <DataItemTemplate>
                                                        <asp:LinkButton ID="lnkFollowup" runat="server" CommandArgument='<%# Eval("Customer_Id") %>' CommandName='<%# Eval("Name") %>' TabIndex="9" ToolTip="Follow Up" OnCommand="lnkFollowup_Command" CausesValidation="False"><i class="fa fa-phone" style="font-size:20px;color:black"></i></asp:LinkButton>
                                                    </DataItemTemplate>
                                                </dx:GridViewDataColumn>



                                                <dx:GridViewDataTextColumn FieldName="Customer_Code" Settings-AutoFilterCondition="Contains" Caption="Code" VisibleIndex="1">
                                                </dx:GridViewDataTextColumn>

                                                <dx:GridViewDataTextColumn FieldName="Status" Settings-AutoFilterCondition="Contains" Caption="Status" VisibleIndex="2">
                                                </dx:GridViewDataTextColumn>

                                                <dx:GridViewDataTextColumn FieldName="Name" Settings-AutoFilterCondition="Contains" Caption="Customer Name" VisibleIndex="2">
                                                </dx:GridViewDataTextColumn>

                                                <dx:GridViewDataTextColumn FieldName="grade" Settings-AutoFilterCondition="Contains" Caption="Grade(1-5)*" VisibleIndex="2">
                                                </dx:GridViewDataTextColumn>

                                                <dx:GridViewDataTextColumn FieldName="field1" Settings-AutoFilterCondition="Contains" Caption="Email" VisibleIndex="3">
                                                </dx:GridViewDataTextColumn>

                                                <dx:GridViewDataTextColumn FieldName="field2" Settings-AutoFilterCondition="Contains" Caption="Mobile" VisibleIndex="4">
                                                    <DataItemTemplate>
                                                        <asp:Label runat="server" ID="gvlblMobile" Text='<%# Eval("field2")%>'></asp:Label><br />
                                                        <asp:LinkButton runat="server" ID="lbtnMore" Text='<%#Eval("More")%>' OnCommand="lbtnMore_Command" CommandArgument='<%# Eval("Customer_Id") %>'></asp:LinkButton>
                                                    </DataItemTemplate>
                                                </dx:GridViewDataTextColumn>

                                                <dx:GridViewDataTextColumn FieldName="MarketingBy" Settings-AutoFilterCondition="Contains" Caption="Marketing Employee" VisibleIndex="5">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="SalesBy" Settings-AutoFilterCondition="Contains" Caption="Sales Employee" VisibleIndex="6">
                                                </dx:GridViewDataTextColumn>




                                                <dx:GridViewDataTextColumn FieldName="TotalYearlySales" Settings-AutoFilterCondition="Contains" Caption="Yearly Sales" VisibleIndex="7">
                                                </dx:GridViewDataTextColumn>

                                                <dx:GridViewDataTextColumn FieldName="TotalSales" Settings-AutoFilterCondition="Contains" Caption="Overall Sales" VisibleIndex="8">
                                                </dx:GridViewDataTextColumn>

                                                <%--                                                <dx:GridViewDataTextColumn FieldName="TotalSales" Settings-AutoFilterCondition="Contains" Caption="TotalSales" VisibleIndex="4">
                                                    <DataItemTemplate>
                                                        <asp:LinkButton runat="server" ID="lbtnTotalSales" Text='<%#Eval("TotalSales")%>' OnCommand="lbtnTotalSales_Command"></asp:LinkButton>
                                                    </DataItemTemplate>
                                                </dx:GridViewDataTextColumn>--%>
                                            </Columns>

                                            <Settings ShowGroupPanel="true" ShowFilterRow="true" />
                                            <SettingsCommandButton>
                                                <EditButton>
                                                    <Image ToolTip="Edit" Url="~/Images/edit.png" />
                                                </EditButton>


                                            </SettingsCommandButton>
                                            <Styles>
                                                <CommandColumn Spacing="2px" Wrap="False" />
                                            </Styles>

                                        </dx:ASPxGridView>

                                    </div>

                                </div>

                                <div style="display: none" id="div_next" runat="server">
                                    <div class="col-md-12">
                                        <asp:Label ID="lblCustomer" runat="server" Text="<%$ Resources:Attendance,Customer %>" />
                                        <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control" BackColor="#eeeeee" OnTextChanged="txtCustomerName_TextChanged" AutoPostBack="true" />

                                        <cc1:AutoCompleteExtender ID="txtCustomerName_AutoCompleteExtender" runat="server"
                                            CompletionInterval="100" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                            ServiceMethod="GetCompletionListCustomer" ServicePath="" TargetControlID="txtCustomerName"
                                            UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                        </cc1:AutoCompleteExtender>
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="Label2" runat="server" Text="Filter" />
                                        <br />
                                        <div style="text-align: left">
                                            <asp:RadioButtonList ID="rbtnFilter" runat="server" Width="100%" BackColor="White" RepeatColumns="2" AutoPostBack="true" OnSelectedIndexChanged="rbtnFilter_OnSelectedIndexChanged" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Contact Person" Value="CP"></asp:ListItem>
                                                <asp:ListItem Text="Sales Inquiry" Value="SINQ"></asp:ListItem>
                                                <asp:ListItem Text="Bank Detail" Value="BD"></asp:ListItem>
                                                <asp:ListItem Text="Sales Quotation" Value="SQ"></asp:ListItem>
                                                <asp:ListItem Text="Address Detail" Value="AD"></asp:ListItem>
                                                <asp:ListItem Text="Sales Order" Value="SO"></asp:ListItem>
                                                <asp:ListItem Text="Call Detail" Value="CD"></asp:ListItem>
                                                <asp:ListItem Text="Work Order Detail" Value="WO"></asp:ListItem>
                                                <asp:ListItem Text="Sales Invoice" Value="SINV"></asp:ListItem>
                                                <asp:ListItem Text="Ticket Detail" Value="TD"></asp:ListItem>
                                                <asp:ListItem Text="Sales Return" Value="SR"></asp:ListItem>
                                                <asp:ListItem Text="Finance Statement" Value="FS"></asp:ListItem>
                                                <asp:ListItem Text="Product History" Value="PH"></asp:ListItem>
                                                <asp:ListItem Text="Invoice Wise Statement" Value="FAS"></asp:ListItem>

                                            </asp:RadioButtonList>
                                        </div>
                                        <br />
                                    </div>
                                    <div class="col-md-6">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblCFLocation" runat="server" Text="<%$ Resources:Attendance,Location%>" />
                                            <br />
                                        </div>
                                        <div class="col-md-5">
                                            <asp:ListBox ID="lstLocation" runat="server" Style="width: 100%;" Height="200px"
                                                SelectionMode="Multiple" CssClass="form-control" Font-Names="Trebuchet MS" Font-Size="Small"
                                                ForeColor="Gray"></asp:ListBox>
                                        </div>
                                        <div class="col-lg-2" style="text-align: center">
                                            <div style="margin-top: 35px; margin-bottom: 35px;" class="btn-group-vertical">

                                                <asp:Button ID="btnPushDept" runat="server" CssClass="btn btn-info" Text=">" OnClick="btnPushDept_Click" />

                                                <asp:Button ID="btnPullDept" Text="<" runat="server" CssClass="btn btn-info" OnClick="btnPullDept_Click" />

                                                <asp:Button ID="btnPushAllDept" Text=">>" OnClick="btnPushAllDept_Click" runat="server" CssClass="btn btn-info" />

                                                <asp:Button ID="btnPullAllDept" Text="<<" OnClick="btnPullAllDept_Click" runat="server" CssClass="btn btn-info" />
                                            </div>
                                        </div>
                                        <div class="col-md-5">
                                            <asp:ListBox ID="lstLocationSelect" runat="server" Style="width: 100%;" Height="200px"
                                                SelectionMode="Multiple" CssClass="form-control" Font-Names="Trebuchet MS" Font-Size="Small"
                                                ForeColor="Gray"></asp:ListBox>
                                        </div>
                                    </div>
                                    <div class="col-md-12">
                                        <br />
                                    </div>
                                    <div id="trDate" runat="server" visible="false">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblFromDate" runat="server" Text="<%$ Resources:Attendance,From Date %>" />
                                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" />
                                            <cc1:CalendarExtender ID="Calender_VoucherDate" runat="server" TargetControlID="txtFromDate"
                                                Format="dd/MMM/yyyy" />
                                            <br />
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Label ID="lblToDate" runat="server" Text="<%$ Resources:Attendance,To Date %>" />
                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" />
                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtToDate"
                                                Format="dd/MMM/yyyy" />
                                            <br />
                                        </div>
                                    </div>
                                    <div id="trCurrency" runat="server" visible="false">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblCurrency" runat="server" Text="<%$ Resources:Attendance,Currency Name %>"></asp:Label>
                                            <asp:DropDownList ID="ddlCurrency" runat="server" class="form-control select2" Style="width: 100%;" />
                                            <br />
                                        </div>
                                     </div>
                                    <div id="trStatus" runat="server" visible="false" class="col-md-6">
                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Attendance,Status %>" />
                                        <asp:DropDownList ID="ddlPosted" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="<%$ Resources:Attendance, All%>" Value="0" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Attendance, Open%>" Value="Open"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Attendance, Close%>" Value="Close"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Attendance, Lost%>" Value="Lost"></asp:ListItem>
                                        </asp:DropDownList>
                                        <br />
                                    </div>
                                    <div id="trOpeningBalance" runat="server" visible="false">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblOpeningBalance" runat="server" Text="<%$ Resources:Attendance,Opening Balance %>" />
                                            <asp:TextBox ID="txtOpeningBalance" runat="server" CssClass="form-control"
                                                ReadOnly="true" />
                                            <br />
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Attendance,Closing Balance %>" />
                                            <asp:TextBox ID="txtClosingBalance" runat="server" CssClass="form-control"
                                                ReadOnly="true" />
                                            <br />
                                        </div>
                                    </div>
                                    <div class="col-md-12" style="text-align: center">
                                        <br />
                                    </div>
                                    <div class="col-md-12" style="text-align: center">
                                        <asp:Button ID="btnGetReport" runat="server" Text="<%$ Resources:Attendance,Execute %>"
                                            CssClass="btn btn-primary" OnClick="btnGetReport_Click" />

                                        <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Attendance,Reset %>"
                                            CssClass="btn btn-primary" OnClick="btnCancel_Click" />
                                        <br />
                                    </div>
                                    <div class="col-md-12" style="text-align: center">
                                        <br />
                                    </div>
                                    <div class="col-md-12">
                                        <div style="overflow: auto; max-height: 500px;">
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="gvCustomerDetail"
                                                runat="server" AutoGenerateColumns="true" Width="100%"
                                                AllowPaging="false" AllowSorting="false">
                                                <Columns>
                                                </Columns>




                                            </asp:GridView>
                                        </div>
                                        <br />
                                    </div>
                                    <div class="col-md-12" style="text-align: center">
                                        <br />
                                    </div>
                                    <div class="col-md-12">
                                        <div style="overflow: auto; max-height: 500px;">
                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GVCStatement" Width="100%" runat="server" AutoGenerateColumns="false"
                                                ShowFooter="true">

                                                <Columns>

                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance,Voucher No %>" SortExpression="Voucher_No">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkgvVoucherNo" runat="server" Text='<%# Eval("Voucher_No") %>'
                                                                OnClick="lblgvVoucherNo_Click" Font-Bold="true" CommandArgument='<%#Eval("Ref_Id") + "," + Eval("Ref_Type")+ "," + Eval("Header_Trans_Id")+ "," + Eval("Location_Id")%>' />
                                                            <asp:HiddenField ID="hdnDetailId" runat="server" Value='<%#Eval("Detail_Trans_Id") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Voucher Date %>" SortExpression="Voucher_Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvVoucherDate" runat="server" Text='<%#GetDate(Eval("Voucher_Date").ToString()) %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Voucher Type %>" SortExpression="Voucher_Type">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvVoucherType" runat="server" Text='<%#Eval("Voucher_Type") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Narration %>" SortExpression="Narration">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvNarration" runat="server" Text='<%#Eval("Narration") %>' />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblgvTotal" runat="server" Text="<%$ Resources:Attendance,TOTAL: %>" />
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Refrence %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvRefrenceNumber" runat="server" Text='<%#Eval("RefrenceNumber") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Debit %>" SortExpression="Debit_Amount">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvDebitAmount" runat="server" Text='<%#Eval("Debit_Amount") %>' />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblgvDebitTotal" runat="server" />
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Credit %>" SortExpression="Credit_Amount">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvCreditAmount" runat="server" Text='<%#Eval("Credit_Amount") %>' />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblgvCreditTotal" runat="server" />
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Balance %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvBalance" runat="server" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblgvBalanceTotal" runat="server" />
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Created By %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvCreatedBy" runat="server" Text='<%#GetEmployeeNameByEmpCode(Eval("CreatedBy").ToString()) %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Attendance, Modified By %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvModifiedBy" runat="server" Text='<%#GetEmployeeNameByEmpCode(Eval("ModifiedBy").ToString()) %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>



                                            </asp:GridView>
                                        </div>
                                        <br />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:HiddenField ID="hdnFollowupTableName" runat="server" Value="CRM Follow Up" />

    <div class="modal fade" id="Followup" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-body">
                    <AT1:AddFollowup ID="FollowupUC" runat="server" />
                </div>
                <div class="modal-footer">
                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>




    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_New">
        <ProgressTemplate>
            <div class="modal_Progress">
                <div class="center_Progress">
                    <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>



    <div style="display: none">
        <asp:Panel ID="pnlMenuList" runat="server"></asp:Panel>
        <asp:Panel ID="pnlMenuNew" runat="server"></asp:Panel>
        <asp:Panel ID="PnlList" runat="server"></asp:Panel>
        <asp:Panel ID="PnlNewEdit" runat="server"></asp:Panel>
        <asp:Panel ID="Panel4" runat="server"></asp:Panel>
        <asp:Panel ID="Panel5" runat="server"></asp:Panel>
        <asp:Panel ID="Panel6" runat="server"></asp:Panel>
        <asp:Panel ID="Panel7" runat="server"></asp:Panel>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">

    <div class="modal fade" id="NumberData" tabindex="-1" role="dialog" aria-labelledby="NumberData_ModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-mg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="myNumbers">All Number List:</h4>
                </div>
                <div class="modal-body">
                    <div id="AllNumberData">
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="Ageing_Detail" tabindex="-1" role="dialog" aria-labelledby="Voucher_DetailLabel" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="Voucher_DetailLabel">
                        <asp:Label ID="Label8" runat="server" Text="Customer Invoice Statement" Font-Bold="true"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="Update_Voucher_Detail" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <div class="col-md-12">
                                                    <asp:HiddenField ID="HDFcurrentCustomerID" runat="server" Value="0" />
                                                    <asp:HiddenField ID="HDFCurrencyID" runat="server" Value="0" />
                                                    <dx:ASPxWebDocumentViewer ID="ReportViewer1" runat="server" Width="100%"></dx:ASPxWebDocumentViewer>
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

                    <button type="button" class="btn btn-danger" data-dismiss="modal">
                        Close</button>


                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="crm_agreement_popup" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" data-backdrop="static"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">

                <div class="modal-body">

                    <AT1:crm_agreement runat="server" ID="agreement" />

                </div>
                <div class="modal-footer">
                    <button type="button" id="" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>



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
        function Modal_Number_Open(data) {
            document.getElementById('AllNumberData').innerHTML = data;
            document.getElementById('<%= Btn_Number.ClientID %>').click();
        }
        function Modal_crm_agreement_Open() {
            document.getElementById('<%= Btn_crm_agreement_popup.ClientID %>').click();
        }

        function LI_Edit_Active_agreement() {

            $("#Li_crm_agreementList1").removeClass("active");
            $("#crm_agreementList1").removeClass("active");

            $("#Li_New1").addClass("active");
            $("#New1").addClass("active");
        }


        function LI_Edit_Active1() { }

        //    $("#Li_FollowupList1").removeClass("active");
        //    $("#FollowupList1").removeClass("active");

        //    $("#Li_New1").addClass("active");
        //    $("#New1").addClass("active");
        //}


        function Modal_Followup_Open() {
            document.getElementById('<%= Btn_Followup_Modal.ClientID %>').click();
        }

        function fnOpenModalAgeing_Detail()
        {
            $('#Ageing_Detail').modal('show');
        }
    </script>

</asp:Content>
