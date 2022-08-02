﻿using EClassField.Core.Domain.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EClassField.API.Models
{
    public class AttributeModel
    {
        public int AttributeId { get; set; }
        public int SubAttributeId { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
        public AttributeType     Type { get; set; }
    }
}