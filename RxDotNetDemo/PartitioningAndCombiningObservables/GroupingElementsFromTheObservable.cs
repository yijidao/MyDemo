using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using RxDotNetDemo.Extensions;

namespace RxDotNetDemo.PartitioningAndCombiningObservables
{
    class GroupingElementsFromTheObservable
    {
        public static void GroupOperate()
        {
            var peoples = People.GetPeoples();
            var o = peoples.ToObservable();
            var genderAge = from gender in o.GroupBy(x => x.Gender)
                            from avg in gender.Average(x => x.Age)
                            select new { Gender = gender.Key, AvgAge = avg };

            genderAge.SubscribeConsole("GenderAge");
        }


        public static void GroupClause()
        {
            var peoples = People.GetPeoples();
            var o = peoples.ToObservable();
            var genderAge = from people in o
                group people by people.Gender
                into gender
                from avg in gender.Average(p => p.Age)
                select new {Gender = gender.Key, AvgAge = avg};
            genderAge.SubscribeConsole("GenderAge");

        }
    }

    public class People
    {
        public string Name { get; }

        /// <summary>
        /// 1 男 2 女
        /// </summary>
        public int Gender { get; }

        public int Age { get; }

        public People(string name, int gender, int age)
        {
            Name = name;
            Gender = gender;
            Age = age;
        }

        public static People[] GetPeoples()
        {
            return new[]
            {
                new People("刘备", 0, 50),
                new People("关羽", 0, 45),
                new People("张飞", 0, 43),
                new People("大乔", 1, 30),
                new People("小乔", 1, 20),
            };
        }
    }
}
