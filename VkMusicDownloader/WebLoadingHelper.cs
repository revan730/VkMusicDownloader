using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Threading;

namespace VkMusicDownloader
{
    /// <summary>
    /// Class which handles async audio file loading
    /// </summary>
    class WebLoadingHelper
    {
        /// <summary>
        /// Download mp3 file asynchronously
        /// </summary>
        /// <param name="FilePath">File name and path </param>
        /// <param name="Url">File URL</param>
        /// <param name="CToken">Cancellation token to check if task must be stopped</param>
        /// <returns></returns>
        public static async Task AsyncDownload(string FilePath,string Url,CancellationToken CToken)
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
                            CToken.ThrowIfCancellationRequested();
                            var FileStream = File.Create(FilePath + ".mp3");
                            var Reader = new StreamReader(ResponseStream);
                            ResponseStream.CopyTo(FileStream);
                            FileStream.Flush();
                            FileStream.Close();
                            Reader.Close();
                        }
                    }
                }
            }

            catch (Exception E)
            {
                if (E is HttpRequestException || E is IOException)
                throw new VkAPIException("Unable to load file:" + E.Message);
            }
        }
    }
}
