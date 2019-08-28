using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;

namespace test
{
    class DbConnect
    {
        static string path = System.AppDomain.CurrentDomain.BaseDirectory; //получение текущего католога приложений
        static string connectionString = "Data Source=" + path + "\\test.db"; // Строка соединения с относительным путем
        public DataTable GetTable(string tableName) 
        {
            DataTable dt = new DataTable();
            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter("SELECT * FROM "+tableName, connectionString);
            dataAdapter.Fill(dt);
            return dt;
        }
    }
}
