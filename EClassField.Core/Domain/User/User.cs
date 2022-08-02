using EClassField.Core.Domain.Catalog;
using EClassField.Core.Domain.OneSignal;
using EClassField.Core.Domain.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core.Domain.User
{
    public class User : BaseEntity<int>
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Password { get; set; }
        public string EvTel { get; set; }
        public string Cep { get; set; }

        public string IsTel { get; set; }
        public string IsTel2 { get; set; }
        public string Email { get; set; }
        public string FacebookID { get; set; }
        public string GoogleID { get; set; }
        public string FirmaAdi { get; set; }
        public string TcKimlik { get; set; }
        public string Adres { get; set; }
        public string VergiNo { get; set; }
        public bool IsActive { get; set; }
        public bool IsEBulten { get; set; }
        public bool IsAdmin { get; set; }
        public string SmsSifre { get; set; }

        public DateTime CreationTime { get; set; }

        private ICollection<Product> _products;
        public virtual ICollection<Product> ProductAttributes
        {
            get { return _products ?? (_products = new List<Product>()); }
            set { _products = value; }
        }


        private ICollection<ProductUser> _userproducts;
        public virtual ICollection<ProductUser> UserProducts
        {
            get { return _userproducts ?? (_userproducts = new List<ProductUser>()); }
            set { _userproducts = value; }
        }


        private ICollection<PlayerUser> _playerusers;
        public virtual ICollection<PlayerUser> PlayerUsers
        {

            get
            {
                return _playerusers ?? (_playerusers = new List<PlayerUser>());
            }
            set
            {
                _playerusers = value;
            }
        }

        private ICollection<UserImage> _userimages;
        public virtual ICollection<UserImage> UserImages
        {

            get
            {
                return _userimages ?? (_userimages = new List<UserImage>());
            }
            set
            {
                _userimages = value;
            }
        }


        private ICollection<KazancUser> _kazancusers;
        public virtual ICollection<KazancUser> KazancUsers
        {

            get
            {
                return _kazancusers ?? (_kazancusers = new List<KazancUser>());
            }
            set
            {
                _kazancusers = value;
            }
        }

        public User()
        {
            IsAdmin = false;
        }
    }
}
