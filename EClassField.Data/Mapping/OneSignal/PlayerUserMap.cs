using EClassField.Core.Domain.OneSignal;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Data.Mapping.OneSignal
{
    class PlayerUserMap:EntityTypeConfiguration<PlayerUser>
    {
        public PlayerUserMap()
        {
            this.HasKey(d => d.Id);
            this.HasRequired(d => d.User).WithMany().HasForeignKey(d => d.UserID);
            this.HasRequired(d => d.Player).WithMany().HasForeignKey(d => d.PlayerID);
        }
    }
}
