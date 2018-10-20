using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebServerAPI.EF;
using WebServerAPI.Models;

namespace WebServerAPI.Controllers
{
    public class TaiKhoanAPIController : ApiController
    {
        HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities();

        /// <summary>
        /// Dịch vụ lấy thông tin tài khoản của cán bộ theo bộ phận
        /// </summary>
        /// <param name="_MaBP">Mã bộ phận</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<CanBo> Get(int _MaBP)
        {
            var listEF = db.CANBOes.Where(p => p.MABP == _MaBP).ToList();
            IList<CanBo> listMD = new List<CanBo>();
            foreach (var item in listEF)
            {
                CanBo md = new CanBo()
                {
                    MaCB = item.MACB,
                    HoTen = item.HOTEN,
                    HinhAnh = item.HINHANH,
                    MaBP = item.MABP,
                    Id = item.ID,
                    Pw = item.PW,
                };
                listMD.Add(md);
            }
            return listMD;
        }

        [HttpPost]
        public bool Create(List<CanBo> model)
        {
            bool success = false;

            foreach (var item in model)
            {
                try
                {
                    string pw = (item.Pw);
                    CANBO md = new CANBO()
                    {
                        HOTEN = item.HoTen,
                        HINHANH = item.HinhAnh,
                        MABP = item.MaBP,
                        ID = item.Id,
                        PW = pw
                    };
                    db.CANBOes.Add(md);
                    db.SaveChanges();
                    success = true;
                }
                catch { }
            }

            return success;
        }

        ///// <summary>
        ///// Dịch vụ thêm mới thông tin tài khoản cán bộ
        ///// </summary>
        ///// <param name="cb">Thông tin cán bộ cần thêm mới</param>
        ///// <returns></returns>
        //[HttpPost]
        //public HttpResponseMessage Post([FromBody]CanBo md)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        CANBO model = new CANBO()
        //        {
        //            HOTEN = md.HoTen,
        //            HINHANH = md.HinhAnh,
        //            MABP = md.MaBP,
        //            ID = md.Id,
        //            PW = md.Pw
        //        };
        //        db.CANBOes.Add(model);
        //        db.SaveChanges();
        //        HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
        //        response.Headers.Location = new Uri(Url.Link("DefaultApi", new { MACB = model.MACB }));
        //        return response;
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest);
        //    }
        //}

        /// <summary>
        /// Dịch vụ cập nhật thông tin tài khoản cán bộ
        /// </summary>
        /// <param name="_MaCB">Mã cán bộ cần cập nhật</param>
        /// <param name="cb">Thông tin cán bộ cần cập nhật</param>
        /// <returns></returns>
        [HttpPut]
        public HttpResponseMessage Put(int _MaCB, CANBO cb)
        {
            if (ModelState.IsValid && _MaCB == cb.MACB)
            {
                db.Entry(cb).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                return Request.CreateResponse(HttpStatusCode.OK, cb);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Dịch vụ xóa thông tin tài khoản cán bộ
        /// </summary>
        /// <param name="_MaCB">Mã cán bộ cần xóa</param>
        /// <returns></returns>
        [HttpDelete]
        public HttpResponseMessage Delete(int _MaCB)
        {
            CANBO cb = db.CANBOes.Find(_MaCB);
            if (cb == null) return Request.CreateResponse(HttpStatusCode.NotFound);
            db.CANBOes.Remove(cb);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(HttpStatusCode.OK, cb);
        }
    }
}
