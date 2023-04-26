using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Jonathan.Harr.FuzzyOctoLamp.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Jonathan.Harr.FuzzyOctoLamp;

public class SetHueLampColor
{
    // private readonly HttpClient httpClient = new();
    private readonly ISetLampService _setLampService;
    
    public SetHueLampColor(ISetLampService setLampService)
    {
        _setLampService = setLampService;
    }
    
    [FunctionName("SetHueLampColor")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
        HttpRequest req,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        string xValue = req.Query["xValue"];
        string yValue = req.Query["yValue"];

        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        dynamic data = JsonConvert.DeserializeObject(requestBody);
        xValue = xValue ?? data?.xValue;
        yValue = yValue ?? data?.yValue;

        if (string.IsNullOrEmpty(xValue) || string.IsNullOrEmpty(yValue))
        {
            xValue = "0.72";
            yValue = "0.52";
        }
        
        log.LogInformation("Setting lamp color to X={XValue}, Y={YValue}", xValue, yValue);
        
        if (!float.TryParse(xValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedXValue) ||
            !float.TryParse(yValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedYValue))
        {
            return new BadRequestObjectResult("Invalid xValue or yValue provided. Please provide valid float values.");
        }

        var result = "";
        try
        {
            result = await _setLampService.SetLampToColor(xValue, yValue);
            return new OkObjectResult(result);
        }
        catch (Exception e)
        {
            return new BadRequestObjectResult(
                result);
        }
    }
    // [FunctionName("SetHueLampColor")]
    // public async Task<IActionResult> Run(
    //     [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
    //     HttpRequest req,
    //     ILogger log)
    // {
    //     log.LogInformation("C# HTTP trigger function processed a request.");
    //
    //     string xValue = req.Query["xValue"];
    //     string yValue = req.Query["yValue"];
    //
    //     var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
    //     dynamic data = JsonConvert.DeserializeObject(requestBody);
    //     xValue = xValue ?? data?.xValue;
    //     yValue = yValue ?? data?.yValue;
    //
    //     if (string.IsNullOrEmpty(xValue) || string.IsNullOrEmpty(yValue))
    //     {
    //         xValue = "0.72";
    //         yValue = "0.52";
    //     }
    //     
    //     log.LogInformation("Setting lamp color to X={XValue}, Y={YValue}", xValue, yValue);
    //
    //     // Set your Hue API user and light ID
    //     var apiUser = "JkKLL75ApxQmtbqZdt79BeSYKE-519yZ7159q1Sk";
    //     var lightId = "10";
    //
    //     // Set the Bearer token
    //     var accessToken = "E1dkaQ17XNLIf774e0aSESNYC5A4";
    //
    //     // Build the API URL
    //     var apiUrl = $"https://api.meethue.com/bridge/{apiUser}/lights/{lightId}/state";
    //     
    //     var culture = System.Globalization.CultureInfo.InvariantCulture;
    //
    //     if (!float.TryParse(xValue, NumberStyles.Float, culture, out float parsedXValue) ||
    //         !float.TryParse(yValue, NumberStyles.Float, culture, out float parsedYValue))
    //     {
    //         return new BadRequestObjectResult("Invalid xValue or yValue provided. Please provide valid float values.");
    //     }
    //
    //
    //     var body = new
    //     {
    //         on = true,
    //         xy = new[] { parsedXValue, parsedYValue },
    //         bri = 100,
    //         transitiontime = 0
    //     };
    //
    //     var jsonBody = JsonConvert.SerializeObject(body);
    //     var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
    //
    //     // Set up the request headers
    //     httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    //
    //     // Send the request
    //     var response = await httpClient.PutAsync(apiUrl, content);
    //
    //     if (response.IsSuccessStatusCode)
    //         return new OkObjectResult(
    //             $"The color of the lamp with ID {lightId} was successfully set to X={xValue}, Y={yValue}.");
    //     return new BadRequestObjectResult(
    //         $"An error occurred while trying to set the lamp color: {response.ReasonPhrase}");
    // }
}