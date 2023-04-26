using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Jonathan.Harr.FuzzyOctoLamp.Service;

public class HueHttpClient : IHttpHueClient
{
    private static HttpClient _httpClient;
    private static string _apiUser;
    private static string _lightId;
    private static string _accessToken;
    
    public HueHttpClient(HttpClient httpClientFactory, IOptions<HueConfig> hueConfig)
    {
        _httpClient = httpClientFactory;
        _apiUser = hueConfig.Value.ApiUser;
        _lightId = hueConfig.Value.LightId;
        _accessToken = hueConfig.Value.AccessToken;
    }

    public async Task SendLightUpdate(float[] xyValues)
    {
        var url = $"{_apiUser}/lights/{_lightId}/state";
        var body = new
        {
            on = true,
            xy = xyValues,
            bri = 100,
            transitiontime = 0
        };
        
        var jsonBody = JsonConvert.SerializeObject(body);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        
        var response = await _httpClient.PutAsync(url, content);
        if (!response.IsSuccessStatusCode)
            throw new Exception($"An error occurred while trying to set the lamp color: {response.ReasonPhrase}");
    }
}