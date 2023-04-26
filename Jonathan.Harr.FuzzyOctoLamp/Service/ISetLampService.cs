using System.Threading.Tasks;

namespace Jonathan.Harr.FuzzyOctoLamp.Service;

public interface ISetLampService
{
    Task<string> SetLampToColor(string xValue, string yValue);
}