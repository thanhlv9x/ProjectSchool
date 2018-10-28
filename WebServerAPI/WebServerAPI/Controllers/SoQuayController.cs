using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebServerAPI.EF;
using WebServerAPI.Models;

namespace WebServerAPI.Controllers
{
    public class SoQuayController : Controller
    {
        HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities();
        // GET: SoQuay
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Lấy toàn bộ dữ liệu thông tin số quầy
        /// </summary>
        /// <returns></returns>
        public JsonResult Read()
        {
            var listEF = db.MAYDANHGIAs.OrderBy(p => p.MAMAY).ToList();
            IList<SoQuay> listMD = new List<SoQuay>();
            foreach (var item in listEF)
            {
                SoQuay md = new SoQuay()
                {
                    MaMay = item.MAMAY,
                    Mac = item.MAC
                };
                listMD.Add(md);
            }
            return Json(listMD, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Thêm mới thông tin số quầy
        /// </summary>
        /// <param name="model">Thông tin quầy</param>
        /// <returns></returns>
        public JsonResult Create(List<SoQuay> model)
        {
            bool success = false;
            foreach (var item in model)
            {
                try
                {
                    MAYDANHGIA md = new MAYDANHGIA()
                    {
                        MAMAY = item.MaMay,
                        MAC = item.Mac
                    };
                    db.MAYDANHGIAs.Add(md);
                    db.SaveChanges();
                    success = true;
                }
                catch { }
            }
            return Json(success, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Cập nhật thông tin số quầy
        /// </summary>
        /// <param name="model">Thông tin số quầy</param>
        /// <returns></returns>
        public JsonResult Update(List<SoQuay> model)
        {
            bool success = false;
            foreach (var item in model)
            {
                try
                {
                    var mamay = item.MaMay;
                    var md = db.MAYDANHGIAs.Where(p => p.MAMAY == mamay).FirstOrDefault();
                    md.MAC = item.Mac;
                    db.SaveChanges();
                    success = true;
                }
                catch { }
            }
            return Json(success, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Phương thức xóa thông tin số quầy
        /// </summary>
        /// <param name="model">Số quầy cần xóa</param>
        /// <returns></returns>
        public JsonResult Delete(List<SoQuay> model)
        {
            bool success = false;
            foreach (var item in model)
            {
                try
                {
                    var mamay = item.MaMay;
                    var md = db.MAYDANHGIAs.Where(p => p.MAMAY == mamay).FirstOrDefault();
                    db.MAYDANHGIAs.Remove(md);
                    db.SaveChanges();
                    success = true;
                }
                catch { }
            }
            return Json(success, JsonRequestBehavior.AllowGet);
        }

    }
}