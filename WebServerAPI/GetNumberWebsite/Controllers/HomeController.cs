using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using GetNumberWebsite.Models;
using Newtonsoft.Json.Linq;

namespace GetNumberWebsite.Controllers
{
    public class HomeController : Controller
    {
        public static string basePath;

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Lấy thông tin bộ phận và số thứ tự
        /// </summary>
        /// <returns></returns>
        public JsonResult GetBP(string url)
        {
            // "http://localhost:61443"
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GetUriServer.GetUri());
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = client.GetAsync("api/GetNumberAPI/").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        return Json(response.Content.ReadAsStringAsync().Result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
                catch
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
        }
        /// <summary>
        /// Lấy số thứ tự
        /// </summary>
        /// <param name="_MaBP"></param>
        /// <returns></returns>
        public JsonResult GetSTT(int _MaBP, string url)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GetUriServer.GetUri());
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = client.GetAsync("api/GetNumberAPI/?_MaBP=" + _MaBP).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(response.Content.ReadAsStringAsync().Result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
                catch
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}