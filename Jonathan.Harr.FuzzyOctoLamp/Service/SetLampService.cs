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
    // private static HttpClient _httpClient;
    private readonly IHttpHueClient _hueClient;
    private static string _lightId;
    
    public SetLampService(IHttpHueClient hueClient, IOptions<HueConfig> hueConfig)
    {
        _hueClient = hueClient;
        _lightId = hueConfig.Value.LightId;
    }
    
    public async Task<string> SetLampToColor(string xValue, string yValue)
    {
        // TODO ersätt?
        // var apiUser = "JkKLL75ApxQmtbqZdt79BeSYKE-519yZ7159q1Sk";
        // var lightId = "10";

        // TODO ersätt?
        // var accessToken = "E1dkaQ17XNLIf774e0aSESNYC5A4";

        // var apiUrl = $"https://api.meethue.com/bridge/{_apiUser}/lights/{_lightId}/state";
        
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
        // var body = new
        // {
        //     on = true,
        //     xy = new[] { parsedXValue, parsedYValue },
        //     bri = 100,
        //     transitiontime = 0
        // };
        //
        // var jsonBody = JsonConvert.SerializeObject(body);
        // var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        // _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        //
        // var response = await _httpClient.PutAsync(apiUrl, content);
        //
        // return response.IsSuccessStatusCode ? $"The color of the lamp with ID {_lightId} was successfully set to X={xValue}, Y={yValue}." : $"An error occurred while trying to set the lamp color: {response.ReasonPhrase}";
    }
}