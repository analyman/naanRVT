using System;
using System.Collections.Generic;
using System.Linq;

using Autodesk.DesignScript.Runtime;
using Autodesk.DesignScript.Geometry;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;

namespace NaaN.dynamo.Nodes
{
    [NodeName("DiamondPoints")]
    [NodeCategory("NaaN.Geometry")]
    [NodeDescription("建立菱形表面")]
    [IsDesignScriptCompatible]
    [InPortNames("XNUM", "YNUM", "XDIS", "YDIS")]
    [InPortTypes("int", "int", "double", "double")]
    [InPortDescriptions("", "", "", "")]
    [OutPortNames("YES")]
    [OutPortTypes("string")]
    public class DiamondPointsNode: NodeModel
    {
        public DiamondPointsNode()
        {
            RegisterAllPorts();
        }

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            var functionCall =
              AstFactory.BuildFunctionCall(
                (int x, int y) => { return x * y; },
                new List<AssociativeNode> { inputAstNodes[0], inputAstNodes[1] });

            return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall) };
        }
    }
}
