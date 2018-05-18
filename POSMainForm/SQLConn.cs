using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using System.Xml.Linq;
using MySql.Data.MySqlClient;

namespace POSMainForm
{
	static class SQLConn
	{
		public static string ServerMySQL;
		public static string PortMySQL;
		public static string UserNameMySQL;
		public static string PwdMySQL;
		public static string DBNameMySQL;
		public static string sqL;
		public static DataSet ds = new DataSet();
		public static MySqlCommand cmd;
		public static MySqlDataReader dr;

        public static bool adding;
        public static bool updating;

        public static string strSearch = "";

		public static MySqlDataAdapter da = new MySqlDataAdapter();

		public static MySqlConnection conn = new MySqlConnection();

        
       

        public static void getData()
		{
			string AppName = Application.ProductName;

			try {

				//DBNameMySQL = Interaction.GetSetting(AppName, "DBSection", "DB_Name", "posisdb_csharp");
				//ServerMySQL = Interaction.GetSetting(AppName, "DBSection", "DB_IP", "localhost");
				//PortMySQL = Interaction.GetSetting(AppName, "DBSection", "DB_Port", "8080");
				//UserNameMySQL = Interaction.GetSetting(AppName, "DBSection", "DB_User", "root");
				//PwdMySQL = Interaction.GetSetting(AppName, "DBSection", "DB_Password", "");

                DBNameMySQL = Interaction.GetSetting(AppName, "DBSection", "DB_Name", System.Configuration.ConfigurationSettings.AppSettings["DBName"]);
                ServerMySQL = Interaction.GetSetting(AppName, "DBSection", "DB_IP", System.Configuration.ConfigurationSettings.AppSettings["DBHost"]);
                PortMySQL = Interaction.GetSetting(AppName, "DBSection", "DB_Port", System.Configuration.ConfigurationSettings.AppSettings["DBPort"]);
                UserNameMySQL = Interaction.GetSetting(AppName, "DBSection", "DB_User", System.Configuration.ConfigurationSettings.AppSettings["DBUser"]);
                PwdMySQL = Interaction.GetSetting(AppName, "DBSection", "DB_Password", System.Configuration.ConfigurationSettings.AppSettings["DBPwd"]);
                

            } catch {
				Interaction.MsgBox("System registry was not established, you can set/save " + "these settings by pressing F1", MsgBoxStyle.Information);
			}

		}

		public static void ConnDB()
		{
			conn.Close();
			try {
				conn.ConnectionString = "Server = '" + ServerMySQL + "';" + "Database = '" + DBNameMySQL + "'; " + "user id = '" + UserNameMySQL + "'; " + "password = '" + PwdMySQL + "'";

				conn.Open();
			} catch  {
				Interaction.MsgBox("The system failed to establish a connection", MsgBoxStyle.Information, "Database Settings");
			}

		}


		public static void DisconnMy()
		{
			conn.Close();
			conn.Dispose();

		}

		public static void SaveData()
		{
			string AppName = Application.ProductName;

			Interaction.SaveSetting(AppName, "DBSection", "DB_Name", DBNameMySQL);
			Interaction.SaveSetting(AppName, "DBSection", "DB_IP", ServerMySQL);
			Interaction.SaveSetting(AppName, "DBSection", "DB_Port", PortMySQL);
			Interaction.SaveSetting(AppName, "DBSection", "DB_User", UserNameMySQL);
			Interaction.SaveSetting(AppName, "DBSection", "DB_Password", PwdMySQL);

			Interaction.MsgBox("Database connection settings are saved.", MsgBoxStyle.Information);
		}



	}

    public class DB
    {
        MySqlDataAdapter da = new MySqlDataAdapter();
        DataTable dt = new DataTable();

        public DataTable GetData(string query)
        {
            try
            {

                da = new MySqlDataAdapter();
                SQLConn.ConnDB();
                da.SelectCommand = new MySqlCommand(query,SQLConn.conn);
                da.SelectCommand.CommandTimeout = 600;
                dt = new DataTable();
                da.Fill(dt);


            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                //  MessageBox.Show("Please Check your internet conntection and Try again!.");
            }
            return dt;
        }
    }
}

