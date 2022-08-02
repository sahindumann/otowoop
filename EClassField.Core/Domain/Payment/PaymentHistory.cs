using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core.Domain.Payment
{
    public enum PaymentAction
    {
        GirisYapildi = 0,
        GunlukGirisYapildi = 1,
        IlanEklendi = 2,
        IlanPaylasildi = 3,
        KullaniciDavetEdildi = 4

    }
    public class PaymentHistory : BaseEntity<int>
    {
        public User.User UserId { get; set; }
        public decimal KazanilanKredi { get; set; }
        public PaymentAction Olay { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
