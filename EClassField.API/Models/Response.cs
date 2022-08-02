using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EClassField.API.Models
{
    public class Response
    {

        public string Exception { get; set; }

        public bool Result { get; set; }

        public object Value { get; set; }
    }
}