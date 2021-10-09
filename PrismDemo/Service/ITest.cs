using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismDemo.Service
{
    public interface ITest
    {
        void T1();

        Task AsyncT1();
    }

    public class Test1 : ITest
    {
        public void T1()
        {
            Debug.WriteLine("T1...");
            throw new Exception();
        }

        public async Task AsyncT1()
        {
            await Task.Delay(TimeSpan.FromSeconds(.5));
            await Task.Run(() => Debug.WriteLine("AsyncT1..."));
        }
    }
}
