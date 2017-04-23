using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Techamante.Web
{
    public class CommandAttribute : Attribute
    {
        public IEnumerable<Type> Commands { get; }
        public CommandAttribute(params Type[] commands)
        {
            Commands = commands;
        }

    }
}
