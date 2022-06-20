using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porównator_Baz.Entities
{
    [Table("OSOBY")]
    public class Osoby
    {
        [Key]
        public int ID_ID { get; set; }
        public string NZW { get; set; }
        public string PIM { get; set; }
        public string DIM { get; set; }
        public string OIM { get; set; }
        public string MIM { get; set; }
        public string DOS { get; set; }
        public string PSL { get; set; }
    }
}
