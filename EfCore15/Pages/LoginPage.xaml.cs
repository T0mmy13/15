using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace EFCORE15.Pages
{
    public partial class LoginPage : Page
    {
        private readonly string pinCode = "1234";

        public LoginPage()
        {
            InitializeComponent();
            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.Title = "Авторизация";
        }

        private void UserLogin(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage(false));
        }

        private void CheckPinAndLogin(object sender, RoutedEventArgs e)
        {
            if (PinCodeField.Password == pinCode)
            {
                NavigationService.Navigate(new MainPage(true));
            }
            else
            {
                MessageBox.Show("Неверный код", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                PinCodeField.Password = "";
            }
        }

        private void BackToRoles(object sender, RoutedEventArgs e)
        {
            PinCodePanel.Visibility = Visibility.Collapsed;
            RoleSelectionPanel.Visibility = Visibility.Visible;
            PinCodeField.Password = "";
        }
    }
}