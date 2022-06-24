using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porównator_Baz.Entities.Interface
{
    public interface IOsoby
    {
        int ID_ID { get; set; }
        string NZW { get; set; }
        string PIM { get; set; }
        string DIM { get; set; }
        string OIM { get; set; }
        string MIM { get; set; }
        string DOS { get; set; }
        string PSL { get; set; }
    }
}
