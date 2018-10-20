using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebServerAPI.Common;
using WebServerAPI.EF;
using WebServerAPI.Models;

namespace WebServerAPI.Controllers
{
    public class LoginController : Controller
    {
        HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities();
        // GET: Login
        public ActionResult Index()
        {
            if (Session[CommonConstants.ADMIN_SESSION] != null)
            {                
                return Redirect("/Home");
            }
            else
            {
                Session[CommonConstants.ADMIN_SESSION] = null;
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
                var result = CheckLogin(model.Id, model.Pw);
                if (result)
                {
                    var UserID = model.Id;

                    Session.Add(CommonConstants.ADMIN_SESSION, UserID);

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
            Session[CommonConstants.ADMIN_SESSION] = null;
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
            //pw = GetMD5(pw);
            int result = db.TAIKHOANADMINs.Count(p => p.ID == id && p.PW == pw);
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
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