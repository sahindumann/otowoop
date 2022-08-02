using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core.Domain.Rating
{
    public enum RatingType
    {

        Cirak = 1,
        Kalfa = 2,
        Usta = 3,
        Uzman = 4,
        Kıdemli = 5

    }

    public enum Harcama
    {
        MobilOdeme = 0,
        IlaniAnasayfayaKoyma = 1,
        IlaniVitrineKoyma = 2,
        IlaniUstSiralaraCikarma = 3,
        RenkVerme = 4,
        AcileAlma = 5,
        AcilBrandasi = 6,
        Branda = 7

    }

    public enum KazancType
    {
        IlanVerme = 0,
        BlogaIcerikGirme = 1,
        ReferansileKaydolma = 2,
        YorumaCevapVerme = 3,
        MobilUygulamayiIndirme = 4
    }






    public class KazancAralik
    {
        public decimal Max { get; set; }
        public decimal Min { get; set; }
        public KazancType KazancTipi { get; set; }

    }
   
    public static class KazancDegisken
    {
        public static decimal IlanVerme = 0.25M;
        public static decimal BlogaIcerikGirme = 0.35M;
        public static decimal ReferansUyelik = 0.15M;
        public static decimal YorumaCevapVerme = 0.10M;
        public static decimal MobilUygulamaIndirme = 0.50M;
    }

    public  class KazancList
    {

        public static List<KazancAralik> GetKazancAralik()
        {
            return new List<KazancAralik>() {

                new KazancAralik {KazancTipi=KazancType.IlanVerme,Min=0.10M,Max=0.30M},
                new KazancAralik {KazancTipi=KazancType.BlogaIcerikGirme,Min=0.10M,Max=0.50M},
                new KazancAralik {KazancTipi=KazancType.YorumaCevapVerme,Min=0.10M,Max=0.30M},
                new KazancAralik {KazancTipi=KazancType.MobilUygulamayiIndirme,Min=0.25M,Max=0.25M}

            };

        }
    }

    public class Kazanc : BaseEntity<int>
    {
        public decimal KazancMiktar { get; set; }
        public int UserID { get; set; }
     public int ProductId { get; set; }
        public KazancType KazancTipi { get; set; }
        public string KazancDegisken { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationTime { get; set; }
    }

    public class KazancUser : BaseEntity<int>
    {
        public int UserId { get; set; }
        public int KazancId { get; set; }
    

        public virtual User.User User { get; set; }
        public virtual Kazanc Kazanc { get; set; }

    }




}
