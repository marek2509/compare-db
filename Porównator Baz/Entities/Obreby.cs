﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Porównator_Baz.Entities
{
    [Table("OBREBY")]
    public class Obreby
    {
        [Key]
        public int ID_ID { get; set; }
        public int ID { get; set; }
        public string NAZ { get; set; }
    }
}