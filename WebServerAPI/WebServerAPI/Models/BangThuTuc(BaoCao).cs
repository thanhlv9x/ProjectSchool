using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServerAPI.Models
{
    public class BangThuTuc_BaoCao_
    {
        public int MaBP { get; set; }
        public string TenBP { get; set; }
        public int MaCB { get; set; }
        public string HoTen { get; set; }
        public double PhienCho { get; set; }
        public double PhienXuLy { get; set; }
        public double TongPhien { get; set; }
        public string MaCBSD { get; set; }
        public string VietTat { get; set; }
        public int STT { get; set; }
        public int SoLuong { get; set; }
    }
}