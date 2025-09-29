using Napps.Windows.Assessment.Services.Interfaces;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Napps.Windows.Assessment.Services
{
    internal class JsonSerializerService : IJsonSerializerService
    {
        public Task<T> DeserializeAsync<T>(string json)
        {
            return Task.Run(() => JsonConvert.DeserializeObject<T>(json));
        }

        public Task<string> SerializeAsync<T>(T obj)
        {
            return Task.Run(() => JsonConvert.SerializeObject(obj));
        }
    }
}