using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkMusicDownloader
{
    class ComboBoxItemAlbum
    {
        public string Name { get; set; }
        public int Id { get; set; }


        public ComboBoxItemAlbum(string Name, int Id)
        {
            this.Name = Name;
            this.Id = Id;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
