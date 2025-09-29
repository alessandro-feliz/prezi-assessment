using System.Threading.Tasks;

namespace Napps.Windows.Assessment.Services.Interfaces
{
    public interface IFileSerializerService
    {
        Task SerializeAsync<T>(T obj, string filePath);
        Task<T> DeserializeAsync<T>(string filePath);
    }
}