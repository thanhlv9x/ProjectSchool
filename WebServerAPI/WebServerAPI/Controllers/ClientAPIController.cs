using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebServerAPI.EF;
using WebServerAPI.Models;

namespace WebServerAPI.Controllers
{
    public class ClientAPIController : ApiController
    {
        HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities();

        /// <summary>
        /// Dịch vụ lấy tất cả bộ phận
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Get()
        {
            IList<BoPhan> listMD = new List<BoPhan>();
            var listEF = db.BOPHANs.ToList();
            if (listEF != null)
            {
                foreach (var item in listEF)
                {
                    BoPhan BoPhanMD = new BoPhan()
                    {
                        MaBP = Convert.ToInt32(item.MABP),
                        TenBP = item.TENBP
                    };
                    listMD.Add(BoPhanMD);
                }
                return Request.CreateResponse<IList<BoPhan>>(HttpStatusCode.OK, listMD);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
        /// <summary>
        /// Dịch vụ lấy tất cả mã máy
        /// </summary>
        /// <param name="_Port">Tham số xác nhận gọi hàm</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetPort(int _Port)
        {
            List<int> lstMD = new List<int>();
            var lstEF = db.MAYDANHGIAs.ToList();
            if (lstEF != null)
            {
                foreach (var item in lstEF)
                {
                    lstMD.Add(item.MAMAY);
                }
                return Request.CreateResponse<List<int>>(HttpStatusCode.OK, lstMD);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
        /// <summary>
        /// Dịch vụ lấy thông tin của cán bộ đang đăng nhập theo mã máy
        /// </summary>
        /// <param name="_MaMay"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetInfo(int _MaMay)
        {
            var lstEF = db.TRANGTHAIDANGNHAPs.Where(p => p.MAMAY == _MaMay &&
                                                         p.BD != null &&
                                                         p.KT == null)
                                             .OrderByDescending(p => p.MADN)
                                             .FirstOrDefault();
            if (lstEF != null)
            {
                string hinh_anh = getImage(lstEF.CANBO.HINHANH);
                TaiKhoanUser md = new TaiKhoanUser()
                {
                    MaCB = lstEF.MACB,
                    HoTen = lstEF.CANBO.HOTEN,
                    HinhAnh = hinh_anh,
                    MaBP = lstEF.CANBO.MABP,
                    TenBP = lstEF.CANBO.BOPHAN.TENBP,
                    MaDN = lstEF.MADN,
                    MaMay = lstEF.MAMAY
                };
                return Request.CreateResponse<TaiKhoanUser>(HttpStatusCode.OK, md);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);

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
                DateTime now = new DateTime();
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
                DateTime now = new DateTime();
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
        /// <summary>
        /// Dịch vụ gọi số thứ tự theo bộ phận
        /// </summary>
        /// <param name="_MaBP">Mã bộ phận</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Get(int _MaCB, int _MaBP)
        {
            var canboEF = db.CANBOes.Where(p => p.MACB == _MaCB && p.MABP == _MaBP).Count();
            if (canboEF > 0)
            {
                DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                var stttdEF = db.SOTOIDAs.Where(p => p.MABP == _MaBP &&
                                                     p.TG >= dt)
                                         .OrderByDescending(p => p.STTTD)
                                         .FirstOrDefault();
                if (stttdEF != null) // Nếu người dân đã rút số
                {
                    var sttEF = db.SOTHUTUs.Where(p => p.CANBO.MABP == _MaBP &&
                                                       p.BD >= dt)
                                           .Select(p => new
                                           {
                                               STT = p.STT,
                                               MACB = p.MACB,
                                               BD = p.BD,
                                               KT = p.KT
                                           })
                                           .OrderByDescending(p => p.STT)
                                           .FirstOrDefault();
                    if (sttEF != null && sttEF.STT < stttdEF.STTTD) // Nếu cán bộ đã từng gọi số và số nhỏ hơn số người dân đã rút
                    {
                        DateTime dtNow = DateTime.Now;
                        try
                        {
                            // Lưu lại thời gian kết thúc của số thứ tự trước
                            var sttBefore = db.SOTHUTUs.Where(p => p.MACB == _MaCB &&
                                                                   p.BD >= dt &&
                                                                   p.KT == null)
                                                       .OrderByDescending(p => p.MASTT)
                                                       .FirstOrDefault();
                            sttBefore.KT = dtNow;
                            db.SaveChanges();
                        }
                        catch { }
                        try
                        {
                            // Khởi tạo số thứ tự mới
                            int stt = (int)sttEF.STT;
                            int next = stt + 1;
                            SOTHUTU ef = new SOTHUTU()
                            {
                                STT = next,
                                MACB = _MaCB,
                                BD = dtNow
                            };
                            db.SOTHUTUs.Add(ef);
                            db.SaveChanges();
                            SoThuTuUser sttMD = new SoThuTuUser()
                            {
                                MaSTT = ef.MASTT,
                                STT = next
                            };
                            return Request.CreateResponse<SoThuTuUser>(HttpStatusCode.OK, sttMD);
                        }
                        catch
                        {
                            return Request.CreateResponse(HttpStatusCode.NotFound);
                        }
                    }
                    else if (sttEF == null) // Nếu cán bộ chưa từng gọi số
                    {
                        try
                        {
                            // Khởi tạo số mới
                            int next = 1;
                            DateTime dtNow = DateTime.Now;
                            SOTHUTU ef = new SOTHUTU()
                            {
                                STT = next,
                                MACB = _MaCB,
                                BD = dtNow
                            };
                            db.SOTHUTUs.Add(ef);
                            db.SaveChanges();
                            SoThuTuUser sttMD = new SoThuTuUser()
                            {
                                MaSTT = ef.MASTT,
                                STT = next
                            };
                            return Request.CreateResponse<SoThuTuUser>(HttpStatusCode.OK, sttMD);
                        }
                        catch
                        {
                            return Request.CreateResponse(HttpStatusCode.NotFound);
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound);
                    }
                }
                else // Nếu người dân chưa rút số nào
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
        /// <summary>
        /// Dịch vụ gọi số thứ tự tự động
        /// </summary>
        /// <param name="_MaCB">Mã cán bộ</param>
        /// <param name="_MaBP">Mã bộ phận</param>
        /// <param name="_Interval">Tham số xác nhận của dịch vụ</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetNumberInterval(int _MaCB, int _MaBP, int _Interval)
        {
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            var numberEF = db.SOTHUTUs.Where(p => p.MACB == _MaCB &&
                                                  p.BD >= dt &&
                                                  p.KT == null)
                                      .OrderByDescending(p => p.MASTT)
                                      .FirstOrDefault();
            if (numberEF != null) // Nếu có số đang gọi
            {
                SoThuTuUser sttMD = new SoThuTuUser()
                {
                    MaSTT = numberEF.MASTT,
                    STT = numberEF.STT,
                    MaCB = numberEF.MACB,
                    TG = numberEF.BD
                };
                return Request.CreateResponse<SoThuTuUser>(HttpStatusCode.OK, sttMD);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
        /// <summary>
        /// Dịch vụ gọi số và lưu lại thời gian hoàn thành số trước theo bộ phận
        /// </summary>
        /// <param name="_MaCB">Mã cán bộ</param>
        /// <param name="_MaBP">Mã bộ phận</param>
        /// <param name="_MaSTT">Mã số thứ tự của số trước</param>
        /// <param name="_Submit">Tham số xác nhận dịch vụ</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAndSave(int _MaCB, int _MaBP, int _MaSTT, int _Submit)
        {
            var canboEF = db.CANBOes.Where(p => p.MACB == _MaCB && p.MABP == _MaBP).Count();
            if (canboEF > 0)
            {
                DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                var stttdEF = db.SOTOIDAs.Where(p => p.MABP == _MaBP &&
                                                     p.TG >= dt)
                                         .OrderByDescending(p => p.STTTD)
                                         .FirstOrDefault();
                if (stttdEF != null) // Nếu người dân đã rút số
                {
                    var sttEF = db.SOTHUTUs.Where(p => p.CANBO.MABP == _MaBP &&
                                                       p.BD >= dt)
                                           .Select(p => new
                                           {
                                               STT = p.STT,
                                               MACB = p.MACB,
                                               BD = p.BD,
                                               KT = p.KT
                                           })
                                           .OrderByDescending(p => p.STT)
                                           .FirstOrDefault();
                    if (sttEF != null && sttEF.STT < stttdEF.STTTD) // Nếu cán bộ đã từng gọi số và số nhỏ hơn số người dân đã rút
                    {
                        DateTime dtNow = DateTime.Now;
                        try
                        {
                            // Lưu lại thời gian kết thúc của số thứ tự trước
                            var sttBefore = db.SOTHUTUs.Where(p => p.MACB == _MaCB &&
                                                                   p.BD >= dt &&
                                                                   p.KT == null)
                                                       .OrderByDescending(p => p.MASTT)
                                                       .FirstOrDefault();
                            sttBefore.KT = dtNow;
                            db.SaveChanges();
                        }
                        catch { }
                        try
                        {
                            // Khởi tạo số thứ tự tiếp theo
                            int stt = (int)sttEF.STT;
                            int next = stt + 1;
                            SOTHUTU ef = new SOTHUTU()
                            {
                                STT = next,
                                MACB = _MaCB,
                                BD = dtNow
                            };
                            db.SOTHUTUs.Add(ef);
                            db.SaveChanges();
                            SoThuTuUser sttMD = new SoThuTuUser()
                            {
                                MaSTT = ef.MASTT,
                                STT = next
                            };
                            return Request.CreateResponse<SoThuTuUser>(HttpStatusCode.OK, sttMD);
                        }
                        catch
                        {
                            return Request.CreateResponse(HttpStatusCode.NotFound);
                        }
                    }
                    else if (sttEF != null && sttEF.STT == stttdEF.STTTD) // Nếu cán bộ đã từng gọi số và số bằng với số người dân đã rút
                    {
                        //if (_MaSTT != 0)
                        //{
                        DateTime dtNow = DateTime.Now;
                        try
                        {
                            // Lưu lại thời gian kết thúc của số thứ tự trước
                            var sttBefore = db.SOTHUTUs.Where(p => p.MACB == _MaCB &&
                                                                   p.BD >= dt &&
                                                                   p.KT == null)
                                                       .OrderByDescending(p => p.MASTT)
                                                       .FirstOrDefault();
                            sttBefore.KT = dtNow;
                            db.SaveChanges();
                        }
                        catch { }
                        //}
                        return Request.CreateResponse(HttpStatusCode.NotFound);
                    }
                    else if (sttEF == null) // Nếu cán bộ chưa từng gọi số
                    {
                        try
                        {
                            // Khởi tạo số thứ tự mới
                            int next = 1;
                            DateTime dtNow = DateTime.Now;
                            SOTHUTU ef = new SOTHUTU()
                            {
                                STT = next,
                                MACB = _MaCB,
                                BD = dtNow
                            };
                            db.SOTHUTUs.Add(ef);
                            db.SaveChanges();
                            SoThuTuUser sttMD = new SoThuTuUser()
                            {
                                MaSTT = ef.MASTT,
                                STT = next
                            };
                            return Request.CreateResponse<SoThuTuUser>(HttpStatusCode.OK, sttMD);
                        }
                        catch
                        {
                            return Request.CreateResponse(HttpStatusCode.NotFound);
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound);
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
        /// <summary>
        /// Dịch vụ đăng nhập User
        /// </summary>
        /// <param name="_User">Thông tin tài khoản user</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Post(TaiKhoanUser _User)
        {
            if ((this.ModelState.IsValid) && (_User != null))
            {
                string id = _User.Id;
                string pw = _User.Pw;
                string mac = _User.Mac;
                var userEF = db.CANBOes.Where(p => p.ID == id && p.PW == pw).FirstOrDefault();

                if (userEF != null)
                {
                    DateTime dtNow = DateTime.Now;
                    int macb = userEF.MACB;
                    int mamay = db.MAYDANHGIAs.Where(p => p.MAC == mac).Select(p => p.MAMAY).FirstOrDefault();
                    if (mamay != null)
                    {
                        TRANGTHAIDANGNHAP login = new TRANGTHAIDANGNHAP()
                        {
                            MACB = macb,
                            MAMAY = mamay,
                            BD = dtNow,
                        };
                        db.TRANGTHAIDANGNHAPs.Add(login);
                        db.SaveChanges();
                        string hinh_anh = getImage(userEF.HINHANH);
                        TaiKhoanUser userMD = new TaiKhoanUser()
                        {
                            MaCB = userEF.MACB,
                            MaBP = userEF.MABP,
                            HoTen = userEF.HOTEN,
                            HinhAnh = hinh_anh,
                            TenBP = userEF.BOPHAN.TENBP,
                            MaMay = mamay,
                            MaDN = login.MADN
                        };
                        var httpResponse = Request.CreateResponse<TaiKhoanUser>(HttpStatusCode.Created, userMD);
                        string uri = Url.Link("DefaultApi", new { id = userMD.MaCB });
                        httpResponse.Headers.Location = new Uri(uri);
                        return httpResponse;
                    }
                }
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }
        /// <summary>
        /// Dịch vụ đăng xuất của User
        /// </summary>
        /// <param name="_Logout"></param>
        /// <param name="_User"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Post(int _Logout, TaiKhoanUser _User)
        {
            if ((this.ModelState.IsValid) && (_User != null))
            {
                int madn = (int)_User.MaDN;
                var lstEF = db.TRANGTHAIDANGNHAPs.Where(p => p.MADN == madn &&
                                                             p.KT == null)
                                                 .OrderByDescending(p => p.MADN)
                                                 .FirstOrDefault();
                if (lstEF != null)
                {
                    lstEF.KT = DateTime.Now;
                    db.SaveChanges();
                }
                var httpResponse = Request.CreateResponse<bool>(HttpStatusCode.Created, true);
                string uri = Url.Link("DefaultApi", new { id = lstEF.MADN });
                httpResponse.Headers.Location = new Uri(uri);
                return httpResponse;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
        /// <summary>
        /// Dịch vụ lưu kết quả đánh giá và góp ý của user
        /// </summary>
        /// <param name="_DanhGia">Kết quả đánh giá và góp ý</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage PostReview(int _Success, KetQuaDanhGiaUser _DanhGia)
        {
            if ((this.ModelState.IsValid) && (_DanhGia != null))
            {
                int macb = _DanhGia.MaCB;
                int count = db.CANBOes.Where(p => p.MACB == macb).Count();
                if (count > 0)
                {
                    int mastt = _DanhGia.MaSTT;
                    int muc_do = _DanhGia.MucDo;
                    string gop_y = _DanhGia.GopY;
                    DateTime dtNow = DateTime.Now;
                    DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                    int count_stt = db.SOTHUTUs.Where(p => p.MASTT == mastt).Count();
                    int count_kq = db.KETQUADANHGIAs.Where(p => p.MASTT == mastt).Count();
                    if (count_kq == 0 && count_stt > 0)
                    {
                        try
                        {
                            KETQUADANHGIA kqEF = new KETQUADANHGIA()
                            {
                                MASTT = mastt,
                                MUCDO = muc_do,
                                TG = dtNow
                            };
                            db.KETQUADANHGIAs.Add(kqEF);
                            db.SaveChanges();

                            if (gop_y != null && gop_y != "" && gop_y != String.Empty)
                            {
                                GOPY gyEF = new GOPY()
                                {
                                    MADG = kqEF.MADG,
                                    NOIDUNG = gop_y
                                };
                                db.GOPies.Add(gyEF);
                                db.SaveChanges();
                            }

                            var httpResponse = Request.CreateResponse<DateTime>(HttpStatusCode.Created, dt);
                            string uri = Url.Link("DefaultApi", new { id = kqEF.MADG });
                            httpResponse.Headers.Location = new Uri(uri);
                            return httpResponse;
                        }
                        catch
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest);
                        }
                    }
                }
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }
        /// <summary>
        /// Dịch vụ thay đổi mật khẩu
        /// </summary>
        /// <param name="_MaCB">Mã cán bộ</param>
        /// <param name="_User">Thông tin tài khoản user</param>
        /// <returns></returns>
        [HttpPut]
        public HttpResponseMessage Put(int _MaCB, TaiKhoanUserNew _User)
        {
            if ((this.ModelState.IsValid) && (_User != null) && (_User.MaCB.Equals(_MaCB)))
            {
                int macb = (int)_User.MaCB;
                string id = _User.Id;
                string old_pw = _User.OldPw;
                string new_pw = _User.NewPw;

                var accountEF = db.CANBOes.Where(p => p.MACB == macb && p.ID == id && p.PW == old_pw).FirstOrDefault();

                if (accountEF != null)
                {
                    accountEF.PW = new_pw;
                    db.SaveChanges();
                    return Request.CreateResponse<bool>(HttpStatusCode.OK, true);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }
        /// <summary>
        /// Phương thức chuyển hình ảnh thành chuỗi byte
        /// </summary>
        /// <param name="path">Đường dẫn hình ảnh</param>
        /// <returns></returns>
        public string getImage(string path)
        {
            MemoryStream ms = new MemoryStream();
            Image img = Image.FromFile(path);
            img.Save(ms, img.RawFormat);
            byte[] data = ms.ToArray();
            string strImg = Convert.ToBase64String(data);
            return strImg;
        }
    }
}
