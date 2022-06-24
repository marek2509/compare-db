using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Porównator_Baz.Entities.Interface
{
    public interface IPodmioty
    {
        int ID_ID { get; set; }
        string TYP { get; set; }
        int ID_OS { get; set; }
    }
}