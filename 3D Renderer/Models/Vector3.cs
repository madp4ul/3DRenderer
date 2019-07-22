using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer.Models
{
    public struct Vector3
    {
        public static readonly Vector3 Zero = new Vector3(0, 0, 0);

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public double Length { get { return Math.Sqrt(X * X + Y * Y + Z * Z); } }

        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        private double GetLength()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public Vector3 Multiply(double scalar)
        {
            return new Vector3(X * scalar, Y * scalar, Z * scalar);
        }

        public Vector3 Subtraction(Vector3 vector)
        {
            return new Vector3(this.X - vector.X, this.Y - vector.Y, this.Z - vector.Z);
        }

        public double Dot(Vector3 vector)
        {
            return this.X * vector.X + this.Y * vector.Y + this.Z * vector.Z;
        }

        public Vector3 Cross(Vector3 vector)
        {

            return new Vector3(
                this.Y * vector.Z - this.Z * vector.Y,
                this.Z * vector.X - this.X * vector.Z,
                this.X * vector.Y - this.Y * vector.X);
        }

        public Vector3 Normalize()
        {
            double length = Length;
            Vector3 normalized = new Vector3(X / length, Y / length, Z / length);
            return normalized;
        }

        public Vector2 Get2D()
        {
            return new Vector2(X, Y);
        }

        public override string ToString()
        {
            return "X: " + X + " Y: " + Y + " Z: " + Z;
        }
    }
}
