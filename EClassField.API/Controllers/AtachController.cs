using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EClassField.API.Controllers
{
    public class AtachController : ApiController
    {

        [ActionName("UploadProductImage")]
        public IHttpActionResult UploadProductImage(string base64)
        {


            return Ok(base64);
        }
    }
}
