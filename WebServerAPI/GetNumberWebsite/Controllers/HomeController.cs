using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using GetNumberWebsite.Models;
using Newtonsoft.Json.Linq;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using RawPrint;

namespace GetNumberWebsite.Controllers
{
    public class HomeController : Controller
    {
        public static string basePath;

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Lấy thông tin bộ phận và số thứ tự
        /// </summary>
        /// <returns></returns>
        public JsonResult GetBP()
        {
            // "http://localhost:61443"
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(GetUriServer.GetUri());
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync("api/GetNumberAPI/").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        return Json(response.Content.ReadAsStringAsync().Result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Lấy số thứ tự theo mã bộ phận
        /// </summary>
        /// <param name="_MaBP">Mã bộ phận</param>
        /// <param name="_TenBP">Tên bộ phận</param>
        /// <param name="_PrinterName">Tên máy in</param>
        /// <returns></returns>
        public JsonResult GetSTT(int _MaBP, string _TenBP, string _PrinterName)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(GetUriServer.GetUri());
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync("api/GetNumberAPI/?_MaBP=" + _MaBP).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var value = response.Content.ReadAsStringAsync().Result;
                        createFile(value, _TenBP);
                        convertTextToPDF();
                        printPDF(_PrinterName);
                        //var files = Directory.EnumerateFiles(@"C:\Windows\System32\config\systemprofile\Documents", "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".pdf"));
                        //foreach (var item in files)
                        //{
                        //    System.IO.File.Delete(item);
                        //}
                        return Json(value, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Phương thức lấy danh sách máy in
        /// </summary>
        /// <returns></returns>
        public JsonResult GetPrinterName()
        {
            List<string> listPrinter = new List<string>();
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                listPrinter.Add(printer);
            }
            return Json(listPrinter, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Phương thức in file PDF trực tiếp
        /// </summary>
        public void printPDF(string _PrinterName)
        {
            // Absolute path to your PDF to print (with filename)
            string Filepath = basePath + @"\phieu-thu-tu.pdf";
            // The name of the PDF that will be printed (just to be shown in the print queue)
            string Filename = "phieu-thu-tu";
            // The name of the printer that you want to use
            // Note: Check step 1 from the B alternative to see how to list
            // the names of all the available printers with C#
            //string PrinterName = "Microsoft Print to PDF";
            string PrinterName = _PrinterName;

            // Create an instance of the Printer
            //IPrinter printer = new Printer();
            Printer.PrintFile(PrinterName, Filepath);
            // Print the file
            //printer.PrintRawFile(PrinterName, Filepath, Filename);
        }
        /// <summary>
        /// Phương thức tạo và ghi file
        /// </summary>
        /// <param name="_Number">Số thứ tự</param>
        /// <param name="_BoPhan">Tên bộ phận</param>
        public void createFile(string _Number, string _BoPhan)
        {
            DateTime now = DateTime.Now;
            DateTime printNow = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            basePath = AppDomain.CurrentDomain.BaseDirectory;
            string path = basePath + @"\phieu-thu-tu.txt";
            if (!System.IO.File.Exists(path))
            {
                System.IO.File.Create(path);
            }
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                sw.WriteLine("TP HỒ CHÍ MINH");
                sw.WriteLine("UBND QUẬN TÂN BÌNH");
                sw.WriteLine("==================================");
                sw.WriteLine("SỐ THỨ TỰ");
                sw.WriteLine(_Number);
                sw.WriteLine("BỘ PHẬN");
                sw.WriteLine(_BoPhan);
                sw.WriteLine("QUÝ KHÁCH VUI LÒNG CHỜ");
                sw.WriteLine("SỐ PHIẾU CỦA QUÝ KHÁCH SẼ ĐƯỢC GỌI KHI ĐẾN LƯỢT");
                sw.WriteLine("==================================");
                sw.WriteLine(now);
            }
        }
        /// <summary>
        /// Chuyển file text thành pdf
        /// </summary>
        public void convertTextToPDF()
        {
            try
            {
                // xPoint khoảng cách theo chiều ngang
                // yPoint khoảng cách theo chiều dọc
                string line = null;
                int yPoint = 0;
                System.IO.TextReader readFile = new StreamReader(basePath + @"\phieu-thu-tu.txt");
                PdfSharp.Pdf.PdfDocument pdf = new PdfSharp.Pdf.PdfDocument();
                PdfPage pdfPage = pdf.AddPage();
                XGraphics graph = XGraphics.FromPdfPage(pdfPage);
                XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);
                XFont font = new XFont("Verdana", 10, XFontStyle.Regular, options);

                while (true)
                {
                    line = readFile.ReadLine();
                    if (line == null)
                    {
                        break;
                    }
                    else
                    {
                        graph.DrawString(line, font, XBrushes.Black, new XRect(10, yPoint, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                        yPoint = yPoint + 20;
                    }
                }
                pdf.Save(basePath + @"\phieu-thu-tu.pdf");
                readFile.Close();
                readFile = null;
            }
            catch { }
        }
    }
}