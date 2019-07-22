using Renderer.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renderer.Models;
using System.Diagnostics;
using Renderer.Debugging.Components.DrawerTimeLogging;

namespace Renderer.Debugging.Components
{
    public class TimeLoggingDefaultDrawerDecorator<TVertexIn, TVertexOut> : DefaultDrawer<TVertexIn, TVertexOut>
    {
        public ILogger Logger { get; set; }

        private Timers _Timers;

        public TimeLoggingDefaultDrawerDecorator(IVertexShader<TVertexIn, TVertexOut> vertexShader, IPixelShader<TVertexOut> pixelShader, IVertexToPixelWeighter<TVertexOut> vpWeighter, IModelResolver modelResolver = null, IWScaler wScaler = null, IScreenScaler screenScaler = null, IRasterizerFactory rasterizerfactory = null, IPositionWeighter posWeighter = null, IDepthBuffer depthBuffer = null, IPixelSpreader pixelSpreader = null, ITransparencyColorMixer transparencyColorMixer = null, ITargetAdapter targetAdapter = null, ILogger logger = null) : base(vertexShader, pixelShader, vpWeighter, modelResolver, wScaler, screenScaler, rasterizerfactory, posWeighter, depthBuffer, pixelSpreader, transparencyColorMixer, targetAdapter)
        {
            Logger = logger ?? new ConsoleLogger();
            _Timers = new Timers();
        }

        public override void Draw(Model<TVertexIn> model)
        {
            _Timers.TimerAll.Start();
            base.Draw(model);
            _Timers.TimerAll.Stop();

            Logger.Log(_Timers.GetTimes());
        }

        public override void Clear()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            base.Clear();

            watch.Stop();

            Logger.Log("Cleartime:" + watch.ElapsedMilliseconds + "ms");
        }

        protected override IEnumerable<TriangleTransformation<TVertexIn>> GetResolvedModel(Model<TVertexIn> model)
        {
            var selector = DoMeasured<Model<TVertexIn>, IEnumerable<TriangleTransformation<TVertexIn>>>(
                base.GetResolvedModel, w => w.TimerVertexShader);

            return selector(model);
        }

        protected override IEnumerable<TransformedTriangle<TVertexOut>> ApplyVertexShader(IEnumerable<TriangleTransformation<TVertexIn>> triangles)
        {
            var selector = DoMeasured<IEnumerable<TriangleTransformation<TVertexIn>>, IEnumerable<TransformedTriangle<TVertexOut>>>(
                base.ApplyVertexShader, w => w.TimerVertexShader);

            return selector(triangles);
        }

        protected override IEnumerable<TransformedTriangle<TVertexOut>> FilterValidTriangles(IEnumerable<TransformedTriangle<TVertexOut>> transformedTriangles)
        {
            var selector = DoMeasured<IEnumerable<TransformedTriangle<TVertexOut>>, IEnumerable<TransformedTriangle<TVertexOut>>>(
            base.FilterValidTriangles, w => w.TimerFilterValidTriangles);

            return selector(transformedTriangles);
        }

        protected override IEnumerable<Triangle<TVertexOut>> ApplyWScaling(IEnumerable<TransformedTriangle<TVertexOut>> transformedTriangles)
        {
            var selector = DoMeasured<
                IEnumerable<TransformedTriangle<TVertexOut>>, IEnumerable<Triangle<TVertexOut>>>(
                base.ApplyWScaling, w => w.TimerWScaling);

            return selector(transformedTriangles);
        }

        protected override IEnumerable<Triangle<TVertexOut>> ApplyScreenScaling(IEnumerable<Triangle<TVertexOut>> wScaledTriangles)
        {
            var selector = DoMeasured<IEnumerable<Triangle<TVertexOut>>, IEnumerable<Triangle<TVertexOut>>>(
    base.ApplyScreenScaling, w => w.TimerScreenScaling);

            return selector(wScaledTriangles);
        }

        protected override IEnumerable<RasterizerPixelInformation<TVertexOut>> ApplyRasterizer(IEnumerable<Triangle<TVertexOut>> screenScaledTriangles)
        {
            var selector = DoMeasured<IEnumerable<Triangle<TVertexOut>>, IEnumerable<RasterizerPixelInformation<TVertexOut>>>(
base.ApplyRasterizer, w => w.TimerRasterizer);

            return selector(screenScaledTriangles);
        }

        protected override IEnumerable<RasterizerPixelPosition<TVertexOut>> ApplyPositionWeighing(IEnumerable<RasterizerPixelInformation<TVertexOut>> pixels)
        {
            var selector = DoMeasured<IEnumerable<RasterizerPixelInformation<TVertexOut>>, IEnumerable<RasterizerPixelPosition<TVertexOut>>>(
base.ApplyPositionWeighing, w => w.TimerPositionWeighting);

            return selector(pixels);
        }

        protected override IEnumerable<RasterizerPixelPosition<TVertexOut>> DepthTest(IEnumerable<RasterizerPixelPosition<TVertexOut>> weightedPositionPixels)
        {
            var selector = DoMeasured<IEnumerable<RasterizerPixelPosition<TVertexOut>>, IEnumerable<RasterizerPixelPosition<TVertexOut>>>(
base.DepthTest, w => w.TimerDepthTest);

            return selector(weightedPositionPixels);
        }

        protected override IEnumerable<Vertex<TVertexOut>> ApplyVertexWeighting(IEnumerable<RasterizerPixelPosition<TVertexOut>> visiblePixelPositions)
        {
            var selector = DoMeasured<IEnumerable<RasterizerPixelPosition<TVertexOut>>, IEnumerable<Vertex<TVertexOut>>>(
base.ApplyVertexWeighting, w => w.TimerVertexweighting);

            return selector(visiblePixelPositions);
        }

        protected override IEnumerable<PositionColor> ApplyPixelShader(IEnumerable<Vertex<TVertexOut>> weighedVisiblePixels)
        {
            var selector = DoMeasured<IEnumerable<Vertex<TVertexOut>>, IEnumerable<PositionColor>>(
base.ApplyPixelShader, w => w.TimerPixelShader);

            return selector(weighedVisiblePixels);
        }

        protected override IEnumerable<PixelColor> ApplyPixelSpreading(IEnumerable<PositionColor> positionColors)
        {
            var selector = DoMeasured<IEnumerable<PositionColor>, IEnumerable<PixelColor>>(
base.ApplyPixelSpreading, w => w.TimerPixelSpreader);

            return selector(positionColors);
        }

        protected override IEnumerable<PixelColorOverride> PrepareColorMixing(IEnumerable<PixelColor> positionColors)
        {
            var selector = DoMeasured<IEnumerable<PixelColor>, IEnumerable<PixelColorOverride>>(
base.PrepareColorMixing, w => w.TimerColorMixPreparation);

            return selector(positionColors);
        }

        protected override IEnumerable<PositionFinalColor> ApplyColorMixing(IEnumerable<PixelColorOverride> positionColorOverrides)
        {
            var selector = DoMeasured<IEnumerable<PixelColorOverride>, IEnumerable<PositionFinalColor>>(
base.ApplyColorMixing, w => w.TimerColorMixing);

            return selector(positionColorOverrides);
        }

        protected override void WriteColor(PositionFinalColor positionColors)
        {
            var selector = DoMeasured<PositionFinalColor, int>(p => { base.WriteColor(p); return 0; },
                w => w.TimerWritingColors);

            selector(positionColors);
        }

        private Func<Tin, Tout> DoMeasured<Tin, Tout>(Func<Tin, Tout> selector, Func<Timers, StopWatchCallcounter> watchSelector)
        {
            var timer = watchSelector(this._Timers);

            return new Func<Tin, Tout>(t =>
            {
                timer.Start();
                var result = selector(t);
                timer.Stop();
                return result;
            });
        }




    }
}
