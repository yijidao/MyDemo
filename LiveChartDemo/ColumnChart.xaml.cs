using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using Newtonsoft.Json;

namespace LiveChartDemo
{
    /// <summary>
    /// ColumnChart.xaml 的交互逻辑
    /// </summary>
    public partial class ColumnChart : UserControl
    {
        public ColumnChart()
        {
            InitializeComponent();


            //Charting.For<DateValueModel>(
            //    Mappers.Xy<DateValueModel>()
            //        .X(model => model.Date.Ticks)
            //        .Y(model => model.Value));
            //Charting.For<DateValueModel>(Mappers.);

            GetData();
            //GetData1();
        }

        private void GetData1()
        {
            var series = new SeriesCollection
            {
                new ColumnSeries {Title = "一号线", Values = new ChartValues<double> {10, 50, 19, 34}},
                new ColumnSeries {Title = "二号线", Values = new ChartValues<double> {38, 21, 23}}
            };
            chart.Series = series;
        }

        public HttpClient Client { get; set; } = new HttpClient();

        private async Task GetData()
        {
            var response = await Client.GetAsync("https://localhost:5001/efficientdesignapi/GetInOutPassengerFlow");
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var chartDates = JsonConvert.DeserializeObject<List<LineChartData>>(result);
                var series = new SeriesCollection();

                var lable = chartDates.FirstOrDefault().DataModels.Select(model => $"{model.Date.Hour}:00").ToList();
                chart.AxisX.Add(new Axis
                {
                    Title = "时间",
                    Labels = lable,
                });
                // 设置纵轴
                chart.AxisY.Add(new Axis
                {
                    Title = "人次",
                    LabelFormatter = d => $"{d}万",
                });

                series.AddRange(chartDates.Select(data => new ColumnSeries()
                {
                    Title = data.Name,
                    Values = new ChartValues<double>(data.DataModels.Select(model => model.Value))
                }));
                chart.Series = series;
            }
        }
    }
}
