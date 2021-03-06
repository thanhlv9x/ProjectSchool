﻿var mabp;
var macb;
var tenbp;
var tencb;
var clickBP = false;
var clickCB = false;
var clickTH = false;
function getBXL(diem) {
    var xeploai = "Không hoàn thành nhiệm vụ";
    $.ajax({
        url: url + "/api/KetQuaDanhGiaAPI?_BXL=1",
        type: "GET",
        dataType: "json",
        async: false,
        success: function (result) {
            result.map(function (item) {
                if (diem >= item["Diem"]) {
                    xeploai = item["XepLoai"];
                }
            })
        },
        error: function (xhr) {}
    })
    return xeploai;
}

// ============ Tổng hợp =============
// Nút tên bộ phận: sự kiện click vào menu xem kết quả
$("#menu-xem-ket-qua-danh-gia").click(function () {
    // Tổng hợp
    $("div.white-div-loading").show();
    createYearColumnTH();
    createMonthCircleTH();
    createChartTableTHAll();
    $("#footer-th").hide();
    $("#div-month-circle-th").hide();
    $("#cb-all-th").prop("checked", "checked");
    getBPName();
})
// Tạo biểu đồ cột
function createChartColumnTH(urlStr, titleStr) {
    $("div.white-div-loading").show();
    setTimeout(function () {
        dataSource = new kendo.data.DataSource({
            transport: {
                read: function (options) {
                    $.ajax({
                        type: "GET",
                        url: urlStr,
                        dataType: 'json',
                        async: false,
                        success: function (result) {
                            options.success(result);
                        },
                        error: function (result) {
                            options.error(result);
                        }
                    });
                },
                parameterMap: function (options, operation) {
                    if (operation !== "read" && options.models) {
                        return { models: kendo.stringify(options.models) };
                    }
                }
            },
            schema: {
                model: {
                    fields: {
                        thang: { type: "string" },
                        RHL: { type: "number" },
                        HL: { type: "number" },
                        BT: { type: "number" },
                        KHL: { type: "number" },
                    }
                }
            }
        });
        $("#chart-column-th").kendoChart({
            dataSource: dataSource,
            title: {
                text: titleStr
            },
            legend: {
                position: "top"
            },
            seriesDefaults: {
                type: "column",
                stack: { type: "100%" }
            },
            plotArea: {
                background: "blue",
                opacity: 0.1
            },
            series:
                [{
                    field: "RHL",
                    categoryField: "thang",
                    name: "Rất hài lòng",
                    color: "#37b24d"
                }, {
                    field: "HL",
                    categoryField: "thang",
                    name: "Hài lòng",
                    color: "#228be6"
                }, {
                    field: "BT",
                    categoryField: "thang",
                    name: "Bình thường",
                    color: "#ffd43b"
                }, {
                    field: "KHL",
                    categoryField: "thang",
                    name: "Không hài lòng",
                    color: "#fa5252"
                }],
            categoryAxis: {
                labels: {
                    rotation: -90
                },
                majorGridLines: {
                    visible: false
                }
            },
            valueAxis: {
                labels: {
                    format: "{0:p0}"
                },
                line: {
                    visible: false
                },
                axisCrossingValue: 0
            },
            tooltip: {
                visible: true,
                format: "{0}%",
                template: "#= series.name #: #= value # ( #= kendo.format('{0:P}', percentage) # )"
            }
        });
        $("div.white-div-loading").hide();
    }, 1000);
}
// Tạo dropdownlist chọn năm
function createYearColumnTH() {
    $.ajax({
        type: "GET",
        url: url + "/api/ValuesAPI",
        dataType: "json",
        success: function (data) {
            if (data.length > 0) {
                $("#part-1").show();
                var arr = [];
                var j = 0;
                var arr1 = data[0].split(" ");
                var arr2 = data[1].split(" ");
                for (var i = Number.parseInt(arr1[0]); i <= Number.parseInt(arr2[0]); i++) {
                    arr[j] = { text: i.toString(), value: i.toString() };
                    j++;
                }
                $("#year-column-th").kendoDropDownList({
                    dataTextField: "text",
                    dataValueField: "value",
                    dataSource: arr
                });
                createChartColumnTH(url + "/api/CotDanhGiaAPI/?_Year=" + $("#year-column-th").val(), "Kết quả tổng hợp năm " + $("#year-column-th").val());
            } else {
                $("#year-column-th").val(0);
                $("#part-1").hide();
            }
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    })
}
// Tạo sự kiện cho dropdownlist chọn năm
$("#year-column-th").change(function () {
    createChartColumnTH(url + "/api/CotDanhGiaAPI/?_Year=" + $("#year-column-th").val(), "Kết quả tổng hợp năm " + $("#year-column-th").val());
})
// Tạo phương thức cho sự kiện checkbox xem tất cả để tạo biểu đồ tròn
$("#month-group-th>div:first-child>span").click(function () {
    $("#cb-all-th").prop("checked", !$("#cb-all-th").prop("checked"))
    if ($("#cb-all-th").prop("checked") == true) {
        createChartTableTHAll();
        $("#div-month-circle-th").hide("slow");
    } else {
        createChartTableTHTime();
        $("#div-month-circle-th").show("slow");
    }
})
$("#cb-all-th").change(function () {
    if ($("#cb-all-th").prop("checked") == true) {
        createChartTableTHAll();
        $("#div-month-circle-th").hide("slow");
    } else {
        createChartTableTHTime();
        $("#div-month-circle-th").show("slow");
    }
});
// Tạo biểu đồ tròn
function createChartCircleTH(urlStr, titleStr) {
    dataSource = new kendo.data.DataSource({
        transport: {
            serverFiltering: true,
            read: function (options) {
                $.ajax({
                    type: "GET",
                    url: urlStr,
                    dataType: 'json',
                    success: function (result) {
                        options.success(result);
                    },
                    error: function (result) {
                        options.error(result);
                    }
                });
            },
            parameterMap: function (options, operation) {
                if (operation !== "read" && options.models) {
                    return { models: kendo.stringify(options.models) };
                }
            }
        },
        schema: {
            model: {
                fields: {
                    category: { type: "string" },
                    value: { type: "number" },
                    color: { type: "string" },
                }
            }
        },
    });
    $("#chart-circle-th").kendoChart({
        dataSource: dataSource,
        title: {
            text: titleStr,
            position: "bottom",
        },
        legend: {
            position: "top"
        },
        seriesDefaults: {
            labels: {
                template: "#= category # - #= kendo.format('{0:P}', percentage)#",
                position: "outsideEnd",
                visible: true,
                background: "transparent"
            },
            type: "pie"
        },
        plotArea: {
            background: "blue",
            opacity: 0.1
        },
        series: [
            {
                field: "value",
                categoryField: "category",
                color: "color"
            }
        ],
        tooltip: {
            visible: true,
            template: "#= category # - #= kendo.format('{0:P}', percentage) #"
        }
    });

}
// Tạo thanh chọn thời gian cho biểu đồ tròn
function createMonthCircleTH() {
    $("#month-circle-th").prop("readonly", false)
    $("#month-circle-th").html("");
    $.ajax({
        type: "GET",
        url: url + "/api/ValuesAPI",
        dataType: "json",
        success: function (data) {
            if (data.length > 0) {
                $("#body-th").show();
                //$("#footer-th").show();
                $("#month-circle-th").kendoDatePicker({
                    start: "year",
                    depth: "year",
                    format: "MM yyyy",
                    dateInput: false,
                    value: new Date(),
                    min: new Date(data[0]),
                    max: new Date(data[1]),
                    //disableDates: ["sa", "su"]
                });
                $("#month-circle-th").prop("readonly", true)
            } else {
                $("#month-circle-th").val("09 2018");
                $("#body-th").hide();
                $("#footer-th").hide();
            }
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    })
}
// Tạo sự kiện chọn thời gian
$("#month-circle-th").change(function () {
    createChartTableTHTime();
})
// Tạo bảng chi tiết
function createTableTH(urlStr, titleStr) {
    dataSource = new kendo.data.DataSource({
        transport: {
            serverFiltering: true,
            read: function (options) {
                $.ajax({
                    type: "GET",
                    url: urlStr,
                    dataType: 'json',
                    success: function (result) {
                        var solan = 0;
                        var diem = 0;
                        $.each(result, function (key, value) {
                            solan += parseInt(value["SoLan"]);
                            diem += parseInt(value["Diem"]);
                        })
                        var trungbinh = 0;
                        if (diem != 0 && solan != 0) {
                            trungbinh = diem / solan;
                            trungbinh = Math.round(trungbinh * 25 * 100) / 100;
                        };
                        var xeploai = getBXL(trungbinh);
                        //if (trungbinh >= 90) xeploai = "Hoàn thành xuất sắc nhiệm vụ";
                        //else if (trungbinh >= 70) xeploai = "Hoàn thành tốt nhiệm vụ";
                        //else if (trungbinh >= 50) xeploai = "Hoàn thành nhiệm vụ";
                        //else if (trungbinh < 50) xeploai = "Không hoàn thành nhiệm vụ";
                        $("#diem-tong").val(trungbinh);
                        $("#xep-loai-tong").val(xeploai);
                        options.success(result);
                    },
                    error: function (result) {
                        options.error(result);
                    }
                });
            },
            parameterMap: function (options, operation) {
                if (operation !== "read" && options.models) {
                    return { models: kendo.stringify(options.models) };
                }
            }
        },
        batch: true,
        pageSize: 20,
        schema: {
            model: {
                id: "MucDo",
                fields: {
                    MucDo: { type: "number" },
                    Loai: {
                        type: "string", validation: { required: true }
                    },
                    TyLe: {
                        type: "number", validation: { required: true }
                    },
                    SoLan: {
                        type: "number", validation: { required: true }
                    },
                    Diem: {
                        type: "number", validation: { required: true }
                    }
                }
            }
        },
        aggregate: [
            { field: "SoLan", aggregate: "sum" },
            { field: "Diem", aggregate: "sum" }]
    });

    var grid = $("#grid-all-th").kendoGrid({
        dataSource: dataSource,
        navigatable: true,
        pageable: {
            refresh: true,
            messages: {
                display: "{0}-{1}/{2}",
                empty: "Dữ liệu không tồn tại",
            }
        },
        columns: [
            { field: "Loai", title: "Mức độ đánh giá", width: 1 },
            { field: "TyLe", title: "Tỷ lệ (%)", width: 1 },
            {
                field: "SoLan", title: "Số lần", width: 1,
                aggregates: ["sum"],
                footerTemplate: "<div>Tổng cộng: #=sum#</div>"
            },
            {
                field: "Diem", title: "Điểm", width: 1,
                aggregates: ["sum"],
                footerTemplate: "<div>Tổng cộng: #=sum#</div>"
            }
        ],
    }).data("kendoGrid");
    $("#title-all-th").text(titleStr);
}
// Tạo bảng chi tiết (Mức độ đánh giá)
function createTableTH_MucDo(urlStr, titleStr, idGrid, idSpan) {
    //$(idGrid).html("");
    dataSource = new kendo.data.DataSource({
        transport: {
            serverFiltering: true,
            read: function (options) {
                $.ajax({
                    type: "GET",
                    url: urlStr,
                    dataType: 'json',
                    success: function (result) {
                        options.success(result);
                    },
                    error: function (result) {
                        options.error(result);
                    }
                });
            },
            parameterMap: function (options, operation) {
                if (operation !== "read" && options.models) {
                    return { models: kendo.stringify(options.models) };
                }
            }
        },
        batch: true,
        pageSize: 20,
        schema: {
            model: {
                id: "MaCB",
                fields: {
                    MaCB: { type: "number" },
                    HoTen: {
                        type: "string", validation: { required: true }
                    },
                    TyLe: {
                        type: "number", validation: { required: true }
                    },
                    SoLan: {
                        type: "number", validation: { required: true }
                    },
                    Diem: {
                        type: "number", validation: { required: true }
                    }
                }
            }
        },
        aggregate: [
            { field: "SoLan", aggregate: "sum" },
            { field: "Diem", aggregate: "sum" },
            { field: "Diem", aggregate: "average" }]
    });

    var grid = $(idGrid).kendoGrid({
        dataSource: dataSource,
        navigatable: true,
        pageable: {
            refresh: true,
            messages: {
                display: "{0}-{1}/{2}",
                empty: "Dữ liệu không tồn tại",
            }
        },
        columns: [
            { field: "HoTen", title: "Bộ phận", width: 5 },
            { field: "TyLe", title: "Tỷ lệ (%)", width: 2 },
            {
                field: "SoLan", title: "Số lần", width: 3,
                aggregates: ["sum"],
                footerTemplate: "<div>Tổng cộng: #=sum#</div>"
            },
            {
                field: "Diem", title: "Điểm", width: 3,
                aggregates: ["sum"],
                footerTemplate: "<div>Tổng cộng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div>"
            }
        ],
    }).data("kendoGrid");
    $(idSpan).text(titleStr);
}
// Phương thức tạo đồ thị và bảng
function createChartTableTHAll() {
    var urlStr = url + "/api/KetQuaDanhGiaAPI";
    createChartCircleTH(urlStr, "Kết quả đánh giá (Tổng hợp)");

    var urlStr1 = url + "/api/BangDanhGiaAPI";
    createTableTH(urlStr1, "Kết quả đánh giá (Tổng hợp)");

    var urlStr2 = url + "/api/BangDanhGiaAPI/?_MucDo=" + 1;
    createTableTH_MucDo(urlStr2, "Mức độ - Rất hài lòng (Tổng hợp)", "#grid-RHL-th", "#title-RHL-th")

    var urlStr3 = url + "/api/BangDanhGiaAPI/?_MucDo=" + 2;
    createTableTH_MucDo(urlStr3, "Mức độ - Hài lòng (Tổng hợp)", "#grid-HL-th", "#title-HL-th")

    var urlStr4 = url + "/api/BangDanhGiaAPI/?_MucDo=" + 3;
    createTableTH_MucDo(urlStr4, "Mức độ - Bình thường (Tổng hợp)", "#grid-BT-th", "#title-BT-th")

    var urlStr5 = url + "/api/BangDanhGiaAPI/?_MucDo=" + 4;
    createTableTH_MucDo(urlStr5, "Mức độ - Không hài lòng (Tổng hợp)", "#grid-KHL-th", "#title-KHL-th")

    $("#ds-xl-th").text("ĐIỂM SỐ - XẾP LOẠI (TỔNG HỢP)");
}
// Phương thức tạo đồ thị và bảng theo thời gian
function createChartTableTHTime() {
    var date = $("#month-circle-th").val();
    var time = '_ThoiGian=' + date;
    var urlStr = url + "/api/KetQuaDanhGiaAPI/?" + time;
    createChartCircleTH(urlStr, "Kết quả đánh giá (" + date + ")");

    var urlStr1 = url + "/api/BangDanhGiaAPI/?" + time;
    createTableTH(urlStr1, "Kết quả đánh giá (" + date + ")");

    var urlStr2 = url + "/api/BangDanhGiaAPI/?_MucDo=" + 1 + "&" + time;
    createTableTH_MucDo(urlStr2, "Mức độ - Rất hài lòng (" + date + ")", "#grid-RHL-th", "#title-RHL-th")

    var urlStr3 = url + "/api/BangDanhGiaAPI/?_MucDo=" + 2 + "&" + time;
    createTableTH_MucDo(urlStr3, "Mức độ - Hài lòng (" + date + ")", "#grid-HL-th", "#title-HL-th")

    var urlStr4 = url + "/api/BangDanhGiaAPI/?_MucDo=" + 3 + "&" + time;
    createTableTH_MucDo(urlStr4, "Mức độ - Bình thường (" + date + ")", "#grid-BT-th", "#title-BT-th")

    var urlStr5 = url + "/api/BangDanhGiaAPI/?_MucDo=" + 4 + "&" + time;
    createTableTH_MucDo(urlStr5, "Mức độ - Không hài lòng (" + date + ")", "#grid-KHL-th", "#title-KHL-th")

    $("#ds-xl-th").text("ĐIỂM SỐ - XẾP LOẠI (" + date + ")");
}
// Tạo sự kiện nút xem chi tiết
$("#click-details-th").click(function () {
    if (!clickTH) {
        $("#footer-th").show("slow");
        $("#click-details-th").text("Thu gọn");
        clickTH = true;
        return;
    } else {
        $("#footer-th").hide("slow");
        $("#click-details-th").text("Xem chi tiết");
        clickTH = false;
        return;
    }
})
$("#diem-tong, #xep-loai-tong, #diem-bp, #xep-loai-bp, #diem-cb, #xep-loai-cb").kendoMaskedTextBox();

// ============ Bộ phận ==============
// Nút tên bộ phận: sử dụng ajax để lấy dữ liệu tên và mã bộ phận
function getBPName() {
    var str = "";
    $.ajax({
        type: "GET",
        url: url + "/api/BoPhanAPI",
        dataType: "json",
        success: function (data) {
            $.each(data, function (key, val) {
                str += "<div id='" + val.MaBP + "' class='btnBP'>" + val.TenBP + "</div>";
            });
            $("#cac-bo-phan").html(str);
            createButtonBP();
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    });
}
// Nút tên bộ phận: tạo phương thức click
function onClickBtnBP(e) {
    $("div.white-div-loading").show();
    $("#tong-hop").hide("slow");
    $("#cac-bo-phan").hide("slow");
    $("#bo-phan").show("slow");
    $("#div-month-circle-1").hide();
    $("#table-details-bp").hide();
    $("#click-details-bp").text("Xem chi tiết");
    $("#content-ten-bo-phan").hide();
    clickBP = false;
    setTimeout(function () {
        mabp = $(e.event.target).attr("id");
        tenbp = $(e.event.target).text();
        $("#bo-phan div h1").text($(e.event.target).text());
        createMonthCircle(mabp);
        createChartTableAll();
        createYearColumn(mabp);
        $("#cb-all").prop("checked", true);
        getCBName(url + "/api/BoPhanAPI/?_MaBP=" + mabp);
    }, 500);
}
// Nút tên bộ phận: tạo form nút
function createButtonBP() {
    $(".btnBP").each(function (index) {
        $(this).kendoButton({
            click: onClickBtnBP
        });
    });
}
// Tạo sự kiện nút quay lại bộ phận
function backBP() {
    $("#cac-bo-phan").show("slow");
    $("#bo-phan").hide("slow");
    $("#tong-hop").show("slow");
}
// Tạo sự kiện nút so sánh bộ phận
function compareBP() {
    createWindowCompare(0, mabp, tenbp, 0, 0);
}
// Tạo biểu đồ tròn
function createChartCircleBP(urlStr, titleStr) {
    dataSource = new kendo.data.DataSource({
        transport: {
            serverFiltering: true,
            read: function (options) {
                $.ajax({
                    type: "GET",
                    url: urlStr,
                    dataType: 'json',
                    success: function (result) {
                        options.success(result);
                    },
                    error: function (result) {
                        options.error(result);
                    }
                });
            },
            parameterMap: function (options, operation) {
                if (operation !== "read" && options.models) {
                    return { models: kendo.stringify(options.models) };
                }
            }
        },
        schema: {
            model: {
                fields: {
                    category: { type: "string" },
                    value: { type: "number" },
                    color: { type: "string" },
                }
            }
        },
    });
    $("#chart-circle-bp").kendoChart({
        dataSource: dataSource,
        title: {
            text: titleStr,
            position: "bottom",
        },
        legend: {
            position: "top"
        },
        seriesDefaults: {
            labels: {
                template: "#= category # - #= kendo.format('{0:P}', percentage)#",
                position: "outsideEnd",
                visible: true,
                background: "transparent"
            },
            type: "pie"
        },
        plotArea: {
            background: "blue",
            opacity: 0.1
        },
        series: [
            {
                field: "value",
                categoryField: "category",
                color: "color"
            }
        ],
        tooltip: {
            visible: true,
            template: "#= category # - #= kendo.format('{0:P}', percentage) #"
        }
    });

}
// Tạo bảng chi tiết
function createTableBP(urlStr, titleStr) {
    //$("#grid-ty-le").html("");
    dataSource = new kendo.data.DataSource({
        transport: {
            serverFiltering: true,
            read: function (options) {
                $.ajax({
                    type: "GET",
                    url: urlStr,
                    dataType: 'json',
                    success: function (result) {
                        var solan = 0;
                        var diem = 0;
                        $.each(result, function (key, value) {
                            solan += parseInt(value["SoLan"]);
                            diem += parseInt(value["Diem"]);
                        })
                        var trungbinh = 0;
                        if (diem != 0 && solan != 0) {
                            trungbinh = diem / solan;
                            trungbinh = Math.round(trungbinh * 25 * 100) / 100;
                        };
                        var xeploai = getBXL(trungbinh);
                        //if (trungbinh >= 90) xeploai = "Hoàn thành xuất sắc nhiệm vụ";
                        //else if (trungbinh >= 70) xeploai = "Hoàn thành tốt nhiệm vụ";
                        //else if (trungbinh >= 50) xeploai = "Hoàn thành nhiệm vụ";
                        //else if (trungbinh < 50) xeploai = "Không hoàn thành nhiệm vụ";
                        $("#diem-bp").val(trungbinh);
                        $("#xep-loai-bp").val(xeploai);
                        options.success(result);
                    },
                    error: function (result) {
                        options.error(result);
                    }
                });
            },
            parameterMap: function (options, operation) {
                if (operation !== "read" && options.models) {
                    return { models: kendo.stringify(options.models) };
                }
            }
        },
        batch: true,
        pageSize: 20,
        schema: {
            model: {
                id: "MucDo",
                fields: {
                    MucDo: { type: "number" },
                    Loai: {
                        type: "string", validation: { required: true }
                    },
                    TyLe: {
                        type: "number", validation: { required: true }
                    },
                    SoLan: {
                        type: "number", validation: { required: true }
                    },
                    Diem: {
                        type: "number", validation: { required: true }
                    }
                }
            }
        },
        aggregate: [
            { field: "SoLan", aggregate: "sum" },
            { field: "Diem", aggregate: "sum" }]
    });

    var grid = $("#grid-ty-le").kendoGrid({
        dataSource: dataSource,
        navigatable: true,
        pageable: {
            refresh: true,
            messages: {
                display: "{0}-{1}/{2}",
                empty: "Dữ liệu không tồn tại",
            }
        },
        columns: [
            { field: "Loai", title: "Mức độ đánh giá", width: 1 },
            { field: "TyLe", title: "Tỷ lệ (%)", width: 1 },
            {
                field: "SoLan", title: "Số lần", width: 1,
                aggregates: ["sum"],
                footerTemplate: "<div>Tổng cộng: #=sum#</div>"
            },
            {
                field: "Diem", title: "Điểm", width: 1,
                aggregates: ["sum"],
                footerTemplate: "<div>Tổng cộng: #=sum#</div>"
            }
        ],
    }).data("kendoGrid");
    $("#span-title-table-1").text(titleStr);
}
// Tạo bảng chi tiết (Mức độ đánh giá)
function createTableBP_MucDo(urlStr, titleStr, idGrid, idSpan) {
    //$(idGrid).html("");
    dataSource = new kendo.data.DataSource({
        transport: {
            serverFiltering: true,
            read: function (options) {
                $.ajax({
                    type: "GET",
                    url: urlStr,
                    dataType: 'json',
                    success: function (result) {
                        options.success(result);
                    },
                    error: function (result) {
                        options.error(result);
                    }
                });
            },
            parameterMap: function (options, operation) {
                if (operation !== "read" && options.models) {
                    return { models: kendo.stringify(options.models) };
                }
            }
        },
        batch: true,
        pageSize: 20,
        schema: {
            model: {
                id: "MaCB",
                fields: {
                    MaCB: { type: "number" },
                    HoTen: {
                        type: "string", validation: { required: true }
                    },
                    TyLe: {
                        type: "number", validation: { required: true }
                    },
                    SoLan: {
                        type: "number", validation: { required: true }
                    },
                    Diem: {
                        type: "number", validation: { required: true }
                    }
                }
            }
        },
        aggregate: [
            { field: "SoLan", aggregate: "sum" },
            { field: "Diem", aggregate: "sum" },
            { field: "Diem", aggregate: "average" }]
    });

    var grid = $(idGrid).kendoGrid({
        dataSource: dataSource,
        navigatable: true,
        pageable: {
            refresh: true,
            messages: {
                display: "{0}-{1}/{2}",
                empty: "Dữ liệu không tồn tại",
            }
        },
        columns: [
            { field: "HoTen", title: "Họ tên cán bộ", width: 5 },
            { field: "TyLe", title: "Tỷ lệ (%)", width: 2 },
            {
                field: "SoLan", title: "Số lần", width: 3,
                aggregates: ["sum"],
                footerTemplate: "<div>Tổng cộng: #=sum#</div>"
            },
            {
                field: "Diem", title: "Điểm", width: 3,
                aggregates: ["sum"],
                footerTemplate: "<div>Tổng cộng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div>"
            }
        ],
    }).data("kendoGrid");
    $(idSpan).text(titleStr);
}
// Tạo biểu đồ cột
function createChartColumnBP(urlStr, titleStr) {
    $("div.white-div-loading").show();
    setTimeout(function () {
        dataSource = new kendo.data.DataSource({
            transport: {
                read: function (options) {
                    $.ajax({
                        type: "GET",
                        url: urlStr,
                        dataType: 'json',
                        async: false,
                        success: function (result) {
                            options.success(result);
                        },
                        error: function (result) {
                            options.error(result);
                        }
                    });
                },
                parameterMap: function (options, operation) {
                    if (operation !== "read" && options.models) {
                        return { models: kendo.stringify(options.models) };
                    }
                }
            },
            schema: {
                model: {
                    fields: {
                        thang: { type: "string" },
                        RHL: { type: "number" },
                        HL: { type: "number" },
                        BT: { type: "number" },
                        KHL: { type: "number" },
                    }
                }
            }
        });
        $("#chart-column-bp").kendoChart({
            dataSource: dataSource,
            title: {
                text: titleStr
            },
            legend: {
                position: "top"
            },
            seriesDefaults: {
                type: "column",
                stack: { type: "100%" }
            },
            plotArea: {
                background: "blue",
                opacity: 0.1
            },
            series:
                [{
                    field: "RHL",
                    categoryField: "thang",
                    name: "Rất hài lòng",
                    color: "#37b24d"
                }, {
                    field: "HL",
                    categoryField: "thang",
                    name: "Hài lòng",
                    color: "#228be6"
                }, {
                    field: "BT",
                    categoryField: "thang",
                    name: "Bình thường",
                    color: "#ffd43b"
                }, {
                    field: "KHL",
                    categoryField: "thang",
                    name: "Không hài lòng",
                    color: "#fa5252"
                }],
            categoryAxis: {
                labels: {
                    rotation: -90
                },
                majorGridLines: {
                    visible: false
                }
            },
            valueAxis: {
                labels: {
                    format: "{0:p0}"
                },
                line: {
                    visible: false
                },
                axisCrossingValue: 0
            },
            tooltip: {
                visible: true,
                format: "{0}%",
                template: "#= series.name #: #= value # ( #= kendo.format('{0:P}', percentage) # )"
            }
        });
        $("div.white-div-loading").hide();
    }, 1000);
}
// Phương thức tạo đồ thị và bảng
function createChartTableAll() {
    var urlStr = url + "/api/KetQuaDanhGiaAPI/?_MaBP=" + mabp;
    createChartCircleBP(urlStr, "Kết quả đánh giá (Tổng hợp)");

    var urlStr1 = url + "/api/BangDanhGiaAPI/?_MaBP=" + mabp;
    createTableBP(urlStr1, "Kết quả đánh giá (Tổng hợp)");

    var urlStr2 = url + "/api/BangDanhGiaAPI/?_MaBP=" + mabp + "&_MucDo=" + 1;
    createTableBP_MucDo(urlStr2, "Mức độ - Rất hài lòng (Tổng hợp)", "#grid-rat-hai-long", "#span-title-table-2")

    var urlStr3 = url + "/api/BangDanhGiaAPI/?_MaBP=" + mabp + "&_MucDo=" + 2;
    createTableBP_MucDo(urlStr3, "Mức độ - Hài lòng (Tổng hợp)", "#grid-hai-long", "#span-title-table-3")

    var urlStr4 = url + "/api/BangDanhGiaAPI/?_MaBP=" + mabp + "&_MucDo=" + 3;
    createTableBP_MucDo(urlStr4, "Mức độ - Bình thường (Tổng hợp)", "#grid-binh-thuong", "#span-title-table-4")

    var urlStr5 = url + "/api/BangDanhGiaAPI/?_MaBP=" + mabp + "&_MucDo=" + 4;
    createTableBP_MucDo(urlStr5, "Mức độ - Không hài lòng (Tổng hợp)", "#grid-khong-hai-long", "#span-title-table-5")

    $("#ds-xl-bp").text("ĐIỂM SỐ - XẾP LOẠI (TỔNG HỢP)");
}
// Phương thức tạo đồ thị và bảng theo thời gian
function createChartTableTime() {
    var time = '&_ThoiGian=' + $("#month-circle").val();
    var date = $("#month-circle").val();
    var urlStr = url + "/api/KetQuaDanhGiaAPI/?_MaBP=" + mabp + time;
    createChartCircleBP(urlStr, "Kết quả đánh giá (" + date + ")");

    var urlStr1 = url + "/api/BangDanhGiaAPI/?_MaBP=" + mabp + time;
    createTableBP(urlStr1, "Kết quả đánh giá (" + date + ")");

    var urlStr2 = url + "/api/BangDanhGiaAPI/?_MaBP=" + mabp + "&_MucDo=" + 1 + time;
    createTableBP_MucDo(urlStr2, "Mức độ - Rất hài lòng (" + date + ")", "#grid-rat-hai-long", "#span-title-table-2")

    var urlStr3 = url + "/api/BangDanhGiaAPI/?_MaBP=" + mabp + "&_MucDo=" + 2 + time;
    createTableBP_MucDo(urlStr3, "Mức độ - Hài lòng (" + date + ")", "#grid-hai-long", "#span-title-table-3")

    var urlStr4 = url + "/api/BangDanhGiaAPI/?_MaBP=" + mabp + "&_MucDo=" + 3 + time;
    createTableBP_MucDo(urlStr4, "Mức độ - Bình thường (" + date + ")", "#grid-binh-thuong", "#span-title-table-4")

    var urlStr5 = url + "/api/BangDanhGiaAPI/?_MaBP=" + mabp + "&_MucDo=" + 4 + time;
    createTableBP_MucDo(urlStr5, "Mức độ - Không hài lòng (" + date + ")", "#grid-khong-hai-long", "#span-title-table-5")

    $("#ds-xl-bp").text("ĐIỂM SỐ - XẾP LOẠI (" + date + ")");
}
// Tạo phương thức cho sự kiện checkbox xem tất cả để tạo biểu đồ tròn
$("#div-month-circle>div:first-child>span").click(function () {
    $("#cb-all").prop("checked", !$("#cb-all").prop("checked"))
    if ($("#cb-all").prop("checked") == true) {
        createChartTableAll();
        $("#div-month-circle-1").hide("slow");
    } else {
        //createMonthCircle(mabp);
        createChartTableTime();
        $("#div-month-circle-1").show("slow");
    }
})
$("#cb-all").change(function () {
    if ($("#cb-all").prop("checked") == true) {
        createChartTableAll();
        $("#div-month-circle-1").hide("slow");
    } else {
        //createMonthCircle(mabp);
        createChartTableTime();
        $("#div-month-circle-1").show("slow");
    }
});
// Tạo thanh chọn thời gian cho biểu đồ tròn
function createMonthCircle(MaBP) {
    $("#month-circle").prop("readonly", false)
    $("#month-circle").html("");
    $.ajax({
        type: "GET",
        url: url + "/api/ValuesAPI/?_MaBP=" + MaBP,
        dataType: "json",
        success: function (data) {
            if (data.length > 0) {
                $("#content-bp").show();
                $("#month-circle").kendoDatePicker({
                    start: "year",
                    depth: "year",
                    format: "MM yyyy",
                    dateInput: false,
                    value: new Date(),
                    min: new Date(data[0]),
                    max: new Date(data[1]),
                    //disableDates: ["sa", "su"]
                });
                $("#month-circle").prop("readonly", true)
            } else {
                $("#month-circle").val("09 2018");
                $("#content-bp").hide();
                $("div.white-div-loading").hide();
            }
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    })
}
// Tạo sự kiện chọn thời gian
$("#month-circle").change(function () {
    createChartTableTime();
})
// Tạo nút xem chi tiết
$("#click-details-bp").click(function () {
    if (!clickBP) {
        $("#table-details-bp").show("slow");
        $("#click-details-bp").text("Thu gọn");
        clickBP = true;
        return;
    } else {
        $("#table-details-bp").hide("slow");
        $("#click-details-bp").text("Xem chi tiết");
        clickBP = false;
        return;
    }
})
// Tạo dropdownlist chọn năm
function createYearColumn(MaBP) {
    $.ajax({
        type: "GET",
        url: url + "/api/ValuesAPI/?_MaBP=" + MaBP,
        dataType: "json",
        success: function (data) {
            if (data.length > 0) {
                $("#content-bp").show();
                $(".compareBP").show();
                var arr = [];
                var j = 0;
                var arr1 = data[0].split(" ");
                var arr2 = data[1].split(" ");
                for (var i = Number.parseInt(arr1[0]); i <= Number.parseInt(arr2[0]); i++) {
                    arr[j] = { text: i.toString(), value: i.toString() };
                    j++;
                }
                $("#year-column").kendoDropDownList({
                    dataTextField: "text",
                    dataValueField: "value",
                    dataSource: arr
                });
                createChartColumnBP(url + "/api/CotDanhGiaAPI/?_MaBP=" + mabp + "&_Year=" + $("#year-column").val(), "Kết quả tổng hợp năm " + $("#year-column").val());
            } else {
                $("#div-year-column").val(0);
                $(".compareBP").hide();
                $("#content-bp").hide();
                $("div.white-div-loading").hide();
            }
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    })
}
// Tạo sự kiện cho dropdownlist chọn năm
$("#year-column").change(function () {
    createChartColumnBP(url + "/api/CotDanhGiaAPI/?_MaBP=" + mabp + "&_Year=" + $("#year-column").val(), "Kết quả tổng hợp năm " + $("#year-column").val());
})

// ============ Cán bộ ==============
// Tạo sự kiện nút quay lại cán bộ
function backCB() {
    $("#bo-phan").show("slow");
    $("#can-bo").hide("slow");
}
// Tạo sự kiện nút so sánh cán bộ
function compareCB() {
    createWindowCompare(macb, mabp, " Cán bộ: " + tencb, 0, 1);
}
// Nút tên cán bộ: sử dụng ajax để lấy dữ liệu tên và mã cán bộ
function getCBName(urlStr) {
    var str = "<table style='width: 98%; margin: 1%'>";
    $.ajax({
        type: "GET",
        url: urlStr,
        dataType: "json",
        success: function (data) {
            $("#content-ten-can-bo").show();
            if (data.length > 0) {
                //var i = 0;
                $.each(data, function (key, val) {
                    str += "<tr><td><div id='" + val.MaCB + "' class='btnCB' style='width: 100%; margin: 2px'><span class=' k-icon k-i-user'></span>" + val.HoTen + "</div></td></tr>";
                    //if (i == 3) {
                    //    str += "</tr><tr><td><div id='" + val.MaCB + "' class='btnCB' style='width: 100%; margin: 2px'><span class=' k-icon k-i-user'></span>" + val.HoTen + " - " + val.MaCB + "</div></td>";
                    //    i = 0;
                    //}
                    //i++;
                });
                str += "</table>";
                $("#div-button-canbo").html(str);
                createButtonCB();
            } else {
                $("#content-ten-can-bo").hide();
                $("div.white-div-loading").hide();
            }
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    })
}
// Nút tên cán bộ: tạo form nút
function createButtonCB() {
    $(".btnCB").each(function (index) {
        $(this).kendoButton({
            click: onClickBtnCB
        });
    });
}
// Nút tên cán bộ: tạo phương thức click
function onClickBtnCB(e) {
    $("div.white-div-loading").show();
    $("#bo-phan").hide("slow");
    $("#can-bo").show("slow");
    $("#div-month-circle-cb-1").hide();
    $("#table-details-cb").hide();
    $("#table-ket-qua-cb").hide();
    $("#click-details-cb").text("Xem chi tiết");
    clickCB = false;
    macb = $(e.event.target).attr("id");
    createInfoCB(macb);
    setTimeout(function () {
        tencb = "Cán bộ: " + $(e.event.target).text();
        $("#can-bo div h1").text("Cán bộ: " + $(e.event.target).text());
        createYearColumnCB(macb);
        createChartCircleCB(url + "/api/KetQuaDanhGiaAPI/?_MaCB=" + macb, "Kết quả đánh giá (Tổng hợp)");
        createMonthCircleCB(macb);
        createTableCB(url + "/api/BangDanhGiaAPI/?_MaCB=" + macb, "Kết quả đánh giá (Tổng hợp)");
        createTableGopY(url + "/api/BangDanhGiaAPI/?_MaCBGopY=" + macb, "Bảng góp ý (Tổng hợp)");
        createTableKetQua(url + "/api/BangDanhGiaAPI/?_MaCB=" + macb + "&_KetQua=1", "Bảng kết quả đánh giá (Tổng hợp)")
        $("#cb-all-cb").prop("checked", true);
        $("#ds-cb").text("ĐIỂM SỐ (TỔNG HỢP)");
        $("#xl-cb").text("XẾP LOẠI (TỔNG HỢP)");
    }, 800);
}
// Tạo biểu đồ cột
function createChartColumnCB(urlStr, titleStr) {
    $("div.white-div-loading").show();
    setTimeout(function () {
        dataSource = new kendo.data.DataSource({
            transport: {
                read: function (options) {
                    $.ajax({
                        type: "GET",
                        url: urlStr,
                        dataType: 'json',
                        async: false,
                        success: function (result) {
                            options.success(result);
                        },
                        error: function (result) {
                            options.error(result);
                        }
                    });
                },
                parameterMap: function (options, operation) {
                    if (operation !== "read" && options.models) {
                        return { models: kendo.stringify(options.models) };
                    }
                }
            },
            schema: {
                model: {
                    fields: {
                        thang: { type: "string" },
                        RHL: { type: "number" },
                        HL: { type: "number" },
                        BT: { type: "number" },
                        KHL: { type: "number" },
                    }
                }
            }
        });
        $("#chart-column-cb").kendoChart({
            dataSource: dataSource,
            title: {
                text: titleStr
            },
            legend: {
                position: "top"
            },
            seriesDefaults: {
                type: "column",
                stack: { type: "100%" }
            },
            plotArea: {
                background: "blue",
                opacity: 0.1
            },
            series:
                [{
                    field: "RHL",
                    categoryField: "thang",
                    name: "Rất hài lòng",
                    color: "#37b24d"
                }, {
                    field: "HL",
                    categoryField: "thang",
                    name: "Hài lòng",
                    color: "#228be6"
                }, {
                    field: "BT",
                    categoryField: "thang",
                    name: "Bình thường",
                    color: "#ffd43b"
                }, {
                    field: "KHL",
                    categoryField: "thang",
                    name: "Không hài lòng",
                    color: "#fa5252"
                }],
            categoryAxis: {
                labels: {
                    rotation: -90
                },
                majorGridLines: {
                    visible: false
                }
            },
            valueAxis: {
                labels: {
                    format: "{0:p0}"
                },
                line: {
                    visible: false
                },
                axisCrossingValue: 0
            },
            tooltip: {
                visible: true,
                format: "{0}%",
                template: "#= series.name #: #= value # ( #= kendo.format('{0:P}', percentage) # )"
            }
        });
        $("div.white-div-loading").hide();
    }, 1000);
}
// Tạo biểu đồ tròn
function createChartCircleCB(urlStr, titleStr) {
    dataSource = new kendo.data.DataSource({
        transport: {
            serverFiltering: true,
            read: function (options) {
                $.ajax({
                    type: "GET",
                    url: urlStr,
                    dataType: 'json',
                    success: function (result) {
                        options.success(result);
                    },
                    error: function (result) {
                        options.error(result);
                    }
                });
            },
            parameterMap: function (options, operation) {
                if (operation !== "read" && options.models) {
                    return { models: kendo.stringify(options.models) };
                }
            }
        },
        schema: {
            model: {
                fields: {
                    category: { type: "string" },
                    value: { type: "number" },
                    color: { type: "string" },
                }
            }
        },
    });
    $("#chart-circle-cb").kendoChart({
        dataSource: dataSource,
        title: {
            text: titleStr,
            position: "bottom",
        },
        legend: {
            position: "top"
        },
        seriesDefaults: {
            labels: {
                template: "#= category # - #= kendo.format('{0:P}', percentage)#",
                position: "outsideEnd",
                visible: true,
                background: "transparent"
            },
            type: "pie"
        },
        plotArea: {
            background: "blue",
            opacity: 0.1
        },
        series: [
            {
                field: "value",
                categoryField: "category",
                color: "color"
            }
        ],
        tooltip: {
            visible: true,
            template: "#= category # - #= kendo.format('{0:P}', percentage) #"
        }
    });
}
// Tạo bảng chi tiết
function createTableCB(urlStr, titleStr) {
    //$("#grid-ty-le-cb").html("");
    dataSource = new kendo.data.DataSource({
        transport: {
            serverFiltering: true,
            read: function (options) {
                $.ajax({
                    type: "GET",
                    url: urlStr,
                    dataType: 'json',
                    success: function (result) {
                        var solan = 0;
                        var diem = 0;
                        $.each(result, function (key, value) {
                            solan += parseInt(value["SoLan"]);
                            diem += parseInt(value["Diem"]);
                        })
                        var trungbinh = 0;
                        if (diem != 0 && solan != 0) {
                            trungbinh = diem / solan;
                            trungbinh = Math.round(trungbinh * 25 * 100) / 100;
                        };
                        var xeploai = getBXL(trungbinh);
                        //if (trungbinh >= 90) xeploai = "Hoàn thành xuất sắc nhiệm vụ";
                        //else if (trungbinh >= 70) xeploai = "Hoàn thành tốt nhiệm vụ";
                        //else if (trungbinh >= 50) xeploai = "Hoàn thành nhiệm vụ";
                        //else if (trungbinh < 50) xeploai = "Không hoàn thành nhiệm vụ";
                        $("#diem-cb").val(trungbinh);
                        $("#xep-loai-cb").val(xeploai);
                        options.success(result);
                    },
                    error: function (result) {
                        options.error(result);
                    }
                });
            },
            parameterMap: function (options, operation) {
                if (operation !== "read" && options.models) {
                    return { models: kendo.stringify(options.models) };
                }
            }
        },
        batch: true,
        pageSize: 20,
        schema: {
            model: {
                id: "MucDo",
                fields: {
                    MucDo: { type: "number" },
                    Loai: {
                        type: "string", validation: { required: true }
                    },
                    TyLe: {
                        type: "number", validation: { required: true }
                    },
                    SoLan: {
                        type: "number", validation: { required: true }
                    },
                    Diem: {
                        type: "number", validation: { required: true }
                    }
                }
            }
        },
        aggregate: [
            { field: "SoLan", aggregate: "sum" },
            { field: "Diem", aggregate: "sum" }
        ]
    });

    var grid = $("#grid-ty-le-cb").kendoGrid({
        dataSource: dataSource,
        navigatable: true,
        pageable: {
            refresh: true,
            messages: {
                display: "{0}-{1}/{2}",
                empty: "Dữ liệu không tồn tại",
            }
        },
        columns: [
            { field: "Loai", title: "Mức độ đánh giá", width: 1 },
            { field: "TyLe", title: "Tỷ lệ (%)", width: 1 },
            {
                field: "SoLan", title: "Số lần", width: 1,
                aggregates: ["sum"],
                footerTemplate: "<div>Tổng cộng: #=sum#</div>"
            },
            {
                field: "Diem", title: "Điểm", width: 1,
                aggregates: ["sum"],
                footerTemplate: "<div>Tổng cộng: #=sum#</div>"
            }
        ],
    }).data("kendoGrid");
    $("#span-title-table-cb-1").text(titleStr);
}
// Tạo bảng góp ý
function createTableGopY(urlStr, titleStr) {
    //$("#grid-gop-y").html("");
    dataSource = new kendo.data.DataSource({
        transport: {
            serverFiltering: true,
            read: function (options) {
                $.ajax({
                    type: "GET",
                    url: urlStr,
                    dataType: 'json',
                    success: function (result) {
                        options.success(result);
                    },
                    error: function (result) {
                        options.error(result);
                    }
                });
            },
            parameterMap: function (options, operation) {
                if (operation !== "read" && options.models) {
                    return { models: kendo.stringify(options.models) };
                }
            }
        },
        batch: true,
        pageSize: 20,
        schema: {
            model: {
                id: "MaDG",
                fields: {
                    MaDG: { type: "number" },
                    MucDo: {
                        type: "string", validation: { required: true }
                    },
                    GopY: {
                        type: "string", validation: { required: true }
                    },
                    ThoiGian: {
                        type: "date", validation: { required: true }
                    },
                    Ngay: {
                        type: "date", validation: { required: true }
                    },
                    SoLan: {
                        type: "number", validation: { required: true }
                    }
                }
            }
        },
        aggregate: { field: "SoLan", aggregate: "sum" }
    });

    var grid = $("#grid-gop-y").kendoGrid({
        dataSource: dataSource,
        navigatable: true,
        pageable: {
            refresh: true,
            messages: {
                display: "{0}-{1}/{2}",
                empty: "Dữ liệu không tồn tại",
            }
        },
        columns: [
            { field: "Ngay", title: "Ngày", width: 1, format: "{0:dd MM yyyy}" },
            { field: "ThoiGian", title: "Thời gian", width: 1, format: "{0:hh mm ss}" },
            { field: "MucDo", title: "Mức độ đánh giá", width: 1 },
            { field: "GopY", title: "Góp ý", width: 5 },
            {
                field: "SoLan", title: "Số lần", width: 1,
                aggregates: ["sum"],
                footerTemplate: "<div>Tổng cộng: #=sum#</div>"
            }
        ],
    }).data("kendoGrid");
    $("#span-title-table-gop-y").text(titleStr);
}
// Tạo bảng góp ý
function createTableKetQua(urlStr, titleStr) {
    //$("#grid-gop-y").html("");
    dataSource = new kendo.data.DataSource({
        transport: {
            serverFiltering: true,
            read: function (options) {
                $.ajax({
                    type: "GET",
                    url: urlStr,
                    dataType: 'json',
                    success: function (result) {
                        options.success(result);
                    },
                    error: function (result) {
                        options.error(result);
                    }
                });
            },
            parameterMap: function (options, operation) {
                if (operation !== "read" && options.models) {
                    return { models: kendo.stringify(options.models) };
                }
            }
        },
        batch: true,
        pageSize: 20,
        schema: {
            model: {
                id: "Ngay",
                fields: {
                    Ngay: { type: "date", validation: { required: true } },
                    RHL_SoLan: { type: "number", validation: { required: true } },
                    HL_SoLan: { type: "number", validation: { required: true } },
                    BT_SoLan: { type: "number", validation: { required: true } },
                    KHL_SoLan: { type: "number", validation: { required: true } },
                    RHL_TyLe: { type: "number", validation: { required: true } },
                    HL_TyLe: { type: "number", validation: { required: true } },
                    BT_TyLe: { type: "number", validation: { required: true } },
                    KHL_TyLe: { type: "number", validation: { required: true } },
                }
            }
        },
        group: {
            field: "Ngay", aggregates: [
                { field: "Ngay", aggregate: "count" },
                { field: "RHL_TyLe", aggregate: "average" },
                { field: "RHL_SoLan", aggregate: "sum" },
                { field: "HL_TyLe", aggregate: "average" },
                { field: "HL_SoLan", aggregate: "sum" },
                { field: "BT_TyLe", aggregate: "average" },
                { field: "BT_SoLan", aggregate: "sum" },
                { field: "KHL_TyLe", aggregate: "average" },
                { field: "KHL_SoLan", aggregate: "sum" },
                { field: "RHL_TyLe", aggregate: "min" },
                { field: "RHL_SoLan", aggregate: "min" },
                { field: "HL_TyLe", aggregate: "min" },
                { field: "HL_SoLan", aggregate: "min" },
                { field: "BT_TyLe", aggregate: "min" },
                { field: "BT_SoLan", aggregate: "min" },
                { field: "KHL_TyLe", aggregate: "min" },
                { field: "KHL_SoLan", aggregate: "min" },
                { field: "RHL_TyLe", aggregate: "max" },
                { field: "RHL_SoLan", aggregate: "max" },
                { field: "HL_TyLe", aggregate: "max" },
                { field: "HL_SoLan", aggregate: "max" },
                { field: "BT_TyLe", aggregate: "max" },
                { field: "BT_SoLan", aggregate: "max" },
                { field: "KHL_TyLe", aggregate: "max" },
                { field: "KHL_SoLan", aggregate: "max" },
            ]
        },

        aggregate: [
            { field: "Ngay", aggregate: "count" },
            { field: "RHL_TyLe", aggregate: "average" },
            { field: "RHL_TyLe", aggregate: "sum" },
            { field: "RHL_TyLe", aggregate: "count" },
            { field: "RHL_SoLan", aggregate: "sum" },
            { field: "HL_TyLe", aggregate: "average" },
            { field: "HL_TyLe", aggregate: "sum" },
            { field: "HL_TyLe", aggregate: "count" },
            { field: "HL_SoLan", aggregate: "sum" },
            { field: "BT_TyLe", aggregate: "average" },
            { field: "BT_TyLe", aggregate: "sum" },
            { field: "BT_TyLe", aggregate: "count" },
            { field: "BT_SoLan", aggregate: "sum" },
            { field: "KHL_TyLe", aggregate: "average" },
            { field: "KHL_TyLe", aggregate: "sum" },
            { field: "KHL_TyLe", aggregate: "count" },
            { field: "KHL_SoLan", aggregate: "sum" },
            { field: "RHL_TyLe", aggregate: "min" },
            { field: "RHL_SoLan", aggregate: "min" },
            { field: "HL_TyLe", aggregate: "min" },
            { field: "HL_SoLan", aggregate: "min" },
            { field: "BT_TyLe", aggregate: "min" },
            { field: "BT_SoLan", aggregate: "min" },
            { field: "KHL_TyLe", aggregate: "min" },
            { field: "KHL_SoLan", aggregate: "min" },
            { field: "RHL_TyLe", aggregate: "max" },
            { field: "RHL_SoLan", aggregate: "max" },
            { field: "HL_TyLe", aggregate: "max" },
            { field: "HL_SoLan", aggregate: "max" },
            { field: "BT_TyLe", aggregate: "max" },
            { field: "BT_SoLan", aggregate: "max" },
            { field: "KHL_TyLe", aggregate: "max" },
            { field: "KHL_SoLan", aggregate: "max" },
        ]
    });

    var grid = $("#grid-ket-qua").kendoGrid({
        dataSource: dataSource,
        navigatable: true,
        pageable: {
            refresh: true,
            messages: {
                display: "{0}-{1}/{2}",
                empty: "Dữ liệu không tồn tại",
            }
        },
        columns: [
            { field: "Ngay", title: "Ngày", width: 70, format: "{0:dd MM yyyy}", hidden: true },
            {
                title: "Rất hài lòng",
                columns: [
                    { field: "RHL_TyLe", title: "Tỷ lệ(%)", width: 70, groupFooterTemplate: "Trung bình: #if(average==null){#<span>#=0#%</span>#}else{#<span>#=Math.round(average*100)/100#%</span>#}#", footerTemplate: "Trung bình: #=Math.round((sum/count)*100)/100#%" },
                    { field: "RHL_SoLan", title: "Số lần", width: 70, groupFooterTemplate: "<div>Tổng: #=sum#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>", footerTemplate: "<div>Tổng: #=sum#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>" }
                ]
            },
            {
                title: "Hài lòng",
                columns: [
                    { field: "HL_TyLe", title: "Tỷ lệ(%)", width: 70, groupFooterTemplate: "Trung bình: #if(average==null){#<span>#=0#%</span>#}else{#<span>#=Math.round(average*100)/100#%</span>#}#", footerTemplate: "Trung bình: #=Math.round((sum/count)*100)/100#%" },
                    { field: "HL_SoLan", title: "Số lần", width: 70, groupFooterTemplate: "<div>Tổng: #=sum#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>", footerTemplate: "<div>Tổng: #=sum#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>" }
                ]
            },
            {
                title: "Bình thường",
                columns: [
                    { field: "BT_TyLe", title: "Tỷ lệ(%)", width: 70, groupFooterTemplate: "Trung bình: #if(average==null){#<span>#=0#%</span>#}else{#<span>#=Math.round(average*100)/100#%</span>#}#", footerTemplate: "Trung bình: #=Math.round((sum/count)*100)/100#%" },
                    { field: "BT_SoLan", title: "Số lần", width: 70, groupFooterTemplate: "<div>Tổng: #=sum#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>", footerTemplate: "<div>Tổng: #=sum#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>" }
                ]
            },
            {
                title: "Không hài lòng",
                columns: [
                    { field: "KHL_TyLe", title: "Tỷ lệ(%)", width: 70, groupFooterTemplate: "Trung bình: #if(average==null){#<span>#=0#%</span>#}else{#<span>#=Math.round(average*100)/100#%</span>#}#", footerTemplate: "Trung bình: #=Math.round((sum/count)*100)/100#%" },
                    { field: "KHL_SoLan", title: "Số lần", width: 70, groupFooterTemplate: "<div>Tổng: #=sum#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>", footerTemplate: "<div>Tổng: #=sum#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>" }
                ]
            },
        ],
    }).data("kendoGrid");
    $("#span-title-table-ket-qua").text(titleStr);
}
// Tạo phương thức cho sự kiện checkbox xem tất cả để tạo biểu đồ tròn
$("#div-month-circle-cb>div:first-child>span").click(function () {
    $("#cb-all-cb").prop("checked", !$("#cb-all-cb").prop("checked"))
    var urlStr = url + "/api/KetQuaDanhGiaAPI/?_MaCB=" + macb;
    if ($("#cb-all-cb").prop("checked") == true) {
        createChartCircleCB(urlStr, "Kết quả đánh giá (Tổng hợp)");
        createTableCB(url + "/api/BangDanhGiaAPI/?_MaCB=" + macb, "Kết quả đánh giá (Tổng hợp)");
        createTableGopY(url + "/api/BangDanhGiaAPI/?_MaCBGopY=" + macb, "Bảng góp ý (Tổng hợp)");
        createTableKetQua(url + "/api/BangDanhGiaAPI/?_MaCB=" + macb + "&_KetQua=1", "Bảng kết quả đánh giá (Tổng hợp)")
        $("#div-month-circle-cb-1").hide("slow");
        $("#ds-cb").text("ĐIỂM SỐ (TỔNG HỢP)");
        $("#xl-cb").text("XẾP LOẠI (TỔNG HỢP)");
    } else {
        var time = '&_ThoiGian=' + $("#month-circle-cb").val();
        var date = $("#month-circle-cb").val();
        var urlStr = url + "/api/KetQuaDanhGiaAPI/?_MaCB=" + macb + time;
        createChartCircleCB(urlStr, "Kết quả đánh giá (" + date + ")");
        var urlStr1 = url + "/api/BangDanhGiaAPI/?_MaCB=" + macb + time;
        createTableCB(urlStr1, "Kết quả đánh giá (" + date + ")");
        var urlStr2 = url + "/api/BangDanhGiaAPI/?_MaCBGopY=" + macb + time;
        createTableGopY(urlStr2, "Bảng góp ý (" + date + ")");
        var urlStr3 = url + "/api/BangDanhGiaAPI/?_MaCB=" + macb + time + "&_KetQua=1";
        createTableKetQua(urlStr3, "Bảng kết quả đánh giá (" + date + ")")
        $("#div-month-circle-cb-1").show("slow");
        $("#ds-cb").text("ĐIỂM SỐ (" + date + ")");
        $("#xl-cb").text("XẾP LOẠI (" + date + ")");
    }
})
$("#cb-all-cb").change(function () {
    var urlStr = url + "/api/KetQuaDanhGiaAPI/?_MaCB=" + macb;
    if ($("#cb-all-cb").prop("checked") == true) {
        createChartCircleCB(urlStr, "Kết quả đánh giá (Tổng hợp)");
        createTableCB(url + "/api/BangDanhGiaAPI/?_MaCB=" + macb, "Kết quả đánh giá (Tổng hợp)");
        createTableGopY(url + "/api/BangDanhGiaAPI/?_MaCBGopY=" + macb, "Bảng góp ý (Tổng hợp)");
        createTableKetQua(url + "/api/BangDanhGiaAPI/?_MaCB=" + macb + "&_KetQua=1", "Bảng kết quả đánh giá (Tổng hợp)")
        $("#div-month-circle-cb-1").hide("slow");
        $("#ds-cb").text("ĐIỂM SỐ (TỔNG HỢP)");
        $("#xl-cb").text("XẾP LOẠI (TỔNG HỢP)");
    } else {
        var time = '&_ThoiGian=' + $("#month-circle-cb").val();
        var date = $("#month-circle-cb").val();
        var urlStr = url + "/api/KetQuaDanhGiaAPI/?_MaCB=" + macb + time;
        createChartCircleCB(urlStr, "Kết quả đánh giá (" + date + ")");
        var urlStr1 = url + "/api/BangDanhGiaAPI/?_MaCB=" + macb + time;
        createTableCB(urlStr1, "Kết quả đánh giá (" + date + ")");
        var urlStr2 = url + "/api/BangDanhGiaAPI/?_MaCBGopY=" + macb + time;
        createTableGopY(urlStr2, "Bảng góp ý (" + date + ")");
        var urlStr3 = url + "/api/BangDanhGiaAPI/?_MaCB=" + macb + time + "&_KetQua=1";
        createTableKetQua(urlStr3, "Bảng kết quả đánh giá (" + date + ")")
        $("#div-month-circle-cb-1").show("slow");
        $("#ds-cb").text("ĐIỂM SỐ (" + date + ")");
        $("#xl-cb").text("XẾP LOẠI (" + date + ")");
    }
});
// Tạo thanh chọn thời gian cho biểu đồ tròn
function createMonthCircleCB(MaCB) {
    $("#month-circle-cb").prop("readonly", false)
    $("#month-circle-cb").html("");
    $.ajax({
        type: "GET",
        url: url + "/api/ValuesAPI/?_MaCB=" + MaCB,
        dataType: "json",
        success: function (data) {
            if (data.length > 0) {
                $("#content-can-bo").show();
                $("#month-circle-cb").kendoDatePicker({
                    start: "year",
                    depth: "year",
                    format: "MM yyyy",
                    dateInput: false,
                    value: new Date(),
                    min: new Date(data[0]),
                    max: new Date(data[1]),
                    //disableDates: ["sa", "su"]
                });
                $("#month-circle-cb").prop("readonly", true)
            } else {
                $("#month-circle-cb").val("09 2018");
                $("#content-can-bo").hide();
                $("div.white-div-loading").hide();
            }
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    })
}
// Tạo sự kiện chọn thời gian
$("#month-circle-cb").change(function () {
    var time = '&_ThoiGian=' + $("#month-circle-cb").val();
    var date = $("#month-circle-cb").val();
    var urlStr = url + "/api/KetQuaDanhGiaAPI/?_MaCB=" + macb + time;
    createChartCircleCB(urlStr, "Kết quả đánh giá (" + date + ")");
    var urlStr1 = url + "/api/BangDanhGiaAPI/?_MaCB=" + macb + time;
    createTableCB(urlStr1, "Kết quả đánh giá (" + date + ")");
    var urlStr2 = url + "/api/BangDanhGiaAPI/?_MaCBGopY=" + macb + time;
    createTableGopY(urlStr2, "Bảng góp ý (" + date + ")");
    var urlStr3 = url + "/api/BangDanhGiaAPI/?_MaCB=" + macb + time + "&_KetQua=1";
    createTableKetQua(urlStr3, "Bảng kết quả đánh giá (" + date + ")")
    $("#ds-cb").text("ĐIỂM SỐ (" + date + ")");
    $("#xl-cb").text("XẾP LOẠI (" + date + ")");
})
// Tạo nút xem chi tiết
$("#click-details-cb").click(function () {
    if (!clickCB) {
        $("#table-details-cb").show("slow");
        $("#table-ket-qua-cb").show("slow");
        $("#click-details-cb").text("Thu gọn");
        clickCB = true;
        return;
    } else {
        $("#table-details-cb").hide("slow");
        $("#table-ket-qua-cb").hide("slow");
        $("#click-details-cb").text("Xem chi tiết");
        clickCB = false;
        return;
    }
})
// Tạo dropdownlist chọn năm
function createYearColumnCB(MaCB) {
    $.ajax({
        type: "GET",
        url: url + "/api/ValuesAPI/?_MaCB=" + MaCB,
        dataType: "json",
        success: function (data) {
            if (data.length > 0) {
                $("#content-can-bo").show();
                $(".compareCB").show();
                var arr = [];
                var j = 0;
                var arr1 = data[0].split(" ");
                var arr2 = data[1].split(" ");
                for (var i = Number.parseInt(arr1[0]); i <= Number.parseInt(arr2[0]); i++) {
                    arr[j] = { text: i.toString(), value: i.toString() };
                    j++;
                }
                $("#year-column-cb").kendoDropDownList({
                    dataTextField: "text",
                    dataValueField: "value",
                    dataSource: arr
                });
                createChartColumnCB(url + "/api/CotDanhGiaAPI/?_MaCB=" + MaCB + "&_Year=" + $("#year-column-cb").val(), "Kết quả tổng hợp năm " + $("#year-column-cb").val());
            } else {
                $("#div-year-column-cb").val(0);
                $(".compareCB").hide();
                $("#content-can-bo").hide();
                $("div.white-div-loading").hide();
            }
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    })
}
// Tạo sự kiện cho dropdownlist chọn năm
$("#year-column-cb").change(function () {
    createChartColumnCB(url + "/api/CotDanhGiaAPI/?_MaCB=" + macb + "&_Year=" + $("#year-column-cb").val(), "Kết quả tổng hợp năm " + $("#year-column-cb").val());
})
// Tạo thông tin cán bộ
function createInfoCB(MaCB) {
    //table - core - cb
    $.ajax({
        url: url + "/api/ValuesAPI/?_MaCB=" + MaCB + "&_Info=1",
        type: "GET",
        dataType: "json",
        success: function (result) {
            $("#id-cb span").text(result["MaCBSD"])
            $("#name-cb span").text(result["HoTen"]);
            $("#id-bp span").text(result["VietTat"])
            $("#name-bp span").text(result["TenBP"]);
            $("#image-cb input").attr('src', `data:image/png;base64,${result["HinhAnh"]}`);
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    })
}