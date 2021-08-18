using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPrismDemo.Service.Impl
{
    public class TestService : ITestService
    {
        public string GetData(int param)
        {
            var result = "返回值是 NaN";
            switch (param)
            {
                case 0:
                    result = "返回值是0";
                    break;
                case 1:
                    result = "返回值是1";
                    break;
                case 2:
                    result = "返回值是2";
                    break;
            }

            return result;
        }

        public string GetData2(int param)
        {
            return GetData(param);
        }

        public void AddData(string key)
        {
            
        }

        public void RemoveData(string key)
        {
        }
    }
}
