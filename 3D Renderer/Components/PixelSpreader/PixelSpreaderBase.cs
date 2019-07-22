using Renderer.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer.Components.PixelSpreader
{
    public abstract class PixelSpreaderBase : IPixelSpreader
    {
        private bool _Active;
        public bool Active
        {
            get { return _Active; }
            set
            {
                _Active = value;
                SetGetPixels();
            }
        }

        public Vector2 ScreenSize { get; set; }

        public PixelSpreaderBase()
        {
            Active = false;
        }

        private void SetGetPixels()
        {
            if (!Active)
            {
                _GetPixels = (pc) =>
                {
                    return new[] {
                        new PixelColor
                        {
                            Color =pc.Color,
                            PixelPosition = new Point ((int)pc.Position.X,(int)pc.Position.Y)
                        },
                    };
                };
            }
            else
            {
                _GetPixels = GetSpreadedPixels;
            }
        }

        private IEnumerable<PixelColor> GetSpreadedPixels(PositionColor pc)
        {
            Point minPoint = new Point((int)pc.Position.X, (int)pc.Position.Y);
            Vector2 cutOff00 = pc.Position.Subtract(new Vector2(minPoint.X, minPoint.Y));

            double negCutOffY = 1 - cutOff00.Y;
            double negCutOffX = 1 - cutOff00.X;

            double faktor11 = GetFaktor(cutOff00);
            double faktor10 = GetFaktor(new Vector2(cutOff00.X, negCutOffY));
            double faktor01 = GetFaktor(new Vector2(negCutOffX, cutOff00.Y));
            double faktor00 = GetFaktor(new Vector2(negCutOffX, negCutOffY));

            double faktorSum = faktor00 + faktor01 + faktor10 + faktor11;

            int maxX = minPoint.X + 1;
            int maxY = minPoint.Y + 1;

            yield return new PixelColor
            {
                PixelPosition = minPoint,
                Color = pc.Color.Multiply(faktor00 / faktorSum),
            };
            yield return new PixelColor
            {
                PixelPosition = new Point(minPoint.X, maxY),
                Color = pc.Color.Multiply(faktor01 / faktorSum),
            };
            yield return new PixelColor
            {
                PixelPosition = new Point(maxX, minPoint.Y),
                Color = pc.Color.Multiply(faktor10 / faktorSum),
            };
            yield return new PixelColor
            {
                PixelPosition = new Point(maxX, maxY),
                Color = pc.Color.Multiply(faktor11 / faktorSum),
            };
        }

        protected abstract double GetFaktor(Vector2 cutOff);

        private Func<PositionColor, IEnumerable<PixelColor>> _GetPixels;
        public IEnumerable<PixelColor> GetPixels(PositionColor posColor)
        {
            return _GetPixels(posColor);
        }
    }
}
