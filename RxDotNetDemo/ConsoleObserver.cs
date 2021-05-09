using System;
using System.Collections.Generic;
using System.Text;

namespace RxDotNetDemo
{
    public class ConsoleObserver<T> : IObserver<T>
    {
        private readonly string _name;
        private readonly Action<T> _onNext;

        public ConsoleObserver(string name = "", Action<T> onNext = null)
        {
            _name = name;
            _onNext = onNext;
        }

        public void OnCompleted() => Console.WriteLine($"{_name} - OnCompleted()");

        public void OnError(Exception error)
        {
            Console.WriteLine($"{_name} - OnError:");
            Console.WriteLine($"\t {error}");
        }

        public void OnNext(T value)
        {
            if (_onNext == null)
            {
                Console.WriteLine($"{_name} - OnNext({value})");
            }
            else
            {
                _onNext.Invoke(value);
            }
        }
    }
}
