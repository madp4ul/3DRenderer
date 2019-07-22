using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer.Models
{
    public struct Ray2
    {
        public Vector2 Start { get; set; }
        public Vector2 End { get; set; }

        public Ray2(Vector2 start, Vector2 end)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        /// check on which site of a ray a position lies.
        /// </summary>
        /// <param name="edgePos1">first point to define the ray</param>
        /// <param name="edgePos2">second point to define th ray</param>
        /// <param name="positionToTest">position on the left or right of the ray</param>
        /// <returns>double, 0 mean on ray, sign is equivalent to site of ray</returns>
        public double RayDiff(Vector2 positionToTest)
        {
            return (Start.Y - End.Y) * positionToTest.X +
                (End.X - Start.X) * positionToTest.Y +
                (Start.X * End.Y - Start.Y * End.X);
        }
    }
}
