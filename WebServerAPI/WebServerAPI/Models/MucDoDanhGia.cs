using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServerAPI.Models
{
    public class MucDoDanhGia
    {
        public int MucDo { get; set; }
        public string Loai { get; set; }
        public Nullable<double> Diem { get; set; }
    }
}