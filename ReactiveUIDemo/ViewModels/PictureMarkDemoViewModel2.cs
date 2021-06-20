using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace ReactiveUIDemo.ViewModels
{
    public class PictureMarkDemoViewModel2 : ReactiveObject
    {
        private string myField;
        public string MyProperty
        {
            get => myField;
            set => this.RaiseAndSetIfChanged(ref myField, value);
        }


    }
}
