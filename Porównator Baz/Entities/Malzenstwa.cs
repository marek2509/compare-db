using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Porównator_Baz.Entities
{
    [Table("MALZENSTWA")]
    public class Malzenstwa
    {
        [Key]
        public int ID_ID { get; set; }

        [ForeignKey("Maz")]
        [Column("MAZ")]
        public int ID_MAZ { get; set; }
        public virtual Osoby Maz { get; set; }

        [ForeignKey("Zona")]
        [Column("ZONA")]
        public int ID_ZONA { get; set; }
        public virtual Osoby Zona { get; set; }
    }
}