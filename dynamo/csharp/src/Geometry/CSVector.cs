using System;

namespace NaaN.dynamo.Geometry
{
    public class CSVector
    {
        private double x, y, z;
        private CSVector(){}

        public double X {get{return this.x;} set{this.x = value;}}
        public double Y {get{return this.y;} set{this.y = value;}}
        public double Z {get{return this.z;} set{this.z = value;}}

        public CSVector(double X, double Y, double Z = 0)
        {
            this.x = X;
            this.y = Y;
            this.z = Z;
        }

        private void swapxy()
        {
            double temp = this.x;
            this.x = this.y;
            this.y = temp;
            return;
        }

        public bool Equals(CSVector cspt)
        {
            if(null == cspt){
                return false;
            }
            if(cspt.x == this.x && cspt.y == this.y && cspt.z == this.z){
                return true;
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            CSVector casted = obj as CSVector;
            if(null == casted){
                return false;
            }
            return this.Equals(casted);
        }

        public static CSVector operator+(CSVector fir, CSVector sec)
        {
            return new CSVector(fir.x + sec.x, fir.y + sec.y, fir.z + sec.z);
        }

        public static CSVector operator-(CSVector fir, CSVector sec)
        {
            return new CSVector(fir.x - sec.x, fir.y - sec.y, fir.z - sec.z);
        }
        
        public override int GetHashCode()
        {
            int xseed = (int)Math.Sin(this.x);
            int yseed = (int)Math.Cosh(this.y);
            int zseed = (int) this.z;
            return (xseed * yseed) / (zseed % 0x22);
        }

        public override string ToString()
        {
            return "CSVector(X = " + this.x.ToString() + ", Y = " +
                        this.y.ToString() + ", Z = " + this.z.ToString() + ")";
        }

        public static CSVector CSVectorZero = new CSVector(0, 0);
    }
}