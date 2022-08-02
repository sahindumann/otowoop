using EClassField.Core.Domain.User;
using EClassField.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EClassFieldWeb_.Models
{
    public class General
    {

        public static User User
        {
            get
            {


                using (ClassFieldDbContext ctx = new ClassFieldDbContext())
                {

                    var user = HttpContext.Current.Request.Cookies["Userotomarket"];

                    if (user != null)
                    {
                        string userr = user.Value;

                        if (!String.IsNullOrEmpty(userr))
                        {




                            var userdb = ctx.Set<User>().FirstOrDefault(d => d.Password == userr);

                            return userdb ?? null;
                        }
                    }

                    return null;

                }



            }
        }


   
    }
}