using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using MyPrismDemo.ViewModels;

namespace MyPrismDemo.Views
{
    /// <summary>
    /// TrainTimeTableView.xaml 的交互逻辑
    /// </summary>
    public partial class TrainTimeTableView : UserControl
    {
        public TrainTimeTableView()
        {
            InitializeComponent();

            var vm = (TrainTimeTableViewModel)DataContext;
            vm.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(TrainTimeTableViewModel.TrainTimeInfo) && vm.TrainTimeInfo?.Count > 0)
                {
                    grid.Columns.Clear();

                    grid.Columns.Add(CreateStationColumn());
                    var columns = CreateColumn(vm.TrainCount);
                    grid.Columns.AddRange(columns);
                    grid.ItemsSource = vm.TrainTimeInfo;
                }
            };
        }

        private DataGridTextColumn CreateStationColumn() => new DataGridTextColumn
        {
            Header = "站点",
            Binding = new Binding("Name")
        };

        private DataGridTemplateColumn[] CreateColumn(int count)
        {
            var list = new List<DataGridTemplateColumn>();
            for (int i = 1; i <= count; i++)
            {
                var column = new DataGridTemplateColumn
                {
                    Width = 200,
                    HeaderTemplate = CreateHeaderTemplate(i),
                    CellTemplate = CreateCellTemplate(i)
                };
                list.Add(column);
            }
            return list.ToArray();
        }

        private DataTemplate CreateHeaderTemplate(int index)
        {
            index -= 1;
            //< TextBlock Grid.ColumnSpan = ""2"" Text = ""车次{ index} "" HorizontalAlignment = ""Center"" />
            var stringReader = new StringReader(@"
<DataTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
              xmlns:local=""clr-namespace:MyPrismDemo.Views;assembly=MyPrismDemo""
              xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""> 
    <DataTemplate.Resources>
        <local:HeaderTimeoutConvert x:Key=""HeaderTimeoutConvert""></local:HeaderTimeoutConvert>
    </DataTemplate.Resources >
    <Grid Width=""200""
          DataContext=""{ Binding DataContext, RelativeSource = { RelativeSource AncestorType = { x:Type DataGrid}}}"">
        <Grid.Style>
            <Style TargetType=""Grid"">
                <Style.Triggers >
                    <DataTrigger Value = ""True"">
                        <DataTrigger.Binding >
                            <MultiBinding Converter = ""{StaticResource HeaderTimeoutConvert}"" >
                                <Binding Path = ""TrainNames[" + index + @"]"" />
                                <Binding Path = ""TimeoutDic"" />
                            </MultiBinding >
                        </DataTrigger.Binding >
                        <DataTrigger.Setters >
                            <Setter Property = ""Background"" Value = ""#4CFF3148"" />
                        </DataTrigger.Setters >
                    </DataTrigger >
                </Style.Triggers >
            </Style >
        </Grid.Style >
        <Grid.RowDefinitions>
            <RowDefinition />
            <RowDefinition />
        </Grid.RowDefinitions>
        <Grid.ColumnDefinitions>
            <ColumnDefinition />
            <ColumnDefinition />
        </Grid.ColumnDefinitions>

        <TextBlock Grid.ColumnSpan = ""2"" Text = ""{Binding TrainNames[" + index + @"]}"" HorizontalAlignment = ""Center"" />
        <TextBlock Grid.Row = ""1"" Text = ""计划"" />
        <TextBlock Grid.Column = ""1"" Grid.Row = ""1"" Text = ""实际"" />
    </Grid >
</DataTemplate>"
                        );
            var xmlReader = XmlReader.Create(stringReader);
            return XamlReader.Load(xmlReader) as DataTemplate; ;
        }

        private DataTemplate CreateCellTemplate(int index)
        {
            var stringReader = new StringReader(@"
<DataTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" 
              xmlns:local=""clr-namespace:MyPrismDemo.Views;assembly=MyPrismDemo""
              xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
    <DataTemplate.Resources>
        <local:TimeConverter x:Key=""TimeConverter""></local:TimeConverter>
    </DataTemplate.Resources >

    <Grid>
        <Grid.Style>
            <Style TargetType=""Grid"">
                <Style.Triggers >
                    <DataTrigger Value = ""True"" Binding = ""{Binding Timeout" + index + @"}"" >
                        <Setter Property = ""Background"" Value = ""#4CFF3148"" />
                    </DataTrigger >
                </Style.Triggers >
            </Style >
        </Grid.Style >
        <Grid.ColumnDefinitions>
            <ColumnDefinition />
            <ColumnDefinition />
        </Grid.ColumnDefinitions>
            <TextBlock Text=""{Binding PlanTime" + index + @", StringFormat = {}{0:HH:mm}}"" >
            </TextBlock>
            <TextBlock Grid.Column=""1"" Text=""{Binding RealTime" + index + @", StringFormat={}{0:HH:mm}, TargetNullValue=-}"" >
                <TextBlock.Style>
                    <Style>
                        <Style.Triggers>
                            <DataTrigger Value=""True"">
                                <DataTrigger.Binding>
                                    <MultiBinding Converter = ""{StaticResource TimeConverter}"">
                                        <Binding Path = ""PlanTime" + index + @""" />
                                        <Binding Path = ""RealTime" + index + @""" />
                                    </MultiBinding>
                                </DataTrigger.Binding>
                                <Setter Property = ""TextBlock.Foreground"" Value = ""#FF3148"" />
                            </DataTrigger>
                        </Style.Triggers>
                    </Style >
                </TextBlock.Style >
            </TextBlock>
    </Grid >
</DataTemplate > "
                );
            var xmlReader = XmlReader.Create(stringReader);
            return XamlReader.Load(xmlReader) as DataTemplate;
        }


        //void AddColumns(string[] newColumnNames)
        //{
        //    foreach (string name in newColumnNames)
        //    {
        //        grid.Columns.Add(new DataGridTextColumn
        //        {
        //            // bind to a dictionary property
        //            Binding = new Binding("Custom[" + name + "]"),
        //            Header = name
        //        });
        //    }
        //}


    }

    public class TimeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is DateTime)) return false;
            var planTime = (DateTime)values[0];
            var realTime = values[1] as DateTime?;
            if (realTime == null) return false;
            return realTime.Value - planTime >= TimeSpan.FromMinutes(2);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class HeaderTimeoutConvert : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is string trainName && values[1] is Dictionary<string, bool> timeoutDic &&
                timeoutDic.TryGetValue(trainName, out var timeout))
            {
                return timeout;
            }
            else
            {
                return false;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
