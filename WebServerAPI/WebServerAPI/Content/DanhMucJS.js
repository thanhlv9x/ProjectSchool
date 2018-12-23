﻿// Tạo Grid thông tin bảng xếp loại
$("#menu-danh-muc").click(function () {
    $("#grid-danh-muc-xep-loai").data("kendoGrid").dataSource.read();
});
$("#menu-danh-muc-he-thong").click(function () {
    $("#grid-danh-muc-xep-loai").data("kendoGrid").dataSource.read();
});
createGridBXL();
function createGridBXL() {
    dataSource = new kendo.data.DataSource({
        transport: {
            serverFiltering: true,
            read: function (options) {
                $.ajax({
                    type: "GET",
                    url: url + "/BXL/Read",
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
                    url: url + "/BXL/Create",
                    data: { model: options.data.models },
                    dataType: 'json',
                    success: function (result) {
                        if (result == "Success") {
                            alert("Thêm mới thành công")
                        } else if (result == "Error") {
                            alert("Thêm mới không thành công")
                        }
                        var grid = $("#grid-danh-muc-xep-loai").data("kendoGrid");
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
                    url: url + "/BXL/Update",
                    data: { model: options.data.models },
                    dataType: 'json',
                    success: function (result) {
                        if (result == "Success") {
                            alert("Cập nhật thành công")
                        } else if (result == "Error") {
                            alert("Cập nhật không thành công")
                        }
                        var grid = $("#grid-danh-muc-xep-loai").data("kendoGrid");
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
                    url: url + "/BXL/Delete",
                    data: { model: options.data.models },
                    dataType: 'json',
                    success: function (result) {
                        if (result == "Success") {
                            alert("Xóa thành công")
                        } else if (result == "Error") {
                            alert("Xóa không thành công")
                        }
                        var grid = $("#grid-danh-muc-xep-loai").data("kendoGrid");
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
                id: "Id",
                fields: {
                    Id: { type: "number", editable: false, validation: { required: { message: "Mã xếp loại không được để trống" } } },
                    Diem: { type: "number", validation: { required: { message: "Điểm không được để trống" } } },
                    XepLoai: { type: "string", validation: { required: { message: "Xếp loại không được để trống" } } },
                }
            }
        },
    });

    var grid = $("#grid-danh-muc-xep-loai").kendoGrid({
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
            { field: "Diem", title: "Mốc điểm", width: 100 },
            { field: "XepLoai", title: "Xếp loại", width: 80 },
            { command: [{ name: "edit", text: "Cập nhật" }, { name: "myDelete", text: "Xóa bỏ", iconClass: "k-icon k-i-delete" }], title: "&nbsp;", width: "150px" }
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
            var nameField = e.container.find("input[name=XepLoai]");
            var name = nameField.val();
            if (name.length > 0) {
                e.container.data("kendoWindow").title("Cập nhật thông tin xếp loại"); // Title
                var updateBtn = e.container.find(".k-button.k-grid-update"); //update button
                updateBtn.text("Cập nhật");
                var cancelBtn = e.container.find(".k-button.k-grid-cancel"); //cancel button
                cancelBtn.text("Hủy bỏ");
            } else {
                e.container.data("kendoWindow").title("Thêm mới thông tin xếp loại"); // Title
                var updateBtn = e.container.find(".k-button.k-grid-update"); //update button
                updateBtn.text("Thêm mới");
                var cancelBtn = e.container.find(".k-button.k-grid-cancel"); //cancel button
                cancelBtn.text("Hủy bỏ");
            }
        }
    }).data("kendoGrid");

    $("#grid-danh-muc-xep-loai").on("click", ".k-grid-myDelete", function (e) {
        e.preventDefault();

        var command = $(this);
        var cell = command.closest("td");

        command.remove();
        cell.append('<a class="k-button k-button-icontext k-grid-myConfirm" href="#"><span class="k-icon k-i-check"></span>XÁC NHÂN</a>');
        cell.append('<a class="k-button k-button-icontext k-grid-myCancel" href="#"><span class="k-icon k-i-close"></span>HỦY BỎ</a>');
    });

    $("#grid-danh-muc-xep-loai").on("click", ".k-grid-myConfirm", function (e) {
        e.preventDefault();
        grid.removeRow($(this).closest("tr"))
    });

    $("#grid-danh-muc-xep-loai").on("click", ".k-grid-myCancel", function (e) {
        e.preventDefault();
        grid.refresh();
    })
}