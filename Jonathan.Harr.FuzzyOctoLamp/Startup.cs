using System;
using Jonathan.Harr.FuzzyOctoLamp;
using Jonathan.Harr.FuzzyOctoLamp.Service;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Jonathan.Harr.FuzzyOctoLamp;
public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddOptions<HueConfig>().Configure<IConfiguration>((settings, configuration) =>
        {
            configuration.GetSection("HueConfig").Bind(settings);
        });
        builder.Services.AddHttpClient<IHttpHueClient, HueHttpClient>(c =>
        {
            c.BaseAddress = new Uri("https://api.meethue.com/bridge/");
        });
        builder.Services.AddSingleton<ISetLampService, SetLampService>();
        
    }
}