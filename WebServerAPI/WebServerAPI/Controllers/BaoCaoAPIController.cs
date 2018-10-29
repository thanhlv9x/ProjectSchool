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
    public class BaoCaoAPIController : ApiController
    {
        HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities();

        /// <summary>
        /// Phương thức lấy kết quả đánh giá để báo cáo
        /// </summary>
        /// <param name="_MaBP">Mã bộ phận</param>
        /// <param name="_MaCB">Mã cán bộ</param>
        /// <param name="_Start">Thời gian bắt đầu</param>
        /// <param name="_End">Thời gian kết thúc</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<KetQuaDanhGia_BaoCao_> GetDanhGia(int _MaBP, int _MaCB, string _Start, string _End)
        {
            IList<KetQuaDanhGia_BaoCao_> listMD = new List<KetQuaDanhGia_BaoCao_>();

            if (_MaCB == 0)
            {
                List<CANBO> listEF = new List<CANBO>();
                if (_MaBP == 0)
                {
                    listEF = db.CANBOes.OrderBy(p => p.MABP).ToList();
                }
                else
                {
                    listEF = db.CANBOes.Where(p => p.MABP == _MaBP).OrderBy(p => p.MABP).ToList();
                }
                foreach (var item in listEF)
                {
                    int mabp = (int)item.MABP;
                    int macb = (int)item.MACB;
                    if (_Start == null && _End == null)
                    {
                        int count_RHL = db.KETQUADANHGIAs.Count(p => p.MUCDO == 1 && p.SOTHUTU.MACB == macb);
                        int total_RHL = db.KETQUADANHGIAs.Count(p => p.MUCDO == 1 && p.SOTHUTU.CANBO.MABP == mabp);
                        double tyle_RHL = Math.Round(((double)count_RHL / (double)total_RHL) * 100.00, 2);
                        int count_HL = db.KETQUADANHGIAs.Count(p => p.MUCDO == 2 && p.SOTHUTU.MACB == macb);
                        int total_HL = db.KETQUADANHGIAs.Count(p => p.MUCDO == 2 && p.SOTHUTU.CANBO.MABP == mabp);
                        double tyle_HL = Math.Round(((double)count_HL / (double)total_HL) * 100.00, 2);
                        int count_BT = db.KETQUADANHGIAs.Count(p => p.MUCDO == 3 && p.SOTHUTU.MACB == macb);
                        int total_BT = db.KETQUADANHGIAs.Count(p => p.MUCDO == 3 && p.SOTHUTU.CANBO.MABP == mabp);
                        double tyle_BT = Math.Round(((double)count_BT / (double)total_BT) * 100.00, 2);
                        int count_KHL = db.KETQUADANHGIAs.Count(p => p.MUCDO == 4 && p.SOTHUTU.MACB == macb);
                        int total_KHL = db.KETQUADANHGIAs.Count(p => p.MUCDO == 4 && p.SOTHUTU.CANBO.MABP == mabp);
                        double tyle_KHL = Math.Round(((double)count_KHL / (double)total_KHL) * 100.00, 2);
                        int total = total_RHL + total_HL + total_BT + total_KHL;
                        int count = count_RHL + count_HL + count_BT + count_KHL;
                        double tyle = Math.Round(((double)count / (double)total) * 100.00, 2);
                        int diem = count_RHL * 3 + count_HL * 2 + count_BT;
                        KetQuaDanhGia_BaoCao_ md = new KetQuaDanhGia_BaoCao_()
                        {
                            MaBP = mabp,
                            TenBP = item.BOPHAN.TENBP,
                            MaCB = macb,
                            HoTen = item.HOTEN,
                            RHL_SoLan = count_RHL,
                            HL_SoLan = count_HL,
                            BT_SoLan = count_BT,
                            KHL_SoLan = count_KHL,
                            TongCong_SoLan = count,
                            RHL_TyLe = tyle_RHL,
                            HL_TyLe = tyle_HL,
                            BT_TyLe = tyle_BT,
                            KHL_TyLe = tyle_KHL,
                            TongCong_TyLe = tyle,
                            Diem = diem
                        };
                        listMD.Add(md);
                    }
                    else if (_Start != null && _End != null)
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
                        else if (arrS.Length == 1)
                        {
                            int namE = Convert.ToInt32(arrE[0]);
                            end = new DateTime(namE, 12, DateTime.DaysInMonth(namE, 12), 23, 59, 59);
                        }

                        int count_RHL = db.KETQUADANHGIAs.Count(p => p.MUCDO == 1 && p.SOTHUTU.MACB == macb && p.TG >= start && p.TG <= end);
                        int total_RHL = db.KETQUADANHGIAs.Count(p => p.MUCDO == 1 && p.SOTHUTU.CANBO.MABP == mabp && p.TG >= start && p.TG <= end);
                        double tyle_RHL = Math.Round(((double)count_RHL / (double)total_RHL) * 100.00, 2);
                        int count_HL = db.KETQUADANHGIAs.Count(p => p.MUCDO == 2 && p.SOTHUTU.MACB == macb && p.TG >= start && p.TG <= end);
                        int total_HL = db.KETQUADANHGIAs.Count(p => p.MUCDO == 2 && p.SOTHUTU.CANBO.MABP == mabp && p.TG >= start && p.TG <= end);
                        double tyle_HL = Math.Round(((double)count_HL / (double)total_HL) * 100.00, 2);
                        int count_BT = db.KETQUADANHGIAs.Count(p => p.MUCDO == 3 && p.SOTHUTU.MACB == macb && p.TG >= start && p.TG <= end);
                        int total_BT = db.KETQUADANHGIAs.Count(p => p.MUCDO == 3 && p.SOTHUTU.CANBO.MABP == mabp && p.TG >= start && p.TG <= end);
                        double tyle_BT = Math.Round(((double)count_BT / (double)total_BT) * 100.00, 2);
                        int count_KHL = db.KETQUADANHGIAs.Count(p => p.MUCDO == 4 && p.SOTHUTU.MACB == macb && p.TG >= start && p.TG <= end);
                        int total_KHL = db.KETQUADANHGIAs.Count(p => p.MUCDO == 4 && p.SOTHUTU.CANBO.MABP == mabp && p.TG >= start && p.TG <= end);
                        double tyle_KHL = Math.Round(((double)count_KHL / (double)total_KHL) * 100.00, 2);
                        int total = total_RHL + total_HL + total_BT + total_KHL;
                        int count = count_RHL + count_HL + count_BT + count_KHL;
                        double tyle = Math.Round(((double)count / (double)total) * 100.00, 2);
                        int diem = count_RHL * 3 + count_HL * 2 + count_BT;
                        KetQuaDanhGia_BaoCao_ md = new KetQuaDanhGia_BaoCao_()
                        {
                            MaBP = mabp,
                            TenBP = item.BOPHAN.TENBP,
                            MaCB = macb,
                            HoTen = item.HOTEN,
                            RHL_SoLan = count_RHL,
                            HL_SoLan = count_HL,
                            BT_SoLan = count_BT,
                            KHL_SoLan = count_KHL,
                            TongCong_SoLan = count,
                            RHL_TyLe = tyle_RHL,
                            HL_TyLe = tyle_HL,
                            BT_TyLe = tyle_BT,
                            KHL_TyLe = tyle_KHL,
                            TongCong_TyLe = tyle,
                            Diem = diem
                        };
                        listMD.Add(md);
                    }
                }
                return listMD;
            }
            else
            {
                string tenbp = db.BOPHANs.Where(p => p.MABP == _MaBP).Select(p => p.TENBP).SingleOrDefault();
                string hoten = db.CANBOes.Where(p => p.MACB == _MaCB).Select(p => p.HOTEN).SingleOrDefault();

                if (_Start == null && _End == null)
                {
                    int total = db.KETQUADANHGIAs.Count(p => p.SOTHUTU.CANBO.MACB == _MaCB);
                    int count_RHL = db.KETQUADANHGIAs.Count(p => p.MUCDO == 1 && p.SOTHUTU.MACB == _MaCB);
                    double tyle_RHL = Math.Round(((double)count_RHL / (double)total) * 100.00, 2);
                    int count_HL = db.KETQUADANHGIAs.Count(p => p.MUCDO == 2 && p.SOTHUTU.MACB == _MaCB);
                    double tyle_HL = Math.Round(((double)count_HL / (double)total) * 100.00, 2);
                    int count_BT = db.KETQUADANHGIAs.Count(p => p.MUCDO == 3 && p.SOTHUTU.MACB == _MaCB);
                    double tyle_BT = Math.Round(((double)count_BT / (double)total) * 100.00, 2);
                    int count_KHL = db.KETQUADANHGIAs.Count(p => p.MUCDO == 4 && p.SOTHUTU.MACB == _MaCB);
                    double tyle_KHL = Math.Round(((double)count_KHL / (double)total) * 100.00, 2);
                    int diem = count_RHL * 3 + count_HL * 2 + count_BT;
                    KetQuaDanhGia_BaoCao_ md = new KetQuaDanhGia_BaoCao_()
                    {
                        MaBP = _MaBP,
                        TenBP = tenbp,
                        MaCB = _MaCB,
                        HoTen = hoten,
                        RHL_SoLan = count_RHL,
                        HL_SoLan = count_HL,
                        BT_SoLan = count_BT,
                        KHL_SoLan = count_KHL,
                        TongCong_SoLan = count_RHL + count_HL + count_BT + count_KHL,
                        RHL_TyLe = tyle_RHL,
                        HL_TyLe = tyle_HL,
                        BT_TyLe = tyle_BT,
                        KHL_TyLe = tyle_KHL,
                        TongCong_TyLe = tyle_RHL + tyle_HL + tyle_BT + tyle_KHL,
                        Diem = diem
                    };
                    listMD.Add(md);
                }
                else if (_Start != null && _End != null)
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
                    else if (arrS.Length == 1)
                    {
                        int namE = Convert.ToInt32(arrE[0]);
                        end = new DateTime(namE, 12, DateTime.DaysInMonth(namE, 12), 23, 59, 59);
                    }

                    int total = db.KETQUADANHGIAs.Count(p => p.SOTHUTU.CANBO.MACB == _MaCB && p.TG >= start && p.TG <= end);
                    int count_RHL = db.KETQUADANHGIAs.Count(p => p.MUCDO == 1 && p.SOTHUTU.MACB == _MaCB && p.TG >= start && p.TG <= end);
                    double tyle_RHL = Math.Round(((double)count_RHL / (double)total) * 100.00, 2);
                    int count_HL = db.KETQUADANHGIAs.Count(p => p.MUCDO == 2 && p.SOTHUTU.MACB == _MaCB && p.TG >= start && p.TG <= end);
                    double tyle_HL = Math.Round(((double)count_HL / (double)total) * 100.00, 2);
                    int count_BT = db.KETQUADANHGIAs.Count(p => p.MUCDO == 3 && p.SOTHUTU.MACB == _MaCB && p.TG >= start && p.TG <= end);
                    double tyle_BT = Math.Round(((double)count_BT / (double)total) * 100.00, 2);
                    int count_KHL = db.KETQUADANHGIAs.Count(p => p.MUCDO == 4 && p.SOTHUTU.MACB == _MaCB && p.TG >= start && p.TG <= end);
                    double tyle_KHL = Math.Round(((double)count_KHL / (double)total) * 100.00, 2);
                    int diem = count_RHL * 3 + count_HL * 2 + count_BT;
                    KetQuaDanhGia_BaoCao_ md = new KetQuaDanhGia_BaoCao_()
                    {
                        MaBP = _MaBP,
                        TenBP = tenbp,
                        MaCB = _MaCB,
                        HoTen = hoten,
                        RHL_SoLan = count_RHL,
                        HL_SoLan = count_HL,
                        BT_SoLan = count_BT,
                        KHL_SoLan = count_KHL,
                        TongCong_SoLan = count_RHL + count_HL + count_BT + count_KHL,
                        RHL_TyLe = tyle_RHL,
                        HL_TyLe = tyle_HL,
                        BT_TyLe = tyle_BT,
                        KHL_TyLe = tyle_KHL,
                        TongCong_TyLe = tyle_RHL + tyle_HL + tyle_BT + tyle_KHL,
                        Diem = diem
                    };
                    listMD.Add(md);
                }
                return listMD;
            }
        }

        /// <summary>
        /// Phương thức lấy góp ý để báo cáo
        /// </summary>
        /// <param name="_MaBP">Mã bộ phận</param>
        /// <param name="_MaCB">Mã cán bộ</param>
        /// <param name="_Start">Thời gian bắt đầu</param>
        /// <param name="_End">Thời gian kết thúc</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<BangGopY_BaoCao_> GetGopY(int _MaBP, int _MaCB, string _Start, string _End, string _GopY)
        {
            IList<BangGopY_BaoCao_> listMD = new List<BangGopY_BaoCao_>();

            if (_MaCB == 0)
            {
                if (_MaBP == 0)
                {
                    if (_Start == null && _End == null)
                    {
                        var listEF = db.GOPies.OrderBy(p => p.KETQUADANHGIA.SOTHUTU.CANBO.MABP).ToList();
                        foreach (var item in listEF)
                        {
                            bool doub = false;
                            foreach (var itemMD in listMD)
                            {
                                if (itemMD.GopY == item.NOIDUNG &&
                                    itemMD.MucDoDanhGia == item.KETQUADANHGIA.MUCDODANHGIA.LOAI &&
                                    itemMD.MaCB == item.KETQUADANHGIA.SOTHUTU.MACB) doub = true;
                            }
                            if (!doub)
                            {
                                int mabp = (int)item.KETQUADANHGIA.SOTHUTU.CANBO.MABP;
                                string tenbp = item.KETQUADANHGIA.SOTHUTU.CANBO.BOPHAN.TENBP;
                                int macb = (int)item.KETQUADANHGIA.SOTHUTU.MACB;
                                string hoten = item.KETQUADANHGIA.SOTHUTU.CANBO.HOTEN;
                                int mucdo = (int)item.KETQUADANHGIA.MUCDO;
                                string mucdo_danhgia = item.KETQUADANHGIA.MUCDODANHGIA.LOAI;
                                string gopy = item.NOIDUNG;
                                int count = db.GOPies.Where(p => p.KETQUADANHGIA.SOTHUTU.MACB == macb &&
                                                                 p.NOIDUNG == gopy &&
                                                                 p.KETQUADANHGIA.MUCDO == mucdo)
                                                     .Count();

                                BangGopY_BaoCao_ md = new BangGopY_BaoCao_()
                                {
                                    MaBP = mabp,
                                    TenBP = tenbp,
                                    MaCB = macb,
                                    HoTen = hoten,
                                    MucDoDanhGia = mucdo_danhgia,
                                    GopY = gopy,
                                    SoLan = count
                                };
                                listMD.Add(md);
                            }
                        }
                    }
                    else if (_Start != null && _End != null)
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
                        else if (arrS.Length == 1)
                        {
                            int namE = Convert.ToInt32(arrE[0]);
                            end = new DateTime(namE, 12, DateTime.DaysInMonth(namE, 12), 23, 59, 59);
                        }

                        var listEF = db.GOPies.Where(p => p.KETQUADANHGIA.TG >= start && p.KETQUADANHGIA.TG <= end)
                                              .OrderBy(p => p.KETQUADANHGIA.SOTHUTU.CANBO.MABP)
                                              .ToList();
                        foreach (var item in listEF)
                        {
                            bool doub = false;
                            foreach (var itemMD in listMD)
                            {
                                if (itemMD.GopY == item.NOIDUNG &&
                                    itemMD.MucDoDanhGia == item.KETQUADANHGIA.MUCDODANHGIA.LOAI &&
                                    itemMD.MaCB == item.KETQUADANHGIA.SOTHUTU.MACB) doub = true;
                            }
                            if (!doub)
                            {
                                int mabp = (int)item.KETQUADANHGIA.SOTHUTU.CANBO.MABP;
                                string tenbp = item.KETQUADANHGIA.SOTHUTU.CANBO.BOPHAN.TENBP;
                                int macb = (int)item.KETQUADANHGIA.SOTHUTU.MACB;
                                string hoten = item.KETQUADANHGIA.SOTHUTU.CANBO.HOTEN;
                                int mucdo = (int)item.KETQUADANHGIA.MUCDO;
                                string mucdo_danhgia = item.KETQUADANHGIA.MUCDODANHGIA.LOAI;
                                string gopy = item.NOIDUNG;
                                int count = db.GOPies.Where(p => p.KETQUADANHGIA.SOTHUTU.MACB == macb &&
                                                                 p.KETQUADANHGIA.TG >= start &&
                                                                 p.KETQUADANHGIA.TG <= end &&
                                                                 p.NOIDUNG == gopy &&
                                                                 p.KETQUADANHGIA.MUCDO == mucdo)
                                                     .Count();

                                BangGopY_BaoCao_ md = new BangGopY_BaoCao_()
                                {
                                    MaBP = mabp,
                                    TenBP = tenbp,
                                    MaCB = macb,
                                    HoTen = hoten,
                                    MucDoDanhGia = mucdo_danhgia,
                                    GopY = gopy,
                                    SoLan = count
                                };
                                listMD.Add(md);
                            }
                        }
                    }
                }
                else
                {
                    if (_Start == null && _End == null)
                    {
                        var listEF = db.GOPies.Where(p => p.KETQUADANHGIA.SOTHUTU.CANBO.MABP == _MaBP)
                                              .OrderBy(p => p.KETQUADANHGIA.SOTHUTU.MACB)
                                              .ToList();
                        foreach (var item in listEF)
                        {
                            bool doub = false;
                            foreach (var itemMD in listMD)
                            {
                                if (itemMD.GopY == item.NOIDUNG &&
                                    itemMD.MucDoDanhGia == item.KETQUADANHGIA.MUCDODANHGIA.LOAI &&
                                    itemMD.MaCB == item.KETQUADANHGIA.SOTHUTU.MACB) doub = true;
                            }
                            if (!doub)
                            {
                                int mabp = (int)item.KETQUADANHGIA.SOTHUTU.CANBO.MABP;
                                string tenbp = item.KETQUADANHGIA.SOTHUTU.CANBO.BOPHAN.TENBP;
                                int macb = (int)item.KETQUADANHGIA.SOTHUTU.MACB;
                                string hoten = item.KETQUADANHGIA.SOTHUTU.CANBO.HOTEN;
                                int mucdo = (int)item.KETQUADANHGIA.MUCDO;
                                string mucdo_danhgia = item.KETQUADANHGIA.MUCDODANHGIA.LOAI;
                                string gopy = item.NOIDUNG;
                                int count = db.GOPies.Where(p => p.KETQUADANHGIA.SOTHUTU.MACB == macb &&
                                                                 p.NOIDUNG == gopy &&
                                                                 p.KETQUADANHGIA.MUCDO == mucdo)
                                                     .Count();

                                BangGopY_BaoCao_ md = new BangGopY_BaoCao_()
                                {
                                    MaBP = mabp,
                                    TenBP = tenbp,
                                    MaCB = macb,
                                    HoTen = hoten,
                                    MucDoDanhGia = mucdo_danhgia,
                                    GopY = gopy,
                                    SoLan = count
                                };
                                listMD.Add(md);
                            }
                        }
                    }
                    else if (_Start != null && _End != null)
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
                        else if (arrS.Length == 1)
                        {
                            int namE = Convert.ToInt32(arrE[0]);
                            end = new DateTime(namE, 12, DateTime.DaysInMonth(namE, 12), 23, 59, 59);
                        }

                        var listEF = db.GOPies.Where(p => p.KETQUADANHGIA.TG >= start &&
                                                          p.KETQUADANHGIA.TG <= end &&
                                                          p.KETQUADANHGIA.SOTHUTU.CANBO.MABP == _MaBP)
                                              .OrderBy(p => p.KETQUADANHGIA.SOTHUTU.MACB)
                                              .ToList();
                        foreach (var item in listEF)
                        {
                            bool doub = false;
                            foreach (var itemMD in listMD)
                            {
                                if (itemMD.GopY == item.NOIDUNG &&
                                    itemMD.MucDoDanhGia == item.KETQUADANHGIA.MUCDODANHGIA.LOAI &&
                                    itemMD.MaCB == item.KETQUADANHGIA.SOTHUTU.MACB) doub = true;
                            }
                            if (!doub)
                            {
                                int mabp = (int)item.KETQUADANHGIA.SOTHUTU.CANBO.MABP;
                                string tenbp = item.KETQUADANHGIA.SOTHUTU.CANBO.BOPHAN.TENBP;
                                int macb = (int)item.KETQUADANHGIA.SOTHUTU.MACB;
                                string hoten = item.KETQUADANHGIA.SOTHUTU.CANBO.HOTEN;
                                int mucdo = (int)item.KETQUADANHGIA.MUCDO;
                                string mucdo_danhgia = item.KETQUADANHGIA.MUCDODANHGIA.LOAI;
                                string gopy = item.NOIDUNG;
                                int count = db.GOPies.Where(p => p.KETQUADANHGIA.SOTHUTU.MACB == macb &&
                                                                 p.KETQUADANHGIA.TG >= start &&
                                                                 p.KETQUADANHGIA.TG <= end &&
                                                                 p.NOIDUNG == gopy &&
                                                                 p.KETQUADANHGIA.MUCDO == mucdo)
                                                     .Count();

                                BangGopY_BaoCao_ md = new BangGopY_BaoCao_()
                                {
                                    MaBP = mabp,
                                    TenBP = tenbp,
                                    MaCB = macb,
                                    HoTen = hoten,
                                    MucDoDanhGia = mucdo_danhgia,
                                    GopY = gopy,
                                    SoLan = count
                                };
                                listMD.Add(md);
                            }
                        }
                    }
                }
            }
            else
            {
                if (_Start == null && _End == null)
                {
                    var listEF = db.GOPies.Where(p => p.KETQUADANHGIA.SOTHUTU.MACB == _MaCB)
                                          .OrderBy(p => p.KETQUADANHGIA.MUCDO)
                                          .ToList();
                    foreach (var item in listEF)
                    {
                        bool doub = false;
                        foreach (var itemMD in listMD)
                        {
                            if (itemMD.GopY == item.NOIDUNG &&
                                itemMD.MucDoDanhGia == item.KETQUADANHGIA.MUCDODANHGIA.LOAI &&
                                itemMD.MaCB == item.KETQUADANHGIA.SOTHUTU.MACB) doub = true;
                        }
                        if (!doub)
                        {
                            int mabp = (int)item.KETQUADANHGIA.SOTHUTU.CANBO.MABP;
                            string tenbp = item.KETQUADANHGIA.SOTHUTU.CANBO.BOPHAN.TENBP;
                            int macb = (int)item.KETQUADANHGIA.SOTHUTU.MACB;
                            string hoten = item.KETQUADANHGIA.SOTHUTU.CANBO.HOTEN;
                            int mucdo = (int)item.KETQUADANHGIA.MUCDO;
                            string mucdo_danhgia = item.KETQUADANHGIA.MUCDODANHGIA.LOAI;
                            string gopy = item.NOIDUNG;
                            int count = db.GOPies.Where(p => p.KETQUADANHGIA.SOTHUTU.MACB == macb &&
                                                             p.NOIDUNG == gopy &&
                                                             p.KETQUADANHGIA.MUCDO == mucdo)
                                                 .Count();

                            BangGopY_BaoCao_ md = new BangGopY_BaoCao_()
                            {
                                MaBP = mabp,
                                TenBP = tenbp,
                                MaCB = macb,
                                HoTen = hoten,
                                MucDoDanhGia = mucdo_danhgia,
                                GopY = gopy,
                                SoLan = count
                            };
                            listMD.Add(md);
                        }
                    }
                }
                else if (_Start != null && _End != null)
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
                    else if (arrS.Length == 1)
                    {
                        int namE = Convert.ToInt32(arrE[0]);
                        end = new DateTime(namE, 12, DateTime.DaysInMonth(namE, 12), 23, 59, 59);
                    }

                    var listEF = db.GOPies.Where(p => p.KETQUADANHGIA.TG >= start &&
                                                      p.KETQUADANHGIA.TG <= end &&
                                                      p.KETQUADANHGIA.SOTHUTU.MACB == _MaCB)
                                          .OrderBy(p => p.KETQUADANHGIA.MUCDO)
                                          .ToList();
                    foreach (var item in listEF)
                    {
                        bool doub = false;
                        foreach (var itemMD in listMD)
                        {
                            if (itemMD.GopY == item.NOIDUNG &&
                                itemMD.MucDoDanhGia == item.KETQUADANHGIA.MUCDODANHGIA.LOAI &&
                                itemMD.MaCB == item.KETQUADANHGIA.SOTHUTU.MACB) doub = true;
                        }
                        if (!doub)
                        {
                            int mabp = (int)item.KETQUADANHGIA.SOTHUTU.CANBO.MABP;
                            string tenbp = item.KETQUADANHGIA.SOTHUTU.CANBO.BOPHAN.TENBP;
                            int macb = (int)item.KETQUADANHGIA.SOTHUTU.MACB;
                            string hoten = item.KETQUADANHGIA.SOTHUTU.CANBO.HOTEN;
                            int mucdo = (int)item.KETQUADANHGIA.MUCDO;
                            string mucdo_danhgia = item.KETQUADANHGIA.MUCDODANHGIA.LOAI;
                            string gopy = item.NOIDUNG;
                            int count = db.GOPies.Where(p => p.KETQUADANHGIA.SOTHUTU.MACB == macb &&
                                                             p.KETQUADANHGIA.TG >= start &&
                                                             p.KETQUADANHGIA.TG <= end &&
                                                             p.NOIDUNG == gopy &&
                                                             p.KETQUADANHGIA.MUCDO == mucdo)
                                                 .Count();

                            BangGopY_BaoCao_ md = new BangGopY_BaoCao_()
                            {
                                MaBP = mabp,
                                TenBP = tenbp,
                                MaCB = macb,
                                HoTen = hoten,
                                MucDoDanhGia = mucdo_danhgia,
                                GopY = gopy,
                                SoLan = count
                            };
                            listMD.Add(md);
                        }
                    }
                }
            }
            return listMD;
        }

        /// <summary>
        /// Phương thức lấy thủ tục để báo cáo
        /// </summary>
        /// <param name="_MaBP">Mã bộ phận</param>
        /// <param name="_MaCB">Mã cán bộ</param>
        /// <param name="_Start">Thời gian bắt đầu</param>
        /// <param name="_End">Thời gian kết thúc</param>
        /// <param name="_Phien">Tham số xác nhận phương thức</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<BangThuTuc_BaoCao_> GetPhien(int _MaBP, int _MaCB, string _Start, string _End, string _Phien)
        {
            IList<BangThuTuc_BaoCao_> listMD = new List<BangThuTuc_BaoCao_>();

            if (_MaCB == 0)
            {
                if (_MaBP == 0)
                {
                    if (_Start == null && _End == null)
                    {
                        // Lấy thời gian xử lý thủ tục của toàn bộ cán bộ trong tất cả thời gian
                        var listCB = db.CANBOes.OrderBy(p => p.MABP).ToList();
                        foreach (var itemCB in listCB)
                        {
                            var mabp = itemCB.MABP;
                            string tenbp = itemCB.BOPHAN.TENBP;
                            var macb = itemCB.MACB;
                            string hoten = itemCB.HOTEN;
                            var listEF = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == macb)
                                                          .OrderBy(p => p.MASTT)
                                                          .ToList();
                            double phiencho = 0;
                            double phienxuly = 0;
                            double tongphien = 0;
                            foreach (var item in listEF)
                            {
                                var mastt = item.MASTT;
                                var stt = item.SOTHUTU.STT;
                                DateTime start = new DateTime(((DateTime)item.TG).Year, ((DateTime)item.TG).Month, ((DateTime)item.TG).Day, 0, 0, 0);
                                DateTime end = new DateTime(((DateTime)item.TG).Year, ((DateTime)item.TG).Month, ((DateTime)item.TG).Day, 23, 59, 59);
                                var rut = db.SOTOIDAs.Where(p => p.MABP == mabp &&
                                                                 p.STTTD == stt &&
                                                                 p.TG >= start &&
                                                                 p.TG <= end)
                                                     .FirstOrDefault();
                                var goi = db.SOTHUTUs.Where(p => p.MASTT == mastt & p.BD != null & p.KT != null & p.BD != null & p.KT != null).FirstOrDefault();
                                phiencho += Math.Round(Math.Abs(((TimeSpan)(goi.BD - rut.TG)).TotalMinutes), 0);
                                phienxuly += Math.Round(Math.Abs(((TimeSpan)(goi.KT - goi.BD)).TotalMinutes), 0);
                                tongphien += phiencho + phienxuly;
                            }
                            BangThuTuc_BaoCao_ md = new BangThuTuc_BaoCao_()
                            {
                                MaBP = (int)mabp,
                                TenBP = tenbp,
                                MaCB = macb,
                                HoTen = hoten,
                                PhienCho = phiencho,
                                PhienXuLy = phienxuly,
                                TongPhien = tongphien
                            };
                            listMD.Add(md);
                        }
                    }
                    
                    else if (_Start != null && _End != null)
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
                        else if (arrS.Length == 1)
                        {
                            int namE = Convert.ToInt32(arrE[0]);
                            end = new DateTime(namE, 12, DateTime.DaysInMonth(namE, 12), 23, 59, 59);
                        }

                        // Lấy thời gian xử lý thủ tục của toàn bộ cán bộ trong khoảng thời gian nhất định
                        var listCB = db.CANBOes.OrderBy(p => p.MABP).ToList();
                        foreach (var itemCB in listCB)
                        {
                            var mabp = itemCB.MABP;
                            string tenbp = itemCB.BOPHAN.TENBP;
                            var macb = itemCB.MACB;
                            string hoten = itemCB.HOTEN;
                            var listEF = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == macb &&
                                                                      p.TG >= start &&
                                                                      p.TG <= end)
                                                          .OrderBy(p => p.MASTT)
                                                          .ToList();
                            double phiencho = 0;
                            double phienxuly = 0;
                            double tongphien = 0;
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
                                phiencho += Math.Round(Math.Abs(((TimeSpan)(goi.BD - rut.TG)).TotalMinutes), 0);
                                phienxuly += Math.Round(Math.Abs(((TimeSpan)(goi.KT - goi.BD)).TotalMinutes), 0);
                                tongphien += phiencho + phienxuly;
                            }
                            BangThuTuc_BaoCao_ md = new BangThuTuc_BaoCao_()
                            {
                                MaBP = (int)mabp,
                                TenBP = tenbp,
                                MaCB = macb,
                                HoTen = hoten,
                                PhienCho = phiencho,
                                PhienXuLy = phienxuly,
                                TongPhien = tongphien
                            };
                            listMD.Add(md);
                        }
                    }
                }
                else
                {
                    if (_Start == null && _End == null)
                    {
                        // Lấy thời gian xử lý thủ tục của toàn bộ cán bộ theo bộ phận trong tất cả thời gian
                        var listCB = db.CANBOes.Where(p=>p.MABP == _MaBP).OrderBy(p => p.MABP).ToList();
                        foreach (var itemCB in listCB)
                        {
                            var mabp = itemCB.MABP;
                            string tenbp = itemCB.BOPHAN.TENBP;
                            var macb = itemCB.MACB;
                            string hoten = itemCB.HOTEN;
                            var listEF = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == macb)
                                                          .OrderBy(p => p.MASTT)
                                                          .ToList();
                            double phiencho = 0;
                            double phienxuly = 0;
                            double tongphien = 0;
                            foreach (var item in listEF)
                            {
                                var mastt = item.MASTT;
                                var stt = item.SOTHUTU.STT;
                                DateTime start = new DateTime(((DateTime)item.TG).Year, ((DateTime)item.TG).Month, ((DateTime)item.TG).Day, 0, 0, 0);
                                DateTime end = new DateTime(((DateTime)item.TG).Year, ((DateTime)item.TG).Month, ((DateTime)item.TG).Day, 23, 59, 59);
                                var rut = db.SOTOIDAs.Where(p => p.MABP == mabp &&
                                                                 p.STTTD == stt &&
                                                                 p.TG >= start &&
                                                                 p.TG <= end)
                                                     .FirstOrDefault();
                                var goi = db.SOTHUTUs.Where(p => p.MASTT == mastt & p.BD != null & p.KT != null).FirstOrDefault();
                                phiencho += Math.Round(Math.Abs(((TimeSpan)(goi.BD - rut.TG)).TotalMinutes), 0);
                                phienxuly += Math.Round(Math.Abs(((TimeSpan)(goi.KT - goi.BD)).TotalMinutes), 0);
                                tongphien += phiencho + phienxuly;
                            }
                            BangThuTuc_BaoCao_ md = new BangThuTuc_BaoCao_()
                            {
                                MaBP = (int)mabp,
                                TenBP = tenbp,
                                MaCB = macb,
                                HoTen = hoten,
                                PhienCho = phiencho,
                                PhienXuLy = phienxuly,
                                TongPhien = tongphien
                            };
                            listMD.Add(md);
                        }
                    }
                    else if (_Start != null && _End != null)
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
                        else if (arrS.Length == 1)
                        {
                            int namE = Convert.ToInt32(arrE[0]);
                            end = new DateTime(namE, 12, DateTime.DaysInMonth(namE, 12), 23, 59, 59);
                        }

                        // Lấy thời gian xử lý thủ tục của toàn bộ cán bộ theo bộ phận trong khoảng thời gian nhất định
                        var listCB = db.CANBOes.Where(p => p.MABP == _MaBP).OrderBy(p => p.MABP).ToList();
                        foreach (var itemCB in listCB)
                        {
                            var mabp = itemCB.MABP;
                            string tenbp = itemCB.BOPHAN.TENBP;
                            var macb = itemCB.MACB;
                            string hoten = itemCB.HOTEN;
                            var listEF = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == macb &&
                                                                      p.TG >= start &&
                                                                      p.TG <= end)
                                                          .OrderBy(p => p.MASTT)
                                                          .ToList();
                            double phiencho = 0;
                            double phienxuly = 0;
                            double tongphien = 0;
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
                                phiencho += Math.Round(Math.Abs(((TimeSpan)(goi.BD - rut.TG)).TotalMinutes), 0);
                                phienxuly += Math.Round(Math.Abs(((TimeSpan)(goi.KT - goi.BD)).TotalMinutes), 0);
                                tongphien += phiencho + phienxuly;
                            }
                            BangThuTuc_BaoCao_ md = new BangThuTuc_BaoCao_()
                            {
                                MaBP = (int)mabp,
                                TenBP = tenbp,
                                MaCB = macb,
                                HoTen = hoten,
                                PhienCho = phiencho,
                                PhienXuLy = phienxuly,
                                TongPhien = tongphien
                            };
                            listMD.Add(md);
                        }
                    }
                }
            }
            else
            {
                if (_Start == null && _End == null)
                {
                    // Lấy thời gian xử lý thủ tục của cán bộ trong tất cả thời gian
                    var listCB = db.CANBOes.Where(p=>p.MACB == _MaCB).ToList();
                    foreach (var itemCB in listCB)
                    {
                        var mabp = itemCB.MABP;
                        string tenbp = itemCB.BOPHAN.TENBP;
                        var macb = itemCB.MACB;
                        string hoten = itemCB.HOTEN;
                        var listEF = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == macb)
                                                      .OrderBy(p => p.MASTT)
                                                      .ToList();
                        double phiencho = 0;
                        double phienxuly = 0;
                        double tongphien = 0;
                        foreach (var item in listEF)
                        {
                            var mastt = item.MASTT;
                            var stt = item.SOTHUTU.STT;
                            DateTime start = new DateTime(((DateTime)item.TG).Year, ((DateTime)item.TG).Month, ((DateTime)item.TG).Day, 0, 0, 0);
                            DateTime end = new DateTime(((DateTime)item.TG).Year, ((DateTime)item.TG).Month, ((DateTime)item.TG).Day, 23, 59, 59);
                            var rut = db.SOTOIDAs.Where(p => p.MABP == mabp &&
                                                             p.STTTD == stt &&
                                                             p.TG >= start &&
                                                             p.TG <= end)
                                                 .FirstOrDefault();
                            var goi = db.SOTHUTUs.Where(p => p.MASTT == mastt & p.BD != null & p.KT != null).FirstOrDefault();
                            phiencho += Math.Round(Math.Abs(((TimeSpan)(goi.BD - rut.TG)).TotalMinutes), 0);
                            phienxuly += Math.Round(Math.Abs(((TimeSpan)(goi.KT - goi.BD)).TotalMinutes), 0);
                            tongphien += phiencho + phienxuly;
                        }
                        BangThuTuc_BaoCao_ md = new BangThuTuc_BaoCao_()
                        {
                            MaBP = (int)mabp,
                            TenBP = tenbp,
                            MaCB = macb,
                            HoTen = hoten,
                            PhienCho = phiencho,
                            PhienXuLy = phienxuly,
                            TongPhien = tongphien
                        };
                        listMD.Add(md);
                    }
                }
                else if (_Start != null && _End != null)
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
                    else if (arrS.Length == 1)
                    {
                        int namE = Convert.ToInt32(arrE[0]);
                        end = new DateTime(namE, 12, DateTime.DaysInMonth(namE, 12), 23, 59, 59);
                    }

                    // Lấy thời gian xử lý thủ tục của cán bộ trong khoảng thời gian nhất định
                    var listCB = db.CANBOes.Where(p => p.MACB == _MaCB).ToList();
                    foreach (var itemCB in listCB)
                    {
                        var mabp = itemCB.MABP;
                        string tenbp = itemCB.BOPHAN.TENBP;
                        var macb = itemCB.MACB;
                        string hoten = itemCB.HOTEN;
                        var listEF = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == macb &&
                                                                  p.TG >= start &&
                                                                  p.TG <= end)
                                                      .OrderBy(p => p.MASTT)
                                                      .ToList();
                        double phiencho = 0;
                        double phienxuly = 0;
                        double tongphien = 0;
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
                            phiencho += Math.Round(Math.Abs(((TimeSpan)(goi.BD - rut.TG)).TotalMinutes), 0);
                            phienxuly += Math.Round(Math.Abs(((TimeSpan)(goi.KT - goi.BD)).TotalMinutes), 0);
                            tongphien += phiencho + phienxuly;
                        }
                        BangThuTuc_BaoCao_ md = new BangThuTuc_BaoCao_()
                        {
                            MaBP = (int)mabp,
                            TenBP = tenbp,
                            MaCB = macb,
                            HoTen = hoten,
                            PhienCho = phiencho,
                            PhienXuLy = phienxuly,
                            TongPhien = tongphien
                        };
                        listMD.Add(md);
                    }

                }
            }
            return listMD;
        }
    }
}
