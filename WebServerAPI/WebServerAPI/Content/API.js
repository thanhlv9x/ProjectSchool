$("#menu-danh-muc-API").click(getData);
$("#APIKey").kendoMaskedTextBox();
$("#SecretKey").kendoMaskedTextBox();
$("#btnAPIKey").kendoButton();
$("#Balance").kendoMaskedTextBox();
$("#Balance").data("kendoMaskedTextBox").readonly();
function getData() {
    $.ajax({
        url: url + "/api/GetKeyAPI",
        type: "GET",
        dataType: "json",
        success: function (result) {
            $("#APIId").val(result["Id"]);
            $("#APIKey").val(result["APIKey"]);
            $("#SecretKey").val(result["SecretKey"]);
            getBalance();
        },
        error: function (xhr) { }
    })
}

function getBalance() {
    $.ajax({
        url: url + "/api/GetKeyAPI?APIKey=" + $("#APIKey").val() + "&SecretKey=" + $("#SecretKey").val(),
        type: "GET",
        dataType: "json",
        success: function (result) {
            $("#Balance").val(result["Balance"] + ' VNĐ');
        },
        error: function (xhr) { }
    })
}

$("#btnAPIKey").click(function () {
    var data = {
        Id: $("#APIId").val(),
        APIKey: $("#APIKey").val(),
        SecretKey: $("#SecretKey").val()
    }
    $.ajax({
        url: url + "/api/GetKeyAPI",
        type: "POST",
        dataType: "json",
        data: data,
        success: function (result) {
            if (result) alert("Thay đổi thành công");
            else alert("Thay đổi thất bại")
        },
        error: function (xhr) { }
    })
})