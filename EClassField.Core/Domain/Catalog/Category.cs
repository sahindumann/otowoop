using EClassField.Core.Domain.Blog;
using EClassField.Core.Domain.Locazition;
using EClassField.Core.Domain.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core.Domain.Catalog
{
    public class Category : BaseEntity<int>, ILocazition
    {



        public string Name { get; set; }
        public int ParentCategoryId { get; set; }
        public string MetaTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public bool IsActive { get; set; }
        public bool IsRental { get; set; }
        public bool IsBlog { get; set; }
        public bool IsIlan { get; set; }
        public bool IsForum { get; set; }
        public bool IsOdev { get; set; }
        public bool IsFun { get; set; }
        public bool IsOtoKuafor { get; set; }
        public bool Deleted { get; set; }
        public int Order { get; set; }
        public string FullPath { get; set; }
        public DateTime CreationTime { get; set; }
        public virtual Picture Picture { get; set; }
        private ICollection<CategoryAttribute> _categoryAttributes;
        public virtual ICollection<CategoryAttribute> SubAttributes
        {
            get { return _categoryAttributes ?? (_categoryAttributes = new List<CategoryAttribute>()); }
            set { _categoryAttributes = value; }
        }


        private ICollection<CategorySubAttribute> _categorysubAttributes;
        public virtual ICollection<CategorySubAttribute> Sub_SubAttributes
        {
            get { return _categorysubAttributes ?? (_categorysubAttributes = new List<CategorySubAttribute>()); }
            set { _categorysubAttributes = value; }
        }



        private ICollection<CategoryProduct> _categoryProducts;
        public virtual ICollection<CategoryProduct> CategoryProducts
        {
            get { return _categoryProducts ?? (_categoryProducts = new List<CategoryProduct>()); }
            set { _categoryProducts = value; }
        }





    }
}
