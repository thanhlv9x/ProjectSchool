using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServerAPI.Models
{
    public class ThongTinCanBo
    {
        public int MaCB { get; set; }
        public string HoTen { get; set; }
        public byte[] HinhAnh { get; set; }
        public string TenBP { get; set; }
        public int MaBP { get; set; }
        public string Id { get; set; }
        public string Pw { get; set; }
    }
}