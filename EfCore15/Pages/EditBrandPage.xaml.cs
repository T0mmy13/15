using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using EFCORE15.Models;
using EFCORE15.Service;

namespace EFCORE15.Pages
{
    public partial class EditBrandPage : Page
    {
        Brand _brand;
        BrandService service = new();
        bool IsEdit = false;

        public EditBrandPage(Brand? brand = null)
        {
            InitializeComponent();

            if (brand != null)
            {
                _brand = brand;
                IsEdit = true;
            }
            else
            {
                _brand = new Brand(); 
            }

            DataContext = _brand;

            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Title = IsEdit ? "Редактирование бренда" : "Новый бренд";
            }
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_brand.Name))
            {
                MessageBox.Show("Введите название");
                return;
            }

            if (IsEdit)
                service.Commit();
            else
                service.Add(_brand); 

            MessageBox.Show("сохранено");
            NavigationService.GoBack();
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            if (!IsEdit) return;

            var result = MessageBox.Show(
                $"Вы хотите удалить  '{_brand.Name}'?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    service.Remove(_brand);
                    MessageBox.Show("Бренд удален");
                    NavigationService.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении.\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}