<%@ Page Title="" Language="C#" EnableEventValidation="false" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="TrailBalance.aspx.cs" Inherits="GeneralLedger_TrailBalance" %>

<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="exp1" Namespace="ControlFreak" Assembly="ExportPanel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/account_voucher.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Trail Balance%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Finance Setup%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,General Ledger%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Trail Balance%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
  
                        <asp:UpdatePanel ID="Update_New" runat="server">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnExport" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box box-primary">
                                            <div class="box-body">
                                                <div class="form-group">												
												<div class="col-md-6">
                                                    <asp:Label ID="lblFromDate" Visible="true" runat="server" 
                                                        Text="<%$ Resources:Attendance,From Date %>" />
                                                    <a style="color: Red">*</a>
        <asp:RequiredFieldValidator EnableClientScript="true" style="float:right;" runat="server" id="RequiredFieldValidator4" ValidationGroup="Save" 
        Display="Dynamic" SetFocusOnError="true" controltovalidate="txtFromDate" errormessage="<%$ Resources:Attendance,Enter From Date%>"></asp:RequiredFieldValidator>

                                                    <asp:TextBox ID="txtFromDate" Visible="true" runat="server" CssClass="form-control" />
                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="Calender_VoucherDate" runat="server" TargetControlID="txtFromDate"
                                                        Format="dd/MMM/yyyy" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Label ID="lblToDate" runat="server" Text="<%$ Resources:Attendance,To Date %>" />
                                                        <a style="color: Red">*</a>
        <asp:RequiredFieldValidator EnableClientScript="true" style="float:right;" runat="server" id="RequiredFieldValidator1" ValidationGroup="Save" 
        Display="Dynamic" SetFocusOnError="true" controltovalidate="txtToDate" errormessage="<%$ Resources:Attendance,Enter To Date%>"></asp:RequiredFieldValidator>

                                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" />
                                                    <cc1:CalendarExtender OnClientShown="showCalendar" ID="CalendarExtender1" runat="server" TargetControlID="txtToDate"
                                                        Format="dd/MMM/yyyy" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-6" id="td" runat="server" visible="false">
                                                        <asp:RadioButton ID="rbPost" runat="server" GroupName="Post" Text="<%$ Resources:Attendance,Posted Record %>" />
                                                        <asp:RadioButton ID="rbUnPost" runat="server" GroupName="Post" Text="<%$ Resources:Attendance,UnPosted Record %>" />
                                                        <br />
                                                    </div>
                                                    	<div class="col-md-12">
		<div class="col-md-2">
		</div>
		<div class="col-md-3">
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
		<div class="col-md-3">
			<asp:ListBox ID="lstLocationSelect" runat="server" Style="width: 100%;" Height="200px"
				SelectionMode="Multiple" CssClass="form-control" Font-Names="Trebuchet MS" Font-Size="Small"
				ForeColor="Gray"></asp:ListBox>
		</div>
		<div class="col-md-2">
		</div>
		<br />
	</div>
                                                    <div class="col-md-12" style="text-align:center">
                                                        <br />
                                                        <asp:Button ID="btnExecute" ValidationGroup="Save" runat="server" Text="<%$ Resources:Attendance,Execute %>"
                                                                    CssClass="btn btn-primary" OnClick="btnExecute_Click" />

                                                        <asp:Button ID="btnExport" runat="server" CssClass="btn btn-primary" Text="Export To Excel"
                                                                    OnClick="ExportToExcel" />

                                                        <asp:Button ID="btnGetReport" runat="server" Text="<%$ Resources:Attendance,Print %>"
                                                        CssClass="btn btn-primary" Visible="false" OnClick="btnGetReport_Click" />
                                                        <br />
                                                    </div>
                                                    <div class="col-md-12">
                                                        <br />
                                                        </div>
                                                    <div class="col-md-12">
                                                        <div style="overflow:auto; max-height:500px;">
                                                            <asp:GridView CssClass="table-striped table-bordered table table-hover" ID="GVTrailBalance" runat="server" Width="100%" ShowFooter="true" AutoGenerateColumns="false"
                                                        >
                                                        
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance,Name %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvGroupName" runat="server" Text='<%#Eval("name") %>' />
                                                                    <asp:HiddenField ID="hdngvParentId" runat="server" Value='<%#Eval("parent_id") %>' />
                                                                    <asp:HiddenField ID="hdngvAccountId" runat="server" Value='<%#Eval("account_no") %>' />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblgvTotal" runat="server" Text="<%$ Resources:Attendance,TOTAL: %>"
                                                                        />
                                                                </FooterTemplate>
                                                                <FooterStyle BorderStyle="None" Font-Bold="true" />
                                                                <ItemStyle  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Opening Debit %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvOpeningBalD" runat="server" Text='<%#Eval("Open_Debit") %>' />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblgvOpeningBalDTotal" runat="server" />
                                                                </FooterTemplate>
                                                                <FooterStyle BorderStyle="None" HorizontalAlign="Right" Font-Bold="true" />
                                                                <ItemStyle  HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Opening Credit %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvOpeningBalC" runat="server" Text='<%#Eval("Open_Credit") %>' />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblgvOpeningBalCTotal" runat="server" />
                                                                </FooterTemplate>
                                                                <FooterStyle BorderStyle="None" HorizontalAlign="Right" Font-Bold="true" />
                                                                <ItemStyle  HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Debit %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvDebit" runat="server" Text='<%#Eval("Debit") %>' />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblgvDebitTotal" runat="server" />
                                                                </FooterTemplate>
                                                                <FooterStyle BorderStyle="None" HorizontalAlign="Right" Font-Bold="true" />
                                                                <ItemStyle  HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Credit %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvCredit" runat="server" Text='<%#Eval("Credit") %>' />
                                                                </ItemTemplate>
                                                                <FooterStyle BorderStyle="None" HorizontalAlign="Right" />
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblgvCreditTotal" runat="server" />
                                                                </FooterTemplate>
                                                                <FooterStyle BorderStyle="None" HorizontalAlign="Right" Font-Bold="true" />
                                                                <ItemStyle  HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Closing Debit %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvClosingBalD" runat="server" Text='<%#Eval("Closing_Debit") %>' />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblgvClosingBalDTotal" runat="server" />
                                                                </FooterTemplate>
                                                                <FooterStyle BorderStyle="None" HorizontalAlign="Right" Font-Bold="true" />
                                                                <ItemStyle  HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Attendance, Closing Credit %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvClosingBalC" runat="server" Text='<%#Eval("Closing_Credit") %>' />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblgvClosingBalCTotal" runat="server" />
                                                                </FooterTemplate>
                                                                <FooterStyle BorderStyle="None" HorizontalAlign="Right" Font-Bold="true" />
                                                                <ItemStyle  HorizontalAlign="Right" />
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
    
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Update_New">
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
    </script>
</asp:Content>
