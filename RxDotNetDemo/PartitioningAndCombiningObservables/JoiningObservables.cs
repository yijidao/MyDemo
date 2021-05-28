using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using RxDotNetDemo.Extensions;

namespace RxDotNetDemo.PartitioningAndCombiningObservables
{
    /// <summary>
    /// 类似于数据库的 join 和 集合的 join，它们的 join 是通过字段或者属性的一致，来表达两个数据或者子项间存在关联关系。
    /// 在流的世界里，也存在join，Rx 通过重叠的时间片（也就是在 A 发射出来的时候，B也发射出来了，两者定义的时间片存在重叠）来表达，两个发射出的通知是存在关联关系的。
    /// 
    /// </summary>
    class JoiningObservables
    {
        /// <summary>
        /// 使用 join 来对两个 observable 进行关联
        /// left 和 right 是两个用来关联的 observable
        /// rightDurationSelector 用来决定，right observable 的通知的时间片是多长
        /// leftDurationSelector 用来决定，left observable 的通知的时间片是多长
        /// 两个时间片重叠就证明存在关联关系
        ///
        /// 这个 demo 描述了当男生和女生同时在房间里时，就打印出来
        /// 
        /// </summary>
        public static void JoiningToAFlatStream()
        {
            var doorOpenedSubject = new Subject<DoorOpened>();

            var entrances = doorOpenedSubject.Where(o => o.Direction == OpenDirection.Entering);
            var maleEntering = entrances.Where(x => x.Gender == Gender.Male);
            var femaleEntering = entrances.Where(x => x.Gender == Gender.Female);

            var exits = doorOpenedSubject.Where(o => o.Direction == OpenDirection.Leaving);
            var maleExiting = exits.Where(x => x.Gender == Gender.Male);
            var femaleExiting = exits.Where(x => x.Gender == Gender.Female);

            // left 和 right 是两个用来关联的 observable
            // rightDurationSelector 用来决定，right observable 的通知的时间片是多长
            // leftDurationSelector 用来决定，left observable 的通知的时间片是多长
            // 两个时间片重叠就证明存在关联关系
            maleEntering.Join(femaleEntering,
                male => maleExiting.Where(exit => exit.Name == male.Name),
                female => femaleExiting.Where(exit => exit.Name == female.Name),
                (m, f) => new { Male = m.Name, Female = f.Name })
                .SubscribeConsole("Together At Room");

            doorOpenedSubject.OnNext(new DoorOpened("Bob", Gender.Male, OpenDirection.Entering));
            doorOpenedSubject.OnNext(new DoorOpened("Sara", Gender.Female, OpenDirection.Entering));
            doorOpenedSubject.OnNext(new DoorOpened("John", Gender.Male, OpenDirection.Entering));
            doorOpenedSubject.OnNext(new DoorOpened("Sara", Gender.Female, OpenDirection.Leaving));
            doorOpenedSubject.OnNext(new DoorOpened("Fibi", Gender.Female, OpenDirection.Entering));
            doorOpenedSubject.OnNext(new DoorOpened("Bob", Gender.Male, OpenDirection.Leaving));
            doorOpenedSubject.OnNext(new DoorOpened("Dan", Gender.Male, OpenDirection.Entering));
            doorOpenedSubject.OnNext(new DoorOpened("Fibi", Gender.Female, OpenDirection.Leaving));
            doorOpenedSubject.OnNext(new DoorOpened("John", Gender.Male, OpenDirection.Leaving));
            doorOpenedSubject.OnNext(new DoorOpened("Dan", Gender.Male, OpenDirection.Leaving));
        }

        /// <summary>
        /// join 的子句写法
        /// </summary>
        public static void JoiningToAFlatStreamClause()
        {
            var doorOpenedSubject = new Subject<DoorOpened>();

            var entrances = doorOpenedSubject.Where(o => o.Direction == OpenDirection.Entering);
            var maleEntering = entrances.Where(x => x.Gender == Gender.Male);
            var femaleEntering = entrances.Where(x => x.Gender == Gender.Female);

            var exits = doorOpenedSubject.Where(o => o.Direction == OpenDirection.Leaving);
            var maleExiting = exits.Where(x => x.Gender == Gender.Male);
            var femaleExiting = exits.Where(x => x.Gender == Gender.Female);

            var o = from male in maleEntering
                    join female in femaleEntering on maleExiting.Where(exit => exit.Name == male.Name) equals
                        femaleExiting.Where(exit => exit.Name == female.Name)
                    select new { Male = male.Name, Female = female.Name };
            o.SubscribeConsole("Together At Room Use Clause");

            doorOpenedSubject.OnNext(new DoorOpened("Bob", Gender.Male, OpenDirection.Entering));
            doorOpenedSubject.OnNext(new DoorOpened("Sara", Gender.Female, OpenDirection.Entering));
            doorOpenedSubject.OnNext(new DoorOpened("John", Gender.Male, OpenDirection.Entering));
            doorOpenedSubject.OnNext(new DoorOpened("Sara", Gender.Female, OpenDirection.Leaving));
            doorOpenedSubject.OnNext(new DoorOpened("Fibi", Gender.Female, OpenDirection.Entering));
            doorOpenedSubject.OnNext(new DoorOpened("Bob", Gender.Male, OpenDirection.Leaving));
            doorOpenedSubject.OnNext(new DoorOpened("Dan", Gender.Male, OpenDirection.Entering));
            doorOpenedSubject.OnNext(new DoorOpened("Fibi", Gender.Female, OpenDirection.Leaving));
            doorOpenedSubject.OnNext(new DoorOpened("John", Gender.Male, OpenDirection.Leaving));
            doorOpenedSubject.OnNext(new DoorOpened("Dan", Gender.Male, OpenDirection.Leaving));

        }

        /// <summary>
        /// GroupJoin 实现分组关联，跟 Join 一样是通过时间片来定义关联关系的
        /// 不过 groupjoin 是返回一个 observable，同一个分组的会被发射到同一个 observable 中
        /// </summary>
        public static void JoiningIntoGroups()
        {
            var doorOpenedSubject = new Subject<DoorOpened>();

            var entrances = doorOpenedSubject.Where(o => o.Direction == OpenDirection.Entering);
            var exits = doorOpenedSubject.Where(o => o.Direction == OpenDirection.Leaving);

            var maleEntering = entrances.Where(x => x.Gender == Gender.Male);
            var femaleEntering = entrances.Where(x => x.Gender == Gender.Female);

            var maleExiting = exits.Where(x => x.Gender == Gender.Male);
            var femaleExiting = exits.Where(x => x.Gender == Gender.Female);

            // resultSelector 的参数是普通对象和一个  observable
            var malesAcquaintances = maleEntering.GroupJoin(femaleEntering,
                male => maleExiting.Where(exit => exit.Name == male.Name),
                female => femaleExiting.Where(exit => exit.Name == female.Name),
                (m, females) => new { Male = m.Name, Females = females});


            // 通过 Scan 显示当前一个男生同时和几个女生在一个房间里
            var amountPerUser = from acquaintances in malesAcquaintances
                from cnt in acquaintances.Females.Scan(0, (acc, curr) => acc + 1)
                select new {acquaintances.Male, cnt};

            amountPerUser.SubscribeConsole("Amount of meetings for User");

            doorOpenedSubject.OnNext(new DoorOpened("Bob", Gender.Male, OpenDirection.Entering));
            doorOpenedSubject.OnNext(new DoorOpened("Sara", Gender.Female, OpenDirection.Entering));
            doorOpenedSubject.OnNext(new DoorOpened("John", Gender.Male, OpenDirection.Entering));
            doorOpenedSubject.OnNext(new DoorOpened("Sara", Gender.Female, OpenDirection.Leaving));
            doorOpenedSubject.OnNext(new DoorOpened("Fibi", Gender.Female, OpenDirection.Entering));
            doorOpenedSubject.OnNext(new DoorOpened("Bob", Gender.Male, OpenDirection.Leaving));
            doorOpenedSubject.OnNext(new DoorOpened("Dan", Gender.Male, OpenDirection.Entering));
            doorOpenedSubject.OnNext(new DoorOpened("Fibi", Gender.Female, OpenDirection.Leaving));
            doorOpenedSubject.OnNext(new DoorOpened("John", Gender.Male, OpenDirection.Leaving));
            doorOpenedSubject.OnNext(new DoorOpened("Dan", Gender.Male, OpenDirection.Leaving));
        }

        public static void JoiningIntoGroupsUseClause()
        {
            var doorOpenedSubject = new Subject<DoorOpened>();

            var entrances = doorOpenedSubject.Where(o => o.Direction == OpenDirection.Entering);
            var exits = doorOpenedSubject.Where(o => o.Direction == OpenDirection.Leaving);

            var maleEntering = entrances.Where(x => x.Gender == Gender.Male);
            var femaleEntering = entrances.Where(x => x.Gender == Gender.Female);

            var maleExiting = exits.Where(x => x.Gender == Gender.Male);
            var femaleExiting = exits.Where(x => x.Gender == Gender.Female);

            var malesAcquaintances = from male in maleEntering
                join female in femaleEntering on maleExiting.Where(e => e.Name == male.Name) equals femaleExiting.Where(
                        e => e.Name == female.Name)
                    into maleEncounters
                select new {Male = male, Females = maleEncounters};

            // 通过 Scan 显示当前一个男生同时和几个女生在一个房间里
            var amountPerUser = from acquaintances in malesAcquaintances
                from cnt in acquaintances.Females.Scan(0, (acc, curr) => acc + 1)
                select new { acquaintances.Male.Name, cnt };
            amountPerUser.SubscribeConsole("Amount of meetings for User");


            doorOpenedSubject.OnNext(new DoorOpened("Bob", Gender.Male, OpenDirection.Entering));
            doorOpenedSubject.OnNext(new DoorOpened("Sara", Gender.Female, OpenDirection.Entering));
            doorOpenedSubject.OnNext(new DoorOpened("John", Gender.Male, OpenDirection.Entering));
            doorOpenedSubject.OnNext(new DoorOpened("Sara", Gender.Female, OpenDirection.Leaving));
            doorOpenedSubject.OnNext(new DoorOpened("Fibi", Gender.Female, OpenDirection.Entering));
            doorOpenedSubject.OnNext(new DoorOpened("Bob", Gender.Male, OpenDirection.Leaving));
            doorOpenedSubject.OnNext(new DoorOpened("Dan", Gender.Male, OpenDirection.Entering));
            doorOpenedSubject.OnNext(new DoorOpened("Fibi", Gender.Female, OpenDirection.Leaving));
            doorOpenedSubject.OnNext(new DoorOpened("John", Gender.Male, OpenDirection.Leaving));
            doorOpenedSubject.OnNext(new DoorOpened("Dan", Gender.Male, OpenDirection.Leaving));
        }
    }

    class DoorOpened
    {
        public string Name { get; set; }

        public OpenDirection Direction { get; set; }

        public Gender Gender { get; set; }

        public DoorOpened(string name, Gender gender, OpenDirection direction)
        {
            Name = name;
            Direction = direction;
            Gender = gender;
        }
    }

    enum OpenDirection
    {
        Entering,
        Leaving
    }

    enum Gender
    {
        Female,
        Male
    }
}
