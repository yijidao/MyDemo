using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DynamicData;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;

namespace LiveChartDemo
{
    /// <summary>
    /// LineChartDemo.xaml 的交互逻辑
    /// </summary>
    public partial class LineChartDemo : UserControl
    {
        public SeriesCollection Series { get; set; } = new SeriesCollection();

        public LineChartDemo()
        {
            InitializeComponent();

            var chartValues1 = new ChartValues<DateValueModel>();
            var chartValues2 = new ChartValues<DateValueModel>();
            var chartValues3 = new ChartValues<DateValueModel>();

            Series.Add(new LineSeries
            {
                Title = "一号线",
                Values = chartValues1
            });

            Series.Add(new LineSeries
            {
                Title = "二号线",
                Values = chartValues2
            });

            Series.Add(new LineSeries
            {
                Title = "三号线",
                Values = chartValues3
            });

            lineChart.Series = Series;

            axisX.LabelFormatter = d => new DateTime((long)d).Hour.ToString();
            axisX.Title = "时间";
            axisY.LabelFormatter = d => $"{d}万";
            axisY.Title = "人次";


            Charting.For<DateValueModel>(
                Mappers.Xy<DateValueModel>()
                             .X(model => model.Date.Ticks)
                             .Y(model => model.Value));
            axisX.MinValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 6, 0, 0).Ticks;
            axisX.MaxValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 0, 0).Ticks;

            var random = new Random();

            Task.Run(async () =>
            {
                var current = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 6, 0, 0);

                while (true)
                {
                    await Task.Delay(1000);

                    Dispatcher.Invoke(() =>
                    {

                        chartValues1.Add(new DateValueModel(current, random.Next(1, 10)));
                        chartValues2.Add(new DateValueModel(current, random.Next(1, 10)));
                        chartValues3.Add(new DateValueModel(current, random.Next(1, 10)));
                    });
                    current = current.AddHours(1);
                }

            });

        }
    }


    public class DateValueModel
    {
        public DateTime Date { get; }
        public double Value { get; }

        public DateValueModel(DateTime date, double value)
        {
            Date = date;
            Value = value;
        }
    }
}
