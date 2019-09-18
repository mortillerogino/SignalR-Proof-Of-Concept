using Microsoft.AspNetCore.SignalR;
using SignalRPoC.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace SignalRPoC
{
    public class TimerService : ITimerService
    {
        private readonly IHubContext<TimeUpdateHub> _hubContext;
        private Timer _timer;
        private int _currentInterval;
        private bool _isStarted;

        public TimerService(IHubContext<TimeUpdateHub> hubContext)
        {
            _hubContext = hubContext;
            _currentInterval = 5000;
            StartTime(_currentInterval);
        }

        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await _hubContext.Clients.All.SendAsync("TimeUpdate", "Step", $"Time update: {DateTime.Now}");
        }

        public void StartTime(int newTime)
        {
            if (_isStarted)
            {
                return;
            }
            _isStarted = true;
            _timer = new Timer(newTime);
            _timer.Elapsed += Timer_Elapsed;
            _timer.AutoReset = true;
            _timer.Enabled = true;

        }

        public async Task ChangeTime(int newTime)
        {
            var newTimeValue = newTime * 1000;

            if (newTimeValue == _currentInterval)
            {
                return;
            }

            _timer.Interval = _currentInterval = newTimeValue;

            string unit = newTime != 1 ? "seconds" : "second";
            await _hubContext.Clients.All.SendAsync("TimeUpdate", "Info", $"Update Interval changed to {newTime} {unit}");
        }

        public async Task StopTime()
        {
            if (!_isStarted)
            {
                return;
            }
            _timer.Stop();
            _isStarted = false;

            await _hubContext.Clients.All.SendAsync("TimeUpdate", "Stop", $"Time stopped at {DateTime.Now}");
        }

        public async Task StartAgain()
        {
            if (_isStarted)
            {
                return;
            }
            _timer.Start();
            _isStarted = true;

            await _hubContext.Clients.All.SendAsync("TimeUpdate", "Start", $"Time started at {DateTime.Now}");
        }
    }
}
