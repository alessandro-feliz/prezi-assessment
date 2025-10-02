using Napps.Windows.Assessment.Services.Interfaces;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Napps.Windows.Assessment.Services
{
    internal class JsonSerializerService : IJsonSerializerService
    {
        public Task<T> DeserializeAsync<T>(string json, CancellationToken cancellationToken)
        {
            return Task.Run(() => JsonConvert.DeserializeObject<T>(json), cancellationToken);
        }

        public Task<string> SerializeAsync<T>(T obj, CancellationToken cancellationToken)
        {
            return Task.Run(() => JsonConvert.SerializeObject(obj), cancellationToken);
        }
    }
}