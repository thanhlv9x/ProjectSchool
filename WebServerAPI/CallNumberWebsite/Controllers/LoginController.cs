using Newtonsoft.Json;
using CallNumberWebsite.Common;
using CallNumberWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CallNumberWebsite.Controllers
{
    public class LoginController : Controller
    {
        public static string url;
        public static string result;

        // GET: Login
        public ActionResult Index()
        {
            url = "http://localhost:49930";
            InfoUser.URL = url;
            if (Session[CommonConstants.USER_SESSION] != null)
            {
                return Redirect("/Home");
            }
            else
            {
                Session[CommonConstants.USER_SESSION] = null;
                Session[CommonConstants.USER_RESULT] = null;
                return View();
            }
        }

        /// <summary>
        /// Phương thức login vào hệ thống
        /// </summary>
        /// <param name="model">Tài khoản và mật khẩu</param>
        /// <returns></returns>
        public ActionResult Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var check = CheckLogin(model.Id, model.Pw);
                if (check)
                {
                    var UserID = model.Id;

                    Session.Add(CommonConstants.USER_SESSION, UserID);
                    Session.Add(CommonConstants.USER_RESULT, result);

                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập không thành công");
                }
            }
            return View("Index");
        }

        /// <summary>
        /// Phương thức thoát ra chuyển về trang login
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null;
            Session[CommonConstants.USER_RESULT] = null;
            return Redirect("/Home");
        }

        /// <summary>
        /// Kiểm tra tài khoản và mật khẩu
        /// </summary>
        /// <param name="id">Tài khoản</param>
        /// <param name="pw">Mật khẩu</param>
        /// <returns></returns>
        public bool CheckLogin(string id, string pw)
        {
            pw = GetMD5(pw);
            TaiKhoanUser user = new TaiKhoanUser()
            {
                Id = id,
                Pw = pw
            };
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(InfoUser.URL);
                }
                catch
                {

                }
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;
                // Serialize our concrete class into a JSON String
                var stringPayload = JsonConvert.SerializeObject(user);

                // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                try
                {
                    response = client.PostAsync("api/ClientAPI", httpContent).Result;
                }
                catch (Exception ex)
                {
                    return false;
                }

                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                    if (result != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Phương thức mã hóa MD5
        /// </summary>
        /// <param name="txt">Chuỗi cần mã hóa</param>
        /// <returns></returns>
        private String GetMD5(string txt)
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