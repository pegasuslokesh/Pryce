window.onload = function () {


    //fnShowProgress();
    //fnGetPayMethodChart(gStoreNo);
    //fnGetSalesCategoryChart(gStoreNo);
    //fnGetSalesPersonChart(gStoreNo);
    //fnGetPosSalesChart(gStoreNo);
    //fnGetBrandSalesChart(gStoreNo);
    //fnHideProgress();
    //fnGetDummyChart('@ViewBag.StoreNo');
}

function fnOnChangeFilterType() {
    getFilterValue();
    fnFillLocationTbl();
    debugger;
    fnShowProgress();
    fnGetSalesCategoryChart(gStoreNo);
    fnGetSalesPersonChart(gStoreNo);
    fnGetBrandSalesChart(gStoreNo);
    fnGetPayMethodChart(gStoreNo);
    //fnGetBrandStockChart(gStoreNo);
    //fnGetFastMovingItemChart(gStoreNo);
}

function fnRemoveLocFilter() {
    try {
        debugger;
        var storeNo = "";
        gStoreNo = storeNo;
        $('#tblLocation tbody tr').each(function () {
            $(this).removeClass('active');
        });

        $('#removeStoreFilter').hide();
        //call function to filter chart data
        fnGetSalesCategoryChart(gStoreNo);
        fnGetSalesPersonChart(gStoreNo);
        fnGetBrandSalesChart(gStoreNo);
        fnGetPayMethodChart(gStoreNo);
    }
    catch (err) {

    }
}

function getFilterValue() {
    debugger;
    if ($('#lstFilterType').val() == gCustomFilerEnumVal) {
        return;
    }
    try {
        debugger;
        $.ajax({
            url: "SalesDashboard.aspx/GetDashboardFilterParamValue",
            type: "POST",
            async: false,
            data: "{from_date:'" + gFromDate + "',to_date:'" + gToDate + "',store_no:'" + gStoreNo + "',filter_type:'" + $('#lstFilterType').val() + "'}",
            contentType: "application/json;charset=utf-8",
            dataType: "Json",
            success: function (ab) {
                debugger;
                var result = JSON.parse(ab.d);
                gFromDate = result.from_date;
                gToDate = result.to_date;
                $("#FromDate").val(gFromDate);
                $("#ToDate").val(gToDate);
                fnHideProgress();
            },
            error: function (ex) {
                fnHideProgress();
            }
        });
    }
    catch (err) {

    }
}

function labelFormatter(label, series, color) {
    return '<div style="font-size:8px; text-align:center; padding:2px; color: ' + color + '; font-weight: 400;">'
      + label
      + '<br>'
      + Math.round(series.percent) + '</div>'
}

//$('#tblStoreWiseSales tr').click(function () {
//    try {
//        var _result = fnValidateCustomFilter();
//        if (_result == false) {
//            return;
//        }
//        var row = $(this);
//        var storeNo = row.find("td:eq(0)").text();
//        if (storeNo == undefined || storeNo == "") {
//            return;
//        }
//        $('#tblStoreWiseSales tbody tr').each(function () {
//            $(this).removeClass('active');
//        });
//        row.addClass('active');
//        $('#removeStoreFilter').show();
//        //call function to filter chart data
//        fnGetPayMethodChart(storeNo);
//        fnGetSalesCategoryChart(storeNo);
//        fnGetSalesPersonChart(storeNo);
//        fnGetPosSalesChart(storeNo);
//        fnGetBrandSalesChart(storeNo);

//    }
//    catch (err) {

//    }
//});

function fnGetPayMethodChart(store_no) {
    try {
        var fromDate = gFromDate;
        var toDate = gToDate;
        var xValues;
        var Ids;
        fnShowProgress();
        $.ajax({
            url: "SalesDashboard.aspx/GetPayMethodChartData",
            type: "POST",
            contentType: "application/json;charset=utf-8",
            data: "{store_no:'" + store_no + "',from_date:'" + fromDate + "',to_date:'" + toDate + "'}",
            dataType: "Json",
            success: function (result) {
                debugger;
                result = JSON.parse(result.d);
                var aLables = result[0];
                xValues = result[0];
                var aDataset1 = result[1];
                Ids = result[0];
                var aData = {
                    labels: aLables,
                    datasets: [{
                        label: "Pay By -",
                        data: aDataset1,
                        fill: false,
                        backgroundColor: ["green", "red", "blue", "yellow", "brown", "white", "black", "orange", "cyan"],
                        borderColor: ["rgb(54, 162, 235)", "rgb(255, 99, 132)", "rgb(255, 159, 64)", "rgb(255, 205, 86)", "rgb(75, 192, 192)", "rgb(153, 102, 255)", "rgb(201, 203, 207)"],
                        borderWidth: 1
                    }]
                };
                try {
                    //remove previos graph
                    $('#payMethodChart').remove();
                    $('#payMethodChartContainer').append("<canvas id='payMethodChart' class='chart' style='height:250px'></canvas>");
                }
                catch (err) {

                }
                var ctx = $('#payMethodChart').get(0).getContext("2d");
                var myNewChart = new Chart(ctx, {
                    type: 'doughnut',
                    data: aData,
                    options: {
                        responsive: true,
                        title: { display: true, text: 'PAYMENT OVERVIEW' },
                        legend: { position: 'top' },
                        scales: {
                            xAxes: [{ gridLines: { display: false }, display: false, scaleLabel: { display: false, labelString: '' } }],
                            yAxes: [{ gridLines: { display: false }, display: false, scaleLabel: { display: false, labelString: '' } }]
                        },
                    }
                });
                document.getElementById("payMethodChart").onclick = function (evt) {
                    try {
                        debugger;
                        var activePoints = myNewChart.getElementsAtEvent(evt);
                        var firstPoint = activePoints[0];
                        //var label = scatterChart.data.labels[firstPoint._index];
                        //var label = myNewChart.data.datasets[firstPoint._index].label;
                        //var value = myNewChart.data.datasets[firstPoint._datasetIndex].data[firstPoint._index];

                        var _id = Ids[firstPoint._index];
                        var title = xValues[firstPoint._index];
                        if (_id != undefined) {
                            $.ajax({
                                url: "SalesDashboard.aspx/GetSinvHeaderByPayMethod",
                                type: "POST",
                                contentType: "application/json;charset=utf-8",
                                data: "{store_no:'" + store_no + "',from_date:'" + fromDate + "',to_date:'" + toDate + "',payment_id:'" + _id + "'}",
                                success: function (result) {
                                    debugger;
                                    var result = JSON.parse(result.d);
                                    $('#divTblProductSummary').hide()
                                    $('#divTblSalesHeader').show()
                                    $('#tblSalesHeader tbody').empty();
                                    tblSalesHeader.clear();
                                    tblSalesHeader.destroy();
                                    $(result).each(function (key, item) {
                                        var htmlRow = "<tr id='" + item.trans_id + "'>";
                                        htmlRow = htmlRow + "<td>" + item.trns_no + "</td>";
                                        htmlRow = htmlRow + "<td>" + item.trns_date + "</td>";
                                        htmlRow = htmlRow + "<td>" + item.customer_name + "</td>";
                                        htmlRow = htmlRow + "<td style='text-align:right'>" + item.invoice_amount + "</td>";
                                        htmlRow = htmlRow + "<td style='text-align:right'>" + item.pay_amount + "</td>";
                                        htmlRow = htmlRow + "<td>" + item.created_by + "</td>";
                                        htmlRow = htmlRow + "</tr>";
                                        $('#tblSalesHeader > tbody').append(htmlRow);
                                    });
                                    //tblProduct.draw();
                                    reDrawSalesHeaderTable();
                                    $('#modalTitle').html('');
                                    $('#modalTitle').html(title);
                                    $('#modalDetail').show();
                                    fnHideProgress();

                                },
                                error: function (err) {
                                    debugger;
                                    alert(err.message);
                                }
                            });
                        }
                        if (firstPoint !== undefined) {
                            //alert(label + ": " + value.x);
                        }
                    }
                    catch (err) {

                    }

                };
                fnHideProgress();
            },

            error: function (ex) {
                fnHideProgress();
            }
        });

    }
    catch (err) {
        fnHideProgress();
    }
}

function fnGetPosSalesChart(store_no) {
    try {
        var fromDate = gFromDate;
        var toDate = gToDate;
        var xValues;
        var Ids;
        fnShowProgress();
        $.ajax({
            url: "GetPosSalesChartData",
            type: "POST",
            contentType: "application/json;charset=utf-8",
            data: "{store_no:'" + store_no + "',from_date:'" + fromDate + "',to_date:'" + toDate + "'}",
            dataType: "Json",
            success: function (result) {

                var aLables = result[0];
                xValues = result[0];
                var aDataset1 = result[1];
                Ids = result[2];
                var aData = {
                    labels: aLables,
                    datasets: [{
                        label: "Sales",
                        data: aDataset1,
                        fill: true,
                        backgroundColor: ["rgba(167, 105, 0, 0.4)"],
                        borderColor: ["rgb(54, 162, 235)", "rgb(255, 99, 132)", "rgb(255, 159, 64)", "rgb(255, 205, 86)", "rgb(75, 192, 192)", "rgb(153, 102, 255)", "rgb(201, 203, 207)"],
                        borderWidth: 1
                    }]
                };
                try {
                    //remove previos graph
                    $('#posSalesChart').remove();
                    $('#posSalesChartContainer').append("<canvas id='posSalesChart' class='chart' style='height:250px'></canvas>");
                }
                catch (err) {

                }
                var ctx = $('#posSalesChart').get(0).getContext("2d");
                var myNewChart = new Chart(ctx, {
                    type: 'line',
                    data: aData,
                    options: {
                        responsive: true,
                        title: { display: true, text: 'POS OVERVIEW' },
                        legend: { position: 'bottom' },
                        scales: {
                            xAxes: [{ gridLines: { display: false }, display: true, scaleLabel: { display: true, labelString: '' } }]
                        },
                    }
                });
                document.getElementById("posSalesChart").onclick = function (evt) {
                    try {
                        debugger;
                        var activePoints = myNewChart.getElementsAtEvent(evt);
                        var firstPoint = activePoints[0];
                        var _id = Ids[firstPoint._index];
                        var title = xValues[firstPoint._index];
                        if (_id != undefined) {
                            $.ajax({
                                url: "GetSinvHeaderByPos",
                                type: "POST",
                                contentType: "application/json;charset=utf-8",
                                data: "{store_no:'" + store_no + "',from_date:'" + fromDate + "',to_date:'" + toDate + "',pos_no:'" + _id + "'}",
                                success: function (result) {
                                    debugger;
                                    //remove existing data
                                    $('#modalBody').html('');
                                    //add new data
                                    $('#modalBody').html(result);
                                    $('#modalTitle').html('');
                                    $('#modalTitle').html(title);
                                    $('#modalDetail').show();

                                },
                                error: function (err) {
                                    debugger;
                                    alert(err.message);
                                }
                            });
                        }
                        if (firstPoint !== undefined) {
                            //alert(label + ": " + value.x);
                        }
                    }
                    catch (err) {

                    }

                };
                fnHideProgress();
            },

            error: function (ex) {
                fnHideProgress();
            }
        });

    }
    catch (err) {
        fnHideProgress();
    }
}

function fnGetBrandSalesChart(store_no) {
    try {
        var fromDate = gFromDate;
        var toDate = gToDate;
        var xValues;
        var Ids;
        fnShowProgress();
        $.ajax({
            url: "SalesDashboard.aspx/GetBrandSalesChartData",
            type: "POST",
            contentType: "application/json;charset=utf-8",
            data: "{store_no:'" + store_no + "',from_date:'" + fromDate + "',to_date:'" + toDate + "'}",
            dataType: "Json",
            success: function (result) {
                debugger;
                result = JSON.parse(result.d);
                var aLables = result[0];
                xValues = result[0];
                var aDataset1 = result[1];
                Ids = result[2];
                var aData = {
                    labels: aLables,
                    datasets: [{
                        label: "Sales",
                        data: aDataset1,
                        fill: true,
                        backgroundColor: ["cyan"],
                        borderColor: ["rgb(54, 162, 235)", "rgb(255, 99, 132)", "rgb(255, 159, 64)", "rgb(255, 205, 86)", "rgb(75, 192, 192)", "rgb(153, 102, 255)", "rgb(201, 203, 207)"],
                        borderWidth: 1
                    }]
                };
                try {
                    //remove previos graph
                    $('#brandSalesChart').remove();
                    $('#brandSalesChartContainer').append("<canvas id='brandSalesChart' class='chart' style='height:250px'></canvas>");
                }
                catch (err) {

                }
                var ctx = $('#brandSalesChart').get(0).getContext("2d");
                var myNewChart = new Chart(ctx, {
                    type: 'line',
                    data: aData,
                    options: {
                        responsive: true,
                        title: { display: true, text: 'BRANDS SALES OVERVIEW' },
                        legend: { position: 'top' },
                        scales: {
                            xAxes: [{ gridLines: { display: false }, display: false, scaleLabel: { display: true, labelString: '' } }]
                        },
                    }
                });
                document.getElementById("brandSalesChart").onclick = function (evt) {
                    try {
                        debugger;
                        var activePoints = myNewChart.getElementsAtEvent(evt);
                        var firstPoint = activePoints[0];
                        var _id = Ids[firstPoint._index];
                        var title = xValues[firstPoint._index];
                        if (_id != undefined) {
                            $.ajax({
                                url: "SalesDashboard.aspx/GetProductSummaryForBrand",
                                type: "POST",
                                contentType: "application/json;charset=utf-8",
                                data: "{store_no:'" + store_no + "',from_date:'" + fromDate + "',to_date:'" + toDate + "',brand_id:'" + _id + "'}",
                                success: function (result) {
                                    debugger;
                                    var result = JSON.parse(result.d);
                                    $('#divTblProductSummary').show()
                                    $('#divTblSalesHeader').hide()
                                    $('#tblProductSummary tbody').empty();
                                    tblProduct.clear();
                                    tblProduct.destroy();
                                    $(result).each(function (key, item) {
                                        var htmlRow = "<tr id='" + item.product_id + "'>";
                                        htmlRow = htmlRow + "<td>" + item.product_code + "</td>";
                                        htmlRow = htmlRow + "<td>" + item.product_name + "</td>";
                                        htmlRow = htmlRow + "<td style='text-align:right'>" + item.total_sale + "</td>";
                                        htmlRow = htmlRow + "<td style='text-align:right'>" + item.total_cost + "</td>";
                                        htmlRow = htmlRow + "<td style='text-align:right'>" + item.profit_per + "</td>";
                                        htmlRow = htmlRow + "<td style='text-align:right'>" + item.profit_amt + "</td>";
                                        htmlRow = htmlRow + "</tr>";
                                        $('#tblProductSummary > tbody').append(htmlRow);
                                    });
                                    //tblProduct.draw();
                                    reDrawProductTable();
                                    $('#modalTitle').html('');
                                    $('#modalTitle').html(title);
                                    $('#modalDetail').show();
                                    fnHideProgress();

                                },
                                error: function (err) {
                                    debugger;
                                    alert(err.message);
                                    fnHideProgress();
                                }
                            });
                        }
                        if (firstPoint !== undefined) {
                            //alert(label + ": " + value.x);
                        }
                    }
                    catch (err) {

                    }

                };
                fnHideProgress();

            },

            error: function (ex) {
                fnHideProgress();
            }
        });

    }
    catch (err) {
        fnHideProgress();
    }
}

function fnGetSalesPersonChart(store_no) {
    try {
        debugger;
        var fromDate = gFromDate;
        var toDate = gToDate;
        var xValues;
        var Ids;
        fnShowProgress();
        $.ajax({
            url: "SalesDashboard.aspx/GetSalesPersonChartData",
            type: "POST",
            contentType: "application/json;charset=utf-8",
            data: "{store_no:'" + store_no + "',from_date:'" + fromDate + "',to_date:'" + toDate + "'}",
            dataType: "Json",
            success: function (result) {
                debugger;
                result = JSON.parse(result.d);
                var aLables = result[0];
                xValues = result[0];
                var aDataset1 = result[1]; //Sales quota
                var aDataset2 = result[2]; //Forecast amt
                var aDataset3 = result[3]; //Order amt
                var aDataset4 = result[4]; //Invoice amt

                Ids = result[5];
                var aData = {
                    labels: aLables,
                    datasets: [{
                        label: "Monthly Sales Quota -",
                        data: aDataset1,
                        fill: true,
                        backgroundColor: ["green", "green", "green", "green", "green", "green", "green", "green", "green", "green", "green", "green", "green", "green", "green", "green", "green", "green", "green", "green"],
                        borderColor: ["green", "green", "green", "green", "green", "green", "green", "green", "green", "green", "green", "green", "green", "green", "green", "green", "green", "green", "green", "green"],
                        borderWidth: 1
                    },
                    {
                        label: "Forecast Amount -",
                        data: aDataset2,
                        fill:true,
                        backgroundColor: ["red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red"],
                        borderColor: ["red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red"],
                        borderWidth: 1
                    },
                    {
                        label: "Order Amount -",
                        data: aDataset3,
                        fill: true,
                        backgroundColor: ["blue", "blue", "blue", "blue", "blue", "blue", "blue", "blue", "blue", "blue", "blue", "blue", "blue", "blue", "blue", "blue", "blue"],
                        borderColor: ["blue", "blue", "blue", "blue", "blue", "blue", "blue", "blue", "blue", "blue", "blue", "blue", "blue", "blue", "blue", "blue", "blue"],
                        borderWidth: 1
                    },
                    {
                        label: "Invoice Amount -",
                        data: aDataset4,
                        fill: true,
                        backgroundColor: ["yellow", "yellow", "yellow", "yellow", "yellow", "yellow", "yellow", "yellow", "yellow", "yellow", "yellow", "yellow", "yellow", "yellow", "yellow", "yellow", "yellow", "yellow", "yellow", "yellow"],
                        borderColor: ["yellow", "yellow", "yellow", "yellow", "yellow", "yellow", "yellow", "yellow", "yellow", "yellow", "yellow", "yellow", "yellow", "yellow", "yellow", "yellow", "yellow", "yellow", "yellow", "yellow"],
                        borderWidth: 1
                    }]
                };

                try {
                    //remove previos graph
                    $('#salesPersonChart').remove();
                    $('#salesPersonChartContainer').append("<canvas id='salesPersonChart' class='chart' style='height:350px'></canvas>");
                }
                catch (err) {

                }
                var ctx = $('#salesPersonChart').get(0).getContext("2d");
                var myNewChart = new Chart(ctx, {
                    type: 'bar',
                    data: aData,
                    options: {
                        responsive: true,
                        title: { display: true, text: 'Summary Sales Forecast By Sales Person' },
                        legend: { position: 'top' },
                        scales: {
                            xAxes: [{ gridLines: { display: true }, display: true, scaleLabel: { display: false, labelString: '' } }],
                            yAxes: [{ gridLines: { display: true }, display: true, scaleLabel: { display: false, labelString: '' } }]
                        },
                    }
                });

                document.getElementById("salesPersonChart").onclick = function (evt) {
                    try {
                        debugger;
                        var activePoints = myNewChart.getElementsAtEvent(evt);
                        var firstPoint = activePoints[0];
                        //var label = scatterChart.data.labels[firstPoint._index];
                        //var label = myNewChart.data.datasets[firstPoint._index].label;
                        //var value = myNewChart.data.datasets[firstPoint._datasetIndex].data[firstPoint._index];

                        var _id = Ids[firstPoint._index];
                        var title = xValues[firstPoint._index];
                        if (_id != undefined) {
                            $.ajax({
                                url: "GetSinvHeaderBySalesPerson",
                                type: "POST",
                                contentType: "application/json;charset=utf-8",
                                data: "{store_no:'" + store_no + "',from_date:'" + fromDate + "',to_date:'" + toDate + "',sman_no:'" + _id + "'}",
                                success: function (result) {
                                    debugger;
                                    //remove existing data
                                    $('#modalBody').html('');
                                    //add new data
                                    $('#modalBody').html(result);
                                    $('#modalTitle').html('');
                                    $('#modalTitle').html(title);
                                    $('#modalDetail').show();

                                },
                                error: function (err) {
                                    debugger;
                                    //alert(err.message);
                                }
                            });
                        }
                        if (firstPoint !== undefined) {
                            //alert(label + ": " + value.x);
                        }
                    }
                    catch (err) {

                    }

                };
                fnHideProgress();
            },

            error: function (ex) {
                debugger;
                fnHideProgress();
            }
        });

    }
    catch (err) {
        fnHideProgress();
    }
}

function fnGetSalesCategoryChart(store_no) {
    try {
        debugger;
        var fromDate = gFromDate;
        var toDate = gToDate;
        var catIds;
        var catNames;
        fnShowProgress();
        $.ajax({
            url: "SalesDashboard.aspx/GetSalesCategoryChartData",
            type: "POST",
            data: "{store_no:'" + store_no + "',from_date:'" + fromDate + "',to_date:'" + toDate + "',filter_type:''}",
            contentType: "application/json;charset=utf-8",
            dataType: "Json",
            success: function (result) {
                debugger;
                result = JSON.parse(result.d);
                var aLables = result[0];
                catNames = result[0];
                var aDataset1 = result[1];
                var aDataset2 = result[2];
                catIds = result[3];
                var aData = {
                    labels: aLables,
                    datasets: [{
                        label: "Sales",
                        data: aDataset1,
                        fill: false,
                        backgroundColor: ["green"],
                        borderColor: ["green"],
                        borderWidth: 1
                    }, {
                        label: "Cost",
                        data: aDataset2,
                        fill: true,
                        backgroundColor: ["red"],
                        borderColor: ["red"],
                        borderWidth: 1
                    }]
                };
                try {
                    //remove previos graph salesCategoryChart
                    $('#salesCategoryChart').remove();
                    $('#salesCategoryChartContainer').append("<canvas id='salesCategoryChart' class='chart' style='height:250px'></canvas>");
                }
                catch (err) {

                }
                var ctx = $('#salesCategoryChart').get(0).getContext("2d");
                var myNewChart = new Chart(ctx, {
                    type: 'line',
                    data: aData,
                    options: {
                        responsive: true,
                        title: { display: true, text: 'CATEGORY WISE SALES AND COST' },
                        legend: { position: 'top' },
                        scales: {
                            xAxes: [{ gridLines: { display: false }, display: false, scaleLabel: { display: true, labelString: '' } }],
                            yAxes: [{ gridLines: { display: true }, display: true, scaleLabel: { display: true, labelString: '' } }]
                        },
                    }
                });
                document.getElementById("salesCategoryChart").onclick = function (evt) {
                    try {
                        var activePoints = myNewChart.getElementsAtEvent(evt);
                        var firstPoint = activePoints[0];
                        //var label = scatterChart.data.labels[firstPoint._index];
                        //var label = myNewChart.data.datasets[firstPoint._index].label;
                        //var value = myNewChart.data.datasets[firstPoint._datasetIndex].data[firstPoint._index];
                        debugger;
                        var catId = catIds[firstPoint._index];
                        var title = catNames[firstPoint._index];
                        if (catId != undefined) {
                            fnShowProgress();
                            $.ajax({
                                url: "SalesDashboard.aspx/GetProductSummaryForCategory",
                                type: "POST",
                                contentType: "application/json;charset=utf-8",
                                data: "{store_no:'" + store_no + "',from_date:'" + fromDate + "',to_date:'" + toDate + "',category_id:'" + catId + "'}",
                                success: function (result) {
                                    debugger;
                                    var result = JSON.parse(result.d);
                                    $('#divTblProductSummary').show()
                                    $('#divTblSalesHeader').hide()
                                    $('#tblProductSummary tbody').empty();
                                    tblProduct.clear();
                                    tblProduct.destroy();
                                    $(result).each(function (key, item) {
                                        var htmlRow = "<tr id='" + item.product_id + "'>";
                                        htmlRow = htmlRow + "<td>" + item.product_code + "</td>";
                                        htmlRow = htmlRow + "<td>" + item.product_name + "</td>";
                                        htmlRow = htmlRow + "<td style='text-align:right'>" + item.total_sale + "</td>";
                                        htmlRow = htmlRow + "<td style='text-align:right'>" + item.total_cost + "</td>";
                                        htmlRow = htmlRow + "<td style='text-align:right'>" + item.profit_per + "</td>";
                                        htmlRow = htmlRow + "<td style='text-align:right'>" + item.profit_amt + "</td>";
                                        htmlRow = htmlRow + "</tr>";
                                        $('#tblProductSummary > tbody').append(htmlRow);
                                    });
                                    //tblProduct.draw();
                                    reDrawProductTable();
                                    $('#modalTitle').html('');
                                    $('#modalTitle').html(title);
                                    $('#modalDetail').show();
                                    fnHideProgress();
                                },
                                error: function (err) {
                                    debugger;
                                    alert(err.message);
                                    fnHideProgress();
                                }
                            });
                        }
                        if (firstPoint !== undefined) {
                            //alert(label + ": " + value.x);
                        }
                    }
                    catch (err) {

                    }

                };
            },

            error: function (ex) {
                fnHideProgress();
            }
        });


    }
    catch (err) {
        fnHideProgress();
    }
}

function fnFillLocationTbl() {
    try {
        debugger;
        var fromDate = gFromDate;
        var toDate = gToDate;
        fnShowProgress();
        $.ajax({
            url: "SalesDashboard.aspx/GetLocationWiseSales",
            type: "POST",
            data: "{from_date:'" + fromDate + "',to_date:'" + toDate + "'}",
            contentType: "application/json;charset=utf-8",
            dataType: "Json",
            success: function (ab) {
                debugger;
                var result = JSON.parse(ab.d);
                $('#tblLocation > tbody').html('');
                var totalValue = 0;
                $(result).each(function (key, item) {
                    debugger;
                    var htmlRow = "<tr id='" + item.loc_id + "' onclick='fnTblLocationOnRowClick(this)'>";
                    htmlRow = htmlRow + "<td style='display: none'>" + item.loc_id + "</td>";
                    htmlRow = htmlRow + "<td>" + item.loc_name + "</td>";
                    htmlRow = htmlRow + "<td style='text-align:right'><span class='badge bg-yellow'>" + item.total_sale + "</span></td>";
                    htmlRow = htmlRow + "</tr>";
                    $('#tblLocation > tbody').append(htmlRow);
                    totalValue += parseFloat(item.total_sale);
                });
                $('#tblLocation > tfoot').html('');
                var htmlRow = "<tr style='border-top:solid'>";
                htmlRow = htmlRow + "<td style='display: none'></td>";
                htmlRow = htmlRow + "<td><strong>Total</strong></td>";
                htmlRow = htmlRow + "<td style='text-align:right'><span class='badge bg-yellow'>" + totalValue.toFixed(3) + "</span></td>";
                htmlRow = htmlRow + "</tr>";
                $('#tblLocation > tfoot').append(htmlRow);
                fnHideProgress();
            },

            error: function (ex) {
                fnHideProgress();
            }
        });


    }
    catch (err) {
        fnHideProgress();
    }
}

function fnTblLocationOnRowClick(row) {
    try {
        debugger;
        var _result = fnValidateCustomFilter();
        if (_result == false) {
            return;
        }
        //var row = $(this);
        var storeNo = $(row).find("td:eq(0)").text();
        gStoreNo = storeNo;
        if (storeNo == undefined || storeNo == "") {
            return;
        }
        $('#tblLocation tbody tr').each(function () {
            $(this).removeClass('active');
        });
        $(row).addClass('active');
        $('#removeStoreFilter').show();
        fnGetSalesCategoryChart(gStoreNo);
        fnGetSalesPersonChart(gStoreNo);
        fnGetBrandSalesChart(gStoreNo);
        fnGetPayMethodChart(gStoreNo);
        //call function to filter chart data
        //fnGetFastMovingItemChart(storeNo);
        //fnGetInventoryCategoryChart(storeNo);
        //fnGetCategoryWiseItemReOrderChart(storeNo);
        //fnGetVendorStockChart(storeNo)
        //fnGetBrandStockChart(storeNo);

    }
    catch (err) {

    }
}

