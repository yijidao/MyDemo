using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace PrismAopModuleDemo
{
    public class AsyncMethodLogInterceptor : ProcessingAsyncInterceptor<string>
    {
        protected override string StartingInvocation(IInvocation invocation)
        {
            
            var msg = $"[AsyncLog]  {invocation.Method.Name}  start...";
            Debug.WriteLine(msg);
            return msg;
        }

        protected override void CompletedInvocation(IInvocation invocation, string state)
        {
            var msg = $"[AsyncLog]  {invocation.Method.Name}  end...";
            Debug.WriteLine(msg);
        }
    }
}
