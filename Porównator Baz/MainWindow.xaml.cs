using Microsoft.Win32;
using Porównator_Baz.Entities;
using Porównator_Baz.Middleware;
using Porównator_Baz.Services;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

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
            textBoxPathBase1.Text = Properties.Settings.Default.PathBase1;
            textBoxPathBase2.Text = Properties.Settings.Default.PathBase2;
            textBoxLogin.Text = Properties.Settings.Default.Login;
            textBoxPassword.Password = Properties.Settings.Default.Password;

            if (Properties.Settings.Default.Port == "3050")
            {
                radioBtn3050.IsChecked = true;
            }
            else
            {
                radioBtn3051.IsChecked = true;
            }
        }
        JednRejService jednRejService = null;

        private void ButtonPathName1_Click(object sender, RoutedEventArgs e)
        {
            var path = PathFromDialog.GetPath();
            textBoxPathBase1.Text = path;
            Properties.Settings.Default.PathBase1 = path;
            Properties.Settings.Default.Save();
        }

        private void ButtonPathName2_Click(object sender, RoutedEventArgs e)
        {
            var path = PathFromDialog.GetPath();
            textBoxPathBase2.Text = path;
            Properties.Settings.Default.PathBase2 = path;
            Properties.Settings.Default.Save();
        }

        private void TextBoxLogin_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

            Properties.Settings.Default.Login = textBoxLogin.Text;
            Properties.Settings.Default.Save();
        }

        private void TextBoxPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Password = textBoxPassword.Password;
            Properties.Settings.Default.Save();
        }

        bool infinity;
        private async void ButtonEqualBase_Click(object sender, RoutedEventArgs e)
        {
            checkBoxIgnoreKW.IsEnabled = false;
            checkBoxIgnoreParcelsArea.IsEnabled = false;
            infinity = true;
            LoadingLabel();

            MainDbContext dbContextOld = new MainDbContext(Properties.Settings.Default.PathBase1);
            MainDbContext dbContextNew = new MainDbContext(Properties.Settings.Default.PathBase2);

            try
            {
                jednRejService = await Task.Run(() => new JednRejService(dbContextOld, dbContextNew));
            }
            catch(Exception ex)
            {
                infinity = false;
                return;
            }


            var resultAddedUnit = await Task.Run(() => jednRejService?.GetAddedUnits());
            addedUnit.Text = resultAddedUnit;

            var resultDeletedUnit = await Task.Run(() => jednRejService?.GetRemovedUnits());
            deletedUnit.Text = resultDeletedUnit;

            var resultDifferencesOwner = await Task.Run(() => jednRejService?.GetDifferencesOwner());
            differenceOwner.Text = resultDifferencesOwner;

            var ignorArea = (bool)checkBoxIgnoreParcelsArea.IsChecked;
            var ignoreKW = (bool)checkBoxIgnoreKW.IsChecked;
            var resultDifferencesParcels = await Task.Run(() => jednRejService?.GetDifferencesParecels(ignorArea, ignoreKW));
            differenceParcels.Text = resultDifferencesParcels;

            infinity = false;
            checkBoxIgnoreKW.IsEnabled = true;
            checkBoxIgnoreParcelsArea.IsEnabled = true;
        }

        public async Task LoadingLabel()
        {
            loadPanel.Visibility = Visibility.Visible;
            mainTabControl.IsEnabled = false;
            while (infinity)
            {
                labelLoad.Dispatcher.Invoke(() => rotateLodingCircle.Angle += 2);
                await Task.Delay(1);
            }


            if (!infinity)
            {
                mainTabControl.IsEnabled = true;
                loadPanel.Visibility = Visibility.Hidden;
            }
        }


        async void RefreshTabParcels()
        {
            infinity = true;
            LoadingLabel();
            var ignorArea = (bool)checkBoxIgnoreParcelsArea.IsChecked;
            var ignoreKW = (bool)checkBoxIgnoreKW.IsChecked;
            var resultDifferencesParcels = await Task.Run(() => jednRejService.GetDifferencesParecels(ignorArea, ignoreKW));
            differenceParcels.Text = resultDifferencesParcels;

            infinity = false;
        }


        private void CheckBoxIgnoreKW_Checked(object sender, RoutedEventArgs e)
        {
            RefreshTabParcels();
        }

        private void CheckBoxIgnoreParcelsArea_Checked(object sender, RoutedEventArgs e)
        {
            RefreshTabParcels();
        }

        private void CheckBoxIgnoreKW_Unchecked(object sender, RoutedEventArgs e)
        {
            RefreshTabParcels();
        }

        private void CheckBoxIgnoreParcelsArea_Unchecked(object sender, RoutedEventArgs e)
        {
            RefreshTabParcels();
        }

        private void RadioButton_Checked_3050(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Port = "3050";
            Properties.Settings.Default.Save();
        }

        private void RadioButton_Checked_3051(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Port = "3051";
            Properties.Settings.Default.Save();
        }

        private void CopyDeletedUnits_Click(object sender, RoutedEventArgs e)
        {
            SaveFile.Save(jednRejService.SaveAsCsvDeletedUnits());
        }
    }
}
