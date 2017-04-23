using System;

namespace Techamante.Core
{
    public class DisplayAttribute : Attribute
    {
        public string Name { get; set; }

        public DisplayAttribute(string name)
        {
            Name = name;
        }
    }
}
