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
    var day = new Date();
    $("#noi-dung-bao-cao").html('<div id = "grid-report-1" ></div ><div id="grid-report-2"></div><div id="grid-report-3"></div>');
    var start;
    var end;
    var title_main = '<div id="title-bao-cao-main" style="padding: 0.8em; font-size: 1.8em"><div>' +
        '<div style="width: 30%; text-align:center; float: left">' +
        '<h4>ỦY BAN NHÂN DÂN</h4>' +
        '<h4>QUẬN TÂN BÌNH</h4>' +
        '<div style="width: 30%;text-align:center;padding: 0 0 0 35%;">' +
        '<hr />' +
        '</div>' +
        '<h5 style="font-weight: 100; margin:0; padding: 0">Số: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;/BC-UBND </h5>' +
        '</div>' +
        '<div style="width: 40%;text-align:center;float: left;padding: 0 0 0 30%;">' +
        '<h4>CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</h4>' +
        '<h4>Độc lập - Tự do - Hạnh phúc</h4>' +
        '<div style="width: 20%; text-align: center; padding: 0 40% 0 40%;">' +
        '<hr />' +
        '</div>' +
        '<h5 style="font-weight: 100; margin:0; padding: 0">Quận Tân Bình, ngày &nbsp; ' + day.getDate() + ' &nbsp; tháng &nbsp;' + (day.getMonth() + 1) + '&nbsp; năm &nbsp;' + day.getFullYear() + '&nbsp; </h5>' +
        '</div>' +
        '<div style="clear:both; text-align: center">' +
        '<br /><h4>BÁO CÁO</h4>' +
        '<h4 style="font-weight: 100"> Tổng kết tình hình đánh giá chất lượng phục vụ <br /> của nhân viên bộ phận tiếp dân</h4>' +
        '</div>' +
        '</div></div> ';
    var title1 = "<div id='title-bao-cao-1' class='title-report'><h4>BẢNG KẾT QUẢ ĐÁNH GIÁ</h4>";
    var title2 = "<div id='title-bao-cao-2' class='title-report'><h4>BẢNG GÓP Ý</h4> ";
    var title3 = "<div id='title-bao-cao-3' class='title-report'><h4>BẢNG THỜI GIAN GIẢI QUYẾT THỦ TỤC</h4>";
    mabp_report = $("#bo-phan-bao-cao").val();
    if (mabp_report == 0) {
        macb_report = 0;
    }
    else {
        macb_report = $("#can-bo-bao-cao").val();
        title1 += "<h5 style='margin-left: 0'>" + $("#bo-phan-bao-cao").data("kendoDropDownList").text() + "</h5>";
        title2 += "<h5 style='margin-left: 0'>" + $("#bo-phan-bao-cao").data("kendoDropDownList").text() + "</h5>";
        title3 += "<h5 style='margin-left: 0'>" + $("#bo-phan-bao-cao").data("kendoDropDownList").text() + "</h5>";
        if (macb_report != 0) {
            title1 += "<div><b>Cán bộ: " + $("#can-bo-bao-cao").data("kendoDropDownList").text() + "</b></div>";
            title2 += "<div><b>Cán bộ: " + $("#can-bo-bao-cao").data("kendoDropDownList").text() + "</b></div>";
            title3 += "<div><b>Cán bộ: " + $("#can-bo-bao-cao").data("kendoDropDownList").text() + "</b></div>";
        }
    }
    if ($("#thoi-gian-bao-cao").val() == 0) {
        start = "";
        end = "";
        title1 += "<div>Tổng hợp</div>";
        title2 += "<div>Tổng hợp</div>";
        title3 += "<div>Tổng hợp</div>";
    } else {
        start = $("#start-bao-cao").val();
        end = $("#end-bao-cao").val();
        title1 += "<div>(" + start + " - " + end + ")</div>";
        title2 += "<div>(" + start + " - " + end + ")</div>";
        title3 += "<div>(" + start + " - " + end + ")</div>";
    }
    title1 += "</div>";
    title2 += "</div>";
    title3 += "</div>";
    if ($("#cb-kq").prop("checked")) {
        if (mabp_report == 0) {
            createGridReportTH(url + "/api/BaoCaoAPI/?_MaBP=" + mabp_report + "&_MaCB=" + macb_report + "&_Start=" + start + "&_End=" + end);
        } else if (macb_report == 0) {
            createGridReportBP(url + "/api/BaoCaoAPI/?_MaBP=" + mabp_report + "&_MaCB=" + macb_report + "&_Start=" + start + "&_End=" + end);
        } else if (macb_report > 0) {
            createGridReportCB(url + "/api/BaoCaoAPI/?_MaBP=" + mabp_report + "&_MaCB=" + macb_report + "&_Start=" + start + "&_End=" + end);
        }
        $("#grid-report-1 .k-grid-header").before(title1);
        //$("#title-bao-cao-1").html(title1);
    }
    if ($("#cb-gy").prop("checked")) {
        if (mabp_report == 0) {
            createGridFeedBackTH(url + "/api/BaoCaoAPI/?_MaBP=" + mabp_report + "&_MaCB=" + macb_report + "&_Start=" + start + "&_End=" + end + "&_GopY=GopY");
        } else if (macb_report == 0) {
            createGridFeedBackBP(url + "/api/BaoCaoAPI/?_MaBP=" + mabp_report + "&_MaCB=" + macb_report + "&_Start=" + start + "&_End=" + end + "&_GopY=GopY");
        } else if (macb_report > 0) {
            createGridFeedBackCB(url + "/api/BaoCaoAPI/?_MaBP=" + mabp_report + "&_MaCB=" + macb_report + "&_Start=" + start + "&_End=" + end + "&_GopY=GopY");
        }
        $("#grid-report-2 .k-grid-header").before(title2);
        //$("#title-bao-cao-2").html(title2);
    }
    if ($("#cb-tt").prop("checked")) {
        if (mabp_report == 0) {
            createGridThuTucReportTH(url + "/api/BaoCaoAPI/?_MaBP=" + mabp_report + "&_MaCB=" + macb_report + "&_Start=" + start + "&_End=" + end + "&_Phien=Phien");
        } else if (macb_report == 0) {
            createGridThuTucReportBP(url + "/api/BaoCaoAPI/?_MaBP=" + mabp_report + "&_MaCB=" + macb_report + "&_Start=" + start + "&_End=" + end + "&_Phien=Phien");
        } else if (macb_report > 0) {
            createGridThuTucReportCB(url + "/api/BaoCaoAPI/?_MaBP=" + mabp_report + "&_MaCB=" + macb_report + "&_Start=" + start + "&_End=" + end + "&_Phien=Phien");
        }
        $("#grid-report-3 .k-grid-header").before(title3);
        //$("#title-bao-cao-3").html(title3);
    }
    if ($("#cb-kq").prop("checked")) {
        $("#grid-report-1 #title-bao-cao-1").before(title_main);
    } else if ($("#cb-gy").prop("checked")) {
        $("#grid-report-2 #title-bao-cao-2").before(title_main);
    } else if ($("#cb-tt").prop("checked")) {
        $("#grid-report-3 #title-bao-cao-3").before(title_main);
    }
    //$("#title-bao-cao-main").html(title_main);
}
// Phương thức tạo bảng thời gian giải quyết thủ tục tổng hợp
function createGridThuTucReportTH(urlStr) {
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
                id: "MaBP",
                fields: {
                    MaBP: { type: "number" },
                    VietTat: { type: "string", validation: { required: true } },
                    TenBP: { type: "string", validation: { required: true } },
                    PhienCho: { type: "number", validation: { required: true } },
                    PhienXuLy: { type: "number", validation: { required: true } },
                    SoLuong: { type: "number", validation: { required: true } },
                    TongPhien: { type: "number", validation: { required: true } }
                }
            }
        },
        //group: {
        //    field: "TenBP", aggregates: [
        //        { field: "HoTen", aggregate: "count" },
        //        { field: "PhienCho", aggregate: "average" },
        //        { field: "PhienCho", aggregate: "sum" },
        //        { field: "PhienCho", aggregate: "min" },
        //        { field: "PhienCho", aggregate: "max" },
        //        { field: "PhienXuLy", aggregate: "average" },
        //        { field: "PhienXuLy", aggregate: "sum" },
        //        { field: "PhienXuLy", aggregate: "min" },
        //        { field: "PhienXuLy", aggregate: "max" },
        //        { field: "TongPhien", aggregate: "average" },
        //        { field: "TongPhien", aggregate: "sum" },
        //        { field: "TongPhien", aggregate: "min" },
        //        { field: "TongPhien", aggregate: "max" }
        //    ]
        //},

        aggregate: [
            { field: "TenBP", aggregate: "count" },
            { field: "PhienCho", aggregate: "average" },
            { field: "PhienCho", aggregate: "sum" },
            { field: "PhienCho", aggregate: "min" },
            { field: "PhienCho", aggregate: "max" },
            { field: "PhienXuLy", aggregate: "average" },
            { field: "PhienXuLy", aggregate: "sum" },
            { field: "PhienXuLy", aggregate: "min" },
            { field: "PhienXuLy", aggregate: "max" },
            { field: "TongPhien", aggregate: "average" },
            { field: "TongPhien", aggregate: "sum" },
            { field: "TongPhien", aggregate: "min" },
            { field: "TongPhien", aggregate: "max" },
            { field: "SoLuong", aggregate: "average" },
            { field: "SoLuong", aggregate: "sum" },
            { field: "SoLuong", aggregate: "min" },
            { field: "SoLuong", aggregate: "max" }
        ]
    });

    var grid = $("#grid-report-3").kendoGrid({
        pdf: {
            allPages: true,
            avoidLinks: true,
            paperSize: "A4",
            margin: { top: "2cm", left: "1cm", right: "1cm", bottom: "1cm" },
            landscape: true,
            repeatHeaders: true,
            template: $("#page-template-thutuc").html(),
            scale: 0.6
        },
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
            { field: "VietTat", title: "Mã bộ phận", width: 100 },
            { field: "TenBP", title: "Tên bộ phận", width: 200, footerTemplate: "Tổng cộng: #=count#", groupFooterTemplate: "Tổng: #=count#" },
            //{ hidden: true, field: "TenBP", title: "Bộ phận", width: 1 },
            { field: "PhienCho", title: "Thời gian chờ trung bình (Phút)", width: 200, groupFooterTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #=Math.round(average*100)/100#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>", footerTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #=Math.round(average*100)/100#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>" },
            { field: "PhienXuLy", title: "Thời gian xử lý trung bình (Phút)", width: 200, groupFooterTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #=Math.round(average*100)/100#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>", footerTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #=Math.round(average*100)/100#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>" },
            { field: "TongPhien", title: "Tổng thời gian trung bình (Phút)", width: 200, groupFooterTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #=Math.round(average*100)/100#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>", footerTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #=Math.round(average*100)/100#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>" },
            { field: "SoLuong", title: "Tổng số lượng (Lần)", width: 100, groupFooterTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #=Math.round(average*100)/100#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>", footerTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #=Math.round(average*100)/100#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>" },
        ],
    }).data("kendoGrid");
    //grid.hideColumn("TenBP");
    //$("#span-title-table-cb-1").text(titleStr);
}
// Phương thức tạo bảng thời gian giải quyết thủ tục bộ phận
function createGridThuTucReportBP(urlStr) {
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
                    MaCBSD: { type: "string", validation: { required: true } },
                    TenBP: { type: "string", validation: { required: true } },
                    HoTen: { type: "string", validation: { required: true } },
                    PhienXuLy: { type: "number", validation: { required: true } },
                    SoLuong: { type: "number", validation: { required: true } },
                }
            }
        },
        group: { field: "TenBP" },
        //    field: "TenBP", aggregates: [
        //        { field: "HoTen", aggregate: "count" },
        //        { field: "PhienCho", aggregate: "average" },
        //        { field: "PhienCho", aggregate: "sum" },
        //        { field: "PhienCho", aggregate: "min" },
        //        { field: "PhienCho", aggregate: "max" },
        //        { field: "PhienXuLy", aggregate: "average" },
        //        { field: "PhienXuLy", aggregate: "sum" },
        //        { field: "PhienXuLy", aggregate: "min" },
        //        { field: "PhienXuLy", aggregate: "max" },
        //        { field: "TongPhien", aggregate: "average" },
        //        { field: "TongPhien", aggregate: "sum" },
        //        { field: "TongPhien", aggregate: "min" },
        //        { field: "TongPhien", aggregate: "max" }
        //    ]
        //},

        aggregate: [
            { field: "HoTen", aggregate: "count" },
            { field: "PhienXuLy", aggregate: "average" },
            { field: "PhienXuLy", aggregate: "sum" },
            { field: "PhienXuLy", aggregate: "min" },
            { field: "PhienXuLy", aggregate: "max" },
            { field: "SoLuong", aggregate: "average" },
            { field: "SoLuong", aggregate: "sum" },
            { field: "SoLuong", aggregate: "min" },
            { field: "SoLuong", aggregate: "max" }
        ]
    });

    var grid = $("#grid-report-3").kendoGrid({
        pdf: {
            allPages: true,
            avoidLinks: true,
            paperSize: "A4",
            margin: { top: "2cm", left: "1cm", right: "1cm", bottom: "1cm" },
            landscape: true,
            repeatHeaders: true,
            template: $("#page-template-thutuc").html(),
            scale: 0.6
        },
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
            { hidden: true, field: "TenBP", title: "Bộ phận", width: 200 },
            { field: "MaCBSD", title: "Mã số", width: 100 },
            { field: "HoTen", title: "Họ tên", width: 200, footerTemplate: "Tổng cộng: #=count#" },
            {
                field: "PhienXuLy", title: "Thời gian xử lý trung bình (Phút)", width: 200,
                footerTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #=Math.round(average*100)/100#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>"
            },
            {
                field: "SoLuong", title: "Tổng số lượng (Lần)", width: 100,
                footerTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #=Math.round(average*100)/100#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>"
            },
        ],
    }).data("kendoGrid");
    //grid.hideColumn("TenBP");
    //$("#span-title-table-cb-1").text(titleStr);
}
// Phương thức tạo bảng thời gian giải quyết thủ tục cán bộ
function createGridThuTucReportCB(urlStr) {
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
                    MaCBSD: { type: "string", validation: { required: true } },
                    TenBP: { type: "string", validation: { required: true } },
                    HoTen: { type: "string", validation: { required: true } },
                    STT: { type: "number", validation: { required: true } },
                    PhienXuLy: { type: "number", validation: { required: true } },
                }
            }
        },
        group: { field: "HoTen" },
        //    field: "TenBP", aggregates: [
        //        { field: "HoTen", aggregate: "count" },
        //        { field: "PhienCho", aggregate: "average" },
        //        { field: "PhienCho", aggregate: "sum" },
        //        { field: "PhienCho", aggregate: "min" },
        //        { field: "PhienCho", aggregate: "max" },
        //        { field: "PhienXuLy", aggregate: "average" },
        //        { field: "PhienXuLy", aggregate: "sum" },
        //        { field: "PhienXuLy", aggregate: "min" },
        //        { field: "PhienXuLy", aggregate: "max" },
        //        { field: "TongPhien", aggregate: "average" },
        //        { field: "TongPhien", aggregate: "sum" },
        //        { field: "TongPhien", aggregate: "min" },
        //        { field: "TongPhien", aggregate: "max" }
        //    ]
        //},

        aggregate: [
            { field: "STT", aggregate: "count" },
            { field: "PhienXuLy", aggregate: "average" },
            { field: "PhienXuLy", aggregate: "sum" },
            { field: "PhienXuLy", aggregate: "min" },
            { field: "PhienXuLy", aggregate: "max" },
        ]
    });

    var grid = $("#grid-report-3").kendoGrid({
        pdf: {
            allPages: true,
            avoidLinks: true,
            paperSize: "A4",
            margin: { top: "2cm", left: "1cm", right: "1cm", bottom: "1cm" },
            landscape: true,
            repeatHeaders: true,
            template: $("#page-template-thutuc").html(),
            scale: 0.6
        },
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
            //{ field: "TenBP", title: "Bộ phận", width: 200 },
            //{ field: "MaCBSD", title: "Mã số", width: 100 },
            { field: "STT", title: "Số thứ tự", width: 100, footerTemplate: "Tổng cộng: #=count#" },
            { hidden: true, field: "HoTen", title: "Họ tên", width: 200 },
            {
                field: "PhienXuLy", title: "Thời gian xử lý trung bình (Phút)", width: 200,
                footerTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #=Math.round(average*100)/100#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>"
            },
        ],
    }).data("kendoGrid");
    //grid.hideColumn("TenBP");
    //$("#span-title-table-cb-1").text(titleStr);
}

// Phương thức tạo bảng kết quả báo cáo tổng hợp
function createGridReportTH(urlStr) {
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
                    MaCBSD: { type: "string", validation: { required: true } },
                    MaBP: { type: "number", validation: { required: true } },
                    TenBP: { type: "string", validation: { required: true } },
                    HoTen: { type: "string", validation: { required: true } },
                    Diem: { type: "number", validation: { required: true } },
                    XepLoai: { type: "string", validation: { required: true } },
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
            scale: 0.6
        },
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
            { field: "MaCBSD", title: "Mã số", width: 50 },
            { field: "HoTen", title: "Họ tên", width: 90, footerTemplate: "Tổng cộng: #=count#", groupFooterTemplate: "Tổng: #=count#" },
            {
                field: "Diem", title: "Số điểm", width: 80,
                groupFooterTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>",
                footerTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>"
            },
            { field: "XepLoai", title: "Xếp loại", width: 100 },
            { hidden: true, field: "TenBP", title: "Bộ phận", width: 1, },
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
    //grid.hideColumn("TenBP");
    //$("#span-title-table-cb-1").text(titleStr);
}
// Phương thức tạo bảng kết quả báo cáo bộ phận
function createGridReportBP(urlStr) {
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
                    MaCBSD: { type: "string", validation: { required: true } },
                    MaBP: { type: "number", validation: { required: true } },
                    TenBP: { type: "string", validation: { required: true } },
                    HoTen: { type: "string", validation: { required: true } },
                    Diem: { type: "number", validation: { required: true } },
                    XepLoai: { type: "string", validation: { required: true } },
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
            field: "HoTen", aggregates: [
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
            scale: 0.6
        },
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
            { field: "MaCBSD", title: "Mã số", width: 50 },
            { hidden: true, field: "HoTen", title: "Họ tên", width: 90 },
            {
                field: "Diem", title: "Số điểm", width: 80,
                footerTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>"
            },
            { field: "XepLoai", title: "Xếp loại", width: 100 },
            { hidden: true, field: "TenBP", title: "Bộ phận", width: 1, },
            {
                title: "Rất hài lòng",
                columns: [
                    {
                        field: "RHL_TyLe", title: "Tỷ lệ(%)", width: 70,
                        footerTemplate: "Trung bình: #=Math.round((sum/count)*100)/100#%"
                    },
                    {
                        field: "RHL_SoLan", title: "Số lần", width: 70,
                        footerTemplate: "<div>Tổng: #=sum#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>"
                    }
                ]
            },
            {
                title: "Hài lòng",
                columns: [
                    {
                        field: "HL_TyLe", title: "Tỷ lệ(%)", width: 70,
                        footerTemplate: "Trung bình: #=Math.round((sum/count)*100)/100#%"
                    },
                    {
                        field: "HL_SoLan", title: "Số lần", width: 70,
                        footerTemplate: "<div>Tổng: #=sum#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>"
                    }
                ]
            },
            {
                title: "Bình thường",
                columns: [
                    {
                        field: "BT_TyLe", title: "Tỷ lệ(%)", width: 70,
                        footerTemplate: "Trung bình: #=Math.round((sum/count)*100)/100#%"
                    },
                    {
                        field: "BT_SoLan", title: "Số lần", width: 70,
                        footerTemplate: "<div>Tổng: #=sum#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>"
                    }
                ]
            },
            {
                title: "Không hài lòng",
                columns: [
                    {
                        field: "KHL_TyLe", title: "Tỷ lệ(%)", width: 70,
                        footerTemplate: "Trung bình: #=Math.round((sum/count)*100)/100#%"
                    },
                    {
                        field: "KHL_SoLan", title: "Số lần", width: 70,
                        footerTemplate: "<div>Tổng: #=sum#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>"
                    }
                ]
            },
            {
                title: "Tổng hợp",
                columns: [
                    {
                        field: "TongCong_TyLe", title: "Tỷ lệ(%)", width: 70,
                        footerTemplate: "Trung bình: #=Math.round((sum/count)*100)/100#%"
                    },
                    {
                        field: "TongCong_SoLan", title: "Số lần", width: 70,
                        footerTemplate: "<div>Tổng: #=sum#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>"
                    }
                ]
            },
        ],
    }).data("kendoGrid");
    //grid.hideColumn("TenBP");
    //$("#span-title-table-cb-1").text(titleStr);
}
// Phương thức tạo bảng kết quả báo cáo cán bộ
function createGridReportCB(urlStr) {
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
                    MaCBSD: { type: "string", validation: { required: true } },
                    MaBP: { type: "number", validation: { required: true } },
                    TenBP: { type: "string", validation: { required: true } },
                    HoTen: { type: "string", validation: { required: true } },
                    Diem: { type: "number", validation: { required: true } },
                    XepLoai: { type: "string", validation: { required: true } },
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
        //group: {
        //    field: "HoTen", aggregates: [
        //        { field: "HoTen", aggregate: "count" },
        //        { field: "RHL_TyLe", aggregate: "average" },
        //        { field: "RHL_SoLan", aggregate: "sum" },
        //        { field: "HL_TyLe", aggregate: "average" },
        //        { field: "HL_SoLan", aggregate: "sum" },
        //        { field: "BT_TyLe", aggregate: "average" },
        //        { field: "BT_SoLan", aggregate: "sum" },
        //        { field: "KHL_TyLe", aggregate: "average" },
        //        { field: "KHL_SoLan", aggregate: "sum" },
        //        { field: "TongCong_TyLe", aggregate: "average" },
        //        { field: "TongCong_SoLan", aggregate: "sum" },
        //        { field: "RHL_TyLe", aggregate: "min" },
        //        { field: "RHL_SoLan", aggregate: "min" },
        //        { field: "HL_TyLe", aggregate: "min" },
        //        { field: "HL_SoLan", aggregate: "min" },
        //        { field: "BT_TyLe", aggregate: "min" },
        //        { field: "BT_SoLan", aggregate: "min" },
        //        { field: "KHL_TyLe", aggregate: "min" },
        //        { field: "KHL_SoLan", aggregate: "min" },
        //        { field: "TongCong_TyLe", aggregate: "min" },
        //        { field: "TongCong_SoLan", aggregate: "min" },
        //        { field: "RHL_TyLe", aggregate: "max" },
        //        { field: "RHL_SoLan", aggregate: "max" },
        //        { field: "HL_TyLe", aggregate: "max" },
        //        { field: "HL_SoLan", aggregate: "max" },
        //        { field: "BT_TyLe", aggregate: "max" },
        //        { field: "BT_SoLan", aggregate: "max" },
        //        { field: "KHL_TyLe", aggregate: "max" },
        //        { field: "KHL_SoLan", aggregate: "max" },
        //        { field: "TongCong_TyLe", aggregate: "max" },
        //        { field: "TongCong_SoLan", aggregate: "max" },
        //        { field: "Diem", aggregate: "average" },
        //        { field: "Diem", aggregate: "sum" },
        //        { field: "Diem", aggregate: "min" },
        //        { field: "Diem", aggregate: "max" },
        //    ]
        //},

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
            scale: 0.6
        },
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
            { field: "MaCBSD", title: "Mã số", width: 50 },
            { field: "HoTen", title: "Họ tên", width: 90 },
            { field: "Diem", title: "Số điểm", width: 80 },
            { field: "XepLoai", title: "Xếp loại", width: 100 },
            { hidden: true, field: "TenBP", title: "Bộ phận", width: 1, },
            {
                title: "Rất hài lòng",
                columns: [
                    {
                        field: "RHL_TyLe", title: "Tỷ lệ(%)", width: 70,
                    },
                    {
                        field: "RHL_SoLan", title: "Số lần", width: 70,
                    }
                ]
            },
            {
                title: "Hài lòng",
                columns: [
                    {
                        field: "HL_TyLe", title: "Tỷ lệ(%)", width: 70,
                    },
                    {
                        field: "HL_SoLan", title: "Số lần", width: 70,
                    }
                ]
            },
            {
                title: "Bình thường",
                columns: [
                    {
                        field: "BT_TyLe", title: "Tỷ lệ(%)", width: 70,
                    },
                    {
                        field: "BT_SoLan", title: "Số lần", width: 70,
                    }
                ]
            },
            {
                title: "Không hài lòng",
                columns: [
                    {
                        field: "KHL_TyLe", title: "Tỷ lệ(%)", width: 70,
                    },
                    {
                        field: "KHL_SoLan", title: "Số lần", width: 70,
                    }
                ]
            },
            {
                title: "Tổng hợp",
                columns: [
                    {
                        field: "TongCong_TyLe", title: "Tỷ lệ(%)", width: 70,
                    },
                    {
                        field: "TongCong_SoLan", title: "Số lần", width: 70,
                    }
                ]
            },
        ],
    }).data("kendoGrid");
    //grid.hideColumn("TenBP");
    //$("#span-title-table-cb-1").text(titleStr);
}


// Phương thức tạo bảng góp ý tổng hợp
function createGridFeedBackTH(urlStr) {
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
                    MaCBSD: { type: "string", validation: { required: true } },
                    MaBP: { type: "number", validation: { required: true } },
                    TenBP: { type: "string", validation: { required: true } },
                    HoTen: { type: "string", validation: { required: true } },
                    MucDoDanhGia: { type: "string", validation: { required: true } },
                    GopY: { type: "string", validation: { required: true } },
                    SoLan: { type: "number", validation: { required: true } }
                }
            }
        },
        group: [{
            field: "TenBP", aggregates: [
                { field: "HoTen", aggregate: "count" },
                { field: "SoLan", aggregate: "average" },
                { field: "SoLan", aggregate: "sum" },
                { field: "SoLan", aggregate: "min" },
                { field: "SoLan", aggregate: "max" }
            ]
        }, {
            field: "HoTen", aggregates: [
                { field: "HoTen", aggregate: "count" },
                { field: "SoLan", aggregate: "average" },
                { field: "SoLan", aggregate: "sum" },
                { field: "SoLan", aggregate: "min" },
                { field: "SoLan", aggregate: "max" }
            ]
        }],

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
            scale: 0.6
        },
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
            //{ field: "MaCBSD", title: "Mã số", width: 80 },
            { hidden: true, field: "HoTen", title: "Họ tên", width: 200, footerTemplate: "Tổng cộng: #=count#", groupFooterTemplate: "Tổng: #=count#" },
            { hidden: true, field: "TenBP", title: "Bộ phận", width: 1 },
            { field: "MucDoDanhGia", title: "Mức độ đánh giá", width: 150 },
            { field: "GopY", title: "Góp ý", width: 0 },
            {
                field: "SoLan", title: "Số lần", width: 120,
                groupFooterTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #=Math.round(average*100)/100#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>",
                footerTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #=Math.round(average*100)/100#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>"
            }
        ],
    }).data("kendoGrid");
    //grid.hideColumn("TenBP");
    //$("#span-title-table-cb-1").text(titleStr);
}
// Phương thức tạo bảng góp ý bộ phận
function createGridFeedBackBP(urlStr) {
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
                    MaCBSD: { type: "string", validation: { required: true } },
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
            field: "HoTen", aggregates: [
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
            scale: 0.6
        },
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
            //{ field: "MaCBSD", title: "Mã số", width: 80 },
            { hidden: true, field: "HoTen", title: "Họ tên", width: 200, footerTemplate: "Tổng cộng: #=count#", groupFooterTemplate: "Tổng: #=count#" },
            { hidden: true, field: "TenBP", title: "Bộ phận", width: 1 },
            { field: "MucDoDanhGia", title: "Mức độ đánh giá", width: 150 },
            { field: "GopY", title: "Góp ý", width: 0 },
            {
                field: "SoLan", title: "Số lần", width: 120,
                groupFooterTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #=Math.round(average*100)/100#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>",
                footerTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #=Math.round(average*100)/100#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>"
            }
        ],
    }).data("kendoGrid");
    //grid.hideColumn("TenBP");
    //$("#span-title-table-cb-1").text(titleStr);
}
// Phương thức tạo bảng góp ý
function createGridFeedBackCB(urlStr) {
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
                    MaCBSD: { type: "string", validation: { required: true } },
                    MaBP: { type: "number", validation: { required: true } },
                    TenBP: { type: "string", validation: { required: true } },
                    HoTen: { type: "string", validation: { required: true } },
                    MucDoDanhGia: { type: "string", validation: { required: true } },
                    GopY: { type: "string", validation: { required: true } },
                    SoLan: { type: "number", validation: { required: true } }
                }
            }
        },
        //group: [{
        //    field: "TenBP", aggregates: [
        //        { field: "HoTen", aggregate: "count" },
        //        { field: "SoLan", aggregate: "average" },
        //        { field: "SoLan", aggregate: "sum" },
        //        { field: "SoLan", aggregate: "min" },
        //        { field: "SoLan", aggregate: "max" }
        //    ]
        //}, {
        //    field: "HoTen", aggregates: [
        //        { field: "HoTen", aggregate: "count" },
        //        { field: "SoLan", aggregate: "average" },
        //        { field: "SoLan", aggregate: "sum" },
        //        { field: "SoLan", aggregate: "min" },
        //        { field: "SoLan", aggregate: "max" }
        //    ]
        //}],

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
            scale: 0.6
        },
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
            //{ field: "MaCBSD", title: "Mã số", width: 80 },
            { hidden: true, field: "HoTen", title: "Họ tên", width: 200, footerTemplate: "Tổng cộng: #=count#" },
            { hidden: true, field: "TenBP", title: "Bộ phận", width: 1 },
            { field: "MucDoDanhGia", title: "Mức độ đánh giá", width: 150 },
            { field: "GopY", title: "Góp ý", width: 0 },
            {
                field: "SoLan", title: "Số lần", width: 120,
                footerTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #=Math.round(average*100)/100#</div><div>Thấp nhất: #= min #</div><div>Cao nhất: #= max #</div>"
            }
        ],
    }).data("kendoGrid");
    //grid.hideColumn("TenBP");
    //$("#span-title-table-cb-1").text(titleStr);
}
// Phương thức click nút xuất báo cáo
$('#btn-report').on('click', function () {
    try {
        //$("#title-bao-cao-main, #title-bao-cao-1, #title-bao-cao-2, #title-bao-cao-3").addClass("title-report-export");
        if ($('#grid-report-1').html().length > 0 && $('#grid-report-2').html().length > 0 && $('#grid-report-3').html().length > 0) {
            var grid1 = $('#grid-report-1').data('kendoGrid');
            var grid2 = $('#grid-report-2').data('kendoGrid');
            var grid3 = $('#grid-report-3').data('kendoGrid');

            var progress = $.Deferred();
            kendo.drawing.drawDOM($("#title-bao-cao-main"))
                .done(function (title) {
                    kendo.drawing.drawDOM($("#title-bao-cao-1"))
                        .done(function (header) {
                            grid1._drawPDF(progress)
                                .then(function (firstGrid) {
                                    kendo.drawing.drawDOM($("#title-bao-cao-2"))
                                        .done(function (body) {
                                            grid2._drawPDF(progress)
                                                .then(function (secondGrid) {
                                                    kendo.drawing.drawDOM($("#title-bao-cao-3"))
                                                        .done(function (footer) {
                                                            grid3._drawPDF(progress)
                                                                .then(function (thirdGrid) {
                                                                    //firstGrid.children.unshift(header);
                                                                    //firstGrid.children.unshift(title);
                                                                    //secondGrid.children.unshift(body);
                                                                    //thirdGrid.children.unshift(footer);
                                                                    secondGrid.children.forEach(function (x) {
                                                                        firstGrid.children.push(x);
                                                                    })
                                                                    thirdGrid.children.forEach(function (x) {
                                                                        firstGrid.children.push(x);
                                                                    })
                                                                    return kendo.drawing.exportPDF(firstGrid, { multiPage: true });

                                                                }).done(function (dataURI) {
                                                                    kendo.saveAs({
                                                                        dataURI: dataURI,
                                                                        fileName: "BaoCao-GopY-ThuTuc.pdf"
                                                                    });
                                                                    progress.resolve();
                                                                })
                                                        })
                                                })
                                        })
                                })
                        })
                })
        }
        else if ($('#grid-report-1').html().length > 0 && $('#grid-report-2').html().length == 0 && $('#grid-report-3').html().length == 0) {
            var grid1 = $('#grid-report-1').data('kendoGrid');

            var progress = $.Deferred();
            kendo.drawing.drawDOM($("#title-bao-cao-main"))
                .done(function (title) {
                    kendo.drawing.drawDOM($("#title-bao-cao-1"))
                        .done(function (header) {
                            grid1._drawPDF(progress)
                                .then(function (firstGrid) {
                                    //firstGrid.children.unshift(header);
                                    //firstGrid.children.unshift(title);
                                    return kendo.drawing.exportPDF(firstGrid, { multiPage: true });

                                }).done(function (dataURI) {
                                    kendo.saveAs({
                                        dataURI: dataURI,
                                        fileName: "BaoCao.pdf"
                                    });
                                    progress.resolve();
                                })
                        })
                })
        }
        else if ($('#grid-report-1').html().length == 0 && $('#grid-report-2').html().length > 0 && $('#grid-report-3').html().length == 0) {
            var grid2 = $('#grid-report-2').data('kendoGrid');

            var progress = $.Deferred();
            kendo.drawing.drawDOM($("#title-bao-cao-main"))
                .done(function (title) {
                    kendo.drawing.drawDOM($("#title-bao-cao-2"))
                        .done(function (header) {
                            grid2._drawPDF(progress)
                                .then(function (firstGrid) {
                                    //firstGrid.children.unshift(header);
                                    //firstGrid.children.unshift(title);
                                    return kendo.drawing.exportPDF(firstGrid, { multiPage: true });

                                }).done(function (dataURI) {
                                    kendo.saveAs({
                                        dataURI: dataURI,
                                        fileName: "GopY.pdf"
                                    });
                                    progress.resolve();
                                })
                        })
                })
        }
        else if ($('#grid-report-1').html().length == 0 && $('#grid-report-2').html().length == 0 && $('#grid-report-3').html().length > 0) {
            var grid3 = $('#grid-report-3').data('kendoGrid');

            var progress = $.Deferred();
            kendo.drawing.drawDOM($("#title-bao-cao-main"))
                .done(function (title) {
                    kendo.drawing.drawDOM($("#title-bao-cao-3"))
                        .done(function (header) {
                            grid3._drawPDF(progress)
                                .then(function (firstGrid) {
                                    //firstGrid.children.unshift(header);
                                    //firstGrid.children.unshift(title);
                                    return kendo.drawing.exportPDF(firstGrid, { multiPage: true });

                                }).done(function (dataURI) {
                                    kendo.saveAs({
                                        dataURI: dataURI,
                                        fileName: "ThuTuc.pdf"
                                    });
                                    progress.resolve();
                                })
                        })
                })
        }
        else if ($('#grid-report-1').html().length > 0 && $('#grid-report-2').html().length > 0 && $('#grid-report-3').html().length == 0) {
            var grid1 = $('#grid-report-1').data('kendoGrid');
            var grid2 = $('#grid-report-2').data('kendoGrid');

            var progress = $.Deferred();
            kendo.drawing.drawDOM($("#title-bao-cao-main"))
                .done(function (title) {
                    kendo.drawing.drawDOM($("#title-bao-cao-1"))
                        .done(function (header) {
                            grid1._drawPDF(progress)
                                .then(function (firstGrid) {
                                    kendo.drawing.drawDOM($("#title-bao-cao-2"))
                                        .done(function (body) {
                                            grid2._drawPDF(progress)
                                                .then(function (secondGrid) {
                                                    //firstGrid.children.unshift(header);
                                                    //firstGrid.children.unshift(title);
                                                    //secondGrid.children.unshift(body);
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
                })
        }
        else if ($('#grid-report-1').html().length > 0 && $('#grid-report-2').html().length == 0 && $('#grid-report-3').html().length > 0) {
            var grid1 = $('#grid-report-1').data('kendoGrid');
            var grid3 = $('#grid-report-3').data('kendoGrid');

            var progress = $.Deferred();
            kendo.drawing.drawDOM($("#title-bao-cao-main"))
                .done(function (title) {
                    kendo.drawing.drawDOM($("#title-bao-cao-1"))
                        .done(function (header) {
                            grid1._drawPDF(progress)
                                .then(function (firstGrid) {
                                    kendo.drawing.drawDOM($("#title-bao-cao-3"))
                                        .done(function (body) {
                                            grid3._drawPDF(progress)
                                                .then(function (secondGrid) {
                                                    //firstGrid.children.unshift(header);
                                                    //firstGrid.children.unshift(title);
                                                    //secondGrid.children.unshift(body);
                                                    secondGrid.children.forEach(function (x) {
                                                        firstGrid.children.push(x);
                                                    })
                                                    return kendo.drawing.exportPDF(firstGrid, { multiPage: true });

                                                }).done(function (dataURI) {
                                                    kendo.saveAs({
                                                        dataURI: dataURI,
                                                        fileName: "BaoCao-ThuTuc.pdf"
                                                    });
                                                    progress.resolve();
                                                })
                                        })
                                })
                        })
                })
        }
        else if ($('#grid-report-1').html().length == 0 && $('#grid-report-2').html().length > 0 && $('#grid-report-3').html().length > 0) {
            var grid2 = $('#grid-report-2').data('kendoGrid');
            var grid3 = $('#grid-report-3').data('kendoGrid');

            var progress = $.Deferred();
            kendo.drawing.drawDOM($("#title-bao-cao-main"))
                .done(function (title) {
                    kendo.drawing.drawDOM($("#title-bao-cao-2"))
                        .done(function (header) {
                            grid2._drawPDF(progress)
                                .then(function (firstGrid) {
                                    kendo.drawing.drawDOM($("#title-bao-cao-3"))
                                        .done(function (body) {
                                            grid3._drawPDF(progress)
                                                .then(function (secondGrid) {
                                                    //firstGrid.children.unshift(header);
                                                    //firstGrid.children.unshift(title);
                                                    //secondGrid.children.unshift(body);
                                                    secondGrid.children.forEach(function (x) {
                                                        firstGrid.children.push(x);
                                                    })
                                                    return kendo.drawing.exportPDF(firstGrid, { multiPage: true });

                                                }).done(function (dataURI) {
                                                    kendo.saveAs({
                                                        dataURI: dataURI,
                                                        fileName: "GopY-ThuTuc.pdf"
                                                    });
                                                    progress.resolve();
                                                })
                                        })
                                })
                        })
                })
        }
        //setTimeout(function () {
        //    $("#title-bao-cao-main, #title-bao-cao-1, #title-bao-cao-2, #title-bao-cao-3").removeClass("title-report-export");
        //}, 10000);
    } catch (ex) {
        console.log(ex);
        alert("Yêu cầu phải xem trước")
    }
})
$("#menu-thong-ke-bao-cao").click(function () {
    createButtonReport();
    createBPbaocao();
    createTGbaocao();
})