using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Accessibility;
using Castle.DynamicProxy;
using DryIoc;
using Prism.Ioc;

namespace ScheduleDemo.Services
{
    public interface IScheduleService
    {
        IScheduleService Invoke<T>(string method, params object[] param);

        IScheduleService Timer(TimeSpan dueTime, TimeSpan period);

        


        //IScheduleService Subscribe();




        void Unsubscribe();

    }

    //public class ScheduleService : IScheduleService
    //{

    //    public string[]? KeyProperties { get; set; }

    //    public Type? InterfaceType { get; set; }

    //    public MethodInfo? Method { get; set; }

    //    public object[]? MethodParams { get; set; }



    //    public IScheduleService Invoke<T>(string method, params object[] methodParams)
    //    {
    //        var instance = ContainerLocator.Container.Resolve(typeof(T));

    //        InterfaceType = typeof(T);
    //        Method = typeof(T).GetMethod(method);
    //        MethodParams = methodParams;

    //        return this;
    //    }


    //    public ScheduleService()
    //    {
            
    //    }


    //    public void Unsubscribe()
    //    {
    //    }
    //}



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

    class MockServiceClass
    {
        private static readonly List<string> _result = new();


        public static async Task<string[]>  MockService()
        {
            _result.Add(_result.Count.ToString());
            return await Task.FromResult(_result.ToArray());
        }
    }


    class ScheduleServiceTestClass
    {
        public IObservable<string[]> Demo1()
        {
            //var service = new ScheduleService();
            //service.Invoke<ITestService>(nameof(ITestService.T1));
            //service.Invoke<ITestService>(x => x.T1());

            //ContainerLocator.Container.Resolve<IScheduleService>()
            //    .Invoke<ITestService>();

            //ProxyUtil.CreateDelegateToMixin()

            var current = Array.Empty<string>();

            return Observable.Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(2))
                .SelectMany(async _ => await MockServiceClass.MockService())
                
                .Select(x =>
                {
                    var increment = x.Except(current);
                    current = x;
                    return increment.ToArray();
                });
        }


        public static IObservable<T> Demo2<T>(Func<Task<T>> func, Func<T, T, T> func2, TimeSpan period)
        {
            var current = default(T);
            return Observable.Timer(TimeSpan.FromSeconds(0), period)
                .SelectMany(async _ => await func())
                .Select(x =>
                {
                    //Debug.WriteLine("emit");
                    if (current == null)
                    {
                        current = x;
                        return x;
                    }
                    var increment = func2(current, x);
                    current = x;
                    return increment;
                });

        }
    }


}
