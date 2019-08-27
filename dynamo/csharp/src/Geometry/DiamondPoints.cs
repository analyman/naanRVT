using System.Collections.Generic;
using System.Linq;
using NaaN;

namespace NaaN.dynamo.Geometry
{
    public class DiamondPoints
    {
        private int xnum, ynum;
        private double xdis, ydis;
        private List<List<List<CSPoint>>> twoLayerPoints;
        private List<List<List<CSPoint>>> allPanels;

        public List<List<List<CSPoint>>> AllPanels { get { return this.allPanels; } }
        public List<List<CSPoint>> DiamondY { get { return this.allPanels[0]; } }
        public List<List<CSPoint>> DiamondX { get { return this.allPanels[1]; } }
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
            double xdis, double ydis, CSPoint offset)
        {
            List<List<CSPoint>> ret = new List<List<CSPoint>>();
            foreach (int i in Enumerable.Range(0, xnum))
            {
                List<CSPoint> xlayer = new List<CSPoint>();
                foreach (int j in Enumerable.Range(0, ynum))
                {
                    xlayer.Add(new CSPoint(i * xdis, j * ydis) + offset);
                }
                ret.Add(xlayer);
            }
            return ret;
        }

        private List<List<List<CSPoint>>> __genDiamondLayer_T1()
        {
            double halfWidth = this.xdis / 2;
            double halfHeight = this.ydis / 2;
            List<List<CSPoint>> firstLayer = this.__DIA_genLayer(this.xnum, this.ynum, this.xdis, this.ydis, CSPoint.CSPointOrigin);
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
                triangleA.Add(new List<CSPoint>() { fl[i][0], fl[i + 1][0], sl[i][0] });
                triangleB.Add(new
                    List<CSPoint>() { fl[i][this.ynum - 1], fl[i + 1][this.ynum - 1], sl[i][this.ynum - 2] });
            }

            foreach (int i in Enumerable.Range(0, this.ynum - 1))
            {
                triangleC.Add(new
                    List<CSPoint>() { fl[this.xnum - 1][i], fl[this.xnum - 1][i + 1], sl[this.xnum - 2][i] });
                triangleD.Add(new List<CSPoint>() { fl[0][i], fl[0][i + 1], sl[0][i] });
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
                foreach (int j in Enumerable.Range(1, this.xnum - 2))
                {
                    diamondA.Add(new List<CSPoint>() { fl[j][i], sl[j - 1][i], fl[j][i + 1], sl[j][i] });
                }
            }
            foreach (int i in Enumerable.Range(0, this.xnum - 1))
            {
                foreach (int j in Enumerable.Range(1, this.ynum - 2))
                {
                    diamondB.Add(new List<CSPoint>() { fl[i][j], sl[i][j - 1], fl[i + 1][j], sl[i][j] });
                }
            }
            return new List<List<List<CSPoint>>>() { diamondA, diamondB };
        }

        private List<List<List<CSPoint>>> __DIAGET()
        {
            List<List<List<CSPoint>>> D = this.__DIA_getDiamond(this.twoLayerPoints);
            List<List<List<CSPoint>>> T = this.__DIA_getTriangles(this.twoLayerPoints);
            return new List<List<List<CSPoint>>>(Enumerable.Concat(D, T));
        }

        public void TransformPoints(double RX, double RY, double RateX = 0.3, double RateY = 0.3)
        {
            if (RateY < 0 || RateX < 0 || RateY > 1 || RateX > 1)
            {
                throw new System.ArgumentException("RateX and RateY must in interval [0, 1]");
            }
            utils.multiLevelMap(
                x =>
                {
                    CSPoint cxpt = x as CSPoint;
                    cxpt.TransformX(RX, RY,
                    this.xnum * this.xdis * RateX,
                    this.ynum * this.ydis * RateY);
                    return 0;
                }, this.twoLayerPoints);
            return;
        }

        public override bool Equals(object obj) { return false; }
        public bool Equals(DiamondPoints obj) { return false; }
        public override int GetHashCode()
        {
            return (ynum % (xnum % 0x20)) * (int)(xdis / ydis);
        }

        public override string ToString()
        {
            return allPanels.ToString();
        }
    }
}