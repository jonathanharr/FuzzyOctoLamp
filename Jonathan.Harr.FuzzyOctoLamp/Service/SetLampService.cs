using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Jonathan.Harr.FuzzyOctoLamp.Service;

public class SetLampService : ISetLampService
{ 
    private readonly IHttpHueClient _hueClient;
    private static string _lightId;
    
    public SetLampService(IHttpHueClient hueClient, IOptions<HueConfig> hueConfig)
    {
        _hueClient = hueClient;
        _lightId = hueConfig.Value.LightId;
    }
    
    public async Task<string> SetLampToColor(string xValue, string yValue)
    {
        var culture = CultureInfo.InvariantCulture;

        if (!float.TryParse(xValue, NumberStyles.Float, culture, out var parsedXValue) ||
            !float.TryParse(yValue, NumberStyles.Float, culture, out var parsedYValue))
        {
            throw new ArgumentException("Invalid xValue or yValue provided. Please provide valid float values.");
        }

        try
        {
            await _hueClient.SendLightUpdate(new[] { parsedXValue, parsedYValue });
        }
        catch (Exception e)
        {
            return e.Message;
        }
        
        return $"The color of the lamp with ID {_lightId} was successfully set to X={xValue}, Y={yValue}.";
    }
}