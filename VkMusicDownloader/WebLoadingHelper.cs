using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;

namespace VkMusicDownloader
{
    /// <summary>
    /// Class which handles async audio file loading
    /// </summary>
    class WebLoadingHelper
    {
        public static async Task AsyncDownload(string FileName,string Url)
        {
            try
            {
                var uri = new Uri(Url);
                using (HttpClient Client = new HttpClient())
                {
                    using (var Response = await Client.GetAsync(uri))
                    {
                        Response.EnsureSuccessStatusCode();

                        using (var ResponseStream = await Response.Content.ReadAsStreamAsync())
                        {
                            var FileStream = File.Create(FileName);
                            var Reader = new StreamReader(ResponseStream);
                            ResponseStream.CopyTo(FileStream);
                            FileStream.Flush();
                        }
                    }
                }
            }

            catch (Exception E)
            {
                throw new VkAPIException("Unable to load file:" + E.Message);
            }
        }
    }
}
