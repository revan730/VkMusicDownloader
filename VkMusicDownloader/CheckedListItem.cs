using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace VkMusicDownloader
{
    /// <summary>
    /// Class which stores list item and handles it's changes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CheckedListItem<T> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private T item;
        private bool isChecked;

        public CheckedListItem() { }

        public CheckedListItem(T Item, bool IsChecked = false)
        {
            this.item = Item;
            this.isChecked = IsChecked;
        }

        public T Item
        {
            get { return item; }
            set
            {
                item = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Item"));

            }
        }

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("IsChecked"));
            }
        }
    }
}
