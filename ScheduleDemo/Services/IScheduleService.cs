using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Castle.DynamicProxy;
using DryIoc;
using Prism.Ioc;

namespace ScheduleDemo.Services
{
    internal interface IScheduleService
    {
        T Subscribe<T>() where T : class;

        void Unsubscribe();

    }

    public class ScheduleService : IScheduleService
    {
        public T Subscribe<T>() where T : class
        {
            
            var generator = new ProxyGenerator();
            var proxy = generator.CreateInterfaceProxyWithTargetInterface<T>(ContainerLocator.Container.Resolve<T>(), ProxyGenerationOptions.Default, ScheduleInterceptor.GetSingleton());
            return proxy;
        }

        public void Unsubscribe()
        {
            throw new NotImplementedException();
        }
    }

    class ScheduleInterceptor : StandardInterceptor
    {
        private static readonly Lazy<ScheduleInterceptor> Singleton = new(LazyThreadSafetyMode.ExecutionAndPublication);

        public static ScheduleInterceptor GetSingleton() => Singleton.Value;

        private ScheduleInterceptor()
        {
            
        }

        public void SetKey()
        {

        }

        protected override void PreProceed(IInvocation invocation)
        {
            base.PreProceed(invocation);
        }
    }

    static class ScheduleInterceptorExtension {
        public static ScheduleInterceptor Key(this ScheduleInterceptor interceptor, params string[] propertys)
        {

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
        void Demo1()
        {
            



        }



    }


}
