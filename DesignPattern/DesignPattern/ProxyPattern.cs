using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using Castle.DynamicProxy;
using Castle.DynamicProxy.Generators;

namespace DesignPattern.DesignPattern
{
    /// <summary>
    /// 代理模式
    /// Provide a surrogate or placeholder for another object to control access to it.
    /// 为其他对象提供一种代理以控制对这个对象的访问。
    /// 
    /// 代理模式分为普通代理、强制代理、动态代理。
    ///
    /// 代理模式一般由Subject 接口、RealSubject 实现类、Proxy 代理类组成。
    ///
    /// 强制代理指无法直接调用目标类方法，必须通过代理类调用目标类方法。
    ///
    /// 动态代理指通过反射来实现代理功能。
    /// 1. .net 提供了 RealProxy 实现动态代理功能。（必须通过继承的方式）
    /// 2. Castle 提供了一整套工具实现动态代理功能。
    /// 
    /// </summary>
    class ProxyPattern
    {
        /// <summary>
        /// 普通代理
        /// </summary>
        public void CommonProxy()
        {
            IGamePlayer proxy = new GamePlayerProxy("张三");
            proxy.Login("admin", "123456");
            proxy.KillBoss();
            proxy.Upgrade();
        }

        /// <summary>
        /// 强制代理
        /// </summary>
        public void ForceProxy()
        {
            IGamePlayerForForceProxy player = new GamePlayerForForceProxy("张三");
            var proxy = player.GetProxy();
            proxy.Login("admin", "123456");
            proxy.KillBoss();
            proxy.Upgrade();
        }

        /// <summary>
        /// 使用 Castle 实现动态代理
        /// 需要添加
        /// <AssemblyAttribute Include="System.Runtime.CompilerServices.InternalsVisibleToAttribute">
        /// <_Parameter1>DynamicProxyGenAssembly2</_Parameter1>
        /// </AssemblyAttribute>
        /// 从而使 Castle 可以访问内部类
        /// </summary>
        public void DynimicProxy()
        {
            var proxyGenerator = new ProxyGenerator();
            var proxy = proxyGenerator.CreateInterfaceProxyWithTarget<IGamePlayer>(new GamePlayer("张三"), ProxyGenerationOptions.Default,
                new GamePlayerInterceptor());
            proxy.Login("admin", "123456");
            proxy.KillBoss();
            proxy.Upgrade();
        }
    }

    #region 代理模式模板代码

    interface ISubject
    {
        public void Request();
    }

    class RealSubject : ISubject
    {
        /// <summary>
        /// 实现方法
        /// </summary>
        public void Request()
        {
            // 业务逻辑
        }
    }

    class Proxy : ISubject
    {
        private readonly ISubject _subject;

        public Proxy(ISubject subject)
        {
            _subject = subject;
        }

        /// <summary>
        /// 代理方法
        /// </summary>
        public void Request()
        {
            Before();
            _subject.Request();
            After();
        }

        /// <summary>
        /// 前置操作
        /// </summary>
        private void Before() { }

        /// <summary>
        /// 后置操作
        /// </summary>
        private void After() { }
    }
    #endregion

    #region 普通代理

    /// <summary>
    /// 定义游戏玩家，可以登录，打怪，和升级
    /// </summary>
    interface IGamePlayer
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        public void Login(string user, string password);

        /// <summary>
        /// 打怪
        /// </summary>
        public void KillBoss();

        /// <summary>
        /// 升级
        /// </summary>
        public void Upgrade();
    }

    class GamePlayer : IGamePlayer
    {
        private readonly IGamePlayer _gamePlayer;
        private readonly string _name;

        public GamePlayer(string name)
        {
            _name = name;
        }

        public GamePlayer(IGamePlayer gamePlayer, string name)
        {
            _gamePlayer = gamePlayer ?? throw new Exception("必须使用代理登录");
            _name = name;
        }

        public void Login(string user, string password)
        {
            Console.WriteLine($"账号为【{user}】的用户【{_name}】登录了！");
        }

        public void KillBoss()
        {
            Console.WriteLine($"【{_name}】正在打怪！");
        }

        public void Upgrade()
        {
            Console.WriteLine($"【{_name}】升级了");
        }
    }

    class GamePlayerProxy : IGamePlayer
    {
        private IGamePlayer _gamePlayer;
        public GamePlayerProxy(string name)
        {
            _gamePlayer = new GamePlayer(this, name);
        }


        public void Login(string user, string password)
        {
            _gamePlayer.Login(user, password);
        }

        public void KillBoss()
        {
            _gamePlayer.KillBoss();
        }

        public void Upgrade()
        {
            _gamePlayer.Upgrade();
        }
    }

    #endregion

    #region 强制代理，不能直接 new 对象进行使用，也不直接 new 代理类，而是通过对象生成代理类

    interface IGamePlayerForForceProxy
    {
        public void Login(string user, string password);

        public void KillBoss();

        public void Upgrade();

        /// <summary>
        /// 获取代理
        /// </summary>
        /// <returns></returns>
        public IGamePlayerForForceProxy GetProxy();
    }

    class GamePlayerForForceProxy : IGamePlayerForForceProxy
    {
        private readonly string _name;

        private IGamePlayerForForceProxy _proxy;

        public GamePlayerForForceProxy(string name)
        {
            _name = name;
        }

        public void Login(string user, string password)
        {
            if (IsProxy()) Console.WriteLine($"账号为【{user}】的用户【{_name}】登录了！");
            else Console.WriteLine("请使用代理访问！");

        }

        public void KillBoss()
        {
            if (IsProxy()) Console.WriteLine($"【{_name}】正在打怪！");
            else Console.WriteLine("请使用代理访问！");
        }

        public void Upgrade()
        {
            if (IsProxy()) Console.WriteLine($"【{_name}】升级了");
            else Console.WriteLine("请使用代理访问！");
        }

        public IGamePlayerForForceProxy GetProxy()
        {
            _proxy = new GamePlayerProxyForForceProxy(this);
            return _proxy;
        }

        private bool IsProxy() => _proxy != null;
    }

    class GamePlayerProxyForForceProxy : IGamePlayerForForceProxy
    {
        private readonly IGamePlayerForForceProxy _gamePlayer;

        public GamePlayerProxyForForceProxy(IGamePlayerForForceProxy gamePlayer)
        {
            _gamePlayer = gamePlayer;
        }

        public void Login(string user, string password)
        {
            _gamePlayer.Login(user, password);
        }

        public void KillBoss()
        {
            _gamePlayer.KillBoss();
        }

        public void Upgrade()
        {
            _gamePlayer.Upgrade();
        }

        public IGamePlayerForForceProxy GetProxy()
        {
            return this;
        }
    }

    #endregion

    #region 使用 Castle 实现动态代理

    class GamePlayerInterceptor : StandardInterceptor
    {
        protected override void PreProceed(IInvocation invocation)
        {
            if (invocation.MethodInvocationTarget.Name.Equals("Login"))
            {
                Console.WriteLine($"代理登录账号：【{invocation.Arguments[0]}】");
            }
            base.PreProceed(invocation);
        }

        protected override void PostProceed(IInvocation invocation)
        {
            base.PostProceed(invocation);

        }
    }

    #endregion

}
