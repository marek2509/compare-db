using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Porównator_Baz.Entities.Interface
{
    public interface IInstytucje
    {
        int ID_ID { get; set; }
        string NPE { get; set; }
        string NSK { get; set; }
        string RGN { get; set; }
        string NIP { get; set; }
    }
}