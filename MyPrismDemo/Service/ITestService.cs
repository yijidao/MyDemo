using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPrismDemo.Service
{
    public interface ITestService
    {
        string GetData(int param);


        string GetData2(int param);

        void AddData(string key);

        void RemoveData(string key);

    }
}
