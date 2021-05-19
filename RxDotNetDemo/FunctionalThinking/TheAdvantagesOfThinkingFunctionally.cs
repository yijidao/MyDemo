using System;
using System.Collections.Generic;
using System.Text;

namespace RxDotNetDemo.FunctionalThinking
{
    /// <summary>
    /// 编程的目标是更高的生产力和鲁棒性。
    /// 实现两个目标有多种方式，如：短代码、可读、内部资源管理等等。
    /// 通过不同的编程方式，可以提供更多角度的编程思考。
    /// 编程光多练还不够，要学会升华，举一反三。
    /// 
    /// 一般编程都是OOP方式，OOP 代码是编程思考方式是抽象、封装、继承、多态
    /// 抽象：通过分离出业务的共性和相似性，并且这些行为和属性归为一个类，这个类只考虑共性的事情，这是聚焦式思考，可以自顶向下的思考
    /// 封装：抽象完通过接口、抽象类等方式进行封装，隐藏实现细节，只开放部分功能。通过这种分离，可以从使用者和开发者的角度思考编程，可以从设计和实现的角度思考编程
    /// 继承：有接口继承、类继承，主要是提供扩展已有代码的思考方式
    /// 多态：通过继承和override 实现，提供扩展已有代码的另一个思考方式
    ///
    /// 函数式编程
    /// 函数式编程提供了其他的编程思考方式，声明式编程、不可变特性、函数优先原则。
    /// Declarative style of programing：Tell what,not how。如xaml、html就是声明书编程的好处，人类可读，所以不易出错。
    /// Immutability: 不要修改值，而是生成一个新的值。value struct 就是一个例子，react 的状态管理也是一个例子。好处不言而喻，就是不用担心状态和多线程问题。加锁太烦了。
    /// First-class functions: 通过一个个具有不可变特性的函数去组成整个代码逻辑，实现声明式的代码。
    /// </summary>
    public class TheAdvantagesOfThinkingFunctionally
    {
    }
}
