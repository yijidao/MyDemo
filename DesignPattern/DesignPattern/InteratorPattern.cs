using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.DesignPattern
{
    /// <summary>
    /// 迭代器模式
    /// </summary>
    class InteratorPattern
    {
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
        private readonly bool _reverse;
        private int _position = -1;


        public ConcreteIterator(ConcreteIteratorAggregate aggregate, bool reverse = false)
        {
            _aggregate = aggregate;
            _reverse = reverse;
            if (_reverse)
            {
            }
        }

        public override bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public override void Reset()
        {
            throw new NotImplementedException();
        }

        public override object? Current()
        {
            throw new NotImplementedException();
        }
    }

    abstract class IteratorAggregate : IEnumerable
    {
        public abstract IEnumerator GetEnumerator();
    }

    class ConcreteIteratorAggregate : IteratorAggregate
    {

        public override IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    #endregion
}
