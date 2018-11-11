$("#old-pw, #new-pw, #renew-pw").kendoMaskedTextBox();
$("#btn-change-pw, #btn-cancel-change-pw").kendoButton();
$("#btn-cancel-change-pw").click(function () {
    $("#old-pw, #new-pw, #renew-pw").val("");
})
$("#btn-change-pw").click(function () {
    var old = $("#old-pw").val();
    var news = $("#new-pw").val();
    var renew = $("#renew-pw").val();
    if (news != renew) { alert("Nhập lại mật khẩu mới không chính xác !"); }
    else {
        $.ajax({
            url: url + "/Login/ChangePw",
            type: "POST",
            dataType: "json",
            data: { "_OldPw": old, "_Pw": news },
            success: function (result) {
                if (result == "success") {
                    alert("Thay đổi thành công !");
                }
                else if (result == "fail") {
                    alert("Sai mật khẩu !");
                }
                else if (result == "error") {
                    alert("Gặp sự cố trong lúc thay đổi. Vui lòng đăng nhập lại !");
                }
                else if (result == "null") {
                    alert("Gặp sự cố trong lúc thay đổi. Vui lòng đăng nhập lại !");
                }
            },
            error: function (xhr) { }
        })
    }
    $("#old-pw, #new-pw, #renew-pw").val("");
})