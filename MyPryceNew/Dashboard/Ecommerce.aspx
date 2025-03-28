<%@ Page Title="" Language="C#" MasterPageFile="~/ERPMaster.master" AutoEventWireup="true" CodeFile="Ecommerce.aspx.cs" Inherits="Dashboard_Ecommerce" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="box-header with-border">
        <div class="col-md-3">
            <h3>ECommerce
                            <small></small>
            </h3>
        </div>

        <div class="col-md-7">
            <form action="/Ecommerce/Index" id="frmDateFilter" method="post">
                <asp:HiddenField ID="hdnFilterType" ClientIDMode="Static" Value="0" runat="server" />
                <asp:HiddenField ID="hdnLocationId" Value="0" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnFilterFromDate" Value="0" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnFilterToDate" Value="0" runat="server" ClientIDMode="Static" />
                <div class="col-md-8" id="divCustomDateFilter" style="display: none">
                    <div class="col-md-5">
                        <div class="input-group date">
                            <div class="input-group-addon">
                                <i class="fa fa-calendar"></i>
                            </div>
                            <input class="form-control pull-right" id="FromDate" name="FromDate" type="text" value="">
                        </div>
                    </div>
                    <div class="col-md-5">
                        <div class="input-group date">
                            <div class="input-group-addon">
                                <i class="fa fa-calendar"></i>
                            </div>
                            <input class="form-control pull-right" id="ToDate" name="ToDate" type="text" value="">
                        </div>
                    </div>
                    <div class="col-md-2">
                        <a onclick="fnCustomDateOnChange();"><i class="fa fa-refresh" style="font-size: 30px;"></i></a>
                    </div>
                </div>
                <div class="col-md-4 pull-right">
                    <select class="form-control select2" id="lstFilterType" name="lstFilterType" onchange="fnDateFilterOnChange()">
                        <option value="0">Today</option>
                        <option value="1">Yesterday</option>
                        <option value="2">This Week</option>
                        <option selected="selected" value="3">This Month</option>
                        <option value="4">This Quarter</option>
                        <option value="5">This Year</option>
                        <option value="6">Custom</option>
                    </select>
                    <input type="hidden" id="purchaseType" name="purchaseType">
                </div>
            </form>
        </div>
    </div>
    <div class="box-body">
        <h3> Order Dispatch Graph </h3>
        <div class="col-md-8" id="inventoryCategoryChartContainer">
            <canvas id="inventoryCategoryChart" class="chart" style="height: 250px"></canvas>
        </div>
        <div class="col-md-4">
            <div class="table-responsive" style="height: 250px">
                <table class="table table-striped table-bordered" id="tblmerchant">
                    <thead>
                        <tr>
                            
                            <th>Merchant</th>
                            <th style="text-align: right">Orders</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                    <tfoot>
                    </tfoot>

                </table>
            </div>
        </div>
         <div class="col-md-12">
               <table class="table table-striped table-bordered" id="tblorderproduct">
                    <thead>
                        <tr>
                            <th>Merchant Name</th>
                            <th>Product Name</th>
                            <th style="text-align: right">Qty</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                    <tfoot>
                    </tfoot>

                </table>
        </div>

        <hr />
         <h3> Order Return Graph </h3>
        <div class="col-md-8" id="inventoryCategoryChartContainer1">
            <canvas id="inventoryCategoryChart1" class="chart" style="height: 250px"></canvas>
        </div>
        <div class="col-md-4">
            <div class="table-responsive" style="height: 250px">
                <table class="table table-striped table-bordered" id="tblmerchant1">
                    <thead>
                        <tr>
                            
                            <th>Merchant</th>
                            <th style="text-align: right">Returns</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                    <tfoot>
                    </tfoot>

                </table>
            </div>
        </div>

        <div class="col-md-12">
               <table class="table table-striped table-bordered" id="tblorderreturnproduct">
                    <thead>
                        <tr>
                            <th>Merchant Name</th>
                            <th>Product Name</th>
                            <th style="text-align: right">Qty</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                    <tfoot>
                    </tfoot>

                </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
    <%--  <div class="modal_Progress" id="divLoading" style="display:none">
        <div class="center_Progress">
            <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
        </div>
    </div>--%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">

    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.6.0/Chart.min.js" type="text/javascript"></script>
    <script src="../Script/Dashboard/dashboarMasterFilter.js" type="text/javascript"></script>
    <script src="../Script/Dashboard/ecommerce.js" type="text/javascript"></script>
    <script src="../Script/jquery.dropselect.js" type="text/javascript"></script>
    <script type="text/javascript">
        var tblProduct;
        var tblProductFastMove;

        //var gStoreNo = $('#hdnLocationId').val();
        //var gFromDate = $('#hdnFilterFromDate').val();
        //var gToDate = $('#hdnFilterToDate').val();
        var gStoreNo = "";
        var gFromDate = "";
        var gToDate = "";
        var gCustomFilerEnumVal = "6";
        fnOnChangeFilterType();



        if ($('#hdnFilterFromDate').val() != undefined && $('#hdnFilterFromDate').val() == "") {
            $('#FromDate').val($('#hdnFilterFromDate').val());
        }

        if ($('#hdnFilterToDate').val() != undefined && $('#hdnFilterToDate').val() == "") {
            $('#ToDate').val($('#hdnFilterToDate').val());
        }

        if ($('#hdnFilterType').val() != undefined && $('#hdnFilterType').val() == "") {
            $('#lstFilterType').val($('#hdnFilterType').val());
            if ($('#lstFilterType').val() == '6') {
                $('#divCustomDateFilter').show();
            }
        }




        function fnHideModalDetail() {
            $('#modalDetail').hide();
        }

        function fnShowProgress() {
            $('#divLoading').show();
        }

        function fnHideProgress() {
            $('#divLoading').hide();
        }

        $(document).ready(function () {

        });

        //$('#tblorderproduct').DataTable({
        //    "pagingType": "full_numbers",
        //    dom: 'Bfrtip',

        //    buttons: [
        //        'copy', 'csv', 'excel', 'pdf', 'print'
        //    ]
        //});

    </script>
</asp:Content>

