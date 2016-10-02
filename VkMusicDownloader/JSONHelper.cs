using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VkMusicDownloader
{
    /// <summary>
    /// Helper class for easier JSON serialization
    /// </summary>
    class JSONHelper
    {
        /// <summary>
        /// Deserialize User object from JSON string
        /// </summary>
        /// <param name="JsonString">JSON string"</param>
        /// <returns>User object</returns>
        public static User ReadUser(string JsonString)
        {
            JObject json = JObject.Parse(JsonString);

            if (json != null && json["response"] != null)
            {
                User Result = JsonConvert.DeserializeObject<User>(json["response"][0].ToString());
                return Result;
            }

            else throw new VkAPIException(json["error"]["error_msg"].ToString());
        }

        /// <summary>
        /// Deserialize list of Album objects from JSON string
        /// </summary>
        /// <param name="JsonString">JSON string</param>
        /// <returns>List of Album type</returns>
        public static List<Album> ReadAlbums(string JsonString)
        {
            JObject json = JObject.Parse(JsonString);

            if (json != null && json["response"] != null)
            {
                List<Album> albums = new List<Album>();
                foreach (JObject Item in json["response"]["items"])
                {
                    albums.Add(JsonConvert.DeserializeObject<Album>(Item.ToString()));
                }

                return albums;
            }

            else throw new VkAPIException(json["error"]["error_msg"].ToString());
        }

        /// <summary>
        /// Deserialize list of Song objects from JSON string
        /// </summary>
        /// <param name="JsonString">JSON string</param>
        /// <returns></returns>
        public static List<Song> ReadSongs(string JsonString)
        {
            JObject json = JObject.Parse(JsonString);

            if (json != null && json["response"] != null)
            {
                List<Song> songs = new List<Song>();

                foreach (JObject Item in json["response"]["items"])
                {
                    songs.Add(JsonConvert.DeserializeObject<Song>(Item.ToString()));
                }

                return songs;
            }

            else throw new VkAPIException(json["error"]["error_msg"].ToString());
        }
    }
}
