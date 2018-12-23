using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServerAPI.Models
{
    public class BangXepLoai
    {
        public int Id { get; set; }
        public Nullable<double> Diem { get; set; }
        public string XepLoai { get; set; }
    }
}