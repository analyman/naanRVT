using System;

namespace NaaN.dynamo.Geometry
{
    public class CSPoint
    {
        private double x, y, z;
        private CSPoint(){}

        public double X {get{return this.x;} set{this.x = value;}}
        public double Y {get{return this.y;} set{this.y = value;}}
        public double Z {get{return this.z;} set{this.z = value;}}

        public CSPoint(double X, double Y, double Z = 0)
        {
            this.x = X;
            this.y = Y;
            this.z = Z;
        }

        public bool Equals(CSPoint cspt)
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
            CSPoint casted = obj as CSPoint;
            if(null == casted){
                return false;
            }
            return this.Equals(casted);
        }

        public CSPoint operator+(CSPoint oth)
        {
            return new CSPoint(this.x + oth.x, this.y + oth.y, this.z + oth.z);
        }

        public CSPoint operator-(CSPoint oth)
        {
            return new CSPoint(this.x - oth.x, this.y - oth.y, this.z - oth.z);
        }
        
        public override int GetHashCode()
        {
            int xseed = (int)Math.Sin(this.x);
            int yseed = (int)Math.Cosh(this.y);
            int zseed = (int) this.z;
            return (xseed * yseed) / (zseed % 0x20);
        }

        public static CSPoint CSPointOrigin = new CSPoint(0, 0);
    }
}
