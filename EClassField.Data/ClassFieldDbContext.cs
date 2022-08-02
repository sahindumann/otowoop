using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Data
{
    public class ClassFieldDbContext : DbContext
    {
        public ClassFieldDbContext() : base("name=classfielddbcontext")
        {




        }



        [EdmFunction("ProductAttributeValue", "ParseDouble")]
        public static double ParseDouble(string stringvalue)
        {
            return Double.Parse(stringvalue);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
                        .Where(type => !String.IsNullOrEmpty(type.Namespace))
                        .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
                        type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));

            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
            base.OnModelCreating(modelBuilder);

        }
    }
}
