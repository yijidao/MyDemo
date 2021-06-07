using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.DesignPrinciples
{
    /// <summary>
    /// 开闭原则
    /// 指应该对扩展开发，对修改关闭，其实就是说，软件实体（模块、抽象和类、方法）应该通过扩展来实现变化，而不限通过修改来实现变化。
    ///
    /// 变化一般分为逻辑变化，或者模块变化
    /// 逻辑变化就像是实现方法的 a*b 改成 a+b，这种直接改实现类就可以
    /// 如果是模块变化，就要多考虑一点，特别是底层模块的改变可以引起高层模块的改变，这时候可以通过继承来改
    ///
    /// 开闭原则的思路是在当业务发生变化的时候，想办法把对原有实现的影响降到最低。
    /// 所以在设计的时候应该多抽象，比如说要有接口，方法的参数返回值也用接口，不直接用实现类等等
    /// 
    /// </summary>
    class OpenClosedPrinciple
    {
        public void MockSell()
        {
            var store = new BookStore();
            store.Sell();
        }

        /// <summary>
        /// 模拟引入新业务，对书进行打折
        /// 不是通过修改原本类的 Price，或者给增加增加一个 OffPrice，而是添加子类 OffNovelBook，覆盖 Price
        /// 因为接口本来就是合理的，而且频繁修改接口，接口就起不到契约的作用了，所以不应该修改接口
        /// 直接修改 Book 的 Price 的话，如果需要看原价，就看不了了，所以选择子类扩展
        /// </summary>
        public void MockOffSell()
        {
            var store = new BookStore();
            store.SellOff();
        }
    }

    interface IBook
    {
        public string Name { get; set; }

        public string Author { get; set; }

        /// <summary>
        /// 在非金融项目中对货币进行处理时，一般取两位精度，通常的设计方法是在运算过程中扩大 100 倍，在需要展示时再缩小 100 倍
        /// 金融项目一般用 decimal
        /// </summary>
        public int Price { get; set; }
    }

    /// <summary>
    /// 小说
    /// </summary>
    class NovelBook : IBook
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public virtual int Price { get; set; }

        public NovelBook(string name, string author, int price)
        {
            Name = name;
            Author = author;
            Price = price;
        }
    }

    /// <summary>
    /// 打折书，高于 40 元打 9 折，其他八折
    /// </summary>
    class OffNovelBook : NovelBook
    {
        private int _price;

        public OffNovelBook(string name, string author, int price) : base(name, author, price)
        {

        }

        public override int Price
        {
            get
            {
                if (_price > 4000)
                {
                    return _price * 90 / 100;
                }
                else
                {
                    return _price * 80 / 100;
                }
            }
            set => _price = value;
        }
    }

    class BookStore
    {
        public static IBook[] Books { get; } =
        {
            new NovelBook("天龙八部", "金庸", 3200),
            new NovelBook("巴黎圣母院", "雨果", 5600),
            new NovelBook("悲惨世界", "雨果", 3500),
            new NovelBook("金瓶梅", "兰陵笑笑生", 4300),
        };

        public static IBook[] OffBooks { get; } =
        {
            new OffNovelBook("天龙八部", "金庸", 3200),
            new OffNovelBook("巴黎圣母院", "雨果", 5600),
            new OffNovelBook("悲惨世界", "雨果", 3500),
            new OffNovelBook("金瓶梅", "兰陵笑笑生", 4300),
        };

        public void Sell()
        {
            Console.WriteLine("------  书店卖出的书籍记录如下：  ------");
            foreach (var book in Books)
            {
                Console.WriteLine($"名称：{book.Name}\t作者：{book.Author}\t价格：{book.Price / 100:#.00}元");
            }
        }

        public void SellOff()
        {
            Console.WriteLine("------  书店卖出的书籍记录如下：  ------");
            foreach (var book in OffBooks)
            {
                Console.WriteLine($"名称：{book.Name}\t作者：{book.Author}\t价格：{book.Price / 100:#.00}元");
            }
        }
    }


}
