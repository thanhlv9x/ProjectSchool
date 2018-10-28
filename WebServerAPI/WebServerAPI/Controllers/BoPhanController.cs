using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebServerAPI.EF;
using WebServerAPI.Models;

namespace WebServerAPI.Controllers
{
    public class BoPhanController : Controller
    {
        HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities();
        // GET: BoPhan
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Lấy toàn bộ dữ liệu thông tin bộ phận
        /// </summary>
        /// <returns></returns>
        public JsonResult Read()
        {
            var listEF = db.BOPHANs.OrderBy(p => p.MABP)
                                   .ToList();

            IList<BoPhan> listMD = new List<BoPhan>();
            foreach (var item in listEF)
            {
                BoPhan md = new BoPhan()
                {
                    MaBP = item.MABP,
                    TenBP = item.TENBP
                };
                listMD.Add(md);
            }

            return Json(listMD, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Thêm mới thông tin bộ phận
        /// </summary>
        /// <param name="model">Thông tin bộ phận</param>
        /// <returns></returns>
        public JsonResult Create(List<BoPhan> model)
        {
            bool success = false;
            foreach (var item in model)
            {
                try
                {
                    BOPHAN md = new BOPHAN()
                    {
                        //MABP = model.MaBP,
                        TENBP = item.TenBP
                    };
                    db.BOPHANs.Add(md);
                    db.SaveChanges();
                    success = true;
                }
                catch { }
            }
            return Json(success, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Cập nhật thông tin bộ phận
        /// </summary>
        /// <param name="model">Thông tin bộ phận</param>
        /// <returns></returns>
        public JsonResult Update(List<BoPhan> model)
        {
            bool success = false;
            foreach (var item in model)
            {
                try
                {
                    var mabp = item.MaBP;
                    var md = db.BOPHANs.Where(p => p.MABP == mabp).FirstOrDefault();
                    md.TENBP = item.TenBP;
                    db.SaveChanges();
                    success = true;
                }
                catch { }
            }
            return Json(success, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Phương thức xóa thông tin bộ phận
        /// </summary>
        /// <param name="model">Bộ phận cần xóa</param>
        /// <returns></returns>
        public JsonResult Delete(List<BoPhan> model)
        {
            bool success = false;
            foreach (var item in model)
            {
                try
                {
                    var mabp = item.MaBP;
                    var md = db.BOPHANs.Where(p => p.MABP == mabp).FirstOrDefault();
                    db.BOPHANs.Remove(md);
                    db.SaveChanges();
                    success = true;
                }
                catch { }
            }
            return Json(success, JsonRequestBehavior.AllowGet);
        }

    }
}