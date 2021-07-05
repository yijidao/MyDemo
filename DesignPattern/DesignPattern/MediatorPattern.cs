using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.DesignPattern
{
    /// <summary>
    /// 中介者模式，调停者模式
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

}
