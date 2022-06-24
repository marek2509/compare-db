using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Porównator_Baz.Middleware
{
    class PathFromDialog
    {
        private PathFromDialog() { }

        public static string GetPath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var dialogResult = (bool)openFileDialog.ShowDialog();
            if (dialogResult == true)
            {
                string fileName = openFileDialog.FileName;
                try
                {
                    return Path.GetFullPath(fileName);

                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
            }
            return "";
        }
    }
}
