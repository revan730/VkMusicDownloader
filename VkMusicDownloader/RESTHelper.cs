using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace VkMusicDownloader
{
    /// <summary>
    /// Helper class for HTTP REST requests
    /// </summary>
    class RESTHelper
    {
        /// <summary>
        /// Perform HTTP GET Request and return response in form of string
        /// </summary>
        /// <param name="Url">HTTP URL to request</param>
        /// <returns>string - response from remote server</returns>
        public static string GETRequest(string Url)
        {
            try
            {
                WebRequest Request = WebRequest.Create(Url);
                WebResponse Response = Request.GetResponse();
                Stream ResponseStream = Response.GetResponseStream();
                StreamReader Reader = new StreamReader(ResponseStream);
                string RawResponse = Reader.ReadToEnd();
                return RawResponse;
            }
            catch (WebException E)
            {
                throw new RESTException("Unable to perform GET request:" + E.Message);
            }
        }
    }
}
