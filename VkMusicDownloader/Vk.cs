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
        /// Get current Vk account owner's name in form of "FirstName LastName" string
        /// </summary>
        /// <returns>string - full name of account's owner</returns>
        public string GetAccountName()
        {
            try
            {
                string Response =  ExecuteMethod("users.get?");
                User Account = JSONHelper.ReadUser(Response);
                string Name = string.Format("{0} {1}",Account.FirstName,Account.LastName);

                return Name;
            }

            catch (Exception E)
            {
                throw new VkAPIException("Unable to get account name:" + E.Message);
            }
        }

        public string ExecuteMethod(string Method)
        {
            try
            {
                Char[] dels = { '[', ']' };
                string RawResponse = RESTHelper.GETRequest(API_URL + Method + "access_token=" + Token + "&v=" + VERSION);
                return RawResponse.Split(dels)[1];
            }

            catch (Exception E)
            {
                throw new VkAPIException("Unable to execute method:" + E.Message);
            }
        }

    }
}
