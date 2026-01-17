using EFCORE15.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для EditPage.xaml
    /// </summary>
    public partial class EditPage : Page
    {
        public ObservableCollection<Category> Categories { get; set; } = new();
        public ObservableCollection<Brand> Brands { get; set; } = new();
        public ObservableCollection<Tag> Tags { get; set; } = new();
        public ObservableCollection<Tag> ProductTags { get; set; } = new();

        CategoryService categoryService = new();
        BrandService brandService = new();
        TagService tagService = new();
        ProductService service = new();

        public Product Product { get; set; }
        public Tag? SelectedTag { get; set; }
        public Tag? SelectedProductTag { get; set; }

        bool IsEdit = false;
        public EditPage(Product product)
        {
            InitializeComponent();

            Product = service.Products
                .First(p => p.Id == product.Id);

            DBService.Instance.Context.Entry(Product)
                .Collection(p => p.Tags)
                .Load();

            LoadCategories();
            LoadBrands();

            DataContext = this;
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Title = "Редактирование товара";
            }
        }

        private void LoadList(object sender, RoutedEventArgs e)
        {
            Tags.Clear();
            ProductTags.Clear();

            tagService.GetAll();

            foreach (var t in tagService.Tags)
                Tags.Add(t);

            if (Product.Tags != null)
            {
                foreach (var t in Product.Tags)
                {
                    ProductTags.Add(t);
                    Tags.Remove(t);
                }
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

        private void Edit(object sender, RoutedEventArgs e)
        {
            // 1. Проверка на пустые поля (оставляем как было)
            if (string.IsNullOrEmpty(Name.Text) || string.IsNullOrEmpty(Description.Text) || string.IsNullOrEmpty(Price.Text)
                || string.IsNullOrEmpty(Stock.Text) || string.IsNullOrEmpty(Rating.Text))
            {
                MessageBox.Show("Заполните все поля", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            
            if (Validation.GetHasError(Name) ||
                Validation.GetHasError(Price) ||
                Validation.GetHasError(Stock) ||
                Validation.GetHasError(Rating) ||
                Validation.GetHasError(Description))
            {
                MessageBox.Show("Введены некорректные данные",
                    "Ошибка валидации",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return; 
            }

            if (Product.CategoryId == 0)
            {
                MessageBox.Show("Выберите категорию", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Product.BrandId == 0)
            {
                MessageBox.Show("Выберите бренд", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                service.Commit();
                MessageBox.Show("Изменения успешно сохранены", "Готово",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                Back(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            if (Product != null)
                if (MessageBox.Show(
                    "Вы действительно хотите удалить?",
                    "Удалить группу?",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    service.Remove(Product);
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

        private void AddTag(object sender, RoutedEventArgs e)
        {
            if (SelectedTag == null)
            {
                MessageBox.Show("Выберите тег", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Product.Tags ??= new List<Tag>();

            Product.Tags.Add(SelectedTag);
            ProductTags.Add(SelectedTag);
            Tags.Remove(SelectedTag);

            SelectedTag = null;
        }

        private void RemoveTag(object sender, RoutedEventArgs e)
        {
            if (SelectedProductTag == null)
            {
                MessageBox.Show("Выберите тег", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Product.Tags?.Remove(SelectedProductTag);
            Tags.Add(SelectedProductTag);
            ProductTags.Remove(SelectedProductTag);

            SelectedProductTag = null;
        }

     
    }
}
