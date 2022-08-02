using EClassField.Core.Domain.Rating;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Data.Mapping.Kazanc
{
    public class KazancUserMap : EntityTypeConfiguration<KazancUser>
    {
        public KazancUserMap()
        {
            HasKey(d => d.Id);
            this.HasRequired(d => d.User).WithMany(d => d.KazancUsers).WillCascadeOnDelete();
            this.HasRequired(d => d.Kazanc).WithMany().HasForeignKey(d => d.KazancId).WillCascadeOnDelete();
            this.Map(d => d.ToTable("KazancUserMap"));
        }
    }
}
