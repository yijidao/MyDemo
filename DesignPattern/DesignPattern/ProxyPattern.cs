using System;
using System.Collections.Generic;
using System.Text;

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
    /// </summary>
    class ProxyPattern
    {
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


    
    #endregion
}
