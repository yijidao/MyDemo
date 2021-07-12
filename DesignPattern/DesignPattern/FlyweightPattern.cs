using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.DesignPattern
{
    /// <summary>
    /// 享元模式
    /// Use sharing to support large numbers of fine-grained objects efficiently.
    /// 使用共享对象可以有效地支持大量细粒度的对象。
    /// 两个要求：
    /// - 共享对象。
    /// - 大量细粒度对象。
    ///
    /// 大量细粒度对象，所以对象数量多且性质相近，所以将这些对象的信息分为两个部分：
    /// - 内部状态 intrinsic
    ///   - 内部状态是对象可以共享出来的信息，存储在享元对象内部，并且不会随环境的改变而改变。属于可以共享的部分。
    /// - 外部状态 extrinsic
    ///   - 外部状态时对象依赖的一个标记，是对象的 Key，是随环境改变而改变的、不可共享的状态，是一批对象的统一标识，是唯一的一个索引值。
    ///
    /// 由四个角色组成：
    /// - 抽象享元角色 Flyweight
    ///   - 一个产品的抽象类，定义出对象的外部状态和内部状态的接口或实现。
    /// - 具体享元角色 ConcreteFlyweight
    ///   - 实现抽象角色定义的业务。需要注意的是内部状态处理应该与环境无关，不应该出现一个操作改变了内部状态，同时修改了外部状态。
    /// - 不可共享的享元角色 UnsharedConcreteFlyweight
    ///   - 不存在外部状态或者安全要求（如线程安全）不能够使用共享技术的对象，该对象一般不会出现在享元工厂中。
    /// - 享元工厂角色
    ///   - 构造一个池容器。同时提供从池中获得对象的方法。
    ///
    /// 优点
    /// - 共用对象，减少内存。
    /// 缺点
    /// - 需要区分外部状态、内部状态，增加了复杂度。
    ///
    /// 使用场景
    /// - 系统中存在大量相似角色。
    /// - 细粒度对象都具备较接近的外部状态，而且内部状态与环境无关，也就是说对象没有特定身份。
    /// - 需要缓冲池的场景。
    /// 
    /// </summary>
    class FlyweightPattern
    {
    }

    #region 享元模式模板代码
    /// <summary>
    /// 抽象享元角色
    /// 一个产品的抽象类，定义出对象的外部状态和内部状态的接口或实现。
    /// </summary>
    abstract class Flyweight
    {
        /// <summary>
        /// 外部状态
        /// </summary>
        public string Extrinsic { get; }

        /// <summary>
        /// 内部状态
        /// </summary>
        public string Intrinsic { get; set; }

        /// <summary>
        /// 要求享元角色必须接收外部状态
        /// </summary>
        /// <param name="extrinsic"></param>
        protected Flyweight(string extrinsic)
        {
            Extrinsic = extrinsic;
        }

        public abstract void Operate();
    }
    /// <summary>
    /// 具体享元角色
    /// 实现抽象角色定义的业务。需要注意的是内部状态处理应该与环境无关，不应该出现一个操作改变了内部状态，同时修改了外部状态。
    /// </summary>
    class ConcreteFlyweight : Flyweight
    {
        public ConcreteFlyweight(string extrinsic) : base(extrinsic)
        {
        }

        /// <summary>
        /// 根据外部状态进行的逻辑处理
        /// </summary>
        public override void Operate()
        {
            // 业务逻辑
        }
    }

    class ConcreteFlyweight2 : Flyweight
    {
        public ConcreteFlyweight2(string extrinsic) : base(extrinsic)
        {
        }

        public override void Operate()
        {
            // 业务逻辑
        }
    }

    /// <summary>
    /// 不可共享的享元角色
    /// 不存在外部状态或者安全要求（如线程安全）不能够使用共享技术的对象，该对象一般不会出现在享元工厂中。
    /// </summary>
    class UnsharedConcreteFlyweight : Flyweight
    {
        public UnsharedConcreteFlyweight(string extrinsic) : base(extrinsic)
        {
        }

        public override void Operate()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 享元工厂角色
    /// 构造一个池容器。同时提供从池中获得对象的方法。
    /// </summary>
    class FlyweightFactory
    {
        private static Dictionary<string, Flyweight> _pool = new Dictionary<string, Flyweight>();

        public static Flyweight GetFlyweight(string extrinsic)
        {
            Flyweight flyweight;
            if (_pool.ContainsKey(extrinsic))
            {
                flyweight = _pool[extrinsic];
            }
            else
            {
                flyweight = new ConcreteFlyweight(extrinsic);
                _pool.Add(extrinsic, flyweight);
            }

            return flyweight;
        }
    }

    #endregion
}
