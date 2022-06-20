using Porównator_Baz.Entities;
using Porównator_Baz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porównator_Baz.Services
{
    class JednRejService
    {
        readonly List<JednRejDto> jednRejDtoFirst = new List<JednRejDto>();
        readonly List<JednRejDto> jednRejDtoSecond = new List<JednRejDto>();

        public JednRejService(MainDbContext dbContextFirst, MainDbContext dbContextSecond)
        {
            var jrsFirst = dbContextFirst.Jedn_Rejs;
            foreach (var item in jrsFirst)
            {
                jednRejDtoFirst.Add(new JednRejDto(item));
            }

            var jrsSecond = dbContextSecond.Jedn_Rejs;
            foreach (var item in jrsSecond)
            {
                jednRejDtoSecond.Add(new JednRejDto(item));
            }
        }

        public string GetAllDifferences()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("Usunięte jednostki.");
            stringBuilder.AppendLine(GetRemovedJednostki());

            stringBuilder.AppendLine("Dodane jednostki.");
            stringBuilder.AppendLine(GetAddedJednostki());


            return stringBuilder.ToString();
        }


        public string GetDifferencesDzialki()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var dtoFirst in jednRejDtoFirst)
            {
                var exist = jednRejDtoSecond.Exists(s => dtoFirst.Equals(s));
                if (exist)
                {
                    var dzialki = jednRejDtoSecond.SingleOrDefault(jednSec => dtoFirst.Equals(jednSec)).Dzialki;
                    sb.AppendLine($"{dtoFirst.ObrebNr}-{dtoFirst.Ijr}\t{dtoFirst.ObrebNazwa}");

                    if (!dtoFirst.Dzialki.SequenceEqual(dzialki))
                    {
                        sb.AppendLine("BAZA 1:");
                        dtoFirst.Dzialki.ForEach(x => sb.AppendLine(x.GetAllDataAboutDzialka()));
                        sb.AppendLine("\nBAZA 2:");
                        dzialki.ForEach(x => sb.AppendLine(x.GetAllDataAboutDzialka()));
                    }

                }
            }

            return sb.ToString();
        }

        public string GetRemovedJednostki()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var dtoFirst in jednRejDtoFirst)
            {
                var exist = jednRejDtoSecond.Exists(s => dtoFirst.Equals(s));
                if (!exist)
                {
                    sb.AppendLine("--------------------------------------------------------\n");
                    sb.AppendLine($" Obręb: {dtoFirst.ObrebNr}-{dtoFirst.ObrebNazwa}\tNr JR: {dtoFirst.Ijr}");

                    sb.AppendLine($"\tWłaściciele:");
                    dtoFirst.Podmioty.ForEach(p => sb.AppendLine($"\t\t{p.RWD} \t{p.Udzial} \t{p.Nazwa}"));

                    sb.AppendLine($"\tDziałki:");
                    dtoFirst.Dzialki.ForEach(d => sb.AppendLine($"\t\tDz.: {d.NrObrebu}-{d.Idd}\tPow.: {d.Pew}\tKW: {d.Kw}"));
                }
            }
            return sb.ToString();
        }

        public string GetAddedJednostki()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var dtoSec in jednRejDtoSecond)
            {
                var exist = jednRejDtoFirst.Exists(s => dtoSec.Equals(s));
                if (!exist)
                {
                    sb.AppendLine("--------------------------------------------------------\n");
                    sb.AppendLine($"Obręb: {dtoSec.ObrebNr}-{dtoSec.ObrebNazwa}\tNr JR: {dtoSec.Ijr}");

                    sb.AppendLine($"\tWłaściciele:");
                    dtoSec.Podmioty.ForEach(p => sb.AppendLine($"\t\t{p.RWD} \t{p.Udzial} \t{p.Nazwa}"));

                    sb.AppendLine($"\tDziałki:");
                    dtoSec.Dzialki.ForEach(d => sb.AppendLine($"\t\tDz.: {d.NrObrebu}-{d.Idd}\tPow.: {d.Pew}\tKW: {d.Kw}"));
                }
            }
            return sb.ToString();
        }
    }
}
