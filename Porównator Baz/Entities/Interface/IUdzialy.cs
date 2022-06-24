using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porównator_Baz.Entities.Interface
{
    public interface IUdzialy
    {
         int ID_ID { get; set; }
         string UD { get; set; }
         double UD_NR { get; set; }
         string GRJ { get; set; }
         int ID_JEDN { get; set; }
         int ID_PODM { get; set; }
         int RWD { get; set; }

    }
}
