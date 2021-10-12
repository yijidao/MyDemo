using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Castle.DynamicProxy;

namespace PrismAopModuleDemo
{
    public class LogInterceptor : AsyncInterceptorBase
    {
        private readonly ILogger _logger;

        public LogInterceptor(ILogger logger)
        {
            _logger = logger;
        }

        protected override async Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed)
        {
            try
            {
                await proceed(invocation, proceedInfo).ConfigureAwait(false);
                _logger.Info(GetLog(invocation));
            }
            catch (Exception e)
            {
                _logger.Error(GetLog(invocation), e);
                throw;
            }
        }

        protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
        {
            try
            {
                var result = await proceed(invocation, proceedInfo).ConfigureAwait(false);
                _logger.Info(GetLog(invocation, result));
                return result;
            }
            catch (Exception e)
            {
                _logger.Error(GetLog(invocation), e);
                throw;
            }
        }

        private string GetLog(IInvocation invocation, object returnValue = null)
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
            var result = $"{className}.{methodName}({arguments})";
            if (returnValue != null)
            {
                result += $"[Return] {returnValue}";
            }
           
            return result;
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
}
