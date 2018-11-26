var wd; // id của cửa sổ
var ddl; // id của dropdownlist
var sdp; // id của start date picker
var edp; // id của end date picker
var cbx; // id của checkbox
var bt; // id của button
var ch; // id của chart
var mabpCompare; // Mã bộ phận
var macbCompare; // Mã cán bộ
var tenCompare; // Tên bảng so sánh
var loaiCompare; // Loại thời gian so sánh (tháng/năm)
var clickCompare; // Tham số xác định để gọi API so sánh kết quả đánh giá
var btnCompare0 = false;
var btnCompare1 = false;
var btnCompare2 = false;
var btnCompare3 = false;
// Phương thức tạo cửa sổ
function createWindowCompare(macb, mabp, ten, loai, click) { // loai = 0 là so sánh kết quả đánh giá
    $("#big-white-div-loading").show();
    if (click == 0) { // So sánh kết quả đánh giá của bộ phận
        wd = "#windowCompareBP";
        ddl = "#thoigianCompareBP";
        sdp = "#startCompareBP";
        edp = "#endCompareBP";
        cbx = "#cbCompareBP";
        bt = "#btnCompareBP";
        ch = "#chartCompareBP";
        createBtnComapre(btnCompare0);
        btnCompare0 = true;
    } else if (click == 1) { // So sánh kết quả đánh giá của cán bộ
        wd = "#windowCompareCB";
        ddl = "#thoigianCompareCB";
        sdp = "#startCompareCB";
        edp = "#endCompareCB";
        cbx = "#cbCompareCB";
        bt = "#btnCompareCB";
        ch = "#chartCompareCB";
        createBtnComapre(btnCompare1);
        btnCompare1 = true;
    } else if (click == 2) { // So sánh thời gian giải quyết thủ tục của bộ phận
        wd = "#windowCompareBPThuTuc";
        ddl = "#thoigianCompareBPThuTuc";
        sdp = "#startCompareBPThuTuc";
        edp = "#endCompareBPThuTuc";
        cbx = "#cbCompareBPThuTuc";
        bt = "#btnCompareBPThuTuc";
        ch = "#chartCompareBPThuTuc";
        createBtnComapre(btnCompare2);
        btnCompare2 = true;
    } else if (click == 3) { // So sánh thời gian giải quyết thủ tục của cán bộ
        wd = "#windowCompareCBThuTuc";
        ddl = "#thoigianCompareCBThuTuc";
        sdp = "#startCompareCBThuTuc";
        edp = "#endCompareCBThuTuc";
        cbx = "#cbCompareCBThuTuc";
        bt = "#btnCompareCBThuTuc";
        ch = "#chartCompareCBThuTuc";
        createBtnComapre(btnCompare3);
        btnCompare3 = true;
    }
    $(ch).html("");
    mabpCompare = mabp;
    macbCompare = macb;
    tenCompare = ten;
    clickCompare = click;
    function onClose(e) {
        $("#big-white-div-loading").hide();
    }
    // Tạo cửa sổ so sánh
    $(wd).kendoWindow({
        width: "1300px",
        title: "Bảng so sánh",
        close: onClose,
    }).data("kendoWindow").center().open();
    createTGCompare(); // Gọi phương thức tạo Dropdownlist
    getDateCompare($(ddl).val(), mabp, macb); // Gọi lấy thời gian cho datepicker
}
// Phương thức tạo nút so sánh
function createBtnComapre(btnCompare) {
    if (!btnCompare) {
        function onClick(e) {
            if ($(ddl).val() == 2) {
                loaiCompare = "thang";
            } else if ($(ddl).val() == 3) {
                loaiCompare = "nam";
            }
            getCompareValue(mabpCompare, macbCompare, $(sdp).val(), $(edp).val(), loaiCompare, clickCompare, tenCompare);
        }
    }
    $(bt).kendoButton({
        click: onClick
    });
}
// Tạo Dropdownlist chọn kiểu thời gian so sánh
function createTGCompare() {
    var arr = [
        { text: "Tháng", value: 2 },
        { text: "Năm", value: 3 },
    ];
    $(ddl).kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: arr,
        change: changeTGCompare // Tạo sự kiện lựa chọn
    });
}
// Tạo sự kiện chọn kiểu thời gian
function changeTGCompare() {
    dateCompare = this.value();
    getDateCompare(dateCompare, mabpCompare, macbCompare); // Gọi phương thức lấy thời gian cho DatePicker
}
// Phương thức lấy thời gian cho DatePicker so sánh từ API
function getDateCompare(value, MABP, MACB) {
    var urlCompare;
    if (MACB == 0) {
        urlCompare = url + "/api/ValuesAPI/?_MaBP=" + MABP;
    } else {
        urlCompare = url + "/api/ValuesAPI/?_MaCB=" + MACB;
    }
    $.ajax({
        type: "GET",
        url: urlCompare,
        dataType: "json",
        async: false,
        success: function (data) {
            startCompare = data[0];
            endCompare = data[1];
            createDPCompare(value, startCompare, endCompare); // Gọi phương thức tạo DatePicker
        },
        error: function (xhr) { }
    })
}
// Phương thức tạo thời gian cho DatePicker so sánh
function createDPCompare(value, startCompare, endCompare) {
    $(sdp).prop("readonly", false);
    $(edp).prop("readonly", false);
    //$(sdp).html("");
    //$(edp).html("");
    var date = new Date(startCompare);
    var start;
    var end;
    if (value == 2) {
        start = $(sdp).kendoDatePicker({
            start: "year",
            depth: "year",
            dateInput: false,
            value: date,
            format: "MM/yyyy",
            min: new Date(startCompare),
            max: new Date(endCompare),
            change: startChange
        }).data("kendoDatePicker");
        end = $(edp).kendoDatePicker({
            start: "year",
            depth: "year",
            dateInput: false,
            value: date,
            format: "MM/yyyy",
            min: new Date(startCompare),
            max: new Date(endCompare),
            change: endChange
        }).data("kendoDatePicker");
        start.max(end.value());
        end.min(start.value());
    } else if (value == 3) {
        start = $(sdp).kendoDatePicker({
            start: "decade",
            depth: "decade",
            dateInput: false,
            value: date,
            format: "yyyy",
            min: new Date(startCompare),
            max: new Date(endCompare),
            change: startChange
        }).data("kendoDatePicker");
        end = $(edp).kendoDatePicker({
            start: "decade",
            depth: "decade",
            dateInput: false,
            value: date,
            format: "yyyy",
            min: new Date(startCompare),
            max: new Date(endCompare),
            change: endChange
        }).data("kendoDatePicker");
        start.max(end.value());
        end.min(start.value());
    }
    function startChange() {
        var startDate = start.value(),
            endDate = end.value();

        if (startDate) {
            startDate = new Date(startDate);
            startDate.setDate(startDate.getDate());
            end.min(startDate);
        } else if (endDate) {
            start.max(new Date(endDate));
        } else {
            endDate = new Date();
            start.max(endDate);
            end.min(endDate);
        }
    }

    function endChange() {
        var endDate = end.value(),
            startDate = start.value();

        if (endDate) {
            endDate = new Date(endDate);
            endDate.setDate(endDate.getDate());
            start.max(endDate);
        } else if (startDate) {
            end.min(new Date(startDate));
        } else {
            endDate = new Date();
            start.max(endDate);
            end.min(endDate);
        }
    }
    $(sdp).prop("readonly", true);
    $(edp).prop("readonly", true);
}
// Phương thức xác định loại giá trị cần so sánh (kết quả đánh giá, thời gian giải quyết thủ tục)
function getCompareValue(MABP, MACB, START, END, LOAITHOIGIAN, LOAI, TITLE) {
    var _Url;
    if (LOAI == 0 || LOAI == 1) { _Url = url + "/api/SoSanhAPI?_MaBP=" + MABP + "&_MaCB=" + MACB + "&_Start=" + START + "&_End=" + END + "&_LoaiThoiGian=" + LOAITHOIGIAN + "&_Loai=" + LOAI }
    else { _Url = url + "/api/SoSanhAPI?_MaBP=" + MABP + "&_MaCB=" + MACB + "&_Start=" + START + "&_End=" + END + "&_LoaiThoiGian=" + LOAITHOIGIAN }
    $.ajax({
        url: _Url,
        type: "GET",
        dataType: "json",
        async: false,
        success: function (result) {
            if (!$(cbx).prop("checked")) { // So sánh nhiều khoảng thời gian
                if (result["length"] == 1) {
                    result = [result[0]];
                } else {
                    result = [result[0], result[result["length"] - 1]]
                }
            }
            if (LOAI == 0 || LOAI == 1) { // LOAI = 0 và 1 là so sánh kết quả đánh giá = 2 và 2 là so sánh thời gian giải quyết thủ tục
                $.ajax({
                    url: url + '/api/SoSanhAPI',
                    type: "GET",
                    dataType: "json",
                    success: function (resultMD) {
                        if (!$(cbx).prop("checked")) {
                            createChartCompare(result, resultMD, "Bảng so sánh - " + TITLE + "(" + START + " - " + END + ")");
                        } else {
                            createChartCompareManyTimes(result, "Bảng so sánh - " + TITLE + "(" + START + " - " + END + ")");
                        }
                    },
                    error: function (xhr) { }
                })
            } else if (LOAI == 2) {
                if (!$(cbx).prop("checked")) {
                    resultMD = ["Thời gian chờ trung bình (Phút)", "Thời gian xử lý trung bình (Phút)", "Tổng thời gian trung bình (Phút)", "Số lượng thủ tục đã giải quyết (Lần)"]
                    createChartCompare(result, resultMD, "Bảng so sánh -" + TITLE + "(" + START + " - " + END + ")");
                } else {
                    createChartCompareManyTimesThuTuc(result, "Bảng so sánh -" + TITLE + "(" + START + " - " + END + ")");
                }
            } else if (LOAI == 3) {
                if (!$(cbx).prop("checked")) {
                    resultMD = ["Thời gian xử lý trung bình (Phút)", "Số lượng thủ tục đã giải quyết (Lần)"]
                    createChartCompare(result, resultMD, "Bảng so sánh -" + TITLE + "(" + START + " - " + END + ")");
                } else {
                    createChartCompareManyTimesThuTucCB(result, "Bảng so sánh -" + TITLE + "(" + START + " - " + END + ")")
                }
            }
        },
        error: function (xhr) { }
    })
}
// Phương thức tạo biểu đồ so sánh
function createChartCompare(array, categories, title) {
    $(ch).kendoChart({
        title: {
            text: title
        },
        legend: {
            position: "top"
        },
        seriesDefaults: {
            type: "column"
        },
        series: array,
        valueAxis: {
            labels: {
                format: "{0}"
            },
            line: {
                visible: false
            },
            axisCrossingValue: 0
        },
        categoryAxis: {
            categories: categories,
            line: {
                visible: false
            },
            labels: {
                padding: { top: 0 }
            }
        },
        tooltip: {
            visible: true,
            format: "{0}",
            template: "#= series.name #: #= value #"
        }
    });
}
// Tạo biểu đồ so sánh kết quả đánh giá của nhiều khoảng thời gian 
function createChartCompareManyTimes(arr, titleStr) {
    var RHL = []
    var HL = []
    var BT = []
    var KHL = []
    var Diem = []
    for (i in arr) {
        for (j in arr[i]["data"]) {
            if (j == 0) RHL.push(arr[i]["data"][j])
            if (j == 1) HL.push(arr[i]["data"][j])
            if (j == 2) BT.push(arr[i]["data"][j])
            if (j == 3) KHL.push(arr[i]["data"][j])
            if (j == 4) Diem.push(arr[i]["data"][j])
        }
    }
    var a = []
    a["name"] = "Rất hài lòng"
    a["data"] = RHL
    a["type"] = "line"
    a["color"] = "#73c100"
    a["opacity"] = 1
    a["axis"] = "quantity"
    a["tooltip"] = {
        "visible": true,
        "template": "#: value # lần"
    }
    var b = []
    b["name"] = "Hài lòng"
    b["data"] = HL
    b["type"] = "line"
    b["color"] = "#007eff"
    b["opacity"] = 1
    b["axis"] = "quantity"
    b["tooltip"] = {
        "visible": true,
        "template": "#: value # lần"
    }
    var c = []
    c["name"] = "Bình thường"
    c["data"] = BT
    c["type"] = "line"
    c["color"] = "#ffae00"
    c["opacity"] = 1
    c["axis"] = "quantity"
    c["tooltip"] = {
        "visible": true,
        "template": "#: value # lần"
    }
    var d = []
    d["name"] = "Không hài lòng"
    d["data"] = KHL
    d["type"] = "line"
    d["color"] = "#ff1c1c"
    d["opacity"] = 1
    d["axis"] = "quantity"
    d["tooltip"] = {
        "visible": true,
        "template": "#: value # lần"
    }
    var e = []
    e["name"] = "Điểm"
    e["data"] = Diem
    e["type"] = "column"
    e["tooltip"] = {
        "visible": true,
        "template": "#: value # điểm"
    }
    var array = new Array()
    array.push(a)
    array.push(b)
    array.push(c)
    array.push(d)
    array.push(e)

    var categories = []
    for (i in arr) {
        categories.push(arr[i]["name"])
    }

    $(ch).kendoChart({
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
        series: array,
        categoryAxis: {
            categories: categories,
            labels: {
                rotation: -90
            },
            crosshair: {
                visible: true
            },
            axisCrossingValue: [0, 1000],
        },
        valueAxis: [{
            name: "quantity",
            labels: {
                format: "{0:n0} lần",
            },
            title: {
                text: "Mức độ đánh giá (Lần)"
            },
            color: "#007eff"
        }, {
            name: "time",
            labels: {
                format: "{0:n0} điểm",
            },
            title: {
                text: "Điểm số"
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
    });
}
// Tạo biểu đồ so sánh thời gian giải quyết thủ tục bộ phận của nhiều khoảng thời gian 
function createChartCompareManyTimesThuTuc(arr, titleStr) {
    var Cho = []
    var XuLy = []
    var Tong = []
    var Total = []
    for (i in arr) {
        for (j in arr[i]["data"]) {
            if (j == 0) Cho.push(arr[i]["data"][j])
            if (j == 1) XuLy.push(arr[i]["data"][j])
            if (j == 2) Tong.push(arr[i]["data"][j])
            if (j == 3) Total.push(arr[i]["data"][j])
        }
    }
    var a = []
    a["name"] = "Thời gian chờ trung bình"
    a["data"] = Cho
    a["type"] = "line"
    a["color"] = "#ffae00"
    a["opacity"] = 1
    a["axis"] = "time"
    a["tooltip"] = {
        "visible": true,
        "template": "#: value # phút"
    }
    var b = []
    b["name"] = "Thời gian xử lý trung bình"
    b["data"] = XuLy
    b["type"] = "line"
    b["color"] = "#73c100"
    b["opacity"] = 1
    b["axis"] = "time"
    b["tooltip"] = {
        "visible": true,
        "template": "#: value # phút"
    }
    var c = []
    c["name"] = "Tổng thời gian trung bình"
    c["data"] = Tong
    c["type"] = "line"
    c["color"] = "#007eff"
    c["opacity"] = 1
    c["axis"] = "time"
    c["tooltip"] = {
        "visible": true,
        "template": "#: value # phút"
    }
    var d = []
    d["name"] = "Số lượng thủ tục đã giải quyết"
    d["data"] = Total
    d["type"] = "column"
    d["color"] = "#ff1c1c"
    d["opacity"] = 1
    d["axis"] = "quantity"
    d["tooltip"] = {
        "visible": true,
        "template": "#: value # lần"
    }
    var array = new Array()
    array.push(a)
    array.push(b)
    array.push(c)
    array.push(d)

    var categories = []
    for (i in arr) {
        categories.push(arr[i]["name"])
    }

    $(ch).kendoChart({
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
        series: array,
        categoryAxis: {
            categories: categories,
            labels: {
                rotation: -90
            },
            crosshair: {
                visible: true
            },
            axisCrossingValue: [0, 1000],
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
    });
}
// Tạo biểu đồ so sánh thời gian giải quyết thủ tục bộ phận của nhiều khoảng thời gian 
function createChartCompareManyTimesThuTucCB(arr, titleStr) {
    var XuLy = []
    var Total = []
    for (i in arr) {
        for (j in arr[i]["data"]) {
            if (j == 0) XuLy.push(arr[i]["data"][j])
            if (j == 1) Total.push(arr[i]["data"][j])
        }
    }
    var b = []
    b["name"] = "Thời gian xử lý trung bình"
    b["data"] = XuLy
    b["type"] = "line"
    b["color"] = "#73c100"
    b["opacity"] = 1
    b["axis"] = "time"
    b["tooltip"] = {
        "visible": true,
        "template": "#: value # phút"
    }
    var d = []
    d["name"] = "Số lượng thủ tục đã giải quyết"
    d["data"] = Total
    d["type"] = "column"
    d["color"] = "#ff1c1c"
    d["opacity"] = 1
    d["axis"] = "quantity"
    d["tooltip"] = {
        "visible": true,
        "template": "#: value # lần"
    }
    var array = new Array()
    array.push(b)
    array.push(d)

    var categories = []
    for (i in arr) {
        categories.push(arr[i]["name"])
    }

    $(ch).kendoChart({
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
        series: array,
        categoryAxis: {
            categories: categories,
            labels: {
                rotation: -90
            },
            crosshair: {
                visible: true
            },
            axisCrossingValue: [0, 1000],
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
    });
}

$("#spanCbCompareBP").click(function () {
    $("#cbCompareBP").prop("checked", !$("#cbCompareBP").prop("checked"))
})
$("#spanCbCompareCB").click(function () {
    $("#cbCompareCB").prop("checked", !$("#cbCompareCB").prop("checked"))
})
$("#spanCbCompareBPThuTuc").click(function () {
    $("#cbCompareBPThuTuc").prop("checked", !$("#cbCompareBPThuTuc").prop("checked"))
})
$("#spanCbCompareCBThuTuc").click(function () {
    $("#cbCompareCBThuTuc").prop("checked", !$("#cbCompareCBThuTuc").prop("checked"))
})