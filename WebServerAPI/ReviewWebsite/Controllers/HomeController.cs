using Newtonsoft.Json;
using ReviewWebsite.Common;
using ReviewWebsite.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ReviewWebsite.Controllers
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

        public JsonResult GetNumber(int _MaCB, int _MaBP)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(InfoUser.URL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = client.GetAsync("api/ClientAPI/?_MaCB=" + _MaCB + "&_MaBP=" + _MaBP + "&_Interval=1").Result;

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

        public JsonResult PostReview(KetQuaDanhGiaUser _User)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(InfoUser.URL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response;
                    // Serialize our concrete class into a JSON String
                    var stringPayload = JsonConvert.SerializeObject(_User);

                    // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                    var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                    try
                    {
                        response = client.PostAsync("api/ClientAPI/?_Success=1", httpContent).Result;
                    }
                    catch (Exception ex)
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }

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