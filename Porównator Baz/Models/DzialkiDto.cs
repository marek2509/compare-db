using Porównator_Baz.Entities;
using System;

namespace Porównator_Baz.Models
{
    public class DzialkiDto : IEquatable<DzialkiDto>
    {
        public string Idd { get; set; }
        public int Pew { get; set; }
        public int NrObrebu { get; set; }
        public string Sidd { get; set; }
        public string Kw { get; set; }

        public DzialkiDto(Dzialka dzialka)
        {
            Idd = dzialka.IDD;
            NrObrebu = dzialka.Obreb.ID;
            Kw = dzialka.KW;
            Pew = dzialka.PEW;
            Sidd = dzialka.SIDD;
        }

        public bool Equals(DzialkiDto dz)
        {
            if (dz is null)
                return false;

            return Idd == dz.Idd &&
                   Kw == dz.Kw &&
                   Pew == dz.Pew &&
                   NrObrebu == dz.NrObrebu;
        }

        public override bool Equals(object obj) => Equals(obj as DzialkiDto);

        public string GetAllDataAboutDzialka()
        {
            return $"{NrObrebu}-{Idd}\tPow.: {Pew}\tKW: {Kw}";
        }
    }
}