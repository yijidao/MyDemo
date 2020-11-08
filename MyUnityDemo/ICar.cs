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

        public int Run2() => ++_kilometer;

        public virtual int Run3() => ++_kilometer;
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

        public Action RunCar;

        public void RunCar2() => Console.WriteLine($"Running {_car.GetType().Name} - {(_car as Ford)?.Run2()} kilometer ");

        public void RunCar3() => Console.WriteLine($"Running {_car.GetType().Name} - {(_car as Ford)?.Run3()} kilometer ");
    }

    public class Driver2
    {
        [Dependency] // 特性注入
        //[Dependency("audi")] // 带标识符的特性注入
        public ICar Car { get; set; }


        public Driver2()
        {

        }
        public void RunCar() => Console.WriteLine($"Running {Car.GetType().Name} - {Car.Run()} kilometer ");

    }

    public class Driver3
    {
        [Dependency("audi")]
        public ICar Car { get; set; }

        public Driver3()
        {

        }
        public void RunCar() => Console.WriteLine($"Running {Car.GetType().Name} - {Car.Run()} kilometer ");
    }

    public class Driver4
    {
        public ICar Car { get; set; }

        public Driver4()
        {
        }
        public void RunCar() => Console.WriteLine($"Running {Car.GetType().Name} - {Car.Run()} kilometer ");
    }

    public class Driver5
    {

        public Driver5()
        {
        }

        private ICar _car;

        [InjectionMethod]
        public void UseCar(ICar car) => _car = car;

        public void UseCar2(ICar car) => _car = car;


        public void RunCar() => Console.WriteLine($"Running {_car.GetType().Name} - {_car.Run()} kilometer ");
    }
}
