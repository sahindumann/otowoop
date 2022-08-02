using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EClassField.API.Models
{
    public class Input
    {
       
        public string Name { get; set; }
        public string Value { get; set; }
        public string Baslik { get; set; }
        public string Aciklama { get; set; }
        public string Geoloc { get; set; }
    }
    public class Input2
    {
        public string Name { get; set; }
        public object Value { get; set; }
     
    }

    public class ProductUploadDto
    {
        public long Id { get; set; }

        public int CategoryId { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public decimal Price { get; set; }

        public List<string> Images { get; set; }

        public List<AttributeModel2> Attributes { get; set; }

        public long CityId { get; set; }

        public long TownId { get; set; }

        public long NeighborHoodId { get; set; }

        public string Value { get; set; }

    }


    public class ProductListDto
    {

        public List<int> CategoryIds { get; set; }

        public List<AttributeModel2> Attributes { get; set; }

        public List<Range> Ranges { get; set; }

        public int PageID { get; set; }

        public int PageSize { get; set; }


    }
    public class Range
    {
        public long AttributeId { get; set; }
        public decimal Min { get; set; }
        public decimal Max { get; set; }

    }
    public class AttributeModel2
    {
        public int AttributeId { get; set; }

        public int SubAttributeId { get; set; }

        public string Value { get; set; }

    }

}