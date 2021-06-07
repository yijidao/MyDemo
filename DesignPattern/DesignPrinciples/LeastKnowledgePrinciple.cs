using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.DesignPrinciples
{
    /// <summary>
    /// 最小知识原则，又称迪米特原则 Law of Demeter。
    /// 定义就是，一个对象，应该对其他对象有最少的了解。
    /// 也就是说，一个类应该对自己需要耦合或者调用的类知道的最少，耦合或被调用的类实现多复杂不需要管，只需要知道目标类就提供了这么多 public 方法就好了，其他都不关心。
    ///
    /// 1. 只和朋友类交流。朋友类定义：出现在成员变量、方法的输入输出参数中的类，就是成员朋友类。
    /// 2. 和朋友类交流也是越少越好。就是调用朋友类的方法越少越好，类内部的逻辑越内聚越好，只提供少量的 public，多利用其他修饰符。
    ///
    /// 最小知识原则的目的是，让开发在写代码时，思考如何减少类跟类之间的耦合程度。
    /// 通过中间类跳转的方式来实现，但是中间类越多，链条就越长，所以也是一个取舍。
    /// 
    /// </summary>
    class LeastKnowledgePrinciple
    {
        public void GetGirlCount()
        {
            var teacher = new Teacher();
            teacher.Command(new GroupLeader());
        }

        /// <summary>
        /// 只和朋友类交流的demo
        /// </summary>
        public void GetGirlCount2()
        {
            var teacher = new Teacher();
            var girls = new Girl[20];
            for (int i = 0; i < 20; i++)
            {
                girls[i] = new Girl();
            }
            teacher.Command2(new GroupLeader(girls));
        }

        public void Install()
        {
            var install = new InstallSoftware();
            install.InstallWizard(new Wizard()); // 分别调用三个步骤

            install.InstallWizard2(new Wizard()); // 只调用一个方法，类跟类的耦合程度更低
        }
    }

    class Teacher
    {
        /// <summary>
        /// 命令组长去清点女生的人数
        /// 最小知识原则推荐我们只和朋友类交流，但是这个方法还拥有 girl 的局部变量，所以还能优化优化。
        /// </summary>
        /// <param name="groupLeader"></param>
        public void Command(GroupLeader groupLeader)
        {
            var girls = new Girl[20];
            for (int i = 0; i < 20; i++)
            {
                girls[i] = new Girl();
            }
            groupLeader.CountGirls(girls);
        }

        /// <summary>
        /// 根据最小知识原则优化，只跟朋友类交流
        /// 将对 Girl 的初始化移动到了场景类中，然后在 GroupLeader 中增加了 girl 的注入，从而避免了 Teacher 对 Girl 的访问，解耦一点点
        /// </summary>
        /// <param name="groupLeader"></param>
        public void Command2(GroupLeader groupLeader)
        {
            groupLeader.CountGirls2();
        }

       
    }

    class GroupLeader
    {
        public Girl[] Girls { get; }

        public GroupLeader(Girl[] girls)
        {
            Girls = girls;
        }

        public GroupLeader()
        {

        }

        public void CountGirls(Girl[] girls)
        {
            Console.WriteLine($"女生数量是：{girls.Length}");
        }

        public void CountGirls2()
        {
            Console.WriteLine($"女生数量是：{Girls.Length}");
        }
    }

    class Girl
    {
    }

    class InstallSoftware
    {
        /// <summary>
        /// 模拟软件安装
        /// 分别调用三个步骤
        /// </summary>
        /// <param name="wizard"></param>
        public void InstallWizard(Wizard wizard)
        {
            var first = wizard.First();
            if (first <= 50) return;
            var second = wizard.Second();
            if (second <= 50) return;
            wizard.Third();
        }

        /// <summary>
        /// 模拟软件安装
        /// 根据最小知识原则优化，调用逻辑更内聚的方法
        /// </summary>
        /// <param name="wizard"></param>
        public void InstallWizard2(Wizard wizard)
        {
            wizard.InstallWizard();
        }
    }

    class Wizard
    {
        private Random Rand { get; } = new Random(DateTime.Now.Millisecond);

        public int First()
        {
            Console.WriteLine("执行第一步");
            return Rand.Next(100);
        }

        public int Second()
        {
            Console.WriteLine("执行第二步");
            return Rand.Next(100);
        }

        public int Third()
        {
            Console.WriteLine("执行第三步");
            return Rand.Next(100);
        }

        /// <summary>
        /// 将三个业务方法合成一个，这样更逻辑内聚
        /// 可以将三个业务方法的 public 改成 private
        /// </summary>
        public void InstallWizard()
        {
            var first = First();
            if (first <= 50) return;
            var second = Second();
            if (second <= 50) return;
            Third();
        }
    }
}
