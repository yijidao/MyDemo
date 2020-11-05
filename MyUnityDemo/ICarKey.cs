using System;
using System.Collections.Generic;
using System.Text;

namespace MyUnityDemo
{
    public interface ICarKey
    {
    }

    public class BMWKey : ICarKey { }

    public class FordKey : ICarKey { }

    public class AudiKey : ICarKey { }
}
