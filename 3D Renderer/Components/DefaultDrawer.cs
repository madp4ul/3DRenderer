using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renderer.Models;
using Renderer.Components.WeighterHelpers;
using Renderer.Components.TargetAdapters;
using Renderer.Components.PixelSpreader;
using System.Drawing;

namespace Renderer.Components
{
    public class DefaultDrawer<TVertexIn, TVertexOut> : IDrawer<TVertexIn, TVertexOut>
    {
        #region pipeline step responsibilities
        public IDepthBuffer DepthBuffer { get; set; }

        public IModelResolver ModelResolver { get; set; }

        public IPixelShader<TVertexOut> PixelShader { get; set; }

        public IRasterizerFactory RasterizerFactory { get; set; }

        public IScreenScaler ScreenScaler { get; set; }

        public ITargetAdapter TargetAdapter { get; set; }

        public ITransparencyColorMixer TransparencyMixer { get; set; }

        public IVertexShader<TVertexIn, TVertexOut> VertexShader { get; set; }

        public IPositionWeighter PositionWeighter { get; set; }

        public IPixelSpreader PixelSpreader { get; set; }

        public IVertexToPixelWeighter<TVertexOut> VertexToPixelWeigher { get; set; }

        public IWScaler WScaler { get; set; }
        #endregion

        #region contructors

        public DefaultDrawer(
            IVertexShader<TVertexIn, TVertexOut> vertexShader,
            IPixelShader<TVertexOut> pixelShader,
            IVertexToPixelWeighter<TVertexOut> vpWeighter,
            IModelResolver modelResolver = null,
            IWScaler wScaler = null,
            IScreenScaler screenScaler = null,
            IRasterizerFactory rasterizerfactory = null,
            IPositionWeighter posWeighter = null,
            IDepthBuffer depthBuffer = null,
            IPixelSpreader pixelSpreader = null,
            ITransparencyColorMixer transparencyColorMixer = null,
            ITargetAdapter targetAdapter = null)
        {
            this.ModelResolver = modelResolver ?? new DefaultModelResolver();
            this.VertexShader = vertexShader;
            this.WScaler = WScaler ?? new DefaultWScaler();
            this.ScreenScaler = screenScaler ?? new DefaultScreenScaler();
            this.RasterizerFactory = rasterizerfactory ?? new DefaultRasterizerFactory();
            this.PositionWeighter = posWeighter ?? new Vector3Weighter();
            this.VertexToPixelWeigher = vpWeighter;
            this.DepthBuffer = depthBuffer ?? new DefaultDepthBuffer();
            this.PixelShader = pixelShader;
            this.PixelSpreader = pixelSpreader ?? new DefaultPixelSpreader();
            this.TransparencyMixer = transparencyColorMixer ?? new DefaultTransparencyColorMixer();
            this.TargetAdapter = targetAdapter ?? new DesktopImageAdapter();

            this.PixelSpreader.Active = false;
        }

        #endregion

        public virtual void Draw(Model<TVertexIn> model)
        {
            //set screensizes
            var screenSize = TargetAdapter.TargetSize;
            ScreenScaler.ScreenSize = screenSize;
            RasterizerFactory.ScreenSize = screenSize;
            DepthBuffer.ScreenSize = screenSize;
            PixelSpreader.ScreenSize = screenSize;

            //build triangle processing query
            IEnumerable<TriangleTransformation<TVertexIn>> triangles = GetResolvedModel(model);
            IEnumerable<TransformedTriangle<TVertexOut>> transformedTriangles = ApplyVertexShader(triangles);
            IEnumerable<TransformedTriangle<TVertexOut>> filteredTriangles = FilterValidTriangles(transformedTriangles);
            IEnumerable<Triangle<TVertexOut>> wScaledTriangles = ApplyWScaling(filteredTriangles);
            IEnumerable<Triangle<TVertexOut>> screenScaledTriangles = ApplyScreenScaling(wScaledTriangles);
            IEnumerable<RasterizerPixelInformation<TVertexOut>> pixels = ApplyRasterizer(screenScaledTriangles);
            IEnumerable<RasterizerPixelPosition<TVertexOut>> weightedPositionPixels = ApplyPositionWeighing(pixels);
            IEnumerable<RasterizerPixelPosition<TVertexOut>> visiblePixelPositions = DepthTest(weightedPositionPixels);
            IEnumerable<Vertex<TVertexOut>> weighedVisiblePixels = ApplyVertexWeighting(visiblePixelPositions);
            IEnumerable<PositionColor> positionColors = ApplyPixelShader(weighedVisiblePixels);
            IEnumerable<PixelColor> pixelColors = ApplyPixelSpreading(positionColors);
            IEnumerable<PixelColorOverride> positionColorOverrides = PrepareColorMixing(pixelColors);
            IEnumerable<PositionFinalColor> finalColors = ApplyColorMixing(positionColorOverrides);

            //process
            WriteColors(finalColors);
        }

        public virtual void Clear()
        {
            DepthBuffer.Clear();
            BeginDraw();
            TargetAdapter.Clear();
            //EndDraw();
        }

        #region overrideable enumerators for each step

        protected virtual IEnumerable<TriangleTransformation<TVertexIn>> GetResolvedModel(
            Model<TVertexIn> model)
        {
            return ModelResolver.Resolve(model);
        }

        protected virtual IEnumerable<TransformedTriangle<TVertexOut>> ApplyVertexShader(
            IEnumerable<TriangleTransformation<TVertexIn>> triangles)
        {
            return triangles.Select(ApplyVertexShader);
        }

        protected virtual IEnumerable<TransformedTriangle<TVertexOut>> FilterValidTriangles(
            IEnumerable<TransformedTriangle<TVertexOut>> transformedTriangles)
        {
            return transformedTriangles.Where(IsValidTriangle);
        }

        protected virtual IEnumerable<Triangle<TVertexOut>> ApplyWScaling(
            IEnumerable<TransformedTriangle<TVertexOut>> transformedTriangles)
        {
            return transformedTriangles.Select(ApplyWScaling);
        }

        protected virtual IEnumerable<Triangle<TVertexOut>> ApplyScreenScaling(
            IEnumerable<Triangle<TVertexOut>> wScaledTriangles)
        {
            return wScaledTriangles.Select(ApplyScreenScaling);
        }

        protected virtual IEnumerable<RasterizerPixelInformation<TVertexOut>> ApplyRasterizer(
            IEnumerable<Triangle<TVertexOut>> screenScaledTriangles)
        {
            return screenScaledTriangles.SelectMany(ApplyRasterizer);
        }

        protected virtual IEnumerable<RasterizerPixelPosition<TVertexOut>> ApplyPositionWeighing(
            IEnumerable<RasterizerPixelInformation<TVertexOut>> pixels)
        {
            return pixels.Select(ApplyPositionWeighing);
        }

        protected virtual IEnumerable<RasterizerPixelPosition<TVertexOut>> DepthTest(
            IEnumerable<RasterizerPixelPosition<TVertexOut>> weightedPositionPixels)
        {
            return weightedPositionPixels.Where(p => DepthBuffer.TrySetBuffer(p.Position));
        }

        protected virtual IEnumerable<Vertex<TVertexOut>> ApplyVertexWeighting(
                IEnumerable<RasterizerPixelPosition<TVertexOut>> visiblePixelPositions)
        {
            return visiblePixelPositions.Select(ApplyVertexWeighting);
        }

        protected virtual IEnumerable<PositionColor> ApplyPixelShader(
            IEnumerable<Vertex<TVertexOut>> weighedVisiblePixels)
        {
            return weighedVisiblePixels.Select(ApplyPixelShader);
        }

        protected virtual IEnumerable<PixelColor> ApplyPixelSpreading(IEnumerable<PositionColor> positionColors)
        {
            return positionColors.SelectMany(ApplyPixelSpreading);
        }

        protected virtual IEnumerable<PixelColorOverride> PrepareColorMixing(
            IEnumerable<PixelColor> positionColors)
        {
            return positionColors.Select(PrepareColorMixing);
        }

        protected virtual IEnumerable<PositionFinalColor> ApplyColorMixing(
            IEnumerable<PixelColorOverride> positionColorOverrides)
        {
            return positionColorOverrides.Select(ApplyColorMixing);
        }

        protected virtual void WriteColors(
            IEnumerable<PositionFinalColor> positionColors)
        {
            TargetAdapter.Begin();
            foreach (var colorPosition in positionColors)
            {
                WriteColor(colorPosition);
            }

            TargetAdapter.End();
        }
        #endregion

        #region apply pipeline step per dataunit methods

        protected TransformedTriangle<TVertexOut> ApplyVertexShader(TriangleTransformation<TVertexIn> triangleTransformation)
        {
            var tA = VertexShader.Apply(triangleTransformation.Triangle.A, triangleTransformation.WorldTransformation);
            var tB = VertexShader.Apply(triangleTransformation.Triangle.B, triangleTransformation.WorldTransformation);
            var tC = VertexShader.Apply(triangleTransformation.Triangle.C, triangleTransformation.WorldTransformation);

            return new TransformedTriangle<TVertexOut>(tA, tB, tC);
        }

        protected bool IsValidTriangle(
            TransformedTriangle<TVertexOut> triangle)
        {
            return triangle.A.Position.W != 0 && triangle.B.Position.W != 0 && triangle.C.Position.W != 0;
        }

        protected Triangle<TVertexOut> ApplyWScaling(TransformedTriangle<TVertexOut> triangle)
        {
            return new Triangle<TVertexOut>(
                         new Vertex<TVertexOut>(WScaler.GetScaled(triangle.A.Position), triangle.A.Information),
                         new Vertex<TVertexOut>(WScaler.GetScaled(triangle.B.Position), triangle.B.Information),
                         new Vertex<TVertexOut>(WScaler.GetScaled(triangle.C.Position), triangle.C.Information));
        }

        protected Triangle<TVertexOut> ApplyScreenScaling(Triangle<TVertexOut> triangle)
        {
            triangle.A.Position = ScreenScaler.GetScaled(triangle.A.Position);
            triangle.B.Position = ScreenScaler.GetScaled(triangle.B.Position);
            triangle.C.Position = ScreenScaler.GetScaled(triangle.C.Position);
            return triangle;
        }

        protected IEnumerable<RasterizerPixelInformation<TVertexOut>> ApplyRasterizer(Triangle<TVertexOut> triangle)
        {
            var rasterizer = RasterizerFactory.CreateRasterizer(triangle);

            while (rasterizer.MoreWork)
            {
                yield return rasterizer.GetNextPixel();
            }
        }

        protected RasterizerPixelPosition<TVertexOut> ApplyPositionWeighing(RasterizerPixelInformation<TVertexOut> pixelWeigh)
        {
            return new RasterizerPixelPosition<TVertexOut>
            {
                RasterizerPixel = pixelWeigh,
                Position = PositionWeighter.GetWeightedPosition(
                    pixelWeigh.VertexWeightA,
                    pixelWeigh.VertexWeightB,
                    pixelWeigh.VertexWeightC)
            };
        }

        protected Vertex<TVertexOut> ApplyVertexWeighting(RasterizerPixelPosition<TVertexOut> pixelPos)
        {
            return new Vertex<TVertexOut>(pixelPos.Position, VertexToPixelWeigher.GetWeightedInformation(
                pixelPos.RasterizerPixel.VertexWeightA,
                pixelPos.RasterizerPixel.VertexWeightB,
                pixelPos.RasterizerPixel.VertexWeightC));
        }

        protected PositionColor ApplyPixelShader(Vertex<TVertexOut> pixelInfo)
        {
            return PixelShader.GetPixelColor(pixelInfo);
        }

        protected IEnumerable<PixelColor> ApplyPixelSpreading(PositionColor positionColor)
        {
            return PixelSpreader.GetPixels(positionColor);
        }

        protected PixelColorOverride PrepareColorMixing(PixelColor pc)
        {
            return new PixelColorOverride(pc, () => TargetAdapter.Read(pc.PixelPosition));
        }

        protected PositionFinalColor ApplyColorMixing(PixelColorOverride positionColorOverride)
        {
            return new PositionFinalColor
            {
                Position = positionColorOverride.Position,
                FinalColor = TransparencyMixer
                .GetMixedColor(positionColorOverride.GetPreviousColor, positionColorOverride.OverrideColor)
            };
        }

        protected virtual void BeginDraw()
        {
            TargetAdapter.Begin();
        }

        protected virtual void WriteColor(PositionFinalColor posColor)
        {
            TargetAdapter.Write(posColor.Position, posColor.FinalColor);
        }

        protected virtual void EndDraw()
        {
            TargetAdapter.End();
        }

        public void Dispose()
        {
            TargetAdapter.Dispose();
        }
        #endregion
    }
}
