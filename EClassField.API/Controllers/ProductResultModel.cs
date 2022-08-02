using EClassField.Core.Domain.Catalog;

namespace EClassField.API.Controllers
{
    internal class ProductResultModel
    {
        public string AttrName { get; set; }
        public object p { get; set; }
        public ProductAttributeValueNumber pval { get; set; }
        public ProductAttribute pattr { get; set; }
        public double Value { get; set; }
    }
}