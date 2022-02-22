using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Refit;

namespace PrismDemo2.ViewModels
{
    public class RefitDemo1ViewModel : BindableBase
    {
        private readonly IWeatherForecast _weatherForecastService;

        public ICommand GetWeatherForecastCommand { get; }
        public ICommand GetWeatherForecastCommand2 { get; }

        public RefitDemo1ViewModel()
        {

            var options = new JsonSerializerOptions();
            //options.AddContext<>();

            //SystemTextJsonContentSerializer
            _weatherForecastService = RestService.For<IWeatherForecast>("https://localhost:5001", new RefitSettings());

            GetWeatherForecastCommand = new DelegateCommand(async () =>
            {
                var r = await _weatherForecastService.GetWeatherForecast();
                Debug.WriteLine(r);
            });

            GetWeatherForecastCommand2 = new DelegateCommand(async () =>
            {
                var r = await _weatherForecastService.GetWeatherForecast2();
                Debug.WriteLine(r);
            });
        }
    }

    
    public interface IWeatherForecast
    {

        [Get("/WeatherForecast")]
        Task<string> GetWeatherForecast();

        [Get("/WeatherForecast")]
        Task<ResponseModel<WeatherForecastModel>> GetWeatherForecast2();
    }


    //public class ResponseJsonConverter<T> : JsonConverter<ResponseModel<T>>
    //{
    //    public override ResponseModel<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    //    {
            
    //        JsonSerializer.Deserialize<T>(ref reader, options)
    //        return new ResponseModel<T>();
    //    }

    //    public override void Write(Utf8JsonWriter writer, ResponseModel<T> value, JsonSerializerOptions options)
    //    {
            
    //    }
    //}

    public class MyJsonContext : JsonSerializerContext
    {
        public MyJsonContext(JsonSerializerOptions? options) : base(options)
        {
        }

        public override JsonTypeInfo? GetTypeInfo(Type type)
        {
            throw new NotImplementedException();
        }

        protected override JsonSerializerOptions? GeneratedSerializerOptions { get; }
    }

    public class ResponseModel<T>
    {
        public List<T> Data { get; set; }

        public int Code { get; set; }
    }

    public class WeatherForecastModel
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF { get; set; }

        public string Summary { get; set; }
    }

    public static class MosRestService
    {
        public static void For<T>( string hostUrl)
        {
        }
    }
}
