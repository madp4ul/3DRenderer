using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renderer.Models;

namespace Renderer.Components
{
    /// <summary>
    /// returns all triangles in a model, including those of the childmodels with correct transformation matrices.
    /// </summary>
    public class DefaultModelResolver : IModelResolver
    {
        public IEnumerable<TriangleTransformation<TVertexInfo>> Resolve<TVertexInfo>(Model<TVertexInfo> model)
        {
            return Resolve(model, Matrix.CreateIndentity());
        }

        public IEnumerable<TriangleTransformation<TVertexInfo>> Resolve<TVertexInfo>(Model<TVertexInfo> model, Matrix parentTransformation)
        {
            Matrix currentTransformation = parentTransformation.Multiply(model.Transformation);

            foreach (var triangle in model)
            {
                triangle.WorldTransformation = currentTransformation;
                yield return triangle;
            }

            foreach (var child in model.Children.SelectMany(c => Resolve(c, currentTransformation)))
            {
                yield return child;
            }
        }

    }
}
