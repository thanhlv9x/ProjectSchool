using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServerAPI.Models
{
    public class CanBo
    {
        public int MaCB { get; set; }
        public string HoTen { get; set; }
        public Nullable<int> MaBP { get; set; }
        public string HinhAnh { get; set; }
        public string Id { get; set; }
        public string Pw { get; set; }
        public string TenBP { get; set; }
        public string MaCBSD { get; set; }
        public string VietTat { get; set; }
    }
}