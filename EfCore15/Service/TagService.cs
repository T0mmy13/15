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
    public class TagService
    {
        private readonly ElectronicsStoreContext _db = DBService.Instance.Context;

        public ObservableCollection<Tag> Tags { get; set; } = new();

        public void GetAll()
        {
            var tags = _db.Tags.ToList();
            Tags.Clear();
            foreach (var tag in tags)
                Tags.Add(tag);
        }

        public TagService()
        {
            GetAll();
        }

        public int Commit() => _db.SaveChanges();

        public void Add(Tag tag)
        {
            var name = tag.Name?.Trim();

            bool exists = _db.Tags
                .Any(c => c.Name.ToLower() == name.ToLower());

            if (exists)
            {
                MessageBox.Show("Тег с таким названием уже существует");
                return;
            }

            var _tag = new Tag
            {
                Name = tag.Name,
            };

            _db.Add<Tag>(_tag);
            Commit();
            Tags.Add(_tag);
        }

        public void Remove(Tag tag)
        {
            var productTags = _db.Set<Dictionary<string, object>>("ProductTag")
                .Where(pt => (int)pt["TagId"] == tag.Id)
                .ToList();

            _db.RemoveRange(productTags);

            _db.Tags.Remove(tag);

            Commit();

            if (Tags.Contains(tag))
                Tags.Remove(tag);
        }
    }
}
