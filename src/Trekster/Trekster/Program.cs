using Npgsql;
using System.Configuration;
using System.Data;

namespace Trekster
{
    public class DataBase
    {
        private NpgsqlConnection Connection { get; set; }
        private NpgsqlCommand Command { get; set; }
        private NpgsqlDataReader Reader { get; set; }
        public DataBase()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["trekster_db"].ToString();
            Connection = new NpgsqlConnection(connectionString);
            Reader = null;

            Connect();
        }
        ~DataBase()
        {
            Disconnect();
        }
        private void Connect()
        {
            try
            {
                Connection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private void Disconnect()
        {
            if (Connection == null)
            {
                return;
            }

            if (Connection.State == ConnectionState.Open)
            {
                Connection.Close();
            }
        }
        private void Execute(string query)
        {
            Command = new NpgsqlCommand(query, Connection);
            Reader = Command.ExecuteReader();
        }
        public List<string> get_tables()
        {
            Execute($"SELECT table_name FROM information_schema.tables WHERE table_schema = 'public' ORDER BY table_name");

            var lst = new List<string>();

            while (Reader.Read())
            {
                lst.Add(Convert.ToString(Reader["table_name"]));
            }

            Reader.Close();

            return lst;
        }
        public List<string> get_cols(string table_name)
        {
            Execute($"SELECT column_name FROM information_schema.columns WHERE table_name = '{table_name}'");

            var lst = new List<string>();

            while (Reader.Read())
            {
                lst.Add(Convert.ToString(Reader["column_name"]));
            }

            Reader.Close();

            return lst;
        }
        private void output_table(string table_name)
        {
            var cols = get_cols(table_name);

            var query_str = $"SELECT * FROM {table_name}";

            Execute(query_str);
            
            Console.WriteLine($"\n{table_name} data:");

            Console.WriteLine(String.Join("\t", cols.ToArray()));
            Console.WriteLine("----------------------------------------------------------------------------");

            while(Reader.Read())
            {
                var row = "";

                foreach (var column in cols)
                {
                    row += Reader[column] + "\t";
                }

                Console.WriteLine(row);
            }

            Reader.Close();
        }
        public void output()
        {
            var tables = get_tables();

            foreach (var table in tables)
            {
                output_table(table);
            }
        }
        public List<int> get_ids(string table)
        {
            var query = $"SELECT * FROM {table}";
            var ids = new List<int>();

            Execute(query);

            while (Reader.Read())
            {
                ids.Add(Convert.ToInt16(Reader["id"]));
            }

            Reader.Close();

            return ids;
        }
        public void clean_table(string table_name)
        {
            var query_str = $"DELETE FROM {table_name}";

            Execute(query_str);

            Console.WriteLine($"Table {table_name} cleaned");

            Reader.Close();
        }
        public void clean()
        {
            var tables = get_tables();

            tables.Reverse();

            foreach (var table in tables)
            {
                clean_table(table);
            }
        }
        public void populate_table(string table_name)
        {
            var cols = get_cols(table_name);

            var query_str = $"INSERT INTO {table_name}(";

            foreach (var col in cols)
            {
                if (col != "id")
                {
                    query_str += col.ToString() + ",";
                }
            }

            query_str = query_str.Remove(query_str.Length - 1);

            query_str += ") VALUES";

            var lst_accounts = new List<string> {"'Сейф'", "'Гаманець'",
                "'Монобанк'", "'Криптогаманець'", "'Приватбанк'", "'Ощадкбанк'"};

            var lst_currencies = new List<string>() {"'Гривня'", "'Долар'", "'Євро'", "'Біткоїн'" };

            var lst_categories = new List<string>() { "1, 'Робота 1'", "1, 'Робота 2'", "1, 'Робота 3'", "1, 'Депозит приват'", "1, 'Депозит моно'",
                "1, 'Інвестиція'", "1 , 'Повернення боргу'", "-1, 'Продукти'", "-1, 'Комунальні послуги'", "-1, 'Підписки'", "-1, 'Автомобіль'",
                "-1, 'Women 1'", "-1, 'Woman 2'", "-1, 'Позика'" };

            var currencies_ids = get_ids("currencies");
            var accounts_ids = get_ids("accounts");
            var categories_ids = get_ids("categories");

            var rndm = new Random();

            var final_list = new List<string>();
            switch (table_name) 
            {
                case "accounts":
                    final_list = lst_accounts;
                    break;
                case "currencies":
                    final_list = lst_currencies;
                    break;
                case "startbalances":
                    var lst_startbalances = new List<string>();

                    foreach (var account in accounts_ids)
                    {
                        var i = rndm.NextInt64(3);
                        for (int j = currencies_ids[0]; j < (i + currencies_ids[0]); j++)
                        {
                            lst_startbalances.Add(account.ToString() + "," + j.ToString() + "," + rndm.NextDouble() * 1000);
                        }
                    }
                    final_list = lst_startbalances;
                    break;  
                case "categories":
                    final_list = lst_categories;
                    break;
               case "transactions":
                    var lst_transactions = new List<string>();
                    for (int i = 0; i < 30; i++)
                    {
                        var start = new DateTime(1995, 1, 1);
                        int range = (DateTime.Today - start).Days;
                        var dt = start.AddDays(rndm.Next(range)).ToString("yyyy-MM-dd");

                        var acc = accounts_ids[rndm.Next(0, accounts_ids.Count)];
                        var curr = currencies_ids[rndm.Next(0, currencies_ids.Count)];
                        var cat = categories_ids[rndm.Next(0, categories_ids.Count)];

                        var sum = rndm.NextDouble() * 100;

                        lst_transactions.Add("'" + dt + "'," + acc + "," + curr + "," + cat + "," + sum + ",'Note" + i + "'");
                    }
                    final_list = lst_transactions;
                    break;   
            }

            foreach (var row in final_list)
            {
                query_str += "(" + row + "),";
            }

            query_str = query_str.Remove(query_str.Length - 1);

            Execute(query_str);

            Console.WriteLine($"Table {table_name} populated");

            Reader.Close();
        }
        public void populate()
        {
            var tables = get_tables();

            foreach (var table in tables)
            {
                populate_table(table);
            }
        }
    }
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Enter 'Output' to print tables");
            Console.WriteLine("Enter 'Clean' to clean tables");
            Console.WriteLine("Enter 'Populate' to populate tables");
            Console.WriteLine("Enter 'Exit' to exit");

            DataBase db = new DataBase();

            while (true)
            {
                Console.WriteLine("\nEnter command:");
                var input = Console.ReadLine();

                if (input == "Output")
                {
                    db.output();
                    continue;
                }

                if (input == "Clean")
                {
                    db.clean();
                    continue;
                }

                if (input == "Populate")
                {
                    db.populate();
                    continue;
                }

                if (input == "Exit")
                {
                    break;
                }
            }

            Console.WriteLine("\nAll tasks finished");
        }
    }
}