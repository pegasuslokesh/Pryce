window.onload = function () {
   
    //fnShowProgress();
    ////fnGetFastMovingItemChart(gStoreNo);
    //fnGetIncomeExpensesChart(gStoreNo);
    //fnGetSundryDrCrChart(gStoreNo);
    //fnGetCashBankBalance(gStoreNo);
    //fnHideProgress();
    ////fnGetDummyChart('@ViewBag.StoreNo');
}

function fnOnChangeFilterType() {
    getFilterValue();
    fnFillLocationTbl();
    debugger;
    fnGetIncomeExpensesChart(gStoreNo);
    fnGetSundryDrCrChart(gStoreNo);
    fnGetCashBankBalance(gStoreNo);
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
        fnGetIncomeExpensesChart(gStoreNo);
        fnGetSundryDrCrChart(gStoreNo);
        fnGetCashBankBalance(gStoreNo);
        
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
            url: "Ac_Dashboard.aspx/GetDashboardFilterParamValue",
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

function fnFillLocationTbl() {
    try {
        debugger;
        var fromDate = gFromDate;
        var toDate = gToDate;
        fnShowProgress();
        $.ajax({
            url: "Ac_Dashboard.aspx/GetLocationWiseVoucherCount",
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
                    htmlRow = htmlRow + "<td style='text-align:right'><span class='badge bg-yellow'>" + item.voucher_count + "</span></td>";
                    htmlRow = htmlRow + "</tr>";
                    $('#tblLocation > tbody').append(htmlRow);
                    totalValue += parseFloat(item.voucher_count);
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
        fnGetIncomeExpensesChart(gStoreNo);
        fnGetSundryDrCrChart(gStoreNo);
        fnGetCashBankBalance(gStoreNo);
        

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
//        gStoreNo = storeNo;
//        if (storeNo == undefined || storeNo == "") {
//            return;
//        }
//        $('#tblStoreWiseSales tbody tr').each(function () {
//            $(this).removeClass('active');
//        });
//        row.addClass('active');
//        $('#removeStoreFilter').show();
//        //call function to filter chart data
//        //fnGetFastMovingItemChart(storeNo);
//        fnGetIncomeExpensesChart(gStoreNo);
//        fnGetSundryDrCrChart(gStoreNo);
//        fnGetCashBankBalance(gStoreNo);

//    }
//    catch (err) {

//    }
//});

function fnGetIncomeExpensesChart(store_no) {
    try {
        debugger;
        var fromDate = gFromDate;
        var toDate = gToDate;
        var catIds;
        var catNames;
        var fromDates;
        var toDates;
        fnShowProgress();
        $.ajax({
            url: "Ac_Dashboard.aspx/GetIncomeExpensesChartData",
            type: "POST",
            data: "{store_no:'" + store_no + "',from_date:'" + fromDate + "',to_date:'" + toDate + "'}",
            contentType: "application/json;charset=utf-8",
            dataType: "Json",
            success: function (result) {
                debugger;
                result = JSON.parse(result.d);
                var aLables = result[0];
                catNames = result[0];
                var aDataset1 = result[1];
                var aDataset2 = result[2];
                catIds = result[2];
                fromDates = result[3];
                toDates = result[4];
                var aData = {
                    labels: aLables,
                    datasets: [{
                        label: "Income",
                        data: aDataset1,
                        backgroundColor: ["green", "green", "green", "green", "green", "green", "green", "green", "green", "green", "green", "green", "green", "green", "green", "green", "green", "green", "green"]
                    }, {
                        label: "Expenses",
                        data: aDataset2,
                        backgroundColor: ["red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red", "red"]
                    }]
                };
                try {
                    //remove previos graph salesCategoryChart
                    $('#incomeExpensesChart').remove();
                    $('#incomeExpensesChartContainer').append("<canvas id='incomeExpensesChart' class='chart' style='height:250px'></canvas>");
                }
                catch (err) {

                }
                var ctx = $('#incomeExpensesChart').get(0).getContext("2d");
                var myNewChart = new Chart(ctx, {
                    type: 'bar',
                    data: aData,
                    options: {
                        onClick: function (e) {
                            try{
                                var firstPoint = this.getElementAtEvent(e)[0];
                                var index = firstPoint._index;
                                var datasetIndex = firstPoint._datasetIndex;
                                var rec_type = firstPoint._model.datasetLabel;
                                var catId = catIds[firstPoint._index];
                                var title = rec_type + " " + catNames[firstPoint._index];
                                var fDate = fromDates[firstPoint._index];
                                var tDate = toDates[firstPoint._index];
                                if (catId != undefined) {
                                    fnShowProgress();
                                    $.ajax({
                                        url: "Ac_Dashboard.aspx/GetIncomeExpensesAccountSummary",
                                        type: "POST",
                                        contentType: "application/json;charset=utf-8",
                                        data: "{store_no:'" + store_no + "',from_date:'" + fDate + "',to_date:'" + tDate + "',rec_type:'" + rec_type + "'}",
                                        success: function (result) {
                                            debugger;
                                            var result = JSON.parse(result.d);
                                            //$('#tblProductSummary').dataTable().destroy();
                                            $('#divTblProductSummary').show()
                                            $('#divTblSalesHeader').hide()
                                            $('#tblProductSummary tbody').empty();
                                            tblProduct.clear();
                                            tblProduct.destroy();

                                            $(result).each(function (key, item) {
                                                var htmlRow = "<tr id='" + item.account_no + "'>";
                                                htmlRow = htmlRow + "<td>" + item.account_no + "</td>";
                                                htmlRow = htmlRow + "<td>" + item.account_name + "</td>";
                                                htmlRow = htmlRow + "<td style='text-align:right'>" + item.cb + "</td>";
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
                            catch(err)
                            {

                            }
                        },
                        responsive: true,
                        title: { display: true, text: 'Income Expenes Overview' },
                        legend: { position: 'bottom' }

                    }
                });
                
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

function fnGetSundryDrCrChart(store_no) {
    try {
        debugger;
        var fromDate = gFromDate;
        var toDate = gToDate;
        var catIds;
        var catNames;
        var fromDates;
        var toDates;
        fnShowProgress();
        $.ajax({
            url: "Ac_Dashboard.aspx/GetSundryDrCrChartData",
            type: "POST",
            data: "{store_no:'" + store_no + "',from_date:'" + fromDate + "',to_date:'" + toDate + "'}",
            contentType: "application/json;charset=utf-8",
            dataType: "Json",
            success: function (result) {
                debugger;
                result = JSON.parse(result.d);
                var aLables = result[0];
                catNames = result[0];
                var aDataset1 = result[1];
                var aDataset2 = result[2];
                catIds = result[2];
                fromDates = result[3];
                toDates = result[4];
                var aData = {
                    labels: aLables,
                    datasets: [{
                        label: "Creditors",
                        data: aDataset1,
                        fill: false,
                        backgroundColor: ["green"],
                        borderColor: ["green"],
                        borderWidth: 1
                    }, {
                        label: "Debitors",
                        data: aDataset2,
                        fill: false,
                        backgroundColor: ["red"],
                        borderColor: ["red"],
                        borderWidth: 1
                    }]
                };
                try {
                    //remove previos graph salesCategoryChart
                    $('#sundryDrCrChart').remove();
                    $('#sundryDrCrChartContainer').append("<canvas id='sundryDrCrChart' class='chart' style='height:250px'></canvas>");
                }
                catch (err) {

                }
                var ctx = $('#sundryDrCrChart').get(0).getContext("2d");
                var myNewChart = new Chart(ctx, {
                    type: 'line',
                    data: aData,
                    options: {
                        onClick: function (e) {
                            try {
                                debugger;
                                var firstPoint = this.getElementAtEvent(e)[0];
                                var index = firstPoint._index;
                                var datasetIndex = firstPoint._datasetIndex;
                                var rec_type;
                                if (datasetIndex == 0) {
                                    rec_type = "Creditors"
                                }
                                else {
                                    rec_type = "Debitors"
                                }
                                var catId = catIds[firstPoint._index];
                                var title = rec_type + " " + catNames[firstPoint._index];
                                var fDate = fromDates[firstPoint._index];
                                var tDate = toDates[firstPoint._index];
                                if (catId != undefined) {
                                    fnShowProgress();
                                    $.ajax({
                                        url: "Ac_Dashboard.aspx/GetSundryDrCrAccountSummary",
                                        type: "POST",
                                        contentType: "application/json;charset=utf-8",
                                        data: "{store_no:'" + store_no + "',from_date:'" + fDate + "',to_date:'" + tDate + "',rec_type:'" + rec_type + "'}",
                                        success: function (result) {
                                            debugger;
                                            var result = JSON.parse(result.d);
                                            //$('#tblProductSummary').dataTable().destroy();
                                            $('#divTblProductSummary').show()
                                            $('#divTblSalesHeader').hide()
                                            $('#tblProductSummary tbody').empty();
                                            tblProduct.clear();
                                            tblProduct.destroy();

                                            $(result).each(function (key, item) {
                                                var htmlRow = "<tr id='" + item.account_no + "'>";
                                                htmlRow = htmlRow + "<td>" + item.account_no + "</td>";
                                                htmlRow = htmlRow + "<td>" + item.account_name + "</td>";
                                                htmlRow = htmlRow + "<td style='text-align:right'>" + item.cb + "</td>";
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
                        },
                        responsive: true,
                        title: { display: true, text: 'Sundry Debitors & Creditros Overview' },
                        legend: { position: 'bottom' },
                        scales: {
                            xAxes: [{ gridLines: { display: true }, display: true, scaleLabel: { display: true, labelString: '' } }],
                            yAxes: [{ gridLines: { display: true }, display: true, scaleLabel: { display: true, labelString: '' } }]
                        },
                    }
                });
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

function fnGetCashBankBalance(store_no) {
    try {
        debugger;
        var fromDate = gFromDate;
        var toDate = gToDate;
        var catIds;
        var catNames;
        var fromDates;
        var toDates;
        fnShowProgress();
        $.ajax({
            url: "Ac_Dashboard.aspx/GetCashBankBlanceData",
            type: "POST",
            data: "{store_no:'" + store_no + "',from_date:'" + fromDate + "',to_date:'" + toDate + "'}",
            contentType: "application/json;charset=utf-8",
            dataType: "Json",
            success: function (result) {
                debugger;
                result = JSON.parse(result.d);
                var cash = result.cash;
                var bank = result.bank;
                cashIncrease = result.cashIncrease;
                bankIncrease = result.bankIncrease;
                var cashDesc;
                var bankDesc;
                $('#cashBalance').html(cash);
                $('#bankBalance').html(bank);
                cashDesc = cashIncrease + "% " + result.cashEffect + " in " + $('#lstFilterType option:selected').text();
                bankDesc = bankIncrease + "% " + result.bankEffect + " in " + $('#lstFilterType option:selected').text();
                $('#cashProgressDescription').html(cashDesc);
                $('#bankProgressDescription').html(bankDesc);
                $('#cashProgress').removeAttr('style');
                $('#bankProgress').removeAttr('style');
                $('#cashProgress').attr("style","width:" + cashIncrease + "%");
                $('#bankProgress').attr("style", "width:" + bankIncrease + "%");
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
function fnGetAccountDetail(acc_type)
{
    try {
        debugger;
        var fromDate = gFromDate;
        var toDate = gToDate;
        var store_no = gStoreNo;
        var title = acc_type.toUpperCase() + " Account Statement " + "<small> from " + fromDate + " to " + toDate + " <small>";
        fnShowProgress();
        $.ajax({
            url: "Ac_Dashboard.aspx/GetCashBankAccStatement",
            type: "POST",
            contentType: "application/json;charset=utf-8",
            data: "{store_no:'" + store_no + "',from_date:'" + fromDate + "',to_date:'" + toDate + "',acc_type:'" + acc_type + "'}",
            success: function (result) {
                debugger;
                var result = JSON.parse(result.d);
                $('#divTblProductSummary').hide()
                $('#divTblSalesHeader').show()
                $('#tblSalesHeader tbody').empty();
                tblSalesHeader.clear();
                tblSalesHeader.destroy();
                $(result).each(function (key, item) {
                    var htmlRow = "<tr>";
                    htmlRow = htmlRow + "<td>" + item.voucher_no + "</td>";
                    htmlRow = htmlRow + "<td>" + item.voucher_date + "</td>";
                    htmlRow = htmlRow + "<td style='text-align:right'>" + item.dr_amt + "</td>";
                    htmlRow = htmlRow + "<td style='text-align:right'>" + item.cr_amt + "</td>";
                    htmlRow = htmlRow + "<td style='text-align:right'>" + item.cb + "</td>";
                    htmlRow = htmlRow + "</tr>";
                    $('#tblSalesHeader > tbody').append(htmlRow);
                });
                reDrawSalesHeaderTable();
                $('#modalTitle').html('');
                $('#modalTitle').html(title);
                $('#modalDetail').show();
            },
            error: function (err) {
                debugger;
                alert(err.message);
                fnHideProgress();
            }
        });
    }
    catch(err){

    }
}

function fnLstNoOfItemOnChange() {
    try {
        fnGetFastMovingItemChart(gStoreNo);
    }
    catch (err) {

    }
}

