window.onload = function () {
   
    //fnShowProgress();
    //fnFillLocationTbl();
    //fnGetInventoryCategoryChart(gStoreNo);
    //fnGetFastMovingItemChart(gStoreNo);
    //fnGetCategoryWiseItemReOrderChart(gStoreNo);
    //fnGetVendorStockChart(gStoreNo)
    //fnGetBrandStockChart(gStoreNo);
    //fnHideProgress();
    //fnGetDummyChart('@ViewBag.StoreNo');
}

function fnOnChangeTopNRecord() {
    try {
        fnGetFastMovingItemChart(gStoreNo);
    }
    catch (err) {

    }
    //get current value of select
    var value = $('#lstNoOfItems').dropselect('value');
    //display red information line
    //$("#display").html('<br><p>"' + value + '" selected. This will post a hidden input named "fruit" and a value of "' + value + '" if wrapped by a form element.</p>');
}

$(document).ready(function () {
    //make Bootstrap dropdown to behave like select
    //this is enough for most people! When user selects an option, a hidden input named "fruit" with the selected value is created.
    $('.dropselect').dropselect();

    //set onchange event handler to display red information line
    $('#lstNoOfItems').on('change', fnOnChangeTopNRecord);
    //$('#lstFilterType').on('change', fnOnChangeFilterType);
});

function labelFormatter(label, series, color) {
    return '<div style="font-size:8px; text-align:center; padding:2px; color: ' + color + '; font-weight: 400;">'
      + label
      + '<br>'
      + Math.round(series.percent) + '</div>'
}

function fnOnChangeFilterType()
{
    getFilterValue();
    fnFillLocationTbl();
    fnGetInventoryCategoryChart(gStoreNo);
    fnGetBrandStockChart(gStoreNo);
    fnGetFastMovingItemChart(gStoreNo);
}

function fnTblLocationOnRowClick(row)
{
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
        //call function to filter chart data
        fnGetFastMovingItemChart(storeNo);
        fnGetInventoryCategoryChart(storeNo);
        //fnGetCategoryWiseItemReOrderChart(storeNo);
        //fnGetVendorStockChart(storeNo)
        fnGetBrandStockChart(storeNo);

    }
    catch (err) {

    }
}

function getFilterValue() {
    if ($('#lstFilterType').val() == gCustomFilerEnumVal)
    {
        return;
    }
    try {
        debugger;
        $.ajax({
            url: "Inv_Dashboard.aspx/GetDashboardFilterParamValue",
            type: "POST",
            async:false,
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

function fnRemoveLocFilter()
{
    try {
        debugger;
        var storeNo = "";
        gStoreNo = storeNo;
        $('#tblLocation tbody tr').each(function () {
            $(this).removeClass('active');
        });
        
        $('#removeStoreFilter').hide();
        //call function to filter chart data
        fnGetFastMovingItemChart(storeNo);
        fnGetInventoryCategoryChart(storeNo);
        //fnGetCategoryWiseItemReOrderChart(storeNo);
        //fnGetVendorStockChart(storeNo)
        fnGetBrandStockChart(storeNo);
    }
    catch (err) {

    }
}

function fnGetFastMovingItemChart(store_no) {
    try {
        debugger;
        var fromDate = gFromDate;
        var toDate = gToDate;
        var xValues;
        var Ids;
        //var noOfItems = $('#lstNoOfItems').val();
        var noOfItems ;
        if ($('#lstNoOfItems').dropselect('value')==undefined)
        {
            noOfItems = '10';
        }
        else
        {
            noOfItems = $('#lstNoOfItems').dropselect('value');
        }
         
        
        var sTitle = "Top " + noOfItems + " Fast Moving Item By Qty";
        fnShowProgress();
        $.ajax({
            url: "Inv_Dashboard.aspx/GetFastMovingItemChartData",
            type: "POST",
            contentType: "application/json;charset=utf-8",
            data: "{store_no:'" + store_no + "',from_date:'" + fromDate + "',to_date:'" + toDate + "',noOfTopItems:'" + noOfItems + "'}",
            dataType: "Json",
            success: function (ab) {
                debugger;
                var result = JSON.parse(ab.d);
                var aLables = result[0];
                xValues = result[0];
                var aDataset1 = result[1];
                Ids = result[2];
                var aData = {
                    labels: aLables,
                    datasets: [{
                        label: "Qty",
                        data: aDataset1,
                        strokeColor: "black",
                        fillColor: "rgba(0,60,100,1)"
                        
                    }]
                };
                try {
                    //remove previos graph
                    $('#fastMovingItemChart').remove();
                    $('#fastMovingItemChartContainer').append("<canvas id='fastMovingItemChart' class='chart' style='height:250px'></canvas>");
                }
                catch (err) {

                }
                var ctx = $('#fastMovingItemChart').get(0).getContext("2d");
                var myNewChart = new Chart(ctx, {
                    type: 'bar',
                    data: aData,
                    options: {
                        responsive: true,
                        title: { display: true, text: sTitle },
                        legend: { position: 'bottom' },
                        scales: {
                            xAxes: [{ gridLines: { display: true }, display: false, scaleLabel: { display: true, labelString: '' } }],
                            yAxes: [{ gridLines: { display: true }, display: true, scaleLabel: { display: true, labelString: '' } }]
                        },
                    }
                });
                document.getElementById("fastMovingItemChart").onclick = function (evt) {
                    try {
                        debugger;
                        var activePoints = myNewChart.getElementsAtEvent(evt);
                        var firstPoint = activePoints[0];

                        var _id = Ids[firstPoint._index];
                        var title = xValues[firstPoint._index];
                        if (_id != undefined) {
                            $.ajax({
                                url: "Inv_Dashboard.aspx/GetProductSalesDetailByProductId",
                                type: "POST",
                                contentType: "application/json;charset=utf-8",
                                data: "{store_no:'" + store_no + "',from_date:'" + fromDate + "',to_date:'" + toDate + "',product_id:'" + _id + "'}",
                                success: function (ab) {
                                    debugger;
                                    var result = JSON.parse(ab.d);
                                    $('#divTblProductSummary').hide()
                                    $('#divTblProductFastMove').show()
                                    $('#tblProductFastMove tbody').empty();
                                    tblProductFastMove.destroy();
                                    $(result).each(function (key, item) {
                                        var htmlRow = "<tr>";
                                        htmlRow = htmlRow + "<td>" + item.trans_type + "</td>";
                                        htmlRow = htmlRow + "<td>" + item.trans_no + "</td>";
                                        htmlRow = htmlRow + "<td>" + item.trans_date + "</td>";
                                        htmlRow = htmlRow + "<td>" + item.unit_name + "</td>";
                                        htmlRow = htmlRow + "<td style='text-align:right'>" + item.qty + "</td>";
                                        htmlRow = htmlRow + "</tr>";
                                        $('#tblProductFastMove > tbody').append(htmlRow);
                                    });
                                    reDrawProductFastMoveTable();
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

function fnGetCategoryWiseItemReOrderChart(store_no) {
    try {
        var fromDate = gFromDate;
        var toDate = gToDate;
        var xValues;
        var Ids;
        fnShowProgress();
        $.ajax({
            url: "GetCatWiseReOrderItemChartData",
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
                        label: "Reorder Categories",
                        data: aDataset1,
                        fill: true,
                        backgroundColor: ["rgba(167, 105, 0, 0.4)"],
                        borderColor: ["rgb(54, 162, 235)", "rgb(255, 99, 132)", "rgb(255, 159, 64)", "rgb(255, 205, 86)", "rgb(75, 192, 192)", "rgb(153, 102, 255)", "rgb(201, 203, 207)"],
                        borderWidth: 1
                    }]
                };
                try {
                    //remove previos graph
                    $('#categoryWiseItemReOrderChart').remove();
                    $('#categoryWiseItemReOrderChartContainer').append("<canvas id='categoryWiseItemReOrderChart' class='chart' style='height:250px'></canvas>");
                }
                catch (err) {

                }
                var ctx = $('#categoryWiseItemReOrderChart').get(0).getContext("2d");
                var myNewChart = new Chart(ctx, {
                    type: 'line',
                    data: aData,
                    options: {
                        responsive: true,
                        title: { display: true, text: 'CATEGORY WISE ITEM REORDER OVERVIEW' },
                        legend: { position: 'bottom' },
                        scales: {
                            xAxes: [{ gridLines: { display: false }, display: true, scaleLabel: { display: true, labelString: '' } }]
                        },
                    }
                });
                document.getElementById("categoryWiseItemReOrderChart").onclick = function (evt) {
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
                                url: "GetReOrderItemByCategory",
                                type: "POST",
                                contentType: "application/json;charset=utf-8",
                                data: "{store_no:'" + store_no + "',from_date:'" + fromDate + "',to_date:'" + toDate + "',category_id:'" + _id + "'}",
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

function fnGetBrandStockChart(store_no) {
    try {
        var fromDate = gFromDate;
        var toDate = gToDate;
        var xValues;
        var Ids;
        fnShowProgress();
        $.ajax({
            url: "Inv_Dashboard.aspx/GetInventoryBrandChartData",
            type: "POST",
            contentType: "application/json;charset=utf-8",
            data: "{store_no:'" + store_no + "',from_date:'" + fromDate + "',to_date:'" + toDate + "'}",
            dataType: "Json",
            success: function (ab) {
                var result = JSON.parse(ab.d);
                var aLables = result[0];
                xValues = result[0];
                var aDataset1 = result[1];
                Ids = result[2];
                var aData = {
                    labels: aLables,
                    datasets: [{
                        label: "Stock Value",
                        data: aDataset1,
                        fill: true,
                        backgroundColor: ["cyan"],
                        borderColor: ["rgb(54, 162, 235)", "rgb(255, 99, 132)", "rgb(255, 159, 64)", "rgb(255, 205, 86)", "rgb(75, 192, 192)", "rgb(153, 102, 255)", "rgb(201, 203, 207)"],
                        borderWidth: 1
                    }]
                };
                try {
                    //remove previos graph
                    $('#brandStockChart').remove();
                    $('#brandStockChartContainer').append("<canvas id='brandStockChart' class='chart' style='height:250px'></canvas>");
                }
                catch (err) {

                }
                var ctx = $('#brandStockChart').get(0).getContext("2d");
                var myNewChart = new Chart(ctx, {
                    type: 'line',
                    data: aData,
                    options: {
                        responsive: true,
                        title: { display: true, text: 'BRANDS OVERVIEW' },
                        legend: { position: 'bottom' },
                        scales: {
                            xAxes: [{ gridLines: { display: false }, display: false, scaleLabel: { display: true, labelString: '' } }]
                        },
                    }
                });
                document.getElementById("brandStockChart").onclick = function (evt) {
                    try {
                        debugger;
                        var activePoints = myNewChart.getElementsAtEvent(evt);
                        var firstPoint = activePoints[0];
                        var _id = Ids[firstPoint._index];
                        var title = xValues[firstPoint._index];
                        if (_id != undefined) {
                            $.ajax({
                                url: "Inv_Dashboard.aspx/GetProductStockByBrand",
                                type: "POST",
                                contentType: "application/json;charset=utf-8",
                                data: "{store_no:'" + store_no + "',from_date:'" + fromDate + "',to_date:'" + toDate + "',brand_id:'" + _id + "'}",
                                success: function (ab) {
                                    debugger;
                                    var result = JSON.parse(ab.d);
                                    $('#divTblProductSummary').show()
                                    $('#divTblProductFastMove').hide()
                                    $('#tblProductSummary tbody').empty();
                                    tblProduct.destroy();
                                    $(result).each(function (key, item) {
                                        var htmlRow = "<tr id='" + item.product_id + "'>";
                                        htmlRow = htmlRow + "<td>" + item.product_id + "</td>";
                                        htmlRow = htmlRow + "<td>" + item.product_name + "</td>";
                                        htmlRow = htmlRow + "<td>" + item.unit_name + "</td>";
                                        htmlRow = htmlRow + "<td style='text-align:right'>" + item.stock_qty + "</td>";
                                        htmlRow = htmlRow + "<td style='text-align:right'>" + item.cost_price + "</td>";
                                        htmlRow = htmlRow + "<td style='text-align:right'>" + item.stock_value + "</td>";
                                        htmlRow = htmlRow + "</tr>";
                                        $('#tblProductSummary > tbody').append(htmlRow);
                                    });
                                    reDrawProductTable();
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

function fnGetInventoryCategoryChart(store_no) {
    try {

        var fromDate = gFromDate;
        var toDate = gToDate;
        var catIds;
        var catNames;
        fnShowProgress();
        $.ajax({
            url: "Inv_Dashboard.aspx/GetInventoryCategoryChartData",
            type: "POST",
            data: "{store_no:'" + store_no + "',from_date:'" + fromDate + "',to_date:'" + toDate + "'}",
            contentType: "application/json;charset=utf-8",
            dataType: "Json",
            success: function (ab) {
                debugger;
                var result = JSON.parse(ab.d);
                var aLables = result[0];
                catNames = result[0];
                var aDataset1 = result[1];
                catIds = result[2];
                var aData = {
                    labels: aLables,
                    datasets: [{
                        label: "Stock",
                        data: aDataset1,
                        fill: false,
                        backgroundColor: ["green"],
                        borderColor: ["green"],
                        borderWidth: 1
                    }]
                };
                try {
                    //remove previos graph salesCategoryChart
                    $('#inventoryCategoryChart').remove();
                    $('#inventoryCategoryChartContainer').append("<canvas id='inventoryCategoryChart' class='chart' style='height:250px'></canvas>");
                }
                catch (err) {

                }
                var ctx = $('#inventoryCategoryChart').get(0).getContext("2d");
                var myNewChart = new Chart(ctx, {
                    type: 'line',
                    data: aData,
                    options: {
                        responsive: true,
                        title: { display: true, text: 'CATEGORY WISE STOCK' },
                        legend: { position: 'bottom' },
                        scales: {
                            xAxes: [{ gridLines: { display: true }, display: false, scaleLabel: { display: true, labelString: '' } }],
                            yAxes: [{ gridLines: { display: true }, display: true, scaleLabel: { display: true, labelString: '' } }]
                        },
                    }
                });
                document.getElementById("inventoryCategoryChart").onclick = function (evt) {
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
                                url: "Inv_Dashboard.aspx/GetProductStockByCategory",
                                type: "POST",
                                async:false,
                                contentType: "application/json;charset=utf-8",
                                data: "{store_no:'" + store_no + "',from_date:'" + fromDate + "',to_date:'" + toDate + "',category_id:'" + catId + "'}",
                                success: function (ab) {
                                    debugger;
                                    var result = JSON.parse(ab.d);
                                    //$('#tblProductSummary').dataTable().destroy();
                                    $('#divTblProductSummary').show()
                                    $('#divTblProductFastMove').hide()
                                    $('#tblProductSummary tbody').empty();
                                    tblProduct.destroy();
                                    
                                    $(result).each(function (key, item) {
                                        var htmlRow = "<tr id='" + item.product_id + "'>";
                                        htmlRow = htmlRow + "<td>" + item.product_id + "</td>";
                                        htmlRow = htmlRow + "<td>" + item.product_name + "</td>";
                                        htmlRow = htmlRow + "<td>" + item.unit_name + "</td>";
                                        htmlRow = htmlRow + "<td style='text-align:right'>" + item.stock_qty + "</td>";
                                        htmlRow = htmlRow + "<td style='text-align:right'>" + item.cost_price + "</td>";
                                        htmlRow = htmlRow + "<td style='text-align:right'>" + item.stock_value + "</td>";
                                        htmlRow = htmlRow + "</tr>";
                                        $('#tblProductSummary > tbody').append(htmlRow);
                                    });
                                    //tblProduct.draw();
                                    reDrawProductTable();


                                    
                                    //remove existing data
                                    //$('#modalBody').html('');
                                    //add new data
                                    //$('#modalBody').html(result);
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

function fnLstNoOfItemOnChange() {
    try {
        fnGetFastMovingItemChart(gStoreNo);
    }
    catch (err) {

    }
}

function fnFillLocationTbl() {
    try {
        debugger;
        var fromDate = gFromDate;
        var toDate = gToDate;
        fnShowProgress();
        $.ajax({
            url: "Inv_Dashboard.aspx/GetLocationWiseStock",
            type: "POST",
            data: "{from_date:'" + fromDate + "',to_date:'" + toDate + "'}",
            contentType: "application/json;charset=utf-8",
            dataType: "Json",
            success: function (ab) {
                debugger;
                var result = JSON.parse(ab.d);
                $('#tblLocation > tbody').html('');
                $(result).each(function (key, item) {
                    debugger;
                    var htmlRow = "<tr id='" + item.loc_id + "' onclick='fnTblLocationOnRowClick(this)'>";
                    htmlRow = htmlRow + "<td style='display: none'>" + item.loc_id + "</td>";
                    htmlRow = htmlRow + "<td>" + item.loc_name + "</td>";
                    htmlRow = htmlRow + "<td style='text-align:right'>" + item.stock_value + "</td>";
                    htmlRow = htmlRow + "</tr>";
                    $('#tblLocation > tbody').append(htmlRow);
                });
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

