using EClassField.Core.Domain.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EClassFieldWeb_.Models
{
    public class Input
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public string Icon { get; set; }
        public string EndText { get; set; }
        public int AttributeId { get; set; }
        public int SubAttributeId { get; set; }
        public AttributeType Type { get; set; }

        public List<Input> SubInputs { get; set; }
    }

    public class LabelInput
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    
}