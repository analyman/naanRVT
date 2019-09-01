using System;
using System.Collections.Generic;
using System.Linq;

using Autodesk.DesignScript.Runtime;
using Autodesk.DesignScript.Geometry;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;

using Newtonsoft.Json;

using NaaN.dynamo.Geometry;

namespace NaaN {
    public static class Geometry{
        public static List<List<List<Point>>> DiamondPointsN(int xnum, int ynum, double xdis, double ydis)
        {
            NaaN.dynamo.Geometry.DiamondPoints xdia = new DiamondPoints(xnum, ynum, xdis, ydis);
            var retx = utils.multiLevelMapList((object x) => 
            {
                CSPoint pt = x as CSPoint;
                return Point.ByCoordinates(pt.X, pt.Y, pt.Z);
            }, xdia.AllPanels);
            List<List<List<Point>>> ret = retx as List<List<List<Point>>>;
            return ret;
        }
    }
}