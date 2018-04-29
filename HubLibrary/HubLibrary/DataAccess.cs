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
        public static class Hub{

            public static void InitializeDB_HUB()
            {
                static filename ="Filename=hub.db";

                using (SqliteConnection db = new SqliteConnection(filename))
                {
                    db.Open();

                    string init_DeviceTable =   "CREATE TABLE IF NOT EXISTS Devices ( " +
                                                "DeviceID INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                                "DeviceSR TEXT, " +
                                                "DeviceName NVARCHAR(20) NULL)";

                    string init_UserTable = "CREATE TABLE IF NOT EXISTS Users ( " +
                                            "UserID INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                            "Username NVARCHAR(20),  " +
                                            "Password NVARCHAR(40), IsAdmin INTEGER, Cert Text)";

                    new SqliteCommand(init_DeviceTable, db).ExecuteReader();
                    new SqliteCommand(init_UserTable, db).ExecuteReader();

                    db.Close();
                }
            }

            public static void AddAdmin(string username, string Password)
            {

            }

            public static bool CheckAdmin(string username)
            {
                using (SqliteConnection db = new SqliteConnection(filename))
                {
                    db.Open();

                    string cmd = "SELECT * from Users where IsAdmin = \'1\'";


                    SqliteDataReader query = new SqliteCommand(init_DeviceTable, db).ExecuteReader();

  

                    db.Close();
                }
            }

        }


        public static class Win{

            public static void InitializeDB_WIN()
            {

            }
        }


        public static class Device{

            public static void InitializeDB_DEVICE()
            {
                using (SqliteConnection db = new SqliteConnection("Filename=device.db"))
                {
                    db.Open();

                    string init_DeviceTable = "CREATE TABLE IF NOT EXISTS " +
                                            "Devices ( DeviceID INTEGER PRIMARY KEY AUTOINCREMENT, DeviceSR TEXT, DeviceName NVARCHAR(20) NULL)";

                    string init_UserTable = "CREATE TABLE IF NOT EXISTS " +
                                            "Users ( UserID INTEGER PRIMARY KEY AUTOINCREMENT, Username NVARCHAR(20),  Password NVARCHAR(40), IsAdmin INTEGER, Cert Text)";

                    new SqliteCommand(init_DeviceTable, db).ExecuteReader();
                    new SqliteCommand(init_UserTable, db).ExecuteReader();

                    db.Close();
                }

            }
        }


    }

    
}
