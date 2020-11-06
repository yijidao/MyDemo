using System;
using Unity;
using Unity.Injection;

namespace MyUnityDemo
{
    /// <summary>
    /// UnityContainer 的demo
    /// 参考文档：https://www.tutorialsteacher.com/ioc/register-and-resolve-in-unity-container
    /// https://docs.microsoft.com/en-us/previous-versions/msp-n-p/dn178462(v=pandp.30)#without-the-event-broker-container-extension
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Log(Resolve);
            Log(MultipleRegistration);
            Log(RegisterNamedType);
            Log(RegisterInstance);
            Log(MultipleParameters);
            Log(MultipleConstructors);
            Log(PrimitiveTypeParameter);
        }

        static void Log(Action action)
        {
            Console.WriteLine($"------ 调用方法 {action.Method.Name} ------");
            Console.WriteLine();
            action.Invoke();
            Console.WriteLine();
        }

        static void Resolve()
        {
            var container = new UnityContainer();
            container.RegisterType<ICar, BMW>();
            var driver = container.Resolve<Driver>();
            driver.RunCar();
            // 默认的生命周期是瞬态的
            var driver2 = container.Resolve<Driver>();
            driver2.RunCar();
        }

        static void MultipleRegistration()
        {
            var container = new UnityContainer();
            container.RegisterType<ICar, BMW>();
            container.RegisterType<ICar, Ford>(); // 同一接口注册多个实例，则保存最后注册的实例
            container.Resolve<Driver>().RunCar();
        }

        static void RegisterNamedType()
        {
            var container = new UnityContainer();
            container.RegisterType<ICar, BMW>();
            container.RegisterType<ICar, Ford>("ford");
            //手动给构造器参数赋值
            container.RegisterType<Driver>("ford driver", new InjectionConstructor(container.Resolve<ICar>("ford")));
            container.Resolve<Driver>().RunCar();
            container.Resolve<Driver>("ford driver").RunCar();
        }

        static void RegisterInstance()
        {
            var container = new UnityContainer();
            container.RegisterInstance<ICar>(new Audi()); // 直接注册实例，这时候是单例的，不是瞬态的
            container.Resolve<Driver>().RunCar();
            container.Resolve<Driver>().RunCar();
        }

        static void MultipleParameters()
        {
            var container = new UnityContainer(); // 构造函数有多个参数，默认选能满足最多参数的构造函数
            container.RegisterType<ICar, Audi>();
            container.RegisterType<ICarKey, AudiKey>();
            container.Resolve<Driver>().RunCar();
        }

        static void MultipleConstructors()
        {
            var container = new UnityContainer();
            container.RegisterType<ICar, Audi>();
            container.RegisterType<ICarKey, AudiKey>();

            // 当类有多个构造函数，可以在构造函数上使用特性 [InjectionConstructor] 标记来指定使用某个构造函数进行初始化
            // 也可以在注册时使用 new InjectionConstructor(container.Resolve<ICar>()) 手动进行配置，指定某个构造函数进行初始化
            container.RegisterType<Driver>(new InjectionConstructor(container.Resolve<ICar>()));
            container.Resolve<Driver>().RunCar();
        }

        /// <summary>
        /// 构造函数的参数是简单类型的
        /// </summary>
        static void PrimitiveTypeParameter()
        {
            var container = new UnityContainer();
            container.RegisterType<ICar, Ford>();
            // 手动使用 InjectionConstructor 进行配置，给构造函数传简单类型的参数
            container.RegisterType<Driver>(new InjectionConstructor(new object[] { container.Resolve<ICar>(), "Jonathon" }));
            container.Resolve<Driver>().RunCar();
        }
    }
}
