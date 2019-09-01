using System;
using System.Collections.Generic;
using System.Linq;

using Autodesk.DesignScript.Runtime;
using Autodesk.DesignScript.Geometry;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;

using Newtonsoft.Json;

using NaaN.dynamo.Geometry;

namespace NaaN
{
    [NodeName("DiamondPointsNode")]
    [NodeCategory("NaaN.Geometry")]
    [NodeDescription("建立菱形表面")]
    [IsDesignScriptCompatible]
    [InPortNames("XNUM", "YNUM", "XDIS", "YDIS")]
    [InPortTypes("int", "int", "double", "double")]
    [InPortDescriptions("", "", "", "")]
    [OutPortNames("YES")]
    [OutPortTypes("int")]
    public class DiamondPointsNode: NodeModel
    {
        #region  constructor
        public DiamondPointsNode()
        {
            InPorts.Add(new PortModel(PortType.Input, this, new PortData("XNUM", "Number of grid cross x axis")));
            InPorts.Add(new PortModel(PortType.Input, this, new PortData("XDIS", "An interval cross x axis")));
            InPorts.Add(new PortModel(PortType.Input, this, new PortData("YNUM", "Number of grid cross y axis")));
            InPorts.Add(new PortModel(PortType.Input, this, new PortData("YDIS", "An interval cross x axis")));
            InPorts.Add(new PortModel(PortType.Output, this, new PortData("Yes", "Nothing")));
            RegisterAllPorts();
        }

        [JsonConstructor]
        public DiamondPointsNode(IEnumerable<PortModel> inputPort, IEnumerable<PortModel> outputPort) : base(inputPort, outputPort) { }
        #endregion

        public int helloworld(int x){
            return 1000;
        }

        [IsVisibleInDynamoLibrary(false)]
        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            var functionCall =
              AstFactory.BuildFunctionCall(
                (int x, int y) => { return x * y; },
                new List<AssociativeNode> { inputAstNodes[0], inputAstNodes[1] });

            return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall) };
        }
    }
    
    [NodeName("Hello")]
    [NodeCategory("NaaN.Geometry")]
    [NodeDescription("NO")]
    [IsDesignScriptCompatible]
    [OutPortNames("YES")]
    [OutPortTypes("int")]
    public class XYN: NodeModel 
    {
        private int x, y, z;
        public XYN(){
            this.x = 0;
            this.y = 0;
            this.z = 0;
            this.x = this.y * this.z;
            this.z = this.y * this.x;
            this.y = this.x * this.z;
            OutPorts.Add(new PortModel(PortType.Output, this, new PortData("Yes", "ENheng")));
        }
    }
}