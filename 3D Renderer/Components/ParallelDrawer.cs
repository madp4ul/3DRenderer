using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renderer.Models;

namespace Renderer.Components
{
    public class ParallelDrawer<TVertexIn, TVertexOut> : DefaultDrawer<TVertexIn, TVertexOut>
    {
        public ParallelDrawer(IVertexShader<TVertexIn, TVertexOut> vertexShader, IPixelShader<TVertexOut> pixelShader, IVertexToPixelWeighter<TVertexOut> vpWeighter, IModelResolver modelResolver = null, IWScaler wScaler = null, IScreenScaler screenScaler = null, IRasterizerFactory rasterizerfactory = null, IPositionWeighter posWeighter = null, IDepthBuffer depthBuffer = null, IPixelSpreader pixelSpreader = null, ITransparencyColorMixer transparencyColorMixer = null, ITargetAdapter targetAdapter = null) : base(vertexShader, pixelShader, vpWeighter, modelResolver, wScaler, screenScaler, rasterizerfactory, posWeighter, depthBuffer, pixelSpreader, transparencyColorMixer, targetAdapter)
        {
        }

        protected override IEnumerable<RasterizerPixelInformation<TVertexOut>> ApplyRasterizer(IEnumerable<Triangle<TVertexOut>> screenScaledTriangles)
        {
            //return base.ApplyRasterizer(screenScaledTriangles);
            //var parallel = base.ApplyRasterizer(screenScaledTriangles
            //    .AsParallel());
            var parallel = screenScaledTriangles.AsParallel().SelectMany(ApplyRasterizer).AsSequential();

            return parallel;
        }

        protected override IEnumerable<RasterizerPixelPosition<TVertexOut>> ApplyPositionWeighing(IEnumerable<RasterizerPixelInformation<TVertexOut>> pixels)
        {
            return base.ApplyPositionWeighing(pixels);
        }

        protected override void BeginDraw()
        {
            if (FinalizeDrawTask != null)
                Task.WaitAll(FinalizeDrawTask);
            base.BeginDraw();
        }

        private Task FinalizeDrawTask;
        protected override void WriteColors(IEnumerable<PositionFinalColor> positionColors)
        {
            var pixels = positionColors.ToList();

            //wait for last draw
            BeginDraw();

            foreach (var pc in pixels)
            {
                WriteColor(pc);
            }

            EndDraw();
        }

        protected override void EndDraw()
        {
            FinalizeDrawTask = Task.Run(() => base.EndDraw());
        }
    }
}
