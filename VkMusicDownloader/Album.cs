using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace VkMusicDownloader
{
    /// <summary>
    /// Class which stores album's info (id,title)
    /// </summary>
    [DataContract]
    class Album
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }
    }
}
