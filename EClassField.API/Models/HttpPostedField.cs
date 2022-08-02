using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EClassField.API.Models
{
    public class HttpPostedField
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public byte[] File { get; set; }
        public HttpPostedField(string name,string value,byte[] file)
        {
            name = Name;
            value = Value;
            File = file;
        }
    }
}