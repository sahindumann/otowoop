using EClassField.Core.Domain.Locazition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core.Domain.Blog
{
    public class PostLanguage : BaseEntity<int>
    {
        public int LanguageId { get; set; }
        public int PostId { get; set; }

        public virtual Language Language { get; set; }
        public virtual Post Post { get; set; }
    }
}
