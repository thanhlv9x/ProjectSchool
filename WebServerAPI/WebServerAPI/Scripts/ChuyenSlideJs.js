document.addEventListener("DOMContentLoaded", function () {
    var btn = document.querySelectorAll(".chuyenslide ul li");
    var slides = document.querySelectorAll(".slides ul li");
    var time = setInterval(function () { autoSlide(); }, 3000); // tự động thực hiện hàm
    for (var i = 0; i < btn.length; i++) {
        btn[i].addEventListener("click", function () {
            clearInterval(time); // xóa bỏ tự động thực hiện hàm

            for (var i = 0; i < btn.length; i++) {
                btn[i].classList.remove("hienthi");
            }
            this.classList.add("hienthi");

            // previousElementSibling trả về vị trí của phần tử trước cùng cấp

            var btnActive = this;
            var positionBtn = 0;
            for (positionBtn = 0; btnActive = btnActive.previousElementSibling; positionBtn++) { }

            for (var i = 0; i < slides.length; i++) {
                slides[i].classList.remove("active");
            }
            slides[positionBtn].classList.add("active");
            time = setInterval(function () { autoSlide(); }, 3000);
        })
    } // hết sự kiện nút


    function autoSlide() {
        var curPos = document.querySelector(".slides ul li.active"); // vị trí hiện tại của slide
        var positionSlide = 0; // vị trí slide
        // Vòng lặp tính vị trí hiện tại của slide đang hiển thị
        for (positionSlide = 0; curPos = curPos.previousElementSibling; positionSlide++) { }

        for (var i = 0; i < slides.length; i++) {
            slides[i].classList.remove("active");
            btn[i].classList.remove("hienthi");
        }
        if (positionSlide < slides.length - 1) {
            slides[positionSlide].nextElementSibling.classList.add("active");
            btn[positionSlide].nextElementSibling.classList.add("hienthi");
        } else {
            slides[0].classList.add("active");
            btn[0].classList.add("hienthi");
        }
    }

}, false);