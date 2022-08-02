using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EClassField.Core.Domain.User;

namespace EClassFieldWeb_.Models.ViewModel
{
    public class ProductModelView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public int ILKOD { get; set; }
        public int ILCEKOD { get; set; }
        public int MAHALLEKOD { get; set; }

        public string HREF { get; set; }
        private List<Input> attrs=new List<Input> ();
        private List<Input> cattrs = new List<Input>();
        private List<Input> images = new List<Input>();

        public decimal Value { get; set; }
        public List<Input> Attribute
        {
            get
            {
                return attrs != null ? attrs : new List<Input>();
            }

            set
            {
                attrs = value;
            }
        }
        public List<Input> Categories
        {
            get
            {
                return cattrs != null ? cattrs : new List<Input>();
            }

            set
            {
                cattrs = value;
            }
        }
        public List<Input> Images
        {
            get
            {
                return images != null ? images : new List<Input>();
            }

            set
            {
                images = value;
            }
        }

        public ProductModelView()
        {
            Attribute = new List<Input>();
            Categories = new List<Input>();
            Images = new List<Input>();

            UserImages = new List<Input>();
        }
        private string price;
        public string Price
        {
            get
            {


                return String.Format("{0:N}", Convert.ToDouble(price)).Split(',')[0];
            }

            set
            {

                price = value;
            }
            }
        public string City { get; set; }
        public string Town { get; set; }
        public string FullLoc { get; set; }
        public string Date { get; set; }
        public string Description { get; internal set; }
        public User User { get; internal set; }
        public List<Input> ValueAttribute { get; internal set; }
        public int CityId { get; internal set; }
        public int TownId { get; internal set; }
        public int AreaId { get; internal set; }
        public int NeigboardId { get; internal set; }
        public string Latude { get; internal set; }
        public string Longtude { get; internal set; }
        public DateTime Datte { get; internal set; }
        public List<Input> UserImages { get; set; }
        public object Link { get; internal set; }
    }
}