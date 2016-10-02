using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace VkMusicDownloader
{
    /// <summary>
    /// Class which stores song information
    /// </summary>
    [DataContract]
    public class Song
    {
        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "artist")]
        public string Artist { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        public string Name
        {
            get
            {
                return Artist + " - " + Title;
            }
        }
    }
}
