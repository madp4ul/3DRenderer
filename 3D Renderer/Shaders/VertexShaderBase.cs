using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renderer.Models;

namespace Renderer.Shaders
{
    public abstract class VertexShaderBase<TVertexIn, TVertexOut> : IVertexShader<TVertexIn, TVertexOut>
    {
        private Matrix _ViewMatrix;
        private Matrix _ProjectionMatrix;

        public Matrix ViewMatrix
        {
            get { return _ViewMatrix; }
            set
            {
                _ViewMatrix = value;
                SetViewProjection();
            }
        }
        public Matrix ProjectionMatrix
        {
            get { return _ProjectionMatrix; }
            set
            {
                _ProjectionMatrix = value;
                SetViewProjection();
            }
        }

        protected Matrix ViewProjectionMatrix { get; private set; }

        public VertexShaderBase()
        {
            _ViewMatrix = Matrix.CreateIndentity();
            _ProjectionMatrix = Matrix.CreateIndentity();
            ViewProjectionMatrix = Matrix.CreateIndentity();
        }

        public abstract TransformedVertex<TVertexOut> Apply(Vertex<TVertexIn> vertex, Matrix worldTransformation);

        private void SetViewProjection()
        {
            ViewProjectionMatrix = ProjectionMatrix.Multiply(ViewMatrix);
        }
    }
}
