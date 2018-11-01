using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebServerAPI.EF;
using WebServerAPI.Models;

namespace WebServerAPI.Controllers
{
    public class GetNumberAPIController : ApiController
    {
        HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities();

        /// <summary>
        /// Dịch vụ lấy thông tin bộ phận và số thứ tự các bộ phận
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Get()
        {
            IList<Number> listMD = new List<Number>();
            var listEF = db.BOPHANs.ToList();
            if (listEF != null)
            {
                foreach (var item in listEF)
                {
                    int mabp = Convert.ToInt32(item.MABP);
                    DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                    DateTime dtEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                    Number NumberMD = new Number()
                    {
                        MaBP = mabp,
                        TenBP = item.TENBP
                    };
                    var number = db.SOTOIDAs.Where(p => p.MABP == mabp && p.TG >= dt && p.TG <= dtEnd)
                                            .OrderByDescending(p => p.STTTD)
                                            .FirstOrDefault();
                    if(number != null)
                    {
                        NumberMD.NumberBP = (int)number.STTTD;
                    }
                    else
                    {
                        NumberMD.NumberBP = 0;
                    }
                    listMD.Add(NumberMD);
                }
                return Request.CreateResponse<IList<Number>>(HttpStatusCode.OK, listMD);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
        /// <summary>
        /// Dịch vụ lấy số thứ tự của người dân theo bộ phận
        /// </summary>
        /// <param name="_MaBP">Mã bộ phận</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Get(int _MaBP)
        {
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DateTime dtEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            var stttdEF = db.SOTOIDAs.Where(p => p.MABP == _MaBP &&
                                                 p.TG >= dt &&
                                                 p.TG <= dtEnd)
                                     .OrderByDescending(p => p.STTTD)
                                     .FirstOrDefault();
            if (stttdEF != null)
            {
                DateTime now = DateTime.Now;
                int next = (int)stttdEF.STTTD + 1;
                SOTOIDA ef = new SOTOIDA()
                {
                    MABP = _MaBP,
                    STTTD = next,
                    TG = now
                };
                db.SOTOIDAs.Add(ef);
                db.SaveChanges();
                return Request.CreateResponse<int>(HttpStatusCode.OK, next);
            }
            else
            {
                DateTime now = DateTime.Now;
                int next = 1;
                SOTOIDA ef = new SOTOIDA()
                {
                    MABP = _MaBP,
                    STTTD = next,
                    TG = now
                };
                db.SOTOIDAs.Add(ef);
                db.SaveChanges();
                return Request.CreateResponse<int>(HttpStatusCode.OK, next);
            }
        }
        /// <summary>
        /// Dịch vụ lấy thông tin phiên giải quyết thủ tục theo ngày
        /// </summary>
        /// <param name="_MaCB">Mã cán bộ</param>
        /// <param name="_Ngay">Ngày</param>
        /// <param name="_Thang">Tháng</param>
        /// <param name="_Nam">Năm</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetPhien(int _MaCB, int _Ngay, int _Thang, int _Nam)
        {
            List<PhienClient> listMD = new List<PhienClient>();
            DateTime start = new DateTime(_Nam, _Thang, _Ngay, 0, 0, 0);
            DateTime end = new DateTime(_Nam, _Thang, _Ngay, 23, 59, 59);
            // Lấy thời gian xử lý thủ tục của cán bộ trong khoảng thời gian nhất định
            var listCB = db.CANBOes.Where(p => p.MACB == _MaCB).ToList();
            if (listCB.Count > 0)
            {
                foreach (var itemCB in listCB)
                {
                    var mabp = itemCB.MABP;
                    string tenbp = itemCB.BOPHAN.TENBP;
                    var macb = itemCB.MACB;
                    string hoten = itemCB.HOTEN;
                    var listEF = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == macb &&
                                                              p.TG >= start &&
                                                              p.TG <= end)
                                                  .OrderByDescending(p => p.MASTT)
                                                  .ToList();
                    foreach (var item in listEF)
                    {
                        var mastt = item.MASTT;
                        var stt = item.SOTHUTU.STT;
                        DateTime startSTT = new DateTime(((DateTime)item.TG).Year, ((DateTime)item.TG).Month, ((DateTime)item.TG).Day, 0, 0, 0);
                        DateTime endSTT = new DateTime(((DateTime)item.TG).Year, ((DateTime)item.TG).Month, ((DateTime)item.TG).Day, 23, 59, 59);
                        var rut = db.SOTOIDAs.Where(p => p.MABP == mabp &&
                                                         p.STTTD == stt &&
                                                         p.TG >= startSTT &&
                                                         p.TG <= endSTT)
                                             .FirstOrDefault();
                        var goi = db.SOTHUTUs.Where(p => p.MASTT == mastt & p.BD != null & p.KT != null).FirstOrDefault();
                        if (rut != null && goi != null)
                        {
                            double phiencho = Math.Round(Math.Abs(((TimeSpan)(goi.BD - rut.TG)).TotalMinutes), 0);
                            double phienxuly = Math.Round(Math.Abs(((TimeSpan)(goi.KT - goi.BD)).TotalMinutes), 0);
                            double tongphien = phiencho + phienxuly;
                            PhienClient md = new PhienClient()
                            {
                                MaSTT = mastt,
                                STT = (int)stt,
                                PhienCho = phiencho,
                                PhienXuLy = phienxuly,
                                TongPhien = tongphien
                            };
                            listMD.Add(md);
                        }
                    }
                }
                return Request.CreateResponse<List<PhienClient>>(HttpStatusCode.OK, listMD);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}
