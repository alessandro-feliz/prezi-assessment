using Napps.Windows.Assessment.Services.Interfaces;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

namespace Napps.Windows.Assessment.Services
{
    internal class BinarySerializerService : IFileSerializerService
    {
        public Task SerializeAsync<T>(T obj, string filePath, CancellationToken cancellationToken)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            EnsureDirectoryExists(filePath);

            return Task.Run(() =>
            {
                using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    new BinaryFormatter().Serialize(fs, obj);
                }
            }, cancellationToken);
        }

        public Task<T> DeserializeAsync<T>(string filePath, CancellationToken cancellationToken)
        {
            EnsureFileExists(filePath);

            return Task.Run(() =>
            {
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    return (T)new BinaryFormatter().Deserialize(fs);
                }
            }, cancellationToken);
        }

        private void EnsureFileExists(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found", filePath);
        }

        private void EnsureDirectoryExists(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));

            var fileDirectory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(fileDirectory))
                Directory.CreateDirectory(fileDirectory);
        }
    }
}