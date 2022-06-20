using Porównator_Baz.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porównator_Baz.Models
{
    public class JednRejDto : IEquatable<JednRejDto>
    {
        public int Ijr { get; set; }
        public int ObrebNr { get; set; }
        public string ObrebNazwa { get; set; }

        public List<PodmiotDto> Podmioty = new List<PodmiotDto>();
        public List<DzialkiDto> Dzialki = new List<DzialkiDto>();

        public JednRejDto(Jedn_rej dbJednRej)
        {
            Ijr = dbJednRej.IJR;

            var obreb = dbJednRej.Obreb;
            ObrebNr = obreb.ID;
            ObrebNazwa = dbJednRej.Obreb.NAZ;

            var dbDzialki = dbJednRej.Dzialki;
            Dzialki = dbDzialki.Select(dzialka => new DzialkiDto(dzialka)).ToList();

            var dbUdzialy = dbJednRej.Udzialy;
            Podmioty = dbUdzialy.Select(udzial => new PodmiotDto(udzial)).ToList();
        }

        public static JednRejDto ConvertObject(Jedn_rej dbJednRej)
        {
            return new JednRejDto(dbJednRej);
        }

        public bool Equals(JednRejDto jDto)
        {
            if (jDto is null)
                return false;
 
                return ObrebNazwa == jDto.ObrebNazwa && ObrebNr == jDto.ObrebNr && Ijr == jDto.Ijr;
        }

        public string GetDiferenceDzialki()
        {
            StringBuilder sb = new StringBuilder();



            return sb.ToString();
        }


        public override bool Equals(object obj) => Equals(obj as JednRejDto);
    }
}
