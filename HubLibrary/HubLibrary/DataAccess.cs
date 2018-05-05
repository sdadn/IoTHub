using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.Sqlite;

namespace HubLibrary
{
    public static class DataAccess
    {
        public static class Win{

            const string filename = "Filename=win.db";


            public static void InitializeDB_WIN()
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

                    string init_HubTable = "CREATE TABLE IF NOT EXISTS Hub ( " +
                                            "HubID INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                            "HubSR NVARCHAR(20),  " +
                                            "Hostname NVARCHAR(40), " +
                                            "IP TEXT)";

                    new SqliteCommand(init_DeviceTable, db).ExecuteReader();
                    new SqliteCommand(init_UserTable, db).ExecuteReader();
                    new SqliteCommand(init_HubTable, db).ExecuteReader();

                    db.Close();
                }
            }
            public static bool CheckHub()
            {
                using (SqliteConnection db = new SqliteConnection(filename))
                {
                    db.Open();

                    string cmd = "SELECT * from Hub";

                    SqliteDataReader query = new SqliteCommand(cmd, db).ExecuteReader();

                    db.Close();

                    return query.HasRows;
                }
            }
            public static Hashtable GetHub()
            {
                string[] s;
                DataTable d = new DataTable();
                Hashtable h = new Hashtable();

                using (SqliteConnection db = new SqliteConnection(filename))
                {
                    db.Open();

                    string cmd = "SELECT * from Hub";

                    SqliteDataReader query = new SqliteCommand(cmd, db).ExecuteReader();

                    db.Close();

                    s = new string[query.FieldCount];
                    for(int i = 1; i < query.FieldCount; i++)
                    {
                        h.Add(query.GetName(i), (query.GetString(i)));
                    }
                }
                return h;
            }
            public static int addHub(string sr, string hostname, string ip)
            {
                if (CheckHub())
                    return 0;

                using (SqliteConnection db = new SqliteConnection(filename))
                {
                    db.Open();

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    // Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "INSERT INTO Hub VALUES (NULL, @sr, @host, @ip);";
                    insertCommand.Parameters.AddWithValue("@sr", sr);
                    insertCommand.Parameters.AddWithValue("@host", hostname);
                    insertCommand.Parameters.AddWithValue("@ip", ip);

                    insertCommand.ExecuteReader();


                    db.Close();
                }
                return 1;
            }

            public static void resetDB()
            {
                using (SqliteConnection db = new SqliteConnection(filename))
                {
                    db.Open();

                    string drop_tb1 = "DROP TABLE IF EXISTS Users";
                    string drop_tb2 = "DROP TABLE IF EXISTS Hub";
                    string drop_tb3 = "DROP TABLE IF EXISTS Devices";


                    new SqliteCommand(drop_tb1, db).ExecuteReader();
                    new SqliteCommand(drop_tb2, db).ExecuteReader();
                    new SqliteCommand(drop_tb3, db).ExecuteReader();


                    db.Close();
                }
            }
        }


        public static class Hub{

            const string filename ="Filename=hub.db";

            public static void InitializeDB_HUB()
            {

                using (SqliteConnection db = new SqliteConnection(filename))
                {
                    db.Open();

                    string init_DeviceTable =   "CREATE TABLE IF NOT EXISTS Devices ( " +
                                                "DeviceID INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                                "DeviceIP TEXT NULL, " +
                                                "DeviceName TEXT NULL)";

                    string init_UserTable = "CREATE TABLE IF NOT EXISTS Users ( " +
                                            "UserID INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                            "Username TEXT,  " +
                                            "Password TEXT, " +
                                            "IsAdmin INTEGER, " +
                                            "Cert Text)";

                    new SqliteCommand(init_DeviceTable, db).ExecuteReader();
                    new SqliteCommand(init_UserTable, db).ExecuteReader();

                    db.Close();
                }
            }

            public static int AddAdmin(string username, string Password)
            {
                if (CheckAdmin())
                    return 0;

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
                return 1;
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
            public static bool AuthAdmin(string username, string pass)
            {
                using (SqliteConnection db = new SqliteConnection(filename))
                {
                    db.Open();

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;
                    insertCommand.CommandText = "SELECT * from Users where IsAdmin = \'1\' AND Username = \'@name\' AND Password = \'@pass\'";
                    insertCommand.Parameters.AddWithValue("@name", username);
                    insertCommand.Parameters.AddWithValue("@pass", pass);

                    SqliteDataReader query = insertCommand.ExecuteReader();

                    db.Close();

                    return query.HasRows;
                }
            }

            public static void AddDevice(string ip, string hostname)
            {
                using (SqliteConnection db = new SqliteConnection(filename))
                {
                    db.Open();

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    // Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "INSERT INTO Users VALUES (NULL, @ip, @host);";
                    insertCommand.Parameters.AddWithValue("@ip", ip);
                    insertCommand.Parameters.AddWithValue("@host", hostname);
                    insertCommand.ExecuteReader();
                    
                    db.Close();
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




        public static class Device{

            static string filename = "Filename=device.db";

            public static void InitializeDB_DEVICE()
            {
                using (SqliteConnection db = new SqliteConnection(filename))
                {
                    db.Open();

                    string init_HubTable = "CREATE TABLE IF NOT EXISTS Hub ( " +
                                            "HubID INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                            "HubSR NVARCHAR(20),  " +
                                            "Hostname NVARCHAR(40), " +
                                            "IP TEXT)";

                    new SqliteCommand(init_HubTable, db).ExecuteReader();
                    db.Close();
                }

            }

            public static bool CheckHub()
            {
                using (SqliteConnection db = new SqliteConnection(filename))
                {
                    db.Open();

                    string cmd = "SELECT * from Hub";

                    SqliteDataReader query = new SqliteCommand(cmd, db).ExecuteReader();

                    db.Close();

                    return query.HasRows;
                }
            }
            public static Hashtable GetHub()
            {
                string[] s;
                DataTable d = new DataTable();
                Hashtable h = new Hashtable();

                using (SqliteConnection db = new SqliteConnection(filename))
                {
                    db.Open();

                    string cmd = "SELECT * from Hub";

                    SqliteDataReader query = new SqliteCommand(cmd, db).ExecuteReader();

                    db.Close();

                    s = new string[query.FieldCount];
                    for (int i = 0; i < query.FieldCount; i++)
                    {
                        h.Add(query.GetName(i), (query.GetString(i)));
                    }
                }
                return h;
            }
            public static int addHub(string sr, string hostname, string ip)
            {
                if (CheckHub())
                    return 0;

                using (SqliteConnection db = new SqliteConnection(filename))
                {
                    db.Open();

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    // Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "INSERT INTO Hub VALUES (NULL, @sr, @host, @ip);";
                    insertCommand.Parameters.AddWithValue("@sr", sr);
                    insertCommand.Parameters.AddWithValue("@host", hostname);
                    insertCommand.Parameters.AddWithValue("@ip", ip);

                    insertCommand.ExecuteReader();


                    db.Close();
                }
                return 1;
            }

            public static void resetDB()
            {
                using (SqliteConnection db = new SqliteConnection(filename))
                {
                    db.Open();

                    string drop_tb1 = "DROP TABLE IF EXISTS Users";
                    string drop_tb2 = "DROP TABLE IF EXISTS Devices";
                    string drop_tb3 = "DROP TABLE IF EXISTS Devices";


                    new SqliteCommand(drop_tb1, db).ExecuteReader();
                    new SqliteCommand(drop_tb2, db).ExecuteReader();
                    new SqliteCommand(drop_tb3, db).ExecuteReader();


                    db.Close();
                }
            }
        }


    }

    
}
