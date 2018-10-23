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
            if(stttdEF != null)
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
                if (stttdEF != null)
                {
                    var sttEF = db.SOTHUTUs.Where(p => p.CANBO.MABP == _MaBP &&
                                                       p.TG >= dt)
                                           .Select(p => new
                                           {
                                               STT = p.STT,
                                               MACB = p.MACB,
                                               TG = p.TG
                                           })
                                           .OrderByDescending(p => p.STT)
                                           .FirstOrDefault();
                    if (sttEF != null && sttEF.STT < stttdEF.STTTD)
                    {
                        try
                        {
                            DateTime dtNow = DateTime.Now;
                            int stt = (int)sttEF.STT;
                            int next = stt + 1;
                            SOTHUTU ef = new SOTHUTU()
                            {
                                STT = next,
                                MACB = _MaCB,
                                TG = dtNow
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
                    else if(sttEF == null)
                    {
                        try
                        {
                            int next = 1;
                            DateTime dtNow = DateTime.Now;
                            SOTHUTU ef = new SOTHUTU()
                            {
                                STT = next,
                                MACB = _MaCB,
                                TG = dtNow
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
            var numberEF = db.SOTHUTUs.Where(p => p.MACB == _MaCB && p.TG >= dt)
                                      .OrderByDescending(p => p.STT)
                                      .FirstOrDefault();
            if (numberEF != null)
            {
                SoThuTuUser sttMD = new SoThuTuUser()
                {
                    MaSTT = numberEF.MASTT,
                    STT = numberEF.STT,
                    MaCB = numberEF.MACB,
                    TG = numberEF.TG
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
                if (stttdEF != null)
                {
                    var sttEF = db.SOTHUTUs.Where(p => p.CANBO.MABP == _MaBP &&
                                                       p.TG >= dt)
                                           .Select(p => new
                                           {
                                               STT = p.STT,
                                               MACB = p.MACB,
                                               TG = p.TG
                                           })
                                           .OrderByDescending(p => p.STT)
                                           .FirstOrDefault();
                    if (sttEF != null && sttEF.STT < stttdEF.STTTD)
                    {
                        try
                        {
                            DateTime dtNow = DateTime.Now;
                            int stt = (int)sttEF.STT;
                            int next = stt + 1;
                            SOTHUTU ef = new SOTHUTU()
                            {
                                STT = next,
                                MACB = _MaCB,
                                TG = dtNow
                            };
                            db.SOTHUTUs.Add(ef);
                            if(_MaSTT != 0)
                            {
                                KETQUADANHGIA kq = new KETQUADANHGIA()
                                {
                                    MASTT = _MaSTT,
                                    TG = dtNow
                                };
                                db.KETQUADANHGIAs.Add(kq);
                            }
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
                    else if (sttEF != null && sttEF.STT == stttdEF.STTTD)
                    {
                        if (_MaSTT != 0)
                        {
                            DateTime dtNow = DateTime.Now;
                            KETQUADANHGIA kq = new KETQUADANHGIA()
                            {
                                MASTT = _MaSTT,
                                TG = dtNow
                            };
                            db.KETQUADANHGIAs.Add(kq);
                            db.SaveChanges();
                        }
                        return Request.CreateResponse(HttpStatusCode.NotFound);
                    }
                    else if (sttEF == null)
                    {
                        try
                        {
                            int next = 1;
                            DateTime dtNow = DateTime.Now;
                            SOTHUTU ef = new SOTHUTU()
                            {
                                STT = next,
                                MACB = _MaCB,
                                TG = dtNow
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

        [HttpGet]
        public HttpResponseMessage GetNumberAfterRefresh(int _MaCB, int _Refresh)
        {
            int number = 0;
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            var lstSTT = db.SOTHUTUs.Where(p => p.MACB == _MaCB && 
                                               p.TG >= dt)
                                   .OrderByDescending(p => p.MASTT)
                                   .FirstOrDefault();
            number = (int)lstSTT.STT;
            int mastt = lstSTT.MASTT;
            var lstKQ = db.KETQUADANHGIAs.Where(p => p.MASTT == mastt && p.TG >= dt).ToList();
            if(lstKQ.Count == 0)
            {
                DateTime dtNow = DateTime.Now;
                KETQUADANHGIA kq = new KETQUADANHGIA()
                {
                    TG = dtNow,
                    MASTT = mastt
                };
                db.KETQUADANHGIAs.Add(kq);
                db.SaveChanges();
            }
            return Request.CreateResponse<int>(HttpStatusCode.OK, number);
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
                var userEF = db.CANBOes.Where(p => p.ID == id && p.PW == pw).FirstOrDefault();

                if (userEF != null)
                {
                    string hinh_anh = getImage(userEF.HINHANH);
                    TaiKhoanUser userMD = new TaiKhoanUser()
                    {
                        MaCB = userEF.MACB,
                        MaBP = userEF.MABP,
                        HoTen = userEF.HOTEN,
                        HinhAnh = hinh_anh,
                        TenBP = userEF.BOPHAN.TENBP
                    };
                    var httpResponse = Request.CreateResponse<TaiKhoanUser>(HttpStatusCode.Created, userMD);
                    string uri = Url.Link("DefaultApi", new { id = userMD.MaCB });
                    httpResponse.Headers.Location = new Uri(uri);
                    return httpResponse;
                }
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
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
                int count = db.CANBOes.Where(p=>p.MACB == macb).Count();
                if (count > 0)
                {
                    int mastt = _DanhGia.MaSTT;
                    int muc_do = _DanhGia.MucDo;
                    string gop_y = _DanhGia.GopY;
                    DateTime dt = DateTime.Now;

                    int count_kq = db.KETQUADANHGIAs.Where(p => p.MASTT == mastt).Count();
                    if (count_kq == 0)
                    {
                        try
                        {
                            KETQUADANHGIA kqEF = new KETQUADANHGIA()
                            {
                                MASTT = mastt,
                                MUCDO = muc_do,
                                TG = dt
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
