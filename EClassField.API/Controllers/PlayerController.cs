using EClassField.API.Models;
using EClassField.Core.Domain.OneSignal;
using EClassField.Core.Domain.User;
using EClassField.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EClassField.API.Controllers
{
    public class PlayerController : ApiController
    {
        [HttpPost]
        public IHttpActionResult AddPlayer(Input id)
        {
            string PlayerID = Function.getValuePlayer(id.Value, "PlayerID");
            string DeviceID = Function.getValuePlayer(id.Value, "DeviceID");
            string DeviceModel = Function.getValuePlayer(id.Value, "DeviceModel");
            int UserID = Convert.ToInt32(Function.getValuePlayer(id.Value, "UserID"));
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {

                var player = ctx.Set<Player>().FirstOrDefault(d => d.PlayerID == PlayerID);
                if (player == null)
                {
                  player=ctx.Set<Player>().Add(new Player {
                        DeviceID = DeviceID,
                        DevicModel = DeviceModel,
                        PlayerID = PlayerID,
                        IsActive = true
                    });

                    ctx.SaveChanges();
                }
                var user = ctx.Set<User>().FirstOrDefault(d => d.Id == UserID);
                if (user != null && player != null)
                {

                    
                    ctx.Set<PlayerUser>().Add(new PlayerUser {Player=player,User=user });

                    ctx.SaveChanges();
                }
          

            }

            return Ok(new {Ok="Yes"});
        }
    }
}
