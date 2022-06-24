using Porównator_Baz.Entities;
using Porównator_Baz.Entities.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porównator_Baz.Models
{
    public class RegisterUnitDto : IEquatable<RegisterUnitDto>
    {
        public int Ijr { get; set; }
        public int ObrebNr { get; set; }
        public string ObrebNazwa { get; set; }

        public List<OwnerDto> Podmioty = new List<OwnerDto>();
        public List<ParcelsDto> Dzialki = new List<ParcelsDto>();

        public RegisterUnitDto(Jedn_rej dbJednRej)
        {
            Ijr = dbJednRej.IJR;

            var obreb = dbJednRej.Obreb;
            ObrebNr = obreb.ID;
            ObrebNazwa = dbJednRej.Obreb.NAZ;

            var dbDzialki = dbJednRej.Dzialki.OrderBy(x => x.SIDD);
            Dzialki = dbDzialki.Select(dzialka => new ParcelsDto(dzialka)).ToList();

            var dbUdzialy = dbJednRej.Udzialy;
            Podmioty = dbUdzialy.Select(udzial => new OwnerDto(udzial)).ToList();
            Podmioty = Podmioty.OrderBy(x => x.UdzialNr).OrderBy(x => x.Nazwa).ToList();
        }

        public static RegisterUnitDto ConvertObject(Jedn_rej dbJednRej)
        {
            return new RegisterUnitDto(dbJednRej);
        }

        public bool Equals(RegisterUnitDto jDto)
        {
            if (jDto is null)
                return false;
 
                return ObrebNazwa == jDto.ObrebNazwa && ObrebNr == jDto.ObrebNr && Ijr == jDto.Ijr;
        }

        public override bool Equals(object obj) => Equals(obj as RegisterUnitDto);

        public string GetAllAboutUnit()
        {
            return $"{ObrebNr}-{Ijr}\t{ObrebNazwa}";
        }
    }
}
