using snmDB.Services;
using snmDB.Models;
using System.Windows;
using System.Windows.Navigation;

namespace snmDB
{
    public partial class LoginRegisterView : Window
    {
        private readonly UserService _userService;

        public LoginRegisterView(UserService userService)
        {
            InitializeComponent();
            _userService = userService;
        }
        public LoginRegisterView()
        {
            InitializeComponent();
            MongoDbService _mongoDbService = new MongoDbService("mongodb://localhost:27017/SocialNetworkDB", "SocialNetworkDB");
            _userService = new UserService(_mongoDbService);
        }


        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = LoginEmailTextBox.Text;
            string password = LoginPasswordBox.Password;

            var user = await _userService.GetUserByEmailAsync(email);
            if (user != null && user.Password == password)
            {
                NavigationWindow window = new NavigationWindow();
                window.Source = new Uri("Window1.xaml", UriKind.Relative);
                window.Show();
                this.Visibility = Visibility.Hidden;

            }
            else
            {
                MessageBox.Show("Invalid email or password.");
            }
        }


        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            MongoDbService _mongoDbService = new MongoDbService("mongodb://localhost:27017/SocialNetworkDB", "SocialNetworkDB");
            var _userService = new UserService(_mongoDbService); 
            var registrationView = new RegistrationView(_userService);
            registrationView.Show();

        }
    }
}
