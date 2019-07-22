using ModelParser;
using Renderer.Models;
using Renderer.Shaders.DiffuseLighting;
using Renderer.VertexInformation.Color;
using Renderer.VertexInformation.Color.Normal;
using Renderer.VertexInformation.Empty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DRendererTest.Scenes
{
    class AffeScene : IScene<VertexColorNormal, VertexColorNormal>
    {
        protected Model<VertexColorNormal> _Affe;

        public double AffeRotationTimeSeconds { get; set; }

        private DiffuseLightingVertexShader _VShader;
        private Cullmode _Cullmode;

        private DiffuseLightingPixelShader _PShader;
        private VertexColorNormalWeighter _VWeighter;

        public AffeScene(IModelParser<VertexEmpty> modelParser, string affeRaw)
        {
            var model = modelParser.Parse(affeRaw);
            var colorized = GetColorized(model);
            _Affe = GetWithNormals(colorized);

            AffeRotationTimeSeconds = 20;

            _VShader = new DiffuseLightingVertexShader();
            _VShader.Culling = _Cullmode = Cullmode.Clockwise;
            _VShader.LightDirection = new Vector3(0, 0, -4);
            _VShader.AmbientLight = 0.3;

            _PShader = new DiffuseLightingPixelShader();

            _VWeighter = new VertexColorNormalWeighter();
        }

        protected virtual Model<VertexColor> GetColorized(Model<VertexEmpty> model)
        {
            int index = 0;
            double colorFaktor = 1d / (model.Vertices.Count() - 1);
            return model.Copy((p, v) => new VertexColor
            {
                Color = new Vector4(colorFaktor * index, 0, 1 - colorFaktor * index++, 1),
            });
        }

        protected virtual Model<VertexColorNormal> GetWithNormals(Model<VertexColor> colorized)
        {
            return colorized.Copy((t, p, v) =>
            {
                Vector3 direction1 = t.B.Position.Subtraction(t.A.Position);
                Vector3 direction2 = t.C.Position.Subtraction(t.A.Position);

                return new VertexColorNormal
                {
                    Color = v.Color,
                    Normal = direction1.Cross(direction2).Multiply(-1).Normalize(),
                };
            });
        }

        public virtual IEnumerable<SceneElement<VertexColorNormal>> Contents(TimeSpan time)
        {
            yield return new SceneElement<VertexColorNormal>
            {
                Model = GetAffe(time),
                View = GetViewMatrix(time),
                Projection = GetProjectionMatrix(time)
            };
        }

        protected virtual Model<VertexColorNormal> GetAffe(TimeSpan time)
        {
            _Affe.Transformation = Matrix.CreateRotationY(GetAffeRotationDegree(time));

            return _Affe;
        }

        protected virtual Matrix GetViewMatrix(TimeSpan time)
        {
            return Matrix.CreateLookAtView(new Vector3(0, 1, 2), Vector3.Zero, new Vector3(0, 1, 0));
        }

        protected virtual Matrix GetProjectionMatrix(TimeSpan time)
        {
            return Matrix.CreatePerspectiveProjection(60, 60, 0.1, 50);
        }

        public double GetAffeRotationDegree(TimeSpan time)
        {
            return 360 * time.TotalSeconds / AffeRotationTimeSeconds;
        }




        public virtual SceneConfig<VertexColorNormal, VertexColorNormal> GetShaderInfo()
        {
            return new SceneConfig<VertexColorNormal, VertexColorNormal>
            {
                VertexShader = _VShader,
                PixelShader = _PShader,
                VertexWeighter = _VWeighter,
                Cullmode = _Cullmode
            };
        }
    }
}
