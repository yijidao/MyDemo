using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.DesignPrinciples
{
    /// <summary>
    /// 单一职责原则的定义：应该有且仅有一个原因引起类的变更。
    /// SRP：There should never be more than one reason for a class to change.
    /// 所以一个接口应该只负责一件事。
    /// 可以对类和方法也使用单一职责，也就是一个方法，只负责一件事。
    /// 单一职责的好处：
    /// 1. 降低复杂度，实现什么职责都有明确的定义。
    /// 2. 简单，所以可读。
    /// 3. 简单，所以可维护。
    /// </summary>
    class SingleResponsibilityPrinciple
    {
        /// <summary>
        /// 单一职责的用法
        /// 面向接口编程，所以向外暴露的应该是接口
        /// </summary>
        public void UsingIUserInfo()
        {
            var userInfo = new UserInfo();
            // 当赋值时，就当作一个纯粹的 BO
            var userBo = (IUserBo) userInfo;
            userBo.UserName = "张三";
            // 当执行动作时，就当作一个业务逻辑类
            var userBiz = (IUserBiz) userInfo;
            userBiz.DeleteUser();
        }
    }

    // <summary>
    // 用户接口设计
    // 一个错误的接口设计，把用户的属性和行为糅杂在一起
    // 正确的设计应该是，用户属性一个接口，用户行为一个接口。好处就是，用户使用时，可以转型成不同的接口使用
    // </summary>
    //interface IUserInfo
    //{
    //    public string UserId { get; set; }

    //    public string Password { get; set; }

    //    public string UserName { get; set; }

    //    public bool ChangePassword(string password);

    //    public bool DeleteUser();

    //    public void MapUser();

    //    public bool AddOrg(int orgId);

    //    public bool AddRole(int orgId);
    //}


    /// <summary>
    /// 将用户的信息抽取成一个 BO(Business Object)
    /// 负责用户的属性
    /// </summary>
    interface IUserBo
    {
        public string UserId { get; set; }

        public string Password { get; set; }

        public string UserName { get; set; }
    }

    /// <summary>
    /// 将用户的行为抽取成一个 Biz（Business Logic）
    /// 负责用户的行为
    /// </summary>
    interface IUserBiz
    {
        public bool ChangePassword(string password);

        public bool DeleteUser();

        public void MapUser();

        public bool AddOrg(int orgId);

        public bool AddRole(int orgId);
    }
    /// <summary>
    /// 复合接口
    /// </summary>
    interface IUserInfo : IUserBo, IUserBiz
    {
        
    }

    public class UserInfo : IUserInfo
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public bool ChangePassword(string password)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUser()
        {
            throw new NotImplementedException();
        }

        public void MapUser()
        {
            throw new NotImplementedException();
        }

        public bool AddOrg(int orgId)
        {
            throw new NotImplementedException();
        }

        public bool AddRole(int orgId)
        {
            throw new NotImplementedException();
        }
    }

}
