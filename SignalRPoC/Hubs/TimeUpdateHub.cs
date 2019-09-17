using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace SignalRPoC.Hubs
{
    public class TimeUpdateHub : Hub
    {
        private readonly ITimerService _timerService;

        public TimeUpdateHub(ITimerService timerService)
        {
            _timerService = timerService;
        }

        public async Task ChangeTime(int newTime)
        {
            await _timerService.ChangeTime(newTime);
        }

        public async Task StopTime()
        {
            await _timerService.StopTime();
        }

        public async Task StartTime()
        {
            await _timerService.StartAgain();
        }


    }
}
