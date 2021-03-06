﻿using System;
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
        public Nullable<int> MaSTT { get; set; }
        public Nullable<int> MaMay { get; set; }
        public Nullable<int> MaDN { get; set; }
        public string MaCBSD { get; set; }
        public string VietTat { get; set; }
        public DateTime BD { get; set; }
        public string Mac { get; set; }
        public string TenBP { get; set; }
        public string HoTen { get; set; }
        public string HinhAnh { get; set; }
    }
}