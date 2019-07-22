using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer.Models
{
    public class PixelColorOverride
    {
        private PixelColor _PC;

        public Point Position { get { return _PC.PixelPosition; } }

        public Vector4 OverrideColor { get { return _PC.Color; } }

        public Func<Vector3> GetPreviousColor { get; private set; }

        public PixelColorOverride(PixelColor pc, Func<Vector3> getPrevColor)
        {
            _PC = pc;
            GetPreviousColor = getPrevColor;
        }
    }
}
