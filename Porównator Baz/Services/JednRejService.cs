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


            StringBuilder sb = new StringBuilder();
            ParcelEqualityComparer parcComp = new ParcelEqualityComparer(ignoreKW, ignoreArea);
            foreach (var dtoFirst in jednRejDtoFirst)
            {
                var existUnit = jednRejDtoSecond.Exists(s => dtoFirst.Equals(s));
                if (existUnit)
                {
                    // select all parcels from selected unit
                    var dzialkiSecond = jednRejDtoSecond.SingleOrDefault(jednSec => dtoFirst.Equals(jednSec)).Dzialki;

                    dtoFirst.Dzialki.SequenceEqual(dzialkiSecond, parcComp);

                    if (dtoFirst.Dzialki.SequenceEqual(dzialkiSecond, parcComp))
                    {
                        continue;
                    }
                    else
                    if (dtoFirst.Dzialki.SequenceEqual(dzialkiSecond))
                    {
                        continue;
                    }



                    sb.AppendLine("________________________________________________________________________________________________________________\n");
                    sb.AppendLine($"{dtoFirst.ObrebNr}-{dtoFirst.Ijr}\t{dtoFirst.ObrebNazwa}");
                    var dzialkiFirst = dtoFirst.Dzialki;
                    var maxLengthKw = dzialkiFirst.Max(d => d.Kw.Length);
                    var maxLengthDz = dzialkiFirst.Max(d => (d.NrObrebu + d.Idd).Length);
                    var maxLengthPew = dzialkiFirst.Max(d => d.Pew.ToString().Length);

                    var maxLengthKwSec = dzialkiSecond.Max(d => d.Kw.Length);
                    var maxLengthDzSec = dzialkiSecond.Max(d => (d.NrObrebu + d.Idd).Length);
                    var maxLengthPewSec = dzialkiSecond.Max(d => d.Pew.ToString().Length);

                    sb.AppendLine("\tBAZA 1:");
                    dzialkiFirst.ForEach(x => sb.AppendLine(x.GetAllDataAboutDzialka(maxLengthKw, maxLengthDz, maxLengthPew)));
                    sb.AppendLine("\n\tBAZA 2:");
                    dzialkiSecond.ForEach(x => sb.AppendLine(x.GetAllDataAboutDzialka(maxLengthKwSec, maxLengthDzSec, maxLengthPewSec)));

                    var countFirst = dzialkiFirst.Count();
                    var countSecond = dzialkiSecond.Count();

                    var countRowsInReport = countFirst > countSecond ? countFirst : countSecond;

                    int indexFirstParcels = 0;
                    int indexSecondParcels = 0;

                    var maxLengthFirst = dzialkiFirst.Max(d => d.GetAllDataAboutDzialka(maxLengthKw, maxLengthDz, maxLengthPew).Length);
                    var maxLengthSecond = dzialkiSecond.Max(d => d.GetAllDataAboutDzialka(maxLengthKwSec, maxLengthDzSec, maxLengthPewSec).Length);


                    sb.AppendLine($"BAZA 1:" + "\t" + "BAZA2:".PadLeft(maxLengthFirst));
                    for (int i = 0; i < countRowsInReport; i++)
                    {


                        void genSecond()
                        {
                            //sb.AppendLine("".PadLeft(maxLengthFirst + maxLengthSecond) + $"\t\t\t{dzialkiSecond[indexSecondParcels].GetAllDataAboutDzialka(maxLengthKwSec, maxLengthDzSec, maxLengthPewSec)}");
                            //sb.AppendLine("".PadRight(maxLengthFirst + maxLengthSecond) + $"\t\t\t{dzialkiSecond[indexSecondParcels].GetAllDataAboutDzialka(maxLengthKwSec, maxLengthDzSec, maxLengthPewSec)}");

                            string strDz = "";

                            for (int j = 0; j < 15; j++)
                            {
                                strDz += " ";
                            }
                            strDz += "\t";
                            for (int k = 0; k < 15; k++)
                            {
                                strDz += " ";
                            }
                            strDz += "\t";
                            for (int l = 0; l < maxLengthKwSec; l++)
                            {
                                strDz += " ";
                            }

                            sb.AppendLine(strDz + "\t\t\t" + dzialkiSecond[indexSecondParcels].GetAllDataAboutDzialka(maxLengthKwSec, maxLengthDzSec, maxLengthPewSec));
                        }



                        if (indexFirstParcels >= countFirst)
                        {
                            genSecond();
                            indexSecondParcels++;
                            continue;
                        }

                        if (indexSecondParcels >= countSecond)
                        {
                            sb.AppendLine($"{dzialkiFirst[indexFirstParcels].GetAllDataAboutDzialka(maxLengthKw, maxLengthDz, maxLengthPew)}");
                            indexFirstParcels++;
                            continue;
                        }

                        var resultComparison = dzialkiFirst[indexFirstParcels].Sidd.CompareTo(dzialkiSecond[indexSecondParcels].Sidd);
                        //Console.WriteLine(dzialkiFirst[indexFirstParcels].Sidd + "<"+ resultComparison +">" + dzialkiSecond[indexSecondParcels].Sidd);
                        if (resultComparison == 0)
                        {

                            sb.AppendLine(dzialkiFirst[indexFirstParcels].GetAllDataAboutDzialka(maxLengthKw, maxLengthDz, maxLengthPew) + "\t\t\t" + dzialkiSecond[indexSecondParcels].GetAllDataAboutDzialka(maxLengthKwSec, maxLengthDzSec, maxLengthPewSec));
                            indexFirstParcels++;
                            indexSecondParcels++;
                        }
                        else if (resultComparison == -1)
                        {
                            sb.AppendLine($"{dzialkiFirst[indexFirstParcels].GetAllDataAboutDzialka(maxLengthKw, maxLengthDz, maxLengthPew)}");
                            indexFirstParcels++;
                        }
                        else
                        {
                            GetAddedUnits();
                            indexSecondParcels++;
                        }



                        //if (indexFirstParcels >= countFirst)
                        //{
                        //    sb.AppendLine($"\t\t\t\t\t\t\t\t\t\t\t{dzialkiSecond[indexSecondParcels].GetAllDataAboutDzialka()}");
                        //    indexSecondParcels++;
                        //}

                        //if (indexFirstParcels >= countFirst)
                        //{
                        //    sb.AppendLine($"{dzialkiFirst[indexFirstParcels].GetAllDataAboutDzialka()}");
                        //    indexFirstParcels++;
                        //}

                    }

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
