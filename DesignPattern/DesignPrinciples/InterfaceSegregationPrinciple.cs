using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.DesignPrinciples
{
    /// <summary>
    /// 接口隔离原则
    /// 接口分两种，一种就是常用的接口，也就是 interface，第二种是类接口，也就是示例类或者抽象类
    ///
    /// 隔离也有两种定义：
    /// 1. Client should not be forced to depend upon interfaces that they don't user. 客户端不应该依赖其不需要的接口。
    /// 2. The dependency of one class to another one should depend on the smallest possible interface. 类间的依赖关系，应该建立在最小接口上。
    /// 其实就是表示，接口应该细化，不要建立臃肿庞大的接口。
    /// 这跟单一职责有点像，但又不太一样，单一职责要求按业务划分接口，而接口隔离原则要求接口的方法尽量少。
    ///
    /// 保证接口的纯洁性：
    /// 1. 接口要尽量小，但是不要为了小，而违背单一职责。
    /// 2. 接口要高内聚，就是 public 要少，也就是对外暴露的协议要少。
    /// 3. 定制服务，意思就是实现类可以实现多个接口，但是向外暴露接口的时候，只暴露用户需要的接口。
    /// 4. 适度设计，不要为了小而硬拆。
    ///
    /// 最佳实践：
    /// 1. 一个接口只服务一个子模块或者逻辑，也就是要满足单一职责。
    /// 2. 接口内的方法要少，不要有不必要方法。
    /// 3. 如果接口已经被污染，尽量去修改，如果修改的风险大，则采用适配器模式进行转换。
    /// 
    /// </summary>
    class InterfaceSegregationPrinciple
    {
        public void ShowPrettyGrid()
        {
            var searcher = new Searcher();
            searcher.Show(new PrettyGirl()); // 接口粒度还不够细的时候

            var prettyGridISP = new PrettyGirlISP(); // 接口粒度再细化的时候，同一个实例，通过不同的接口满足不同的有业务场景
            searcher.Show((IGoodBodyGirl)prettyGridISP);
            searcher.Show((IGreatTemperamentGirl)prettyGridISP);

        }


    }

    /// <summary>
    /// 定义一个标准美女，颜好身材好气质佳
    /// </summary>
    public interface IPrettyGirl
    {
        /// <summary>
        /// 脸好看
        /// </summary>
        public void GoodLooking();

        /// <summary>
        /// 身材好
        /// </summary>
        public void NiceFigure();

        /// <summary>
        /// 有气质
        /// </summary>
        public void GreatTemperament();
    }

    public class PrettyGirl : IPrettyGirl
    {
        public void GoodLooking()
        {
            Console.WriteLine("脸好看...");
        }

        public void NiceFigure()
        {
            Console.WriteLine("身材好...");
        }

        public void GreatTemperament()
        {
            Console.WriteLine("气质好...");
        }
    }

    /// <summary>
    /// 星探抽象类
    /// </summary>
    public abstract class AbstractSearcher
    {
        /// <summary>
        /// 展示标准美女
        /// </summary>
        public abstract void Show(IPrettyGirl prettyGirl);

        /// <summary>
        /// 展示外形美女
        /// </summary>
        /// <param name="goodBodyGirl"></param>
        public abstract void Show(IGoodBodyGirl goodBodyGirl);

        /// <summary>
        /// 展示气质美女
        /// </summary>
        /// <param name="greatTemperamentGirl"></param>
        public abstract void Show(IGreatTemperamentGirl greatTemperamentGirl);
    }

    public class Searcher : AbstractSearcher
    {

        public override void Show(IPrettyGirl prettyGirl)
        {
            prettyGirl.GoodLooking();
            prettyGirl.NiceFigure();
            prettyGirl.GreatTemperament();
        }

        public override void Show(IGoodBodyGirl goodBodyGirl)
        {
            goodBodyGirl.GoodLooking();
            goodBodyGirl.NiceFigure();
        }

        public override void Show(IGreatTemperamentGirl greatTemperamentGirl)
        {
            greatTemperamentGirl.GreatTemperament();
        }
    }

    /// <summary>
    /// 气质美女
    /// </summary>
    public interface IGreatTemperamentGirl
    {
        public void GreatTemperament();
    }

    /// <summary>
    /// 外型美女
    /// </summary>
    public interface IGoodBodyGirl
    {
        public void GoodLooking();
        public void NiceFigure();
    }

    /// <summary>
    /// 美女可以有气质美女，也可有外形美女
    /// </summary>
    public class PrettyGirlISP : IGreatTemperamentGirl, IGoodBodyGirl
    {
        public void GreatTemperament()
        {
            Console.WriteLine("气质佳");
        }

        public void GoodLooking()
        {
            Console.WriteLine("颜好");
        }

        public void NiceFigure()
        {
            Console.WriteLine("身材好");
        }
    }
}
