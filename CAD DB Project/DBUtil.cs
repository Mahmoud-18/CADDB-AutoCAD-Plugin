using System.Data.SqlClient;
using System.Windows;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using CAD_DB_Project;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

namespace CAD_Database
{
    public class DBUtil : ICadCommand
    {
        public override void Execute()
        {
           DBRun();
        }
      
        public static void DBRun()
        {           
            MainWindow main = new MainWindow();
            Application.ShowModalWindow(main);
        }

        public static SqlConnection GetConnection()
        {
            string Connstr = Settings1.Default.Connstr;
            SqlConnection conn = new SqlConnection(Connstr);
            return conn;
        }
    }
}
