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
        private const int ID = 5650763;
        private const string SCOPE = "audio,offline,";
        private const string REDIRECT_URL = "https://oauth.vk.com/blank.html";
        private string Token;
        private bool redirected = false;
        private const string AUTH_URL = "https://oauth.vk.com/authorize";
        private const string API_URL = "https://api.vk.com/method/";

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
        }

        /// <summary>
        /// Read OAuth token from file
        /// </summary>
        /// <returns>string - OAuth token</returns>
        private string ReadTokenFromFile()
        {
            try
            {
                string Token = System.IO.File.ReadAllText(FILE_NAME);
                return Token;
            }
            catch (FileNotFoundException)
            {
                throw new VkAPIException("Token file is missing");
            }
        }

        /// <summary>
        /// Get current Vk account owner's name in form of "FirstName LastName" string
        /// </summary>
        /// <returns>string - full name of account's owner</returns>
        public string GetAccountName()
        {
            try
            {
                string RawResponse =  RESTHelper.GETRequest(API_URL + "users.get?");
                User Account = JSONHelper.ReadUser(RawResponse);
                string Name = string.Format("{0} {1}",Account.FirstName,Account.LastName);

                return Name;
            }

            catch (Exception E)
            {
                throw new VkAPIException("Unable to get account name:" + E.Message);
            }
        }
    }
}
