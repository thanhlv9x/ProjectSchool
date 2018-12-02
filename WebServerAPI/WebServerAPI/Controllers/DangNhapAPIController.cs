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
    public class DangNhapAPIController : ApiController
    {
        HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities();

        /// <summary>
        /// Lấy thông tin đăng nhập của cán bộ
        /// </summary>
        /// <param name="_MaCB">Mã cán bộ</param>
        /// <param name="_Loai">Loại lọc (Năm hoặc tháng)</param>
        /// <param name="_ThoiGian">Thời gian theo loại lọc</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<ThoiGianDangNhap> Get(int _MaCB, string _Loai, string _ThoiGian)
        {
            IList<ThoiGianDangNhap> lstMD = new List<ThoiGianDangNhap>();
            if (_Loai == "nam")
            {
                int Nam = Convert.ToInt32(_ThoiGian);
                var canbo = db.CANBOes.Where(p => p.MACB == _MaCB).FirstOrDefault();
                var macbsd = canbo.MACBSD;
                var hoten = canbo.HOTEN;
                for (int i = 1; i < 13; i++)
                {
                    DateTime start = new DateTime(Nam, i, 1, 0, 0, 0);
                    DateTime end = new DateTime(Nam, i, DateTime.DaysInMonth(Nam, i), 23, 59, 59);
                    var lstEF = db.TRANGTHAIDANGNHAPs.Where(p => p.MACB == _MaCB &&
                                                                 p.BD != null &&
                                                                 p.KT != null &&
                                                                 p.BD >= start &&
                                                                 p.BD <= end &&
                                                                 p.KT >= start &&
                                                                 p.KT <= end)
                                                     .OrderBy(p => p.MACB)
                                                     .ToList();
                    double time = 0;
                    foreach (var item in lstEF)
                    {
                        time += Math.Round(Math.Abs(((TimeSpan)(item.KT - item.BD)).TotalMinutes), 0);
                        DateTime dt = (DateTime)item.BD;
                        DateTime now = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
                        ThoiGianDangNhap md = new ThoiGianDangNhap()
                        {
                            MaMay = (int)item.MAYDANHGIA.MAC,
                            Ngay = now,
                            Thang = start,
                            BD = (DateTime)item.BD,
                            KT = (DateTime)item.KT,
                            ThoiGian = time,
                        };
                        lstMD.Add(md);
                    }
                }
            }
            else if (_Loai == "thang")
            {
                string[] arr = _ThoiGian.Split(' ');
                int Nam = Convert.ToInt32(arr[1]);
                int Thang = Convert.ToInt32(arr[0]);
                DateTime start = new DateTime(Nam, Thang, 1, 0, 0, 0);
                DateTime end = new DateTime(Nam, Thang, DateTime.DaysInMonth(Nam, Thang), 23, 59, 59);
                var lstEF = db.TRANGTHAIDANGNHAPs.Where(p => p.MACB == _MaCB &&
                                                             p.BD != null &&
                                                             p.KT != null &&
                                                             p.BD >= start &&
                                                             p.BD <= end &&
                                                             p.KT >= start &&
                                                             p.KT <= end)
                                                 .OrderBy(p => p.MACB)
                                                 .ToList();
                foreach (var item in lstEF)
                {
                    double time = Math.Round(Math.Abs(((TimeSpan)(item.KT - item.BD)).TotalMinutes), 0);
                    DateTime dt = (DateTime)item.BD;
                    DateTime now = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
                    ThoiGianDangNhap md = new ThoiGianDangNhap()
                    {
                        MaMay = (int)item.MAYDANHGIA.MAC,
                        Ngay = now,
                        BD = (DateTime)item.BD,
                        KT = (DateTime)item.KT,
                        ThoiGian = time,
                    };
                    lstMD.Add(md);
                }
            }
            return lstMD;
        }
    }
}
