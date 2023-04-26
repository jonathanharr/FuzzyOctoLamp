using FakeItEasy;
using FluentAssertions;
using Jonathan.Harr.FuzzyOctoLamp.Service;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jonathan.Harr.FuzzyOctoLamp.Tests;

[TestClass]
public class SetLampServiceTests_InvalidXYValues
{
    private const string XValue = "nollPunktFem";
    private const string YValue = "nollPunktFem";
    private const string LightId = "42";
    
    private ISetLampService _setLampService;

    private IHttpHueClient _hueHttpClient = A.Fake<IHttpHueClient>();

    private Func<Task> _setLampToColorAction;
    
    [TestInitialize]
    public async Task When_sending_an_invalid_xy_value()
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
        _setLampToColorAction = async () => await _setLampService.SetLampToColor(XValue, YValue);
    }
    
    [TestMethod]
    public void Should_not_have_been_able_to_send_req_to_api()
    {
        A.CallTo(() => _hueHttpClient.SendLightUpdate(A<float[]>._)).MustNotHaveHappened();
    }

    [TestMethod]
    public void Should_throw_exception()
    {
        _setLampToColorAction.Should()
            .ThrowAsync<ArgumentException>("Invalid xValue or yValue provided. Please provide valid float values.");
    }
}