using FakeItEasy;
using FluentAssertions;
using Jonathan.Harr.FuzzyOctoLamp.Service;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jonathan.Harr.FuzzyOctoLamp.Tests;

[TestClass]
public class SetLampServiceTests_Unauthorized
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
            AccessToken = "some_invalid_access_token",
            ApiUser = "some_api_user",
            LightId = LightId
        });
        
        _setLampService = new SetLampService(_hueHttpClient, hueConfig);
        A.CallTo(() => _hueHttpClient.SendLightUpdate(A<float[]>._))
            .Throws(() => new Exception("An error occurred while trying to set the lamp color: Unauthorized"));
        _response = await _setLampService.SetLampToColor(XValue, YValue);
    }
    
    [TestMethod]
    public void Should_send_a_request_to_the_hue_api()
    {
        A.CallTo(() => _hueHttpClient.SendLightUpdate(A<float[]>._)).MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public void Should_return_an_unsuccessful_response()
    {
        _response.Should()
            .Be("An error occurred while trying to set the lamp color: Unauthorized");
    }
}