<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="SendReportAsMail.aspx.cs" Inherits="_SendReportAsMail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <h1>
        <i class="fa fa-envelope"></i>
        <asp:Label ID="lblHeader" runat="server" Text="Send Mail"></asp:Label>
    </h1>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">

                <div class="tab-content">
                    <div class="tab-pane active" id="List">
                        <asp:UpdatePanel ID="Update_List" runat="server">
                            <ContentTemplate>


                                <div class="box box-primary box-solid">
                                    <div class="box-body">
                                        <div class="row">

                                            <div class="col-md-12">
                                                <div class="col-md-9">
                                                    Name:
                            <asp:TextBox runat="server" ID="txtCustomer" ClientIDMode="Static" CssClass="form-control" onchange="getContactIdFromNameNIdNSetSession(this,'ContactID');getEmpData();"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="txtCustomerName_AutoCompleteExtender" runat="server"
                                                        DelimiterCharacters="" Enabled="True" CompletionInterval="100" CompletionSetCount="1"
                                                        MinimumPrefixLength="1" ServiceMethod="GetCompletionListCustomer" ServicePath=""
                                                        TargetControlID="txtCustomer" UseContextKey="True" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem" CompletionListHighlightedItemCssClass="autoCompleteSelectedListItem" OnClientShown="resetPosition">
                                                    </cc1:AutoCompleteExtender>
                                                </div>
                                                <div class="col-md-3">
                                                    <br />
                                                    <a class="btn btn-primary" style="cursor: pointer" onclick="reset()">Reset</a>
                                                    <a class="btn btn-primary" style="cursor: pointer" onclick="sendMail()">Send Mail</a>
                                                    <a class="btn btn-primary" style="cursor: pointer" onclick="backButton()">Back</a>
                                                </div>
                                            <div class="col-lg-12">
                                                <br />
                                            </div>
                                            <div class="col-lg-12">
                                                <table id="tblContactEmail" class="table table-striped" style="width: 100%">
                                                    <thead>
                                                        <tr>
                                                            <td></td>
                                                            <td>Contact Person</td>
                                                            <td>Email</td>
                                                            <td>Mobile</td>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                    </tbody>
                                                </table>
                                            </div>

                                        </div>
                                    </div>
                                    <!-- /.box-body -->
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

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="../Script/common.js"></script>
    <script src="../Script/customer.js"></script>
    <script>
        function getEmpData() {
            var custid = setCustomerId();
            if (custid == "") {
                alert("Please select customer id");
                return false;
            }
            $('#tblContactEmail tbody tr').remove();

            PageMethods.getEmpData(custid, function (data) {
                var vico = JSON.parse(data);
                $(vico).each(function () {
                    var row = $(this)[0];
                    var htmlRow = "<tr><td><input type='checkbox' /></td>";
                    htmlRow = htmlRow + "<td>" + row.Name + "</td>";
                    htmlRow = htmlRow + "<td>" + row.email + "</td>";
                    htmlRow = htmlRow + "<td>" + row.contact + "</td>";
                    htmlRow = htmlRow + "</tr>";
                    $('#tblContactEmail > tbody:last-child').append(htmlRow);
                });
            }, function (data) { alert(data); });
            return true;
        }

        function setCustomerId() {
            var ctl = $('#txtCustomer').val();
            if (ctl == "") {
                return "";
            }
            return ctl.split('/')[1];
        }
        function reset() {
            $('#txtCustomer').val('');
            $('#tblContactEmail tbody tr').remove();
        }
        function backButton()
        {
            parent.history.back();
        }
        function sendMail()
        {
            var emails = '';
            $('#tblContactEmail tbody tr').each(function () {
                debugger;
                var row = $(this)[0];
                var checkbox = row.cells[0].childNodes[0];
                if (checkbox.checked == true) {
                    emails+=row.cells[2].innerHTML+';';
                }
            });

            PageMethods.setEmailAddress(emails, function (data) {
                if(data!='true')
                {
                    showAlert('Due to some problems we cannot process your request, please try again after some time','orange','white');
                }
                else
                {
                    window.open('../EmailSystem/SendMail.aspx?RS=1', '_blank', 'width=1024, ');
                }
            }, function (data) { alert(data) });
        }
    </script>
</asp:Content>

