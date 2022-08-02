using EClassField.Core.Domain.User;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Data.Mapping.User
{
   public class UserImageMap:EntityTypeConfiguration<UserImage>
    {

        public UserImageMap()
        {
            this.HasKey(d => d.Id);
            this.HasRequired(d => d.User).WithMany(d => d.UserImages);
            this.HasRequired(d => d.Picture).WithMany().HasForeignKey(d => d.PictureId);
            this.ToTable("User_Image_Pictures_Mapping");
        }
    }
}
