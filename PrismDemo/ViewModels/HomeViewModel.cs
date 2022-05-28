using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using PrismDemo.Service;
using PrismDemo.Views;

namespace PrismDemo.ViewModels
{
    public class HomeViewModel : BindableBase
    {
        private readonly ITest _test;
        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private long? _input;
        public long? Input
        {
            get => _input;
            set => SetProperty(ref _input, value);
        }

        public ICommand Command1 { get; }

        public ICommand Command2 { get; }

        public ICommand Command3 { get; }
        public ICommand Command4 { get; }

        public ICommand PrintPdfCommand { get; }
        public ICommand PrintPdf2Command { get; }
        public ICommand ComboBoxViewCommand { get; }

        public HomeViewModel(ITest test, IDialogService dialogService)
        {
            _test = test;
            Title = "HOME";
            Command1 = new DelegateCommand(() =>
            {
                _test?.T1();
                Debug.WriteLine("Command1");
            });

            Command2 = new DelegateCommand(async () =>
            {

                await _test.AsyncT1();
                Debug.WriteLine("Command2");
            });

            Command3 = new DelegateCommand(async () =>
            {
                var result = await _test.AsyncT2(Input);
                Debug.WriteLine($"Command3  {result}");
            });

            Command4 = new DelegateCommand(async () =>
            {
                var result = await _test.AsyncT2(Input?.ToString());
                Debug.WriteLine($"Command4  {result}");
            });

            PrintPdfCommand = new DelegateCommand(() =>
            {

                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = $"PrismDemo.ViewModels.test_print.html";

                using var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream == null) return;
                using var reader = new StreamReader(stream);
                var t = reader.ReadToEnd();
                dynamic d = new ExpandoObject();
                d.title = "购书目录";
                var list = new List<Book>
                {
                    new()
                    {
                        Title = "JavaScript权威指南 原书第7版",
                        Author = "巨佬1",
                        Price = 90.3
                    },
                    new()
                    {
                        Title = "深入浅出node.js",
                        Author = "巨佬2",
                        Price = 57.8
                    },
                    new()
                    {
                        Title = "编码：隐匿在计算机软硬件背后的语言",
                        Author = "巨佬3",
                        Price = 89.00
                    }
                };
                d.books=list;


                var p = new DialogParameters
                {
                    {"template", t},
                    {"data", d}
                };
                dialogService.ShowDialog(nameof(PrintPdfView), p, null);
            });

            PrintPdf2Command = new DelegateCommand(() =>
            {

                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = $"PrismDemo.ViewModels.test_print2.html";

                using var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream == null) return;
                using var reader = new StreamReader(stream);
                var t = reader.ReadToEnd();
                dynamic d = new ExpandoObject();
                d.title = "购书目录";
                d.books = new List<Book>
                {
                    new()
                    {
                        Title = "JavaScript权威指南 原书第7版",
                        Author = "巨佬1",
                        Price = 90.3
                    },
                    new()
                    {
                        Title = "深入浅出node.js",
                        Author = "巨佬2",
                        Price = 57.8
                    },
                    new()
                    {
                        Title = "编码：隐匿在计算机软硬件背后的语言",
                        Author = "巨佬3",
                        Price = 89.00
                    }
                };
                d.columns = new List<Column>
                {
                    new()
                    {
                        Header = "书名",
                        Binding = nameof(Book.Title)
                    },
                    new()
                    {
                        Header = "作者",
                        Binding = nameof(Book.Author)
                    },
                    new()
                    {
                        Header = "价格",
                        Binding = nameof(Book.Price)
                    },
                };


                var p = new DialogParameters
                {
                    {"template", t},
                    {"data", d}
                };
                dialogService.ShowDialog(nameof(PrintPdf2View), p, null);
            });

            ComboBoxViewCommand = new DelegateCommand(() => dialogService.Show(nameof(ComboBoxView)));
        }
    }

    public class Book
    {
        

        public string Title { get; set; }

        public string Author { get; set; }

        public double Price { get; set; }
    }

    public class Column
    {
        public string Header { get; set; }

        public string Binding { get; set; }
    }
}
