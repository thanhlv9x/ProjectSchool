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
    public class CotDanhGiaAPIController : ApiController
    {
        HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities();

        /// <summary>
        /// Lấy kết quả đánh giá của bộ phận theo từng tháng trong năm
        /// </summary>
        /// <param name="_MaBP">Mã bộ phận</param>
        /// <param name="_Year">Năm</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<KetQuaDanhGia_Column_> GetKetQuaDanhGiaAll(int _MaBP, string _Year)
        {
            // Tạo khoảng thời gian
            int year = Convert.ToInt32(_Year);

            // Tạo danh sách chứa các đối tượng KetQuaDanhGia_Circle_ để đưa ra giao diện theo kiểu Json
            IList<KetQuaDanhGia_Column_> listMD = new List<KetQuaDanhGia_Column_>();

            for (int i = 1; i < 13; i++)
            {
                KetQuaDanhGia_Column_ KetQuaDanhGiaMD = new KetQuaDanhGia_Column_()
                {
                    thang = "Tháng " + i
                };
                DateTime start = new DateTime(year, i, 1, 0, 0, 0);
                DateTime end = new DateTime(year, i, DateTime.DaysInMonth(year, i), 23, 59, 59);
                KetQuaDanhGiaMD.RHL = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.CANBO.MABP == _MaBP &&
                                                          p.MUCDO == 1 &&
                                                          p.TG >= start &&
                                                          p.TG <= end)
                                              .Count();
                KetQuaDanhGiaMD.HL = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.CANBO.MABP == _MaBP &&
                                                          p.MUCDO == 2 &&
                                                          p.TG >= start &&
                                                          p.TG <= end)
                                              .Count();
                KetQuaDanhGiaMD.BT = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.CANBO.MABP == _MaBP &&
                                                          p.MUCDO == 3 &&
                                                          p.TG >= start &&
                                                          p.TG <= end)
                                              .Count();
                KetQuaDanhGiaMD.KHL = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.CANBO.MABP == _MaBP &&
                                                          p.MUCDO == 4 &&
                                                          p.TG >= start &&
                                                          p.TG <= end)
                                              .Count();
                listMD.Add(KetQuaDanhGiaMD);
            }
            return listMD;
        }

        /// <summary>
        /// Lấy kết quả đánh giá của cán bộ theo từng tháng trong năm
        /// </summary>
        /// <param name="_MaCB">Mã cán bộ</param>
        /// <param name="_Year">Năm</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<KetQuaDanhGia_Column_> GetKetQuaDanhGiaCBAll(int _MaCB, string _Year)
        {
            // Tạo khoảng thời gian
            int year = Convert.ToInt32(_Year);

            // Tạo danh sách chứa các đối tượng KetQuaDanhGia_Circle_ để đưa ra giao diện theo kiểu Json
            IList<KetQuaDanhGia_Column_> listMD = new List<KetQuaDanhGia_Column_>();

            for (int i = 1; i < 13; i++)
            {
                KetQuaDanhGia_Column_ KetQuaDanhGiaMD = new KetQuaDanhGia_Column_()
                {
                    thang = "Tháng " + i
                };
                DateTime start = new DateTime(year, i, 1, 0, 0, 0);
                DateTime end = new DateTime(year, i, DateTime.DaysInMonth(year, i), 23, 59, 59);
                KetQuaDanhGiaMD.RHL = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == _MaCB &&
                                                          p.MUCDO == 1 &&
                                                          p.TG >= start &&
                                                          p.TG <= end)
                                              .Count();
                KetQuaDanhGiaMD.HL = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == _MaCB &&
                                                          p.MUCDO == 2 &&
                                                          p.TG >= start &&
                                                          p.TG <= end)
                                              .Count();
                KetQuaDanhGiaMD.BT = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == _MaCB &&
                                                          p.MUCDO == 3 &&
                                                          p.TG >= start &&
                                                          p.TG <= end)
                                              .Count();
                KetQuaDanhGiaMD.KHL = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == _MaCB &&
                                                          p.MUCDO == 4 &&
                                                          p.TG >= start &&
                                                          p.TG <= end)
                                              .Count();
                listMD.Add(KetQuaDanhGiaMD);
            }
            return listMD;
        }
    }
}
