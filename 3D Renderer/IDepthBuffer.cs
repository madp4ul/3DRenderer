using Renderer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer
{
    public interface IDepthBuffer
    {
        void Clear();
        bool TrySetBuffer(Vector3 position);
        Vector2 ScreenSize { get; set; }

        double MaxDepth { get; set; }
    }
}
