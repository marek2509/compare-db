using Porównator_Baz.Entities;
using Porównator_Baz.Entities.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porównator_Baz.Models
{
    public class OwnerDto : IEquatable<OwnerDto>
    {
        public double UdzialNr { get; set; }
        public string Udzial { get; set; }
        public string Nazwa { get; set; }
        public string RWD { get; set; }

        public OwnerDto(IUdzialy udzialy)
        {
            var udzial = udzialy as Udzialy;
            Udzial = udzial.UD;
            UdzialNr = udzial.UD_NR;
            RWD = udzial.RodzajWladania.SYMBOL;

            var typPodmiotu = udzial.Podmiot.TYP;

            if (typPodmiotu == "F")
            {
                var osobaFizyczna = udzial.Podmiot.OsobaFizyczna;
                Nazwa = $"{osobaFizyczna.NZW} {osobaFizyczna.PIM} {osobaFizyczna.DIM} " +
                        $"({osobaFizyczna.OIM}, {osobaFizyczna.MIM})";
            }
            else if (typPodmiotu == "M")
            {
                var malzenstwo = udzial.Podmiot.Malzenstwo;
                var maz = malzenstwo.Maz;
                var zona = malzenstwo.Zona;

                Nazwa = $"MĄŻ: {maz.NZW} {maz.PIM} {maz.DIM} " +
                        $"({maz.OIM}, {maz.MIM})\t" +
                        $"ŻONA: {zona.NZW} {zona.PIM} {zona.DIM} " +
                        $"({zona.OIM}, {zona.MIM})";
            }
            else if (typPodmiotu == "P")
            {
                var instytucja = udzial.Podmiot.Instytucja;
                Nazwa = instytucja.NPE;
            }
            else if (typPodmiotu == "I")
            {
                var innyPodmiot = udzial.Podmiot.InnyPodmiot;
                Nazwa = innyPodmiot.NPE;
            }
        }

        public bool Equals(OwnerDto owner)
        {
            if (owner is null)
                return false;

            return Udzial == owner.Udzial &&
                   UdzialNr == owner.UdzialNr &&
                   Nazwa == owner.Nazwa &&
                   RWD == owner.RWD;
        }

        public override bool Equals(object obj) => Equals(obj as OwnerDto);

        public string GetAllAboutOwner()
        {
            return $"\t\t{RWD}\t{Udzial}\t{Nazwa}";
        }
    }
}
