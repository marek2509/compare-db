using Porównator_Baz.Entities;
using Porównator_Baz.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Porównator_Baz
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //using (MainDbContext dbContext = new MainDbContext(@"D:\Bazy\Scalenia\Milejczyce porównanie przed po\MILEJCZYCE 2021.FDB"))
            //{
            //    var jedn = dbContext.Jedn_Rejs.FirstOrDefault(x => x.IJR == 1);
            //    Console.WriteLine(jedn.IJR + " <> " + jedn.Dzialki.First().IDD);
            //    var podmiot = jedn.Udzialy.FirstOrDefault().Podmiot;
            //    if (podmiot.TYP == "P")
            //    {
            //        var intytucja = podmiot.Instytucja;
            //        Console.WriteLine("INSTYTUCJA>" + intytucja.NPE);
            //    }

            //    var fiz = dbContext.Jedn_Rejs.FirstOrDefault(x => x.IJR == 15);
            //    var podm2 = fiz.Udzialy.FirstOrDefault().Podmiot;
            //    if (podm2.TYP == "F")
            //    {
            //        var osoba = podm2.OsobaFizyczna;
            //        Console.WriteLine("OSOBA>" + osoba.NZW + " " + osoba.PIM);
            //    }

            //    var malz = dbContext.Jedn_Rejs.FirstOrDefault(x => x.IJR == 58);
            //    var podm3 = malz.Udzialy.FirstOrDefault().Podmiot;
            //    if (podm3.TYP == "M")
            //    {
            //        var osoba = podm3.Malzenstwo;
            //        Console.WriteLine("MALZENSTWO>" + osoba.Maz.NZW + " " + osoba.Maz.PIM + " " + osoba.Zona.PIM);
            //    }
            //}

            //MainDbContext dbContextWrobel = new MainDbContext(@"D:\Bazy\Scalenia\Debitka wyk gr otrzym\DEBITKA_09_06_2021.FDB");

            //var jedn2 = dbContextWrobel.Jedn_Rejs.FirstOrDefault(x => x.IJR == 1000);

            //Console.WriteLine("Inny>" + jedn2.Udzialy.FirstOrDefault(x => x.Podmiot.TYP == "I").Podmiot.InnyPodmiot.NPE);


            MainDbContext dbContextOld = new MainDbContext(@"D:\Bazy\Scalenia\Milejczyce porównanie przed po\MILEJCZYCE 2021.FDB");
            MainDbContext dbContextNew = new MainDbContext(@"D:\Bazy\Scalenia\Milejczyce porównanie przed po\MILEJCZYCE 2022.FDB");

            JednRejService jednRejService = new JednRejService(dbContextOld, dbContextNew);

            addedUnit.Text = jednRejService.GetAddedJednostki();
            deletedUnit.Text = jednRejService.GetRemovedJednostki();
            differenceDzialki.Text = jednRejService.GetDifferencesDzialki();
        }
    }
}
