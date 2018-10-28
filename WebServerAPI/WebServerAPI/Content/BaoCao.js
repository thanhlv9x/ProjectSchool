// ============ Báo cáo ============
var mabp_report = 0;
var macb_report = 0;
var date_report = 0;
var start_report;
var end_report;
// Tạo dropdownlist chọn bộ phận
function createBPbaocao() {
    $.ajax({
        type: "GET",
        url: url + "/api/BoPhanAPI",
        dataType: "json",
        async: false,
        success: function (data) {
            $("#bo-phan-bao-cao").show();
            var arr = [{ text: "Chọn tất cả", value: 0 }];
            data.forEach(function (item) {
                arr.push({ text: item.TenBP, value: item.MaBP });
            });
            $("#bo-phan-bao-cao").kendoDropDownList({
                dataTextField: "text",
                dataValueField: "value",
                dataSource: arr,
                select: selectBPbaocao
            });
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    })
}
// Tạo sự kiện cho dropdownlist bộ phận
function selectBPbaocao(e) {
    mabp_report = e.dataItem.value;
    if (mabp_report == 0) {
        $("#can-bo-report-1").hide();
        $("#can-bo-report-2").hide();
    } else {
        $("#can-bo-report-1").show();
        $("#can-bo-report-2").show();
        createCBbaocao(mabp_report);
    }
    getDate(mabp_report);
    createDPbaocao(date_report);
}
// Tạo dropdownlist chọn cán bộ
function createCBbaocao(MaBP) {
    var url_report;
    if (MaBP == 0) {
        url_report = url + "/api/BoPhanAPI";
    } else {
        url_report = url + "/api/BoPhanAPI/?_MaBP=" + MaBP;
    }
    $.ajax({
        type: "GET",
        url: url_report,
        dataType: "json",
        success: function (data) {
            $("#can-bo-bao-cao").show();
            var arr = [{ text: "Chọn tất cả", value: 0 }];
            data.forEach(function (item) {
                arr.push({ text: item.HoTen, value: item.MaCB });
            });
            $("#can-bo-bao-cao").kendoDropDownList({
                dataTextField: "text",
                dataValueField: "value",
                dataSource: arr,
                select: selectCBbaocao
            });
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    })
}
// Tạo sự kiện cho dropdownlist cán bộ
function selectCBbaocao(e) {
    macb_report = e.dataItem.value;
}
// Tạo phương thức lấy ngày bắt đầu và kết thúc
function getDate(MaBP) {
    var url_report;
    if (MaBP == 0) {
        url_report = url + "/api/ValuesAPI";
    } else {
        url_report = url + "/api/ValuesAPI/?_MaBP=" + MaBP;
    }
    $.ajax({
        type: "GET",
        url: url_report,
        dataType: "json",
        async: false,
        success: function (data) {
            start_report = data[0];
            end_report = data[1];
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    })
}
// Tạo dropdownlist chọn kiểu thời gian
function createTGbaocao() {
    var arr = [
        { text: "Chọn tất cả", value: 0 },
        { text: "Ngày", value: 1 },
        { text: "Tháng", value: 2 },
        { text: "Năm", value: 3 },
    ];
    $("#thoi-gian-bao-cao").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: arr,
        change: changeTGbaocao
    });
}
// Tạo sự kiện chọn kiểu thời gian
function changeTGbaocao() {
    date_report = this.value();
    getDate(mabp_report);
    createDPbaocao(date_report);
}
// Tạo datepicker chọn thời gian
function createDPbaocao(value) {
    var date = new Date(Date.now());
    var start;
    var end;
    $("#start-report-1").show();
    $("#start-report-2").show();
    $("#end-report-1").show();
    $("#end-report-2").show();
    if (value == 0) {
        $("#start-report-1").hide();
        $("#start-report-2").hide();
        $("#end-report-1").hide();
        $("#end-report-2").hide();
    } else if (value == 1) {
        start = $("#start-bao-cao").kendoDatePicker({
            start: "month",
            depth: "month",
            dateInput: false,
            value: date,
            min: new Date(start_report),
            max: new Date(end_report),
            change: startChange
        }).data("kendoDatePicker");
        end = $("#end-bao-cao").kendoDatePicker({
            start: "month",
            depth: "month",
            dateInput: false,
            value: date,
            min: new Date(start_report),
            max: new Date(end_report),
            change: endChange
        }).data("kendoDatePicker");
        start.max(end.value());
        end.min(start.value());
    } else if (value == 2) {
        start = $("#start-bao-cao").kendoDatePicker({
            start: "year",
            depth: "year",
            dateInput: false,
            value: date,
            format: "MM/yyyy",
            min: new Date(start_report),
            max: new Date(end_report),
            change: startChange
        }).data("kendoDatePicker");
        end = $("#end-bao-cao").kendoDatePicker({
            start: "year",
            depth: "year",
            dateInput: false,
            value: date,
            format: "MM/yyyy",
            min: new Date(start_report),
            max: new Date(end_report),
            change: endChange
        }).data("kendoDatePicker");
        start.max(end.value());
        end.min(start.value());
    } else if (value == 3) {
        start = $("#start-bao-cao").kendoDatePicker({
            start: "decade",
            depth: "decade",
            dateInput: false,
            value: date,
            format: "yyyy",
            min: new Date(start_report),
            max: new Date(end_report),
            change: startChange
        }).data("kendoDatePicker");
        end = $("#end-bao-cao").kendoDatePicker({
            start: "decade",
            depth: "decade",
            dateInput: false,
            value: date,
            format: "yyyy",
            min: new Date(start_report),
            max: new Date(end_report),
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
}
// Tạo nút xem báo cáo
function createButtonReport() {
    $("#btn-view").kendoButton({
        click: onClickBtnReport
    });
    $("#btn-report").kendoButton();
}
// Tạo sự kiện nút xem báo cáo
function onClickBtnReport() {
    $("#noi-dung-bao-cao").html('<div id="title-bao-cao-1" style="margin: 1em 0 1em 0; padding: 0.8em; text-align:center"></div><div id = "grid-report-1" ></div ><div id="title-bao-cao-2" style="margin: 1em 0 1em 0; padding: 0.8em; text-align:center"></div><div id="grid-report-2"></div>');
    var start;
    var end;
    var title1 = "<h2>BẢNG KẾT QUẢ ĐÁNH GIÁ</h2>";
    var title2 = "<h2>BẢNG GÓP Ý</h2> ";
    mabp_report = $("#bo-phan-bao-cao").val();
    if (mabp_report == 0) {
        macb_report = 0;
    }
    else {
        macb_report = $("#can-bo-bao-cao").val();
        title1 += "<h3 style='margin-left: 0'>" + $("#bo-phan-bao-cao").data("kendoDropDownList").text() + "</h3>";
        title2 += "<h3 style='margin-left: 0'>" + $("#bo-phan-bao-cao").data("kendoDropDownList").text() + "</h3>";
        if (macb_report != 0) {
            title1 += "<div><b>Cán bộ: " + $("#can-bo-bao-cao").data("kendoDropDownList").text() + "</b></div>";
            title2 += "<div><b>Cán bộ: " + $("#can-bo-bao-cao").data("kendoDropDownList").text() + "</b></div>";
        }
    }
    if ($("#thoi-gian-bao-cao").val() == 0) {
        start = "";
        end = "";
        title1 += "<div>Tổng hợp</div>";
        title2 += "<div>Tổng hợp</div>";
    } else {
        start = $("#start-bao-cao").val();
        end = $("#end-bao-cao").val();
        title1 += "<div>(" + start + " - " + end + ")</div>";
        title2 += "<div>(" + start + " - " + end + ")</div>";
    }
    if ($("#cb-kq").prop("checked") && $("#cb-gy").prop("checked")) {
        createGridReport(url + "/api/BaoCaoAPI/?_MaBP=" + mabp_report + "&_MaCB=" + macb_report + "&_Start=" + start + "&_End=" + end);
        createGridFeedBack(url + "/api/BaoCaoAPI/?_MaBP=" + mabp_report + "&_MaCB=" + macb_report + "&_Start=" + start + "&_End=" + end + "&_GopY=" + "GopY");
        $("#title-bao-cao-1").html(title1);
        $("#title-bao-cao-2").html(title2);
    }
    else if ($("#cb-kq").prop("checked") && !$("#cb-gy").prop("checked")) {
        createGridReport(url + "/api/BaoCaoAPI/?_MaBP=" + mabp_report + "&_MaCB=" + macb_report + "&_Start=" + start + "&_End=" + end);
        $("#title-bao-cao-1").html(title1);
    }
    else if (!$("#cb-kq").prop("checked") && $("#cb-gy").prop("checked")) {
        createGridFeedBack(url + "/api/BaoCaoAPI/?_MaBP=" + mabp_report + "&_MaCB=" + macb_report + "&_Start=" + start + "&_End=" + end + "&_GopY=" + "GopY");
        $("#title-bao-cao-2").html(title2);
    }
}
// Phương thức tạo bảng kết quả báo cáo
function createGridReport(urlStr) {
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
                    MaBP: { type: "number", validation: { required: true } },
                    TenBP: { type: "string", validation: { required: true } },
                    HoTen: { type: "string", validation: { required: true } },
                    Diem: { type: "number", validation: { required: true } },
                    RHL_TyLe: { type: "number", validation: { required: true } },
                    RHL_SoLan: { type: "number", validation: { required: true } },
                    HL_TyLe: { type: "number", validation: { required: true } },
                    HL_SoLan: { type: "number", validation: { required: true } },
                    BT_TyLe: { type: "number", validation: { required: true } },
                    BT_SoLan: { type: "number", validation: { required: true } },
                    KHL_TyLe: { type: "number", validation: { required: true } },
                    KHL_SoLan: { type: "number", validation: { required: true } },
                    TongCong_TyLe: { type: "number", validation: { required: true } },
                    TongCong_SoLan: { type: "number", validation: { required: true } },
                }
            }
        },
        group: {
            field: "TenBP", aggregates: [
                { field: "HoTen", aggregate: "count" },
                { field: "RHL_TyLe", aggregate: "average" },
                { field: "RHL_SoLan", aggregate: "sum" },
                { field: "HL_TyLe", aggregate: "average" },
                { field: "HL_SoLan", aggregate: "sum" },
                { field: "BT_TyLe", aggregate: "average" },
                { field: "BT_SoLan", aggregate: "sum" },
                { field: "KHL_TyLe", aggregate: "average" },
                { field: "KHL_SoLan", aggregate: "sum" },
                { field: "TongCong_TyLe", aggregate: "average" },
                { field: "TongCong_SoLan", aggregate: "sum" },
                { field: "RHL_TyLe", aggregate: "min" },
                { field: "RHL_SoLan", aggregate: "min" },
                { field: "HL_TyLe", aggregate: "min" },
                { field: "HL_SoLan", aggregate: "min" },
                { field: "BT_TyLe", aggregate: "min" },
                { field: "BT_SoLan", aggregate: "min" },
                { field: "KHL_TyLe", aggregate: "min" },
                { field: "KHL_SoLan", aggregate: "min" },
                { field: "TongCong_TyLe", aggregate: "min" },
                { field: "TongCong_SoLan", aggregate: "min" },
                { field: "RHL_TyLe", aggregate: "max" },
                { field: "RHL_SoLan", aggregate: "max" },
                { field: "HL_TyLe", aggregate: "max" },
                { field: "HL_SoLan", aggregate: "max" },
                { field: "BT_TyLe", aggregate: "max" },
                { field: "BT_SoLan", aggregate: "max" },
                { field: "KHL_TyLe", aggregate: "max" },
                { field: "KHL_SoLan", aggregate: "max" },
                { field: "TongCong_TyLe", aggregate: "max" },
                { field: "TongCong_SoLan", aggregate: "max" },
                { field: "Diem", aggregate: "average" },
                { field: "Diem", aggregate: "sum" },
                { field: "Diem", aggregate: "min" },
                { field: "Diem", aggregate: "max" },
            ]
        },

        aggregate: [
            { field: "HoTen", aggregate: "count" },
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
            { field: "TongCong_TyLe", aggregate: "average" },
            { field: "TongCong_TyLe", aggregate: "sum" },
            { field: "TongCong_TyLe", aggregate: "count" },
            { field: "TongCong_SoLan", aggregate: "sum" },
            { field: "RHL_TyLe", aggregate: "min" },
            { field: "RHL_SoLan", aggregate: "min" },
            { field: "HL_TyLe", aggregate: "min" },
            { field: "HL_SoLan", aggregate: "min" },
            { field: "BT_TyLe", aggregate: "min" },
            { field: "BT_SoLan", aggregate: "min" },
            { field: "KHL_TyLe", aggregate: "min" },
            { field: "KHL_SoLan", aggregate: "min" },
            { field: "TongCong_TyLe", aggregate: "min" },
            { field: "TongCong_SoLan", aggregate: "min" },
            { field: "RHL_TyLe", aggregate: "max" },
            { field: "RHL_SoLan", aggregate: "max" },
            { field: "HL_TyLe", aggregate: "max" },
            { field: "HL_SoLan", aggregate: "max" },
            { field: "BT_TyLe", aggregate: "max" },
            { field: "BT_SoLan", aggregate: "max" },
            { field: "KHL_TyLe", aggregate: "max" },
            { field: "KHL_SoLan", aggregate: "max" },
            { field: "TongCong_TyLe", aggregate: "max" },
            { field: "TongCong_SoLan", aggregate: "max" },
            { field: "Diem", aggregate: "average" },
            { field: "Diem", aggregate: "sum" },
            { field: "Diem", aggregate: "min" },
            { field: "Diem", aggregate: "max" },
        ]
    });

    var grid = $("#grid-report-1").kendoGrid({
        pdf: {
            allPages: true,
            avoidLinks: true,
            paperSize: "A4",
            margin: { top: "2cm", left: "1cm", right: "1cm", bottom: "1cm" },
            landscape: true,
            repeatHeaders: true,
            template: $("#page-template-report").html(),
            scale: 0.8
        },
        dataSource: dataSource,
        navigatable: true,
        pageable: true,
        columns: [
            { field: "MaCB", title: "Mã số", width: 50 },
            { field: "HoTen", title: "Họ tên", width: 90, footerTemplate: "Tổng cộng: #=count#", groupFooterTemplate: "Tổng: #=count#" },
            { field: "Diem", title: "Số điểm", width: 80, groupFooterTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>", footerTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#%</span>#}else{#<span>#=Math.round(average*100)/100#%</span>#}#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>" },
            { field: "TenBP", title: "Bộ phận", width: 1, },
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
            {
                title: "Tổng hợp",
                columns: [
                    { field: "TongCong_TyLe", title: "Tỷ lệ(%)", width: 70, groupFooterTemplate: "Trung bình: #if(average==null){#<span>#=0#%</span>#}else{#<span>#=Math.round(average*100)/100#%</span>#}#", footerTemplate: "Trung bình: #=Math.round((sum/count)*100)/100#%" },
                    { field: "TongCong_SoLan", title: "Số lần", width: 70, groupFooterTemplate: "<div>Tổng: #=sum#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>", footerTemplate: "<div>Tổng: #=sum#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>" }
                ]
            },
        ],
    }).data("kendoGrid");
    grid.hideColumn("TenBP");
    //$("#span-title-table-cb-1").text(titleStr);
}
// Phương thức tạo bảng góp ý
function createGridFeedBack(urlStr) {
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
                    MaBP: { type: "number", validation: { required: true } },
                    TenBP: { type: "string", validation: { required: true } },
                    HoTen: { type: "string", validation: { required: true } },
                    MucDoDanhGia: { type: "string", validation: { required: true } },
                    GopY: { type: "string", validation: { required: true } },
                    SoLan: { type: "number", validation: { required: true } }
                }
            }
        },
        group: {
            field: "TenBP", aggregates: [
                { field: "HoTen", aggregate: "count" },
                { field: "SoLan", aggregate: "average" },
                { field: "SoLan", aggregate: "sum" },
                { field: "SoLan", aggregate: "min" },
                { field: "SoLan", aggregate: "max" }
            ]
        },

        aggregate: [
            { field: "HoTen", aggregate: "count" },
            { field: "SoLan", aggregate: "average" },
            { field: "SoLan", aggregate: "sum" },
            { field: "SoLan", aggregate: "min" },
            { field: "SoLan", aggregate: "max" }
        ]
    });

    var grid = $("#grid-report-2").kendoGrid({
        pdf: {
            allPages: true,
            avoidLinks: true,
            paperSize: "A4",
            margin: { top: "2cm", left: "1cm", right: "1cm", bottom: "1cm" },
            landscape: true,
            repeatHeaders: true,
            template: $("#page-template-feedback").html(),
            scale: 0.8
        },
        dataSource: dataSource,
        navigatable: true,
        pageable: true,
        columns: [
            { field: "MaCB", title: "Mã số", width: 80 },
            { field: "HoTen", title: "Họ tên", width: 200, footerTemplate: "Tổng cộng: #=count#", groupFooterTemplate: "Tổng: #=count#" },
            { field: "TenBP", title: "Bộ phận", width: 1 },
            { field: "MucDoDanhGia", title: "Mức độ đánh giá", width: 150 },
            { field: "GopY", title: "Góp ý", width: 0 },
            { field: "SoLan", title: "Số lần", width: 120, groupFooterTemplate: "<div>Tổng: #=sum#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>", footerTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #=Math.round(average*100)/100#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>" }
        ],
    }).data("kendoGrid");
    grid.hideColumn("TenBP");
    //$("#span-title-table-cb-1").text(titleStr);
}
// Phương thức click nút xuất báo cáo
$('#btn-report').on('click', function () {
    try {
        if ($('#grid-report-1').html().length > 0 && $('#grid-report-2').html().length > 0) {
            var grid1 = $('#grid-report-1').data('kendoGrid');
            var grid2 = $('#grid-report-2').data('kendoGrid');

            var progress = $.Deferred();
            kendo.drawing.drawDOM($("#title-bao-cao-1"))
                .done(function (header) {
                    grid1._drawPDF(progress)
                        .then(function (firstGrid) {
                            kendo.drawing.drawDOM($("#title-bao-cao-2"))
                                .done(function (footer) {
                                    grid2._drawPDF(progress)
                                        .then(function (secondGrid) {
                                            firstGrid.children.unshift(header);
                                            secondGrid.children.unshift(footer)
                                            secondGrid.children.forEach(function (x) {
                                                firstGrid.children.push(x);
                                            })
                                            return kendo.drawing.exportPDF(firstGrid, { multiPage: true });

                                        }).done(function (dataURI) {
                                            kendo.saveAs({
                                                dataURI: dataURI,
                                                fileName: "BaoCao-GopY.pdf"
                                            });
                                            progress.resolve();
                                        })
                                })
                        })
                })
        }
        else if ($('#grid-report-1').html().length > 0 && $('#grid-report-2').html().length == 0) {
            var grid1 = $('#grid-report-1').data('kendoGrid');

            var progress = $.Deferred();
            kendo.drawing.drawDOM($("#title-bao-cao-1"))
                .done(function (header) {
                    grid1._drawPDF(progress)
                        .then(function (firstGrid) {
                            firstGrid.children.unshift(header);
                            return kendo.drawing.exportPDF(firstGrid, { multiPage: true });

                        }).done(function (dataURI) {
                            kendo.saveAs({
                                dataURI: dataURI,
                                fileName: "BaoCao.pdf"
                            });
                            progress.resolve();
                        })
                })
        }
        else if ($('#grid-report-1').html().length == 0 && $('#grid-report-2').html().length > 0) {
            var grid2 = $('#grid-report-2').data('kendoGrid');

            var progress = $.Deferred();
            kendo.drawing.drawDOM($("#title-bao-cao-2"))
                .done(function (header) {
                    grid2._drawPDF(progress)
                        .then(function (firstGrid) {
                            firstGrid.children.unshift(header);
                            return kendo.drawing.exportPDF(firstGrid, { multiPage: true });

                        }).done(function (dataURI) {
                            kendo.saveAs({
                                dataURI: dataURI,
                                fileName: "GopY.pdf"
                            });
                            progress.resolve();
                        })
                })
        }
    } catch (ex) {
        alert("Yêu cầu phải xem trước hai bảng")
    }
})
createButtonReport();
createBPbaocao();
createTGbaocao();