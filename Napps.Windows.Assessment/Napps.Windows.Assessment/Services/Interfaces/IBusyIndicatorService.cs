namespace Napps.Windows.Assessment.Services.Interfaces
{
    public interface IBusyIndicatorService
    {
        void Show();
        void Hide();
        bool IsBusy { get; }
    }
}