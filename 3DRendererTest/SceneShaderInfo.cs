using Renderer;
using Renderer.Models;
using Renderer.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DRendererTest
{
    public class SceneConfig<TVertexIn, TVertexOut>
    {
        public VertexShaderBase<TVertexIn, TVertexOut> VertexShader { get; set; }
        public IPixelShader<TVertexOut> PixelShader { get; set; }
        public IVertexToPixelWeighter<TVertexOut> VertexWeighter { get; set; }
        public Cullmode Cullmode { get; set; }
    }
}
