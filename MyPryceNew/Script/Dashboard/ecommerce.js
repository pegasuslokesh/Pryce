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



$(document).ready(function () {
    // fnGetInventoryCategoryChart();
    getFilterValue();
});

function fnOnChangeFilterType() {
    getFilterValue();

    fnGetECommerceReturn(gStoreNo);
    fnGetECommerceOrderProduct(gStoreNo);
    fnGetECommerceOrder(gStoreNo);
    fnGetECommerceOrderReturnProduct(gStoreNo);
}
function getFilterValue() {

    if ($('#lstFilterType').val() == gCustomFilerEnumVal) {
        return;
    }
    
    try {
        debugger;
        $.ajax({
            //url: "Inv_Dashboard.aspx/GetDashboardFilterParamValue",
            url: "http://localhost:55202/Inventory/Inv_Dashboard.aspx/GetDashboardFilterParamValue",
            type: "POST",
            async: false,
            data: "{from_date:'" + gFromDate + "',to_date:'" + gToDate + "',store_no:'8',filter_type:'" + $('#lstFilterType').val() + "'}",
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

        //gFromDate = "01-May-2021";
        //gToDate = "31-May-2021";
        //$("#FromDate").val(gFromDate);
        //$("#ToDate").val(gToDate);
        //fnHideProgress();
    }
    catch (err) {
      

    }
}

function fnGetECommerceOrder() {
    try {

        var fromDate = gFromDate;
        var toDate = gToDate;
        var catIds;
        var catNames;
        fnShowProgress();
        $.ajax({
            url: "Ecommerce.aspx/GetECommerceOrder",
            type: "POST",
            data: "{from_date:'" + gFromDate + "',to_date:'" + gToDate + "'}",
            contentType: "application/json;charset=utf-8",
            dataType: "Json",
            success: function (ab) {
                debugger;
                var result = JSON.parse(ab.d);

                $('#tblmerchant tbody').empty();


                var lengthA = result[0].length;
                console.log(lengthA);
                for (var k = 0; k < lengthA; k++)
                {
                    var htmlRow = "<tr id='" + result[0][k] + "'>";
                    htmlRow = htmlRow + "<td>" + result[0][k] + "</td>";
                    htmlRow = htmlRow + "<td>" + result[1][k] + "</td>";

                    htmlRow = htmlRow + "</tr>";
                    $('#tblmerchant > tbody').append(htmlRow);
                    console.log(htmlRow);
                }



            
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
                        title: { display: true, text: 'Total Ordes' },
                        legend: { position: 'bottom' },
                        scales: {
                            xAxes: [{ gridLines: { display: true }, display: false, scaleLabel: { display: true, labelString: '' } }],
                            yAxes: [{ gridLines: { display: true }, display: true, scaleLabel: { display: true, labelString: '' } }]
                        },
                    }
                });
                //document.getElementById("inventoryCategoryChart").onclick = function (evt) {
                //    try {
                //        var activePoints = myNewChart.getElementsAtEvent(evt);
                //        var firstPoint = activePoints[0];
                //        //var label = scatterChart.data.labels[firstPoint._index];
                //        //var label = myNewChart.data.datasets[firstPoint._index].label;
                //        //var value = myNewChart.data.datasets[firstPoint._datasetIndex].data[firstPoint._index];
                //        debugger;
                //        var catId = catIds[firstPoint._index];
                //        var title = catNames[firstPoint._index];
                //        if (catId != undefined) {
                //            fnShowProgress();
                //            $.ajax({
                //                url: "Inv_Dashboard.aspx/GetProductStockByCategory",
                //                type: "POST",
                //                async: false,
                //                contentType: "application/json;charset=utf-8",
                //                data: "{store_no:'" + store_no + "',from_date:'" + fromDate + "',to_date:'" + toDate + "',category_id:'" + catId + "'}",
                //                success: function (ab) {
                //                    debugger;
                //                    var result = JSON.parse(ab.d);
                //                    //$('#tblProductSummary').dataTable().destroy();
                //                    $('#divTblProductSummary').show()
                //                    $('#divTblProductFastMove').hide()
                //                    $('#tblProductSummary tbody').empty();
                //                    tblProduct.clear();
                //                    tblProduct.destroy();

                //                    $(result).each(function (key, item) {
                //                        var htmlRow = "<tr id='" + item.product_id + "'>";
                //                        htmlRow = htmlRow + "<td>" + item.product_id + "</td>";
                //                        htmlRow = htmlRow + "<td>" + item.product_name + "</td>";
                //                        htmlRow = htmlRow + "<td>" + item.unit_name + "</td>";
                //                        htmlRow = htmlRow + "<td style='text-align:right'>" + item.stock_qty + "</td>";
                //                        htmlRow = htmlRow + "<td style='text-align:right'>" + item.cost_price + "</td>";
                //                        htmlRow = htmlRow + "<td style='text-align:right'>" + item.stock_value + "</td>";
                //                        htmlRow = htmlRow + "</tr>";
                //                        $('#tblProductSummary > tbody').append(htmlRow);
                //                    });
                //                    //tblProduct.draw();
                //                    reDrawProductTable();



                //                    //remove existing data
                //                    //$('#modalBody').html('');
                //                    //add new data
                //                    //$('#modalBody').html(result);
                //                    $('#modalTitle').html('');
                //                    $('#modalTitle').html(title);
                //                    $('#modalDetail').show();
                //                    fnHideProgress();
                //                },
                //                error: function (err) {
                //                    debugger;
                //                    alert(err.message);
                //                    fnHideProgress();
                //                }
                //            });
                //        }
                //        if (firstPoint !== undefined) {
                //            //alert(label + ": " + value.x);
                //        }
                //    }
                //    catch (err) {

                //    }

                //};
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



function fnGetECommerceReturn() {
    try {

        var fromDate = gFromDate;
        var toDate = gToDate;
        var catIds;
        var catNames;
        fnShowProgress();
        $.ajax({
            url: "Ecommerce.aspx/GetECommerceReturn",
            type: "POST",
            data: "{from_date:'" + gFromDate + "',to_date:'" + gToDate + "'}",
            contentType: "application/json;charset=utf-8",
            dataType: "Json",
            success: function (ab) {
                debugger;
                var result = JSON.parse(ab.d);

                $('#tblmerchant1 tbody').empty();


                var lengthA = result[0].length;
                console.log(lengthA);
                for (var k = 0; k < lengthA; k++) {
                    var htmlRow = "<tr id='" + result[0][k] + "'>";
                    htmlRow = htmlRow + "<td>" + result[0][k] + "</td>";
                    htmlRow = htmlRow + "<td>" + result[1][k] + "</td>";

                    htmlRow = htmlRow + "</tr>";
                    $('#tblmerchant1 > tbody').append(htmlRow);
                    console.log(htmlRow);
                }




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
                    $('#inventoryCategoryChart1').remove();
                    $('#inventoryCategoryChartContainer1').append("<canvas id='inventoryCategoryChart1' class='chart' style='height:250px'></canvas>");
                }
                catch (err) {

                }
                var ctx = $('#inventoryCategoryChart1').get(0).getContext("2d");
                var myNewChart = new Chart(ctx, {
                    type: 'line',
                    data: aData,
                    options: {
                        responsive: true,
                        title: { display: true, text: 'Total Return' },
                        legend: { position: 'bottom' },
                        scales: {
                            xAxes: [{ gridLines: { display: true }, display: false, scaleLabel: { display: true, labelString: '' } }],
                            yAxes: [{ gridLines: { display: true }, display: true, scaleLabel: { display: true, labelString: '' } }]
                        },
                    }
                });
                //document.getElementById("inventoryCategoryChart").onclick = function (evt) {
                //    try {
                //        var activePoints = myNewChart.getElementsAtEvent(evt);
                //        var firstPoint = activePoints[0];
                //        //var label = scatterChart.data.labels[firstPoint._index];
                //        //var label = myNewChart.data.datasets[firstPoint._index].label;
                //        //var value = myNewChart.data.datasets[firstPoint._datasetIndex].data[firstPoint._index];
                //        debugger;
                //        var catId = catIds[firstPoint._index];
                //        var title = catNames[firstPoint._index];
                //        if (catId != undefined) {
                //            fnShowProgress();
                //            $.ajax({
                //                url: "Inv_Dashboard.aspx/GetProductStockByCategory",
                //                type: "POST",
                //                async: false,
                //                contentType: "application/json;charset=utf-8",
                //                data: "{store_no:'" + store_no + "',from_date:'" + fromDate + "',to_date:'" + toDate + "',category_id:'" + catId + "'}",
                //                success: function (ab) {
                //                    debugger;
                //                    var result = JSON.parse(ab.d);
                //                    //$('#tblProductSummary').dataTable().destroy();
                //                    $('#divTblProductSummary').show()
                //                    $('#divTblProductFastMove').hide()
                //                    $('#tblProductSummary tbody').empty();
                //                    tblProduct.clear();
                //                    tblProduct.destroy();

                //                    $(result).each(function (key, item) {
                //                        var htmlRow = "<tr id='" + item.product_id + "'>";
                //                        htmlRow = htmlRow + "<td>" + item.product_id + "</td>";
                //                        htmlRow = htmlRow + "<td>" + item.product_name + "</td>";
                //                        htmlRow = htmlRow + "<td>" + item.unit_name + "</td>";
                //                        htmlRow = htmlRow + "<td style='text-align:right'>" + item.stock_qty + "</td>";
                //                        htmlRow = htmlRow + "<td style='text-align:right'>" + item.cost_price + "</td>";
                //                        htmlRow = htmlRow + "<td style='text-align:right'>" + item.stock_value + "</td>";
                //                        htmlRow = htmlRow + "</tr>";
                //                        $('#tblProductSummary > tbody').append(htmlRow);
                //                    });
                //                    //tblProduct.draw();
                //                    reDrawProductTable();



                //                    //remove existing data
                //                    //$('#modalBody').html('');
                //                    //add new data
                //                    //$('#modalBody').html(result);
                //                    $('#modalTitle').html('');
                //                    $('#modalTitle').html(title);
                //                    $('#modalDetail').show();
                //                    fnHideProgress();
                //                },
                //                error: function (err) {
                //                    debugger;
                //                    alert(err.message);
                //                    fnHideProgress();
                //                }
                //            });
                //        }
                //        if (firstPoint !== undefined) {
                //            //alert(label + ": " + value.x);
                //        }
                //    }
                //    catch (err) {

                //    }

                //};
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

function fnGetECommerceOrderProduct() {
    try {

        var fromDate = gFromDate;
        var toDate = gToDate;
        var catIds;
        var catNames;
        fnShowProgress();
        $.ajax({
            url: "Ecommerce.aspx/GetECommerceOrderProduct",
            type: "POST",
            data: "{from_date:'" + gFromDate + "',to_date:'" + gToDate + "'}",
            contentType: "application/json;charset=utf-8",
            dataType: "Json",
            success: function (ab) {
                debugger;
                var result = JSON.parse(ab.d);

                $('#tblorderproduct tbody').empty();


                var lengthA = result[0].length;
                console.log(lengthA);
                for (var k = 0; k < lengthA; k++) {
                    var htmlRow = "<tr>";
                    htmlRow = htmlRow + "<td>" + result[0][k] + "</td>";
                    htmlRow = htmlRow + "<td>" + result[2][k] + "</td>";
                    htmlRow = htmlRow + "<td>" + result[1][k] + "</td>";
                    htmlRow = htmlRow + "</tr>";
                    $('#tblorderproduct > tbody').append(htmlRow);
                    console.log(htmlRow);
                }




            
              
              
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

function fnGetECommerceOrderReturnProduct() {
    try {

        var fromDate = gFromDate;
        var toDate = gToDate;
        var catIds;
        var catNames;
        fnShowProgress();
        $.ajax({
            url: "Ecommerce.aspx/GetECommerceOrderReturnProduct",
            type: "POST",
            data: "{from_date:'" + gFromDate + "',to_date:'" + gToDate + "'}",
            contentType: "application/json;charset=utf-8",
            dataType: "Json",
            success: function (ab) {
                debugger;
                var result = JSON.parse(ab.d);

                $('#tblorderreturnproduct tbody').empty();


                var lengthA = result[0].length;
                console.log(lengthA);
                for (var k = 0; k < lengthA; k++) {
                    var htmlRow = "<tr>";
                    htmlRow = htmlRow + "<td>" + result[0][k] + "</td>";
                    htmlRow = htmlRow + "<td>" + result[2][k] + "</td>";
                    htmlRow = htmlRow + "<td>" + result[1][k] + "</td>";

                    htmlRow = htmlRow + "</tr>";
                    $('#tblorderreturnproduct > tbody').append(htmlRow);
                    console.log(htmlRow);
                }







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


