using Porównator_Baz.Entities;
using Porównator_Baz.Middleware;
using Porównator_Baz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Porównator_Baz.Services
{
    class JednRejService
    {
        private readonly List<RegisterUnitDto> jednRejDtoFirst = new List<RegisterUnitDto>();
        private readonly List<RegisterUnitDto> jednRejDtoSecond = new List<RegisterUnitDto>();

        public JednRejService(MainDbContext dbContextFirst, MainDbContext dbContextSecond)
        {
            if (!dbContextFirst.Database.Exists() && !dbContextSecond.Database.Exists())
            {
                MessageBox.Show("Problem połączenia z bazą.");
                throw new ConnectionDataBaseException();
            }


            var jrsFirst = dbContextFirst.Jedn_Rejs;
            foreach (var item in jrsFirst)
            {
                jednRejDtoFirst.Add(new RegisterUnitDto(item));
            }

            jednRejDtoFirst.Sort(OrderObrAndIjr);


            var jrsSecond = dbContextSecond.Jedn_Rejs;
            foreach (var item in jrsSecond)
            {
                jednRejDtoSecond.Add(new RegisterUnitDto(item));
            }
            jednRejDtoSecond.Sort(OrderObrAndIjr);
        }

        int OrderObrAndIjr(RegisterUnitDto x, RegisterUnitDto y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're
                    // equal.
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y
                    // is greater.
                    return -1;
                }
            }
            else
            {

                if (y == null)
                {
                    return 1;
                }

                    var equalObr = x.ObrebNazwa.CompareTo(y.ObrebNazwa);




                    if (equalObr != 0)
                    {
                        return equalObr;
                    }
                    else
                    {
                        return x.Ijr.CompareTo(y.Ijr);
                    }
                }
            }
        }


        public string GetDifferencesOwner()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var dtoFirst in jednRejDtoFirst)
            {
                var existUnit = jednRejDtoSecond.Exists(s => dtoFirst.Equals(s));
                if (existUnit)
                {
                    var owners = jednRejDtoSecond.SingleOrDefault(jednSec => dtoFirst.Equals(jednSec)).Podmioty;

                    if (!dtoFirst.Podmioty.SequenceEqual(owners))
                    {
                        sb.AppendLine("________________________________________________________________________________________________________________\n");
                        sb.AppendLine($"{dtoFirst.GetAllAboutUnit()}");

                        sb.AppendLine("\tBAZA 1:");
                        dtoFirst.Podmioty.ForEach(x => sb.AppendLine(x.GetAllAboutOwner()));
                        sb.AppendLine("\n\tBAZA 2:");
                        owners.ForEach(o => sb.AppendLine(o.GetAllAboutOwner()));
                    }
                }
            }

            return sb.ToString();
        }

        //public string GetAllDifferences()
        //{
        //    StringBuilder stringBuilder = new StringBuilder();

        //    stringBuilder.AppendLine("Usunięte jednostki.");
        //    stringBuilder.AppendLine(GetRemovedJednostki());

        //    stringBuilder.AppendLine("Dodane jednostki.");
        //    stringBuilder.AppendLine(GetAddedJednostki());

        //    return stringBuilder.ToString();
        //}


        public string GetDifferencesParecels(bool ignoreArea = false, bool ignoreKW = false)
        {
            StringBuilder sb = new StringBuilder();
            ParcelEqualityComparer parcComp = new ParcelEqualityComparer(ignoreKW, ignoreArea);
            foreach (var dtoFirst in jednRejDtoFirst)
            {
                var existUnit = jednRejDtoSecond.Exists(s => dtoFirst.Equals(s));
                if (existUnit)
                {
                    // select all parcels from selected unit
                    var dzialki = jednRejDtoSecond.SingleOrDefault(jednSec => dtoFirst.Equals(jednSec)).Dzialki;

                    dtoFirst.Dzialki.SequenceEqual(dzialki, parcComp);

                    if (dtoFirst.Dzialki.SequenceEqual(dzialki, parcComp))
                    {
                        continue;
                    }
                    else
                    if (dtoFirst.Dzialki.SequenceEqual(dzialki))
                    {
                        continue;
                    }

                    sb.AppendLine("________________________________________________________________________________________________________________\n");
                    sb.AppendLine($"{dtoFirst.ObrebNr}-{dtoFirst.Ijr}\t{dtoFirst.ObrebNazwa}");

                    sb.AppendLine("\tBAZA 1:");
                    dtoFirst.Dzialki.ForEach(x => sb.AppendLine(x.GetAllDataAboutDzialka()));
                    sb.AppendLine("\n\tBAZA 2:");
                    dzialki.ForEach(x => sb.AppendLine(x.GetAllDataAboutDzialka()));
                }
                else
                {
                    Console.WriteLine("NOT EXIST : " + dtoFirst);
                }
            }
            return sb.ToString();
        }

        public string GetRemovedUnits()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var dtoFirst in jednRejDtoFirst)
            {
                var exist = jednRejDtoSecond.Exists(s => dtoFirst.Equals(s));
                if (!exist)
                {
                    sb.AppendLine("________________________________________________________________________________________________________________\n");
                    sb.AppendLine($"Obręb: {dtoFirst.ObrebNr}-{dtoFirst.ObrebNazwa}\tNr JR: {dtoFirst.Ijr}");

                    sb.AppendLine($"\tWłaściciele:");
                    dtoFirst.Podmioty.ForEach(p => sb.AppendLine($"\t\t{p.GetAllAboutOwner()}"));

                    sb.AppendLine($"\tDziałki:");
                    dtoFirst.Dzialki.ForEach(d => sb.AppendLine($"\t\tDz.: {d.NrObrebu}-{d.Idd}\tPow.: {d.Pew}\tKW: {d.Kw}"));
                }
            }
            return sb.ToString();
        }

        public string GetAddedUnits()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var dtoSec in jednRejDtoSecond)
            {
                var exist = jednRejDtoFirst.Exists(s => dtoSec.Equals(s));
                if (!exist)
                {
                    sb.AppendLine("________________________________________________________________________________________________________________\n");
                    sb.AppendLine($"Obręb: {dtoSec.ObrebNr}-{dtoSec.ObrebNazwa}\tNr JR: {dtoSec.Ijr}");

                    sb.AppendLine($"\tWłaściciele:");
                    dtoSec.Podmioty.ForEach(p => sb.AppendLine($"\t\t{p.GetAllAboutOwner()}"));

                    sb.AppendLine($"\tDziałki:");
                    dtoSec.Dzialki.ForEach(d => sb.AppendLine($"\t\tDz.: {d.NrObrebu}-{d.Idd}\tPow.: {d.Pew}\tKW: {d.Kw}"));
                }
            }
            return sb.ToString();
        }
    }
}
