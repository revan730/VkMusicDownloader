﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace VkMusicDownloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Vk VkApi;
        private List<Album> Albums;
        public ObservableCollection<CheckedListItem<Song>> ListItems {get; set;}

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e) // Init Vk API class (check if token exists and it's usable, load account name etc.)
        {
            InitVk();
            SetAlbums();
        }

        private void SetAlbums()
        {
            Albums = VkApi.GetAlbums();
            cb_albums.Items.Add(new ComboBoxItemAlbum("Все", 0));
            foreach (Album a in Albums)
            {
                cb_albums.Items.Add(new ComboBoxItemAlbum(a.Title, a.Id));
            }
        }

        private void SetAccountName()
        {
            menu_vk_name.Header += " - " + VkApi.GetAccountName();
        }

        /// <summary>
        /// Call login window and wait for it to return access token
        /// </summary>
        /// <returns>string - OAuth token</returns>
        private string Authorize()
        {
            LoginWindow Login = new LoginWindow();
            Login.ShowDialog();

            return Login.Token;
        }
        /// <summary>
        /// Initialize Vk Api object
        /// </summary>
        private void InitVk()
        {
            try
            {
                VkApi = new Vk(); // Try to init object with token stored in file
            }
            catch (VkAPIException)
            {
                var Token = Authorize();
                VkApi = new Vk(Token);
            }

            SetAccountName();
            menu_vk_logout.IsEnabled = true;
        }

        /// <summary>
        /// Log out of Vk account (Delete token and reset Vk API object)
        /// </summary>
        private void LogOut()
        {
            VkApi.ResetToken();
            menu_vk_name.Header = "Аккаунт";
            InitVk();
        }
        /// <summary>
        /// Load all songs from album
        /// </summary>
        /// <param name="AlbumId">Album's Vkontakte identifier</param>
        private void LoadAlbum(int AlbumId)
        {
            var Songs = VkApi.GetSongs(AlbumId);
            ListItems = new ObservableCollection<CheckedListItem<Song>>();
            foreach (Song s in Songs)
            {
                ListItems.Add(new CheckedListItem<Song>(s));
            }

            DataContext = null;
            DataContext = this;
        }

        private void menu_vk_logout_Click(object sender, RoutedEventArgs e)
        {
            menu_vk_logout.IsEnabled = false;
            LogOut();
        }

        private void cb_albums_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var Item = (ComboBoxItemAlbum) e.AddedItems[0];
            LoadAlbum(Item.Id);
        }

    }
}
