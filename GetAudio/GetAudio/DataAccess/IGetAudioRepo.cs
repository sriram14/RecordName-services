using GetAudio.Models;

namespace GetAudio.DataAccess
{
    public interface IGetAudioRepo
    {
        string GetBlobURL(GetAudioRequest getAudioRequest);
    }
}
