// Tạo dropdownlist bộ phận
function createBPDangNhap() {
    $.ajax({
        type: "GET",
        url: url + "/api/BoPhanAPI",
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.length > 0) {
                var arr = [];
                for (var i = 0; i < data.length; i++) {
                    arr[i] = { text: data[i]["TenBP"], value: data[i]["MaBP"] };
                }
                $("#bo-phan-dang-nhap").kendoDropDownList({
                    dataTextField: "text",
                    dataValueField: "value",
                    dataSource: arr
                });
                createCBDangNhap($("#bo-phan-dang-nhap").val());
            } else { }
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    })
}
// Tạo sự kiện chọn dropdownlist bộ phận
$("#bo-phan-dang-nhap").change(function () {
    createCBDangNhap($("#bo-phan-dang-nhap").val())
})
// Tạo dropdownlist cán bộ
function createCBDangNhap(MaBP) {
    $.ajax({
        type: "GET",
        url: url + "/api/BoPhanAPI/?_MaBP=" + MaBP,
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.length > 0) {
                var arr = [];
                for (var i = 0; i < data.length; i++) {
                    arr[i] = { text: data[i]["HoTen"], value: data[i]["MaCB"] };
                }
                $("#can-bo-dang-nhap").kendoDropDownList({
                    dataTextField: "text",
                    dataValueField: "value",
                    dataSource: arr
                });
                createMonthDangNhap($("#can-bo-dang-nhap").val());
                createYearDangNhap($("#can-bo-dang-nhap").val());
            } else { }
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    })
}
// Tạo sự kiện chọn dropdownlist cán bộ
$("#can-bo-dang-nhap").change(function () {
    createYearDangNhap($("#can-bo-dang-nhap").val());
    createMonthDangNhap($("#can-bo-dang-nhap").val());
})
// Tạo grid đăng nhập theo tháng
function createGridDangNhapThang(MaCB, Loai, ThoiGian) {
    $("#grid-dang-nhap").html("");
    dataSource = new kendo.data.DataSource({
        transport: {
            serverFiltering: true,
            read: function (options) {
                $.ajax({
                    type: "GET",
                    url: url + "/api/DangNhapAPI/?_MaCB=" + MaCB + "&_Loai=" + Loai + "&_ThoiGian=" + ThoiGian,
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
                    MaMay: { type: "number", validation: { required: true } },
                    Ngay: { type: "date", validation: { required: true } },
                    BD: { type: "date", validation: { required: true } },
                    KT: { type: "date", validation: { required: true } },
                    ThoiGian: { type: "number", validation: { required: true } }
                }
            }
        },
        group: {
            field: "Ngay", aggregates: [
                { field: "MaMay", aggregate: "count" },
                { field: "ThoiGian", aggregate: "average" },
                { field: "ThoiGian", aggregate: "sum" },
                { field: "ThoiGian", aggregate: "min" },
                { field: "ThoiGian", aggregate: "max" }
            ]
        },

        aggregate: [
            { field: "MaMay", aggregate: "count" },
            { field: "ThoiGian", aggregate: "average" },
            { field: "ThoiGian", aggregate: "sum" },
            { field: "ThoiGian", aggregate: "min" },
            { field: "ThoiGian", aggregate: "max" }
        ]
    });

    var grid = $("#grid-dang-nhap").kendoGrid({
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
            { field: "MaMay", title: "Số quầy", width: 100, groupFooterTemplate: "<div>Tổng cộng: #=count#</div>", footerTemplate: "<div>Tổng cộng: #=count#</div>" },
            { field: "BD", title: "Thời điểm đăng nhập", width: 100, format: "{0: HH:mm:ss}" },
            { field: "KT", title: "Thời điểm đăng xuất", width: 100, format: "{0: HH:mm:ss}" },
            {
                field: "ThoiGian", title: "Tổng thời gian (Phút)", width: 100,
                groupFooterTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Lớn nhất: #=max#</div><div>Nhỏ nhất: #=min#</div>",
                footerTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Lớn nhất: #=max#</div><div>Nhỏ nhất: #=min#</div>"
            },
            { hidden: true, field: "Ngay", title: "Ngày", width: 100, format: "{0: dd MM yyyy}" },
        ],
    }).data("kendoGrid");
    //grid.hideColumn("Ngay");
    $("#div-grid-dang-nhap h3").text("Thời gian đăng nhập của " + $("#can-bo-dang-nhap").data("kendoDropDownList").text() + "(" + ThoiGian + ")");
}
// Tạo grid đăng nhập theo nam
function createGridDangNhapNam(MaCB, Loai, ThoiGian) {
    $("#grid-dang-nhap").html("");
    dataSource = new kendo.data.DataSource({
        transport: {
            serverFiltering: true,
            read: function (options) {
                $.ajax({
                    type: "GET",
                    url: url + "/api/DangNhapAPI/?_MaCB=" + MaCB + "&_Loai=" + Loai + "&_ThoiGian=" + ThoiGian,
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
                id: "Thang",
                fields: {
                    Thang: { type: "date", validation: { required: true } },
                    Ngay: { type: "date", validation: { required: true } },
                    ThoiGian: { type: "number", validation: { required: true } },
                    MaMay: { type: "number", validation: { required: true } },
                    BD: { type: "date", validation: { required: true } },
                    KT: { type: "date", validation: { required: true } },
                }
            }
        },
        group: [{
            field: "Thang", aggregates: [
                { field: "MaMay", aggregate: "count" },
                { field: "ThoiGian", aggregate: "average" },
                { field: "ThoiGian", aggregate: "sum" },
                { field: "ThoiGian", aggregate: "min" },
                { field: "ThoiGian", aggregate: "max" }
            ],
        }, {
            field: "Ngay", aggregates: [
                { field: "MaMay", aggregate: "count" },
                { field: "ThoiGian", aggregate: "average" },
                { field: "ThoiGian", aggregate: "sum" },
                { field: "ThoiGian", aggregate: "min" },
                { field: "ThoiGian", aggregate: "max" }
            ]
        }],

        aggregate: [
            { field: "MaMay", aggregate: "count" },
            { field: "ThoiGian", aggregate: "average" },
            { field: "ThoiGian", aggregate: "sum" },
            { field: "ThoiGian", aggregate: "min" },
            { field: "ThoiGian", aggregate: "max" }
        ]
    });

    var grid = $("#grid-dang-nhap").kendoGrid({
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
            { field: "MaMay", title: "Số quầy", width: 100, groupFooterTemplate: "<div>Tổng cộng: #=count#</div>", footerTemplate: "<div>Tổng cộng: #=count#</div>" },
            { hidden: true, field: "Ngay", title: "Ngày", width: 100, format: "{0: dd MM yyyy}" },
            { field: "BD", title: "Thời điểm đăng nhập", width: 100, format: "{0: HH:mm:ss}" },
            { field: "KT", title: "Thời điểm đăng xuất", width: 100, format: "{0: HH:mm:ss}" },
            {
                field: "ThoiGian", title: "Tổng thời gian (Phút)", width: 100,
                groupFooterTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Lớn nhất: #=max#</div><div>Nhỏ nhất: #=min#</div>",
                footerTemplate: "<div>Tổng: #=sum#</div><div>Trung bình: #if(average==null){#<span>#=0#</span>#}else{#<span>#=Math.round(average*100)/100#</span>#}#</div><div>Lớn nhất: #=max#</div><div>Nhỏ nhất: #=min#</div>"
            },
            { hidden: true, field: "Thang", title: "Tháng", width: 100, format: "{0: MM yyyy}" },
        ],
    }).data("kendoGrid");
    //grid.hideColumn("Ngay");
    $("#div-grid-dang-nhap h3").text("Thời gian đăng nhập của " + $("#can-bo-dang-nhap").data("kendoDropDownList").text() + "(" + ThoiGian + ")");
}
// Tạo dropdownlist tháng
function createMonthDangNhap(MaCB) {
    $("#thang-dang-nhap").prop("readonly", false);
    $("#thang-dang-nhap").html("");
    $.ajax({
        type: "GET",
        url: url + "/api/ValuesAPI/?_MaCB=" + MaCB,
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.length > 0) {
                $("#thang-dang-nhap").kendoDatePicker({
                    start: "year",
                    depth: "year",
                    format: "MM yyyy",
                    dateInput: false,
                    value: new Date(),
                    min: new Date(data[0]),
                    max: new Date(data[1]),
                    //disableDates: ["sa", "su"]
                });
                $("#thang-dang-nhap").prop("readonly", true);
            } else {
                $("#thang-dang-nhap").val("09 2018");
            }
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    })
}
// Tạo dropdownlist năm
function createYearDangNhap(MaCB) {
    $.ajax({
        type: "GET",
        url: url + "/api/ValuesAPI/?_MaCB=" + MaCB,
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.length > 0) {
                var arr = [];
                var j = 0;
                var arr1 = data[0].split(" ");
                var arr2 = data[1].split(" ");
                for (var i = Number.parseInt(arr1[0]); i <= Number.parseInt(arr2[0]); i++) {
                    arr[j] = { text: i.toString(), value: i.toString() };
                    j++;
                }
                $("#nam-dang-nhap").kendoDropDownList({
                    dataTextField: "text",
                    dataValueField: "value",
                    dataSource: arr
                });
            } else { }
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    })
}
// Tạo sự kiện nút xem
function onClick(e) {
    if ($("#cbx-thang-dang-nhap").prop("checked")) {
        createGridDangNhapThang($("#can-bo-dang-nhap").val(), "thang", $("#thang-dang-nhap").val())
    }
    if ($("#cbx-nam-dang-nhap").prop("checked")) {
        createGridDangNhapNam($("#can-bo-dang-nhap").val(), "nam", $("#nam-dang-nhap").val())
    }
}
// Tạo form nút xem
$("#btn-dang-nhap").kendoButton({
    click: onClick
});
// Phương thức thêm thuộc tính readonly vào DatePicker
function addReadonlyDatePicker() {
    $("#thang-dang-nhap").attr("readonly", true);
    $("#nam-dang-nhap").attr("readonly", false);
}
// Phương thức thêm thuộc tính readonly vào Dropdownlist
function addReadonlyDropdownlist() {
    $("#thang-dang-nhap").attr("readonly", false);
    $("#nam-dang-nhap").attr("readonly", true);
}
// Tạo sự kiện checkbox tháng đăng nhập
function readonlyDatePicker() {
    if (!$("#cbx-thang-dang-nhap").prop("checked")) {
        $("#cbx-thang-dang-nhap").removeAttr("checked");
        $("#cbx-nam-dang-nhap").prop("checked", "checked");
        //addReadonlyDatePicker();
    }
    else {
        $("#cbx-nam-dang-nhap").removeAttr("checked");
        $("#cbx-thang-dang-nhap").prop("checked", "checked");
        //addReadonlyDropdownlist();
    }
}
$("#div-loai-dang-nhap div:first-child div:first-child label").click(function () {
    $("#cbx-thang-dang-nhap").prop("checked", !$("#cbx-thang-dang-nhap").prop("checked"));
    readonlyDatePicker();
})
$("#cbx-thang-dang-nhap").change(function () {
    readonlyDatePicker();
})
// Tạo sự kiện checkbox năm đăng nhập
function readonlyDropdownlist() {
    if (!$("#cbx-nam-dang-nhap").prop("checked")) {
        $("#cbx-nam-dang-nhap").removeAttr("checked");
        $("#cbx-thang-dang-nhap").prop("checked", "checked");
        //addReadonlyDropdownlist();
    }
    else {
        $("#cbx-thang-dang-nhap").removeAttr("checked");
        $("#cbx-nam-dang-nhap").prop("checked", "checked");
        //addReadonlyDatePicker();
    }
}
$("#div-loai-dang-nhap div:last-child div:first-child label").click(function () {
    $("#cbx-nam-dang-nhap").prop("checked", !$("#cbx-nam-dang-nhap").prop("checked"));
    readonlyDropdownlist();
})
$("#cbx-nam-dang-nhap").change(function () {
    readonlyDropdownlist();
})
// Tạo sự kiện click của tabstrip xem thông tin đăng nhập
$("#menu-xem-dang-nhap").click(function () {
    createBPDangNhap();
})