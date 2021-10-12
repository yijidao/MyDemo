using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.Extensions.Caching.Memory;

namespace PrismAopModuleDemo
{
    public class CacheInterceptor : AsyncInterceptorBase
    {
        private readonly IMemoryCache _memoryCache;

        public CacheInterceptor(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        protected override async Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed)
        {
            await proceed(invocation, proceedInfo).ConfigureAwait(false);
        }

        protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
        {
            var attribute = invocation.Method.GetCustomAttribute<CacheAttribute>();
            if (attribute == null || _memoryCache == null)
            {
                return await proceed(invocation, proceedInfo).ConfigureAwait(false);
            }

            var cacheKey = attribute.CacheKey ?? GenerateKey(invocation);
            if (_memoryCache.TryGetValue(cacheKey, out TResult cacheValue))
            {
                Debug.WriteLine($"[Value from cache]  Key: {cacheKey}, Value: {cacheValue}");
                return cacheValue;
            }
            else
            {
                cacheValue = await proceed(invocation, proceedInfo).ConfigureAwait(false);
                _memoryCache.Set(cacheKey, cacheValue);
                return cacheValue;
            }
        }

        private string GenerateKey(IInvocation invocation)
        {
            var methodInfo = invocation.MethodInvocationTarget ?? invocation.Method;
            var className = (methodInfo.DeclaringType ?? this.GetType()).FullName;
            var methodName = methodInfo.Name;
            var arguments = string.Empty;
            var parameters = methodInfo.GetParameters();

            if (parameters.Any() && parameters.Length == invocation.Arguments.Length)
            {
                // 格式化 {name : value, name2 : value2 }
                arguments =
                    $"{{ {string.Join(", ", parameters.Select((arg, index) => this.FormatArgumentString(arg, invocation.Arguments[index])))} }}";
            }

            // Create a cache key using the retrieved info.
            return $"{className}.{methodName}({arguments})";
        }

        private string FormatArgumentString(ParameterInfo argument, object value)
        {
            // Convert value to string and remove line breaks.
            var stringValue = Convert.ToString(value)
                ?.Replace("\r", "\\r")
                .Replace("\n", "\\n");

            // Wrap value in quotes if it's a string
            var formatted = argument.ParameterType == typeof(string)
                ? string.Concat("\"", stringValue, "\"")
                : stringValue;

            return $"{argument.Name} : {formatted}";
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class CacheAttribute : Attribute
    {
        public string CacheKey { get; }
        public long Expiration { get; }

        public CacheAttribute(string cacheKey = null, long expiration = 0)
        {
            CacheKey = cacheKey;
            Expiration = expiration;
        }

        public override string ToString() => $"{{ CacheKey: {CacheKey ?? "null"}, Expiration: {Expiration} }}";
    }
}