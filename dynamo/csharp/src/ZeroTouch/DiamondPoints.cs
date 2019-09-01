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
    public class DPoints
    {
        private DiamondPoints dp;
        public DPoints(int xnum, int ynum, double xdis, double ydis)
        {
            this.dp = new DiamondPoints(xnum, ynum, xdis, ydis);
        }

        public List<List<List<Point>>> getPanels()
        {
            var x =  utils.multiLevelMapList( pt => 
            {
                CSPoint xpt = pt as CSPoint;
                return Point.ByCoordinates(xpt.X, xpt.Y, xpt.Z);
            }, this.dp.AllPanels);
            List<List<List<Point>>> ret = x as List<List<List<Point>>>;
            return ret;
        }

        public DPoints TransformsPoints(int RX, int RY, double RateX, double RateY)
        {
            this.dp.TransformPoints(RX, RY, RateX, RateY);
            return this;
        }
    }
}