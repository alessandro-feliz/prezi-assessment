using Napps.Windows.Assessment.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Napps.Windows.Assessment.Repositories.Presentations.Interfaces
{
    public interface IPresentationWriter
    {
        Task SaveAsync(IEnumerable<Presentation> presentations);
    }
}