using EClassField.Core;
using EClassField.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClassFieldWeb_.Models
{
  public  class Util
    {
        public static void RemoveEntity(Type entity, Expression ex, ClassFieldDbContext ctx)
        {
            var list = ctx.Set(entity).Find(ex);
            
           



        }

    }
}
