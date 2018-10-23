using CallNumberWebsite.Common;
using CallNumberWebsite.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace CallNumberWebsite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session[CommonConstants.USER_SESSION] != null)
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
                Session[CommonConstants.USER_SESSION] = null;
                Session[CommonConstants.USER_RESULT] = null;
                return Redirect("/Login");
            }
        }
        public JsonResult GetInfo()
        {
            return Json(Session[CommonConstants.USER_RESULT], JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNumber(int _MaCB)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(InfoUser.URL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = client.GetAsync("api/ClientAPI/?_MaCB=" + _MaCB + "&_Refresh=1").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                        if (result != null)
                        {
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(false, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CallNumber(int _MaCB, int _MaBP, int _MaSTT)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(InfoUser.URL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = client.GetAsync("api/ClientAPI/?_MaCB=" + _MaCB + "&_MaBP=" + _MaBP + "&_MaSTT=" + _MaSTT + "&_Submit=1").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                        if (result != null)
                        {
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(false, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}