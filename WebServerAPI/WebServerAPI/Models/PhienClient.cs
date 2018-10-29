using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServerAPI.Models
{
    public class PhienClient
    {
        public int MaSTT { get; set; }
        public int STT { get; set; }
        public double PhienCho { get; set; }
        public double PhienXuLy { get; set; }
        public double TongPhien { get; set; }
    }
}