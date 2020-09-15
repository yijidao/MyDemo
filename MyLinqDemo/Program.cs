using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace MyLinqDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("执行...");

            QueryVariable();
            FromClause();
            IntoAndLetClause();
            Subqueries();
            SimpleKeyJoin();
            CompositeKeyJoin();
            MultipleJoin();
            GroupJoin();
            LeftJoin();

            Console.ReadLine();
        }

        static void QueryVariable()
        {
            Console.WriteLine($"{nameof(QueryVariable)}...");

            var scores = new[] { 55, 78, 89, 82, 66, 94 };
            var qv = from score in scores // qv就是查询变量
                     where score > 80
                     orderby score descending
                     select score;

            foreach (var score in qv)
            {
                Console.WriteLine(score);
            }
        }

        static void FromClause()
        {
            Console.WriteLine($"{nameof(FromClause)}...");

            var countries = Country.GetCountries();

            var names = from country in countries // Country 中包含 IList<City>，所以这里用两个form
                        from city in country.Cites
                        select city.Name;

            foreach (var name in names)
            {
                Console.WriteLine(name);
            }
        }

        static void IntoAndLetClause()
        {
            Console.WriteLine($"{nameof(IntoAndLetClause)}...");

            var random = new Random();
            var list = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(random.Next(1000));
            }

            var groups = from number in list
                         let p = (number % 2 == 0) // 使用 let 存储判断表达式的结果
                         group number by p
                into numberGroup // 使用 into 存储上一个linq 的结果，执行新的 linq 查询
                         select new { numberGroup.Key, count = numberGroup.Count() };

            foreach (var g in groups)
            {
                Console.WriteLine($"{g.Key},{g.count}");
            }
        }

        static void Subqueries()
        {
            Console.WriteLine($"{nameof(Subqueries)}...");

            var countries = Country.GetCountries();

            var o = from country in countries
                    group country by country.Name
                into countryGroup
                    select new
                    {
                        countryGroup.Key,
                        cityNames = from g in countryGroup
                                    where g.Name == countryGroup.Key
                                    select g.Cites into cities1
                                    from city in cities1
                                    select city.Name
                    };

            foreach (var item in o)
            {
                Console.Write($"{item.Key}");
                foreach (var cityName in item.cityNames)
                {
                    Console.Write($"{cityName}");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// 单个键join
        /// </summary>
        static void SimpleKeyJoin()
        {
            Console.WriteLine($"{nameof(SimpleKeyJoin)}...");

            var (persons, cats, dogs) = GetPersonAndCatAndDog();

            var query = from p in persons
                        join c in cats on p equals c.Owner
                        select new { OwnerName = p.FullName, PetName = c.Name };
            foreach (var item in query)
            {
                Console.WriteLine($"主人：{item.OwnerName}，猫咪：{item.PetName}");
            }

            var query2 = from cat in cats
                         from dog in dogs
                         where (cat.Owner != dog.Owner && cat.Gender == dog.Gender)
                         select new { Cat = cat.Name, Dog = dog.Name };
            foreach (var item in query2)
            {
                Console.WriteLine(item);
            }

        }

        /// <summary>
        /// 组合键join
        /// </summary>
        static void CompositeKeyJoin()
        {
            Console.WriteLine($"{nameof(CompositeKeyJoin)}...");

            var (persons, cats, dogs) = GetPersonAndCatAndDog();

            var query = from cat in cats
                        join dog in dogs on new { cat.Owner, cat.Gender } equals new { dog.Owner, dog.Gender }
                        select new { CatName = cat.Name, DogName = dog.Name, CatOwner = cat.Owner, DogOwner = dog.Owner, CatGender = cat.Gender, DogGender = dog.Gender };

            foreach (var item in query)
            {
                Console.WriteLine($"猫名：{item.CatName}，猫性别：{item.CatGender}，猫主人：{item.CatOwner.FullName} 狗名：{item.DogName}，狗性别：{item.DogGender}，狗主人：{item.DogOwner.FullName}");
            }
        }

        /// <summary>
        /// 分组join
        /// </summary>
        static void GroupJoin()
        {
            Console.WriteLine($"{nameof(GroupJoin)}...");
            var (persons, pets) = GetPersonAndPet();

            var query = from person in persons
                        join pet in pets on person equals pet.Owner into gj
                        from subpet in gj
                        select new { PetName = subpet.Name, Master = subpet.Owner.FullName };
            foreach (var item in query)
            {
                Console.WriteLine(item);
            }

            // 上面的查询等于这个查询
            var query2 = from pet1 in pets
                         join person1 in persons on pet1.Owner equals person1
                         select new { PetName = pet1.Name, Master = person1.FullName };
            foreach (var item in query2)
            {
                Console.WriteLine(item);
            }
        }

        /// <summary>
        /// 左关联
        /// </summary>
        static void LeftJoin()
        {
            Console.WriteLine($"{nameof(LeftJoin)}");

            var (persons, pets) = GetPersonAndPet();

            var query = from person in persons
                        join pet in pets on person equals pet.Owner into gj
                        from subPet in gj.DefaultIfEmpty()
                        orderby subPet?.Name ?? string.Empty
                        select new { Master = person.FullName, pet = subPet?.Name ?? string.Empty };

            foreach (var item in query)
            {
                Console.WriteLine(item);
            }

        }

        /// <summary>
        /// 多个join
        /// </summary>
        static void MultipleJoin()
        {
            var (persons, cats, dogs) = GetPersonAndCatAndDog();

            var query = from cat in cats
                        join dog in dogs on cat.Owner equals dog.Owner
                        join person in persons on dog.Owner equals person
                        select new { Cat = cat.Name, Dog = dog.Name, Master = person.FullName };

            foreach (var item in query)
            {
                Console.WriteLine(item);
            }
        }

        public static (IList<Person>, IList<Cat>, IList<Dog>) GetPersonAndCatAndDog()
        {
            var p1 = new Person("张", "三");
            var p2 = new Person("李", "四");
            var p3 = new Person("王", "五");

            var cat1 = new Cat("杜甫", p1, "母");
            var cat2 = new Cat("李白", p1, "公");
            var cat3 = new Cat("分哥", p2, "公");
            var cat4 = new Cat("猪皮", p2, "母");

            var dog1 = new Dog("金毛", p2, "母");
            var dog2 = new Dog("二哈", p2, "公");

            var list1 = new List<Person> { p1, p2, p3 };
            var list2 = new List<Cat> { cat1, cat2, cat3, cat4 };
            var list3 = new List<Dog> { dog1, dog2 };
            return (list1, list2, list3);
        }

        public static (IList<Person>, IList<Pet>) GetPersonAndPet()
        {
            var (persons, cats, dogs) = GetPersonAndCatAndDog();
            var pets = new List<Pet>();
            pets.AddRange(cats);
            pets.AddRange(dogs);
            return (persons, pets);
        }
    }

    public class Country
    {
        public string Name { get; set; }

        public IList<City> Cites { get; set; } = new List<City>();

        public static IList<Country> GetCountries() =>
            new List<Country>
            {
                new Country
                {
                    Name = "中国",
                    Cites = new List<City>
                    {
                        new City {Name = "广州"},
                        new City {Name = "中山"},
                        new City {Name = "汕头"},
                        new City {Name = "深圳"},
                    }
                },
                new Country
                {
                    Name = "日本",
                    Cites = new List<City>
                    {
                        new City {Name = "东京"},
                        new City {Name = "大阪"},
                        new City {Name = "名古屋"},
                    }
                },
            };
    }

    public class City
    {
        public string Name { get; set; }
    }

    class Person
    {
        public Person(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }

    class Pet
    {
        public Pet(string name, Person owner, string gender)
        {
            Name = name;
            Owner = owner;
            Gender = gender;
        }

        public string Name { get; set; }

        public Person Owner { get; set; }
        public string Gender { get; }
    }

    class Cat : Pet
    {
        public Cat(string name, Person owner, string gender) : base(name, owner, gender)
        {
        }
    }

    class Dog : Pet
    {
        public Dog(string name, Person owner, string gender) : base(name, owner, gender)
        {
        }
    }
}
