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
                .SubscribeConsole("Together At Root");

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
