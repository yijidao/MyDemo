using System;
using Unity;
using Unity.Injection;
using Unity.Interception;
using Unity.Interception.ContainerIntegration;
using Unity.Interception.Interceptors.InstanceInterceptors.InterfaceInterception;
using Unity.Interception.Interceptors.TypeInterceptors.VirtualMethodInterception;
using Unity.Resolution;

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
            Log(PropertyInjection);
            Log(PropertyInjectionNamedMapping);
            Log(PropertyInjectionRuntime);
            Log(MethodInjectionAttribute);
            Log(MethodInjectionRuntime);
            Log(ParameterOverride);
            Log(MultipleParameterOverride);
            Log(PropertyOverride);
            Log(DependencyOverride);
            Log(InterfaceInterception);
            Log(VirtualMethodInterception);
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

        /// <summary>
        /// 不通过构造函数，而是通过属性注入
        /// </summary>
        static void PropertyInjection()
        {
            var container = new UnityContainer();
            container.RegisterType<ICar, Audi>();
            container.Resolve<Driver2>().RunCar();
        }

        /// <summary>
        /// 不通过构造函数，而是通过带标识符的属性注入
        /// </summary>
        static void PropertyInjectionNamedMapping()
        {
            var container = new UnityContainer();
            container.RegisterType<ICar, Audi>("audi");
            container.RegisterType<ICar, BMW>();
            container.Resolve<Driver3>().RunCar();
        }

        /// <summary>
        /// 不通过[Dependency] 特性，而是手动注入属性
        /// </summary>
        static void PropertyInjectionRuntime()
        {
            var container = new UnityContainer();
            container.RegisterType<Driver4>(new InjectionProperty(nameof(Driver4.Car), new Ford()));
            container.Resolve<Driver4>().RunCar();
        }

        /// <summary>
        /// 使用[InjectionMethod] 进行方法参数注入
        /// </summary>
        static void MethodInjectionAttribute()
        {
            var container = new UnityContainer();
            container.RegisterType<ICar, Ford>();
            container.Resolve<Driver5>().RunCar();
        }

        /// <summary>
        /// 手动进行方法参数注入
        /// </summary>
        static void MethodInjectionRuntime()
        {
            var container = new UnityContainer();
            container.AddNewExtension<Diagnostic>();
            container.RegisterType<ICar, Ford>();
            container.RegisterType<Driver5>(new InjectionMethod(nameof(Driver5.UseCar2), container.Resolve<ICar>()));
            container.Resolve<Driver5>().RunCar();
        }

        /// <summary>
        /// 构造函数参数覆盖
        /// </summary>
        static void ParameterOverride()
        {
            var container = new UnityContainer();
            container.RegisterType<ICar, Audi>();
            container.Resolve<Driver>(new ParameterOverride("car", new Ford())).RunCar();
        }

        /// <summary>
        /// 覆盖多个构造函数参数
        /// </summary>
        static void MultipleParameterOverride()
        {
            var container = new UnityContainer();
            container.RegisterType<ICar, Audi>();
            container.RegisterType<ICarKey, AudiKey>();
            container.Resolve<Driver>(new ParameterOverride[]
                {new ParameterOverride("car", new Ford()), new ParameterOverride("key", new FordKey())}).RunCar();
        }

        /// <summary>
        /// 属性覆盖
        /// </summary>
        static void PropertyOverride()
        {
            var container = new UnityContainer();
            container.RegisterType<Driver4>(new InjectionProperty(nameof(Driver4.Car), new Audi()));
            container.Resolve<Driver4>().RunCar();
            container.Resolve<Driver4>(new PropertyOverride(nameof(Driver4.Car), new Ford())).RunCar();
        }

        /// <summary>
        /// 依赖覆盖
        /// </summary>
        static void DependencyOverride()
        {
            var container = new UnityContainer();
            container.RegisterType<ICar, Audi>();
            container.Resolve<Driver2>().RunCar();

            container.Resolve<Driver2>(new DependencyOverride<ICar>(new Ford())).RunCar(); // 这里要用泛型，直接给对象传名称不好使
        }

        static void InterfaceInterception()
        {
            var container = new UnityContainer();
            container.AddNewExtension<Interception>(); // 
            container.RegisterType<ICar, Ford>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptionBehavior>()
                /*,new InterceptionBehavior<CacheInterceptionBehavior>()*/); // 可以添加多个拦截器
            var driver = container.Resolve<Driver>();
            driver.RunCar(); // 这里打印出来的类名会是生成的代理类
            driver.RunCar2(); // 接口代理生成的代理类不是派生类，所以不能通过 as Ford 转型
            driver.RunCar3(); 
        }

        /// <summary>
        /// 透明代理已经被弃用
        /// </summary>
        static void TransparentProxyInterception()
        {

        }

        /// <summary>
        /// 虚方法代理，这个方法不会生成代理类，而是生成派生类
        /// </summary>
        static void VirtualMethodInterception()
        {
            var container = new UnityContainer();
            container.AddNewExtension<Interception>();
            container.RegisterType<ICar, Ford>(
                new Interceptor<VirtualMethodInterceptor>(),
                new InterceptionBehavior<LoggingInterceptionBehavior>());
            var driver= container.Resolve<Driver>();
            driver.RunCar();
            driver.RunCar2(); // 虚方法代理生成的是派生类，所以可以通过 as Ford 转型。
            driver.RunCar3();

        }
    }
}
