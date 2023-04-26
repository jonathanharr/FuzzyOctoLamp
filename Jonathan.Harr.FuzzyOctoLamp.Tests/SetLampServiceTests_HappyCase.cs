using FakeItEasy;
using FluentAssertions;
using Jonathan.Harr.FuzzyOctoLamp.Service;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jonathan.Harr.FuzzyOctoLamp.Tests;

[TestClass]
public class SetLampServiceTests_HappyCase
{
    private const string XValue = "0.5";
    private const string YValue = "0.5";
    private const string LightId = "42";
    private ISetLampService _setLampService;

    private IHttpHueClient _hueHttpClient = A.Fake<IHttpHueClient>();

    private string _response = "";
    
    [TestInitialize]
    public async Task When_sending_a_valid_xy_value()
    {
        var hueConfig = Options.Create(new HueConfig
        {
            AccessToken = "some_access_token",
            ApiUser = "some_api_user",
            LightId = LightId
        });
        
        _setLampService = new SetLampService(_hueHttpClient, hueConfig);
        A.CallTo(() => _hueHttpClient.SendLightUpdate(A<float[]>._))
            .Returns(Task.CompletedTask);
        _response = await _setLampService.SetLampToColor(XValue, YValue);
    }
    
    [TestMethod]
    public void Should_send_a_request_to_the_hue_api()
    {
        A.CallTo(() => _hueHttpClient.SendLightUpdate(A<float[]>._)).MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public void Should_return_a_successful_response()
    {
        _response.Should()
            .Be($"The color of the lamp with ID {LightId} was successfully set to X={XValue}, Y={YValue}.");
    }
}
