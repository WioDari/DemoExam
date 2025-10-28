using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace DemoExam.Timer
{
    public class SessionTimer
    {
        private DispatcherTimer _timer;
        private TimeSpan _session;
        private DateTime _startTime;

        public event EventHandler sessionEnded;
        public event Action<TimeSpan> timeRemaining;

        public SessionTimer(TimeSpan session)
        {
            _session = session;
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += _timer_Tick;
        }

        public void Start()
        {
            _startTime = DateTime.Now;
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private void _timer_Tick(object? sender, EventArgs e)
        {
            TimeSpan r = _session - (DateTime.Now - _startTime); 
            if (r <= TimeSpan.Zero)
            {
                _timer.Stop();
                sessionEnded?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                timeRemaining?.Invoke(r);
            }
        }
    }
}
