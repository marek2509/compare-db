using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Porównator_Baz.Entities.Interface
{
    public interface IInnePodmioty
    {
         int ID_ID { get; set; }
         string NPE { get; set; }
         string NSK { get; set; }
    }
}