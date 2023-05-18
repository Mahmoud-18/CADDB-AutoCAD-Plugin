using CAD_Database;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace CAD_DB_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadLinesToDB(object sender, RoutedEventArgs e)
        {
            DBLoadUtil dBLoad = new DBLoadUtil();
            dBLoad.LoadLines();

            LoadLinesBTN.Background = Brushes.ForestGreen;
            LoadLinesBTN.Content = "Lines Loaded";
            LoadLinesBTN.IsEnabled = false;
            
        }

        private void LoadPolyLinesToDB(object sender, RoutedEventArgs e)
        {
            DBLoadUtil dBLoad = new DBLoadUtil();
            dBLoad.LoadPolyLines();

            LoadPolyLinesBTN.Background = Brushes.ForestGreen;
            LoadPolyLinesBTN.Content= "Polylines Loaded";
            LoadPolyLinesBTN.IsEnabled = false;
            
        }   

        private void LoadBlocksToDB(object sender, RoutedEventArgs e)
        {
            DBLoadUtil dBLoad = new DBLoadUtil();
            dBLoad.LoadBlocksNoAttributes();

            LoadBlocksBTN.Background = Brushes.ForestGreen;
            LoadBlocksBTN.Content= "Blocks Loaded";
            LoadBlocksBTN.IsEnabled = false;
            
        }

        private void OK(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
