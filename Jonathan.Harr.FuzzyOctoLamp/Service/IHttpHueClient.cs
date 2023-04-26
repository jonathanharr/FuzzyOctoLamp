using System.Threading.Tasks;

namespace Jonathan.Harr.FuzzyOctoLamp.Service;

public interface IHttpHueClient
{
    Task SendLightUpdate(float[] xyValues);
}