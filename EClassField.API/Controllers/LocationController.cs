using EClassField.Core.Domain.Directory;
using EClassField.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EClassField.API.Controllers
{
    public class LocationController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="typeID">1 Country | 2 City |  3 Town  | 4 Area  | 5 Neighboord|</param>
        /// <returns></returns>
        public IHttpActionResult Get(int ID, int typeID)
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {



                switch (typeID)
                {
                    //case 5:
                    //    return Ok(ctx.Set<Neighborhood>().Where(d => d.AreaID == ID).Select(d => new {Name=d.Name,Value=d.Id }).ToList());
                    case 3:
                        return Ok(ctx.Set<Neighborhood>().Where(d => d.TownId == ID).Select(d => new { Name = d.Name, Value = d.Id }).OrderBy(x=>x.Name).ToList());
                    case 2:
                        return Ok(ctx.Set<Town>().Where(d => d.CityId == ID).Select(d => new { Name = d.Name, Value = d.TownId}).OrderBy(x=>x.Name).ToList());
                    case 1:
                        return Ok(ctx.Set<City>().Select(d => new { Name = d.Name, Value = d.CityId }).OrderBy(x=>x.Name).ToList());
                    default:
                        return Ok(ctx.Set<City>().Select(d => new { Name = d.Name, Value = d.CityId}).ToList());
                        
                }

            }

        }
    }
}
