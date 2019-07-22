using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer.Models
{
    public class Model<TVertexInfo> : IEnumerable<TriangleTransformation<TVertexInfo>>
    {
        public virtual Matrix Transformation { get; set; } //TODO wenn geändert, kann transformation in lazy triangletranformations falsch sein
        public IEnumerable<Model<TVertexInfo>> Children { get; set; }

        private Vertex<TVertexInfo>[] _Vertices;
        private int[] _Indices;
        private Lazy<TriangleTransformation<TVertexInfo>[]> _Triangles;

        public IEnumerable<Vertex<TVertexInfo>> Vertices { get { return _Vertices; } }
        public IEnumerable<int> Indices { get { return _Indices; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="indices">3 indices are for one triangle</param>
        public Model(Vertex<TVertexInfo>[] vertices, int[] indices,
            Matrix transformation = default(Matrix),
            IEnumerable<Model<TVertexInfo>> children = null)
        {
            if (indices.Length % 3 != 0)
            {
                throw new ArgumentException("Indices length must be dividable by 3.");
            }

            _Vertices = vertices;
            _Indices = indices;

            Transformation = default(Matrix).Equals(transformation) ? Matrix.CreateIndentity() : transformation;
            Children = children ?? Enumerable.Empty<Model<TVertexInfo>>();

            _Triangles = new Lazy<TriangleTransformation<TVertexInfo>[]>(GetTriangles);
        }

        private TriangleTransformation<TVertexInfo>[] GetTriangles()
        {
            TriangleTransformation<TVertexInfo>[] triangles = new TriangleTransformation<TVertexInfo>[_Indices.Length / 3];

            int i0 = 0;
            int i1 = 0;
            int i2 = 0;

            int triangleCount = 0;
            for (int i = 0; i < _Indices.Length; i++)
            {
                int curIndex = _Indices[i];
                switch (i % 3)
                {

                    case 0:
                        {
                            i0 = curIndex;
                            break;
                        }

                    case 1:
                        {
                            i1 = curIndex;
                            break;
                        }
                    case 2:
                        {
                            i2 = curIndex;
                            triangles[triangleCount++] = new TriangleTransformation<TVertexInfo>
                            {

                                Triangle = new Triangle<TVertexInfo>(
                                _Vertices[i0],
                                _Vertices[i1],
                                _Vertices[i2]),
                                WorldTransformation = Transformation
                            };
                            break;
                        }
                    default:
                        break;
                }
            }
            return triangles;
        }

        public virtual Model<TVertexInfo> Copy()
        {
            return Copy((p, v) => v);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TVertexInfoResult"></typeparam>
        /// <param name="copyInfoFunction">function gives position:vector3 and VertexInfo:TVertexInfo to create another vertexInfotype from that data</param>
        /// <returns></returns>
        public virtual Model<TVertexInfoResult> Copy<TVertexInfoResult>(
            Func<Vector3, TVertexInfo, TVertexInfoResult> copyInfoFunction)
        {
            int[] indices = new int[_Indices.Length];
            _Indices.CopyTo(indices, 0);

            return new Model<TVertexInfoResult>(_Vertices
                .Select(v => new Vertex<TVertexInfoResult>(v.Position,
                copyInfoFunction(v.Position, v.Information))).ToArray(),
                indices,
                Transformation);
        }

        /// <summary>
        /// Copyfunction provides Triangle so the face can be accessed to get information, but the downside is
        /// that the new model is stored in an less efficient way because Vertices may be stored multiple times.
        /// </summary>
        /// <typeparam name="TVertexInfoResult"></typeparam>
        /// <param name="copyInfoFunction">function gives triangle=the containing triangle, position=vector3 and VertexInfo=TVertexInfo to create another vertexInfotype from that data</param>
        /// <returns></returns>
        public virtual Model<TVertexInfoResult> Copy<TVertexInfoResult>(
            Func<Triangle<TVertexInfo>, Vector3, TVertexInfo, TVertexInfoResult> copyInfoFunction)
        {
            var vertices = _Triangles.Value.SelectMany(t => new[] { t.Triangle.A, t.Triangle.B, t.Triangle.C }
                .Select(v => new Vertex<TVertexInfoResult>(v.Position,
                copyInfoFunction(t.Triangle, v.Position, v.Information)))).ToArray();

            int[] indices = new int[vertices.Length];
            for (int i = 0; i < indices.Length; i++)
                indices[i] = i;

            return new Model<TVertexInfoResult>(
                vertices,
                indices,
                Transformation);
        }

        public IEnumerator<TriangleTransformation<TVertexInfo>> GetEnumerator()
        {
            foreach (var item in _Triangles.Value)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


    }
}
