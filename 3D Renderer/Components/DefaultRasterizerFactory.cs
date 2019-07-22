using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renderer.Models;

namespace Renderer.Components
{
    public class DefaultRasterizerFactory : IRasterizerFactory
    {
        public Cullmode Cullmode { get; set; }

        public Vector2 ScreenSize { get; set; }

        public double Threshold { get; set; }

        public DefaultRasterizerFactory()
        {
            Threshold = 0;
        }

        public IRasterizer<TVertexInfo> CreateRasterizer<TVertexInfo>(Triangle<TVertexInfo> triangle)
        {
            return new DefaultRasterizer<TVertexInfo>(triangle, new Vector2(ScreenSize.X - 1, ScreenSize.Y - 1), Cullmode, Threshold);
        }
    }

    class DefaultRasterizer<TVertexInfo> : IRasterizer<TVertexInfo>
    {
        private readonly Triangle<TVertexInfo> _Triangle;
        private readonly Vector2 _ScreenSize;
        private readonly Cullmode _Cullmode;
        private readonly double _Threshold;

        //These values contain the rasterization progress
        private Vector2 CurrentPixel;
        private int A01, A12, A20, B01, B12, B20;
        private double W0Row, W1Row, W2Row;
        private double W0, W1, W2;
        private double WeightSum;

        //bounding box
        Vector2 BoxMin, BoxMax;

        public bool MoreWork { get; private set; }

        public DefaultRasterizer(Triangle<TVertexInfo> triangle, Vector2 screenSize, Cullmode cullmode, double threshold)
        {
            _Triangle = triangle;
            _ScreenSize = screenSize;
            _Cullmode = cullmode;
            _Threshold = threshold;

            MoreWork = true;
            _IsCurrentPixelWithinTriangle = GetIsCurrentPixelWithinTriangle();
            _OnNextPixel = GetOnNextPixel();
        }

        private Func<RasterizerPixelInformation<TVertexInfo>> _OnNextPixel;

        public RasterizerPixelInformation<TVertexInfo> GetNextPixel()
        {
            return _OnNextPixel();
        }

        private Func<RasterizerPixelInformation<TVertexInfo>> GetOnNextPixel()
        {
            SetBoundingBox();
            if (MoreWork)
                SetProgressValues();

            if (!MoreWork || !IsTriangleVisible())
            {
                MoreWork = false;
                return () =>
                {
                    throw new InvalidOperationException();
                };
            }
            else
            {
                SkipToNextTrianglePixel();

                return Rasterize;
            }
        }

        private RasterizerPixelInformation<TVertexInfo> Rasterize()
        {
            var result = new RasterizerPixelInformation<TVertexInfo>
            {
                VertexWeightA = new Weighted<Vertex<TVertexInfo>>
                {
                    Object = _Triangle.A,
                    Weight = W0 / WeightSum,
                },
                VertexWeightB = new Weighted<Vertex<TVertexInfo>>
                {
                    Object = _Triangle.B,
                    Weight = W1 / WeightSum,
                },
                VertexWeightC = new Weighted<Vertex<TVertexInfo>>
                {
                    Object = _Triangle.C,
                    Weight = W2 / WeightSum,
                },
            };

            SkipToNextTrianglePixel();

            return result;
        }

        private void SkipToNextTrianglePixel()
        {
            //loop ends when running out of pixels
            while (!CurrentPixel.Equals(BoxMax))
            {
                //if not end of line
                if (this.CurrentPixel.X < BoxMax.X)
                {
                    SetCurrentRight();
                }
                else
                {
                    if (this.CurrentPixel.Y < BoxMax.Y)
                    {
                        SetCurrentDown();
                    }
                    else
                    {
                        return;//finished
                    }
                }

                if (IsCurrentPixelWithinTriangle())
                {
                    WeightSum = W0 + W1 + W2;
                    if (WeightSum != 0)
                    {
                        return;
                    }
                }
            }
            //when not returning, no work is left
            MoreWork = false;
        }

        private void SetCurrentDown()
        {
            //increase currentpixel
            this.CurrentPixel = new Vector2(BoxMin.X, CurrentPixel.Y + 1);

            //set rowweight to one row below
            this.W0Row += B12;
            this.W1Row += B20;
            this.W2Row += B01;
            //set current weight to new rowweight
            this.W0 = W0Row;
            this.W1 = W1Row;
            this.W2 = W2Row;
        }

        private void SetCurrentRight()
        {
            //increase currentpixel
            this.CurrentPixel = new Vector2(CurrentPixel.X + 1, CurrentPixel.Y);
            ////set current weight one pixel to the right
            this.W0 += A12;
            this.W1 += A20;
            this.W2 += A01;
        }

        private Func<bool> _IsCurrentPixelWithinTriangle;
        public bool IsCurrentPixelWithinTriangle()
        {
            return _IsCurrentPixelWithinTriangle();
        }

        private Func<bool> GetIsCurrentPixelWithinTriangle()
        {
            if (_Cullmode == Cullmode.Clockwise)
            {
                return () => (W0 <= _Threshold && W1 <= _Threshold && W2 <= _Threshold);
            }
            else if (_Cullmode == Cullmode.CounterClockwise)
            {
                return () => (W0 >= _Threshold && W1 >= _Threshold && W2 >= _Threshold);
            }
            else
            {
                return () =>
                (W0 <= _Threshold && W1 <= _Threshold && W2 <= _Threshold)
                || (W0 >= _Threshold && W1 >= _Threshold && W2 >= _Threshold);
            }
        }

        private void SetProgressValues()
        {
            this.W0Row = OrientationTest(
                _Triangle.B.Position.Get2D(),
                _Triangle.C.Position.Get2D(),
                CurrentPixel);
            this.W1Row = OrientationTest(
                _Triangle.C.Position.Get2D(),
                _Triangle.A.Position.Get2D(),
                CurrentPixel);
            this.W2Row = OrientationTest(
                _Triangle.A.Position.Get2D(),
                _Triangle.B.Position.Get2D(),
                CurrentPixel);

            if (W0Row == 0 && W1Row == 0 && W2Row == 0)
            {
                //all weights will be NaN and Triangle can not be visible
                MoreWork = false;
                return;
            }

            //factors to add when moving along pixels
            this.A01 = (int)((_Triangle.A.Position.Y - _Triangle.B.Position.Y));
            this.B01 = (int)((_Triangle.B.Position.X - _Triangle.A.Position.X));
            this.A12 = (int)((_Triangle.B.Position.Y - _Triangle.C.Position.Y));
            this.B12 = (int)((_Triangle.C.Position.X - _Triangle.B.Position.X));
            this.A20 = (int)((_Triangle.C.Position.Y - _Triangle.A.Position.Y));
            this.B20 = (int)((_Triangle.A.Position.X - _Triangle.C.Position.X));

            this.W0 = W0Row;
            this.W1 = W1Row;
            this.W2 = W2Row;
        }

        private void SetBoundingBox()
        {
            //get triangle-boundingbox
            this.BoxMin = new Vector2(
                (int)Math.Floor(Math.Min(Math.Min(_Triangle.B.Position.X, _Triangle.C.Position.X), _Triangle.A.Position.X)),
                (int)Math.Floor(Math.Min(Math.Min(_Triangle.B.Position.Y, _Triangle.C.Position.Y), _Triangle.A.Position.Y)));
            this.BoxMax = new Vector2(
                (int)Math.Ceiling(Math.Max(Math.Max(_Triangle.B.Position.X, _Triangle.C.Position.X), _Triangle.A.Position.X)),
                (int)Math.Ceiling(Math.Max(Math.Max(_Triangle.B.Position.Y, _Triangle.C.Position.Y), _Triangle.A.Position.Y)));

            //clamp triangle-boundingbox to screen-box
            if (this.BoxMin.X < 0)
                this.BoxMin.X = 0;
            if (this.BoxMin.Y < 0)
                this.BoxMin.Y = 0;
            if (this.BoxMax.X >= this._ScreenSize.X)
                this.BoxMax.X = this._ScreenSize.X - 1;
            if (this.BoxMax.Y >= this._ScreenSize.Y)
                this.BoxMax.Y = this._ScreenSize.Y - 1;

            if (BoxMin.X >= this._ScreenSize.X)
                BoxMin.X = _ScreenSize.X - 1;
            if (BoxMin.Y >= this._ScreenSize.Y)
                BoxMin.Y = this._ScreenSize.Y - 1;

            if (BoxMin.Y > BoxMax.Y || BoxMin.X > BoxMax.X)
            {
                MoreWork = false;
            }

            this.CurrentPixel = BoxMin;
        }

        private double OrientationTest(Vector2 corner1, Vector2 corner2, Vector2 corner3)
        {
            return (corner2.X - corner1.X) * (corner3.Y - corner1.Y) -
                (corner2.Y - corner1.Y) * (corner3.X - corner1.X);
        }

        protected virtual bool IsTriangleVisible()
        {
            return _Cullmode == Cullmode.None
            || (new Ray2(_Triangle.A.Position.Get2D(), _Triangle.B.Position.Get2D())
                .RayDiff(_Triangle.C.Position.Get2D()) * (int)_Cullmode >= 0);

        }
    }
}
