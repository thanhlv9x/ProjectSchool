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
                        createChartCompare(result, resultMD, "Bảng so sánh - " + TITLE + "(" + START + " - " + END + ")");
                    },
                    error: function (xhr) { }
                })
            } else if (LOAI == 2) {
                resultMD = ["Thời gian chờ trung bình (Phút)", "Thời gian xử lý trung bình (Phút)", "Tổng thời gian trung bình (Phút)", "Số lượng thủ tục đã giải quyết (Lần)"]
                createChartCompare(result, resultMD, "Bảng so sánh -" + TITLE + "(" + START + " - " + END + ")");
            } else if (LOAI == 3) {
                resultMD = ["Thời gian xử lý trung bình (Phút)", "Số lượng thủ tục đã giải quyết (Lần)"]
                createChartCompare(result, resultMD, "Bảng so sánh -" + TITLE + "(" + START + " - " + END + ")");
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