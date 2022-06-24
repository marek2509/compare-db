using Porównator_Baz.Entities.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porównator_Baz.Entities
{
    [Table("DZIALKA")]
    public class Dzialka : IDzialka
    {
        [Key]
        public int ID_ID { get; set; }
        public string IDD { get; set; }
        public int PEW { get; set; }
        public string SIDD { get; set; }
        public string KW { get; set; }

        [ForeignKey("Obreb")]
        public int IDOBR { get; set; }
        public Obreby Obreb { get; set; } 
        [ForeignKey("Jedn_rej")]
        public int RJDR { get; set; }
        public Jedn_rej Jedn_rej { get; set; }
    }
}
