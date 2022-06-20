using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Porównator_Baz.Entities
{
    [Table("JEDN_REJ")]
    public class Jedn_rej
    {
        [Key]
        public int ID_ID { get; set; }
        public int IJR { get; set; }

        [ForeignKey("Obreb")]
        public int ID_OBR { get; set; }
        public virtual Obreby Obreb { get; set; }

        public virtual List<Dzialka> Dzialki { get; set; }

        public virtual List<Udzialy> Udzialy { get; set; }
    }
}