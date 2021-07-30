using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;

namespace DesignPattern.AOP
{
    public class TimingDemo
    {
        public async Task Test()
        {
            //var generator = new ProxyGenerator();
            //var impl = new TestServiceImpl();
            //var proxy = generator.CreateInterfaceProxyWithTarget<ITestService>(impl, new TestAsyncTimingInterceptor().ToInterceptor());
            //var result= await proxy.GetDatas("123456");

            var builder = new ContainerBuilder();
            builder.RegisterType<TestServiceImpl>().As<ITestService>().EnableInterfaceInterceptors();
            builder.Register(c => new TestAsyncTimingInterceptor());
            var container = builder.Build();
            var test = container.Resolve<ITestService>();
            var result = await test.GetDatas("12345");

            //new TestAsyncTimingInterceptor().ToInterceptor()
        }
    }

    [Intercept(typeof(TestAsyncTimingInterceptor))]
    public interface ITestService
    {
        public Task<string[]> GetDatas(string condition);
    }

    public class TestServiceImpl : ITestService
    {
        public Task<string[]> GetDatas(string condition)
        {
            return Task.Run(() => condition.Split(','));
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
}
