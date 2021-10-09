using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using DryIoc;
using Prism.DryIoc;
using Prism.Ioc;

namespace PrismAopModuleDemo
{
    public class AsyncInterceptor<T> : AsyncDeterminationInterceptor where T : IAsyncInterceptor
    {
        public AsyncInterceptor(T asyncInterceptor) : base(asyncInterceptor) { }
    }

    public static class DryIocInterceptionAsyncExtension
    {
        public static void InterceptAsync<TService, TInterceptor>(this IRegistrator registrator, object serviceKey = null)
            where TInterceptor : class, IAsyncInterceptor
        {
            registrator.Register<AsyncInterceptor<TInterceptor>>();
            registrator.Intercept<TService, AsyncInterceptor<TInterceptor>>(serviceKey);
        }

        public static void InterceptAsync<TService, TInterceptor>(this IContainerRegistry containerRegistry)
            where TInterceptor : class, IAsyncInterceptor
        {
            var container = containerRegistry.GetContainer();
            container.InterceptAsync<TService, TInterceptor>();
        }
    }
}
