using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Techamante.Data.Interfaces;
using Techamante.Domain.Interfaces;
using Techamante.Domain.Validations;

namespace Techamante.Data
{

    public abstract class Entity : IObjectState
    {
        [NotMapped]
        public ObjectState ObjectState { get; set; }


    }
}
