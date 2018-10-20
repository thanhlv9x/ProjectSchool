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
                    Number NumberMD = new Number()
                    {
                        MaBP = mabp,
                        TenBP = item.TENBP
                    };
                    var number = db.SOTOIDAs.Where(p => p.MABP == mabp && p.TG >= dt)
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
            var stttdEF = db.SOTOIDAs.Where(p => p.MABP == _MaBP &&
                                                 p.TG >= dt)
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

    }
}
