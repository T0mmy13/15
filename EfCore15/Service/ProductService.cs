using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using EFCORE15.Models;

namespace EFCORE15.Service
{
    public class ProductService
    {
        private readonly ElectronicsStoreContext _db = DBService.Instance.Context;

        public ObservableCollection<Product> Products { get; set; } = new();

        public void GetAll()
        {
            var products = _db.Products.ToList();
            Products.Clear();
            foreach (var product in products)
                Products.Add(product);
        }

        public ProductService()
        {
            GetAll();
        }

        public int Commit() => _db.SaveChanges();

        public void Add(Product product)
        {
            var name = product.Name?.Trim();

            bool exists = _db.Products
                .Any(c => c.Name.ToLower() == name.ToLower());

            if (exists)
            {
                MessageBox.Show("Товар с таким названием уже существует");
            }
            else
            {
                _db.Products.Add(product);
                Commit();
                Products.Add(product);
                MessageBox.Show("Продукт успешно добавлен", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void Remove(Product product)
        {
            _db.Entry(product)
               .Collection(p => p.Tags)
               .Load();

            product.Tags.Clear();

            _db.Products.Remove(product);
            Commit();

            Products.Remove(product);
        }
    }
}
