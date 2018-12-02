 // Phương thức hiển thị hình upload
function readURL(input) {
    if (input.files && input.files[0]) {
        var extend = input.files[0].name.split('.').pop();
        if (extend == "jpg" || extend == "jpeg" || extend == "png" || extend == "bmp" || extend == "gif") {
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
        } else {
            alert("Vui lòng chọn file ảnh !");
        }
    }
}
$("#menu-thong-tin-can-bo").click(function () {
    createTableTk(url + "/TaiKhoan/Read");
})
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
                                if (result == "macbsd") {
                                    alert("Thêm mới không thành công. Mã cán bộ đã tồn tại !");
                                }
                                else if (result == "error") {
                                    alert("Thêm mới không thành công. Thông tin nhập không chính xác, vui lòng kiểm tra lại !");
                                }
                                else {
                                    alert("Thêm mới thành công")
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
                                if (result == "macbsd") {
                                    alert("Cập nhật không thành công. Mã cán bộ đã tồn tại !");
                                }
                                else if (result == "image") {
                                    alert("Cập nhật không thành công. Lỗi hình ảnh !");
                                }
                                else if (result == "null") {
                                    alert("Cập nhật không thành công. Cán bộ không tồn tại !");
                                }
                                else if (result == "error") {
                                    alert("Cập nhật không thành công. Thông tin nhập không chính xác, vui lòng kiểm tra lại !");
                                }
                                else {
                                    alert("Cập nhật thành công");
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
                                    alert("Xóa thành công");
                                }
                                else {
                                    alert("Xóa không thành công. Cán bộ không tồn tại !");
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
                            MaCB: { type: "number", editable: false, validation: { required: { message: "Mã cán bộ không được để trống" } } },
                            HoTen: { type: "string", validation: { required: { message: "Họ tên không được để trống" } } },
                            HinhAnh: { type: "string", validation: { required: { message: "Hình ảnh không được để trống" } } },
                            MaBP: { field: "MaBP", type: "number", validation: { required: { message: "Bộ phận không được để trống" } }, defaultValue: mabp_tk },
                            Id: { type: "string", validation: { required: { message: "Tài khoản không được để trống" } } },
                            Pw: { type: "string", validation: { required: { message: "Mật khẩu không được để trống" } } },
                            MaCBSD: { type: "string", validation: { required: { message: "Mã cán bộ không được để trống" } } }
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
                pageable: {
                    refresh: true,
                    messages: {
                        display: "{0}-{1}/{2}",
                        empty: "Dữ liệu không tồn tại",
                    }
                },
                toolbar: [
                    { name: "create", text: "Thêm mới" },
                    { name: "custom", text: "Nhập excel", iconClass: "k-icon k-i-file-add" }],
                columns: [
                    { field: "HinhAnh", title: "Hình ảnh", width: 100, editor: categoryDropDownEditor, template: '<img src="resources/#= HinhAnh #" alt="image" style="width: 54px; height: 72px"/>' },
                    { field: "MaCBSD", title: "Mã cán bộ", width: 80 },
                    { field: "HoTen", title: "Họ tên", width: 100, footerTemplate: "Tổng cộng: #=count#", groupFooterTemplate: "Tổng: #=count#" },
                    { field: "MaBP", title: "Tên bộ phận", width: 100, values: arr },
                    { field: "Id", title: "Tài khoản", width: 100 },
                    { field: "Pw", title: "Mật khẩu", width: 100 },
                    { command: [{ name: "edit", text: "Cập nhật" }, { name: "myDelete", text: "Xóa bỏ", iconClass: "k-icon k-i-delete" }], title: "&nbsp;", width: "250px" }
                ],
                editable: {
                    confirmation: false,
                    mode: "popup"
                },
                dataBound: function () {
                    $(".k-grid-myDelete span").addClass("k-icon k-delete");
                },
                cancel: function () {
                    setTimeout(function () {
                        $(".k-grid-myDelete span").addClass("k-icon k-delete");
                    });
                },
                edit: function (e) {
                    $("input[type='file']").siblings("span").text("Chọn file...")
                    var nameField = e.container.find("input[name=HoTen]");
                    var name = nameField.val();
                    if (name.length > 0) {
                        e.container.data("kendoWindow").title("Cập nhật cán bộ " + name); // Title
                        var updateBtn = e.container.find(".k-button.k-grid-update"); //update button
                        updateBtn.text("Cập nhật");
                        var cancelBtn = e.container.find(".k-button.k-grid-cancel"); //cancel button
                        cancelBtn.text("Hủy bỏ");
                    } else {
                        e.container.data("kendoWindow").title("Thêm mới cán bộ"); // Title
                        var updateBtn = e.container.find(".k-button.k-grid-update"); //update button
                        updateBtn.text("Thêm mới");
                        var cancelBtn = e.container.find(".k-button.k-grid-cancel"); //cancel button
                        cancelBtn.text("Hủy bỏ");
                    }
                }
            }).data("kendoGrid");

            $("#grid-tai-khoan-can-bo").on("click", ".k-grid-myDelete", function (e) {
                e.preventDefault();

                var command = $(this);
                var cell = command.closest("td");

                command.remove();
                cell.append('<a class="k-button k-button-icontext k-grid-myConfirm" href="#"><span class="k-icon k-i-check"></span>XÁC NHÂN</a>');
                cell.append('<a class="k-button k-button-icontext k-grid-myCancel" href="#"><span class="k-icon k-i-close"></span>HỦY BỎ</a>');
            });

            $("#grid-tai-khoan-can-bo").on("click", ".k-grid-myConfirm", function (e) {
                e.preventDefault();
                grid.removeRow($(this).closest("tr"))
            });

            $("#grid-tai-khoan-can-bo").on("click", ".k-grid-myCancel", function (e) {
                e.preventDefault();
                grid.refresh();
            })

            // Nhập file excel
            $("#files").kendoUpload({
                validation: {
                    allowedExtensions: [".xls", ".xlsx"]
                },
                //showFileList: false,
                text: "Chọn file cần nhập",
                multiple: false
            });

            $("form#formUpload").submit(function (e) {
                e.preventDefault();
                var excelfile = new FormData(this);
                $.ajax({
                    url: url + "/TaiKhoan/Upload",
                    type: 'POST',
                    data: excelfile,
                    dataType: "json",
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
                $("#open-img").kendoUpload({
                    validation: {
                        allowedExtensions: [".jpg", ".jpeg", ".png", ".bmp", ".gif"]
                    },
                    showFileList: false,
                    text: "Chọn file cần nhập"
                });
            }

            // Tạo nút nhập excel
            var importBtn = $(".k-button.k-button-icontext.k-grid-custom");
            importBtn.click(function () {
                $("div#big-white-div-loading").show();
                $("input[type='file']").siblings("span").text("Chọn file...")
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
                    $("div#big-white-div-loading").hide();
                }
                myWindow.data("kendoWindow").open();
                importBtn.fadeOut();
            });
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    })
    //$("#grid-tai-khoan-can-bo").html("");

}
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
                            alert("Thêm không thành công. Mã bộ phận hoặc tên bộ phận đã tồn tại !")
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
                            alert("Cập nhật không thành công. Mã bộ phận hoặc tên bộ phận đã tồn tại !")
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
                            alert("Xóa không thành công. Vui lòng kiểm tra lại thông tin !")
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
                    MaBP: { type: "number", editable: false, validation: { required: { message: "Mã bộ phận không được để trống" } } },
                    TenBP: { type: "string", validation: { required: { message: "Tên bộ phận không được để trống" } } },
                    VietTat: { type: "string", validation: { required: { message: "Mã bộ phận không được để trống" } } }
                }
            }
        }
    });

    var grid = $("#grid-thong-tin-bo-phan").kendoGrid({
        dataSource: dataSource,
        navigatable: true,
        pageable: {
            refresh: true,
            messages: {
                display: "{0}-{1}/{2}",
                empty: "Dữ liệu không tồn tại",
            }
        },
        toolbar: [{ name: "create", text: "Thêm mới" }],
        columns: [
            { field: "VietTat", title: "Mã bộ phận", width: 80 },
            { field: "TenBP", title: "Tên bộ phận", width: 100 },
            { command: [{ name: "edit", text: "Cập nhật" }, { name: "myDelete", text: "Xóa bỏ", iconClass: "k-icon k-i-delete" } ], title: "&nbsp;", width: "250px" }
        ],
        editable: {
            confirmation: false,
            mode: "popup"
        },
        dataBound: function () {
            $(".k-grid-myDelete span").addClass("k-icon k-delete");
        },
        cancel: function () {
            setTimeout(function () {
                $(".k-grid-myDelete span").addClass("k-icon k-delete");
            });
        },
        edit: function (e) {
            $("input[type='file']").siblings("span").text("Chọn file...")
            var nameField = e.container.find("input[name=TenBP]");
            var name = nameField.val();
            if (name.length > 0) {
                e.container.data("kendoWindow").title("Cập nhật bộ phận " + name); // Title
                var updateBtn = e.container.find(".k-button.k-grid-update"); //update button
                updateBtn.text("Cập nhật");
                var cancelBtn = e.container.find(".k-button.k-grid-cancel"); //cancel button
                cancelBtn.text("Hủy bỏ");
            } else {
                e.container.data("kendoWindow").title("Thêm mới bộ phận"); // Title
                var updateBtn = e.container.find(".k-button.k-grid-update"); //update button
                updateBtn.text("Thêm mới");
                var cancelBtn = e.container.find(".k-button.k-grid-cancel"); //cancel button
                cancelBtn.text("Hủy bỏ");
            }
        }
    }).data("kendoGrid");

    $("#grid-thong-tin-bo-phan").on("click", ".k-grid-myDelete", function (e) {
        e.preventDefault();

        var command = $(this);
        var cell = command.closest("td");

        command.remove();
        cell.append('<a class="k-button k-button-icontext k-grid-myConfirm" href="#"><span class="k-icon k-i-check"></span>XÁC NHẬN</a>');
        cell.append('<a class="k-button k-button-icontext k-grid-myCancel" href="#"><span class="k-icon k-i-close"></span>HỦY BỎ</a>');
    });

    $("#grid-thong-tin-bo-phan").on("click", ".k-grid-myConfirm", function (e) {
        e.preventDefault();
        grid.removeRow($(this).closest("tr"))
    });

    $("#grid-thong-tin-bo-phan").on("click", ".k-grid-myCancel", function (e) {
        e.preventDefault();
        grid.refresh();
    })
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
                            alert("Thêm không thành công. Số quầy đã tồn tại !")
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
                            alert("Cập nhật không thành công. Số quầy đã tồn tại !")
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
                            alert("Xóa không thành công. Sai thông tin vui lòng kiểm tra lại !")
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
                    MaMay: { type: "number", validation: { required: { message: "Số quầy không được để trống" }, min: 1, default: 1 } },
                    Mac: { type: "number", validation: { required: { message: "Mã máy không được để trống" } } }
                }
            }
        }
    });

    var grid = $("#grid-thong-tin-quay").kendoGrid({
        dataSource: dataSource,
        navigatable: true,
        pageable: {
            refresh: true,
            messages: {
                display: "{0}-{1}/{2}",
                empty: "Dữ liệu không tồn tại",
            }
        },
        toolbar: [{ name: "create", text: "Thêm mới" }],
        columns: [
            //{ field: "MaMay", title: "Số quầy", width: 80 },
            { field: "Mac", title: "Số quầy", width: 100 },
            { command: [{ name: "edit", text: "Cập nhật" }, { name: "myDelete", text: "Xóa bỏ", iconClass: "k-icon k-i-delete" } ], title: "&nbsp;", width: "250px" }
        ],
        editable: {
            confirmation: false,
            mode: "popup"
        },
        dataBound: function () {
            $(".k-grid-myDelete span").addClass("k-icon k-delete");
        },
        cancel: function () {
            setTimeout(function () {
                $(".k-grid-myDelete span").addClass("k-icon k-delete");
            });
        },
        edit: function (e) {
            $("input[type='file']").siblings("span").text("Chọn file...")
            var nameField = e.container.find("input[name=MaMay]");
            var name = nameField.val();
            if (name > 0) {
                e.container.data("kendoWindow").title("Cập nhật số quầy " + name); // Title
                var updateBtn = e.container.find(".k-button.k-grid-update"); //update button
                updateBtn.text("Cập nhật");
                var cancelBtn = e.container.find(".k-button.k-grid-cancel"); //cancel button
                cancelBtn.text("Hủy bỏ");
            } else {
                e.container.data("kendoWindow").title("Thêm mới số quầy"); // Title
                var updateBtn = e.container.find(".k-button.k-grid-update"); //update button
                updateBtn.text("Thêm mới");
                var cancelBtn = e.container.find(".k-button.k-grid-cancel"); //cancel button
                cancelBtn.text("Hủy bỏ");
            }
        }
    }).data("kendoGrid");

    $("#grid-thong-tin-quay").on("click", ".k-grid-myDelete", function (e) {
        e.preventDefault();

        var command = $(this);
        var cell = command.closest("td");

        command.remove();
        cell.append('<a class="k-button k-button-icontext k-grid-myConfirm" href="#"><span class="k-icon k-i-check"></span>XÁC NHẬN</a>');
        cell.append('<a class="k-button k-button-icontext k-grid-myCancel" href="#"><span class="k-icon k-i-close"></span>HỦY BỎ</a>');
    });

    $("#grid-thong-tin-quay").on("click", ".k-grid-myConfirm", function (e) {
        e.preventDefault();
        grid.removeRow($(this).closest("tr"))
    });

    $("#grid-thong-tin-quay").on("click", ".k-grid-myCancel", function (e) {
        e.preventDefault();
        grid.refresh();
    })
}

// Tạo sự kiện chọn các sub-tabstrip trong tab Thông tin
$("#menu-thong-tin-can-bo-tk").click(function () {
    clearGrid();
    // Tạo bảng tài khoản cán bộ
    createTableTk(url + "/TaiKhoan/Read");
})
$("#menu-thong-tin-can-bo-bp").click(function () {
    clearGrid();
    // Tạo bảng thông tin bộ phận
    createTableTTBP(url + "/BoPhan/Read");
})
$("#menu-thong-tin-can-bo-sq").click(function () {
    clearGrid();
    // Tạo bảng thông tin quầy
    createTableTTQ(url + "/SoQuay/Read");
})
// Xóa toàn bộ grid
function clearGrid() {
    $("#grid-tai-khoan-can-bo").html("");
    $("#grid-thong-tin-bo-phan").html("");
    $("#grid-thong-tin-quay").html("");
}