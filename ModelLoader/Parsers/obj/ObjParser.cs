using Renderer.VertexInformation.Empty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renderer.Models;
using System.Text.RegularExpressions;
using System.Globalization;

namespace ModelParser.Parsers.obj
{
    public abstract class ObjParser<TVertexInfo> : IModelParser<TVertexInfo>
    {
        protected const string PositionPraefix = "v";
        protected const string FaceIndexPraefix = "f";

        public Model<TVertexInfo> Parse(string text)
        {
            string[] lines = GetLines(text);

            var positions = ParsePositions(lines);
            var indices = ParseIndices(lines);
            var vertexInfo = GetVertexInfo(lines);

            var vertices = positions.Zip(vertexInfo, (pos, info) => new Vertex<TVertexInfo>(pos, info))
                .ToList().ToArray();

            return new Model<TVertexInfo>(vertices, indices);
        }

        protected virtual Vector3[] ParsePositions(string[] lines)
        {
            var positionLines = GetPraefixLines(lines, PositionPraefix).ToArray();
            Vector3[] result = new Vector3[positionLines.Length];

            for (int i = 0; i < positionLines.Length; i++)
            {
                string line = positionLines[i];
                string[] vectorComponents = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                if (vectorComponents.Length != 3)
                {
                    throw new InvalidOperationException("Model must be in 3D-Space. Found Position in "
                        + vectorComponents.Length + "D-Space.");
                }

                double[] vectorComponentResults = new double[vectorComponents.Length];

                for (int j = 0; j < vectorComponents.Length; j++)
                {
                    double parsed;
                    if (double.TryParse(vectorComponents[j], NumberStyles.Number, CultureInfo.InvariantCulture,
                        out parsed))
                    {
                        vectorComponentResults[j] = parsed;
                    }
                    else
                    {
                        throw new InvalidOperationException("Error parsing '" + vectorComponents[j] + "' as double.");
                    }
                }
                result[i] = new Vector3(vectorComponentResults[0], vectorComponentResults[1], vectorComponentResults[2]);
            }

            return result;
        }

        protected virtual int[] ParseIndices(string[] lines)
        {
            var indexLines = GetPraefixLines(lines, FaceIndexPraefix).ToArray();
            List<int> result = new List<int>();

            for (int i = 0; i < indexLines.Length; i++)
            {
                string line = indexLines[i];
                string[] faceIndexComponents = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                if (faceIndexComponents.Length != 3)
                {
                    throw new InvalidOperationException("All faces must have 3 vertices. Found face with "
                        + faceIndexComponents.Length + " vertices.");
                }

                for (int j = 0; j < faceIndexComponents.Length; j++)
                {
                    int parsed;
                    if (int.TryParse(faceIndexComponents[j], out parsed))
                    {
                        result.Add(parsed - 1);
                    }
                    else
                    {
                        throw new InvalidOperationException("Error parsing '" + faceIndexComponents[j] + "' as int.");
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// index from array has to match the indices in the position array
        /// </summary>
        /// <returns></returns>
        protected abstract TVertexInfo[] GetVertexInfo(string[] lines);

        private string[] GetLines(string text)
        {
            return text.Split(new string[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
        }

        protected IEnumerable<string> GetPraefixLines(string[] allLines, string praefix)
        {
            return allLines.Where(l => Regex.IsMatch(l, "^" + praefix))
                .Select(l => l.Substring(praefix.Length));
        }
    }
}
