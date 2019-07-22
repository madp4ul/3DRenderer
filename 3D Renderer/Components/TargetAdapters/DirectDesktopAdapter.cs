using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renderer.Models;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Renderer.Components.TargetAdapters
{
    public class DirectDesktopAdapter : ITargetAdapter
    {
        [DllImport("User32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("User32.dll")]
        static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);

        private Vector2 _TargetSize;
        public virtual Vector2 TargetSize { get { return _TargetSize; } set { _TargetSize = value; } }

        private Graphics _Device;
        private IntPtr _DeviceHandle;

        public DirectDesktopAdapter()
        {
            _DeviceHandle = GetDC(IntPtr.Zero);
            _Device = Graphics.FromHdc(_DeviceHandle);
            _TargetSize = new Vector2(100, 100);
        }

        public virtual void Begin()
        {

        }

        public virtual void End()
        {

        }

        public virtual Vector3 Read(Point position)
        {
            return new Vector3(0, 0, 0);
        }

        public virtual void Write(Point position, Vector3 color)
        {
            _Device.DrawRectangle(
                new Pen(new SolidBrush(WriteHelper.ToColor(color))),
                new Rectangle((int)position.X, (int)position.Y, 1, 1));
        }

        public void WriteImage(Image image)
        {
            _Device.DrawImage(image, Point.Empty);
        }

        public virtual void Clear()
        {
            WriteImage(new Bitmap((int)TargetSize.X, (int)TargetSize.Y));
        }

        public virtual void Dispose()
        {
            _Device.Dispose();
            ReleaseDC(IntPtr.Zero, _DeviceHandle);
        }


    }
}
