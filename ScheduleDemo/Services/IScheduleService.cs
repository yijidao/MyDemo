using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Castle.DynamicProxy;
using DryIoc;
using Prism.Ioc;

namespace ScheduleDemo.Services
{
    public interface IScheduleService
    {
        //T Subscribe<T>() where T : class;


        //void Subscribe<T>(Action<T> action);

        IScheduleService Subscribe<T>(string method, params object[] param);


        void Unsubscribe();

    }

    public class ScheduleService : IScheduleService
    {

        public string[]? KeyProperties { get; set; }

        public Type? InterfaceType { get; set; }

        public MethodInfo? Method { get; set; }

        public object[]? MethodParams { get; set; }

        public IScheduleService Subscribe<T>(string method, params object[] methodParams)
        {
            var instance = ContainerLocator.Container.Resolve(typeof(T));

            InterfaceType = typeof(T);
            Method = typeof(T).GetMethod(method);
            MethodParams = methodParams;

            return this;
        }

        public IScheduleService SetKeyProperties(params string[] keyProperties)
        {
            KeyProperties = keyProperties;
            return this;
        }

        public ScheduleService()
        {
            
        }


        public void Unsubscribe()
        {
        }
    }



    class ScheduleInterceptor : IInterceptor
    {
        public string[]? KeyProperty { get; set; }
        public Type? TargetType { get; set; }
        public MethodInfo? Method { get; set; }
        public object[]? Arguments { get; set; }

        public void Intercept(IInvocation invocation)
        {
            TargetType = invocation.TargetType;
            Method = invocation.Method;
            Arguments = invocation.Arguments;
        }


    }

    static class ScheduleInterceptorExtension
    {
        public static ScheduleInterceptor Key(this ScheduleInterceptor interceptor, params string[]? properties)
        {
            interceptor.KeyProperty = properties;
            return interceptor;
        }
    }

    public interface ITestService
    {
        void T1();
        void T2();
    }

    public class TestService : ITestService
    {
        public void T1()
        {
        }

        public void T2()
        {
        }
    }


    class ScheduleServiceTestClass
    {
        public void Demo1()
        {
            var service = new ScheduleService();
            service.Subscribe<ITestService>(nameof(ITestService.T1));
            //service.Subscribe<ITestService>(x => x.T1());

            //ContainerLocator.Container.Resolve<IScheduleService>()
            //    .Subscribe<ITestService>();

            //ProxyUtil.CreateDelegateToMixin()


        }
    }


}
