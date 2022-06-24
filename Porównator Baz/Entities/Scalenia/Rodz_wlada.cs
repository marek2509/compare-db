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
    [Table("RODZ_WLADA")]
    public class Rodz_wlada : IRodz_wlada
    {
        [Key]
        public int ID_ID { get; set; }
        public string SYMBOL { get; set; }
    }
}
