using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renderer.Models;
using System.Drawing;

namespace Renderer.Components.PixelSpreader
{
    public class DefaultPixelSpreader : IPixelSpreader
    {
        public bool Active { get; set; }

        public Vector2 ScreenSize { get; set; }

        public IEnumerable<PixelColor> GetPixels(PositionColor posColor)
        {
            Point minPoint = new Point((int)posColor.Position.X, (int)posColor.Position.Y);
            int maxX = minPoint.X + 1;
            int maxY = minPoint.Y + 1;

            bool drawright = maxX < ScreenSize.X;
            bool drawdown = maxY < ScreenSize.Y;
            bool drawdownright = drawright && drawdown;

            yield return new PixelColor
            {
                PixelPosition = minPoint,
                Color = posColor.Color,
            };
            if (drawdown)
                yield return new PixelColor
                {
                    PixelPosition = new Point(minPoint.X, maxY),
                    Color = posColor.Color,
                };
            if (drawright)
                yield return new PixelColor
                {
                    PixelPosition = new Point(maxX, minPoint.Y),
                    Color = posColor.Color,
                };
            if (drawdownright)
                yield return new PixelColor
                {
                    PixelPosition = new Point(maxX, maxY),
                    Color = posColor.Color,
                };
        }
    }
}
