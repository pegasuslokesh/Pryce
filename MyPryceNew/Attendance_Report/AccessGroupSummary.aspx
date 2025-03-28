<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" EnableEventValidation="true" CodeFile="AccessGroupSummary.aspx.cs" Inherits="Attendance_Report_AccessGroupSummary" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register TagPrefix="exp1" Namespace="ControlFreak" Assembly="ExportPanel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <img src="../Images/InvStyle.png" alt="" />
        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Attendance,Access Group Summary%>"></asp:Label>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Attendance,Attendance Module%>"></asp:Label></a></li>
        <li><a href="#"><i class="fa fa-dashboard"></i>
            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Attendance,Attendance Report%>"></asp:Label></a></li>
        <li class="active">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Attendance,Access Group Summary%>"></asp:Label></li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="Update_List" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDownload" />

        </Triggers>

        <ContentTemplate>
            <asp:Panel ID="pnlReport" runat="server">
                <div class="row">
                    <div class="col-md-12">
                        <div class="box box-primary">
                            <div class="box-body">
                                <div class="form-group">
                                    <div class="col-md-6" style="text-align: left;">
                                        <asp:LinkButton ID="lnkback" runat="server" CssClass="acc" OnClick="lnkback_Click"
                                            Text="<%$ Resources:Attendance,Back %>"></asp:LinkButton>
                                        <br />
                                    </div>
                                    <div class="col-md-6" style="text-align: right;">

                                        <asp:Button ID="btnDownload" runat="server" Text="<%$ Resources:Attendance,Download%>" CssClass="btn btn-primary" OnClick="btnDownload_Click" CausesValidation="False" />

                                        <%--<asp:ImageButton ID="btnExportPdf" runat="server" CommandArgument="1" CommandName="OP"
                                            Height="23px" ImageUrl="~/Images/pdfIcon.jpg" OnCommand="btnExportPdf_Command" />
                                        <asp:ImageButton ID="btnExportToExcel" runat="server" CommandArgument="2" CommandName="OP"
                                            Height="21px" ImageUrl="~/Images/excel-icon.gif" OnCommand="btnExportPdf_Command" />--%>
                                        <br />
                                    </div>
                                    
                                    <div class="col-md-12" runat="server" id="divexport">
                                        <div class="col-md-12" style="overflow: auto; max-height: 400px; width:100%;">
                                            <br />


                                            <asp:Table ID="Table1" runat="server" BorderStyle="Solid" CellPadding="1" CellSpacing="1" Font-Size="12px"
                                                    GridLines="Both" Height="22px" Width="100%">
                                            </asp:Table>
                                            <br />

                                        </div>
                                        <div class="col-md-12" >
                                            &nbsp;
                                            </div>


                                        <div class="col-md-12" >
                                            <br />
                                              <asp:Table ID="tblColorCode"  runat="server" GridLines="Both" Width="100%" Font-Size="12px" Height="22px">
                                             </asp:Table>




                                            <asp:Label ID="lblweekoff" runat="server" Text="WO-Week OFF" Style="margin-right: 30px;" Visible="false"></asp:Label>


                                            <asp:Label ID="lblMissedout" runat="server" Text="MO-Missed OUT" Style="margin-right: 30px;" Visible="false"></asp:Label>

                                            <asp:Label ID="lblMissedIn" runat="server" Text="MI-Missed IN" Style="margin-right: 30px;" Visible="false"></asp:Label>

                                            <asp:Label ID="lblabsent" runat="server" Text="AB-Absent" Style="margin-right: 30px;" Visible="false"></asp:Label>

                                            <asp:Label ID="lbllatein" runat="server" Text="Late Check IN" Style="margin-right: 30px;" Visible="false"></asp:Label>

                                            <asp:Label ID="lblearlyout" runat="server" Text="Early Check OUT" Style="margin-right: 30px;" Visible="false"></asp:Label>

                                            <asp:Label ID="lblleave" runat="server" Text="Leave" Style="margin-right: 30px;" Visible="false"></asp:Label>

                                            <asp:Label ID="lblHoliday" runat="server" Text="PH-Public Holiday" Style="margin-right: 30px;" Visible="false"></asp:Label>

                                            <asp:Label ID="lblPresent" runat="server" Text="Present" Style="margin-right: 30px;" Visible="false"></asp:Label>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Update_List">
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
</asp:Content>

