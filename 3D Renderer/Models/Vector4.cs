using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer.Models
{
    public struct Vector4
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double W { get; set; }

        public Vector4(double x, double y, double z, double w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Vector4 Multiply(double scalar)
        {
            return new Vector4(X * scalar, Y * scalar, Z * scalar, W * scalar);
        }

        public Vector3 Get3D()
        {
            return new Vector3(X, Y, Z);
        }

        public override string ToString()
        {
            return "X: " + X + " Y: " + Y + " Z: " + Z + " W: " + W;
        }
    }
}
