using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServerAPI.Models
{
    public class ThoiGianDangNhap
    {
        public string MaCBSD { get; set; }
        public int MaCB { get; set; }
        public string HoTen { get; set; }
        public int MaMay { get; set; }
        public DateTime Ngay { get; set; }
        public DateTime Thang { get; set; }
        public DateTime BD { get; set; }
        public DateTime KT { get; set; }
        public double ThoiGian { get; set; }
    }
}