using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace VkMusicDownloader
{
    /// <summary>
    /// Class which stores user's credentials
    /// </summary>
    [DataContract]
    class User
    {
        [DataMember(Name = "first_name")]
        public string FirstName{ get; set; }

        [DataMember(Name = "last_name")]
        public string LastName{ get; set; }

        [DataMember(Name = "id")]
        public int Id { get; set; }
    }
}
