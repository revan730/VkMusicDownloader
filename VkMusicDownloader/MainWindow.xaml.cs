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

namespace VkMusicDownloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Vk VkApi;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e) // Init Vk API class (check if token exists and it's usable, load account name etc.)
        {
            try
            {
                VkApi = new Vk(); // Try to init object with token stored in file
            }
            catch (VkAPIException)
            {
                string Token = Authorize();
                VkApi = new Vk(Token);
            }

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
    }
}
