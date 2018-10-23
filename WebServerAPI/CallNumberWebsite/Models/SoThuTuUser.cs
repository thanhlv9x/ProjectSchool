using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CallNumberWebsite.Models
{
    public class SoThuTuUser
    {
        public int MaSTT { get; set; }
        public Nullable<int> STT { get; set; }
        public Nullable<int> MaCB { get; set; }
        public Nullable<System.DateTime> TG { get; set; }
    }
}