﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace ShowNumber.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Phương thức lấy số thứ tự và số quầy để hiển thị lên màn hình lớn gọi số
        /// </summary>
        /// <returns></returns>
        public JsonResult GetInfoCallNumber()
        {
            try
            {
                using(var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(GetUriServer.GetUri());
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync("/api/ClientAPI/?_ShowNumber=1").Result;
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
        /// Phương thức lấy số quầy để hiển thị lên màn hình lớn gọi số
        /// </summary>
        /// <returns></returns>
        public JsonResult GetInfoSoQuay()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(GetUriServer.GetUri());
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync("/api/ClientAPI/?_Port=1").Result;
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