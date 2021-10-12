using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrismAopModuleDemo;

namespace PrismDemo.Service
{
    public interface ITest
    {
        void T1();

        Task AsyncT1();
        [Cache]
        Task<string> AsyncT2(long? id = null);

        [Cache]
        Task<string> AsyncT2(string id = null);
    }

    public class Test1 : ITest

    {
        public void T1()
        {
            Debug.WriteLine("T1...");
            //throw new Exception();
        }

        public async Task AsyncT1()
        {
            await Task.Delay(TimeSpan.FromSeconds(.5));
            await Task.Run(() => Debug.WriteLine("AsyncT1..."));
        }

        public async Task<string> AsyncT2(long? id)
        {
            await Task.Delay(TimeSpan.FromSeconds(.5));
            if (id == 3) throw new ArgumentException("3");
            Debug.WriteLine("AsyncT2...");
            return id == null ? "id is  null" : id.ToString();
        }

        public async Task<string> AsyncT2(string id = null)
        {
            await Task.Delay(TimeSpan.FromSeconds(.5));
            Debug.WriteLine("String AsyncT2...");
            return id ?? "id is  null";
        }
    }
}
