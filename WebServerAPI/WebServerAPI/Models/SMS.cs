using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServerAPI.Models
{
    public class SoDienThoai
    {
        public int Id { get; set; }
        public string HoTen { get; set; }
        public Nullable<int> Vung { get; set; }
        public Nullable<int> Sdt { get; set; }
    }

    public class MaVung
    {
        public int Id { get; set; }
        public string TenVung { get; set; }
        public string MaSoVung { get; set; }
        public int value { get; set; }
        public string text { get; set; }
    }

    public class TinNhan
    {
        public int Id { get; set; }
        public string HoTen { get; set; }
        public int MaVung { get; set; }
        public Nullable<int> IdSdt { get; set; }
        public Nullable<int> Sdt { get; set; }
        public Nullable<bool> Bp1 { get; set; }
        public Nullable<bool> Bp2 { get; set; }
        public Nullable<bool> Bp3 { get; set; }
        public Nullable<bool> Bp4 { get; set; }
        public Nullable<bool> Bp5 { get; set; }
        public Nullable<bool> Bp6 { get; set; }
        public Nullable<bool> Bp7 { get; set; }
        public Nullable<bool> Bp8 { get; set; }
        public Nullable<bool> Bp9 { get; set; }
        public Nullable<bool> Bp10 { get; set; }
        public Nullable<bool> Bp11 { get; set; }
        public Nullable<bool> Bp12 { get; set; }
        public Nullable<bool> Bp13 { get; set; }
        public Nullable<bool> Bp14 { get; set; }
        public Nullable<bool> Bp15 { get; set; }
    }

    public class BoPhanSDT
    {
        public int Id { get; set; }
        public int Stt { get; set; }
        public string TenBP { get; set; }
        public int value { get; set; }
        public string text { get; set; }
    }
}