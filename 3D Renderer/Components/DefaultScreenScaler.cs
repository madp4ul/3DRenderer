using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renderer.Models;

namespace Renderer.Components
{
    public class DefaultScreenScaler : IScreenScaler
    {
        private Vector2 _ScreenSize;
        public Vector2 ScreenSize
        {
            get { return _ScreenSize; }
            set
            {
                _ScreenSize = value;
                HalfScreenSize = _ScreenSize.Multiply(0.5);
            }
        }

        public Vector2 HalfScreenSize { get; private set; }

        public Vector3 GetScaled(Vector3 vector)
        {
            return new Vector3(
                vector.X * HalfScreenSize.X + HalfScreenSize.X,
                vector.Y * -HalfScreenSize.Y + HalfScreenSize.Y,
                vector.Z);
        }
    }
}
