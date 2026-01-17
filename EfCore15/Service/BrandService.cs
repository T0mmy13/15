using EFCORE15.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace EFCORE15.Service
{
    public class BrandService
    {
        private readonly ElectronicsStoreContext _db = DBService.Instance.Context;

        public ObservableCollection<Brand> Brands { get; set; } = new();

        public void GetAll()
        {
            var brands = _db.Brands.ToList();
            Brands.Clear();
            foreach (var brand in brands)
                Brands.Add(brand);
        }

        public BrandService()
        {
            GetAll();
        }

        public int Commit() => _db.SaveChanges();

        public void Add(Brand brand)
        {
            var name = brand.Name?.Trim();

            bool exists = _db.Brands
                .Any(c => c.Name.ToLower() == name.ToLower());

            if (exists)
            {
                MessageBox.Show("Бренд с таким названием уже существует");
                return;
            }

            var _brand = new Brand
            {
                Name = brand.Name,
            };

            _db.Add<Brand>(_brand);
            Commit();
            Brands.Add(_brand);
        }

        public void Remove(Brand brand)
        {
            bool hasProducts = _db.Products.Any(p => p.BrandId == brand.Id);

            if (hasProducts)
            {
                MessageBox.Show("Нельзя удалить бренд, в которой есть товары");
            }
            else
            {
                _db.Brands.Remove(brand);
                Commit();

                if (Brands.Contains(brand))
                    Brands.Remove(brand);
            }
        }
    }
}
