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
    public class SoSanhAPIController : ApiController
    {
        /// <summary>
        /// Phương thức lấy mức độ đánh giá
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<string> GetMucDo()
        {
            string[] array;
            using (HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities())
            {
                var lstEF = db.MUCDODANHGIAs.ToList();
                array = new string[lstEF.Count + 1];
                int i = 0;
                foreach (var item in lstEF)
                {
                    array[i] = item.LOAI + " (Lần)";
                    i++;
                }
                array[lstEF.Count] = "Điểm số";
            }
            return array;
        }
        /// <summary>
        /// Phương thức so sánh kết quả đánh giá
        /// </summary>
        /// <param name="_MaBP">Mã bộ phận</param>
        /// <param name="_MaCB">Mã cán bộ</param>
        /// <param name="_Start">Chuỗi ngày bắt đầu</param>
        /// <param name="_End">Chuỗi ngày kết thúc</param>
        /// <param name="_LoaiThoiGian">Loại thời gian so sánh</param>
        /// <param name="_Loai">Tham số xác định phương thức</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<SoSanhKetQuaDanhGia> GetKetQuaSoSanh(int _MaBP, int _MaCB, string _Start, string _End, string _LoaiThoiGian, string _Loai)
        {
            IList<SoSanhKetQuaDanhGia> listMD = new List<SoSanhKetQuaDanhGia>();
            using (HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities())
            {
                if (_MaCB == 0) // Nếu mã cán bộ bằng 0 thì so sánh bộ phận
                {
                    DateTime start = GetStartDateTime(_Start); // Lấy thời gian bắt đầu
                    DateTime end = GetEndDateTime(_End); // Lấy thời gian kết thúc
                    var lstMucDo = db.MUCDODANHGIAs.ToList(); // Lấy mức độ đánh giá
                    if (_LoaiThoiGian == "nam") // Nếu loại thời gian cần so sánh là năm
                    {
                        for (int i = start.Year; i <= end.Year; i++) // Vòng lặp từ năm bắt đầu đến năm kết thúc
                        {
                            // Lấy khoảng thời gian so sánh
                            DateTime startCompare = new DateTime(i, 1, 1, 0, 0, 0); // Ngày bắt đầu (Đầu năm)
                            DateTime endCompare = new DateTime(i, 12, DateTime.DaysInMonth(i, 12), 23, 59, 59); // Ngày kết thúc (Cuối năm)
                            double[] arr = new double[lstMucDo.Count + 1]; // Tạo mảng chứa dữ liệu kết quả đánh giá trung bình theo khoảng thời gian đang xét
                            foreach (var item in lstMucDo) // Mỗi năm sẽ gồm có tổng các mức độ đánh giá
                            {
                                var mucdo = item.MUCDO;
                                var count = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.CANBO.MABP == _MaBP &&
                                                                         p.TG != null &&
                                                                         p.TG >= startCompare &&
                                                                         p.TG <= endCompare &&
                                                                         p.MUCDO == mucdo)
                                                             .Count(); // Đếm kết quả đánh giá với mức độ đánh giá tương ứng trong khoảng thời gian đang xét
                                arr[mucdo - 1] = count; // Gán giá trị vào các phần tử trong mảng tương ứng với các mức độ đánh giá
                            }
                            SoSanhKetQuaDanhGia md = new SoSanhKetQuaDanhGia() // Tạo đối tượng chứa năm và kết quả đánh giá trung bình tương ứng
                            {
                                name = i.ToString(),
                                data = arr,
                            };
                            listMD.Add(md);
                        }
                    }
                    else if (_LoaiThoiGian == "thang") // Nếu loại thời gian cần so sánh là tháng
                    {
                        if (start.Year == end.Year) // Nếu khoảng thời gian nằm trong cùng 1 năm
                        {
                            for (int i = start.Month; i <= end.Month; i++)
                            {
                                // Lấy khoảng thời gian so sánh
                                DateTime startCompare = new DateTime(start.Year, i, 1, 0, 0, 0); // Ngày bắt đầu (Đầu tháng)
                                DateTime endCompare = new DateTime(start.Year, i, DateTime.DaysInMonth(start.Year, i), 23, 59, 59); // Ngày kết thúc (Cuối tháng)
                                double[] arr = new double[lstMucDo.Count]; // Tạo mảng chứa dữ liệu kết quả đánh giá trung bình theo khoảng thời gian đang xét
                                foreach (var item in lstMucDo) // Mỗi tháng sẽ có tổng các mức độ đánh giá
                                {
                                    var mucdo = item.MUCDO;
                                    var count = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.CANBO.MABP == _MaBP &&
                                                                             p.TG != null &&
                                                                             p.TG >= startCompare &&
                                                                             p.TG <= endCompare &&
                                                                             p.MUCDO == mucdo)
                                                                 .Count(); // Đếm kết quả đánh giá với mức độ đánh giá tương ứng trong khoảng thời gian đang xét
                                    arr[mucdo - 1] = count; // Gán giá trị vào các phần tử trong mảng tương ứng với các mức độ đánh giá
                                }
                                string date = i + "/" + start.Year;
                                SoSanhKetQuaDanhGia md = new SoSanhKetQuaDanhGia() // Tạo đối tượng chứa năm và kết quả đánh giá trung bình tương ứng
                                {
                                    name = date,
                                    data = arr,
                                };
                                listMD.Add(md);
                            }
                        }
                        else // Nếu khoảng thời gian khác năm
                        {
                            for (int i = start.Year; i <= end.Year; i++)
                            {
                                if (i < end.Year) // Nếu năm hiện tại chưa bằng năm kết thúc
                                {
                                    for (int j = start.Month; j < 13; j++)
                                    {
                                        // Lấy khoảng thời gian so sánh
                                        DateTime startCompare = new DateTime(start.Year, j, 1, 0, 0, 0); // Ngày bắt đầu (Đầu tháng)
                                        DateTime endCompare = new DateTime(start.Year, j, DateTime.DaysInMonth(start.Year, j), 23, 59, 59); // Ngày kết thúc (Cuối tháng)
                                        double[] arr = new double[lstMucDo.Count]; // Tạo mảng chứa dữ liệu kết quả đánh giá trung bình theo khoảng thời gian đang xét
                                        foreach (var item in lstMucDo) // Mỗi tháng sẽ có tổng các mức độ đánh giá
                                        {
                                            var mucdo = item.MUCDO;
                                            var count = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.CANBO.MABP == _MaBP &&
                                                                                     p.TG != null &&
                                                                                     p.TG >= startCompare &&
                                                                                     p.TG <= endCompare &&
                                                                                     p.MUCDO == mucdo)
                                                                         .Count(); // Đếm kết quả đánh giá với mức độ đánh giá tương ứng trong khoảng thời gian đang xét
                                            arr[mucdo - 1] = count; // Gán giá trị vào các phần tử trong mảng tương ứng với các mức độ đánh giá
                                        }
                                        string date = j + "/" + i;
                                        SoSanhKetQuaDanhGia md = new SoSanhKetQuaDanhGia() // Tạo đối tượng chứa năm và kết quả đánh giá trung bình tương ứng
                                        {
                                            name = date,
                                            data = arr,
                                        };
                                        listMD.Add(md);
                                    }
                                }
                                if (i == end.Year)// Nếu năm hiện tại bằng năm kết thúc
                                {
                                    for (int j = 1; j <= end.Month; j++)
                                    {
                                        // Lấy khoảng thời gian so sánh
                                        DateTime startCompare = new DateTime(end.Year, j, 1, 0, 0, 0); // Ngày bắt đầu (Đầu tháng)
                                        DateTime endCompare = new DateTime(end.Year, j, DateTime.DaysInMonth(end.Year, j), 23, 59, 59); // Ngày kết thúc (Cuối tháng)
                                        double[] arr = new double[lstMucDo.Count]; // Tạo mảng chứa dữ liệu kết quả đánh giá trung bình theo khoảng thời gian đang xét
                                        foreach (var item in lstMucDo) // Mỗi tháng sẽ có tổng các mức độ đánh giá
                                        {
                                            var mucdo = item.MUCDO;
                                            var count = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.CANBO.MABP == _MaBP &&
                                                                                     p.TG != null &&
                                                                                     p.TG >= startCompare &&
                                                                                     p.TG <= endCompare &&
                                                                                     p.MUCDO == mucdo)
                                                                         .Count(); // Đếm kết quả đánh giá với mức độ đánh giá tương ứng trong khoảng thời gian đang xét
                                            arr[mucdo - 1] = count; // Gán giá trị vào các phần tử trong mảng tương ứng với các mức độ đánh giá
                                        }
                                        string date = j + "/" + i;
                                        SoSanhKetQuaDanhGia md = new SoSanhKetQuaDanhGia() // Tạo đối tượng chứa năm và kết quả đánh giá trung bình tương ứng
                                        {
                                            name = date,
                                            data = arr,
                                        };
                                        listMD.Add(md);
                                    }
                                }
                            }
                        }
                    }
                }
                else // Nếu mã cán bộ khác 0 thì so sánh theo cán bộ
                {
                    DateTime start = GetStartDateTime(_Start); // Lấy thời gian bắt đầu
                    DateTime end = GetEndDateTime(_End); // Lấy thời gian kết thúc
                    var lstMucDo = db.MUCDODANHGIAs.ToList(); // Lấy mức độ đánh giá
                    if (_LoaiThoiGian == "nam") // Nếu loại thời gian cần so sánh là năm
                    {
                        for (int i = start.Year; i <= end.Year; i++) // Vòng lặp từ năm bắt đầu đến năm kết thúc
                        {
                            // Lấy khoảng thời gian so sánh
                            DateTime startCompare = new DateTime(i, 1, 1, 0, 0, 0); // Ngày bắt đầu (Đầu năm)
                            DateTime endCompare = new DateTime(i, 12, DateTime.DaysInMonth(i, 12), 23, 59, 59); // Ngày kết thúc (Cuối năm)
                            double[] arr = new double[lstMucDo.Count]; // Tạo mảng chứa dữ liệu kết quả đánh giá trung bình theo khoảng thời gian đang xét
                            foreach (var item in lstMucDo) // Mỗi năm sẽ gồm có tổng các mức độ đánh giá
                            {
                                var mucdo = item.MUCDO;
                                var count = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == _MaCB &&
                                                                         p.TG != null &&
                                                                         p.TG >= startCompare &&
                                                                         p.TG <= endCompare &&
                                                                         p.MUCDO == mucdo)
                                                             .Count(); // Đếm kết quả đánh giá với mức độ đánh giá tương ứng trong khoảng thời gian đang xét
                                arr[mucdo - 1] = count; // Gán giá trị vào các phần tử trong mảng tương ứng với các mức độ đánh giá
                            }
                            SoSanhKetQuaDanhGia md = new SoSanhKetQuaDanhGia() // Tạo đối tượng chứa năm và kết quả đánh giá trung bình tương ứng
                            {
                                name = i.ToString(),
                                data = arr,
                            };
                            listMD.Add(md);
                        }
                    }
                    else if (_LoaiThoiGian == "thang") // Nếu loại thời gian cần so sánh là tháng
                    {
                        if (start.Year == end.Year) // Nếu khoảng thời gian nằm trong cùng 1 năm
                        {
                            for (int i = start.Month; i <= end.Month; i++)
                            {
                                // Lấy khoảng thời gian so sánh
                                DateTime startCompare = new DateTime(start.Year, i, 1, 0, 0, 0); // Ngày bắt đầu (Đầu tháng)
                                DateTime endCompare = new DateTime(start.Year, i, DateTime.DaysInMonth(start.Year, i), 23, 59, 59); // Ngày kết thúc (Cuối tháng)
                                double[] arr = new double[lstMucDo.Count]; // Tạo mảng chứa dữ liệu kết quả đánh giá trung bình theo khoảng thời gian đang xét
                                foreach (var item in lstMucDo) // Mỗi tháng sẽ có tổng các mức độ đánh giá
                                {
                                    var mucdo = item.MUCDO;
                                    var count = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == _MaCB &&
                                                                             p.TG != null &&
                                                                             p.TG >= startCompare &&
                                                                             p.TG <= endCompare &&
                                                                             p.MUCDO == mucdo)
                                                                 .Count(); // Đếm kết quả đánh giá với mức độ đánh giá tương ứng trong khoảng thời gian đang xét
                                    arr[mucdo - 1] = count; // Gán giá trị vào các phần tử trong mảng tương ứng với các mức độ đánh giá
                                }
                                string date = i + "/" + start.Year;
                                SoSanhKetQuaDanhGia md = new SoSanhKetQuaDanhGia() // Tạo đối tượng chứa năm và kết quả đánh giá trung bình tương ứng
                                {
                                    name = date,
                                    data = arr,
                                };
                                listMD.Add(md);
                            }
                        }
                        else // Nếu khoảng thời gian khác năm
                        {
                            for (int i = start.Year; i <= end.Year; i++)
                            {
                                if (i < end.Year) // Nếu năm hiện tại chưa bằng năm kết thúc
                                {
                                    for (int j = start.Month; j < 13; j++)
                                    {
                                        // Lấy khoảng thời gian so sánh
                                        DateTime startCompare = new DateTime(start.Year, j, 1, 0, 0, 0); // Ngày bắt đầu (Đầu tháng)
                                        DateTime endCompare = new DateTime(start.Year, j, DateTime.DaysInMonth(start.Year, j), 23, 59, 59); // Ngày kết thúc (Cuối tháng)
                                        double[] arr = new double[lstMucDo.Count]; // Tạo mảng chứa dữ liệu kết quả đánh giá trung bình theo khoảng thời gian đang xét
                                        foreach (var item in lstMucDo) // Mỗi tháng sẽ có tổng các mức độ đánh giá
                                        {
                                            var mucdo = item.MUCDO;
                                            var count = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == _MaCB &&
                                                                                     p.TG != null &&
                                                                                     p.TG >= startCompare &&
                                                                                     p.TG <= endCompare &&
                                                                                     p.MUCDO == mucdo)
                                                                         .Count(); // Đếm kết quả đánh giá với mức độ đánh giá tương ứng trong khoảng thời gian đang xét
                                            arr[mucdo - 1] = count; // Gán giá trị vào các phần tử trong mảng tương ứng với các mức độ đánh giá
                                        }
                                        string date = j + "/" + i;
                                        SoSanhKetQuaDanhGia md = new SoSanhKetQuaDanhGia() // Tạo đối tượng chứa năm và kết quả đánh giá trung bình tương ứng
                                        {
                                            name = date,
                                            data = arr,
                                        };
                                        listMD.Add(md);
                                    }
                                }
                                if (i == end.Year) // Nếu năm hiện tại bằng năm kết thúc
                                {
                                    for (int j = 1; j <= end.Month; j++)
                                    {
                                        // Lấy khoảng thời gian so sánh
                                        DateTime startCompare = new DateTime(end.Year, j, 1, 0, 0, 0); // Ngày bắt đầu (Đầu tháng)
                                        DateTime endCompare = new DateTime(end.Year, j, DateTime.DaysInMonth(end.Year, j), 23, 59, 59); // Ngày kết thúc (Cuối tháng)
                                        double[] arr = new double[lstMucDo.Count]; // Tạo mảng chứa dữ liệu kết quả đánh giá trung bình theo khoảng thời gian đang xét
                                        foreach (var item in lstMucDo) // Mỗi tháng sẽ có tổng các mức độ đánh giá
                                        {
                                            var mucdo = item.MUCDO;
                                            var count = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == _MaCB &&
                                                                                     p.TG != null &&
                                                                                     p.TG >= startCompare &&
                                                                                     p.TG <= endCompare &&
                                                                                     p.MUCDO == mucdo)
                                                                         .Count(); // Đếm kết quả đánh giá với mức độ đánh giá tương ứng trong khoảng thời gian đang xét
                                            arr[mucdo - 1] = count; // Gán giá trị vào các phần tử trong mảng tương ứng với các mức độ đánh giá
                                        }
                                        string date = j + "/" + i;
                                        SoSanhKetQuaDanhGia md = new SoSanhKetQuaDanhGia() // Tạo đối tượng chứa năm và kết quả đánh giá trung bình tương ứng
                                        {
                                            name = date,
                                            data = arr,
                                        };
                                        listMD.Add(md);
                                    }
                                }
                            }
                        }
                    }

                }
            }
            return listMD;
        }
        /// <summary>
        /// Phương thức so sánh thời gian giải quyết thủ tục
        /// </summary>
        /// <param name="_MaBP">Mã bộ phận</param>
        /// <param name="_MaCB">Mã cán bộ</param>
        /// <param name="_Start">Thời gian bắt đầu</param>
        /// <param name="_End">Thời gian kết thúc</param>
        /// <param name="_LoaiThoiGian">Loại thời gian</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<SoSanhKetQuaDanhGia> GetThoiGianSoSanh(int _MaBP, int _MaCB, string _Start, string _End, string _LoaiThoiGian)
        {
            IList<SoSanhKetQuaDanhGia> listMD = new List<SoSanhKetQuaDanhGia>();
            using (HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities())
            {
                if (_MaCB == 0) // Nếu mã cán bộ bằng 0 thì so sánh bộ phận
                {
                    DateTime start = GetStartDateTime(_Start); // Lấy thời gian bắt đầu
                    DateTime end = GetEndDateTime(_End); // Lấy thời gian kết thúc
                    if (_LoaiThoiGian == "nam") // Nếu loại thời gian cần so sánh là năm
                    {
                        for (int i = start.Year; i <= end.Year; i++) // Vòng lặp từ năm bắt đầu đến năm kết thúc
                        {
                            // Lấy khoảng thời gian so sánh
                            DateTime startCompare = new DateTime(i, 1, 1, 0, 0, 0); // Ngày bắt đầu (Đầu năm)
                            DateTime endCompare = new DateTime(i, 12, DateTime.DaysInMonth(i, 12), 23, 59, 59); // Ngày kết thúc (Cuối năm)
                            double[] arr = new double[4]; // Tạo mảng chứa dữ liệu kết quả đánh giá trung bình theo khoảng thời gian đang xét
                            var lstSttEF = db.KETQUADANHGIAs.Where(p => p.TG >= startCompare &&
                                                                        p.TG <= endCompare &&
                                                                        p.SOTHUTU.CANBO.MABP == _MaBP)
                                                            .ToList(); // Tìm tất cả kết quả đánh giá trong khoảng thời gian đang xét
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
                                int stt = (int)item.SOTHUTU.STT;
                                int mastt = (int)item.MASTT;

                                var rutso = db.SOTOIDAs.Where(p => p.STTTD == stt &&
                                                                   p.MABP == _MaBP &&
                                                                   p.TG >= now &&
                                                                   p.TG <= endNow)
                                                       .Select(p => p.TG)
                                                       .FirstOrDefault(); // Tìm thời gian rút số
                                var goiso = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                                       .Select(p => p.BD)
                                                       .FirstOrDefault(); // Tìm thời gian gọi số
                                var hoantat = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                                         .Select(p => p.KT)
                                                         .FirstOrDefault(); // Tìm thời gian hoàn tất
                                if (rutso != null && goiso != null && hoantat != null)
                                {
                                    DateTime tg_rut = (DateTime)rutso;
                                    DateTime tg_goi = (DateTime)goiso;
                                    DateTime tg_hoan_tat = (DateTime)hoantat;
                                    double phien_cho = Math.Abs(((TimeSpan)(tg_rut - tg_goi)).TotalMinutes);
                                    double phien_xu_ly = Math.Abs(((TimeSpan)(tg_goi - tg_hoan_tat)).TotalMinutes);
                                    average_cho += phien_cho;
                                    average_xuly += phien_xu_ly;
                                    so_luong_giai_quyet++;
                                }
                            }
                            if (so_luong_giai_quyet > 0)
                            {
                                average_cho = Math.Round(average_cho / so_luong_giai_quyet, 0);
                                average_xuly = Math.Round(average_xuly / so_luong_giai_quyet, 0);
                                average_tong = average_cho + average_xuly;
                            }
                            arr[0] = average_cho;
                            arr[1] = average_xuly;
                            arr[2] = average_tong;
                            arr[3] = so_luong_giai_quyet;
                            SoSanhKetQuaDanhGia md = new SoSanhKetQuaDanhGia()
                            {
                                name = i.ToString(),
                                data = arr
                            };
                            listMD.Add(md);
                        }
                    }
                    else if (_LoaiThoiGian == "thang") // Nếu loại thời gian cần so sánh là tháng
                    {
                        if (start.Year == end.Year) // Nếu khoảng thời gian nằm trong cùng 1 năm
                        {
                            for (int i = start.Month; i <= end.Month; i++)
                            {
                                // Lấy khoảng thời gian so sánh
                                DateTime startCompare = new DateTime(start.Year, i, 1, 0, 0, 0); // Ngày bắt đầu (Đầu tháng)
                                DateTime endCompare = new DateTime(start.Year, i, DateTime.DaysInMonth(start.Year, i), 23, 59, 59); // Ngày kết thúc (Cuối tháng)
                                double[] arr = new double[4]; // Tạo mảng chứa dữ liệu kết quả đánh giá trung bình theo khoảng thời gian đang xét
                                var lstSttEF = db.KETQUADANHGIAs.Where(p => p.TG >= startCompare &&
                                                                            p.TG <= endCompare &&
                                                                            p.SOTHUTU.CANBO.MABP == _MaBP)
                                                                .ToList(); // Tìm tất cả kết quả đánh giá trong khoảng thời gian đang xét
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
                                    int stt = (int)item.SOTHUTU.STT;
                                    int mastt = (int)item.MASTT;

                                    var rutso = db.SOTOIDAs.Where(p => p.STTTD == stt &&
                                                                       p.MABP == _MaBP &&
                                                                       p.TG >= now &&
                                                                       p.TG <= endNow)
                                                           .Select(p => p.TG)
                                                           .FirstOrDefault(); // Tìm thời gian rút số
                                    var goiso = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                                           .Select(p => p.BD)
                                                           .FirstOrDefault(); // Tìm thời gian gọi số
                                    var hoantat = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                                             .Select(p => p.KT)
                                                             .FirstOrDefault(); // Tìm thời gian hoàn tất
                                    if (rutso != null && goiso != null && hoantat != null)
                                    {
                                        DateTime tg_rut = (DateTime)rutso;
                                        DateTime tg_goi = (DateTime)goiso;
                                        DateTime tg_hoan_tat = (DateTime)hoantat;
                                        double phien_cho = Math.Abs(((TimeSpan)(tg_rut - tg_goi)).TotalMinutes);
                                        double phien_xu_ly = Math.Abs(((TimeSpan)(tg_goi - tg_hoan_tat)).TotalMinutes);
                                        average_cho += phien_cho;
                                        average_xuly += phien_xu_ly;
                                        so_luong_giai_quyet++;
                                    }
                                }
                                if (so_luong_giai_quyet > 0)
                                {
                                    average_cho = Math.Round(average_cho / so_luong_giai_quyet, 0);
                                    average_xuly = Math.Round(average_xuly / so_luong_giai_quyet, 0);
                                    average_tong = average_cho + average_xuly;
                                }
                                arr[0] = average_cho;
                                arr[1] = average_xuly;
                                arr[2] = average_tong;
                                arr[3] = so_luong_giai_quyet;
                                SoSanhKetQuaDanhGia md = new SoSanhKetQuaDanhGia()
                                {
                                    name = i + "/" + start.Year,
                                    data = arr
                                };
                                listMD.Add(md);
                            }
                        }
                        else // Nếu khoảng thời gian khác năm
                        {
                            for (int i = start.Year; i <= end.Year; i++)
                            {
                                if (i < end.Year) // Nếu năm hiện tại chưa bằng năm kết thúc
                                {
                                    for (int j = start.Month; j < 13; j++)
                                    {
                                        // Lấy khoảng thời gian so sánh
                                        DateTime startCompare = new DateTime(start.Year, j, 1, 0, 0, 0); // Ngày bắt đầu (Đầu tháng)
                                        DateTime endCompare = new DateTime(start.Year, j, DateTime.DaysInMonth(start.Year, j), 23, 59, 59); // Ngày kết thúc (Cuối tháng)
                                        double[] arr = new double[4]; // Tạo mảng chứa dữ liệu kết quả đánh giá trung bình theo khoảng thời gian đang xét
                                        var lstSttEF = db.KETQUADANHGIAs.Where(p => p.TG >= startCompare &&
                                                                                    p.TG <= endCompare &&
                                                                                    p.SOTHUTU.CANBO.MABP == _MaBP)
                                                                        .ToList(); // Tìm tất cả kết quả đánh giá trong khoảng thời gian đang xét
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
                                            int stt = (int)item.SOTHUTU.STT;
                                            int mastt = (int)item.MASTT;

                                            var rutso = db.SOTOIDAs.Where(p => p.STTTD == stt &&
                                                                               p.MABP == _MaBP &&
                                                                               p.TG >= now &&
                                                                               p.TG <= endNow)
                                                                   .Select(p => p.TG)
                                                                   .FirstOrDefault(); // Tìm thời gian rút số
                                            var goiso = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                                                   .Select(p => p.BD)
                                                                   .FirstOrDefault(); // Tìm thời gian gọi số
                                            var hoantat = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                                                     .Select(p => p.KT)
                                                                     .FirstOrDefault(); // Tìm thời gian hoàn tất
                                            if (rutso != null && goiso != null && hoantat != null)
                                            {
                                                DateTime tg_rut = (DateTime)rutso;
                                                DateTime tg_goi = (DateTime)goiso;
                                                DateTime tg_hoan_tat = (DateTime)hoantat;
                                                double phien_cho = Math.Abs(((TimeSpan)(tg_rut - tg_goi)).TotalMinutes);
                                                double phien_xu_ly = Math.Abs(((TimeSpan)(tg_goi - tg_hoan_tat)).TotalMinutes);
                                                average_cho += phien_cho;
                                                average_xuly += phien_xu_ly;
                                                so_luong_giai_quyet++;
                                            }
                                        }
                                        if (so_luong_giai_quyet > 0)
                                        {
                                            average_cho = Math.Round(average_cho / so_luong_giai_quyet, 0);
                                            average_xuly = Math.Round(average_xuly / so_luong_giai_quyet, 0);
                                            average_tong = average_cho + average_xuly;
                                        }
                                        arr[0] = average_cho;
                                        arr[1] = average_xuly;
                                        arr[2] = average_tong;
                                        arr[3] = so_luong_giai_quyet;
                                        SoSanhKetQuaDanhGia md = new SoSanhKetQuaDanhGia()
                                        {
                                            name = j + "/" + i,
                                            data = arr
                                        };
                                        listMD.Add(md);
                                    }
                                }
                                if (i == end.Year) // Nếu năm hiện tại bằng năm kết thúc
                                {
                                    for (int j = 1; j <= end.Month; j++)
                                    {
                                        // Lấy khoảng thời gian so sánh
                                        DateTime startCompare = new DateTime(end.Year, j, 1, 0, 0, 0); // Ngày bắt đầu (Đầu tháng)
                                        DateTime endCompare = new DateTime(end.Year, j, DateTime.DaysInMonth(end.Year, j), 23, 59, 59); // Ngày kết thúc (Cuối tháng)
                                        double[] arr = new double[4]; // Tạo mảng chứa dữ liệu kết quả đánh giá trung bình theo khoảng thời gian đang xét
                                        var lstSttEF = db.KETQUADANHGIAs.Where(p => p.TG >= startCompare &&
                                                                                    p.TG <= endCompare &&
                                                                                    p.SOTHUTU.CANBO.MABP == _MaBP)
                                                                        .ToList(); // Tìm tất cả kết quả đánh giá trong khoảng thời gian đang xét
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
                                            int stt = (int)item.SOTHUTU.STT;
                                            int mastt = (int)item.MASTT;

                                            var rutso = db.SOTOIDAs.Where(p => p.STTTD == stt &&
                                                                               p.MABP == _MaBP &&
                                                                               p.TG >= now &&
                                                                               p.TG <= endNow)
                                                                   .Select(p => p.TG)
                                                                   .FirstOrDefault(); // Tìm thời gian rút số
                                            var goiso = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                                                   .Select(p => p.BD)
                                                                   .FirstOrDefault(); // Tìm thời gian gọi số
                                            var hoantat = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                                                     .Select(p => p.KT)
                                                                     .FirstOrDefault(); // Tìm thời gian hoàn tất
                                            if (rutso != null && goiso != null && hoantat != null)
                                            {
                                                DateTime tg_rut = (DateTime)rutso;
                                                DateTime tg_goi = (DateTime)goiso;
                                                DateTime tg_hoan_tat = (DateTime)hoantat;
                                                double phien_cho = Math.Abs(((TimeSpan)(tg_rut - tg_goi)).TotalMinutes);
                                                double phien_xu_ly = Math.Abs(((TimeSpan)(tg_goi - tg_hoan_tat)).TotalMinutes);
                                                average_cho += phien_cho;
                                                average_xuly += phien_xu_ly;
                                                so_luong_giai_quyet++;
                                            }
                                        }
                                        if (so_luong_giai_quyet > 0)
                                        {
                                            average_cho = Math.Round(average_cho / so_luong_giai_quyet, 0);
                                            average_xuly = Math.Round(average_xuly / so_luong_giai_quyet, 0);
                                            average_tong = average_cho + average_xuly;
                                        }
                                        arr[0] = average_cho;
                                        arr[1] = average_xuly;
                                        arr[2] = average_tong;
                                        arr[3] = so_luong_giai_quyet;
                                        SoSanhKetQuaDanhGia md = new SoSanhKetQuaDanhGia()
                                        {
                                            name = j + "/" + i,
                                            data = arr
                                        };
                                        listMD.Add(md);
                                    }
                                }
                            }
                        }
                    }
                }
                else // Nếu mã cán bộ khác 0 thì so sánh theo cán bộ
                {
                    DateTime start = GetStartDateTime(_Start); // Lấy thời gian bắt đầu
                    DateTime end = GetEndDateTime(_End); // Lấy thời gian kết thúc
                    var lstMucDo = db.MUCDODANHGIAs.ToList(); // Lấy mức độ đánh giá
                    if (_LoaiThoiGian == "nam") // Nếu loại thời gian cần so sánh là năm
                    {
                        for (int i = start.Year; i <= end.Year; i++) // Vòng lặp từ năm bắt đầu đến năm kết thúc
                        {
                            // Lấy khoảng thời gian so sánh
                            DateTime startCompare = new DateTime(i, 1, 1, 0, 0, 0); // Ngày bắt đầu (Đầu năm)
                            DateTime endCompare = new DateTime(i, 12, DateTime.DaysInMonth(i, 12), 23, 59, 59); // Ngày kết thúc (Cuối năm)
                            double[] arr = new double[2]; // Tạo mảng chứa dữ liệu kết quả đánh giá trung bình theo khoảng thời gian đang xét
                            var lstSttEF = db.KETQUADANHGIAs.Where(p => p.TG >= startCompare &&
                                                                        p.TG <= endCompare &&
                                                                        p.SOTHUTU.MACB == _MaCB)
                                                            .ToList(); // Tìm tất cả kết quả đánh giá trong khoảng thời gian đang xét
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
                                int stt = (int)item.SOTHUTU.STT;
                                int mastt = (int)item.MASTT;

                                var rutso = db.SOTOIDAs.Where(p => p.STTTD == stt &&
                                                                   p.MABP == _MaBP &&
                                                                   p.TG >= now &&
                                                                   p.TG <= endNow)
                                                       .Select(p => p.TG)
                                                       .FirstOrDefault(); // Tìm thời gian rút số
                                var goiso = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                                       .Select(p => p.BD)
                                                       .FirstOrDefault(); // Tìm thời gian gọi số
                                var hoantat = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                                         .Select(p => p.KT)
                                                         .FirstOrDefault(); // Tìm thời gian hoàn tất
                                if (rutso != null && goiso != null && hoantat != null)
                                {
                                    DateTime tg_rut = (DateTime)rutso;
                                    DateTime tg_goi = (DateTime)goiso;
                                    DateTime tg_hoan_tat = (DateTime)hoantat;
                                    double phien_cho = Math.Abs(((TimeSpan)(tg_rut - tg_goi)).TotalMinutes);
                                    double phien_xu_ly = Math.Abs(((TimeSpan)(tg_goi - tg_hoan_tat)).TotalMinutes);
                                    average_xuly += phien_xu_ly;
                                    so_luong_giai_quyet++;
                                }
                            }
                            if (so_luong_giai_quyet > 0)
                            {
                                average_cho = Math.Round(average_cho / so_luong_giai_quyet, 0);
                                average_xuly = Math.Round(average_xuly / so_luong_giai_quyet, 0);
                                average_tong = average_cho + average_xuly;
                            }
                            arr[0] = average_xuly;
                            arr[1] = so_luong_giai_quyet;
                            SoSanhKetQuaDanhGia md = new SoSanhKetQuaDanhGia()
                            {
                                name = i.ToString(),
                                data = arr
                            };
                            listMD.Add(md);
                        }
                    }
                    else if (_LoaiThoiGian == "thang") // Nếu loại thời gian cần so sánh là tháng
                    {
                        if (start.Year == end.Year) // Nếu khoảng thời gian nằm trong cùng 1 năm
                        {
                            for (int i = start.Month; i <= end.Month; i++)
                            {
                                // Lấy khoảng thời gian so sánh
                                DateTime startCompare = new DateTime(start.Year, i, 1, 0, 0, 0); // Ngày bắt đầu (Đầu tháng)
                                DateTime endCompare = new DateTime(start.Year, i, DateTime.DaysInMonth(start.Year, i), 23, 59, 59); // Ngày kết thúc (Cuối tháng)
                                double[] arr = new double[2]; // Tạo mảng chứa dữ liệu kết quả đánh giá trung bình theo khoảng thời gian đang xét
                                var lstSttEF = db.KETQUADANHGIAs.Where(p => p.TG >= startCompare &&
                                                                            p.TG <= endCompare &&
                                                                            p.SOTHUTU.MACB == _MaCB)
                                                                .ToList(); // Tìm tất cả kết quả đánh giá trong khoảng thời gian đang xét
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
                                    int stt = (int)item.SOTHUTU.STT;
                                    int mastt = (int)item.MASTT;

                                    var rutso = db.SOTOIDAs.Where(p => p.STTTD == stt &&
                                                                       p.MABP == _MaBP &&
                                                                       p.TG >= now &&
                                                                       p.TG <= endNow)
                                                           .Select(p => p.TG)
                                                           .FirstOrDefault(); // Tìm thời gian rút số
                                    var goiso = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                                           .Select(p => p.BD)
                                                           .FirstOrDefault(); // Tìm thời gian gọi số
                                    var hoantat = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                                             .Select(p => p.KT)
                                                             .FirstOrDefault(); // Tìm thời gian hoàn tất
                                    if (rutso != null && goiso != null && hoantat != null)
                                    {
                                        DateTime tg_rut = (DateTime)rutso;
                                        DateTime tg_goi = (DateTime)goiso;
                                        DateTime tg_hoan_tat = (DateTime)hoantat;
                                        double phien_cho = Math.Abs(((TimeSpan)(tg_rut - tg_goi)).TotalMinutes);
                                        double phien_xu_ly = Math.Abs(((TimeSpan)(tg_goi - tg_hoan_tat)).TotalMinutes);
                                        average_xuly += phien_xu_ly;
                                        so_luong_giai_quyet++;
                                    }
                                }
                                if (so_luong_giai_quyet > 0)
                                {
                                    average_cho = Math.Round(average_cho / so_luong_giai_quyet, 0);
                                    average_xuly = Math.Round(average_xuly / so_luong_giai_quyet, 0);
                                    average_tong = average_cho + average_xuly;
                                }
                                arr[0] = average_xuly;
                                arr[1] = so_luong_giai_quyet;
                                SoSanhKetQuaDanhGia md = new SoSanhKetQuaDanhGia()
                                {
                                    name = i + "/" + start.Year,
                                    data = arr
                                };
                                listMD.Add(md);
                            }
                        }
                        else // Nếu khoảng thời gian khác năm
                        {
                            for (int i = start.Year; i <= end.Year; i++)
                            {
                                if (i < end.Year) // Nếu năm hiện tại chưa bằng năm kết thúc
                                {
                                    for (int j = start.Month; j < 13; j++)
                                    {
                                        // Lấy khoảng thời gian so sánh
                                        DateTime startCompare = new DateTime(start.Year, j, 1, 0, 0, 0); // Ngày bắt đầu (Đầu tháng)
                                        DateTime endCompare = new DateTime(start.Year, j, DateTime.DaysInMonth(start.Year, j), 23, 59, 59); // Ngày kết thúc (Cuối tháng)
                                        double[] arr = new double[2]; // Tạo mảng chứa dữ liệu kết quả đánh giá trung bình theo khoảng thời gian đang xét
                                        var lstSttEF = db.KETQUADANHGIAs.Where(p => p.TG >= startCompare &&
                                                                                    p.TG <= endCompare &&
                                                                                    p.SOTHUTU.MACB == _MaCB)
                                                                        .ToList(); // Tìm tất cả kết quả đánh giá trong khoảng thời gian đang xét
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
                                            int stt = (int)item.SOTHUTU.STT;
                                            int mastt = (int)item.MASTT;

                                            var rutso = db.SOTOIDAs.Where(p => p.STTTD == stt &&
                                                                               p.MABP == _MaBP &&
                                                                               p.TG >= now &&
                                                                               p.TG <= endNow)
                                                                   .Select(p => p.TG)
                                                                   .FirstOrDefault(); // Tìm thời gian rút số
                                            var goiso = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                                                   .Select(p => p.BD)
                                                                   .FirstOrDefault(); // Tìm thời gian gọi số
                                            var hoantat = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                                                     .Select(p => p.KT)
                                                                     .FirstOrDefault(); // Tìm thời gian hoàn tất
                                            if (rutso != null && goiso != null && hoantat != null)
                                            {
                                                DateTime tg_rut = (DateTime)rutso;
                                                DateTime tg_goi = (DateTime)goiso;
                                                DateTime tg_hoan_tat = (DateTime)hoantat;
                                                double phien_cho = Math.Abs(((TimeSpan)(tg_rut - tg_goi)).TotalMinutes);
                                                double phien_xu_ly = Math.Abs(((TimeSpan)(tg_goi - tg_hoan_tat)).TotalMinutes);
                                                average_xuly += phien_xu_ly;
                                                so_luong_giai_quyet++;
                                            }
                                        }
                                        if (so_luong_giai_quyet > 0)
                                        {
                                            average_cho = Math.Round(average_cho / so_luong_giai_quyet, 0);
                                            average_xuly = Math.Round(average_xuly / so_luong_giai_quyet, 0);
                                            average_tong = average_cho + average_xuly;
                                        }
                                        arr[0] = average_xuly;
                                        arr[1] = so_luong_giai_quyet;
                                        SoSanhKetQuaDanhGia md = new SoSanhKetQuaDanhGia()
                                        {
                                            name = j + "/" + i,
                                            data = arr
                                        };
                                        listMD.Add(md);
                                    }
                                }
                                if (i == end.Year) // Nếu năm hiện tại bằng năm kết thúc
                                {
                                    for (int j = 1; j <= end.Month; j++)
                                    {
                                        // Lấy khoảng thời gian so sánh
                                        DateTime startCompare = new DateTime(end.Year, j, 1, 0, 0, 0); // Ngày bắt đầu (Đầu tháng)
                                        DateTime endCompare = new DateTime(end.Year, j, DateTime.DaysInMonth(end.Year, j), 23, 59, 59); // Ngày kết thúc (Cuối tháng)
                                        double[] arr = new double[2]; // Tạo mảng chứa dữ liệu kết quả đánh giá trung bình theo khoảng thời gian đang xét
                                        var lstSttEF = db.KETQUADANHGIAs.Where(p => p.TG >= startCompare &&
                                                                                    p.TG <= endCompare &&
                                                                                    p.SOTHUTU.MACB == _MaCB)
                                                                        .ToList(); // Tìm tất cả kết quả đánh giá trong khoảng thời gian đang xét
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
                                            int stt = (int)item.SOTHUTU.STT;
                                            int mastt = (int)item.MASTT;

                                            var rutso = db.SOTOIDAs.Where(p => p.STTTD == stt &&
                                                                               p.MABP == _MaBP &&
                                                                               p.TG >= now &&
                                                                               p.TG <= endNow)
                                                                   .Select(p => p.TG)
                                                                   .FirstOrDefault(); // Tìm thời gian rút số
                                            var goiso = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                                                   .Select(p => p.BD)
                                                                   .FirstOrDefault(); // Tìm thời gian gọi số
                                            var hoantat = db.SOTHUTUs.Where(p => p.MASTT == mastt)
                                                                     .Select(p => p.KT)
                                                                     .FirstOrDefault(); // Tìm thời gian hoàn tất
                                            if (rutso != null && goiso != null && hoantat != null)
                                            {
                                                DateTime tg_rut = (DateTime)rutso;
                                                DateTime tg_goi = (DateTime)goiso;
                                                DateTime tg_hoan_tat = (DateTime)hoantat;
                                                double phien_cho = Math.Abs(((TimeSpan)(tg_rut - tg_goi)).TotalMinutes);
                                                double phien_xu_ly = Math.Abs(((TimeSpan)(tg_goi - tg_hoan_tat)).TotalMinutes);
                                                average_xuly += phien_xu_ly;
                                                so_luong_giai_quyet++;
                                            }
                                        }
                                        if (so_luong_giai_quyet > 0)
                                        {
                                            average_cho = Math.Round(average_cho / so_luong_giai_quyet, 0);
                                            average_xuly = Math.Round(average_xuly / so_luong_giai_quyet, 0);
                                            average_tong = average_cho + average_xuly;
                                        }
                                        arr[0] = average_xuly;
                                        arr[1] = so_luong_giai_quyet;
                                        SoSanhKetQuaDanhGia md = new SoSanhKetQuaDanhGia()
                                        {
                                            name = j + "/" + i,
                                            data = arr
                                        };
                                        listMD.Add(md);
                                    }
                                }
                            }
                        }
                    }

                }
            }
            return listMD;
        }
        /// <summary>
        /// Phương thức định dạng ngày bắt đầu
        /// </summary>
        /// <param name="_Start">Chuỗi thời gian bắt đầu</param>
        /// <returns></returns>
        public DateTime GetStartDateTime(string _Start)
        {
            string[] arrS = _Start.Split('/');
            DateTime start = new DateTime();
            if (arrS.Length == 3)
            {
                int ngayS = Convert.ToInt32(arrS[1]);
                int thangS = Convert.ToInt32(arrS[0]);
                int namS = Convert.ToInt32(arrS[2]);
                start = new DateTime(namS, thangS, ngayS, 0, 0, 0);
            }
            else if (arrS.Length == 2)
            {
                int thangS = Convert.ToInt32(arrS[0]);
                int namS = Convert.ToInt32(arrS[1]);
                start = new DateTime(namS, thangS, 1, 0, 0, 0);
            }
            else if (arrS.Length == 1)
            {
                int namS = Convert.ToInt32(arrS[0]);
                start = new DateTime(namS, 1, 1, 0, 0, 0);
            }
            return start;
        }
        /// <summary>
        /// Phương thức định dạng ngày kết thúc
        /// </summary>
        /// <param name="_End">Chuỗi thời gian kết thúc</param>
        /// <returns></returns>
        public DateTime GetEndDateTime(string _End)
        {
            string[] arrE = _End.Split('/');
            DateTime end = new DateTime();
            if (arrE.Length == 3)
            {
                int ngayE = Convert.ToInt32(arrE[1]);
                int thangE = Convert.ToInt32(arrE[0]);
                int namE = Convert.ToInt32(arrE[2]);
                end = new DateTime(namE, thangE, ngayE, 23, 59, 59);
            }
            else if (arrE.Length == 2)
            {
                int thangE = Convert.ToInt32(arrE[0]);
                int namE = Convert.ToInt32(arrE[1]);
                end = new DateTime(namE, thangE, DateTime.DaysInMonth(namE, thangE), 23, 59, 59);
            }
            else if (arrE.Length == 1)
            {
                int namE = Convert.ToInt32(arrE[0]);
                end = new DateTime(namE, 12, DateTime.DaysInMonth(namE, 12), 23, 59, 59);
            }
            return end;
        }
    }
}