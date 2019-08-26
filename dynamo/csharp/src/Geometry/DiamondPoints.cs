using System.Collections.Generic;
using System.Linq;

namespace NaaN.dynamo.Geometry
{
    public class DiamondPoints
    {
        private int xnum, ynum;
        private double xdis, ydis;
        private List<List<CSPoint>> twoLayerPoints;
        private List<List<List<CSPoint>>> allPanels;

        public List<List<List<CSPoint>>> AllPanels { get { return this.allPanels; } }
        public List<List<CSPoint>> DiamondOuter { get { return this.allPanels[0]; } }
        public List<List<CSPoint>> DiamondInner { get { return this.allPanels[1]; } }
        public List<List<CSPoint>> TriangleA { get { return this.allPanels[2]; } }
        public List<List<CSPoint>> TriangleB { get { return this.allPanels[3]; } }
        public List<List<CSPoint>> TriangleC { get { return this.allPanels[4]; } }
        public List<List<CSPoint>> TriangleD { get { return this.allPanels[5]; } }

        public DiamondPoints(int xnum, int ynum, double xdis, double ydis)
        {
            this.xnum = xnum;
            this.ynum = ynum;
            this.xdis = xdis;
            this.ydis = ydis;
            this.twoLayerPoints = __genDiamondLayer_T1();
            this.allPanels = __DIAGET();
        }
        private DiamondPoints() { }

        public System.Collections.ArrayList DiamondInfo
        {
            get
            {
                return new System.Collections.ArrayList() { this.xnum, this.ynum, this.xdis, this.ydis };
            }
        }

        private List<List<CSPoint>> __DIA_genLayer(
            int xnum, int ynum,
            double xdis, double ydis, CSPoint offset = CSPoint.CSPointOrigin)
        {
            List<List<CSPoint>> ret = new List<List<CSPoint>>();
            foreach (int i in Enumerable.Range(0, xnum))
            {
                List<CSPoint> xlayer = new List<CSPoint>();
                foreach (int j in Enumerable.Range(0, ynum))
                {
                    xlayer.Append(new CSPoint(i * xdis, j * ydis) + offset);
                }
                ret.Append(xlayer);
            }
            return ret;
        }

        private __genDiamondLayer_T1()
        {
            halfWidth = this.xdis / 2;
            halfHeight = this.ydis / 2;
            List<List<CSPoint>> firstLayer = this.__DIA_genLayer(this.xnum, this.ynum, this.xdis, this.ydis);
            List<List<CSPoint>> secondLayer = this.__DIA_genLayer(this.xnum - 1, this.ynum - 1,
                                                                  this.xdis, this.ydis, new CSPoint(halfWidth, halfHeight));
            return new List<List<List<CSPoint>>>() { firstLayer, secondLayer };
        }

        private List<List<List<CSPoint>>> __DIA_getTriangles(List<List<List<CSPoint>>> TP)
        {
            List<List<CSPoint>> fl = TP[0];
            List<List<CSPoint>> sl = TP[1];
            List<List<CSPoint>> triangleA = new List<List<CSPoint>>();
            List<List<CSPoint>> triangleB = new List<List<CSPoint>>();
            List<List<CSPoint>> triangleC = new List<List<CSPoint>>();
            List<List<CSPoint>> triangleD = new List<List<CSPoint>>();
            foreach (int i in Enumerable.Range(0, this.xnum - 1))
            {
                triangleA.Append(new List<CSPoint>() { fl[i][0], fl[i + 1][0], sl[i][0] });
                triangleB.Append(new
                    List<CSPoint>() { fl[i][this.ynum - 1], fl[i + 1][this.ynum - 1], sl[i][this.ynum - 2] });
            }

            foreach (int i in Enumerable.Range(0, this.ynum - 1))
            {
                triangleB.Append(new
                    List<CSPoint>() { fl[this.ynum - 1][i], fl[this.ynum - 1][i + 1], sl[this.ynum - 2][i] });
                triangleA.Append(new List<CSPoint>() { fl[0][i], fl[0][i + 1], sl[0][i] });
            }
            return new List<List<List<CSPoint>>>() { triangleA, triangleB, triangleC, triangleD };
        }

        private List<List<List<CSPoint>>> __DIA_getDiamond(List<List<List<CSPoint>>> TP)
        {
            List<List<CSPoint>> fl = TP[0];
            List<List<CSPoint>> sl = TP[1];
            List<List<CSPoint>> diamondA = new List<List<CSPoint>>();
            List<List<CSPoint>> diamondB = new List<List<CSPoint>>();
            foreach (int i in Enumerable.Range(0, this.ynum - 1))
            {
                foreach (int j in Enumerable.Range(0, this.xnum - 1))
                {
                    diamondA.Append(new List<CSPoint>() { fl[j][i], sl[j - 1][i], fl[j][i + 1], sl[j][i] });
                }
            }
            foreach (int i in Enumerable.Range(0, this.xnum - 1))
            {
                foreach (int j in Enumerable.Range(0, this.ynum - 1))
                {
                    diamondB.Append(new List<CSPoint>() { fl[i][j], sl[i][j - 1], fl[i + 1][j], sl[i][j] });
                }
            }
            return new List<List<List<CSPoint>>>() { diamondA, diamondB };
        }

        private List<List<List<CSPoint>>> __DIAGET()
        {
            List<List<List<CSPoint>>> D = this.__DIA_getDiamond(this.twoLayerPoints);
            List<List<List<CSPoint>>> T = this.__DIA_getTriangles(this.twoLayerPoints);
            return D.Join(T);
        }

        public override bool Equals(object obj) {return false;}
        public bool Equals(DiamondPoints obj) {return false;}
    }
}