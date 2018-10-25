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
        public static string strIP = "localhost:49930";
        public static string basePath;

        public ActionResult Index()
        {
            try
            {
                basePath = AppDomain.CurrentDomain.BaseDirectory;
                if (!System.IO.File.Exists(basePath + "id.txt"))
                {
                    using (StreamWriter sw = new StreamWriter(basePath + "id.txt"))
                    {
                        sw.WriteLine("admin");
                    }
                }
                // Đọc dữ liệu từ file idaddress.txt
                string line = "";
                using (StreamReader sr = new StreamReader(basePath + "ipaddress.txt"))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        strIP = line;
                    }
                }
            }
            catch { }
            return View();
        }

        /// <summary>
        /// Lấy thông tin bộ phận và số thứ tự
        /// </summary>
        /// <returns></returns>
        public JsonResult GetBP()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://" + strIP + "/");
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
        public JsonResult GetSTT(int _MaBP)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://" + strIP + "/");
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
        /// <summary>
        /// Thay đổi địa chỉ IP
        /// </summary>
        /// <param name="ip">Địa chỉ IP</param>
        /// <param name="id">Tên tài khoản</param>
        /// <param name="pw">Mật khẩu</param>
        /// <returns></returns>
        public JsonResult SettingIP(string ip, string id)
        {
            try
            {
                bool access = false;
                // Đọc dữ liệu từ file idaddress.txt
                string line = "";
                using (StreamReader sr = new StreamReader(basePath + "id.txt"))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (id == line) access = true;
                    }
                }
                if (access)
                {
                    using (StreamWriter sw = new StreamWriter(basePath + "ipaddress.txt"))
                    {
                        sw.WriteLine(ip);
                    }
                    strIP = ip;
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}