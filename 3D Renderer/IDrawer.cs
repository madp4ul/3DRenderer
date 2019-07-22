using Renderer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer
{
    public interface IDrawer<TVertexIn, TVertexOut> : IDisposable
    {
        IModelResolver ModelResolver { get; set; }
        IVertexShader<TVertexIn, TVertexOut> VertexShader { get; set; }
        IWScaler WScaler { get; set; }
        IScreenScaler ScreenScaler { get; set; }
        IRasterizerFactory RasterizerFactory { get; set; }
        IPositionWeighter PositionWeighter { get; set; }

        IDepthBuffer DepthBuffer { get; set; }

        IVertexToPixelWeighter<TVertexOut> VertexToPixelWeigher { get; set; }
        IPixelShader<TVertexOut> PixelShader { get; set; }
        IPixelSpreader PixelSpreader { get; set; }
        ITransparencyColorMixer TransparencyMixer { get; set; }
        ITargetAdapter TargetAdapter { get; set; }

        void Draw(Model<TVertexIn> model);

        void Clear();
    }
}
