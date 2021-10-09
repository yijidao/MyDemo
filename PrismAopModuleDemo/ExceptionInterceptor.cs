using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace PrismAopModuleDemo
{
    public class ExceptionInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"[Exception]  {invocation.Method.Name}");
                //throw;
            }
        }
    }
}
