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
using System.Windows.Shapes;

namespace VkMusicDownloader
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public string Token { get; private set; }
        private const string AUTH_URL = "https://oauth.vk.com/authorize";
        private const int ID = 5650763;
        private const string SCOPE = "audio,offline,users";
        private const string REDIRECT_URL = "https://oauth.vk.com/blank.html";

        public LoginWindow()
        {
            InitializeComponent();
            Uri AuthUri = new Uri(AUTH_URL + "?client_id=" + ID + "&display=page&response_type=token&scope=" + SCOPE);
            browser.Navigate(AuthUri);
        }

        private void browser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if ( e.Uri.AbsolutePath == "/blank.html" && e.Uri.Fragment.Contains("access_token"))
            {
                Char[] dels = { '#','&','=' };
                Token = e.Uri.Fragment.Split(dels)[2];
                this.Close();
            }
        }
    }
}
