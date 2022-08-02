using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EClassFieldWeb_.Models.ViewModel
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }

        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime UpdateTime { get; set; }

        public List<Input> Pictures { get; set; }

        public List<Input> Tags { get; set; }

        public List<Input> Categories { get; set; }
    }
}