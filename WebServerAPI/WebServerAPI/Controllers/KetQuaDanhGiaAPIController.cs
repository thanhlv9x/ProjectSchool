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
    public class KetQuaDanhGiaAPIController : ApiController
    {
        HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities();

        /// <summary>
        /// Lấy kết quả đánh giá tổng hợp
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KetQuaDanhGia_Circle_> GetKetQuaDanhGiaAll()
        {
            // Tạo danh sách chứa các đối tượng KetQuaDanhGia_Circle_ để đưa ra giao diện theo kiểu Json
            IList<KetQuaDanhGia_Circle_> listMD = new List<KetQuaDanhGia_Circle_>();

            // Lấy danh sách các mức độ đánh giá
            var listEF = db.MUCDODANHGIAs.ToList();

            // Mảng màu sắc
            string[] arr = { "#37b24d", "#228be6", "#ffd43b", "#fa5252", "#e8590c", "#cccccc" };
            int i = 0;
            foreach (var item in listEF)
            {
                // Đếm số lượng các mức độ của bộ phận
                int ma_muc_do = Convert.ToInt32(item.MUCDO);
                int so_luong = db.KETQUADANHGIAs.Where(p => p.MUCDO == ma_muc_do)
                                                .Count();
                KetQuaDanhGia_Circle_ KetQuaDanhGiaMD = new KetQuaDanhGia_Circle_()
                {
                    category = item.LOAI,
                    value = so_luong,
                    color = arr[i],
                };
                listMD.Add(KetQuaDanhGiaMD);
                i++;
            }
            return listMD;
        }
        /// <summary>
        /// Lấy kết quả đánh giá của bộ phận theo thời gian
        /// </summary>
        /// <param name="_ThoiGian">Thời gian</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<KetQuaDanhGia_Circle_> GetKetQuaDanhGiaAllThoiGian(string _ThoiGian)
        {
            // Chuyển đổi thời gian thành 2 phần bắt đầu (start) và kết thúc (end)
            string[] thoi_gian = _ThoiGian.Split(' ');
            int thang = Convert.ToInt32(thoi_gian[0]);
            int nam = Convert.ToInt32(thoi_gian[1]);
            DateTime start = new DateTime(nam, thang, 1, 0, 0, 0);
            DateTime end = new DateTime(nam, thang, DateTime.DaysInMonth(nam, thang), 23, 59, 59);

            // Tạo danh sách chứa các đối tượng KetQuaDanhGia_Circle_ để đưa ra giao diện theo kiểu Json
            IList<KetQuaDanhGia_Circle_> listMD = new List<KetQuaDanhGia_Circle_>();

            // Lấy danh sách các mức độ đánh giá
            var listEF = db.MUCDODANHGIAs.ToList();

            // Mảng màu sắc
            string[] arr = { "#37b24d", "#228be6", "#ffd43b", "#fa5252", "#e8590c", "#cccccc" };
            int i = 0;

            foreach (var item in listEF)
            {
                // Đếm số lượng các mức độ của bộ phận theo thời gian
                int ma_muc_do = Convert.ToInt32(item.MUCDO);
                int so_luong = db.KETQUADANHGIAs.Where(p => p.MUCDO == ma_muc_do &&
                                                            p.TG >= start &&
                                                            p.TG <= end)
                                                .Count();
                KetQuaDanhGia_Circle_ KetQuaDanhGiaMD = new KetQuaDanhGia_Circle_()
                {
                    category = item.LOAI,
                    value = so_luong,
                    color = arr[i],
                };
                listMD.Add(KetQuaDanhGiaMD);
                i++;
            }
            return listMD;
        }
        /// <summary>
        /// Lấy toàn kết quả đánh giá theo bộ phận
        /// </summary>
        /// <param name="_MaBP">Mã bộ phận</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<KetQuaDanhGia_Circle_> GetKetQuaDanhGiaBPAll(int _MaBP)
        {
            // Tạo danh sách chứa các đối tượng KetQuaDanhGia_Circle_ để đưa ra giao diện theo kiểu Json
            IList<KetQuaDanhGia_Circle_> listMD = new List<KetQuaDanhGia_Circle_>();

            // Lấy danh sách các mức độ đánh giá
            var listEF = db.MUCDODANHGIAs.ToList();

            // Mảng màu sắc
            string[] arr = { "#37b24d", "#228be6", "#ffd43b", "#fa5252", "#e8590c", "#cccccc" };
            int i = 0;
            foreach (var item in listEF)
            {
                // Đếm số lượng các mức độ của bộ phận
                int ma_muc_do = Convert.ToInt32(item.MUCDO);
                int so_luong = db.KETQUADANHGIAs.Where(p => p.MUCDO == ma_muc_do &&
                                                            p.SOTHUTU.CANBO.MABP == _MaBP)
                                                .Count();
                KetQuaDanhGia_Circle_ KetQuaDanhGiaMD = new KetQuaDanhGia_Circle_()
                {
                    category = item.LOAI,
                    value = so_luong,
                    color = arr[i],
                };
                listMD.Add(KetQuaDanhGiaMD);
                i++;
            }
            return listMD;
        }
        /// <summary>
        /// Lấy kết quả đánh giá của bộ phận theo thời gian
        /// </summary>
        /// <param name="_MaBP">Mã bộ phận</param>
        /// <param name="_ThoiGian">Thời gian</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<KetQuaDanhGia_Circle_> GetKetQuaDanhGiaBPAllThoiGian(int _MaBP, string _ThoiGian)
        {
            // Chuyển đổi thời gian thành 2 phần bắt đầu (start) và kết thúc (end)
            string[] thoi_gian = _ThoiGian.Split(' ');
            int thang = Convert.ToInt32(thoi_gian[0]);
            int nam = Convert.ToInt32(thoi_gian[1]);
            DateTime start = new DateTime(nam, thang, 1, 0, 0, 0);
            DateTime end = new DateTime(nam, thang, DateTime.DaysInMonth(nam, thang), 23, 59, 59);

            // Tạo danh sách chứa các đối tượng KetQuaDanhGia_Circle_ để đưa ra giao diện theo kiểu Json
            IList<KetQuaDanhGia_Circle_> listMD = new List<KetQuaDanhGia_Circle_>();

            // Lấy danh sách các mức độ đánh giá
            var listEF = db.MUCDODANHGIAs.ToList();

            // Mảng màu sắc
            string[] arr = { "#37b24d", "#228be6", "#ffd43b", "#fa5252", "#e8590c", "#cccccc" };
            int i = 0;

            foreach (var item in listEF)
            {
                // Đếm số lượng các mức độ của bộ phận theo thời gian
                int ma_muc_do = Convert.ToInt32(item.MUCDO);
                int so_luong = db.KETQUADANHGIAs.Where(p => p.MUCDO == ma_muc_do &&
                                                            p.SOTHUTU.CANBO.MABP == _MaBP &&
                                                            p.TG >= start &&
                                                            p.TG <= end)
                                                .Count();
                KetQuaDanhGia_Circle_ KetQuaDanhGiaMD = new KetQuaDanhGia_Circle_()
                {
                    category = item.LOAI,
                    value = so_luong,
                    color = arr[i],
                };
                listMD.Add(KetQuaDanhGiaMD);
                i++;
            }
            return listMD;
        }
        /// <summary>
        /// Lấy toàn kết quả đánh giá theo cán bộ
        /// </summary>
        /// <param name="_MaCB">Mã cán bộ</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<KetQuaDanhGia_Circle_> GetKetQuaDanhGiaCBAll(int _MaCB)
        {
            // Tạo danh sách chứa các đối tượng KetQuaDanhGia_Circle_ để đưa ra giao diện theo kiểu Json
            IList<KetQuaDanhGia_Circle_> listMD = new List<KetQuaDanhGia_Circle_>();

            // Lấy danh sách các mức độ đánh giá
            var listEF = db.MUCDODANHGIAs.ToList();

            // Mảng màu sắc
            string[] arr = { "#37b24d", "#228be6", "#ffd43b", "#fa5252", "#e8590c", "#cccccc" };
            int i = 0;
            foreach (var item in listEF)
            {
                // Đếm số lượng các mức độ của bộ phận
                int ma_muc_do = Convert.ToInt32(item.MUCDO);
                int so_luong = db.KETQUADANHGIAs.Where(p => p.MUCDO == ma_muc_do &&
                                                            p.SOTHUTU.MACB == _MaCB)
                                                .Count();
                KetQuaDanhGia_Circle_ KetQuaDanhGiaMD = new KetQuaDanhGia_Circle_()
                {
                    category = item.LOAI,
                    value = so_luong,
                    color = arr[i],
                };
                listMD.Add(KetQuaDanhGiaMD);
                i++;
            }
            return listMD;
        }
        /// <summary>
        /// Lấy kết quả đánh giá của cán bộ theo thời gian
        /// </summary>
        /// <param name="_MaCB">Mã cán bộ</param>
        /// <param name="_ThoiGian">Thời gian</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<KetQuaDanhGia_Circle_> GetKetQuaDanhGiaCBAllThoiGian(int _MaCB, string _ThoiGian)
        {
            // Chuyển đổi thời gian thành 2 phần bắt đầu (start) và kết thúc (end)
            string[] thoi_gian = _ThoiGian.Split(' ');
            int thang = Convert.ToInt32(thoi_gian[0]);
            int nam = Convert.ToInt32(thoi_gian[1]);
            DateTime start = new DateTime(nam, thang, 1, 0, 0, 0);
            DateTime end = new DateTime(nam, thang, DateTime.DaysInMonth(nam, thang), 23, 59, 59);

            // Tạo danh sách chứa các đối tượng KetQuaDanhGia_Circle_ để đưa ra giao diện theo kiểu Json
            IList<KetQuaDanhGia_Circle_> listMD = new List<KetQuaDanhGia_Circle_>();

            // Lấy danh sách các mức độ đánh giá
            var listEF = db.MUCDODANHGIAs.ToList();

            // Mảng màu sắc
            string[] arr = { "#37b24d", "#228be6", "#ffd43b", "#fa5252", "#e8590c", "#cccccc" };
            int i = 0;

            foreach (var item in listEF)
            {
                // Đếm số lượng các mức độ của bộ phận theo thời gian
                int ma_muc_do = Convert.ToInt32(item.MUCDO);
                int so_luong = db.KETQUADANHGIAs.Where(p => p.MUCDO == ma_muc_do &&
                                                            p.SOTHUTU.MACB == _MaCB &&
                                                            p.TG >= start &&
                                                            p.TG <= end)
                                                .Count();
                KetQuaDanhGia_Circle_ KetQuaDanhGiaMD = new KetQuaDanhGia_Circle_()
                {
                    category = item.LOAI,
                    value = so_luong,
                    color = arr[i],
                };
                listMD.Add(KetQuaDanhGiaMD);
                i++;
            }
            return listMD;
        }
    }
}
