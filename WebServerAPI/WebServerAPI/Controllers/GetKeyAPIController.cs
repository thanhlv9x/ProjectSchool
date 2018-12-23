using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebServerAPI.EF;
using WebServerAPI.Models;

namespace WebServerAPI.Controllers
{
    public class GetKeyAPIController : ApiController
    {
        [HttpGet]
        public SMSKey Get()
        {
            SMSKey md = new SMSKey();
            using (HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities())
            {
                var EF = db.SMSKEYs.FirstOrDefault();
                if (EF != null)
                {
                    md.Id = EF.ID;
                    md.APIKey = EF.APIKEY;
                    md.SecretKey = EF.SECRETKEY;
                }
            }
            return md;
        }

        [HttpPost]
        public bool Post(SMSKey md)
        {
            bool success = false;
            using(HETHONGDANHGIAsaEntities db = new HETHONGDANHGIAsaEntities())
            {
                var EF = db.SMSKEYs.Where(p => p.ID == md.Id).FirstOrDefault();
                if (EF != null)
                {
                    EF.APIKEY = md.APIKey;
                    EF.SECRETKEY = md.SecretKey;
                }
                else
                {
                    SMSKEY ef = new SMSKEY()
                    {
                        APIKEY = md.APIKey,
                        SECRETKEY = md.SecretKey
                    };
                    db.SMSKEYs.Add(ef);
                }
                try
                {
                    db.SaveChanges();
                    success = true;
                }
                catch { }
            }
            return success;
        }

        [HttpGet]
        //Get Account Balance - Lay so du tai khoan
        public JObject GetBalance(string APIKey, string SecretKey)
        {
            string data = "http://rest.esms.vn/MainService.svc/json/GetBalance/" + APIKey + "/" + SecretKey + "";
            string result = SendGetRequest(data);
            JObject ojb = JObject.Parse(result);
            int CodeResult = (int)ojb["CodeResponse"];//trả về 100 là thành công
            int UserID = (int)ojb["UserID"];//id tài khoản
            long Balance = (long)ojb["Balance"];//tiền trong tài khoản
            return ojb;
        }

        [HttpPost]
        private string SendGetRequest(string RequestUrl)
        {
            Uri address = new Uri(RequestUrl);
            HttpWebRequest request;
            HttpWebResponse response = null;
            StreamReader reader;
            if (address == null) { throw new ArgumentNullException("address"); }
            try
            {
                request = WebRequest.Create(address) as HttpWebRequest;
                request.UserAgent = ".NET Sample";
                request.KeepAlive = false;
                request.Timeout = 15 * 1000;
                response = request.GetResponse() as HttpWebResponse;
                if (request.HaveResponse == true && response != null)
                {
                    reader = new StreamReader(response.GetResponseStream());
                    string result = reader.ReadToEnd();
                    result = result.Replace("</string>", "");
                    return result;
                }
            }
            catch (WebException wex)
            {
                if (wex.Response != null)
                {
                    using (HttpWebResponse errorResponse = (HttpWebResponse)wex.Response)
                    {
                        Console.WriteLine(
                            "The server returned '{0}' with the status code {1} ({2:d}).",
                            errorResponse.StatusDescription, errorResponse.StatusCode,
                            errorResponse.StatusCode);
                    }
                }
            }
            finally
            {
                if (response != null) { response.Close(); }
            }
            return null;
        }
    }
}
