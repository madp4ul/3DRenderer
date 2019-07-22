using Renderer.VertexInformation.Color.Normal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renderer.Models;

namespace Renderer.Shaders.DiffuseLighting
{
    public class DiffuseLightingVertexShader : VertexShaderBase<VertexColorNormal, VertexColorNormal>
    {
        private Vector3 _LightDirection;
        public Vector3 LightDirection
        {
            get { return _LightDirection; }
            set
            {
                _LightDirection = value.Normalize();
            }
        }
        public double LightIntensity { get; set; }

        private double _AmbientLight;
        private double _AntiAmbientLight;
        public double AmbientLight
        {
            get { return _AmbientLight; }
            set
            {
                _AmbientLight = value;
                _AntiAmbientLight = 1 - value;
            }
        }

        public Cullmode Culling { get; set; }

        public DiffuseLightingVertexShader()
        {
            LightDirection = new Vector3(0, -1, 0);
            LightIntensity = 1;
            AmbientLight = 0.1;
        }

        public override TransformedVertex<VertexColorNormal> Apply(Vertex<VertexColorNormal> vertex, Matrix worldTransformation)
        {
            Matrix wvp = this.ViewProjectionMatrix.Multiply(worldTransformation);

            Vector3 transformedNormal = worldTransformation.Transform(vertex.Information.Normal).Get3D();

            double diffuseIntensity = ShaderMath.Limit(transformedNormal.Dot(LightDirection.Multiply(-1 * (int)Culling)), 0, 1);

            Vector4 color = ShaderMath.MultiplyRGB(vertex.Information.Color,
                AmbientLight + (_AntiAmbientLight * LightIntensity * diffuseIntensity));

            var info = new VertexColorNormal
            {
                Color = color,
                Normal = transformedNormal,
            };

            return new TransformedVertex<VertexColorNormal>(
                wvp.Transform(vertex.Position), info);
        }
    }
}
