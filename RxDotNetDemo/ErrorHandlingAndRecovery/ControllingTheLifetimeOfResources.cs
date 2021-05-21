using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RxDotNetDemo.Extensions;

namespace RxDotNetDemo.ErrorHandlingAndRecovery
{
    public class ControllingTheLifetimeOfResources
    {
        /// <summary>
        /// 托管对象是在gc时回收，但是gc不会回收非托管对象，所以如果托管对象使用了非托管对象，又没有手动关闭，可能造成资源占用bug，例如打开了文本没有关闭，那么其他程序就用不来这个文本。
        /// .net 通过实现 IDisposable，在 Dispose 中编写非托管对象回收逻辑，并且手动调用来实现对非托管对象的释放
        /// rx 提供了 using 来实现类似操作，using 接收两个参数，第一个参数是生成资源的工厂方法，第二个是生成 observable 用于操作资源的工厂方法
        /// </summary>
        public static void DisposingInDeterministicWay()
        {
            var o2 = new string[] { "1", "2" }.ToObservable();

            var o = Observable.Using(() => new StreamWriter("../TextFile1.txt"), writer =>
            {
                return o2.Do(x => writer.WriteLine(x));
            });

            o.SubscribeConsole();
        }

        /// <summary>
        /// 这个demo 展示了，using 会在 OnError OnCompleted Dispose 时执行 Dispose
        /// </summary>
        public static void DisposingInDeterministicWay2()
        {
            var subject = new Subject<int>();
            var observable = Observable.Using(() => Disposable.Create(() => { Console.WriteLine("DISPOSED"); }),
                _ => subject);

            Console.WriteLine("Disposed when completed");
            observable.SubscribeConsole();
            subject.OnCompleted();

            Console.WriteLine("Disposed when error occurs");
            subject = new Subject<int>();
            observable.SubscribeConsole();
            subject.OnError(new Exception("error"));

            Console.WriteLine("Disposed when subscription disposed");
            subject = new Subject<int>();
            var subscription = observable.SubscribeConsole();
            subscription.Dispose();
        }

        /// <summary>
        /// rx 提供了 Finally 操作符，用法跟 try finally 一样，用于放置一些不需要放置到 dispose 的代码，但又不得不执行的代码
        /// Finally 在 OnComplete Dispose OnError 之后执行
        /// </summary>
        public static void DeterministicFinalization()
        {
            Console.WriteLine("Successful complete");
            Observable.Interval(TimeSpan.FromSeconds(1))
                .Take(3)
                .Finally(() => Console.WriteLine("Finally Code"))
                .SubscribeConsole();

            Console.WriteLine("Error termination");
            Observable.Throw<Exception>(new Exception("error"))
                .Finally(() => Console.WriteLine("Finally Code"))
                .SubscribeConsole();

            Console.WriteLine("Unsubscribing");
            var subject = new Subject<int>();
            var sbj = subject.AsObservable()
                .Finally(() => Console.WriteLine("Finally Code"))
                .SubscribeConsole();
            sbj.Dispose();
        }

        /// <summary>
        /// Dangling Observer 悬挂观察者是指 observer 被 observable 保持住引用，即使 Observer 已经执行完 OnComplete 了。这个场景类似于事件订阅，因为没有取消订阅而且源对象没有被GC，导致订阅者一直存在。
        /// 出现 Dangling Observer 是因为没有对 subscribe 调用 dispose，假如有个聊天室 window，订阅了一个消息 observable，在关闭 window 时，没有调用dispose，那么由于 observable 会一直给window 推消息，
        /// 那么就算 window 被关闭了，也依旧一直在接收消息，导致无法被gc，所以可能会有内存问题
        /// 在 gc 的时候，subscribe 的 Dispose 是不会被调用的，因为 Rx disposable 没有实现 finalizer，很多人都会误解，以为会调用，所以不能依赖 GC 来调用 Dispose
        /// </summary>
        public static void DanglingObservers()
        {

        }

        /// <summary>
        /// 无法 gc 原因 https://stackoverflow.com/questions/29119997/c-sharp-why-gc-cant-collect-weakreferences-target-in-my-code
        /// </summary>
        public static void CreatingWeakObserver()
        {
            object obj = new object();
            var weak = new WeakReference(obj);

            GC.Collect();
            Console.WriteLine($"IsAlive: {weak.IsAlive}");

            obj = null;
            GC.Collect();
            //GC.WaitForPendingFinalizers();
            Thread.Sleep(TimeSpan.FromSeconds(2));
            Console.WriteLine($"IsAlive: {weak.IsAlive}");
        }

        /// <summary>
        /// 通过虚引用来解决一些无法手动释放引用的情况
        /// </summary>
        public static void CreatingWeakObserver2()
        {
            var subscription = Observable.Interval(TimeSpan.FromSeconds(1))
                .AsWeakObservable()
                .SubscribeConsole("Interval");

            Console.WriteLine("Collecting");
            GC.Collect();
            Thread.Sleep(2000);

            GC.KeepAlive(subscription);
            Console.WriteLine("Done sleeping");
            Console.WriteLine("Collecting");

            subscription = null;
            GC.Collect();
            Thread.Sleep(TimeSpan.FromSeconds(2));
            Console.WriteLine("Done sleeping");
        }

        public static void WeakCache()
        {
            var cacheSize = 50;
            var r = new Random(cacheSize);
            var c = new Cache(cacheSize);

            var dataName = "";
            GC.Collect(0);
            for (int i = 0; i < c.Count; i++)
            {
                var index = r.Next(c.Count);
                dataName = c[index].Name;
            }

            var regenPercent = c.RegenerationCount / (double) c.Count;
            Console.WriteLine($"Cache size:{c.Count}, Regenerated:{regenPercent:P2}%");
        }
    }

    public class Cache
    {
        private static Dictionary<int, WeakReference> _cache;

        private int _regenCount = 0;

        public Cache(int count)
        {
            _cache = new Dictionary<int, WeakReference>();

            for (int i = 0; i < count; i++)
            {
                _cache.Add(i, new WeakReference(new Data(i), false));
            }
        }

        public int Count => _cache.Count;

        public int RegenerationCount => _regenCount;

        public Data this[int index]
        {
            get
            {
                var d = _cache[index].Target as Data;
                if (d == null)
                {
                    Console.WriteLine($"Regenerate object at {index}: Yes");
                    d = new Data(index);
                    _cache[index].Target = d;
                    _regenCount++;
                }
                else
                {
                    Console.WriteLine($"Regenerate object at {index}: No");
                }

                return d;
            }
        }
    }

    public class Data
    {
        private byte[] _data;
        private string _name;

        public Data(int size)
        {
            _data = new byte[size * 1024];
            _name = size.ToString();
        }

        public string Name => _name;
    }
}
