using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.DesignPattern
{
    /// <summary>
    /// 迭代器模式
    /// Provide a way to access the elements of an aggregate object sequentially without exposing its underlying representation.
    /// 提供一种方法去顺序地访问容器对象中的元素，而有不需要暴露该容器对象的内部实现细节。
    ///
    /// 迭代器模式是为了容器（list、set 等等）而服务的，是为了解决遍历容器中的元素。
    /// 容器只需要管理增减元素，需要遍历时则有迭代器负责。
    /// 
    /// 主要有四个角色：
    /// - Aggregate
    ///   - 抽象容器角色。必然有一个提供迭代器的方法。并且可能有其他管理容器内元素增减的方法。
    /// - ConcreteAggregate
    ///   - 具体容器角色。
    /// - Iterator
    ///   - 抽象迭代器角色。必然有访问下一个元素和判断是否已经访问到底的方法。
    /// - ConcreteAggregate
    ///   - 具体迭代器角色。
    ///
    /// 迭代器模式因为实在用的太多，所以在 .net 已经提供了接口。
    /// - 抽象容器角色实现 IEnumerator。
    /// - 抽象迭代器角色实现 IEnumerable。
    /// - 也可以直接使用 yield return  关键字直接生成迭代器类，CLR 已经在编译器层级提供了支持。
    /// 
    /// </summary>
    class InteratorPattern
    {
        public void InteratorDemo()
        {
            var cia = new ConcreteIteratorAggregate();
            cia.Add("1");
            cia.Add("2");
            cia.Add("3");
            cia.Add("4");

            var i = cia.GetEnumerator();

            while (i.MoveNext())
            {
                Console.WriteLine(i.Current);
            }

            var cia2 = new ConcreteIteratorAggregate2();
            cia2.Add("4");
            cia2.Add("3");
            cia2.Add("2");
            cia2.Add("1");
            var i2 = cia2.GetEnumerator();
            while (i2.MoveNext())
            {
                Console.WriteLine(i2.Current);
            }
        }
    }

    #region 迭代器模式模板代码

    abstract class Iterator : IEnumerator
    {
        public abstract bool MoveNext();

        public abstract void Reset();

        object? IEnumerator.Current => Current();

        public abstract object? Current();

    }

    class ConcreteIterator : Iterator
    {
        private readonly ConcreteIteratorAggregate _aggregate;
        private int _position = -1;


        public ConcreteIterator(ConcreteIteratorAggregate aggregate)
        {
            _aggregate = aggregate;
        }

        public override bool MoveNext()
        {
            var next = ++_position;
            if (next >= _aggregate.GetItems().Count) return false;

            _position = next;
            return true;
        }

        public override void Reset()
        {
            _position = -1;
        }

        public override object? Current() => _aggregate.GetItems()[_position];
    }

    abstract class IteratorAggregate : IEnumerable
    {
        public abstract IEnumerator GetEnumerator();
    }

    class ConcreteIteratorAggregate : IteratorAggregate
    {
        private readonly List<string> _list = new List<string>();

        public void Add(string item) => _list.Add(item);

        public List<string> GetItems() => _list;


        public override IEnumerator GetEnumerator()
        {
            return new ConcreteIterator(this);
        }
    }

    #endregion

    #region 迭代器模式 yield 简化实现

    class ConcreteIteratorAggregate2 : IEnumerable
    {
        private readonly List<string> _list = new List<string>();


        public void Add(string item) => _list.Add(item);

        public List<string> GetItems() => _list;

        public IEnumerator GetEnumerator()
        {
            foreach (var s in _list)
            {
                yield return s;
            }
        }
    }

    #endregion
}
