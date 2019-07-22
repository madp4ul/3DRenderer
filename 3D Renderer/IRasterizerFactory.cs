using Renderer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer
{
    /// <summary>
    /// create a rasterizer for each triangle
    /// </summary>
    public interface IRasterizerFactory
    {
        IRasterizer<TVertexInfo> CreateRasterizer<TVertexInfo>(Triangle<TVertexInfo> triangle);

        Vector2 ScreenSize { get; set; }

        Cullmode Cullmode { get; set; }
    }
}
