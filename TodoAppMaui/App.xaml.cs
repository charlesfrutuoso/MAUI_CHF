using TodoAppMaui.Services;
using System.IO;

namespace TodoAppMaui
{
    public partial class App : Application
    {
        static DatabaseService? database;

        public static DatabaseService Database
        {
            get
            {
                if (database == null)
                {
                    string dbPath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "tasks.db3");

                    database = new DatabaseService(dbPath);
                }
                return database;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
