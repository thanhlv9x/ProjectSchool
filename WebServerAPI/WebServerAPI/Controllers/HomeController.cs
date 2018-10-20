using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebServerAPI.Common;

namespace WebServerAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session[CommonConstants.ADMIN_SESSION] != null)
            {
                ViewBag.Title = "Home Page";
                string thuMucGoc = AppDomain.CurrentDomain.BaseDirectory;
                string thuMucHinh = thuMucGoc + @"\resources\";
                if (!Directory.Exists(thuMucHinh))
                {
                    Directory.CreateDirectory(thuMucHinh);
                }
                string thuMucFile = thuMucGoc + @"\Files\";
                if (!Directory.Exists(thuMucFile))
                {
                    Directory.CreateDirectory(thuMucFile);
                }
                return View();
            }
            else
            {
                Session[CommonConstants.ADMIN_SESSION] = null;
                return Redirect("/Login");
            }
        }
    }
}
