using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renderer.Models;
using System.Drawing;

namespace Renderer.Components.TargetAdapters
{
    public class DesktopImageAdapter : DirectDesktopAdapter
    {
        private ImageAdapter _ImageAdapter;
        private bool _CurrentlyDrawing;

        public override Vector2 TargetSize
        {
            get
            {
                return base.TargetSize;
            }

            set
            {
                base.TargetSize = value;
                _ImageAdapter.TargetSize = value;
            }
        }

        public DesktopImageAdapter()
        {
            _ImageAdapter = new ImageAdapter();
        }

        public override void Begin()
        {
            if (!_CurrentlyDrawing)
            {
                _CurrentlyDrawing = true;
                _ImageAdapter.Begin();

                base.Begin();
            }
        }

        public override void End()
        {
            if (!_CurrentlyDrawing)
                throw new InvalidOperationException("Begin before ending.");

            _CurrentlyDrawing = false;
            _ImageAdapter.End();
            this.WriteImage(_ImageAdapter.Image);
            base.End();
        }

        public override void Write(Point position, Vector3 color)
        {
            _ImageAdapter.Write(position, color);
        }

        public override Vector3 Read(Point position)
        {
            return _ImageAdapter.Read(position);
        }

        public override void Clear()
        {
            _ImageAdapter.Clear();
        }

        public override void Dispose()
        {
            _ImageAdapter.Dispose();
        }
    }
}
