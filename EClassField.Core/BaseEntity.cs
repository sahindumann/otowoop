using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core
{
    public class BaseEntity<T>
    {
       
        public T Id { get; set; }
        public BaseEntity()
        {
            if (Id.GetType() == typeof(string))
            {
                string guid = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);

                
            }
        }
    }
}
