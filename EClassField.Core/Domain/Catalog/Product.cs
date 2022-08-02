using EClassField.Core.Domain.Directory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EClassField.Core.Domain.User;
using EClassField.Core.Domain.Comment;
using System.Data.Entity;
using System.Data.Entity.Spatial;

namespace EClassField.Core.Domain.Catalog
{
    public class Product : BaseEntity<int>
    {

        
        public string Title { get; set; }
        public decimal Price { get; set; }
        public decimal OldPrce { get; set; }
        public decimal SpecialPrice { get; set; }
        public string Description { get; set; }
        public string MetaTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public bool IsPending { get; set; }
        public string VideoLink { get; set; }
        public string FaceLink { get; set; }

        public DateTime CreationTime { get; set; }
        public DateTime UpdateTme { get; set; }
        public virtual Country Country { get; set; }

        public int ILKOD { get; set; }
        public int ILCEKOD { get; set; }
        public int SEMTKOD { get; set; }
        public int MAHALLEKOD { get; set; }
        public virtual City City { get; set; }
        public virtual Town Town { get; set; }
        public virtual Area Area { get; set; }
        public virtual Neighborhood Neighborhood { get; set; }
        public string IL { get; set; }
        public string ILCE { get; set; }
        public string MAHALLE{ get; set; }
        public string BULVARCADDE { get; set; }
        public string SOKAK { get; set; }
       

        public string Latitude { get; set; }
        public string Longitude { get; set; }

        //public DbGeography Location { get; set; }
        public bool IsOrnek { get; set; }

        public string UserName { get; set; }
        public string Tel1 { get; set; }
        public string  Tel2 { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        private ICollection<ProductCategory> _productcategories;
        public virtual ICollection<ProductCategory> ProductCategories
        {
            
            get { return _productcategories ?? (_productcategories = new List<ProductCategory>());  }
            set { _productcategories = value; }
        }



        public virtual User.User User { get; set; }

        private ICollection<ProductPicture> _productpicture;
        public virtual ICollection<ProductPicture> ProductPictures
        {
            get { return _productpicture ?? (_productpicture = new List<ProductPicture>()); }
            set { _productpicture = value; }
        }


        private ICollection<ProductAttribute> _productAttributes;
        public virtual ICollection<ProductAttribute> ProductAttributes
        {
            get { return _productAttributes ?? (_productAttributes = new List<ProductAttribute>()); }
            set { _productAttributes = value; }
        }
        private ICollection<ProductComment> _productcomment;
        public virtual ICollection<ProductComment> ProductComments
        {
            get { return _productcomment ?? (_productcomment = new List<ProductComment>()); }
            set { _productcomment = value; }
        }
        public string    VenueId { get; set; }


        public Product()
        {
            IsPending = true;
            IsActive = false;
            
        }



    }





    public static class Geometry
    {

        public  static string ToWkt(this DbGeography geom)
        {
            if(geom!=null)
            return geom.WellKnownValue.WellKnownText;
            return "";
        }


        public static DbGeography ToWktGeometry(this string wkt)
        {
            DbGeography  geom=  DbGeography.FromText(wkt, 4326);

            return geom;
        
        }

    }

}