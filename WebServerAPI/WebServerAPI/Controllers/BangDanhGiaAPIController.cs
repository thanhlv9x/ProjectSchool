﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebServerAPI.EF;
using WebServerAPI.Models;

namespace WebServerAPI.Controllers
{
    public class BangDanhGiaAPIController : ApiController
    {
        HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities();

        /// <summary>
        /// Lấy kết quả đánh giá tổng hợp
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<KetQuaDanhGia_Table_> GetBangDanhGiaAll()
        {
            // Tạo danh sách chứa các đối tượng KetQuaDanhGia_Circle_ để đưa ra giao diện theo kiểu Json
            IList<KetQuaDanhGia_Table_> listMD = new List<KetQuaDanhGia_Table_>();

            // Lấy danh sách các mức độ đánh giá
            var listEF = db.MUCDODANHGIAs.ToList();

            // Lấy tổng số lượng kết quả đánh giá theo bộ phận
            int tong = db.KETQUADANHGIAs.Count(p => p.MUCDO != null);
            foreach (var item in listEF)
            {
                // Đếm số lượng các mức độ của bộ phận
                int ma_muc_do = Convert.ToInt32(item.MUCDO);
                int so_luong = db.KETQUADANHGIAs.Where(p => p.MUCDO == ma_muc_do)
                                                .Count();
                double ty_le = Math.Round(((double)so_luong / (double)tong) * 100.00, 2);
                int diem = LayDiem(ma_muc_do, so_luong);
                //switch (ma_muc_do)
                //{
                //    case 1: diem = 4; break;
                //    case 2: diem = 3; break;
                //    case 3: diem = 2; break;
                //    case 4: diem = 1; break;
                //}
                //diem *= so_luong;
                KetQuaDanhGia_Table_ BangDanhGiaMD = new KetQuaDanhGia_Table_()
                {
                    MucDo = ma_muc_do,
                    Loai = item.LOAI,
                    SoLan = so_luong,
                    TyLe = ty_le,
                    Diem = diem
                };
                listMD.Add(BangDanhGiaMD);
            }
            return listMD;
        }
        /// <summary>
        /// Lấy kết quả đánh giá tổng hợp theo thời gian
        /// </summary>
        /// <param name="_ThoiGian">Thời gian</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<KetQuaDanhGia_Table_> GetBangDanhGiaAllThoiGian(string _ThoiGian)
        {
            // Chuyển đổi thời gian thành 2 phần bắt đầu (start) và kết thúc (end)
            string[] thoi_gian = _ThoiGian.Split(' ');
            int thang = Convert.ToInt32(thoi_gian[0]);
            int nam = Convert.ToInt32(thoi_gian[1]);
            DateTime start = new DateTime(nam, thang, 1, 0, 0, 0);
            DateTime end = new DateTime(nam, thang, DateTime.DaysInMonth(nam, thang), 23, 59, 59);

            // Tạo danh sách chứa các đối tượng KetQuaDanhGia_Circle_ để đưa ra giao diện theo kiểu Json
            IList<KetQuaDanhGia_Table_> listMD = new List<KetQuaDanhGia_Table_>();

            // Lấy danh sách các mức độ đánh giá
            var listEF = db.MUCDODANHGIAs.ToList();

            // Lấy tổng số lượng kết quả đánh giá theo bộ phận
            int tong = db.KETQUADANHGIAs.Where(p => p.TG >= start &&
                                                    p.TG <= end)
                                        .Count();
            foreach (var item in listEF)
            {
                // Đếm số lượng các mức độ của bộ phận
                int ma_muc_do = Convert.ToInt32(item.MUCDO);
                int so_luong = db.KETQUADANHGIAs.Where(p => p.MUCDO == ma_muc_do &&
                                                            p.TG >= start &&
                                                            p.TG <= end)
                                                .Count();
                double ty_le = Math.Round(((double)so_luong / (double)tong) * 100.00, 2);
                int diem = LayDiem(ma_muc_do, so_luong);
                //switch (ma_muc_do)
                //{
                //    case 1: diem = 4; break;
                //    case 2: diem = 3; break;
                //    case 3: diem = 2; break;
                //    case 4: diem = 1; break;
                //}
                //diem *= so_luong;
                KetQuaDanhGia_Table_ BangDanhGiaMD = new KetQuaDanhGia_Table_()
                {
                    MucDo = ma_muc_do,
                    Loai = item.LOAI,
                    SoLan = so_luong,
                    TyLe = ty_le,
                    Diem = diem
                };
                listMD.Add(BangDanhGiaMD);
            }
            return listMD;
        }
        /// <summary>
        /// Lấy toàn bộ kết quả đánh giá theo bộ phận
        /// </summary>
        /// <param name="_MaBP">Mã bộ phận</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<KetQuaDanhGia_Table_> GetBangDanhGiaBPAll(int _MaBP)
        {
            // Tạo danh sách chứa các đối tượng KetQuaDanhGia_Circle_ để đưa ra giao diện theo kiểu Json
            IList<KetQuaDanhGia_Table_> listMD = new List<KetQuaDanhGia_Table_>();

            // Lấy danh sách các mức độ đánh giá
            var listEF = db.MUCDODANHGIAs.ToList();

            // Lấy tổng số lượng kết quả đánh giá theo bộ phận
            int tong = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.CANBO.MABP == _MaBP)
                                        .Count();
            foreach (var item in listEF)
            {
                // Đếm số lượng các mức độ của bộ phận
                int ma_muc_do = Convert.ToInt32(item.MUCDO);
                int so_luong = db.KETQUADANHGIAs.Where(p => p.MUCDO == ma_muc_do &&
                                                            p.SOTHUTU.CANBO.MABP == _MaBP)
                                                .Count();
                double ty_le = Math.Round(((double)so_luong / (double)tong) * 100.00, 2);
                int diem = LayDiem(ma_muc_do, so_luong);
                //switch (ma_muc_do)
                //{
                //    case 1: diem = 4; break;
                //    case 2: diem = 3; break;
                //    case 3: diem = 2; break;
                //    case 4: diem = 1; break;
                //}
                //diem *= so_luong;
                KetQuaDanhGia_Table_ BangDanhGiaMD = new KetQuaDanhGia_Table_()
                {
                    MucDo = ma_muc_do,
                    Loai = item.LOAI,
                    SoLan = so_luong,
                    TyLe = ty_le,
                    Diem = diem
                };
                listMD.Add(BangDanhGiaMD);
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
        public IEnumerable<KetQuaDanhGia_Table_> GetBangDanhGiaBPAllThoiGian(int _MaBP, string _ThoiGian)
        {
            // Chuyển đổi thời gian thành 2 phần bắt đầu (start) và kết thúc (end)
            string[] thoi_gian = _ThoiGian.Split(' ');
            int thang = Convert.ToInt32(thoi_gian[0]);
            int nam = Convert.ToInt32(thoi_gian[1]);
            DateTime start = new DateTime(nam, thang, 1, 0, 0, 0);
            DateTime end = new DateTime(nam, thang, DateTime.DaysInMonth(nam, thang), 23, 59, 59);

            // Tạo danh sách chứa các đối tượng KetQuaDanhGia_Circle_ để đưa ra giao diện theo kiểu Json
            IList<KetQuaDanhGia_Table_> listMD = new List<KetQuaDanhGia_Table_>();

            // Lấy danh sách các mức độ đánh giá
            var listEF = db.MUCDODANHGIAs.ToList();

            // Lấy tổng số lượng kết quả đánh giá theo bộ phận
            int tong = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.CANBO.MABP == _MaBP &&
                                                    p.TG >= start &&
                                                    p.TG <= end)
                                        .Count();
            foreach (var item in listEF)
            {
                // Đếm số lượng các mức độ của bộ phận
                int ma_muc_do = Convert.ToInt32(item.MUCDO);
                int so_luong = db.KETQUADANHGIAs.Where(p => p.MUCDO == ma_muc_do &&
                                                            p.SOTHUTU.CANBO.MABP == _MaBP &&
                                                            p.TG >= start &&
                                                            p.TG <= end)
                                                .Count();
                double ty_le = Math.Round(((double)so_luong / (double)tong) * 100.00, 2);
                int diem = LayDiem(ma_muc_do, so_luong);
                //switch (ma_muc_do)
                //{
                //    case 1: diem = 4; break;
                //    case 2: diem = 3; break;
                //    case 3: diem = 2; break;
                //    case 4: diem = 1; break;
                //}
                //diem *= so_luong;
                KetQuaDanhGia_Table_ BangDanhGiaMD = new KetQuaDanhGia_Table_()
                {
                    MucDo = ma_muc_do,
                    Loai = item.LOAI,
                    SoLan = so_luong,
                    TyLe = ty_le,
                    Diem = diem
                };
                listMD.Add(BangDanhGiaMD);
            }
            return listMD;
        }
        /// <summary>
        /// Lấy kết quả đánh giá của bộ phận theo mức độ đánh giá
        /// </summary>
        /// <param name="_MucDo">Mức độ đánh giá</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<KetQuaDanhGia_ChiTiet_> GetBangDanhGiaChiTietAll(int _MucDo)
        {
            // Tạo danh sách chứa các đối tượng KetQuaDanhGia_Circle_ để đưa ra giao diện theo kiểu Json
            IList<KetQuaDanhGia_ChiTiet_> listMD = new List<KetQuaDanhGia_ChiTiet_>();

            // Lấy danh sách cán bộ của bộ phận
            var listEF = db.BOPHANs.ToList();

            // Lấy tổng kết quả đánh giá của bộ phận theo mức độ đánh giá
            int tong = db.KETQUADANHGIAs.Where(p => p.MUCDO == _MucDo)
                                        .Count();

            foreach (var item in listEF)
            {
                // Lấy số lượng kết quả đánh giá của từng cán bộ theo mức độ đánh giá
                int so_luong = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.CANBO.MABP == item.MABP &&
                                                            p.MUCDO == _MucDo)
                                                .Count();

                double ty_le = Math.Round(((double)so_luong / (double)tong) * 100.00, 2);
                int diem = LayDiem(_MucDo, so_luong);
                //switch (ma_muc_do)
                //{
                //    case 1: diem = 4; break;
                //    case 2: diem = 3; break;
                //    case 3: diem = 2; break;
                //    case 4: diem = 1; break;
                //}
                //diem *= so_luong;
                KetQuaDanhGia_ChiTiet_ BangDanhGiaMD = new KetQuaDanhGia_ChiTiet_()
                {
                    MaCB = Convert.ToInt32(item.MABP),
                    HoTen = item.TENBP,
                    SoLan = so_luong,
                    TyLe = ty_le,
                    Diem = diem
                };
                listMD.Add(BangDanhGiaMD);
            }

            return listMD;
        }
        /// <summary>
        /// Lấy kết quả đánh giá của bộ phận theo mức độ đánh giá và thời gian
        /// </summary>
        /// <param name="_MucDo">Mức độ đánh giá</param>
        /// <param name="_ThoiGian">Thời gian</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<KetQuaDanhGia_ChiTiet_> GetBangDanhGiaChiTietAllThoiGian(int _MucDo, string _ThoiGian)
        {
            // Chuyển đổi thời gian thành 2 phần bắt đầu (start) và kết thúc (end)
            string[] thoi_gian = _ThoiGian.Split(' ');
            int thang = Convert.ToInt32(thoi_gian[0]);
            int nam = Convert.ToInt32(thoi_gian[1]);
            DateTime start = new DateTime(nam, thang, 1, 0, 0, 0);
            DateTime end = new DateTime(nam, thang, DateTime.DaysInMonth(nam, thang), 23, 59, 59);

            // Tạo danh sách chứa các đối tượng KetQuaDanhGia_Circle_ để đưa ra giao diện theo kiểu Json
            IList<KetQuaDanhGia_ChiTiet_> listMD = new List<KetQuaDanhGia_ChiTiet_>();

            // Lấy danh sách cán bộ của bộ phận
            var listEF = db.BOPHANs.ToList();

            // Lấy tổng kết quả đánh giá của bộ phận theo mức độ đánh giá
            int tong = db.KETQUADANHGIAs.Where(p => p.MUCDO == _MucDo &&
                                                    p.TG >= start &&
                                                    p.TG <= end)
                                        .Count();

            foreach (var item in listEF)
            {
                // Lấy số lượng kết quả đánh giá của từng cán bộ theo mức độ đánh giá
                int so_luong = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.CANBO.MABP == item.MABP &&
                                                            p.MUCDO == _MucDo &&
                                                            p.TG >= start &&
                                                            p.TG <= end)
                                                .Count();

                double ty_le = Math.Round(((double)so_luong / (double)tong) * 100.00, 2);
                int diem = LayDiem(_MucDo, so_luong);
                //switch (ma_muc_do)
                //{
                //    case 1: diem = 4; break;
                //    case 2: diem = 3; break;
                //    case 3: diem = 2; break;
                //    case 4: diem = 1; break;
                //}
                //diem *= so_luong;
                KetQuaDanhGia_ChiTiet_ BangDanhGiaMD = new KetQuaDanhGia_ChiTiet_()
                {
                    MaCB = Convert.ToInt32(item.MABP),
                    HoTen = item.TENBP,
                    SoLan = so_luong,
                    TyLe = ty_le,
                    Diem = diem
                };
                listMD.Add(BangDanhGiaMD);
            }

            return listMD;
        }
        /// <summary>
        /// Lấy kết quả đánh giá của bộ phận theo mức độ đánh giá
        /// </summary>
        /// <param name="_MaBP">Mã bộ phận</param>
        /// <param name="_MucDo">Mức độ đánh giá</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<KetQuaDanhGia_ChiTiet_> GetBangDanhGiaChiTiet(int _MaBP, int _MucDo)
        {
            // Tạo danh sách chứa các đối tượng KetQuaDanhGia_Circle_ để đưa ra giao diện theo kiểu Json
            IList<KetQuaDanhGia_ChiTiet_> listMD = new List<KetQuaDanhGia_ChiTiet_>();

            // Lấy danh sách cán bộ của bộ phận
            var listEF = db.CANBOes.Where(p => p.MABP == _MaBP);

            // Lấy tổng kết quả đánh giá của bộ phận theo mức độ đánh giá
            int tong = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.CANBO.MABP == _MaBP &&
                                                    p.MUCDO == _MucDo)
                                        .Count();

            foreach (var item in listEF)
            {
                // Lấy số lượng kết quả đánh giá của từng cán bộ theo mức độ đánh giá
                int so_luong = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == item.MACB &&
                                                            p.MUCDO == _MucDo)
                                                .Count();

                double ty_le = Math.Round(((double)so_luong / (double)tong) * 100.00, 2);
                int diem = LayDiem(_MucDo, so_luong);
                //switch (ma_muc_do)
                //{
                //    case 1: diem = 4; break;
                //    case 2: diem = 3; break;
                //    case 3: diem = 2; break;
                //    case 4: diem = 1; break;
                //}
                //diem *= so_luong;
                KetQuaDanhGia_ChiTiet_ BangDanhGiaMD = new KetQuaDanhGia_ChiTiet_()
                {
                    MaCB = Convert.ToInt32(item.MACB),
                    HoTen = item.HOTEN,
                    SoLan = so_luong,
                    TyLe = ty_le,
                    Diem = diem
                };
                listMD.Add(BangDanhGiaMD);
            }

            return listMD;
        }
        /// <summary>
        /// Lấy kết quả đánh giá của bộ phận theo mức độ đánh giá và thời gian
        /// </summary>
        /// <param name="_MaBP">Mã bộ phận</param>
        /// <param name="_MucDo">Mức độ đánh giá</param>
        /// <param name="_ThoiGian">Thời gian</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<KetQuaDanhGia_ChiTiet_> GetBangDanhGiaChiTietThoiGian(int _MaBP, int _MucDo, string _ThoiGian)
        {
            // Chuyển đổi thời gian thành 2 phần bắt đầu (start) và kết thúc (end)
            string[] thoi_gian = _ThoiGian.Split(' ');
            int thang = Convert.ToInt32(thoi_gian[0]);
            int nam = Convert.ToInt32(thoi_gian[1]);
            DateTime start = new DateTime(nam, thang, 1, 0, 0, 0);
            DateTime end = new DateTime(nam, thang, DateTime.DaysInMonth(nam, thang), 23, 59, 59);

            // Tạo danh sách chứa các đối tượng KetQuaDanhGia_Circle_ để đưa ra giao diện theo kiểu Json
            IList<KetQuaDanhGia_ChiTiet_> listMD = new List<KetQuaDanhGia_ChiTiet_>();

            // Lấy danh sách cán bộ của bộ phận
            var listEF = db.CANBOes.Where(p => p.MABP == _MaBP);

            // Lấy tổng kết quả đánh giá của bộ phận theo mức độ đánh giá
            int tong = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.CANBO.MABP == _MaBP &&
                                                    p.MUCDO == _MucDo &&
                                                    p.TG >= start &&
                                                    p.TG <= end)
                                        .Count();

            foreach (var item in listEF)
            {
                // Lấy số lượng kết quả đánh giá của từng cán bộ theo mức độ đánh giá
                int so_luong = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == item.MACB &&
                                                            p.MUCDO == _MucDo &&
                                                            p.TG >= start &&
                                                            p.TG <= end)
                                                .Count();

                double ty_le = Math.Round(((double)so_luong / (double)tong) * 100.00, 2);
                int diem = LayDiem(_MucDo, so_luong);
                //switch (ma_muc_do)
                //{
                //    case 1: diem = 4; break;
                //    case 2: diem = 3; break;
                //    case 3: diem = 2; break;
                //    case 4: diem = 1; break;
                //}
                //diem *= so_luong;
                KetQuaDanhGia_ChiTiet_ BangDanhGiaMD = new KetQuaDanhGia_ChiTiet_()
                {
                    MaCB = Convert.ToInt32(item.MACB),
                    HoTen = item.HOTEN,
                    SoLan = so_luong,
                    TyLe = ty_le,
                    Diem = diem
                };
                listMD.Add(BangDanhGiaMD);
            }

            return listMD;
        }
        /// <summary>
        /// Lấy toàn bộ kết quả đánh giá theo cán bộ
        /// </summary>
        /// <param name="_MaCB">Mã cán bộ</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<KetQuaDanhGia_Table_> GetBangDanhGiaCBAll(int _MaCB)
        {
            // Tạo danh sách chứa các đối tượng KetQuaDanhGia_Circle_ để đưa ra giao diện theo kiểu Json
            IList<KetQuaDanhGia_Table_> listMD = new List<KetQuaDanhGia_Table_>();

            // Lấy danh sách các mức độ đánh giá
            var listEF = db.MUCDODANHGIAs.ToList();

            // Lấy tổng số lượng kết quả đánh giá theo bộ phận
            int tong = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == _MaCB)
                                        .Count();
            foreach (var item in listEF)
            {
                // Đếm số lượng các mức độ của bộ phận
                int ma_muc_do = Convert.ToInt32(item.MUCDO);
                int so_luong = db.KETQUADANHGIAs.Where(p => p.MUCDO == ma_muc_do &&
                                                            p.SOTHUTU.MACB == _MaCB)
                                                .Count();
                double ty_le = Math.Round(((double)so_luong / (double)tong) * 100.00, 2);
                int diem = LayDiem(ma_muc_do, so_luong);
                //switch (ma_muc_do)
                //{
                //    case 1: diem = 4; break;
                //    case 2: diem = 3; break;
                //    case 3: diem = 2; break;
                //    case 4: diem = 1; break;
                //}
                //diem *= so_luong;
                KetQuaDanhGia_Table_ BangDanhGiaMD = new KetQuaDanhGia_Table_()
                {
                    MucDo = ma_muc_do,
                    Loai = item.LOAI,
                    SoLan = so_luong,
                    TyLe = ty_le,
                    Diem = diem
                };
                listMD.Add(BangDanhGiaMD);
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
        public IEnumerable<KetQuaDanhGia_Table_> GetBangDanhGiaCBAllThoiGian(int _MaCB, string _ThoiGian)
        {
            // Chuyển đổi thời gian thành 2 phần bắt đầu (start) và kết thúc (end)
            string[] thoi_gian = _ThoiGian.Split(' ');
            int thang = Convert.ToInt32(thoi_gian[0]);
            int nam = Convert.ToInt32(thoi_gian[1]);
            DateTime start = new DateTime(nam, thang, 1, 0, 0, 0);
            DateTime end = new DateTime(nam, thang, DateTime.DaysInMonth(nam, thang), 23, 59, 59);

            // Tạo danh sách chứa các đối tượng KetQuaDanhGia_Circle_ để đưa ra giao diện theo kiểu Json
            IList<KetQuaDanhGia_Table_> listMD = new List<KetQuaDanhGia_Table_>();

            // Lấy danh sách các mức độ đánh giá
            var listEF = db.MUCDODANHGIAs.ToList();

            // Lấy tổng số lượng kết quả đánh giá theo bộ phận
            int tong = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == _MaCB &&
                                                    p.TG >= start &&
                                                    p.TG <= end)
                                        .Count();
            foreach (var item in listEF)
            {
                // Đếm số lượng các mức độ của bộ phận
                int ma_muc_do = Convert.ToInt32(item.MUCDO);
                int so_luong = db.KETQUADANHGIAs.Where(p => p.MUCDO == ma_muc_do &&
                                                            p.SOTHUTU.MACB == _MaCB &&
                                                            p.TG >= start &&
                                                            p.TG <= end)
                                                .Count();
                double ty_le = Math.Round(((double)so_luong / (double)tong) * 100.00, 2);
                int diem = LayDiem(ma_muc_do, so_luong);
                //switch (ma_muc_do)
                //{
                //    case 1: diem = 4; break;
                //    case 2: diem = 3; break;
                //    case 3: diem = 2; break;
                //    case 4: diem = 1; break;
                //}
                //diem *= so_luong;
                KetQuaDanhGia_Table_ BangDanhGiaMD = new KetQuaDanhGia_Table_()
                {
                    MucDo = ma_muc_do,
                    Loai = item.LOAI,
                    SoLan = so_luong,
                    TyLe = ty_le,
                    Diem = diem
                };
                listMD.Add(BangDanhGiaMD);
            }
            return listMD;
        }
        /// <summary>
        /// Lấy góp ý của từng cán bộ
        /// </summary>
        /// <param name="_MaCB">Mã cán bộ</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<BangGopY> GetBangGopYAll(int _MaCBGopY)
        {
            IList<BangGopY> listMD = new List<BangGopY>();
            var listEF = db.GOPies.Where(p => p.KETQUADANHGIA.SOTHUTU.MACB == _MaCBGopY)
                                  .ToList();
            foreach (var item in listEF)
            {
                bool doub = false;
                foreach (var itemMD in listMD)
                {
                    if (item.NOIDUNG == itemMD.GopY && item.KETQUADANHGIA.MUCDODANHGIA.LOAI == itemMD.MucDo) doub = true;
                }
                if (!doub)
                {
                    string noi_dung = item.NOIDUNG;
                    int muc_do = item.KETQUADANHGIA.MUCDODANHGIA.MUCDO;
                    DateTime dt = (DateTime)item.KETQUADANHGIA.TG;
                    DateTime dtNow = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
                    BangGopY bang_gop_y_MD = new BangGopY()
                    {
                        MaDG = Convert.ToInt32(item.MADG),
                        MucDo = item.KETQUADANHGIA.MUCDODANHGIA.LOAI,
                        GopY = noi_dung,
                        ThoiGian = (DateTime)item.KETQUADANHGIA.TG,
                        Ngay = dtNow
                    };
                    if (item.NOIDUNG != null)
                    {
                        noi_dung = item.NOIDUNG.ToLower();
                    }
                    int count = db.GOPies.Where(p => p.KETQUADANHGIA.SOTHUTU.MACB == _MaCBGopY &&
                                                     p.NOIDUNG.ToLower().Equals(noi_dung) &&
                                                     p.KETQUADANHGIA.MUCDO == muc_do)
                                         .Count();
                    bang_gop_y_MD.SoLan = count;
                    listMD.Add(bang_gop_y_MD);
                }
            }
            return listMD;
        }
        /// <summary>
        /// Lấy góp ý của từng cán bộ theo thời gian
        /// </summary>
        /// <param name="_MaCB">Mã cán bộ</param>
        /// <param name="_ThoiGian">Thời gian</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<BangGopY> GetBangGopYAllTime(int _MaCBGopY, string _ThoiGian)
        {
            // Chuyển đổi thời gian thành 2 phần bắt đầu (start) và kết thúc (end)
            string[] thoi_gian = _ThoiGian.Split(' ');
            int thang = Convert.ToInt32(thoi_gian[0]);
            int nam = Convert.ToInt32(thoi_gian[1]);
            DateTime start = new DateTime(nam, thang, 1, 0, 0, 0);
            DateTime end = new DateTime(nam, thang, DateTime.DaysInMonth(nam, thang), 23, 59, 59);

            IList<BangGopY> listMD = new List<BangGopY>();
            var listEF = db.GOPies.Where(p => p.KETQUADANHGIA.SOTHUTU.MACB == _MaCBGopY &&
                                              p.KETQUADANHGIA.TG >= start &&
                                              p.KETQUADANHGIA.TG <= end)
                                  .ToList();
            foreach (var item in listEF)
            {
                bool doub = false;
                foreach (var itemMD in listMD)
                {
                    if (item.NOIDUNG == itemMD.GopY && item.KETQUADANHGIA.MUCDODANHGIA.LOAI == itemMD.MucDo) doub = true;
                }
                if (!doub)
                {
                    string noi_dung = item.NOIDUNG;
                    int muc_do = item.KETQUADANHGIA.MUCDODANHGIA.MUCDO;
                    DateTime dt = (DateTime)item.KETQUADANHGIA.TG;
                    DateTime dtNow = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
                    BangGopY bang_gop_y_MD = new BangGopY()
                    {
                        MaDG = Convert.ToInt32(item.MADG),
                        MucDo = item.KETQUADANHGIA.MUCDODANHGIA.LOAI,
                        GopY = noi_dung,
                        ThoiGian = (DateTime)item.KETQUADANHGIA.TG,
                        Ngay = dtNow
                    };
                    if (noi_dung != null) noi_dung = item.NOIDUNG.ToLower();
                    int count = db.GOPies.Where(p => p.KETQUADANHGIA.SOTHUTU.MACB == _MaCBGopY &&
                                                     p.NOIDUNG.ToLower().Equals(noi_dung) &&
                                                     p.KETQUADANHGIA.MUCDO == muc_do)
                                         .Count();
                    bang_gop_y_MD.SoLan = count;
                    listMD.Add(bang_gop_y_MD);
                }
            }
            return listMD;
        }
        /// <summary>
        /// Lấy kết quả đánh giá của cán bộ tổng hợp cho Grid bảng kết quả đánh giá
        /// </summary>
        /// <param name="_MaCB">Mã cán bộ</param>
        /// <param name="_KetQua">Tham số xác định phương thức</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<KetQuaDanhGia_BaoCao_> GetKetQuaDanhGiaCB(int _MaCB, string _KetQua)
        {
            IList<KetQuaDanhGia_BaoCao_> listMD = new List<KetQuaDanhGia_BaoCao_>();
            var listKQ = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == _MaCB)
                                          .Select(p => p.TG)
                                          .ToList();
            DateTime dt = new DateTime();
            foreach (var itemKQ in listKQ)
            {
                if (dt.Year != ((DateTime)itemKQ).Year || dt.Month != ((DateTime)itemKQ).Month || dt.Day != ((DateTime)itemKQ).Day)
                {
                    dt = (DateTime)itemKQ;
                }
                else
                {
                    continue;
                }
                DateTime start = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
                DateTime end = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
                var countRHL = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == _MaCB &&
                                                            p.MUCDO == 1 &&
                                                            p.TG >= start &&
                                                            p.TG <= end)
                                               .Count();
                var countHL = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == _MaCB &&
                                                            p.MUCDO == 2 &&
                                                            p.TG >= start &&
                                                            p.TG <= end)
                                               .Count();
                var countBT = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == _MaCB &&
                                                            p.MUCDO == 3 &&
                                                            p.TG >= start &&
                                                            p.TG <= end)
                                               .Count();
                var countKHL = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == _MaCB &&
                                                            p.MUCDO == 4 &&
                                                            p.TG >= start &&
                                                            p.TG <= end)
                                               .Count();
                var total = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == _MaCB &&
                                                         p.TG >= start &&
                                                         p.TG <= end)
                                             .Count();
                double tyle_RHL = 0;
                double tyle_HL = 0;
                double tyle_BT = 0;
                double tyle_KHL = 0;
                if (total > 0)
                {
                    tyle_RHL = Math.Round(((double)countRHL / (double)total) * 100.00, 2);
                    tyle_HL = Math.Round(((double)countHL / (double)total) * 100.00, 2);
                    tyle_BT = Math.Round(((double)countBT / (double)total) * 100.00, 2);
                    tyle_KHL = Math.Round(((double)countKHL / (double)total) * 100.00, 2);
                }
                KetQuaDanhGia_BaoCao_ md = new KetQuaDanhGia_BaoCao_()
                {
                    Ngay = start,
                    RHL_SoLan = countRHL,
                    HL_SoLan = countHL,
                    BT_SoLan = countBT,
                    KHL_SoLan = countKHL,
                    RHL_TyLe = tyle_RHL,
                    HL_TyLe = tyle_HL,
                    BT_TyLe = tyle_BT,
                    KHL_TyLe = tyle_KHL
                };
                listMD.Add(md);
            }
            return listMD;
        }
        /// <summary>
        /// Lấy kết quả đánh giá của cán bộ theo thời gian cho Grid bảng kết quả đánh giá
        /// </summary>
        /// <param name="_MaCB">Mã cán bộ</param>
        /// <param name="_ThoiGian">Thời gian</param>
        /// <param name="_KetQua">Tham số xác định phương thức</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<KetQuaDanhGia_BaoCao_> GetKetQuaDanhGiaCBTime(int _MaCB, string _ThoiGian, string _KetQua)
        {
            // Chuyển đổi thời gian thành 2 phần bắt đầu (start) và kết thúc (end)
            string[] thoi_gian = _ThoiGian.Split(' ');
            int thang = Convert.ToInt32(thoi_gian[0]);
            int nam = Convert.ToInt32(thoi_gian[1]);
            DateTime start = new DateTime(nam, thang, 1, 0, 0, 0);
            DateTime end = new DateTime(nam, thang, DateTime.DaysInMonth(nam, thang), 23, 59, 59);

            IList<KetQuaDanhGia_BaoCao_> listMD = new List<KetQuaDanhGia_BaoCao_>();
            var listKQ = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == _MaCB &&
                                                      p.TG >= start &&
                                                      p.TG <= end)
                                          .Select(p => p.TG)
                                          .ToList();
            DateTime dt = new DateTime();
            foreach (var itemKQ in listKQ)
            {
                if (dt.Year != ((DateTime)itemKQ).Year || dt.Month != ((DateTime)itemKQ).Month || dt.Day != ((DateTime)itemKQ).Day)
                {
                    dt = (DateTime)itemKQ;
                }
                else
                {
                    continue;
                }
                DateTime dtStart = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
                DateTime dtEnd = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
                var countRHL = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == _MaCB &&
                                                            p.MUCDO == 1 &&
                                                            p.TG >= dtStart &&
                                                            p.TG <= dtEnd)
                                               .Count();
                var countHL = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == _MaCB &&
                                                            p.MUCDO == 2 &&
                                                            p.TG >= dtStart &&
                                                            p.TG <= dtEnd)
                                               .Count();
                var countBT = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == _MaCB &&
                                                            p.MUCDO == 3 &&
                                                            p.TG >= dtStart &&
                                                            p.TG <= dtEnd)
                                               .Count();
                var countKHL = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == _MaCB &&
                                                            p.MUCDO == 4 &&
                                                            p.TG >= dtStart &&
                                                            p.TG <= dtEnd)
                                               .Count();
                var total = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == _MaCB &&
                                                         p.TG >= dtStart &&
                                                         p.TG <= dtEnd)
                                             .Count();
                double tyle_RHL = 0;
                double tyle_HL = 0;
                double tyle_BT = 0;
                double tyle_KHL = 0;
                if (total > 0)
                {
                    tyle_RHL = Math.Round(((double)countRHL / (double)total) * 100.00, 2);
                    tyle_HL = Math.Round(((double)countHL / (double)total) * 100.00, 2);
                    tyle_BT = Math.Round(((double)countBT / (double)total) * 100.00, 2);
                    tyle_KHL = Math.Round(((double)countKHL / (double)total) * 100.00, 2);
                }
                KetQuaDanhGia_BaoCao_ md = new KetQuaDanhGia_BaoCao_()
                {
                    Ngay = dtStart,
                    RHL_SoLan = countRHL,
                    HL_SoLan = countHL,
                    BT_SoLan = countBT,
                    KHL_SoLan = countKHL,
                    RHL_TyLe = tyle_RHL,
                    HL_TyLe = tyle_HL,
                    BT_TyLe = tyle_BT,
                    KHL_TyLe = tyle_KHL
                };
                listMD.Add(md);
            }
            return listMD;
        }
        /// <summary>
        /// Phương thức lấy điểm
        /// </summary>
        /// <param name="MaMucDo">Mã mức độ</param>
        /// <returns></returns>
        public int LayDiem(int MaMucDo, int soluong)
        {
            int diem = 0;
            var ef = db.MUCDODANHGIAs.Where(p=>p.MUCDO == MaMucDo).FirstOrDefault();
            if (ef != null) diem = (int)ef.DIEM;
            return diem * soluong;
        }
    }
}
