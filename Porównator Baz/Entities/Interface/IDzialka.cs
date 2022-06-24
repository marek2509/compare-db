using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porównator_Baz.Entities.Interface
{
    public interface IDzialka
    {
         int ID_ID { get; set; }
         string IDD { get; set; }
         int PEW { get; set; }
         string SIDD { get; set; }
         string KW { get; set; }
         int IDOBR { get; set; }
         int RJDR { get; set; }
    }
}
