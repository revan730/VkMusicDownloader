using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkMusicDownloader
{
    /// <summary>
    /// Helper class for easy interaction with Vkontakte public API
    /// </summary>
    class Vk
    {
        private string FILE_NAME = "token";
        private string Token;
        private const string API_URL = "https://api.vk.com/method/";
        private const string VERSION = "5.57";

        /// <summary>
        /// Default constructor which reads token from file
        /// </summary>
        public Vk()
        {
            Token = ReadTokenFromFile();
        }

        /// <summary>
        /// Constructor overload which excepts Vk API OAuth token as parameter
        /// </summary>
        /// <param name="Token">string - OAuth token</param>
        public Vk(String Token)
        {
            this.Token = Token;
            WriteTokenToFile();
        }

        /// <summary>
        /// Read OAuth token from file
        /// </summary>
        /// <returns>string - OAuth token</returns>
        private string ReadTokenFromFile()
        {
            try
            {
                string Token = File.ReadAllText(FILE_NAME);
                return Token;
            }
            catch (FileNotFoundException)
            {
                throw new VkAPIException("Token file is missing");
            }
        }

        /// <summary>
        /// Write OAuth token to file
        /// </summary>
        private void WriteTokenToFile()
        {
            try
            {
                File.WriteAllText(FILE_NAME,Token);
            }
            catch (Exception E)
            {
                throw new VkAPIException("Unable to write token to file:" + E.Message);
            }
        }
        /// <summary>
        /// Remove token file 
        /// </summary>
        public void ResetToken()
        {
            File.Delete(FILE_NAME);
        }

        /// <summary>
        /// Get current Vk account owner's name in form of "FirstName LastName" string
        /// </summary>
        /// <returns>string - full name of account's owner</returns>
        public string GetAccountName()
        {
            try
            {
                string Response =  ExecuteMethod("users.get");
                User Account = JSONHelper.ReadUser(Response);
                string Name = string.Format("{0} {1}",Account.FirstName,Account.LastName);

                return Name;
            }

            catch (Exception E)
            {
                throw new VkAPIException("Unable to get account name:" + E.Message);
            }
        }
        /// <summary>
        /// Get list of Album objects
        /// </summary>
        /// <returns>List of Album type</returns>
        public List<Album> GetAlbums()
        {
            try
            {
                string Response = ExecuteMethod("audio.getAlbums");
                List<Album> Albums = JSONHelper.ReadAlbums(Response);

                return Albums;

            }

            catch (Exception E)
            {
                throw new VkAPIException("Unable to get albums:" + E.Message);
            }
        }

        /// <summary>
        /// Get list of all Song objects in album
        /// </summary>
        /// <param name="AlbumId">Id of Vkontakte audio album</param>
        /// <returns></returns>
        public List<Song> GetSongs(int AlbumId)
        {
            try
            {
                string Response;

                if (AlbumId == 0)
                    Response = ExecuteMethod("audio.get");
                else
                    Response = ExecuteMethod("audio.get", "album_id=" + AlbumId);
                List<Song> Songs = JSONHelper.ReadSongs(Response);

                return Songs;
            }

            catch (Exception E)
            {
                throw new VkAPIException("Unable to get songs:" + E.Message);
            }
        }
        /// <summary>
        /// Wraps method call with api url and token parameter
        /// </summary>
        /// <param name="Method"> Vk API method name </param>
        /// <param name="Params">Method parameters</param>
        /// <returns>Vk API response string</returns>
        private string ExecuteMethod(string Method, string Params="")
        {
            try
            {
                Char[] dels = { '[', ']' };
                string RawResponse = RESTHelper.GETRequest(API_URL + Method + "?access_token=" + Token + "&v=" + VERSION + "&" + Params);
                return RawResponse;
            }

            catch (Exception E)
            {
                throw new VkAPIException("Unable to execute method:" + E.Message);
            }
        }

    }
}
