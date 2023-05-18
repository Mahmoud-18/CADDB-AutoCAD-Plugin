using Autodesk.AutoCAD.Runtime;
using Autodesk.Windows;
using CAD_Database;
using CAD_DB_Project.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace CAD_DB_Project
{
    public class Ribbon
    {
        public const string RibbonTitle = "ITI";
        public const string RibbonId = "10 10";
   
        [CommandMethod("Database")]
        public void CreateRibbon()
        {
            RibbonControl ribbon = ComponentManager.Ribbon;
            if (ribbon != null)
            {
                RibbonTab rtab = ribbon.FindTab(RibbonId);
                if (rtab != null)
                {
                    ribbon.Tabs.Remove(rtab);
                }

                rtab = new RibbonTab();
                rtab.Title = RibbonTitle;
                rtab.Id = RibbonId;
                ribbon.Tabs.Add(rtab);
                AddContentToTab(rtab);
            }
        }

        private void AddContentToTab(RibbonTab rtab)
        {
            rtab.Panels.Add(AddPanelOne());
        }

        private static RibbonPanel AddPanelOne()
        {
            RibbonPanelSource rps = new RibbonPanelSource();
            rps.Title = "Export to Database";
            RibbonPanel rp = new RibbonPanel();
            rp.Source = rps;
            RibbonButton rci = new RibbonButton();
            rci.Name = "ITI Addin";
            rps.DialogLauncher = rci;
            //create button1
            var addinAssembly = typeof(System.Windows.Controls.Ribbon.Ribbon).Assembly;
            RibbonButton btnPythonShell = new RibbonButton
            {
                Orientation = Orientation.Vertical,
                AllowInStatusBar = true,
                Size = RibbonItemSize.Large,
                Name = "ITI",
                ShowText = true,
                Text = "Export to DB",
                Description = "ITI Plugin",
                ShowImage = true,
                LargeImage = Resource1.Export.ToBitmapImage(),               
                CommandHandler = new RelayCommand(new DBUtil().Execute)
            };
            rps.Items.Add(btnPythonShell);
            return rp;

        }
        public static System.Windows.Media.ImageSource GetEmbeddedPng(System.Reflection.Assembly app, string imageName)
        {
            var file = app.GetManifestResourceStream(imageName);
            BitmapDecoder source = PngBitmapDecoder.Create(file, BitmapCreateOptions.None, BitmapCacheOption.None);
            return source.Frames[0];
        }

    }
    public static class BitmapExtensions
    {
       public static BitmapImage ToBitmapImage(this Bitmap image)
       {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();
            return bi;
       }
    }
}
