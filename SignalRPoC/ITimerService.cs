using System.Threading.Tasks;

namespace SignalRPoC
{
    public interface ITimerService
    {
        void StartTime(int newTime);
        Task ChangeTime(int newTime);
        Task StopTime();
        Task StartAgain();
    }
}