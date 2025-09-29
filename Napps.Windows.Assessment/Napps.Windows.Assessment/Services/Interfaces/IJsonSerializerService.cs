using System.Threading.Tasks;

namespace Napps.Windows.Assessment.Services.Interfaces
{
    public interface IJsonSerializerService
    {
        Task<string> SerializeAsync<T>(T obj);
        Task<T> DeserializeAsync<T>(string json);
    }
}