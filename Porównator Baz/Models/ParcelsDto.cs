﻿using Porównator_Baz.Entities;
using Porównator_Baz.Entities.Interface;
using System;

namespace Porównator_Baz.Models
{
    public class ParcelsDto : IEquatable<ParcelsDto>
    {
        public string Idd { get; set; }
        public int Pew { get; set; }
        public int NrObrebu { get; set; }
        public string Sidd { get; set; }
        public string Kw { get; set; }

        public ParcelsDto(Dzialka dzialka)
        {
            Idd = dzialka.IDD.Trim();
            NrObrebu = dzialka.Obreb.ID;
            Kw = dzialka.KW.Trim();
            Pew = dzialka.PEW;
            Sidd = dzialka.SIDD;
        }

        public bool Equals(ParcelsDto dz, bool ignoreKW = false, bool ignoreArea = false)
        {
            if (dz is null)
                return false;

            if (ignoreArea && ignoreKW)
            {
                return Idd == dz.Idd &&
                       NrObrebu == dz.NrObrebu;
            }

            if (ignoreKW)
            {
                return Idd == dz.Idd &&
                       Pew == dz.Pew &&
                       NrObrebu == dz.NrObrebu;
            }


            if (ignoreArea)
            {
                return Idd == dz.Idd &&
                        Kw == dz.Kw &&
                       NrObrebu == dz.NrObrebu;
            }


            return Idd == dz.Idd &&
                   Kw == dz.Kw &&
                   Pew == dz.Pew &&
                   NrObrebu == dz.NrObrebu;
        }

        public bool Equals(ParcelsDto dz)
        {
            if (dz is null)  return false;

            return Idd == dz.Idd &&
                   Kw == dz.Kw &&
                   Pew == dz.Pew &&
                   NrObrebu == dz.NrObrebu;
        }

        public override bool Equals(object obj) => Equals(obj as ParcelsDto);

        public string GetAllDataAboutDzialka(int padKW = 100, int padObrNrdz = 20, int padPwe = 20)
        {
            var lnParcel = $"{NrObrebu}-{Idd}".Length;
            var lnPew = Pew.ToString().Length;
            var lnKw = Kw.Length;


            return ($"{NrObrebu}-{Idd}".PadRight(15) + $"\tPow.: {Pew}".PadRight(15) + $"\tKW: {Kw}".PadRight(padKW+10));
        }
    }
}