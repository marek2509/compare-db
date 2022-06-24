using Porównator_Baz.Entities.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Porównator_Baz.Entities
{
    [Table("INNE_PODM")]
    public class InnePodmioty : IInnePodmioty
    {
        [Key]
        public int ID_ID { get; set; }
        public string NPE { get; set; }
        public string NSK { get; set; }
    }
}