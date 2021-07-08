using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.DesignPattern
{
    /// <summary>
    /// 原型模式
    /// Specify the kinds of object to create using a prototypical instance,and create new objects by coping this prototype.
    /// 使用原型实例指定创建对象的类型，并且通过拷贝这个原型去创建新的对象。
    ///
    /// 一个复制工具类 https://github.com/Burtsev-Alexey/net-object-deep-copy/blob/bbe10e6bac16687cb28196f3b45238f1ac7aa072/ObjectExtensions.cs#L54
    ///
    /// 注意点：
    /// - .net 提供了 MemberwiseClone 进行浅拷贝。
    /// - 使用 MemberwiseClone 进行拷贝的时候，是不会调用构造函数的，因为拷贝是直接在内存通过二进制流进行拷贝的。
    /// - 浅拷贝在面对数组和对象时只复制引用，所以修改原型，也会影响到副本。
    /// - 想要深拷贝必须自己实现。
    ///
    /// 优点：
    /// - 性能好。是内存二进制流复制，所以比 new 对象性能更好，特别是在循环中大量产生对象时更明显。
    /// - 避开构造函数约束。因为通过原型拷贝不执行构造函数，而是直接在二进制流复制，然后在堆上开辟内存。
    /// 
    /// 使用场景：
    /// - 类初始化很耗费资源的场景。这个资源包括数据和硬件。
    /// - 性能和安全要求很高的场景。如果 new 一个对象需要非常繁琐的数据准备和安全校验，可以考虑用原型模式代替 new。
    /// - 一个对象多个修改者的场景。一个对象提供给多个修改者，并且修改者有可能修改其中的值，可以考虑原型模式。
    /// - 跟工厂模式结合使用。
    /// 
    /// </summary>
    class PrototypePattern
    {
        public void PrototypeDemo()
        {
            var prototype = new PrototypeClass
            {
                Id = 66,
                Info = new PrototypeInfoClass { Info = "详细信息" }
            };

            var shall = prototype.ShallCopy();
            var deep = prototype.DeepCopy();

            prototype.Info.Info = "修改后的详细信息";

            Console.WriteLine($"------  Shall Copy  ------");
            Console.WriteLine(shall);

            Console.WriteLine($"------  Deep Copy  ------");
            Console.WriteLine(deep);
        }
    }

    class PrototypeClass : ICloneable
    {
        public PrototypeClass()
        {
            Console.WriteLine("执行 PrototypeClass 构造函数");
        }


        public long Id { get; set; }

        public PrototypeInfoClass Info { get; set; }


        public object Clone()
        {
            return MemberwiseClone(); // 浅拷贝
        }

        /// <summary>
        /// 浅拷贝
        /// </summary>
        /// <returns></returns>
        public PrototypeClass ShallCopy()
        {
            return (PrototypeClass)MemberwiseClone();
        }

        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <returns></returns>
        public PrototypeClass DeepCopy()
        {
            var o = ShallCopy();
            o.Info = new PrototypeInfoClass
            {
                Info = this.Info.Info
            };
            return o;
        }

        public override string ToString() => $"Id:{Id}  Info:{Info.Info}";
    }

    class PrototypeInfoClass
    {
        public string Info { get; set; }

    }

}
