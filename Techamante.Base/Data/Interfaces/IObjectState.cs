
using System.ComponentModel.DataAnnotations.Schema;

namespace Techamante.Data.Interfaces
{
    public interface IObjectState
    {
        [NotMapped]
        ObjectState ObjectState { get; set; }
    }
}