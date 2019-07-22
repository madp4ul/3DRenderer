using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer.Models
{
    public struct Vector2 : IEquatable<Vector2>
    {
        public double X { get; set; }
        public double Y { get; set; }

        public double Length { get { return Math.Sqrt(X * X + Y * Y); } }
        public double LengthSquared { get { return X * X + Y * Y; } }

        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vector2 Subtract(Vector2 vector)
        {
            return new Vector2(X - vector.X, Y - vector.Y);
        }

        public Vector2 Multiply(double scalar)
        {
            return new Vector2(X * scalar, Y * scalar);
        }

        public bool Equals(Vector2 other)
        {
            return X == other.X && Y == other.Y;
        }

        public override string ToString()
        {
            return "X: " + X + " Y: " + Y;
        }
    }
}
