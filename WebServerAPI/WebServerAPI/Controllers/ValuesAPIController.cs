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
    public class ValuesAPIController : ApiController
    {
        HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities();
        #region example
        // GET api/values
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //public void Delete(int id)
        //{
        //}
        #endregion

        /// <summary>
        /// Lấy thời gian của kết quả đánh giá
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<string> GetDate()
        {
            IList<string> listMD = new List<string>();
            try
            {
                var start = db.KETQUADANHGIAs.OrderBy(p => p.TG)
                                             .FirstOrDefault();
                int ngayS = ((DateTime)start.TG).Day;
                int thangS = ((DateTime)start.TG).Month;
                int namS = ((DateTime)start.TG).Year;
                string BatDau = namS + "-" + thangS + "-" + ngayS;
                var end = db.KETQUADANHGIAs.OrderByDescending(p => p.TG)
                                           .FirstOrDefault();

                int ngayE = ((DateTime)end.TG).Day;
                int thangE = ((DateTime)end.TG).Month;
                int namE = ((DateTime)end.TG).Year;
                string KetThuc = namE + "-" + thangE + "-" + ngayE;
                listMD.Add(BatDau);
                listMD.Add(KetThuc);
                return listMD;
            }
            catch
            {
                return listMD;
            }
        }

        /// <summary>
        /// Lấy thời gian của kết quả đánh giá theo bộ phận
        /// </summary>
        /// <param name="_MaBP">Mã bộ phận</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<string> GetDateBP(int _MaBP)
        {
            IList<string> listMD = new List<string>();
            try
            {
                var start = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.CANBO.MABP == _MaBP)
                                             .OrderBy(p => p.TG)
                                             .FirstOrDefault();
                int ngayS = ((DateTime)start.TG).Day;
                int thangS = ((DateTime)start.TG).Month;
                int namS = ((DateTime)start.TG).Year;
                string BatDau = namS + "-" + thangS + "-" + ngayS;
                var end = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.CANBO.MABP == _MaBP)
                                           .OrderByDescending(p => p.TG)
                                           .FirstOrDefault();

                int ngayE = ((DateTime)end.TG).Day;
                int thangE = ((DateTime)end.TG).Month;
                int namE = ((DateTime)end.TG).Year;
                string KetThuc = namE + "-" + thangE + "-" + ngayE;
                listMD.Add(BatDau);
                listMD.Add(KetThuc);
                return listMD;
            }
            catch
            {
                return listMD;
            }
        }

        /// <summary>
        /// Lấy thời gian của kết quả đánh giá theo cán bộ
        /// </summary>
        /// <param name="_MaCB">Mã cán bộ</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<string> GetDateCB(int _MACB)
        {
            IList<string> listMD = new List<string>();
            try
            {
                var start = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == _MACB)
                                             .OrderBy(p => p.TG)
                                             .FirstOrDefault();
                int ngayS = ((DateTime)start.TG).Day;
                int thangS = ((DateTime)start.TG).Month;
                int namS = ((DateTime)start.TG).Year;
                string BatDau = namS + "-" + thangS + "-" + ngayS;
                var end = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == _MACB)
                                           .OrderByDescending(p => p.TG)
                                           .FirstOrDefault();

                int ngayE = ((DateTime)end.TG).Day;
                int thangE = ((DateTime)end.TG).Month;
                int namE = ((DateTime)end.TG).Year;
                string KetThuc = namE + "-" + thangE + "-" + ngayE;
                listMD.Add(BatDau);
                listMD.Add(KetThuc);
                return listMD;
            }
            catch
            {
                return listMD;
            }
        }
    }
}
