using Renderer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer
{
    public interface IRasterizer<TVertexInfo>
    {
        bool MoreWork { get; }

        RasterizerPixelInformation<TVertexInfo> GetNextPixel();
    }

}
