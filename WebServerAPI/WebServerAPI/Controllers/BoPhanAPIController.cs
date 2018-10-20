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
    public class BoPhanAPIController : ApiController
    {
        HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities();

        /// <summary>
        /// Lấy thông tin tên các bộ phận
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<BoPhan> GetBP()
        {
            IList<BoPhan> listMD = new List<BoPhan>();
            var listEF = db.BOPHANs.ToList();
            foreach (var item in listEF)
            {
                BoPhan BoPhanMD = new BoPhan()
                {
                    MaBP = Convert.ToInt32(item.MABP),
                    TenBP = item.TENBP
                };
                listMD.Add(BoPhanMD);
            }
            return listMD;
        }

        /// <summary>
        /// Lấy thông tin tên các cán bộ theo bộ phận
        /// </summary>
        /// <param name="_MaBP">Mã bộ phận</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<CanBo> GetCB(int _MaBP)
        {
            IList<CanBo> listMD = new List<CanBo>();
            var listEF = db.CANBOes.Where(p => p.MABP == _MaBP)
                                   .Select(p => new
                                   {
                                       MACB = p.MACB,
                                       HOTEN = p.HOTEN,
                                       MABP = p.MABP
                                   })
                                   .ToList();
            foreach (var item in listEF)
            {
                CanBo canboMD = new CanBo()
                {
                    MaCB = Convert.ToInt32(item.MACB),
                    HoTen = item.HOTEN,
                    MaBP = Convert.ToInt32(item.MABP),
                };
                listMD.Add(canboMD);
            }
            return listMD;
        }
    }
}
