using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServerAPI.Models
{
    public class SMSKey
    {
        public int Id { get; set; }
        public string APIKey { get; set; }
        public string SecretKey { get; set; }
    }
}