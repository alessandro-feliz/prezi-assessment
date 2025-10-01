using System.Collections.Generic;

namespace Napps.Windows.Assessment.Domain
{
    public class PresentationsLoadResult
    {
        public Mode Mode { get; private set; }
        public IEnumerable<Presentation> Presentations { get; private set; }

        public PresentationsLoadResult(Mode mode, IEnumerable<Presentation> presentations)
        {
            Mode = mode;
            Presentations = presentations ?? new List<Presentation>();
        }
    }
}