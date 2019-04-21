using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebServerAPI.EF;
using WebServerAPI.Models;

namespace WebServerAPI.Controllers
{
    public class SMSController : Controller
    {
        // GET: SMS
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Read()
        {
            List<TinNhan> listMD = new List<TinNhan>();
            using (HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities())
            {
                var listEF = db.TINNHANs.ToList();
                foreach (var itemEF in listEF)
                {
                    TinNhan md = new TinNhan()
                    {
                        Id = itemEF.ID,
                        HoTen = itemEF.SODIENTHOAI.HOTEN,
                        MaVung = itemEF.SODIENTHOAI.VUNG1.ID,
                        IdSdt = itemEF.SODIENTHOAI.ID,
                        Sdt = itemEF.SODIENTHOAI.SDT,
                        Email = itemEF.SODIENTHOAI.EMAIL,
                        Bp1 = itemEF.BP1,
                        Bp2 = itemEF.BP2,
                        Bp3 = itemEF.BP3,
                        Bp4 = itemEF.BP4,
                        Bp5 = itemEF.BP5,
                        Bp6 = itemEF.BP6,
                        Bp7 = itemEF.BP7,
                        Bp8 = itemEF.BP8,
                        Bp9 = itemEF.BP9,
                        Bp10 = itemEF.BP10,
                        Bp11 = itemEF.BP11,
                        Bp12 = itemEF.BP12,
                        Bp13 = itemEF.BP13,
                        Bp14 = itemEF.BP14,
                        Bp15 = itemEF.BP15,
                    };
                    listMD.Add(md);
                }
            }
            return Json(listMD, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Create(List<TinNhan> model)
        {
            int indexCreate = 0;
            using (HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities())
            {
                foreach (var item in model)
                {
                    var ef = db.SODIENTHOAIs.Where(p => p.HOTEN == item.HoTen &&
                                                        p.SDT == item.Sdt &&
                                                        p.VUNG == item.MaVung &&
                                                        p.EMAIL == item.Email).FirstOrDefault();
                    if (ef != null)
                    {
                        item.IdSdt = ef.ID;
                    }
                    else if (ef == null)
                    {
                        SODIENTHOAI sdt = new SODIENTHOAI()
                        {
                            HOTEN = item.HoTen,
                            SDT = item.Sdt,
                            VUNG = item.MaVung,
                            EMAIL = item.Email
                        };
                        try
                        {
                            db.SODIENTHOAIs.Add(sdt);
                            db.SaveChanges();
                            item.IdSdt = sdt.ID;
                        }
                        catch { }
                    }
                    TINNHAN tn = new TINNHAN()
                    {
                        SDT = item.IdSdt,
                        EMAIL = item.Email,
                        BP1 = item.Bp1,
                        BP2 = item.Bp2,
                        BP3 = item.Bp3,
                        BP4 = item.Bp4,
                        BP5 = item.Bp5,
                        BP6 = item.Bp6,
                        BP7 = item.Bp7,
                        BP8 = item.Bp8,
                        BP9 = item.Bp9,
                        BP10 = item.Bp10,
                        BP11 = item.Bp11,
                        BP12 = item.Bp12,
                        BP13 = item.Bp13,
                        BP14 = item.Bp14,
                        BP15 = item.Bp15,
                    };
                    try
                    {
                        db.TINNHANs.Add(tn);
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

        public JsonResult Update(List<TinNhan> model)
        {
            int indexUpdate = 0;
            using (HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities())
            {
                foreach (var item in model)
                {
                    var ef = db.TINNHANs.Where(p => p.ID == item.Id).FirstOrDefault();
                    var sdtEF = db.SODIENTHOAIs.Where(p => p.HOTEN == item.HoTen &&
                                                           p.SDT == item.Sdt &&
                                                           p.VUNG == item.MaVung &&
                                                           p.EMAIL == item.Email).FirstOrDefault();
                    if (sdtEF != null)
                    {
                        item.IdSdt = sdtEF.ID;
                    }
                    else if (sdtEF == null)
                    {
                        SODIENTHOAI sdt = new SODIENTHOAI()
                        {
                            HOTEN = item.HoTen,
                            SDT = item.Sdt,
                            VUNG = item.MaVung,
                            EMAIL = item.Email
                        };
                        try
                        {
                            db.SODIENTHOAIs.Add(sdt);
                            db.SaveChanges();
                            item.IdSdt = sdt.ID;
                        }
                        catch { }
                    }
                    ef.SDT = item.IdSdt;
                    ef.EMAIL = item.Email;
                    ef.BP1 = item.Bp1;
                    ef.BP2 = item.Bp2;
                    ef.BP3 = item.Bp3;
                    ef.BP4 = item.Bp4;
                    ef.BP5 = item.Bp5;
                    ef.BP6 = item.Bp6;
                    ef.BP7 = item.Bp7;
                    ef.BP8 = item.Bp8;
                    ef.BP9 = item.Bp9;
                    ef.BP10 = item.Bp10;
                    ef.BP11 = item.Bp11;
                    ef.BP12 = item.Bp12;
                    ef.BP13 = item.Bp13;
                    ef.BP14 = item.Bp14;
                    ef.BP15 = item.Bp15;
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

        public JsonResult Delete(List<TinNhan> model)
        {
            int indexDelete = 0;
            using (HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities())
            {
                foreach (var item in model)
                {
                    var ef = db.TINNHANs.Where(p => p.ID == item.Id).FirstOrDefault();
                    try
                    {
                        db.TINNHANs.Remove(ef);
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

        public JsonResult GetMaVung()
        {
            List<MaVung> listMD = new List<MaVung>();
            using (HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities())
            {
                var listEF = db.VUNGs.ToList();
                foreach (var itemEF in listEF)
                {
                    MaVung md = new MaVung()
                    {
                        value = itemEF.ID,
                        text = itemEF.TENVUNG + " (" + itemEF.MAVUNG + ")",
                    };
                    listMD.Add(md);
                }
            }
            return Json(listMD, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBoPhan()
        {
            List<BoPhanSDT> listMD = new List<BoPhanSDT>();
            using(HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities())
            {
                var listEF = db.BOPHAN_SDT.ToList();
                foreach (var itemEF in listEF)
                {
                    BoPhanSDT md = new BoPhanSDT()
                    {
                        value = (int)itemEF.STT,
                        text = itemEF.BOPHAN.TENBP
                    };
                    listMD.Add(md);
                }
            }
            return Json(listMD, JsonRequestBehavior.AllowGet);
        }
    }
}