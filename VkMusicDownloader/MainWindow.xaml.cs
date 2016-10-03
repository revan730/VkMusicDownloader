using System;
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
using System.IO;
using System.Threading;

namespace VkMusicDownloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Vk VkApi;
        private List<Album> Albums;
        private CancellationTokenSource CTokenSource;
        public ObservableCollection<CheckedListItem<Song>> ListItems {get; set;}
        const string PATH_FILE = "last_download";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e) // Init Vk API class (check if token exists and it's usable, load account name etc.)
        {
            try
            {
                InitVk();
                LoadPath();
                SetAlbums();
            }
            catch (VkAPIException E)
            {
                ShowExceptionError(E);
            }
        }

        /// <summary>
        /// Set albums combobox items
        /// </summary>
        private void SetAlbums()
        {
            Albums = VkApi.GetAlbums();
            cb_albums.Items.Add(new ComboBoxItemAlbum("Все", 0));
            foreach (Album a in Albums)
            {
                cb_albums.Items.Add(new ComboBoxItemAlbum(a.Title, a.Id));
            }

            cb_albums.SelectedIndex = 0;
        }

        /// <summary>
        /// Set account's owner name in menu
        /// </summary>
        private void SetAccountName()
        {
            menu_vk_name.Header += " - " + VkApi.GetAccountName();
        }

        /// <summary>
        /// Call login window and wait for it to return access token
        /// </summary>
        /// <returns>OAuth token</returns>
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
        /// Log out of Vk account (Delete token and reset Vk API object) and stop program execution
        /// </summary>
        private void LogOut()
        {
            VkApi.ResetToken();
            Application.Current.Shutdown();
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

        /// <summary>
        /// Save last file download path 
        /// </summary>
        private void SavePath()
        {
            try
            {
                string path = tb_dest.Text;
                File.WriteAllText(PATH_FILE,path);
            }
            catch (IOException E)
            {
                ShowExceptionError(E);
            }

        }

        /// <summary>
        /// Load last download path from file and set in in textbox
        /// </summary>
        private void LoadPath()
        {
            try
            {
                string path = File.ReadAllText(PATH_FILE);
                tb_dest.Text = path;
            }

            catch (IOException) { }
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

        private async void btn_load_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListItems != null && ListItems.Where(item => item.IsChecked == true).Count() > 0)
                {
                    if (Directory.Exists(tb_dest.Text))
                    {
                        DeactivateInputs();
                        CTokenSource = new CancellationTokenSource();

                        await LoadFiles(tb_dest.Text, new Progress<int>(percent => pb_songs_loaded.Value = percent), CTokenSource.Token);

                        pb_songs_loaded.Visibility = Visibility.Hidden;
                        ActivateInputs();
                        SavePath();
                    }

                    else ShowError("Вы не ввели путь к директории загрузки,либо он не верный");
                }
                else ShowError("Вы не выбрали песни для загрузки");
            }
            catch (Exception E)
            {
                if (E is VkAPIException)
                    ShowExceptionError(E);
                else if (E is OperationCanceledException)
                {
                    ActivateInputs();
                }
            }
        }

        /// <summary>
        /// Async task for file download
        /// </summary>
        /// <param name="Destination"> Path to downloaded files</param>
        /// <param name="Progress">Provider for progress updates, invokes after every downloaded file</param>
        /// <param name="CToken">Cancellation token to check if task must be stopped</param>
        /// <returns>Number of downloaded files</returns>
        private async Task<int> LoadFiles(string Destination ,IProgress<int> Progress,CancellationToken CToken)
        {
            var Total = ListItems.Where(item => item.IsChecked == true).Count();
            var Downloaded = await Task.Run<int>(async () =>
                {
                    var Count = 0;
                    foreach (CheckedListItem<Song> S in ListItems.Where(item => item.IsChecked == true))
                    {
                        await WebLoadingHelper.AsyncDownload(Destination + S.Item.Name, S.Item.Url,CToken);
                        if (Progress != null)
                            Progress.Report((Count * 100 / Total));

                        Count++;
                    }
                    return Count;
                });
            return Downloaded;
        }

        /// <summary>
        /// Disable UI inputs
        /// </summary>
        private void DeactivateInputs()
        {
            pb_songs_loaded.Visibility = Visibility.Visible;
            tb_dest.IsEnabled = false;
            cb_albums.IsEnabled = false;
            lb_songs.IsEnabled = false;
            btn_load.Visibility = Visibility.Hidden;
            btn_stop.Visibility = Visibility.Visible;
            menu_vk_logout.IsEnabled = false;
        }

        /// <summary>
        /// Enable UI Inputs
        /// </summary>
        private void ActivateInputs()
        {
            pb_songs_loaded.Visibility = Visibility.Hidden;
            tb_dest.IsEnabled = true;
            cb_albums.IsEnabled = true;
            lb_songs.IsEnabled = true;
            btn_load.Visibility = Visibility.Visible;
            btn_stop.Visibility = Visibility.Hidden;
            menu_vk_logout.IsEnabled = true;
        }

        /// <summary>
        /// Show MessageBox with error message
        /// </summary>
        /// <param name="Error">Error message</param>
        private void ShowError(string Error)
        {
            MessageBox.Show(Error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Show error if exception occured
        /// </summary>
        /// <param name="E">Occcured exception</param>
        private void ShowExceptionError(Exception E)
        {
            ShowError("Ошибка работы с сетью: " + E.Message);
        }

        private void btn_stop_Click(object sender, RoutedEventArgs e)
        {
            if (CTokenSource != null)
                CTokenSource.Cancel();
        }
    }
}
