using CallNumberWebsite.Common;
using CallNumberWebsite.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
        /// <summary>
        /// Phương thức lấy thông tin cá nhân của Cán bộ
        /// </summary>
        /// <returns></returns>
        public JsonResult GetInfo()
        {
            return Json(Session[CommonConstants.USER_RESULT], JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Phương thức lấy thông tin phiên giải quyết thủ tục
        /// </summary>
        /// <param name="_MaCB">Mã cán bộ</param>
        /// <param name="_Ngay">Ngày</param>
        /// <param name="_Thang">Tháng</param>
        /// <param name="_Nam">Năm</param>
        /// <returns></returns>
        public JsonResult GetPhien(int _MaCB, int _Ngay, int _Thang, int _Nam)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(InfoUser.URL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = client.GetAsync("api/GetNumberAPI/?_MaCB=" + _MaCB + "&_Ngay=" + _Ngay + "&_Thang=" + _Thang + "&_Nam=" + _Nam).Result;

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
        /// Phương thức lưu lại thông tin cán bộ
        /// </summary>
        /// <param name="_MaCB">Mã cán bộ</param>
        /// <param name="_MaBP">Mã bộ phận</param>
        /// <param name="_MaMay">Mã máy</param>
        /// <returns></returns>
        public JsonResult SaveInfo(int _MaCB, int _MaBP, int _MaMay, int _MaDN)
        {
            Session[CommonConstants.USER_MACB] = _MaCB;
            Session[CommonConstants.USER_MABP] = _MaBP;
            Session[CommonConstants.USER_MAMAY] = _MaMay;
            Session[CommonConstants.USER_MADN] = _MaDN;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Phương thức lưu lại số đã gọi trước đó
        /// </summary>
        /// <param name="_MaSTT">Mã số thứ tự trước</param>
        /// <returns></returns>
        public JsonResult SaveNumber(int _MaSTT)
        {
            Session[CommonConstants.USER_MASTT] = _MaSTT;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Lấy số thứ tự trước
        /// </summary>
        /// <param name="_MaCB">Mã cán bộ</param>
        /// <returns></returns>
        public JsonResult GetNumber(int _MaCB)
        {
            return Json(Session[CommonConstants.USER_MASTT], JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Phương thức gọi số thứ tự theo bộ phận
        /// </summary>
        /// <param name="_MaCB">Mã cán bộ</param>
        /// <param name="_MaBP">Mã bộ phận</param>
        /// <returns></returns>
        public JsonResult CallNumber(int _MaCB, int _MaBP)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    int mastt;
                    client.BaseAddress = new Uri(InfoUser.URL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        mastt = (int)Session[CommonConstants.USER_MASTT];
                    }
                    catch
                    {
                        mastt = 0;
                    }
                    HttpResponseMessage response = client.GetAsync("api/ClientAPI/?_MaCB=" + _MaCB + "&_MaBP=" + _MaBP + "&_MaSTT=" + mastt + "&_Submit=1").Result;

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
        /// Phương thức thay đổi mật khẩu
        /// </summary>
        /// <param name="_MKCu">Mật khẩu cũ</param>
        /// <param name="_MKMoi">Mật khẩu mới</param>
        /// <returns></returns>
        public JsonResult ChangePW(string _MKCu, string _MKMoi)
        {
            _MKCu = GetMD5(_MKCu);
            _MKMoi = GetMD5(_MKMoi);
            TaiKhoanUserNew md = new TaiKhoanUserNew()
            {
                Id = Session[CommonConstants.USER_SESSION].ToString(),
                MaCB = (int)Session[CommonConstants.USER_MACB],
                OldPw = _MKCu,
                NewPw = _MKMoi
            };
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(InfoUser.URL);
                }
                catch { }
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;
                // Serialize our concrete class into a JSON String
                var stringPayload = JsonConvert.SerializeObject(md);

                // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                try
                {
                    response = client.PutAsync("api/ClientAPI/?_MaCB=" + md.MaCB, httpContent).Result;
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
                        return Json(true, JsonRequestBehavior.AllowGet);
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
        /// Phương thức mã hóa MD5
        /// </summary>
        /// <param name="txt">Chuỗi cần mã hóa</param>
        /// <returns></returns>
        public static String GetMD5(string txt)
        {
            String str = "";
            Byte[] buffer = System.Text.Encoding.UTF8.GetBytes(txt);
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            buffer = md5.ComputeHash(buffer);
            foreach (Byte b in buffer)
            {
                str += b.ToString("X2");
            }
            return str;
        }
    }
}