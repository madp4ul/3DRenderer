using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer.Debugging.Components.DrawerTimeLogging
{
    class Timers
    {
        public StopWatchCallcounter TimerAll { get; set; }
        public StopWatchCallcounter TimerModelResolver { get; set; }
        public StopWatchCallcounter TimerVertexShader { get; set; }
        public StopWatchCallcounter TimerFilterValidTriangles { get; set; }
        public StopWatchCallcounter TimerWScaling { get; set; }
        public StopWatchCallcounter TimerScreenScaling { get; set; }
        public StopWatchCallcounter TimerRasterizer { get; set; }
        public StopWatchCallcounter TimerPositionWeighting { get; set; }
        public StopWatchCallcounter TimerDepthTest { get; set; }
        public StopWatchCallcounter TimerVertexweighting { get; set; }
        public StopWatchCallcounter TimerPixelShader { get; set; }
        public StopWatchCallcounter TimerPixelSpreader { get; set; }
        public StopWatchCallcounter TimerColorMixPreparation { get; set; }
        public StopWatchCallcounter TimerColorMixing { get; set; }
        public StopWatchCallcounter TimerWritingColors { get; set; }

        public Timers()
        {
            TimerAll = new StopWatchCallcounter();
            TimerModelResolver = new StopWatchCallcounter();
            TimerVertexShader = new StopWatchCallcounter();
            TimerFilterValidTriangles = new StopWatchCallcounter();
            TimerWScaling = new StopWatchCallcounter();
            TimerScreenScaling = new StopWatchCallcounter();
            TimerRasterizer = new StopWatchCallcounter();
            TimerPositionWeighting = new StopWatchCallcounter();
            TimerDepthTest = new StopWatchCallcounter();
            TimerVertexweighting = new StopWatchCallcounter();
            TimerPixelShader = new StopWatchCallcounter();
            TimerPixelSpreader = new StopWatchCallcounter();
            TimerColorMixPreparation = new StopWatchCallcounter();
            TimerColorMixing = new StopWatchCallcounter();
            TimerWritingColors = new StopWatchCallcounter();
        }

        public string GetTimes()
        {
            string result = "";

            result += (GetTimerString("All", t => t.TimerAll) + "\n");
            result += (GetTimerString("Modelresolver", t => t.TimerModelResolver) + "\n");
            result += (GetTimerString("Vertexshader", t => t.TimerVertexShader) + "\n");
            result += (GetTimerString("Filtervalid", t => t.TimerFilterValidTriangles) + "\n");
            result += (GetTimerString("W-Scaling", t => t.TimerWScaling) + "\n");
            result += (GetTimerString("Screenscaling", t => t.TimerScreenScaling) + "\n");
            result += (GetTimerString("Rasterizer", t => t.TimerRasterizer) + "\n");
            result += (GetTimerString("Positionweighting", t => t.TimerPositionWeighting) + "\n");
            result += (GetTimerString("Depthtest", t => t.TimerDepthTest) + "\n");
            result += (GetTimerString("VertexWeighting", t => t.TimerVertexweighting) + "\n");
            result += (GetTimerString("Pixelshader", t => t.TimerPixelShader) + "\n");
            result += (GetTimerString("Pixelspreader", t => t.TimerPixelSpreader) + "\n");
            result += (GetTimerString("Colormixing preparation", t => t.TimerColorMixPreparation) + "\n");
            result += (GetTimerString("Color mixing", t => t.TimerColorMixing) + "\n");
            result += (GetTimerString("Writing colors", t => t.TimerWritingColors) + "\n");

            return result;
        }

        private string GetTimerString(string name, Func<Timers, StopWatchCallcounter> selector)
        {
            StopWatchCallcounter selected = selector(this);

            if (selected.Running)
            {
                throw new InvalidOperationException();
            }

            return $@"{name}: {selected.Elapsed.TotalMilliseconds / selected.Calls} ms per call. {selected.Calls} calls in {selected.Elapsed.TotalMilliseconds}ms. ";
        }
    }
}
