using System;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace Lab_13_14
{
    class Program
    {
        static void Main(string[] args)
        {
            string pathToDbFile = "C:\\Users\\Lenovo\\source\\repos\\Lab_13-14\\OnlineStorebd.mdf";
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + pathToDbFile + ";Trusted_Connection=Yes;MultipleActiveResultSets=True";      
            SqlConnection connection = new SqlConnection(connectionString);
            using (connection)
            {
                connection.Open();
                Console.WriteLine("Подключение открыто");

                ///
                string sqlExpressionIN = "INSERT INTO [User] (Id,Name, Password, Email, Money) VALUES (2,'Tom', '1221', 'qwrqwe', 50)";
                string sqlExpressionUP = "UPDATE [User] SET Money=20 WHERE Name='Dima'";
                string sqlExpressionDL = "DELETE  FROM [User] WHERE Name='Tom'";
                SqlCommand command = new SqlCommand(sqlExpressionIN, connection);
                int number = command.ExecuteNonQuery();
                Console.WriteLine("Добавлено объектов: {0}", number);
                PrintDB(connection);
                command = new SqlCommand(sqlExpressionUP, connection);
                number = command.ExecuteNonQuery();
                Console.WriteLine("Обновление объектов: {0}", number);
                PrintDB(connection);
                command = new SqlCommand(sqlExpressionDL, connection);
                number = command.ExecuteNonQuery();
                Console.WriteLine("Удалено объектов: {0}", number);
                PrintDB(connection);
                /////Dataset позволяет работать с рассихронным набором данных
                string sql = "SELECT * FROM [User]";
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable dt = ds.Tables[0];
                DataRow newRow = dt.NewRow();
                newRow["Id"] = 3;
                newRow["Name"] = "Alice";
                newRow["Password"] = "4224";
                newRow["Email"] = "adasd";
                newRow["Money"] = 300;
                dt.Rows.Add(newRow);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
                adapter.Update(ds);
                PrintDB(connection);
            }
           
        }
        public static void PrintDB(SqlConnection connection)
        {

            string getUsersExpression = "SELECT * FROM [User]";
            SqlCommand command = new SqlCommand(getUsersExpression, connection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                Console.WriteLine("{0, 5} {1, 20} {2, 20} {3, 20}", reader.GetName(0), reader.GetName(1), reader.GetName(2), reader.GetName(3), reader.GetName(4));

                while (reader.Read())
                {
                    object id = reader.GetValue(0);
                    object Name = reader.GetValue(1);
                    object Password = reader.GetValue(2);
                    object Email =reader.GetValue(3);
                    object Money =reader.GetValue(4);
           
                    Console.WriteLine("{0, 5} {1, 20} {2, 20} {3, 20}", id, Name, Password, Email, Money);
                }
            }
            reader.Close();
        }
    }
}
