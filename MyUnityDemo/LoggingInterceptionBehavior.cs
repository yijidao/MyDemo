using System;
using System.Collections.Generic;
using System.Text;
using Unity.Interception.InterceptionBehaviors;
using Unity.Interception.PolicyInjection.Pipeline;

namespace MyUnityDemo
{
    /// <summary>
    /// 实现接口并实现三个方法。
    /// unity实现切面的方式是通过生成代理类，然后在行为管道中(Behaviour pipeline) 不停地调用 Invoke。
    ///
    /// </summary>
    public class LoggingInterceptionBehavior : IInterceptionBehavior
    {
        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            // 前切
            Console.WriteLine($"Invoking method {input.MethodBase.Name} ar {DateTime.Now.ToLongTimeString()}");

            // 执行方法
            var result = getNext()(input, getNext);

            // 后切，有异常时打日志
            if (result.Exception != null)
            {
                Console.WriteLine($"Method {input.MethodBase.Name} threw exception {result.Exception.Message} at {DateTime.Now.ToLongTimeString()}");
            }
            else
            {
                Console.WriteLine($"Method {input.MethodBase} return {result.ReturnValue} at {DateTime.Now.ToLongTimeString()}");
            }

            return result;

        }

        /// <summary>
        /// GetRequiredInterfaces方法使您可以指定要与行为关联的接口类型
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        /// <summary>
        /// WillExecute方法，可以通过指定此行为是否应当执行来优化您的行为链;
        /// 一般为 true，也就是始终执行。
        /// </summary>
        public bool WillExecute => true;
    }
}
