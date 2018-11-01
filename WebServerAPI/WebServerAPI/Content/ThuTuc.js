﻿// ============ Thời gian giải quyết thủ tục ============
// Tạo sự kiện khi click vào tab xem thời gian giải quyết thủ tục
$("#menu-xem-thu-tuc").click(function () {
    getBPNameThuTuc();
    createMonthCircleThuTucTH();
    createYearColumnThuTucTH();
})
// ============ Tổng hợp ================================
var clickBPThuTuc = false;
var clickCBThuTuc = false;
var mabp_thutuc = 0;
var macb_thutuc = 0;
// Nút tên bộ phận: sử dụng ajax để lấy dữ liệu tên và mã bộ phận
function getBPNameThuTuc() {
    var str = "";
    $.ajax({
        type: "GET",
        url: url + "/api/BoPhanAPI",
        dataType: "json",
        success: function (data) {
            $.each(data, function (key, val) {
                str += "<div id='" + val.MaBP + "' class='btnBPThuTuc'>" + val.TenBP + "</div>";
            });
            $("#cac-bo-phan-thu-tuc").html(str);
            createButtonBPThuc();
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    });
}
// Nút tên bộ phận: tạo phương thức click
function onClickBtnBPThuTuc(e) {
    $("#thu-tuc-th").hide("slow");
    $("#thu-tuc-bp").show("slow");
    $("#cbx-year-thu-tuc-bp").prop("checked", "checked")
    $("#cbx-month-thu-tuc-bp").removeAttr("checked")
    clickBPThuTuc = false;
    setTimeout(function () {
        mabp_thutuc = $(e.event.target).attr("id");
        $("#header-thu-tuc-bp h1").text($(e.event.target).text());
        getCBNameThuTuc(url + "/api/BoPhanAPI/?_MaBP=" + mabp_thutuc);
        createMonthCircleThuTucBP();
        createYearColumnThuTucBP();
    }, 500);
}
// Nút tên bộ phận: tạo form nút
function createButtonBPThuc() {
    $(".btnBPThuTuc").each(function (index) {
        $(this).kendoButton({
            click: onClickBtnBPThuTuc
        });
    });
}
getBPNameThuTuc();
// Tạo sự kiện nút quay lại
function backBPThuTuc() {
    $("#thu-tuc-th").show("slow");
    $("#thu-tuc-bp").hide("slow");
}
// Tạo biểu đồ miền theo năm/tháng
function createChartThuTucTH(urlStr, titleStr) {
    $("#chart-column-thu-tuc-th").kendoChart({
        dataSource: {
            transport: {
                read: {
                    url: urlStr,
                    dataType: "json"
                }
            },
            sort: {
                field: "ThoiGian",
                //dir: "asc"
            }
        },
        title: {
            text: titleStr
        },
        legend: {
            position: "top"
        },
        seriesDefaults: {
            type: "area"
        },
        series:
            [{
                type: "line",
                field: "SoLuongGiaiQuyet",
                categoryField: "ThoiGian",
                name: "Số lượng",
                color: "#ff1c1c",
                axis: "quantity"
            }, {
                field: "ThoiGianCho",
                categoryField: "ThoiGian",
                name: "Phiên chờ",
                color: "#FFFF00",
            }, {
                field: "ThoiGianGiaiQuyet",
                categoryField: "ThoiGian",
                name: "Phiên giải quyết",
                color: "#33FF00"
            }, {
                field: "TongThoiGian",
                categoryField: "ThoiGian",
                name: "Tổng phiên",
                color: "#0066FF"
            }],
        categoryAxis: {
            labels: {
                rotation: -90
            },
            crosshair: {
                visible: true
            },
            axisCrossingValue: [0, 12],
        },
        valueAxis: [{
            name: "time",
            labels: {
                format: "{0:n0} phút",
            },
            title: {
                text: "Thời gian giải quyết thủ tục (Phút)"
            },
            color: "#007eff"
        }, {
            name: "quantity",
            labels: {
                format: "{0:n0} lần",
            },
            title: {
                text: "Số lượng thủ tục đã giải quyết (Lần)"
            },
            color: "#ff1c1c"
        }],
        tooltip: {
            visible: true,
            shared: true,
            format: "N0"
        },
        dataBound: function (e) {
            var chart = this;
            var categoriesLen = chart.options.categoryAxis.categories.length;
            chart.options.categoryAxis.axisCrossingValue = [0, categoriesLen];
            chart.redraw();
        }
        //render: function (e) {
            // Effective axis range is available in the render event
            //
            // See
            // http://docs.telerik.com/kendo-ui/api/javascript/dataviz/ui/chart/events/render
            // http://docs.telerik.com/kendo-ui/api/javascript/dataviz/ui/chart/methods/getAxis
            // http://docs.telerik.com/kendo-ui/api/javascript/dataviz/chart/chart_axis
        //    var range = e.sender.getAxis("value").range();
        //    if (range > 20) var majorUnit = range.max / 4;
        //    else if (range > 100) var majorUnit = range.max / 20;
        //    else if (range > 500) var majorUnit = range.max / 100;
        //    else if (range > 1000) var majorUnit = range.max / 200;
        //    var axis = e.sender.options.valueAxis;

        //    if (axis.majorUnit !== majorUnit) {
        //        axis.majorUnit = majorUnit;

        //        // We need to redraw the chart to apply the changes
        //        e.sender.redraw();
        //    }
        //}
    });
}
// Tạo dropdownlist chọn năm
function createYearColumnThuTucTH() {
    $.ajax({
        type: "GET",
        url: url + "/api/ValuesAPI",
        dataType: "json",
        success: function (data) {
            if (data.length > 0) {
                $("#body-thu-tuc-th>div:first-child").show();
                $("#footer-thu-tuc-th").show();
                var arr = [];
                var j = 0;
                var arr1 = data[0].split(" ");
                var arr2 = data[1].split(" ");
                for (var i = Number.parseInt(arr1[0]); i <= Number.parseInt(arr2[0]); i++) {
                    arr[j] = { text: i.toString(), value: i.toString() };
                    j++;
                }
                $("#year-column-thu-tuc-th").kendoDropDownList({
                    dataTextField: "text",
                    dataValueField: "value",
                    dataSource: arr
                });
                createChartThuTucTH(url + "/api/ThuTucAPI/?_Loai=nam&_GiaTri=" + $("#year-column-thu-tuc-th").val(), "Thời gian giải quyết thủ tục tổng hợp năm " + $("#year-column-thu-tuc-th").val());
                createGridThuTucTH(url + "/api/ThuTucAPI/?_Loai=nam&_GiaTri=" + $("#year-column-thu-tuc-th").val() + "&_Tong=1", "Bảng thời gian giải quyết thủ tục tổng hợp năm " + $("#year-column-thu-tuc-th").val());
            } else {
                $("#year-column-thu-tuc-th").val(0);
                $("#body-thu-tuc-th>div:first-child").hide();
                $("#footer-thu-tuc-th").hide();
            }
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    })
}
// Tạo sự kiện cho dropdownlist chọn năm
$("#year-column-thu-tuc-th").change(function () {
    if ($("#cbx-year-thu-tuc-th").prop("checked")) {
        createChartThuTucTH(url + "/api/ThuTucAPI/?_Loai=nam&_GiaTri=" + $("#year-column-thu-tuc-th").val(), "Thời gian giải quyết thủ tục tổng hợp năm " + $("#year-column-thu-tuc-th").val());
        createGridThuTucTH(url + "/api/ThuTucAPI/?_Loai=nam&_GiaTri=" + $("#year-column-thu-tuc-th").val() + "&_Tong=1", "Bảng thời gian giải quyết thủ tục năm " + $("#year-column-thu-tuc-th").val());
    }
})
// Tạo thanh chọn thời gian cho biểu đồ tròn
function createMonthCircleThuTucTH() {
    $.ajax({
        type: "GET",
        url: url + "/api/ValuesAPI",
        dataType: "json",
        success: function (data) {
            if (data.length > 0) {
                $("#month-column-thu-tuc-th").kendoDatePicker({
                    start: "year",
                    depth: "year",
                    format: "MM yyyy",
                    dateInput: false,
                    value: new Date(),
                    min: new Date(data[0]),
                    max: new Date(data[1]),
                    //disableDates: ["sa", "su"]
                });
                createChartThuTucTH(url + "/api/ThuTucAPI/?_Loai=thang&_GiaTri=" + $("#month-column-thu-tuc-th").val(), "Thời gian giải quyết thủ tục tổng hợp (" + $("#month-column-thu-tuc-th").val() + ")");
                createGridThuTucTH(url + "/api/ThuTucAPI/?_Loai=thang&_GiaTri=" + $("#month-column-thu-tuc-th").val() + "&_Tong=1", "Bảng thời gian giải quyết thủ tục tổng hợp (" + $("#month-column-thu-tuc-th").val() + ")");
            } else {
                $("#month-column-thu-tuc-th").val("09 2018");
            }
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    })
}
// Tạo sự kiện chọn thời gian
$("#month-column-thu-tuc-th").change(function () {
    if ($("#cbx-month-thu-tuc-th").prop("checked")) {
        createChartThuTucTH(url + "/api/ThuTucAPI/?_Loai=thang&_GiaTri=" + $("#month-column-thu-tuc-th").val(), "Thời gian giải quyết thủ tục tổng hợp (" + $("#month-column-thu-tuc-th").val() + ")");
        createGridThuTucTH(url + "/api/ThuTucAPI/?_Loai=thang&_GiaTri=" + $("#month-column-thu-tuc-th").val() + "&_Tong=1", "Bảng thời gian giải quyết thủ tục tổng hợp (" + $("#month-column-thu-tuc-th").val() + ")");
    }
})
// Tạo sự kiện click checkbox chọn xem biểu đồ miền theo tháng hoặc năm
$("#cbx-month-thu-tuc-th").change(function () {
    if (!$("#cbx-month-thu-tuc-th").prop("checked")) { // Chưa check
        createYearColumnThuTucTH();
        $("#cbx-year-thu-tuc-th").prop("checked", "checked")
    } else { // Đã check
        createMonthCircleThuTucTH();
        $("#cbx-year-thu-tuc-th").removeAttr("checked")
    }
})
$("#cbx-year-thu-tuc-th").change(function () {
    if (!$("#cbx-year-thu-tuc-th").prop("checked")) { // Chưa check
        createMonthCircleThuTucTH();
        $("#cbx-month-thu-tuc-th").prop("checked", "checked")
    } else { // Đã check
        createYearColumnThuTucTH();
        $("#cbx-month-thu-tuc-th").removeAttr("checked")
    }
})
// Phương thức tạo bảng kết quả báo cáo
function createGridThuTucTH(urlStr, titleStr) {
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
                id: "MaSTT",
                fields: {
                    MaSTT: { type: "number" },
                    SoThuTu: { type: "number", validation: { required: true } },
                    TenBP: { type: "string", validation: { required: true } },
                    Ngay: { type: "date", validation: { required: true } },
                    ThoiGianRut: { type: "date", validation: { required: true } },
                    ThoiGianGoi: { type: "date", validation: { required: true } },
                    ThoiGianHoanTat: { type: "date", validation: { required: true } },
                    ThoiGianCho: { type: "number", validation: { required: true } },
                    ThoiGianGiaiQuyet: { type: "number", validation: { required: true } },
                    TongThoiGian: { type: "number", validation: { required: true } }
                }
            }
        },
        group: {
            field: "TenBP", aggregates: [
                { field: "SoThuTu", aggregate: "count" },
                { field: "ThoiGianCho", aggregate: "average" },
                { field: "ThoiGianCho", aggregate: "sum" },
                { field: "ThoiGianCho", aggregate: "max" },
                { field: "ThoiGianCho", aggregate: "min" },
                { field: "ThoiGianGiaiQuyet", aggregate: "average" },
                { field: "ThoiGianGiaiQuyet", aggregate: "sum" },
                { field: "ThoiGianGiaiQuyet", aggregate: "max" },
                { field: "ThoiGianGiaiQuyet", aggregate: "min" },
                { field: "TongThoiGian", aggregate: "average" },
                { field: "TongThoiGian", aggregate: "sum" },
                { field: "TongThoiGian", aggregate: "max" },
                { field: "TongThoiGian", aggregate: "min" },
            ]
        },
        aggregate: [
            { field: "SoThuTu", aggregate: "count" },
            { field: "ThoiGianCho", aggregate: "average" },
            { field: "ThoiGianCho", aggregate: "sum" },
            { field: "ThoiGianCho", aggregate: "max" },
            { field: "ThoiGianCho", aggregate: "min" },
            { field: "ThoiGianGiaiQuyet", aggregate: "average" },
            { field: "ThoiGianGiaiQuyet", aggregate: "sum" },
            { field: "ThoiGianGiaiQuyet", aggregate: "max" },
            { field: "ThoiGianGiaiQuyet", aggregate: "min" },
            { field: "TongThoiGian", aggregate: "average" },
            { field: "TongThoiGian", aggregate: "sum" },
            { field: "TongThoiGian", aggregate: "max" },
            { field: "TongThoiGian", aggregate: "min" },
        ]
    });

    var grid = $("#grid-thu-tuc-th").kendoGrid({
        dataSource: dataSource,
        navigatable: true,
        pageable: true,
        columns: [
            { field: "SoThuTu", title: "Số thứ tự", width: 70, footerTemplate: "Tổng cộng: #=count#", groupFooterTemplate: "Tổng cộng: #=count#" },
            { field: "Ngay", title: "Ngày", width: 70, format: "{0:dd MM yyyy}" },
            { field: "ThoiGianRut", title: "Thời điểm lấy số", width: 120, format: "{0:HH:mm:ss}" },
            { field: "ThoiGianGoi", title: "Thời điểm gọi số", width: 120, format: "{0:HH:mm:ss}" },
            { field: "ThoiGianHoanTat", title: "Thời điểm hoàn tất", width: 80, format: "{0:HH:mm:ss}" },
            { field: "ThoiGianCho", title: "Phiên chờ (Phút)", width: 100, groupFooterTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Lớn nhất: #=max#</div><div>Nhỏ nhất: #=min#</div>", footerTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Lớn nhất: #=max#</div><div>Nhỏ nhất: #=min#</div>" },
            { field: "ThoiGianGiaiQuyet", title: "Phiên giải quyết (Phút)", width: 100, groupFooterTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Lớn nhất: #=max#</div><div>Nhỏ nhất: #=min#</div>", footerTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Lớn nhất: #=max#</div><div>Nhỏ nhất: #=min#</div>" },
            { field: "TongThoiGian", title: "Tổng phiên (Phút)", width: 100, groupFooterTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Lớn nhất: #=max#</div><div>Nhỏ nhất: #=min#</div>", footerTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Lớn nhất: #=max#</div><div>Nhỏ nhất: #=min#</div>" },
            { hidden: true, field: "TenBP", title: "Tên bộ phận", width: 1 },
        ],
    }).data("kendoGrid");
    $("#title-grid-thu-tuc-th").text(titleStr);
    //grid.hideColumn("TenBP");
}

// ============ Bộ phận =================================
// Nút tên cán bộ: sử dụng ajax để lấy dữ liệu tên và mã cán bộ
function getCBNameThuTuc(urlStr) {
    var str = "<table style='width: 98%; margin: 1%'>";
    $.ajax({
        type: "GET",
        url: urlStr,
        dataType: "json",
        success: function (data) {
            if (data.length > 0) {
                $.each(data, function (key, val) {
                    str += "<tr><td><div id='" + val.MaCB + "' class='btnCBThuTuc' style='width: 100%; margin: 2px'><span class=' k-icon k-i-user'></span>" + val.HoTen + "</div></td></tr>";
                });
                str += "</table>";
                $("#cac-can-bo-thu-tuc").html(str);
                createButtonCBThuTuc();
            } else {
                $("#body-thu-tuc-bp").hide();
                $("#footer-thu-tuc-bp").hide();
            }
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    })
}
// Nút tên cán bộ: tạo form nút
function createButtonCBThuTuc() {
    $(".btnCBThuTuc").each(function (index) {
        $(this).kendoButton({
            click: onClickBtnCBThuTuc
        });
    });
}
// Nút tên cán bộ: tạo phương thức click
function onClickBtnCBThuTuc(e) {
    $("#cbx-year-thu-tuc-cb").prop("checked", "checked")
    $("#cbx-month-thu-tuc-cb").removeAttr("checked")
    clickCBThuTuc = false;
    macb_thutuc = $(e.event.target).attr("id");
    createInfoThuTucCB(macb_thutuc);
    $("#thu-tuc-bp").hide("slow");
    $("#thu-tuc-cb").show("slow");
    //createInfoCB(macb);
    setTimeout(function () {
        $("#header-thu-tuc-cb h1").text("Cán bộ: " + $(e.event.target).text());
        createMonthCircleThuTucCB();
        createYearColumnThuTucCB();
    }, 800);
}
// Tạo sự kiện nút quay lại
function backCBThuTuc() {
    $("#thu-tuc-bp").show("slow");
    $("#thu-tuc-cb").hide("slow");
}
// Tạo biểu đồ miền theo năm/tháng
function createChartThuTucBP(urlStr, titleStr) {
    $("#chart-column-thu-tuc-bp").kendoChart({
        dataSource: {
            transport: {
                read: {
                    url: urlStr,
                    dataType: "json"
                }
            },
            sort: {
                field: "ThoiGian",
                //dir: "asc"
            }
        },
        title: {
            text: titleStr
        },
        legend: {
            position: "top"
        },
        seriesDefaults: {
            type: "area"
        },
        series:
            [{
                type: "line",
                field: "SoLuongGiaiQuyet",
                categoryField: "ThoiGian",
                name: "Số lượng",
                color: "#ff1c1c",
                axis: "quantity"
            },{
                field: "ThoiGianCho",
                categoryField: "ThoiGian",
                name: "Phiên chờ",
                color: "#FFFF00",
            }, {
                field: "ThoiGianGiaiQuyet",
                categoryField: "ThoiGian",
                name: "Phiên giải quyết",
                color: "#33FF00"
            }, {
                field: "TongThoiGian",
                categoryField: "ThoiGian",
                name: "Tổng phiên",
                color: "#0066FF"
            }],
        categoryAxis: {
            labels: {
                rotation: -90
            },
            crosshair: {
                visible: true
            },
            axisCrossingValue: [0, 12],
        },
        valueAxis: [{
            name: "time",
            labels: {
                format: "{0:n0} phút",
            },
            title: {
                text: "Thời gian giải quyết thủ tục (Phút)"
            },
            color: "#007eff"
        }, {
            name: "quantity",
            labels: {
                format: "{0:n0} lần",
            },
            title: {
                text: "Số lượng thủ tục đã giải quyết (Lần)"
            },
            color: "#ff1c1c"
        }],
        tooltip: {
            visible: true,
            shared: true,
            format: "N0"
        },
        dataBound: function (e) {
            var chart = this;
            var categoriesLen = chart.options.categoryAxis.categories.length;
            chart.options.categoryAxis.axisCrossingValue = [0, categoriesLen];
            chart.redraw();
        }
        //render: function (e) {
        //    // Effective axis range is available in the render event
        //    //
        //    // See
        //    // http://docs.telerik.com/kendo-ui/api/javascript/dataviz/ui/chart/events/render
        //    // http://docs.telerik.com/kendo-ui/api/javascript/dataviz/ui/chart/methods/getAxis
        //    // http://docs.telerik.com/kendo-ui/api/javascript/dataviz/chart/chart_axis
        //    var range = e.sender.getAxis("value").range();
        //    if (range > 20) var majorUnit = range.max / 4;
        //    else if (range > 100) var majorUnit = range.max / 20;
        //    else if (range > 500) var majorUnit = range.max / 100;
        //    else if (range > 1000) var majorUnit = range.max / 200;
        //    var axis = e.sender.options.valueAxis;

        //    if (axis.majorUnit !== majorUnit) {
        //        axis.majorUnit = majorUnit;

        //        // We need to redraw the chart to apply the changes
        //        e.sender.redraw();
        //    }
        //}
    });
}
// Tạo dropdownlist chọn năm
function createYearColumnThuTucBP() {
    $.ajax({
        type: "GET",
        url: url + "/api/ValuesAPI/?_MaBP=" + mabp_thutuc,
        dataType: "json",
        success: function (data) {
            if (data.length > 0) {
                $("#body-thu-tuc-bp>div:first-child").show();
                $("#footer-thu-tuc-bp").show();
                var arr = [];
                var j = 0;
                var arr1 = data[0].split(" ");
                var arr2 = data[1].split(" ");
                for (var i = Number.parseInt(arr1[0]); i <= Number.parseInt(arr2[0]); i++) {
                    arr[j] = { text: i.toString(), value: i.toString() };
                    j++;
                }
                $("#year-column-thu-tuc-bp").kendoDropDownList({
                    dataTextField: "text",
                    dataValueField: "value",
                    dataSource: arr
                });
                createChartThuTucBP(url + "/api/ThuTucAPI/?_Loai=nam&_GiaTri=" + $("#year-column-thu-tuc-bp").val() + "&_MaBP=" + mabp_thutuc, "Thời gian giải quyết thủ tục năm " + $("#year-column-thu-tuc-bp").val());
                createGridThuTucBP(url + "/api/ThuTucAPI/?_Loai=nam&_GiaTri=" + $("#year-column-thu-tuc-bp").val() + "&_MaBP=" + mabp_thutuc + "&_Tong=1", "Bảng thời gian giải quyết thủ tục năm " + $("#year-column-thu-tuc-bp").val());
            } else {
                $("#year-column-thu-tuc-bp").val(0);
                $("#body-thu-tuc-bp>div:first-child").hide();
                $("#footer-thu-tuc-bp").hide();
            }
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    })
}
// Tạo sự kiện cho dropdownlist chọn năm
$("#year-column-thu-tuc-bp").change(function () {
    if ($("#cbx-year-thu-tuc-bp").prop("checked")) {
        createChartThuTucBP(url + "/api/ThuTucAPI/?_Loai=nam&_GiaTri=" + $("#year-column-thu-tuc-bp").val() + "&_MaBP=" + mabp_thutuc, "Thời gian giải quyết thủ tục năm " + $("#year-column-thu-tuc-bp").val());
        createGridThuTucBP(url + "/api/ThuTucAPI/?_Loai=nam&_GiaTri=" + $("#year-column-thu-tuc-bp").val() + "&_MaBP=" + mabp_thutuc + "&_Tong=1", "Bảng thời gian giải quyết thủ tục năm " + $("#year-column-thu-tuc-bp").val());
    }
})
// Tạo thanh chọn thời gian cho biểu đồ tròn
function createMonthCircleThuTucBP() {
    $.ajax({
        type: "GET",
        url: url + "/api/ValuesAPI/?_MaBP=" + mabp_thutuc,
        dataType: "json",
        success: function (data) {
            if (data.length > 0) {
                $("#month-column-thu-tuc-bp").kendoDatePicker({
                    start: "year",
                    depth: "year",
                    format: "MM yyyy",
                    dateInput: false,
                    value: new Date(),
                    min: new Date(data[0]),
                    max: new Date(data[1]),
                    //disableDates: ["sa", "su"]
                });
                createChartThuTucBP(url + "/api/ThuTucAPI/?_Loai=thang&_GiaTri=" + $("#month-column-thu-tuc-bp").val() + "&_MaBP=" + mabp_thutuc, "Thời gian giải quyết thủ tục (" + $("#month-column-thu-tuc-bp").val() + ")");
                createGridThuTucBP(url + "/api/ThuTucAPI/?_Loai=thang&_GiaTri=" + $("#month-column-thu-tuc-bp").val() + "&_MaBP=" + mabp_thutuc + "&_Tong=1", "Bảng thời gian giải quyết thủ tục (" + $("#month-column-thu-tuc-bp").val() + ")");
            } else {
                $("#month-column-thu-tuc-bp").val("09 2018");
            }
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    })
}
// Tạo sự kiện chọn thời gian
$("#month-column-thu-tuc-bp").change(function () {
    if ($("#cbx-month-thu-tuc-bp").prop("checked")) {
        createChartThuTucBP(url + "/api/ThuTucAPI/?_Loai=thang&_GiaTri=" + $("#month-column-thu-tuc-bp").val() + "&_MaBP=" + mabp_thutuc, "Thời gian giải quyết thủ tục (" + $("#month-column-thu-tuc-bp").val() + ")");
        createGridThuTucBP(url + "/api/ThuTucAPI/?_Loai=thang&_GiaTri=" + $("#month-column-thu-tuc-bp").val() + "&_MaBP=" + mabp_thutuc + "&_Tong=1", "Bảng thời gian giải quyết thủ tục (" + $("#month-column-thu-tuc-bp").val() + ")");
    }
})
// Tạo sự kiện click checkbox chọn xem biểu đồ miền theo tháng hoặc năm
$("#cbx-month-thu-tuc-bp").change(function () {
    if (!$("#cbx-month-thu-tuc-bp").prop("checked")) { // Chưa check
        createYearColumnThuTucBP();
        $("#cbx-year-thu-tuc-bp").prop("checked", "checked")
    } else { // Đã check
        createMonthCircleThuTucBP();
        $("#cbx-year-thu-tuc-bp").removeAttr("checked")
    }
})
$("#cbx-year-thu-tuc-bp").change(function () {
    if (!$("#cbx-year-thu-tuc-bp").prop("checked")) { // Chưa check
        createMonthCircleThuTucBP();
        $("#cbx-month-thu-tuc-bp").prop("checked", "checked")
    } else { // Đã check
        createYearColumnThuTucBP();
        $("#cbx-month-thu-tuc-bp").removeAttr("checked")
    }
})
// Phương thức tạo bảng kết quả báo cáo
function createGridThuTucBP(urlStr, titleStr) {
    $("#title-grid-thu-tuc-bp").text(titleStr);
    dataSource = new kendo.data.DataSource({
        transport: {
            serverFiltering: true,
            read: {
                url: urlStr,
                dataType: "json"
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
                id: "MaSTT",
                fields: {
                    MaSTT: { type: "number" },
                    SoThuTu: { type: "number", validation: { required: true } },
                    HoTen: { type: "string", validation: { required: true } },
                    Ngay: { type: "date", validation: { required: true }, },
                    ThoiGianRut: { type: "date", validation: { required: true } },
                    ThoiGianGoi: { type: "date", validation: { required: true } },
                    ThoiGianHoanTat: { type: "date", validation: { required: true } },
                    ThoiGianCho: { type: "number", validation: { required: true } },
                    ThoiGianGiaiQuyet: { type: "number", validation: { required: true } },
                    TongThoiGian: { type: "number", validation: { required: true } }
                }
            }
        },
        group: {
            field: "HoTen", aggregates: [
                { field: "SoThuTu", aggregate: "count" },
                { field: "ThoiGianCho", aggregate: "average" },
                { field: "ThoiGianCho", aggregate: "sum" },
                { field: "ThoiGianCho", aggregate: "min" },
                { field: "ThoiGianCho", aggregate: "max" },
                { field: "ThoiGianGiaiQuyet", aggregate: "average" },
                { field: "ThoiGianGiaiQuyet", aggregate: "sum" },
                { field: "ThoiGianGiaiQuyet", aggregate: "min" },
                { field: "ThoiGianGiaiQuyet", aggregate: "max" },
                { field: "TongThoiGian", aggregate: "average" },
                { field: "TongThoiGian", aggregate: "sum" },
                { field: "TongThoiGian", aggregate: "min" },
                { field: "TongThoiGian", aggregate: "max" },
            ]
        },

        aggregate: [
            { field: "SoThuTu", aggregate: "count" },
            { field: "ThoiGianCho", aggregate: "average" },
            { field: "ThoiGianCho", aggregate: "sum" },
            { field: "ThoiGianCho", aggregate: "min" },
            { field: "ThoiGianCho", aggregate: "max" },
            { field: "ThoiGianGiaiQuyet", aggregate: "average" },
            { field: "ThoiGianGiaiQuyet", aggregate: "sum" },
            { field: "ThoiGianGiaiQuyet", aggregate: "min" },
            { field: "ThoiGianGiaiQuyet", aggregate: "max" },
            { field: "TongThoiGian", aggregate: "average" },
            { field: "TongThoiGian", aggregate: "sum" },
            { field: "TongThoiGian", aggregate: "min" },
            { field: "TongThoiGian", aggregate: "max" },
        ]

    });

    var grid = $("#grid-thu-tuc-bp").kendoGrid({
        dataSource: dataSource,
        navigatable: true,
        pageable: true,
        columns: [
            { field: "SoThuTu", title: "Số thứ tự", width: 60, footerTemplate: "Tổng cộng: #=count#", groupFooterTemplate: "Tổng cộng: #=count#" },
            { field: "Ngay", title: "Ngày", width: 50, format: "{0:dd MM yyyy}" },
            { field: "ThoiGianRut", title: "Thời điểm lấy số", width: 80, format: "{0:HH:mm:ss}" },
            { field: "ThoiGianGoi", title: "Thời điểm gọi số", width: 80, format: "{0:HH:mm:ss}" },
            { field: "ThoiGianHoanTat", title: "Thời điểm hoàn tất", width: 80, format: "{0:HH:mm:ss}" },
            { field: "ThoiGianCho", title: "Phiên chờ (Phút)", width: 100, groupFooterTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Lớn nhất: #=max#</div><div>Nhỏ nhất: #=min#</div>", footerTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Lớn nhất: #=max#</div><div>Nhỏ nhất: #=min#</div>" },
            { field: "ThoiGianGiaiQuyet", title: "Phiên chờ (Phút)", width: 100, groupFooterTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Lớn nhất: #=max#</div><div>Nhỏ nhất: #=min#</div>", footerTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Lớn nhất: #=max#</div><div>Nhỏ nhất: #=min#</div>" },
            { field: "TongThoiGian", title: "Phiên chờ (Phút)", width: 100, groupFooterTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Lớn nhất: #=max#</div><div>Nhỏ nhất: #=min#</div>", footerTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Lớn nhất: #=max#</div><div>Nhỏ nhất: #=min#</div>" },
            { hidden: true, field: "HoTen", title: "Họ tên", width: 1 },
        ],
    }).data("kendoGrid");
    //grid.hideColumn("HoTen");
}

// ============ Cán bộ =================================
// Tạo thông tin cán bộ
function createInfoThuTucCB(MaCB) {
    //table - core - cb
    $.ajax({
        url: url + "/api/ValuesAPI/?_MaCB=" + MaCB + "&_Info=1",
        type: "GET",
        dataType: "json",
        success: function (result) {
            $("#id-thu-tuc-cb span").text(result["MaCBSD"]);
            $("#name-thu-tuc-cb span").text(result["HoTen"]);
            $("#id-thu-tuc-bp span").text(result["VietTat"]);
            $("#name-thu-tuc-bp span").text(result["TenBP"]);
            $("#image-thu-tuc-cb input").attr('src', `data:image/png;base64,${result["HinhAnh"]}`);
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    })
}
// Tạo biểu đồ miền theo năm/tháng
function createChartThuTucCB(urlStr, titleStr) {
    $("#chart-column-thu-tuc-cb").kendoChart({
        dataSource: {
            transport: {
                read: {
                    url: urlStr,
                    dataType: "json"
                }
            },
            sort: {
                field: "ThoiGian",
                //dir: "asc"
            }
        },
        title: {
            text: titleStr
        },
        legend: {
            position: "top"
        },
        seriesDefaults: {
            type: "area"
        },
        series:
            [{
                type: "line",
                field: "SoLuongGiaiQuyet",
                categoryField: "ThoiGian",
                name: "Số lượng",
                color: "#ff1c1c",
                axis: "quantity"
            }, {
                field: "ThoiGianGiaiQuyet",
                categoryField: "ThoiGian",
                name: "Phiên giải quyết",
                color: "#007eff",
                axis: "time"
            },
                //{
                //field: "TongThoiGian",
                //categoryField: "ThoiGian",
                //name: "Tổng phiên",
                //color: "#0066FF"
                //}
            ],
        categoryAxis: {
            labels: {
                rotation: -90
            },
            crosshair: {
                visible: true
            },
            axisCrossingValue: [0, 12],
        },
        valueAxis: [{
            name: "time",
            labels: {
                format: "{0:n0} phút",
            },
            title: {
                text: "Thời gian giải quyết thủ tục (Phút)"
            },
            color: "#007eff"
        }, {
            name: "quantity",
            labels: {
                format: "{0:n0} lần",
            },
            title: {
                text: "Số lượng thủ tục đã giải quyết (Lần)"
            },
            color: "#ff1c1c"
        }],
        tooltip: {
            visible: true,
            shared: true,
            format: "N0"
        },
        dataBound: function (e) {
            var chart = this;
            var categoriesLen = chart.options.categoryAxis.categories.length;
            chart.options.categoryAxis.axisCrossingValue = [0, categoriesLen];
            chart.redraw();
        }
        //render: function (e) {
        //    // Effective axis range is available in the render event
        //    //
        //    // See
        //    // http://docs.telerik.com/kendo-ui/api/javascript/dataviz/ui/chart/events/render
        //    // http://docs.telerik.com/kendo-ui/api/javascript/dataviz/ui/chart/methods/getAxis
        //    // http://docs.telerik.com/kendo-ui/api/javascript/dataviz/chart/chart_axis
        //    var range = e.sender.getAxis("value").range();
        //    if (range > 20) var majorUnit = range.max / 4;
        //    else if (range > 100) var majorUnit = range.max / 20;
        //    else if (range > 500) var majorUnit = range.max / 100;
        //    else if (range > 1000) var majorUnit = range.max / 200;
        //    var axis = e.sender.options.valueAxis;

        //    if (axis.majorUnit !== majorUnit) {
        //        axis.majorUnit = majorUnit;

        //        // We need to redraw the chart to apply the changes
        //        e.sender.redraw();
        //    }
        //}
    });
}
// Tạo dropdownlist chọn năm
function createYearColumnThuTucCB() {
    $.ajax({
        type: "GET",
        url: url + "/api/ValuesAPI/?_MaCB=" + macb_thutuc,
        dataType: "json",
        success: function (data) {
            if (data.length > 0) {
                $("#body-thu-tuc-cb>div:first-child").show();
                $("#footer-thu-tuc-cb").show();
                var arr = [];
                var j = 0;
                var arr1 = data[0].split(" ");
                var arr2 = data[1].split(" ");
                for (var i = Number.parseInt(arr1[0]); i <= Number.parseInt(arr2[0]); i++) {
                    arr[j] = { text: i.toString(), value: i.toString() };
                    j++;
                }
                $("#year-column-thu-tuc-cb").kendoDropDownList({
                    dataTextField: "text",
                    dataValueField: "value",
                    dataSource: arr
                });
                createChartThuTucCB(url + "/api/ThuTucAPI/?_Loai=nam&_GiaTri=" + $("#year-column-thu-tuc-bp").val() + "&_MaCB=" + macb_thutuc, "Thời gian giải quyết thủ tục năm " + $("#year-column-thu-tuc-cb").val());
                createGridThuTucCB(url + "/api/ThuTucAPI/?_Loai=nam&_GiaTri=" + $("#year-column-thu-tuc-bp").val() + "&_MaCB=" + macb_thutuc + "&_Tong=1", "Bảng thời gian giải quyết thủ tục năm " + $("#year-column-thu-tuc-cb").val());
            } else {
                $("#year-column-thu-tuc-cb").val(0);
                $("#body-thu-tuc-cb>div:first-child").hide();
                $("#footer-thu-tuc-cb").hide();
            }
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    })
}
// Tạo sự kiện cho dropdownlist chọn năm
$("#year-column-thu-tuc-cb").change(function () {
    if ($("#cbx-year-thu-tuc-cb").prop("checked")) {
        createChartThuTucCB(url + "/api/ThuTucAPI/?_Loai=nam&_GiaTri=" + $("#year-column-thu-tuc-cb").val() + "&_MaCB=" + macb_thutuc, "Thời gian giải quyết thủ tục năm " + $("#year-column-thu-tuc-cb").val());
        createGridThuTucCB(url + "/api/ThuTucAPI/?_Loai=nam&_GiaTri=" + $("#year-column-thu-tuc-cb").val() + "&_MaCB=" + macb_thutuc + "&_Tong=1", "Bảng thời gian giải quyết thủ tục năm " + $("#year-column-thu-tuc-cb").val());
    }
})
// Tạo thanh chọn thời gian cho biểu đồ tròn
function createMonthCircleThuTucCB() {
    $.ajax({
        type: "GET",
        url: url + "/api/ValuesAPI/?_MaCB=" + macb_thutuc,
        dataType: "json",
        success: function (data) {
            if (data.length > 0) {
                $("#month-column-thu-tuc-cb").kendoDatePicker({
                    start: "year",
                    depth: "year",
                    format: "MM yyyy",
                    dateInput: false,
                    value: new Date(),
                    min: new Date(data[0]),
                    max: new Date(data[1]),
                    //disableDates: ["sa", "su"]
                });
                createChartThuTucCB(url + "/api/ThuTucAPI/?_Loai=thang&_GiaTri=" + $("#month-column-thu-tuc-cb").val() + "&_MaCB=" + macb_thutuc, "Thời gian giải quyết thủ tục (" + $("#month-column-thu-tuc-cb").val() + ")");
                createGridThuTucCB(url + "/api/ThuTucAPI/?_Loai=thang&_GiaTri=" + $("#month-column-thu-tuc-cb").val() + "&_MaCB=" + macb_thutuc + "&_Tong=1", "Bảng thời gian giải quyết thủ tục (" + $("#month-column-thu-tuc-cb").val() + ")");
            } else {
                $("#month-column-thu-tuc-cb").val("09 2018");
            }
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    })
}
// Tạo sự kiện chọn thời gian
$("#month-column-thu-tuc-cb").change(function () {
    if ($("#cbx-month-thu-tuc-cb").prop("checked")) {
        createChartThuTucCB(url + "/api/ThuTucAPI/?_Loai=thang&_GiaTri=" + $("#month-column-thu-tuc-cb").val() + "&_MaCB=" + macb_thutuc, "Thời gian giải quyết thủ tục (" + $("#month-column-thu-tuc-cb").val() + ")");
        createGridThuTucCB(url + "/api/ThuTucAPI/?_Loai=thang&_GiaTri=" + $("#month-column-thu-tuc-cb").val() + "&_MaCB=" + macb_thutuc + "&_Tong=1", "Bảng thời gian giải quyết thủ tục (" + $("#month-column-thu-tuc-cb").val() + ")");
    }
})
// Tạo sự kiện click checkbox chọn xem biểu đồ miền theo tháng hoặc năm
$("#cbx-month-thu-tuc-cb").change(function () {
    if (!$("#cbx-month-thu-tuc-cb").prop("checked")) { // Chưa check
        createYearColumnThuTucCB();
        $("#cbx-year-thu-tuc-cb").prop("checked", "checked")
    } else { // Đã check
        createMonthCircleThuTucCB();
        $("#cbx-year-thu-tuc-cb").removeAttr("checked")
    }
})
$("#cbx-year-thu-tuc-cb").change(function () {
    if (!$("#cbx-year-thu-tuc-cb").prop("checked")) { // Chưa check
        createMonthCircleThuTucCB();
        $("#cbx-month-thu-tuc-cb").prop("checked", "checked")
    } else { // Đã check
        createYearColumnThuTucCB();
        $("#cbx-month-thu-tuc-cb").removeAttr("checked")
    }
})
// Phương thức tạo bảng kết quả báo cáo
function createGridThuTucCB(urlStr, titleStr) {
    $("#title-grid-thu-tuc-cb").text(titleStr);
    dataSource = new kendo.data.DataSource({
        transport: {
            serverFiltering: true,
            read: {
                url: urlStr,
                dataType: "json"
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
                id: "MaSTT",
                fields: {
                    MaSTT: { type: "number" },
                    SoThuTu: { type: "number", validation: { required: true } },
                    HoTen: { type: "string", validation: { required: true } },
                    Ngay: { type: "date", validation: { required: true }, },
                    ThoiGianRut: { type: "date", validation: { required: true } },
                    ThoiGianGoi: { type: "date", validation: { required: true } },
                    ThoiGianHoanTat: { type: "date", validation: { required: true } },
                    ThoiGianCho: { type: "number", validation: { required: true } },
                    ThoiGianGiaiQuyet: { type: "number", validation: { required: true } },
                    TongThoiGian: { type: "number", validation: { required: true } }
                }
            }
        },
        group: {
            field: "Ngay", aggregates: [
                { field: "SoThuTu", aggregate: "count" },
                { field: "ThoiGianCho", aggregate: "average" },
                { field: "ThoiGianCho", aggregate: "sum" },
                { field: "ThoiGianCho", aggregate: "min" },
                { field: "ThoiGianCho", aggregate: "max" },
                { field: "ThoiGianGiaiQuyet", aggregate: "average" },
                { field: "ThoiGianGiaiQuyet", aggregate: "sum" },
                { field: "ThoiGianGiaiQuyet", aggregate: "min" },
                { field: "ThoiGianGiaiQuyet", aggregate: "max" },
                { field: "TongThoiGian", aggregate: "average" },
                { field: "TongThoiGian", aggregate: "sum" },
                { field: "TongThoiGian", aggregate: "min" },
                { field: "TongThoiGian", aggregate: "max" },
            ]
        },

        aggregate: [
            { field: "SoThuTu", aggregate: "count" },
            { field: "ThoiGianCho", aggregate: "average" },
            { field: "ThoiGianCho", aggregate: "sum" },
            { field: "ThoiGianCho", aggregate: "min" },
            { field: "ThoiGianCho", aggregate: "max" },
            { field: "ThoiGianGiaiQuyet", aggregate: "average" },
            { field: "ThoiGianGiaiQuyet", aggregate: "sum" },
            { field: "ThoiGianGiaiQuyet", aggregate: "min" },
            { field: "ThoiGianGiaiQuyet", aggregate: "max" },
            { field: "TongThoiGian", aggregate: "average" },
            { field: "TongThoiGian", aggregate: "sum" },
            { field: "TongThoiGian", aggregate: "min" },
            { field: "TongThoiGian", aggregate: "max" },
        ]

    });

    var grid = $("#grid-thu-tuc-cb").kendoGrid({
        dataSource: dataSource,
        navigatable: true,
        pageable: true,
        columns: [
            //{ field: "HoTen", title: "Họ tên", width: 100 },
            { field: "SoThuTu", title: "Số thứ tự", width: 60, footerTemplate: "Tổng cộng: #=count#", groupFooterTemplate: "Tổng cộng: #=count#" },
            { field: "ThoiGianRut", title: "Thời điểm lấy số", width: 80, format: "{0:HH:mm:ss}" },
            { field: "ThoiGianGoi", title: "Thời điểm gọi số", width: 80, format: "{0:HH:mm:ss}" },
            { field: "ThoiGianHoanTat", title: "Thời điểm hoàn tất", width: 80, format: "{0:HH:mm:ss}" },
            { field: "ThoiGianCho", title: "Phiên chờ (Phút)", width: 100, groupFooterTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Lớn nhất: #=max#</div><div>Nhỏ nhất: #=min#</div>", footerTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Lớn nhất: #=max#</div><div>Nhỏ nhất: #=min#</div>" },
            { field: "ThoiGianGiaiQuyet", title: "Phiên giải quyết (Phút)", width: 100, groupFooterTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Lớn nhất: #=max#</div><div>Nhỏ nhất: #=min#</div>", footerTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Lớn nhất: #=max#</div><div>Nhỏ nhất: #=min#</div>" },
            { field: "TongThoiGian", title: "Tổng phiên (Phút)", width: 100, groupFooterTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Lớn nhất: #=max#</div><div>Nhỏ nhất: #=min#</div>", footerTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Lớn nhất: #=max#</div><div>Nhỏ nhất: #=min#</div>" },
            { hidden: true, field: "Ngay", title: "Ngày", width: 1, format: "{0:dd MM yyyy}" },
        ],
    }).data("kendoGrid");
    //grid.hideColumn("Ngay");
}
