using EFCORE15.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using EFCORE15.Models;

namespace EFCORE15.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddProductPage.xaml
    /// </summary>
    public partial class AddProductPage : Page
    {
        public ElectronicsStoreContext db = DBService.Instance.Context;
        public ICollectionView tagsView { get; set; }
        public ObservableCollection<Category> Categories { get; set; } = new();
        public ObservableCollection<Brand> Brands { get; set; } = new();
        public ObservableCollection<Tag> Tags { get; set; } = new();
        public ObservableCollection<Tag> ProductTags { get; set; } = new();

        CategoryService categoryService = new();
        BrandService brandService = new();
        TagService tagService = new();
        ProductService service = new();

        public Tag? SelectedTag { get; set; }
        public Tag? SelectedProductTag { get; set; }
        public Product Product { get; set; }
        bool IsEdit = false;

        public AddProductPage(Product? product = null)
        {
            InitializeComponent();

            Product = product ?? new Product();

            LoadCategories();
            LoadBrands();

            IsEdit = product != null;

            DataContext = this;

            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Title = IsEdit ? "Редактирование товара" : "Добавление товара";
            }
        }

        private void LoadList(object sender, RoutedEventArgs e)
        {
            Tags.Clear();
            tagService.GetAll();

            foreach (var c in tagService.Tags)
                Tags.Add(c);

            if (Product.Tags != null)
            {
                ProductTags.Clear(); 
                foreach (var t in Product.Tags)
                {
                    ProductTags.Add(t);

                    var tagToRemove = Tags.FirstOrDefault(x => x.Id == t.Id);
                    if (tagToRemove != null)
                    {
                        Tags.Remove(tagToRemove);
                    }
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

        private void AddProduct(object sender, RoutedEventArgs e)
        {
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
                MessageBox.Show(
                    "Выберите категорию",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (Product.BrandId == 0)
            {
                MessageBox.Show(
                    "Выберите бренд",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            //try
            //{
                if (IsEdit)
                {
                    service.Commit();
                    MessageBox.Show("Изменения успешно сохранены", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    service.Add(Product);
                }

                Back(sender, e);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
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