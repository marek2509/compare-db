using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Porównator_Baz.Entities.Interface
{
    public interface IObreby
    {
         int ID_ID { get; set; }
         int ID { get; set; }
         string NAZ { get; set; }
    }
}