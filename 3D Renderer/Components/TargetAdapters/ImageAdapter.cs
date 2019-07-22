using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renderer.Models;
using System.Drawing;
using Renderer.Components.TargetAdapters.ImageAdapt;

namespace Renderer.Components.TargetAdapters
{
    public class ImageAdapter : ITargetAdapter
    {
        private Vector2 _TargetSize;
        public Vector2 TargetSize
        {
            get { return _TargetSize; }
            set
            {
                _TargetSize = value;
                ResetBitmap();
            }
        }

        public Image Image { get { return _CurrentTarget.Bitmap; } }
        private DirectBitmap _CurrentTarget;

        public ImageAdapter()
        {
            _TargetSize = new Vector2(100, 100);
            ResetBitmap();
        }

        public void Begin()
        {

        }

        public void End()
        {

        }

        public Vector3 Read(Point position)
        {
            int pixelOffset = (position.Y * _CurrentTarget.Width + position.X) * 3;

            byte red = _CurrentTarget.Bits[pixelOffset + 2];
            byte green = _CurrentTarget.Bits[pixelOffset + 1];
            byte blue = _CurrentTarget.Bits[pixelOffset];

            return new Vector3(red / 255d, green / 255d, blue / 255d);
        }

        public void Write(Point position, Vector3 color)
        {
            Point posInt = new Point((int)position.X, (int)position.Y);

            int pixelOffset = (posInt.Y * _CurrentTarget.Width + posInt.X) * 3;

            _CurrentTarget.Bits[pixelOffset + 2] = (byte)(color.X * 255);
            _CurrentTarget.Bits[pixelOffset + 1] = (byte)(color.Y * 255);
            _CurrentTarget.Bits[pixelOffset] = (byte)(color.Z * 255);
        }

        private void ResetBitmap()
        {
            if (_CurrentTarget != null)
            {
                _CurrentTarget.Dispose();
            }
            _CurrentTarget = new DirectBitmap((int)TargetSize.X, (int)TargetSize.Y);
        }

        public void Clear()
        {
            ResetBitmap();
        }

        public void Dispose()
        {
            if (_CurrentTarget != null)
            {
                _CurrentTarget.Dispose();
            }
        }


    }
}
