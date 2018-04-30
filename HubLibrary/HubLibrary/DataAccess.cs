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

            static string filename ="Filename=hub.db";

            public static void InitializeDB_HUB()
            {

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
                                            "Password NVARCHAR(40), " +
                                            "IsAdmin INTEGER, " +
                                            "Cert Text)";

                    new SqliteCommand(init_DeviceTable, db).ExecuteReader();
                    new SqliteCommand(init_UserTable, db).ExecuteReader();

                    db.Close();
                }
            }

            public static void AddAdmin(string username, string Password)
            {
                if (CheckAdmin())
                    return;

                using (SqliteConnection db = new SqliteConnection(filename))
                {
                    db.Open();

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    // Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "INSERT INTO Users VALUES (NULL, @name, @pwd, @admin, \'asdsad\');";
                    insertCommand.Parameters.AddWithValue("@name", username);
                    insertCommand.Parameters.AddWithValue("@pwd", Password);
                    insertCommand.Parameters.AddWithValue("@admin", 1);

                    insertCommand.ExecuteReader();


                    db.Close();
                }
            }

            public static bool CheckAdmin()
            {
                using (SqliteConnection db = new SqliteConnection(filename))
                {
                    db.Open();

                    string cmd = "SELECT * from Users where IsAdmin = \'1\'";


                    SqliteDataReader query = new SqliteCommand(cmd, db).ExecuteReader();

                    db.Close();

                    return query.HasRows;
                }
            }

            public static void resetDB()
            {
                using (SqliteConnection db = new SqliteConnection(filename))
                {
                    db.Open();

                    string drop_tb1 = "DROP TABLE IF EXISTS Users";
                    string drop_tb2 = "DROP TABLE IF EXISTS Devices";


                    new SqliteCommand( drop_tb1, db).ExecuteReader();
                    new SqliteCommand( drop_tb2, db).ExecuteReader();

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
