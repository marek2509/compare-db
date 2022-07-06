using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porównator_Baz.Middleware
{
    static class SaveFile
    {
        //public static void Save(string[] contentToWrite)
        //{
        //    SaveFileDialog sfd = new SaveFileDialog();
        //    if ((bool)sfd.ShowDialog())
        //    {
        //        File.WriteAllLines(sfd.FileName, contentToWrite);
        //    }
        //}

        public static void Save(string contentToWrite)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                DefaultExt = "txt",
                Filter = "TXT (*.txt)|*.txt|All files (*.*)|*.*"
            };

            if ((bool)sfd.ShowDialog())
            {
                File.WriteAllText(sfd.FileName, contentToWrite, Encoding.Default);
            }
        }
    }
}
