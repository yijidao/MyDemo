using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Prism.Mvvm;

namespace MyDemo
{
    /// <summary>
    /// ComboboxDemo2.xaml 的交互逻辑
    /// </summary>
    public partial class ComboboxDemo2 : UserControl
    {




        public ComboboxDemo2()
        {
            InitializeComponent();

            DataContext = new ComboboxDemo2ViewModel();
        }

        private void OpenPopup(object sender, MouseButtonEventArgs e)
        {
            dropdown.IsOpen = !dropdown.IsOpen;
        }
    }


    public class ComboboxDemo2ViewModel : BindableBase
    {
        private ObservableCollection<ComboboxDemo2Model> _models;
        public ObservableCollection<ComboboxDemo2Model> Models
        {
            get => _models;
            set => SetProperty(ref _models, value);
        }

        private ComboboxDemo2Model _selectedModel;
        public ComboboxDemo2Model SelectedModel
        {
            get => _selectedModel;
            set => SetProperty(ref _selectedModel, value);
        }

        public ComboboxDemo2ViewModel()
        {
            var models = new List<ComboboxDemo2Model>();

            var index = 1;

            for (int i = 1; i <= 10; i++)
            {
                var model = new ComboboxDemo2Model
                {
                    Name = $"Item{index}",
                    Children = new List<string>()
                };

                for (int j = 0; j < 8; j++)
                {
                    model.Children.Add($"Node{index++}");
                }
                models.Add(model);
            }

            Models = new ObservableCollection<ComboboxDemo2Model>(models);
        }

    }

    public class ComboboxDemo2Model
    {
        public string Name { get; set; }

        public List<string> Children { get; set; }

    }
}
