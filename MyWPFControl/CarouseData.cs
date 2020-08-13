using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWPFControl
{
    public class CarouseData : INotifyPropertyChanged
    {

        public CarouseData()
        {
            Datas = new ObservableCollection<TClass> { new TClass { Text1 = "11", Text2 = "22" }, new TClass { Text1 = "33", Text2 = "44" } };
            T1 = new TClass { Text1 = "11", Text2 = "22" };
        }


        private TClass _T1;

        public TClass T1
        {
            get { return _T1; }
            set
            {
                _T1 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(T1)));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;


        private ObservableCollection<TClass> _Datas = new ObservableCollection<TClass>();

        public ObservableCollection<TClass> Datas
        {
            get { return _Datas; }
            set
            {
                _Datas = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Datas)));
            }
        }

        private TClass _SelectedData;

        public TClass SelectedData
        {
            get { return _SelectedData; }
            set
            {
                _SelectedData = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedData)));
            }
        }


        public void Add()
        {
            Datas.Add(new TClass { Text1="新增", Text2="新增" });
        }
    }
}
