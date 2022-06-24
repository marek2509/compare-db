using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porównator_Baz.Entities.Interface
{
    public interface IRodz_wlada
    {
        int ID_ID { get; set; }
        string SYMBOL { get; set; }
    }
}
