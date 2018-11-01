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
    public class ThuTucAPIController : ApiController
    {
        HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities();
        /// <summary>
        /// Lấy thời gian giải quyết thủ tục tổng hợp
        /// (thời gian chờ, thời gian giải quyết, tổng thời gian)
        /// </summary>
        /// <param name="_Loai">Kiểu lọc (tháng hoặc năm)</param>
        /// <param name="_GiaTri">Giá trị theo kiểu lọc</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<ThuTucChart> Get(string _Loai, string _GiaTri)
        {
            IList<ThuTucChart> lstMD = new List<ThuTucChart>();
            DateTime start = new DateTime();
            DateTime end = new DateTime();

            if (_Loai == "nam") // Nếu thống kê theo năm thì sẽ tạo ra khoảng thời gian trong 1 năm
            {
                int Nam = Convert.ToInt32(_GiaTri);
                for (int i = 1; i < 13; i++)
                {
                    start = new DateTime(Nam, i, 1, 0, 0, 0);
                    end = new DateTime(Nam, i, DateTime.DaysInMonth(Nam, i), 23, 59, 59);// Tìm các số thứ tự có trong khoảng thời gian trên
                    var lstSttEF = db.KETQUADANHGIAs.Where(p => p.TG >= start &&
                                                                p.TG <= end)
                                                    .ToList();
                    double average_cho = 0;
                    double average_xuly = 0;
                    double average_tong = 0;
                    int so_luong_giai_quyet = 0;
                    // Tìm thời gian rút số, gọi số, hoàn tất với mỗi số thứ tự
                    foreach (var item in lstSttEF)
                    {
                        DateTime tg = (DateTime)(item.TG);
                        DateTime now = new DateTime(tg.Year, tg.Month, tg.Day, 0, 0, 0);
                        DateTime endNow = new DateTime(tg.Year, tg.Month, tg.Day, 23, 59, 59);
                        int mabp = (int)item.SOTHUTU.CANBO.MABP;
                        int macb = (int)item.SOTHUTU.MACB;
                        int stt = (int)item.SOTHUTU.STT;
                        int mastt = (int)item.MASTT;

                        var rutso = db.SOTOIDAs.Where(p => p.STTTD == stt &&
                                                           p.MABP == mabp &&
                                                           p.TG >= now &&
                                                           p.TG <= endNow)
                                                    .Select(p => p.TG)
                                                    .FirstOrDefault();
                        var goiso = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                               .Select(p => p.BD)
                                               .FirstOrDefault();
                        var hoantat = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                                 .Select(p => p.KT)
                                                 .FirstOrDefault();
                        if (rutso != null && goiso != null && hoantat != null)
                        {
                            DateTime tg_rut = (DateTime)rutso;
                            DateTime tg_goi = (DateTime)goiso;
                            DateTime tg_hoan_tat = (DateTime)hoantat;
                            double phien_cho = Math.Abs(((TimeSpan)(tg_rut - tg_goi)).TotalMinutes);
                            double phien_xu_ly = Math.Abs(((TimeSpan)(tg_goi - tg_hoan_tat)).TotalMinutes);
                            //double tong_phien = Math.Abs(((TimeSpan)(tg_rut - tg_hoan_tat)).TotalMinutes);
                            average_cho += phien_cho;
                            average_xuly += phien_xu_ly;
                            //average_tong += tong_phien;
                        so_luong_giai_quyet++;
                        }
                    }
                    if (lstSttEF.Count > 0)
                    {
                        average_cho = Math.Round(average_cho / lstSttEF.Count, 0);
                        average_xuly = Math.Round(average_xuly / lstSttEF.Count, 0);
                        average_tong = average_cho + average_xuly;
                    }
                    ThuTucChart md = new ThuTucChart()
                    {
                        ThoiGianCho = average_cho,
                        ThoiGianGiaiQuyet = average_xuly,
                        TongThoiGian = average_tong,
                        ThoiGian = "Tháng " + i,
                        SoLuongGiaiQuyet = so_luong_giai_quyet
                    };
                    lstMD.Add(md);
                }
            }
            else if (_Loai == "thang") // Nếu thống kê theo tháng thì sẽ tạo ra khoảng thời gian trong 1 tháng
            {
                string[] arr = _GiaTri.Split(' ');
                int Nam = Convert.ToInt32(arr[1]);
                int Thang = Convert.ToInt32(arr[0]);
                for (int i = 1; i < DateTime.DaysInMonth(Nam, Thang) + 1; i++)
                {
                    start = new DateTime(Nam, Thang, i, 0, 0, 0);
                    end = new DateTime(Nam, Thang, i, 23, 59, 59);
                    // Tìm các số thứ tự có trong khoảng thời gian trên
                    var lstSttEF = db.KETQUADANHGIAs.Where(p => p.TG >= start &&
                                                                p.TG <= end)
                                                    .ToList();
                    // Tìm thời gian rút số, gọi số, hoàn tất với mỗi số thứ tự
                    double average_cho = 0;
                    double average_xuly = 0;
                    double average_tong = 0;
                    int so_luong_giai_quyet = 0;
                    foreach (var item in lstSttEF)
                    {
                        DateTime tg = (DateTime)(item.TG);
                        DateTime now = new DateTime(tg.Year, tg.Month, tg.Day, 0, 0, 0);
                        DateTime endNow = new DateTime(tg.Year, tg.Month, tg.Day, 23, 59, 59);
                        int mabp = (int)item.SOTHUTU.CANBO.MABP;
                        int macb = (int)item.SOTHUTU.MACB;
                        int stt = (int)item.SOTHUTU.STT;
                        int mastt = (int)item.MASTT;

                        var rutso = db.SOTOIDAs.Where(p => p.STTTD == stt &&
                                                           p.MABP == mabp &&
                                                           p.TG >= now &&
                                                           p.TG <= endNow)
                                                    .Select(p => p.TG)
                                                    .FirstOrDefault();
                        var goiso = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                               .Select(p => p.BD)
                                               .FirstOrDefault();
                        var hoantat = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                                 .Select(p => p.KT)
                                                 .FirstOrDefault();
                        if (rutso != null && goiso != null && hoantat != null)
                        {
                            DateTime tg_rut = (DateTime)rutso;
                            DateTime tg_goi = (DateTime)goiso;
                            DateTime tg_hoan_tat = (DateTime)hoantat;
                            double phien_cho = Math.Abs(((TimeSpan)(tg_rut - tg_goi)).TotalMinutes);
                            double phien_xu_ly = Math.Abs(((TimeSpan)(tg_goi - tg_hoan_tat)).TotalMinutes);
                            //double tong_phien = phien_cho + phien_xu_ly;
                            average_cho += phien_cho;
                            average_xuly += phien_xu_ly;
                            so_luong_giai_quyet++;
                        }
                    }
                    if (lstSttEF.Count > 0)
                    {
                        average_cho = Math.Round(Math.Abs(average_cho / lstSttEF.Count), 0);
                        average_xuly = Math.Round(Math.Abs(average_xuly / lstSttEF.Count), 0);
                        average_tong = average_cho + average_xuly;
                    }
                    ThuTucChart md = new ThuTucChart()
                    {
                        ThoiGianCho = average_cho,
                        ThoiGianGiaiQuyet = average_xuly,
                        TongThoiGian = average_tong,
                        ThoiGian = "Ngày " + i,
                        SoLuongGiaiQuyet = so_luong_giai_quyet
                    };
                    lstMD.Add(md);
                }
            }
            return lstMD;
        }
        /// <summary>
        /// Lấy thời giải quyết thủ tục cho grid tổng hợp
        /// </summary>
        /// <param name="_Loai">Kiểu lọc (Năm hoặc tháng)</param>
        /// <param name="_GiaTri">Giá trị theo kiểu lọc</param>
        /// <param name="_Tong">Tham số xác nhận của phương thức</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<ThuTucChart> Get(string _Loai, string _GiaTri, int _Tong)
        {
            IList<ThuTucChart> lstMD = new List<ThuTucChart>();
            DateTime start = new DateTime();
            DateTime end = new DateTime();

            if (_Loai == "nam") // Nếu thống kê theo năm thì sẽ tạo ra khoảng thời gian trong 1 năm
            {
                int Nam = Convert.ToInt32(_GiaTri);
                start = new DateTime(Nam, 1, 1, 0, 0, 0);
                end = new DateTime(Nam, 12, DateTime.DaysInMonth(Nam, 12), 23, 59, 59);
                // Tìm các số thứ tự có trong khoảng thời gian trên
                var lstSttEF = db.KETQUADANHGIAs.Where(p => p.TG >= start &&
                                                            p.TG <= end)
                                                .OrderBy(p => p.SOTHUTU.CANBO.MABP)
                                                .ToList();
                // Tìm thời gian rút số, gọi số, hoàn tất với mỗi số thứ tự
                foreach (var item in lstSttEF)
                {
                    DateTime tg = (DateTime)(item.TG);
                    DateTime now = new DateTime(tg.Year, tg.Month, tg.Day, 0, 0, 0);
                    DateTime endNow = new DateTime(tg.Year, tg.Month, tg.Day, 23, 59, 59);
                    int mabp = (int)item.SOTHUTU.CANBO.MABP;
                    int macb = (int)item.SOTHUTU.MACB;
                    int stt = (int)item.SOTHUTU.STT;
                    int mastt = (int)item.MASTT;

                    var rutso = db.SOTOIDAs.Where(p => p.STTTD == stt &&
                                                       p.MABP == mabp &&
                                                       p.TG >= now &&
                                                       p.TG <= endNow)
                                                .Select(p => p.TG)
                                                .FirstOrDefault();
                    var goiso = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                           .Select(p => p.BD)
                                           .FirstOrDefault();
                    var hoantat = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                             .Select(p => p.KT)
                                             .FirstOrDefault();
                    if (rutso != null && goiso != null && hoantat != null)
                    {
                        DateTime tg_rut = (DateTime)rutso;
                        DateTime tg_goi = (DateTime)goiso;
                        DateTime tg_hoan_tat = (DateTime)hoantat;
                        double phien_cho = Math.Round(Math.Abs(((TimeSpan)(tg_rut - tg_goi)).TotalMinutes), 0);
                        double phien_xu_ly = Math.Round(Math.Abs(((TimeSpan)(tg_goi - tg_hoan_tat)).TotalMinutes), 0);
                        double tong_phien = phien_cho + phien_xu_ly;

                        ThuTucChart md = new ThuTucChart()
                        {
                            MaSTT = item.MASTT,
                            MaBP = (int)item.SOTHUTU.CANBO.MABP,
                            TenBP = item.SOTHUTU.CANBO.BOPHAN.TENBP,
                            SoThuTu = (int)item.SOTHUTU.STT,
                            ThoiGianRut = tg_rut,
                            ThoiGianGoi = tg_goi,
                            ThoiGianHoanTat = tg_hoan_tat,
                            ThoiGianCho = phien_cho,
                            ThoiGianGiaiQuyet = phien_xu_ly,
                            TongThoiGian = tong_phien,
                            Ngay = now
                        };
                        lstMD.Add(md);
                    }
                }
            }
            else if (_Loai == "thang") // Nếu thống kê theo tháng thì sẽ tạo ra khoảng thời gian trong 1 tháng
            {
                string[] arr = _GiaTri.Split(' ');
                int Nam = Convert.ToInt32(arr[1]);
                int Thang = Convert.ToInt32(arr[0]);
                start = new DateTime(Nam, Thang, 1, 0, 0, 0);
                end = new DateTime(Nam, Thang, DateTime.DaysInMonth(Nam, Thang), 23, 59, 59);
                // Tìm các số thứ tự có trong khoảng thời gian trên
                var lstSttEF = db.KETQUADANHGIAs.Where(p => p.TG >= start &&
                                                            p.TG <= end)
                                                .OrderBy(p => p.SOTHUTU.CANBO.MABP)
                                                .ToList();
                // Tìm thời gian rút số, gọi số, hoàn tất với mỗi số thứ tự
                foreach (var item in lstSttEF)
                {
                    DateTime tg = (DateTime)(item.TG);
                    DateTime now = new DateTime(tg.Year, tg.Month, tg.Day, 0, 0, 0);
                    DateTime endNow = new DateTime(tg.Year, tg.Month, tg.Day, 23, 59, 59);
                    int mabp = (int)item.SOTHUTU.CANBO.MABP;
                    int macb = (int)item.SOTHUTU.MACB;
                    int stt = (int)item.SOTHUTU.STT;
                    int mastt = (int)item.MASTT;

                    var rutso = db.SOTOIDAs.Where(p => p.STTTD == stt &&
                                                       p.MABP == mabp &&
                                                       p.TG >= now &&
                                                       p.TG <= endNow)
                                                .Select(p => p.TG)
                                                .FirstOrDefault();
                    var goiso = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                           .Select(p => p.BD)
                                           .FirstOrDefault();
                    var hoantat = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                             .Select(p => p.KT)
                                             .FirstOrDefault();
                    if (rutso != null && goiso != null && hoantat != null)
                    {
                        DateTime tg_rut = (DateTime)rutso;
                        DateTime tg_goi = (DateTime)goiso;
                        DateTime tg_hoan_tat = (DateTime)hoantat;
                        double phien_cho = Math.Round(Math.Abs(((TimeSpan)(tg_rut - tg_goi)).TotalMinutes), 0);
                        double phien_xu_ly = Math.Round(Math.Abs(((TimeSpan)(tg_goi - tg_hoan_tat)).TotalMinutes), 0);
                        double tong_phien = phien_cho + phien_xu_ly;
                        ThuTucChart md = new ThuTucChart()
                        {
                            MaSTT = item.MASTT,
                            MaBP = (int)item.SOTHUTU.CANBO.MABP,
                            TenBP = item.SOTHUTU.CANBO.BOPHAN.TENBP,
                            SoThuTu = (int)item.SOTHUTU.STT,
                            ThoiGianRut = tg_rut,
                            ThoiGianGoi = tg_goi,
                            ThoiGianHoanTat = tg_hoan_tat,
                            ThoiGianCho = phien_cho,
                            ThoiGianGiaiQuyet = phien_xu_ly,
                            TongThoiGian = tong_phien,
                            Ngay = now
                        };
                        lstMD.Add(md);
                    }
                }
            }
            return lstMD;
        }
        /// <summary>
        /// Lấy thời gian giải quyết thủ tục theo bộ phận
        /// (thời gian chờ, thời gian giải quyết, tổng thời gian)
        /// </summary>
        /// <param name="_Loai">Kiểu lọc (tháng hoặc năm)</param>
        /// <param name="_GiaTri">Giá trị theo kiểu lọc</param>
        /// <param name="_MaBP">Mã bộ phận</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<ThuTucChart> GetBoPhan(string _Loai, string _GiaTri, int _MaBP)
        {
            IList<ThuTucChart> lstMD = new List<ThuTucChart>();
            DateTime start = new DateTime();
            DateTime end = new DateTime();

            if (_Loai == "nam") // Nếu thống kê theo năm thì sẽ tạo ra khoảng thời gian trong 1 năm
            {
                int Nam = Convert.ToInt32(_GiaTri);
                for (int i = 1; i < 13; i++)
                {
                    start = new DateTime(Nam, i, 1, 0, 0, 0);
                    end = new DateTime(Nam, i, DateTime.DaysInMonth(Nam, i), 23, 59, 59);// Tìm các số thứ tự có trong khoảng thời gian trên
                    var lstSttEF = db.KETQUADANHGIAs.Where(p => p.TG >= start &&
                                                                p.TG <= end &&
                                                                p.SOTHUTU.CANBO.MABP == _MaBP)
                                                    .ToList();
                    double average_cho = 0;
                    double average_xuly = 0;
                    double average_tong = 0;
                    int so_luong_giai_quyet = 0;
                    // Tìm thời gian rút số, gọi số, hoàn tất với mỗi số thứ tự
                    foreach (var item in lstSttEF)
                    {
                        DateTime tg = (DateTime)(item.TG);
                        DateTime now = new DateTime(tg.Year, tg.Month, tg.Day, 0, 0, 0);
                        DateTime endNow = new DateTime(tg.Year, tg.Month, tg.Day, 23, 59, 59);
                        int mabp = (int)item.SOTHUTU.CANBO.MABP;
                        int macb = (int)item.SOTHUTU.MACB;
                        int stt = (int)item.SOTHUTU.STT;
                        int mastt = (int)item.MASTT;

                        var rutso = db.SOTOIDAs.Where(p => p.STTTD == stt &&
                                                           p.MABP == mabp &&
                                                           p.TG >= now &&
                                                           p.TG <= endNow)
                                                    .Select(p => p.TG)
                                                    .FirstOrDefault();
                        var goiso = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                               .Select(p => p.BD)
                                               .FirstOrDefault();
                        var hoantat = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                                 .Select(p => p.KT)
                                                 .FirstOrDefault();
                        if (rutso != null && goiso != null && hoantat != null)
                        {
                            DateTime tg_rut = (DateTime)rutso;
                            DateTime tg_goi = (DateTime)goiso;
                            DateTime tg_hoan_tat = (DateTime)hoantat;
                            double phien_cho = Math.Abs(((TimeSpan)(tg_rut - tg_goi)).TotalMinutes);
                            double phien_xu_ly = Math.Abs(((TimeSpan)(tg_goi - tg_hoan_tat)).TotalMinutes);
                            //double phien_tong = phien_cho + phien_xu_ly;
                            average_cho += phien_cho;
                            average_xuly += phien_xu_ly;
                            //average_tong += tong_phien;
                        so_luong_giai_quyet++;
                        }
                    }
                    if (lstSttEF.Count > 0)
                    {
                        average_cho = Math.Round(average_cho / lstSttEF.Count, 0);
                        average_xuly = Math.Round(average_xuly / lstSttEF.Count, 0);
                        average_tong = average_cho + average_xuly;
                    }
                    ThuTucChart md = new ThuTucChart()
                    {
                        ThoiGianCho = average_cho,
                        ThoiGianGiaiQuyet = average_xuly,
                        TongThoiGian = average_tong,
                        ThoiGian = "Tháng " + i,
                        SoLuongGiaiQuyet = so_luong_giai_quyet
                    };
                    lstMD.Add(md);
                }
            }
            else if (_Loai == "thang") // Nếu thống kê theo tháng thì sẽ tạo ra khoảng thời gian trong 1 tháng
            {
                string[] arr = _GiaTri.Split(' ');
                int Nam = Convert.ToInt32(arr[1]);
                int Thang = Convert.ToInt32(arr[0]);
                for (int i = 1; i < DateTime.DaysInMonth(Nam, Thang) + 1; i++)
                {
                    start = new DateTime(Nam, Thang, i, 0, 0, 0);
                    end = new DateTime(Nam, Thang, i, 23, 59, 59);
                    // Tìm các số thứ tự có trong khoảng thời gian trên
                    var lstSttEF = db.KETQUADANHGIAs.Where(p => p.TG >= start &&
                                                                p.TG <= end &&
                                                                p.SOTHUTU.CANBO.MABP == _MaBP)
                                                    .ToList();
                    // Tìm thời gian rút số, gọi số, hoàn tất với mỗi số thứ tự
                    double average_cho = 0;
                    double average_xuly = 0;
                    double average_tong = 0;
                    int so_luong_giai_quyet = 0;
                    foreach (var item in lstSttEF)
                    {
                        DateTime tg = (DateTime)(item.TG);
                        DateTime now = new DateTime(tg.Year, tg.Month, tg.Day, 0, 0, 0);
                        DateTime endNow = new DateTime(tg.Year, tg.Month, tg.Day, 23, 59, 59);
                        int mabp = (int)item.SOTHUTU.CANBO.MABP;
                        int macb = (int)item.SOTHUTU.MACB;
                        int stt = (int)item.SOTHUTU.STT;
                        int mastt = (int)item.MASTT;

                        var rutso = db.SOTOIDAs.Where(p => p.STTTD == stt &&
                                                           p.MABP == mabp &&
                                                           p.TG >= now &&
                                                           p.TG <= endNow)
                                                    .Select(p => p.TG)
                                                    .FirstOrDefault();
                        var goiso = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                               .Select(p => p.BD)
                                               .FirstOrDefault();
                        var hoantat = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                                 .Select(p => p.KT)
                                                 .FirstOrDefault();
                        if (rutso != null && goiso != null && hoantat != null)
                        {
                            DateTime tg_rut = (DateTime)rutso;
                            DateTime tg_goi = (DateTime)goiso;
                            DateTime tg_hoan_tat = (DateTime)hoantat;
                            double phien_cho = Math.Abs(((TimeSpan)(tg_rut - tg_goi)).TotalMinutes);
                            double phien_xu_ly = Math.Abs(((TimeSpan)(tg_goi - tg_hoan_tat)).TotalMinutes);
                            double tong_phien = phien_cho + phien_xu_ly;
                            average_cho += phien_cho;
                            average_xuly += phien_xu_ly;
                        so_luong_giai_quyet++;
                        }
                    }
                    if (lstSttEF.Count > 0)
                    {
                        average_cho = Math.Round(Math.Abs(average_cho / lstSttEF.Count), 0);
                        average_xuly = Math.Round(Math.Abs(average_xuly / lstSttEF.Count), 0);
                        average_tong = average_cho + average_xuly;
                    }
                    ThuTucChart md = new ThuTucChart()
                    {
                        ThoiGianCho = average_cho,
                        ThoiGianGiaiQuyet = average_xuly,
                        TongThoiGian = average_tong,
                        ThoiGian = "Ngày " + i,
                        SoLuongGiaiQuyet = so_luong_giai_quyet
                    };
                    lstMD.Add(md);
                }
            }
            return lstMD;
        }
        /// <summary>
        /// Lấy thời giải quyết thủ tục cho grid bộ phận
        /// </summary>
        /// <param name="_Loai">Kiểu lọc (Năm hoặc tháng)</param>
        /// <param name="_GiaTri">Giá trị theo kiểu lọc</param>
        /// <param name="_MaBP">Mã bộ phận</param>
        /// <param name="_Tong">Tham số xác nhận của phương thức</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<ThuTucChart> GetBoPhan(string _Loai, string _GiaTri, int _MaBP, int _Tong)
        {
            IList<ThuTucChart> lstMD = new List<ThuTucChart>();
            DateTime start = new DateTime();
            DateTime end = new DateTime();

            if (_Loai == "nam") // Nếu thống kê theo năm thì sẽ tạo ra khoảng thời gian trong 1 năm
            {
                int Nam = Convert.ToInt32(_GiaTri);
                start = new DateTime(Nam, 1, 1, 0, 0, 0);
                end = new DateTime(Nam, 12, DateTime.DaysInMonth(Nam, 12), 23, 59, 59);
                // Tìm các số thứ tự có trong khoảng thời gian trên
                var lstSttEF = db.KETQUADANHGIAs.Where(p => p.TG >= start &&
                                                            p.TG <= end &&
                                                            p.SOTHUTU.CANBO.MABP == _MaBP)
                                                .OrderBy(p => p.SOTHUTU.MACB)
                                                .ToList();
                // Tìm thời gian rút số, gọi số, hoàn tất với mỗi số thứ tự
                foreach (var item in lstSttEF)
                {
                    DateTime tg = (DateTime)(item.TG);
                    DateTime now = new DateTime(tg.Year, tg.Month, tg.Day, 0, 0, 0);
                    DateTime endNow = new DateTime(tg.Year, tg.Month, tg.Day, 23, 59, 59);
                    int mabp = (int)item.SOTHUTU.CANBO.MABP;
                    int macb = (int)item.SOTHUTU.MACB;
                    string hoten = item.SOTHUTU.CANBO.HOTEN;
                    int stt = (int)item.SOTHUTU.STT;
                    int mastt = (int)item.MASTT;

                    var rutso = db.SOTOIDAs.Where(p => p.STTTD == stt &&
                                                       p.MABP == mabp &&
                                                       p.TG >= now &&
                                                       p.TG <= endNow)
                                                .Select(p => p.TG)
                                                .FirstOrDefault();
                    var goiso = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                           .Select(p => p.BD)
                                           .FirstOrDefault();
                    var hoantat = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                             .Select(p => p.KT)
                                             .FirstOrDefault();
                    if (rutso != null && goiso != null && hoantat != null)
                    {
                        DateTime tg_rut = (DateTime)rutso;
                        DateTime tg_goi = (DateTime)goiso;
                        DateTime tg_hoan_tat = (DateTime)hoantat;
                        double phien_cho = Math.Round(Math.Abs(((TimeSpan)(tg_rut - tg_goi)).TotalMinutes), 0);
                        double phien_xu_ly = Math.Round(Math.Abs(((TimeSpan)(tg_goi - tg_hoan_tat)).TotalMinutes), 0);
                        double tong_phien = phien_cho + phien_xu_ly;

                        ThuTucChart md = new ThuTucChart()
                        {
                            MaSTT = item.MASTT,
                            MaCB = macb,
                            HoTen = hoten,
                            SoThuTu = (int)item.SOTHUTU.STT,
                            ThoiGianRut = tg_rut,
                            ThoiGianGoi = tg_goi,
                            ThoiGianHoanTat = tg_hoan_tat,
                            ThoiGianCho = phien_cho,
                            ThoiGianGiaiQuyet = phien_xu_ly,
                            TongThoiGian = tong_phien,
                            Ngay = now
                        };
                        lstMD.Add(md);
                    }
                }
            }
            else if (_Loai == "thang") // Nếu thống kê theo tháng thì sẽ tạo ra khoảng thời gian trong 1 tháng
            {
                string[] arr = _GiaTri.Split(' ');
                int Nam = Convert.ToInt32(arr[1]);
                int Thang = Convert.ToInt32(arr[0]);
                start = new DateTime(Nam, Thang, 1, 0, 0, 0);
                end = new DateTime(Nam, Thang, DateTime.DaysInMonth(Nam, Thang), 23, 59, 59);
                // Tìm các số thứ tự có trong khoảng thời gian trên
                var lstSttEF = db.KETQUADANHGIAs.Where(p => p.TG >= start &&
                                                            p.TG <= end &&
                                                            p.SOTHUTU.CANBO.MABP == _MaBP)
                                                .OrderBy(p => p.SOTHUTU.MACB)
                                                .ToList();
                // Tìm thời gian rút số, gọi số, hoàn tất với mỗi số thứ tự
                foreach (var item in lstSttEF)
                {
                    DateTime tg = (DateTime)(item.TG);
                    DateTime now = new DateTime(tg.Year, tg.Month, tg.Day, 0, 0, 0);
                    DateTime endNow = new DateTime(tg.Year, tg.Month, tg.Day, 23, 59, 59);
                    int mabp = (int)item.SOTHUTU.CANBO.MABP;
                    int macb = (int)item.SOTHUTU.MACB;
                    string hoten = item.SOTHUTU.CANBO.HOTEN;
                    int stt = (int)item.SOTHUTU.STT;
                    int mastt = (int)item.MASTT;

                    var rutso = db.SOTOIDAs.Where(p => p.STTTD == stt &&
                                                       p.MABP == mabp &&
                                                       p.TG >= now &&
                                                       p.TG <= endNow)
                                                .Select(p => p.TG)
                                                .FirstOrDefault();
                    var goiso = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                           .Select(p => p.BD)
                                           .FirstOrDefault();
                    var hoantat = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                             .Select(p => p.KT)
                                             .FirstOrDefault();
                    if (rutso != null && goiso != null && hoantat != null)
                    {
                        DateTime tg_rut = (DateTime)rutso;
                        DateTime tg_goi = (DateTime)goiso;
                        DateTime tg_hoan_tat = (DateTime)hoantat;
                        double phien_cho = Math.Round(Math.Abs(((TimeSpan)(tg_rut - tg_goi)).TotalMinutes), 0);
                        double phien_xu_ly = Math.Round(Math.Abs(((TimeSpan)(tg_goi - tg_hoan_tat)).TotalMinutes), 0);
                        double tong_phien = phien_cho + phien_xu_ly;
                        ThuTucChart md = new ThuTucChart()
                        {
                            MaSTT = item.MASTT,
                            MaCB = macb,
                            HoTen = hoten,
                            SoThuTu = (int)item.SOTHUTU.STT,
                            ThoiGianRut = tg_rut,
                            ThoiGianGoi = tg_goi,
                            ThoiGianHoanTat = tg_hoan_tat,
                            ThoiGianCho = phien_cho,
                            ThoiGianGiaiQuyet = phien_xu_ly,
                            TongThoiGian = tong_phien,
                            Ngay = now
                        };
                        lstMD.Add(md);
                    }
                }
            }
            return lstMD;
        }
        /// <summary>
        /// Lấy thời gian giải quyết thủ tục theo cán bộ
        /// (thời gian chờ, thời gian giải quyết, tổng thời gian)
        /// </summary>
        /// <param name="_Loai">Kiểu lọc (tháng hoặc năm)</param>
        /// <param name="_GiaTri">Giá trị theo kiểu lọc</param>
        /// <param name="_MaCB">Mã cán bộ</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<ThuTucChart> GetCanBo(string _Loai, string _GiaTri, int _MaCB)
        {
            IList<ThuTucChart> lstMD = new List<ThuTucChart>();
            DateTime start = new DateTime();
            DateTime end = new DateTime();

            if (_Loai == "nam") // Nếu thống kê theo năm thì sẽ tạo ra khoảng thời gian trong 1 năm
            {
                int Nam = Convert.ToInt32(_GiaTri);
                for (int i = 1; i < 13; i++)
                {
                    start = new DateTime(Nam, i, 1, 0, 0, 0);
                    end = new DateTime(Nam, i, DateTime.DaysInMonth(Nam, i), 23, 59, 59);// Tìm các số thứ tự có trong khoảng thời gian trên
                    var lstSttEF = db.KETQUADANHGIAs.Where(p => p.TG >= start &&
                                                                p.TG <= end &&
                                                                p.SOTHUTU.MACB == _MaCB)
                                                    .ToList();
                    double average_cho = 0;
                    double average_xuly = 0;
                    double average_tong = 0;
                    int so_luong_giai_quyet = 0;
                    // Tìm thời gian rút số, gọi số, hoàn tất với mỗi số thứ tự
                    foreach (var item in lstSttEF)
                    {
                        DateTime tg = (DateTime)(item.TG);
                        DateTime now = new DateTime(tg.Year, tg.Month, tg.Day, 0, 0, 0);
                        DateTime endNow = new DateTime(tg.Year, tg.Month, tg.Day, 23, 59, 59);
                        int mabp = (int)item.SOTHUTU.CANBO.MABP;
                        int macb = (int)item.SOTHUTU.MACB;
                        int stt = (int)item.SOTHUTU.STT;
                        int mastt = (int)item.MASTT;

                        var rutso = db.SOTOIDAs.Where(p => p.STTTD == stt &&
                                                           p.MABP == mabp &&
                                                           p.TG >= now &&
                                                           p.TG <= endNow)
                                                    .Select(p => p.TG)
                                                    .FirstOrDefault();
                        var goiso = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                               .Select(p => p.BD)
                                               .FirstOrDefault();
                        var hoantat = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                                 .Select(p => p.KT)
                                                 .FirstOrDefault();
                        if (rutso != null && goiso != null && hoantat != null)
                        {
                            DateTime tg_rut = (DateTime)rutso;
                            DateTime tg_goi = (DateTime)goiso;
                            DateTime tg_hoan_tat = (DateTime)hoantat;
                            double phien_cho = Math.Abs(((TimeSpan)(tg_rut - tg_goi)).TotalMinutes);
                            double phien_xu_ly = Math.Abs(((TimeSpan)(tg_goi - tg_hoan_tat)).TotalMinutes);
                            //double phien_tong = phien_cho + phien_xu_ly;
                            average_cho += phien_cho;
                            average_xuly += phien_xu_ly;
                            //average_tong += tong_phien;
                        so_luong_giai_quyet++;
                        }
                    }
                    if (lstSttEF.Count > 0)
                    {
                        average_cho = Math.Round(average_cho / lstSttEF.Count, 0);
                        average_xuly = Math.Round(average_xuly / lstSttEF.Count, 0);
                        average_tong = average_cho + average_xuly;
                    }
                    ThuTucChart md = new ThuTucChart()
                    {
                        ThoiGianCho = average_cho,
                        ThoiGianGiaiQuyet = average_xuly,
                        TongThoiGian = average_tong,
                        ThoiGian = "Tháng " + i,
                        SoLuongGiaiQuyet = so_luong_giai_quyet
                    };
                    lstMD.Add(md);
                }
            }
            else if (_Loai == "thang") // Nếu thống kê theo tháng thì sẽ tạo ra khoảng thời gian trong 1 tháng
            {
                string[] arr = _GiaTri.Split(' ');
                int Nam = Convert.ToInt32(arr[1]);
                int Thang = Convert.ToInt32(arr[0]);
                for (int i = 1; i < DateTime.DaysInMonth(Nam, Thang) + 1; i++)
                {
                    start = new DateTime(Nam, Thang, i, 0, 0, 0);
                    end = new DateTime(Nam, Thang, i, 23, 59, 59);
                    // Tìm các số thứ tự có trong khoảng thời gian trên
                    var lstSttEF = db.KETQUADANHGIAs.Where(p => p.TG >= start &&
                                                                p.TG <= end &&
                                                                p.SOTHUTU.MACB == _MaCB)
                                                    .ToList();
                    // Tìm thời gian rút số, gọi số, hoàn tất với mỗi số thứ tự
                    double average_cho = 0;
                    double average_xuly = 0;
                    double average_tong = 0;
                    int so_luong_giai_quyet = 0;
                    foreach (var item in lstSttEF)
                    {
                        DateTime tg = (DateTime)(item.TG);
                        DateTime now = new DateTime(tg.Year, tg.Month, tg.Day, 0, 0, 0);
                        DateTime endNow = new DateTime(tg.Year, tg.Month, tg.Day, 23, 59, 59);
                        int mabp = (int)item.SOTHUTU.CANBO.MABP;
                        int macb = (int)item.SOTHUTU.MACB;
                        int stt = (int)item.SOTHUTU.STT;
                        int mastt = (int)item.MASTT;

                        var rutso = db.SOTOIDAs.Where(p => p.STTTD == stt &&
                                                           p.MABP == mabp &&
                                                           p.TG >= now &&
                                                           p.TG <= endNow)
                                                    .Select(p => p.TG)
                                                    .FirstOrDefault();
                        var goiso = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                               .Select(p => p.BD)
                                               .FirstOrDefault();
                        var hoantat = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                                 .Select(p => p.KT)
                                                 .FirstOrDefault();
                        if (rutso != null && goiso != null && hoantat != null)
                        {
                            DateTime tg_rut = (DateTime)rutso;
                            DateTime tg_goi = (DateTime)goiso;
                            DateTime tg_hoan_tat = (DateTime)hoantat;
                            double phien_cho = Math.Abs(((TimeSpan)(tg_rut - tg_goi)).TotalMinutes);
                            double phien_xu_ly = Math.Abs(((TimeSpan)(tg_goi - tg_hoan_tat)).TotalMinutes);
                            double tong_phien = phien_cho + phien_xu_ly;
                            average_cho += phien_cho;
                            average_xuly += phien_xu_ly;
                        so_luong_giai_quyet++;
                        }
                    }
                    if (lstSttEF.Count > 0)
                    {
                        average_cho = Math.Round(Math.Abs(average_cho / lstSttEF.Count), 0);
                        average_xuly = Math.Round(Math.Abs(average_xuly / lstSttEF.Count), 0);
                        average_tong = average_cho + average_xuly;
                    }
                    ThuTucChart md = new ThuTucChart()
                    {
                        ThoiGianCho = average_cho,
                        ThoiGianGiaiQuyet = average_xuly,
                        TongThoiGian = average_tong,
                        ThoiGian = "Ngày " + i,
                        SoLuongGiaiQuyet = so_luong_giai_quyet
                    };
                    lstMD.Add(md);
                }
            }
            return lstMD;
        }
        /// <summary>
        /// Lấy thời giải quyết thủ tục cho grid cán bộ
        /// </summary>
        /// <param name="_Loai">Kiểu lọc (Năm hoặc tháng)</param>
        /// <param name="_GiaTri">Giá trị theo kiểu lọc</param>
        /// <param name="_MaCB">Mã cán bộ</param>
        /// <param name="_Tong">Tham số xác nhận của phương thức</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<ThuTucChart> GetCanBo(string _Loai, string _GiaTri, int _MaCB, int _Tong)
        {
            IList<ThuTucChart> lstMD = new List<ThuTucChart>();
            DateTime start = new DateTime();
            DateTime end = new DateTime();

            if (_Loai == "nam") // Nếu thống kê theo năm thì sẽ tạo ra khoảng thời gian trong 1 năm
            {
                int Nam = Convert.ToInt32(_GiaTri);
                start = new DateTime(Nam, 1, 1, 0, 0, 0);
                end = new DateTime(Nam, 12, DateTime.DaysInMonth(Nam, 12), 23, 59, 59);
                // Tìm các số thứ tự có trong khoảng thời gian trên
                var lstSttEF = db.KETQUADANHGIAs.Where(p => p.TG >= start &&
                                                            p.TG <= end &&
                                                            p.SOTHUTU.MACB == _MaCB)
                                                .OrderBy(p => p.SOTHUTU.MACB)
                                                .ToList();
                // Tìm thời gian rút số, gọi số, hoàn tất với mỗi số thứ tự
                foreach (var item in lstSttEF)
                {
                    DateTime tg = (DateTime)(item.TG);
                    DateTime now = new DateTime(tg.Year, tg.Month, tg.Day, 0, 0, 0);
                    DateTime endNow = new DateTime(tg.Year, tg.Month, tg.Day, 23, 59, 59);
                    int mabp = (int)item.SOTHUTU.CANBO.MABP;
                    int macb = (int)item.SOTHUTU.MACB;
                    string hoten = item.SOTHUTU.CANBO.HOTEN;
                    int stt = (int)item.SOTHUTU.STT;
                    int mastt = (int)item.MASTT;

                    var rutso = db.SOTOIDAs.Where(p => p.STTTD == stt &&
                                                       p.MABP == mabp &&
                                                       p.TG >= now &&
                                                       p.TG <= endNow)
                                                .Select(p => p.TG)
                                                .FirstOrDefault();
                    var goiso = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                           .Select(p => p.BD)
                                           .FirstOrDefault();
                    var hoantat = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                             .Select(p => p.KT)
                                             .FirstOrDefault();
                    if (rutso != null && goiso != null && hoantat != null)
                    {
                        DateTime tg_rut = (DateTime)rutso;
                        DateTime tg_goi = (DateTime)goiso;
                        DateTime tg_hoan_tat = (DateTime)hoantat;
                        double phien_cho = Math.Round(Math.Abs(((TimeSpan)(tg_rut - tg_goi)).TotalMinutes), 0);
                        double phien_xu_ly = Math.Round(Math.Abs(((TimeSpan)(tg_goi - tg_hoan_tat)).TotalMinutes), 0);
                        double tong_phien = phien_cho + phien_xu_ly;

                        ThuTucChart md = new ThuTucChart()
                        {
                            MaSTT = item.MASTT,
                            MaCB = macb,
                            HoTen = hoten,
                            SoThuTu = (int)item.SOTHUTU.STT,
                            ThoiGianRut = tg_rut,
                            ThoiGianGoi = tg_goi,
                            ThoiGianHoanTat = tg_hoan_tat,
                            ThoiGianCho = phien_cho,
                            ThoiGianGiaiQuyet = phien_xu_ly,
                            TongThoiGian = tong_phien,
                            Ngay = now
                        };
                        lstMD.Add(md);
                    }
                }
            }
            else if (_Loai == "thang") // Nếu thống kê theo tháng thì sẽ tạo ra khoảng thời gian trong 1 tháng
            {
                string[] arr = _GiaTri.Split(' ');
                int Nam = Convert.ToInt32(arr[1]);
                int Thang = Convert.ToInt32(arr[0]);
                start = new DateTime(Nam, Thang, 1, 0, 0, 0);
                end = new DateTime(Nam, Thang, DateTime.DaysInMonth(Nam, Thang), 23, 59, 59);
                // Tìm các số thứ tự có trong khoảng thời gian trên
                var lstSttEF = db.KETQUADANHGIAs.Where(p => p.TG >= start &&
                                                            p.TG <= end &&
                                                            p.SOTHUTU.MACB == _MaCB)
                                                .OrderBy(p => p.SOTHUTU.MACB)
                                                .ToList();
                // Tìm thời gian rút số, gọi số, hoàn tất với mỗi số thứ tự
                foreach (var item in lstSttEF)
                {
                    DateTime tg = (DateTime)(item.TG);
                    DateTime now = new DateTime(tg.Year, tg.Month, tg.Day, 0, 0, 0);
                    DateTime endNow = new DateTime(tg.Year, tg.Month, tg.Day, 23, 59, 59);
                    int mabp = (int)item.SOTHUTU.CANBO.MABP;
                    int macb = (int)item.SOTHUTU.MACB;
                    string hoten = item.SOTHUTU.CANBO.HOTEN;
                    int stt = (int)item.SOTHUTU.STT;
                    int mastt = (int)item.MASTT;

                    var rutso = db.SOTOIDAs.Where(p => p.STTTD == stt &&
                                                       p.MABP == mabp &&
                                                       p.TG >= now &&
                                                       p.TG <= endNow)
                                                .Select(p => p.TG)
                                                .FirstOrDefault();
                    var goiso = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                           .Select(p => p.BD)
                                           .FirstOrDefault();
                    var hoantat = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                             .Select(p => p.KT)
                                             .FirstOrDefault();
                    if (rutso != null && goiso != null && hoantat != null)
                    {
                        DateTime tg_rut = (DateTime)rutso;
                        DateTime tg_goi = (DateTime)goiso;
                        DateTime tg_hoan_tat = (DateTime)hoantat;
                        double phien_cho = Math.Round(Math.Abs(((TimeSpan)(tg_rut - tg_goi)).TotalMinutes), 0);
                        double phien_xu_ly = Math.Round(Math.Abs(((TimeSpan)(tg_goi - tg_hoan_tat)).TotalMinutes), 0);
                        double tong_phien = phien_cho + phien_xu_ly;
                        ThuTucChart md = new ThuTucChart()
                        {
                            MaSTT = item.MASTT,
                            MaCB = macb,
                            HoTen = hoten,
                            SoThuTu = (int)item.SOTHUTU.STT,
                            ThoiGianRut = tg_rut,
                            ThoiGianGoi = tg_goi,
                            ThoiGianHoanTat = tg_hoan_tat,
                            ThoiGianCho = phien_cho,
                            ThoiGianGiaiQuyet = phien_xu_ly,
                            TongThoiGian = tong_phien,
                            Ngay = now
                        };
                        lstMD.Add(md);
                    }
                }
            }
            return lstMD;
        }

    }
}
