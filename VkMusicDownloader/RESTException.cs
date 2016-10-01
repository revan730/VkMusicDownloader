using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkMusicDownloader
{
    /// <summary>
    /// Exception class to handle HTTP REST requests errors
    /// </summary>
    class RESTException: System.Exception
    {
        public RESTException(string message) : base(message) { }
    }
}
