using System;
using System.Collections.Generic;
using System.Text;
using Unity;

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
        private readonly string _driverName;
        private readonly ICarKey _key;
        private readonly ICar _car;

        //[InjectionConstructor]
        public Driver(ICar car)
        {
            _car = car;
            RunCar = () => Console.WriteLine($"Running {_car.GetType().Name} - {_car.Run()} kilometer ");
        }

        public Driver(ICar car, ICarKey key)
        {
            _car = car;
            _key = key;
            RunCar = () => Console.WriteLine($"Running {_car.GetType().Name} with {_key.GetType().Name} - {_car.Run()} kilometer ");
        }

        public Driver(ICar car, string driverName)
        {
            _car = car;
            _driverName = driverName;
            RunCar = () => Console.WriteLine($"{_driverName} is Running {_car.GetType().Name} - {_car.Run()} kilometer ");
        }

        //public void RunCar()
        //{
        //    if (_car != null && _key != null)
        //    {
        //        Console.WriteLine($"Running {_car.GetType().Name} with {_key.GetType().Name} - {_car.Run()} kilometer ");
        //    }
        //    else if (_car != null)
        //    {
        //        Console.WriteLine($"Running {_car.GetType().Name} - {_car.Run()} kilometer ");
        //    }
        //}

        public Action RunCar;
    }
}
