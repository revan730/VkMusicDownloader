using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkMusicDownloader
{
    /// <summary>
    /// Exception class to catch Vk API errors
    /// </summary>
    class VkAPIException: System.Exception
    {
        public VkAPIException(string message) : base(message) { }
    }
}
