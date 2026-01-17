using EFCORE15.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public ElectronicsStoreContext db = DBService.Instance.Context;
        public ObservableCollection<Product> products { get; set; } = new();
        public ObservableCollection<Category> Categories { get; set; } = new();
        public ObservableCollection<Brand> Brands { get; set; } = new();
        public ICollectionView productsView { get; set; }

        CategoryService categoryService = new();
        BrandService brandService = new();
        TagService tagService = new();
        ProductService service = new();

        public Product? product { get; set; } = null;

        public string searchQuery { get; set; } = null!;
        public string filterPriceFrom { get; set; } = null!;
        public string filterPriceTo { get; set; } = null!;

        public int? SelectedCategoryId { get; set; }
        public int? SelectedBrandId { get; set; }

        // Храним текущие направления сортировки
        private ListSortDirection? _nameSortDirection;
        private ListSortDirection? _priceSortDirection;
        private ListSortDirection? _stockSortDirection;

        bool _manager = false;
        public MainPage(bool manager)
        {
            productsView = CollectionViewSource.GetDefaultView(products);
            productsView.Filter = FilterForms;
            InitializeComponent();
            LoadCategories();
            LoadBrands();
            if (manager)
            {
                managerAddButton.Visibility = Visibility.Visible;
                _manager = true;
            }
            else
            {
                managerAddButton.Visibility = Visibility.Collapsed;
            }
            DataContext = this;
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Title = "Товары электроники";
            }
        }

        private void LoadCategories()
        {
            Categories.Clear();
            categoryService.GetAll();

            foreach (var c in categoryService.Categories)
                Categories.Add(c);
        }

        private void LoadBrands()
        {
            Brands.Clear();
            brandService.GetAll();

            foreach (var b in brandService.Brands)
                Brands.Add(b);
        }

        public bool FilterForms(object obj)
        {
            if (obj is not Product)
                return false;

            var product = (Product)obj;

            if (searchQuery != null && !product.Name.Contains(searchQuery, StringComparison.CurrentCultureIgnoreCase))
                return false;

            if (SelectedCategoryId.HasValue && SelectedCategoryId > 0 && product.CategoryId != SelectedCategoryId.Value)
                return false;

            if (SelectedBrandId.HasValue && SelectedBrandId > 0 && product.BrandId != SelectedBrandId.Value)
                return false;

            if (!string.IsNullOrEmpty(filterPriceFrom))
            {
                if (decimal.TryParse(filterPriceFrom, out decimal minPrice))
                {
                    if (minPrice > product.Price)
                        return false;
                }
                else
                {
                    return false;
                }
            }
            if (!string.IsNullOrEmpty(filterPriceTo))
            {
                if (decimal.TryParse(filterPriceTo, out decimal maxPrice))
                {
                    if (maxPrice < product.Price)
                        return false;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private void LoadList(object sender, RoutedEventArgs e)
        {
            LoadCategories();
            LoadBrands();
            products.Clear();

            var loadedProducts = db.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Tags)
                .ToList();
            foreach (var product in loadedProducts)
            {

                products.Add(product);
            }
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
            searchText.Text = string.Empty;
            priceFromText.Text = string.Empty;
            priceToText.Text = string.Empty;

            PriceComboBox.SelectedIndex = -1;
            StockComboBox.SelectedIndex = -1;
            SortNameComboBox.SelectedIndex = -1;
            CategoriesComboBox.SelectedIndex = -1;
            BrandsComboBox.SelectedIndex = -1;

            searchQuery = null;
            filterPriceFrom = null;
            filterPriceTo = null;
            SelectedCategoryId = null;
            SelectedBrandId = null;

            // Сброс сортировок
            _nameSortDirection = null;
            _priceSortDirection = null;
            _stockSortDirection = null;

            productsView.SortDescriptions.Clear();
            productsView.Refresh();
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPage());
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditPage(product));
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchQuery = searchText.Text;
            productsView.Refresh();
        }

        private void FilterPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterPriceFrom = priceFromText.Text;
            filterPriceTo = priceToText.Text;
            productsView.Refresh();
        }

        private void Price_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (productsView == null)
                return;

            var cb = sender as ComboBox;
            if (cb?.SelectedItem is not ComboBoxItem selected)
            {
                // Если ничего не выбрано, сбрасываем сортировку по цене
                _priceSortDirection = null;
                ApplyAllSortDescriptions();
                return;
            }

            var selectedSort = selected.Tag?.ToString();

            // Определяем направление сортировки
            ListSortDirection? newDirection = null;
            switch (selectedSort)
            {
                case "ascendingPrice":
                    newDirection = ListSortDirection.Ascending;
                    break;
                case "descendingPrice":
                    newDirection = ListSortDirection.Descending;
                    break;
            }

            // Если выбрана та же сортировка (то же направление), сбрасываем ее
            if (_priceSortDirection == newDirection)
            {
                _priceSortDirection = null;
                cb.SelectedIndex = -1;
            }
            else
            {
                _priceSortDirection = newDirection;
            }

            ApplyAllSortDescriptions();
        }

        private void Stock_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (productsView == null)
                return;

            var cb = sender as ComboBox;
            if (cb?.SelectedItem is not ComboBoxItem selected)
            {
                // Если ничего не выбрано, сбрасываем сортировку по количеству
                _stockSortDirection = null;
                ApplyAllSortDescriptions();
                return;
            }

            var selectedSort = selected.Tag?.ToString();

            // Определяем направление сортировки
            ListSortDirection? newDirection = null;
            switch (selectedSort)
            {
                case "ascendingStock":
                    newDirection = ListSortDirection.Ascending;
                    break;
                case "descendingStock":
                    newDirection = ListSortDirection.Descending;
                    break;
            }

            // Если выбрана та же сортировка (то же направление), сбрасываем ее
            if (_stockSortDirection == newDirection)
            {
                _stockSortDirection = null;
                cb.SelectedIndex = -1;
            }
            else
            {
                _stockSortDirection = newDirection;
            }

            ApplyAllSortDescriptions();
        }

        private void SortName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (productsView == null) return;
            var cb = sender as ComboBox;
            if (cb?.SelectedItem is not ComboBoxItem selected)
            {
                // Если ничего не выбрано, сбрасываем сортировку по названию
                _nameSortDirection = null;
                ApplyAllSortDescriptions();
                return;
            }

            var selectedSort = selected.Tag?.ToString();

            // Определяем направление сортировки
            ListSortDirection? newDirection = null;
            switch (selectedSort)
            {
                case "nameAZ":
                    newDirection = ListSortDirection.Ascending;
                    break;
                case "nameZA":
                    newDirection = ListSortDirection.Descending;
                    break;
            }

            // Если выбрана та же сортировка (то же направление), сбрасываем ее
            if (_nameSortDirection == newDirection)
            {
                _nameSortDirection = null;
                cb.SelectedIndex = -1;
            }
            else
            {
                _nameSortDirection = newDirection;
            }

            ApplyAllSortDescriptions();
        }

        private void ApplyAllSortDescriptions()
        {
            productsView.SortDescriptions.Clear();

            // Применяем сортировки в порядке приоритета: 1) Название, 2) Цена, 3) Количество
            if (_nameSortDirection.HasValue)
            {
                productsView.SortDescriptions.Add(
                    new SortDescription("Name", _nameSortDirection.Value));
            }

            if (_priceSortDirection.HasValue)
            {
                productsView.SortDescriptions.Add(
                    new SortDescription("Price", _priceSortDirection.Value));
            }

            if (_stockSortDirection.HasValue)
            {
                productsView.SortDescriptions.Add(
                    new SortDescription("Stock", _stockSortDirection.Value));
            }

            productsView.Refresh();
        }

        private void CategoriesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoriesComboBox.SelectedValue != null && int.TryParse(CategoriesComboBox.SelectedValue.ToString(), out int categoryId))
                SelectedCategoryId = categoryId;
            else
                SelectedCategoryId = null;

            productsView.Refresh();
        }

        private void BrandsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BrandsComboBox.SelectedValue != null && int.TryParse(BrandsComboBox.SelectedValue.ToString(), out int brandId))
                SelectedBrandId = brandId;
            else
                SelectedBrandId = null;

            productsView.Refresh();
        }

        private void ProductsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_manager)
            {
                if (product != null)
                    NavigationService.Navigate(new EditPage(product));
                else
                    MessageBox.Show("Выберите товар");
            }
        }
    }
}