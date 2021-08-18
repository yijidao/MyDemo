using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Moq;
using MyPrismDemo.Service;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;

namespace MyPrismDemo.ViewModels
{
    public class MoqViewModel : BindableBase
    {
        public ICommand DataCommand { get; set; }

        private int _param;
        public int Param
        {
            get => _param;
            set => SetProperty(ref _param, value);
        }

        public MoqViewModel(IContainerProvider provider)
        {
            DataCommand = new DelegateCommand(() =>
            {
                var service = ((PrismApplication)Application.Current).Container.Resolve<ITestService>();
                var result = service.GetData(Param);
                MessageBox.Show(result);


                var model = new TestModel() { Age = 20, Name = "33" };
                var json = JsonConvert.SerializeObject(model);
                var model2 = JsonConvert.DeserializeObject<object>(json);
                Debug.WriteLine(model2.ToString());
                //for (int i = 0; i < 10; i++)
                //{
                //    var i1 = i;
                //    Task.Run(() =>
                //    {
                //        while (true)
                //        {


                //            service.RemoveData($"{i1}");
                //            Task.Delay(500).Wait();
                //        }
                //    });

                //}

                //for (int i = 0; i < 10; i++)
                //{
                //    var i1 = i;
                //    Task.Run(() =>
                //    {
                //        while (true)
                //        {


                //            service.AddData($"{i1}");
                //            Task.Delay(500).Wait();
                //        }
                //    });

                //}


            });
            //DataCommand = new DelegateCommand(() =>
            //{
            //    var service = provider.Resolve<ITestService>();
            //    var result = service.GetData(Param);
            //    MessageBox.Show(result);
            //});
            //DataCommand = new DelegateCommand(() =>
            //{
            //    var mock = new Mock<ITestService>();
            //    mock.Setup(x => x.GetData(1)).Returns("Mock 1");
            //    mock.Setup(x => x.GetData(0)).Returns("Mock 0");
            //    var result = mock.Object.GetData(Param);
            //    MessageBox.Show(result);
            //});
            //DataCommand = new DelegateCommand(() =>
            //{
            //    var mock = new Mock<ITestService>();
            //    mock.Setup(x => x.GetData(1)).Returns("Mock 1");
            //    mock.Setup(x => x.GetData(0)).Returns("Mock 0");
            //    var result = mock.Object.GetData(Param);
            //    MessageBox.Show(result);
            //});
        }


    }

    public class TestModel
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
