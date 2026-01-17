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
    /// Логика взаимодействия для EditCategoryPage.xaml
    /// </summary>
    public partial class EditCategoryPage : Page
    {
        Category _category = new();
        CategoryService service = new();
        bool IsEdit = false;
        public EditCategoryPage(Category? category = null)
        {
            InitializeComponent();

            if (category != null)
            {
                _category = category;
                IsEdit = true;
            }
            DataContext = _category;
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Title = "Редактирование категории";
            }
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            if (IsEdit)
                service.Commit();
            else
                service.Add(_category);
            Back(sender, e);
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            if (_category != null)
                if (MessageBox.Show(
                    "Вы действительно хотите удалить?",
                    "Удалить группу?",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    service.Remove(_category);
                    Back(sender, e);
                }
                else
                    MessageBox.Show(
                        "Выберите для удаления",
                        "Выберите",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
