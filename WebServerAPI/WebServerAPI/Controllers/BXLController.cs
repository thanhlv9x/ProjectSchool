using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebServerAPI.EF;
using WebServerAPI.Models;

namespace WebServerAPI.Controllers
{
    public class BXLController : Controller
    {
        // GET: BXL
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Read()
        {
            List<BangXepLoai> listMD = new List<BangXepLoai>();
            using (HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities())
            {
                var listEF = db.BANGXEPLOAIs.ToList();
                foreach (var itemEF in listEF)
                {
                    BangXepLoai md = new BangXepLoai()
                    {
                        Id = itemEF.ID,
                        Diem = itemEF.DIEM,
                        XepLoai = itemEF.XEPLOAI
                    };
                    listMD.Add(md);
                }
            }
            return Json(listMD, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Create(List<BangXepLoai> model)
        {
            int indexCreate = 0;
            foreach (var item in model)
            {
                using (HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities())
                {
                    BANGXEPLOAI ef = new BANGXEPLOAI()
                    {
                        DIEM = item.Diem,
                        XEPLOAI = item.XepLoai,
                    };
                    try
                    {
                        db.BANGXEPLOAIs.Add(ef);
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

        public JsonResult Update(List<BangXepLoai> model)
        {
            int indexUpdate = 0;
            foreach (var item in model)
            {
                using (HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities())
                {
                    var ef = db.BANGXEPLOAIs.Where(p => p.ID == item.Id).FirstOrDefault();
                    ef.DIEM = item.Diem;
                    ef.XEPLOAI = item.XepLoai;
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

        public JsonResult Delete(List<BangXepLoai> model)
        {
            int indexDelete = 0;
            foreach (var item in model)
            {
                using (HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities())
                {
                    var ef = db.BANGXEPLOAIs.Where(p => p.ID == item.Id).FirstOrDefault();
                    try
                    {
                        db.BANGXEPLOAIs.Remove(ef);
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