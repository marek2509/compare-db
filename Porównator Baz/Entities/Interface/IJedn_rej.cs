using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Porównator_Baz.Entities.Interface
{
    public interface IJedn_rej
    {
        int ID { get; set; }
        int IJR { get; set; }
        int ID_OBR { get; set; }
    }
}