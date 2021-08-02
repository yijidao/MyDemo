using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;

namespace DesignPattern.AOP
{
    /// <summary>
    /// Castle.Core 中提供了动态代理的功能，但是只提供了对同步方法的代理，异步方法需要自己实现。
    /// 异步方法如果只是返回 Task，则可以通过使用 Task.ContinueWith 来封装一下，但是如果返回 Task<TResult> 自己实现则很费劲。
    /// https://github.com/JSkimming/Castle.Core.AsyncInterceptor 提供了异步方法的代理实现，并且通过桥梁模式连接到了 Castle.Core 的实现中。
    /// 这个类写了一些 AOP 的 Demo。
    ///
    /// https://github.com/castleproject/Core/tree/master/docs 部分文档
    ///
    /// Castle.Core 中动态代理的类型：
    /// ## 基于继承
    /// - 类代理。创建一个被代理类的子类，并且代理其中的虚方法
    /// ## 基于组合
    /// - 有目标对象的类代理。一般不用。
    /// - 没有目标对象的接口代理。需要一个接口的所有方法实现。
    /// - 有目标对象的接口代理。目标对象必须实现被代理的接口。
    /// - 有目标接口的接口代理。常用于多个接口混合的情况下。
    /// 
    /// </summary>
    public class TimingDemo
    {
        /// <summary>
        /// 拦截异步方法
        /// </summary>
        /// <returns></returns>
        public async Task Test()
        {
            var generator = new ProxyGenerator();
            var impl = new TestServiceImpl();
            var proxy = generator.CreateInterfaceProxyWithTarget<ITestService>(impl, new TestAsyncTimingInterceptor().ToInterceptor());
            var result = await proxy.GetData("1,2,3,4,5,6");
            Console.WriteLine(string.Join('-', result));

        }

        /// <summary>
        /// 拦截异步方法
        /// 实例是从 Autofac 中获取的
        /// Autofac 集成了 Castle.Core 但是没有集成 Castle.Core.IAsyncInterceptor，所以使用的接口是 IInterceptor 而不是 IAsyncInterceptor
        /// 所以需要做一些转换，这个 Demo 提供了适配器来转换
        /// 具体参考 https://github.com/JSkimming/Castle.Core.AsyncInterceptor/issues/42
        /// </summary>
        /// <returns></returns>
        public async Task Test2()
        {
            var builder = new ContainerBuilder();
            builder.RegisterGeneric(typeof(AsyncInterceptorAdaper<>));
            builder.RegisterType<TestAsyncTimingInterceptor>().As<IAsyncInterceptor>();
            builder.RegisterType<TestServiceImpl>().As<ITestService>().EnableInterfaceInterceptors();
            var container = builder.Build();
            var test = container.Resolve<ITestService>();
            var result = await test.GetData("1,2,3,4,5");
            Console.WriteLine(string.Join('-', result));

            var result2 = await test.GetData2();
            Console.WriteLine(result2);
        }

        /// <summary>
        /// 拦截异步方法
        /// 使用 Hook 进行粒度控制
        /// </summary>
        /// <returns></returns>
        public async Task Test3()
        {
            var builder = new ContainerBuilder();
            builder.RegisterGeneric(typeof(AsyncInterceptorAdaper<>));
            builder.RegisterType<TestAsyncTimingInterceptor>().As<IAsyncInterceptor>();
            builder.RegisterType<TestServiceImpl>().As<ITestService>().EnableInterfaceInterceptors(new ProxyGenerationOptions(new TestInterceptorHook()));
            var container = builder.Build();
            var test = container.Resolve<ITestService>();
            var result = await test.GetData("1,2,3,4,5");
            Console.WriteLine(string.Join('-', result));

            var result2 = await test.GetData2();
            Console.WriteLine(result2);
        }
    }

    [Intercept(typeof(AsyncInterceptorAdaper<TestAsyncTimingInterceptor>))]
    public interface ITestService
    {
        public Task<string[]> GetData(string condition);

        public Task<string> GetData2();
    }

    public class TestServiceImpl : ITestService
    {
        public Task<string[]> GetData(string condition)
        {
            return Task.Run(async () =>
            {
                await Task.Delay(500);
                return condition.Split(',');
            });
        }

        public Task<string> GetData2()
        {
            return Task.Run(async () =>
            {
                await Task.Delay(500);
                return nameof(GetData2);
            });
        }
    }

    public class TestAsyncTimingInterceptor : AsyncTimingInterceptor
    {
        protected override void StartingTiming(IInvocation invocation)
        {
            Console.WriteLine($"{invocation.Method.Name}:StartingTiming");
        }

        protected override void CompletedTiming(IInvocation invocation, Stopwatch stopwatch)
        {
            Console.WriteLine($"{invocation.Method.Name}:CompletedTiming:{stopwatch.Elapsed:g}");
        }
    }

    /// <summary>
    /// 用来适配 IAsyncInterceptor 和 IInterceptor
    /// </summary>
    /// <typeparam name="TAsyncInterceptor"></typeparam>
    public class AsyncInterceptorAdaper<TAsyncInterceptor> : AsyncDeterminationInterceptor where TAsyncInterceptor : IAsyncInterceptor
    {
        public AsyncInterceptorAdaper(IAsyncInterceptor asyncInterceptor) : base(asyncInterceptor)
        {
        }
    }

    public class TestInterceptorHook : IProxyGenerationHook
    {
        public void MethodsInspected()
        {
        }

        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
        }

        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            return methodInfo.Name != nameof(ITestService.GetData2);
        }
    }
}
