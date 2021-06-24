using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.DesignPattern
{
    /// <summary>
    /// 建造者模式，也叫做生成器模式。
    /// Separate the construction of a complex object from its representation so that the same construction precess can create different representations.
    /// 讲一个复杂对象的构建和表示分离，从而使同样的构建过程有不同的表示。
    /// </summary>
    class BuilderPattren
    {
    }

    public abstract class Builder
    {
        public abstract void SetPart();

        //public abstract Product BuildProduct();
    }
}
