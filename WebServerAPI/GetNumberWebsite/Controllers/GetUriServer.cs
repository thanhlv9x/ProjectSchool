using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GetNumberWebsite.Controllers
{
    public static class GetUriServer
    {
        /// <summary>
        /// Phương thức lấy địa chỉ host của server
        /// </summary>
        /// <returns></returns>
        public static string GetUri()
        {
            Uri myuri = (new Uri(System.Web.HttpContext.Current.Request.Url.AbsoluteUri));
            string pathQuery = myuri.PathAndQuery;
            string hostName = myuri.ToString().Replace(pathQuery, "") + "/";
            //hostName = "http://localhost:49930/";
            //string urlServer = hostName.Substring(0, hostName.IndexOf(myuri.Port.ToString()) - 1);
            return hostName;
        }
    }
}