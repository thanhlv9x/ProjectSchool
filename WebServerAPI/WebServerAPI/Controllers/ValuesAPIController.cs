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
    public class ValuesAPIController : ApiController
    {
        HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities();
        #region example
        // GET api/values
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //public void Delete(int id)
        //{
        //}
        #endregion

        /// <summary>
        /// Lấy thời gian của kết quả đánh giá
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<string> GetDate()
        {
            IList<string> listMD = new List<string>();
            try
            {
                var start = db.KETQUADANHGIAs.OrderBy(p => p.TG)
                                             .FirstOrDefault();
                int ngayS = ((DateTime)start.TG).Day;
                int thangS = ((DateTime)start.TG).Month;
                int namS = ((DateTime)start.TG).Year;
                string BatDau = namS + "-" + thangS + "-" + ngayS;
                var end = db.KETQUADANHGIAs.OrderByDescending(p => p.TG)
                                           .FirstOrDefault();

                int ngayE = ((DateTime)end.TG).Day;
                int thangE = ((DateTime)end.TG).Month;
                int namE = ((DateTime)end.TG).Year;
                string KetThuc = namE + "-" + thangE + "-" + ngayE;
                listMD.Add(BatDau);
                listMD.Add(KetThuc);
                return listMD;
            }
            catch
            {
                return listMD;
            }
        }
        /// <summary>
        /// Lấy thời gian của kết quả đánh giá theo bộ phận
        /// </summary>
        /// <param name="_MaBP">Mã bộ phận</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<string> GetDateBP(int _MaBP)
        {
            IList<string> listMD = new List<string>();
            try
            {
                var start = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.CANBO.MABP == _MaBP)
                                             .OrderBy(p => p.TG)
                                             .FirstOrDefault();
                int ngayS = ((DateTime)start.TG).Day;
                int thangS = ((DateTime)start.TG).Month;
                int namS = ((DateTime)start.TG).Year;
                string BatDau = namS + "-" + thangS + "-" + ngayS;
                var end = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.CANBO.MABP == _MaBP)
                                           .OrderByDescending(p => p.TG)
                                           .FirstOrDefault();

                int ngayE = ((DateTime)end.TG).Day;
                int thangE = ((DateTime)end.TG).Month;
                int namE = ((DateTime)end.TG).Year;
                string KetThuc = namE + "-" + thangE + "-" + ngayE;
                listMD.Add(BatDau);
                listMD.Add(KetThuc);
                return listMD;
            }
            catch
            {
                return listMD;
            }
        }
        /// <summary>
        /// Lấy thời gian của kết quả đánh giá theo cán bộ
        /// </summary>
        /// <param name="_MaCB">Mã cán bộ</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<string> GetDateCB(int _MACB)
        {
            IList<string> listMD = new List<string>();
            try
            {
                var start = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == _MACB)
                                             .OrderBy(p => p.TG)
                                             .FirstOrDefault();
                int ngayS = ((DateTime)start.TG).Day;
                int thangS = ((DateTime)start.TG).Month;
                int namS = ((DateTime)start.TG).Year;
                string BatDau = namS + "-" + thangS + "-" + ngayS;
                var end = db.KETQUADANHGIAs.Where(p => p.SOTHUTU.MACB == _MACB)
                                           .OrderByDescending(p => p.TG)
                                           .FirstOrDefault();

                int ngayE = ((DateTime)end.TG).Day;
                int thangE = ((DateTime)end.TG).Month;
                int namE = ((DateTime)end.TG).Year;
                string KetThuc = namE + "-" + thangE + "-" + ngayE;
                listMD.Add(BatDau);
                listMD.Add(KetThuc);
                return listMD;
            }
            catch
            {
                return listMD;
            }
        }
        /// <summary>
        /// Lấy thông tin cán bộ
        /// </summary>
        /// <param name="_MaCB"></param>
        /// <returns></returns>
        [HttpGet]
        public CanBo GetInfo(int _MaCB, int _Info)
        {
            var lstEF = db.CANBOes.Where(p => p.MACB == _MaCB).FirstOrDefault();
            string img = getImage(lstEF.HINHANH);
            CanBo md = new CanBo()
            {
                MaCB = lstEF.MACB,
                HoTen = lstEF.HOTEN,
                MaBP = lstEF.MABP,
                TenBP = lstEF.BOPHAN.TENBP,
                HinhAnh = img
            };
            return md;
        }
        /// <summary>
        /// Phương thức chuyển hình ảnh thành chuỗi byte
        /// </summary>
        /// <param name="path">Đường dẫn hình ảnh</param>
        /// <returns></returns>
        public string getImage(string path)
        {
            string thuMucGoc = AppDomain.CurrentDomain.BaseDirectory;
            string thuMucHinh = thuMucGoc + @"\resources\";
            MemoryStream ms = new MemoryStream();
            Image img = Image.FromFile(thuMucHinh + path);
            img.Save(ms, img.RawFormat);
            byte[] data = ms.ToArray();
            string strImg = Convert.ToBase64String(data);
            return strImg;
        }

    }
}
