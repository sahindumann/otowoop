using EClassField.API.Models;
using EClassField.Core;
using EClassField.Core.Domain.Attribute;
using EClassField.Core.Domain.Catalog;
using EClassField.Core.Domain.Comment;
using EClassField.Core.Domain.Directory;
using EClassField.Core.Domain.Media;
using EClassField.Core.Domain.OneSignal;
using EClassField.Core.Domain.Rating;
using EClassField.Core.Domain.User;
using EClassField.Data;
using EClassField.Data.Mapping.Catalog;

using ImageProcessor;
using ImageProcessor.Imaging.Formats;

using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Data.Entity.SqlServer;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Xml.Linq;

namespace EClassField.API.Controllers
{
    public class ProductController : ApiController
    {



        [HttpGet]
        public IHttpActionResult UploadPhotoFoursquare()
        {



            return Ok("");
        }



        public IHttpActionResult Get(int ProductID = 0, int KatID = 0, bool edit = false, bool filter = false)

        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {

                if (filter)
                {
                    var a = GetAttributes(KatID, ctx);

                    return Ok(a);
                }

                if (ProductID == 0)
                {
                    var a = GetAttributes(KatID, ctx);

                    return Ok(new { S = "", AttributeValues = a });
                }
                else
                {


                    string productstr = ProductID + "";
                    var product = ctx.Set<Product>().FirstOrDefault(d => d.Id == ProductID);

                    if (KatID == 0)
                    {

                        KatID = product.ProductCategories.OrderByDescending(x => x.CategoryId).FirstOrDefault().CategoryId;
                    }

                    return Ok(new
                    {
                        Id = product.Id,

                        Title = product.Title,
                        Description = StripHTML(product.Description),
                        Attributes = GetValues(ctx.Set<ProductAttribute>().Where(x => x.ProductId == product.Id).ToList(), ctx.Set<ProductAttributeValue>().Where(x => x.ProductId == product.Id).ToList(), ctx.Set<ProductAttributeValueNumber>().Where(x => x.ProductId == product.Id).ToList()),
                        Location = product.IL + "/" + product.ILCE + "/" + product.MAHALLE,
                        LocationsID = new int[] { product.City.CityId, product.Town.TownId, product.Neighborhood.Id },
                        Katyol = product.ProductCategories.OrderBy(y => y.CategoryId).Select(d => new { Name = d.Category.Name, Value = d.Category.Id }).ToList(),
                        Phones = new string[] { product.User.EvTel, product.User.IsTel, product.User.IsTel2 },
                        User = new
                        {
                            Id = product.User.Id,
                            User = product.User.Name + " " + product.User.SurName,
                            Email = product.User.Email,
                            Tel1 = !String.IsNullOrEmpty(product.Tel1) ? product.Tel1 : product.User.Cep,
                            Tel2 = product.User.EvTel,
                            Tel3 = product.User.IsTel,
                            Tel4 = product.User.IsTel2
                        },
                        Price = edit ? product.Price.ToString() : String.Format("{0:C}", product.Price).Split(',')[0] + " TL",
                        Images = product.ProductPictures.Select(d => d.Picture.FileName).ToList(),
                        CreationTime = product.CreationTime.ToShortDateString(),
                        AttributeValues = edit ? GetAttributes(KatID, ctx) : null,
                        Aktif = product.IsActive,
                        Pending = product.IsPending,
                        //LocationCoord = product.Location != null ? product.Location.Longitude.Value.ToString().Replace(",", ".") + "," + product.Location.Latitude.Value.ToString().Replace(",", ".") : "",
                        VenueId = product.VenueId
                    });
                }
            }



        }

        public dynamic GetValues(List<ProductAttribute> attrids, List<ProductAttributeValue> texts, List<ProductAttributeValueNumber> numberrange)
        {

            List<dynamic> x = new List<dynamic>();

            attrids.ForEach(d =>
            {

                x.Add(new
                {

                    Name = d.Attribute.Name,
                    Value = d.SubAttribute.Value,
                    AttributeId = d.AttributeId,
                });
            });

            texts.ForEach(d =>
            {

                x.Add(new
                {

                    Name = d.M_Attribute.Name,
                    Value = d.Value
                });
            });
            numberrange.ForEach(d =>
            {

                x.Add(new
                {

                    Name = d.M_Attribute.Name,
                    Value = Convert.ToInt64(string.Format("{0:N}", d.Value).Split(',')[0].Replace(".", ""))
                });
            });




            return x;
        }

        public string StripHTML(string input)
        {
            if (input != null)
                return Regex.Replace(input, "<.*?>", String.Empty);

            return "";
        }
        public static string DoFormatDecimal(decimal myNumber)
        {
            try
            {
                if (myNumber >= 1000 && myNumber <= 9999)
                {
                    return string.Format("{0:N}", myNumber).Split(',')[0].Replace(".", "");
                }

                return string.Format("{0:N}", myNumber).Split(',')[0];

            }
            catch (Exception)
            {

                return "Belirtilmemiş";
            }

        }
        object GetAttributes(int KatID, ClassFieldDbContext ctx)
        {
            var cats = Function.getKategoriYol(KatID).Select(d => Convert.ToInt32(d.Id)).OrderByDescending(d => d).ToList();
            int _katID = 0;
            List<CategoryAttribute> attrs = new List<CategoryAttribute>();
            foreach (var item in cats)
            {
                attrs = ctx.Set<CategoryAttribute>().Where(d => d.CategoryId == item).ToList();
                _katID = item;
                if (attrs.Count >= 1)
                    break;
            }
            var attribute = attrs.Select(d =>
            new
            {
                AttributeName = d.Attribute.Name,
                AttributeValues = ctx.Set<CategorySubAttribute>()
                .Where(c => c.SubAttribute.Value != null && c.M_Attribute.AttributeType == AttributeType.DropDown
                && c.AttributeId == d.AttributeId && c.CategoryId == _katID)
                .ToList().Select(x => new { Name = x.SubAttribute.Value, Value = x.SubAttributeId,SelectedValue=x.SubAttribute.ValueNumber }).ToList(),
                AttributeId = d.AttributeId,
               


            }).ToList();

            return attribute;
        }

        [HttpGet]
        public IHttpActionResult GetUserIlan(int ID = 0, int pageID = 1, bool isAktif = false, bool favori = false, bool isAdmin = false)
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {
                if (favori)
                {
                    var products = ctx.Set<ProductFavori>().Where(d => (d.UserId == ID)).OrderByDescending(d => d.Product.CreationTime).ToList().Select(d => new
                    {
                        Id = d.ProductId,
                        Title = d.Product.Title,
                        Price = String.Format("{0:C}", d.Product.Price).Split(',')[0],
                        Image = d.Product.ProductPictures.Any() ? d.Product.ProductPictures.LastOrDefault().Picture.FileName : null,
                        Location = Function.FullLoc(d.Product.City, d.Product.Town, null, null),

                        //Kategori = Function.getKategoriYol(d.Product.Category.Id)
                        //.OrderBy(a => a.Id).Select(a => a.Name).Aggregate((a, b) => a + " > " + b)
                    }).Skip((pageID - 1) * 8).Take(8);

                    return Ok(products.ToList());
                }
                else
                {

                    var products = ctx.Set<Product>().Where(d => (isAdmin && d.IsActive == isAktif) || (!isAdmin && d.User.Id == ID && d.IsActive == isAktif)).OrderByDescending(d => d.CreationTime).ToList();

                    if (products.Any())
                    {

                        var aa = products.Select(d =>
                          new
                          {
                              Id = d.Id,
                              Title = d.Title,
                              Price = String.Format("{0:C}", d.Price).Split(',')[0],
                              Image = d.ProductPictures.Any() ? d.ProductPictures.LastOrDefault().Picture.FileName : null,
                              Location = Function.FullLoc(d.City, d.Town, null, null)
                              //   Kategori = d.ProductCategories==null?null:d.ProductCategories
                              // .OrderBy(a => a.Id).Select(a => a.Category.Name).Aggregate((a, b) => a + " > " + b)
                          }).Skip((pageID - 1) * 8).Take(8);


                        return Ok(aa.ToList());
                    }
                    else
                    {
                        return Ok(products.ToList());
                    }
                }
            }


        }



        [HttpGet]
        public IHttpActionResult GetIlan(bool isfun = false, int categoryID = 2, int pageID = 1, string attrs = "", string range = "", string locs = "", string orderby = "desc", string orderTip = "CreationTime", string locsID = "", double radius = 1000, string ll = "", string lon = "", string lat = "", string point = "", string geoloc = "")
        {


            //using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            //{

            //    //var products = ctx.Set<Product>().Where(x => x.Link != null).ToList();

            //    //foreach (var item in products)
            //    //{

            //    //    if (item.City != null)
            //    //    {
            //    //        try
            //    //        {
            //    //            string city = item.City.Name;
            //    //            string town = item.Town.Name;
            //    //            string neighbordhood = item.Neighborhood.Name;


            //    //            string address = city + " " + town + " " + neighbordhood;
            //    //            string requestUri = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false", Uri.EscapeDataString(address));

            //    //            WebRequest request = WebRequest.Create(requestUri);
            //    //            WebResponse response = request.GetResponse();
            //    //            XDocument xdoc = XDocument.Load(response.GetResponseStream());

            //    //            XElement result = xdoc.Element("GeocodeResponse").Element("result");
            //    //            XElement locationElement = result.Element("geometry").Element("location");
            //    //            XElement latt = locationElement.Element("lat");
            //    //            XElement lngg = locationElement.Element("lng");

            //    //            string name = $"POINT({latt.Value + " " + lngg.Value})";

            //    //            Product p = ctx.Set<Product>().FirstOrDefault(x => x.Id == item.Id);
            //    //            p.Location = DbGeography.FromText(name);

            //    //            ctx.SaveChanges();
            //    //        }
            //    //        catch
            //    //        { }
            //    //    }
            //    //}





            //}





            DbGeography geography = null;
            if (!string.IsNullOrEmpty(ll))
            {

                lon = ll.Split(',')[0].ToString();
                lat = ll.Split(',')[1].ToString();
                geography = DbGeography.PointFromText($"POINT({lon} {lat})", 4326);
                geography = geography.Buffer(radius);
            }
            //if (!String.IsNullOrWhiteSpace(geoloc))
            //{
            //    geography = DbGeography.PointFromText($"POINT({geoloc})", 4326);
            //    geography = geography.Buffer(radius);
            //}





            if (!String.IsNullOrEmpty(range) && range.Length >= 1 && range.EndsWith("-"))
                range = range.Substring(0, range.Length - 1);



            List<int> _attrs = attrs.Split('-').Where(d => d != "").Select(d => Convert.ToInt32(d)).ToList();
            List<string> _locs = locs.Split('-').ToList();

            int[] locsIDs = new int[1] { 0 };


            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {
                var productattrs = Cache.CatSubAttribute.Where(d => _attrs.Contains(d.Id)).ToList();
                var m_attrbute = Cache.CatSubAttribute.Where(d => _attrs.Contains(d.SubAttributeId));
                // geography != null ?d.Location.Intersects(newGeography) :false || 


                var products = ctx.Set<Product>().Where(d => (d.IsActive || d.ProductCategories.Any(x => x.Category.IsFun))).AsQueryable()
                 .Where(d => d.ProductCategories.Select(a => a.CategoryId).Contains(categoryID) || isfun);
                locsIDs = locs.Split('-').Where(d => d != "").Select(d => Convert.ToInt32(d)).ToArray();
                IQueryable<Product> productsgeoloc = null;
                if (geography != null)
                {
                  
                }

                if (locsIDs.Length >= 1)
                {



                    if (locsIDs.Length == 1)
                    {
                        products = products.Where(d => locsIDs.Contains(d.ILKOD));
                    }
                    else if (locsIDs.Length == 2)
                    {
                        products = products.Where(d => locsIDs.Contains(d.ILCEKOD));
                    }

                    //else if (locsIDs.Length == 3)
                    //{
                    //    products = products.Where(d => locsIDs.Contains(d.Area));
                    //}

                    else if (locsIDs.Length == 3)
                    {
                        products = products.Where(d => locsIDs.Contains(d.MAHALLEKOD));
                    }
                }





                //foreach (var item in ranges)
                //{
                //    int[] _range = item.Split('=')[1].Split('-')[0].Split(',').Select(d => Convert.ToInt32(d)).ToArray();

                //    if (range[0] >= 1 && range[1] >= range[0]) {

                //        products = products.Where("ProductAttribute.SubAttribute.Value=@0",2010); 
                //    }



                //}



                List<AttributeModel> attributes = new List<AttributeModel>();

                var cats = Function.getKategoriYol(categoryID).Select(x => x.Id);
                var lastKatID = Cache.CatAttribute.Where(x => x.Category != null && x.Category.SubAttributes != null).LastOrDefault(d => d.Category.SubAttributes.Any() && cats.Contains(d.CategoryId)) ?? null;

                int catID = lastKatID != null ? lastKatID.CategoryId : 0;
                var attrss = range.Split(';');
                foreach (var item in attrss)
                {
                    string attrname = item.Split('=')[0];
                    var m_attr = Cache.CatAttribute.Find(d => d.Attribute.Name == attrname && d.Category.Id == catID);
                    if (m_attr != null)
                    {
                        if (m_attr.Attribute.AttributeType == AttributeType.Range)
                        {
                            attributes.Add(new AttributeModel { AttributeId = m_attr.AttributeId, Name = m_attr.Attribute.Name, Text = item.Split('=')[1], SubAttributeId = -1, Type = m_attr.Attribute.AttributeType });
                        }
                        else
                        {
                            attributes.Add(new AttributeModel { AttributeId = m_attr.AttributeId, Name = m_attr.Attribute.Name, Text = item.Split('=')[1], SubAttributeId = Convert.ToInt32(item.Split('=')[1]), Type = m_attr.Attribute.AttributeType });
                        }
                    }

                }




                var attrsnames = Cache.CatSubAttribute.Where(d => attrs.Contains(d.SubAttributeId.ToString())).Select(d => d.SubAttribute.Value).Distinct().ToList();


                foreach (var item in attributes)
                {
                    if (item.Type == AttributeType.Range)
                    {

                        double attrvalue1 = Convert.ToInt64(item.Text.Split(',')[0]);

                        double attrvalue2 = Convert.ToInt64(item.Text.Split(',')[1]);

                        products = products.Join(ctx.Set<ProductAttributeValueNumber>(), pro => pro.Id, pvalue => pvalue.ProductId, (p, pval) => new
                        {

                            p = p,
                            pval = pval

                        }).Where(x => x.pval.M_Attribute.Id == item.AttributeId && x.pval.Value >= attrvalue1 && x.pval.Value <= attrvalue2).Select(x => x.p);
                    }
                    else if (item.Type == AttributeType.DropDown)
                    {


                        products = products.Join(ctx.Set<ProductAttribute>(), p => p.Id, pattr => pattr.ProductId, (p, pattr) => new
                        {

                            p = p,
                            pattr = pattr,
                     
                        }).Where(x => x.pattr.SubAttributeId == item.SubAttributeId).Select(x => x.p);
                    }
                }


                if (orderTip == "CreationTime" || orderTip == "Price")
                {
                    if (orderby == "desc")
                        products = products.OrderBy(orderTip + " descending");
                    else
                    {
                        products = products.OrderBy(orderTip);
                    }
                }
                else
                {

                    products = products.Join(ctx.Set<ProductAttributeValueNumber>().Where(x => x.M_Attribute.Name == orderTip), pro => pro.Id, pvalue => pvalue.ProductId, (p, pval) => new
                    {

                        p = p,
                        pvalue = pval.Value

                    }).OrderBy("pvalue " + orderby).Select(x => x.p);


                }






                var ranges = range.Split(';');
                foreach (var item in ranges)
                {
                    if (!String.IsNullOrEmpty(item))
                    {

                        string name = item.Split('=')[1];
                        string[] rangess = name.Split(',');
                        int min = Convert.ToInt32(rangess[0]);
                        int max = Convert.ToInt32(rangess[1]);


                        products = products.Where(d => d.Price >= min && d.Price <= max);
                    }
                }
                //for (int i = 0; i < ranges.Count(); i++)
                //{
                //    string name = range.Split(';')[i].Split('=')[0];
                //    string[] rangess = range.Split(';')[i].Split('=')[1].Split(',');
                //    int min = Convert.ToInt32(rangess[0]);
                //    int max = Convert.ToInt32(rangess[1]);
                //    List<string> ids = new List<string>();

                //    if (name == "Fiyat")
                //    {
                //        products = products.Where(d => d.Price >= min && d.Price <= max);

                //    }
                //    else
                //    {



                //        products = products.Where(d => d.ProductAttributes.FirstOrDefault(p => p.Attribute.Name == name).SubAttribute.ValueNumber >= min && d.ProductAttributes.FirstOrDefault(p => p.Attribute.Name == name).SubAttribute.ValueNumber <= max);

                //    }

                //    //products = products.Where(d => d.ProductAttributes.Select(c => c.SubAttribute.Value).Any(f => ids.Contains(f)));


                //    //else
                //    //    products = products.Where(d => d.ProductAttributes.Where(y=>y.Attribute.Name==name).Select(f => f.SubAttribute.Value).Any(p=>Between(p,min,max)))
                //}







                var ab = products.Skip((pageID - 1) * 8)
                        .Take(8).ToList();

                var ab2 = ab.ToList().Select(d => new
                {

                    Id = d.Id,
                    Title = d.Title,
                    Price = String.Format("{0:C}", d.Price),
                    Fiyat = d.Price,
                    CreationTime = d.CreationTime.ToShortDateString(),
                    Video = d.VideoLink,
                    Type = ctx.Set<ProductAttribute>().Where(x => x.ProductId == d.Id).Select(x => x.SubAttribute.Value).FirstOrDefault(),
                    Value = ctx.Set<ProductAttributeValue>().Where(f => f.ProductId == d.Id).Select(f => f.Value).FirstOrDefault(),


                    Comments = ctx.Set<ProductComment>().Where(c => c.ProductId == d.Id).ToList().Select(c => new { Title = c.Comment.Title, User = d.User.Name, Image = d.User.UserImages.ToList().FirstOrDefault() != null ? d.User.UserImages.FirstOrDefault().Picture.FileName : null }).ToList(),
                    Image = ctx.Set<ProductPicture>().Where(x => x.Product != null && x.ProductId == d.Id).Select(x => x.Picture.FileName).ToList(),
                    Location = d.IL + "/" + d.ILCE + "/" + d.MAHALLE,
           
                    LocationIDS = new int[] { d.City != null ? d.City.Id : -1, d.Town != null ? d.Town.Id : -1, d.Area != null ? d.Area.Id : -1, d.Neighborhood != null ? d.Neighborhood.Id : -1 },
                    OrderValues = SpecialValue(orderTip, d) == null ? d.ProductAttributes.Where(a => a.Attribute.Name == orderTip)
                           .Select(a => new Input2 { Name = a.Attribute.Name, Value = a.SubAttribute.Value }).FirstOrDefault() : SpecialValue(orderTip, d),
                    Kategori = d.ProductCategories.Where(x => x.Category != null).ToList()
                           .OrderBy(a => a.Id).Select(a => a.Category.Name).Aggregate((a, b) => a + " > " + b),

                }).ToList();


                //if (orderTip == "Price" || orderTip == "Fiyat" || orderTip == "CreationTime")
                //{
                //    ab = ab.OrderBy((orderTip == "Price" ? "Fiyat " : orderTip) + " " + orderby);

                //}
                //else
                //{
                //    ab = ab.OrderBy("OrderValues.Value" + " " + orderby);
                //}




                return Ok(ab2.ToList());
            }
        }

        public string fulloc(int ilkod = -1, int ilcekod = -1, int mahkod = -1, ClassFieldDbContext ctx = null)
        {

            if (mahkod >= 1)
            {
                string s = "";
                s += ctx.Set<City>().FirstOrDefault(d => d.CityId == ilkod).Name + "/" +
                     ctx.Set<Town>().FirstOrDefault(d => d.Id == ilcekod).Name + "/" +
                     ctx.Set<Neighborhood>().FirstOrDefault(d => d.Id == mahkod).Name;
                return s;

            }
            else if (ilcekod >= 1)
            {
                string s = "";
                s += ctx.Set<City>().FirstOrDefault(d => d.CityId == ilkod).Name + "/" +
                     ctx.Set<Town>().FirstOrDefault(d => d.TownId == ilcekod).Name;
                return s;

            }
            else if (ilkod >= 1)
            {
                string s = "";
                s += ctx.Set<City>().FirstOrDefault(d => d.CityId == ilkod).Name;

                return s;
            }

            return "";
        }

        public bool Between(string val, int min, int max)
        {

            long _val = Convert.ToInt64(val);
            if (_val >= min && _val <= max)
                return true;
            else
                return false;



        }

        public Input2 SpecialValue(string name, dynamic value)
        {

            var input = new Input2 { Name = name, Value = ((Product)value).ProductAttributes.ToList().Find(d => d.Attribute.Name == name) };
            if (input != null && input.Value != null)
            {
                input.Value = ((ProductAttribute)input.Value).SubAttribute.Value;
                return input;
            }


            return new Input2 { Name = name, Value = "" };
        }
        [HttpGet]
        public IHttpActionResult GetOrderList(int KatID = 0)
        {


            return Ok(Function.GetOrderFilterList(KatID));
        }


        [HttpPost]

        public IHttpActionResult Post(Input id)
        {


            string cat = Function.getValue(id.Value, "KatID");
            string baslik = id.Baslik;
            string aciklama = id.Aciklama;
            string address = Function.getValue(id.Value, "Address");
            string fiyat = Function.getValue(id.Value, "Fiyat");
            string geoloc = id.Geoloc;
            DbGeography geophray = null;
            if (!String.IsNullOrWhiteSpace(geoloc))
            {
                geophray = DbGeography.FromText($"POINT( {geoloc.Replace(",", " ")})");

            }

            int cityID = -1, TownId = -1, NeighborhoodId = -1;


            int userID = Convert.ToInt32(Function.getValue(id.Value, "UserID"));
            int productID = -1;
            if (Function.getValue(id.Value, "ProductID") != "")
            {
                productID = Convert.ToInt32(Function.getValue(id.Value, "ProductID"));
            }

            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {
                var cats = Function.getKategoriYol(Convert.ToInt32(cat)).ToList();
                int _katID = Convert.ToInt32(cat);

                List<Input> inputs = new List<Input>();
                List<ProductAttribute> pattrs = new List<ProductAttribute>();
                Product p = ctx.Set<Product>().FirstOrDefault(d => d.Id == productID);
                if (p == null)
                    p = new Product();
                else
                {

                    ctx.Set<Picture>().RemoveRange(p.ProductPictures.Select(d => d.Picture));
                    ctx.Set<ProductCategory>().RemoveRange(p.ProductCategories);
                    ctx.Set<ProductAttribute>().RemoveRange(p.ProductAttributes);
                    ctx.Set<ProductAttributeValue>().RemoveRange(ctx.Set<ProductAttributeValue>().Where(d => d.ProductId == p.Id).ToList());
                    ctx.Set<ProductAttributeValueNumber>().RemoveRange(ctx.Set<ProductAttributeValueNumber>().Where(d => d.ProductId == p.Id).ToList());


                    ctx.SaveChanges();
                }

                if (!string.IsNullOrEmpty(Function.getValue(id.Value, "Location")))
                {
                    int cityID2 = -1;

                    Int32.TryParse(Function.getValue(id.Value, "CityId"), out cityID2);

                    string[] lonlat = Function.getValue(id.Value, "Location").Split(',');
                    string longitude = lonlat[0];
                    string latitude = lonlat[1];
                    var point = string.Format("POINT({1} {0})", latitude, longitude);

     

                    if (!String.IsNullOrWhiteSpace(address) && cityID2 <= 0)
                    {
                        string[] address2 = address.Split(',');
                        string city = address2[0].ToString().Trim();
                        string town = address2[1].ToString().Trim();
                        string negih = address2[2].ToString().Replace("Mahallesi", "").Trim();
                        City c = ctx.Set<City>().FirstOrDefault(d => d.Name.Contains(city));
                        Town t = ctx.Set<Town>().FirstOrDefault(d => d.Name.Contains(town));

                        Neighborhood n = ctx.Set<Neighborhood>().FirstOrDefault(d => d.CityId == c.CityId && d.TownId == t.TownId && d.Name.Contains(negih.Replace("Mahallesi", "")));

                        p.IL = city;
                        p.ILCE = town;
                        p.MAHALLE = negih;
                        p.City = c;
                        p.Town = t;
                        p.Neighborhood = n;
                        p.ILKOD = p.City.CityId;
                        p.ILCEKOD = p.Town.TownId;
                        p.MAHALLEKOD = p.Neighborhood.Id;
                    }
                    else
                    {
                        cityID = Convert.ToInt32(Function.getValue(id.Value, "CityId"));
                        TownId = Convert.ToInt32(Function.getValue(id.Value, "TownId"));
                        NeighborhoodId = Convert.ToInt32(Function.getValue(id.Value, "NeighborhoodId"));

                        p.City = ctx.Set<City>().FirstOrDefault(d => d.CityId == cityID);
                        p.Town = ctx.Set<Town>().FirstOrDefault(d => d.TownId == TownId);
                        p.Neighborhood = ctx.Set<Neighborhood>().FirstOrDefault(d => d.Id == NeighborhoodId);


                        p.IL = p.City.Name;
                        p.ILCE = p.Town.Name;
                        p.MAHALLE = p.Neighborhood.Name;


                        p.ILKOD = cityID;
                        p.ILCEKOD = TownId;
                        p.MAHALLEKOD = NeighborhoodId;

                    }
                }
                else
                {
                    cityID = Convert.ToInt32(Function.getValue(id.Value, "CityId"));
                    TownId = Convert.ToInt32(Function.getValue(id.Value, "TownId"));
                    //int AreaID = Convert.ToInt32(Function.getValue(id.Value, "AreaID"));
                    NeighborhoodId = Convert.ToInt32(Function.getValue(id.Value, "NeighborhoodId"));
                }

                List<AttributeModel> attributes = new List<AttributeModel>();

                var lastKatID = Cache.CatAttribute.Where(x => cats.Any(y => y.Id == x.CategoryId)).LastOrDefault(d => d.Category.SubAttributes.Any()) ?? null;

                int catID = lastKatID != null ? lastKatID.CategoryId : 0;
                var attrss = id.Value.Split(';');
                foreach (var item in attrss)
                {

                    if (item.Split('=').Length >= 1 && !String.IsNullOrEmpty(item))
                    {
                        string attrname = item.Split('=')[0];

                        if (!String.IsNullOrEmpty(item.Split('=')[1]))
                        {
                            var m_attr = Cache.CatAttribute.Find(d => d.Attribute.Name == attrname && d.Category.Id == catID);
                            if (m_attr != null)
                            {

                                attributes.Add(new AttributeModel { AttributeId = m_attr.AttributeId, Name = m_attr.Attribute.Name, Text = item.Split('=')[1], SubAttributeId = Convert.ToInt32(item.Split('=')[1]) });
                            }
                        }
                    }


                }


                if (attributes != null && attributes.Any())
                {



                    foreach (var item in attributes)
                    {
                        var attribute = ctx.Set<M_Attribute>().FirstOrDefault(d => d.Id == item.AttributeId);
                        if (attribute.AttributeType == AttributeType.Text || attribute.AttributeType == AttributeType.Range)
                        {

                            double ddd = 0;
                            Double.TryParse(item.Text, out ddd);

                            if (ddd == 0)
                            {
                                ProductAttributeValue proattrvalue2 = ctx.Set<ProductAttributeValue>().FirstOrDefault(d => d.ProductId == p.Id && d.AttributeId == item.AttributeId);
                                if (proattrvalue2 != null)
                                {
                                    proattrvalue2.Value = item.Text;
                                }
                                else
                                {

                                    ProductAttributeValue proattrvalue = new ProductAttributeValue { AttributeId = item.AttributeId, ProductId = p.Id, Value = item.Text };
                                    ctx.Set<ProductAttributeValue>().Add(proattrvalue);


                                }
                            }
                            else
                            {
                                ProductAttributeValueNumber proattrvalue2 = ctx.Set<ProductAttributeValueNumber>().FirstOrDefault(dd => dd.ProductId == p.Id && dd.AttributeId == item.AttributeId);
                                if (proattrvalue2 != null)
                                {
                                    proattrvalue2.Value = Convert.ToDouble(item.Text);
                                }
                                else
                                {
                                    double val = Convert.ToDouble(item.Text);
                                    ProductAttributeValueNumber proattrvalue = new ProductAttributeValueNumber { AttributeId = item.AttributeId, ProductId = p.Id, Value = val };
                                    ctx.Set<ProductAttributeValueNumber>().Add(proattrvalue);


                                }
                            }

                        }
                        else if (attribute.AttributeType == AttributeType.DropDown)
                        {

                            SubAttribute subattr = ctx.Set<SubAttribute>().FirstOrDefault(d => d.Id == item.SubAttributeId);

                            p.ProductAttributes.Add(new ProductAttribute { AttributeId = attribute.Id, SubAttributeId = subattr.Id, Product = p });
                        }



                    }


                }

                int KatID = Convert.ToInt32(cat);
                //p.Category = ctx.Set<Category>().FirstOrDefault(d => d.Id == KatID);
                p.Title = baslik;
                p.Description = aciklama;
                p.CreationTime = DateTime.Now;
                p.Price = Convert.ToDecimal(fiyat.Split(' ')[0]);

                p.UpdateTme = DateTime.Now;
                if (p.User == null)

                    p.User = ctx.Set<User>().FirstOrDefault(d => d.Id == userID);


                if (cityID != -1)
                {



                    p.City = ctx.Set<City>().FirstOrDefault(d => d.CityId == cityID);
                    p.Town = ctx.Set<Town>().FirstOrDefault(d => d.TownId == TownId);
                    //p.Area = ctx.Set<Area>().FirstOrDefault(d => d.Id == AreaID);
                    p.Neighborhood = ctx.Set<Neighborhood>().FirstOrDefault(d => d.NeigborhoodId == NeighborhoodId);
                }
                p.IsActive = false;
                p.IsPending = true;


                //p.Location = new Core.Domain.Directory.Location();

                //p.Location.CountryId = 1;
                //p.Location.CityId = cityID;
                //p.Location.TownId = TownId;
                //p.Location.AreaId = AreaID;
                //p.Location.NeighborhoodId = NeighborhoodId;
                //p.Location.UserId = userID;
                //p.Location.ProductId = p.Id;

                if (p.Id <= 0)
                    ctx.Set<Product>().Add(p);

                ctx.SaveChanges();

                foreach (var item in cats.OrderBy(d => d.Id))
                {
                    p.ProductCategories.Add(new ProductCategory
                    {
                        CategoryId = item.Id,
                        ProductId = p.Id

                    });


                }

                if (p.User != null)
                {
                    ctx.Set<ProductUser>().Add(new ProductUser { ProductId = p.Id, UserId = p.User.Id });
                }
                else
                    ctx.Set<ProductUser>().Add(new ProductUser { ProductId = p.Id, UserId = userID });




        


                ctx.SaveChanges();


                var kazanc = ctx.Set<Kazanc>().FirstOrDefault(d => d.ProductId == p.Id);
                if (kazanc != null)
                {
                    p.IsActive = true;
                }


                var player = ctx.Set<Player>().FirstOrDefault(d => d.Id == 9);
                if (player != null)
                {
                    OneSignalAPI.SendMessage("Yeni Bir İlan Eklendi", new List<string>() { player.PlayerID });
                }


                return Ok(new { Id = p.Id });





            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> PostNew(ProductUploadDto _dto)
        {

            try
            {
                using (ClassFieldDbContext ctx = new ClassFieldDbContext())
                {

                    var _product = ctx.Set<Product>().FirstOrDefault(x => x.Id == _dto.Id);


                    if (_product == null)
                    {
                        _product = new Product();
                        _product.CreationTime = DateTime.Now;
                        _product.UpdateTme = DateTime.Now;
                    }
                    //

                    // ctx.SaveChanges();


                    var _categories = Function.getKategoriYol(_dto.CategoryId);

                    City _c = ctx.Set<City>().FirstOrDefault(d => d.CityId == _dto.CityId);

                    Town _t = ctx.Set<Town>().FirstOrDefault(d => d.TownId == _dto.TownId);

                    Neighborhood _n = ctx.Set<Neighborhood>().FirstOrDefault(d => d.Id == _dto.NeighborHoodId);

                    _product.Title = _dto.Title;

                    _product.Description = _dto.Description;

                    _product.Price = _dto.Price;

                    _product.City = _c;

                    _product.Town = _t;

                    _product.Neighborhood = _n;

                    _product.ILKOD = _c.Id;

                    _product.ILCEKOD = _t.Id;

                    _product.MAHALLEKOD = _n.Id;

                    _product.IL = _c.Name;

                    _product.ILCE = _t.Name;

                    _product.MAHALLE = _n.Name;

                    ctx.Set<Picture>().RemoveRange(_product.ProductPictures.Select(d => d.Picture));
                    ctx.Set<ProductCategory>().RemoveRange(_product.ProductCategories);
                    ctx.Set<ProductAttribute>().RemoveRange(_product.ProductAttributes);
                    ctx.Set<ProductAttributeValue>().RemoveRange(ctx.Set<ProductAttributeValue>().Where(d => d.ProductId == _product.Id).ToList());
                    ctx.Set<ProductAttributeValueNumber>().RemoveRange(ctx.Set<ProductAttributeValueNumber>().Where(d => d.ProductId == _product.Id).ToList());

                    if (_categories.Any())
                    {
                        foreach (var item in _categories)
                        {
                            _product.ProductCategories.Add(new ProductCategory
                            {

                                ProductId = _product.Id,
                                CategoryId = item.Id

                            });

                        }
                    }

                    if (_dto.Attributes != null && _dto.Attributes.Any())
                    {
                        foreach (var item in _dto.Attributes)
                        {

                            var subAttr = ctx.Set<SubAttribute>().FirstOrDefault(x => x.Id == item.SubAttributeId || x.Value == item.Value);

                            var attr = ctx.Set<M_Attribute>().FirstOrDefault(x => x.Id == item.AttributeId);

                            if (subAttr == null)
                            {
                                subAttr = ctx.Set<SubAttribute>().Add(new SubAttribute { Value = item.Value, ValueNumber = Convert.ToDecimal(item.Value) });
                                ctx.SaveChanges();

                            }

                            _product.ProductAttributes.Add(new ProductAttribute
                            {
                                AttributeId = item.AttributeId,
                                ProductId = _product.Id,
                                SubAttributeId = subAttr.Id,


                            });

                         
                        }

                    }


                    if (_dto.Images != null && _dto.Images.Any())
                    {

                        foreach (var item in _dto.Images)
                        {
                            _product.ProductPictures.Add(new ProductPicture
                            {

                                Picture = new Picture { FileName = item },
                                ProductId = _product.Id,

                            });

                        }

                    }

                    _product.User = ctx.Set<User>().FirstOrDefault(x => x.Id == _dto.UserId);

                    // ctx.Entry(_product).State = EntityState.Modified;
                    if (_product.Id <= 0)
                    {
                        ctx.Set<Product>().Add(_product);
                    }
                   var productId=await ctx.SaveChangesAsync();

                    return Ok(new { Id = productId });
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return Ok("");


        }

        [HttpPost]
        public async Task<Response> GetList(ProductListDto _dto)

        {
            Response _response = new Response();

            try
            {
                using (ClassFieldDbContext ctx = new ClassFieldDbContext())
                {
                    var _query = ctx.Set<Product>().AsQueryable();

                    if (_dto.CategoryIds != null && _dto.CategoryIds.Any())
                    {

                        _query = _query.Where(x => x.ProductCategories.Any(y => _dto.CategoryIds.Contains(y.CategoryId)));
                    }
                    if (_dto.Attributes != null && _dto.Attributes.Any())

                    {

                        foreach (var item in _dto.Attributes.GroupBy(x=>x.AttributeId))
                        {
                            
                            
                            var list = item.Select(x=>x.SubAttributeId).ToList();
                            _query = _query.Where(x => x.ProductAttributes.Any(y =>list.Contains(y.SubAttributeId)));
                            
                            
                        }
                    
                    }

                    if (_dto.Ranges != null && _dto.Ranges.Any())
                    {

                        foreach (var item in _dto.Ranges.Where(x=>x.AttributeId>0))
                        {
                            _query = _query.Where(x => x.ProductAttributes.Any(y => y.SubAttribute.ValueNumber >= item.Min && y.SubAttribute.ValueNumber<=item.Max &&y.AttributeId==item.AttributeId));
                        }
          
                    
                    }

                    _query = _query.OrderBy(x=>x.Id) .Skip((_dto.PageID-1)*_dto.PageSize).Take(_dto.PageSize);


                    _response.Value = _query.ToList().Select(d => new
                    {

                        Id = d.Id,
                        Title = d.Title,
                        Price = String.Format("{0:C}", d.Price),
                        Fiyat = d.Price,
                        CreationTime = d.CreationTime.ToShortDateString(),
                        Video = d.VideoLink,
                        Type = ctx.Set<ProductAttribute>().Where(x => x.ProductId == d.Id).Select(x => x.SubAttribute.Value).FirstOrDefault(),
                        Value = ctx.Set<ProductAttributeValue>().Where(f => f.ProductId == d.Id).Select(f => f.Value).FirstOrDefault(),

                        Comments = ctx.Set<ProductComment>().Where(c => c.ProductId == d.Id).ToList().Select(c => new { Title = c.Comment.Title, User = d.User.Name, Image = d.User.UserImages.ToList().FirstOrDefault() != null ? d.User.UserImages.FirstOrDefault().Picture.FileName : null }).ToList(),
                        Image = ctx.Set<ProductPicture>().Where(x => x.Product != null && x.ProductId == d.Id).Select(x => x.Picture.FileName).ToList(),
                        Location = d.IL + "/" + d.ILCE + "/" + d.MAHALLE,
                        //GeolocWKT = d.Location != null ? d.Location.ToWkt() : null,
                        LocationIDS = new int[] { d.City != null ? d.City.Id : -1, d.Town != null ? d.Town.Id : -1, d.Area != null ? d.Area.Id : -1, d.Neighborhood != null ? d.Neighborhood.Id : -1 },
                        //OrderValues = SpecialValue(orderTip, d) == null ? d.ProductAttributes.Where(a => a.Attribute.Name == orderTip)
                        //       .Select(a => new Input2 { Name = a.Attribute.Name, Value = a.SubAttribute.Value }).FirstOrDefault() : SpecialValue(orderTip, d),
                        Kategori = d.ProductCategories.Where(x => x.Category != null).ToList()
                               .OrderBy(a => a.Id).Select(a => a.Category.Name).Aggregate((a, b) => a + " > " + b),

                    }).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }

            return _response;
        }

        [HttpGet]
        public IHttpActionResult PostDelete(int id)
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {

                var product = ctx.Set<Product>().FirstOrDefault(d => d.Id == id);


                ctx.Set<ProductPicture>().RemoveRange(product.ProductPictures);
                ctx.Set<ProductCategory>().RemoveRange(product.ProductCategories);
                ctx.Set<ProductAttribute>().RemoveRange(product.ProductAttributes);
                ctx.Set<Product>().Remove(product);
                ctx.SaveChanges();

                return Ok("T");
            }

        }




        [HttpGet]
        public string AddDeleteFavori(int productID, int userID)
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {

                ProductFavori fav = ctx.Set<ProductFavori>().FirstOrDefault(d => d.UserId == userID && d.ProductId == productID);
                if (fav != null)
                {
                    ctx.Set<ProductFavori>().Remove(fav);
                    ctx.SaveChanges();
                    return "-1";
                }
                else
                {

                    ctx.Set<ProductFavori>().Add(new ProductFavori { ProductId = productID, UserId = userID });
                    ctx.SaveChanges();
                    return "1";
                }




            }


        }



        [HttpGet]
        public string PendingIlan(int productID, int userID)
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {

                Product product = ctx.Set<Product>().FirstOrDefault(d => d.User.Id == userID && d.Id == productID);
                if (product != null)
                {

                    product.IsPending = product.IsPending ? false : true;

                    product.IsActive = false;
                    ctx.SaveChanges();
                    return "1";
                }
                return "-1";


            }


        }

        public void PhotoGeneraton(string image = "")
        {



            MemoryStream ms = new MemoryStream();

            Bitmap bmp = GetImage(image, 200, 150, 80, false);

            bmp.Save(ms, ImageFormat.Jpeg);

            AmazonS3.UploadFile(ms, image, "ilanthumb/");

        }

        public Bitmap GetImage(string image, int width = 480, int height = 360, int quality = 50, bool isface = false)
        {

            string aURL = "https://s3-us-west-2.amazonaws.com/otonomide/ilanimages/" + image;
            Stream rtn = null;
            HttpWebRequest aRequest = (HttpWebRequest)WebRequest.Create(aURL);
            HttpWebResponse aResponse = (HttpWebResponse)aRequest.GetResponse();
            rtn = aResponse.GetResponseStream();



            // Format is automatically detected though can be changed.
            ISupportedImageFormat format = new JpegFormat { Quality = quality };
            Size size = new Size(width, height);

            Bitmap bmp2 = new Bitmap(rtn);
            if (bmp2.Height > bmp2.Width)
            {
                size = new Size(height, width);
            }

            MemoryStream ms2 = new MemoryStream();
            bmp2.Save(ms2, ImageFormat.Jpeg);

            using (MemoryStream outStream = new MemoryStream())
            {
                // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                {
                    if (isface)
                    {
                        ISupportedImageFormat formatt = new JpegFormat { Quality = 100 };
                        imageFactory.Load(bmp2)

               .Format(formatt)
               .BackgroundColor(Color.White)

               .Save(outStream);
                    }
                    else
                    {
                        imageFactory.Load(bmp2)
               .Resize(size)
               .Format(format)
               .BackgroundColor(Color.White)
               .Crop(new ImageProcessor.Imaging.CropLayer(0, 0, 0, 0, ImageProcessor.Imaging.CropMode.Percentage))
               .Save(outStream);
                    }
                    // Load, resize, set the format and quality and save an image.




                    Bitmap bmp = new Bitmap(outStream);



                    bmp = Yaziyaz(bmp, width, height, "", "otowoop.com");

                    MemoryStream ms = new MemoryStream();
                    bmp.Save(ms, ImageFormat.Jpeg);
                    //bmp.Save(ms, ImageFormat.Jpeg);
                    //return File(ms.GetBuffer(), "image/jpeg");
                    return bmp;
                }
                // Do something with the stream.


            }


        }

        public Bitmap Yaziyaz(Bitmap resim, int genislik, int yukseklik, string yol, string watermark)
        {

            //resmin boyutu bizim vermiş olduğumuz genişlik veya yükseklikten büyükse boyutlandırma yapıyoruz.
            if (resim.Width > genislik || resim.Height > yukseklik)
            {
                Size ebatlar = new Size(resim.Width, resim.Height);
                //resmin genişlik ve yükseklik oranını alıyoruz.
                double oran = ((double)resim.Width / (double)resim.Height);
                if (resim.Width > genislik && genislik > 0)
                {//burada genişlik parametresi 0 olarak verilmişse boyutlandırma yapılmayacak. Yani resim orijinal genişliğinde kalacak.
                    ebatlar.Width = genislik;
                    ebatlar.Height = (int)((double)genislik / oran);
                }
                if (ebatlar.Height > yukseklik && yukseklik > 0)
                {//burada yükseklik parametresi 0 olarak verilmişse boyutlandırma yapılmayacak. Yani resim orijinal yükseklikte kalacak.
                    ebatlar.Height = yukseklik;
                    ebatlar.Width = (int)((double)yukseklik * oran);
                }
                resim = new Bitmap(resim, ebatlar);
            }

            //resmin üzerine yazı yazmak istemeyebiliriz o yüzden “watermark” parametresine boş string verebiliriz.
            if (!string.IsNullOrEmpty(watermark))
            {
                Graphics graf = Graphics.FromImage(resim);
                //resmin şeffaflık (alpha) değeri ve renk değerleri belirleniyor.
                SolidBrush firca = new SolidBrush(Color.FromArgb(80, 192, 192, 192));

                //resmin köşegen uzunluğu pisagor denklemiyle hesaplanıyor.
                double kosegen = Math.Sqrt(resim.Width * resim.Width + resim.Height * resim.Height);
                Rectangle kutu = new Rectangle();

                //bu 3 satırda ise yazının başlama noktası (x,y koordinatları) ve ayrıca font boyutu ayarlanıyor.
                //bunun için aşağıdaki gibi yaklaşık değerler kullandım 1,3..... 1,6.... gibi siz bu rakamlarla oynama yapabilirsiniz.
                kutu.X = (int)(kosegen / 3);
                float yazi = (float)(kosegen / watermark.Length * 0.6);
                kutu.Y = -(int)(yazi / 1);

                Font fnt = new Font("times new roman", yazi, FontStyle.Bold);//font tipi ve boyutu       
                                                                             //can alıcı noktamız burası
                                                                             //burada köşegen eğimini aşağıdaki formülle hesaplıyoruz.
                float egim = (float)(Math.Atan2(resim.Height, resim.Width) * 180 / System.Math.PI);
                graf.RotateTransform(egim);
                StringFormat sf = new StringFormat();
                // ve nihayet watermarkımızı resim üzerine yazdırıyoruz.
                graf.DrawString(watermark, fnt, firca, kutu, sf);
            }

            return resim;
        }







        [HttpGet]
        public string ActiveIlan(int productID, int userID)
        {

            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {

                Product product = ctx.Set<Product>().FirstOrDefault(d => d.Id == productID);
                if (product != null)
                {
                    if (product.User.Id == 2040 || product.User.IsAdmin)
                    {
                        product.IsActive = true;

                    }
                    else
                    {
                        product.IsActive = false;
                        return "-1";
                    }


                    product.IsPending = product.IsActive ? false : true;


                    if (product.ProductPictures != null)
                    {
                        string image = product.ProductPictures.FirstOrDefault().Picture.FileName;


                        Bitmap bmp = GetImage(image, 200, 150, 70, product.FaceLink != null);
                        Bitmap bmp2 = GetImage(image, 480, 360, 100, product.FaceLink != null);


                        MemoryStream ms = new MemoryStream();
                        bmp.Save(ms, ImageFormat.Jpeg);
                        //if (AmazonS3.GetFile("otonomide", "ilanthumb/" + image) == null)
                        ////{
                        //AmazonS3.UploadFile(ms, image, "ilanthumb/");


                        //AmazonS3.UploadFile(ms, image, "ilanthumb/");

                        //}
                    }
                    else
                    {

                    }

                    ctx.SaveChanges();




                    return "1";
                }
                return "-1";


            }


        }

        [HttpGet]
        public string AddDeleteFavoriState(int productID, int userID)
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {
                ProductFavori fav = ctx.Set<ProductFavori>().FirstOrDefault(d => d.UserId == userID && d.ProductId == productID);


                return fav != null ? "1" : "-1";
            }
        }

        [HttpGet]
        public string DeleteImages(string productID = "")
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {
                int producttID = Convert.ToInt32(productID);
                var product = ctx.Set<Product>().FirstOrDefault(d => d.Id == producttID);

                if (product != null)
                {
                    var images = product.ProductPictures;

                    ctx.Set<ProductPicture>().RemoveRange(images);
                    ctx.SaveChanges();
                }

            }
            return "ok";

        }



    }
}
