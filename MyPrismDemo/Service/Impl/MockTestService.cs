using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace MyPrismDemo.Service.Impl
{
    public class MockTestService : ITestService
    {

        public ConcurrentDictionary<string, byte> Tags { get; set; } = new ConcurrentDictionary<string, byte>();

        private readonly ITestService _realService;

        public MockTestService(ITestService realService)
        {
            _realService = realService;
        }


        public string GetData(int param)
        {
            var result = "返回值是 Mock NaN";
            switch (param)
            {
                case 0:
                    result = "返回值是 Mock 0";
                    break;
                case 1:
                    result = "返回值是 Mock 1";
                    break;
                case 2:
                    result = "返回值是 Mock 2";
                    break;
            }

            return result;
        }

        public string GetData2(int param)
        {
            return _realService.GetData2(param);
        }

        public void AddData(string key)
        {
            Debug.WriteLine(Tags.TryAdd(key, new byte()) ? "新增成功" : "新增失败");
        }

        public void RemoveData(string key)
        {
            Debug.WriteLine(Tags.TryRemove(key, out var _) ? "移除成功" : "移除失败");
        }
    }
}
