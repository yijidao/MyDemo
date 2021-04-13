using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;

namespace RxUIDemo2.ViewModels
{
    public class InteractionViewModel : ReactiveObject
    {

        public Interaction<string, bool> Confirm { get; }

        public InteractionViewModel()
        {
            Confirm = new Interaction<string, bool>();
        }
    }
}
