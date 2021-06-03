using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.DesignPrinciples
{
    /// <summary>
    /// 依赖倒置原则 DIP
    /// High level modules should not depend upon low level modules.Both should depend upon abstractions. 高层模块不应该依赖底层模块，两者都应该依赖其抽象。
    /// Abstractions should not depend upon details.Details should depend upon abstractions.抽象不应该依赖细节，细节应该依赖抽象。
    ///
    /// 底层模块就是原子逻辑，也就是无法再分割的逻辑，由底层模块组成的就是高层模块。
    /// 抽象就是接口、抽象类，细节就是实现类。
    /// 依赖倒置在代码中的表现形式就是：
    /// 1. 模块间的依赖通过抽象类或者接口实现，而不是实现类之间互相直接依赖。
    /// 2. 接口或抽象类不依赖实现类，实现类依赖接口或抽象类。
    /// 更简单来说，就是面向接口编程，这个接口不一定是严格的 Interface，而是指契约。
    ///
    /// 依赖倒置有利于扩展，又利于并行开发（先定义抽象，各个开发再进行实现类的开发，比如单元测试就是依赖倒置的极佳使用场景）
    ///
    /// 依赖的三种写法：
    /// 1. 构造函数参数传递依赖对象
    /// 2. 属性赋值依赖对象
    /// 3. 接口依赖，就是在方法参数传递接口（Driver.DriveForDIP 就是这个例子）。
    ///
    /// 最佳实践：
    /// 1. 每个类尽量有抽象类或者接口。
    /// 2. 变量的表面类型尽量是接口或者抽象类。
    /// 3. 任何类都不应该从具体类派生。
    /// 4. 尽量不要覆写基类地方法，只要覆写了，就有可能对依赖的稳定造成破坏。
    /// 5. 结合里氏替换来思考。
    /// </summary>
    class DependenceInversionPrinciple
    {

        public void DIPDemo()
        {
            var benz = new Benz();
            var bmw = new BMW();

            var driver = new Driver(); // 司机，可以开各种各样的车
            driver.Drive(benz); // 直接依赖，只能开奔驰，不能开宝马

            driver.DriveForDIP(benz); // 依赖倒置，只要实现了 ICar 就能开
            driver.DriveForDIP(bmw); 
        }
    }

    public interface ICar
    {
        public void Run();
    }

    public class Benz : ICar
    {
        public void Run()
        {
            Console.WriteLine("奔驰车开始运行...");
        }
    }

    public class BMW : ICar
    {
        public void Run()
        {
            Console.WriteLine("宝马车开始运行...");
        }
    }

    public class Driver
    {
        public void Drive(Benz benz)
        {
            benz.Run();
        }

        public void DriveForDIP(ICar car)
        {
            car.Run();
        }
    }
}
