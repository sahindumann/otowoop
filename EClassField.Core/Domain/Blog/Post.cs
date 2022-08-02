using EClassField.Core.Domain.Catalog;
using EClassField.Core.Domain.Locazition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core.Domain.Blog
{
    public class Post : BaseEntity<int>, ILocazition
    {

        public string Title { get; set; }
        public string Description { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }

        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime UpdateTime { get; set; }

        private ICollection<PostPicture> _postPictures;
        public virtual ICollection<PostPicture> PostPictures
        {
            get { return _postPictures ?? (_postPictures = new List<PostPicture>()); }
            set { _postPictures = value; }
        }



        private ICollection<PostTag> _posttags;
        public virtual ICollection<PostTag> PostTags
        {
            get { return _posttags ?? (_posttags = new List<PostTag>()); }
            set { _posttags = value; }
        }

        private ICollection<PostLanguage> _postlanguages;
        public virtual ICollection<PostLanguage> PostLanguages
        {
            get { return _postlanguages ?? (_postlanguages = new List<PostLanguage>()); }
            set { _postlanguages = value; }
        }

        private ICollection<CategoryPost> _postcategories;
        public virtual ICollection<CategoryPost> PostCategories
        {
            get { return _postcategories ?? (_postcategories = new List<CategoryPost>()); }
            set { _postcategories = value; }
        }


    }
}
