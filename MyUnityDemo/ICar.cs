using System;
using System.Collections.Generic;
using System.Text;

namespace MyUnityDemo
{
    public interface ICar
    {

        int Run();
    }

    public class BMW : ICar
    {

        private int _kilometer; // 公里

        public int Run() => ++_kilometer;
    }

    public class Ford : ICar
    {
        private int _kilometer;
        public int Run() => ++_kilometer;
    }

    public class Audi : ICar
    {
        private int _kilometer;
        public int Run() => ++_kilometer;
    }

    public class Driver
    {
        private readonly ICar _car;

        public Driver(ICar car)
        {
            _car = car;
        }

        public void RunCar() => Console.WriteLine($"Running {_car.GetType().Name} - {_car.Run()} kilometer ");
    }
}
