using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServerAPI.Models
{
    public class TaiKhoanUser
    {
        public string Id { get; set; }
        public string Pw { get; set; }
        public Nullable<int> MaCB { get; set; }
        public Nullable<int> MaBP { get; set; }
        public string TenBP { get; set; }
        public string HoTen { get; set; }
        public string HinhAnh { get; set; }
    }
}