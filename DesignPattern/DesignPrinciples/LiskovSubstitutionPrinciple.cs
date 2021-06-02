using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.DesignPrinciples
{
    /// <summary>
    /// 里氏替换原则
    /// 定义：所有能使用父类的地方，就能使用子类，而且替换成子类不会有任何一次，使用者可能根本不需要知道是父类还是子类。
    ///       但是，反之则不成立，使用子类的地方，不一定能使用父类。
    /// OOP 有继承，继承的好处就是复用代码，并且可以实现多态。
    /// 继承的坏处就是子类必须有父类的属性和行为，并且修改父类时，可能会影响到子类。
    /// 所以里氏替换原则，其实是用来思考如何设计父类和子类的。
    ///
    /// 当子类继承父类时，会异化父类的属性或者方法（就是说在这个子类里，父类只有部分属性或者行为有意义），那么就不应该使用继承，
    /// 因为这样继承的话，在使用父类的地方使用了这个子类，就有隐藏的bug可能。此时应该使用组合、聚合来使用父类的部分行为或者属性。
    /// </summary>
    class LiskovSubstitutionPrinciple
    {
        public void SoldierDemo()
        {
            var soldier = new Soldier
            {
                Gun = new Handgun()
            };

            var soldier2 = new Soldier
            {
                Gun = new Rifle()
            };

            var soldier3 = new Soldier
            {
                Gun = new MachineGun()
            };

            // 加入 ToyGun 也继承 AbstractGun，但是玩具枪又不能射击，那么就只能对 shoot 提供一个空实现，
            // 那么就不符合里氏替换，在现实的业务场景中，可能会有bug。
            //var soldier4 = new Soldier
            //{
            //    Gun = new ToyGun()
            //};

            soldier.KillEnemy();
            soldier2.KillEnemy();
            soldier3.KillEnemy();

        }

    }

    abstract class AbstractGun
    {
        /// <summary>
        /// 射击
        /// </summary>
        public abstract void Shoot();

        /// <summary>
        /// 外观
        /// </summary>
        public abstract void Sharp();
    }

    class Handgun : AbstractGun
    {
        public override void Shoot()
        {
            Console.WriteLine("手枪射击...");
        }

        public override void Sharp()
        {
            Console.WriteLine("手枪");
        }
    }

    class Rifle : AbstractGun
    {
        public override void Shoot()
        {
            Console.WriteLine("步枪射击...");
        }

        public override void Sharp()
        {
            Console.WriteLine("步枪");
        }
    }

    class MachineGun : AbstractGun
    {
        public override void Shoot()
        {
            Console.WriteLine("机枪射击...");
        }

        public override void Sharp()
        {
            Console.WriteLine("机枪");
        }
    }

    /// <summary>
    /// 玩具枪抽象类
    /// 玩具枪不能射击，但是可以有枪的外观
    /// 当子类会异化父类的方法时，可以不使用继承，而是使用组合、聚合。
    /// </summary>
    abstract class AbstractToy
    {
        public AbstractGun Gun { get; set; }

        public void Sharp() => Gun.Sharp();
    }

    /// <summary>
    /// 玩具枪
    /// </summary>
    class ToyGun : AbstractToy
    {
        
    }

    class Soldier
    {
        /// <summary>
        /// 士兵的枪必须可以射击，才能杀人
        /// </summary>
        public AbstractGun Gun { get; set; }

        public void KillEnemy()
        {
            Console.WriteLine("士兵开始杀人...");
            Gun.Shoot();
        }
    }
}
