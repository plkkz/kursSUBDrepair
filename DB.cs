using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdkurs
{
    internal class DB
    {
        SqlConnection connection = new SqlConnection(@"Data Source=WIN-UL8JLULEF5G\SQLEXPRESS;Initial Catalog=bdkurs;Integrated Security=True"); //подключение к базе данных
        
        public void openConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();//открытие соединения с бд
        }
        public void closeConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();//закрытие соединения с бд
        }
        public SqlConnection getConnection()
        {
            return connection;
        }

    }
}
