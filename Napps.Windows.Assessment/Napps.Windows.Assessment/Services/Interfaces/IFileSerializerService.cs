using System.Threading;
using System.Threading.Tasks;

namespace Napps.Windows.Assessment.Services.Interfaces
{
    public interface IFileSerializerService
    {
        Task SerializeAsync<T>(T obj, string filePath, CancellationToken cancellationToken);
        Task<T> DeserializeAsync<T>(string filePath, CancellationToken cancellationToken);
    }
}