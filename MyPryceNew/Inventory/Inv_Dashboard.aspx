<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeFile="Inv_Dashboard.aspx.cs" Inherits="Inventory_Inv_Dashboard" MasterPageFile="~/ERPMaster.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <div class="box box-default">
        <div class="box-header with-border">
            <div class="col-md-3">
                <h3>Inventory
                            <small>Overview</small>
                </h3>
            </div>

            <div class="col-md-7">
                <form action="/SupplierDashboard/Index" id="frmDateFilter" method="post">
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
            <div class="box box-primary">
                <div class="box-header">
                    <div class="box-tools pull-right">
                        <a id="removeStoreFilter" style="display: none" title="Remove Store Filter" href="#" onclick="fnRemoveLocFilter(); return false;">
                            <img src="../Images/remove-filter_new.png" style="width: 25px; height: 25px; margin-right: 5px;" />
                        </a>
                        <button type="button" class="btn btn-primary btn-sm pull-right" data-widget="collapse" data-toggle="tooltip" title="" style="margin-right: 5px;" data-original-title="Collapse">
                            <i class="fa fa-minus"></i>
                        </button>
                    </div>
                </div>
                <div class="box-body">
                    <div class="col-md-8" id="inventoryCategoryChartContainer">
                        <canvas id="inventoryCategoryChart" class="chart" style="height: 250px"></canvas>
                    </div>
                    <div class="col-md-4">
                        <div class="table-responsive" style="height: 250px">
                            <table class="table table-hover" id="tblLocation">
                                <thead>
                                    <tr>
                                        <th style="display: none">Location Id</th>
                                        <th>Location</th>
                                        <th style="text-align: right">Stock</th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                                <tfoot>
                                </tfoot>

                            </table>
                        </div>
                    </div>
                </div>
            </div>

            <div class="box">
                <div class="box-header">
                    <div class="box-tools pull-right">
                        <button type="button" class="btn btn-box-tool" data-widget="collapse" data-toggle="tooltip" title="" data-original-title="Collapse">
                            <i class="fa fa-minus"></i>
                        </button>
                        <button type="button" class="btn btn-box-tool" data-widget="remove" data-toggle="tooltip" title="" data-original-title="Remove">
                            <i class="fa fa-times"></i>
                        </button>
                    </div>
                    <div class="col-md-2 pull-right">
                        <div id="lstNoOfItems" class="dropdown dropselect" style="margin-right: 20px">
                            <input type="hidden" name="btnTopNRecords" value="10">
                            <button class="btn btn-warning dropdown-toggle" type="button" id="btnTopNRecords" data-toggle="dropdown" aria-expanded="true">
                                <span class="dropdown-label">Dropdown</span>
                                <span class="caret"></span>
                            </button>
                            <ul class="dropdown-menu" role="menu" aria-labelledby="option-post-cleanup">
                                <li role="presentation"><a data-value="5" role="menuitem" tabindex="-1">Top 5</a></li>
                                <li role="presentation"><a data-value="10" data-selected="selected" role="menuitem" tabindex="-1">Top 10</a></li>
                                <li role="presentation"><a data-value="15" role="menuitem" tabindex="-1">Top 15</a></li>
                                <li role="presentation"><a data-value="20" role="menuitem" tabindex="-1">Top 20</a></li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div class="box-body">
                    <div class="col-md-12" id="fastMovingItemChartContainer">
                        <canvas id="fastMovingItemChart"></canvas>
                    </div>
                </div>
            </div>

            <div class="box box-primary">
                <div class="box-header with-border">
                    <div class="box-tools pull-right">
                        <button type="button" class="btn btn-box-tool" data-widget="collapse" data-toggle="tooltip" title="" data-original-title="Collapse">
                            <i class="fa fa-minus"></i>
                        </button>
                        <button type="button" class="btn btn-box-tool" data-widget="remove" data-toggle="tooltip" title="" data-original-title="Remove">
                            <i class="fa fa-times"></i>
                        </button>
                    </div>
                </div>
                <div class="box-body">
                    <div class="col-md-12" id="brandStockChartContainer">
                        <canvas id="brandStockChart" class="chart" style="height: 250px"></canvas>
                    </div>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="modal" id="modalDetail" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" onclick="fnHideModalDetail()">
                        <span aria-hidden="true">×</span>
                    </button>
                    <h4 class="modal-title" id="modalTitle">Title</h4>
                </div>
                <div class="modal-body" id="modalBody" style="overflow-y: scroll; height: 400px">
                    <div id="divTblProductSummary">
                        <table id="tblProductSummary" class="table table-hover" style="width: 100%">
                            <thead style="background-color: #dadada;">
                                <tr>
                                    <td>Product ID </td>
                                    <td>Product Name</td>
                                    <td>Unit Name</td>
                                    <td style="text-align: right">Stock Qty</td>
                                    <td style="text-align: right">Unit Price</td>
                                    <td style="text-align: right">Stock Value</td>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                            <tfoot>
                                <tr>
                                    <th colspan="3" style="text-align: right">Page Total:</th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                </tr>
                            </tfoot>
                        </table>
                    </div>
                    <div id="divTblProductFastMove">
                        <table id="tblProductFastMove" class="table table-hover" style="width: 100%">
                            <thead style="background-color: #dadada;">
                                <tr>
                                    <td>Trans Type </td>
                                    <td>Trans No</td>
                                    <td>Trans Date</td>
                                    <td>Unit Name</td>
                                    <td style="text-align: right">Qunatity</td>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                            <tfoot>
                                <tr>
                                    <th colspan="4" style="text-align: right">Page Total:</th>
                                    <th></th>
                                </tr>
                            </tfoot>
                        </table>
                    </div>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterContent" runat="Server">
    <div class="modal_Progress" id="divLoading" style="display:none">
        <div class="center_Progress">
            <img alt="" src="../Images/ajax-loader2.gif" style="vertical-align: middle; background-color: White; border-radius: 10px;" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AfterLoadScriptContent" runat="Server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.6.0/Chart.min.js" type="text/javascript"></script>
    <script src="../Script/Dashboard/dashboarMasterFilter.js" type="text/javascript"></script>
    <script src="../Script/Dashboard/inventoryDashboard.js" type="text/javascript"></script>
    <script src="../Script/jquery.dropselect.js" type="text/javascript"></script>
    <script type="text/javascript">
        var tblProduct;
        var tblProductFastMove;
        reDrawProductTable();
        reDrawProductFastMoveTable();
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

        function reDrawProductTable() {
            debugger;

            tblProduct = $('#tblProductSummary').DataTable({
                destroy: true,
                draw: true,
                dom: 'Bfrtip',
                buttons: [
                    'copy', 'csv', 'excel', 'pdf', 'print'
                ],
                "footerCallback": function (row, data, start, end, display) {
                    debugger;
                    var api = this.api(), data;

                    // Remove the formatting to get integer data for summation
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                            i : 0;
                    };

                    // Total over all pages
                    total = api
                        .column(3)
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);

                        }, 0);

                    // Total over this page
                    pageTotal = api
                        .column(3, { page: 'current' })
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);

                    // Update footer
                    $(api.column(3).footer()).html(
                        pageTotal.toFixed(3) + ' ( ' + total.toFixed(3) + ' total)'
                    );
                }

            });
        }
        function reDrawProductFastMoveTable() {
            debugger;

            tblProductFastMove = $('#tblProductFastMove').DataTable({
                destroy: true,
                draw: true,
                dom: 'Bfrtip',
                buttons: [
                    'copy', 'csv', 'excel', 'pdf', 'print'
                ],
                "footerCallback": function (row, data, start, end, display) {
                    debugger;
                    var api = this.api(), data;

                    // Remove the formatting to get integer data for summation
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                            i : 0;
                    };

                    // Total over all pages
                    total = api
                        .column(4)
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);

                        }, 0);

                    // Total over this page
                    pageTotal = api
                        .column(4, { page: 'current' })
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);

                    // Update footer
                    $(api.column(4).footer()).html(
                        pageTotal.toFixed(3) + ' ( ' + total.toFixed(3) + ' total)'
                    );
                }

            });
        }

    </script>
</asp:Content>

