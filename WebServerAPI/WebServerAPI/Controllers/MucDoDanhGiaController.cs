using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebServerAPI.EF;
using WebServerAPI.Models;

namespace WebServerAPI.Controllers
{
    public class MucDoDanhGiaController : Controller
    {
        // GET: MucDoDanhGia
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Read()
        {
            List<MucDoDanhGia> listMD = new List<MucDoDanhGia>();
            using (HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities())
            {
                var listEF = db.MUCDODANHGIAs.ToList();
                foreach (var itemEF in listEF)
                {
                    MucDoDanhGia md = new MucDoDanhGia()
                    {
                        MucDo = itemEF.MUCDO,
                        Loai = itemEF.LOAI,
                        Diem = itemEF.DIEM
                    };
                    listMD.Add(md);
                }
            }
            return Json(listMD, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Create(List<MucDoDanhGia> model)
        {
            int indexCreate = 0;
            foreach (var item in model)
            {
                using (HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities())
                {
                    MUCDODANHGIA ef = new MUCDODANHGIA()
                    {
                        MUCDO = item.MucDo,
                        LOAI = item.Loai,
                        DIEM = item.Diem
                    };
                    try
                    {
                        db.MUCDODANHGIAs.Add(ef);
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

        public JsonResult Update(List<MucDoDanhGia> model)
        {
            int indexUpdate = 0;
            foreach (var item in model)
            {
                using (HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities())
                {
                    var ef = db.MUCDODANHGIAs.Where(p => p.MUCDO == item.MucDo).FirstOrDefault();
                    ef.MUCDO = item.MucDo;
                    ef.DIEM = item.Diem;
                    ef.LOAI = item.Loai;
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

        public JsonResult Delete(List<MucDoDanhGia> model)
        {
            int indexDelete = 0;
            foreach (var item in model)
            {
                using (HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities())
                {
                    var ef = db.MUCDODANHGIAs.Where(p => p.MUCDO == item.MucDo).FirstOrDefault();
                    try
                    {
                        db.MUCDODANHGIAs.Remove(ef);
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