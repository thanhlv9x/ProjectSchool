// Lấy dữ liệu mã vùng
function getMaVung() {
    var arrMaVung = [];
    $.ajax({
        url: url + "/SMS/GetMaVung",
        type: "POST",
        dataType: "json",
        async: false,
        success: function (result) {
            arrMaVung = result;
        },
        error: function (xhr) { }
    })
    return arrMaVung;
}
// Lấy dữ liệu tên bộ phận để thay tên cột
function getTenBoPhan() {
    $.ajax({
        url: url + "/SMS/GetBoPhan",
        type: "POST",
        dataType: "json",
        success: function (result) {
            for (var i = 0; i < $("#grid-sms th").length; i++) {
                for (var j = 0; j < result.length; j++) {
                    if ($("#grid-sms th")[i].innerText == "Bộ phận " + result[j]["value"]) {
                        $("#grid-sms th")[i].innerText = result[j]["text"];
                    }
                }
            }
        },
        error: function (xhr) {}
    })
}
$("#menu-sms").click(function () {
    $("#grid-sms").data("kendoGrid").dataSource.read();
});
createGridSMS();
// Tạo Grid thông tin tin nhắn thông báo
function createGridSMS() {
    dataSource = new kendo.data.DataSource({
        transport: {
            serverFiltering: true,
            read: function (options) {
                $.ajax({
                    type: "GET",
                    url: url + "/SMS/Read",
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
                    url: url + "/SMS/Create",
                    data: { model: options.data.models },
                    dataType: 'json',
                    success: function (result) {
                        if (result == "Success") {
                            alert("Thêm mới thành công")
                        } else if (result == "Error") {
                            alert("Thêm mới không thành công")
                        }
                        var grid = $("#grid-sms").data("kendoGrid");
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
                    url: url + "/SMS/Update",
                    data: { model: options.data.models },
                    dataType: 'json',
                    success: function (result) {
                        if (result == "Success") {
                            alert("Cập nhật thành công")
                        } else if (result == "Error") {
                            alert("Cập nhật không thành công")
                        }
                        var grid = $("#grid-sms").data("kendoGrid");
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
                    url: url + "/SMS/Delete",
                    data: { model: options.data.models },
                    dataType: 'json',
                    success: function (result) {
                        if (result == "Success") {
                            alert("Xóa thành công")
                        } else if (result == "Error") {
                            alert("Xóa không thành công")
                        }
                        var grid = $("#grid-sms").data("kendoGrid");
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
                    Id: { type: "number", editable: false, validation: { required: { message: "Mã cán bộ không được để trống" } } },
                    HoTen: { type: "string", validation: { required: { message: "Họ tên không được để trống" } } },
                    IdSdt: { type: "number", validation: { required: { message: "Mã số điện thoại không được để trống" } } },
                    MaVung: { field: "MaVung", type: "number", validation: { required: { message: "Mã vùng không được để trống" } }, defaultValue: -1 },
                    Sdt: { type: "number", validation: { required: { message: "Số điện thoại không được để trống" } } },
                    Bp1: { type: "boolean" },
                    Bp2: { type: "boolean" },
                    Bp3: { type: "boolean" },
                    Bp4: { type: "boolean" },
                    Bp5: { type: "boolean" },
                    Bp6: { type: "boolean" },
                    Bp7: { type: "boolean" },
                    Bp8: { type: "boolean" },
                    Bp9: { type: "boolean" },
                    Bp10: { type: "boolean" },
                    Bp11: { type: "boolean" },
                    Bp12: { type: "boolean" },
                    Bp13: { type: "boolean" },
                    Bp14: { type: "boolean" },
                    Bp15: { type: "boolean" },
                }
            }
        },
    });

    var grid = $("#grid-sms").kendoGrid({
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
            { field: "HoTen", title: "Họ tên", width: 100 },
            { field: "MaVung", title: "Mã vùng", width: 80, values: getMaVung() },
            { field: "Sdt", title: "Số điện thoại", width: 100 },
            {
                field: "Bp1", title: "Bộ phận 1", width: 100,
                template: function (dataItem) { return dataItem.Bp1 ? "Nhận" : "" },
            },
            {
                field: "Bp2", title: "Bộ phận 2", width: 100,
                template: function (dataItem) { return dataItem.Bp2 ? "Nhận" : "" }, },
            {
                field: "Bp3", title: "Bộ phận 3", width: 100,
                template: function (dataItem) { return dataItem.Bp3 ? "Nhận" : "" }, },
            {
                field: "Bp4", title: "Bộ phận 4", width: 100,
                template: function (dataItem) { return dataItem.Bp4 ? "Nhận" : "" }, },
            {
                field: "Bp5", title: "Bộ phận 5", width: 100,
                template: function (dataItem) { return dataItem.Bp5 ? "Nhận" : "" }, },
            {
                field: "Bp6", title: "Bộ phận 6", width: 100,
                template: function (dataItem) { return dataItem.Bp6 ? "Nhận" : "" }, },
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
            var nameField = e.container.find("input[name=HoTen]");
            var name = nameField.val();
            if (name.length > 0) {
                e.container.data("kendoWindow").title("Cập nhật tin nhắn thông báo cán bộ " + name); // Title
                var updateBtn = e.container.find(".k-button.k-grid-update"); //update button
                updateBtn.text("Cập nhật");
                var cancelBtn = e.container.find(".k-button.k-grid-cancel"); //cancel button
                cancelBtn.text("Hủy bỏ");
            } else {
                e.container.data("kendoWindow").title("Thêm mới tin nhắn thông báo cán bộ"); // Title
                var updateBtn = e.container.find(".k-button.k-grid-update"); //update button
                updateBtn.text("Thêm mới");
                var cancelBtn = e.container.find(".k-button.k-grid-cancel"); //cancel button
                cancelBtn.text("Hủy bỏ");
            }
        }
    }).data("kendoGrid");

    $("#grid-sms").on("click", ".k-grid-myDelete", function (e) {
        e.preventDefault();

        var command = $(this);
        var cell = command.closest("td");

        command.remove();
        cell.append('<a class="k-button k-button-icontext k-grid-myConfirm" href="#"><span class="k-icon k-i-check"></span>XÁC NHÂN</a>');
        cell.append('<a class="k-button k-button-icontext k-grid-myCancel" href="#"><span class="k-icon k-i-close"></span>HỦY BỎ</a>');
    });

    $("#grid-sms").on("click", ".k-grid-myConfirm", function (e) {
        e.preventDefault();
        grid.removeRow($(this).closest("tr"))
    });

    $("#grid-sms").on("click", ".k-grid-myCancel", function (e) {
        e.preventDefault();
        grid.refresh();
    })
    getTenBoPhan();
}