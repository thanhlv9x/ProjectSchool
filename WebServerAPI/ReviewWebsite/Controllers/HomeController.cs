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
        public static string url;
        public ActionResult Index()
        {
            url = "http://localhost:49930";
            return View();
        }

        /// <summary>
        /// Phương thức lấy mã máy
        /// </summary>
        /// <returns></returns>
        public JsonResult GetPort()
        {
            using(var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(url);
                }
                catch { }
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("api/ClientAPI/?_Port=1").Result;
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
        /// <summary>
        /// Phương thức lấy thông tin cán bộ đăng nhập vào mã máy đã chọn
        /// </summary>
        /// <param name="_MaMay">Mã máy</param>
        /// <returns></returns>
        public JsonResult GetInfo(int _MaMay)
        {
            using(var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(url);
                }
                catch
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("api/ClientAPI/?_MaMay=" + _MaMay).Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    if (result != null)
                    {
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Phương thức gọi số tự động
        /// </summary>
        /// <param name="_MaCB">Mã cán bộ</param>
        /// <param name="_MaBP">Mã bộ phận</param>
        /// <returns></returns>
        public JsonResult GetNumber(int _MaCB, int _MaBP)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
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
        /// <summary>
        /// Phương thức gửi kết quả đánh giá lên server
        /// </summary>
        /// <param name="_User"></param>
        /// <returns></returns>
        public JsonResult PostReview(KetQuaDanhGiaUser _User)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
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