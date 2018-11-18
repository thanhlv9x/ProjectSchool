// ============ Thời gian giải quyết thủ tục ============
// Tạo sự kiện khi click vào tab xem thời gian giải quyết thủ tục
$("li#menu-xem-thu-tuc").click(function () {
    getBPNameThuTuc();
    createMonthCircleThuTucTH();
    createYearColumnThuTucTH();
    $("#cbx-month-thu-tuc-th").prop("checked", false);
    $("#cbx-year-thu-tuc-th").prop("checked", true);
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
        plotArea: {
            background: "blue",
            opacity: 0.1
        },
        series:
            [{
                opacity: 0.2,
                type: "column",
                field: "SoLuongGiaiQuyet",
                categoryField: "ThoiGian",
                name: "Tổng số lượng",
                color: "#ff1c1c",
                axis: "quantity",
                tooltip: {
                    visible: true,
                    template: "#: value # lần"
                }
            }, {
                opacity: 1,
                type: "line",
                field: "TongThoiGian",
                categoryField: "ThoiGian",
                name: "Tổng thời gian trung bình",
                color: "#0066FF",
                tooltip: {
                    visible: true,
                    template: "#: value # phút"
                }
            }, {
                opacity: 1,
                type: "line",
                field: "ThoiGianCho",
                categoryField: "ThoiGian",
                name: "Thời gian chờ trung bình",
                color: "#FFFF00",
                tooltip: {
                    visible: true,
                    template: "#: value # phút"
                }
            }, {
                opacity: 1,
                type: "line",
                field: "ThoiGianGiaiQuyet",
                categoryField: "ThoiGian",
                name: "Thời gian giải quyết trung bình",
                color: "#33FF00",
                tooltip: {
                    visible: true,
                    template: "#: value # phút"
                }
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
                createGridThuTucTH(url + "/api/ThuTucAPI/?_Loai=nam&_GiaTri=" + $("#year-column-thu-tuc-th").val() + "&_BP=1", "Bảng thời gian giải quyết thủ tục tổng hợp năm " + $("#year-column-thu-tuc-th").val());
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
        createGridThuTucTH(url + "/api/ThuTucAPI/?_Loai=nam&_GiaTri=" + $("#year-column-thu-tuc-th").val() + "&_BP=1", "Bảng thời gian giải quyết thủ tục năm " + $("#year-column-thu-tuc-th").val());
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
                createGridThuTucTH(url + "/api/ThuTucAPI/?_Loai=thang&_GiaTri=" + $("#month-column-thu-tuc-th").val() + "&_BP=1", "Bảng thời gian giải quyết thủ tục tổng hợp (" + $("#month-column-thu-tuc-th").val() + ")");
            } else {
                $("#month-column-thu-tuc-th").val("09 2018");
            }
            if (!$("#cbx-month-thu-tuc-th").prop("checked")) {
                $("#year-group-thu-tuc-th-month span.k-widget.k-datepicker.k-header").hide();
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
        createGridThuTucTH(url + "/api/ThuTucAPI/?_Loai=thang&_GiaTri=" + $("#month-column-thu-tuc-th").val() + "&_BP=1", "Bảng thời gian giải quyết thủ tục tổng hợp (" + $("#month-column-thu-tuc-th").val() + ")");
    }
})
// Tạo sự kiện click checkbox chọn xem biểu đồ miền theo tháng hoặc năm
function hideShowMonth() {
    if (!$("#cbx-month-thu-tuc-th").prop("checked")) { // Nếu checkbox chọn tháng chưa check
        createYearColumnThuTucTH(); // Tạo biểu đồ cột theo 12 tháng trong năm
        $("#cbx-year-thu-tuc-th").prop("checked", "checked") // Check checkbox năm
        $("#year-group-thu-tuc-th-month span.k-widget.k-datepicker.k-header").hide(); // Ẩn datepicker của chọn tháng
        $("#year-group-thu-tuc-th-year span.k-widget.k-dropdown.k-header").show(); // Hiện dropdownlist của chọn năm
    } else { // Nếu checkbox đã check
        createMonthCircleThuTucTH(); // Tạo biểu đò cột theo các ngày trong tháng
        $("#cbx-year-thu-tuc-th").removeAttr("checked") // Bỏ check checkbox năm
        $("#year-group-thu-tuc-th-month span.k-widget.k-datepicker.k-header").show(); // Hiện datepicker của chọn tháng
        $("#year-group-thu-tuc-th-year span.k-widget.k-dropdown.k-header").hide(); // Ẩn dropdownlist của chọn năm
    }
}
function hideShowYear() {
    if (!$("#cbx-year-thu-tuc-th").prop("checked")) { // Nếu checbox chọn năm chưa check
        createMonthCircleThuTucTH(); // Tạo biểu đồ cột theo các ngày trong tháng
        $("#cbx-month-thu-tuc-th").prop("checked", "checked")  // Check checkbox chọn tháng
        $("#year-group-thu-tuc-th-month span.k-widget.k-datepicker.k-header").show(); // Hiện datepicker của chọn tháng
        $("#year-group-thu-tuc-th-year span.k-widget.k-dropdown.k-header").hide(); // Ẩn Dropdownlist của chọn năm
    } else { // Nếu checbox chọn năm đã check
        createYearColumnThuTucTH(); // Tạo biểu đồ cột theo 12 tháng trong năm
        $("#cbx-month-thu-tuc-th").removeAttr("checked") // Bỏ check checkbox tháng
        $("#year-group-thu-tuc-th-month span.k-widget.k-datepicker.k-header").hide(); // Ẩn datepicker của chọn tháng
        $("#year-group-thu-tuc-th-year span.k-widget.k-dropdown.k-header").show(); // Hiện Dropdownlist của chọn năm
    }
}
$("#year-group-thu-tuc-th div:first-child label").click(function () {
    $("#cbx-month-thu-tuc-th").prop("checked", !$("#cbx-month-thu-tuc-th").prop("checked"));
    hideShowMonth();
})
$("#year-group-thu-tuc-th div:last-child label").click(function () {
    $("#cbx-year-thu-tuc-th").prop("checked", !$("#cbx-year-thu-tuc-th").prop("checked"));
    hideShowYear();
})
$("#cbx-month-thu-tuc-th").change(function () {
    hideShowMonth();
})
$("#cbx-year-thu-tuc-th").change(function () {
    hideShowYear();
})
// Phương thức tạo bảng kết quả báo cáo
function createGridThuTucTH(urlStr, titleStr) {
    $("#grid-thu-tuc-th").html("");
    dataSource = new kendo.data.DataSource({
        transport: {
            serverFiltering: true,
            read: function (options) {
                $.ajax({
                    type: "GET",
                    url: urlStr,
                    dataType: 'json',
                    //async: false,
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
                id: "MaBP",
                fields: {
                    MaBP: { type: "number" },
                    TenBP: { type: "string", validation: { required: true } },
                    VietTat: { type: "string", validation: { required: true } },
                    ThoiGianCho: { type: "number", validation: { required: true } },
                    ThoiGianGiaiQuyet: { type: "number", validation: { required: true } },
                    TongThoiGian: { type: "number", validation: { required: true } },
                    SoLuongGiaiQuyet: { type: "number", validation: { required: true } },
                }
            }
        },
        aggregate: [
            { field: "VietTat", aggregate: "count" },
            { field: "SoLuongGiaiQuyet", aggregate: "average" },
            { field: "SoLuongGiaiQuyet", aggregate: "sum" },
            { field: "SoLuongGiaiQuyet", aggregate: "min" },
            { field: "SoLuongGiaiQuyet", aggregate: "max" },
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
        pageable: {
            refresh: true,
            messages: {
                display: "{0}-{1}/{2}",
                empty: "Dữ liệu không tồn tại",
            }
        },
        columns: [
            { field: "VietTat", title: "Mã bộ phận", width: 1, footerTemplate: "Tổng cộng: #=count#" },
            { field: "TenBP", title: "Tên bộ phận", width: 2 },
            {
                field: "ThoiGianCho", title: "Thời gian chờ trung bình (Phút)", width: 2,
                footerTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Lớn nhất: #=max#</div><div>Nhỏ nhất: #=min#</div>"
            },
            {
                field: "ThoiGianGiaiQuyet", title: "Thời gian giải quyết trung bình (Phút)", width: 2,
                footerTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Lớn nhất: #=max#</div><div>Nhỏ nhất: #=min#</div>"
            },
            {
                field: "TongThoiGian", title: "Tổng thời gian trung bình (Phút)", width: 2,
                footerTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Lớn nhất: #=max#</div><div>Nhỏ nhất: #=min#</div>"
            },
            {
                field: "SoLuongGiaiQuyet", title: "Tổng số lượng (Lần)", width: 2,
                footerTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Lớn nhất: #=max#</div><div>Nhỏ nhất: #=min#</div>"
            }
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
        plotArea: {
            background: "blue",
            opacity: 0.1
        },
        series:
            [{
                opacity: 0.2,
                type: "column",
                field: "SoLuongGiaiQuyet",
                categoryField: "ThoiGian",
                name: "Tổng số lượng",
                color: "#ff1c1c",
                axis: "quantity",
                tooltip: {
                    visible: true,
                    template: "#: value # lần"
                }
            }, {
                opacity: 1,
                type: "line",
                field: "TongThoiGian",
                categoryField: "ThoiGian",
                name: "Tổng thời gian trung bình",
                color: "#0066FF",
                tooltip: {
                    visible: true,
                    template: "#: value # phút"
                }
            }, {
                opacity: 1,
                type: "line",
                field: "ThoiGianCho",
                categoryField: "ThoiGian",
                name: "Thời gian chờ trung bình",
                color: "#FFFF00",
                tooltip: {
                    visible: true,
                    template: "#: value # phút"
                }
            }, {
                opacity: 1,
                type: "line",
                field: "ThoiGianGiaiQuyet",
                categoryField: "ThoiGian",
                name: "Thời gian giải quyết trung bình",
                color: "#33FF00",
                tooltip: {
                    visible: true,
                    template: "#: value # phút"
                }
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
                createGridThuTucBP(url + "/api/ThuTucAPI/?_Loai=nam&_GiaTri=" + $("#year-column-thu-tuc-bp").val() + "&_MaBP=" + mabp_thutuc + "&_CB=1", "Bảng thời gian giải quyết thủ tục năm " + $("#year-column-thu-tuc-bp").val());
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
        createGridThuTucBP(url + "/api/ThuTucAPI/?_Loai=nam&_GiaTri=" + $("#year-column-thu-tuc-bp").val() + "&_MaBP=" + mabp_thutuc + "&_CB=1", "Bảng thời gian giải quyết thủ tục năm " + $("#year-column-thu-tuc-bp").val());
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
                createGridThuTucBP(url + "/api/ThuTucAPI/?_Loai=thang&_GiaTri=" + $("#month-column-thu-tuc-bp").val() + "&_MaBP=" + mabp_thutuc + "&_CB=1", "Bảng thời gian giải quyết thủ tục (" + $("#month-column-thu-tuc-bp").val() + ")");
            } else {
                $("#month-column-thu-tuc-bp").val("09 2018");
            }
            if (!$("#cbx-month-thu-tuc-bp").prop("checked")) {
                $("#year-group-thu-tuc-bp-month span.k-widget.k-datepicker.k-header").hide(); // Ẩn datepicker của chọn tháng
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
        createGridThuTucBP(url + "/api/ThuTucAPI/?_Loai=thang&_GiaTri=" + $("#month-column-thu-tuc-bp").val() + "&_MaBP=" + mabp_thutuc + "&_CB=1", "Bảng thời gian giải quyết thủ tục (" + $("#month-column-thu-tuc-bp").val() + ")");
    }
})
// Tạo sự kiện click checkbox chọn xem biểu đồ miền theo tháng hoặc năm
function hideShowMonthBP() {
    if (!$("#cbx-month-thu-tuc-bp").prop("checked")) { // Chưa check
        createYearColumnThuTucBP();
        $("#cbx-year-thu-tuc-bp").prop("checked", "checked")
        $("#year-group-thu-tuc-bp-month span.k-widget.k-datepicker.k-header").hide(); // Ẩn datepicker của chọn tháng
        $("#year-group-thu-tuc-bp-year span.k-widget.k-dropdown.k-header").show(); // Hiện dropdownlist của chọn năm
    } else { // Đã check
        createMonthCircleThuTucBP();
        $("#cbx-year-thu-tuc-bp").removeAttr("checked")
        $("#year-group-thu-tuc-bp-month span.k-widget.k-datepicker.k-header").show(); // Hiện datepicker của chọn tháng
        $("#year-group-thu-tuc-bp-year span.k-widget.k-dropdown.k-header").hide(); // Ẩn dropdownlist của chọn năm
    }
}
function hideShowYearBP() {
    if (!$("#cbx-year-thu-tuc-bp").prop("checked")) { // Chưa check
        createMonthCircleThuTucBP();
        $("#cbx-month-thu-tuc-bp").prop("checked", "checked")
        $("#year-group-thu-tuc-bp-month span.k-widget.k-datepicker.k-header").show(); // Hiện datepicker của chọn tháng
        $("#year-group-thu-tuc-bp-year span.k-widget.k-dropdown.k-header").hide(); // Ẩn dropdownlist của chọn năm
    } else { // Đã check
        createYearColumnThuTucBP();
        $("#cbx-month-thu-tuc-bp").removeAttr("checked")
        $("#year-group-thu-tuc-bp-month span.k-widget.k-datepicker.k-header").hide(); // Ẩn datepicker của chọn tháng
        $("#year-group-thu-tuc-bp-year span.k-widget.k-dropdown.k-header").show(); // Hiện dropdownlist của chọn năm
    }
}
$("#year-group-thu-tuc-bp div:first-child label").click(function () {
    $("#cbx-month-thu-tuc-bp").prop("checked", !$("#cbx-month-thu-tuc-bp").prop("checked"));
    hideShowMonthBP();
})
$("#year-group-thu-tuc-bp div:last-child label").click(function () {
    $("#cbx-year-thu-tuc-bp").prop("checked", !$("#cbx-year-thu-tuc-bp").prop("checked"));
    hideShowYearBP();
})
$("#cbx-month-thu-tuc-bp").change(function () {
    hideShowMonthBP();
})
$("#cbx-year-thu-tuc-bp").change(function () {
    hideShowYearBP();
})
// Phương thức tạo bảng kết quả báo cáo
function createGridThuTucBP(urlStr, titleStr) {
    $("#grid-thu-tuc-bp").html("");
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
                id: "MaCB",
                fields: {
                    MaCB: { type: "number" },
                    HoTen: { type: "string", validation: { required: true } },
                    MaCBSD: { type: "string", validation: { required: true } },
                    ThoiGianCho: { type: "number", validation: { required: true } },
                    ThoiGianGiaiQuyet: { type: "number", validation: { required: true } },
                    TongThoiGian: { type: "number", validation: { required: true } },
                    SoLuongGiaiQuyet: { type: "number", validation: { required: true } }
                }
            }
        },

        aggregate: [
            { field: "MaCBSD", aggregate: "count" },
            { field: "SoLuongGiaiQuyet", aggregate: "average" },
            { field: "SoLuongGiaiQuyet", aggregate: "sum" },
            { field: "SoLuongGiaiQuyet", aggregate: "min" },
            { field: "SoLuongGiaiQuyet", aggregate: "max" },
            { field: "ThoiGianGiaiQuyet", aggregate: "average" },
            { field: "ThoiGianGiaiQuyet", aggregate: "sum" },
            { field: "ThoiGianGiaiQuyet", aggregate: "min" },
            { field: "ThoiGianGiaiQuyet", aggregate: "max" }
        ]

    });

    var grid = $("#grid-thu-tuc-bp").kendoGrid({
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
            { field: "MaCBSD", title: "Mã cán bộ", width: 1, footerTemplate: "Tổng cộng: #=count#" },
            { field: "HoTen", title: "Họ tên", width: 2 },
            {
                field: "ThoiGianGiaiQuyet", title: "Thời gian giải quyết trung bình (Phút)", width: 2,
                footerTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Lớn nhất: #=max#</div><div>Nhỏ nhất: #=min#</div>"
            },
            {
                field: "SoLuongGiaiQuyet", title: "Tổng số lượng (Lần)", width: 2,
                footerTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Lớn nhất: #=max#</div><div>Nhỏ nhất: #=min#</div>"
            }
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
        plotArea: {
            background: "blue",
            opacity: 0.1
        },
        series:
            [{
                opacity: 0.2,
                type: "column",
                field: "SoLuongGiaiQuyet",
                categoryField: "ThoiGian",
                name: "Tổng số lượng",
                color: "#ff1c1c",
                axis: "quantity",
                tooltip: {
                    visible: true,
                    template: "#: value # lần"
                }
            }, {
                opacity: 1,
                type: "line",
                field: "ThoiGianGiaiQuyet",
                categoryField: "ThoiGian",
                name: "Thời gian giải quyết trung bình",
                color: "#33FF00",
                axis: "time",
                tooltip: {
                    visible: true,
                    template: "#: value # phút"
                }
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
            if (!$("#cbx-month-thu-tuc-cb").prop("checked")) {
                $("#year-group-thu-tuc-cb-month span.k-widget.k-datepicker.k-header").hide(); // Ẩn datepicker của chọn tháng
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
function hideShowMonthCB() {
    if (!$("#cbx-month-thu-tuc-cb").prop("checked")) { // Chưa check
        createYearColumnThuTucCB();
        $("#cbx-year-thu-tuc-cb").prop("checked", "checked")
        $("#year-group-thu-tuc-cb-month span.k-widget.k-datepicker.k-header").hide(); // Ẩn datepicker của chọn tháng
        $("#year-group-thu-tuc-cb-year span.k-widget.k-dropdown.k-header").show(); // Hiện Dropdownlist của chọn năm

    } else { // Đã check
        createMonthCircleThuTucCB();
        $("#cbx-year-thu-tuc-cb").removeAttr("checked")
        $("#year-group-thu-tuc-cb-month span.k-widget.k-datepicker.k-header").show(); // Hiện datepicker của chọn tháng
        $("#year-group-thu-tuc-cb-year span.k-widget.k-dropdown.k-header").hide(); // Ẩn Dropdownlist của chọn năm
    }
}
function hideShowYearCB() {
    if (!$("#cbx-year-thu-tuc-cb").prop("checked")) { // Chưa check
        createMonthCircleThuTucCB();
        $("#cbx-month-thu-tuc-cb").prop("checked", "checked")
        $("#year-group-thu-tuc-cb-month span.k-widget.k-datepicker.k-header").show(); // Hiện datepicker của chọn tháng
        $("#year-group-thu-tuc-cb-year span.k-widget.k-dropdown.k-header").hide(); // Ẩn Dropdownlist của chọn năm

    } else { // Đã check
        createYearColumnThuTucCB();
        $("#cbx-month-thu-tuc-cb").removeAttr("checked")
        $("#year-group-thu-tuc-cb-month span.k-widget.k-datepicker.k-header").hide(); // Ẩn datepicker của chọn tháng
        $("#year-group-thu-tuc-cb-year span.k-widget.k-dropdown.k-header").show(); // Hiện Dropdownlist của chọn năm
    }
}
$("#year-group-thu-tuc-cb div:first-child label").click(function () {
    $("#cbx-month-thu-tuc-cb").prop("checked", !$("#cbx-month-thu-tuc-cb").prop("checked"));
    hideShowMonthCB();
})
$("#year-group-thu-tuc-cb div:last-child label").click(function () {
    $("#cbx-year-thu-tuc-cb").prop("checked", !$("#cbx-year-thu-tuc-cb").prop("checked"));
    hideShowYearCB();
})
$("#cbx-month-thu-tuc-cb").change(function () {
    hideShowMonthCB();
})
$("#cbx-year-thu-tuc-cb").change(function () {
    hideShowYearCB();
})
// Phương thức tạo bảng kết quả báo cáo
function createGridThuTucCB(urlStr, titleStr) {
    $("#grid-thu-tuc-cb").html("");
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
                    Ngay: { type: "date", validation: { required: true }, },
                    ThoiGianGoi: { type: "date", validation: { required: true } },
                    ThoiGianHoanTat: { type: "date", validation: { required: true } },
                    ThoiGianGiaiQuyet: { type: "number", validation: { required: true } },
                }
            }
        },
        group: {
            field: "Ngay", aggregates: [
                { field: "SoThuTu", aggregate: "count" },
                { field: "ThoiGianGiaiQuyet", aggregate: "average" },
                { field: "ThoiGianGiaiQuyet", aggregate: "sum" },
                { field: "ThoiGianGiaiQuyet", aggregate: "min" },
                { field: "ThoiGianGiaiQuyet", aggregate: "max" },
            ]
        },

        aggregate: [
            { field: "SoThuTu", aggregate: "count" },
            { field: "ThoiGianGiaiQuyet", aggregate: "average" },
            { field: "ThoiGianGiaiQuyet", aggregate: "sum" },
            { field: "ThoiGianGiaiQuyet", aggregate: "min" },
            { field: "ThoiGianGiaiQuyet", aggregate: "max" },
        ]

    });

    var grid = $("#grid-thu-tuc-cb").kendoGrid({
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
            { field: "SoThuTu", title: "Số thứ tự", width: 60, footerTemplate: "Tổng cộng: #=count#", groupFooterTemplate: "Tổng cộng: #=count#" },
            { field: "ThoiGianGoi", title: "Thời điểm gọi số", width: 80, format: "{0:HH:mm:ss}" },
            { field: "ThoiGianHoanTat", title: "Thời điểm hoàn tất", width: 80, format: "{0:HH:mm:ss}" },
            { field: "ThoiGianGiaiQuyet", title: "Thời gian giải quyết (Phút)", width: 100, groupFooterTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Lớn nhất: #=max#</div><div>Nhỏ nhất: #=min#</div>", footerTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Lớn nhất: #=max#</div><div>Nhỏ nhất: #=min#</div>" },
            { hidden: true, field: "Ngay", title: "Ngày", width: 1, format: "{0:dd MM yyyy}" },
        ],
    }).data("kendoGrid");
    //grid.hideColumn("Ngay");
}
