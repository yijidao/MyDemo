using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;

namespace RxDotNetDemo
{
    class CreateOperate
    {
        public CreateOperate()
        {
            Observable.Create<int>(observer =>
            {
                for (int i = 0; i < 5; i++)
                {
                    observer.OnNext(i);
                }
                observer.OnCompleted();
                return Disposable.Empty;
            });
        }

        public IObservable<int> GetObservableByCreate()
        {
            return Observable.Create<int>(observer =>
            {
                for (int i = 0; i < 5; i++)
                {
                    observer.OnNext(i);
                }

                return Disposable.Empty;
            });
        }

    }
}
