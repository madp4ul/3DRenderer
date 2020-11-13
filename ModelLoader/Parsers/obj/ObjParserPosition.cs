using Renderer.VertexInformation.Empty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelParser.Parsers.obj
{
    public class ObjParserPosition : ObjParser<VertexEmpty>
    {
        protected override VertexEmpty[] GetVertexInfo(string[] lines)
        {
            return new VertexEmpty[GetPraefixLines(lines, PositionPraefix).Count()];
        }
    }
}
