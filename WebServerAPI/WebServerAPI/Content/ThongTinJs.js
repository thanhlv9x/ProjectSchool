/*
// ============ Thông tin tài khoản cán bộ ==============
//// Nút tên bộ phận: sử dụng ajax để lấy dữ liệu tên và mã bộ phận
//function getTKName() {
//    var str = "";
//    $.ajax({
//        type: "GET",
//        url: url + "/api/BoPhanAPI",
//        dataType: "json",
//        success: function (data) {
//            $.each(data, function (key, val) {
//                str += "<div id='tk" + val.MaBP + "' class='btnTK'>" + val.TenBP + "</div>";
//            });
//            $("#tai-khoan-ten-bo-phan").html(str);
//            createButtonTK();
//        },
//        error: function (xhr) {
//            console.log(xhr.responseText);
//        }
//    });
//}
//// Nút tên bộ phận: tạo phương thức click
//function onClickBtnTK(e) {
//    $("#tai-khoan-ten-bo-phan").hide("slow");
//    $("#div-tai-khoan-can-bo").show("slow");
//    setTimeout(function () {
//        mabp = $(e.event.target).attr("id").substring(2);
//        $("#div-tai-khoan-can-bo h1").text($(e.event.target).text());
//        createTableTk(url + "/api/TaiKhoanAPI/?_MaBP=" + mabp);
//    }, 500);
//}
//// Nút tên bộ phận: tạo form nút
//function createButtonTK() {
//    $(".btnTK").each(function (index) {
//        $(this).kendoButton({
//            click: onClickBtnTK
//        });
//    });
//}
//// Nút tên bộ phận: sự kiện click vào menu xem kết quả
//$("#menu-thong-tin-can-bo-tk").click(getTKName)
*/
// Phương thức hiển thị hình upload
function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#chan-dung')
                .attr('src', e.target.result)
                .width(54)
                .height(72);
            $("#chan-dung").val(e.target.result);
            $("#chan-dung")
                .attr('class', 'k-valid')
            var id = $("div[data-uid]").attr("data-uid")
            var a = e.target.result;
            var grid = $('#grid-tai-khoan-can-bo').data().kendoGrid.dataSource.data();
            for (var i = 0; i < grid.length; i++) {
                if (grid[i].uid == id) {
                    var firstItem = grid[i];
                    firstItem.set('HinhAnh', a.substring(a.lastIndexOf(',') + 1));
                }
            }
        };

        reader.readAsDataURL(input.files[0]);
    }
}

// ================ Thông tin tài khoản cán bộ =================
// Tạo sự kiện nút quay lại bộ phận
function backTK() {
    $("#tai-khoan-ten-bo-phan").show("slow");
    $("#div-tai-khoan-can-bo").hide("slow");
}
// Tạo bảng thông tin tài khoản của cán bộ
function createTableTk(urlGet) {
    var arr = [];
    var mabp_tk
    $.ajax({
        type: "GET",
        url: url + "/api/BoPhanAPI",
        dataType: "json",
        async: false,
        success: function (data) {
            data.forEach(function (item) {
                arr.push({ text: item.TenBP, value: item.MaBP });
                mabp_tk = item.MaBP
            });
            dataSource = new kendo.data.DataSource({
                transport: {
                    serverFiltering: true,
                    read: function (options) {
                        $.ajax({
                            type: "GET",
                            url: urlGet,
                            dataType: 'json',
                            success: function (result) {
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                            }
                        });
                    },
                    create: function (options) {
                        $.ajax({
                            type: "POST",
                            url: url + "/TaiKhoan/Create",
                            data: { model: options.data.models },
                            dataType: 'json',
                            success: function (result) {
                                if (result) {
                                    alert("Thêm mới thành công")
                                }
                                else {
                                    alert("Thêm mới không thành công")
                                }
                                var grid = $("#grid-tai-khoan-can-bo").data("kendoGrid");
                                grid.dataSource.read();
                            },
                            error: function (xhr) {
                                console.log(xhr.responseJSON);
                            }
                        });
                    },
                    update: function (options) {
                        $.ajax({
                            type: "POST",
                            url: url + "/TaiKhoan/Update",
                            data: { model: options.data.models },
                            dataType: 'json',
                            success: function (result) {
                                if (result) {
                                    alert("Cập nhật thành công")
                                }
                                else {
                                    alert("Cập nhật không thành công")
                                }
                                var grid = $("#grid-tai-khoan-can-bo").data("kendoGrid");
                                grid.dataSource.read();
                            },
                            error: function (xhr) {
                                console.log(xhr.responseJSON);
                            }
                        });
                    },
                    destroy: function (options) {
                        $.ajax({
                            type: "POST",
                            url: url + "/TaiKhoan/Delete",
                            data: { model: options.data.models },
                            dataType: 'json',
                            success: function (result) {
                                if (result) {
                                    alert("Xóa thành công")
                                }
                                else {
                                    alert("Xóa không thành công")
                                }
                                var grid = $("#grid-tai-khoan-can-bo").data("kendoGrid");
                                grid.dataSource.read();
                            },
                            error: function (xhr) {
                                console.log(xhr.responseJSON);
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
                            MaCB: { type: "number", editable: false, validation: { required: true } },
                            HoTen: { type: "string", validation: { required: true } },
                            HinhAnh: { type: "string", validation: { required: true } },
                            MaBP: { field: "MaBP", type: "number", validation: { required: true }, defaultValue: mabp_tk },
                            Id: { type: "string", validation: { required: true } },
                            Pw: { type: "string", validation: { required: true } },
                            MaCBSD: { type: "string", validation: { required: true } }
                        }
                    }
                },
                group: {
                    field: "MaBP", aggregates: [{ field: "HoTen", aggregate: "count" }]
                },
                aggregate: [{ field: "HoTen", aggregate: "count" }]
            });

            var grid = $("#grid-tai-khoan-can-bo").kendoGrid({
                dataSource: dataSource,
                navigatable: true,
                pageable: true,
                toolbar: [
                    { name: "create", text: "Thêm mới" },
                    { name: "custom", text: "Nhập excel", iconClass: "k-icon k-i-file-add" },
                    { name: "custom1", text: "Làm mới", iconClass: "k-icon k-i-refresh" }],
                columns: [
                    { field: "HinhAnh", title: "Hình ảnh", width: 100, editor: categoryDropDownEditor, template: '<img src="resources/#= HinhAnh #" alt="image" style="width: 54px; height: 72px"/>' },
                    { field: "MaCBSD", title: "Mã cán bộ", width: 80 },
                    { field: "HoTen", title: "Họ tên", width: 100, footerTemplate: "Tổng cộng: #=count#", groupFooterTemplate: "Tổng: #=count#" },
                    { field: "MaBP", title: "Tên bộ phận", width: 100, values: arr },
                    { field: "Id", title: "Tài khoản", width: 100 },
                    { field: "Pw", title: "Mật khẩu", width: 100 },
                    { command: [{ name: "edit", title: "Cập nhật" }, { name: "destroy", title: "Xóa bỏ" }], title: "&nbsp;", width: "250px" }
                ],
                editable: "popup"
            }).data("kendoGrid");
            // Nhập file excel
            var myWindow = $("#windowPopup");
            myWindow.kendoWindow({
                width: "600px",
                height: "250px",
                title: "Nhập excel",
                visible: false,
                actions: [
                    "Close"
                ],
            }).data("kendoWindow").center();
            $("#files").kendoUpload({ text: "Chọn file cần nhập" });

            $("form#formUpload").submit(function (e) {
                e.preventDefault();
                var excelfile = new FormData(this);
                $.ajax({
                    url: url + "/TaiKhoan/Upload",
                    type: 'POST',
                    data: excelfile,
                    //async: false,
                    success: function (data) {
                        alert(data)
                    },
                    cache: false,
                    contentType: false,
                    processData: false
                });
                var grid = $("#grid-tai-khoan-can-bo").data("kendoGrid");
                grid.dataSource.read();
                return false;
            });
            function categoryDropDownEditor(container, options) {
                $('<img required="required" name="' + options.field + '" id="chan-dung" /><input type="file" onchange="readURL(this)" id="open-img"/>')
                    .appendTo(container)
                $("#open-img").kendoUpload(); 
            }

            // Tạo nút nhập excel
            var importBtn = $(".k-button.k-button-icontext.k-grid-custom");
            importBtn.click(function () {
                var myWindow = $("#windowPopup");
                myWindow.kendoWindow({
                    width: "600px",
                    title: "Nhập excel",
                    visible: false,
                    actions: [
                        "Close"
                    ],
                    close: onClose
                }).data("kendoWindow").center();
                function onClose() {
                    importBtn.fadeIn();
                }
                myWindow.data("kendoWindow").open();
                importBtn.fadeOut();
            });

            var refreshBtn = $(".k-button.k-button-icontext.k-grid-custom1");
            refreshBtn.click(function () {
                grid.dataSource.read();
                //$("#grid-tai-khoan-can-bo").html("");
                //createTableTk(url + "/TaiKhoan/Read");
            });
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    })
    //$("#grid-tai-khoan-can-bo").html("");

}
// Nhập file excel
var myWindow = $("#windowPopup");
myWindow.kendoWindow({
    width: "600px",
    height: "250px",
    title: "Nhập excel",
    visible: false,
    actions: [
        "Close"
    ],
}).data("kendoWindow").center();
$("#files").kendoUpload({ text: "Chọn file cần nhập" });
// ================ Thông tin bộ phận ====================
// Tạo bảng thông tin bộ phận
function createTableTTBP(urlGet) {
    //$("#grid-tai-khoan-can-bo").html("");
    dataSource = new kendo.data.DataSource({
        transport: {
            serverFiltering: true,
            read: function (options) {
                $.ajax({
                    type: "GET",
                    url: urlGet,
                    dataType: 'json',
                    success: function (result) {
                        options.success(result);
                    },
                    error: function (result) {
                        options.error(result);
                    }
                });
            },
            create: function (options) {
                $.ajax({
                    type: "POST",
                    url: url + "/BoPhan/Create",
                    data: { model: options.data.models },
                    dataType: 'json',
                    success: function (result) {
                        if (result) {
                            alert("Thêm thành công")
                        }
                        else {
                            alert("Thêm không thành công")
                        }
                        var grid = $("#grid-thong-tin-bo-phan").data("kendoGrid");
                        grid.dataSource.read();
                    },
                    error: function (xhr) {
                        console.log(xhr.responseJSON);
                    }
                });
            },
            update: function (options) {
                $.ajax({
                    type: "POST",
                    url: url + "/BoPhan/Update",
                    data: { model: options.data.models },
                    dataType: 'json',
                    success: function (result) {
                        if (result) {
                            alert("Cập nhật thành công")
                        }
                        else {
                            alert("Cập nhật không thành công")
                        }
                        var grid = $("#grid-thong-tin-bo-phan").data("kendoGrid");
                        grid.dataSource.read();
                    },
                    error: function (xhr) {
                        console.log(xhr.responseJSON);
                    }
                });
            },
            destroy: function (options) {
                $.ajax({
                    type: "POST",
                    url: url + "/BoPhan/Delete",
                    data: { model: options.data.models },
                    dataType: 'json',
                    success: function (result) {
                        if (result) {
                            alert("Xóa thành công")
                        }
                        else {
                            alert("Xóa không thành công")
                        }
                        var grid = $("#grid-thong-tin-bo-phan").data("kendoGrid");
                        grid.dataSource.read();
                    },
                    error: function (xhr) {
                        console.log(xhr.responseJSON);
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
                    MaBP: { type: "number", editable: false, validation: { required: true } },
                    TenBP: { type: "string", validation: { required: true } },
                    VietTat: { type: "string", validation: { required: true } }
                }
            }
        }
    });

    var grid = $("#grid-thong-tin-bo-phan").kendoGrid({
        dataSource: dataSource,
        navigatable: true,
        pageable: true,
        toolbar: [{ name: "create", text: "Thêm mới" }, { name: "custom2", text: "Làm mới", iconClass: "k-icon k-i-refresh" }],
        columns: [
            { field: "VietTat", title: "Mã bộ phận", width: 80 },
            { field: "TenBP", title: "Tên bộ phận", width: 100 },
            { command: [{ name: "edit", title: "Cập nhật" }, { name: "destroy", title: "Xóa bỏ" }], title: "&nbsp;", width: "250px" }
        ],
        editable: "popup"
    }).data("kendoGrid");

    var refreshBtn = $(".k-button.k-button-icontext.k-grid-custom2");
    refreshBtn.click(function () {
        grid.dataSource.read();
    });
}
// ================ Thông tin quầy ====================
// Tạo bảng thông tin quầy
function createTableTTQ(urlGet) {
    //$("#grid-tai-khoan-can-bo").html("");
    dataSource = new kendo.data.DataSource({
        transport: {
            serverFiltering: true,
            read: function (options) {
                $.ajax({
                    type: "GET",
                    url: urlGet,
                    dataType: 'json',
                    success: function (result) {
                        options.success(result);
                    },
                    error: function (result) {
                        options.error(result);
                    }
                });
            },
            create: function (options) {
                $.ajax({
                    type: "POST",
                    url: url + "/SoQuay/Create",
                    data: { model: options.data.models },
                    dataType: 'json',
                    success: function (result) {
                        if (result) {
                            alert("Thêm thành công")
                        }
                        else {
                            alert("Thêm không thành công")
                        }
                        var grid = $("#grid-thong-tin-quay").data("kendoGrid");
                        grid.dataSource.read();
                    },
                    error: function (xhr) {
                        console.log(xhr.responseJSON);
                    }
                });
            },
            update: function (options) {
                $.ajax({
                    type: "POST",
                    url: url + "/SoQuay/Update",
                    data: { model: options.data.models },
                    dataType: 'json',
                    success: function (result) {
                        if (result) {
                            alert("Cập nhật thành công")
                        }
                        else {
                            alert("Cập nhật không thành công")
                        }
                        var grid = $("#grid-thong-tin-quay").data("kendoGrid");
                        grid.dataSource.read();
                    },
                    error: function (xhr) {
                        console.log(xhr.responseJSON);
                    }
                });
            },
            destroy: function (options) {
                $.ajax({
                    type: "POST",
                    url: url + "/SoQuay/Delete",
                    data: { model: options.data.models },
                    dataType: 'json',
                    success: function (result) {
                        if (result) {
                            alert("Xóa thành công")
                        }
                        else {
                            alert("Xóa không thành công")
                        }
                        var grid = $("#grid-thong-tin-quay").data("kendoGrid");
                        grid.dataSource.read();
                    },
                    error: function (xhr) {
                        console.log(xhr.responseJSON);
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
                id: "MaMay",
                fields: {
                    MaMay: { type: "number", validation: { required: true } },
                    Mac: { type: "string", validation: { required: true } }
                }
            }
        }
    });

    var grid = $("#grid-thong-tin-quay").kendoGrid({
        dataSource: dataSource,
        navigatable: true,
        pageable: true,
        toolbar: [{ name: "create", text: "Thêm mới" }, { name: "custom3", text: "Làm mới", iconClass: "k-icon k-i-refresh" }],
        columns: [
            { field: "MaMay", title: "Số quầy", width: 80 },
            { field: "Mac", title: "Mã máy", width: 100 },
            { command: [{ name: "edit", title: "Cập nhật" }, { name: "destroy", title: "Xóa bỏ" }], title: "&nbsp;", width: "250px" }
        ],
        editable: "popup"
    }).data("kendoGrid");

    var refreshBtn = $(".k-button.k-button-icontext.k-grid-custom3");
    refreshBtn.click(function () {
        grid.dataSource.read();
    });
}

// Tạo sự kiện chọn các sub-tabstrip trong tab Thông tin
$("#menu-thong-tin-can-bo-tk").click(function () {
    // Tạo bảng tài khoản cán bộ
    createTableTk(url + "/TaiKhoan/Read");
})
$("#menu-thong-tin-can-bo-bp").click(function () {
    // Tạo bảng thông tin bộ phận
    createTableTTBP(url + "/BoPhan/Read");
})
$("#menu-thong-tin-can-bo-sq").click(function () {
    // Tạo bảng thông tin quầy
    createTableTTQ(url + "/SoQuay/Read");
})