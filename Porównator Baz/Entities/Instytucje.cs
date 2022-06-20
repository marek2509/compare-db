using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Porównator_Baz.Entities
{
    [Table("INSTYTUCJE")]
    public class Instytucje
    {
        [Key]
        public int ID_ID { get; set; }
        public string NPE { get; set; }
        public string NSK { get; set; }
        public string RGN { get; set; }
        public string NIP { get; set; } 
    }
}