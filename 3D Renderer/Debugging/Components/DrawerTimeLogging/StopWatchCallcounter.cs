using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer.Debugging.Components.DrawerTimeLogging
{
    class StopWatchCallcounter
    {
        private Stopwatch _Watch;
        public int Calls { get; private set; }

        public TimeSpan Elapsed { get { return _Watch.Elapsed; } }
        public bool Running { get { return _Watch.IsRunning; } }

        public StopWatchCallcounter()
        {
            _Watch = new Stopwatch();
        }

        public void Start()
        {
            Calls++;
            _Watch.Start();
        }

        public void Stop()
        {
            _Watch.Stop();
        }

    }
}
