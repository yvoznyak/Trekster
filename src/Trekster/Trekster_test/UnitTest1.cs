using Trekster;
using Npgsql;
using System.Configuration;
using System.Data;
using System.Collections.Generic;

namespace Trekster_test
{
    public class UnitTest1
    {
        private readonly DataBase db;
        public UnitTest1()
        {
        }
        
        [Fact]
        public void get_tables_test()
        {
            var db = new DataBase();

            var lst = new List<string>() { "accounts", "categories", "currencies", "startbalances", "transactions" };

            var tables = db.get_tables();

            Assert.Equal(lst.Count, tables.Count);

            foreach (var table in tables)
            {
                Assert.Contains(table, lst);
            }
        }

        [Fact]
        public void get_cols_test()
        {
            var db = new DataBase();

            var dct = new Dictionary<string, List<string>>();

            dct.Add("accounts", new List<string>() { "id", "name" });
            dct.Add("categories", new List<string>() { "id", "name", "type" });
            dct.Add("currencies", new List<string>() { "id", "name" });
            dct.Add("startbalances", new List<string>() { "id", "idaccount", "idcurrency", "sum" });
            dct.Add("transactions", new List<string>() { "id", "date", "idaccount", "idcurrency", "idcategory", "sum", "note" });

            foreach (var elem in dct)
            {
                var cols = db.get_cols(elem.Key);

                Assert.Equal(elem.Value.Count, cols.Count);

                foreach (var col in elem.Value)
                {
                    Assert.Contains(col, cols);
                }
            }
        }

        [Fact]
        public void get_ids_test()
        {
            var db = new DataBase();

            var dct = new Dictionary<string, List<int>>();

            dct.Add("accounts", new List<int>() { 100, 101, 102, 103 });
            dct.Add("categories", new List<int>() { 113, 114, 115, 116, 117 });
            dct.Add("currencies", new List<int>() { 34, 35, 36, 37, 38 });
            dct.Add("startbalances", new List<int>() { 39, 40, 41, 42, 43, 44, 45, 46 });
            
            foreach (var elem in dct)
            {
                var ids = db.get_ids(elem.Key);

                foreach (var id in elem.Value)
                {
                    Assert.Contains(id, ids);
                }
            }
        }

        [Fact]
        public void clean_test()
        {
            var db = new DataBase();

            db.clean_table("transactions");

            Assert.Empty(db.get_ids("transactions"));
        }
    }
}