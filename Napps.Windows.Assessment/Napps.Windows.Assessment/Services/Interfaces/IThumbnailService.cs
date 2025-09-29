using System.Threading.Tasks;

namespace Napps.Windows.Assessment.Services.Interfaces
{
    public interface IThumbnailService
    {
        Task<string> DownloadAndSaveAsync(string thumbnailUrl, string thumbnailName);
    }
}