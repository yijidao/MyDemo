using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.PatternCategory
{
    /// <summary>
    /// 创建类设计模式
    /// 单例模式、原型模式、简单工厂模式、抽象工厂模式、建造者模式。
    /// 单例模式是为了生成单一实例。
    /// 原型模式是根据原型拷贝实例。
    /// 工厂模式关注的是生成不同的产品对象，关注的是产品整体，生产出的产品应该具有相似的功能和架构。
    /// 抽象工厂模式是简单工厂的扩展，关注的是多个产品族的生产，一个工厂负责一个产品族的生产。
    /// 建造者模式关注的是产品对象的装配过程。
    /// </summary>
    class CreationalPatterns
    {
    }

    #region 工厂模式和建造者模式对比
    /*
    - 意图不同
      - 工厂模式关注产品整体
      - 建造者模式关注产品的创建过程
    - 复杂度不同，粒度不同
      - 工厂模式创建的一般都是单一性质的产品
      - 建造者模式一般都是复合型产品
    */

    #region 工厂模式
    /// <summary>
    /// 超人接口
    /// 抽象产品角色
    /// </summary>
    interface ISuperMan
    {
        /// <summary>
        /// 超能力
        /// </summary>
        public void SpecialTalent();
    }

    /// <summary>
    /// 成年超人
    /// 具体产品角色
    /// </summary>
    class AdultSuperMan : ISuperMan
    {
        public void SpecialTalent()
        {
            Console.WriteLine("成年超人力大无穷！");
        }
    }

    /// <summary>
    /// 未成年超人
    /// 具体产品角色
    /// </summary>
    class ChildSuperMan : ISuperMan
    {
        public void SpecialTalent()
        {
            Console.WriteLine("未成年超人会飞！");
        }
    }
    /// <summary>
    /// 超人工厂
    /// 具体工厂角色
    /// </summary>
    class SuperManFactory
    {
        public static ISuperMan CreateSuperMan(string type)
        {
            return type.ToLower() switch
            {
                "adult" => new AdultSuperMan(),
                "child" => new ChildSuperMan(),
                _ => null
            };
        }
    }

    class SuperManClient
    {
        public static void SuperManFactoryDemo()
        {
            var superman = SuperManFactory.CreateSuperMan("adult");
            superman.SpecialTalent();
            superman = SuperManFactory.CreateSuperMan("child");
            superman.SpecialTalent();
        }
    }

    #endregion

    #region 建造者模式

    abstract class SuperManBuilder
    {
        protected SuperMan2 SuperMan = new SuperMan2();

        public void SetBody(string body) => SuperMan.Body = body;
        public void SetSpecialTalent(string specialTalent) => SuperMan.SpecialTalent = specialTalent;
        public void SetSpecialSymbol(string symbol) => SuperMan.SpecialSymbol = symbol;
        public abstract SuperMan2 GetSuperMan();
    }

    class AdultSuperManBuilder : SuperManBuilder
    {
        public override SuperMan2 GetSuperMan()
        {
            SetBody("强壮的身体");
            SetSpecialTalent("力大无穷");
            SetSpecialSymbol("S");
            return SuperMan;
        }
    }

    class ChildSuperManBuilder : SuperManBuilder
    {
        public override SuperMan2 GetSuperMan()
        {
            SetBody("强壮的身体");
            SetSpecialTalent("会飞");
            SetSpecialSymbol("Mini S");
            return SuperMan;
        }
    }

    class SuperMan2
    {
        /// <summary>
        /// 身体
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 超能力
        /// </summary>
        public string SpecialTalent { get; set; }

        /// <summary>
        /// 标志
        /// </summary>
        public string SpecialSymbol { get; set; }

    }

    class SuperManDirector
    {
        public SuperManBuilder AdultSuperManBuilder { get; set; }
        public SuperManBuilder ChildSuperManBuilder { get; set; }

        public SuperMan2 GetAdultSuperMan() => AdultSuperManBuilder.GetSuperMan();

        public SuperMan2 GetChildSuperMan() => ChildSuperManBuilder.GetSuperMan();
    }

    #endregion

    #endregion


    #region 抽象工厂和建造者模式对比
    /*
     * 抽象工厂关注的是产品族的创建，使用不同的工厂创建不同分类的产品。
     * 
     */

    #region 抽象工厂
    /// <summary>
    /// 抽象产品角色
    /// </summary>
    interface ICar
    {
        /// <summary>
        /// 汽车品牌
        /// </summary>
        /// <returns></returns>
        public string GetBand();
        /// <summary>
        /// 汽车类型，SUV、商务车
        /// </summary>
        /// <returns></returns>
        public string GetModel();
    }

    abstract class AbsBenz : ICar
    {
        public string GetBand() => "奔驰";

        public abstract string GetModel();
    }

    class BenzSuv : AbsBenz
    {
        public override string GetModel() => "Suv";
    }

    class BenzVan : AbsBenz
    {
        public override string GetModel() => "商务车";
    }

    abstract class AbsBMW : ICar
    {
        public string GetBand() => "宝马";

        public abstract string GetModel();
    }

    class BMWSuv : AbsBMW
    {
        public override string GetModel() => "SUV";
    }

    class BMWVan : AbsBMW
    {
        public override string GetModel() => "商务车";
    }
    /// <summary>
    /// 抽象工厂角色
    /// </summary>
    interface ICarFactory
    {
        /// <summary>
        /// 生产 Suv
        /// </summary>
        /// <returns></returns>
        ICar CreateSnv();

        /// <summary>
        /// 生产商务车
        /// </summary>
        /// <returns></returns>
        ICar CreateVan();
    }
    /// <summary>
    /// 具体工厂角色
    /// </summary>
    class BenzFactory : ICarFactory
    {
        public ICar CreateSnv() => new BenzSuv();

        public ICar CreateVan() => new BenzVan();
    }

    class BMWFactory : ICarFactory
    {
        public ICar CreateSnv() => new BMWSuv();

        public ICar CreateVan() => new BMWVan();
    }

    #endregion

    #region 建造者模式

    interface ICar2
    {
        /// <summary>
        /// 汽车车轮
        /// </summary>
        /// <returns></returns>
        public string GetWheel();
        /// <summary>
        /// 汽车引擎
        /// </summary>
        /// <returns></returns>
        public string GetEngine();
    }

    class Car : ICar2
    {
        public string Engine { get; }
        public string Wheel { get; }

        public Car(string engine, string wheel)
        {
            Engine = engine;
            Wheel = wheel;
        }

        public string GetWheel() => Wheel;

        public string GetEngine() => Engine;

        public override string ToString() => $"引擎是：{Engine}\n 车轮是:{Wheel}";
    }

    /// <summary>
    /// 汽车设计蓝图
    /// </summary>
    class Blueprint
    {
        public string Wheel { get; set; }

        public string Engine { get; set; }
    }

    abstract class CarBuilder
    {
        protected Blueprint Blueprint;
        public void ReceiveBlueprint(Blueprint blueprint) => Blueprint = blueprint;

        public ICar2 BuildCar() => new Car(BuildEngine(), BuildWheel());
        /// <summary>
        /// 构建轮子
        /// </summary>
        /// <returns></returns>
        public abstract string BuildWheel();
        /// <summary>
        /// 构建引擎
        /// </summary>
        /// <returns></returns>
        public abstract string BuildEngine();
    }

    class BMWBulider : CarBuilder
    {
        public override string BuildWheel() => Blueprint.Wheel;

        public override string BuildEngine() => Blueprint.Engine;
    }

    class BenzBuilder : CarBuilder
    {
        public override string BuildWheel() => Blueprint.Wheel;

        public override string BuildEngine() => Blueprint.Engine;
    }

    /// <summary>
    /// 导演类
    /// </summary>
    class CarDirector
    {
        private CarBuilder _benzBuilder = new BenzBuilder();
        private CarBuilder _bmwBuilder = new BMWBulider();

        /// <summary>
        /// 生产奔驰 SUV
        /// </summary>
        /// <returns></returns>
        public ICar2 CreateBenzSuv()
        {
            return CreateCar(_benzBuilder, "奔驰的引擎", "奔驰的轮胎");
        }

        /// <summary>
        /// 生产宝马商务车
        /// </summary>
        /// <returns></returns>
        public ICar2 CreateBMWVan()
        {
            return CreateCar(_bmwBuilder, "宝马的引擎", "宝马的轮胎");
        }

        /// <summary>
        /// 生产混合车型
        /// </summary>
        /// <returns></returns>
        public ICar2 CreateMixtureCar()
        {
            return CreateCar(_benzBuilder, "宝马的引擎", "奔驰的轮胎");
        }

        public ICar2 CreateCar(CarBuilder builder, string engine, string wheel)
        {
            var blueprint = new Blueprint();
            blueprint.Engine = engine;
            blueprint.Wheel = engine;
            builder.ReceiveBlueprint(blueprint);
            return builder.BuildCar();
        }
    }

    #endregion

    #endregion
}
