using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using DryIoc;
using ImTools;
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
        private static readonly DefaultProxyBuilder _proxyBuilder = new DefaultProxyBuilder();

        public static void Intercept<TService, TInterceptor>(this IRegistrator registrator, object serviceKey = null)
            where TInterceptor : class, IInterceptor
        {
            var serviceType = typeof(TService);

            Type proxyType;
            if (serviceType.IsInterface())
                proxyType = _proxyBuilder.CreateInterfaceProxyTypeWithTargetInterface(
                    serviceType, ArrayTools.Empty<Type>(), ProxyGenerationOptions.Default);
            else if (serviceType.IsClass())
                proxyType = _proxyBuilder.CreateClassProxyTypeWithTarget(
                    serviceType, ArrayTools.Empty<Type>(), ProxyGenerationOptions.Default);
            else
                throw new ArgumentException(
                    $"Intercepted service type {serviceType} is not a supported, cause it is nor a class nor an interface");

            registrator.Register(serviceType, proxyType,
                made: Made.Of(pt => pt.PublicConstructors().FindFirst(ctor => ctor.GetParameters().Length != 0),
                    Parameters.Of.Type<IInterceptor[]>(typeof(TInterceptor[]))),
                setup: Setup.DecoratorOf(useDecorateeReuse: true, decorateeServiceKey: serviceKey));
        }

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
