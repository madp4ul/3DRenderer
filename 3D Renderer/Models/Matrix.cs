using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer.Models
{
    public struct Matrix : IEquatable<Matrix>
    {
        public double M11 { get; set; }
        public double M12 { get; set; }
        public double M13 { get; set; }
        public double M14 { get; set; }

        public double M21 { get; set; }
        public double M22 { get; set; }
        public double M23 { get; set; }
        public double M24 { get; set; }

        public double M31 { get; set; }
        public double M32 { get; set; }
        public double M33 { get; set; }
        public double M34 { get; set; }

        public double M41 { get; set; }
        public double M42 { get; set; }
        public double M43 { get; set; }
        public double M44 { get; set; }

        public Matrix(double[,] values)
            : this(values[0, 0], values[0, 1], values[0, 2], values[0, 3],
                 values[1, 0], values[1, 1], values[1, 2], values[1, 3],
                 values[2, 0], values[2, 1], values[2, 2], values[2, 3],
                 values[3, 0], values[3, 1], values[3, 2], values[3, 3])
        {
            if (values.GetLength(0) != 4 || values.GetLength(1) != 4)
            {
                throw new ArgumentException();
            }
        }

        public Matrix(double m11, double m12, double m13, double m14,
            double m21, double m22, double m23, double m24,
            double m31, double m32, double m33, double m34,
            double m41, double m42, double m43, double m44)
        {
            M11 = m11;
            M12 = m12;
            M13 = m13;
            M14 = m14;

            M21 = m21;
            M22 = m22;
            M23 = m23;
            M24 = m24;

            M31 = m31;
            M32 = m32;
            M33 = m33;
            M34 = m34;

            M41 = m41;
            M42 = m42;
            M43 = m43;
            M44 = m44;
        }

        public Matrix Multiply(Matrix n)
        {
            return new Matrix(
                M11 * n.M11 + M12 * n.M21 + M13 * n.M31 + M14 * n.M41,
                M11 * n.M12 + M12 * n.M22 + M13 * n.M32 + M14 * n.M42,
                M11 * n.M13 + M12 * n.M23 + M13 * n.M33 + M14 * n.M43,
                M11 * n.M14 + M12 * n.M24 + M13 * n.M34 + M14 * n.M44,

                M21 * n.M11 + M22 * n.M21 + M23 * n.M31 + M24 * n.M41,
                M21 * n.M12 + M22 * n.M22 + M23 * n.M32 + M24 * n.M42,
                M21 * n.M13 + M22 * n.M23 + M23 * n.M33 + M24 * n.M43,
                M21 * n.M14 + M22 * n.M24 + M23 * n.M34 + M24 * n.M44,

                M31 * n.M11 + M32 * n.M21 + M33 * n.M31 + M34 * n.M41,
                M31 * n.M12 + M32 * n.M22 + M33 * n.M32 + M34 * n.M42,
                M31 * n.M13 + M32 * n.M23 + M33 * n.M33 + M34 * n.M43,
                M31 * n.M14 + M32 * n.M24 + M33 * n.M34 + M34 * n.M44,

                M41 * n.M11 + M42 * n.M21 + M43 * n.M31 + M44 * n.M41,
                M41 * n.M12 + M42 * n.M22 + M43 * n.M32 + M44 * n.M42,
                M41 * n.M13 + M42 * n.M23 + M43 * n.M33 + M44 * n.M43,
                M41 * n.M14 + M42 * n.M24 + M43 * n.M34 + M44 * n.M44
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vertexPosition"></param>
        /// <returns></returns>
        public Vector4 Transform(Vector3 vertexPosition)
        {
            return new Vector4(
                M11 * vertexPosition.X + M12 * vertexPosition.Y + M13 * vertexPosition.Z + M14,
                M21 * vertexPosition.X + M22 * vertexPosition.Y + M23 * vertexPosition.Z + M24,
                M31 * vertexPosition.X + M32 * vertexPosition.Y + M33 * vertexPosition.Z + M34,
                M41 * vertexPosition.X + M42 * vertexPosition.Y + M43 * vertexPosition.Z + M44);
        }


        public override int GetHashCode()
        {
            return (int)M32 * 3 + (int)(1 + M41) * 5;
        }

        public bool Equals(Matrix other)
        {
            return GetHashCode() == other.GetHashCode()
                && M11 == other.M11
                && M12 == other.M12
                && M13 == other.M13
                && M14 == other.M14

                && M21 == other.M21
                && M22 == other.M22
                && M23 == other.M23
                && M24 == other.M24

                && M31 == other.M31
                && M32 == other.M32
                && M33 == other.M33
                && M34 == other.M34

                && M41 == other.M41
                && M42 == other.M42
                && M43 == other.M43
                && M44 == other.M44;
        }

        #region factory
        public static Matrix CreateIndentity()
        {
            return new Matrix(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
        }

        public static Matrix CreateRotationZ(double winkel)
        {
            double rad = ToRad(winkel);

            return new Matrix(new double[,]
            {
                {Math.Cos(rad),-Math.Sin(rad),0,0 },
                {Math.Sin(rad), Math.Cos(rad),0,0 },
                {0            , 0            ,1,0 },
                {0            , 0            ,0,1 }
            });
        }

        public static Matrix CreateRotationY(double winkel)
        {
            double rad = ToRad(winkel);

            return new Matrix(new double[,]
            {
                { Math.Cos(rad),0 ,Math.Sin(rad),0 },
                { 0            ,1 ,0            ,0 },
                {-Math.Sin(rad),0 ,Math.Cos(rad),0 },
                { 0            ,0 ,0            ,1 }
            });
        }

        public static Matrix CreateRotationX(double winkel)
        {
            double rad = ToRad(winkel);

            return new Matrix(new double[,]
            {
                {1, 0            , 0             , 0},
                {0, Math.Cos(rad),-Math.Sin(rad) , 0},
                {0, Math.Sin(rad), Math.Cos(rad) , 0},
                {0, 0            , 0             , 1 }
            });
        }

        public static Matrix CreateTranslation(double x, double y, double z)
        {
            return new Matrix(new double[,]
            {
                {1,0,0,x},
                {0,1,0,y},
                {0,0,1,z},
                {0,0,0,1}
            });
        }

        public static Matrix CreateScale(double x, double y, double z)
        {
            return new Matrix(new double[,]
            {
                {x,0,0,0},
                {0,y,0,0},
                {0,0,z,0},
                {0,0,0,1}
            });
        }

        public static Matrix CreateOrthographicProjection(double width, double height, double zNear, double zFar)
        {
            return new Matrix(new double[,]
            {
                {1/width,        0,                  0,                             0},
                {      0, 1/height,                  0,                             0},
                {      0,        0,  -(2/(zFar-zNear)),  -((zFar+zNear)/(zFar-zNear))},
                {      0,        0,                  0,                             1}
            });
        }

        public static Matrix CreatePerspectiveProjection(double fovX, double fovY, double zNear, double zFar)
        {
            double zDif = zFar - zNear;
            return new Matrix(new double[,]
            {
                {1/Math.Atan(ToRad(fovX/2)),                 0,                        0,                          0},
                {                0, 1/Math.Atan(ToRad(fovY/2)),                        0,                          0},
                {                0,                        0, -((zFar + zNear)/(zDif)), -((2*(zNear*zFar))/(zDif))},
                {                0,                        0,                       -1,                          0}
            });
        }

        public static Matrix CreateLookAtView(Vector3 position, Vector3 target, Vector3 up)
        {
            Vector3 zaxis = position.Subtraction(target).Normalize();
            Vector3 xaxis = up.Cross(zaxis).Normalize();
            Vector3 yaxis = zaxis.Cross(xaxis);

            return new Matrix(new double[,]
            {
                { xaxis.X, xaxis.Y, xaxis.Z, -xaxis.Dot(position)},
                { yaxis.X, yaxis.Y, yaxis.Z, -yaxis.Dot(position)},
                { zaxis.X, zaxis.Y, zaxis.Z, -zaxis.Dot(position)},
                {       0,       0,       0,                    1}
            });
        }

        public static Matrix CreateFPSView(Vector3 position, double pitch, double yaw)
        {

            pitch = ToRad(pitch);
            yaw = ToRad(yaw);

            double cosPitch = Math.Cos(pitch);
            double sinPitch = Math.Sin(pitch);
            double cosYaw = Math.Cos(yaw);
            double sinYaw = Math.Sin(yaw);

            Vector3 xaxis = new Vector3(cosYaw, 0, -sinYaw);
            Vector3 yaxis = new Vector3(sinYaw * sinPitch, cosPitch, cosYaw * sinPitch);
            Vector3 zaxis = new Vector3(sinYaw * cosPitch, -sinPitch, cosPitch * cosYaw);


            return new Matrix(new double[,]
            {
                {        xaxis.X,         xaxis.Y,         xaxis.Z, -xaxis.Dot(position)},
                {        yaxis.X,         yaxis.Y,         yaxis.Z, -yaxis.Dot(position)},
                {        zaxis.X,         zaxis.Y,         zaxis.Z, -zaxis.Dot(position)},
                {              0,               0,               0,                    1}
            });

        }

        private static double ToRad(double winkel)
        {
            return winkel * Math.PI / 180;
        }
        #endregion
    }
}
