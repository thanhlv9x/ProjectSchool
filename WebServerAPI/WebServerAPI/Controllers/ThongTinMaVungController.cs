using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebServerAPI.EF;
using WebServerAPI.Models;

namespace WebServerAPI.Controllers
{
    public class ThongTinMaVungController : Controller
    {
        // GET: ThongTinMaVung
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Read()
        {
            List<ThongTinMaVung> listMD = new List<ThongTinMaVung>();
            using (HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities())
            {
                var listEF = db.VUNGs.ToList();
                foreach (var itemEF in listEF)
                {
                    ThongTinMaVung md = new ThongTinMaVung()
                    {
                        Id = itemEF.ID,
                        MaVung = itemEF.MAVUNG,
                        TenVung = itemEF.TENVUNG
                    };
                    listMD.Add(md);
                }
            }
            return Json(listMD, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Create(List<ThongTinMaVung> model)
        {
            int indexCreate = 0;
            foreach (var item in model)
            {
                using (HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities())
                {
                    VUNG ef = new VUNG()
                    {
                        ID = item.Id,
                        MAVUNG = item.MaVung,
                        TENVUNG = item.TenVung
                    };
                    try
                    {
                        db.VUNGs.Add(ef);
                        db.SaveChanges();
                        indexCreate++;
                    }
                    catch { }
                }
            }
            if (indexCreate > 0)
            {
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Update(List<ThongTinMaVung> model)
        {
            int indexUpdate = 0;
            foreach (var item in model)
            {
                using (HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities())
                {
                    var ef = db.VUNGs.Where(p => p.ID == item.Id).FirstOrDefault();
                    ef.MAVUNG = item.MaVung;
                    ef.TENVUNG = item.TenVung;
                    try
                    {
                        db.SaveChanges();
                        indexUpdate++;
                    }
                    catch { }
                }
            }
            if (indexUpdate > 0)
            {
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Delete(List<ThongTinMaVung> model)
        {
            int indexDelete = 0;
            foreach (var item in model)
            {
                using (HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities())
                {
                    var ef = db.VUNGs.Where(p => p.ID == item.Id).FirstOrDefault();
                    try
                    {
                        db.VUNGs.Remove(ef);
                        db.SaveChanges();
                        indexDelete++;
                    }
                    catch { }
                }
            }
            if (indexDelete > 0)
            {
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
    }
}