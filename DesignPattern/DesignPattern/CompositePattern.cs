using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.DesignPattern
{
    /// <summary>
    /// 组合模式
    /// Compose objects into tree structures to represent part-whole hierarchies.
    /// Composite lets clients treat individual objects and compositions of objects uniformly.
    /// 将对象组合成树状结构，用于表示局部-整体的层次。
    /// 组合模式让用户对单个对象和组合对象的使用具有一致性。
    ///
    /// 树状结构一般都要考虑怎么实现前序遍历、后序遍历、中序遍历。
    /// 
    /// 优点：
    /// - 调用简单，都是Component 对象。
    /// - 增加节点简单。
    /// 缺点：
    /// - 破坏了依赖导致原则。使用中调用的而一般都是实现类而非抽象类（查看display遍历方法）。
    ///
    /// 使用场景：
    /// 只要类之间的关系是树形结构，只要是需要体现局部和整体关系的，就考虑使用组合模式。
    /// 如树形菜单、文件和文件夹管理等。
    /// </summary>
    class CompositePattern
    {
        public void CompositeDemo()
        {
            // 根组件
            var root = new Composite();
            // 树枝组件
            var branch = new Composite();
            // 叶子组件
            var leaf = new Leaf();

            root.Add(branch);
            branch.Add(leaf);

            Display(root);
        }
        /// <summary>
        /// 遍历
        /// </summary>
        /// <param name="root"></param>
        public void Display(Composite root)
        {
            foreach (var child in root.GetChildren())
            {
                if (child is Leaf leaf)
                {
                    leaf.DoSomething();
                }
                else
                {
                    Display((Composite)child);
                }
            }
        }
    }

    #region 安全的组合模式模板代码
    /// <summary>
    /// 抽象组件角色
    /// 共有的行为和属性
    /// </summary>
    abstract class ComponentCp
    {
        public ComponentCp Parent { get; set; }

        /// <summary>
        /// 共用业务逻辑
        /// </summary>
        public virtual void DoSomething()
        {
            Console.WriteLine("分支组件");
        }

    }

    /// <summary>
    /// 树枝构件角色
    /// </summary>
    class Composite : ComponentCp
    {
        private readonly List<ComponentCp> _list = new List<ComponentCp>();

        public void Add(ComponentCp component)
        {
            component.Parent = this;
            _list.Add(component);
        }

        public void Remove(ComponentCp component)
        {
            _list.Remove(component);
        }

        public List<ComponentCp> GetChildren()
        {
            return _list;
        }
    }

    /// <summary>
    /// 叶子构件角色
    /// 是遍历的最小单位，其下再也没有其他分支
    /// </summary>
    class Leaf : ComponentCp
    {
        public override void DoSomething()
        {
            //base.DoSomething();
            Console.WriteLine("叶子组件");
        }
    }

    #endregion

    #region 透明的组合模式模板代码

    /// <summary>
    /// 透明模式就是指将分支类 Composite 的方法提升到基类中，这样叶子类 Leaf 也可以看到分支类的方法（但是叶子类不实现分支类方法）。
    /// </summary>
    abstract class TransparentComponentCp
    {
        public ComponentCp Parent { get; set; }

        /// <summary>
        /// 共用业务逻辑
        /// </summary>
        public virtual void DoSomething()
        {
            Console.WriteLine("分支组件");
        }

        public abstract void Add(TransparentComponentCp component);

        public abstract void Remove(TransparentComponentCp component);

        public abstract List<TransparentComponentCp> GetChildren();
    }

    #endregion
}
