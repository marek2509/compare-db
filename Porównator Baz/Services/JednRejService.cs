using Newtonsoft.Json;
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

            splaszczenieTest(jednRejDtoFirst);
        }

        public void splaszczenieTest(List<RegisterUnitDto> dtos)
        {
           var spl = jednRejDtoFirst.SelectMany(x => x.Dzialki, (jedn, dzialka)  => new
            {
             dz = dzialka,
              ijr = jedn.Ijr,
              obn =   jedn.ObrebNazwa
            });

            Console.WriteLine("Spłaszczenie ziomy");
            spl.ToList().ForEach(x => Console.WriteLine(string.Join(", ", x.ijr, x.obn, x.dz.Idd)));
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


        public string GetDifferencesParecels(bool ignoreArea = false, bool ignoreKW = false)
        {


            //var query = jednRejDtoFirst.SelectMany(jedn => jedn.Dzialki, (jedn, dzialka) => new
            //{
            //    Ijr = jedn.Ijr,
            //    Dzialka = string.Join("-", dzialka.NrObrebu, dzialka.Idd),

            //});
            //Console.WriteLine(string.Join(",", query));
            //Console.WriteLine(JsonConvert.SerializeObject(jednRejDtoFirst));

            StringBuilder sb = new StringBuilder();
            ParcelEqualityComparer parcComp = new ParcelEqualityComparer(ignoreKW, ignoreArea);
            foreach (var dtoFirst in jednRejDtoFirst)
            {
                var existUnit = jednRejDtoSecond.Exists(s => dtoFirst.Equals(s));
                if (existUnit)
                {
                    // select all parcels from selected unit
                    var dzialkiSecond = jednRejDtoSecond.SingleOrDefault(jednSec => dtoFirst.Equals(jednSec)).Dzialki;
                    var dzialkiFirst = dtoFirst.Dzialki;

                    if (dzialkiFirst.SequenceEqual(dzialkiSecond, parcComp))
                    {
                        continue;
                    }


                    //var maxLengthDz = dzialkiFirst.Max(d => (d.NrObrebu + d.Idd).Length);
                    //var maxLengthPew = dzialkiFirst.Max(d => d.Pew.ToString().Length);

                    //var maxLengthDzSec = dzialkiSecond.Max(d => (d.NrObrebu + d.Idd).Length);
                    //var maxLengthPewSec = dzialkiSecond.Max(d => d.Pew.ToString().Length);

                    //sb.AppendLine("\tBAZA 1:");
                    //dzialkiFirst.ForEach(x => sb.AppendLine(x.GetAllDataAboutDzialka(maxLengthKw)));
                    //sb.AppendLine("\n\tBAZA 2:");
                    //dzialkiSecond.ForEach(x => sb.AppendLine(x.GetAllDataAboutDzialka(maxLengthKwSec)));

                    sb.AppendLine("________________________________________________________________________________________________________________\n");
                    sb.AppendLine($"{dtoFirst.ObrebNr}-{dtoFirst.Ijr}\t{dtoFirst.ObrebNazwa}");
                    var maxLengthKw = dzialkiFirst.Max(d => d.Kw.Length);
                    var maxLengthKwSec = dzialkiSecond.Max(d => d.Kw.Length);
                    var countFirst = dzialkiFirst.Count();
                    var countSecond = dzialkiSecond.Count();

                    List<ParcelsDto> newParcelsList = new List<ParcelsDto>();
                    newParcelsList.AddRange(dzialkiFirst);
                    newParcelsList.AddRange(dzialkiSecond);

                    var countRowsInReport = newParcelsList.Select(x => x.NrObrebu + "-" + x.Idd).Distinct().ToList().Count;

                    int indexFirstParcels = 0;
                    int indexSecondParcels = 0;

                    var maxLengthFirst = dzialkiFirst.Max(d => d.GetAllDataAboutDzialka(maxLengthKw).Length);
                    var maxLengthSecond = dzialkiSecond.Max(d => d.GetAllDataAboutDzialka(maxLengthKwSec).Length);


                    sb.AppendLine($"BAZA 1:" + "\t" + "BAZA2:".PadLeft(maxLengthFirst));
                    for (int i = 0; i < countRowsInReport; i++)
                    {
                        var resultComparison = indexFirstParcels < countFirst && indexSecondParcels < countSecond ? dzialkiFirst[indexFirstParcels].Sidd.CompareTo(dzialkiSecond[indexSecondParcels].Sidd) : -5;


                        if (indexFirstParcels >= countFirst || resultComparison == 1)
                        {
                            sb.AppendLine($"{dzialkiSecond[indexSecondParcels].GetRightSiteDataAboutDzialka(maxLengthKw, maxLengthKwSec)}");
                            indexSecondParcels++;
                        }
                        else

                        if (indexSecondParcels >= countSecond || resultComparison == -1)
                        {
                            sb.AppendLine($"{dzialkiFirst[indexFirstParcels].GetAllDataAboutDzialka(maxLengthKw)}");
                            indexFirstParcels++;
                        }
                        else
                        if (resultComparison == 0)
                        {

                            sb.AppendLine(dzialkiFirst[indexFirstParcels].GetAllDataAboutDzialka(maxLengthKw) + "\t\t\t" + dzialkiSecond[indexSecondParcels].GetAllDataAboutDzialka(maxLengthKwSec));
                            indexFirstParcels++;
                            indexSecondParcels++;
                        }
                    }



                }
                else
                {
                    Console.WriteLine("NOT EXIST : " + dtoFirst.Ijr);
                }
            }
            return sb.ToString();
        }

        internal string SaveAsTxtAddedUnits()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Join("\t", "Nr jednostki", "Obręb", "Właściciel", "Działka", "Powierzchnia", "KW"));
            foreach (var dtoSec in jednRejDtoSecond)
            {
                var exist = jednRejDtoFirst.Exists(s => dtoSec.Equals(s));
                if (!exist)
                {
                    sb.AppendLine(string.Join("\t", dtoSec.GetAll('\t'), "", "", "", ""));
                    dtoSec.Podmioty.ForEach(p => sb.AppendLine(string.Join("\t", "", "", p.GetAll(), "", "", "")));
                    dtoSec.Dzialki.ForEach(d => sb.AppendLine(string.Join("\t", "", "", "", d.GetAll('\t'))));
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }

        public string SaveAsTxtDifferencesParcels(bool ignoreArea = false, bool ignoreKW = false)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Join("\t", "Nr jednostki", "Obręb" ,"Baza 1", "Działka", "Powierzchnia", "KW", "Baza 2", "Działka", "Powierzchnia", "KW"));

            ParcelEqualityComparer parcComp = new ParcelEqualityComparer(ignoreKW, ignoreArea);
            foreach (var dtoFirst in jednRejDtoFirst)
            {
                var existUnit = jednRejDtoSecond.Exists(s => dtoFirst.Equals(s));
                if (existUnit)
                {
                    // select all parcels from selected unit
                    var dzialkiSecond = jednRejDtoSecond.SingleOrDefault(jednSec => dtoFirst.Equals(jednSec)).Dzialki;
                    var dzialkiFirst = dtoFirst.Dzialki;

                    if (dzialkiFirst.SequenceEqual(dzialkiSecond, parcComp))
                    {
                        continue;
                    }

                    var maxLengthKw = dzialkiFirst.Max(d => d.Kw.Length);
                    var maxLengthKwSec = dzialkiSecond.Max(d => d.Kw.Length);
                    var countFirst = dzialkiFirst.Count();
                    var countSecond = dzialkiSecond.Count();

                    List<ParcelsDto> newParcelsList = new List<ParcelsDto>();
                    newParcelsList.AddRange(dzialkiFirst);
                    newParcelsList.AddRange(dzialkiSecond);

                    var countRowsInReport = newParcelsList.Select(x => x.NrObrebu + "-" + x.Idd).Distinct().ToList().Count;

                    int indexFirstParcels = 0;
                    int indexSecondParcels = 0;

                    var maxLengthFirst = dzialkiFirst.Max(d => d.GetAllDataAboutDzialka(maxLengthKw).Length);
                    var maxLengthSecond = dzialkiSecond.Max(d => d.GetAllDataAboutDzialka(maxLengthKwSec).Length);


                    sb.AppendLine(dtoFirst.GetAll('\t'));
                    sb.AppendLine("\t\tBAZA 1:" + "\t\t\t\t" + "BAZA 2:");
                    string tabBeforParcet = "\t\t\t";
                    for (int i = 0; i < countRowsInReport; i++)
                    {
                        var resultComparison = indexFirstParcels < countFirst && indexSecondParcels < countSecond ? dzialkiFirst[indexFirstParcels].Sidd.CompareTo(dzialkiSecond[indexSecondParcels].Sidd) : -5;


                        if (indexFirstParcels >= countFirst || resultComparison == 1)
                        {
                            sb.AppendLine(tabBeforParcet + "\t\t\t\t" + dzialkiSecond[indexSecondParcels].GetAll('\t'));
                            indexSecondParcels++;
                        }
                        else

                        if (indexSecondParcels >= countSecond || resultComparison == -1)
                        {
                            sb.AppendLine(tabBeforParcet + dzialkiFirst[indexFirstParcels].GetAll('\t'));
                            indexFirstParcels++;
                        }
                        else
                        if (resultComparison == 0)
                        {

                            sb.AppendLine(tabBeforParcet + dzialkiFirst[indexFirstParcels].GetAll('\t') + "\t\t" + dzialkiSecond[indexSecondParcels].GetAll('\t'));
                            indexFirstParcels++;
                            indexSecondParcels++;
                        }
                    }


                    sb.AppendLine();
                }
                else
                {
                    Console.WriteLine("NOT EXIST : " + dtoFirst.Ijr);
                }
            }
            return sb.ToString();
        }

        public string SaveAsTxtDeletedUnits()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Join("\t", "Nr jednostki", "Obręb", "Właściciel", "Działka", "Powierzchnia", "KW"));
            foreach (var dtoFirst in jednRejDtoFirst)
            {
                var exist = jednRejDtoSecond.Exists(s => dtoFirst.Equals(s));
                if (!exist)
                {

                    //sb.AppendLine($"Obręb: {dtoFirst.ObrebNr}-{dtoFirst.ObrebNazwa}\tNr JR: {dtoFirst.Ijr}");
                    sb.AppendLine(string.Join("\t", dtoFirst.GetAll('\t'), "", "", "", ""));

                    //sb.AppendLine($"\tWłaściciele:");
                    //dtoFirst.Podmioty.ForEach(p => sb.AppendLine($"\t\t{p.GetAllAboutOwner()}"));
                    dtoFirst.Podmioty.ForEach(p => sb.AppendLine(string.Join("\t", "", "", p.GetAll(), "", "", "")));

                    //sb.AppendLine($"\tDziałki:");
                    dtoFirst.Dzialki.ForEach(d => sb.AppendLine(string.Join("\t", "", "", "", d.GetAll('\t'))));
                    sb.AppendLine();
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
