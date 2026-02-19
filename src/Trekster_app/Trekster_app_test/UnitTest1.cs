using Trekster_app;
using Trekster_app.BLL;

namespace Trekster_app_test
{
    public class UnitTest1
    {
        Controller controller = new Controller();

        [Fact]
        public void Add_account_test()
        {
            var test_name = "test_account_name";

            var test_balances = new Dictionary<string, string>();
            test_balances.Add("uah", "100");
            test_balances.Add("eur", "200");
            test_balances.Add("usdt", "75");

            controller.Add_account(test_name, test_balances);

            var context = new TreksterDbContext();

            var acc = context.Accounts.Where(x => x.Name == test_name).First();

            var currencies = context.Currencies.ToList();

            Assert.NotNull(acc);

            var startbalances = context.Startbalances.Where(x => x.Idaccount == acc.Id).ToList();

            Assert.NotNull(startbalances);

            var dct = new Dictionary<string, string>();

            foreach (var elem in startbalances)
            {
                var currency = currencies.Where(x => x.Id == elem.Idcurrency).First().Name;
                dct.Add(currency, elem.Sum.ToString());
            }

            Assert.Equal(startbalances.Count, test_balances.Count);

            foreach (var elem in test_balances)
            {
                Assert.Equal(dct[elem.Key], elem.Value);
            }

            controller.Delete_account(acc.Id);
        }

        [Fact]
        public void Account_name_exists_test()
        {
            Assert.False(controller.Account_name_exists("name_that_doesn't_exist"));
            Assert.False(controller.Account_name_exists("fake name1"));
            Assert.False(controller.Account_name_exists("impossible-name_for_account_53"));

            var dct = new Dictionary<string, string>();

            controller.Add_account("test_name_1", dct);
            controller.Add_account("test_name_2", dct);
            controller.Add_account("test_name_3", dct);

            Assert.True(controller.Account_name_exists("test_name_1"));
            Assert.True(controller.Account_name_exists("test_name_2"));
            Assert.True(controller.Account_name_exists("test_name_3"));

            var context = new TreksterDbContext();

            var accounts = context.Accounts.ToList();

            Assert.NotNull(accounts);

            controller.Delete_account(accounts.Where(x => x.Name == "test_name_1").First().Id);
            controller.Delete_account(accounts.Where(x => x.Name == "test_name_2").First().Id);
            controller.Delete_account(accounts.Where(x => x.Name == "test_name_3").First().Id);
        }

        [Fact]
        public void Category_name_exists_test()
        {
            Assert.False(controller.Category_name_exists("name_that_doesn't_exist"));
            Assert.False(controller.Category_name_exists("fake name1"));
            Assert.False(controller.Category_name_exists("impossible-name_for_category_53"));

            controller.Add_category("test_name_1", 1);
            controller.Add_category("test_name_2", 1);
            controller.Add_category("test_name_3", 1);

            Assert.True(controller.Category_name_exists("test_name_1"));
            Assert.True(controller.Category_name_exists("test_name_2"));
            Assert.True(controller.Category_name_exists("test_name_3"));

            var context = new TreksterDbContext();

            var categories = context.Categories.ToList();

            Assert.NotNull(categories);

            controller.Delete_category(categories.Where(x => x.Name == "test_name_1").First().Id);
            controller.Delete_category(categories.Where(x => x.Name == "test_name_2").First().Id);
            controller.Delete_category(categories.Where(x => x.Name == "test_name_3").First().Id);
        }

        [Fact]
        public void Delete_account_test()
        {
            var context = new TreksterDbContext();

            var new_acc = new Account();
            new_acc.Name = "test_name_acc";

            context.Accounts.Add(new_acc);

            context.SaveChanges();

            var context1 = new TreksterDbContext();

            var accs = context1.Accounts;
            var acc = accs.Where(x => x.Name == "test_name_acc").First();

            Assert.NotNull(acc);

            context1.Accounts.Remove(acc);

            context1.SaveChanges();

            Assert.DoesNotContain(acc, accs);
        }

        [Fact]
        public void Delete_category_test()
        {
            var context = new TreksterDbContext();

            var new_cat = new Category();
            new_cat.Name = "test_name_cat";
            new_cat.Type = 1;

            context.Categories.Add(new_cat);

            context.SaveChanges();

            var context1 = new TreksterDbContext();

            var cats = context1.Categories;
            var cat = cats.Where(x => x.Name == "test_name_cat").First();

            Assert.NotNull(cat);

            context1.Categories.Remove(cat);

            context1.SaveChanges();

            Assert.DoesNotContain(cat, cats);
        }

        [Fact]
        public void Edit_category_test()
        {
            var context = new TreksterDbContext();

            var new_cat = new Category();
            new_cat.Name = "test_name_cat";
            new_cat.Type = 1;

            context.Categories.Add(new_cat);

            context.SaveChanges();

            var context1 = new TreksterDbContext();

            var cats = context1.Categories;
            var cat = cats.Where(x => x.Name == "test_name_cat").First();

            Assert.NotNull(cat);

            cat.Name = "new_name_cat";

            cat.Type = -1;

            context1.SaveChanges();

            var context2 = new TreksterDbContext();

            var cats_2 = context2.Categories;
            var cat_edit = cats_2.Where(x => x.Name == "new_name_cat").First();

            Assert.NotNull(cat);

            Assert.Equal("new_name_cat", cat_edit.Name);
            Assert.Equal(-1, cat_edit.Type);

            context2.Categories.Remove(cat_edit);

            context2.SaveChanges();
        }

        [Fact]
        public void Add_category_test()
        {
            var context = new TreksterDbContext();

            var categories = context.Categories;

            var test_name = "test_category_name";

            var cat = new Category();
            cat.Name = test_name;
            cat.Type = 1;

            Assert.DoesNotContain(cat, categories);

            context.Categories.Add(cat);

            context.SaveChanges();

            var context1 = new TreksterDbContext();

            var categories1 = context1.Categories.ToList();

            var cat_add = categories1.Where(x => x.Name == test_name).First();

            Assert.NotNull(cat_add);

            Assert.Equal(1, cat_add.Type);
            Assert.Equal(test_name, cat_add.Name);

            context1.Categories.Remove(cat_add);

            context1.SaveChanges();
        }
    }
}