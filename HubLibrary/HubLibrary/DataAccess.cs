using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.Sqlite;

namespace HubLibrary
{
    public static class DataAccess
    {
        public static void InitializeDB_HUB()
        {
            using (SqliteConnection db = new SqliteConnection("Filename=hub.db"))
            {
                db.Open();

                string init_DeviceTable = "CREATE TABLE IF NOT EXISTS " +
                                          "Devices ( Primary_Key INTEGER PRIMARY KEY AUTOINCREMENT, DeviceID INTEGER,  DeviceName NVARCHAR(20) NULL)";


                //db.Close();
            }
        }
        public static void InitializeDB_WIN()
        {

        }
        public static void InitializeDB_DEVICE()
        {

        }

        public static void AddAdmin()
        {

        }

    }

    
}
