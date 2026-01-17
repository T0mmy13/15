using EFCORE15.Service;
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
using EFCORE15.Models;

namespace EFCORE15.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddBrandPage.xaml
    /// </summary>
    public partial class AddBrandPage : Page
    {
        Brand _brand = new();
        BrandService service = new();
        bool IsEdit = false;
        public AddBrandPage(Brand? brand = null)
        {
            InitializeComponent();

            if (brand != null)
            {
                _brand = brand;
                IsEdit = true;
            }
            DataContext = _brand;
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Title = "Добавление бренда";
            }
        }

        private void AddBrand(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Name.Text))
            {
                MessageBox.Show("Введите название бренда");
            }
            else
            {
                if (IsEdit)
                    service.Commit();
                else
                    service.Add(_brand);
                Back(sender, e);
            }
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
