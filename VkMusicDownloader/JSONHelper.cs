using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.IO;

namespace VkMusicDownloader
{
    /// <summary>
    /// Helper class for easier JSON serialization
    /// </summary>
    class JSONHelper
    {
        public static User ReadUser(string JsonString)
        {
            DataContractJsonSerializer Serializer = new DataContractJsonSerializer(typeof(User));
            User Result = (User) Serializer.ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(JsonString)));

            return Result;
        }
    }
}
