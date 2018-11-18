using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServerAPI.Models
{
    public class KetQuaDanhGia_BaoCao_
    {
        public int MaBP { get; set; }
        public string TenBP { get; set; }
        public int MaCB { get; set; }
        public string HoTen { get; set; }
        public int RHL_SoLan { get; set; }
        public int HL_SoLan { get; set; }
        public int BT_SoLan { get; set; }
        public int KHL_SoLan { get; set; }
        public int TongCong_SoLan { get; set; }
        public double RHL_TyLe { get; set; }
        public double HL_TyLe { get; set; }
        public double BT_TyLe { get; set; }
        public double KHL_TyLe { get; set; }
        public double TongCong_TyLe { get; set; }
        public double Diem { get; set; }
        public string XepLoai { get; set; }
        public string MaCBSD { get; set; }
        public DateTime Ngay { get; set; }
    }
}