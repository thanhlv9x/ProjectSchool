using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CallNumberWebsite.Models
{
    public class TaiKhoanUserNew
    {
        public int MaCB { get; set; }
        public string Id { get; set; }
        public string OldPw { get; set; }
        public string NewPw { get; set; }
    }
}