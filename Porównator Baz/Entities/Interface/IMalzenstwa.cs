using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Porównator_Baz.Entities.Interface
{
    public interface IMalzenstwa
    {
        int ID_ID { get; set; }
        int ID_MAZ { get; set; }
        int ID_ZONA { get; set; }
    }
}