using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core.Domain.Locazition
{
    public class Language : BaseEntity<int>
    {
        private ICollection<LocaleString> _localestring;
        public string Name { get; set; }
        public string SeoName { get; set; }
        public string Icon { get; set; }
        public virtual ICollection<LocaleString> LocaleStrng
        {
            get { return _localestring ?? (_localestring= new List<LocaleString>()); }
            set { _localestring = value; }
        }

    }
}
