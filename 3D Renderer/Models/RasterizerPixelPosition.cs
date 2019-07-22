using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer.Models
{
    public class RasterizerPixelPosition<TVertexInfo>
    {
        public RasterizerPixelInformation<TVertexInfo> RasterizerPixel { get; set; }
        public Vector3 Position { get; set; }
    }
}
