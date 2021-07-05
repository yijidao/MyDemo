using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace DesignPattern.DesignPattern
{
    /// <summary>
    /// 中介者模式，调停者模式
    /// Define an object that encapsulates how a set of objects interact.
    /// Mediator promotes loose coupling by keeping objects from referring to each other explicitly,and it lets you vary their interaction independently.
    /// 定义一个对象封装一系列的对象交互。
    /// 中介者通过跟各个对象保持引用，实现各个对象之间的松耦合，从而实现可以单独修改一个对象而不影响其他对象。
    ///
    /// 中介者模式由几部分组成：
    /// 1. Mediator
    ///    抽象中介者角色。定义中介者的接口，用于各个同事类之间的交互。
    /// 2. Concrete Mediator
    ///    具体中介者角色。调用各个同事类进行交互，这个类必须依赖各个同事角色。
    /// 3. Colleague
    ///    同事角色。每个同事角色都关联中介者类，并且跟其他同事类通讯时必须通过中介者类。
    ///    同事类的行为可以分为两种：
    ///    1. 自我方法（Self-Method），比如改变自身的状态、处理自身的行为。
    ///    2. 依赖方法（Dep-Method），必须依赖中介者才能完成的行为。
    ///
    /// 优点：将本来的 N 对 N 关系，变成 N 对 1 关系
    /// 缺点：如果逻辑非常复杂，中介类也会很复杂
    /// 
    /// 如果是对象间的依赖是网状的，就一定要用中介者模式来解耦。
    /// </summary>
    class MediatorPattern
    {
        /// <summary>
        /// 耦合进销存系统
        /// </summary>
        public void CouplePssDemo()
        {
            var sale = new Sale();
            sale.Sell(80);
            sale.Sell(40);

        }

        /// <summary>
        /// 中介者模式进销存系统
        /// </summary>
        public void MpPssDemo()
        {
            var mediator = new Mediator();
            var sale = new SaleMp(mediator);
            sale.Sell(80);
            sale.Sell(40);

            var purchase = new PurchaseMp(mediator);
            purchase.Buy(50);

            var stock = new StockMp(mediator);
            stock.ClearStock();
        }
    }

    #region 进销存系统互相直接依赖的代码

    class Purchase
    {

        public void Buy(int number)
        {
            var stock = new Stock();
            var sale = new Sale();

            // 获取销售情况，大于 80 就是销售良好
            if (sale.GetSaleStatus() > 80)
            {
                stock.Increase(number);
                Console.WriteLine($"销售良好，正常采购：{number}");
            }
            else
            {
                stock.Increase(number / 2);
                Console.WriteLine($"销售不佳，折半采购：{number / 2}");
            }
        }

        public void RefuseBuy()
        {
            Console.WriteLine("停止采购");
        }
    }

    class Sale
    {
        public void Sell(int number)
        {
            var stock = new Stock();
            var purchase = new Purchase();

            while (stock.GetStockNumber() < number)
            {
                purchase.Buy(number);
            }

            Console.WriteLine($"销售数量为：{number}");
            stock.Decrease(number);
        }

        public int GetSaleStatus()
        {
            var rand = new Random(DateTime.Now.Millisecond);
            var saleStatue = rand.Next(100);
            Console.WriteLine($"销售情况为：{saleStatue}");
            return saleStatue;
        }

        public void OffSale()
        {
            var stock = new Stock();
            var count = stock.GetStockNumber();
            Console.WriteLine($"打折销售数量为：{count}");
            stock.Decrease(count);
        }
    }

    class Stock
    {
        private static int COUNT = 100;

        public void Increase(int number)
        {
            COUNT += number;
            Console.WriteLine($"库存数量为：{COUNT}");
        }

        public void Decrease(int number)
        {
            COUNT -= number;
            Console.WriteLine($"库存数量为:{COUNT}");
        }

        public int GetStockNumber() => COUNT;

        /// <summary>
        /// 清库存
        /// </summary>
        public void ClearStock()
        {
            var purchase = new Purchase();
            var sale = new Sale();
            Console.WriteLine($"清库存数量为：{COUNT}");
            sale.OffSale(); // 打折促销
            purchase.RefuseBuy(); // 停止采购
        }

    }

    #endregion

    #region 进销存系统，中介者模式

    abstract class AbstractMediator
    {
        public PurchaseMp Purchase { get; set; }
        public SaleMp Sale { get; set; }
        public StockMp Stock { get; set; }

        protected AbstractMediator()
        {
            Purchase = new PurchaseMp(this);
            Sale = new SaleMp(this);
            Stock = new StockMp(this);
        }

        public abstract void Execute(string str, params object[] objs);
    }

    class Mediator : AbstractMediator
    {
        public override void Execute(string str, params object[] objs)
        {
            switch (str)
            {
                case "purchase.buy":
                    Buy((int)objs[0]);
                    break;
                case "sale.sell":
                    Sell((int)objs[0]);
                    break;
                case "sale.offsell":
                    OffSell();
                    break;
                case "stock.clear":
                    ClearStock();
                    break;
            }
        }

        private void Buy(int number)
        {
            // 获取销售情况，大于 80 就是销售良好
            if (base.Sale.GetSaleStatus() > 80)
            {
                base.Stock.Increase(number);
                Console.WriteLine($"销售良好，正常采购：{number}");
            }
            else
            {
                base.Stock.Increase(number / 2);
                Console.WriteLine($"销售不佳，折半采购：{number / 2}");
            }
        }

        private void Sell(int number)
        {
            while (base.Stock.GetStockNumber() < number)
            {
                base.Purchase.Buy(number);
            }

            Console.WriteLine($"销售数量为：{number}");
            base.Stock.Decrease(number);
        }

        private void OffSell()
        {
            var count = base.Stock.GetStockNumber();
            Console.WriteLine($"打折销售数量为：{count}");
            base.Stock.Decrease(count);
        }

        private void ClearStock()
        {
            Console.WriteLine($"清库存数量为：{base.Stock.GetStockNumber()}");
            base.Sale.OffSale(); // 打折促销
            base.Purchase.RefuseBuy(); // 停止采购
        }
    }


    abstract class AbstractColleague
    {
        protected AbstractMediator Mediator { get; set; }

        protected AbstractColleague(AbstractMediator mediator)
        {
            Mediator = mediator;
        }
    }

    class PurchaseMp : AbstractColleague
    {
        public PurchaseMp(AbstractMediator mediator) : base(mediator)
        {

        }
        public void Buy(int number)
        {
            Mediator.Execute("purchase.buy", number);
        }

        public void RefuseBuy()
        {
            Console.WriteLine("停止采购");
        }
    }

    class StockMp : AbstractColleague
    {
        private static int COUNT = 100;

        public StockMp(AbstractMediator mediator) : base(mediator)
        {
        }

        public void Increase(int number)
        {
            COUNT += number;
            Console.WriteLine($"库存数量为：{COUNT}");
        }

        public void Decrease(int number)
        {
            COUNT -= number;
            Console.WriteLine($"库存数量为:{COUNT}");
        }

        public int GetStockNumber() => COUNT;

        public void ClearStock()
        {
            Mediator.Execute("stock.clear");
        }
    }

    class SaleMp : AbstractColleague
    {
        public SaleMp(AbstractMediator mediator) : base(mediator)
        {
        }

        public void Sell(int number)
        {
            Mediator.Execute("sale.sell", number);
        }

        public int GetSaleStatus()
        {
            var rand = new Random(DateTime.Now.Millisecond);
            var saleStatue = rand.Next(100);
            Console.WriteLine($"销售情况为：{saleStatue}");
            return saleStatue;
        }

        public void OffSale()
        {
            Mediator.Execute("sale.offsell");
        }
    }


    #endregion

    #region 中介者模式通用源码

    /// <summary>
    /// 抽象中介者
    /// </summary>
    abstract class MediatorSample
    {
        /// <summary>
        /// 同事类，这里使用具体类而不使用抽象类，是因为在抽象者模式中，常常需要调用具体同事类的自定义方法。
        /// </summary>
        protected ConcreteColleagueSample C1 { get; set; }
        /// <summary>
        /// 同事类，这里使用具体类而不使用抽象类，是因为在抽象者模式中，常常需要调用具体同事类的自定义方法。
        /// </summary>
        protected ConcreteColleagueSample C2 { get; set; }

        public abstract void DoSomething1();
        public abstract void DoSomething2();
    }

    /// <summary>
    /// 具体中介者
    /// </summary>
    class ConcreteMediatorSample : MediatorSample
    {
        public override void DoSomething1()
        {
            throw new NotImplementedException();
        }

        public override void DoSomething2()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 抽象同事类
    /// </summary>
    abstract class ColleagueSample
    {
        public MediatorSample Mediator { get; }

        protected ColleagueSample(MediatorSample mediator)
        {
            Mediator = mediator;
        }
    }

    /// <summary>
    /// 具体同事类
    /// </summary>
    class ConcreteColleagueSample : ColleagueSample
    {
        public ConcreteColleagueSample(MediatorSample mediator) : base(mediator)
        {
        }
        /// <summary>
        /// 自有方法，处理自己的逻辑
        /// </summary>
        public void SelftMethod()
        {

        }

        /// <summary>
        /// 依赖方法，自己不能处理的逻辑，委托给中介者处理
        /// </summary>
        public void DepMethod()
        {
            Mediator.DoSomething1();
        }
    }

    #endregion
}
