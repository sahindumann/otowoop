using EClassField.Core.Domain.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core
{ 

    public class Ordertip
    {

        public int[] KatID { get; set; }
        public string Orderby { get; set; }
        public string OrderType { get; set; }
        public string OrderDiplayName { get; set; }
        public int OrderIndex { get; set; }
        public AttributeType AttrType { get; set; }

        public string Text { get; set; }

    }

    public class OrderList
    {

        public static List<Ordertip> GetOrderTip()
        {
            var orderList = new List<Ordertip>() {
                new Ordertip {KatID=new int[] {2,3,16,17,18,19 } ,Orderby="Price",OrderDiplayName="Fiyata göre artan",OrderType="asc",OrderIndex=-5,AttrType=AttributeType.Range ,Text="Fiyat"},
                new Ordertip {KatID=new int[] {2,3,16,17,18,19 },Orderby="Price",OrderDiplayName="Fiyata göre azalan",OrderType="desc",OrderIndex=-10,AttrType=AttributeType.Range,Text="Fiyat" },
                new Ordertip {KatID=new int[] {2,3,16,17,18,19 },Orderby="CreationTime",OrderDiplayName="Tarihe göre en eski",OrderType="asc",OrderIndex=-15,AttrType=AttributeType.Range,Text="Tarih" },
                new Ordertip {KatID=new int[] {2,3,16,17,18,19 },Orderby="CreationTime",OrderDiplayName="Tarihe göre en yeni",OrderType="desc",OrderIndex=-20,AttrType=AttributeType.Range ,Text="Tarih"},
                new Ordertip {KatID=new int[] {16,17,18,19 },Orderby="Km",OrderDiplayName="Kilometre'ye göre artan",OrderType="asc",OrderIndex=-25,AttrType=AttributeType.Range,Text="Km" },
                new Ordertip {KatID=new int[] {16,17,18,19 },Orderby="Km",OrderDiplayName="Kilometre'ye göre azalan",OrderType="desc",OrderIndex=-30,AttrType=AttributeType.Range,Text="Km" },
                new Ordertip {KatID=new int[] {16,17,18,19 },Orderby="Yıl",OrderDiplayName="Yıla  göre artan",OrderType="asc",OrderIndex=-35,AttrType=AttributeType.Range,Text="Year" },
                new Ordertip {KatID=new int[] {16,17,18,19 },Orderby="Yıl",OrderDiplayName="Yıla göre azalan",OrderType="desc",OrderIndex=-40,AttrType=AttributeType.Range,Text="Year" },
            };
            return orderList;
        }

        public static List<Ordertip> GetOrderTi2p()
        {
            var orderList = new List<Ordertip>() {
             
                new Ordertip {KatID=new int[] {2,3,16,17,18,19 },Orderby="Price",OrderDiplayName="Fiyata göre azalan",OrderType="desc",OrderIndex=-10,AttrType=AttributeType.Range,Text="Fiyat" },
                new Ordertip {KatID=new int[] {2,3,16,17,18,19 },Orderby="CreationTime",OrderDiplayName="Tarihe göre en eski",OrderType="asc",OrderIndex=-15,AttrType=AttributeType.Range,Text="Tarih" },
         
         
                new Ordertip {KatID=new int[] {16,17,18,19 },Orderby="Km",OrderDiplayName="Kilometre'ye göre azalan",OrderType="desc",OrderIndex=-30,AttrType=AttributeType.Range,Text="Km" },
               
                new Ordertip {KatID=new int[] {16,17,18,19 },Orderby="Year",OrderDiplayName="Yıla göre azalan",OrderType="desc",OrderIndex=-40,AttrType=AttributeType.Range,Text="Yıl" },
            };
            return orderList;
        }

    }
}
