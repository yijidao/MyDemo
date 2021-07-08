using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Reflection;
using System.Text;
using DesignPattern.Extensions;

namespace DesignPattern.DesignPattern
{
    /// <summary>
    /// 备忘录模式
    /// Without violating encapsulation, capture and externalize an object's internal state so that the object can be restored to this state later.
    /// 在不破坏封装性的前提下，捕获对象的内部状态，并且将这个状态保存在该对象之外，这样以后就可以将该对象恢复到这个状态。
    ///
    /// 由三个角色组成：
    /// - Originator
    /// 发起人角色。记录当前时刻的内部状态，负责定义哪些状态需要备份，负责创建和恢复备忘录数据。
    /// - Memento
    /// 备忘录角色。负责存储 Originator 对象的内部状态，在需要的时候，提供 Originator 所需的内部状态。
    /// - Caretaker
    /// 备忘录管理员角色。对备忘录角色进行管理、保存和提供。
    ///
    /// 场景：
    /// - 需要保存和恢复数据相关状态的场景。
    /// - 需要提供一个回退操作的场景。
    /// - 数据库ADO的事务回滚操作。
    /// - 需要监控的副本场景。
    ///
    /// 注意事项：
    /// - 备份不需要了，就要把引用释放掉。
    /// - 备份有可能很耗资源。所以不要在循环中创建备份。
    ///
    /// 备份的方式：
    /// - 原型复制
    /// - 反射后将字段和属性储存到字典或者临时表
    /// - 序列化后保存到内存等
    ///
    /// 恢复的方式：
    /// - 手动赋值
    /// - 反射后赋值
    /// - 如果使用字典或者临时表存储备份，因为字典和临时表已经维护了字段和值的关系，则可以利用这个关系进行恢复
    /// </summary>
    class MementoPattern
    {
        public void MementoDemo()
        {
            var originator = new Originator { State = "s1" };

            var caretaker = new Caretaker(originator);
            caretaker.CreateMemento();

            originator.State = "s2";
            Console.WriteLine($"当前状态：{originator.State}");
            Console.WriteLine("------  恢复上一个状态  ------");
            caretaker.RestoreMemento();
            Console.WriteLine($"当前状态：{originator.State}");
        }

        public void MementoDemo2()
        {
            var originator2 = new Originator2();
            originator2.State = "s1";
            originator2.CreateMemento();

            originator2.State = "s2";
            Console.WriteLine($"当前状态：{originator2.State}");
            Console.WriteLine("------  恢复上一个状态  ------");
            originator2.RestoreMemento();
            Console.WriteLine($"当前状态：{originator2.State}");
        }

        public void MementoDemo3()
        {
            var originator = new Originator();
            var caretaker = new Caretaker2(originator);
            originator.State = "s11";
            originator.State2 = "s21";
            originator.State3 = "s31";
            Console.WriteLine($"当前状态：【{originator.State}】【{originator.State2}】【{originator.State3}】");
            caretaker.CreateMemento();
            originator.State = "s12";
            originator.State2 = "s22";
            originator.State3 = "s32";
            Console.WriteLine($"当前状态：【{originator.State}】【{originator.State2}】【{originator.State3}】");

            Console.WriteLine("------  恢复上一个状态  ------");
            caretaker.RestoreMemento();
            Console.WriteLine($"当前状态：【{originator.State}】【{originator.State2}】【{originator.State3}】");

        }

    }

    #region 备忘录模式经典实现
    /// <summary>
    /// 备忘录角色
    /// 负责存储 Originator 对象的内部状态，在需要的时候，提供 Originator 所需的内部状态。
    /// </summary>
    class Memento
    {
        public string State { get; }
        public Memento(string state)
        {
            State = state;
        }
    }
    /// <summary>
    /// 备忘录管理员角色
    /// 对备忘录角色进行管理、保存和提供。
    /// </summary>
    class Caretaker
    {
        public Originator Originator { get; }
        public Memento Memento { get; set; }

        public Caretaker(Originator originator)
        {
            Originator = originator;
        }

        public void CreateMemento()
        {
            Memento = new Memento(Originator.State);
        }


        public void RestoreMemento()
        {
            Originator.State = Memento.State;
        }
    }

    /// <summary>
    /// 备忘录管理员角色
    /// 对备忘录角色进行管理、保存和提供。
    /// 因为手动赋值麻烦而且易出错，所以使用原型模式+反射进行多字段自动备份。
    /// </summary>
    class Caretaker2
    {
        public Originator Originator { get; }
        public Caretaker2(Originator originator)
        {
            Originator = originator;
        }

        private Originator _menento;

        public void CreateMemento()
        {
            _menento = Originator.Copy();
        }

        public void RestoreMemento()
        {
            var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public |
                               BindingFlags.FlattenHierarchy;

            var type = Originator.GetType();

            foreach (var fieldInfo in type.GetFields(bindingFlags))
            {
                fieldInfo.SetValue(Originator, fieldInfo.GetValue(_menento));
            }
        }
    }

    /// <summary>
    /// 发起人角色
    /// 记录当前时刻的内部状态，负责定义哪些状态需要备份，负责创建和恢复备忘录数据。
    /// </summary>
    class Originator
    {
        public string State { get; set; }
        public string State2 { get; set; }
        public string State3 { get; set; }

        public void DoSomething()
        {
            
        }
    }

    #endregion

    #region 备忘录模式+原型模式+扩展方法实现

    /// <summary>
    /// 将 Memento 角色和 Caretaker 角色全部融入到 Originator 角色中
    /// 这里备份的方法使用原型模式，也可以使用序列化到内存、临时储存到数据库中的方式。
    /// </summary>
    class Originator2
    {
        private Originator2 _backup;

        public string State { get; set; }

        public void CreateMemento()
        {
            _backup = this.Copy();
        }

        public void RestoreMemento()
        {
            State = _backup.State;
        }
    }

    #endregion

    #region 备忘录模式多状态备份



    #endregion
}
