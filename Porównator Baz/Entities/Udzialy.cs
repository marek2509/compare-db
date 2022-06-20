using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porównator_Baz.Entities
{
    [Table("UDZIALY")]
    public class Udzialy
    {
        [Key]
        public int ID_ID { get; set; }
        public string UD { get; set; }
        public double UD_NR { get; set; }
        public string GRJ { get; set; }

        [ForeignKey("Jedn_rej")]
        public int ID_JEDN { get; set; }
        public virtual Jedn_rej Jedn_rej { get; set; }

        [ForeignKey("Podmiot")]
        public int ID_PODM { get; set; }
        public virtual Podmioty Podmiot { get; set; }

        [ForeignKey("RodzajWladania")]
        public int RWD { get; set; }
        public virtual Rodz_wlada RodzajWladania { get; set; }
    }
}
