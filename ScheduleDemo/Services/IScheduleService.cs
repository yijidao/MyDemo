using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
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


        public static async Task<string[]> MockService()
        {
            _result.Add(_result.Count.ToString());
            return await Task.FromResult(_result.ToArray());
        }
    }


    class ScheduleServiceTestClass
    {
        /// <summary>
        /// 定时轮询方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="getDataFunc"></param>
        /// <param name="compareDataFunc"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public static IObservable<T?> Demo2<T>(Func<Task<T>> getDataFunc, Func<T, T, T> compareDataFunc, TimeSpan period)
        {
            var current = default(T);
            return Observable.Timer(TimeSpan.FromSeconds(0), period)
                .SelectMany(async _ =>
                {
                    try
                    {
                        return await getDataFunc();
                    }
                    catch (Exception e)
                    {
                        // 日志处理
                        return default(T);
                    }
                })
                .Where(x => x != null)
                .Select(x =>
                {
                    //Debug.WriteLine("emit");
                    if (current == null)
                    {
                        current = x;
                        return x;
                    }
                    var increment = compareDataFunc(current, x);
                    current = x;
                    return increment;
                });
        }

        private static ReplaySubject<string[]?>? _subject;

        /// <summary>
        /// 主题订阅，如客流等。
        /// </summary>
        /// <returns></returns>
        public static IObservable<string[]?> MockSubject1()
        {
            if (_subject == null)
            {
                _subject = new ReplaySubject<string[]?>();
                var o = Demo2(MockServiceClass.MockService,
                   (oldValue, newValue) => newValue.Except(oldValue).ToArray(),
                   TimeSpan.FromSeconds(2))
                    .Subscribe(_subject);

            }

            return _subject.AsObservable();
        }

        private static IObservable<string[]?>? _observable = null;

        /// <summary>
        /// 主题订阅，如客流等。
        /// 跟 <see cref="MockSubject1"/> 的不同点是使用 rx 提供的 api，并且支持了连接数为 0 时，自动取消订阅。
        /// </summary>
        /// <returns></returns>
        public static IObservable<string[]?> MockSubject2()
        {
            return _observable ??= Demo2(MockServiceClass.MockService,
                    (oldValue, newValue) => newValue.Except(oldValue).ToArray(),
                    TimeSpan.FromSeconds(2))
                    .Replay()
                .RefCount();
        }
    }


}
