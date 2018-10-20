using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebServerAPI.EF;
using WebServerAPI.Models;
using Excel = Microsoft.Office.Interop.Excel;

namespace WebServerAPI.Controllers
{
    public class TaiKhoanController : Controller
    {
        HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities();
        // GET: TaiKhoan
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Lấy toàn bộ dữ liệu thông tin tài khoản của cán bộ
        /// </summary>
        /// <returns></returns>
        public JsonResult Read()
        {
            var listEF = db.CANBOes.Select(p => new
            {
                ID = p.ID,
                PW = p.PW,
                MACB = p.MACB,
                HOTEN = p.HOTEN,
                HINHANH = p.HINHANH,
                MABP = p.BOPHAN.MABP,
                TENBP = p.BOPHAN.TENBP
            })
                                   .OrderBy(p => p.MACB)
                                   .ToList();

            IList<CanBo> listMD = new List<CanBo>();
            foreach (var item in listEF)
            {
                CanBo md = new CanBo()
                {
                    MaCB = (int)item.MACB,
                    HoTen = item.HOTEN,
                    HinhAnh = item.HINHANH,
                    MaBP = item.MABP,
                    Id = item.ID,
                    Pw = item.PW,
                    TenBP = item.TENBP,
                };
                listMD.Add(md);
            }

            return Json(listMD, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Thêm mới thông tin tài khoản cán bộ
        /// </summary>
        /// <param name="model">Thông tin tài khoản cán bộ</param>
        /// <returns></returns>
        public JsonResult Create(CanBo model)
        {
            bool success = false;
            try
            {
                string pw = GetMD5(model.Pw);
                CANBO md = new CANBO()
                {
                    HOTEN = model.HoTen,
                    HINHANH = model.HinhAnh,
                    MABP = model.MaBP,
                    ID = model.Id,
                    PW = pw
                };
                db.CANBOes.Add(md);
                db.SaveChanges();
                success = true;
            }
            catch { }
            return Json(success, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Cập nhật thông tin tài khoản cán bộ
        /// </summary>
        /// <param name="model">Thông tin tài khoản cán bộ</param>
        /// <returns></returns>
        public JsonResult Update(List<CanBo> model)
        {
            bool success = false;
            foreach (var item in model)
            {
                try
                {
                    string tenbp = "";
                    if (item.MaBP == 1) tenbp = "TNDD";
                    else if (item.MaBP == 2) tenbp = "XD";
                    else if (item.MaBP == 3) tenbp = "TP";
                    else if (item.MaBP == 4) tenbp = "LDTBXH";
                    else if (item.MaBP == 5) tenbp = "CPXD";
                    int macb = item.MaCB;
                    string id = item.Id + "-" + item.MaCB + "-" + tenbp;
                    string pw = GetMD5(item.Pw);
                    var md = db.CANBOes.Where(p => p.MACB == macb).FirstOrDefault();
                    if (md.ID.Equals(String.Empty))
                    {
                        md.ID = id;
                    };
                    if (!md.PW.Equals(pw))
                    {
                        md.PW = pw;
                    }
                    db.SaveChanges();
                    success = true;
                }
                catch { }
            }
            return Json(success, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Phương thức xóa thông tin tài khoản của cán bộ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult Delete(List<CanBo> model)
        {
            bool success = false;
            foreach (var item in model)
            {
                try
                {
                    int macb = item.MaCB;
                    string id = item.Id;
                    var md = db.CANBOes.Where(p => p.MACB == macb && p.ID == id).FirstOrDefault();
                    md.ID = String.Empty;
                    md.PW = String.Empty;
                    db.SaveChanges();
                    success = true;
                }
                catch { }
            }
            return Json(success, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Hàm upload file excel
        /// </summary>
        /// <param name="excelfile">File Excel</param>
        /// <returns></returns>
        public ActionResult Upload(HttpPostedFileBase excelfile)
        {
            if (excelfile == null)
            {
                ViewBag.Error = "Vui lòng chọn file excel !";
                //return RedirectToAction("Index", "Home");
                return Json("Vui lòng chọn file excel !", JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    string path = Server.MapPath("~/Files/" + excelfile.FileName);
                    if (System.IO.File.Exists(path))
                    {
                        try
                        {
                            System.IO.File.Delete(path);
                        }
                        catch (Exception ex){ }
                    }
                    try
                    {
                        excelfile.SaveAs(path);
                    }
                    catch (Exception ex) { }

                    // Tạo đối tượng Excel
                    Excel.Application app = new Excel.Application();
                    // Mở tệp Excel
                    Excel.Workbook wb = app.Workbooks.Open(path);
                    try
                    {
                        // Mở Sheet
                        Excel._Worksheet sheet = wb.Sheets[1];
                        // Lấy vùng dữ liệu người dùng đã sử dụng
                        Excel.Range range = sheet.UsedRange;
                        // Lấy số cột
                        int cols = range.Columns.Count;
                        // Lấy số dòng
                        int rows = range.Rows.Count;
                        //for (int c = 1; c <= cols; c++)
                        //{
                        //    // đọc dòng tiêu đề
                        //}
                        for (int i = 2; i <= rows; i++)
                        {
                            try
                            {
                                string thuMucGoc = AppDomain.CurrentDomain.BaseDirectory;
                                string thuMucHinh = thuMucGoc + @"\resources\";
                                if (!Directory.Exists(thuMucHinh))
                                {
                                    Directory.CreateDirectory(thuMucHinh);
                                }
                                CANBO cb = new CANBO();
                                cb.HOTEN = range.Cells[i, 1].Value.ToString();

                                // Thay đổi kích thước và lưu ảnh vào thư mục resource của project
                                Image img = Image.FromFile(range.Cells[i, 2].Value.ToString());
                                img = ResizeByWidth(img);
                                string tenbp = range.Cells[i, 3].Value.ToString();
                                string tenbp_new = tenbp.Substring(0,1).ToUpper() + tenbp.Substring(1).ToLower();
                                int mabp = db.BOPHANs.Where(p => p.TENBP == tenbp_new).Select(p => p.MABP).FirstOrDefault();
                                cb.MABP = mabp;
                                string tenbp_id = "";
                                if (mabp == 1) tenbp_id = "TNDD";
                                else if (mabp == 2) tenbp_id = "XD";
                                else if (mabp == 3) tenbp_id = "TP";
                                else if (mabp == 4) tenbp_id = "LDTBXH";
                                else if (mabp == 5) tenbp_id = "CPXD";
                                string pw = GetMD5("123");
                                string[] hoten = cb.HOTEN.Split(' ');
                                string id = "";
                                for (int j = 0; j < hoten.Length; j++)
                                {
                                    id += getCharacter(hoten[j].ToLower()[0]);
                                }
                                cb.PW = pw;
                                cb.ID = id;
                                db.CANBOes.Add(cb);
                                db.SaveChanges();
                                id += "-" + cb.MACB + "-" + tenbp_id;
                                img.Save(thuMucHinh + cb.MACB + ".png");
                                cb.HINHANH = thuMucHinh + cb.MACB + ".png";
                                cb.ID = id;
                                db.SaveChanges();
                            }
                            catch(Exception ex) { }
                        }
                        wb.Close(0);
                        app.Quit();

                        return Json("Dữ liệu được nhập thành công !", JsonRequestBehavior.AllowGet);
                    }
                    catch
                    {
                        wb.Close(0);
                        app.Quit();
                        //return RedirectToAction("Index", "Home");
                        return Json("Dữ liệu không chính xác, vui lòng kiểm tra lại thông tin !", JsonRequestBehavior.AllowGet);
                    }

                    //return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Error = "Vui lòng chọn file excel !";
                    //return RedirectToAction("Index", "Home");

                    return Json("Vui lòng chọn file excel !", JsonRequestBehavior.AllowGet);
                }
            }
            //if (Request.Files.Count > 0)
            //{
            //    var file = Request.Files[0];

            //    if (file != null && file.ContentLength > 0)
            //    {
            //        var fileName = Path.GetFileName(file.FileName);
            //        var path = Path.Combine(Server.MapPath("~/Files/"), fileName);
            //        file.SaveAs(path);
            //    }
            //}

            //return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Phương thức mã hóa MD5
        /// </summary>
        /// <param name="txt">Chuỗi cần mã hóa</param>
        /// <returns></returns>
        private String GetMD5(string txt)
        {
            String str = "";
            Byte[] buffer = System.Text.Encoding.UTF8.GetBytes(txt);
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            buffer = md5.ComputeHash(buffer);
            foreach (Byte b in buffer)
            {
                str += b.ToString("X2");
            }
            return str;
        }

        /// <summary>
        /// Thay đổi kích thước ảnh
        /// </summary>
        /// <param name="img">Hình ảnh</param>
        /// <param name="width">Kích thước chiều ngang muốn thay đổi, ở đây sử dụng tỷ lệ cố định là 108</param>
        /// <returns></returns>
        public Image ResizeByWidth(Image img, int width = 108)
        {
            //// lấy chiều rộng và chiều cao ban đầu của ảnh
            //int originalW = img.Width;
            //int originalH = img.Height;

            //// lấy chiều rộng và chiều cao mới tương ứng với chiều rộng truyền vào của ảnh (nó sẽ giúp ảnh của chúng ta sau khi resize vần giứ được độ cân đối của tấm ảnh
            //int resizedW = width;
            //int resizedH = (originalH * resizedW) / originalW;

            int newSizeW = 108;
            int newSizeH = 144;

            // tạo một Bitmap có kích thước tương ứng với chiều rộng và chiều cao mới
            Bitmap bmp = new Bitmap(newSizeW, newSizeH);

            // tạo mới một đối tượng từ Bitmap
            Graphics graphic = Graphics.FromImage((Image)bmp);
            graphic.InterpolationMode = InterpolationMode.High;

            // vẽ lại ảnh với kích thước mới
            graphic.DrawImage(img, 0, 0, newSizeW, newSizeH);

            // gải phóng resource cho đối tượng graphic
            graphic.Dispose();

            // trả về anh sau khi đã resize
            return (Image)bmp;
        }

        /// <summary>
        /// Phương thức bỏ dấu chữ cái
        /// </summary>
        /// <param name="c">Chữ cái</param>
        /// <returns></returns>
        public char getCharacter(char c)
        {            
            switch (c)
            {
                case 'á': case 'à': case 'ả': case 'ã': case 'ạ': case 'â': case 'ă': case 'ấ': case 'ầ': case 'ẩ': case 'ẫ': case 'ậ': case 'ắ': case 'ằ': case 'ẳ': case 'ẵ': case 'ặ': c = 'a'; break;
                case 'é': case 'è': case 'ẻ': case 'ẽ': case 'ẹ': case 'ê': case 'ế': case 'ề': case 'ể': case 'ễ': case 'ệ': c = 'e'; break;
                case 'ó': case 'ò': case 'ỏ': case 'õ': case 'ọ': case 'ô': case 'ơ': case 'ố': case 'ồ': case 'ổ': case 'ỗ': case 'ộ': case 'ớ': case 'ờ': case 'ở': case 'ỡ': case 'ợ': c = 'o'; break;
                case 'ú': case 'ù': case 'ủ': case 'ũ': case 'ụ': case 'ư': case 'ứ': case 'ừ': case 'ử': case 'ữ': case 'ự': c = 'u'; break;
                case 'í': case 'ì': case 'ỉ': case 'ĩ': case 'ị': c = 'i'; break;
            }
            return c;
        }
    }
}