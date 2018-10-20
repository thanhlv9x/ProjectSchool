using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServerAPI.Models
{
    public class DanhGiaUser
    {
        public int MaDG { get; set; }
        public Nullable<int> MucDo { get; set; }
        public string GopY { get; set; }
        public Nullable<System.DateTime> ThoiGian { get; set; }
        public int MaSTT { get; set; }
    }
}