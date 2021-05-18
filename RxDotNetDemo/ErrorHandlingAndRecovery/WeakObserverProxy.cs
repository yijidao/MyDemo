using System;
using System.Collections.Generic;
using System.Text;

namespace RxDotNetDemo.ErrorHandlingAndRecovery
{
    class WeakObserverProxy<T> : IObserver<T>
    {
        private WeakReference<IObserver<T>> _weakObserver;
        private IDisposable _subscriptionToSource;

        public WeakObserverProxy(IObserver<T> observer)
        {
            _weakObserver = new WeakReference<IObserver<T>>(observer);
        }

        internal void SetSubscription(IDisposable subscriptionToSource)
        {
            _subscriptionToSource = subscriptionToSource;
        }

        void NotifyObserver(Action<IObserver<T>> action)
        {
            if (_weakObserver.TryGetTarget(out var observer))
            {
                action(observer);
            }
            else
            {
                _subscriptionToSource.Dispose();
            }
        }


        public void OnCompleted()
        {
            NotifyObserver(observer => observer.OnCompleted());
        }

        public void OnError(Exception error)
        {
            NotifyObserver(observer => observer.OnError(error));
        }

        public void OnNext(T value)
        {
            NotifyObserver(observer => observer.OnNext(value));
        }

        public IDisposable AsDisposable()
        {
            return _subscriptionToSource;
        }
    }
}
