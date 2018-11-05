using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CallNumberWebsite.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Mời nhập tài khoản")]
        public string Id { get; set; }
        [Required(ErrorMessage = "Mời nhập mật khẩu")]
        public string Pw { get; set; }
        [Required(ErrorMessage = "Mời chọn số quầy")]
        public string Port { get; set; }
    }
}