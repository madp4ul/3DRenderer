using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renderer.Models;

namespace Renderer.Components
{
    public class DefaultDepthBuffer : IDepthBuffer
    {
        private short[,] _Buffer;

        private Vector2 _ScreenSize;
        public Vector2 ScreenSize
        {
            get
            {
                return _ScreenSize;
            }
            set
            {
                if (!_ScreenSize.Equals(value))
                {
                    _ScreenSize = value;
                    ResetBuffer();
                }
            }
        }

        public double MaxDepth { get; set; }

        public DefaultDepthBuffer(double maxdepth = 1)
        {
            MaxDepth = maxdepth;
        }

        public void Clear()
        {
            ResetBuffer();
        }

        private void ResetBuffer()
        {
            _Buffer = new short[(int)ScreenSize.X, (int)ScreenSize.Y];
        }

        public bool TrySetBuffer(Vector3 position)
        {
            int bufferX = (int)position.X;
            int bufferY = (int)position.Y;
            //int bufferX = (int)Math.Round(position.X);
            //int bufferY = (int)Math.Round(position.Y);

            if (IsInBuffer(bufferX, bufferY))
            {

                short? positionBufferDepth = GetBufferDepth(position.Z);

                bool isVisible = positionBufferDepth.HasValue && positionBufferDepth.Value > _Buffer[bufferX, bufferY];

                if (isVisible)
                {
                    _Buffer[bufferX, bufferY] = positionBufferDepth.Value;
                }

                return isVisible;
            }
            else
                return false;
        }

        private bool IsInBuffer(int bufferX, int bufferY)
        {
            return bufferX >= 0 && bufferY >= 0 && bufferX < ScreenSize.X && bufferY < ScreenSize.Y;
        }

        private short? GetBufferDepth(double zPos)
        {
            if (zPos < 0 || zPos > MaxDepth)
            {
                return null;
            }
            else
            {
                return (short)((1 - zPos / MaxDepth) * short.MaxValue);
            }
        }
    }
}
