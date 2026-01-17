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
    /// Логика взаимодействия для EditTagPage.xaml
    /// </summary>
    public partial class EditTagPage : Page
    {
        Tag _tag = new();
        TagService service = new();
        bool IsEdit = false;
        public EditTagPage(Tag? tag = null)
        {
            InitializeComponent();

            if (tag != null)
            {
                _tag = tag;
                IsEdit = true;
            }
            DataContext = _tag;
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Title = "Редактирование";
            }
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            if (IsEdit)
                service.Commit();
            else
                service.Add(_tag);
            Back(sender, e);
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            if (_tag != null)
                if (MessageBox.Show(
                    "Вы хотите удалить?",
                    "Удалить?",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    service.Remove(_tag);
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
