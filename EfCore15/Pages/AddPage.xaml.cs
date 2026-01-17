using EFCORE15.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
    /// Логика взаимодействия для AddPage.xaml
    /// </summary>
    public partial class AddPage : Page
    {
        public ElectronicsStoreContext db = DBService.Instance.Context;
        public ObservableCollection<Product> products { get; set; } = new();
        public ObservableCollection<Tag> tags { get; set; } = new();
        public ObservableCollection<Brand> brands { get; set; } = new();
        public ObservableCollection<Category> categories { get; set; } = new();

        public Category? category { get; set; } = null;
        public Tag? tag { get; set; } = null;
        public Brand? brand { get; set; } = null;

        public AddPage()
        {
            InitializeComponent();
            DataContext = this;
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Title = "Добавление/изменение";
            }

        }

        private void LoadList(object sender, RoutedEventArgs e)
        {
            categories.Clear();

            var loadedCategories = db.Categories
                .ToList();

            foreach (var category in loadedCategories)
                categories.Add(category);

            tags.Clear();

            var loadedTags = db.Tags
                .ToList();

            foreach (var tag in loadedTags)
                tags.Add(tag);

            brands.Clear();

            var loadedBrands = db.Brands
                .ToList();

            foreach (var brand in loadedBrands)
                brands.Add(brand);
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void CategoriesList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (category != null)
                NavigationService.Navigate(new EditCategoryPage(category));
            else
                MessageBox.Show("Выберите категорию");
        }

        private void TagsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (tag != null)
                NavigationService.Navigate(new EditTagPage(tag));
            else
                MessageBox.Show("Выберите тег");
        }

        private void BrandsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (brand != null)
                NavigationService.Navigate(new EditBrandPage(brand));
            else
                MessageBox.Show("Выберите бренд");
        }

        private void GoAddProductPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddProductPage());
        }

        private void GoAddCategoryPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddCategoryPage());
        }

        private void GoAddTagPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddTagPage());
        }

        private void GoAddBrandPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddBrandPage());
        }

       
      
    }
}
