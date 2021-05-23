using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RxDotNetDemo.FunctionalThinking
{
    /// <summary>
    /// linq 操作符就是链式编程和拓展方法融合使用的一个标准案例，可以借鉴这种使用方法
    /// linq 有几点需要注意：
    /// 1. 在需要嵌套查询和join 关联等场景，一般使用 linq 表达式而不是linq  操作符。
    /// 2. linq 表达式不支持 distinct 关键字，所以需要手动调用 distinct 操作符。
    /// 3. linq 操作符是延迟执行的，按需执行的，内部通过使用 yield 来实现延迟功能的。
    /// 4. 因为 linq 操作符是延迟执行的，所以需要使用的时候要注意，在遇到 foreach 或者立刻执行的操作符（如 last、 count）才会执行。
    /// 5. 因为 linq 操作符是延迟执行的，所以 linq 的性能往往取决于实际的场景。
    /// 6. 延迟执行的好处就是性能优化。
    /// 7. 延迟执行可以用于动态构建查询。
    /// </summary>
    class QueryingCollectionsWithLinq
    {
        /// <summary>
        /// linq 就是标准的链式编程
        /// 使用 linq 返回大于10的奇数，并且结果值 + 2，去重并排序
        /// </summary>
        public static void LinqTest()
        {
            var numbers = new List<int> { 1, 35, 22, 6, 10, 11 };
            var result = numbers.Where(x => x % 2 == 1)
                .Where(x => x > 10)
                .Select(x => x + 2)
                .Distinct()
                .OrderBy(x => x);
            Console.WriteLine($"{string.Join(",", result)}");

            // linq 也可以使用非方法实现，有两点需要注意
            // 1. from... 开头  select 结尾
            // 2. linq 没有 distinct 关键字，只能手动调用方法去重
            var result2 = from number in numbers
                          where number % 2 == 1
                          where number > 10
                          orderby number
                          select number + 2;

            result2 = result2.Distinct();

            Console.WriteLine($"{string.Join(",", result2)}");
        }

        /// <summary>
        /// 使用 linq 实现嵌套子查询和关联，并且使用匿名类存储数据
        /// 这个 demo 使用 linq 实现书和作者关联，并打印对应的书和作者
        ///
        /// 匿名类和元组的注意点：
        /// 两个相似，都可以用来存储数据，
        /// 元组使用 Tuple.Create 创建，并且可以指定具体类型，可以充当方法的返回值和参数，
        /// 匿名类往往只能作为临时存储数据的数据结构，
        /// 但是使用元组作为返回值，可读性很差，所以为了减少bug，还是直接声明一个类比较合适
        /// </summary>
        public static void NestedQueriesAndJoins()
        {
            var authors = new[]
            {
                new Author(1, "张三"),
                new Author(2, "李四"),
            };

            var books = new[]
            {
                new Book("张三的书1", 1),
                new Book("李四的书1", 2),
                new Book("李四的书2", 2)
            };

            var result = from book in books
                         from author in authors
                         where book.AuthorId == author.Id
                         select new { Author = author.Name, Book = book.Name };

            foreach (var r in result)
            {
                Console.WriteLine($"{r.Author}写了{r.Book}");
            }

        }

        /// <summary>
        /// linq 需要注意的是，linq 是延迟执行的，是按需执行的，所以linq 有时候需要特别对待
        /// 按需执行的意思是，只有显示地遍历值（如 foreach）或者使用到一些立刻执行的操作符（如 Last 或者 Count）才会执行
        /// </summary>
        public static void EfficientByDeferredExecution()
        {
            var numbers = new List<int> { 1, 2, 3, 4 };
            var evenNumbers = from number in numbers
                              where number % 2 == 0
                              select number;
            numbers.Add(6); // 在 linq 之后又加入了 6，依旧会打印出来
            Console.WriteLine($"{string.Join(",",evenNumbers)}");
        }

        /// <summary>
        /// Linq 所有操作符都实现了 iterators，然后使用 yield 来实现延迟执行，也就是按需执行
        /// 延迟执行有许多好处，如性能优化（比如说只需要返回前五个数据），或者构建动态查询（使用条件语句）
        /// 这个 demo 实现了一个日志打印版本的 where 操作符，来简单地解释延迟执行的原理
        /// </summary>
        public static void TheYieldKeyword()
        {
            var numbers = new[] {1, 2, 3, 4, 5, 6};
            var evenNumbers = numbers.WhereWithLog(x => x % 2 == 0);
            Console.WriteLine("before foreach");
            foreach (var number in evenNumbers)
            {
                Console.WriteLine($"evenNumber:{number}");
            }

            // 延迟执行也可以动态构建查询的使用方式，下面代码就展示了这种方式
            // 在到达 foreach 之前，通过条件表达式来动态地构建查询，实现动态查询
            //if (/*some condition*/)
            //{
            //    query = query.Where(x => x > 5)
            //}
            //if (/*some condition*/)
            //{
            //    query = query.Where(x => x > 7)
            //}
            //foreach (var VARIABLE in COLLECTION)
            //{
                
            //}
        }
    }

    class Author
    {
        public long Id { get; }
        public string Name { get; }

        public Author(long id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    class Book
    {
        public string Name { get; }
        public long AuthorId { get; }

        public Book(string name, long authorId)
        {
            Name = name;
            AuthorId = authorId;
        }
    }

    static class EnumerableDeferredExtensions
    {
        public static IEnumerable<T> WhereWithLog<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            foreach (var item in source)
            {
                Console.WriteLine($"Checking item {item}");
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }
    }
}

