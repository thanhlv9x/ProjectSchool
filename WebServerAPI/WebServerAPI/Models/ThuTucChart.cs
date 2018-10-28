using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServerAPI.Models
{
    public class ThuTucChart
    {
        public int MaSTT { get; set; }
        public int SoThuTu { get; set; }
        public int MaBP { get; set; }
        public string TenBP { get; set; }
        public int MaCB { get; set; }
        public string HoTen { get; set; }
        public DateTime ThoiGianRut { get; set; }
        public DateTime ThoiGianGoi { get; set; }
        public DateTime ThoiGianHoanTat { get; set; }
        public double ThoiGianCho { get; set; }
        public double ThoiGianGiaiQuyet { get; set; }
        public double TongThoiGian { get; set; }
        public string ThoiGian { get; set; }
        public DateTime Ngay { get; set; }
    }
}